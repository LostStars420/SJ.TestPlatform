/*
 *  BinaryCounterReading.cs
 */

using System;

namespace lib60870
{
	public class BinaryCounterReading
	{

		private byte[] encodedValue = new byte[5];

		public byte[] GetEncodedValue() 
		{
			return encodedValue;
		}

		public Int32 Value {
			get {
				Int32 value = encodedValue [0];
				value += (encodedValue [1] * 0x100);
				value += (encodedValue [2] * 0x10000);
				value += (encodedValue [3] * 0x1000000);

				return value;
			}

			set {
				byte[] valueBytes = BitConverter.GetBytes (value);

				if (BitConverter.IsLittleEndian == false)
					Array.Reverse (valueBytes);

				Array.Copy (valueBytes, encodedValue, 4);
			}
		}

		public int SequenceNumber {
			get {
				return (encodedValue [4] & 0x1f);
			}

			set {
				int seqNumber = value & 0x1f;
				int flags = encodedValue[4] & 0xe0;

				encodedValue[4] = (byte) (flags | seqNumber);
			}
		}

		public bool Carry {
			get {
				return ((encodedValue[4] & 0x20) == 0x20);
			}

			set {
				if (value)
					encodedValue[4] |= 0x20;
				else
					encodedValue[4] &= 0xdf;
			}
		}

		public bool Adjusted {
			get {
				return ((encodedValue[4] & 0x40) == 0x40);
			}

			set {
				if (value)
					encodedValue[4] |= 0x40;
				else
					encodedValue[4] &= 0xbf;
			}
		}

		public bool Invalid {
			get {
				return ((encodedValue[4] & 0x80) == 0x80);
			}

			set {
				if (value)
					encodedValue[4] |= 0x80;
				else
					encodedValue[4] &= 0x7f;
			}
		}

		internal BinaryCounterReading (byte[] msg, int startIndex)
		{
			if (msg.Length < startIndex + 5)
				throw new ASDUParsingException ("Message too small for parsing BinaryCounterReading");

			for (int i = 0; i < 5; i++)
				encodedValue [i] = msg [startIndex + i];
		}

		public BinaryCounterReading ()
		{
		}
	}
}

