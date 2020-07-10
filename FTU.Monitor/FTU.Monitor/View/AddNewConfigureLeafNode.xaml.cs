using FTU.Monitor.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FTU.Monitor.View
{
    /// <summary>
    /// AddNewConfigureLeafNode.xaml 的交互逻辑
    /// </summary>
    public partial class AddNewConfigureLeafNode : Window
    {
        public AddNewConfigureLeafNode(bool isEnable)
        {
            InitializeComponent();
            this.DataContext = new AddNewConfigureLeafNodeViewModel();
            this.IPInputBox.IsEnabled = isEnable;
            this.BreakerOrTieSwitch.IsEnabled = isEnable;
            this.MianOrBranch.IsEnabled = isEnable;
        }

        private void Check_IP_Format(object sender, RoutedEventArgs e)
        {
            AddNewConfigureLeafNodeViewModel.CheckIPAddrFormat(this.IPInputBox.Text);
        }

        /// <summary>
        /// 选择断路器或者联络开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectBreakerOrTieSwitch(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem.ToString() == "断路器")
            {
                AddNewConfigureLeafNodeViewModel.selectedBreakerOrTieSwitch = 0;
            }
            else if (cb.SelectedItem.ToString() == "联络")
            {
                AddNewConfigureLeafNodeViewModel.selectedBreakerOrTieSwitch = 1;
            }
        }

        /// <summary>
        /// 选择主线或者支线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectMianOrBranchLine(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem.ToString() == "主线")
            {
                AddNewConfigureLeafNodeViewModel.selectedMainOrBranch = 0;
            }
            else if (cb.SelectedItem.ToString() == "支线")
            {
                AddNewConfigureLeafNodeViewModel.selectedMainOrBranch = 1;
            }
        }

        /// <summary>
        /// 设置节点名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetNodeName(object sender, RoutedEventArgs e)
        {
            AddNewConfigureLeafNodeViewModel.nodeNameInput = this.NodeNameInputBox.Text;
        }

    }
}
