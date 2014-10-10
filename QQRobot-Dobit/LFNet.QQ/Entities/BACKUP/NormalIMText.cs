using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 0x01文本
    /// 分析QQ2009消息中的文本的实体
    /// </summary>
   public class NormalIMText
    {
       public QQClient QQClient { get; set; }
       public string Text { get; set; }
       public byte[] RemainBytes { get; set; }
       public NormalIMText(QQClient qqClient,  byte[] buffer)
       {
           QQClient = qqClient;
           ByteBuffer buf = new ByteBuffer(buffer);
           Read( buf);
       }

       public void Read(ByteBuffer buf)
       {
           buf.Get();//0x01
           int len=buf.GetUShort();
           Text = Utils.Util.GetString(buf.GetByteArray(len));
           if (buf.HasRemaining())
           {
               RemainBytes = buf.GetByteArray(buf.Remaining());
               QQClient.LogManager.Log("NormalIMText Class Parse Buf Remaining Data:" + Utils.Util.ToHex(RemainBytes));
           }
       }

       public override string ToString()
       {
           return Text;
       }
    }
}
