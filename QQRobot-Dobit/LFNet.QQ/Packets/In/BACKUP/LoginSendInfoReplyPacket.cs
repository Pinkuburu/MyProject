using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginSendInfoReplyPacket : BasicInPacket
    {
        public byte ReplyCode { get; private set; }
        public byte[] SessionKey { get; private set; }
        public string ClientIP { get; private set; }
        public int ClientPort { get; private set; }
        public string LocalIP { get; private set; }
        public int LocalPort { get; private set; }
        public DateTime LoginTime { get; private set; }
        public DateTime LastLoginTime { get; private set; }
        public QQStatus LoginMode { get; private set; }
        public byte[] IM_Key { get; private set; }

        /// <summary>
        /// 服务器返回的QQ号
        /// </summary>
        public int QQNumber { get; private set; }

        public LoginSendInfoReplyPacket(ByteBuffer buf, int length, QQClient client)
            : base(buf, length, client)
        {
        }
        public override string GetPacketName()
        {
            return "Login SendInfo Reply Packet";
        }

        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " Decoded Data:" + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            ReplyCode = buf.Get();
            if (ReplyCode == 0)//正确
            {
                this.SessionKey = buf.GetByteArray(16);
                this.QQNumber = buf.GetInt();
                this.ClientIP = Utils.Util.GetIpStringFromBytes(buf.GetByteArray(4));
                this.ClientPort = buf.GetUShort();
                this.LocalIP = Utils.Util.GetIpStringFromBytes(buf.GetByteArray(4));
                this.LocalPort = buf.GetUShort();
                this.LoginTime = Utils.Util.GetDateTimeFromMillis(buf.GetInt());
                buf.Get();//03
                this.LoginMode = (QQStatus)buf.Get();
                buf.Position += 96;
                this.LastLoginTime = Utils.Util.GetDateTimeFromMillis(buf.GetInt() *1000L);
                //IM_Key
                ByteBuffer BBuf = new ByteBuffer();
                BBuf.PutInt(QQNumber);
                BBuf.Put(SessionKey);
                this.IM_Key = Utils.Crypter.MD5(BBuf.ToByteArray());
                BBuf = null;
                Client.QQUser.QQKey.SessionKey = this.SessionKey;
                Client.QQUser.QQKey.IM_Key = this.IM_Key;


#if DEBUG
                Client.LogManager.Log("SessionKey:" + Utils.Util.ToHex(SessionKey));
#endif

                //if (Client.QQUser.QQ != )
                //{
                //    Client.LogManager.Log("The QQ Number Server returned is Error!");
                //}

                
            }
            //        bytebuffer *buf = p->buf;
            ////	hex_dump( buf->data, buf->len );
            //    uchar result = get_byte( buf );
            //    if( result != 0 )
            //    {
            //        DBG("login result = %d", result );
            //        qqclient_set_process( qq, P_ERROR );
            //        return;
            //    }
            //    get_data( buf, qq->data.session_key, sizeof(qq->data.session_key) );
            //    DBG("session key: " );
            //    hex_dump( qq->data.session_key, 16 );
            //    if( qq->number != get_int( buf ) ){
            //        DBG("qq->number is wrong?");
            //    }
            //    qq->client_ip = get_int( buf );
            //    qq->client_port = get_word( buf );
            //    qq->local_ip = get_int( buf );
            //    qq->local_port = get_word( buf );
            //    qq->login_time = get_int( buf );
            //    get_byte( buf );	//03
            //    get_byte( buf );	//mode
            //    buf->pos += 96;
            //    qq->last_login_time = get_int( buf );
            //    //prepare IM key
            //    uchar data[20];
            //    *(uint*)data = htonl( qq->number );
            //    memcpy( data+4, qq->data.session_key, 16 );
            //    //md5
            //    md5_state_t mst;
            //    md5_init( &mst );
            //    md5_append( &mst, (md5_byte_t*)data, 20 );
            //    md5_finish( &mst, (md5_byte_t*)qq->data.im_key );
            //    //
            //    time_t t;
            //    t = CN_TIME( qq->last_login_time );
            //    DBG("last login time: %s", ctime( &t ) );
            //    qqclient_set_process( qq, P_LOGIN );

            //    //get information
            //    prot_user_change_status( qq );
            //    prot_user_get_level( qq );
            //    group_update_list( qq );
            //    buddy_update_list( qq );
            //#ifndef NO_QUN_INFO
            //    qun_update_all( qq );
            //#endif
            //    qq->online_clock = 0;

        }

    }
}
