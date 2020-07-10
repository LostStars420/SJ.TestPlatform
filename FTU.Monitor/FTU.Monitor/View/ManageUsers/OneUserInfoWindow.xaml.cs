using FTU.Monitor.ViewModel.ManageUsers;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FTU.Monitor.View.ManageUsers
{
    /// <summary>
    /// OneUserInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OneUserInfoWindow : Window
    {
        public OneUserInfoWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<string>(this, "CloseWindowCmd", ExcuteCloseWindowCmd);
        }

        /// <summary>
        /// 加载新增/修改界面时的逻辑动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadOneUserInfoWindow(object sender, RoutedEventArgs e)
        {
            this.DataContext = new OneUserInfoViewModel();
        }

        /// <summary>
        /// 执行关闭新增/修改界面指令
        /// </summary>
        /// <param name="arg"></param>
        private void ExcuteCloseWindowCmd(string arg)
        {
            switch (arg)
            {
                case "close":
                    this.Close();
                    break;
            }
        }
    }
}
