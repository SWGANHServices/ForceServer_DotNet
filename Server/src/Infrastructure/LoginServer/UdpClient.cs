using System.Net;
using System.Net.Sockets;
using Server.src.Contracts;
using SwgAnh.Docker.Contracts;

namespace Server
{
    public class UdpClient : IUdpClient
    {

        private const int LoginServerPort = 44453;
        private readonly System.Net.Sockets.UdpClient Client = new System.Net.Sockets.UdpClient(LoginServerPort);
        private IPEndPoint _server = new IPEndPoint(IPAddress.Any, LoginServerPort);

        public void Close()
        {
            Client.Close();
        }

        public byte[] Receive()
        {
            return Client.Receive(ref _server);
        }

        public void SendAsync(byte[] datagram, int bytes)
        {
            Client.SendAsync(datagram, bytes, _server);
        }
    }
}