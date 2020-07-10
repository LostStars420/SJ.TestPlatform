/*
 *  ConnectionParameters.cs
 */

using FTU.Monitor.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;

namespace lib60870
{
    public class ConnectionParameters : ViewModelBase
    {

        private static int k = 12; /* number of unconfirmed APDUs in I format
		                (range: 1 .. 32767 (2^15 - 1) - sender will
		                stop transmission after k unconfirmed I messages */

        private static int w = 8; /* number of unconfirmed APDUs in I format 
						      (range: 1 .. 32767 (2^15 - 1) - receiver
						      will confirm latest after w messages */

        private static int t0 = 10; /* connection establishment (in s) */

        private static int t1 = 15; /* timeout for transmitted APDUs in I/U format (in s)
		                   when timeout elapsed without confirmation the connection
		                   will be closed */

        private static int t2 = 10; /* timeout to confirm messages (in s) */

        private static int t3 = 20; /* time until test telegrams in case of idle connection */

        private static int sizeOfTypeId = 1;

        private static int sizeOfVSQ = 1; /* VSQ = variable sturcture qualifier */

        public static int sizeOfCOT = 2; /* (parameter b) COT = cause of transmission (1/2) */

        private static int originatorAddress = 0;

        private static int sizeOfCA = 2; /* (parameter a) CA = common address of ASDUs (1/2) */

        private static int sizeOfIOA = 3; /* (parameter c) IOA = information object address (1/2/3) */

        private static int restranCount = 3;//重传次数

        private static int restranTime = 1000;//重传时间

        private static int pollTime = 5000;//定时召唤数据时间

        public RelayCommand ClosedCommand { get; private set; }
        void ExecuteClosedCommand()
        {
            //允许再次弹出窗体
            MainViewModel.ShowProtocolParameterEnable = true;
        }
        public ConnectionParameters()
        {
            ClosedCommand = new RelayCommand(ExecuteClosedCommand);
        }
        public ConnectionParameters(int IOA)
        {
            sizeOfIOA = IOA;
        }

        public int K
        {
            get
            {
                return k;
            }
            set
            {
                k = value;
                RaisePropertyChanged("K");
            }
        }

        public int W
        {
            get
            {
                return w;
            }
            set
            {
                w = value;
                RaisePropertyChanged("W");
            }
        }

        public int T0
        {
            get
            {
                return t0;
            }
            set
            {
                t0 = value;
                RaisePropertyChanged("T0");
            }
        }

        public int T1
        {
            get
            {
                return t1;
            }
            set
            {
                t1 = value;
                RaisePropertyChanged("T1");
            }
        }

        public int T2
        {
            get
            {
                return t2;
            }
            set
            {
                t2 = value;
                RaisePropertyChanged("T2");
            }
        }

        public int T3
        {
            get
            {
                return t3;
            }
            set
            {
                t3 = value;
                RaisePropertyChanged("T3");
            }
        }
        /// <summary>
        /// SizeOfCOT
        /// </summary>
        public int SizeOfCOT
        {
            get
            {
                return sizeOfCOT;
            }
            set
            {
                sizeOfCOT = value;
                RaisePropertyChanged("SizeOfCOT");
            }
        }

        public int OriginatorAddress
        {
            get
            {
                return originatorAddress;
            }
            set
            {
                originatorAddress = value;
                RaisePropertyChanged("OriginatorAddress");
            }
        }
        /// <summary>
        /// common address of ASDUs (1/2) */
        /// </summary>
        public int SizeOfCA
        {
            get
            {
                return sizeOfCA;
            }
            set
            {
                sizeOfCA = value;
                RaisePropertyChanged("SizeOfCA");
            }
        }
        /// <summary>
        /// information object address (1/2/3)
        /// </summary>
        public int SizeOfIOA
        {
            get
            {
                return sizeOfIOA;
            }
            set
            {
                sizeOfIOA = value;
                RaisePropertyChanged("SizeOfIOA");
            }
        }


        public int SizeOfTypeId
        {
            get
            {
                return sizeOfTypeId;
            }

        }

        public int SizeOfVSQ
        {
            get
            {
                return sizeOfVSQ;
            }
        }

        public int RestranCount
        {
            get
            {
                return restranCount;
            }
            set
            {
                restranCount = value;
                RaisePropertyChanged("RestranCount");
            }
        }
        public int RestranTime
        {
            get
            {
                return restranTime;
            }
            set
            {
                restranTime = value;
                RaisePropertyChanged("RestranTime");
            }
        }
        public int PollTime
        {
            get
            {
                return pollTime;
            }
            set
            {
                pollTime = value;
                RaisePropertyChanged("PollTime");
            }
        }

    }
}

