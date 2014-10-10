using System;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Collections;

namespace Show_Web
{
    public class SessionItem
    {
        /// <summary>
        /// 格式化请求
        /// </summary>
        /// <param name="strIndex">请求的索引</param>
        /// <param name="intType">请求的类型0:整型 1:字符串 2:短整型 3:长整型</param>
        /// <returns>返回值</returns>
        public static object GetRequest(string strIndex, int intType)
        {
            HttpContext hc = HttpContext.Current;
            object objOut = hc.Request.QueryString[strIndex];
            if (objOut == null)
            {
                //hc.Response.Redirect("Report.aspx?Parameter=3");
                if (intType == 0)
                    return 0;
                else if (intType == 1)
                    return "";
                else if (intType == 2)
                    return Convert.ToInt16(0);
                else
                    return Convert.ToInt64(0);
            }
            else
            {
                try
                {
                    if (intType == 0)
                        return Convert.ToInt32(objOut.ToString().Trim());
                    else if (intType == 1)
                        return objOut.ToString().Trim();
                    else if (intType == 2)
                        return Convert.ToInt16(objOut.ToString().Trim());
                    else
                        return Convert.ToInt64(objOut.ToString().Trim());
                }
                catch
                {
                    //hc.Response.Redirect("Report.aspx?Parameter=3");
                    if (intType == 0)
                        return 0;
                    else if (intType == 1)
                        return "";
                    else if (intType == 2)
                        return Convert.ToInt16(0);
                    else
                        return Convert.ToInt64(0);
                }
            }
        }

        public static Hashtable GetPara(string strParament)
        {
            if (strParament == null || strParament.IndexOf("=") == -1)
                return null;
            else if (!StringItem.IsSafeWordPara(strParament))
                return null;

            Hashtable htPara = new Hashtable();

            Cuter cPara = StringItem.GetWebServiceArgument(strParament);
            string[] strParaments = cPara.GetArrCuter();
            foreach (string strPara in strParaments)
            {
                if (strPara.IndexOf("=") != -1)
                {
                    Cuter cP = new Cuter(strPara, "=");
                    HttpContext hc = HttpContext.Current;
                    try
                    {
                        htPara.Add(cP.GetCuter(0), hc.Server.UrlDecode(cP.GetCuter(1)));
                    }
                    catch
                    {}
                }
            }

            return htPara;
        }

        public static Hashtable GetParaNew(string strParament)
        {
            if (strParament == null || strParament.IndexOf("=") == -1)
                return null;
            else if (!StringItem.IsSafeWordPara(strParament))
                return null;

            Hashtable htPara = new Hashtable();

            Cuter cPara = new Cuter(strParament, "&");//StringItem.GetWebServiceArgument(strParament);
            string[] strParaments = cPara.GetArrCuter();
            foreach (string strPara in strParaments)
            {
                if (strPara.IndexOf("=") != -1)
                {
                    Cuter cP = new Cuter(strPara, "=");
                    HttpContext hc = HttpContext.Current;
                    try
                    {
                        htPara.Add(cP.GetCuter(0), hc.Server.UrlDecode(cP.GetCuter(1)));
                    }
                    catch
                    { }
                }
            }

            return htPara;
        }

        /// <summary>
        /// 方法：  GetAllURL
        /// 功能：  得到本页面的完全URL，带完全参数
        /// 作者：  齐玮
        /// 修改：  
        /// 时间：  2007-2-2 11:22
        /// </summary>
        /// <returns>返回一个URL地址</returns>
        public static string GetAllURL()
        {
            HttpContext hc = HttpContext.Current;
            string strAllURL = hc.Request.ServerVariables["URL"];
            int intLen = hc.Request.QueryString.Count;
            if (intLen > 0)
            {
                strAllURL += "?";
                for (int i = 0; i < intLen; i++)
                {
                    if (i == 0)
                        strAllURL += hc.Request.QueryString.AllKeys[i] + "=" + hc.Request.QueryString[i];
                    else
                        strAllURL += "&" + hc.Request.QueryString.AllKeys[i] + "=" + hc.Request.QueryString[i];
                }
            }
            return strAllURL;
        }

