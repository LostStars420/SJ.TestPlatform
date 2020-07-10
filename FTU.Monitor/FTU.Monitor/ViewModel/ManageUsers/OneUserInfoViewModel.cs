using FTU.Monitor.Dao;
using FTU.Monitor.Model.ManageUsersModelCollectiopn;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FTU.Monitor.ViewModel.ManageUsers
{
    /// <summary>
    /// OneUserInfoViewModel 的摘要说明
    /// author: liyan
    /// date：2018/10/17 9:47:07
    /// desc：新增或者修改用户界面的ViewModel
    /// version: 1.0
    /// </summary>
    public class OneUserInfoViewModel : ViewModelBase
    {
        public OneUserInfoViewModel()
        {

            this._userRoleMapDao = new Dao.UserRoleMapDao();
            // 命令初始化
            OneUserInfoCommand = new RelayCommand<string>(ExcuteOneUserInfoCommand);

            OneUserInfo = new UserRoleMapModel();
            if (OneUserInfoTitle == "修改用户")
            {
                if (SuperAdministratorViewModel._userGridIndex != -1)
                {
                    OneUserInfo = SuperAdministratorViewModel._allUserInformation[SuperAdministratorViewModel._userGridIndex];
                    OneUserInfo.SetRoleNameIndex(OneUserInfo.RoleName);
                    BeforeUpdateUserInfoID = Convert.ToInt32(UserRoleMapDao.QueryByUserName(OneUserInfo));
                    int beforeUpdateRoleInfoID = Convert.ToInt32(UserRoleMapDao.QueryByRoleName(OneUserInfo));
                    BeforeUpdateUserRoleMapID = Convert.ToInt32(UserRoleMapDao.QueryByRoleIDUserID(BeforeUpdateUserInfoID, beforeUpdateRoleInfoID));
                }
            }
        }

        /// <summary>
        /// 新用户标题
        /// </summary>
        public static string _oneUserInfoTitle;

        /// <summary>
        /// 设置和获取新用户标题
        /// </summary>
        public string OneUserInfoTitle
        {
            get
            {
                return _oneUserInfoTitle;
            }
            set
            {
                _oneUserInfoTitle = value;
                this.RaisePropertyChanged(() => OneUserInfoTitle);
            }
        }

        /// <summary>
        /// 新增或修改用户的信息
        /// </summary>
        private static UserRoleMapModel _oneUserInfo;

        /// <summary>
        /// 设置和获取新增或修改用户的信息
        /// </summary>
        public UserRoleMapModel OneUserInfo
        {
            get
            {
                return _oneUserInfo;
            }
            set
            {
                _oneUserInfo = value;
                this.RaisePropertyChanged(() => OneUserInfo);
            }
        }

        /// <summary>
        /// 被修改用户信息在user_information中的关键字id
        /// </summary>
        private int _beforeUpdateUserInfoID;

        /// <summary>
        /// 设置和获取被修改用户信息在user_information中的关键字id
        /// </summary>
        public int BeforeUpdateUserInfoID
        {
            get
            {
                return this._beforeUpdateUserInfoID;
            }
            set
            {
                this._beforeUpdateUserInfoID = value;
            }
        }

        private int _beforeUpdateUserRoleMapID;

        public int BeforeUpdateUserRoleMapID
        {
            get
            {
                return this._beforeUpdateUserRoleMapID;
            }
            set
            {
                this._beforeUpdateUserRoleMapID = value;
            }
        }

        /// <summary>
        /// 访问数据库对象
        /// </summary>
        private UserRoleMapDao _userRoleMapDao;

        /// <summary>
        /// 设置和获取访问数据库对象
        /// </summary>

        public UserRoleMapDao UserRoleMapDao
        {
            get
            {
                return this._userRoleMapDao;
            }
            set
            {
                this._userRoleMapDao = value;
            }
        }

        /// <summary>
        /// 新增或修改用户的命令
        /// </summary>
        public RelayCommand<string> OneUserInfoCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 新增或修改用户所执行的逻辑
        /// </summary>
        /// <param name="arg"></param>
        private void ExcuteOneUserInfoCommand(string arg)
        {
            switch (arg)
            {
                case "Done":

                    AddOrUpdateUserRoleInfo();

                    break;

                case "Cancel":

                    // 关闭窗口
                    Messenger.Default.Send<string>("close", "CloseWindowCmd");
                    break;
            }

        }

        /// <summary>
        /// 数据库中是否存在用户名与角色名一致的记录
        /// </summary>
        /// <returns>是否存在记录</returns>
        private bool IsExitByUserRole()
        {
            IList<UserRoleMapModel> allUserList = UserRoleMapDao.QueryAllUser();
            if (allUserList != null)
            {
                foreach (var item in allUserList)
                {
                    if (item.UserName == OneUserInfo.UserName && item.RoleName == OneUserInfo.RoleName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 用户基本信息相同
        /// </summary>
        /// <returns>user_information中是否存在相同记录</returns>
        private bool IsExitSameUser()
        {
            IList<UserRoleMapModel> allUserList = UserRoleMapDao.QueryAllUser();
            if (allUserList != null)
            {
                foreach (var item in allUserList)
                {
                    if (item.UserName == OneUserInfo.UserName && item.Password == OneUserInfo.Password && item.TelephoneNumber == OneUserInfo.TelephoneNumber && item.Remark == OneUserInfo.Remark)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 用户确定添加或者修改用户角色信息
        /// </summary>
        private void AddOrUpdateUserRoleInfo()
        {
            OneUserInfo.SetRoleName();
            if (OneUserInfo.UserName == null || OneUserInfo.RoleName == null || OneUserInfo.TelephoneNumber == null || OneUserInfo.Remark == null)
            {
                MessageBox.Show("请输入完整的用户信息！", "警告");
                return;
            }

            if (OneUserInfoTitle == "新增用户")
            {
                // 查询数据库中是否存在即将添加的用户名与角色
                bool isExitRecord = IsExitByUserRole();
                if (isExitRecord == false)
                {
                    // 查询user_information表中是否存在信息完全匹配的用户信息
                    bool isExitSameUser = IsExitSameUser();
                    if (isExitSameUser == false)
                    {
                        // 插入用户信息到user_information记录中,并返回插入所在的id
                        int result = UserRoleMapDao.InsertOneUserInfo(OneUserInfo);
                        if (result <= 0)
                        {

                        }
                        // 返回用户名所在的id
                        int userInfoID = Convert.ToInt32(UserRoleMapDao.QueryByUserName(OneUserInfo));
                        // 返回角色名称所在的id
                        int roleInfoID = Convert.ToInt32(UserRoleMapDao.QueryByRoleName(OneUserInfo));
                        // 插入信息到user_role_map表中
                        UserRoleMapDao.InsertToUserRoleMap(userInfoID, roleInfoID);
                    }
                    else
                    {
                        // 插入信息到user_role_map表中
                        //返回用户名称所在的id
                        int userInfoID = Convert.ToInt32(UserRoleMapDao.QueryByUserName(OneUserInfo));
                        // 返回角色名称所在的id
                        int roleInfoID = Convert.ToInt32(UserRoleMapDao.QueryByRoleName(OneUserInfo));
                        // 插入信息到user_role_map表中
                        UserRoleMapDao.InsertToUserRoleMap(userInfoID, roleInfoID);
                    }

                }
                else
                {
                    // 提示已存在该用户，不允许重复添加
                    MessageBox.Show("该用户信息已存在，请勿重复添加！", "提示");

                }
            }
            else if (OneUserInfoTitle == "修改用户")
            {
                // 更新用户信息
                UserRoleMapDao.UpdateOneUserInfoToUserInfo(OneUserInfo, BeforeUpdateUserInfoID);
                // 返回角色名称所在的id
                int roleInfoID = Convert.ToInt32(UserRoleMapDao.QueryByRoleName(OneUserInfo));
                UserRoleMapDao.UpdateOneUserInfoToUserRoleMap(roleInfoID, BeforeUpdateUserRoleMapID);
            }

            // 关闭窗口
            Messenger.Default.Send<string>("close", "CloseWindowCmd");
        }

    }
}
