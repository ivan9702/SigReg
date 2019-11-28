using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreSetting
{
    class Program
    {
        static public string REGISTRY_SECTION = "SOFTWARE\\SVS";
        static public string SUPPORT_PRODUCTS_REG_NAME = "VS_PL";
        static public string SUPPORT_PRODUCTS_VALUE = "0F276F347315D9BB,D602E281F6835507,BFDD3E6C9E84E441,EFB741AA71E137B7,6FFB0DB3AB634105,5D15E9C9CA524503,C97C0916BB20140A,582E548EC5E66FFE,847C4AB6D818B03B,FD9A71A4AB6C3983,8AC2A5B16CF6B172,3B11D477E9755933,5707448C14173BD4,0237FAD4B3E56242,80798855AFC97F04,F3FFFDACA43F06AF,D4059E981B9A6ED1,DC7D51726737B51C,86EF7D02AE97DCD2";
        static public string TRIAL_DATE_REG_NAME = "VS_TD";
        static public string TRIAL_DATE_REG_VALUE = "8C55BFD281030864F51B25556F966DD1";

        static void Main(string[] args)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey software = hklm.OpenSubKey(REGISTRY_SECTION, true);
            if (software == null)
                software = hklm.CreateSubKey(REGISTRY_SECTION);
            software.SetValue(SUPPORT_PRODUCTS_REG_NAME, SUPPORT_PRODUCTS_VALUE);
            software.SetValue(TRIAL_DATE_REG_NAME, TRIAL_DATE_REG_VALUE);    
        }
    }
}
