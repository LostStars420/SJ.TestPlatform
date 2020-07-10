using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using FTU.Monitor.Util;
using FTU.Monitor.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace FTU.Monitor.View
{
    /// <summary>
    /// DTUConfigureView.xaml 的交互逻辑
    /// </summary>
    public partial class DTUConfigureView : Page
    {
        /// <summary>
        /// DTU配置树的集合
        /// </summary>
        public static List<Node> nodeList { get; set; }

        /// <summary>
        /// 鼠标选中的当前节点
        /// </summary>
        public static Node CurrentNode { get; set; }

        /// <summary>
        /// DTUConfigureViewModel对象
        /// </summary>
        DTUConfigureViewModel DTUConfigureViewModelObject;

        //DTUConfigureUtil DTUConfigureUtilObject;
        void ExcuteReloadDTUConfigureTree(string arg)
        {
            ReloadDTUConfigureTree();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DTUConfigureView()
        {
            InitializeComponent();
            this.DataContext = new DTUConfigureViewModel();
            DTUConfigureViewModelObject = new DTUConfigureViewModel();
            //DTUConfigureUtilObject = new DTUConfigureUtil();
            Messenger.Default.Register<string>(this, "reloadDTUConfigureTree", ExcuteReloadDTUConfigureTree);
        }

        /// <summary>
        /// DTU配置参数窗体加载执行的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DTUConfigure_Loaded(object sender, RoutedEventArgs e)
        {
            nodeList = (List<Node>)DTUConfigureViewModelObject.GetNodeList();
            this.TreeView_NodeList.ItemsSource = nodeList;
            //ExpandTree();
        }

        /// <summary>
        /// 重新加载DTU配置树
        /// </summary>
        public void ReloadDTUConfigureTree()
        {
            nodeList = (List<Node>)DTUConfigureViewModelObject.GetNodeList();
            this.TreeView_NodeList.ItemsSource = nodeList;
            //ExpandTree();
        }

        /// <summary>
        /// 展开DT配置树
        /// </summary>
        private void ExpandTree()
        {
            if (this.TreeView_NodeList.Items != null && this.TreeView_NodeList.Items.Count > 0)
            {
                foreach (var item in this.TreeView_NodeList.Items)
                {
                    DependencyObject dependencyObject = this.TreeView_NodeList.ItemContainerGenerator.ContainerFromItem(item);
                    if (dependencyObject != null)//第一次打开程序，dependencyObject为null，会出错
                    {
                        ((TreeViewItem)dependencyObject).ExpandSubtree();
                    }
                }
            }
        }

        /// <summary>
        /// 添加选中的当前同级节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_AddNode_Click(object sender, RoutedEventArgs e)
        {
            var currentNode = GetCurrentNode();
            if (currentNode != null)
            {
                if ((currentNode.NodeType == (int)DTUConfigureUtil.NodeType.RootNode) || (currentNode.NodeType == (int)DTUConfigureUtil.NodeType.SecondLevelNode))
                {
                    MessageBox.Show("本节点不支持新增节点操作!");
                    return;
                }
                // 开始执行增加节点的操作
                // 将要添加的是表x
                if (currentNode.NodeType == (int)DTUConfigureUtil.NodeType.FirstLevelNode)
                {
                    //添加初始表到整个数据表中
                    DTUConfigureViewModelObject.AddNewTable();
                }
                else if (currentNode.NodeType == (int)DTUConfigureUtil.NodeType.ThirdLevelNode)
                {
                    // 添加3级节点到树结构中
                    DTUConfigureViewModelObject.AddNewThirdLevelNode(DTUConfigureView.CurrentNode.Path.Substring(0, DTUConfigureView.CurrentNode.Path.LastIndexOf('/')));
                }
                else if (currentNode.NodeType == (int)DTUConfigureUtil.NodeType.LeafNode)
                {
                    // 添加一个叶节点到树结构中
                    DTUConfigureViewModelObject.AddNewLeafNode(DTUConfigureView.CurrentNode.Path.Substring(0, DTUConfigureView.CurrentNode.Path.LastIndexOf('/')));
                }
            }
        }

        /// <summary>
        /// 添加子级节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_AddChildrenNode_Click(object sender, RoutedEventArgs e)
        {
             var currentNode = GetCurrentNode();
            if (currentNode != null)
            {
                if (currentNode.NodeName == DTUConfigureUtil.GetInstance().MSide 
                    || currentNode.NodeName == DTUConfigureUtil.GetInstance().NSide 
                    || currentNode.NodeType == (int)DTUConfigureUtil.NodeType.ThirdLevelNode)
                {
                    DTUConfigureViewModelObject.AddNewLeafNode(DTUConfigureView.CurrentNode.Path);
                }
                else if (currentNode.NodeName == DTUConfigureUtil.GetInstance().PowerSupply)
                {
                    DTUConfigureViewModelObject.AddNewThirdLevelNode(DTUConfigureView.CurrentNode.Path);
                }
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_DeleteNode_Click(object sender, RoutedEventArgs e)
        {
            var currentNode = GetCurrentNode();
            if (currentNode != null)
            {
                if (currentNode.NodeType == (int)DTUConfigureUtil.NodeType.RootNode || currentNode.NodeType == (int)DTUConfigureUtil.NodeType.SecondLevelNode)
                {
                    MessageBox.Show("该节点不支持删除操作!");
                }
                else
                {
                    MessageBoxResult dr = MessageBox.Show("确定要删除这个节点吗？", "提示", MessageBoxButton.OKCancel);
                    if (dr == MessageBoxResult.OK)
                    {
                        bool result = DeleteTheNode(currentNode);
                        if (result == true)
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
        /// 删除节点
        /// </summary>
        /// <param name="deleteNode">要删除的节点</param>
        private bool DeleteTheNode(Node deleteNode)
        {
            // 查询当前节点的所有子节点
            List<Node> deletedNodes = (List<Node>)new DTUConfigureDao().QueryNodeChildren(deleteNode.Path);
            List<Node> allNodes = (List<Node>)new DTUConfigureDao().Query();

            if (deletedNodes == null || allNodes == null)
            {
                return false;
            }

            int result = 0;
            foreach (Node deletedNode in deletedNodes)
            {
                foreach (Node node in allNodes)
                {
                    if (deletedNode.NodeID == node.NodeID)
                    {
                        result = new DTUConfigureDao().DeleteNodeConfigure(node.NodeID);
                    }
                }
            }
            if (result == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 鼠标右键选中节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem != null)
            {
                Node currentNode = treeViewItem.Header as Node;
                if ((currentNode.NodeType == (int)DTUConfigureUtil.NodeType.LeafNode) 
                    || (currentNode.NodeType == (int)DTUConfigureUtil.NodeType.FirstLevelNode) 
                    || (currentNode.NodeType == (int)DTUConfigureUtil.NodeType.ThirdLevelNode))
                {
                    MenuItem_AddNode.IsEnabled = true;
                    MenuItem_DeleteNode.IsEnabled = true;
                }
                else
                {
                    MenuItem_AddNode.IsEnabled = false;
                    MenuItem_DeleteNode.IsEnabled = false;
                }


                if ((currentNode.Children.Count == 0) && (currentNode.NodeType == (int)DTUConfigureUtil.NodeType.SecondLevelNode || currentNode.NodeType == (int)DTUConfigureUtil.NodeType.ThirdLevelNode))
                {
                    MenuItem_AddChildrenNode.IsEnabled = true;
                }
                else
                {
                    MenuItem_AddChildrenNode.IsEnabled = false;
                }
                treeViewItem.Focus();
                e.Handled = true;
            }
        }
        private DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
            {
                source = VisualTreeHelper.GetParent(source);
            }
            return source;
        }

        /// <summary>
        /// 获取当前用户选中的节点
        /// </summary>
        /// <returns></returns>
        private Node GetCurrentNode()
        {
            CurrentNode = this.TreeView_NodeList.SelectedItem as Node;
            return CurrentNode;
        }

        /// <summary>
        /// 双击树的节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleClickedNode(object sender, MouseButtonEventArgs e)
        {
            Node selectedNode = this.TreeView_NodeList.SelectedItem as Node;
            if (selectedNode.NodeType == (int)DTUConfigureUtil.NodeType.ThirdLevelNode)
            {
                //用户选中的是叶节点,将选中的节点信息赋值给显示信息的弹框
                AddNewConfigureLeafNode changeLeafConfig = new AddNewConfigureLeafNode(false);
                AddNewConfigureLeafNodeViewModel.nodeNameInput = selectedNode.NodeName;

                // 显示弹框
                changeLeafConfig.ShowDialog();

                // 保存用户修改的值
                selectedNode.NodeName = AddNewConfigureLeafNodeViewModel.nodeNameInput;

                // 更新到数据库中
                new DTUConfigureDao().UpdateNodeConfigure(selectedNode);
                // 更新界面显示树
                ReloadDTUConfigureTree();
            }
            else if (selectedNode.NodeType == (int)DTUConfigureUtil.NodeType.LeafNode || selectedNode.NodeType == (int)DTUConfigureUtil.NodeType.FirstLevelNode)
            {
                //用户选中的是叶节点或者是表X,将选中的节点信息赋值给显示信息的弹框
                AddNewConfigureLeafNode changeLeafConfig = new AddNewConfigureLeafNode(true);
                AddNewConfigureLeafNodeViewModel.nodeNameInput = selectedNode.NodeName;
                AddNewConfigureLeafNodeViewModel.nodeIP = selectedNode.NodeIP;
                AddNewConfigureLeafNodeViewModel.selectedBreakerOrTieSwitch = selectedNode.NodeBreakersOrTieSwitch;
                AddNewConfigureLeafNodeViewModel.selectedMainOrBranch = selectedNode.NodeMainOrBranchLine;

                // 显示弹框
                changeLeafConfig.ShowDialog();

                // 保存用户修改的值
                selectedNode.NodeName = AddNewConfigureLeafNodeViewModel.nodeNameInput;
                selectedNode.NodeIP = AddNewConfigureLeafNodeViewModel.nodeIP;
                selectedNode.NodeBreakersOrTieSwitch = AddNewConfigureLeafNodeViewModel.selectedBreakerOrTieSwitch;
                selectedNode.NodeMainOrBranchLine = AddNewConfigureLeafNodeViewModel.selectedMainOrBranch;

                // 更新到数据库中
                new DTUConfigureDao().UpdateNodeConfigure(selectedNode);
                // 更新界面显示树
                ReloadDTUConfigureTree();
            }
        }

        /// <summary>
        /// 单击事件的操作逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SingleClickedNode(object sender, MouseButtonEventArgs e)
        {
            Node selectedNode = this.TreeView_NodeList.SelectedItem as Node;
            if(selectedNode == null)
            {
                return;
            }

            if (selectedNode.NodeType == (int)DTUConfigureUtil.NodeType.FirstLevelNode)
            {
                // 用户选中的是表X,设置显示该表的所有配置信息
                // 查询该表的所有子节点列表
                List<Node> seletedNodes = (List<Node>)new DTUConfigureDao().QueryNodeChildren(selectedNode.Path);
                // 将选中的表显示所有配置信息
                Messenger.Default.Send<List<Node>>(seletedNodes, "ShowConfigureTable");
            }
        }

    }
}
