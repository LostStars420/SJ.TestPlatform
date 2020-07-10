using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.UpLoadConfigurePointTable
{
    /// <summary>
    /// TelemeteringForUpLoad 的摘要说明
    /// author: liyan
    /// date：2018/5/29 16:45:33
    /// desc：
    /// version: 1.0
    /// </summary>
    public class TelemeteringForUpLoad
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
        private string _pNameUp;

        /// <summary>
        /// 设置和获取当前点号名称
        /// </summary>
        public string pNameUp
        {
            get
            {
                return this._pNameUp;
            }
            set
            {
                this._pNameUp = value;
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
    }
}
