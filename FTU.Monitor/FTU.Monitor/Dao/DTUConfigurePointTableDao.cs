using FTU.Monitor.Model;
using FTU.Monitor.Model.DTUConfigurePointTableModelCollection;
using FTU.Monitor.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.Dao
{
    /// <summary>
    /// DTUConfigurePointTableDao 的摘要说明
    /// author: songminghao
    /// date：2018/7/30 21:09:02
    /// desc：
    /// version: 1.0
    /// </summary>
    public class DTUConfigurePointTableDao
    {
        /// <summary>
        /// 从数据库中查询所有的DTU模块配置信息，树结构
        /// </summary>
        /// <returns>每个节点的信息</returns>
        public IList<DTUNode> Query()
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM DTUConfigureModuleTree ");
            return DBHelperSQLite.GetList<DTUNode>(sb.ToString(), null);
        }

        /// <summary>
        /// 查询数据库中记录的总数
        /// </summary>
        /// <returns></returns>
        public int QueryMaxNodeID()
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT max(NodeID) FROM DTUConfigureModuleTree ");

            object obj = DBHelperSQLite.GetSingle(sb.ToString(), null);
            if (obj == null)
            {
                return -1;
            }

            return Convert.ToInt32(obj.ToString());
        }

        /// <summary>
        /// 新增一个节点
        /// </summary>
        /// <param name="node">节点对象</param>
        public int InsertNode(DTUNode node)
        {
            StringBuilder sb = new StringBuilder();
            //插入语句
            sb.Append("INSERT INTO DTUConfigureModuleTree(NodeName, Path, NodeType) ");
            sb.Append("VALUES(@NodeName, @Path, @NodeType) ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@NodeName", DbType.String),
                                     new SQLiteParameter("@Path", DbType.String),
                                     new SQLiteParameter("@NodeType", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = node.NodeName;
            parameters[1].Value = node.Path;
            parameters[2].Value = node.NodeType;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

        /// <summary>
        /// 根据nodeID删除某一条记录
        /// </summary>
        /// <returns></returns>
        public int DeleteNodeConfigure(int nodeID)
        {
            StringBuilder sb = new StringBuilder();
            // 删除语句
            sb.Append("DELETE FROM DTUConfigureModuleTree WHERE NodeID = @NodeID");

            // 设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@NodeID", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = nodeID;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

        /// <summary>
        /// 更新节点名称
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int UpdateNodeConfigure(DTUNode node)
        {
            StringBuilder sb = new StringBuilder();
            //更新语句
            sb.Append("UPDATE DTUConfigureModuleTree SET NodeName = @NodeName WHERE NodeID = @NodeID");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@NodeName", DbType.String),
                                     new SQLiteParameter("@NodeID",DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = node.NodeName;
            parameters[1].Value = node.NodeID;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

        /// <summary>
        /// 根据点号类型与所属模块查询满足条件的点号列表
        /// </summary>
        /// <returns></returns>
        public IList<DTUEachModulePointTableModel> QueryByPointTypeAndBelongToModule(int PointType, int BelongToModule)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM DTUEachModulePointTable ");
            sb.Append("WHERE PointType = @PointType AND BelongToModule = @BelongToModule");
            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@PointType", DbType.Int32),
                                     new SQLiteParameter("@BelongToModule", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = PointType;
            parameters[1].Value = BelongToModule;

            IList<DTUEachModulePointTableModel> modulePointTable = DBHelperSQLite.GetList<DTUEachModulePointTableModel>(sb.ToString(), parameters);
            return (modulePointTable == null || modulePointTable.Count <= 0) ? null : modulePointTable;
        } 

        /// <summary>
        /// 删除指定模块中的所有点表
        /// </summary>
        /// <param name="belongToModule">指定模块的ID</param>
        /// <returns></returns>
        public int DeleteByBelongToModule(int belongToModule)
        {
            StringBuilder delDevPointSb = new StringBuilder();
            // 删除语句
            delDevPointSb.Append("DELETE FROM DTUEachModulePointTable WHERE BelongToModule = @belongToModule");

            // 设置参数
            SQLiteParameter[] delDevPointParameters = {
                                     new SQLiteParameter("@belongToModule", DbType.Int32)
                                 };
            //给参数赋值
            delDevPointParameters[0].Value = belongToModule;

            return DBHelperSQLite.ExecuteSql(delDevPointSb.ToString(), delDevPointParameters); 

        }

        /// <summary>
        /// 批量增加所配置的点表
        /// </summary>
        /// <param name="pointTables"></param>
        public void InsertConfigurePointTable(List<DTUEachModulePointTableModel> pointTables)
        {
            if(pointTables != null)
            {
                //执行的SQL语句列表
                IList<string> sqlStringList = new List<string>();
                //执行的每条SQL语句对应的参数
                IList<SQLiteParameter[]> commandParametersList = new List<SQLiteParameter[]>();

                foreach(var pointTable in pointTables)
                {
                    StringBuilder sb = new StringBuilder();
                    //插入语句
                    sb.Append("INSERT INTO DTUEachModulePointTable(PointType, BelongToModule, PointNumber, PointName) ");
                    sb.Append("VALUES(@PointType, @BelongToModule, @PointNumber, @PointName) ");

                    //设置参数
                    SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@PointType", DbType.Int32),
                                     new SQLiteParameter("@BelongToModule", DbType.Int32),
                                     new SQLiteParameter("@PointNumber",DbType.String),
                                     new SQLiteParameter("@PointName",DbType.String)
                                 };
                    //给参数赋值
                    parameters[0].Value = pointTable.PointType;
                    parameters[1].Value = pointTable.BelongToModule;
                    parameters[2].Value = pointTable.PointNumber;
                    parameters[3].Value = pointTable.PointName;

                    sqlStringList.Add(sb.ToString());
                    commandParametersList.Add(parameters);
                }
                DBHelperSQLite.ExecuteBatchSql(sqlStringList, commandParametersList);
            }
        }

        /// <summary>
        /// 查询当前选中模块的配置信息
        /// </summary>
        /// <param name="BelongToModule"></param>
        public DTUEachModuleConfigurationModel QueryModuleInfoByBeLongToModule(int BelongToModule)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM DTUEachModuleConfiguration ");
            sb.Append("WHERE BelongToModule = @BelongToModule");
            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@BelongToModule", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = BelongToModule;

            IList<DTUEachModuleConfigurationModel> infoList = DBHelperSQLite.GetList<DTUEachModuleConfigurationModel>(sb.ToString(), parameters);

            return (infoList == null || infoList.Count <= 0) ? null : infoList[0];
        }

        /// <summary>
        /// 根据节点ID查询节点名称
        /// </summary>
        /// <param name="NodeID"></param>
        /// <returns></returns>
        public DTUNode QueryModuleName(int NodeID)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT NodeName FROM DTUConfigureModuleTree ");
            sb.Append("WHERE NodeID = @NodeID");
            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@NodeID", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = NodeID;

            IList<DTUNode> infoList = DBHelperSQLite.GetList<DTUNode>(sb.ToString(), parameters);

            return (infoList == null || infoList.Count <= 0) ? null : infoList[0];

        }

        /// <summary>
        /// 查询当前配置项记录中，是否有某一节点的配置信息
        /// </summary>
        /// <param name="BelongToModule"></param>
        /// <returns></returns>
        public bool IsExistConfigureRecord(int BelongToModule)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM DTUEachModuleConfiguration ");
            sb.Append("WHERE BelongToModule = @BelongToModule");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@BelongToModule", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = BelongToModule;

            IList<DTUEachModuleConfigurationModel> infoList = DBHelperSQLite.GetList<DTUEachModuleConfigurationModel>(sb.ToString(), parameters);

            return (infoList == null || infoList.Count <= 0) ? false : true;

        }

        /// <summary>
        /// 插入一条新配置模块记录到数据库中
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public int InsertOneConfiguration(DTUEachModuleConfigurationModel configuration)
        {
            StringBuilder sb = new StringBuilder();

            // 插入语句
            sb.Append("INSERT INTO DTUEachModuleConfiguration(BelongToModule,TeleSignalisationNumber,TeleMeteringNumber,TeleControlNumber,SlaveAddress) ");
            sb.Append("VALUES(@BelongToModule,@TeleSignalisationNumber,@TeleMeteringNumber,@TeleControlNumber,@SlaveAddress) ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@BelongToModule", DbType.Int32),
                                     new SQLiteParameter("@TeleSignalisationNumber", DbType.Int32),
                                     new SQLiteParameter("@TeleMeteringNumber", DbType.Int32),
                                     new SQLiteParameter("@TeleControlNumber", DbType.Int32),
                                     new SQLiteParameter("@SlaveAddress", DbType.Int32)
                                 };

            //给参数赋值
            parameters[0].Value = configuration.BelongToModule;
            parameters[1].Value = configuration.TeleSignalisationNumber;
            parameters[2].Value = configuration.TeleMeteringNumber;
            parameters[3].Value = configuration.TeleControlNumber;
            parameters[4].Value = configuration.SlaveAddress;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条新配置模块记录到数据库中
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public int UpdateOneConfiguration(DTUEachModuleConfigurationModel configuration)
        {
            StringBuilder sb = new StringBuilder();
            //更新语句
            sb.Append("UPDATE DTUEachModuleConfiguration SET TeleSignalisationNumber = @TeleSignalisationNumber, TeleMeteringNumber = @TeleMeteringNumber, TeleControlNumber = @TeleControlNumber, SlaveAddress = @SlaveAddress WHERE BelongToModule = @BelongToModule");
            //设置参数
            SQLiteParameter[] parameters = {
       
                                     new SQLiteParameter("@TeleSignalisationNumber", DbType.Int32),
                                     new SQLiteParameter("@TeleMeteringNumber", DbType.Int32),
                                     new SQLiteParameter("@TeleControlNumber", DbType.Int32),
                                     new SQLiteParameter("@SlaveAddress", DbType.Int32),
                                     new SQLiteParameter("@BelongToModule",DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = configuration.TeleSignalisationNumber;
            parameters[1].Value = configuration.TeleMeteringNumber;
            parameters[2].Value = configuration.TeleControlNumber;
            parameters[3].Value = configuration.SlaveAddress;
            parameters[4].Value = configuration.BelongToModule;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

        /// <summary>
        /// 根据nodeID删除模板配置表中的某一条记录
        /// </summary>
        /// <returns></returns>
        public int DeleteOneConfigure(int nodeID)
        {
            StringBuilder sb = new StringBuilder();
            // 删除语句
            sb.Append("DELETE FROM DTUEachModuleConfiguration WHERE BelongToModule = @BelongToModule");

            // 设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@BelongToModule", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = nodeID;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }
    }
}
