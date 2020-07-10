namespace Sojo.TestPlatform.ControlPlatform.View
{
    partial class StateSwitch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.labelOutput = new System.Windows.Forms.Label();
            this.labelInput = new System.Windows.Forms.Label();
            this.textBoxT = new System.Windows.Forms.TextBox();
            this.labelT = new System.Windows.Forms.Label();
            this.dataGridViewStateSwitch = new System.Windows.Forms.DataGridView();
            this.columnIA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAngle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStateSwitch)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.labelOutput);
            this.groupBox.Controls.Add(this.labelInput);
            this.groupBox.Controls.Add(this.textBoxT);
            this.groupBox.Controls.Add(this.labelT);
            this.groupBox.Controls.Add(this.dataGridViewStateSwitch);
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(215, 380);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "groupBox1";
            // 
            // labelOutput
            // 
            this.labelOutput.AutoSize = true;
            this.labelOutput.Location = new System.Drawing.Point(20, 356);
            this.labelOutput.Name = "labelOutput";
            this.labelOutput.Size = new System.Drawing.Size(35, 12);
            this.labelOutput.TabIndex = 14;
            this.labelOutput.Text = "输 出";
            // 
            // labelInput
            // 
            this.labelInput.AutoSize = true;
            this.labelInput.Location = new System.Drawing.Point(19, 333);
            this.labelInput.Name = "labelInput";
            this.labelInput.Size = new System.Drawing.Size(35, 12);
            this.labelInput.TabIndex = 13;
            this.labelInput.Text = "输 入";
            // 
            // textBoxT
            // 
            this.textBoxT.Location = new System.Drawing.Point(69, 300);
            this.textBoxT.Name = "textBoxT";
            this.textBoxT.Size = new System.Drawing.Size(100, 21);
            this.textBoxT.TabIndex = 12;
            // 
            // labelT
            // 
            this.labelT.AutoSize = true;
            this.labelT.Location = new System.Drawing.Point(20, 303);
            this.labelT.Name = "labelT";
            this.labelT.Size = new System.Drawing.Size(35, 12);
            this.labelT.TabIndex = 11;
            this.labelT.Text = "时间T";
            // 
            // dataGridViewStateSwitch
            // 
            this.dataGridViewStateSwitch.AllowUserToAddRows = false;
            this.dataGridViewStateSwitch.AllowUserToResizeRows = false;
            this.dataGridViewStateSwitch.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewStateSwitch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewStateSwitch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStateSwitch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnIA,
            this.ColumnValue,
            this.ColumnAngle});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewStateSwitch.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewStateSwitch.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dataGridViewStateSwitch.Location = new System.Drawing.Point(16, 20);
            this.dataGridViewStateSwitch.Name = "dataGridViewStateSwitch";
            this.dataGridViewStateSwitch.RightToLeft = System.Windows.Forms.RightToLeft.No;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewStateSwitch.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewStateSwitch.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Format = "N4";
            dataGridViewCellStyle4.NullValue = null;
            this.dataGridViewStateSwitch.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewStateSwitch.RowTemplate.Height = 23;
            this.dataGridViewStateSwitch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStateSwitch.Size = new System.Drawing.Size(193, 268);
            this.dataGridViewStateSwitch.TabIndex = 10;
            // 
            // columnIA
            // 
            this.columnIA.DataPropertyName = "(无)IA IBIC";
            this.columnIA.HeaderText = "属性";
            this.columnIA.Name = "columnIA";
            this.columnIA.Width = 60;
            // 
            // ColumnValue
            // 
            this.ColumnValue.HeaderText = "值";
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.Width = 70;
            // 
            // ColumnAngle
            // 
            this.ColumnAngle.HeaderText = "角度";
            this.ColumnAngle.Name = "ColumnAngle";
            this.ColumnAngle.Width = 70;
            // 
            // StateSwitch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "StateSwitch";
            this.Size = new System.Drawing.Size(221, 380);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStateSwitch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label labelOutput;
        private System.Windows.Forms.Label labelInput;
        private System.Windows.Forms.TextBox textBoxT;
        private System.Windows.Forms.Label labelT;
        private System.Windows.Forms.DataGridView dataGridViewStateSwitch;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIA;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAngle;
    }
}
