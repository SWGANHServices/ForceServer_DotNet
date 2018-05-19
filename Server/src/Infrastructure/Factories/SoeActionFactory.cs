using System;
using System.Collections.Generic;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.SwgStream;
using SwgAnh.Docker.src.Contracts;

namespace Server.src.Infrastructure
{
    public class SoeActionFactory : ISoeActionFactory
    {
        private readonly ISessionRecivedHandler _sessionRecivedHandler;
        private readonly IChlDataRecived _chlDataRecived;
        private readonly IDictionary<SoeOpCodes, Action<SwgInputStream>> Operations;

        public SoeActionFactory(ISessionRecivedHandler sessionRecivedHandler, 
            IChlDataRecived chlDataRecived)
        {
            _sessionRecivedHandler = sessionRecivedHandler;
            _chlDataRecived = chlDataRecived;
            Operations = new Dictionary<SoeOpCodes, Action<SwgInputStream>> {
                { SoeOpCodes.SoeSessionRequest, HandleSessionRequest },
                { SoeOpCodes.SoeChlDataA, HandleChannelDataA },
            };
        }

        public void InitiateAction(SwgInputStream stream)
        {
            Operations[(SoeOpCodes)stream.OpCode].Invoke(stream);
        }

        private void HandleChannelDataA(SwgInputStream stream)
        {
            _chlDataRecived.IChlDataARecived(stream);
        }

        private void HandleSessionRequest(SwgInputStream stream)
        {
            _sessionRecivedHandler.HandleSessionRecived(stream);
        }

    }
}