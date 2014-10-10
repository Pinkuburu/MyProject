
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 临时会话操作请求包，格式为
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 当2部分为0x01时，表示发送临时会话消息，格式为
    /// * 3. 接收者QQ号，4字节
    /// * 4. 未知的4字节
    /// * 5. 发送者昵称长度，1字节
    /// * 6. 发送者昵称
    /// * 7. Site名称长度，1字节
    /// * 8. Site名称
    /// * Note: 所谓Site就是这个临时会话发源的地点，如果用户从某个群中开始一个
    /// * 临时会话，Site就是群名称，这个域可以指定为任意值，没有什么影响
    /// * 9. 未知的1字节
    /// * Note: 测试发现，9部分只有为0x01或者0x02时，才能使对方收到消息
    /// * 10. 未知的4字节
    /// * 11. 后面的内容长度，2字节，exclusive
    /// * 12. 消息内容，结尾追加空格
    /// * 13. 字体属性，参加edu.tsinghua.lumaqq.qq.beans.FontStyle
    /// * 
    /// * Note: 临时会话消息在QQ中是限制发送长度的，而且不支持多条发送
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class TempSessionOpPacket : BasicOutPacket
    {
        public TempSessionSubCmd SubCommand { get; set; }

        // 用于发送临时会话消息时
        public int Receiver { get; set; }
        public string Nick { get; set; }
        public string Site { get; set; }
        public string Message { get; set; }
        public FontStyle FontStyle { get; set; }

        public TempSessionOpPacket(QQClient client)
            : base(QQCommand.Temp_Session_OP_05,true,client)
        {
            Site = Nick = Message = string.Empty;
            FontStyle = new FontStyle();
        }
        public TempSessionOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            switch (SubCommand)
            {
                case TempSessionSubCmd.SendIM:
                    return "Send Temp Session IM Packet";
                default:
                    return "Unknown Temp Session Op Packet";
            }
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            switch (SubCommand)
            {
                case TempSessionSubCmd.SendIM:
                    // 接收者
                    buf.PutInt(Receiver);
                    // 未知
                    buf.PutInt(0);
                    // nick
                    byte[] b = Utils.Util.GetBytes(Nick);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                    // site name
                    b = Utils.Util.GetBytes(Site);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                    // 未知
                    buf.Put((byte)1);
                    // 未知
                    buf.PutInt(0);
                    // 长度，最后再填
                    int pos = buf.Length;
                    buf.PutChar((char)0);
                    // 消息内容
                    b = Utils.Util.GetBytes(Message);
                    buf.Put(b);
                    buf.Put((byte)0x20);
                    // 字体属性
                    FontStyle.Write(buf);
                    // 回填长度
                    buf.PutChar(pos, (char)(buf.Length - pos - 2));
                    break;
            }
        }
    }
}
