using FTU.Monitor.Util;
using FTU.Monitor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.DataService
{
    /// <summary>
    /// RedefineSocket 的摘要说明
    /// author: liyan
    /// date：2018/5/18 14:33:24
    /// desc：
    /// version: 1.0
    /// </summary>
    public class RedefineSocket
    {
        /// <summary>
        /// 该值指示 System.Net.Sockets.Socket 是否处于阻止模式
        /// </summary>
        private bool _blocking;
        /// <summary>
        /// 获取或设置一个值，该值指示 System.Net.Sockets.Socket 是否处于阻止模式
        /// </summary>
        public bool Blocking
        {
            get
            {
                return this._socket.Blocking;
            }
            set
            {
                this._socket.Blocking = value;
                this._blocking = value;
            }
        }
        /// <summary>
        /// 获取已经从网络接收且可供读取的数据量
        /// </summary>
        public int Available
        {
            get
            {
                return this._socket.Available;
            }
        }
        /// <summary>
        /// System.Boolean 值，该值指定流 System.Net.Sockets.Socket 是否正在使用 Nagle 算法。
        /// </summary>
        private bool _noDelay;
        /// <summary>
        /// 获取或设置 System.Boolean 值，该值指定流 System.Net.Sockets.Socket 是否正在使用 Nagle 算法。
        /// </summary>
        public bool NoDelay
        {
            get
            {
                return this._noDelay;
            }
            set
            {
                this._socket.NoDelay = value;
                this._noDelay = value;
            }
        }

        /// <summary>
        /// 指定之后同步 Overload:System.Net.Sockets.Socket.Receive 调用将
        /// 的时间长度
        /// </summary>
        private int _receiveTimeout;

        /// <summary>
        /// 获取或设置一个值，该值指定之后同步 Overload:System.Net.Sockets.Socket.Receive 调用将超时的时间长度
        /// </summary>
        public int ReceiveTimeout
        {
            get
            {
                return this._receiveTimeout;
            }
            set
            {
                this._socket.ReceiveTimeout = value;
                this._receiveTimeout = value;
            }
        }
        /// <summary>
        /// 获取远程终结点
        /// </summary>
        public EndPoint RemoteEndPoint
        {
            get
            {
                return this._socket.RemoteEndPoint;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="addressFamily">IP地址协议族</param>
        /// <param name="socketType">套接字类型</param>
        /// <param name="protocolType">支持的协议</param>
        public RedefineSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            this._socket = new Socket(addressFamily, socketType, protocolType);
        }

        /// <summary>
        /// 系统套接字对象
        /// </summary>
        private Socket _socket;

        /// <summary>
        /// 开始一个对远程主机连接的异步请求
        /// </summary>
        /// <param name="remoteEP">它表示远程主机</param>
        /// <param name="callback">System.AsyncCallback 委托</param>
        /// <param name="state">一个对象，它包含此请求的状态信息</param>
        /// <returns></returns>
        public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
        {
            return this._socket.BeginConnect(remoteEP, callback, state);
        }

        /// <summary>
        /// 结束挂起的异步连接请求
        /// </summary>
        /// <param name="asyncResult">它存储此异步操作的状态信息以及所有用户定义数据</param>
        public void EndConnect(IAsyncResult asyncResult)
        {
            this._socket.EndConnect(asyncResult);
        }

        /// <summary>
        /// 关闭 System.Net.Sockets.Socket 连接并释放所有关联的资源
        /// </summary>
        public void Close()
        {
            this._socket.Close();
        }

        /// <summary>
        /// 确定 System.Net.Sockets.Socket 的状态
        /// </summary>
        /// <param name="microSeconds">等待响应的时间（以微秒为单位）</param>
        /// <param name="mode">System.Net.Sockets.SelectMode 值之一</param>
        /// <returns></returns>
        public bool Poll(int microSeconds, SelectMode mode)
        {
            return this._socket.Poll(microSeconds, mode);
        }

        /// <summary>
        /// 禁用某 System.Net.Sockets.Socket 上的发送和接收
        /// </summary>
        /// <param name="how">System.Net.Sockets.SocketShutdown 值之一，它指定不再允许执行的操作</param>
        public void Shutdown(SocketShutdown how)
        {
            this._socket.Shutdown(how);
        }

        /// <summary>
        /// 非监听模式下，网口发送字节的方法
        /// </summary>
        /// <param name="buffer">System.Byte 类型的数组，它包含要发送的数据</param>
        /// <returns>已发送到 System.Net.Sockets.Socket 的字节数</returns>
        public int Send(byte[] buffer)
        {
            while (RedefineSerialPort.IsListeningModel())
            {
                // 休眠1秒
                System.Threading.Thread.Sleep(1000);
            }
            return this._socket.Send(buffer);
        }

        /// <summary>
        /// 非监听模式下，网口发送字节的方法
        /// </summary>
        /// <param name="buffer">类型的数组，它包含要发送的数据</param>
        /// <param name="size">要发送的字节数</param>
        /// <param name="socketFlags">System.Net.Sockets.SocketFlags 值的按位组合</param>
        /// <returns></returns>
        public int Send(byte[] buffer, int size, SocketFlags socketFlags)
        {
            while (RedefineSerialPort.IsListeningModel())
            {
                // 休眠1秒
                System.Threading.Thread.Sleep(1000);
            }

            int ret = -1;
            try
            {
                ret = this._socket.Send(buffer, size, socketFlags);
            }
            catch (ArgumentNullException ae)
            {
                LogHelper.Fatal(typeof(RedefineSocket), "网络写入错误,ArgumentNullException " + ae.Message);
                // 关闭网口连接
                CommunicationViewModel.CloseNetConnect();
            }
            catch (ArgumentOutOfRangeException aor)
            {
                LogHelper.Fatal(typeof(RedefineSocket), "网络写入错误,ArgumentOutOfRangeException " + aor.Message);
                // 关闭网口连接
                CommunicationViewModel.CloseNetConnect();
            }
            catch (ObjectDisposedException ode)
            {
                LogHelper.Fatal(typeof(RedefineSocket), "网络写入错误,ObjectDisposedException " + ode.Message);
                // 关闭网口连接
                CommunicationViewModel.CloseNetConnect();
            }
            catch (SocketException se)
            {
                LogHelper.Fatal(typeof(RedefineSocket), "网络写入错误,SocketException " + se.Message);
                // 关闭网口连接
                CommunicationViewModel.CloseNetConnect();
            }
            return ret;
        }

        /// <summary>
        /// 监听模式下网口的发送字节方法
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public int SendByListeningModel(byte[] buffer)
        {           
            while (!RedefineSerialPort.IsListeningModel())
            {
                // 休眠1秒
                System.Threading.Thread.Sleep(1000);
            }
            int ret = -1;
            try
            {
                ret = this._socket.Send(buffer);
            }
            catch(SocketException ex)
            {
                LogHelper.Fatal(typeof(RedefineSocket), "网络写入错误" + ex.ToString());
            }
            return ret;
        }

        /// <summary>
        /// 非通道监视模式下，接收网口数据的方法
        /// </summary>
        /// <param name="buffer">存储接收到的数据的位置</param>
        /// <param name="offset">buffer 中存储所接收数据的位置</param>
        /// <param name="size">要接收的字节数</param>
        /// <param name="socketFlags">System.Net.Sockets.SocketFlags 值的按位组合</param>
        /// <returns>接收到的字节数</returns>
        public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            if (RedefineSerialPort.IsListeningModel())
            {
                LogHelper.Warn(typeof(RedefineSocket), "listening mode, will return 0");
                return 0;
            }
            return this._socket.Receive(buffer, offset, size, socketFlags);
        }

        /// <summary>
        /// 通道监视模式下，接收网口数据的方法
        /// </summary>
        /// <param name="buffer">存储接收到的数据的位置</param>
        /// <param name="socketFlags">System.Net.Sockets.SocketFlags 值的按位组合</param>
        /// <returns>接收到的字节数</returns>
        public int ReceiveByListeningModel(byte[] buffer, SocketFlags socketFlags)
        {
            if (!RedefineSerialPort.IsListeningModel())
            {
                LogHelper.Warn(typeof(RedefineSocket), "not listening mode, will return 0");
                return 0;
            }
            return this._socket.Receive(buffer, socketFlags);
        }
    }
}
