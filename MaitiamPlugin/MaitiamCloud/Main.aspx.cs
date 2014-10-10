using System;
using System.Web;
using System.Text;
using System.Data;

namespace MaitiamCloud
{
    public partial class Main : System.Web.UI.Page
    {
        public int intAdminID = 0;
        public string strContent = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie mycookie = Request.Cookies["adminid"];
            this.intAdminID = Convert.ToInt32(mycookie.Value);
            //this.intAdminID = (int)Session["AdminID"];
            if (this.intAdminID == 0)
            {
                Response.Redirect("Default.aspx");
            }   
        }

        protected void btnShowUser_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = UserManager.ShowUser(intAdminID);
            if (dt != null)
            {
                sb.Append("<table class=\"warp_table\" border=\"1\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">");
                sb.Append("<tr align=\"center\">");
                sb.Append("<td>[UserName]</td><td>[Password]</td><td>[Status]</td><td>[UserGroup]</td><td>[Weixin]</td><td>[QQ]</td><td>[Email]</td><td>[CreateTime]</td>");
                sb.Append("</tr>");
                foreach (DataRow dr in dt.Rows)
                {
                    //[UserName],[Password],[Status],[UserGroup],[Weixin],[QQ],[Email],[CreateTime]
                    sb.Append("<tr>");
                    sb.Append("<td>" + dr["UserName"] + "</td><td>" + dr["Password"] + "</td><td>" + dr["Status"] + "</td><td>" + dr["GroupName"] + "</td><td>" + dr["Weixin"] + "</td><td>" + dr["QQ"] + "</td><td>" + dr["Email"] + "</td><td>" + dr["CreateTime"] + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                this.strContent = sb.ToString();
            }
        }
    }
}