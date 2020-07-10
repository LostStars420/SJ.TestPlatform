/*
 * 
 */
using System;

namespace lib60870
{
    internal class BufferFrame : Frame
    {

        private byte[] buffer;
        private int startPos;
        private int bufPos;

        public BufferFrame(byte[] buffer, int startPos)
        {
            this.buffer = buffer;
            this.startPos = startPos;
            this.bufPos = startPos;
        }

        public override void ResetFrame()
        {
            bufPos = startPos;
        }

        public override void SetNextByte(byte value)
        {
            buffer[bufPos++] = value;
        }

        public override void AppendBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
                buffer[bufPos++] = bytes[i];
        }

        public override int GetMsgSize()
        {
            return bufPos;
        }

        public override byte[] GetBuffer()
        {
            return buffer;
        }
    }
}

