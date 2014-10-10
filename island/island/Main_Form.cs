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
            //�ؼ�״̬����
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

        #region ��¼У������ Login_Handle()
        /// <summary>
        /// ��¼У������
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
                //����HTTP����Ĭ�ϱ���
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
                    MessageBox.Show("�û������������", "ϵͳ��Ϣ");
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("��Ϣ��д�������飡", "ϵͳ��Ϣ");
            }
        }
        #endregion ��¼У������ Login_Handle()

        #region ��¼��Ϸ���� Login_Game()
        /// <summary>
        /// ��¼��Ϸ����
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

        #region ��ȡ�û���Ϣ ReadUserInfo(string strContent)
        /// <summary>
        /// ��ȡ�û���Ϣ
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
            //    label_Main_Form_NickName.Text += resultString.Replace("<title>������ У�� - ", "").Replace("</title>", "");
            //}
            //catch (ArgumentException ex)
            //{
            //    // Syntax error in the regular expression
            //}
        }
        #endregion

        #region URL���� UrlEncode(string str, string encode)
        /// <summary>
        /// URL����
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
            //����Ҫ������ַ�

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.";
            System.Text.Encoder encoder = System.Text.Encoding.GetEncoding(encode).GetEncoder();
            char[] c1 = str.ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //һ���ַ�һ���ַ��ı���

            for (int i = 0; i < c1.Length; i++)
            {
                //����Ҫ����

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

        #region ʱ��� ConvertDateTimeInt(DateTime time)
        /// <summary>
        /// ��c# DateTimeʱ���ʽת��ΪUnixʱ�����ʽ
        /// </summary>
        /// <param name="time">ʱ��</param>
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

        #region MD5�����㷨 UserMd5(string str)
        /// <summary>
        /// MD5�����㷨
        /// </summary>
        /// <param name="str">�����ַ���</param>
        /// <returns>���32λСдMD5����ֵ</returns>
        static string UserMd5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//ʵ����һ��md5����  
            // ���ܺ���һ���ֽ����͵����飬����Ҫע�����UTF8/Unicode�ȵ�ѡ��  
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // ͨ��ʹ��ѭ�������ֽ����͵�����ת��Ϊ�ַ��������ַ����ǳ����ַ���ʽ������  
            for (int i = 0; i < s.Length; i++)
            {
                // ���õ����ַ���ʹ��ʮ���������͸�ʽ����ʽ����ַ���Сд����ĸ�����ʹ�ô�д��X�����ʽ����ַ��Ǵ�д�ַ�
                pwd = pwd + s[i].ToString("x2");
            }
            return pwd;
        }
        #endregion

        #region ��Unicodeת��ΪCharacter Unicode2Character(string str)
        /// <summary>
        /// ��Unicodeת��ΪCharacter
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
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

        #region ��ȡ�������û���Ϣ LoadUserInfo()
        /// <summary>
        /// ��ȡ�������û���Ϣ
        /// </summary>
        private void LoadUserInfo()
        {
            //�û���Ϣ��ȡURL
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
            label_Main_Form_UserID.Text = "У��ID:" + (string)o["user"]["uid"];
            label_Main_Form_NickName.Text = "�ǳ�:" + (string)o["user"]["name"];
            label_Main_Form_Exp.Text = (string)o["user"]["exp"] + "/" + (string)o["user"]["maxExp"];
            label_Main_Form_Level.Text = (string)o["user"]["level"];
            label_Main_Form_Coin.Text = (string)o["user"]["coin"];
            label_Main_Form_Praise.Text = (string)o["user"]["praise"];
        }
        #endregion

        #region ��ʾϵͳ��־ ShowSysLog(string strLog)
        /// <summary>
        /// ��ʾϵͳ��־
        /// </summary>
        /// <param name="strLog"></param>        
        private void ShowSysLog(string strLog)
        {
            DateTime dt_1 = DateTime.Now;
            Main_Form_textBox1.Text += strLog + "    " + dt_1 + "\r\n";
            Main_Form_textBox1.SelectionStart = Main_Form_textBox1.Text.Length;
            Main_Form_textBox1.ScrollToCaret();
            //ʼ����ʾTextBox����һ�У�ʼ�չ�������ײ�
            Main_Form_textBox1.SelectionStart = Main_Form_textBox1.Text.Length;
            Main_Form_textBox1.ScrollToCaret();
        }
        #endregion

        #region ��ȡ�����б���ʾ LoadFriends()
        /// <summary>
        /// ��ȡ�����б���ʾ
        /// </summary>
        private void LoadFriends()
        {
            //�û����ѵ�ȡ��Ϣ
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
                if ((string)friends[i]["name"] != "����")
                {
                    ListViewItem li = new ListViewItem();
                    li.SubItems.Clear();
                    li.SubItems[0].Text = (string)friends[i]["uid"];
                    li.SubItems.Add((string)friends[i]["name"]);
                    li.SubItems.Add((string)friends[i]["level"]);
                    listView_Main_Form_FriendList.Items.Add(li);
                }
            }

            ShowSysLog("�����б�������");
            //listView_Main_Form_FriendList.Items[listView_Main_Form_FriendList.Items.Count - 1].Remove();
        }
        #endregion

        #region �����û��ۿ���Ϣ���������� LoadBoatInfo(int intOpID, string strUserID, string strNickName)
        /// <summary>
        /// �����û��ۿ���Ϣ����������
        /// </summary>
        /// <param name="intOpID">0,��ȡ�û��ۿ���Ϣ 1,�����ۿڴ�ֻ 2,͵ȡ���Ѵ�ֻ</param>
        /// <param name="strUserID"></param>
        /// <param name="strNickName"></param>
        private void LoadBoatInfo(int intOpID, string strUserID, string strNickName)
        {
            //�����û��ۿ���Ϣ
            //����:POST
            //����:authid=fad254cddb744968ecbf79cab3bb483b&ownerUid=228618602
            //http://rrisland.hapyfish.com/api/initisland?time=1278580378815

            string strBoatState = null;
            string strContent = null;
            ShowSysLog("���ڼ��ظۿ���Ϣ");

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
                    ShowSysLog("��ֻ��Ϣ�������");
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
                    ShowSysLog("��ֻ��Ϣ�������");
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
                    //��������
                    //����:POST
                    //����:authid=fad254cddb744968ecbf79cab3bb483b&positionId=2
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
                        ShowSysLog((string)boatInfo[i]["id"] + "�Ŵ�����");
                    }
                }
            }
            if (intOpID == 2)
            {
                ShowSysLog("���ѣ�" + strNickName + "���ۿ���Ϣ�������");
                for (int i = 0; i < boatInfo.Count(); i++)
                {
                    //͵ȡ�ۿڳ˿͵���
                    //����:POST
                    //����:positionId=1&authid=b375b7c1ef01d91036383cf11ad5f1ba&ownerUid=272438384
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
                            ShowSysLog("͵ȡ���ѣ�" + strNickName + "�� " + (string)boatInfo[i]["id"] + "�Ŵ��ɹ�");
                        }
                        else
                        {
                            ShowSysLog("͵ȡ���ѣ�" + strNickName + "�� " + (string)boatInfo[i]["id"] + "�Ŵ�ʧ��");
                        }
                    }
                    else
                    {
                        ShowSysLog("���ѣ�" + strNickName + "�� " + (string)boatInfo[i]["id"] + "�Ŵ����ڸۿ�");
                    }
                }
            }
        }
        #endregion

        #region ��ȡ��ֻ���� BoatClass(string strID)
        /// <summary>
        /// ��ȡ��ֻ����
        /// </summary>
        /// <param name="intID"></param>
        /// <returns></returns>
        private string BoatClass(string strID)
        {
            string strBoatName = null;

            switch (strID)
            {
                case "1":
                    strBoatName = "Сľ��";
                    break;
                case "2":
                    strBoatName = "ľ��";
                    break;
                case "3":
                    strBoatName = "��Ƥͧ";
                    break;
                case "4":
                    strBoatName = "��ľ��";
                    break;
                case "5":
                    strBoatName = "�󷫴�";
                    break;
                case "6":
                    strBoatName = "��ɫ��ͧ";
                    break;
                case "7":
                    strBoatName = "��ɫ��ͧ";
                    break;
                case "8":
                    strBoatName = "��������";
                    break;
                default:
                    strBoatName = "UFO";
                    break;
            }
            return strBoatName;
        }
        #endregion

        #region ��ȡ�ڸ۴�ֻ״̬ BoatState(string strState)
        /// <summary>
        /// ��ȡ�ڸ۴�ֻ״̬
        /// </summary>
        /// <param name="strState"></param>
        /// <returns></returns>
        private string BoatState(string strState)
        {
            switch (strState)
            {
                case "arrive_1":
                    strState = "�մ�";
                    break;
                case "arrive_2":
                    strState = "�ڸ�";
                    break;
                case "onTheRoad":
                    strState = "���";
                    break;
                default:
                    strState = "������";
                    break;
            }
            return strState;
        }
        #endregion

        #region ʱ�任�� TimeCal(int intTime)
        /// <summary>
        /// ʱ�任��
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
                return strTime = "����";
            }
        }
        #endregion

        #region ˫�������б�͵ȡ���Ѹۿڴ�ֻ
        /// <summary>
        /// ˫�������б�͵ȡ���Ѹۿڴ�ֻ
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

        #region ����ϵͳ������ CleanBadWord(string strContent)
        /// <summary>
        /// ����ϵͳ������
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

        #region ��ȡ�������� BuildClass(string strID)
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        private string BuildClass(string strID)
        {
            string strBuildName = null;
            switch (strID)
            {
                case "6221":
                    strBuildName = "����Ͱ";
                    break;
                case "6321":
                    strBuildName = "����Ͱ";
                    break;
                case "6421":
                    strBuildName = "����";
                    break;
                case "6521":
                    strBuildName = "����";
                    break;
                case "6621":
                    strBuildName = "����";
                    break;
                case "6721":
                    strBuildName = "ѩ��";
                    break;
                case "6821":
                    strBuildName = "����";
                    break;
                case "6921":
                    strBuildName = "������";
                    break;
                case "7021":
                    strBuildName = "��ͩ��";
                    break;
                case "7121":
                    strBuildName = "��Ҷ��";
                    break;
                case "7221":
                    strBuildName = "������";
                    break;
                case "7321":
                    strBuildName = "������";
                    break;
                case "7421":
                    strBuildName = "����";
                    break;
                case "7521":
                    strBuildName = "Ģ��ͷ��";
                    break;
                case "7621":
                    strBuildName = "����";
                    break;
                case "7721":
                    strBuildName = "����";
                    break;
                case "7821":
                    strBuildName = "��ƺ";
                    break;
                case "7921":
                    strBuildName = "ɳ��2";
                    break;
                case "8021":
                    strBuildName = "ɳ��1";
                    break;
                case "8121":
                    strBuildName = "����";
                    break;
                case "8221":
                    strBuildName = "����";
                    break;
                case "8321":
                    strBuildName = "��8ײ��";
                    break;
                case "8421":
                    strBuildName = "9����";
                    break;
                case "8521":
                    strBuildName = "������";
                    break;
                case "8621":
                    strBuildName = "ʮ�ּ�";
                    break;
                case "8721":
                    strBuildName = "�ۺ�С��";
                    break;
                case "8921":
                    strBuildName = "դ��ת��";
                    break;
                case "9021":
                    strBuildName = "դ��ת��";
                    break;
                case "9121":
                    strBuildName = "դ��ת��";
                    break;
                case "9221":
                    strBuildName = "դ��ת��";
                    break;
                case "9321":
                    strBuildName = "դ��ת��";
                    break;
                case "9421":
                    strBuildName = "դ��ת��";
                    break;
                case "9521":
                    strBuildName = "����դ��ת��";
                    break;
                case "9621":
                    strBuildName = "����դ��ת��";
                    break;
                case "9721":
                    strBuildName = "����դ��ת��";
                    break;
                case "9821":
                    strBuildName = "����դ��ת��";
                    break;
                case "9921":
                    strBuildName = "����դ��ת��";
                    break;
                case "10021":
                    strBuildName = "����դ��ת��";
                    break;
                case "10121":
                    strBuildName = "����դ��ת��";
                    break;
                case "10221":
                    strBuildName = "����դ��ת��";
                    break;
                case "10321":
                    strBuildName = "����դ��ת��";
                    break;
                case "10421":
                    strBuildName = "����դ��ת��";
                    break;
                case "10521":
                    strBuildName = "����դ��ת��";
                    break;
                case "10621":
                    strBuildName = "����դ��ת��";
                    break;
                case "10721":
                    strBuildName = "����դ��ת��";
                    break;
                case "10821":
                    strBuildName = "����դ��ת��";
                    break;
                case "10921":
                    strBuildName = "����դ��ת��";
                    break;
                case "11021":
                    strBuildName = "����դ��ת��";
                    break;
                case "11121":
                    strBuildName = "�ܲ�դ��ת��";
                    break;
                case "11221":
                    strBuildName = "�ܲ�դ��ת��";
                    break;
                case "11321":
                    strBuildName = "�ܲ�դ��ת��";
                    break;
                case "11421":
                    strBuildName = "�ܲ�դ��ת��";
                    break;
                case "11521":
                    strBuildName = "�ܲ�դ��ת��";
                    break;
                case "11621":
                    strBuildName = "�ܲ�դ��ת��";
                    break;
                case "11721":
                    strBuildName = "�ܲ�դ��ת��";
                    break;
                case "11821":
                    strBuildName = "�ܲ�դ��ת��";
                    break;
                case "12021":
                    strBuildName = "�Ʊ�դ��ת��";
                    break;
                case "12121":
                    strBuildName = "�Ʊ�դ��ת��";
                    break;
                case "12221":
                    strBuildName = "�Ʊ�դ��ת��";
                    break;
                case "12321":
                    strBuildName = "�Ʊ�դ��ת��";
                    break;
                case "12421":
                    strBuildName = "�Ʊ�դ��ת��";
                    break;
                case "12521":
                    strBuildName = "�Ʊ�դ��ת��";
                    break;
                case "12621":
                    strBuildName = "�Ʊ�դ��ת��";
                    break;
                case "12721":
                    strBuildName = "�Ʊ�դ��ת��";
                    break;
                case "12821":
                    strBuildName = "�̱�դ��ת��";
                    break;
                case "12921":
                    strBuildName = "�̱�դ��ת��";
                    break;
                case "13021":
                    strBuildName = "�̱�դ��ת��";
                    break;
                case "13121":
                    strBuildName = "�̱�դ��ת��";
                    break;
                case "13221":
                    strBuildName = "�̱�դ��ת��";
                    break;
                case "13321":
                    strBuildName = "�̱�դ��ת��";
                    break;
                case "13421":
                    strBuildName = "�̱�դ��ת��";
                    break;
                case "13521":
                    strBuildName = "�̱�դ��ת��";
                    break;
                case "13621":
                    strBuildName = "���դ��ת��";
                    break;
                case "13721":
                    strBuildName = "���դ��ת��";
                    break;
                case "13821":
                    strBuildName = "���դ��ת��";
                    break;
                case "13921":
                    strBuildName = "���դ��ת��";
                    break;
                case "14021":
                    strBuildName = "���դ��ת��";
                    break;
                case "14121":
                    strBuildName = "���դ��ת��";
                    break;
                case "14221":
                    strBuildName = "���դ��ת��";
                    break;
                case "14321":
                    strBuildName = "���դ��ת��";
                    break;
                case "21721":
                    strBuildName = "·��";
                    break;
                case "21821":
                    strBuildName = "·��";
                    break;
                case "21921":
                    strBuildName = "·��";
                    break;
                case "22121":
                    strBuildName = "����";
                    break;
                case "32921":
                    strBuildName = "��������";
                    break;
                case "33021":
                    strBuildName = "��������";
                    break;
                case "33121":
                    strBuildName = "�����ľ";
                    break;
                case "33221":
                    strBuildName = "��õ�����ľ";
                    break;
                case "33321":
                    strBuildName = "��õ�����ľ";
                    break;
                case "33421":
                    strBuildName = "��õ�廨̳";
                    break;
                case "33521":
                    strBuildName = "����";
                    break;
                case "33621":
                    strBuildName = "��ɫ����";
                    break;
                case "33721":
                    strBuildName = "��ɫ����";
                    break;
                case "33821":
                    strBuildName = "��ɫ����";
                    break;
                case "33921":
                    strBuildName = "�ص�";
                    break;
                case "34021":
                    strBuildName = "ľ׮";
                    break;
                case "34121":
                    strBuildName = "��ʹ����";
                    break;
                case "34221":
                    strBuildName = "��õ�廨��";
                    break;
                case "34321":
                    strBuildName = "�ٺϻ���";
                    break;
                case "34421":
                    strBuildName = "СϪ��ˮ��";
                    break;
                case "34521":
                    strBuildName = "С����Ȫ��";
                    break;
                case "34621":
                    strBuildName = "�ɻ�ˮ̶";
                    break;
                case "35821":
                    strBuildName = "��ɫ����Ͱ";
                    break;
                case "35921":
                    strBuildName = "�ƽ�����Ͱ";
                    break;
                case "36221":
                    strBuildName = "����·��";
                    break;
                case "36321":
                    strBuildName = "����·��";
                    break;
                case "36421":
                    strBuildName = "������";
                    break;
                case "36521":
                    strBuildName = "��ɫ������";
                    break;
                case "36621":
                    strBuildName = "�ö�����";
                    break;
                case "36721":
                    strBuildName = "������";
                    break;
                case "36821":
                    strBuildName = "��ɫ��Ͳ";
                    break;
                case "36921":
                    strBuildName = "ͶƱ��";
                    break;
                case "37021":
                    strBuildName = "UFO";
                    break;
                case "37121":
                    strBuildName = "������ͤ";
                    break;
                case "37221":
                    strBuildName = "�绰ͤ";
                    break;
                case "37321":
                    strBuildName = "��ɫָʾ��";
                    break;
                case "37421":
                    strBuildName = "��ɫָʾ��";
                    break;
                case "37521":
                    strBuildName = "�۾�ĸ������";
                    break;
                case "38221":
                    strBuildName = "Ͽ��";
                    break;
                case "42421":
                    strBuildName = "ʯ��";
                    break;
                case "42521":
                    strBuildName = "����Ȧ";
                    break;
                case "42621":
                    strBuildName = "������";
                    break;
                case "42721":
                    strBuildName = "�����̨";
                    break;
                case "42821":
                    strBuildName = "ˮ����";
                    break;
                case "42921":
                    strBuildName = "��ɫҡ��";
                    break;
                case "43021":
                    strBuildName = "ͯ���Ϲϳ�";
                    break;
                case "43121":
                    strBuildName = "ͯ��ˮ��Ь";
                    break;
                case "132":
                    strBuildName = "����";
                    break;
                case "232":
                    strBuildName = "����";
                    break;
                case "332":
                    strBuildName = "����";
                    break;
                case "432":
                    strBuildName = "����";
                    break;
                case "532":
                    strBuildName = "����";
                    break;
                case "632":
                    strBuildName = "�ù�";
                    break;
                case "732":
                    strBuildName = "�ù�";
                    break;
                case "832":
                    strBuildName = "�ù�";
                    break;
                case "932":
                    strBuildName = "�ù�";
                    break;
                case "1032":
                    strBuildName = "�ù�";
                    break;
                case "1132":
                    strBuildName = "�����";
                    break;
                case "1232":
                    strBuildName = "�����";
                    break;
                case "1332":
                    strBuildName = "�����";
                    break;
                case "1432":
                    strBuildName = "�����";
                    break;
                case "1532":
                    strBuildName = "�����";
                    break;
                case "1632":
                    strBuildName = "����ͤ";
                    break;
                case "1732":
                    strBuildName = "����ͤ";
                    break;
                case "1832":
                    strBuildName = "����ͤ";
                    break;
                case "1932":
                    strBuildName = "����ͤ";
                    break;
                case "2032":
                    strBuildName = "����ͤ";
                    break;
                case "2132":
                    strBuildName = "��͵�";
                    break;
                case "2232":
                    strBuildName = "��͵�";
                    break;
                case "2332":
                    strBuildName = "��͵�";
                    break;
                case "2432":
                    strBuildName = "��͵�";
                    break;
                case "2532":
                    strBuildName = "��͵�";
                    break;
                case "2631":
                    strBuildName = "�Ǳ�";
                    break;
                case "2731":
                    strBuildName = "�Ǳ�";
                    break;
                case "2831":
                    strBuildName = "�Ǳ�";
                    break;
                case "2931":
                    strBuildName = "�Ǳ�";
                    break;
                case "3031":
                    strBuildName = "�Ǳ�";
                    break;
                case "3132":
                    strBuildName = "¶Ӫ";
                    break;
                case "3232":
                    strBuildName = "¶Ӫ";
                    break;
                case "3332":
                    strBuildName = "¶Ӫ";
                    break;
                case "3431":
                    strBuildName = "������";
                    break;
                case "3531":
                    strBuildName = "������";
                    break;
                case "3631":
                    strBuildName = "������";
                    break;
                case "3931":
                    strBuildName = "�糵";
                    break;
                case "4031":
                    strBuildName = "�糵";
                    break;
                case "4131":
                    strBuildName = "�糵";
                    break;
                case "4231":
                    strBuildName = "�糵";
                    break;
                case "4331":
                    strBuildName = "�糵";
                    break;
                case "4431":
                    strBuildName = "ɳ̲����";
                    break;
                case "4531":
                    strBuildName = "ɳ̲����";
                    break;
                case "4631":
                    strBuildName = "ɳ̲����";
                    break;
                case "4732":
                    strBuildName = "����";
                    break;
                case "4832":
                    strBuildName = "����";
                    break;
                case "4932":
                    strBuildName = "����";
                    break;
                case "5032":
                    strBuildName = "����";
                    break;
                case "5132":
                    strBuildName = "����";
                    break;
                case "5232":
                    strBuildName = "����";
                    break;
                case "5332":
                    strBuildName = "����";
                    break;
                case "5432":
                    strBuildName = "����";
                    break;
                case "5532":
                    strBuildName = "����";
                    break;
                case "5632":
                    strBuildName = "����";
                    break;
                case "5731":
                    strBuildName = "��Ժ";
                    break;
                case "5831":
                    strBuildName = "��Ժ";
                    break;
                case "5931":
                    strBuildName = "��Ժ";
                    break;
                case "6031":
                    strBuildName = "��Ժ";
                    break;
                case "6131":
                    strBuildName = "��Ժ";
                    break;
                case "14431":
                    strBuildName = "����ok";
                    break;
                case "14531":
                    strBuildName = "����ok";
                    break;
                case "14631":
                    strBuildName = "����ok";
                    break;
                case "14732":
                    strBuildName = "��Ʒ��";
                    break;
                case "14832":
                    strBuildName = "��Ʒ��";
                    break;
                case "14932":
                    strBuildName = "��Ʒ��";
                    break;
                case "15032":
                    strBuildName = "��Ʒ��";
                    break;
                case "15132":
                    strBuildName = "��Ʒ��";
                    break;
                case "15231":
                    strBuildName = "�����";
                    break;
                case "15431":
                    strBuildName = "�����";
                    break;
                case "15531":
                    strBuildName = "��ɽ��";
                    break;
                case "15631":
                    strBuildName = "��ɽ��";
                    break;
                case "15731":
                    strBuildName = "��ɽ��";
                    break;
                case "15831":
                    strBuildName = "��ɽ��";
                    break;
                case "15931":
                    strBuildName = "��ɽ��";
                    break;
                case "16031":
                    strBuildName = "����";
                    break;
                case "16131":
                    strBuildName = "����";
                    break;
                case "16231":
                    strBuildName = "����";
                    break;
                case "16331":
                    strBuildName = "����";
                    break;
                case "16431":
                    strBuildName = "����";
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
                    strBuildName = "����ܵ�";
                    break;
                case "17132":
                    strBuildName = "����ܵ�";
                    break;
                case "17232":
                    strBuildName = "����ܵ�";
                    break;
                case "17332":
                    strBuildName = "����ܵ�";
                    break;
                case "17432":
                    strBuildName = "����ܵ�";
                    break;
                case "17532":
                    strBuildName = "ɳ̲��";
                    break;
                case "17632":
                    strBuildName = "ɳ̲��";
                    break;
                case "17732":
                    strBuildName = "ɳ̲��";
                    break;
                case "17832":
                    strBuildName = "ɳ̲��";
                    break;
                case "17932":
                    strBuildName = "ɳ̲��";
                    break;
                case "18031":
                    strBuildName = "��תľ��";
                    break;
                case "18131":
                    strBuildName = "��תľ��";
                    break;
                case "18231":
                    strBuildName = "��תľ��";
                    break;
                case "18331":
                    strBuildName = "��ͯ��԰";
                    break;
                case "18431":
                    strBuildName = "��ͯ��԰";
                    break;
                case "18832":
                    strBuildName = "���";
                    break;
                case "18932":
                    strBuildName = "���";
                    break;
                case "19032":
                    strBuildName = "���";
                    break;
                case "19132":
                    strBuildName = "���";
                    break;
                case "19232":
                    strBuildName = "���";
                    break;
                case "19332":
                    strBuildName = "����";
                    break;
                case "19432":
                    strBuildName = "����";
                    break;
                case "19532":
                    strBuildName = "����";
                    break;
                case "19632":
                    strBuildName = "����";
                    break;
                case "19732":
                    strBuildName = "����";
                    break;
                case "19832":
                    strBuildName = "���׻���";
                    break;
                case "19932":
                    strBuildName = "���׻���";
                    break;
                case "20032":
                    strBuildName = "���׻���";
                    break;
                case "20132":
                    strBuildName = "��¥";
                    break;
                case "20232":
                    strBuildName = "��¥";
                    break;
                case "20332":
                    strBuildName = "��¥";
                    break;
                case "20432":
                    strBuildName = "ˮ����";
                    break;
                case "20532":
                    strBuildName = "ˮ����";
                    break;
                case "20632":
                    strBuildName = "ˮ����";
                    break;
                case "20732":
                    strBuildName = "ˮ����";
                    break;
                case "20832":
                    strBuildName = "ˮ����";
                    break;
                case "20931":
                    strBuildName = "Ħ����";
                    break;
                case "21031":
                    strBuildName = "Ħ����";
                    break;
                case "21131":
                    strBuildName = "Ħ����";
                    break;
                case "21232":
                    strBuildName = "������";
                    break;
                case "21332":
                    strBuildName = "������";
                    break;
                case "21432":
                    strBuildName = "������";
                    break;
                case "21532":
                    strBuildName = "������";
                    break;
                case "21632":
                    strBuildName = "������";
                    break;
                case "36032":
                    strBuildName = "����";
                    break;
                case "36132":
                    strBuildName = "����";
                    break;
                case "37631":
                    strBuildName = "�ٲ�";
                    break;
                case "37731":
                    strBuildName = "�ٲ�";
                    break;
                case "37831":
                    strBuildName = "�ٲ�";
                    break;
                case "37931":
                    strBuildName = "ѩɽ";
                    break;
                case "38031":
                    strBuildName = "ѩɽ";
                    break;
                case "38131":
                    strBuildName = "ѩɽ";
                    break;
                case "38332":
                    strBuildName = "���������������";
                    break;
                case "38432":
                    strBuildName = "����͢�����";
                    break;
                case "38532":
                    strBuildName = "�Ĵ����������";
                    break;
                case "38632":
                    strBuildName = "�����������";
                    break;
                case "38732":
                    strBuildName = "���������";
                    break;
                case "38832":
                    strBuildName = "���������";
                    break;
                case "38932":
                    strBuildName = "���������";
                    break;
                case "39032":
                    strBuildName = "�¹������";
                    break;
                case "39132":
                    strBuildName = "���������";
                    break;
                case "39232":
                    strBuildName = "���������";
                    break;
                case "39332":
                    strBuildName = "���������";
                    break;
                case "39432":
                    strBuildName = "�鶼��˹�����";
                    break;
                case "39532":
                    strBuildName = "���������";
                    break;
                case "39632":
                    strBuildName = "����¡�����";
                    break;
                case "39732":
                    strBuildName = "���ص��������";
                    break;
                case "39832":
                    strBuildName = "���������";
                    break;
                case "40032":
                    strBuildName = "�Ϸ������";
                    break;
                case "40132":
                    strBuildName = "�������������";
                    break;
                case "40232":
                    strBuildName = "�����������";
                    break;
                case "40332":
                    strBuildName = "�ձ������";
                    break;
                case "40432":
                    strBuildName = "��ʿ�����";
                    break;
                case "40532":
                    strBuildName = "����ά�������";
                    break;
                case "40632":
                    strBuildName = "˹�工�������";
                    break;
                case "40732":
                    strBuildName = "˹�������������";
                    break;
                case "40832":
                    strBuildName = "�����������";
                    break;
                case "40932":
                    strBuildName = "�����������";
                    break;
                case "41032":
                    strBuildName = "ϣ�������";
                    break;
                case "41132":
                    strBuildName = "�����������";
                    break;
                case "41232":
                    strBuildName = "����������";
                    break;
                case "41332":
                    strBuildName = "Ӣ���������";
                    break;
                case "41432":
                    strBuildName = "���������";
                    break;
                case "41831":
                    strBuildName = "��Ϸ��";
                    break;
                case "41931":
                    strBuildName = "��Ϸ��";
                    break;
                case "42031":
                    strBuildName = "��Ϸ��";
                    break;
                case "42131":
                    strBuildName = "��ɽ";
                    break;
                case "42231":
                    strBuildName = "��ɽ";
                    break;
                case "42331":
                    strBuildName = "��ɽ";
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
            //�����û�������Ϣ
            //����:POST
            //����:authid=fad254cddb744968ecbf79cab3bb483b&ownerUid=228618602
            //http://rrisland.hapyfish.com/api/initisland?time=1278580378815
            string strContent = null;

            ShowSysLog("���ڼ��ؽ�����Ϣ");

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
                                li.SubItems.Add("���ο͹���");
                            }
                            else if (Convert.ToInt32((string)buildInfo[i]["deposit"]) > 0 && (int)buildInfo[i]["payRemainder"] == 0)
                            {
                                li.SubItems.Add("�ȴ�������");
                            }
                            else
                            {
                                li.SubItems.Add(TimeCal((int)buildInfo[i]["payRemainder"]));
                            }
                            li.SubItems.Add((string)buildInfo[i]["event"]);
                            listView_Main_Form_BuildList.Items.Add(li);
                        }
                    }
                    ShowSysLog("������Ϣ�������");
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
                                li.SubItems.Add("���ο͹���");
                            }
                            else if (Convert.ToInt32((string)buildInfo[i]["deposit"]) > 0 && (int)buildInfo[i]["payRemainder"] == 0)
                            {
                                li.SubItems.Add("�ȴ�������");
                            }
                            else
                            {
                                li.SubItems.Add(TimeCal((int)buildInfo[i]["payRemainder"]));
                            }
                            li.SubItems.Add((string)buildInfo[i]["event"]);
                            listView_Main_Form_BuildList.Items.Add(li);
                        }
                    }
                    ShowSysLog("������Ϣ�������");
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

            if (intOpID == 1)//�ջ��Լ����
            {
                //��ȡ���
                //����:POST
                //����:authid=e7f7a684538d63ff0c9b099a7514083a&itemId=1655997232
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
                            ShowSysLog("��ȡ�Լ� " + BuildClass((string)buildInfo[i]["cid"]) + " " + (string)buildInfo[i]["deposit"] + " ���");
                        }
                    }
                }
            }
            if (intOpID == 2)//�ջ���ѽ��
            {
                //��ȡ���ѽ��
                //����:POST
                //����:fid=272438384&authid=d7c00cf02c1b878d1dd831a8eeea5eb0&itemId=664380432
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
                                ShowSysLog("��ȡ���� " + strNickName + " " + BuildClass((string)buildInfo[i]["cid"]) + " +" + (int)o["coinChange"] + " ��� ���Ӿ��� +" + (int)o["expChange"]);
                            }
                        }
                    }
                }
            }
        }

        #region ������� ���������֡����� RandStr
        /// <summary>
        /// ������� ���������֡�����
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
            /// ��δ�ṩ��������,��Ĭ��������+Сд��ĸ����
            /// </summary>
            public RandStr()
            {
                framerStr = numStr + lowerStr;
            }

            /// <summary>
            /// ���캯��,��ָ�����ɵ��ַ�
            /// </summary>
            /// <param name="useNum">�Ƿ�ʹ������</param>
            /// <param name="useUpper">�Ƿ�ʹ�ô�д��ĸ</param>
            /// <param name="useLower">�Ƿ�ʹ��Сд��ĸ</param>
            /// <param name="useMark">�Ƿ�ʹ�÷���</param>
            public RandStr(bool useNum, bool useUpper, bool useLower, bool useMark)
            {
                // �����ͼ���첻�����κ�����ַ�����,���׳��쳣
                if (!useNum && !useUpper && !useLower && !useMark)
                {
                    throw new ArgumentException("��������ʹ��һ�ֹ����ַ�!");
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
            /// ʹ���Զ��������ַ�����
            /// </summary>
            /// <param name="userStr">�Զ����ַ�</param>
            public RandStr(string userStr)
            {
                // �����ͼ�ÿ��ַ���������,���׳��쳣
                if (userStr.Length == 0)
                {
                    throw new ArgumentException("������ʹ��һ���ַ�!");
                }
                else
                {
                    framerStr = userStr;
                }
            }

            /// <summary>
            /// ȡ��һ������ַ���
            /// </summary>
            /// <param name="length">ȡ������ַ����ĳ���</param>
            /// <returns>���ص�����ַ���</returns>
            public string GetRandStr(int length)
            {
                // ��ȡ�ĳ��Ȳ���Ϊ0�����߸�����
                if (length < 1)
                {
                    throw new ArgumentException("�ַ����Ȳ���Ϊ0���߸���!");
                }
                else
                {
                    // ���ֻ�ǻ�ȡ��������ַ���,
                    // ����û������.
                    // �������Ҫ��ʱ���ȡ��������ַ����Ļ�,
                    // �����������ܲ���.
                    // ���Ը���StringBuilder�����������,
                    // ��Ҫ�Ŀ����Լ���һ�� ^o^
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

        #region �Ա���¼���� TB_Login_Handle()
        /// <summary>
        /// �Ա���¼����
        /// </summary>
        public void TB_Login_Handle()
        {
            string strContent = null;       //
            string resultString = null;
            string strRedirectURL = null;   //��ȡ��ת
            string strRequest = null;       //��ȡGet����
            string strParameter = null;     //��ȡPost����
            string strCookie = null;        //��ȡCookie

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
                //����HTTP����Ĭ�ϱ���
                //HTTPproc.Encoding = System.Text.Encoding.UTF8;
                HTTPproc.Encoding = System.Text.Encoding.Default;

                #region �����һ��

                //==============  Cookie ����  =================
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

                #region ����ڶ���

                //==============  Cookie ����  =================
                //cookie2          522b0f7e4609ff4fde596087a9bce9f3
                //_tb_token_       477837e79383
                //t                fe8ad20f41c5f335d453be685896227e
                //v                0
                //_lang            zh_CN:GBK
                //uc1              lltime=1279085212&cookie14=UoM8cfXdNnANow==&existShop=false&cookie16=UtASsssmPlP/f1IHDsDaPRu+Pw==&sg=�?5&_yb_=false&cookie21=URm48syIZQ==&cookie15=W5iHLLyFOGW7aA==&_msg_v=true&_rt_=1251959051&_msg_=0
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

                #region ���������

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

                #region ������Ĳ�

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

                #region ������岽

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

                #region ���������

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

                #region ������߲�

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

                #region ������Ϸ�ж�����

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
                MessageBox.Show("��Ϣ��д�������飡", "ϵͳ��Ϣ");
            }
        }
        #endregion �Ա���¼���� Login_Handle()

        #region Url���� UrlEncode(string url)
        /// <summary>
        /// Url����
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