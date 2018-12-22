using System.IO;
using SwgAnh.Docker.Infrastructure.Packets.Reader;
using SwgAnh.Docker.Models;

namespace SwgAnh.Docker.Infrastructure.Packets.Formatter
{
    public class NetStatusRequestFormatter : SoeFormatterBase
    {
        public override object Deserialize(Stream serializationStream)
        {
            var target = new NetStatusRequest();
            using (SoeReader = new SoeReader(serializationStream))
            {
                target.OpCode = SetOpCode();
                target.ClientTickCount = SetClientTickCount();
                target.LastUpdate = SetLastUpdate();
                target.AvarageUpdate = SetAvarageUpdate();
                target.ShortestUpdate = SetShortestUpdate();
                target.LongestUpdate = SetLongestUpdate();
                target.LastServerUpdate = SetLastServerUpdate();
                target.PacketsSent = SetPacketsSent();
                target.PacketsRecived = SetPacketsReceived();
            }

            return target;
        }

        private int SetLastServerUpdate() => SoeReader.ReadInt32();

        private int SetLongestUpdate() => SoeReader.ReadInt32();

        private int SetShortestUpdate() => SoeReader.ReadInt32();

        private int SetAvarageUpdate() => SoeReader.ReadInt32();

        private int SetLastUpdate() => SoeReader.ReadInt32();

        public override void Serialize(Stream serializationStream, object graph)
        {
        }

        private ushort SetClientTickCount()
        {
            SoeReader.BaseStream.Position = sizeof(short);
            return SoeReader.ReadUInt16();
        }

        private long SetPacketsSent() => SoeReader.ReadInt64();

        private long SetPacketsReceived() => SoeReader.ReadInt64();
    }
}