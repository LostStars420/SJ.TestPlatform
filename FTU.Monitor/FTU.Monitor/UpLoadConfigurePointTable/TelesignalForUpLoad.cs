using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.UpLoadConfigurePointTable
{
    /// <summary>
    /// TelesignalForUpLoad 的摘要说明
    /// author: liyan
    /// date：2018/5/29 16:47:31
    /// desc：
    /// version: 1.0
    /// </summary>
    public class TelesignalForUpLoad
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
        /// 当前点号的值
        /// </summary>
        private IList<int> _pVal;

        /// <summary>
        /// 设置和获取当前点号的值
        /// </summary>
        public  IList<int> pVal
        {
            get
            {
                return this._pVal;
            }
            set
            {
                this._pVal = value;
            }
        }

        /// <summary>
        /// 当前遥信点号备注
        /// </summary>
        private IList<string> _pContentYx;

        /// <summary>
        /// 设置和获取当前遥信点号备注
        /// </summary>
        public IList<string> pContentYx
        {
            get
            {
                return this._pContentYx;
            }
            set
            {
                this._pContentYx = value;
            }
        }

        /// <summary>
        /// 当前点号SOE信息
        /// </summary>
        private IList<string> _pContentSoe;

        /// <summary>
        /// 设置和获取当前点号SOE信息
        /// </summary>
        public IList<string> pContentSoe
        {
            get
            {
                return this._pContentSoe;
            }
            set
            {
                this._pContentSoe = value;
            }
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public TelesignalForUpLoad()
        {
            this._pVal = new List<int>();
            this._pContentYx = new List<string>();
            this._pContentSoe = new List<string>();
        }
    }
}
