using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Models
{
    [Serializable]
    public abstract class SoeBaseObject
    {
        public int ClientId { get; set; }
        public short OpCode { get; set; }
        public short CsrSeed { get; set; }
        public int MaxPacketSize { get; set; }
        
    }
}