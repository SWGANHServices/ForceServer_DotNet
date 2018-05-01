using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SwgAnh.Docker.Infrastructure.SwgStream;

namespace SwgAnh.Docker.Infrastructure.Serialization {
    public static class SwgSerialization {
        public static byte[] SerializeToByteArray (this object obj) {
            if (obj == null) {
                return null;
            }
            var bf = new BinaryFormatter ();
            using (var ms = new MemoryStream ()) {
                bf.Serialize (ms, obj);
                return ms.ToArray ();
            }
        }

        public static SwgInputStream GetSOEPacket (this byte[] byteArray){
            if (byteArray == null) {
                return null;
            }
            using (var memStream = new MemoryStream(byteArray)) {
                var swgStream = new SwgInputStream(memStream);
                return swgStream;
            }
        }
    }
}