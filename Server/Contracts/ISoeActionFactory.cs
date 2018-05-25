using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Contracts
{
    public interface ISoeActionFactory
    {
        void InitiateAction (SwgInputStream stream);
    }
}