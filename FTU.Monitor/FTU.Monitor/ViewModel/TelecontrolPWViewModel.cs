using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Xml;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// TelecontrolPWViewModel 的摘要说明
    /// author: zhengshuiqing
    /// date：2017/10/10 9:12:29
    /// desc：遥控操作密码确认ViewModel
    /// version: 1.0
    /// </summary>
    class TelecontrolPWViewModel : ViewModelBase
    {
        /// <summary>
        /// 文本框输入的密码
        /// </summary>
        public static string pwBox;

        /// <summary>
        /// 设置和获取文本框输入的密码
        /// </summary>
        public string PWBox
        {
            get
            {
                return pwBox;
            }
            set
            {
                pwBox = value;
                RaisePropertyChanged(() => PWBox);
            }
        }

        /// <summary>
        /// 读取配置文件XML的对象
        /// </summary>
        private XmlDocument document;

        public XmlDocument PWdocument;

        /// <summary>
        /// 密码操作相关命令
        /// </summary>
        public RelayCommand<string> PWCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 密码操作相关命令执行方法
        /// </summary>
        /// <param name="arg">参数</param>
        public void ExecutePWCommand(string arg)
        {
            switch (arg)
            {
                case "Confirm":
                    TelecontrolViewModel.isPW = true;
                    ConfigViewModel.isPW = true;
                    break;

                case "Cancel":
                    TelecontrolViewModel.isPW= false;
                    ConfigViewModel.isPW = false;
                    break;

            }

        }

        /// <summary>
        /// 读取XML配置文件中的遥控口令
        /// </summary>
        public void ReadPassword(string tagName)
        {
            PWdocument = new XmlDocument();

            string ExePath = System.AppDomain.CurrentDomain.BaseDirectory + @"Config\XML\Password.xml";
            PWdocument.Load(ExePath);

            XmlNodeList listPW = PWdocument.GetElementsByTagName(tagName);

            if (tagName == "TelecontrolPW")
            {
                for (int i = 0; i < listPW.Count; i++)
                {
                    TelecontrolViewModel.telecontrolPW = listPW[i].InnerText;
                }
            }
            else if (tagName == "ConfigPW")
            {
                for (int i = 0; i < listPW.Count; i++)
                {
                    ConfigViewModel.configPW = listPW[i].InnerText;
                }
            }
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public TelecontrolPWViewModel()
        {
            PWCommand = new RelayCommand<string>(ExecutePWCommand);
            // 加载Password XML
            ReadPassword("TelecontrolPW");
            ReadPassword("ConfigPW");
            document = new XmlDocument();
        }

    }
}
