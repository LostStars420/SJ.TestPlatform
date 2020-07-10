using lib60870;
using FTU.Monitor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using FTU.Monitor.Util;
using FTU.Monitor.Model;
using GalaSoft.MvvmLight.Messaging;

namespace FTU.Monitor.DataService
{
    public class SerialPortService
    {
        public void SetASDUReceivedHandler(ASDUReceivedHandler handler, object parameter)
        {
            asduReceivedHandler = handler;
            asduReceivedHandlerParameter = parameter;
        }
        ASDUReceivedHandler asduReceivedHandler = null;
        object asduReceivedHandlerParameter = null;

        //   bool testModel = true;//测试模式，关闭异常处理
        bool testModel = false;//测试模式，关闭异常处理
        public SerialPortService()
        {
            serialPort = new RedefineSerialPort();
            ReceivceBuffer = new List<byte>(4096);
            ReceivceBufferForChannelMonitor = new List<byte>(4096);
            retransmitTimer = new System.Timers.Timer(2000);
            retransmitTimer.Elapsed += RetransmitTimeOutEvent;
            retransmitEnable = true;

            askSecondDataTimer = new System.Timers.Timer(1000);
            askSecondDataTimer.Elapsed += askSecondDataTimeOutEvent;
            askSecondDataTimer.Enabled = false;

            FCB = 1;
            unknownFrame = new List<byte>();
        }
        public static bool AskAllOver = false;
        public static System.Timers.Timer retransmitTimer;
        public static System.Timers.Timer askSecondDataTimer;
        public static int retransmitCount = 0;
        public static RedefineSerialPort serialPort;
        public static List<byte> ReceivceBuffer;
        public static List<byte> ReceivceBufferForChannelMonitor;
        public static byte[] TxTemp;
        public bool retransmitEnable = false;

        void askSecondDataTimeOutEvent(Object source, ElapsedEventArgs e)
        {
            CmdAskSecondData();
            askSecondDataTimer.Enabled = false;
        }
        void RetransmitTimeOutEvent(Object source, ElapsedEventArgs e)
        {
            //retransmitTimer.Enabled = false;

            retransmitCount++;
            if (retransmitCount == 3)
            {
                retransmitCount = 0;
                TxTemp = new byte[6];
                TxTemp[0] = 0x10;
                TxTemp[1] = 0x49;
                TxTemp[2] = (byte)CommunicationViewModel.deviceAddress;
                TxTemp[3] = (byte)(CommunicationViewModel.deviceAddress >> 8);
                TxTemp[4] = (byte)(TxTemp[1] + TxTemp[2] + TxTemp[3]);
                //TxTemp[4] = 0x4a;
                TxTemp[5] = 0x16;

                serialPort.Write(TxTemp, 0, TxTemp.Length);
                ShowMessage.Show(TxTemp, TxTemp.Length, "发送");
                LogHelper.Info(typeof(SerialPortService), "发送报文：" + UtilHelper.ListToString(TxTemp));
                return;
            }
            serialPort.Write(TxTemp, 0, TxTemp.Length);
            ShowMessage.Show(TxTemp, TxTemp.Length, "发送");
            LogHelper.Info(typeof(SerialPortService), "发送报文：" + UtilHelper.ListToString(TxTemp));
        }


        public byte[] infoObj = new byte[255];
        private byte _infoObjNum;
        public byte infoObjNum
        {
            set { _infoObjNum = value; }
            get { return _infoObjNum; }
        }

        private int RxACD = 0;//ACD标志位
        private byte _FCB;
        public byte FCB
        {
            set { _FCB = value; }
            get { return _FCB; }
        }

        private byte _ProtocolModel;
        public byte ProtocolModel
        {
            set { _ProtocolModel = value; }
            get { return _ProtocolModel; }
        }

        #region 接收数据帧标识位
        /// <summary>
        /// 接收控制域
        /// </summary>
        private byte _RxControlByte;
        public byte RxControlByte
        {
            set { _RxControlByte = value; }
            get { return _RxControlByte; }
        }
        /// <summary>
        /// 发送控制域
        /// </summary>
        private byte _TxControlByte;
        public byte TxControlByte
        {
            set { _TxControlByte = value; }
            get { return _TxControlByte; }
        }
        /// <summary>
        /// 发送帧中的传送原因
        /// </summary>
        private byte _TxCOT;
        public byte TxCOT
        {
            set { _TxCOT = value; }
            get { return _TxCOT; }
        }
        /// <summary>
        /// 接收帧中的传送原因
        /// </summary>
        private UInt16 _RxCOTByte;

        /// <summary>
        /// 获取和设置COT
        /// </summary>
        public UInt16 RxCOTByte
        {
            set
            {
                _RxCOTByte = value;
            }
            get
            {
                return _RxCOTByte;
            }
        }
        /// <summary>
        /// 可变结构限定词
        /// </summary>
        private byte _RxVSQByte;
        public byte RxVSQByte
        {
            set { _RxVSQByte = value; }
            get { return _RxVSQByte; }
        }
        /// <summary>
        /// 接收帧中的设备地址
        /// </summary>
        private UInt16 _RxTerminalAddress;
        public UInt16 RxTerminalAddress
        {
            set { _RxTerminalAddress = value; }
            get { return _RxTerminalAddress; }
        }
        /// <summary>
        /// 设备地址
        /// </summary>
        private UInt16 _TerminalAddress = 0x01;
        public UInt16 TerminalAddress
        {
            set { _TerminalAddress = value; }
            get { return _TerminalAddress; }
        }
        /// <summary>
        /// 类型标志
        /// </summary>
        private byte _RxTypeByte;
        public byte RxTypeByte
        {
            set { _RxTypeByte = value; }
            get { return _RxTypeByte; }
        }

        /// <summary>
        ///接收帧中的ASDU地址
        /// </summary>
        private UInt16 _RxASDUAddress;
        public UInt16 RxASDUAddress
        {
            set { _RxASDUAddress = value; }
            get { return _RxASDUAddress; }
        }
        /// <summary>
        /// 信息体对象地址
        /// </summary>
        private UInt16 _RxInfoObjAddress;
        public UInt16 RxInfoObjAddress
        {
            set { _RxInfoObjAddress = value; }
            get { return _RxInfoObjAddress; }
        }

        public static int parameterContinueFlag = 0;

        /// <summary>
        /// 通道监视报文传输方向
        /// </summary>
        private static string dirForChannelMonitor;

        /// <summary>
        /// 设置和获取通道监视报文传输方向
        /// </summary>
        public static string DirForChannelMonitor
        {
            get 
            { 
                return dirForChannelMonitor; 
            }
            set 
            { 
                dirForChannelMonitor = value; 
            }
        }
        #endregion

        // 串口通道监视接收报文的识别标志位
        private const byte firstSendByte = 0xAA;
        private const byte secondSendByte = 0x55;

