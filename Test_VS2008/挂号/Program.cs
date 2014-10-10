using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Threading;

namespace 挂号
{
    /// <summary>
    /// 计算程序的运行时间
    /// </summary>
    class StopWatch
    {
        private int mintStart;
        public void start()
        {
            mintStart = Environment.TickCount;
        }
        public long elapsed()
        {
            return Environment.TickCount - mintStart;
        }
    }

    class Program
    {
        public static bool _isLogin = false;
        public static string _strUserID = "";
        public static string strDomain = "http://guahao.qingdaonews.com";
        public static WebClient HTTPproc = new WebClient();
        private static IniFile ini = new IniFile(@"Option.ini");

        static void Main(string[] args)
        {
            string strUserName = null;
            string strPassword = null;
            string strDoctorID = null;
            string strTime = null;
            string strReserveTime = null;
            string strHospitalID = null;
            StopWatch sw = new StopWatch();

            sw.start();
            ImageCode();
            SysLog("加载验证码用时 " + sw.elapsed() + "ms");

            //登陆部分
            if (File.Exists(ini.Path))
            {
                //读取配置文件查询是否有过登录记录，自动填表。
                string strINIuserName = ini.GetSectionValues("UserInfo")["UserName"];
                Console.Write("U:(默认 " + strINIuserName + " )");
                strUserName = Console.ReadLine();
                if (strUserName == "")
                {
                    strUserName = strINIuserName;
                }

                string strINIPassword = ini.GetSectionValues("UserInfo")["Password"];
                Console.Write("P:(默认 " + strINIPassword + " )");
                strPassword = Console.ReadLine();
                if (strPassword == "")
                {
                    strPassword = strINIPassword;
                }
            }
            else
            {
                Console.Write("用户名:");
                strUserName = Console.ReadLine();
                Console.Write("密码:");
                strPassword = Console.ReadLine();
            }

            //验证码
            Console.Write("验证码:");
            string strCode = Console.ReadLine();
            //医院ID
            Console.Write("医院ID:");
            strHospitalID = Console.ReadLine();
            //医生ID
            Console.Write("医生ID:");
            strDoctorID = Console.ReadLine();
            //问诊时间
            Console.Write("问诊时间 1.上午 2.下午:");
            strTime = Console.ReadLine();
            //预约时间
            Console.Write("预约时间:");
            strReserveTime = Console.ReadLine();

            //============= Step ================
            //性别
            Console.Write("性别: 0.女 1.男:");
            string strGender = Console.ReadLine();
            //===================================

            _isLogin = Login(strUserName, strPassword, strCode);

            if (_isLogin)
            {
                _strUserID = FindUserID();
                //Console.WriteLine(_strUserID);
                bool blLoop = true;
                while (blLoop)
                {
                    string strLink = FindReserveLink(strHospitalID, strDoctorID, strReserveTime, strTime);
                    if (strLink != "")
                    {
                        blLoop = false;
                        //开始走预约流程
                        SysLog("开始预约流程");

                        sw.start();

                        string strContent = HTTPproc.OpenRead(strLink);
                        string strLocation = HTTPproc.ResponseHeaders["Location"];
                        if (strLocation == "/Order/Step.html")
                        {
                            SysLog("进入流程提交。");
                            GoStep(_strUserID, strGender);
                            SysLog("提交用时 " + sw.elapsed() + "ms");
                        }
                    }
                    else
                    {
                        SysLog("预约还没开始");
                    }
                    Thread.Sleep(1000);
                }
            }

            Console.ReadKey();
        }

