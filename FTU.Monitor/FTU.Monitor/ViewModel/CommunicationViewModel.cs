using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System;
using System.Threading;
using System.Windows;
using System.IO.Ports;
using FTU.Monitor.DataService;
using lib60870;
using FTU.Monitor.Util;
using System.Timers;
using FTU.Monitor.Model;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using Microsoft.Office.Interop.Excel;
using System.Globalization;
using FTU.Monitor.View;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// CommunicationViewModel 的摘要说明
    /// author: songminghao
    /// date：2017/10/12 9:48:09
    /// desc：通信连接ViewModel
    /// version: 1.0
    /// </summary>
    public class CommunicationViewModel : ViewModelBase, IDisposable, IIEC104Handler
    {
        /// <summary>
        /// 链路连接对象
        /// </summary>
        public static Connection con;

        /// <summary>
        /// 总召唤结束标志
        /// </summary>
        public static bool GeneralInterrogationFinished = true;

        /// <summary>
        /// 用于显示报文序号
        /// </summary>
        public static int RawMessageCount = 0;

        /// <summary>
        /// 保存当前显示的报文数量
        /// </summary>
        public static int CurrentRawMessageCount = 0;

        /// <summary>
        /// 串口接收数据超时时间
        /// </summary>
        System.Timers.Timer timerSerialPortReadTimeout;
        //System.Timers.Timer timerTcpConnectTimeout = new System.Timers.Timer(2000);

        /// <summary>
        /// 串口数据接收超时触发事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void serialPortReadTimeoutTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            timerSerialPortReadTimeout.Stop();

            LogHelper.Info(typeof(CommunicationViewModel), "接收报文：" + UtilHelper.ListToString(SerialPortService.ReceivceBuffer));

            serialPortSerice.CheckLinkData(SerialPortService.ReceivceBuffer);

            SerialPortService.ReceivceBuffer.Clear();
            for (int i = 0; i < SerialPortService.ReceivceBuffer.Count; i++)
            {
                Console.WriteLine(SerialPortService.ReceivceBuffer[i]);
            }

        }

        #region 通道监视测试帧定时器定义
        // 监视串口测试帧的定时器
        public static System.Timers.Timer testFrameTimerForChannelMonitor;

        // 监视串口测试帧的定时器计时是否开始标志
        public static bool isTiming = false;

        // 是否收到上一条任意报文
        public static bool isReceiveFrameResponse = true;

        // 通道监视未收到报文的超时次数
        public static UInt32 unrecieveFrameTime = 0;
        #endregion 通道监视测试帧定时器定义

        #region 101平衡模式下测试帧定时器定义

        // 101平衡模式测试帧定时器
        private static System.Timers.Timer testFrameFor101Balance;

        // 101平衡模式是否收到任意一条报文
        private bool isReciveFrameResponse101Balance = true;

        // 101平衡模式下测试帧重传的次数
        private UInt32 reTransferCount = 0;

        #endregion 101平衡模式下测试帧定时器定义

        #region 101通信协议召唤固有参数定时器定义

        // 101平衡模式召唤固有参数定时器
        private System.Timers.Timer callFiexedParameterFor101;

        #endregion 101通信协议召唤固有参数定时器定义

        /// <summary>
        /// 召唤二级数据定时器
        /// </summary>
        System.Timers.Timer timerAskSecondData;

        /// <summary>
        /// 召唤二级数据定时器触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void timerAskSecondData_Elapsed(object sender, EventArgs e)
        {

            //召唤二级数据条件：非平衡方式，总召唤结束，无一级数据，召唤使能， 定时器超时,重传未超时
            if (CommunicationViewModel.selectedIndexProtocol == 1 && SerialPortService.AskAllOver == true)//非平衡方式
            {
                serialPortSerice.CmdAskSecondData();
            }
        }

        /// <summary>
        /// 串口连接服务对象
        /// </summary>
        public static SerialPortService serialPortSerice = new SerialPortService();

        /// <summary>
        /// 连接参数
        /// </summary>
        public static ConnectionParameters parameters;

        /// <summary>
        /// 定时总召唤定时器
        /// </summary>
        public static System.Timers.Timer timingGeneralInterrogationTimer;

        /// <summary>
        /// 开启定时总召唤
        /// </summary>
        /// <param name="t">定时器</param>
        public static void OpenTimingGeneralInterrogation(System.Timers.Timer t)
        {
            //timingGeneralInterrogationTimer = new System.Timers.Timer(5000);
            //timingGeneralInterrogationTimer.Elapsed += TimingGeneralInterrogationTimeOutEvent;
            t.Elapsed += TimingGeneralInterrogationTimeOutEvent;
        }

        /// <summary>
        /// 关闭定时总召唤
        /// </summary>
        public static void CloseTimingGeneralInterrogation()
        {
            timingGeneralInterrogationTimer.Enabled = false;
        }

        /// <summary>
        /// 定时总召唤
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public static void TimingGeneralInterrogationTimeOutEvent(Object source, ElapsedEventArgs e)
        {
            if (GeneralInterrogationFinished)
            {
                GeneralInterrogationFinished = false;
                con.SendInterrogationCommand(CauseOfTransmission.ACTIVATION, 1, QualifierOfInterrogation.STATION);
            }

        }

        /// <summary>
        /// 用于召唤二级数据
        /// </summary>
        public RelayCommand<string> ValueChangedCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 召唤二级数据执行方法
        /// </summary>
        /// <param name="arg">参数</param>
        public void ExecuteValueChangedCommand(string arg)
        {
            switch (arg)
            {
                case "AskSecondDataTextBox":
                    timerAskSecondData = new System.Timers.Timer(AskSecondDataTime);
                    break;
            }
        }

        /// <summary>
        /// 复选框命令
        /// </summary>
        public RelayCommand<string> CheckBoxCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 复选框命令执行操作
        /// </summary>
        /// <param name="arg"></param>
        public void ExecuteCheckBoxCommand(string arg)
        {
            switch (arg)
            {
                case "AskSecondDataEnable":

                    if (AskSecondDataEnable == true)
                    {
                        timerAskSecondData.Enabled = true;
                    }
                    else
                    {
                        timerAskSecondData.Enabled = false;
                    }
                    break;

            }
        }

        /// <summary>
        /// 加载串口号命令
        /// </summary>
        public RelayCommand LoadPortNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// 加载串口号命令操作执行方法
        /// </summary>
        public void ExecuteLoadPortNumber()
        {
            PortNum.Clear();

            foreach (string s in SerialPort.GetPortNames())
            {
                PortNum.Add(s);
            }

            SelectedIndexPortNum = 0;

        }

        #region 串口设置

        /// <summary>
        /// 波特率集合
        /// </summary>
        private ObservableCollection<int> _baud;

        /// <summary>
        /// 设置和获取波特率集合
        /// </summary>
        public ObservableCollection<int> Baud
        {
            get
            {
                return this._baud;
            }
            set
            {
                this._baud = value;
                RaisePropertyChanged("Baud");
            }
        }

        /// <summary>
        /// 选中的波特率索引
        /// </summary>
        private int _selectedIndexBaud;

        /// <summary>
        /// 设置和获取选中的波特率索引
        /// </summary>
        public int SelectedIndexBaud
        {
            get
            {
                return this._selectedIndexBaud;
            }
            set
            {
                this._selectedIndexBaud = value;
                RaisePropertyChanged("SelectedIndexBaud");
            }
        }

        /// <summary>
        /// 串口号集合
        /// </summary>
        private ObservableCollection<string> _portNum;

        /// <summary>
        /// 设置和获取串口号集合
        /// </summary>
        public ObservableCollection<string> PortNum
        {
            get
            {
                return this._portNum;
            }
            set
            {
                this._portNum = value;
                RaisePropertyChanged("PortNum");
            }
        }

        /// <summary>
        /// 选中的串口号索引
        /// </summary>
        private int _selectedIndexPortNum;

        /// <summary>
        /// 设置和获取选中的串口号索引
        /// </summary>
        public int SelectedIndexPortNum
        {
            get
            {
                return this._selectedIndexPortNum;
            }
            set
            {
                this._selectedIndexPortNum = value;
                RaisePropertyChanged("SelectedIndexPortNum");
            }
        }

        /// <summary>
        /// 奇偶校验位对象集合
        /// </summary>
        private ObservableCollection<Parity> _parityBit;

        /// <summary>
        /// 设置和获取奇偶校验位对象集合
        /// </summary>
        public ObservableCollection<Parity> ParityBit
        {
            get
            {
                return this._parityBit;
            }
            set
            {
                this._parityBit = value;
                RaisePropertyChanged("ParityBit");
            }
        }

        /// <summary>
        /// 选中的奇偶校验位索引
        /// </summary>
        private int _selectedIndexParityBit;

        /// <summary>
        /// 设置和获取选中的奇偶校验位索引
        /// </summary>
        public int SelectedIndexParityBit
        {
            get
            {
                return this._selectedIndexParityBit;
            }
            set
            {
                this._selectedIndexParityBit = value;
                RaisePropertyChanged("SelectedIndexParityBit");
            }
        }

        /// <summary>
        /// 数据位集合
        /// </summary>
        private ObservableCollection<int> _dataBit;

        /// <summary>
        /// 设置和获取数据位集合
        /// </summary>
        public ObservableCollection<int> DataBit
        {
            get
            {
                return this._dataBit;
            }
            set
            {
                this._dataBit = value;
                RaisePropertyChanged("DataBit");
            }
        }

        /// <summary>
        /// 选中的数据位索引
        /// </summary>
        private int _selectedIndexDataBit;

        /// <summary>
        /// 设置和获取选中的数据位索引
        /// </summary>
        public int SelectedIndexDataBit
        {
            get
            {
                return this._selectedIndexDataBit;
            }
            set
            {
                this._selectedIndexDataBit = value;
                RaisePropertyChanged("SelectedIndexDataBit");
            }
        }

        /// <summary>
        /// 停止位集合
        /// </summary>
        private ObservableCollection<StopBits> _stopBit;

        /// <summary>
        /// 设置和获取停止位集合
        /// </summary>
        public ObservableCollection<StopBits> StopBit
        {
            get
            {
                return this._stopBit;
            }
            set
            {
                this._stopBit = value;
                RaisePropertyChanged("StopBit");
            }
        }

        /// <summary>
        /// 选中的停止位索引
        /// </summary>
        private int _selectedIndexStopBit;

        /// <summary>
        /// 设置和获取选中的停止位索引
        /// </summary>
        public int SelectedIndexStopBit
        {
            get
            {
                return this._selectedIndexStopBit;
            }
            set
            {
                this._selectedIndexStopBit = value;
                RaisePropertyChanged("SelectedIndexStopBit");
            }
        }

        #endregion 串口设置

        #region 网口设置

        /// <summary>
        /// 端口号
        /// </summary>
        private int _port;

        /// <summary>
        /// 设置和获取端口号
        /// </summary>
        public int Port
        {
            get
            {
                return this._port;
            }
            set
            {
                this._port = value;
                RaisePropertyChanged("Port");
            }
        }

        /// <summary>
        /// IP地址
        /// </summary>
        private string _IPAddress;

        /// <summary>
        /// 设置和获取IP地址
        /// </summary>
        public string IPAddress
        {
            get
            {
                return this._IPAddress;
            }
            set
            {
                this._IPAddress = value;
                RaisePropertyChanged("IPAddress");
            }
        }

        #endregion 网口设置

        #region 规约数据长度设置

        /// <summary>
        /// 类型标识长度集合
        /// </summary>
        private ObservableCollection<byte> _TILen;

        /// <summary>
        /// 设置和获取类型标识长度集合
        /// </summary>
        public ObservableCollection<byte> TILen
        {
            get
            {
                return this._TILen;
            }
            set
            {
                this._TILen = value;
                RaisePropertyChanged("TILen");
            }
        }

        /// <summary>
        /// 选中的类型标识长度索引
        /// </summary>
        private int _selectedIndexTILen;

        /// <summary>
        /// 设置和获取选中的类型标识长度索引
        /// </summary>
        public int SelectedIndexTILen
        {
            get
            {
                return this._selectedIndexTILen;
            }
            set
            {
                this._selectedIndexTILen = value;
                RaisePropertyChanged("SelectedIndexTILen");
            }
        }

        /// <summary>
        /// 可变结构限定词长度集合
        /// </summary>
        private ObservableCollection<byte> _VSQLen;

        /// <summary>
        /// 设置和获取可变结构限定词长度集合
        /// </summary>
        public ObservableCollection<byte> VSQLen
        {
            get
            {
                return this._VSQLen;
            }
            set
            {
                this._VSQLen = value;
                RaisePropertyChanged("VSQLen");
            }
        }

        /// <summary>
        /// 选中的可变结构限定词长度索引
        /// </summary>
        private int _selectedIndexVSQLen;

        /// <summary>
        /// 设置和获取选中的可变结构限定词长度索引
        /// </summary>
        public int SelectedIndexVSQLen
        {
            get
            {
                return this._selectedIndexVSQLen;
            }
            set
            {
                this._selectedIndexVSQLen = value;
                RaisePropertyChanged("SelectedIndexVSQLen");
            }
        }

        /// <summary>
        /// 传送原因长度集合
        /// </summary>
        public ObservableCollection<byte> _COTLen;

        /// <summary>
        /// 设置和获取传送原因长度集合
        /// </summary>
        public ObservableCollection<byte> COTLen
        {
            get
            {
                return this._COTLen;
            }
            set
            {
                this._COTLen = value;
                RaisePropertyChanged("COTLen");
            }
        }

        /// <summary>
        /// 选中的传送原因长度索引
        /// </summary>
        private int _selectedIndexCOTLen;

        /// <summary>
        /// 设置和获取选中的传送原因长度索引
        /// </summary>
        public int SelectedIndexCOTLen
        {
            get
            {
                return this._selectedIndexCOTLen;
            }
            set
            {
                this._selectedIndexCOTLen = value;
                RaisePropertyChanged("SelectedIndexCOTLen");
            }
        }

        /// <summary>
        /// ASDU公共地址
        /// </summary>
        public static UInt16 _ASDUAddress;

        /// <summary>
        /// 设置和获取ASDU公共地址
        /// </summary>
        public UInt16 ASDUAddress
        {
            get
            {
                return _ASDUAddress;
            }
            set
            {
                _ASDUAddress = value;
                RaisePropertyChanged("ASDUAddress");
            }
        }

        /// <summary>
        /// ASDU公共地址长度集合
        /// </summary>
        private ObservableCollection<byte> _ASDUAddressLen;

        /// <summary>
        /// 设置和获取ASDU公共地址长度集合
        /// </summary>
        public ObservableCollection<byte> ASDUAddressLen
        {
            get
            {
                return this._ASDUAddressLen;
            }
            set
            {
                this._ASDUAddressLen = value;
                RaisePropertyChanged("ASDUAddressLen");
            }
        }

        /// <summary>
        /// 选中的ASDU公共地址长度索引
        /// </summary>
        private int _selectedIndexASDUAddressLen;

        /// <summary>
        /// 设置和获取选中的ASDU公共地址长度索引
        /// </summary>
        public int SelectedIndexASDUAddressLen
        {
            get
            {
                return this._selectedIndexASDUAddressLen;
            }
            set
            {
                this._selectedIndexASDUAddressLen = value;
                RaisePropertyChanged("SelectedIndexASDUAddressLen");
            }
        }

        /// <summary>
        /// 信息对象地址长度集合
        /// </summary>
        private ObservableCollection<byte> _infomationObjectLen;

        /// <summary>
        /// 设置和获取信息对象地址长度集合
        /// </summary>
        public ObservableCollection<byte> InfomationObjectLen
        {
            get
            {
                return this._infomationObjectLen;
            }
            set
            {
                this._infomationObjectLen = value;
                RaisePropertyChanged("InfomationObjectLen");
            }
        }

        /// <summary>
        /// 选中的信息对象地址长度索引
        /// </summary>
        private int _selectIndexInfomationObjectLen;

        /// <summary>
        /// 设置和获取选中的信息对象地址长度索引
        /// </summary>
        public int SelectIndexInfomationObjectLen
        {
            get
            {
                return this._selectIndexInfomationObjectLen;
            }
            set
            {
                this._selectIndexInfomationObjectLen = value;
                RaisePropertyChanged("SelectIndexInfomationObjectLen");
            }
        }

        /// <summary>
        /// 设备地址
        /// </summary>
        public static int deviceAddress;

        /// <summary>
        /// 设置和获取设备地址
        /// </summary>
        public int DeviceAddress
        {
            get
            {
                return deviceAddress;
            }
            set
            {
                deviceAddress = value;
                RaisePropertyChanged("DeviceAddress");
            }
        }

        /// <summary>
        /// 设备地址长度集合
        /// </summary>
        private ObservableCollection<byte> _deviceAddressLen;

        /// <summary>
        /// 设置和获取设备地址长度集合
        /// </summary>
        public ObservableCollection<byte> DeviceAddressLen
        {
            get
            {
                return this._deviceAddressLen;
            }
            set
            {
                this._deviceAddressLen = value;
                RaisePropertyChanged("DeviceAddressLen");
            }
        }

        /// <summary>
        /// 选中的设备地址长度索引
        /// </summary>
        private int _selectIndexDeviceAddressLen;

        /// <summary>
        /// 设置和获取选中的设备地址长度索引
        /// </summary>
        public int SelectIndexDeviceAddressLen
        {
            get
            {
                return this._selectIndexDeviceAddressLen;
            }
            set
            {
                this._selectIndexDeviceAddressLen = value;
                RaisePropertyChanged("SelectIndexDeviceAddressLen");
            }
        }

        #endregion 规约数据长度设置

        #region 规约设置

        /// <summary>
        /// 定时召唤二级数据时间
        /// </summary>
        private int _askSecondDataTime;

        /// <summary>
        /// 设置和获取定时召唤二级数据时间
        /// </summary>
        public int AskSecondDataTime
        {
            get
            {
                return this._askSecondDataTime;
            }
            set
            {
                this._askSecondDataTime = value;
                RaisePropertyChanged(() => AskSecondDataTime);
            }
        }

        /// <summary>
        /// 召唤二级数据使能
        /// </summary>
        private bool _askSecondDataEnable;

        /// <summary>
        /// 设置和获取召唤二级数据使能
        /// </summary>
        public bool AskSecondDataEnable
        {
            get
            {
                return this._askSecondDataEnable;
            }
            set
            {
                this._askSecondDataEnable = value;
                RaisePropertyChanged(() => AskSecondDataEnable);
            }
        }

        /// <summary>
        /// 端口选择集合(串口和网口)
        /// </summary>
        public static ObservableCollection<string> portSelect;

        /// <summary>
        /// 设置和获取端口选择集合(串口和网口)
        /// </summary>
        public ObservableCollection<string> PortSelect
        {
            get
            {
                return portSelect;
            }
            set
            {
                portSelect = value;
                RaisePropertyChanged(() => PortSelect);
            }
        }

        /// <summary>
        /// 选中的端口选择(串口或网口)索引
        /// </summary>
        public static int selectedIndexPort;

        /// <summary>
        /// 设置和获取选中的端口选择(串口或网口)索引
        /// </summary>
        public int SelectedIndexPort
        {
            get
            {
                return selectedIndexPort;
            }
            set
            {
                selectedIndexPort = value;
                RaisePropertyChanged(() => SelectedIndexPort);
            }
        }

        /// <summary>
        /// 协议选择集合(101平衡,101非平衡和104平衡)
        /// </summary>
        public static ObservableCollection<string> protocolSelect;

        /// <summary>
        /// 设置和获取协议选择集合(101平衡,101非平衡和104平衡)
        /// </summary>
        public ObservableCollection<string> ProtocolSelect
        {
            get
            {
                return protocolSelect;
            }
            set
            {
                protocolSelect = value;
                RaisePropertyChanged(() => ProtocolSelect);
            }

        }

        /// <summary>
        /// 选中的协议选择(101平衡,101非平衡或104平衡)索引
        /// </summary>
        public static int selectedIndexProtocol;

        /// <summary>
        /// 设置和获取选中的协议选择(101平衡,101非平衡或104平衡)索引
        /// </summary>
        public int SelectedIndexProtocol
        {
            get
            {
                return selectedIndexProtocol;
            }
            set
            {
                selectedIndexProtocol = value;
                RaisePropertyChanged(() => SelectedIndexProtocol);
            }
        }

        #endregion 规约设置

        /// <summary>
        /// 显示信息
        /// </summary>
        private string _parseInformationShow;

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public string ParseInformationShow
        {
            get
            {
                return this._parseInformationShow;
            }
            set
            {
                this._parseInformationShow = value;
            }
        }

        //private TelemeteringViewModel TelemeteringObj;

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public CommunicationViewModel()
        {
            TelesignalisationViewModel telesignalisationObject = new TelesignalisationViewModel();
            TelemeteringViewModel TelemeteringObject = new TelemeteringViewModel();
            // 注册接收到总召唤，初始化结束ASDU的处理事件 
            IEC104.RegisterIEC104Handler(TypeID.C_IC_NA_1, this);// 总召唤 100
            //IEC104.RegisterIEC104Handler(TypeID.M_EI_NA_1, this);// 初始化结束 70

            timerSerialPortReadTimeout = new System.Timers.Timer(100);
            // 注册连接命令操作执行方法消息
            Messenger.Default.Register<string>(this, "LinkCommand", ExecuteLinkCommand);
            // 注册连接建立后相关请求命令执行方法消息
            Messenger.Default.Register<string>(this, "MasterCommand", ExecuteMasterCmd);
            // 启动101平衡测试帧定时器
            Messenger.Default.Register<string>(this, "StartTestFrameTimer", ExecuteStartTestFrameTimer);
            //获取遥测数据列表的执行方法消息
            Messenger.Default.Register<string>(this, "ObtainTelemetery", ExcuteObtainTelemetery);

            // 绑定加载串口号命令操作执行方法
            LoadPortNumber = new RelayCommand(ExecuteLoadPortNumber);
            // 绑定连接建立后相关请求命令执行方法
            MasterCmd = new RelayCommand<string>(ExecuteMasterCmd);
            LinkCommand = new RelayCommand<string>(ExecuteLinkCommand);
            // 绑定复选框命令执行操作
            CheckBoxCommand = new RelayCommand<string>(ExecuteCheckBoxCommand);
            // 串口数据接收触发事件
            SerialPortService.serialPort.DataReceived += serialPort_DataReceived;

            // 初始化波特率集合
            this._baud = new ObservableCollection<int>();
            this._baud.Add(1200);
            this._baud.Add(2400);
            this._baud.Add(4800);
            this._baud.Add(9600);
            this._baud.Add(38400);
            this._baud.Add(115200);
            this._baud.Add(230400);
            // 设置选中的波特率索引
            this._selectedIndexBaud = 5;

            // 初始化数据位集合
            this._dataBit = new ObservableCollection<int>();
            this._dataBit.Add(8);
            this._dataBit.Add(9);
            // 设置选中的数据位索引
            this._selectedIndexDataBit = 0;

            // 初始化奇偶校验位对象集合
            this._parityBit = new ObservableCollection<Parity>();
            this._parityBit.Add(Parity.Even);
            this._parityBit.Add(Parity.Mark);
            this._parityBit.Add(Parity.None);
            this._parityBit.Add(Parity.Odd);
            this._parityBit.Add(Parity.Space);
            // 设置选中的奇偶校验位索引
            this._selectedIndexParityBit = 2;

            // 初始化停止位集合
            this._stopBit = new ObservableCollection<StopBits>();
            this._stopBit.Add(StopBits.None);
            this._stopBit.Add(StopBits.One);
            this._stopBit.Add(StopBits.OnePointFive);
            this._stopBit.Add(StopBits.Two);
            // 设置选中的停止位索引
            this._selectedIndexStopBit = 1;

            // 初始化串口号集合
            this._portNum = new ObservableCollection<string>();
            foreach (string s in SerialPort.GetPortNames())
            {
                this._portNum.Add(s);
            }
            // 设置选中的串口号索引
            this._selectedIndexPortNum = 0;

            // 初始化类型标识长度集合
            this._TILen = new ObservableCollection<byte>();
            this._TILen.Add(1);
            // 设置选中的类型标识长度索引
            this._selectedIndexTILen = 0;

            // 初始化可变结构限定词长度集合
            this._VSQLen = new ObservableCollection<byte>();
            this._VSQLen.Add(1);
            // 设置选中的可变结构限定词长度索引
            this._selectedIndexVSQLen = 0;

            // 初始化传送原因长度集合
            this._COTLen = new ObservableCollection<byte>();
            this._COTLen.Add(2);
            // 设置选中的传送原因长度索引
            this._selectedIndexCOTLen = 0;

            // 设置ASDU公共地址
            _ASDUAddress = 0x01;
            // 初始化ASDU公共地址长度集合
            this._ASDUAddressLen = new ObservableCollection<byte>();
            this._ASDUAddressLen.Add(2);
            // 设置选中的ASDU公共地址长度索引
            this._selectedIndexASDUAddressLen = 0;

            // 初始化信息对象地址长度集合
            this._infomationObjectLen = new ObservableCollection<byte>();
            this._infomationObjectLen.Add(2);
            this._infomationObjectLen.Add(3);
            // 设置选中的信息对象地址长度索引
            this._selectIndexInfomationObjectLen = 0;

            // 设置设备地址
            deviceAddress = 0x01;
            // 初始化设备地址长度集合
            this._deviceAddressLen = new ObservableCollection<byte>();
            this._deviceAddressLen.Add(2);
            // 设置选中的设备地址长度索引
            this._selectIndexDeviceAddressLen = 0;

            // 初始化端口选择集合(串口和网口)
            portSelect = new ObservableCollection<string>();
            portSelect.Add("串口");
            portSelect.Add("网口");

            // 设置选中的端口选择(串口或网口)索引
            selectedIndexPort = 0;

            // 初始化协议选择集合(101平衡,101非平衡和104平衡)
            protocolSelect = new ObservableCollection<string>();
            protocolSelect.Add("101平衡");
            protocolSelect.Add("101非平衡");
            protocolSelect.Add("104平衡");
            // 设置选中的协议选择(101平衡,101非平衡或104平衡)索引
            selectedIndexProtocol = 0;

            // 设置端口号
            this._port = 2404;
            // 设置IP地址
            this._IPAddress = "192.168.60.100";

            // 设置定时召唤二级数据时间
            this._askSecondDataTime = 200;
            // 设置召唤二级数据使能
            this._askSecondDataEnable = true;
            testFrameFor101Balance = new System.Timers.Timer(60000);

            // 初始化召唤二级数据定时器
            timerAskSecondData = new System.Timers.Timer(AskSecondDataTime);
            // 添加召唤二级数据定时器触发事件
            timerAskSecondData.Elapsed += timerAskSecondData_Elapsed;

            // 设置报文信息数据(显示报文数据时使用)
            MainViewModel.outputdata.RawMessageData = "";
            //到达时间的时候执行事件
            //timerSerialPortReadTimeout.Elapsed += new System.Timers.ElapsedEventHandler(serialPortReadTimeoutTimer);
            InitCallFiexedParameterFor101();
        }

        /// <summary>
        /// 初始化101固有参数定时器
        /// </summary>
        private void InitCallFiexedParameterFor101()
        {
            // 召唤固有参数定时器 1秒
            callFiexedParameterFor101 = new System.Timers.Timer(1000);
            callFiexedParameterFor101.AutoReset = false;
            callFiexedParameterFor101.Enabled = true;
            callFiexedParameterFor101.Elapsed += new System.Timers.ElapsedEventHandler(CallFiexedParameterFor101TimerOut);
        }

        /// <summary>
        /// 启动链路结束后1秒 召唤固有参数
        /// </summary>
        private void CallFiexedParameterFor101TimerOut(object source, System.Timers.ElapsedEventArgs e)
        {
            // 读取终端设备运行的软件版本号并做处理
            InherentParameterViewModel.CheckProgrammVersion();
            // 启动测试帧定时器
            Messenger.Default.Send<string>("startTimer", "StartTestFrameTimer");
        }


        /// <summary>
        /// 接受处理遥信数据集合
        /// </summary>
        /// <param name="TI">总召唤类型标识</param>
        /// <param name="asdu">对应总召唤类型标识的ASDU</param>
        public void HandleASDUData(TypeID TI, ASDU asdu)
        {
            if (asdu.TypeId == TypeID.C_IC_NA_1) // 总召唤 100
            {
                ParseInformationShow = "\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);
                if (asdu.Cot == CauseOfTransmission.ACTIVATION_CON)
                {
                    Console.WriteLine((asdu.IsNegative ? "Negative" : "Positive") + "confirmation for interrogation command");
                    ParseInformationShow = (asdu.IsNegative ? "Negative" : "Positive") + "confirmation for interrogation command";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                }
                else if (asdu.Cot == CauseOfTransmission.ACTIVATION_TERMINATION)
                {
                    CommunicationViewModel.GeneralInterrogationFinished = true;
                    Console.WriteLine("Interrogation command terminated");


                    tele = TelemeteringViewModel.telemeteringList;
                    telsa = TelesignalisationViewModel.telesignalisationList;
                    try
                    {
                        string primaryname = "遥测点号信息" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "CSV (*.CSV)|*.csv";
                        saveFileDialog.AddExtension = true;//是否自动添加扩展名
                        saveFileDialog.OverwritePrompt = true;//文件已存在是否提示覆盖
                        saveFileDialog.CheckPathExists = true;//提示输入的文件名无效
                        saveFileDialog.FileName = primaryname;//文件初始名

                        string path = ".\\TelMetering\\" + saveFileDialog.FileName.ToString() + ".csv";//save path
                        System.IO.FileStream fs = new FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs, new System.Text.UnicodeEncoding());



                        string telesingalname = "遥信点号信息" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        SaveFileDialog telesingalDialog = new SaveFileDialog();
                        telesingalDialog.Filter = "CSV (*.CSV)|*.csv";
                        telesingalDialog.AddExtension = true;//是否自动添加扩展名
                        telesingalDialog.OverwritePrompt = true;//文件已存在是否提示覆盖
                        telesingalDialog.CheckPathExists = true;//提示输入的文件名无效
                        telesingalDialog.FileName = telesingalname;//文件初始名

                        string telesingalpath = ".\\TelSingal\\" + telesingalDialog.FileName.ToString() + ".csv";//save path
                        System.IO.FileStream ttelesingalfs = new FileStream(telesingalpath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                        StreamWriter singalsw = new StreamWriter(ttelesingalfs, new System.Text.UnicodeEncoding());

                        //获取遥测数据的列表
                        var list = new List<Tuple<int, float>>();
                        //获取遥信数据的列表
                        var telsaList = new List<Tuple<int, bool>>();

                        //导出遥测数据并存储到列表
                        for (int i = 0; i < tele.Count; i++)
                        {
                            sw.Write(tele[i].ID.ToString());
                            sw.Write("\t");
                            sw.Write(tele[i].VALUE.ToString());
                            sw.WriteLine("");
                            int id = tele[i].ID;
                            Single value = tele[i].VALUE;
                            list.Add(new Tuple<int, float>(id, value));
                        }
                        sw.Flush();
                        sw.Close();
                     //   MessageBox.Show("导出成功", "提示");

                        //导出遥信数据并存储到列表
                        for (int i = 0; i < telsa.Count; i++)
                        {
                            singalsw.Write(telsa[i].ID.ToString());
                            singalsw.Write("\t");
                            singalsw.Write(telsa[i].VALUE.ToString());
                            singalsw.WriteLine("");
                            int id = telsa[i].ID;
                            bool value = telsa[i].VALUE;
                            telsaList.Add(new Tuple<int, bool>(id, value));
                        }
                        singalsw.Flush();
                        singalsw.Close();
                     //   MessageBox.Show("导出成功", "提示");

                        MainWindow.IecOut.GiComplted(list);
                        MainWindow.IecOut.GetTelsa(telsaList);
                        TelemeteringViewModel.telemeteringList.Clear();
                        TelesignalisationViewModel.telesignalisationList.Clear();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                }
            }
        }

        /// <summary>
        /// 启动101平衡测试帧定时器
        /// </summary>
        /// <param name="arg"></param>
        private void ExecuteStartTestFrameTimer(string arg)
        {
            if (protocolSelect[selectedIndexProtocol] == "101平衡")
            {
                testFrameFor101Balance.AutoReset = true;
                testFrameFor101Balance.Enabled = true;
                testFrameFor101Balance.Elapsed += new System.Timers.ElapsedEventHandler(TestFrameFor101BalanceTimerOut);
            }
        }
        //执行总召唤，为获取遥测数据列表
        public static void ExcuteObtainTelemetery(string arg)
        {
            con.SendInterrogationCommand(CauseOfTransmission.ACTIVATION, 1, QualifierOfInterrogation.STATION);
            LogHelper.Info(typeof(CommunicationViewModel), "总召唤结束");
        }
        /// <summary>
        /// 101平衡模式下测试帧超时处理
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void TestFrameFor101BalanceTimerOut(object source, System.Timers.ElapsedEventArgs e)
        {
            // 是否收到任意报文
            if (isReciveFrameResponse101Balance)
            {
                // 收到任意报文后，发送测试帧，并将标志位置false
                isReciveFrameResponse101Balance = false;
                serialPortSerice.CmdHeartbeatData();
            }
            else
            {
                // 未收到任意报文，重发3次测试帧后，关闭定时器，执行启动链路的过程
                reTransferCount++;
                if (reTransferCount == 4)
                {
                    testFrameFor101Balance.Stop();
                    reTransferCount = 0;
                    isReciveFrameResponse101Balance = true;
                    serialPortSerice.SendFixedFrame(0x49);
                    SerialPortService.retransmitTimer.Enabled = true;
                }
                else
                {
                    serialPortSerice.CmdHeartbeatData();
                }
            }
        }

        /// <summary>
        /// 串口数据接收触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            RedefineSerialPort sp = (RedefineSerialPort)sender;

            // 先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致
            int n = SerialPortService.serialPort.BytesToRead;
            // 声明一个临时数组存储当前来的串口数据
            byte[] buf = new byte[n];

            // 判断串口是否为通道监视
            if (System.Threading.Interlocked.Read(ref MainViewModel.ChannelMonitorListening) != 0)
            {
                // 串口为通道监视
                // 读取缓冲数据
                sp.ReadForChannelMonitor(buf, 0, n);
                // 重置定时器
                if (isTiming)
                {
                    // 屏蔽的一下两行代码为取消在空闲链路状态下发送测试帧的逻辑
                    //testFrameTimerForChannelMonitor.Stop();
                    //testFrameTimerForChannelMonitor.Start();
                    isReceiveFrameResponse = true;
                    unrecieveFrameTime = 0;
                }

                SerialPortService.ReceivceBufferForChannelMonitor.AddRange(buf);
                LogHelper.Info(typeof(CommunicationViewModel), "通道监视接收报文：" + UtilHelper.ListToString(SerialPortService.ReceivceBufferForChannelMonitor));
                serialPortSerice.CheckLinkDataForChannelMonitor(ref SerialPortService.ReceivceBufferForChannelMonitor);
            }
            else
            {
                // 读取缓冲数据
                sp.Read(buf, 0, n);

                // 串口不为通道监视所用
                // 若为101平衡，定时器清零，计时从零开始
                if (protocolSelect[selectedIndexProtocol] == "101平衡" && testFrameFor101Balance != null)
                {
                    testFrameFor101Balance.Stop();
                    testFrameFor101Balance.Start();
                    isReciveFrameResponse101Balance = true;
                }
                SerialPortService.ReceivceBuffer.AddRange(buf);
                LogHelper.Info(typeof(CommunicationViewModel), "接收报文：" + UtilHelper.ListToString(SerialPortService.ReceivceBuffer));
                serialPortSerice.CheckLinkData(SerialPortService.ReceivceBuffer);
            }

        }

        public RelayCommand<string> LinkCommand
        {
            get;
            private set;
        }
        /// <summary>
        /// 连接命令操作执行方法
        /// </summary>
        /// <param name="arg">参数</param>
        public void ExecuteLinkCommand(string arg)
        {
            con = new Connection(3);
            // 打开连接
            con = new Connection(IPAddress, Port);

            con.UseSendMessageQueue = false;

            con.SetASDUReceivedHandler(IEC104.asduReceivedHandler, null);
            con.SetConnectionHandler(IEC104.ConnectionHandler, null);

            con.Connect();
            if (con.IsRunning)
            {
                LogHelper.Info(typeof(CommunicationViewModel), "网口连接");
                MessageBox.Show("成功打开网口连接！", "提示");
                return;
            }
            else
            {
                MessageBox.Show("网口未连接");
                return;
            }

        }

        #region 为检测平台提供104接口
        /// <summary>
        /// 打开104链路连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool OpenLink(string ip, int port)
        {
            IPAddress = ip;
            Port = port;
            // Messenger.Default.Send<string>("LinkCommand", "ExecuteLinkCommand");
            ExecuteLinkCommand("");

            return con.IsRunning;
        }

        /// <summary>
        /// 总召唤
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool CmdAskAll(string s)
        {
            ExecuteMasterCmd("CmdAskAll");
            return true;

        }

        /// <summary>
        /// 打开故障录波界面
        /// </summary>
        /// <param name="str"></param>
        public void OpenFaultRecord(string str)
        {
            new FileServiceView().Show();
            new ComtradeView().Show();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public RelayCommand<string> MasterCmd
        {
            get;
            private set;
        }
        //召唤后遥测数据导入的list
        public static List<TelemeteringList> tele;
        public List<TelemeteringList> Tele
        {
            get { return tele; }
            set
            {
                RaisePropertyChanged(() => Tele);
            }
        }
        //召唤后遥信数据导入的list
        public static List<TelesignalisationList> telsa;
        public List<TelesignalisationList> Telsa
        {
            get { return telsa; }
            set
            {
                RaisePropertyChanged(() => Telsa);
            }
        }
        /// <summary>
        /// 连接建立后相关请求命令执行方法
        /// </summary>
        /// <param name="arg">参数</param>
        void ExecuteMasterCmd(string arg)
        {
            if (!CommunicationViewModel.IsLinkConnect())
            {
                return;
            }
            try
            {
                #region 串口命令
                if (SerialPortService.serialPort.IsOpen)
                {
                    switch (arg)
                    {
                        // 请求链路
                        case "CmdStartLink":
                            serialPortSerice.SendFixedFrame(0x49);
                            SerialPortService.retransmitTimer.Enabled = true;
                            // 启动定时器，召唤固有参数
                            callFiexedParameterFor101.Start();
                            break;

                        // 总召唤
                        case "CmdAskAll":               //传送原因列表                       //总召唤命令为100                  
                            con.SendInterrogationCommand(CauseOfTransmission.ACTIVATION, 1, QualifierOfInterrogation.STATION);
                            break;

                        // 链路测试
                        case "CmdTestData":
                            con.SendTestCommand(0x01);
                            break;

                        // 进程复位
                        case "CmdInitProcessData":
                            con.SendResetProcessCommand(CauseOfTransmission.ACTIVATION, 1, 2);
                            break;

                        // 心跳测试
                        case "CmdHeartbeatData":
                            serialPortSerice.CmdHeartbeatData();
                            break;

                        // 召唤一级数据
                        case "CmdAskFirstData":
                            serialPortSerice.CmdAskFirstData();
                            break;

                        // 召唤二级数据
                        case "CmdAskSecondData":
                            serialPortSerice.CmdAskSecondData();
                            break;

                    }
                }
                #endregion 串口命令

                #region 网口命令
                else if (Connection.running)
                {
                    switch (arg)
                    {
                        // 请求链路
                        case "CmdStartLink":
                            con.SendStartDT();
                            break;

                        // 总召唤
                        case "CmdAskAll":
                            con.SendInterrogationCommand(CauseOfTransmission.ACTIVATION, 1, QualifierOfInterrogation.STATION);
                            // MessageBox.Show("召唤结束");
                            //tele= new List<TelemeteringList>(TelemeteringViewModel.telemeteringList);

                            LogHelper.Info(typeof(CommunicationViewModel), "总召唤结束");
                            break;

                        // 链路测试
                        case "CmdTestData":
                            con.SendTestCommand(0x01);
                            break;

                        // 进程复位
                        case "CmdInitProcessData":
                            con.SendResetProcessCommand(CauseOfTransmission.ACTIVATION, 1, 2);
                            SerialPortService.askSecondDataTimer.Enabled = false;
                            break;

                        // 心跳测试
                        case "CmdHeartbeatData":
                            break;

                        // 召唤一级数据
                        case "CmdAskFirstData":
                            break;

                        // 召唤二级数据
                        case "CmdAskSecondData":
                            break;

                    }
                }
                #endregion 网口命令

            }
            catch (Exception ex)
            {
                con.DebugLog(ex.ToString());
                LogHelper.Error(typeof(CommunicationViewModel), "发送控制报文出现异常 \n");
            }
        }

        #region 实现IDisposable接口，释放资源

        /// <summary>
        /// 释放标志位
        /// </summary>
        private int _disposed;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">释放资源布尔值</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
            {
                return;
            }

            if (disposing)
            {
                Destroy();
            }

        }

        /// <summary>
        /// 释放内存资源
        /// </summary>
        virtual protected void Destroy()
        {
            if (timerSerialPortReadTimeout != null)
            {
                timerSerialPortReadTimeout.Close();
                timerSerialPortReadTimeout.Dispose();
            }

            if (timerAskSecondData != null)
            {
                timerAskSecondData.Close();
                timerAskSecondData.Dispose();
            }

            if (timingGeneralInterrogationTimer != null)
            {
                timingGeneralInterrogationTimer.Close();
                timingGeneralInterrogationTimer.Dispose();
            }

        }

        #endregion 实现IDisposable接口，释放资源

        /// <summary>
        /// 串口或者网口是否打开
        /// </summary>
        /// <returns>打开返回true,关闭返回false</returns>
        public static bool IsLinkConnect()
        {
            // 判断串口是否连接
            if (SerialPortService.serialPort.IsOpen)
            {
                return true;
            }
            // 判断网口是否为空
            if (con != null)
            {
                // 判断网口是否连接
                if (con.IsRunning)
                {
                    return true;
                }
            }
            // 串口与网口不通
            MessageBox.Show("物理链路已断开", "警告");
            return false;
        }

        /// <summary>
        /// 关闭链路连接(串口或者网口)
        /// </summary>
        /// <returns>成功关闭返回true,失败返回false</returns>
        public static bool CloseLinkConnect()
        {
            // 关闭串口或者网口
            if (CloseSerialPortConnect() && CloseNetConnect())
            {
                // 成功关闭串口或者网口
                return true;
            }

            // 串口或者网口关闭失败
            return false;
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns>成功关闭返回true,失败返回false</returns>
        public static bool CloseSerialPortConnect()
        {
            SerialPortService.askSecondDataTimer.Enabled = false;
            // 关闭重传计时器
            SerialPortService.retransmitTimer.Enabled = false;
            // 关闭101平衡测试帧计时器
            testFrameFor101Balance.Enabled = false;
            // 重传次数清0
            SerialPortService.retransmitCount = 0;
            // 关闭串口
            if (SerialPortService.serialPort.IsOpen)
            {
                SerialPortService.serialPort.Close();

                if (!SerialPortService.serialPort.IsOpen)
                {
                    return true;
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// 关闭网口
        /// </summary>
        /// <returns>成功关闭返回true,失败返回false</returns>
        public static bool CloseNetConnect()
        {
            // 关闭网口
            if (con != null && con.IsRunning == true)
            {
                con.Close();
                Thread.Sleep(10);
                if (!con.IsRunning)
                {
                    return true;
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// 关闭网口：用于委托调用
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public bool CloseConnection(string arg)
        {
            // 关闭网口
            if (con != null && con.IsRunning == true)
            {
                con.Close();
                Thread.Sleep(10);
                if (!con.IsRunning)
                {
                    return true;
                }
                return false;
            }

            return true;
        }

    }
}
