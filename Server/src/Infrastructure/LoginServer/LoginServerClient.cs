using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Server.src.Contracts;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.LoginServer;
using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Infrastructure
{
    public class LoginServerClient : ILoginServer
    {
        private LoginEventHandler eventHandler = new LoginEventHandler();
        private volatile bool IsRunning;
        private readonly ISessionRecivedHandler _sessionRecivedHandler;
        private readonly ISystemMessage _systemMessage;
        private readonly ILogger _logger;
        private readonly IUdpClient _udpClient;
        private readonly ISoeActionFactory _soeActionFactory;

        public LoginServerClient(ISessionRecivedHandler sessionRecivedHandler,
            ISystemMessage systemMessage,
            ILogger logger, IUdpClient udpClient,
            ISoeActionFactory soeActionFactory)
        {
            _soeActionFactory = soeActionFactory;
            _sessionRecivedHandler = sessionRecivedHandler;
            _systemMessage = systemMessage;
            _logger = logger;
            _udpClient = udpClient;
            eventHandler.UdpPacketsRecived += TryHandleInncommingPacket;
        }

        /// <inheritdoc />
        /// <summary>
        /// Start Login Server 
        /// </summary>
        public void StartServer()
        {
            try
            {
                IsRunning = true;
                if (!IsRunning) return;
                var listeningThread = new Thread(ListenForUdpPackets);
                listeningThread.Start();
                _logger.LogDebug("Listening for login attemps...");
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
                _logger.LogError($"Starting login server failed with unkown error: {e}");
            }
        }

        private void ListenForUdpPackets()
        {
            while (IsRunning)
            {
                var bytes = _udpClient.Receive();
                eventHandler.Login(bytes);
            }
        }

        protected virtual void TryHandleInncommingPacket(object sender, BytesRecivedEventArgs e)
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
                _logger.LogError($"Could not handle inncomming packet: {exception}");
            }
        }

        /// <summary>
        /// Stops the server and threads for Login
        /// </summary>
        public void CloseServer()
        {
            IsRunning = false;
            eventHandler.UdpPacketsRecived -= TryHandleInncommingPacket;
            _udpClient.Close();
        }

    }
}