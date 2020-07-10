using FTU.Monitor.ViewModel;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// Telemeteringxaml.xaml 的交互逻辑
    /// </summary>
    public partial class TelemeteringView : Page
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public TelemeteringView()
        {
            InitializeComponent();
            this.DataContext = new TelemeteringViewModel();
        }

    }
}
