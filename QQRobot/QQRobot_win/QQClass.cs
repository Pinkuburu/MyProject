using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Windows.Forms;

namespace QQRobot_win
{
    class QQClass
    {
        //声明序列信息
        private static int i = 12;
        //声明HTTP请求类
        private static WebClient HTTPproc = new WebClient();
        //声明随机数字类
        private static RandStr rnd = new RandStr(true, false, false, false);

        #region QQ密码加密算法
        /// <summary>
        /// QQ密码加密算法
        /// </summary>
        public static class qqPwdEncrypt
        {
            /// <summary>
            /// 计算网页上QQ登录时密码加密后的结果
            /// </summary>
            /// <param name="pwd" />QQ密码</param>
            /// <param name="verifyCode" />验证码</param>
            /// <returns></returns>
            public static String Encrypt(string pwd, string verifyCode)
            {
                return (md5(md5_3(pwd).ToUpper() + verifyCode.ToUpper())).ToUpper();
            }
            /// <summary>
            /// 计算字符串的三次MD5
            /// </summary>
            /// <param name="s" /></param>
            /// <returns></returns>
            private static String md5_3(String s)
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);

                bytes = md5.ComputeHash(bytes);
                bytes = md5.ComputeHash(bytes);
                bytes = md5.ComputeHash(bytes);

                md5.Clear();

                string ret = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
                }

