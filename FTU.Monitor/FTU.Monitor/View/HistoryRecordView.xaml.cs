using FTU.Monitor.ViewModel;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// HistoryRecordView.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryRecordView : Page
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public HistoryRecordView()
        {
            InitializeComponent();
            this.DataContext = new HistoryRecordViewModel();
        }
    }
}
