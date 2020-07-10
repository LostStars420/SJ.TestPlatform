using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// DevPoint 的摘要说明
    /// author: songminghao
    /// date：2017/11/30 11:24:12
    /// desc：设备点号类，与SQLite数据库中设备点号表对应
    /// version: 1.0
    /// </summary>
    public class DevPoint : ObservableObject
    {
        /// <summary>
        /// 当前点号是否被使用
        /// </summary>
        private int _enable;

        /// <summary>
        /// 设置和获取当前点号是否被使用
        /// </summary>
        public int Enable
        {
            get
            {
                return this._enable;
            }
            set
            {
                this._enable = value;
            }
        }

        /// <summary>
        /// 是否选择标志
        /// </summary>
        private bool _selected;

        /// <summary>
        /// 获取和设置选择标志
        /// </summary>
        public bool Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;
                RaisePropertyChanged(() => Selected);
            }
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
        /// 流水号
        /// </summary>
        private int _devpid;

        /// <summary>
        /// 设置和获取流水号
        /// </summary>
        public int Devpid
        {
            get
            {
                return this._devpid;
            }
            set
            {
                this._devpid = value;
                RaisePropertyChanged(() => Devpid);
            }
        }

        /// <summary>
        /// 点号类型（遥信、遥测等）编号
        /// </summary>
        private int _pointTypeId;

        /// <summary>
        /// 设置和获取点号类型编号
        /// </summary>
        public int PointTypeId
        {
            get
            {
                return this._pointTypeId;
            }
            set
            {
                this._pointTypeId = value;
                RaisePropertyChanged(() => PointTypeId);
            }
        }

        /// <summary>
        /// 点号类型（遥信、遥测等）
        /// </summary>
        private string _pointType;

        /// <summary>
        /// 设置和获取点号类型
        /// </summary>
        public string PointType
        {
            get
            {
                return this._pointType;
            }
            set
            {
                this._pointType = value;
                RaisePropertyChanged(() => PointType);
            }
        }

        /// <summary>
        /// 点号
        /// </summary>
        private string _ID;

        /// <summary>
        /// 设置和获取点号
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
        private double _value;

        /// <summary>
        /// 获取和设置值
        /// </summary>
        public double Value
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
        /// 备注
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

        /// <summary>
        /// 倍率
        /// </summary>
        private double _rate;

        /// <summary>
        /// 获取和设置倍率
        /// </summary>
        public double Rate
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
        /// 组合遥信标志(1代表组合遥信)
        /// </summary>
        private int _flag;

        /// <summary>
        /// 获取和设置组合遥信标志(1代表组合遥信)
        /// </summary>
        public int Flag
        {
            get
            {
                return this._flag;
            }
            set
            {
                this._flag = value;
                RaisePropertyChanged(() => Flag);
            }
        }

        /// <summary>
        /// 定值最小值
        /// </summary>
        private double _minValue;

        /// <summary>
        /// 设置和获取定值最小值
        /// </summary>
        public double MinValue
        {
            get
            {
                return this._minValue;
            }
            set
            {
                this._minValue = value;
                RaisePropertyChanged(() => MinValue);
            }
        }

        /// <summary>
        /// 定值最大值
        /// </summary>
        private double _maxValue;

        /// <summary>
        /// 设置和获取定值最大值
        /// </summary>
        public double MaxValue
        {
            get
            {
                return this._maxValue;
            }
            set
            {
                this._maxValue = value;
                RaisePropertyChanged(() => MaxValue);
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        private double _defaultValue;

        /// <summary>
        /// 设置和获取默认值
        /// </summary>
        public double DefaultValue
        {
            get
            {
                return this._defaultValue;
            }
            set
            {
                this._defaultValue = value;
                RaisePropertyChanged(() => DefaultValue);
            }
        }

    }
}

