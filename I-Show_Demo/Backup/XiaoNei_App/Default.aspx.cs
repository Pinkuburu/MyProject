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

namespace XiaoNei_App
{
    public partial class _Default : System.Web.UI.Page
    {
        string strSessionKey = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            strSessionKey = Request.QueryString["xn_sig_session_key"].ToString();
            strSessionKey = Server.UrlEncode(strSessionKey);
            Response.Write(strSessionKey);
        }
    }
}
