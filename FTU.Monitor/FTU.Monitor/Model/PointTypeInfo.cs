using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// PointType 的摘要说明
    /// author: songminghao
    /// date：2017/12/1 15:27:00
    /// desc：点号类型配置类，与SQLite数据库中点号类型配置表对应
    /// version: 1.0
    /// </summary>
    public class PointTypeInfo : ObservableObject
    {
        /// <summary>
        /// 点号类型配置表流水号
        /// </summary>
        private int _ID;

        /// <summary>
        /// 设置和获取点号类型配置表流水号
        /// </summary>
        public int ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this._ID = value;
                RaisePropertyChanged(() => ID);
            }
        }

        /// <summary>
        /// 点号类型
        /// </summary>
        private string _pointType;

        /// <summary>
        /// 获取和设置点号类型
        /// </summary>
        public string PointType
        {
            get
            {
                return this._pointType;
            }
            set
            {
                this._pointType = value;
                RaisePropertyChanged(() => PointType);
            }
        }

    }
}
