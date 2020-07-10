using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTU.Monitor.DataService
{
   public class SerialPortConnectionParameter
    {
        private int sizeOfTypeId = 1;

		private int sizeOfVSQ = 1; /* VSQ = variable sturcture qualifier */

		private int sizeOfCOT = 2; /* (parameter b) COT = cause of transmission (1/2) */

		private int originatorAddress = 0;

		private int sizeOfCA = 2; /* (parameter a) CA = common address of ASDUs (1/2) */
	
		private int sizeOfIOA = 3; /* (parameter c) IOA = information object address (1/2/3) */

        public SerialPortConnectionParameter()
		{
		}


        /// <summary>
        /// SizeOfCOT
        /// </summary>
		public int SizeOfCOT {
			get {
				return this.sizeOfCOT;
			}
			set {
				sizeOfCOT = value;
			}
		}

		public int OriginatorAddress {
			get {
				return this.originatorAddress;
			}
			set {
				originatorAddress = value;
			}
		}
        /// <summary>
        /// common address of ASDUs (1/2) */
        /// </summary>
		public int SizeOfCA {
			get {
				return sizeOfCA;
			}
			set {
				sizeOfCA = value;
			}
		}
        /// <summary>
        /// information object address (1/2/3)
        /// </summary>
		public int SizeOfIOA {
			get {
				return this.sizeOfIOA;
			}
			set {
				sizeOfIOA = value;
			}
		}	


		public int SizeOfTypeId {
			get {
				return this.sizeOfTypeId;
			}
		}

		public int SizeOfVSQ {
			get {
				return this.sizeOfVSQ;
			}
		}
	}
}

