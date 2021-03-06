﻿
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets
{
    /// <summary>
    /// </summary>
    public class PacketHelper
    {
        //private IParser parser;
        private const int PARSER_COUNT = 4;
        private Dictionary<ProtocolFamily, IParser> parsers;
        public PacketHelper()
        {
            parsers = new Dictionary<ProtocolFamily, IParser>();
            parsers.Add(ProtocolFamily.Basic, new BasicFamilyParser());
        }
        /// <summary>
        /// 通过回复包获得请求包
        /// 通过重载Packet的Equals方法，本判断两个不同类型的对象相等。  
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <returns>OutPacket对象，如果没有找到，返回null</returns>
        public OutPacket RetriveSent(InPacket inPacket)
        {
            if (parsers[inPacket.GetFamily()] != null)
            {
                PacketHistory history = parsers[inPacket.GetFamily()].GetHistory();
                if (history != null)
                {
                    return history.RetrieveSent(inPacket);
                }
            }
            return null;
        }
        /// <summary>
        /// 缓存输出包
        /// </summary>
        /// <param name="outPacket">The out packet.</param>
        public void PutSent(OutPacket outPacket)
        {
            if (parsers[outPacket.GetFamily()] != null)
            {
                PacketHistory history = parsers[outPacket.GetFamily()].GetHistory();
                if (history != null)
                {
                    history.PutSent(outPacket);
                }
            }
        }
        /// <summary>
        /// 这个方法检查包是否已收到，要注意的是检查是针对这个包的hash值进行的，
        /// * 并不是对packet这个对象，hash值的计算是在packet的hashCode中完成的，
        /// * 如果两个packet的序号或者命令有不同，则hash值肯定不同。
    /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="add">if set to <c>true</c> [add].如果为true，则当这个包不存在时，添加这个包的hash，否则不添加</param>
        /// <returns>true如果这个包已经收到，否则false</returns>
        public bool IsReplied(OutPacket packet, bool add)
        {
            if (parsers[packet.GetFamily()] != null)
            {
                PacketHistory history = parsers[packet.GetFamily()].GetHistory();
                if (history != null)
                {
                    return history.Check(packet, add);
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// 检查包是否重复收到
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <returns></returns>
        public bool IsDuplicated(InPacket packet)
        {
            if (parsers[packet.GetFamily()] != null)
            {
                return parsers[packet.GetFamily()].IsDuplicate(packet);
            }
            return false;
        }
        /// <summary>
        /// 把ByteBuffer中的内容解析成一个InPacket子类，从buf的当前位置开始解析，直到limit为止
        /// * 不论解析成功或者失败，要把buf的position置于length后
        /// </summary>
        /// <returns></returns>
        public InPacket ParseIn(ProtocolFamily supportedFamily, ByteBuffer buf, QQClient client)
        {
            IParser parser = FindParser(supportedFamily, buf);
            if (parser == null)
            {
                return null;
            }
            return ParseIn(parser, buf, parser.GetLength(buf), client);
        }
        /// <summary>
        /// 查找一个能解析buf中内容的parser
        /// </summary>
        /// <param name="supportedFamily">The supported family.</param>
        /// <param name="buf">The buf.</param>
        /// <returns></returns>
        private IParser FindParser(ProtocolFamily supportedFamily, ByteBuffer buf)
        {
            IParser parser = parsers[supportedFamily];
            if (parser != null)
            {
                if (parser.Accept(buf))
                {
                    return parser;
                }
            }
            return null;
        }
        /// <summary>
        /// 把ByteBuffer中的内容解析成一个InPacket子类，从buf的当前位置开始解析length字节
        /// * 不论解析成功或者失败，buf的position将位于length后
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="buf">The buf.</param>
        /// <param name="length">The length.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private InPacket ParseIn(IParser parser, ByteBuffer buf, int length, QQClient client)
        {
            // 保存当前位置
            int offset = buf.Position;
            try
            {
                InPacket ret = parser.ParseIncoming(buf, length, client);
                bool duplicated = IsDuplicated(ret);
                bool needReply = parser.IsDuplicatedNeedReply(ret);
                if (duplicated && !needReply)
                {
                    return null;
                }
                else
                {
                    ret.IsDuplicated = duplicated;
                    return ret;
                }
            }
            finally
            {
                buf.Position = offset + length;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="supportedFamily">The supported family.</param>
        /// <param name="buf">The buf.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public OutPacket ParseOut(ProtocolFamily supportedFamily, ByteBuffer buf, QQClient client)
        {
            IParser parser = FindParser(supportedFamily, buf);
            if (parser == null)
            {
                return null;
            }
            return ParseOut(parser, buf, parser.GetLength(buf), client);
        }
        /// <summary> 把ByteBuffer中的内容解析成一个InPacket子类，从buf的当前位置开始解析，直到limit为止
        /// * 不论解析成功或者失败，要把buf的position置于length后
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="buf">The buf.</param>
        /// <param name="length">The length.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private OutPacket ParseOut(IParser parser, ByteBuffer buf, int length, QQClient client)
        {
            int pos = buf.Position;
            try
            {
                OutPacket ret = parser.ParseOutcoming(buf, length, client);
                return ret;
            }
            finally
            {
                buf.Position = pos + length;
            }
        }
        /// <summary> 把position设置到下一个包的起始位置处。一般当某段数据没有parser
        /// * 可以时，调用此方法跳过这段数据
        /// </summary>
        /// <param name="supportedFamily">The supported family.</param>
        /// <param name="buf">The buf.</param>
        /// <returns>true表示重定位成功，false表示失败或者推迟重定位</returns>
        public bool Relocate(ProtocolFamily supportedFamily, ByteBuffer buf)
        {
            int offset = buf.Position;
            if (parsers[supportedFamily] != null)
            {
                int relocated = parsers[supportedFamily].Relocate(buf);
                if (relocated > offset)
                {
                    buf.Position = relocated;
                    return true;
                }
                else
                    return false;
            }
            return false;
        }
    }
}
