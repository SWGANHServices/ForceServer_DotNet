using SwgAnh.Docker.Infrastructure.Packets.Reader;
using SwgAnh.Docker.Models;
using System;
using System.IO;

namespace SwgAnh.Docker.Infrastructure.Packets.Formatter
{
    public class SessionRequestFormatter : SoeFormatterBase
    {

        public override object Deserialize(Stream serializationStream)
        {
            var target = new SessionRequest();

            using (SoeReader = new SoeReader(serializationStream))
            {
                target.ClientId = SetClientId();
                target.OpCode = SetOpCode();
                target.CRCLength = SetCRCLength();
                target.ClientUDPSize = SetUDPSize();
            }
            return target;
        }
        
        // TODO: Serialize back to bytes.
        public override void Serialize(Stream serializationStream, object graph)
        {
            throw new NotImplementedException();
        }


        private int SetCRCLength()
        {
            SoeReader.BaseStream.Position = sizeof(short);
            return SoeReader.ReadInt32();
        }

        private int SetClientId()
        {
            SoeReader.BaseStream.Position = sizeof(int) + sizeof(short);
            return SoeReader.ReadInt32();
        }
        
        private int SetUDPSize()
        {
            SoeReader.BaseStream.Position = sizeof(short) + (sizeof(int) * 2);
            return SoeReader.ReadInt32();
        }
    }
}
