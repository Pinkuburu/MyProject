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
using QQRobot_Module;

namespace QQRobot_InterFace_vs2005
{
    public partial class OnlineMax : System.Web.UI.Page
    {
        public string strContent = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            ServerCheck sc = new ServerCheck();
            Response.Write(sc.GetOnlineMax());
        }
    }
}
