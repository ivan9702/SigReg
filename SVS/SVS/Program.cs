using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SVS
{
    
    static class Program
    {
        static FigureContainer test;
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            test = new FigureContainer();
            
            Application.Run(test);
           
        }
    }
}
