using FTU.Monitor.ViewModel;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// PowerEnergyView.xaml 的交互逻辑
    /// </summary>
    public partial class EnergyView : Page
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public EnergyView()
        {
            InitializeComponent();
            this.DataContext = new EnergyViewModel();
        }

    }
}
