using static SwgAnh.Docker.Infrastructure.Packets.PacketRecivedEvents;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class LoginEventHandler
    {
        public event PacketsRecivedEventHandler UdpPacketsRecived;

        public void Login(byte[] bytes)
        {
            var eventArgs = new LoginServerEventsArgs
            {
                RecivedBytes = bytes
            };
            OnUdpPacketsRecived(eventArgs);
        }


        protected virtual void OnUdpPacketsRecived(LoginServerEventsArgs e)
        {
            UdpPacketsRecived?.Invoke(this, e);
        }
    }
}