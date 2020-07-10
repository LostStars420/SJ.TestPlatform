using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using System.Windows;

namespace FTU.Monitor.Util
{
    /// <summary>
    /// ReportUtil 的摘要说明
    /// author: songminghao
    /// date：2017/12/1 16:34:12
    /// desc：报表操作工具类，包含一些常用类型的报表导入导出功能
    /// version: 1.0
    /// </summary>
    public class ReportUtil
    {
        /// <summary>
        /// 导出到Excel表操作(单个sheet)
        /// </summary>
        /// <param name="name">默认Excel文件名称</param>
        /// <param name="titleList">sheet列标题</param>
        /// <param name="dt">sheet中DataTable数据表对象</param>
        /// <returns></returns>
        public static string ExportExcel(string name, IList<string> titleList, System.Data.DataTable dt)
        {
            try
            {
                string primaryname = name + DateTime.Now.ToString("yyyyMMddHHmmss");
                // 定义一个通用对话框,用户可以使用此对话框来指定一个要将文件另存为的文件名
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                // 设置筛选器字符串
                saveFileDialog.Filter = "Excel (*.XLSX)|*.xlsx";
                // 是否自动添加扩展名
                saveFileDialog.AddExtension = true;
                // 文件已存在是否提示覆盖
                saveFileDialog.OverwritePrompt = true;
                // 提示输入的文件名无效
                saveFileDialog.CheckPathExists = true;
                // 文件初始名
                saveFileDialog.FileName = primaryname;

                if (saveFileDialog.ShowDialog() == true)
                {
                    Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

                    // 创建工作簿(WorkBook:即Excel文件主体本身)
                    Workbook excelWB = excelApp.Workbooks.Add(System.Type.Missing);
                    // 创建工作表(即Excel里的子表sheet) 1表示在子表sheet1里进行数据导出
                    Worksheet excelWS = (Worksheet)excelWB.Worksheets[1];
                    excelWS.Name = name;

                    // 如果数据中存在数字类型,可以让它变文本格式显示
                    excelWS.Cells.NumberFormat = "@";

                    // 给工作表添加列标题
                    for (int i = 0; i < titleList.Count; i++)
                    {
                        excelWS.Cells[1, i + 1] = titleList[i];
                    }

                    // 将数据导入到工作表的单元格
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            // Excel单元格第一个从索引1开始
                            excelWS.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                        }
                    }

                    // 将其进行保存到指定的路径
                    excelWB.SaveAs(saveFileDialog.FileName);
                    excelWB.Close();
                    // 释放可能还没释放的进程
                    // KillAllExcel(excelApp);
                    excelApp.Quit();

                    return "SUCCESS";
                }

                return "ERROR";
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// 导出到Excel表操作(多个sheet)
        /// </summary>
        /// <param name="name">默认Excel文件名称</param>
        /// <param name="sheetNames">sheet名称集合</param>
        /// <param name="titlesList">多个sheet列标题的集合(与sheetNames顺序对应)</param>
        /// <param name="dtList">多个sheet数据的集合(与sheetNames顺序对应)</param>
        /// <returns></returns>
        public static string ExportExcel(string name, IList<string> sheetNames, IList<IList<string>> titlesList, IList<System.Data.DataTable> dtList)
        {
            try
            {
                string primaryname = name + DateTime.Now.ToString("yyyyMMddHHmmss");
                // 定义一个通用对话框,用户可以使用此对话框来指定一个要将文件另存为的文件名
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                // 设置筛选器字符串
                saveFileDialog.Filter = "Excel (*.XLSX)|*.xlsx";
                // 是否自动添加扩展名
                saveFileDialog.AddExtension = true;
                // 文件已存在是否提示覆盖
                saveFileDialog.OverwritePrompt = true;
                // 提示输入的文件名无效
                saveFileDialog.CheckPathExists = true;
                // 文件初始名
                saveFileDialog.FileName = primaryname;

                if (saveFileDialog.ShowDialog() == true)
                {
                    Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

                    // 创建工作簿(WorkBook:即Excel文件主体本身)
                    Workbook excelWB = excelApp.Workbooks.Add(System.Type.Missing);
                    Sheets sheets = excelWB.Worksheets;
                    for (int i = 0; i < dtList.Count - 1; i++)
                    {
                        sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    }

                    int t = sheets.Count;

                    for (int k = 0; k < dtList.Count; k++)
                    {
                        // 创建工作表(即Excel里的子表sheet) 1表示在子表sheet1里进行数据导出
                        Worksheet excelWS = (Worksheet)sheets.get_Item(k + 1);
                        excelWS.Name = sheetNames[k];

                        // 如果数据中存在数字类型,可以让它变文本格式显示
                        excelWS.Cells.NumberFormat = "@";

                        // 给工作表添加列标题
                        // 获取当前sheet的标题
                        IList<string> titleList = titlesList[k];
                        for (int i = 0; i < titleList.Count; i++)
                        {
                            excelWS.Cells[1, i + 1] = titleList[i];
                        }

                        // 将数据导入到工作表的单元格
                        System.Data.DataTable dt = dtList[k];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                // Excel单元格第一个从索引1开始
                                excelWS.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                            }
                        }

                    }

                    // 将其进行保存到指定的路径
                    excelWB.SaveAs(saveFileDialog.FileName);
                    excelWB.Close();
                    // 释放可能还没释放的进程
                    // KillAllExcel(excelApp);
                    excelApp.Quit();
                    //对导出的点表加密
                    //EncryptDecryptForFileUtil.Encrypt(saveFileDialog.FileName, saveFileDialog.FileName);

                    return "SUCCESS";
                }

