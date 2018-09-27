using System;
using System.Collections.Generic;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Infrastructure.Factories
{
    /// <summary>
    /// SoeActionFactory handles all inncoming bytes, and distribute them to the different modules based on OPCode
    /// </summary>
    public class SoeActionFactory : ISoeActionFactory
    {
        private readonly ISessionRecivedHandler _sessionRecivedHandler;
        private readonly IChlDataRecived _chlDataRecived;
        private readonly INetStatusRequestRecived _netStatusRequestRecived;
        private readonly IDictionary<SoeOpCodes, Action<SwgInputStream>> _operations;

        public SoeActionFactory(ISessionRecivedHandler sessionRecivedHandler,
            IChlDataRecived chlDataRecived,
            INetStatusRequestRecived netStatusRequestRecived)
        {
            _sessionRecivedHandler = sessionRecivedHandler;
            _chlDataRecived = chlDataRecived;
            _netStatusRequestRecived = netStatusRequestRecived;
            _operations = new Dictionary<SoeOpCodes, Action<SwgInputStream>> {
                { SoeOpCodes.SoeSessionRequest, HandleSessionRequest },
                { SoeOpCodes.SoeChlDataA, HandleChannelDataA },
                { SoeOpCodes.SoeNetStatusReq, HandleNetSatusRequest}
            };
        }

        private void HandleNetSatusRequest(SwgInputStream stream)
        {
            _netStatusRequestRecived.HandleNetStatusRequest(stream);
        }

        public void InitiateAction(SwgInputStream stream)
        {
            _operations[(SoeOpCodes)stream.OpCode].Invoke(stream);
        }

        private void HandleChannelDataA(SwgInputStream stream)
        {
            _chlDataRecived.ChlDataARecived(stream);
        }

        private void HandleSessionRequest(SwgInputStream stream)
        {
            _sessionRecivedHandler.HandleSessionRecived(stream);
        }

    }
}