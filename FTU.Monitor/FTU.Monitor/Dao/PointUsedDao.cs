using FTU.Monitor.Model;
using FTU.Monitor.Util;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace FTU.Monitor.Dao
{
    /// <summary>
    /// PointUsedDao 的摘要说明
    /// author: songminghao
    /// date：2017/11/30 14:13:01
    /// desc：对SQLite数据库中使用的设备点号表进行增删查改操作
    /// version: 1.0
    /// </summary>
    public class PointUsedDao
    {
        /// <summary>
        /// 根据点号类型查询此类型下使用的点号列表
        /// </summary>
        /// <param name="pointType">点号类型</param>
        /// <returns>设备点号列表</returns>
        public IList<PointUsed> queryByPointType(string pointType)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT pu.puid, pu.no, pu.usedRate, dp.*, pt.pointType FROM point_used pu, dev_point dp, point_type pt ");
            //筛选条件
            sb.Append("WHERE pu.devpid = dp.devpid AND dp.pointTypeId = pt.id AND pt.pointType = @pointType ORDER BY pu.no ASC ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@pointType", DbType.String)
                                 };
            //给参数赋值
            parameters[0].Value = pointType;

            return DBHelperSQLite.GetList<PointUsed>(sb.ToString(), parameters);
        }

        /// <summary>
        /// 根据点号类型编号查询此类型下使用的点号列表
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <returns>设备点号列表</returns>
        public IList<PointUsed> queryByPointTypeId(int pointTypeId)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT pu.puid, pu.no, pu.doublePoint, pu.isNegated, pu.usedRate, dp.*, pt.pointType FROM point_used pu, dev_point dp, point_type pt ");
            //筛选条件
            sb.Append("WHERE pu.devpid = dp.devpid AND dp.pointTypeId = pt.id AND dp.pointTypeId = @pointTypeId ORDER BY pu.no ASC ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@pointTypeId", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = pointTypeId;

            return DBHelperSQLite.GetList<PointUsed>(sb.ToString(), parameters);
        }

        /// <summary>
        /// 根据点号类型编号批量添加使用的设备点号信息
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <param name="pointUsedList">设备点号信息对象列表</param>
        /// <returns></returns>
        public void batchInsert(int pointTypeId, IList<PointUsed> pointUsedList)
        {
            if (pointUsedList != null)
            {
                //执行的SQL语句列表
                IList<string> sqlStringList = new List<string>();
                //执行的每条SQL语句对应的参数
                IList<SQLiteParameter[]> commandParametersList = new List<SQLiteParameter[]>();

                StringBuilder delPointUsed = new StringBuilder();
                //删除sql语句
                delPointUsed.Append("DELETE FROM point_used WHERE devpid IN (SELECT pu.devpid FROM point_used pu, dev_point dp WHERE pu.devpid = dp.devpid AND dp.pointTypeId = @pointTypeId) ");

                //设置参数
                SQLiteParameter[] delParameters = {
                                     new SQLiteParameter("@pointTypeId", DbType.Int32)
                                 };
                //给参数赋值
                delParameters[0].Value = pointTypeId;

                sqlStringList.Add(delPointUsed.ToString());
                commandParametersList.Add(delParameters);

                foreach (PointUsed pointUsed in pointUsedList)
                {
                    StringBuilder sb = new StringBuilder();
                    //插入语句
                    sb.Append("INSERT INTO point_used(devpid, no, isChanged, isSOE, isNegated, doublePoint, usedRate) ");
                    sb.Append("VALUES(@devpid, @no, @isChanged, @isSOE, @isNegated, @doublePoint, @usedRate) ");

                    //设置参数
                    SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@devpid", DbType.Int32),
                                     new SQLiteParameter("@no", DbType.Int32),
                                     new SQLiteParameter("@isChanged", DbType.Int32),
                                     new SQLiteParameter("@isSOE", DbType.Int32),
                                     new SQLiteParameter("@isNegated", DbType.Int32),
                                     new SQLiteParameter("@doublePoint", DbType.Int32),
                                     new SQLiteParameter("@usedRate", DbType.Double)
                                 };
                    //给参数赋值
                    parameters[0].Value = pointUsed.Devpid;
                    parameters[1].Value = pointUsed.NO;
                    parameters[2].Value = pointUsed.IsChanged;
                    parameters[3].Value = pointUsed.IsSOE;
                    parameters[4].Value = pointUsed.IsNegated;
                    parameters[5].Value = pointUsed.DoublePoint;
                    parameters[6].Value = pointUsed.UsedRate;

                    sqlStringList.Add(sb.ToString());
                    commandParametersList.Add(parameters);
                }

                DBHelperSQLite.ExecuteBatchSql(sqlStringList, commandParametersList);
            }
        }

        /// <summary>
        /// 批量添加所有使用的设备点号信息
        /// </summary>
        /// <param name="pointUsedList">所有使用的设备点号信息对象列表</param>
        /// <returns></returns>
        public void BatchInsert(IList<PointUsed> pointUsedList)
        {
            if (pointUsedList != null)
            {
                //执行的SQL语句列表
                IList<string> sqlStringList = new List<string>();
                //执行的每条SQL语句对应的参数
                IList<SQLiteParameter[]> commandParametersList = new List<SQLiteParameter[]>();

                StringBuilder delPointUsed = new StringBuilder();
                //删除sql语句
                delPointUsed.Append("DELETE FROM point_used ");
                sqlStringList.Add(delPointUsed.ToString());
                commandParametersList.Add(null);

                // 将自增字段置为0
                StringBuilder delPointUsedSeq = new StringBuilder();
                // 更新自增字段置sql语句
                delPointUsedSeq.Append("UPDATE sqlite_sequence SET seq = 0 WHERE name = 'point_used' ");
                sqlStringList.Add(delPointUsedSeq.ToString());
                commandParametersList.Add(null);

                foreach (PointUsed pointUsed in pointUsedList)
                {
                    StringBuilder sb = new StringBuilder();
                    //插入语句
                    sb.Append("INSERT INTO point_used(devpid, no, isChanged, isSOE, isNegated, doublePoint, usedRate) ");
                    sb.Append("VALUES(@devpid, @no, @isChanged, @isSOE, @isNegated, @doublePoint, @usedRate) ");

                    //设置参数
                    SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@devpid", DbType.Int32),
                                     new SQLiteParameter("@no", DbType.Int32),
                                     new SQLiteParameter("@isChanged", DbType.Int32),
                                     new SQLiteParameter("@isSOE", DbType.Int32),
                                     new SQLiteParameter("@isNegated", DbType.Int32),
                                     new SQLiteParameter("@doublePoint", DbType.Int32),
                                     new SQLiteParameter("@usedRate", DbType.Double)
                                 };
                    //给参数赋值
                    parameters[0].Value = pointUsed.Devpid;
                    parameters[1].Value = pointUsed.NO;
                    parameters[2].Value = pointUsed.IsChanged;
                    parameters[3].Value = pointUsed.IsSOE;
                    parameters[4].Value = pointUsed.IsNegated;
                    parameters[5].Value = pointUsed.DoublePoint;
                    parameters[6].Value = pointUsed.UsedRate;

                    sqlStringList.Add(sb.ToString());
                    commandParametersList.Add(parameters);
                }

                DBHelperSQLite.ExecuteBatchSql(sqlStringList, commandParametersList);
            }
        }

        /// <summary>
        /// 根据点号类型批量添加使用的设备点号信息
        /// </summary>
        /// <param name="pointType">点号类型</param>
        /// <param name="pointUsedList">设备点号信息对象列表</param>
        /// <returns></returns>
        public void batchInsert(string pointType, IList<PointUsed> pointUsedList)
        {
            if (pointUsedList != null)
            {
                //执行的SQL语句列表
                IList<string> sqlStringList = new List<string>();
                //执行的每条SQL语句对应的参数
                IList<SQLiteParameter[]> commandParametersList = new List<SQLiteParameter[]>();

                StringBuilder delPointUsed = new StringBuilder();
                //删除sql语句
                delPointUsed.Append("DELETE FROM point_used IN (SELECT pu.devpid FROM point_used pu, dev_point dp, point_type pt WHERE pu.devpid = dp.devpid AND dp.pointTypeId = pt.id AND pt.pointType = @pointType) ");

                //设置参数
                SQLiteParameter[] delParameters = {
                                     new SQLiteParameter("@pointType", DbType.String)
                                 };
                //给参数赋值
                delParameters[0].Value = pointType;

                sqlStringList.Add(delPointUsed.ToString());
                commandParametersList.Add(delParameters);

                foreach (PointUsed pointUsed in pointUsedList)
                {
                    StringBuilder sb = new StringBuilder();
                    //插入语句
                    sb.Append("INSERT INTO point_used(devpid, no, isChanged, isSOE, isNegated, doublePoint, usedRate) ");
                    sb.Append("VALUES(@devpid, @no, @isChanged, @isSOE, @isNegated, @doublePoint, @usedRate) ");

                    //设置参数
                    SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@devpid", DbType.Int32),
                                     new SQLiteParameter("@no", DbType.Int32),
                                     new SQLiteParameter("@isChanged", DbType.Int32),
                                     new SQLiteParameter("@isSOE", DbType.Int32),
                                     new SQLiteParameter("@isNegated", DbType.Int32),
                                     new SQLiteParameter("@doublePoint", DbType.Int32),
                                     new SQLiteParameter("@usedRate", DbType.Double)
                                 };
                    //给参数赋值
                    parameters[0].Value = pointUsed.Devpid;
                    parameters[1].Value = pointUsed.NO;
                    parameters[2].Value = pointUsed.IsChanged;
                    parameters[3].Value = pointUsed.IsSOE;
                    parameters[4].Value = pointUsed.IsNegated;
                    parameters[5].Value = pointUsed.DoublePoint;
                    parameters[6].Value = pointUsed.UsedRate;

                    sqlStringList.Add(sb.ToString());
                    commandParametersList.Add(parameters);
                }

                DBHelperSQLite.ExecuteBatchSql(sqlStringList, commandParametersList);
            }
        }

    }
}
