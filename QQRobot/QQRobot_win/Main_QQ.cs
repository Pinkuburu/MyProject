using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace QQRobot_win
{
    public partial class Main_QQ : Form
    {
        public string strContent = null;
        public string strQQ = null;
        public string strRequest = null;
        public string vc_type = null;
        public string strKey = null;
        
        //声明序列信息
        public int i = 12;
        //声明HTTP请求类
        public WebClient HTTPproc = new WebClient();
        //声明随机数字类
        public RandStr rnd = new RandStr(true, false, false, false);

        public Main_QQ()
        {
            InitializeComponent();
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            this.strKey = Login(this.strQQ, textBox_Password.Text.Trim(), textBox_Code.Text.Trim());
        }

        private void button_ReadCode_Click(object sender, EventArgs e)
        {
            if (textBox_QQ.Text.Trim() != "" && textBox_QQ.Text.Trim().Length > 4)
            {
                strContent = Login(textBox_QQ.Text.Trim());
                SysLog("获取的验证码:" + strContent);
            }
            else
            {
                MessageBox.Show("QQ号输入有误", "系统消息");
            }

            if (strContent != "")
            {
                vc_type = strContent;
                GetImageCode(strQQ, vc_type);
            }
        }

        #region 通过QQ号得到验证码加密串 Login(string strQQ)
        /// <summary>
        /// 通过QQ号得到验证码加密串
        /// </summary>
        /// <param name="strQQ"></param>
        /// <returns></returns>
        private string Login(string strQQ)
        {            
            //变量声明
            string strRequest = null;
            string strContent = null;
            string resultString = null;
            string vc_type = null;
            string[] arrResult = { };
            this.strQQ = strQQ;

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

            return vc_type;
        }
        #endregion 通过QQ号得到验证码加密串 Login(string strQQ)

        #region 登录QQ并返回登录信息 Login(string strQQ, string strPwd, string strImageCode)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strQQ"></param>
        /// <param name="strPwd"></param>
        /// <param name="strImageCode"></param>
        /// <returns>Key</returns>
        private string Login(string strQQ, string strPwd, string strImageCode)
        {
            string[] arrResult = { };
            string[] arrKey = { };
            string resultString = null;
            string strSkey = null;
            string strPtwebqq = null;
            string strPost = null;
            
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            strRequest = "http://ptlogin2.qq.com/login?u=" + strQQ + "&p=" + qqPwdEncrypt.Encrypt(strPwd, strImageCode) + "&verifycode=" + strImageCode + "&remember_uin=1&aid=1002101&u1=http%3A%2F%2Fweb.qq.com%2Fmain.shtml%3Fdirect__2&h=1&ptredirect=1&ptlang=2052&from_ui=1&pttype=1&dumy=&fp=loginerroralert";

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
                SysLog(arrResult[4]);
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
                SysLog(arrResult[4]);
            }
            else if (arrResult[0] == "2")   //2：已经过期的QQ号码。
            {
                SysLog(arrResult[4]);
            }
            else if (arrResult[0] == "3")   //3：您输入的密码有误，请重试。
            {
                SysLog(arrResult[4]);
            }
            else if (arrResult[0] == "4")   //4：您输入的验证码有误，请重试。
            {
                SysLog(arrResult[4]);
            }
            else if (arrResult[0] == "5")   //5：校验失败。
            {
                SysLog(arrResult[4]);
            }
            else if (arrResult[0] == "6")   //6：密码错误。如果您刚修改过密码, 请稍后再登录.
            {
                SysLog(arrResult[4]);
            }
            else if (arrResult[0] == "7")   //7：您的输入有误, 请重试。
            {
                SysLog(arrResult[4]);
            }
            else                            //8：您的IP输入错误的次数过多，请稍后再试。
            {
                SysLog("未知错误！");
            }
            SysLog(arrResult[1]);
            #endregion 登录返回值判定

            strSkey = Regex.Replace(strSkey, ";.*", "").Replace("skey=", "");
            strPtwebqq = Regex.Replace(strPtwebqq, ";.*", "").Replace("ptwebqq=", "");

            strPost = strQQ + ";22;0;00000000;" + strSkey + ";" + strPtwebqq + ";0;";
            this.strRequest = "http://web-proxy.qq.com/conn_s";
            strContent = HTTPproc.Post(this.strRequest, strPost);
            arrKey = strContent.Split(';');
            return arrKey[4].ToString();
        }
        #endregion 登录QQ并返回登录信息 Login(string strQQ, string strPwd, string strImageCode)

        #region 读取验证码方法 GetImageCode(string strQQ,string vc_type)
        private void GetImageCode(string strQQ,string vc_type)
        {
            //向指定网址请求返回数据流
            strRequest = "http://captcha.qq.com/getimage?aid=1002101&r=" + rnd.GetRandStr(13) + "&uin=" + strQQ + "&vc_type=" + vc_type;
            this.pictureBox_ImageCode.Image = Image.FromStream(new MemoryStream(HTTPproc.GetData(strRequest)));
        }
        #endregion 读取验证码方法 GetImageCode(string strQQ,string vc_type)

        #region 刷新验证码方法
        private void pictureBox_ImageCode_Click(object sender, EventArgs e)
        {
            try
            {
                GetImageCode(strQQ, vc_type);
            }
            catch
            {

            }
        }
        #endregion 刷新验证码方法

        #region 修改签名 ModifySign(string strQQ, int i, string strKey, string strContent)
        /// <summary>
        /// 修改签名
        /// </summary>
        /// <param name="strQQ">QQ号</param>
        /// <param name="i">消息序列</param>
        /// <param name="strKey">令牌</param>
        /// <param name="strContent">签名内容</param>
        private void ModifySign(string strQQ, int i, string strKey, string strContent)
        {
            i++;
            string strPost = strQQ + ";67;" + i + ";" + strKey + ";01;" + strContent + ";";
            //1349836289;67;512;ad7e15a4;01;Tests;
            //1349836289;67;14;701f4ae8;01;Tests;
            //1349836289;67;13;eac1407d;01;Cupid;
            //1349836289;67;15;eac1407d;01;Cupids;
            //1349836289;67;34;eac1407d;01;Cupid;
            HTTPproc.Post(this.strRequest, strPost);
        }

        #endregion 修改签名 ModifySign(string strQQ, string i, string strKey, string strContent)

        //被加好友
        //标识:80
        //1349836289;80;50680;41;182536608;1349836289;Test;1;1288270763;
        //1349836289;80;8772;41;182536608;1349836289;Test;1;1288271081;
        //1349836289;80;29061;41;182536608;1349836289;Test;1;1288271252;
        //1349836289;80;27158;41;182536608;1349836289;Test;1;1288271786;

        //添加好友(被动)
        //1349836289;a8;65;a01b4ae8;3;182536608;0;
        //1349836289;a8;90;a01b4ae8;3;182536608;0;
        //1349836289;a8;109;a01b4ae8;3;182536608;0;        

        #region 添加好友(主动) AddFriend(string strQQ,string i,string strKey,string strF_QQ,string strContent)
        /// <summary>
        /// 添加好友(主动)
        /// </summary>
        /// <param name="strQQ">QQ号</param>
        /// <param name="i">消息序列</param>
        /// <param name="strKey">令牌</param>
        /// <param name="strF_QQ">好友QQ号</param>
        /// <param name="strContent">验证消息</param>
        private void AddFriend(string strQQ,string i,string strKey,string strF_QQ,string strContent)
        {
            //添加好友(主动)
            //1349836289;a8;33;701f4ae8;2;182536608;1;0;Test;
            //1349836289;a8;58;701f4ae8;2;182536608;1;0;Test;
            this.strRequest = strQQ + ";a8;" + i + ";" + strKey + ";2;" + strF_QQ + ";1;0;" + strContent + ";";
        }

        #endregion 添加好友(主动) AddFriend(string strQQ,string i,string strKey,string strF_QQ,string strContent)

        #region 运行日志 SysLog(string strLog)
        /// <summary>
        /// 运行日志
        /// </summary>
        /// <param name="strLog"></param>        
        private void SysLog(string strLog)
        {
            DateTime dt = DateTime.Now;
            textBox_SysLog.Text += dt + "  " + strLog + "\r\n";
            textBox_SysLog.SelectionStart = textBox_SysLog.Text.Length;
            textBox_SysLog.ScrollToCaret();
            //始终显示TextBox最新一行，始终滚动到最底部
            textBox_SysLog.SelectionStart = textBox_SysLog.Text.Length;
            textBox_SysLog.ScrollToCaret();
        }
        #endregion

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

        private void button_ModifySign_Click(object sender, EventArgs e)
        {            
            ModifySign(this.strQQ, this.i, this.strKey, "Testssss");
        }
    }
}