
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FTU.Monitor.Model
{
  public  class TelesignalisationList: ObservableObject
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
        private bool value;
        public bool VALUE
        {
            get { return this.value; }
            set
            {
                this.value = value;
                RaisePropertyChanged(() => VALUE);
            }
        }
    }
}