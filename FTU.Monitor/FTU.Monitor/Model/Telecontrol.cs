using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// Telecontrol 的摘要说明
    /// author: zhengshuiqing
    /// date：2017/10/10 21:25:23
    /// desc：遥控信息Model
    /// version: 1.0
    /// </summary>
    public class Telecontrol : ObservableObject
    {
        /// <summary>
        /// 选择
        /// </summary>
        private bool _selected;

        /// <summary>
        /// 设置和获取选择信息
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
        /// 遥控ID
        /// </summary>
        private int _YKID;

        /// <summary>
        /// 设置和获取遥控ID
        /// </summary>
        public int YKID
        {
            get
            {
                return this._YKID;
            }
            set
            {
                this._YKID = value;
                RaisePropertyChanged(() => YKID);
            }
        }

        /// <summary>
        /// 遥控点号名称
        /// </summary>
        private string _YKName;

        /// <summary>
        /// 设置和获取遥控点号名称
        /// </summary>
        public string YKName
        {
            get
            {
                return this._YKName;
            }
            set
            {
                this._YKName = value;
                RaisePropertyChanged(() => YKName);
            }
        }

        /// <summary>
        /// 遥控结果
        /// </summary>
        private string _YKResoult;

        /// <summary>
        /// 设置和获取遥控结果
        /// </summary>
        public string YKResoult
        {
            get
            {
                return this._YKResoult;
            }
            set
            {
                this._YKResoult = value;
                RaisePropertyChanged(() => YKResoult);
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        private string _YKRemark;

        /// <summary>
        /// 设置和获取备注
        /// </summary>
        public string YKRemark
        {
            get
            {
                return this._YKRemark;
            }
            set
            {
                this._YKRemark = value;
                RaisePropertyChanged(() => YKRemark);
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

    }
}
