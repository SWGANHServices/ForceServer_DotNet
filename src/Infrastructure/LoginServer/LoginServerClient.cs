using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using SwgAnh.Docker.Contracts;

namespace SwgAnh.Docker.Infrastructure
{
    /**
     *    LoginServerClient.cs
     * --------------------------
     *     Handles the login packets from the SWG client.
     *     UDP Packet server for handling Login attemps.
     *     TODO: Should htis be its own server? Could be
     */
    public class LoginServerClient : ILoginServer
    {
        private readonly UdpClient Client = new UdpClient(LoginServerPort);
        private IPEndPoint  Server = new IPEndPoint(IPAddress.Any, LoginServerPort);
        private const int LoginServerPort = 2000;
        private const int PacketTimeOut = 300000;
        private const int MaxPacketSize = 496;
        private const int MaxPacketSizeBeforeCompression = 149;
        private volatile bool IsRunning = false;
        
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
            }
            catch (SocketException e)
            {
                Console.WriteLine($"Error reciving UDP packets: {e}");
            }
        }

        private void ListenForUdpPackets()
        {
            while (IsRunning)
            {
                Client.Receive(ref Server);
            }
        }

        /**
         * Stop the UDP Server Listener
         */
        public void CloseServer()
        {
            IsRunning = false;
            Client.Close();
        }


        /**
         * Recive packets bytes
         */
        private void RecivePackets(IAsyncResult asyncResult)
        {
            var recivedBytes = Client.EndReceive(asyncResult, ref Server);
            // Send Event or something with bytes?
        }
    }
}