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
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, sessionRecived);

                using (var output = new SwgOutputStream(stream))
                {
                    output.Write((short)SoeOpCodes.SoeChlDataA);
                    output.Write((short)0);
                    output.Write((short)SoeOpCodes.WORLD_UPDATE);
                    output.Write(Constants.Constants.LoginServer.LoginServerString);
                    output.Write(Constants.Constants.LoginServer.LoginServerInfo);
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
                    output.Write((short)SoeOpCodes.SoeChlDataA);
                    output.Write((short)0);
                    output.Write((short)SoeOpCodes.WORLD_UPDATE);
                    output.Write(Constants.Constants.LoginServer.LoginServerID);
                    output.Write((int)29411);
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
                    output.SetOpCode((short)SoeOpCodes.SoeSessionResponse);
                    output.Write(sessionRecived.ClientId);
                    output.Write(sessionRecived.CsrSeed);
                    output.Write((byte)2);
                    output.Write((byte)1);
                    output.Write((byte)4);
                    output.Write(Constants.Constants.LoginServer.MaxPacketSize);
                    stream.Position = 0;
                    queueList.Enqueue(stream.ToArray());
                }
            }
        }
    }
}