using FTU.Monitor.ViewModel;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// FaultEventView.xaml 的交互逻辑
    /// </summary>
    public partial class FaultEventView : Page
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public FaultEventView()
        {
            InitializeComponent();
            this.DataContext=new FaultEventViewModel();
        }

    }
}
