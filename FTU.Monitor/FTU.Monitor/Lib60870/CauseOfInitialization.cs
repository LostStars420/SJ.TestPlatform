using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTU.Monitor.DataService
{/// <summary>
    /// 初始化原因 CauseOfInitialization COI
    /// </summary>
    public enum CauseOfInitialization : byte
    {
        /// <summary>
        /// 当地电源合上 0
        /// </summary>
        LocalClosePower = 0,

        /// <summary>
        /// 进程总复位 1
        /// </summary>
        LocalManualReset = 1,

        /// <summary>
        /// 远方复位 2
        /// </summary>
        DistantReset = 2


    }
}
