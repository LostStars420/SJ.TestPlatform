using lib60870;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// IIEC104Handler 的摘要说明
    /// author: liyan
    /// date：2018/7/20 11:24:01
    /// desc：各ViewMode模块接收到ASDU包后的处理函数接口
    /// version: 1.0
    /// </summary>
    public interface IIEC104Handler
    {
        void HandleASDUData(TypeID TI, ASDU asdu);
    }
}
