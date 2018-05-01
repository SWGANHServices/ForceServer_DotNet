using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Contracts
{
    public interface ISessionRecivedHandler
    {
        void HandleSessionRecived(SwgInputStream inputStream);
    }
}