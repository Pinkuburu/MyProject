
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 用户属性操作请求包
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 起始位置，2字节
    /// * 4. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class UserPropertyOpPacket : BasicOutPacket
    {
        public UserPropertySubCmd SubCommand { get; set; }
        public ushort StartPosition { get; set; }
        public UserPropertyOpPacket(QQClient client)
            : base(QQCommand.User_Property_OP_05,true,client)
        {
            SubCommand = UserPropertySubCmd.GET;
            StartPosition = QQGlobal.QQ_POSITION_USER_PROPERTY_START;
        }
        public UserPropertyOpPacket(ByteBuffer buf, int length, QQClient client)
            : base(buf, length, client)
        {

        }
        public override string GetPacketName()
        {
            return "User Property Op Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.PutUShort(StartPosition);
        }
    }
}
