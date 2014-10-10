
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 请求密钥包，格式为：
    /// * 1. 头部
    /// * 2. 密钥类型，一个字节，0x3或者0x4
    /// * 3. 尾部
    /// * 
    /// * 这个包用来请求得到一些操作的密钥，比如文件中转，或者语音视频之类的都有可能
    /// </summary>
    public class RequestKeyPacket : BasicOutPacket
    {
        /// <summary>
        /// 密匙类型
        /// </summary>
        public byte KeyType { get; set; }
        public RequestKeyPacket(QQClient client, byte keyType) : base(QQCommand.Request_Key, true, client) { this.KeyType = keyType; }
        public RequestKeyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Request Key Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put(KeyType);
        }
    }
}
