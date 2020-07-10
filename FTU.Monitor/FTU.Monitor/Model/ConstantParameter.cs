using GalaSoft.MvvmLight;
using System.ComponentModel;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// ConstantParameter 的摘要说明
    /// author: songminghao
    /// date：2017/10/12 9:48:09
    /// desc：定值参数信息类
    /// version: 1.0
    /// </summary>
    public class ConstantParameter : ObservableObject
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
        /// 定值参数ID
        /// </summary>
        private string _ID;

        /// <summary>
        /// 获取和设置定值参数ID
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
        /// 定值参数名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 获取和设置定值参数名称
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
        /// 定值参数值(字符串型)
        /// </summary>
        private string _stringValue;

        /// <summary>
        /// 获取和设置定值参数值(字符串型)
        /// </summary>
        public string StringValue
        {
            get
            {
                return this._stringValue;
            }
            set
            {
                this._stringValue = value;
                RaisePropertyChanged(() => StringValue);
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
        /// 定值参数值(单精度浮点数型)
        /// </summary>
        private float _value;

        /// <summary>
        /// 获取和设置定值参数值(单精度浮点数型)
        /// </summary>
        public float Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if (value > MaxValue)
                {
                    this._value = (float)MaxValue;
                }
                else if (value < MinValue)
                {
                    this._value = (float)MinValue;
                }
                else
                {
                    this._value = value;
                }
                RaisePropertyChanged(() => Value);
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        private float _defaultValue;

        /// <summary>
        /// 设置和获取默认值
        /// </summary>
        public float DefaultValue
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
