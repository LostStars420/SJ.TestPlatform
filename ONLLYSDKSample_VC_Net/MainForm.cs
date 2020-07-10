using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Linq;
using OnllyDataLib;
using OnllyCalcEngineLib;
// 使用 ONLLYTS 动态库中的函数
using OnllyTs;
using Sojo.TestPlatform.PlatformModel.OnllyOperation;
using Sojo.TestPlatform.ControlPlatform.View;
using Sojo.TestPlatform.ControlPlatform.Util;
using Sojo.TestPlatform.PlatformModel.Util;
using Sojo.TestPlatform.PlatformModel.DatabaseHelper;
using System.Data;
using System.Threading;
using System.Timers;
using Sojo.Checkplatform.libcollect;
using Sojo.TestPlatform.PlatformModel.Model;
using System.Drawing;
using System.Collections;
using System.ComponentModel;

namespace Sojo.TestPlatform.ControlPlatform
{
   
    // 定义 WIN32 中的变量类型
    using HRESULT = System.UInt32;
    using HANDLE = System.IntPtr;
    using HWND = System.IntPtr;
    using BOOL = System.Int32;
    using UINT16 = System.UInt16;
    using INT32 = System.Int32;
    using LONG = System.Int32;
    using ULONG = System.UInt32;
    using VARIANT = System.Object;
    using BYTE = System.Char;
    using WPARAM = System.Int32;
    using LPARAM = System.Int32;


    public unsafe partial class MainForm : Form
    {

        public readonly HRESULT S_OK = 0;
        public readonly BOOL TRUE = 1;
        public readonly BOOL FALSE = 0;
        public const int ratedVoltage = 220;   //额定电压
        public const int ratedCurrent = 50;    //额定电流

        //测试服务器消息
        public struct OnllySERVERMSG
        {
            public ULONG nMsgID;	//消息ID
            public UINT16 nType;	//消息来源 ：硬件消息、测试组件消息
            public WPARAM wParam;
            public LPARAM lParam;
            public BYTE* pMsg;		//有关消息的数据域
            public ULONG uLen;		//消息数据域的长度
        };

        public string m_strIp_PC;       //PC 机的 IP 地址
        public string m_strIp_Onlly;    //测试仪的 IP 地址
        public int m_bTestBeginFlag;    //试验开始标志: 1--开始试验


        public IGenericData m_spTestParam = new GenericData();
        public OnllyResult_UI m_result = new OnllyResult_UI();
        public UISet[] m_Up = new UISet[6]; //6 路电压
        public UISet[] m_Ip = new UISet[6]; //6 路电流
        public int m_nPowerMode;		//0--6路电流均 < 10A; 	1--任何一相>=10A;

        private LinkDevice linkDevice = new LinkDevice();  //操作测试仪接口函数


        //误差分析界面的VV接线和YY接线方案数据存储
        private Dictionary<string, float> valuesVV = new Dictionary<string, float>();
        private List<Dictionary<string, float>> valuesCollectionVV = new List<Dictionary<string, float>>();
        private Dictionary<string, float> valuesYY = new Dictionary<string, float>();
        private List<Dictionary<string, float>> valuesCollectionYY = new List<Dictionary<string, float>>();

        //存储遥测数据
        private List<float> teleMeteringData = new List<float>();
        //存储遥信数据
        private List<Tuple<int, bool>> teleSingalData = new List<Tuple<int, bool>>();
        //数据库路径
        string dbPath = @"Database\das.db";

        //104获取事件委托
        private static IEC104Interface iec;
        private static IEC104InterfaceOut iecOut;


        public MainForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;  //避免线程操作控件引起的错误
            GetIecInterfaceOut();
            iecOut.GiComplted = ReadTeleTestDataForError;
            iecOut.GetTelsa = GetTelsa;
            //连接数据库
            SQLiteHelper.Connect(dbPath);

            int i;
            m_strIp_PC = "192.168.60.97";
            m_strIp_Onlly = "192.168.60.231";
            for (i = 0; i < 6; i++)
            {
                //6路电压
                m_Up[i].Mag = 50.0f;
                switch (i % 3)
                {
                    case 0:
                        m_Up[i].Ang = 0.0f;
                        break;
                    case 1:
                        m_Up[i].Ang = -120.0f;
                        break;
                    case 2:
                        m_Up[i].Ang = 120.0f;
                        break;
                }
                m_Up[i].Fre = 50.0f;
                m_Up[i].bDC = 0;
                //6路电流
                m_Ip[i].Mag = 1.0f;
                m_Ip[i].Ang = m_Up[i].Ang;
                m_Ip[i].Fre = m_Up[i].Fre;
                m_Ip[i].bDC = m_Up[i].bDC;
            }
            m_bTestBeginFlag = 0;

            //PC机IP地址
            this.textPCIP.Text = m_strIp_PC;
            //测试仪的IP地址
            this.textONLLYIP.Text = m_strIp_Onlly;

            //104连接的默认IP地址
            textBoxIP.Text = "192.168.60.100";
            textBoxPort.Text = 2404.ToString();

            HWND hWnd = this.Handle;
            HRESULT hr = linkDevice.CreateServer(hWnd);
            CheckError("OTS_CreateServer", hr);
        }

        /// <summary>
        /// iec接口初始化
        /// </summary>
        /// <returns></returns>
        public static IEC104Interface GetIecInterface()
        {
            if (iec == null)
            {
                iec = new IEC104Interface();
                return iec;
            }
            return iec;
        }

        /// <summary>
        /// iecOut接口初始化
        /// </summary>
        /// <returns></returns>
        public static IEC104InterfaceOut GetIecInterfaceOut()
        {
            if (iecOut == null)
            {
                iecOut = new IEC104InterfaceOut();
                return iecOut;
            }
            return iecOut;
        }

        /// <summary>
        /// 读取处理遥测数据
        /// </summary>
        /// <param name="meterData"></param>
        /// <returns></returns>
        private bool ReadTeleTestDataForError(List<Tuple<int, float>> meterData)
        {
            TeleMeterPointTable pointTable = new TeleMeterPointTable(16386, 16387, 16388, 16389, 16390, 16391, 16392, 16393);
            teleMeteringData.Clear();
            var table = new int[8] { pointTable.Ia, pointTable.Ib, pointTable.Ic, pointTable.I0,
                pointTable.Ua, pointTable.Ub, pointTable.Uc, pointTable.U0 };

            for (int i = 0; i < meterData.Count; i++)
            {
                //电压点号对应的点表
                if (meterData[i].Item1 == table[0] && meterData[i + 1].Item1 == table[1]
                    && meterData[i + 2].Item1 == table[2] && meterData[i + 3].Item1 == table[3]
                    && meterData[i + 4].Item1 == table[4] && meterData[i + 5].Item1 == table[5]
                    && meterData[i + 6].Item1 == table[6] && meterData[i + 7].Item1 == table[7])
                {
                    //添加电压点号
                    var subMeterVData = new List<Tuple<int, float>>();
                    subMeterVData = meterData.GetRange(i + 4, 4);
                    List<float> subVData = new List<float>();
                    foreach (var m in subMeterVData)
                    {
                        subVData.Add(m.Item2);
                    }
                    teleMeteringData.AddRange(subVData);

                    //添加电流点号
                    var subMeterIData = new List<Tuple<int, float>>();
                    subMeterIData = meterData.GetRange(i, 4);
                    List<float> subIData = new List<float>();
                    foreach (var m in subMeterIData)
                    {
                        subIData.Add(m.Item2);
                    }
                    teleMeteringData.AddRange(subIData);
                }
            }
            return true;
        }

        /// <summary>
        /// 读取处理遥信数据
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private bool GetTelsa(List<Tuple<int, bool>> m)
        {
            TeleSingalPointTable teleSingalTable = new TeleSingalPointTable(1, 2, 3, 4, 5, 6, 11);
            var table = new int[] { teleSingalTable.switchOpenStatus, teleSingalTable.switchCloseStatus, teleSingalTable.energyStorage,
            teleSingalTable.cyclone, teleSingalTable.powerFailureAlarm, teleSingalTable.batteryUnderVAlarm, teleSingalTable.protectivePlate};
            teleSingalData.Clear();
            int index = 0;
            foreach (var list in m)
            {
                if (index < table.Length)
                {
                    if (list.Item1 == table[index])
                    {
                        teleSingalData.Add(list);
                        index++;
                    }
                    else
                    {
                        continue;
                    }
                }

            }
            return true;
        }

        #region 控件随窗口大小变化

        private ArrayList InitialCrl = new ArrayList();//用以存储窗体中所有的控件名称
        private ArrayList CrlLocationX = new ArrayList();//用以存储窗体中所有的控件原始位置
        private ArrayList CrlLocationY = new ArrayList();//用以存储窗体中所有的控件原始位置
        private ArrayList CrlSizeWidth = new ArrayList();//用以存储窗体中所有的控件原始的水平尺寸
        private ArrayList CrlSizeHeight = new ArrayList();//用以存储窗体中所有的控件原始的垂直尺寸
        private int FormSizeWidth;//用以存储窗体原始的水平尺寸
        private int FormSizeHeight;//用以存储窗体原始的垂直尺寸

        private double FormSizeChangedX;//用以存储相关父窗体/容器的水平变化量
        private double FormSizeChangedY;//用以存储相关父窗体/容器的垂直变化量 

        private int Wcounter = 0;//为防止递归遍历控件时产生混乱，故专门设定一个全局计数器

        /// <summary>
        /// 获得并存储窗体中各控件的初始位置
        /// </summary>
        /// <param name="CrlContainer"></param>
        public void GetAllCrlLocation(Control CrlContainer)
        {
            foreach (Control iCrl in CrlContainer.Controls)
            {
                if (iCrl.Controls.Count > 0)
                    GetAllCrlLocation(iCrl);
                InitialCrl.Add(iCrl);
                CrlLocationX.Add(iCrl.Location.X);
                CrlLocationY.Add(iCrl.Location.Y);
            }
        }

