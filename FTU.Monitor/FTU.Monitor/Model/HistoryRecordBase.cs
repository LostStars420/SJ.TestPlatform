using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// HistoryRecordBase 的摘要说明
    /// author: songminghao
    /// date：2017/12/22 18:49:43
    /// desc：历史记录基类
    /// version: 1.0
    /// </summary>
    public class HistoryRecordBase : ObservableObject
    {
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
        /// 信息体地址
        /// </summary>
        private int _IOA;

        /// <summary>
        /// 设置和获取信息体地址
        /// </summary>
        public int IOA
        {
            get
            {
                return this._IOA;
            }
            set
            {
                this._IOA = value;
                RaisePropertyChanged(() => IOA);
            }
        }

        /// <summary>
        /// 信息体地址点号对应名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 设置和获取信息体地址点号对应名称
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
        /// 时标
        /// </summary>
        private string _TM;

        /// <summary>
        /// 设置和获取时标
        /// </summary>
        public string TM
        {
            get
            {
                return this._TM;
            }
            set
            {
                this._TM = value;
                RaisePropertyChanged(() => TM);
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        private string _val;

        /// <summary>
        /// 设置和获取值
        /// </summary>
        public string Val
        {
            get
            {
                return this._val;
            }
            set
            {
                this._val = value;
                RaisePropertyChanged(() => Val);
            }
        }

    }
}
