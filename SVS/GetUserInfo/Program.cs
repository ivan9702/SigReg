using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;
using System.Security.Cryptography;

namespace GetUserInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            string macAdress = "";
            string serialnumber = "";
            string output = "";
            string output2 = "";
            string output3 = "";
            
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
            Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey os = start.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography");
            serialnumber = os.GetValue("MachineGuid").ToString();

            string username = Environment.UserName;

            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            byte[] plainTextData = Encoding.Unicode.GetBytes(macAdress);
            byte[] keyData = MD5.ComputeHash(Encoding.Unicode.GetBytes("viewsonic"));
            byte[] IVData = MD5.ComputeHash(Encoding.Unicode.GetBytes("svs"));
            ICryptoTransform transform = AES.CreateEncryptor(keyData, IVData);
            byte[] outputData = transform.TransformFinalBlock(plainTextData, 0, plainTextData.Length);
            output = Convert.ToBase64String(outputData);

            byte[] plainTextData2 = Encoding.Unicode.GetBytes(serialnumber);
            byte[] outputData2 = transform.TransformFinalBlock(plainTextData2, 0, plainTextData2.Length);
            output2 = Convert.ToBase64String(outputData2);

            byte[] plainTextData3 = Encoding.Unicode.GetBytes(username);
            byte[] outputData3 = transform.TransformFinalBlock(plainTextData3, 0, plainTextData3.Length);
            output3 = Convert.ToBase64String(outputData3);
            using (StreamWriter sw = new StreamWriter("userInfo.txt"))   //小寫TXT     
            {
                // Add some text to the file.
                sw.WriteLine(output);
                sw.WriteLine(output2);
                sw.WriteLine(output3);
                sw.Close();
            }            
        }
    }
}
