using static SwgAnh.Docker.Infrastructure.LoginServerClient;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class LoginEventHandler
    {
        public event ThresholdReachedEventHandler UdpPacketsRecived;
        
        public void Login(byte[] bytes)
        {
            var eventArgs = new BytesRecivedEventArgs 
            {
                RecivedBytes = bytes
            };
            OnUdpPacketsRecived(eventArgs);
        }


        protected virtual void OnUdpPacketsRecived(BytesRecivedEventArgs e) {
            ThresholdReachedEventHandler handler = UdpPacketsRecived;
            if (handler != null) {
                handler(this, e);
            }
        }
    }
}