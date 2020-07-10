using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FTU.Monitor;
using FTU.Monitor.ViewModel;

namespace SOJO.CheckPlatform.Test
{
    public partial class Form1 : Form
    {
        private void  Test()
        {
            var mw = new MainWindow();
            CommunicationViewModel cv =  mw.GetModel();
            cv.OpenLink("192.168.60.100", 2404);
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Test();
        }
    }
}
