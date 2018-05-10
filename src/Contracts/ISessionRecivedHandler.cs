using System.Collections.Generic;
using System.Net.Sockets;
using SwgAnh.Docker.Infrastructure.SwgStream;
using SwgAnh.Docker.Models;

namespace SwgAnh.Docker.Contracts
{
    public interface ISessionRecivedHandler
    {
        Queue<byte[]> HandleSessionRecived(SwgInputStream inputStream);
    }
}