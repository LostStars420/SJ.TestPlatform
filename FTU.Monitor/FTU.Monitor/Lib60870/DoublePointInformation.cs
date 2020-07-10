/*
 *  DoublePointInformation.cs
 */

using System;

namespace lib60870
{
	public enum DoublePointValue {
		INTERMEDIATE = 0,
		OFF = 1,
		ON = 2,
		INDETERMINATE = 3
	}

	public class DoublePointInformation : InformationObject
	{
		override public int GetEncodedSize() {
			return 1;
		}

		override public TypeID Type {
			get {
				return TypeID.M_DP_NA_1;
			}
		}

		override public bool SupportsSequence {
			get {
				return true;
			}
		}

		private DoublePointValue value;

		public DoublePointValue Value {
			get {
				return this.value;
			}
		}

		private QualityDescriptor quality;

		public QualityDescriptor Quality {
			get {
				return this.quality;
			}
		}

		public DoublePointInformation(int ioa, DoublePointValue value, QualityDescriptor quality)
			: base(ioa)
		{
			this.value = value;
			this.quality = quality;
		}

		internal DoublePointInformation (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
			base(parameters, msg, startIndex, isSequence)
		{
			if (!isSequence)
				startIndex += parameters.SizeOfIOA; /* skip IOA */

			/* parse DIQ (double point information with qualitiy) */
			byte diq = msg [startIndex++];

			value = (DoublePointValue)(diq & 0x03);

			quality = new QualityDescriptor ((byte) (diq & 0xf0));
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			byte val = quality.EncodedValue;

			val += (byte)value;

			frame.SetNextByte (val);
		}
	}

	public class DoublePointWithCP24Time2a : DoublePointInformation
	{
		override public int GetEncodedSize() {
			return 4;
		}

		override public TypeID Type {
			get {
				return TypeID.M_DP_TA_1;
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

		public DoublePointWithCP24Time2a(int ioa, DoublePointValue value, QualityDescriptor quality, CP24Time2a timestamp)
			: base(ioa, value, quality)
		{
			this.timestamp = timestamp;
		}

		internal DoublePointWithCP24Time2a (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
		base(parameters, msg, startIndex, isSequence)
		{
			if (!isSequence)
				startIndex += parameters.SizeOfIOA; /* skip IOA */

			startIndex += 1; /* skip DIQ */

			/* parse CP24Time2a (time stamp) */
			timestamp = new CP24Time2a (msg, startIndex);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.AppendBytes (timestamp.GetEncodedValue ());
		}
	}

	public class DoublePointWithCP56Time2a : DoublePointInformation
	{
		override public int GetEncodedSize() {
			return 8;
		}

		override public TypeID Type {
			get {
				return TypeID.M_DP_TB_1;
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

		public DoublePointWithCP56Time2a(int ioa, DoublePointValue value, QualityDescriptor quality, CP56Time2a timestamp)
			: base(ioa, value, quality)
		{
			this.timestamp = timestamp;
		}

		internal DoublePointWithCP56Time2a (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
			base(parameters, msg, startIndex, isSequence)
		{
			if (!isSequence)
				startIndex += parameters.SizeOfIOA; /* skip IOA */

			startIndex += 1; /* skip DIQ */

			/* parse CP56Time2a (time stamp) */
			timestamp = new CP56Time2a (msg, startIndex);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.AppendBytes (timestamp.GetEncodedValue ());
		}
	}

}

