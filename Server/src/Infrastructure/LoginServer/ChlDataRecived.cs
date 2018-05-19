using SwgAnh.Docker.Infrastructure.SwgStream;
using SwgAnh.Docker.src.Contracts;
using System;
using System.Collections.Generic;

namespace SwgAnh.Docker.src.Infrastructure.LoginServer
{
    public class ChlDataRecived : IChlDataRecived
    {
        public Queue<byte[]> IChlDataARecived(SwgInputStream inputStream)
        {
            throw new NotImplementedException();
        }
    }
}
