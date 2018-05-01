using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class SessionRecivedHandler : ISessionRecivedHandler
    {
        public void HandleSessionRecived(SwgInputStream inputStream)
        {
            var clientId = inputStream.ReadInt32();
        }
    }
}