using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sojo.TestPlatform.ControlPlatform.View
{
    public partial class StateSwitch : UserControl
    {
        public StateSwitch()
        {
            InitializeComponent();
        }


        public GroupBox GroupBoxContainer
        {
            get
            {
                return groupBox;
            }
            set
            {
                groupBox = value;
            }
        }


        public DataGridView DatagridView
        {
            get
            {
                return dataGridViewStateSwitch;
            }
            set
            {
                dataGridViewStateSwitch = value;
            }
        }

        public TextBox TimeT
        {
            get
            {
                return textBoxT;
            }
            set
            {
                textBoxT = value;
            }
        }
    }

}
