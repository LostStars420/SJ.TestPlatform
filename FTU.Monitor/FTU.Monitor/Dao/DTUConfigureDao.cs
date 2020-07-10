using FTU.Monitor.Model;
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
    /// DTUConfigureDao 的摘要说明
    /// author: liyan
    /// date：2018/5/27 11:25:22
    /// desc：
    /// version: 1.0
    /// </summary>
    public class DTUConfigureDao
    {
        /// <summary>
        /// 从数据库中查询所有的DTU配置信息，树结构
        /// </summary>
        /// <returns>每个节点的信息</returns>
        public IList<Node> Query()
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM DTUConfigure ");
            return DBHelperSQLite.GetList<Node>(sb.ToString(), null);
        }

        /// <summary>
        /// 从数据库中查询DTU配置信息初始化树结构
        /// </summary>
        /// <returns>每个节点的信息</returns>
        public IList<Node> QueryDTUConfigureInitTable()
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM DTUConfigure_InitTable ");
            return DBHelperSQLite.GetList<Node>(sb.ToString(), null);
        }

        /// <summary>
        /// 从数据库中查询DTU配置信息初始化三级节点的结构
        /// </summary>
        /// <returns>每个节点的信息</returns>
        public IList<Node> QueryDTUConfigureInitThirdLevel()
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM DTUConfigure_InitThirdLevel ");
            return DBHelperSQLite.GetList<Node>(sb.ToString(), null);
        }

        /// <summary>
        /// 查询某节点所有的子节点
        /// </summary>
        /// <param name="nodePath">当前节点的路径</param>
        /// <returns></returns>
        public IList<Node> QueryNodeChildren(string nodePath)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            nodePath = nodePath + "%";
            sb.Append("SELECT * FROM DTUConfigure WHERE Path LIKE @NodePath ");

            SQLiteParameter[] parameters = {
                                               new SQLiteParameter("@NodePath",DbType.String)
                                           };
            parameters[0].Value = nodePath;
            return DBHelperSQLite.GetList<Node>(sb.ToString(), parameters);
        }

        /// <summary>
        /// 新增一个节点
        /// </summary>
        /// <param name="node">节点对象</param>
        public int InsertNode(Node node)
        {
            StringBuilder sb = new StringBuilder();
            //插入语句
            sb.Append("INSERT INTO DTUConfigure(NodeName, Path, NodeType, NodeIP, NodeBreakersOrTieSwitch, NodeMainOrBranchLine) ");
            sb.Append("VALUES(@NodeName, @Path, @NodeType, @NodeIP, @NodeBreakersOrTieSwitch, @NodeMainOrBranchLine) ");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@NodeName", DbType.String),
                                     new SQLiteParameter("@Path", DbType.String),
                                     new SQLiteParameter("@NodeType", DbType.Int32),
                                     new SQLiteParameter("@NodeIP", DbType.String),
                                     new SQLiteParameter("@NodeBreakersOrTieSwitch", DbType.Int32),
                                     new SQLiteParameter("@NodeMainOrBranchLine", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = node.NodeName;
            parameters[1].Value = node.Path;
            parameters[2].Value = node.NodeType;
            parameters[3].Value = node.NodeIP;
            parameters[4].Value = node.NodeBreakersOrTieSwitch;
            parameters[5].Value = node.NodeMainOrBranchLine;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

        /// <summary>
        /// 查询数据库中记录的总数
        /// </summary>
        /// <returns></returns>
        public int QueryMaxNodeID()
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT max(NodeID) FROM DTUConfigure ");

            object obj = DBHelperSQLite.GetSingle(sb.ToString(), null);
            if (obj == null)
            {
                return -1;
            }

            return Convert.ToInt32(obj.ToString());
        }

        /// <summary>
        /// 更新节点配置信息
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int UpdateNodeConfigure(Node node)
        {
            StringBuilder sb = new StringBuilder();
            //更新语句
            sb.Append("UPDATE DTUConfigure SET NodeName = @NodeName, NodeIP = @NodeIP, NodeBreakersOrTieSwitch = @NodeBreakersOrTieSwitch, NodeMainOrBranchLine = @NodeMainOrBranchLine WHERE NodeID = @NodeID");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@NodeName", DbType.String),
                                     new SQLiteParameter("@NodeIP", DbType.String),
                                     new SQLiteParameter("@NodeBreakersOrTieSwitch", DbType.Int32),
                                     new SQLiteParameter("@NodeMainOrBranchLine", DbType.Int32),
                                     new SQLiteParameter("@NodeID",DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = node.NodeName;
            parameters[1].Value = node.NodeIP;
            parameters[2].Value = node.NodeBreakersOrTieSwitch;
            parameters[3].Value = node.NodeMainOrBranchLine;
            parameters[4].Value = node.NodeID;

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
            sb.Append("DELETE FROM DTUConfigure WHERE NodeID = @NodeID");

            // 设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@NodeID", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = nodeID;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters); 
        }
           
        /// <summary>
        /// 根据节点的ID号查询
        /// </summary>
        /// <param name="NodeType">节点的NodeType字段</param>
        /// <returns>指定节点的集合</returns>
        public IList<Node> QueryByNodeType(int NodeType)
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM DTUConfigure ");
            sb.Append("WHERE NodeType = @NodeType");
            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@NodeType", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = NodeType;

            IList<Node> nodes = DBHelperSQLite.GetList<Node>(sb.ToString(), parameters);

            return (nodes == null || nodes.Count <= 0) ? null : nodes;  
        }


    }
}
