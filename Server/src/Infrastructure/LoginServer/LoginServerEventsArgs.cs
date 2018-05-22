using System;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class LoginServerEventsArgs : EventArgs 
    {
        public byte[] RecivedBytes { get; set; }
    }
}