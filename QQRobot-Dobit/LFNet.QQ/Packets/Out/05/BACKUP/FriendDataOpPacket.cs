
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 上传下载好友备注的消息包，格式为
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 页号，1字节，从1开始，如果为0，表示此字段未用
    /// * 4. 操作对象的QQ号，4字节
    /// * 5. 未知1字节，0x00
    /// * 6. 以下为备注信息，一共7个域，域的顺序依次次是
    /// *    姓名、手机、电话、地址、邮箱、邮编、备注
    /// *    每个域都有一个前导字节，这个字节表示了这个域的字节长度
    /// * 7. 尾部
    /// * 
    /// * Note: 如果子命令是0x00(批量下载备注)，只有2，3部分
    /// * 		 如果子命令是0x01(上传备注)，所有部分都要，3部分未用
    /// *       如果子命令是0x02(删除好友)，仅保留1,2,4,7部分
    /// *       如果子命令是0x03(下载备注)，仅保留1,2,4,7部分
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class FriendDataOpPacket : BasicOutPacket
    {
        /// <summary>操作类型，上传还是下载
        /// 	<remark>abu 2008-02-29 </remark>
        /// </summary>
        /// <value></value>
        public FriendOpSubCmd SubCommand { get; set; }
        /// <summary> 操作的对象的QQ号
        /// 	<remark>abu 2008-02-29 </remark>
        /// </summary>
        /// <value></value>
        public int QQ { get; set; }
        /// <summary>好友备注对象
        /// 	<remark>abu 2008-02-29 </remark>
        /// </summary>
        /// <value></value>
        public FriendRemark Remark { get; set; }
        /// <summary>页号
        /// 	<remark>abu 2008-02-29 </remark>
        /// </summary>
        /// <value></value>
        public int Page { get; set; }

        public FriendDataOpPacket(QQClient client)
            : base(QQCommand.Friend_Data_OP_05,true,client)
        {
            SubCommand = FriendOpSubCmd.UPLOAD_FRIEND_REMARK;
            Remark = new FriendRemark();
        }
        public FriendDataOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            switch (SubCommand)
            {
                case FriendOpSubCmd.BATCH_DOWNLOAD_FRIEND_REMARK:
                    return "Friend Data Packet - Batch Download Remark";
                case FriendOpSubCmd.UPLOAD_FRIEND_REMARK:
                    return "Friend Data Packet - Upload Remark";
                case FriendOpSubCmd.REMOVE_FRIEND_FROM_LIST:
                    return "Friend Data Packet - Remove Friend From List";
                case FriendOpSubCmd.DOWNLOAD_FRIEND_REMARK:
                    return "Friend Data Packet - Download Remark";
                default:
                    return "Friend Data Packet - Unknown Sub Command";
            }
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 操作类型
            buf.Put((byte)SubCommand);
            // 未知字节0x0，仅在上传时
            if (SubCommand == FriendOpSubCmd.UPLOAD_FRIEND_REMARK || SubCommand == FriendOpSubCmd.BATCH_DOWNLOAD_FRIEND_REMARK)
                buf.Put((byte)Page);
            // 操作对象的QQ号
            if (SubCommand != FriendOpSubCmd.BATCH_DOWNLOAD_FRIEND_REMARK)
                buf.PutInt(QQ);
            // 后面的内容为一个未知字节0和备注信息，仅在上传时
            if (SubCommand == FriendOpSubCmd.UPLOAD_FRIEND_REMARK)
            {
                buf.Put((byte)0);
                // 备注信息
                // 姓名
                if (string.IsNullOrEmpty(Remark.Name))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Name);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
                // 手机
                if (string.IsNullOrEmpty(Remark.Mobile))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Mobile);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
                // 电话
                if (string.IsNullOrEmpty(Remark.Telephone))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Telephone);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
                // 地址
                if (string.IsNullOrEmpty(Remark.Address))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Address);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
                // 邮箱
                if (string.IsNullOrEmpty(Remark.Email))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Email);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
                // 邮编
                if (string.IsNullOrEmpty(Remark.Zipcode))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Zipcode);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
                // 备注
                if (string.IsNullOrEmpty(Remark.Note))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Note);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
            }
        }
    }
}
