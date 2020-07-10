using FTU.Monitor.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// ComtradeView.xaml 的交互逻辑
    /// </summary>
    public partial class ComtradeView : Window
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public ComtradeView()
        {
            InitializeComponent();
            this.DataContext = new ComtradeViewModel();
        }

    }
}
