using FTU.Monitor.ViewModel;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// CoefficientView.xaml 的交互逻辑
    /// </summary>
    public partial class CoefficientView : Page
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public CoefficientView()
        {
            InitializeComponent();
            this.DataContext = new CoefficientViewModel();
        }
    }
}
