using System.Net;
using System.Net.Sockets;
using Server.src.Contracts;

namespace Server {
    public class UdpClient : IUdpClient {

        private const int LoginServerPort = 44453;
        private readonly System.Net.Sockets.UdpClient Client = new System.Net.Sockets.UdpClient (LoginServerPort);
        private IPEndPoint Server = new IPEndPoint (IPAddress.Any, LoginServerPort);

        public void Close () {
            Client.Close ();
        }

        public byte[] Receive () {
            return Client.Receive (ref Server);
        }

        public void SendAsync (byte[] datagram, int bytes) {
            Client.SendAsync (datagram, bytes, Server);
        }
    }
}