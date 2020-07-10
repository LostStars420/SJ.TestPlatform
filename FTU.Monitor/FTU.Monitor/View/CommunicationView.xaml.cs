using FTU.Monitor.ViewModel;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// CommunicationView.xaml 的交互逻辑
    /// </summary>
    public partial class CommunicationView : Page
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public CommunicationView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 用户选择端口改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortSelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            string portName = this.portName.SelectedItem.ToString();
            if (portName == "网口")
            {
                // 协议自动更新为104平衡
                this.protocolCombox.SelectedIndex = 2;
            }
            else
            {
                // 协议自动更新为101平衡
                this.protocolCombox.SelectedIndex = 0;
            }
        }
    }
}
