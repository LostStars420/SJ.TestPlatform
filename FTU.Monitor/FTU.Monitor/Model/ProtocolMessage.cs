using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// ProtocolMessage 的摘要说明
    /// author: songminghao
    /// date：2017/11/30 11:19:07
    /// desc：报文信息类
    /// version: 1.0
    /// </summary>
    public class ProtocolMessage : ObservableObject
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public ProtocolMessage()
        {
        }

        /// <summary>
        /// 序号
        /// </summary>
        private int _number;

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
        /// 方向说明(接收和发送)
        /// </summary>
        private string _messageDirection;

        /// <summary>
        /// 设置和获取方向说明(接收和发送)
        /// </summary>
        public string MessageDirection
        {
            get
            {
                return this._messageDirection;
            }
            set
            {
                this._messageDirection = value;
                RaisePropertyChanged(() => MessageDirection);
            }
        }

        /// <summary>
        /// 时间
        /// </summary>
        private string _messageTime;

        /// <summary>
        /// 设置和获取时间
        /// </summary>
        public string MessageTime
        {
            get
            {
                return this._messageTime;
            }
            set
            {
                this._messageTime = value;
                RaisePropertyChanged(() => MessageTime);
            }
        }

        /// <summary>
        /// 功能说明
        /// </summary>
        private string _messageFunction;

        /// <summary>
        /// 设置和获取功能说明
        /// </summary>
        public string MessageFunction
        {
            get
            {
                return this._messageFunction;
            }
            set
            {
                this._messageFunction = value;
                RaisePropertyChanged(() => MessageFunction);
            }
        }

        /// <summary>
        /// 信息内容
        /// </summary>
        private string _messageContent;

        /// <summary>
        /// 设置和获取信息内容
        /// </summary>
        public string MessageContent
        {
            get
            {
                return this._messageContent;
            }
            set
            {
                this._messageContent = value;
                RaisePropertyChanged(() => MessageContent);

                Messenger.Default.Send<object>(null, "AddMessage");
            }
        }

        /// <summary>
        /// 重写ToString()方法
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return this.Number.ToString() + "  " +
                   this.MessageTime.ToString() + "  " +
                   this.MessageDirection + "  " +
                   this.MessageContent + "\n";
        }

    }
}
