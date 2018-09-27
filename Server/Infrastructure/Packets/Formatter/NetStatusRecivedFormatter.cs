using SwgAnh.Docker.Infrastructure.Packets.Reader;
using SwgAnh.Docker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
                target.PacketsSent = SetPacketsSent();
                target.PacketsRecived = SetPacketsRecived();
            }
            return target;
        }
        
        public override void Serialize(Stream serializationStream, object graph)
        {
        }

        private short SetClientTickCount()
        {
            SoeReader.BaseStream.Position = sizeof(short);
            return SoeReader.ReadInt16();
        }

        private long SetPacketsSent()
        {
            SoeReader.BaseStream.Position = (sizeof(short) * 2) + (sizeof(int) * 5);
            return SoeReader.ReadInt64();
        }

        private long SetPacketsRecived()
        {
            SoeReader.BaseStream.Position = (sizeof(short) * 2) + (sizeof(int) * 5) + sizeof(long);
            return SoeReader.ReadInt64();
        }

        
    }
}
