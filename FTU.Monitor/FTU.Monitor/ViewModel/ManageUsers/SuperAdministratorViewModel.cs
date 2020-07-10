using FTU.Monitor.Dao;
using FTU.Monitor.Model.ManageUsersModelCollectiopn;
using FTU.Monitor.View.ManageUsers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FTU.Monitor.ViewModel.ManageUsers
{
    /// <summary>
    /// SuperAdministratorViewModel 的摘要说明
    /// author: liyan
    /// date：2018/10/15 8:57:50
    /// desc：
    /// version: 1.0
    /// </summary>
    public class SuperAdministratorViewModel : ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SuperAdministratorViewModel()
        {
            _allUserInformation = new ObservableCollection<UserRoleMapModel>();
            SuperAdministratorCmd = new RelayCommand<string>(ExcuteSuperAdministratorCommand);
            LoadUser();
        }

        /// <summary>
        /// 新用户代理标题
        /// </summary>
        public static string _oneUserInfoTitle;

        /// <summary>
        /// 新用户代理标题 Get、Set方法
        /// </summary>
        public string OneUserInfoTitle
        {
            get { return _oneUserInfoTitle; }
            set
            {
                _oneUserInfoTitle = value;
                this.RaisePropertyChanged("NewUserTitle");
            }
        }

        /// <summary>
        /// 用户表格索引
        /// </summary>
        public static int _userGridIndex;

        /// <summary>
        /// 用户表格索引 Get、Set方法
        /// </summary>
        public int UserGridIndex
        {
            get { return _userGridIndex; }
            set
            {
                _userGridIndex = value;
                this.RaisePropertyChanged("UserGridIndex");
            }
        }

        /// <summary>
        /// 所有用户信息集合，与界面显示的表格一一对应
        /// </summary>
        public static ObservableCollection<UserRoleMapModel> _allUserInformation;

        /// <summary>
        /// 设置和获取所有用户信息集合，与界面显示的表格一一对应
        /// </summary>
        public ObservableCollection<UserRoleMapModel> AllUserInformation
        {
            get
            {
                return _allUserInformation;
            }
            set
            {
                _allUserInformation = value;
            }
        }

        /// <summary>
        /// 动作指令
        /// </summary>
        public RelayCommand<string> SuperAdministratorCmd
        {
            get;
            private set;
        }

        /// <summary>
        /// 执行用户操作的按钮动作
        /// </summary>
        /// <param name="arg">各按钮动作</param>
        private void ExcuteSuperAdministratorCommand(string arg)
        {
            if (arg.Equals("ChangeUserInfo") && _userGridIndex == -1)
            {
                MessageBox.Show("请先选择需要修改的用户");
                return;
            }

            switch (arg)
            {
                // 添加新用户
                case "AddNewUserInfo":
                    // 添加用户界面信息均为空
                    OneUserInfoViewModel._oneUserInfoTitle = "新增用户";
                    new OneUserInfoWindow().ShowDialog();
                    LoadUser();
                    break;

                // 修改现有用户信息
                case "ChangeUserInfo":
                    // 将当前选中需要修改的用户信息加载到修改界面上
                    OneUserInfoViewModel._oneUserInfoTitle = "修改用户";
                    new OneUserInfoWindow().ShowDialog();
                    LoadUser();
                    break;

                // 删除用户
                case "DeleteUserInfo":
                    DeleteOneUserInfo();
                    LoadUser();
                    break;
            }
        }

        /// <summary>
        /// 删除当前选中的记录
        /// </summary>
        private void DeleteOneUserInfo()
        {
            UserRoleMapDao userRoleMapDao = new UserRoleMapDao();
            // 获取当前选中行的信息
            UserRoleMapModel selectedUser = AllUserInformation[UserGridIndex];
            // 查询数据库中的userID与roleID
            int userID = Convert.ToInt32(userRoleMapDao.QueryByUserName(selectedUser));
            int roleID = Convert.ToInt32(userRoleMapDao.QueryByRoleName(selectedUser));

            // 删除user_role_map表中对应的记录
            userRoleMapDao.DeleteFromUserRoleMap(userID, roleID);
            int result = Convert.ToInt32(userRoleMapDao.QueryByUserID(userID));
            if (result == -1)
            {
                // 删除user_information中该条信息记录
                userRoleMapDao.DeleteFromUserInfomation(userID);
            }
        }

        /// <summary>
        /// 加载用户信息
        /// </summary>
        private void LoadUser()
        {
            UserRoleMapDao userRoleMapDao = new UserRoleMapDao();
            IList<UserRoleMapModel> allUserInfo = userRoleMapDao.QueryAllUser();
            AllUserInformation.Clear();
            for (int i = 0; i < allUserInfo.Count; i++)
            {
                allUserInfo[i].SerialNumber = i + 1;
                AllUserInformation.Add(allUserInfo[i]);
            }
        }
    }
}
