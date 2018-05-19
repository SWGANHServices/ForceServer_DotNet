using System.Collections.Generic;

namespace Server.src.Contracts {
    public interface ISystemMessage {
        void SendMessage (Queue<byte[]> message);
    }
}