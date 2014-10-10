
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 这个查询QQ号等级的包，格式是
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 查询的号码，4字节
    /// * 4. 如果有更多好友，重复3部分
    /// * 5. 尾部
    /// * 
    /// * QQ的做法是一次最多请求70个。号码必须按照大小排序，本来之前不排序也可以，后来腾讯可能在服务器端动了些手脚，必须
    /// * 得排序了。这种顺序并没有在这个类中维护，所以是否排序目前是上层的责任，这个类假设收到的是一个排好序的用户QQ号
    /// * 列表
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class FriendLevelOpPacket : BasicOutPacket
    {
        /// <summary>
        /// Gets or sets the friends.
        /// </summary>
        /// <value>The friends.</value>
        public List<int> Friends { get; set; }
        /// <summary>
        /// Gets or sets the sub command.
        /// </summary>
        /// <value>The sub command.</value>
        public FriendLevelSubCmd SubCommand { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendLevelOpPacket"/> class.
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">The length.</param>
        /// <param name="user">The user.</param>
        public FriendLevelOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendLevelOpPacket"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public FriendLevelOpPacket(QQClient client)
            : base(QQCommand.Friend_Level_OP_05,true,client)
        {
            SubCommand = FriendLevelSubCmd.GET;
        }
        /// <summary>
        /// 初始化包体
        /// <remark>abu 2008-02-18 </remark>
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            foreach (int friend in Friends)
                buf.PutInt(friend);
        }
        /// <summary>
        /// Gets the name of the packet.
        /// </summary>
        /// <returns></returns>
        public override string GetPacketName()
        {
            return "Get Friends Level Packet";
        }
    }
}
