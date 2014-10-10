using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;

namespace TaoBao_Try
{
    class Program
    {
        static WebClient HTTPproc = new WebClient();
        static ArrayList alItemID = new ArrayList();
        static int i = 0;

        static void Main(string[] args)
        {
            TB_Login_Handle("cupid0426", "850616cupid0426");
            //ReceiveCoin();
            Show_Try();
            //while (true)
            //{
            //    if (i > 360)
            //    {
            //        HTTPproc = new WebClient();
            //        TB_Login_Handle("cupid0426", "850616cupid0426");
            //        i = 0;
            //    }
            //    try
            //    {
            //        Show_Try();
            //    }
            //    catch
            //    {
            //        Thread.CurrentThread.Abort();
            //    }
            //    Thread.Sleep(10000);
            //    i++;
            //}
        }

        private static void Show_Try()
        {
            string strRequest = null;
            string strContent = null;
            string strCookie = null;
            string resultString = null;
            string strTemp = null;
            int intPage = 0;

            HTTPproc.Encoding = System.Text.Encoding.Default;

            strRequest = "http://try.taobao.com/item_list.htm";

            //resultString = HTTPproc.ResponseHeaders[2];

            foreach (string strCookies in HTTPproc.ResponseHeaders.GetValues("Set-Cookie"))
            {
                strTemp = Regex.Replace(strCookies, "Domain=.{5,20};", "");
                strTemp = Regex.Replace(strTemp, "Expires=.{5,35};", "");
                strTemp = Regex.Replace(strTemp, "Path=/|,", "").Replace(" ", "").Replace("HttpOnly", "");
                strTemp = Regex.Replace(strTemp, @"\r", "");
                resultString += strTemp;
            }
            
            strCookie = HTTPproc.Cookie.ToString();
            strCookie = Regex.Replace(strCookie, "uc1=.{120,200}%3D%3D;", "");
            strCookie = resultString + strCookie;
            HTTPproc.Cookie = strCookie;
            strContent = HTTPproc.OpenRead(strRequest);
            //Console.Write(strContent);

            //得到总页数
            //try
            //{
            //    resultString = Regex.Match(strContent, @"/\d{0,3}</span>").Value.Replace("</span>", "").Replace("/", "");
            //    intPage = Convert.ToInt32(resultString);
            //    Console.WriteLine("试用共有 " + intPage + " 页");
            //}
            //catch
            //{
            //    Console.WriteLine("没有解析到页码");
            //}

            Console.WriteLine("页面分析开始");
            Analyze_Link(strContent);
            //for (int i = 2; i < intPage + 1; i++)
            //{
            //    strRequest = "http://try.taobao.com/item_list.htm?q=&look_type=2&order_type=0&cat_id=&brand=&o_cat_id=&zone_type=0&page=" + i;
            //    Console.WriteLine(strRequest);
            //    strContent = HTTPproc.OpenRead(strRequest);
            //    Analyze_Link(strContent);
            //}
        }

