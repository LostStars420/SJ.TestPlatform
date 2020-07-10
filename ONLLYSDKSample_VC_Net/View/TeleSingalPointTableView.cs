using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sojo.TestPlatform.PlatformModel.DatabaseHelper;

namespace Sojo.TestPlatform.ControlPlatform.View
{
    public partial class TeleSingalPointTableView : UserControl
    {
        public TeleSingalPointTableView()
        {
            InitializeComponent();
            string dbPath = @"Database\das.db";
            SQLiteHelper.Connect(dbPath);
            DataTable dtMeter = SQLiteHelper.ReadTable("TeleSingalTable");
            dataGridViewSingalTable.DataSource = dtMeter;
            SQLiteHelper.Close();
            //设置表头
            var singalHeaderText = new string[5] { "序号", "名称", "点号", "值", "说明" };
            var singalDatabaseName = new string[5] { "Index", "Name", "PointNumber", "Value", "Comment" };
            for (int index = 0; index < singalHeaderText.Length; index++)
            {
                var currentName = singalDatabaseName[index];
                dataGridViewSingalTable.Columns[currentName].HeaderText = singalHeaderText[index];
            }
            //某些列不可改
            for (int columnIndex = 0; columnIndex < dataGridViewSingalTable.Columns.Count; columnIndex++)
            {
                if (columnIndex == 0 || columnIndex == 1)
                {
                    dataGridViewSingalTable.Columns[columnIndex].ReadOnly = true;
                }
            }
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

        public DataGridView DataGridViewSingalTable
        {
            get
            {
                return dataGridViewSingalTable;
            }
            set
            {
                dataGridViewSingalTable = value;
            }
        }
    }
}
