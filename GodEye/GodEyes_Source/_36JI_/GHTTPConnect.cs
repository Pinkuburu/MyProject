namespace _36JI_
{
    using Jayrock.Json;
    using Jayrock.Json.Conversion;
    using ManagedZLib;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Net.Sockets;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    internal class GHTTPConnect
    {
        private CookieContainer _CookieContainer = new CookieContainer();
        private string _ErrorCode;
        private string _LoginCode;
        private string _Session;
        private string _strName;
        private string _strPassWord;
        private string _strServer;
        private string _strValidateCode;
        private static string s_Dic = "LfCgMwPZHhAJnj2tR4X5lWszxGeo31E7mYBqi80pUvDdF9rVyTQa6bKcONSkuI";

        public GHTTPConnect()
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(GHTTPConnect.ValidateServerCertificate);
        }

        private string EncryptSession(bool bEncrypt)
        {
            string str = "";
            if (bEncrypt)
            {
                Random random = new Random();
                int num2 = random.Next(0, 0xff);
                int length = this._Session.Length;
                for (int i = 0; i < length; i++)
                {
                    char ch = this._Session[i];
                    int num = (s_Dic.IndexOf(ch) + num2) % 0x3e;
                    str = str + s_Dic[num];
                }
                return (str + string.Format("{0:x2}{1:x2}{2:x2}{3:x2}", new object[] { random.Next(0, 0x100), random.Next(0, 0x100), random.Next(0, 0x100), num2 }));
            }
            return this._Session;
        }

        public string GetHTTPData(string strformData, bool bNeedResponse)
        {
            string text = null;
            string requestUriString = "http://" + this.Server + "/ultima.do";
            string s = strformData.Clone().ToString() + "&session=" + this.EncryptSession(true);
            byte[] bytes = new ASCIIEncoding().GetBytes(s);
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                request.Referer = "http://df.36ji.uuzu.com/UltimaMain.swf";
                request.Headers.Add("x-flash-version", "10,0,32,18");
                request.CookieContainer = this._CookieContainer;
                request.AllowAutoRedirect = false;
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    goto Label_029D;
                }
                this._ErrorCode = "SUCCESS";
                if (response.Cookies.Count > 0)
                {
                    this._CookieContainer.Add(response.Cookies);
                }
                if (!bNeedResponse)
                {
                    goto Label_029D;
                }
                Stream responseStream = response.GetResponseStream();
                if (response.ContentType.IndexOf("bin") != -1)
                {
                    ManagedZLib.Initialize();
                    CompressionStream stream = new CompressionStream(responseStream, CompressionOptions.Decompress);
                    try
                    {
                        text = new StreamReader(stream, Encoding.GetEncoding("UTF-8")).ReadToEnd();
                        text = text.Replace(@"\n", "\n");
                        text = text.Replace("\\\"", "\"");
                        text = text.Replace("\"{", "{");
                        text = text.Replace("}\"", "}");
                        goto Label_01E8;
                    }
                    catch (ZLibException)
                    {
                        goto Label_01E8;
                    }
                    finally
                    {
                        stream.Close();
                        ManagedZLib.Terminate();
                    }
                }
                text = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")).ReadToEnd();
            Label_01E8:
                responseStream.Close();
                if (text != null)
                {
                    JsonObject obj2 = (JsonObject) JsonConvert.Import(text);
                    if (obj2 != null)
                    {
                        obj2 = obj2["body"] as JsonObject;
                        if ((obj2 != null) && (obj2["errcode"] != null))
                        {
                            this._ErrorCode = obj2["errcode"].ToString();
                            if ((string.Compare(this._ErrorCode, "ERR_SESSION_000001", true) == 0) || (string.Compare(this._ErrorCode, "ERR_MAIN_000007", true) == 0))
                            {
                                if (this.IsExpired())
                                {
                                    this._ErrorCode = "USER_EXPIRED";
                                }
                                else
                                {
                                    this.GetSession();
                                    text = this.GetHTTPData(strformData, bNeedResponse);
                                }
                            }
                        }
                    }
                }
                else
                {
                    this._ErrorCode = "NET_ERROR";
                }
            Label_029D:
                response.Close();
            }
            catch (WebException)
            {
            }
            return text;
        }

        private bool GetLoginCode(string StrUrl)
        {
            bool flag = false;
            string requestUriString = null;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(StrUrl);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
            request.CookieContainer = this._CookieContainer;
            request.AllowAutoRedirect = false;
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            if (response.StatusCode == HttpStatusCode.Found)
            {
                if (response.Cookies.Count > 0)
                {
                    this._CookieContainer.Add(response.Cookies);
                }
                requestUriString = response.Headers.Get("Location").Trim();
                string[] strArray = requestUriString.Split(new string[] { "logincode=" }, StringSplitOptions.RemoveEmptyEntries);
                if (strArray.Length > 0)
                {
                    this._LoginCode = strArray[1];
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            response.Close();
            if (flag)
            {
                request = (HttpWebRequest) WebRequest.Create(requestUriString);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                request.CookieContainer = this._CookieContainer;
                request.AllowAutoRedirect = false;
                response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.Cookies.Count > 0)
                    {
                        this._CookieContainer.Add(response.Cookies);
                    }
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                response.Close();
            }
            return flag;
        }

        private bool GetSession()
        {
            Random random = new Random();
            string str = random.Next(0, 0x10).ToString("x");
            for (int i = 0; i < 11; i++)
            {
                str = str + random.Next(0, 0x10).ToString("x");
            }
            bool flag = false;
            string requestUriString = "http://" + this.Server + "/ultima.do";
            string s = "identify=" + str + "&logincode=" + this._LoginCode + "&action=Main%2Elogin";
            byte[] bytes = new ASCIIEncoding().GetBytes(s);
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                request.Referer = "http://df.36ji.uuzu.com/Portal.swf";
                request.Headers.Add("x-flash-version", "10,0,32,18");
                request.CookieContainer = this._CookieContainer;
                request.AllowAutoRedirect = false;
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.Cookies.Count > 0)
                    {
                        this._CookieContainer.Add(response.Cookies);
                    }
                    Stream responseStream = response.GetResponseStream();
                    JsonObject obj2 = (JsonObject) JsonConvert.Import(new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")).ReadToEnd());
                    obj2 = obj2["body"] as JsonObject;
                    if (obj2["session"] != null)
                    {
                        this._Session = obj2["session"].ToString();
                        flag = true;
                    }
                    responseStream.Close();
                }
                response.Close();
            }
            catch (WebException)
            {
                flag = false;
            }
            return flag;
        }

        public Image GetValidateCodeStream(string strServer)
        {
            Image image = null;
            if (strServer.IndexOf("xunlei.com") != -1)
            {
                Random random = new Random();
                string str = string.Format("{0}000", random.Next(100, 0x3e7));
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://verify.xunlei.com/image?" + str);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "*/*";
                request.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
                request.Referer = "http://36ji.xunlei.com/";
                request.CookieContainer = this._CookieContainer;
                request.AllowAutoRedirect = false;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.Cookies.Count > 0)
                    {
                        this._CookieContainer.Add(response.Cookies);
                    }
                    Stream responseStream = response.GetResponseStream();
                    image = Image.FromStream(responseStream);
                    responseStream.Close();
                }
                response.Close();
                return image;
            }
            if (strServer.IndexOf("baofeng.com") == -1)
            {
                strServer.IndexOf("game518.com");
            }
            return image;
        }

        public void Init(string Name, string PassWord, string Server, string ValidateCode)
        {
            this._strName = Name;
            this._strPassWord = PassWord;
            this._strServer = Server;
            this._strValidateCode = ValidateCode;
        }

        private bool IsExpired()
        {
            string s = "1";
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint remoteEP = new IPEndPoint(new IPAddress(new byte[] { 0xde, 0xd3, 0x5d, 0x80 }), 0x3f3);
                socket.Connect(remoteEP);
                string str2 = string.Format("godeyes-isexpired:{0}", this.Name);
                byte[] bytes = Encoding.UTF8.GetBytes(str2);
                socket.Send(bytes);
                byte[] buffer = new byte[0x3e8];
                int count = socket.Receive(buffer);
                s = Encoding.UTF8.GetString(buffer, 0, count);
                str2 = "godeyes-close";
                bytes = Encoding.UTF8.GetBytes(str2);
                socket.Send(bytes);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (SocketException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
            return (int.Parse(s) != 0);
        }

        public bool Login()
        {
            if (this._strServer.IndexOf("17173.com") != -1)
            {
                return this.Login17173();
            }
            if (this._strServer.IndexOf("xunlei.com") != -1)
            {
                return this.LoginXunLei();
            }
            if (this._strServer.IndexOf("youxi.com") != -1)
            {
                return this.LoginYouXi();
            }
            bool loginCode = false;
            string requestUriString = "https://passport.uuzu.com/login.php";
            string s = "act=login&username=" + this.Name + "&password=" + this.PassWord;
            byte[] bytes = new ASCIIEncoding().GetBytes(s);
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                request.Referer = "http://36ji.uuzu.com/";
                request.CookieContainer = this._CookieContainer;
                request.AllowAutoRedirect = false;
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.Cookies.Count > 0)
                    {
                        this._CookieContainer.Add(response.Cookies);
                    }
                    requestUriString = "https://passport.uuzu.com/36ji/activation_test.php?drefer=" + this.Server;
                    request = (HttpWebRequest) WebRequest.Create(requestUriString);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                    request.Referer = "http://36ji.uuzu.com/";
                    request.CookieContainer = this._CookieContainer;
                    request.AllowAutoRedirect = false;
                    HttpWebResponse response2 = (HttpWebResponse) request.GetResponse();
                    if (response2.StatusCode == HttpStatusCode.OK)
                    {
                        if (response2.Cookies.Count > 0)
                        {
                            this._CookieContainer.Add(response2.Cookies);
                        }
                        Stream responseStream = response2.GetResponseStream();
                        string[] strArray = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")).ReadToEnd().Split(new string[] { "url=", "'>" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strArray.Length > 0)
                        {
                            requestUriString = strArray[1];
                            loginCode = true;
                        }
                        else
                        {
                            loginCode = false;
                        }
                        responseStream.Close();
                    }
                    response2.Close();
                    if (loginCode)
                    {
                        loginCode = this.GetLoginCode(requestUriString);
                    }
                }
                response.Close();
            }
            catch (Exception)
            {
                loginCode = false;
            }
            if (loginCode)
            {
                loginCode = this.GetSession();
            }
            return loginCode;
        }

        private bool Login17173()
        {
            bool loginCode = false;
            string[] strArray = this._strServer.Split(new char[] { ',' });
            this._strServer = strArray[0];
            string str = strArray[1];
            string str2 = this.Name.Replace("@", "%40");
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(this.PassWord));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x2"));
            }
            string str3 = builder.ToString();
            string requestUriString = "http://passport.sohu.com/sso/login.jsp?userid=" + str2 + "&password=" + str3 + "&appid=1029&persistentcookie=0&s=1255515885375&b=1&w=1280&pwdtype=1&domain=17173.com";
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                request.Referer = "http://mygame.17173.com/36ji/";
                request.CookieContainer = this._CookieContainer;
                request.AllowAutoRedirect = false;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode == HttpStatusCode.Found)
                {
                    if (response.Cookies.Count > 0)
                    {
                        this._CookieContainer.Add(response.Cookies);
                    }
                    requestUriString = response.Headers.Get("Location").Trim();
                    request = (HttpWebRequest) WebRequest.Create(requestUriString);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                    request.Referer = "http://mygame.17173.com/36ji/";
                    request.CookieContainer = this._CookieContainer;
                    request.AllowAutoRedirect = false;
                    HttpWebResponse response2 = (HttpWebResponse) request.GetResponse();
                    if (response2.StatusCode == HttpStatusCode.OK)
                    {
                        if (response2.Cookies.Count > 0)
                        {
                            this._CookieContainer.Add(response2.Cookies);
                        }
                        loginCode = true;
                    }
                    else
                    {
                        loginCode = false;
                    }
                    response2.Close();
                    if (loginCode)
                    {
                        requestUriString = "http://mygame.17173.com/port/startgame.php?gamekind=20022&area=" + str;
                        request = (HttpWebRequest) WebRequest.Create(requestUriString);
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                        request.Referer = "http://mygame.17173.com/36ji/";
                        request.CookieContainer = this._CookieContainer;
                        request.AllowAutoRedirect = false;
                        response2 = (HttpWebResponse) request.GetResponse();
                        if (response2.StatusCode == HttpStatusCode.Found)
                        {
                            if (response2.Cookies.Count > 0)
                            {
                                this._CookieContainer.Add(response2.Cookies);
                            }
                            requestUriString = response2.Headers.Get("Location").Trim();
                            loginCode = true;
                        }
                        else
                        {
                            loginCode = false;
                        }
                        response2.Close();
                    }
                    if (loginCode)
                    {
                        request = (HttpWebRequest) WebRequest.Create(requestUriString);
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                        request.CookieContainer = this._CookieContainer;
                        request.AllowAutoRedirect = false;
                        response2 = (HttpWebResponse) request.GetResponse();
                        if (response2.StatusCode == HttpStatusCode.OK)
                        {
                            if (response2.Cookies.Count > 0)
                            {
                                this._CookieContainer.Add(response2.Cookies);
                            }
                            Stream responseStream = response2.GetResponseStream();
                            string[] strArray2 = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")).ReadToEnd().Split(new string[] { "url=", "'>" }, StringSplitOptions.RemoveEmptyEntries);
                            if (strArray2.Length > 0)
                            {
                                requestUriString = strArray2[1];
                                loginCode = true;
                            }
                            else
                            {
                                loginCode = false;
                            }
                            responseStream.Close();
                        }
                        response2.Close();
                    }
                    if (loginCode)
                    {
                        loginCode = this.GetLoginCode(requestUriString);
                    }
                }
                response.Close();
            }
            catch (Exception)
            {
                loginCode = false;
            }
            if (loginCode)
            {
                loginCode = this.GetSession();
            }
            return loginCode;
        }

        private bool LoginXunLei()
        {
            bool loginCode = false;
            string[] strArray = this._strServer.Split(new char[] { ',' });
            this._strServer = strArray[0];
            string str = strArray[1];
            string requestUriString = "http://svr.game.xunlei.com/login.webGameLogin?rtnName=scriptDataRtn&username=" + this._strName + "&password=" + this._strPassWord + "&vcodestr=" + this._strValidateCode;
            string str3 = this._CookieContainer.GetCookies(new Uri("http://www.xunlei.com/"))["VERIFY_KEY"].Value;
            requestUriString = requestUriString + "&vcode=" + str3 + "&Random=y&gameid=000031";
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "*/*";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                request.Referer = "http://36ji.xunlei.com/";
                request.CookieContainer = this._CookieContainer;
                request.AllowAutoRedirect = false;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.Cookies.Count > 0)
                    {
                        this._CookieContainer.Add(response.Cookies);
                    }
                    requestUriString = "http://web.stat.xunlei.com/pv?&sd=1259327129637&vb=1258683561216&vd=2&dm=xunlei.com&ul=https%3A%2F%2Fpassport.uuzu.com%2Fapi%2F36ji%2Fuinterface.php%3Fserverid%3D";
                    request = (HttpWebRequest) WebRequest.Create(requestUriString + str + "&vc=4&st=1258683561216&co=32&jv=NA&fv=10.0&ru=128001024&os=WinXP&br=MSIE%2F7.0&ln=en-us&zn=-8&al=0&tt=%E8%BF%85%E9%9B%B7%E4%B8%89%E5%8D%81%E5%85%AD%E8%AE%A1%E5%AE%98%E6%96%B9%E7%BD%91%E7%AB%99%20-%20%E7%BD%91%E9%A1%B5%E6%B8%B8%E6%88%8F%20%E6%97%A0%E9%9C%80%E4%B8%8B%E8%BD%BD%20%E6%84%9F%E5%8F%97%E5%8D%81%E4%BA%8C%E5%9B%BD%E4%BA%89%E9%9C%B8%E5%B8%A6%E7%BB%99%E4%BD%A0%E7%9A%84%E5%BC%BA%E7%83%88%E9%9C%87%E6%92%BC&rf=http%3A%2F%2F36ji.xunlei.com%2F&lul=http%3A%2F%2F36ji.xunlei.com%2F&pi=001731E16732RLR4&clp=&_a=&gs=GSID_001_001_001_037&1258683574000&1258683584000");
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Accept = "*/*";
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                    request.Referer = "http://36ji.xunlei.com/";
                    request.CookieContainer = this._CookieContainer;
                    request.AllowAutoRedirect = false;
                    HttpWebResponse response2 = (HttpWebResponse) request.GetResponse();
                    if (response2.StatusCode == HttpStatusCode.OK)
                    {
                        if (response2.Cookies.Count > 0)
                        {
                            this._CookieContainer.Add(response2.Cookies);
                        }
                        loginCode = true;
                    }
                    else
                    {
                        loginCode = false;
                    }
                    response2.Close();
                    requestUriString = ("http://svr.game.xunlei.com/loginWithServerid.webGameLogin?rtnName=scriptDataRtn&username=" + this._strName + "&serverid=" + str) + "&loginurl=https%3A%2F%2Fpassport.uuzu.com%2Fapi%2F36ji%2Fuinterface.php&Random=rnmbjq&gameid=000031";
                    request = (HttpWebRequest) WebRequest.Create(requestUriString);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Accept = "*/*";
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                    request.Referer = "http://36ji.xunlei.com/";
                    request.CookieContainer = this._CookieContainer;
                    request.AllowAutoRedirect = false;
                    response2 = (HttpWebResponse) request.GetResponse();
                    if (response2.StatusCode == HttpStatusCode.OK)
                    {
                        if (response2.Cookies.Count > 0)
                        {
                            this._CookieContainer.Add(response2.Cookies);
                        }
                        Stream responseStream = response2.GetResponseStream();
                        string[] strArray2 = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")).ReadToEnd().Split(new string[] { "gameLoginURL\":\"", "\"}" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strArray2.Length > 0)
                        {
                            requestUriString = strArray2[1];
                            loginCode = true;
                        }
                        else
                        {
                            loginCode = false;
                        }
                        responseStream.Close();
                    }
                    else
                    {
                        loginCode = false;
                    }
                    response2.Close();
                    if (loginCode)
                    {
                        loginCode = this.GetLoginCode(requestUriString);
                    }
                }
                response.Close();
            }
            catch (Exception)
            {
                loginCode = false;
            }
            if (loginCode)
            {
                loginCode = this.GetSession();
            }
            return loginCode;
        }

        private bool LoginYouXi()
        {
            bool loginCode = false;
            string requestUriString = "http://www.youxi.com/AjaxLogin.aspx";
            string s = "username=" + this.Name + "&pwd=" + this.PassWord + "&chk_login=false";
            byte[] bytes = new ASCIIEncoding().GetBytes(s);
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "*/*";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                request.Referer = "http://www.youxi.com/game_login.aspx?gameid=28&Domain=" + this.Server;
                request.Headers.Add("x-requested-with", "XMLHttpRequest");
                request.CookieContainer = this._CookieContainer;
                request.AllowAutoRedirect = false;
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.Cookies.Count > 0)
                    {
                        this._CookieContainer.Add(response.Cookies);
                    }
                    requestUriString = "http://www.youxi.com/logins/go.aspx?Domain=" + this.Server;
                    request = (HttpWebRequest) WebRequest.Create(requestUriString);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Accept = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                    request.CookieContainer = this._CookieContainer;
                    request.AllowAutoRedirect = false;
                    HttpWebResponse response2 = (HttpWebResponse) request.GetResponse();
                    if (response2.StatusCode == HttpStatusCode.Found)
                    {
                        if (response2.Cookies.Count > 0)
                        {
                            this._CookieContainer.Add(response2.Cookies);
                        }
                        requestUriString = response2.Headers.Get("Location").Trim();
                        loginCode = true;
                    }
                    else
                    {
                        loginCode = false;
                    }
                    response2.Close();
                    request = (HttpWebRequest) WebRequest.Create(requestUriString);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Accept = "*/*";
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1))";
                    request.CookieContainer = this._CookieContainer;
                    request.AllowAutoRedirect = false;
                    response2 = (HttpWebResponse) request.GetResponse();
                    if (response2.StatusCode == HttpStatusCode.OK)
                    {
                        if (response2.Cookies.Count > 0)
                        {
                            this._CookieContainer.Add(response2.Cookies);
                        }
                        Stream responseStream = response2.GetResponseStream();
                        string[] strArray = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")).ReadToEnd().Split(new string[] { "url=", "'>" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strArray.Length > 0)
                        {
                            requestUriString = strArray[1];
                            loginCode = true;
                        }
                        else
                        {
                            loginCode = false;
                        }
                        responseStream.Close();
                    }
                    else
                    {
                        loginCode = false;
                    }
                    response2.Close();
                    if (loginCode)
                    {
                        loginCode = this.GetLoginCode(requestUriString);
                    }
                }
                response.Close();
            }
            catch (Exception)
            {
                loginCode = false;
            }
            if (loginCode)
            {
                loginCode = this.GetSession();
            }
            return loginCode;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public string ErrorCode
        {
            get
            {
                return this._ErrorCode;
            }
        }

        public string Name
        {
            get
            {
                return this._strName;
            }
        }

        public string PassWord
        {
            get
            {
                return this._strPassWord;
            }
        }

        public string Server
        {
            get
            {
                return this._strServer;
            }
        }

        public string Session
        {
            get
            {
                return this._Session;
            }
        }
    }
}

