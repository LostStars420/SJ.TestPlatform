using FTU.Monitor.ViewModel;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// Timexaml.xaml 的交互逻辑
    /// </summary>
    public partial class TimeView : Page
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public TimeView()
        {
            InitializeComponent();
            this.DataContext = new TimeViewModel();
        }

    }
}
