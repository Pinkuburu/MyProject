
#region 版权声明
//=========================================================== 
// 版权声明：LFNet.QQ是基于QQ2009版本的QQ协议开发而成的，协议
// 分析主要参考自小虾的MyQQ(C++)源代码，代码开发主要基于阿布
// 的LumaQQ.NET的C#.NET代码修改而成，故继续遵照使用LumaQQ的开
// 源协议。
//
// 本人没有对LumaQQ.NET的C#.NET代码的框架做过多的改动，主
// 要工作为将MyQQ的C++协议代码部分翻译成符合LumaQQ.Net框架
// 的C#代码，故请尊重LumaQQ作者Luma的著作权和版权声明。
// 
// 代码开源主要用于解决大家在学习和研究协议过程中遇到由于缺乏代码所带来的制约性问题。
// 本代码仅供学习交流使用，大家在使用此开发包前请自行协调好多方面关系，
// 不得用于任何商业用途和非法用途，本人不享受和承担由此产生的任何权利以及任何法律责任。
// 
// 本源代码可通过以下网址获取:
// http://QQCode.lynfo.com, http://www.lynfo.com, http://bbs.lynfo.com, http://hi.baidu.com/dobit.
//
// Copyright @ 2009-2010  Lynfo.com.  All Rights Reserved.   
// Framework: 2.0
// Author: Luma(java版) → Abu(C# QQ2005协议版) → Dobit(C# QQ2009协议版本)
// Email: dobit@msn.cn   
// Created: 2009-3-1~ 2009-11-28
// Last Modified:2009-11-28    
//   
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details. 
//===========================================================   
#endregion

using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// * 天气操作回复包
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 回复码，1字节
    /// * 4. 省名称字节长度，1字节
    /// * 5. 省
    /// * 6. 市名称字节长度，1字节
    /// * 7. 市
    /// * Note: 如果4部分为0，则可以认为腾讯无法找到你要的天气预报信息，不应再往下解析
    /// * 8. 未知的2字节
    /// * 9. 市(2)名称字节长度，1字节
    /// * 10. 市(2)
    /// * Note: 不明白为什么有两个市，这两个市有时候都有内容，有时候只有一个，要注意各种情况
    /// * 11. 预报的天数，1字节，如果72小时预报，这个就是0x03
    /// * 12. 时间，4字节，天气数据的开始时间
    /// * 13. 天气情况字节长度，1字节
    /// * 14. 天气情况
    /// * 15. 风向字节长度，1字节
    /// * 16. 风向
    /// * 17. 最低温度，2字节，单位是摄氏度
    /// * 18. 最高温度，2字节，单位是摄氏度
    /// * Note: 要注意温度为零下时，是负数，用java处理时要注意转换
    /// * 19. 未知的1字节
    /// * 20. 提示字节长度，1字节
    /// * 21. 提示
    /// * 22. 如果还有更多数据，重复12-21部分
    /// * 23. 未知的2字节
    /// * 24. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class WeatherOpReplyPacket : BasicInPacket
    {
        public byte SubCommand { get; set; }
        public ReplyCode ReplyCode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public List<Weather> Weathers { get; set; }
        public WeatherOpReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void ParseBody(ByteBuffer buf)
        {
            SubCommand = buf.Get();
            ReplyCode = (ReplyCode)buf.Get();

            int len = buf.Get() & 0xFF;
            if (len == 0)
                return;
            Province = Utils.Util.GetString(buf, len);

            len = buf.Get() & 0xFF;
            if (len > 0)
            {
                City = Utils.Util.GetString(buf, len);
                buf.GetUShort();
                len = buf.Get() & 0xFF;
                buf.Position  = buf.Position + len;
            }
            else
            {
                buf.GetUShort();
                len = buf.Get() & 0xFF;
                City = Utils.Util.GetString(buf, len);
            }
            int count = buf.Get() & 0xFF;

            Weathers = new List<Weather>();
            while (count-- > 0)
            {
                Weather w = new Weather();
                w.Read(buf);
                Weathers.Add(w);
            }
        }
    }
}
