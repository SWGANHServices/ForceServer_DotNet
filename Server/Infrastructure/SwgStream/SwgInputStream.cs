using System.IO;
using SwgAnh.Docker.Infrastructure.Packets;

namespace SwgAnh.Docker.Infrastructure.SwgStream
{
    public class SwgInputStream : BinaryReader
    {

        public short OpCode { get; }
        public short Sequence { get; set; }
        public short UpdateType { get; }

        public SwgInputStream(Stream stream) : base(stream)
        {
            var inncommingData = new byte[stream.Length];
            BaseStream.Read(inncommingData, 0, inncommingData.Length);
            BaseStream.Seek(0, SeekOrigin.Begin);
            OpCode = ReadInt16();

            if (OpCode == (short)SoeOpCodes.SoeChlDataA
                || OpCode == (short)SoeOpCodes.SoeDataFragA
                || OpCode == (short)SoeOpCodes.SoeAckA
                || OpCode == (short)SoeOpCodes.SoeOutOrderPktA)
            {
                Sequence = ReverseBytes();
                if (OpCode == (short)SoeOpCodes.SoeChlDataA)
                {
                    UpdateType = ReadInt16();
                }
                else
                {
                    UpdateType = -1;
                }
            }
            else
            {
                Sequence = -1;
            }
        }

        public short GetClientTick()
        {
            BaseStream.Position = 0;
            BaseStream.Position = ReadInt16();
            return ReverseBytes();
        }
        public int ClientPacketsSent()
        {
            BaseStream.Position = 0;
            BaseStream.Position = Read();
            BaseStream.Position = Read();
            BaseStream.Position = Read();
            BaseStream.Position = Read();
            BaseStream.Position = Read();
            return ReadInt32();
        }
        public int ClientPacketsRecived()
        {
            BaseStream.Position = 0;
            BaseStream.Position = ReadInt32();
            BaseStream.Position = ReadInt32();
            BaseStream.Position = ReadInt32();
            BaseStream.Position = ReadInt32();
            BaseStream.Position = ReadInt32();
            return ReadInt32();
        }

        private short ReverseBytes()
        {
            var i = ReadInt16();
            return (short)((i << 8) + (i >> 8));
        }

        public sealed override short ReadInt16()
        {
            var ch1 = BaseStream.ReadByte();
            var ch2 = BaseStream.ReadByte();
            if ((ch1 | ch2) < 0)
                throw new EndOfStreamException();
            return (short)((ch2 << 8) + (ch1 << 0));
        }
    }
}