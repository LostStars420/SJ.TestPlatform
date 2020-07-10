using FTU.Monitor.Comtrade;
using FTU.Monitor.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using OxyPlot;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// ComtradeViewModel 的摘要说明
    /// author: songminghao
    /// date：2017/12/08 8:35:09
    /// desc：录波文件处理类
    /// version: 1.0
    /// </summary>
    public class ComtradeViewModel : ViewModelBase
    {
        /// <summary>
        /// 录波配置文件数据
        /// </summary>
        private string _cfgData;

        /// <summary>
        /// 设置和获取录波配置文件数据
        /// </summary>
        public string CfgData
        {
            get
            {
                return this._cfgData;
            }
            set
            {
                this._cfgData = value;
                RaisePropertyChanged(() => CfgData);
            }
        }

        /// <summary>
        /// 显示的波形列表
        /// </summary>
        private ObservableCollection<PlotShowModel> _plotViewCollect;

        /// <summary>
        /// 设置和获取显示的波形列表
        /// </summary>
        public ObservableCollection<PlotShowModel> PlotViewCollect
        {
            get
            {
                return this._plotViewCollect;
            } 
            set
            {
                this._plotViewCollect = value;
                RaisePropertyChanged(() => PlotViewCollect);
            }
        }

        /// <summary>
        /// 读录波文件数据对象
        /// </summary>
        private RecordReader _record;

        /// <summary>
        /// 设置和获取读录波文件数据对象
        /// </summary>
        public RecordReader Record
        {
            get 
            { 
                return this._record; 
            }
            set 
            { 
                this._record = value;
                RaisePropertyChanged(() => Record);
            }
        }

        

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public ComtradeViewModel()
        {
            ComtradeCommand = new RelayCommand(ExecuteComtradeCommand);

            this._plotViewCollect = new ObservableCollection<PlotShowModel>();
        }

        /// <summary>
        /// 打开录波文件指令（录波配置文件）
        /// </summary>
        public RelayCommand ComtradeCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 打开录波文件指令（录波配置文件）执行操作
        /// </summary>
        void ExecuteComtradeCommand()
        {
            try
            {
                // 创建一个通用对话框对象，用户可以使用此对话框来指定一个或多个要打开的文件的文件名
                OpenFileDialog openFileDialog = new OpenFileDialog();

                // 获取或设置筛选器字符串
                // (*.jpg,*.png,*.jpeg,*.bmp,*.gif)|*.jgp;*.png;*.jpeg;*.bmp;*.gif|All files(*.*)|*.*
                // ‘|’分割的两个，一个是注释，一个是真的Filter，显示出来的是那个注释。如果要一次显示多中类型的文件，用分号分开
                openFileDialog.Filter = "config files (*.cfg)|*.cfg|data files (*.dat)|*.dat|All files (*.*)|*.*";

                //获取项目启动路径
                string startPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                //设置默认的打开路径
                openFileDialog.InitialDirectory = startPath + @"\Comtrade\";

                if ((bool)(openFileDialog.ShowDialog()))
                {
                    // 初始化读录波文件数据对象
                    this._record = new RecordReader(openFileDialog.FileName);

                    // 设置录波配置文件信息
                    CfgData = this._record.Configuration.ToCFGString();

                    // 获取采样数据总数
                    int len = this._record.data.samplesCount;

                    // 清空显示的波形列表
                    PlotViewCollect.Clear();

                    for (int i = 0; i < this._record.Configuration.analogChannelsCount; i++)
                    {
                        PlotModel plotModel = new PlotModel();
                        var series = new OxyPlot.Series.LineSeries { Title = this._record.Configuration.AnalogChannelInformations[i].name, MarkerType = MarkerType.Cross };

                        for (int j = 0; j < len; j++)
                        {
                            series.Points.Add(new DataPoint(j, (int)this._record.data.samples[j].analogs[i]));
                        }

                        plotModel.Series.Add(series);

                        PlotShowModel plotShowModel = new PlotShowModel();
                        plotShowModel.Name = this._record.Configuration.AnalogChannelInformations[i].name;
                        plotShowModel.PlotModelShow = plotModel;
                        PlotViewCollect.Add(plotShowModel);
                    }

                    Console.WriteLine(openFileDialog.SafeFileName);
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

    }
}

