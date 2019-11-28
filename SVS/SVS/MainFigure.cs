using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Signature;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;


namespace SVS
{
    public partial class MainFigure : Form
    {
        static Form FC;

        Size DBformSize = new Size();
        Size EntrollformSize = new Size();

        //-- 執行檔位置
        string ExeDirectory = "";

        //-- 測試訊號
        SVSystem Template = new SVSystem();
        SVSystem Test = new SVSystem();

        string Filename_test;

        bool TemplateReady = false;
        bool TesterReady = false;

        double SuggestScore = 0;

        Stopwatch stopWatch = new Stopwatch();
        long pre_costTime=0;
        struct logInfo
        {
           // public string name;
            public string Score;
            public string Serurity;
            public string Result;
            public string vcostTime;
            public string Woner;
            public string date;
        }
        logInfo logInfoData;
        public MainFigure()
        {
            InitializeComponent();

            ExeDirectory = "";

            //-- 得到執行檔的目錄層
            String Dir = System.Windows.Forms.Application.StartupPath;
            /*for (int i = 0; i < 4; i++)
            {
                DirectoryInfo DI = new DirectoryInfo(Dir);

                //取得父代的 DirectoryInfo 類
                DirectoryInfo DI_parent = DI.Parent;

                //假若父目錄存在
                if (DI_parent != null)
                {
                    // 利用SetCurrentDirectory設定工作目錄上移
                    Directory.SetCurrentDirectory(DI_parent.FullName);
                    Dir = Directory.GetCurrentDirectory();
                }
            }
            String ParentDicectory = Directory.GetCurrentDirectory();
            //-- 保存父目錄
            ParameterPass.ParentDirectory = ParentDicectory;

            //-- 設定目錄位置
            ExeDirectory = ParentDicectory + @"\V1.0.0.3";*/
            ExeDirectory = Dir;

            //-- 傳遞樣板資料
            Template = ParameterPass.Template;
            //-- 使用者名稱
            lbFileName_Temp.Text = Template.FileName;
            //-- 建議分數
            SuggestScore = ParameterPass.Score[0];
            lbAdviceScore.Text = SuggestScore.ToString("0.00");// + " ~ " + ParameterPass.Score[1].ToString("0.00");
            //-- 花費時間
            lbCostTime.Text = Template.CostTime.ToString("0.00") + MultipleLangString.GetString("_sec");

            TemplateReady = true;
        }

