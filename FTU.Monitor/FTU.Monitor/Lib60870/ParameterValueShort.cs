using lib60870;
using System;

namespace FTU.Monitor.lib60870
{
    public class ParameterValueShort : InformationObject
    {
        override public int GetEncodedSize() //信息体长度
        {
            return 6;
        }

        override public TypeID Type
        {
            get
            {
                return TypeID.C_RS_NA_1;//202
            }
        }

        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        private float value;

        public float Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
        private int sn;//定值区号

        public int SN
        {
            get { return sn; }
            set { sn = value; }
        }

        private byte tag = 38;//单精度浮点型

        public byte Tag
        {
            get
            {
                return this.tag;
            }
        }
        private byte len = 4;

        public byte Len//数据长度
        {
            get { return len; }
            set { len = value; }
        }

        internal ParameterValueShort(ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
            base(parameters, msg, startIndex, isSequence)
        {
            startIndex += parameters.SizeOfIOA; /* skip IOA */
            tag = (byte)startIndex++;
            len = (byte)startIndex++;
            /* parse float value */
            // 返回由字节数组中指定位置的四个字节转换来的单精度浮点数。
            value = System.BitConverter.ToSingle(msg, startIndex);
            startIndex += 4;
        }

        internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence)
        {
            base.Encode(frame, parameters, isSequence);

            byte[] floatEncoded = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian == false)
                Array.Reverse(floatEncoded);

            frame.AppendBytes(floatEncoded);

            //frame.SetNextByte (quality.EncodedValue);
        }
    }
}
