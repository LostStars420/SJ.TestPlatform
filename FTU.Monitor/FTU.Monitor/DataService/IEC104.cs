using lib60870;
using FTU.Monitor.lib60870;
using FTU.Monitor.Model;
using FTU.Monitor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using FTU.Monitor.Lib60870;
using FTU.Monitor.Util;
using GalaSoft.MvvmLight.Messaging;
using FTU.Monitor.Dao;

namespace FTU.Monitor.DataService
{
    public class IEC104
    {
        /// <summary>
        /// 注册事件键值对
        /// </summary>
       private static Dictionary<TypeID, IIEC104Handler> handler  = new Dictionary<TypeID,IIEC104Handler>();

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="TI">报文类型标识</param>
        /// <param name="handlerList">处理事件</param>
        public static void RegisterIEC104Handler(TypeID TI, IIEC104Handler handlerList)
        {
            if(!handler.ContainsKey(TI))
            {
                handler.Add(TI, handlerList);
            }
        }

        /// <summary>
        /// 通知事件
        /// </summary>
        /// <param name="TI">报文类型标识</param>
        /// <param name="asdu">ASDU数据包</param>
        private static void Notify(TypeID TI, ASDU asdu)
        {
            if (!handler.ContainsKey(TI))
            {
                return;
            }
            foreach (KeyValuePair<TypeID, IIEC104Handler> kvp in handler)
            {
                if(kvp.Key.Equals(TI))
                {
                    kvp.Value.HandleASDUData(TI, asdu);
                }
            }           
        }

        public static void ConnectionHandler(object parameter, ConnectionEvent connectionEvent)
        {
            switch (connectionEvent)
            {
                case ConnectionEvent.OPENED:
                    Console.WriteLine("Connected");
                    break;
                case ConnectionEvent.CLOSED:
                    Console.WriteLine("Connection closed");
                    break;
                case ConnectionEvent.STARTDT_CON_RECEIVED:
                    Console.WriteLine("STARTDT CON received");
                    break;
                case ConnectionEvent.STOPDT_CON_RECEIVED:
                    Console.WriteLine("STOPDT CON received");
                    break;
            }
        }

