
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// QQLive描述信息 ，找不到StringTokenizer的替代类，暂时还没有实现
    /// </summary>
    public class QQLive
    {
        public ushort Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public void Read(ByteBuffer buf)
        {
            Type = buf.GetUShort();
            int len = buf.GetUShort() & 0xFF;
            switch (Type)
            {
                case QQGlobal.QQ_LIVE_IM_TYPE_DISK:
                    String s = Utils.Util.GetString(buf, len);
                    //还没实现
                    //StringTokenizer st = new StringTokenizer(s, "\u0002");
                    //if(st.hasMoreTokens())
                    //    title = st.nextToken();
                    //if(st.hasMoreTokens())
                    //    description = st.nextToken();
                    //if(st.hasMoreTokens())
                    //    url = st.nextToken();
                    break;
            }
        }
    }
}
