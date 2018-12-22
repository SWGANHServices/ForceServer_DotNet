using System;
using System.Collections.Generic;
using System.IO;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.SwgStream;
using static SwgAnh.Docker.Constant.Constants.LoginServer;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class ChlDataReceived : IChlDataRecived
    {
        private readonly ILogger _logger;

        private readonly Queue<byte[]> _queuedMessages = new Queue<byte[]>();

        // Received 
        private readonly ISystemMessage _systemMessage;

        public ChlDataReceived(ISystemMessage systemMessage, ILogger logger)
        {
            _systemMessage = systemMessage;
            _logger = logger;
        }

        public void ChlDataAReceived(SwgInputStream inputStream)
        {
            var updateType = inputStream.UpdateType;
            GenerateAck();
            _logger.LogDebug($"UpdateType = {updateType}");
            switch (updateType)
            {
                case (short) UpdateCodes.WorldUpdate:
                    _logger.LogDebug(UpdateCodes.WorldUpdate.ToString());
                    break;
                case (short) UpdateCodes.AccountUpdate:
                    _logger.LogDebug(UpdateCodes.AccountUpdate.ToString());

                    break;
                case (short) UpdateCodes.ClientUiUpdate:
                    _logger.LogDebug(UpdateCodes.ClientUiUpdate.ToString());

                    break;
                case (short) UpdateCodes.SceneUpdate:
                    _logger.LogDebug(UpdateCodes.SceneUpdate.ToString());

                    break;
                case (short) UpdateCodes.ObjectUpdate:
                    _logger.LogDebug(UpdateCodes.ObjectUpdate.ToString());

                    break;
                case (short) UpdateCodes.ServerUpdate:
                    _logger.LogDebug(UpdateCodes.ServerUpdate.ToString());
                    var updateCrc = inputStream.ReadInt32(); // TODO: should create object out of these
                    var isLoginClient = updateCrc == LoginClientId;
                    if (isLoginClient)
                    {
                        // yey finally we can start with db stuff :)
                        SendLoginClientToken();
                        SendLoginEnumCluster();
                        SendLoginClusterStatus();
                        SendEnumerateCharacterID();
                    }

                    break;
                case (short) UpdateCodes.UpdateCharCreate:
                    _logger.LogDebug(UpdateCodes.UpdateCharCreate.ToString());
                    break;
                default:
                    throw new NotSupportedException("Update code not supported");
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
            using (var memoryStream = new MemoryStream())
            using (var swgOutputStream = new SwgOutputStream(memoryStream))
            {
                swgOutputStream.Write((short) SoeOpCodes.SoeChlDataA);
                swgOutputStream.Write((short) 0);
                swgOutputStream.Write((short) UpdateCodes.AccountUpdate);
                swgOutputStream.Write(LoginEnumCluster);
                swgOutputStream.Write(1);
                swgOutputStream.Write(10);
                swgOutputStream.Write("SwgAng Core");
                swgOutputStream.Write(0);
                swgOutputStream.Write(1);
                _queuedMessages.Enqueue(memoryStream.ToArray());
            }
        }

        private void SendLoginClientToken()
        {
            using (var memoryStream = new MemoryStream())
            using (var swgOutputStream = new SwgOutputStream(memoryStream))
            {
                swgOutputStream.Write((short) SoeOpCodes.SoeChlDataA);
                swgOutputStream.Write((short) 0);
                swgOutputStream.Write(LoginClientToken);
                swgOutputStream.Write(10 + 4); // TODO: SessionKey length?
                swgOutputStream.Write(10); // TODO: Session Key
                swgOutputStream.Write(9000); // Account ID set to 9000
                swgOutputStream.Write(9000); // Account ID set to 9000
                swgOutputStream.Write("admin"); // username
                _queuedMessages.Enqueue(memoryStream.ToArray());
            }
        }


        private void GenerateAck()
        {
            using (var stream = new MemoryStream())
            {
                using (var output = new SwgOutputStream(stream))
                {
                    output.Write((short) SoeOpCodes.SoeChlDataA);
                    output.Write((short) 0);
                    output.Write(0);
                    SwgOutputStream.GenerateCrCSeed(stream.ToArray(), 0);
                    _systemMessage.SendMessage(stream.ToArray());
                }
            }
        }
    }
}