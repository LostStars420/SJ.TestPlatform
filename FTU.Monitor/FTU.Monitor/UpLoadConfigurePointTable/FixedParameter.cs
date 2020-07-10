using System.Collections.Generic;

namespace FTU.Monitor.UpLoadConfigurePointTable
{
    /// <summary>
    /// FixedParameter 的摘要说明
    /// author: songminghao
    /// date：2018/5/31 8:37:57
    /// desc：解析终端上传的定值参数模型类
    /// version: 1.0
    /// </summary>
    public class FixedParameter
    {
        /// <summary>
        /// 当前点号是否被使用
        /// </summary>
        private int _enable;

        /// <summary>
        /// 设置和获取当前点号是否被使用
        /// </summary>
        public int enable
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
        /// 当前点号名称
        /// </summary>
        private string _pName;

        /// <summary>
        /// 设置和获取当前点号名称
        /// </summary>
        public string pName
        {
            get
            {
                return this._pName;
            }
            set
            {
                this._pName = value;
            }
        }

        /// <summary>
        /// 当前点号单位
        /// </summary>
        private string _pUnit;

        /// <summary>
        /// 设置和获取当前点号单位
        /// </summary>
        public string pUnit
        {
            get
            {
                return this._pUnit;
            }
            set
            {
                this._pUnit = value;
            }
        }

        /// <summary>
        /// 当前点号最大值
        /// </summary>
        private float _valMax;

        /// <summary>
        /// 设置和获取当前点号最大值
        /// </summary>
        public float valMax
        {
            get
            {
                return this._valMax;
            }
            set
            {
                this._valMax = value;
            }
        }

        /// <summary>
        /// 当前点号最小值
        /// </summary>
        private float _valMin;

        /// <summary>
        /// 设置和获取当前点号最小值
        /// </summary>
        public float valMin
        {
            get
            {
                return this._valMin;
            }
            set
            {
                this._valMin = value;
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        private float _defaultVal;

        /// <summary>
        /// 设置和获取默认值
        /// </summary>
        public float defaultVal
        {
            get
            {
                return this._defaultVal;
            }
            set
            {
                this._defaultVal = value;
            }
        }

        /// <summary>
        /// 当前点号的备注
        /// </summary>
        private IList<string> _pContent;

        /// <summary>
        /// 设置和获取当前点号的备注
        /// </summary>
        public IList<string> pContent
        {
            get
            {
                return this._pContent;
            }
            set
            {
                this._pContent = value;
            }
        }

        /// <summary>
        /// 当前点号备注pContent的附加备注
        /// </summary>
        private string _pNote;

        /// <summary>
        /// 设置和获取当前点号备注pContent的附加备注
        /// </summary>
        public string pNote
        {
            get
            {
                return this._pNote;
            }
            set
            {
                this._pNote = value;
            }
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public FixedParameter()
        {
            this._pContent = new List<string>();
        }

    }
}
