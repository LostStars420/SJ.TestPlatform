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
    /// ChangeSoftwareVersionView.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeSoftwareVersionView : Window
    {
        public ChangeSoftwareVersionView()
        {
            InitializeComponent();
            this.DataContext = new ChangeSoftwareVersionViewModel();
        }

        /// <summary>
        ///用户点击 “确定”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 用户点击“取消”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
