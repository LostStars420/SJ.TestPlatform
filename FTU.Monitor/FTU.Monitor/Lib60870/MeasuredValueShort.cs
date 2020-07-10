/*
 *  MeasuredValueShortFloat.cs
 *  ¶Ì¸¡µãÊý
 */

using System;

namespace lib60870
{
	public class MeasuredValueShort : InformationObject
	{
		override public int GetEncodedSize() {
			return 5;
		}

		override public TypeID Type {
			get {
				return TypeID.M_ME_NC_1;
			}
		}

		override public bool SupportsSequence {
			get {
				return true;
			}
		}

		private float value;

		public float Value {
			get {
				return this.value;
			}
			set {
				this.value = value;
			}
		}

		private QualityDescriptor quality;

		public QualityDescriptor Quality {
			get {
				return this.quality;
			}
		}

		public MeasuredValueShort (int objectAddress, float value, QualityDescriptor quality)
			: base(objectAddress)
		{
			this.value = value;
			this.quality = quality;
		}


		internal MeasuredValueShort (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
			base(parameters, msg, startIndex, isSequence)
		{
			if (!isSequence)
				startIndex += parameters.SizeOfIOA; /* skip IOA */

			/* parse float value */
			value = System.BitConverter.ToSingle (msg, startIndex);
			startIndex += 4;

			/* parse QDS (quality) */
			quality = new QualityDescriptor (msg [startIndex++]);
		}
        internal MeasuredValueShort(ConnectionParameters parameters, byte[] msg, int startIndex) :
            base(parameters, msg, startIndex)
        {
            startIndex += parameters.SizeOfIOA; /* skip IOA */

            /* parse float value */
            value = System.BitConverter.ToSingle(msg, startIndex);
            startIndex += 4;
        }
		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			byte[] floatEncoded = BitConverter.GetBytes (value);

			if (BitConverter.IsLittleEndian == false)
				Array.Reverse (floatEncoded);

			frame.AppendBytes (floatEncoded);

			frame.SetNextByte (quality.EncodedValue);
		}
	}

	public class MeasuredValueShortWithCP24Time2a : MeasuredValueShort
	{
		override public int GetEncodedSize() {
			return 8;
		}

		override public TypeID Type {
			get {
				return TypeID.M_ME_TC_1;
			}
		}

		override public bool SupportsSequence {
			get {
				return false;
			}
		}

		private CP24Time2a timestamp;

		public CP24Time2a Timestamp {
			get {
				return this.timestamp;
			}
		}

		public MeasuredValueShortWithCP24Time2a (int objectAddress, float value, QualityDescriptor quality, CP24Time2a timestamp)
			: base(objectAddress, value, quality)
		{
			this.timestamp = timestamp;
		}

		internal MeasuredValueShortWithCP24Time2a (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
		base(parameters, msg, startIndex, isSequence)
		{
			if (!isSequence)
				startIndex += parameters.SizeOfIOA; /* skip IOA */

			startIndex += 5; /* skip float */

			/* parse CP56Time2a (time stamp) */
			timestamp = new CP24Time2a (msg, startIndex);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.AppendBytes (timestamp.GetEncodedValue ());
		}

	}

	public class MeasuredValueShortWithCP56Time2a : MeasuredValueShort
	{
		override public int GetEncodedSize() {
			return 12;
		}

		override public TypeID Type {
			get {
				return TypeID.M_ME_TF_1;
			}
		}

		override public bool SupportsSequence {
			get {
				return false;
			}
		}

		private CP56Time2a timestamp;

		public CP56Time2a Timestamp {
			get {
				return this.timestamp;
			}
		}

		public MeasuredValueShortWithCP56Time2a (int objectAddress, float value, QualityDescriptor quality, CP56Time2a timestamp)
			: base(objectAddress, value, quality)
		{
			this.timestamp = timestamp;
		}

		internal MeasuredValueShortWithCP56Time2a (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
		base(parameters, msg, startIndex, isSequence)
		{
			if (!isSequence)
				startIndex += parameters.SizeOfIOA; /* skip IOA */

			startIndex += 5; /* skip float */

			/* parse CP56Time2a (time stamp) */
			timestamp = new CP56Time2a (msg, startIndex);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.AppendBytes (timestamp.GetEncodedValue ());
		}
	}
}

