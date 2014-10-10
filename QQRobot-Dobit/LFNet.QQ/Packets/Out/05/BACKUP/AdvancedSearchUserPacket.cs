
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 高级搜索用户的请求包：
    /// * 1. 头部
    /// * 2. 页数，从0开始，2字节
    /// * 3. 在线与否，1字节，0x01表示在线，0x00表示不在线
    /// * 4. 是否有摄像头，1字节，0x01表示有，0x00表示无，TX QQ 2004中的处理是如果要查找
    /// *    有摄像头的用户，则必须查找在线用户，不知道不这样行不行
    /// * 5. 年龄，1字节，表示在下拉框中的索引
    /// * 6. 性别，1字节，表示在下拉框中的索引
    /// * 7. 省份，2字节，表示在下拉框中的索引
    /// * 8. 城市，2字节，表示在下拉框中的索引
    /// * 9. 尾部
    /// 	<remark>abu 2008-02-27 </remark>
    /// </summary>
    public class AdvancedSearchUserPacket : BasicOutPacket
    {
        public bool SearchOnline { get; set; }
        public bool HasCam { get; set; }
        public ushort Page { get; set; }
        public byte AgeIndex { get; set; }
        public byte GenderIndex { get; set; }
        public ushort ProvinceIndex { get; set; }
        public ushort CityIndex { get; set; }

        public AdvancedSearchUserPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public AdvancedSearchUserPacket(QQClient client)
            : base(QQCommand.Advanced_Search_05,true,client)
        {
            SearchOnline = true;
            HasCam = false;
            Page = ProvinceIndex = CityIndex = 0;
            AgeIndex = GenderIndex = 0;
        }
        public override string GetPacketName()
        {
            return "Advanced Search Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 2. 页数，从0开始
            buf.PutUShort(Page);
            // 3. 在线与否，1字节，0x01表示在线，0x00表示不在线
            buf.Put(SearchOnline ? (byte)0x01 : (byte)0x00);
            // 4. 是否有摄像头，1字节，0x01表示有，0x00表示无，TX QQ 2004中的处理是如果要查找
            //   有摄像头的用户，则必须查找在线用户，不知道不这样行不行
            buf.Put(HasCam ? (byte)0x01 : (byte)0x00);
            // 5. 年龄，1字节，表示在下拉框中的索引
            buf.Put(AgeIndex);
            // 6. 性别，1字节，表示在下拉框中的索引
            buf.Put(GenderIndex);
            // 7. 省份，2字节，表示在下拉框中的索引
            buf.PutUShort(ProvinceIndex);
            // 8. 城市，2字节，表示在下拉框中的索引
            buf.PutUShort(CityIndex);
        }
    }
}