                return "ERROR";
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// 从Excel文件中获取指定sheet工作簿的数据
        /// </summary>
        /// <param name="sqlExcel">查询Excel文件中要导入指定sheet名里的数据的sql语句</param>
        /// <returns></returns>
        public static object GetExcelSheetData(string sqlExcel)
        {
            try
            {
                // 定义一个通用对话框,用户可以使用此对话框来指定一个或多个要打开的文件的文件名
                OpenFileDialog openFileDialog = new OpenFileDialog();
                // 获取或设置筛选器字符串
                openFileDialog.Filter = "Excel (*.XLSX)|*.xlsx";

                if ((bool)(openFileDialog.ShowDialog()))
                {
                    // 定义字符串,其中包含在文件对话框中选定的文件的完整路径
                    string excelPath = openFileDialog.FileName;

                    // 这种连接写法不需要创建一个数据源DSN，DRIVERID表示驱动ID，Excel2003后都使用790，FIL表示Excel文件类型，
                    // Excel2007用excel 8.0，MaxBufferSize表示缓存大小，DBQ表示读取Excel的文件名（全路径）
                    //Driver={Driver do Microsoft Excel(*.xls)}

                    // 连接语句，读取文件路径
                    string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelPath + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
                    // 创建连接
                    using (OleDbConnection ole = new OleDbConnection(strConn))
                    {
                        // 要对数据源执行的 SQL 语句或存储过程对象
                        OleDbCommand oleDbCommand = new System.Data.OleDb.OleDbCommand();
                        // 设置连接对象
                        oleDbCommand.Connection = ole;
                        // 设置一个指示如何解释 System.Data.OleDb.OleDbCommand.CommandText 属性的值
                        oleDbCommand.CommandType = CommandType.Text;
                        // 设置要对数据源执行的 SQL 语句或存储过程
                        oleDbCommand.CommandText = sqlExcel;

                        //打开连接
                        ole.Open();

                        using (OleDbDataAdapter odp = new OleDbDataAdapter(oleDbCommand))
                        {
                            // 创建DataTable数据表对象，用来接收要导入的相应的数据
                            System.Data.DataTable dataTable = new System.Data.DataTable();
                            try
                            {
                                // 填充 System.Data.DataSet数据源
                                odp.Fill(dataTable);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message);
                            }

                            // 将DataTable数据表对象返回
                            return dataTable;
                        }
                    }

                }

                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

        }

        /// <summary>
        /// 从Excel文件中获取所有sheet工作簿的数据
        /// </summary>
        /// <param name="pointTypeNameList">sheet对应的点号类型集合</param>
        /// <returns></returns>
        public static object GetExcelSheetData(IList<string> pointTypeNameList)
        {
            try
            {
                // 定义一个通用对话框,用户可以使用此对话框来指定一个或多个要打开的文件的文件名
                OpenFileDialog openFileDialog = new OpenFileDialog();
                // 获取或设置筛选器字符串
                openFileDialog.Filter = "Excel (*.XLSX)|*.xlsx";

                if ((bool)(openFileDialog.ShowDialog()))
                {
                    // 定义字符串,其中包含在文件对话框中选定的文件的完整路径
                    string excelPath = openFileDialog.FileName;
                    // 读取Excel文件中所有的sheet名称
                    IList<string> sheetNames = GetSheetNames(excelPath);
                    if (sheetNames == null || sheetNames.Count == 0)
                    {
                        return "";
                    }

                    string noPointTypeName = "";
                    foreach (string sheetName in sheetNames)
                    {
                        if (pointTypeNameList.IndexOf(sheetName) == -1)
                        {
                            noPointTypeName += sheetName + ",";
                        }
                    }

                    if (!"".Equals(noPointTypeName))
                    {
                        return "点号类型" + noPointTypeName + "系统没有配置";
                    }

                    //连接语句，读取文件路径
                    string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelPath + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
                    // 创建连接
                    using (OleDbConnection ole = new OleDbConnection(strConn))
                    {
                        //打开连接
                        ole.Open();

                        // 定义IDictionary类型返回对象
                        IDictionary<string, System.Data.DataTable> retDic = new Dictionary<string, System.Data.DataTable>();
                        // 循环获取各sheet里的数据
                        foreach (string sheetName in sheetNames)
                        {
                            // 从sheet工作簿中获取数据的查询语句
                            string strExcel = "SELECT * FROM [" + sheetName.Replace('.', '#') + "$] WHERE [点号] is not null AND [名称] is not null";

                            // 创建DataTable数据表对象，用来接收要导入的相应的数据
                            System.Data.DataTable dataTable = new System.Data.DataTable();
                            // 表示一组数据命令和一个数据库连接，用于填充 System.Data.DataSet 和更新数据源
                            OleDbDataAdapter odp = new OleDbDataAdapter(strExcel, strConn);
                            // 填充 System.Data.DataSet数据源
                            odp.Fill(dataTable);

                            // IDictionary类型返回对象添加一个键值对
                            retDic.Add(sheetName, dataTable);
                        }

                        // 将Dictionary对象返回
                        return retDic;
                    }
                    
                }

                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

        }

