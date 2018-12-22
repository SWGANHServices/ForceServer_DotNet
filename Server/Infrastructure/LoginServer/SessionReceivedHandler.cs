using System.Collections.Generic;
using System.IO;
using SwgAnh.Docker.Constant;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.Packets.Formatter;
using SwgAnh.Docker.Infrastructure.SwgStream;
using SwgAnh.Docker.Models;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class SessionReceivedHandler : ISessionReceivedHandler
    {
        private readonly ILogger _logger;

        private readonly ISystemMessage _systemMessage;

        public SessionReceivedHandler(ISystemMessage systemMessage, ILogger logger)
        {
            _systemMessage = systemMessage;
            _logger = logger;
        }

        public void HandleSessionReceived(SwgInputStream baseObject)
        {
            _logger.Log($"HandleSessionReceived: Session Received: {baseObject.OpCode}");
            var queueList = new Queue<byte[]>();
            var formatter = new SessionRequestFormatter();
            var sessionRequest = (SessionRequest) formatter.Deserialize(baseObject.BaseStream);
            QueueServerSessionResponse(queueList, sessionRequest);
            QueueLoginServerResponse(queueList);
            _systemMessage.SendMessage(queueList);
            _logger.Log($"HandleSessionReceived: Session Done {baseObject.OpCode}");
            _logger.LogDebug($"HandleSessionReceived: Stream data -> {baseObject.BaseStream}");
        }

        private static void QueueLoginServerResponse(Queue<byte[]> queueList)
        {
            using (var stream = new MemoryStream())
            {
                using (var output = new SwgOutputStream(stream))
                {
                    output.Write((short) SoeOpCodes.SoeChlDataA);
                    output.Write((short) 0);
                    output.Write((short) UpdateCodes.WorldUpdate);
                    output.Write(Constants.LoginServer.LoginServerString);
                    output.Write(Constants.LoginServer.LoginServerInfo);
                    stream.Position = 0;
                    queueList.Enqueue(stream.ToArray());
                }
            }

            using (var stream = new MemoryStream())
            {
                using (var output = new SwgOutputStream(stream))
                {
                    output.Write((short) SoeOpCodes.SoeChlDataA);
                    output.Write((short) 0);
                    output.Write((short) UpdateCodes.WorldUpdate);
                    output.Write(Constants.LoginServer.LoginServerId);
                    output.Write(29411);
                    stream.Position = 0;
                    queueList.Enqueue(stream.ToArray());
                }
            }
        }

        private static void QueueServerSessionResponse(Queue<byte[]> queueList, SoeBaseObject sessionReceived)
        {
            using (var stream = new MemoryStream())
            {
                using (var output = new SwgOutputStream(stream))
                {
                    output.Write((short) SoeOpCodes.SoeSessionResponse); // OPCode
                    output.Write(sessionReceived.ClientId); // Client Id
                    output.Write((uint) sessionReceived.CsrSeed); // CsrSeed
                    output.Write((byte) 2); // CsrLength
                    output.Write((byte) 1); // Use compression
                    output.Write((byte) 4); // SeedSize
                    output.Write((uint) Constants.LoginServer.MaxPacketSize); // Server UDP Size
                    stream.Position = 0;
                    var bytearray = stream.ToArray();
                    queueList.Enqueue(bytearray);
                }
            }
        }
    }
}