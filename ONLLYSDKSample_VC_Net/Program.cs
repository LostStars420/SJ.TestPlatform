﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Sojo.TestPlatform.ControlPlatform
{
    public static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}