using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// OutputData 的摘要说明
    /// author: songminghao
    /// date：2018/4/9 14:59:01
    /// desc：输出信息显示
    /// version: 1.0
    /// </summary>
    public class OutputData : ObservableObject
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public OutputData()
        {
            this._currentArea = "1";
        }

        /// <summary>
        /// 输出调试信息
        /// </summary>
        private string _debug;

        /// <summary>
        /// 设置和获取输出调试信息
        /// </summary>
        public string Debug
        {
            get
            {
                return this._debug;
            }
            set
            {
                this._debug = value;
                RaisePropertyChanged(() => Debug);
            }
        }

        /// <summary>
        /// 显示处理完成的数据(报文数据分析后提示的信息)
        /// </summary>
        private string _parseInformation;

        /// <summary>
        /// 设置和获取显示处理完成的数据(报文数据分析后提示的信息)
        /// </summary>
        public string ParseInformation
        {
            get
            {
                return this._parseInformation;
            }
            set
            {
                this._parseInformation = value;
                RaisePropertyChanged(() => ParseInformation);
            }
        }

        /// <summary>
        /// 端口状态信息(打开或关闭)
        /// </summary>
        private string _portStatus;

        /// <summary>
        /// 设置和获取端口状态信息(打开或关闭)
        /// </summary>
        public string PortStatus
        {
            get
            {
                return this._portStatus;
            }
            set
            {
                this._portStatus = value;
                RaisePropertyChanged(() => PortStatus);
            }
        }

        /// <summary>
        /// 当前定值区号
        /// </summary>
        private string _currentArea;

        /// <summary>
        /// 设置和获取当前定值区号
        /// </summary>
        public string CurrentArea
        {
            get
            {
                return this._currentArea;
            }
            set
            {
                this._currentArea = value;
                RaisePropertyChanged(() => CurrentArea);
            }
        }

        /// <summary>
        /// 报文信息数据(显示报文数据时使用)
        /// </summary>
        private string _rawMessageData;

        /// <summary>
        /// 设置和获取报文信息数据(显示报文数据时使用)
        /// </summary>
        public string RawMessageData
        {
            get
            {
                return this._rawMessageData;
            }
            set
            {
                this._rawMessageData = value;
                RaisePropertyChanged(() => RawMessageData);
            }
        }

    }
}
