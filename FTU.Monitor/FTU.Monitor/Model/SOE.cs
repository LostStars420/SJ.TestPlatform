using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// SOE 的摘要说明
    /// author: songminghao
    /// date：2017/10/12 9:48:09
    /// desc：时间顺序记录SOE信息类
    /// version: 1.0
    /// </summary>
    public class SOE : ObservableObject
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public SOE()
        {

        }

        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="number">序号</param>
        /// <param name="ID">SOE信息ID</param>
        /// <param name="time">时间</param>
        public SOE(int number, string ID, string time)
        {
            this._number = number;
            this._ID = ID;
            this._time = time;
        }

        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="number">序号</param>
        /// <param name="ID">SOE信息ID</param>
        /// <param name="time">时间</param>
        /// <param name="content">内容</param>
        public SOE(int number, string ID, string time, string content)
            : this(number, ID, time)
        {
            this._content = content;

        }

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
        /// SOE信息ID
        /// </summary>
        private string _ID;

        /// <summary>
        /// 获取和设置SOE信息ID
        /// </summary>
        public string ID
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
        /// SOE信息名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 获取和设置SOE信息名称
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        /// <summary>
        /// 动作时间,SOE信息记录时间
        /// </summary>
        private string _time;

        /// <summary>
        /// 获取和设置SOE记录时间
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
        /// 遥控结果内容
        /// </summary>
        private string _content;

        /// <summary>
        /// 获取和设置遥控结果内容
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

        /// <summary>
        /// 遥控值
        /// </summary>
        private int _value;

        /// <summary>
        /// 获取和设置遥控值
        /// </summary>
        public int Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        /// <summary>
        /// 备注信息
        /// </summary>
        private string _comment;

        /// <summary>
        /// 获取和设置备注信息
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
