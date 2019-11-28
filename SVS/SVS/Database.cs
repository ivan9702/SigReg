using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Signature;
using System.Threading;

namespace SVS
{
   
    public partial class Database : Form
    {
        Form FC;

        Size DBformSize = new Size();
        Size EntrollformSize = new Size();

        string m_Language = Thread.CurrentThread.CurrentCulture.Name;

        //-- 樣板訊號 & 分數
        double[] Score = new double[2];
        SVSystem TemplateData = new SVSystem();

        //-- MySQL connect information
        string dbHost = "127.0.0.1";
        string dbPort = "3306";
        string dbUser = "root";
        string dbPass = "";
        string dbName = "SVS";

        //-- 確認旗標
        bool Connect_flag = true;
        bool Login_flag = false;

        //-- Database used
        int SID = 0;

        public Database()
        {
            InitializeComponent();
            //MultipleLangString.m_Language = "zh-TW"; 
            MultipleLangString.m_Language = m_Language;
            if (m_Language == "zh-TW")
            {
                radioButton1.Checked = true;
            }
            else if (m_Language == "en-US")
            {
                radioButton2.Checked = true;
            }


        }

        /// <summary>
        /// 登入按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEnter_Click(object sender, EventArgs e)
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
                lbInfo.Text = "Error";

                switch (ex.Number)
                {
                    case 0:
                        lbInfo.Text = (MultipleLangString.GetString("_connectfail"));
                        break;
                    case 1045:
                        lbInfo.Text = (MultipleLangString.GetString("_relogin"));
                        break;
                    default:
                        lbInfo.Text = ex.Message;
                        break;
                }
            }

