using System.IO;
using SwgAnh.Docker.Constant;

namespace SwgAnh.Docker.Infrastructure.SwgStream
{
    public class SwgOutputStream : BinaryWriter
    {
        public SwgOutputStream(Stream stream) : base(stream)
        {
        }

        public static void GenerateCrCSeed(byte[] stream, int crcSeed)
        {
            const short crcLength = 2;
            var length = (short) (stream.Length - crcLength);
            var arrayIndex = ~crcSeed & 0xFF;
            var nCrc = Constants.LoginServer.CrcTable[arrayIndex];
            nCrc ^= 0x00FFFFFF;
            var nIndex = (int) ((crcSeed >> 8) ^ nCrc);
            nCrc = (nCrc >> 8) & 0x00FFFFFF;
            nCrc ^= Constants.LoginServer.CrcTable[nIndex & 0xFF];
            nIndex = (int) ((crcSeed >> 16) ^ nCrc);
            nCrc = (nCrc >> 8) & 0x00FFFFFF;
            nCrc ^= Constants.LoginServer.CrcTable[nIndex & 0xFF];
            nIndex = (int) ((crcSeed >> 24) ^ nCrc);
            nCrc = (nCrc >> 8) & 0x00FFFFFF;
            nCrc ^= Constants.LoginServer.CrcTable[nIndex & 0xFF];
            for (var i = 0; i < length; i++)
            {
                nIndex = (int) (stream[i] ^ nCrc);
                nCrc = (nCrc >> 8) & 0x00FFFFFF;
                nCrc ^= Constants.LoginServer.CrcTable[nIndex & 0xFF];
            }

            var crc = ~nCrc;
            var newOutput = stream;
            for (short i = 0; i < crcLength; i++) newOutput[length - 1 - i] = (byte) ((crc >> (8 * i)) & 0xFF);
            var newStream = new MemoryStream(newOutput);
            newStream.Write(newOutput);
        }
    }
}