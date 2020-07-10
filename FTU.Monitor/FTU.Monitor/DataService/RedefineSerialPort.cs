using FTU.Monitor.ViewModel;
using System;
using System.IO.Ports;
using System.Threading;
using System.Windows;

namespace FTU.Monitor.DataService
{
    /// <summary>
    /// RedefineSerialPort 的摘要说明
    /// author: liyan
    /// date：2018/3/30 13:17:40
    /// desc：重新定义串口接口方法
    /// version: 1.0
    /// </summary>
    public class RedefineSerialPort : IDisposable
    {
        /// <summary>
        /// 串口接收数据流事件的代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void SerialDataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e);

        /// <summary>
        /// 串口接收数据流事件
        /// </summary>
        public event SerialDataReceivedEventHandler DataReceived;

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public RedefineSerialPort()
        {
            this._realSerialPort = new SerialPort();
            this._realSerialPort.DataReceived += on_data_received;
        }

        /// <summary>
        /// 系统串口对象
        /// </summary>
        private SerialPort _realSerialPort;

        /// <summary>
        ///获取按字节读方法
        /// </summary>
        public int BytesToRead
        {
            get
            {
                return _realSerialPort.BytesToRead;
            }
        }

        /// <summary>
        /// 自动获取串口号
        /// </summary>
        /// <returns></returns>
        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// 串口号
        /// </summary>
        private string _portName;

        /// <summary>
        /// 设置和获取串口号
        /// </summary>
        public string PortName 
        {
            get
            {                
                return this._portName;
            }
            set
            {
                this._realSerialPort.PortName = value;
                this._portName = value;
            }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        private int _baudRate;

        /// <summary>
        /// 设置和获取波特率
        /// </summary>
        public int BaudRate
        {
            get
            {
                return this._baudRate;
            }
            set
            {
                this._realSerialPort.BaudRate = value;
                this._baudRate = value;
            }
        }

        /// <summary>
        /// 奇偶校验
        /// </summary>
        private Parity _parity;

        /// <summary>
        /// 设置和获取奇偶校验
        /// </summary>
        public Parity Parity
        {
            get
            {
                return this._parity;
            }
            set
            {
                this._realSerialPort.Parity = value;
                this._parity = value;
            }
        }

        /// <summary>
        /// 数据位
        /// </summary>
        private int _dataBits;

        /// <summary>
        /// 设置和获取数据位
        /// </summary>
        public int DataBits
        {
            get
            {
                return this._dataBits;
            }
            set
            {
                this._realSerialPort.DataBits = value;
                this._dataBits = value;
            }
        }

        /// <summary>
        /// 停止位
        /// </summary>
        private StopBits _stopBits;

        /// <summary>
        /// 设置和获取停止位
        /// </summary>
        public StopBits StopBits
        {
            get
            {
                return this._stopBits;
            }
            set
            {
                this._realSerialPort.StopBits = value;
                this._stopBits = value;
            }
        }

        /// <summary>
        /// 串口打开方法
        /// </summary>
        public void Open()
        {
            this._realSerialPort.Open();
        }

        /// <summary>
        /// 获取当前串口是否打开
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return this._realSerialPort.IsOpen;
            }
        }

        /// <summary>
        /// 串口关闭方法
        /// </summary>
        public void Close()
        {
            this._realSerialPort.Close();
        }

        /// <summary>
        /// 串口读方法
        /// </summary>
        /// <param name="buffer">存读取数据的缓冲区</param>
        /// <param name="offset">偏移</param>
        /// <param name="count">读取字节的个数</param>
        /// <returns>实际读取到的字节数</returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            while (IsListeningModel())
            {
                // 休眠1秒
                System.Threading.Thread.Sleep(1000);
            }
            return this._realSerialPort.Read(buffer, offset, count);
        }

        /// <summary>
        /// 串口监视读程序
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int ReadForChannelMonitor(byte[] buffer, int offset, int count)
        {
            while (!IsListeningModel())
            {
                // 休眠1秒
                System.Threading.Thread.Sleep(1000);
            }
            return this._realSerialPort.Read(buffer, offset, count);
        }

        /// <summary>
        /// 串口写方法
        /// </summary>
        /// <param name="buffer">缓冲区写入串口</param>
        /// <param name="offset">偏移</param>
        /// <param name="count">写入串口字节的个数</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                while (IsListeningModel())
                {
                    // 休眠1秒
                    System.Threading.Thread.Sleep(1000);
                }
                this._realSerialPort.Write(buffer, offset, count);
            }
            catch (Exception ex)
            {
                MessageBox.Show("串口写入错误！" + ex.ToString(),"警告");
                CommunicationViewModel.CloseSerialPortConnect();
            }

        }

        /// <summary>
        /// 通道监视写入串口的方法
        /// </summary>
        /// <param name="buffer">缓冲区写入串口</param>
        /// <param name="offset">偏移</param>
        /// <param name="count">写入串口字节的个数</param>
        public void WriteByListeningModel(byte[] buffer, int offset, int count)
        {
            while (!IsListeningModel())
            {
                // 休眠1秒
                System.Threading.Thread.Sleep(1000);
            }
            this._realSerialPort.Write(buffer, offset, count);
        }
       
        /// <summary>
        ///当前串口是否处于监听模式
        /// </summary>
        /// <returns>网络当前状态</returns>
        public static bool IsListeningModel()
        {
            return System.Threading.Interlocked.Read(ref MainViewModel.ChannelMonitorListening) != 0;
        }

        /// <summary>
        /// 串口数据触发事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e"></param>
        void on_data_received(object sender, SerialDataReceivedEventArgs e)
        {
            DataReceived(this, e);
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
            if (_realSerialPort != null)
            {
                _realSerialPort.Close();
                _realSerialPort.Dispose();
            }

        }

        #endregion 实现IDisposable接口，释放资源

    }
}
