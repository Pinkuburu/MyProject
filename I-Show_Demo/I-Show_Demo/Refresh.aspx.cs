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
using System.Data.SqlClient;

namespace I_Show_Demo
{
    public partial class Refresh : System.Web.UI.Page
    {
        public StringBuilder sb = new StringBuilder();
        public string strContent = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            //string strUserName = "Cupid";
            ArrayList alBmp = new ArrayList();
            ArrayList alUid = new ArrayList();

            //Session["username"] = strUserName;
            //if (Session["username"] == null)
            //{
            //    Response.Redirect("Login.aspx");
            //}

            string strSQL = "SELECT UserId,Bmp FROM [Media] ORDER BY Time DESC";
            SqlDataReader dr = SqlHelper.ExecuteReader(SqlLibrary.GetServer_Main(), CommandType.Text, strSQL);
            
            if (dr != null)
            {
                int j = 0;
                sb.Append("<table id=\"tb_ShowImg\">");
                sb.Append("<tr>");
                while (dr.Read())
                {
                    sb.Append("<td align=\"center\"><table><tr><td style='border-collapse:collapse;border: 1px solid #20B2AA;width:160px;height:160px;'><img src=\"./image/" + (string)dr["Bmp"].ToString().Trim() + "?tmp=" + rnd.Next(000000, 999999).ToString() + "\"/></td></tr><tr><td>" + (string)dr["UserId"].ToString().Trim() + "</td></tr></table></td>");
                    j++;
                    if (j == 3)
                    {
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        j = 0;
                    }
                }
                sb.Append("</tr>");
                dr.Close();
                sb.Append("</table>");
                this.strContent = sb.ToString();
            }
            else
            {
                this.strContent = "当前没有在线用户~~~";
            }
            
        }
    }
}
