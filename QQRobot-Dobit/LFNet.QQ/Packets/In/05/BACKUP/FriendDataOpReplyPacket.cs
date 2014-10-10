
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 上传下载好友备注的回复包，格式为：
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 如果是0x0，后面的部分为
    /// * 	  a. 是否还有更多备注，0x00表示有，0x01表示无
    /// *    b. QQ号，4字节
    /// *    c. 一个未知字节，0x00
    /// *    d. 名称长度，1字节
    /// *    e. 名称
    /// *    f. 手机号码长度, 1字节
    /// *    g. 手机号码
    /// *    h. 常用电话长度, 1字节
    /// *    i. 常用电话
    /// *    j. 联系地址长度, 1字节
    /// *    k. 联系地址
    /// *    l. 电子邮箱长度, 1字节
    /// *    m. 电子邮箱
    /// *    n. 邮编长度，1字节
    /// *    o. 邮编
    /// *    p. 备注长度，1字节
    /// *    q. 备注
    /// *    r. 如果还有更多，重复b - q部分
    /// *    如果是0x1或者0x02，后面的部分为
    /// *    i. 1字节应答码，0x0表示成功
    /// *    如果是0x3，后面的部分为(后面也可能什么都没有，说明这个好友没有备注)
    /// *    i.   操作对象的QQ号，4字节
    /// *    ii.  一个未知字节0
    /// *    iii. 分隔符0x1
    /// *    iv.  以下为备注信息，一共7个域，域的顺序依次次是
    /// *   	   姓名、手机、电话、地址、邮箱、邮编、备注
    /// *         每个域都有一个前导字节，这个字节表示了这个域的字节长度
    /// * 4. 尾部
    /// 	<remark>abu 2008-02-22 </remark>
    /// </summary>
    public class FriendDataOpReplyPacket : BasicInPacket
    {
        /// <summary>
        /// 应答码，仅用在上传回复
        /// 	<remark>abu 2008-02-22 </remark>
        /// </summary>
        /// <value></value>
        public ReplyCode ReplyCode { get; set; }
        /// <summary>
        /// 操作对象的QQ号
        /// 	<remark>abu 2008-02-22 </remark>
        /// </summary>
        /// <value></value>
        public int QQ { get; set; }
        /// <summary>
        /// 备注信息，仅用在下载时
        /// 	<remark>abu 2008-02-22 </remark>
        /// </summary>
        /// <value></value>
        public FriendRemark Remark { get; set; }
        /// <summary>
        /// 操作字节
        /// 	<remark>abu 2008-02-22 </remark>
        /// </summary>
        /// <value></value>
        public FriendOpSubCmd SubCommand { get; set; }
        /// <summary>
        /// 是否有备注，如果子命令是下载备注，则这个字段表示这个好友是否有备注
        /// 如果子命令是批量下载备注，则这个字段表示是否还有更多的备注可以下载
        /// 	<remark>abu 2008-02-22 </remark>
        /// </summary>
        /// <value></value>
        public bool HasRemark { get; set; }
        /// <summary>仅用在批量下载备注时，key是qq号，value是FriendRemark对象
        /// 	<remark>abu 2008-02-22 </remark>
        /// </summary>
        /// <value></value>
        public Dictionary<uint, FriendRemark> Remarks { get; set; }
        public FriendDataOpReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            switch (SubCommand)
            {
                case FriendOpSubCmd.BATCH_DOWNLOAD_FRIEND_REMARK:
                    return "Friend Data Reply Packet - Batch Download Remark";
                case FriendOpSubCmd.UPLOAD_FRIEND_REMARK:
                    return "Friend Data Reply Packet - Upload Remark";
                case FriendOpSubCmd.DOWNLOAD_FRIEND_REMARK:
                    return "Friend Data Reply Packet - Download Remark";
                case FriendOpSubCmd.REMOVE_FRIEND_FROM_LIST:
                    return "Friend Data Reply Packet - Remove Friend From List";
                default:
                    return "Friend Data Reply Packeet - Unknown Sub Command";
            }
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            //操作字节
            SubCommand = (FriendOpSubCmd)buf.Get();
            switch (SubCommand)
            {
                case FriendOpSubCmd.BATCH_DOWNLOAD_FRIEND_REMARK:
                    HasRemark = buf.Get() == 0x0;
                    Remarks = new Dictionary<uint, FriendRemark>();
                    while (buf.HasRemaining())
                    {
                        uint qq = buf.GetUInt();
                        // 跳过一个未知字节0x0
                        buf.Get();
                        // 创建备注对象 
                        FriendRemark r = new FriendRemark();
                        // 读入备注对象
                        r.Read(buf);
                        // 加入到哈希表
                        Remarks.Add(qq, r);
                    }
                    break;
                case FriendOpSubCmd.UPLOAD_FRIEND_REMARK:
                    break;
                case FriendOpSubCmd.REMOVE_FRIEND_FROM_LIST:
                    ReplyCode = (ReplyCode)buf.Get();
                    break;
                case FriendOpSubCmd.DOWNLOAD_FRIEND_REMARK:
                    if (buf.HasRemaining())
                    {
                        // 读取操作对象的QQ号
                        QQ = buf.GetInt();
                        // 创建备注对象 
                        Remark = new FriendRemark();
                        // 跳过一个未知字节0x0
                        buf.Get();
                        // 读入备注对象
                        Remark.Read(buf);

                        HasRemark = true;
                    }
                    else
                        HasRemark = false;                    
                    break;
                default:
                    break;
            }
        }
    }
}
