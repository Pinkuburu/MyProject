using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

namespace XiaoNei_App
{
    public partial class _Default : System.Web.UI.Page
    {
        string strSessionKey = null;
        string strRegApp = null;
        StringBuilder sb = new StringBuilder();
        public string strContent = null;
        public string strMsg = null;
        UserInfo uInfo = new UserInfo();
        XiaoNeiApi xn = new XiaoNeiApi();

        protected void Page_Load(object sender, EventArgs e)
        {
            //tb_DebugSessionKey.Visible = true;//debug
            tb_DebugSessionKey.Visible = false;//release
            //div_FirstLogin.Visible = true;//debug
            strRegApp = Request.QueryString["xn_sig_added"].ToString();
            if (strRegApp == "0")
            {
                if (Request.QueryString["xn_sig_api_key"].ToString() == "2c3dae2f4a494b7898b8dd361783f8e2")
                {
                    sb.Append("<script type=\"text/javascript\" src=\"http://static.connect.renren.com/js/v1.0/FeatureLoader.jsp\"></script>" + "\r\n");
                    sb.Append("<script type=\"text/javascript\">" + "\r\n");
                    sb.Append("XN_RequireFeatures([\"EXNML\"], function()" + "\r\n");
                    sb.Append("{" + "\r\n");
                    sb.Append("XN.Main.init(\"2c3dae2f4a494b7898b8dd361783f8e2\", \"/xn/xd_receiver.htm\");" + "\r\n");
                    sb.Append("var callback = function(){ top.location.href = \"http://apps.renren.com/qweqweabc\"; }" + "\r\n");
                    sb.Append("var cancel = function(){ alert('Authorize Failed!'); }" + "\r\n");
                    sb.Append("XN.Connect.showAuthorizeAccessDialog(callback,cancel);" + "\r\n");
                    sb.Append("});" + "\r\n");
                    sb.Append("</script>");
                }
                else if (Request.QueryString["xn_sig_api_key"].ToString() == "e987f59f192b44cd81c4b6dd9d37b100")
                {
                    sb.Append("<script type=\"text/javascript\" src=\"http://static.connect.renren.com/js/v1.0/FeatureLoader.jsp\"></script>" + "\r\n");
                    sb.Append("<script type=\"text/javascript\">" + "\r\n");
                    sb.Append("XN_RequireFeatures([\"EXNML\"], function()" + "\r\n");
                    sb.Append("{" + "\r\n");
                    sb.Append("XN.Main.init(\"e987f59f192b44cd81c4b6dd9d37b100\", \"xd_receiver.htm\");" + "\r\n");
                    sb.Append("var callback = function(){ top.location.href = \"http://apps.renren.com/teamshow/\"; }" + "\r\n");
                    sb.Append("var cancel = function(){ alert('Authorize Failed!'); }" + "\r\n");
                    sb.Append("XN.Connect.showAuthorizeAccessDialog(callback,cancel);" + "\r\n");
                    sb.Append("});" + "\r\n");
                    sb.Append("</script>" + "\r\n");
                }
                this.strContent = sb.ToString();
            }
            else
            {
                strSessionKey = Request.QueryString["xn_sig_session_key"].ToString();
                if (strSessionKey != "")
                {
                    strSessionKey = Server.UrlEncode(strSessionKey);
                    xn.strSessionKey = strSessionKey;
                    //Response.Write(strSessionKey + "<br>");
                    uInfo.Uid = xn.users_getLoggedInUser();
                    //Response.Write(uInfo.Uid + "<br>");
                    if (xn.CheckFirstLogin(uInfo.Uid) == "1")
                    {
                        div_FirstLogin.Visible = true;
                    }
                    else
                    {
                        div_FirstLogin.Visible = false;
                        Response.Redirect("Show.aspx?xn_sig_session_key=" + xn.strSessionKey + "&page=1");
                    }
                    //Response.Write(xn.users_getInfo(uInfo.Uid) + "<br>");
                    //Response.Write(xn.friends_getFriends());
                }
            }
        }

        protected void btn_Send_Click(object sender, EventArgs e)
        {
            //string strSessionKey = tb_DebugSessionKey.Text.ToString().Trim();
            string strPassword = tb_Password.Text.ToString().Trim();

            if (strSessionKey != "")
            {
                strSessionKey = Server.UrlEncode(strSessionKey);
                xn.strSessionKey = strSessionKey;
                //Response.Write(strSessionKey + "<br>");
                uInfo.Uid = xn.users_getLoggedInUser();
                //Response.Write(uInfo.Uid + "<br>");
                //Response.Write(xn.friends_getAppFriends());
                //Response.Write(xn.users_getInfo(uInfo.Uid));
                if (xn.CheckFirstLogin(uInfo.Uid) == "1")
                {
                    div_FirstLogin.Visible = true;
                }
                else
                {
                    div_FirstLogin.Visible = false;
                }

                if (strPassword.Length > 20)
                {
                    this.strMsg = "<font color='red'>密码长度不能大于20</font>";
                }
                else if (strPassword == "")
                {
                    this.strMsg = "<font color='red'>密码不能为空，请输入密码，这点对您以后的应用很重要，谢谢。</font>";
                }
                else if (strPassword.Length < 10 && strPassword != "")
                {
                    string strStatus = xn.users_getInfo(uInfo.Uid, strPassword);
                    if (strStatus == "1")
                    {
                        div_FirstLogin.Visible = false;
                        this.strMsg = "恭喜你，激活成功，你可以尽情的HIGH了~~~" + "<br>";
                        this.strMsg += "请记录您的登录ID和密码，您的登录ID为:" + uInfo.Uid + ", 密码为:" + tb_Password.Text.ToString() + "<br>";
                        this.strMsg += "请下载客户端，用以上ID和密码登录该客户端" + "<br>";
                        this.strMsg += "客户端下载：<a href=\"http://ishow.xba.com.cn/image/update/PlayCap.rar\">下载</a>" + "<br>";
                        this.strMsg += "客户端下载成功后，请刷新此页面!";
                    }
                    else if (strStatus == "0")
                    {
                        div_FirstLogin.Visible = false;
                        this.strMsg = "<font color='red'>您已经激活过了，您是怎么再激活的？</font>";
                    }
                    else if (strStatus == "-1")
                    {
                        div_FirstLogin.Visible = true;
                        this.strMsg = "<font color='red'>请求超时请稍后重试</font>";
                    }
                    else
                    {
                        div_FirstLogin.Visible = false;
                        this.strMsg = "<font color='red'>终于出现程序也无法判定的错误了，还是联系一下管理员吧</font>";
                    }
                }
            }
        }
    }
}