        private static void Analyze_Link(string strContent)
        {
            int j = 0;
            string strItemID = null;
            try
            {
                Regex regexObj = new Regex(@"<a class=""item-name"" target=""_blank"" href=""http://try\.taobao\.com/item/item_detail\.htm\?item_id=\d{0,8}");
                Match matchResults = regexObj.Match(strContent);
                while (matchResults.Success)
                {
                    strItemID = matchResults.Value.Replace("<a class=\"item-name\" target=\"_blank\" href=\"http://try.taobao.com/item/item_detail.htm?item_id=", "");
                    if (strItemID != "12918")
                    {
                        if (alItemID.Count > 0 && i > 0)
                        {
                            if (alItemID.IndexOf(strItemID) == -1)
                            {
                                Request_Try(strItemID);
                            }
                        }
                        else
                        {
                            if (strItemID != "")
                            {
                                Request_Try(strItemID);
                            }
                        }
                    }
                    matchResults = matchResults.NextMatch();
                }
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
        }

        #region 淘宝登录流程 TB_Login_Handle()
        /// <summary>
        /// 淘宝登录流程
        /// </summary>
        private static void TB_Login_Handle(string strUserName, string strPassword)
        {
            string strContent = null;       //
            string resultString = null;
            string strRequest = null;       //存取Get请求
            string strParameter = null;     //存取Post参数
            string strCookie = null;        //存取Cookie
            string strToken = null;

            if (strUserName != "" && strPassword != "")
            {
                //设置HTTP请求默认编码
                HTTPproc.Encoding = System.Text.Encoding.Default;

                //==============  Cookie 参数  =================
                //cookie2          522b0f7e4609ff4fde596087a9bce9f3
                //_tb_token_       477837e79383
                //t                fe8ad20f41c5f335d453be685896227e
                //uc1              cookie14=UoM8cfXc1sA+RA==
                //v                0
                //_lang            zh_CN:GBK
                //==============================================

                strRequest = "http://login.taobao.com/member/login.jhtml";
                HTTPproc.OpenRead(strRequest);
                strToken = Regex.Match(HTTPproc.Cookie, "_tb_token_=.{10,15};").Value.Replace("_tb_token_=", "").Replace(";", "");
                strParameter = "TPL_username=" + UrlEncode(strUserName) + "&TPL_password=" + UrlEncode(strPassword) + "&_tb_token_=" + strToken + "&action=Authenticator&event_submit_do_login=anything&TPL_redirect_url=&from=tb&fc=2&style=default&css_style=&tid=XOR_1_000000000000000000000000000000_63584451347B0E700A71020D&support=000001&CtrlVersion=1%2C0%2C0%2C7&loginType=3&minititle=&minipara=&pstrong=2&longLogin=-1&llnick=&sign=&need_sign=&isIgnore=&popid=&callback=&guf=&not_duplite_str=&need_user_id=&poy=&gvfdcname=10&from_encoding=";
                strContent = HTTPproc.OpenRead(strRequest, strParameter);
                //得到掉转链接
                strRequest = Regex.Match(strContent, "Location:.*").Value.Replace("Location: ", "").Replace("\r", "");
                try
                {
                    Regex regexObj = new Regex("Set-Cookie: .*");
                    Match matchResults = regexObj.Match(strContent);
                    while (matchResults.Success)
                    {
                        resultString += matchResults.Value;
                        matchResults = matchResults.NextMatch();
                    }
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }
                resultString = Regex.Replace(resultString, "Set-Cookie: ", "");
                resultString = Regex.Replace(resultString, "Domain=.{5,20};", "");
                resultString = Regex.Replace(resultString, "Expires=.{5,35};", "");
                resultString = Regex.Replace(resultString, "Path=/|,", "").Replace(" ", "").Replace("HttpOnly", "");
                resultString = Regex.Replace(resultString, @"\r", "");

                HTTPproc.RequestHeaders.Add("Referer:http://login.taobao.com/member/login.jhtml");
                HTTPproc.Cookie = resultString + "_tb_token_=" + strToken + ";";
                strContent = HTTPproc.OpenRead(strRequest);

                //Console.WriteLine(strContent);
            }
            else
            {
                Console.WriteLine("信息填写有误请检查！", "系统消息");
            }
        }
        #endregion 淘宝登录流程 Login_Handle()

        private static void Request_Try(string strItemID)
        {
            string strRequest = null;
            string strParameter = null;
            string strContent = null;
            string resultString = null;
            string strCookie = null;
            string strTemp = null;

            strRequest = "http://try.taobao.com/item.htm?id=" + strItemID;
            //试用详情页
            strCookie = HTTPproc.Cookie.ToString();
            strCookie = Regex.Replace(strCookie, "uc1=.{120,200}%3D%3D;", "");
            foreach (string strCookies in HTTPproc.ResponseHeaders.GetValues("Set-Cookie"))
            {
                strTemp = Regex.Replace(strCookies, "Set-Cookie: ", "");
                strTemp = Regex.Replace(strTemp, "Domain=.{5,20};", "");
                strTemp = Regex.Replace(strTemp, "Expires=.{5,35};", "");
                strTemp = Regex.Replace(strTemp, "Path=/|,", "").Replace(" ", "").Replace("HttpOnly", "");
                strTemp = Regex.Replace(strTemp, @"\r", "");
                strTemp = Regex.Replace(strTemp, @"v=0;", "");
                resultString += strTemp;
            }
            strCookie = strCookie + resultString;
            HTTPproc.Cookie = strCookie;
            strContent = HTTPproc.OpenRead(strRequest);
            if (strContent.IndexOf("您的申请已提交") < 0)
            {
                //试用申请页     
                strCookie = HTTPproc.Cookie.ToString();
                strCookie = Regex.Replace(strCookie, "uc1=.{120,200}%3D%3D;", "");
                foreach (string strCookies in HTTPproc.ResponseHeaders.GetValues("Set-Cookie"))
                {
                    strTemp = Regex.Replace(strCookies, "Set-Cookie: ", "");
                    strTemp = Regex.Replace(strTemp, "Domain=.{5,20};", "");
                    strTemp = Regex.Replace(strTemp, "Expires=.{5,35};", "");
                    strTemp = Regex.Replace(strTemp, "Path=/|,", "").Replace(" ", "").Replace("HttpOnly", "");
                    strTemp = Regex.Replace(strTemp, @"\r", "");
                    strTemp = Regex.Replace(strTemp, @"v=0;", "");
                    resultString += strTemp;
                }
                strCookie = strCookie + resultString;
                HTTPproc.Cookie = strCookie;
                strRequest = "http://try.taobao.com/item/request_rule.htm?item_id=" + strItemID;
                strContent = HTTPproc.OpenRead(strRequest);

                //试用提交页
                strCookie = HTTPproc.Cookie.ToString();
                strCookie = Regex.Replace(strCookie, "uc1=.{120,200}%3D%3D;", "");
                foreach (string strCookies in HTTPproc.ResponseHeaders.GetValues("Set-Cookie"))
                {
                    strTemp = Regex.Replace(strCookies, "Set-Cookie: ", "");
                    strTemp = Regex.Replace(strTemp, "Domain=.{5,20};", "");
                    strTemp = Regex.Replace(strTemp, "Expires=.{5,35};", "");
                    strTemp = Regex.Replace(strTemp, "Path=/|,", "").Replace(" ", "").Replace("HttpOnly", "");
                    strTemp = Regex.Replace(strTemp, @"\r", "");
                    strTemp = Regex.Replace(strTemp, @"v=0;", "");
                    resultString += strTemp;
                }
                strCookie = strCookie + resultString;
                HTTPproc.Cookie = strCookie;
                strRequest = "http://try.taobao.com/item/add_try_request.htm?item_id=" + strItemID;
                strContent = HTTPproc.OpenRead(strRequest);

                //试用表单提交内容分析
                string _tb_token_ = Regex.Match(strContent, "_tb_token_\" value=\".*\"").Value.Replace("_tb_token_\" value=\"", "").Replace("\"", "");//_tb_token_" value="e87d7d5313ba3"
                string strNickname = Regex.Match(strContent, "_fmd.add._0.u\"  value=\".*\"").Value.Replace("_fmd.add._0.u\"  value=\"", "").Replace("\"", "");//_fmd.add._0.u"  value="diudiuqwe"

                //试用提交内容
                strCookie = HTTPproc.Cookie.ToString();
                strCookie = Regex.Replace(strCookie, "uc1=.{120,200}%3D%3D;", "");
                foreach (string strCookies in HTTPproc.ResponseHeaders.GetValues("Set-Cookie"))
                {
                    strTemp = Regex.Replace(strCookies, "Set-Cookie: ", "");
                    strTemp = Regex.Replace(strTemp, "Domain=.{5,20};", "");
                    strTemp = Regex.Replace(strTemp, "Expires=.{5,35};", "");
                    strTemp = Regex.Replace(strTemp, "Path=/|,", "").Replace(" ", "").Replace("HttpOnly", "");
                    strTemp = Regex.Replace(strTemp, @"\r", "");
                    strTemp = Regex.Replace(strTemp, @"v=0;", "");
                    resultString += strTemp;
                }
                strCookie = strCookie + resultString;
                HTTPproc.Cookie = strCookie;
                strRequest = "http://try.taobao.com/item/add_try_request.htm?item_id=" + strItemID;
                //strParameter = "_tb_token_=" + _tb_token_ + "&action=%2Frequest_action&event_submit_do_add_try_request=true&_fmd.add._0.i=" + strItemID + "&_fmd.add._0.in=&_fmd.add._0.pr=%C9%BD%B6%AB%CA%A1&_fmd.add._0.ci=%C7%E0%B5%BA%CA%D0&_fmd.add._0.are=%CA%D0%C4%CF%C7%F8&_fmd.add._0.co=100&_fmd.add._0.u=" + UrlEncode(strNickname) + "&_fmd.add._0.g=male&_fmd.add._0.b=1985&_fmd.add._0.r=%B7%C7%B3%A3%B8%D0%D0%BB%D4%DE%D6%FA%B5%EA%C6%CC%CE%AA%CE%D2%C3%C7%CC%E1%B9%A9%C1%CB%D5%E2%B4%CE%C3%E2%B7%D1%CA%D4%D3%C3%B5%C4%BB%FA%BB%E1%A3%AC%CE%D2%D2%B2%CA%AE%B7%D6%CF%B2%BB%B6%D5%E2%BC%FE%C9%CC%C6%B7%A3%AC%D2%B2%BA%DC%CF%A3%CD%FB%B3%C9%CE%AA%D2%BB%C3%FB%D0%D2%D4%CB%B6%F9%C4%DC%D3%D0%D5%E2%C3%B4%B8%F6%BB%FA%BB%E1%B5%C3%B5%BD%D5%E2%BC%FE%C3%E2%B7%D1%CA%D4%D3%C3%B5%C4%C9%CC%C6%B7%A1%A3%CF%A3%CD%FB%BA%C3%D4%CB%BF%C9%D2%D4%BD%B5%C1%D9%A3%AC%C8%E7%B9%FB%B3%C9%CE%AA%C1%CB%D0%D2%D4%CB%B6%F9%A3%AC%CC%E5%D1%E9%B9%FD%BA%F3%A3%AC%CE%D2%BB%E1%C8%CF%D5%E6%B9%AB%D5%FD%B5%D8%CC%EE%D0%B4%CA%D4%D3%C3%B1%A8%B8%E6%A3%AC%BF%CD%B9%DB%B5%C4%B6%D4%C6%E4%D0%D4%BC%DB%B1%C8%BC%B0%B8%F7%B8%F6%B7%BD%C3%E6%BD%F8%D0%D0%C6%C0%BC%DB%A1%A3%D7%A3%D4%B8%B5%EA%BC%D2%C9%FA%D2%E2%D0%CB%C2%A1%A3%AC%D2%B2%CF%A3%CD%FB%CE%D2%BF%C9%D2%D4%C9%EA%C7%EB%B3%C9%B9%A6%A3%AC%D0%BB%D0%BB+%A3%A1%A3%A1%A3%A1%A3%A1%A3%A1%A3%A1&_fmd.add._0.a=10444643&_fmd.add._0.p=370000&_fmd.add._0.c=370200&_fmd.add._0.ar=370202&_fmd.add._0.d=&_fmd.add._0.po=266000&_fmd.add._0.ad=%C7%E0%B5%BA%B9%FA%BC%CA%B6%AF%C2%FE%B2%FA%D2%B5%D4%B0E%D7%F9219%A3%AC%C3%C0%CC%EC%CD%F8%C2%E7&_fmd.add._0.n=%BA%AB%D6%BE%CE%B0&_fmd.add._0.ph=13156280289&_fmd.add._0.pho=&_fmd.add._0.phon=&_fmd.add._0.phone=&_fmd.add._0.ag=1";
                //strParameter = "_tb_token_=" + _tb_token_ + "&action=%2Frequest_action&event_submit_do_add_try_request=true&_fmd.add._0.i= " + strItemID + "&_fmd.add._0.in=  &_fmd.add._0.pr=%C9%BD%B6%AB%CA%A1&  _fmd.add._0.ci=%C7%E0%B5%BA%CA%D0&_fmd.add._0.are=%CA%D0%C4%CF%C7%F8&_fmd.add._0.co=122&youhuiduoActiveId=&youhuiduoActiveType=&youhuiduoStoreId=&youhuiduoMobile=&youhuiduoProName=&youhuiduoCityName=&_fmd.add._0.g=male&_fmd.add._0.ge=&_fmd.add._0.b=1985&_fmd.add._0.bi=&_fmd.add._0.ad=10444643&_fmd.add._0.p=370000&_fmd.add._0.c=370200&_fmd.add._0.ar=370202&_fmd.add._0.d=&_fmd.add._0.po=266000&_fmd.add._0.add=%C7%E0%B5%BA%B9%FA%BC%CA%B6%AF%C2%FE%B2%FA%D2%B5%D4%B0E%D7%F9219%A3%AC%C3%C0%CC%EC%CD%F8%C2%E7&_fmd.add._0.n=%BA%AB%D6%BE%CE%B0&_fmd.add._0.ph=13156280289&_fmd.add._0.pho=&_fmd.add._0.phon=&_fmd.add._0.phone=&_fmd.add._0.e=5&_fmd.add._0.is=on&_fmd.add._0.t=1&_fmd.add._0.tr=1&_fmd.add._0.r=%B7%C7%B3%A3%B8%D0%D0%BB%D4%DE%D6%FA%B5%EA%C6%CC%CE%AA%CE%D2%C3%C7%CC%E1%B9%A9%C1%CB%D5%E2%B4%CE%C3%E2%B7%D1%CA%D4%D3%C3%B5%C4%BB%FA%BB%E1%A3%AC%CE%D2%D2%B2%CA%AE%B7%D6%CF%B2%BB%B6%D5%E2%BC%FE%C9%CC%C6%B7%A3%AC%D2%B2%BA%DC%CF%A3%CD%FB%B3%C9%CE%AA%D2%BB%C3%FB%D0%D2%D4%CB%B6%F9%C4%DC%D3%D0%D5%E2%C3%B4%B8%F6%BB%FA%BB%E1%B5%C3%B5%BD%D5%E2%BC%FE%C3%E2%B7%D1%CA%D4%D3%C3%B5%C4%C9%CC%C6%B7%A1%A3%CF%A3%CD%FB%BA%C3%D4%CB%BF%C9%D2%D4%BD%B5%C1%D9%A3%AC%C8%E7%B9%FB%B3%C9%CE%AA%C1%CB%D0%D2%D4%CB%B6%F9%A3%AC%CC%E5%D1%E9%B9%FD%BA%F3%A3%AC%CE%D2%BB%E1%C8%CF%D5%E6%B9%AB%D5%FD%B5%D8%CC%EE%D0%B4%CA%D4%D3%C3%B1%A8%B8%E6%A3%AC%BF%CD%B9%DB%B5%C4%B6%D4%C6%E4%D0%D4%BC%DB%B1%C8%BC%B0%B8%F7%B8%F6%B7%BD%C3%E6%BD%F8%D0%D0%C6%C0%BC%DB%A1%A3%D7%A3%D4%B8%B5%EA%BC%D2%C9%FA%D2%E2%D0%CB%C2%A1%A3%AC%D2%B2%CF%A3%CD%FB%CE%D2%BF%C9%D2%D4%C9%EA%C7%EB%B3%C9%B9%A6%A3%AC%D0%BB%D0%BB+%A3%A1%A3%A1%A3%A1%A3%A1%A3%A1%A3%A1&_fmd.add._0.a=%D0%B4%D2%BB%B7%DD%D5%E6%CA%B5%BF%CD%B9%DB%B5%C4%CA%D4%D3%C3%C6%C0%BC%DB%A3%AC%C9%CF%BC%B8%D5%C5%C6%AF%C1%C1%B5%C4%CD%BC%CD%BC&_fmd.add._0.ag=1";
                strParameter = "_tb_token_=" + _tb_token_ + "&action=%2Frequest_action&event_submit_do_add_try_request=true&_fmd.addt._0.i=" + strItemID + "&_fmd.addt._0.in=&_fmd.addt._0.pr=%C9%BD%B6%AB%CA%A1&_fmd.addt._0.ci=%C7%E0%B5%BA%CA%D0&_fmd.addt._0.are=%CA%D0%C4%CF%C7%F8&_fmd.addt._0.co=40&youhuiduoActiveId=&youhuiduoActiveType=&youhuiduoStoreId=&youhuiduoMobile=&youhuiduoProName=&youhuiduoCityName=&_fmd.addt._0.g=male&_fmd.addt._0.b=1985&_fmd.addt._0.ad=10444643&_fmd.addt._0.p=370000&_fmd.addt._0.c=370200&_fmd.addt._0.ar=370202&_fmd.addt._0.d=&_fmd.addt._0.po=266000&_fmd.addt._0.add=%C7%E0%B5%BA%B9%FA%BC%CA%B6%AF%C2%FE%B2%FA%D2%B5%D4%B0E%D7%F9219%A3%AC%C3%C0%CC%EC%CD%F8%C2%E7&_fmd.addt._0.n=%BA%AB%D6%BE%CE%B0&_fmd.addt._0.ph=13156280289&_fmd.addt._0.pho=&_fmd.addt._0.phon=&_fmd.addt._0.phone=&_fmd.addt._0.is=on&_fmd.addt._0.t=1&_fmd.addt._0.r=%B7%C7%B3%A3%B8%D0%D0%BB%D4%DE%D6%FA%B5%EA%C6%CC%CE%AA%CE%D2%C3%C7%CC%E1%B9%A9%C1%CB%D5%E2%B4%CE%C3%E2%B7%D1%CA%D4%D3%C3%B5%C4%BB%FA%BB%E1%A3%AC%CE%D2%D2%B2%CA%AE%B7%D6%CF%B2%BB%B6%D5%E2%BC%FE%C9%CC%C6%B7%A3%AC%D2%B2%BA%DC%CF%A3%CD%FB%B3%C9%CE%AA%D2%BB%C3%FB%D0%D2%D4%CB%B6%F9%C4%DC%D3%D0%D5%E2%C3%B4%B8%F6%BB%FA%BB%E1%B5%C3%B5%BD%D5%E2%BC%FE%C3%E2%B7%D1%CA%D4%D3%C3%C9%CC%C6%B7%A1%A3%CF%A3%CD%FB%BA%C3%D4%CB%BF%C9%D2%D4%BD%B5%C1%D9%A3%AC%C8%E7%B9%FB%B3%C9%CE%AA%D0%D2%D4%CB%B6%F9%A3%AC%CC%E5%D1%E9%B9%FD%BA%F3%A3%AC%CE%D2%BB%E1%C8%CF%D5%E6%B9%AB%D5%FD%B5%D8%CC%EE%D0%B4%B8%C3%D3%C3%B1%A8%B8%E6%A3%AC%BF%CD%B9%DB%B5%C4%B6%D4%C6%DA%D0%D4%BC%DB%B1%C8%BC%B0%B8%F7%B8%F6%B7%BD%C3%E6%BD%F8%D0%D0%C6%C0%BC%DB%A1%A3%D7%A3%D4%B8%B5%EA%BC%D2%C9%FA%D2%E2%D0%CB%C2%A1%A3%AC%D2%B2%CF%A3%CD%FB%CE%D2%BF%C9%D2%D4%C9%EA%C7%EB%B3%C9%B9%A6%A3%AC%D0%BB%D0%BB%7E%7E%7E&_fmd.addt._0.su=&_fmd.addt._0.ag=1";
                strContent = HTTPproc.OpenRead(strRequest, strParameter);
                if (strContent.IndexOf("提交成功") > 0)
                {
                    Console.WriteLine("宝贝" + strItemID + "申请成功");
                    alItemID.Add(strItemID);
                    Thread.Sleep(5000);
                }
                else
                {
                    Console.WriteLine("程序出现异常");
                }
            }
            else
            {
                Console.WriteLine("宝贝" + strItemID + "已经申请过了");
                alItemID.Add(strItemID);
                Thread.Sleep(2000);
            }
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

        private static void ReceiveCoin()
        {
            string strRequest = "http://qz.jianghu.taobao.com/record/coin_get.htm?tracelog=qzindex005";
            HTTPproc.OpenRead(strRequest);
        }
    }
}
