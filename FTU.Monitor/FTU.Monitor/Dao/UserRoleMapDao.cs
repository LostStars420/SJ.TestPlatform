using FTU.Monitor.Model.ManageUsersModelCollectiopn;
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
    /// UserRoleMapDao 的摘要说明
    /// author: liyan
    /// date：2018/10/17 11:19:47
    /// desc：用户管理系统所有信息的数据库访问对象
    /// version: 1.0
    /// </summary>
    public class UserRoleMapDao
    {
        /// <summary>
        /// 查询所有用户信息
        /// </summary>
        /// <returns>查询的结果</returns>
        public IList<UserRoleMapModel> QueryAllUser()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT urm.id, ui.userName, ri.roleName, ui.password, ui.telephoneNumber, ui.remark FROM user_role_map as urm left join user_information as ui on ui.id = urm.userID left join role_information as ri on ri.id = urm.roleID");
            return DBHelperSQLite.GetList<UserRoleMapModel>(sb.ToString(), null);
        }

        /// <summary>
        /// 插入一条用户信息记录
        /// </summary>
        /// <param name="user">插入的用户记录</param>
        /// <returns>插入记录的关键字id</returns>
        public int InsertOneUserInfo(UserRoleMapModel user)
        {
            // 插入user_information表中信息
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO user_information(userName, password, telephoneNumber, remark) ");
            sb.Append("VALUES( @userName, @password, @telephoneNumber, @remark)");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@userName", DbType.String),
                                     new SQLiteParameter("@password", DbType.String),
                                     new SQLiteParameter("@telephoneNumber", DbType.String),
                                     new SQLiteParameter("@remark", DbType.String)
                                 };

            //给参数赋值
            parameters[0].Value = user.UserName;
            parameters[1].Value = user.Password;
            parameters[2].Value = user.TelephoneNumber;
            parameters[3].Value = user.Remark;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

        /// <summary>
        /// 查询user_information中对应角色的id
        /// </summary>
        /// <param name="user">用户记录</param>
        /// <returns>用户在user_information中对应的关键字id</returns>
        public object QueryByUserName(UserRoleMapModel user)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT id FROM user_information ");
            sb.Append("WHERE userName = @userName AND password = @password AND telephoneNumber = @telephoneNumber AND remark = @remark");
            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@userName", DbType.String),
                                     new SQLiteParameter("@password", DbType.String),
                                     new SQLiteParameter("@telephoneNumber", DbType.String),
                                     new SQLiteParameter("@remark", DbType.String)
                                 };
            //给参数赋值
            parameters[0].Value = user.UserName;
            parameters[1].Value = user.Password;
            parameters[2].Value = user.TelephoneNumber;
            parameters[3].Value = user.Remark;

            return DBHelperSQLite.GetSingle(sb.ToString(), parameters);
        }

        /// <summary>
        /// 查询role_information中对应角色的id
        /// </summary>
        /// <param name="user">当前用户记录</param>
        /// <returns>角色对应的id</returns>
        public object QueryByRoleName(UserRoleMapModel user)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT id FROM role_information ");
            sb.Append("WHERE roleName = @roleName");
            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@roleName", DbType.String)
                                 };
            //给参数赋值
            parameters[0].Value = user.RoleName;

            return DBHelperSQLite.GetSingle(sb.ToString(), parameters);
        }

        /// <summary>
        /// 从user_role_map中查询被修改记录的id
        /// </summary>
        /// <param name="userID">user_role_map中的userID</param>
        /// <param name="roleID">user_role_map中的roleID</param>
        /// <returns>被选中记录user_role_map中的id</returns>
        public object QueryByRoleIDUserID(int userID, int roleID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT id FROM user_role_map ");
            sb.Append("WHERE userID  = @userID AND roleID = @roleID");
            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@userID", DbType.Int32),
                                     new SQLiteParameter("@roleID",DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = userID;
            parameters[1].Value = roleID;

            return DBHelperSQLite.GetSingle(sb.ToString(), parameters);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public object QueryByUserID(int userID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM user_role_map ");
            sb.Append("WHERE userID = @userID");

            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@userID", DbType.Int32)
                                 };

            //给参数赋值
            parameters[0].Value = userID;

            object result = DBHelperSQLite.GetSingle(sb.ToString(), parameters);
            if (result == null)
            {
                return -1;
            }

            return Convert.ToInt32(result.ToString());
        }

        /// <summary>
        /// 向user_role_map中插入一条记录
        /// </summary>
        /// <param name="userID">用户信息关键字</param>
        /// <param name="roleID">角色名称关键字</param>
        /// <returns>受影响的行</returns>
        public int InsertToUserRoleMap(int userID, int roleID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO user_role_map(userID, roleID) ");
            sb.Append("VALUES(@userID, @roleID)");
            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@userID", DbType.Int32),
                                     new SQLiteParameter("@roleID", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = userID;
            parameters[1].Value = roleID;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

        /// <summary>
        /// 修改一条用户记录
        /// </summary>
        /// <param name="user">被修改的用户记录</param>
        /// <param name="userInfoID">被修改的用户记录在user_information中的关键字</param>
        /// <returns>受影响的行</returns>
        public int UpdateOneUserInfoToUserInfo(UserRoleMapModel user, int userInfoID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE user_information SET userName = @userName,password = @password,telephoneNumber = @telephoneNumber,remark = @remark ");
            sb.Append("WHERE id = @id");
            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@userName", DbType.String),
                                     new SQLiteParameter("@password", DbType.String),
                                     new SQLiteParameter("@telephoneNumber", DbType.String),
                                     new SQLiteParameter("@remark", DbType.String),
                                     new SQLiteParameter("@id", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = user.UserName;
            parameters[1].Value = user.Password;
            parameters[2].Value = user.TelephoneNumber;
            parameters[3].Value = user.Remark;
            parameters[4].Value = userInfoID;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

        /// <summary>
        /// 更新user_role_map中的角色关键字
        /// </summary>
        /// <param name="roleID">角色关键字</param>
        /// <returns>受影响的行</returns>
        public int UpdateOneUserInfoToUserRoleMap(int roleID, int userRoleMapID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE user_role_map SET roleID = @roleID ");
            sb.Append("WHERE id = @id");
            //设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@roleID", DbType.Int32),
                                     new SQLiteParameter("@id",DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = roleID;
            parameters[1].Value = userRoleMapID;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条用户记录
        /// </summary>
        /// <param name="user">被删除的用户对象</param>
        /// <returns>受影响的行</returns>
        public int DeleteFromUserRoleMap(int userID, int roleID)
        {
            StringBuilder sb = new StringBuilder();
            // 删除语句
            sb.Append("DELETE FROM user_role_map WHERE userID = @userID AND roleID = @roleID");

            // 设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@userID", DbType.Int32),
                                     new SQLiteParameter("@roleID", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = userID;
            parameters[1].Value = roleID;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

        /// <summary>
        /// 删除user_information中被选中的记录
        /// </summary>
        /// <param name="id">user_information中的id关键字</param>
        /// <returns>受影响的行</returns>
        public int DeleteFromUserInfomation(int id)
        {
            StringBuilder sb = new StringBuilder();
            // 删除语句
            sb.Append("DELETE FROM user_information WHERE id = @id");

            // 设置参数
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@id", DbType.Int32)
                                 };
            //给参数赋值
            parameters[0].Value = id;

            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }
    }
}
