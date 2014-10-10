using System;
using System.Collections;
using System.Data;
using System.Text;

namespace Show_Web
{
    public partial class Search : System.Web.UI.Page
    {
        public StringBuilder sb = new StringBuilder();
        public string strContent = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            ArrayList alBmp = new ArrayList();
            ArrayList alUid = new ArrayList();
            DataTable dt;

            int intType = Convert.ToInt32(Request.QueryString["Type"].ToString().Trim());
            int intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 0));

            if (intType < 0 || intType > 2 || !intType.Equals(intType))
            {
                intType = 1;
            }

            switch (intType)
            {
                case 1:
                    int intPage = Convert.ToInt32(Request.QueryString["Page"].ToString().Trim());
                    string strProvince = Request.QueryString["Province"].ToString().Trim();                 //省
                    string strCity = Request.QueryString["City"].ToString().Trim();                         //市
                    int intGender = Convert.ToInt32(Request.QueryString["Gender"].ToString().Trim());       //性别 1男、0女、2全部
                    int intCategory = Convert.ToInt32(Request.QueryString["Category"].ToString().Trim());   //0 全部,1 小于15岁,2 16-22岁,3 23-30岁,4 31-40岁,5 大于40岁

                    if (intPage < 0 || !intPage.Equals(intPage))
                    {
                        intPage = 1;
                    }

                    dt = SqlLibrary.SearchUserList(intPage, strProvince, strCity, intGender, intCategory);
                    DataTable dtConcernList = SqlLibrary.ReadConcernList(intUserID);
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
                                    this.sb.Append("<div class=\"headadd\"><a href=\"javascript:;\" onclick=\"AddConcern(" + intUserID + "," + intUserIDT + ")\" ><img src=\"Images/add.jpg\" alt=\"添加对该用户的关注\" onMouseOver='this.src=\"Images/add_hover.jpg\"' onMouseOut='this.src=\"Images/add.jpg\"' /></a></div>");
                                }
                            }
                            this.sb.Append("</div>\r\n");
                            this.sb.Append("</div>\r\n");
                            i++;
                        }
                        //ShowImg.Visible = true;

                        ////////////////////////////////////////////////
                        //if (intCount % 8 > 0)
                        //{
                        //    intCount = intCount / 8 + 1;
                        //}
                        //else
                        //{
                        //    intCount = intCount / 8;
                        //}
                        //this.strContent = "<script type=\"text/javascript\">SearchPage(" + intCount + ");</script>";
                        ////////////////////////////////////////////////

                        this.strContent = this.sb.ToString();
                        dt.Dispose();
                    }
                    else
                    {
                        this.strContent = "当前没有满足查询条件的用户";
                    }
            	    break;
                case 2:
                    string strNickName = Server.UrlDecode(Request.QueryString["NickName"].ToString().Trim());

                    dt = SqlLibrary.SearchUser(strNickName);
                    dtConcernList = SqlLibrary.ReadConcernList(intUserID);
                    if (dt != null)
                    {
                        int i = 1;
                        foreach (DataRow dr in dt.Rows)
                        {
                            int intUserIDT = (int)dr["UserId"];
                            this.sb.Append("<div class=\"pic" + i + "\">");
                            this.sb.Append("<div class=\"headpic\"><a href=\"#\"><img src=\"http://ishow.xba.com.cn/image/" + (string)dr["Bmp"].ToString().Trim() + "?tmp=" + rnd.Next(000000, 999999).ToString() + "\" width=\"150\" height=\"130\" /></a></div>");
                            this.sb.Append("<div class=\"show_text\">");
                            this.sb.Append("<div class=\"headname\"><a href=\"#\">" + (string)dr["NickName"].ToString().Trim() + "</a></div>");
                            this.sb.Append("<div class=\"headtime\">" + StringItem.FormatDate((DateTime)dr["CreateTime"], "hh:mm") + "</div>");
                            if (intUserID == intUserIDT)
                            {
                                this.sb.Append("<div class=\"sms\"></div>");
                            }
                            else
                            {
                                this.sb.Append("<div class=\"sms\"><a href=\"javascript:;\" onclick=\"ShowMsgBox(" + (int)dr["UserId"] + ",'" + (string)dr["NickName"].ToString().Trim() + "')\" ><img src=\"Images/pao.jpg\" onMouseOver='this.src=\"Images/pao_hover.jpg\"' onMouseOut='this.src=\"Images/pao.jpg\"'></a></div>");
                            }

                            if (dtConcernList != null)
                            {
                                DataRow[] drs = dtConcernList.Select("ConcernID=" + intUserIDT);
                                if (drs != null && drs.Length > 0)
                                {
                                    this.sb.Append("<div class=\"headadd\"><a href=\"javascript:;\" onclick=\"DeleteConcern(" + intUserID + "," + intUserIDT + ")\" ><img src=\"Images/cut.jpg\" onMouseOver='this.src=\"Images/cut_hover.jpg\"' onMouseOut='this.src=\"Images/cut.jpg\"'></a></div>");
                                }
                                else
                                {
                                    if (intUserID == intUserIDT)
                                    {
                                        this.sb.Append("<div class=\"headadd\"></div>");
                                    }
                                    else
                                    {
                                        this.sb.Append("<div class=\"headadd\"><a href=\"javascript:;\" onclick=\"AddConcern(" + intUserID + "," + intUserIDT + ")\" ><img src=\"Images/add.jpg\" onMouseOver='this.src=\"Images/add_hover.jpg\"' onMouseOut='this.src=\"Images/add.jpg\"'></a></div>");
                                    }
                                }
                                dtConcernList.Dispose();
                            }
                            else
                            {
                                if (intUserID == intUserIDT)
                                {
                                    this.sb.Append("<div class=\"headadd\"><img src=\"Images/add.jpg\" onMouseOver='this.src=\"Images/add_hover.jpg\"' onMouseOut='this.src=\"Images/add.jpg\"'></div>");
                                }
                                else
                                {
                                    this.sb.Append("<div class=\"headadd\"><a href=\"javascript:;\" onclick=\"AddConcern(" + intUserID + "," + intUserIDT + ")\" ><img src=\"Images/add.jpg\" onMouseOver='this.src=\"Images/add_hover.jpg\"' onMouseOut='this.src=\"Images/add.jpg\"'></a></div>");
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
                        this.strContent = "当前没有满足查询条件的用户";
                    }
                    break;
            }
        }
    }
}
