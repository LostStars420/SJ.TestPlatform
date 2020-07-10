using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// AddNewConfigureLeafNodeViewModel 的摘要说明
    /// author: liyan
    /// date：2018/5/29 20:59:10
    /// desc：用户配置DTU叶节点参数
    /// version: 1.0
    /// </summary>
    public class AddNewConfigureLeafNodeViewModel : ViewModelBase
    {
        /// <summary>
        /// 叶节点名称
        /// </summary>
        public static string nodeNameInput;

        /// <summary>
        /// 设置和获取叶节点名称
        /// </summary>
        public string NodeNameInput
        {
            get 
            { 
                return nodeNameInput; 
            }
            set 
            { 
                nodeNameInput = value;
                RaisePropertyChanged(() => NodeNameInput);
            }
        }

        /// <summary>
        /// 叶节点IP地址
        /// </summary>
        public static string nodeIP;

        /// <summary>
        /// 设置和获取叶节点IP地址
        /// </summary>
        public string NodeIP
        {
            get
            {
                return nodeIP;
            }
            set
            {
                nodeIP = value;
                RaisePropertyChanged(() => NodeIP);
            }
        }

        /// <summary>
        /// 配置断路器或者联络开关
        /// </summary>
        private ObservableCollection<string> _breakerOrTieSwitch;

        /// <summary>
        /// 设置和获取配置断路器或者联络开关
        /// </summary>
        public ObservableCollection<string> BreakerOrTieSwitch
        {
            get 
            { 
                return this._breakerOrTieSwitch; 
            }
            set 
            { 
                this._breakerOrTieSwitch = value;
                RaisePropertyChanged(() => BreakerOrTieSwitch);
            }
        }

        /// <summary>
        /// 配置断路器或者联络开关索引
        /// </summary>
        public static int selectedBreakerOrTieSwitch;

        /// <summary>
        /// 设置和获取配置断路器或者联络开关索引
        /// </summary>
        public int SelectedBreakerOrTieSwitch
        {
            get 
            { 
                return selectedBreakerOrTieSwitch; 
            }
            set 
            { 
                selectedBreakerOrTieSwitch = value;
                RaisePropertyChanged(() => SelectedBreakerOrTieSwitch);
            }
        }

        /// <summary>
        /// 配置主线或者支线
        /// </summary>
        private ObservableCollection<string> _mainOrBranch;

        /// <summary>
        /// 设置和获取配置主线或者支线
        /// </summary>
        public ObservableCollection<string> MainOrBranch
        {
            get 
            { 
                return this._mainOrBranch; 
            }
            set 
            { 
                this._mainOrBranch = value;
                RaisePropertyChanged(() => MainOrBranch);
            }
        }

        /// <summary>
        /// 配置主线或者支线索引
        /// </summary>
        public static int selectedMainOrBranch;

        /// <summary>
        /// 设置和获取配置主线或者支线索引
        /// </summary>
        public int SelectedMainOrBranch
        {
            get 
            { 
                return selectedMainOrBranch; 
            }
            set 
            { 
                selectedMainOrBranch = value;
                RaisePropertyChanged(() => SelectedMainOrBranch);
            }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        public AddNewConfigureLeafNodeViewModel()
        {
            nodeNameInput = null;
            nodeIP = null;
            this._breakerOrTieSwitch = new ObservableCollection<string>();
            this._breakerOrTieSwitch.Add("断路器");
            this._breakerOrTieSwitch.Add("联络");
            selectedBreakerOrTieSwitch = 0;

            this._mainOrBranch = new ObservableCollection<string>();
            this._mainOrBranch.Add("主线");
            this._mainOrBranch.Add("支线");
            selectedMainOrBranch = 0;
        }

        /// <summary>
        /// 检查用户输入的IP地址是否有效
        /// </summary>
        /// <param name="ip">用户输入的IP地址</param>
        public static void CheckIPAddrFormat(string ip)
        {
            if(ip != null)
            {
                ip = ip.Trim();

                int startIndex = 0;
                int dotNumber = 0;
                while(true)
                {
                    int IPindex = ip.IndexOf('.', startIndex);
                    if(IPindex != -1)
                    {
                        dotNumber++;
                        startIndex = IPindex + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if(dotNumber != 3)
                {
                    MessageBox.Show("输入的IP地址非法", "警告");
                    return;
                }
                else
                {
                    int firstDotIndex = ip.IndexOf('.');
                    int secondDotIndex = ip.IndexOf('.', firstDotIndex + 1);
                    int thirdDotIndex = ip.IndexOf('.', secondDotIndex + 1);
                    int ip0 = int.Parse(ip.Substring(0, firstDotIndex));
                    int ip1 = int.Parse(ip.Substring(firstDotIndex + 1, secondDotIndex - firstDotIndex - 1));
                    int ip2 = int.Parse(ip.Substring(secondDotIndex + 1, thirdDotIndex - secondDotIndex - 1));
                    int ip3 = int.Parse(ip.Substring(thirdDotIndex + 1));
                    if (ip0 >= 0 && ip0 <= 255 && ip1 >= 0 && ip1 <= 255 && ip2 >= 0 && ip2 <= 255 && ip3 >= 0 && ip3 <= 255)
                    {
                        // IP地址格式校验正确
                        nodeIP = ip;
                    }
                    else
                    {
                        MessageBox.Show("输入的IP地址非法", "警告");
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
}
