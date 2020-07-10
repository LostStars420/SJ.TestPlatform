using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// ChannelMonitorShowMessage 的摘要说明
    /// author: liyan
    /// date：2018/3/30 19:32:40
    /// desc：通道监视信息显示
    /// version: 1.0
    /// </summary>
    public class ChannelMonitorShowMessage : ObservableObject
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public ChannelMonitorShowMessage()
        {
            // 给序号赋个初值噻，从0开始
            this._number = 0;
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int _number;

        /// <summary>
        /// 设置和获取序号
        /// </summary>
        public int Number
        {
            get
            {
                return this._number;
            }
            set
            {
                this._number = value;
                RaisePropertyChanged(() => Number);
            }
        }

        /// <summary>
        /// 报文传输方向
        /// </summary>
        private string _direction;

        /// <summary>
        /// 设置和获取报文传输方向
        /// </summary>
        public string Direction
        {
            get
            {
                return this._direction;
            }
            set
            {
                this._direction = value;
                RaisePropertyChanged(() => Direction);
            }
        }

        /// <summary>
        /// 接受报文时间
        /// </summary>
        private string _time;

        /// <summary>
        /// 设置和获取接受报文时间
        /// </summary>
        public string Time
        {
            get
            {
                return this._time;
            }
            set
            {
                this._time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        /// <summary>
        /// 显示接收到的报文
        /// </summary>
        private string _frame;

        /// <summary>
        /// 设置和获取显示接收到的报文
        /// </summary>
        public string Frame
        {
            get
            {
                return this._frame;
            }
            set
            {
                this._frame = value;
                RaisePropertyChanged(() => Frame);
            }
        }

        /// <summary>
        /// 显示解析式报文的内容
        /// </summary>
        private string _parseFrame;

        /// <summary>
        /// 设置和获取显示解析式报文的内容
        /// </summary>
        public string ParseFrame
        {
            get
            {
                return this._parseFrame;
            }
            set
            {
                this._parseFrame = value;
                RaisePropertyChanged(() => ParseFrame);
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        private string _comment;

        /// <summary>
        /// 设置和获取备注
        /// </summary>
        public string Comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                this._comment = value;
                RaisePropertyChanged(() => Comment);
            }
        }

    }
}
