using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using FTU.Monitor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using lib60870;
using FTU.Monitor.DataService;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// TelemeteringViewModel 的摘要说明
    /// author: liyan
    /// date：2018/7/21 9:48:09
    /// desc：故障事件ViewModel
    /// version: 1.0
    /// </summary>
    public class FaultEventViewModel : ViewModelBase,IIEC104Handler
    {
         /// <summary>
        /// Initializes a new instance of the DataGridPageViewModel class.
        /// </summary>
        public FaultEventViewModel()
        {
            IEC104.RegisterIEC104Handler(TypeID.M_FT_NA_1, this);//注册故障事件数据处理事件
            FaultEventCommand = new RelayCommand<string>(ExecuteFaultEventCommand);
        }
        public RelayCommand<string> FaultEventCommand { get; private set; }
        public void ExecuteFaultEventCommand(string arg)
        {
            switch (arg)
            { 
               
                case "Clear":
                    FaultEventData.Clear();
                    break;
            }
        }
        
        /************** 属性 **************/
        public static ObservableCollection<FaultEvent> faultEventData=new ObservableCollection<FaultEvent>();
        /// <summary>
        /// 用户信息数据
        /// </summary>
        public ObservableCollection<FaultEvent> FaultEventData
        {
            get { return faultEventData; }
            set
            {
                faultEventData = value;
                RaisePropertyChanged("FaultEventData");
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
        /// 接受处理故障事件数据集合
        /// </summary>
        /// <param name="TI">故障事件类型标识</param>
        /// <param name="asdu">对应故障事件类型标识的ASDU</param>
        public void HandleASDUData(TypeID TI, ASDU asdu)
        {
            ParseInformationShow = "故障值信息 \n";
            ShowMessage.ParseInformationShow(ParseInformationShow);

            var val = (SinglePointWithCP56Time2a)asdu.GetElement(0);
        }
      
    }
}
