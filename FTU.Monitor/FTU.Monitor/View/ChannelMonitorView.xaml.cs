using FTU.Monitor.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// ChannelMonitorView.xaml 的交互逻辑
    /// </summary>
    public partial class ChannelMonitorView : Window
    {
        public ChannelMonitorView()
        {
            InitializeComponent();
            this.DataContext = new ChannelMonitorViewModel();
            Messenger.Default.Register<string>(this, "CloseChannelMonitorView", ExcuteCloseChannelMonitorView);
        }

        /// <summary>
        /// 点击“停止监听”按钮后，对应的关闭通道监听方法
        /// </summary>
        /// <param name="arg"></param>
        private void ExcuteCloseChannelMonitorView(string arg)
        {
            MainViewModel.ShowChannelMonitorEnable = true;
            this.Dispatcher.Invoke(new Action(() => { this.Close(); }));
        }

        /// <summary>
        /// 重写通道监视窗口关闭方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (System.Threading.Interlocked.Read(ref MainViewModel.ChannelMonitorListening) != 0)
            {
                MessageBoxResult result = MessageBox.Show("确认已执行<停止监听>", "提示", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    // 强制退出通道监听
                    MainViewModel.ShowChannelMonitorEnable = true;
                    System.Threading.Interlocked.Exchange(ref MainViewModel.ChannelMonitorListening, 0);//否则，主程序会死机
                    e.Cancel = false;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                MainViewModel.ShowChannelMonitorEnable = true;
                e.Cancel = false;
            }
        }

        /// <summary>
        /// 监听记录滚动到最新一条处
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_ChannelMonitorLoadingRow(object sender, DataGridRowEventArgs e)
        {
            // 将滚动条滚动至最新通道监视记录处
            dg.ScrollIntoView(dg.Items[dg.Items.Count - 1]);
        }
    }
}
