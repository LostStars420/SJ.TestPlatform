using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// Energy 的摘要说明
    /// author: songminghao
    /// date：2018/6/26 14:20:09
    /// desc：电能量模型类
    /// version: 1.0
    /// </summary>
    public class Energy : ObservableObject
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
        /// 点号名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 设置和获取点号名称
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
        /// 值
        /// </summary>
        private float _value;

        /// <summary>
        /// 获取和设置值
        /// </summary>
        public float Value
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
        /// 单位
        /// </summary>
        private string _unit;

        /// <summary>
        /// 获取和设置单位
        /// </summary>
        public string Unit
        {
            get
            {
                return this._unit;
            }
            set
            {
                this._unit = value;
                RaisePropertyChanged(() => Unit);
            }
        }

        /// <summary>
        /// 倍率
        /// </summary>
        private float _rate;

        /// <summary>
        /// 获取和设置倍率
        /// </summary>
        public float Rate
        {
            get
            {
                return this._rate;
            }
            set
            {
                this._rate = value;
                RaisePropertyChanged(() => Rate);
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

    }
}