
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>在线用户的结构表示
    /// </summary>
    public class UserInfo
    {
        public int QQ { get; set; }
        public string Nick { get; set; }
        public string Province { get; set; }
        public int Face { get; set; }
        /// <summary>
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            ByteBuffer temp = new ByteBuffer();
            int i = 0;
            while (true)
            {
                byte b = buf.Get();
                if (b != 0x1F)
                {
                    if (b != 0x1E)
                    {
                        temp.Put(b);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            QQ = Utils.Util.GetInt(Utils.Util.GetString(temp.ToByteArray()), 0000);
                        }
                        else if (i == 1)
                            Nick = Utils.Util.GetString(temp.ToByteArray());
                        else if (i == 2)
                            Province = Utils.Util.GetString(temp.ToByteArray());
                        i++;
                        temp.Initialize();
                    }
                }
                else
                {
                    Face = Utils.Util.GetInt(Utils.Util.GetString(temp.ToByteArray()), 0);
                    break;
                }
            }
        }
    }
}
