using System.Collections.Generic;
using System.IO;
using System.Text;
using SwgAnh.Docker.Constant;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.Packets.Formatter;
using SwgAnh.Docker.Infrastructure.SwgStream;
using SwgAnh.Docker.Models;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class SessionRecivedHandler : ISessionRecivedHandler
    {

        private readonly ISystemMessage _systemMessage;
        private readonly ILogger _logger;

        public SessionRecivedHandler(ISystemMessage systemMessage, ILogger logger)
        {
            _systemMessage = systemMessage;
            _logger = logger;
        }

        public Queue<byte[]> HandleSessionRecived(SwgInputStream baseObject)
        {
            _logger.Log($"HandleSessionRecived: Session Recived: {baseObject.OpCode}");
            var queueList = new Queue<byte[]>();
            var formatter = new SessionRequestFormatter();
            var sessionRequest = (SessionRequest) formatter.Deserialize(baseObject.BaseStream);
            QueueServerSessionResponse(queueList, sessionRequest);
            QueueLoginServerResponse(queueList);
            _systemMessage.SendMessage(queueList);
            _logger.Log($"HandleSessionRecived: Session Done {baseObject.OpCode}");
            _logger.LogDebug($"HandleSessionRecived: Stream data -> {baseObject.BaseStream}");
            return queueList;
        }

        private void QueueLoginServerResponse(Queue<byte[]> queueList)
        {
            using (var stream = new MemoryStream())
            {
                using (var output = new SwgOutputStream(stream))
                {
                    output.WriteShort((short)SoeOpCodes.SoeChlDataA);
                    output.WriteShort(0);
                    output.WriteShort((short)UpdateCodes.WorldUpdate);
                    output.WriteInt(Constants.LoginServer.LoginServerString);
                    output.WriteUtf(Constants.LoginServer.LoginServerInfo);
                    stream.Position = 0;
                    queueList.Enqueue(stream.ToArray());
                }
            }

            using (var stream = new MemoryStream())
            {
                using (var output = new SwgOutputStream(stream))
                {
                    output.SetOpCode((short)SoeOpCodes.SoeChlDataA);
                    output.SetSequence(0);
                    output.WriteShort((short)UpdateCodes.WorldUpdate);
                    output.WriteInt(Constants.LoginServer.LoginServerId);
                    output.WriteInt(29411);
                    stream.Position = 0;
                    queueList.Enqueue(stream.ToArray());
                }
            }
        }

        private static void QueueServerSessionResponse(Queue<byte[]> queueList, SoeBaseObject sessionRecived)
        {
            using (var stream = new MemoryStream())
            {
                using (var output = new SwgOutputStream(stream))
                {
                    output.SetOpCode((short)SoeOpCodes.SoeSessionResponse); // OPCode
                    output.WriteInt(sessionRecived.ClientId); // Client Id
                    output.ReverseBytes(sessionRecived.CsrSeed); // CsrSeed
                    output.WriteByte(2); // CsrLength
                    output.WriteByte(1); // Use compression
                    output.WriteByte(4); // SeedSize
                    output.WriteInt(Constants.LoginServer.MaxPacketSize); // Server UDP Size
                    stream.Position = 0;
                    var byterray = stream.ToArray();
                    queueList.Enqueue(byterray);
                }
            }
        }
    }
}