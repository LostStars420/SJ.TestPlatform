using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.Model
{
   public  class TelemeteringList : ObservableObject
    {
        private int id;
        public int ID
        {
            get { return this.id; }
            set
            {
                this.id = value;
                RaisePropertyChanged(() => ID);
            }
        }
        private float value;
        public float VALUE
        {
            get { return this.value; }
            set
            {
                this.value = value;
                RaisePropertyChanged(()=> VALUE);
            }
        }
    }
}
