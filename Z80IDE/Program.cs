using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Z80IDE
{
    static class Program
    {

        public static Form MainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new Form1();
            Application.Run(MainForm);
        }
    }
}
