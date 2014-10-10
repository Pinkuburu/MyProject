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
        /// ��ʽ������
        /// </summary>
        /// <param name="strIndex">���������</param>
        /// <param name="intType">���������0:���� 1:�ַ��� 2:������ 3:������</param>
        /// <returns>����ֵ</returns>
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
        /// ������  GetAllURL
        /// ���ܣ�  �õ���ҳ�����ȫURL������ȫ����
        /// ���ߣ�  ����
        /// �޸ģ�  
        /// ʱ�䣺  2007-2-2 11:22
        /// </summary>
        /// <returns>����һ��URL��ַ</returns>
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
        /// ������  JumpToRequestCookiePage
        /// ���ܣ�  ��ת����վ�Ի�ȡCookie��Ϣ��
        ///         Ҫ�������NeedLogin��URL������
        /// ���ߣ�  ����
        /// �޸ģ�  
        /// ʱ�䣺  2007-2-5 11:09
        /// </summary>
        /// <param name="intNeedLogin">��ҳ���Ƿ���Ҫ��¼</param>
        public static void JumpToRequestCookiePage(int intNeedLogin)
        {
            string strAllURL = SessionItem.GetAllURL().ToLower();  //���ҳ�����·���ҽ���·��ΪСд/game/TestCookie.aspx?xxxx
            //?:!    ^:&    .:=
            strAllURL = strAllURL.Replace("?", "!").Replace("&", "^").Replace("=", ".");

            HttpContext hc = HttpContext.Current;
            strAllURL = "http://" + hc.Request.ServerVariables["HTTP_HOST"] + strAllURL;
            hc.Response.Redirect("http://xba.test/RequestCookiePage.aspx?NeedLogin=" + intNeedLogin + "&JumpPage=" + strAllURL);
        }

        /// <summary>
        /// ������  JumpToResponseCookiePage
        /// ���ܣ�  ����վ�ж��Ѿ����ڵ�¼��Ϣ����ת�����վ����Cookie��ҳ�档
        ///         Ҫ�������JumpPage��NeedLogin��URL������
        /// ���ߣ�  ����
        /// �޸ģ�  
        /// ʱ�䣺  2007-2-2 16:09
        /// </summary>
        /// <param name="intUserID">Cookie�е�UserID</param>
        /// <param name="intReportParameter">���󱨸�����Ҫ�Ĳ���</param>
        public static void JumpToResponseCookiePage(int intUserID, int intReportParameter)
        {
            string strJumpURL = ((string)SessionItem.GetRequest("JumpPage", 1)).ToLower();  //���ҳ�����·���ҽ���·��ΪСд
            int intNeedLogin = (int)SessionItem.GetRequest("NeedLogin", 0);

            //?:!    ^:&    .:=
            strJumpURL = strJumpURL.Replace("http://", "").Replace("?", "!").Replace("&", "^").Replace("=", ".");

            Cuter cuter = new Cuter(strJumpURL, "/");

            string strDomain;
            if (DBConnection.IsOnline)
                strDomain = cuter.GetCuter(0);
            else
                strDomain = cuter.GetCuter(0);// +"/" + cuter.GetCuter(1); //����GetCuter����Ϊ�����ڱ��أ���������������ֻ��Ҫ1��GetCuter�ˡ�

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
        /// ������  JumpToResponseUnionCookiePage
        /// ���ܣ�  ����վ�ж��Ѿ����ڵ�¼��Ϣ����ת��������վ����Cookie��ҳ�档
        ///         Ҫ�������JumpPage��NeedLogin��URL������
        /// ���ߣ�  �ݿ�
        /// �޸ģ�  
        /// ʱ�䣺  2007-3-2 15:26
        /// </summary>
        /// <param name="intUserID">Cookie�е�UserID</param>
        /// <param name="intReportParameter">���󱨸�����Ҫ�Ĳ���</param>
        public static void JumpToResponseUnionCookiePage(int intUserID, int intReportParameter)
        {
            string strJumpURL = ((string)SessionItem.GetRequest("JumpPage", 1)).ToLower();  //���ҳ�����·���ҽ���·��ΪСд
            int intNeedLogin = (int)SessionItem.GetRequest("NeedLogin", 0);

            //?:!    ^:&    .:=
            strJumpURL = strJumpURL.Replace("http://", "").Replace("?", "!").Replace("&", "^").Replace("=", ".");

            Cuter cuter = new Cuter(strJumpURL, "/");

            string strDomain;
            if (DBConnection.IsOnline)
                strDomain = cuter.GetCuter(0);
            else
                strDomain = cuter.GetCuter(0);//+"/" + cuter.GetCuter(1); //����GetCuter����Ϊ�����ڱ��أ���������������ֻ��Ҫ1��GetCuter�ˡ�

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
        /// ������  JumpToTargetPage
        /// ���ܣ�  ��ת��Ŀ��ҳ��
        ///         Ҫ�������JumpPage��URL������
        /// ���ߣ�  ����
        /// �޸ģ�  
        /// ʱ�䣺  2007-2-2 17:41
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
        /// ������  GetAllServerVariables
        /// ���ܣ�  �õ����еķ���������Request.ServerVariables
        /// ���ߣ�  ����
        /// �޸ģ�  
        /// ʱ�䣺  2007-2-2 16:03
        /// </summary>
        /// <returns>���һ���ַ������г����еķ������������Լ���ǰ��������ֵ</returns>
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
        /// ������  GetCookieValue
        /// ���ܣ�  �õ�Cookie��ֵ
        /// ���ߣ�  ����
        /// �޸ģ�  
        /// ʱ�䣺  2007-2-5 10:52
        /// </summary>
        /// <param name="strCookie">Cookie</param>
        /// <param name="strCookieItem">Cookie��Ŀ</param>
        /// <returns>����Cookieֵ�����޴�Cookie���򷵻�null</returns>
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
        /// ������  GetCookieValue
        /// ���ܣ�  �õ�Cookie��ֵ
        /// ���ߣ�  KXT
        /// �޸ģ�  
        /// ʱ�䣺  2007-3-27 10:52
        /// </summary>
        /// <param name="strCookie">Cookie</param>
        /// <returns>����Cookieֵ�����޴�Cookie���򷵻�null</returns>
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
