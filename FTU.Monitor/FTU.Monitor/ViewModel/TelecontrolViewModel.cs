using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using lib60870;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;
using FTU.Monitor.View;
using FTU.Monitor.Util;
using FTU.Monitor.Service;
using GalaSoft.MvvmLight.Messaging;
using FTU.Monitor.Model;
using FTU.Monitor.DataService;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// TelecontrolViewModel 的摘要说明
    /// author: songminghao
    /// date：2017/12/22 13:17:40
    /// desc：遥控ViewModel
    /// version: 1.0
    /// </summary>
    public class TelecontrolViewModel : ViewModelBase,IIEC104Handler
    {
        /// <summary>
        /// Xml中Password
        /// </summary>
        public static string telecontrolPW;

        /// <summary>
        /// 设置和获取配置文件XML中的遥控口令
        /// </summary>
        public string TelecontrolPW
        {
            get 
            { 
                return telecontrolPW; 
            }
            set
            {
                telecontrolPW = value;
            }
        }
        
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
                RaisePropertyChanged(() => IsPW);
            }
        }

        /// <summary>
        /// 遥控点表
        /// </summary>
        public static ObservableCollection<Telecontrol> telecontrolData;

        /// <summary>
        /// 设置和获取遥控点表
        /// </summary>
        public ObservableCollection<Telecontrol> TelecontrolData
        {
            get 
            { 
                return telecontrolData; 
            }
            set
            {
                telecontrolData = value;
                RaisePropertyChanged(() => TelecontrolData);
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        private string _parseInformationShow;

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public string ParseInformationShow
        {
            get
            {
                return this._parseInformationShow;
            }
            set
            {
                this._parseInformationShow = value;
            }
        }

        /// <summary>
        /// 遥控点号
        /// </summary>
        public static Int32 ID;

        /// <summary>
        /// 类型标识
        /// </summary>
        public static byte TI;

        /// <summary>
        /// 单点命令、双点命令
        /// </summary>
        public static byte SCODCO;

        /// <summary>
        /// 传输原因
        /// </summary>
        public static byte COT;

        /// <summary>
        /// 选择、执行、取消命令
        /// </summary>
        public RelayCommand<string> TelecontrolCommand 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// 接受处理遥控数据集合
        /// </summary>
        /// <param name="TI">遥控类型标识</param>
        /// <param name="asdu">对应遥控类型标识的ASDU</param>
        public void HandleASDUData(TypeID TI,ASDU asdu)
        {
            if (asdu.TypeId == TypeID.C_SC_NA_1)// 单点命令 45
            {
                if (asdu.IsNegative)
                {
                    //未知的类型标识
                    Console.WriteLine("单点遥控：P/N = 1");
                    ParseInformationShow = " P/N of telecontrol is negative\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    MessageBox.Show("失败原因：否定确认 P/N = 1", "单点遥控操作失败");
                }

                if (asdu.Cot == CauseOfTransmission.ACTIVATION_CON)
                {
                    Console.WriteLine((asdu.IsNegative ? "Negative" : "Positive") + "confirmation for Single command");
                    ParseInformationShow = (asdu.IsNegative ? "Negative" : "Positive") + "confirmation for Single command" + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                }
                else if (asdu.Cot == CauseOfTransmission.ACTIVATION_TERMINATION)
                {
                    Console.WriteLine("Single command terminated");
                    ParseInformationShow = "Single command terminated\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                }
                else if (asdu.Cot == CauseOfTransmission.UNKNOWN_TYPE_ID)
                {
                    //未知的类型标识
                    Console.WriteLine("单点遥控：未知的类型标识 unknownTypeID = 44");
                    ParseInformationShow = " Unknown TypeId of telecontrol\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    MessageBox.Show("失败原因：未知的类型标识 TI = 44", "单点遥控操作失败");
                }
                else if (asdu.Cot == CauseOfTransmission.UNKNOWN_CAUSE_OF_TRANSMISSION)
                {
                    //未知的传送原因 UnknownTypeCaseTransmission =  45
                    Console.WriteLine("单点遥控：未知的传送原因 UnknownTypeCaseTransmission =  45");
                    ParseInformationShow = " Unknown COT of telecontrol\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    MessageBox.Show("失败原因：未知的类型标识 COT = 45", "单点遥控操作失败");
                }
                else if (asdu.Cot == CauseOfTransmission.UNKNOWN_COMMON_ADDRESS_OF_ASDU)
                {
                    //未知的应用服务数据单元公共地址 UnknownAppDataPublicAddress= 46
                    Console.WriteLine("单点遥控：未知的应用服务数据单元公共地址 UnknownAppDataPublicAddress = 46");
                    ParseInformationShow = " Unknown ASDU common address of telecontrol\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    MessageBox.Show("失败原因：未知的类型标识 CA = 46", "单点遥控操作失败");
                }
                else if (asdu.Cot == CauseOfTransmission.UNKNOWN_INFORMATION_OBJECT_ADDRESS)
                {
                    //未知信息对象地址 UnknownInformationObjectAddress= 47
                    Console.WriteLine("单点遥控：未知信息对象地址 UnknownInformationObjectAddress= 47");
                    ParseInformationShow = " Unknown infomation object address of telecontrol\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    MessageBox.Show("失败原因：未知的类型标识 infomation object address = 47", "单点遥控操作失败");
                }

                var sc = (SingleCommand)asdu.GetElement(0);
                Console.WriteLine("  IOA: " + sc.ObjectAddress + " state : " + sc.State);
                ParseInformationShow = "  IOA: " + sc.ObjectAddress + " state : " + sc.State + "\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);

            }
            else if (asdu.TypeId == TypeID.C_DC_NA_1)// 双点命令 46
            {
                if (asdu.IsNegative)
                {
                    //未知的类型标识
                    Console.WriteLine("单点遥控：P/N = 1");
                    ParseInformationShow = " P/N of telecontrol is negative\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    MessageBox.Show("失败原因：否定确认 P/N = 1", "单点遥控操作失败");
                }

                if (asdu.Cot == CauseOfTransmission.ACTIVATION_CON)
                {
                    Console.WriteLine((asdu.IsNegative ? "Negative" : "Positive") + "confirmation for Double command");
                    ParseInformationShow = (asdu.IsNegative ? "Negative" : "Positive") + "confirmation for Double command" + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                }
                else if (asdu.Cot == CauseOfTransmission.ACTIVATION_TERMINATION)
                {
                    //ShowMessage.ShowFunction("双点命令激活终止");
                    Console.WriteLine("Double command terminated");
                    ParseInformationShow = "Double command terminated" + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                }
                else if (asdu.Cot == CauseOfTransmission.UNKNOWN_TYPE_ID)
                {
                    //未知的类型标识
                    Console.WriteLine("双点遥控：未知的类型标识 unknownTypeID = 44");
                    ParseInformationShow = " Unknown TypeId of telecontrol\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    MessageBox.Show("失败原因：未知的类型标识 TI = 44", "双点遥控操作失败");
                }
                else if (asdu.Cot == CauseOfTransmission.UNKNOWN_CAUSE_OF_TRANSMISSION)
                {
                    //未知的传送原因 UnknownTypeCaseTransmission =  45
                    Console.WriteLine("双点遥控：未知的传送原因 UnknownTypeCaseTransmission =  45");
                    ParseInformationShow = " Unknown COT of telecontrol\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    MessageBox.Show("失败原因：未知的类型标识 COT = 45", "双点遥控操作失败");
                }
                else if (asdu.Cot == CauseOfTransmission.UNKNOWN_COMMON_ADDRESS_OF_ASDU)
                {
                    //未知的应用服务数据单元公共地址 UnknownAppDataPublicAddress= 46
                    Console.WriteLine("双点遥控：未知的应用服务数据单元公共地址 UnknownAppDataPublicAddress = 46");
                    ParseInformationShow = " Unknown ASDU common address of telecontrol\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    MessageBox.Show("失败原因：未知的类型标识 CA = 46", "双点遥控操作失败");
                }
                else if (asdu.Cot == CauseOfTransmission.UNKNOWN_INFORMATION_OBJECT_ADDRESS)
                {
                    //未知信息对象地址 UnknownInformationObjectAddress= 47
                    Console.WriteLine("双点遥控：未知信息对象地址 UnknownInformationObjectAddress= 47");
                    ParseInformationShow = " Unknown infomation object address of telecontrol\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    MessageBox.Show("失败原因：未知的类型标识 IOA = 47", "双点遥控操作失败");
                }
                var dc = (DoubleCommand)asdu.GetElement(0);

                Console.WriteLine("  IOA: " + dc.ObjectAddress + " state : " + dc.State);
                ParseInformationShow = "  IOA: " + dc.ObjectAddress + " state : " + dc.State + "\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);

            }
        }


        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool Action(string s)
        {
            ExecuteTelecontrolCommand("Action");
            return true;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool Cancel(string s)
        {
            ExecuteTelecontrolCommand("Cancel");
            return true;
        }


        /// <summary>
        /// 加载遥控点表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool ReloadPoint(object obj)
        {
            ReloadTelecontrolPoint(null);
            return true;
        }

        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public bool Select(string arg)
        {
            ExecuteTelecontrolCommand("Select");
            return true;
        }


        /// <summary>
        /// 选择、执行、取消命令的执行方法
        /// </summary>
        /// <param name="arg">参数</param>
        public void ExecuteTelecontrolCommand(string arg)
        {
            // 链路是否连通
            if(!CommunicationViewModel.IsLinkConnect())
            {
                return;
            }
            TI = (SelectedIndexSingleORDouble == 0) ? (byte)45 : (byte)46;
            bool seleceCommand = true;
            TelecontrolPWView PWView = new TelecontrolPWView();
            TelecontrolPWViewModel PWViewModelObject = new TelecontrolPWViewModel();
            int selectNum;
            switch (arg)
            {
                // 选中
                case "Select":
                    //selectNum = 0;
                    //for (int i = 0; i < TelecontrolData.Count(); i++)
                    //{
                    //    if (TelecontrolData[i].Selected == true)
                    //    {
                    //        selectNum++;
                    //    }
                    //}
                    //if (selectNum == 1)
                    //{
                    //    for (int i = 0; i < TelecontrolData.Count(); i++)
                    //    {
                    //        if (TelecontrolData[i].Selected == true)
                    //        {
                    //            ID = TelecontrolData[i].YKID;
                    //        }
                    //    }

                    //    PWView.ShowDialog();
                    //    if (IsPW == true)
                    //    {
                    //        if (TelecontrolPW == TelecontrolPWViewModel.pwBox)
                    //        {
                                //重新读取Xml遥控密码
                                PWViewModelObject.ReadPassword("TelecontrolPW");
                                if (TI == 45)
                                {
                                    SCODCO = (SelectIndexOpenORColse == 0) ? (byte)0x80 : (byte)0x81;

                                }
                                else if (TI == 46)
                                {
                                    SCODCO = (SelectIndexOpenORColse == 0) ? (byte)0x81 : (byte)0x82;
                                }
                                COT = 6;
                                seleceCommand = true;
                                TelecontrolPWViewModel.pwBox = "";

                    //        }
                    //        else
                    //        {
                    //            TelecontrolPWViewModel.pwBox = "";
                    //            MessageBox.Show("输入的口令不正确", "提示");
                    //            IsPW = false;
                    //            return;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        IsPW = false;
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("选择一个遥控对象", "提示");
                    //    return;
                    //}
                    break;

                // 执行
                case "Action":
                    //selectNum = 0;
                    //for (int i = 0; i < TelecontrolData.Count(); i++)
                    //{
                    //    if (TelecontrolData[i].Selected == true)
                    //    {
                    //        selectNum++;
                    //    }
                    //}
                    //if (selectNum == 1)
                    //{
                    //    PWView.ShowDialog();
                    //    if (IsPW == true)
                    //    {
                    //        if (TelecontrolPW == TelecontrolPWViewModel.pwBox)
                    //        {
                    //            //重新读取Xml遥控密码
                    //            PWViewModelObject.ReadPassword("TelecontrolPW");
                                if (TI == 45)
                                {
                                    SCODCO = (SelectIndexOpenORColse == 0) ? (byte)0x0 : (byte)0x1;
                                }
                                else if (TI == 46)
                                {
                                    SCODCO = (SelectIndexOpenORColse == 0) ? (byte)0x1 : (byte)0x2;
                                }
                                seleceCommand = false;
                                COT = 6;
                                TelecontrolPWViewModel.pwBox = "";
                    //        }
                    //        else
                    //        {
                    //            TelecontrolPWViewModel.pwBox = "";
                    //            MessageBox.Show("输入的密码不正确", "提示");
                    //            IsPW = false;
                    //            return;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        IsPW = false;
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("选择一个遥控对象", "提示");
                    //    return;
                    //}
                    break;

                //取消
                case "Cancel":
                    //selectNum = 0;
                    //for (int i = 0; i < TelecontrolData.Count(); i++)
                    //{
                    //    if (TelecontrolData[i].Selected == true)
                    //    {
                    //        selectNum++;
                    //    }
                    //}
                    //if (selectNum == 1)
                    //{
                    //    PWView.ShowDialog();
                    //    if (IsPW == true)
                    //    {
                    //        if (TelecontrolPW == TelecontrolPWViewModel.pwBox)
                    //        {
                    //            //重新读取Xml遥控密码
                    //            PWViewModelObject.ReadPassword("TelecontrolPW");
                                if (TI == 45)
                                {
                                    SCODCO = (SelectIndexOpenORColse == 0) ? (byte)0x0 : (byte)0x1;
                                }
                                else if (TI == 46)
                                {
                                    SCODCO = (SelectIndexOpenORColse == 0) ? (byte)0x1 : (byte)0x2;
                                }
                                COT = 8;
                                seleceCommand = false;
                                TelecontrolPWViewModel.pwBox = "";
                    //        }
                    //        else
                    //        {
                    //            TelecontrolPWViewModel.pwBox = "";
                    //            MessageBox.Show("输入的密码不正确", "提示");
                    //            IsPW = false;
                    //            return;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        IsPW = false;
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("选择一个遥控对象", "提示");
                    //    return;
                    //}
                    break;
            }

            bool command = (SelectIndexOpenORColse == 0) ? false : true;

            if (TI == 45)
            {
                CommunicationViewModel.con.SendControlCommand((CauseOfTransmission)COT, 0x01, new SingleCommand(ID, command, seleceCommand, 0));
            }
            else
            {
                CommunicationViewModel.con.SendControlCommand((CauseOfTransmission)COT, 0x01, new DoubleCommand(ID, SCODCO, seleceCommand, 0));
            }

        }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TelecontrolViewModel()
        {
            // 注册接收到遥控ASDU的处理事件
            IEC104.RegisterIEC104Handler(TypeID.C_SC_NA_1, this); // 单点命令 45
            IEC104.RegisterIEC104Handler(TypeID.C_DC_NA_1, this); // 双点命令 46 
            // 注册接收更新相应页面的点表消息(点表重新导入后，需要更新相应页面显示的点表)
            Messenger.Default.Register<object>(this, "UpdateSourcePoint", ReloadTelecontrolPoint);
            // 注册接收更新三遥页面的点表消息(配置点表下发后，需要更新三遥页面显示的点表)
            Messenger.Default.Register<object>(this, "UpdateUsedThreeRemotePoint", ReloadTelecontrolPoint);

            isPW = false;

            // 命令
            TelecontrolCommand = new RelayCommand<string>(ExecuteTelecontrolCommand);

            telecontrolData = new ObservableCollection<Telecontrol>();
            // 重新载入点表
            ReloadTelecontrolPoint(null);

            this._openORClose = new ObservableCollection<string>();
            this._openORClose.Add("分");
            this._openORClose.Add("合");
            this._selectedIndexOpenORColse = 0;

            this._singleORDouble = new ObservableCollection<string>();
            this._singleORDouble.Add("单点");
            this._singleORDouble.Add("双点");
            this._selectedIndexSingleORDouble = 0;

        }

        /// <summary>
        /// 开关分合选择
        /// </summary>
        private ObservableCollection<string> _openORClose;

        /// <summary>
        /// 设置和获取开关分合选择
        /// </summary>
        public ObservableCollection<string> OpenORClose
        {
            get 
            { 
                return this._openORClose; 
            }
            set
            {
                this._openORClose = value;
                RaisePropertyChanged(() => OpenORClose);
            }
        }

        /// <summary>
        /// 开关分合选择索引
        /// </summary>
        private int _selectedIndexOpenORColse;

        /// <summary>
        /// 设置和获取开关分合选择索引
        /// </summary>
        public int SelectIndexOpenORColse
        {
            get 
            { 
                return this._selectedIndexOpenORColse; 
            }
            set
            {
                this._selectedIndexOpenORColse = value;
                RaisePropertyChanged(() => SelectIndexOpenORColse);
            }
        }

        /// <summary>
        /// 单点、双点选择
        /// </summary>
        private ObservableCollection<string> _singleORDouble;

        /// <summary>
        /// 设置和获取单点、双点选择
        /// </summary>
        public ObservableCollection<string> SingleORDouble
        {
            get 
            {
                return this._singleORDouble; 
            }
            set
            {
                this._singleORDouble = value;
                RaisePropertyChanged(() => SingleORDouble);
            }
        }

        /// <summary>
        /// 单点、双点选择索引
        /// </summary>
        private int _selectedIndexSingleORDouble;

        /// <summary>
        /// 设置和获取单点、双点选择索引
        /// </summary>
        public int SelectedIndexSingleORDouble
        {
            get 
            { 
                return this._selectedIndexSingleORDouble; 
            }
            set
            {
                this._selectedIndexSingleORDouble = value;
                RaisePropertyChanged(() => SelectedIndexSingleORDouble);
            }
        }

        public bool ValueToParameter(int id ,byte singleordouble ,int openorclose)
        {
            ID = id;
            TI = singleordouble;
            SelectIndexOpenORColse = openorclose;
            return true;
        }

        /// <summary>
        /// 重新载入遥控点表
        /// </summary>
        private void ReloadTelecontrolPoint(object obj)
        {
            TelecontrolManageService telecontrolManageService = new TelecontrolManageService();
            telecontrolData.Clear();
            // 获取使用的所有遥控点表
            IList<Telecontrol> telecontrolList = telecontrolManageService.GetTelecontrolPoint(ConfigUtil.getPointTypeID("遥控"));

            if (telecontrolList != null && telecontrolList.Count > 0)
            {
                foreach (var telecontrol in telecontrolList)
                {
                    telecontrolData.Add(telecontrol);
                }
            }

        }

    }
}
