using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;
using System.Data;
using System.Threading;
using System.Runtime.InteropServices;

namespace Magic_Farm
{
    public partial class Main : Form
    {
        //[DllImport("ycode.dll")]
        //public static extern int loadcode(int code, int Length, String address, String pass);
        //[DllImport("ycode.dll", EntryPoint = "Recognition")]
        //public static extern string Recognition(int ItemNo, int picin, int Length, string Address1, string Address2, out int lppicout, out int lpLength, out int cLength);
        #region ��������

        public string strUserNames = null;
        public string strPasswords = null;
        public string strCookies = null;
        public string strImgCode = null;
        ArrayList al_Fushi = new ArrayList();
        ArrayList al_Jiaju = new ArrayList();
        ArrayList al_Meirong = new ArrayList();
        ArrayList al_Shipin = new ArrayList();
        ArrayList al_Dianqi = new ArrayList();

        DateTime dt = DateTime.Now;
        Magic_Farm.WebClient HTTPproc = new WebClient();
        Magic_Farm.WebClient HTTPproc_1 = new WebClient();

        #endregion ��������

        public Main()
        {
            InitializeComponent();
            SqlLibrary.IsOnline = true;
            
            if (!Directory.Exists("Image"))
            {
                string filePath = Path.GetFullPath(@"Image");
                Directory.CreateDirectory(filePath);
            }
            tabControl_Option.TabPages[1].Controls[0].Enabled = false;
            Thread t1 = new Thread(ReadProduct);
            t1.Start();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.listView_Fushi.ListViewItemSorter = new ListViewColumnSorter();
            this.listView_Fushi.ColumnClick += new ColumnClickEventHandler(ListViewHelper.ListView_ColumnClick);

            this.listView_Jiaju.ListViewItemSorter = new ListViewColumnSorter();
            this.listView_Jiaju.ColumnClick += new ColumnClickEventHandler(ListViewHelper.ListView_ColumnClick);

            this.listView_Meirong.ListViewItemSorter = new ListViewColumnSorter();
            this.listView_Meirong.ColumnClick += new ColumnClickEventHandler(ListViewHelper.ListView_ColumnClick);

            this.listView_Shipin.ListViewItemSorter = new ListViewColumnSorter();
            this.listView_Shipin.ColumnClick += new ColumnClickEventHandler(ListViewHelper.ListView_ColumnClick);

            this.listView_Dianqi.ListViewItemSorter = new ListViewColumnSorter();
            this.listView_Dianqi.ColumnClick += new ColumnClickEventHandler(ListViewHelper.ListView_ColumnClick);

            this.listView_MyBag.ListViewItemSorter = new ListViewColumnSorter();
            this.listView_MyBag.ColumnClick += new ColumnClickEventHandler(ListViewHelper.ListView_ColumnClick);
        }

