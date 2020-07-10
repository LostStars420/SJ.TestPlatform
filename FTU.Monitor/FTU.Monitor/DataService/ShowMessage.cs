using FTU.Monitor.Model;
using FTU.Monitor.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace FTU.Monitor.DataService
{
    /// <summary>
    /// ShowMessage 的摘要说明
    /// author: songminghao
    /// date：2017/11/30 11:19:07
    /// desc：显示报文信息类
    /// version: 1.0
    /// </summary>
    public class ShowMessage
    {
        public static int numb = 1;
        public static string messTemp = "";
        public static string showTemp = "";
        public static Queue messQueue1 = new Queue();
        public static Queue messQueue2 = new Queue();

        public static int numb2 = 1;
        public static string messTemp2 = "";
        public static string showTemp2 = "";
        public static Queue messQueue21 = new Queue();
        public static Queue messQueue22 = new Queue();

        // 通道监视的计数器
        private static int countForChannelMonitor = 1;

        /// <summary>
        /// 显示报文信息
        /// </summary>
        /// <param name="buf">要显示的字节数组</param>
        /// <param name="size">要显示的字节数组长度</param>
        /// <param name="direction">方向说明(接收和发送)</param>
        public static void Show(byte[] buf, int size, string direction)
        {
            StringBuilder builder = new StringBuilder();//用于显示报文

            for (int i = 0; i < size; i++)
            {
                builder.Append(buf[i].ToString("X2") + " ");
            }
            string time = DateTime.Now.ToString("yy-MM-dd HH:mm:ss:fff");

            ProtocolMessage d = new ProtocolMessage();
            CommunicationViewModel.CurrentRawMessageCount++;
            d.Number = (++CommunicationViewModel.RawMessageCount); ;
            d.MessageDirection = direction;
            d.MessageTime = time;
            d.MessageFunction = "0";
            d.MessageContent = builder.ToString();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate()
            {
                Messenger.Default.Send<string>(direction, "ChangeMessageColor");
                if (CommunicationViewModel.CurrentRawMessageCount <= 10)
                {
                    showTemp = "";
                    messQueue1 = messQueue2;
                    messTemp = d.ToString();
                    messQueue1.Enqueue(messTemp);
                    messQueue2 = messQueue1;
                    MainViewModel.outputdata.RawMessageData += messTemp;
                }
                else
                {
                    showTemp = "";
                    messQueue1 = messQueue2;
                    messTemp = d.ToString();
                    if (messQueue1.Count > 0)
                    {
                        messQueue1.Dequeue();
                    }
                    messQueue1.Enqueue(messTemp);

                    messQueue2 = messQueue1;
                    foreach (String c in messQueue1)
                        showTemp += c;
                    MainViewModel.outputdata.RawMessageData = showTemp;
                }

            });
            builder.Clear();
        }

        /// <summary>
        /// 显示报文信息
        /// </summary>
        /// <param name="buf">要显示的字节数组</param>
        /// <param name="size">要显示的字节数组长度</param>
        /// <param name="direction">方向说明(接收和发送)</param>
        public static void Show(List<byte> buf, int size, string direction)
        {
            StringBuilder builder = new StringBuilder();//用于显示报文

            for (int i = 0; i < size; i++)
            {
                builder.Append(buf[i].ToString("X2") + " ");
            }
            string time = DateTime.Now.ToString("yy-MM-dd HH:mm:ss:fff");

            ProtocolMessage d = new ProtocolMessage();
            CommunicationViewModel.CurrentRawMessageCount++;
            d.Number = (++CommunicationViewModel.RawMessageCount); ;
            d.MessageDirection = direction;
            d.MessageTime = time;
            d.MessageFunction = "0";
            d.MessageContent = builder.ToString();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate()
            {
                Messenger.Default.Send<string>(direction, "ChangeMessageColor");
                if (CommunicationViewModel.CurrentRawMessageCount <= 10)
                {
                    showTemp = "";
                    messQueue1 = messQueue2;
                    messTemp = d.ToString();
                    messQueue1.Enqueue(messTemp);
                    messQueue2 = messQueue1;
                    MainViewModel.outputdata.RawMessageData += messTemp;
                }
                else
                {
                    showTemp = "";
                    messQueue1 = messQueue2;
                    messTemp = d.ToString();
                    if (messQueue1.Count > 0)
                    {
                        messQueue1.Dequeue();
                    }
                    messQueue1.Enqueue(messTemp);

                    messQueue2 = messQueue1;
                    foreach (String c in messQueue1)
                        showTemp += c;
                    MainViewModel.outputdata.RawMessageData = showTemp;
                }

            });
            builder.Clear();
        }

        /// <summary>
        /// 显示报文信息
        /// </summary>
        /// <param name="buf">要显示的字节数组</param>
        /// <param name="size">要显示的字节数组长度</param>
        /// <param name="direction">方向说明(接收和发送)</param>
        /// <param name="messageFunction">功能说明</param>
        public static void Show(List<byte> buf, int size, string direction, string messageFunction)
        {
            StringBuilder builder = new StringBuilder();//用于显示报文

            for (int i = 0; i < size; i++)
            {
                builder.Append(buf[i].ToString("X2") + " ");
            }
            string time = DateTime.Now.ToString("yy-MM-dd HH:mm:ss:fff");

            ProtocolMessage d = new ProtocolMessage();
            d.Number = (++CommunicationViewModel.RawMessageCount); ;
            d.MessageDirection = direction;
            d.MessageTime = time;
            d.MessageFunction = messageFunction;
            d.MessageContent = builder.ToString();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate()
            {
                MainViewModel.outputdata.RawMessageData += d.ToString();

            });
            builder.Clear();
        }

        /// <summary>
        /// 显示说明信息
        /// </summary>
        /// <param name="direction">方向说明(接收和发送)</param>
        /// <param name="messageContent">报文内容</param>
        public static void Show(string direction, string messageContent)
        {

            string time = DateTime.Now.ToString("yy-MM-dd HH:mm:ss:fff");

            ProtocolMessage d = new ProtocolMessage();
            d.Number = (++CommunicationViewModel.RawMessageCount); ;
            d.MessageDirection = direction;
            d.MessageTime = time;
            d.MessageFunction = "0";
            d.MessageContent = messageContent;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate()
            {
                MainViewModel.outputdata.RawMessageData += d.ToString();

            });
        }

        /// <summary>
        /// 信息说明显示
        /// </summary>
        /// <param name="s">要显示的字符串</param>
        public static void ParseInformationShow(string s)
        {
            if (numb2 <= 10)
            {
                showTemp2 = "";
                messQueue21 = messQueue22;
                messTemp2 = s;
                messQueue21.Enqueue(messTemp2);
                messQueue22 = messQueue21;
                MainViewModel.outputdata.ParseInformation += s;
                numb2++;
            }
            else
            {
                showTemp2 = "";
                messQueue21 = messQueue22;
                messTemp2 = s;
                messQueue21.Enqueue(messTemp2);
                messQueue21.Dequeue();
                messQueue22 = messQueue21;
                foreach (String c in messQueue21)
                    showTemp2 += c;
                MainViewModel.outputdata.ParseInformation = showTemp2;
                numb2++;
            }

        }

        /*************************************************************************************************/
        /********************************监视功能相关程序部分*********************************************/
        /*************************************************************************************************/
        /// <summary>
        /// 显示通道监视的各种信息
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <param name="parseInfo"></param>
        /// <param name="comment"></param>
        public static void ShowForChannelMonitor(List<byte> buf, int size, string direction, string parseInfo, string comment)
        {
            StringBuilder builder = new StringBuilder();//用于显示报文

            for (int i = 0; i < size; i++)
            {
                builder.Append(buf[i].ToString("X2") + " ");
            }
            string time = DateTime.Now.ToString("yy-MM-dd HH:mm:ss:fff");

            ChannelMonitorShowMessage oneMessage = new ChannelMonitorShowMessage();
            oneMessage.Number = countForChannelMonitor;
            oneMessage.Direction = direction;
            oneMessage.Time = time;
            oneMessage.Frame = builder.ToString();
            oneMessage.ParseFrame = parseInfo;
            oneMessage.Comment = comment;

            countForChannelMonitor++;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate()
            {
                if (ChannelMonitorViewModel.channelMonitorDataMessage.Count > 1000)
                {
                    ChannelMonitorViewModel.channelMonitorDataMessage.Clear();
                    ChannelMonitorViewModel.channelMonitorDataMessage.Add(oneMessage);
                }
                else
                {
                    ChannelMonitorViewModel.channelMonitorDataMessage.Add(oneMessage);
                }
            });
        }
    }
}
