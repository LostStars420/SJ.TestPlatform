using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using FTU.Monitor.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Data;
using FTU.Monitor.DataService;
using FTU.Monitor.Util;
using System.Collections.Generic;


namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// SOEViewModel 的摘要说明
    /// author: liyan
    /// date：2018/4/10 14:47:09
    /// desc：SOEViewModel类
    /// version: 1.0
    /// </summary>
    public class SOEViewModel : ViewModelBase
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public SOEViewModel()
        {
            SOECommand = new RelayCommand<string>(ExecuteSOECommand);
            _SOEData = new ObservableCollection<SOE>();
        }

        /// <summary>
        /// SOE相关命令
        /// </summary>
        public RelayCommand<string> SOECommand
        {
            get;
            private set;
        }

        /// <summary>
        /// SOE相关命令执行方法
        /// </summary>
        /// <param name="arg">参数</param>
        public void ExecuteSOECommand(string arg)
        {
            switch (arg)
            {
                // "清除SOE记录"
                case "Clear":
                    //SOEView.RollEnable = true;
                    TelesignalisationViewModel.soeCounter = 0;
                    SOEData.Clear();
                    break;

                // “导入SOE记录”
                case "ImportData":
                    //SOEView.RollEnable = true;
                    // 查询Excel文件中要导入指定sheet名里的数据的sql语句
                    string sqlExcel = string.Format("select * from [SOE$]");
                    // 从选择的Excel文件中提取数据，存到DataTable表对象中
                    object obj = ReportUtil.GetExcelSheetData(sqlExcel);
                    // 判断返回值类型
                    if (obj.GetType() == typeof(string))
                    {
                        if (obj == null || obj.ToString().Trim().Length == 0)
                        {
                            break;
                        }

                        MessageBox.Show(obj.ToString(), "提示");
                        break;
                    }

                    // 将返回对象转换为System.Data.DataTable类型
                    System.Data.DataTable dataTable = (System.Data.DataTable)obj;
                    if (dataTable.Rows.Count > 0)
                    {
                        SOEData.Clear();
                        try
                        {
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                FTU.Monitor.Model.SOE soe = new FTU.Monitor.Model.SOE();
                                soe.Number = Convert.ToInt32(dataTable.Rows[i][0].ToString());
                                soe.ID = dataTable.Rows[i][1].ToString();
                                soe.Time = dataTable.Rows[i][2].ToString();
                                soe.Content = dataTable.Rows[i][3].ToString();
                                soe.Value = Convert.ToByte(dataTable.Rows[i][4].ToString());
                                soe.Comment = dataTable.Rows[i][4].ToString();
                                SOEData.Add(soe);
                            }

                            MessageBox.Show("导入成功", "提示");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "异常");
                        }

                    }

                    break;

                //“导出SOE记录”
                case "ExportData":
                    // 获取将要导出的数据,将这些数据都存在DataTable中  
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Columns.Add("Number", typeof(int));
                    dt.Columns.Add("SOEID", typeof(string));
                    dt.Columns.Add("Time", typeof(string));
                    dt.Columns.Add("SOEResoult", typeof(string));
                    dt.Columns.Add("SOEValue", typeof(int));
                    dt.Columns.Add("SOERemark", typeof(string));

                    DataRow row;
                    // 创建Excel  
                    for (int i = 0; i < SOEData.Count; i++)
                    {
                        row = dt.NewRow();
                        row["Number"] = SOEData[i].Number;
                        row["SOEID"] = SOEData[i].ID;
                        row["Time"] = SOEData[i].Time;
                        row["SOEResoult"] = SOEData[i].Content;
                        row["SOEValue"] = SOEData[i].Value;
                        row["SOERemark"] = SOEData[i].Comment;
                        dt.Rows.Add(row);
                    }

                    // 设置导出的Excel文件中sheet的列标题
                    IList<string> titleList = new List<string>();
                    titleList.Add("序号");
                    titleList.Add("点号");
                    titleList.Add("时间");
                    titleList.Add("内容");
                    titleList.Add("值");
                    titleList.Add("备注");

                    // 导出到Excel
                    string status = ReportUtil.ExportExcel("SOE", titleList, dt);
                    // 获取提示信息,并显示
                    MessageBox.Show(ReportUtil.GetPromptMessage(status), "提示");
                    
                    break;

            }
        }

        /// <summary>
        /// SOE数据集合
        /// </summary>
        public static ObservableCollection<SOE> _SOEData;

        /// <summary>
        /// 设置和获取SOE数据集合
        /// </summary>
        public ObservableCollection<SOE> SOEData
        {
            get
            {
                return _SOEData;
            }
            set
            {
                _SOEData = value;
                RaisePropertyChanged(() => SOEData);
            }
        }

        #region 表格操作

        /// <summary>
        /// SOE数据表格右键相关操作命令
        /// </summary>
        public RelayCommand<string> DataGridMenumSelected
        {
            get;
            private set;
        }

        /// <summary>
        /// SOE数据表格右键相关操作命令执行的方法
        /// </summary>
        /// <param name="arg">参数</param>
        private void ExecuteDataGridMenumSelected(string arg)
        {
            try
            {
                switch (arg)
                {
                    // 重新载入
                    case "Reload":
                        //UserData = monitorData.ReadTelesignalisation(true);                            
                        break;

                    // 保存
                    case "Save":
                        //monitorData.InsertTelesignalisation();
                        break;

                    // 在选中行上面插入一行
                    case "AddUp":
                        //if (SelectedIndex > -1)
                        //{
                        //    var item = new Telesignalisation(0, "xxx", 0, "否", 0, "xxx", "xxx", "StateA", "StateB");
                        //    UserData.Insert(SelectedIndex, item);
                        //}
                        break;

                    // 在选中行下面插入一行
                    case "AddDown":
                        //if (SelectedIndex > -1)
                        //{
                        //    var item = new Telesignalisation(0, "xxx", 0, "否", 0, "xxx", "xxx", "StateA", "StateB");
                        //    if (SelectedIndex < UserData.Count - 1)
                        //    {

                        //        UserData.Insert(SelectedIndex + 1, item);
                        //    }
                        //    else
                        //    {
                        //        UserData.Add(item);
                        //    }
                        //}
                        break;

                    // 删除选中行
                    case "DeleteSelect":
                        //if (SelectedIndex > -1)
                        //{
                        //    //var result = MessageBox.Show("是否删除选中行:" + gridTelesignalisation.SelectedItem.ToString(),
                        //    //    "确认删除", MessageBoxButton.OKCancel);
                        //    var result = true;
                        //    if (result)
                        //    {
                        //        UserData.RemoveAt(SelectedIndex);
                        //    }
                        //}
                        break;

                }
            }
            catch (Exception ex)
            {
                CommunicationViewModel.con.DebugLog(ex.ToString());
            }
        }

        #endregion 表格操作

    }
}