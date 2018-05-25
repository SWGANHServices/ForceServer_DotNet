using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Models
{
    [Serializable]
    public class SessionRecived : SoeBaseObject
    {
        public override void Deserialize(SwgInputStream inputStream)
        {
            OpCode = inputStream.OpCode;
            ClientId = SetClientId(inputStream);
        }

        private static int SetClientId(BinaryReader inputStream)
        {
            inputStream.BaseStream.Position = 6;
            return inputStream.ReadInt32();
        }
    }
}