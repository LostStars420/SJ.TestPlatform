using System;
using System.Windows.Forms;

namespace Sojo.TestPlatform.ControlPlatform.View
{
    partial class TeleControlPointTableView
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.dataGridViewControlTable = new System.Windows.Forms.DataGridView();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewControlTable)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.dataGridViewControlTable);
            this.groupBox.Location = new System.Drawing.Point(0, 3);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(1300, 422);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "groupBox1";
            // 
            // dataGridViewControlTable
            // 
            this.dataGridViewControlTable.AllowUserToAddRows = false;
            this.dataGridViewControlTable.AllowUserToResizeRows = false;
            this.dataGridViewControlTable.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewControlTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewControlTable.EnableHeadersVisualStyles = false;
            this.dataGridViewControlTable.Location = new System.Drawing.Point(7, 16);
            this.dataGridViewControlTable.Name = "dataGridViewControlTable";
            this.dataGridViewControlTable.RowHeadersVisible = false;
            this.dataGridViewControlTable.RowTemplate.Height = 23;
            this.dataGridViewControlTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewControlTable.Size = new System.Drawing.Size(1290, 400);
            this.dataGridViewControlTable.TabIndex = 0;
            this.dataGridViewControlTable.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewControlTable_CellMouseClick);
            // 
            // TeleControlPointTableView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "TeleControlPointTableView";
            this.Size = new System.Drawing.Size(1300, 422);
            this.groupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewControlTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.DataGridView dataGridViewControlTable;
    }
}
