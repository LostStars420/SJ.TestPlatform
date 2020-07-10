using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using FTU.Monitor.Model;
using System;
using System.Collections.ObjectModel;
using FTU.Monitor.Util;
using lib60870;
using System.Timers;
using FTU.Monitor.Dao;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using FTU.Monitor.Service;
using FTU.Monitor.DataService;
using Microsoft.Win32;
using System.Data;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// TelemeteringViewModel 的摘要说明
    /// author: songminghao
    /// date：2017/10/12 9:48:09
    /// desc：遥测ViewModel
    /// version: 1.0
    /// </summary>
    public class TelemeteringViewModel : ViewModelBase,IIEC104Handler
    {
        /// <summary>
        /// 定时间隔
        /// </summary>
        private int _timingInterval;

        /// <summary>
        /// 设置和获取定时间隔
        /// </summary>
        public int TimingInterval
        {
            get
            {
                return this._timingInterval;
            }
            set
            {
                this._timingInterval = value;
                RaisePropertyChanged(() => TimingInterval);
            }
        }
        /// <summary>
        ///*** 需要打印的遥测集合***
        /// </summary>
        public static List<TelemeteringList> telemeteringList;
        public List<TelemeteringList> TelemeteringList
        {
            get { return telemeteringList; }
            set
            {
                RaisePropertyChanged(()=>TelemeteringList);
            }
        }
       
        /// <summary>
        /// 定时总召唤定时器
        /// </summary>
        public static System.Timers.Timer timingGeneralInterrogationTimer;

        /// <summary>
        /// 是否处于定时总召唤
        /// </summary>
        private static bool IstimingGeneralInterrogation;

        /// <summary>
        /// 遥测点表集合
        /// </summary>
        public static ObservableCollection<Telemetering> telemeteringData;

        /// <summary>
        /// 获取和设置遥测点表集合
        /// </summary>
        public ObservableCollection<Telemetering> TelemeteringData
        {
            get
            {
                return telemeteringData;
            }
            set
            {
                telemeteringData = value;
                RaisePropertyChanged(() => TelemeteringData);
            }
        }      

        /// <summary>
        /// 定时总召唤刷新
        /// </summary>
        public RelayCommand TimingGeneralInterrogationCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 定时总召唤刷新执行操作
        /// </summary>
        public void ExecuteTimingGeneralInterrogationCommand()
        {
            if (CommunicationViewModel.IsLinkConnect() && IstimingGeneralInterrogation == false)
            {
                IstimingGeneralInterrogation = true;
                timingGeneralInterrogationTimer = new System.Timers.Timer(100);
                timingGeneralInterrogationTimer.Elapsed += CommunicationViewModel.TimingGeneralInterrogationTimeOutEvent;
                timingGeneralInterrogationTimer.AutoReset = true;
                timingGeneralInterrogationTimer.Enabled = true;
            }
        }

        /// <summary>
        /// 越限的遥测点号信息
        /// </summary>
        private string _telemeteringPointError;

        /// <summary>
        /// 设置和获取越限的遥测点号信息
        /// </summary>
        public string TelemeteringPointError
        {
            get
            {
                return this._telemeteringPointError;
            }
            set
            {
                this._telemeteringPointError = value;
                RaisePropertyChanged(() => TelemeteringPointError);
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
        /// 关闭定时总召唤刷新
        /// </summary>
        public RelayCommand CloseTimingGeneralInterrogationCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 关闭定时总召唤刷新执行操作
        /// </summary>
        public static void ExecuteCloseTimingGeneralInterrogationCommand()
        {
            if (CommunicationViewModel.IsLinkConnect() && IstimingGeneralInterrogation == true)
            {
                IstimingGeneralInterrogation = false;
                if (timingGeneralInterrogationTimer != null)
                {
                    timingGeneralInterrogationTimer.Stop();
                    timingGeneralInterrogationTimer.Enabled = false;
                    timingGeneralInterrogationTimer.Close();
                    timingGeneralInterrogationTimer.Dispose();
                }
            }
        }

        /// <summary>
        /// 总召唤刷新
        /// </summary>
        public RelayCommand GeneralInterrogationCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 总召唤刷新执行操作
        /// </summary>
        public void ExecuteGeneralInterrogationCommand()
        {
            if(CommunicationViewModel.IsLinkConnect())
            {
                CommunicationViewModel.con.SendInterrogationCommand(CauseOfTransmission.ACTIVATION, 1, QualifierOfInterrogation.STATION);
            }           
        }

        /// <summary>
        /// 执行总召唤事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void TimingGeneralInterrogationTimeOutEvent(Object source, ElapsedEventArgs e)
        {
            CommunicationViewModel.con.SendInterrogationCommand(CauseOfTransmission.ACTIVATION, 1, QualifierOfInterrogation.STATION);
        }

        /// <summary>
        /// 赋值遥测错误点号信息
        /// </summary>
        /// <param name="arg"></param>
        void ExcuteTeleMeteringPointError(string arg)
        {
            TelemeteringPointError = arg;
        }

        /// <summary>
        /// 接收到遥测ASDU数据包的处理方法
        /// </summary>
        /// <param name="TI">遥测类型标识</param>
        /// <param name="asdu">遥测ASDU数据</param>
        public void HandleASDUData(TypeID TI, ASDU asdu)
        {
           
            
           
            if (asdu.TypeId == TypeID.M_ME_NA_1)//测量值，归一化值 09
            {
                ParseInformationShow = "测量值，归一化值\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);                
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var msv = (MeasuredValueNormalized)asdu.GetElement(i);
                    ParseInformationShow = "  IOA: " + msv.ObjectAddress + "  IOA(HEX): " + msv.ObjectAddress.ToString("X") + " scaled value: " + msv.NormalizedValue + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    LogHelper.Info(typeof(TelemeteringViewModel), "测量值，归一化值");
                    Telemetering telemetering = telemeteringData[msv.ObjectAddress - 0x4001];
                    telemetering.Rate = telemetering.Rate <= 1 ? 1 : telemetering.Rate;

                    telemeteringData[msv.ObjectAddress - 0x4001].Value = msv.NormalizedValue * telemetering.Rate;
                    if (CoefficientViewModel.number != 0)//校准模式
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            CoefficientViewModel.userData[j].Data[CoefficientViewModel.number - 1] = msv.NormalizedValue;
                            float total = 0;
                            for (int k = 0; k < CoefficientViewModel.number; k++)
                            {
                                total += CoefficientViewModel.userData[j].Data[k];
                            }
                            CoefficientViewModel.userData[j].AverageValue = total / (CoefficientViewModel.number + 1);
                        }
                    }
                }
            }
            else if (asdu.TypeId == TypeID.M_ME_NB_1)//测量值，标度化值 11
            {
                ParseInformationShow = "测量值，标度化值\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);              
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var msv = (MeasuredValueScaled)asdu.GetElement(i);

                    Console.WriteLine("  IOA: " + msv.ObjectAddress + " scaled value: " + msv.ScaledValue);
                    Console.WriteLine("   " + msv.Quality.ToString());

                    ParseInformationShow = "  IOA: " + msv.ObjectAddress + " scaled value: " + msv.ScaledValue + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    LogHelper.Info(typeof(TelemeteringViewModel), "测量值，标度化值");
                    Telemetering telemetering = telemeteringData[msv.ObjectAddress - 0x4001];
                    telemetering.Rate = telemetering.Rate <= 1 ? 1 : telemetering.Rate;

                    telemeteringData[msv.ObjectAddress - 0x4001].Value = msv.ScaledValue.ShortValue / telemetering.Rate;
                }

            }
            else if (asdu.TypeId == TypeID.M_ME_NC_1)//测量值，短浮点数 13
            {
                ParseInformationShow = "测量值，短浮点数\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);
                // 不对应的遥测点表字符串，点号以逗号隔开
                string notExistsTelemeteringPoint = "";

                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var mfv = (MeasuredValueShort)asdu.GetElement(i);//获取特定索引的数据元素
                    ParseInformationShow = "  IOA: " + mfv.ObjectAddress + "  IOA(HEX): " + mfv.ObjectAddress.ToString("X") + " float value: " + mfv.Value + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    LogHelper.Info(typeof(TelemeteringViewModel), "测量值，短浮点数" + ParseInformationShow);
                    int telemeteringPointIndex = mfv.ObjectAddress - 0x4001;
                    TelemeteringList telemeterdata = new TelemeteringList();
                    telemeterdata.ID = mfv.ObjectAddress;
                    telemeterdata.VALUE = mfv.Value;
                    telemeteringList.Add(telemeterdata);
                    if (telemeteringPointIndex >= telemeteringData.Count)
                    {
                        MainViewModel.outputdata.Debug += "遥测报文包含的点号越限:" + mfv.ObjectAddress.ToString("X4") + "\n";
                        notExistsTelemeteringPoint += mfv.ObjectAddress.ToString("X4") + ",";
                        continue;
                    }

                    telemeteringData[telemeteringPointIndex].Value = mfv.Value;

                    if (CoefficientViewModel.number != 0)//校准模式
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            CoefficientViewModel.userData[j].Data[CoefficientViewModel.number - 1] = mfv.Value;
                            float total = 0;
                            for (int k = 0; k < CoefficientViewModel.number; k++)
                            {
                                total += CoefficientViewModel.userData[j].Data[k];
                            }
                            CoefficientViewModel.userData[j].AverageValue = total / (CoefficientViewModel.number + 1);
                        }
                    }
                }
                // 判断不对应的遥测点表字符串是不是为空，不为空则提示异常信息
                if (!UtilHelper.IsEmpty(notExistsTelemeteringPoint))
                {
                    string telemeteringPointMessage = "遥测报文包含的点号越限:" + notExistsTelemeteringPoint.Substring(0, notExistsTelemeteringPoint.Length - 1);
                    Messenger.Default.Send<string>(telemeteringPointMessage, "teleMeteringPointMessage");
                }
                else
                {
                    Messenger.Default.Send<string>("", "teleMeteringPointMessage");
                }

            }
            else if (asdu.TypeId == TypeID.M_ME_ND_1)// 测量值，不带品质描述的归一化值 21
            {
                ParseInformationShow = "测量值，不带品质描述的归一化值\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);
                LogHelper.Info(typeof(TelemeteringViewModel), "测量值，不带品质描述的归一化值");
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var msv = (MeasuredValueNormalizedWithoutQuality)asdu.GetElement(i);

                    Console.WriteLine("  IOA: " + msv.ObjectAddress + " scaled value: " + msv.NormalizedValue);
                    ParseInformationShow = "  IOA: " + msv.ObjectAddress + " scaled value: " + msv.NormalizedValue + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                }

            }
            else if (asdu.TypeId == TypeID.M_ME_TE_1)// 带CP56Time2a时标的测量值，标度化值 35
            {
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var msv = (MeasuredValueScaledWithCP56Time2a)asdu.GetElement(i);

                    Console.WriteLine("  IOA: " + msv.ObjectAddress + " scaled value: " + msv.ScaledValue);
                    Console.WriteLine("   " + msv.Quality.ToString());
                    Console.WriteLine("   " + msv.Timestamp.ToString());
                }

            }
            else if (asdu.TypeId == TypeID.M_ME_TF_1)// 带CP56Time2a时标的测量值，短浮点数 36
            {
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var mfv = (MeasuredValueShortWithCP56Time2a)asdu.GetElement(i);

                    Console.WriteLine("  IOA: " + mfv.ObjectAddress + " float value: " + mfv.Value);
                    Console.WriteLine("   " + mfv.Quality.ToString());
                    Console.WriteLine("   " + mfv.Timestamp.ToString());
                    Console.WriteLine("   " + mfv.Timestamp.GetDateTime().ToString());
                }
            }
            
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public TelemeteringViewModel()
        {
            IEC104.RegisterIEC104Handler(TypeID.M_ME_NA_1, this);//测量值，归一化值 09
            IEC104.RegisterIEC104Handler(TypeID.M_ME_NB_1, this);//测量值，标度化值 11
            IEC104.RegisterIEC104Handler(TypeID.M_ME_NC_1, this);//测量值，短浮点数 13
            IEC104.RegisterIEC104Handler(TypeID.M_ME_ND_1, this);//测量值，不带品质描述的归一化值 21
            IEC104.RegisterIEC104Handler(TypeID.M_ME_TE_1, this);//带CP56Time2a时标的测量值，标度化值 35
            IEC104.RegisterIEC104Handler(TypeID.M_ME_TF_1, this);//带CP56Time2a时标的测量值，短浮点数 36

            // 注册接收更新相应页面的点表消息(点表重新导入后，需要更新相应页面显示的点表)
            Messenger.Default.Register<object>(this, "UpdateSourcePoint", ReloadTelemeteringPoint);
            // 注册接收更新三遥页面的点表消息(配置点表下发后，需要更新三遥页面显示的点表)
            Messenger.Default.Register<object>(this, "UpdateUsedThreeRemotePoint", ReloadTelemeteringPoint);
            Messenger.Default.Register<string>(this, "teleMeteringPointMessage", ExcuteTeleMeteringPointError);//遥测信息
            this._timingInterval = 100;

            TimingGeneralInterrogationCommand = new RelayCommand(ExecuteTimingGeneralInterrogationCommand);
            CloseTimingGeneralInterrogationCommand = new RelayCommand(ExecuteCloseTimingGeneralInterrogationCommand);
            GeneralInterrogationCommand = new RelayCommand(ExecuteGeneralInterrogationCommand);

            // 未开启定时召唤
            IstimingGeneralInterrogation = false;

            //初始化遥测点号集合
            telemeteringData = new ObservableCollection<Telemetering>();
            telemeteringList = new List<TelemeteringList>();
            //重新载入点表
            ReloadTelemeteringPoint(null);
        }

        /// <summary>
        /// 重新载入遥测点表
        /// </summary>
        /// <param name="obj"></param>
        private void ReloadTelemeteringPoint(object obj)
        {
            TelemeteringManageService telemeteringManageService = new TelemeteringManageService();
            telemeteringData.Clear();
            // 获取使用的所有遥测点表
            IList<Telemetering> telemeteringList = telemeteringManageService.GetTelemeteringPoint(ConfigUtil.getPointTypeID("遥测"));

            if (telemeteringList != null && telemeteringList.Count > 0)
            {
                foreach (var telemetering in telemeteringList)
                {
                    telemeteringData.Add(telemetering);
                }
            }

        }

    }
}