        /// <summary>
        /// 方法：  JumpToRequestCookiePage
        /// 功能：  跳转到主站以获取Cookie信息。
        ///         要求必须有NeedLogin的URL参数。
        /// 作者：  齐玮
        /// 修改：  
        /// 时间：  2007-2-5 11:09
        /// </summary>
        /// <param name="intNeedLogin">此页面是否需要登录</param>
        public static void JumpToRequestCookiePage(int intNeedLogin)
        {
            string strAllURL = SessionItem.GetAllURL().ToLower();  //获得页面的来路并且将来路变为小写/game/TestCookie.aspx?xxxx
            //?:!    ^:&    .:=
            strAllURL = strAllURL.Replace("?", "!").Replace("&", "^").Replace("=", ".");

            HttpContext hc = HttpContext.Current;
            strAllURL = "http://" + hc.Request.ServerVariables["HTTP_HOST"] + strAllURL;
            hc.Response.Redirect("http://xba.test/RequestCookiePage.aspx?NeedLogin=" + intNeedLogin + "&JumpPage=" + strAllURL);
        }

        /// <summary>
        /// 方法：  JumpToResponseCookiePage
        /// 功能：  在主站判断已经存在登录信息，跳转到向分站请求Cookie的页面。
        ///         要求必须有JumpPage、NeedLogin的URL参数。
        /// 作者：  齐玮
        /// 修改：  
        /// 时间：  2007-2-2 16:09
        /// </summary>
        /// <param name="intUserID">Cookie中的UserID</param>
        /// <param name="intReportParameter">错误报告所需要的参数</param>
        public static void JumpToResponseCookiePage(int intUserID, int intReportParameter)
        {
            string strJumpURL = ((string)SessionItem.GetRequest("JumpPage", 1)).ToLower();  //获得页面的来路并且将来路变为小写
            int intNeedLogin = (int)SessionItem.GetRequest("NeedLogin", 0);

            //?:!    ^:&    .:=
            strJumpURL = strJumpURL.Replace("http://", "").Replace("?", "!").Replace("&", "^").Replace("=", ".");

            Cuter cuter = new Cuter(strJumpURL, "/");

            string strDomain;
            if (DBConnection.IsOnline)
                strDomain = cuter.GetCuter(0);
            else
                strDomain = cuter.GetCuter(0);// +"/" + cuter.GetCuter(1); //两个GetCuter是因为这是在本地，如果传入服务器，只需要1个GetCuter了。

            string strJumpPage = strJumpURL.Replace(strDomain, "");

            if (intUserID > 0)
            {
                string strUserID = StringItem.MD5Encrypt(intUserID.ToString().Trim(), "x1~baK07").ToString().Trim();
                strJumpURL = "http://" + strDomain + "/ResponseCookiePage.aspx?CheckCookie=true&UserID=" + strUserID + "&JumpPage=" + strJumpPage;
            }
            else
            {
                if (intNeedLogin == 0)
                    strJumpURL = "http://" + strDomain + "/ResponseCookiePage.aspx?CheckCookie=false&JumpPage=" + strJumpPage;
                else
                    strJumpURL = "Report.aspx?Parameter=" + intReportParameter;
            }

            HttpContext hc = HttpContext.Current;
            hc.Response.Redirect(strJumpURL);
        }

        /// <summary>
        /// 方法：  JumpToResponseUnionCookiePage
        /// 功能：  在主站判断已经存在登录信息，跳转到向联盟站请求Cookie的页面。
        ///         要求必须有JumpPage、NeedLogin的URL参数。
        /// 作者：  戚凯
        /// 修改：  
        /// 时间：  2007-3-2 15:26
        /// </summary>
        /// <param name="intUserID">Cookie中的UserID</param>
        /// <param name="intReportParameter">错误报告所需要的参数</param>
        public static void JumpToResponseUnionCookiePage(int intUserID, int intReportParameter)
        {
            string strJumpURL = ((string)SessionItem.GetRequest("JumpPage", 1)).ToLower();  //获得页面的来路并且将来路变为小写
            int intNeedLogin = (int)SessionItem.GetRequest("NeedLogin", 0);

            //?:!    ^:&    .:=
            strJumpURL = strJumpURL.Replace("http://", "").Replace("?", "!").Replace("&", "^").Replace("=", ".");

            Cuter cuter = new Cuter(strJumpURL, "/");

            string strDomain;
            if (DBConnection.IsOnline)
                strDomain = cuter.GetCuter(0);
            else
                strDomain = cuter.GetCuter(0);//+"/" + cuter.GetCuter(1); //两个GetCuter是因为这是在本地，如果传入服务器，只需要1个GetCuter了。

            string strJumpPage = strJumpURL.Replace(cuter.GetCuter(0), "");

            if (intUserID > 0)
                strJumpURL = "http://" + cuter.GetCuter(0) + "/Common/ResponseCookiePage.aspx?CheckCookie=true&UserID=" + intUserID + "&JumpPage=" + strJumpPage;
            else
            {
                if (intNeedLogin == 0)
                    strJumpURL = "http://" + strDomain + "/Common/ResponseCookiePage.aspx?CheckCookie=false&JumpPage=" + strJumpPage;
                else
                    strJumpURL = "Report.aspx?Parameter=" + intReportParameter;
            }

            HttpContext hc = HttpContext.Current;
            hc.Response.Redirect(strJumpURL);
        }

