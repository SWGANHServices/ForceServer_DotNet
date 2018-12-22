using System.IO;

namespace SwgAnh.Docker.Infrastructure.Packets.Reader
{
    public class SoeReader : BinaryReader
    {
        public SoeReader(Stream input) : base(input)
        {
        }
    }
}