        private void btOpenTest_Click(object sender, EventArgs e)
        {
           // openTest();
           SignatureStyle();
        }
        private void SignatureStyle()
        {
            List<string> FileName = new List<string>();
            List<int> FileOrder = new List<int>();

            //-- 清除雷達圖
            chtRadar.Series["Similarity"].Points.Clear();

            //-- 清除顏色
            lbScore.BackColor = Color.White;

            //-- 取得當前目錄夾下資料
            string path = ExeDirectory + @"\" + Template.FileName;
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (var fi in di.GetFiles("*.txt"))
            {
                FileName.Add(fi.Name);
            }


            for (int i = 0; i < FileName.Count; i++)
            {
                char[] t = FileName[i].ToCharArray();
                int year = Convert.ToInt32(FileName[i].Substring(0, 4));
                int mon = Convert.ToInt32(FileName[i].Substring(5, 2));
                int day = Convert.ToInt32(FileName[i].Substring(8, 2));
                int hh = Convert.ToInt32(FileName[i].Substring(11, 2));
                int mm = Convert.ToInt32(FileName[i].Substring(14, 2));
                int ss = Convert.ToInt32(FileName[i].Substring(17, 2));
                int ms = Convert.ToInt32(FileName[i].Substring(20, 2));

                FileOrder.Add(mon * 100000000 + day * 1000000 + hh * 10000 + mm * 100 + ss);
            }

            //-- 找出最小值且位置
            int Max = FileOrder[0];
            int k = 0;
            for (int i = 0; i < FileOrder.Count; i++)
            {
                if (FileOrder[i] < Max)
                {
                    Max = FileOrder[i];
                    k = i;
                }
            }

            //-- 選擇的完整路徑
            Filename_test = path + @"\" + FileName[k].ToString();
            StreamReader TesterData = new System.IO.StreamReader(Filename_test, System.Text.Encoding.Default);

            //-- 顯示檔案名稱 for testing
            lbFileName_Test.Text = Path.GetFileName(Filename_test);

            //-- Clear all dataset 
            Test.SVS_Reset();

            //-- 資料解碼   
            int DataLength = 0;
            DataLength = Test.DataDecoder(TesterData);

            //-- SVS
            if (DataLength > 2)
            {
                Test.SignatureDataProcessing(DataLength);
            }

            //-- Check all feature OK
            bool FeatureError = false;
            FeatureError = Test.CheckFeature();
            if (FeatureError == true)
            {
                MessageBox.Show(MultipleLangString.GetString("_signError"));
            }
            else
                TesterReady = true;

            //-- 繪出原始訊號圖                  
            chtSignature_Test.Series["Tester"].ChartType = SeriesChartType.FastPoint;
            chtSignature_Test.Series["Tester"].Points.Clear();

            for (int i = 0; i < DataLength; i++)
            {
                chtSignature_Test.Series["Tester"].Points.AddXY(Test.X_Array[i], Test.Y_Array[i]);
            }
        }
        private void openTest()
        {
            List<string> FileName = new List<string>();
            List<int> FileOrder = new List<int>();

            //-- 清除雷達圖
            chtRadar.Series["Similarity"].Points.Clear();

            //-- 清除顏色
            lbScore.BackColor = Color.White;

            //-- 取得當前目錄夾下資料
            string path = ExeDirectory + @"\" + Template.FileName;
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (var fi in di.GetFiles("*.txt"))
            {
                FileName.Add(fi.Name);
            }


            for (int i = 0; i < FileName.Count; i++)
            {
                char[] t = FileName[i].ToCharArray();
                int year = Convert.ToInt32(FileName[i].Substring(0, 4));
                int mon = Convert.ToInt32(FileName[i].Substring(5, 2));
                int day = Convert.ToInt32(FileName[i].Substring(8, 2));
                int hh = Convert.ToInt32(FileName[i].Substring(11, 2));
                int mm = Convert.ToInt32(FileName[i].Substring(14, 2));
                int ss = Convert.ToInt32(FileName[i].Substring(17, 2));
                int ms = Convert.ToInt32(FileName[i].Substring(20, 2));

                FileOrder.Add(mon * 100000000 + day * 1000000 + hh * 10000 + mm * 100 + ss);
            }

            //-- 找出最大值且位置
            int Max = 0;
            int k = 0;
            for (int i = 0; i < FileOrder.Count; i++)
            {
                if (FileOrder[i] > Max)
                {
                    Max = FileOrder[i];
                    k = i;
                }
            }

            //-- 選擇的完整路徑
            Filename_test = path + @"\" + FileName[k].ToString();
            StreamReader TesterData = new System.IO.StreamReader(Filename_test, System.Text.Encoding.Default);

            //-- 顯示檔案名稱 for testing
            lbFileName_Test.Text = Path.GetFileName(Filename_test);

            //-- Clear all dataset 
            Test.SVS_Reset();

            //-- 資料解碼   
            int DataLength = 0;
            DataLength = Test.DataDecoder(TesterData);

            //-- SVS
            if (DataLength > 2)
            {
                Test.SignatureDataProcessing(DataLength);
            }

            //-- Check all feature OK
            bool FeatureError = false;
            FeatureError = Test.CheckFeature();
            if (FeatureError == true)
            {
                MessageBox.Show(MultipleLangString.GetString("_signError"));
            }
            else
                TesterReady = true;

            //-- 繪出原始訊號圖                  
            chtSignature_Test.Series["Tester"].ChartType = SeriesChartType.FastPoint;
            chtSignature_Test.Series["Tester"].Points.Clear();

            for (int i = 0; i < DataLength; i++)
            {
                chtSignature_Test.Series["Tester"].Points.AddXY(Test.X_Array[i], Test.Y_Array[i]);
            }
        }

