using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace C_wQQ
{
    public class User
    {
        #region //变量
        private string uin = string.Empty;
        private string qqpassword = String.Empty;
        private string appid = "1003903";
        private string verifyCode = String.Empty;                   //不需要验证码，获取4位代码
        private string verifyCode32 = String.Empty;                 //需要验证码，获取48位代码
        private string state = String.Empty;
        private string cliendid = ClientID.GenerateClientID();
        private string psessionid = string.Empty;
        private int messagecount = 29500001;
        private string vfwebqq = String.Empty;
        private CookieContainer cc = new CookieContainer();
        private Random ro = new Random();
        private Dictionary<string, string> logincode = new Dictionary<string, string>();
        #endregion

        #region //变量赋值
        public string Uin
        {
            get
            {
                return uin;
            }
            set
            {
                uin = value;
            }
        }
        public string QQPassword
        {
            set
            {
                qqpassword = value;
            }
        }
        public string Verifycode
        {
            get
            {
                return verifyCode;
            }
            set
            {
                verifyCode = value;
            }
        }
        public string Psessionid
        {
            set
            {
                psessionid = value;
            }
        }
        public CookieContainer Cookie
        {
            get
            {
                return cc;
            }
        }
        public string Vfwebqq
        {
            set
            {
                vfwebqq = value;
            }
        }
        #endregion

        #region //构造函数
        public User()
        {
            logincode.Add("0'", "登录成功");
            logincode.Add("1'", "系统繁忙，请稍后重试");
            logincode.Add("2'", "已经过期的QQ号码");
            logincode.Add("3'", "您输入的密码有误");
            logincode.Add("4'", "您输入的验证码有误");
            logincode.Add("5'", "校验失败");
            logincode.Add("6'", "密码错误");
            logincode.Add("7'", "输入有误，请重新输");
            logincode.Add("8'", "IP输入次数过多，请稍后重试");
            logincode.Add("9'", "您输入的帐号不存在");
            logincode.Add("10", "您输入的帐号不存在");
            logincode.Add("11", "您输入的帐号不存在");
            logincode.Add("12", "已经过期的QQ号码");
            logincode.Add("13", "登录失败13");
            logincode.Add("14", "该QQ号已经转换成Email");
            logincode.Add("15", "登录失败15");
            logincode.Add("16", "IP输入错误次数过多");
            logincode.Add("17", "登录失败17");
            logincode.Add("18", "Email帐号未验证");
            logincode.Add("19", "您的号码暂时不能登录");
            logincode.Add("20", "您的号码暂时不能登录20");
        }
        #endregion

        #region //功能函数
        public bool isNeedVerifyCode()
        {
            string url = String.Format("http://ptlogin2.qq.com/check?uin={0}&appid={1}&r={2}", uin, appid, ro.NextDouble().ToString());
            string result = HttpHelper.GetHtml(url, cc);
            if (result.Contains("\'0\'"))
            {
                string vc = "";
                try
                {
                    vc = result.Substring(result.LastIndexOf("\'") - 4, 4);
                }
                catch (Exception)
                {
                    throw new Exception("不需要验证码，但是在截取验证码时出现错误");
                }
                verifyCode = vc;
                return false;
            }
            else if (result.Contains("\'1\'"))
            {
                string vc = "";
                try
                {
                    vc = result.Substring(result.LastIndexOf("\'") - 48, 48);
                }
                catch (Exception)
                {
                    throw new Exception("需要验证码，但是在截取验证码时发生错误");
                }
                verifyCode32 = vc;
                return true;
            }
            else
            {
                return true;
            }
        }
        public Stream getVerifyCodePicStream()
        {
            Stream result = null;
            try
            {
                string url = String.Format("http://captcha.qq.com/getimage?aid={0}&r={1}&uin={2}&vc_type={3}", appid, ro.NextDouble().ToString(), uin, verifyCode32);
                result = HttpHelper.GetStream(url, cc);
            }
            catch (Exception)
            {
            }
            return result;
        }
        public string loginWebQQ(bool offline)
        {
            string result;
            string status = string.Empty;
            if (offline)
            {
                status = @"&webqq_type=1";
            }
            string url = String.Format("http://ptlogin2.qq.com/login?u={0}&p={1}&verifycode={2}&remember_uin=1&aid={3}&u1=http%3A%2F%2Fweb2.qq.com%2Floginproxy.html%3Fstrong%3Dtrue&h=1&ptredirect=0&ptlang=2052&from_ui=1&pttype=1&dumy=&fp=loginerroralert", uin, Password.getPassword(qqpassword, verifyCode), verifyCode + status, appid);
            result = HttpHelper.GetHtml(url, cc);
            string state = "接收错误";
            if (result != "")
            {
                state = result.Substring(8, 2);
                state = logincode[state];
            }
            return state;
        }
        public string loginGetValue(string ptwebqq)
        {
            string result = string.Empty;
            result = HttpHelper.GetHtml("http://d.web2.qq.com/channel/login2", String.Format("r=%7B%22status%22%3A%22%22%2C%22ptwebqq%22%3A%22{0}%22%2C%22passwd_sig%22%3A%22%22%2C%22clientid%22%3A%22{1}%22%2C%22psessionid%22%3Anull%7D", ptwebqq, cliendid), true, cc);
            return result;
        }
        public string getfriendinfo(string touin)
        {
            string result;
            string url = String.Format("http://s.web2.qq.com/api/get_friend_info2?tuin={0}&verifysession=&code=&vfwebqq={1}&t={2}", touin, vfwebqq, ClientID.GetTime(DateTime.Now));
            result = HttpHelper.GetHtml(url, cc);
            return result;
        }
        public string getFriend()
        {
            string result;
            result = HttpHelper.GetHtml("http://s.web2.qq.com/api/get_user_friends2", String.Format("r=%7B%22h%22%3A%22hello%22%2C%22vfwebqq%22%3A%22{0}%22%7D", vfwebqq), true, cc);
            return result;
        }
        public string getonline()
        {
            string result;
            string url = String.Format("http://d.web2.qq.com/channel/get_online_buddies2?clientid={0}&psessionid={1}&t={2}&vfwebqq={3}", cliendid, psessionid, ClientID.GetTime(DateTime.Now), vfwebqq);
            result = HttpHelper.GetHtml(url, cc);
            return result;
        }
        public string getgroup()
        {
            string result;
            result = HttpHelper.GetHtml("http://s.web2.qq.com/api/get_group_name_list_mask2", String.Format("r=%7B%22vfwebqq%22%3A%22{0}%22%7D", vfwebqq), true, cc);
            return result;
        }
        public string sendmessage(string text, string touin)
        {
            string result;
            text = Converts.StrConvUrlEncoding(text, "UTF-8");
            string postdate = String.Format("r=%7B%22to%22%3A{0}%2C%22face%22%3A0%2C%22content%22%3A%22%5B%5C%22{1}%5C%22%2C%5B%5C%22font%5C%22%2C%7B%5C%22name%5C%22%3A%5C%22Arial%5C%22%2C%5C%22size%5C%22%3A%5C%2210%5C%22%2C%5C%22style%5C%22%3A%5B0%2C0%2C0%5D%2C%5C%22color%5C%22%3A%5C%22000000%5C%22%7D%5D%5D%22%2C%22msg_id%22%3A{2}%2C%22clientid%22%3A%22{3}%22%2C%22psessionid%22%3A%22{4}%22%7D", touin, text, messagecount++, cliendid, psessionid);
            result = HttpHelper.GetHtml("http://d.web2.qq.com/channel/send_msg2", postdate, true, cc);
            return result;
        }
        public string sendgroupmessage(string text, string touin)
        {
            string result;
            text = Converts.StrConvUrlEncoding(text, "UTF-8");
            string groupuin = Main.thefriendlist.QQGroups[touin].gid;
            string postdate = String.Format("r=%7B%22group_uin%22%3A{0}%2C%22content%22%3A%22%5B%5C%22{1}%5C%22%2C%5B%5C%22font%5C%22%2C%7B%5C%22name%5C%22%3A%5C%22%E5%AE%8B%E4%BD%93%5C%22%2C%5C%22size%5C%22%3A%5C%2210%5C%22%2C%5C%22style%5C%22%3A%5B0%2C0%2C0%5D%2C%5C%22color%5C%22%3A%5C%22000000%5C%22%7D%5D%5D%22%2C%22msg_id%22%3A{2}%2C%22clientid%22%3A%22{3}%22%2C%22psessionid%22%3A%22{4}%22%7D", groupuin, text, messagecount++, cliendid, psessionid);
            result = HttpHelper.GetHtml("http://d.web2.qq.com/channel/send_group_msg2", postdate, true, cc);
            return result;
        }
        public string getmessage()
        {
            string result;
            string url = String.Format("http://d.web2.qq.com/channel/poll2?clientid={0}&psessionid={1}&t={2}&vfwebqq={3}", cliendid, psessionid, ClientID.GetTime(DateTime.Now), vfwebqq);
            result = HttpHelper.GetHtml(url, cc);
            return result;

        }
        public string getgourpfriendlist(string uin)
        {
            string result;
            string url = String.Format("http://s.web2.qq.com/api/get_group_info_ext2?gcode={0}&vfwebqq={1}&t={2}", uin, vfwebqq, ClientID.GetTime(DateTime.Now));
            result = HttpHelper.GetHtml(url, cc);
            return result;
        }
        public string getlongnick2(string nick)
        {
            string result;
            nick = Converts.StrConvUrlEncoding(nick, "UTF-8");
            string postdate = String.Format("r=%7B%22nlk%22%3A%22{0}%22%2C%22vfwebqq%22%3A%22{1}%22%7D", nick, vfwebqq);
            result = HttpHelper.GetHtml("http://s.web2.qq.com/api/set_long_nick2", postdate, true, cc);

            if (result.IndexOf("\"result\":0") > 0)
            {
                result = "设置签名成功!";
            }
            else
            {
                result = "设置签名失败!";
            }
            return result;
        }
        public string getchangestatus(string status)
        {
            string result;
            switch (status)
            {
                case "在线":
                    status = "online";
                    break;
                case "离开":
                    status = "away";
                    break;
                case "隐身":
                    status = "hidden";
                    break;
            }
            string url = String.Format("http://d.web2.qq.com/channel/change_status2?newstatus={0}&clientid={1}&psessionid={2}&t={3}", status, cliendid, psessionid, ClientID.GetTime(DateTime.Now));
            result = HttpHelper.GetHtml(url, cc);
            if (result.IndexOf("ok") > 0)
            {
                result = "设置状态" + status + "成功!";
            }
            else
            {
                result = "设置状态" + status + "失败!";
            }
            return result;
        }
        public void logout()
        {
            string url = String.Format("http://d.web2.qq.com/channel/logout2?clientid={0}&psessionid={1}&t={2}&vfwebqq={3}", cliendid, psessionid, ClientID.GetTime(DateTime.Now), vfwebqq);
            HttpHelper.GetHtml(url, cc);
        }
        #endregion
    }
}
