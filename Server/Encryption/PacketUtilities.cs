using System;
using System.IO;
using System.IO.Compression;

namespace SwgAnh.Docker.Encryption
{
    public static class PacketUtilities
    {
        public static byte[] CompressMessage(byte[] uncompressed)
        {
            var offset = uncompressed[0] == 0 ? (short)2 : (short)1;
            var toCompress = new byte[uncompressed.Length];
            Array.Copy(uncompressed, toCompress, uncompressed.Length);
            using (var memoryStream = new MemoryStream())
            using (var deflater = new DeflateStream(memoryStream, CompressionMode.Compress))
            {
                deflater.Write(toCompress);
                deflater.Flush();
                var output = memoryStream.ToArray();

                // long newLength = output.length;
                using (var secondMemoryStream = new MemoryStream())
                using (var binaryWriter = new BinaryWriter(secondMemoryStream))
                {
                    binaryWriter.Write(uncompressed, 0, offset);
                    binaryWriter.Write(output);
                    binaryWriter.Write(true);
                    binaryWriter.Write(0);
                    return secondMemoryStream.ToArray();
                }
            }
        }

        public static byte[] EncryptMessage(byte[] data, int length, int crcSeed)
        {
            crcSeed = ReverseBytes(crcSeed);
            int offset;
            if (data[0] == 0)
            {
                length -= 4;
                offset = 2;
            }
            else
            {
                length -= 3;
                offset = 1;
            }
            var blockCount = (length) / 4;
            var byteCount = (length) % 4;


            using (var memoryStream = new MemoryStream(data))
            using (var binaryReader = new BinaryReader(memoryStream))
            using (var writtenMemory = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(writtenMemory))
            {
                var header = new byte[offset];
                binaryWriter.Write(header);
                int temp;
                for (var i = 0; i < blockCount; i++)
                {
                    temp = binaryReader.ReadInt32();
                    temp ^= crcSeed;
                    crcSeed = temp;
                    binaryWriter.Write(temp);
                }
                crcSeed = ReverseBytes(crcSeed);
                for (var i = 0; i < byteCount; i++)
                {
                    temp = binaryReader.ReadByte();
                    temp ^= crcSeed;
                    binaryWriter.Write((byte)temp);
                }

                for (var i = data.Length - 2; i < data.Length; i++)
                {
                    binaryWriter.Write(data[i]);
                }


                var toReturn = writtenMemory.ToArray();
                return toReturn;
            }
        }

        private static int ReverseBytes(int crcSeed) => crcSeed << 24 |
                                                 (crcSeed & 0xff00) << 8 |
                                                 (int)((uint)(crcSeed >> 8) & 0xff00) |
                                                 (int)(uint)(crcSeed >> 24);


    }
}
