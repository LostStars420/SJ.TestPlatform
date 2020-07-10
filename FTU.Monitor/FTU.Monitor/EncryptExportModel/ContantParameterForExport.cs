namespace FTU.Monitor.EncryptExportModel
{
    /// <summary>
    /// ContantParameterForExport 的摘要说明
    /// author: liyan
    /// date：2018/5/15 17:19:09
    /// desc：为定值点表导入导出专门打造的类，直接对VALUE字段赋值，不将其限定在最大值与最小值范围内
    /// version: 1.0
    /// </summary>
    public class ContantParameterForExport
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
                this._value = value;
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
            }
        }

        /// <summary>
        /// 定值能否使用标志位
        /// </summary>
        private int _enable;

        /// <summary>
        /// 获取和设置定值能否使用标志位
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

    }
}
