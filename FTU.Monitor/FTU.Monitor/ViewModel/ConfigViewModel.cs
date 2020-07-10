using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using FTU.Monitor.Model.DTUConfigurePointTableModelCollection;
using FTU.Monitor.Service;
using FTU.Monitor.UpLoadConfigurePointTable;
using FTU.Monitor.Util;
using FTU.Monitor.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using lib60870;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// ConfigViewModel 的摘要说明
    /// author: zhengshuiqing
    /// date：2017/10/10 15:33:32
    /// desc：下发参数配置ViewModel
    /// version: 1.0
    /// </summary>
    class ConfigViewModel : ViewModelBase
    {
        #region 模板点表配置属性定义
        /// <summary>
        /// 模板所有配置列表
        /// </summary>
        private List<DTUNode> _DTUConfigurePointTableAllNodes;

        /// <summary>
        /// 设置和获取所有配置列表
        /// </summary>
        private List<DTUNode> DTUConfigurePointTableAllNodes
        {
            get
            {
                return this._DTUConfigurePointTableAllNodes;
            }
            set
            {
                this._DTUConfigurePointTableAllNodes = value;
            }
        }

        /// <summary>
        /// DTU模板点表数据库操作对象
        /// </summary>
        private DTUConfigurePointTableDao DTUConfigurePointTableDaoObject;

        /// <summary>
        /// 当前模块名称
        /// </summary>
        private string _moduleName;

        /// <summary>
        /// 设置和获取当前模块名称
        /// </summary>
        public string ModuleName
        {
            get
            {
                return this._moduleName;
            }
            set
            {
                this._moduleName = value;
                RaisePropertyChanged(() => ModuleName);
            }
        }

        /// <summary>
        /// 当前模块的遥信个数
        /// </summary>
        private int? _currentTeleSignalisationNumber;

        /// <summary>
        /// 设置和获取当前模块的遥信个数
        /// </summary>
        public int? CurrentTeleSignalisationNumber
        {
            get
            {
                return this._currentTeleSignalisationNumber;
            }
            set
            {
                this._currentTeleSignalisationNumber = value;
                RaisePropertyChanged(() => CurrentTeleSignalisationNumber);
            }
        }

        /// <summary>
        /// 当前模块的遥测个数
        /// </summary>
        private int? _currentTeleMeteringNumber;

        /// <summary>
        /// 设置和获取当前模块的遥测个数
        /// </summary>
        public int? CurrentTeleMeteringNumber
        {
            get
            {
                return this._currentTeleMeteringNumber;
            }
            set
            {
                this._currentTeleMeteringNumber = value;
                RaisePropertyChanged(() => CurrentTeleMeteringNumber);
            }
        }

        /// <summary>
        /// 当前模块的遥控个数
        /// </summary>
        private int? _currentTeleControlNumber;

        /// <summary>
        /// 设置和获取当前模块的遥控个数
        /// </summary>
        public int? CurrentTeleControlNumber
        {
            get
            {
                return this._currentTeleControlNumber;
            }
            set
            {
                this._currentTeleControlNumber = value;
                RaisePropertyChanged(() => CurrentTeleControlNumber);
            }
        }

        /// <summary>
        /// 当前模块的从站地址
        /// </summary>
        private int? _currentSalveAddress;

        /// <summary>
        /// 设置和获取当前模块的从站地址
        /// </summary>
        public int? CurrentSalveAddress
        {
            get
            {
                return this._currentSalveAddress;
            }
            set
            {
                this._currentSalveAddress = value;
                RaisePropertyChanged(() => CurrentSalveAddress);
            }
        }

        # endregion 模板点表配置属性定义

        /// <summary>
        /// 设备ID号内容字符串验证正则表达式
        /// </summary>
        private static string validDeviceID = "^[F]([0-9a-zA-Z.]){23}$";

        #region 属性定义

        /// <summary>
        /// 数据表格选中索引数组
        /// </summary>
        private int[] _gridIndex;

        /// <summary>
        /// 设置和获取数据表格选中索引数组
        /// </summary>
        public int[] GridIndex
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
        /// 遥信原始点表模板
        /// </summary>
        public static ObservableCollection<Telesignalisation> telesignalisationPointModel;

        /// <summary>
        /// 设置和获取遥信原始点表模板
        /// </summary>
        public ObservableCollection<Telesignalisation> TelesignalisationPointModel
        {
            get
            {
                return telesignalisationPointModel;
            }
            set
            {
                telesignalisationPointModel = value;
                RaisePropertyChanged(() => TelesignalisationPointModel);
            }
        }

        /// <summary>
        /// 遥信原始点表模板中的点号数组
        /// </summary>
        private static List<int> telesignalisationPointModelIDIndex;

        /// <summary>
        /// 选中的要下发的遥信点号
        /// </summary>
        public static ObservableCollection<Telesignalisation> telesignalisationPoint;

        /// <summary>
        /// 设置和获取选中的要下发的遥信点号
        /// </summary>
        public ObservableCollection<Telesignalisation> TelesignalisationPoint
        {
            get
            {
                return telesignalisationPoint;
            }
            set
            {
                telesignalisationPoint = value;
                RaisePropertyChanged(() => TelesignalisationPoint);
            }
        }

        /// <summary>
        /// 遥测原始点表模板
        /// </summary>
        public static ObservableCollection<Telemetering> telemeteringPointModel;

        /// <summary>
        /// 设置和获取遥测原始点表模板
        /// </summary>
        public ObservableCollection<Telemetering> TelemeteringPointModel
        {
            get
            {
                return telemeteringPointModel;
            }
            set
            {
                telemeteringPointModel = value;
                RaisePropertyChanged(() => TelemeteringPointModel);
            }
        }

        /// <summary>
        /// 遥测原始点表模板中的点号数组
        /// </summary>
        private static List<int> telemeteringPointModelIDIndex;

        /// <summary>
        /// 选中的要下发的遥测点号
        /// </summary>
        public static ObservableCollection<Telemetering> telemeteringPoint;

        /// <summary>
        /// 设置和获取选中的要下发的遥测点号
        /// </summary>
        public ObservableCollection<Telemetering> TelemeteringPoint
        {
            get
            {
                return telemeteringPoint;
            }
            set
            {
                telemeteringPoint = value;
                RaisePropertyChanged(() => TelemeteringPoint);
            }
        }

        /// <summary>
        /// 遥控原始点表模板
        /// </summary>
        public static ObservableCollection<Telecontrol> telecontrolPointModel;

        /// <summary>
        /// 设置和获取遥控原始点表模板
        /// </summary>
        public ObservableCollection<Telecontrol> TelecontrolPointModel
        {
            get
            {
                return telecontrolPointModel;
            }
            set
            {
                telecontrolPointModel = value;
                RaisePropertyChanged(() => TelecontrolPointModel);
            }
        }

        /// <summary>
        /// 遥控原始点表模板中的点号数组
        /// </summary>
        private static List<int> telecontrolPointModelIDIndex;

        /// <summary>
        /// 选中的要下发的遥控点号
        /// </summary>
        public static ObservableCollection<Telecontrol> telecontrolPoint;

        /// <summary>
        /// 设置和获取选中的要下发的遥控点号
        /// </summary>
        public ObservableCollection<Telecontrol> TelecontrolPoint
        {
            get
            {
                return telecontrolPoint;
            }
            set
            {
                telecontrolPoint = value;
                RaisePropertyChanged(() => TelecontrolPoint);
            }
        }

        /// <summary>
        /// 配置数据Model
        /// </summary>
        public static ConfigurationSet configurationSetData;

        /// <summary>
        /// 设置和获取配置数据Model
        /// </summary>
        public ConfigurationSet ConfigurationSetData
        {
            get
            {
                return configurationSetData;
            }
            set
            {
                configurationSetData = value;
                RaisePropertyChanged(() => ConfigurationSetData);
            }
        }

        #region 定义配置输出结构体中配置项的数量

        private const int YXNumber = 300;
        private const int YCNumber = 300;
        private const int YKNumber = 16;
        private const int DeviceIDNumber = 24;

        #endregion 定义配置输出结构体中配置项的数量

        /// <summary>
        /// 定义配置输出结构体
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct _ConfigurationSet
        {
            /*
            
            #region 串口接口配置

            public UInt16 UartPort;          // 使用串口，左为1，中为2
            public UInt16 UartBaudRate;      // 波特率
            public UInt16 UartWordLength;    // 数据位
            public UInt16 UartStopBits;      // 停止位
            public UInt16 UartParity;        // 校验位，无校验1,奇校验2,偶校验3

            #endregion 串口接口配置


            #region 串口应用配置

            public UInt16 UartBalanMode;     // 模式，非平衡1，平衡2
            public UInt16 UartSourceAddr;    // 从站地址
            public UInt16 UartLinkAddrSize;  // 从站地址长度
            public UInt16 UartASDUCotSize;   // 传送原因长度
            public UInt16 UartASDUAddr;      // ASDU地址
            public UInt16 UartASDUAddrSize;  // ASDU地址长度

            #endregion 串口应用配置


            #region 网口接口配置

            // IP地址1
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt16[] NetIPOne;

            // IP地址2
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt16[] NetIPTwo;

            // 子网掩码
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt16[] NetNetmask;

            // 网关
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt16[] NetGateway;

            // DNS
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt16[] NetDNS;

            #endregion 网口接口配置


            #region  网口应用配置

            // 从站地址
            public UInt16 NetSourceAddr;
            // ASDU地址
            public UInt16 NetASDUAddr;

            #endregion 网口应用配置
            */

            // 遥信点表配置内容,包括遥信点表其它标志(取反、变化、双点)
            // 取反(不取反为0，取反为1)，占0-1位；变化(不变化为0，变化为1)，占2-3位；双点(单点为1，双点为3)，占4-5位
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = YXNumber)]// 300
            public UInt16[] YXAddrVariable;

            // 遥测点表映射
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = YCNumber)]// 300
            public UInt16[] YCAddr;//原始点号

            // 遥测点表其它标志(取反、变化、双点)
            // 数据类型(0代表归一化值（9），1代表标度化值（11），2代表短浮点数值（13）)，占0-1位；倍率，占2-15位
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = YCNumber)]// 300
            public UInt16[] YCOtherFlag;

            // 遥测倍率
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = YCNumber)]// 300
            public float[] YCRate;

            // 遥控点表映射
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = YKNumber)]// 16
            public UInt16[] YKAddr;//原始点号

            // 遥控点表其它标志(取反、变化、双点)
            // 取反(不取反为0，取反为1)，占0-1位；变化(不变化为0，变化为1)，占2-3位；双点(单点为1，双点为3)，占4-5位
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = YKNumber)]// 16
            public UInt16[] YKOtherFlag;

            /// <summary>
            /// 设备ID号,24字节长度
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DeviceIDNumber)]// 24
            public byte[] DeviceID;

        }

        /// <summary>
        /// 声明一个配置输出结构体对象
        /// </summary>
        private _ConfigurationSet configurationSet;

        /// <summary>
        /// 用户选择标志位
        /// </summary>
        public static bool isPW;

        /// <summary>
        /// 设置和获取用户选择标志位
        /// </summary>
        public bool IsPW
        {
            get
            {
                return isPW;
            }
            set
            {
                isPW = value;
            }
        }

        /// <summary>
        /// Xml中Password
        /// </summary>
        public static string configPW;

        /// <summary>
        /// 设置和获取配置文件XML中的遥控口令
        /// </summary>
        public string ConfigPW
        {
            get
            {
                return configPW;
            }
            set
            {
                configPW = value;
            }
        }

        #endregion 属性定义


        #region 方法定义

        /// <summary>
        /// 配置参数报表指令
        /// </summary>
        public RelayCommand<string> ConfigReportCommand { get; private set; }

        /// <summary>
        /// 配置参数报表指令执行操作
        /// </summary>
        /// <param name="arg">字符串参数，表示哪一个操作</param>
        private void ExecuteConfigReportCommand(string arg)
        {
            // 输入召唤终端所有点表操作权限密码
            TelecontrolPWView PWView = new TelecontrolPWView();
            TelecontrolPWViewModel PWViewModelObject = new TelecontrolPWViewModel();

            switch (arg)
            {
                // 从本地读配置文件
                case "FromLocal":

                    string errorFromLocalMsg = ReadConfigFromLocal();
                    if (!UtilHelper.IsEmpty(errorFromLocalMsg))
                    {
                        MessageBox.Show(errorFromLocalMsg, "错误");
                    }

                    break;

                // 导出配置文件到本地
                case "ToLocal":
                    List<byte> telesignalisationPointArryToLocal = GetTelesignalisationPointArry();
                    if (telesignalisationPointArryToLocal.Count > (YXNumber * 2))
                    {
                        MessageBox.Show("遥信点表配置内容超过上限", "提示");
                        break;
                    }

                    // 检验设备ID号
                    if (ConfigurationSetData.DeviceID != null && !"".Equals(ConfigurationSetData.DeviceID.Trim()))
                    {
                        bool isLegal = CheckDeviceID(ConfigurationSetData.DeviceID);
                        if (!isLegal)
                        {
                            MessageBox.Show("设备ID号不合法.设备ID号是以F开头,以数字和字母组合的长度为24个的字符串", "提示");
                            break;
                        }
                    }

                    /*
                    // 检验要下发的遥信点号是否正确
                    if(!checkDownTelesignalisationPoint())
                    {
                        break;
                    }
                     * */

                    DownConfig(telesignalisationPointArryToLocal);
                    break;

                // 下发配置到设备
                case "Down":

                    try
                    {
                        if (!CommunicationViewModel.IsLinkConnect())
                        {
                            return;
                        }
                        List<byte> telesignalisationPointArryDown = GetTelesignalisationPointArry();
                        if (telesignalisationPointArryDown.Count > (YXNumber * 2))
                        {
                            MessageBox.Show("遥信点表配置内容超过上限", "提示");
                            break;
                        }

                        // 检验设备ID号
                        if (ConfigurationSetData.DeviceID != null && !"".Equals(ConfigurationSetData.DeviceID.Trim()))
                        {
                            bool isLegal = CheckDeviceID(ConfigurationSetData.DeviceID);
                            if (!isLegal)
                            {
                                MessageBox.Show("设备ID号不合法.设备ID号是以F开头,以数字和字母组合的长度为24个的字符串", "提示");
                                break;
                            }
                        }

                        /*
                        // 检验要下发的遥信点号是否正确
                        if (!checkDownTelesignalisationPoint())
                        {
                            break;
                        }
                         * */

                        string configFilePath = DownConfig(telesignalisationPointArryDown);
                        SaveUsedPointToDB();

                        if (configFilePath != null && configFilePath.Trim().Count() > 0)
                        {
                            FileServiceViewModel.SelectFile(configFilePath);
                            FileServiceViewModel.WriteFileAct(configFilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "警告");
                    }

                    break;

                // 复位进程
                case "ResetProcess":
                    Messenger.Default.Send<string>("CmdInitProcessData", "MasterCommand");
                    break;

                // 上载三遥配置
                case "Upload":
                    try
                    {
                        if (!CommunicationViewModel.IsLinkConnect())
                        {
                            return;
                        }
                        string fileName = "/sojo/ConfigurationSet.cfg";
                        GetFileFromDevice(fileName);
                        MainViewModel.waitUnbrokenFile.WaitOne();
                        if (MainViewModel.isParseUploadFile)
                        {
                            string filePath = @".\InteractiveFile\ConfigurationSet.cfg";
                            string errorUploadMsg = getStruct(filePath);
                            if (!UtilHelper.IsEmpty(errorUploadMsg))
                            {
                                MessageBox.Show(errorUploadMsg, "错误");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "警告");
                    }

                    break;

                // 配置本地
                case "LocalConfig":
                    try
                    {
                        SaveUsedPointToDB();
                        MessageBox.Show("操作成功", "成功");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        LogHelper.Error(typeof(ConfigViewModel), "配置本地异常 \n" + e.Message);
                    }

                    break;

                // 召唤终端所有点表
                case "CallTerminalPointTable":

                    if (!CommunicationViewModel.IsLinkConnect())
                    {
                        return;
                    }
                    PWView.ShowDialog();
                    if(IsPW == true)
                    {
                        if (configPW == TelecontrolPWViewModel.pwBox)
                        {
                            PWViewModelObject.ReadPassword("ConfigPW");
                            string terminalPointFileName = "/sojo/AllJsonCfg.json";
                            GetFileFromDevice(terminalPointFileName);
                            TelecontrolPWViewModel.pwBox = "";
                        }
                        else
                        {
                            TelecontrolPWViewModel.pwBox = "";
                            MessageBox.Show("输入的密码不正确,请联系技术部获取使用权限！", "提示");
                            IsPW = false;
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }

                    break;

                case "UpdateDatabase":

                    PWView.ShowDialog();
                    if(IsPW == true)
                    {
                        if (configPW == TelecontrolPWViewModel.pwBox)
                        {
                            PWViewModelObject.ReadPassword("ConfigPW");
                            string terminalPointFilePath = @".\InteractiveFile\AllJsonCfg.json";
                            try
                            {
                                // 判断文件是否存在
                                if (!File.Exists(terminalPointFilePath))
                                {
                                    MessageBox.Show("文件不存在，请召唤终端点表", "错误");
                                    return;
                                }

                                string json = System.IO.File.ReadAllText(terminalPointFilePath, Encoding.GetEncoding("GB2312"));
                                // 将Json格式的字符串转换成对应的数据对象
                                PointNumberCollection pointNumberCollection = EncryptAndDecodeUtil.JsonToObject<PointNumberCollection>(json);
                                AnalysisPointNumberCollection(pointNumberCollection);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                MessageBox.Show("文件内容有误 \n" + ex.Message, "异常");
                            }
                            TelecontrolPWViewModel.pwBox = "";
                        }
                        else
                        {
                            TelecontrolPWViewModel.pwBox = "";
                            MessageBox.Show("输入的密码不正确,请联系技术部获取使用权限！", "提示");
                            IsPW = false;
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }

                    break;
            }

        }

        /// <summary>
        /// 解析终端上传点表的json格式数据
        /// </summary>
        /// <param name="pointNumberCollection"></param>
        private void AnalysisPointNumberCollection(PointNumberCollection pointNumberCollection)
        {
            // 判断点号集合是否为空
            if (pointNumberCollection == null)
            {
                return;
            }

            // 解析终端上传点表的json格式数据并更新至数据库中
            ManagePointTableService ManagePointTableService = new ManagePointTableService();
            ManagePointTableService.AnalysisPointNumberCollectionAndInsertIntoDatabase(pointNumberCollection);

            // 更新相应页面的点表
            UpdateDisplayedPoint();

            MessageBox.Show("导入成功", "成功");

        }

        /// <summary>
        /// 更新相应页面的点表
        /// </summary>
        public static void UpdateDisplayedPoint()
        {
            // 重新获取空点号集合
            ConfigUtil.GetNullDevPointList();

            #region 更新相应页面的点表

            // 重新载入遥测点表、遥信点表、遥控点表
            LoadPointByPointTypeFlag(true, true, true);

            // 发送更新相应页面的点表消息，包括:组合遥信页面CombineTelesignalisationManagementViewModel
            // 三遥相应页面:遥测页面TelemeteringViewModel、遥信页面TelesignalisationViewModel、遥控页面TelecontrolViewModel
            // 定值参数页面ParameterViewModel、固有定值参数页面InherentParameterViewModel、系数校准页面CoefficientViewModel
            Messenger.Default.Send<object>(null, "UpdateSourcePoint");

            #endregion 并更新相应页面的点表
        }

        /// <summary>
        /// 拼接两个字节数组到一个大的字节数组中
        /// </summary>
        /// <param name="bBig">第一个字节数组</param>
        /// <param name="bSmall">第二个字节数组</param>
        /// <returns></returns>
        private byte[] CopyToBig(byte[] bBig, byte[] bSmall)
        {
            byte[] tmp = new byte[bBig.Length + bSmall.Length];
            System.Buffer.BlockCopy(bBig, 0, tmp, 0, bBig.Length);
            System.Buffer.BlockCopy(bSmall, 0, tmp, bBig.Length, bSmall.Length);
            return tmp;
        }

        /// <summary>
        /// 点表配置指令
        /// </summary>
        public RelayCommand<string> ConfigCommand { get; private set; }

        /// <summary>
        /// 点表配置指令执行操作
        /// </summary>
        /// <param name="arg">字符串参数，表示哪一个操作</param>
        private void ExecuteConfigCommand(string arg)
        {
            switch (arg)
            {
                // 遥信原始点表点号右移
                case "YXOKCommand":

                    if (GridIndex[0] != -1)
                    {
                        TelesignalisationPoint.Add(TelesignalisationPointModel[GridIndex[0]]);
                        TelesignalisationPointModel.RemoveAt(GridIndex[0]);
                    }

                    break;

                // 选择配置的遥信点表点号上移
                case "YXUpCommand":

                    if (GridIndex[1] != -1)
                    {
                        if (TelesignalisationPoint != null && TelesignalisationPoint.Count() > 0)
                        {
                            // 获取选中上移遥信点号的索引
                            int startTelesignalisationPointUpIndex = GridIndex[1];
                            int index = TelesignalisationPoint.IndexOf(TelesignalisationPoint[GridIndex[1]]);
                            if (index > 0)
                            {
                                Telesignalisation telesignalisation = TelesignalisationPoint[index];
                                TelesignalisationPoint.RemoveAt(index);
                                TelesignalisationPoint.Insert(index - 1, telesignalisation);
                                GridIndex[1] = startTelesignalisationPointUpIndex - 1;
                            }
                        }
                    }

                    break;

                // 选择配置的遥信点表点号后面添加空行
                case "YXBlankCommand":

                    if (GridIndex[1] != -1)
                    {
                        if (TelesignalisationPoint != null && TelesignalisationPoint.Count() > 0)
                        {
                            int index = TelesignalisationPoint.IndexOf(TelesignalisationPoint[GridIndex[1]]);
                            int devpid = ConfigUtil.getDevPointID("遥信");

                            if (devpid != -1)
                            {
                                Telesignalisation telesignalisation = new Telesignalisation();
                                telesignalisation.Number = 0;
                                telesignalisation.Devpid = devpid;
                                telesignalisation.ID = 0;
                                telesignalisation.Name = "0";

                                TelesignalisationPoint.Insert(index + 1, telesignalisation);
                            }

                        }
                    }

                    break;

                // 选择配置的遥信点表点号下移
                case "YXDownCommand":

                    if (GridIndex[1] != -1)
                    {
                        if (TelesignalisationPoint != null && TelesignalisationPoint.Count() > 0)
                        {
                            // 获取选中下移遥信点号的索引
                            int startTelesignalisationPointDownIndex = GridIndex[1];
                            int index = TelesignalisationPoint.IndexOf(TelesignalisationPoint[GridIndex[1]]);
                            if (index < TelesignalisationPoint.Count - 1)
                            {
                                Telesignalisation telesignalisation = TelesignalisationPoint[index];
                                TelesignalisationPoint.RemoveAt(index);
                                TelesignalisationPoint.Insert(index + 1, telesignalisation);
                                GridIndex[1] = startTelesignalisationPointDownIndex + 1;
                            }
                        }
                    }

                    break;

                // 选择配置的遥信点表点号左移
                case "YXCancelCommand":

                    if (GridIndex[1] != -1)
                    {
                        if (TelesignalisationPoint[GridIndex[1]].ID == 0)
                        {
                            TelesignalisationPoint.RemoveAt(GridIndex[1]);
                            break;
                        }

                        if (TelesignalisationPointModel.Count() == 0 || (TelesignalisationPointModel[TelesignalisationPointModel.Count() - 1].Number < TelesignalisationPoint[GridIndex[1]].Number))
                        {
                            TelesignalisationPointModel.Add(TelesignalisationPoint[GridIndex[1]]);
                        }
                        else
                        {
                            for (int i = 0; i <= TelesignalisationPointModel.Count(); i++)
                            {
                                if (TelesignalisationPointModel[i].Number > TelesignalisationPoint[GridIndex[1]].Number)
                                {
                                    TelesignalisationPointModel.Insert(i, TelesignalisationPoint[GridIndex[1]]);
                                    break;
                                }
                            }
                        }

                        TelesignalisationPoint.RemoveAt(GridIndex[1]);
                    }

                    break;

                // 遥测原始点表点号右移
                case "YCOKCommand":

                    if (GridIndex[2] != -1)
                    {
                        TelemeteringPoint.Add(TelemeteringPointModel[GridIndex[2]]);
                        TelemeteringPointModel.RemoveAt(GridIndex[2]);
                    }

                    break;

                // 选择配置的遥测点表点号上移
                case "YCUpCommand":

                    if (GridIndex[3] != -1)
                    {
                        if (TelemeteringPoint != null && TelemeteringPoint.Count() > 0)
                        {
                            // 获取选中上移遥测点号的索引
                            int startTelemeteringPointUpIndex = GridIndex[3];
                            int index = TelemeteringPoint.IndexOf(TelemeteringPoint[GridIndex[3]]);
                            if (index > 0)
                            {
                                Telemetering telemetering = TelemeteringPoint[index];
                                TelemeteringPoint.RemoveAt(index);
                                TelemeteringPoint.Insert(index - 1, telemetering);
                                GridIndex[3] = startTelemeteringPointUpIndex - 1;
                            }
                        }
                    }

                    break;

                // 选择配置的遥测点表点号后面添加空行
                case "YCBlankCommand":

                    if (GridIndex[3] != -1)
                    {
                        if (TelemeteringPoint != null && TelemeteringPoint.Count() > 0)
                        {
                            int index = TelemeteringPoint.IndexOf(TelemeteringPoint[GridIndex[3]]);
                            int devpid = ConfigUtil.getDevPointID("遥测");

                            if (devpid != -1)
                            {
                                Telemetering telemetering = new Telemetering();
                                telemetering.Number = 0;
                                telemetering.Devpid = devpid;
                                telemetering.ID = 0;
                                telemetering.Name = "0";

                                TelemeteringPoint.Insert(index + 1, telemetering);
                            }

                        }
                    }

                    break;

                // 选择配置的遥测点表点号下移
                case "YCDownCommand":

                    if (GridIndex[3] != -1)
                    {
                        if (TelemeteringPoint != null && TelemeteringPoint.Count() > 0)
                        {
                            // 获取选中下移遥测点号的索引
                            int startTelemeteringPointDownIndex = GridIndex[3];
                            int index = TelemeteringPoint.IndexOf(TelemeteringPoint[GridIndex[3]]);
                            if (index < TelemeteringPoint.Count - 1)
                            {
                                Telemetering telemetering = TelemeteringPoint[index];
                                TelemeteringPoint.RemoveAt(index);
                                TelemeteringPoint.Insert(index + 1, telemetering);
                                GridIndex[3] = startTelemeteringPointDownIndex + 1;
                            }
                        }
                    }

                    break;

                // 选择配置的遥测点表点号左移
                case "YCCancelCommand":

                    if (GridIndex[3] != -1)
                    {
                        if (TelemeteringPoint[GridIndex[3]].ID == 0)
                        {
                            TelemeteringPoint.RemoveAt(GridIndex[3]);
                            break;
                        }

                        if (TelemeteringPointModel.Count() == 0 || (TelemeteringPointModel[TelemeteringPointModel.Count() - 1].Number < TelemeteringPoint[GridIndex[3]].Number))
                        {
                            TelemeteringPointModel.Add(TelemeteringPoint[GridIndex[3]]);
                        }
                        else
                        {
                            for (int i = 0; i < TelemeteringPointModel.Count(); i++)
                            {
                                if (TelemeteringPointModel[i].Number > TelemeteringPoint[GridIndex[3]].Number)
                                {
                                    TelemeteringPointModel.Insert(i, TelemeteringPoint[GridIndex[3]]);
                                    break;
                                }
                            }
                        }

                        TelemeteringPoint.RemoveAt(GridIndex[3]);
                    }

                    break;

                // 遥控原始点表点号右移
                case "YKOKCommand":

                    if (GridIndex[4] != -1)
                    {
                        TelecontrolPoint.Add(TelecontrolPointModel[GridIndex[4]]);
                        TelecontrolPointModel.RemoveAt(GridIndex[4]);
                    }

                    break;

                // 选择配置的遥控点表点号上移
                case "YKUpCommand":

                    if (GridIndex[5] != -1)
                    {
                        if (TelecontrolPoint != null && TelecontrolPoint.Count() > 0)
                        {
                            // 获取选中上移遥控点号的索引
                            int startTelecontrolPointUpIndex = GridIndex[5];
                            int index = TelecontrolPoint.IndexOf(TelecontrolPoint[GridIndex[5]]);
                            if (index > 0)
                            {
                                Telecontrol telecontrol = TelecontrolPoint[index];
                                TelecontrolPoint.RemoveAt(index);
                                TelecontrolPoint.Insert(index - 1, telecontrol);
                                GridIndex[5] = startTelecontrolPointUpIndex - 1;
                            }
                        }
                    }

                    break;

                // 选择配置的遥控点表点号后面添加空行
                case "YKBlankCommand":

                    if (GridIndex[5] != -1)
                    {
                        if (TelecontrolPoint != null && TelecontrolPoint.Count() > 0)
                        {
                            int index = TelecontrolPoint.IndexOf(TelecontrolPoint[GridIndex[5]]);
                            int devpid = ConfigUtil.getDevPointID("遥控");

                            if (devpid != -1)
                            {
                                Telecontrol telecontrol = new Telecontrol();
                                telecontrol.Number = 0;
                                telecontrol.Devpid = devpid;
                                telecontrol.YKID = 0;
                                telecontrol.YKName = "0";

                                TelecontrolPoint.Insert(index + 1, telecontrol);
                            }

                        }
                    }

                    break;

                // 选择配置的遥控点表点号下移
                case "YKDownCommand":

                    if (GridIndex[5] != -1)
                    {
                        if (TelecontrolPoint != null && TelecontrolPoint.Count() > 0)
                        {
                            // 获取选中下移遥控点号的索引
                            int startTelecontrolPointDownIndex = GridIndex[5];
                            int index = TelecontrolPoint.IndexOf(TelecontrolPoint[GridIndex[5]]);
                            if (index < TelecontrolPoint.Count - 1)
                            {
                                Telecontrol telecontrol = TelecontrolPoint[index];
                                TelecontrolPoint.RemoveAt(index);
                                TelecontrolPoint.Insert(index + 1, telecontrol);
                                GridIndex[5] = startTelecontrolPointDownIndex + 1;
                            }
                        }
                    }

                    break;

                // 选择配置的遥控点表点号左移
                case "YKCancelCommand":

                    if (GridIndex[5] != -1)
                    {
                        if (TelecontrolPoint[GridIndex[5]].YKID == 0)
                        {
                            TelecontrolPoint.RemoveAt(GridIndex[5]);
                            break;
                        }

                        if (TelecontrolPointModel.Count() == 0 || (TelecontrolPointModel[TelecontrolPointModel.Count() - 1].Number < TelecontrolPoint[GridIndex[5]].Number))
                        {
                            TelecontrolPointModel.Add(TelecontrolPoint[GridIndex[5]]);
                        }
                        else
                        {
                            for (int i = 0; i < TelecontrolPointModel.Count(); i++)
                            {
                                if (TelecontrolPointModel[i].Number > TelecontrolPoint[GridIndex[5]].Number)
                                {
                                    TelecontrolPointModel.Insert(i, TelecontrolPoint[GridIndex[5]]);
                                    break;
                                }
                            }
                        }

                        TelecontrolPoint.RemoveAt(GridIndex[5]);
                    }

                    break;

            }
        }

        /// <summary>
        /// 选中全部点表配置指令
        /// </summary>
        public RelayCommand AllSelectedCommand { get; private set; }

        /// <summary>
        /// 选中全部点表配置指令执行操作
        /// </summary>
        private void ExecuteAllSelectedCommand()
        {
            for (int i = 0; i < TelesignalisationPointModel.Count; i++)
            {
                TelesignalisationPoint.Add(TelesignalisationPointModel[i]);
            }

            for (int i = 0; i < TelemeteringPointModel.Count; i++)
            {
                TelemeteringPoint.Add(TelemeteringPointModel[i]);
            }

            for (int i = 0; i < TelecontrolPointModel.Count; i++)
            {
                TelecontrolPoint.Add(TelecontrolPointModel[i]);
            }

            TelesignalisationPointModel.Clear();
            TelemeteringPointModel.Clear();
            TelecontrolPointModel.Clear();

        }

        /// <summary>
        /// 恢复点表默认配置指令（不选中任何点号）
        /// </summary>
        public RelayCommand RecoveryCommand { get; private set; }

        /// <summary>
        /// 恢复点表默认配置指令（不选中任何点号）执行操作
        /// </summary>
        private void ExecuteRecoveryCommand()
        {
            TelemeteringPoint.Clear();
            TelesignalisationPoint.Clear();
            TelecontrolPoint.Clear();

            // 重新加载遥测、遥信、遥控点表
            LoadPointByPointTypeFlag(true, true, true);

        }

        /*
        /// <summary>
        /// 加载原始点表
        /// </summary>
        public static void load()
        {
            DevPointDao devPointDao = new DevPointDao();

            #region 加载遥测原始点表

            telemeteringPointModel.Clear();

            IList<DevPoint> telemeteringPointList = devPointDao.queryByPointTypeId(ConfigUtil.getPointTypeID("遥测"));

            if (telemeteringPointList != null && telemeteringPointList.Count > 0)
            {
                telemeteringPointModelIDIndex = new List<int>();

                for (int i = 0; i < telemeteringPointList.Count; i++)
                {
                    Telemetering data = new Telemetering();
                    data.Selected = false;
                    data.Number = i + 1;
                    data.Devpid = telemeteringPointList[i].Devpid;
                    data.Name = telemeteringPointList[i].Name;
                    // data.ID = i + 16385;
                    data.ID = Convert.ToInt32(telemeteringPointList[i].ID, 16);
                    data.Unit = telemeteringPointList[i].Unit;
                    data.Comment = telemeteringPointList[i].Comment;
                    data.DataType = 2;
                    // data.Rate = (float)(Convert.ToDouble(telemeteringPointList[i].Rate));
                    data.Rate = 1;

                    telemeteringPointModelIDIndex.Add(data.ID);

                    telemeteringPointModel.Add(data);
                }
            }

            #endregion

            #region 加载遥信原始点表

            telesignalisationPointModel.Clear();

            IList<DevPoint> telesignalisationPointList = devPointDao.queryByPointTypeId(ConfigUtil.getPointTypeID("遥信"));

            if (telesignalisationPointList != null && telesignalisationPointList.Count > 0)
            {
                telesignalisationPointModelIDIndex = new List<int>();

                for (int i = 0; i < telesignalisationPointList.Count; i++)
                {
                    Telesignalisation data = new Telesignalisation();
                    data.Selected = false;
                    data.Number = i + 1;
                    data.Devpid = telesignalisationPointList[i].Devpid;
                    data.Name = telesignalisationPointList[i].Name;
                    //data.ID = i + 1;
                    data.ID = Convert.ToInt32(telesignalisationPointList[i].ID, 16);
                    data.Comment = telesignalisationPointList[i].Comment;
                    data.Flag = telesignalisationPointList[i].Flag;
                    data.IsSOE = true;

                    telesignalisationPointModelIDIndex.Add(data.ID);

                    telesignalisationPointModel.Add(data);
                }
            }
            

            #endregion

            #region 加载遥控原始点表

            telecontrolPointModel.Clear();

            IList<DevPoint> telecontrolPointPointList = devPointDao.queryByPointTypeId(ConfigUtil.getPointTypeID("遥控"));

            if (telecontrolPointPointList != null && telecontrolPointPointList.Count > 0)
            {
                telecontrolPointModelIDIndex = new List<int>();

                for (int i = 0; i < telecontrolPointPointList.Count; i++)
                {
                    Telecontrol data = new Telecontrol();
                    data.Selected = false;
                    data.Number = i + 1;
                    data.Devpid = telecontrolPointPointList[i].Devpid;
                    data.YKName = telecontrolPointPointList[i].Name;
                    //data.YKID = i + 24577;
                    data.YKID = Convert.ToInt32(telecontrolPointPointList[i].ID, 16);
                    data.YKRemark = telecontrolPointPointList[i].Comment;

                    telecontrolPointModelIDIndex.Add(data.YKID);

                    telecontrolPointModel.Add(data);
                }
            }

            #endregion        
            
        }
         * */

        /// <summary>
        /// 根据点号类型标记加载原始点表
        /// </summary>
        /// <param name="telemeteringFlag">是否重新加载遥测点号(true代表是,false代表否)</param>
        /// <param name="telesignalisationFlag">是否重新加载遥信点号(true代表是,false代表否)</param>
        /// <param name="telecontrolFlag">是否重新加载遥控点号(true代表是,false代表否)</param>
        public static void LoadPointByPointTypeFlag(bool telemeteringFlag, bool telesignalisationFlag, bool telecontrolFlag)
        {
            #region 加载遥测原始点表

            if (telemeteringFlag)
            {
                LoadTelemeteringPoint();

            }

            #endregion 加载遥测原始点表

            #region 加载遥信点表

            if (telesignalisationFlag)
            {
                LoadTelesignalisationPoint();
            }

            #endregion 加载遥信点表

            #region 加载遥控原始点表

            if (telecontrolFlag)
            {
                LoadTelecontrolPoint();
            }

            #endregion 加载遥控原始点表

        }

        /// <summary>
        /// 重新加载遥测点号
        /// </summary>
        public static void LoadTelemeteringPoint()
        {
            DevPointDao devPointDao = new DevPointDao();

            telemeteringPoint.Clear();
            telemeteringPointModel.Clear();

            IList<DevPoint> telemeteringPointList = devPointDao.queryByPointTypeId(ConfigUtil.getPointTypeID("遥测"));

            if (telemeteringPointList != null && telemeteringPointList.Count > 0)
            {
                telemeteringPointModelIDIndex = new List<int>();

                for (int i = 0; i < telemeteringPointList.Count; i++)
                {
                    Telemetering data = new Telemetering();
                    data.Selected = false;
                    data.Number = i + 1;
                    data.Devpid = telemeteringPointList[i].Devpid;
                    data.Name = telemeteringPointList[i].Name;
                    // data.ID = i + 16385;
                    data.ID = Convert.ToInt32(telemeteringPointList[i].ID, 16);
                    data.Unit = telemeteringPointList[i].Unit;
                    data.Comment = telemeteringPointList[i].Comment;
                    data.DataType = 2;
                    data.Rate = (float)(Convert.ToDouble(telemeteringPointList[i].Rate));
                    //data.Rate = 1;屏蔽该行代码的原因是为了能使倍率可设

                    telemeteringPointModelIDIndex.Add(data.ID);

                    telemeteringPointModel.Add(data);
                }
            }

        }

        /// <summary>
        /// 重新加载遥信点号
        /// </summary>
        public static void LoadTelesignalisationPoint()
        {
            DevPointDao devPointDao = new DevPointDao();

            telesignalisationPoint.Clear();
            telesignalisationPointModel.Clear();

            IList<DevPoint> telesignalisationPointList = devPointDao.queryByPointTypeId(ConfigUtil.getPointTypeID("遥信"));

            if (telesignalisationPointList != null && telesignalisationPointList.Count > 0)
            {
                telesignalisationPointModelIDIndex = new List<int>();

                for (int i = 0; i < telesignalisationPointList.Count; i++)
                {
                    Telesignalisation data = new Telesignalisation();
                    data.Selected = false;
                    data.Number = i + 1;
                    data.Devpid = telesignalisationPointList[i].Devpid;
                    data.Name = telesignalisationPointList[i].Name;
                    //data.ID = i + 1;
                    data.ID = Convert.ToInt32(telesignalisationPointList[i].ID, 16);
                    data.Comment = telesignalisationPointList[i].Comment;
                    data.Flag = telesignalisationPointList[i].Flag;
                    data.IsSOE = true;
                    data.IsChanged = true;

                    telesignalisationPointModelIDIndex.Add(data.ID);

                    telesignalisationPointModel.Add(data);
                }
            }

        }

        /// <summary>
        /// 重新加载遥控点号
        /// </summary>
        public static void LoadTelecontrolPoint()
        {
            DevPointDao devPointDao = new DevPointDao();

            telecontrolPoint.Clear();
            telecontrolPointModel.Clear();

            IList<DevPoint> telecontrolPointPointList = devPointDao.queryByPointTypeId(ConfigUtil.getPointTypeID("遥控"));

            if (telecontrolPointPointList != null && telecontrolPointPointList.Count > 0)
            {
                telecontrolPointModelIDIndex = new List<int>();

                for (int i = 0; i < telecontrolPointPointList.Count; i++)
                {
                    Telecontrol data = new Telecontrol();
                    data.Selected = false;
                    data.Number = i + 1;
                    data.Devpid = telecontrolPointPointList[i].Devpid;
                    data.YKName = telecontrolPointPointList[i].Name;
                    //data.YKID = i + 24577;
                    data.YKID = Convert.ToInt32(telecontrolPointPointList[i].ID, 16);
                    data.YKRemark = telecontrolPointPointList[i].Comment;

                    telecontrolPointModelIDIndex.Add(data.YKID);

                    telecontrolPointModel.Add(data);
                }
            }

        }

        #endregion 方法定义


        /// <summary>
        /// 无参构造方法
        /// </summary>
        public ConfigViewModel()
        {
            // 注册接收更新遥信点表消息(组合遥信管理页面新增或删除组合遥信点表后，需要重新加载配置参数遥信点表)
            Messenger.Default.Register<Telesignalisation>(this, "UpdateTelesignalisationPoint", ReloadTelesignalisationPoint);

            #region 模板点表

            this._DTUConfigurePointTableAllNodes = new List<DTUNode>();
            DTUConfigurePointTableDaoObject = new DTUConfigurePointTableDao();
            Messenger.Default.Register<int>(this, "GetCurrentModuleConfigure", ExcuteGetCurrentModuleConfigure);
            ConfigModuleCommand = new RelayCommand<string>(ExcuteConfigModuleCommand);

            #endregion 模板点表

            #region 点表

            telesignalisationPointModel = new ObservableCollection<Telesignalisation>();
            telesignalisationPoint = new ObservableCollection<Telesignalisation>();
            telemeteringPointModel = new ObservableCollection<Telemetering>();
            telemeteringPoint = new ObservableCollection<Telemetering>();
            telecontrolPointModel = new ObservableCollection<Telecontrol>();
            telecontrolPoint = new ObservableCollection<Telecontrol>();

            // 重新加载遥测、遥信、遥控点表
            LoadPointByPointTypeFlag(true, true, true);

            #endregion 点表


            #region 属性
            // 配置数据Model
            configurationSetData = new ConfigurationSet();

            /*
            // 使用串口
            configurationSetData.UartPortItems = new List<string>();
            configurationSetData.UartPortItems.Add("左");
            configurationSetData.UartPortItems.Add("中");
            configurationSetData.UartPortIndex = 0;

            // 波特率
            configurationSetData.UartBaudRateItems = new List<string>();
            configurationSetData.UartBaudRateItems.Add("1200");
            configurationSetData.UartBaudRateItems.Add("2400");
            configurationSetData.UartBaudRateItems.Add("4800");
            configurationSetData.UartBaudRateItems.Add("9600");
            configurationSetData.UartBaudRateItems.Add("38400");
            configurationSetData.UartBaudRateItems.Add("115200");
            configurationSetData.UartBaudRateIndex = 3;

            // 数据位
            configurationSetData.UartWordLengthItems = new List<UInt16>();
            configurationSetData.UartWordLengthItems.Add(8);
            configurationSetData.UartWordLengthItems.Add(9);
            configurationSetData.UartWordLengthIndex = 0;

            // 停止位
            configurationSetData.UartStopBitsItems = new List<UInt16>();
            configurationSetData.UartStopBitsItems.Add(1);
            configurationSetData.UartStopBitsItems.Add(2);
            configurationSetData.UartStopBitsIndex = 0;

            // 校验位
            configurationSetData.UartParityItems = new List<string>();
            configurationSetData.UartParityItems.Add("无校验");
            configurationSetData.UartParityItems.Add("奇校验");
            configurationSetData.UartParityItems.Add("偶校验");
            configurationSetData.UartParityIndex = 0;

            // 模式
            configurationSetData.UartBalanModeItems = new List<string>();
            configurationSetData.UartBalanModeItems.Add("非平衡");
            configurationSetData.UartBalanModeItems.Add("平衡");
            configurationSetData.UartBalanModeIndex = 0;

            // 从站地址长度
            configurationSetData.UartLinkAddrSizeItems = new List<UInt16>();
            configurationSetData.UartLinkAddrSizeItems.Add(1);
            configurationSetData.UartLinkAddrSizeItems.Add(2);
            configurationSetData.UartLinkAddrSizeIndex = 1;

            // 传送原因长度
            configurationSetData.UartASDUCotSizeItems = new List<UInt16>();
            configurationSetData.UartASDUCotSizeItems.Add(1);
            configurationSetData.UartASDUCotSizeItems.Add(2);
            configurationSetData.UartASDUCotSizeIndex = 1;

            // ASDU地址长度
            configurationSetData.UartASDUAddrSizeItems = new List<UInt16>();
            configurationSetData.UartASDUAddrSizeItems.Add(1);
            configurationSetData.UartASDUAddrSizeItems.Add(2);
            configurationSetData.UartASDUAddrSizeIndex = 1;

            // 串口从站地址
            configurationSetData.UartSourceAddr = 1;

            // 串口ASDU地址
            configurationSetData.UartASDUAddr = 1;

            // 网口从站地址
            configurationSetData.NetSourceAddr = 1;

            // 网口ASDU地址
            configurationSetData.NetASDUAddr = 1;

            // IP地址1
            configurationSetData.NetIPOne = new UInt16[4];
            configurationSetData.NetIPOne[0] = 192;
            configurationSetData.NetIPOne[1] = 168;
            configurationSetData.NetIPOne[2] = 60;
            configurationSetData.NetIPOne[3] = 100;

            // IP地址2
            configurationSetData.NetIPTwo = new UInt16[4];
            configurationSetData.NetIPTwo[0] = 192;
            configurationSetData.NetIPTwo[1] = 168;
            configurationSetData.NetIPTwo[2] = 60;
            configurationSetData.NetIPTwo[3] = 101;

            // 子网掩码
            configurationSetData.NetNetmask = new UInt16[4];
            configurationSetData.NetNetmask[0] = 255;
            configurationSetData.NetNetmask[1] = 255;
            configurationSetData.NetNetmask[2] = 255;
            configurationSetData.NetNetmask[3] = 0;

            // 网关
            configurationSetData.NetGateway = new UInt16[4];
            configurationSetData.NetGateway[0] = 192;
            configurationSetData.NetGateway[1] = 168;
            configurationSetData.NetGateway[2] = 60;
            configurationSetData.NetGateway[3] = 254;

            // DNS
            configurationSetData.NetDNS = new UInt16[4];
            configurationSetData.NetDNS[0] = 1;
            configurationSetData.NetDNS[1] = 1;
            configurationSetData.NetDNS[2] = 1;
            configurationSetData.NetDNS[3] = 1;
            
            */

            // 数据表格选中索引数组
            this._gridIndex = new int[6];
            for (int i = 0; i < 6; i++)
            {
                this._gridIndex[i] = -1;
            }

            // 结构体
            //this.configurationSet.NetIPOne = new UInt16[4];
            //this.configurationSet.NetIPTwo = new UInt16[4];
            //this.configurationSet.NetNetmask = new UInt16[4];
            //this.configurationSet.NetGateway = new UInt16[4];
            //this.configurationSet.NetDNS = new UInt16[4];
            this.configurationSet.YXAddrVariable = new UInt16[YXNumber]; //300
            this.configurationSet.YCAddr = new UInt16[YCNumber];//300
            this.configurationSet.YCOtherFlag = new UInt16[YCNumber];//300
            this.configurationSet.YCRate = new float[YCNumber];//300
            this.configurationSet.YKAddr = new UInt16[YKNumber];//16
            this.configurationSet.YKOtherFlag = new UInt16[YKNumber];//16
            this.configurationSet.DeviceID = new byte[DeviceIDNumber];//16

            #endregion 属性


            #region Command执行方法

            ConfigReportCommand = new RelayCommand<string>(ExecuteConfigReportCommand);
            ConfigCommand = new RelayCommand<string>(ExecuteConfigCommand);
            AllSelectedCommand = new RelayCommand(ExecuteAllSelectedCommand);
            RecoveryCommand = new RelayCommand(ExecuteRecoveryCommand);

            #endregion Command执行方法
        }

        /// <summary>
        /// 重新载入遥信相关点表
        /// </summary>
        /// <param name="telesignalisation">遥信点号</param>
        public void ReloadTelesignalisationPoint(Telesignalisation telesignalisation)
        {
            if (telesignalisation != null)
            {
                telesignalisationPointModel.Add(telesignalisation);
                return;
            }

            // 重新加载遥信点表
            LoadTelesignalisationPoint();
        }

        /// <summary>
        /// 从本地读配置文件
        /// </summary>
        /// <returns>错误提示信息</returns>
        private string ReadConfigFromLocal()
        {
            try
            {
                // 创建一个通用对话框对象，用户可以使用此对话框来指定一个或多个要打开的文件的文件名
                OpenFileDialog openFileDialog = new OpenFileDialog();
                // 获取或设置筛选器字符串
                openFileDialog.Filter = "config files (*.cfg)|*.cfg|data files (*.txt)|*.txt|All files (*.*)|*.*";

                if ((bool)(openFileDialog.ShowDialog()))
                {
                    return getStruct(openFileDialog.FileName);
                }
                return "";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.Error(typeof(ConfigViewModel), "从本地读配置文件失败 \n" + ex.Message);
                return "从本地读配置文件失败: " + ex.Message;
            }

        }

        /// <summary>
        /// 获取配置文件数据字节
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        private string getStruct(string filePath)
        {
            try
            {
                // 得到结构体的大小
                int size = Marshal.SizeOf(configurationSet);

                string configFilePath = filePath;
                System.IO.FileStream fs = new System.IO.FileStream(configFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
                byte[] allBytes = new byte[size];
                fs.Read(allBytes, 0, size);
                fs.Close();
                fs.Dispose();
                // byte[] allBytes = System.IO.File.ReadAllBytes(configFilePath);

                // byte数组长度小于结构体的大小
                if (size != allBytes.Length)
                {
                    //返回
                    return "配置文件内容有误，解析失败";
                }

                //分配结构体大小的内存空间
                IntPtr structPtr = Marshal.AllocHGlobal(size);
                //将byte数组拷到分配好的内存空间
                Marshal.Copy(allBytes, 0, structPtr, size);
                //将内存空间转换为目标结构体
                configurationSet = (_ConfigurationSet)Marshal.PtrToStructure(structPtr, typeof(_ConfigurationSet));
                //释放内存空间
                Marshal.FreeHGlobal(structPtr);

                // 结构体数据解析为显示的数据
                return AnalysisStruct();
            }
            catch (Exception e)
            {
                Console.WriteLine("获取配置文件数据字节失败:" + e.Message);
                LogHelper.Error(typeof(ConfigViewModel), "获取配置文件数据字节失败 \n" + e.Message);
                return "获取配置文件数据字节失败:" + e.Message;
            }
        }

        /// <summary>
        /// 结构体数据解析为显示的数据
        /// </summary>
        /// <returns>错误提示信息</returns>
        private string AnalysisStruct()
        {
            /*
            ConfigurationSetData.UartPortIndex = configurationSet.UartPort - 1;
            ConfigurationSetData.UartBaudRateIndex = configurationSet.UartBaudRate - 1;
            ConfigurationSetData.UartWordLengthIndex = ConfigurationSetData.UartWordLengthItems.IndexOf(configurationSet.UartWordLength);
            ConfigurationSetData.UartStopBitsIndex = ConfigurationSetData.UartStopBitsItems.IndexOf(configurationSet.UartStopBits);
            ConfigurationSetData.UartParityIndex = configurationSet.UartParity - 1;
            ConfigurationSetData.UartBalanModeIndex = configurationSet.UartBalanMode;
            ConfigurationSetData.UartSourceAddr = configurationSet.UartSourceAddr;
            ConfigurationSetData.UartLinkAddrSizeIndex = ConfigurationSetData.UartLinkAddrSizeItems.IndexOf(configurationSet.UartLinkAddrSize);
            ConfigurationSetData.UartASDUCotSizeIndex = ConfigurationSetData.UartASDUCotSizeItems.IndexOf(configurationSet.UartASDUCotSize);
            ConfigurationSetData.UartASDUAddr = configurationSet.UartASDUAddr;
            ConfigurationSetData.UartASDUAddrSizeIndex = ConfigurationSetData.UartASDUAddrSizeItems.IndexOf(configurationSet.UartASDUAddrSize);

            ConfigurationSetData.NetIPOne = configurationSet.NetIPOne;
            ConfigurationSetData.NetIPTwo = configurationSet.NetIPTwo;
            ConfigurationSetData.NetNetmask = configurationSet.NetNetmask;
            ConfigurationSetData.NetGateway = configurationSet.NetGateway;
            ConfigurationSetData.NetDNS = configurationSet.NetDNS;

            ConfigurationSetData.NetSourceAddr = configurationSet.NetSourceAddr;
            ConfigurationSetData.NetASDUAddr = configurationSet.NetASDUAddr;
            */

            // 恢复默认点表设置
            ExecuteRecoveryCommand();

            #region 解析要下发的遥信点号配置内容(默认长度是300，不到300，则后续点号设置为0)

            // 获取返回的不存在的遥信点号集合的集合
            IList<IList<string>> notExistsTelesignalisation = analysisTelesignalisationPointArry();
            // 不存在的遥信点号错误提示信息
            string notExistsTelesignalisationMsg = "";
            // 遍历不存在的遥信点号集合的集合
            if (notExistsTelesignalisation != null && notExistsTelesignalisation.Count == 2)
            {
                // 设置不存在的遥信点号错误提示信息
                if (notExistsTelesignalisation[0] != null && notExistsTelesignalisation[0].Count > 0)
                {
                    notExistsTelesignalisationMsg += "原始遥信点号:";
                    foreach (var s in notExistsTelesignalisation[0])
                    {
                        notExistsTelesignalisationMsg += s.ToUpper() + ",";
                    }
                    notExistsTelesignalisationMsg = notExistsTelesignalisationMsg.Substring(0, notExistsTelesignalisationMsg.Length - 1);
                    notExistsTelesignalisationMsg += "不存在，请添加！" + "\n";
                }
                //for (int i = 0; i < 2; i++)
                //{
                //    // 设置不存在的遥信点号错误提示信息
                //    if (notExistsTelesignalisation[i] != null && notExistsTelesignalisation[i].Count > 0)
                //    {
                //        notExistsTelesignalisationMsg += (i == 0 ? "原始遥信点号:" : "组合遥信点号内容:");
                //        foreach (var s in notExistsTelesignalisation[i])
                //        {
                //            notExistsTelesignalisationMsg += s.ToUpper() + ",";
                //        }
                //        notExistsTelesignalisationMsg = notExistsTelesignalisationMsg.Substring(0, notExistsTelesignalisationMsg.Length - 1);
                //        notExistsTelesignalisationMsg += "不存在，请添加！" + "\n";
                //    }
                //}
            }

            #endregion 解析要下发的遥信点号配置内容(默认长度是300，不到300，则后续点号设置为0)

            #region 依次获取选中的要下发的遥测点号（默认个数是300，不到300，则后续点号设置为0）

            // 查找最后一个非零点表索引
            int notNullTelemeteringPointIndex = -1;
            for (int i = YCNumber - 1; i >= 0; i--)
            {
                if (configurationSet.YCAddr[i] != 0)
                {
                    notNullTelemeteringPointIndex = i;
                    break;
                }
            }

            // 空遥测点号编号
            int telemeteringPointDevpid = ConfigUtil.getDevPointID("遥测");

            // 不存在遥测点表
            List<int> notExistsTelemeteringPoint = new List<int>();
            // 不存在的遥测点号错误提示信息
            string notExistsTelemeteringPointMsg = "";

            for (int i = 0; i <= notNullTelemeteringPointIndex; i++)
            {
                // 判断空遥测点号
                if (configurationSet.YCAddr[i] == 0 && telemeteringPointDevpid != -1)
                {
                    Telemetering telemetering = new Telemetering();
                    telemetering.Number = 0;
                    telemetering.Devpid = telemeteringPointDevpid;
                    telemetering.ID = 0;
                    telemetering.Name = "0";

                    TelemeteringPoint.Add(telemetering);

                }

                // 非空点号
                if (configurationSet.YCAddr[i] != 0)
                {
                    int index = telemeteringPointModelIDIndex.IndexOf(configurationSet.YCAddr[i]);
                    if (index != -1)
                    {
                        Telemetering telemetering = TelemeteringPointModel[index];
                        UInt16 otherFlag = configurationSet.YCOtherFlag[i];
                        float rate = configurationSet.YCRate[i];

                        telemetering.DataType = otherFlag & 0x0003;
                        telemetering.Rate = rate;

                        TelemeteringPoint.Add(telemetering);
                        TelemeteringPointModel.RemoveAt(index);
                        telemeteringPointModelIDIndex.RemoveAt(index);
                    }
                    else
                    {
                        notExistsTelemeteringPoint.Add(configurationSet.YCAddr[i]);
                    }
                }

            }

            // 设置不存在的遥测点号错误提示信息
            if (notExistsTelemeteringPoint != null && notExistsTelemeteringPoint.Count > 0)
            {
                notExistsTelemeteringPointMsg += "原始遥测点号:";
                foreach (var s in notExistsTelemeteringPoint)
                {
                    notExistsTelemeteringPointMsg += s + ",";
                }
                notExistsTelemeteringPointMsg = notExistsTelemeteringPointMsg.Substring(0, notExistsTelemeteringPointMsg.Length - 1);
                notExistsTelemeteringPointMsg += "不存在，请添加！" + "\n";
            }

            #endregion 遥测点号

            #region 依次获取选中的要下发的遥控点号（默认个数是16，不到16，则后续点号设置为0）

            // 查找最后一个非零点表索引
            int notNullTelecontrolPointIndex = -1;
            for (int i = YKNumber - 1; i >= 0; i--)
            {
                if (configurationSet.YKAddr[i] != 0)
                {
                    notNullTelecontrolPointIndex = i;
                    break;
                }
            }

            // 空遥控点号编号
            int telecontrolPointDevpid = ConfigUtil.getDevPointID("遥控");

            // 不存在遥控点表
            List<int> notExistsTelecontrolPoint = new List<int>();
            // 不存在的遥控点号错误提示信息
            string notExistsTelecontrolPointMsg = "";

            for (int i = 0; i <= notNullTelecontrolPointIndex; i++)
            {
                // 判断空遥控点号
                if (configurationSet.YKAddr[i] == 0 && telecontrolPointDevpid != -1)
                {
                    Telecontrol telecontrol = new Telecontrol();
                    telecontrol.Number = 0;
                    telecontrol.Devpid = telecontrolPointDevpid;
                    telecontrol.YKID = 0;
                    telecontrol.YKName = "0";

                    TelecontrolPoint.Add(telecontrol);

                }

                // 非空点号
                if (configurationSet.YKAddr[i] != 0)
                {
                    int index = telecontrolPointModelIDIndex.IndexOf(configurationSet.YKAddr[i]);
                    if (index != -1)
                    {
                        Telecontrol telecontrol = TelecontrolPointModel[index];
                        UInt16 otherFlag = configurationSet.YKOtherFlag[i];
                        if ((otherFlag & 0x00C0) == 0)
                        {
                            telecontrol.IsNegated = false;
                        }
                        else
                        {
                            telecontrol.IsNegated = true;
                        }

                        TelecontrolPoint.Add(telecontrol);
                        TelecontrolPointModel.RemoveAt(index);
                        telecontrolPointModelIDIndex.RemoveAt(index);
                    }
                    else
                    {
                        notExistsTelecontrolPoint.Add(configurationSet.YKAddr[i]);
                    }
                }

            }

            // 设置不存在的遥控点号错误提示信息
            if (notExistsTelecontrolPoint != null && notExistsTelecontrolPoint.Count > 0)
            {
                notExistsTelecontrolPointMsg += "原始遥控点号:";
                foreach (var s in notExistsTelecontrolPoint)
                {
                    notExistsTelecontrolPointMsg += s + ",";
                }
                notExistsTelecontrolPointMsg = notExistsTelecontrolPointMsg.Substring(0, notExistsTelecontrolPointMsg.Length - 1);
                notExistsTelecontrolPointMsg += "不存在，请添加！" + "\n";
            }

            #endregion 遥控点号

            #region 设备ID号

            if (configurationSet.DeviceID != null && configurationSet.DeviceID.Length > 0)
            {
                bool flag = true;
                for (int i = 0; i < configurationSet.DeviceID.Length; i++)
                {
                    if (configurationSet.DeviceID[i] == 0)
                    {
                        continue;
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }

                if (!flag)
                {
                    ConfigurationSetData.DeviceID = new System.Text.ASCIIEncoding().GetString(configurationSet.DeviceID);
                }
                else
                {
                    ConfigurationSetData.DeviceID = "";
                }
            }

            #endregion 设备ID号

            return notExistsTelesignalisationMsg + notExistsTelemeteringPointMsg + notExistsTelecontrolPointMsg;

        }

        /// <summary>
        /// 下发配置到设备
        /// </summary>
        /// <param name="telesignalisationPointArry">遥信点表配置内容</param>
        /// <returns></returns>
        private string DownConfig(List<byte> telesignalisationPointArry)
        {
            try
            {
                //string primaryPath = @".\Config";
                string primaryName = "ConfigurationSet" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".cfg";
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Config (*.cfg)|*.cfg";

                //是否自动添加扩展名
                saveFileDialog.AddExtension = true;
                //文件已存在是否提示覆盖
                saveFileDialog.OverwritePrompt = true;
                //提示输入的文件名无效
                saveFileDialog.CheckPathExists = true;

                //保存对话框是否记忆上次打开的目录  
                //saveFileDialog.RestoreDirectory = true;

                // 获取或设置文件对话框显示的初始目录
                // saveFileDialog.InitialDirectory = primaryPath;

                // 文件初始名
                saveFileDialog.FileName = primaryName;

                if (saveFileDialog.ShowDialog() == true)
                {
                    byte[] AllBuff = ConfigDataToStructBytes(telesignalisationPointArry);

                    System.IO.File.WriteAllBytes(saveFileDialog.FileName, AllBuff);

                    MessageBox.Show("操作成功");

                    return saveFileDialog.FileName;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 从设备读取文件
        /// </summary>
        /// <param name="fileName">保存在设备中的文件全路径</param>
        private void GetFileFromDevice(string fileName)
        {
            byte[] temp = new byte[1024];
            byte[] buffer = null;
            int len = 0;

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
            temp[len++] = (byte)fileName.Length;
            for (int i = 0; i < fileName.Length; i++)
            {
                temp[len++] = (byte)fileName[i];
            }

            //  len = (byte)(5 + fileName.Length);
            buffer = new byte[len];
            for (int i = 0; i < len; i++)
            {
                buffer[i] = temp[i];
            }

            CommunicationViewModel.con.SendFileServiceCommand(CauseOfTransmission.ACTIVATION, 1, buffer);
        }

        /// <summary>
        /// 配置数据转换成结构体的字节数组
        /// </summary>
        /// <param name="telesignalisationPointArry">遥信点表配置内容</param>
        /// <returns></returns>
        private byte[] ConfigDataToStructBytes(List<byte> telesignalisationPointArry)
        {
            // 保存导出文件中内容的所有字节
            byte[] AllBuff;
            // 保存每一个配置参数的字节
            byte[] intBuff;

            //取数据
            /*
            configurationSet.UartPort = Convert.ToUInt16(ConfigurationSetData.UartPortItems[ConfigurationSetData.UartPortIndex] == "左" ? 1 : 2);
            // 以字节数组的形式返回指定的16位有符号整数值
            intBuff = BitConverter.GetBytes(configurationSet.UartPort);
            AllBuff = intBuff;

            switch (ConfigurationSetData.UartBaudRateItems[ConfigurationSetData.UartBaudRateIndex])
            {
                case "1200":
                    configurationSet.UartBaudRate = 1;
                    intBuff = BitConverter.GetBytes(configurationSet.UartBaudRate);
                    AllBuff = CopyToBig(AllBuff, intBuff);
                    break;
                case "2400":
                    configurationSet.UartBaudRate = 2;
                    intBuff = BitConverter.GetBytes(configurationSet.UartBaudRate);
                    AllBuff = CopyToBig(AllBuff, intBuff);
                    break;
                case "4800":
                    configurationSet.UartBaudRate = 3;
                    intBuff = BitConverter.GetBytes(configurationSet.UartBaudRate);
                    AllBuff = CopyToBig(AllBuff, intBuff);
                    break;
                case "9600":
                    configurationSet.UartBaudRate = 4;
                    intBuff = BitConverter.GetBytes(configurationSet.UartBaudRate);
                    AllBuff = CopyToBig(AllBuff, intBuff);
                    break;
                case "38400":
                    configurationSet.UartBaudRate = 5;
                    intBuff = BitConverter.GetBytes(configurationSet.UartBaudRate);
                    AllBuff = CopyToBig(AllBuff, intBuff);
                    break;
                case "115200":
                    configurationSet.UartBaudRate = 6;
                    intBuff = BitConverter.GetBytes(configurationSet.UartBaudRate);
                    AllBuff = CopyToBig(AllBuff, intBuff);
                    break;
            }

            configurationSet.UartWordLength = ConfigurationSetData.UartWordLengthItems[ConfigurationSetData.UartWordLengthIndex];
            intBuff = BitConverter.GetBytes(configurationSet.UartWordLength);
            AllBuff = CopyToBig(AllBuff, intBuff);

            configurationSet.UartStopBits = ConfigurationSetData.UartStopBitsItems[ConfigurationSetData.UartStopBitsIndex];
            intBuff = BitConverter.GetBytes(configurationSet.UartStopBits);
            AllBuff = CopyToBig(AllBuff, intBuff);

            configurationSet.UartParity = Convert.ToUInt16(ConfigurationSetData.UartParityItems[ConfigurationSetData.UartParityIndex] == "无校验" ? 1 : (ConfigurationSetData.UartParityItems[ConfigurationSetData.UartParityIndex] == "奇校验" ? 2 : 3));
            intBuff = BitConverter.GetBytes(configurationSet.UartParity);
            AllBuff = CopyToBig(AllBuff, intBuff);

            configurationSet.UartBalanMode = Convert.ToUInt16(ConfigurationSetData.UartBalanModeItems[ConfigurationSetData.UartBalanModeIndex] == "非平衡" ? 0 : 1);
            intBuff = BitConverter.GetBytes(configurationSet.UartBalanMode);
            AllBuff = CopyToBig(AllBuff, intBuff);

            configurationSet.UartSourceAddr = Convert.ToUInt16(ConfigurationSetData.UartSourceAddr);
            intBuff = BitConverter.GetBytes(configurationSet.UartSourceAddr);
            AllBuff = CopyToBig(AllBuff, intBuff);

            configurationSet.UartLinkAddrSize = ConfigurationSetData.UartLinkAddrSizeItems[ConfigurationSetData.UartLinkAddrSizeIndex];
            intBuff = BitConverter.GetBytes(configurationSet.UartLinkAddrSize);
            AllBuff = CopyToBig(AllBuff, intBuff);

            configurationSet.UartASDUCotSize = ConfigurationSetData.UartASDUCotSizeItems[ConfigurationSetData.UartASDUCotSizeIndex];
            intBuff = BitConverter.GetBytes(configurationSet.UartASDUCotSize);
            AllBuff = CopyToBig(AllBuff, intBuff);

            configurationSet.UartASDUAddr = Convert.ToUInt16(ConfigurationSetData.UartASDUAddr);
            intBuff = BitConverter.GetBytes(configurationSet.UartASDUAddr);
            AllBuff = CopyToBig(AllBuff, intBuff);

            configurationSet.UartASDUAddrSize = ConfigurationSetData.UartASDUAddrSizeItems[ConfigurationSetData.UartASDUAddrSizeIndex];
            intBuff = BitConverter.GetBytes(configurationSet.UartASDUAddrSize);
            AllBuff = CopyToBig(AllBuff, intBuff);

            // 获取IP地址1数据字节
            for (int i = 0; i < 4; i++)
            {
                configurationSet.NetIPOne[i] = Convert.ToUInt16(ConfigurationSetData.NetIPOne[i]);
                intBuff = BitConverter.GetBytes(configurationSet.NetIPOne[i]);
                AllBuff = CopyToBig(AllBuff, intBuff);
            }

            // 获取IP地址2数据字节
            for (int i = 0; i < 4; i++)
            {
                configurationSet.NetIPTwo[i] = Convert.ToUInt16(ConfigurationSetData.NetIPTwo[i]);
                intBuff = BitConverter.GetBytes(configurationSet.NetIPTwo[i]);
                AllBuff = CopyToBig(AllBuff, intBuff);
            }

            // 获取子网掩码数据字节
            for (int i = 0; i < 4; i++)
            {
                configurationSet.NetNetmask[i] = Convert.ToUInt16(ConfigurationSetData.NetNetmask[i]);
                intBuff = BitConverter.GetBytes(configurationSet.NetNetmask[i]);
                AllBuff = CopyToBig(AllBuff, intBuff);
            }

            // 获取网关数据字节
            for (int i = 0; i < 4; i++)
            {
                configurationSet.NetGateway[i] = Convert.ToUInt16(ConfigurationSetData.NetGateway[i]);
                intBuff = BitConverter.GetBytes(configurationSet.NetGateway[i]);
                AllBuff = CopyToBig(AllBuff, intBuff);
            }

            // 获取DNS数据字节
            for (int i = 0; i < 4; i++)
            {
                configurationSet.NetDNS[i] = Convert.ToUInt16(ConfigurationSetData.NetDNS[i]);
                intBuff = BitConverter.GetBytes(configurationSet.NetDNS[i]);
                AllBuff = CopyToBig(AllBuff, intBuff);
            }

            // 获取从站地址数据字节
            configurationSet.NetSourceAddr = Convert.ToUInt16(ConfigurationSetData.NetSourceAddr);
            intBuff = BitConverter.GetBytes(configurationSet.NetSourceAddr);
            AllBuff = CopyToBig(AllBuff, intBuff);

            // 获取ASDU地址数据字节
            configurationSet.NetASDUAddr = Convert.ToUInt16(ConfigurationSetData.NetASDUAddr);
            intBuff = BitConverter.GetBytes(configurationSet.NetASDUAddr);
            AllBuff = CopyToBig(AllBuff, intBuff);
            */

            // 设置要下发的遥信点号数据字节<默认字节总长度是600,不到600，则后续字节设置为0,超过600,提示错误)
            // 获取当前要下发的遥信点号实际数据字节长度
            int current = telesignalisationPointArry.Count;
            for (int i = 0; i < (YXNumber * 2) - current; i++)
            {
                telesignalisationPointArry.Add(Convert.ToByte(0));
            }
            intBuff = telesignalisationPointArry.ToArray();
            AllBuff = intBuff;
            // AllBuff = CopyToBig(AllBuff, telesignalisationPointArry.ToArray());

            // 依次获取选中的要下发的遥测点号数据字节（默认个数是300，不到300，则后续点号设置为0）
            for (int i = 0; i < YCNumber; i++)
            {
                if (i < TelemeteringPoint.Count())
                {
                    configurationSet.YCAddr[i] = Convert.ToUInt16(TelemeteringPoint[i].ID);
                }
                else
                {
                    configurationSet.YCAddr[i] = Convert.ToUInt16(0);
                }
                intBuff = BitConverter.GetBytes(configurationSet.YCAddr[i]);
                AllBuff = CopyToBig(AllBuff, intBuff);
            }

            // 依次获取选中的要下发的遥测点号其他标志数据字节（取反、变化、双点）
            for (int i = 0; i < YCNumber; i++)
            {
                if (i < TelemeteringPoint.Count())
                {
                    if (TelemeteringPoint[i].DataType != 0 && TelemeteringPoint[i].DataType != 1)
                    {
                        TelemeteringPoint[i].DataType = 2;
                    }

                    configurationSet.YCOtherFlag[i] = Convert.ToUInt16(TelemeteringPoint[i].DataType & 0x0003);
                }
                else
                {
                    configurationSet.YCOtherFlag[i] = Convert.ToUInt16(0);
                }
                intBuff = BitConverter.GetBytes(configurationSet.YCOtherFlag[i]);
                AllBuff = CopyToBig(AllBuff, intBuff);
            }

            // 依次获取选中的要下发的遥测点号遥测倍率数据字节（float型）
            for (int i = 0; i < YCNumber; i++)
            {
                if (i < TelemeteringPoint.Count())
                {
                    configurationSet.YCRate[i] = TelemeteringPoint[i].Rate;
                }
                else
                {
                    configurationSet.YCRate[i] = 0;
                }
                intBuff = BitConverter.GetBytes(configurationSet.YCRate[i]);
                AllBuff = CopyToBig(AllBuff, intBuff);
            }

            // 依次获取选中的要下发的遥控点号数据字节（默认个数是16，不到16，则后续点号设置为0）
            for (int i = 0; i < YKNumber; i++)
            {
                if (i < TelecontrolPoint.Count())
                {
                    configurationSet.YKAddr[i] = Convert.ToUInt16(TelecontrolPoint[i].YKID);
                }
                else
                {
                    configurationSet.YKAddr[i] = Convert.ToUInt16(0);
                }
                intBuff = BitConverter.GetBytes(configurationSet.YKAddr[i]);
                AllBuff = CopyToBig(AllBuff, intBuff);
            }

            // 依次获取选中的要下发的遥测点号其他标志数据字节（取反、变化、双点）
            for (int i = 0; i < YKNumber; i++)
            {
                if (i < TelecontrolPoint.Count())
                {
                    //UInt16 doublePoint = (UInt16)(TelecontrolPoint[i].DoublePoint == false ? 1 : 3);
                    UInt16 isNegated = (UInt16)(TelecontrolPoint[i].IsNegated == false ? 0 : 1);
                    //UInt16 isSOE = (UInt16)(TelecontrolPoint[i].IsSOE == false ? 0 : 1);
                    //UInt16 isChanged = (UInt16)(TelecontrolPoint[i].IsChanged == false ? 0 : 1);

                    configurationSet.YKOtherFlag[i] = Convert.ToUInt16(isNegated * 4);
                }
                else
                {
                    configurationSet.YKOtherFlag[i] = Convert.ToUInt16(0);
                }
                intBuff = BitConverter.GetBytes(configurationSet.YKOtherFlag[i]);
                AllBuff = CopyToBig(AllBuff, intBuff);
            }

            // 获取设备ID号数据字节
            if (UtilHelper.IsEmpty(ConfigurationSetData.DeviceID))
            {
                for (int i = 0; i < configurationSet.DeviceID.Length; i++)
                {
                    configurationSet.DeviceID[i] = 0;
                }
            }
            else
            {
                configurationSet.DeviceID = Encoding.ASCII.GetBytes(ConfigurationSetData.DeviceID);
            }

            AllBuff = CopyToBig(AllBuff, configurationSet.DeviceID);

            return AllBuff;
        }

        /// <summary>
        /// 将使用的点表保存到数据库
        /// </summary>
        private void SaveUsedPointToDB()
        {
            try
            {
                // 定义配置管理业务逻辑处理(service)类对象
                ConfigManageService configManageService = new ConfigManageService();
                // 将使用的点表保存到数据库
                configManageService.SaveUsedPointToDB(TelemeteringPoint, TelesignalisationPoint, TelecontrolPoint);

                // 发送更新三遥页面的点表消息,包括:
                // 三遥相应页面:遥测页面TelemeteringViewModel、遥信页面TelesignalisationViewModel、遥控页面TelecontrolViewModel
                Messenger.Default.Send<object>(null, "UpdateUsedThreeRemotePoint");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        /// <summary>
        /// 获取遥信点号内容数组
        /// </summary>
        /// <returns></returns>
        private List<byte> GetTelesignalisationPointArry()
        {
            // 定义遥信点号内容字节数组
            List<byte> telesignalisationPointArry = new List<byte>();
            // 遥信点号总数
            UInt16 total = Convert.ToUInt16(TelesignalisationPoint.Count);

            // 设置遥信点表总数量字节
            telesignalisationPointArry.AddRange(BitConverter.GetBytes(total).ToList());

            // 依次添加遥信点号及其内容
            for (int i = 0; i < total; i++)
            {
                #region 获取属性值

                UInt16 doublePoint = (UInt16)(TelesignalisationPoint[i].DoublePoint == false ? 1 : 3);
                UInt16 isNegated = (UInt16)(TelesignalisationPoint[i].IsNegated == false ? 0 : 1);
                UInt16 isSOE = (UInt16)(TelesignalisationPoint[i].IsSOE == false ? 0 : 1);
                UInt16 isChanged = (UInt16)(TelesignalisationPoint[i].IsChanged == false ? 0 : 1);
                UInt16 amountAndOtherFlag = Convert.ToUInt16(doublePoint + isNegated * 4 + isSOE * 16 + isChanged * 64);

                #endregion 获取属性值

                // 判断是不是组合遥信
                if (CombineTelesignalisationManageService.ValidCombineContent(TelesignalisationPoint[i].Comment))
                {
                    #region 组合遥信

                    // 获取组合遥信包含的遥信点号数组
                    //string combineContent = TelesignalisationPoint[i].Comment.Replace("!", "");
                    //string[] idInfos = combineContent.Split(new char[2] { '|', '&' });
                    string[] idInfos = TelesignalisationPoint[i].Comment.Split(new char[2] { '|', '&' });

                    string[] ids = new string[idInfos.Length];
                    for (int k = 0; k < idInfos.Length; k++)
                    {
                        if (idInfos[k].IndexOf('(') != -1)
                        {
                            ids[k] = idInfos[k].Substring(0, idInfos[k].IndexOf('('));
                        }
                        else
                        {
                            ids[k] = idInfos[k];
                        }
                    }

                    // 设置数量和属性字节
                    amountAndOtherFlag += Convert.ToUInt16(ids.Length * 256);
                    telesignalisationPointArry.AddRange(BitConverter.GetBytes(amountAndOtherFlag).ToList());

                    // 添加组合遥信里第一个遥信点号
                    // 获取遥信点号及其相应的操作符
                    byte not0 = ids[0].IndexOf('!') == -1 ? (byte)0 : (byte)1;
                    string id0 = ids[0].Replace("!", "");
                    UInt16 firstelesignalisationPointContent = Convert.ToUInt16((UInt16)not0 * 4096 + (Convert.ToUInt16(id0, 16) & 0x0FFF));
                    telesignalisationPointArry.AddRange(BitConverter.GetBytes(firstelesignalisationPointContent).ToList());

                    // 查找备注中 & |
                    List<char> logicList = new List<char>();
                    for (int m = 0; m < TelesignalisationPoint[i].Comment.Length; m++)
                    {
                        if(TelesignalisationPoint[i].Comment[m] == '&' || TelesignalisationPoint[i].Comment[m] == '|')
                        {
                            logicList.Add(TelesignalisationPoint[i].Comment[m]);
                        }
                    }

                    /*
                    if (logicList.Count != (ids.Length -1))
                    {
                        throw new Exception("即将下发的组合遥信中，组合遥信点号个数与逻辑运算符个数不一致");
                    }
                    */

                    for (int j = 1; j < ids.Length; j++)
                    {
                        // 判断操作符,如果是|,则设置为1,如果是&,则设置为2
                        //byte op = TelesignalisationPoint[i].Comment[4 + (j - 1) * 5] == '|' ? (byte)1 : (byte)2;
                        byte op = logicList[j - 1] == '|' ? (byte)1 : (byte)2;
                        // 获取遥信点号及其相应的操作符
                        byte not = ids[j].IndexOf('!') == -1 ? (byte)0 : (byte)1;
                        string id = ids[j].Replace("!", "");
                        UInt16 telesignalisationPointContent = Convert.ToUInt16((UInt16)(op * 4 + not) * 4096 + (Convert.ToUInt16(id, 16) & 0x0FFF));
                        // 设置遥信点号及其相应的操作符字节
                        telesignalisationPointArry.AddRange(BitConverter.GetBytes(telesignalisationPointContent).ToList());
                    }

                    #endregion 组合遥信
                }
                else
                {
                    #region 单遥信点号,包括空行处理

                    // 判断是不是空行
                    if (TelesignalisationPoint[i].ID == 0)
                    {
                        // 设置数量和属性字节
                        telesignalisationPointArry.AddRange(BitConverter.GetBytes(Convert.ToUInt16(0)).ToList());
                    }
                    else
                    {
                        // 设置数量和属性字节
                        telesignalisationPointArry.AddRange(BitConverter.GetBytes((UInt16)(amountAndOtherFlag + 256)).ToList());
                        // 设置遥信点号及其相应的操作符字节
                        telesignalisationPointArry.AddRange(BitConverter.GetBytes(Convert.ToUInt16(TelesignalisationPoint[i].ID)).ToList());
                    }

                    #endregion 单遥信点号,包括空行处理

                }

            }

            return telesignalisationPointArry;
        }

        /// <summary>
        /// 解析遥信点表配置内容
        /// </summary>
        /// <returns>字符串集合的集合，一共两个集合元素：第一个集合元素表示不存在的原始遥信点号集合，第二个集合元素表示不存在的组合遥信点号集合</returns>
        private IList<IList<string>> analysisTelesignalisationPointArry()
        {
            telesignalisationPoint.Clear();

            DevPointDao devPointDao = new DevPointDao();

            // 索引
            int location = 0;
            UInt16 total = configurationSet.YXAddrVariable[location++];

            // 空遥信点号编号
            int telesignalisationPointDevpid = ConfigUtil.getDevPointID("遥信");

            // 不存在的遥信点表
            List<string> notExistsTelesignalisationPoint = new List<string>();
            // 不存在的组合遥信点表
            List<string> notExistsCombineTelesignalisationPoint = new List<string>();

            for (int i = 0; i < total; i++)
            {
                // 数量属性值
                UInt16 amountAndOtherFlag = configurationSet.YXAddrVariable[location++];
                // 数量
                int amount = amountAndOtherFlag / 256;
                // 判断数量是不是为0,为0代表空行
                if (amount == 0)
                {
                    #region 数量为0,代表空行

                    Telesignalisation telesignalisation = new Telesignalisation();
                    telesignalisation.Number = 0;
                    telesignalisation.Devpid = telesignalisationPointDevpid;
                    telesignalisation.ID = 0;
                    telesignalisation.Name = "0";

                    TelesignalisationPoint.Add(telesignalisation);

                    #endregion 数量为0,代表空行
                }
                else
                {
                    Telesignalisation telesignalisationTemp = new Telesignalisation();
                    telesignalisationTemp.ID = 0;
                    telesignalisationTemp.Name = "0";

                    #region 设置点号属性值

                    // 双点
                    if ((amountAndOtherFlag & 0x0003) == 1)
                    {
                        telesignalisationTemp.DoublePoint = false;
                    }
                    else
                    {
                        telesignalisationTemp.DoublePoint = true;
                    }

                    // 取反
                    if ((amountAndOtherFlag & 0x000C) == 0)
                    {
                        telesignalisationTemp.IsNegated = false;
                    }
                    else
                    {
                        telesignalisationTemp.IsNegated = true;
                    }

                    // SOE
                    if ((amountAndOtherFlag & 0x0030) == 0)
                    {
                        telesignalisationTemp.IsSOE = false;
                    }
                    else
                    {
                        telesignalisationTemp.IsSOE = true;
                    }

                    // 变化遥信
                    if ((amountAndOtherFlag & 0x00C0) == 0)
                    {
                        telesignalisationTemp.IsChanged = false;
                    }
                    else
                    {
                        telesignalisationTemp.IsChanged = true;
                    }

                    #endregion 设置点号属性值

                    #region 数量为1,代表原始遥信单个点号

                    // 如果数量为1,代表原始遥信单个点号
                    if (amount == 1)
                    {
                        // 遥信单个点号
                        UInt16 telesignalisationPointID = configurationSet.YXAddrVariable[location++];

                        #region 查找点号存不存在

                        // 查找点号索引
                        int index = telesignalisationPointModelIDIndex.IndexOf(telesignalisationPointID);
                        if (index != -1)
                        {
                            Telesignalisation telesignalisation = TelesignalisationPointModel[index];
                            telesignalisation.DoublePoint = telesignalisationTemp.DoublePoint;
                            telesignalisation.IsNegated = telesignalisationTemp.IsNegated;
                            telesignalisation.IsSOE = telesignalisationTemp.IsSOE;
                            telesignalisation.IsChanged = telesignalisationTemp.IsChanged;

                            TelesignalisationPoint.Add(telesignalisation);
                            TelesignalisationPointModel.RemoveAt(index);
                            telesignalisationPointModelIDIndex.RemoveAt(index);
                        }
                        else
                        {
                            notExistsTelesignalisationPoint.Add(telesignalisationPointID.ToString());
                        }

                        #endregion 查找点号存不存在

                    }

                    #endregion 数量为1,代表原始遥信单个点号

                    #region 数量大于1,代表组合遥信包含的点号数量

                    try
                    {
                        if (amount > 1)
                        {
                            string content = "";

                            // 从第一个包含的点号开始(从第二个开始数值的头两位比特位是1或2,代表|或&)
                            for (int j = 0; j < amount; j++)
                            {
                                UInt16 telesignalisationPointContent = configurationSet.YXAddrVariable[location++];
                                int andOr = telesignalisationPointContent / 16384;
                                string andOrFlag = (andOr == 0 ? "" : (andOr == 1 ? "|" : "&"));
                                content += andOrFlag;
                                string id = (telesignalisationPointContent % 4096).ToString("x4").ToUpper();
                                content += (((telesignalisationPointContent / 4096) % 2) == 1 ? "!" : "") + id;
                                content = AddChineseComment(content, id);
                            }

                            IList<DevPoint> dpList = devPointDao.queryByPointTypeIdAndComment(ConfigUtil.getPointTypeID("遥信"), content.ToUpper());

                            telesignalisationTemp.ID = 1111;
                            telesignalisationTemp.Name = "组合遥信";
                            telesignalisationTemp.Comment = content;

                            if (dpList != null && dpList.Count > 0)
                            {
                                Telesignalisation telesignalisation = new Telesignalisation();
                                telesignalisation.ID = Convert.ToInt32(dpList[0].ID, 16);
                                telesignalisation.Flag = dpList[0].Flag;
                                int index = telesignalisationPointModelIDIndex.IndexOf(telesignalisation.ID);

                                telesignalisation = TelesignalisationPointModel[index];
                                telesignalisation.DoublePoint = telesignalisationTemp.DoublePoint;
                                telesignalisation.IsNegated = telesignalisationTemp.IsNegated;
                                telesignalisation.IsSOE = telesignalisationTemp.IsSOE;
                                telesignalisation.IsChanged = telesignalisationTemp.IsChanged;

                                TelesignalisationPoint.Add(telesignalisation);
                                TelesignalisationPointModel.RemoveAt(index);
                                telesignalisationPointModelIDIndex.RemoveAt(index);
                            }
                            else
                            {
                                DevPoint combineTelesignalisationPoint = new DevPoint();
                                combineTelesignalisationPoint.ID = CombineTelesignalisationManageService.GetMaxTelesignalisationPointID().ToString("x4").ToUpper();
                                combineTelesignalisationPoint.Name = "组合遥信";
                                combineTelesignalisationPoint.PointTypeId = ConfigUtil.getPointTypeID("遥信");
                                combineTelesignalisationPoint.PointType = "遥信";
                                combineTelesignalisationPoint.Comment = content;
                                combineTelesignalisationPoint.Flag = 1;

                                int success = devPointDao.insert(combineTelesignalisationPoint);
                                if (success > 0)
                                {
                                    // 获取该组合遥信点号
                                    DevPoint devPoint = devPointDao.queryByPointTypeIdAndPoint(ConfigUtil.getPointTypeID("遥信"), combineTelesignalisationPoint.ID);

                                    Telesignalisation telesignalisationCombine = new Telesignalisation();
                                    telesignalisationCombine.Selected = false;
                                    telesignalisationCombine.Devpid = devPoint.Devpid;
                                    telesignalisationCombine.Name = devPoint.Name;
                                    telesignalisationCombine.ID = Convert.ToInt32(devPoint.ID, 16);
                                    telesignalisationCombine.Number = TelesignalisationPoint.Count + 1;
                                    telesignalisationCombine.Comment = devPoint.Comment;
                                    telesignalisationCombine.Flag = devPoint.Flag;
                                    telesignalisationCombine.DoublePoint = telesignalisationTemp.DoublePoint;
                                    telesignalisationCombine.IsNegated = telesignalisationTemp.IsNegated;
                                    telesignalisationCombine.IsSOE = telesignalisationTemp.IsSOE;
                                    telesignalisationCombine.IsChanged = telesignalisationTemp.IsChanged;

                                    TelesignalisationPoint.Add(telesignalisationCombine);
                                }
                                else
                                {
                                    throw new Exception("组合遥信上载失败");
                                }
                                notExistsCombineTelesignalisationPoint.Add(content);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "警告");
                    }

                    #endregion 数量大于1,代表组合遥信包含的点号数量
                }

            }

            // 定义IList类型返回对象
            IList<IList<string>> retList = new List<IList<string>>();
            retList.Add(notExistsTelesignalisationPoint);
            retList.Add(notExistsCombineTelesignalisationPoint);
            return retList;
        }

        /// <summary>
        /// 组合遥信添加各组合点号的中文备注
        /// </summary>
        /// <param name="content">组合遥信点号备注</param>
        /// <param name="id">即将添加的组合点号</param>
        /// <returns>组合遥信的备注</returns>
        private string AddChineseComment(string content, string id)
        {
            DevPointDao devPointDaoObject = new DevPointDao();
            DevPoint combineTeleSignalisationPoint = devPointDaoObject.QueryByID(id);
            if (combineTeleSignalisationPoint != null)
            {
                content += "(" + combineTeleSignalisationPoint.Name + ")";
            }
            else
            {
                throw new Exception("组合遥信中包含原始点表中不存在的遥信点号");
            }
            return content;
        }

        /// <summary>
        /// 检验要下发的遥信点号是否正确
        /// </summary>
        /// <returns></returns>
        private bool checkDownTelesignalisationPoint()
        {
            // 定义一个遥信点表点号set集合
            HashSet<int> telesignalisationPointIDSet = new HashSet<int>();
            // 定义一个组合遥信中包含的点表点号set集合
            HashSet<int> combineTelesignalisationPointContainIDSet = new HashSet<int>();
            // 定义一个不合法的组合遥信点号set集合
            HashSet<int> illegalCombineTelesignalisationPointIDSet = new HashSet<int>();

            if (TelesignalisationPoint != null && TelesignalisationPoint.Count > 0)
            {
                for (int i = 0; i < TelesignalisationPoint.Count; i++)
                {
                    string combineContent = Regex.Replace(TelesignalisationPoint[i].Comment.Trim(), @"\s", "");
                    if (TelesignalisationPoint[i].Flag == 1)
                    {
                        if (CombineTelesignalisationManageService.ValidCombineContent(combineContent))
                        {
                            string[] ids = TelesignalisationPoint[i].Comment.Split(new char[2] { '|', '&' });
                            // 将组合遥信点号包含的点号添加到set集合中
                            foreach (var id in ids)
                            {
                                combineTelesignalisationPointContainIDSet.Add(Convert.ToInt32(id, 16));
                            }
                        }
                        else
                        {
                            illegalCombineTelesignalisationPointIDSet.Add(TelesignalisationPoint[i].ID);
                        }

                    }
                    else
                    {
                        telesignalisationPointIDSet.Add(TelesignalisationPoint[i].ID);
                    }
                }
            }

            string errorMsg = "";
            combineTelesignalisationPointContainIDSet.ExceptWith(telesignalisationPointIDSet);
            if (combineTelesignalisationPointContainIDSet.Count > 0)
            {
                foreach (var id in combineTelesignalisationPointContainIDSet)
                {
                    errorMsg += id.ToString("x4").ToUpper() + ",";
                }
                errorMsg = "组合遥信中包含的点号：" + errorMsg.Substring(0, errorMsg.Length - 1) + "不存在";
            }

            string illegalCombineTelesignalisationPointIDMsg = "";
            if (illegalCombineTelesignalisationPointIDSet.Count > 0)
            {
                foreach (var id in illegalCombineTelesignalisationPointIDSet)
                {
                    illegalCombineTelesignalisationPointIDMsg += id.ToString("x4").ToUpper() + ",";
                }
                illegalCombineTelesignalisationPointIDMsg = "组合遥信点号" + illegalCombineTelesignalisationPointIDMsg.Substring(0, illegalCombineTelesignalisationPointIDMsg.Length - 1) + "内容不合法;";
            }

            if (!UtilHelper.IsEmpty(errorMsg) || !UtilHelper.IsEmpty(illegalCombineTelesignalisationPointIDMsg))
            {
                MessageBox.Show(illegalCombineTelesignalisationPointIDMsg + errorMsg, "提示");
                return false;
            }

            return true;

        }

        /// <summary>
        /// 设备ID号内容字符串验证方法
        /// </summary>
        /// <param name="deviceID">设备ID号</param>
        /// <returns></returns>
        public bool CheckDeviceID(string deviceID)
        {
            return new Regex(validDeviceID).IsMatch(deviceID);
        }

        #region 模板点表操作逻辑

        /// <summary>
        /// 配置参数报表指令
        /// </summary>
        public RelayCommand<string> ConfigModuleCommand { get; private set; }


        /// <summary>
        /// 得到DTU配置模块树
        /// </summary>
        /// <returns></returns>
        public List<DTUNode> GetNodeList()
        {
            // 获取树结构中的所有节点
            DTUConfigurePointTableAllNodes = (List<DTUNode>)DTUConfigurePointTableDaoObject.Query();

            foreach (var treeNode in DTUConfigurePointTableAllNodes)
            {
                treeNode.FindParent(DTUConfigurePointTableAllNodes);
            }

            List<DTUNode> root = new List<DTUNode>();
            foreach (var treeNode in DTUConfigurePointTableAllNodes)
            {
                if (treeNode.NodeType == (int)DTUConfigureUtil.NodeType.RootNode)
                {
                    root.Add(treeNode);
                    break;
                }
            }
            return root;
        }

        /// <summary>
        /// 添加一个配置模块
        /// </summary>
        public void AddNewCongigureModule(string parentNodePath)
        {
            // 获取数据库中最大的NodeID值
            int nextRecord = 1;
            int nodeNumber = DTUConfigurePointTableDaoObject.QueryMaxNodeID() + nextRecord;

            DTUNode newModuleNode = new DTUNode();
            newModuleNode.NodeName = "模板";
            newModuleNode.NodeType = (int)DTUConfigureUtil.NodeType.FirstLevelNode;
            newModuleNode.Path = parentNodePath + "/" + nodeNumber;

            // 将新节点加入到数据库中
            DTUConfigurePointTableDaoObject.InsertNode(newModuleNode);
            // 为新插入的节点们找爸爸,同时重新加载新的配置树
            Messenger.Default.Send<string>("AddNewNode", "reloadDTUConfigureModuleTree");
        }

        /// <summary>
        /// 用户点击模板时，显示该模板的配置信息
        /// </summary>
        /// <param name="NodeID"></param>
        private void ExcuteGetCurrentModuleConfigure(int NodeID)
        {
            // 清空历史记录
            ModuleName = null;
            CurrentSalveAddress = null;
            CurrentTeleSignalisationNumber = null;
            CurrentTeleMeteringNumber = null;
            CurrentTeleControlNumber = null;

            // 获取当前模块的三遥数量、从站地址的信息
            DTUEachModuleConfigurationModel moduleInfo = DTUConfigurePointTableDaoObject.QueryModuleInfoByBeLongToModule(NodeID);
            if (moduleInfo != null)
            {
                CurrentSalveAddress = moduleInfo.SlaveAddress;
                CurrentTeleSignalisationNumber = moduleInfo.TeleSignalisationNumber;
                CurrentTeleMeteringNumber = moduleInfo.TeleMeteringNumber;
                CurrentTeleControlNumber = moduleInfo.TeleControlNumber;

                // 查询当前模块的名称
                DTUNode node = DTUConfigurePointTableDaoObject.QueryModuleName(NodeID);
                if (!node.Equals(null))
                {
                    ModuleName = node.NodeName;
                }
            }
        }

        /// <summary>
        /// 用户点击“保存当前模块设置”执行的逻辑-》将当前配置保存到数据库中
        /// </summary>
        private void ExcuteConfigModuleCommand(string arg)
        {
            switch (arg)
            {
                case "SaveConfigureModule":
                    SaveConfigureModule();
                    break;

                case "AddModuleToOriginPointTable":

                    break;
            }

        }

        /// <summary>
        /// 保存当前配置项
        /// </summary>
        private void SaveConfigureModule()
        {
            if (ConfigView.CurrentNode != null && ConfigView.CurrentNode.NodeType != 0)
            {
                if (!IsNullInEachConfigure())
                {
                    MessageBox.Show("请输入当前模块完整的配置参数", "提示");
                    return;
                }

                DTUEachModuleConfigurationModel currentConfiguration = new DTUEachModuleConfigurationModel
                {
                    BelongToModule = ConfigView.CurrentNode.NodeID,
                    TeleSignalisationNumber = (int)CurrentTeleSignalisationNumber,
                    TeleMeteringNumber = (int)CurrentTeleMeteringNumber,
                    TeleControlNumber = (int)CurrentTeleControlNumber,
                    SlaveAddress = (int)CurrentSalveAddress
                };

                try
                {
                    // 数据库中是否存在当前节点的配置记录
                    bool isExistConfigureRecord = DTUConfigurePointTableDaoObject.IsExistConfigureRecord(ConfigView.CurrentNode.NodeID);
                    if (isExistConfigureRecord)
                    {
                        // 当前配置项中存在配置信息记录，更新记录到数据库中
                        int result = DTUConfigurePointTableDaoObject.UpdateOneConfiguration(currentConfiguration);
                        if (result <= 0)
                        {
                            throw new Exception("更新模块记录至数据库时发生异常");
                        }
                        else
                        {
                            MessageBox.Show("保存成功", "提示");
                        }
                    }
                    else
                    {
                        // 当前配置项中不存在配置信息记录，直接添加记录到数据库中
                        int result = DTUConfigurePointTableDaoObject.InsertOneConfiguration(currentConfiguration);
                        if (result <= 0)
                        {
                            throw new Exception("插入模块记录至数据库时发生异常");
                        }
                        else
                        {
                            MessageBox.Show("保存成功", "提示");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "警告");
                }
            }
        }

        /// <summary>
        /// 当前配置参数是否为空
        /// </summary>
        /// <returns></returns>
        private bool IsNullInEachConfigure()
        {
            if (CurrentSalveAddress != null && CurrentTeleSignalisationNumber != null
                && CurrentTeleMeteringNumber != null && CurrentTeleControlNumber != null)
            {
                return true;
            }
            return false;
        }
        #endregion 模板点表操作逻辑
    }
}
