using static SwgAnh.Docker.Infrastructure.LoginServer.LoginEventHandler;
using static SwgAnh.Docker.Infrastructure.LoginServerClient;

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