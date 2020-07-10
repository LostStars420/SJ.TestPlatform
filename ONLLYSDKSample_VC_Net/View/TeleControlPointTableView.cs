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
    public partial class TeleControlPointTableView : UserControl
    {
        public TeleControlPointTableView()
        {
            InitializeComponent();
            string dbPath = @"Database\das.db";
            SQLiteHelper.Connect(dbPath);
            DataTable dtMeter = SQLiteHelper.ReadTable("TeleControlTable");
            dataGridViewControlTable.DataSource = dtMeter;
            SQLiteHelper.Close();
            //添加一列CheckBox选择
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "select";
            checkBoxColumn.HeaderText = "选择";
            checkBoxColumn.TrueValue = true;
            checkBoxColumn.FalseValue = false;
            dataGridViewControlTable.Columns.Insert(dataGridViewControlTable.Columns.Count, checkBoxColumn);
            //设置表头
            var controlHeaderText = new string[4] { "序号", "名称", "点号", "说明" };
            var controlDatabaseName = new string[4] { "Index", "Name", "PointNumber", "Comment" };
            for (int index = 0; index < controlHeaderText.Length; index++)
            {
                var currentName = controlDatabaseName[index];
                dataGridViewControlTable.Columns[currentName].HeaderText = controlHeaderText[index];
            }
            //某些列不可改
            for (int columnIndex = 0; columnIndex < dataGridViewControlTable.Columns.Count; columnIndex++)
            {
                if (columnIndex == 0 || columnIndex == 1 || columnIndex == 4)
                {
                    dataGridViewControlTable.Columns[columnIndex].ReadOnly = true;
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


        public DataGridView DataGridViewControlTable
        {
            get
            {
                return dataGridViewControlTable;
            }
            set
            {
                dataGridViewControlTable = value;
            }
        }


        /// <summary>
        /// 给checkbox打勾
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewControlTable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //checkbox 勾上
                if ((bool)dataGridViewControlTable.Rows[e.RowIndex].Cells[0].EditedFormattedValue == true)
                {
                    this.dataGridViewControlTable.Rows[e.RowIndex].Cells[0].Value = false;
                }
                else
                {
                    this.dataGridViewControlTable.Rows[e.RowIndex].Cells[0].Value = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