        // 暂存错误报文
        private static  List<byte> unknownFrame;

        public static List<byte> UnknownFrame
        {
            get
            {
                return unknownFrame;
            }
            set
            {
                unknownFrame = value;
            }
        }

        public void CheckLinkData(List<byte> buffer)
        {
            parameterContinueFlag = 0;
            while (buffer.Count > 5)
            {
                if (buffer[0] == 0x01 && (buffer[1] == 0x03 || buffer[1] == 0x10))
                {
                    // 地址
                    byte addr = buffer[2];
                    // 数据长度
                    byte dataLen = buffer[3];

                    if (buffer[1] == 0x03)
                    {
                        UInt16 CRC16 = UtilHelper.CRC16Check(buffer, 0, 4);

                        if (CRC16 == (UInt16)(buffer[4] + (UInt16)(buffer[5] << 8)))
                        {
                            byte[] TxBuffer = new byte[6 + dataLen];
                            TxBuffer[0] = 0x01;
                            TxBuffer[1] = 0x03;
                            TxBuffer[2] = addr;
                            TxBuffer[3] = dataLen;

                            // 界面值,传给底层设备，先默认为0
                            for (int i = 0; i < dataLen; i++)
                            {
                                TxBuffer[4 + i] = 0;
                            }

                            // CRC16校验码
                            UInt16 TxCRC16 = UtilHelper.CRC16Check(TxBuffer.ToList(), 0, 5);
                            TxBuffer[4 + dataLen] = (byte)(TxCRC16 & 0x00FF);
                            TxBuffer[5 + dataLen] = (byte)((TxCRC16 & 0xFF00) / 256);

                            ShowMessage.Show(TxBuffer, TxBuffer.Length, "发送");
                            LogHelper.Info(typeof(SerialPortService), "发送报文：" + UtilHelper.ListToString(TxBuffer));

                            buffer.RemoveRange(0, 6);
                        }
                        else
                        {
                            buffer.RemoveAt(0);
                        }
                    }
                    else if (buffer[1] == 0x10)
                    {
                        if (buffer.Count < dataLen + 6)
                        {
                            return;
                        }

                        UInt16 CRC16 = UtilHelper.CRC16Check(buffer, 0, 4 + dataLen);
                        if (CRC16 == (UInt16)(buffer[4 + dataLen] + (UInt16)(buffer[5 + dataLen] << 8)))
                        {
                            byte[] TxBuffer = new byte[6];
                            TxBuffer[0] = 0x01;
                            TxBuffer[1] = 0x10;
                            TxBuffer[2] = addr;
                            TxBuffer[3] = dataLen;

                            // CRC16校验码
                            UInt16 TxCRC16 = UtilHelper.CRC16Check(TxBuffer.ToList(), 0, 4);
                            TxBuffer[4] = (byte)(TxCRC16 & 0x00FF);
                            TxBuffer[5] = (byte)((TxCRC16 & 0xFF00) / 256);

                            ShowMessage.Show(TxBuffer, TxBuffer.Length, "发送");
                            LogHelper.Info(typeof(SerialPortService), "发送报文：" + UtilHelper.ListToString(TxBuffer));

                            buffer.RemoveRange(0, 6 + dataLen);
                        }
                        else
                        {
                            buffer.RemoveAt(0);
                        }
                    }
                }
                else if (buffer[0] == 0x10)
                {
                    #region 短帧
                    byte sum = (byte)(buffer[1] + buffer[2] + buffer[3]);
                    if (sum == buffer[4] && buffer[5] == 0x16)
                    {
                        //接收到有效固定帧 处理数据
                        RxControlByte = buffer[1];//控制域 
                        RxTerminalAddress = (UInt16)(buffer[2] + (UInt16)buffer[3] << 8);
                        retransmitTimer.Enabled = false;
                        ShowMessage.Show(buffer, 6, "接收");
                        FixedFrameProcessing(buffer);
                        buffer.RemoveRange(0, 6);
                    }
                    else
                    {
                        buffer.RemoveAt(0);
                    }
                    #endregion 短帧
                }
                else if (buffer[0] == 0x68)
                {
                    # region 101长帧
                    if (buffer[0] == 0x68 && buffer[3] == 0x68 && buffer[1] == buffer[2])
                    {
                        if (buffer.Count < buffer[1] + 6)
                        {
                            return;
                        }

                        if (buffer[buffer[1] + 5] == 0x16)
                        {
                            int sum_cs = 0;
                            for (int i = 4; i < buffer[1] + 4; i++)
                            {
                                sum_cs += buffer[i];
                            }

                            if ((sum_cs % 256) == buffer[buffer[1] + 6 - 2])
                            {
                                //此时，校验完成，收到可变数据长度帧 开始处理数据
                                byte len = (byte)(buffer[1] + 6);//一个完整帧长度
                                RxControlByte = buffer[4];//控制域 
                                RxACD = (buffer[4] & 0x20) >> 5;
                                RxTerminalAddress = (UInt16)(buffer[5] + (UInt16)buffer[6] << 8);
                                RxTypeByte = buffer[7];//类型标识
                                RxVSQByte = buffer[8];

                                int offset = 0;
                                //判断COT长度
                                if (ConnectionParameters.sizeOfCOT == 1)
                                {
                                    RxCOTByte = buffer[9];
                                    offset = 9;
                                }
                                else
                                {
                                    RxCOTByte = (UInt16)(buffer[9] + (UInt16)buffer[10] << 8);
                                    offset = 10;
                                }

                                RxASDUAddress = (UInt16)(buffer[offset + 1] + (UInt16)buffer[offset + 2] << 8);
                                RxInfoObjAddress = (UInt16)(buffer[offset + 3] + (UInt16)buffer[offset + 4] << 8);

                                if (RxTypeByte == 203 && buffer[offset + 5] % 2 == 1)
                                {
                                    parameterContinueFlag = 1;
                                }

                                if (RxTypeByte == 202 && (RxCOTByte == 7 || RxCOTByte == 47) && buffer[offset + 5] % 2 == 1)
                                {
                                    parameterContinueFlag = 1;
                                }

                                retransmitTimer.Enabled = false;
                                ShowMessage.Show(buffer, len, "接收");
                                VariableFrameProcessing(buffer);
                            }
                            else
                            {
                                buffer.RemoveAt(0);
                            }
                        }
                        else
                        {
                            buffer.RemoveAt(0);
                        }
                    }
                    else
                    {
                        buffer.RemoveAt(0);
                    }
                    #endregion 101长帧
                }
                else if(buffer[0] == 0x11)
                {
                    #region 通道监视响应报文
                    byte sum = (byte)(buffer[1] + buffer[2] + buffer[3]);
                    if (sum == buffer[4] && buffer[5] == 0x66)
                    {
                        if (buffer[1] == 0x0B)
                        {
                            //通道监视标志位置1
                            System.Threading.Interlocked.Exchange(ref MainViewModel.ChannelMonitorListening, 1);
                            LogHelper.Info(typeof(Connection), "MainViewModel.ChannelMonitorListening:"
                                + System.Threading.Interlocked.Read(ref MainViewModel.ChannelMonitorListening));
                            MessageBox.Show("通道监视功能开启");
                            LogHelper.Info(typeof(ChannelMonitorViewModel), "开始通道监听");

                            //发送测试帧的定时器启动
                            CommunicationViewModel.testFrameTimerForChannelMonitor = new System.Timers.Timer(2000);
                            CommunicationViewModel.testFrameTimerForChannelMonitor.AutoReset = true;
                            CommunicationViewModel.testFrameTimerForChannelMonitor.Enabled = true;
                            CommunicationViewModel.testFrameTimerForChannelMonitor.Elapsed += new System.Timers.ElapsedEventHandler(TimerOut);
                            CommunicationViewModel.isTiming = true;
                        }
                        else if (buffer[1] == 0x0F)
                        {
                            MessageBox.Show("通道监视端口正与上位机连接中，请选择正确的监视端口");
                        }
                        else
                        {
                            MessageBox.Show("通道监视回复未知功能码：" + buffer[1] + " " + buffer[2] + " " + buffer[3] + " " + buffer[4] + " " + buffer[5]);
                        }
                        // 删除已处理的帧
                        buffer.RemoveRange(0, 6);
                    }
                    else
                    {
                        // 报文校验和不对
                        buffer.RemoveAt(0);
                    }
                    #endregion 通道监视响应报文
                }
                else
                {
                    buffer.RemoveAt(0);
                }
            }
        }
        /// <summary>
        /// 处理短帧数据
        /// </summary>
        /// <param name="buffer"></param>
        public void FixedFrameProcessing(List<byte> buffer)
        {
            #region   短帧 平衡方式
            if (CommunicationViewModel.selectedIndexProtocol == 0)//平衡方式
            {
                if (RxControlByte == 0x8b)//响应链路状态
                {
                    SendFixedFrame(0x40);
                    retransmitTimer.Enabled = true;

                    return;
                }
                if (RxControlByte == 0x80)//确认
                {
                    //写文件使能
                    if (FileServiceViewModel.SendFileDataRunning == true)
                    {
                        FileServiceViewModel.sendfileContinue = true;
                    }
                    return;
                }
                if (RxControlByte == 0xc9)//终端召唤链路
                {
                    SendFixedFrame(0x0b);
                    retransmitTimer.Enabled = true;

                    return;
                }
                if (RxControlByte == 0xC0)
                {
                    SendFixedFrame(0x0);//主站确认复位远方链路
                    retransmitTimer.Enabled = true;

                    return;
                }
            }
            #endregion
            #region   短帧 非平衡方式
            else if (CommunicationViewModel.selectedIndexProtocol == 1)//非平衡方式
            {

                if (RxControlByte == 0x00)//肯定认可
                {
                    //ShowMessage.ShowFunction("确认");
                    FCB = (byte)((FCB == 1) ? 0 : 1);
                    SendFixedFrame((byte)((FCB << 5) | 0x5b));
                    retransmitTimer.Enabled = true;
                    //写文件使能
                    if (FileServiceViewModel.SendFileDataRunning == true)
                    {
                        FileServiceViewModel.sendfileContinue = true;
                    }

                    return;
                }
                if (RxControlByte == 0x01)//否定认可
                {

                    FCB = (byte)((FCB == 1) ? 0 : 1);
                    SendFixedFrame((byte)((FCB << 5) | 0x5b)); ;
                    retransmitTimer.Enabled = true;
                    return;
                }
                if (RxControlByte == 0x08)//响应用户数据
                {

                    SendFixedFrame(0x0b);
                    retransmitTimer.Enabled = true;

                    return;
                }
                if (RxControlByte == 0x09)//无请求的用户数据
                {

                    askSecondDataTimer.Enabled = true;
                    return;
                }
                if (RxControlByte == 0x0b)//响应： 链路状态 
                {

                    SendFixedFrame(0x40);//复位链路  
                    askSecondDataTimer.Enabled = true;

                    return;
                }
                if ((RxControlByte & 0x20) == 0x20)
                {

                    FCB = (byte)((FCB == 1) ? 0 : 1);
                    SendFixedFrame((byte)((FCB << 5) | 0x5a));//召唤一级数据
                    askSecondDataTimer.Enabled = true;

                    return;
                }
            }
        }
            #endregion
        /// <summary>
        /// 处理长帧数据
        /// </summary>
        /// <param name="buffer"></param>
        public void VariableFrameProcessing(List<byte> buffer)
        {
            byte[] buf = new byte[buffer.Count];
            for (int i = 0; i < buffer.Count; i++)
            {
                buf[i] = buffer[i];
            }
            //缓存中清除Len个字节
            byte len = (byte)(buffer[1] + 6);
            SerialPortService.ReceivceBuffer.RemoveRange(0, len);

            #region   长帧 平衡方式

            ConnectionParameters parameters = new ConnectionParameters(2);

            ASDU asdu = new ASDU(parameters, buf, 7, buf.Length - 2);
            //CommunicationViewModel.con.DebugLog("ASDU错误！！");

            SetASDUReceivedHandler(IEC104.asduReceivedHandler, null);
            if (CommunicationViewModel.selectedIndexProtocol == 0)//平衡方式
            {
                SendFixedFrame(0x0);//长帧确认帧
                switch (RxControlByte & 0x0f)
                {
                    case 0x00://复位远方链路
                        break;
                    case 0x02:// 发送/确认 链路测试功能 
                        break;
                    case 0x03://发送/确认用户数据

                        if (asduReceivedHandler != null)
                            asduReceivedHandler(asduReceivedHandlerParameter, asdu);
                        break;

                    case 0x04://发送/无回答用户数据
                        break;
                    case 0x09://请求/响应 请求链路状态
                        break;
                }
                // SendFixedFrame(0x0);//长帧确认帧
                // //ShowMessage.ShowFunction("确认");
            }
            #endregion
            #region 长帧 非平衡方式
            else if (CommunicationViewModel.selectedIndexProtocol == 1)//非平衡方式
            {
                switch (RxControlByte & 0x0f)
                {
                    case 0x00://确认：认可
                        break;
                    case 0x01://确认：否定认可
                        break;
                    case 0x08://响应：用户数据

                        //if (RxACD == 1)
                        //{
                        //    CmdAskFirstData();
                        //}
                        //else if (RxACD == 0)
                        //{
                        //    CmdAskSecondData();
                        //}
                        if (asduReceivedHandler != null)
                            asduReceivedHandler(asduReceivedHandlerParameter, asdu);

                        askSecondDataTimer.Enabled = true;
                        break;
                    case 0x09://响应：无所请求的用户数据。
                        break;
                    case 0x0b://响应：链路状态
                        break;

                }
            }
            #endregion
        }


