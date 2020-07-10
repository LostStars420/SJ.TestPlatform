using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sojo.TestPlatform.ControlPlatform.Util
{
    public class ExcelHelper
    {

        public ExcelHelper()
        {

        }

        /// <summary>
        /// 保存数据到Excel
        /// </summary>
        public static void GenerateDataToExcel(List<string[]> dataSource)
        {
            //实例化一个Excel.Application对象  
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            //让后台执行设置为不可见，为true的话会看到打开一个Excel，然后数据在往里写  
            excel.Visible = true;

            //新增加一个工作簿，Workbook是直接保存，不会弹出保存对话框，加上Application会弹出保存对话框，值为false会报错  
            excel.Application.Workbooks.Add(true);

            //把DataGridView当前页的数据保存在Excel中  
            for (int i = 0; i < dataSource.Count; i++)
            {
              //  System.Windows.Forms.Application.DoEvents();
                for (int j = 0; j < dataSource[i].Length; j++)
                {
                    excel.Cells[i + 1, j + 1] = dataSource[i][j];
                }
            }

            //设置禁止弹出保存和覆盖的询问提示框  
            excel.DisplayAlerts = false;
            excel.AlertBeforeOverwriting = false;

            //保存工作簿  
            excel.Application.Workbooks.Add(true).Save();
            //保存excel文件  
            excel.Save(".\\误差报告.xls");

            //确保Excel进程关闭  
            excel.Quit();
            excel = null;
            GC.Collect();//如果不使用这条语句会导致excel进程无法正常退出，使用后正常退出
        }
    }
}