        /// <summary>
        /// ASDU数据单元处理
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="asdu">asdu数据</param>
        /// <returns></returns>
        public static bool asduReceivedHandler(object parameter, ASDU asdu)
        {
            string parseInformationShow = "";
            try
            {
                switch(asdu.TypeId)
                {
                    // 单点信息 1
                    case TypeID.M_SP_NA_1:
                        Notify(TypeID.M_SP_NA_1, asdu);
                        break;
                    // 双点信息 3
                    case TypeID.M_DP_NA_1:
                        Notify(TypeID.M_DP_NA_1, asdu);
                        break;
                    // 测量值，归一化值 09
                    case TypeID.M_ME_NA_1:
                        Notify(TypeID.M_ME_NA_1, asdu);
                        break;
                    // 测量值，标度化值 11
                    case TypeID.M_ME_NB_1:
                        Notify(TypeID.M_ME_NB_1, asdu);
                        break;
                    // 测量值，短浮点数 13
                    case TypeID.M_ME_NC_1:
                        Notify(TypeID.M_ME_NC_1, asdu);
                        break;
                    // 测量值，不带品质描述的归一化值 21
                    case TypeID.M_ME_ND_1:
                        Notify(TypeID.M_ME_ND_1, asdu);
                        break;
                    // 带CP56Time2a时标的单点信息 30
                    case TypeID.M_SP_TB_1:
                        Notify(TypeID.M_SP_TB_1, asdu);
                        break;
                    // 带CP56Time2a时标的双点信息 31
                    case TypeID.M_DP_TB_1:
                        Notify(TypeID.M_DP_TB_1, asdu);
                        break;
                    // 带CP56Time2a时标的测量值，标度化值 35
                    case TypeID.M_ME_TE_1: 
                        Notify(TypeID.M_ME_TE_1, asdu);
                        break;
                    // 带CP56Time2a时标的测量值，短浮点数 36
                    case TypeID.M_ME_TF_1:
                        Notify(TypeID.M_ME_TF_1, asdu);
                        break;
                    // 故障值信息 42
                    case TypeID.M_FT_NA_1:
                        Notify(TypeID.M_FT_NA_1, asdu);
                        break;
                     // 单点命令 45
                    case TypeID.C_SC_NA_1:
                        Notify(TypeID.C_SC_NA_1, asdu);
                        break;
                    // 双点命令 46
                    case TypeID.C_DC_NA_1:
                        Notify(TypeID.C_DC_NA_1, asdu);
                        break;
                    // 初始化结束 70
                    case TypeID.M_EI_NA_1:
                        Notify(TypeID.M_EI_NA_1, asdu);
                        break;
                    // 总召唤 100
                    case TypeID.C_IC_NA_1: 
                        Notify(TypeID.C_IC_NA_1, asdu);
                        break;
                    // 时钟同步命令 103
                    case TypeID.C_CS_NA_1:
                        Notify(TypeID.C_CS_NA_1, asdu);
                        break;
                    // 读定值区号 201
                    case TypeID.C_RR_NA_1:
                        Notify(TypeID.C_RR_NA_1, asdu);
                        break;
                    // 读参数和定值 202
                    case TypeID.C_RS_NA_1:
                        Notify(TypeID.C_RS_NA_1, asdu);
                        break;
                    // 写参数和定值 203
                    case TypeID.C_WS_NA_1:
                        Notify(TypeID.C_WS_NA_1, asdu);
                        break;
                     // 累计量，短浮点数
                    case TypeID.M_IT_NB_1: 
                        Notify(TypeID.M_IT_NB_1, asdu);
                        break;
                    // 文件传输 210
                    case TypeID.F_FR_NA_1:
                        Notify(TypeID.F_FR_NA_1, asdu);
                        break;
                    // 软件升级 211
                    case TypeID.F_SR_NA_1:
                        Notify(TypeID.F_SR_NA_1, asdu);
                        break;
                    default:
                        Console.WriteLine("Unknown message type!");
                        parseInformationShow = "Unknown message type!" + "\n";
                        ShowMessage.ParseInformationShow(parseInformationShow);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("解析ASDU:" + ex.Message);
                LogHelper.Warn(typeof(IEC104), "解析ASDU:" + ex.ToString());
            }
            return true;
        }


        //public static bool asduReceivedHandler(object parameter, ASDU asdu)
        //{
        //    try
        //    {
        //        if (asdu.TypeId == TypeID.M_SP_NA_1)//单点信息 1
        //        {
        //            //ShowMessage.ShowFunction("单点信息");
        //            //MainViewModel.outputdata.PraseInformation += "单点信息\n";
        //            praseInformationShow = "单点信息\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);

        //            // 不对应的遥测点表字符串，点号以逗号隔开
        //            string notExistsTelesignalisationPoint = "";
        //            for (int i = 0; i < asdu.NumberOfElements; i++)
        //            {
        //                var val = (SinglePointInformation)asdu.GetElement(i);

        //                // Console.WriteLine("  IOA: " + val.ObjectAddress + " SP value: " + val.Value);
        //                // Console.WriteLine("   " + val.Quality.ToString());
        //                //MainViewModel.outputdata.PraseInformation += "  IOA: " + val.ObjectAddress + "  IOA(HEX): " + val.ObjectAddress.ToString("X") + " SP value: " + val.Value + "\n";
        //                praseInformationShow = "  IOA: " + val.ObjectAddress + "  IOA(HEX): " + val.ObjectAddress.ToString("X") + " SP value: " + val.Value + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                //MainViewModel.outputdata.PraseInformation += "   " + val.Quality.ToString() + "\n";
        //                praseInformationShow = "   " + val.Quality.ToString() + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);

        //                int telesignalisationPointIndex = val.ObjectAddress - 1;
        //                if (telesignalisationPointIndex >= TelesignalisationViewModel.telesignalisationData.Count)
        //                {
        //                    MainViewModel.outputdata.Debug += "遥信报文包含的点号越限:" + val.ObjectAddress.ToString("X4") + "\n";
        //                    notExistsTelesignalisationPoint += val.ObjectAddress.ToString("X4") + ",";
        //                    // MessageBox.Show("遥信报文包含的点号越限:" + val.ObjectAddress.ToString("X4"), "异常");
        //                    continue;
        //                }

        //                TelesignalisationViewModel.telesignalisationData[telesignalisationPointIndex].Value = (byte)((val.Value == true) ? 1 : 0);
        //            }

        //            // 判断不对应的遥信点表字符串是不是为空，不为空则提示异常信息
        //            if (!UtilHelper.IsEmpty(notExistsTelesignalisationPoint))
        //            {
        //                //MessageBox.Show("遥信报文包含的点号越限:" + notExistsTelesignalisationPoint.Substring(0, notExistsTelesignalisationPoint.Length - 1), "异常");
        //                string teleSignalPoint = "遥信报文包含的点号越限:" + notExistsTelesignalisationPoint.Substring(0, notExistsTelesignalisationPoint.Length - 1);
        //                Messenger.Default.Send<string>(teleSignalPoint, "teleSignalPointError");
        //            }
        //            else
        //            {
        //                Messenger.Default.Send<string>("", "teleSignalPointError");
        //            }
        //        }
        //        else if (asdu.TypeId == TypeID.M_DP_NA_1)//双点信息 3
        //        {
        //            praseInformationShow = "双点信息\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);
        //            for (int i = 0; i < asdu.NumberOfElements; i++)
        //            {
        //                var val = (DoublePointInformation)asdu.GetElement(i);

        //                praseInformationShow = "  IOA: " + val.ObjectAddress + "  IOA(HEX): " + val.ObjectAddress.ToString("X") + " DP value: " + val.Value + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                praseInformationShow = "   " + val.Quality.ToString() + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);

        //                TelesignalisationViewModel.telesignalisationData[val.ObjectAddress - 1].Value = (byte)(val.Value);
        //            }
        //        }
        //        else if (asdu.TypeId == TypeID.M_ME_NA_1)//测量值，归一化值 09
        //        {
        //            //ShowMessage.ShowFunction("测量值，归一化值");
        //            //MainViewModel.outputdata.PraseInformation += "测量值，归一化值\n";
        //            praseInformationShow = "测量值，归一化值\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);
        //            for (int i = 0; i < asdu.NumberOfElements; i++)
        //            {
        //                var msv = (MeasuredValueNormalized)asdu.GetElement(i);

        //                // Console.WriteLine("  IOA: " + msv.ObjectAddress + " scaled value: " + msv.NormalizedValue);
        //                //MainViewModel.outputdata.PraseInformation += "  IOA: " + msv.ObjectAddress + "  IOA(HEX): " + msv.ObjectAddress.ToString("X") + " scaled value: " + msv.NormalizedValue + "\n";
        //                praseInformationShow = "  IOA: " + msv.ObjectAddress + "  IOA(HEX): " + msv.ObjectAddress.ToString("X") + " scaled value: " + msv.NormalizedValue + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);

        //                Telemetering telemetering = TelemeteringViewModel.telemeteringData[msv.ObjectAddress - 0x4001];
        //                telemetering.Rate = telemetering.Rate <= 1 ? 1 : telemetering.Rate;

        //                TelemeteringViewModel.telemeteringData[msv.ObjectAddress - 0x4001].Value = msv.NormalizedValue * telemetering.Rate;
        //                if (CoefficientViewModel.number != 0)//校准模式
        //                {
        //                    for (int j = 0; j < 9; j++)
        //                    {
        //                        CoefficientViewModel.userData[j].Data[CoefficientViewModel.number - 1] = msv.NormalizedValue;
        //                        float total = 0;
        //                        for (int k = 0; k < CoefficientViewModel.number; k++)
        //                        {
        //                            total += CoefficientViewModel.userData[j].Data[k];
        //                        }
        //                        CoefficientViewModel.userData[j].AverageValue = total / (CoefficientViewModel.number + 1);
        //                    }
        //                }
        //            }
        //        }
        //        else if (asdu.TypeId == TypeID.M_ME_NB_1)//测量值，标度化值 11
        //        {
        //            //ShowMessage.ShowFunction("测量值，标度化值");
        //            //MainViewModel.outputdata.PraseInformation += "测量值，标度化值\n";
        //            praseInformationShow = "测量值，标度化值\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);
        //            for (int i = 0; i < asdu.NumberOfElements; i++)
        //            {
        //                var msv = (MeasuredValueScaled)asdu.GetElement(i);

        //                Console.WriteLine("  IOA: " + msv.ObjectAddress + " scaled value: " + msv.ScaledValue);
        //                Console.WriteLine("   " + msv.Quality.ToString());

        //                //MainViewModel.outputdata.PraseInformation += "  IOA: " + msv.ObjectAddress + " scaled value: " + msv.ScaledValue + "\n";
        //                praseInformationShow = "  IOA: " + msv.ObjectAddress + " scaled value: " + msv.ScaledValue + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);

        //                Telemetering telemetering = TelemeteringViewModel.telemeteringData[msv.ObjectAddress - 0x4001];
        //                telemetering.Rate = telemetering.Rate <= 1 ? 1 : telemetering.Rate;

        //                TelemeteringViewModel.telemeteringData[msv.ObjectAddress - 0x4001].Value = msv.ScaledValue.ShortValue / telemetering.Rate;
        //            }

        //        }
        //        else if (asdu.TypeId == TypeID.M_ME_NC_1)//测量值，短浮点数 13
        //        {
        //            //ShowMessage.ShowFunction("测量值，短浮点数");
        //            //MainViewModel.outputdata.PraseInformation += "测量值，短浮点数\n";
        //            praseInformationShow = "测量值，短浮点数\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);

        //            // 不对应的遥测点表字符串，点号以逗号隔开
        //            string notExistsTelemeteringPoint = "";
        //            for (int i = 0; i < asdu.NumberOfElements; i++)
        //            {
        //                var mfv = (MeasuredValueShort)asdu.GetElement(i);

        //                // Console.WriteLine("  IOA: " + mfv.ObjectAddress + " float value: " + mfv.Value);
        //                // Console.WriteLine("   " + mfv.Quality.ToString());

        //                //MainViewModel.outputdata.PraseInformation += "  IOA: " + mfv.ObjectAddress + "  IOA(HEX): " + mfv.ObjectAddress.ToString("X") + " float value: " + mfv.Value + "\n";
        //                praseInformationShow = "  IOA: " + mfv.ObjectAddress + "  IOA(HEX): " + mfv.ObjectAddress.ToString("X") + " float value: " + mfv.Value + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);

        //                int telemeteringPointIndex = mfv.ObjectAddress - 0x4001;
        //                if (telemeteringPointIndex >= TelemeteringViewModel.telemeteringData.Count)
        //                {
        //                    MainViewModel.outputdata.Debug += "遥测报文包含的点号越限:" + mfv.ObjectAddress.ToString("X4") + "\n";
        //                    notExistsTelemeteringPoint += mfv.ObjectAddress.ToString("X4") + ",";
        //                    // MessageBox.Show("遥测报文包含的点号越限:" + mfv.ObjectAddress.ToString("X4"), "异常");
        //                    continue;
        //                }

        //                TelemeteringViewModel.telemeteringData[telemeteringPointIndex].Value = mfv.Value;
        //                if (CoefficientViewModel.number != 0)//校准模式
        //                {
        //                    for (int j = 0; j < 9; j++)
        //                    {
        //                        CoefficientViewModel.userData[j].Data[CoefficientViewModel.number - 1] = mfv.Value;
        //                        float total = 0;
        //                        for (int k = 0; k < CoefficientViewModel.number; k++)
        //                        {
        //                            total += CoefficientViewModel.userData[j].Data[k];
        //                        }
        //                        CoefficientViewModel.userData[j].AverageValue = total / (CoefficientViewModel.number + 1);
        //                    }
        //                }
        //            }

        //            // 判断不对应的遥测点表字符串是不是为空，不为空则提示异常信息
        //            if (!UtilHelper.IsEmpty(notExistsTelemeteringPoint))
        //            {
        //                //MessageBox.Show("遥测报文包含的点号越限:" + notExistsTelemeteringPoint.Substring(0, notExistsTelemeteringPoint.Length - 1), "异常");
        //                string telemeteringPointMessage = "遥测报文包含的点号越限:" + notExistsTelemeteringPoint.Substring(0, notExistsTelemeteringPoint.Length - 1);
        //                Messenger.Default.Send<string>(telemeteringPointMessage, "teleMeteringPointMessage");
        //            }
        //            else
        //            {
        //                Messenger.Default.Send<string>("", "teleMeteringPointMessage");
        //            }

        //        }
        //        else if (asdu.TypeId == TypeID.M_ME_ND_1)// 测量值，不带品质描述的归一化值 21
        //        {
        //            //ShowMessage.ShowFunction("测量值，不带品质描述的归一化值");
        //            //MainViewModel.outputdata.PraseInformation += "测量值，不带品质描述的归一化值\n";
        //            praseInformationShow = "测量值，不带品质描述的归一化值\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);
        //            for (int i = 0; i < asdu.NumberOfElements; i++)
        //            {

        //                var msv = (MeasuredValueNormalizedWithoutQuality)asdu.GetElement(i);

        //                Console.WriteLine("  IOA: " + msv.ObjectAddress + " scaled value: " + msv.NormalizedValue);
        //                //MainViewModel.outputdata.PraseInformation += "  IOA: " + msv.ObjectAddress + " scaled value: " + msv.NormalizedValue + "\n";
        //                praseInformationShow = "  IOA: " + msv.ObjectAddress + " scaled value: " + msv.NormalizedValue + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //            }

        //        }
        //        else if (asdu.TypeId == TypeID.M_SP_TB_1)//带CP56Time2a时标的单点信息 30
        //        {
        //            //ShowMessage.ShowFunction("带CP56Time2a时标的单点信息");
        //            //MainViewModel.outputdata.PraseInformation += "带CP56Time2a时标的单点信息\n";
        //            praseInformationShow = "带CP56Time2a时标的单点信息\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);
        //            for (int i = 0; i < asdu.NumberOfElements; i++)
        //            {

        //                var val = (SinglePointWithCP56Time2a)asdu.GetElement(i);

        //                Console.WriteLine("  IOA: " + val.ObjectAddress + " SP value: " + val.Value);
        //                Console.WriteLine("   " + val.Quality.ToString());
        //                Console.WriteLine("   " + val.Timestamp.ToString());

        //                //MainViewModel.outputdata.PraseInformation += "  IOA: " + val.ObjectAddress + " SP value: " + val.Value + "\n";
        //                praseInformationShow = "  IOA: " + val.ObjectAddress + " SP value: " + val.Value + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                //MainViewModel.outputdata.PraseInformation += "   " + val.Quality.ToString() + "\n";
        //                praseInformationShow = "   " + val.Quality.ToString() + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                //MainViewModel.outputdata.PraseInformation += "   " + val.Timestamp.ToString() + "\n";
        //                praseInformationShow = "   " + val.Timestamp.ToString() + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);

        //                string result = "";
        //                try
        //                {
        //                    TelesignalisationViewModel.telesignalisationData[val.ObjectAddress - 1].Value = (byte)((val.Value == true) ? 1 : 0);
        //                    result = TelesignalisationViewModel.telesignalisationData[val.ObjectAddress - 1].Name;
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine("报异常:" + ex.Message);
        //                    MainViewModel.outputdata.Debug += "未知遥信点号！\n";
        //                }

        //                #region 更新SOE显示

        //                SOE t = new SOE();
        //                t.Number = soeCounter++;
        //                //t.Number = SOEViewModel._SOEData.Count + 1;

        //                StringBuilder sb = new StringBuilder();
        //                t.ID = sb.AppendFormat("{0:X4}", val.ObjectAddress).ToString();

        //                t.Time = val.Timestamp.ToStringDateTime();
        //                t.Content = result;
        //                t.Value = Convert.ToByte((val.Value == true) ? 1 : 0);
        //                t.Comment = (t.Value == 1 ? "0 -> 1" : "1 -> 0");

        //                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
        //                (ThreadStart)delegate()
        //                {
        //                    SOEViewModel._SOEData.Add(t);
        //                });

        //                #endregion
        //            }
        //        }
        //        else if (asdu.TypeId == TypeID.M_DP_TB_1)//带CP56Time2a时标的双点信息 31
        //        {
        //            praseInformationShow = "带CP56Time2a时标的双点信息\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);
        //            for (int i = 0; i < asdu.NumberOfElements; i++)
        //            {

        //                var val = (DoublePointWithCP56Time2a)asdu.GetElement(i);

        //                Console.WriteLine("  IOA: " + val.ObjectAddress + " DP value: " + val.Value);
        //                Console.WriteLine("   " + val.Quality.ToString());
        //                Console.WriteLine("   " + val.Timestamp.ToString());

        //                praseInformationShow = "  IOA: " + val.ObjectAddress + " DP value: " + val.Value + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                praseInformationShow = "   " + val.Quality.ToString() + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                praseInformationShow = "   " + val.Timestamp.ToString() + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);

        //                string result = "";
        //                try
        //                {
        //                    TelesignalisationViewModel.telesignalisationData[val.ObjectAddress - 1].Value = (byte)(val.Value);
        //                    result = TelesignalisationViewModel.telesignalisationData[val.ObjectAddress - 1].Name;
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine("报异常:" + ex.Message);
        //                    MainViewModel.outputdata.Debug += "未知遥信点号！\n";
        //                }

        //                #region 更新SOE显示
        //                SOE t = new SOE();
        //                t.Number = soeCounter++;

        //                StringBuilder sb = new StringBuilder();
        //                t.ID = sb.AppendFormat("{0:X4}", val.ObjectAddress).ToString();

        //                t.Time = val.Timestamp.ToStringDateTime();
        //                t.Content = result;
        //                t.Value = Convert.ToByte(val.Value);
        //                t.Comment = (t.Value == 1 ? "2 -> 1" : "1 -> 2");

        //                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
        //                (ThreadStart)delegate()
        //                {
        //                    SOEViewModel._SOEData.Add(t);
        //                });

        //                #endregion
        //            }
        //        }
        //        else if (asdu.TypeId == TypeID.M_ME_TE_1)// 带CP56Time2a时标的测量值，标度化值 35
        //        {
        //            //ShowMessage.ShowFunction("带CP56Time2a时标的测量值，标度化值");
        //            for (int i = 0; i < asdu.NumberOfElements; i++)
        //            {

        //                var msv = (MeasuredValueScaledWithCP56Time2a)asdu.GetElement(i);

        //                Console.WriteLine("  IOA: " + msv.ObjectAddress + " scaled value: " + msv.ScaledValue);
        //                Console.WriteLine("   " + msv.Quality.ToString());
        //                Console.WriteLine("   " + msv.Timestamp.ToString());
        //            }

        //        }
        //        else if (asdu.TypeId == TypeID.M_ME_TF_1)// 带CP56Time2a时标的测量值，短浮点数 36
        //        {
        //            //ShowMessage.ShowFunction("带CP56Time2a时标的测量值，短浮点数");
        //            for (int i = 0; i < asdu.NumberOfElements; i++)
        //            {
        //                var mfv = (MeasuredValueShortWithCP56Time2a)asdu.GetElement(i);

        //                Console.WriteLine("  IOA: " + mfv.ObjectAddress + " float value: " + mfv.Value);
        //                Console.WriteLine("   " + mfv.Quality.ToString());
        //                Console.WriteLine("   " + mfv.Timestamp.ToString());
        //                Console.WriteLine("   " + mfv.Timestamp.GetDateTime().ToString());
        //            }
        //        }
        //        else if (asdu.TypeId == TypeID.M_FT_NA_1)// 故障值信息 42
        //        {
        //            //ShowMessage.ShowFunction("故障值信息");
        //            //MainViewModel.outputdata.PraseInformation += "故障值信息 \n";
        //            praseInformationShow = "故障值信息 \n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);

        //            var val = (SinglePointWithCP56Time2a)asdu.GetElement(0);

        //        }
        //        else if (asdu.TypeId == TypeID.C_SC_NA_1)// 单点命令 45
        //        {

        //            if (asdu.Cot == CauseOfTransmission.ACTIVATION_CON)
        //            {
        //                //ShowMessage.ShowFunction("单点命令激活确认");
        //                Console.WriteLine((asdu.IsNegative ? "Negative" : "Positive") + "confirmation for Single command");
        //                //MainViewModel.outputdata.PraseInformation += (asdu.IsNegative ? "Negative" : "Positive") + "confirmation for Single command" + "\n";
        //                praseInformationShow = (asdu.IsNegative ? "Negative" : "Positive") + "confirmation for Single command" + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationConfirmationReceived++;
        //            }
        //            else if (asdu.Cot == CauseOfTransmission.ACTIVATION_TERMINATION)
        //            {
        //                //ShowMessage.ShowFunction("单点命令激活终止");
        //                Console.WriteLine("Single command terminated");
        //                //MainViewModel.outputdata.PraseInformation += "Single command terminated\n";
        //                praseInformationShow = "Single command terminated\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationTerminationReceived++;
        //            }
        //            else if (asdu.Cot == CauseOfTransmission.UNKNOWN_TYPE_ID)
        //            {
        //                //未知的类型标识
        //                Console.WriteLine("单点遥控：未知的类型标识 unknownTypeID = 44");
        //                praseInformationShow = " Unknown TypeId of telecontrol\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationTerminationReceived++;
        //                MessageBox.Show("失败原因：未知的类型标识 TI = 44", "单点遥控操作失败");
        //            }
        //            else if (asdu.Cot == CauseOfTransmission.UNKNOWN_CAUSE_OF_TRANSMISSION)
        //            {
        //                //未知的传送原因 UnknownTypeCaseTransmission =  45
        //                Console.WriteLine("单点遥控：未知的传送原因 UnknownTypeCaseTransmission =  45");
        //                praseInformationShow = " Unknown COT of telecontrol\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationTerminationReceived++;
        //                MessageBox.Show("失败原因：未知的类型标识 COT = 45", "单点遥控操作失败");
        //            }
        //            else if (asdu.Cot == CauseOfTransmission.UNKNOWN_COMMON_ADDRESS_OF_ASDU)
        //            {
        //                //未知的应用服务数据单元公共地址 UnknownAppDataPublicAddress= 46
        //                Console.WriteLine("单点遥控：未知的应用服务数据单元公共地址 UnknownAppDataPublicAddress = 46");
        //                praseInformationShow = " Unknown ASDU common address of telecontrol\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationTerminationReceived++;
        //                MessageBox.Show("失败原因：未知的类型标识 CA = 46", "单点遥控操作失败");
        //            }
        //            else if (asdu.Cot == CauseOfTransmission.UNKNOWN_INFORMATION_OBJECT_ADDRESS)
        //            {
        //                //未知信息对象地址 UnknownInformationObjectAddress= 47
        //                Console.WriteLine("单点遥控：未知信息对象地址 UnknownInformationObjectAddress= 47");
        //                praseInformationShow = " Unknown infomation object address of telecontrol\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationTerminationReceived++;
        //                MessageBox.Show("失败原因：未知的类型标识 infomation object address = 47", "单点遥控操作失败");
        //            }

        //            var sc = (SingleCommand)asdu.GetElement(0);
        //            Console.WriteLine("  IOA: " + sc.ObjectAddress + " state : " + sc.State);
        //            //MainViewModel.outputdata.PraseInformation += "  IOA: " + sc.ObjectAddress + " state : " + sc.State + "\n";
        //            praseInformationShow = "  IOA: " + sc.ObjectAddress + " state : " + sc.State + "\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);

        //        }
        //        else if (asdu.TypeId == TypeID.C_DC_NA_1)// 双点命令 46
        //        {

        //            if (asdu.Cot == CauseOfTransmission.ACTIVATION_CON)
        //            {
        //                //ShowMessage.ShowFunction("双点命令激活确认");
        //                Console.WriteLine((asdu.IsNegative ? "Negative" : "Positive") + "confirmation for Double command");
        //                //MainViewModel.outputdata.PraseInformation += (asdu.IsNegative ? "Negative" : "Positive") + "confirmation for Double command" + "\n";
        //                praseInformationShow = (asdu.IsNegative ? "Negative" : "Positive") + "confirmation for Double command" + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationConfirmationReceived++;
        //            }
        //            else if (asdu.Cot == CauseOfTransmission.ACTIVATION_TERMINATION)
        //            {
        //                //ShowMessage.ShowFunction("双点命令激活终止");
        //                Console.WriteLine("Double command terminated");
        //                //MainViewModel.outputdata.PraseInformation += "Double command terminated" + "\n";
        //                praseInformationShow = "Double command terminated" + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationTerminationReceived++;
        //            }
        //            else if (asdu.Cot == CauseOfTransmission.UNKNOWN_TYPE_ID)
        //            {
        //                //未知的类型标识
        //                Console.WriteLine("双点遥控：未知的类型标识 unknownTypeID = 44");
        //                praseInformationShow = " Unknown TypeId of telecontrol\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationTerminationReceived++;
        //                MessageBox.Show("失败原因：未知的类型标识 TI = 44", "双点遥控操作失败");
        //            }
        //            else if (asdu.Cot == CauseOfTransmission.UNKNOWN_CAUSE_OF_TRANSMISSION)
        //            {
        //                //未知的传送原因 UnknownTypeCaseTransmission =  45
        //                Console.WriteLine("双点遥控：未知的传送原因 UnknownTypeCaseTransmission =  45");
        //                praseInformationShow = " Unknown COT of telecontrol\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationTerminationReceived++;
        //                MessageBox.Show("失败原因：未知的类型标识 COT = 45", "双点遥控操作失败");
        //            }
        //            else if (asdu.Cot == CauseOfTransmission.UNKNOWN_COMMON_ADDRESS_OF_ASDU)
        //            {
        //                //未知的应用服务数据单元公共地址 UnknownAppDataPublicAddress= 46
        //                Console.WriteLine("双点遥控：未知的应用服务数据单元公共地址 UnknownAppDataPublicAddress = 46");
        //                praseInformationShow = " Unknown ASDU common address of telecontrol\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationTerminationReceived++;
        //                MessageBox.Show("失败原因：未知的类型标识 CA = 46", "双点遥控操作失败");
        //            }
        //            else if (asdu.Cot == CauseOfTransmission.UNKNOWN_INFORMATION_OBJECT_ADDRESS)
        //            {
        //                //未知信息对象地址 UnknownInformationObjectAddress= 47
        //                Console.WriteLine("双点遥控：未知信息对象地址 UnknownInformationObjectAddress= 47");
        //                praseInformationShow = " Unknown infomation object address of telecontrol\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationTerminationReceived++;
        //                MessageBox.Show("失败原因：未知的类型标识 IOA = 47", "双点遥控操作失败");
        //            }
        //            var dc = (DoubleCommand)asdu.GetElement(0);

        //            Console.WriteLine("  IOA: " + dc.ObjectAddress + " state : " + dc.State);
        //            //MainViewModel.outputdata.PraseInformation += "  IOA: " + dc.ObjectAddress + " state : " + dc.State + "\n";
        //            praseInformationShow = "  IOA: " + dc.ObjectAddress + " state : " + dc.State + "\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);

        //        }
        //        else if (asdu.TypeId == TypeID.M_EI_NA_1)// 初始化结束 70
        //        {
        //            //读取终端设备运行的软件版本号并做处理
        //            InherentParameterViewModel.CheckProgrammVersion();
        //            //ShowMessage.ShowFunction("初始化结束");
        //        }
        //        else if (asdu.TypeId == TypeID.C_IC_NA_1) // 总召唤 100
        //        {
        //            //MainViewModel.outputdata.PraseInformation += "\n";
        //            praseInformationShow = "\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);
        //            if (asdu.Cot == CauseOfTransmission.ACTIVATION_CON)
        //            {
        //                //ShowMessage.ShowFunction("总召唤确认");
        //                Console.WriteLine((asdu.IsNegative ? "Negative" : "Positive") + "confirmation for interrogation command");
        //                //MainViewModel.outputdata.PraseInformation += (asdu.IsNegative ? "Negative" : "Positive") + "confirmation for interrogation command";
        //                praseInformationShow = (asdu.IsNegative ? "Negative" : "Positive") + "confirmation for interrogation command";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationConfirmationReceived++;
        //            }
        //            else if (asdu.Cot == CauseOfTransmission.ACTIVATION_TERMINATION)
        //            {
        //                //ShowMessage.ShowFunction("总召唤结束");
        //                CommunicationViewModel.GeneralInterrogationFinished = true;
        //                Console.WriteLine("Interrogation command terminated");
        //                //  MainViewModel.outputdata.PraseInformation += "Interrogation command terminated" + "\n";
        //                interrogationTerminationReceived++;
        //                if (CommunicationViewModel.selectedIndexProtocol == 1)//非平衡
        //                {
        //                    SerialPortService.askSecondDataTimer.Enabled = true;
        //                }
        //            }
        //        }
        //        else if (asdu.TypeId == TypeID.C_CS_NA_1)// 时钟同步命令 103
        //        {
        //            if (asdu.Cot == CauseOfTransmission.REQUEST)//读取
        //            {
        //                //ShowMessage.ShowFunction("时钟读取");
        //                var csc = (ClockSynchronizationCommand)asdu.GetElement(0);

        //                TimeViewModel.timeData[1] = csc.NewTime;
        //                //MainViewModel.outputdata.PraseInformation += "终端时间：" + csc.NewTime.ToString() + "\n";
        //                praseInformationShow = "终端时间：" + csc.NewTime.ToString() + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationConfirmationReceived++;
        //            }
        //            else if (asdu.Cot == CauseOfTransmission.ACTIVATION_CON)
        //            {
        //                //ShowMessage.ShowFunction("激活确认");
        //                Console.WriteLine((asdu.IsNegative ? "Negative" : "Positive") + "confirmation for SYNCtime command");
        //                var csc = (ClockSynchronizationCommand)asdu.GetElement(0);

        //                //MainViewModel.outputdata.PraseInformation += (asdu.IsNegative ? "Negative" : "Positive") + "confirmation for SYNCtime command" + csc.NewTime.ToStringDateTime() + "\n";
        //                praseInformationShow = (asdu.IsNegative ? "Negative" : "Positive") + "confirmation for SYNCtime command" + csc.NewTime.ToStringDateTime() + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationConfirmationReceived++;
        //            }
        //            else if (asdu.Cot == CauseOfTransmission.ACTIVATION_TERMINATION)
        //            {
        //                //ShowMessage.ShowFunction("激活终止");
        //                Console.WriteLine("SYNCtime command terminated");

        //                //MainViewModel.outputdata.PraseInformation += "SYNCtime command terminated" + "\n";
        //                praseInformationShow = "SYNCtime command terminated" + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                interrogationTerminationReceived++;
        //            }

        //        }
        //        else if (asdu.TypeId == TypeID.C_RR_NA_1)// 读定值区号 201
        //        {
        //            //ShowMessage.ShowFunction("读定值区号");
        //            var rac = (ReadParameterAreaCommand)asdu.GetElement(0);

        //            Console.WriteLine(" sn1={0},sn2={1},sn3={2}. ", rac.SN1, rac.SN2, rac.SN3);
        //            //MainViewModel.outputdata.PraseInformation += " sn1=" + rac.SN1.ToString() + "sn2=" + rac.SN2.ToString() + "sn3=" + rac.SN3.ToString() + "\n" + "\n";
        //            praseInformationShow = " sn1=" + rac.SN1.ToString() + "sn2=" + rac.SN2.ToString() + "sn3=" + rac.SN3.ToString() + "\n" + "\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);
        //            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
        //                 (ThreadStart)delegate()
        //                 {
        //                     ParameterViewModel.parameterAreaData.Clear();
        //                     //  ParameterViewModel._parameterAreaData.Add(0);
        //                     for (int i = 0; i <= (rac.SN3 - rac.SN2); i++)
        //                     {
        //                         ParameterViewModel.parameterAreaData.Add(i);
        //                     }
        //                     // ParameterViewModel.parameterAreaSelectedIndex = rac.SN1;

        //                 });
        //            ParameterViewModel.outputdata.CurrentArea = rac.SN1.ToString();

        //        }
        //        else if (asdu.TypeId == TypeID.C_RS_NA_1)// 读参数和定值 202
        //        {
        //            praseInformationShow = "定值数据，个数" + asdu.NumberOfElements.ToString() + "." + DateTime.Now.ToString() + "\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);

        //            FixedValueParameter fixedValueParameter = (FixedValueParameter)asdu.GetElement(0);
        //            if (fixedValueParameter.FixedValueParameterObjectList == null || fixedValueParameter.FixedValueParameterObjectList.Count == 0)
        //            {
        //                return false;
        //            }

        //            for (int i = 0; i < fixedValueParameter.FixedValueParameterObjectList.Count; i++)
        //            {
        //                FixedValueParameterObject val = fixedValueParameter.FixedValueParameterObjectList[i];

        //                ShowMessage.PraseInformationShow(praseInformationShow);
        //                try
        //                {
        //                    DevPointDao devPointDaoObject = new DevPointDao();
        //                    switch (val.Tag)
        //                    {
        //                        case 38:

        //                            foreach (var para in ParameterViewModel.parameterData)
        //                            {
        //                                if (Convert.ToInt32(para.ID, 16) == val.IOA)
        //                                {
        //                                    // 返回由字节数组中指定位置的四个字节转换来的单精度浮点数
        //                                    para.Value = System.BitConverter.ToSingle(val.ValueBytes, 0);

        //                                    praseInformationShow = "  IOA: " + val.IOA + " HEx:" + val.IOA.ToString("X") + " value: " + para.Value + "\n";
        //                                    // 更新定值区的值到数据库中  
        //                                    // devPointDaoObject.UpdateFixedParameterValue(val.IOA.ToString("X"), para.Value);
        //                                }
        //                            }

        //                            break;

        //                        case 4:

        //                            foreach (var inherentParameter in InherentParameterViewModel.inherentParameterData)
        //                            {
        //                                if (Convert.ToInt32(inherentParameter.ID, 16) == val.IOA)
        //                                {
        //                                    // 字符串类型
        //                                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
        //                                    inherentParameter.StringValue = Encoding.GetEncoding("GB2312").GetString(val.ValueBytes);// asciiEncoding.GetString(val.ValueBytes);

        //                                    praseInformationShow = "  IOA: " + val.IOA + " HEx:" + val.IOA.ToString("X") + " value: " + inherentParameter.StringValue + "\n";
        //                                }
        //                            }

        //                            break;
        //                    }

        //                }
        //                catch (Exception e)
        //                {
        //                    CommunicationViewModel.con.DebugLog("解析读参数和定值报文错误" + e.ToString());
        //                }
        //            }
        //        }
        //        else if (asdu.TypeId == TypeID.C_WS_NA_1)// 写参数和定值 203
        //        {
        //            //ShowMessage.ShowFunction("写参数和定值");
        //            if (asdu.Cot == CauseOfTransmission.ACTIVATION_CON)
        //            {
        //                ParameterViewModel.send_Continue = true;
        //            }
        //            ParameterViewModel.send_Continue = true;
        //        }
        //        else if (asdu.TypeId == TypeID.M_IT_NB_1)// 累计量，短浮点数 
        //        {
        //            //ShowMessage.ShowFunction("累计量，短浮点数");
        //            for (int i = 0; i < asdu.NumberOfElements; i++)
        //            {
        //                var mfv = (MeasuredValueShort)asdu.GetElement(i);

        //                Console.WriteLine("  IOA: " + mfv.ObjectAddress + " float value: " + mfv.Value);
        //                Console.WriteLine("   " + mfv.Quality.ToString());
        //                //MainViewModel.outputdata.PraseInformation += "  IOA: " + mfv.ObjectAddress + " float value: " + mfv.Value + "\n";
        //                praseInformationShow = "  IOA: " + mfv.ObjectAddress + " float value: " + mfv.Value + "\n";
        //                ShowMessage.PraseInformationShow(praseInformationShow);

        //                try
        //                {
        //                    EnergyViewModel.energyData[mfv.ObjectAddress].Value = mfv.Value;
        //                }
        //                catch (Exception e)
        //                {
        //                    CommunicationViewModel.con.DebugLog("读取和解析电能量报文,电能量值赋值错误" + e.ToString());
        //                }

        //            }
        //        }
        //        else if (asdu.TypeId == TypeID.F_FR_NA_1)// 文件传输 210
        //        {
        //            //ShowMessage.ShowFunction("文件传输");
        //            var f = (FileService)asdu.GetElement(0);
        //            if (asdu.Cot == CauseOfTransmission.NOT_ALLOWED_WRITE_FIEL)
        //            {
        //                MessageBox.Show("不允许写入文件，请等待重试", "提示");
        //            }
        //        }
        //        else if (asdu.TypeId == TypeID.F_SR_NA_1)// 软件升级 211
        //        {
        //            //ShowMessage.ShowFunction("升级确认");
        //            // var f = (FileService)asdu.GetElement(0);
        //        }
        //        else
        //        {
        //            //ShowMessage.ShowFunction("Unknown message type！");
        //            Console.WriteLine("Unknown message type!");
        //            //MainViewModel.outputdata.PraseInformation += "Unknown message type!" + "\n";
        //            praseInformationShow = "Unknown message type!" + "\n";
        //            ShowMessage.PraseInformationShow(praseInformationShow);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show("解析失败:" + ex.ToString(), "错误");
        //        Console.WriteLine("解析ASDU:" + ex.Message);
        //        LogHelper.Warn(typeof(IEC104), ex.ToString());
        //    }

        //    Console.WriteLine("interrogationConfirmationReceived: " + interrogationConfirmationReceived);
        //    Console.WriteLine("interrogationTerminationReceived:  " + interrogationTerminationReceived);
        //    // MainViewModel.outputdata.PraseInformation += "interrogationConfirmationReceived: " + interrogationConfirmationReceived+"\n";
        //    // MainViewModel.outputdata.PraseInformation += "interrogationTerminationReceived:  " + interrogationTerminationReceived + "\n";
        //    return true;
        //}
    }
}
