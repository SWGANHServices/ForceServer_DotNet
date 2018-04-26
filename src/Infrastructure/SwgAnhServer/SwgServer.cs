using System;
using SwgAnh.Docker.Contracts;

namespace SwgAnh.Docker.Infrastructure.SwgAnhServer
{
    public class SwgServer : ISwgServer
    {
        private readonly ILoginServer _loginServer;

        public SwgServer(ILoginServer loginServer)
        {
            _loginServer = loginServer;
        }
        public void Run()
        {
            Console.WriteLine("Starting up SWG Server...");
            
            _loginServer.StartServer();
        }
    }
}