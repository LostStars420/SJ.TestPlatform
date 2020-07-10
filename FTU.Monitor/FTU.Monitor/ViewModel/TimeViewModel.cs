using FTU.Monitor.DataService;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using lib60870;
using System;
using System.Collections.ObjectModel;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// TimeViewModel 的摘要说明
    /// author: songminghao
    /// date：2017/10/12 9:48:09
    /// desc：时钟同步ViewModel
    /// version: 1.0
    /// </summary>
    public class TimeViewModel : ViewModelBase,IIEC104Handler
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public TimeViewModel()
        {
            // 注册接收到时钟同步ASDU的处理事件 
            IEC104.RegisterIEC104Handler(TypeID.C_CS_NA_1, this);// 时钟同步命令 103

            TimeCommand = new RelayCommand<string>(ExecuteTimeCommand);

            timeData = new ObservableCollection<CP56Time2a>();
            timeData.Add(new CP56Time2a(DateTime.Now));//本机时间
            timeData.Add(new CP56Time2a(DateTime.Now));//终端时间
        }

        /// <summary>
        /// 绑定界面时间设定的值
        /// </summary>
        public static ObservableCollection<CP56Time2a> timeData;

        /// <summary>
        /// 获取和设置绑定界面时间设定的值
        /// </summary>
        public ObservableCollection<CP56Time2a> TimeData
        {
            get { return timeData; }
            set
            {
                timeData = value;
                this.DayOfWeek = TimeData[0].DayOfWeek;
                RaisePropertyChanged(() => TimeData);
            }
        }

        /// <summary>
        /// 年份
        /// </summary>
        public int year;

        /// <summary>
        /// 获取和设置年份
        /// </summary>
        public int Year
        {
            get
            {
                return this.year;
            }
            set
            {
                this.year = value;
                RaisePropertyChanged(() => Year);
            }
        }

        /// <summary>
        /// 星期几
        /// </summary>
        public int dayOfWeek;

        /// <summary>
        /// 获取和设置星期几
        /// </summary>
        public int DayOfWeek
        {
            get 
            {
                return TimeData[0].DayOfWeek; 
            }
            set
            {
                dayOfWeek = value;
                RaisePropertyChanged(() => DayOfWeek);
            }
        }

        /// <summary>
        /// 判断系统时间是否选中
        /// </summary>
        private bool systemTimeChecked;

        /// <summary>
        /// 获取和设置系统时间是否选中
        /// </summary>
        public bool SystemTimeChecked
        {
            get 
            {
                return this.systemTimeChecked;
            }
            set 
            {
                this.systemTimeChecked = value;
                RaisePropertyChanged(() => SystemTimeChecked);
            }
        }

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


        /// <summary>
        /// 设置时间参数
        /// </summary>
        /// <param name="isChecked"></param>
        /// <param name="dateTime"></param>
        public void SetTimeParam(DateTime dateTime)
        {
            TimeViewModel.timeData[0] = new CP56Time2a(dateTime);
        }

        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool Set(string s)
        {
            ExecuteTimeCommand("Set");
            return true;
        }

        /// <summary>
        /// 读取时间
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool Read(string s)
        {
            ExecuteTimeCommand("Read");
            return true;
        }

        /// <summary>
        /// 时钟同步绑定事件
        /// </summary>
        public RelayCommand<string> TimeCommand { get; private set; }

        /// <summary>
        /// 时钟同步绑定事件执行方法
        /// </summary>
        /// <param name="arg"></param>
        void ExecuteTimeCommand(string arg)
        {
            if(!CommunicationViewModel.IsLinkConnect())
            {
                return;
            }
            switch (arg)
            {
                case "Read"://读取时间                   
                 
                    CP56Time2a myTime = new CP56Time2a();
                    CommunicationViewModel.con.SendClockReadCommand(0x01, myTime);
                    break;

                case "Set"://时钟同步

                    //if (this.systemTimeChecked)
                    //{
                    //    TimeViewModel.timeData[0] = new CP56Time2a(DateTime.Now);
                    //}
                    //CommunicationViewModel.con.SendClockSyncCommand(0x01, new CP56Time2a(DateTime.Now));
                    CommunicationViewModel.con.SendClockSyncCommand(0x01, TimeViewModel.timeData[0]);
                    //TimeViewModel.timeData[0] = new CP56Time2a(DateTime.Now);
                    break;

                case "DelayGet"://延时获得

                    CommunicationViewModel.con.SendDelayAcquisitionCommand(CauseOfTransmission.ACTIVATION, 0x01, new CP16Time2a(DateTime.Now.Second * 1000 + DateTime.Now.Millisecond));
                    break;

                case "DelaySend"://延时传递                   
                    CommunicationViewModel.con.SendDelayAcquisitionCommand(CauseOfTransmission.SPONTANEOUS, 0x01, new CP16Time2a(DateTime.Now.Second * 1000 + DateTime.Now.Millisecond));
                    break;

            }
        }

        /// <summary>
        /// 接受处理遥信数据集合
        /// </summary>
        /// <param name="TI">时钟同步类型标识</param>
        /// <param name="asdu">时钟同步类型标识的ASDU</param>
        public void HandleASDUData(TypeID TI, ASDU asdu)
        {
            if (asdu.Cot == CauseOfTransmission.REQUEST)//读取
            {
                var csc = (ClockSynchronizationCommand)asdu.GetElement(0);

                TimeViewModel.timeData[1] = csc.NewTime;

                //向检测平台界面发送数据
                var terminalTime = TimeViewModel.timeData[1];
                var dateTime = new DateTime(terminalTime.Year, terminalTime.Month, terminalTime.DayOfMonth,
                    terminalTime.Hour, terminalTime.Minute, terminalTime.Second, terminalTime.Millisecond);
                var DayOfWeek = (int)terminalTime.DayOfWeek == 0 ? 7 : (int)terminalTime.DayOfWeek;
                MainWindow.IecOut.GetTimeData(dateTime, DayOfWeek);

                ParseInformationShow = "终端时间：" + csc.NewTime.ToString() + "\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);
            }
            else if (asdu.Cot == CauseOfTransmission.ACTIVATION_CON)
            {
                Console.WriteLine((asdu.IsNegative ? "Negative" : "Positive") + "confirmation for SYNCtime command");
                var csc = (ClockSynchronizationCommand)asdu.GetElement(0);

                ParseInformationShow = (asdu.IsNegative ? "Negative" : "Positive") + "confirmation for SYNCtime command" + csc.NewTime.ToStringDateTime() + "\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);
            }
            else if (asdu.Cot == CauseOfTransmission.ACTIVATION_TERMINATION)
            {
                Console.WriteLine("SYNCtime command terminated");

                ParseInformationShow = "SYNCtime command terminated" + "\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);
            }

        }

    }
}
