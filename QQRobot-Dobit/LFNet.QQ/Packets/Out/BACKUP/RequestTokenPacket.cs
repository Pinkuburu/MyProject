
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    /// 
    /// * 请求密钥包，格式为：
    /// * 1. 头部
    /// * 2. 密钥类型，一个字节，0x3或者0x4
    /// * 3. 尾部
    /// * 
    /// * 这个包用来请求得到一些操作的密钥，比如文件中转，或者语音视频之类的都有可能
    /// </summary>
    public class RequestTokenPacket : BasicOutPacket
    {
        /// <summary>
        /// 请求的命令
        /// 0x01 添加删除好友请求
        /// 0x02 添加删除好友发送验证码
        /// 暂不支持其它命令，目前直接通过验证码判断了 这个属性无效
        /// </summary>
        public byte Request { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 0x01添加好友
        /// 0x02删除
        /// </summary>
        public ushort Type { get; set; }
        /// <summary>
        /// 要添加的QQ号
        /// </summary>
        public int QQ { get; set; }
        /// <summary>
        /// 获取的验证码得到的会话字节
        /// </summary>
        public byte[] VCodeSession { get; set; }
        public RequestTokenPacket(QQClient client) : base(QQCommand.RequestToken, true, client) { }
        public RequestTokenPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Request Token Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            if (Code != 0)
            {
                buf.Put(0x02);//sub cmd
                buf.PutUShort(Type);
                buf.PutInt(QQ);
                buf.PutUShort(4);
                buf.PutInt(Code);
                buf.PutUShort((ushort)VCodeSession.Length);
                buf.Put(VCodeSession);

            }
            else 
            {
                buf.Put(0x01);//sub cmd
                buf.PutUShort(Type);
                buf.PutInt(QQ);
            }
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }





    //    qqpacket* p = packetmgr_new_send( qq, QQ_CMD_REQUEST_TOKEN );
    //if( !p ) return;
    //bytebuffer *buf = p->buf;
    //qq->data.operation = operation;
    //if( code ){	//输入验证码
    //    put_byte( buf, 2 );	//sub cmd
    //    put_word( buf, type );	//
    //    put_int( buf, number );
    //    put_word( buf, 4 );
    //    put_int( buf, htonl(code) );
    //    put_word( buf, strlen(qq->data.qqsession));
    //    put_data( buf, (uchar*)qq->data.qqsession, strlen(qq->data.qqsession));
    //}else{
    //    put_byte( buf, 1 );	//sub cmd
    //    put_word( buf, type );	//
    //    put_int( buf, number );
    //    qq->data.operating_number = number ;
    //}
    //post_packet( qq, p, SESSION_KEY );
    }
}
