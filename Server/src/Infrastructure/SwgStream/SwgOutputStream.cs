using System.IO;
using System.Text;

namespace SwgAnh.Docker.Infrastructure.SwgStream
{
    public class SwgOutputStream : BinaryWriter
    {
        private int _written;
        private short _opCode;
        private short _sequence;
        private short _updateType;

        public SwgOutputStream()
        {

        }

        public SwgOutputStream(Stream stream) : base(stream)
        {
        }

        public SwgOutputStream(Stream stream, Encoding encoding) : base(stream, encoding)
        {
        }

        public void SetOpCode(short value)
        {
            WriteShort(value);
            _opCode = value;
        }

        public void SetSequence(short value)
        {
            if (_written == 2)
            {
                var reversedValue = ReverseBytes(value);
                _sequence = reversedValue;
                WriteShort(_sequence);
            }
            else
            {
                throw new System.Exception("Sequence must be set right after OPCode");
            };
        }
        public void WriteByteReversed(short value)
        {
            var val = ReverseBytes(value);
            WriteShort(val);
        }
        public short ReverseBytes(short i)
        {
            return (short)((i << 8) + (i >> 8));
        }
        public void WriteByte(int v)
        {
            Write((byte)v);
            _written += 1;
        }

        public void WriteShort(short value)
        {
            int val1 = (int)((uint)value >> 0) & 0xFFF;
            int val2 = (int)((uint)value >> 8) & 0xFFF;
            Write((byte)val1);
            Write((byte)val2);
            _written += 2;
        }

        public void WriteInt(int value)
        {
            int val1 = (int)((uint)value >> 0) & 0xFFF;
            int val2 = (int)((uint)value >> 8) & 0xFFF;
            int val3 = (int)((uint)value >> 16) & 0xFFF;
            int val4 = (int)((uint)value >> 24) & 0xFFF;

            Write((byte)val1);
            Write((byte)val2);
            Write((byte)val3);
            Write((byte)val4);
            _written += 4;
        }

        public void WriteChar(int value)
        {
            int val1 = (int)((uint)value >> 0) & 0xFFF;
            int val2 = (int)((uint)value >> 8) & 0xFFF;

            Write((byte)val1);
            Write((byte)val2);
            _written += 2;
        }

        public void WriteUtf(string str)
        {
            if (str == null)
            {
                WriteShort(0);
            }
            else
            {
                int strLen = str.Length;
                WriteInt(strLen);
                for (int i = 0; i < str.Length; i++)
                {
                    WriteChar(str[i]);
                }
            }
        }
    }
}