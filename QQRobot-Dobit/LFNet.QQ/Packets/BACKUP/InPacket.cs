
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets
{
    /// <summary>
    /// 所有输入包的基类
    /// </summary>
    public abstract class InPacket : Packet
    {       
        /// <summary>
    /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="source">The source.</param>
        /// <param name="command">The command.</param>
        /// <param name="user">The user.</param>
        public InPacket(byte header, char source, QQCommand command, QQClient client)
            : base(header, source, command, (char)0, client)
        {
        }
        /// <summary>
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">The length.</param>
        /// <param name="user">The user.</param>
        public InPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        /// <summary>
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="user">The user.</param>
        public InPacket(ByteBuffer buf, QQClient client) : base(buf, client) { }
        /// <summary>
        /// 校验头部, 默认返回 true
    /// </summary>
        /// <returns>true</returns>
        protected override bool ValidateHeader()
        {
            return true;
        }    

    }
}
