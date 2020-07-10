using System.Collections.Generic;

namespace FTU.Monitor.EncryptExportModel
{
    /// <summary>
    /// ConstantParameterExportModel 的摘要说明
    /// author: liyan
    /// date：2018/4/26 13:51:45
    /// desc：定值参数导出模型类(包括固有定值参数和定值参数0区、1区、2区)
    /// version: 1.0
    /// </summary>
    public class ConstantParameterExportModel : EncryptExportModelBase
    {
        /// <summary>
        /// 固有参数对象列表
        /// </summary>
        private IList<ContantParameterForExport> _constantParameterList;

        /// <summary>
        /// 设置和获取固有参数对象列表
        /// </summary>
        public IList<ContantParameterForExport> ConstantParameterList
        {
            get
            {
                return this._constantParameterList;
            }
            set
            {
                this._constantParameterList = value;
            }
        }

        /// <summary>
        /// 定值0区参数对象列表
        /// </summary>
        private IList<ContantParameterForExport> _constantParameterZeroList;

        /// <summary>
        /// 设置和获取定值0区参数对象列表
        /// </summary>
        public IList<ContantParameterForExport> ConstantParameterZeroList
        {
            get
            {
                return this._constantParameterZeroList;
            }
            set
            {
                this._constantParameterZeroList = value;
            }
        }

        /// <summary>
        /// 定值1区参数对象列表
        /// </summary>
        private IList<ContantParameterForExport> _constantParameterOneList;

        /// <summary>
        /// 设置和获取定值1区参数对象列表
        /// </summary>
        public IList<ContantParameterForExport> ConstantParameterOneList
        {
            get
            {
                return this._constantParameterOneList;
            }
            set
            {
                this._constantParameterOneList = value;
            }
        }

        /// <summary>
        /// 定值2区参数对象列表
        /// </summary>
        private IList<ContantParameterForExport> _constantParameterTwoList;

        /// <summary>
        /// 设置和获取定值2区参数对象列表
        /// </summary>
        public IList<ContantParameterForExport> ConstantParameterTwoList
        {
            get
            {
                return this._constantParameterTwoList;
            }
            set
            {
                this._constantParameterTwoList = value;
            }
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public ConstantParameterExportModel()
        {
            this._constantParameterList = new List<ContantParameterForExport>();
            this._constantParameterZeroList = new List<ContantParameterForExport>();
            this._constantParameterOneList = new List<ContantParameterForExport>();
            this._constantParameterTwoList = new List<ContantParameterForExport>();

        }

    }
}