        private void btVerification_Click(object sender, EventArgs e)
        {
            //-- 安全分數設定 - 隨機仿冒者分數落在60-70分居多  
            int SerurityScore_Norm = 80;
            int SerurityScore_Min = 75;

            double Score = 0;

            stopWatch.Reset();
            stopWatch.Start();

            if (TemplateReady == true && TesterReady == true)
            {
                Score = SVSystem.Recognizer(Template, Test);
                lbcurTime.Text = Test.CostTime.ToString("0.00") + MultipleLangString.GetString("_sec");

                //-- 書寫時間設定 _ 正負2秒內認可
                if (Math.Abs(Template.CostTime - Test.CostTime) > 2)
                {
                    TesterReady = false;
                    lbScore.Text = Score.ToString("0.00");
                    lbScore.BackColor = Color.Red;
                    lbResultStatus.Text = MultipleLangString.GetString("_fail");
                }
                else
                {
                    if (Score < SerurityScore_Min)
                    {
                        Score = 0;
                        lbScore.BackColor = Color.Red;
                        lbResultStatus.Text = MultipleLangString.GetString("_fail");
                    }
                    else
                    {
                        if (Score >= SuggestScore)
                        {
                            lbScore.BackColor = Color.LightGreen;
                            lbResultStatus.Text = MultipleLangString.GetString("_pass");
                        }
                        else if (Score >= SerurityScore_Norm && Score < SuggestScore)
                        {
                            lbScore.BackColor = Color.LightBlue;
                            lbResultStatus.Text = MultipleLangString.GetString("_pass");
                        }
                        else if (Score < SerurityScore_Norm)
                        {
                            lbScore.BackColor = Color.Red;
                            lbResultStatus.Text = MultipleLangString.GetString("_fail");
                        }
                    }
                    lbScore.Text = Score.ToString("0.00");
                }

                //-- The data for the chart
                double shape = (SVSystem.Similarity[0] + SVSystem.Similarity[1] + SVSystem.Similarity[7]) * 100 / 3;
                double Pressure = SVSystem.Similarity[2] * 100;
                double Velocity = (SVSystem.Similarity[3] + SVSystem.Similarity[4]) * 100 / 2;
                double Acceleration = (SVSystem.Similarity[5] + SVSystem.Similarity[6]) * 100 / 2;

                double[] Similarity = { shape, Pressure, Velocity, Acceleration };

                //-- The labels for the chart
                string[] labels = { MultipleLangString.GetString("_shape"), MultipleLangString.GetString("_pressure"), MultipleLangString.GetString("_velocity"), MultipleLangString.GetString("_acceleration") };

                chtRadar.Series["Similarity"].Points.DataBindXY(labels, Similarity);
                lbShape.Text = shape.ToString("#0.0") + '%';
                lbPressure.Text = Pressure.ToString("#0.0") + '%';
                lbVelocity.Text = Velocity.ToString("#0.0") + '%';
                lbAcc.Text = Acceleration.ToString("#0.0") + '%';

                
                stopWatch.Stop();
                Console.WriteLine("Verify algorithm function cost time..." + stopWatch.ElapsedMilliseconds + " ms");
                if ((pre_costTime + stopWatch.ElapsedMilliseconds) < 2000)
                    logInfoData.vcostTime = Convert.ToString((pre_costTime + stopWatch.ElapsedMilliseconds))+"ms";
                else
                    logInfoData.vcostTime = "error";

                lbVerifyCost.Text = logInfoData.vcostTime;

                // collect log data
                logInfoData.Score = lbScore.Text;
                if (Score < SerurityScore_Norm)
                    logInfoData.Result = "FAIL";
                else
                    logInfoData.Result = "PASS";

                logInfoData.Serurity = SerurityScore_Norm.ToString("0.00");
               // logInfoData.vcostTime = lbcurTime.Text;
                logInfoData.date = lbFileName_Test.Text;

                DialogResult myResult = MessageBox.Show("此為\"本人\"簽名嗎?", "FAR / FRR記錄", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (myResult == DialogResult.Yes)
                {
                    logInfoData.Woner = "YES";
                    ExportLogToExcel(true);
                }
                else if (myResult == DialogResult.No)
                {
                    logInfoData.Woner = "NO";
                    ExportLogToExcel(false);
                }
                DialogResult diag = MessageBox.Show("DONE", "Information",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }

        Demo pointform = null;

        private void SetPointFormNull()
        {
            pointform = null;
        }

        private void btNewSignature_Click(object sender, EventArgs e)
        {
            //-- 開啟執行檔位置
            /*Process p = new Process();
            p.StartInfo.FileName = ExeDirectory + @"\TestTool.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();*/
       
            pointform = new Demo();
            pointform.ReturnPointFormNull += new Demo.ReturnValueDelegate(SetPointFormNull);
            pointform.ShowDialog();
            if (pointform != null)
            {
                if (pointform.DialogResult == DialogResult.OK)
                {
                    //-- 建立使用者目錄
                    string path = ExeDirectory + @"\" + Template.FileName;
                    try
                    {
                        // Determine whether the directory exists.
                        if (!Directory.Exists(path))
                        {
                            // Try to create the directory.
                            DirectoryInfo userdir = Directory.CreateDirectory(path);
                        }
                    }
                    catch (System.IO.IOException err)
                    {
                        MessageBox.Show(err.Message);
                    }
                    DirectoryInfo di = new DirectoryInfo(ExeDirectory);
                    foreach (var fi in di.GetFiles("*.txt"))
                    {
                        System.IO.File.Move(di + @"\" + fi.Name, path + @"\" + fi.Name);
                    }

                    stopWatch.Reset();
                    stopWatch.Start();
                    openTest();
                    stopWatch.Stop();
                    pre_costTime = stopWatch.ElapsedMilliseconds;
                    Console.WriteLine("Data decoder @openTest function cost time..." + pre_costTime + " ms");

                }
            }
        }

        private void MainFigure_Load(object sender, EventArgs e)
        {
            this.Text = MultipleLangString.GetString("_svs");
            this.lbFilleNameTemp_fixed.Text = MultipleLangString.GetString("_userName");
            this.lbAdviceScore_fixed.Text = MultipleLangString.GetString("_adviceScore");
            this.lbCostTime_fixed.Text = MultipleLangString.GetString("_costTime");
            this.btNewSignature.Text = MultipleLangString.GetString("_newSignature");
            this.lbFileNameTest_fixed.Text = MultipleLangString.GetString("_filename");
            this.btVerification.Text = MultipleLangString.GetString("_verification");
            this.lbScore_fixed.Text = MultipleLangString.GetString("_currentScore");
            this.lbcurTime_fixed.Text = MultipleLangString.GetString("_costTime");
            this.lbShape_fixed.Text = MultipleLangString.GetString("_shape") + "：";
            this.lbPressure_fixed.Text = MultipleLangString.GetString("_pressure") + "：";
            this.lbVelocity_fixed.Text = MultipleLangString.GetString("_velocity") + "：";
            this.lbAcc_fixed.Text = MultipleLangString.GetString("_acceleration") + "：";
            this.lbSignauture_fixed.Text = MultipleLangString.GetString("_userSignature");
            this.lbAnalysis_fixed.Text = MultipleLangString.GetString("_analysis");

            this.lbVerifyCost_fixed.Text = MultipleLangString.GetString("_verifycost") + "：";
            this.lbVerifyCost_fixed.Text = MultipleLangString.GetString("_logout");
        }

        //==========================================================================================//
        //                                                                                          //
        // File Log寫入                                                                             //
        //                                                                                          //
        //==========================================================================================//
        Excel.Application excelApp;
        Excel._Workbook wBook;
        Excel._Worksheet wSheet;
        Excel.Range wRange;
        private void ExportLogToExcel(Boolean IsWoner)
        {


            // 設定儲存檔名，不用設定副檔名，系統自動判斷 excel 版本，產生 .xls 或 .xlsx 副檔名
            //Log File Path
            string pathFile = @"D:\SVS_FAR_FRR_Log";

            int currentNumofSheet = 0;

            // 開啟一個新的應用程式
            excelApp = new Excel.Application();
            if (excelApp == null)
            {
                MessageBox.Show("Excel APP NEW Error !!!");
                System.Windows.Forms.Application.Exit();
            }

            // 讓Excel文件可見
            //excelApp.Visible = true;
            excelApp.Visible = false;
            
            // 停用警告訊息
            excelApp.DisplayAlerts = false;
            excelApp.AlertBeforeOverwriting = false;

            //Create Log File and set title
            if (!(System.IO.File.Exists(pathFile + ".xls")) && !(System.IO.File.Exists(pathFile + ".xlsx")))
            {


                //// 開啟一個新的應用程式
                //excelApp = new Excel.Application();

                //// 讓Excel文件可見
                //excelApp.Visible = true;

                //// 停用警告訊息
                //excelApp.DisplayAlerts = false;

                //// 加入新的活頁簿
                excelApp.Workbooks.Add(Type.Missing);

                // 引用第一個活頁簿
                wBook = excelApp.Workbooks[1];

                currentNumofSheet = excelApp.Worksheets.Count;
                for (int i = currentNumofSheet; i < 2; i++)
                    excelApp.Worksheets.Add(Type.Missing);

                currentNumofSheet = excelApp.Worksheets.Count;


                // 設定活頁簿焦點
                wBook.Activate();
                try
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        // 引用第1個工作表
                        wSheet = (Excel._Worksheet)wBook.Worksheets[i + 1];

                        // 命名工作表的名稱
                        if (i == 0)
                            wSheet.Name = "FRR簽名測試";
                        else
                            wSheet.Name = "FAR簽名測試";
                        // 設定工作表焦點
                        wSheet.Activate();
                        excelApp.Cells[1, 1] = "Excel測試";

                        // 設定第1列資料
                        excelApp.Cells[1, 1] = "NAME";
                        excelApp.Cells[1, 2] = "SCORE";
                        excelApp.Cells[1, 3] = "RESULT";
                        excelApp.Cells[1, 4] = "SECURITY";
                        excelApp.Cells[1, 5] = "OWNER";
                        excelApp.Cells[1, 6] = "COST TIME";
                        excelApp.Cells[1, 7] = "FILE DATE";

                    }

                    try
                    {
                        //另存活頁簿
                        wBook.SaveAs(pathFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                        Console.WriteLine("儲存文件於 " + Environment.NewLine + pathFile);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("儲存檔案出錯，檔案可能正在使用" + Environment.NewLine + ex.Message);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("產生報表時出錯！" + Environment.NewLine + ex.Message);
                }

                ////關閉活頁簿
                wBook.Close(false, Type.Missing, Type.Missing);
            }
            //============================================
            excelApp.UserControl = true;
            wBook = excelApp.Workbooks.Open(pathFile);
            if (IsWoner)
                wSheet = (Excel._Worksheet)wBook.Worksheets[1];
            else
                wSheet = (Excel._Worksheet)wBook.Worksheets[2];

            wSheet.Activate();

            int x = wSheet.UsedRange.Cells.Rows.Count;
            int y = wSheet.UsedRange.Cells.Columns.Count;

            //write verify data into log
            //// 設定第1列資料
            //excelApp.Cells[1, 1] = "NAME";
            //excelApp.Cells[1, 2] = "SCORE";
            //excelApp.Cells[1, 3] = "RESULT";
            //excelApp.Cells[1, 4] = "SECURITY";
            //excelApp.Cells[1, 5] = "WONER";
            //excelApp.Cells[1, 6] = "CostTime";
            //excelApp.Cells[1, 7] = "DATE";

            //============================================
            x++; //new data line
            excelApp.Cells[x, 1] = Template.FileName;
            excelApp.Cells[x, 2] = logInfoData.Score;
            excelApp.Cells[x, 3] = logInfoData.Result;
            excelApp.Cells[x, 4] = logInfoData.Serurity;
            excelApp.Cells[x, 5] = logInfoData.Woner;
            excelApp.Cells[x, 6] = logInfoData.vcostTime;
            excelApp.Cells[x, 7] = logInfoData.date;


            //TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT
            //// 設定第1列顏色
            //wRange = wSheet.Range[wSheet.Cells[1, 1], wSheet.Cells[1, 5]];
            //wRange.Select();
            //wRange.Font.Color = ColorTranslator.ToOle(Color.White);
            //wRange.Interior.Color = ColorTranslator.ToOle(Color.LightSeaGreen);

            //// 設定第2列資料
            //excelApp.Cells[2, 1] = "AA";
            //excelApp.Cells[2, 2] = "10";

            //// 設定第3列資料
            //excelApp.Cells[3, 1] = "BB";
            //excelApp.Cells[3, 2] = "20";

            //// 設定第4列資料
            //excelApp.Cells[4, 1] = "CC";
            //excelApp.Cells[4, 2] = "30";


            //VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV


            //// 設定第5列資料
            //excelApp.Cells[5, 1] = "總計";
            //// 設定總和公式 =SUM(B2:B4)
            //excelApp.Cells[5, 2].Formula = string.Format("=SUM(B{0}:B{1})", 2, 4);
            //// 設定第5列顏色
            //wRange = wSheet.Range[wSheet.Cells[5, 1], wSheet.Cells[5, 2]];
            //wRange.Select();
            //wRange.Font.Color = ColorTranslator.ToOle(Color.Red);
            //wRange.Interior.Color = ColorTranslator.ToOle(Color.Yellow);

            // 自動調整欄寬
            wRange = wSheet.Range[wSheet.Cells[1, 1], wSheet.Cells[x, 7]];
            wRange.Select();
            wRange.Columns.AutoFit();

            try
            {
                //另存活頁簿
                wBook.SaveAs(pathFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Console.WriteLine("儲存文件於 " + Environment.NewLine + pathFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("儲存檔案出錯，檔案可能正在使用" + Environment.NewLine + ex.Message);
            }
        

            //關閉活頁簿
            wBook.Close(false, Type.Missing, Type.Missing);

            //關閉Excel
            excelApp.Quit();

            //釋放Excel資源
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            wBook = null;
            wSheet = null;
            wRange = null;
            excelApp = null;
            GC.Collect();

            Console.Read();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //-- 若已開啟註冊頁面，關閉
            if (Application.OpenForms["Entrollment"] as Database != null)
            {
                Application.OpenForms["Entrollment"].Close();
            }

            if (Application.OpenForms["MainFigure"] as Database != null)
            {
                Application.OpenForms["MainFigure"].Close();
            }

            //-- 開啟登入頁面
            if (Application.OpenForms["Database"] as Database != null)
            {

                FC.Size = DBformSize;
                Application.OpenForms["Database"].WindowState = FormWindowState.Maximized;
                ParameterPass.MainFugure_flag = false;
            }
            else
            {
                //-- open
                Database DBform = new Database();
                DBform.MdiParent = FC;

                //-- 設定呈現大小
                DBformSize = DBform.Size;
                FC.Size = DBform.Size;
                DBform.WindowState = FormWindowState.Maximized;

                DBform.Show();
                ParameterPass.MainFugure_flag = false;

            }
        }
        public void getFrom(Form f)
        {
            FC = f;
        }
    }
}
