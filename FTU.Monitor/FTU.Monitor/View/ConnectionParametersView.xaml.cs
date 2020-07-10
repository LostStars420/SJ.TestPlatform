using lib60870;
using System.Windows;

namespace FTU.Monitor.View
{
    /// <summary>
    /// ConnectionParametersView.xaml 的交互逻辑
    /// </summary>
    public partial class ConnectionParametersView : Window
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public ConnectionParametersView()
        {
            InitializeComponent();
            this.DataContext = new ConnectionParameters();
        }

    }
}
