/*
 *  EventOfProtectionEquipment.cs
 */

using System;

namespace lib60870
{
	public class EventOfProtectionEquipment : InformationObject
	{
		override public int GetEncodedSize() {
			return 6;
		}

		override public TypeID Type {
			get {
				return TypeID.M_EP_TA_1;
			}
		}

		override public bool SupportsSequence {
			get {
				return false;
			}
		}

		private SingleEvent singleEvent;

		public SingleEvent Event {
			get {
				return singleEvent;
			}
		}

		private CP16Time2a elapsedTime;

		public CP16Time2a ElapsedTime {
			get {
				return this.elapsedTime;
			}
		}

		private CP24Time2a timestamp;

		public CP24Time2a Timestamp {
			get {
				return this.timestamp;
			}
		}

		public EventOfProtectionEquipment(int ioa, SingleEvent singleEvent, CP16Time2a elapsedTime, CP24Time2a timestamp)
			: base (ioa)
		{
			this.singleEvent = singleEvent;
			this.elapsedTime = elapsedTime;
			this.timestamp = timestamp;
		}

		internal EventOfProtectionEquipment (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
		base(parameters, msg, startIndex, isSequence)
		{
			if (!isSequence)
				startIndex += parameters.SizeOfIOA; /* skip IOA */

			singleEvent = new SingleEvent (msg [startIndex++]);

			elapsedTime = new CP16Time2a (msg, startIndex);
			startIndex += 2;

			/* parse CP56Time2a (time stamp) */
			timestamp = new CP24Time2a (msg, startIndex);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.SetNextByte (singleEvent.EncodedValue);

			frame.AppendBytes (elapsedTime.GetEncodedValue ());

			frame.AppendBytes (timestamp.GetEncodedValue ());
		}
	}

	public class EventOfProtectionEquipmentWithCP56Time2a : InformationObject
	{
		override public int GetEncodedSize() {
			return 10;
		}

		override public TypeID Type {
			get {
				return TypeID.M_EP_TD_1;
			}
		}

		override public bool SupportsSequence {
			get {
				return false;
			}
		}

		private SingleEvent singleEvent;

		public SingleEvent Event {
			get {
				return singleEvent;
			}
		}

		private CP16Time2a elapsedTime;

		public CP16Time2a ElapsedTime {
			get {
				return this.elapsedTime;
			}
		}

		private CP56Time2a timestamp;

		public CP56Time2a Timestamp {
			get {
				return this.timestamp;
			}
		}

		public EventOfProtectionEquipmentWithCP56Time2a (int ioa, SingleEvent singleEvent, CP16Time2a elapsedTime, CP56Time2a timestamp)
			: base (ioa)
		{
			this.singleEvent = singleEvent;
			this.elapsedTime = elapsedTime;
			this.timestamp = timestamp;
		}

		internal EventOfProtectionEquipmentWithCP56Time2a (ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
		base(parameters, msg, startIndex, isSequence)
		{
			if (!isSequence)
				startIndex += parameters.SizeOfIOA; /* skip IOA */

			singleEvent = new SingleEvent (msg [startIndex++]);

			elapsedTime = new CP16Time2a (msg, startIndex);
			startIndex += 2;

			/* parse CP56Time2a (time stamp) */
			timestamp = new CP56Time2a (msg, startIndex);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.SetNextByte (singleEvent.EncodedValue);

			frame.AppendBytes (elapsedTime.GetEncodedValue ());

			frame.AppendBytes (timestamp.GetEncodedValue ());
		}
	}
}

