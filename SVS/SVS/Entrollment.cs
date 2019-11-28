using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Signature;
using MySql.Data.MySqlClient;
using System.Diagnostics;


namespace SVS
{


    public partial class Entrollment : Form
    {
        Size DBformSize = new Size();
        Size EntrollformSize = new Size();
        static Form FC;
        //-- 樣板訊號
        SVSystem TemplateData = new SVSystem();

        //-- 執行檔位置
        string ExeDirectory = "";

        //-- MySQL connect information
        string dbHost = "127.0.0.1";
        string dbPort = "3306";
        string dbUser = "root";
        string dbPass = "";
        string dbName = "SVS";

        //-- 挑選與註冊確認旗標
        bool TemplateTrainDone = false;
        bool Connect_flag = true;      
        bool CheckFeature = true;
        bool CheckInfoFilled = true;

        //-- 更新、紀錄檔案數量
        bool FirstCheck = true;
        int FileCount = 0;

        public Entrollment()
        {
            InitializeComponent();

            lbTemplateInfo.Text = "";

            ExeDirectory = "";
          

            TemplateTrainDone = false;
            Connect_flag = true;
            CheckFeature = true;
            CheckInfoFilled = true;

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
            ExeDirectory = ParentDicectory + @"\V1.0.0.3";      */
            ExeDirectory = Dir;
        }

        /// <summary>
        /// 新筆簽名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNewSignature_Click(object sender, EventArgs e)
        {
            //-- 開啟執行檔位置
            /*
            Process p = new Process();
            p.StartInfo.FileName = ExeDirectory + @"\TestTool.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();*/
            pointform = new Demo();
            pointform.ReturnPointFormNull += new Demo.ReturnValueDelegate(SetPointFormNull);
            pointform.Show();
        }

        Demo pointform = null;

        private void SetPointFormNull()
        {
            pointform = null;
        }

