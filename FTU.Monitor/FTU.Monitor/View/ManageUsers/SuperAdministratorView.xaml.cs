using FTU.Monitor.ViewModel;
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
    /// SuperAdministratorView.xaml 的交互逻辑
    /// </summary>
    public partial class SuperAdministratorView : Window
    {
        public SuperAdministratorView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 超级用户管理界面关闭时触发的动作
        /// </summary>
        /// <param name="sender">信号发送者</param>
        /// <param name="e">事件</param>
        private void window_closed(object sender, EventArgs e)
        {
            MainViewModel.ShowSuperAdministratorEnable = true;
        }
    }
}
