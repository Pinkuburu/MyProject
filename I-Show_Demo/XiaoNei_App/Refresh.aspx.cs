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

namespace XiaoNei_App
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

            int intPage = Convert.ToInt32(Request.QueryString["Page"].ToString());

            if (intPage < 0 || !intPage.Equals(intPage))
            {
                intPage = 1;
            }
            //Session["username"] = strUserName;
            //if (Session["username"] == null)
            //{
            //    Response.Redirect("Login.aspx");
            //}

            SqlParameter[] sp = new SqlParameter[2];
            sp[0] = new SqlParameter("@PageIndex", SqlDbType.Int, 4);
            sp[1] = new SqlParameter("@PageSize", SqlDbType.Int, 4);
            sp[0].Value = intPage;
            sp[1].Value = 8;

            DataTable dt = SqlHelper.ExecuteDataTable(SqlLibrary.GetXn_Main(), CommandType.StoredProcedure, "Xn_GetShowList", sp);
            if (dt.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("<div class=\"pic" + i + "\">");
                    sb.Append("<div class=\"headpic\"><a href=\"#\"><img src=\"http://ishow.xba.com.cn/image/" + (string)dr["Bmp"].ToString().Trim() + "?tmp=" + rnd.Next(000000, 999999).ToString() + "\" width=\"149\" height=\"115\" /></a></div>");
                    sb.Append("<div class=\"headname\"><a href=\"#\">" + (string)dr["UserId"].ToString().Trim() + "</a></div>");
                    sb.Append("<div class=\"headtime\">16:20</div>");
                    sb.Append("</div>\r\n");
                    i++;
                }
                //ShowImg.Visible = true;
                this.strContent = sb.ToString();
                dt.Dispose();
            }
            else
            {
                strContent = "当前没有在线用户~~~";
            }                

            //string strSQL = "SELECT UserId,Bmp FROM [Media] ORDER BY Time DESC";
            //SqlDataReader dr = SqlHelper.ExecuteReader(SqlLibrary.GetXn_Main(), CommandType.Text, strSQL);

            //if (dr != null)
            //{
            //    int i = 1;
            //    //int j = 0;
            //    while (dr.Read())
            //    {
            //        sb.Append("<div class=\"pic" + i + "\">");
            //        sb.Append("<div class=\"headpic\"><a href=\"#\"><img src=\"http://ishow.xba.com.cn/image/" + (string)dr["Bmp"].ToString().Trim() + "?tmp=" + rnd.Next(000000, 999999).ToString() + "\" width=\"149\" height=\"115\" /></a></div>");
            //        sb.Append("<div class=\"headname\"><a href=\"#\">" + (string)dr["UserId"].ToString().Trim() + "</a></div>");
            //        sb.Append("<div class=\"headtime\">16:20</div>");
            //        sb.Append("</div>\r\n");
            //        //j++;
            //        //if (j == 3)
            //        //{
            //        //    sb.Append("");
            //        //    j = 0;
            //        //}
            //        i++;
            //    }
            //    dr.Close();
            //    //ShowImg.Visible = true;
            //    this.strContent = sb.ToString();
            //    dr.Dispose();
            //}
            //else
            //{
            //    strContent = "当前没有在线用户~~~";
            //}            
        }
    }
}
