
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 用来发送验证消息
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 要添加的QQ号，4字节
    /// * 4. 是否允许对方加自己为好友，1字节
    /// * 5. 把好友加到第几组，我的好友组是0，然后以此类推，1字节
    /// * 6. 验证消息字节长度，1字节
    /// * 7. 验证消息
    /// * 8. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class AddFriendAuthorizePacket : BasicOutPacket
    {
        public AddFriendAuthSubCmd SubCommand { get; set; }
        public int To { get; set; }
        public RevenseAdd ReverseAdd { get; set; }
        public int DestGroup { get; set; }
        public string Message { get; set; }
        /// <summary>
        /// 添加好友操作 通过0xAE请求的令牌
        /// </summary>
        public byte[] AddFriendToken { get; set; }
        /// <summary>
        /// 添加好友操作 回答问题后得到的令牌
        /// </summary>
        public byte[] AnswerToken { get; set; }
        public AddFriendAuthorizePacket(QQClient client)
            : base(QQCommand.AddFriendAuthorize,true,client)
        {
            //SubCommand = AddFriendAuthSubCmd.Request;// 0x02;
            //ReverseAdd = RevenseAdd.Allow;
            DestGroup = 0;
            Message = string.Empty;
        }
        public AddFriendAuthorizePacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void PutBody(ByteBuffer buf)
        {
            switch (SubCommand)
            { 
                case AddFriendAuthSubCmd.Approve:
                case AddFriendAuthSubCmd.ApproveAndAdd:
                case AddFriendAuthSubCmd.Reject:
                case AddFriendAuthSubCmd.NoAuth:
                    //03 (01表示不需要验证时的加对方为好友,03表示接受并加对方为好友,04表示只接受,05表示拒绝)
                    //25 D0 1F E1 
                    //00 00 00
                    buf.Put((byte)SubCommand);
                    buf.PutInt(To);
                    buf.PutUShort(0);//00 00
                    byte[] b = Utils.Util.GetBytes(Message);
                    buf.Put((byte)b.Length);//长度
                    buf.Put(b);
                    break;
                case AddFriendAuthSubCmd.Add:
                case AddFriendAuthSubCmd.AnswerAdd:
                case AddFriendAuthSubCmd.NeedAuthor:
                    buf.Put((byte)SubCommand);
                    buf.PutInt(To);
                    buf.Put((byte)ReverseAdd);//00
                    buf.Put((byte)DestGroup);//00
                    buf.PutUShort((ushort)AddFriendToken.Length);
                    buf.Put(AddFriendToken);
                    if (AnswerToken != null)
                    {
                        buf.PutUShort((ushort)AnswerToken.Length);
                        buf.Put(AnswerToken);
                    }
                    buf.Put(0x01);
                    buf.Put(0x00);
                    if (!string.IsNullOrEmpty(Message))
                    {
                        b = Utils.Util.GetBytes(Message);
                        buf.Put((byte)b.Length);
                        buf.Put(b);
                    }
                    break;
                default:
                    throw new Exception("unknown AddFriendAuthSubCmd=0x" + SubCommand.ToString("X"));
            
            }
            

#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            
        }
    }
}