        /// <summary>
        /// 发送固定帧
        /// </summary>
        public void SendFixedFrame(byte TxControlByte)
        {
            byte[] TxBuffer = new byte[6];
            byte txlen;
            TxBuffer[0] = 0x10;
            TxBuffer[1] = TxControlByte;
            TxBuffer[2] = (byte)CommunicationViewModel.deviceAddress;
            TxBuffer[3] = (byte)(CommunicationViewModel.deviceAddress >> 8);
            //TxBuffer[2] = (byte)TerminalAddress;这就是导致用户不管输入什么设备地址，报文中下发的地址都是1的原因
            //TxBuffer[3] = (byte)(TerminalAddress >> 8);

            TxBuffer[4] = (byte)(TxBuffer[1] + TxBuffer[2] + TxBuffer[3]);
            TxBuffer[5] = 0x16;
            txlen = 6;
            serialPort.Write(TxBuffer, 0, txlen);
            if (retransmitEnable == true && testModel == false)
            {
                TxTemp = new byte[6];
                for (int j = 0; j < 6; j++)
                {
                    TxTemp[j] = TxBuffer[j];
                }                

            }
            retransmitCount = 0;
            ShowMessage.Show(TxBuffer, txlen, "发送");
            LogHelper.Info(typeof(SerialPortService), "发送报文：" + UtilHelper.ListToString(TxBuffer));

        }

