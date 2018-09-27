using System;
using System.Collections.Generic;
using System.IO;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Encryption;
using SwgAnh.Docker.Helper;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.Packets.Formatter;
using SwgAnh.Docker.Infrastructure.SwgStream;
using SwgAnh.Docker.Models;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class NetStatusRequestRecived : INetStatusRequestRecived
    {
        private readonly ISystemMessage _systemMessage;
        private readonly ILogger _logger;

        public NetStatusRequestRecived(ISystemMessage systemMessage, ILogger logger)
        {
            _systemMessage = systemMessage;
            _logger = logger;
        }

        public void HandleNetStatusRequest(SwgInputStream stream)
        {
            
            var message = new Queue<byte[]>();
            _logger.Log($"HandleNetStatusRequest: NetStatusRequest recived {stream.OpCode}");

            var formatter = new NetStatusRequestFormatter();
            var netStatusRequest = (NetStatusRequest)formatter.Deserialize(stream.BaseStream);

            GameTimeHelper.ClientTick = netStatusRequest.ClientTickCount;
            GameTimeHelper.ClientPacketSent = netStatusRequest.PacketsSent;
            GameTimeHelper.ClientPacketRecived = netStatusRequest.PacketsRecived;

            _logger.Log($"ClientTick = {GameTimeHelper.ClientTick}");
            _logger.Log($"ClientPacketSent = {GameTimeHelper.ClientPacketSent}");
            _logger.Log($"ClientPacketRecived = {GameTimeHelper.ClientPacketRecived}");

            message.Enqueue(GenerateNetStatusRequest());
            _systemMessage.SendMessage(message);
            _logger.Log($"HandleNetStatusRequest: NetStatusRequest sendt {stream.OpCode}");
            _logger.LogDebug($"HandleNetStatusRequest: StreamData -> {stream.ToString()}");
        }

        private static byte[] GenerateNetStatusRequest()
        {
            GameTimeHelper.ClientTick += GameTimeHelper.ServerTick;
            using (var memoryStream = new MemoryStream())
            using (var outputStream = new SwgOutputStream(memoryStream))
            {
                outputStream.WriteShort((short) SoeOpCodes.SoeNetStatusRes);
                outputStream.((short) GameTimeHelper.ClientTick); // Client Tick (TODO: Implement client tick)
                outputStream.ReverseBytes((short) GameTimeHelper.ServerTick); // Tick Count (TODO: Implement tick count)
                outputStream.ReverseBytes((short) GameTimeHelper.ClientPacketSent); // ClientPacketsSent (TODO: Implement tick count)
                outputStream.ReverseBytes((short) GameTimeHelper.ClientPacketRecived); // ClientPacketsReceived (TODO: Implement tick count)
                outputStream.ReverseBytes((short) GameTimeHelper.ServerPacketSent); // ClientPacketsReceived (TODO: Implement tick count)
                outputStream.ReverseBytes((short) GameTimeHelper.ServerPacketRecived); // serverPacketsReceivedThisClient (TODO: Implement)
                var buffer = memoryStream.ToArray();
                var compressed = PacketUtilities.CompressMessage(buffer);
                var encoded = PacketUtilities.EncryptMessage(compressed, compressed.Length, 0);
                return encoded;
            }
        }
    }
}
