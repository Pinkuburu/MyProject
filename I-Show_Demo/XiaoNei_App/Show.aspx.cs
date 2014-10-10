using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;

namespace XiaoNei_App
{
    public partial class Show : System.Web.UI.Page
    {
        public StringBuilder sb = new StringBuilder();
        public StringBuilder sb_FriendList = new StringBuilder();
        public StringBuilder sb_Page = new StringBuilder();
        UserInfo uInfo = new UserInfo();

        public string strContent = null;
        public string strFriend_List = null;
        public string strPageShow = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            string strSessionKey = null;
            string[] arrFriend_List;
            string tinyurl = null;
            string name = null;
            int intPage = 1;
            int intPageCount = 0;

            ShowImg.Visible = false;
            //string strUserName = "Cupid";
            ArrayList alBmp = new ArrayList();
            ArrayList alUid = new ArrayList();
            XiaoNeiApi xn = new XiaoNeiApi();

            strSessionKey = Request.QueryString["xn_sig_session_key"].ToString();
            intPage = Convert.ToInt32(Request.QueryString["page"].ToString());

            if (intPage < 0 || !intPage.Equals(intPage))
            {
                intPage = 1;
            }

            xn.strSessionKey = strSessionKey;
            arrFriend_List = xn.friends_getAppFriends().Split('|');
            uInfo.Uid = xn.users_getLoggedInUser();

            JObject o = JObject.Parse(xn.users_getInfo(uInfo.Uid));
            tinyurl = (string)o["tinyurl"];
            name = (string)o["name"];
            sb_FriendList.Append("<li class=\"headbg\"><a href=\"#\"><img src='" + tinyurl + "' width=\"50\" height=\"50\" /></a><br /><a href=\"#\">" + name + "</a></li>");

            if (arrFriend_List.Length > 0 && arrFriend_List[0].ToString() != "")
            {
                foreach (string strContent in arrFriend_List)
                {
                    o = JObject.Parse(strContent);
                    tinyurl = (string)o["tinyurl"];
                    name = (string)o["name"];
                    //sb_FriendList.Append("<li style='float:left;'><img src='" + tinyurl + "' width=\"50\" height=\"50\" alt='" + name + "' /></li>");
                    sb_FriendList.Append("<li class=\"headbg\"><a href=\"#\"><img src='" + tinyurl + "' width=\"50\" height=\"50\" /></a><br /><a href=\"#\">" + name + "</a></li>");
                }
            }
            
            strFriend_List = sb_FriendList.ToString();

            string strSQL = "SELECT ISNULL(Count(id),0) AS Count FROM [Media]";
            intPageCount = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlLibrary.GetXn_Main(), CommandType.Text, strSQL));
            if (intPageCount % 8 > 0)
            {
                intPageCount = intPageCount / 8 + 1;
            }
            else
            {
                intPageCount = intPageCount / 8;
            }

            if (intPageCount > 0)
            {
                int j;
                sb_Page.Append("<ul style=\"list-style:none; margin:0 auto; width:" + 15 * intPageCount + "px;\">");
                for (j = 1; j <= intPageCount; j++)
                {
                    if (j == intPage)
                    {
                        //sb_Page.Append("<li class=\"pagebutton\" onclick=\"highlight(this,'pagebutton')\"><a href=\"javascript:;\" onclick=\"vGetRands(" + j + ")\"></a></li>");//<a href=\"Show.aspx?xn_sig_session_key=" + xn.strSessionKey + "&page=" + j + "\" style=\"display:block; width:15px; height:15px;\"></a>
                        sb_Page.Append("<li class=\"pagebutton\" onclick=\"highlight(this,'pagebutton')\"><a href=\"javascript:;\" onclick=\"vGetRands(" + j + ")\" style=\"display:block; width:15px; height:15px;\" ></a></li>");//<a href=\"Show.aspx?xn_sig_session_key=" + xn.strSessionKey + "&page=" + j + "\" style=\"display:block; width:15px; height:15px;\"></a>
                    }
                    else
                    {
                        sb_Page.Append("<li class=\"pg2\" onclick=\"highlight(this,'pagebutton')\"><a href=\"javascript:;\" onclick=\"vGetRands(" + j + ")\" style=\"display:block; width:15px; height:15px;\" ></a></li>");//<a href=\"Show.aspx?xn_sig_session_key=" + xn.strSessionKey + "&page=" + j + "\" style=\"display:block; width:15px; height:15px;\"></a>
                    }                    
                }
                sb_Page.Append("</ul>");
                this.strPageShow = sb_Page.ToString();

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
                    ShowImg.Visible = true;
                    this.strContent = sb.ToString();
                    dt.Dispose();
                }
                else
                {
                    strContent = "当前没有在线用户~~~";
                }                
            }            

            //string strSQL = "SELECT UserId,Bmp FROM [Media] ORDER BY Time DESC";
            //SqlDataReader dr = SqlHelper.ExecuteReader(SqlLibrary.GetXn_Main(), CommandType.Text, strSQL);
            //if (dr != null)
            //{
            //    int j = 0;
            //    sb.Append("<tr>");
            //    while (dr.Read())
            //    {
            //        //sb.Append("<td align=\"center\"><table><tr><td style='border-collapse:collapse;border: 1px solid #20B2AA;width:160px;height:160px;'><img src=\"./image/" + (string)dr["Bmp"].ToString().Trim() + "?tmp=" + rnd.Next(000000,999999).ToString() +"\"/></td></tr><tr><td>" + (string)dr["UserId"].ToString().Trim() + "</td></tr></table></td>");
            //        sb.Append("<td align=\"center\"><table><tr><td style='border-collapse:collapse;border: 1px solid #20B2AA;width:160px;height:160px;'><img src=\"http://ishow.xba.com.cn/image/" + (string)dr["Bmp"].ToString().Trim() + "?tmp=" + rnd.Next(000000, 999999).ToString() + "\"/></td></tr><tr><td>" + (string)dr["UserId"].ToString().Trim() + "</td></tr></table></td>");
            //        j++;
            //        if (j == 3)
            //        {
            //            sb.Append("</tr>");
            //            sb.Append("<tr>");
            //            j = 0;
            //        }
            //    }
            //    sb.Append("</tr>");
            //    dr.Close();
            //    ShowImg.Visible = true;
            //    strContent = sb.ToString();
            //}
            //else
            //{
            //    strContent = "当前没有在线用户~~~";
            //}            
        }
    }
}