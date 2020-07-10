using FTU.Monitor.DataService;
using FTU.Monitor.Model;
using FTU.Monitor.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using lib60870;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// ChannelMonitorViewModel 的摘要说明
    /// author: liyan
    /// date：2018/4/10 9:48:09
    /// desc：通道监视ViewModel
    /// version: 1.0
    /// </summary>
    public class ChannelMonitorViewModel : ViewModelBase
    {
        /// <summary>
        /// 通道监视报文帧长度
        /// </summary>
        private const int FEXED_FRAME_SIZE = 6;
        /// <summary>
        /// 控制位，启动监视
        /// </summary>
        private const byte START_MONITOR = 0x09;
        /// <summary>
        /// 控制位，停止监视
        /// </summary>
        private const byte END_MONITOR = 0x0A;
        /// <summary>
        /// FTU回复的，响应监视
        /// </summary>
        private const byte RESPOND_MONITOR = 0x0B;

        /// <summary>
        /// 测试链路
        /// </summary>
        private const byte TEST_LINK = 0x01;

        /// <summary>
        /// 收到监视报文后，输出报文及解析的信息
        /// </summary>
        public static ChannelMonitorShowMessage channelMonitorShowData;

        /// <summary>
        /// 设置和获取收到监视报文后，输出报文及解析的信息
        /// </summary>
        public ChannelMonitorShowMessage ChannelMonitorShowData
        {
            get
            {
                return channelMonitorShowData;
            }
            set
            {
                channelMonitorShowData = value;
                RaisePropertyChanged(() => ChannelMonitorShowData);
            }
        }

        /// <summary>
        /// 通道监视的端口号
        /// </summary>
        private ObservableCollection<string> _portNum;

        /// <summary>
        /// 设置和获取通道监视的端口号
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
                RaisePropertyChanged(() => PortNum);
            }
        }

        /// <summary>
        /// 被选择的监视端口号索引
        /// </summary>
        private static int selectedIndexPortNum;

        /// <summary>
        /// 设置和获取被选择的监视端口号索引
        /// </summary>
        public int SelectedIndexPortNum
        {
            get
            {
                return selectedIndexPortNum;
            }
            set
            {
                selectedIndexPortNum = value;
                RaisePropertyChanged(() => SelectedIndexPortNum);
            }
        }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ChannelMonitorViewModel()
        {
            channelMonitorShowData = new ChannelMonitorShowMessage();
            Messenger.Default.Register<string>(this, "ChannelMonitorCommand", ExecuteChannelMonitorCmd);
            Messenger.Default.Register<string>(this, "TestFrameTimeOut", ExcuteTestFrameTimeOut);
            ChannelMonitorCmd = new RelayCommand<string>(ExecuteChannelMonitorCmd);

            channelMonitorDataMessage = new ObservableCollection<ChannelMonitorShowMessage>();

            //添加通道监视通道号
            this._portNum = new ObservableCollection<string>();
            this._portNum.Add("");
            this._portNum.Add("串口1");
            this._portNum.Add("串口2");

            //通道监视通道索引值初始化为0
            selectedIndexPortNum = 0;
        }

        /// <summary>
        /// 通道监视的数据信息集合
        /// </summary>
        public static ObservableCollection<ChannelMonitorShowMessage> channelMonitorDataMessage;

        /// <summary>
        /// 设置和获取通道监视数据信息集合
        /// </summary>
        public ObservableCollection<ChannelMonitorShowMessage> ChannelMonitorDataMessage
        {
            get
            {
                return channelMonitorDataMessage;
            }
            set
            {
                channelMonitorDataMessage = value;
                RaisePropertyChanged("ChannelMonitorDataMessage");
            }
        }

        /// <summary>
        /// 通道监视命令
        /// </summary>
        public RelayCommand<string> ChannelMonitorCmd
        {
            get;
            private set;
        }

        /// <summary>
        /// 通道监视界面用户动作的处理函数
        /// </summary>
        /// <param name="arg"></param>
        void ExecuteChannelMonitorCmd(string arg)
        {
            try
            {
                switch (arg)
                {
                    //用户点击“开始监视”按钮，响应的动作
                    case "StartMonitoring":
                        StartMonitoring();
                        break;
                    //用户点击“停止监视”按钮，响应的动作
                    case "StopMonitoring":
                        StopMonitoring();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.Error(typeof(ChannelMonitorViewModel),ex.Message);
            }
        }

        /// <summary>
        /// 执行发送测试帧
        /// </summary>
        /// <param name="arg"></param>
        private void ExcuteTestFrameTimeOut(string arg)
        {
            //发送启动监视报文
            SendFixedFrame(TEST_LINK, (UInt16)SelectedIndexPortNum);
        }

        /// <summary>
        /// 启动监视对应的动作
        /// </summary>
        private void StartMonitoring()
        {
            //发送启动监视报文
            SendFixedFrame(START_MONITOR, (UInt16)SelectedIndexPortNum);
        }

        /// <summary>
        /// 停止监视对应的动作
        /// </summary>
        public void StopMonitoring()
        {
            //发送停止监视功能的报文
            SendFixedFrame(END_MONITOR, (UInt16)selectedIndexPortNum);
            // 清除通道监视中的历史数据记录
            ChannelMonitorViewModel.channelMonitorDataMessage.Clear();
        }

        /// <summary>
        /// 组建固定报文帧，并发送
        /// </summary>
        /// <param name="controlByte">控制字节</param>
        /// <param name="addressByte">地址字节</param>
        public void SendFixedFrame(byte controlByte, UInt16 addressByte)
        {
            byte[] fixedFrameBuff = new byte[FEXED_FRAME_SIZE];
            fixedFrameBuff[0] = 0x11;
            fixedFrameBuff[1] = controlByte;
            fixedFrameBuff[2] = (byte)(addressByte & 0x00FF);
            fixedFrameBuff[3] = (byte)(addressByte & 0xFF00);
            fixedFrameBuff[4] = (byte)(controlByte + addressByte);
            fixedFrameBuff[5] = 0x66;

            WriteToDevice(fixedFrameBuff);
        }

        /// <summary>
        /// 向设备发送通道监视交互帧
        /// </summary>
        /// <param name="fixedFrameBuff"></param>
        private void WriteToDevice(byte[] fixedFrameBuff)
        {
            if (CommunicationViewModel.IsLinkConnect())
            {
                //发动报文
                if (SerialPortService.serialPort.IsOpen)
                {
                    if (System.Threading.Interlocked.Read(ref MainViewModel.ChannelMonitorListening) != 0)
                    {
                        // 从串口发送报文
                        SerialPortService.serialPort.WriteByListeningModel(fixedFrameBuff, 0, FEXED_FRAME_SIZE);
                        LogHelper.Info(typeof(ChannelMonitorViewModel), "通道监视串口发送：" + BitConverter.ToString(fixedFrameBuff).Replace("-", string.Empty));
                    }
                    else
                    {
                        SerialPortService.serialPort.Write(fixedFrameBuff, 0, FEXED_FRAME_SIZE);
                        LogHelper.Info(typeof(ChannelMonitorViewModel), "通道监视串口发送：" + BitConverter.ToString(fixedFrameBuff).Replace("-", string.Empty));
                    }
                }
                else
                {
                    if (System.Threading.Interlocked.Read(ref MainViewModel.ChannelMonitorListening) != 0)
                    {
                        // 从网口发送报文
                        Connection.socket.SendByListeningModel(fixedFrameBuff);
                        LogHelper.Info(typeof(ChannelMonitorViewModel), "通道监视网口发送：" + BitConverter.ToString(fixedFrameBuff).Replace("-", string.Empty));
                    }
                    else
                    {
                        Connection.socket.Send(fixedFrameBuff);
                        LogHelper.Info(typeof(ChannelMonitorViewModel), "通道监视网口发送：" + BitConverter.ToString(fixedFrameBuff).Replace("-", string.Empty));
                    }
                }
            }
        }
    }
}
