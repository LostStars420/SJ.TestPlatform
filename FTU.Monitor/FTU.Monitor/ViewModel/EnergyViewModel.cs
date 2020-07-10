using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using FTU.Monitor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using lib60870;
using FTU.Monitor.Dao;
using FTU.Monitor.Util;
using FTU.Monitor.DataService;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// EnergyViewModel 的摘要说明
    /// author: songminghao
    /// date：2018/3/15 21:40:52
    /// desc：电能量ViewModel
    /// version: 1.0
    /// </summary>
    public class EnergyViewModel : ViewModelBase,IIEC104Handler
    {
        /// <summary>
        /// 电能量点表加载显示数据集合
        /// </summary>
        public ObservableCollection<Energy> _energyData;

        /// <summary>
        /// 设置和获取电能量点表加载显示数据集合
        /// </summary>
        public ObservableCollection<Energy> EnergyData
        {
            get
            {
                return this._energyData;
            }
            set
            {
                this._energyData = value;
                RaisePropertyChanged(() => EnergyData);
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

        public void HandleASDUData(TypeID TI,ASDU asdu)
        {
            for (int i = 0; i < asdu.NumberOfElements; i++)
            {
                var mfv = (MeasuredValueShort)asdu.GetElement(i);

                Console.WriteLine("  IOA: " + mfv.ObjectAddress + " float value: " + mfv.Value);
                Console.WriteLine("   " + mfv.Quality.ToString());
                ParseInformationShow = "  IOA: " + mfv.ObjectAddress + " float value: " + mfv.Value + "\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);

                try
                {
                    EnergyData[mfv.ObjectAddress].Value = mfv.Value;
                }
                catch (Exception e)
                {
                    CommunicationViewModel.con.DebugLog("读取和解析电能量报文,电能量值赋值错误" + e.ToString());
                }

            }
        }
        /// <summary>
        /// 电能量报文召唤命令
        /// </summary>
        public RelayCommand<string> EnergyCommand
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 电能量报文召唤命令执行操作
        /// </summary>
        /// <param name="arg">参数</param>
        public void ExecuteEnergyCommand(string arg)
        {
            if(CommunicationViewModel.IsLinkConnect())
            {
                CommunicationViewModel.con.SendCounterInterrogationCommand(CauseOfTransmission.ACTIVATION, 0x01, 5);
            }
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public EnergyViewModel()
        {
            // 注册接收到初始化结束ASDU的处理事件 
            IEC104.RegisterIEC104Handler(TypeID.M_IT_NB_1, this);// 累计量，短浮点数

            EnergyCommand = new RelayCommand<string>(ExecuteEnergyCommand);

            this._energyData = new ObservableCollection<Energy>();
            // 获取使用的所有电能量点表
            DevPointDao devPointDao = new DevPointDao();
            IList<DevPoint> devPointList = devPointDao.queryByPointTypeId(ConfigUtil.getPointTypeID("电能量"));
            // 检验和解析使用的所有电能量点表
            CheckData(devPointList);

        }

        /// <summary>
        /// 检验和解析使用的所有电能量点表
        /// </summary>
        /// <param name="devPointList">使用的所有电能量点表</param>
        private void CheckData(IList<DevPoint> devPointList)
        {
            if (devPointList != null && devPointList.Count > 0)
            {
                for (int i = 0; i < devPointList.Count; i++)
                {
                    Energy energy = new Energy();
                    energy.Number = i + 1;
                    energy.ID = Convert.ToInt32(devPointList[i].ID, 16);
                    energy.Name = devPointList[i].Name;
                    energy.Value = (float)devPointList[i].Value;
                    energy.Unit = devPointList[i].Unit;
                    energy.Rate = (float)devPointList[i].Rate;

                    EnergyData.Add(energy);
                }
            }

        }
        
    }
    
}
