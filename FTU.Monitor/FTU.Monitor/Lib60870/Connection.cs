/*
 *  Connection.cs
 */

using System;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using FTU.Monitor.DataService;
using FTU.Monitor.ViewModel;
using FTU.Monitor.Util;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace lib60870
{
    /// <summary>
    /// ConnectionException 的摘要说明
    /// author: songminghao
    /// date：2017/12/07 13:34:09
    /// desc：网络连接异常类
    /// version: 1.0
    /// </summary>
    [Serializable]
    public class ConnectionException : Exception
    {
        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="message">异常提示信息</param>
        public ConnectionException(string message)
            : base(message)
        {
            MainViewModel.outputdata.Debug += message + "\n";
            CommunicationViewModel.con.Close();
        }

        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="message">异常提示信息</param>
        /// <param name="e">异常对象</param>
        public ConnectionException(string message, Exception e)
            : base(message, e)
        {
            MainViewModel.outputdata.Debug += message + e.ToString() + "\n";
            CommunicationViewModel.con.Close();
        }
    }

    /// <summary>
    /// 网口连接事件枚举
    /// </summary>
    public enum ConnectionEvent
    {
        OPENED = 0,
        CLOSED = 1,
        STARTDT_CON_RECEIVED = 2,
        STOPDT_CON_RECEIVED = 3
    }

    /// <summary>
    /// ASDU received handler.
    /// </summary>
    /// <param name="parameter">参数</param>
    /// <param name="asdu">ASDU</param>
    /// <returns></returns>
    public delegate bool ASDUReceivedHandler(object parameter, ASDU asdu);

    /// <summary>
    /// 网口连接handler
    /// </summary>
    /// <param name="parameter">参数</param>
    /// <param name="connectionEvent">网口连接事件枚举</param>
    public delegate void ConnectionHandler(object parameter, ConnectionEvent connectionEvent);

    /// <summary>
    /// Raw message handler. Can be used to access the raw message.
    /// Returns true when message should be handled by the protocol stack, false, otherwise.
    /// </summary>
    /// <param name="parameter">参数</param>
    /// <param name="message">报文信息体</param>
    /// <param name="messageSize">报文信息长度</param>
    /// <returns></returns>
    public delegate bool RawMessageHandler(object parameter, byte[] message, int messageSize);

    /// <summary>
    /// Connection 的摘要说明
    /// author: songminghao
    /// date：2017/12/07 13:42:09
    /// desc：网络连接类
    /// version: 1.0
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// 链路启动请求帧
        /// </summary>
        public static byte[] STARTDT_ACT_MSG = new byte[] { 0x68, 0x04, 0x07, 0x00, 0x00, 0x00 };

        /// <summary>
        /// 链路启动确认帧
        /// </summary>
        public static byte[] STARTDT_CON_MSG = new byte[] { 0x68, 0x04, 0x0B, 0x00, 0x00, 0x00 };

        /// <summary>
        /// 链路停止请求帧
        /// </summary>
        public static byte[] STOPDT_ACT_MSG = new byte[] { 0x68, 0x04, 0x13, 0x00, 0x00, 0x00 };

        /// <summary>
        /// 链路测试请求帧
        /// </summary>
        public static byte[] TESTFR_ACT_MSG = new byte[] { 0x68, 0x04, 0x43, 0x00, 0x00, 0x00 };

        /// <summary>
        /// 链路测试确认帧
        /// </summary>
        public static byte[] TESTFR_CON_MSG = new byte[] { 0x68, 0x04, 0x83, 0x00, 0x00, 0x00 };

        /// <summary>
        /// 接收到的ASDU信息处理Handler(委托)
        /// </summary>
        private ASDUReceivedHandler asduReceivedHandler = null;

        /// <summary>
        /// 接收到的ASDU信息处理Handler参数
        /// </summary>
        private object asduReceivedHandlerParameter = null;

        /// <summary>
        /// 网口连接Handler(委托)
        /// </summary>
        private ConnectionHandler connectionHandler = null;

        /// <summary>
        /// 网口连接Handler参数
        /// </summary>
        private object connectionHandlerParameter = null;

        /// <summary>
        /// 发送报文的序列号
        /// </summary>
        private int sendSequenceNumber;

        /// <summary>
        /// 设置和获取发送报文的序列号.Gets or sets the send sequence number N(S). WARNING: For test purposes only! Do net set
        /// in real application!
        /// </summary>
        /// <value>The send sequence number N(S)</value>
        public int SendSequenceNumber
        {
            get
            {
                return this.sendSequenceNumber;
            }
            set
            {
                this.sendSequenceNumber = value;
            }
        }

        /// <summary>
        /// 接收报文的序列号
        /// </summary>
        private int receiveSequenceNumber;

        /// <summary>
        /// 设置和获取接收报文的序列号.Gets or sets the receive sequence number N(R). 
        /// WARNING: For test purposes only! Do net set in real application!
        /// </summary>
        /// <value>The receive sequence number N(R)</value>
        public int ReceiveSequenceNumber
        {
            get
            {
                return this.receiveSequenceNumber;
            }
            set
            {
                this.receiveSequenceNumber = value;
            }
        }

        /// <summary>
        /// 发送或测试APDU的超时时间(T1)
        /// </summary>
        private UInt64 uMessageTimeout = 0;

        #region k缓冲区报文结构体定义及相关参数设置

        /// <summary>
        /// k缓冲区报文结构体(包含时间和发送序列号).data structure for k-size sent ASDU buffer
        /// </summary>
        private struct SentASDU
        {
            public long sentTime; // required for T1 timeout
            public int seqNo;
        }

        /// <summary>
        /// 未确认帧的最大个数,参数k对应.maximum number of ASDU to be sent without confirmation - parameter k
        /// </summary>
        public static int maxSentASDUs;

        /// <summary>
        /// k缓冲区中最先发送的报文索引.index of oldest entry in k-buffer
        /// </summary>
        public static int oldestSentASDU = -1;

        /// <summary>
        /// k缓冲区中最近发送的报文索引加1(即将要发送的下一帧报文的发送序列号).index of newest entry in k-buffer
        /// </summary>
        public static int newestSentASDU = -1;

        /// <summary>
        /// 发送报文的k缓冲区.the k-buffer 
        /// </summary>
        private SentASDU[] sentASDUs = null;

        #endregion k缓冲区报文结构体定义及相关参数设置

        /// <summary>
        /// 等待发送的ASDU队列
        /// </summary>
        private Queue<ASDU> waitingToBeSent = null;

        /// <summary>
        /// 使用报文发送队列标志
        /// </summary>
        private bool useSendMessageQueue = true;

        /// <summary>
        /// 设置和获取使用报文发送队列标志.Gets or sets a value indicating whether this <see cref="lib60870.Connection"/> use send message queue.
        /// </summary>
        /// <description>
        /// If <c>true</c> the Connection stores the ASDUs to be sent in a Queue when the connection cannot send
        /// ASDUs. This is the case when the counterpart (slave/server) is (temporarily) not able to handle new message,
        /// or the slave did not confirm the reception of the ASDUs for other reasons. If <c>false</c> the ASDU will be 
        /// ignored and a <see cref="lib60870.ConnectionException"/> will be thrown in this case.
        /// </description>
        /// <value><c>true</c> if use send message queue; otherwise, <c>false</c>.</value>
        public bool UseSendMessageQueue
        {
            get
            {
                return this.useSendMessageQueue;
            }
            set
            {
                this.useSendMessageQueue = value;
            }
        }

        /// <summary>
        /// 长期空闲状态下发送测试帧的超时，设定值为20s
        /// </summary>
        private UInt64 nextT3Timeout;

        /// <summary>
        /// 长期空闲状态下发送测试帧的数量
        /// </summary>
        private int outStandingTestFRConMessages = 0;

        /// <summary>
        /// 接收方接收到不确认I格式的报文数量(最大值是w).number of unconfirmed messages received
        /// </summary>
        private int unconfirmedReceivedIMessages;

        /// <summary>
        /// 最近发送的确认报文时间戳(用于检测无数据报文时确认的超时T2).timestamp when the last confirmation message was sent
        /// </summary>
        private long lastConfirmationTime;

        /// <summary>
        /// socket对象
        /// </summary>
        public static RedefineSocket socket;

        /// <summary>
        /// 自动发送启动帧报文标志
        /// </summary>
        private bool autostart = true;

        /// <summary>
        /// 设置和获取自动发送启动帧报文标志.
        /// Gets or sets a value indicating whether this <see cref="lib60870.Connection"/> is automatically sends
        /// a STARTDT_ACT message on startup.
        /// </summary>
        /// <value><c>true</c> to send STARTDT_ACT message on startup; otherwise, <c>false</c>.</value>
        public bool Autostart
        {
            get
            {
                return this.autostart;
            }
            set
            {
                this.autostart = value;
            }
        }

        /// <summary>
        /// 主机名
        /// </summary>
        private string hostname;

        /// <summary>
        /// TCP端口号
        /// </summary>
        private int tcpPort;

        /// <summary>
        /// 网口运行状态标志
        /// </summary>
        public static bool running = false;

        /// <summary>
        /// 获取网口运行状态标志
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return running;
            }
        }

        /// <summary>
        /// 网口连接状态
        /// </summary>
        private bool connecting = false;

        /// <summary>
        /// socket错误标志
        /// </summary>
        private bool socketError;

        /// <summary>
        /// 是否是第一个接收到的I帧报文标志
        /// </summary>
        private bool firstIMessageReceived = false;

        /// <summary>
        /// 上一个异常对象
        /// </summary>
        private Exception lastException;

        /// <summary>
        /// 是否打印输出信息标志
        /// </summary>
        private bool debugOutput = false;

        /// <summary>
        /// 设置和获取是否打印输出信息标志
        /// </summary>
        public bool DebugOutput
        {
            get
            {
                return this.debugOutput;
            }
            set
            {
                this.debugOutput = value;
            }
        }

        /// <summary>
        /// 连接状态计数器
        /// </summary>
        private static int connectionCounter = 0;

        /// <summary>
        /// 连接状态ID
        /// </summary>
        private int connectionID;

        /// <summary>
        /// 关于连接信息的统计对象
        /// </summary>
        public static ConnectionStatistics statistics = new ConnectionStatistics();

        /// <summary>
        /// 建立连接的超时时间，初始化为1秒
        /// </summary>
        private int connectTimeoutInMs = 1000;

        /// <summary>
        /// 连接参数对象
        /// </summary>
        private ConnectionParameters parameters;

        // 串口通道监视接收报文的识别标志位
        private const byte firstSendByte = 0xAA;
        private const byte secondSendByte = 0x55;

        // 监视串口测试帧的定时器
        public System.Timers.Timer testFrameTimerForChannelMonitor;

        // 监视串口测试帧的定时器计时是否开始标志
        bool isTiming = false;

        // 是否收到上一条测试帧的回复报文
        // bool isReceiveTestFrameResponse = true;

        // 是否收到上一条任意报文
        bool isReceiveFrameResponse = true;

        // 未收到报文的超时次数
        UInt32 unrecieveFrameTime = 0;

        /// <summary>
        /// 设置和获取连参数对象
        /// </summary>
        public ConnectionParameters Parameters
        {
            get
            {
                return this.parameters;
            }
        }

        /// <summary>
        /// 打印输出信息
        /// </summary>
        /// <param name="message">输出信息</param>
        public void DebugLog(string message)
        {
            if (debugOutput)
            {
                Console.WriteLine("CS104 MASTER CONNECTION " + connectionID + ": " + message);
                MainViewModel.outputdata.Debug += "CS104 MASTER CONNECTION " + connectionID + ": " + message + "\n";
            }

        }

        /// <summary>
        /// 重置连接
        /// </summary>
        private void ResetConnection()
        {
            // 设置发送报文的序列号
            sendSequenceNumber = 0;
            // 设置接收报文的序列号
            receiveSequenceNumber = 0;
            // 设置未确认的已接收到的报文数量
            unconfirmedReceivedIMessages = 0;
            // 设置最近发送的确认报文时间戳
            lastConfirmationTime = System.Int64.MaxValue;
            // 设置是否是第一个接收到的I帧报文标志
            firstIMessageReceived = false;

            outStandingTestFRConMessages = 0;

            uMessageTimeout = 0;

            // 设置socket错误标志
            socketError = false;
            // 设置上一个异常对象
            lastException = null;

            // 设置未确认帧的最大个数,参数k对应
            maxSentASDUs = parameters.K + 1;
            // 设置k缓冲区中最先发送的报文索引
            oldestSentASDU = -1;
            // 设置k缓冲区中即将要发送的报文索引
            newestSentASDU = -1;
            // 初始化发送报文的k缓冲区
            sentASDUs = new SentASDU[maxSentASDUs];

            // 判断使用报文发送队列标志
            if (useSendMessageQueue)
            {
                // 初始化等待发送的ASDU队列
                waitingToBeSent = new Queue<ASDU>();
            }

            // 重置连接统计对象相关信息
            statistics.Reset();
        }

        /// <summary>
        /// 发送S帧报文
        /// </summary>
        private void SendSMessage()
        {
            // 定义发送S帧报文字节数组
            byte[] msg = new byte[6];

            // 启动字符68H
            msg[0] = 0x68;
            // APDU长度(最大，253)
            msg[1] = 0x04;
            // 控制域八位位组1
            msg[2] = 0x01;
            // 控制域八位位组2
            msg[3] = 0x00;

            // 控制域八位位组3:接收序列号
            msg[4] = (byte)((receiveSequenceNumber % 128) * 2);
            // 控制域八位位组4:接收序列号
            msg[5] = (byte)(receiveSequenceNumber / 128);

            // 将S帧报文数据发送到连接的 System.Net.Sockets.Socket
            socket.Send(msg);

            // 显示发送的报文信息
            ShowMessage.Show(msg, msg.Length, "发送");
            LogHelper.Info(typeof(Connection), "发送报文：" + UtilHelper.ListToString(msg));
            // 关于连接信息的统计对象中发送报文数量加1
            statistics.SentMsgCounter++;
        }

        /// <summary>
        /// 检验序列号是否合法有效.check if received sequence number is valid
        /// </summary>
        /// <param name="seqNo">序列号</param>
        /// <returns></returns>
        private bool CheckSequenceNumber(int seqNo)
        {
            // DebugLog("最先和最近即将要发送的报文索引" + oldestSentASDU + ":" + sentASDUs[oldestSentASDU].seqNo + "-----------" + newestSentASDU + ":" + sentASDUs[newestSentASDU].seqNo + "----待验证接收报文的接收序列号" + seqNo);
            // 给发送报文的k缓冲区加锁(用于线程同步)
            lock (sentASDUs)
            {
                // 序列号有效标志,初始化为false,无效
                bool seqNoIsValid = false;
                // 计数器溢出检测标志,初始化为false,未溢出
                bool counterOverflowDetected = false;

                // 判断k缓冲区中最先发送的报文索引，为-1表示k缓冲区为空
                if (oldestSentASDU == -1)
                {
                    // if k-Buffer is empty
                    if (seqNo == sendSequenceNumber)
                    {
                        // 设置序列号有效标志为true,有效
                        seqNoIsValid = true;
                    }
                }
                else
                {
                    DebugLog("最先和最近即将要发送的报文索引" + oldestSentASDU + ":" + sentASDUs[oldestSentASDU].seqNo + "-----------" + newestSentASDU + ":" + sentASDUs[newestSentASDU].seqNo + "----待验证接收报文的接收序列号" + seqNo);
                    // k缓冲区不为空

                    // Two cases are required to reflect sequence number overflow
                    // 判断最先发送和即将发送的报文序列号:即将发送的报文序列号大于等于最先发送的报文序列号.
                    if (sentASDUs[oldestSentASDU].seqNo <= sentASDUs[newestSentASDU].seqNo)
                    {
                        // 判断待检测序列号是否在最先发送和即将发送的报文序列号之间
                        if (((seqNo >= sentASDUs[oldestSentASDU].seqNo) && (seqNo <= sentASDUs[newestSentASDU].seqNo)))
                        {
                            // 设置序列号有效标志为true,有效
                            seqNoIsValid = true;
                        }
                    }
                    else
                    {
                        // 最近发送的报文序列号小于最先发送的报文序列号

                        // 判断待检测序列号是否在最先发送的报文序列号之后,或在即将发送的报文序列号之前
                        if ((seqNo >= sentASDUs[oldestSentASDU].seqNo) || (seqNo <= sentASDUs[newestSentASDU].seqNo))
                        {
                            // 设置序列号有效标志为true,有效
                            seqNoIsValid = true;
                        }

                        // 计数器溢出检测标志,设置为true,溢出
                        counterOverflowDetected = true;
                    }

                    // 获取最先发送的S帧报文的前一条发送的报文序列号
                    int latestValidSeqNo = (sentASDUs[oldestSentASDU].seqNo - 1) % 32768;

                    // 如果前一条报文序列号等于待检测报文序列号
                    if (latestValidSeqNo == seqNo)
                    {
                        // 设置序列号有效标志为true,有效
                        seqNoIsValid = true;
                    }

                }

                // 判断序列号有效标志,若为false,无效
                if (seqNoIsValid == false)
                {
                    // 打印输出错误信息
                    DebugLog("Received sequence number out of range");
                    // 返回false
                    return false;
                }

                // 判断k缓冲区中最先发送的报文索引,不为-1表示k缓冲区非空
                if (oldestSentASDU != -1)
                {
                    do
                    {
                        // 判断计数器溢出检测标志,false表示未溢出
                        if (counterOverflowDetected == false)
                        {
                            // 判断待检测序列号是不是在最先发送的报文序列号之前
                            if (seqNo < sentASDUs[oldestSentASDU].seqNo)
                            {
                                break;
                            }
                        }
                        else
                        {
                            // 判断待检测序列号是不是最先发送的报文序列号的前一条发送的报文序列号
                            if (seqNo == ((sentASDUs[oldestSentASDU].seqNo - 1) % 32768))
                            {
                                break;
                            }
                        }

                        // 将最先发送的报文序列号索引加1作为新的最先发送的报文序列号索引
                        oldestSentASDU = (oldestSentASDU + 1) % maxSentASDUs;

                        // 即将发送的报文序列号索引加1
                        int checkIndex = (newestSentASDU + 1) % maxSentASDUs;

                        // 判断新的最先发送的报文序列号索引和最先发送的报文序列号索引加1后的索引
                        if (oldestSentASDU == checkIndex)
                        {
                            // 相等，表示k缓冲区为空,设置最先发送的报文序列号索引为-1
                            oldestSentASDU = -1;
                            break;
                        }

                        // 判断最先发送的报文序列号是不是等于待检测序列号
                        if (sentASDUs[oldestSentASDU].seqNo == seqNo)
                        {
                            break;
                        }

                    } while (true);
                }
            }

            return true;
        }

        /// <summary>
        /// 判断发送缓冲区是不是已满
        /// </summary>
        /// <returns></returns>
        public static bool IsSentBufferFull()
        {
            // 获取k缓冲区中即将发送的报文索引加1
            int newIndex = (newestSentASDU + 1) % maxSentASDUs;
            // 判断新索引是否等于k缓冲区中最先发送的报文索引,相等,则表示k缓冲区已满
            if (newIndex == oldestSentASDU)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 发送I帧报文
        /// </summary>
        /// <param name="asdu">ASDU数据</param>
        /// <returns></returns>
        private int SendIMessage(ASDU asdu)
        {
            // 定义发送帧数据对象
            BufferFrame frame = new BufferFrame(new byte[260], 6);
            // 将ASDU数据封装到发送帧中
            asdu.Encode(frame, parameters);

            // 获取发送帧字节数组
            byte[] buffer = frame.GetBuffer();

            //此处有错误
            //int msgSize = frame.GetMsgSize () + 6; /* ASDU size + ACPI size */

            // 获取发送帧大小
            int msgSize = frame.GetMsgSize();

            // 启动字符68H
            buffer[0] = 0x68;
            // 设置APDU长度.set size field
            buffer[1] = (byte)(msgSize - 2);

            // 设置报文发送序列号
            buffer[2] = (byte)((sendSequenceNumber % 128) * 2);
            buffer[3] = (byte)(sendSequenceNumber / 128);

            // 设置报文接收序列号
            buffer[4] = (byte)((receiveSequenceNumber % 128) * 2);
            buffer[5] = (byte)(receiveSequenceNumber / 128);

            // 判断网口运行状态
            if (running)
            {
                // 指定流 System.Net.Sockets.Socket 是否正在使用 Nagle 算法
                socket.NoDelay = true;
                // 使用指定的 System.Net.Sockets.SocketFlags,将指定字节数的数据发送到已连接的 System.Net.Sockets.Socket
                socket.Send(buffer, msgSize, SocketFlags.None);
                // 设置下一条发送报文的序列号
                sendSequenceNumber = (sendSequenceNumber + 1) % 32768;
                // 设置连接信息的统计对象的发送报文数量加1
                statistics.SentMsgCounter++;
                // 设置未确认的已接收到的报文数量为0
                unconfirmedReceivedIMessages = 0;

                // 显示发送报文信息
                ShowMessage.Show(buffer, msgSize, "发送");
                LogHelper.Info(typeof(Connection), "发送报文：" + UtilHelper.ListToString(buffer, msgSize));

                // 返回发送报文的序列号
                return sendSequenceNumber;
            }
            else
            {
                // 判断上一个异常是否为空
                if (lastException != null)
                {
                    throw new ConnectionException(lastException.Message, lastException);
                }
                else
                {
                    throw new ConnectionException("not connected", new SocketException(10057));
                }
            }
        }

        /// <summary>
        /// 打印发送的报文缓冲区信息
        /// </summary>
        private void PrintSendBuffer()
        {
            // 判断k缓冲区中最先发送的报文索引,-1表示k缓冲区为空
            if (oldestSentASDU != -1)
            {
                // 将最先发送的报文索引定义为当前索引
                int currentIndex = oldestSentASDU;

                // 设置k缓冲区下一个报文的索引为0
                int nextIndex = 0;

                DebugLog("------k-buffer------");

                do
                {
                    DebugLog(currentIndex + " : S " + sentASDUs[currentIndex].seqNo + " : time " + sentASDUs[currentIndex].sentTime);

                    // 判断当前索引是否等于即将发送的报文索引
                    if (currentIndex == newestSentASDU)
                    {
                        // 相等,表示k缓冲区为空,设置k缓冲区下一个报文的索引为-1
                        nextIndex = -1;
                    }

                    // 当前索引加1
                    currentIndex = (currentIndex + 1) % maxSentASDUs;

                } while (nextIndex != -1);

                DebugLog("--------------------");

            }
        }

        /// <summary>
        /// 发送I帧报文并更新已发送的报文数组信息
        /// </summary>
        /// <param name="asdu">ASDU数据</param>
        private void SendIMessageAndUpdateSentASDUs(ASDU asdu)
        {
            // 给发送报文的k缓冲区加锁(用于线程同步)
            lock (sentASDUs)
            {
                // 判断k缓冲区中最先发送的报文索引,-1表示k缓冲区为空
                if (oldestSentASDU == -1)
                {
                    // 设置最先发送的报文索引为0
                    oldestSentASDU = 0;
                    // 设置即将发送的报文索引为0
                    newestSentASDU = 0;
                }

                // 获取当前发送报文的序列号
                int currentSendSeq = SendIMessage(asdu) - 1;

                // 设置发送报文的k缓冲区当前发送报文的序列号
                sentASDUs[newestSentASDU].seqNo = currentSendSeq;
                // 设置发送报文的k缓冲区当前发送报文的发送时间
                sentASDUs[newestSentASDU].sentTime = SystemUtils.currentTimeMillis();

                // 设置即将发送的报文索引
                newestSentASDU = (newestSentASDU + 1) % maxSentASDUs;

                // 设置发送报文的k缓冲区即将发送报文的序列号
                sentASDUs[newestSentASDU].seqNo = currentSendSeq + 1;
                // 设置发送报文的k缓冲区即将发送报文的发送时间
                sentASDUs[newestSentASDU].sentTime = SystemUtils.currentTimeMillis();

                // 打印发送的报文缓冲区信息
                PrintSendBuffer();
            }

        }

        /// <summary>
        /// 发送下一个正在等待的ASDU数据
        /// </summary>
        /// <returns>ASDU发送是否成功,true表示成功,false表示失败</returns>
        private bool SendNextWaitingASDU()
        {
            // 发送ASDU标志,初始化为false
            bool sentAsdu = false;

            // 判断网口运行状态
            if (running == false)
            {
                throw new ConnectionException("connection lost");
            }

            try
            {
                // 给等待发送的ASDU队列加锁(线程同步使用)
                lock (waitingToBeSent)
                {
                    // 等待发送的ASDU队列非空,则一直循环
                    while (waitingToBeSent.Count > 0)
                    {
                        // 判断发送缓冲区(k缓冲区)是不是已满
                        if (IsSentBufferFull() == true)
                        {
                            // 满,则跳出
                            break;
                        }

                        // 发送缓冲区(k缓冲区)不满,将等待发送的ASDU队列队头元素出队
                        ASDU asdu = waitingToBeSent.Dequeue();

                        // asdu不为空
                        if (asdu != null)
                        {
                            // 发送I帧报文并更新已发送的报文数组信息
                            SendIMessageAndUpdateSentASDUs(asdu);
                            // 设置发送ASDU标志为true
                            sentAsdu = true;
                        }
                        else
                        {
                            break;
                        }
                    }

                }
            }
            catch (Exception)
            {
                // 设置网口运行状态为false
                running = false;
                throw new ConnectionException("connection lost");
            }

            // 返回发送ASDU标志
            return sentAsdu;
        }

        /// <summary>
        /// 发送ASDU数据
        /// </summary>
        /// <param name="asdu">ASDU数据</param>
        private void SendASDUInternal(ASDU asdu)
        {
            try
            {
                // 判断是不是串口发送
                if (SerialPortService.serialPort.IsOpen)
                {
                    CommunicationViewModel.serialPortSerice.SendVariableFrame(asdu);
                }
                else if (Connection.running)
                {
                    // 网口发送
                    // 给socket加锁(线程同步使用)
                    lock (socket)
                    {
                        // 判断网口运行状态
                        if (running == false)
                        {
                            throw new ConnectionException("not connected", new SocketException(10057));
                        }

                        // 判断使用报文发送队列标志
                        if (useSendMessageQueue)
                        {
                            // 给等待发送的ASDU队列加锁(线程同步使用)
                            lock (waitingToBeSent)
                            {
                                // 将ASDU数据入队等待发送的ASDU队列
                                waitingToBeSent.Enqueue(asdu);
                            }

                            // 发送下一个正在等待的ASDU数据
                            SendNextWaitingASDU();
                        }
                        else
                        {
                            #region 不使用报文发送队列

                            // 判断发送缓冲区(k缓冲区)是不是已满
                            if (IsSentBufferFull())
                            {
                                throw new ConnectionException("Flow control congestion. Try again later.");
                            }

                            // 发送I帧报文并更新已发送的报文数组信息
                            SendIMessageAndUpdateSentASDUs(asdu);

                            #endregion 不使用报文发送队列
                        }
                    }

                }
                else if (SerialPortService.serialPort.IsOpen == false || Connection.running == false)
                {
                    MessageBox.Show("链路已断开", "警告");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                LogHelper.Info(typeof(Connection), "异常：" + e.ToString());
                throw new ConnectionException(e.ToString(), e);
            }
        }

        /// <summary>
        /// 启动方法
        /// </summary>
        /// <param name="parameters">网口连接参数对象</param>
        private void Setup(ConnectionParameters parameters)
        {
            this.parameters = parameters;
        }

        /// <summary>
        /// 启动方法(重载)
        /// </summary>
        /// <param name="hostname">主机名</param>
        /// <param name="parameters">网口连接参数对象</param>
        /// <param name="tcpPort">TCP端口号</param>
        private void Setup(string hostname, ConnectionParameters parameters, int tcpPort)
        {
            this.hostname = hostname;
            this.parameters = parameters;
            this.tcpPort = tcpPort;

            // 设置建立连接的超时时间
            this.connectTimeoutInMs = parameters.T0 * 1000;

            // 连接状态计数器自增1
            connectionCounter++;
            // 设置连接状态ID
            this.connectionID = connectionCounter;
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public Connection()
        {
            Setup(new ConnectionParameters());
        }

        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="a">信息对象地址长度</param>
        public Connection(int a)
        {
            Setup(new ConnectionParameters(a));
        }

        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="hostname">主机名</param>
        public Connection(string hostname)
        {
            Setup(hostname, new ConnectionParameters(), 2404);
        }

        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="hostname">主机名</param>
        /// <param name="tcpPort">TCP端口号</param>
        public Connection(string hostname, int tcpPort)
        {
            Setup(hostname, new ConnectionParameters(), tcpPort);
        }

        /// <summary>
        /// Sends the interrogation command.发送询问命令
        /// </summary>
        /// <param name="cot">Cause of transmission</param>传送原因
        /// <param name="ca">Common address  公共地址</param>
        /// <param name="qoi">Qualifier of interrogation (20 = station interrogation) 召唤限定词，总召唤为20</param>
        /// <exception cref="ConnectionException">description</exception>
        public void SendInterrogationCommand(CauseOfTransmission cot, int ca, byte qoi)
        {
            ASDU asdu = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);

            asdu.AddInformationObject(new InterrogationCommand(0, qoi));//此函数将信息对象（InformationObject）添加到ASDU

            SendASDUInternal(asdu);//发送ASDU数据
        }

        /// <summary>
        /// 切换定值区
        /// </summary>
        /// <param name="cot">传送原因</param>
        /// <param name="ca">ASDU公共地址</param>
        /// <param name="parameterArea">定值区</param>
        public void SendChangeParameterAreaCommand(CauseOfTransmission cot, int ca, int parameterArea)
        {
            ASDU asdu = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);

            asdu.AddInformationObject(new ChangeSettingAreaCommand(0, parameterArea));

            SendASDUInternal(asdu);
        }

        /// <summary>
        /// 读当前定值区号
        /// </summary>
        /// <param name="cot">传送原因</param>
        /// <param name="ca">ASDU公共地址</param>
        public void SendReadParameterAreaCommand(CauseOfTransmission cot, int ca)
        {
            ASDU asdu = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);

            asdu.AddInformationObject(new ReadParameterAreaCommand(0));

            SendASDUInternal(asdu);
        }

        /// <summary>
        /// 读定值
        /// </summary>
        /// <param name="cot">传送原因</param>
        /// <param name="ca">ASDU公共地址</param>
        /// <param name="buf">定值参数数组</param>
        public void SendReadParameterCommand(CauseOfTransmission cot, int ca, byte[] buf)
        {
            ASDU asdu = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);
            asdu.AddInformationObject(new ReadParameterCommand(buf));

            SendASDUInternal(asdu);
        }

        /// <summary>
        /// 固化报文
        /// </summary>
        /// <param name="cot">传送原因</param>
        /// <param name="ca">ASDU公共地址</param>
        /// <param name="buf">固化参数数组</param>
        public void SendSetParameterCommand(CauseOfTransmission cot, int ca, byte[] buf)
        {
            ASDU asdu = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);
            asdu.AddInformationObject(new SetParameterCommand(buf));

            SendASDUInternal(asdu);
        }

        /// <summary>
        /// 发送电能量数据报文.Sends the counter interrogation command (C_CI_NA_1 typeID: 101)
        /// </summary>
        /// <param name="cot">Cause of transmission</param>
        /// <param name="ca">Common address</param>
        /// <param name="qcc">Qualifier of counter interrogation command</param>
        /// <exception cref="ConnectionException">description</exception>
        public void SendCounterInterrogationCommand(CauseOfTransmission cot, int ca, byte qcc)
        {
            ASDU asdu = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);

            asdu.AddInformationObject(new CounterInterrogationCommand(0, qcc));

            SendASDUInternal(asdu);
        }

        /// <summary>
        /// Sends a read command (C_RD_NA_1 typeID: 102).
        /// </summary>
        /// 
        /// This will send a read command C_RC_NA_1 (102) to the slave/outstation. The COT is always REQUEST (5).
        /// It is used to implement the cyclical polling of data application function.
        /// 
        /// <param name="ca">Common address</param>
        /// <param name="ioa">Information object address</param>
        /// <exception cref="ConnectionException">description</exception>
        public void SendReadCommand(int ca, int ioa)
        {
            ASDU asdu = new ASDU(parameters, CauseOfTransmission.REQUEST, false, false, (byte)parameters.OriginatorAddress, ca, false);

            asdu.AddInformationObject(new ReadCommand(ioa)); //增加信息到asdu后面，ioa定值点号

            SendASDUInternal(asdu);
        }

        /// <summary>
        /// Sends a clock synchronization command (C_CS_NA_1 typeID: 103).
        /// </summary>
        /// <param name="ca">Common address</param>
        /// <param name="time">the new time to set</param>
        /// <exception cref="ConnectionException">description</exception>
        public void SendClockSyncCommand(int ca, CP56Time2a time)
        {
            ASDU asdu = new ASDU(parameters, CauseOfTransmission.ACTIVATION, false, false, (byte)parameters.OriginatorAddress, ca, false);

            asdu.AddInformationObject(new ClockSynchronizationCommand(0, time));

            SendASDUInternal(asdu);
        }

        /// <summary>
        /// 时钟读取命令
        /// </summary>
        /// <param name="ca">ASDU公共地址</param>
        /// <param name="time">CP56Time2a时标</param>
        public void SendClockReadCommand(int ca, CP56Time2a time)
        {
            ASDU asdu = new ASDU(parameters, CauseOfTransmission.REQUEST, false, false, (byte)parameters.OriginatorAddress, ca, false);

            asdu.AddInformationObject(new ClockSynchronizationCommand(0, time));

            SendASDUInternal(asdu);

        }

        /// <summary>
        /// Sends a test command (C_TS_NA_1 typeID: 104).
        /// </summary>
        /// 
        /// Not required and supported by IEC 60870-5-104. 
        /// 
        /// <param name="ca">Common address</param>
        /// <exception cref="ConnectionException">description</exception>
        public void SendTestCommand(int ca)
        {
            ASDU asdu = new ASDU(parameters, CauseOfTransmission.ACTIVATION, false, false, (byte)parameters.OriginatorAddress, ca, false);

            asdu.AddInformationObject(new TestCommand());

            SendASDUInternal(asdu);
        }

        /// <summary>
        /// 发送进程复位命令.Sends a reset process command (C_RP_NA_1 typeID: 105).
        /// </summary>
        /// <param name="cot">Cause of transmission</param>
        /// <param name="ca">Common address</param>
        /// <param name="qrp">Qualifier of reset process command</param>
        /// <exception cref="ConnectionException">description</exception>
        public void SendResetProcessCommand(CauseOfTransmission cot, int ca, byte qrp)
        {
            ASDU asdu = new ASDU(parameters, CauseOfTransmission.ACTIVATION, false, false, (byte)parameters.OriginatorAddress, ca, false);

            asdu.AddInformationObject(new ResetProcessCommand(0, qrp));

            SendASDUInternal(asdu);
        }


        /// <summary>
        /// 发送时钟延时获得命令.Sends a delay acquisition command (C_CD_NA_1 typeID: 106).
        /// </summary>
        /// <param name="cot">Cause of transmission</param>
        /// <param name="ca">Common address</param>
        /// <param name="delay">delay for acquisition</param>
        /// <exception cref="ConnectionException">description</exception>
        public void SendDelayAcquisitionCommand(CauseOfTransmission cot, int ca, CP16Time2a delay)
        {
            ASDU asdu = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);

            asdu.AddInformationObject(new DelayAcquisitionCommand(0, delay));

            SendASDUInternal(asdu);
        }

        /// <summary>
        /// 发送遥控命令.Sends the control command.
        /// </summary>
        /// 
        /// The type ID has to match the type of the InformationObject!
        /// 
        /// C_SC_NA_1 -> SingleCommand
        /// C_DC_NA_1 -> DoubleCommand
        /// C_RC_NA_1 -> StepCommand
        /// C_SC_TA_1 -> SingleCommandWithCP56Time2a
        /// C_SE_NA_1 -> SetpointCommandNormalized
        /// C_SE_NB_1 -> SetpointCommandScaled
        /// C_SE_NC_1 -> SetpointCommandShort
        /// C_BO_NA_1 -> Bitstring32Command
        /// 
        /// <param name="cot">Cause of transmission (use ACTIVATION to start a control sequence)</param>
        /// <param name="ca">Common address</param>
        /// <param name="sc">Information object of the command</param>
        /// <exception cref="ConnectionException">description</exception>
        public void SendControlCommand(CauseOfTransmission cot, int ca, InformationObject sc)
        {
            ASDU controlCommand = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);
            controlCommand.AddInformationObject(sc);
            SendASDUInternal(controlCommand);
        }

        /// <summary>
        /// Start data transmission on this connection
        /// </summary>
        public void SendStartDT()
        {
            // 判断网口运行状态
            if (running)
            {
                // 将数据发送到连接的 System.Net.Sockets.Socket
                socket.Send(STARTDT_ACT_MSG);
                // 显示发送的报文信息
                ShowMessage.Show(STARTDT_ACT_MSG, STARTDT_ACT_MSG.Length, "发送");
                LogHelper.Info(typeof(Connection), "发送报文：" + UtilHelper.ListToString(STARTDT_ACT_MSG));
                // 连接信息的统计对象的发送报文数量加1
                statistics.SentMsgCounter++;
            }
            else
            {
                // 判断上一个异常对象是否为空
                if (lastException != null)
                {
                    throw new ConnectionException(lastException.Message, lastException);
                }
                else
                {
                    throw new ConnectionException("not connected", new SocketException(10057));
                }

            }
        }

        /// <summary>
        /// Stop data transmission on this connection
        /// </summary>
        public void SendStopDT()
        {
            // 判断网口运行状态
            if (running)
            {
                // 将数据发送到连接的 System.Net.Sockets.Socket
                socket.Send(STOPDT_ACT_MSG);
                // 显示发送的报文信息
                ShowMessage.Show(STOPDT_ACT_MSG, STOPDT_ACT_MSG.Length, "发送");
                LogHelper.Info(typeof(Connection), "发送报文：" + UtilHelper.ListToString(STOPDT_ACT_MSG));
                // 连接信息的统计对象的发送报文数量加1
                statistics.SentMsgCounter++;
            }
            else
            {
                // 判断上一个异常对象是否为空
                if (lastException != null)
                {
                    throw new ConnectionException(lastException.Message, lastException);
                }
                else
                {
                    throw new ConnectionException("not connected", new SocketException(10057));
                }
            }
        }

        #region  文件服务相关指令操作

        /// <summary>
        /// 文件服务指令操作方法
        /// </summary>
        /// <param name="cot">传送原因</param>
        /// <param name="ca">ASDU公共地址</param>
        /// <param name="buf">报文字节数组</param>
        public void SendFileServiceCommand(CauseOfTransmission cot, int ca, byte[] buf)
        {
            ASDU controlCommand = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);
            controlCommand.AddInformationObject(new FileServiceCommand(buf));
            SendASDUInternal(controlCommand);
        }

        /// <summary>
        /// 发送升级文件命令
        /// </summary>
        /// <param name="cot">传送原因</param>
        /// <param name="ca">ASDU公共地址</param>
        /// <param name="ctype">命令类型</param>
        public void SendUpdataCommand(CauseOfTransmission cot, int ca, byte ctype)
        {
            ASDU controlCommand = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);
            controlCommand.AddInformationObject(new UpdataCommand(0, ctype));
            SendASDUInternal(controlCommand);
        }

        #endregion 文件服务相关指令操作

        /// <summary>
        /// Connect this instance.
        /// </summary>
        /// The function will throw a SocketException if the connection attempt is rejected or timed out.
        /// <exception cref="ConnectionException">description</exception>
        public void Connect()
        {
            // 连接通信
            ConnectAsync();

            Console.WriteLine("--------------异步网口连接中---------------");

            int i = 0;

            // 判断网口连接状态和socket错误标志
            while ((running == false) && (socketError == false))
            {
                Console.WriteLine("--------------循环判断网口连接状态中---------------");
                // 将当前线程挂起1毫秒
                Thread.Sleep(1);
                i++;
                if (i == 200)
                {
                    socketError = true;
                }
            }

            // 判断socket错误标志
            if (socketError)
            {
                if (lastException != null)
                {
                    throw new ConnectionException("socket错误:" + lastException.Message, lastException);
                }
                DebugLog("循环判断网口连接状态超时");
            }

        }

        /// <summary>
        /// 重置长期空闲状态下发送测试帧的超时(T3)
        /// </summary>
        private void ResetT3Timeout()
        {
            this.nextT3Timeout = (UInt64)SystemUtils.currentTimeMillis() + (UInt64)(parameters.T3 * 1000);
        }

        /// <summary>
        /// Connects to the server (outstation). This is a non-blocking call. Before using the connection
        /// you have to check if the connection is already connected and running.
        /// </summary>
        /// <exception cref="ConnectionException">description</exception>
        public void ConnectAsync()
        {
            // 判断网口运行状态和网口连接状态
            if ((running == false) && (connecting == false))
            {
                // 重置连接
                ResetConnection();
                Console.WriteLine("--------------重置连接结束---------------");

                // 重置长期空闲状态下发送测试帧的超时(T3)
                ResetT3Timeout();
                Console.WriteLine("--------------重置长期空闲状态下发送测试帧的超时(T3)结束---------------");

                // 开启线程
                Thread workerThread = new Thread(Run);
                // 启动线程
                workerThread.IsBackground = true;
                workerThread.Start();
            }
            else
            {
                // 判断网口运行状态
                if (running)
                {
                    // WSAEISCONN - Socket is already connected
                    throw new ConnectionException("already connected", new SocketException(10056));
                }
                else
                {
                    // WSAEALREADY - Operation already in progress
                    throw new ConnectionException("already connecting", new SocketException(10037));
                }
            }

        }

        /// <summary>
        /// 接收报文信息
        /// </summary>
        /// <param name="socket">socket套接字</param>
        /// <param name="buffer">接收报文缓冲区</param>
        /// <returns>
        ///      0 : if remote site closed connection
        ///     -1 : error occurs
        ///     >0 : The number of bytes received.
        /// </returns>
        private int ReceiveMessage(RedefineSocket socket, byte[] buffer)
        {
            int ret = socket.Receive(buffer, 0, 1, SocketFlags.None);
            if (ret != 1)
            {
                return ret;
            }

            // 判断第一个字节是不是启动字符0x68
            if (buffer[0] != 0x68 && buffer[0] != 0x11)
            {
                DebugLog("Missing SOF indicator!");
                LogHelper.Warn(typeof(Connection), "buffer[0] != 0x68 and buffer[0] != 0x11 buffer[0]:" + buffer[0]);
                return -1;
            }
            if (buffer[0] == 0x68)
            {
                // 读取报文长度字节.read length byte
                ret = socket.Receive(buffer, 1, 1, SocketFlags.None);
                if (ret != 1)
                {
                    LogHelper.Warn(typeof(Connection), "Failed to read packet length, ret:" + ret);
                    return ret;
                }
                int length = buffer[1] + 2;
                int nread = 2;
                while (nread < length)
                {
                    ret = socket.Receive(buffer, nread, length - nread, SocketFlags.None);
                    if (ret <= 0)
                    {
                        LogHelper.Warn(typeof(Connection), "Faied to receive, ret:" + ret);
                        return ret;
                    }
                    nread += ret;
                }
                return nread;
            }
            else if (buffer[0] == 0x11)
            {
                int length = 6;
                int nread = 1;
                while (nread < length)
                {
                    ret = socket.Receive(buffer, nread, length - nread, SocketFlags.None);
                    if (ret <= 0)
                    {
                        LogHelper.Warn(typeof(Connection), "Faied to receive, ret:" + ret);
                        return ret;
                    }
                    nread += ret;
                }
                return nread;
            }
            return -1;
        }

        /// <summary>
        /// 检测是否无数据报文时确认的超时(T2,T2小于T1)
        /// </summary>
        /// <param name="currentTime">当前时间</param>
        /// <returns></returns>
        private bool checkConfirmTimeout(long currentTime)
        {
            if ((currentTime - lastConfirmationTime) >= (parameters.T2 * 1000))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 检验以及解析报文数据
        /// </summary>
        /// <param name="socket">socket套接字</param>
        /// <param name="buffer">包含接收到的一帧报文字节数组(长度大于等于这帧报文长度)</param>
        /// <param name="msgSize">接收到的一帧报文长度</param>
        /// <returns></returns>
        private bool CheckMessage(RedefineSocket socket, byte[] buffer, int msgSize)
        {
            // 判断接收到的报文长度是否满足最低报长度
            if (msgSize < 6)
            {
                DebugLog("msg too small!");
                return false;
            }

            // 获取当前时间
            long currentTime = SystemUtils.currentTimeMillis();

            #region 通道监视响应报文处理
            if (buffer[0] == 0x11)
            {
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
                        testFrameTimerForChannelMonitor = new System.Timers.Timer(2000);
                        testFrameTimerForChannelMonitor.AutoReset = true;
                        testFrameTimerForChannelMonitor.Enabled = true;
                        testFrameTimerForChannelMonitor.Elapsed += new System.Timers.ElapsedEventHandler(TimerOut);
                        isTiming = true;
                    }
                    else if (buffer[1] == 0x0F)
                    {
                        MessageBox.Show("通道监视端口正与上位机连接中，请选择正确的监视端口");
                    }
                    else
                    {
                        MessageBox.Show("通道监视回复未知功能码：" + buffer[1] + " " + buffer[2] + " " + buffer[3] + " " + buffer[4] + " " + buffer[5]);
                    }
                    return true;
                }
                else
                {
                    // 报文校验和不对
                    return false;
                }
            }

            #endregion 通道监视响应报文处理

            #region I格式帧数据处理

            // 判断是不是I格式帧(I format frame)
            if ((buffer[2] & 1) == 0 && (buffer[4] & 1) == 0)
            {
                // 判断第一个接收到的I帧报文标志
                if (!firstIMessageReceived)
                {
                    firstIMessageReceived = true;
                    // 最近发送的确认报文时间戳(用于检测无数据报文时确认的超时T2).start timeout T2
                    lastConfirmationTime = currentTime;
                }

                // 判断I帧报文长度,至少大于7
                if (msgSize < 7)
                {
                    DebugLog("I msg too small!");
                    return false;
                }

                // 获取接收帧的发送序列号
                int frameSendSequenceNumber = ((buffer[3] * 0x100) + (buffer[2] & 0xfe)) / 2;
                // 获取接收帧的接收序列号
                int frameRecvSequenceNumber = ((buffer[5] * 0x100) + (buffer[4] & 0xfe)) / 2;

                // 打印输出信息
                DebugLog("Received I frame: N(S) = " + frameSendSequenceNumber + " N(R) = " + frameRecvSequenceNumber);

                // 检验接收到帧的发送序列号,是否等于接收序列号
                // check the receive sequence number N(R) - connection will be closed on an unexpected value
                if (frameSendSequenceNumber != receiveSequenceNumber)
                {
                    DebugLog(frameSendSequenceNumber + "(s):(r)" + receiveSequenceNumber + " Sequence error: Close connection!");
                    return false;
                }

                // 检验接收帧的接收序列号是否合法
                if (!CheckSequenceNumber(frameRecvSequenceNumber))
                {
                    return false;
                }

                // 设置接收报文的序列号加1
                receiveSequenceNumber = (receiveSequenceNumber + 1) % 32768;
                // 未确认的已接收到的报文数量自增1
                unconfirmedReceivedIMessages++;

                try
                {
                    // 获取ASDU应用服务数据单元
                    ASDU asdu = new ASDU(parameters, buffer, 6, msgSize);

                    // 显示接收的报文信息
                    ShowMessage.Show(buffer, msgSize, "接收");
                    LogHelper.Info(typeof(Connection), "接收报文：" + UtilHelper.ListToString(buffer, msgSize));

                    // 处理ASDU应用服务数据单元
                    if (asduReceivedHandler != null)
                    {
                        asduReceivedHandler(asduReceivedHandlerParameter, asdu);
                    }

                }
                catch (ASDUParsingException e)
                {
                    DebugLog("ASDU parsing failed: " + e.Message);
                    LogHelper.Error(typeof(Connection), "解析ASDU异常 \n" + e.Message);
                    return false;
                }

                // 重置长期空闲状态下发送测试帧的超时(T3)
                ResetT3Timeout();

                return true;

            }

            #endregion I格式帧数据处理

            #region S格式帧数据处理

            // 判断是不是S格式帧(S format frame)
            if (buffer[2] == 0x01 && buffer[3] == 0x00 && (buffer[4] & 0x01) == 0x00)
            {
                // 获取接收序列号
                int seqNo = (buffer[4] + buffer[5] * 0x100) / 2;

                DebugLog("Recv S(" + seqNo + ") (own sendcounter = " + sendSequenceNumber + ")");
                // 显示接收到的报文信息
                ShowMessage.Show(buffer, 6, "接收");
                LogHelper.Info(typeof(Connection), "接收报文：" + UtilHelper.ListToString(buffer, 6));

                // 检验序列号是否合法有效
                if (!CheckSequenceNumber(seqNo))
                {
                    return false;
                }

                // 重置长期空闲状态下发送测试帧的超时(T3)
                ResetT3Timeout();

                return true;

            }

            #endregion S格式帧数据处理

            #region U格式帧数据处理

            // 判断是不是U格式帧(U format frame)
            if ((buffer[2] & 0x03) == 0x03 && (buffer[4] & 0x01) == 0x00)
            {
                // 显示接受的报文信息
                ShowMessage.Show(buffer, 6, "接收");
                LogHelper.Info(typeof(Connection), "接收报文：" + UtilHelper.ListToString(buffer, 6));
                // 设置发送或测试APDU的超时时间
                uMessageTimeout = 0;

                switch (buffer[2])
                {
                    case 0x43:
                        // 测试请求帧.Check for TESTFR_ACT message

                        statistics.RcvdTestFrActCounter++;
                        DebugLog("RCVD TESTFR_ACT");
                        DebugLog("SEND TESTFR_CON");

                        // 发送链路测试确认帧
                        socket.Send(TESTFR_CON_MSG);
                        // 显示接收到的报文信息
                        ShowMessage.Show(TESTFR_CON_MSG, TESTFR_CON_MSG.Length, "发送");
                        LogHelper.Info(typeof(Connection), "发送报文：" + UtilHelper.ListToString(TESTFR_CON_MSG));
                        // 连接信息的统计对象的报文数量加1
                        statistics.SentMsgCounter++;

                        break;

                    case 0x83:
                        // 测试确认帧.TESTFR_CON

                        DebugLog("RCVD TESTFR_CON");
                        statistics.RcvdTestFrConCounter++;
                        outStandingTestFRConMessages = 0;
                        break;

                    case 0x07:
                        // 链路启动请求帧.STARTDT ACT

                        DebugLog("RCVD STARTDT_ACT");
                        // 发送链路启动确认帧
                        socket.Send(STARTDT_CON_MSG);
                        // 显示接收到的报文信息
                        ShowMessage.Show(STARTDT_CON_MSG, STARTDT_CON_MSG.Length, "发送");
                        LogHelper.Info(typeof(Connection), "发送报文：" + UtilHelper.ListToString(STARTDT_CON_MSG));
                        // 连接信息的统计对象的报文数量加1
                        statistics.SentMsgCounter++;

                        break;

                    case 0x0B:
                        // 链路启动确认帧.STARTDT_CON

                        DebugLog("RCVD STARTDT_CON");
                        // 网口连接状况处理
                        if (connectionHandler != null)
                        {
                            connectionHandler(connectionHandlerParameter, ConnectionEvent.STARTDT_CON_RECEIVED);
                        }

                        break;

                    case 0x23:
                        // 链路停止确认帧.STOPDT_CON

                        DebugLog("RCVD STOPDT_CON");
                        // 网口连接状况处理
                        if (connectionHandler != null)
                        {
                            connectionHandler(connectionHandlerParameter, ConnectionEvent.STOPDT_CON_RECEIVED);
                        }

                        break;
                }

                // 重置长期空闲状态下发送测试帧的超时(T3)
                ResetT3Timeout();

                return true;
            }

            #endregion U格式帧数据处理

            #region 错误格式帧数据处理

            // 输出未识别帧格式
            DebugLog("Unknown message type");
            return false;

            #endregion 错误格式帧数据处理

        }

        /// <summary>
        /// 连接套接字创建
        /// </summary>
        private void ConnectSocketWithTimeout()
        {
            // 提供网际协议 (IP) 地址
            IPAddress ipAddress;
            // 将网络端点表示为 IP 地址和端口号
            IPEndPoint remoteEP;

            try
            {
                // 将 IP 地址字符串转换为 System.Net.IPAddress 实例
                ipAddress = IPAddress.Parse(hostname);
                // 用指定的地址和端口号初始化 System.Net.IPEndPoint 类的新实例
                remoteEP = new IPEndPoint(ipAddress, tcpPort);
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------IP地址和端口号有误---------------");
                LogHelper.Error(typeof(Connection), "IP地址和端口号有误 \n");
                // wrong argument
                throw new ConnectionException("SocketException:IP地址和端口号有误" + e.Message, new SocketException(87));
            }

            Console.WriteLine("--------------IP地址和端口号初始化成功---------------");

            try
            {
                // 创建套接字.Create a TCP/IP  socket.
                socket = new RedefineSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------套接字创建失败---------------");
                LogHelper.Error(typeof(Connection), "套接字创建失败 \n");
                // wrong argument
                throw new ConnectionException("SocketException:套接字创建失败" + e.Message, new SocketException(87));
            }

            Console.WriteLine("--------------套接字创建成功---------------");

            try
            {
                // 开始一个对远程主机连接的异步请求
                var result = socket.BeginConnect(remoteEP, null, null);
                Console.WriteLine("--------------开始一个对远程主机连接的异步请求---------------");

                // 阻止当前线程，直到当前的 System.Threading.WaitHandle 收到信号为止，
                // 同时使用 32 位带符号整数指定时间间隔，并指定是否在等待之前退出同步域
                bool success = result.AsyncWaitHandle.WaitOne(connectTimeoutInMs, true);
                Console.WriteLine("--------------阻止当前线程，直到当前的 System.Threading.WaitHandle 收到信号---------------");
                if (success)
                {
                    Console.WriteLine("--------------结束挂起的异步连接请求---------------");
                    // 结束挂起的异步连接请求
                    socket.EndConnect(result);
                }
                else
                {
                    // 关闭 System.Net.Sockets.Socket 连接并释放所有关联的资源
                    socket.Close();

                    Console.WriteLine("--------------套接字关闭,连接时间超时---------------");

                    // Connection timed out.
                    throw new ConnectionException("SocketException:套接字关闭,连接时间超时", new SocketException(10060));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------对远程主机连接的异步请求失败---------------");
                LogHelper.Error(typeof(Connection), "对远程主机连接的异步请求失败 \n");

                throw new ConnectionException("SocketException:对远程主机连接的异步请求失败。" + e.Message, new SocketException(10060));
            }

        }

        private bool ParseMessage(byte[] bytes)
        {
            // Receive a message from from the remote device.
            int bytesRec = ReceiveMessage(socket, bytes);
            if (bytesRec == 0)
            {
                LogHelper.Warn(typeof(Connection), "Connection has been closed by peer side");
                return false;
            }
            if (bytesRec < 0)
            {
                LogHelper.Warn(typeof(Connection), "Failed to receive message");
                return false;
            }

            // 连接信息的统计对象的接收报文数量加1
            statistics.RcvdMsgCounter++;

            DebugLog("RCVD: " + BitConverter.ToString(bytes, 0, bytesRec));

            // 检验以及解析报文数据
            if (!CheckMessage(socket, bytes, bytesRec))
            {
                DebugLog("loopRunning设置为false:checkMessage()错误");
                return false;
            }

            // 接收方接收到不确认I格式的报文数量是否达到上限
            if (unconfirmedReceivedIMessages >= parameters.W)
            {
                // 达到上限,设置最近发送的确认报文时间戳(用于检测无数据报文时确认的超时T2)为当前时间
                lastConfirmationTime = SystemUtils.currentTimeMillis();
                // 接收方接收到不确认I格式的报文数量设置为0
                unconfirmedReceivedIMessages = 0;
                // 发送S帧报文
                SendSMessage();
            }
            return true;
        }

        private bool MonitorMessage(byte[] bytesForChannelMonitor, ref List<byte> ListForChannelMonitor)
        {
            // 从远程设备读取字节
            int bytesRecForChannelMonitor = ReceiveMessageForChannelMonitor(socket, bytesForChannelMonitor);
            if (bytesRecForChannelMonitor > 0)
            {
                // 重置定时器
                if (isTiming)
                {
                    // 屏蔽的一下两行代码为取消在空闲链路状态下发送测试帧的逻辑
                    //testFrameTimerForChannelMonitor.Stop();
                    //testFrameTimerForChannelMonitor.Start();
                    //isReceiveTestFrameResponse = true;
                    isReceiveFrameResponse = true;
                    unrecieveFrameTime = 0;
                }

                // 输出接收报文信息
                DebugLog("串口监视RCVD: " + BitConverter.ToString(bytesForChannelMonitor, 0, bytesRecForChannelMonitor));
                // 将接收的字符byte转换成List
                byte[] subset = new byte[bytesRecForChannelMonitor];
                Array.Copy(bytesForChannelMonitor, subset, bytesRecForChannelMonitor);
                ListForChannelMonitor.AddRange(subset);
                // 检验以及解析报文数据
                bool ok = CheckMessageForChannelMonitor(ref ListForChannelMonitor);
                if (ok)
                {
                    LogHelper.Warn(typeof(Connection), "解析报文成功");
                    return true;
                }
                else
                {
                    testFrameTimerForChannelMonitor.Stop();
                    LogHelper.Warn(typeof(Connection), "解析报文错误");
                    return false;
                }
            }
            else if (bytesRecForChannelMonitor == 0)
            {
                testFrameTimerForChannelMonitor.Stop();
                LogHelper.Warn(typeof(Connection), "端口监视loopRunning设置为false:接收的报文字节数组长度为0,服务端已经断开连接");
                return false;
            }
            else
            {
                testFrameTimerForChannelMonitor.Stop();
                LogHelper.Warn(typeof(Connection), "端口监视loopRunning设置为false:接收的报文字节数组长度为-1,或是不是已连接:");
                return false;
            }
        }

        /// <summary>
        /// 超时判断处理
        /// </summary>
        /// <returns></returns>
        private bool HandleTimeouts()
        {
            // 获取当前总毫秒数
            UInt64 currentTime = (UInt64)SystemUtils.currentTimeMillis();

            #region 长期空闲状态下发送测试帧的超时

            // 判断是不是T3超时(长期空闲状态下发送测试帧的超时)
            if (currentTime > nextT3Timeout)
            {
                // 长期空闲状态下发送测试帧的数量是不是超过上限(2次)
                /*
                if (outStandingTestFRConMessages > 2)
                {
                    DebugLog("Timeout for TESTFR_CON message");

                    // close connection
                    return false;
                }
                */

                // 发送链路测试请求帧
                socket.Send(TESTFR_ACT_MSG);
                // 显示发送报文的信息
                ShowMessage.Show(TESTFR_ACT_MSG, TESTFR_ACT_MSG.Length, "发送");
                LogHelper.Info(typeof(Connection), "发送报文：" + UtilHelper.ListToString(TESTFR_ACT_MSG));
                // 连接信息的统计对象的发送信息数量加1
                statistics.SentMsgCounter++;
                // 打印输出信息
                DebugLog("U message T3 timeout");

                // 设置发送或测试APDU的超时时间
                uMessageTimeout = (UInt64)currentTime + (UInt64)(parameters.T1 * 1000);
                // 长期空闲状态下发送测试帧的数量自增1
                outStandingTestFRConMessages++;
                // 重置长期空闲状态下发送测试帧的超时(T3)
                ResetT3Timeout();
            }

            #endregion 长期空闲状态下发送测试帧的超时

            #region 无数据报文时确认的超时(T2,T2<T1)

            // 判断接收方接收到不确认I格式的报文数量(最大值是w)是否大于零,且是否无数据报文时确认的超时(T2,T2<T1)
            if (unconfirmedReceivedIMessages > 0 && checkConfirmTimeout((long)currentTime))
            {
                // 设置最近发送的确认报文时间戳(用于检测无数据报文时确认的超时T2)
                lastConfirmationTime = (long)currentTime;
                // 设置接收方接收到不确认I格式的报文数量(最大值是w)为0
                unconfirmedReceivedIMessages = 0;
                // send confirmation message
                SendSMessage();
            }

            #endregion 无数据报文时确认的超时(T2,T2<T1)

            #region 发送或测试APDU的超时时间(T1)

            // 发送或测试APDU的超时时间(T1)不等于零,且小于当前时间
            if (uMessageTimeout != 0 && uMessageTimeout < currentTime)
            {
                DebugLog("U message T1 timeout");
                //throw new ConnectionException("SocketException", new SocketException(10060));
                return false;
            }

            // check if counterpart confirmed I messages
            // 给发送报文的k缓冲区加锁(用于线程同步)
            lock (sentASDUs)
            {
                // 最早发送的未接收到确认的报文时间到当前时间的间隔是否超时
                if (oldestSentASDU != -1 && oldestSentASDU != newestSentASDU && ((long)currentTime - sentASDUs[oldestSentASDU].sentTime) >= (parameters.T1 * 1000))
                {
                    DebugLog("最早发送的未接收到确认的报文时间到当前时间的间隔超时" + oldestSentASDU + "---" + currentTime + "---" + sentASDUs[oldestSentASDU].sentTime + "---" + parameters.T1);
                    return false;
                }
            }

            #endregion 发送或测试APDU的超时时间(T1)0

            return true;
        }

        /// <summary>
        /// 套接字连接
        /// </summary>
        /// <returns></returns>
        private bool InitConnection()
        {
            // 设置连接状态为true
            connecting = true;
            try
            {
                // Connect to a remote device.
                ConnectSocketWithTimeout();
                Console.WriteLine("--------------Connect to a remote device success---------------");
                DebugLog("Socket connected to " + socket.RemoteEndPoint.ToString());

                // 判断自动发送启动帧报文标志
                if (autostart)
                {
                    // 发送链路启动请求帧
                    socket.Send(STARTDT_ACT_MSG);

                    // 显示接收到的报文信息
                    ShowMessage.Show(STARTDT_ACT_MSG, STARTDT_ACT_MSG.Length, "发送");
                    LogHelper.Info(typeof(Connection), "发送报文：" + UtilHelper.ListToString(STARTDT_ACT_MSG));
                    // 连接信息的统计对象的发送报文数量加1
                    statistics.SentMsgCounter++;
                }

                // 设置网口运行状态为true
                running = true;
                // 设置socket错误标志位false
                socketError = false;
                // 设置网口连接状态为false
                connecting = false;

                // 网口连接状态处理
                if (connectionHandler != null)
                {
                    connectionHandler(connectionHandlerParameter, ConnectionEvent.OPENED);
                }
                return true;
            }
            catch (Exception se)
            {
                DebugLog("套接字连接异常,SocketException: " + se.ToString());
                lastException = se;
            }
            running = false;
            socketError = true;
            return false;
        }

        /// <summary>
        /// 关闭socket连接
        /// </summary>
        private void CloseConnection()
        {
            LogHelper.Warn(typeof(Connection), "关闭网络连接");
            DebugLog("CLOSE CONNECTION!");
            // 关闭Socket连接并释放所有关联的资源
            socket.Close();
            // 网口连接状态设置为false
            connecting = false;
            // 判断网口连接状态
            if (connectionHandler != null)
            {
                connectionHandler(connectionHandlerParameter, ConnectionEvent.CLOSED);
            }
        }

        /// <summary>
        /// 退出网口运行的run线程
        /// </summary>
        private void AbortRun()
        {
            System.Threading.Thread.CurrentThread.Abort();
            LogHelper.Info(typeof(Connection), "关闭Socket连接并释放所有关联的资源");
        }

        /// <summary>
        /// 运行网络连接，收取报文并解析
        /// </summary>
        private void Run()
        {
            LogHelper.Info(typeof(Connection), "后台网络线程开始运行");
            try
            {
                // 建立连接
                if (!InitConnection())
                {
                    LogHelper.Fatal(typeof(Connection), "网路连接初始化失败");
                    MessageBox.Show("网路连接失败", "警告");
                    // 退出当前网络线程
                    AbortRun();
                    return;
                }

                // 处理连接
                // loop until error
                HandleConnection();

                // 断开连接
                CloseConnection();
            }
            catch (Exception e)
            {
                LogHelper.Fatal(typeof(Connection), e);
            }
            LogHelper.Warn(typeof(Connection), "后台网络线程停止运行");

            // 关闭 System.Net.Sockets.Socket 连接并释放所有关联的资源
            AbortRun();
        }

        /// <summary>
        /// 连接处理
        /// </summary>
        private void HandleConnection()
        {
            LogHelper.Info(typeof(Connection), "开始处理网络报文");
            // 通道监视报文接受缓冲区
            // 接收报文的缓冲区字节数组
            byte[] bytes = new byte[300];
            byte[] bytesForChannelMonitor = new byte[300];
            List<byte> ListForChannelMonitor = new List<byte>();
            socket.ReceiveTimeout = 1000;

            while (running)
            {
                bool suspendThread = true;
                bool isMonitering = System.Threading.Interlocked.Read(ref MainViewModel.ChannelMonitorListening) != 0;
                // LogHelper.Info(typeof(Connection), "isMonitering:" + isMonitering);
                if (isMonitering)
                {
                    try
                    {
                        // 端口监视模式下，报文的接收与解析
                        if (!MonitorMessage(bytesForChannelMonitor, ref ListForChannelMonitor))
                        {
                            LogHelper.Warn(typeof(Connection), "端口监视模式下，报文的接收与解析失败MonitorMessage()");
                            running = false;
                        }
                    }
                    catch (SocketException e)
                    {
                        if (e.ErrorCode == 10060)
                        {
                        }
                        else
                        {
                            LogHelper.Fatal(typeof(Connection), "Exception:" + e.Message);
                            testFrameTimerForChannelMonitor.Stop();
                        }
                    }
                }
                else
                {
                    #region 非端口监视模式下，网口数据的接收与解析
                    try
                    {
                        if (!ParseMessage(bytes))
                        {
                            LogHelper.Warn(typeof(Connection), "非端口监视模式下，报文的接收与解析失败ParseMessage()");
                            running = false;
                        }
                    }
                    catch (SocketException e)
                    {
                        if (e.ErrorCode == 10060)
                        {
                        }
                        else
                        {
                            LogHelper.Warn(typeof(Connection), "非端口监视模式下，网口数据的接收与解析失败" + e.Message);
                            running = false;
                        }
                    }

                    if (!HandleTimeouts())
                    {
                        // 循环运行标志设置为false
                        running = false;
                        DebugLog("普通模式 网络连接超时");
                        LogHelper.Warn(typeof(Connection), "普通模式 网络连接超时");
                    }
                    // 判断是否使用报文发送队列标志和是否可以发送下一个等待的ASDU应用服务数据单元
                    if (useSendMessageQueue && SendNextWaitingASDU())
                    {
                        // 设置挂起线程标志为false
                        suspendThread = false;
                    }
                    // 判断挂起线程标志
                    if (suspendThread)
                    {
                        //LogHelper.Warn(typeof(Connection), "suspendThread 10ms");
                        Thread.Sleep(10);
                    }
                    #endregion 非端口监视模式下，网口数据的接收与解析
                }
            }
            LogHelper.Fatal(typeof(Connection), "Quit HandleConnection()");
        }

        /// <summary>
        /// 关闭socket
        /// </summary>
        public void Close()
        {
            // 判断网口运行状态标志
            if (socket != null && running)
            {
                // 禁用Socket的发送和接收
                socket.Shutdown(SocketShutdown.Both);

                // 循环判断网口运行状态标志
                while (running)
                {
                    Thread.Sleep(1);
                }
            }
        }

        /// <summary>
        /// 接收的ASDU应用服务数据单元处理Handler
        /// </summary>
        /// <param name="handler">接收到的ASDU信息处理Handler(委托)</param>
        /// <param name="parameter">接收到的ASDU信息处理Handler参数</param>
        public void SetASDUReceivedHandler(ASDUReceivedHandler handler, object parameter)
        {
            this.asduReceivedHandler = handler;
            this.asduReceivedHandlerParameter = parameter;
        }

        /// <summary>
        /// Sets the connection handler. The connection handler is called when
        /// the connection is established or closed
        /// </summary>
        /// <param name="handler">the handler to be called</param>
        /// <param name="parameter">user provided parameter that is passed to the handler</param>
        public void SetConnectionHandler(ConnectionHandler handler, object parameter)
        {
            this.connectionHandler = handler;
            this.connectionHandlerParameter = parameter;
        }

        /***********************************************************************************************************************/
        /***********************************************监视功能相关程序部分****************************************************/
        /**********************************************************************************************************************/
        /// <summary>
        /// 通道监视网口接受数据
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="buffer">接受的字节缓冲数组</param>
        /// <returns>返回接受的字节数组个数</returns>
        private int ReceiveMessageForChannelMonitor(RedefineSocket socket, byte[] buffer)
        {
            int len = socket.ReceiveByListeningModel(buffer, SocketFlags.None);
            LogHelper.Info(typeof(Connection), "ReceiveMessageForChannelMonitor() 接收" + len + "字节 "
                            + "接收的报文：" + BitConverter.ToString(buffer).Replace("-", string.Empty));
            return len;
        }

        /// <summary>
        /// 解析网口收到的报文
        /// </summary>
        /// <param name="buffer">接收缓冲区</param>
        /// <returns></returns>
        private bool CheckMessageForChannelMonitor(ref List<byte> buffer)
        {
            LogHelper.Info(typeof(Connection), "开始解析通道监视收到的报文:" + BitConverter.ToString(buffer.ToArray()).Replace("-", string.Empty));
            // 接收到的字节数可构成101最小完整报文
            while (buffer.Count > 5)
            {
                LogHelper.Info(typeof(Connection), "接收到的字节数可构成101最小完整报文");
                // 查找buffer中的 AA 55 或者68 或者 10 或者 11（有效报文的起始字节）
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
                                    SerialPortService.DealUnkownFrame(ref buffer, i);
                                    // 重新开始循环判定剩余的字节
                                    break;
                                }

                                // AA 55前面没有未知报文,即为起始字节
                                // AA 55后面为68
                                if (buffer[2] == 0x68)
                                {
                                    LogHelper.Info(typeof(Connection), "AA 55后面为68");
                                    // 101长帧报文报文头正确
                                    if (buffer[2] == 0x68 && buffer[5] == 0x68 && buffer[3] == buffer[4])
                                    {
                                        LogHelper.Info(typeof(Connection), "AA 55 101长帧报文报文头正确");
                                        //报文没收完整
                                        if (buffer.Count < buffer[3] + 8)
                                        {
                                            LogHelper.Error(typeof(Connection), "通道监视网口收到不完整的报文");
                                            return true;
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
                                                SerialPortService.DealAA55(ref buffer);
                                                List<byte> oneLongFrame = new List<byte>();
                                                for (int k = 0; k < buffer[1] + 6; ++k)
                                                {
                                                    oneLongFrame.Add(buffer[k]);
                                                }
                                                LogHelper.Info(typeof(Connection), "处理正确的长帧报文");
                                                // 处理正确的长帧报文
                                                SerialPortService.VariableFrameProcessingForChannelMonitor(oneLongFrame);
                                                buffer.RemoveRange(0, buffer[1] + 6);
                                                break;
                                            }
                                            else
                                            {
                                                LogHelper.Info(typeof(Connection), "校验和错误");
                                                // 校验和错误
                                                SerialPortService.DealUnkownFrame(ref buffer, buffer[3] + 8);
                                            }
                                        }
                                        else
                                        {
                                            LogHelper.Info(typeof(Connection), "结束符不正确");
                                            // 结束符不正确
                                            SerialPortService.DealUnkownFrame(ref buffer, buffer[3] + 8);
                                        }
                                    }
                                    else
                                    {
                                        LogHelper.Info(typeof(Connection), "101长帧报文头不正确,将AA 55连同报文头一起加入错误报文List");
                                        // 101长帧报文头不正确,将AA 55连同报文头一起加入错误报文List
                                        SerialPortService.DealUnkownFrame(ref buffer, 6);
                                    }
                                }
                                else if (buffer[2] == 0x10)
                                {
                                    LogHelper.Info(typeof(Connection), "AA55短帧");
                                    // 短帧未收取完整
                                    if (buffer.Count < 9)
                                    {
                                        LogHelper.Info(typeof(Connection), "AA55短帧未收取完整");
                                        return true;
                                    }
                                    // 101短帧报文校验和
                                    byte sum = (byte)(buffer[3] + buffer[4] + buffer[5]);
                                    // 校验和正确并且结束符正确
                                    if (sum == buffer[6] && buffer[7] == 0x16)
                                    {
                                        LogHelper.Info(typeof(Connection), "AA55101短帧报文校验和正确并且结束符正确");
                                        // 从buffer中将AA55删除
                                        SerialPortService.DealAA55(ref buffer);
                                        //复制一帧有效的101短帧报文到oneFrame中
                                        List<byte> oneShortFrame = new List<byte>();
                                        for (int n = 0; n < 6; ++n)
                                        {
                                            oneShortFrame.Add(buffer[n]);
                                        }
                                        LogHelper.Info(typeof(Connection), "处理有效报文帧");
                                        //处理有效报文帧
                                        SerialPortService.FixedFrameProcessingForChannelMonitor(oneShortFrame);
                                        buffer.RemoveRange(0, 6);
                                        break;
                                    }
                                    else
                                    {
                                        LogHelper.Info(typeof(Connection), "无效短帧报文");
                                        // 无效短帧报文
                                        SerialPortService.DealUnkownFrame(ref buffer, 8);
                                    }
                                }
                                else
                                {
                                    LogHelper.Info(typeof(Connection), "AA 55后面既不是68 也不是10 ");
                                    // AA 55后面既不是68 也不是10 
                                    continue;
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
                            return true;
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
                            SerialPortService.DealUnkownFrame(ref buffer, i);
                            // 重新开始循环判定剩余的字节
                            break;
                        }
                        // 以68开始的报文处理
                        if (buffer[0] == 0x68 && buffer[3] == 0x68 && buffer[1] == buffer[2])
                        {
                            LogHelper.Info(typeof(Connection), "以68开始的报文处理");
                            // 报文没收完整
                            if (buffer.Count < buffer[1] + 6)
                            {
                                LogHelper.Info(typeof(Connection), "报文没收完整");
                                return true;
                            }
                            // 报文结束符正确
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
                                    SerialPortService.DirForChannelMonitor = "接收";
                                    SerialPortService.VariableFrameProcessingForChannelMonitor(oneLongFrame);
                                    buffer.RemoveRange(0, buffer[1] + 6);
                                    break;
                                }
                            }
                            else
                            {
                                LogHelper.Info(typeof(Connection), "校验和不对");
                                // 校验和不对
                                SerialPortService.DealUnkownFrame(ref buffer, buffer[1] + 6);
                                break;
                            }
                        }
                        else
                        {
                            LogHelper.Info(typeof(Connection), "报文头不对");
                            // 报文头不对
                            SerialPortService.DealUnkownFrame(ref buffer, 4);
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
                            SerialPortService.DealUnkownFrame(ref buffer, i);
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
                            SerialPortService.DirForChannelMonitor = "接收";
                            SerialPortService.FixedFrameProcessingForChannelMonitor(oneShortFrame);
                            buffer.RemoveRange(0, 6);
                            break;
                        }
                        else
                        {
                            LogHelper.Info(typeof(Connection), "无效短帧报文");
                            // 无效短帧报文
                            SerialPortService.DealUnkownFrame(ref buffer, 6);
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
                            SerialPortService.DealUnkownFrame(ref buffer, i);
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

                            //    //发送测试帧的定时器启动
                            //    testFrameTimerForChannelMonitor = new System.Timers.Timer(5000);
                            //    testFrameTimerForChannelMonitor.AutoReset = true;
                            //    testFrameTimerForChannelMonitor.Enabled = true;
                            //    testFrameTimerForChannelMonitor.Elapsed += new System.Timers.ElapsedEventHandler(TimerOut);
                            //    isTiming = true;

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
                                //isReceiveTestFrameResponse = true;
                            }
                            else if(buffer[1] == 0x01)
                            {
                                LogHelper.Info(typeof(Connection), "FTU响应测试帧");
                            }
                            else if (buffer[1] == 0x0C)
                            {
                                LogHelper.Info(typeof(Connection), "停止监听帧回复");
                                // 停止监听帧回复
                                //通道监视标志置0,停止测试帧的计时器
                                testFrameTimerForChannelMonitor.Stop();
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
                            LogHelper.Info(typeof(Connection), "校验和错误，删除字节");
                            // 校验和错误，删除字节
                            SerialPortService.DealUnkownFrame(ref buffer, 6);
                            break;
                        }
                    }
                    else
                    {
                        // 非有效起始报文，即非 AA 68 10 11，将该字节存入未知报文中，并从待解析报文中删除
                        SerialPortService.DealUnkownFrame(ref buffer, 1);
                        // 返回while循环，重新解析剩余报文
                        break;
                    }
                } // for
            } // while
            // 接收到的字节数不可构成最小101完整报文
            LogHelper.Info(typeof(Connection), "网络监听报文长度不满足最小要求，count:" + buffer.Count);
            return true;
        }

        /// <summary>
        /// 链路测试计时时间到
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void TimerOut(object source, System.Timers.ElapsedEventArgs e)
        {
            // 判断是否有接收到任意报文
            if (isReceiveFrameResponse)
            {
                isReceiveFrameResponse = false;
                Messenger.Default.Send<string>("testFrameTimeOut", "TestFrameTimeOut");
            }
            else if (!isReceiveFrameResponse)
            {
                // 累加未收到报文的次数
                unrecieveFrameTime = unrecieveFrameTime + 1;
                // 6秒未收到任何报文
                if (unrecieveFrameTime  == 3)
                {
                    LinkDisconnectOnChannelMonitor();
                }
            }
        }

        /// <summary>
        /// 通道监视链路断开
        /// </summary>
        private void LinkDisconnectOnChannelMonitor()
        {
            // 未收到任何报文，关闭计时器
            testFrameTimerForChannelMonitor.Stop();
            // 提示用户
            MessageBox.Show("通道监视链路连接超时，请关闭通道监视窗口", "警告");
            // 关闭网口连接
            running = false;
        }
    }
}

