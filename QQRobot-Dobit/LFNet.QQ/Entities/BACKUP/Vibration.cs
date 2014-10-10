using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
   public class Vibration
    {
       /// <summary>
       /// 输入状态
       /// </summary>
       public bool IsInputState { get; set; }
       public bool IsShake { get; set; }
       public void Read(ByteBuffer buf)
       {
           //振动 00 00 00 01 00 09 41 A1 34 00 00 00 00
           //输入 00 00 00 01
           buf.GetInt();// 00 00 00 01
           if (buf.HasRemaining())
           {
               if (buf.GetLong() == (long)0x000941A134000000)
               {
                   IsShake = true;
               }
           }
           else
           {
               IsInputState = true;
           }
       
       }
    }
}
