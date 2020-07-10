/*
 *  CP16Time2a.cs
 */

using System;

namespace lib60870
{
	public class CP16Time2a
	{
		private byte[] encodedValue = new byte[2];

		internal CP16Time2a (byte[] msg, int startIndex)
		{
			if (msg.Length < startIndex + 3)
				throw new ASDUParsingException ("Message too small for parsing CP16Time2a");

			for (int i = 0; i < 2; i++)
				encodedValue [i] = msg [startIndex + i];
		}

		public CP16Time2a(int elapsedTimeInMs)
		{
			ElapsedTimeInMs = elapsedTimeInMs;
		}
        public CP16Time2a()
        {
            encodedValue[0] = 0;
            encodedValue[1] = 0;
        }
		public int ElapsedTimeInMs {
			get {
				return (encodedValue[0] + (encodedValue[1] * 0x100));
			}

			set {
				encodedValue [0] = (byte) (value % 0x100);
				encodedValue [1] = (byte) (value / 0x100);
			}
		}

		public byte[] GetEncodedValue() 
		{
			return encodedValue;
		}

	}
}

