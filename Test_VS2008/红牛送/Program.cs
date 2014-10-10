using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace 红牛送
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient HTTPproc = new WebClient();
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            string strParameter = null;
            string[] arrList = { "吕春燕|13589298159", "王鑫|13061434820", "张倩|13070882759", "戴雪莲|13105197082", "郑铁源|13853247537", "张忆豫|13061257775", "李丽|13953256039", "李杨|13789860913", "丁喆|15253212675", "孔翔天|13589291804", "王连会|15863051701", "谢升飞|13864839329", "于少军|15615327658", "李力|15864749310", "孙乐|13864817885", "方寅福|15194201027", "郭任重|15192695230", "石栋梁|15954888434", "王岳|13553056832", "王茜|13792477703", "郑曼曼|13589341076", "何岩|13061282323", "吴彦锋|13969717613", "褚治国|15615427972", "姜涵文|15265328910", "徐志成|13573283061", "付航|13573837375", "贾存存|15020021023", "吴蕾|13854251842", "崔兆飞|15966855406", "隋隆|13791993937", "刘晓宁|15064887596", "高超|13969729515", "方鹏|15864702776", "谢奇超|15254275327", "王长虹|15053202079", "程艳|15194229368", "王智勇|13864856527", "曲涛|13001699586", "濮小艺|13646421624", "陈会莲|13608973747", "张岩|15092276163", "葛靖堃|15006580867", "徐非|13583223453", "张紫杨|15966934620", "邸炜鹏|13853242943", "范微微|15166695916", "刘阳|13805429170", "左晶轩|15192046988", "来守云|15275299065", "殷帅|15192677105", "方超|15092107662", "尚延蔚|15965564003", "贾广顺|15253214582", "周纯|13792878136", "杨阳|15964249298", "孙暖|15966805495", "张慧军|13793240175", "周超|13780601039", "曹璇|13206472128", "杨雪|15954850859", "王乐|15853280760", "张梦颖|13969728183", "纪刚|13780645905", "徐腾|13678857980" };
            string strRequest = "http://www.redbulltime.com/ashx/save_profile.ashx";
            strParameter = "reg_realname=韩志伟&reg_city=青岛&reg_zone=市南区&reg_office=青岛国际动漫产业园&reg_floor=E座&reg_room=207&reg_company=美天网络&reg_mobile=13156280289&reg_phone=";
            string[] arrContent;

            foreach (string strSSS in arrList)
            {
                arrContent = strSSS.Split('|');
                strParameter = "reg_realname=" + arrContent[0] + "&reg_city=青岛&reg_zone=市南区&reg_office=青岛国际动漫产业园&reg_floor=E座&reg_room=207&reg_company=美天网络&reg_mobile=" + arrContent[1] + "&reg_phone=";
                string strContent = HTTPproc.OpenRead(strRequest, UrlEncode(strParameter, "UTF-8"));
                Console.WriteLine(arrContent[0] + " " + strContent);
                Thread.Sleep(3000);
            }
            //Console.WriteLine(UrlEncode(strParameter, "UTF-8"));
            //string strContent = HTTPproc.OpenRead(strRequest, UrlEncode(strParameter, "UTF-8"));            
            Console.ReadKey();
        }

        #region URL编码 UrlEncode(string str, string encode)
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private static string UrlEncode(string str, string encode)
        {
            int factor = 0;
            if (encode == "UTF-8")
                factor = 3;
            if (encode == "GB2312")
                factor = 2;
            //不需要编码的字符

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._=&";
            System.Text.Encoder encoder = System.Text.Encoding.GetEncoding(encode).GetEncoder();
            char[] c1 = str.ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //一个字符一个字符的编码

            for (int i = 0; i < c1.Length; i++)
            {
                //不需要编码

                if (okChar.IndexOf(c1[i]) > -1)
                    sb.Append(c1[i]);
                else
                {
                    byte[] c2 = new byte[factor];
                    int charUsed, byteUsed; bool completed;

                    encoder.Convert(c1, i, 1, c2, 0, factor, true, out charUsed, out byteUsed, out completed);

                    foreach (byte b in c2)
                    {
                        if (b != 0)
                            sb.AppendFormat("%{0:X}", b);
                    }
                }
            }
            return sb.ToString().Trim();
        }
        #endregion


    }
}
