using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.UpLoadConfigurePointTable
{
    /// <summary>
    /// PointNumberCollection 的摘要说明
    /// author: liyan
    /// date：2018/5/24 10:52:59
    /// desc：终端上传点表的json格式数据
    /// version: 1.0
    /// </summary>
    public class PointNumberCollection
    {
        /// <summary>
        /// 点表类型
        /// </summary>
        private string _pointTableType;

        /// <summary>
        /// 设置和获取点表类型
        /// </summary>
        public string pointTableType
        {
            get
            {
                return this._pointTableType;
            }
            set
            {
                this._pointTableType = value;
            }
        }

        /// <summary>
        /// 终端产品序列号
        /// </summary>
        private string _productSerialNumber;

        /// <summary>
        /// 设置和获取终端产品序列号
        /// </summary>
        public string productSerialNumber
        {
            get 
            { 
                return this._productSerialNumber; 
            }
            set 
            { 
                this._productSerialNumber = value; 
            }
        }

        /// <summary>
        /// 定值0区点号集合
        /// </summary>
        private IList<FixedParameter> _fixedValueAreaZero;

        /// <summary>
        /// 设置和获取定值0区点号集合
        /// </summary>
        public IList<FixedParameter> FixedValueAreaZero
        {
            get 
            {
                return this._fixedValueAreaZero; 
            }
            set 
            {
                this._fixedValueAreaZero = value; 
            }
        }

        /// <summary>
        /// 定值1和2区点号集合
        /// </summary>
        private IList<FixedParameter> _fixedValueAreaOne;

        /// <summary>
        /// 设置和获取定值1区点号集合
        /// </summary>
        public IList<FixedParameter> FixedValueAreaOne
        {
            get
            {
                return this._fixedValueAreaOne;
            }
            set
            {
                this._fixedValueAreaOne = value;
            }
        }

        /// <summary>
        /// 系数校准点表集合
        /// </summary>
        private IList<CalibrateCoefficient> _calibrateCoefficient;

        /// <summary>
        /// 设置和获取系数校准点表集合
        /// </summary>
        public IList<CalibrateCoefficient> CalibrateCoefficient
        {
            get
            {
                return this._calibrateCoefficient;
            }
            set
            {
                this._calibrateCoefficient = value;
            }
        }

        /// <summary>
        /// 遥测点号集合
        /// </summary>
        private IList<TelemeteringForUpLoad> _telemeteringForUpLoad;

        /// <summary>
        /// 设置和获取遥测点号集合
        /// </summary>
        public IList<TelemeteringForUpLoad> TelemeteringForUpLoad
        {
            get
            {
                return this._telemeteringForUpLoad;
            }
            set
            {
                this._telemeteringForUpLoad = value;
            }
        }

        /// <summary>
        /// 遥信点号集合
        /// </summary>
        private IList<TelesignalForUpLoad> _telesignalForUpLoad;

        /// <summary>
        /// 设置和获取遥信点号集合
        /// </summary>
        public IList<TelesignalForUpLoad> TelesignalForUpLoad
        {
            get
            {
                return this._telesignalForUpLoad;
            }
            set
            {
                this._telesignalForUpLoad = value;
            }
        }

        /// <summary>
        /// 遥控点号集合
        /// </summary>
        private IList<TelecontrolForUpLoad> _telecontrolForUpLoad;

        /// <summary>
        /// 设置和获取遥控点号集合
        /// </summary>
        public IList<TelecontrolForUpLoad> TelecontrolForUpLoad
        {
            get
            {
                return this._telecontrolForUpLoad;
            }
            set
            {
                this._telecontrolForUpLoad = value;
            }
        }

        /// <summary>
        /// 固有参数集合
        /// </summary>
        private IList<FixedParameter> _inherentParameter;

        /// <summary>
        /// 设置和获取固有参数集合
        /// </summary>
        public IList<FixedParameter> InherentParameter
        {
            get
            {
                return this._inherentParameter;
            }
            set
            {
                this._inherentParameter = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PointNumberCollection()
        {
            this._fixedValueAreaZero = new List<FixedParameter>();
            this._fixedValueAreaOne = new List<FixedParameter>();
            this._calibrateCoefficient = new List<CalibrateCoefficient>();
            this._telesignalForUpLoad = new List<TelesignalForUpLoad>();
            this._telemeteringForUpLoad = new List<TelemeteringForUpLoad>();
            this._telecontrolForUpLoad = new List<TelecontrolForUpLoad>();
            this._inherentParameter = new List<FixedParameter>();        
        }

    }
}
