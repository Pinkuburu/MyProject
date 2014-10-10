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

namespace LittleWar_Web
{
    public partial class Test : System.Web.UI.Page
    {
        public string strReturn="";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.strReturn = "1112";
            int a = 1;
            int b = 0;
            int c = 0;
            try
            {
                c = a / b;
            }
            catch (Exception ex)
            {
                this.strReturn += ex.ToString().Trim();
            }
        }
    }
}
