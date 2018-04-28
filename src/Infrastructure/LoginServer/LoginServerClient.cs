using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.src.Infrastructure.LoginServer;

namespace SwgAnh.Docker.Infrastructure
{
    /**
     *     LoginServerClient.cs
     * 
     *     Handles the login packets from the SWG client.
     *     UDP Packet server for handling Login attemps.
     */
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LoginServerClient : ILoginServer
    {
        public delegate void ThresholdReachedEventHandler(object sender, BytesRecivedEventArgs e);
        private readonly UdpClient Client = new UdpClient(LoginServerPort);
        private IPEndPoint  Server = new IPEndPoint(IPAddress.Any, LoginServerPort);
        private LoginEventHandler eventHandler = new LoginEventHandler();
        private volatile bool IsRunning;
        private readonly ILogger _logger;
        private const int LoginServerPort = 2000;
        
        public LoginServerClient(ILogger logger)
        {
            _logger = logger;
            eventHandler.UdpPacketsRecived += TryLogin;
        }
        
        /**
         * Start the UDP Server listener
         */
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
            catch (SocketException e)
            {
                _logger.LogError($"Starting login server failed with error: {e}");
            }
        }

        private void ListenForUdpPackets()
        {
            while (IsRunning)
            {
                var bytes = Client.Receive(ref Server);
                eventHandler.Login(bytes);
            }
        }

        /**
         *  This is where we will try to login to the client.!--
         *  Validation against database etc.
         */
        protected virtual void TryLogin(object sender, BytesRecivedEventArgs e)
        {
            
        }

        /**
         * Stop the UDP Server Listener
         */
        public void CloseServer()
        {
            IsRunning = false;
            eventHandler.UdpPacketsRecived -= TryLogin;
            Client.Close();
        }

    }
}