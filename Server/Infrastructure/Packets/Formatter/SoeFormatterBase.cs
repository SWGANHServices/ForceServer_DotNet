using SwgAnh.Docker.Infrastructure.Packets.Reader;
using System.IO;
using System.Runtime.Serialization;

namespace SwgAnh.Docker.Infrastructure.Packets.Formatter
{
    public abstract class SoeFormatterBase : IFormatter
    {
        public SerializationBinder Binder { get; set; }
        public StreamingContext Context { get; set; }
        public ISurrogateSelector SurrogateSelector { get; set; }
        protected SoeReader SoeReader;

        public abstract object Deserialize(Stream serializationStream);
        public abstract void Serialize(Stream serializationStream, object graph);


        protected virtual short SetOpCode()
        {
            SoeReader.BaseStream.Position = 0;
            return SoeReader.ReadInt16();
        }

    }
}
