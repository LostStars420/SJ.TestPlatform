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
    public partial class TeleMeterPointTableView : UserControl
    {
        public TeleMeterPointTableView()
        {
            InitializeComponent();
            string dbPath = @"Database\das.db";
            SQLiteHelper.Connect(dbPath);
            DataTable dtMeter = SQLiteHelper.ReadTable("TeleMeterTable");
            dataGridViewMeterTable.DataSource = dtMeter;
            SQLiteHelper.Close();
            //设置表头
            var headerText = new string[6] { "序号", "名称", "点号", "值", "单位", "说明" };
            var databaseName = new string[6] { "Index", "Name", "PointNumber", "Value", "Unit", "Comment" };
            for (int index = 0; index < headerText.Length; index++)
            {
                var currentName = databaseName[index];
                dataGridViewMeterTable.Columns[currentName].HeaderText = headerText[index];
            }
            //某些列不可改
            for (int columnIndex = 0; columnIndex < dataGridViewMeterTable.Columns.Count; columnIndex++)
            {
                if (columnIndex == 0 || columnIndex == 1 || columnIndex == 4)
                {
                    dataGridViewMeterTable.Columns[columnIndex].ReadOnly = true;
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

        public DataGridView DataGridViewMeterTable
        {
            get
            {
                return dataGridViewMeterTable;
            }
            set
            {
                dataGridViewMeterTable = value;
            }
        }

    }
}
