using System;
using System.IO;

namespace SwgAnh.Docker.Infrastructure.Packets.Reader
{
    public class SoeReader : BinaryReader
    {
        public SoeReader(Stream input) : base(input)
        {
        }

        public override Stream BaseStream => base.BaseStream;

        public override void Close()
        {
            base.Close();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override int PeekChar()
        {
            return base.PeekChar();
        }

        public override int Read()
        {
            return base.Read();
        }

        public override int Read(byte[] buffer, int index, int count)
        {
            return base.Read(buffer, index, count);
        }

        public override int Read(char[] buffer, int index, int count)
        {
            return base.Read(buffer, index, count);
        }

        public override int Read(Span<byte> buffer)
        {
            return base.Read(buffer);
        }

        public override int Read(Span<char> buffer)
        {
            return base.Read(buffer);
        }

        public override bool ReadBoolean()
        {
            return base.ReadBoolean();
        }

        public override byte ReadByte()
        {
            return base.ReadByte();
        }

        public override byte[] ReadBytes(int count)
        {
            return base.ReadBytes(count);
        }

        public override char ReadChar()
        {
            return base.ReadChar();
        }

        public override char[] ReadChars(int count)
        {
            return base.ReadChars(count);
        }

        public override decimal ReadDecimal()
        {
            return base.ReadDecimal();
        }

        public override double ReadDouble()
        {
            return base.ReadDouble();
        }

        public override short ReadInt16()
        {
            return base.ReadInt16();
        }

        public override int ReadInt32()
        {
            return base.ReadInt32();
        }

        public override long ReadInt64()
        {
            return base.ReadInt64();
        }

        public override sbyte ReadSByte()
        {
            return base.ReadSByte();
        }

        public override float ReadSingle()
        {
            return base.ReadSingle();
        }

        public override string ReadString()
        {
            return base.ReadString();
        }

        public override ushort ReadUInt16()
        {
            return base.ReadUInt16();
        }

        public override uint ReadUInt32()
        {
            return base.ReadUInt32();
        }

        public override ulong ReadUInt64()
        {
            return base.ReadUInt64();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void FillBuffer(int numBytes)
        {
            base.FillBuffer(numBytes);
        }
    }
}
