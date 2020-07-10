using FTU.Monitor.Model;
using FTU.Monitor.Util;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace FTU.Monitor.Dao
{
    /// <summary>
    /// PointTypeDao 的摘要说明
    /// author: songminghao
    /// date：2017/12/1 15:25:00
    /// desc：对SQLite数据库中点号类型配置表进行增删查改操作
    /// version: 1.0
    /// </summary>
    public class PointTypeDao
    {
        /// <summary>
        /// 查询所有的点号类型配置信息列表
        /// </summary>
        /// <returns>设备点号列表</returns>
        public IList<PointTypeInfo> query()
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM point_type ORDER BY id ASC");

            return DBHelperSQLite.GetList<PointTypeInfo>(sb.ToString(), null);
        }

        /// <summary>
        /// 根据点号类型编号查询对应的点号类型信息
        /// </summary>
        /// <param name="id">点号类型编号</param>
        /// <returns>点号类型信息</returns>
        public PointTypeInfo queryById(int id)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM point_type ");
            //筛选条件
            sb.Append("WHERE id = @id ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@id", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = id;

            IList<PointTypeInfo> pointTypeInfoList = DBHelperSQLite.GetList<PointTypeInfo>(sb.ToString(), parameters);

            return (pointTypeInfoList == null || pointTypeInfoList.Count <= 0) ? null : pointTypeInfoList[0];  
        }

        /// <summary>
        /// 根据点号类型名称查询对应的点号类型信息
        /// </summary>
        /// <param name="pointType">点号类型名称</param>
        /// <returns>点号类型信息</returns>
        public PointTypeInfo queryById(string pointType)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM point_type ");
            //筛选条件
            sb.Append("WHERE pointType = @pointType ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@pointType", DbType.String)
                                 };
            //给参数赋值
            parameters[0].Value = pointType;

            IList<PointTypeInfo> pointTypeInfoList = DBHelperSQLite.GetList<PointTypeInfo>(sb.ToString(), parameters);

            return (pointTypeInfoList == null || pointTypeInfoList.Count <= 0) ? null : pointTypeInfoList[0];
        }

    }
}
