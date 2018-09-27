using System;
using System.Collections.Generic;
using System.IO;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.SwgStream;
using static SwgAnh.Docker.Constant.Constants.LoginServer;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class ChlDataRecived : IChlDataRecived
    {
        private readonly ISystemMessage _systemMessage;
        private readonly ILogger _logger;
        private readonly Queue<byte[]> _queuedMessages = new Queue<byte[]>();

        public ChlDataRecived(ISystemMessage systemMessage, ILogger logger)
        {
            _systemMessage = systemMessage;
            _logger = logger;
        }
        public void ChlDataARecived(SwgInputStream inputStream)
        {
            var updateType = inputStream.UpdateType;
            GenerateAck();
            switch (updateType)
            {
                case (short)UpdateCodes.WorldUpdate:
                    _logger.LogDebug(UpdateCodes.WorldUpdate.ToString());
                    break;
                case (short)UpdateCodes.AccountUpdate:
                    _logger.LogDebug(UpdateCodes.AccountUpdate.ToString());

                    break;
                case (short)UpdateCodes.ClientUiUpdate:
                    _logger.LogDebug(UpdateCodes.ClientUiUpdate.ToString());

                    break;
                case (short)UpdateCodes.SceneUpdate:
                    _logger.LogDebug(UpdateCodes.SceneUpdate.ToString());

                    break;
                case (short)UpdateCodes.ObjectUpdate:
                    _logger.LogDebug(UpdateCodes.ObjectUpdate.ToString());

                    break;
                case (short)UpdateCodes.ServerUpdate:
                    _logger.LogDebug(UpdateCodes.ServerUpdate.ToString());

                    var updateCrc = inputStream.ReadInt32(); // TODO: should create oject out of these
                    var isLoginClient = updateCrc == LoginClientId;
                    if (isLoginClient)
                    {
                        // yey finaly we can start with db stuff :)
                        SendLoginClientToken();
                        SendLoginEnumCluster();
                        SendLoginClusterStatus();
                        SendEnumerateCharacterID();
                    }
                    break;
                case (short)UpdateCodes.UpdateCharCreate:
                    _logger.LogDebug(UpdateCodes.UpdateCharCreate.ToString());
                    break;
                default:
                    throw new NotSupportedException("Updatecode not supported");
            }

            _systemMessage.SendMessage(_queuedMessages);
        }

        private void SendEnumerateCharacterID()
        {
        }

        private void SendLoginClusterStatus()
        {
        }

        private void SendLoginEnumCluster()
        {
            using (var memorystream = new MemoryStream())
            using (var swgOutputStream = new SwgOutputStream(memorystream))
            {
                swgOutputStream.SetOpCode((short)SoeOpCodes.SoeChlDataA);
                swgOutputStream.SetSequence(0);
                swgOutputStream.SetUpdateType(UpdateCodes.AccountUpdate);
                swgOutputStream.WriteInt(LoginEnumCluster);
                swgOutputStream.WriteInt(1);
                swgOutputStream.WriteInt(10);
                swgOutputStream.WriteUtf("SwgAng Core");
                swgOutputStream.WriteInt(0);
                swgOutputStream.WriteInt(1);
                _queuedMessages.Enqueue(memorystream.ToArray());
            }
        }

        private void SendLoginClientToken()
        {
            using (var memoryStream = new MemoryStream())
            using (var swgOutputStream = new SwgOutputStream(memoryStream))
            {
                swgOutputStream.SetOpCode((short)SoeOpCodes.SoeChlDataA);
                swgOutputStream.SetSequence(0);
                swgOutputStream.WriteInt(LoginClientToken);
                swgOutputStream.WriteInt(10 + 4); // TODO: SessionKey length?
                swgOutputStream.WriteInt(10); // TODO: Session Key
                swgOutputStream.WriteInt(9000); // Account ID set to 9000
                swgOutputStream.WriteInt(9000); // Account ID set to 9000
                swgOutputStream.WriteUtf("admin"); // username
                _queuedMessages.Enqueue(memoryStream.ToArray());
            }

        }


        private void GenerateAck()
        {
            using (var stream = new MemoryStream())
            {
                using (var output = new SwgOutputStream(stream))
                {
                    output.WriteShort((short)SoeOpCodes.SoeChlDataA);
                    output.WriteByte(0);
                    output.WriteShort(0);
                    output.GenerateCrCSeed(stream.ToArray(), 0);
                    _systemMessage.SendMessage(stream.ToArray());
                }
            }
        }
    }
}
