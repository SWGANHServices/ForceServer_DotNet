using System;
using System.Collections.Generic;
using System.Text;

namespace SwgAnh.Docker.Infrastructure.Packets
{
    public enum UpdateCodes : short
    {
        ClientUiUpdate = 0x0001,
        WorldUpdate = 0x0002,
        AccountUpdate = 0x0003,
        ServerUpdate = 0x0004,
        ObjectUpdate = 0x0005,
        UpdateSix = 0x0006,
        UpdateSeven = 0x0007,
        SceneUpdate = 0x0008,
        UpdateNine = 0x0009,
        UpdateTen = 0x000A,
        UpdateEleven = 0x000B,
        UpdateCharCreate = 0x000C,
        UpdateThirteen = 0x000D,
        UpdateFourteen = 0x000E,
        UpdateFifteen = 0x000F
    }
}
