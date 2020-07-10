using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.Model.ManageUsersModelCollectiopn
{
    /// <summary>
    /// UserRoleMapModel 的摘要说明
    /// author: liyan
    /// date：2018/10/12 17:02:23
    /// desc：用户与角色总信息的对象模型，与超级用户管理界面SuperAdministratorView中的表格一一对应
    /// version: 1.0
    /// </summary>
    public class UserRoleMapModel : ObservableObject
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserRoleMapModel()
        {
            RoleName = "研发工程师";
            RoleNameList = new List<string>();
            RoleNameList.Add("超级管理员");
            RoleNameList.Add("研发工程师");
            RoleNameList.Add("测试工程师");
            RoleNameList.Add("质检工程师");
            RoleNameList.Add("售后工程师");
            RoleNameIndex = 1;
        }

        /// <summary>
        /// 序号
        /// </summary>
        private int _serialNumber;

        /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber
        {
            get
            {
                return this._serialNumber;
            }
            set
            {
                this._serialNumber = value;
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        private string _userName;

        /// <summary>
        /// 设置和获取用户名
        /// </summary>
        public string UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
                this._userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        /// <summary>
        /// 角色
        /// </summary>
        private string _roleName;

        /// <summary>
        /// 设置和获取角色
        /// </summary>
        public string RoleName
        {
            get
            {
                return this._roleName;
            }
            set
            {
                this._roleName = value;
            }
        }

        /// <summary>
        /// 角色名称集合
        /// </summary>
        private List<string> _roleNameList;

        /// <summary>
        /// 设置和获取角色名称集合
        /// </summary>
        public List<string> RoleNameList
        {
            get
            {
                return this._roleNameList;
            }
            set
            {
                this._roleNameList = value;
            }
        }

        /// <summary>
        /// 角色名称索引
        /// </summary>
        private int _roleNameIndex;

        /// <summary>
        /// 设置和获取角色名称索引
        /// </summary>
        public int RoleNameIndex
        {
            get
            {
                return this._roleNameIndex;
            }
            set
            {
                this._roleNameIndex = value;
            }
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        private string _password;

        /// <summary>
        /// 设置和获取用户密码
        /// </summary>
        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        /// <summary>
        /// 电话号码
        /// </summary>
        private string _telephoneNumber;

        /// <summary>
        /// 设置和获取电话号码
        /// </summary>
        public string TelephoneNumber
        {
            get
            {
                return this._telephoneNumber;
            }
            set
            {
                this._telephoneNumber = value;
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        private string _remark;

        /// <summary>
        /// 设置和获取备注
        /// </summary>
        public string Remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._remark = value;
            }
        }

        /// <summary>
        /// 设置用户角色的索引
        /// </summary>
        public void SetRoleNameIndex(string roleName)
        {
            for (int i = 0; i < RoleNameList.Count; i++)
            {
                if (roleName == RoleNameList[i])
                {
                    RoleNameIndex = i;
                }
            }
        }

        /// <summary>
        /// 设置用户角色名
        /// </summary>
        public void SetRoleName()
        {
            RoleName = RoleNameList[RoleNameIndex];
        }
    }
}
