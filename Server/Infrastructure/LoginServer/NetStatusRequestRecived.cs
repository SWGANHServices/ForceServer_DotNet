using System;
using System.Collections.Generic;
using System.IO;
using Server.src.Contracts;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.SwgStream;
using SwgAnh.Docker.src.Contracts;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class NetStatusRequestRecived : INetStatusRequestRecived
    {
        private readonly ISystemMessage _systemMessage;

        public NetStatusRequestRecived(ISystemMessage systemMessage)
        {
            _systemMessage = systemMessage;
        }

        public void HandleNetStatusRequest(SwgInputStream stream)
        {
            var message = new Queue<byte[]>();
            message.Enqueue(GenerateNetStatusRequest());
            _systemMessage.SendMessage(message);
        }

        private static byte[] GenerateNetStatusRequest()
        {
            using (var memoryStream = new MemoryStream())
            using (var outputStream = new SwgOutputStream(memoryStream))
            {
                outputStream.WriteShort((short) SoeOpCodes.SoeNetStatusRes);
                outputStream.ReverseBytes(0); // Client Tick (TODO: Implement client tick)
                outputStream.ReverseBytes(0); // Tick Count (TODO: Implement tick count)
                outputStream.ReverseBytes(0); // ClientPacketsSent (TODO: Implement tick count)
                outputStream.ReverseBytes(0); // ClientPacketsReceived (TODO: Implement tick count)
                outputStream.ReverseBytes(0); // ClientPacketsReceived (TODO: Implement tick count)
                outputStream.ReverseBytes(0); // serverPacketsReceivedThisClient (TODO: Implement)
                return memoryStream.ToArray();
            }
        }
    }
}
