using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{

    /// <summary>
    /// QQ2009 聊天消息 自定义图片
    /// </summary>
   public class NormalIMImage
    {
       public QQClient QQClient { get; set; }
       public string FileName { get; set; }
       /// <summary>
       /// 图片大小字节
       /// </summary>
       public int FileSize { get; set; }
       /// <summary>
       /// 文件guid /f92df54d-89e6-4683-a5d5-870705f54f49
       /// </summary>
       public string FGuid { get; set; }
       public byte[] FFData { get; set; }
       public byte[] RemainBytes { get; set; }
       public NormalIMImage(QQClient qqClient, byte[] buffer)
       {
           QQClient = qqClient;
           ByteBuffer buf = new ByteBuffer(buffer);
           Read( buf);
       }

       public void Read(ByteBuffer buf)
       {
           while (buf.HasRemaining())
           {
               byte type = buf.Get();
               int len = buf.GetUShort();
               switch (type)
               { 
                   case 0x02:
                       FileName = Utils.Util.GetString(buf.GetByteArray(len));
                       break;
                   case 0x03://filesize
                       FileSize =(int) Utils.Util.GetUInt(buf.GetByteArray(len), 0, 4); //buf.GetInt();
                       break;
                   case 0x04://guid
                       FGuid = Utils.Util.GetString(buf.GetByteArray(len));
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
           return "<Image FileName=\"" + FileName + "\" FileSize=\"" + FileSize.ToString() + "\" FGuid=\"" + FGuid + "\" FFData=\"" + Utils.Util.ToHex(FFData) + "\" />";
       }
    }
}
