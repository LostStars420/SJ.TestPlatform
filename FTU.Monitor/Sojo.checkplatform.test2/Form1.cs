using Sojo.Checkplatform.libcollect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sojo.checkplatform.test2
{
    
    public partial class Form1 : Form
    {
        //连接，总召唤委托的对象
        private static IEC104Interface iec;
        //获取列表委托的对象
        private static IEC104InterfaceOut iecOut;

        public static IEC104Interface GetIecInterface()
        {
            if (iec == null)
            {
                iec = new IEC104Interface();
                return iec;
            }
            return iec;
        }

        public static IEC104InterfaceOut GetIecInterfaceOut()
        {
            if (iecOut == null)
            {
                iecOut = new IEC104InterfaceOut();
                return iecOut;
            }
            return iecOut;
        }
        public Form1()
        {
            InitializeComponent();
            GetIecInterfaceOut();
            iecOut.GiComplted = showStr;
        }
       

        bool showStr(List<Tuple<int, float>> m)
        {
            MessageBox.Show(m.ToString());
            return true;
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            iec.TestAct("192.168.60.100", 2404);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            iec.GiAct("");
        }
    }
}