            //-- 伺服器連線成功
            if (Connect_flag)
            {
                Login_flag = false;

                //-- 命令與伺服器連線，通過命令來對資料庫進行存取
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                //-- 進行使用者帳號密碼認證           
                try
                {
                    Query = "select * from SVS.account where Uid='" + tbUid.Text + "'and Password='" + tbPass.Text + "';";
                    cmd.CommandText = Query;
                    MySqlDataReader Account = cmd.ExecuteReader();

                    //-- 如果沒有資料,顯示沒有資料的訊息
                    if (!Account.HasRows)
                    {
                        lbInfo.Text = (MultipleLangString.GetString("_relogin"));
                        Account.Close();
                    }
                    else
                    {
                        //-- 使用者登入成功
                        Login_flag = true;
                        Account.Close();
                        lbInfo.Text = MultipleLangString.GetString("_loginSuccess");

                        //-- 讀取使用者名稱並且顯示出來
                        Query = "select UserName from SVS.account where Uid='" + tbUid.Text + "'and Password='" + tbPass.Text + "';";
                        cmd.CommandText = Query;
                        MySqlDataReader UserName = cmd.ExecuteReader();
                        while (UserName.Read())
                        {
                            lbInfo.Text = lbInfo.Text + '\n' + UserName.GetString(0);
                            Console.WriteLine("DBDBDBDBDB USER NAME: " + UserName.GetString(0));
                            //-- 記錄使用者名稱
                            TemplateData.FileName = UserName.GetString(0);
                        }
                        UserName.Close();

                        //-- 讀取最低建議分數並且顯示出來
                        Query = "select ScoreLow from SVS.account where Uid='" + tbUid.Text + "'and Password='" + tbPass.Text + "';";
                        cmd.CommandText = Query;
                        MySqlDataReader Score_L = cmd.ExecuteReader();
                        while (Score_L.Read())
                        {
                            //-- 讀取最低建議分數
                            Score[0] = Score_L.GetDouble(0);

                        }
                        Score_L.Close();

                        //-- 讀取建議分數並且顯示出來
                        Query = "select ScoreHigh from SVS.account where Uid='" + tbUid.Text + "'and Password='" + tbPass.Text + "';";
                        cmd.CommandText = Query;
                        MySqlDataReader Score_H = cmd.ExecuteReader();
                        while (Score_H.Read())
                        {
                            //-- 讀取最低建議分數
                            Score[1] = Score_H.GetDouble(0);

                        }
                        Score_H.Close();

                        //-- 讀取簽名花費時間並且顯示出來
                        Query = "select CostTime from SVS.account where Uid='" + tbUid.Text + "'and Password='" + tbPass.Text + "';";
                        cmd.CommandText = Query;
                        MySqlDataReader CostTime = cmd.ExecuteReader();
                        while (CostTime.Read())
                        {
                            TemplateData.CostTime = CostTime.GetDouble(0);
                            Console.WriteLine("DBDBDBDBDB CostTime: " + Convert.ToString(CostTime.GetDouble(0)));
                        }
                        CostTime.Close();

                        //-- 讀取SID
                        Query = "select SID from SVS.account where Uid='" + tbUid.Text + "'and Password='" + tbPass.Text + "';";
                        cmd.CommandText = Query;
                        MySqlDataReader SID_t = cmd.ExecuteReader();
                        while (SID_t.Read())
                        {
                            SID = SID_t.GetInt32(0);
                            //MessageBox.Show(SID.ToString());
                        }
                        SID_t.Close();

                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show("Error " + ex.Number + " : " + ex.Message);
                }


                if (Login_flag)
                {
                    int Len = 0;
                    MySqlDataReader DataReader;

                    //-- X, 讀資料長度
                    Query = "select Len from SVS.feature_x where SID='" + SID.ToString() + "';";
                    cmd.CommandText = Query;
                    DataReader = cmd.ExecuteReader();
                    while (DataReader.Read())
                    {
                        Len = DataReader.GetInt32(0);
                        //MessageBox.Show(Len.ToString());
                    }
                    DataReader.Close();

                    for (int i = 1; i <= Len; i++)
                    {
                        Query = "select X" + i.ToString() + " from SVS.feature_x where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        DataReader = cmd.ExecuteReader();
                        while (DataReader.Read())
                        {
                            TemplateData.Feature_X.Add(DataReader.GetDouble(0));
                            //MessageBox.Show(SID.ToString());
                        }
                        DataReader.Close();
                    }
                    //-------------------------------------------------------------------------------------------------------------//
                    //-------------------------------------------------------------------------------------------------------------//
                    //-- Y, 讀資料長度
                    Query = "select Len from SVS.feature_y where SID='" + SID.ToString() + "';";
                    cmd.CommandText = Query;
                    DataReader = cmd.ExecuteReader();
                    while (DataReader.Read())
                    {
                        Len = DataReader.GetInt32(0);
                        //MessageBox.Show(Len.ToString());
                    }
                    DataReader.Close();

                    for (int i = 1; i <= Len; i++)
                    {
                        Query = "select Y" + i.ToString() + " from SVS.feature_y where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        DataReader = cmd.ExecuteReader();
                        while (DataReader.Read())
                        {
                            TemplateData.Feature_Y.Add(DataReader.GetDouble(0));
                            //MessageBox.Show(SID.ToString());
                        }
                        DataReader.Close();
                    }
                    //-------------------------------------------------------------------------------------------------------------//
                    //-------------------------------------------------------------------------------------------------------------//
                    //-- P, 讀資料長度
                    Query = "select Len from SVS.feature_P where SID='" + SID.ToString() + "';";
                    cmd.CommandText = Query;
                    DataReader = cmd.ExecuteReader();
                    while (DataReader.Read())
                    {
                        Len = DataReader.GetInt32(0);
                        //MessageBox.Show(Len.ToString());
                    }
                    DataReader.Close();

                    for (int i = 1; i <= Len; i++)
                    {
                        Query = "select P" + i.ToString() + " from SVS.feature_p where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        DataReader = cmd.ExecuteReader();
                        while (DataReader.Read())
                        {
                            TemplateData.Feature_P.Add(DataReader.GetDouble(0));
                            //MessageBox.Show(SID.ToString());
                        }
                        DataReader.Close();
                    }
                    //-------------------------------------------------------------------------------------------------------------//
                    //-------------------------------------------------------------------------------------------------------------//
                    //-- Vx, 讀資料長度
                    Query = "select Len from SVS.feature_vx where SID='" + SID.ToString() + "';";
                    cmd.CommandText = Query;
                    DataReader = cmd.ExecuteReader();
                    while (DataReader.Read())
                    {
                        Len = DataReader.GetInt32(0);
                        //MessageBox.Show(Len.ToString());
                    }
                    DataReader.Close();

                    for (int i = 1; i <= Len; i++)
                    {
                        Query = "select Vx" + i.ToString() + " from SVS.feature_vx where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        DataReader = cmd.ExecuteReader();
                        while (DataReader.Read())
                        {
                            TemplateData.Feature_Vx.Add(DataReader.GetDouble(0));
                            //MessageBox.Show(SID.ToString());
                        }
                        DataReader.Close();
                    }
                    //-------------------------------------------------------------------------------------------------------------//
                    //-------------------------------------------------------------------------------------------------------------//
                    //-- Vy, 讀資料長度
                    Query = "select Len from SVS.feature_vy where SID='" + SID.ToString() + "';";
                    cmd.CommandText = Query;
                    DataReader = cmd.ExecuteReader();
                    while (DataReader.Read())
                    {
                        Len = DataReader.GetInt32(0);
                        //MessageBox.Show(Len.ToString());
                    }
                    DataReader.Close();

                    for (int i = 1; i <= Len; i++)
                    {
                        Query = "select Vy" + i.ToString() + " from SVS.feature_vy where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        DataReader = cmd.ExecuteReader();
                        while (DataReader.Read())
                        {
                            TemplateData.Feature_Vy.Add(DataReader.GetDouble(0));
                            //MessageBox.Show(SID.ToString());
                        }
                        DataReader.Close();
                    }
                    //-------------------------------------------------------------------------------------------------------------//
                    //-------------------------------------------------------------------------------------------------------------//
                    //-- Ax, 讀資料長度
                    Query = "select Len from SVS.feature_ax where SID='" + SID.ToString() + "';";
                    cmd.CommandText = Query;
                    DataReader = cmd.ExecuteReader();
                    while (DataReader.Read())
                    {
                        Len = DataReader.GetInt32(0);
                        //MessageBox.Show(Len.ToString());
                    }
                    DataReader.Close();

                    for (int i = 1; i <= Len; i++)
                    {
                        Query = "select Ax" + i.ToString() + " from SVS.feature_ax where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        DataReader = cmd.ExecuteReader();
                        while (DataReader.Read())
                        {
                            TemplateData.Feature_Ax.Add(DataReader.GetDouble(0));
                            //MessageBox.Show(SID.ToString());
                        }
                        DataReader.Close();
                    }
                    //-------------------------------------------------------------------------------------------------------------//
                    //-------------------------------------------------------------------------------------------------------------//
                    //-- Ay, 讀資料長度
                    Query = "select Len from SVS.feature_ay where SID='" + SID.ToString() + "';";
                    cmd.CommandText = Query;
                    DataReader = cmd.ExecuteReader();
                    while (DataReader.Read())
                    {
                        Len = DataReader.GetInt32(0);
                        //MessageBox.Show(Len.ToString());
                    }
                    DataReader.Close();

                    for (int i = 1; i <= Len; i++)
                    {
                        Query = "select Ay" + i.ToString() + " from SVS.feature_ay where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        DataReader = cmd.ExecuteReader();
                        while (DataReader.Read())
                        {
                            TemplateData.Feature_Ay.Add(DataReader.GetDouble(0));
                            //MessageBox.Show(SID.ToString());
                        }
                        DataReader.Close();
                    }
                    //-------------------------------------------------------------------------------------------------------------//
                    //-------------------------------------------------------------------------------------------------------------//
                    //-- A, 讀資料長度
                    Query = "select Len from SVS.feature_a where SID='" + SID.ToString() + "';";
                    cmd.CommandText = Query;
                    DataReader = cmd.ExecuteReader();
                    while (DataReader.Read())
                    {
                        Len = DataReader.GetInt32(0);
                        //MessageBox.Show(Len.ToString());
                    }
                    DataReader.Close();

                    for (int i = 1; i <= Len; i++)
                    {
                        Query = "select A" + i.ToString() + " from SVS.feature_a where SID='" + SID.ToString() + "';";
                        cmd.CommandText = Query;
                        DataReader = cmd.ExecuteReader();
                        while (DataReader.Read())
                        {
                            TemplateData.Feature_A.Add(DataReader.GetDouble(0));
                            //MessageBox.Show(SID.ToString());
                        }
                        DataReader.Close();
                    }

                    //-------------------------------------------------------------------------------------------------------------//
                    //-------------------------------------------------------------------------------------------------------------//

                    //-- 取得該帳號之訊號後，傳入比對程式中
                    ParameterPass.Score = Score;
                    ParameterPass.Template = TemplateData;

                    //-- 關閉其餘表單
                    Form[] aryf = this.Parent.FindForm().MdiChildren;
                    foreach (Form f in aryf)
                    {
                        if (f.Name != this.Name)
                            f.Close();
                    }

                    //-- 成功登入後，進入訊號比對畫面
                    MainFigure MainProcess = new MainFigure();
                    MainProcess.MdiParent = this.MdiParent;
                    MainProcess.WindowState = FormWindowState.Maximized;
                    this.MdiParent.Size = MainProcess.Size;
                    MainProcess.Show();

                    //-- 關閉其餘功能選項
                    ParameterPass.MainFugure_flag = true;

                    conn.Close();
                    this.Close();
                }
            }

            conn.Close();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tmRun_Tick(object sender, EventArgs e)
        {

        }

        private void dempEntrollment_Click(object sender, EventArgs e)
        {
            //-- 若已開啟登入頁面，關閉
            if (Application.OpenForms["Database"] as Database != null)
            {
                Application.OpenForms["Database"].Close();
            }

            //-- 開啟註冊頁面
            if (Application.OpenForms["Entrollment"] as Entrollment != null)
            {
                this.Size = EntrollformSize;

                Application.OpenForms["Entrollment"].WindowState = FormWindowState.Maximized;
            }
            else
            {
                // open
                Entrollment Entrollform = new Entrollment();
                //Entrollform.MdiParent = ;

                //-- 設定呈現大小
                EntrollformSize = Entrollform.Size;
                this.Size = Entrollform.Size;
                Entrollform.WindowState = FormWindowState.Maximized;

                Entrollform.Show();
            }

        }

        public void getSize(Size DB, Size Enroll)
        {
            Size DBformSize = DB;
            Size EntrollformSize = Enroll;
        }
        private void Database_Load(object sender, EventArgs e)
        {
            this.Text = MultipleLangString.GetString("_loginTitle");
            this.lbUid_fixed.Text = MultipleLangString.GetString("_account");
            this.lbpass_fixed.Text = MultipleLangString.GetString("_password");
            this.btEnter.Text = MultipleLangString.GetString("_login");
            this.btExit.Text = MultipleLangString.GetString("_exit");
            this.button1.Text = MultipleLangString.GetString("_entrollment");

        }
        public void DatabaseStringLoad()
        {
            this.Text = MultipleLangString.GetString("_loginTitle");
            this.lbUid_fixed.Text = MultipleLangString.GetString("_account");
            this.lbpass_fixed.Text = MultipleLangString.GetString("_password");
            this.btEnter.Text = MultipleLangString.GetString("_login");
            this.btExit.Text = MultipleLangString.GetString("_exit");
            this.button1.Text = MultipleLangString.GetString("_entrollment");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //-- 若已開啟登入頁面，關閉
            if (Application.OpenForms["Database"] as Database != null)
            {
                Application.OpenForms["Database"].Close();
            }

            //-- 開啟註冊頁面
            if (Application.OpenForms["Entrollment"] as Entrollment != null)
            {
                FC.Size = EntrollformSize;

                Application.OpenForms["Entrollment"].WindowState = FormWindowState.Maximized;
            }
            else
            {
                // open
                Entrollment Entrollform = new Entrollment();
                Entrollform.MdiParent = FC;

                ////-- 設定呈現大小
                EntrollformSize = Entrollform.Size;
                FC.Size = Entrollform.Size;
                Entrollform.WindowState = FormWindowState.Maximized;

                //Entrollform.Size = new System.Drawing.Size(1028, 538);

                Entrollform.Show();
            }
        }

        public void getFrom(Form f)
        {
            FC = f;
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton2.Checked)
            {
                // m_Language = "en-US";
                MultipleLangString.m_Language = "en-US";
                toolBarString_Update();
            }
            else if (radioButton1.Checked)
            {
                // m_Language = "zh-TW";
                MultipleLangString.m_Language = "zh-TW";
                toolBarString_Update();
            }
        }

        void toolBarString_Update()
        {
            this.DatabaseStringLoad();
        }

    }
}
