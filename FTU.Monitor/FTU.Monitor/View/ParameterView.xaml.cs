using FTU.Monitor.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Data;
using System.Web.Configuration;
using System.Windows.Controls;
using System.Windows.Media;

namespace FTU.Monitor.View
{
    /// <summary>
    /// ParameterView.xaml 的交互逻辑
    /// </summary>
    public partial class ParameterView : Page
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public ParameterView()
        {
            InitializeComponent();
            this.DataContext = new ParameterViewModel();
        }    
    }
}
