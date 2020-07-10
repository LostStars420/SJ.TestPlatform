using FTU.Monitor.ViewModel;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// CombineTelesignalisationManagementView.xaml 的交互逻辑
    /// </summary>
    public partial class CombineTelesignalisationManagementView : Page
    {
        /// <summary>
        /// /无参构造方法
        /// </summary>
        public CombineTelesignalisationManagementView()
        {
            InitializeComponent();
            this.DataContext = new CombineTelesignalisationManagementViewModel();
        }
    }
}
