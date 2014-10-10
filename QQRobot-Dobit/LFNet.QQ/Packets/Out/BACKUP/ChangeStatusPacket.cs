
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 改变状态的包，格式为
    /// </summary>
    public class ChangeStatusPacket : BasicOutPacket
    {
        /// <summary>
        /// 是否显示虚拟摄像头
        /// </summary>
        /// <value></value>
        public bool ShowFakeCam { get; set; }
        /// <summary>
        /// QQ状态
        /// </summary>
        public QQStatus Status { get; set; }
        //public ChangeStatusPacket(QQClient client)
        //    : base(QQCommand.Change_Status, true, client)
        //{
        //    ShowFakeCam = client.QQUser.IsShowFakeCam;
        //    Status = client.PrivateManager.
        //}
        /// <summary>
        /// QQ状态等于当前QQ状态
        /// </summary>
        /// <param name="client"></param>
        /// <param name="showFakeCam"></param>
        public ChangeStatusPacket(QQClient client, QQStatus status)
            : base(QQCommand.Change_Status, true, client)
        {
            this.ShowFakeCam = client.QQUser.IsShowFakeCam;
            this.Status = status;
        }
        public ChangeStatusPacket(QQClient client,QQStatus status, bool showFakeCam)
            : base(QQCommand.Change_Status, true, client)
        {
            this.ShowFakeCam = showFakeCam;
            this.Status = status;
        }
        public ChangeStatusPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length,  client) { }
        protected override void PutBody(ByteBuffer buf)
        {
            // 设置状态
            buf.Put((byte)Status);
            buf.PutInt(0);//00 00 00 00
            buf.PutInt(ShowFakeCam ? 1 : 0);// 显示虚拟摄像头	
            buf.PutUShort(0);
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }

    }
}
