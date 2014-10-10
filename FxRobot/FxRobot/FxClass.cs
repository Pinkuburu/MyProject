using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections;

namespace FxRobot
{
    public class FxClass
    {
        //初始化HTTP类
        WebClient HTTPproc = new WebClient();
        ArrayList FriendList = new ArrayList();
        public string ssid = null;
        public int intVersion = 0;        

        #region 飞信登录 Fx_Login(string strMobile, string strPwd, string Code, int Status)
        /// <summary>
        /// 飞信登录
        /// </summary>
        /// <param name="strMobile">手机号</param>
        /// <param name="strPwd">密码</param>
        /// <param name="Code">验证码</param>
        /// <param name="Status">登录状态:400在线</param>
        /// <returns></returns>
        public string Fx_Login(string strMobile, string strPwd, string Code, int Status)
        {
            int intCode = 0;
            string strRequest = null;
            string strContent = null;
            string strParameter = null;
            WebClient.strMobile = strMobile;
            //Status:400在线
            //ccpsession       e7548ddf-66d7-4773-bbdc-ef43c514a84e                 
            strRequest = "https://webim.feixin.10086.cn/WebIM/Login.aspx";
            strParameter = "UserName=" + strMobile + "&Pwd=" + strPwd + "&Ccp=" + Code + "&OnlineStatus=" + Status;

            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            strContent = HTTPproc.Post(strRequest, strParameter);
            JObject o = JObject.Parse(Unicode2Character(strContent));
            intCode = (int)o["rc"];

            switch (intCode)
            {
                case 312:
                    strContent = "验证码出错~";
                    break;
                case 321:
                    strContent = "密码出错~";
                    break;
                case 404:
                    strContent = "尚未开通飞信服务~";
                    break;
                case 200:
                    strContent = "登录成功~";
                    this.ssid = HTTPproc.ResponseHeaders.GetValues(1).GetValue(0).ToString().Replace("webim_sessionid=", "").Replace("; path=/", "");
                    GetPersonalInfo();
                    GetContactList();
                    GetFriendList();
                    break;
            }

            return strContent;
        }
        #endregion 飞信登录 Fx_Login(string strMobile, string strPwd, string Code, int Status)

        #region 读取用户信息 GetPersonalInfo()
        /// <summary>
        /// 读取用户信息
        /// </summary>
        /// <returns></returns>
        private string GetPersonalInfo()
        {
            string strRequest = null;
            string strContent = null;
            string strParameter = null;

            strRequest = "http://webim.feixin.10086.cn/WebIM/GetPersonalInfo.aspx?Version=0";
            strParameter = "ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);
            
            //读取用户信息就不写了，读了也没有什么意思

            return strContent;
        }
        #endregion 读取用户信息 GetPersonalInfo()

        #region 读取好友及分组信息 GetContactList()
        /// <summary>
        /// 读取好友及分组信息
        /// </summary>
        /// <returns></returns>
        private string GetContactList()
        {            
            string strRequest = null;
            string strContent = null;
            string strParameter = null;

            strRequest = "http://webim.feixin.10086.cn/WebIM/GetContactList.aspx?Version=" + Version();
            strParameter = "ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);                    

            return strContent;
        }
        #endregion 读取好友及分组信息 GetContactList()

        #region 读取好友列表 GetFriendList()
        /// <summary>
        /// 读取好友列表
        /// </summary>
        /// <returns></returns>
        private string GetFriendList()
        {
            string strRequest = null;
            string strContent = null;
            string strParameter = null;

            strRequest = "http://webim.feixin.10086.cn/WebIM/GetConnect.aspx?Version=" + Version();
            strParameter = "ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);

            JObject o = JObject.Parse((Unicode2Character(strContent)));
            JArray friends = (JArray)o["rv"];

            try
            {
                foreach (JObject uid in friends)
                {
                    FriendList.Add(uid["Data"]);
                }
                strContent = "读取好友列表成功!";
            }
            catch
            {
                strContent = "读取好友列表失败!";
            }
            return strContent;
        }
        #endregion 读取好友列表 GetFriendList()

        /// <summary>
        /// 守护进程
        /// </summary>
        public string GetConnect()
        {
            string strRequest = null;
            string strContent = null;
            string strParameter = null;

            strRequest = "http://webim.feixin.10086.cn/WebIM/GetConnect.aspx?Version=" + Version();
            strParameter = "ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);

            JObject o = JObject.Parse((Unicode2Character(strContent)));

            try
            {
                JArray Event = (JArray)o["rv"];
                if ((int)Event[0]["DataType"] == 3)
                {
                    strContent = SendMsg((int)Event[0]["Data"]["fromUid"], (string)Event[0]["Data"]["msg"], 0);
                }
            }
            catch
            {

            }      
      
            return strContent;
        }

        //好友上线提示状态
        //{"rc":200,"rv":[{"DataType":2,"Data":{"pb":"400","pd":"","dt":"WEB","dc":"463","uid":289724462}}]}
        //{"rc":200,"rv":[{"DataType":2,"Data":{"pb":"600","pd":"忙碌","dt":"WEB","dc":"463","uid":289724462}}]}
        //{"rc":200,"rv":[{"DataType":2,"Data":{"sms":"365.0:0:0","pb":"0","pd":"","dt":"","dc":"0","uid":289724462}}]}
        //{"rc":200,"rv":[{"DataType":2,"Data":{"pb":"0","pd":"","dt":"","dc":"0","uid":289724462}}]}

