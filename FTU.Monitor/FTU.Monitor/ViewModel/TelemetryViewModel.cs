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

namespace FTU.Monitor.ViewModel
{
    class TelemetryViewModel: ViewModelBase
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

    }
}