        public void SendVariableFrame(ASDU asdu)
        {
            BufferFrame frame = new BufferFrame(new byte[260], 7);
            asdu.Encode(frame, new ConnectionParameters(2));

            byte[] buffer = frame.GetBuffer();
            //此处有错误
            //int msgSize = frame.GetMsgSize () + 6; /* ASDU size + ACPI size */
            int msgSize = frame.GetMsgSize() + 2;

            buffer[0] = 0x68;

            /* set size field */
            buffer[1] = (byte)(msgSize - 6);
            buffer[2] = (byte)(msgSize - 6);

            buffer[3] = (byte)0x68;
            FCB = FCB == 1 ? (byte)0 : (byte)1;
            buffer[4] = (byte)(FCB << 5 | 0x03 | 0x50);//控制域

            buffer[5] = (byte)(byte)CommunicationViewModel.deviceAddress;
            buffer[6] = (byte)(byte)(CommunicationViewModel.deviceAddress >> 8);
            for (int j = 0; j < msgSize - 6; j++)
            {
                buffer[msgSize - 2] += buffer[4 + j];
            }
            buffer[msgSize - 1] = 0x16;
            try
            {
                serialPort.Write(buffer, 0, msgSize);

                if (retransmitEnable == true && testModel == false)
                {
                    TxTemp = new byte[msgSize];
                    for (int j = 0; j < msgSize; j++)
                    {
                        TxTemp[j] = buffer[j];
                    }
                    retransmitTimer.Enabled = true;

                }
                retransmitCount = 0;

                ShowMessage.Show(buffer, msgSize, "发送");
                LogHelper.Info(typeof(SerialPortService), "发送报文：" + UtilHelper.ListToString(buffer));

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                Console.WriteLine(e.ToString());
                LogHelper.Info(typeof(SerialPortService), "异常：" + e.ToString());
            }
        }

        /// <summary>
        /// 召唤一级数据
        /// </summary>
        public void CmdAskFirstData()
        {
            FCB = FCB == 1 ? (byte)0 : (byte)1;
            SendFixedFrame((byte)(FCB << 5 | 0x0a | 0x50));
        }

        /// <summary>
        /// 召唤二级数据
        /// </summary>
        public void CmdAskSecondData()
        {
            FCB = FCB == 1 ? (byte)0 : (byte)1;
            SendFixedFrame((byte)(FCB << 5 | 0x0b | 0x50));
        }

        /// <summary>
        /// 心跳测试报文
        /// </summary>
        public void CmdHeartbeatData()
        {
            FCB = FCB == 1 ? (byte)0 : (byte)1;
            SendFixedFrame((byte)(FCB << 5 | 0x02 | 0x50));
        }

