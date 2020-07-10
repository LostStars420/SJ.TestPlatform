/*
 *  Frame.cs
 *
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace lib60870
{
	internal abstract class Frame
	{
		public abstract void ResetFrame ();

		public abstract void SetNextByte (byte value);

		public abstract void AppendBytes (byte[] bytes);

		public abstract int GetMsgSize ();

		public abstract byte[] GetBuffer ();
	}
}