                return ret.PadLeft(32, '0');
            }
            /// <summary>
            /// 计算字符串的一次MD5
            /// </summary>
            /// <param name="s" /></param>
            /// <returns></returns>
            private static String md5(String s)
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);

                bytes = md5.ComputeHash(bytes);

                md5.Clear();

                string ret = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
                }

                return ret.PadLeft(32, '0');
            }
        }
        #endregion QQ密码加密算法
        
        public static string Login(string strQQ,string strPwd)
        {
            //变量声明
            string strMsg = null;
            string strReMsg = null;
            string strRequest = null;
            string strContent = null;
            string strCookies = null;
            string resultString = null;
            string strSkey = null;
            string strPtcz = null;
            string strPtwebqq = null;
            string strPost = null;

            string[] arrResult = { };
            HTTPproc.Encoding = System.Text.Encoding.UTF8;

            strRequest = "http://ptlogin2.qq.com/check?uin=" + strQQ + "&appid=1002101&r=" + rnd.GetRandStr(13);
            //strContent = HTTPproc.OpenRead(strRequest);
            strContent = HTTPproc.GetHtml(strRequest);

            try
            {
                resultString = Regex.Match(strContent, @"'\d','.*'").Value.Replace("\'", "");//'0','!MLF'
                arrResult = resultString.Split(',');
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("没有获取到验证码正则出错，请检查！");
                Console.WriteLine(ex);
            }
            Console.WriteLine("获取的验证码:" + arrResult[1]);

            strCookies = HTTPproc.ResponseHeaders.GetValues(3).GetValue(0).ToString();

            try
            {
                resultString = Regex.Replace(strCookies, ";.*", "").Replace("ptvfsession=","");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            HTTPproc.CookieContainer.Add(new Uri("http://ptlogin2.qq.com"), new Cookie("ptvfsession", resultString));
            strRequest = "http://ptlogin2.qq.com/login?u=" + strQQ + "&p=" + qqPwdEncrypt.Encrypt(strPwd, arrResult[1]) + "&verifycode=" + arrResult[1] + "&remember_uin=1&aid=1002101&u1=http%3A%2F%2Fweb.qq.com%2Fmain.shtml%3Fdirect__2&h=1&ptredirect=1&ptlang=2052&from_ui=1&pttype=1&dumy=&fp=loginerroralert";

            //strContent = HTTPproc.OpenRead(strRequest);
            strContent = HTTPproc.GetHtml(strRequest);

            try
            {
                resultString = Regex.Match(strContent, @"'\d','.*'").Value.Replace("\'", "");//'3','0','','0','您输入的密码有误，请重试。'
                arrResult = resultString.Split(',');
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("没有获取到验证码正则出错，请检查！");
                Console.WriteLine(ex);
            }

            #region 登录返回值判定
            if (arrResult[0] == "0")        //0：登录成功!
            {
                Console.WriteLine(arrResult[4]);
                strSkey = HTTPproc.ResponseHeaders.GetValues(1).GetValue(2).ToString();
                //strPtcz = HTTPproc.ResponseHeaders.GetValues(1).GetValue(9).ToString();
                try
                {
                    strPtwebqq = HTTPproc.ResponseHeaders.GetValues(1).GetValue(9).ToString();
                }
                catch
                {
                    strPtwebqq = HTTPproc.ResponseHeaders.GetValues(1).GetValue(8).ToString();
                }
            }
            else if (arrResult[0] == "1")   //1：系统繁忙，请稍后重试。
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "2")   //2：已经过期的QQ号码。
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "3")   //3：您输入的密码有误，请重试。
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "4")   //4：您输入的验证码有误，请重试。
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "5")   //5：校验失败。
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "6")   //6：密码错误。如果您刚修改过密码, 请稍后再登录.
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "7")   //7：您的输入有误, 请重试。
            {
                Console.WriteLine(arrResult[4]);
            }
            else                            //8：您的IP输入错误的次数过多，请稍后再试。
            {
                Console.WriteLine("未知错误！");
            }
            Console.WriteLine(arrResult[1]);
            #endregion 登录返回值判定
                  
            //strSkey = Regex.Replace(strSkey, ";.*", "").Replace("skey=", "");
            //strPtwebqq = Regex.Replace(strPtwebqq, ";.*", "").Replace("ptwebqq=", "");

            strSkey = Regex.Replace(strSkey, ";.*", "").Replace("skey=", "");
            //strPtcz = Regex.Replace(strPtcz, ";.*", "").Replace("ptcz=", "");
            strPtwebqq = Regex.Replace(strPtwebqq, ";.*", "").Replace("ptwebqq=", "");

            //HTTPproc.CookieContainer.Add(new Uri("http://ptlogin2.qq.com"), new Cookie("skey", resultString));
            //HTTPproc.CookieContainer.Add(new Uri("http://ptlogin2.qq.com"), new Cookie("ptcz", resultString));
            //HTTPproc.CookieContainer.Add(new Uri("http://ptlogin2.qq.com"), new Cookie("ptwebqq", resultString));

            //*strPost = strQQ + ";22;0;00000000;" + strSkey + ";" + strPtwebqq + ";0;";
            //*strRequest = "http://web-proxy.qq.com/conn_s";
            
            //string strOldCookie = HTTPproc.Cookie.ToString();
            //try 
            //{
            //    resultString = Regex.Replace(strOldCookie, "; .*pti", "pti");
            //    resultString = Regex.Replace(resultString, "; .*p", "p");
            //} 
            //catch (ArgumentException ex) 
            //{
            //    // Syntax error in the regular expression
            //}
            //HTTPproc.RequestHeaders.Clear();
            //HTTPproc.RequestHeaders.Add("Cookie:" + resultString);
            //HTTPproc.RequestHeaders.Set("Cookie", resultString);
            //HTTPproc.RequestHeaders.Add("Referer:http://web-proxy.qq.com/");
            //strContent = HTTPproc.OpenRead(strRequest, strPost);
            //*strContent = HTTPproc.Post(strRequest, strPost);
            //*Console.WriteLine(strContent);

            //*string[] arrKey = strContent.Split(';');

            //*//读取分组
            //*strContent = HTTPproc.Post(strRequest, strQQ + ";3c;0;" + arrKey[4].ToString() + ";1;" + strQQ + ";00;1;" + arrKey[4].ToString() + ";");
            //*Console.WriteLine(strContent);

            //*//读取好友列表
            //*strContent = HTTPproc.Post(strRequest, strQQ + ";06;2;" + arrKey[4].ToString() + ";" + strQQ + ";" + strQQ + ";5c;3;" + arrKey[4].ToString() + ";88;" + strQQ + ";67;4;" + arrKey[4].ToString() + ";03;1;" + strQQ + ";" + strQQ + ";58;5;" + arrKey[4].ToString() + ";0;" + strQQ + ";26;6;" + arrKey[4].ToString() + ";0;0;" + strQQ + ";3e;7;" + arrKey[4].ToString() + ";4;0;" + strQQ + ";65;8;" + arrKey[4].ToString() + ";02;" + strQQ + ";" + strQQ + ";1d;9;" + arrKey[4].ToString() + ";" + strQQ + ";00;10;" + arrKey[4].ToString() + ";");
            //*Console.WriteLine(strContent);

            //*strContent = HTTPproc.Post(strRequest, strQQ + ";00;11;" + arrKey[4].ToString() + ";");
            //*Console.WriteLine(strContent);

            //*string[] arrReMsg;

            //*while (i < 100)
            //*{
            //*    Console.WriteLine("正在等待接收消息:{0}", i);
            //*    strContent = HTTPproc.Post(strRequest, strQQ + ";00;" + i.ToString() + ";" + arrKey[4].ToString() + ";");

            //*    arrReMsg = strContent.Split(';');
            //*    if (arrReMsg.Length > 10)
            //*    {
            //*        if (arrReMsg[1].ToString() == "17")
            //*        {
            //*            i++;
            //*            strMsg = HTTPproc.Post(strRequest, strQQ + ";17;" + arrReMsg[2].ToString() + ";" + arrKey[4].ToString() + ";" + arrReMsg[3].ToString() + ";" + strQQ + ";" + arrReMsg[4].ToString() + ";1;" + arrReMsg[10].ToString() + ";" + strQQ + ";00;" + i.ToString() + ";" + arrKey[4].ToString() + ";");
            //*                                             //strQQ + ";17;" + arrReMsg[2].ToString() + ";" + arrKey[4].ToString() + ";" + arrReMsg[3].ToString() + ";" + strQQ + ";" + arrReMsg[4].ToString() + ";1;" + arrReMsg[10].ToString() + ";" + strQQ + ";00;" + i.ToString() + ";" + arrKey[4].ToString() + ";"
            //*            Console.WriteLine("发送确认消息:{0}", strMsg);

            //*            //发送消息
            //*            i++;
            //*            strReMsg = HTTPproc.Post(strRequest, strQQ + ";16;" + i.ToString() + ";" + arrKey[4].ToString() + ";" + arrReMsg[3].ToString() + ";0b;528;" + UrlEncode(arrReMsg[7].ToString(),"UTF-8") + ";0a00000010%E5%AE%8B%E4%BD%93");
            //*            Console.WriteLine("发送消息:{0}", strReMsg);
            //*            //1307364337;16;119;165fad25;182536608;0b;528;%E4%BD%A0%E5%A5%BD;0a00000010%E5%AE%8B%E4%BD%93;
            //*            //1307364337;16;135;165fad25;182536608;0b;528;%E4%BD%A0%E5%A5%BD;0aff000010%E5%AE%8B%E4%BD%93;
            //*        }
            //*    }
            //*    Console.WriteLine(strContent);
            //*    i++;
            //*}

            return strSkey;
        }

        public static string Login(string strQQ)
        {            
            //变量声明
            string strRequest = null;
            string strContent = null;
            string resultString = null;
            string vc_type = null;
            string strCookies = null;
            string[] arrResult = { };

            HTTPproc.Encoding = System.Text.Encoding.UTF8;

            strRequest = "http://ptlogin2.qq.com/check?uin=" + strQQ + "&appid=1002101&r=" + rnd.GetRandStr(13);
            strContent = HTTPproc.GetHtml(strRequest);

            try
            {
                resultString = Regex.Match(strContent, @"'\d','.*'").Value.Replace("\'", "");//'0','!MLF'
                arrResult = resultString.Split(',');
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("没有获取到验证码正则出错，请检查！");
                MessageBox.Show(ex.ToString());
            }
            vc_type = arrResult[1].ToString();

            strCookies = HTTPproc.ResponseHeaders.GetValues(3).GetValue(0).ToString();

            try
            {
                resultString = Regex.Replace(strCookies, ";.*", "").Replace("ptvfsession=", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            HTTPproc.CookieContainer.Add(new Uri("http://ptlogin2.qq.com"), new Cookie("ptvfsession", resultString));

            return vc_type;
        }        

        public static string ReadUserInfo(string strQQ, string strSkey)
        {
            string strUserInfo = null;            
            string strRequest = null;
            string strPost = null;
            
            try
            {
                strSkey = Regex.Replace(strSkey, ";.*", "").Replace("skey=", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            strPost = strQQ + ";5c;3;" + strSkey + ";88;";
            strRequest = "http://web-proxy.qq.com/conn_s";

            //strContent = HTTPproc.OpenRead(strRequest);
            strUserInfo = HTTPproc.Post(strRequest, strPost);
            return strUserInfo;
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

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.";
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

        #region 随机生成 字数、数字、符号 RandStr
        /// <summary>
        /// 随机生成 字数、数字、符号
        /// </summary>
        public class RandStr
        {
            private string framerStr = null;
            private string numStr = "0123456789";
            private string upperStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            private string lowerStr = "abcdefghijklmnopqrstuvwxyz";
            private string markStr = @"`-=[];'\,./~!@#$%^&*()_+{}:""|<>?";
            private static Random myRandom = new Random();

            /// <summary>
            /// 如未提供参数构造,则默认由数字+小写字母构成
            /// </summary>
            public RandStr()
            {
                framerStr = numStr + lowerStr;
            }

            /// <summary>
            /// 构造函数,可指定构成的字符
            /// </summary>
            /// <param name="useNum">是否使用数字</param>
            /// <param name="useUpper">是否使用大写字母</param>
            /// <param name="useLower">是否使用小写字母</param>
            /// <param name="useMark">是否使用符号</param>
            public RandStr(bool useNum, bool useUpper, bool useLower, bool useMark)
            {
                // 如果试图构造不包含任何组成字符的类,则抛出异常
                if (!useNum && !useUpper && !useLower && !useMark)
                {
                    throw new ArgumentException("必须至少使用一种构成字符!");
                }
                else
                {
                    if (useNum)
                        framerStr += numStr;
                    if (useUpper)
                        framerStr += upperStr;
                    if (useLower)
                        framerStr += lowerStr;
                    if (useMark)
                        framerStr += markStr;
                }
            }

            /// <summary>
            /// 使用自定义的组成字符构造
            /// </summary>
            /// <param name="userStr">自定义字符</param>
            public RandStr(string userStr)
            {
                // 如果试图用空字符串构造类,则抛出异常
                if (userStr.Length == 0)
                {
                    throw new ArgumentException("请至少使用一个字符!");
                }
                else
                {
                    framerStr = userStr;
                }
            }

            /// <summary>
            /// 取得一个随机字符串
            /// </summary>
            /// <param name="length">取得随机字符串的长度</param>
            /// <returns>返回的随机字符串</returns>
            public string GetRandStr(int length)
            {
                // 获取的长度不能为0个或者负数个
                if (length < 1)
                {
                    throw new ArgumentException("字符长度不能为0或者负数!");
                }
                else
                {
                    // 如果只是获取少量随机字符串,
                    // 这样没有问题.
                    // 但如果需要短时间获取大量随机字符串的话,
                    // 这样可能性能不高.
                    // 可以改用StringBuilder类来提高性能,
                    // 需要的可以自己改一下 ^o^
                    string tempStr = null;
                    for (int i = 0; i < length; i++)
                    {
                        int randNum = myRandom.Next(framerStr.Length);
                        tempStr += framerStr[randNum];
                    }
                    return tempStr;
                }
            }
        }
        #endregion 随机生成 字数、数字、符号 RandStr
    }
}
