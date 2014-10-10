

using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Utils;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 级用户信息，这个结果由高级搜索返回
    /// </summary>
    public class AdvancedUserInfo
    {
        public uint QQ { get; set; }
        public uint Age { get; set; }
        public uint GenderIndex { get; set; }
        public bool Online { get; set; }
        public string Nick { get; set; }
        public uint ProvinceIndex { get; set; }
        public uint CityIndex { get; set; }
        public uint Face { get; set; }
        /// <summary>给定一个输入流，解析AdvancedUserInfo结构
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadBean(ByteBuffer buf)
        {
            QQ = buf.GetUInt();
            GenderIndex = (uint)buf.Get();
            Age = buf.GetUInt();
            Online = buf.Get() != 0;
            int len = (int)(buf.Get() & 0xFF);
            Nick = Util.GetString(buf.GetByteArray(len));
            ProvinceIndex = buf.GetUInt();
            CityIndex = buf.GetUInt();
            Face = buf.GetUInt();
        }
    }
}