        /***************************************************************************************************************************/
        /***************************************************监视功能相关程序部分*****************************************************/
        /****************************************************************************************************************************/
        #region 监视功能相关程序部分
        /// <summary>
        /// 解析串口收取到的101报文字节集合
        /// </summary>
        /// <param name="buffer">串口收取到的字节集合</param>
        public void CheckLinkDataForChannelMonitor(ref List<byte> buffer)
        {
            LogHelper.Info(typeof(Connection), "开始解析通道监视收到的报文:" + BitConverter.ToString(buffer.ToArray()).Replace("-", string.Empty));
            // 串口收到的字节数至少为6个字节才处理（短帧为6个字节）
            while (buffer.Count > 5)
            {
                LogHelper.Info(typeof(Connection), "接收到的字节数可构成101最小完整报文");
                // 查找buffer中的 AA 55 或者68 或者 10 （有效报文的起始字节）
                for (int i = 0; i < buffer.Count; ++i)
                {
                    // 找到AA
                    if (buffer[i] == firstSendByte)
                    {
                        LogHelper.Info(typeof(Connection), "找到AA");
                        // AA下一个字节存在
                        if (i + 1 < buffer.Count)
                        {
                            LogHelper.Info(typeof(Connection), "AA下一个字节存在");
                            // AA下一个字节是55
                            if (buffer[i + 1] == secondSendByte)
                            {
                                LogHelper.Info(typeof(Connection), "AA下一个字节是55");
                                // AA 55前面有未知报文
                                if (i > 0)
                                {
                                    LogHelper.Info(typeof(Connection), "AA 55前面有未知报文");
                                    // 处理未知报文
                                    DealUnkownFrame(ref buffer, i);
                                    // 重新开始循环判定剩余的字节
                                    break;
                                }
                                else
                                {
                                    // AA 55前面没有未知报文,即为起始字节
                                    // AA 55后面为68
                                    if (buffer[2] == 0x68)
                                    {
                                        LogHelper.Info(typeof(Connection), "AA 55后面为68");
                                        //101长帧报文报文头正确
                                        if (buffer[2] == 0x68 && buffer[5] == 0x68 && buffer[3] == buffer[4])
                                        {
                                            LogHelper.Info(typeof(Connection), "AA 55 101长帧报文报文头正确");
                                            //报文没收完整
                                            if (buffer.Count < buffer[3] + 8)
                                            {
                                                LogHelper.Error(typeof(Connection), "通道监视网口收到不完整的报文");
                                                return;
                                            }
                                            //报文结束符正确
                                            if (buffer[buffer[3] + 7] == 0x16)
                                            {
                                                LogHelper.Info(typeof(Connection), "报文结束符正确");
                                                int sum_cs = 0;
                                                for (int j = 6; j < buffer[3] + 6; j++)
                                                {
                                                    sum_cs += buffer[j];
                                                }

                                                if ((sum_cs % 256) == buffer[buffer[3] + 6])
                                                {
                                                    // 此时，校验完成，收到可变数据长度帧 开始处理数据
                                                    // 从buffer中将AA55删除
                                                    DealAA55(ref buffer);
                                                    List<byte> oneLongFrame = new List<byte>();
                                                    for (int k = 0; k < buffer[1] + 6; ++k)
                                                    {
                                                        oneLongFrame.Add(buffer[k]);                                                     
                                                    }
                                                    LogHelper.Info(typeof(Connection), "处理正确的长帧报文");
                                                    // 处理正确的长帧报文
                                                    VariableFrameProcessingForChannelMonitor(oneLongFrame);
                                                    buffer.RemoveRange(0, buffer[1] + 6);
                                                    // 改动处
                                                    break;
                                                }
                                                else
                                                {
                                                    LogHelper.Info(typeof(Connection), "校验和错误");
                                                    // 校验和错误
                                                    DealUnkownFrame(ref buffer, buffer[3] + 8);
                                                }
                                            }
                                            else
                                            {
                                                LogHelper.Info(typeof(Connection), "结束符不正确");
                                                // 结束符不正确
                                                DealUnkownFrame(ref buffer, buffer[3] + 8);
                                            }
                                        }
                                        else
                                        {
                                            LogHelper.Info(typeof(Connection), "101长帧报文头不正确,将AA 55连同报文头一起加入错误报文List");
                                            // 101长帧报文头不正确,将AA 55连同报文头一起加入错误报文List
                                            DealUnkownFrame(ref buffer, 6);
                                        }

                                    }
                                    else if (buffer[2] == 0x10)
                                    {
                                        LogHelper.Info(typeof(Connection), "AA55短帧");
                                        // 短帧未收取完整
                                        if (buffer.Count < 9)
                                        {
                                            LogHelper.Info(typeof(Connection), "AA55短帧未收取完整");
                                            return;
                                        }
                                        //101短帧报文校验和
                                        byte sum = (byte)(buffer[3] + buffer[4] + buffer[5]);
                                        // 校验和正确并且结束符正确
                                        if (sum == buffer[6] && buffer[7] == 0x16)
                                        {
                                            LogHelper.Info(typeof(Connection), "AA55101短帧报文校验和正确并且结束符正确");
                                            // 从buffer中将AA55删除
                                            DealAA55(ref buffer);
                                            //复制一帧有效的101短帧报文到oneFrame中
                                            List<byte> oneShortFrame = new List<byte>();
                                            for (int n = 0; n < 6; ++n)
                                            {
                                                oneShortFrame.Add(buffer[n]);
                                            }
                                            LogHelper.Info(typeof(Connection), "处理有效报文帧");
                                            //处理有效报文帧
                                            FixedFrameProcessingForChannelMonitor(oneShortFrame);
                                            buffer.RemoveRange(0, 6);
                                            break;
                                        }
                                        else
                                        {
                                            LogHelper.Info(typeof(Connection), "无效短帧报文");
                                            //无效短帧报文
                                            DealUnkownFrame(ref buffer, 8);
                                        }
                                    }
                                    else
                                    {
                                        LogHelper.Info(typeof(Connection), "AA 55后面既不是68 也不是10 ");
                                        // AA 55后面既不是68 也不是10 
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                LogHelper.Info(typeof(Connection), "AA下一个字节不是55,忽略这个AA，继续寻找下一个AA");
                                // AA下一个字节不是55,忽略这个AA，继续寻找下一个AA
                                continue;
                            }
                        }
                        else
                        {
                            LogHelper.Info(typeof(Connection), "AA下一个字节不存在，返回继续等待收取字节");
                            // AA下一个字节不存在，返回继续等待收取字节
                            return;
                        }

                    }
                    else if (buffer[i] == 0x68)
                    {
                        LogHelper.Info(typeof(Connection), "buffer[i] == 0x68");
                        // 68前方存在无效报文
                        if (i > 0)
                        {
                            LogHelper.Info(typeof(Connection), "68前方存在无效报文");
                            // 处理前方的无效报文
                            DealUnkownFrame(ref buffer, i);
                            // 重新开始循环判定剩余的字节
                            break;
                        }

                        // 以68开始的报文处理
                        if (buffer[0] == 0x68 && buffer[3] == 0x68 && buffer[1] == buffer[2])
                        {
                            LogHelper.Info(typeof(Connection), "以68开始的报文处理");
                            //报文没收完整
                            if (buffer.Count < buffer[1] + 6)
                            {
                                LogHelper.Info(typeof(Connection), "报文没收完整");
                                return;
                            }
                            //报文结束符正确
                            if (buffer[buffer[1] + 5] == 0x16)
                            {
                                LogHelper.Info(typeof(Connection), "报文结束符正确");
                                int sum_cs = 0;
                                for (int j = 4; j < buffer[1] + 4; j++)
                                {
                                    sum_cs += buffer[j];
                                }
                                if ((sum_cs % 256) == buffer[buffer[1] + 4])
                                {
                                    LogHelper.Info(typeof(Connection), "此时，校验完成，收到可变数据长度帧 开始处理数据");
                                    // 此时，校验完成，收到可变数据长度帧 开始处理数据
                                    List<byte> oneLongFrame = new List<byte>();
                                    for (int k = 0; k < buffer[1] + 6; ++k)
                                    {
                                        oneLongFrame.Add(buffer[k]);                                        
                                    }
                                    LogHelper.Info(typeof(Connection), "处理正确的长帧报文");
                                    // 处理正确的长帧报文
                                    DirForChannelMonitor = "接收";
                                    VariableFrameProcessingForChannelMonitor(oneLongFrame);
                                    buffer.RemoveRange(0, buffer[1] + 6);
                                    break;
                                }
                            }
                            else
                            {
                                LogHelper.Info(typeof(Connection), "校验和不对");
                                // 校验和不对
                                DealUnkownFrame(ref buffer, buffer[1] + 6);
                                break;
                            }
                        }
                        else
                        {
                            LogHelper.Info(typeof(Connection), "报文头不对");
                            //报文头不对
                            DealUnkownFrame(ref buffer, 4);
                            break;
                        }
                    }
                    else if (buffer[i] == 0x10)
                    {
                        LogHelper.Info(typeof(Connection), "buffer[i] == 0x10");
                        // 10前方存在无效报文
                        if (i > 0)
                        {
                            LogHelper.Info(typeof(Connection), "处理前方的无效报文");
                            // 处理前方的无效报文
                            DealUnkownFrame(ref buffer, i);
                            // 重新开始循环判定剩余的字节
                            break;
                        }
                        //101短帧报文校验和
                        byte sum = (byte)(buffer[1] + buffer[2] + buffer[3]);
                        // 校验和正确并且结束符正确
                        if (sum == buffer[4] && buffer[5] == 0x16)
                        {
                            LogHelper.Info(typeof(Connection), "校验和正确并且结束符正确");
                            //复制一帧有效的101短帧报文到oneFrame中
                            List<byte> oneShortFrame = new List<byte>();
                            for (int n = 0; n < 6; ++n)
                            {
                                oneShortFrame.Add(buffer[n]);
                            }
                            LogHelper.Info(typeof(Connection), "处理有效报文帧");
                            //处理有效报文帧
                            DirForChannelMonitor = "接收";
                            FixedFrameProcessingForChannelMonitor(oneShortFrame);
                            buffer.RemoveRange(0, 6);
                            break;
                        }
                        else
                        {
                            LogHelper.Info(typeof(Connection), "无效短帧报文");
                            //无效短帧报文
                            DealUnkownFrame(ref buffer, 6);
                            break;
                        }
                    }
                    else if (buffer[i] == 0x11)
                    {
                        // 11前方存在无效报文
                        if (i > 0)
                        {
                            LogHelper.Info(typeof(Connection), "11前方存在无效报文");
                            // 处理前方的无效报文
                            DealUnkownFrame(ref buffer, i);
                            // 重新开始循环判定剩余的字节
                            break;
                        }
                        // FTU回复的启动监听或者停止监听的报文
                        byte sum = (byte)(buffer[1] + buffer[2] + buffer[3]);
                        if (sum == buffer[4] && buffer[5] == 0x66)
                        {
                            LogHelper.Info(typeof(Connection), "FTU回复的启动监听或者停止监听的报文");
                            //校验和正确，有效通道监视自定义协议中的回复报文
                            //if (buffer[1] == 0x0B)
                            //{
                            //    MessageBox.Show("通道监视功能开启");
                            //}
                            //else if (buffer[1] == 0x0F)
                            //{
                            //    MessageBox.Show("通道监视端口正与上位机连接中，请选择正确的监视端口");
                            //    System.Threading.Interlocked.Exchange(ref MainViewModel.ChannelMonitorListening, 0);
                            //}
                            if (buffer[1] == 0x02)
                            {
                                LogHelper.Info(typeof(Connection), "FTU响应测试帧");
                                // FTU响应测试帧
                                //CommunicationViewModel.isReceiveTestFrameResponse = true;
                            }
                            if (buffer[1] == 0x01)
                            {
                                LogHelper.Info(typeof(Connection), "FTU响应测试帧");
                            }
                            else if (buffer[1] == 0x0C)
                            {
                                LogHelper.Info(typeof(Connection), "停止监听帧回复");
                                //停止监听帧回复
                                //通道监视标志置0,停止测试帧的计时器
                                CommunicationViewModel.testFrameTimerForChannelMonitor.Stop();
                                System.Threading.Interlocked.Exchange(ref MainViewModel.ChannelMonitorListening, 0);
                                LogHelper.Info(typeof(ChannelMonitorViewModel), "停止通道监听");
                                Messenger.Default.Send<string>("closeChannelMonitorView", "CloseChannelMonitorView");

                            }
                            else
                            {
                                LogHelper.Info(typeof(Connection), "未知通道监视回复帧");
                                // FTU发送过来的未知通道监视回复帧
                                MessageBox.Show("未知通道监视回复帧");
                            }
                            LogHelper.Info(typeof(Connection), "删除已经处理的通道监视回复帧");
                            // 删除已经处理的通道监视回复帧
                            buffer.RemoveRange(0, 6);
                            break;
                        }
                        else
                        {
                            LogHelper.Info(typeof(SerialPortService), "校验和错误，删除字节");
                            //校验和错误，删除字节
                            DealUnkownFrame(ref buffer, 6);
                            break;
                        }
                    }
                    else
                    {
                        // 非有效起始报文，即非 AA 68 10 11，将该字节存入未知报文中，并从待解析报文中删除
                        DealUnkownFrame(ref buffer, 1);
                        // 返回while循环，重新解析剩余报文
                        break;
                    }
                }// for
            }// while
            // 接收到的字节数不可构成最小101完整报文
            LogHelper.Info(typeof(Connection), "网络监听报文长度不满足最小要求，count:" + buffer.Count);
        }

        /// <summary>
        /// 链路测试计时时间到
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void TimerOut(object source, System.Timers.ElapsedEventArgs e)
        {
            // 判断是否有接收到测试帧的回复报文
            if (CommunicationViewModel.isReceiveFrameResponse)
            {
                CommunicationViewModel.isReceiveFrameResponse = false;
                Messenger.Default.Send<string>("testFrameTimeOut", "TestFrameTimeOut");
            }
            else if (!CommunicationViewModel.isReceiveFrameResponse)
            {
                CommunicationViewModel.unrecieveFrameTime = CommunicationViewModel.unrecieveFrameTime + 1;
                // 6秒未收到任何报文
                if (CommunicationViewModel.unrecieveFrameTime == 3)
                {
                    LinkDisconnectOnChannelMonitor();
                }
            }
        }

        /// <summary>
        /// 通道监视过程中串口链路断开
        /// </summary>
        private void LinkDisconnectOnChannelMonitor()
        {
            // 未收到任何报文，关闭计时器
            CommunicationViewModel.testFrameTimerForChannelMonitor.Stop();
            // 关闭串口
            CommunicationViewModel.CloseSerialPortConnect();
            // 提示用户
            MessageBox.Show("通道监视链路连接超时，请关闭通道监视窗口", "警告");
        }
        /// <summary>
        /// 处理未知报文
        /// </summary>
        /// <param name="buffer">待处理的字节集合</param>
        /// <param name="num">未知报文的长度</param>
        public static void DealUnkownFrame(ref List<byte> buffer, int num)
        {
            int i;
            for (i = 0; i < num; ++i)
            {
                UnknownFrame.Add(buffer[i]);
            }
            // 删除buffer中的未知报文
            buffer.RemoveRange(0, num);
        }

        /// <summary>
        /// 处理AA55两个字节
        /// </summary>
        /// <param name="buffer">待处理的字节集合</param>
        public static void DealAA55(ref List<byte> buffer)
        {
            DirForChannelMonitor = "发送";
            buffer.RemoveRange(0, 2);
        }

        /// <summary>
        /// 解析通道监视的长帧报文
        /// </summary>
        /// <param name="buffer"></param>
        public static void FixedFrameProcessingForChannelMonitor(List<byte> buffer)
        {
            // 控制域
            byte controlByte = buffer[1];
            Action<string> send = (string str1) => { ShowMessage.ShowForChannelMonitor(buffer, 6, DirForChannelMonitor, "请求链路", "FC = " + controlByte.ToString("X")); };
            #region   短帧 平衡方式
            if (controlByte == 0xC9)//请求链路状态
            {
                send("请求链路");
                return;
            }
            if (controlByte == 0x0B)//响应链路状态
            {
                ShowMessage.ShowForChannelMonitor(buffer, 6, DirForChannelMonitor, "响应链路", "FC = 0x0B");
                return;
            }
            if (controlByte == 0xC0)//复位远方链路
            {
                ShowMessage.ShowForChannelMonitor(buffer, 6, DirForChannelMonitor, "复位链路", "FC = 0xC0");
                return;
            }
            if (controlByte == 0x00)//肯定确认
            {
                ShowMessage.ShowForChannelMonitor(buffer, 6, DirForChannelMonitor, "肯定确认", "FC = 0x00");
                return;
            }
            if (controlByte == 0x49)//请求链路状态
            {
                ShowMessage.ShowForChannelMonitor(buffer, 6, DirForChannelMonitor, "请求链路状态", "FC = 0x49");
                return;
            }
            if (controlByte == 0x8B)//响应链路状态
            {
                ShowMessage.ShowForChannelMonitor(buffer, 6, DirForChannelMonitor, "响应链路状态", "FC = 0x8B");
                return;
            }
            if (controlByte == 0x40)//复位远方链路
            {
                ShowMessage.ShowForChannelMonitor(buffer, 6, DirForChannelMonitor, "复位远方链路", "FC = 0x40");
                return;
            }
            if (controlByte == 0x80)//肯定确认
            {
                ShowMessage.ShowForChannelMonitor(buffer, 6, DirForChannelMonitor, "肯定确认", "FC = 0x80");
                return;
            }
            #endregion

            #region   短帧 非平衡方式

            if (controlByte == 0x01)//否定认可
            {
                ShowMessage.ShowForChannelMonitor(buffer, 6, DirForChannelMonitor, "非平衡否定认可", "FC = 0x01");
                return;
            }
            if (controlByte == 0x08)//响应用户数据
            {
                ShowMessage.ShowForChannelMonitor(buffer, 6, DirForChannelMonitor, "非平衡响应用户数据", "FC = 0x08");
                return;
            }
            if (controlByte == 0x09)//无请求的用户数据
            {
                ShowMessage.ShowForChannelMonitor(buffer, 6, DirForChannelMonitor, "非平衡无请求的用户数据", "FC = 0x09");
                return;
            }
        }
            #endregion
        
        /// <summary>
        /// 处理101协议的变长帧
        /// </summary>
        /// <param name="buffer"></param>
        public static void VariableFrameProcessingForChannelMonitor(List<byte> buffer)
        {
            byte[] buf = buffer.ToArray();
            ConnectionParameters parameters = new ConnectionParameters(2);
            ASDU asdu = new ASDU(parameters, buf, 7, buf.Length - 2);
            if (!UnknownFrame.Count.Equals(0))
            {
                // 显示未知报文
                ShowMessage.ShowForChannelMonitor(UnknownFrame, UnknownFrame.Count, "未知", "未知报文", "");
                // 清除所有已显示的未知报文
                UnknownFrame.Clear();
            }
            //判断COT长度
            int Cot;
            if (ConnectionParameters.sizeOfCOT == 1)
            {
                Cot = buffer[9];
            }
            else
            {
                UInt16 tmp10 = (UInt16)(buffer[10] << 8);
                UInt16 tmp9 = buffer[9];
                Cot = tmp10 + tmp9;
                //Cot = (UInt16)(buffer[9] + (UInt16)buffer[10] << 8);错误的计算方式
            }
            int len = buffer.Count;
            int typeID = (int)buffer[7];
            try
            {
                switch (typeID)//TypeId
                {
                    case (int)TypeID.M_SP_NA_1:
                        if (Cot == (int)CauseOfTransmission.BACKGROUND_SCAN)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "单点信息", "背景扫描");
                        }
                        else if (Cot == (int)CauseOfTransmission.SPONTANEOUS)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "单点信息", "自发");
                        }
                        else if (Cot == (int)CauseOfTransmission.REQUEST)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "单点信息", "被请求");
                        }
                        else if (Cot == (int)CauseOfTransmission.INTERROGATED_BY_STATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "单点信息", "响应站召唤");
                        }
                        break;

                    case (int)TypeID.M_DP_NA_1:
                        if (Cot == (int)CauseOfTransmission.BACKGROUND_SCAN)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "双点信息", "背景扫描");
                        }
                        else if (Cot == (int)CauseOfTransmission.SPONTANEOUS)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "双点信息", "自发");
                        }
                        else if (Cot == (int)CauseOfTransmission.REQUEST)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "双点信息", "被请求");
                        }
                        else if (Cot == (int)CauseOfTransmission.INTERROGATED_BY_STATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "双点信息", "响应站召唤");
                        }
                        break;

                    case (int)TypeID.M_ME_NA_1:
                        string normalizedMsg = "";
                        for (int i = 0; i < asdu.NumberOfElements; i++)
                        {
                            var msv = (MeasuredValueNormalized)asdu.GetElement(i);
                            Telemetering telemetering = TelemeteringViewModel.telemeteringData[msv.ObjectAddress - 0x4001];
                            telemetering.Rate = telemetering.Rate <= 1 ? 1 : telemetering.Rate;
                            normalizedMsg += "  点号: " + msv.ObjectAddress.ToString("X") + " " + telemetering.Name + " " + (msv.NormalizedValue * telemetering.Rate);
                        }
                        ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "测量值，归一化值", normalizedMsg);
                        break;

                    case (int)TypeID.M_ME_NB_1:
                        string scalingMsg = "";
                        for (int i = 0; i < asdu.NumberOfElements; i++)
                        {
                            var msv = (MeasuredValueScaled)asdu.GetElement(i);
                            Telemetering telemetering = TelemeteringViewModel.telemeteringData[msv.ObjectAddress - 0x4001];
                            telemetering.Rate = telemetering.Rate <= 1 ? 1 : telemetering.Rate;
                            scalingMsg += "  点号: " + msv.ObjectAddress.ToString("X") + " " + telemetering.Name + " " + (msv.ScaledValue.ShortValue / telemetering.Rate);
                        }
                        ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "测量值，标度化值", scalingMsg);
                        break;

                    case (int)TypeID.M_ME_NC_1:
                        string shortFloatMsg = "";
                        for (int i = 0; i < asdu.NumberOfElements; i++)
                        {
                            var mfv = (MeasuredValueShort)asdu.GetElement(i);
                            Telemetering telemetering = TelemeteringViewModel.telemeteringData[mfv.ObjectAddress - 0x4001];
                            shortFloatMsg += "  点号: " + mfv.ObjectAddress.ToString("X") + " " + telemetering.Name + " " + mfv.Value;
                        }
                        ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "测量值，短浮点数", shortFloatMsg);
                        break;

                    case (int)TypeID.M_ME_ND_1:
                        ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "测量值，不带品质描述的归一化值", "");
                        break;

                    case (int)TypeID.M_SP_TB_1:
                        ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "带CP56Time2a时标的单点信息", "");
                        break;

                    case (int)TypeID.M_DP_TB_1:
                        ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "带CP56Time2a时标的双点信息", "");
                        break;

                    case (int)TypeID.M_ME_TE_1:
                        ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "带CP56Time2a时标的测量值，标度化值", "");
                        break;

                    case (int)TypeID.M_ME_TF_1:
                        ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "带CP56Time2a时标的测量值，短浮点数", "");
                        break;

                    case (int)TypeID.M_FT_NA_1:
                        if (Cot == (int)CauseOfTransmission.SPONTANEOUS)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "故障值信息", "突发（自发）");
                        }
                        break;

                    case (int)TypeID.C_SC_NA_1:
                        if (Cot == (int)CauseOfTransmission.ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "遥控单点命令激活确认", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_TERMINATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "遥控单点命令激活终止", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "遥控单点命令激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.DEACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "遥控单点命令停止激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.DEACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "遥控单点命令停止激活确认", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.TELECONTROL_FAIL)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "遥控单点命令失败", "");
                        }
                        break;

                    case (int)TypeID.C_DC_NA_1:
                        if (Cot == (int)CauseOfTransmission.ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "遥控双点命令激活确认", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_TERMINATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "遥控双点命令激活终止", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "遥控双点命令激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.DEACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "遥控双点命令停止激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.DEACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "遥控双点命令停止激活确认", "");
                        }
                        else
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "遥控命令", "未知传输原因：" + Cot);
                        }
                        break;

                    case (int)TypeID.M_EI_NA_1:
                        if (Cot == (int)CauseOfTransmission.INITIALIZED)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "初始化结束", "");
                        }
                        break;

                    case (int)TypeID.C_IC_NA_1:
                        if (Cot == (int)CauseOfTransmission.ACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "总召唤激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "总召唤确认", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.DEACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "总召唤停止激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.DEACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "总召唤停止激活确认", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_TERMINATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "总召唤结束", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.INTERROGATED_BY_STATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "响应总召唤", "");
                        }
                        else
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "响应总召唤", "未知传输原因：" + Cot);
                        }
                        break;

                    case (int)TypeID.C_CS_NA_1:
                        if (Cot == (int)CauseOfTransmission.REQUEST)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "时钟读取请求", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "时钟同步激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "时钟同步激活确认", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_TERMINATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "时钟同步激活终止", "");
                        }
                        else
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "时钟同步", "未知传输原因：" + Cot);
                        }
                        break;

                    case (int)TypeID.F_FR_NA_1:
                        if (Cot == (int)CauseOfTransmission.ACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "文件传输", "激活");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "文件传输", "激活确认");
                        }
                        else if (Cot == (int)CauseOfTransmission.REQUEST)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "文件传输", "请求/被请求");
                        }
                        break;

                    case (int)TypeID.C_RP_NA_1:
                        if (Cot == (int)CauseOfTransmission.ACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "复位进程激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "复位进程激活确认", "");
                        }
                        else
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "复位进程", "未知传输原因：" + Cot);
                        }
                        break;

                    case (int)TypeID.C_TS_NA_1:
                        if (Cot == (int)CauseOfTransmission.ACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "测试命令激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "测试命令激活确认", "");
                        }
                        else
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "测试命令", "未知传输原因：" + Cot);
                        }
                        break;

                    case (int)TypeID.C_CI_NA_1:
                        if (Cot == (int)CauseOfTransmission.ACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "电能量召唤", "电能量召唤激活");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "电能量召唤", "电能量召唤激活确认");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_TERMINATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "电能量召唤", "电能量召唤激活终止");
                        }
                        else
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "电能量召唤", "未知传送原因：" + Cot);
                        }
                        break;

                    case (int)TypeID.M_IT_NB_1:
                        if (Cot == (int)CauseOfTransmission.SPONTANEOUS)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "电能量数据", "突发（自发）");
                        }
                        else if (Cot == (int)CauseOfTransmission.REQUESTED_BY_GENERAL_COUNTER)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "电能量数据", "响应电能量总召唤");
                        }
                        break;

                    case (int)TypeID.C_SR_NA_1:
                        if (Cot == (int)CauseOfTransmission.ACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "切换定值区激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "切换定值区激活确认", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.CHANGE_FIXED_AREA_ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "切换定值区激活终止", "");
                        }
                        break;

                    case (int)TypeID.C_RR_NA_1:
                        if (Cot == (int)CauseOfTransmission.ACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "读当前定值区号激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "读当前定值区号激活确认", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.CHANGE_FIXED_AREA_ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "读当前定值区号激活终止", "");
                        }
                        break;

                    case (int)TypeID.C_RS_NA_1:
                        if (Cot == (int)CauseOfTransmission.ACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "读多个/全部参数和定值激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "读多个/全部参数和定值激活确认", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.CHANGE_FIXED_AREA_ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "读多个/全部参数和定值激活终止", "");
                        }
                        break;

                    case (int)TypeID.C_WS_NA_1:
                        if (Cot == (int)CauseOfTransmission.ACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "写多个参数和定值激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "写多个参数和定值激活确认", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.DEACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "写多个参数和定值停止激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.DEACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "写多个参数和定值停止激活确认", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_TERMINATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "写多个参数和定值激活终止", "");
                        }
                        else
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "写多个参数和定值", "未知传输原因：" + Cot);
                        }
                        break;

                    case (int)TypeID.F_SR_NA_1:
                        if (Cot == (int)CauseOfTransmission.ACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "软件升级激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "软件升级激活确认", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.DEACTIVATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "软件升级停止激活", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.DEACTIVATION_CON)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "软件升级停止激活确认", "");
                        }
                        else if (Cot == (int)CauseOfTransmission.ACTIVATION_TERMINATION)
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "软件升级激活终止", "");
                        }
                        else
                        {
                            ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "软件升级", "未知传输原因：" + Cot);
                        }
                        break;

                    default:
                        ShowMessage.ShowForChannelMonitor(buffer, len, DirForChannelMonitor, "类型标识：" + typeID, "");
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("通道监视报文解析抛异常！请确保三遥点表配置正确"+ ex.Message);
            }

        }
        #endregion 监视功能相关程序部分
    }
}