using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// FaultEvent 的摘要说明
    /// author: liyan
    /// date：2018/6/26 14:44:11
    /// desc：故障事件模型类
    /// version: 1.0
    /// </summary>
    public class FaultEvent : ObservableObject
    {
        /// <summary>
        /// 序号
        /// </summary>
        private int _number;

        /// <summary>
        /// 获取和设置序号
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
        /// 点号
        /// </summary>
        private int _ID;

        /// <summary>
        /// 设置和获取点号
        /// </summary>
        public int ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this._ID = value;
                RaisePropertyChanged(() => ID);
            }
        }

        /// <summary>
        /// 时间
        /// </summary>
        private string _time;

        /// <summary>
        /// 获取和设置时间
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
        /// 内容
        /// </summary>
        private string _content;

        /// <summary>
        /// 获取和设置内容
        /// </summary>
        public string Content
        {
            get
            {
                return this._content;
            }
            set
            {
                this._content = value;
                RaisePropertyChanged(() => Content);
            }
        }

    }
}
