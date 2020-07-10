using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using FTU.Monitor.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.Service
{
    /// <summary>
    /// ParameterManageService 的摘要说明
    /// author: liyan
    /// date：2018/6/25 16:03:02
    /// desc：定值参数业务逻辑处理(service)类
    /// version: 1.0
    /// </summary>
    public class ParameterManageService
    {
        /// <summary>
        /// 导入Excel定值参数数据
        /// </summary>
        /// <param name="sheetName">sheet名称</param>
        /// <returns></returns>
        public IList<ConstantParameter> ImportParameterData(string sheetName)
        {
            // 查询Excel文件中要导入指定sheet名里的数据的sql语句
            string sqlExcel = string.Format("select * from [SOE$]");
            // 从选择的Excel文件中提取数据，存到DataTable表对象中
            object obj = ReportUtil.GetExcelSheetData(sqlExcel);
            // 判断返回值类型
            if (obj.GetType() == typeof(string))
            {
                if (obj == null || obj.ToString().Trim().Length == 0)
                {
                    return null;
                }

                // MessageBox.Show(obj.ToString(), "提示");
                return null;
            }

            // 将返回对象转换为System.Data.DataTable类型
            System.Data.DataTable dt = (System.Data.DataTable)obj;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    IList<ConstantParameter> parameterList = new List<ConstantParameter>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ConstantParameter u = new ConstantParameter();
                        u.Number = i;
                        u.ID = dt.Rows[i][0].ToString();
                        u.Name = dt.Rows[i][1].ToString();
                        // string s = dt.Rows[i][2].ToString();
                        u.Value = float.Parse(dt.Rows[i][2].ToString());
                        u.Unit = dt.Rows[i][3].ToString();
                        u.Comment = dt.Rows[i][4].ToString();
                        parameterList.Add(u);
                    }
                    return parameterList;

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return null;

        }

        /// <summary>
        /// 导出定值参数数据到Excel
        /// </summary>
        /// <param name="name">sheet名称</param>
        /// <param name="parameterList">要导出的定值参数数据集合</param>
        public string ExportParameterData(string name, IList<ConstantParameter> parameterList)
        {
            // 首先建立接收将要导出的数据集合,这些数据都存于DataTable中  
            System.Data.DataTable dt = new System.Data.DataTable();
            //dt.Columns.Add("Number", typeof(int));
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(float));
            dt.Columns.Add("Unit", typeof(string));
            dt.Columns.Add("MinValue", typeof(float));
            dt.Columns.Add("MaxValue", typeof(float));
            dt.Columns.Add("Comment", typeof(string));
            dt.Columns.Add("Rate", typeof(int));

            DataRow row;
            // 创建Excel  
            for (int i = 0; i < parameterList.Count; i++)
            {
                row = dt.NewRow();
                //row["Number"] = parameterList[i].Number;
                row["ID"] = parameterList[i].ID;
                row["Name"] = parameterList[i].Name;
                row["Value"] = parameterList[i].Value;
                row["Unit"] = parameterList[i].Unit;
                row["MinValue"] = parameterList[i].MinValue;
                row["MaxValue"] = parameterList[i].MaxValue;
                row["Comment"] = parameterList[i].Comment;
                row["Rate"] = 0;
                dt.Rows.Add(row);
            }

            // 设置导出的Excel文件中sheet的列标题
            IList<string> titleList = new List<string>();
            titleList.Add("点号");
            titleList.Add("名称");
            titleList.Add("值");
            titleList.Add("单位");
            titleList.Add("最小值");
            titleList.Add("最大值");
            titleList.Add("备注");
            titleList.Add("倍率");

            // 导出到Excel
            return ReportUtil.ExportExcel(name, titleList, dt);

        }

        /// <summary>
        /// 根据点号类型编号获取定值参数点表集合
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <returns>定值参数点表集合</returns>
        public IList<ConstantParameter> GetConstantParameter(int pointTypeId)
        {
            // 获取定值参数点表
            DevPointDao devPointDao = new DevPointDao();
            IList<DevPoint> devPointList = devPointDao.queryByPointTypeId(pointTypeId);

            if (devPointList != null && devPointList.Count > 0)
            {
                IList<ConstantParameter> constantParameterList = new List<ConstantParameter>();
                for (int i = 0; i < devPointList.Count; i++)
                {
                    ConstantParameter constantParameter = new ConstantParameter();
                    constantParameter.Number = i;
                    constantParameter.MinValue = devPointList[i].MinValue;
                    constantParameter.MaxValue = devPointList[i].MaxValue;
                    constantParameter.ID = devPointList[i].ID;
                    constantParameter.Name = devPointList[i].Name;
                    constantParameter.Unit = devPointList[i].Unit;
                    constantParameter.Comment = devPointList[i].Comment;
                    constantParameter.Selected = false;
                    constantParameter.Enable = devPointList[i].Enable;
                    constantParameter.Value = (float)devPointList[i].Value;
                    constantParameter.DefaultValue = (float)devPointList[i].DefaultValue;

                    constantParameterList.Add(constantParameter);

                }
                return constantParameterList;
            }

            return null;
        }

        /// <summary>
        /// 更新终端软件程序版本号
        /// </summary>
        /// <param name="programVersion">程序版本号</param>
        /// <param name="programVersionId">程序版本号编号</param>
        /// <returns>失败成功标志</returns>
        public bool UpdateProgramVersion(string programVersion, int programVersionId)
        {
            ProgramVersionDao programVersionDao = new ProgramVersionDao();
            int flag = programVersionDao.UpdateProgramVersion(programVersion, programVersionId);
            return flag > 0 ? true : false;
        }

    }
}
