using System.Windows;
using FTU.Monitor.View;
using FTU.Monitor.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;
using System.Threading;
using FTU.Monitor.DataService;
using FTU.Monitor.Util;
using lib60870;
using System;
using FTU.Monitor.View.ManageUsers;
using Sojo.Checkplatform.libcollect;
using Sojo.TestPlatform.ControlPlatform;

namespace FTU.Monitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IEC104InterfaceOut IecOut;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Closing += (s, e) => ViewModelLocator.Cleanup();
                var test = new CommunicationViewModel();
                var Control = new TelecontrolViewModel();
                var time = new TimeViewModel();
                this.DataContext = test;
                var iec = MainForm.GetIecInterface();
                //执行连接和总召唤
                iec.TestAct = test.OpenLink;                  
                iec.GiAct = test.CmdAskAll;
                iec.CloseConnection = test.CloseConnection;
                //遥控
                iec.TeleSelect = Control.Select;
                iec.TeleAction = Control.Action;
                iec.TeleCancel = Control.Cancel;
                iec.TeleReloadPoint = Control.ReloadPoint;
                iec.ValueToParameter = Control.ValueToParameter;

                //时间测试
                iec.Set = time.Set;
                iec.Read = time.Read;
                iec.SetTimeParam = time.SetTimeParam;

                //打开故障录波
                iec.OpenFaultRecord = test.OpenFaultRecord ;

                IecOut = MainForm.GetIecInterfaceOut();
                Program.Main();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
    }
}
