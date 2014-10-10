using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace island
{
    public partial class Main_Form : Form
    {
        DateTime dt = DateTime.Now;
        island.WebClient HTTPproc = new WebClient();
        island.WebClient HTTPproc_1 = new WebClient();
        public string strUserid = null;
        public string strSaveUserid = null;
        public string strAuthID = null;
        public string strSESSION = null;
        public string strUserName = null;
        public string strPassword = null;
        public string strSwitch = "renren";
        public string strPHPSESSID = null;

        public Main_Form()
        {
            InitializeComponent();
            //控件状态设置
            label_Main_Form_NickName.Visible = false;
            label_Main_Form_UserID.Visible = false;
            pictureBox_Main_Form.Visible = false;
            label_Main_Form_Exp.Visible = false;
            label_Main_Form_Level.Visible = false;
            label_Main_Form_Coin.Visible = false;
            label_Main_Form_Praise.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;

        }

        #region 登录校内流程 Login_Handle()
        /// <summary>
        /// 登录校内流程
        /// </summary>
        public void Login_Handle()
        {
            string strContent = null;
            string resultString = null;

            if (strUserName.Trim() == "1" && strPassword.Trim() == "1")
            {
                strUserName = "cupid0426@163.com";
                strPassword = "loveemma++";
            }
            if (strUserName.Trim() != "" && strPassword.Trim() != "")
            {
                //设置HTTP请求默认编码
                HTTPproc.Encoding = System.Text.Encoding.UTF8;
                string strRedirectURL = HTTPproc.OpenRead("http://www.renren.com/PLogin.do", "email=" + UrlEncode(strUserName.Trim(), "UTF-8") + "&password=" + UrlEncode(strPassword.Trim(), "UTF-8") + "&origURL=http%3A%2F%2Fwww.renren.com%2FSysHome.do&domain=renren.com");
                try
                {
                    strRedirectURL = strRedirectURL.Replace("The URL has moved <a href=\"", "").Replace("\">here</a>", "");
                    HTTPproc.OpenRead(strRedirectURL);
                    HTTPproc.OpenRead("http://www.renren.com/SysHome.do");

                    strContent = HTTPproc.OpenRead("http://www.renren.com/Home.do");
                    if (strContent.IndexOf("guide.do") > 0)
                    {
                        HTTPproc.OpenRead("http://guide.renren.com/guide.do");
                        strContent = HTTPproc.OpenRead("http://guide.renren.com/guidexf.do");
                    }
                    ReadUserInfo(strContent);
                    Login_Game();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("用户名或密码出错", "系统消息");
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("信息填写有误请检查！", "系统消息");
            }
        }
        #endregion 登录校内流程 Login_Handle()

        #region 登录游戏流程 Login_Game()
        /// <summary>
        /// 登录游戏流程
        /// </summary>
        private void Login_Game()
        {
            string strContent = null;
            string resultString = null;
            string resultString_1 = null;

            strContent = HTTPproc.OpenRead("http://apps.renren.com/rrisland");

            try
            {
                strAuthID = Regex.Match(HTTPproc.Cookie, @"_xnc_PHPSESSID=\w{0,32}").Value;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            try
            {
                resultString_1 = Regex.Match(strContent, @"xn_validation_vars=\{"".*\}").Value;
                try
                {
                    resultString = Regex.Match(resultString_1, "xn_sig_api_key.*\",\"xn_sig_app_id").Value;
                    //strAuthID = resultString.Replace("xn_sig_api_key\":\"", "").Replace("\",\"xn_sig_app_id", "");
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                resultString_1 = resultString_1.Replace("xn_validation_vars={", "").Replace("}", "").Replace("\"", "").Replace(",", "&").Replace(":", "=");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            try
            {
                resultString = Regex.Match(strContent, @"http://.*/show\?userid=.*&tt=.*&type=\.js").Value;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            HTTPproc.RequestHeaders.Add("Referer:http://apps.renren.com/rrisland");
            HTTPproc.OpenRead(resultString);
            HTTPproc.RequestHeaders.Add("Referer:http://apps.renren.com/rrisland");
            strContent = "http://rrisland.hapyfish.com/flash?v=11&xn_sig_in_iframe=1&" + resultString_1;
            strContent = HTTPproc.OpenRead(strContent);
            try
            {
                resultString = Regex.Match(strContent, "scode.*\"l").Value;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            strSESSION = UserMd5(resultString.Replace("scode\":\"", "").Replace("\",\"uid\":\"", "").Replace("\",\"l", ""));

            HTTPproc_1.RequestHeaders.Add("Cookie:PHPSESSID=" + strAuthID.Replace("_xnc_PHPSESSID=", ""));
            HTTPproc_1.RequestHeaders.Add("Referer:http://static.hapyfish.com/renren/swf/v17/islandLoader.swf");
            HTTPproc_1.OpenRead("http://rrisland.hapyfish.com/api/initswf", "authid=" + strSESSION);
            LoadUserInfo();
            LoadFriends();
            LoadBoatInfo(1, "", "");
            LoadBoatInfo(0, "", "");
            LoadBuildInfo(1, "", "");
            LoadBuildInfo(0, "", "");
        }
        #endregion

        #region 读取用户信息 ReadUserInfo(string strContent)
        /// <summary>
        /// 读取用户信息
        /// </summary>
        /// <param name="strContent"></param>
        private void ReadUserInfo(string strContent)
        {
            string resultString = null;
            //try
            //{
            //    resultString = Regex.Match(strContent, "XN.user.tinyPic = '.*'").Value;
            //    pictureBox_Main_Form.ImageLocation = resultString.Replace("XN.user.tinyPic = '", "").Replace("'", "");
            //}
            //catch (ArgumentException ex)
            //{
            //    // Syntax error in the regular expression
            //}

            try
            {
                resultString = Regex.Match(strContent, "XN.user.id = '.*'").Value;
                strUserid = resultString.Replace("XN.user.id = '", "").Replace("'", "");
                strSaveUserid = resultString.Replace("XN.user.id = '", "").Replace("'", "");
                //label_Main_Form_UserID.Text += strUserid;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            //try
            //{
            //    resultString = Regex.Match(strContent, "<title>.*</title>").Value;
            //    label_Main_Form_NickName.Text += resultString.Replace("<title>人人网 校内 - ", "").Replace("</title>", "");
            //}
            //catch (ArgumentException ex)
            //{
            //    // Syntax error in the regular expression
            //}
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

        #region 时间戳 ConvertDateTimeInt(DateTime time)
        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public double ConvertDateTimeInt(DateTime time)
        {
            double intResult = 0;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            if (strSwitch == "renren")
            {
                return intResult * 10;
            }
            else
            {
                return intResult * 1000;
            }
        }
        #endregion

        #region MD5加密算法 UserMd5(string str)
        /// <summary>
        /// MD5加密算法
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <returns>输出32位小写MD5加密值</returns>
        static string UserMd5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像  
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　  
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得  
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                pwd = pwd + s[i].ToString("x2");
            }
            return pwd;
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

        #region 读取并设置用户信息 LoadUserInfo()
        /// <summary>
        /// 读取并设置用户信息
        /// </summary>
        private void LoadUserInfo()
        {
            //用户信息调取URL
            //http://rrisland.hapyfish.com/api/inituserinfo

            string strContent = null;

            if (strSwitch == "renren")
            {
                HTTPproc_1.RequestHeaders.Add("Cookie:PHPSESSID=" + strAuthID.Replace("_xnc_PHPSESSID=", ""));
                HTTPproc_1.RequestHeaders.Add("Referer:http://static.hapyfish.com/renren/swf/v17/islandLoader.swf");
                strContent = HTTPproc_1.OpenRead("http://rrisland.hapyfish.com/api/inituserinfo", "authid=" + strSESSION);
            }
            else
            {
                HTTPproc_1.RequestHeaders.Add("Cookie:" + strPHPSESSID);
                HTTPproc_1.RequestHeaders.Add("Referer:http://tbstatic.hapyfish.com/swf/v06/islandLoader.swf");
                strContent = HTTPproc_1.OpenRead("http://island.hapyfish.com/api/inituserinfo", "authid=" + strSESSION);
            }

            label_Main_Form_NickName.Visible = true;
            label_Main_Form_UserID.Visible = true;
            pictureBox_Main_Form.Visible = true;
            label_Main_Form_Exp.Visible = true;
            label_Main_Form_Level.Visible = true;
            label_Main_Form_Coin.Visible = true;
            label_Main_Form_Praise.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;

            label_Main_Form_Exp.ForeColor = Color.Red;
            label_Main_Form_Level.ForeColor = Color.Red;
            label_Main_Form_Coin.ForeColor = Color.Red;
            label_Main_Form_Praise.ForeColor = Color.Red;

            JObject o = JObject.Parse(Unicode2Character(strContent));
            pictureBox_Main_Form.ImageLocation = (string)o["user"]["smallFace"];
            label_Main_Form_UserID.Text = "校内ID:" + (string)o["user"]["uid"];
            label_Main_Form_NickName.Text = "昵称:" + (string)o["user"]["name"];
            label_Main_Form_Exp.Text = (string)o["user"]["exp"] + "/" + (string)o["user"]["maxExp"];
            label_Main_Form_Level.Text = (string)o["user"]["level"];
            label_Main_Form_Coin.Text = (string)o["user"]["coin"];
            label_Main_Form_Praise.Text = (string)o["user"]["praise"];
        }
        #endregion

        #region 显示系统日志 ShowSysLog(string strLog)
        /// <summary>
        /// 显示系统日志
        /// </summary>
        /// <param name="strLog"></param>        
        private void ShowSysLog(string strLog)
        {
            DateTime dt_1 = DateTime.Now;
            Main_Form_textBox1.Text += strLog + "    " + dt_1 + "\r\n";
            Main_Form_textBox1.SelectionStart = Main_Form_textBox1.Text.Length;
            Main_Form_textBox1.ScrollToCaret();
            //始终显示TextBox最新一行，始终滚动到最底部
            Main_Form_textBox1.SelectionStart = Main_Form_textBox1.Text.Length;
            Main_Form_textBox1.ScrollToCaret();
        }
        #endregion

        #region 读取好友列表并显示 LoadFriends()
        /// <summary>
        /// 读取好友列表并显示
        /// </summary>
        private void LoadFriends()
        {
            //用户好友调取信息
            //http://rrisland.hapyfish.com/api/getfriends

            string resultString = null;
            string strContent = null;

            if (strSwitch == "renren")
            {
                HTTPproc_1.RequestHeaders.Add("Cookie:PHPSESSID=" + strAuthID.Replace("_xnc_PHPSESSID=", ""));
                HTTPproc_1.RequestHeaders.Add("Referer:http://static.hapyfish.com/renren/swf/v17/islandLoader.swf");
                strContent = HTTPproc_1.OpenRead("http://rrisland.hapyfish.com/api/getfriends", "authid=" + strSESSION);
            }
            else
            {
                HTTPproc_1.RequestHeaders.Add("Cookie:" + strPHPSESSID);
                HTTPproc_1.RequestHeaders.Add("Referer:http://tbstatic.hapyfish.com/swf/v06/islandLoader.swf");
                strContent = HTTPproc_1.OpenRead("http://island.hapyfish.com/api/getfriends", "authid=" + strSESSION);
            }
            JObject o = JObject.Parse(CleanBadWord(Unicode2Character(strContent)));
            JArray friends = (JArray)o["friends"];

            for (int i = 0; i < friends.Count(); i++)
            {
                if ((string)friends[i]["name"] != "乐乐")
                {
                    ListViewItem li = new ListViewItem();
                    li.SubItems.Clear();
                    li.SubItems[0].Text = (string)friends[i]["uid"];
                    li.SubItems.Add((string)friends[i]["name"]);
                    li.SubItems.Add((string)friends[i]["level"]);
                    listView_Main_Form_FriendList.Items.Add(li);
                }
            }

            ShowSysLog("好友列表加载完成");
            //listView_Main_Form_FriendList.Items[listView_Main_Form_FriendList.Items.Count - 1].Remove();
        }
        #endregion

        #region 加载用户港口信息及操作方法 LoadBoatInfo(int intOpID, string strUserID, string strNickName)
        /// <summary>
        /// 加载用户港口信息及操作方法
        /// </summary>
        /// <param name="intOpID">0,读取用户港口信息 1,操作港口船只 2,偷取好友船只</param>
        /// <param name="strUserID"></param>
        /// <param name="strNickName"></param>
        private void LoadBoatInfo(int intOpID, string strUserID, string strNickName)
        {
            //加载用户港口信息
            //方法:POST
            //参数:authid=fad254cddb744968ecbf79cab3bb483b&ownerUid=228618602
            //http://rrisland.hapyfish.com/api/initisland?time=1278580378815

            string strBoatState = null;
            string strContent = null;
            ShowSysLog("正在加载港口信息");

            if (strSwitch == "renren")
            {
                HTTPproc_1.RequestHeaders.Add("Cookie:PHPSESSID=" + strAuthID.Replace("_xnc_PHPSESSID=", ""));
                HTTPproc_1.RequestHeaders.Add("Referer:http://static.hapyfish.com/renren/swf/v17/islandLoader.swf");
                if (strUserID == "")
                {
                    strContent = HTTPproc_1.OpenRead("http://rrisland.hapyfish.com/api/initisland?time=" + Convert.ToInt64(ConvertDateTimeInt(dt)).ToString(), "authid=" + strSESSION + "&ownerUid=" + strUserid);
                }
                else
                {
                    strContent = HTTPproc_1.OpenRead("http://rrisland.hapyfish.com/api/initisland?time=" + Convert.ToInt64(ConvertDateTimeInt(dt)).ToString(), "authid=" + strSESSION + "&ownerUid=" + strUserID);
                }
            }
            else
            {
                HTTPproc_1.RequestHeaders.Add("Cookie:" + strPHPSESSID);
                HTTPproc_1.RequestHeaders.Add("Referer:http://tbstatic.hapyfish.com/swf/v06/islandLoader.swf");
                if (strUserID == "")
                {
                    strContent = HTTPproc_1.OpenRead("http://island.hapyfish.com/api/initisland?time=" + Convert.ToInt64(ConvertDateTimeInt(dt)).ToString(), "authid=" + strSESSION + "&ownerUid=" + strUserid);
                }
                else
                {
                    strContent = HTTPproc_1.OpenRead("http://island.hapyfish.com/api/initisland?time=" + Convert.ToInt64(ConvertDateTimeInt(dt)).ToString(), "authid=" + strSESSION + "&ownerUid=" + strUserID);
                }
            }

            JObject o = JObject.Parse(CleanBadWord(Unicode2Character(strContent)));
            JArray boatInfo = (JArray)o["dockVo"]["boatPositions"];

            if (intOpID == 0)
            {
                if (listView_Main_Form_BoatList.Items.Count > 0)
                {
                    foreach (ListViewItem li in listView_Main_Form_BoatList.Items)
                    {
                        if (listView_Main_Form_BoatList.Items.Count > 0)
                        {
                            li.Remove();
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int i = 0; i < boatInfo.Count(); i++)
                    {
                        ListViewItem li = new ListViewItem();
                        li.SubItems.Clear();
                        li.SubItems[0].Text = (string)boatInfo[i]["id"];
                        li.SubItems.Add(BoatClass((string)boatInfo[i]["boatId"]));
                        li.SubItems.Add(TimeCal((int)boatInfo[i]["time"]));
                        li.SubItems.Add(BoatState((string)boatInfo[i]["state"]));
                        listView_Main_Form_BoatList.Items.Add(li);
                    }
                    ShowSysLog("船只信息加载完成");
                }
                else
                {
                    for (int i = 0; i < boatInfo.Count(); i++)
                    {
                        ListViewItem li = new ListViewItem();
                        li.SubItems.Clear();
                        li.SubItems[0].Text = (string)boatInfo[i]["id"];
                        li.SubItems.Add(BoatClass((string)boatInfo[i]["boatId"]));
                        li.SubItems.Add(TimeCal((int)boatInfo[i]["time"]));
                        li.SubItems.Add(BoatState((string)boatInfo[i]["state"]));
                        listView_Main_Form_BoatList.Items.Add(li);
                    }
                    ShowSysLog("船只信息加载完成");
                    //listView_Main_Form_BoatList.Items[listView_Main_Form_BoatList.Items.Count - 1].Remove();
                }
            }

            if ((bool)o["islandVo"]["isFriend"] == false)
            {
                intOpID = 1;
            }
            else
            {
                intOpID = 2;
            }

            if (intOpID == 1)
            {
                for (int i = 0; i < boatInfo.Count(); i++)
                {
                    //发船调用
                    //方法:POST
                    //参数:authid=fad254cddb744968ecbf79cab3bb483b&positionId=2
                    //http://rrisland.hapyfish.com/api/receiveboat

                    if ((string)boatInfo[i]["state"] == "arrive_1" || (string)boatInfo[i]["state"] == "arrive_2")
                    {
                        if (strSwitch == "renren")
                        {
                            HTTPproc_1.RequestHeaders.Add("Cookie:PHPSESSID=" + strAuthID.Replace("_xnc_PHPSESSID=", ""));
                            HTTPproc_1.RequestHeaders.Add("Referer:http://static.hapyfish.com/renren/swf/v17/islandLoader.swf");
                            HTTPproc_1.OpenRead("http://rrisland.hapyfish.com/api/receiveboat", "authid=" + strSESSION + "&positionId=" + (string)boatInfo[i]["id"]);
                        }
                        else
                        {
                            HTTPproc_1.RequestHeaders.Add("Cookie:" + strPHPSESSID);
                            HTTPproc_1.RequestHeaders.Add("Referer:http://tbstatic.hapyfish.com/swf/v06/islandLoader.swf");
                            HTTPproc_1.OpenRead("http://island.hapyfish.com/api/receiveboat", "authid=" + strSESSION + "&positionId=" + (string)boatInfo[i]["id"]);
                        }
                        ShowSysLog((string)boatInfo[i]["id"] + "号船出港");
                    }
                }
            }
            if (intOpID == 2)
            {
                ShowSysLog("好友（" + strNickName + "）港口信息加载完成");
                for (int i = 0; i < boatInfo.Count(); i++)
                {
                    //偷取港口乘客调用
                    //方法:POST
                    //参数:positionId=1&authid=b375b7c1ef01d91036383cf11ad5f1ba&ownerUid=272438384
                    //http://rrisland.hapyfish.com/api/moochvisitor

                    if ((string)boatInfo[i]["state"] == "arrive_2")
                    {
                        if (strSwitch == "renren")
                        {
                            HTTPproc_1.RequestHeaders.Add("Cookie:PHPSESSID=" + strAuthID.Replace("_xnc_PHPSESSID=", ""));
                            HTTPproc_1.RequestHeaders.Add("Referer:http://static.hapyfish.com/renren/swf/v17/islandLoader.swf");
                            strContent = HTTPproc_1.OpenRead("http://rrisland.hapyfish.com/api/moochvisitor", "positionId=" + (string)boatInfo[i]["id"] + "&authid=" + strSESSION + "&ownerUid=" + strUserID);
                        }
                        else
                        {
                            HTTPproc_1.RequestHeaders.Add("Cookie:" + strPHPSESSID);
                            HTTPproc_1.RequestHeaders.Add("Referer:http://tbstatic.hapyfish.com/swf/v06/islandLoader.swf");
                            strContent = HTTPproc_1.OpenRead("http://island.hapyfish.com/api/moochvisitor", "positionId=" + (string)boatInfo[i]["id"] + "&authid=" + strSESSION + "&ownerUid=" + strUserID);
                        }

                        o = JObject.Parse(Unicode2Character(strContent));
                        if ((int)o["result"]["status"] > 0)
                        {
                            ShowSysLog("偷取好友（" + strNickName + "） " + (string)boatInfo[i]["id"] + "号船成功");
                        }
                        else
                        {
                            ShowSysLog("偷取好友（" + strNickName + "） " + (string)boatInfo[i]["id"] + "号船失败");
                        }
                    }
                    else
                    {
                        ShowSysLog("好友（" + strNickName + "） " + (string)boatInfo[i]["id"] + "号船不在港口");
                    }
                }
            }
        }
        #endregion

        #region 获取船只名称 BoatClass(string strID)
        /// <summary>
        /// 获取船只名称
        /// </summary>
        /// <param name="intID"></param>
        /// <returns></returns>
        private string BoatClass(string strID)
        {
            string strBoatName = null;

            switch (strID)
            {
                case "1":
                    strBoatName = "小木筏";
                    break;
                case "2":
                    strBoatName = "木舟";
                    break;
                case "3":
                    strBoatName = "橡皮艇";
                    break;
                case "4":
                    strBoatName = "帆木舟";
                    break;
                case "5":
                    strBoatName = "大帆船";
                    break;
                case "6":
                    strBoatName = "白色快艇";
                    break;
                case "7":
                    strBoatName = "红色快艇";
                    break;
                case "8":
                    strBoatName = "豪华游轮";
                    break;
                default:
                    strBoatName = "UFO";
                    break;
            }
            return strBoatName;
        }
        #endregion

        #region 获取在港船只状态 BoatState(string strState)
        /// <summary>
        /// 获取在港船只状态
        /// </summary>
        /// <param name="strState"></param>
        /// <returns></returns>
        private string BoatState(string strState)
        {
            switch (strState)
            {
                case "arrive_1":
                    strState = "空船";
                    break;
                case "arrive_2":
                    strState = "在港";
                    break;
                case "onTheRoad":
                    strState = "离港";
                    break;
                default:
                    strState = "在天上";
                    break;
            }
            return strState;
        }
        #endregion

        #region 时间换算 TimeCal(int intTime)
        /// <summary>
        /// 时间换算
        /// </summary>
        /// <param name="intTime"></param>
        /// <returns></returns>
        private string TimeCal(int intTime)
        {
            string strTime = null;
            intTime = Math.Abs(intTime);
            if (intTime > 0)
            {
                strTime = Convert.ToString(dt.AddSeconds(Convert.ToDouble(intTime)));
                return strTime;
            }
            else
            {
                return strTime = "现在";
            }
        }
        #endregion

        #region 双击好友列表偷取好友港口船只
        /// <summary>
        /// 双击好友列表偷取好友港口船只
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_Main_Form_FriendList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string strUserID = null;
            string strNickName = null;
            foreach (ListViewItem li in listView_Main_Form_FriendList.SelectedItems)
            {
                if (listView_Main_Form_FriendList.SelectedIndices.Count > 0)
                {
                    strUserID = li.SubItems[0].Text;
                    strNickName = li.SubItems[1].Text;
                    LoadBoatInfo(2, strUserID, strNickName);
                    //LoadBuildInfo(0, strUserID, strNickName);
                    LoadBoatInfo(0, strUserID, strNickName);
                    LoadBuildInfo(2, strUserID, strNickName);
                    LoadBuildInfo(0, strUserID, strNickName);
                    LoadUserInfo();
                }
                else
                {
                    break;
                }
            }
        }
        #endregion

        #region 清理系统扰乱字 CleanBadWord(string strContent)
        /// <summary>
        /// 清理系统扰乱字
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        private string CleanBadWord(string strContent)
        {
            string resultString = null;
            try
            {
                resultString = Regex.Replace(strContent, @"\w{0,4}\r\n", "");
                resultString = Regex.Replace(resultString, @"\w{0,4}\r\n", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            return resultString;
        }
        #endregion

        #region 获取建筑名称 BuildClass(string strID)
        /// <summary>
        /// 获取建筑名称
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        private string BuildClass(string strID)
        {
            string strBuildName = null;
            switch (strID)
            {
                case "6221":
                    strBuildName = "垃圾桶";
                    break;
                case "6321":
                    strBuildName = "垃圾桶";
                    break;
                case "6421":
                    strBuildName = "盆栽";
                    break;
                case "6521":
                    strBuildName = "盆栽";
                    break;
                case "6621":
                    strBuildName = "盆栽";
                    break;
                case "6721":
                    strBuildName = "雪松";
                    break;
                case "6821":
                    strBuildName = "柏树";
                    break;
                case "6921":
                    strBuildName = "西瓜树";
                    break;
                case "7021":
                    strBuildName = "梧桐树";
                    break;
                case "7121":
                    strBuildName = "彩叶树";
                    break;
                case "7221":
                    strBuildName = "白云树";
                    break;
                case "7321":
                    strBuildName = "星星树";
                    break;
                case "7421":
                    strBuildName = "桃树";
                    break;
                case "7521":
                    strBuildName = "蘑菇头树";
                    break;
                case "7621":
                    strBuildName = "贝壳";
                    break;
                case "7721":
                    strBuildName = "足球";
                    break;
                case "7821":
                    strBuildName = "草坪";
                    break;
                case "7921":
                    strBuildName = "沙包2";
                    break;
                case "8021":
                    strBuildName = "沙包1";
                    break;
                case "8121":
                    strBuildName = "足球";
                    break;
                case "8221":
                    strBuildName = "篮球";
                    break;
                case "8321":
                    strBuildName = "黑8撞球";
                    break;
                case "8421":
                    strBuildName = "9号球";
                    break;
                case "8521":
                    strBuildName = "保龄球";
                    break;
                case "8621":
                    strBuildName = "十字架";
                    break;
                case "8721":
                    strBuildName = "粉红小屋";
                    break;
                case "8921":
                    strBuildName = "栅栏转角";
                    break;
                case "9021":
                    strBuildName = "栅栏转角";
                    break;
                case "9121":
                    strBuildName = "栅栏转角";
                    break;
                case "9221":
                    strBuildName = "栅栏转角";
                    break;
                case "9321":
                    strBuildName = "栅栏转角";
                    break;
                case "9421":
                    strBuildName = "栅栏转角";
                    break;
                case "9521":
                    strBuildName = "西瓜栅栏转角";
                    break;
                case "9621":
                    strBuildName = "西瓜栅栏转角";
                    break;
                case "9721":
                    strBuildName = "西瓜栅栏转角";
                    break;
                case "9821":
                    strBuildName = "西瓜栅栏转角";
                    break;
                case "9921":
                    strBuildName = "西瓜栅栏转角";
                    break;
                case "10021":
                    strBuildName = "西瓜栅栏转角";
                    break;
                case "10121":
                    strBuildName = "西瓜栅栏转角";
                    break;
                case "10221":
                    strBuildName = "西瓜栅栏转角";
                    break;
                case "10321":
                    strBuildName = "警告栅栏转角";
                    break;
                case "10421":
                    strBuildName = "警告栅栏转角";
                    break;
                case "10521":
                    strBuildName = "警告栅栏转角";
                    break;
                case "10621":
                    strBuildName = "警告栅栏转角";
                    break;
                case "10721":
                    strBuildName = "警告栅栏转角";
                    break;
                case "10821":
                    strBuildName = "警告栅栏转角";
                    break;
                case "10921":
                    strBuildName = "警告栅栏转角";
                    break;
                case "11021":
                    strBuildName = "警告栅栏转角";
                    break;
                case "11121":
                    strBuildName = "萝卜栅栏转角";
                    break;
                case "11221":
                    strBuildName = "萝卜栅栏转角";
                    break;
                case "11321":
                    strBuildName = "萝卜栅栏转角";
                    break;
                case "11421":
                    strBuildName = "萝卜栅栏转角";
                    break;
                case "11521":
                    strBuildName = "萝卜栅栏转角";
                    break;
                case "11621":
                    strBuildName = "萝卜栅栏转角";
                    break;
                case "11721":
                    strBuildName = "萝卜栅栏转角";
                    break;
                case "11821":
                    strBuildName = "萝卜栅栏转角";
                    break;
                case "12021":
                    strBuildName = "黄笔栅栏转角";
                    break;
                case "12121":
                    strBuildName = "黄笔栅栏转角";
                    break;
                case "12221":
                    strBuildName = "黄笔栅栏转角";
                    break;
                case "12321":
                    strBuildName = "黄笔栅栏转角";
                    break;
                case "12421":
                    strBuildName = "黄笔栅栏转角";
                    break;
                case "12521":
                    strBuildName = "黄笔栅栏转角";
                    break;
                case "12621":
                    strBuildName = "黄笔栅栏转角";
                    break;
                case "12721":
                    strBuildName = "黄笔栅栏转角";
                    break;
                case "12821":
                    strBuildName = "绿笔栅栏转角";
                    break;
                case "12921":
                    strBuildName = "绿笔栅栏转角";
                    break;
                case "13021":
                    strBuildName = "绿笔栅栏转角";
                    break;
                case "13121":
                    strBuildName = "绿笔栅栏转角";
                    break;
                case "13221":
                    strBuildName = "绿笔栅栏转角";
                    break;
                case "13321":
                    strBuildName = "绿笔栅栏转角";
                    break;
                case "13421":
                    strBuildName = "绿笔栅栏转角";
                    break;
                case "13521":
                    strBuildName = "绿笔栅栏转角";
                    break;
                case "13621":
                    strBuildName = "红笔栅栏转角";
                    break;
                case "13721":
                    strBuildName = "红笔栅栏转角";
                    break;
                case "13821":
                    strBuildName = "红笔栅栏转角";
                    break;
                case "13921":
                    strBuildName = "红笔栅栏转角";
                    break;
                case "14021":
                    strBuildName = "红笔栅栏转角";
                    break;
                case "14121":
                    strBuildName = "红笔栅栏转角";
                    break;
                case "14221":
                    strBuildName = "红笔栅栏转角";
                    break;
                case "14321":
                    strBuildName = "红笔栅栏转角";
                    break;
                case "21721":
                    strBuildName = "路灯";
                    break;
                case "21821":
                    strBuildName = "路灯";
                    break;
                case "21921":
                    strBuildName = "路灯";
                    break;
                case "22121":
                    strBuildName = "花灯";
                    break;
                case "32921":
                    strBuildName = "红心气球";
                    break;
                case "33021":
                    strBuildName = "蓝心气球";
                    break;
                case "33121":
                    strBuildName = "绿篱灌木";
                    break;
                case "33221":
                    strBuildName = "红玫绿篱灌木";
                    break;
                case "33321":
                    strBuildName = "白玫绿篱灌木";
                    break;
                case "33421":
                    strBuildName = "白玫瑰花坛";
                    break;
                case "33521":
                    strBuildName = "树雕";
                    break;
                case "33621":
                    strBuildName = "粉色长椅";
                    break;
                case "33721":
                    strBuildName = "白色长椅";
                    break;
                case "33821":
                    strBuildName = "绿色长椅";
                    break;
                case "33921":
                    strBuildName = "地灯";
                    break;
                case "34021":
                    strBuildName = "木桩";
                    break;
                case "34121":
                    strBuildName = "天使雕像";
                    break;
                case "34221":
                    strBuildName = "黄玫瑰花座";
                    break;
                case "34321":
                    strBuildName = "百合花座";
                    break;
                case "34421":
                    strBuildName = "小溪流水池";
                    break;
                case "34521":
                    strBuildName = "小型喷泉池";
                    break;
                case "34621":
                    strBuildName = "荷花水潭";
                    break;
                case "35821":
                    strBuildName = "灰色垃圾桶";
                    break;
                case "35921":
                    strBuildName = "黄金垃圾桶";
                    break;
                case "36221":
                    strBuildName = "心心路障";
                    break;
                case "36321":
                    strBuildName = "竖心路障";
                    break;
                case "36421":
                    strBuildName = "爱心树";
                    break;
                case "36521":
                    strBuildName = "粉色爱心树";
                    break;
                case "36621":
                    strBuildName = "兔耳气球";
                    break;
                case "36721":
                    strBuildName = "玉树祈福";
                    break;
                case "36821":
                    strBuildName = "红色邮筒";
                    break;
                case "36921":
                    strBuildName = "投票箱";
                    break;
                case "37021":
                    strBuildName = "UFO";
                    break;
                case "37121":
                    strBuildName = "阿拉伯亭";
                    break;
                case "37221":
                    strBuildName = "电话亭";
                    break;
                case "37321":
                    strBuildName = "红色指示牌";
                    break;
                case "37421":
                    strBuildName = "绿色指示牌";
                    break;
                case "37521":
                    strBuildName = "眼镜母鸡雕像";
                    break;
                case "38221":
                    strBuildName = "峡谷";
                    break;
                case "42421":
                    strBuildName = "石墩";
                    break;
                case "42521":
                    strBuildName = "救生圈";
                    break;
                case "42621":
                    strBuildName = "罗马柱";
                    break;
                case "42721":
                    strBuildName = "蜡烛灯台";
                    break;
                case "42821":
                    strBuildName = "水晶球";
                    break;
                case "42921":
                    strBuildName = "蓝色摇椅";
                    break;
                case "43021":
                    strBuildName = "童话南瓜车";
                    break;
                case "43121":
                    strBuildName = "童话水晶鞋";
                    break;
                case "132":
                    strBuildName = "厕所";
                    break;
                case "232":
                    strBuildName = "厕所";
                    break;
                case "332":
                    strBuildName = "厕所";
                    break;
                case "432":
                    strBuildName = "厕所";
                    break;
                case "532":
                    strBuildName = "厕所";
                    break;
                case "632":
                    strBuildName = "旅馆";
                    break;
                case "732":
                    strBuildName = "旅馆";
                    break;
                case "832":
                    strBuildName = "旅馆";
                    break;
                case "932":
                    strBuildName = "旅馆";
                    break;
                case "1032":
                    strBuildName = "旅馆";
                    break;
                case "1132":
                    strBuildName = "蛋糕店";
                    break;
                case "1232":
                    strBuildName = "蛋糕店";
                    break;
                case "1332":
                    strBuildName = "蛋糕店";
                    break;
                case "1432":
                    strBuildName = "蛋糕店";
                    break;
                case "1532":
                    strBuildName = "蛋糕店";
                    break;
                case "1632":
                    strBuildName = "饮料亭";
                    break;
                case "1732":
                    strBuildName = "饮料亭";
                    break;
                case "1832":
                    strBuildName = "饮料亭";
                    break;
                case "1932":
                    strBuildName = "饮料亭";
                    break;
                case "2032":
                    strBuildName = "饮料亭";
                    break;
                case "2132":
                    strBuildName = "快餐店";
                    break;
                case "2232":
                    strBuildName = "快餐店";
                    break;
                case "2332":
                    strBuildName = "快餐店";
                    break;
                case "2432":
                    strBuildName = "快餐店";
                    break;
                case "2532":
                    strBuildName = "快餐店";
                    break;
                case "2631":
                    strBuildName = "城堡";
                    break;
                case "2731":
                    strBuildName = "城堡";
                    break;
                case "2831":
                    strBuildName = "城堡";
                    break;
                case "2931":
                    strBuildName = "城堡";
                    break;
                case "3031":
                    strBuildName = "城堡";
                    break;
                case "3132":
                    strBuildName = "露营";
                    break;
                case "3232":
                    strBuildName = "露营";
                    break;
                case "3332":
                    strBuildName = "露营";
                    break;
                case "3431":
                    strBuildName = "海盗船";
                    break;
                case "3531":
                    strBuildName = "海盗船";
                    break;
                case "3631":
                    strBuildName = "海盗船";
                    break;
                case "3931":
                    strBuildName = "风车";
                    break;
                case "4031":
                    strBuildName = "风车";
                    break;
                case "4131":
                    strBuildName = "风车";
                    break;
                case "4231":
                    strBuildName = "风车";
                    break;
                case "4331":
                    strBuildName = "风车";
                    break;
                case "4431":
                    strBuildName = "沙滩排球";
                    break;
                case "4531":
                    strBuildName = "沙滩排球";
                    break;
                case "4631":
                    strBuildName = "沙滩排球";
                    break;
                case "4732":
                    strBuildName = "教堂";
                    break;
                case "4832":
                    strBuildName = "教堂";
                    break;
                case "4932":
                    strBuildName = "教堂";
                    break;
                case "5032":
                    strBuildName = "教堂";
                    break;
                case "5132":
                    strBuildName = "教堂";
                    break;
                case "5232":
                    strBuildName = "花店";
                    break;
                case "5332":
                    strBuildName = "花店";
                    break;
                case "5432":
                    strBuildName = "花店";
                    break;
                case "5532":
                    strBuildName = "花店";
                    break;
                case "5632":
                    strBuildName = "花店";
                    break;
                case "5731":
                    strBuildName = "剧院";
                    break;
                case "5831":
                    strBuildName = "剧院";
                    break;
                case "5931":
                    strBuildName = "剧院";
                    break;
                case "6031":
                    strBuildName = "剧院";
                    break;
                case "6131":
                    strBuildName = "剧院";
                    break;
                case "14431":
                    strBuildName = "卡拉ok";
                    break;
                case "14531":
                    strBuildName = "卡拉ok";
                    break;
                case "14631":
                    strBuildName = "卡拉ok";
                    break;
                case "14732":
                    strBuildName = "礼品店";
                    break;
                case "14832":
                    strBuildName = "礼品店";
                    break;
                case "14932":
                    strBuildName = "礼品店";
                    break;
                case "15032":
                    strBuildName = "礼品店";
                    break;
                case "15132":
                    strBuildName = "礼品店";
                    break;
                case "15231":
                    strBuildName = "鲸鱼馆";
                    break;
                case "15431":
                    strBuildName = "鲸鱼馆";
                    break;
                case "15531":
                    strBuildName = "过山车";
                    break;
                case "15631":
                    strBuildName = "过山车";
                    break;
                case "15731":
                    strBuildName = "过山车";
                    break;
                case "15831":
                    strBuildName = "过山车";
                    break;
                case "15931":
                    strBuildName = "过山车";
                    break;
                case "16031":
                    strBuildName = "鬼屋";
                    break;
                case "16131":
                    strBuildName = "鬼屋";
                    break;
                case "16231":
                    strBuildName = "鬼屋";
                    break;
                case "16331":
                    strBuildName = "鬼屋";
                    break;
                case "16431":
                    strBuildName = "鬼屋";
                    break;
                case "16532":
                    strBuildName = "SPA";
                    break;
                case "16632":
                    strBuildName = "SPA";
                    break;
                case "16732":
                    strBuildName = "SPA";
                    break;
                case "16832":
                    strBuildName = "SPA";
                    break;
                case "16932":
                    strBuildName = "SPA";
                    break;
                case "17032":
                    strBuildName = "冰淇淋店";
                    break;
                case "17132":
                    strBuildName = "冰淇淋店";
                    break;
                case "17232":
                    strBuildName = "冰淇淋店";
                    break;
                case "17332":
                    strBuildName = "冰淇淋店";
                    break;
                case "17432":
                    strBuildName = "冰淇淋店";
                    break;
                case "17532":
                    strBuildName = "沙滩椅";
                    break;
                case "17632":
                    strBuildName = "沙滩椅";
                    break;
                case "17732":
                    strBuildName = "沙滩椅";
                    break;
                case "17832":
                    strBuildName = "沙滩椅";
                    break;
                case "17932":
                    strBuildName = "沙滩椅";
                    break;
                case "18031":
                    strBuildName = "旋转木马";
                    break;
                case "18131":
                    strBuildName = "旋转木马";
                    break;
                case "18231":
                    strBuildName = "旋转木马";
                    break;
                case "18331":
                    strBuildName = "儿童乐园";
                    break;
                case "18431":
                    strBuildName = "儿童乐园";
                    break;
                case "18832":
                    strBuildName = "茶馆";
                    break;
                case "18932":
                    strBuildName = "茶馆";
                    break;
                case "19032":
                    strBuildName = "茶馆";
                    break;
                case "19132":
                    strBuildName = "茶馆";
                    break;
                case "19232":
                    strBuildName = "茶馆";
                    break;
                case "19332":
                    strBuildName = "理发店";
                    break;
                case "19432":
                    strBuildName = "理发店";
                    break;
                case "19532":
                    strBuildName = "理发店";
                    break;
                case "19632":
                    strBuildName = "理发店";
                    break;
                case "19732":
                    strBuildName = "理发店";
                    break;
                case "19832":
                    strBuildName = "爆米花店";
                    break;
                case "19932":
                    strBuildName = "爆米花店";
                    break;
                case "20032":
                    strBuildName = "爆米花店";
                    break;
                case "20132":
                    strBuildName = "钟楼";
                    break;
                case "20232":
                    strBuildName = "钟楼";
                    break;
                case "20332":
                    strBuildName = "钟楼";
                    break;
                case "20432":
                    strBuildName = "水果店";
                    break;
                case "20532":
                    strBuildName = "水果店";
                    break;
                case "20632":
                    strBuildName = "水果店";
                    break;
                case "20732":
                    strBuildName = "水果店";
                    break;
                case "20832":
                    strBuildName = "水果店";
                    break;
                case "20931":
                    strBuildName = "摩天轮";
                    break;
                case "21031":
                    strBuildName = "摩天轮";
                    break;
                case "21131":
                    strBuildName = "摩天轮";
                    break;
                case "21232":
                    strBuildName = "气球屋";
                    break;
                case "21332":
                    strBuildName = "气球屋";
                    break;
                case "21432":
                    strBuildName = "气球屋";
                    break;
                case "21532":
                    strBuildName = "气球屋";
                    break;
                case "21632":
                    strBuildName = "气球屋";
                    break;
                case "36032":
                    strBuildName = "钟塔";
                    break;
                case "36132":
                    strBuildName = "钟塔";
                    break;
                case "37631":
                    strBuildName = "瀑布";
                    break;
                case "37731":
                    strBuildName = "瀑布";
                    break;
                case "37831":
                    strBuildName = "瀑布";
                    break;
                case "37931":
                    strBuildName = "雪山";
                    break;
                case "38031":
                    strBuildName = "雪山";
                    break;
                case "38131":
                    strBuildName = "雪山";
                    break;
                case "38332":
                    strBuildName = "阿尔及利亚足球馆";
                    break;
                case "38432":
                    strBuildName = "阿根廷足球馆";
                    break;
                case "38532":
                    strBuildName = "澳大利亚足球馆";
                    break;
                case "38632":
                    strBuildName = "巴拉圭足球馆";
                    break;
                case "38732":
                    strBuildName = "巴西足球馆";
                    break;
                case "38832":
                    strBuildName = "朝鲜足球馆";
                    break;
                case "38932":
                    strBuildName = "丹麦足球馆";
                    break;
                case "39032":
                    strBuildName = "德国足球馆";
                    break;
                case "39132":
                    strBuildName = "法国足球馆";
                    break;
                case "39232":
                    strBuildName = "韩国足球馆";
                    break;
                case "39332":
                    strBuildName = "荷兰足球馆";
                    break;
                case "39432":
                    strBuildName = "洪都拉斯足球馆";
                    break;
                case "39532":
                    strBuildName = "加纳足球馆";
                    break;
                case "39632":
                    strBuildName = "喀麦隆足球馆";
                    break;
                case "39732":
                    strBuildName = "科特迪瓦足球馆";
                    break;
                case "39832":
                    strBuildName = "美国足球馆";
                    break;
                case "40032":
                    strBuildName = "南非足球馆";
                    break;
                case "40132":
                    strBuildName = "尼日利亚足球馆";
                    break;
                case "40232":
                    strBuildName = "葡萄牙足球馆";
                    break;
                case "40332":
                    strBuildName = "日本足球馆";
                    break;
                case "40432":
                    strBuildName = "瑞士足球馆";
                    break;
                case "40532":
                    strBuildName = "塞尔维亚足球馆";
                    break;
                case "40632":
                    strBuildName = "斯洛伐克足球馆";
                    break;
                case "40732":
                    strBuildName = "斯洛文尼亚足球馆";
                    break;
                case "40832":
                    strBuildName = "乌拉圭足球馆";
                    break;
                case "40932":
                    strBuildName = "西班牙足球馆";
                    break;
                case "41032":
                    strBuildName = "希腊足球馆";
                    break;
                case "41132":
                    strBuildName = "新西兰足球馆";
                    break;
                case "41232":
                    strBuildName = "意大利足球馆";
                    break;
                case "41332":
                    strBuildName = "英格兰足球馆";
                    break;
                case "41432":
                    strBuildName = "智利足球馆";
                    break;
                case "41831":
                    strBuildName = "马戏团";
                    break;
                case "41931":
                    strBuildName = "马戏团";
                    break;
                case "42031":
                    strBuildName = "马戏团";
                    break;
                case "42131":
                    strBuildName = "火山";
                    break;
                case "42231":
                    strBuildName = "火山";
                    break;
                case "42331":
                    strBuildName = "火山";
                    break;
                default:
                    strBuildName = "UFO";
                    break;
            }
            return strBuildName;
        }
        #endregion;

        private void LoadBuildInfo(int intOpID, string strUserID, string strNickName)
        {
            //加载用户建筑信息
            //方法:POST
            //参数:authid=fad254cddb744968ecbf79cab3bb483b&ownerUid=228618602
            //http://rrisland.hapyfish.com/api/initisland?time=1278580378815
            string strContent = null;

            ShowSysLog("正在加载建筑信息");

            if (strSwitch == "renren")
            {
                HTTPproc_1.RequestHeaders.Add("Cookie:PHPSESSID=" + strAuthID.Replace("_xnc_PHPSESSID=", ""));
                HTTPproc_1.RequestHeaders.Add("Referer:http://static.hapyfish.com/renren/swf/v17/islandLoader.swf");
                if (strUserID == "")
                {
                    strContent = HTTPproc_1.OpenRead("http://rrisland.hapyfish.com/api/initisland?time=" + Convert.ToInt64(ConvertDateTimeInt(dt)).ToString(), "authid=" + strSESSION + "&ownerUid=" + strUserid);
                }
                else
                {
                    strContent = HTTPproc_1.OpenRead("http://rrisland.hapyfish.com/api/initisland?time=" + Convert.ToInt64(ConvertDateTimeInt(dt)).ToString(), "authid=" + strSESSION + "&ownerUid=" + strUserID);
                }
            }
            else
            {
                HTTPproc_1.RequestHeaders.Add("Cookie:" + strPHPSESSID);
                HTTPproc_1.RequestHeaders.Add("Referer:http://tbstatic.hapyfish.com/swf/v06/islandLoader.swf");
                if (strUserID == "")
                {
                    strContent = HTTPproc_1.OpenRead("http://island.hapyfish.com/api/initisland?time=" + Convert.ToInt64(ConvertDateTimeInt(dt)).ToString(), "authid=" + strSESSION + "&ownerUid=" + strUserid);
                }
                else
                {
                    strContent = HTTPproc_1.OpenRead("http://island.hapyfish.com/api/initisland?time=" + Convert.ToInt64(ConvertDateTimeInt(dt)).ToString(), "authid=" + strSESSION + "&ownerUid=" + strUserID);
                }
            }

            JObject o = JObject.Parse(CleanBadWord(Unicode2Character(strContent)));
            JArray buildInfo = (JArray)o["islandVo"]["buildings"];

            if (intOpID == 0)
            {
                if (listView_Main_Form_BuildList.Items.Count > 0)
                {
                    foreach (ListViewItem li in listView_Main_Form_BuildList.Items)
                    {
                        if (listView_Main_Form_BuildList.Items.Count > 0)
                        {
                            li.Remove();
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int i = 0; i < buildInfo.Count(); i++)
                    {
                        if ((string)buildInfo[i]["event"] != null)
                        {
                            ListViewItem li = new ListViewItem();
                            li.SubItems.Clear();
                            li.SubItems[0].Text = (string)buildInfo[i]["id"];
                            li.SubItems.Add(BuildClass((string)buildInfo[i]["cid"]));
                            li.SubItems.Add((string)buildInfo[i]["deposit"] + "/" + (string)buildInfo[i]["startDeposit"]);
                            if (Convert.ToInt32((string)buildInfo[i]["deposit"]) == 0 && (int)buildInfo[i]["payRemainder"] == 0)
                            {
                                li.SubItems.Add("无游客光临");
                            }
                            else if (Convert.ToInt32((string)buildInfo[i]["deposit"]) > 0 && (int)buildInfo[i]["payRemainder"] == 0)
                            {
                                li.SubItems.Add("等待结算中");
                            }
                            else
                            {
                                li.SubItems.Add(TimeCal((int)buildInfo[i]["payRemainder"]));
                            }
                            li.SubItems.Add((string)buildInfo[i]["event"]);
                            listView_Main_Form_BuildList.Items.Add(li);
                        }
                    }
                    ShowSysLog("建筑信息加载完成");
                }
                else
                {
                    for (int i = 0; i < buildInfo.Count(); i++)
                    {
                        if ((string)buildInfo[i]["event"] != null)
                        {
                            ListViewItem li = new ListViewItem();
                            li.SubItems.Clear();
                            li.SubItems[0].Text = (string)buildInfo[i]["id"];
                            li.SubItems.Add(BuildClass((string)buildInfo[i]["cid"]));
                            li.SubItems.Add((string)buildInfo[i]["deposit"] + "/" + (string)buildInfo[i]["startDeposit"]);
                            if (Convert.ToInt32((string)buildInfo[i]["deposit"]) == 0 && (int)buildInfo[i]["payRemainder"] == 0)
                            {
                                li.SubItems.Add("无游客光临");
                            }
                            else if (Convert.ToInt32((string)buildInfo[i]["deposit"]) > 0 && (int)buildInfo[i]["payRemainder"] == 0)
                            {
                                li.SubItems.Add("等待结算中");
                            }
                            else
                            {
                                li.SubItems.Add(TimeCal((int)buildInfo[i]["payRemainder"]));
                            }
                            li.SubItems.Add((string)buildInfo[i]["event"]);
                            listView_Main_Form_BuildList.Items.Add(li);
                        }
                    }
                    ShowSysLog("建筑信息加载完成");
                    //listView_Main_Form_BoatList.Items[listView_Main_Form_BoatList.Items.Count - 1].Remove();
                }
            }

            if ((bool)o["islandVo"]["isFriend"] == false)
            {
                intOpID = 1;
            }
            else
            {
                intOpID = 2;
            }

            if (intOpID == 1)//收获自己金币
            {
                //收取金币
                //方法:POST
                //参数:authid=e7f7a684538d63ff0c9b099a7514083a&itemId=1655997232
                //http://rrisland.hapyfish.com/api/harvestplant?time=1278733498183

                for (int i = 0; i < buildInfo.Count(); i++)
                {
                    if ((string)buildInfo[i]["event"] != null)
                    {
                        if ((string)buildInfo[i]["deposit"] != "0" && (int)buildInfo[i]["payRemainder"] == 0)
                        {
                            if (strSwitch == "renren")
                            {
                                HTTPproc_1.RequestHeaders.Add("Cookie:PHPSESSID=" + strAuthID.Replace("_xnc_PHPSESSID=", ""));
                                HTTPproc_1.RequestHeaders.Add("Referer:http://static.hapyfish.com/renren/swf/v17/islandLoader.swf");
                                HTTPproc_1.OpenRead("http://rrisland.hapyfish.com/api/harvestplant?time=" + Convert.ToInt64(ConvertDateTimeInt(dt)).ToString(), "authid=" + strSESSION + "&itemId=" + (string)buildInfo[i]["id"]);
                            }
                            else
                            {
                                HTTPproc_1.RequestHeaders.Add("Cookie:" + strPHPSESSID);
                                HTTPproc_1.RequestHeaders.Add("Referer:http://tbstatic.hapyfish.com/swf/v06/islandLoader.swf");
                                HTTPproc_1.OpenRead("http://island.hapyfish.com/api/harvestplant?time=" + Convert.ToInt64(ConvertDateTimeInt(dt)).ToString(), "authid=" + strSESSION + "&itemId=" + (string)buildInfo[i]["id"]);
                            }
                            ShowSysLog("收取自己 " + BuildClass((string)buildInfo[i]["cid"]) + " " + (string)buildInfo[i]["deposit"] + " 金币");
                        }
                    }
                }
            }
            if (intOpID == 2)//收获好友金币
            {
                //收取好友金币
                //方法:POST
                //参数:fid=272438384&authid=d7c00cf02c1b878d1dd831a8eeea5eb0&itemId=664380432
                //http://rrisland.hapyfish.com/api/moochplant
                double dubA = 0;
                double dubB = 0;
                int intCount = 0;
                for (int i = 0; i < buildInfo.Count(); i++)
                {
                    if ((string)buildInfo[i]["event"] != null)
                    {
                        dubA = Convert.ToDouble((string)buildInfo[i]["deposit"]);
                        dubB = Convert.ToDouble((string)buildInfo[i]["startDeposit"]);
                        if (dubA == 0 && dubB == 0)
                        {
                            intCount = 1;
                        }
                        else
                        {
                            intCount = Convert.ToInt32((dubA / dubB) * 10);
                        }
                        if ((int)buildInfo[i]["payRemainder"] == 0 && (int)buildInfo[i]["hasSteal"] == 0 && intCount > 7)
                        {
                            if (strSwitch == "renren")
                            {
                                HTTPproc_1.RequestHeaders.Add("Cookie:PHPSESSID=" + strAuthID.Replace("_xnc_PHPSESSID=", ""));
                                HTTPproc_1.RequestHeaders.Add("Referer:http://static.hapyfish.com/renren/swf/v17/islandLoader.swf");
                                strContent = HTTPproc_1.OpenRead("http://rrisland.hapyfish.com/api/moochplant", "fid=" + strUserID + "&authid=" + strSESSION + "&itemId=" + (string)buildInfo[i]["id"]);
                            }
                            else
                            {
                                HTTPproc_1.RequestHeaders.Add("Cookie:" + strPHPSESSID);
                                HTTPproc_1.RequestHeaders.Add("Referer:http://tbstatic.hapyfish.com/swf/v06/islandLoader.swf");
                                strContent = HTTPproc_1.OpenRead("http://island.hapyfish.com/api/moochplant", "fid=" + strUserID + "&authid=" + strSESSION + "&itemId=" + (string)buildInfo[i]["id"]);
                            }

                            o = JObject.Parse(Unicode2Character(strContent));
                            if ((int)o["status"] > 0)
                            {
                                ShowSysLog("收取好友 " + strNickName + " " + BuildClass((string)buildInfo[i]["cid"]) + " +" + (int)o["coinChange"] + " 金币 增加经验 +" + (int)o["expChange"]);
                            }
                        }
                    }
                }
            }
        }

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
        #endregion

        #region 淘宝登录流程 TB_Login_Handle()
        /// <summary>
        /// 淘宝登录流程
        /// </summary>
        public void TB_Login_Handle()
        {
            string strContent = null;       //
            string resultString = null;
            string strRedirectURL = null;   //存取跳转
            string strRequest = null;       //存取Get请求
            string strParameter = null;     //存取Post参数
            string strCookie = null;        //存取Cookie

            strSwitch = "taobao";
            RandStr rndKey = new RandStr();
            RandStr rndKey_1 = new RandStr(true, false, false, false);

            if (strUserName.Trim() == "2" && strPassword.Trim() == "2")
            {
                strUserName = "destiny20045";
                strPassword = "cupid0426";
            }

            if (strUserName.Trim() != "" && strPassword.Trim() != "")
            {
                //设置HTTP请求默认编码
                //HTTPproc.Encoding = System.Text.Encoding.UTF8;
                HTTPproc.Encoding = System.Text.Encoding.Default;

                #region 请求第一步

                //==============  Cookie 参数  =================
                //cookie2          522b0f7e4609ff4fde596087a9bce9f3
                //_tb_token_       477837e79383
                //t                fe8ad20f41c5f335d453be685896227e
                //uc1              cookie14=UoM8cfXc1sA+RA==
                //v                0
                //_lang            zh_CN:GBK
                //==============================================

                //strRequest = "http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fjianghu.taobao.com%2Flogin.htm";
                strRequest = "http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F";
                HTTPproc.OpenRead(strRequest);

                resultString = Regex.Match(HTTPproc.ResponseHeaders.ToString(), "Set-Cookie.*").Value.Replace("Set-Cookie: ", "").Replace(";", "\r\n").Replace(",", "\r\n").Replace("Path=/", "");
                strCookie += Regex.Match(resultString, "cookie2=.*").Value + ";";
                strCookie += Regex.Match(resultString, "_tb_token_=.*").Value + ";";
                strCookie += Regex.Match(resultString, "t=.*").Value + ";";
                strCookie += Regex.Match(resultString, "uc1=.*").Value + ";";
                strCookie += Regex.Match(resultString, "v=.*").Value + ";";
                strCookie += Regex.Match(resultString, "_lang=.*").Value + ";";

                #endregion 

                #region 请求第二步

                //==============  Cookie 参数  =================
                //cookie2          522b0f7e4609ff4fde596087a9bce9f3
                //_tb_token_       477837e79383
                //t                fe8ad20f41c5f335d453be685896227e
                //v                0
                //_lang            zh_CN:GBK
                //uc1              lltime=1279085212&cookie14=UoM8cfXdNnANow==&existShop=false&cookie16=UtASsssmPlP/f1IHDsDaPRu+Pw==&sg=鎵?5&_yb_=false&cookie21=URm48syIZQ==&cookie15=W5iHLLyFOGW7aA==&_msg_v=true&_rt_=1251959051&_msg_=0
                //ck1                                                                                                                                                                                                                      
                //_sv_             0                                                                                                                                                                                                       
                //tg               0                                                                                                                                                                                                       
                //_cc_             UtASsssmfA==                                                                                                                                                                                            
                //_nk_             \u98CE\u4E2D\u8131\u624B                                                                                                                                                                                
                //nt               URm48syINoWHMQeDoD7OxbXupdeyfjknJs/LDJQGgRZW                                                                                                                                                            
                //_l_g_            Ug==                                                                                                                                                                                                    
                //_wwmsg_          0,0                                                                                                                                                                                                     
                //tracknick        \u98CE\u4E2D\u8131\u624B                                                                                                                                                                                
                //ssllogin                                                                                                                                                                                                                 
                //lastgetwwmsg     MTI3OTA5MTQyOA==                                                                                                                                                                                        
                //cookie1          AH4IA6mJonJDfo4k9kFzpWPwnqr692blgcw5+IURwC0=                                                                                                                                                            
                //cookie17         UUGjNjAvZoWY                                                                                                                                                                                            
                //==============================================

                string strCookieTemp = strCookie.Replace("\r", "");
                HTTPproc.RequestHeaders.Add("Cookie:" + strCookie.Replace("\r",""));
                //HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fjianghu.taobao.com%2Flogin.htm");
                HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F");
                strCookie = strCookie.Replace(";", "\r\n");
                string strToken = Regex.Match(strCookie, "_tb_token_=.*").Value.Replace("_tb_token_=", "");

                strParameter = "TPL_username=" + UrlEncode(strUserName) + "&TPL_password=" + strPassword + "&_tb_token_=" + strToken + "&actionForStable=enable_post_user_action&action=Authenticator&event_submit_do_login=anything&mi_uid=&mcheck=&TPL_redirect_url=http%3A%2F%2Fjianghu.taobao.com%2Flogin.htm&from=tbTop&fc=2&style=default&yparam=&done=&loginType=3&tid=&support=000001&CtrlVersion=1%2C0%2C0%2C7&pstrong=2&longLogin=-1&llnick=";
                HTTPproc.OpenRead("http://login.taobao.com/member/login.jhtml", strParameter);
                string strCookie_1 = HTTPproc.RespHtml;
                resultString = Regex.Match(HTTPproc.ResponseHeaders.ToString(), "Set-Cookie.*").Value.Replace("Set-Cookie: ", "").Replace(";", "\r\n").Replace(",", "\r\n").Replace("Path=/", "");

                strCookie = strCookieTemp.Replace(";", "\r\n");
                strCookie = Regex.Replace(strCookie, "uc1=.*", "");
                resultString = strCookie + resultString;

                strCookie = Regex.Match(resultString, "cookie2=.*").Value + ";";
                strCookie += Regex.Match(resultString, "_tb_token_=.*").Value + ";";
                strCookie += Regex.Match(resultString, "t=.*").Value + ";";
                strCookie += Regex.Match(resultString, "v=.*").Value + ";";
                strCookie += Regex.Match(resultString, "_lang=.*").Value + ";";
                strCookie += Regex.Match(resultString, "uc1=.*").Value + ";";
                strCookie += Regex.Match(resultString, "ck1=.*").Value + ";";
                strCookie += Regex.Match(resultString, "_sv_=.*").Value + ";";
                strCookie += Regex.Match(resultString, "tg=.*").Value + ";";
                strCookie += Regex.Match(resultString, "_cc_=.*").Value + ";";
                strCookie += Regex.Match(resultString, "_nk_=.*").Value + ";";
                strCookie += Regex.Match(resultString, "nt=.*").Value + ";";
                strCookie += Regex.Match(resultString, "_l_g_=.*").Value + ";";
                strCookie += Regex.Match(resultString, "_wwmsg_=.*").Value + ";";
                strCookie += Regex.Match(resultString, "tracknick=.*").Value + ";";
                strCookie += Regex.Match(resultString, "ssllogin=.*").Value + ";";
                strCookie += Regex.Match(strCookie_1, "lastgetwwmsg=.*; D").Value.Replace("; D", ";");
                strCookie += Regex.Match(strCookie_1, "cookie1=.*;D").Value.Replace(";D",";");
                strCookie += Regex.Match(strCookie_1, "cookie17=.*;D").Value.Replace(";D", ";");
                strCookie = strCookie.Replace("\r\n", ";").Replace("\r", "").Replace("\n","");

                #endregion

                #region 请求第三步

                strCookieTemp = strCookie;
                HTTPproc.RequestHeaders.Add("Cookie:" + strCookie);
                //HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fjianghu.taobao.com%2Flogin.htm");
                HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F");
                strRequest = "http://jianghu.taobao.com/login.htm";
                HTTPproc.OpenRead(strRequest);
                resultString = Regex.Match(HTTPproc.ResponseHeaders.ToString(), "Set-Cookie.*").Value.Replace("Set-Cookie: ", "").Replace(";", "\r\n").Replace(",", "\r\n").Replace("Path=/", "");

                strCookie = strCookieTemp.Replace(";", "\r\n");
                strCookie = Regex.Replace(strCookie, "uc1=.*", "");
                resultString = strCookie + "\r\n" + resultString;
                strCookie = resultString.Replace("\r\n", ";").Replace("Domain=.taobao.com;","");

                #endregion

                #region 请求第四步

                strCookieTemp = strCookie;
                HTTPproc.RequestHeaders.Add("Cookie:" + strCookie.Replace("\r","").Replace("\n",""));
                //HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fjianghu.taobao.com%2Flogin.htm");
                HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F");
                //strRequest = "http://jianghu.taobao.com/home.htm";
                //strRequest = "http://bo.tianxia.taobao.com/taoboyuan/seedstore.htm";
                
                strRequest = "http://bo.tianxia.taobao.com/taoboyuan/dayscore.htm";
                resultString = HTTPproc.OpenRead(strRequest);
                MessageBox.Show(resultString);

                resultString = Regex.Match(HTTPproc.ResponseHeaders.ToString(), "Set-Cookie.*").Value.Replace("Set-Cookie: ", "").Replace("; Domain=.taobao.com; Path=/\r", "");

                strCookie = strCookieTemp.Replace(";", "\r\n");
                strCookie = Regex.Replace(strCookie, "uc1=.*", "");
                resultString = strCookie + resultString;
                strCookie = resultString.Replace("\r\n", ";").Replace("\r", "").Replace("\n", "").Replace(";;  ", ";");

                #endregion

                #region 请求第五步

                strCookieTemp = strCookie;
                HTTPproc.RequestHeaders.Add("Cookie:" + strCookie.Replace("\r", "").Replace("\n", ""));
                HTTPproc.RequestHeaders.Add("Referer:http://jianghu.taobao.com/home.htm");

                strRequest = "http://jianghu.taobao.com/admin/plugin.htm?appkey=12029234&tracelog=jhleftmenu";
                HTTPproc.OpenRead(strRequest);

                resultString = Regex.Match(HTTPproc.ResponseHeaders.ToString(), "Set-Cookie.*").Value.Replace("Set-Cookie: ", "").Replace("; Domain=.taobao.com; Path=/\r", "");
                strCookie = strCookieTemp.Replace(";", "\r\n");
                strCookie = Regex.Replace(strCookie, "uc1=.*", "");
                resultString = strCookie + resultString;
                strCookie = resultString.Replace("\r\n", ";").Replace("\r", "").Replace("\n", "");

                #endregion

                #region 请求第六步

                strCookieTemp = strCookie;
                HTTPproc.RequestHeaders.Add("Cookie:" + strCookie.Replace("\r", "").Replace("\n", ""));
                HTTPproc.RequestHeaders.Add("Referer:http://jianghu.taobao.com/admin/plugin.htm?appkey=12029234&tracelog=jhleftmenu");
                strRequest = "http://container.api.taobao.com/container?appkey=12029234&tracelog=jhleftmenu";
                HTTPproc.OpenRead(strRequest);
                strRedirectURL = Regex.Match(HTTPproc.ResponseHeaders.ToString(), "Location.*").Value.Replace("Location: ","");

                //resultString = Regex.Match(HTTPproc.ResponseHeaders.ToString(), "Set-Cookie.*").Value.Replace("Set-Cookie: ", "").Replace("; Domain=.taobao.com; Path=/\r", "");
                //strCookie = strCookieTemp.Replace(";", "\r\n");
                //strCookie = Regex.Replace(strCookie, "uc1=.*", "");
                //resultString = strCookie + resultString;

                #endregion

                #region 请求第七步

                strCookieTemp = strCookie;
                HTTPproc.RequestHeaders.Add("Cookie:" + strCookie.Replace("\r", "").Replace("\n", ""));
                HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fjianghu.taobao.com%2Flogin.htm");

                resultString = Regex.Match(HTTPproc.OpenRead(strRedirectURL), "scode.*\"l").Value;
                strContent = resultString;
                strContent = Regex.Replace(strContent, @"scode"":""\d{0,}"",""uid"":""", "");
                strUserid = strContent.Replace("\",\"l", ""); //scode":"1279033297","uid":"535131","l
                strSESSION = UserMd5(resultString.Replace("scode\":\"", "").Replace("\",\"uid\":\"", "").Replace("\",\"l", ""));
                strPHPSESSID = Regex.Match(HTTPproc.Cookie.ToString(), @"PHPSESSID=\w{0,32}").Value;

                #endregion

                #region 进入游戏判断流程

                HTTPproc_1.RequestHeaders.Add("Cookie:" + strPHPSESSID);
                HTTPproc_1.RequestHeaders.Add("Referer:http://tbstatic.hapyfish.com/swf/v06/islandLoader.swf");
                HTTPproc_1.OpenRead("http://island.hapyfish.com/api/inituser", "authid=" + strSESSION);
                LoadUserInfo();
                LoadFriends();
                LoadBoatInfo(1, "", "");
                LoadBoatInfo(0, "", "");
                LoadBuildInfo(1, "", "");
                LoadBuildInfo(0, "", "");

                #endregion
            }
            else
            {
                MessageBox.Show("信息填写有误请检查！", "系统消息");
            }
        }
        #endregion 淘宝登录流程 Login_Handle()

        #region Url编码 UrlEncode(string url)
        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string UrlEncode(string url)
        { 
            byte[] bs = Encoding.GetEncoding("GB2312").GetBytes(url); 
            StringBuilder sb = new StringBuilder(); 
            for (int i = 0; i < bs.Length; i++) 
            {
                if (bs[i] < 128)
                {
                    sb.Append((char)bs[i]);
                }
                else 
                { 
                    sb.Append("%" + bs[i++].ToString("x").PadLeft(2, '0')); 
                    sb.Append("%" + bs[i].ToString("x").PadLeft(2, '0')); 
                }
            } return sb.ToString();
        }
        #endregion

        private string ConvertToUnicode(string strGB) 
        { 
            char[] chs = strGB.ToCharArray(); 
            string result = string.Empty; 
            foreach(char c in chs) 
            { 
                result += @"\u" + char.ConvertToUtf32(c.ToString(), 0).ToString("x").ToUpper(); 
            } 
            return result; 
        } 

    }
}