        #region �Ա���¼���� TB_Login_Handle()
        /// <summary>
        /// �Ա���¼����
        /// </summary>
        public void TB_Login_Handle()
        {
            string strContent = null;       //
            string resultString = null;     //
            string strRedirectURL = null;   //��ȡ��ת
            string strRequest = null;       //��ȡGet����
            string strParameter = null;     //��ȡPost����
            string strCookie = null;        //��ȡCookie
            string strUserName = strUserNames;
            string strPassword = strPasswords;

            RandStr rndKey = new RandStr();
            RandStr rndKey_1 = new RandStr(true, false, false, false);
            
            if (strUserName.Trim() == "2" && strPassword.Trim() == "2")
            {
                strUserName = "cupid0426";
                strPassword = "850616cupid0426";
            }

            if (strUserName.Trim() == "3" && strPassword.Trim() == "3")
            {
                strUserName = "���Ķ���1985";
                strPassword = "shang851205";
            }

            CheckUser(strUserName);

            if (strUserName.Trim() != "" && strPassword.Trim() != "")
            {
                //����HTTP����Ĭ�ϱ���
                //HTTPproc.Encoding = System.Text.Encoding.UTF8;
                //HTTPproc.Encoding = System.Text.Encoding.;

                #region �����һ��

                //==============  Cookie ����  =================
                //cookie2          522b0f7e4609ff4fde596087a9bce9f3
                //_tb_token_       477837e79383
                //t                fe8ad20f41c5f335d453be685896227e
                //uc1              cookie14=UoM8cfXc1sA+RA==
                //v                0
                //_lang            zh_CN:GBK
                //==============================================

                strRequest = "http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F";
                HTTPproc.RequestHeaders.Add("Accept:text/html,application/xhtml+xml,application/xml;q=0.9;q=0.8");
                HTTPproc.RequestHeaders.Add("Accept-Language:zh;q=0.5");
                HTTPproc.RequestHeaders.Add("Accept-Encoding:gzip,deflate");
                HTTPproc.RequestHeaders.Add("Accept-Charset:GB2312,utf-8;q=0.7,*;q=0.7");
                HTTPproc.OpenRead(strRequest);

                resultString = HTTPproc.ResponseHeaders.ToString();
                resultString = Regex.Match(resultString, "Set-Cookie.*").Value.Replace("Set-Cookie: ", "").Replace(";", "\r\n").Replace(",", "\r\n").Replace("Path=/", "");
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
                string strCookieTemps = null;
                //HTTPproc.Encoding = System.Text.Encoding.UTF8;
                //HTTPproc.RequestHeaders.Add("Cookie:" + strCookie.Replace("\r", ""));
                HTTPproc.RequestHeaders.Add("Accept:text/html,application/xhtml+xml,application/xml;q=0.9;q=0.8");
                HTTPproc.RequestHeaders.Add("Accept-Language:zh;q=0.5");
                HTTPproc.RequestHeaders.Add("Accept-Encoding:gzip,deflate");
                HTTPproc.RequestHeaders.Add("Accept-Charset:GB2312,utf-8;q=0.7,*;q=0.7");
                HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F");
                strCookie = strCookie.Replace(";", "\r\n");
                string strToken = Regex.Match(strCookie, "_tb_token_=.*").Value.Replace("_tb_token_=", "");
                strParameter = "TPL_username=" + UrlEncode(strUserName) + "&TPL_password=" + strPassword + "&_tb_token_=" + strToken + "&action=Authenticator&event_submit_do_login=anything&TPL_redirect_url=http%3A%2F%2Fbo.tianxia.taobao.com%2F&from=tbTop&fc=2&style=default&tid=&support=000001&CtrlVersion=1%2C0%2C0%2C7&loginType=3&minititle=&minipara=&pstrong=2&longLogin=-1&llnick=&sign=&need_sign=&isIgnore=&popid=&callback=&not_duplite_str=&need_user_id=&from_encoding=";
                HTTPproc.OpenRead("http://login.taobao.com/member/login.jhtml", strParameter);
                string strCookie_1 = HTTPproc.RespHtml;

                try
                {
                    Regex regexObj = new Regex("Set-Cookie.*");
                    Match matchResults = regexObj.Match(strCookie_1);
                    while (matchResults.Success)
                    {
                        // matched text: matchResults.Value
                        // match start: matchResults.Index
                        // match length: matchResults.Length
                        strCookieTemp = matchResults.Value.ToString().Replace("Set-Cookie: ","");
                        strCookieTemps += Regex.Replace(strCookieTemp, "Domain=.*", "");
                        matchResults = matchResults.NextMatch();
                    }
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                //resultString = Regex.Match(HTTPproc.ResponseHeaders.ToString(), "Set-Cookie.*").Value.Replace("Set-Cookie: ", "").Replace(";", "\r\n").Replace(",", "\r\n").Replace("Path=/", "");

                //strCookie = strCookieTemp.Replace(";", "\r\n");
                //strCookie = Regex.Replace(strCookie, "uc1=.*", "");
                //resultString = strCookie + resultString;

                //strCookie = Regex.Match(resultString, "cookie2=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "_tb_token_=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "t=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "v=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "_lang=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "uc1=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "ck1=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "_sv_=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "tg=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "_cc_=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "_nk_=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "nt=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "_l_g_=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "_wwmsg_=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "tracknick=.*").Value + ";";
                //strCookie += Regex.Match(resultString, "ssllogin=.*").Value + ";";
                //strCookie += Regex.Match(strCookie_1, "lastgetwwmsg=.*; D").Value.Replace("; D", ";");
                //strCookie += Regex.Match(strCookie_1, "cookie1=.*;D").Value.Replace(";D", ";");
                //strCookie += Regex.Match(strCookie_1, "cookie17=.*;D").Value.Replace(";D", ";");
                //strCookie = strCookie.Replace("\r\n", ";").Replace("\r", "").Replace("\n", "");

                #endregion

                #region ���������

                strCookieTemp = strCookie;
                HTTPproc.RequestHeaders.Add("Cookie:" + strCookieTemps);
                HTTPproc.RequestHeaders.Add("Accept:text/html,application/xhtml+xml,application/xml;q=0.9;q=0.8");
                HTTPproc.RequestHeaders.Add("Accept-Language:zh;q=0.5");
                HTTPproc.RequestHeaders.Add("Accept-Encoding:gzip,deflate");
                HTTPproc.RequestHeaders.Add("Accept-Charset:GB2312,utf-8;q=0.7,*;q=0.7");
                HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F");

                strRequest = "http://bo.tianxia.taobao.com";
                HTTPproc.OpenRead(strRequest);

                resultString = Regex.Match(HTTPproc.ResponseHeaders.ToString(), "Set-Cookie.*").Value.Replace("Set-Cookie: ", "").Replace("; Domain=.taobao.com; Path=/\r", "");
                resultString = resultString.Replace(" Path=/", "");
                strCookie = strCookieTemp.Replace(";", "\r\n");
                strCookie = Regex.Replace(strCookie, "uc1=.*", "");
                resultString = strCookie + strCookieTemps + resultString;
                strCookie = resultString.Replace("\r\n", ";").Replace("\r", "").Replace("\n", "").Replace(";;  ", ";");
                strCookies = strCookie;

                #endregion                

                #region ������Ĳ�

                strCookieTemp = strCookie;
                HTTPproc.RequestHeaders.Add("Cookie:" + strCookie.Replace("\r", "").Replace("\n", ""));
                HTTPproc.RequestHeaders.Add("Accept:text/html,application/xhtml+xml,application/xml;q=0.9;q=0.8");
                HTTPproc.RequestHeaders.Add("Accept-Language:zh;q=0.5");
                HTTPproc.RequestHeaders.Add("Accept-Encoding:gzip,deflate");
                HTTPproc.RequestHeaders.Add("Accept-Charset:GB2312,utf-8;q=0.7,*;q=0.7");
                HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F");

                strRequest = " http://container.open.taobao.com/container?appkey=12095553&return_url=http%3A%2F%2Fbo.tianxia.taobao.com%2F";
                HTTPproc.OpenRead(strRequest);

                //resultString = Regex.Match(HTTPproc.ResponseHeaders.ToString(), "Set-Cookie.*").Value.Replace("Set-Cookie: ", "").Replace("; Domain=.taobao.com; Path=/\r", "");
                resultString = HTTPproc.ResponseHeaders.ToString();
                strRequest = Regex.Match(resultString, "http://.*").Value;

                #endregion

                HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F");
                HTTPproc.OpenRead(strRequest);

                ReadUserInfo(strCookie);
                getMySeeds(strCookie);
                ReadMyBag();
            }
            else
            {
                MessageBox.Show("��Ϣ��д�������飡", "ϵͳ��Ϣ");
            }
        }
        #endregion �Ա���¼���� Login_Handle()

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
        #endregion ������� ���������֡����� RandStr

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

        #region ��ȡ�û���Ϣ ReadUserInfo(string strCookie)
        /// <summary>
        /// ��ȡ�û���Ϣ
        /// </summary>
        /// <param name="strCookie"></param>
        private void ReadUserInfo(string strCookie)
        {
            //�û���Ϣ��ȡURL
            //��ʽ:JSON
            //http://bo.tianxia.taobao.com/taoboyuan/interface/getUserInfo.jhtml
            string strContent = null;
            string strCookieTemp = strCookie;
            string strRequest = null;
            string resultString = null;

            strCookieTemp = strCookie;
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookie.Replace("\r", "").Replace("\n", ""));
            HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F");

            strRequest = "http://bo.tianxia.taobao.com/taoboyuan/interface/getUserInfo.jhtml";
                          //http://bo.tianxia.taobao.com/taoboyuan/interface/getUserInfo.jhtml?t=1284964452466
            strContent = HTTPproc.OpenRead(strRequest);

            JObject o = JObject.Parse(Unicode2Character(strContent));
            label_NickName.Text = (string)o["user"]["nick"];
            label_Exp.Text = (int)o["user"]["exp"] + "/" + (int)o["user"]["nextExp"];
            label_Level.Text = Convert.ToString((int)o["user"]["level"]);
            label_Score.Text = Convert.ToString((int)o["user"]["score"]);

            HTTPproc.Encoding = System.Text.Encoding.Default;
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookie.Replace("\r", "").Replace("\n", ""));
            HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F");

