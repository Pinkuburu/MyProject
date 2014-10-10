using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// QQ2009消息Face
    /// </summary>
    public class NormalIMFace
    {
       public QQClient QQClient { get; set; }
       public int FaceId { get; set; }
       public byte[] FFData { get; set; }
       public byte[] RemainBytes { get; set; }
       public NormalIMFace(QQClient qqClient, byte[] buffer)
       {
           QQClient = qqClient;
           ByteBuffer buf = new ByteBuffer(buffer);
           Read( buf);
       }

       public void Read( ByteBuffer buf)
       {
           while (buf.HasRemaining())
           {
               byte type = buf.Get();
               int len = buf.GetUShort();
               switch (type)
               { 
                   case 0x01:
                       FaceId = buf.Get();
                       break;
                   case 0xff:
                       FFData = buf.GetByteArray(len);
                       break;
                   default:
                       QQClient.LogManager.Log(base.ToString()+" Parse Error,Unknown Type=" + type.ToString("X") + ": Data=" + Utils.Util.ToHex(buf.GetByteArray(len)));
                       break;
                    
               }
           }
           if (buf.HasRemaining())
           {
               RemainBytes = buf.GetByteArray(buf.Remaining());
               QQClient.LogManager.Log(base.ToString() + " Class Parse Buf Remaining Data:" + Utils.Util.ToHex(RemainBytes));
           }
       }

       public override string ToString()
       {
           return "<Face FaceId=\"" + FaceId.ToString() + "\" FFData=\"" + Utils.Util.ToHex(FFData) + "\" />";
       }
    }
}
