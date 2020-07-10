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
    /// ConnectionException ��ժҪ˵��
    /// author: songminghao
    /// date��2017/12/07 13:34:09
    /// desc�����������쳣��
    /// version: 1.0
    /// </summary>
    [Serializable]
    public class ConnectionException : Exception
    {
        /// <summary>
        /// �вι��췽��
        /// </summary>
        /// <param name="message">�쳣��ʾ��Ϣ</param>
        public ConnectionException(string message)
            : base(message)
        {
            MainViewModel.outputdata.Debug += message + "\n";
            CommunicationViewModel.con.Close();
        }

        /// <summary>
        /// �вι��췽��
        /// </summary>
        /// <param name="message">�쳣��ʾ��Ϣ</param>
        /// <param name="e">�쳣����</param>
        public ConnectionException(string message, Exception e)
            : base(message, e)
        {
            MainViewModel.outputdata.Debug += message + e.ToString() + "\n";
            CommunicationViewModel.con.Close();
        }
    }

    /// <summary>
    /// ���������¼�ö��
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
    /// <param name="parameter">����</param>
    /// <param name="asdu">ASDU</param>
    /// <returns></returns>
    public delegate bool ASDUReceivedHandler(object parameter, ASDU asdu);

    /// <summary>
    /// ��������handler
    /// </summary>
    /// <param name="parameter">����</param>
    /// <param name="connectionEvent">���������¼�ö��</param>
    public delegate void ConnectionHandler(object parameter, ConnectionEvent connectionEvent);

    /// <summary>
    /// Raw message handler. Can be used to access the raw message.
    /// Returns true when message should be handled by the protocol stack, false, otherwise.
    /// </summary>
    /// <param name="parameter">����</param>
    /// <param name="message">������Ϣ��</param>
    /// <param name="messageSize">������Ϣ����</param>
    /// <returns></returns>
    public delegate bool RawMessageHandler(object parameter, byte[] message, int messageSize);

    /// <summary>
    /// Connection ��ժҪ˵��
    /// author: songminghao
    /// date��2017/12/07 13:42:09
    /// desc������������
    /// version: 1.0
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// ��·��������֡
        /// </summary>
        public static byte[] STARTDT_ACT_MSG = new byte[] { 0x68, 0x04, 0x07, 0x00, 0x00, 0x00 };

        /// <summary>
        /// ��·����ȷ��֡
        /// </summary>
        public static byte[] STARTDT_CON_MSG = new byte[] { 0x68, 0x04, 0x0B, 0x00, 0x00, 0x00 };

        /// <summary>
        /// ��·ֹͣ����֡
        /// </summary>
        public static byte[] STOPDT_ACT_MSG = new byte[] { 0x68, 0x04, 0x13, 0x00, 0x00, 0x00 };

        /// <summary>
        /// ��·��������֡
        /// </summary>
        public static byte[] TESTFR_ACT_MSG = new byte[] { 0x68, 0x04, 0x43, 0x00, 0x00, 0x00 };

        /// <summary>
        /// ��·����ȷ��֡
        /// </summary>
        public static byte[] TESTFR_CON_MSG = new byte[] { 0x68, 0x04, 0x83, 0x00, 0x00, 0x00 };

        /// <summary>
        /// ���յ���ASDU��Ϣ����Handler(ί��)
        /// </summary>
        private ASDUReceivedHandler asduReceivedHandler = null;

        /// <summary>
        /// ���յ���ASDU��Ϣ����Handler����
        /// </summary>
        private object asduReceivedHandlerParameter = null;

        /// <summary>
        /// ��������Handler(ί��)
        /// </summary>
        private ConnectionHandler connectionHandler = null;

        /// <summary>
        /// ��������Handler����
        /// </summary>
        private object connectionHandlerParameter = null;

        /// <summary>
        /// ���ͱ��ĵ����к�
        /// </summary>
        private int sendSequenceNumber;

        /// <summary>
        /// ���úͻ�ȡ���ͱ��ĵ����к�.Gets or sets the send sequence number N(S). WARNING: For test purposes only! Do net set
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
        /// ���ձ��ĵ����к�
        /// </summary>
        private int receiveSequenceNumber;

        /// <summary>
        /// ���úͻ�ȡ���ձ��ĵ����к�.Gets or sets the receive sequence number N(R). 
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
        /// ���ͻ����APDU�ĳ�ʱʱ��(T1)
        /// </summary>
        private UInt64 uMessageTimeout = 0;

        #region k���������Ľṹ�嶨�弰��ز�������

        /// <summary>
        /// k���������Ľṹ��(����ʱ��ͷ������к�).data structure for k-size sent ASDU buffer
        /// </summary>
        private struct SentASDU
        {
            public long sentTime; // required for T1 timeout
            public int seqNo;
        }

        /// <summary>
        /// δȷ��֡��������,����k��Ӧ.maximum number of ASDU to be sent without confirmation - parameter k
        /// </summary>
        public static int maxSentASDUs;

        /// <summary>
        /// k�����������ȷ��͵ı�������.index of oldest entry in k-buffer
        /// </summary>
        public static int oldestSentASDU = -1;

        /// <summary>
        /// k��������������͵ı���������1(����Ҫ���͵���һ֡���ĵķ������к�).index of newest entry in k-buffer
        /// </summary>
        public static int newestSentASDU = -1;

        /// <summary>
        /// ���ͱ��ĵ�k������.the k-buffer 
        /// </summary>
        private SentASDU[] sentASDUs = null;

        #endregion k���������Ľṹ�嶨�弰��ز�������

        /// <summary>
        /// �ȴ����͵�ASDU����
        /// </summary>
        private Queue<ASDU> waitingToBeSent = null;

        /// <summary>
        /// ʹ�ñ��ķ��Ͷ��б�־
        /// </summary>
        private bool useSendMessageQueue = true;

        /// <summary>
        /// ���úͻ�ȡʹ�ñ��ķ��Ͷ��б�־.Gets or sets a value indicating whether this <see cref="lib60870.Connection"/> use send message queue.
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
        /// ���ڿ���״̬�·��Ͳ���֡�ĳ�ʱ���趨ֵΪ20s
        /// </summary>
        private UInt64 nextT3Timeout;

        /// <summary>
        /// ���ڿ���״̬�·��Ͳ���֡������
        /// </summary>
        private int outStandingTestFRConMessages = 0;

        /// <summary>
        /// ���շ����յ���ȷ��I��ʽ�ı�������(���ֵ��w).number of unconfirmed messages received
        /// </summary>
        private int unconfirmedReceivedIMessages;

        /// <summary>
        /// ������͵�ȷ�ϱ���ʱ���(���ڼ�������ݱ���ʱȷ�ϵĳ�ʱT2).timestamp when the last confirmation message was sent
        /// </summary>
        private long lastConfirmationTime;

        /// <summary>
        /// socket����
        /// </summary>
        public static RedefineSocket socket;

        /// <summary>
        /// �Զ���������֡���ı�־
        /// </summary>
        private bool autostart = true;

        /// <summary>
        /// ���úͻ�ȡ�Զ���������֡���ı�־.
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
        /// ������
        /// </summary>
        private string hostname;

        /// <summary>
        /// TCP�˿ں�
        /// </summary>
        private int tcpPort;

        /// <summary>
        /// ��������״̬��־
        /// </summary>
        public static bool running = false;

        /// <summary>
        /// ��ȡ��������״̬��־
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return running;
            }
        }

        /// <summary>
        /// ��������״̬
        /// </summary>
        private bool connecting = false;

        /// <summary>
        /// socket�����־
        /// </summary>
        private bool socketError;

        /// <summary>
        /// �Ƿ��ǵ�һ�����յ���I֡���ı�־
        /// </summary>
        private bool firstIMessageReceived = false;

        /// <summary>
        /// ��һ���쳣����
        /// </summary>
        private Exception lastException;

        /// <summary>
        /// �Ƿ��ӡ�����Ϣ��־
        /// </summary>
        private bool debugOutput = false;

        /// <summary>
        /// ���úͻ�ȡ�Ƿ��ӡ�����Ϣ��־
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
        /// ����״̬������
        /// </summary>
        private static int connectionCounter = 0;

        /// <summary>
        /// ����״̬ID
        /// </summary>
        private int connectionID;

        /// <summary>
        /// ����������Ϣ��ͳ�ƶ���
        /// </summary>
        public static ConnectionStatistics statistics = new ConnectionStatistics();

        /// <summary>
        /// �������ӵĳ�ʱʱ�䣬��ʼ��Ϊ1��
        /// </summary>
        private int connectTimeoutInMs = 1000;

        /// <summary>
        /// ���Ӳ�������
        /// </summary>
        private ConnectionParameters parameters;

        // ����ͨ�����ӽ��ձ��ĵ�ʶ���־λ
        private const byte firstSendByte = 0xAA;
        private const byte secondSendByte = 0x55;

        // ���Ӵ��ڲ���֡�Ķ�ʱ��
        public System.Timers.Timer testFrameTimerForChannelMonitor;

        // ���Ӵ��ڲ���֡�Ķ�ʱ����ʱ�Ƿ�ʼ��־
        bool isTiming = false;

        // �Ƿ��յ���һ������֡�Ļظ�����
        // bool isReceiveTestFrameResponse = true;

        // �Ƿ��յ���һ�����ⱨ��
        bool isReceiveFrameResponse = true;

        // δ�յ����ĵĳ�ʱ����
        UInt32 unrecieveFrameTime = 0;

        /// <summary>
        /// ���úͻ�ȡ����������
        /// </summary>
        public ConnectionParameters Parameters
        {
            get
            {
                return this.parameters;
            }
        }

        /// <summary>
        /// ��ӡ�����Ϣ
        /// </summary>
        /// <param name="message">�����Ϣ</param>
        public void DebugLog(string message)
        {
            if (debugOutput)
            {
                Console.WriteLine("CS104 MASTER CONNECTION " + connectionID + ": " + message);
                MainViewModel.outputdata.Debug += "CS104 MASTER CONNECTION " + connectionID + ": " + message + "\n";
            }

        }

        /// <summary>
        /// ��������
        /// </summary>
        private void ResetConnection()
        {
            // ���÷��ͱ��ĵ����к�
            sendSequenceNumber = 0;
            // ���ý��ձ��ĵ����к�
            receiveSequenceNumber = 0;
            // ����δȷ�ϵ��ѽ��յ��ı�������
            unconfirmedReceivedIMessages = 0;
            // ����������͵�ȷ�ϱ���ʱ���
            lastConfirmationTime = System.Int64.MaxValue;
            // �����Ƿ��ǵ�һ�����յ���I֡���ı�־
            firstIMessageReceived = false;

            outStandingTestFRConMessages = 0;

            uMessageTimeout = 0;

            // ����socket�����־
            socketError = false;
            // ������һ���쳣����
            lastException = null;

            // ����δȷ��֡��������,����k��Ӧ
            maxSentASDUs = parameters.K + 1;
            // ����k�����������ȷ��͵ı�������
            oldestSentASDU = -1;
            // ����k�������м���Ҫ���͵ı�������
            newestSentASDU = -1;
            // ��ʼ�����ͱ��ĵ�k������
            sentASDUs = new SentASDU[maxSentASDUs];

            // �ж�ʹ�ñ��ķ��Ͷ��б�־
            if (useSendMessageQueue)
            {
                // ��ʼ���ȴ����͵�ASDU����
                waitingToBeSent = new Queue<ASDU>();
            }

            // ��������ͳ�ƶ��������Ϣ
            statistics.Reset();
        }

        /// <summary>
        /// ����S֡����
        /// </summary>
        private void SendSMessage()
        {
            // ���巢��S֡�����ֽ�����
            byte[] msg = new byte[6];

            // �����ַ�68H
            msg[0] = 0x68;
            // APDU����(���253)
            msg[1] = 0x04;
            // �������λλ��1
            msg[2] = 0x01;
            // �������λλ��2
            msg[3] = 0x00;

            // �������λλ��3:�������к�
            msg[4] = (byte)((receiveSequenceNumber % 128) * 2);
            // �������λλ��4:�������к�
            msg[5] = (byte)(receiveSequenceNumber / 128);

            // ��S֡�������ݷ��͵����ӵ� System.Net.Sockets.Socket
            socket.Send(msg);

            // ��ʾ���͵ı�����Ϣ
            ShowMessage.Show(msg, msg.Length, "����");
            LogHelper.Info(typeof(Connection), "���ͱ��ģ�" + UtilHelper.ListToString(msg));
            // ����������Ϣ��ͳ�ƶ����з��ͱ���������1
            statistics.SentMsgCounter++;
        }

        /// <summary>
        /// �������к��Ƿ�Ϸ���Ч.check if received sequence number is valid
        /// </summary>
        /// <param name="seqNo">���к�</param>
        /// <returns></returns>
        private bool CheckSequenceNumber(int seqNo)
        {
            // DebugLog("���Ⱥ��������Ҫ���͵ı�������" + oldestSentASDU + ":" + sentASDUs[oldestSentASDU].seqNo + "-----------" + newestSentASDU + ":" + sentASDUs[newestSentASDU].seqNo + "----����֤���ձ��ĵĽ������к�" + seqNo);
            // �����ͱ��ĵ�k����������(�����߳�ͬ��)
            lock (sentASDUs)
            {
                // ���к���Ч��־,��ʼ��Ϊfalse,��Ч
                bool seqNoIsValid = false;
                // �������������־,��ʼ��Ϊfalse,δ���
                bool counterOverflowDetected = false;

                // �ж�k�����������ȷ��͵ı���������Ϊ-1��ʾk������Ϊ��
                if (oldestSentASDU == -1)
                {
                    // if k-Buffer is empty
                    if (seqNo == sendSequenceNumber)
                    {
                        // �������к���Ч��־Ϊtrue,��Ч
                        seqNoIsValid = true;
                    }
                }
                else
                {
                    DebugLog("���Ⱥ��������Ҫ���͵ı�������" + oldestSentASDU + ":" + sentASDUs[oldestSentASDU].seqNo + "-----------" + newestSentASDU + ":" + sentASDUs[newestSentASDU].seqNo + "----����֤���ձ��ĵĽ������к�" + seqNo);
                    // k��������Ϊ��

                    // Two cases are required to reflect sequence number overflow
                    // �ж����ȷ��ͺͼ������͵ı������к�:�������͵ı������кŴ��ڵ������ȷ��͵ı������к�.
                    if (sentASDUs[oldestSentASDU].seqNo <= sentASDUs[newestSentASDU].seqNo)
                    {
                        // �жϴ�������к��Ƿ������ȷ��ͺͼ������͵ı������к�֮��
                        if (((seqNo >= sentASDUs[oldestSentASDU].seqNo) && (seqNo <= sentASDUs[newestSentASDU].seqNo)))
                        {
                            // �������к���Ч��־Ϊtrue,��Ч
                            seqNoIsValid = true;
                        }
                    }
                    else
                    {
                        // ������͵ı������к�С�����ȷ��͵ı������к�

                        // �жϴ�������к��Ƿ������ȷ��͵ı������к�֮��,���ڼ������͵ı������к�֮ǰ
                        if ((seqNo >= sentASDUs[oldestSentASDU].seqNo) || (seqNo <= sentASDUs[newestSentASDU].seqNo))
                        {
                            // �������к���Ч��־Ϊtrue,��Ч
                            seqNoIsValid = true;
                        }

                        // �������������־,����Ϊtrue,���
                        counterOverflowDetected = true;
                    }

                    // ��ȡ���ȷ��͵�S֡���ĵ�ǰһ�����͵ı������к�
                    int latestValidSeqNo = (sentASDUs[oldestSentASDU].seqNo - 1) % 32768;

                    // ���ǰһ���������кŵ��ڴ���ⱨ�����к�
                    if (latestValidSeqNo == seqNo)
                    {
                        // �������к���Ч��־Ϊtrue,��Ч
                        seqNoIsValid = true;
                    }

                }

                // �ж����к���Ч��־,��Ϊfalse,��Ч
                if (seqNoIsValid == false)
                {
                    // ��ӡ���������Ϣ
                    DebugLog("Received sequence number out of range");
                    // ����false
                    return false;
                }

                // �ж�k�����������ȷ��͵ı�������,��Ϊ-1��ʾk�������ǿ�
                if (oldestSentASDU != -1)
                {
                    do
                    {
                        // �жϼ������������־,false��ʾδ���
                        if (counterOverflowDetected == false)
                        {
                            // �жϴ�������к��ǲ��������ȷ��͵ı������к�֮ǰ
                            if (seqNo < sentASDUs[oldestSentASDU].seqNo)
                            {
                                break;
                            }
                        }
                        else
                        {
                            // �жϴ�������к��ǲ������ȷ��͵ı������кŵ�ǰһ�����͵ı������к�
                            if (seqNo == ((sentASDUs[oldestSentASDU].seqNo - 1) % 32768))
                            {
                                break;
                            }
                        }

                        // �����ȷ��͵ı������к�������1��Ϊ�µ����ȷ��͵ı������к�����
                        oldestSentASDU = (oldestSentASDU + 1) % maxSentASDUs;

                        // �������͵ı������к�������1
                        int checkIndex = (newestSentASDU + 1) % maxSentASDUs;

                        // �ж��µ����ȷ��͵ı������к����������ȷ��͵ı������к�������1�������
                        if (oldestSentASDU == checkIndex)
                        {
                            // ��ȣ���ʾk������Ϊ��,�������ȷ��͵ı������к�����Ϊ-1
                            oldestSentASDU = -1;
                            break;
                        }

                        // �ж����ȷ��͵ı������к��ǲ��ǵ��ڴ�������к�
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
        /// �жϷ��ͻ������ǲ�������
        /// </summary>
        /// <returns></returns>
        public static bool IsSentBufferFull()
        {
            // ��ȡk�������м������͵ı���������1
            int newIndex = (newestSentASDU + 1) % maxSentASDUs;
            // �ж��������Ƿ����k�����������ȷ��͵ı�������,���,���ʾk����������
            if (newIndex == oldestSentASDU)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ����I֡����
        /// </summary>
        /// <param name="asdu">ASDU����</param>
        /// <returns></returns>
        private int SendIMessage(ASDU asdu)
        {
            // ���巢��֡���ݶ���
            BufferFrame frame = new BufferFrame(new byte[260], 6);
            // ��ASDU���ݷ�װ������֡��
            asdu.Encode(frame, parameters);

            // ��ȡ����֡�ֽ�����
            byte[] buffer = frame.GetBuffer();

            //�˴��д���
            //int msgSize = frame.GetMsgSize () + 6; /* ASDU size + ACPI size */

            // ��ȡ����֡��С
            int msgSize = frame.GetMsgSize();

            // �����ַ�68H
            buffer[0] = 0x68;
            // ����APDU����.set size field
            buffer[1] = (byte)(msgSize - 2);

            // ���ñ��ķ������к�
            buffer[2] = (byte)((sendSequenceNumber % 128) * 2);
            buffer[3] = (byte)(sendSequenceNumber / 128);

            // ���ñ��Ľ������к�
            buffer[4] = (byte)((receiveSequenceNumber % 128) * 2);
            buffer[5] = (byte)(receiveSequenceNumber / 128);

            // �ж���������״̬
            if (running)
            {
                // ָ���� System.Net.Sockets.Socket �Ƿ�����ʹ�� Nagle �㷨
                socket.NoDelay = true;
                // ʹ��ָ���� System.Net.Sockets.SocketFlags,��ָ���ֽ��������ݷ��͵������ӵ� System.Net.Sockets.Socket
                socket.Send(buffer, msgSize, SocketFlags.None);
                // ������һ�����ͱ��ĵ����к�
                sendSequenceNumber = (sendSequenceNumber + 1) % 32768;
                // ����������Ϣ��ͳ�ƶ���ķ��ͱ���������1
                statistics.SentMsgCounter++;
                // ����δȷ�ϵ��ѽ��յ��ı�������Ϊ0
                unconfirmedReceivedIMessages = 0;

                // ��ʾ���ͱ�����Ϣ
                ShowMessage.Show(buffer, msgSize, "����");
                LogHelper.Info(typeof(Connection), "���ͱ��ģ�" + UtilHelper.ListToString(buffer, msgSize));

                // ���ط��ͱ��ĵ����к�
                return sendSequenceNumber;
            }
            else
            {
                // �ж���һ���쳣�Ƿ�Ϊ��
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
        /// ��ӡ���͵ı��Ļ�������Ϣ
        /// </summary>
        private void PrintSendBuffer()
        {
            // �ж�k�����������ȷ��͵ı�������,-1��ʾk������Ϊ��
            if (oldestSentASDU != -1)
            {
                // �����ȷ��͵ı�����������Ϊ��ǰ����
                int currentIndex = oldestSentASDU;

                // ����k��������һ�����ĵ�����Ϊ0
                int nextIndex = 0;

                DebugLog("------k-buffer------");

                do
                {
                    DebugLog(currentIndex + " : S " + sentASDUs[currentIndex].seqNo + " : time " + sentASDUs[currentIndex].sentTime);

                    // �жϵ�ǰ�����Ƿ���ڼ������͵ı�������
                    if (currentIndex == newestSentASDU)
                    {
                        // ���,��ʾk������Ϊ��,����k��������һ�����ĵ�����Ϊ-1
                        nextIndex = -1;
                    }

                    // ��ǰ������1
                    currentIndex = (currentIndex + 1) % maxSentASDUs;

                } while (nextIndex != -1);

                DebugLog("--------------------");

            }
        }

        /// <summary>
        /// ����I֡���Ĳ������ѷ��͵ı���������Ϣ
        /// </summary>
        /// <param name="asdu">ASDU����</param>
        private void SendIMessageAndUpdateSentASDUs(ASDU asdu)
        {
            // �����ͱ��ĵ�k����������(�����߳�ͬ��)
            lock (sentASDUs)
            {
                // �ж�k�����������ȷ��͵ı�������,-1��ʾk������Ϊ��
                if (oldestSentASDU == -1)
                {
                    // �������ȷ��͵ı�������Ϊ0
                    oldestSentASDU = 0;
                    // ���ü������͵ı�������Ϊ0
                    newestSentASDU = 0;
                }

                // ��ȡ��ǰ���ͱ��ĵ����к�
                int currentSendSeq = SendIMessage(asdu) - 1;

                // ���÷��ͱ��ĵ�k��������ǰ���ͱ��ĵ����к�
                sentASDUs[newestSentASDU].seqNo = currentSendSeq;
                // ���÷��ͱ��ĵ�k��������ǰ���ͱ��ĵķ���ʱ��
                sentASDUs[newestSentASDU].sentTime = SystemUtils.currentTimeMillis();

                // ���ü������͵ı�������
                newestSentASDU = (newestSentASDU + 1) % maxSentASDUs;

                // ���÷��ͱ��ĵ�k�������������ͱ��ĵ����к�
                sentASDUs[newestSentASDU].seqNo = currentSendSeq + 1;
                // ���÷��ͱ��ĵ�k�������������ͱ��ĵķ���ʱ��
                sentASDUs[newestSentASDU].sentTime = SystemUtils.currentTimeMillis();

                // ��ӡ���͵ı��Ļ�������Ϣ
                PrintSendBuffer();
            }

        }

        /// <summary>
        /// ������һ�����ڵȴ���ASDU����
        /// </summary>
        /// <returns>ASDU�����Ƿ�ɹ�,true��ʾ�ɹ�,false��ʾʧ��</returns>
        private bool SendNextWaitingASDU()
        {
            // ����ASDU��־,��ʼ��Ϊfalse
            bool sentAsdu = false;

            // �ж���������״̬
            if (running == false)
            {
                throw new ConnectionException("connection lost");
            }

            try
            {
                // ���ȴ����͵�ASDU���м���(�߳�ͬ��ʹ��)
                lock (waitingToBeSent)
                {
                    // �ȴ����͵�ASDU���зǿ�,��һֱѭ��
                    while (waitingToBeSent.Count > 0)
                    {
                        // �жϷ��ͻ�����(k������)�ǲ�������
                        if (IsSentBufferFull() == true)
                        {
                            // ��,������
                            break;
                        }

                        // ���ͻ�����(k������)����,���ȴ����͵�ASDU���ж�ͷԪ�س���
                        ASDU asdu = waitingToBeSent.Dequeue();

                        // asdu��Ϊ��
                        if (asdu != null)
                        {
                            // ����I֡���Ĳ������ѷ��͵ı���������Ϣ
                            SendIMessageAndUpdateSentASDUs(asdu);
                            // ���÷���ASDU��־Ϊtrue
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
                // ������������״̬Ϊfalse
                running = false;
                throw new ConnectionException("connection lost");
            }

            // ���ط���ASDU��־
            return sentAsdu;
        }

        /// <summary>
        /// ����ASDU����
        /// </summary>
        /// <param name="asdu">ASDU����</param>
        private void SendASDUInternal(ASDU asdu)
        {
            try
            {
                // �ж��ǲ��Ǵ��ڷ���
                if (SerialPortService.serialPort.IsOpen)
                {
                    CommunicationViewModel.serialPortSerice.SendVariableFrame(asdu);
                }
                else if (Connection.running)
                {
                    // ���ڷ���
                    // ��socket����(�߳�ͬ��ʹ��)
                    lock (socket)
                    {
                        // �ж���������״̬
                        if (running == false)
                        {
                            throw new ConnectionException("not connected", new SocketException(10057));
                        }

                        // �ж�ʹ�ñ��ķ��Ͷ��б�־
                        if (useSendMessageQueue)
                        {
                            // ���ȴ����͵�ASDU���м���(�߳�ͬ��ʹ��)
                            lock (waitingToBeSent)
                            {
                                // ��ASDU������ӵȴ����͵�ASDU����
                                waitingToBeSent.Enqueue(asdu);
                            }

                            // ������һ�����ڵȴ���ASDU����
                            SendNextWaitingASDU();
                        }
                        else
                        {
                            #region ��ʹ�ñ��ķ��Ͷ���

                            // �жϷ��ͻ�����(k������)�ǲ�������
                            if (IsSentBufferFull())
                            {
                                throw new ConnectionException("Flow control congestion. Try again later.");
                            }

                            // ����I֡���Ĳ������ѷ��͵ı���������Ϣ
                            SendIMessageAndUpdateSentASDUs(asdu);

                            #endregion ��ʹ�ñ��ķ��Ͷ���
                        }
                    }

                }
                else if (SerialPortService.serialPort.IsOpen == false || Connection.running == false)
                {
                    MessageBox.Show("��·�ѶϿ�", "����");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                LogHelper.Info(typeof(Connection), "�쳣��" + e.ToString());
                throw new ConnectionException(e.ToString(), e);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="parameters">�������Ӳ�������</param>
        private void Setup(ConnectionParameters parameters)
        {
            this.parameters = parameters;
        }

        /// <summary>
        /// ��������(����)
        /// </summary>
        /// <param name="hostname">������</param>
        /// <param name="parameters">�������Ӳ�������</param>
        /// <param name="tcpPort">TCP�˿ں�</param>
        private void Setup(string hostname, ConnectionParameters parameters, int tcpPort)
        {
            this.hostname = hostname;
            this.parameters = parameters;
            this.tcpPort = tcpPort;

            // ���ý������ӵĳ�ʱʱ��
            this.connectTimeoutInMs = parameters.T0 * 1000;

            // ����״̬����������1
            connectionCounter++;
            // ��������״̬ID
            this.connectionID = connectionCounter;
        }

        /// <summary>
        /// �޲ι��췽��
        /// </summary>
        public Connection()
        {
            Setup(new ConnectionParameters());
        }

        /// <summary>
        /// �вι��췽��
        /// </summary>
        /// <param name="a">��Ϣ�����ַ����</param>
        public Connection(int a)
        {
            Setup(new ConnectionParameters(a));
        }

        /// <summary>
        /// �вι��췽��
        /// </summary>
        /// <param name="hostname">������</param>
        public Connection(string hostname)
        {
            Setup(hostname, new ConnectionParameters(), 2404);
        }

        /// <summary>
        /// �вι��췽��
        /// </summary>
        /// <param name="hostname">������</param>
        /// <param name="tcpPort">TCP�˿ں�</param>
        public Connection(string hostname, int tcpPort)
        {
            Setup(hostname, new ConnectionParameters(), tcpPort);
        }

        /// <summary>
        /// Sends the interrogation command.����ѯ������
        /// </summary>
        /// <param name="cot">Cause of transmission</param>����ԭ��
        /// <param name="ca">Common address  ������ַ</param>
        /// <param name="qoi">Qualifier of interrogation (20 = station interrogation) �ٻ��޶��ʣ����ٻ�Ϊ20</param>
        /// <exception cref="ConnectionException">description</exception>
        public void SendInterrogationCommand(CauseOfTransmission cot, int ca, byte qoi)
        {
            ASDU asdu = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);

            asdu.AddInformationObject(new InterrogationCommand(0, qoi));//�˺�������Ϣ����InformationObject����ӵ�ASDU

            SendASDUInternal(asdu);//����ASDU����
        }

        /// <summary>
        /// �л���ֵ��
        /// </summary>
        /// <param name="cot">����ԭ��</param>
        /// <param name="ca">ASDU������ַ</param>
        /// <param name="parameterArea">��ֵ��</param>
        public void SendChangeParameterAreaCommand(CauseOfTransmission cot, int ca, int parameterArea)
        {
            ASDU asdu = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);

            asdu.AddInformationObject(new ChangeSettingAreaCommand(0, parameterArea));

            SendASDUInternal(asdu);
        }

        /// <summary>
        /// ����ǰ��ֵ����
        /// </summary>
        /// <param name="cot">����ԭ��</param>
        /// <param name="ca">ASDU������ַ</param>
        public void SendReadParameterAreaCommand(CauseOfTransmission cot, int ca)
        {
            ASDU asdu = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);

            asdu.AddInformationObject(new ReadParameterAreaCommand(0));

            SendASDUInternal(asdu);
        }

        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="cot">����ԭ��</param>
        /// <param name="ca">ASDU������ַ</param>
        /// <param name="buf">��ֵ��������</param>
        public void SendReadParameterCommand(CauseOfTransmission cot, int ca, byte[] buf)
        {
            ASDU asdu = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);
            asdu.AddInformationObject(new ReadParameterCommand(buf));

            SendASDUInternal(asdu);
        }

        /// <summary>
        /// �̻�����
        /// </summary>
        /// <param name="cot">����ԭ��</param>
        /// <param name="ca">ASDU������ַ</param>
        /// <param name="buf">�̻���������</param>
        public void SendSetParameterCommand(CauseOfTransmission cot, int ca, byte[] buf)
        {
            ASDU asdu = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);
            asdu.AddInformationObject(new SetParameterCommand(buf));

            SendASDUInternal(asdu);
        }

        /// <summary>
        /// ���͵��������ݱ���.Sends the counter interrogation command (C_CI_NA_1 typeID: 101)
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

            asdu.AddInformationObject(new ReadCommand(ioa)); //������Ϣ��asdu���棬ioa��ֵ���

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
        /// ʱ�Ӷ�ȡ����
        /// </summary>
        /// <param name="ca">ASDU������ַ</param>
        /// <param name="time">CP56Time2aʱ��</param>
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
        /// ���ͽ��̸�λ����.Sends a reset process command (C_RP_NA_1 typeID: 105).
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
        /// ����ʱ����ʱ�������.Sends a delay acquisition command (C_CD_NA_1 typeID: 106).
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
        /// ����ң������.Sends the control command.
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
            // �ж���������״̬
            if (running)
            {
                // �����ݷ��͵����ӵ� System.Net.Sockets.Socket
                socket.Send(STARTDT_ACT_MSG);
                // ��ʾ���͵ı�����Ϣ
                ShowMessage.Show(STARTDT_ACT_MSG, STARTDT_ACT_MSG.Length, "����");
                LogHelper.Info(typeof(Connection), "���ͱ��ģ�" + UtilHelper.ListToString(STARTDT_ACT_MSG));
                // ������Ϣ��ͳ�ƶ���ķ��ͱ���������1
                statistics.SentMsgCounter++;
            }
            else
            {
                // �ж���һ���쳣�����Ƿ�Ϊ��
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
            // �ж���������״̬
            if (running)
            {
                // �����ݷ��͵����ӵ� System.Net.Sockets.Socket
                socket.Send(STOPDT_ACT_MSG);
                // ��ʾ���͵ı�����Ϣ
                ShowMessage.Show(STOPDT_ACT_MSG, STOPDT_ACT_MSG.Length, "����");
                LogHelper.Info(typeof(Connection), "���ͱ��ģ�" + UtilHelper.ListToString(STOPDT_ACT_MSG));
                // ������Ϣ��ͳ�ƶ���ķ��ͱ���������1
                statistics.SentMsgCounter++;
            }
            else
            {
                // �ж���һ���쳣�����Ƿ�Ϊ��
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

        #region  �ļ��������ָ�����

        /// <summary>
        /// �ļ�����ָ���������
        /// </summary>
        /// <param name="cot">����ԭ��</param>
        /// <param name="ca">ASDU������ַ</param>
        /// <param name="buf">�����ֽ�����</param>
        public void SendFileServiceCommand(CauseOfTransmission cot, int ca, byte[] buf)
        {
            ASDU controlCommand = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);
            controlCommand.AddInformationObject(new FileServiceCommand(buf));
            SendASDUInternal(controlCommand);
        }

        /// <summary>
        /// ���������ļ�����
        /// </summary>
        /// <param name="cot">����ԭ��</param>
        /// <param name="ca">ASDU������ַ</param>
        /// <param name="ctype">��������</param>
        public void SendUpdataCommand(CauseOfTransmission cot, int ca, byte ctype)
        {
            ASDU controlCommand = new ASDU(parameters, cot, false, false, (byte)parameters.OriginatorAddress, ca, false);
            controlCommand.AddInformationObject(new UpdataCommand(0, ctype));
            SendASDUInternal(controlCommand);
        }

        #endregion �ļ��������ָ�����

        /// <summary>
        /// Connect this instance.
        /// </summary>
        /// The function will throw a SocketException if the connection attempt is rejected or timed out.
        /// <exception cref="ConnectionException">description</exception>
        public void Connect()
        {
            // ����ͨ��
            ConnectAsync();

            Console.WriteLine("--------------�첽����������---------------");

            int i = 0;

            // �ж���������״̬��socket�����־
            while ((running == false) && (socketError == false))
            {
                Console.WriteLine("--------------ѭ���ж���������״̬��---------------");
                // ����ǰ�̹߳���1����
                Thread.Sleep(1);
                i++;
                if (i == 200)
                {
                    socketError = true;
                }
            }

            // �ж�socket�����־
            if (socketError)
            {
                if (lastException != null)
                {
                    throw new ConnectionException("socket����:" + lastException.Message, lastException);
                }
                DebugLog("ѭ���ж���������״̬��ʱ");
            }

        }

        /// <summary>
        /// ���ó��ڿ���״̬�·��Ͳ���֡�ĳ�ʱ(T3)
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
            // �ж���������״̬����������״̬
            if ((running == false) && (connecting == false))
            {
                // ��������
                ResetConnection();
                Console.WriteLine("--------------�������ӽ���---------------");

                // ���ó��ڿ���״̬�·��Ͳ���֡�ĳ�ʱ(T3)
                ResetT3Timeout();
                Console.WriteLine("--------------���ó��ڿ���״̬�·��Ͳ���֡�ĳ�ʱ(T3)����---------------");

                // �����߳�
                Thread workerThread = new Thread(Run);
                // �����߳�
                workerThread.IsBackground = true;
                workerThread.Start();
            }
            else
            {
                // �ж���������״̬
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
        /// ���ձ�����Ϣ
        /// </summary>
        /// <param name="socket">socket�׽���</param>
        /// <param name="buffer">���ձ��Ļ�����</param>
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

            // �жϵ�һ���ֽ��ǲ��������ַ�0x68
            if (buffer[0] != 0x68 && buffer[0] != 0x11)
            {
                DebugLog("Missing SOF indicator!");
                LogHelper.Warn(typeof(Connection), "buffer[0] != 0x68 and buffer[0] != 0x11 buffer[0]:" + buffer[0]);
                return -1;
            }
            if (buffer[0] == 0x68)
            {
                // ��ȡ���ĳ����ֽ�.read length byte
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
        /// ����Ƿ������ݱ���ʱȷ�ϵĳ�ʱ(T2,T2С��T1)
        /// </summary>
        /// <param name="currentTime">��ǰʱ��</param>
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
        /// �����Լ�������������
        /// </summary>
        /// <param name="socket">socket�׽���</param>
        /// <param name="buffer">�������յ���һ֡�����ֽ�����(���ȴ��ڵ�����֡���ĳ���)</param>
        /// <param name="msgSize">���յ���һ֡���ĳ���</param>
        /// <returns></returns>
        private bool CheckMessage(RedefineSocket socket, byte[] buffer, int msgSize)
        {
            // �жϽ��յ��ı��ĳ����Ƿ�������ͱ�����
            if (msgSize < 6)
            {
                DebugLog("msg too small!");
                return false;
            }

            // ��ȡ��ǰʱ��
            long currentTime = SystemUtils.currentTimeMillis();

            #region ͨ��������Ӧ���Ĵ���
            if (buffer[0] == 0x11)
            {
                byte sum = (byte)(buffer[1] + buffer[2] + buffer[3]);
                if (sum == buffer[4] && buffer[5] == 0x66)
                {
                    if (buffer[1] == 0x0B)
                    {
                        //ͨ�����ӱ�־λ��1
                        System.Threading.Interlocked.Exchange(ref MainViewModel.ChannelMonitorListening, 1);
                        LogHelper.Info(typeof(Connection), "MainViewModel.ChannelMonitorListening:"
                            + System.Threading.Interlocked.Read(ref MainViewModel.ChannelMonitorListening));
                        MessageBox.Show("ͨ�����ӹ��ܿ���");
                        LogHelper.Info(typeof(ChannelMonitorViewModel), "��ʼͨ������");

                        //���Ͳ���֡�Ķ�ʱ������
                        testFrameTimerForChannelMonitor = new System.Timers.Timer(2000);
                        testFrameTimerForChannelMonitor.AutoReset = true;
                        testFrameTimerForChannelMonitor.Enabled = true;
                        testFrameTimerForChannelMonitor.Elapsed += new System.Timers.ElapsedEventHandler(TimerOut);
                        isTiming = true;
                    }
                    else if (buffer[1] == 0x0F)
                    {
                        MessageBox.Show("ͨ�����Ӷ˿�������λ�������У���ѡ����ȷ�ļ��Ӷ˿�");
                    }
                    else
                    {
                        MessageBox.Show("ͨ�����ӻظ�δ֪�����룺" + buffer[1] + " " + buffer[2] + " " + buffer[3] + " " + buffer[4] + " " + buffer[5]);
                    }
                    return true;
                }
                else
                {
                    // ����У��Ͳ���
                    return false;
                }
            }

            #endregion ͨ��������Ӧ���Ĵ���

            #region I��ʽ֡���ݴ���

            // �ж��ǲ���I��ʽ֡(I format frame)
            if ((buffer[2] & 1) == 0 && (buffer[4] & 1) == 0)
            {
                // �жϵ�һ�����յ���I֡���ı�־
                if (!firstIMessageReceived)
                {
                    firstIMessageReceived = true;
                    // ������͵�ȷ�ϱ���ʱ���(���ڼ�������ݱ���ʱȷ�ϵĳ�ʱT2).start timeout T2
                    lastConfirmationTime = currentTime;
                }

                // �ж�I֡���ĳ���,���ٴ���7
                if (msgSize < 7)
                {
                    DebugLog("I msg too small!");
                    return false;
                }

                // ��ȡ����֡�ķ������к�
                int frameSendSequenceNumber = ((buffer[3] * 0x100) + (buffer[2] & 0xfe)) / 2;
                // ��ȡ����֡�Ľ������к�
                int frameRecvSequenceNumber = ((buffer[5] * 0x100) + (buffer[4] & 0xfe)) / 2;

                // ��ӡ�����Ϣ
                DebugLog("Received I frame: N(S) = " + frameSendSequenceNumber + " N(R) = " + frameRecvSequenceNumber);

                // ������յ�֡�ķ������к�,�Ƿ���ڽ������к�
                // check the receive sequence number N(R) - connection will be closed on an unexpected value
                if (frameSendSequenceNumber != receiveSequenceNumber)
                {
                    DebugLog(frameSendSequenceNumber + "(s):(r)" + receiveSequenceNumber + " Sequence error: Close connection!");
                    return false;
                }

                // �������֡�Ľ������к��Ƿ�Ϸ�
                if (!CheckSequenceNumber(frameRecvSequenceNumber))
                {
                    return false;
                }

                // ���ý��ձ��ĵ����кż�1
                receiveSequenceNumber = (receiveSequenceNumber + 1) % 32768;
                // δȷ�ϵ��ѽ��յ��ı�����������1
                unconfirmedReceivedIMessages++;

                try
                {
                    // ��ȡASDUӦ�÷������ݵ�Ԫ
                    ASDU asdu = new ASDU(parameters, buffer, 6, msgSize);

                    // ��ʾ���յı�����Ϣ
                    ShowMessage.Show(buffer, msgSize, "����");
                    LogHelper.Info(typeof(Connection), "���ձ��ģ�" + UtilHelper.ListToString(buffer, msgSize));

                    // ����ASDUӦ�÷������ݵ�Ԫ
                    if (asduReceivedHandler != null)
                    {
                        asduReceivedHandler(asduReceivedHandlerParameter, asdu);
                    }

                }
                catch (ASDUParsingException e)
                {
                    DebugLog("ASDU parsing failed: " + e.Message);
                    LogHelper.Error(typeof(Connection), "����ASDU�쳣 \n" + e.Message);
                    return false;
                }

                // ���ó��ڿ���״̬�·��Ͳ���֡�ĳ�ʱ(T3)
                ResetT3Timeout();

                return true;

            }

            #endregion I��ʽ֡���ݴ���

            #region S��ʽ֡���ݴ���

            // �ж��ǲ���S��ʽ֡(S format frame)
            if (buffer[2] == 0x01 && buffer[3] == 0x00 && (buffer[4] & 0x01) == 0x00)
            {
                // ��ȡ�������к�
                int seqNo = (buffer[4] + buffer[5] * 0x100) / 2;

                DebugLog("Recv S(" + seqNo + ") (own sendcounter = " + sendSequenceNumber + ")");
                // ��ʾ���յ��ı�����Ϣ
                ShowMessage.Show(buffer, 6, "����");
                LogHelper.Info(typeof(Connection), "���ձ��ģ�" + UtilHelper.ListToString(buffer, 6));

                // �������к��Ƿ�Ϸ���Ч
                if (!CheckSequenceNumber(seqNo))
                {
                    return false;
                }

                // ���ó��ڿ���״̬�·��Ͳ���֡�ĳ�ʱ(T3)
                ResetT3Timeout();

                return true;

            }

            #endregion S��ʽ֡���ݴ���

            #region U��ʽ֡���ݴ���

            // �ж��ǲ���U��ʽ֡(U format frame)
            if ((buffer[2] & 0x03) == 0x03 && (buffer[4] & 0x01) == 0x00)
            {
                // ��ʾ���ܵı�����Ϣ
                ShowMessage.Show(buffer, 6, "����");
                LogHelper.Info(typeof(Connection), "���ձ��ģ�" + UtilHelper.ListToString(buffer, 6));
                // ���÷��ͻ����APDU�ĳ�ʱʱ��
                uMessageTimeout = 0;

                switch (buffer[2])
                {
                    case 0x43:
                        // ��������֡.Check for TESTFR_ACT message

                        statistics.RcvdTestFrActCounter++;
                        DebugLog("RCVD TESTFR_ACT");
                        DebugLog("SEND TESTFR_CON");

                        // ������·����ȷ��֡
                        socket.Send(TESTFR_CON_MSG);
                        // ��ʾ���յ��ı�����Ϣ
                        ShowMessage.Show(TESTFR_CON_MSG, TESTFR_CON_MSG.Length, "����");
                        LogHelper.Info(typeof(Connection), "���ͱ��ģ�" + UtilHelper.ListToString(TESTFR_CON_MSG));
                        // ������Ϣ��ͳ�ƶ���ı���������1
                        statistics.SentMsgCounter++;

                        break;

                    case 0x83:
                        // ����ȷ��֡.TESTFR_CON

                        DebugLog("RCVD TESTFR_CON");
                        statistics.RcvdTestFrConCounter++;
                        outStandingTestFRConMessages = 0;
                        break;

                    case 0x07:
                        // ��·��������֡.STARTDT ACT

                        DebugLog("RCVD STARTDT_ACT");
                        // ������·����ȷ��֡
                        socket.Send(STARTDT_CON_MSG);
                        // ��ʾ���յ��ı�����Ϣ
                        ShowMessage.Show(STARTDT_CON_MSG, STARTDT_CON_MSG.Length, "����");
                        LogHelper.Info(typeof(Connection), "���ͱ��ģ�" + UtilHelper.ListToString(STARTDT_CON_MSG));
                        // ������Ϣ��ͳ�ƶ���ı���������1
                        statistics.SentMsgCounter++;

                        break;

                    case 0x0B:
                        // ��·����ȷ��֡.STARTDT_CON

                        DebugLog("RCVD STARTDT_CON");
                        // ��������״������
                        if (connectionHandler != null)
                        {
                            connectionHandler(connectionHandlerParameter, ConnectionEvent.STARTDT_CON_RECEIVED);
                        }

                        break;

                    case 0x23:
                        // ��·ֹͣȷ��֡.STOPDT_CON

                        DebugLog("RCVD STOPDT_CON");
                        // ��������״������
                        if (connectionHandler != null)
                        {
                            connectionHandler(connectionHandlerParameter, ConnectionEvent.STOPDT_CON_RECEIVED);
                        }

                        break;
                }

                // ���ó��ڿ���״̬�·��Ͳ���֡�ĳ�ʱ(T3)
                ResetT3Timeout();

                return true;
            }

            #endregion U��ʽ֡���ݴ���

            #region �����ʽ֡���ݴ���

            // ���δʶ��֡��ʽ
            DebugLog("Unknown message type");
            return false;

            #endregion �����ʽ֡���ݴ���

        }

        /// <summary>
        /// �����׽��ִ���
        /// </summary>
        private void ConnectSocketWithTimeout()
        {
            // �ṩ����Э�� (IP) ��ַ
            IPAddress ipAddress;
            // ������˵��ʾΪ IP ��ַ�Ͷ˿ں�
            IPEndPoint remoteEP;

            try
            {
                // �� IP ��ַ�ַ���ת��Ϊ System.Net.IPAddress ʵ��
                ipAddress = IPAddress.Parse(hostname);
                // ��ָ���ĵ�ַ�Ͷ˿ںų�ʼ�� System.Net.IPEndPoint �����ʵ��
                remoteEP = new IPEndPoint(ipAddress, tcpPort);
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------IP��ַ�Ͷ˿ں�����---------------");
                LogHelper.Error(typeof(Connection), "IP��ַ�Ͷ˿ں����� \n");
                // wrong argument
                throw new ConnectionException("SocketException:IP��ַ�Ͷ˿ں�����" + e.Message, new SocketException(87));
            }

            Console.WriteLine("--------------IP��ַ�Ͷ˿ںų�ʼ���ɹ�---------------");

            try
            {
                // �����׽���.Create a TCP/IP  socket.
                socket = new RedefineSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------�׽��ִ���ʧ��---------------");
                LogHelper.Error(typeof(Connection), "�׽��ִ���ʧ�� \n");
                // wrong argument
                throw new ConnectionException("SocketException:�׽��ִ���ʧ��" + e.Message, new SocketException(87));
            }

            Console.WriteLine("--------------�׽��ִ����ɹ�---------------");

            try
            {
                // ��ʼһ����Զ���������ӵ��첽����
                var result = socket.BeginConnect(remoteEP, null, null);
                Console.WriteLine("--------------��ʼһ����Զ���������ӵ��첽����---------------");

                // ��ֹ��ǰ�̣߳�ֱ����ǰ�� System.Threading.WaitHandle �յ��ź�Ϊֹ��
                // ͬʱʹ�� 32 λ����������ָ��ʱ��������ָ���Ƿ��ڵȴ�֮ǰ�˳�ͬ����
                bool success = result.AsyncWaitHandle.WaitOne(connectTimeoutInMs, true);
                Console.WriteLine("--------------��ֹ��ǰ�̣߳�ֱ����ǰ�� System.Threading.WaitHandle �յ��ź�---------------");
                if (success)
                {
                    Console.WriteLine("--------------����������첽��������---------------");
                    // ����������첽��������
                    socket.EndConnect(result);
                }
                else
                {
                    // �ر� System.Net.Sockets.Socket ���Ӳ��ͷ����й�������Դ
                    socket.Close();

                    Console.WriteLine("--------------�׽��ֹر�,����ʱ�䳬ʱ---------------");

                    // Connection timed out.
                    throw new ConnectionException("SocketException:�׽��ֹر�,����ʱ�䳬ʱ", new SocketException(10060));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------��Զ���������ӵ��첽����ʧ��---------------");
                LogHelper.Error(typeof(Connection), "��Զ���������ӵ��첽����ʧ�� \n");

                throw new ConnectionException("SocketException:��Զ���������ӵ��첽����ʧ�ܡ�" + e.Message, new SocketException(10060));
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

            // ������Ϣ��ͳ�ƶ���Ľ��ձ���������1
            statistics.RcvdMsgCounter++;

            DebugLog("RCVD: " + BitConverter.ToString(bytes, 0, bytesRec));

            // �����Լ�������������
            if (!CheckMessage(socket, bytes, bytesRec))
            {
                DebugLog("loopRunning����Ϊfalse:checkMessage()����");
                return false;
            }

            // ���շ����յ���ȷ��I��ʽ�ı��������Ƿ�ﵽ����
            if (unconfirmedReceivedIMessages >= parameters.W)
            {
                // �ﵽ����,����������͵�ȷ�ϱ���ʱ���(���ڼ�������ݱ���ʱȷ�ϵĳ�ʱT2)Ϊ��ǰʱ��
                lastConfirmationTime = SystemUtils.currentTimeMillis();
                // ���շ����յ���ȷ��I��ʽ�ı�����������Ϊ0
                unconfirmedReceivedIMessages = 0;
                // ����S֡����
                SendSMessage();
            }
            return true;
        }

        private bool MonitorMessage(byte[] bytesForChannelMonitor, ref List<byte> ListForChannelMonitor)
        {
            // ��Զ���豸��ȡ�ֽ�
            int bytesRecForChannelMonitor = ReceiveMessageForChannelMonitor(socket, bytesForChannelMonitor);
            if (bytesRecForChannelMonitor > 0)
            {
                // ���ö�ʱ��
                if (isTiming)
                {
                    // ���ε�һ�����д���Ϊȡ���ڿ�����·״̬�·��Ͳ���֡���߼�
                    //testFrameTimerForChannelMonitor.Stop();
                    //testFrameTimerForChannelMonitor.Start();
                    //isReceiveTestFrameResponse = true;
                    isReceiveFrameResponse = true;
                    unrecieveFrameTime = 0;
                }

                // ������ձ�����Ϣ
                DebugLog("���ڼ���RCVD: " + BitConverter.ToString(bytesForChannelMonitor, 0, bytesRecForChannelMonitor));
                // �����յ��ַ�byteת����List
                byte[] subset = new byte[bytesRecForChannelMonitor];
                Array.Copy(bytesForChannelMonitor, subset, bytesRecForChannelMonitor);
                ListForChannelMonitor.AddRange(subset);
                // �����Լ�������������
                bool ok = CheckMessageForChannelMonitor(ref ListForChannelMonitor);
                if (ok)
                {
                    LogHelper.Warn(typeof(Connection), "�������ĳɹ�");
                    return true;
                }
                else
                {
                    testFrameTimerForChannelMonitor.Stop();
                    LogHelper.Warn(typeof(Connection), "�������Ĵ���");
                    return false;
                }
            }
            else if (bytesRecForChannelMonitor == 0)
            {
                testFrameTimerForChannelMonitor.Stop();
                LogHelper.Warn(typeof(Connection), "�˿ڼ���loopRunning����Ϊfalse:���յı����ֽ����鳤��Ϊ0,������Ѿ��Ͽ�����");
                return false;
            }
            else
            {
                testFrameTimerForChannelMonitor.Stop();
                LogHelper.Warn(typeof(Connection), "�˿ڼ���loopRunning����Ϊfalse:���յı����ֽ����鳤��Ϊ-1,���ǲ���������:");
                return false;
            }
        }

        /// <summary>
        /// ��ʱ�жϴ���
        /// </summary>
        /// <returns></returns>
        private bool HandleTimeouts()
        {
            // ��ȡ��ǰ�ܺ�����
            UInt64 currentTime = (UInt64)SystemUtils.currentTimeMillis();

            #region ���ڿ���״̬�·��Ͳ���֡�ĳ�ʱ

            // �ж��ǲ���T3��ʱ(���ڿ���״̬�·��Ͳ���֡�ĳ�ʱ)
            if (currentTime > nextT3Timeout)
            {
                // ���ڿ���״̬�·��Ͳ���֡�������ǲ��ǳ�������(2��)
                /*
                if (outStandingTestFRConMessages > 2)
                {
                    DebugLog("Timeout for TESTFR_CON message");

                    // close connection
                    return false;
                }
                */

                // ������·��������֡
                socket.Send(TESTFR_ACT_MSG);
                // ��ʾ���ͱ��ĵ���Ϣ
                ShowMessage.Show(TESTFR_ACT_MSG, TESTFR_ACT_MSG.Length, "����");
                LogHelper.Info(typeof(Connection), "���ͱ��ģ�" + UtilHelper.ListToString(TESTFR_ACT_MSG));
                // ������Ϣ��ͳ�ƶ���ķ�����Ϣ������1
                statistics.SentMsgCounter++;
                // ��ӡ�����Ϣ
                DebugLog("U message T3 timeout");

                // ���÷��ͻ����APDU�ĳ�ʱʱ��
                uMessageTimeout = (UInt64)currentTime + (UInt64)(parameters.T1 * 1000);
                // ���ڿ���״̬�·��Ͳ���֡����������1
                outStandingTestFRConMessages++;
                // ���ó��ڿ���״̬�·��Ͳ���֡�ĳ�ʱ(T3)
                ResetT3Timeout();
            }

            #endregion ���ڿ���״̬�·��Ͳ���֡�ĳ�ʱ

            #region �����ݱ���ʱȷ�ϵĳ�ʱ(T2,T2<T1)

            // �жϽ��շ����յ���ȷ��I��ʽ�ı�������(���ֵ��w)�Ƿ������,���Ƿ������ݱ���ʱȷ�ϵĳ�ʱ(T2,T2<T1)
            if (unconfirmedReceivedIMessages > 0 && checkConfirmTimeout((long)currentTime))
            {
                // ����������͵�ȷ�ϱ���ʱ���(���ڼ�������ݱ���ʱȷ�ϵĳ�ʱT2)
                lastConfirmationTime = (long)currentTime;
                // ���ý��շ����յ���ȷ��I��ʽ�ı�������(���ֵ��w)Ϊ0
                unconfirmedReceivedIMessages = 0;
                // send confirmation message
                SendSMessage();
            }

            #endregion �����ݱ���ʱȷ�ϵĳ�ʱ(T2,T2<T1)

            #region ���ͻ����APDU�ĳ�ʱʱ��(T1)

            // ���ͻ����APDU�ĳ�ʱʱ��(T1)��������,��С�ڵ�ǰʱ��
            if (uMessageTimeout != 0 && uMessageTimeout < currentTime)
            {
                DebugLog("U message T1 timeout");
                //throw new ConnectionException("SocketException", new SocketException(10060));
                return false;
            }

            // check if counterpart confirmed I messages
            // �����ͱ��ĵ�k����������(�����߳�ͬ��)
            lock (sentASDUs)
            {
                // ���緢�͵�δ���յ�ȷ�ϵı���ʱ�䵽��ǰʱ��ļ���Ƿ�ʱ
                if (oldestSentASDU != -1 && oldestSentASDU != newestSentASDU && ((long)currentTime - sentASDUs[oldestSentASDU].sentTime) >= (parameters.T1 * 1000))
                {
                    DebugLog("���緢�͵�δ���յ�ȷ�ϵı���ʱ�䵽��ǰʱ��ļ����ʱ" + oldestSentASDU + "---" + currentTime + "---" + sentASDUs[oldestSentASDU].sentTime + "---" + parameters.T1);
                    return false;
                }
            }

            #endregion ���ͻ����APDU�ĳ�ʱʱ��(T1)0

            return true;
        }

        /// <summary>
        /// �׽�������
        /// </summary>
        /// <returns></returns>
        private bool InitConnection()
        {
            // ��������״̬Ϊtrue
            connecting = true;
            try
            {
                // Connect to a remote device.
                ConnectSocketWithTimeout();
                Console.WriteLine("--------------Connect to a remote device success---------------");
                DebugLog("Socket connected to " + socket.RemoteEndPoint.ToString());

                // �ж��Զ���������֡���ı�־
                if (autostart)
                {
                    // ������·��������֡
                    socket.Send(STARTDT_ACT_MSG);

                    // ��ʾ���յ��ı�����Ϣ
                    ShowMessage.Show(STARTDT_ACT_MSG, STARTDT_ACT_MSG.Length, "����");
                    LogHelper.Info(typeof(Connection), "���ͱ��ģ�" + UtilHelper.ListToString(STARTDT_ACT_MSG));
                    // ������Ϣ��ͳ�ƶ���ķ��ͱ���������1
                    statistics.SentMsgCounter++;
                }

                // ������������״̬Ϊtrue
                running = true;
                // ����socket�����־λfalse
                socketError = false;
                // ������������״̬Ϊfalse
                connecting = false;

                // ��������״̬����
                if (connectionHandler != null)
                {
                    connectionHandler(connectionHandlerParameter, ConnectionEvent.OPENED);
                }
                return true;
            }
            catch (Exception se)
            {
                DebugLog("�׽��������쳣,SocketException: " + se.ToString());
                lastException = se;
            }
            running = false;
            socketError = true;
            return false;
        }

        /// <summary>
        /// �ر�socket����
        /// </summary>
        private void CloseConnection()
        {
            LogHelper.Warn(typeof(Connection), "�ر���������");
            DebugLog("CLOSE CONNECTION!");
            // �ر�Socket���Ӳ��ͷ����й�������Դ
            socket.Close();
            // ��������״̬����Ϊfalse
            connecting = false;
            // �ж���������״̬
            if (connectionHandler != null)
            {
                connectionHandler(connectionHandlerParameter, ConnectionEvent.CLOSED);
            }
        }

        /// <summary>
        /// �˳��������е�run�߳�
        /// </summary>
        private void AbortRun()
        {
            System.Threading.Thread.CurrentThread.Abort();
            LogHelper.Info(typeof(Connection), "�ر�Socket���Ӳ��ͷ����й�������Դ");
        }

        /// <summary>
        /// �����������ӣ���ȡ���Ĳ�����
        /// </summary>
        private void Run()
        {
            LogHelper.Info(typeof(Connection), "��̨�����߳̿�ʼ����");
            try
            {
                // ��������
                if (!InitConnection())
                {
                    LogHelper.Fatal(typeof(Connection), "��·���ӳ�ʼ��ʧ��");
                    MessageBox.Show("��·����ʧ��", "����");
                    // �˳���ǰ�����߳�
                    AbortRun();
                    return;
                }

                // ��������
                // loop until error
                HandleConnection();

                // �Ͽ�����
                CloseConnection();
            }
            catch (Exception e)
            {
                LogHelper.Fatal(typeof(Connection), e);
            }
            LogHelper.Warn(typeof(Connection), "��̨�����߳�ֹͣ����");

            // �ر� System.Net.Sockets.Socket ���Ӳ��ͷ����й�������Դ
            AbortRun();
        }

        /// <summary>
        /// ���Ӵ���
        /// </summary>
        private void HandleConnection()
        {
            LogHelper.Info(typeof(Connection), "��ʼ�������籨��");
            // ͨ�����ӱ��Ľ��ܻ�����
            // ���ձ��ĵĻ������ֽ�����
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
                        // �˿ڼ���ģʽ�£����ĵĽ��������
                        if (!MonitorMessage(bytesForChannelMonitor, ref ListForChannelMonitor))
                        {
                            LogHelper.Warn(typeof(Connection), "�˿ڼ���ģʽ�£����ĵĽ��������ʧ��MonitorMessage()");
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
                    #region �Ƕ˿ڼ���ģʽ�£��������ݵĽ��������
                    try
                    {
                        if (!ParseMessage(bytes))
                        {
                            LogHelper.Warn(typeof(Connection), "�Ƕ˿ڼ���ģʽ�£����ĵĽ��������ʧ��ParseMessage()");
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
                            LogHelper.Warn(typeof(Connection), "�Ƕ˿ڼ���ģʽ�£��������ݵĽ��������ʧ��" + e.Message);
                            running = false;
                        }
                    }

                    if (!HandleTimeouts())
                    {
                        // ѭ�����б�־����Ϊfalse
                        running = false;
                        DebugLog("��ͨģʽ �������ӳ�ʱ");
                        LogHelper.Warn(typeof(Connection), "��ͨģʽ �������ӳ�ʱ");
                    }
                    // �ж��Ƿ�ʹ�ñ��ķ��Ͷ��б�־���Ƿ���Է�����һ���ȴ���ASDUӦ�÷������ݵ�Ԫ
                    if (useSendMessageQueue && SendNextWaitingASDU())
                    {
                        // ���ù����̱߳�־Ϊfalse
                        suspendThread = false;
                    }
                    // �жϹ����̱߳�־
                    if (suspendThread)
                    {
                        //LogHelper.Warn(typeof(Connection), "suspendThread 10ms");
                        Thread.Sleep(10);
                    }
                    #endregion �Ƕ˿ڼ���ģʽ�£��������ݵĽ��������
                }
            }
            LogHelper.Fatal(typeof(Connection), "Quit HandleConnection()");
        }

        /// <summary>
        /// �ر�socket
        /// </summary>
        public void Close()
        {
            // �ж���������״̬��־
            if (socket != null && running)
            {
                // ����Socket�ķ��ͺͽ���
                socket.Shutdown(SocketShutdown.Both);

                // ѭ���ж���������״̬��־
                while (running)
                {
                    Thread.Sleep(1);
                }
            }
        }

        /// <summary>
        /// ���յ�ASDUӦ�÷������ݵ�Ԫ����Handler
        /// </summary>
        /// <param name="handler">���յ���ASDU��Ϣ����Handler(ί��)</param>
        /// <param name="parameter">���յ���ASDU��Ϣ����Handler����</param>
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
        /***********************************************���ӹ�����س��򲿷�****************************************************/
        /**********************************************************************************************************************/
        /// <summary>
        /// ͨ���������ڽ�������
        /// </summary>
        /// <param name="socket">�׽���</param>
        /// <param name="buffer">���ܵ��ֽڻ�������</param>
        /// <returns>���ؽ��ܵ��ֽ��������</returns>
        private int ReceiveMessageForChannelMonitor(RedefineSocket socket, byte[] buffer)
        {
            int len = socket.ReceiveByListeningModel(buffer, SocketFlags.None);
            LogHelper.Info(typeof(Connection), "ReceiveMessageForChannelMonitor() ����" + len + "�ֽ� "
                            + "���յı��ģ�" + BitConverter.ToString(buffer).Replace("-", string.Empty));
            return len;
        }

        /// <summary>
        /// ���������յ��ı���
        /// </summary>
        /// <param name="buffer">���ջ�����</param>
        /// <returns></returns>
        private bool CheckMessageForChannelMonitor(ref List<byte> buffer)
        {
            LogHelper.Info(typeof(Connection), "��ʼ����ͨ�������յ��ı���:" + BitConverter.ToString(buffer.ToArray()).Replace("-", string.Empty));
            // ���յ����ֽ����ɹ���101��С��������
            while (buffer.Count > 5)
            {
                LogHelper.Info(typeof(Connection), "���յ����ֽ����ɹ���101��С��������");
                // ����buffer�е� AA 55 ����68 ���� 10 ���� 11����Ч���ĵ���ʼ�ֽڣ�
                for (int i = 0; i < buffer.Count; ++i)
                {
                    // �ҵ�AA
                    if (buffer[i] == firstSendByte)
                    {
                        LogHelper.Info(typeof(Connection), "�ҵ�AA");
                        // AA��һ���ֽڴ���
                        if (i + 1 < buffer.Count)
                        {
                            LogHelper.Info(typeof(Connection), "AA��һ���ֽڴ���");
                            // AA��һ���ֽ���55
                            if (buffer[i + 1] == secondSendByte)
                            {
                                LogHelper.Info(typeof(Connection), "AA��һ���ֽ���55");
                                // AA 55ǰ����δ֪����
                                if (i > 0)
                                {
                                    LogHelper.Info(typeof(Connection), "AA 55ǰ����δ֪����");
                                    // ����δ֪����
                                    SerialPortService.DealUnkownFrame(ref buffer, i);
                                    // ���¿�ʼѭ���ж�ʣ����ֽ�
                                    break;
                                }

                                // AA 55ǰ��û��δ֪����,��Ϊ��ʼ�ֽ�
                                // AA 55����Ϊ68
                                if (buffer[2] == 0x68)
                                {
                                    LogHelper.Info(typeof(Connection), "AA 55����Ϊ68");
                                    // 101��֡���ı���ͷ��ȷ
                                    if (buffer[2] == 0x68 && buffer[5] == 0x68 && buffer[3] == buffer[4])
                                    {
                                        LogHelper.Info(typeof(Connection), "AA 55 101��֡���ı���ͷ��ȷ");
                                        //����û������
                                        if (buffer.Count < buffer[3] + 8)
                                        {
                                            LogHelper.Error(typeof(Connection), "ͨ�����������յ��������ı���");
                                            return true;
                                        }
                                        //���Ľ�������ȷ
                                        if (buffer[buffer[3] + 7] == 0x16)
                                        {
                                            LogHelper.Info(typeof(Connection), "���Ľ�������ȷ");
                                            int sum_cs = 0;
                                            for (int j = 6; j < buffer[3] + 6; j++)
                                            {
                                                sum_cs += buffer[j];
                                            }

                                            if ((sum_cs % 256) == buffer[buffer[3] + 6])
                                            {
                                                // ��ʱ��У����ɣ��յ��ɱ����ݳ���֡ ��ʼ��������
                                                // ��buffer�н�AA55ɾ��
                                                SerialPortService.DealAA55(ref buffer);
                                                List<byte> oneLongFrame = new List<byte>();
                                                for (int k = 0; k < buffer[1] + 6; ++k)
                                                {
                                                    oneLongFrame.Add(buffer[k]);
                                                }
                                                LogHelper.Info(typeof(Connection), "������ȷ�ĳ�֡����");
                                                // ������ȷ�ĳ�֡����
                                                SerialPortService.VariableFrameProcessingForChannelMonitor(oneLongFrame);
                                                buffer.RemoveRange(0, buffer[1] + 6);
                                                break;
                                            }
                                            else
                                            {
                                                LogHelper.Info(typeof(Connection), "У��ʹ���");
                                                // У��ʹ���
                                                SerialPortService.DealUnkownFrame(ref buffer, buffer[3] + 8);
                                            }
                                        }
                                        else
                                        {
                                            LogHelper.Info(typeof(Connection), "����������ȷ");
                                            // ����������ȷ
                                            SerialPortService.DealUnkownFrame(ref buffer, buffer[3] + 8);
                                        }
                                    }
                                    else
                                    {
                                        LogHelper.Info(typeof(Connection), "101��֡����ͷ����ȷ,��AA 55��ͬ����ͷһ����������List");
                                        // 101��֡����ͷ����ȷ,��AA 55��ͬ����ͷһ����������List
                                        SerialPortService.DealUnkownFrame(ref buffer, 6);
                                    }
                                }
                                else if (buffer[2] == 0x10)
                                {
                                    LogHelper.Info(typeof(Connection), "AA55��֡");
                                    // ��֡δ��ȡ����
                                    if (buffer.Count < 9)
                                    {
                                        LogHelper.Info(typeof(Connection), "AA55��֡δ��ȡ����");
                                        return true;
                                    }
                                    // 101��֡����У���
                                    byte sum = (byte)(buffer[3] + buffer[4] + buffer[5]);
                                    // У�����ȷ���ҽ�������ȷ
                                    if (sum == buffer[6] && buffer[7] == 0x16)
                                    {
                                        LogHelper.Info(typeof(Connection), "AA55101��֡����У�����ȷ���ҽ�������ȷ");
                                        // ��buffer�н�AA55ɾ��
                                        SerialPortService.DealAA55(ref buffer);
                                        //����һ֡��Ч��101��֡���ĵ�oneFrame��
                                        List<byte> oneShortFrame = new List<byte>();
                                        for (int n = 0; n < 6; ++n)
                                        {
                                            oneShortFrame.Add(buffer[n]);
                                        }
                                        LogHelper.Info(typeof(Connection), "������Ч����֡");
                                        //������Ч����֡
                                        SerialPortService.FixedFrameProcessingForChannelMonitor(oneShortFrame);
                                        buffer.RemoveRange(0, 6);
                                        break;
                                    }
                                    else
                                    {
                                        LogHelper.Info(typeof(Connection), "��Ч��֡����");
                                        // ��Ч��֡����
                                        SerialPortService.DealUnkownFrame(ref buffer, 8);
                                    }
                                }
                                else
                                {
                                    LogHelper.Info(typeof(Connection), "AA 55����Ȳ���68 Ҳ����10 ");
                                    // AA 55����Ȳ���68 Ҳ����10 
                                    continue;
                                }
                            }
                            else
                            {
                                LogHelper.Info(typeof(Connection), "AA��һ���ֽڲ���55,�������AA������Ѱ����һ��AA");
                                // AA��һ���ֽڲ���55,�������AA������Ѱ����һ��AA
                                continue;
                            }
                        }
                        else
                        {
                            LogHelper.Info(typeof(Connection), "AA��һ���ֽڲ����ڣ����ؼ����ȴ���ȡ�ֽ�");
                            // AA��һ���ֽڲ����ڣ����ؼ����ȴ���ȡ�ֽ�
                            return true;
                        }
                    }
                    else if (buffer[i] == 0x68)
                    {
                        LogHelper.Info(typeof(Connection), "buffer[i] == 0x68");
                        // 68ǰ��������Ч����
                        if (i > 0)
                        {
                            LogHelper.Info(typeof(Connection), "68ǰ��������Ч����");
                            // ����ǰ������Ч����
                            SerialPortService.DealUnkownFrame(ref buffer, i);
                            // ���¿�ʼѭ���ж�ʣ����ֽ�
                            break;
                        }
                        // ��68��ʼ�ı��Ĵ���
                        if (buffer[0] == 0x68 && buffer[3] == 0x68 && buffer[1] == buffer[2])
                        {
                            LogHelper.Info(typeof(Connection), "��68��ʼ�ı��Ĵ���");
                            // ����û������
                            if (buffer.Count < buffer[1] + 6)
                            {
                                LogHelper.Info(typeof(Connection), "����û������");
                                return true;
                            }
                            // ���Ľ�������ȷ
                            if (buffer[buffer[1] + 5] == 0x16)
                            {
                                LogHelper.Info(typeof(Connection), "���Ľ�������ȷ");
                                int sum_cs = 0;
                                for (int j = 4; j < buffer[1] + 4; j++)
                                {
                                    sum_cs += buffer[j];
                                }
                                if ((sum_cs % 256) == buffer[buffer[1] + 4])
                                {
                                    LogHelper.Info(typeof(Connection), "��ʱ��У����ɣ��յ��ɱ����ݳ���֡ ��ʼ��������");
                                    // ��ʱ��У����ɣ��յ��ɱ����ݳ���֡ ��ʼ��������
                                    List<byte> oneLongFrame = new List<byte>();
                                    for (int k = 0; k < buffer[1] + 6; ++k)
                                    {
                                        oneLongFrame.Add(buffer[k]);
                                    }
                                    LogHelper.Info(typeof(Connection), "������ȷ�ĳ�֡����");
                                    // ������ȷ�ĳ�֡����
                                    SerialPortService.DirForChannelMonitor = "����";
                                    SerialPortService.VariableFrameProcessingForChannelMonitor(oneLongFrame);
                                    buffer.RemoveRange(0, buffer[1] + 6);
                                    break;
                                }
                            }
                            else
                            {
                                LogHelper.Info(typeof(Connection), "У��Ͳ���");
                                // У��Ͳ���
                                SerialPortService.DealUnkownFrame(ref buffer, buffer[1] + 6);
                                break;
                            }
                        }
                        else
                        {
                            LogHelper.Info(typeof(Connection), "����ͷ����");
                            // ����ͷ����
                            SerialPortService.DealUnkownFrame(ref buffer, 4);
                            break;
                        }
                    }
                    else if (buffer[i] == 0x10)
                    {
                        LogHelper.Info(typeof(Connection), "buffer[i] == 0x10");
                        // 10ǰ��������Ч����
                        if (i > 0)
                        {
                            LogHelper.Info(typeof(Connection), "����ǰ������Ч����");
                            // ����ǰ������Ч����
                            SerialPortService.DealUnkownFrame(ref buffer, i);
                            // ���¿�ʼѭ���ж�ʣ����ֽ�
                            break;
                        }
                        //101��֡����У���
                        byte sum = (byte)(buffer[1] + buffer[2] + buffer[3]);
                        // У�����ȷ���ҽ�������ȷ
                        if (sum == buffer[4] && buffer[5] == 0x16)
                        {
                            LogHelper.Info(typeof(Connection), "У�����ȷ���ҽ�������ȷ");
                            //����һ֡��Ч��101��֡���ĵ�oneFrame��
                            List<byte> oneShortFrame = new List<byte>();
                            for (int n = 0; n < 6; ++n)
                            {
                                oneShortFrame.Add(buffer[n]);
                            }
                            LogHelper.Info(typeof(Connection), "������Ч����֡");
                            //������Ч����֡
                            SerialPortService.DirForChannelMonitor = "����";
                            SerialPortService.FixedFrameProcessingForChannelMonitor(oneShortFrame);
                            buffer.RemoveRange(0, 6);
                            break;
                        }
                        else
                        {
                            LogHelper.Info(typeof(Connection), "��Ч��֡����");
                            // ��Ч��֡����
                            SerialPortService.DealUnkownFrame(ref buffer, 6);
                            break;
                        }
                    }
                    else if (buffer[i] == 0x11)
                    {
                        // 11ǰ��������Ч����
                        if (i > 0)
                        {
                            LogHelper.Info(typeof(Connection), "11ǰ��������Ч����");
                            // ����ǰ������Ч����
                            SerialPortService.DealUnkownFrame(ref buffer, i);
                            // ���¿�ʼѭ���ж�ʣ����ֽ�
                            break;
                        }
                        // FTU�ظ���������������ֹͣ�����ı���
                        byte sum = (byte)(buffer[1] + buffer[2] + buffer[3]);
                        if (sum == buffer[4] && buffer[5] == 0x66)
                        {
                            LogHelper.Info(typeof(Connection), "FTU�ظ���������������ֹͣ�����ı���");
                            //У�����ȷ����Чͨ�������Զ���Э���еĻظ�����
                            //if (buffer[1] == 0x0B)
                            //{
                            //    MessageBox.Show("ͨ�����ӹ��ܿ���");

                            //    //���Ͳ���֡�Ķ�ʱ������
                            //    testFrameTimerForChannelMonitor = new System.Timers.Timer(5000);
                            //    testFrameTimerForChannelMonitor.AutoReset = true;
                            //    testFrameTimerForChannelMonitor.Enabled = true;
                            //    testFrameTimerForChannelMonitor.Elapsed += new System.Timers.ElapsedEventHandler(TimerOut);
                            //    isTiming = true;

                            //}
                            //else if (buffer[1] == 0x0F)
                            //{
                            //    MessageBox.Show("ͨ�����Ӷ˿�������λ�������У���ѡ����ȷ�ļ��Ӷ˿�");
                            //    System.Threading.Interlocked.Exchange(ref MainViewModel.ChannelMonitorListening, 0);
                            //}
                            if (buffer[1] == 0x02)
                            {
                                LogHelper.Info(typeof(Connection), "FTU��Ӧ����֡");
                                // FTU��Ӧ����֡
                                //isReceiveTestFrameResponse = true;
                            }
                            else if(buffer[1] == 0x01)
                            {
                                LogHelper.Info(typeof(Connection), "FTU��Ӧ����֡");
                            }
                            else if (buffer[1] == 0x0C)
                            {
                                LogHelper.Info(typeof(Connection), "ֹͣ����֡�ظ�");
                                // ֹͣ����֡�ظ�
                                //ͨ�����ӱ�־��0,ֹͣ����֡�ļ�ʱ��
                                testFrameTimerForChannelMonitor.Stop();
                                System.Threading.Interlocked.Exchange(ref MainViewModel.ChannelMonitorListening, 0);
                                LogHelper.Info(typeof(ChannelMonitorViewModel), "ֹͣͨ������");
                                Messenger.Default.Send<string>("closeChannelMonitorView", "CloseChannelMonitorView");
                            }
                            else
                            {
                                LogHelper.Info(typeof(Connection), "δ֪ͨ�����ӻظ�֡");
                                // FTU���͹�����δ֪ͨ�����ӻظ�֡
                                MessageBox.Show("δ֪ͨ�����ӻظ�֡");
                            }
                            LogHelper.Info(typeof(Connection), "ɾ���Ѿ������ͨ�����ӻظ�֡");
                            // ɾ���Ѿ������ͨ�����ӻظ�֡
                            buffer.RemoveRange(0, 6);
                            break;
                        }
                        else
                        {
                            LogHelper.Info(typeof(Connection), "У��ʹ���ɾ���ֽ�");
                            // У��ʹ���ɾ���ֽ�
                            SerialPortService.DealUnkownFrame(ref buffer, 6);
                            break;
                        }
                    }
                    else
                    {
                        // ����Ч��ʼ���ģ����� AA 68 10 11�������ֽڴ���δ֪�����У����Ӵ�����������ɾ��
                        SerialPortService.DealUnkownFrame(ref buffer, 1);
                        // ����whileѭ�������½���ʣ�౨��
                        break;
                    }
                } // for
            } // while
            // ���յ����ֽ������ɹ�����С101��������
            LogHelper.Info(typeof(Connection), "����������ĳ��Ȳ�������СҪ��count:" + buffer.Count);
            return true;
        }

        /// <summary>
        /// ��·���Լ�ʱʱ�䵽
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void TimerOut(object source, System.Timers.ElapsedEventArgs e)
        {
            // �ж��Ƿ��н��յ����ⱨ��
            if (isReceiveFrameResponse)
            {
                isReceiveFrameResponse = false;
                Messenger.Default.Send<string>("testFrameTimeOut", "TestFrameTimeOut");
            }
            else if (!isReceiveFrameResponse)
            {
                // �ۼ�δ�յ����ĵĴ���
                unrecieveFrameTime = unrecieveFrameTime + 1;
                // 6��δ�յ��κα���
                if (unrecieveFrameTime  == 3)
                {
                    LinkDisconnectOnChannelMonitor();
                }
            }
        }

        /// <summary>
        /// ͨ��������·�Ͽ�
        /// </summary>
        private void LinkDisconnectOnChannelMonitor()
        {
            // δ�յ��κα��ģ��رռ�ʱ��
            testFrameTimerForChannelMonitor.Stop();
            // ��ʾ�û�
            MessageBox.Show("ͨ��������·���ӳ�ʱ����ر�ͨ�����Ӵ���", "����");
            // �ر���������
            running = false;
        }
    }
}