            strRequest = "http://bo.tianxia.taobao.com/taoboyuan/seedstore.htm";
            strContent = HTTPproc.OpenRead(strRequest);

            try
            {
                resultString = Regex.Match(strContent, "������.*</div>").Value;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            label_Notice.Text = resultString.Replace("</div>", "").Replace("&nbsp;&nbsp;", ",");

            //MessageBox.Show(strContent);
        }
        #endregion ѡȡ�û���Ϣ ReadUserInfo(string strCookie)

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

        #region ��ȡ��Ʒ���� ReadProduct()
        /// <summary>
        /// ��ȡ��Ʒ����
        /// </summary>
        private void ReadProduct()
        {
            //����HTTP����Ĭ�ϱ���
            HTTPproc.Encoding = System.Text.Encoding.Default;

            string resultString = null;
            string resultString_0 = null;
            string resultString_1 = null;
            string resultString_2 = null;
            string resultString_3 = null;
            string resultString_4 = null;
            string resultString_5 = null;
            string strCatId = null;
            int intPage = 0;
            int i = 0;
            int j = 0;
            int k = 0;

            //��������  --- id=788" target="_blank"><span class="title"><font title="������Ծ�����T��">������Ծ�����T��</font>
            Regex regexObj = new Regex(@"id=\d{1,4}.*</font>.*\r\n.*<br>");
            Regex regexObj_1 = new Regex("src=\"http://img.*.jpg");

            for (j = 1; j <= 5; j++)
            {
                resultString = HTTPproc_1.OpenRead("http://bo.tianxia.taobao.com/taoboyuan/seedstore_category.jhtml?pageNo=1&catId=" + j + "");
                try
                {
                    resultString_4 = resultString;
                    resultString = Regex.Match(resultString, @"��\d{1,2}ҳ").Value.Replace("��", "").Replace("ҳ", "");                    
                    intPage = Convert.ToInt32(resultString);
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                for (k = 1; k <= intPage; k++)
                {
                    if (k > 1)
                    {
                        resultString = HTTPproc_1.OpenRead("http://bo.tianxia.taobao.com/taoboyuan/seedstore_category.jhtml?pageNo=" + k + "&catId=" + j + "");
                    }

                    if (k > 1)
                    {
                        
                    }
                    else
                    {
                        resultString = resultString_4;
                    }

                    Match matchResults = regexObj.Match(resultString);
                    Match matchResults_1 = regexObj_1.Match(resultString);
                    while (matchResults.Success)
                    {
                        try
                        {
                            //����ID����  --- id=788" target="_blank"><span class="title"><font title="������Ծ�����T��">������Ծ�����T��</font>
                            resultString_0 = Regex.Match(matchResults.ToString(), @"id=\d{1,4}").Value.Replace("id=", "");
                            //������������  --- id=788" target="_blank"><span class="title"><font title="������Ծ�����T��">������Ծ�����T��</font>
                            //resultString_1 = Regex.Match(matchResults.ToString(), "<font.*</font>").Value;
                            //resultString_1 = Regex.Replace(resultString_1, "<font.*\">", "").Replace("</font>", "");
                            resultString_1 = Regex.Match(matchResults.ToString(), "<font.*\">").Value.Replace("<font title=\"", "").Replace("\">", "");
                            //����ͨ������  --- ����ͨ��:<span class="xuyao">40</span><br>
                            resultString_2 = Regex.Match(matchResults.ToString(), @"\d{1,3}</span>").Value.Replace("</span>", "");
                            //����ͼƬ����
                            resultString_3 = matchResults_1.ToString().Replace("src=\"", "");
                            //al_Product.Add(resultString_3);
                            resultString_5 = Regex.Match(resultString_3.ToString(), "/T.*.160x160.jpg").Value.Replace("/", "");

                            if (j == 1)
                            {
                                al_Fushi.Add(resultString_3);
                            }
                            if (j == 2)
                            {
                                al_Jiaju.Add(resultString_3);
                            }
                            if (j == 3)
                            {
                                al_Meirong.Add(resultString_3);
                            }
                            if (j == 4)
                            {
                                al_Shipin.Add(resultString_3);
                            }
                            if (j == 5)
                            {
                                al_Dianqi.Add(resultString_3);
                            }

                            ListViewItem li = new ListViewItem();
                            li.SubItems.Clear();
                            li.SubItems[0].Text = resultString_0.Replace("\">", "");
                            li.SubItems.Add(resultString_1);
                            li.SubItems.Add(resultString_2);
                            li.SubItems.Add(resultString_5);

                            if (j == 1)
                            {
                                listView_Fushi.Items.Add(li);
                            }
                            if (j == 2)
                            {
                                listView_Jiaju.Items.Add(li);
                            }
                            if (j == 3)
                            {
                                listView_Meirong.Items.Add(li);
                            }
                            if (j == 4)
                            {
                                listView_Shipin.Items.Add(li);
                            }
                            if (j == 5)
                            {
                                listView_Dianqi.Items.Add(li);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                        matchResults = matchResults.NextMatch();
                        matchResults_1 = matchResults_1.NextMatch();
                    }

                    #region ͼƬ���ص�����Ŀ¼

                    if (j == 1)
                    {
                        for (i = 0; i < al_Fushi.Count; i++)
                        {
                            try
                            {
                                resultString = Regex.Match(al_Fushi[i].ToString(), "/T.*.160x160.jpg").Value.Replace("/", "");
                                if (File.Exists(@"Image\\" + resultString + ""))
                                {
                                    //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                }
                                else
                                {
                                    HTTPproc_1.DownloadFile(al_Fushi[i].ToString(), @"Image\\" + resultString + "");
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                // Syntax error in the regular expression
                            }
                        }
                    }
                    if (j == 2)
                    {
                        for (i = 0; i < al_Jiaju.Count; i++)
                        {
                            try
                            {
                                resultString = Regex.Match(al_Jiaju[i].ToString(), "/T.*.160x160.jpg").Value.Replace("/", "");
                                if (File.Exists(@"Image\\" + resultString + ""))
                                {
                                    //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                }
                                else
                                {
                                    HTTPproc_1.DownloadFile(al_Jiaju[i].ToString(), @"Image\\" + resultString + "");
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                // Syntax error in the regular expression
                            }
                        }
                    }
                    if (j == 3)
                    {
                        for (i = 0; i < al_Meirong.Count; i++)
                        {
                            try
                            {
                                resultString = Regex.Match(al_Meirong[i].ToString(), "/T.*.160x160.jpg").Value.Replace("/", "");
                                if (File.Exists(@"Image\\" + resultString + ""))
                                {
                                    //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                }
                                else
                                {
                                    HTTPproc_1.DownloadFile(al_Meirong[i].ToString(), @"Image\\" + resultString + "");
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                // Syntax error in the regular expression
                            }
                        }
                    }
                    if (j == 4)
                    {
                        for (i = 0; i < al_Shipin.Count; i++)
                        {
                            try
                            {
                                resultString = Regex.Match(al_Shipin[i].ToString(), "/T.*.160x160.jpg").Value.Replace("/", "");
                                if (File.Exists(@"Image\\" + resultString + ""))
                                {
                                    //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                }
                                else
                                {
                                    HTTPproc_1.DownloadFile(al_Shipin[i].ToString(), @"Image\\" + resultString + "");
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                // Syntax error in the regular expression
                            }
                        }
                    }
                    if (j == 5)
                    {
                        for (i = 0; i < al_Dianqi.Count; i++)
                        {
                            try
                            {
                                resultString = Regex.Match(al_Dianqi[i].ToString(), "/T.*.160x160.jpg").Value.Replace("/", "");
                                if (File.Exists(@"Image\\" + resultString + ""))
                                {
                                    //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                }
                                else
                                {
                                    HTTPproc_1.DownloadFile(al_Dianqi[i].ToString(), @"Image\\" + resultString + "");
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                // Syntax error in the regular expression
                            }
                        }
                    }                    

                    #endregion ͼƬ���ص�����Ŀ¼
                }
            }
            Main.CheckForIllegalCrossThreadCalls = false;
            tabControl_Option.TabPages[1].Controls[0].Enabled = true;
        }
        #endregion ��ȡ��Ʒ���� ReadProduct()

        #region ��ʾͼƬ ShowImage(string strPath)
        /// <summary>
        /// ��ʾͼƬ
        /// </summary>
        /// <param name="strPath"></param>
        private void ShowImage(string strPath)
        {
            string resultString = null;
            try
            {
                resultString = Regex.Match(strPath, "/T.*.160x160.jpg").Value.Replace("/", "");
                if (File.Exists(@"Image\\" + resultString + ""))
                {                    
                    pictureBox_ShowImg.ImageLocation = @"Image\\" + resultString + "";
                }
                else
                {
                    pictureBox_ShowImg.ImageLocation = @"Image\\" + strPath + "";
                }                
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
        }
        #endregion ��ʾͼƬ ShowImage(string strPath)

        #region ListView ����

        private void listView_Fushi_MouseClick(object sender, MouseEventArgs e)
        {
            int i = 0;
            foreach (ListViewItem li in listView_Fushi.SelectedItems)
            {
                if (listView_Fushi.SelectedIndices.Count > 0)
                {
                    i = li.Index;
                    //ShowImage(al_Fushi[i].ToString());
                    ShowImage(listView_Fushi.Items[i].SubItems[3].Text.ToString());
                    //MessageBox.Show(iiii + "," + al_Product[iiii].ToString());
                }
                else
                {
                    break;
                }
            }
        }

        private void listView_Jiaju_MouseClick(object sender, MouseEventArgs e)
        {
            int i = 0;
            foreach (ListViewItem li in listView_Jiaju.SelectedItems)
            {
                if (listView_Jiaju.SelectedIndices.Count > 0)
                {
                    i = li.Index;
                    //ShowImage(al_Jiaju[i].ToString());
                    ShowImage(listView_Jiaju.Items[i].SubItems[3].Text.ToString());
                    //MessageBox.Show(iiii + "," + al_Product[iiii].ToString());
                }
                else
                {
                    break;
                }
            }
        }

        private void listView_Meirong_MouseClick(object sender, MouseEventArgs e)
        {
            int i = 0;
            foreach (ListViewItem li in listView_Meirong.SelectedItems)
            {
                if (listView_Meirong.SelectedIndices.Count > 0)
                {
                    i = li.Index;
                    //ShowImage(al_Meirong[i].ToString());
                    ShowImage(listView_Meirong.Items[i].SubItems[3].Text.ToString());
                    //MessageBox.Show(iiii + "," + al_Product[iiii].ToString());
                }
                else
                {
                    break;
                }
            }
        }

        private void listView_Shipin_MouseClick(object sender, MouseEventArgs e)
        {
            int i = 0;
            foreach (ListViewItem li in listView_Shipin.SelectedItems)
            {
                if (listView_Shipin.SelectedIndices.Count > 0)
                {
                    i = li.Index;
                    //ShowImage(al_Shipin[i].ToString());
                    ShowImage(listView_Shipin.Items[i].SubItems[3].Text.ToString());
                    //MessageBox.Show(iiii + "," + al_Product[iiii].ToString());
                }
                else
                {
                    break;
                }
            }
        }

        private void listView_Dianqi_MouseClick(object sender, MouseEventArgs e)
        {
            int i = 0;
            foreach (ListViewItem li in listView_Dianqi.SelectedItems)
            {
                if (listView_Dianqi.SelectedIndices.Count > 0)
                {
                    i = li.Index;
                    //ShowImage(al_Dianqi[i].ToString());
                    ShowImage(listView_Dianqi.Items[i].SubItems[3].Text.ToString());
                    //MessageBox.Show(iiii + "," + al_Product[iiii].ToString());
                }
                else
                {
                    break;
                }
            }
        }

        #endregion ListView ����

        #region ��ȡ������Ϣ getMySeeds(string strCookie)
        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        /// <param name="strCookie"></param>
        private void getMySeeds(string strCookie)
        {
            //��ȡ���ص�ȡURL
            //��ʽ:JSON
            //http://bo.tianxia.taobao.com/taoboyuan/interface/getMySeeds.jhtml?t=1284084634446

            string strContent = null;
            string strCookieTemp = strCookie;
            string strRequest = null;
            int intgrowTime = 0;
            int inttotalTime = 0;

            RandStr rndKey = new RandStr(true, false, false, false);

            strCookieTemp = strCookie;
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookie.Replace("\r", "").Replace("\n", ""));
            HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F");

            strRequest = "http://bo.tianxia.taobao.com/taoboyuan/interface/getMySeeds.jhtml?t=" + rndKey.GetRandStr(12) + "";
            strContent = HTTPproc.OpenRead(strRequest);

            JObject o = JObject.Parse(Unicode2Character(strContent));
            JArray seeds = (JArray)o["data"]["seeds"];

            if (listView_ShowSeed.Items.Count > 0)
            {
                foreach (ListViewItem li in listView_ShowSeed.Items)
                {
                    if (listView_ShowSeed.Items.Count > 0)
                    {
                        li.Remove();
                    }
                    else
                    {
                        break;
                    }
                }                
            }

            for (int i = 0; i < seeds.Count(); i++)
            {
                ListViewItem li = new ListViewItem();
                li.SubItems.Clear();
                li.SubItems[0].Text = Convert.ToString((int)seeds[i]["mySeedId"]);
                li.SubItems.Add((string)seeds[i]["title"]);
                intgrowTime = (int)seeds[i]["growTime"];
                inttotalTime = (int)seeds[i]["totalTime"];

                if (intgrowTime == inttotalTime)
                {
                    li.SubItems.Add("�ѳ���");
                }
                else
                {
                    int intT_Time = inttotalTime - intgrowTime;
                    int intSecond = intT_Time % 60;
                    int intMinute = ((intT_Time - intSecond) / 60) % 60;
                    int intHour = (intT_Time - intSecond) / 3600;
                    li.SubItems.Add(string.Format("{0}Сʱ{1}��{2}��", intHour, intMinute, intSecond));
                }

                listView_ShowSeed.Items.Add(li);
            }
        }
        #endregion ��ȡ������Ϣ getMySeeds(string strCookie)

        #region ��ʾϵͳ��־ ShowSysLog(string strLog)
        /// <summary>
        /// ��ʾϵͳ��־
        /// </summary>
        /// <param name="strLog"></param>        
        private void ShowSysLog(string strLog)
        {
            DateTime dt_1 = DateTime.Now;
            textBox_RunLog.Text += dt_1 + "  " + strLog + "\r\n";
            textBox_RunLog.SelectionStart = textBox_RunLog.Text.Length;
            textBox_RunLog.ScrollToCaret();
            //ʼ����ʾTextBox����һ�У�ʼ�չ�������ײ�
            textBox_RunLog.SelectionStart = textBox_RunLog.Text.Length;
            textBox_RunLog.ScrollToCaret();
        }
        #endregion

        #region ˫���ջ��ʵ

        private void listView_ShowSeed_DoubleClick(object sender, EventArgs e)
        {
            string strID = null;
            foreach (ListViewItem li in listView_ShowSeed.SelectedItems)
            {
                if (listView_ShowSeed.SelectedIndices.Count > 0)
                {
                    strID = li.SubItems[0].Text;
                    ShowSysLog("��ʼ�ջ���ʵ���Ժ�...");
                    if (growSeed(strID, strCookies) == "true")
                    {
                        li.Remove();
                        ReadUserInfo(strCookies);
                        getMySeeds(strCookies);
                    }
                    else
                    {
                        getMySeeds(strCookies);
                        ShowSysLog("��ʵ��ȡʧ��...");
                    }
                }
                else
                {
                    break;
                }
            }
        }

        #endregion ˫���ջ��ʵ

        #region �ջ��ʵ���� growSeed(string strID, string strCookie)

        /// <summary>
        /// �ջ��ʵ����
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="strCookie"></param>
        /// <returns></returns>
        private string growSeed(string strID, string strCookie)
        {
            //�ջ��ʳ��ȡURL
            //��ʽ:GET
            //http://bo.tianxia.taobao.com/taoboyuan/service/gain.jhtml?timeStamp=1283949653089&id=2124992&flag=1

            string strContent = null;
            string strCookieTemp = strCookie;
            string strRequest = null;
            //int n1, n2, n3 = 0;
            bool boolCode = true;

            RandStr rndKey = new RandStr(true, false, false, false);
            
            strCookieTemp = strCookie;
            HTTPproc.Encoding = System.Text.Encoding.Default;
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookie.Replace("\r", "").Replace("\n", ""));
            HTTPproc.RequestHeaders.Add("Accept:text/html,application/xhtml+xml,application/xml;q=0.9;q=0.8");
            HTTPproc.RequestHeaders.Add("Accept-Language:zh;q=0.5");
            HTTPproc.RequestHeaders.Add("Accept-Encoding:gzip,deflate");
            HTTPproc.RequestHeaders.Add("Accept-Charset:GB2312,utf-8;q=0.7,*;q=0.7");
            HTTPproc.RequestHeaders.Add("Referer:http://bo.tianxia.taobao.com/taoboyuan/myfarm.htm");

            //��֤��ʶ��
            //loadcode(0, 0, "MagicFarm.fc", "qweqwe123");
            strRequest = "http://bo.tianxia.taobao.com/taoboyuan/service/checkGain.jhtml?timeStamp=" + rndKey.GetRandStr(13) + "&id=" + strID + "";
            strContent = HTTPproc.OpenRead(strRequest);


            strRequest = "http://bo.tianxia.taobao.com/taoboyuan/gain/checkCode.jhtml?id=" + strID + "";
            string ssss = strRequest;

            ImgCode ImgCode_Form = new ImgCode();
            ImgCode_Form.strCookies = HTTPproc.Cookie.ToString();
            ImgCode_Form.strRequest = strRequest;

            while (boolCode)
            {
                //ImgCode ImgCode_Form = new ImgCode();
                //ImgCode_Form.strCookies = strCookie;
                //ImgCode_Form.strRequest = strRequest;
                DialogResult r = ImgCode_Form.ShowDialog();
                if (r == DialogResult.OK)
                {
                    strImgCode = ImgCode_Form.strImgCode;

                    //strRequest = "http://bo.tianxia.taobao.com/GainCode";
                    //HTTPproc.DownloadFile(strRequest, @"Image\Code.jpg");
                    //strCode = Recognition(1, 0, 0, "", "Image\\Code.jpg", out n1, out n2, out n3);
                    HTTPproc.RequestHeaders.Add("Accept:text/html,application/xhtml+xml,application/xml;q=0.9;q=0.8");
                    HTTPproc.RequestHeaders.Add("Accept-Language:zh;q=0.5");
                    HTTPproc.RequestHeaders.Add("Accept-Encoding:gzip,deflate");
                    HTTPproc.RequestHeaders.Add("Accept-Charset:GB2312,utf-8;q=0.7,*;q=0.7");
                    HTTPproc.RequestHeaders.Add("Referer:" + ssss + "");
                    strRequest = "http://bo.tianxia.taobao.com/taoboyuan/service/checkGainCode.jhtml?timeStamp=" + rndKey.GetRandStr(13) + "&code=" + strImgCode + "";
                    //strRequest = "http://bo.tianxia.taobao.com/taoboyuan/service/checkGainCode.jhtml?timeStamp=" + rndKey.GetRandStr(13) + "&code=" + strCode + "";
                    //http://bo.tianxia.taobao.com/taoboyuan/service/checkGainCode.jhtml?timeStamp=1285339418841                &code=wwcgc
                    strContent = HTTPproc.OpenRead(strRequest);
                    int intA = strContent.IndexOf("{");
                    int intB = strContent.IndexOf("}");
                    strContent.Substring(intA, intB - intA + 1);

                    JObject oStatus = JObject.Parse(Unicode2Character(strContent));
                    int intStatus = (int)oStatus["status"];  //����״̬�ж�
                    if (intStatus > 0)
                    {
                        strRequest = "http://bo.tianxia.taobao.com/taoboyuan/service/gain.jhtml?timeStamp=" + rndKey.GetRandStr(13) + "&id=" + strID + "&code=" + strImgCode + "";
                        //http://bo.tianxia.taobao.com/taoboyuan/service/gain.jhtml?timeStamp=1285339419449                &id=6327414      &code=wwcgc
                        strContent = HTTPproc.OpenRead(strRequest);
                        boolCode = false;
                    }
                    else
                    {
                        ImgCode_Form.Dispose();
                        ImgCode_Form = new ImgCode();
                        ImgCode_Form.strCookies = HTTPproc.Cookie.ToString();
                        ImgCode_Form.strRequest = ssss;
                    }
                }
                else
                {
                    break;
                }
            }

            try
            {
                JObject o = JObject.Parse(Unicode2Character(strContent));
                string strUpGrade = Convert.ToString((int)o["data"]["upgrade"]);  //����״̬�ж�
                string strResult = Convert.ToString((int)o["data"]["result"]);    //�ջ�״̬�ж�
                string strMsg = (string)o["data"]["msg"];                         //ϵͳ��Ϣ

                if (strResult == "3")
                {
                    ShowSysLog(strMsg);
                    return "true";
                }
                if (strResult == "1")
                {
                    ShowSysLog(strMsg);
                    return "true";
                }
                else
                {
                    ShowSysLog(strMsg);
                    return "false";
                }
            }
            catch
            {
                return "false";
            }
        }

        #endregion �ջ��ʵ���� growSeed(string strID, string strCookie)

        #region ˫������
        private void listView_Fushi_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string strID = null;
            foreach (ListViewItem li in listView_Fushi.SelectedItems)
            {
                if (listView_Fushi.SelectedIndices.Count > 0)
                {
                    strID = li.SubItems[0].Text;
                    if (buySeed(strID, strCookies) == "true")
                    {
                        getMySeeds(strCookies);
                        ReadUserInfo(strCookies);
                    }
                    else
                    {

                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void listView_Jiaju_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string strID = null;
            foreach (ListViewItem li in listView_Jiaju.SelectedItems)
            {
                if (listView_Jiaju.SelectedIndices.Count > 0)
                {
                    strID = li.SubItems[0].Text;
                    if (buySeed(strID, strCookies) == "true")
                    {
                        getMySeeds(strCookies);
                        ReadUserInfo(strCookies);
                    }
                    else
                    {

                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void listView_Meirong_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string strID = null;
            foreach (ListViewItem li in listView_Meirong.SelectedItems)
            {
                if (listView_Meirong.SelectedIndices.Count > 0)
                {
                    strID = li.SubItems[0].Text;
                    if (buySeed(strID, strCookies) == "true")
                    {
                        getMySeeds(strCookies);
                        ReadUserInfo(strCookies);
                    }
                    else
                    {

                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void listView_Shipin_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string strID = null;
            foreach (ListViewItem li in listView_Shipin.SelectedItems)
            {
                if (listView_Shipin.SelectedIndices.Count > 0)
                {
                    strID = li.SubItems[0].Text;
                    if (buySeed(strID, strCookies) == "true")
                    {
                        getMySeeds(strCookies);
                        ReadUserInfo(strCookies);
                    }
                    else
                    {

                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void listView_Dianqi_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string strID = null;
            foreach (ListViewItem li in listView_Dianqi.SelectedItems)
            {
                if (listView_Dianqi.SelectedIndices.Count > 0)
                {
                    strID = li.SubItems[0].Text;
                    if (buySeed(strID, strCookies) == "true")
                    {
                        getMySeeds(strCookies);
                        ReadUserInfo(strCookies);
                    }
                    else
                    {

                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void listView_MyBag_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string strID = null;
            foreach (ListViewItem li in listView_MyBag.SelectedItems)
            {
                if (listView_MyBag.SelectedIndices.Count > 0)
                {
                    strID = li.SubItems[0].Text;
                    if (growSeeds(strID, strCookies) == "true")
                    {
                        getMySeeds(strCookies);
                        ReadUserInfo(strCookies);
                        li.Remove();
                    }
                    else
                    {
                        ShowSysLog("���� " + li.SubItems[1].Text + " ��ֲʧ�ܣ�");
                        ReadMyBag();
                    }
                }
                else
                {
                    break;
                }
            }
        }

        #endregion ˫������

        #region �������� buySeed(string strID, string strCookie)
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="strCookie"></param>
        /// <returns></returns>
        private string buySeed(string strID, string strCookie)
        {
            //�������ӵ�ȡURL
            //��ʽ:GET
            //http://bo.tianxia.taobao.com/taoboyuan/service/buySeed.jhtml?timeStamp=1283948156278&id=418

            string strContent = null;
            string strCookieTemp = strCookie;
            string strRequest = null;

            RandStr rndKey = new RandStr(true, false, false, false);

            strCookieTemp = strCookie;
            HTTPproc.Encoding = System.Text.Encoding.Default;
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookie.Replace("\r", "").Replace("\n", ""));
            HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F");

            strRequest = "http://bo.tianxia.taobao.com/taoboyuan/service/buySeed.jhtml?timeStamp=" + rndKey.GetRandStr(13) + "&id=" + strID;
            strContent = HTTPproc.OpenRead(strRequest);

            JObject o = JObject.Parse(Unicode2Character(strContent));
            string strStatus = Convert.ToString((int)o["status"]);      //״̬�ж�
            string strScore = Convert.ToString((int)o["score"]);        //ʣ��ͨ��
            string strMsg = (string)o["msg"];                           //ϵͳ��Ϣ
            string strMySeedId = Convert.ToString((int)o["mySeedId"]);  //���ӱ�ʶ

            if (strStatus == "1")
            {
                ShowSysLog(strMsg + "  ʣ��ͨ��:" + strScore);
                growSeeds(strMySeedId, strCookies);
                return "true";
            }
            else
            {
                ShowSysLog(strMsg);
                ReadMyBag();
                return "false";
            }
        }

        #endregion �������� buySeed(string strID, string strCookie)

        #region ��ֲ���� growSeeds(string strID, string strCookie)
        /// <summary>
        /// ��ֲ����
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="strCookie"></param>
        /// <returns></returns>
        private string growSeeds(string strID, string strCookie)
        {
            //��ֲ���ӵ�ȡURL
            //��ʽ:GET
            //http://bo.tianxia.taobao.com/taoboyuan/service/growSeed.jhtml?timeStamp=1283948371005&id=2308730

            string strContent = null;
            string strCookieTemp = strCookie;
            string strRequest = null;

            RandStr rndKey = new RandStr(true, false, false, false);

            strCookieTemp = strCookie;
            HTTPproc.Encoding = System.Text.Encoding.Default;
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookie.Replace("\r", "").Replace("\n", ""));
            HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fbo.tianxia.taobao.com%2F");

            strRequest = "http://bo.tianxia.taobao.com/taoboyuan/service/growSeed.jhtml?timeStamp=" + rndKey.GetRandStr(13) + "&id=" + strID;
            strContent = HTTPproc.OpenRead(strRequest);

            JObject o = JObject.Parse(Unicode2Character(strContent));
            string strStatus = Convert.ToString((int)o["status"]);  //״̬�ж�
            string strMsg = (string)o["msg"];                       //ϵͳ��Ϣ

            if (strStatus == "1")
            {
                ShowSysLog(strMsg);
                return "true";
            }
            else
            {
                ShowSysLog(strMsg);
                return "false";
            }
        }
        #endregion ��ֲ���� growSeeds(string strID, string strCookie)

        #region ����û��Ƿ���Ȩ CheckUser(string strUserNames)
        /// <summary>
        /// ����û��Ƿ���Ȩ
        /// </summary>
        /// <param name="strUserNames"></param>
        private void CheckUser(string strUserName)
        {
            bool boolStatus = false;
            string strEndTime = null;
            DateTime dtNowTime;
            string strDays = null;
            string strSQL = "SELECT TOP 1 [Status],[EndTime],GETDATE() AS DT FROM [BO_User] WHERE UserName = '" + strUserName + "'";

            DataRow dr = SqlHelper.ExecuteDataRow(SqlLibrary.GetBo_Main(), CommandType.Text, strSQL);
            
            if (dr == null)
            {
                string strSQL_TempUser = "INSERT INTO BO_User(UserName,EndTime) VALUES('" + strUserName + "',GETDATE()+30)";
                SqlHelper.ExecuteNonQuery(SqlLibrary.GetBo_Main(), CommandType.Text, strSQL_TempUser);

                MessageBox.Show("�����ʺ�" + strUserName + "�Ѿ��ɹ�������ԣ�ʱ��30�죬���µ�¼����ʹ�ã�");
                //MessageBox.Show("�����ʺ�δ���󶨣������ http://shop61556905.taobao.com ������Ȩ��лл��");
                Application.Exit();
            }
            else if(dr != null)
            {
                boolStatus = Convert.ToBoolean(dr["Status"]);
                strEndTime = Convert.ToDateTime(dr["EndTime"]).ToString();
                dtNowTime = Convert.ToDateTime(dr["DT"]);

                if (boolStatus == false)
                {
                    if (dtNowTime > Convert.ToDateTime(strEndTime))
                    {
                        MessageBox.Show("�����ʺ��ѵ��ڣ������ http://shop61556905.taobao.com ��Լ��Ȩ��лл��", "ϵͳ��Ϣ");
                        Application.Exit();
                    }
                    else
                    {
                        strSQL = "SELECT DATEDIFF(DAY,GETDATE(),'" + strEndTime + "') AS Days";
                        DataRow dr_EndTime = SqlHelper.ExecuteDataRow(SqlLibrary.GetBo_Main(), CommandType.Text, strSQL);
                        strDays = Convert.ToString(dr_EndTime["Days"]);
                        //this.DialogResult = MessageBox.Show("��½�ɹ����������" + strDays + "�쵽�ڡ�", "ϵͳ��Ϣ");
                        MessageBox.Show("��½�ɹ����������" + strDays + "�쵽�ڡ�", "ϵͳ��Ϣ");
                    }                    
                }
                else
                {
                    this.DialogResult = MessageBox.Show("�����ʺ��ѵ��ڣ������ http://shop61556905.taobao.com ��Լ��Ȩ��лл��", "ϵͳ��Ϣ");
                }
            }
        }

        #endregion ����û��Ƿ���Ȩ CheckUser(string strUserNames)

        #region ��ȡͨ�� Receive_Tongbao(string strCookie)
        /// <summary>
        /// ��ȡͨ��
        /// </summary>
        /// <param name="strCookie"></param>
        private void Receive_Tongbao(string strCookie)
        {
            bool boolCode = true;
            string strContent = null;
            string strRequest = null;

            ImgCode ImgCode_Form = new ImgCode();
            ImgCode_Form.strCookies = strCookie;
            ImgCode_Form.strRequest = "http://bo.tianxia.taobao.com/taoboyuan/seedstore.htm";

            while (boolCode)
            {
                DialogResult r = ImgCode_Form.ShowDialog();
                if (r == DialogResult.OK)
                {
                    strImgCode = ImgCode_Form.strImgCode;

                    HTTPproc.RequestHeaders.Add("Accept:text/html,application/xhtml+xml,application/xml;q=0.9;q=0.8");
                    HTTPproc.RequestHeaders.Add("Accept-Language:zh;q=0.5");
                    HTTPproc.RequestHeaders.Add("Accept-Encoding:gzip,deflate");
                    HTTPproc.RequestHeaders.Add("Accept-Charset:GB2312,utf-8;q=0.7,*;q=0.7");
                    HTTPproc.RequestHeaders.Add("Referer:http://bo.tianxia.taobao.com/taoboyuan/seedstore.htm");

                    strRequest = "http://bo.tianxia.taobao.com/taoboyuan/dayscore.htm";
                    strContent = HTTPproc.OpenRead(strRequest, "code=" + strImgCode + "");
                    int intA = strContent.IndexOf("{");
                    int intB = strContent.IndexOf("}");
                    strContent.Substring(intA, intB - intA + 1);

                    JObject oStatus = JObject.Parse(Unicode2Character(strContent));
                    int intData = Convert.ToInt32((string)oStatus["data"]);  //�ж���ȡ״̬
                    if (intData == 0)
                    {
                        ShowSysLog("ͨ����ȡ�ɹ�");
                        ReadUserInfo(strCookie);
                        boolCode = false;
                    }
                    else if (intData == 1)
                    {
                        ShowSysLog("�������Ѿ���ȡ��ͨ���ˣ��벻Ҫ�ظ���ȡ");
                        ReadUserInfo(strCookie);
                        boolCode = false;
                    }
                    else
                    {
                        ImgCode_Form.Dispose();
                        ImgCode_Form = new ImgCode();
                        ImgCode_Form.strCookies = HTTPproc.Cookie.ToString();
                        ImgCode_Form.strRequest = "http://bo.tianxia.taobao.com/taoboyuan/seedstore.htm";
                    }
                }
                else
                {
                    boolCode = false;
                    break;
                }
            }
        }

        #endregion ��ȡͨ�� Receive_Tongbao(string strCookie)

        #region ��ȡ���� ReadMyBag()
        /// <summary>
        /// ��ȡ����
        /// </summary>
        private void ReadMyBag()
        {
            string strContent = null;
            string strRequest = null;
            string resultString_0 = null;
            string resultString_1 = null;
            string resultString_2 = null;
            string resultString_3 = null;

            if (listView_MyBag.Items.Count > 0)
            {
                foreach (ListViewItem li in listView_MyBag.Items)
                {
                    if (listView_MyBag.Items.Count > 0)
                    {
                        li.Remove();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //����HTTP����Ĭ�ϱ���
            HTTPproc.Encoding = System.Text.Encoding.Default;
            HTTPproc.RequestHeaders.Add("Accept:text/html,application/xhtml+xml,application/xml;q=0.9;q=0.8");
            HTTPproc.RequestHeaders.Add("Accept-Language:zh;q=0.5");
            HTTPproc.RequestHeaders.Add("Accept-Encoding:gzip,deflate");
            HTTPproc.RequestHeaders.Add("Accept-Charset:GB2312,utf-8;q=0.7,*;q=0.7");
            HTTPproc.RequestHeaders.Add("Referer:http://bo.tianxia.taobao.com/taoboyuan/mybag/list.jhtml");

            strRequest = "http://bo.tianxia.taobao.com/taoboyuan/mybag/list.jhtml";
            strContent = HTTPproc.OpenRead(strRequest);

            Regex regexObj_0 = new Regex("����ʱ��.*</div>");
            Regex regexObj_1 = new Regex("����ʱ��.*<br>");
            Regex regexObj_2 = new Regex("font title=\".*\"");
            Regex regexObj_3 = new Regex(@"value=""\d{0,}""");

            Match matchResults_0 = regexObj_0.Match(strContent);
            Match matchResults_1 = regexObj_1.Match(strContent);
            Match matchResults_2 = regexObj_2.Match(strContent);
            Match matchResults_3 = regexObj_3.Match(strContent);

            while (matchResults_0.Success)
            {
                try
                {
                    resultString_0 = matchResults_0.Value.ToString().Replace("����ʱ�䣺","").Replace("</div>","");
                    resultString_1 = matchResults_1.Value.ToString().Replace("����ʱ��:","").Replace("<br>","");
                    resultString_2 = matchResults_2.Value.ToString().Replace("font title=\"","").Replace("\"","");
                    resultString_3 = matchResults_3.Value.ToString().Replace("value=\"", "").Replace("\"", "");

                    ListViewItem li = new ListViewItem();
                    li.SubItems.Clear();
                    li.SubItems[0].Text = resultString_3;
                    li.SubItems.Add(resultString_2);
                    li.SubItems.Add(resultString_0);
                    li.SubItems.Add(resultString_1);
                    listView_MyBag.Items.Add(li);

                    matchResults_0 = matchResults_0.NextMatch();
                    matchResults_1 = matchResults_1.NextMatch();
                    matchResults_2 = matchResults_2.NextMatch();
                    matchResults_3 = matchResults_3.NextMatch();
                }
                catch
                {
 
                }
            }
        }

        #endregion ��ȡ���� ReadMyBag()
        
        #region ��ť�¼�
        private void button_ReGrowseed_Click(object sender, EventArgs e)
        {
            getMySeeds(strCookies);
        }

        private void button_Receive_Tongbao_Click(object sender, EventArgs e)
        {
            Receive_Tongbao(strCookies);
        }

        private void button_ReadMybag_Click(object sender, EventArgs e)
        {
            ReadMyBag();
        }

        #endregion ��ť�¼�

        private void button_LoadUser_Click(object sender, EventArgs e)
        {
            openFileDialog_LoadUser.ShowDialog();
        }

        private void openFileDialog_LoadUser_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string strPath = Path.GetFullPath(openFileDialog_LoadUser.FileName);
            BatchLoadUser(strPath);
        }

        #region �����û����ݵ��� BatchLoadUser(string strPath)
        /// <summary>
        /// �����û����ݵ���
        /// </summary>
        /// <param name="strPath"></param>
        private void BatchLoadUser(string strPath)
        {
            string[] arrInfo;
            int i = 1;

            if (listView_BatchLogin.Items.Count > 0)
            {
                foreach (ListViewItem li in listView_BatchLogin.Items)
                {
                    if (listView_BatchLogin.Items.Count > 0)
                    {
                        li.Remove();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //C#��ȡTXT�ļ�֮���� FileStream �Ķ���,˵���˸��߳���,  
            //�ļ�������,���ļ���δ���,���ļ����ݲ�ȡ�Ĵ���ʽ  
            FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read);
            //�� ���ı� ���� ��д����
            StreamReader sr = new StreamReader(fs);
            //��λ������,begin ��һ���ο���
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            //��һ�£������ļ�����û�����ݣ�Ϊ��һ��ѭ�� �ṩ�ж�����
            //sr.ReadLine() ������ StreamReader�ķ��� �ɲ��� console �е�~
            string strInfo = sr.ReadLine();

            //��� �ļ�������
            while (strInfo != null)
            {                
                arrInfo = strInfo.Split('|');
                ListViewItem li = new ListViewItem();
                li.SubItems.Clear();
                li.SubItems[0].Text = i.ToString();
                li.SubItems.Add(arrInfo[0].ToString().Trim());
                li.SubItems.Add(arrInfo[1].ToString().Trim());
                listView_BatchLogin.Items.Add(li);
                strInfo = sr.ReadLine();//�ж���һ���ж��Ƿ�Ϊ��
                i++;
            }
            //C#��ȡTXT�ļ�֮�ر��ļ���ע��˳���ȶ��ļ��ڲ����йرգ�Ȼ������ļ�~
            sr.Close();
            fs.Close();
        }
        #endregion �����û����ݵ��� BatchLoadUser(string strPath)

        private void listView_BatchLogin_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem li in listView_BatchLogin.SelectedItems)
            {
                if (listView_BatchLogin.SelectedIndices.Count > 0)
                {
                    strCookies = null;
                    HTTPproc = new WebClient();
                    strUserNames = li.SubItems[1].Text;
                    strPasswords = li.SubItems[2].Text;
                    TB_Login_Handle();
                }
                else
                {
                    break;
                }
            }            
        }
    }
}