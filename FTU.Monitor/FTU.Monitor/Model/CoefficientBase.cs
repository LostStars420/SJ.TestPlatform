using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// CoefficientBase 的摘要说明
    /// author: songminghao
    /// date：2017/10/23 16:01:09
    /// desc：系数校准基础类
    /// version: 1.0
    /// </summary>
    public class CoefficientBase : ObservableObject
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public CoefficientBase()
        {
            this._averageValue = 0;
            this._rate = 1;
            this._data = new float[10];
        }

        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="number">序号</param>
        /// <param name="ID"></param>
        /// <param name="name"></param>
        public CoefficientBase(int number, int ID, string name)
        {
            this._number = number;
            this._ID = ID;
            this._name = name;
        }

        /// <summary>
        /// 多选框选中bool值
        /// </summary>
        private bool _selected;

        /// <summary>
        /// 获取和设置多选框选中bool值
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
        /// 名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 获取和设置名称
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
        /// 点号
        /// </summary>
        private int _ID;

        /// <summary>
        /// 获取和设置点号
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
        /// 平均值
        /// </summary>
        private float _averageValue;

        /// <summary>
        /// 获取和设置平均值
        /// </summary>
        public float AverageValue
        {
            get
            {
                return this._averageValue;
            }
            set
            {
                this._averageValue = value;
                RaisePropertyChanged(() => AverageValue);
            }
        }

        /// <summary>
        /// 参数值
        /// </summary>
        private float _value;

        /// <summary>
        /// 获取和设置参数值
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
        /// 倍率
        /// </summary>
        private int _rate;

        /// <summary>
        /// 获取和设置倍率
        /// </summary>
        public int Rate
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
        /// 浮点数数组
        /// </summary>
        private float[] _data;

        /// <summary>
        /// 获取和设置浮点数数组
        /// </summary>
        public float[] Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
                RaisePropertyChanged(() => Data);
            }
        }

    }
}
