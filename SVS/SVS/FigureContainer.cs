#define MYTEST
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SVS
{
    public partial class FigureContainer : Form
    {
        //-- 紀錄初始表單大小
        Size DBformSize = new Size();
        Size EntrollformSize = new Size();

       

        MainFigure MainProcess = new MainFigure();
        string username = "";
        string m_Language = Thread.CurrentThread.CurrentCulture.Name;
        Database DBform = new Database();
        Entrollment Entrollform = new Entrollment();

        public FigureContainer()
        {
            InitializeComponent();
#if MYTEST

            //MultipleLangString.m_Language = "zh-TW"; 
            MultipleLangString.m_Language = m_Language;

            //if (m_Language == "zh-TW")
            //{
            //    radioButton1.Checked = true;
            //}
            //else if(m_Language == "en-US")
            //{
            //    radioButton2.Checked = true;
            //}

#endif
               


            bool userInfo = CheckUserInfo();
#if MYTEST
    userInfo = true;
#endif
            if (!userInfo)
            {
                MessageBox.Show(MultipleLangString.GetString("_usererror") + "：" + username, MultipleLangString.GetString("_warning"));
                Process p = Process.GetCurrentProcess();
                p.Kill();
            }
            try
            {
                string[] product = ReadProductID();

                // Binding Product ID (9012, 8808, 8415, 9003, 9014, 8534).
                String[] devicePathName = new String[128];
                Guid HID_GUID = new Guid("{0x4D1E55B2, 0xF16F, 0x11CF, { 0x88, 0xCB, 0x00, 0x11, 0x11, 0x00, 0x00, 0x30}}");

                bool devicefound = false;
               
                for (int i = 0; i < product.Length; i++)
                {
                    if (product[i] == "")
                        continue;

                    if (FindDeviceFromGuid(HID_GUID, ref devicePathName, product[i]))
                    {
                       
                        devicefound = true;
                        break;
                    }
                }

                // Binding computer name has "viewsonic", "Model" is { VS16270 }
                if (!devicefound)
                {
                    string INFO_REGISTRY_SECTION = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\OEMInformation";

                    object value;

                    Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
                    Microsoft.Win32.RegistryKey programName = start.OpenSubKey(INFO_REGISTRY_SECTION);

                    if (programName != null)
                    {
                        if ((value = programName.GetValue("Manufacturer")) != null)
                        {
                            if (value.ToString().ToLower().IndexOf("viewsonic") >= 0)
                            {
                                if ((value = programName.GetValue("Model")) != null)
                                {
                                    for (int i = 0; i < product.Length; i++)
                                    {
                                        if (value.ToString() == product[i])
                                        {
                                            devicefound = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        programName.Close();
                    }
                }

#if MYTEST
    devicefound = true;
#endif
                if (!devicefound)
                {
                    MessageBox.Show(MultipleLangString.GetString("_connect"), MultipleLangString.GetString("_warning"));
                    Process p = Process.GetCurrentProcess();
                    p.Kill();
                }
#if MYTEST
                Global.TrialFlag = false;
#endif
                if (Global.TrialFlag)       // trial build, it needs to check trial date
                {
                    string[] trialdate = ReadTrialDate();
                    DateTime dt = DateTime.Now;
                    int trial_year = Convert.ToInt32(trialdate[0]);
                    int trial_month = Convert.ToInt32(trialdate[1]);
                    int trial_day = Convert.ToInt32(trialdate[2]);

                    if ((dt.Year > trial_year)
                    || ((dt.Year == trial_year) && (dt.Month > trial_month))
                    || ((dt.Year == trial_year) && (dt.Month == trial_month) && (dt.Day > trial_day)))
                    {
                        MessageBox.Show(MultipleLangString.GetString("_trialexpired"), MultipleLangString.GetString("_info"));
                        Process p = Process.GetCurrentProcess();
                        p.Kill();
                    }
                }
            }
            catch
            {
                MessageBox.Show(MultipleLangString.GetString("_trialexpired"), MultipleLangString.GetString("_info"));
                Process p = Process.GetCurrentProcess();
                p.Kill();
            }
            
            //-- 初始設定，主功能關閉
            tmLoop.Enabled = true;
            //tsmiApplication.Enabled = false;

            //-- 初始畫面為登入頁面
          
            DBform.MdiParent = this;

            //-- open
            DBformSize = DBform.Size;
            this.Size = DBform.Size;
            DBform.WindowState = FormWindowState.Maximized;
            DBform.Show();

            DBform.getSize(DBformSize, EntrollformSize);

            DBform.getFrom(this);
            MainProcess.getFrom(this);
            Entrollform.getFrom(this);
        }

        static public string REGISTRY_SECTION = "SOFTWARE\\SVS";
        static public string SUPPORT_PRODUCTS_VALUE = "0F276F347315D9BB,D602E281F6835507,BFDD3E6C9E84E441,EFB741AA71E137B7,6FFB0DB3AB634105,5D15E9C9CA524503,C97C0916BB20140A,582E548EC5E66FFE,847C4AB6D818B03B,FD9A71A4AB6C3983,8AC2A5B16CF6B172,3B11D477E9755933,5707448C14173BD4,0237FAD4B3E56242,80798855AFC97F04,F3FFFDACA43F06AF,D4059E981B9A6ED1,DC7D51726737B51C,86EF7D02AE97DCD2";
        static public string TRIAL_DATE_REG_VALUE = "D70ED0BF1C2B1FB0F7380E217CDEE3F2";

        private Boolean CheckUserInfo()
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey software = hklm.OpenSubKey(REGISTRY_SECTION, true);
            if (software == null)
                software = hklm.CreateSubKey(REGISTRY_SECTION);
            if (software.GetValue(SUPPORT_PRODUCTS_REG_NAME) == null)
                software.SetValue(SUPPORT_PRODUCTS_REG_NAME, SUPPORT_PRODUCTS_VALUE);
            if (software.GetValue(TRIAL_DATE_REG_NAME) == null)
                software.SetValue(TRIAL_DATE_REG_NAME, TRIAL_DATE_REG_VALUE);    

            string macAdress = "";
            string serialnumber = "";
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryMAC = query.Get();
            foreach (ManagementObject mac in queryMAC)
            {
                if (mac["IPEnabled"].ToString() == "True")
                {
                    macAdress = mac["MacAddress"].ToString();
                    break;
                }
            }
            //string key = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Cryptography";
            //serialnumber = (string)Registry.GetValue(key, "MachineGuid", (object)"default");

            Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey os = start.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography");
            var test = os.GetValue("MachineGuid");
            serialnumber = test.ToString();

            username = Environment.UserName;

            string resource_data = Properties.Resources.userInfo;
            List<string> words = resource_data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string str1 = words[0];
            string str2 = words[1];
            string str3 = words[2];
            str1 = decrypt(Convert.FromBase64String(str1));
            str2 = decrypt(Convert.FromBase64String(str2));
            str3 = decrypt(Convert.FromBase64String(str3));

            if (macAdress.Equals(str1) && serialnumber.Equals(str2) && username.Equals(str3))
                return true;
            else
            {
                username = str3;
                return false;
            }
        }

        private string decrypt(byte[] cipherTextData)
        {
            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            byte[] keyData = MD5.ComputeHash(Encoding.Unicode.GetBytes("viewsonic"));
            byte[] IVData = MD5.ComputeHash(Encoding.Unicode.GetBytes("svs"));
            ICryptoTransform transform = AES.CreateDecryptor(keyData, IVData);
            byte[] outputData = transform.TransformFinalBlock(cipherTextData, 0, cipherTextData.Length);
            return Encoding.Unicode.GetString(outputData);
        }

        private Boolean FindDeviceFromGuid(Guid myGuid, ref String[] devicePathName, string str)
        {
            Int32 bufferSize = 0;
            IntPtr deviceInfoSet = new IntPtr();
            Boolean lastDevice = false;
            DeviceManagement.SpDeviceInterfaceData myDeviceInterfaceData = new DeviceManagement.SpDeviceInterfaceData();
            try
            {
                Int32 memberIndex;
                deviceInfoSet = DeviceManagement.SetupDiGetClassDevs(ref myGuid, IntPtr.Zero, IntPtr.Zero, DeviceManagement.DigcfPresent | DeviceManagement.DigcfDeviceinterface);
                bool deviceFound = false;
                memberIndex = 0;
                myDeviceInterfaceData.CbSize = Marshal.SizeOf(myDeviceInterfaceData);
                do
                {
                    Boolean success = DeviceManagement.SetupDiEnumDeviceInterfaces
                        (deviceInfoSet,
                         IntPtr.Zero,
                         ref myGuid,
                         memberIndex,
                         ref myDeviceInterfaceData);
                    // Find out if a device information set was retrieved.
                    if (!success)
                    { lastDevice = true; }
                    else
                    {
                        DeviceManagement.SetupDiGetDeviceInterfaceDetail
                            (deviceInfoSet,
                             ref myDeviceInterfaceData,
                             IntPtr.Zero,
                             0,
                             ref bufferSize,
                             IntPtr.Zero);

                        // Allocate memory for the SP_DEVICE_INTERFACE_DETAIL_DATA structure using the returned buffer size.
                        IntPtr detailDataBuffer = Marshal.AllocHGlobal(bufferSize);
                        // Store cbSize in the first bytes of the array. Adjust for 32- and 64-bit systems.
                        Int32 cbsize;

                        if (IntPtr.Size == 4)
                        {
                            cbsize = 4 + Marshal.SystemDefaultCharSize;
                        }
                        else
                        {
                            cbsize = 8;
                        }

                        Marshal.WriteInt32(detailDataBuffer, cbsize);

                        DeviceManagement.SetupDiGetDeviceInterfaceDetail
                            (deviceInfoSet,
                             ref myDeviceInterfaceData,
                             detailDataBuffer,
                             bufferSize,
                             ref bufferSize,
                             IntPtr.Zero);

                        IntPtr pDevicePathName = new IntPtr(detailDataBuffer.ToInt64() + 4);

                        devicePathName[memberIndex] = Marshal.PtrToStringAuto(pDevicePathName);

                        if (detailDataBuffer != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(detailDataBuffer);
                        }
                        if (devicePathName[memberIndex].ToLower().IndexOf(str.ToLower()) != -1)
                        {
                            deviceFound = true;
                        }
                    }
                    memberIndex = memberIndex + 1;
                }
                while (!lastDevice);

                return deviceFound;
            }
            finally
            {
                if (deviceInfoSet != IntPtr.Zero)
                {
                    DeviceManagement.SetupDiDestroyDeviceInfoList(deviceInfoSet);
                }
            }
        }

        static public string REGISTRY_SECTION_64BITS = "SOFTWARE\\SVS";
        static public string REGISTRY_SECTION_32BITS = "SOFTWARE\\Wow6432Node\\SVS";
        static public string SUPPORT_PRODUCTS_KEY = "VS_Prods";
        static public string SUPPORT_PRODUCTS_REG_NAME = "VS_PL";
        static public string TRIAL_DATE_KEY = "VS_Trial";
        static public string TRIAL_DATE_REG_NAME = "VS_TD";

        private string[] ReadProductID()
        {
            Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey programName = start.OpenSubKey(REGISTRY_SECTION_64BITS);
            string[] nothing = { "" };

            if (programName == null)
            {
                programName = start.OpenSubKey(REGISTRY_SECTION_32BITS);
            }
            else
            {
                if (programName.GetValue(SUPPORT_PRODUCTS_REG_NAME) == null)
                {
                    programName.Close();
                    programName = start.OpenSubKey(REGISTRY_SECTION_32BITS);        // change to 32bits if not found in 64bits
                }
            }

            if (programName != null)
            {
                object value = "";
                if ((value = programName.GetValue(SUPPORT_PRODUCTS_REG_NAME)) != null)
                {
                    string[] product = value.ToString().Split(',');
                    for (int i = 0; i < product.Length; i++)
                    {
                        product[i] = Decrypt(product[i], SUPPORT_PRODUCTS_KEY);
                    }
                    programName.Close();
                    return product;
                }
                else
                {
                    programName.Close();
                    return nothing;
                }
            }
            else
            {
                return nothing;
            }
        }

        private string[] ReadTrialDate()
        {
            Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey programName = start.OpenSubKey(REGISTRY_SECTION_64BITS);
            string[] nothing = { "" };

            if (programName == null)
            {
                programName = start.OpenSubKey(REGISTRY_SECTION_32BITS);
            }
            else
            {
                if (programName.GetValue(TRIAL_DATE_REG_NAME) == null)
                {
                    programName.Close();
                    programName = start.OpenSubKey(REGISTRY_SECTION_32BITS);        // change to 32bits if not found in 64bits
                }
            }

            if (programName != null)
            {
                object value = "";
                if ((value = programName.GetValue(TRIAL_DATE_REG_NAME)) != null)
                {
                    string str = Decrypt(value.ToString(), TRIAL_DATE_KEY);
                    string[] date = str.Split('/');
                    programName.Close();
                    return date;
                }
                else
                {
                    programName.Close();
                    return nothing;
                }
            }
            else
            {
                return nothing;
            }
        }

        //解密方法  
        static public string Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //Put  the  input  string  into  the  byte  array  
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            //建立加密對象的密鑰和偏移量，此值重要，不能修改  
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //Flush  the  data  through  the  crypto  stream  into  the  memory  stream  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //Get  the  decrypted  data  back  from  the  memory  stream  
            //建立StringBuild對象，CreateDecrypt使用的是流對象，必須把解密後的文本變成流對像  
            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

       private void tsmiDatabase_Click(object sender, EventArgs e)
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
                
                this.Size = DBformSize;
                Application.OpenForms["Database"].WindowState = FormWindowState.Maximized;
                ParameterPass.MainFugure_flag = false;
            }
            else
            {
                //-- open
                Database DBform = new Database();
                DBform.MdiParent = this;

                //-- 設定呈現大小
                DBformSize = DBform.Size;
                this.Size = DBform.Size;
                DBform.WindowState = FormWindowState.Maximized;

                DBform.Show();
                ParameterPass.MainFugure_flag = false;
                if (tsmiDatabase.Text == MultipleLangString.GetString("_logout"))
                    tsmiDatabase.Text = MultipleLangString.GetString("_login");
            }
        }

        //private void tsmiEntrollment_Click(object sender, EventArgs e)
        public void tsmiEntrollment_Click(object sender, EventArgs e)
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
                Entrollform.MdiParent = this;

                //-- 設定呈現大小
                EntrollformSize = Entrollform.Size;
                this.Size = Entrollform.Size;
                Entrollform.WindowState = FormWindowState.Maximized;

                Entrollform.Show();
            }

  

        }

        private void tsmiApplication_Click(object sender, EventArgs e)
        {
        

        }

        private void tmLoop_Tick(object sender, EventArgs e)
        {
            //-- 偵測到帳號登入時，切換主功能頁面，其餘功能關閉
            if(ParameterPass.MainFugure_flag == true)
            {
                tsmiDatabase.Enabled = true;
                tsmiEntrollment.Enabled = false;
               // tsmiApplication.Enabled = true;
                tsmiDatabase.Text = MultipleLangString.GetString("_logout");
                //radioButton1.Visible = false;
                //radioButton2.Visible = false;

            }
            else
            {
                tsmiDatabase.Enabled = true;
                tsmiEntrollment.Enabled = true;
               // tsmiApplication.Enabled = false;
            }
        }

        public class Global
        {
            public static bool TrialFlag = true; // Trial : true
        }

        private void FigureContainer_Load(object sender, EventArgs e)
        {
            this.tsmiDatabase.Text = MultipleLangString.GetString("_login");
            this.tsmiEntrollment.Text = MultipleLangString.GetString("_entrollment");
           // this.tsmiApplication.Text = MultipleLangString.GetString("_mainFunction");
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {

            //if (radioButton2.Checked)
            //{
            //    // m_Language = "en-US";
            //    MultipleLangString.m_Language = "en-US";
            //    toolBarString_Update();
            //}
            //else if (radioButton1.Checked)
            //{
            //    // m_Language = "zh-TW";
            //    MultipleLangString.m_Language = "zh-TW";
            //    toolBarString_Update();
            //}
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //if (radioButton1.Checked)
            //    return;
            //m_Language = "zh-TW";
            //MultipleLangString.m_Language = m_Language;
        }

        void toolBarString_Update()
        {

            this.tsmiDatabase.Text = MultipleLangString.GetString("_login");
            this.tsmiEntrollment.Text = MultipleLangString.GetString("_entrollment");
            DBform.DatabaseStringLoad();
        }


    }
}
