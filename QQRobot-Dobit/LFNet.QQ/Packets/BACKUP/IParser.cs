
using System;

namespace LFNet.QQ.Packets
{
    /// <summary>
    /// 包解析器
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// 判断此parser是否可以处理这个包，判断不能影响到buf的指针位置
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <returns>true表示这个parser可以处理这个包</returns>
        bool Accept(ByteBuffer buf);
        /// <summary>包的总长度
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <returns>包的总长度</returns>
        int GetLength(ByteBuffer buf);
        /// <summary>从buf当前位置解析出一个输入包对象，解析完毕后指针位于length之后
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">包长度.</param>
        /// <param name="user">The user.</param>
        /// <returns>InPacket子类，如果解析不了返回null</returns>
        InPacket ParseIncoming(ByteBuffer buf, int length, QQClient client);
        /// <summary>从buf当前位置解析出一个输出包对象，解析完毕后指针位于length之后
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">包长度.</param>
        /// <param name="user">QQ用户对象.</param>
        /// <returns>OutPacket子类，如果解析不了，返回null</returns>
        OutPacket ParseOutcoming(ByteBuffer buf, int length, QQClient client);
        /// <summary>
        /// 检查这个输入包是否重复
    /// </summary>
        /// <param name="packet">The packet.</param>
        /// <returns>true表示重复</returns>
        bool IsDuplicate(InPacket packet);
        /// <summary>检查这个包是重复包是否也要回复
    /// </summary>
        /// <param name="packet">The packet.</param>
        /// <returns>true表示即使这个包是重复包也要回复</returns>
        bool IsDuplicatedNeedReply(InPacket packet);
        /// <summary>假设buf的当前位置处是一个包，返回下一个包的起始位置。这个方法
        /// 用来重新调整buf指针。如果无法重定位，返回当前位置
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <returns>下一个包的起始位置</returns>
        int Relocate(ByteBuffer buf);

        /// <summary>
        /// PacketHistory类
    /// </summary>
        /// <returns></returns>
        PacketHistory GetHistory();
    }
}
