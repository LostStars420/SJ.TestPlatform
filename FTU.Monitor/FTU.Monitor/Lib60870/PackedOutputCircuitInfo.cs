/*
 */

using System;

namespace lib60870
{

	public class PackedOutputCircuitInfo : InformationObject
	{
		override public int GetEncodedSize() {
			return 7;
		}

		override public TypeID Type {
			get {
				return TypeID.M_EP_TC_1;
			}
		}

		override public bool SupportsSequence {
			get {
				return true;
			}
		}

		OutputCircuitInfo oci;

		public OutputCircuitInfo OCI {
			get {
				return this.oci;
			}
		}

		QualityDescriptorP qdp;

		public QualityDescriptorP QDP {
			get {
				return this.qdp;
			}
		}

		private CP16Time2a operatingTime;

		public CP16Time2a OperatingTime {
			get {
				return this.operatingTime;
			}
		}

		private CP24Time2a timestamp;

		public CP24Time2a Timestamp {
			get {
				return this.timestamp;
			}
		}

		public PackedOutputCircuitInfo (int objectAddress, OutputCircuitInfo oci, QualityDescriptorP qdp, CP16Time2a operatingTime, CP24Time2a timestamp)
			: base(objectAddress)
		{
			this.oci = oci;
			this.qdp = qdp;
			this.operatingTime = operatingTime;
			this.timestamp = timestamp;
		}
			
		internal PackedOutputCircuitInfo (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
		base(parameters, msg, startIndex, isSequence)
		{
			if (!isSequence)
				startIndex += parameters.SizeOfIOA; /* skip IOA */

			oci = new OutputCircuitInfo (msg [startIndex++]);

			qdp = new QualityDescriptorP (msg [startIndex++]);

			operatingTime = new CP16Time2a (msg, startIndex);
			startIndex += 2;

			/* parse CP56Time2a (time stamp) */
			timestamp = new CP24Time2a (msg, startIndex);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.SetNextByte (oci.EncodedValue);

			frame.SetNextByte (qdp.EncodedValue);

			frame.AppendBytes (operatingTime.GetEncodedValue ());

			frame.AppendBytes (timestamp.GetEncodedValue ());
		}
	}

	public class PackedOutputCircuitInfoWithCP56Time2a : InformationObject
	{
		override public int GetEncodedSize() {
			return 11;
		}

		override public TypeID Type {
			get {
				return TypeID.M_EP_TF_1;
			}
		}

		override public bool SupportsSequence {
			get {
				return true;
			}
		}

		OutputCircuitInfo oci;

		public OutputCircuitInfo OCI {
			get {
				return this.oci;
			}
		}

		QualityDescriptorP qdp;

		public QualityDescriptorP QDP {
			get {
				return this.qdp;
			}
		}

		private CP16Time2a operatingTime;

		public CP16Time2a OperatingTime {
			get {
				return this.operatingTime;
			}
		}

		private CP56Time2a timestamp;

		public CP56Time2a Timestamp {
			get {
				return this.timestamp;
			}
		}


		public PackedOutputCircuitInfoWithCP56Time2a (int objectAddress, OutputCircuitInfo oci, QualityDescriptorP qdp, CP16Time2a operatingTime, CP56Time2a timestamp)
			: base(objectAddress)
		{
			this.oci = oci;
			this.qdp = qdp;
			this.operatingTime = operatingTime;
			this.timestamp = timestamp;
		}

		internal PackedOutputCircuitInfoWithCP56Time2a (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
		base(parameters, msg, startIndex, isSequence)
		{
			if (!isSequence)
				startIndex += parameters.SizeOfIOA; /* skip IOA */

			oci = new OutputCircuitInfo (msg [startIndex++]);

			qdp = new QualityDescriptorP (msg [startIndex++]);

			operatingTime = new CP16Time2a (msg, startIndex);
			startIndex += 2;

			/* parse CP56Time2a (time stamp) */
			timestamp = new CP56Time2a (msg, startIndex);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.SetNextByte (oci.EncodedValue);

			frame.SetNextByte (qdp.EncodedValue);

			frame.AppendBytes (operatingTime.GetEncodedValue ());

			frame.AppendBytes (timestamp.GetEncodedValue ());
		}
	}
}
