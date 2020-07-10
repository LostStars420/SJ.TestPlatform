/*
 * 品质描述词
 */

using System;

namespace lib60870
{
	public class QualityDescriptor
	{
		private byte encodedValue;

		public static QualityDescriptor VALID() {
			return new QualityDescriptor ();
		}

		public static QualityDescriptor INVALID() {
			var qd = new QualityDescriptor ();
			qd.Invalid = true;
			return qd;
		}

		public QualityDescriptor()
		{
			this.encodedValue = 0;
		}

		public QualityDescriptor (byte encodedValue)
		{
			this.encodedValue = encodedValue;
		}
        /// <summary>
        /// 未溢出
        /// </summary>
		public bool Overflow {
			get {
				if ((encodedValue & 0x01) != 0)
					return true;
				else
					return false;
			}

			set {
				if (value) 
					encodedValue |= 0x01;
				else
					encodedValue &= 0xfe;
			}
		}
        /// <summary>
        /// 未被闭锁
        /// </summary>
		public bool Blocked {
			get {
				if ((encodedValue & 0x10) != 0)
					return true;
				else
					return false;
			}

			set {
				if (value) 
					encodedValue |= 0x10;
				else
					encodedValue &= 0xef;
			}
		}
        /// <summary>
        /// 未被取代
        /// </summary>
		public bool Substituted {
			get {
				if ((encodedValue & 0x20) != 0)
					return true;
				else
					return false;
			}

			set {
				if (value) 
					encodedValue |= 0x20;
				else
					encodedValue &= 0xdf;
			}
		}

        /// <summary>
        /// 当前值
        /// </summary>
		public bool NonTopical {
			get {
				if ((encodedValue & 0x40) != 0)
					return true;
				else
					return false;
			}

			set {
				if (value) 
					encodedValue |= 0x40;
				else
					encodedValue &= 0xbf;
			}
		}

        /// <summary>
        /// 有效
        /// </summary>
		public bool Invalid {
			get {
				if ((encodedValue & 0x80) != 0)
					return true;
				else
					return false;
			}

			set {
				if (value) 
					encodedValue |= 0x80;
				else
					encodedValue &= 0x7f;
			}
		}

		public byte EncodedValue {
			get {
				return this.encodedValue;
			}
			set {
				encodedValue = value;
			}
		}

		public override string ToString ()
		{
			return string.Format ("[QualityDescriptor: Overflow={0}, Blocked={1}, Substituted={2}, NonTopical={3}, Invalid={4}]", Overflow, Blocked, Substituted, NonTopical, Invalid);
		}
	}
		
}

