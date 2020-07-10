using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using FTU.Monitor.Model.DTUConfigurePointTableModelCollection;
using FTU.Monitor.Util;
using FTU.Monitor.ViewModel;
using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FTU.Monitor.View
{
    /// <summary>
    /// DTUConfigurePointTableView.xaml 的交互逻辑
    /// </summary>
    public partial class DTUConfigurePointTableView : Page
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public DTUConfigurePointTableView()
        {
            InitializeComponent();
            this.DataContext = new DTUConfigurePointTableViewModel();
            DTUConfigurePointTableViewModelObject = new DTUConfigurePointTableViewModel();
            Messenger.Default.Register<string>(this, "reloadDTUConfigureModuleTree", ExcuteReloadDTUConfigureTree);
        }

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
        /// 加载DTU配置界面时，自动加载配置树结构
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DTUConfigurePointTable_Loaded(object sender, RoutedEventArgs e)
        {
            nodeList = (List<DTUNode>)DTUConfigurePointTableViewModelObject.GetNodeList();
            this.DTUConfigureModule.ItemsSource = nodeList;
        }

        /// <summary>
        /// 添加节点后重新加载配置模块树
        /// </summary>
        /// <param name="arg"></param>
        void ExcuteReloadDTUConfigureTree(string arg)
        {
            ReloadDTUConfigureTree();
        }

        /// <summary>
        /// 重新加载DTU配置树
        /// </summary>
        public void ReloadDTUConfigureTree()
        {
            nodeList = (List<DTUNode>)DTUConfigurePointTableViewModelObject.GetNodeList();
            this.DTUConfigureModule.ItemsSource = nodeList;
            ExpandTree();
        }

        /// <summary>
        /// 展开DT配置树
        /// </summary>
        private void ExpandTree()
        {
            if (this.DTUConfigureModule.Items != null && this.DTUConfigureModule.Items.Count > 0)
            {
                foreach (var item in this.DTUConfigureModule.Items)
                {
                    DependencyObject dependencyObject = this.DTUConfigureModule.ItemContainerGenerator.ContainerFromItem(item);
                    if (dependencyObject != null)//第一次打开程序，dependencyObject为null，会出错
                    {
                        ((TreeViewItem)dependencyObject).ExpandSubtree();
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前用户选中的节点
        /// </summary>
        /// <returns></returns>
        private DTUNode GetCurrentNode()
        {
            CurrentNode = this.DTUConfigureModule.SelectedItem as DTUNode;
            return CurrentNode;
        }

        /// <summary>
        /// 增加同级节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_AddNode_Click(object sender, RoutedEventArgs e)
        {
            var currentNode = GetCurrentNode();
            if (currentNode != null)
            {
                if ((currentNode.NodeType == (int)DTUConfigureUtil.NodeType.RootNode))
                {
                    MessageBox.Show("根点不支持新增节点操作!");
                    return;
                }
                // 开始执行增加节点的操作
                if (currentNode.NodeType == (int)DTUConfigureUtil.NodeType.FirstLevelNode)
                {
                    //添加一个配置模块节点到整个树形表中
                    DTUConfigurePointTableViewModelObject.AddNewCongigureModule(CurrentNode.Path.Substring(0,CurrentNode.Path.LastIndexOf('/')));
                }
            }
        }

        /// <summary>
        /// 删除同级节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_DeleteNode_Click(object sender, RoutedEventArgs e)
        {
            var currentNode = GetCurrentNode();
            if(currentNode != null)
            {
                if (currentNode.NodeType == (int)DTUConfigureUtil.NodeType.RootNode)
                {
                    MessageBox.Show("该节点不支持删除操作!");
                }
                else
                {
                    MessageBoxResult dr = MessageBox.Show("确定要删除这个节点吗？", "提示", MessageBoxButton.OKCancel);
                    if (dr == MessageBoxResult.OK)
                    {
                        int result = new DTUConfigurePointTableDao().DeleteNodeConfigure(currentNode.NodeID);
                        if (result > 0)
                        {
                            MessageBox.Show("成功删除节点！");
                            ReloadDTUConfigureTree();
                        }
                        else
                        {
                            MessageBox.Show("删除节点操作失败！");
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 单击节点时，显示该模块节点对应的三遥点表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SingleClickedNode(object sender, MouseButtonEventArgs e)
        {
            CurrentNode = GetCurrentNode();
            if(CurrentNode != null)
            {
                Messenger.Default.Send<int>(CurrentNode.NodeID, "GetCurrentModulePointTable");
            }
        }

        /// <summary>
        /// 重命名节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_RenameNode_Click(object sender, RoutedEventArgs e)
        {
            //获取在TreeView.ItemTemplate中定义的TextBox控件
            tempTextBox = FindVisualChild<TextBox>(item as DependencyObject);
            //设置该TextBox的Visibility 属性为Visible
            tempTextBox.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 下面的部分是在鼠标指针位于此元素（TreeViewItem）上并且按下鼠标右键时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //此处item定义的是一个类的成员变量，是一个TreeViewItem类型
            item = GetParentObjectEx<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (item != null)
            {
                //使当前节点获得焦点
                item.Focus();

                //系统不再处理该操作
                e.Handled = true;
            }
        }

        /// <summary>
        /// 获取当前TreeView的TreeViewItem
        /// </summary>
        /// <typeparam name="TreeViewItem"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public TreeViewItem GetParentObjectEx<TreeViewItem>(DependencyObject obj) where TreeViewItem : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is TreeViewItem)
                {
                    return (TreeViewItem)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        /// <summary>
        /// 当TextBox失去焦点时发生此事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameTextBox_LostFous(object sender, RoutedEventArgs e)
        {
            tempTextBox.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 修改完成后，用户按下Enter键后更新数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Key_Down(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                CurrentNode = GetCurrentNode();
                CurrentNode.NodeName = tempTextBox.Text;
                int result = new DTUConfigurePointTableDao().UpdateNodeConfigure(CurrentNode);
                if (result <= 0)
                {
                    MessageBox.Show("修改节点名称失败", "警告");
                }
                else
                {
                    tempTextBox.Visibility = Visibility.Collapsed;
                }
            }
        }

    }
}
