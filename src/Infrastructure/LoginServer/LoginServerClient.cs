using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
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

        
        /**
         * Start the UDP Server listener
         */
        public void StartServer()
        {
            try
            {
                Client.BeginReceive(RecivePackets, null);
            }
            catch (SocketException e)
            {
                Console.WriteLine($"Error reciving UDP packets: {e}");
            }
        }

        /**
         * Stop the UDP Server Listener
         */
        public void CloseServer()
        {
            Client.Close();
        }


        /**
         * Recive packets bytes
         */
        private void RecivePackets(IAsyncResult asyncResult)
        {
            var recivedBytes = Client.EndReceive(asyncResult, ref Server);
        }
    }
}