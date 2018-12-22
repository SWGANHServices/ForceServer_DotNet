using SwgAnh.Docker.Infrastructure.LoginServer;

namespace SwgAnh.Docker.Infrastructure.Packets
{
    public class PacketRecivedEvents
    {
        public delegate void PacketsRecivedEventHandler(object sender, LoginServerEventsArgs e);
    }
}