using FTU.Monitor.WaitingDlgServer;
using System;
using System.Collections.Generic;
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
    public delegate void TaskEndNotify(Object result);

    /// <summary>
    /// WaitingDlg.xaml 的交互逻辑
    /// </summary>
    public partial class WaitingDlg
    {
        public Object TaskResult { get { return m_taskResult; } }

        private Object m_taskResult;
        private bool m_bCloseByMe;
        private readonly ILongTimeTask m_task;
        private readonly string m_strThePromptText;

        public WaitingDlg(ILongTimeTask task, string strThePromptText = null)
        {
            m_task = task;
            m_strThePromptText = strThePromptText;
            InitializeComponent();
        }

        private delegate void CloseMethod();

        public void TaskEnd(Object result)
        {
            m_taskResult = result;
            m_bCloseByMe = true;
            Dispatcher.BeginInvoke(new CloseMethod(Close));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!m_bCloseByMe) { e.Cancel = true; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(m_strThePromptText))
            {
                tbPrompt.Text = m_strThePromptText;
            }
            m_task.Start(this);
        }
    }
}
