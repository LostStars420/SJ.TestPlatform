using System.Collections.Generic;
using System.Text;
using FTU.Monitor.Util;
using FTU.Monitor.Model;
using System.Data;
using System.Data.SQLite;
using System.Collections;
using System;

namespace FTU.Monitor.Dao
{
    /// <summary>
    /// DevPointDao 的摘要说明
    /// author: songminghao
    /// date：2017/11/30 11:19:07
    /// desc：对SQLite数据库中设备点号表进行增删查改操作
    /// version: 1.0
    /// </summary>
    public class DevPointDao
    {
        /// <summary>
        /// 查询所有的设备点号列表
        /// </summary>
        /// <returns>设备点号列表</returns>
        public IList<DevPoint> query()
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT dp.*, pt.pointType FROM dev_point dp, point_type pt ");
            //筛选条件
            sb.Append("WHERE dp.pointTypeId = pt.id AND dp.id != '0' ORDER BY dp.devpid ");

            return DBHelperSQLite.GetList<DevPoint>(sb.ToString(), null);
        }

        /// <summary>
        /// 根据点号类型编号查询此类型下的点号数量
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <returns>设备点号数量</returns>
        public int GetCountByPointTypeId(int pointTypeId)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT COUNT(*) FROM dev_point dp, point_type pt ");
            //筛选条件
            sb.Append("WHERE dp.pointTypeId = pt.id AND dp.id != '0' AND dp.pointTypeId = @pointTypeId ORDER BY dp.devpid ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@pointTypeId", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = pointTypeId;

            object obj = DBHelperSQLite.GetSingle(sb.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }

            return Convert.ToInt32(obj.ToString());
        }

        /// <summary>
        /// 根据点号类型编号查询此类型下的点号列表
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <returns>设备点号列表</returns>
        public IList<DevPoint> queryByPointTypeId(int pointTypeId)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT dp.*, pt.pointType FROM dev_point dp, point_type pt ");
            //筛选条件
            sb.Append("WHERE dp.pointTypeId = pt.id AND dp.id != '0' AND dp.pointTypeId = @pointTypeId ORDER BY dp.devpid ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@pointTypeId", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = pointTypeId;

            return DBHelperSQLite.GetList<DevPoint>(sb.ToString(), parameters);
        }

        /// <summary>
        /// 根据点号类型编号查询此类型下的所有点号列表(包括三遥空行点号)
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <returns>设备点号列表</returns>
        public IList<DevPoint> queryAllByPointTypeId(int pointTypeId)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT dp.*, pt.pointType FROM dev_point dp, point_type pt ");
            //筛选条件
            sb.Append("WHERE dp.pointTypeId = pt.id AND dp.pointTypeId = @pointTypeId ORDER BY dp.devpid ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@pointTypeId", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = pointTypeId;

            return DBHelperSQLite.GetList<DevPoint>(sb.ToString(), parameters);
        }

        /// <summary>
        /// 根据点号类型编号和组合遥信标志位查询此类型下的点号列表
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <param name="flag">组合遥信标志位(1代表是组合遥信,0代表原始遥信点号)</param>
        /// <returns>设备点号列表</returns>
        public IList<DevPoint> queryByPointTypeIdAndFlag(int pointTypeId, int flag)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT dp.*, pt.pointType FROM dev_point dp, point_type pt ");
            //筛选条件
            sb.Append("WHERE dp.pointTypeId = pt.id AND dp.id != '0' AND dp.flag = @flag AND dp.pointTypeId = @pointTypeId ORDER BY dp.devpid ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@flag", DbType.Int32),
                                     new SQLiteParameter("@pointTypeId", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = flag;
            parameters[1].Value = pointTypeId;

            return DBHelperSQLite.GetList<DevPoint>(sb.ToString(), parameters);
        }