        /// <summary>
        /// 获得并存储窗体中各控件的初始尺寸
        /// </summary>
        /// <param name="CrlContainer"></param>
        public void GetAllCrlSize(Control CrlContainer)
        {
            foreach (Control iCrl in CrlContainer.Controls)
            {
                if (iCrl.Controls.Count > 0)
                    GetAllCrlSize(iCrl);
                CrlSizeWidth.Add(iCrl.Width);
                CrlSizeHeight.Add(iCrl.Height);
            }
        }

        /// <summary>
        /// 获得并存储窗体的初始尺寸
        /// </summary>
        public void GetInitialFormSize()
        {
            FormSizeWidth = this.Size.Width;
            FormSizeHeight = this.Size.Height;
        }

        /// <summary>
        /// 窗口大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                Wcounter = 0;
                int counter = 0;
                if (this.Size.Width < FormSizeWidth || this.Size.Height < FormSizeHeight)
                //如果窗体的大小在改变过程中小于窗体尺寸的初始值，
                //则窗体中的各个控件自动重置为初始尺寸，且窗体自动添加滚动条
                {
                    foreach (Control iniCrl in InitialCrl)
                    {
                        iniCrl.Width = (int)CrlSizeWidth[counter];
                        iniCrl.Height = (int)CrlSizeHeight[counter];
                        Point point = new Point();
                        point.X = (int)CrlLocationX[counter];
                        point.Y = (int)CrlLocationY[counter];
                        iniCrl.Bounds = new Rectangle(point, iniCrl.Size);
                        counter++;
                    }
                    this.AutoScroll = true;
                }
                else
                //否则，重新设定窗体中所有控件的大小（窗体内所有控件的大小随窗体大小的变化而变化）
                {
                    this.AutoScroll = false;
                    ResetAllCrlState(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 重新设定窗体中各控件的状态（在与原状态的对比中计算而来）
        /// </summary>
        /// <param name="CrlContainer"></param>
        public void ResetAllCrlState(Control CrlContainer)
        {
            try
            {
                FormSizeChangedX = (double)this.Size.Width / (double)FormSizeWidth;
                FormSizeChangedY = (double)this.Size.Height / (double)FormSizeHeight;
                foreach (Control kCrl in CrlContainer.Controls)
                {
                    if (kCrl.Controls.Count > 0)
                    {
                        ResetAllCrlState(kCrl);
                    }
                    Point point = new Point();
                    point.X = (int)((int)CrlLocationX[Wcounter] * FormSizeChangedX);
                    point.Y = (int)((int)CrlLocationY[Wcounter] * FormSizeChangedY);
                    kCrl.Width = (int)((int)CrlSizeWidth[Wcounter] * FormSizeChangedX);
                    kCrl.Height = (int)((int)CrlSizeHeight[Wcounter] * FormSizeChangedY);
                    kCrl.Bounds = new Rectangle(point, kCrl.Size);
                    Wcounter++;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion

        #region 链路连接

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            linkDevice.DestroyServer();     //关闭与测试仪的连接
            iec.CloseConnection("");      //关闭104连接
            System.Environment.Exit(System.Environment.ExitCode);
            this.Dispose();
            this.Close();
        }

        /// <summary>
        /// 104连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            var isConnect = iec.TestAct(textBoxIP.Text, int.Parse(textBoxPort.Text));
            if (isConnect == true)
            {
                //使能设置
                buttonConnect.Enabled = false;
                buttonClose.Enabled = true;
                //底部状态栏颜色变化
                toolStripStatusLabelLink.ForeColor = System.Drawing.Color.Green;
            }
        }

        /// <summary>
        /// 断开104连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClose_Click(object sender, EventArgs e)
        {
            var isDisConnect = iec.CloseConnection("");
            if (isDisConnect == true)
            {
                //使能设置
                buttonClose.Enabled = false;
                buttonConnect.Enabled = true;
                //底部状态栏颜色变化
                toolStripStatusLabelLink.ForeColor = System.Drawing.Color.Red;
            }
        }

        /// <summary>
        /// 链路连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLinkDevice_Click(object sender, EventArgs e)
        {
            string strLink;
            strLink = "PC-IP:" + this.textPCIP.Text + "; PC-PORT:2001; ";
            strLink += "DEV-IP:" + this.textONLLYIP.Text + "; DEV-PORT:2001;";

            HRESULT hr = linkDevice.ConnectDevice(strLink);
            if (hr == S_OK)
            {
                btnLinkDevice.Enabled = false;
            }
            CheckError("OTS_LinkDevice", hr);
        }

        /// <summary>
        /// 开始实验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBeginTest_Click(object sender, EventArgs e)
        {
            if (m_bTestBeginFlag == 0)
            {
                HRESULT hr = linkDevice.ActiveTest();
                CheckError("OTS_ActiveTest", hr);
                if (hr == S_OK)
                {
                    hr = linkDevice.BeginTest(m_Up, m_Ip, m_spTestParam);
                    CheckError("OTS_BeginTest", hr);
                    if (hr == S_OK)
                        m_bTestBeginFlag = 1;
                }
                btnBeginTest.Enabled = false;
                btnStopTest.Enabled = true;
            }
        }

        /// <summary>
        /// 结束实验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStopTest_Click(object sender, EventArgs e)
        {
            if (m_bTestBeginFlag == 1)
            {
                HRESULT hr = linkDevice.StopTest();
                CheckError("OTS_StopTest", hr);
                if (hr == S_OK)
                    m_bTestBeginFlag = 0;
                btnBeginTest.Enabled = true;
                btnLinkDevice.Enabled = true;
                btnStopTest.Enabled = false;
            }
        }


        /// <summary>
        /// 退出试验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            iec.CloseConnection("");
        }

        /// <summary>
        /// 清空连接信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            this.richTextBoxLog.Clear();
        }

        /// <summary>
        /// 消息接收处理
        /// </summary>
        /// <param name="m"></param>
        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == OnllyTs.OnllyDef.WM_ONLLY_DEVMSG)
            {
                int wParam = (int)m.WParam;
                int lParam = (int)m.LParam;
                switch (wParam)
                {
                    case OnllyTs.OnllyDef.OSMSG_TestResult:
                        {
                            Trace(richTextBoxLog, "--------------------------------- OSMSG_TestResult ----------------------------------\r\n");
                            My_testResult_trace();
                        }
                        break;
                    case OnllyTs.OnllyDef.OSMSG_ioState:    //开关量状态信息上传
                        {
                            Trace(richTextBoxLog, "--------------------------------- OSMSG_ioState --------------------------------------\r\n");
                            My_ioState_trace(lParam);
                        }
                        break;
                    case OnllyTs.OnllyDef.OSMSG_GPSTime:
                        {
                            OnllySERVERMSG* pOSMsg = (OnllySERVERMSG*)lParam;
                            if (pOSMsg != null)
                            {
                                SystemTime* gpsTime = (SystemTime*)(pOSMsg->pMsg);
                                if (gpsTime->wYear == 0 || gpsTime->wMonth == 0 || gpsTime->wDay == 0)
                                {
                                    //GPS 时间信息有错误
                                }
                                else
                                {
                                    //GPS 时间信息正确, 使用 gpsTime 修正 PC 机时间
                                }
                            }
                        }
                        break;
                    case OnllyTs.OnllyDef.OSMSG_TestProcess:
                        {
                            //  试验进入第n状态 = OnllySERVERMSG.wParam 
                            OnllySERVERMSG* pOSMsg = (OnllySERVERMSG*)lParam;
                            int nTestProcess = (int)pOSMsg->wParam;

                            //如果本状态的结束方式为 GPS 的 PPM 触发, 则双方约定好时间后,
                            //提前10-50s 通知测试仪检测 PPM 
                            //下传命令，触发下一状态
                            //HRESULT hr=OTS_Execute(STSCMD_GPS_PPM,NULL,NULL);

                        }
                        break;
                    default:
                        base.DefWndProc(ref m);
                        break;
                }

            }
            else
                base.DefWndProc(ref m);
        }


        public void CheckError(string lpszText, HRESULT hr)
        {
            string strLog = lpszText;

            if (hr != S_OK)
            {
                strLog += ", 错误码：0x";
                strLog += string.Format("{0:X8}", hr);
            }
            else
            {
                strLog = lpszText;
            }
            strLog += "\n";
            Trace(richTextBoxLog, strLog);
        }

        /// <summary>
        /// 添加提示信息
        /// </summary>
        /// <param name="richText"></param>
        /// <param name="lpStr"></param>
        public void Trace(RichTextBox richText, string lpStr)
        {
            richText.AppendText(lpStr);
            richText.ScrollToCaret();
        }

        public void My_testResult_trace()
        {
            IGenericData spTestParams = new GenericData();
            OnllyTs.OTS_Win32.OTS_GetTestResult(out spTestParams);
            if (spTestParams != null)
            {
                //获取根节点
                IGenericDataNode ptrRoot;
                ptrRoot = spTestParams.GetRootNode();
                if (ptrRoot != null)
                {
                    //获取结果接点
                    IGenericDataNode ptrResult;
                    ptrResult = ptrRoot.GetChild("TestParams.Result");
                    if (ptrResult != null)
                    {
                        //解析结果
                        string strPara;
                        VARIANT var;

                        strPara = "TripFlag";
                        var = ptrResult.GetChildValue(strPara);
                        if (var is IntPtr)
                        {
                            string strTemp;
                            strTemp = Convert.ToString(var);
                            m_result.nTripFlag = Int32.Parse(strTemp);
                        }
                        else if (var is Int16)
                            m_result.nTripFlag = Convert.ToInt16(var);
                        else if (var is Int32)
                            m_result.nTripFlag = Convert.ToInt32(var);
                        else if (var is Int64)
                            m_result.nTripFlag = (int)Convert.ToInt64(var);
                        else
                            m_result.nTripFlag = 0;

                        strPara = "TripTime";
                        var = ptrResult.GetChildValue(strPara);
                        if (var is float)
                            m_result.fTripTime = Convert.ToSingle(var);
                        else
                            m_result.fTripTime = -1000.0f;

                        strPara = "Receive TestResult: TripFlag=";
                        strPara += string.Format("{0:D0}", m_result.nTripFlag);
                        strPara += ", TripTime=";
                        strPara += string.Format("{0:F3}", m_result.fTripTime);
                        strPara += "\r\n";
                        Trace(richTextBoxLog, strPara);
                    }
                }
            }
        }

        public void My_ioState_trace(LPARAM lParam)
        {
            UINT16[] BoutState = new UINT16[16];	//开出状态
            UINT16[] BinState = new UINT16[16]; 	//开入状态

            //提取 16 个开出, 16 个开入的状态
            OnllySERVERMSG* pOSMsg = (OnllySERVERMSG*)lParam;
            if (pOSMsg != null)
            {
                UINT16* p = (UINT16*)(pOSMsg->pMsg);

                if (p != null)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        BoutState[i] = *p;
                        p++;
                    }
                    for (int i = 0; i < 16; i++)
                    {
                        BinState[i] = *p;
                        p++;
                    }

                    //Trace　8个开入接点状态
                    string strText;

                    strText = "开入ABCRabcr: ";
                    for (int i = 0; i < 8; i++)
                    {
                        if (BinState[i] == 0)
                            strText += " 0";
                        else
                            strText += " 1";
                    }
                    strText += " \r\n";
                    Trace(richTextBoxLog, strText);

                    //Trace 4个开出接点状态
                    strText = "开出1234: ";
                    for (int i = 0; i < 4; i++)
                    {
                        if (BoutState[i] == 0)
                            strText += " 0";
                        else
                            strText += " 1";
                    }
                    strText += " \r\n";
                    Trace(richTextBoxLog, strText);
                }
            }
        }

        /// <summary>
        /// 系统配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSystemConfig_Click(object sender, EventArgs e)
        {
            HWND hWnd = this.Handle;
            HRESULT hr = OnllyTs.OTS_Win32.OTS_OnSystemConfig(hWnd);
            if (hr != S_OK)
            {
                string strCode;
                strCode = "Onlly 测试仪系统配置出错! 错误码：0x" + string.Format("{0:X8}", hr);
                strCode += " \r\n";
                Trace(richTextBoxLog, strCode);
            }
        }


        #endregion

        //遥信遥测数据源
        private DataTable dtBasicMeter = null;
        private DataTable dtBasicSingal = null;

        /// <summary>
        /// 加载界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //界面控件大小自适应
            GetInitialFormSize();
            GetAllCrlLocation(this);
            GetAllCrlSize(this);

            //链路连接
            //104连接断开使能设置
            buttonConnect.Enabled = true;
            buttonClose.Enabled = false;
            //开始/结束试验的使能设置
            btnBeginTest.Enabled = true;
            btnStopTest.Enabled = false;


            //载入遥测点表并设置样式
            ReloadTable("TeleMeterTable");
            //载入遥信点表并设置样式
            ReloadTable("TeleSingalTable");
            //载入遥控点表并设置样式
            ReloadTable("TeleControlTable");

            //遥信默认为普通遥信
            radioBtnOridinaryTeleSingal.Checked = true;

            //载入三遥界面数据
            ReloadTable("ThreeTeleTest");

            //设置下拉列表的默认显示
            SetComBoIndex(this);

            //底部状态栏未连接时的状态
            toolStripStatusLabelLink.ForeColor = System.Drawing.Color.Red;

            //获取遥信遥测点表数据源，并赋初始值
            dtBasicMeter = (DataTable)dataGridViewTeleMeterTable.DataSource;
            dtBasicSingal = (DataTable)dataGridViewTeleSingalTable.DataSource;
            GetBasicMeterAttrValue(dtBasicMeter);
            GetBasicSingalAttrValue(dtBasicSingal);


            var currTime = DateTime.Now;
            textBoxYear.Text = currTime.Year.ToString();
            textBoxMonth.Text = currTime.Month.ToString();
            textBoxDate.Text = currTime.Day.ToString();
            textBoxHour.Text = currTime.Hour.ToString();
            textBoxMinute.Text = currTime.Minute.ToString();
            textBoxSecond.Text = currTime.Second.ToString();
            textBoxMillSec.Text = currTime.Millisecond.ToString();
            textBoxDayOfWeek.Text = currTime.DayOfWeek.ToString();
        }

        /// <summary>
        /// 设置下拉列表默认显示第一个
        /// </summary>
        /// <param name="crl"></param>
        private void SetComBoIndex(Control crl)
        {
            foreach (Control iCrl in crl.Controls)
            {
                if (iCrl.Controls.Count > 0)
                    SetComBoIndex(iCrl);
                if (iCrl is ComboBox)
                {
                    if (iCrl.Name.Equals("ConBoRatedVPerOutput") || iCrl.Name.Equals("ConBoRatedIPerOutput"))
                    {
                        continue;
                    }
                    ((ComboBox)iCrl).SelectedIndex = 0;
                }
            }
        }

        #region 遥信测试

        System.Timers.Timer LoopTime;        //定时器
        private static int totalTime = 0;    //遥信风暴执行的总时间

        /// <summary>
        /// 按钮使能设置：遥信风暴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadBtnTelesignallingStorm_CheckedChanged(object sender, EventArgs e)
        {
            if (radBtnTelesignallingStorm.Checked)
            {
                //选中的单选框内容可用
                panelTeleSingallingStorm.Enabled = true;
                btnTeleSingalTest.Enabled = true;  //启动测试的使能
                //其他的按钮不可用并且内容不可操作
                radioBtnTeleSingalResolution.Checked = false;
                panelTeleSingalResolution.Enabled = false;
                radioBtnOridinaryTeleSingal.Checked = false;
                panelOridinaryTeleSingal.Enabled = false;
            }
        }

        /// <summary>
        /// 按钮使能设置：遥信分辨率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioBtnTeleSingalResolution_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnTeleSingalResolution.Checked)
            {
                //选中的单选框内容可用
                panelTeleSingalResolution.Enabled = true;
                btnTeleSingalTest.Enabled = true;  //启动测试的使能
                //其他的按钮不可用并且内容不可操作
                radBtnTelesignallingStorm.Checked = false;
                panelTeleSingallingStorm.Enabled = false;
                radioBtnOridinaryTeleSingal.Checked = false;
                panelOridinaryTeleSingal.Enabled = false;

            }
        }

        /// <summary>
        /// 按钮使能设置：普通遥信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioBtnOridinaryTeleSingal_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnOridinaryTeleSingal.Checked)
            {
                //选中的单选框内容可用
                panelOridinaryTeleSingal.Enabled = true;
                //其他的按钮不可用并且内容不可操作
                radBtnTelesignallingStorm.Checked = false;
                panelTeleSingallingStorm.Enabled = false;
                radioBtnTeleSingalResolution.Checked = false;
                panelTeleSingalResolution.Enabled = false;
                btnTeleSingalTest.Enabled = false;  //启动测试的使能
            }
        }

        /// <summary>
        /// 普通遥信中按钮的使能：选择单个开出量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioBtnSingalOpen_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnSingalOpen.Checked)
            {
                comBoSingalOpen.Enabled = true;
                comBoManyOpenStart.Enabled = false;
                comBoManyOpenEnd.Enabled = false;
            }
        }

        /// <summary>
        ///  普通遥信中按钮的使能：选择多个开出量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadionBtnManyOpen_CheckedChanged(object sender, EventArgs e)
        {
            if (radionBtnManyOpen.Checked)
            {
                comBoSingalOpen.Enabled = false;
                comBoManyOpenStart.Enabled = true;
                comBoManyOpenEnd.Enabled = true;
            }
        }

        /// <summary>
        /// 在规定事件内执行函数，超时则强制停止
        /// </summary>
        /// <param name="action"></param>
        /// <param name="timeoutMilliseconds"></param>
        private void CallWithTimeout(Action action, int timeoutMilliseconds)
        {
            try
            {
                Thread threadToKill = null;
                Action wrappedAction = () =>
                {
                    threadToKill = Thread.CurrentThread;
                    action();
                };
                IAsyncResult result = wrappedAction.BeginInvoke(null, null);
                if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
                {
                    wrappedAction.EndInvoke(result);
                }
                else
                {
                    threadToKill.Abort();
                    throw new TimeoutException();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 执行n次的遥信风暴
        /// </summary>
        private void TestTeleSingalStorm()
        {
            try
            {
                int count = int.Parse(numUpDownCount.Value.ToString());   //获取执行次数
                for (int i = 0; i < count; i++)
                {
                    UINT16[] bout = new UINT16[4];
                    if (radBtnTelesignallingStorm.Checked)
                    {
                        int startIndex = int.Parse(comBoStartRange.Text.Substring(2));
                        int endIndex = int.Parse(comBoEndRange.Text.Substring(2));
                        var index = startIndex;
                        if (startIndex > endIndex)
                        {
                            MessageBox.Show("请选择正确的开出范围");
                            return;
                        }
                        for (int j = 0; j < 4; j++)
                        {
                            if (index < endIndex || index == endIndex)
                            {
                                if ((j + 1) == index)
                                {
                                    bout[j] = 1;
                                    index++;
                                }
                                else
                                {
                                    bout[j] = 0;
                                }
                            }
                            else
                            {
                                bout[j] = 0;
                            }
                        }
                    }
                    HRESULT hr = OnllyTs.OTS_Win32.OTS_TransBoutState(bout, bout.Length);
                    CheckError("OTS_TransBoutState", hr);
                    iec.GiAct("");      //获取遥信
                    iecOut.GetTelsa = GetTelsa;    //处理遥信数据
                    Delay(5000);
                    //11是保护压板
                    foreach (var data in teleSingalData)
                    {
                        if (data.Item1 == 11)
                        {
                            var info = System.DateTime.Now.ToLongTimeString() + "  当前状态：" + data.Item2.ToString() + "   ";
                            Trace(richTextBoxTeleSingalInfo, info);
                            if (data.Item2 == true)
                            {
                                Trace(richTextBoxTeleSingalInfo, "状态符合要求." + "\n");
                            }
                            else
                            {
                                Trace(richTextBoxTeleSingalInfo, "状态不符合要求." + "\n");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 打开定时器
        /// </summary>
        /// <param name="time"></param>
        private void ReStartTimer(int time)
        {
            LoopTime = new System.Timers.Timer(time);
            totalTime += time;
            LoopTime.Elapsed += LoopTestAction_Elapsed;
            LoopTime.AutoReset = false;
            LoopTime.Start();
        }

        /// <summary>
        /// 关闭定时器
        /// </summary>
        private void CloseLoopTime()
        {
            if (LoopTime != null)
            {
                LoopTime.Enabled = false;
                LoopTime.Elapsed -= LoopTestAction_Elapsed;
                LoopTime.Stop();
                LoopTime.Close();
                LoopTime.Dispose();
            }
        }

        /// <summary>
        /// 执行遥信分辨率操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoopTestAction_Elapsed(object sender, ElapsedEventArgs e)
        {
            //获取遥信脉宽
            int pulseWidth = int.Parse(textBoxTelesingalResolution.Text);
            //获取分辨率
            int resolutionRatio = int.Parse(textBoxResolution.Text);
            //如果执行时间超过遥信脉宽，则停止计时器
            if (totalTime > pulseWidth)
            {
                CloseLoopTime();
                return;
            }
            UINT16[] bout = new UINT16[4];
            //获取开出范围
            int startIndex = int.Parse(comBoResolutionStartRange.Text.Substring(2));
            int endIndex = int.Parse(comBoResolutonEndRange.Text.Substring(2));
            var index = startIndex;
            if (startIndex > endIndex)
            {
                MessageBox.Show("请选择正确的开出范围");
                return;
            }
            for (int j = 0; j < 4; j++)
            {
                if (index < endIndex || index == endIndex)
                {
                    if ((j + 1) == index)
                    {
                        bout[j] = 1;
                        index++;
                    }
                    else
                    {
                        bout[j] = 0;
                    }
                }
                else
                {
                    bout[j] = 0;
                }
            }

            HRESULT hr = OnllyTs.OTS_Win32.OTS_TransBoutState(bout, bout.Length);
            CheckError("OTS_TransBoutState", hr);
            iec.GiAct("");       //获取遥信
            iecOut.GetTelsa = GetTelsa;      //处理遥信数据
            Delay(5000);
            totalTime += 5000;    //将延迟时间加入总时间
            //11是保护压板
            foreach (var data in teleSingalData)
            {
                if (data.Item1 == 11)
                {
                    var info = System.DateTime.Now.ToLongTimeString() + "  当前状态：" + data.Item2.ToString() + "   ";
                    Trace(richTextBoxTeleSingalInfo, info);
                    if (data.Item2 == true)
                    {
                        Trace(richTextBoxTeleSingalInfo, "状态符合要求." + "\n");
                    }
                    else
                    {
                        Trace(richTextBoxTeleSingalInfo, "状态不符合要求." + "\n");
                    }
                }
            }
            ReStartTimer(resolutionRatio);
        }

        /// <summary>
        /// 启动测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTeleSingalTest_Click(object sender, EventArgs e)
        {
            var time = int.Parse(textBoxTelesingalPulseWidth.Text.ToString());
            //遥信风暴
            if (radBtnTelesignallingStorm.Checked)
            {
                CallWithTimeout(TestTeleSingalStorm, time);
            }
            //遥信分辨率
            if (radioBtnTeleSingalResolution.Checked)
            {
                int resolutionRatio = int.Parse(textBoxResolution.Text);
                ReStartTimer(resolutionRatio);
            }
        }


        /// <summary>
        /// 普通遥信开出分合
        /// </summary>
        /// <param name="arg1">分合闸参数</param>
        /// <param name="arg2">分合闸参数</param>
        /// <returns></returns>
        private UINT16[] Output(UINT16 arg1, UINT16 arg2)
        {
            UINT16[] bout = new UINT16[4];
            //单个开出量
            if (radioBtnSingalOpen.Checked)
            {
                int textValue = int.Parse(comBoSingalOpen.Text.Substring(2));
                for (int i = 0; i < 4; i++)
                {
                    if ((i + 1) == textValue)
                    {
                        bout[i] = arg1;
                    }
                    else
                    {
                        bout[i] = arg2;
                    }
                }
            }
            //多个开出量
            else if (radionBtnManyOpen.Checked)
            {
                int startIndex = int.Parse(comBoManyOpenStart.Text.Substring(2));
                int endIndex = int.Parse(comBoManyOpenEnd.Text.Substring(2));
                var index = startIndex;
                if (startIndex > endIndex)
                {
                    MessageBox.Show("请选择正确的开出范围");
                    //  return;
                }
                for (int i = 0; i < 4; i++)
                {
                    if (index < endIndex || index == endIndex)
                    {
                        if ((i + 1) == index)
                        {
                            bout[i] = arg1;
                            index++;
                        }
                        else
                        {
                            bout[i] = arg2;
                        }
                    }
                    else
                    {
                        bout[i] = arg2;
                    }
                }
            }
            return bout;
        }

        /// <summary>
        /// 遥信测试中普通遥信开出合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenAndClose_Click(object sender, EventArgs e)
        {
            //测试前获取上一次的遥信数据
            var firstData = new List<Tuple<int, bool>>();
            iec.GiAct("");
            iecOut.GetTelsa = GetTelsa;
            Delay(5000);
            firstData.AddRange(teleSingalData);
            //开出量
            var bout = Output(1, 0);
            HRESULT hr = OnllyTs.OTS_Win32.OTS_TransBoutState(bout, bout.Length);
            CheckError("OTS_TransBoutState", hr);
            Trace(richTextBoxTeleSingalInfo, System.DateTime.Now.ToLongTimeString() + "   操作：开出合" + "\n");
            Delay(5000);
            //测试后获取此次的遥信数据
            var twiceData = new List<Tuple<int, bool>>();
            iec.GiAct("");
            iecOut.GetTelsa = GetTelsa;
            Delay(5000);
            twiceData.AddRange(teleSingalData);
            ShowSingalInfo(firstData, twiceData);   //显示测试结果
        }

        /// <summary>
        /// 遥信测试中普通遥信开出分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenAndOpen_Click(object sender, EventArgs e)
        {
            //测试前获取上一次的遥信数据
            var firstData = new List<Tuple<int, bool>>();
            iec.GiAct("");
            iecOut.GetTelsa = GetTelsa;
            Delay(5000);
            firstData.AddRange(teleSingalData);
            //开出量
            var bout = Output(0, 1);
            HRESULT hr = OnllyTs.OTS_Win32.OTS_TransBoutState(bout, bout.Length);
            CheckError("OTS_TransBoutState", hr);
            Trace(richTextBoxTeleSingalInfo, System.DateTime.Now.ToLongTimeString() + "   操作：开出分" + "\n");
            Delay(5000);
            //测试后获取此次的遥信数据
            var twiceData = new List<Tuple<int, bool>>();
            iec.GiAct("");
            iecOut.GetTelsa = GetTelsa;
            Delay(5000);
            twiceData.AddRange(teleSingalData);
            ShowSingalInfo(firstData, twiceData);    //显示测试结果
        }

        /// <summary>
        /// 显示遥信信息
        /// </summary>
        /// <param name="firstData"></param>
        /// <param name="twiceData"></param>
        private void ShowSingalInfo(List<Tuple<int, bool>> firstData, List<Tuple<int, bool>> twiceData)
        {
            foreach (var data in firstData)
            {
                switch (data.Item1)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 11:
                        bool value = false;
                        foreach (var tData in twiceData)
                        {
                            if (tData.Item1 == 11)
                            {
                                value = tData.Item2;
                            }
                        }
                        var info = System.DateTime.Now.ToLongTimeString() + "  保护压板:" + data.Item2 + " -> " + value + "\n";
                        Trace(richTextBoxTeleSingalInfo, info);
                        break;
                }
            }
        }

        /// <summary>
        /// 遥信测试界面：清空遥信信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemTeleSingal_Click(object sender, EventArgs e)
        {
            this.richTextBoxTeleSingalInfo.Clear();
        }

        #endregion

        #region 三遥测试

        private bool testFlag = false;   //测试状态的标志

        /// <summary>
        /// 步长输入值限定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxStepSize_Leave(object sender, EventArgs e)
        {
            //赋值的最小步长是0.01
            if (comBoAdjustAttr.Text.Equals("幅值"))
            {
                if (textBoxStepSize.Text != "")
                {
                    if (float.Parse(textBoxStepSize.Text) < 0.01f)
                    {
                        MessageBox.Show("幅值最小步长位0.01V");
                        textBoxStepSize.Text = 0.01.ToString();
                    }
                }
                else return;
            }
            //相位的最小步长是0.1
            else if (comBoAdjustAttr.Text.Equals("相位"))
            {
                if (textBoxStepSize.Text != " ")
                {
                    if (float.Parse(textBoxStepSize.Text) < 0.1f)
                    {
                        MessageBox.Show("相位最小步长位0.1");
                        textBoxStepSize.Text = 0.1.ToString();
                    }
                    if (float.Parse(textBoxStepSize.Text) > 360
                        || float.Parse(textBoxStepSize.Text) < -360)
                    {
                        MessageBox.Show("相位范围必须在-360~360");
                        textBoxStepSize.Text = 360.ToString();
                    }
                }
                else return;
            }
            //频率的最小步长是0.01
            else if (comBoAdjustAttr.Text.Equals("频率"))
            {
                if (textBoxStepSize.Text != " ")
                {
                    if (float.Parse(textBoxStepSize.Text) < 0.01f)
                    {
                        MessageBox.Show("频率最小步长位0.01HZ");
                        textBoxStepSize.Text = 0.01.ToString();
                    }
                }
                else return;
            }
        }

        /// <summary>
        /// 在表格中定位上调下调数据位置
        /// </summary>
        /// <param name="columnIndex">数据所在行</param>
        /// <param name="rowIndex">数据所在列</param>
        /// <returns></returns>
        private float Positioning(ref int columnIndex, ref int rowIndex)
        {
            bool flag = false;
            float value = 0.0f;
            var selectedAttr = comBoAdjustAttr.Text;
            var selectedPassage = comBoPassageCollection.Text;
            for (columnIndex = 0; columnIndex < dataGridViewPassageData.Columns.Count; columnIndex++)
            {
                if (dataGridViewPassageData.Columns[columnIndex].HeaderText.Equals(selectedAttr))
                {
                    for (rowIndex = 0; rowIndex < dataGridViewPassageData.Rows.Count; rowIndex++)
                    {
                        if (dataGridViewPassageData.Rows[rowIndex].Cells[0].Value.ToString().Equals(selectedPassage))
                        {
                            value = float.Parse(dataGridViewPassageData.Rows[rowIndex].Cells[columnIndex].Value.ToString());
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag == true)
                {
                    break;
                }
            }
            return value;
        }

        /// <summary>
        /// 上调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonUp_Click(object sender, EventArgs e)
        {
            try
            {
                float stepLength = float.Parse(textBoxStepSize.Text);
                var selectedAttr = comBoAdjustAttr.Text;
                var selectedPassage = comBoPassageCollection.Text;
                int i = 0;
                int j = 0;
                var value = Positioning(ref i, ref j);
                dataGridViewPassageData.Rows[j].Cells[i].Value = (float)(value + stepLength);
                if (testFlag == true)
                {
                    ThreeTeleTest();
                    HRESULT hr = linkDevice.SwitchTest(m_Up, m_Ip, m_spTestParam);
                    CheckError("OTS_SwitchTestPoint:" + selectedPassage + "的" + selectedAttr + "上调" + stepLength.ToString(), hr);
                    GetTeleMeterData();
                    ShowError();
                    dataGridViewError.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 下调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMinus_Click(object sender, EventArgs e)
        {
            try
            {
                float stepLength = float.Parse(textBoxStepSize.Text);
                var selectedAttr = comBoAdjustAttr.Text;
                var selectedPassage = comBoPassageCollection.Text;
                int i = 0;
                int j = 0;
                var value = Positioning(ref i, ref j);
                dataGridViewPassageData.Rows[j].Cells[i].Value = (float)(value - stepLength);
                if (testFlag == true)
                {
                    ThreeTeleTest();
                    HRESULT hr = linkDevice.SwitchTest(m_Up, m_Ip, m_spTestParam);
                    CheckError("OTS_SwitchTestPoint:" + selectedPassage + "的" + selectedAttr + "下调" + stepLength.ToString(), hr);
                    GetTeleMeterData();
                    ShowError();
                    dataGridViewError.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 电流电压相角范围限定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewPassageData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                for (int rowsIndex = 0; rowsIndex < dataGridViewPassageData.Rows.Count; rowsIndex++)
                {
                    if (dataGridViewPassageData.Rows[rowsIndex].Cells[0].Value.ToString().Contains("U"))
                    {
                        var value = float.Parse(dataGridViewPassageData.Rows[rowsIndex].Cells[1].Value.ToString());
                        if (value > 450 || value < 0)
                        {
                            MessageBox.Show("请确保电压在0~450V内");
                            dataGridViewPassageData.Rows[rowsIndex].Cells[1].Value = 50;
                        }
                    }
                    if (dataGridViewPassageData.Rows[rowsIndex].Cells[0].Value.ToString().Contains("I"))
                    {
                        var value = float.Parse(dataGridViewPassageData.Rows[rowsIndex].Cells[1].Value.ToString());
                        if (value > 50 || value < 0)
                        {
                            MessageBox.Show("请确保电流在0~50V内");
                            dataGridViewPassageData.Rows[rowsIndex].Cells[1].Value = 1;
                        }
                    }
                }
                //因为允许用户修改表格，所以就多出一行空行，如果不减1就会出现未将对象引用到实例
                for (int rowIndex = 0; rowIndex < dataGridViewPassageData.Rows.Count; rowIndex++)
                {
                    float angle = float.Parse(dataGridViewPassageData.Rows[rowIndex].Cells[2].Value.ToString());
                    if (angle < -360 || angle > 360)
                    {
                        MessageBox.Show("相角范围必须在-360~360之间");
                        dataGridViewPassageData.Rows[rowIndex].Cells[2].Value = 120;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 电压相等
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUEqual_Click(object sender, EventArgs e)
        {
            try
            {
                float valueUa = float.Parse(dataGridViewPassageData.Rows[0].Cells[1].Value.ToString());
                for (int rowsIndex = 0; rowsIndex < dataGridViewPassageData.Rows.Count; rowsIndex++)
                {
                    if (dataGridViewPassageData.Rows[rowsIndex].Cells[0].Value.ToString().Contains("U"))
                    {
                        dataGridViewPassageData.Rows[rowsIndex].Cells[1].Value = valueUa;
                    }
                }
                //虽然不加也可以变化
                dataGridViewPassageData.Refresh();
                if (testFlag == true)
                {
                    ThreeTeleTest();
                    HRESULT hr = linkDevice.SwitchTest(m_Up, m_Ip, m_spTestParam);
                    CheckError("OTS_SwitchTestPoint:电压相等", hr);
                    GetTeleMeterData();
                    ShowError();
                    dataGridViewError.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 电流相等
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnIEqual_Click(object sender, EventArgs e)
        {
            try
            {
                float valueIa = float.Parse(dataGridViewPassageData.Rows[4].Cells[1].Value.ToString());
                for (int rowsIndex = 0; rowsIndex < dataGridViewPassageData.Rows.Count; rowsIndex++)
                {
                    if (dataGridViewPassageData.Rows[rowsIndex].Cells[0].Value.ToString().Contains("I"))
                    {
                        dataGridViewPassageData.Rows[rowsIndex].Cells[1].Value = valueIa;
                    }
                }
                //虽然不加也可以变化
                dataGridViewPassageData.Refresh();
                if (testFlag == true)
                {
                    ThreeTeleTest();
                    HRESULT hr = linkDevice.SwitchTest(m_Up, m_Ip, m_spTestParam);
                    CheckError("OTS_SwitchTestPoint:电流相等", hr);
                    GetTeleMeterData();
                    ShowError();
                    dataGridViewError.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 正序平衡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPositiveOrderBalance_Click(object sender, EventArgs e)
        {
            int[] ang = new int[] { 240, 120, 0 };
            //电压正序
            for (int i = 0; i < 3; i++)
            {
                dataGridViewPassageData.Rows[i].Cells[2].Value = ang[i];
            }
            //电流正序
            for (int i = 4; i < 7; i++)
            {
                dataGridViewPassageData.Rows[i].Cells[2].Value = ang[i - 4];
            }
            if (testFlag == true)
            {
                ThreeTeleTest();
                HRESULT hr = linkDevice.SwitchTest(m_Up, m_Ip, m_spTestParam);
                CheckError("OTS_SwitchTestPoint:正序平衡", hr);
            }
        }

        /// <summary>
        /// 逆序平衡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReverseOrderBalance_Click(object sender, EventArgs e)
        {
            int[] ang = new int[] { 0, 120, 240 };
            //电压逆序
            for (int i = 0; i < 3; i++)
            {
                dataGridViewPassageData.Rows[i].Cells[2].Value = ang[i];
            }
            //电流逆序
            for (int i = 4; i < 7; i++)
            {
                dataGridViewPassageData.Rows[i].Cells[2].Value = ang[i - 4];
            }
            if (testFlag == true)
            {
                ThreeTeleTest();
                HRESULT hr = linkDevice.SwitchTest(m_Up, m_Ip, m_spTestParam);
                CheckError("OTS_SwitchTestPoint:逆序平衡", hr);
            }
        }

        /// <summary>
        /// 额定电压输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConBoRatedVPerOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            var perVol = float.Parse(ConBoRatedVPerOutput.Text.Replace("%", "")) * 0.01;
            for (int i = 0; i < 4; i++)
            {
                dataGridViewPassageData.Rows[i].Cells[1].Value = ratedVoltage * perVol;
            }
            if (testFlag == true)
            {
                ThreeTeleTest();
                HRESULT hr = linkDevice.SwitchTest(m_Up, m_Ip, m_spTestParam);
                CheckError("OTS_SwitchTestPoint:额定电压输出", hr);
                GetTeleMeterData();
                ShowError();
                dataGridViewError.Refresh();
            }
        }

        /// <summary>
        /// 额定电流输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConBoRatedIPerOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            //额定电流
            var perCur = float.Parse(ConBoRatedIPerOutput.Text.Replace("%", "")) * 0.01;
            for (int i = 4; i < 8; i++)
            {
                dataGridViewPassageData.Rows[i].Cells[1].Value = perCur * ratedCurrent;
            }
            if (testFlag == true)
            {
                ThreeTeleTest();
                HRESULT hr = linkDevice.SwitchTest(m_Up, m_Ip, m_spTestParam);
                CheckError("OTS_SwitchTestPoint:额定电流输出", hr);
                GetTeleMeterData();
                ShowError();
                dataGridViewError.Refresh();
            }
        }

        /// <summary>
        /// 遥控指令：取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancle_Click(object sender, EventArgs e)
        {
            iec.TeleCancel("");
        }

        /// <summary>
        /// 遥测测试赋值
        /// </summary>
        private void ThreeTeleTest()
        {
            List<float> magList = new List<float>();
            List<float> angList = new List<float>();
            List<float> freList = new List<float>();
            for (int rowIndex = 0; rowIndex < dataGridViewPassageData.Rows.Count; rowIndex++)
            {
                magList.Add(float.Parse(dataGridViewPassageData.Rows[rowIndex].Cells[1].Value.ToString()));
                angList.Add(float.Parse(dataGridViewPassageData.Rows[rowIndex].Cells[2].Value.ToString()));
                freList.Add(float.Parse(dataGridViewPassageData.Rows[rowIndex].Cells[3].Value.ToString()));
            }

            for (int i = 0, j = 0, z = 0; i < 4 && j < 4
                && z < 4; i++, j++, z++)
            {
                m_Up[i].Mag = magList[i];
                m_Up[i].Ang = angList[j];
                m_Up[i].Fre = freList[z];
            }
            for (int i = 4, j = 4, z = 4; i < 8 && j < 8
                && z < 8; i++, j++, z++)
            {
                m_Ip[i - 4].Mag = magList[i];
                m_Ip[i - 4].Ang = angList[j];
                m_Ip[i - 4].Fre = freList[z];
            }
        }

        /// <summary>
        /// 获取遥测数据
        /// </summary>
        private void GetTeleMeterData()
        {
            iec.GiAct("");
            iecOut.GiComplted = ReadTeleTestDataForError;
        }

        /// <summary>
        /// 启动遥测测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTestTeleSingal_Click(object sender, EventArgs e)
        {
            //获取标准误差
            var standardError = float.Parse(textBoxStandardError.Text);
            //启动测试
            testFlag = true;
            ThreeTeleTest();
            HRESULT hr = linkDevice.SwitchTest(m_Up, m_Ip, m_spTestParam);
            CheckError("OTS_SwitchTestPoint:遥测测试", hr);

            //获取遥测数据
            GetTeleMeterData();
            Delay(5000);

            //如果已经有数据误差，显示误差
            if (dataGridViewError.Rows.Count == 0)
            {
                //显示测试结果的数据
                for (int j = 0; j < dataGridViewPassageData.Rows.Count; j++)
                {
                    int index = this.dataGridViewError.Rows.Add();
                    dataGridViewError.Rows[index].Cells[0].Value = dataGridViewPassageData.Rows[j].Cells[0].Value;
                    float value = float.Parse(dataGridViewPassageData.Rows[j].Cells[1].Value.ToString());
                    dataGridViewError.Rows[index].Cells[1].Value = value;
                    dataGridViewError.Rows[index].Cells[2].Value = teleMeteringData[j];
                    dataGridViewError.Rows[index].Cells[3].Value = teleMeteringData[j] - value;
                }
                //显示误差列的数据颜色
                ChangeColor();
                dataGridViewError.Refresh();
            }
            else
            {
                ShowError();
            }
        }

        /// <summary>
        /// 误差颜色随标准误差的变化而变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxStandardError_TextChanged(object sender, EventArgs e)
        {
            if (dataGridViewError != null)
            {
                ChangeColor();
            }
            dataGridViewError.Refresh();
        }

        /// <summary>
        /// 遥测：显示误差
        /// </summary>
        private void ShowError()
        {
            Delay(5000);
            for (int i = 0; i < dataGridViewError.Rows.Count; i++)
            {
                float value = float.Parse(dataGridViewPassageData.Rows[i].Cells[1].Value.ToString());
                dataGridViewError.Rows[i].Cells[1].Value = value;
                dataGridViewError.Rows[i].Cells[2].Value = teleMeteringData[i];
                dataGridViewError.Rows[i].Cells[3].Value = teleMeteringData[i] - value;
            }
            //改变误差列的颜色
            ChangeColor();
            dataGridViewError.Refresh();
        }

        /// <summary>
        /// 更新颜色
        /// </summary>
        private void ChangeColor()
        {
            try
            {
                //显示误差列的数据颜色
                for (int index = 0; index < dataGridViewError.Rows.Count; index++)
                {
                    //确保当文本框中什么都没有时不会出现错误
                    if (textBoxStandardError.Text != "")
                    {
                        if (Math.Abs((float)(dataGridViewError.Rows[index].Cells[3].Value)) > float.Parse(textBoxStandardError.Text))
                        {
                            dataGridViewError.Rows[index].Cells[3].Style.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            dataGridViewError.Rows[index].Cells[3].Style.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 三遥测试：清空遥控信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemClearTeleContorol_Click(object sender, EventArgs e)
        {
            this.richTextBoxControllInfo.Clear();
        }

        /// <summary>
        /// 遥控自动化测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTestTeleControl_Click(object sender, EventArgs e)
        {
            List<int> idCollect = new List<int>();
            //判断选择的遥控操作是否合法
            if (dataGridViewTeleControlTable.Rows.Count > 0)
            {
                for (int rowIndex = 0; rowIndex < dataGridViewTeleControlTable.Rows.Count; rowIndex++)
                {
                    string _selectValue = dataGridViewTeleControlTable.Rows[rowIndex].Cells[0].EditedFormattedValue.ToString();
                    if (_selectValue == "True")
                    {
                        idCollect.Add(int.Parse(dataGridViewTeleControlTable.Rows[rowIndex].Cells[3].Value.ToString()));
                    }
                }
            }
            if (idCollect.Count > 1)
            {
                MessageBox.Show("只能选择一个遥控操作");
                return;
            }
            //赋值
            int ID = 0;
            if (idCollect.Count != 0)
            {
                ID = idCollect[0];
            }
            else
            {
                MessageBox.Show("请选择遥控操作");
                return;
            }

            var singelOrDouble = comboBoxSingleOrDouble.Text;
            byte TI = 45;
            if (singelOrDouble.Equals("单点"))
            {
                TI = 45;
            }
            else if (singelOrDouble.Equals("双点"))
            {
                TI = 46;
            }
            var openOrCloseString = comBoCloseOrOpen.Text;
            int openOrClose = 1;
            if (openOrCloseString.Equals("合闸"))
            {
                openOrClose = 1;
            }
            else if (openOrCloseString.Equals("分闸"))
            {
                openOrClose = 0;
            }
            iec.ValueToParameter(ID, TI, openOrClose);

            //选择
            iec.TeleSelect("");
            //执行
            bool isSuccess = iec.TeleAction("");
            Delay(5000);
            //处理遥信数据，显示遥控信息
            string info = "";
            bool switchClose = false;    //开关合位
            bool switchOpen = false;    //开关分位
            iec.GiAct("");
            iecOut.GetTelsa = GetTelsa;
            Delay(5000);
            foreach (var teleSingal in teleSingalData)
            {
                switch (teleSingal.Item1)
                {
                    case 1:
                        info = System.DateTime.Now.ToLongTimeString() + "  开关分位:" + teleSingal.Item2 + "\n";
                        switchOpen = teleSingal.Item2;
                        Trace(richTextBoxControllInfo, info);
                        break;
                    case 2:
                        info = System.DateTime.Now.ToLongTimeString() + "  开关合位:" + teleSingal.Item2 + "\n";
                        switchClose = teleSingal.Item2;
                        Trace(richTextBoxControllInfo, info);
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 11:
                        break;
                    default:
                        break;
                }
            }
            //合闸
            if (openOrClose == 1)
            {
                if (switchClose == true && switchOpen == false)
                {
                    Trace(richTextBoxControllInfo, "开关状态符合." + "\n");
                }
                else
                {
                    Trace(richTextBoxControllInfo, "开关状态不符合." + "\n");
                }
            }
            //分闸
            else if (openOrClose == 0)
            {
                if (switchOpen == true && switchClose == false)
                {
                    Trace(richTextBoxControllInfo, "开关状态符合." + "\n");
                }
                else
                {
                    Trace(richTextBoxControllInfo, "开关状态不符合." + "\n");
                }
            }
        }

        #endregion

        #region 遥测遥控遥信点表配置

        private static int meterTableCount = 1;    //遥测点表的个数
        private static int singalTableCount = 1;    //遥信点表的个数

        private List<int> teleMeterIndex = new List<int>(16);   //存储遥测表格的序列号
        private List<string> teleMeterName = new List<string>(16);   //存储遥测表格的名字
        private List<string> teleMeterUnit = new List<string>(16);     //存储遥测表格的单位
        private List<string> teleMeterComment = new List<string>(16);    //存储遥测表格的说明

        private List<int> teleSingalIndex = new List<int>(9);    //存储遥信表格的序列号
        private List<string> teleSingalName = new List<string>(9);   //存储遥信表格的名字
        private List<string> teleSingalComment = new List<string>(9);   //存储遥信表格的说明

        /// <summary>
        /// 设置表格的样式
        /// </summary>
        /// <param name="datagridView"></param>
        private void SetDataGridViewStyle(DataGridView datagridView)
        {
            //设置每行颜色
            datagridView.RowsDefaultCellStyle.BackColor = Color.AliceBlue;
            //不可以对表格排序
            for (int columnIndex = 0; columnIndex < datagridView.Columns.Count; columnIndex++)
            {
                datagridView.Columns[columnIndex].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //表头设置颜色
            datagridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Bisque;
            //设置表头字体
            datagridView.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", 12, FontStyle.Bold);
            //设置字体颜色
            datagridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Purple;
        }

        /// <summary>
        /// 载入点表
        /// </summary>
        /// <param name="tableName">点表名称</param>
        private void ReloadTable(string tableName)
        {
            try
            {
                switch (tableName)
                {
                    case "TeleMeterTable":
                        {
                            //遥测点表样式设置
                            //加载点表表格
                            DataTable dtMeter = SQLiteHelper.ReadTable("TeleMeterTable");
                            dataGridViewTeleMeterTable.DataSource = dtMeter;
                            SQLiteHelper.Close();
                            SetDataGridViewStyle(dataGridViewTeleMeterTable);
                            //设置表头
                            var headerText = new string[6] { "序号", "名称", "点号", "值", "单位", "说明" };
                            var databaseName = new string[6] { "Index", "Name", "PointNumber", "Value", "Unit", "Comment" };
                            for (int index = 0; index < headerText.Length; index++)
                            {
                                var currentName = databaseName[index];
                                dataGridViewTeleMeterTable.Columns[currentName].HeaderText = headerText[index];
                                if (currentName.Equals("Comment"))
                                {
                                    dataGridViewTeleMeterTable.Columns[currentName].Width = 790;
                                }
                            }
                            //某些列不可改
                            for (int columnIndex = 0; columnIndex < dataGridViewTeleMeterTable.Columns.Count; columnIndex++)
                            {
                                if (columnIndex == 0 || columnIndex == 1 || columnIndex == 4)
                                {
                                    dataGridViewTeleMeterTable.Columns[columnIndex].ReadOnly = true;
                                }
                            }
                            break;
                        }
                    case "TeleSingalTable":
                        {
                            //遥信点表样式设置
                            //加载点表表格
                            DataTable dtSingal = SQLiteHelper.ReadTable("TeleSingalTable");
                            dataGridViewTeleSingalTable.DataSource = dtSingal;
                            SQLiteHelper.Close();
                            SetDataGridViewStyle(dataGridViewTeleSingalTable);
                            //设置表头
                            var singalHeaderText = new string[5] { "序号", "名称", "点号", "值", "说明" };
                            var singalDatabaseName = new string[5] { "Index", "Name", "PointNumber", "Value", "Comment" };
                            for (int index = 0; index < singalHeaderText.Length; index++)
                            {
                                var currentName = singalDatabaseName[index];
                                dataGridViewTeleSingalTable.Columns[currentName].HeaderText = singalHeaderText[index];
                                if (currentName.Equals("Comment"))
                                {
                                    dataGridViewTeleSingalTable.Columns[currentName].Width = 790;
                                }
                            }
                            //某些列不可改
                            for (int columnIndex = 0; columnIndex < dataGridViewTeleSingalTable.Columns.Count; columnIndex++)
                            {
                                if (columnIndex == 0 || columnIndex == 1)
                                {
                                    dataGridViewTeleSingalTable.Columns[columnIndex].ReadOnly = true;
                                }
                            }
                            break;
                        }
                    case "TeleControlTable":
                        {
                            //遥控点表样式设置
                            //加载点表表格
                            DataTable dtControl = SQLiteHelper.ReadTable("TeleControlTable");
                            dataGridViewTeleControlTable.DataSource = dtControl;
                            SQLiteHelper.Close();
                            //添加一列CheckBox选择
                            if (!dataGridViewTeleControlTable.Columns.Contains("select"))
                            {
                                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                                checkBoxColumn.Name = "select";
                                checkBoxColumn.HeaderText = "选择";
                                checkBoxColumn.TrueValue = true;
                                checkBoxColumn.FalseValue = false;
                                dataGridViewTeleControlTable.Columns.Insert(dataGridViewTeleControlTable.Columns.Count, checkBoxColumn);
                            }
                            //设置样式
                            SetDataGridViewStyle(dataGridViewTeleControlTable);
                            //设置表头
                            var controlHeaderText = new string[4] { "序号", "名称", "点号", "说明" };
                            var controlDatabaseName = new string[4] { "Index", "Name", "PointNumber", "Comment" };
                            for (int index = 0; index < controlHeaderText.Length; index++)
                            {
                                var currentName = controlDatabaseName[index];
                                dataGridViewTeleControlTable.Columns[currentName].HeaderText = controlHeaderText[index];
                                if (currentName.Equals("Comment"))
                                {
                                    dataGridViewTeleControlTable.Columns[currentName].Width = 790;
                                }
                            }
                            //某些列不可改
                            for (int columnIndex = 0; columnIndex < dataGridViewTeleControlTable.Columns.Count; columnIndex++)
                            {
                                if (columnIndex == 0 || columnIndex == 1 || columnIndex == 4)
                                {
                                    dataGridViewTeleControlTable.Columns[columnIndex].ReadOnly = true;
                                }
                            }
                            break;
                        }
                    case "ThreeTeleTest":
                        {
                            DataTable dtTest = SQLiteHelper.ReadTable("ThreeTeleTest");
                            dataGridViewPassageData.DataSource = dtTest;
                            SQLiteHelper.Close();
                            //设置表格的基本样式
                            SetDataGridViewStyle(dataGridViewPassageData);
                            SetDataGridViewStyle(dataGridViewError);
                            //设置表头名称
                            var testHeaderText = new string[4] { "属性", "幅值", "相位", "频率" };
                            var testDatabaseName = new string[4] { "Attr", "Mag", "Ang", "Fre" };
                            for (int index = 0; index < testHeaderText.Length; index++)
                            {
                                var currentName = testDatabaseName[index];
                                dataGridViewPassageData.Columns[currentName].HeaderText = testHeaderText[index];
                            }
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 根据表名保存点表到数据库
        /// </summary>
        /// <param name="tableNmae">点表名称</param>
        private void SaveTable(string tableNmae)
        {
            try
            {
                List<string> sqlList = new List<string>();
                switch (tableNmae)
                {
                    case "TeleMeterTable":
                        {
                            for (int rowIndex = 0; rowIndex < dataGridViewTeleMeterTable.Rows.Count; rowIndex++)
                            {
                                string sql = string.Format("INSERT INTO TeleMeterTable" +
                                    " VALUES({0},\'{1}\',{2},{3},\'{4}\',\'{5}\')",
                                int.Parse(dataGridViewTeleMeterTable.Rows[rowIndex].Cells[0].Value.ToString()),
                                dataGridViewTeleMeterTable.Rows[rowIndex].Cells[1].Value.ToString(),
                                int.Parse(dataGridViewTeleMeterTable.Rows[rowIndex].Cells[2].Value.ToString()),
                                double.Parse(dataGridViewTeleMeterTable.Rows[rowIndex].Cells[3].Value.ToString()),
                                dataGridViewTeleMeterTable.Rows[rowIndex].Cells[4].Value.ToString(),
                                dataGridViewTeleMeterTable.Rows[rowIndex].Cells[5].Value.ToString());
                                sqlList.Add(sql);
                            }
                            break;
                        }
                    case "TeleSingalTable":
                        {
                            for (int rowIndex = 0; rowIndex < dataGridViewTeleSingalTable.Rows.Count; rowIndex++)
                            {
                                string sql = string.Format("INSERT INTO TeleSingalTable" +
                                    " VALUES({0},\'{1}\',{2},{3},\'{4}\')",
                                int.Parse(dataGridViewTeleSingalTable.Rows[rowIndex].Cells[0].Value.ToString()),
                                dataGridViewTeleSingalTable.Rows[rowIndex].Cells[1].Value.ToString(),
                                int.Parse(dataGridViewTeleSingalTable.Rows[rowIndex].Cells[2].Value.ToString()),
                                double.Parse(dataGridViewTeleSingalTable.Rows[rowIndex].Cells[3].Value.ToString()),
                                dataGridViewTeleSingalTable.Rows[rowIndex].Cells[4].Value.ToString());
                                sqlList.Add(sql);
                            }
                            break;
                        }
                    case "TeleControlTable":
                        {
                            for (int rowIndex = 0; rowIndex < dataGridViewTeleControlTable.Rows.Count; rowIndex++)
                            {
                                string sql = string.Format("INSERT INTO TeleControlTable" +
                                    " VALUES({0},\'{1}\',{2},\'{3}\')",
                                int.Parse(dataGridViewTeleControlTable.Rows[rowIndex].Cells[1].Value.ToString()),
                                dataGridViewTeleControlTable.Rows[rowIndex].Cells[2].Value.ToString(),
                                int.Parse(dataGridViewTeleControlTable.Rows[rowIndex].Cells[3].Value.ToString()),
                                dataGridViewTeleControlTable.Rows[rowIndex].Cells[4].Value.ToString());
                                sqlList.Add(sql);
                            }
                            break;
                        }
                }
                if (sqlList.Count > 0)
                {
                    string sqlClear = "delete from " + tableNmae;
                    SQLiteHelper.InsertTable(sqlList, sqlClear, dbPath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        /// <summary>
        /// 获取遥测属性的值
        /// </summary>
        /// <param name="dt"></param>
        private void GetBasicMeterAttrValue(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                teleMeterIndex.Add(int.Parse(dt.Rows[i]["Index"].ToString()));
                teleMeterName.Add(dt.Rows[i]["Name"].ToString());
                teleMeterUnit.Add(dt.Rows[i]["Unit"].ToString());
                teleMeterComment.Add(dt.Rows[i]["Comment"].ToString());
            }
        }

        /// <summary>
        /// 获取遥信属性的值
        /// </summary>
        /// <param name="dt"></param>
        private void GetBasicSingalAttrValue(DataTable dt)
        {
            for (int i = 7; i < dt.Rows.Count; i++)
            {
                teleSingalIndex.Add(int.Parse(dt.Rows[i]["Index"].ToString()));
                teleSingalName.Add(dt.Rows[i]["Name"].ToString());
                teleSingalComment.Add(dt.Rows[i]["Comment"].ToString());
            }
        }

        /// <summary>
        /// 增加遥测点表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddMeterTable_Click(object sender, EventArgs e)
        {
            meterTableCount++;
            for (int i = 0; i < teleMeterIndex.Count; i++)
            {
                DataRow drw = dtBasicMeter.NewRow();//定义一行
                drw["Index"] = teleMeterIndex[i] + teleMeterIndex.Count * (meterTableCount - 1);
                drw["Name"] = teleMeterName[i].Replace(1.ToString(), meterTableCount.ToString());
                drw["PointNumber"] = "0";
                drw["Value"] = "0";
                drw["Unit"] = teleMeterUnit[i];
                drw["Comment"] = teleMeterComment[i];
                dtBasicMeter.Rows.Add(drw);
            }
            dataGridViewTeleMeterTable.DataSource = dtBasicMeter;
        }

        /// <summary>
        /// 保存遥测点表到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            SaveTable("TeleMeterTable");
        }

        /// <summary>
        /// 重新载入遥测点表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsMenuItemReloadMeterTable_Click(object sender, EventArgs e)
        {
            ReloadTable("TeleMeterTable");
        }

        /// <summary>
        /// 添加遥信点表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddTeleSingalTable_Click(object sender, EventArgs e)
        {
            singalTableCount++;
            for (int i = 0; i < teleSingalIndex.Count; i++)
            {
                DataRow drw = dtBasicSingal.NewRow();//定义一行
                drw["Index"] = teleSingalIndex[i] + teleSingalIndex.Count * (singalTableCount - 1);
                drw["Name"] = teleSingalName[i].ToString();
                drw["PointNumber"] = "0";
                drw["Value"] = "0";
                drw["Comment"] = teleSingalComment[i];
                dtBasicSingal.Rows.Add(drw);
            }
            dataGridViewTeleSingalTable.DataSource = dtBasicSingal;
        }

        /// <summary>
        /// 保存遥信点表到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemSaveSingalTable_Click(object sender, EventArgs e)
        {
            SaveTable("TeleSingalTable");
        }

        /// <summary>
        /// 重新载入遥信点表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsMenuItemReloadSingalTable_Click(object sender, EventArgs e)
        {
            ReloadTable("TeleSingalTable");
        }

        /// <summary>
        /// 保存遥控点表到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemSaveControlTable_Click(object sender, EventArgs e)
        {
            SaveTable("TeleControlTable");
        }

        /// <summary>
        /// 重新载入遥控点表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsMenuItemReloadControlTable_Click(object sender, EventArgs e)
        {
            ReloadTable("TeleControlTable");
        }

        /// <summary>
        /// 点击数据勾上checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewTeleControlTable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int columnCount = dataGridViewTeleControlTable.Columns.Count;
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridViewTeleControlTable.Rows.Count)
                {
                    //checkbox 勾上
                    if ((bool)dataGridViewTeleControlTable.Rows[e.RowIndex].Cells[0].EditedFormattedValue == true)
                    {
                        dataGridViewTeleControlTable.Rows[e.RowIndex].Cells[0].Value = false;
                    }
                    else
                    {
                        dataGridViewTeleControlTable.Rows[e.RowIndex].Cells[0].Value = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 选择所有遥控操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (int rowIndex = 0; rowIndex < dataGridViewTeleControlTable.Rows.Count; rowIndex++)
                {
                    dataGridViewTeleControlTable.Rows[rowIndex].Cells[0].Value = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 取消选择所有遥控操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (int rowIndex = 0; rowIndex < dataGridViewTeleControlTable.Rows.Count; rowIndex++)
                {
                    dataGridViewTeleControlTable.Rows[rowIndex].Cells[0].Value = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 延时操作：代替Thread.Sleep()。防止界面卡
        /// </summary>
        /// <param name="mm">延时的时间</param>
        public static void Delay(int mm)
        {
            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(mm) > DateTime.Now)
            {
                Application.DoEvents();
            }
            return;
        }

        #region 时间测试
        /// <summary>
        /// 时间测试：设置时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSetTime_Click(object sender, EventArgs e)
        {

            int year = int.Parse(textBoxYear.Text.ToString());
            int month = int.Parse(textBoxMonth.Text.ToString());
            int date = int.Parse(textBoxDate.Text.ToString());
            int hour = int.Parse(textBoxHour.Text.ToString());
            int minute = int.Parse(textBoxMinute.Text.ToString());
            int second = int.Parse(textBoxSecond.Text.ToString());
            int millSec = int.Parse(textBoxMillSec.Text.ToString());
            int dayOfWeek = int.Parse(textBoxDayOfWeek.Text.ToString());
            iec.SetTimeParam(new DateTime(year, month, date, hour, minute, second, millSec));
            iec.Set("Set");
        }


        /// <summary>
        /// 时间测试：读取时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnreadTime_Click(object sender, EventArgs e)
        {
            iec.Read("Read");
            iecOut.GetTimeData = DealTimeData;
        }

        /// <summary>
        /// 处理接收的时间数据
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="dayofWeek"></param>
        /// <returns></returns>
        private bool DealTimeData(DateTime dateTime, int dayofWeek)
        {
            txbTerminalYear.Text = dateTime.Year.ToString();
            txbTerminalMonth.Text = dateTime.Month.ToString();
            txbTerminaldate.Text = dateTime.Day.ToString();
            txbTerminalHour.Text = dateTime.Hour.ToString();
            txbTerminalMinute.Text = dateTime.Minute.ToString();
            txbTerminalSec.Text = dateTime.Second.ToString();
            txbTerminalMillSec.Text = dateTime.Millisecond.ToString();
            txbTerDateOfWeek.Text = dayofWeek.ToString();
            return true;
        }

        #endregion

        private void CkbSystemTime_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbSystemTime.Checked)
            {
                textBoxYear.Text = DateTime.Now.Year.ToString();
                textBoxMonth.Text = DateTime.Now.Month.ToString();
                textBoxDate.Text = DateTime.Now.Day.ToString();
                textBoxHour.Text = DateTime.Now.Hour.ToString();
                textBoxMinute.Text = DateTime.Now.Minute.ToString();
                textBoxSecond.Text = DateTime.Now.Second.ToString();
                textBoxMillSec.Text = DateTime.Now.Millisecond.ToString();
                int dayOfWeek = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                textBoxDayOfWeek.Text = dayOfWeek.ToString();
            }
        }


        private void TextBoxYear_Validating(object sender, CancelEventArgs e)
        {
            if (textBoxYear.Text == string.Empty || !Regex.IsMatch(textBoxYear.Text, @"^\d{2}$|^\d{4}$"))
            {
                errorProviderTime.SetError(textBoxYear, "请输入正确的年份！");
                e.Cancel = true;
            }
            else
            {
                errorProviderTime.SetError(textBoxYear, "");
            }
        }

        private void TextBoxMonth_Validating(object sender, CancelEventArgs e)
        {
            if(!Regex.IsMatch(textBoxMonth.Text, @"^\d*$") || textBoxMonth.Text == string.Empty)
            {
                errorProviderTime.SetError(textBoxMonth, "月份必须为数字！");
                e.Cancel = true;
            }
            else if (int.Parse(textBoxMonth.Text) < 1 || int.Parse(textBoxMonth.Text) > 12)
            {
                errorProviderTime.SetError(textBoxMonth, "请输入正确的月份！");
                e.Cancel = true;
            }
            else
            {
                errorProviderTime.SetError(textBoxMonth, "");
            }
        }


        private void TextBoxDate_Validating(object sender, CancelEventArgs e)
        {
            if (!Regex.IsMatch(textBoxDate.Text, @"^\d*$") || textBoxDate.Text == string.Empty ||
                int.Parse(textBoxDate.Text) < 1 || int.Parse(textBoxDate.Text) > 31)
            {
                errorProviderTime.SetError(textBoxDate, "请输入1-31的数字！");
                e.Cancel = true;
            }
            else
            {
                errorProviderTime.SetError(textBoxDate, "");
            }
        }


        private void TextBoxHour_Validating(object sender, CancelEventArgs e)
        {
            if (!Regex.IsMatch(textBoxHour.Text, @"^\d*$") || textBoxHour.Text == string.Empty ||
                int.Parse(textBoxHour.Text) <= 0 || int.Parse(textBoxHour.Text) >= 12)
            {
                errorProviderTime.SetError(textBoxHour, "请输入0-60的整数！");
                e.Cancel = true;
            }
            else
            {
                errorProviderTime.SetError(textBoxHour, "");
            }
        }


        private void TextBoxMinute_Validating(object sender, CancelEventArgs e)
        {
            if (!Regex.IsMatch(textBoxMinute.Text, @"^\d*$") || textBoxMinute.Text == string.Empty ||
                int.Parse(textBoxMinute.Text) <= 0 || int.Parse(textBoxMinute.Text) >= 60)
            {
                errorProviderTime.SetError(textBoxMinute, "请输入0-60的整数！");
                e.Cancel = true;
            }
            else
            {
                errorProviderTime.SetError(textBoxMinute, "");
            }
        }


        private void TextBoxSecond_Validating(object sender, CancelEventArgs e)
        {
            if (!Regex.IsMatch(textBoxSecond.Text, @"^\d*$") || textBoxSecond.Text == string.Empty ||
                int.Parse(textBoxSecond.Text) <= 0 || int.Parse(textBoxSecond.Text) >= 60)
            {
                errorProviderTime.SetError(textBoxSecond, "请输入0-60的整数！");
                e.Cancel = true;
            }
            else
            {
                errorProviderTime.SetError(textBoxSecond, "");
            }
        }

        private void TextBoxMillSec_Validating(object sender, CancelEventArgs e)
        {
            if (!Regex.IsMatch(textBoxMillSec.Text, @"^\d*$") || textBoxMillSec.Text == string.Empty ||
                int.Parse(textBoxMillSec.Text) <= 0 || int.Parse(textBoxMillSec.Text) >= 1000)
            {
                errorProviderTime.SetError(textBoxMillSec, "请输入0-1000的整数！");
                e.Cancel = true;
            }
            else
            {
                errorProviderTime.SetError(textBoxMillSec, "");
            }
        }

        private void BtnOpenFaultRecord_Click(object sender, EventArgs e)
        {
            iec.OpenFaultRecord("");
        }
    }
}