        /// <summary>
        /// 樣板挑選
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTemplateSelection_Click(object sender, EventArgs e)
        {
            SVSystem[] Database;
            int FileCount = 0;

            List<double> DatabaseCostTime = new List<double>();
            bool Error_flag = false;

            //-- Step 3: 開啟對話框
            {
                try
                {
                    //-- 確認檔案數量，建立簽名庫
                    int FileLength = clbNewSignature.Items.Count;

                    //-- 配置陣列大小，並宣告類別給各陣列值
                    Database = new SVSystem[FileLength];
                    for (int i = 0; i < FileLength; i++)
                    {
                        Database[i] = new SVSystem();
                    }

                    //-- 依序對所有資料進行資料處理
                    foreach (String file in clbNewSignature.Items)
                    {
                        String FilePath = ExeDirectory + @"\" + file;

                        //-- 註冊檔案名稱
                        Database[FileCount].FileName = Path.GetFileName(FilePath);

                        //-- 選擇的完整路徑
                        StreamReader srData = new StreamReader(FilePath, Encoding.Default);

                        //-- 資料解碼   
                        int DataLength = 0;
                        DataLength = Database[FileCount].DataDecoder(srData);

                        //-- 取得每筆資料特徵
                        if (DataLength > 2)
                        {
                            Database[FileCount].SignatureDataProcessing(DataLength);
                            DatabaseCostTime.Add(Database[FileCount].CostTime);
                        }

                        //-- Check all feature OK
                        bool FeatureError = false;
                        FeatureError = Database[FileCount].CheckFeature();
                        if (FeatureError == true)
                        {
                            MessageBox.Show(MultipleLangString.GetString("_signError"));
                            Error_flag = true;
                            break;
                        }

                        FileCount++;

                        srData.Dispose();
                    }


                    //-- 有錯誤就不進入
                    if (!Error_flag)
                    {
                        //-- 初始化顯示框與旗標變數
                        lbTemplateInfo.Text = "";
                        TemplateTrainDone = false;

                        //-- Template selection
                        double Score = 0;
                        double Upper = 0;
                        double lower = 0;
                        double Mean = 0;
                        double Std = 0;
                        double[] BestTemplate = new double[10 + 2 * Database.Length];
                        List<int> BadSig = new List<int>();
                        String TemplateInfo;

                        //-- 挑選最佳樣板
                        BestTemplate = SVSystem.SignatureTemplateSelection(Database, ref TemplateData, ref BadSig);

                        Score = BestTemplate[1];
                        Mean = BestTemplate[2];
                        Std = BestTemplate[3];
                        Upper = BestTemplate[4];
                        lower = BestTemplate[5];

                        //-- 紀錄最佳樣板分數
                        TemplateData.CostTime = BestTemplate[6];
                        TemplateData.ScoreLow = lower;
                        TemplateData.ScoreHigh = Upper;

                        //-- 顯示相關資訊
                        TemplateInfo = MultipleLangString.GetString("_scoreRange") + lower.ToString("0.00") + " - " + Upper.ToString("0.00") + '\n' +                                           
                                              "Index: " + BestTemplate[0].ToString("") + "\n\n" +
                                              MultipleLangString.GetString("_signatureScore") + '\n';

                        for (int i = 1; i <= Database.Length; i++)
                        {
                            TemplateInfo = TemplateInfo + i.ToString() + " : " + BestTemplate[10 + 2 * (i - 1)].ToString("0.00") + " +- " + BestTemplate[10 + 2 * (i - 1) + 1].ToString("0.00")
                                           + "   " + DatabaseCostTime[i - 1].ToString("0.00") + "s\n";
                        }

                        TemplateInfo = TemplateInfo + "\n" + MultipleLangString.GetString("_reSignatures") + '\n';
                        if (BadSig.Count > 0)
                        {
                            for (int i = 0; i < BadSig.Count; i++)
                            {
                                TemplateInfo = TemplateInfo + BadSig[i].ToString() + '\n';
                            }
                        }
                        else
                            TemplateInfo = TemplateInfo + MultipleLangString.GetString("_no") + '\n' + MultipleLangString.GetString("_signComplete");

                        lbTemplateInfo.Text = TemplateInfo;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            
            //openFileDialog_Data.Dispose();
        }
        /// <summary>
        /// 樣板確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTemplateConfirm_Click(object sender, EventArgs e)
        {
            //TemplateData;
            TemplateTrainDone = true;

            lbTemplateInfo.Text = lbTemplateInfo.Text + "\n\n" +
                                  MultipleLangString.GetString("_confirmComplete");
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEntrollment_Click(object sender, EventArgs e)
        {            
            //-- 確認資料表格、樣板訓練都有完成
            if (tbAccount.Text == "" || tbPassword.Text == "" || tbUserName.Text == "" || TemplateTrainDone == false)
            {
                CheckInfoFilled = false;
                MessageBox.Show(MultipleLangString.GetString("_inComplete"));
            }

            //-- 確認資料準備好，才寫進資料庫中
            if (CheckInfoFilled)
            {
                string Query;
                string connStr = "server=" + dbHost + ";port=" + dbPort + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
                //("server=127.0.0.1;user=root;database=test;port=3306;password=1111;")

                MySqlConnection conn = new MySqlConnection(connStr);

                //-- 連線到資料庫
                try
                {
                    conn.Open();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    Connect_flag = false;
                    MessageBox.Show("Error");

                    switch (ex.Number)
                    {
                        case 0:
                            MessageBox.Show(MultipleLangString.GetString("_connectfail"));
                            break;
                        case 1045:
                            MessageBox.Show(MultipleLangString.GetString("_relogin"));
                            break;
                    }
                }

                //-- 伺服器連線成功
                if (Connect_flag)
                {                   
                    //-- 命令與伺服器連線，通過命令來對資料庫進行存取
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    //-- 查詢 xxx 資料表 共建立多少帳號
                    Query = "select SID from SVS.account;";
                    cmd.CommandText = Query;
                    MySqlDataReader Account = cmd.ExecuteReader();

                    int Account_len = 0;
                    while (Account.Read())
                    {
                        Account_len++;
                    }
                    Account.Close();

                    //-- 建立新帳號
                    int SID = Account_len + 1;
                    Query = "insert into SVS.account(SID, Uid, Password, UserName, Gender, FileName, ScoreLow, ScoreHigh, CostTime) values('"
                            + SID.ToString() + "','" + tbAccount.Text + "','" + tbPassword.Text + "','" + tbUserName.Text + "','" + cbSex.SelectedItem.ToString()
                            + "','" + TemplateData.FileName + "','" + TemplateData.ScoreLow + "','" + TemplateData.ScoreHigh + "','" + TemplateData.CostTime + "');";
                    cmd.CommandText = Query;
                    cmd.ExecuteNonQuery();
                    try
                    {
                        //MessageBox.Show("Create account successfully");
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                        CheckFeature = false;
                    }

                    //-- 建立特徵集
                    //-------------------------------------------------------------------------------------------------------------//
                    //-------------------------------------------------------------------------------------------------------------//
                    //-- X-SID
                    while (CheckFeature)
                    {
                        Query = "insert into SVS.feature_x(SID) values('" + SID.ToString() + "');";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {                           
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }
                        //-- X-寫資料
                        Query = "update SVS.feature_x set Len ='" + TemplateData.Feature_X.Count + "'where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {                          
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }

                        for (int i = 1; i <= TemplateData.Feature_X.Count; i++)
                        {
                            Query = "update SVS.feature_x set X" + i.ToString() + "='" + TemplateData.Feature_X[i - 1] + "'where SID='" + SID.ToString() + "';";
                            cmd.CommandText = Query;
                            cmd.ExecuteNonQuery();
                            try
                            {
                            }
                            catch (MySql.Data.MySqlClient.MySqlException ex)
                            {                              
                                MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                                CheckFeature = false;
                                break;
                            }
                        }
                        //-------------------------------------------------------------------------------------------------------------//
                        //-------------------------------------------------------------------------------------------------------------//
                        //--Y - SID
                        Query = "insert into SVS.feature_y(SID) values('" + SID.ToString() + "');";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }
                        //-- Y-寫資料
                        Query = "update SVS.feature_y set Len ='" + TemplateData.Feature_Y.Count + "'where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }

                        for (int i = 1; i <= TemplateData.Feature_Y.Count; i++)
                        {
                            Query = "update SVS.feature_y set Y" + i.ToString() + "='" + TemplateData.Feature_Y[i - 1] + "'where SID='" + SID.ToString() + "';";
                            cmd.CommandText = Query;
                            cmd.ExecuteNonQuery();
                            try
                            {
                            }
                            catch (MySql.Data.MySqlClient.MySqlException ex)
                            {
                                MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                                CheckFeature = false;
                                break;
                            }
                        }
                        ////-------------------------------------------------------------------------------------------------------------//
                        ////-------------------------------------------------------------------------------------------------------------//
                        ////-- P-SID
                        Query = "insert into SVS.feature_p(SID) values('" + SID.ToString() + "');";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }
                        //-- P-寫資料
                        Query = "update SVS.feature_p set Len ='" + TemplateData.Feature_P.Count + "'where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }

                        for (int i = 1; i <= TemplateData.Feature_P.Count; i++)
                        {
                            Query = "update SVS.feature_p set P" + i.ToString() + "='" + TemplateData.Feature_P[i - 1] + "'where SID='" + SID.ToString() + "';";
                            cmd.CommandText = Query;
                            cmd.ExecuteNonQuery();
                            try
                            {
                            }
                            catch (MySql.Data.MySqlClient.MySqlException ex)
                            {
                                MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                                CheckFeature = false;
                                break;
                            }
                        }
                        ////-------------------------------------------------------------------------------------------------------------//
                        ////-------------------------------------------------------------------------------------------------------------//
                        ////-- Vx-SID
                        Query = "insert into SVS.feature_vx(SID) values('" + SID.ToString() + "');";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }
                        //-- Vx-寫資料
                        Query = "update SVS.feature_vx set Len ='" + TemplateData.Feature_Vx.Count + "'where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }

                        for (int i = 1; i <= TemplateData.Feature_Vx.Count; i++)
                        {
                            Query = "update SVS.feature_vx set Vx" + i.ToString() + "='" + TemplateData.Feature_Vx[i - 1] + "'where SID='" + SID.ToString() + "';";
                            cmd.CommandText = Query;
                            cmd.ExecuteNonQuery();
                            try
                            {
                            }
                            catch (MySql.Data.MySqlClient.MySqlException ex)
                            {
                                MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                                CheckFeature = false;
                                break;
                            }
                        }
                        ////-------------------------------------------------------------------------------------------------------------//
                        ////-------------------------------------------------------------------------------------------------------------//
                        ////-- Vy-SID
                        Query = "insert into SVS.feature_vy(SID) values('" + SID.ToString() + "');";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }
                        //-- Vy-寫資料
                        Query = "update SVS.feature_vy set Len ='" + TemplateData.Feature_Vy.Count + "'where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }

                        for (int i = 1; i <= TemplateData.Feature_Vy.Count; i++)
                        {
                            Query = "update SVS.feature_vy set Vy" + i.ToString() + "='" + TemplateData.Feature_Vy[i - 1] + "'where SID='" + SID.ToString() + "';";
                            cmd.CommandText = Query;
                            cmd.ExecuteNonQuery();
                            try
                            {
                            }
                            catch (MySql.Data.MySqlClient.MySqlException ex)
                            {
                                MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                                CheckFeature = false;
                                break;
                            }
                        }
                        ////-------------------------------------------------------------------------------------------------------------//
                        ////-------------------------------------------------------------------------------------------------------------//
                        ////-- Ax-SID
                        Query = "insert into SVS.feature_ax(SID) values('" + SID.ToString() + "');";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }
                        //-- Ax-寫資料
                        Query = "update SVS.feature_ax set Len ='" + TemplateData.Feature_Ax.Count + "'where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }

                        for (int i = 1; i <= TemplateData.Feature_Ax.Count; i++)
                        {
                            Query = "update SVS.feature_ax set Ax" + i.ToString() + "='" + TemplateData.Feature_Ax[i - 1] + "'where SID='" + SID.ToString() + "';";
                            cmd.CommandText = Query;
                            cmd.ExecuteNonQuery();
                            try
                            {
                            }
                            catch (MySql.Data.MySqlClient.MySqlException ex)
                            {
                                MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                                CheckFeature = false;
                                break;
                            }
                        }
                        ////-------------------------------------------------------------------------------------------------------------//
                        ////-------------------------------------------------------------------------------------------------------------//
                        ////-- Ay-SID
                        Query = "insert into SVS.feature_ay(SID) values('" + SID.ToString() + "');";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }
                        //-- Ay-寫資料
                        Query = "update SVS.feature_ay set Len ='" + TemplateData.Feature_Ay.Count + "'where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }

                        for (int i = 1; i <= TemplateData.Feature_Ay.Count; i++)
                        {
                            Query = "update SVS.feature_ay set Ay" + i.ToString() + "='" + TemplateData.Feature_Ay[i - 1] + "'where SID='" + SID.ToString() + "';";
                            cmd.CommandText = Query;
                            cmd.ExecuteNonQuery();
                            try
                            {
                            }
                            catch (MySql.Data.MySqlClient.MySqlException ex)
                            {
                                MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                                CheckFeature = false;
                                break;
                            }
                        }
                        ////-------------------------------------------------------------------------------------------------------------//
                        ////-------------------------------------------------------------------------------------------------------------//
                        ////-- A-SID
                        Query = "insert into SVS.feature_a(SID) values('" + SID.ToString() + "');";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }
                        //-- A-寫資料
                        Query = "update SVS.feature_a set Len ='" + TemplateData.Feature_A.Count + "'where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        cmd.ExecuteNonQuery();
                        try
                        {
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                            CheckFeature = false;
                            break;
                        }