        public string ShowFriendList()
        {
            StringBuilder sb = new StringBuilder();
            foreach (JObject userInfo in FriendList)
            {
                sb.Append(userInfo.ToString().Replace("\r\n","").Replace("  ","") + "\r\n");
            }
            return sb.ToString();
        }        

        #region 发送消息 SendMsg(int To, string strMsg, int IsSendSms)
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="To">收信方号码</param>
        /// <param name="strMsg">消息内容</param>
        /// <param name="IsSendSms">消息类型：0在线消息，1短信消息</param>
        /// <returns></returns>
        private string SendMsg(int To, string strMsg, int IsSendSms)
        {
            string strRequest = null;
            string strContent = null;
            string strParameter = null;
            int intCode = 0;

            strRequest = "http://webim.feixin.10086.cn/WebIM/SendMsg.aspx?Version=" + Version();
            strParameter = "To=" + To + "&IsSendSms=" + IsSendSms + "&msg=" + strMsg + "&ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);

            JObject o = JObject.Parse(Unicode2Character(strContent));
            intCode = (int)o["rc"];

            if (intCode == 200)
            {
                strContent = "发送消息：" + strMsg + "成功!";
            }
            else
            {
                strContent = "发送消息：" + strMsg + "失败!";
            }

            return strContent;
        }
        #endregion 发送消息 SendMsg(string To, string strMsg, int IsSendSms)

        #region 设置签名 SetPersonalInfo(string strImpresa)
        /// <summary>
        /// 设置签名
        /// </summary>
        /// <param name="strImpresa">签名内容</param>
        /// <returns></returns>
        private string SetPersonalInfo(string strImpresa)
        {            
            string strRequest = null;
            string strContent = null;
            string strParameter = null;
            int intCode = 0;

            strRequest = "http://webim.feixin.10086.cn/WebIM/SetPersonalInfo.aspx?Version=" + Version();
            strParameter = "Impresa=" + strImpresa + "&ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);

            JObject o = JObject.Parse(Unicode2Character(strContent));
            intCode = (int)o["rc"];

            if (intCode == 200)
            {
                strContent = "签名修改为：" + strImpresa + "成功!";
            }
            else
            {
                strContent = "签名修改为：" + strImpresa + "失败!";
            }

            return strContent;
        }
        #endregion 设置签名 SetPersonalInfo(string strImpresa)

        #region 设置在线状态 SetPresence(int intStatus)
        /// <summary>
        /// 设置在线状态
        /// </summary>
        /// <param name="intStatus"></param>
        /// <returns></returns>
        private string SetPresence(int intStatus)
        {            
            string strRequest = null;
            string strContent = null;
            string strParameter = null;
            string strCustom = null;
            int intCode = 0;

            switch (intStatus)
            {
                case 400:
                    strCustom = "在线";
                    break;
                case 600:
                    strCustom = "忙碌";
                    break;
                case 100:
                    strCustom = "离开";
                    break;
                case 0:
                    strCustom = "隐身";
                    break;
                case 300:
                    strCustom = "马上回来";
                    break;
                case 850:
                    strCustom = "会议中";
                    break;
                case 500:
                    strCustom = "接电话";
                    break;
                case 150:
                    strCustom = "外出就餐";
                    break;
            }

            strRequest = "http://webim.feixin.10086.cn/WebIM/SetPresence.aspx?Version=" + Version();
            strParameter = "Presence=" + intStatus + "&Custom=" + UrlEncode(strCustom, "UTF-8") + "+&ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);

            JObject o = JObject.Parse(Unicode2Character(strContent));
            intCode = (int)o["rc"];

            if (intCode == 200)
            {
                strContent = "设置：" + strCustom + "状态成功!";
            }
            else
            {
                strContent = "设置：" + strCustom + "状态失败!";
            }
            
            return strContent;
        }
        #endregion 设置在线状态 SetPresence(int intStatus)

        #region 心跳记数器 Version()
        /// <summary>
        /// 心跳记数器
        /// </summary>
        /// <returns></returns>
        private string Version()
        {
            this.intVersion++;
            return this.intVersion.ToString();
        }
        #endregion 心跳记数器 Version()

        #region 读取验证码 Fx_ImageCode()
        /// <summary>
        /// 读取验证码
        /// </summary>
        public void Fx_ImageCode()
        {
            string strRequest = null;
            strRequest = "http://webim.feixin.10086.cn/WebIM/GetPicCode.aspx?Type=ccpsession&" + Timestamp();            
            HTTPproc.DownloadFile(strRequest, @"Code.jpg");
        }
        #endregion 读取验证码 Fx_ImageCode()

        #region 时间戳 Timestamp()
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        private long Timestamp()
        {
            long longTimestamp = 0;
            longTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return longTimestamp;
        }
        #endregion 时间戳 Timestamp()

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

        #region 将Unicode转找为Character Unicode2Character(string str)
        /// <summary>
        /// 将Unicode转找为Character
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns></returns>
        private string Unicode2Character(string str)
        {
            string text = str;
            string strPattern = "(?<code>\\\\u[A-F0-9]{4})";
            do
            {
                Match m = Regex.Match(text, strPattern, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    string strValue = m.Groups["code"].Value;
                    int i = System.Int32.Parse(strValue.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                    char ch = Convert.ToChar(i);
                    text = text.Replace(strValue, ch.ToString());
                }
                else
                {
                    break;
                }
            }
            while (true);

            return text;
        }
        #endregion
    }
}
