using FTU.Monitor.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// FileServiceView.xaml 的交互逻辑
    /// </summary>
    public partial class FileServiceView : Window
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public FileServiceView()
        {
            InitializeComponent();
            this.DataContext = new FileServiceViewModel();
            Messenger.Default.Register<int>(this, "progressBarValue", ExecuteSetProgressBarValue);
            m_update = new UpdateDelegate(pBar.SetValue);
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private delegate void UpdateDelegate(System.Windows.DependencyProperty dp, Object value);
        private UpdateDelegate m_update;

        /// <summary>
        /// 更新写文件进度条的值
        /// </summary>
        /// <param name="arg"></param>
        void ExecuteSetProgressBarValue(int arg)
        {
            Dispatcher.Invoke(m_update, System.Windows.Threading.DispatcherPriority.Background,
                new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(arg) });
        }

        /// <summary>
        /// 文件目录记录滚动到最新一条处
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_FileServerViewLoadingRow(object sender, DataGridRowEventArgs e)
        {
            // 将滚动条滚动至最新通道监视记录处
            dg.ScrollIntoView(dg.Items[dg.Items.Count - 1]);
        }

    }
}
