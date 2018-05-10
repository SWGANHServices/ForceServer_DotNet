using System.IO;

namespace SwgAnh.Docker.Infrastructure.SwgStream
{
    public class SwgOutputStream : BinaryWriter
    {
        private int _written;
        private short _opCode;
        private short _sequence;
        private short _updateType;

        public SwgOutputStream(Stream stream) : base(stream)
        {
        }

        public void SetOpCode(short value)
        {
            Write(value);
            _opCode = value;
        }

        public override void Write(short value)
        {
            Write((value >> 0 ) & 0xFFF);
            Write((value >> 8 ) & 0xFFF);
            _written++;
        }
    }
}