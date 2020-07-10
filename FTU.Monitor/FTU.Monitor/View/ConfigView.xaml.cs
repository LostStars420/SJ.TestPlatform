using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using FTU.Monitor.Model.DTUConfigurePointTableModelCollection;
using FTU.Monitor.Util;
using FTU.Monitor.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FTU.Monitor.View
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigView : Page
    {
        /// <summary>
        /// DTU配置点表树的集合
        /// </summary>
        public List<DTUNode> nodeList { get; set; }

        /// <summary>
        /// 鼠标选中的当前节点
        /// </summary>
        public static DTUNode CurrentNode { get; set; }

        /// <summary>
        /// DTUConfigurePointTableViewModel的对象
        /// </summary>
        DTUConfigurePointTableViewModel DTUConfigurePointTableViewModelObject;

        TreeViewItem item = null;

        TextBox tempTextBox = null;

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public ConfigView()
        {
            InitializeComponent();
            this.DataContext = new ConfigViewModel();
            Messenger.Default.Register<string>(this, "UpdateProductSerialNumberSecond", ExcuteUpdateProductSerialNumberSecond);
        }

        /// <summary>
        /// 更新设备id
        /// </summary>
        /// <param name="arg"></param>
        private void ExcuteUpdateProductSerialNumberSecond(string arg)
        {
            this.deviceID.Text = arg;
        }

        /// <summary>
        /// 遥信点表配置界面增加一行时触发的动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YXTable_AddLine(object sender, DataGridRowEventArgs e)
        {
            // 获取发生此事件的行
            DataGridRow dataGridRow = e.Row;

            for (int i = YXTable.Items.Count; i > 0; --i)
            {
                var HistoryRow = YXTable.ItemContainerGenerator.ContainerFromItem(YXTable.Items[YXTable.Items.Count - i]) as DataGridRow;
                if (HistoryRow != null)
                {
                    HistoryRow.Background = new SolidColorBrush(Colors.White);
                }
            }

            // 设置新增行的背景色
            dataGridRow.Background = new SolidColorBrush(Colors.SkyBlue);
        }

        /// <summary>
        /// 遥测点表配置界面增加一行时触发的动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YCTable_AddLine(object sender, DataGridRowEventArgs e)
        {
            // 获取发生此事件的行
            DataGridRow dataGridRow = e.Row;

            for (int i = YCTable.Items.Count; i > 0; --i)
            {
                var HistoryRow = YCTable.ItemContainerGenerator.ContainerFromItem(YCTable.Items[YCTable.Items.Count - i]) as DataGridRow;
                if (HistoryRow != null)
                {
                    HistoryRow.Background = new SolidColorBrush(Colors.White);
                }
            }

            // 设置新增行的背景色
            dataGridRow.Background = new SolidColorBrush(Colors.SkyBlue);
        }

        /// <summary>
        /// 遥控点表配置界面增加一行时触发的动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YKTable_AddLine(object sender, DataGridRowEventArgs e)
        {
            // 获取发生此事件的行
            DataGridRow dataGridRow = e.Row;

            for (int i = YKTable.Items.Count; i > 0; --i)
            {
                var HistoryRow = YKTable.ItemContainerGenerator.ContainerFromItem(YKTable.Items[YKTable.Items.Count - i]) as DataGridRow;
                if (HistoryRow != null)
                {
                    HistoryRow.Background = new SolidColorBrush(Colors.White);
                }
            }

            // 设置新增行的背景色
            dataGridRow.Background = new SolidColorBrush(Colors.SkyBlue);
        }
    }
}
