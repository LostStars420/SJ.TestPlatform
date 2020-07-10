using FTU.Monitor.ViewModel;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// TelecontrolView.xaml 的交互逻辑
    /// </summary>
    public partial class TelecontrolView : Page
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public TelecontrolView()
        {
            InitializeComponent();
            this.DataContext = new TelecontrolViewModel();
        }
        
    }
}
