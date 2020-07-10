using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System;
using FTU.Monitor.Model;
using FTU.Monitor.Util;
using lib60870;
using FTU.Monitor.Dao;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using FTU.Monitor.Service;
using FTU.Monitor.DataService;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Threading;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// TelesignalisationViewModel 的摘要说明
    /// author: songminghao
    /// date：2017/10/12 9:48:09
    /// desc：遥信ViewModel
    /// version: 1.0
    /// </summary>
    public class TelesignalisationViewModel : ViewModelBase, IIEC104Handler
    {
        /// <summary>
        /// 遥信点表集合
        /// </summary>
        public static ObservableCollection<Telesignalisation> telesignalisationData;

        /// <summary>
        /// 获取和设置遥信点表集合
        /// </summary>
        public ObservableCollection<Telesignalisation> TelesignalisationData
        {
            get
            {
                return telesignalisationData;
            }
            set
            {
                telesignalisationData = value;
                RaisePropertyChanged(() => TelesignalisationData);
            }
        }
        /// <summary>
        ///*** 需要打印的遥信集合***
        /// </summary>
        public static List<TelesignalisationList> telesignalisationList;
        public List<TelesignalisationList> TelesignalisationList
        {
            get { return telesignalisationList; }
            set
            {
                RaisePropertyChanged(() => TelesignalisationList);
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
        /// SOE计数器
        /// </summary>
        public static int soeCounter = 0;

        /// <summary>
        /// COS计数器
        /// </summary>
        public static int cosCounter = 0;

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
            if (CommunicationViewModel.IsLinkConnect())
            {
                CommunicationViewModel.con.SendInterrogationCommand(CauseOfTransmission.ACTIVATION, 1, QualifierOfInterrogation.STATION);
            }
        }

        /// <summary>
        /// 赋值遥信错误点号信息
        /// </summary>
        /// <param name="arg"></param>
        void ExcuteTeleSignalPointError(string arg)
        {
            TelesignalPointError = arg;
        }

        /// <summary>
        /// 接受处理遥信数据集合
        /// </summary>
        /// <param name="TI">遥信类型标识</param>
        /// <param name="asdu">对应遥信类型标识的ASDU</param>
        public void HandleASDUData(TypeID TI, ASDU asdu)
        {
            if (asdu.TypeId == TypeID.M_SP_NA_1)//单点信息 1
            {
                ParseInformationShow = "单点信息\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);

                // 不对应的遥测点表字符串，点号以逗号隔开
                string notExistsTelesignalisationPoint = "";
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var val = (SinglePointInformation)asdu.GetElement(i);
                    ParseInformationShow = "  IOA: " + val.ObjectAddress + "  IOA(HEX): " + val.ObjectAddress.ToString("X") + " SP value: " + val.Value + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    ParseInformationShow = "   " + val.Quality.ToString() + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    //将遥信数据添加到list
                    TelesignalisationList telesignalisation = new TelesignalisationList();
                    telesignalisation.ID = val.ObjectAddress;
                    telesignalisation.VALUE = val.Value;
                    telesignalisationList.Add(telesignalisation);

                    int telesignalisationPointIndex = val.ObjectAddress - 1;
                    if (telesignalisationPointIndex >= telesignalisationData.Count)
                    {
                        MainViewModel.outputdata.Debug += "遥信报文包含的点号越限:" + val.ObjectAddress.ToString("X4") + "\n";
                        notExistsTelesignalisationPoint += val.ObjectAddress.ToString("X4") + ",";
                        continue;
                    }

                    if (asdu.Cot == CauseOfTransmission.SPONTANEOUS)
                    {
                        // COS
                        COS cos = new COS();
                        cos.Number = cosCounter++;

                        StringBuilder sb = new StringBuilder();
                        cos.ID = sb.AppendFormat("{0:X4}", val.ObjectAddress).ToString();
                        cos.Content = telesignalisationData[val.ObjectAddress - 1].Name;
                        cos.Value = Convert.ToByte((val.Value == true) ? 1 : 0);
                        cos.Comment = (cos.Value == 1 ? "0 -> 1" : "1 -> 0");

                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (ThreadStart)delegate()
                        {
                            COSViewModel._COSData.Add(cos);
                        });
                    }

                    telesignalisationData[telesignalisationPointIndex].Value = (byte)((val.Value == true) ? 1 : 0);
                }

                // 判断不对应的遥信点表字符串是不是为空，不为空则提示异常信息
                if (!UtilHelper.IsEmpty(notExistsTelesignalisationPoint))
                {
                    string teleSignalPoint = "遥信报文包含的点号越限:" + notExistsTelesignalisationPoint.Substring(0, notExistsTelesignalisationPoint.Length - 1);
                    Messenger.Default.Send<string>(teleSignalPoint, "teleSignalPointError");
                }
                else
                {
                    Messenger.Default.Send<string>("", "teleSignalPointError");
                }
            }
            else if (asdu.TypeId == TypeID.M_DP_NA_1)//双点信息 3
            {
                ParseInformationShow = "双点信息\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var val = (DoublePointInformation)asdu.GetElement(i);

                    ParseInformationShow = "  IOA: " + val.ObjectAddress + "  IOA(HEX): " + val.ObjectAddress.ToString("X") + " DP value: " + val.Value + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    ParseInformationShow = "   " + val.Quality.ToString() + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);

                    if (asdu.Cot == CauseOfTransmission.SPONTANEOUS)
                    {
                        // COS
                        COS cos = new COS();
                        cos.Number = cosCounter++;

                        StringBuilder sb = new StringBuilder();
                        cos.ID = sb.AppendFormat("{0:X4}", val.ObjectAddress).ToString();
                        cos.Content = telesignalisationData[val.ObjectAddress - 1].Name;
                        cos.Value = (byte)val.Value;
                        cos.Comment = (cos.Value == 1 ? "2 -> 1" : "1 -> 2");
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (ThreadStart)delegate()
                        {
                            COSViewModel._COSData.Add(cos);
                        });
                    }

                    telesignalisationData[val.ObjectAddress - 1].Value = (byte)(val.Value);
                }
            }
            else if (asdu.TypeId == TypeID.M_SP_TB_1)//带CP56Time2a时标的单点信息 30
            {
                ParseInformationShow = "带CP56Time2a时标的单点信息\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var val = (SinglePointWithCP56Time2a)asdu.GetElement(i);

                    Console.WriteLine("  IOA: " + val.ObjectAddress + " SP value: " + val.Value);
                    Console.WriteLine("   " + val.Quality.ToString());
                    Console.WriteLine("   " + val.Timestamp.ToString());

                    ParseInformationShow = "  IOA: " + val.ObjectAddress + " SP value: " + val.Value + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    ParseInformationShow = "   " + val.Quality.ToString() + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    ParseInformationShow = "   " + val.Timestamp.ToString() + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);

                    string result = "";
                    try
                    {
                        TelesignalisationViewModel.telesignalisationData[val.ObjectAddress - 1].Value = (byte)((val.Value == true) ? 1 : 0);
                        result = TelesignalisationViewModel.telesignalisationData[val.ObjectAddress - 1].Name;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("报异常:" + ex.Message);
                        MainViewModel.outputdata.Debug += "未知遥信点号！\n";
                        LogHelper.Error(typeof(TelesignalisationViewModel),"解析遥信点号出现异常 \n" + ex.Message);
                    }

                    #region 更新SOE显示

                    SOE t = new SOE();
                    t.Number = soeCounter++;

                    StringBuilder sb = new StringBuilder();
                    t.ID = sb.AppendFormat("{0:X4}", val.ObjectAddress).ToString();

                    t.Time = val.Timestamp.ToStringDateTime();
                    t.Content = result;
                    t.Value = Convert.ToByte((val.Value == true) ? 1 : 0);
                    t.Comment = (t.Value == 1 ? "0 -> 1" : "1 -> 0");

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate()
                    {
                        SOEViewModel._SOEData.Add(t);
                    });

                    #endregion
                }
            }
            else if (asdu.TypeId == TypeID.M_DP_TB_1)//带CP56Time2a时标的双点信息 31
            {
                ParseInformationShow = "带CP56Time2a时标的双点信息\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var val = (DoublePointWithCP56Time2a)asdu.GetElement(i);

                    Console.WriteLine("  IOA: " + val.ObjectAddress + " DP value: " + val.Value);
                    Console.WriteLine("   " + val.Quality.ToString());
                    Console.WriteLine("   " + val.Timestamp.ToString());

                    ParseInformationShow = "  IOA: " + val.ObjectAddress + " DP value: " + val.Value + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    ParseInformationShow = "   " + val.Quality.ToString() + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    ParseInformationShow = "   " + val.Timestamp.ToString() + "\n";
                    ShowMessage.ParseInformationShow(ParseInformationShow);

                    string result = "";
                    try
                    {
                        TelesignalisationViewModel.telesignalisationData[val.ObjectAddress - 1].Value = (byte)(val.Value);
                        result = TelesignalisationViewModel.telesignalisationData[val.ObjectAddress - 1].Name;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("报异常:" + ex.Message);
                        MainViewModel.outputdata.Debug += "未知遥信点号！\n";
                        LogHelper.Error(typeof(TelesignalisationViewModel), "未知遥信点号 \n" + ex.Message);
                    }

                    #region 更新SOE显示
                    SOE t = new SOE();
                    t.Number = soeCounter++;

                    StringBuilder sb = new StringBuilder();
                    t.ID = sb.AppendFormat("{0:X4}", val.ObjectAddress).ToString();

                    t.Time = val.Timestamp.ToStringDateTime();
                    t.Content = result;
                    t.Value = Convert.ToByte(val.Value);
                    t.Comment = (t.Value == 1 ? "2 -> 1" : "1 -> 2");

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate()
                    {
                        SOEViewModel._SOEData.Add(t);
                    });

                    #endregion
                }
            }
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public TelesignalisationViewModel()
        {
            IEC104.RegisterIEC104Handler(TypeID.M_SP_NA_1, this);//单点信息 1
            IEC104.RegisterIEC104Handler(TypeID.M_DP_NA_1, this);//双点信息 3
            IEC104.RegisterIEC104Handler(TypeID.M_SP_TB_1, this);//带CP56Time2a时标的单点信息 30
            IEC104.RegisterIEC104Handler(TypeID.M_DP_TB_1, this);//带CP56Time2a时标的双点信息 31

            // 注册接收更新相应页面的点表消息(点表重新导入后，需要更新相应页面显示的点表)
            Messenger.Default.Register<object>(this, "UpdateSourcePoint", ReloadTelesignalisationPoint);
            // 注册接收更新三遥页面的点表消息(配置点表下发后，需要更新三遥页面显示的点表)
            Messenger.Default.Register<object>(this, "UpdateUsedThreeRemotePoint", ReloadTelesignalisationPoint);
            Messenger.Default.Register<string>(this, "teleSignalPointError", ExcuteTeleSignalPointError);


            GeneralInterrogationCommand = new RelayCommand(ExecuteGeneralInterrogationCommand);

            //初始化遥信点号集合
            telesignalisationData = new ObservableCollection<Telesignalisation>();
            telesignalisationList = new List<TelesignalisationList>();
            //重新载入点表
            ReloadTelesignalisationPoint(null);
            SOEViewModel._SOEData = new ObservableCollection<SOE>();

        }

        /// <summary>
        /// 遥信错误点号信息
        /// </summary>
        private string _telesignalPointError;

        /// <summary>
        /// 设置和获取遥信错误点号信息
        /// </summary>
        public string TelesignalPointError
        {
            get
            {
                return this._telesignalPointError;
            }
            set
            {
                this._telesignalPointError = value;
                RaisePropertyChanged(() => TelesignalPointError);
            }
        }

        /// <summary>
        /// 重新载入遥信点表
        /// </summary>
        /// <param name="obj"></param>
        private void ReloadTelesignalisationPoint(object obj)
        {
            TelesignalisationManageService telesignalisationManageService = new TelesignalisationManageService();
            telesignalisationData.Clear();
            // 获取使用的所有遥信点表
            IList<Telesignalisation> telesignalisationList = telesignalisationManageService.GetTelesignalisationPoint(ConfigUtil.getPointTypeID("遥信"));

            if (telesignalisationList != null && telesignalisationList.Count > 0)
            {
                foreach (var telesignalisation in telesignalisationList)
                {
                    telesignalisationData.Add(telesignalisation);
                }
            }

        }

    }
}