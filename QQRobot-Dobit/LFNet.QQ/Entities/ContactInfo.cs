
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

using LFNet.QQ.Utils;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 一个用户的详细信息，全部都是字符串形式，按照QQ请求用户信息应答包中的顺序排列，一共37项
    /// </summary>
    public class ContactInfo
    {
        public static string DIVIDER = System.Text.Encoding.Default.GetString(new byte[] { (byte)30 });
        /// <summary>
        /// 1. QQ号
        /// </summary>
        private const int QQ_NUM = 0;
        /// <summary>
        /// 2. 昵称
        /// </summary>
        private const int NICK = 1;
        /// <summary>
        /// 3. 国家
        /// </summary>
        private const int COUNTRY = 2;
        /// <summary>
        /// 4. 省
        /// </summary>
        private const int PROVINCE = 3;
        /// <summary>
        /// 5. 邮政编码
        /// </summary>
        private const int ZIPCODE = 4;
        /// <summary>
        /// 6. 地址
        /// </summary>
        private const int ADDRESS = 5;
        /// <summary>
        /// 7. 电话
        /// </summary>
        private const int TELEPHONE = 6;
        /// <summary>
        /// 8. 年龄
        /// </summary>
        private const int AGE = 7;
        /// <summary>
        /// 9. 性别
        /// </summary>
        private const int GENDER = 8;
        /// <summary>
        /// 10. 姓名
        /// </summary>
        private const int NAME = 9;
        /// <summary>
        /// 11. Email
        /// </summary>
        private const int EMAIL = 10;
        /// <summary>
        /// 12. 寻呼机sn，（sn是什么玩意，我也不知道）
        /// </summary>
        private const int PAGER_SN = 11;
        /// <summary>
        /// 13. 寻呼机号
        /// </summary>
        private const int PAGER = 12;
        /// <summary>
        /// 14. 寻呼机服务提供商
        /// </summary>
        private const int PAGER_SP = 13;
        /// <summary>
        /// 15. 寻呼机base num（也不清楚这是什么）
        /// </summary>
        private const int PAGER_BASE_NUM = 14;
        /// <summary>
        /// 16. 寻呼机类型
        /// </summary>
        private const int PAGER_TYPE = 15;
        /// <summary>
        /// 17. 职业
        /// </summary>
        private const int OCCUPATION = 16;
        /// <summary>
        /// 18. 主页
        /// </summary>
        private const int HOMEPAGE = 17;
        /// <summary>
        /// 19. 认证类型（应该是被人加自己为好友的时候的认证类型把）
        /// </summary>
        private const int AUTH_TYPE = 18;
        /// <summary>
        /// 20. unknown 1
        /// </summary>
        private const int UNKNOWN_1 = 19;
        /// <summary>
        /// 21. unknown 2
        /// </summary>
        private const int UNKNOWN_2 = 20;
        /// <summary>
        /// 22. 头像，头像是用一个数代表的，比如27, 因为QQ目录下的头像是从1开始编号的，
        ///     但是这个头像的数字却是从0开始计数的。并且注意，QQ的目录下面每种头像都
        ///    有3个bmp，所以按数字这么一排，27应该是10-1.bmp
        /// </summary>
        private const int HEAD = 21;
        /// <summary>
        /// 23. 手机号
        /// </summary>
        private const int MOBILE = 22;
        /// <summary>
        /// 24. 手机类型
        /// </summary>
        private const int MOBILE_TYPE = 23;
        /// <summary>
        /// 25. 介绍
        /// </summary>
        private const int INTRO = 24;
        /// <summary>
        /// 26. 城市
        /// </summary>
        private const int CITY = 25;
        /// <summary>
        /// 27. unknown 3
        /// </summary>
        private const int UNKNOWN_3 = 26;
        /// <summary>
        /// 28. unknown 4
        /// </summary>
        private const int UNKNOWN_4 = 27;
        /// <summary>
        /// 29. unknown 5
        /// </summary>
        private const int UNKNOWN_5 = 28;
        /// <summary>
        /// 30. is_open_hp
        /// </summary>
        private const int OPEN_HP = 29;
        /// <summary>
        /// 31. is_open_contact（通讯方式是否对其他人可见）
        /// </summary>
        private const int OPEN_CONTACT = 30;
        /// <summary>
        /// 32. 学校
        /// </summary>
        private const int COLLEGE = 31;
        /// <summary>
        /// 33. 星座
        /// </summary>
        private const int HOROSCOPE = 32;
        /// <summary>
        /// 34. 生肖
        /// </summary>
        private const int ZODIAC = 33;
        /// <summary>
        /// 35. 血型
        /// </summary>
        private const int BLOOD = 34;
        /// <summary>
        /// 36. UserFlag
        /// </summary>
        private const int USER_FLAG = 35;
        /// <summary>
        /// 37. unknown 6，总是0x2D
        /// </summary>
        private const int UNKNOWN_6 = 36;

        /// <summary>
        /// 1. QQ号
    /// </summary>
        /// <value></value>
        public int QQ { get; set; }
        /// <summary>
        /// 2. 昵称
    /// </summary>
        /// <value></value>
        public string Nick { get; set; }
        /// <summary>
        /// 3. 国家
    /// </summary>
        /// <value></value>
        public string Country { get; set; }
        /// <summary>
        /// 4. 省
    /// </summary>
        /// <value></value>
        public string Province { get; set; }
        /// <summary>
        /// 5. 邮政编码
    /// </summary>
        /// <value></value>
        public string ZipCode { get; set; }
        /// <summary>
        /// 6. 地址
    /// </summary>
        /// <value></value>
        public string Address { get; set; }
        /// <summary>
        /// 7. 电话
    /// </summary>
        /// <value></value>
        public string Telephone { get; set; }
        /// <summary>
        /// 8. 年龄
    /// </summary>
        /// <value></value>
        public int Age { get; set; }
        /// <summary>
        /// 9. 性别
    /// </summary>
        /// <value></value>
        public string Gender { get; set; }
        /// <summary>
        /// 10. 姓名
    /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 11. Email
    /// </summary>
        /// <value></value>
        public string Email { get; set; }
        /// <summary>
        /// 17. 职业
    /// </summary>
        /// <value></value>
        public string Occupation { get; set; }
        /// <summary>
        /// 18. 主页
    /// </summary>
        /// <value></value>
        public string HomePage { get; set; }
        /// <summary>
        /// 19. 认证类型（应该是被人加自己为好友的时候的认证类型把）
    /// </summary>
        /// <value></value>
        public AuthType AuthType { get; set; }
        /// <summary>
        /// 22. 头像，头像是用一个数代表的，比如27, 因为QQ目录下的头像是从1开始编号的，
        ///     但是这个头像的数字却是从0开始计数的。并且注意，QQ的目录下面每种头像都
        ///     有3个bmp，所以按数字这么一排，27应该是10-1.bmp
    /// </summary>
        /// <value></value>
        public int Head { get; set; }
        /// <summary>
        /// 23. 手机号
    /// </summary>
        /// <value></value>
        public string Mobile { get; set; }
        /// <summary>
        /// 25. 介绍
    /// </summary>
        /// <value></value>
        public string Intro { get; set; }
        /// <summary>
        /// 26. 城市
    /// </summary>
        /// <value></value>
        public string City { get; set; }
        /// <summary>
        /// 31. is_open_contact（通讯方式是否对其他人可见）
    /// </summary>
        /// <value></value>
        public OpenContact OpenContact { get; set; }
        /// <summary>
        /// 32. 学校
    /// </summary>
        /// <value></value>
        public string College { get; set; }
        /// <summary>
        /// 33. 星座
    /// </summary>
        /// <value></value>
        public int Horoscope { get; set; }
        /// <summary>
        /// 34. 生肖
    /// </summary>
        /// <value></value>
        public int Zodiac { get; set; }
        /// <summary>
        /// 35. 血型
    /// </summary>
        /// <value></value>
        public int Blood { get; set; }
        /// <summary>
        /// 36. UserFlag
    /// </summary>
        /// <value></value>
        public int UserFlag { get; set; }

        /// <summary>
        /// 字段数
    /// </summary>
        /// <value></value>
        public int FieldCount { get; set; }
        /// <summary>
        /// 原始信息数组
        /// </summary>
        private string[] infos;
        public ContactInfo()
        {
            this.Nick = string.Empty;
            this.Country = string.Empty;
            this.Province = string.Empty;
            this.ZipCode = string.Empty;
            this.Address = string.Empty;
            this.Telephone = string.Empty;
            this.Name = string.Empty;
            this.Email = string.Empty;
            this.Occupation = string.Empty;
            this.HomePage = string.Empty;
            this.Mobile = string.Empty;
            this.Intro = string.Empty;
            this.City = string.Empty;
            this.College = string.Empty;
            this.Gender = string.Empty;
            AuthType = AuthType.NeedAuth;
            OpenContact = OpenContact.Friends;
            FieldCount = QQGlobal.QQ_COUNT_GET_USER_INFO_FIELD;
        }
        public ContactInfo(ByteBuffer buf)
        {
            string s = Utils.Util.GetString(buf);
            infos = s.Split(DIVIDER.ToCharArray());
            FieldCount = infos.Length;

            QQ = Utils.Util.GetInt(infos[QQ_NUM], 0);
            //LumaQQ中还有过滤不可打印字符
            Nick = infos[NICK];
            Country = infos[COUNTRY];
            Province = infos[PROVINCE];
            ZipCode = infos[ZIPCODE];
            Address = infos[ADDRESS];
            Telephone = infos[TELEPHONE];
            Age = Util.GetInt(infos[AGE], 0);
            Gender = infos[GENDER];
            Name = infos[NAME];
            Email = infos[EMAIL];
            Occupation = infos[OCCUPATION];
            HomePage = infos[HOMEPAGE];
            AuthType = (AuthType)Util.GetInt(infos[AUTH_TYPE], (int)AuthType.NeedAuth);
            Head = Util.GetInt(infos[HEAD], 0);
            Mobile = infos[MOBILE];
            Intro = infos[INTRO];
            City = infos[CITY];
            OpenContact = (OpenContact)Util.GetInt(infos[OPEN_CONTACT], (int)OpenContact.Friends);
            College = infos[COLLEGE];
            Horoscope = Util.GetInt(infos[HOROSCOPE], 0);
            Zodiac = Util.GetInt(infos[ZODIAC], 0);
            Blood = Util.GetInt(infos[BLOOD], 0);
            UserFlag = Util.GetInt(infos[USER_FLAG], 0);
        }
        /// <summary>
        /// 得到信息字符串数组
    /// </summary>
        /// <returns></returns>
        public string[] GetInfoArray()
        {
            if (infos == null)
            {
                CreatInfoArray();
            }
            return infos;
        }
        private void CreatInfoArray()
        {
            infos = new string[QQGlobal.QQ_COUNT_GET_USER_INFO_FIELD];
            infos[QQ_NUM] = QQ.ToString();
            infos[NICK] = Nick;
            infos[COUNTRY] = Country;
            infos[PROVINCE] = Province;
            infos[ZIPCODE] = ZipCode;
            infos[ADDRESS] = Address;
            infos[TELEPHONE] = Telephone;
            infos[AGE] = Age.ToString();
            infos[GENDER] = Gender;

            // 10. 姓名
            infos[NAME] = Name;
            // 11. Email
            infos[EMAIL] = Email;
            // 12. 寻呼机sn，（sn是什么玩意，我也不知道）
            infos[PAGER_SN] = "";
            // 13. 寻呼机号
            infos[PAGER] = "";
            // 14. 寻呼机服务提供商
            infos[PAGER_SP] = "";
            // 15. 寻呼机base num（也不清楚这是什么）
            infos[PAGER_BASE_NUM] = "";
            // 16. 寻呼机类型
            infos[PAGER_TYPE] = "";
            // 17. 职业
            infos[OCCUPATION] = Occupation;
            // 18. 主页
            infos[HOMEPAGE] = HomePage;
            // 19. 认证类型（应该是被人加自己为好友的时候的认证类型把）
            infos[AUTH_TYPE] = ((int)AuthType).ToString();
            // 20. unknown 1
            infos[UNKNOWN_1] = "";
            // 21. unknown 2
            infos[UNKNOWN_2] = "";
            // 22. 头像，头像是用一个数代表的，比如27, 因为QQ目录下的头像是从1开始编号的，
            //     但是这个头像的数字却是从0开始计数的。并且注意，QQ的目录下面每种头像都
            //     有3个bmp，所以按数字这么一排，27应该是10-1.bmp
            infos[HEAD] = Head.ToString();
            // 23. 手机号
            infos[MOBILE] = Mobile;
            // 24. 手机类型 
            infos[MOBILE_TYPE] = "";
            // 25. 介绍
            infos[INTRO] = Intro;
            // 26. 城市
            infos[CITY] = City;
            // 27. unknown 3
            infos[UNKNOWN_3] = "";
            // 28. unknown 4
            infos[UNKNOWN_4] = "";
            // 29. unknown 5
            infos[UNKNOWN_5] = "";
            // 30. is_open_hp
            infos[OPEN_HP] = "";
            // 31. is_open_contact（通讯方式是否对其他人可见）
            infos[OPEN_CONTACT] = ((int)OpenContact).ToString();
            // 32. 学校
            infos[COLLEGE] = College;
            // 33. 星座
            infos[HOROSCOPE] = Horoscope.ToString();
            // 34. 生肖
            infos[ZODIAC] = Zodiac.ToString();
            // 35. 血型
            infos[BLOOD] = Blood.ToString();
            // 36. UserFlag
            infos[USER_FLAG] = UserFlag.ToString();
            // 37. unknown 6，总是0x2D
            infos[UNKNOWN_6] = "";
        }
    }
}
