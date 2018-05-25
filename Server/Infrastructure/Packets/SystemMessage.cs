using System.Collections.Generic;
using System.Threading;
using Server.src.Contracts;
using SwgAnh.Docker.Contracts;

namespace SwgAnh.Docker.Infrastructure.Packets {
    public class SystemMessage : ISystemMessage {
        private readonly IUdpClient _udpClient;

        public SystemMessage (IUdpClient udpClient) {
            _udpClient = udpClient;
        }
        
        public void SendMessage(byte[] message)
        {
            _udpClient.SendAsync(message, message.Length);
        }

        public void SendMessage(Queue<byte[]> message)
        {
            var count = message.Count;
            for (var i = 0; i < count; i++) {
                var item = message.Dequeue ();
                _udpClient.SendAsync (item, item.Length);
                Thread.Sleep(500); // See if this helps with crash
            }
        }
    }
}