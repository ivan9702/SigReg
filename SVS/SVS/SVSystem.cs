using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Signature
{
    class SVSystem
    {
        //-- Similarity score
        public static double[] Similarity = new double[FeatureNum];

        //-- File name
        public string FileName;

        //-- 該樣板簽名的安全分數範圍
        public double ScoreLow = 0;
        public double ScoreHigh = 0;
        public double CostTime = 0;

        //-- Original
        public List<double> X_Array = new List<double>();
        public List<double> Y_Array = new List<double>();
        public List<double> P_Array = new List<double>();
        public List<double> T_Array = new List<double>();

        //-- Noise removal
        private List<double> X_Array_t = new List<double>();
        private List<double> Y_Array_t = new List<double>();
        private List<double> P_Array_t = new List<double>();
        private List<double> T_Array_t = new List<double>();

        //-- Feature
        public static int FeatureNum = 8;
        public List<double> Feature_X = new List<double>();
        public List<double> Feature_Y = new List<double>();
        public List<double> Feature_P = new List<double>();
        public List<double> Feature_Vx = new List<double>();
        public List<double> Feature_Vy = new List<double>();
        public List<double> Feature_Ax = new List<double>();
        public List<double> Feature_Ay = new List<double>();
        public List<double> Feature_A = new List<double>();
        public List<double> Feature_R = new List<double>();

        //-- Feature std
        public double X_std = 0;
        public double Y_std = 0;
        public double P_std = 0;
        public double Vx_std = 0;
        public double Vy_std = 0;
        public double Ax_std = 0;
        public double Ay_std = 0;
        public double A_std = 0;
        public double R_std = 0;

        

        //-- Feature length
        int Space_Divide = 128;    
        int SpacedTime_Divide = 128;
        //int Time_Divide = 128;

        // *************************************************************************************************
        // @fn          SVS_Reset
        // @brief       
        // @return      
        // *************************************************************************************************
        public void SVS_Reset()
        {
            X_Array.Clear();
            Y_Array.Clear();
            P_Array.Clear();
            T_Array.Clear();

            X_Array_t.Clear();
            Y_Array_t.Clear();
            P_Array_t.Clear();
            T_Array_t.Clear();

            Feature_X.Clear();
            Feature_Y.Clear();
            Feature_P.Clear();
            Feature_Vx.Clear();
            Feature_Vy.Clear();
            Feature_A.Clear();
            Feature_R.Clear();
        }

        // *************************************************************************************************
        // @fn          CheckFeature
        // @brief       
        // @return      
        // *************************************************************************************************
        public bool CheckFeature()
        {
            for (int i = 0; i < Feature_X.Count; i++)
                if (Double.IsNaN(Feature_X[i]) || Double.IsInfinity(Feature_X[i]))
                    return true;

            for (int i = 0; i < Feature_Y.Count; i++)
                if (Double.IsNaN(Feature_Y[i]) || Double.IsInfinity(Feature_Y[i]))
                    return true;

            for (int i = 0; i < Feature_P.Count; i++)
                if (Double.IsNaN(Feature_P[i]) || Double.IsInfinity(Feature_P[i]))
                    return true;

            for (int i = 0; i < Feature_Vx.Count; i++)
                if (Double.IsNaN(Feature_Vx[i]) || Double.IsInfinity(Feature_Vx[i]))
                    return true;

            for (int i = 0; i < Feature_Vy.Count; i++)
                if (Double.IsNaN(Feature_Vy[i]) || Double.IsInfinity(Feature_Vy[i]))
                    return true;

            for (int i = 0; i < Feature_A.Count; i++)
                if (Double.IsNaN(Feature_A[i]) || Double.IsInfinity(Feature_A[i]))
                    return true;

            for (int i = 0; i < Feature_R.Count; i++)
                if (Double.IsNaN(Feature_R[i]) || Double.IsInfinity(Feature_R[i]))
                    return true;

            return false;
        }

        // *************************************************************************************************
        // @fn          Recognizer
        // @brief       
        // @return      
        // *************************************************************************************************
        public static double Recognizer (SVSystem Template, SVSystem Tester)
        {
            double Score = 0;
            double[] D = new double[FeatureNum];

            //-- Feature std
            double X_std_T = CalculateFeatureStd(Template.Feature_X);
            double Y_std_T = CalculateFeatureStd(Template.Feature_Y);
            double P_std_T = CalculateFeatureStd(Template.Feature_P);
            double Vx_std_T = CalculateFeatureStd(Template.Feature_Vx);
            double Vy_std_T = CalculateFeatureStd(Template.Feature_Vy);
            double Ax_std_T = CalculateFeatureStd(Template.Feature_Ax);
            double Ay_std_T = CalculateFeatureStd(Template.Feature_Ay);
            double A_std_T = CalculateFeatureStd(Template.Feature_A);
            double R_std_T = CalculateFeatureStd(Template.Feature_R);

            //-- 辨識器        
            D[0] = ELCSS(Template.Feature_X, Tester.Feature_X, Math.Abs(X_std_T) * 0.65, 5);
            D[1] = ELCSS(Template.Feature_Y, Tester.Feature_Y, Math.Abs(Y_std_T) * 0.5, 5);
            D[2] = ELCSS(Template.Feature_P, Tester.Feature_P, Math.Abs(P_std_T) * 0.9, 5);
            D[3] = ELCSS(Template.Feature_Vx, Tester.Feature_Vx, Math.Abs(Vx_std_T) * 0.55, 5);
            D[4] = ELCSS(Template.Feature_Vy, Tester.Feature_Vy, Math.Abs(Vy_std_T) * 0.65, 5); // 0.4
            D[5] = ELCSS(Template.Feature_Ax, Tester.Feature_Ax, Math.Abs(Ax_std_T) * 0.95, 5); // 0.9
            D[6] = ELCSS(Template.Feature_Ay, Tester.Feature_Ay, Math.Abs(Ay_std_T) * 0.6, 5); // 0.5
            D[7] = ELCSS(Template.Feature_A, Tester.Feature_A, Math.Abs(A_std_T) * 0.65, 5);

            Similarity = D;

            Score = D.Sum() * FeatureNum * 100 / Math.Pow(SVSystem.FeatureNum, 2); ;

            return Score;
        }

        // *************************************************************************************************
        // @fn          SignatureDataProcessing
        // @brief       
        // @return      
        // *************************************************************************************************
        public void SignatureDataProcessing(int DataLength)
        {
            //-- 訊號前處理
            Preprocessing(DataLength);

            //-- 紀錄
            CostTime = T_Array_t[T_Array_t.Count - 1];

            //-- 特徵擷取
            FeatureExtraction();
        }

        // *************************************************************************************************
        // @fn          DataDecoder
        // @brief       
        // @return      
        // *************************************************************************************************
        public int DataDecoder(StreamReader srData)
        {
            int DataCount = 0;
            int DataLength = 0;
            double DataFirstTime = 0;

            //-- 讀取全部檔案
            string strTextData = String.Empty;
            strTextData = srData.ReadToEnd();
            //-- 第一次分割
            string[] strFirstSplit = strTextData.Split(new Char[] { '\n' });

            //-- 簽名原始資料-初始化陣列長度
            DataLength = strFirstSplit.Length - 1;

            foreach (string s in strFirstSplit)
            {
                string[] strSecondSplit = s.Split(new Char[] { ',' });

                //-- 第二次分割
                foreach (string s2 in strSecondSplit)
                {
                    string strData = String.Empty;
                    int start = 0;
                    int end = 0;

                    s2.Trim();
                    start = s2.IndexOf("=") + 1;
                    end = s2.Length - 1 - start;
                    strData = s2.Substring(start);

                    if (s2.Contains('X') == true)
                    {
                        //-- X axis
                        X_Array.Add(int.Parse(strData));
                    }
                    else if (s2.Contains('Y') == true)
                    {
                        //-- Y axis
                        Y_Array.Add(int.Parse(strData));
                    }
                    else if (s2.Contains("Pressure") == true)
                    {
                        //-- Pressure
                        P_Array.Add(int.Parse(strData));
                    }
                    else if (s2.Contains("Time") == true)
                    {
                        //-- 切割分、秒與萬分之一秒                            
                        string[] strTimeData = strData.Split(new Char[] { '-' });
                        string[] strTimeData2 = strTimeData[5].Split(new Char[] { ':' });
                        double Minute = double.Parse(strTimeData[4]);
                        double Second = double.Parse(strTimeData2[0]);
                        double Second2 = double.Parse(strTimeData2[1]);

                        //-- 計算第一筆訊號時間，用來計算整體簽名時間
                        if (DataCount == 0)
                        {
                            DataFirstTime = Minute * 60 + Second + (Second2 / 10000);
                            T_Array.Add(0);
                        }
                        else
                            T_Array.Add(Minute * 60 + Second + (Second2 / 10000) - DataFirstTime);
                    }

                }
                DataCount++;
            }

            return DataLength;
        }

        // *************************************************************************************************
        // @fn          SignatureTemplateSelection
        // @brief       
        // @return      
        // *************************************************************************************************
        public static double[] SignatureTemplateSelection(SVSystem[] Database, ref SVSystem TemplateData, ref List<int> BadSig)
        {
            //-- 安全分數設定 - 隨機仿冒者分數落在60-70分居多  
            //int SerurityScore_Norm = 80;
            int SerurityScore_Min = 70;

            //-- Each person entroll fixed signatures to choose template
            int Entrollment = Database.Length;

            //-- BestTemplate
            double[] BestTemplate = new double[10 + 2 * Entrollment];

            //-- Training data 
            int TrainData_startPos = 0;       

            //-- Record all signature inner score
            double[,] Score_All = new double[Entrollment, Entrollment-1];

            //-- Final score to select which best
            double[] TemplateScore = new double[Entrollment];
            double[] TemplateScoreMean = new double[Entrollment];
            double[] TemplateScoreStd = new double[Entrollment];

            for (int i = TrainData_startPos; i < (TrainData_startPos + Entrollment); i++)
            {
                int cnt = 0;
                for (int j = TrainData_startPos; j < (TrainData_startPos + Entrollment); j++)
                {
                    if (i == j)
                        continue;

                    double Score_t = 0;
                    double[] D = new double[FeatureNum];

                    Score_t = Recognizer(Database[i], Database[j]);

                    Score_All[i, cnt++] = Score_t;
                }
            }

            for (int i = TrainData_startPos; i < (TrainData_startPos + Entrollment); i++)
            {
                //-- 取平均值
                double mean = 0;                
                for(int j = 0; j < (Entrollment - 1); j++)
                {
                    mean += Score_All[i, j];
                }
                mean = mean / (Entrollment - 1);

                //-- 計算平方和
                double sum = 0;
                for (int k = 0; k < (Entrollment - 1); k++)
                {
                    sum += Math.Pow(Score_All[i, k] - mean, 2);
                }

                //-- 計算標準差
                double std = 0;
                std = Math.Sqrt(sum / (Entrollment - 2));

                TemplateScore[i] = mean - std;
                TemplateScoreMean[i] = mean;
                TemplateScoreStd[i] = std;

                BestTemplate[2*i + 10] = mean;
                BestTemplate[2*i+1 + 10] = std;
            }

            //-- 選擇最大樣板分數，與最大標準差
            double ScoreMax = 0, StdMax = 0;
            ScoreMax = TemplateScore.Max();
            StdMax = TemplateScoreStd.Max();

            for (int i = 0; i < Entrollment; i++)
            {
                if(TemplateScore[i] == ScoreMax)
                {
                    BestTemplate[0] = i+1;
                    BestTemplate[1] = ScoreMax;
                    BestTemplate[2] = TemplateScoreMean[i];
                    BestTemplate[3] = TemplateScoreStd[i];
                    BestTemplate[4] = TemplateScoreMean[i] + 2 * TemplateScoreStd[i];
                    BestTemplate[5] = TemplateScoreMean[i] - 2 * TemplateScoreStd[i];
                    //BestTemplate[4] = TemplateScoreMean[i] + StdMax;
                    //BestTemplate[5] = TemplateScoreMean[i] - StdMax;
                    BestTemplate[6] = Database[i].CostTime;
                    break;
                }
            }

            //-- 將相似度分數轉換至0-100分範圍
            for (int i = 1; i < BestTemplate.Length; i++)
            {
                //-- 書寫時間跳過
                if (i == 6) continue;

                BestTemplate[i] = BestTemplate[i] * 100 / Math.Pow(SVSystem.FeatureNum, 2);
                //-- 小於 最低安全分數者 分當作絕對仿冒者 or 不好的簽名
                if (BestTemplate[i] < SerurityScore_Min && i >= 10 && (i % 2 == 0))
                {
                    BestTemplate[i] = 0;
                    BestTemplate[i + 1] = 0;
                }
                //else if (BestTemplate[i] > SerurityScore_Min && BestTemplate[i] < SerurityScore_Norm && i >= 10 && (i % 2 == 0))
                //{
                //    BestTemplate[i] = ((BestTemplate[i] - SerurityScore_Min) / (SerurityScore_Norm - SerurityScore_Min)) * (SerurityScore_Norm - 50) + 50;
                //}
            }

            double Upper = 0;
            double lower = 0;
            double Mean = 0;
            double Std = 0;
            Mean = BestTemplate[2];
            Std = BestTemplate[3];
            Upper = Mean + Std;
            lower = Mean - Std;

            //-- 樣板標準判斷
            if (Std >= 3.5)
            {
                bool BigBadSig_flag = false;

                for (int i = 0; i < Entrollment; i++)
                {
                    if (i == (BestTemplate[0] - 1))
                        continue;

                    if (Math.Abs(BestTemplate[2 * i + 10] - Mean) > 5)
                    {
                        BadSig.Add(i + 1);
                        BigBadSig_flag = true;
                    }
                }

                if(BigBadSig_flag == false)
                {
                    for (int i = 0; i < Entrollment; i++)
                    {
                        if (i == (BestTemplate[0] - 1))
                            continue;


                        //-- 設定標準差小於閥值建議重新簽名
                        if (Math.Abs(BestTemplate[2 * i + 1 + 10]) > 5)
                        {
                            BadSig.Add(i + 1);
                        }

                        //-- 設定小於最佳樣版的最低分數者且該標準差大於閥值，建議重新簽名
                        //if ((BestTemplate[2 * i + 10] - BestTemplate[2 * i + 1 + 10]) < lower && BestTemplate[2 * i + 1 + 10] >= 3)
                        //{
                        //    BadSig.Add(i + 1);
                        //}
                    }
                }
                
            }

            //-- 取得最新樣板並回傳
            TemplateData = Database[ (int) BestTemplate[0]-1];

            return BestTemplate;
        }
        //--------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------//
        // *************************************************************************************************
        // @fn          ELCSS
        // @brief       
        // @return      
        // *************************************************************************************************
        private static double ELCSS(List<double> X, List<double> Y, double errorThr, short EdgeThr)
        {
            int i, j;
            int Max_len;
            double D;

            //-- size = (n+1) x (m+1)
            double[] L = new double[16641];

            double TT = X.Count;
            double YY = X.Count;
            for (i = 0; i < X.Count; i++)
            {
                for (j = 0; j < Y.Count; j++)
                {
                    // Up and left
                    if (Math.Abs(X[i] - Y[j]) <= errorThr && Math.Abs((2 + i) - (2 + j)) <= EdgeThr)
                    {
                        L[(i + X.Count * (j + 1)) + 1] = L[i + X.Count * j] + 1;
                    }
                    else
                    {
                        L[(i + X.Count * (j + 1)) + 1] = L[i + X.Count * j];
                    }

                    // Up
                    if (L[(i + X.Count * (j + 1))] >= L[(i + X.Count * (j + 1)) + 1])
                    {
                        L[(i + X.Count * (j + 1)) + 1] = L[(i + X.Count * (j + 1))];
                    }

                    // Left
                    if (L[(i + X.Count * j) + 1] >= L[(i + X.Count * (j + 1)) + 1])
                    {
                        L[(i + X.Count * (j + 1)) + 1] = L[(i + X.Count * j) + 1];
                    }

                }
            }

            if (Y.Count <= X.Count)
            {
                Max_len = Y.Count;
            }
            else
            {
                Max_len = X.Count;
            }

            D = L[X.Count + X.Count*Y.Count] / Max_len;

            return D;
        }

        // *************************************************************************************************
        // @fn          LinearInterpolation
        // @brief       
        // @return      
        // *************************************************************************************************
        private double[] LinearInterpolation(int InterpSize, List<double> Data)
        {
            //-- Also called "Feature_spaceNorm"
            double[] Result = new double[InterpSize];
            short[] Inter = new short[InterpSize];
            double cnt = 0;

            double Interval = Data.Count / (InterpSize - 1);
            //Console.WriteLine(Interval);          

            //-- 對資料做線性內插，取出相對應分段之時間

            for (int i = 0; i < InterpSize; i++)
            {
                //Console.WriteLine(Math.Round(cnt));
                if ((int)Math.Floor(cnt) + 1 < Data.Count - 1)
                {
                    Result[i] = Data[(int)Math.Floor(cnt)] + (cnt - Math.Floor(cnt)) * (Data[(int)Math.Floor(cnt) + 1] - Data[(int)Math.Floor(cnt)]);
                }
                else
                {
                    Result[i] = Data[Data.Count - 1];
                }

                cnt += Interval;
            }

            return Result;
        }

        // *************************************************************************************************
        // @fn          scaleData
        // @brief       Size normalization
        // @return      
        // *************************************************************************************************
        private double[] scaleData(List<double> X, double fmin, double fmax)
        {
            double X_min = X.Min();
            double X_max = X.Max();
            double X_std = 0;
            double[] Norm = new double[X.Count];

            for (int i = 0; i < X.Count; i++)
            {
                X_std = (X[i] - X_min) / (X_max - X_min);
                Norm[i] = X_std * (fmax - fmin) + fmin;
            }
            return Norm;
        }

        // *************************************************************************************************
        // @fn          Space_Normalization
        // @brief       
        // @return      
        // *************************************************************************************************
        private double[] Space_Normalization(List<double> X, int Size)
        {
            double[] Norm = new double[Size];

            //-- Space normalization
            //-- 對訊號做內插，取出相對應的點
            Norm = LinearInterpolation(Size, X);

            //-- Check index has no repeated time points
            for (int i = 1; i < Norm.Length - 1; i++)
            {
                if (Norm[i] == Norm[i - 1] || Norm[i] == Norm[i + 1])
                {
                    Norm[i] = (Norm[i - 1] + Norm[i] + Norm[i + 1]) / 3;
                }
            }

            return Norm;
        }

        // *************************************************************************************************
        // @fn          Time_Normalization
        // @brief       
        // @return      
        // *************************************************************************************************
        private double[] Time_Normalization(List<double> T, List<double> X, int Size)
        {
            bool CheckRepeat_flag = true;
            int RepeatMeanCnt = 1;

            double[] Norm = new double[Size];
            double[] SpacedTime_temp = new double[Size + 1];
            double[] SpacedTime = new double[Size];

            List<int> SpacedTime_Index = new List<int>();
            int DataCount = 0;

            //-- Mean normalization
            //-- 對時間做內插，取出相對分段的時間
            //-- Size + 1 是為了捨棄第一筆資料為0的值           
            SpacedTime_temp = LinearInterpolation(Size + 1, T);

            for (int i = 0; i < Size; i++)
            {
                SpacedTime[i] = SpacedTime_temp[i + 1];
            }

            //-- 取出時間相對應位置
            foreach (double t in SpacedTime)
            {

                for (int i = DataCount; i < T.Count; i++)
                {
                    if (T[i] >= t)
                    {
                        SpacedTime_Index.Add(i);
                        DataCount = i;
                        break;
                    }
                }
            }

            //-- Check index has no repeated time points
            while (CheckRepeat_flag)
            {

                CheckRepeat_flag = false;

                for (int i = 1; i < SpacedTime_Index.Count - 1; i++)
                {
                    double TempSum = 0;

                    if (SpacedTime_Index[i] == SpacedTime_Index[i - 1] || SpacedTime_Index[i] == SpacedTime_Index[i + 1])
                    {
                        for (int j = i - RepeatMeanCnt; j <= i + RepeatMeanCnt; j++)
                        {
                            if (j >= SpacedTime_Index.Count - 1)
                                j = SpacedTime_Index.Count - 1;
                            if (j < 1)
                                j = 1;

                            TempSum += SpacedTime_Index[j];
                        }
                        TempSum = TempSum / (1 + 2 * RepeatMeanCnt);
                        SpacedTime_Index[i] = (int)Math.Round(TempSum);
                    }
                }
                if (SpacedTime_Index[SpacedTime_Index.Count - 1] == 127 || SpacedTime_Index[SpacedTime_Index.Count - 1] == 128)  //2017.02.15 by zack, 因為最多就是127
                {
                    for (int i = 0; i < SpacedTime_Index.Count; i++)
                    {
                        SpacedTime_Index[i] = i + 1;
                    }
                }
                //-- 最後檢查是否有重複點，如果有，繼續補償
                for (int i = 1; i < SpacedTime_Index.Count - 1; i++)
                {
                    if (SpacedTime_Index[i] == SpacedTime_Index[i - 1])
                    {
                        CheckRepeat_flag = true;
                        RepeatMeanCnt++;
                    }

                }
            }

            //-- 對應取值
            for (int i = 0; i < Size; i++)
            {
                Norm[i] = X[SpacedTime_Index[i]];
            }

            //-- Check index has no repeated time points
            for (int i = 1; i < Norm.Length - 1; i++)
            {
                if (Norm[i] == Norm[i - 1] || Norm[i] == Norm[i + 1])
                {
                    Norm[i] = (Norm[i - 1] + Norm[i] + Norm[i + 1]) / 3;
                }
            }

            return Norm;
        }

        // *************************************************************************************************
        // @fn          Mean_Normalization
        // @brief       
        // @return      
        // *************************************************************************************************
        private double[] Mean_Normalization(List<double> T, List<double> X, int Size)
        {
            bool CheckRepeat_flag = true;
            int RepeatMeanCnt = 1;

            double[] Norm = new double[Size];
            double[] SpacedTime_temp = new double[Size + 1];
            double[] SpacedTime = new double[Size];

            List<int> SpacedTime_Index = new List<int>();
            int DataCount = 0;

            //-- Mean normalization
            //-- 對時間做內插，取出相對分段的時間
            //-- Size + 1 是為了捨棄第一筆資料為0的值           
            SpacedTime_temp = LinearInterpolation(Size + 1, T);

            for (int i = 0; i < Size; i++)
            {
                SpacedTime[i] = SpacedTime_temp[i + 1];
            }


            //-- 取出時間相對應位置
            foreach (double t in SpacedTime)
            {

                for (int i = DataCount; i < T.Count; i++)
                {
                    if (T[i] >= t)
                    {
                        SpacedTime_Index.Add(i);
                        DataCount = i;
                        break;
                    }
                }
            }

            //-- Check index has no repeated time points
            while (CheckRepeat_flag)
            {

                CheckRepeat_flag = false;

                for (int i = 1; i < SpacedTime_Index.Count - 1; i++)
                {
                    double TempSum = 0;

                    if (SpacedTime_Index[i] == SpacedTime_Index[i - 1] || SpacedTime_Index[i] == SpacedTime_Index[i + 1])
                    {
                        for (int j = i - RepeatMeanCnt; j <= i + RepeatMeanCnt; j++)
                        {
                            if (j >= SpacedTime_Index.Count - 1)
                            {
                                j = SpacedTime_Index.Count - 1;  
                            }
                            if (j < 1)
                                j = 1;

                            TempSum += SpacedTime_Index[j];
                        }
                        TempSum = TempSum / (1 + 2 * RepeatMeanCnt);
                        SpacedTime_Index[i] = (int)Math.Round(TempSum);
                    }
                }
                if(SpacedTime_Index[SpacedTime_Index.Count - 1] == 127 || SpacedTime_Index[SpacedTime_Index.Count - 1] == 128)  //2017.02.15 by zack, 因為最多就是127
                {
                    for(int i = 0; i < SpacedTime_Index.Count; i++)
                    {
                        SpacedTime_Index[i] = i + 1;
                    }
                }

                //-- 最後檢查是否有重複點，如果有，繼續補償
                for (int i = 1; i < SpacedTime_Index.Count - 1; i++)
                {
                    if (SpacedTime_Index[i] == SpacedTime_Index[i - 1])
                    {
                        CheckRepeat_flag = true;
                        RepeatMeanCnt++;
                    }

                }
            }


            //-- 相加取平均
            for (int i = 0; i < Size; i++)
            {
                double Temp = 0;
                double Len = 0;

                if (i == 0)
                {
                    //-- 相加
                    for (int j = 0; j <= SpacedTime_Index[i]; j++)
                    {
                        Temp += X[j];
                    }

                    Len = SpacedTime_Index[i] + 1;

                    //-- 平均
                    Norm[i] = Temp / Len;
                }
                else if (i > 0)
                {
                    if (SpacedTime_Index[i] != SpacedTime_Index[i - 1])
                    {
                        //-- 相加
                        for (int j = SpacedTime_Index[i - 1] + 1; j <= SpacedTime_Index[i]; j++)
                        {
                            Temp += X[j];
                        }

                        Len = SpacedTime_Index[i] - SpacedTime_Index[i - 1] + 1;

                        //-- 平均
                        Norm[i] = Temp / Len;
                        if (Len == 0)                                       //2017.02.15 by zack
                            Norm[i] = 0;
                    }
                    else
                        Norm[i] = X[i];
                }
            }

            //-- Check index has no repeated time points
            for (int i = 1; i < Norm.Length - 1; i++)
            {
                if (Norm[i] == Norm[i - 1] || Norm[i] == Norm[i + 1])
                {
                    Norm[i] = (Norm[i - 1] + Norm[i] + Norm[i + 1]) / 3;
                }
            }

            return Norm;
        }

        // *************************************************************************************************
        // @fn          Zero_Normalization
        // @brief       
        // @return      
        // *************************************************************************************************
        private double[] Zero_Normalization(List<double> X, int Size)
        {
            double[] Norm = new double[Size];      

            //-- Zero normalization
            double std = 0;
            double mean = 0;
            double sum = 0;

            // 計算平均值
            for (int i = 0; i < Size; i++)
            {
                sum = sum + X[i];
            }
            mean = sum / Size;

            //-- 計算平方和
            sum = 0;
            for (int i = 0; i < Size; i++)
            {
                sum += Math.Pow(X[i] - mean, 2);
            }

            //-- 計算標準差
            std = Math.Sqrt(sum / (Size - 1));
            

            //-- 計算 Z score
            if (std != 0)
            {
                for (int i = 0; i < Size; i++)
                {
                    Norm[i] = (X[i] - mean) / std;
                }
            }
  

            return Norm;
        }

        // *************************************************************************************************
        // @fn          regression
        // @brief       
        // @return      
        // *************************************************************************************************
        private double[] regression(List<double> X, int order)
        {
            int loopStart = order;
            int loopEnd = X.Count - order - 1;
            double[] reg = new double[loopEnd - loopStart + 1];
            int DataCount = 0;

            int Denominator = 0;
            for (int i = 1; i <= order; i++)
            {
                Denominator = Denominator + (int)Math.Pow((double)i, 2);
            }
            Denominator = 2 * Denominator;

            for (int i = loopStart; i <= loopEnd; i++)
            {
                double temp = 0;
                for (int j = 1; j <= order; j++)
                {
                    temp += j * (X[i + j] - X[i - j]);
                }
                reg[DataCount] = temp / Denominator;
                DataCount++;
            }

            return reg;
        }

        // *************************************************************************************************
        // @fn          CalculateFeatureStd
        // @brief       
        // @return      
        // *************************************************************************************************
        private static double CalculateFeatureStd(List<double> X)
        {
            double std = 0;
            double mean = 0;
            double sum = 0;

            // 計算平均值
            for (int i = 0; i < X.Count; i++)
            {
                sum = sum + X[i];
            }
            mean = sum / X.Count;

            //-- 計算平方和
            sum = 0;
            for (int i = 0; i < X.Count; i++)
            {
                sum += Math.Pow(X[i] - mean, 2);
            }

            //-- 計算標準差
            std = Math.Sqrt(sum / (X.Count - 1));

            return std;
        }

        // *************************************************************************************************
        // @fn          Preprocessing
        // @brief       
        // @return      
        // *************************************************************************************************
        private void Preprocessing(int DataLength)
        {
            if (DataLength > 2)
            {
                //-- 座標軸校正
                if (X_Array[0] - X_Array[X_Array.Count - 2] > 0)
                {
                    for (int i = 0; i < DataLength; i++)
                    {
                        X_Array[i] = -1 * X_Array[i];// + X_Array.Max();
                    }
                }
                else
                {
                    for (int i = 0; i < DataLength; i++)
                    {
                        Y_Array[i] = -1 * Y_Array[i];// + Y_Array.Max();
                    }
                }

                //-- 限定第一象限並最小值從0開始           
                //double X_min = X_Array.Min();
                //double Y_min = Y_Array.Min();
                //for (int i = 0; i < DataLength; i++)
                //{
                //    X_Array[i] -= X_min;
                //    Y_Array[i] -= Y_min;
                //}

                //-- 雜訊濾除
                //-- 坐標軸連續一樣濾除
                List<int> Noise = new List<int>();
                for (int i = 0; i < DataLength-1; i++)
                {
                    if (X_Array[i] == X_Array[i + 1] && Y_Array[i] == Y_Array[i + 1])
                    {
                        Noise.Add(i);
                    }
                }

                int Noise_cnt = 1;
                if (Noise.Count > 1)
                {
                    for (int i = 0; i < DataLength; i++)
                    {
                        if (i != Noise[Noise_cnt])
                        {
                            X_Array_t.Add(X_Array[i]);
                            Y_Array_t.Add(Y_Array[i]);
                            P_Array_t.Add(P_Array[i]);
                            T_Array_t.Add(T_Array[i]);
                        }
                        else
                        {
                            if (Noise_cnt < Noise.Count - 1)
                                Noise_cnt++;
                        }
                    }
                }
                else
                {
                    X_Array_t = X_Array;
                    Y_Array_t = Y_Array;
                    P_Array_t = P_Array;
                    T_Array_t = T_Array;
                }
            }
        }

        // *************************************************************************************************
        // @fn          FeatureExtraction
        // @brief       
        // @return      
        // *************************************************************************************************
        private void FeatureExtraction()
        {
            //-- Mean norm 
            List<double> X_MeanNorm = new List<double>();
            List<double> Y_MeanNorm = new List<double>();
            List<double> P_MeanNorm = new List<double>();
            List<double> T_MeanNorm = new List<double>();

            List<double> V_Value = new List<double>();
            List<double> Vx_Value = new List<double>();
            List<double> Vy_Value = new List<double>();

            List<double> A_Value = new List<double>();
            List<double> Ax_Value = new List<double>();
            List<double> Ay_Value = new List<double>();

            List<double> X1_Value = new List<double>();
            List<double> Y1_Value = new List<double>();
       
            double V = 0;
            double Vx = 0;
            double Vy = 0;

            double A = 0;
            double Ax = 0;
            double Ay = 0;      

            //-- 大小範圍落在0~1
            X_Array_t = scaleData(X_Array_t, 0, 1).ToList();
            Y_Array_t = scaleData(Y_Array_t, 0, 1).ToList();
            P_Array_t = scaleData(P_Array_t, 0, 1).ToList();

            //-- Mean Normalization
            X_MeanNorm = Mean_Normalization(T_Array_t, X_Array_t, SpacedTime_Divide).ToList();
            Y_MeanNorm = Mean_Normalization(T_Array_t, Y_Array_t, SpacedTime_Divide).ToList();
            P_MeanNorm = Mean_Normalization(T_Array_t, P_Array_t, SpacedTime_Divide).ToList();
            T_MeanNorm = Time_Normalization(T_Array_t, T_Array_t, SpacedTime_Divide).ToList();

            //-- Feature-X
            Feature_X = regression(X_MeanNorm, 3).ToList();

            //-- Feature-Y
            Feature_Y = Zero_Normalization(Y_Array_t, Y_Array_t.Count).ToList();
            Feature_Y = Space_Normalization(Feature_Y, Space_Divide).ToList();

            //-- Feature P
            Feature_P = Zero_Normalization(P_MeanNorm, P_MeanNorm.Count).ToList();

            //-- Feature-V
            //-- a、b、c、d、e
            //-- a = d(a,b)/t(a,b). c = d(b,d)/t(b,d)
            for (int i = 0; i < SpacedTime_Divide; i++)
            {
                if (i == 0)
                {
                    if ((T_MeanNorm[1] - T_MeanNorm[0]) == 0)
                    {
                        V_Value.Add(0);
                        Vx_Value.Add(0);
                        Vy_Value.Add(0);
                    }
                    else if ((X_MeanNorm[1] - X_MeanNorm[0] == 0 && (Y_MeanNorm[1] - Y_MeanNorm[0]) == 0))
                    {
                        V_Value.Add(0);
                        Vx_Value.Add(0);
                        Vy_Value.Add(0);
                    }
                    else
                    {
                        V = Math.Sqrt(Math.Pow(X_MeanNorm[1] - X_MeanNorm[0], 2) + Math.Pow(Y_MeanNorm[1] - Y_MeanNorm[0], 2))
                            / (T_MeanNorm[1] - T_MeanNorm[0]);
                        V_Value.Add(V);

                        Vx = (X_MeanNorm[1] - X_MeanNorm[0]) / (T_MeanNorm[1] - T_MeanNorm[0]);
                        Vx_Value.Add(Vx);

                        Vy = (Y_MeanNorm[1] - Y_MeanNorm[0]) / (T_MeanNorm[1] - T_MeanNorm[0]);
                        Vy_Value.Add(Vy);
                    }
                }
                else if (i == SpacedTime_Divide - 1)
                {
                    if ((T_MeanNorm[i] - T_MeanNorm[i - 1]) == 0)
                    {
                        V_Value.Add(0);
                        Vx_Value.Add(0);
                        Vy_Value.Add(0);
                    }
                    else if ((X_MeanNorm[i] - X_MeanNorm[i - 1] == 0 && (Y_MeanNorm[i] - Y_MeanNorm[i - 1]) == 0))
                    {
                        V_Value.Add(0);
                        Vx_Value.Add(0);
                        Vy_Value.Add(0);
                    }
                    else
                    {
                        V = Math.Sqrt(Math.Pow(X_MeanNorm[i] - X_MeanNorm[i - 1], 2) + Math.Pow(Y_MeanNorm[i] - Y_MeanNorm[i - 1], 2))
                            / (T_MeanNorm[i] - T_MeanNorm[i - 1]);
                        V_Value.Add(V);

                        Vx = (X_MeanNorm[i] - X_MeanNorm[i - 1]) / (T_MeanNorm[i] - T_MeanNorm[i - 1]);
                        Vx_Value.Add(Vx);

                        Vy = (Y_MeanNorm[i] - Y_MeanNorm[i - 1]) / (T_MeanNorm[i] - T_MeanNorm[i - 1]);
                        Vy_Value.Add(Vy);
                    }
                }
                else
                {
                    if ((T_MeanNorm[i + 1] - T_MeanNorm[i - 1]) == 0)
                    {
                        V_Value.Add(0);
                        Vx_Value.Add(0);
                        Vy_Value.Add(0);
                    }
                    else if ((X_MeanNorm[i + 1] - X_MeanNorm[i - 1] == 0 && (Y_MeanNorm[i + 1] - Y_MeanNorm[i - 1]) == 0))
                    {
                        V_Value.Add(0);
                        Vx_Value.Add(0);
                        Vy_Value.Add(0);
                    }
                    else
                    {
                        V = Math.Sqrt(Math.Pow(X_MeanNorm[i + 1] - X_MeanNorm[i - 1], 2) + Math.Pow(Y_MeanNorm[i + 1] - Y_MeanNorm[i - 1], 2))
                            / (T_MeanNorm[i + 1] - T_MeanNorm[i - 1]);

                        V_Value.Add(V);

                        Vx = (X_MeanNorm[i + 1] - X_MeanNorm[i - 1]) / (T_MeanNorm[i + 1] - T_MeanNorm[i - 1]);
                        Vx_Value.Add(Vx);

                        Vy = (Y_MeanNorm[i + 1] - Y_MeanNorm[i - 1]) / (T_MeanNorm[i + 1] - T_MeanNorm[i - 1]);
                        Vy_Value.Add(Vy);
                    }
                }

            }

            //-- Feature-Vx & Vy
            List<double> Vx_Zscore = new List<double>();
            Vx_Zscore = Zero_Normalization(Vx_Value, Vx_Value.Count).ToList();
            //Feature_Vx = regression(Vx_Zscore, 3).ToList();
            Feature_Vx = Vx_Zscore;
            Feature_Vy = Zero_Normalization(Vy_Value, Vy_Value.Count).ToList();

            //-- Feature-A
            for (int i = 0; i < SpacedTime_Divide; i++)
            {
                if (i == 0)
                {
                    if ((T_MeanNorm[1] - T_MeanNorm[0]) == 0)
                    {
                        A_Value.Add(0);
                        Ax_Value.Add(0);
                        Ay_Value.Add(0);
                    }
                    else if ((Vx_Value[1] - Vx_Value[0] == 0 && (Vy_Value[1] - Vy_Value[0]) == 0))
                    {
                        A_Value.Add(0);
                        Ax_Value.Add(0);
                        Ay_Value.Add(0);
                    }
                    else
                    {
                        A = Math.Sqrt(Math.Pow(Vx_Value[1] - Vx_Value[0], 2) + Math.Pow(Vy_Value[1] - Vy_Value[0], 2))
                            / (T_MeanNorm[1] - T_MeanNorm[0]);
                        A_Value.Add(A);

                        Ax = (Vx_Value[1] - Vx_Value[0]) / (T_MeanNorm[1] - T_MeanNorm[0]);
                        Ax_Value.Add(Ax);

                        Ay = (Vy_Value[1] - Vy_Value[0]) / (T_MeanNorm[1] - T_MeanNorm[0]);
                        Ay_Value.Add(Ay);
                    }
                }
                else if (i == SpacedTime_Divide - 1)
                {
                    if ((T_MeanNorm[i] - T_MeanNorm[i - 1]) == 0)
                    {
                        A_Value.Add(0);
                        Ax_Value.Add(0);
                        Ay_Value.Add(0);
                    }
                    else if ((Vx_Value[i] - Vx_Value[i - 1] == 0 && (Vy_Value[i] - Vy_Value[i - 1]) == 0))
                    {
                        A_Value.Add(0);
                        Ax_Value.Add(0);
                        Ay_Value.Add(0);
                    }
                    else
                    {
                        A = Math.Sqrt(Math.Pow(Vx_Value[i] - Vx_Value[i - 1], 2) + Math.Pow(Vy_Value[i] - Vy_Value[i - 1], 2))
                            / (T_MeanNorm[i] - T_MeanNorm[i - 1]);
                        A_Value.Add(A);

                        Ax = (Vx_Value[i] - Vx_Value[i - 1]) / (T_MeanNorm[i] - T_MeanNorm[i - 1]);
                        Ax_Value.Add(Ax);

                        Ay = (Vy_Value[i] - Vy_Value[i - 1]) / (T_MeanNorm[i] - T_MeanNorm[i - 1]);
                        Ay_Value.Add(Ay);
                    }
                }
                else
                {
                    if ((T_MeanNorm[i + 1] - T_MeanNorm[i - 1]) == 0)
                    {
                        A_Value.Add(0);
                        Ax_Value.Add(0);
                        Ay_Value.Add(0);
                    }
                    else if ((Vx_Value[i + 1] - Vx_Value[i - 1] == 0 && (Vy_Value[i + 1] - Vy_Value[i - 1]) == 0))
                    {
                        A_Value.Add(0);
                        Ax_Value.Add(0);
                        Ay_Value.Add(0);
                    }
                    else
                    {
                        A = Math.Sqrt(Math.Pow(Vx_Value[i + 1] - Vx_Value[i - 1], 2) + Math.Pow(Vy_Value[i + 1] - Vy_Value[i - 1], 2))
                            / (T_MeanNorm[i + 1] - T_MeanNorm[i - 1]);

                        A_Value.Add(A);

                        Ax = (Vx_Value[i + 1] - Vx_Value[i - 1]) / (T_MeanNorm[i + 1] - T_MeanNorm[i - 1]);
                        Ax_Value.Add(Ax);

                        Ay = (Vy_Value[i + 1] - Vy_Value[i - 1]) / (T_MeanNorm[i + 1] - T_MeanNorm[i - 1]);
                        Ay_Value.Add(Ay);
                    }
                }
            }

            //-- Feature-Ax & Ay
            Feature_Ax = Zero_Normalization(Ax_Value, Ax_Value.Count).ToList();
            Feature_Ay = Zero_Normalization(Ay_Value, Ay_Value.Count).ToList();

            //-- Polar coordinates
            List<double> R1_Value = new List<double>();

            for (int i = 1; i < X_MeanNorm.Count; i++)
            {
                X1_Value.Add(X_MeanNorm[i] - X_MeanNorm[i - 1]);
                Y1_Value.Add(Y_MeanNorm[i] - Y_MeanNorm[i - 1]);
            }

            for (int i = 0; i < X1_Value.Count; i++)
            {
                double temp;

                R1_Value.Add(Math.Sqrt(Math.Pow(X1_Value[i], 2) + Math.Pow(Y1_Value[i], 2)));

                if (Y1_Value[i] == 0)
                    Feature_A.Add(0);
                else if (X1_Value[i] == 0 && Y1_Value[i] > 0)
                    Feature_A.Add(90);
                else if (X1_Value[i] == 0 && Y1_Value[i] < 0)
                    Feature_A.Add(270);
                else
                {
                    temp = Math.Atan(Y1_Value[i] / X1_Value[i]) * 180 / Math.PI;

                    if (X1_Value[i] > 0 && Y1_Value[i] > 0)
                        Feature_A.Add(temp);
                    else if (X1_Value[i] < 0 && Y1_Value[i] > 0)
                        Feature_A.Add(temp + 180);
                    else if (X1_Value[i] < 0 && Y1_Value[i] < 0)
                        Feature_A.Add(temp + 180);
                    else if (X1_Value[i] > 0 && Y1_Value[i] < 0)
                        Feature_A.Add(temp + 360);
                }
            }

            //-- Feature A
            for (int i = 0; i < Feature_A.Count; i++)
            {
                Feature_A[i] = Math.Sin(Feature_A[i] / 180 * Math.PI);
            }

            //-- Feature R
            List<double> R1_Zscore = new List<double>();
            R1_Zscore = Zero_Normalization(R1_Value, R1_Value.Count).ToList();
            Feature_R = Space_Normalization(R1_Zscore, Space_Divide).ToList();
            //Feature_R = regression(R1_Value, 3).ToList();

            //-- FeatureStd 
            X_std = CalculateFeatureStd(Feature_X);
            Y_std = CalculateFeatureStd(Feature_Y);
            Vx_std = CalculateFeatureStd(Feature_Vx);
            Vy_std = CalculateFeatureStd(Feature_Vy);
            Ax_std = CalculateFeatureStd(Feature_Ax);
            Ay_std = CalculateFeatureStd(Feature_Ay);
            P_std = CalculateFeatureStd(Feature_P);
            A_std = CalculateFeatureStd(Feature_A);
            R_std = CalculateFeatureStd(Feature_R);
        }

        
    }
}
