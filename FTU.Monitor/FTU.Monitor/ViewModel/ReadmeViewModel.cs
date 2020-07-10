using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// ReadmeViewModel 的摘要说明
    /// author: liyan
    /// date：2018/7/24 15:48:11
    /// desc：“读我”功能的交互逻辑模块
    /// version: 1.0
    /// </summary>
    public class ReadmeViewModel : ViewModelBase
    {
        public ReadmeViewModel()
        {
            GetReadmeContent();
        }
        
        private string _readmeContent;
        public string ReadmeContent
        {
            get
            {
                return this._readmeContent;
            }
            set
            {
                this._readmeContent = value;
                RaisePropertyChanged(() => ReadmeContent);
            }
        }

        public void GetReadmeContent()
        {
            try
            {
                string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"Config\Readme\Readme.txt";
                if (!File.Exists(filePath))
                {
                    throw new Exception("“读我”文件不存在");
                }
                else
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        // 使用StreamReader类来读取文件
                        StreamReader streamReader = new StreamReader(fs);
                        // 从数据流中读取每一行，直到文件的最后一行
                        streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

                        string strLine = streamReader.ReadLine();
                        while (strLine != null)
                        {
                            ReadmeContent += strLine + "\n";
                            strLine = streamReader.ReadLine();
                        }

                        // 关闭此StreamReader对象
                        streamReader.Close();
                    }
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "读我");
            }
        }
    }
}
