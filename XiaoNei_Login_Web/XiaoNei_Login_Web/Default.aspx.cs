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

namespace XiaoNei_Login_Web
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataBaseitem.IsOnline = false;
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            Response.Write(DataBaseitem.GetLoginConn());
        }
    }
}
