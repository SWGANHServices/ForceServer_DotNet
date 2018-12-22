using System;
using System.Collections.Generic;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Infrastructure.Factories
{
    /// <summary>
    ///     SoeActionFactory handles all incoming bytes, and distribute them to the different modules based on OPCode
    /// </summary>
    public class SoeActionFactory : ISoeActionFactory
    {
        private readonly IChlDataRecived _chlDataReceived;
        private readonly INetStatusRequestRecived _netStatusRequestReceived;
        private readonly IDictionary<SoeOpCodes, Action<SwgInputStream>> _operations;
        private readonly ISessionReceivedHandler _sessionReceivedHandler;

        public SoeActionFactory(
            ISessionReceivedHandler sessionReceivedHandler,
            IChlDataRecived chlDataReceived,
            INetStatusRequestRecived netStatusRequestReceived)
        {
            _sessionReceivedHandler = sessionReceivedHandler;
            _chlDataReceived = chlDataReceived;
            _netStatusRequestReceived = netStatusRequestReceived;
            _operations = new Dictionary<SoeOpCodes, Action<SwgInputStream>>
            {
                {SoeOpCodes.SoeSessionRequest, HandleSessionRequest},
                {SoeOpCodes.SoeChlDataA, HandleChannelDataA},
                {SoeOpCodes.SoeNetStatusReq, HandleNetSatusRequest}
            };
        }

        public void InitiateAction(SwgInputStream stream)
        {
            _operations[(SoeOpCodes) stream.OpCode].Invoke(stream);
        }

        private void HandleNetSatusRequest(SwgInputStream stream)
        {
            _netStatusRequestReceived.HandleNetStatusRequest(stream);
        }

        private void HandleChannelDataA(SwgInputStream stream)
        {
            _chlDataReceived.ChlDataAReceived(stream);
        }

        private void HandleSessionRequest(SwgInputStream stream)
        {
            _sessionReceivedHandler.HandleSessionReceived(stream);
        }
    }
}