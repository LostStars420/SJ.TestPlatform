using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.UpLoadConfigurePointTable
{
    /// <summary>
    /// CalibrateCoefficient 的摘要说明
    /// author: liyan
    /// date：2018/5/25 8:52:20
    /// desc：
    /// version: 1.0
    /// </summary>
    public class CalibrateCoefficient
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
    }
}
