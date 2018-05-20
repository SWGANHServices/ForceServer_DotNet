using SwgAnh.Docker.Infrastructure.SwgStream;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwgAnh.Docker.src.Contracts
{
    public interface INetStatusRequestRecived
    {
        void HandleNetStatusRequest(SwgInputStream stream);
    }
}
