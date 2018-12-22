namespace SwgAnh.Docker.Infrastructure.Packets
{
    public enum SoeOpCodes : short
    {
        SoeSessionRequest = 0x0100,
        SoeSessionResponse = 0x0200,
        Disconnect = 0x0500,
        Ping = 0x0600,
        SoeNetStatusReq = 0x0700,
        SoeNetStatusRes = 0x0800,
        SoeDataFragB = 0x0E00,
        SoeDataFragC = 0x0F00,
        SoeDataFragD = 0x1000,

        SoeOutOrderPktA = 0x1100,
        SoeOutOrderPktB = 0x1200,
        SoeOutOrderPktC = 0x1300,
        SoeOutOrderPktD = 0x1400,

        SoeAckA = 0x1500,
        SoeAckB = 0x1600,
        SoeAckC = 0x1700,
        SoeAckD = 0x1800,

        SoeChlDataB = 0x0A00,
        SoeChlDataC = 0x0B00,
        SoeChlDataD = 0x0C00,
        SoeDataFragA = 0x0D00,
        SoeChlDataA = 0x0900,

        // UpdateCodes TODO: Move out
        SoeFatalErr = 0x1D00
    }
}