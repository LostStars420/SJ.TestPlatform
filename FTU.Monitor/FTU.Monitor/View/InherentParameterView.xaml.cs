using FTU.Monitor.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;

namespace FTU.Monitor.View
{
    /// <summary>
    /// InherentParameterView.xaml 的交互逻辑
    /// </summary>
    public partial class InherentParameterView : Page
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public InherentParameterView()
        {
            InitializeComponent();
            this.DataContext = new InherentParameterViewModel();
        }
    }
}
