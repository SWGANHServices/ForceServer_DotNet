using System.IO;
using SwgAnh.Docker.Infrastructure.Packets;

namespace SwgAnh.Docker.Infrastructure.SwgStream
{
    public sealed class SwgInputStream : BinaryReader
    {
        public SwgInputStream(Stream stream) : base(stream)
        {
            var incomingData = new byte[stream.Length];
            BaseStream.Read(incomingData, 0, incomingData.Length);
            BaseStream.Position = 0;
            OpCode = ReadInt16();

            if (OpCode == (short) SoeOpCodes.SoeChlDataA
                || OpCode == (short) SoeOpCodes.SoeDataFragA
                || OpCode == (short) SoeOpCodes.SoeAckA
                || OpCode == (short) SoeOpCodes.SoeOutOrderPktA)
            {
                Sequence = ReadInt16();
                if (OpCode == (short) SoeOpCodes.SoeChlDataA)
                    UpdateType = ReadInt16();
                else
                    UpdateType = -1;
            }
            else
            {
                Sequence = -1;
            }
        }

        public short OpCode { get; }
        public short Sequence { get; }
        public short UpdateType { get; }
    }
}