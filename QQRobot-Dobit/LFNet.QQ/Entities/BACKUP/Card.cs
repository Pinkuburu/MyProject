
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>群名片
    /// </summary>
    public class Card
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Remark { get; set; }
        public string Email { get; set; }
        public Gender GenderIndex { get; set; }
        public Card()
        {
            Name = "";
            Phone = "";
            Remark = "";
            Email = "";
        }
        /// <summary>
        /// 从字节缓冲区中解析一个群名片结构
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            int len = (int)buf.Get();
            Name = Utils.Util.GetString(buf.GetByteArray(len));
            GenderIndex = (Gender)buf.Get();

            len = (int)buf.Get();
            Phone = Utils.Util.GetString(buf.GetByteArray(len));

            len = (int)buf.Get();
            Email = Utils.Util.GetString(buf.GetByteArray(len));

            len = (int)buf.Get();
            Remark = Utils.Util.GetString(buf.GetByteArray(len));
        }
        /// <summary>
        /// 写入bean的内容到缓冲区中
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void Write(ByteBuffer buf)
        {
            byte[] b = Utils.Util.GetBytes(Name);
            buf.Put((byte)b.Length);
            buf.Put(b);

            buf.Put((byte)GenderIndex);

            b = Utils.Util.GetBytes(Phone);
            buf.Put((byte)b.Length);
            buf.Put(b);

            b = Utils.Util.GetBytes(Email);
            buf.Put((byte)b.Length);
            buf.Put(b);

            b = Utils.Util.GetBytes(Remark);
            buf.Put((byte)b.Length);
            buf.Put(b);
        }
    }
}
