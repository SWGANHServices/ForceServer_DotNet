using System.Collections.Generic;
using System.IO;
using System.Text;
using Server.src.Contracts;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.SwgStream;
using SwgAnh.Docker.Models;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class SessionRecivedHandler : ISessionRecivedHandler
    {

        private readonly ISystemMessage _systemMessage;

        public SessionRecivedHandler(ISystemMessage systemMessage)
        {
            _systemMessage = systemMessage;
        }

        public Queue<byte[]> HandleSessionRecived(SwgInputStream baseObject)
        {
            var queueList = new Queue<byte[]>();
            var sessionRecived = new SessionRecived();
            sessionRecived.Deserialize(baseObject);
            QueueServerSessionResponse(queueList, sessionRecived);
            QueueLoginServerResponse(queueList);
            _systemMessage.SendMessage(queueList);
            return queueList;
        }

        private void QueueLoginServerResponse(Queue<byte[]> queueList)
        {
            var encoding = Encoding.UTF8;
            using (var stream = new MemoryStream())
            {
                using (var output = new SwgOutputStream(stream))
                {
                    output.WriteShort((short)SoeOpCodes.SoeChlDataA);
                    output.WriteShort(0);
                    output.WriteShort((short)SoeOpCodes.WORLD_UPDATE);
                    output.WriteInt(Constants.Constants.LoginServer.LoginServerString);
                    output.WriteUtf(Constants.Constants.LoginServer.LoginServerInfo);
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
                    output.WriteShort((short)SoeOpCodes.WORLD_UPDATE);
                    output.WriteInt(Constants.Constants.LoginServer.LoginServerID);
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
                    output.WriteInt(Constants.Constants.LoginServer.MaxPacketSize); // Server UDP Size
                    stream.Position = 0;
                    var byterray = stream.ToArray();
                    queueList.Enqueue(byterray);
                }
            }
        }
    }
}