
namespace FTU.Monitor.Model
{
    /// <summary>
    /// FaultEventHistoryRecord 的摘要说明
    /// author: songminghao
    /// date：2017/12/22 19:20:31
    /// desc：故障事件历史记录模型类
    /// version: 1.0
    /// </summary>
    public class FaultEventHistoryRecord : HistoryRecordBase
    {
        /// <summary>
        /// 信息体地址0
        /// </summary>
        private string _IOA0;

        /// <summary>
        /// 设置和获取信息体地址0
        /// </summary>
        public string IOA0
        {
            get
            {
                return this._IOA0;
            }
            set
            {
                this._IOA0 = value;
                RaisePropertyChanged(() => IOA0);
            }
        }

        /// <summary>
        /// 信息体地址0对应值
        /// </summary>
        private string _val0;

        /// <summary>
        /// 设置和获取信息体地址0对应值
        /// </summary>
        public string Val0
        {
            get
            {
                return this._val0;
            }
            set
            {
                this._val0 = value;
                RaisePropertyChanged(() => Val0);
            }
        }

        /// <summary>
        /// 信息体地址1
        /// </summary>
        private string _IOA1;

        /// <summary>
        /// 设置和获取信息体地址1
        /// </summary>
        public string IOA1
        {
            get
            {
                return this._IOA1;
            }
            set
            {
                this._IOA1 = value;
                RaisePropertyChanged(() => IOA1);
            }
        }

        /// <summary>
        /// 信息体地址1对应值
        /// </summary>
        private string _val1;

        /// <summary>
        /// 设置和获取信息体地址1对应值
        /// </summary>
        public string Val1
        {
            get
            {
                return this._val1;
            }
            set
            {
                this._val1 = value;
                RaisePropertyChanged(() => Val1);
            }
        }

        /// <summary>
        /// 信息体地址2
        /// </summary>
        private string _IOA2;

        /// <summary>
        /// 设置和获取信息体地址2
        /// </summary>
        public string IOA2
        {
            get
            {
                return this._IOA2;
            }
            set
            {
                this._IOA2 = value;
                RaisePropertyChanged(() => IOA2);
            }
        }

        /// <summary>
        /// 信息体地址2对应值
        /// </summary>
        private string _val2;

        /// <summary>
        /// 设置和获取信息体地址2对应值
        /// </summary>
        public string Val2
        {
            get
            {
                return this._val2;
            }
            set
            {
                this._val2 = value;
                RaisePropertyChanged(() => Val2);
            }
        }

        /// <summary>
        /// 信息体地址3
        /// </summary>
        private string _IOA3;

        /// <summary>
        /// 设置和获取信息体地址3
        /// </summary>
        public string IOA3
        {
            get
            {
                return this._IOA3;
            }
            set
            {
                this._IOA3 = value;
                RaisePropertyChanged(() => IOA3);
            }
        }

        /// <summary>
        /// 信息体地址3对应值
        /// </summary>
        private string _val3;

        /// <summary>
        /// 设置和获取信息体地址3对应值
        /// </summary>
        public string Val3
        {
            get
            {
                return this._val3;
            }
            set
            {
                this._val3 = value;
                RaisePropertyChanged(() => Val3);
            }
        }

        /// <summary>
        /// 信息体地址4
        /// </summary>
        private string _IOA4;

        /// <summary>
        /// 设置和获取信息体地址4
        /// </summary>
        public string IOA4
        {
            get
            {
                return this._IOA4;
            }
            set
            {
                this._IOA4 = value;
                RaisePropertyChanged(() => IOA4);
            }
        }

        /// <summary>
        /// 信息体地址4对应值
        /// </summary>
        private string _val4;

        /// <summary>
        /// 设置和获取信息体地址4对应值
        /// </summary>
        public string Val4
        {
            get
            {
                return this._val4;
            }
            set
            {
                this._val4 = value;
                RaisePropertyChanged(() => Val4);
            }
        }

        /// <summary>
        /// 信息体地址5
        /// </summary>
        private string _IOA5;

        /// <summary>
        /// 设置和获取信息体地址5
        /// </summary>
        public string IOA5
        {
            get
            {
                return this._IOA5;
            }
            set
            {
                this._IOA5 = value;
                RaisePropertyChanged(() => IOA5);
            }
        }

        /// <summary>
        /// 信息体地址5对应值
        /// </summary>
        private string _val5;

        /// <summary>
        /// 设置和获取信息体地址5对应值
        /// </summary>
        public string Val5
        {
            get
            {
                return this._val5;
            }
            set
            {
                this._val5 = value;
                RaisePropertyChanged(() => Val5);
            }
        }

        /// <summary>
        /// 信息体地址6
        /// </summary>
        private string _IOA6;

        /// <summary>
        /// 设置和获取信息体地址6
        /// </summary>
        public string IOA6
        {
            get
            {
                return this._IOA6;
            }
            set
            {
                this._IOA6 = value;
                RaisePropertyChanged(() => IOA6);
            }
        }

        /// <summary>
        /// 信息体地址6对应值
        /// </summary>
        private string _val6;

        /// <summary>
        /// 设置和获取信息体地址6对应值
        /// </summary>
        public string Val6
        {
            get
            {
                return this._val6;
            }
            set
            {
                this._val6 = value;
                RaisePropertyChanged(() => Val6);
            }
        }

        /// <summary>
        /// 信息体地址7
        /// </summary>
        private string _IOA7;

        /// <summary>
        /// 设置和获取信息体地址7
        /// </summary>
        public string IOA7
        {
            get
            {
                return this._IOA7;
            }
            set
            {
                this._IOA7 = value;
                RaisePropertyChanged(() => IOA7);
            }
        }

        /// <summary>
        /// 信息体地址7对应值
        /// </summary>
        private string _val7;

        /// <summary>
        /// 设置和获取信息体地址7对应值
        /// </summary>
        public string Val7
        {
            get
            {
                return this._val7;
            }
            set
            {
                this._val7 = value;
                RaisePropertyChanged(() => Val7);
            }
        }

        /// <summary>
        /// 信息体地址8
        /// </summary>
        private string _IOA8;

        /// <summary>
        /// 设置和获取信息体地址8
        /// </summary>
        public string IOA8
        {
            get
            {
                return this._IOA8;
            }
            set
            {
                this._IOA8 = value;
                RaisePropertyChanged(() => IOA8);
            }
        }

        /// <summary>
        /// 信息体地址8对应值
        /// </summary>
        private string _val8;

        /// <summary>
        /// 设置和获取信息体地址8对应值
        /// </summary>
        public string Val8
        {
            get
            {
                return this._val8;
            }
            set
            {
                this._val8 = value;
                RaisePropertyChanged(() => Val8);
            }
        }

        /// <summary>
        /// 信息体地址9
        /// </summary>
        private string _IOA9;

        /// <summary>
        /// 设置和获取信息体地址9
        /// </summary>
        public string IOA9
        {
            get
            {
                return this._IOA9;
            }
            set
            {
                this._IOA9 = value;
                RaisePropertyChanged(() => IOA9);
            }
        }

        /// <summary>
        /// 信息体地址9对应值
        /// </summary>
        private string _val9;

        /// <summary>
        /// 设置和获取信息体地址9对应值
        /// </summary>
        public string Val9
        {
            get
            {
                return this._val9;
            }
            set
            {
                this._val9 = value;
                RaisePropertyChanged(() => Val9);
            }
        }

    }
}
