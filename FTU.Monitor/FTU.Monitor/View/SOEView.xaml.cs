using FTU.Monitor.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;


namespace FTU.Monitor.View
{
    /// <summary>
    /// DataGridPageView.xaml 的交互逻辑
    /// </summary>
    public partial class SOEView : PageFunction<String>
    {
        /// <summary>
        /// 滚动使能
        /// </summary>
        public static bool RollEnable;

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public SOEView()
        {
            this.DataContext = new SOEViewModel();
            InitializeComponent();
            RollEnable = false;
        }

        /// <summary>
        /// DataGrid加载行触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void dg_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    // 获取发生此事件的行
        //    DataGridRow dataGridRow = e.Row;

        //    for (int i = dg.Items.Count; i > 0 ; --i)
        //    {
        //        var HistoryRow = dg.ItemContainerGenerator.ContainerFromItem(dg.Items[dg.Items.Count - i]) as DataGridRow;
        //        if (HistoryRow != null)
        //        {
        //            HistoryRow.Background = new SolidColorBrush(Colors.White);
        //        }
        //    }

        //    // 设置新增行的背景色
        //    dataGridRow.Background = new SolidColorBrush(Colors.LightBlue);
        //    // 将滚动条滚动至最新SOE记录处
        //    dg.ScrollIntoView(dg.Items[dg.Items.Count - 1]);
        //}

    }
}
