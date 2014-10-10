using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

namespace 领取淘金币
{
    public class TaobaoWapClass
    {
        WebClient HTTPproc = new WebClient();
        ArrayList alSID = new ArrayList();
        public string strReferer = null;
        //private string _sid;

        #region WAP登录并返回SID WapLogin(string strUserName, string strPassword)
        /// <summary>
        /// WAP登录并返回SID
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        /// <returns>SID</returns>
        public string WapLogin(string strUserName, string strPassword)
        {
            HTTPproc = new WebClient();
            HTTPproc.Encoding = Encoding.UTF8;

            string strRequest = null;
            string strContent = null;
            string strParameter = null;
            string strSID = null;
            string strTOKEN = null;

            strRequest = "http://wap.taobao.com/login/login.htm";
            strContent = HTTPproc.OpenRead(strRequest);

            try
            {
                strSID = Regex.Match(strContent, @"sid=\w{5,}").Value;
                strTOKEN = Regex.Match(strContent, @"token=\w{5,}").Value.Replace("token", "_umid_token");
            }
            catch (ArgumentException ex)
            {
                return ex.ToString();
            }
            strRequest = "http://wap.taobao.com/login/login.htm?" + strSID + "&_input_charset=utf-8";
            strParameter = "TPL_username=" + UrlEncode(strUserName, "UTF-8") + "&TPL_password=" + strPassword + "&action=LoginAction&event_submit_do_login=1&TPL_redirect_url=&ssottid=&" + strSID + "&" + strTOKEN;
            strContent = HTTPproc.OpenRead(strRequest, strParameter);

            try
            {
                if (strContent.IndexOf("密码和账户名不匹配") > -1)
                {
                    return "密码和账户名不匹配";
                }
                if (strContent.IndexOf("该账户名不存在") > -1)
                {
                    return "该账户名不存在";
                }

                string strLocationURL = null;
                if (HTTPproc.StatusCode == 302)
                {
                    bool status = true;
                    while (status)
                    {
                        if (HTTPproc.StatusCode == 302)
                        {
                            strLocationURL = HTTPproc.ResponseHeaders["Location"].ToString();
                            HTTPproc.OpenRead(strLocationURL);
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }
                strSID = Regex.Match(strLocationURL, @"sid=\w{5,}").Value;
                if (strSID == "")
                {
                    return "没有找到SID链接";
                }
                return strSID;
            }
            catch (ArgumentException ex)
            {
                return ex.ToString();
            }            
        }
        #endregion WAP登录并返回SID WapLogin(string strUserName, string strPassword)

        #region 保存用户名对应的SID saveSID(string strUserName, string strSID)
        /// <summary>
        /// 保存用户名对应的SID
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strSID"></param>
        /// <returns></returns>
        public void saveSID(string strUserName, string strSID)
        {
            alSID.Add(strUserName + "|" + strSID);
        }
        #endregion 保存用户名对应的SID saveSID(string strUserName, string strSID)

        #region 领取金币 ReceiveCoin(string strSID)
        /// <summary>
        /// 领取金币
        /// </summary>
        /// <param name="strSID"></param>
        /// <returns></returns>
        public string ReceiveCoin(string strSID, string strUserName)
        {
            HTTPproc.Encoding = Encoding.UTF8;
            string strContent = null;
            string strRequest = null;
            string strLocationURL = null;

            if (strSID == "")
            {
                return "SID不能为空";
            }

            strRequest = "http://i.m.taobao.com/ms_index.htm?sid=" + strSID;
            strContent = HTTPproc.OpenRead(strRequest);

            if (HTTPproc.StatusCode == 302)
            {
                strLocationURL = HTTPproc.ResponseHeaders["Location"].ToString();
                strContent = HTTPproc.OpenRead(strLocationURL);
            }

            try
            {
                if (strContent.IndexOf("今日已签到") > -1)
                {
                    return strUserName + " 今日已经签到过了";
                }
                else
                {
                    strContent = Regex.Match(strContent, @"http://i\.m\.taobao\.com/.*sprefer=sydz19").Value.Replace("amp;", "");

                    if (strContent == "")
                    {
                        return strUserName + "没有查找到领取金币链接";
                    }

                    strRequest = strContent;
                    HTTPproc.OpenRead(strRequest);
                    return strUserName + " 金币领取成功";
                }
            }
            catch (ArgumentException ex)
            {
                return ex.ToString();
            }
        }
        #endregion 领取金币 ReceiveCoin(string strSID)

        #region 统计好友数 FriendCount(string strSID)
        /// <summary>
        /// 统计好友数
        /// </summary>
        /// <param name="strSID"></param>
        /// <returns></returns>
        public string FriendCount(string strSID)
        {
            HTTPproc.Encoding = Encoding.UTF8;
            string strContent = null;
            string strRequest = null;

            if (strSID == "")
            {
                return "SID不能为空";
            }

            strRequest = "http://m.taobao.com/my_taobao.htm?sid=" + strSID;
            this.strReferer = strRequest;
            strContent = HTTPproc.OpenRead(strRequest);

            try
            {
                strContent = Regex.Match(strContent, "好友(.+?</a>)").Value;//匹配链接
                if (strContent == "")
                {
                    return "没有查找到好友统计链接";
                }
                strContent = Regex.Match(strContent, @">\d{1,}<").Value.Replace(">","").Replace("<","");//匹配好友数
                return strContent;
            }
            catch (ArgumentException ex)
            {
                return ex.ToString();
            }
        }
        #endregion 统计好友数 FriendCount(string strSID)

        #region 读取帐号信息 ReadTXT()
        /// <summary>
        /// 读取帐号信息
        /// </summary>
        /// <returns></returns>
        public ArrayList ReadTXT()
        {
            FileStream fs = new FileStream("user.txt", FileMode.Open);
            StreamReader m_streamReader = new StreamReader(fs);
            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

            ArrayList aluser = new ArrayList();
            string strLine = m_streamReader.ReadLine();
            do
            {
                aluser.Add(strLine);
                strLine = m_streamReader.ReadLine();

            } 
            while (strLine != null && strLine != "");
            m_streamReader.Close();
            m_streamReader.Dispose();
            fs.Close();
            fs.Dispose();
            return aluser;
        }
        #endregion 读取帐号信息 ReadTXT()

        #region URL编码 UrlEncode(string str, string encode)
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public string UrlEncode(string str, string encode)
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
    }
}
