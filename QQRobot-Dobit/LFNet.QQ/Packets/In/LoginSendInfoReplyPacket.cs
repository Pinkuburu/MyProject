#region 版权声明
//=========================================================== 
// 版权声明：LFNet.QQ是基于QQ2009版本的QQ协议开发而成的，协议
// 分析主要参考自小虾的MyQQ(C++)源代码，代码开发主要基于阿布
// 的LumaQQ.NET的C#.NET代码修改而成，故继续遵照使用LumaQQ的开
// 源协议。
//
// 本人没有对LumaQQ.NET的C#.NET代码的框架做过多的改动，主
// 要工作为将MyQQ的C++协议代码部分翻译成符合LumaQQ.Net框架
// 的C#代码，故请尊重LumaQQ作者Luma的著作权和版权声明。
// 
// 代码开源主要用于解决大家在学习和研究协议过程中遇到由于缺乏代码所带来的制约性问题。
// 本代码仅供学习交流使用，大家在使用此开发包前请自行协调好多方面关系，
// 不得用于任何商业用途和非法用途，本人不享受和承担由此产生的任何权利以及任何法律责任。
// 
// 本源代码可通过以下网址获取:
// http://QQCode.lynfo.com, http://www.lynfo.com, http://bbs.lynfo.com, http://hi.baidu.com/dobit.
//
// Copyright @ 2009-2010  Lynfo.com.  All Rights Reserved.   
// Framework: 2.0
// Author: Luma(java版) → Abu(C# QQ2005协议版) → Dobit(C# QQ2009协议版本)
// Email: dobit@msn.cn   
// Created: 2009-3-1~ 2009-11-28
// Last Modified:2009-11-28    
//   
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details. 
//===========================================================   
#endregion

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
