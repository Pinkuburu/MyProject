
using System;

namespace LFNet.QQ.Packets
{
    public class PacketParseException : Exception
    {
        public PacketParseException()
            : base()
        {
        }
        public PacketParseException(string s) : base(s) { }
        public PacketParseException(string s, Exception e) : base(s, e) { }
    }
}
