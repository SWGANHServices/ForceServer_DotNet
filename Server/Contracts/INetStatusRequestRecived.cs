using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Contracts
{
    public interface INetStatusRequestRecived
    {
        void HandleNetStatusRequest(SwgInputStream stream);
    }
}