using System.Net;
using System.Net.Sockets;

namespace Server.src.Contracts
{
    public interface IUdpClient
    {
        /// <summary>
        /// Must be called first
        /// </summary>
        byte[] Receive();
        void SendAsync(byte[] datagram, int bytes);
        void Close();

    }
}