
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 获取在线好友列表的请求包，格式为
    /// * 1. 头部
    /// * 2. 1个字节，只有值为0x02或者0x03时服务器才有反应，不然都是返回0xFF
    /// *    经过初步的试验，发现3得到的好友都是一些系统服务，号码比如72000001到72000013，
    /// *    就是那些移动QQ，会员服务之类的；而2是用来得到好友的
    /// * 3. 起始位置，4字节。这个起始位置的含义与得到好友列表中的字段完全不同。估计是两拨人
    /// *    设计的，-_-!...
    /// *    这个起始位置需要有回复包得到，我们已经知道，在线好友的回复包一次最多返回30个好友，
    /// *    那么如果你的在线好友超过30个，就需要计算这个值。第一个请求包，这个字段肯定是0，后面
    /// *    的请求包和前一个回复包就是相关的了。具体的规则是这样的，在前一个回复包中的30个好友里面，
    /// *    找到QQ号最大的那个，然后把他的QQ号加1，就是下一个请求包的起始位置了！
    /// * 6. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class GetOnlineOpPacket : BasicOutPacket
    {
        public int StartPosition { get; set; }
        public GetOnlineSubCmd SubCommand { get; set; }
        public GetOnlineOpPacket(QQClient client)
            : base(QQCommand.Get_Online_OP,true,client)
        {
            StartPosition = QQGlobal.QQ_POSITION_ONLINE_LIST_START;
            SubCommand = GetOnlineSubCmd.GET_ONLINE_FRIEND;
        }
        public GetOnlineOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Get Friend Online Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.PutInt(StartPosition);
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
