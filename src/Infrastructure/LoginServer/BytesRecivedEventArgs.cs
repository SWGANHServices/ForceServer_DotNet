using System;

namespace SwgAnh.Docker.Infrastructure.LoginServer
{
    public class BytesRecivedEventArgs : EventArgs 
    {
        public byte[] RecivedBytes { get; set; }
    }
}