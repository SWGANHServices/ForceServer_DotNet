﻿using Server.src.Contracts;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.SwgStream;
using SwgAnh.Docker.src.Contracts;
using System;
using System.Collections.Generic;
using System.IO;

namespace SwgAnh.Docker.src.Infrastructure.LoginServer
{
    public class ChlDataRecived : IChlDataRecived
    {
        private readonly ISystemMessage _systemMessage;

        public ChlDataRecived(ISystemMessage systemMessage)
        {
            _systemMessage = systemMessage;
        }
        public Queue<byte[]> IChlDataARecived(SwgInputStream inputStream)
        {
            GenerateAck(inputStream.Sequence);
            throw new NotImplementedException();
        }
        

        private void GenerateAck(short sequence)
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
