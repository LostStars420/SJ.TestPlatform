using FTU.Monitor.Model;
using FTU.Monitor.Service;
using FTU.Monitor.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// CombineTelesignalisationManagementViewModel 的摘要说明
    /// author: songminghao
    /// date：2018/3/21 15:27:58
    /// desc：组合遥信管理ViewModel
    /// version: 1.0
    /// </summary>
    public class CombineTelesignalisationManagementViewModel : ViewModelBase
    {
        #region 组合遥信管理

        /// <summary>
        /// 组合遥信点表中选中点号索引
        /// </summary>
        private int _combineTelesignalisationGridIndex;

        /// <summary>
        /// 设置和获取组合遥信点表中选中点号索引
        /// </summary>
        public int CombineTelesignalisationGridIndex
        {
            get
            {
                return this._combineTelesignalisationGridIndex;
            }
            set
            {
                this._combineTelesignalisationGridIndex = value;
                RaisePropertyChanged(() => CombineTelesignalisationGridIndex);
            }
        }

        /// <summary>
        /// 组合遥信点表集合
        /// </summary>
        private ObservableCollection<Telesignalisation> combineTelesignalisationList;

        /// <summary>
        /// 设置和获取组合遥信点表集合
        /// </summary>
        public ObservableCollection<Telesignalisation> CombineTelesignalisationList
        {
            get
            {
                return combineTelesignalisationList;
            }
            set
            {
                combineTelesignalisationList = value;
                RaisePropertyChanged(() => CombineTelesignalisationList);
            }
        }

        /// <summary>
        /// 遥信原始点表中选中点号索引
        /// </summary>
        private int _gridIndex;

        /// <summary>
        /// 设置和获取遥信原始点表中选中点号索引
        /// </summary>
        public int GridIndex
        {
            get
            {
                return this._gridIndex;
            }
            set
            {
                this._gridIndex = value;
                RaisePropertyChanged(() => GridIndex);
            }
        }

        /// <summary>
        /// 新增组合遥信点号的内容
        /// </summary>
        private string _combineTelesignalisationContent;

        /// <summary>
        /// 设置和获取新增组合遥信点号的内容
        /// </summary>
        public string CombineTelesignalisationContent
        {
            get
            {
                return this._combineTelesignalisationContent;
            }
            set
            {
                this._combineTelesignalisationContent = value;
                RaisePropertyChanged(() => CombineTelesignalisationContent);
            }
        }

        /// <summary>
        /// 新增组合遥信点号的名称
        /// </summary>
        private string _combineTelesignalisationName;

        /// <summary>
        /// 设置和获取新增组合遥信点号的名称
        /// </summary>
        public string CombineTelesignalisationName
        {
            get
            {
                return this._combineTelesignalisationName;
            }
            set
            {
                this._combineTelesignalisationName = value;
                RaisePropertyChanged(() => CombineTelesignalisationName);
            }
        }

        /// <summary>
        /// 遥信原始点表集合
        /// </summary>
        public ObservableCollection<Telesignalisation> telesignalisationSourceList;

        /// <summary>
        /// 获取和设置遥信原始点表集合
        /// </summary>
        public ObservableCollection<Telesignalisation> TelesignalisationSourceList
        {
            get
            {
                return telesignalisationSourceList;
            }
            set
            {
                telesignalisationSourceList = value;
                RaisePropertyChanged(() => TelesignalisationSourceList);
            }
        }

        /// <summary>
        /// 遥信原始点表中的点号数组
        /// </summary>
        private List<int> telesignalisationSourcePointIDIndex;

        /// <summary>
        /// 逻辑运算命令
        /// </summary>
        public RelayCommand<string> LogicOperationCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 逻辑运算命令执行操作
        /// </summary>
        /// <param name="arg">参数</param>
        public void ExecuteLogicOperationCommand(string arg)
        {
            switch(arg)
            {
                case "AND":
                    CombineTelesignalisationContent += "&";
                    break;

                case "OR":
                    CombineTelesignalisationContent += "|";
                    break;

                case "NOT":
                    CombineTelesignalisationContent += "!";
                    break;
            }
        }

        /// <summary>
        /// 保存组合遥信命令
        /// </summary>
        public RelayCommand SaveCombineTelesignalisationCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 保存组合遥信命令执行操作
        /// </summary>
        public void ExecuteSaveCombineTelesignalisationCommand()
        {
            // 检查组合遥信名称是否合法
            if (CombineTelesignalisationName == null || "".Equals(CombineTelesignalisationName.Trim()))
            {
                MessageBox.Show("组合遥信名称不能为空", "提示");
                return;
            }
            
            // 定义组合遥信管理业务逻辑处理(service)类对象
            CombineTelesignalisationManageService combineTelesignalisationManageService = new CombineTelesignalisationManageService();
            // 检查组合遥信内容是否合法
            string msg = combineTelesignalisationManageService.CheckCombineTelesignalisationName(CombineTelesignalisationContent);
            if (!UtilHelper.IsEmpty(msg))
            {
                MessageBox.Show(msg, "提示");
                return;
            }
            
            // 定义组合遥信点号对象
            DevPoint combineTelesignalisationPoint = new DevPoint();
            combineTelesignalisationPoint.ID = CombineTelesignalisationManageService.GetMaxTelesignalisationPointID().ToString("x4").ToUpper();
            combineTelesignalisationPoint.Name = Regex.Replace(CombineTelesignalisationName.Trim(), @"\s", "");
            combineTelesignalisationPoint.PointTypeId = ConfigUtil.getPointTypeID("遥信");
            combineTelesignalisationPoint.PointType = "遥信";
            combineTelesignalisationPoint.Comment = Regex.Replace(CombineTelesignalisationContent.Trim(), @"\s", "");
            combineTelesignalisationPoint.Flag = 1;

            Telesignalisation telesignalisationCombine = combineTelesignalisationManageService.SaveCombineTelesignalisation(combineTelesignalisationPoint);
            if (telesignalisationCombine != null)
            {
                MessageBox.Show("新增组合遥信点号成功", "提示");

                // 设置该组合遥信序号
                telesignalisationCombine.Number = CombineTelesignalisationList.Count + 1;
                // 将该组合要信添加到组合遥信点表集合中
                CombineTelesignalisationList.Add(telesignalisationCombine);
                TelesignalisationSourceList.Add(telesignalisationCombine);

                // 获取遥信点号个数
                int telesignalisationPointCount = combineTelesignalisationManageService.GetCountByPointTypeId(ConfigUtil.getPointTypeID("遥信"));
                Telesignalisation telesignalisation = new Telesignalisation();
                telesignalisation = telesignalisationCombine;
                telesignalisation.Number = telesignalisationPointCount + 1;

                // 发送更新配置管理页面的遥信点表消息,新增组合遥信后重新加载配置参数遥信点表
                Messenger.Default.Send<Telesignalisation>(telesignalisation, "UpdateTelesignalisationPoint");

                return;
            }
            MessageBox.Show("新增组合遥信点号失败", "提示");

        }

        /// <summary>
        /// 遥信原始点表中选中点号索引命令
        /// </summary>
        public RelayCommand SelectTelesignalisationPointCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 遥信原始点表中选中点号索引命令执行操作
        /// </summary>
        public void ExecuteSelectTelesignalisationPointCommand()
        {
            if (GridIndex != -1)
            {
                CombineTelesignalisationContent += TelesignalisationSourceList[GridIndex].ID.ToString("x4").ToUpper()
                    + "(" + TelesignalisationSourceList[GridIndex].Name + ")";
            }
        }

        /// <summary>
        /// 删除组合遥信点号命令
        /// </summary>
        public RelayCommand DeleteCombineTelesignalisationCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 删除组合遥信点号命令执行操作
        /// </summary>
        public void ExecuteDeleteCombineTelesignalisationCommand()
        {
            if (CombineTelesignalisationGridIndex != -1)
            {
                // 定义组合遥信管理业务逻辑处理(service)类对象
                CombineTelesignalisationManageService combineTelesignalisationManageService = new CombineTelesignalisationManageService();
                // 根据遥信点号流水号删除遥信点号
                int flag = combineTelesignalisationManageService.DeleteByDevpid(CombineTelesignalisationList[CombineTelesignalisationGridIndex].Devpid);
                if (flag == -1)
                {
                    MessageBox.Show("点号" + CombineTelesignalisationList[CombineTelesignalisationGridIndex].ID.ToString("X4") + "被使用,不能删除", "提示");
                }
                else
                {
                    MessageBox.Show("点号" + CombineTelesignalisationList[CombineTelesignalisationGridIndex].ID.ToString("X4") + "删除成功", "提示");
                    // 重新加载组合遥信点表
                    LoadTelesignalisationPointByFlag(1);
                    // 发送更新配置管理页面的遥信点表消息,重新加载配置参数遥信点表
                    Messenger.Default.Send<Telesignalisation>(null, "UpdateTelesignalisationPoint");
                }
            }            
        }

        #endregion 组合遥信管理

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public CombineTelesignalisationManagementViewModel()
        {
            // 注册接收更新相应页面的点表消息(点表重新导入后，需要更新相应页面显示的点表)
            Messenger.Default.Register<object>(this, "UpdateSourcePoint", ReloadCombineTelesignalisationPoint);

            LogicOperationCommand = new RelayCommand<string>(ExecuteLogicOperationCommand);
            SaveCombineTelesignalisationCommand = new RelayCommand(ExecuteSaveCombineTelesignalisationCommand);
            SelectTelesignalisationPointCommand = new RelayCommand(ExecuteSelectTelesignalisationPointCommand);
            DeleteCombineTelesignalisationCommand = new RelayCommand(ExecuteDeleteCombineTelesignalisationCommand);

            combineTelesignalisationList = new ObservableCollection<Telesignalisation>();
            telesignalisationSourceList = new ObservableCollection<Telesignalisation>();

            this._combineTelesignalisationContent = "";
            this._combineTelesignalisationName = "";

            // 加载遥信原始点表和组合遥信点表
            ReloadCombineTelesignalisationPoint(null);
            
        }

        /// <summary>
        /// 重新载入组合遥信相关点表
        /// </summary>
        /// <param name="obj"></param>
        public void ReloadCombineTelesignalisationPoint(object obj)
        {
            // 加载遥信点表
            LoadTelesignalisationPointByFlag(0);
            LoadTelesignalisationPointByFlag(1);
        }

        /// <summary>
        /// 根据组合遥信标志位加载点号列表
        /// </summary>
        /// <param name="flag">组合遥信标志位(1代表是组合遥信,0代表原始遥信点号)</param>
        public void LoadTelesignalisationPointByFlag(int flag)
        {
            // 定义组合遥信管理业务逻辑处理(service)类对象
            CombineTelesignalisationManageService combineTelesignalisationManageService = new CombineTelesignalisationManageService();
            // 根据点号类型编号和组合遥信标志位查询此类型下的点表集合
            IList<Telesignalisation> telesignalisationListTemp = combineTelesignalisationManageService.GetTelesignalisationPointByFlag(flag);

            // 加载原始遥信点号列表
            if (flag == 0)
            {
                // 清空遥信原始点表集合
                telesignalisationSourceList.Clear();
                // 初始化遥信原始点表的点号数组
                telesignalisationSourcePointIDIndex = new List<int>();
                if (telesignalisationListTemp != null && telesignalisationListTemp.Count > 0)
                {
                    foreach (var telesignalisation in telesignalisationListTemp)
                    {
                        // 将原始遥信点表点号添加到遥信原始点表的点号数组
                        telesignalisationSourcePointIDIndex.Add(telesignalisation.ID);
                        // 将原始遥信点表添加到遥信原始点表
                        telesignalisationSourceList.Add(telesignalisation);
                    }
                }
            }
            else if (flag == 1)
            {
                // 加载组合遥信点号列表
                combineTelesignalisationList.Clear();
                if (telesignalisationListTemp != null && telesignalisationListTemp.Count > 0)
                {
                    foreach (var combineTelesignalisation in telesignalisationListTemp)
                    {
                        // 将组合遥信点号添加到组合遥信点表集合
                        combineTelesignalisationList.Add(combineTelesignalisation);
                        telesignalisationSourceList.Add(combineTelesignalisation);
                    }
                }
            }
        }
    }
}
