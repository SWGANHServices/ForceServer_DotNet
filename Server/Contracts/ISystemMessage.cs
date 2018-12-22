using System.Collections.Generic;

namespace SwgAnh.Docker.Contracts
{
    public interface ISystemMessage
    {
        void SendMessage(byte[] message);
        void SendMessage(Queue<byte[]> message);
    }
}