using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using SwgAnh.Docker.Infrastructure.Packets;

namespace SwgAnh.Docker.Infrastructure.SwgStream {
    public class SwgInputStream : StreamWriter {
        private byte[] inncommingData;
        private readonly Stream _stream;
        
        public short OpCode { get; private set; }
        public short Sequence { get; private set; }
        public short UpdateType { get; private set; }
        
        public SwgInputStream (Stream stream) : base (stream) {
            _stream = stream;
            inncommingData = new byte[stream.Length];
            _stream.Read (inncommingData, 0, inncommingData.Length);
            _stream.Seek (0, SeekOrigin.Begin);
            OpCode = ReadShort();
            
            if (OpCode == (short)SoeOpCodes.SoeChlDataA 
                || OpCode == (short)SoeOpCodes.SoeDataFragA 
                || OpCode == (short)SoeOpCodes.SoeAckA 
                || OpCode == (short)SoeOpCodes.SoeOutOrderPktA) {
                Sequence = ReverseBytes();
                if (OpCode == (short)SoeOpCodes.SoeChlDataA) {
                    UpdateType = ReadShort();
                } else {
                    UpdateType = -1;
                }
            } else {
                Sequence = -1;
            }
        }

        private short ReadShort () {
            var ch1 = _stream.ReadByte();
            var ch2 = _stream.ReadByte();
            if ((ch1 | ch2) < 0)
                throw new EndOfStreamException ();
            return (short) ((ch2 << 8) + (ch1 << 0));
        }

        private short ReverseBytes () {
            var i = ReadShort();
            return (short) ((i<<8) + (i>> 8));
        }
    }
}