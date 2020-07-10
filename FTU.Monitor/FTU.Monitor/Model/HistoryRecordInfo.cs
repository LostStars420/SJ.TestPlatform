using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// HistoryRecordInfo 的摘要说明
    /// author: liyan
    /// date：2018/4/11 15:16:33
    /// desc：历史文件基本信息
    /// version: 1.0
    /// </summary>
    public class HistoryRecordInfo : ObservableObject
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        private string _fileType;

        /// <summary>
        /// 设置和获取文件类型
        /// </summary>
        public string FileType
        {
            get
            {
                return this._fileType;
            }
            set
            {
                this._fileType = value;
                RaisePropertyChanged(() => FileType);
            }
        }

        /// <summary>
        /// 文件版本
        /// </summary>
        private string _fileVersion;

        /// <summary>
        /// 设置和获取文件版本
        /// </summary>
        public string FileVersion
        {
            get
            {
                return this._fileVersion;
            }
            set
            {
                this._fileVersion = value;
                RaisePropertyChanged(() => FileVersion);
            }
        }

        /// <summary>
        /// 终端名称
        /// </summary>
        private string _devName;

        /// <summary>
        /// 设置和获取终端名称
        /// </summary>
        public string DevName
        {
            get
            {
                return this._devName;
            }
            set
            {
                this._devName = value;
                RaisePropertyChanged(() => DevName);
            }
        }

        /// <summary>
        /// 记录个数
        /// </summary>
        private int _recordCounts;

        /// <summary>
        /// 设置和获取记录个数
        /// </summary>
        public int RecordCounts
        {
            get
            {
                return this._recordCounts;
            }
            set
            {
                this._recordCounts = value;
                RaisePropertyChanged(() => RecordCounts);
            }
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public HistoryRecordInfo()
        {

        }

        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="fileType">文件类型</param>
        /// <param name="fileVersion">文件版本</param>
        /// <param name="devName">终端名称</param>
        /// <param name="recordCounts">记录个数</param>
        public HistoryRecordInfo(string fileType, string fileVersion, string devName, int recordCounts)
        {
            this._fileType = fileType;
            this._fileVersion = fileVersion;
            this._devName = devName;
            this._recordCounts = recordCounts;
        }

    }
}
