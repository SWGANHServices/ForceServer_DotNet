using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Contracts
{
    public interface IChlDataRecived
    {
        void ChlDataAReceived(SwgInputStream inputStream);
    }
}