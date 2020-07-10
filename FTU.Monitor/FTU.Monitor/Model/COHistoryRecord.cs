
namespace FTU.Monitor.Model
{
    /// <summary>
    /// COHistoryRecord 的摘要说明
    /// author: songminghao
    /// date：2017/12/22 19:14:10
    /// desc：操作历史记录类
    /// version: 1.0
    /// </summary>
    public class COHistoryRecord : HistoryRecordBase
    {
        /// <summary>
        /// 操作命令
        /// </summary>
        private string _CMD;

        /// <summary>
        /// 设置和获取操作命令
        /// </summary>
        public string CMD
        {
            get
            {
                return this._CMD;
            }
            set
            {
                this._CMD = value;
                RaisePropertyChanged(() => CMD);
            }
        }


    }
}
