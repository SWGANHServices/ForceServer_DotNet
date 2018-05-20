using static SwgAnh.Docker.src.Infrastructure.Packets.PacketRecivedEvents;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class LoginEventHandler
    {
        public event PacketsRecivedEventHandler UdpPacketsRecived;
        
        public void Login(byte[] bytes)
        {
            var eventArgs = new BytesRecivedEventArgs 
            {
                RecivedBytes = bytes
            };
            OnUdpPacketsRecived(eventArgs);
        }


        protected virtual void OnUdpPacketsRecived(BytesRecivedEventArgs e) {
            UdpPacketsRecived?.Invoke(this, e);
        }
    }
}