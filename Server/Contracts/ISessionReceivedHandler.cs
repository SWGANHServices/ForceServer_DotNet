using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Contracts
{
    public interface ISessionReceivedHandler
    {
        void HandleSessionReceived(SwgInputStream inputStream);
    }
}