using System.Net;
using SwgAnh.Docker.Contracts;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class UdpClient : IUdpClient
    {

        private const int LoginServerPort = 44453;
        private readonly System.Net.Sockets.UdpClient _client = new System.Net.Sockets.UdpClient(LoginServerPort);
        private IPEndPoint _server = new IPEndPoint(IPAddress.Any, LoginServerPort);

        public void Close()
        {
            _client.Close();
        }

        public byte[] Receive()
        {
            return _client.Receive(ref _server);
        }

        public void SendAsync(byte[] datagram, int bytes)
        {
            _client.SendAsync(datagram, bytes, _server);
        }
    }
}