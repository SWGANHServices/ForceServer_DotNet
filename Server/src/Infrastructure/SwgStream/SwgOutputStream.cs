using System;
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

        public MemoryStream GenerateCrCSeed(byte[] stream, int crcSeed)
        {
            short nCrcLength = 2;
            var length = (short)(stream.Length - nCrcLength);
            int arrayIndex = (~crcSeed) & 0xFF;
            uint nCrc = Constants.Constants.LoginServer.CrcTable[arrayIndex];
            nCrc ^= 0x00FFFFFF;
            int nIndex = (int)((crcSeed >> 8) ^ nCrc);
            nCrc = (nCrc >> 8) & 0x00FFFFFF;
            nCrc ^= Constants.Constants.LoginServer.CrcTable[(nIndex) & 0xFF];
            nIndex = (int)((crcSeed >> 16) ^ nCrc);
            nCrc = (nCrc >> 8) & 0x00FFFFFF;
            nCrc ^= Constants.Constants.LoginServer.CrcTable[(nIndex) & 0xFF];
            nIndex = (int)((crcSeed >> 24) ^ nCrc);
            nCrc = (nCrc >> 8) & 0x00FFFFFF;
            nCrc ^= Constants.Constants.LoginServer.CrcTable[(nIndex) & 0xFF];
            for (int i = 0; i < length; i++)
            {

                nIndex = (int)(stream[i] ^ nCrc);
                nCrc = (nCrc >> 8) & 0x00FFFFFF;
                nCrc ^= Constants.Constants.LoginServer.CrcTable[nIndex & 0xFF];
            }
            var crc = ~nCrc;
            byte[] newOutput = stream;
            for (short i = 0; i < nCrcLength; i++)
            {
                newOutput[(length - 1) - i] = (byte)((crc >> (8 * i)) & 0xFF);
            }
            var newStream = new MemoryStream(newOutput);
            newStream.Write(newOutput);
            return newStream;
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