        #region 开始流程 GoStep(string strUserID, string strGender)
        /// <summary>
        /// 开始流程
        /// </summary>
        /// <param name="strUserID"></param>
        /// <param name="strGender"></param>
        private static void GoStep(string strUserID, string strGender)
        {
            string strRequest = "http://guahao.qingdaonews.com/Order/Step.html";
            string strParameter = "";

            if (strGender == "0")
            {
                strParameter = "orduser=" + strUserID + "&user_realname=&friend=0&user_pass=&ccode=&user_sex=%C5%AE&ord_chufu=0&known=1";
            }
            else
            {
                strParameter = "orduser=" + strUserID + "&user_realname=&friend=0&user_pass=&ccode=&user_sex=%C4%D0&ord_chufu=0&known=1";
            }

            string strContent = HTTPproc.OpenRead(strRequest, strParameter);
            string strLocation = HTTPproc.ResponseHeaders["Location"];

            if (strLocation == "/Order/Steps.html")
            {
                SysLog("第一步");
                strRequest = "http://guahao.qingdaonews.com/Order/Steps.html";
                strParameter = "ext_see=0&ext_dis_id=0&ext_dis_name=&ext_dis_yes=0&ext_symptom=ff";
                strContent = HTTPproc.OpenRead(strRequest, strParameter);
                strLocation = HTTPproc.ResponseHeaders["Location"];
                if (strLocation == "/Order/Stepc.html")
                {
                    SysLog("第二步");
                    strRequest = "http://guahao.qingdaonews.com/Order/Stepc.html";
                    strContent = HTTPproc.OpenRead(strRequest);

                    string resultString = null;
                    try
                    {
                        resultString = Regex.Match(strContent, @"tid' value='\d{1,2}").Value.Replace("tid' value='", "");
                        if(resultString != "")
                        {
                            SysLog("第三步");
                            strRequest = "http://guahao.qingdaonews.com/Order/order.html";
                            strParameter = "tid=" + resultString;
                            strContent = HTTPproc.OpenRead(strRequest, strParameter);
                            if (strContent.IndexOf("预约成功") > 0)
                            {
                                strRequest = "http://guahao.qingdaonews.com/Order/send";
                                strParameter = "m=1433&n=2046";
                                strContent = HTTPproc.OpenRead(strRequest, strParameter);
                                Console.WriteLine(strContent);
                                switch (strContent.Replace("\r\n", "").Trim())
                                {
                                    case "1":
                                        SysLog("此预约已过期,不能再发送短信!");
                                        break;
                                    case "2":
                                        SysLog("发送次数超过了系统设置!");
                                        break;
                                    case "3":
                                        SysLog("此次发送失败，如需要重新发送请进入预约记录详情发送!");
                                        break;
                                    case "4":
                                        SysLog("短信发送成功!");
                                        break;
                                    default:
                                        SysLog("发送失败，请检查你的登录状态及登录账号!");
                                        break;
                                }
                            }
                        }                        
                    }
                    catch (ArgumentException ex)
                    {
                        SysLog("哎，预约都被人抢光了。");
                        // Syntax error in the regular expression
                    }
                }
            }
        }
        #endregion

        #region 查询是否开始预约 FindReserveLink(string strHospitalID, string strDoctorID, string strReserveTime, string strTime)
        /// <summary>
        /// 查询是否开始预约
        /// </summary>
        /// <param name="strHospitalID"></param>
        /// <param name="strDoctorID"></param>
        /// <param name="strReserveTime"></param>
        /// <param name="strTime"></param>
        /// <returns></returns>
        private static string FindReserveLink(string strHospitalID, string strDoctorID, string strReserveTime, string strTime)
        {            
            string strRequest = "http://guahao.qingdaonews.com/YyYisheng/" + strDoctorID + ".html";
            string strContent = HTTPproc.OpenRead(strRequest);
            string strFindLink = "/Order/orderdoc/yyid/" + strHospitalID + "/ysid/" + strDoctorID + "/index/" + strReserveTime + "/day/" + strTime + ".html";
            // /Order/orderdoc/yyid/1001/ysid/77/index/5/day/1.html

            if (strContent.IndexOf(strFindLink) > 0)
            {
                return strDomain + strFindLink;
            }

            return "";

            //查询预约链接
            //string resultString = null;
            //try
            //{
            //    resultString = Regex.Match(strContent, @"/Order/orderdoc/yyid/1001/ysid/77/index/4/day/1\.html.*'>").Value;
            //}
            //catch (ArgumentException ex)
            //{
            //    // Syntax error in the regular expression
            //}
        }
        #endregion

