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

namespace FTU.Monitor.View
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            Messenger.Default.Register<object>(this, "CloseLoginWindow", ExcuteCloseLoginWindow);
        }

        /// <summary>
        /// 执行关闭窗口的逻辑
        /// </summary>
        /// <param name="arg"></param>
        private void ExcuteCloseLoginWindow(object arg)
        {
            // this.Close();
            this.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginCancel(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 强制退出当前进程，强制关闭所有线程进而正常结束进程
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
