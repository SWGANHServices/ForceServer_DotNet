using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public sealed class LoginServerClient : ILoginServer
    {
        private readonly LoginEventHandler _eventHandler = new LoginEventHandler();
        private readonly ILogger _logger;
        private readonly ISoeActionFactory _soeActionFactory;
        private readonly IUdpClient _udpClient;
        private volatile bool _isRunning;

        public LoginServerClient(ILogger logger, IUdpClient udpClient,
            ISoeActionFactory soeActionFactory)
        {
            _soeActionFactory = soeActionFactory;
            _logger = logger;
            _udpClient = udpClient;
            _eventHandler.UdpPacketsRecived += TryHandleIncomingPacket;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Start Login Server
        /// </summary>
        public void StartServer()
        {
            try
            {
                _isRunning = true;
                if (!_isRunning) return;
                var listeningThread = new Thread(ListenForUdpPackets);
                listeningThread.Start();
                _logger.LogDebug("Listening for login attempt...");
            }
            catch (ThreadStateException e)
            {
                _logger.LogError($"Thread failed with error: {e}");
            }
            catch (OutOfMemoryException e)
            {
                _logger.LogError($"Server is out of memory. can't start thread with error: {e}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Starting login server failed with unknown error: {e}");
            }
        }

        /// <summary>
        ///     Stops the server and threads for Login
        /// </summary>
        public void CloseServer()
        {
            _isRunning = false;
            _eventHandler.UdpPacketsRecived -= TryHandleIncomingPacket;
            _udpClient.Close();
        }

        private void ListenForUdpPackets()
        {
            while (_isRunning)
            {
                var bytes = _udpClient.Receive();
                _eventHandler.Login(bytes);
            }
        }

        private void TryHandleIncomingPacket(object sender, LoginServerEventsArgs e)
        {
            if (e.RecivedBytes == null)
                return;
            try
            {
                using (var memStream = new MemoryStream(e.RecivedBytes))
                {
                    var swgStream = new SwgInputStream(memStream);

                    _soeActionFactory.InitiateAction(swgStream);
                }
            }
            catch (KeyNotFoundException keyException)
            {
                _logger.LogWarning($"Unknown opCode: {keyException.Message}");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not handle incoming packet: {exception}");
            }
        }
    }
}