        #region 预约时间设置暂时无用
        /// <summary>
        /// 预约时间设置暂时无用
        /// </summary>
        /// <param name="strDoctorID"></param>
        /// <returns></returns>
        private static string ReserveTime(string strDoctorID)
        {
            string strRequest = "http://guahao.qingdaonews.com/YyYisheng/" + strDoctorID + ".html";
            string strContent = HTTPproc.OpenRead(strRequest);

            strContent = "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n"+
                        "<thead>\r\n"+
                        "<tr>\r\n"+
                        "<th width=\"60\"></a></th>\r\n"+
                        "<th>07/18<br />星期五</th>\r\n"+
                        "<th>07/19<br />星期六</th>\r\n"+
                        "<th>07/20<br />星期天</th>\r\n"+
                        "<th>07/21<br />星期一</th>\r\n"+
                        "<th>07/22<br />星期二</th>\r\n"+
                        "<th>07/23<br />星期三</th>\r\n"+
                        "<th>07/24<br />星期四</th>\r\n"+
                        "<th width=\"60\"></a></th>\r\n"+
                        "</tr></thead>\r\n"+
                        "<tbody>\r\n"+
                        "<tr><th>上午</th>\r\n"+
                        "<td class=\"hover\">&nbsp;</td>\r\n"+
                        "<td >&nbsp;</td>\r\n"+
                        "<td class=\"hover\">&nbsp;</td>\r\n"+
                        "<td >&nbsp;</td>\r\n"+
                        "<td class=\"hover\">&nbsp;</td>\r\n"+
                        "<td >&nbsp;</td>\r\n"+
                        "<td class=\"hover\"><span class='cGray'><b>待放号<br>0/15</b></span></td>\r\n"+
                        "<th>&nbsp;</th></tr>\r\n"+
                        "<tr class='hover'><th>下午</th>\r\n"+
                        "<td class=\"hover\">&nbsp;</td>\r\n"+
                        "<td >&nbsp;</td>\r\n"+
                        "<td class=\"hover\">&nbsp;</td>\r\n"+
                        "<td >&nbsp;</td>\r\n"+
                        "<td class=\"hover\">&nbsp;</td>\r\n"+
                        "<td >&nbsp;</td>\r\n"+
                        "<td class=\"hover\"><span class='cGray'><b>待放号<br>0/12</b></span></td>\r\n"+
                        "<th>&nbsp;</th></tr>\r\n"+
                        "</tbody>\r\n"+
                        "<tfoot>\r\n"+
                        "<tr><td colspan=\"9\"><strong class=\"mb cRed z\">←您可以尝试向左滑动</strong> 号约满了怎么办？您可以查看<a href=\"/YyKeshi/view/yyid/1001/ksid/37.html\" class=\"cDGreen\">其它同科室医生</a></td></tr>\r\n"+
                        "</tfoot>\r\n"+
                        "</table>\r\n";
            ArrayList alTemp = new ArrayList();
            string aaa = null;

            try
            {
                Regex regexObj = new Regex("<td.*</td>");
                Match matchResults = regexObj.Match(strContent);
                while (matchResults.Success)
                {
                    // matched text: matchResults.Value
                    // match start: matchResults.Index
                    // match length: matchResults.Length
                    alTemp.Add(matchResults.Value);
                    matchResults = matchResults.NextMatch();                    
                }
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            if (alTemp.Count == 15)
            {
                alTemp.RemoveAt(14);
                int i = 1;
                foreach (string alContent in alTemp)
                {
                    if (alContent.IndexOf("待放号") > 0)
                    {
                        Console.WriteLine(i + "|" + alContent);                        
                    }
                    i++;
                }
                i = 1;
            }

            
            return "asdf";
        }
        #endregion

        #region 查找用户ID FindUserID()
        /// <summary>
        /// 查找用户ID
        /// </summary>
        /// <returns></returns>
        private static string FindUserID()
        {
            string strRequest = "http://guahao.qingdaonews.com/GhUser/index.html";
            string strContent = HTTPproc.OpenRead(strRequest);

            try
            {
                strContent = Regex.Match(strContent, @"userid=\d{0,}").Value.Replace("userid=", "");
                ini.WriteValue("UserInfo", "UserID", strContent);
                return strContent;
            }
            catch (ArgumentException ex)
            {
                return "";
                // Syntax error in the regular expression
            }
        }
        #endregion

        #region 用户登录 Login(string strUsername, string strPassword, string strCode)
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="strUsername"></param>
        /// <param name="strPassword"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        private static bool Login(string strUsername, string strPassword, string strCode)
        {
            if (strUsername != "" && strPassword != "" && strCode != "")
            {
                string strRequest = "http://guahao.qingdaonews.com/GhUser/login.html";
                string strParameter = "GhUser%5Buser_pass%5D=" + strUsername + "&GhUser%5Buser_password%5D=" + strPassword + "&code=" + strCode;
                string strContent = HTTPproc.OpenRead(strRequest, strParameter);

                if (strContent.IndexOf("验证码不正确") > 0)
                {
                    SysLog("验证码不正确");
                    return false;
                }
                else if (strContent.IndexOf("密码不正确！") > 0)
                {
                    SysLog("密码不正确！");
                    return false;
                }
                else if (strContent.IndexOf("手机号不正确") > 0)
                {
                    SysLog("手机号不正确");
                    return false;
                }
                else if (strContent.IndexOf("手机号尚未注册") > 0)
                {
                    SysLog("手机号尚未注册");
                    return false;
                }
                else if (strContent.IndexOf("身份证号不合法!") > 0)
                {
                    SysLog("身份证号不合法!");
                    return false;
                }
                else if (strContent.IndexOf("身份证号不存在！") > 0)
                {
                    SysLog("身份证号不存在！");
                    return false;
                }
                else if (strContent.IndexOf("window") > 0)
                {
                    SysLog("登录成功");
                    ini.WriteValue("UserInfo", "UserName", strUsername);
                    ini.WriteValue("UserInfo", "Password", strPassword);
                    _isLogin = true;
                    return true;
                }
                else
                {
                    SysLog(strContent);
                    return false;
                }
            }
            else
            {
                SysLog("用户名，密码，验证码不能为空！");
                return false;
            }
        }
        #endregion

        #region SysLog SysLog(string strContent)
        /// <summary>
        /// SysLog
        /// </summary>
        /// <param name="strContent"></param>
        private static void SysLog(string strContent)
        {
            DateTime dt = DateTime.Now;
            Console.WriteLine(dt.ToString() + " | " + strContent);
        }
        #endregion

        #region 获取验证码图片 ImageCode()
        /// <summary>
        /// 获取验证码图片
        /// </summary>
        private static void ImageCode()
        {
            SysLog("加载验证码...");
            long lTimeSpan = TimeSpans();
            string strRequest = "http://guahao.qingdaonews.com/ghUser/captcha/refresh/1.html?_=" + lTimeSpan;
            HTTPproc.RequestHeaders.Add("Referer:http://guahao.qingdaonews.com/GhUser/login.html");
            HTTPproc.RequestHeaders.Add("Accept:application/json, text/javascript, */*; q=0.01");
            string strContent = HTTPproc.OpenRead(strRequest);
            try
            {
                strContent = Regex.Match(strContent, @"\w{13,}").Value;
                //向指定网址请求返回数据流
                HTTPproc.RequestHeaders.Add("Referer:http://guahao.qingdaonews.com/GhUser/login.html");
                HTTPproc.RequestHeaders.Add("Accept:image/png,image/*;q=0.8,*/*;q=0.5");
                HTTPproc.DownloadFile("http://guahao.qingdaonews.com/ghUser/captcha/v/" + strContent + ".html",@"1.png");
                SysLog("加载完成，请在程序运行目录中查看验证码。");
            }
            catch (ArgumentException ex)
            {
                SysLog("加载失败，请联系作者。");
                // Syntax error in the regular expression
            }
        }
        #endregion 获取验证码图片 ImageCode()

        #region 动态时间戳 TimeSpans()
        /// <summary>
        /// 动态时间戳
        /// </summary>
        /// <returns></returns>
        private static long TimeSpans()
        {
            long epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            return epoch;
        }
        #endregion

        #region URLEncode UrlEncode(string str, string encode)
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
            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-";
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
