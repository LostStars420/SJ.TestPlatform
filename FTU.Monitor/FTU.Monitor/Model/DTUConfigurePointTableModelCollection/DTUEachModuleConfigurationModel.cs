using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.Model.DTUConfigurePointTableModelCollection
{
    /// <summary>
    /// DTUEachModuleConfigurationModel 的摘要说明
    /// author: songminghao
    /// date：2018/8/2 16:20:54
    /// desc：
    /// version: 1.0
    /// </summary>
    public class DTUEachModuleConfigurationModel
    {
        /// <summary>
        /// 配置信息所属模块
        /// </summary>
        public int BelongToModule { get; set; }

        /// <summary>
        /// 当前模块的遥信个数
        /// </summary>
        public int TeleSignalisationNumber { get; set; }

        /// <summary>
        /// 当前模块的遥测个数
        /// </summary>
        public int TeleMeteringNumber { get; set; }

        /// <summary>
        /// 当前模块的遥控个数
        /// </summary>
        public int TeleControlNumber { get; set; }

        /// <summary>
        /// 当前模块从站地址
        /// </summary>
        public int SlaveAddress { get; set; }
    }
}
