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
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web;
using System.Net;
using System.IO;

namespace LFNet.QQ.Utils
{
    public static class Util
    {
        static Encoding DefaultEncoding = Encoding.GetEncoding(QQGlobal.QQ_CHARSET_DEFAULT);
        static DateTime baseDateTime = DateTime.Parse("1970-1-01 00:00:00.000");
        public static Random Random = new Random();
        /// <summary>
        /// 把字节数组从offset开始的len个字节转换成一个unsigned int，
    /// </summary>
        /// <param name="inData">字节数组</param>
        /// <param name="offset">从哪里开始转换.</param>
        /// <param name="len">转换长度, 如果len超过8则忽略后面的.</param>
        /// <returns></returns>
        public static uint GetUInt(byte[] inData, int offset, int len)
        {
            uint ret = 0;
            int end = 0;
            if (len > 8)
                end = offset + 8;
            else
                end = offset + len;
            for (int i = 0; i < end; i++)
            {
                ret <<= 8;
                ret |= (uint)inData[i];
            }
            return ret;
        }

        /// <summary>
        /// 根据某种编码方式将字节数组转换成字符串
    /// </summary>
        /// <param name="b">字节数组</param>
        /// <param name="encoding">encoding 编码方式</param>
        /// <returns> 如果encoding不支持，返回一个缺省编码的字符串</returns>
        public static string GetString(byte[] b, string encoding)
        {
            try
            {
                return Encoding.GetEncoding(encoding).GetString(b);
            }
            catch
            {
                return Encoding.Default.GetString(b);
            }
        }

        /// <summary>
        /// 根据缺省编码将字节数组转换成字符串
    /// </summary>
        /// <param name="b">字节数组</param>
        /// <returns>字符串</returns>
        public static string GetString(byte[] b)
        {
            return GetString(b, QQGlobal.QQ_CHARSET_DEFAULT);
        }
        /// <summary>
        /// * 从buf的当前位置解析出一个字符串，直到碰到了buf的结尾
        /// * <p>
        /// * 此方法不负责调整buf位置，调用之前务必使buf当前位置处于字符串开头。在读取完成
        /// * 后，buf当前位置将位于buf最后之后
        /// * </p>
        /// * <p>
        /// * 返回的字符串将使用QQ缺省编码，一般来说就是GBK编码
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <returns></returns>
        public static string GetString(ByteBuffer buf)
        {
            ByteBuffer temp = new ByteBuffer();
            while (buf.HasRemaining())
            {
                temp.Put(buf.Get());
            }
            return GetString(temp.ToByteArray());
        }
        /// <summary>从buf的当前位置解析出一个字符串，直到碰到了buf的结尾或者读取了len个byte之后停止
        /// 此方法不负责调整buf位置，调用之前务必使buf当前位置处于字符串开头。在读取完成
        /// * 后，buf当前位置将位于len字节之后或者最后之后
    /// </summary>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static string GetString(ByteBuffer buf, int len)
        {
            ByteBuffer temp = new ByteBuffer();
            while (buf.HasRemaining() && len-- > 0)
            {
                temp.Put(buf.Get());
            }
            return GetString(temp.ToByteArray());
        }

        /// <summary>
        /// * 从buf的当前位置解析出一个字符串，直到碰到了delimit或者读取了maxLen个byte或者
        /// * 碰到结尾之后停止
        /// *此方法不负责调整buf位置，调用之前务必使buf当前位置处于字符串开头。在读取完成
        /// *后，buf当前位置将位于maxLen之后
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="delimit">The delimit.</param>
        /// <param name="maxLen">The max len.</param>
        /// <returns></returns>
        public static String GetString(ByteBuffer buf, byte delimit, int maxLen)
        {
            ByteBuffer temp = new ByteBuffer();
            while (buf.HasRemaining() && maxLen-- > 0)
            {
                byte b = buf.Get();
                if (b == delimit)
                    break;
                else
                    temp.Put(b);
            }
            while (buf.HasRemaining() && maxLen-- > 0)
                buf.Get();
            return GetString(temp.ToByteArray());
        }
        /// <summary>根据某种编码方式将字节数组转换成字符串
    /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="len">The len.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string GetString(byte[] b, int offset, int len)
        {
            byte[] temp = new byte[len];
            Array.Copy(b, offset, temp, 0, len);
            return GetString(temp);
        }

