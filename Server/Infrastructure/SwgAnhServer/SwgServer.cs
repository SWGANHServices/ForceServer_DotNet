using SwgAnh.Docker.Contracts;

namespace SwgAnh.Docker.Infrastructure.SwgAnhServer
{
    public class SwgServer : ISwgServer
    {
        private readonly ILogger _logger;
        private readonly ILoginServer _loginServer;

        public SwgServer(ILoginServer loginServer, ILogger logger)
        {
            _loginServer = loginServer;
            _logger = logger;
        }

        /// <summary>
        ///     Begin All server components
        /// </summary>
        public void Run()
        {
            _logger.Log("Starting Server...");
            _loginServer.StartServer();
            _logger.Log("Starting Server... DONE");
        }
    }
}