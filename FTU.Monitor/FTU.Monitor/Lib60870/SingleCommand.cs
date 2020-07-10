
using System;

namespace lib60870
{

	/// <summary>
	/// Regulating step command state (RCS) according to IEC 60870-5-101:2003 7.2.6.17
	/// </summary>
	public enum StepCommandValue {
		INVALID_0 = 0,
		LOWER = 1,
		HIGHER = 2,
		INVALID_3 = 3
	}

	public class SingleCommand : InformationObject
	{
		override public int GetEncodedSize() {
			return 1;
		}

		override public TypeID Type {
			get {
				return TypeID.C_SC_NA_1;
			}
		}

		override public bool SupportsSequence {
			get {
				return false;
			}
		}

		private byte sco;

		public SingleCommand (int ioa, bool command, bool selectCommand, int qu) : base(ioa)
		{
			sco = (byte) ((qu & 0x1f) * 4);

			if (command) sco |= 0x01;

			if (selectCommand)
				sco |= 0x80;
		}

		internal SingleCommand (ConnectionParameters parameters, byte[] msg, int startIndex) :
			base(parameters, msg, startIndex, false)
		{
			startIndex += parameters.SizeOfIOA; /* skip IOA */

			sco = msg [startIndex++];
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.SetNextByte (sco);
		}

		public int QU {
			get {
				return ((sco & 0x7c) / 4);
			}
		}

		/// <summary>
		/// Gets the state (off - false / on - true) of this command
		/// </summary>
		/// <value><c>true</c> if on; otherwise, <c>false</c>.</value>
		public bool State {
			get {
				return ((sco & 0x01) == 0x01);
			}
		}

		/// <summary>
		/// Indicates if the command is a select or an execute command
		/// </summary>
		/// <value><c>true</c> if select; execute, <c>false</c>.</value>
		public bool Select {
			get {
				return ((sco & 0x80) == 0x80);
			}
		}

		public override string ToString ()
		{
			return string.Format ("[SingleCommand: QU={0}, State={1}, Select={2}]", QU, State, Select);
		}
		
	}

	public class SingleCommandWithCP56Time2a : SingleCommand
	{
		override public TypeID Type {
			get {
				return TypeID.C_SC_TA_1;
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
				return timestamp;
			}
		}

		public SingleCommandWithCP56Time2a (int ioa, bool command, bool selectCommand, int qu, CP56Time2a timestamp) : 
			base(ioa, command, selectCommand, qu)
		{
			this.timestamp = timestamp;
		}

		internal SingleCommandWithCP56Time2a (ConnectionParameters parameters, byte[] msg, int startIndex) :
			base(parameters, msg, startIndex)
		{
			startIndex += parameters.SizeOfIOA + 1; /* skip IOA + SCQ*/

			timestamp = new CP56Time2a (msg, startIndex);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.AppendBytes (timestamp.GetEncodedValue ());
		}
	}

	public class DoubleCommand : InformationObject
	{
		override public int GetEncodedSize() {
			return 1;
		}

		override public TypeID Type {
			get {
				return TypeID.C_DC_NA_1;
			}
		}

		override public bool SupportsSequence {
			get {
				return false;
			}
		}

		public static int OFF = 1;
		public static int ON = 2;

		private byte dcq;

		public DoubleCommand (int ioa, int command, bool select, int quality) : base(ioa)
		{
			dcq = (byte) (command & 0x03);
			dcq += (byte)((quality & 0x1f) * 4);

			if (select)
				dcq |= 0x80;
		}

		internal DoubleCommand (ConnectionParameters parameters, byte[] msg, int startIndex) :
			base(parameters, msg, startIndex, false)
		{
			startIndex += parameters.SizeOfIOA; /* skip IOA */

			dcq = msg [startIndex++];
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.SetNextByte (dcq);
		}

		public int QU {
			get {
				return ((dcq & 0x7c) / 4);
			}
		}

		public int State {
			get {
				return (dcq & 0x03);
			}
		}

		public bool Select {
			get {
				return ((dcq & 0x80) == 0x80);
			}
		}
	}

	public class DoubleCommandWithCP56Time2a : DoubleCommand
	{
		override public TypeID Type {
			get {
				return TypeID.C_DC_TA_1;
			}
		}

		override public bool SupportsSequence {
			get {
				return false;
			}
		}

		public CP56Time2a Timestamp {
			get {
				return timestamp;
			}
		}

		private CP56Time2a timestamp;

		public DoubleCommandWithCP56Time2a (int ioa, int command, bool select, int quality, CP56Time2a timestamp) : 
			base(ioa, command, select, quality)
		{
			this.timestamp = timestamp;
		}

		internal DoubleCommandWithCP56Time2a (ConnectionParameters parameters, byte[] msg, int startIndex) :
			base(parameters, msg, startIndex)
		{
			startIndex += parameters.SizeOfIOA + 1; /* skip IOA + DCQ*/

			timestamp = new CP56Time2a (msg, startIndex);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.AppendBytes (timestamp.GetEncodedValue ());
		}
	
	}


	public class StepCommand : DoubleCommand 
	{
		override public TypeID Type {
			get {
				return TypeID.C_RC_NA_1;
			}
		}

		override public bool SupportsSequence {
			get {
				return false;
			}
		}

		public StepCommand (int ioa, StepCommandValue command, bool select, int quality) : base(ioa, (int) command, select, quality)
		{
		}

		internal StepCommand (ConnectionParameters parameters, byte[] msg, int startIndex) :
			base(parameters, msg, startIndex)
		{
		}

		public new StepCommandValue State {
			get {
				return (StepCommandValue) (base.State);
			}
		}
	}

	public class StepCommandWithCP56Time2a : StepCommand
	{
		override public TypeID Type {
			get {
				return TypeID.C_RC_TA_1;
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
				return timestamp;
			}
		}

		public StepCommandWithCP56Time2a (int ioa, StepCommandValue command, bool select, int quality, CP56Time2a timestamp) : 
			base(ioa, command, select, quality)
		{
			this.timestamp = timestamp;
		}

		internal StepCommandWithCP56Time2a (ConnectionParameters parameters, byte[] msg, int startIndex) :
			base(parameters, msg, startIndex)
		{
			startIndex += parameters.SizeOfIOA + 1; /* skip IOA + DCQ*/

			timestamp = new CP56Time2a (msg, startIndex);
		}

		internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence) {
			base.Encode(frame, parameters, isSequence);

			frame.AppendBytes (timestamp.GetEncodedValue ());
		}
			
	}



}

