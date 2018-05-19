using System.Collections.Generic;
using Server.src.Contracts;

namespace Server.src.Infrastructure {
    public class SystemMessage : ISystemMessage {
        private readonly IUdpClient _udpClient;

        public SystemMessage (IUdpClient udpClient) {
            _udpClient = udpClient;
        }

        public void SendMessage(Queue<byte[]> message)
        {
            var count = message.Count;
            for (int i = 0; i < count; i++) {
                var item = message.Dequeue ();
                _udpClient.SendAsync (item, item.Length);
            }
        }
    }
}