        /// <summary>
        /// 根据点号类型编号和点号查询此类型下的点号信息
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <param name="id">点号</param>
        /// <returns>设备点号信息</returns>
        public DevPoint queryByPointTypeIdAndPoint(int pointTypeId, string id)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT dp.*, pt.pointType FROM dev_point dp, point_type pt ");
            //筛选条件
            sb.Append("WHERE dp.pointTypeId = pt.id AND dp.id = @id AND dp.pointTypeId = @pointTypeId ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@pointTypeId", DbType.Int32),
                                     new SQLiteParameter("@id", DbType.String)
                                 };
            //给参数赋值
            parameters[0].Value = pointTypeId;
            parameters[1].Value = id;

            IList<DevPoint> devPointList = DBHelperSQLite.GetList<DevPoint>(sb.ToString(), parameters);

            return (devPointList == null || devPointList.Count <= 0) ? null : devPointList[0];
        }

        /// <summary>
        /// 根据点号查询此类型下的点号信息
        /// </summary>
        /// <param name="id">点号</param>
        /// <returns>设备点号信息</returns>
        public IList<DevPoint> queryByPoint(string id)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT dp.*, pt.pointType FROM dev_point dp, point_type pt ");
            //筛选条件
            sb.Append("WHERE dp.pointTypeId = pt.id AND dp.id = @id ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@id", DbType.String)
                                 };
            //给参数赋值
            parameters[0].Value = id;

            return DBHelperSQLite.GetList<DevPoint>(sb.ToString(), parameters);
        }

        /// <summary>
        /// 根据点号名称查询此设备点号内容对应列表（模糊查询）
        /// </summary>
        /// <param name="name">点号名称</param>
        /// <returns>设备点号列表</returns>
        public IList<DevPoint> queryByName(string name)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM dev_point ");
            //筛选条件
            sb.Append("WHERE name = @name ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@name", DbType.String)
                                 };
            //给参数赋值
            parameters[0].Value = "%" + name + "%";

            return DBHelperSQLite.GetList<DevPoint>(sb.ToString(), parameters);
        }

        /// <summary>
        /// 根据点号名称查询此设备点号内容对应列表（模糊查询）
        /// </summary>
        /// <param name="id">点号名称</param>
        /// <returns>设备点号列表</returns>
        public DevPoint QueryByID(string id)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM dev_point ");
            //筛选条件
            sb.Append("WHERE id = @id ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@id", DbType.String)
                                 };
            //给参数赋值
            parameters[0].Value = id;

            IList<DevPoint> list = DBHelperSQLite.GetList<DevPoint>(sb.ToString(), parameters);
            return (list == null || list.Count <= 0) ? null : list[0];
        }

        /// <summary>
        /// 根据设备点号表的流水号查询某条设备点号内容
        /// </summary>
        /// <param name="devpid">设备点号表的流水号</param>
        /// <returns>一条设备点号内容</returns>
        public DevPoint query(int devpid)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM dev_point ");
            //筛选条件
            sb.Append("WHERE devpid = @devpid ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@devpid", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = devpid;

            IList<DevPoint> list = DBHelperSQLite.GetList<DevPoint>(sb.ToString(), parameters);

            return (list == null || list.Count <= 0) ? null : list[0];
        }

        /// <summary>
        /// 根据点号类型查询设备点号列表
        /// </summary>
        /// <param name="pointType">点号类型</param>
        /// <returns>设备点号列表</returns>
        public IList<DevPoint> queryByPointType(string pointType)
        {
            StringBuilder sb = new StringBuilder();

            //查询语句
            sb.Append("SELECT dp.*, pt.pointType FROM dev_point dp, point_type pt ");
            //筛选条件
            sb.Append("WHERE dp.pointTypeId = pt.id AND dp.id != '0' AND pt.pointType = @pointType ORDER BY dp.devpid ");

            //设置参数
            SQLiteParameter[] parameters = {                 
                                     new SQLiteParameter("@pointType", DbType.String)
                                 };
            //给参数赋值
            parameters[0].Value = pointType;

            return DBHelperSQLite.GetList<DevPoint>(sb.ToString(), parameters);
        }

        /// <summary>
        /// 根据点号类型和备注内容查询设备点号列表
        /// </summary>
        /// <param name="pointType">点号类型</param>
        /// <param name="comment">备注</param>
        /// <returns>设备点号列表</returns>
        public IList<DevPoint> queryByPointTypeAndComment(string pointType, string comment)
        {
            StringBuilder sb = new StringBuilder();

            //查询语句
            sb.Append("SELECT dp.*, pt.pointType FROM dev_point dp, point_type pt ");
            //筛选条件
            sb.Append("WHERE dp.pointTypeId = pt.id AND dp.id != '0' AND pt.pointType = @pointType AND dp.comment = @comment ORDER BY dp.devpid ");

            //设置参数
            SQLiteParameter[] parameters = {                 
                                     new SQLiteParameter("@pointType", DbType.String),
                                     new SQLiteParameter("@comment", DbType.String)
                                 };
            //给参数赋值
            parameters[0].Value = pointType;
            parameters[1].Value = comment;

            return DBHelperSQLite.GetList<DevPoint>(sb.ToString(), parameters);
        }

        /// <summary>
        /// 根据点号类型编号和备注内容查询设备点号列表
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <param name="comment">备注</param>
        /// <returns>设备点号列表</returns>
        public IList<DevPoint> queryByPointTypeIdAndComment(int pointTypeId, string comment)
        {
            StringBuilder sb = new StringBuilder();

            // 组建通配备注字符串
            List<string> SQLCommentList = new List<string>();
            int startIndex = -1;
            int logicCharIndex = 4;
            for (int i = 0; i < comment.Length; i++)
            {
                if (comment[i] == '&' || comment[i] == '|')
                {
                    string subComment = comment.Substring(startIndex + 1, i - startIndex).Insert(logicCharIndex, "%");
                    SQLCommentList.Add(subComment);
                    startIndex = i;
                }
            }
            string lastSubComment = comment.Substring(startIndex + 1, comment.Length - startIndex - 1).Insert(logicCharIndex, "%");
            SQLCommentList.Add(lastSubComment);

            string SQLComment = "";
            for (int i = 0; i < SQLCommentList.Count; i++)
            {
                SQLComment += SQLCommentList[i];
            }

            //查询语句
            sb.Append("SELECT dp.*, pt.pointType FROM dev_point dp, point_type pt ");
            //筛选条件
            sb.Append("WHERE dp.pointTypeId = pt.id AND dp.id != '0' AND dp.pointTypeId = @pointTypeId AND dp.comment LIKE @SQLComment ORDER BY dp.devpid ");

            //设置参数
            SQLiteParameter[] parameters = {                 
                                     new SQLiteParameter("@pointTypeId", DbType.Int32),
                                     new SQLiteParameter("@SQLComment", DbType.String)
                                 };
            //给参数赋值
            parameters[0].Value = pointTypeId;
            parameters[1].Value = SQLComment;

            return DBHelperSQLite.GetList<DevPoint>(sb.ToString(), parameters);
        }

        /// <summary>
        /// 新增一条设备点号
        /// </summary>
        /// <param name="devPoint">设备点号对象</param>
        public int insert(DevPoint devPoint)
        {
            StringBuilder sb = new StringBuilder();
            //插入语句
            sb.Append("INSERT INTO dev_point(pointTypeId, id, name, value, unit, comment, rate, flag) ");
            sb.Append("VALUES(@pointTypeId, @id, @name, @value, @unit, @comment, @rate, @flag) ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@pointTypeId", DbType.Int32),
                                     new SQLiteParameter("@id", DbType.String),
                                     new SQLiteParameter("@name", DbType.String),
                                     new SQLiteParameter("@value", DbType.Double),
                                     new SQLiteParameter("@unit", DbType.String),
                                     new SQLiteParameter("@comment", DbType.String),
                                     new SQLiteParameter("@rate", DbType.Double),
                                     new SQLiteParameter("@flag", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = devPoint.PointTypeId;
            parameters[1].Value = devPoint.ID;
            parameters[2].Value = devPoint.Name;
            parameters[3].Value = devPoint.Value;
            parameters[4].Value = devPoint.Unit;
            parameters[5].Value = devPoint.Comment;
            parameters[6].Value = devPoint.Rate;
            parameters[7].Value = devPoint.Flag;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

        /// <summary>
        /// 批量添加设备点号信息
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <param name="devPointList">设备点号信息对象列表</param>
        /// <returns></returns>
        public void batchInsert(int pointTypeId, IList<DevPoint> devPointList)
        {
            if (devPointList != null)
            {
                //执行的SQL语句列表
                IList<string> sqlStringList = new List<string>();
                //执行的每条SQL语句对应的参数
                IList<SQLiteParameter[]> commandParametersList = new List<SQLiteParameter[]>();

                #region 删除使用的点号表中相应的点号类型的数据

                StringBuilder delPointUsed = new StringBuilder();
                //删除sql语句
                delPointUsed.Append("DELETE FROM point_used WHERE devpid IN (SELECT pu.devpid FROM point_used pu, dev_point dp WHERE pu.devpid = dp.devpid AND dp.pointTypeId = @pointTypeId) ");

                //设置参数
                SQLiteParameter[] delPointUsedParameters = {
                                     new SQLiteParameter("@pointTypeId", DbType.Int32)
                                 };
                //给参数赋值
                delPointUsedParameters[0].Value = pointTypeId;

                sqlStringList.Add(delPointUsed.ToString());
                commandParametersList.Add(delPointUsedParameters);

                #endregion

                #region 删除点号表中相应的点号类型的数据

                StringBuilder delDevPoint = new StringBuilder();
                //删除sql语句
                delDevPoint.Append("DELETE FROM dev_point WHERE pointTypeId = @pointTypeId ");

                //设置参数
                SQLiteParameter[] delParameters = {
                                     new SQLiteParameter("@pointTypeId", DbType.Int32)
                                 };
                //给参数赋值
                delParameters[0].Value = pointTypeId;

                sqlStringList.Add(delDevPoint.ToString());
                commandParametersList.Add(delParameters);

                #endregion

                foreach (DevPoint devPoint in devPointList)
                {
                    StringBuilder sb = new StringBuilder();
                    //插入语句
                    sb.Append("INSERT INTO dev_point(pointTypeId, id, name, value, unit, comment, rate, flag, minValue, maxValue) ");
                    sb.Append("VALUES(@pointTypeId, @id, @name, @value, @unit, @comment, @rate, @flag, @minValue, @maxValue) ");

                    //设置参数
                    SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@pointTypeId", DbType.Int32),
                                     new SQLiteParameter("@id", DbType.String),
                                     new SQLiteParameter("@name", DbType.String),
                                     new SQLiteParameter("@value", DbType.Double),
                                     new SQLiteParameter("@unit", DbType.String),
                                     new SQLiteParameter("@comment", DbType.String),
                                     new SQLiteParameter("@rate", DbType.Double),
                                     new SQLiteParameter("@flag", DbType.Int32),
                                     new SQLiteParameter("@minValue",DbType.Double),
                                     new SQLiteParameter("@maxValue",DbType.Double)
                                 };
                    //给参数赋值
                    parameters[0].Value = devPoint.PointTypeId;
                    parameters[1].Value = devPoint.ID;
                    parameters[2].Value = devPoint.Name;
                    parameters[3].Value = devPoint.Value;
                    parameters[4].Value = devPoint.Unit;
                    parameters[5].Value = devPoint.Comment;
                    parameters[6].Value = devPoint.Rate;
                    parameters[7].Value = devPoint.Flag;
                    parameters[8].Value = devPoint.MinValue;
                    parameters[9].Value = devPoint.MaxValue;

                    sqlStringList.Add(sb.ToString());
                    commandParametersList.Add(parameters);
                }

                DBHelperSQLite.ExecuteBatchSql(sqlStringList, commandParametersList);
            }

        }

        /// <summary>
        /// 批量添加设备所有点号信息
        /// </summary>
        /// <param name="devPointList">设备所有点号信息对象列表</param>
        /// <returns></returns>
        public void batchInsert(IList<DevPoint> devPointList)
        {
            if (devPointList != null)
            {
                //执行的SQL语句列表
                IList<string> sqlStringList = new List<string>();
                //执行的每条SQL语句对应的参数
                IList<SQLiteParameter[]> commandParametersList = new List<SQLiteParameter[]>();

                #region 删除使用的点号表中相应的点号类型的数据

                StringBuilder delPointUsed = new StringBuilder();
                // 删除sql语句
                delPointUsed.Append("DELETE FROM point_used ");
                sqlStringList.Add(delPointUsed.ToString());
                commandParametersList.Add(null);

                // 将自增字段置为0
                StringBuilder delPointUsedSeq = new StringBuilder();
                // 更新自增字段置sql语句
                delPointUsedSeq.Append("UPDATE sqlite_sequence SET seq = 0 WHERE name = 'point_used' ");
                sqlStringList.Add(delPointUsedSeq.ToString());
                commandParametersList.Add(null);

                #endregion

                #region 删除点号表中相应的点号类型的数据

                StringBuilder delDevPoint = new StringBuilder();
                //删除sql语句
                delDevPoint.Append("DELETE FROM dev_point ");
                sqlStringList.Add(delDevPoint.ToString());
                commandParametersList.Add(null);

                // 将自增字段置为0
                StringBuilder delDevPointSeq = new StringBuilder();
                // 更新自增字段置sql语句
                delDevPointSeq.Append("UPDATE sqlite_sequence SET seq = 0 WHERE name = 'dev_point' ");
                sqlStringList.Add(delDevPointSeq.ToString());
                commandParametersList.Add(null);

                #endregion

                foreach (DevPoint devPoint in devPointList)
                {
                    StringBuilder sb = new StringBuilder();
                    //插入语句
                    sb.Append("INSERT INTO dev_point(pointTypeId, id, name, value, unit, comment, rate, flag, minValue, maxValue, enable, defaultValue) ");
                    sb.Append("VALUES(@pointTypeId, @id, @name, @value, @unit, @comment, @rate, @flag, @minValue, @maxValue, @enable, @defaultValue) ");

                    //设置参数
                    SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@pointTypeId", DbType.Int32),
                                     new SQLiteParameter("@id", DbType.String),
                                     new SQLiteParameter("@name", DbType.String),
                                     new SQLiteParameter("@value", DbType.Double),
                                     new SQLiteParameter("@unit", DbType.String),
                                     new SQLiteParameter("@comment", DbType.String),
                                     new SQLiteParameter("@rate", DbType.Double),
                                     new SQLiteParameter("@flag", DbType.Int32),
                                     new SQLiteParameter("@minValue",DbType.Double),
                                     new SQLiteParameter("@maxValue",DbType.Double),
                                     new SQLiteParameter("@enable",DbType.Int32),
                                     new SQLiteParameter("@defaultValue", DbType.Double)
                                 };
                    //给参数赋值
                    parameters[0].Value = devPoint.PointTypeId;
                    parameters[1].Value = devPoint.ID;
                    parameters[2].Value = devPoint.Name;
                    parameters[3].Value = devPoint.Value;
                    parameters[4].Value = devPoint.Unit;
                    parameters[5].Value = devPoint.Comment;
                    parameters[6].Value = devPoint.Rate;
                    parameters[7].Value = devPoint.Flag;
                    parameters[8].Value = devPoint.MinValue;
                    parameters[9].Value = devPoint.MaxValue;
                    parameters[10].Value = devPoint.Enable;
                    parameters[11].Value = devPoint.DefaultValue;

                    sqlStringList.Add(sb.ToString());
                    commandParametersList.Add(parameters);
                }

                DBHelperSQLite.ExecuteBatchSql(sqlStringList, commandParametersList);
            }

        }

        /// <summary>
        /// 更新一条设备点号信息
        /// </summary>
        /// <param name="devPoint">设备点号信息对象</param>
        public int update(DevPoint devPoint)
        {
            StringBuilder sb = new StringBuilder();
            //更新语句
            sb.Append("UPDATE dev_point SET pointTypeId = @pointTypeId, id = @id, name = @name, value = @value, unit = @unit, comment = @comment, rate = @rate, flag = @flag WHERE devpid = @devpid");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@pointTypeId", DbType.Int32),
                                     new SQLiteParameter("@id", DbType.String),
                                     new SQLiteParameter("@name", DbType.String),
                                     new SQLiteParameter("@value", DbType.Double),
                                     new SQLiteParameter("@unit", DbType.String),
                                     new SQLiteParameter("@comment", DbType.String),
                                     new SQLiteParameter("@rate", DbType.Double),
                                     new SQLiteParameter("@flag", DbType.Int32),
                                     new SQLiteParameter("@devpid", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = devPoint.PointTypeId;
            parameters[1].Value = devPoint.ID;
            parameters[2].Value = devPoint.Name;
            parameters[3].Value = devPoint.Value;
            parameters[4].Value = devPoint.Unit;
            parameters[5].Value = devPoint.Comment;
            parameters[6].Value = devPoint.Rate;
            parameters[7].Value = devPoint.Flag;
            parameters[8].Value = devPoint.Devpid;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条设备点号(定值参数的值)信息
        /// </summary>
        /// <param name="IOA">定值点号</param>
        /// <param name="Value">定值点号对应的值</param>
        public int UpdateFixedParameterValue(string IOA, float Value)
        {
            StringBuilder sb = new StringBuilder();
            //更新语句
            sb.Append("UPDATE dev_point SET value = @Value WHERE id = @IOA");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@Value", DbType.Double),
                                     new SQLiteParameter("@IOA", DbType.String)
                                 };
            //给参数赋值
            parameters[0].Value = Value;
            parameters[1].Value = IOA;
            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }


        /// <summary>
        /// 根据点号流水号删除点号
        /// </summary>
        /// <param name="devpid"></param>
        /// <returns></returns>
        public int DeleteByDevpid(int devpid)
        {
            StringBuilder sb = new StringBuilder();
            // 查询语句,先查询此点号有没有被使用,被使用则不能删除
            sb.Append("SELECT COUNT(*) FROM point_used pu WHERE pu.devpid = @devpid");

            // 设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@devpid", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = devpid;

            object obj = DBHelperSQLite.GetSingle(sb.ToString(), parameters);
            if (obj != null && Convert.ToInt32(obj.ToString()) > 0)
            {
                return -1;
            }

            StringBuilder delDevPointSB = new StringBuilder();
            // 删除语句
            delDevPointSB.Append("DELETE FROM dev_point WHERE devpid = @devpid");

            // 设置参数
            SQLiteParameter[] delDevPointParameters = {
                                     new SQLiteParameter("@devpid", DbType.Int32)
                                 };
            //给参数赋值
            delDevPointParameters[0].Value = devpid;

            return DBHelperSQLite.ExecuteSql(delDevPointSB.ToString(), parameters);

        }

    }
}

