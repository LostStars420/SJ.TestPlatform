using FTU.Monitor.Model;
using FTU.Monitor.Util;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace FTU.Monitor.Dao
{
    /// <summary>
    /// ProgramVersionDao 的摘要说明
    /// author: songminghao
    /// date：2018/3/30 14:54:52
    /// desc：对SQLite数据库中程序版本号表进行增删查改操作
    /// version: 1.0
    /// </summary>
    public class ProgramVersionDao
    {
        /// <summary>
        /// 查询所有的程序版本号列表
        /// </summary>
        /// <returns>设备点号列表</returns>
        public IList<ProgramVersionModel> query()
        {
            StringBuilder sb = new StringBuilder();
            //查询语句
            sb.Append("SELECT * FROM program_version ");

            return DBHelperSQLite.GetList<ProgramVersionModel>(sb.ToString(), null);
        }

        /// <summary>
        /// 更新软件版本
        /// </summary>
        /// <param name="newVersion">新版本号</param>
        /// <param name="versionId">版本号编号</param>
        /// <returns>数据库受影响的行数</returns>
        public int UpdateProgramVersion(string newVersion, int versionId)
        {
            StringBuilder sb = new StringBuilder();
            //更新语句
            sb.Append("UPDATE program_version SET version = @version WHERE id = @id");
            SQLiteParameter[] parameters = {
                                     new SQLiteParameter("@version", DbType.String),
                                     new SQLiteParameter("@id", DbType.Int32)
                                 };
            parameters[0].Value = newVersion;
            parameters[1].Value = versionId;
            return DBHelperSQLite.ExecuteSql(sb.ToString(), parameters);
        }

    }
}