        /// <summary>
        /// 读取Excel文件中所有的sheet名称
        /// </summary>
        /// <param name="excelPath">Excel文件路径</param>
        /// <returns></returns>
        public static IList<string> GetSheetNames(string excelPath)
        {
            //Driver={Driver do Microsoft Excel(*.xls)} 这种连接写法不需要创建一个数据源DSN，DRIVERID表示驱动ID，Excel2003后都使用790，FIL表示Excel文件类型，Excel2007用excel 8.0，MaxBufferSize表示缓存大小，DBQ表示读取Excel的文件名（全路径）
            //连接语句，读取文件路径
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelPath + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";

            OleDbConnection ole = new OleDbConnection(strConn);
            // 打开连接
            ole.Open();
            /* 下面这种方式可以获取Excel表所有的sheet工作簿
             System.Data.DataTable schemaTable = ole.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
             string tableName = schemaTable.Rows[0][2].ToString().Trim();
            */
            // 获取Excel表所有的sheet工作簿
            System.Data.DataTable sheetNames = ole.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            ole.Close();

            IList<string> names = new List<string>();
            // 获取Excel表所有的sheet工作簿名称
            foreach (DataRow dr in sheetNames.Rows)
            {
                string name = dr[2].ToString();
                names.Add(name.Substring(0, name.Length - 1));
            }

            return names;
        }

        /// <summary>
        /// 返回提示信息
        /// </summary>
        /// <param name="status">状态字符串</param>
        /// <returns>提示信息</returns>
        public static string GetPromptMessage(string status)
        {
            if (status.Equals("SUCCESS"))
            {
                return "导出成功";
            }
            else if (status.Equals("ERROR"))
            {
                return "导出失败";
            }

            return status;
        }

        /// <summary>
        /// 获取文件内容解密后的字符串
        /// </summary>
        /// <returns>Json格式的明文</returns>
        public static string GetParameterDataCiphertext()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                // 获取或设置筛选器字符串
                openFileDialog.Filter = "config files (*.cfg)|*.cfg|data files (*.txt)|*.txt|All files (*.*)|*.*";

                if ((bool)(openFileDialog.ShowDialog()))
                {
                    // 打开一个文件，使用指定的编码读取文件的所有行，然后关闭该文件
                    string encryptStr = System.IO.File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                    // 解密加密的字符串
                    return EncryptAndDecodeUtil.AESDecrypt(encryptStr, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;

        }

        /// <summary>
        /// 导出密文
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="encrypt">密文</param>
        /// <returns>成功或失败信息</returns>
        public static string ExportParameterData(string fileName, string encrypt)
        {
            // 提示信息
            string promptMsg = "";
            try
            {
                string primaryName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".cfg";
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Config (*.cfg)|*.cfg";

                //是否自动添加扩展名
                saveFileDialog.AddExtension = true;
                //文件已存在是否提示覆盖
                saveFileDialog.OverwritePrompt = true;
                //提示输入的文件名无效
                saveFileDialog.CheckPathExists = true;
                // 文件初始名
                saveFileDialog.FileName = primaryName;

                if (saveFileDialog.ShowDialog() == true)
                {
                    System.IO.File.WriteAllBytes(saveFileDialog.FileName, Encoding.UTF8.GetBytes(encrypt));
                    promptMsg = "点表导出操作成功";
                    return promptMsg;
                }
                return null;

            }
            catch (Exception ex)
            {
                promptMsg = ex.ToString();
                return promptMsg;
            }
        }

    }
}
