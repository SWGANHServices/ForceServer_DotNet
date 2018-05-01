using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using SwgAnh.Docker.Infrastructure.Packets;

namespace SwgAnh.Docker.Infrastructure.SwgStream {
    public class SwgInputStream : BinaryReader {
        private byte[] inncommingData;
        private readonly Stream _stream;
        
        public short OpCode { get; }
        public short Sequence { get; set; }
        public short UpdateType { get; }
        
        public SwgInputStream (Stream stream) : base (stream) {
            _stream = stream;
            inncommingData = new byte[stream.Length];
            _stream.Read (inncommingData, 0, inncommingData.Length);
            _stream.Seek (0, SeekOrigin.Begin);
            OpCode = ReadInt16();
            
            if (OpCode == (short)SoeOpCodes.SoeChlDataA 
                || OpCode == (short)SoeOpCodes.SoeDataFragA 
                || OpCode == (short)SoeOpCodes.SoeAckA 
                || OpCode == (short)SoeOpCodes.SoeOutOrderPktA) {
                Sequence = ReverseBytes();
                if (OpCode == (short)SoeOpCodes.SoeChlDataA) {
                    UpdateType = ReadInt16();
                } else {
                    UpdateType = -1;
                }
            } else {
                Sequence = -1;
            }
        }

        

        private short ReverseBytes () {
            var i = ReadInt16();
            return (short) ((i<<8) + (i>> 8));
        }

        public override short ReadInt16()
        {
            var ch1 = _stream.ReadByte();
            var ch2 = _stream.ReadByte();
            if ((ch1 | ch2) < 0)
                throw new EndOfStreamException ();
            return (short) ((ch2 << 8) + (ch1 << 0));
        }
    }
}