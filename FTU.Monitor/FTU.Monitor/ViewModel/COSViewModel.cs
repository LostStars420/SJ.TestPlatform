using FTU.Monitor.Model;
using FTU.Monitor.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// COSViewModel 的摘要说明
    /// author: liyan
    /// date：2018/12/21 13:30:36
    /// desc：
    /// version: 1.0
    /// </summary>
    public class COSViewModel : ViewModelBase
    {
        public COSViewModel()
        {
            _COSData = new ObservableCollection<COS>();
            COSCommand = new RelayCommand<string>(ExcuteCOSCommand);
        }

        /// <summary>
        /// COS数据集合
        /// </summary>
        public static ObservableCollection<COS> _COSData;

        /// <summary>
        /// 设置和获取COS数据集合
        /// </summary>
        public ObservableCollection<COS> COSData
        {
            get
            {
                return _COSData;
            }
            set
            {
                _COSData = value;
                RaisePropertyChanged(() => COSData);
            }
        }

        /// <summary>
        /// COS相关命令
        /// </summary>
        public RelayCommand<string> COSCommand
        {
            get;
            private set;
        }

        private void ExcuteCOSCommand(string arg)
        {
            switch(arg)
            {
                // "清除COS记录"
                case "Clear":

                    TelesignalisationViewModel.cosCounter = 0;
                    COSData.Clear();
                    break;

                case "ImportData":

                    // 从用户打开的文件中获取json字符串
                    string jsonStr = GetJosnString();
                    if(jsonStr != null)
                    {
                        try
                        {
                            List<COS> cos = EncryptAndDecodeUtil.JsonToObject<List<COS>>(jsonStr);
                            COSData.Clear();
                            COSData = new ObservableCollection<COS>(cos);
                        }
                        catch
                        {
                            MessageBox.Show("COS文件非法，请检查文件是否损坏！","警告");
                        }
                    }
                    else
                    {
                        MessageBox.Show("打开的文件中无COS记录，请选择正确的COS文件","提示");
                    }

                    break;

                case "ExportData":
                    if(COSData != null && COSData.Count > 0)
                    {
                        string ObjToJson = EncryptAndDecodeUtil.GetJson(COSData);
                        BuildJsonToLocal(ObjToJson);
                    }
                    else
                    {
                        MessageBox.Show("COS数据为空！请产生COS记录后再执行导出操作","提示");
                    }


                    break;
            }
        }

        /// <summary>
        /// 生成Json文件到本地
        /// </summary>
        /// <param name="allDownPointTable">Json文件内容</param>
        /// <returns></returns>
        private string BuildJsonToLocal(string allDownPointTable)
        {
            try
            {
                //string primaryPath = @".\Config";
                string primaryName = "COS.json";
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "AllPointTable (*.json)|*.json";

                // 是否自动添加扩展名
                saveFileDialog.AddExtension = true;
                // 文件已存在是否提示覆盖
                saveFileDialog.OverwritePrompt = true;
                // 提示输入的文件名无效
                saveFileDialog.CheckPathExists = true;
                // 文件初始名
                saveFileDialog.FileName = primaryName;

                if (saveFileDialog.ShowDialog() == true)
                {
                    System.IO.File.WriteAllText(saveFileDialog.FileName, allDownPointTable, System.Text.Encoding.GetEncoding("GB2312"));

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

        /// <summary>
        /// 获取用户打开文件内容字符串
        /// </summary>
        /// <returns>Json格式的明文</returns>
        public string GetJosnString()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                // 获取或设置筛选器字符串
                openFileDialog.Filter = "config files (*.cfg)|*.cfg|data files (*.txt)|*.txt|All files (*.*)|*.*";

                if ((bool)(openFileDialog.ShowDialog()))
                {
                    // 打开一个文件，使用指定的编码读取文件的所有行，然后关闭该文件
                    string str = System.IO.File.ReadAllText(openFileDialog.FileName, System.Text.Encoding.GetEncoding("GB2312"));
                    return str;
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
