using FTU.Monitor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FTU.Monitor.View
{
    /// <summary>
    /// ReadmeView.xaml 的交互逻辑
    /// </summary>
    public partial class ReadmeView : Window
    {
        public ReadmeView()
        {
            InitializeComponent();
            this.DataContext = new ReadmeViewModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadmeFormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainViewModel.ShowReadmeEnable = true;
        }
    }
}
