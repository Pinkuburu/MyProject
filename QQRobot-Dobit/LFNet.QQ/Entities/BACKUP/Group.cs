using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// QQ好友分组
    /// </summary>
   public class Group
    {
       public int GroupId { get; set; }
       public string Name { get; set; }
       public Group()
       { 
       }
       public Group(ByteBuffer buf)
       {
           Read(buf);
       }
       /// <summary>
       /// 给定一个字节流，解析DownloadFriendEntry结构
       /// </summary>
       /// <param name="buf">The buf.</param>
       public void Read(ByteBuffer buf)
       {
           this.GroupId =(int) buf.Get();
           int len = (int)buf.GetUShort();
           this.Name = Utils.Util.GetString(buf.GetByteArray(len));
       }
       public override string ToString()
       {
           if (GroupId != 0)
           {
               return "GroupId:" + GroupId + " Group:" + Name;
           }
           else
               return base.ToString();
       }
    }
}
