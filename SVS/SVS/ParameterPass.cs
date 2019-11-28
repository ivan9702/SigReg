using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signature;

namespace SVS
{
    class ParameterPass
    {
        //--  登入成功旗標
        public static bool MainFugure_flag = false;
        //-- 登入成功後，擷取之樣板
        public static SVSystem Template = new SVSystem();
        //-- 登入成功後，擷取之安全分數
        public static double[] Score = new double[2];

        //-- 保存父目錄
        public static string ParentDirectory = "";

    }
}
