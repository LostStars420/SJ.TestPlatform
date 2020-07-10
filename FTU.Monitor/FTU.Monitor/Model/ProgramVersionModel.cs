using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// ProgramVersionModel 的摘要说明
    /// author: songminghao
    /// date：2018/3/30 14:57:45
    /// desc：程序版本号类
    /// version: 1.0
    /// </summary>
    public class ProgramVersionModel : ObservableObject
    {
        /// <summary>
        /// 流水号
        /// </summary>
        private int _id;

        /// <summary>
        /// 设置和获取流水号
        /// </summary>
        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        /// <summary>
        /// 程序版本号
        /// </summary>
        private string _version;

        /// <summary>
        /// 设置和获取程序版本号
        /// </summary>
        public string Version
        {
            get
            {
                return this._version;
            }
            set
            {
                this._version = value;
                RaisePropertyChanged(() => Version);
            }
        }

    }
}
