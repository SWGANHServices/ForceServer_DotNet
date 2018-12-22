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
    public class NetStatusRequestReceived : INetStatusRequestRecived
    {
        private readonly ILogger _logger;
        private readonly ISystemMessage _systemMessage;

        public NetStatusRequestReceived(ISystemMessage systemMessage, ILogger logger)
        {
            _systemMessage = systemMessage;
            _logger = logger;
        }

        public void HandleNetStatusRequest(SwgInputStream stream)
        {
            var message = new Queue<byte[]>();
            _logger.Log($"HandleNetStatusRequest: NetStatusRequest received {stream.OpCode}");

            var formatter = new NetStatusRequestFormatter();
            var netStatusRequest = (NetStatusRequest) formatter.Deserialize(stream.BaseStream);

            GameTimeHelper.ClientTick = netStatusRequest.ClientTickCount;
            GameTimeHelper.ClientPacketSent = netStatusRequest.PacketsSent;
            GameTimeHelper.ClientPacketRecived = netStatusRequest.PacketsRecived;

            _logger.Log($"ClientTick = {GameTimeHelper.ClientTick}");
            _logger.Log($"ClientPacketSent = {GameTimeHelper.ClientPacketSent}");
            _logger.Log($"ClientPacketReceived = {GameTimeHelper.ClientPacketRecived}");

            message.Enqueue(GenerateNetStatusRequest());
            _systemMessage.SendMessage(message);
            _logger.Log($"HandleNetStatusRequest: NetStatusRequest sent {stream.OpCode}");
        }

        private static byte[] GenerateNetStatusRequest()
        {
            GameTimeHelper.ClientTick += GameTimeHelper.ServerTick;
            using (var memoryStream = new MemoryStream())
            using (var outputStream = new SwgOutputStream(memoryStream))
            {
                outputStream.Write((short) SoeOpCodes.SoeNetStatusRes);
                outputStream.Write(GameTimeHelper.ClientTick);
                outputStream.Write(GameTimeHelper.ServerTick);
                outputStream.Write(GameTimeHelper.ClientPacketSent);
                outputStream.Write(GameTimeHelper.ClientPacketRecived);
                outputStream.Write(GameTimeHelper.ServerPacketSent);
                outputStream.Write(GameTimeHelper.ServerPacketRecived);
                var buffer = memoryStream.ToArray();
                var compressed = PacketUtilities.CompressMessage(buffer);
                var encoded = PacketUtilities.EncryptMessage(compressed, compressed.Length, 0);
                var endStream = PacketUtilities.AppendCrc(encoded, encoded.Length, 0);
                return (byte[]) endStream;
            }
        }
    }
}