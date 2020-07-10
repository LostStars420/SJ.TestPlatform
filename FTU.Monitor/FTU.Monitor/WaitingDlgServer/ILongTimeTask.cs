using FTU.Monitor.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.WaitingDlgServer
{
    public interface ILongTimeTask
    {
        void Start(WaitingDlg dlg);
    }
}
