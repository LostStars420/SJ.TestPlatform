using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// Telesignalisation 的摘要说明
    /// author: songminghao
    /// date：2017/10/12 9:48:09
    /// desc：遥信信息类
    /// version: 1.0
    /// </summary>
    public class Telesignalisation : ObservableObject
    {
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
        /// 遥信ID
        /// </summary>
        private int _ID;

        /// <summary>
        /// 获取和设置遥信ID
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
        /// 遥信名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 获取和设置遥信名称
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
        /// 遥信值
        /// </summary>
        private byte _value;

        /// <summary>
        /// 获取和设置遥信值
        /// </summary>
        public byte Value
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
        /// 变化遥信标志信息
        /// </summary>
        private bool _isChanged;

        /// <summary>
        /// 获取和设置变化遥信标志信息(true或1代表变化,false或0代表不变化)
        /// </summary>
        public bool IsChanged
        {
            get
            {
                return this._isChanged;
            }
            set
            {
                this._isChanged = value;
                RaisePropertyChanged(() => IsChanged);
            }
        }

        /// <summary>
        /// SOE标志信息
        /// </summary>
        private bool _isSOE;

        /// <summary>
        /// 获取和设置SOE标志信息(true或1代表上送SOE,false或0代表不上送SOE)
        /// </summary>
        public bool IsSOE
        {
            get
            {
                return this._isSOE;
            }
            set
            {
                this._isSOE = value;
                RaisePropertyChanged(() => IsSOE);
            }
        }

        /// <summary>
        /// 取反标志信息
        /// </summary>
        private bool _isNegated;

        /// <summary>
        /// 获取和设置取反标志信息(true或1代表取反,false或0代表不取反)
        /// </summary>
        public bool IsNegated
        {
            get
            {
                return this._isNegated;
            }
            set
            {
                this._isNegated = value;
                RaisePropertyChanged(() => IsNegated);
            }
        }

        /// <summary>
        /// 单双点标志信息
        /// </summary>
        private bool _doublePoint;

        /// <summary>
        /// 获取和设置单双点标志信息(true或1代表双点信息,false或0代表单点信息)
        /// </summary>
        public bool DoublePoint
        {
            get
            {
                return this._doublePoint;
            }
            set
            {
                this._doublePoint = value;
                RaisePropertyChanged(() => DoublePoint);
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

    }
}
