using SwgAnh.Docker.Infrastructure.LoginServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwgAnh.Docker.src.Infrastructure.Packets
{
    public class PacketRecivedEvents
    {
        public delegate void PacketsRecivedEventHandler(object sender, BytesRecivedEventArgs e);
    }
}
