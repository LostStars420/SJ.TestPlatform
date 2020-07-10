using FTU.Monitor.ViewModel;
using System.Windows;

namespace FTU.Monitor.View
{
    /// <summary>
    /// ConnectionParametersView.xaml 的交互逻辑
    /// </summary>
    public partial class TelecontrolPWView : Window
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public TelecontrolPWView()
        {
            InitializeComponent();
            this.PWD.Focus();
            this.DataContext = new TelecontrolPWViewModel();
        }

        /// <summary>
        /// 取消按钮点击响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确认按钮点击响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
