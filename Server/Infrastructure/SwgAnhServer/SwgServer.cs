using System;
using SwgAnh.Docker.Contracts;

namespace SwgAnh.Docker.Infrastructure.SwgAnhServer
{
    public class SwgServer : ISwgServer
    {
        private readonly ILoginServer _loginServer;
        private readonly ILogger _logger;

        public SwgServer(ILoginServer loginServer, ILogger logger)
        {
            _loginServer = loginServer;
            _logger = logger;
        }

        /// <summary>
        /// Begin All server components
        /// </summary>
        public void Run()
        {
            _logger.Log("Starting Server...");
            _loginServer.StartServer();
            _logger.Log("Starting Server... DONE");
        }
    }
}