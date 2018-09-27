using System.Collections.Generic;
using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Contracts
{
    public interface IChlDataRecived
    {
        void ChlDataARecived(SwgInputStream inputStream);
    }
}
