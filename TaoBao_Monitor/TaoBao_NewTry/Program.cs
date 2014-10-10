using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace TaoBao_NewTry
{
    class Program
    {
        public static string _umto = null;
        public static string _strUserName = null;
        public static string _strPassword = null;
        public static string _strItemID = null;
        public static string _sessionID = null;
        public static string _cookie = null;
        public static string _token = null;
        public static string _st = null;
        public static string _strRequest = null;
        public static string _strQuestion = null;
        public static string _strAnswer = null;

        static HttpHelper HTTPproc = new HttpHelper();

        static void Main(string[] args)
        {
            //Login("cupid0426", "850616cupid0426");
            //Login("diudiuqwe", "qweqwe123");
            Console.WriteLine(CheckTryStatus("5719879"));
            Console.ReadKey();
        }

        #region 登录方法 Login(string strUserName,string strPassword)
        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        private static void Login(string strUserName,string strPassword)
        {
            string strContent = null;
            string strRequest = null;       //存取Get请求
            string strParameter = null;     //存取Post参数
            string strLocationURL = null;
            string strReferer = null;

            if (strUserName != "" && strPassword != "")
            {
                HttpItem item = new HttpItem()
                {
                    URL = "https://login.taobao.com/member/login.jhtml?redirectURL=http%3A%2F%2Ftry.taobao.com%2Fitem%2Fmy_try_item.htm%3Fspm%3Da1z0i.1000799.0.72.5jWSof",//URL     必需项  
                    Encoding = Encoding.GetEncoding("UTF-8"),//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别  
                    Method = "GET",//URL     可选项 默认为Get  
                    Timeout = 15000,//连接超时时间     可选项默认为100000  
                    ReadWriteTimeout = 15000,//写入Post数据超时时间     可选项默认为30000  
                    IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                    Cookie = "",//字符串Cookie     可选项  
                    UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                    Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                    ContentType = "text/html" //返回类型    可选项有默认值
                };  
                HttpResult result = HTTPproc.GetHtml(item);  
                string html = result.Html;


                //得到Cookie列表
                List<CookieItem> cookilist = HttpCookieHelper.GetCookieList(result.Cookie);

                foreach (CookieItem ck in cookilist)
                {
                    _cookie += ck.Key + "=" + ck.Value + ";";             
                }

                try
                {
                    _umto = Regex.Match(html, "(?<=umto.*?value=\").*(?=\")").Value;
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }
                
                if (_umto != "")
                {                    
                    strRequest = "https://login.taobao.com/member/request_nick_check.do?_input_charset=utf-8&username=" + UrlEncode(strUserName) + "&_ksTS=" + TimeStamp() + "_29";

                    //---------------------------------------------------------------------
                    item = new HttpItem()
                    {
                        URL = strRequest,//URL     必需项  
                        Encoding = Encoding.GetEncoding("UTF-8"),//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别  
                        Method = "GET",//URL     可选项 默认为Get  
                        Timeout = 15000,//连接超时时间     可选项默认为100000  
                        ReadWriteTimeout = 15000,//写入Post数据超时时间     可选项默认为30000  
                        IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                        Cookie = _cookie,//字符串Cookie     可选项  
                        UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                        Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值 
                        Referer = "https://login.taobao.com/member/login.jhtml?redirectURL=http%3A%2F%2Ftry.taobao.com%2Fitem%2Fmy_try_item.htm%3Fspm%3Da1z0i.1000799.0.72.5jWSof",//来源URL     可选项
                        ContentType = "application/x-www-form-urlencoded" //返回类型    可选项有默认值
                    };
                    result = HTTPproc.GetHtml(item);
                    strContent = result.Html;


                    strParameter = "need_check_code=&loginsite=0&newlogin=1&TPL_redirect_url=&from=tb&fc=default&style=default&css_style=&tid=&support=000001&CtrlVersion=1%2C0%2C0%2C7&loginType=3&minititle=&minipara=&umto=" + _umto + _umto + "%2C200&pstrong=2&llnick=&sign=&need_sign=&isIgnore=&full_redirect=&popid=&callback=1&guf=&not_duplite_str=&need_user_id=&poy=&gvfdcname=10&gvfdcre=687474703A2F2F74616F6A696E62692E74616F62616F&from_encoding=&sub=&oslanguage=&sr=&osVer=&naviVer=&TPL_username=" + UrlEncode(strUserName) + "&TPL_password=" + UrlEncode(strPassword) + "&TPL_checkcode=";
                    //---------------------------------------------------------------------
                    item = new HttpItem()
                    {
                        URL = "https://login.taobao.com/member/login.jhtml",//URL     必需项  
                        Encoding = Encoding.GetEncoding("UTF-8"),//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别  
                        Method = "POST",//URL     可选项 默认为Get  
                        Timeout = 15000,//连接超时时间     可选项默认为100000  
                        ReadWriteTimeout = 15000,//写入Post数据超时时间     可选项默认为30000  
                        IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                        Cookie = _cookie,//字符串Cookie     可选项  
                        UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                        Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                        ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值  
                        Referer = "https://login.taobao.com/member/login.jhtml?redirectURL=http%3A%2F%2Ftry.taobao.com%2Fitem%2Fmy_try_item.htm%3Fspm%3Da1z0i.1000799.0.72.5jWSof",//来源URL     可选项
                        Postdata = strParameter//Post数据     可选项GET时不需要写 
                    };
                    result = HTTPproc.GetHtml(item);
                    strContent = result.Html;

                    try
                    {
                        _token = Regex.Match(strContent, "(?<=token.*?:\").*(?=\")").Value;
                    }
                    catch (ArgumentException ex)
                    {
                        // Syntax error in the regular expression
                    }

                    strRequest = "https://passport.alipay.com/mini_apply_st.js?site=0&token=" + _token + "&_ksTS=" + TimeStamp() + "_77&callback=jsonp78";

                    if (_token != "")
                    {
                        //---------------------------------------------------------------------
                        item = new HttpItem()
                        {
                            URL = strRequest, //URL     必需项  
                            Encoding = Encoding.GetEncoding("UTF-8"), //编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别  
                            Method = "POST", //URL     可选项 默认为Get  
                            Timeout = 15000, //连接超时时间     可选项默认为100000  
                            ReadWriteTimeout = 15000, //写入Post数据超时时间     可选项默认为30000  
                            IsToLower = false, //得到的HTML代码是否转成小写     可选项默认转小写  
                            Cookie = "", //字符串Cookie     可选项  
                            UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0", //用户的浏览器类型，版本，操作系统     可选项有默认值  
                            Accept = "text/html, application/xhtml+xml, */*", //    可选项有默认值  
                            ContentType = "application/x-www-form-urlencoded", //返回类型    可选项有默认值  
                            Referer = "https://login.taobao.com/member/login.jhtml?redirectURL=http%3A%2F%2Ftry.taobao.com%2Fitem%2Fmy_try_item.htm%3Fspm%3Da1z0i.1000799.0.72.5jWSof",//来源URL     可选项
                            Postdata = "" //Post数据     可选项GET时不需要写 
                        };
                        result = HTTPproc.GetHtml(item);
                        strContent = result.Html;

                        try
                        {
                            _st = Regex.Match(strContent, @"(?<=st.*?:"").*(?=""\})").Value;
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }


                        //---------------------------------------------------------------------
                        if (_st != "")
                        {
                            //strRequest = "https://login.taobao.com/member/vst.htm?st=" + _st + "&params=style%253Ddefault%2526sub%253D%2526longLogin%253D0%2526TPL_username%253D" + UrlEncode(strUserName) + "%2526loginsite%253D0%2526from_encoding%253D%2526not_duplite_str%253D%2526guf%253D%2526full_redirect%253D%2526isIgnore%253D%2526need_sign%253D%2526sign%253D%2526from%253Dtb%2526TPL_redirect_url%253Dhttp%25253A%25252F%25252Fwww.taobao.com%25252F&_ksTS=" + TimeStamp() + "_89&callback=jsonp90";
                            strRequest = "https://login.taobao.com/member/vst.htm?st=" + _st + "&params=style%3Ddefault%26sub%3D%26longLogin%3D0%26TPL_username%3D" + UrlEncode(strUserName) + "%26loginsite%3D0%26from_encoding%3D%26not_duplite_str%3D%26guf%3D%26full_redirect%3D%26isIgnore%3D%26need_sign%3D%26sign%3D%26from%3Dtb%26TPL_redirect_url%3Dhttp%25253A%25252F%25252Ftry.taobao.com%25252Fitem%25252Fmy_try_item.htm%25253Fspm%25253Da1z0i.1000799.0.72.5jWSof&_ksTS=" + TimeStamp() + "_83&callback=jsonp84";

                            item = new HttpItem()
                            {
                                URL = strRequest, //URL     必需项  
                                Encoding = Encoding.GetEncoding("UTF-8"), //编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别  
                                Method = "POST", //URL     可选项 默认为Get  
                                Timeout = 15000, //连接超时时间     可选项默认为100000  
                                ReadWriteTimeout = 15000, //写入Post数据超时时间     可选项默认为30000  
                                IsToLower = false, //得到的HTML代码是否转成小写     可选项默认转小写  
                                Cookie = _cookie, //字符串Cookie     可选项  
                                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0", //用户的浏览器类型，版本，操作系统     可选项有默认值  
                                Accept = "text/html, application/xhtml+xml, */*", //    可选项有默认值  
                                ContentType = "application/x-www-form-urlencoded", //返回类型    可选项有默认值  
                                Referer = "https://login.taobao.com/member/login.jhtml?redirectURL=http%3A%2F%2Ftry.taobao.com%2Fitem%2Fmy_try_item.htm%3Fspm%3Da1z0i.1000799.0.72.5jWSof",//来源URL     可选项
                                Postdata = "" //Post数据     可选项GET时不需要写 
                            };
                            result = HTTPproc.GetHtml(item);
                            strContent = result.Html;

                            //得到Cookie列表
                            cookilist = HttpCookieHelper.GetCookieList(result.Cookie);
                            _cookie = "";
                            foreach (CookieItem ck in cookilist)
                            {
                                _cookie += ck.Key + "=" + ck.Value + ";";
                            }


                            //---------------------------------------------------------------------
                            item = new HttpItem()
                            {
                                URL = "http://try.taobao.com/item/my_try_item.htm?spm=a1z0i.1000799.0.72.5jWSof", //URL     必需项  
                                Encoding = Encoding.GetEncoding("UTF-8"), //编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别  
                                Method = "GET", //URL     可选项 默认为Get  
                                Timeout = 15000, //连接超时时间     可选项默认为100000  
                                ReadWriteTimeout = 15000, //写入Post数据超时时间     可选项默认为30000  
                                IsToLower = false, //得到的HTML代码是否转成小写     可选项默认转小写  
                                Cookie = "v=0;" + _cookie, //字符串Cookie     可选项  
                                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0", //用户的浏览器类型，版本，操作系统     可选项有默认值  
                                Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, */*" //    可选项有默认值

                            };
                            result = HTTPproc.GetHtml(item);
                            strContent = result.Html;
                        }  
                    }                                      
                }
                
                Console.WriteLine(strContent);
            }
        }
        #endregion 登录方法 Login(string strUserName,string strPassword)
        
        private static void Request_Try(string strItemID)
        {
            string strRequest = null;
            string strParameter = null;
            string strContent = null;
            string resultString = null;
            string strTemp = null;

            ////http://try.taobao.com/item.htm?id=3160723
            //strRequest = "http://try.taobao.com/item.htm?id=" + strItemID;
            //strContent = HTTPproc.OpenRead(strRequest);
            //if (strContent.IndexOf("申请免费试用") > 0)
            //{
            //    //Console.WriteLine(strItemID + " 可以申请试用");
            //    //Login(_strUserName, _strPassword);
            //    strRequest = "http://try.taobao.com/item/add_try_request.htm?item_id=" + strItemID;
            //    strParameter = "_tb_token_=" + _umto + "&action=%2Frequest_action&event_submit_do_add_try_request=true&_fmd.addt._0.i=" + strItemID + "&_fmd.addt._0.in=&_fmd.addt._0.pr=%C9%BD%B6%AB%CA%A1&_fmd.addt._0.ci=%C7%E0%B5%BA%CA%D0&_fmd.addt._0.are=%CA%D0%C4%CF%C7%F8&_fmd.addt._0.co=40&youhuiduoActiveId=&youhuiduoActiveType=&youhuiduoStoreId=&youhuiduoMobile=&youhuiduoProName=&youhuiduoCityName=&_fmd.addt._0.g=male&_fmd.addt._0.b=1985&_fmd.addt._0.ad=10444643&_fmd.addt._0.p=370000&_fmd.addt._0.c=370200&_fmd.addt._0.ar=370202&_fmd.addt._0.d=&_fmd.addt._0.po=266000&_fmd.addt._0.add=%C7%E0%B5%BA%B9%FA%BC%CA%B6%AF%C2%FE%B2%FA%D2%B5%D4%B0E%D7%F9219%A3%AC%C3%C0%CC%EC%CD%F8%C2%E7&_fmd.addt._0.n=%BA%AB%D6%BE%CE%B0&_fmd.addt._0.ph=13156280289&_fmd.addt._0.pho=&_fmd.addt._0.phon=&_fmd.addt._0.phone=&_fmd.addt._0.is=on&_fmd.addt._0.t=1&_fmd.addt._0.r=%B7%C7%B3%A3%B8%D0%D0%BB%D4%DE%D6%FA%B5%EA%C6%CC%CE%AA%CE%D2%C3%C7%CC%E1%B9%A9%C1%CB%D5%E2%B4%CE%C3%E2%B7%D1%CA%D4%D3%C3%B5%C4%BB%FA%BB%E1%A3%AC%CE%D2%D2%B2%CA%AE%B7%D6%CF%B2%BB%B6%D5%E2%BC%FE%C9%CC%C6%B7%A3%AC%D2%B2%BA%DC%CF%A3%CD%FB%B3%C9%CE%AA%D2%BB%C3%FB%D0%D2%D4%CB%B6%F9%C4%DC%D3%D0%D5%E2%C3%B4%B8%F6%BB%FA%BB%E1%B5%C3%B5%BD%D5%E2%BC%FE%C3%E2%B7%D1%CA%D4%D3%C3%C9%CC%C6%B7%A1%A3%CF%A3%CD%FB%BA%C3%D4%CB%BF%C9%D2%D4%BD%B5%C1%D9%A3%AC%C8%E7%B9%FB%B3%C9%CE%AA%D0%D2%D4%CB%B6%F9%A3%AC%CC%E5%D1%E9%B9%FD%BA%F3%A3%AC%CE%D2%BB%E1%C8%CF%D5%E6%B9%AB%D5%FD%B5%D8%CC%EE%D0%B4%B8%C3%D3%C3%B1%A8%B8%E6%A3%AC%BF%CD%B9%DB%B5%C4%B6%D4%C6%DA%D0%D4%BC%DB%B1%C8%BC%B0%B8%F7%B8%F6%B7%BD%C3%E6%BD%F8%D0%D0%C6%C0%BC%DB%A1%A3%D7%A3%D4%B8%B5%EA%BC%D2%C9%FA%D2%E2%D0%CB%C2%A1%A3%AC%D2%B2%CF%A3%CD%FB%CE%D2%BF%C9%D2%D4%C9%EA%C7%EB%B3%C9%B9%A6%A3%AC%D0%BB%D0%BB%7E%7E%7E&_fmd.addt._0.su=&_fmd.addt._0.ag=1";
            //    strContent = HTTPproc.OpenRead(strRequest, strParameter);
            //    if (strContent.IndexOf("提交成功") > 0)
            //    {
            //        Log.WriteLog(LogFile.Trace, "宝贝" + strItemID + "申请成功");
            //    }
            //    else
            //    {
            //        Log.WriteLog(LogFile.Trace, strItemID + " 程序出现异常");
            //    }
            //    //Thread.Sleep(5000);
            //}
            //else if (strContent.IndexOf("您的申请已提交") > 0)
            //{
            //    //Console.WriteLine(strItemID + " 申请已提交");
            //    Log.WriteLog(LogFile.Trace, strItemID + " 申请已提交");
            //    //Thread.Sleep(2000);
            //}
            //else
            //{
            //    //Console.WriteLine(strItemID + " 试用还没开始");
            //    Log.WriteLog(LogFile.Trace, strItemID + " 试用还没开始");
            //    //Thread.Sleep(2000);
            //}
        }

        #region 检查试用状态 CheckTryStatus(string strItemID)
        /// <summary>
        /// 检查试用状态
        /// </summary>
        /// <param name="strItemID">试用链接</param>
        /// <returns></returns>
        private static bool CheckTryStatus(string strItemID)
        {
            string strRequest = "http://try.taobao.com/item.htm?id=" + strItemID;
            string strContent = null;

            Console.WriteLine("正在读取:" + strItemID);

            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = strRequest, //URL     必需项  
                //Encoding = Encoding.GetEncoding("UTF-8"), //编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别  
                Timeout = 15000, //连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 15000 //写入Post数据超时时间     可选项默认为30000
            };
            HttpResult result = http.GetHtml(item);
            strContent = result.Html;

            
            if (strContent.IndexOf("找答案") > 0)
            {
                Console.WriteLine("原来已经开始了");
                string resultString = null;
                try
                {
                    if (strContent.IndexOf("试用品申请成功后需提交") > 0)
                    {
                        _strAnswer = "试用报告";
                    }
                    else
                    {
                        Console.WriteLine("正在搜索问题链接");
                        _strRequest = Regex.Match(strContent, "(?<=是 ？<a href=\").*(?=\" target=\"_blank\")").Value; //找出来的是链接
                        Console.WriteLine("找到问题链接:" + _strRequest);

                        Console.WriteLine("正在搜索问题内容");
                        _strQuestion = CheckTryQuestion(strContent);
                        Console.WriteLine("找到问题内容:" + _strQuestion);

                        Console.WriteLine("正在搜索答案");
                        _strAnswer = CheckTryAnswer(_strRequest);
                        Console.WriteLine("答案内容:" + _strAnswer);

                        if (_strAnswer != "")
                        {
                            Console.WriteLine("找到问题并返回");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("没找到问题或找问题出错");
                            return false;
                        }
                    }
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

            }

            if (strContent.IndexOf("距离开始") > 0)
            {
                Console.WriteLine("链接不能使用:" + strItemID);
                return false;
            }
            else
            {
                Console.WriteLine("链接可以使用:" + strItemID);
                return true;
            }
        }
        #endregion 检查试用状态 CheckTryStatus(string strItemID)

        /// <summary>
        /// 打开淘宝链接搜索答案
        /// </summary>
        /// <param name="strLink"></param>
        /// <returns></returns>
        private static string CheckTryAnswer(string strLink)
        {
            string strContent = null;

            Console.WriteLine("正在读取答案链接:" + strLink);

            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = strLink, //URL     必需项  
                Encoding = Encoding.GetEncoding("GB2312"), //编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别  
                Allowautoredirect = true,
                Timeout = 15000, //连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 15000 //写入Post数据超时时间     可选项默认为30000
            };
            HttpResult result = http.GetHtml(item);
            strContent = result.Html;

            string resultString = null;
            try
            {
                _strAnswer = Regex.Match(strContent, "(?<=" + _strQuestion + ":&nbsp;).*?(?=</li>)").Value; //正在根据问题搜索答案
                _strAnswer = System.Web.HttpUtility.HtmlDecode(_strAnswer);
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            return _strAnswer;
        }

        /// <summary>
        /// 搜索问题
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        private static string CheckTryQuestion(string strContent)
        {
            string resultString = null;
            try
            {
                resultString = Regex.Match(strContent, "(?<=属性描述.*<em>).*(?=</em>)").Value;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            return resultString;
        }

        #region Url编码 UrlEncode(string url)
        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string UrlEncode(string url)
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

        #region 判定是否需要验证码 CheckCode(string strUserName)
        /// <summary>
        /// 判定是否需要验证码
        /// </summary>
        /// <param name="strUserName"></param>
        /// <returns></returns>
        private static bool CheckCode(string strUserName)
        {
            //string strRequest = null;
            //string strContent = null;

            //strRequest = "http://login.taobao.com/member/request_nick_check.do?_input_charset=utf-8&username=" + strUserName + "&_ksTS=1337151092385_29";
            //strContent = HTTPproc.OpenRead(strRequest);
            //if (strContent.IndexOf("false") > 0)
            //{
            //    return false;
            //}
            //else
            //{
                return true;
            //}
        }
        #endregion 判定是否需要验证码 CheckCode(string strUserName)

        #region 时间戳 TimeStamp()
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        private static string TimeStamp()
        {
            string strTimeStamp = Convert.ToString((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000);
            return strTimeStamp;
        }
        #endregion 时间戳 TimeStamp()
    }
}
