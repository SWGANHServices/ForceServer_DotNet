using System;
using System.Collections.Generic;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.SwgStream;

namespace Server.src.Infrastructure
{
    public class SoeActionFactory : ISoeActionFactory
    {
        private readonly ISessionRecivedHandler _sessionRecivedHandler;
        private readonly IDictionary<SoeOpCodes, Action<SwgInputStream>> Operations;

        public SoeActionFactory(ISessionRecivedHandler sessionRecivedHandler)
        {
            _sessionRecivedHandler = sessionRecivedHandler;

            Operations = new Dictionary<SoeOpCodes, Action<SwgInputStream>> {
                { SoeOpCodes.SoeSessionRequest, HandleSessionRequest },
                { SoeOpCodes.SoeChlDataA, HandleChannelDataA }
            };
        }

        public void InitiateAction(SwgInputStream stream)
        {
            Operations[(SoeOpCodes)stream.OpCode].Invoke(stream);
        }

        private void HandleChannelDataA(SwgInputStream obj)
        {
            throw new NotImplementedException();
        }

        private void HandleSessionRequest(SwgInputStream obj)
        {
            _sessionRecivedHandler.HandleSessionRecived(obj);
        }

    }
}