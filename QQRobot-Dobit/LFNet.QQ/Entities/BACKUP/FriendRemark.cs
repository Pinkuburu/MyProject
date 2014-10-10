
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 存放好友的备注信息
    /// </summary>
    public class FriendRemark
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Zipcode { get; set; }
        public string Note { get; set; }
        public FriendRemark()
        {
            Name = Mobile = Telephone = Address = Email = Zipcode = Note = "";
        }
        public void Read(ByteBuffer buf)
        {
            for (int i = 0; i < QQGlobal.QQ_COUNT_REMARK_FIELD; i++)
            {
                //判断字段是否存在
                int len = (int)buf.Get();
                if (len > 0)
                {
                    string s = Utils.Util.GetString(buf.GetByteArray(len));
                    //根据i的值赋值
                    switch (i)
                    {
                        case 0:
                            Name = s;
                            break;
                        case 1:
                            Mobile = s;
                            break;
                        case 2:
                            Telephone = s;
                            break;
                        case 3:
                            Address = s;
                            break;
                        case 4:
                            Email = s;
                            break;
                        case 5:
                            Zipcode = s;
                            break;
                        case 6:
                            Note = s;
                            break;
                    }
                }
            }
        }
    }
}
