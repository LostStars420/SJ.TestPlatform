/*
 *  MeasuredValueScaled.cs
 *  
 */

using System;

namespace lib60870
{
	public class MeasuredValueScaled : InformationObject
	{
		override public int GetEncodedSize() {
			return 3;
		}

		override public TypeID Type {
			get {
				return TypeID.M_ME_NB_1;
			}
		}

		override public bool SupportsSequence {
			get {
				return true;
			}
		}

		private ScaledValue scaledValue;

		public ScaledValue ScaledValue {
			get {
				return this.scaledValue;
			}
		}

		private QualityDescriptor quality;

		public QualityDescriptor Quality {
			get {
				return this.quality;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="lib60870.MeasuredValueScaled"/> class.
		/// </summary>
		/// <param name="objectAddress">Information object address</param>
		/// <param name="value">scaled value (range -32768 - 32767) </param>
		/// <param name="quality">quality descriptor (according to IEC 60870-5-101:2003 7.2.6.3)</param>
		public MeasuredValueScaled (int objectAddress, int value, QualityDescriptor quality)
			: base(objectAddress)
		{
			this.scaledValue = new ScaledValue(value);
			this.quality = quality;
		}

		internal MeasuredValueScaled (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSquence) :
			base(parameters, msg, startIndex, isSquence)
		{
			if (!isSquence) 
				startIndex += parameters.SizeOfIOA; /* skip IOA */

			scaledValue = new ScaledValue (msg, startIndex);
			startIndex += 2;

			/* parse QDS (quality) */
			quality = new QualityDescriptor (msg [startIndex++]);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.AppendBytes (scaledValue.GetEncodedValue ());

			frame.SetNextByte (quality.EncodedValue);
		}
	}
	public class MeasuredValueScaledWithCP24Time2a : MeasuredValueScaled
	{
		override public int GetEncodedSize() {
			return 6;
		}

		override public TypeID Type {
			get {
				return TypeID.M_ME_TB_1;
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

		public MeasuredValueScaledWithCP24Time2a (int objectAddress, int value, QualityDescriptor quality, CP24Time2a timestamp)
			: base(objectAddress, value, quality)
		{
			this.timestamp = timestamp;
		}

		internal MeasuredValueScaledWithCP24Time2a (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
			base(parameters, msg, startIndex, isSequence)
		{
			if (!isSequence)
				startIndex += parameters.SizeOfIOA; /* skip IOA */

			startIndex += 3; /* scaledValue + QDS */

			/* parse CP56Time2a (time stamp) */
			timestamp = new CP24Time2a (msg, startIndex);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.AppendBytes (timestamp.GetEncodedValue ());
		}
	}

	public class MeasuredValueScaledWithCP56Time2a : MeasuredValueScaled
	{
		override public int GetEncodedSize() {
			return 10;
		}

		override public TypeID Type {
			get {
				return TypeID.M_ME_TE_1;
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

		public MeasuredValueScaledWithCP56Time2a (int objectAddress, int value, QualityDescriptor quality, CP56Time2a timestamp)
			: base(objectAddress, value, quality)
		{
			this.timestamp = timestamp;
		}
			
		internal MeasuredValueScaledWithCP56Time2a (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
		base(parameters, msg, startIndex, isSequence)
		{
			if (!isSequence)
				startIndex += parameters.SizeOfIOA; /* skip IOA */

			startIndex += 3; /* scaledValue + QDS */

			/* parse CP56Time2a (time stamp) */
			timestamp = new CP56Time2a (msg, startIndex);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.AppendBytes (timestamp.GetEncodedValue ());
		}

	}


}

