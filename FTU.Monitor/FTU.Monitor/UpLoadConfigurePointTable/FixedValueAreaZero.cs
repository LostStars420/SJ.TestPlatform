using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.UpLoadConfigurePointTable
{
    /// <summary>
    /// FixedValueAreaZero 的摘要说明
    /// author: liyan
    /// date：2018/5/24 11:38:48
    /// desc：
    /// version: 1.0
    /// </summary>
    class FixedValueAreaZero
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

        public FixedValueAreaZero()
        {
            _pContent = new List<string>();
        }
    }
}
