using GalaSoft.MvvmLight;
using FTU.Monitor.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using FTU.Monitor.DataService;
using FTU.Monitor.Service;
using System.Windows;
using FTU.Monitor.Util;
using FTU.Monitor.EncryptExportModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Diagnostics;
using System;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {

        }

        /// <summary>
        /// 通知文件传输完成的线程事件
        /// </summary>
       public static AutoResetEvent  waitUnbrokenFile = new AutoResetEvent(false);

       /// <summary>
       /// 是否解析上载的文件（上载三遥点表，上载硬件输入配置）
       /// </summary>
       public static bool isParseUploadFile = true;

        /// <summary>
        /// 使能弹出规约设置窗体，使唯一
        /// </summary>
        public static bool ShowProtocolParameterEnable = true;

        /// <summary>
        /// 使能弹出规约设置窗体，使唯一
        /// </summary>
        public static bool ShowChannelMonitorEnable = true;

        /// <summary>
        /// 使能弹出“超级用户管理”窗体，使唯一
        /// </summary>
        public static bool ShowSuperAdministratorEnable = true;

        /// <summary>
        /// 使能弹出"关于"窗体，使唯一
        /// </summary>
        public static bool ShowAboutEnable = true;

        /// <summary>
        /// 使能弹出"读我"窗体，使唯一
        /// </summary>
        public static bool ShowReadmeEnable = true;

        /// <summary>
        /// 使能弹出"操作手册"窗体，使唯一
        /// </summary>
        public static bool ShowOperatiopnManualEnable = true;

        /// <summary>
        /// 通道监视是否启动
        /// </summary>
        public static long ChannelMonitorListening = 0;

        /// <summary>
        /// 输出数据
        /// </summary>
        public static OutputData outputdata = new OutputData();

        /// <summary>
        /// 设置和获取输出数据
        /// </summary>
        public OutputData Outputdata
        {
            get
            {
                return outputdata;
            }
            set
            {
                outputdata = value;
                RaisePropertyChanged(() => Outputdata);
            }
        }

        #region 菜单栏命令

        /// <summary>
        /// 菜单栏命令
        /// </summary>
        private RelayCommand<string> menuItemCommand;

        /// <summary>
        /// 设置和获取菜单栏命令
        /// </summary>
        public RelayCommand<string> MenuItemCommand
        {
            get
            {
                if (menuItemCommand == null)
                {
                    menuItemCommand = new RelayCommand<string>(p => ExecuteMenuItemCommand(p));
                }
                return menuItemCommand;
            }

            private set
            {
                menuItemCommand = value;
            }
        }

        /// <summary>
        /// 菜单栏命令执行操作
        /// </summary>
        /// <param name="arg">参数</param>
        private void ExecuteMenuItemCommand(string arg)
        {
            switch (arg)
            {
                case "ClearMessageData"://清除报文
                    MainViewModel.outputdata.RawMessageData = "";
                    CommunicationViewModel.CurrentRawMessageCount = 0;
                    ShowMessage.messQueue1.Clear();
                    ShowMessage.messQueue2.Clear();
                    break;

                case "Continue"://报文操作
                    // MainWindow.RollEnable = true;
                    break;

                case "Stop":
                    // MainWindow.RollEnable = false;
                    break;

                case "RawMessageClear":
                    MainViewModel.outputdata.RawMessageData = "";
                    break;

                case "DebugMessageclear":
                    outputdata.Debug = "";

                    break;
                case "PraseInformationclear":
                    outputdata.ParseInformation = "";

                    break;

                case "ShowProtocolParameter":
                    if (ShowProtocolParameterEnable)
                    {
                        ShowProtocolParameterEnable = false;
                        Messenger.Default.Send<object>(null, "ShowProtocolParameter");
                    }
                    break;

                case "ShowChannelMonitor":
                    if (ShowChannelMonitorEnable)
                    {
                        ShowChannelMonitorEnable = false;
                        Messenger.Default.Send<object>(null, "ShowChannelMonitor");
                    }
                    break;

                case "ShowWaveFormAnalysis":
                    try
                    {
                        // 获取分布式DTU程序的路径
                        string waveFormAnalysisExePath = System.AppDomain.CurrentDomain.BaseDirectory + @"WaveFormAnalysis\caap2000.exe";
                        var p = new Process();
                        p.StartInfo = new ProcessStartInfo(waveFormAnalysisExePath);
                        // 设置分布式DTU程的工作目录
                        p.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(waveFormAnalysisExePath);
                        p.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("启动分布式DTU上位机时抛出异常！ " + ex.ToString(), "警告");
                    }
                    break;

                case "ShowManageUsers":
                    if (ShowSuperAdministratorEnable)
                    {
                        ShowSuperAdministratorEnable = false;
                        Messenger.Default.Send<object>(null, "ShowManageUsers");
                    }
                    break;

                case "ShowAbout":
                    if (ShowAboutEnable)
                    {
                        ShowAboutEnable = false;
                        Messenger.Default.Send<object>(null, "ShowAbout");
                    }
                    break;

                case "ShowReadme":
                    if (ShowReadmeEnable)
                    {
                        ShowReadmeEnable = false;
                        Messenger.Default.Send<object>(null, "ShowReadme");
                    }
                    break;

                case "ShowOperationManual":
                    if(ShowOperatiopnManualEnable)
                    {
                        ShowOperatiopnManualEnable = false;
                        Messenger.Default.Send<object>(null, "ShowOperationManual");
                    }
                    break;
            }

        }

        #endregion 菜单栏命令

        #region 工具栏命令
        /// <summary>
        /// 工具栏命令
        /// </summary>
        private RelayCommand<string> toolBarCommand;

        /// <summary>
        /// 设置和获取工具栏命令
        /// </summary>
        public RelayCommand<string> ToolBarCommand
        {
            get
            {
                if (toolBarCommand == null)
                {
                    toolBarCommand = new RelayCommand<string>(p => ExecuteToolBarCommand(p));
                }
                return toolBarCommand;
            }

            private set
            {
                toolBarCommand = value;
            }
        }

        /// <summary>
        /// 主界面工具栏执行命令
        /// </summary>
        /// <param name="arg"></param>
        private void ExecuteToolBarCommand(string arg)
        {
            switch(arg)
            {
                // 打开连接
                case "StartLink":
                    Messenger.Default.Send<string>("StartLink", "LinkCommand");
                    break;

                // 关闭连接
                case "StopLink":
                    Messenger.Default.Send<string>("StopLink", "LinkCommand");
                    break;

                // 复位进程
                case "InitProcess":
                    Messenger.Default.Send<string>("CmdInitProcessData", "MasterCommand");
                    break;

                // 总召唤
                case "CallAllData":
                    Messenger.Default.Send<string>("CmdAskAll", "MasterCommand");
                    break;

                // 测试链路
                case "TestLink":
                    Messenger.Default.Send<string>("CmdTestData", "MasterCommand");
                    break;
            }
        }


        #endregion 工具栏命令

        #region 主窗口关闭命令

        /// <summary>
        /// 主窗口关闭命令
        /// </summary>
        private RelayCommand<object> mainWindowCloseCommand;

        /// <summary>
        /// 设置和获取主窗口关闭命令
        /// </summary>
        public RelayCommand<object> MainWindowCloseCommand
        {
            get
            {
                if (mainWindowCloseCommand == null)
                {
                    mainWindowCloseCommand = new RelayCommand<object>(p => ExecuteMainWindowCloseCommand(p));
                }
                return mainWindowCloseCommand;
            }

            private set
            {
                mainWindowCloseCommand = value;
            }
        }

        /// <summary>
        /// 主窗口关闭命令执行操作
        /// </summary>
        /// <param name="e"></param>
        private void ExecuteMainWindowCloseCommand(object e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

        #endregion 主窗口关闭命令

        #region 点表管理菜单栏命令

        /// <summary>
        /// 点表管理菜单栏命令
        /// </summary>
        public RelayCommand<string> menuItemPointCommand;

        /// <summary>
        /// 点表管理菜单栏命令
        /// </summary>
        public RelayCommand<string> MenuItemPointCommand
        {
            get
            {
                if (menuItemPointCommand == null)
                {
                    menuItemPointCommand = new RelayCommand<string>(p => ExecuteMenuItemPointCommand(p));
                }
                return menuItemPointCommand;
            }

            private set
            {
                menuItemPointCommand = value;
            }
        }

        /// <summary>
        /// 点表管理菜单栏命令执行操作
        /// </summary>
        /// <param name="arg"></param>
        private void ExecuteMenuItemPointCommand(string arg)
        {
            // 定义点表管理业务逻辑service类对象
            ManagePointTableService managePointTableService = new ManagePointTableService();
            switch (arg)
            {
                // 导入所有点表
                case "ImportAllPoint":
                    ConstantParameterExportModel constantParameterExportModel = new ConstantParameterExportModel();
                    ProgramVersionModel programVersionModel = managePointTableService.GetProgramVersion();
                    if (programVersionModel == null)
                    {
                        MessageBox.Show("没有程序版本号");
                        break;
                    }
                    string promptMsg = "";
                    managePointTableService.ImportAllPoint(programVersionModel.Version, ref promptMsg, ref constantParameterExportModel);
                    // 更新相应页面的点表
                    ConfigViewModel.UpdateDisplayedPoint();
                    // 更新定值参数显示界面
                    Messenger.Default.Send<ConstantParameterExportModel>(constantParameterExportModel, "UpdateConstantPoint");
                    MessageBox.Show(promptMsg);
                    break;

                // 导出所有点表
                case "ExportAllPoint":
                    Messenger.Default.Send<object>(null, "ExportAllPoint");
                    break;
            }
        }

        #endregion 点表管理菜单栏命令

    }
}