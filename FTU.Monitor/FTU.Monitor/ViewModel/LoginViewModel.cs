using FTU.Monitor.Dao;
using FTU.Monitor.Model.ManageUsersModelCollectiopn;
using FTU.Monitor.Util;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// LoginViewModel 的摘要说明
    /// author: liyan
    /// date：2018/10/24 10:49:51
    /// desc：
    /// version: 1.0
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public LoginViewModel()
        {
            this._inputRoleNameList = new List<string>();
            this._inputRoleNameList.Add("超级管理员");
            this._inputRoleNameList.Add("研发工程师");
            this._inputRoleNameList.Add("测试工程师");
            this._inputRoleNameList.Add("质检工程师");
            this._inputRoleNameList.Add("售后工程师");

            this._inputRoleNameIndex = 2;

            ClickLoginBtn = new RelayCommand<string>(ExcuteLoginCommand);

        }

        /// <summary>
        /// 输入的用户名
        /// </summary>
        private string _inputUserName;

        /// <summary>
        /// 设置和获取输入的用户名
        /// </summary>
        public string InputUserName
        {
            get
            {
                return this._inputUserName;
            }
            set
            {
                this._inputUserName = value;
            }
        }

        /// <summary>
        /// 输入的密码
        /// </summary>
        private string _inputPassword;

        /// <summary>
        /// 设置和获取的输入密码
        /// </summary>
        public string InputPassword
        {
            get
            {
                return this._inputPassword;
            }
            set
            {
                this._inputPassword = value;
            }
        }

        /// <summary>
        /// 输入的角色名称
        /// </summary>
        private IList<string> _inputRoleNameList;

        /// <summary>
        /// 设置和获取输入的角色名称
        /// </summary>
        public IList<string> InputRoleNameList
        {
            get
            {
                return this._inputRoleNameList;
            }
            set
            {
                this._inputRoleNameList = value;
            }
        }

        /// <summary>
        /// 输入的角色名称索引
        /// </summary>
        private int _inputRoleNameIndex;

        /// <summary>
        /// 设置和获取输入的角色名称索引
        /// </summary>
        public int InputRoleNameIndex
        {
            get
            {
                return this._inputRoleNameIndex;
            }
            set
            {
                this._inputRoleNameIndex = value;
            }
        }

        /// <summary>
        /// 用户登陆时选择的角色名称
        /// </summary>
        private string _inputRoleName;

        /// <summary>
        /// 设置和获取用户登陆时选择的角色名称
        /// </summary>
        public string InputRoleName
        {
            get
            {
                return this._inputRoleName;
            }
            set
            {
                this._inputRoleName = value;
            }
        }

        /// <summary>
        /// 登陆界面命令
        /// </summary>
        public RelayCommand<string> ClickLoginBtn
        {
            get;
            private set;
        }

        /// <summary>
        /// 执行用户登陆命令界面
        /// </summary>
        /// <param name="arg"></param>
        private void ExcuteLoginCommand(string arg)
        {
            switch (arg)
            {
                case "Login":

                    ExcuteLogin();
                    break;
            }

        }

        /// <summary>
        /// 用户点击“登陆”后的逻辑操作
        /// </summary>
        private void ExcuteLogin()
        {
            try
            {
                // 查询数据库中所有用户的登陆信息
                IList<UserRoleMapModel> allUserInfo = new UserRoleMapDao().QueryAllUser();
                if (allUserInfo == null)
                {
                    throw new Exception("数据库中的用户记录为空");
                }
                else
                {
                    if (InputUserName == null)
                    {
                        MessageBox.Show("用户名不能为空", "提示");
                        return;
                    }
                    else if (InputPassword == null)
                    {
                        MessageBox.Show("请输入登陆密码", "提示");
                        return;
                    }
                    else
                    {
                        // 获取用户输入的信息
                        InputRoleName = InputRoleNameList[InputRoleNameIndex];
                        foreach (var item in allUserInfo)
                        {
                            if (InputUserName == item.UserName && InputRoleName == item.RoleName && InputPassword == item.Password)
                            {
                                // 信息匹配，登陆成功。启动主界面，关闭登陆界面
                                Messenger.Default.Send<string>(item.UserName, "SetUseName");
                                Messenger.Default.Send<string>(item.RoleName, "SetUserRole");
                                Messenger.Default.Send<object>(null, "CloseLoginWindow");
                                LogHelper.Info(typeof(LoginViewModel), "当前用户:" + item.UserName + "   角色：" + item.RoleName);
                                return;
                            }
                        }
                        MessageBox.Show("登陆账户不匹配，请重新输入登陆信息", "提示");
                        return;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("登陆时逻辑出现错误" + ex.ToString(), "警告");
            }

        }
    }
}