        /// <summary>
        /// 从buf的当前位置解析出一个字符串，直到碰到一个分隔符为止，或者到了buf的结尾
        /// 此方法不负责调整buf位置，调用之前务必使buf当前位置处于字符串开头。在读取完成
        /// * 后，buf当前位置将位于分隔符之后
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="delimit">The delimit.</param>
        /// <returns></returns>
        public static string GetString(ByteBuffer buf, byte delimit)
        {
            ByteBuffer temp = new ByteBuffer();
            while (buf.HasRemaining())
            {
                byte b = buf.Get();
                if (b == delimit)
                    return GetString(temp.ToByteArray());
                else
                    buf.Put(b);
            }
            return GetString(temp.ToByteArray());
        }
        /// <summary>
        /// 生成一个随机Sequence
        /// </summary>
        /// <returns></returns>
        public static byte[] GetSequence(int lenth)
        {
            byte[] bs = new byte[lenth];
            new Random().NextBytes(bs);
            return bs;
        }
        /// <summary>
        /// 生成一个2字节的随机Sequence
        /// </summary>
        /// <returns></returns>
        public static byte[] GetSequence()
        {
            return GetSequence(2);
        }
        /// <summary>
        /// 把字符串转换成int
    /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="defaultValue">如果转换失败，返回这个值</param>
        /// <returns></returns>
        public static int GetInt(string s, int defaultValue)
        {
            int value;
            if (int.TryParse(s, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 字符串转二进制字数组
    /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static byte[] GetBytes(string s)
        {
            return DefaultEncoding.GetBytes(s);
        }
        /// <summary>
        /// 16进制字符串转byte[]
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public static byte[] HexStrToBytes(string hexStr)
        {
            hexStr = hexStr.Replace("0x", "");
            int len = hexStr.Length;
            byte[] ret=new byte[len/2];
            for(int i=0;i<len;i+=2)
            {
                ret[i / 2] = Convert.ToByte(hexStr.Substring(i, 2), 16);
            }
            return ret;
        }
        /// <summary>一个随机产生的密钥字节数组
        /// </summary>
        /// <returns></returns>
        public static byte[] RandomKey()
        {
            byte[] key = new byte[QQGlobal.QQ_LENGTH_KEY];
            (new Random()).NextBytes(key);
            return key;
        }


        /// <summary>这个不是用于调试的，真正要用的方法
    /// </summary>
        /// <param name="encoding">编码方式.</param>
        /// <returns>编码方式的字符串表示形式</returns>
        public static String GetEncodingString(Charset encoding)
        {
            switch (encoding)
            {
                case Charset.GB:
                    return "GBK";
                case Charset.EN:
                    return "ISO-8859-1";
                case Charset.BIG5:
                    return "BIG5";
                default:
                    return "GBK";
            }
        }

        /// <summary>
        /// 用于代替 System.currentTimeMillis()
    /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static long GetTimeMillis(DateTime dateTime)
        {
            return (long)(dateTime - baseDateTime).TotalMilliseconds;
        }
        /// <summary>
        /// 根据服务器返回的毫秒表示的日期，获得实际的日期
        /// Gets the date time from millis.
        /// 似乎服务器返回的日期要加上8个小时才能得到正确的 +8 时区的登录时间
        /// </summary>
        /// <param name="millis">The millis.</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromMillis(long millis)
        {
            return baseDateTime.AddTicks(millis * TimeSpan.TicksPerMillisecond).AddHours(8);
        }

        /// <summary>判断IP是否全0
    /// </summary>
        /// <param name="ip">The ip.</param>
        /// <returns></returns>
        public static bool IsIPZero(byte[] ip)
        {
            for (int i = 0; i < ip.Length; i++)
            {
                if (ip[i] != 0)
                    return false;
            }
            return true;
        }
        /// <summary>ip的字节数组形式转为字符串形式的ip
    /// </summary>
        /// <param name="ip">The ip.</param>
        /// <returns></returns>
        public static String GetIpStringFromBytes(byte[] ip)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ip[0] & 0xFF);
            sb.Append('.');
            sb.Append(ip[1] & 0xFF);
            sb.Append('.');
            sb.Append(ip[2] & 0xFF);
            sb.Append('.');
            sb.Append(ip[3] & 0xFF);
            return sb.ToString();
        }

        public static byte[] IpToBytes(string ip) 
        {
            byte[] bytes = new byte[4];
            string[] s = ip.Split('.');
            if (s.Length == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    bytes[i] = (byte)int.Parse(s[i]);
                }
                
            }
            return bytes;
        }
        /// <summary>转为16进制字符串
    /// </summary>
        /// <param name="bs">The bs.</param>
        /// <returns></returns>
        public static string ToHex(byte[] bs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("X2") + " ");
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        /// <summary>
        /// 文体信息中的表情数据处理
        /// <author>蓝色的风之精灵</author>
    /// </summary>
        /// <param name="IMBytes">The IM bytes.</param>
        /// <returns></returns>
        internal static string AnalyseMessage(byte[] IMBytes)
        {
            List<byte> al = new List<byte>();
            List<string> Faces = new List<string>();
            byte[] tempBytes;
            int bytesSize = 0;
            int shortcutSize = 0;
            bool FaceOrPic = true;//true是自定义表情，false是截屏
            string FaceName = "";
            byte[] FaceNameBytes;
            byte[] facebytes;
            al.AddRange(IMBytes);

            for (int i = 0; i < al.Count - 1; i++)
            {

                if ((FaceType)al[i] == FaceType.DEFAULT && (byte)al[i + 1] >= 0x40 && (byte)al[i + 1] <= 0xC7)//QQ的表情符号是0x14开头的，下一字节表示表情索引号。0x15开头的是自定义表情。这里处理系统表情，以免乱码。
                {
                    string face = string.Format("[face{0}.gif]", al[i + 1].ToString());
                    facebytes = Encoding.GetEncoding("GBK").GetBytes(face);
                    al.RemoveRange(i, 2);
                    al.InsertRange(i, facebytes);
                    i += facebytes.Length - 1;
                    continue;
                }

                if ((FaceType)al[i] == FaceType.CUSTOM && (FaceType)al[i + 1] == FaceType.NEW_CUSTOM)
                {
                    int extSize = (int)(al[i + 2] - 0x30 + 1);//扩展名长度
                    shortcutSize = (int)(al[i + 2 + 32 + 1 + extSize + 1] - 0x41);//快捷键长度
                    bytesSize = 3 + 32 + 1 + extSize + 1 + shortcutSize;
                    tempBytes = new byte[bytesSize];
                    al.CopyTo(i, tempBytes, 0, bytesSize);
                    FaceNameBytes = new byte[36];
                    Array.Copy(tempBytes, 3, FaceNameBytes, 0, 36);
                    FaceName = Encoding.GetEncoding("GBK").GetString(FaceNameBytes);
                    string face = string.Format("[CustomFace={0}]", FaceName);
                    facebytes = Encoding.GetEncoding("GBK").GetBytes(face);
                    al.RemoveRange(i, bytesSize);//删除原数据
                    al.InsertRange(i, facebytes);//插入自己转换后的数据
                    Faces.Add(FaceName);//加入队列，因为和群自定义表情不会同时在同一条消息里出现，因此不会有问题
                    i += facebytes.Length - 1;
                }
                else if ((FaceType)al[i] == FaceType.CUSTOM && (FaceType)al[i + 1] == FaceType.EXISTING_CUSTOM)
                {
                    FaceName = Faces[al[i + 2] - 0x41];
                    string face = string.Format("[CustomFace={0}]", FaceName);
                    facebytes = Encoding.GetEncoding("GBK").GetBytes(face);
                    al.RemoveRange(i, 3);//删除原数据
                    al.InsertRange(i, facebytes);//插入自己转换后的数据
                    i += facebytes.Length - 1;

                }
                else if ((FaceType)al[i] == FaceType.CUSTOM && (FaceType)al[i + 1] == FaceType.NEW_SERVER_SIDE_CUSTOM)//这里转换自定义表情和贴图 FaceType.CUSTOM表示是自定义表情或贴图， FaceType.NEW_SERVER_SIDE_CUSTOM表示是在这条消息里第一次出现的自定义表情或贴图
                {
                    tempBytes = new byte[3];//这里获取表情数据的长度
                    al.CopyTo(i + 2, tempBytes, 0, 3);
                    bytesSize = Convert.ToInt32(Encoding.GetEncoding("GBK").GetString(tempBytes));
                    tempBytes = new byte[bytesSize];
                    al.CopyTo(i, tempBytes, 0, bytesSize);

                    if (tempBytes[5] == 0x65)//如果是'e'表示是自定义表情
                    {
                        FaceOrPic = true;
                    }
                    else if (tempBytes[5] == 0x6B)//如果是'k'表示是贴图 自定义表情和贴图的区别在于文件名不同，自定义表情的文件名是MD5字串加扩展名，贴图是{GUID}加扩展名
                    {
                        FaceOrPic = false;
                    }

                    shortcutSize = (int)(tempBytes[6] - 0x41);//快捷键长度

                    if (FaceOrPic)
                    {
                        FaceNameBytes = new byte[36];//MD5+".gif"一共36个字节
                        Array.Copy(tempBytes, 0x31, FaceNameBytes, 0, 36);
                        FaceName = Encoding.GetEncoding("GBK").GetString(FaceNameBytes);
                        string face = string.Format("[CustomFace={0}]", FaceName);
                        facebytes = Encoding.GetEncoding("GBK").GetBytes(face);
                        al.RemoveRange(i, bytesSize);//删除原数据
                        al.InsertRange(i, facebytes);//插入自己转换后的数据
                    }
                    else
                    {
                        FaceNameBytes = new byte[42];//{GUID}+".gif"一共42个字节
                        Array.Copy(tempBytes, 0x31, FaceNameBytes, 0, 42);
                        FaceName = Encoding.GetEncoding("GBK").GetString(FaceNameBytes).Replace("{", "").Replace("}", "");
                        string face = string.Format("[CustomFace={0}]", FaceName);
                        facebytes = Encoding.GetEncoding("GBK").GetBytes(face);
                        al.RemoveRange(i, bytesSize);
                        al.InsertRange(i, facebytes);
                    }

                    Faces.Add(FaceName);//加入队列
                    i += facebytes.Length - 1;

                }
                else if ((FaceType)al[i] == FaceType.CUSTOM && (FaceType)al[i + 1] == FaceType.EXISTING_SERVER_SIDE_CUSTOM_SIDE)//如果是本消息中已经出现过的表情或截图
                {
                    tempBytes = new byte[3];
                    al.CopyTo(i + 2, tempBytes, 0, 3);
                    bytesSize = Convert.ToInt32(Encoding.GetEncoding("GBK").GetString(tempBytes));

                    tempBytes = new byte[bytesSize];

                    al.CopyTo(i, tempBytes, 0, bytesSize);

                    FaceName = Faces[tempBytes[5] - 0x41];//从队列中取出
                    string face = string.Format("[CustomFace={0}]", FaceName);
                    facebytes = Encoding.GetEncoding("GBK").GetBytes(face);
                    al.RemoveRange(i, bytesSize);
                    al.InsertRange(i, facebytes);

                    i += facebytes.Length - 1;
                }
            }

            tempBytes = new byte[al.Count];
            al.CopyTo(0, tempBytes, 0, tempBytes.Length);//ArrayList转byte[]
            return Encoding.GetEncoding("GBK").GetString(tempBytes);//byte[]转String
        }

        /// <summary>
        /// 当前路径
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string MapPath(string strPath)
        {
            if (System.Web.HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用 
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    //strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\'); 
                    strPath = strPath.TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        public static void DownLoadFileFromUrl(string url,string filename,out string getQQSession)
        {
            
            WebClient Client = new WebClient();
            string directory = filename.Substring(0, filename.LastIndexOf("\\")+1);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            Client.DownloadFile(url, filename);
            getQQSession=Client.ResponseHeaders["getqqsession"];
            
            
            
        }

       

    }
}
