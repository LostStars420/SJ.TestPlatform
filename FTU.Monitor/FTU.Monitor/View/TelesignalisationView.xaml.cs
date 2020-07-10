using FTU.Monitor.ViewModel;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// TelesignalisationView.xaml 的交互逻辑
    /// </summary>
    public partial class TelesignalisationView : Page
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public TelesignalisationView()
        {
            InitializeComponent();
            this.DataContext = new TelesignalisationViewModel();
        }

    }
}
