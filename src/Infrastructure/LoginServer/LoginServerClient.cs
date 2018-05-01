using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.LoginServer;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.Serialization;
using SwgAnh.Docker.Infrastructure.SwgStream;
using SwgAnh.Docker.Models;

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
        private readonly ISessionRecivedHandler _sessionRecivedHandler;
        private readonly ILogger _logger;
        private const int LoginServerPort = 44453;
        
        public LoginServerClient(ISessionRecivedHandler sessionRecivedHandler, ILogger logger)
        {
            _sessionRecivedHandler = sessionRecivedHandler;
            _logger = logger;
            eventHandler.UdpPacketsRecived += TryHandleInncommingPacket;
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Start Login Server for inncomming UDP packets
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
            catch(ThreadStateException e)
            {
                _logger.LogError($"Thread failed with error: {e}");
            }
            catch(OutOfMemoryException e)
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
                var bytes = Client.Receive(ref Server);
                eventHandler.Login(bytes);
            }
        }

        /**
         *  This is where we will try to login to the client.!--
         *  Validation against database etc.
         */
        protected virtual void TryHandleInncommingPacket(object sender, BytesRecivedEventArgs e)
        {
            if (e.RecivedBytes == null) {
                return;
            }
            using (var memStream = new MemoryStream(e.RecivedBytes)) {
                var swgStream = new SwgInputStream(memStream);
                switch (swgStream.OpCode)
                {
                    case (short)SoeOpCodes.Ping:
                        _logger.LogDebug("Ping recived.");
                        break;
                    case (short) SoeOpCodes.SoeSessionRequest:
                        _logger.LogDebug($"{nameof(SoeOpCodes.SoeSessionRequest)} recived");
                        _sessionRecivedHandler.HandleSessionRecived(swgStream);
                        break;
                    default:
                        _logger.LogDebug("Uknown OPCode recived");
                        break;
                }
            }
        }

        /// <summary>
        /// Stops the server and threads for Login
        /// </summary>
        public void CloseServer()
        {
            IsRunning = false;
            eventHandler.UdpPacketsRecived -= TryHandleInncommingPacket;
            Client.Close();
        }

    }
}