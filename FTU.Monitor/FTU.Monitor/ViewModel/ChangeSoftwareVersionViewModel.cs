using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// ChangeSoftwareVersionViewModel 的摘要说明
    /// author: liyan
    /// date：2018/4/8 13:17:40
    /// desc：修改软件版本号ViewModel
    /// version: 1.0
    /// </summary>
    class ChangeSoftwareVersionViewModel: ViewModelBase
    {
        /// <summary>
        /// 用户第一次输入的新版本号
        /// </summary>
        public string _firstNewVersion;

        /// <summary>
        /// 设置和获取用户第一次输入的新版本号
        /// </summary>
        public string FirstNewVersion
        {
            get
            {
                return this._firstNewVersion;
            }
            set
            {
                this._firstNewVersion = value;
                RaisePropertyChanged(() => FirstNewVersion);
            }
        }

        /// <summary>
        /// 用户第二次输入的新版本号
        /// </summary>
        public string _secondNewVersion;

        /// <summary>
        /// 设置和获取用户第二次输入的新版本号
        /// </summary>
        public string SecondNewVersion
        {
            get
            {
                return this._secondNewVersion;
            }
            set
            {
                this._secondNewVersion = value;
                RaisePropertyChanged(() => SecondNewVersion);
            }
        }

        /// <summary>
        /// 修改终端程序版本指令
        /// </summary>
        public RelayCommand<string> ChangeSoftwareVersionCommand 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// 修改终端软件版本界面用户操作对应的响应
        /// </summary>
        /// <param name="arg">不同的用户操作</param>
        public void ExecuteChangeSoftwareVersionCommand(string arg)
        {
            switch(arg)
            {
                // 用户点击“确定”
                case "Confirm":
                    if(FirstNewVersion.Equals(SecondNewVersion))
                    {
                        ProgramVersionDao programVersionDao = new ProgramVersionDao();
                        // 更新数据库中的终端软件程序版本
                        int isUpdateSuccess = programVersionDao.UpdateProgramVersion(FirstNewVersion, InherentParameterViewModel.programVersionId);
                        if(isUpdateSuccess > 0)
                        {
                            Messenger.Default.Send<string>(FirstNewVersion, "updateSoftwareVersion");              
                            MessageBox.Show("终端软件版本更新成功！");
                            FirstNewVersion = "";
                            SecondNewVersion = "";
                        }
                        else
                        {
                            MessageBox.Show("终端软件版本更新失败！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("两次输入的终端软件版本不一致","提示");
                        return;
                    }
                    break;

                // 用户点击“取消”
                case "Cancel":
                    FirstNewVersion = "";
                    SecondNewVersion = "";
                    break;

            }

        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public ChangeSoftwareVersionViewModel()
        {
            ChangeSoftwareVersionCommand = new RelayCommand<string>(ExecuteChangeSoftwareVersionCommand);
        }
    }
}
