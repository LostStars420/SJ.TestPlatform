using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using FTU.Monitor.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// HistoryRecordViewModel 的摘要说明
    /// author: songminghao
    /// date：2017/12/22 19:32:40
    /// desc：历史文件记录ViewModel,用来显示SOE历史记录,CO操作历史记录以及故障事件历史记录
    /// version: 1.0
    /// </summary>
    public class HistoryRecordViewModel : ViewModelBase
    {
        /// <summary>
        /// 历史文件信息
        /// </summary>
        private string _fileInfo;

        /// <summary>
        /// 设置和获取历史文件信息
        /// </summary>
        public string FileInfo
        {
            get
            {
                return this._fileInfo;
            }
            set
            {
                this._fileInfo = value;
                RaisePropertyChanged(() => FileInfo);
            }
        }

        #region SOE历史记录文件处理

        /// <summary>
        /// SOE历史记录文件基本信息
        /// </summary>
        private HistoryRecordInfo _SOEHistoryRecordFileInfo;

        /// <summary>
        /// 设置和获取SOE历史记录文件基本信息
        /// </summary>
        public HistoryRecordInfo SOEHistoryRecordFileInfo
        {
            get
            {
                return this._SOEHistoryRecordFileInfo;
            }
            set
            {
                this._SOEHistoryRecordFileInfo = value;
                RaisePropertyChanged(() => SOEHistoryRecordFileInfo);
            }
        }

        /// <summary>
        /// SOE历史记录列表
        /// </summary>
        private ObservableCollection<HistoryRecordBase> _SOEHistoryRecordList;

        /// <summary>
        /// 设置和获取SOE历史记录列表
        /// </summary>
        public ObservableCollection<HistoryRecordBase> SOEHistoryRecordList
        {
            get
            {
                return this._SOEHistoryRecordList;
            }
            set
            {
                this._SOEHistoryRecordList = value;
                RaisePropertyChanged(() => SOEHistoryRecordList);
            }
        }

        #endregion SOE历史记录文件处理

        #region CO操作历史记录文件处理

        /// <summary>
        /// CO操作历史记录文件基本信息
        /// </summary>
        private HistoryRecordInfo _COHistoryRecordFileInfo;

        /// <summary>
        /// 设置和获取CO操作历史记录文件基本信息
        /// </summary>
        public HistoryRecordInfo COHistoryRecordFileInfo
        {
            get
            {
                return this._COHistoryRecordFileInfo;
            }
            set
            {
                this._COHistoryRecordFileInfo = value;
                RaisePropertyChanged(() => COHistoryRecordFileInfo);
            }
        }

        /// <summary>
        /// CO操作历史记录列表
        /// </summary>
        private ObservableCollection<COHistoryRecord> _COHistoryRecordList;

        /// <summary>
        /// 设置和获取CO操作历史记录列表
        /// </summary>
        public ObservableCollection<COHistoryRecord> COHistoryRecordList
        {
            get
            {
                return this._COHistoryRecordList;
            }
            set
            {
                this._COHistoryRecordList = value;
                RaisePropertyChanged(() => COHistoryRecordList);
            }
        }

        #endregion CO操作历史记录文件处理

        #region 故障事件历史记录文件处理

        /// <summary>
        /// 故障事件历史记录文件基本信息
        /// </summary>
        private HistoryRecordInfo _faultEventHistoryRecordFileInfo;

        /// <summary>
        /// 设置和获取故障事件历史记录文件基本信息
        /// </summary>
        public HistoryRecordInfo FaultEventHistoryRecordFileInfo
        {
            get
            {
                return this._faultEventHistoryRecordFileInfo;
            }
            set
            {
                this._faultEventHistoryRecordFileInfo = value;
                RaisePropertyChanged(() => FaultEventHistoryRecordFileInfo);
            }
        }

        /// <summary>
        /// 故障事件历史记录列表
        /// </summary>
        private ObservableCollection<FaultEventHistoryRecord> _faultEventHistoryRecordList;

        /// <summary>
        /// 设置和获取故障事件历史记录列表
        /// </summary>
        public ObservableCollection<FaultEventHistoryRecord> FaultEventHistoryRecordList
        {
            get
            {
                return this._faultEventHistoryRecordList;
            }
            set
            {
                this._faultEventHistoryRecordList = value;
                RaisePropertyChanged(() => FaultEventHistoryRecordList);
            }
        }

        #endregion 故障事件历史记录文件处理

        #region 打开历史文件指令

        /// <summary>
        /// 打开历史文件指令(SOE,CO,故障事件等历史记录文件)
        /// </summary>
        public RelayCommand HistoryRecordCommand 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// 打开历史文件指令(SOE,CO,故障事件等历史记录文件)执行操作
        /// </summary>
        private void ExecuteHistoryRecordCommand()
        {
            try
            {
                // 创建一个通用对话框对象，用户可以使用此对话框来指定一个或多个要打开的文件的文件名
                OpenFileDialog openFileDialog = new OpenFileDialog();

                // 获取或设置筛选器字符串
                // (*.jpg,*.png,*.jpeg,*.bmp,*.gif)|*.jgp;*.png;*.jpeg;*.bmp;*.gif|All files(*.*)|*.*
                // ‘|’分割的两个，一个是注释，一个是真的Filter，显示出来的是那个注释。如果要一次显示多中类型的文件，用分号分开
                openFileDialog.Filter = "historyRecord files (*.xml)|*.xml";

                //获取项目启动路径
                string startPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                //设置默认的打开路径
                openFileDialog.InitialDirectory = startPath + @"\Comtrade\";

                if ((bool)(openFileDialog.ShowDialog()))
                {
                    string filePath = openFileDialog.FileName;
                    LoadXml(filePath);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        #endregion 打开历史文件指令

        /// <summary>
        /// 加载历史文件记录
        /// </summary>
        /// <param name="filePath">历史文件路径</param>
        public void LoadXml(string filePath)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filePath);

            XmlNodeList header = document.GetElementsByTagName("Header");
            XmlElement item = (XmlElement)header[0];
            // 文件类型
            string fileType = item.GetAttribute("fileType").Trim();
            // 文件版本
            string fileVersion = item.GetAttribute("fileVer").Trim();
            // 终端名称
            string devName = item.GetAttribute("devName").Trim();

            XmlNodeList dataRec = document.GetElementsByTagName("DataRec");
            XmlElement dataRecElement = (XmlElement)dataRec[0];
            // 记录个数
            int recordCounts = Convert.ToInt32(dataRecElement.GetAttribute("num").Trim());

            // 设置历史文件信息
            FileInfo = "文件类型:" + fileType + "\n" + "文件版本:" + fileVersion + "\n" + "终端名称:" + devName + "\n" + "记录个数:" + recordCounts.ToString() + "\n";

            XmlNodeList listDI = document.GetElementsByTagName("DI");

            if (UtilHelper.IsEmpty(listDI))
            {
                MessageBox.Show("记录内容为空", "提示");
                return;
            }

            // 判断加载的文件类型
            switch (fileType.ToUpper())
            {
                // SOE文件
                case "SOE":
                    // 初始化SOE历史记录文件基本信息
                    SOEHistoryRecordFileInfo = new HistoryRecordInfo(fileType, fileVersion, devName, recordCounts);

                    // 获取使用的所有遥信点表
                    PointUsedDao pointUsedDao = new PointUsedDao();
                    IList<int> IDList = new List<int>();
                    IList<PointUsed> telesignalisationPointList = pointUsedDao.queryByPointTypeId(ConfigUtil.getPointTypeID("遥信"));

                    if (telesignalisationPointList != null && telesignalisationPointList.Count > 0)
                    {
                        foreach (PointUsed pointUsed in telesignalisationPointList)
                        {
                            IDList.Add(Convert.ToInt32(pointUsed.ID, 16));
                        }
                    }

                    int SOENumber = 1;
                    foreach (XmlElement DIElement in listDI)
                    {
                        HistoryRecordBase historyRecord = new HistoryRecordBase();
                        historyRecord.Number = SOENumber++;
                        historyRecord.IOA = Convert.ToInt32(DIElement.GetAttribute("ioa").Trim());

                        int index = IDList.IndexOf(historyRecord.IOA);
                        if(index != -1)
                        {
                            historyRecord.Name = telesignalisationPointList[index].Name;
                        }

                        historyRecord.TM = DIElement.GetAttribute("tm");
                        historyRecord.Val = DIElement.GetAttribute("val");

                        SOEHistoryRecordList.Add(historyRecord);
                    }

                    break;

                // 操作记录文件
                case "CO":
                    // 初始化CO操作历史记录文件基本信息
                    COHistoryRecordFileInfo = new HistoryRecordInfo(fileType, fileVersion, devName, recordCounts);

                    // 获取使用的所有操作记录点表
                    DevPointDao devPointDao = new DevPointDao();
                    IList<int> COIDList = new List<int>();
                    IList<DevPoint> COPointList = devPointDao.queryByPointTypeId(ConfigUtil.getPointTypeID("操作记录"));

                    if (COPointList != null && COPointList.Count > 0)
                    {
                        foreach (DevPoint COPoint in COPointList)
                        {
                            COIDList.Add(Convert.ToInt32(COPoint.ID, 16));
                        }
                    }

                    int CONumber = 1;
                    foreach (XmlElement DIElement in listDI)
                    {
                        COHistoryRecord COHistoryRecord = new COHistoryRecord();
                        COHistoryRecord.Number = CONumber++;
                        COHistoryRecord.IOA = Convert.ToInt32(DIElement.GetAttribute("ioa").Trim());

                        int index = COIDList.IndexOf(COHistoryRecord.IOA);
                        if (index != -1)
                        {
                            COHistoryRecord.Name = COPointList[index].Name;
                        }

                        COHistoryRecord.TM = DIElement.GetAttribute("tm");
                        COHistoryRecord.CMD = DIElement.GetAttribute("cmd");
                        COHistoryRecord.Val = DIElement.GetAttribute("val");

                        COHistoryRecordList.Add(COHistoryRecord);
                    }

                    break;
 
                // 故障事件文件
                case "FEVENT":
                    break;

            }

        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public HistoryRecordViewModel()
        {
            // 绑定打开历史文件指令(SOE,CO,故障事件等历史记录文件)命令
            HistoryRecordCommand = new RelayCommand(ExecuteHistoryRecordCommand);

            // 初始化SOE历史记录文件基本信息对象
            this._SOEHistoryRecordFileInfo = new HistoryRecordInfo();
            // 初始化SOE历史记录列表
            this._SOEHistoryRecordList = new ObservableCollection<HistoryRecordBase>();

            // 初始化CO操作历史记录文件基本信息对象
            this._COHistoryRecordFileInfo = new HistoryRecordInfo();
            // 初始化CO操作历史记录列表
            this._COHistoryRecordList = new ObservableCollection<COHistoryRecord>();

            // 初始化故障事件历史记录文件基本信息对象
            this._faultEventHistoryRecordFileInfo = new HistoryRecordInfo();
            // 初始化故障事件历史记录列表
            this._faultEventHistoryRecordList = new ObservableCollection<FaultEventHistoryRecord>();

        }

    }
}
