using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using FTU.Monitor.Util;
using FTU.Monitor.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// DTUConfigureViewModel 的摘要说明
    /// author: liyan
    /// date：2018/5/27 11:04:04
    /// desc：分布式参数配置操作逻辑
    /// version: 1.0
    /// </summary>
    public class DTUConfigureViewModel : ViewModelBase
    {

        /// <summary>
        /// 所有配置列表
        /// </summary>
        private List<Node> DTUConfigureAllNodes { get; set; }

        /// <summary>
        /// 表X的所有数据
        /// </summary>
        private ObservableCollection<Node> _DTUConfigureTable;

        /// <summary>
        /// 设置和获取表X的所有数据
        /// </summary>
        public ObservableCollection<Node> DTUConfigureTable
        {
            get
            {
                return this._DTUConfigureTable;
            }

            set
            {
                this._DTUConfigureTable = value;
                RaisePropertyChanged(() => DTUConfigureTable);
            }
        }

        public RelayCommand<string> DTUConfigureCommand { get; private set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public DTUConfigureViewModel()
        {
            DTUConfigureAllNodes = new List<Node>();
            DTUConfigureTable = new ObservableCollection<Node>();
            DTUConfigureCommand = new RelayCommand<string>(ExcuteDTUConfigureCommand);
            Messenger.Default.Register<List<Node>>(this, "ShowConfigureTable", ExcuteShowTable);
        }

        /// <summary>
        /// 给表X的配置信息对象赋值
        /// </summary>
        /// <param name="nodesInTable"></param>
        public void ExcuteShowTable(List<Node> nodesInTable)
        {
            DTUConfigureTable.Clear();
            foreach (Node nodeInTable in nodesInTable)
            {
                Node node = new Node();
                node.NodeID = nodeInTable.NodeID;
                node.NodeName = nodeInTable.NodeName;
                node.NodeType = nodeInTable.NodeType;
                node.Path = nodeInTable.Path;
                node.NodeIP = nodeInTable.NodeIP;
                node.NodeMainOrBranchLine = nodeInTable.NodeMainOrBranchLine;
                node.NodeBreakersOrTieSwitch = nodeInTable.NodeBreakersOrTieSwitch;

                DTUConfigureTable.Add(node);
            }
        }

        /// <summary>
        /// 得到DTU配置树
        /// </summary>
        /// <returns></returns>
        public List<Node> GetNodeList()
        {
            // 获取树结构中的所有节点
            DTUConfigureDao DTUConfigureDaoObject = new DTUConfigureDao();
            DTUConfigureAllNodes = (List<Node>)DTUConfigureDaoObject.Query();

            foreach (var treeNode in DTUConfigureAllNodes)
            {
                treeNode.FindParent(DTUConfigureAllNodes);
            }

            List<Node> root = new List<Node>();
            foreach (var treeNode in DTUConfigureAllNodes)
            {
                if (treeNode.NodeType == (int)DTUConfigureUtil.NodeType.RootNode)
                {
                    root.Add(treeNode);
                    break;
                }
            }
            return root;
        }

        /// <summary>
        /// 添加一个表到设备结构中
        /// </summary>
        public void AddNewTable()
        {
            DTUConfigureDao DTUConfigureDaoObject = new DTUConfigureDao();
            List<Node> InitTable = (List<Node>)DTUConfigureDaoObject.QueryDTUConfigureInitTable();

            // 获取数据库中最大的NodeID值
            int nextRecord = 1;
            int nodeNumber = DTUConfigureDaoObject.QueryMaxNodeID() + nextRecord;

            // 处理每个新节点中的Path
            foreach (Node newNode in InitTable)
            {
                if (newNode.NodeType == (int)DTUConfigureUtil.NodeType.FirstLevelNode)
                {
                    // 表节点
                    newNode.Path = "/1/" + nodeNumber;
                }
                else if (newNode.NodeType == (int)DTUConfigureUtil.NodeType.SecondLevelNode && newNode.NodeName.Equals(DTUConfigureUtil.GetInstance().MSide))
                {
                    newNode.Path = "/1/" + nodeNumber + "/" + (nodeNumber + 1);
                }
                else if (newNode.NodeType == (int)DTUConfigureUtil.NodeType.SecondLevelNode && newNode.NodeName.Equals(DTUConfigureUtil.GetInstance().NSide))
                {
                    newNode.Path = "/1/" + nodeNumber + "/" + (nodeNumber + 2);
                }
                else if (newNode.NodeType == (int)DTUConfigureUtil.NodeType.SecondLevelNode && newNode.NodeName.Equals(DTUConfigureUtil.GetInstance().PowerSupply))
                {
                    newNode.Path = "/1/" + nodeNumber + "/" + (nodeNumber + 3);
                }
                else if (newNode.NodeType == (int)DTUConfigureUtil.NodeType.ThirdLevelNode && newNode.NodeName.Equals(DTUConfigureUtil.GetInstance().PowerSupplyOne))
                {
                    newNode.Path = "/1/" + nodeNumber + "/" + (nodeNumber + 3) + "/" + (nodeNumber + 4);
                }
                else if (newNode.NodeType == (int)DTUConfigureUtil.NodeType.LeafNode && newNode.NodeName.Equals(DTUConfigureUtil.GetInstance().MSideChild))
                {
                    newNode.Path = "/1/" + nodeNumber + "/" + (nodeNumber + 1) + "/" + (nodeNumber + 5);
                }
                else if (newNode.NodeType == (int)DTUConfigureUtil.NodeType.LeafNode && newNode.NodeName.Equals(DTUConfigureUtil.GetInstance().NSideChild))
                {
                    newNode.Path = "/1/" + nodeNumber + "/" + (nodeNumber + 2) + "/" + (nodeNumber + 6);
                }
                else if (newNode.NodeType == (int)DTUConfigureUtil.NodeType.LeafNode && newNode.NodeName.Equals(DTUConfigureUtil.GetInstance().PowerSupplyOneChild))
                {
                    newNode.Path = "/1/" + nodeNumber + "/" + (nodeNumber + 3) + "/" + (nodeNumber + 4) + "/" + (nodeNumber + 7);
                }
            }

            //将初始表插入现有的数据结构中
            foreach (Node initNode in InitTable)
            {
                DTUConfigureDaoObject.InsertNode(initNode);
            }

            // 为新插入的节点们找爸爸,同时重新加载新的配置树
            string reloadDTUConfigureTree = "reloadTree";
            Messenger.Default.Send<string>(reloadDTUConfigureTree, "reloadDTUConfigureTree");
        }

        /// <summary>
        /// 添加三级节点到树结构中
        /// </summary>
        public void AddNewThirdLevelNode(string parentNodePath)
        {
            DTUConfigureDao DTUConfigureDaoObject = new DTUConfigureDao();
            List<Node> initTableThirdLevel = (List<Node>)DTUConfigureDaoObject.QueryDTUConfigureInitThirdLevel();

            // 获取数据库中最大的NodeID值
            int nextRecord = 1;
            int nodeNumber = DTUConfigureDaoObject.QueryMaxNodeID() + nextRecord;
            string fourthLevel = null;

            foreach (Node newNode in initTableThirdLevel)
            {
                if (newNode.NodeType == (int)DTUConfigureUtil.NodeType.ThirdLevelNode)
                {
                    newNode.Path = parentNodePath + "/" + nodeNumber;
                    fourthLevel = newNode.Path;
                }
            }

            foreach (Node newNode in initTableThirdLevel)
            {
                if (newNode.NodeType == (int)DTUConfigureUtil.NodeType.LeafNode)
                {
                    if (fourthLevel != null)
                    {
                        newNode.Path = fourthLevel + "/" + (nodeNumber + nextRecord);
                    }
                }
            }

            //将初始表插入现有的数据结构中
            foreach (Node initNode in initTableThirdLevel)
            {
                DTUConfigureDaoObject.InsertNode(initNode);
            }

            // 为新插入的节点们找爸爸,同时重新加载新的配置树
            string reloadDTUConfigureTree = "reloadTree";
            Messenger.Default.Send<string>(reloadDTUConfigureTree, "reloadDTUConfigureTree");
        }

        /// <summary>
        /// 添加叶子节点
        /// </summary>
        public void AddNewLeafNode(string parentNodePath)
        {
            DTUConfigureDao DTUConfigureDaoObject = new DTUConfigureDao();
            // 获取数据库中最大的NodeID值
            int nextRecord = 1;
            int nodeNumber = DTUConfigureDaoObject.QueryMaxNodeID() + nextRecord;

            // 弹出配置项的对话框
            AddNewConfigureLeafNode newConfigureLeafNode = new AddNewConfigureLeafNode(true);
            newConfigureLeafNode.ShowDialog();

            // 将用户的输入组织成一个新的叶节点
            Node newLeafNode = new Node();
            if (AddNewConfigureLeafNodeViewModel.nodeNameInput != null)
            {
                newLeafNode.NodeName = AddNewConfigureLeafNodeViewModel.nodeNameInput;
            }
            else
            {
                MessageBox.Show("请设置该配置项的名称！", "警告");
                return;
            }

            if (AddNewConfigureLeafNodeViewModel.nodeIP != null)
            {
                newLeafNode.NodeIP = AddNewConfigureLeafNodeViewModel.nodeIP;
            }
            else
            {
                MessageBox.Show("请设置该配置项的IP地址！", "警告");
                return;
            }

            newLeafNode.NodeBreakersOrTieSwitch = AddNewConfigureLeafNodeViewModel.selectedBreakerOrTieSwitch;
            newLeafNode.NodeMainOrBranchLine = AddNewConfigureLeafNodeViewModel.selectedMainOrBranch;
            newLeafNode.NodeType = (int)DTUConfigureUtil.NodeType.LeafNode;

            newLeafNode.Path = parentNodePath + "/" + nodeNumber;

            // 将新节点加入到数据库中
            DTUConfigureDaoObject.InsertNode(newLeafNode);

            // 为新插入的节点们找爸爸,同时重新加载新的配置树
            string reloadDTUConfigureTree = "reloadTree";
            Messenger.Default.Send<string>(reloadDTUConfigureTree, "reloadDTUConfigureTree");
        }

        /// <summary>
        /// 执行DTU参数配置界面的按钮动作
        /// </summary>
        /// <param name="arg">按钮对应的命令参数</param>
        public void ExcuteDTUConfigureCommand(string arg)
        {
            switch (arg)
            {
                // 下发配置信息
                case "DownloadConfigure":
                    if (!CommunicationViewModel.IsLinkConnect())
                    {
                        return;
                    }
                    List<byte> buffer = new List<byte>();
                    OrganizeDTUConfigureFrame(buffer);
                    string DTUconfigFilePath = DownDTUConfigure(buffer);

                    if (DTUconfigFilePath != null && DTUconfigFilePath.Trim().Count() > 0)
                    {
                        FileServiceViewModel.SelectFile(DTUconfigFilePath);
                        FileServiceViewModel.WriteFileAct(DTUconfigFilePath);
                    }
                    break;
            }
        }

        /// <summary>
        /// 构建下发DTU配置信息的报文
        /// </summary>
        /// <param name="buffer">构建的报文缓冲区</param>
        public void OrganizeDTUConfigureFrame(List<byte> buffer)
        {
            buffer.Clear();

            DTUConfigureDao DTUConfigureDaoObject = new DTUConfigureDao();
            // M侧节点
            Node MSideNode = new Node();
            // N侧节点
            Node NSiseNode = new Node();
            // 供电链路节点
            Node PowerSupplyNode = new Node();
            // M侧子节点集合
            List<Node> MSideNodeCollection = new List<Node>();
            // N侧子节点集合
            List<Node> NSideNodeCollection = new List<Node>();
            // 供电链路节点集合
            List<Node> PowerSupplyNodeCollection = new List<Node>();
            // 供电链路叶节点集合
            List<List<Node>> PowerSupplyLeafNodeCollection = new List<List<Node>>();

            // 所有表的集合
            List<Node> TableCollection = (List<Node>)DTUConfigureDaoObject.QueryByNodeType((int)DTUConfigureUtil.NodeType.FirstLevelNode);

            buffer.Add((byte)TableCollection.Count);

            // 遍历每个表的所有节点
            List<Node> TableChilrenNodes = new List<Node>();
            foreach (Node TableX in TableCollection)
            {
                // 清空所有缓存空间
                TableChilrenNodes.Clear();
                MSideNode = null;
                NSiseNode = null;
                PowerSupplyNode = null;
                MSideNodeCollection.Clear();
                NSideNodeCollection.Clear();
                PowerSupplyNodeCollection.Clear();
                PowerSupplyLeafNodeCollection.Clear();

                // 查询表x的所有子节点
                TableChilrenNodes = (List<Node>)DTUConfigureDaoObject.QueryNodeChildren(TableX.Path);
                // 查找二级节点，即为M侧 N侧 供电链表侧
                foreach (Node TableChilrenNode in TableChilrenNodes)
                {
                    if (TableChilrenNode.NodeType == (int)DTUConfigureUtil.NodeType.SecondLevelNode)
                    {
                        if (TableChilrenNode.NodeName == DTUConfigureUtil.GetInstance().MSide)
                        {
                            MSideNode = TableChilrenNode;
                        }
                        else if (TableChilrenNode.NodeName == DTUConfigureUtil.GetInstance().NSide)
                        {
                            NSiseNode = TableChilrenNode;
                        }
                        else if (TableChilrenNode.NodeName == DTUConfigureUtil.GetInstance().PowerSupply)
                        {
                            PowerSupplyNode = TableChilrenNode;
                        }
                    }
                }

                // 查找M N 供电链路的直接子节点集合
                if (MSideNode != null && NSiseNode != null && PowerSupplyNode != null)
                {
                    // 删除二级节点本身
                    TableChilrenNodes.Remove(MSideNode);
                    TableChilrenNodes.Remove(NSiseNode);
                    TableChilrenNodes.Remove(PowerSupplyNode);

                    foreach (Node TableChilrenNode in TableChilrenNodes)
                    {
                        if (TableChilrenNode.Path.Contains(MSideNode.Path))
                        {
                            MSideNodeCollection.Add(TableChilrenNode);
                        }
                        else if (TableChilrenNode.Path.Contains(NSiseNode.Path))
                        {
                            NSideNodeCollection.Add(TableChilrenNode);
                        }
                        else if (TableChilrenNode.NodeType == (int)DTUConfigureUtil.NodeType.ThirdLevelNode)
                        {
                            PowerSupplyNodeCollection.Add(TableChilrenNode);
                        }
                    }
                }
                else
                {
                    // 树形结构错误
                    MessageBox.Show("DTU配置树形结构错误", "警告");
                    return;
                }

                // 查找供电链路x的直接子节点
                foreach (Node powerSupplyNodeCollection in PowerSupplyNodeCollection)
                {
                    // 缓存供电链路x的所有集合
                    List<Node> powerSupplyXNodeCollection = new List<Node>();

                    foreach (Node TableChilrenNode in TableChilrenNodes)
                    {
                        if (TableChilrenNode.Path.Contains(powerSupplyNodeCollection.Path) && TableChilrenNode.Path != powerSupplyNodeCollection.Path)
                        {
                            powerSupplyXNodeCollection.Add(TableChilrenNode);
                        }
                    }
                    PowerSupplyLeafNodeCollection.Add(powerSupplyXNodeCollection);
                }

                #region 构建一个表的数组
                try
                {
                    // 遍历结束一个表，构建一个表的数组
                    // 计算供电链表的数量
                    int powerSupplyLeafNodeCollectionCount = 0;
                    foreach (List<Node> powerSupplyLeafNodeCollection in PowerSupplyLeafNodeCollection)
                    {
                        powerSupplyLeafNodeCollectionCount += powerSupplyLeafNodeCollection.Count;
                    }
                    //            表x的属性  M侧数量    M侧个数的字节总数        N侧数量     N侧个数的字节总数       链表数量   链表x的个数                    链表叶节点的个数
                    int tablexByteCount = 3 + 1 + MSideNodeCollection.Count * 3 + 1 + NSideNodeCollection.Count * 3 + 1 + PowerSupplyLeafNodeCollection.Count + powerSupplyLeafNodeCollectionCount * 3;
                    short tablexByteCountInt16 = (short)tablexByteCount;
                    buffer.Add((byte)(tablexByteCount & 0x00ff));
                    buffer.Add((byte)((tablexByteCount & 0xff00) >> 8));
                    buffer.Add(byte.Parse(TableX.NodeIP.Substring(TableX.NodeIP.LastIndexOf('.') + 1)));
                    buffer.Add((byte)TableX.NodeBreakersOrTieSwitch);
                    buffer.Add((byte)TableX.NodeMainOrBranchLine);

                    // M侧
                    buffer.Add((byte)MSideNodeCollection.Count);
                    foreach (Node MNode in MSideNodeCollection)
                    {
                        buffer.Add(byte.Parse(MNode.NodeIP.Substring(MNode.NodeIP.LastIndexOf('.') + 1)));
                        buffer.Add((byte)MNode.NodeBreakersOrTieSwitch);
                        buffer.Add((byte)MNode.NodeMainOrBranchLine);
                    }

                    // N侧
                    buffer.Add((byte)NSideNodeCollection.Count);
                    foreach (Node NNode in NSideNodeCollection)
                    {
                        buffer.Add(byte.Parse(NNode.NodeIP.Substring(NNode.NodeIP.LastIndexOf('.') + 1)));
                        buffer.Add((byte)NNode.NodeBreakersOrTieSwitch);
                        buffer.Add((byte)NNode.NodeMainOrBranchLine);
                    }

                    // 供电链表侧
                    buffer.Add((byte)PowerSupplyLeafNodeCollection.Count);

                    foreach (List<Node> powerSupplyLeafNodeCollection in PowerSupplyLeafNodeCollection)
                    {
                        buffer.Add((byte)powerSupplyLeafNodeCollection.Count);
                        foreach (Node powerSupplyLeafNode in powerSupplyLeafNodeCollection)
                        {
                            buffer.Add(byte.Parse(powerSupplyLeafNode.NodeIP.Substring(powerSupplyLeafNode.NodeIP.LastIndexOf('.') + 1)));
                            buffer.Add((byte)powerSupplyLeafNode.NodeBreakersOrTieSwitch);
                            buffer.Add((byte)powerSupplyLeafNode.NodeMainOrBranchLine);
                        }
                    }

                    FileServiceViewModel.filebuf = new byte[buffer.Count];
                    FileServiceViewModel.fileSize = buffer.Count;
                    for (int i = 0; i < buffer.Count; ++i)
                    {
                        FileServiceViewModel.filebuf[i] = buffer[i];
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("请检查配置项IP地址是否有误" + ex.ToString(), "异常");
                }
                #endregion 构建一个表的数组
            }
        }

        /// <summary>
        /// 下发配置到设备
        /// </summary>
        /// <param name="buffer">分布式配置内容</param>
        /// <returns></returns>
        private string DownDTUConfigure(List<byte> buffer)
        {
            try
            {
                //string primaryPath = @".\Config";
                string primaryName = "GridStructureSet" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".cfg";
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Config (*.cfg)|*.cfg";

                //是否自动添加扩展名
                saveFileDialog.AddExtension = true;
                //文件已存在是否提示覆盖
                saveFileDialog.OverwritePrompt = true;
                //提示输入的文件名无效
                saveFileDialog.CheckPathExists = true;

                // 文件初始名
                saveFileDialog.FileName = primaryName;

                if (saveFileDialog.ShowDialog() == true)
                {
                    byte[] AllBuff = buffer.ToArray();

                    System.IO.File.WriteAllBytes(saveFileDialog.FileName, AllBuff);

                    MessageBox.Show("操作成功");

                    return saveFileDialog.FileName;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return null;
        }
    }
}
