using FTU.Monitor.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FTU.Monitor.View
{
    /// <summary>
    /// OperationManualView.xaml 的交互逻辑
    /// </summary>
    public partial class OperationManualView : Window
    {
        /// <summary>
        /// 是否加载文档成功
        /// </summary>
        private bool _isLoaded;

        /// <summary>
        /// 设置和获取是否加载文档成功
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                return this._isLoaded;
            }

            set
            {
                this._isLoaded = value;
            }
        }
        public OperationManualView()
        {
            InitializeComponent();
            this._isLoaded = false;
        }

        /// <summary>
        /// 关闭操作手册窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationManualFormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainViewModel.ShowOperatiopnManualEnable = true;
        }

        /// <summary>
        /// 打开操作手册窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPDFFile(object sender, RoutedEventArgs e)
        {
            LoadPDFFile();
        }

        /// <summary>
        /// 加载操作手册文档
        /// </summary>
        private void LoadPDFFile()
        {
            try
            {
                string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"Config\OperationManual\OperationManual.pdf";
                if (!File.Exists(filePath))
                {
                    throw new Exception("“操作手册”文件不存在");
                }
                else
                {
                    moonPdfPanel.OpenFile(filePath);
                    IsLoaded = true;
                }
            }
            catch(Exception ex)
            {
                IsLoaded = false;
                MessageBox.Show(ex.Message, "操作手册");
            }

        }

        /// <summary>
        /// 放大查看文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                moonPdfPanel.ZoomIn();
            }
        }

        /// <summary>
        /// 缩小查看文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                moonPdfPanel.ZoomOut();
            }
        }

        /// <summary>
        /// 100%模式查看文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NormalButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                moonPdfPanel.Zoom(1.0);
            }
        }

        /// <summary>
        /// 整页查看文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FitToHeightButton_Click(object sender, RoutedEventArgs e)
        {
            moonPdfPanel.ZoomToHeight();
        }

        /// <summary>
        /// 双页查看文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FacingButton_Click(object sender, RoutedEventArgs e)
        {
            moonPdfPanel.ViewType = MoonPdfLib.ViewType.Facing;
        }

        /// <summary>
        /// 单页查看文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SinglePageButton_Click(object sender, RoutedEventArgs e)
        {
            moonPdfPanel.ViewType = MoonPdfLib.ViewType.SinglePage;
        }

    }
}
