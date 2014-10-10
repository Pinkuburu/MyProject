using System;
using System.Collections;
using System.Data;
using System.Text;

namespace Show_Web
{
    public partial class FansList : System.Web.UI.Page
    {
        public StringBuilder sb = new StringBuilder();
        public string strContent = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();

            int intUserID = Convert.ToInt32(Session["UserID"]);
            int intPage = Convert.ToInt32(Request.QueryString["Page"].ToString());
            intUserID = Convert.ToInt32(Request.QueryString["UserID"].ToString());

            if (intPage < 0 || !intPage.Equals(intPage))
            {
                intPage = 1;
            }

            DataTable dt = SqlLibrary.ShowFansList(intPage, intUserID);
            if (dt != null)
            {
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
                        this.sb.Append("<div class=\"headadd\"><img src=\"Images/add.jpg\" onMouseOver='this.src=\"Images/add_hover.jpg\"' onMouseOut='this.src=\"Images/add.jpg\"' /></div>");
                    }
                    else
                    {
                        this.sb.Append("<div class=\"sms\"><a href=\"javascript:;\" onclick=\"ShowMsgBox(" + (int)dr["UserId"] + ",'" + (string)dr["NickName"].ToString().Trim() + "')\" ><img src=\"Images/pao.jpg\" alt=\"联系我\" onMouseOver='this.src=\"Images/pao_hover.jpg\"' onMouseOut='this.src=\"Images/pao.jpg\"' /></a></div>");
                        this.sb.Append("<div class=\"headadd\"><a href=\"javascript:;\" onclick=\"AddConcern(" + intUserID + "," + intUserIDT + ")\" ><img src=\"Images/add.jpg\" alt=\"添加对该用户的关注\" onMouseOver='this.src=\"Images/add_hover.jpg\"' onMouseOut='this.src=\"Images/add.jpg\"' /></a></div>");
                    }
                    this.sb.Append("</div>\r\n");
                    this.sb.Append("</div>\r\n");
                    i++;
                }
                this.strContent = this.sb.ToString();
                dt.Dispose();
            }
            else
            {
                this.strContent = "很遗憾，还没有人成为你的粉丝，你需要散发出更多的魅力呦O(∩_∩)O~";
            }
        }
    }
}
