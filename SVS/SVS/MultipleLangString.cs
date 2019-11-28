using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace SVS
{
    class MultipleLangString
    {
        //static string m_Language = "";
        public static string m_Language = "";
        //static string LANG_KEYNAME = "lang_viewsign";

        static public string GetString(string str)
        {
           
            if (m_Language == "")           // language not assigned yet, assign it
            {
                //try
                //{
                    /*Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
                    Microsoft.Win32.RegistryKey programName = start.OpenSubKey(SVS.FigureContainer.REGISTRY_SECTION_64BITS);

                    if (programName == null)
                    {
                        programName = start.OpenSubKey(SVS.FigureContainer.REGISTRY_SECTION_32BITS);
                    }
                    else
                    {
                        if (programName.GetValue(LANG_KEYNAME) == null)
                        {
                            programName.Close();
                            programName = start.OpenSubKey(SVS.FigureContainer.REGISTRY_SECTION_32BITS);        // change to 32bits if not found in 64bits
                        }
                    }*/

                    // default use current OS language
                    m_Language = Thread.CurrentThread.CurrentCulture.Name;
                    // if found in registry, change the language
                   /* object value = "";
                    if ((value = programName.GetValue(LANG_KEYNAME)) != null)
                    {
                        string reg_lang = value.ToString();
                        if (reg_lang == "ENG")
                            m_Language = "en";
                        else if (reg_lang == "CHS")
                            m_Language = "zh-CN";
                        else if (reg_lang == "CHT")
                            m_Language = "zh-TW";
                    }
                    programName.Close();
                }
                catch
                {
                    m_Language = Thread.CurrentThread.CurrentCulture.Name;
                }*/
            }

            string ret_str = "";

            try
            {
                ResourceManager res_man;    // declare Resource manager to access to specific cultureinfo

                if (m_Language == "zh-TW")
                {
                    res_man = new ResourceManager("SVS.Strings.Resource_zh_tw", typeof(Strings.Resource_zh_tw).Assembly);
                }
                else if (m_Language == "zh-CN")
                {
                    res_man = new ResourceManager("SVS.Strings.Resource_zh_cn", typeof(Strings.Resource_zh_cn).Assembly);
                }
                else        // default English
                {
                    res_man = new ResourceManager("SVS.Strings.Resource_en", typeof(Strings.Resource_en).Assembly);
                }

                CultureInfo cul;        // declare culture info
                cul = CultureInfo.CreateSpecificCulture(m_Language);
                ret_str = res_man.GetString(str, cul);
            }
            catch { }

            return ret_str;
        }
    }
}
