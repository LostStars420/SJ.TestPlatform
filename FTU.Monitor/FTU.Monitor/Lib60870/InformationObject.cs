/*
 *  InformationObject.cs
 *  信息体
 */

using System;

namespace lib60870
{
	public abstract class InformationObject
	{
		private int objectAddress;
        /// <summary>
        /// 返回信息体对象地址 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
		internal static int ParseInformationObjectAddress(ConnectionParameters parameters, byte[] msg, int startIndex)
		{
			int ioa = msg [startIndex];

			if (parameters.SizeOfIOA > 1)
				ioa += (msg [startIndex + 1] * 0x100);

			if (parameters.SizeOfIOA > 2)
				ioa += (msg [startIndex + 2] * 0x10000);

			return ioa;
		}

		internal InformationObject (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence)
		{
			if (!isSequence)
				objectAddress = ParseInformationObjectAddress (parameters, msg, startIndex);
		}
        internal InformationObject(ConnectionParameters parameters, byte[] msg, int startIndex)
        {
            
          objectAddress = ParseInformationObjectAddress(parameters, msg, startIndex);
        }
		public InformationObject(int objectAddress) {
			this.objectAddress = objectAddress;
		}
        public InformationObject()
        {
           
        }

		/// <summary>
		/// Gets the encoded size of the object (without the IOA)
		/// </summary>
		/// <returns>The encoded size.</returns>
		public abstract int GetEncodedSize();

		public int ObjectAddress {
			get {
				return this.objectAddress;
			}
			internal set {
				objectAddress = value;
			}
		}
			
		public abstract bool SupportsSequence {
			get;
		}

		public abstract TypeID Type {
			get;
		}

		internal virtual void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			if (!isSequence) {
				frame.SetNextByte ((byte)(objectAddress & 0xff));

				if (parameters.SizeOfIOA > 1)
					frame.SetNextByte ((byte)((objectAddress / 0x100) & 0xff));

				if (parameters.SizeOfIOA > 2)
					frame.SetNextByte ((byte)((objectAddress / 0x10000) & 0xff));
			}
		}


	}
}

