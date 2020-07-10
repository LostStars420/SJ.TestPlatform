using FTU.Monitor.Model;
using System.Collections.Generic;

namespace FTU.Monitor.EncryptExportModel
{
    /// <summary>
    /// DevPointExportModel 的摘要说明
    /// author: liyan
    /// date：2018/4/26 14:03:29
    /// desc：所有点表导出模型类
    /// version: 1.0
    /// </summary>
    public class DevPointExportModel : EncryptExportModelBase
    {
        /// <summary>
        /// 所有点号列表
        /// </summary>
        private IList<DevPoint> _devPointList;

        /// <summary>
        /// 设置和获取所有点号列表
        /// </summary>
        public IList<DevPoint> DevPointList
        {
            get
            {
                return this._devPointList;
            }
            set
            {
                this._devPointList = value;
            }
        }

        /// <summary>
        /// 定值参数导出模型对象(包括固有定值参数和定值参数0区、1区、2区)
        /// </summary>
        private ConstantParameterExportModel _constantParameterExportModel;

        /// <summary>
        /// 设置和获取定值参数导出模型对象(包括固有定值参数和定值参数0区、1区、2区)
        /// </summary>
        public ConstantParameterExportModel ConstantParameterExportModel
        {
            get
            {
                return this._constantParameterExportModel;
            }
            set
            {
                this._constantParameterExportModel = value;
            }
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public DevPointExportModel()
        {
            this._devPointList = new List<DevPoint>();
            this._constantParameterExportModel = new ConstantParameterExportModel();
        }
    }
}
