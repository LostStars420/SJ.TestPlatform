using FTU.Monitor.DataService;
using FTU.Monitor.EncryptExportModel;
using FTU.Monitor.Model;
using FTU.Monitor.Service;
using FTU.Monitor.Util;
using FTU.Monitor.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using lib60870;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Xml;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// InherentParameterViewModel 的摘要说明
    /// author: songminghao
    /// date：2017/12/20 21:40:52
    /// desc：固有参数ViewModel
    /// version: 1.0
    /// </summary>
    public class InherentParameterViewModel : ViewModelBase,IIEC104Handler
    {
        /// <summary>
        /// 配置文件对象
        /// </summary>
        public XmlDocument PWdocument;

        /// <summary>
        /// 程序版本号
        /// </summary>
        public static string programVersion;

        /// <summary>
        /// 设置和获取程序版本号
        /// </summary>
        public string ProgramVersion
        {
            get
            {
                return programVersion;
            }
            set
            {
                programVersion = value;
                RaisePropertyChanged(() => ProgramVersion);
            }

        }

        /// <summary>
        /// 终端软件程序对应的版本
        /// </summary>
        public static int programVersionId;

        /// <summary>
        /// 设置和获取终端软件程序对应的版本
        /// </summary>
        public int ProgramVersionId
        {
            get
            {
                return programVersionId;
            }
            set
            {
                programVersionId = value;
                RaisePropertyChanged(() => ProgramVersionId);
            }
        }

        /// <summary>
        /// Xml配置文件中，修改软件程序版本的Password
        /// </summary>
        private string _changeSoftwareVersionPassword;

        /// <summary>
        /// 设置和获取Xml配置文件中，修改软件程序版本的Password
        /// </summary>
        public string ChangeSoftwareVersionPassword
        {
            get 
            { 
                return this._changeSoftwareVersionPassword; 
            }
            set
            {
                this._changeSoftwareVersionPassword = value;
                RaisePropertyChanged(() => ChangeSoftwareVersionPassword);
            }
        }

        /// <summary>
        /// 固有定值参数加载显示数据集合
        /// </summary>
        public static ObservableCollection<ConstantParameter> inherentParameterData;

        /// <summary>
        /// 设置和获取固有定值参数加载显示数据集合
        /// </summary>
        public ObservableCollection<ConstantParameter> InherentParameterData
        {
            get
            {
                return inherentParameterData;
            }
            set
            {
                inherentParameterData = value;
                RaisePropertyChanged(() => InherentParameterData);
            }
        }

        /// <summary>
        /// 全选复选框选中状态值
        /// </summary>
        private bool _comboxInherentChecked;

        /// <summary>
        /// 设置和获取全选复选框选中状态值
        /// </summary>
        public bool ComboxInherentChecked
        {
            get
            {
                return this._comboxInherentChecked;
            }
            set
            {
                this._comboxInherentChecked = value;
                RaisePropertyChanged(() => ComboxInherentChecked);
            }
        }

        /// <summary>
        /// 全部选择命令
        /// </summary>
        public RelayCommand SelectAllInherentCommand { get; private set; }

        /// <summary>
        /// 全部选择命令执行操作
        /// </summary>
        public void ExecuteSelectAllInherentCommand()
        {
            if (InherentParameterData != null && InherentParameterData.Count > 0)
            {
                foreach (ConstantParameter p in InherentParameterData)
                {
                    p.Selected = ComboxInherentChecked;
                }
            }

        }

        /// <summary>
        /// 固有定值参数命令
        /// </summary>
        public RelayCommand<string> InherentParameterCommand { get; private set; }

        /// <summary>
        /// 固有定值参数命令执行操作
        /// </summary>
        /// <param name="arg">参数</param>
        public void ExecuteInherentParameterCommand(string arg)
        {
            try
            {
                TelecontrolPWView PWView = new TelecontrolPWView();
                switch (arg)
                {
                    // 导入Excel固有定值参数数据
                    case "ImportData":

                        // 获取文件内容解密后的字符串
                        string jsonToImport = ReportUtil.GetParameterDataCiphertext();
                        // 判断加密的字符串解密是否成功
                        if (UtilHelper.IsEmpty(jsonToImport))
                        {
                            MessageBox.Show("文件内容不正确，解密失败", "提示");
                            break;
                        }

                        // 将Json格式的字符串转换成对应的数据对象
                        ConstantParameterExportModel inherentParameterExportModelImport = EncryptAndDecodeUtil.JsonToObject<ConstantParameterExportModel>(jsonToImport);
                        // 获取终端序列号
                        string programVersionFromJson = inherentParameterExportModelImport.DeviceSerialNumber;
                        // 判断终端序列号和连接的终端序列号是否一致
                        if(!programVersionFromJson.Equals(InherentParameterViewModel.programVersion))
                        {
                            MessageBox.Show("即将导入的点表中，终端序列号不匹配", "提示");
                            break;
                        }

                        //IList<ConstantParameter> parameterList = ParameterViewModel.ImportParameterData("固有定值参数");

                        // 获取固有定值参数对象列表
                        IList<ContantParameterForExport> parameterList = inherentParameterExportModelImport.ConstantParameterList;
                        if (parameterList != null && parameterList.Count > 0)
                        {
                            InherentParameterData.Clear();
                            foreach (var parameter in parameterList)
                            {
                                ConstantParameter constantParameter = new ConstantParameter();
                                constantParameter.Selected = parameter.Selected;
                                constantParameter.Number = parameter.Number;
                                constantParameter.ID = parameter.ID;
                                constantParameter.Name = parameter.Name;
                                constantParameter.StringValue = parameter.StringValue;
                                constantParameter.Unit = parameter.Unit;
                                constantParameter.Comment = parameter.Comment;
                                constantParameter.MinValue = parameter.MinValue;
                                constantParameter.MaxValue = parameter.MaxValue;
                                constantParameter.Value = parameter.Value;

                                inherentParameterData.Add(constantParameter);
                            }
                            MessageBox.Show("导入成功", "提示");
                        }

                        break;

                    // 导出固有定值参数数据
                    case "ExportData":

                        // 固有定值参数导出对象
                        ConstantParameterExportModel inherentParameterExportModel = new ConstantParameterExportModel();
                        if (inherentParameterData != null && inherentParameterData.Count > 0)
                        {
                            // 清空定值参数对象列表
                            inherentParameterExportModel.ConstantParameterList.Clear();
                            for (int i = 0; i < inherentParameterData.Count; i++)
                            {
                                ContantParameterForExport contantParameterForExport = new ContantParameterForExport();
                                contantParameterForExport.Selected = inherentParameterData[i].Selected;
                                contantParameterForExport.Number = inherentParameterData[i].Number;
                                contantParameterForExport.ID = inherentParameterData[i].ID;
                                contantParameterForExport.Name = inherentParameterData[i].Name;
                                contantParameterForExport.StringValue = inherentParameterData[i].StringValue;
                                contantParameterForExport.Unit = inherentParameterData[i].Unit;
                                contantParameterForExport.Comment = inherentParameterData[i].Comment;
                                contantParameterForExport.MinValue = inherentParameterData[i].MinValue;
                                contantParameterForExport.MaxValue = inherentParameterData[i].MaxValue;
                                contantParameterForExport.Value = inherentParameterData[i].Value;

                                // 添加到定值参数对象列表
                                inherentParameterExportModel.ConstantParameterList.Add(contantParameterForExport);
                            }
                        }

                        // 设置终端序列号
                        inherentParameterExportModel.DeviceSerialNumber = programVersion;

                        // 将固有定值参数导出对象转换为Json格式字符串
                        string inherentParameterExportModelToJson = EncryptAndDecodeUtil.GetJson(inherentParameterExportModel);
                        // 给转换后的Json格式字符串进行AES加密
                        string encrypt = EncryptAndDecodeUtil.AESEncrypt(inherentParameterExportModelToJson, false);
                        // 判断加密是否成功
                        if (String.Empty.Equals(encrypt))
                        {
                            MessageBox.Show("导出内容加密失败", "提示");
                            break;
                        }

                        ReportUtil.ExportParameterData("固有定值参数", encrypt);
                        break;

                    // 读多个参数和定值(固有参数)
                    case "InherentParameterRead":

                        if(CommunicationViewModel.IsLinkConnect())
                        {
                            // 定义选中的定值参数列表
                            IList<ConstantParameter> inherentParameterReadList = new List<ConstantParameter>();
                            // 获取选中的定值参数
                            for (int i = 0; i < InherentParameterData.Count; i++)
                            {
                                if (InherentParameterData[i].Selected == true)
                                {
                                    inherentParameterReadList.Add(InherentParameterData[i]);
                                }
                            }

                            if (inherentParameterReadList == null || inherentParameterReadList.Count == 0)
                            {
                                MessageBox.Show("请选择要读取的固有定值参数！！！");
                                break;
                            }

                            ParameterViewModel.readPartsParameter(0, inherentParameterReadList);
                        }
                        break;

                    case "ChangeVersion": // 当前已取消该手动修改程序版本的功能
                        //弹出提示框 输入密码
                        PWView.ShowDialog();
                        if (TelecontrolViewModel.isPW == true)//用户点击“确定”按钮
                        {
                            //从XML配置文件中读取正确的密码，并与用户输入密码进行比较
                            ReadChangeSoftwareVersionPassword();
                            if(ChangeSoftwareVersionPassword == TelecontrolPWViewModel.pwBox)
                            {
                                TelecontrolPWViewModel.pwBox = "";
                                //修改FTU软件程序版本
                                new ChangeSoftwareVersionView().ShowDialog();
                            }
                            else
                            {
                                TelecontrolPWViewModel.pwBox = "";
                                MessageBox.Show("输入的口令不正确", "提示");
                                TelecontrolViewModel.isPW = false;
                                return;
                            }
                        }
                        else
                        {
                            TelecontrolViewModel.isPW = false;
                            return;
                        }
                        //执行修改数据库的操作
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 更新终端软件程序版本
        /// </summary>
        /// <param name="arg"></param>
        void UpdateSoftwareVersion(string arg)
        {
            ProgramVersion = arg;
        }

        /// <summary>
        /// 上载终端所有点表后，更新终端软件程序版本
        /// </summary>
        /// <param name="arg"></param>
        private void ExcuteUpdateProductSerialNumber(string arg)
        {
            //ProgramVersion = arg.Trim().Substring(0, 12);
            ProgramVersion = UtilHelper.GetTerminalSeriaNumber(arg.Trim());
            ParameterManageService parameterManageService = new ParameterManageService();
            bool isUpdateSuccess = parameterManageService.UpdateProgramVersion(arg, programVersionId);
            if(!isUpdateSuccess)
            {
                MessageBox.Show("终端序列号更新失败","警告");
            }
            else
            {
                programVersion = arg;
                Messenger.Default.Send<string>(arg, "UpdateProductSerialNumberSecond");
            }
        }

        /// <summary>
        /// 接受处理固有参数数据集合
        /// </summary>
        /// <param name="TI">固有参数类型标识</param>
        /// <param name="asdu">对应固有参数类型标识的ASDU</param>
        public void HandleASDUData(TypeID TI, ASDU asdu)
        {
            // 读取终端设备运行的软件版本号并做处理
            // CheckProgrammVersion();
            // 启动测试帧定时器
            // Messenger.Default.Send<string>("startTimer", "StartTestFrameTimer");
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public InherentParameterViewModel()
        {
            // 注册接收到初始化结束ASDU的处理事件 
            IEC104.RegisterIEC104Handler(TypeID.M_EI_NA_1, this);// 初始化结束 70
            Messenger.Default.Register<string>(this, "updateSoftwareVersion", UpdateSoftwareVersion);
            Messenger.Default.Register<string>(this, "UpdateProductSerialNumber", ExcuteUpdateProductSerialNumber);
            // 注册接收更新相应页面的点表消息(点表重新导入后，需要更新相应页面显示的点表)
            Messenger.Default.Register<object>(this, "UpdateSourcePoint", ReloadinherentParameterPoint);

            ManagePointTableService managePointTableService = new ManagePointTableService();
            ProgramVersionModel programVersionModel = managePointTableService.GetProgramVersion();
            if (programVersionModel != null)
            {
                programVersion = programVersionModel.Version;
                programVersionId = programVersionModel.Id;
            }

            InherentParameterCommand = new RelayCommand<string>(ExecuteInherentParameterCommand);
            SelectAllInherentCommand = new RelayCommand(ExecuteSelectAllInherentCommand);

            inherentParameterData = new ObservableCollection<ConstantParameter>();
            // 重新载入点表
            ReloadinherentParameterPoint(null);

            this._comboxInherentChecked = false;
        }

        /// <summary>
        /// 重新载入固有定值参数点表
        /// </summary>
        /// <param name="obj"></param>
        private void ReloadinherentParameterPoint(object obj)
        {
            ParameterManageService parameterManageService = new ParameterManageService();
            // 获取固有定值参数点表
            IList<ConstantParameter> constantParameterList = parameterManageService.GetConstantParameter(ConfigUtil.getPointTypeID("固有定值参数"));
            if (constantParameterList != null && constantParameterList.Count > 0)
            {
                inherentParameterData.Clear();
                foreach (var constantParameter in constantParameterList)
                {
                    inherentParameterData.Add(constantParameter);
                }
            }

        }

        /// <summary>
        /// 初始化结束后，读取终端运行软件程序的版本号，并作出决策
        /// </summary>
        public static void CheckProgrammVersion()
        {
            //读取所有固有定值参数
            ParameterViewModel.readPartsParameter(0, inherentParameterData);

            Console.WriteLine(inherentParameterData);

            Thread thread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(6000);
                for (int i = 0; i < inherentParameterData.Count; i++)
                {
                    if (inherentParameterData[i].Name.Equals("终端产品序列号"))
                    {
                        if (inherentParameterData[i].StringValue != null)
                        {
                            //读取的终端序列号前12位
                            //string inherentParameter = inherentParameterData[i].StringValue.Trim().Substring(0, 12);
                            string inherentParameter = UtilHelper.GetTerminalSeriaNumber(inherentParameterData[i].StringValue.Trim());
                            //存储的终端序列号前12位
                            //string terminalProgram = programVersion.Substring(0, 12);
                            string terminalProgram = UtilHelper.GetTerminalSeriaNumber(programVersion);
                            if (!inherentParameter.Equals(terminalProgram))
                            {
                                MessageBoxResult result = MessageBox.Show("终端序列号与上位机软件版本不匹配！","警告");
                            }                         
                        }
                        else
                        {
                            MessageBoxResult result = MessageBox.Show("终端序列号为空值，召唤固有参数超时，请重启上位机！", "警告");
                            if (result == MessageBoxResult.OK || result == MessageBoxResult.Cancel)
                            {
                                Environment.Exit(0);//强制退出程序，即使其他线程没有结束
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }));

            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 读取修改软件程序版本的操作密码
        /// </summary>
        private void ReadChangeSoftwareVersionPassword()
        {
            PWdocument = new XmlDocument();

            //PWdocument.Load(@".\Config\XML\ChangeSoftwareVersionPW.xml");
            string ExePath = System.AppDomain.CurrentDomain.BaseDirectory + @"Config\XML\ChangeSoftwareVersionPW.xml";
            PWdocument.Load(ExePath);

            XmlNodeList listPW = PWdocument.GetElementsByTagName("Password");

            for (int i = 0; i < listPW.Count; i++)
            {
                ChangeSoftwareVersionPassword = listPW[i].InnerText;
            }
        }
    }
}