                        for (int i = 1; i <= TemplateData.Feature_A.Count; i++)
                        {
                            Query = "update SVS.feature_a set A" + i.ToString() + "='" + TemplateData.Feature_A[i - 1] + "'where SID='" + SID.ToString() + "';";
                            cmd.CommandText = Query;
                            cmd.ExecuteNonQuery();
                            try
                            { 
                            }
                            catch (MySql.Data.MySqlClient.MySqlException ex)
                            {
                                MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                                CheckFeature = false;
                                break;
                            }
                        }

                        //-- Final proceedure
                        MessageBox.Show(MultipleLangString.GetString("_createAccountSuccess"));
                        goBackLogin();
                        //-- 脫離迴圈
                        break;
                    }
                    //-- 建立使用者目錄
                    string path = ExeDirectory + @"\" + tbAccount.Text;
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
                }
            }
            
        }
        private void goBackLogin()
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
        /// <summary>
        /// Update File 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmUpdateFile_Tick(object sender, EventArgs e)
        {          
            if (FirstCheck)
            {
                FirstCheck = false;

                List<string> FileName = new List<string>();

                //-- 取得當前目錄夾下資料
                DirectoryInfo di = new DirectoryInfo(ExeDirectory);

                //-- 清空
                clbNewSignature.BeginUpdate();
                clbNewSignature.Items.Clear();
                clbNewSignature.EndUpdate();

                foreach (var fi in di.GetFiles("*.txt"))
                {
                    FileName.Add(fi.Name);
                }
                clbNewSignature.BeginUpdate();
                clbNewSignature.Items.AddRange(FileName.ToArray());
                clbNewSignature.EndUpdate();

                FileCount = FileName.Count;
            }
            else
            {
                //-- 取得當前目錄夾下資料
                DirectoryInfo di = new DirectoryInfo(ExeDirectory);
                if(FileCount != di.GetFiles("*.txt").Length)
                {
                    List<string> FileName = new List<string>();

                    //-- 清空
                    clbNewSignature.BeginUpdate();
                    clbNewSignature.Items.Clear();
                    clbNewSignature.EndUpdate();

                    foreach (var fi in di.GetFiles("*.txt"))
                    {
                        FileName.Add(fi.Name);
                    }

                    clbNewSignature.BeginUpdate();
                    clbNewSignature.Items.Clear();
                    clbNewSignature.Items.AddRange(FileName.ToArray());
                    clbNewSignature.EndUpdate();

                    FileCount = FileName.Count;
                }
                    
            }
        }

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDelete_Click(object sender, EventArgs e)
        {
            clbNewSignature.BeginUpdate();

            string Chosen = "";

            //for (int i = 0; i <= (clbNewSignature.Items.Count-1); i++)
            //{
            for (int i = 0; i <= clbNewSignature.CheckedItems.Count - 1; i++)
            {
                Chosen = Chosen + clbNewSignature.CheckedItems[i].ToString() + '\n';

                //if (clbNewSignature.GetItemChecked(i))
                //{

                    //-- 刪除選取TXT檔案名稱
                    System.IO.FileInfo fiTXT = new System.IO.FileInfo(ExeDirectory + @"\" + clbNewSignature.CheckedItems[i].ToString());

                    //-- 刪除選取JPG檔案名稱
                    String JPGFileName = clbNewSignature.CheckedItems[i].ToString();
                    JPGFileName = JPGFileName.Remove(25, 3);
                    JPGFileName = JPGFileName + "jpg";

                    System.IO.FileInfo fiJPG = new System.IO.FileInfo(ExeDirectory + @"\" + JPGFileName.ToString());

                    try
                    {
                        //-- 刪除檔案
                        fiJPG.Delete();
                        fiTXT.Delete();
                    }
                    catch (System.IO.IOException err)
                    {
                        MessageBox.Show(err.Message);
                    }

                    //clbNewSignature.Items.RemoveAt(i);
                    //}               
            }

            while (clbNewSignature.CheckedItems.Count > 0)
            {
                clbNewSignature.Items.Remove(clbNewSignature.Items[clbNewSignature.CheckedIndices[0]]);
            }

            MessageBox.Show(Chosen);

            clbNewSignature.ClearSelected();

            clbNewSignature.EndUpdate();
        }

        private void btClearScreen_Click(object sender, EventArgs e)
        {
            //-- 清除一些資訊
            lbTemplateInfo.Text = "";
            tbAccount.Text = "";
            tbPassword.Text = "";
            tbUserName.Text = "";
            cbSex.Text = "";         
        }

        private void btDeleteAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= clbNewSignature.Items.Count - 1; i++)
            {
                //-- 刪除選取TXT檔案名稱
                System.IO.FileInfo fiTXT = new System.IO.FileInfo(ExeDirectory + @"\" + clbNewSignature.Items[i].ToString());

                //-- 刪除選取JPG檔案名稱
                String JPGFileName = clbNewSignature.Items[i].ToString();
                JPGFileName = JPGFileName.Remove(25, 3);
                JPGFileName = JPGFileName + "jpg";

                System.IO.FileInfo fiJPG = new System.IO.FileInfo(ExeDirectory + @"\" + JPGFileName.ToString());

                try
                {
                    //-- 刪除檔案
                    fiJPG.Delete();
                    fiTXT.Delete();
                }
                catch (System.IO.IOException err)
                {
                    MessageBox.Show(err.Message);
                }
            }

            clbNewSignature.Items.Clear();
        }

        private void Entrollment_Load(object sender, EventArgs e)
        {
            this.Text = MultipleLangString.GetString("_entrollmentTitle");
            this.lbUserInfo_fixed.Text = MultipleLangString.GetString("_personalInformation");
            this.lbAccount_fixed.Text = MultipleLangString.GetString("_account");
            this.lbPassword_fixed.Text = MultipleLangString.GetString("_password");
            this.lbUserName_fixed.Text = MultipleLangString.GetString("_userName");
            this.lbSex_fixed.Text = MultipleLangString.GetString("_sex");
            this.cbSex.Items.Add(MultipleLangString.GetString("_male"));
            this.cbSex.Items.Add(MultipleLangString.GetString("_female"));
            this.btTemplateSelection.Text = MultipleLangString.GetString("_templateSelection");
            this.btTemplateConfirm.Text = MultipleLangString.GetString("_templateConfirm");
            this.btEntrollment.Text = MultipleLangString.GetString("_entrollment");
            this.btNewSignature.Text = MultipleLangString.GetString("_newSignature");
            this.btDelete.Text = MultipleLangString.GetString("_delete");
            this.btDeleteAll.Text = MultipleLangString.GetString("_deleteAll");
            this.btClearScreen.Text = MultipleLangString.GetString("_clearScreen");
        }
    }
}
