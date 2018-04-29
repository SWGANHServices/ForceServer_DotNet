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

        public static T Deserialize<T> (this byte[] byteArray) where T : class {
            if (byteArray == null) {
                return null;
            }
            using (var memStream = new MemoryStream(byteArray)) {
                var swgStream = new SwgInputStream(memStream);
                
                // memStream.Write (byteArray, 0, byteArray.Length);
                // memStream.Seek (0, SeekOrigin.Begin);
                // var obj = (T) binForm.Deserialize (memStream);
                return null;
            }
        }
    }
}