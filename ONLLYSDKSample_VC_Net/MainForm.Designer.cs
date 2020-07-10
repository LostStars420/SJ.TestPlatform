

using System;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Sojo.TestPlatform.ControlPlatform
{
    partial class MainForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabControlLink = new System.Windows.Forms.TabPage();
            this.groupBox104Link = new System.Windows.Forms.GroupBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.labelLocalIP = new System.Windows.Forms.Label();
            this.groupBoxLink = new System.Windows.Forms.GroupBox();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.contextMenuStripInfo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemClear = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLinkDevice = new System.Windows.Forms.Button();
            this.labelMachineIp = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.labelPcIp = new System.Windows.Forms.Label();
            this.textONLLYIP = new System.Windows.Forms.TextBox();
            this.textPCIP = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnStopTest = new System.Windows.Forms.Button();
            this.btnBeginTest = new System.Windows.Forms.Button();
            this.btnSystemConfig = new System.Windows.Forms.Button();
            this.tabPageTeleMeterTable = new System.Windows.Forms.TabPage();
            this.panelTeleMeterTable = new System.Windows.Forms.Panel();
            this.groupBoxTeleMeterTable = new System.Windows.Forms.GroupBox();
            this.dataGridViewTeleMeterTable = new System.Windows.Forms.DataGridView();
            this.ctxMenuStripTeleMeterTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMenuItemReloadMeterTable = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddMeterTable = new System.Windows.Forms.Button();
            this.tabPageTeleSingalTable = new System.Windows.Forms.TabPage();
            this.panelTeleSingalTable = new System.Windows.Forms.Panel();
            this.groupBoxTeleSingalTable = new System.Windows.Forms.GroupBox();
            this.dataGridViewTeleSingalTable = new System.Windows.Forms.DataGridView();
            this.ctxMenuStripTeleSingalTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSaveSingalTable = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMenuItemReloadSingalTable = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddTeleSingalTable = new System.Windows.Forms.Button();
            this.tabPageTeleControlTable = new System.Windows.Forms.TabPage();
            this.btnCancelSelectAll = new System.Windows.Forms.Button();
            this.panelTeleControlTable = new System.Windows.Forms.Panel();
            this.groupBoxTeleControlTable = new System.Windows.Forms.GroupBox();
            this.dataGridViewTeleControlTable = new System.Windows.Forms.DataGridView();
            this.ctxMenuStripTeleControlTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSaveControlTable = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMenuItemReloadControlTable = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.tabPageTeleSinaglling = new System.Windows.Forms.TabPage();
            this.richTextBoxTeleSingalInfo = new System.Windows.Forms.RichTextBox();
            this.contextMenuStripTeleSingal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemTeleSingal = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTeleSingalTest = new System.Windows.Forms.Button();
            this.panelOridinaryTeleSingal = new System.Windows.Forms.Panel();
            this.btnOpenAndOpen = new System.Windows.Forms.Button();
            this.btnOpenAndClose = new System.Windows.Forms.Button();
            this.comBoSingalOpen = new System.Windows.Forms.ComboBox();
            this.labelLine = new System.Windows.Forms.Label();
            this.comBoManyOpenEnd = new System.Windows.Forms.ComboBox();
            this.comBoManyOpenStart = new System.Windows.Forms.ComboBox();
            this.radionBtnManyOpen = new System.Windows.Forms.RadioButton();
            this.radioBtnSingalOpen = new System.Windows.Forms.RadioButton();
            this.radioBtnOridinaryTeleSingal = new System.Windows.Forms.RadioButton();
            this.panelTeleSingalResolution = new System.Windows.Forms.Panel();
            this.labelResolutionUnit = new System.Windows.Forms.Label();
            this.textBoxResolution = new System.Windows.Forms.TextBox();
            this.labelResolution = new System.Windows.Forms.Label();
            this.labelResolutionPulseWidthUnit = new System.Windows.Forms.Label();
            this.textBoxTelesingalResolution = new System.Windows.Forms.TextBox();
            this.labelTeleSingalResolution = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comBoResolutonEndRange = new System.Windows.Forms.ComboBox();
            this.comBoResolutionStartRange = new System.Windows.Forms.ComboBox();
            this.labelResolutionOpenRange = new System.Windows.Forms.Label();
            this.radioBtnTeleSingalResolution = new System.Windows.Forms.RadioButton();
            this.panelTeleSingallingStorm = new System.Windows.Forms.Panel();
            this.numUpDownCount = new System.Windows.Forms.NumericUpDown();
            this.labelExecuteCount = new System.Windows.Forms.Label();
            this.labelUnit = new System.Windows.Forms.Label();
            this.textBoxTelesingalPulseWidth = new System.Windows.Forms.TextBox();
            this.labelTeleSingalPluseWidth = new System.Windows.Forms.Label();
            this.labelWavyLine = new System.Windows.Forms.Label();
            this.comBoEndRange = new System.Windows.Forms.ComboBox();
            this.comBoStartRange = new System.Windows.Forms.ComboBox();
            this.labelOpenRange = new System.Windows.Forms.Label();
            this.radBtnTelesignallingStorm = new System.Windows.Forms.RadioButton();
            this.tabPageThreeTele = new System.Windows.Forms.TabPage();
            this.contextMenuStripTeleControl = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemClearTeleContorol = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridViewError = new System.Windows.Forms.DataGridView();
            this.attr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attrValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.measureValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.error = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBoxSettingValue = new System.Windows.Forms.GroupBox();
            this.dataGridViewPassageData = new System.Windows.Forms.DataGridView();
            this.richTextBoxControllInfo = new System.Windows.Forms.RichTextBox();
            this.groupBoxOutputSetting = new System.Windows.Forms.GroupBox();
            this.btnTestTeleControl = new System.Windows.Forms.Button();
            this.btnCancle = new System.Windows.Forms.Button();
            this.comBoCloseOrOpen = new System.Windows.Forms.ComboBox();
            this.labelCloseOrOpen = new System.Windows.Forms.Label();
            this.comboBoxSingleOrDouble = new System.Windows.Forms.ComboBox();
            this.labelSingleOrDouble = new System.Windows.Forms.Label();
            this.groupBoxUAUIParaSetting = new System.Windows.Forms.GroupBox();
            this.textBoxStandardError = new System.Windows.Forms.TextBox();
            this.labelStandardError = new System.Windows.Forms.Label();
            this.buttonTestTeleSingal = new System.Windows.Forms.Button();
            this.comBoOutputPassage = new System.Windows.Forms.ComboBox();
            this.labelOutputPassage = new System.Windows.Forms.Label();
            this.ConBoRatedIPerOutput = new System.Windows.Forms.ComboBox();
            this.labelRatedIPerOutput = new System.Windows.Forms.Label();
            this.ConBoRatedVPerOutput = new System.Windows.Forms.ComboBox();
            this.labelRatedVPerOutput = new System.Windows.Forms.Label();
            this.btnReverseOrderBalance = new System.Windows.Forms.Button();
            this.btnPositiveOrderBalance = new System.Windows.Forms.Button();
            this.btnIEqual = new System.Windows.Forms.Button();
            this.btnUEqual = new System.Windows.Forms.Button();
            this.buttonMinus = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.textBoxStepSize = new System.Windows.Forms.TextBox();
            this.labelStepSize = new System.Windows.Forms.Label();
            this.comBoPassageCollection = new System.Windows.Forms.ComboBox();
            this.labelChoosePassage = new System.Windows.Forms.Label();
            this.comBoAdjustAttr = new System.Windows.Forms.ComboBox();
            this.labelAdjustAttr = new System.Windows.Forms.Label();
            this.tabPageTime = new System.Windows.Forms.TabPage();
            this.ckbSystemTime = new System.Windows.Forms.CheckBox();
            this.btnreadTime = new System.Windows.Forms.Button();
            this.btnSetTime = new System.Windows.Forms.Button();
            this.labelTerDateOfWeek = new System.Windows.Forms.Label();
            this.txbTerDateOfWeek = new System.Windows.Forms.TextBox();
            this.labelTerminalMillSec = new System.Windows.Forms.Label();
            this.txbTerminalMillSec = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.labelTerminalSec = new System.Windows.Forms.Label();
            this.txbTerminalSec = new System.Windows.Forms.TextBox();
            this.labelTerminalMinute = new System.Windows.Forms.Label();
            this.txbTerminalMinute = new System.Windows.Forms.TextBox();
            this.labelTerminalHour = new System.Windows.Forms.Label();
            this.txbTerminalHour = new System.Windows.Forms.TextBox();
            this.labelTerminalDate = new System.Windows.Forms.Label();
            this.txbTerminaldate = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.labelTerminalMonth = new System.Windows.Forms.Label();
            this.txbTerminalMonth = new System.Windows.Forms.TextBox();
            this.labelTerminalTear = new System.Windows.Forms.Label();
            this.txbTerminalYear = new System.Windows.Forms.TextBox();
            this.labelTerminalTime = new System.Windows.Forms.Label();
            this.labelDateOfWeek = new System.Windows.Forms.Label();
            this.textBoxDayOfWeek = new System.Windows.Forms.TextBox();
            this.labelMillSec = new System.Windows.Forms.Label();
            this.textBoxMillSec = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labelSecond = new System.Windows.Forms.Label();
            this.textBoxSecond = new System.Windows.Forms.TextBox();
            this.labelMinute = new System.Windows.Forms.Label();
            this.textBoxMinute = new System.Windows.Forms.TextBox();
            this.labelHour = new System.Windows.Forms.Label();
            this.textBoxHour = new System.Windows.Forms.TextBox();
            this.labeldate = new System.Windows.Forms.Label();
            this.textBoxDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelMonth = new System.Windows.Forms.Label();
            this.textBoxMonth = new System.Windows.Forms.TextBox();
            this.labelYear = new System.Windows.Forms.Label();
            this.textBoxYear = new System.Windows.Forms.TextBox();
            this.labelTimeSetting = new System.Windows.Forms.Label();
            this.tabControlErrorAnalyse = new System.Windows.Forms.TabPage();
            this.btngetTeleMeterData = new System.Windows.Forms.Button();
            this.buttonGenerateReport = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.groupBoxError = new System.Windows.Forms.GroupBox();
            this.textBoxErrorRange = new System.Windows.Forms.TextBox();
            this.labelErrorRange = new System.Windows.Forms.Label();
            this.buttonGetError = new System.Windows.Forms.Button();
            this.textBoxTime = new System.Windows.Forms.TextBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.dataGridViewWuCha = new System.Windows.Forms.DataGridView();
            this.ColumnGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAttr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSetValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTestValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBoxTestMethod = new System.Windows.Forms.GroupBox();
            this.buttonTest = new System.Windows.Forms.Button();
            this.dataGridViewVV = new System.Windows.Forms.DataGridView();
            this.UAVV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UBVV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UCVV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.U0VV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IAVV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IBVV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ICVV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.I0VV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewYY = new System.Windows.Forms.DataGridView();
            this.UABYY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UBCYY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IAYY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IBYY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ICYY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.I0YY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkBoxYY = new System.Windows.Forms.CheckBox();
            this.checkBoxVV = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelNetStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelLink = new System.Windows.Forms.ToolStripStatusLabel();
            this.errorProviderTime = new System.Windows.Forms.ErrorProvider(this.components);
            this.tpfaultRecord = new System.Windows.Forms.TabPage();
            this.btnOpenFaultRecord = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabControlLink.SuspendLayout();
            this.groupBox104Link.SuspendLayout();
            this.groupBoxLink.SuspendLayout();
            this.contextMenuStripInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPageTeleMeterTable.SuspendLayout();
            this.panelTeleMeterTable.SuspendLayout();
            this.groupBoxTeleMeterTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeleMeterTable)).BeginInit();
            this.ctxMenuStripTeleMeterTable.SuspendLayout();
            this.tabPageTeleSingalTable.SuspendLayout();
            this.panelTeleSingalTable.SuspendLayout();
            this.groupBoxTeleSingalTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeleSingalTable)).BeginInit();
            this.ctxMenuStripTeleSingalTable.SuspendLayout();
            this.tabPageTeleControlTable.SuspendLayout();
            this.panelTeleControlTable.SuspendLayout();
            this.groupBoxTeleControlTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeleControlTable)).BeginInit();
            this.ctxMenuStripTeleControlTable.SuspendLayout();
            this.tabPageTeleSinaglling.SuspendLayout();
            this.contextMenuStripTeleSingal.SuspendLayout();
            this.panelOridinaryTeleSingal.SuspendLayout();
            this.panelTeleSingalResolution.SuspendLayout();
            this.panelTeleSingallingStorm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownCount)).BeginInit();
            this.tabPageThreeTele.SuspendLayout();
            this.contextMenuStripTeleControl.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewError)).BeginInit();
            this.groupBoxSettingValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPassageData)).BeginInit();
            this.groupBoxOutputSetting.SuspendLayout();
            this.groupBoxUAUIParaSetting.SuspendLayout();
            this.tabPageTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWuCha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewYY)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderTime)).BeginInit();
            this.tpfaultRecord.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabControlLink);
            this.tabControl1.Controls.Add(this.tabPageTeleMeterTable);
            this.tabControl1.Controls.Add(this.tabPageTeleSingalTable);
            this.tabControl1.Controls.Add(this.tabPageTeleControlTable);
            this.tabControl1.Controls.Add(this.tabPageTeleSinaglling);
            this.tabControl1.Controls.Add(this.tabPageThreeTele);
            this.tabControl1.Controls.Add(this.tabPageTime);
            this.tabControl1.Controls.Add(this.tpfaultRecord);
            this.tabControl1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1343, 501);
            this.tabControl1.TabIndex = 12;
            // 
            // tabControlLink
            // 
            this.tabControlLink.BackColor = System.Drawing.SystemColors.Control;
            this.tabControlLink.Controls.Add(this.groupBox104Link);
            this.tabControlLink.Controls.Add(this.groupBoxLink);
            this.tabControlLink.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControlLink.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabControlLink.Location = new System.Drawing.Point(4, 22);
            this.tabControlLink.Name = "tabControlLink";
            this.tabControlLink.Padding = new System.Windows.Forms.Padding(3);
            this.tabControlLink.Size = new System.Drawing.Size(1335, 475);
            this.tabControlLink.TabIndex = 0;
            this.tabControlLink.Text = "链路连接";
            // 
            // groupBox104Link
            // 
            this.groupBox104Link.Controls.Add(this.buttonClose);
            this.groupBox104Link.Controls.Add(this.buttonConnect);
            this.groupBox104Link.Controls.Add(this.textBoxPort);
            this.groupBox104Link.Controls.Add(this.labelPort);
            this.groupBox104Link.Controls.Add(this.textBoxIP);
            this.groupBox104Link.Controls.Add(this.labelLocalIP);
            this.groupBox104Link.Location = new System.Drawing.Point(791, 19);
            this.groupBox104Link.Name = "groupBox104Link";
            this.groupBox104Link.Size = new System.Drawing.Size(422, 422);
            this.groupBox104Link.TabIndex = 18;
            this.groupBox104Link.TabStop = false;
            this.groupBox104Link.Text = " 104连接";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(124, 187);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "断开";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(124, 143);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 4;
            this.buttonConnect.Text = "连接";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(97, 98);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(135, 21);
            this.textBoxPort.TabIndex = 3;
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(41, 103);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(41, 12);
            this.labelPort.TabIndex = 2;
            this.labelPort.Text = "端  口";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(97, 57);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(135, 21);
            this.textBoxIP.TabIndex = 1;
            // 
            // labelLocalIP
            // 
            this.labelLocalIP.AutoSize = true;
            this.labelLocalIP.Location = new System.Drawing.Point(41, 62);
            this.labelLocalIP.Name = "labelLocalIP";
            this.labelLocalIP.Size = new System.Drawing.Size(41, 12);
            this.labelLocalIP.TabIndex = 0;
            this.labelLocalIP.Text = "远程IP";
            // 
            // groupBoxLink
            // 
            this.groupBoxLink.Controls.Add(this.richTextBoxLog);
            this.groupBoxLink.Controls.Add(this.groupBox1);
            this.groupBoxLink.Controls.Add(this.groupBox2);
            this.groupBoxLink.Location = new System.Drawing.Point(23, 19);
            this.groupBoxLink.Name = "groupBoxLink";
            this.groupBoxLink.Size = new System.Drawing.Size(599, 422);
            this.groupBoxLink.TabIndex = 17;
            this.groupBoxLink.TabStop = false;
            this.groupBoxLink.Text = "测试仪连接";
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxLog.ContextMenuStrip = this.contextMenuStripInfo;
            this.richTextBoxLog.Location = new System.Drawing.Point(21, 251);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(540, 159);
            this.richTextBoxLog.TabIndex = 13;
            this.richTextBoxLog.Text = "";
            // 
            // contextMenuStripInfo
            // 
            this.contextMenuStripInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemClear});
            this.contextMenuStripInfo.Name = "contextMenuStripInfo";
            this.contextMenuStripInfo.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItemClear
            // 
            this.toolStripMenuItemClear.Name = "toolStripMenuItemClear";
            this.toolStripMenuItemClear.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItemClear.Text = "清空";
            this.toolStripMenuItemClear.Click += new System.EventHandler(this.ToolStripMenuItemClear_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLinkDevice);
            this.groupBox1.Controls.Add(this.labelMachineIp);
            this.groupBox1.Controls.Add(this.btnExit);
            this.groupBox1.Controls.Add(this.labelPcIp);
            this.groupBox1.Controls.Add(this.textONLLYIP);
            this.groupBox1.Controls.Add(this.textPCIP);
            this.groupBox1.Location = new System.Drawing.Point(21, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 196);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "连接测试仪";
            // 
            // btnLinkDevice
            // 
            this.btnLinkDevice.Location = new System.Drawing.Point(70, 97);
            this.btnLinkDevice.Name = "btnLinkDevice";
            this.btnLinkDevice.Size = new System.Drawing.Size(87, 27);
            this.btnLinkDevice.TabIndex = 4;
            this.btnLinkDevice.Text = "联机";
            this.btnLinkDevice.UseVisualStyleBackColor = true;
            this.btnLinkDevice.Click += new System.EventHandler(this.BtnLinkDevice_Click);
            // 
            // labelMachineIp
            // 
            this.labelMachineIp.Location = new System.Drawing.Point(16, 61);
            this.labelMachineIp.Name = "labelMachineIp";
            this.labelMachineIp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelMachineIp.Size = new System.Drawing.Size(65, 12);
            this.labelMachineIp.TabIndex = 3;
            this.labelMachineIp.Text = "测试仪 IP";
            this.labelMachineIp.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(70, 146);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(87, 26);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // labelPcIp
            // 
            this.labelPcIp.Location = new System.Drawing.Point(16, 29);
            this.labelPcIp.Name = "labelPcIp";
            this.labelPcIp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelPcIp.Size = new System.Drawing.Size(65, 12);
            this.labelPcIp.TabIndex = 2;
            this.labelPcIp.Text = "PC 机 IP";
            this.labelPcIp.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textONLLYIP
            // 
            this.textONLLYIP.Location = new System.Drawing.Point(87, 57);
            this.textONLLYIP.Name = "textONLLYIP";
            this.textONLLYIP.Size = new System.Drawing.Size(119, 21);
            this.textONLLYIP.TabIndex = 1;
            // 
            // textPCIP
            // 
            this.textPCIP.Location = new System.Drawing.Point(87, 25);
            this.textPCIP.Name = "textPCIP";
            this.textPCIP.Size = new System.Drawing.Size(119, 21);
            this.textPCIP.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnStopTest);
            this.groupBox2.Controls.Add(this.btnBeginTest);
            this.groupBox2.Controls.Add(this.btnSystemConfig);
            this.groupBox2.Location = new System.Drawing.Point(325, 37);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(236, 196);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "测试仪操作";
            // 
            // btnStopTest
            // 
            this.btnStopTest.Location = new System.Drawing.Point(78, 81);
            this.btnStopTest.Name = "btnStopTest";
            this.btnStopTest.Size = new System.Drawing.Size(87, 27);
            this.btnStopTest.TabIndex = 5;
            this.btnStopTest.Text = "结束试验";
            this.btnStopTest.UseVisualStyleBackColor = true;
            this.btnStopTest.Click += new System.EventHandler(this.BtnStopTest_Click);
            // 
            // btnBeginTest
            // 
            this.btnBeginTest.Location = new System.Drawing.Point(78, 25);
            this.btnBeginTest.Name = "btnBeginTest";
            this.btnBeginTest.Size = new System.Drawing.Size(87, 27);
            this.btnBeginTest.TabIndex = 4;
            this.btnBeginTest.Text = "开始试验";
            this.btnBeginTest.UseVisualStyleBackColor = true;
            this.btnBeginTest.Click += new System.EventHandler(this.BtnBeginTest_Click);
            // 
            // btnSystemConfig
            // 
            this.btnSystemConfig.Location = new System.Drawing.Point(78, 136);
            this.btnSystemConfig.Name = "btnSystemConfig";
            this.btnSystemConfig.Size = new System.Drawing.Size(87, 27);
            this.btnSystemConfig.TabIndex = 16;
            this.btnSystemConfig.Text = "系统配置";
            this.btnSystemConfig.UseVisualStyleBackColor = true;
            this.btnSystemConfig.Click += new System.EventHandler(this.BtnSystemConfig_Click);
            // 
            // tabPageTeleMeterTable
            // 
            this.tabPageTeleMeterTable.AutoScroll = true;
            this.tabPageTeleMeterTable.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageTeleMeterTable.Controls.Add(this.panelTeleMeterTable);
            this.tabPageTeleMeterTable.Controls.Add(this.btnAddMeterTable);
            this.tabPageTeleMeterTable.Location = new System.Drawing.Point(4, 22);
            this.tabPageTeleMeterTable.Name = "tabPageTeleMeterTable";
            this.tabPageTeleMeterTable.Size = new System.Drawing.Size(1335, 475);
            this.tabPageTeleMeterTable.TabIndex = 5;
            this.tabPageTeleMeterTable.Text = "遥测点表";
            // 
            // panelTeleMeterTable
            // 
            this.panelTeleMeterTable.AutoScroll = true;
            this.panelTeleMeterTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTeleMeterTable.Controls.Add(this.groupBoxTeleMeterTable);
            this.panelTeleMeterTable.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelTeleMeterTable.Location = new System.Drawing.Point(16, 40);
            this.panelTeleMeterTable.Name = "panelTeleMeterTable";
            this.panelTeleMeterTable.Size = new System.Drawing.Size(1316, 432);
            this.panelTeleMeterTable.TabIndex = 3;
            // 
            // groupBoxTeleMeterTable
            // 
            this.groupBoxTeleMeterTable.Controls.Add(this.dataGridViewTeleMeterTable);
            this.groupBoxTeleMeterTable.Location = new System.Drawing.Point(10, 5);
            this.groupBoxTeleMeterTable.Name = "groupBoxTeleMeterTable";
            this.groupBoxTeleMeterTable.Size = new System.Drawing.Size(1300, 425);
            this.groupBoxTeleMeterTable.TabIndex = 2;
            this.groupBoxTeleMeterTable.TabStop = false;
            this.groupBoxTeleMeterTable.Text = "遥测点表1路";
            // 
            // dataGridViewTeleMeterTable
            // 
            this.dataGridViewTeleMeterTable.AllowUserToAddRows = false;
            this.dataGridViewTeleMeterTable.AllowUserToResizeRows = false;
            this.dataGridViewTeleMeterTable.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTeleMeterTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTeleMeterTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTeleMeterTable.ContextMenuStrip = this.ctxMenuStripTeleMeterTable;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTeleMeterTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTeleMeterTable.EnableHeadersVisualStyles = false;
            this.dataGridViewTeleMeterTable.Location = new System.Drawing.Point(6, 20);
            this.dataGridViewTeleMeterTable.Name = "dataGridViewTeleMeterTable";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTeleMeterTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTeleMeterTable.RowHeadersVisible = false;
            this.dataGridViewTeleMeterTable.RowTemplate.Height = 23;
            this.dataGridViewTeleMeterTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTeleMeterTable.Size = new System.Drawing.Size(1290, 400);
            this.dataGridViewTeleMeterTable.TabIndex = 0;
            // 
            // ctxMenuStripTeleMeterTable
            // 
            this.ctxMenuStripTeleMeterTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSave,
            this.tsMenuItemReloadMeterTable});
            this.ctxMenuStripTeleMeterTable.Name = "ctxMenuStripTeleMeterTable";
            this.ctxMenuStripTeleMeterTable.Size = new System.Drawing.Size(149, 48);
            // 
            // toolStripMenuItemSave
            // 
            this.toolStripMenuItemSave.Name = "toolStripMenuItemSave";
            this.toolStripMenuItemSave.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItemSave.Text = "保存";
            this.toolStripMenuItemSave.Click += new System.EventHandler(this.ToolStripMenuItemSave_Click);
            // 
            // tsMenuItemReloadMeterTable
            // 
            this.tsMenuItemReloadMeterTable.Name = "tsMenuItemReloadMeterTable";
            this.tsMenuItemReloadMeterTable.Size = new System.Drawing.Size(148, 22);
            this.tsMenuItemReloadMeterTable.Text = "重新载入点表";
            this.tsMenuItemReloadMeterTable.Click += new System.EventHandler(this.TsMenuItemReloadMeterTable_Click);
            // 
            // btnAddMeterTable
            // 
            this.btnAddMeterTable.Image = ((System.Drawing.Image)(resources.GetObject("btnAddMeterTable.Image")));
            this.btnAddMeterTable.Location = new System.Drawing.Point(184, 11);
            this.btnAddMeterTable.Name = "btnAddMeterTable";
            this.btnAddMeterTable.Size = new System.Drawing.Size(58, 23);
            this.btnAddMeterTable.TabIndex = 2;
            this.btnAddMeterTable.UseVisualStyleBackColor = true;
            this.btnAddMeterTable.Click += new System.EventHandler(this.BtnAddMeterTable_Click);
            // 
            // tabPageTeleSingalTable
            // 
            this.tabPageTeleSingalTable.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageTeleSingalTable.Controls.Add(this.panelTeleSingalTable);
            this.tabPageTeleSingalTable.Controls.Add(this.btnAddTeleSingalTable);
            this.tabPageTeleSingalTable.Location = new System.Drawing.Point(4, 22);
            this.tabPageTeleSingalTable.Name = "tabPageTeleSingalTable";
            this.tabPageTeleSingalTable.Size = new System.Drawing.Size(1335, 475);
            this.tabPageTeleSingalTable.TabIndex = 6;
            this.tabPageTeleSingalTable.Text = "遥信点表";
            // 
            // panelTeleSingalTable
            // 
            this.panelTeleSingalTable.AutoScroll = true;
            this.panelTeleSingalTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTeleSingalTable.Controls.Add(this.groupBoxTeleSingalTable);
            this.panelTeleSingalTable.Location = new System.Drawing.Point(16, 40);
            this.panelTeleSingalTable.Name = "panelTeleSingalTable";
            this.panelTeleSingalTable.Size = new System.Drawing.Size(1316, 432);
            this.panelTeleSingalTable.TabIndex = 5;
            // 
            // groupBoxTeleSingalTable
            // 
            this.groupBoxTeleSingalTable.Controls.Add(this.dataGridViewTeleSingalTable);
            this.groupBoxTeleSingalTable.Location = new System.Drawing.Point(10, 5);
            this.groupBoxTeleSingalTable.Name = "groupBoxTeleSingalTable";
            this.groupBoxTeleSingalTable.Size = new System.Drawing.Size(1300, 425);
            this.groupBoxTeleSingalTable.TabIndex = 5;
            this.groupBoxTeleSingalTable.TabStop = false;
            this.groupBoxTeleSingalTable.Text = "遥信点表1路";
            // 
            // dataGridViewTeleSingalTable
            // 
            this.dataGridViewTeleSingalTable.AllowUserToAddRows = false;
            this.dataGridViewTeleSingalTable.AllowUserToResizeRows = false;
            this.dataGridViewTeleSingalTable.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTeleSingalTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTeleSingalTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTeleSingalTable.ContextMenuStrip = this.ctxMenuStripTeleSingalTable;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTeleSingalTable.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTeleSingalTable.EnableHeadersVisualStyles = false;
            this.dataGridViewTeleSingalTable.Location = new System.Drawing.Point(6, 20);
            this.dataGridViewTeleSingalTable.Name = "dataGridViewTeleSingalTable";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTeleSingalTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTeleSingalTable.RowHeadersVisible = false;
            this.dataGridViewTeleSingalTable.RowTemplate.Height = 23;
            this.dataGridViewTeleSingalTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTeleSingalTable.Size = new System.Drawing.Size(1290, 400);
            this.dataGridViewTeleSingalTable.TabIndex = 0;
            // 
            // ctxMenuStripTeleSingalTable
            // 
            this.ctxMenuStripTeleSingalTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSaveSingalTable,
            this.tsMenuItemReloadSingalTable});
            this.ctxMenuStripTeleSingalTable.Name = "ctxMenuStripTeleSingalTable";
            this.ctxMenuStripTeleSingalTable.Size = new System.Drawing.Size(149, 48);
            // 
            // toolStripMenuItemSaveSingalTable
            // 
            this.toolStripMenuItemSaveSingalTable.Name = "toolStripMenuItemSaveSingalTable";
            this.toolStripMenuItemSaveSingalTable.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItemSaveSingalTable.Text = "保存";
            this.toolStripMenuItemSaveSingalTable.Click += new System.EventHandler(this.ToolStripMenuItemSaveSingalTable_Click);
            // 
            // tsMenuItemReloadSingalTable
            // 
            this.tsMenuItemReloadSingalTable.Name = "tsMenuItemReloadSingalTable";
            this.tsMenuItemReloadSingalTable.Size = new System.Drawing.Size(148, 22);
            this.tsMenuItemReloadSingalTable.Text = "重新载入点表";
            this.tsMenuItemReloadSingalTable.Click += new System.EventHandler(this.TsMenuItemReloadSingalTable_Click);
            // 
            // btnAddTeleSingalTable
            // 
            this.btnAddTeleSingalTable.Image = ((System.Drawing.Image)(resources.GetObject("btnAddTeleSingalTable.Image")));
            this.btnAddTeleSingalTable.Location = new System.Drawing.Point(184, 11);
            this.btnAddTeleSingalTable.Name = "btnAddTeleSingalTable";
            this.btnAddTeleSingalTable.Size = new System.Drawing.Size(58, 23);
            this.btnAddTeleSingalTable.TabIndex = 4;
            this.btnAddTeleSingalTable.UseVisualStyleBackColor = true;
            this.btnAddTeleSingalTable.Click += new System.EventHandler(this.BtnAddTeleSingalTable_Click);
            // 
            // tabPageTeleControlTable
            // 
            this.tabPageTeleControlTable.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageTeleControlTable.Controls.Add(this.btnCancelSelectAll);
            this.tabPageTeleControlTable.Controls.Add(this.panelTeleControlTable);
            this.tabPageTeleControlTable.Controls.Add(this.btnSelectAll);
            this.tabPageTeleControlTable.Location = new System.Drawing.Point(4, 22);
            this.tabPageTeleControlTable.Name = "tabPageTeleControlTable";
            this.tabPageTeleControlTable.Size = new System.Drawing.Size(1335, 475);
            this.tabPageTeleControlTable.TabIndex = 7;
            this.tabPageTeleControlTable.Text = "遥控点表";
            // 
            // btnCancelSelectAll
            // 
            this.btnCancelSelectAll.Location = new System.Drawing.Point(342, 11);
            this.btnCancelSelectAll.Name = "btnCancelSelectAll";
            this.btnCancelSelectAll.Size = new System.Drawing.Size(68, 23);
            this.btnCancelSelectAll.TabIndex = 7;
            this.btnCancelSelectAll.Text = "取消选择";
            this.btnCancelSelectAll.UseVisualStyleBackColor = true;
            this.btnCancelSelectAll.Click += new System.EventHandler(this.BtnCancelSelectAll_Click);
            // 
            // panelTeleControlTable
            // 
            this.panelTeleControlTable.AutoScroll = true;
            this.panelTeleControlTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTeleControlTable.Controls.Add(this.groupBoxTeleControlTable);
            this.panelTeleControlTable.Location = new System.Drawing.Point(16, 40);
            this.panelTeleControlTable.Name = "panelTeleControlTable";
            this.panelTeleControlTable.Size = new System.Drawing.Size(1312, 432);
            this.panelTeleControlTable.TabIndex = 6;
            // 
            // groupBoxTeleControlTable
            // 
            this.groupBoxTeleControlTable.Controls.Add(this.dataGridViewTeleControlTable);
            this.groupBoxTeleControlTable.Location = new System.Drawing.Point(10, 5);
            this.groupBoxTeleControlTable.Name = "groupBoxTeleControlTable";
            this.groupBoxTeleControlTable.Size = new System.Drawing.Size(1300, 425);
            this.groupBoxTeleControlTable.TabIndex = 4;
            this.groupBoxTeleControlTable.TabStop = false;
            this.groupBoxTeleControlTable.Text = "遥控点表1路";
            // 
            // dataGridViewTeleControlTable
            // 
            this.dataGridViewTeleControlTable.AllowUserToAddRows = false;
            this.dataGridViewTeleControlTable.AllowUserToResizeRows = false;
            this.dataGridViewTeleControlTable.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTeleControlTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewTeleControlTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTeleControlTable.ContextMenuStrip = this.ctxMenuStripTeleControlTable;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTeleControlTable.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTeleControlTable.EnableHeadersVisualStyles = false;
            this.dataGridViewTeleControlTable.Location = new System.Drawing.Point(6, 20);
            this.dataGridViewTeleControlTable.Name = "dataGridViewTeleControlTable";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTeleControlTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewTeleControlTable.RowHeadersVisible = false;
            this.dataGridViewTeleControlTable.RowTemplate.Height = 23;
            this.dataGridViewTeleControlTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTeleControlTable.Size = new System.Drawing.Size(1290, 400);
            this.dataGridViewTeleControlTable.TabIndex = 0;
            this.dataGridViewTeleControlTable.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewTeleControlTable_CellMouseClick);
            // 
            // ctxMenuStripTeleControlTable
            // 
            this.ctxMenuStripTeleControlTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSaveControlTable,
            this.tsMenuItemReloadControlTable});
            this.ctxMenuStripTeleControlTable.Name = "ctxMenuStripTeleControlTable";
            this.ctxMenuStripTeleControlTable.Size = new System.Drawing.Size(149, 48);
            // 
            // toolStripMenuItemSaveControlTable
            // 
            this.toolStripMenuItemSaveControlTable.Name = "toolStripMenuItemSaveControlTable";
            this.toolStripMenuItemSaveControlTable.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItemSaveControlTable.Text = "保存";
            this.toolStripMenuItemSaveControlTable.Click += new System.EventHandler(this.ToolStripMenuItemSaveControlTable_Click);
            // 
            // tsMenuItemReloadControlTable
            // 
            this.tsMenuItemReloadControlTable.Name = "tsMenuItemReloadControlTable";
            this.tsMenuItemReloadControlTable.Size = new System.Drawing.Size(148, 22);
            this.tsMenuItemReloadControlTable.Text = "重新载入点表";
            this.tsMenuItemReloadControlTable.Click += new System.EventHandler(this.TsMenuItemReloadControlTable_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(184, 11);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(58, 23);
            this.btnSelectAll.TabIndex = 5;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.BtnSelectAll_Click);
            // 
            // tabPageTeleSinaglling
            // 
            this.tabPageTeleSinaglling.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageTeleSinaglling.Controls.Add(this.richTextBoxTeleSingalInfo);
            this.tabPageTeleSinaglling.Controls.Add(this.btnTeleSingalTest);
            this.tabPageTeleSinaglling.Controls.Add(this.panelOridinaryTeleSingal);
            this.tabPageTeleSinaglling.Controls.Add(this.radioBtnOridinaryTeleSingal);
            this.tabPageTeleSinaglling.Controls.Add(this.panelTeleSingalResolution);
            this.tabPageTeleSinaglling.Controls.Add(this.radioBtnTeleSingalResolution);
            this.tabPageTeleSinaglling.Controls.Add(this.panelTeleSingallingStorm);
            this.tabPageTeleSinaglling.Controls.Add(this.radBtnTelesignallingStorm);
            this.tabPageTeleSinaglling.Location = new System.Drawing.Point(4, 22);
            this.tabPageTeleSinaglling.Name = "tabPageTeleSinaglling";
            this.tabPageTeleSinaglling.Size = new System.Drawing.Size(1335, 475);
            this.tabPageTeleSinaglling.TabIndex = 3;
            this.tabPageTeleSinaglling.Text = "遥信测试";
            // 
            // richTextBoxTeleSingalInfo
            // 
            this.richTextBoxTeleSingalInfo.ContextMenuStrip = this.contextMenuStripTeleSingal;
            this.richTextBoxTeleSingalInfo.Location = new System.Drawing.Point(985, 219);
            this.richTextBoxTeleSingalInfo.Name = "richTextBoxTeleSingalInfo";
            this.richTextBoxTeleSingalInfo.ReadOnly = true;
            this.richTextBoxTeleSingalInfo.Size = new System.Drawing.Size(336, 218);
            this.richTextBoxTeleSingalInfo.TabIndex = 20;
            this.richTextBoxTeleSingalInfo.Text = "";
            // 
            // contextMenuStripTeleSingal
            // 
            this.contextMenuStripTeleSingal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemTeleSingal});
            this.contextMenuStripTeleSingal.Name = "contextMenuStripTeleSingal";
            this.contextMenuStripTeleSingal.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItemTeleSingal
            // 
            this.toolStripMenuItemTeleSingal.Name = "toolStripMenuItemTeleSingal";
            this.toolStripMenuItemTeleSingal.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItemTeleSingal.Text = "清空";
            this.toolStripMenuItemTeleSingal.Click += new System.EventHandler(this.ToolStripMenuItemTeleSingal_Click);
            // 
            // btnTeleSingalTest
            // 
            this.btnTeleSingalTest.Location = new System.Drawing.Point(1119, 54);
            this.btnTeleSingalTest.Name = "btnTeleSingalTest";
            this.btnTeleSingalTest.Size = new System.Drawing.Size(75, 23);
            this.btnTeleSingalTest.TabIndex = 13;
            this.btnTeleSingalTest.Text = "启动测试";
            this.btnTeleSingalTest.UseVisualStyleBackColor = true;
            this.btnTeleSingalTest.Click += new System.EventHandler(this.BtnTeleSingalTest_Click);
            // 
            // panelOridinaryTeleSingal
            // 
            this.panelOridinaryTeleSingal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOridinaryTeleSingal.Controls.Add(this.btnOpenAndOpen);
            this.panelOridinaryTeleSingal.Controls.Add(this.btnOpenAndClose);
            this.panelOridinaryTeleSingal.Controls.Add(this.comBoSingalOpen);
            this.panelOridinaryTeleSingal.Controls.Add(this.labelLine);
            this.panelOridinaryTeleSingal.Controls.Add(this.comBoManyOpenEnd);
            this.panelOridinaryTeleSingal.Controls.Add(this.comBoManyOpenStart);
            this.panelOridinaryTeleSingal.Controls.Add(this.radionBtnManyOpen);
            this.panelOridinaryTeleSingal.Controls.Add(this.radioBtnSingalOpen);
            this.panelOridinaryTeleSingal.Location = new System.Drawing.Point(34, 328);
            this.panelOridinaryTeleSingal.Name = "panelOridinaryTeleSingal";
            this.panelOridinaryTeleSingal.Size = new System.Drawing.Size(926, 109);
            this.panelOridinaryTeleSingal.TabIndex = 12;
            // 
            // btnOpenAndOpen
            // 
            this.btnOpenAndOpen.Location = new System.Drawing.Point(711, 65);
            this.btnOpenAndOpen.Name = "btnOpenAndOpen";
            this.btnOpenAndOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpenAndOpen.TabIndex = 9;
            this.btnOpenAndOpen.Text = "开出分";
            this.btnOpenAndOpen.UseVisualStyleBackColor = true;
            this.btnOpenAndOpen.Click += new System.EventHandler(this.BtnOpenAndOpen_Click);
            // 
            // btnOpenAndClose
            // 
            this.btnOpenAndClose.Location = new System.Drawing.Point(711, 22);
            this.btnOpenAndClose.Name = "btnOpenAndClose";
            this.btnOpenAndClose.Size = new System.Drawing.Size(75, 23);
            this.btnOpenAndClose.TabIndex = 8;
            this.btnOpenAndClose.Text = "开出合";
            this.btnOpenAndClose.UseVisualStyleBackColor = true;
            this.btnOpenAndClose.Click += new System.EventHandler(this.BtnOpenAndClose_Click);
            // 
            // comBoSingalOpen
            // 
            this.comBoSingalOpen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoSingalOpen.FormattingEnabled = true;
            this.comBoSingalOpen.Items.AddRange(new object[] {
            "开出1",
            "开出2",
            "开出3",
            "开出4"});
            this.comBoSingalOpen.Location = new System.Drawing.Point(310, 21);
            this.comBoSingalOpen.Name = "comBoSingalOpen";
            this.comBoSingalOpen.Size = new System.Drawing.Size(88, 20);
            this.comBoSingalOpen.TabIndex = 7;
            // 
            // labelLine
            // 
            this.labelLine.AutoSize = true;
            this.labelLine.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelLine.Location = new System.Drawing.Point(405, 73);
            this.labelLine.Name = "labelLine";
            this.labelLine.Size = new System.Drawing.Size(28, 29);
            this.labelLine.TabIndex = 6;
            this.labelLine.Text = "~";
            // 
            // comBoManyOpenEnd
            // 
            this.comBoManyOpenEnd.BackColor = System.Drawing.SystemColors.Window;
            this.comBoManyOpenEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoManyOpenEnd.FormattingEnabled = true;
            this.comBoManyOpenEnd.Items.AddRange(new object[] {
            "开出1",
            "开出2",
            "开出3",
            "开出4"});
            this.comBoManyOpenEnd.Location = new System.Drawing.Point(440, 67);
            this.comBoManyOpenEnd.Name = "comBoManyOpenEnd";
            this.comBoManyOpenEnd.Size = new System.Drawing.Size(88, 20);
            this.comBoManyOpenEnd.TabIndex = 5;
            // 
            // comBoManyOpenStart
            // 
            this.comBoManyOpenStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoManyOpenStart.FormattingEnabled = true;
            this.comBoManyOpenStart.Items.AddRange(new object[] {
            "开出1",
            "开出2",
            "开出3",
            "开出4"});
            this.comBoManyOpenStart.Location = new System.Drawing.Point(310, 67);
            this.comBoManyOpenStart.Name = "comBoManyOpenStart";
            this.comBoManyOpenStart.Size = new System.Drawing.Size(88, 20);
            this.comBoManyOpenStart.TabIndex = 4;
            // 
            // radionBtnManyOpen
            // 
            this.radionBtnManyOpen.AutoSize = true;
            this.radionBtnManyOpen.Location = new System.Drawing.Point(201, 67);
            this.radionBtnManyOpen.Name = "radionBtnManyOpen";
            this.radionBtnManyOpen.Size = new System.Drawing.Size(83, 16);
            this.radionBtnManyOpen.TabIndex = 1;
            this.radionBtnManyOpen.TabStop = true;
            this.radionBtnManyOpen.Text = "多个开出量";
            this.radionBtnManyOpen.UseVisualStyleBackColor = true;
            this.radionBtnManyOpen.CheckedChanged += new System.EventHandler(this.RadionBtnManyOpen_CheckedChanged);
            // 
            // radioBtnSingalOpen
            // 
            this.radioBtnSingalOpen.AutoSize = true;
            this.radioBtnSingalOpen.Location = new System.Drawing.Point(201, 22);
            this.radioBtnSingalOpen.Name = "radioBtnSingalOpen";
            this.radioBtnSingalOpen.Size = new System.Drawing.Size(83, 16);
            this.radioBtnSingalOpen.TabIndex = 0;
            this.radioBtnSingalOpen.TabStop = true;
            this.radioBtnSingalOpen.Text = "单个开出量";
            this.radioBtnSingalOpen.UseVisualStyleBackColor = true;
            this.radioBtnSingalOpen.CheckedChanged += new System.EventHandler(this.RadioBtnSingalOpen_CheckedChanged);
            // 
            // radioBtnOridinaryTeleSingal
            // 
            this.radioBtnOridinaryTeleSingal.AutoSize = true;
            this.radioBtnOridinaryTeleSingal.Location = new System.Drawing.Point(34, 306);
            this.radioBtnOridinaryTeleSingal.Name = "radioBtnOridinaryTeleSingal";
            this.radioBtnOridinaryTeleSingal.Size = new System.Drawing.Size(71, 16);
            this.radioBtnOridinaryTeleSingal.TabIndex = 11;
            this.radioBtnOridinaryTeleSingal.Text = "普通遥信";
            this.radioBtnOridinaryTeleSingal.UseVisualStyleBackColor = true;
            this.radioBtnOridinaryTeleSingal.CheckedChanged += new System.EventHandler(this.RadioBtnOridinaryTeleSingal_CheckedChanged);
            // 
            // panelTeleSingalResolution
            // 
            this.panelTeleSingalResolution.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTeleSingalResolution.Controls.Add(this.labelResolutionUnit);
            this.panelTeleSingalResolution.Controls.Add(this.textBoxResolution);
            this.panelTeleSingalResolution.Controls.Add(this.labelResolution);
            this.panelTeleSingalResolution.Controls.Add(this.labelResolutionPulseWidthUnit);
            this.panelTeleSingalResolution.Controls.Add(this.textBoxTelesingalResolution);
            this.panelTeleSingalResolution.Controls.Add(this.labelTeleSingalResolution);
            this.panelTeleSingalResolution.Controls.Add(this.label5);
            this.panelTeleSingalResolution.Controls.Add(this.comBoResolutonEndRange);
            this.panelTeleSingalResolution.Controls.Add(this.comBoResolutionStartRange);
            this.panelTeleSingalResolution.Controls.Add(this.labelResolutionOpenRange);
            this.panelTeleSingalResolution.Location = new System.Drawing.Point(34, 184);
            this.panelTeleSingalResolution.Name = "panelTeleSingalResolution";
            this.panelTeleSingalResolution.Size = new System.Drawing.Size(926, 109);
            this.panelTeleSingalResolution.TabIndex = 10;
            // 
            // labelResolutionUnit
            // 
            this.labelResolutionUnit.AutoSize = true;
            this.labelResolutionUnit.Location = new System.Drawing.Point(807, 40);
            this.labelResolutionUnit.Name = "labelResolutionUnit";
            this.labelResolutionUnit.Size = new System.Drawing.Size(17, 12);
            this.labelResolutionUnit.TabIndex = 9;
            this.labelResolutionUnit.Text = "ms";
            // 
            // textBoxResolution
            // 
            this.textBoxResolution.Location = new System.Drawing.Point(730, 35);
            this.textBoxResolution.Name = "textBoxResolution";
            this.textBoxResolution.Size = new System.Drawing.Size(70, 21);
            this.textBoxResolution.TabIndex = 8;
            this.textBoxResolution.Text = "5";
            // 
            // labelResolution
            // 
            this.labelResolution.AutoSize = true;
            this.labelResolution.Location = new System.Drawing.Point(680, 40);
            this.labelResolution.Name = "labelResolution";
            this.labelResolution.Size = new System.Drawing.Size(53, 12);
            this.labelResolution.TabIndex = 7;
            this.labelResolution.Text = "分辨率：";
            // 
            // labelResolutionPulseWidthUnit
            // 
            this.labelResolutionPulseWidthUnit.AutoSize = true;
            this.labelResolutionPulseWidthUnit.Location = new System.Drawing.Point(585, 40);
            this.labelResolutionPulseWidthUnit.Name = "labelResolutionPulseWidthUnit";
            this.labelResolutionPulseWidthUnit.Size = new System.Drawing.Size(17, 12);
            this.labelResolutionPulseWidthUnit.TabIndex = 6;
            this.labelResolutionPulseWidthUnit.Text = "ms";
            // 
            // textBoxTelesingalResolution
            // 
            this.textBoxTelesingalResolution.Location = new System.Drawing.Point(505, 35);
            this.textBoxTelesingalResolution.Name = "textBoxTelesingalResolution";
            this.textBoxTelesingalResolution.Size = new System.Drawing.Size(70, 21);
            this.textBoxTelesingalResolution.TabIndex = 5;
            this.textBoxTelesingalResolution.Text = "500";
            // 
            // labelTeleSingalResolution
            // 
            this.labelTeleSingalResolution.AutoSize = true;
            this.labelTeleSingalResolution.Location = new System.Drawing.Point(440, 40);
            this.labelTeleSingalResolution.Name = "labelTeleSingalResolution";
            this.labelTeleSingalResolution.Size = new System.Drawing.Size(65, 12);
            this.labelTeleSingalResolution.TabIndex = 4;
            this.labelTeleSingalResolution.Text = "遥信脉宽：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(225, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 29);
            this.label5.TabIndex = 3;
            this.label5.Text = "~";
            // 
            // comBoResolutonEndRange
            // 
            this.comBoResolutonEndRange.BackColor = System.Drawing.SystemColors.Window;
            this.comBoResolutonEndRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoResolutonEndRange.FormattingEnabled = true;
            this.comBoResolutonEndRange.Items.AddRange(new object[] {
            "开出1",
            "开出2",
            "开出3",
            "开出4"});
            this.comBoResolutonEndRange.Location = new System.Drawing.Point(260, 35);
            this.comBoResolutonEndRange.Name = "comBoResolutonEndRange";
            this.comBoResolutonEndRange.Size = new System.Drawing.Size(88, 20);
            this.comBoResolutonEndRange.TabIndex = 2;
            // 
            // comBoResolutionStartRange
            // 
            this.comBoResolutionStartRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoResolutionStartRange.FormattingEnabled = true;
            this.comBoResolutionStartRange.Items.AddRange(new object[] {
            "开出1",
            "开出2",
            "开出3",
            "开出4"});
            this.comBoResolutionStartRange.Location = new System.Drawing.Point(130, 35);
            this.comBoResolutionStartRange.Name = "comBoResolutionStartRange";
            this.comBoResolutionStartRange.Size = new System.Drawing.Size(88, 20);
            this.comBoResolutionStartRange.TabIndex = 1;
            // 
            // labelResolutionOpenRange
            // 
            this.labelResolutionOpenRange.AutoSize = true;
            this.labelResolutionOpenRange.Location = new System.Drawing.Point(65, 40);
            this.labelResolutionOpenRange.Name = "labelResolutionOpenRange";
            this.labelResolutionOpenRange.Size = new System.Drawing.Size(65, 12);
            this.labelResolutionOpenRange.TabIndex = 0;
            this.labelResolutionOpenRange.Text = "开出范围：";
            // 
            // radioBtnTeleSingalResolution
            // 
            this.radioBtnTeleSingalResolution.AutoSize = true;
            this.radioBtnTeleSingalResolution.Location = new System.Drawing.Point(34, 162);
            this.radioBtnTeleSingalResolution.Name = "radioBtnTeleSingalResolution";
            this.radioBtnTeleSingalResolution.Size = new System.Drawing.Size(83, 16);
            this.radioBtnTeleSingalResolution.TabIndex = 9;
            this.radioBtnTeleSingalResolution.Text = "遥信分辨率";
            this.radioBtnTeleSingalResolution.UseVisualStyleBackColor = true;
            this.radioBtnTeleSingalResolution.CheckedChanged += new System.EventHandler(this.RadioBtnTeleSingalResolution_CheckedChanged);
            // 
            // panelTeleSingallingStorm
            // 
            this.panelTeleSingallingStorm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTeleSingallingStorm.Controls.Add(this.numUpDownCount);
            this.panelTeleSingallingStorm.Controls.Add(this.labelExecuteCount);
            this.panelTeleSingallingStorm.Controls.Add(this.labelUnit);
            this.panelTeleSingallingStorm.Controls.Add(this.textBoxTelesingalPulseWidth);
            this.panelTeleSingallingStorm.Controls.Add(this.labelTeleSingalPluseWidth);
            this.panelTeleSingallingStorm.Controls.Add(this.labelWavyLine);
            this.panelTeleSingallingStorm.Controls.Add(this.comBoEndRange);
            this.panelTeleSingallingStorm.Controls.Add(this.comBoStartRange);
            this.panelTeleSingallingStorm.Controls.Add(this.labelOpenRange);
            this.panelTeleSingallingStorm.Location = new System.Drawing.Point(34, 45);
            this.panelTeleSingallingStorm.Name = "panelTeleSingallingStorm";
            this.panelTeleSingallingStorm.Size = new System.Drawing.Size(926, 109);
            this.panelTeleSingallingStorm.TabIndex = 1;
            // 
            // numUpDownCount
            // 
            this.numUpDownCount.Location = new System.Drawing.Point(740, 35);
            this.numUpDownCount.Name = "numUpDownCount";
            this.numUpDownCount.Size = new System.Drawing.Size(55, 21);
            this.numUpDownCount.TabIndex = 8;
            this.numUpDownCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // labelExecuteCount
            // 
            this.labelExecuteCount.AutoSize = true;
            this.labelExecuteCount.Location = new System.Drawing.Point(680, 40);
            this.labelExecuteCount.Name = "labelExecuteCount";
            this.labelExecuteCount.Size = new System.Drawing.Size(65, 12);
            this.labelExecuteCount.TabIndex = 7;
            this.labelExecuteCount.Text = "执行次数：";
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Location = new System.Drawing.Point(585, 40);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(17, 12);
            this.labelUnit.TabIndex = 6;
            this.labelUnit.Text = "ms";
            // 
            // textBoxTelesingalPulseWidth
            // 
            this.textBoxTelesingalPulseWidth.Location = new System.Drawing.Point(505, 35);
            this.textBoxTelesingalPulseWidth.Name = "textBoxTelesingalPulseWidth";
            this.textBoxTelesingalPulseWidth.Size = new System.Drawing.Size(70, 21);
            this.textBoxTelesingalPulseWidth.TabIndex = 5;
            this.textBoxTelesingalPulseWidth.Text = "500";
            // 
            // labelTeleSingalPluseWidth
            // 
            this.labelTeleSingalPluseWidth.AutoSize = true;
            this.labelTeleSingalPluseWidth.Location = new System.Drawing.Point(440, 40);
            this.labelTeleSingalPluseWidth.Name = "labelTeleSingalPluseWidth";
            this.labelTeleSingalPluseWidth.Size = new System.Drawing.Size(65, 12);
            this.labelTeleSingalPluseWidth.TabIndex = 4;
            this.labelTeleSingalPluseWidth.Text = "遥信脉宽：";
            // 
            // labelWavyLine
            // 
            this.labelWavyLine.AutoSize = true;
            this.labelWavyLine.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelWavyLine.Location = new System.Drawing.Point(225, 41);
            this.labelWavyLine.Name = "labelWavyLine";
            this.labelWavyLine.Size = new System.Drawing.Size(28, 29);
            this.labelWavyLine.TabIndex = 3;
            this.labelWavyLine.Text = "~";
            // 
            // comBoEndRange
            // 
            this.comBoEndRange.BackColor = System.Drawing.SystemColors.Window;
            this.comBoEndRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoEndRange.FormattingEnabled = true;
            this.comBoEndRange.Items.AddRange(new object[] {
            "开出1",
            "开出2",
            "开出3",
            "开出4"});
            this.comBoEndRange.Location = new System.Drawing.Point(260, 35);
            this.comBoEndRange.Name = "comBoEndRange";
            this.comBoEndRange.Size = new System.Drawing.Size(88, 20);
            this.comBoEndRange.TabIndex = 2;
            // 
            // comBoStartRange
            // 
            this.comBoStartRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoStartRange.FormattingEnabled = true;
            this.comBoStartRange.Items.AddRange(new object[] {
            "开出1",
            "开出2",
            "开出3",
            "开出4"});
            this.comBoStartRange.Location = new System.Drawing.Point(130, 35);
            this.comBoStartRange.Name = "comBoStartRange";
            this.comBoStartRange.Size = new System.Drawing.Size(88, 20);
            this.comBoStartRange.TabIndex = 1;
            // 
            // labelOpenRange
            // 
            this.labelOpenRange.AutoSize = true;
            this.labelOpenRange.Location = new System.Drawing.Point(65, 40);
            this.labelOpenRange.Name = "labelOpenRange";
            this.labelOpenRange.Size = new System.Drawing.Size(65, 12);
            this.labelOpenRange.TabIndex = 0;
            this.labelOpenRange.Text = "开出范围：";
            // 
            // radBtnTelesignallingStorm
            // 
            this.radBtnTelesignallingStorm.AutoSize = true;
            this.radBtnTelesignallingStorm.Checked = true;
            this.radBtnTelesignallingStorm.Location = new System.Drawing.Point(34, 23);
            this.radBtnTelesignallingStorm.Name = "radBtnTelesignallingStorm";
            this.radBtnTelesignallingStorm.Size = new System.Drawing.Size(71, 16);
            this.radBtnTelesignallingStorm.TabIndex = 0;
            this.radBtnTelesignallingStorm.TabStop = true;
            this.radBtnTelesignallingStorm.Text = "遥信风暴";
            this.radBtnTelesignallingStorm.UseVisualStyleBackColor = true;
            this.radBtnTelesignallingStorm.CheckedChanged += new System.EventHandler(this.RadBtnTelesignallingStorm_CheckedChanged);
            // 
            // tabPageThreeTele
            // 
            this.tabPageThreeTele.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageThreeTele.ContextMenuStrip = this.contextMenuStripTeleControl;
            this.tabPageThreeTele.Controls.Add(this.groupBox3);
            this.tabPageThreeTele.Controls.Add(this.groupBoxSettingValue);
            this.tabPageThreeTele.Controls.Add(this.richTextBoxControllInfo);
            this.tabPageThreeTele.Controls.Add(this.groupBoxOutputSetting);
            this.tabPageThreeTele.Controls.Add(this.groupBoxUAUIParaSetting);
            this.tabPageThreeTele.Location = new System.Drawing.Point(4, 22);
            this.tabPageThreeTele.Name = "tabPageThreeTele";
            this.tabPageThreeTele.Size = new System.Drawing.Size(1335, 475);
            this.tabPageThreeTele.TabIndex = 4;
            this.tabPageThreeTele.Text = "遥测遥控测试";
            // 
            // contextMenuStripTeleControl
            // 
            this.contextMenuStripTeleControl.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenuStripTeleControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemClearTeleContorol});
            this.contextMenuStripTeleControl.Name = "contextMenuStripTeleControl";
            this.contextMenuStripTeleControl.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItemClearTeleContorol
            // 
            this.toolStripMenuItemClearTeleContorol.Name = "toolStripMenuItemClearTeleContorol";
            this.toolStripMenuItemClearTeleContorol.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItemClearTeleContorol.Text = "清空";
            this.toolStripMenuItemClearTeleContorol.Click += new System.EventHandler(this.ToolStripMenuItemClearTeleContorol_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridViewError);
            this.groupBox3.Location = new System.Drawing.Point(525, 135);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(405, 331);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "测试结果";
            // 
            // dataGridViewError
            // 
            this.dataGridViewError.AllowUserToAddRows = false;
            this.dataGridViewError.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewError.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewError.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewError.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.attr,
            this.attrValue,
            this.measureValue,
            this.error});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewError.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewError.EnableHeadersVisualStyles = false;
            this.dataGridViewError.Location = new System.Drawing.Point(0, 20);
            this.dataGridViewError.Name = "dataGridViewError";
            this.dataGridViewError.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewError.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridViewError.RowHeadersVisible = false;
            this.dataGridViewError.RowTemplate.Height = 23;
            this.dataGridViewError.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewError.Size = new System.Drawing.Size(405, 308);
            this.dataGridViewError.TabIndex = 1;
            // 
            // attr
            // 
            this.attr.HeaderText = "测量属性";
            this.attr.Name = "attr";
            this.attr.ReadOnly = true;
            // 
            // attrValue
            // 
            this.attrValue.HeaderText = "给定值";
            this.attrValue.Name = "attrValue";
            this.attrValue.ReadOnly = true;
            // 
            // measureValue
            // 
            this.measureValue.HeaderText = "测量值";
            this.measureValue.Name = "measureValue";
            this.measureValue.ReadOnly = true;
            // 
            // error
            // 
            this.error.HeaderText = "误差";
            this.error.Name = "error";
            this.error.ReadOnly = true;
            // 
            // groupBoxSettingValue
            // 
            this.groupBoxSettingValue.Controls.Add(this.dataGridViewPassageData);
            this.groupBoxSettingValue.Location = new System.Drawing.Point(37, 135);
            this.groupBoxSettingValue.Name = "groupBoxSettingValue";
            this.groupBoxSettingValue.Size = new System.Drawing.Size(432, 331);
            this.groupBoxSettingValue.TabIndex = 20;
            this.groupBoxSettingValue.TabStop = false;
            this.groupBoxSettingValue.Text = "给定值";
            // 
            // dataGridViewPassageData
            // 
            this.dataGridViewPassageData.AllowUserToAddRows = false;
            this.dataGridViewPassageData.AllowUserToResizeRows = false;
            this.dataGridViewPassageData.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPassageData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewPassageData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewPassageData.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewPassageData.EnableHeadersVisualStyles = false;
            this.dataGridViewPassageData.Location = new System.Drawing.Point(3, 20);
            this.dataGridViewPassageData.Name = "dataGridViewPassageData";
            this.dataGridViewPassageData.ReadOnly = true;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPassageData.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridViewPassageData.RowHeadersVisible = false;
            this.dataGridViewPassageData.RowHeadersWidth = 60;
            this.dataGridViewPassageData.RowTemplate.Height = 23;
            this.dataGridViewPassageData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPassageData.Size = new System.Drawing.Size(429, 308);
            this.dataGridViewPassageData.TabIndex = 0;
            this.dataGridViewPassageData.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewPassageData_CellValueChanged);
            // 
            // richTextBoxControllInfo
            // 
            this.richTextBoxControllInfo.ContextMenuStrip = this.contextMenuStripTeleControl;
            this.richTextBoxControllInfo.Location = new System.Drawing.Point(970, 184);
            this.richTextBoxControllInfo.Name = "richTextBoxControllInfo";
            this.richTextBoxControllInfo.ReadOnly = true;
            this.richTextBoxControllInfo.Size = new System.Drawing.Size(336, 279);
            this.richTextBoxControllInfo.TabIndex = 19;
            this.richTextBoxControllInfo.Text = "";
            // 
            // groupBoxOutputSetting
            // 
            this.groupBoxOutputSetting.Controls.Add(this.btnTestTeleControl);
            this.groupBoxOutputSetting.Controls.Add(this.btnCancle);
            this.groupBoxOutputSetting.Controls.Add(this.comBoCloseOrOpen);
            this.groupBoxOutputSetting.Controls.Add(this.labelCloseOrOpen);
            this.groupBoxOutputSetting.Controls.Add(this.comboBoxSingleOrDouble);
            this.groupBoxOutputSetting.Controls.Add(this.labelSingleOrDouble);
            this.groupBoxOutputSetting.Location = new System.Drawing.Point(970, 11);
            this.groupBoxOutputSetting.Name = "groupBoxOutputSetting";
            this.groupBoxOutputSetting.Size = new System.Drawing.Size(336, 146);
            this.groupBoxOutputSetting.TabIndex = 10;
            this.groupBoxOutputSetting.TabStop = false;
            this.groupBoxOutputSetting.Text = "遥控测试";
            // 
            // btnTestTeleControl
            // 
            this.btnTestTeleControl.Location = new System.Drawing.Point(248, 44);
            this.btnTestTeleControl.Name = "btnTestTeleControl";
            this.btnTestTeleControl.Size = new System.Drawing.Size(64, 23);
            this.btnTestTeleControl.TabIndex = 26;
            this.btnTestTeleControl.Text = "测试";
            this.btnTestTeleControl.UseVisualStyleBackColor = true;
            this.btnTestTeleControl.Click += new System.EventHandler(this.BtnTestTeleControl_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(248, 102);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(64, 23);
            this.btnCancle.TabIndex = 24;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.BtnCancle_Click);
            // 
            // comBoCloseOrOpen
            // 
            this.comBoCloseOrOpen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoCloseOrOpen.FormattingEnabled = true;
            this.comBoCloseOrOpen.Items.AddRange(new object[] {
            "合闸",
            "分闸"});
            this.comBoCloseOrOpen.Location = new System.Drawing.Point(59, 104);
            this.comBoCloseOrOpen.Name = "comBoCloseOrOpen";
            this.comBoCloseOrOpen.Size = new System.Drawing.Size(93, 20);
            this.comBoCloseOrOpen.TabIndex = 23;
            // 
            // labelCloseOrOpen
            // 
            this.labelCloseOrOpen.AutoSize = true;
            this.labelCloseOrOpen.Location = new System.Drawing.Point(6, 110);
            this.labelCloseOrOpen.Name = "labelCloseOrOpen";
            this.labelCloseOrOpen.Size = new System.Drawing.Size(47, 12);
            this.labelCloseOrOpen.TabIndex = 22;
            this.labelCloseOrOpen.Text = "合/分：";
            // 
            // comboBoxSingleOrDouble
            // 
            this.comboBoxSingleOrDouble.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSingleOrDouble.FormattingEnabled = true;
            this.comboBoxSingleOrDouble.Items.AddRange(new object[] {
            "单点",
            "双点"});
            this.comboBoxSingleOrDouble.Location = new System.Drawing.Point(59, 42);
            this.comboBoxSingleOrDouble.Name = "comboBoxSingleOrDouble";
            this.comboBoxSingleOrDouble.Size = new System.Drawing.Size(93, 20);
            this.comboBoxSingleOrDouble.TabIndex = 20;
            // 
            // labelSingleOrDouble
            // 
            this.labelSingleOrDouble.AutoSize = true;
            this.labelSingleOrDouble.Location = new System.Drawing.Point(6, 48);
            this.labelSingleOrDouble.Name = "labelSingleOrDouble";
            this.labelSingleOrDouble.Size = new System.Drawing.Size(47, 12);
            this.labelSingleOrDouble.TabIndex = 19;
            this.labelSingleOrDouble.Text = "单/双：";
            // 
            // groupBoxUAUIParaSetting
            // 
            this.groupBoxUAUIParaSetting.Controls.Add(this.textBoxStandardError);
            this.groupBoxUAUIParaSetting.Controls.Add(this.labelStandardError);
            this.groupBoxUAUIParaSetting.Controls.Add(this.buttonTestTeleSingal);
            this.groupBoxUAUIParaSetting.Controls.Add(this.comBoOutputPassage);
            this.groupBoxUAUIParaSetting.Controls.Add(this.labelOutputPassage);
            this.groupBoxUAUIParaSetting.Controls.Add(this.ConBoRatedIPerOutput);
            this.groupBoxUAUIParaSetting.Controls.Add(this.labelRatedIPerOutput);
            this.groupBoxUAUIParaSetting.Controls.Add(this.ConBoRatedVPerOutput);
            this.groupBoxUAUIParaSetting.Controls.Add(this.labelRatedVPerOutput);
            this.groupBoxUAUIParaSetting.Controls.Add(this.btnReverseOrderBalance);
            this.groupBoxUAUIParaSetting.Controls.Add(this.btnPositiveOrderBalance);
            this.groupBoxUAUIParaSetting.Controls.Add(this.btnIEqual);
            this.groupBoxUAUIParaSetting.Controls.Add(this.btnUEqual);
            this.groupBoxUAUIParaSetting.Controls.Add(this.buttonMinus);
            this.groupBoxUAUIParaSetting.Controls.Add(this.buttonUp);
            this.groupBoxUAUIParaSetting.Controls.Add(this.textBoxStepSize);
            this.groupBoxUAUIParaSetting.Controls.Add(this.labelStepSize);
            this.groupBoxUAUIParaSetting.Controls.Add(this.comBoPassageCollection);
            this.groupBoxUAUIParaSetting.Controls.Add(this.labelChoosePassage);
            this.groupBoxUAUIParaSetting.Controls.Add(this.comBoAdjustAttr);
            this.groupBoxUAUIParaSetting.Controls.Add(this.labelAdjustAttr);
            this.groupBoxUAUIParaSetting.Location = new System.Drawing.Point(37, 11);
            this.groupBoxUAUIParaSetting.Name = "groupBoxUAUIParaSetting";
            this.groupBoxUAUIParaSetting.Size = new System.Drawing.Size(893, 118);
            this.groupBoxUAUIParaSetting.TabIndex = 0;
            this.groupBoxUAUIParaSetting.TabStop = false;
            this.groupBoxUAUIParaSetting.Text = "电压电流参数设置";
            // 
            // textBoxStandardError
            // 
            this.textBoxStandardError.Location = new System.Drawing.Point(821, 19);
            this.textBoxStandardError.Name = "textBoxStandardError";
            this.textBoxStandardError.Size = new System.Drawing.Size(66, 21);
            this.textBoxStandardError.TabIndex = 20;
            this.textBoxStandardError.Text = "0.1";
            this.textBoxStandardError.TextChanged += new System.EventHandler(this.TextBoxStandardError_TextChanged);
            // 
            // labelStandardError
            // 
            this.labelStandardError.AutoSize = true;
            this.labelStandardError.Location = new System.Drawing.Point(750, 23);
            this.labelStandardError.Name = "labelStandardError";
            this.labelStandardError.Size = new System.Drawing.Size(65, 12);
            this.labelStandardError.TabIndex = 19;
            this.labelStandardError.Text = "标准误差：";
            // 
            // buttonTestTeleSingal
            // 
            this.buttonTestTeleSingal.Location = new System.Drawing.Point(787, 78);
            this.buttonTestTeleSingal.Name = "buttonTestTeleSingal";
            this.buttonTestTeleSingal.Size = new System.Drawing.Size(75, 23);
            this.buttonTestTeleSingal.TabIndex = 18;
            this.buttonTestTeleSingal.Text = "测试";
            this.buttonTestTeleSingal.UseVisualStyleBackColor = true;
            this.buttonTestTeleSingal.Click += new System.EventHandler(this.ButtonTestTeleSingal_Click);
            // 
            // comBoOutputPassage
            // 
            this.comBoOutputPassage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoOutputPassage.FormattingEnabled = true;
            this.comBoOutputPassage.Items.AddRange(new object[] {
            "组1",
            "组2",
            "组3",
            "组4",
            "组5",
            "组6",
            "组7",
            "组8",
            "组9"});
            this.comBoOutputPassage.Location = new System.Drawing.Point(627, 80);
            this.comBoOutputPassage.Name = "comBoOutputPassage";
            this.comBoOutputPassage.Size = new System.Drawing.Size(93, 20);
            this.comBoOutputPassage.TabIndex = 17;
            // 
            // labelOutputPassage
            // 
            this.labelOutputPassage.AutoSize = true;
            this.labelOutputPassage.Location = new System.Drawing.Point(507, 85);
            this.labelOutputPassage.Name = "labelOutputPassage";
            this.labelOutputPassage.Size = new System.Drawing.Size(53, 12);
            this.labelOutputPassage.TabIndex = 16;
            this.labelOutputPassage.Text = "输出通道";
            // 
            // ConBoRatedIPerOutput
            // 
            this.ConBoRatedIPerOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ConBoRatedIPerOutput.FormattingEnabled = true;
            this.ConBoRatedIPerOutput.Items.AddRange(new object[] {
            "100%",
            "90%",
            "80%",
            "70%",
            "60%",
            "50%",
            "40%",
            "30%",
            "20%",
            "10%"});
            this.ConBoRatedIPerOutput.Location = new System.Drawing.Point(627, 50);
            this.ConBoRatedIPerOutput.Name = "ConBoRatedIPerOutput";
            this.ConBoRatedIPerOutput.Size = new System.Drawing.Size(93, 20);
            this.ConBoRatedIPerOutput.TabIndex = 15;
            this.ConBoRatedIPerOutput.SelectedIndexChanged += new System.EventHandler(this.ConBoRatedIPerOutput_SelectedIndexChanged);
            // 
            // labelRatedIPerOutput
            // 
            this.labelRatedIPerOutput.AutoSize = true;
            this.labelRatedIPerOutput.Location = new System.Drawing.Point(507, 55);
            this.labelRatedIPerOutput.Name = "labelRatedIPerOutput";
            this.labelRatedIPerOutput.Size = new System.Drawing.Size(113, 12);
            this.labelRatedIPerOutput.TabIndex = 14;
            this.labelRatedIPerOutput.Text = "额定电流百分比输出";
            // 
            // ConBoRatedVPerOutput
            // 
            this.ConBoRatedVPerOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ConBoRatedVPerOutput.FormattingEnabled = true;
            this.ConBoRatedVPerOutput.Items.AddRange(new object[] {
            "100%",
            "90%",
            "80%",
            "70%",
            "60%",
            "50%",
            "40%",
            "30%",
            "20%",
            "10%"});
            this.ConBoRatedVPerOutput.Location = new System.Drawing.Point(627, 20);
            this.ConBoRatedVPerOutput.Name = "ConBoRatedVPerOutput";
            this.ConBoRatedVPerOutput.Size = new System.Drawing.Size(93, 20);
            this.ConBoRatedVPerOutput.TabIndex = 13;
            this.ConBoRatedVPerOutput.SelectedIndexChanged += new System.EventHandler(this.ConBoRatedVPerOutput_SelectedIndexChanged);
            // 
            // labelRatedVPerOutput
            // 
            this.labelRatedVPerOutput.AutoSize = true;
            this.labelRatedVPerOutput.Location = new System.Drawing.Point(507, 25);
            this.labelRatedVPerOutput.Name = "labelRatedVPerOutput";
            this.labelRatedVPerOutput.Size = new System.Drawing.Size(113, 12);
            this.labelRatedVPerOutput.TabIndex = 12;
            this.labelRatedVPerOutput.Text = "额定电压百分比输出";
            // 
            // btnReverseOrderBalance
            // 
            this.btnReverseOrderBalance.Location = new System.Drawing.Point(408, 75);
            this.btnReverseOrderBalance.Name = "btnReverseOrderBalance";
            this.btnReverseOrderBalance.Size = new System.Drawing.Size(75, 23);
            this.btnReverseOrderBalance.TabIndex = 11;
            this.btnReverseOrderBalance.Text = "逆序平衡";
            this.btnReverseOrderBalance.UseVisualStyleBackColor = true;
            this.btnReverseOrderBalance.Click += new System.EventHandler(this.BtnReverseOrderBalance_Click);
            // 
            // btnPositiveOrderBalance
            // 
            this.btnPositiveOrderBalance.Location = new System.Drawing.Point(408, 29);
            this.btnPositiveOrderBalance.Name = "btnPositiveOrderBalance";
            this.btnPositiveOrderBalance.Size = new System.Drawing.Size(75, 23);
            this.btnPositiveOrderBalance.TabIndex = 10;
            this.btnPositiveOrderBalance.Text = "正序平衡";
            this.btnPositiveOrderBalance.UseVisualStyleBackColor = true;
            this.btnPositiveOrderBalance.Click += new System.EventHandler(this.BtnPositiveOrderBalance_Click);
            // 
            // btnIEqual
            // 
            this.btnIEqual.Location = new System.Drawing.Point(306, 74);
            this.btnIEqual.Name = "btnIEqual";
            this.btnIEqual.Size = new System.Drawing.Size(75, 23);
            this.btnIEqual.TabIndex = 9;
            this.btnIEqual.Text = "电流相等";
            this.btnIEqual.UseVisualStyleBackColor = true;
            this.btnIEqual.Click += new System.EventHandler(this.BtnIEqual_Click);
            // 
            // btnUEqual
            // 
            this.btnUEqual.Location = new System.Drawing.Point(306, 29);
            this.btnUEqual.Name = "btnUEqual";
            this.btnUEqual.Size = new System.Drawing.Size(75, 23);
            this.btnUEqual.TabIndex = 8;
            this.btnUEqual.Text = "电压相等";
            this.btnUEqual.UseVisualStyleBackColor = true;
            this.btnUEqual.Click += new System.EventHandler(this.BtnUEqual_Click);
            // 
            // buttonMinus
            // 
            this.buttonMinus.Location = new System.Drawing.Point(199, 75);
            this.buttonMinus.Name = "buttonMinus";
            this.buttonMinus.Size = new System.Drawing.Size(75, 23);
            this.buttonMinus.TabIndex = 7;
            this.buttonMinus.Text = "下调";
            this.buttonMinus.UseVisualStyleBackColor = true;
            this.buttonMinus.Click += new System.EventHandler(this.ButtonMinus_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(199, 29);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(75, 23);
            this.buttonUp.TabIndex = 6;
            this.buttonUp.Text = "上调";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.ButtonUp_Click);
            // 
            // textBoxStepSize
            // 
            this.textBoxStepSize.Location = new System.Drawing.Point(79, 80);
            this.textBoxStepSize.Name = "textBoxStepSize";
            this.textBoxStepSize.Size = new System.Drawing.Size(93, 21);
            this.textBoxStepSize.TabIndex = 5;
            this.textBoxStepSize.Text = "1.0";
            this.textBoxStepSize.Leave += new System.EventHandler(this.TextBoxStepSize_Leave);
            // 
            // labelStepSize
            // 
            this.labelStepSize.AutoSize = true;
            this.labelStepSize.Location = new System.Drawing.Point(14, 85);
            this.labelStepSize.Name = "labelStepSize";
            this.labelStepSize.Size = new System.Drawing.Size(29, 12);
            this.labelStepSize.TabIndex = 4;
            this.labelStepSize.Text = "步长";
            // 
            // comBoPassageCollection
            // 
            this.comBoPassageCollection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoPassageCollection.FormattingEnabled = true;
            this.comBoPassageCollection.Items.AddRange(new object[] {
            "UA",
            "UB",
            "UC",
            "U0",
            "IA",
            "IB",
            "IC",
            "I0"});
            this.comBoPassageCollection.Location = new System.Drawing.Point(79, 50);
            this.comBoPassageCollection.Name = "comBoPassageCollection";
            this.comBoPassageCollection.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comBoPassageCollection.Size = new System.Drawing.Size(93, 20);
            this.comBoPassageCollection.TabIndex = 3;
            // 
            // labelChoosePassage
            // 
            this.labelChoosePassage.AutoSize = true;
            this.labelChoosePassage.Location = new System.Drawing.Point(14, 55);
            this.labelChoosePassage.Name = "labelChoosePassage";
            this.labelChoosePassage.Size = new System.Drawing.Size(53, 12);
            this.labelChoosePassage.TabIndex = 2;
            this.labelChoosePassage.Text = "通道选择";
            // 
            // comBoAdjustAttr
            // 
            this.comBoAdjustAttr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoAdjustAttr.FormattingEnabled = true;
            this.comBoAdjustAttr.Items.AddRange(new object[] {
            "幅值",
            "相位",
            "频率"});
            this.comBoAdjustAttr.Location = new System.Drawing.Point(79, 20);
            this.comBoAdjustAttr.Name = "comBoAdjustAttr";
            this.comBoAdjustAttr.Size = new System.Drawing.Size(93, 20);
            this.comBoAdjustAttr.TabIndex = 1;
            // 
            // labelAdjustAttr
            // 
            this.labelAdjustAttr.AutoSize = true;
            this.labelAdjustAttr.Location = new System.Drawing.Point(14, 25);
            this.labelAdjustAttr.Name = "labelAdjustAttr";
            this.labelAdjustAttr.Size = new System.Drawing.Size(53, 12);
            this.labelAdjustAttr.TabIndex = 0;
            this.labelAdjustAttr.Text = "调节对象";
            // 
            // tabPageTime
            // 
            this.tabPageTime.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageTime.Controls.Add(this.ckbSystemTime);
            this.tabPageTime.Controls.Add(this.btnreadTime);
            this.tabPageTime.Controls.Add(this.btnSetTime);
            this.tabPageTime.Controls.Add(this.labelTerDateOfWeek);
            this.tabPageTime.Controls.Add(this.txbTerDateOfWeek);
            this.tabPageTime.Controls.Add(this.labelTerminalMillSec);
            this.tabPageTime.Controls.Add(this.txbTerminalMillSec);
            this.tabPageTime.Controls.Add(this.label6);
            this.tabPageTime.Controls.Add(this.labelTerminalSec);
            this.tabPageTime.Controls.Add(this.txbTerminalSec);
            this.tabPageTime.Controls.Add(this.labelTerminalMinute);
            this.tabPageTime.Controls.Add(this.txbTerminalMinute);
            this.tabPageTime.Controls.Add(this.labelTerminalHour);
            this.tabPageTime.Controls.Add(this.txbTerminalHour);
            this.tabPageTime.Controls.Add(this.labelTerminalDate);
            this.tabPageTime.Controls.Add(this.txbTerminaldate);
            this.tabPageTime.Controls.Add(this.label11);
            this.tabPageTime.Controls.Add(this.labelTerminalMonth);
            this.tabPageTime.Controls.Add(this.txbTerminalMonth);
            this.tabPageTime.Controls.Add(this.labelTerminalTear);
            this.tabPageTime.Controls.Add(this.txbTerminalYear);
            this.tabPageTime.Controls.Add(this.labelTerminalTime);
            this.tabPageTime.Controls.Add(this.labelDateOfWeek);
            this.tabPageTime.Controls.Add(this.textBoxDayOfWeek);
            this.tabPageTime.Controls.Add(this.labelMillSec);
            this.tabPageTime.Controls.Add(this.textBoxMillSec);
            this.tabPageTime.Controls.Add(this.label4);
            this.tabPageTime.Controls.Add(this.labelSecond);
            this.tabPageTime.Controls.Add(this.textBoxSecond);
            this.tabPageTime.Controls.Add(this.labelMinute);
            this.tabPageTime.Controls.Add(this.textBoxMinute);
            this.tabPageTime.Controls.Add(this.labelHour);
            this.tabPageTime.Controls.Add(this.textBoxHour);
            this.tabPageTime.Controls.Add(this.labeldate);
            this.tabPageTime.Controls.Add(this.textBoxDate);
            this.tabPageTime.Controls.Add(this.label3);
            this.tabPageTime.Controls.Add(this.labelMonth);
            this.tabPageTime.Controls.Add(this.textBoxMonth);
            this.tabPageTime.Controls.Add(this.labelYear);
            this.tabPageTime.Controls.Add(this.textBoxYear);
            this.tabPageTime.Controls.Add(this.labelTimeSetting);
            this.tabPageTime.Location = new System.Drawing.Point(4, 22);
            this.tabPageTime.Name = "tabPageTime";
            this.tabPageTime.Size = new System.Drawing.Size(1335, 475);
            this.tabPageTime.TabIndex = 8;
            this.tabPageTime.Text = "对时测试";
            // 
            // ckbSystemTime
            // 
            this.ckbSystemTime.AutoSize = true;
            this.ckbSystemTime.Location = new System.Drawing.Point(70, 188);
            this.ckbSystemTime.Name = "ckbSystemTime";
            this.ckbSystemTime.Size = new System.Drawing.Size(72, 16);
            this.ckbSystemTime.TabIndex = 40;
            this.ckbSystemTime.Text = "系统时间";
            this.ckbSystemTime.UseVisualStyleBackColor = true;
            this.ckbSystemTime.CheckedChanged += new System.EventHandler(this.CkbSystemTime_CheckedChanged);
            // 
            // btnreadTime
            // 
            this.btnreadTime.Location = new System.Drawing.Point(349, 181);
            this.btnreadTime.Name = "btnreadTime";
            this.btnreadTime.Size = new System.Drawing.Size(75, 23);
            this.btnreadTime.TabIndex = 39;
            this.btnreadTime.Text = "读取时间";
            this.btnreadTime.UseVisualStyleBackColor = true;
            this.btnreadTime.Click += new System.EventHandler(this.BtnreadTime_Click);
            // 
            // btnSetTime
            // 
            this.btnSetTime.Location = new System.Drawing.Point(212, 181);
            this.btnSetTime.Name = "btnSetTime";
            this.btnSetTime.Size = new System.Drawing.Size(75, 23);
            this.btnSetTime.TabIndex = 38;
            this.btnSetTime.Text = "设置时间";
            this.btnSetTime.UseVisualStyleBackColor = true;
            this.btnSetTime.Click += new System.EventHandler(this.BtnSetTime_Click);
            // 
            // labelTerDateOfWeek
            // 
            this.labelTerDateOfWeek.AutoSize = true;
            this.labelTerDateOfWeek.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTerDateOfWeek.Location = new System.Drawing.Point(699, 112);
            this.labelTerDateOfWeek.Name = "labelTerDateOfWeek";
            this.labelTerDateOfWeek.Size = new System.Drawing.Size(29, 12);
            this.labelTerDateOfWeek.TabIndex = 37;
            this.labelTerDateOfWeek.Text = "星期";
            // 
            // txbTerDateOfWeek
            // 
            this.txbTerDateOfWeek.Location = new System.Drawing.Point(733, 107);
            this.txbTerDateOfWeek.Name = "txbTerDateOfWeek";
            this.txbTerDateOfWeek.ReadOnly = true;
            this.txbTerDateOfWeek.Size = new System.Drawing.Size(54, 21);
            this.txbTerDateOfWeek.TabIndex = 36;
            // 
            // labelTerminalMillSec
            // 
            this.labelTerminalMillSec.AutoSize = true;
            this.labelTerminalMillSec.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTerminalMillSec.Location = new System.Drawing.Point(628, 113);
            this.labelTerminalMillSec.Name = "labelTerminalMillSec";
            this.labelTerminalMillSec.Size = new System.Drawing.Size(29, 12);
            this.labelTerminalMillSec.TabIndex = 35;
            this.labelTerminalMillSec.Text = "毫秒";
            // 
            // txbTerminalMillSec
            // 
            this.txbTerminalMillSec.Location = new System.Drawing.Point(578, 108);
            this.txbTerminalMillSec.Name = "txbTerminalMillSec";
            this.txbTerminalMillSec.ReadOnly = true;
            this.txbTerminalMillSec.Size = new System.Drawing.Size(42, 21);
            this.txbTerminalMillSec.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(660, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 12);
            this.label6.TabIndex = 33;
            // 
            // labelTerminalSec
            // 
            this.labelTerminalSec.AutoSize = true;
            this.labelTerminalSec.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTerminalSec.Location = new System.Drawing.Point(555, 113);
            this.labelTerminalSec.Name = "labelTerminalSec";
            this.labelTerminalSec.Size = new System.Drawing.Size(17, 12);
            this.labelTerminalSec.TabIndex = 32;
            this.labelTerminalSec.Text = "秒";
            // 
            // txbTerminalSec
            // 
            this.txbTerminalSec.Location = new System.Drawing.Point(512, 109);
            this.txbTerminalSec.Name = "txbTerminalSec";
            this.txbTerminalSec.ReadOnly = true;
            this.txbTerminalSec.Size = new System.Drawing.Size(37, 21);
            this.txbTerminalSec.TabIndex = 31;
            // 
            // labelTerminalMinute
            // 
            this.labelTerminalMinute.AutoSize = true;
            this.labelTerminalMinute.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTerminalMinute.Location = new System.Drawing.Point(489, 113);
            this.labelTerminalMinute.Name = "labelTerminalMinute";
            this.labelTerminalMinute.Size = new System.Drawing.Size(17, 12);
            this.labelTerminalMinute.TabIndex = 30;
            this.labelTerminalMinute.Text = "分";
            // 
            // txbTerminalMinute
            // 
            this.txbTerminalMinute.Location = new System.Drawing.Point(440, 109);
            this.txbTerminalMinute.Name = "txbTerminalMinute";
            this.txbTerminalMinute.ReadOnly = true;
            this.txbTerminalMinute.Size = new System.Drawing.Size(43, 21);
            this.txbTerminalMinute.TabIndex = 29;
            // 
            // labelTerminalHour
            // 
            this.labelTerminalHour.AutoSize = true;
            this.labelTerminalHour.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTerminalHour.Location = new System.Drawing.Point(419, 112);
            this.labelTerminalHour.Name = "labelTerminalHour";
            this.labelTerminalHour.Size = new System.Drawing.Size(17, 12);
            this.labelTerminalHour.TabIndex = 28;
            this.labelTerminalHour.Text = "时";
            // 
            // txbTerminalHour
            // 
            this.txbTerminalHour.Location = new System.Drawing.Point(365, 108);
            this.txbTerminalHour.Name = "txbTerminalHour";
            this.txbTerminalHour.ReadOnly = true;
            this.txbTerminalHour.Size = new System.Drawing.Size(48, 21);
            this.txbTerminalHour.TabIndex = 27;
            // 
            // labelTerminalDate
            // 
            this.labelTerminalDate.AutoSize = true;
            this.labelTerminalDate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTerminalDate.Location = new System.Drawing.Point(346, 111);
            this.labelTerminalDate.Name = "labelTerminalDate";
            this.labelTerminalDate.Size = new System.Drawing.Size(17, 12);
            this.labelTerminalDate.TabIndex = 26;
            this.labelTerminalDate.Text = "日";
            // 
            // txbTerminaldate
            // 
            this.txbTerminaldate.Location = new System.Drawing.Point(299, 107);
            this.txbTerminaldate.Name = "txbTerminaldate";
            this.txbTerminaldate.ReadOnly = true;
            this.txbTerminaldate.Size = new System.Drawing.Size(42, 21);
            this.txbTerminaldate.TabIndex = 25;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(347, 111);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(0, 12);
            this.label11.TabIndex = 24;
            // 
            // labelTerminalMonth
            // 
            this.labelTerminalMonth.AutoSize = true;
            this.labelTerminalMonth.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTerminalMonth.Location = new System.Drawing.Point(276, 112);
            this.labelTerminalMonth.Name = "labelTerminalMonth";
            this.labelTerminalMonth.Size = new System.Drawing.Size(17, 12);
            this.labelTerminalMonth.TabIndex = 23;
            this.labelTerminalMonth.Text = "月";
            // 
            // txbTerminalMonth
            // 
            this.txbTerminalMonth.Location = new System.Drawing.Point(233, 108);
            this.txbTerminalMonth.Name = "txbTerminalMonth";
            this.txbTerminalMonth.ReadOnly = true;
            this.txbTerminalMonth.Size = new System.Drawing.Size(37, 21);
            this.txbTerminalMonth.TabIndex = 22;
            // 
            // labelTerminalTear
            // 
            this.labelTerminalTear.AutoSize = true;
            this.labelTerminalTear.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTerminalTear.Location = new System.Drawing.Point(210, 111);
            this.labelTerminalTear.Name = "labelTerminalTear";
            this.labelTerminalTear.Size = new System.Drawing.Size(17, 12);
            this.labelTerminalTear.TabIndex = 21;
            this.labelTerminalTear.Text = "年";
            // 
            // txbTerminalYear
            // 
            this.txbTerminalYear.Location = new System.Drawing.Point(129, 107);
            this.txbTerminalYear.Name = "txbTerminalYear";
            this.txbTerminalYear.ReadOnly = true;
            this.txbTerminalYear.Size = new System.Drawing.Size(75, 21);
            this.txbTerminalYear.TabIndex = 20;
            // 
            // labelTerminalTime
            // 
            this.labelTerminalTime.AutoSize = true;
            this.labelTerminalTime.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTerminalTime.Location = new System.Drawing.Point(58, 110);
            this.labelTerminalTime.Name = "labelTerminalTime";
            this.labelTerminalTime.Size = new System.Drawing.Size(65, 12);
            this.labelTerminalTime.TabIndex = 19;
            this.labelTerminalTime.Text = "终端时间：";
            // 
            // labelDateOfWeek
            // 
            this.labelDateOfWeek.AutoSize = true;
            this.labelDateOfWeek.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDateOfWeek.Location = new System.Drawing.Point(699, 50);
            this.labelDateOfWeek.Name = "labelDateOfWeek";
            this.labelDateOfWeek.Size = new System.Drawing.Size(29, 12);
            this.labelDateOfWeek.TabIndex = 18;
            this.labelDateOfWeek.Text = "星期";
            // 
            // textBoxDayOfWeek
            // 
            this.textBoxDayOfWeek.Location = new System.Drawing.Point(733, 45);
            this.textBoxDayOfWeek.Name = "textBoxDayOfWeek";
            this.textBoxDayOfWeek.ReadOnly = true;
            this.textBoxDayOfWeek.Size = new System.Drawing.Size(54, 21);
            this.textBoxDayOfWeek.TabIndex = 17;
            // 
            // labelMillSec
            // 
            this.labelMillSec.AutoSize = true;
            this.labelMillSec.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMillSec.Location = new System.Drawing.Point(628, 51);
            this.labelMillSec.Name = "labelMillSec";
            this.labelMillSec.Size = new System.Drawing.Size(29, 12);
            this.labelMillSec.TabIndex = 16;
            this.labelMillSec.Text = "毫秒";
            // 
            // textBoxMillSec
            // 
            this.textBoxMillSec.Location = new System.Drawing.Point(578, 46);
            this.textBoxMillSec.Name = "textBoxMillSec";
            this.textBoxMillSec.Size = new System.Drawing.Size(42, 21);
            this.textBoxMillSec.TabIndex = 15;
            this.textBoxMillSec.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxMillSec_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(660, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 12);
            this.label4.TabIndex = 14;
            // 
            // labelSecond
            // 
            this.labelSecond.AutoSize = true;
            this.labelSecond.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelSecond.Location = new System.Drawing.Point(555, 51);
            this.labelSecond.Name = "labelSecond";
            this.labelSecond.Size = new System.Drawing.Size(17, 12);
            this.labelSecond.TabIndex = 13;
            this.labelSecond.Text = "秒";
            // 
            // textBoxSecond
            // 
            this.textBoxSecond.Location = new System.Drawing.Point(512, 47);
            this.textBoxSecond.Name = "textBoxSecond";
            this.textBoxSecond.Size = new System.Drawing.Size(37, 21);
            this.textBoxSecond.TabIndex = 12;
            this.textBoxSecond.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxSecond_Validating);
            // 
            // labelMinute
            // 
            this.labelMinute.AutoSize = true;
            this.labelMinute.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMinute.Location = new System.Drawing.Point(489, 51);
            this.labelMinute.Name = "labelMinute";
            this.labelMinute.Size = new System.Drawing.Size(17, 12);
            this.labelMinute.TabIndex = 11;
            this.labelMinute.Text = "分";
            // 
            // textBoxMinute
            // 
            this.textBoxMinute.Location = new System.Drawing.Point(440, 47);
            this.textBoxMinute.Name = "textBoxMinute";
            this.textBoxMinute.Size = new System.Drawing.Size(43, 21);
            this.textBoxMinute.TabIndex = 10;
            this.textBoxMinute.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxMinute_Validating);
            // 
            // labelHour
            // 
            this.labelHour.AutoSize = true;
            this.labelHour.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelHour.Location = new System.Drawing.Point(419, 50);
            this.labelHour.Name = "labelHour";
            this.labelHour.Size = new System.Drawing.Size(17, 12);
            this.labelHour.TabIndex = 9;
            this.labelHour.Text = "时";
            // 
            // textBoxHour
            // 
            this.textBoxHour.Location = new System.Drawing.Point(365, 46);
            this.textBoxHour.Name = "textBoxHour";
            this.textBoxHour.Size = new System.Drawing.Size(48, 21);
            this.textBoxHour.TabIndex = 8;
            this.textBoxHour.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxHour_Validating);
            // 
            // labeldate
            // 
            this.labeldate.AutoSize = true;
            this.labeldate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labeldate.Location = new System.Drawing.Point(346, 49);
            this.labeldate.Name = "labeldate";
            this.labeldate.Size = new System.Drawing.Size(17, 12);
            this.labeldate.TabIndex = 7;
            this.labeldate.Text = "日";
            // 
            // textBoxDate
            // 
            this.textBoxDate.Location = new System.Drawing.Point(299, 45);
            this.textBoxDate.Name = "textBoxDate";
            this.textBoxDate.Size = new System.Drawing.Size(42, 21);
            this.textBoxDate.TabIndex = 6;
            this.textBoxDate.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxDate_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(347, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 12);
            this.label3.TabIndex = 5;
            // 
            // labelMonth
            // 
            this.labelMonth.AutoSize = true;
            this.labelMonth.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMonth.Location = new System.Drawing.Point(276, 50);
            this.labelMonth.Name = "labelMonth";
            this.labelMonth.Size = new System.Drawing.Size(17, 12);
            this.labelMonth.TabIndex = 4;
            this.labelMonth.Text = "月";
            // 
            // textBoxMonth
            // 
            this.textBoxMonth.Location = new System.Drawing.Point(233, 46);
            this.textBoxMonth.Name = "textBoxMonth";
            this.textBoxMonth.Size = new System.Drawing.Size(37, 21);
            this.textBoxMonth.TabIndex = 3;
            this.textBoxMonth.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxMonth_Validating);
            // 
            // labelYear
            // 
            this.labelYear.AutoSize = true;
            this.labelYear.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelYear.Location = new System.Drawing.Point(210, 49);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(17, 12);
            this.labelYear.TabIndex = 2;
            this.labelYear.Text = "年";
            // 
            // textBoxYear
            // 
            this.textBoxYear.Location = new System.Drawing.Point(129, 45);
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.Size = new System.Drawing.Size(75, 21);
            this.textBoxYear.TabIndex = 1;
            this.textBoxYear.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxYear_Validating);
            // 
            // labelTimeSetting
            // 
            this.labelTimeSetting.AutoSize = true;
            this.labelTimeSetting.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTimeSetting.Location = new System.Drawing.Point(58, 48);
            this.labelTimeSetting.Name = "labelTimeSetting";
            this.labelTimeSetting.Size = new System.Drawing.Size(65, 12);
            this.labelTimeSetting.TabIndex = 0;
            this.labelTimeSetting.Text = "设置时间：";
            // 
            // tabControlErrorAnalyse
            // 
            this.tabControlErrorAnalyse.Location = new System.Drawing.Point(0, 0);
            this.tabControlErrorAnalyse.Name = "tabControlErrorAnalyse";
            this.tabControlErrorAnalyse.Size = new System.Drawing.Size(200, 100);
            this.tabControlErrorAnalyse.TabIndex = 0;
            // 
            // btngetTeleMeterData
            // 
            this.btngetTeleMeterData.Location = new System.Drawing.Point(0, 0);
            this.btngetTeleMeterData.Name = "btngetTeleMeterData";
            this.btngetTeleMeterData.Size = new System.Drawing.Size(75, 23);
            this.btngetTeleMeterData.TabIndex = 0;
            // 
            // buttonGenerateReport
            // 
            this.buttonGenerateReport.Location = new System.Drawing.Point(0, 0);
            this.buttonGenerateReport.Name = "buttonGenerateReport";
            this.buttonGenerateReport.Size = new System.Drawing.Size(75, 23);
            this.buttonGenerateReport.TabIndex = 0;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(0, 0);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 0;
            // 
            // groupBoxError
            // 
            this.groupBoxError.Location = new System.Drawing.Point(0, 0);
            this.groupBoxError.Name = "groupBoxError";
            this.groupBoxError.Size = new System.Drawing.Size(200, 100);
            this.groupBoxError.TabIndex = 0;
            this.groupBoxError.TabStop = false;
            // 
            // textBoxErrorRange
            // 
            this.textBoxErrorRange.Location = new System.Drawing.Point(0, 0);
            this.textBoxErrorRange.Name = "textBoxErrorRange";
            this.textBoxErrorRange.Size = new System.Drawing.Size(100, 21);
            this.textBoxErrorRange.TabIndex = 0;
            // 
            // labelErrorRange
            // 
            this.labelErrorRange.Location = new System.Drawing.Point(0, 0);
            this.labelErrorRange.Name = "labelErrorRange";
            this.labelErrorRange.Size = new System.Drawing.Size(100, 23);
            this.labelErrorRange.TabIndex = 0;
            // 
            // buttonGetError
            // 
            this.buttonGetError.Location = new System.Drawing.Point(0, 0);
            this.buttonGetError.Name = "buttonGetError";
            this.buttonGetError.Size = new System.Drawing.Size(75, 23);
            this.buttonGetError.TabIndex = 0;
            // 
            // textBoxTime
            // 
            this.textBoxTime.Location = new System.Drawing.Point(0, 0);
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.Size = new System.Drawing.Size(100, 21);
            this.textBoxTime.TabIndex = 0;
            // 
            // labelTime
            // 
            this.labelTime.Location = new System.Drawing.Point(0, 0);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(100, 23);
            this.labelTime.TabIndex = 0;
            // 
            // dataGridViewWuCha
            // 
            this.dataGridViewWuCha.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewWuCha.Name = "dataGridViewWuCha";
            this.dataGridViewWuCha.Size = new System.Drawing.Size(240, 150);
            this.dataGridViewWuCha.TabIndex = 0;
            // 
            // ColumnGroup
            // 
            this.ColumnGroup.Name = "ColumnGroup";
            // 
            // ColumnAttr
            // 
            this.ColumnAttr.Name = "ColumnAttr";
            // 
            // ColumnSetValue
            // 
            this.ColumnSetValue.Name = "ColumnSetValue";
            // 
            // ColumnTestValue
            // 
            this.ColumnTestValue.Name = "ColumnTestValue";
            // 
            // ColumnError
            // 
            this.ColumnError.Name = "ColumnError";
            // 
            // groupBoxTestMethod
            // 
            this.groupBoxTestMethod.Location = new System.Drawing.Point(0, 0);
            this.groupBoxTestMethod.Name = "groupBoxTestMethod";
            this.groupBoxTestMethod.Size = new System.Drawing.Size(200, 100);
            this.groupBoxTestMethod.TabIndex = 0;
            this.groupBoxTestMethod.TabStop = false;
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(0, 0);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 0;
            // 
            // dataGridViewVV
            // 
            this.dataGridViewVV.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewVV.Name = "dataGridViewVV";
            this.dataGridViewVV.Size = new System.Drawing.Size(240, 150);
            this.dataGridViewVV.TabIndex = 0;
            // 
            // UAVV
            // 
            this.UAVV.Name = "UAVV";
            // 
            // UBVV
            // 
            this.UBVV.Name = "UBVV";
            // 
            // UCVV
            // 
            this.UCVV.Name = "UCVV";
            // 
            // U0VV
            // 
            this.U0VV.Name = "U0VV";
            // 
            // IAVV
            // 
            this.IAVV.Name = "IAVV";
            // 
            // IBVV
            // 
            this.IBVV.Name = "IBVV";
            // 
            // ICVV
            // 
            this.ICVV.Name = "ICVV";
            // 
            // I0VV
            // 
            this.I0VV.Name = "I0VV";
            // 
            // dataGridViewYY
            // 
            this.dataGridViewYY.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewYY.Name = "dataGridViewYY";
            this.dataGridViewYY.Size = new System.Drawing.Size(240, 150);
            this.dataGridViewYY.TabIndex = 0;
            // 
            // UABYY
            // 
            this.UABYY.Name = "UABYY";
            // 
            // UBCYY
            // 
            this.UBCYY.Name = "UBCYY";
            // 
            // IAYY
            // 
            this.IAYY.Name = "IAYY";
            // 
            // IBYY
            // 
            this.IBYY.Name = "IBYY";
            // 
            // ICYY
            // 
            this.ICYY.Name = "ICYY";
            // 
            // I0YY
            // 
            this.I0YY.Name = "I0YY";
            // 
            // checkBoxYY
            // 
            this.checkBoxYY.Location = new System.Drawing.Point(0, 0);
            this.checkBoxYY.Name = "checkBoxYY";
            this.checkBoxYY.Size = new System.Drawing.Size(104, 24);
            this.checkBoxYY.TabIndex = 0;
            // 
            // checkBoxVV
            // 
            this.checkBoxVV.Location = new System.Drawing.Point(0, 0);
            this.checkBoxVV.Name = "checkBoxVV";
            this.checkBoxVV.Size = new System.Drawing.Size(104, 24);
            this.checkBoxVV.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelNetStatus,
            this.toolStripStatusLabelLink});
            this.statusStrip1.Location = new System.Drawing.Point(0, 554);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1367, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip";
            // 
            // toolStripStatusLabelNetStatus
            // 
            this.toolStripStatusLabelNetStatus.Name = "toolStripStatusLabelNetStatus";
            this.toolStripStatusLabelNetStatus.Size = new System.Drawing.Size(89, 17);
            this.toolStripStatusLabelNetStatus.Text = "104连接状态：";
            // 
            // toolStripStatusLabelLink
            // 
            this.toolStripStatusLabelLink.Name = "toolStripStatusLabelLink";
            this.toolStripStatusLabelLink.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabelLink.Text = "通讯连接";
            // 
            // errorProviderTime
            // 
            this.errorProviderTime.ContainerControl = this;
            // 
            // tpfaultRecord
            // 
            this.tpfaultRecord.BackColor = System.Drawing.SystemColors.Control;
            this.tpfaultRecord.Controls.Add(this.btnOpenFaultRecord);
            this.tpfaultRecord.Location = new System.Drawing.Point(4, 22);
            this.tpfaultRecord.Name = "tpfaultRecord";
            this.tpfaultRecord.Size = new System.Drawing.Size(1335, 475);
            this.tpfaultRecord.TabIndex = 9;
            this.tpfaultRecord.Text = "故障录波";
            // 
            // btnOpenFaultRecord
            // 
            this.btnOpenFaultRecord.Location = new System.Drawing.Point(217, 87);
            this.btnOpenFaultRecord.Name = "btnOpenFaultRecord";
            this.btnOpenFaultRecord.Size = new System.Drawing.Size(107, 23);
            this.btnOpenFaultRecord.TabIndex = 0;
            this.btnOpenFaultRecord.Text = "打开故障录波";
            this.btnOpenFaultRecord.UseVisualStyleBackColor = true;
            this.btnOpenFaultRecord.Click += new System.EventHandler(this.BtnOpenFaultRecord_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1367, 576);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "测试平台";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.tabControl1.ResumeLayout(false);
            this.tabControlLink.ResumeLayout(false);
            this.groupBox104Link.ResumeLayout(false);
            this.groupBox104Link.PerformLayout();
            this.groupBoxLink.ResumeLayout(false);
            this.contextMenuStripInfo.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabPageTeleMeterTable.ResumeLayout(false);
            this.panelTeleMeterTable.ResumeLayout(false);
            this.groupBoxTeleMeterTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeleMeterTable)).EndInit();
            this.ctxMenuStripTeleMeterTable.ResumeLayout(false);
            this.tabPageTeleSingalTable.ResumeLayout(false);
            this.panelTeleSingalTable.ResumeLayout(false);
            this.groupBoxTeleSingalTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeleSingalTable)).EndInit();
            this.ctxMenuStripTeleSingalTable.ResumeLayout(false);
            this.tabPageTeleControlTable.ResumeLayout(false);
            this.panelTeleControlTable.ResumeLayout(false);
            this.groupBoxTeleControlTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeleControlTable)).EndInit();
            this.ctxMenuStripTeleControlTable.ResumeLayout(false);
            this.tabPageTeleSinaglling.ResumeLayout(false);
            this.tabPageTeleSinaglling.PerformLayout();
            this.contextMenuStripTeleSingal.ResumeLayout(false);
            this.panelOridinaryTeleSingal.ResumeLayout(false);
            this.panelOridinaryTeleSingal.PerformLayout();
            this.panelTeleSingalResolution.ResumeLayout(false);
            this.panelTeleSingalResolution.PerformLayout();
            this.panelTeleSingallingStorm.ResumeLayout(false);
            this.panelTeleSingallingStorm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownCount)).EndInit();
            this.tabPageThreeTele.ResumeLayout(false);
            this.contextMenuStripTeleControl.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewError)).EndInit();
            this.groupBoxSettingValue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPassageData)).EndInit();
            this.groupBoxOutputSetting.ResumeLayout(false);
            this.groupBoxOutputSetting.PerformLayout();
            this.groupBoxUAUIParaSetting.ResumeLayout(false);
            this.groupBoxUAUIParaSetting.PerformLayout();
            this.tabPageTime.ResumeLayout(false);
            this.tabPageTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWuCha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewYY)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderTime)).EndInit();
            this.tpfaultRecord.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabControlErrorAnalyse;
        private System.Windows.Forms.GroupBox groupBoxTestMethod;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.DataGridView dataGridViewVV;
        private System.Windows.Forms.DataGridView dataGridViewYY;
        private System.Windows.Forms.DataGridViewTextBoxColumn UABYY;
        private System.Windows.Forms.DataGridViewTextBoxColumn UBCYY;
        private System.Windows.Forms.DataGridViewTextBoxColumn IAYY;
        private System.Windows.Forms.DataGridViewTextBoxColumn IBYY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ICYY;
        private System.Windows.Forms.DataGridViewTextBoxColumn I0YY;
        private System.Windows.Forms.CheckBox checkBoxYY;
        private System.Windows.Forms.CheckBox checkBoxVV;
        private System.Windows.Forms.GroupBox groupBoxError;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.TextBox textBoxErrorRange;
        private System.Windows.Forms.Label labelErrorRange;
        private System.Windows.Forms.Button buttonGetError;
        private System.Windows.Forms.TextBox textBoxTime;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.DataGridView dataGridViewWuCha;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAttr;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSetValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTestValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnError;
        private System.Windows.Forms.Button buttonGenerateReport;
        private System.Windows.Forms.DataGridViewTextBoxColumn UAVV;
        private System.Windows.Forms.DataGridViewTextBoxColumn UBVV;
        private System.Windows.Forms.DataGridViewTextBoxColumn UCVV;
        private System.Windows.Forms.DataGridViewTextBoxColumn U0VV;
        private System.Windows.Forms.DataGridViewTextBoxColumn IAVV;
        private System.Windows.Forms.DataGridViewTextBoxColumn IBVV;
        private System.Windows.Forms.DataGridViewTextBoxColumn ICVV;
        private System.Windows.Forms.DataGridViewTextBoxColumn I0VV;
        private System.Windows.Forms.TabPage tabPageTeleSinaglling;
        private System.Windows.Forms.RadioButton radBtnTelesignallingStorm;
        private System.Windows.Forms.Panel panelTeleSingallingStorm;
        private System.Windows.Forms.Label labelOpenRange;
        private System.Windows.Forms.ComboBox comBoEndRange;
        private System.Windows.Forms.ComboBox comBoStartRange;
        private System.Windows.Forms.Label labelWavyLine;
        private System.Windows.Forms.Label labelTeleSingalPluseWidth;
        private System.Windows.Forms.TextBox textBoxTelesingalPulseWidth;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.Label labelExecuteCount;
        private System.Windows.Forms.NumericUpDown numUpDownCount;
        private System.Windows.Forms.Panel panelTeleSingalResolution;
        private System.Windows.Forms.Label labelResolution;
        private System.Windows.Forms.Label labelResolutionPulseWidthUnit;
        private System.Windows.Forms.TextBox textBoxTelesingalResolution;
        private System.Windows.Forms.Label labelTeleSingalResolution;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comBoResolutonEndRange;
        private System.Windows.Forms.ComboBox comBoResolutionStartRange;
        private System.Windows.Forms.Label labelResolutionOpenRange;
        private System.Windows.Forms.RadioButton radioBtnTeleSingalResolution;
        private System.Windows.Forms.Label labelResolutionUnit;
        private System.Windows.Forms.TextBox textBoxResolution;
        private System.Windows.Forms.Panel panelOridinaryTeleSingal;
        private System.Windows.Forms.RadioButton radioBtnOridinaryTeleSingal;
        private System.Windows.Forms.RadioButton radionBtnManyOpen;
        private System.Windows.Forms.RadioButton radioBtnSingalOpen;
        private System.Windows.Forms.ComboBox comBoSingalOpen;
        private System.Windows.Forms.Label labelLine;
        private System.Windows.Forms.ComboBox comBoManyOpenEnd;
        private System.Windows.Forms.ComboBox comBoManyOpenStart;
        private System.Windows.Forms.Button btnOpenAndOpen;
        private System.Windows.Forms.Button btnOpenAndClose;
        private System.Windows.Forms.TabPage tabPageThreeTele;
        private System.Windows.Forms.GroupBox groupBoxUAUIParaSetting;
        private System.Windows.Forms.ComboBox comBoAdjustAttr;
        private System.Windows.Forms.Label labelAdjustAttr;
        private System.Windows.Forms.Label labelStepSize;
        private System.Windows.Forms.ComboBox comBoPassageCollection;
        private System.Windows.Forms.Label labelChoosePassage;
        private System.Windows.Forms.TextBox textBoxStepSize;
        private System.Windows.Forms.Button buttonMinus;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button btnIEqual;
        private System.Windows.Forms.Button btnUEqual;
        private System.Windows.Forms.Button btnReverseOrderBalance;
        private System.Windows.Forms.Button btnPositiveOrderBalance;
        private System.Windows.Forms.Label labelOutputPassage;
        private System.Windows.Forms.ComboBox ConBoRatedIPerOutput;
        private System.Windows.Forms.Label labelRatedIPerOutput;
        private System.Windows.Forms.ComboBox ConBoRatedVPerOutput;
        private System.Windows.Forms.Label labelRatedVPerOutput;
        private System.Windows.Forms.ComboBox comBoOutputPassage;
        private System.Windows.Forms.GroupBox groupBoxOutputSetting;
        private System.Windows.Forms.Button btnTeleSingalTest;
        private System.Windows.Forms.Button btngetTeleMeterData;
        private System.Windows.Forms.RichTextBox richTextBoxControllInfo;
        private System.Windows.Forms.Label labelSingleOrDouble;
        private System.Windows.Forms.ComboBox comboBoxSingleOrDouble;
        private System.Windows.Forms.RichTextBox richTextBoxTeleSingalInfo;
        private System.Windows.Forms.ComboBox comBoCloseOrOpen;
        private System.Windows.Forms.Label labelCloseOrOpen;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTeleControl;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClearTeleContorol;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTeleSingal;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTeleSingal;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripInfo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClear;
        private System.Windows.Forms.TabPage tabControlLink;
        private System.Windows.Forms.GroupBox groupBox104Link;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Label labelLocalIP;
        private System.Windows.Forms.GroupBox groupBoxLink;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLinkDevice;
        private System.Windows.Forms.Label labelMachineIp;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label labelPcIp;
        private System.Windows.Forms.TextBox textONLLYIP;
        private System.Windows.Forms.TextBox textPCIP;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnStopTest;
        private System.Windows.Forms.Button btnBeginTest;
        private System.Windows.Forms.Button btnSystemConfig;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelNetStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelLink;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridViewError;
        private System.Windows.Forms.DataGridViewTextBoxColumn attr;
        private System.Windows.Forms.DataGridViewTextBoxColumn attrValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn measureValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn error;
        private System.Windows.Forms.GroupBox groupBoxSettingValue;
        private System.Windows.Forms.DataGridView dataGridViewPassageData;
        private System.Windows.Forms.Button buttonTestTeleSingal;
        private System.Windows.Forms.TextBox textBoxStandardError;
        private System.Windows.Forms.Label labelStandardError;
        private System.Windows.Forms.Button btnTestTeleControl;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.TabPage tabPageTeleMeterTable;
        private System.Windows.Forms.TabPage tabPageTeleSingalTable;
        private System.Windows.Forms.TabPage tabPageTeleControlTable;
        private System.Windows.Forms.Button btnAddMeterTable;
        private System.Windows.Forms.Panel panelTeleMeterTable;
        private System.Windows.Forms.GroupBox groupBoxTeleMeterTable;
        private System.Windows.Forms.DataGridView dataGridViewTeleMeterTable;
        private System.Windows.Forms.Button btnAddTeleSingalTable;
        private System.Windows.Forms.Panel panelTeleSingalTable;
        private System.Windows.Forms.GroupBox groupBoxTeleSingalTable;
        private System.Windows.Forms.DataGridView dataGridViewTeleSingalTable;
        private System.Windows.Forms.Panel panelTeleControlTable;
        private System.Windows.Forms.GroupBox groupBoxTeleControlTable;
        private System.Windows.Forms.DataGridView dataGridViewTeleControlTable;
        private System.Windows.Forms.ContextMenuStrip ctxMenuStripTeleMeterTable;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSave;
        private System.Windows.Forms.ContextMenuStrip ctxMenuStripTeleSingalTable;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveSingalTable;
        private System.Windows.Forms.ContextMenuStrip ctxMenuStripTeleControlTable;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveControlTable;
        private System.Windows.Forms.ToolStripMenuItem tsMenuItemReloadMeterTable;
        private System.Windows.Forms.ToolStripMenuItem tsMenuItemReloadSingalTable;
        private System.Windows.Forms.ToolStripMenuItem tsMenuItemReloadControlTable;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnCancelSelectAll;
        private TabPage tabPageTime;
        private System.Windows.Forms.Label labelHour;
        private System.Windows.Forms.TextBox textBoxHour;
        private System.Windows.Forms.Label labeldate;
        private System.Windows.Forms.TextBox textBoxDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelMonth;
        private System.Windows.Forms.Label labelYear;
        private System.Windows.Forms.TextBox textBoxYear;
        private System.Windows.Forms.Label labelTimeSetting;
        private System.Windows.Forms.Label labelTerDateOfWeek;
        private System.Windows.Forms.TextBox txbTerDateOfWeek;
        private System.Windows.Forms.Label labelTerminalMillSec;
        private System.Windows.Forms.TextBox txbTerminalMillSec;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelTerminalSec;
        private System.Windows.Forms.TextBox txbTerminalSec;
        private System.Windows.Forms.Label labelTerminalMinute;
        private System.Windows.Forms.TextBox txbTerminalMinute;
        private System.Windows.Forms.Label labelTerminalHour;
        private System.Windows.Forms.TextBox txbTerminalHour;
        private System.Windows.Forms.Label labelTerminalDate;
        private System.Windows.Forms.TextBox txbTerminaldate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelTerminalMonth;
        private System.Windows.Forms.TextBox txbTerminalMonth;
        private System.Windows.Forms.Label labelTerminalTear;
        private System.Windows.Forms.TextBox txbTerminalYear;
        private System.Windows.Forms.Label labelTerminalTime;
        private System.Windows.Forms.Label labelDateOfWeek;
        private System.Windows.Forms.TextBox textBoxDayOfWeek;
        private System.Windows.Forms.Label labelMillSec;
        private System.Windows.Forms.TextBox textBoxMillSec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelSecond;
        private System.Windows.Forms.TextBox textBoxSecond;
        private System.Windows.Forms.Label labelMinute;
        private System.Windows.Forms.TextBox textBoxMinute;
        private System.Windows.Forms.Button btnreadTime;
        private System.Windows.Forms.Button btnSetTime;
        private System.Windows.Forms.CheckBox ckbSystemTime;
        private ErrorProvider errorProviderTime;
        private System.Windows.Forms.TextBox textBoxMonth;
        private TabPage tpfaultRecord;
        private System.Windows.Forms.Button btnOpenFaultRecord;
    }
}

