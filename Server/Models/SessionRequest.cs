using SwgAnh.Docker.Infrastructure.Packets;
using System;
using System.IO;

namespace SwgAnh.Docker.Models
{
    public class SessionRequest : SoeBaseObject
    {
        public int CRCLength { get; set; }
        public int ClientUDPSize { get; set; }
    }
}