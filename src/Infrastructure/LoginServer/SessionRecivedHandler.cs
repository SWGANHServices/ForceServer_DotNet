using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.SwgStream;
using SwgAnh.Docker.Models;
using SwgAnh.Docker.Constants;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class SessionRecivedHandler : ISessionRecivedHandler
    {
        public Queue<byte[]> HandleSessionRecived(SwgInputStream baseObject)
        {
            var queueList = new Queue<byte[]>();
            var sessionRecived = new SessionRecived();
            sessionRecived.Deserialize(baseObject);
            QueueServerSessionResponse(queueList, sessionRecived);
            QueueLoginServerResponse(queueList, sessionRecived);
            return queueList;
        }

        private void QueueLoginServerResponse(Queue<byte[]> queueList, SessionRecived sessionRecived)
        {
            var encoding = Encoding.UTF8;
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, sessionRecived);

                using (var output = new SwgOutputStream(stream, encoding))
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
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, sessionRecived);

                using (var output = new SwgOutputStream(stream))
                {
                    output.WriteShort((short)SoeOpCodes.SoeChlDataA);
                    output.WriteShort(0);
                    output.WriteShort((short)SoeOpCodes.WORLD_UPDATE);
                    output.WriteInt(Constants.Constants.LoginServer.LoginServerID);
                    output.WriteInt(29411);
                    stream.Position = 0;
                    queueList.Enqueue(stream.ToArray());
                }
            }
        }

        private static void QueueServerSessionResponse(Queue<byte[]> queueList, SessionRecived sessionRecived)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, sessionRecived);

                using (var output = new SwgOutputStream(stream))
                {
                    output.WriteShort((short)SoeOpCodes.SoeSessionResponse);
                    output.WriteInt(sessionRecived.ClientId);
                    output.WriteShort(sessionRecived.CsrSeed);
                    output.WriteByte(2);
                    output.WriteByte(1);
                    output.WriteByte(4);
                    output.WriteInt(Constants.Constants.LoginServer.MaxPacketSize);
                    stream.Position = 0;
                    queueList.Enqueue(stream.ToArray());
                }
            }
        }
    }
}