        /// <summary>
        /// 方法：  JumpToTargetPage
        /// 功能：  跳转到目标页。
        ///         要求必须有JumpPage的URL参数。
        /// 作者：  齐玮
        /// 修改：  
        /// 时间：  2007-2-2 17:41
        /// </summary>
        public static void JumpToTargetPage()
        {
            string strJumpPage = (string)SessionItem.GetRequest("JumpPage", 1);
            //?:!    ^:&    .:=
            if (strJumpPage.IndexOf("!") != -1)
            {
                strJumpPage = strJumpPage.Replace("!", ",");
                Cuter cuterJumpPage = new Cuter(strJumpPage);
                string strJumpPage1 = cuterJumpPage.GetCuter(0);
                string strJumpPage2 = cuterJumpPage.GetCuter(1).Replace("^", "&").Replace(".", "=");
                strJumpPage = strJumpPage1 + "?" + strJumpPage2;
            }
            strJumpPage = strJumpPage.Replace("/", "");
            HttpContext hc = HttpContext.Current;
            hc.Response.Redirect(strJumpPage);
        }

        /// <summary>
        /// 方法：  GetAllServerVariables
        /// 功能：  得到所有的服务器变量Request.ServerVariables
        /// 作者：  齐玮
        /// 修改：  
        /// 时间：  2007-2-2 16:03
        /// </summary>
        /// <returns>输出一个字符串，列出所有的服务器变量，以及当前服务器的值</returns>
        public static string GetAllServerVariables()
        {
            HttpContext hc = HttpContext.Current;
            string strReturn = "";
            int intLen = hc.Request.ServerVariables.Count;
            for (int i = 0; i < intLen; i++)
            {

                strReturn += hc.Request.ServerVariables.AllKeys[i] + " : " + hc.Request.ServerVariables[i] + "<br />";
            }

            return strReturn;
        }

        /// <summary>
        /// 方法：  GetCookieValue
        /// 功能：  得到Cookie的值
        /// 作者：  齐玮
        /// 修改：  
        /// 时间：  2007-2-5 10:52
        /// </summary>
        /// <param name="strCookie">Cookie</param>
        /// <param name="strCookieItem">Cookie项目</param>
        /// <returns>返回Cookie值，若无此Cookie，则返回null</returns>
        public static string GetCookieValue(string strCookie, string strCookieItem)
        {
            HttpContext hc = HttpContext.Current;

            if (hc.Request.Cookies[strCookie] == null)
                return null;
            else
            {
                if (strCookieItem == "NickName")
                    return HttpUtility.UrlDecode(hc.Request.Cookies[strCookie][strCookieItem]);
                else
                    return hc.Request.Cookies[strCookie][strCookieItem];
            }
        }

        /// <summary>
        /// 方法：  GetCookieValue
        /// 功能：  得到Cookie的值
        /// 作者：  KXT
        /// 修改：  
        /// 时间：  2007-3-27 10:52
        /// </summary>
        /// <param name="strCookie">Cookie</param>
        /// <returns>返回Cookie值，若无此Cookie，则返回null</returns>
        public static string GetCookieValue(string strCookie)
        {
            HttpContext hc = HttpContext.Current;

            if (hc.Request.Cookies[strCookie] == null)
                return null;
            else
                return hc.Request.Cookies[strCookie].Value;
        }
    }
}
