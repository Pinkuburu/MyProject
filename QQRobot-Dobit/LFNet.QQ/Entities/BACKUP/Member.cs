
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 得到群信息中的成员信息
    /// </summary>
    public class Member
    {
        public uint QQ { get; set; }
        public uint Organization { get; set; }
        public byte Role { get; set; }
        public void Read(ByteBuffer buf)
        {
            QQ = buf.GetUInt();
            Organization = buf.Get();
            Role = buf.Get();
        }
        public void ReadTemp(ByteBuffer buf)
        {
            QQ = buf.GetUInt();
            Organization = buf.Get();
            Role = 0;
        }
        /// <summary>
        /// 是否为管理员
        /// 
        /// 根据Role的值，最好改为枚举
    /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            return (Role & QQGlobal.QQ_ROLE_ADMIN) != 0;
        }
        /// <summary>
        /// 是否为股东
    /// </summary>
        public bool IsStockHolder()
        {
            return (Role & QQGlobal.QQ_ROLE_STOCKHOLDER) != 0;
        }
    }
}
