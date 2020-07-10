using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.Model.DTUConfigurePointTableModelCollection
{
    /// <summary>
    /// DTUEachModulePointTableModel 的摘要说明
    /// author: liyan
    /// date：2018/7/31 16:23:21
    /// desc：DTUEachModulePointTable表格的建模
    /// version: 1.0
    /// </summary>
    public class DTUEachModulePointTableModel
    {
        /// <summary>
        /// 自增字段
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 当前点号类型
        /// </summary>
        public int PointType { get; set; }

        /// <summary>
        /// 当前点号所属的模块
        /// </summary>
        public int BelongToModule { get; set; }

        /// <summary>
        /// 点号
        /// </summary>
        public string PointNumber { get; set; }

        /// <summary>
        /// 点号对应的名称
        /// </summary>
        public string PointName { get; set; }
    }
}
