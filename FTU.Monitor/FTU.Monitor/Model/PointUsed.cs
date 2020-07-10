
namespace FTU.Monitor.Model
{
    /// <summary>
    /// PointUsed 的摘要说明
    /// author: songminghao
    /// date：2017/11/30 14:05:39
    /// desc：使用的设备点号类，与SQLite数据库中使用的设备点号表对应
    /// version: 1.0
    /// </summary>
    public class PointUsed : DevPoint
    {
        /// <summary>
        /// 流水号
        /// </summary>
        private int _puid;

        /// <summary>
        /// 设置和获取流水号
        /// </summary>
        public int Puid
        {
            get
            {
                return this._puid;
            }
            set
            {
                this._puid = value;
                RaisePropertyChanged(() => Puid);
            }
        }

        /// <summary>
        /// 序号
        /// </summary>
        private int _NO;

        /// <summary>
        /// 获取和设置序号
        /// </summary>
        public int NO
        {
            get
            {
                return this._NO;
            }
            set
            {
                this._NO = value;
                RaisePropertyChanged(() => NO);
            }
        }

        /// <summary>
        /// 变化遥信标志信息
        /// </summary>
        private int _isChanged;

        /// <summary>
        /// 获取和设置变化遥信标志信息(1代表变化遥信,0代表不变化遥信)
        /// </summary>
        public int IsChanged
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
        private int _isSOE;

        /// <summary>
        /// 获取和设置SOE标志信息(1代表上送SOE,0代表不上送SOE)
        /// </summary>
        public int IsSOE
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
        private int _isNegated;

        /// <summary>
        /// 获取和设置取反标志信息(1代表取反,0代表不取反)
        /// </summary>
        public int IsNegated
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
        private int _doublePoint;

        /// <summary>
        /// 获取和设置单双点标志信息(1代表双点信息,0代表单点信息)
        /// </summary>
        public int DoublePoint
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
        /// 使用倍率
        /// </summary>
        private double _usedRate;

        /// <summary>
        /// 获取和设置使用倍率
        /// </summary>
        public double UsedRate
        {
            get
            {
                return this._usedRate;
            }
            set
            {
                this._usedRate = value;
                RaisePropertyChanged(() => UsedRate);
            }
        }

    }
}
