using GalaSoft.MvvmLight;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// FileModel 的摘要说明
    /// author: songminghao
    /// date：2017/12/9 17:15:40
    /// desc：读取文件基本信息类
    /// version: 1.0
    /// </summary>
    public class FileModel : ObservableObject
    {
        /// <summary>
        /// 是否选择标志
        /// </summary>
        private bool _selected;

        /// <summary>
        /// 获取和设置选择标志
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
        /// 文件所在目录ID
        /// </summary>
        private int _IDOfDirectory;

        /// <summary>
        /// 设置和获取文件所在目录ID
        /// </summary>
        public int IDOfDirectory
        {
            get
            {
                return this._IDOfDirectory;
            }
            set
            {
                this._IDOfDirectory = value;
                RaisePropertyChanged(() => IDOfDirectory);
            }
        }

        /// <summary>
        /// 序号
        /// </summary>
        private int _number;

        /// <summary>
        /// 设置和获取序号
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
        /// 文件名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 设置和获取文件名称
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
        /// 文件大小
        /// </summary>
        private int _size;

        /// <summary>
        /// 设置和获取文件大小
        /// </summary>
        public int Size
        {
            get
            {
                return this._size;
            }
            set
            {
                this._size = value;
                RaisePropertyChanged(() => Size);
            }
        }

        /// <summary>
        /// 时间
        /// </summary>
        private string _time;

        /// <summary>
        /// 设置和获取时间
        /// </summary>
        public string Time
        {
            get
            {
                return this._time;
            }
            set
            {
                this._time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        /// <summary>
        /// 备注信息
        /// </summary>
        private string _remark;

        /// <summary>
        /// 设置和获取备注信息
        /// </summary>
        public string Remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._remark = value;
                RaisePropertyChanged(() => Remark);
            }
        }

    }
}
