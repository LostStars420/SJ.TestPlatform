
namespace FTU.Monitor.EncryptExportModel
{
    /// <summary>
    /// EncryptExportModelBase 的摘要说明
    /// author: liyan
    /// date：2018/6/25 16:29:59
    /// desc：加密导出模型基类
    /// version: 1.0
    /// </summary>
    public class EncryptExportModelBase
    {
        /// <summary>
        /// 设终端序列号
        /// </summary>
        private string _deviceSerialNumber;

        /// <summary>
        /// 设置和获取终端序列号
        /// </summary>
        public string DeviceSerialNumber
        {
            get
            {
                return this._deviceSerialNumber;
            }
            set
            {
                this._deviceSerialNumber = value;
            }
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public EncryptExportModelBase()
        {

        }

    }
}
