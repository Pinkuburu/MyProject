using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Data.SqlClient;

namespace Show_Web
{
    public partial class Refresh : System.Web.UI.Page
    {
        public StringBuilder sb = new StringBuilder();
        public string strContent = "";
        public int intPageSize = 8;

        protected void Page_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            ArrayList alBmp = new ArrayList();
            ArrayList alUid = new ArrayList();
            int intUserID = 0;
            intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 0));

            if (intUserID > 0)
            {
                Session["UserID"] = intUserID;
            }
            else
            {
                Response.Redirect("Logins.aspx");
                return;
            }

            int intPage = Convert.ToInt32(Request.QueryString["Page"].ToString());

            if (intPage < 0 || !intPage.Equals(intPage))
            {
                intPage = 1;
            }

            SqlParameter[] sp = new SqlParameter[2];
            sp[0] = new SqlParameter("@PageIndex", SqlDbType.Int, 4);
            sp[1] = new SqlParameter("@PageSize", SqlDbType.Int, 4);
            sp[0].Value = intPage;
            sp[1].Value = 8;

            DataTable dt = SqlHelper.ExecuteDataTable(SqlLibrary.GetServer_Main(), CommandType.StoredProcedure, "Xn_GetShowList", sp);
            DataTable dtConcernList = SqlLibrary.ReadConcernList(intUserID);
            if (dt != null)
            {
                string strSQL = "SELECT ISNULL(Count(id),0) AS Count FROM [MT_Media]";
                this.intPageSize = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlLibrary.GetServer_Main(), CommandType.Text, strSQL));
                if (this.intPageSize % 8 > 0)
                {
                    this.intPageSize = this.intPageSize / 8 + 1;
                }
                else
                {
                    this.intPageSize = this.intPageSize / 8;
                }

                int i = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    int intUserIDT = (int)dr["UserId"];
                    DateTime dtnow = DateTime.Now;
                    DateTime Birthday = (DateTime)dr["Birthday"];
                    this.sb.Append("<div class=\"pic" + i + "\">");
                    if (Convert.ToString((bool)dr["Online"]) == "True")
                    {
                        this.sb.Append("<div class=\"headpic\"><a href=\"javascript:;\" onclick=\"ShowMsgBox(" + (int)dr["UserId"] + ",'" + (string)dr["NickName"].ToString().Trim() + "')\" title=\"昵称:" + (string)dr["NickName"].ToString() + "&#13;地区:" + (string)dr["Province"].ToString() + " " + (string)dr["City"].ToString() + "&#13;年龄:" + Convert.ToString(dtnow.Year - Birthday.Year) + "&#13;性别:" + (Convert.ToString((bool)dr["Sex"]) == "True" ? "男" : "女") + "\"><img src=\"http://ishow.xba.com.cn/image/" + (string)dr["Bmp"].ToString().Trim() + "?tmp=" + rnd.Next(000000, 999999).ToString() + "\" width=\"150\" height=\"130\" alt=\"昵称:" + (string)dr["NickName"].ToString() + "&#13;地区:" + (string)dr["Province"].ToString() + " " + (string)dr["City"].ToString() + "&#13;年龄:" + Convert.ToString(dtnow.Year - Birthday.Year) + "&#13;性别:" + (Convert.ToString((bool)dr["Sex"]) == "True" ? "男" : "女") + "\"/></a></div>");
                        //this.sb.Append("<div class=\"headpic\"><a href=\"#\" title='" + (string)dr["NickName"].ToString().Trim() + "'><img src=\"http://ishow.xba.com.cn/image/" + (string)dr["Bmp"].ToString().Trim() + "?tmp=" + rnd.Next(000000, 999999).ToString() + "\" width=\"150\" height=\"130\" alt='" + (string)dr["NickName"].ToString().Trim() + "' /></a></div>");
                    }
                    else
                    {
                        this.sb.Append("<div class=\"headpic\"><a href=\"javascript:;\" onclick=\"ShowMsgBox(" + (int)dr["UserId"] + ",'" + (string)dr["NickName"].ToString().Trim() + "')\" title=\"昵称:" + (string)dr["NickName"].ToString() + "&#13;地区:" + (string)dr["Province"].ToString() + " " + (string)dr["City"].ToString() + "&#13;年龄:" + Convert.ToString(dtnow.Year - Birthday.Year) + "&#13;性别:" + (Convert.ToString((bool)dr["Sex"]) == "True" ? "男" : "女") + "\"><img src=\"http://ishow.xba.com.cn/image/" + (string)dr["Bmp"].ToString().Trim() + "\" width=\"150\" height=\"130\" alt=\"昵称:" + (string)dr["NickName"].ToString() + "&#13;地区:" + (string)dr["Province"].ToString() + " " + (string)dr["City"].ToString() + "&#13;年龄:" + Convert.ToString(dtnow.Year - Birthday.Year) + "&#13;性别:" + (Convert.ToString((bool)dr["Sex"]) == "True" ? "男" : "女") + "\"/></a></div>");
                    }
                    this.sb.Append("<div class=\"show_text\">");
                    this.sb.Append("<div class=\"headname\"><a href=\"#\" title='" + (string)dr["NickName"].ToString().Trim() + "'>" + StringItem.StringTruncat((string)dr["NickName"].ToString().Trim(), 10, "...") + "</a></div>");
                    this.sb.Append("<div class=\"headtime\">" + StringItem.FormatDate((DateTime)dr["CreateTime"], "hh:mm") + "</div>");

                    if (intUserID == intUserIDT)
                    {
                        this.sb.Append("<div class=\"sms\"></div>");
                    }
                    else
                    {
                        this.sb.Append("<div class=\"sms\"><a href=\"javascript:;\" onclick=\"ShowMsgBox(" + (int)dr["UserId"] + ",'" + (string)dr["NickName"].ToString().Trim() + "')\" ><img src=\"Images/pao.jpg\" alt=\"联系我\" onMouseOver='this.src=\"Images/pao_hover.jpg\"' onMouseOut='this.src=\"Images/pao.jpg\"' /></a></div>");
                    }

                    if (dtConcernList != null)
                    {
                        DataRow[] drs = dtConcernList.Select("ConcernID=" + intUserIDT);
                        if (drs != null && drs.Length > 0)
                        {
                            this.sb.Append("<div class=\"headadd\"><a href=\"javascript:;\" onclick=\"DeleteConcern(" + intUserID + "," + intUserIDT + ")\" ><img src=\"Images/cut.jpg\" alt=\"取消对该用户的关注\" onMouseOver='this.src=\"Images/cut_hover.jpg\"' onMouseOut='this.src=\"Images/cut.jpg\"' /></a></div>");
                        }
                        else
                        {
                            if (intUserID == intUserIDT)
                            {
                                this.sb.Append("<div class=\"headadd\"></div>");
                            }
                            else
                            {
                                this.sb.Append("<div class=\"headadd\"><a href=\"javascript:;\" onclick=\"AddConcern(" + intUserID + "," + intUserIDT + ")\" ><img src=\"Images/add.jpg\" alt=\"添加对该用户的关注\" onMouseOver='this.src=\"Images/add_hover.jpg\"' onMouseOut='this.src=\"Images/add.jpg\"' /></a></div>");
                            }
                        }
                        dtConcernList.Dispose();
                    }
                    else
                    {
                        if (intUserID == intUserIDT)
                        {
                            this.sb.Append("<div class=\"headadd\"></div>");
                        }
                        else
                        {
                            this.sb.Append("<div class=\"headadd\"><a href=\"javascript:;\" onclick=\"AddConcern(" + intUserID + "," + intUserIDT + ")\" ><img src=\"Images/add.jpg\" alt=\"添加对该用户的关注\" onMouseOver='this.src=\"Images/add_hover.jpg\"' onMouseOut='this.src=\"Images/add.jpg\"'></a></div>");
                        }
                    }
                    this.sb.Append("</div>\r\n");
                    this.sb.Append("</div>\r\n");
                    i++;
                }
                //ShowImg.Visible = true;
                this.strContent = this.sb.ToString();
                dt.Dispose();
            }
            else
            {
                this.strContent = "当前没有在线用户~~~";
            }        
        }
    }
}
