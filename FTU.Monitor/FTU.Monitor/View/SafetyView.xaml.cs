using FTU.Monitor.ViewModel;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// SafetyView.xaml 的交互逻辑
    /// </summary>
    public partial class SafetyView : Page
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public SafetyView()
        {
            InitializeComponent();
            this.DataContext = new SafetyViewModel();
        }

    }
}
