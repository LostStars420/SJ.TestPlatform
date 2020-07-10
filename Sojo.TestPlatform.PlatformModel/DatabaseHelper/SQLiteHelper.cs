using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;


namespace Sojo.TestPlatform.PlatformModel.DatabaseHelper
{
    public class SQLiteHelper
    {

        private static SQLiteConnection conn;

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="dbPath">数据源aram>
        public static void Connect(string dbPath)
        {
            try
            {
                conn = null;
                //创建数据库实例，指定文件位置
                conn = new SQLiteConnection("Data Source =" + dbPath);
                //打开数据库，若文件不存在会自动创建  
                conn.Open();
            }
            catch(Exception e)
            {
                throw e;
            }
            
        }

        /// <summary>
        /// 读取列表
        /// </summary>
        /// <returns>返回数据表</returns>
        public static DataTable ReadTable(string tableName)
        {
            SQLiteDataAdapter mAdapter = new SQLiteDataAdapter("select * from " + tableName, conn);
            DataTable dt = new DataTable();
            mAdapter.Fill(dt);
            return dt;
        }


        /// <summary>
        /// 插入语句
        /// </summary>
        /// <param name="sqlString">插入语句</param>
        /// <param name="sqlClear">清除语句</param>
        /// <param name="datasource">数据源</param>
        public static void InsertTable(List<string> sqlString, string sqlClear, string datasource)
        {
            try
            {
                using (var conn = new System.Data.SQLite.SQLiteConnection())
                {
                    var connstr = new SQLiteConnectionStringBuilder();
                    connstr.DataSource = datasource;
                    conn.ConnectionString = connstr.ToString();
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = conn;
                        //删除表格中索引的数据
                        if (sqlClear != null)
                        {
                            cmd.CommandText = sqlClear;
                            cmd.ExecuteNonQuery();
                        }
                        foreach (var m in sqlString)
                        {
                            cmd.CommandText = m;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public static void Close()
        {
            conn.Close();
        }
    }
}
