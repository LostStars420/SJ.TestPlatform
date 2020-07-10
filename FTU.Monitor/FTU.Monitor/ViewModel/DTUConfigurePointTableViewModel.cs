using FTU.Monitor.Dao;
using FTU.Monitor.Model.DTUConfigurePointTableModelCollection;
using FTU.Monitor.Util;
using FTU.Monitor.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// DTUConfigurePointTableViewModel 的摘要说明
    /// author: liyan
    /// date：2018/7/30 21:02:03
    /// desc：DTU三遥点表配置交互逻辑
    /// version: 1.0
    /// </summary>
    public class DTUConfigurePointTableViewModel : ViewModelBase
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public DTUConfigurePointTableViewModel()
        {
            this._DTUConfigurePointTableAllNodes = new List<DTUNode>();
            this._currentTeleSignalisation = new ObservableCollection<DTUPointTableModuleModel>();
            this._currentTeleMetering = new ObservableCollection<DTUPointTableModuleModel>();
            this._currentTeleControl = new ObservableCollection<DTUPointTableModuleModel>();
            DTUConfigurePointTableDaoObject = new DTUConfigurePointTableDao();
            Messenger.Default.Register<int>(this,"GetCurrentModulePointTable", ExcuteGetCurrentModulePointTable);
            DataGridMenumSelected = new RelayCommand<string>(ExecuteDataGridMenumSelected);

            // 模板数据表格选中索引数组
            this._moduleGridIndex = new int[3];
            for (int i = 0; i < 3; i++)
            {
                this._moduleGridIndex[i] = -1;
            }
        }

        private DTUConfigurePointTableDao DTUConfigurePointTableDaoObject;
        /// <summary>
        /// 显示界面的模块遥信点号集合
        /// </summary>
        private ObservableCollection<DTUPointTableModuleModel> _currentTeleSignalisation;

        /// <summary>
        /// 设置和获取显示界面的模块遥信点号集合
        /// </summary>
        public ObservableCollection<DTUPointTableModuleModel> CurrentTeleSignalisation
        {
            get
            {
                return this._currentTeleSignalisation;
            }
            set
            {
                this._currentTeleSignalisation = value;
                RaisePropertyChanged(() => CurrentTeleSignalisation);
            }
        }

        /// <summary>
        /// 显示界面的模块遥测点号集合
        /// </summary>
        private ObservableCollection<DTUPointTableModuleModel> _currentTeleMetering;

        /// <summary>
        /// 设置和获取显示界面的模块遥测点号集合
        /// </summary>
        public ObservableCollection<DTUPointTableModuleModel> CurrentTeleMetering
        {
            get
            {
                return this._currentTeleMetering;
            }
            set
            {
                this._currentTeleMetering = value;
                RaisePropertyChanged(() => CurrentTeleMetering);
            }
        }

        /// <summary>
        /// 显示界面的模块遥控点号集合
        /// </summary>
        private ObservableCollection<DTUPointTableModuleModel> _currentTeleControl;

        /// <summary>
        /// 设置和获取显示界面的模块遥控点号集合
        /// </summary>
        public ObservableCollection<DTUPointTableModuleModel> CurrentControl
        {
            get
            {
                return this._currentTeleControl;
            }
            set
            {
                this._currentTeleControl = value;
                RaisePropertyChanged(() => CurrentControl);
            }
        }

        /// <summary>
        /// 所有配置列表
        /// </summary>
        private List<DTUNode> _DTUConfigurePointTableAllNodes;

        /// <summary>
        /// 设置和获取所有配置列表
        /// </summary>
        private List<DTUNode> DTUConfigurePointTableAllNodes
        {
            get
            {
                return this._DTUConfigurePointTableAllNodes;
            }
            set
            {
                this._DTUConfigurePointTableAllNodes = value;
            }
        }

        /// <summary>
        /// 得到DTU配置模块树
        /// </summary>
        /// <returns></returns>
        public List<DTUNode> GetNodeList()
        {
            // 获取树结构中的所有节点
            DTUConfigurePointTableAllNodes = (List<DTUNode>)DTUConfigurePointTableDaoObject.Query();

            foreach (var treeNode in DTUConfigurePointTableAllNodes)
            {
                treeNode.FindParent(DTUConfigurePointTableAllNodes);
            }

            List<DTUNode> root = new List<DTUNode>();
            foreach (var treeNode in DTUConfigurePointTableAllNodes)
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
        /// 添加一个配置模块
        /// </summary>
        public void AddNewCongigureModule(string parentNodePath)
        {
            // 获取数据库中最大的NodeID值
            int nextRecord = 1;
            int nodeNumber = DTUConfigurePointTableDaoObject.QueryMaxNodeID() + nextRecord;

            DTUNode newModuleNode = new DTUNode();
            newModuleNode.NodeName = "模板";
            newModuleNode.NodeType = (int)DTUConfigureUtil.NodeType.FirstLevelNode;
            newModuleNode.Path = parentNodePath + "/" + nodeNumber;

            // 将新节点加入到数据库中
            DTUConfigurePointTableDaoObject.InsertNode(newModuleNode);
            // 为新插入的节点们找爸爸,同时重新加载新的配置树
            Messenger.Default.Send<string>("AddNewNode", "reloadDTUConfigureModuleTree");
        }

        /// <summary>
        /// 获取当前选中模块的所有模板点表，并显示
        /// </summary>
        /// <param name="NodeID"></param>
        private void ExcuteGetCurrentModulePointTable(int NodeID)
        {
            // 清空历史显示记录
            CurrentTeleSignalisation.Clear();
            CurrentTeleMetering.Clear();
            CurrentControl.Clear();

            // 获取当前模块的遥信
            IList<DTUEachModulePointTableModel> teleSignalisationList = DTUConfigurePointTableDaoObject.QueryByPointTypeAndBelongToModule(ConfigUtil.getPointTypeID("遥信"), NodeID);
            if(teleSignalisationList != null)
            {
                int number = 0;
                foreach(var teleSignalisation in teleSignalisationList)
                {
                    DTUPointTableModuleModel DTUPointTableModuleTmp = new DTUPointTableModuleModel();
                    DTUPointTableModuleTmp.Number = ++number;
                    DTUPointTableModuleTmp.PointNumber = teleSignalisation.PointNumber;
                    DTUPointTableModuleTmp.PointName = teleSignalisation.PointName;

                    CurrentTeleSignalisation.Add(DTUPointTableModuleTmp);
                }
            }

            // 获取当前模块的遥测
            IList<DTUEachModulePointTableModel> teleMeteringList = DTUConfigurePointTableDaoObject.QueryByPointTypeAndBelongToModule(ConfigUtil.getPointTypeID("遥测"), NodeID);
            if (teleMeteringList != null)
            {
                int number = 0;
                foreach (var teleMetering in teleMeteringList)
                {
                    DTUPointTableModuleModel DTUPointTableModuleTmp = new DTUPointTableModuleModel();
                    DTUPointTableModuleTmp.Number = ++number;
                    DTUPointTableModuleTmp.PointNumber = teleMetering.PointNumber;
                    DTUPointTableModuleTmp.PointName = teleMetering.PointName;

                    CurrentTeleMetering.Add(DTUPointTableModuleTmp);
                }
            }

            // 获取当前模块的遥控
            IList<DTUEachModulePointTableModel> teleControlList = DTUConfigurePointTableDaoObject.QueryByPointTypeAndBelongToModule(ConfigUtil.getPointTypeID("遥控"), NodeID);
            if (teleMeteringList != null)
            {
                int number = 0;
                foreach (var teleControl in teleControlList)
                {
                    DTUPointTableModuleModel DTUPointTableModuleTmp = new DTUPointTableModuleModel();
                    DTUPointTableModuleTmp.Number = ++number;
                    DTUPointTableModuleTmp.PointNumber = teleControl.PointNumber;
                    DTUPointTableModuleTmp.PointName = teleControl.PointName;

                    CurrentControl.Add(DTUPointTableModuleTmp);
                }
            }

        }

        /// <summary>
        /// 保存当前模块的点表配置，存入数据库中
        /// </summary>
        /// <param name="nodeID"></param>
        private void SaveCurrentModuleConfig(int nodeID)
        {
            // 删除数据库中原有的选中模块的所有记录
            int result = DTUConfigurePointTableDaoObject.DeleteByBelongToModule(nodeID);
            if(result <= 0)
            {
                MessageBox.Show("删除当前模块中的点表出错","警告");
                return;
            }

            // 将当前模块配置的遥信点表插入到数据库中
            List<DTUEachModulePointTableModel> teleSignalisationList = new List<DTUEachModulePointTableModel>();
            foreach (var currentTeleSignalisation in CurrentTeleSignalisation)
            {
                DTUEachModulePointTableModel teleSignalisation = new DTUEachModulePointTableModel();
                teleSignalisation.PointType = ConfigUtil.getPointTypeID("遥信");
                teleSignalisation.BelongToModule = nodeID;
                teleSignalisation.PointNumber = currentTeleSignalisation.PointNumber;
                teleSignalisation.PointName = currentTeleSignalisation.PointName;

                teleSignalisationList.Add(teleSignalisation);

            }

            // 将当前模块配置的遥测点表插入到数据库中
            List<DTUEachModulePointTableModel> teleMeteringList = new List<DTUEachModulePointTableModel>();
            foreach (var currentTeleMetering in CurrentTeleMetering)
            {
                DTUEachModulePointTableModel teleMetering = new DTUEachModulePointTableModel();
                teleMetering.PointType = ConfigUtil.getPointTypeID("遥测");
                teleMetering.BelongToModule = nodeID;
                teleMetering.PointNumber = currentTeleMetering.PointNumber;
                teleMetering.PointName = currentTeleMetering.PointName;

                teleMeteringList.Add(teleMetering);
            }

            // 将当前模块配置的遥控点表插入到数据库中
            List<DTUEachModulePointTableModel> teleControlList = new List<DTUEachModulePointTableModel>();
            foreach (var currentControl in CurrentControl)
            {
                DTUEachModulePointTableModel teleControl = new DTUEachModulePointTableModel();
                teleControl.PointType = ConfigUtil.getPointTypeID("遥控");
                teleControl.BelongToModule = nodeID;
                teleControl.PointNumber = currentControl.PointNumber;
                teleControl.PointName = currentControl.PointName;

                teleControlList.Add(teleControl);
            }
            // 将当前模块的配置点表增加到数据库中
            DTUConfigurePointTableDaoObject.InsertConfigurePointTable(teleSignalisationList);
            DTUConfigurePointTableDaoObject.InsertConfigurePointTable(teleMeteringList);
            DTUConfigurePointTableDaoObject.InsertConfigurePointTable(teleControlList);

        }

        # region 模板点表鼠标右键操作逻辑，包括新增行、删除行
        /// <summary>
        /// 数据表格选中索引数组
        /// </summary>
        private int[] _moduleGridIndex;

        /// <summary>
        /// 设置和获取数据表格选中索引数组
        /// </summary>
        public int[] ModuleGridIndex
        {
            get
            {
                return this._moduleGridIndex;
            }
            set
            {
                this._moduleGridIndex = value;
                RaisePropertyChanged(() => ModuleGridIndex);
            }
        }

        /// <summary>
        /// 模板点表表格右键相关操作命令
        /// </summary>
        public RelayCommand<string> DataGridMenumSelected
        {
            get;
            private set;
        }

        /// <summary>
        /// SOE数据表格右键相关操作命令执行的方法
        /// </summary>
        /// <param name="arg">参数</param>
        private void ExecuteDataGridMenumSelected(string arg)
        {
            try
            {
                switch (arg)
                {
                    // 在选中行上面插入一行
                    case "YXAddUp":
                        if (ModuleGridIndex[0] > -1)
                        {
                            var item = new DTUPointTableModuleModel { Number = ModuleGridIndex[0], PointNumber = "添加点号", PointName = "添加名称" };
                            CurrentTeleSignalisation.Insert(ModuleGridIndex[0], item);
                            //对序号列重新排序
                            for(int i = 0; i < CurrentTeleSignalisation.Count; i++)
                            {
                                CurrentTeleSignalisation[i].Number = i + 1;
                            }
                        }
                        break;

                    // 在选中行下面插入一行
                    case "YXAddDown":
                        //if (SelectedIndex > -1)
                        //{
                        //    var item = new Telesignalisation(0, "xxx", 0, "否", 0, "xxx", "xxx", "StateA", "StateB");
                        //    if (SelectedIndex < UserData.Count - 1)
                        //    {

                        //        UserData.Insert(SelectedIndex + 1, item);
                        //    }
                        //    else
                        //    {
                        //        UserData.Add(item);
                        //    }
                        //}
                        break;

                    // 删除选中行
                    case "YXDeleteSelect":
                        //if (SelectedIndex > -1)
                        //{
                        //    //var result = MessageBox.Show("是否删除选中行:" + gridTelesignalisation.SelectedItem.ToString(),
                        //    //    "确认删除", MessageBoxButton.OKCancel);
                        //    var result = true;
                        //    if (result)
                        //    {
                        //        UserData.RemoveAt(SelectedIndex);
                        //    }
                        //}
                        break;
                    case "SaveConfigure":
                        SaveCurrentModuleConfig(DTUConfigurePointTableView.CurrentNode.NodeID);
                        break;

                }
            }
            catch (Exception ex)
            {
                CommunicationViewModel.con.DebugLog(ex.ToString());
            }
        }


        #endregion 模板点表鼠标右键操作逻辑，包括新增行、删除行

    }
}
