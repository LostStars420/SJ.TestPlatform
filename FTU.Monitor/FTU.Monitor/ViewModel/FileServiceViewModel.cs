using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using lib60870;
using Microsoft.Win32;
using FTU.Monitor.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Threading;
using System.Threading;
using FTU.Monitor.View;
using FTU.Monitor.DataService;
using FTU.Monitor.lib60870;
using System.Collections.Generic;
using FTU.Monitor.WaitingDlgServer;


namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// FileServiceViewModel 的摘要说明
    /// author: songminghao
    /// date：2017/12/9 17:19:40
    /// desc：文件服务ViewModel
    /// version: 1.0
    /// </summary>
    public class FileServiceViewModel : ViewModelBase,IIEC104Handler, ILongTimeTask
    {
        // 默认设置文件缓存区1024*1024字节
        public static byte[] filebuf = new byte[1024 * 1024];
        // 文件长度
        public static int fileSize = 0;
        // 文件位置
        public static int pos = 0;
        // 写文件时继续发送标志位
        public static bool sendfileContinue = false;
        // 正在处于写文件阶段，报文解析会用
        public static bool SendFileDataRunning = false;
        // 等待界面对象
        public static WaitingDlg m_dlgWaiting;
        // 读取文件线程
        private Thread readFileThread;

        /// <summary>
        /// 选中文件所在的索引
        /// </summary>
        public static int fileSelectedIndex;

        /// <summary>
        /// 设置和获取选中文件所在的索引
        /// </summary>
        public int FileSelectedIndex
        {
            get
            {
                return fileSelectedIndex;
            }
            set
            {
                fileSelectedIndex = value;
                RaisePropertyChanged(() => FileSelectedIndex);
            }
        }

        /// <summary>
        /// 目录ID
        /// </summary>
        private int _directoryID;

        /// <summary>
        /// 设置和获取目录ID
        /// </summary>
        public int DirectoryID
        {
            get
            {
                return this._directoryID;
            }
            set
            {
                this._directoryID = value;
                RaisePropertyChanged(() => DirectoryID);
            }
        }

        /// <summary>
        /// 目录名称
        /// </summary>
        private string _directoryName;

        /// <summary>
        /// 设置和获取目录名称
        /// </summary>
        public string DirectoryName
        {
            get
            {
                return this._directoryName;
            }
            set
            {
                this._directoryName = value;
                RaisePropertyChanged(() => DirectoryName);
            }
        }

        /// <summary>
        /// 起始时间
        /// </summary>
        private DateTime _startTime;

        /// <summary>
        /// 设置和获取起始时间
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return this._startTime;
            }
            set
            {
                this._startTime = value;
                RaisePropertyChanged(() => StartTime);
            }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        private DateTime _endTime;

        /// <summary>
        /// 设置和获取结束时间
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return this._endTime;
            }
            set
            {
                this._endTime = value;
                RaisePropertyChanged(() => EndTime);
            }
        }

        /// <summary>
        /// 满足时间段标志
        /// </summary>
        public static bool timeChecked;

        /// <summary>
        /// 设置和获取满足时间段标志
        /// </summary>
        public bool TimeChecked
        {
            get
            {
                return timeChecked;
            }
            set
            {
                timeChecked = value;
                RaisePropertyChanged(() => TimeChecked);
            }
        }

        /// <summary>
        /// 默认目录标志
        /// </summary>
        public static bool defaultChecked;

        /// <summary>
        /// 设置和获取默认目录标志
        /// </summary>
        public bool DefaultChecked
        {
            get
            {
                return defaultChecked;
            }
            set
            {
                defaultChecked = value;
                RaisePropertyChanged(() => DefaultChecked);
            }
        }

        /// <summary>
        /// 读取文件基本信息的集合
        /// </summary>
        public static ObservableCollection<FileModel> fileData;

        /// <summary>
        /// 设置和获取读取文件基本信息的集合
        /// </summary>
        public ObservableCollection<FileModel> FileData
        {
            get
            {
                return fileData;
            }
            set
            {
                fileData = value;
                RaisePropertyChanged(() => FileData);
            }
        }

        /// <summary>
        /// 选中
        /// </summary>
        private int _gridIndex;

        /// <summary>
        /// 设置和获取选中索引
        /// </summary>
        public int GridIndex
        {
            get
            {
                return this._gridIndex;
            }
            set
            {
                this._gridIndex = value;
                RaisePropertyChanged(() => GridIndex);
            }
        }

        /// <summary>
        /// 要读取的文件名
        /// </summary>
        private string _readFileName;

        /// <summary>
        /// 设置和获取要读取的文件名
        /// </summary>
        public string ReadFileName
        {
            get
            {
                return this._readFileName;
            }
            set
            {
                this._readFileName = value;
                RaisePropertyChanged(() => ReadFileName);
            }
        }

        /// <summary>
        /// 要写入数据的文件名
        /// </summary>
        private string _writeFileName;

        /// <summary>
        /// 设置和获取要写入数据的文件名
        /// </summary>
        public string WriteFileName
        {
            get
            {
                return this._writeFileName;
            }
            set
            {
                this._writeFileName = value;
                RaisePropertyChanged(() => WriteFileName);
            }
        }

        /// <summary>
        /// 有关 System.Windows.Controls.DataGrid 类中特定单元格的信息的类对象
        /// </summary>
        private DataGridCellInfo _cellInfo;

        /// <summary>
        /// 设置和获取有关 System.Windows.Controls.DataGrid 类中特定单元格的信息的类对象
        /// </summary>
        public DataGridCellInfo CellInfo
        {
            get
            {
                return this._cellInfo;
            }
            set
            {
                this._cellInfo = value;
                RaisePropertyChanged(() => CellInfo);
            }
        }

        /// <summary>
        /// 已被读取过的文件名集合
        /// </summary>
        public static IList<string> existFileName;

        /// <summary>
        /// 设置和获取已被读取过的文件名集合
        /// </summary>
        public IList<string> ExistFileName
        {
            get
            {
                return existFileName;
            }
            set
            {
                existFileName = value;
            }
        }

        /// <summary>
        /// 全选复选框选中状态值
        /// </summary>
        private bool _comboxChecked;

        /// <summary>
        /// 设置和获取全选复选框选中状态值
        /// </summary>
        public bool ComboxChecked
        {
            get
            {
                return this._comboxChecked;
            }
            set
            {
                this._comboxChecked = value;
                RaisePropertyChanged(() => ComboxChecked);
            }
        }

        /// <summary>
        /// 接受处理文件服务数据集合
        /// </summary>
        /// <param name="TI">文件服务类型标识</param>
        /// <param name="asdu">对应文件服务类型标识的ASDU</param>
        public void HandleASDUData(TypeID TI,ASDU asdu)
        {
             if (asdu.TypeId == TypeID.F_FR_NA_1)// 文件传输 210
                {
                    var f = (FileService)asdu.GetElement(0);
                    if (asdu.Cot == CauseOfTransmission.NOT_ALLOWED_WRITE_FIEL)
                    {
                        MessageBox.Show("不允许写入文件，请等待重试", "提示");
                        MainViewModel.isParseUploadFile = false;
                        MainViewModel.waitUnbrokenFile.Set();
                    }
                }
             else if (asdu.TypeId == TypeID.F_SR_NA_1)// 软件升级 211
             {

             }
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public FileServiceViewModel()
        {
            // 注册接收到初始化结束ASDU的处理事件 
            IEC104.RegisterIEC104Handler(TypeID.F_FR_NA_1, this);// 文件传输 210
            IEC104.RegisterIEC104Handler(TypeID.F_SR_NA_1, this);// 软件升级 211

            FileServiceCommand = new RelayCommand<string>(ExecuteFileServiceCommand);

            this._readFileName = "0";
            this._writeFileName = "0";
            timeChecked = true;
            this._directoryID = 1;
            this._directoryName = "0";
            this._startTime = DateTime.Now;
            this._endTime = DateTime.Now;
            defaultChecked = true;
            existFileName = new List<string>();

            fileData = new ObservableCollection<FileModel>();

            // CellInfo = new DataGridCellInfo(new DataGridCell());
            // fileSelectedIndex = FileServiceViewModel.fileData.Count - 2;
        }

        /// <summary>
        /// 文件服务相关指令
        /// </summary>
        public RelayCommand<string> FileServiceCommand
        {
            get;
            private set;
        }

        public void Start(WaitingDlg dlg)
        {
            m_dlgWaiting = dlg;
            readFileThread = new Thread(readFileThreadWorking);
            readFileThread.Start();
        }

        private void readFileThreadWorking()
        {
            if (!CommunicationViewModel.IsLinkConnect())
            {
                return;
            }

            byte[] temp = new byte[1024];
            byte[] buffer = null;
            byte len = 0;

            foreach (var item in fileData)
            {
                if (item.Selected == true)
                {
                    ReadFileName = item.Name;
                    temp = new byte[1024];
                    buffer = null;
                    len = 0;

                    // 信息体地址
                    temp[len++] = 0x00;
                    temp[len++] = 0x00;
                    if (CommunicationViewModel.con.Parameters.SizeOfIOA == 3)
                    {
                        temp[len++] = 0x00;
                    }

                    temp[len++] = 0x02;
                    // 读文件激活
                    temp[len++] = 0x03;

                    temp[len++] = (byte)ReadFileName.Length;
                    for (int i = 0; i < ReadFileName.Length; i++)
                    {
                        temp[len++] = (byte)ReadFileName[i];
                    }
                    //  len = (byte)(5 + readFileName.Length);
                    buffer = new byte[len];
                    for (int i = 0; i < len; i++)
                    {
                        buffer[i] = temp[i];
                    }

                    CommunicationViewModel.con.SendFileServiceCommand(CauseOfTransmission.ACTIVATION, 1, buffer);

                    // 等待该文件读取结束
                    MainViewModel.waitUnbrokenFile.WaitOne();
                    //if (MainViewModel.isParseUploadFile)
                    //{
                    //    continue;
                    //}
                    //else
                    //{
                    //    break;
                    //}
                }
            }
            m_dlgWaiting.TaskEnd(null);
        }
        /// <summary>
        /// 文件服务相关指令执行操作
        /// </summary>
        /// <param name="arg">指令参数</param>
        private void ExecuteFileServiceCommand(string arg)
        {
            try
            {
                switch (arg)
                {
                    // 清除
                    case "Clear":
                        fileData.Clear();
                        break;

                    // 读目录
                    case "ReadDirectory":

                        #region 读目录
                        if (!CommunicationViewModel.IsLinkConnect())
                        {
                            return;
                        }
                        fileData.Clear();

                        byte[] temp = new byte[1024];
                        byte[] buffer = null;
                        byte len = 0;

                        // 信息体地址,两个字节
                        temp[len++] = 0x00;
                        temp[len++] = 0x00;
                        // 判断信息体对象地址长度
                        if (CommunicationViewModel.con.Parameters.SizeOfIOA == 3)
                        {
                            temp[len++] = 0x00;
                        }

                        // 附加数据包类型
                        temp[len++] = 0x02;


                        #region 文件目录召唤内容

                        // 操作标识,1:读目录
                        temp[len++] = 0x01;

                        // 目录ID,目录标识,低字节在前,4个字节
                        temp[len++] = (byte)DirectoryID;
                        temp[len++] = (byte)(DirectoryID >> 8);
                        temp[len++] = (byte)(DirectoryID >> 16);
                        temp[len++] = (byte)(DirectoryID >> 24);

                        // 是否读取默认目录标志
                        if (DefaultChecked == false)
                        {
                            // 目录名长度:1个字节(大小为x个字节)
                            int length = DirectoryName.Count();
                            temp[len++] = (byte)(length);

                            // 目录名,x个字节
                            for (int i = 0; i < length; i++)
                            {
                                temp[len++] = (byte)DirectoryName[i];
                            }

                            // 是否按时间段查询
                            if (TimeChecked == true)
                            {
                                // 召唤标志:1,目录下满足搜索时间段的文件
                                temp[len++] = 1;

                                // 设置查询起始时间
                                temp[len++] = Convert.ToByte((StartTime.Second * 1000 + DateTime.Now.Millisecond) >> 8);
                                temp[len++] = Convert.ToByte((StartTime.Second * 1000 + DateTime.Now.Millisecond) % 0XFF);
                                temp[len++] = Convert.ToByte(StartTime.Minute);
                                temp[len++] = Convert.ToByte(StartTime.Hour);
                                temp[len++] = Convert.ToByte(StartTime.Day.ToString());
                                temp[len++] = Convert.ToByte(StartTime.Month);
                                temp[len++] = Convert.ToByte(StartTime.Year.ToString().Substring(2, 2));

                                // 设置查询终止时间
                                temp[len++] = Convert.ToByte((EndTime.Second * 1000 + DateTime.Now.Millisecond) >> 8);
                                temp[len++] = Convert.ToByte((EndTime.Second * 1000 + DateTime.Now.Millisecond) % 0XFF);
                                temp[len++] = Convert.ToByte(EndTime.Minute);
                                temp[len++] = Convert.ToByte(EndTime.Hour);
                                temp[len++] = Convert.ToByte(EndTime.Day.ToString());
                                temp[len++] = Convert.ToByte(EndTime.Month);
                                temp[len++] = Convert.ToByte(EndTime.Year.ToString().Substring(2, 2));
                            }
                            else
                            {
                                // 召唤标志:0,目录下所有文件
                                temp[len++] = 0;

                                // 将查询起始时间和查询终止时间设置为0
                                for (int i = 0; i < 14; i++)
                                {
                                    temp[len++] = 0;
                                }
                            }

                            buffer = new byte[len];
                            for (int i = 0; i < len; i++)
                            {
                                buffer[i] = temp[i];
                            }
                        }
                        else
                        {
                            // 是读取默认目录:值为0,表示读取默认目录,目录名则不占任何字节
                            temp[len++] = 0;

                            // 是否按时间段查询
                            if (TimeChecked == true)
                            {
                                // 召唤标志:1,目录下满足搜索时间段的文件
                                temp[len++] = 1;

                                // 设置查询起始时间
                                temp[len++] = Convert.ToByte((StartTime.Second * 1000 + DateTime.Now.Millisecond) >> 8);
                                temp[len++] = Convert.ToByte((StartTime.Second * 1000 + DateTime.Now.Millisecond) % 0XFF);
                                temp[len++] = Convert.ToByte(StartTime.Minute);
                                temp[len++] = Convert.ToByte(StartTime.Hour);
                                temp[len++] = Convert.ToByte(StartTime.Day.ToString());
                                temp[len++] = Convert.ToByte(StartTime.Month);
                                temp[len++] = Convert.ToByte(StartTime.Year.ToString().Substring(2, 2));

                                // 设置查询终止时间
                                temp[len++] = Convert.ToByte((EndTime.Second * 1000 + DateTime.Now.Millisecond) >> 8);
                                temp[len++] = Convert.ToByte((EndTime.Second * 1000 + DateTime.Now.Millisecond) % 0XFF);
                                temp[len++] = Convert.ToByte(EndTime.Minute);
                                temp[len++] = Convert.ToByte(EndTime.Hour);
                                temp[len++] = Convert.ToByte(EndTime.Day.ToString());
                                temp[len++] = Convert.ToByte(EndTime.Month);
                                temp[len++] = Convert.ToByte(EndTime.Year.ToString().Substring(2, 2));
                            }
                            else
                            {
                                // 召唤标志:0,目录下所有文件
                                temp[len++] = 0;

                                // 将查询起始时间和查询终止时间设置为0
                                for (int i = 0; i < 14; i++)
                                {
                                    temp[len++] = 0;
                                }
                            }

                            buffer = new byte[len];
                            for (int i = 0; i < len; i++)
                            {
                                buffer[i] = temp[i];
                            }
                        }

                        #endregion 文件目录召唤内容

                        CommunicationViewModel.con.SendFileServiceCommand(CauseOfTransmission.ACTIVATION, 1, buffer);

                        #endregion 读目录
                        break;

                    // 读文件
                    case "ReadFile":

                        #region 读文件
                        bool isSelected = false;
                        foreach (var item in fileData)
                        {
                            if (item.Selected == true)
                            {
                                isSelected = true;
                                break;
                            }
                        }

                        if (isSelected == true)
                        {
                            WaitingDlg waitingDlg = new WaitingDlg(this);
                            waitingDlg.ShowInTaskbar = false;
                            waitingDlg.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("请至少选择一个读取文件！", "提示");
                        }

                        #endregion

                      
                        break;

                    // 打开录波文件所在的目录
                    case "OpenFolderPath":

                        string v_OpenFolderPath = @".\Comtrade"; 
                        System.Diagnostics.Process.Start("explorer.exe", v_OpenFolderPath);

                        break;
                    #region 读文件数据传输确认
                    //case "ReadFileConfirm"://读文件数据传输确认
                    //    temp = new byte[1024];
                    //    buffer = null;
                    //    len = 0;
                    //    int FileID = 0;
                    //    temp[0] = 0x00;//信息体地址
                    //    temp[1] = 0x00;
                    //    temp[2] = 0x02;//附加数据包类型


                    //    temp[3] = 0x06;//读文件传输确认

                    //    temp[4] = (byte)FileID;//文件ID
                    //    temp[5] = (byte)(FileID >> 8);
                    //    temp[6] = (byte)(FileID >> 16);
                    //    temp[7] = (byte)(FileID >> 24);

                    //    temp[4] = 0;//文件内容偏移指针
                    //    temp[5] = 0;
                    //    temp[6] = 0;
                    //    temp[7] = 0;

                    //    temp[8] = 0;


                    //    buffer = new byte[9];
                    //    for (int i = 0; i < 9; i++)
                    //    {
                    //        buffer[i] = temp[i];
                    //    }
                    //    //Messenger.Default.Send<byte[]>(buffer, "FileServiceMessage");
                    //    CommunicationViewModel.con.SendFileServiceCommand(CauseOfTransmission.ACTIVATION, 1, buffer);

                    //    break;
                    #endregion

                    // 写文件激活
                    case "WriteFile":
                        if (!CommunicationViewModel.IsLinkConnect())
                        {
                            return;
                        }
                        if (WriteFileName != "0")
                        {
                            WriteFileAct(WriteFileName);
                        }
                        break;

                    // 选择文件
                    case "SelectFile":
                        OpenFileDialog dlg = new OpenFileDialog();
                        if (dlg.ShowDialog() == true)
                        {
                            // Open document 
                            string filename = dlg.FileName;
                            WriteFileName = filename;
                        }

                        SelectFile(WriteFileName);

                        break;

                    // 升级文件
                    case "UpdateStart":

                        if (CommunicationViewModel.IsLinkConnect())
                        {
                            CommunicationViewModel.con.SendUpdataCommand(CauseOfTransmission.ACTIVATION, 0x01, 0x80);
                        }
                        break;

                    // 升级文件取消
                    case "UpdateCancel":
                        if (CommunicationViewModel.IsLinkConnect())
                        {
                            CommunicationViewModel.con.SendUpdataCommand(CauseOfTransmission.DEACTIVATION, 0x01, 0x80);
                        }
                        break;

                    // 升级文件结束
                    case "UpdateOver":
                        if (CommunicationViewModel.IsLinkConnect())
                        {
                            CommunicationViewModel.con.SendUpdataCommand(CauseOfTransmission.ACTIVATION, 0x01, 0x0);
                        }
                        break;

                    // 全部选中
                    case "SelectAllCommand":

                        foreach (FileModel p in FileData)
                        {
                            p.Selected = ComboxChecked;
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 写文件激活
        /// </summary>
        /// <param name="fileNameAllPath">文件全路径</param>
        public static void WriteFileAct(string fileNameAllPath)
        {
            byte[] temp = new byte[1024];
            byte[] buffer = null;
            int len = 0;
            int FileID = 0;

            // 信息体地址
            temp[len++] = 0x00;
            temp[len++] = 0x00;
            if (CommunicationViewModel.con.Parameters.SizeOfIOA == 3)
            {
                temp[len++] = 0x00;
            }

            // 附加数据包类型
            temp[len++] = 0x02;
            // 写文件激活
            temp[len++] = 0x07;

            temp[len++] = (byte)(fileNameAllPath.Length);
            for (int i = 0; i < fileNameAllPath.Length; i++)
            {
                temp[len++] = (byte)fileNameAllPath[i];
            }

            // 文件ID
            temp[len++] = (byte)FileID;
            temp[len++] = (byte)(FileID >> 8);
            temp[len++] = (byte)(FileID >> 16);
            temp[len++] = (byte)(FileID >> 24);

            // 文件大小
            temp[len++] = (byte)fileSize;
            temp[len++] = (byte)(fileSize >> 8);
            temp[len++] = (byte)(fileSize >> 16);
            temp[len++] = (byte)(fileSize >> 24);

            buffer = new byte[len];
            for (int i = 0; i < len; i++)
            {
                buffer[i] = temp[i];
            }
            CommunicationViewModel.con.SendFileServiceCommand(CauseOfTransmission.ACTIVATION, 1, buffer);
        }

        /// <summary>
        /// 读文件数据到缓冲区
        /// </summary>
        /// <param name="fileNameAllPath">文件全路径</param>
        public static void SelectFile(string fileNameAllPath)
        {
            try
            {
                using (FileStream read = new FileStream(fileNameAllPath, FileMode.Open))
                {
                    if (read.Length > 10 * 1024 * 1024)
                    {
                        throw new Exception("文件过大");
                    }
                    filebuf = new byte[read.Length];
                    fileSize = (int)read.Length;
                    read.Read(filebuf, 0, (int)read.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "载入文件");
            }

        }

    }
}