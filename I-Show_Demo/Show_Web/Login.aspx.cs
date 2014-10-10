using System;
using System.Data;
using System.Text;

namespace Show_Web
{
    public partial class Login : System.Web.UI.Page
    {
        public string strContent = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            int intType = 0;
            string strUserName = null;
            string strPassword = null;
            StringBuilder sb = new StringBuilder();

            intType = Convert.ToInt32(Classlibrary.GetRequest("Type", 0));

            if (!intType.Equals(intType) || intType > 0)
            {                
                switch (intType)
                {
                    case 1: //用户登录验证
                        strUserName = Server.HtmlDecode(Classlibrary.GetRequest("UserName", 1).ToString());
                        strPassword = Server.HtmlDecode(Classlibrary.GetRequest("Password", 1).ToString());

                        DataRow dr = SqlLibrary.LoginAuth(strUserName, strPassword);
                        if (dr != null)
                        {
                            sb.Append("{\"UserID\":" + (int)dr["UserID"] + ",\"NickName\":\"" + (string)dr["NickName"] + "\",\"Sex\":\"" + (bool)dr["Sex"] + "\",\"LockTime\":" + (int)dr["LockTime"] + ",\"Status\":\"" + (bool)dr["Status"] + "\",\"Birthday\":\"" + (DateTime)dr["Birthday"] + "\",\"Province\":\"" + (string)dr["Province"] + "\",\"City\":\"" + (string)dr["City"] + "\",\"Tinyurl\":\"" + (string)dr["Tinyurl"] + "\",\"Headurl\":\"" + (string)dr["Headurl"] + "\",\"LastActiveTime\":\"" + (DateTime)dr["LastActiveTime"] + "\",\"QQ\":\"" + (string)dr["QQ"] + "\",\"VIP\":\"" + (bool)dr["VIP"] + "\"}");
                            SqlLibrary.SendQQMsg("182536608", "用户：" + (string)dr["NickName"] + " 登录");
                            this.strContent = sb.ToString();
                        }
                        else
                        {
                            this.strContent = "{\"UserID\":0}";
                        }
                        break;
                    case 2:
                        int intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 2));

                        bool blVIP = SqlLibrary.CheckVIP(intUserID);
                        if (blVIP)
                        {
                            this.strContent = "{\"VIP\":1}";
                        }
                        else
                        {
                            this.strContent = "{\"VIP\":0}";
                        }
                        break;
                    case 3:
                        strUserName = Server.HtmlDecode(Classlibrary.GetRequest("UserName", 1).ToString());
                        strPassword = Server.HtmlDecode(Classlibrary.GetRequest("Password", 1).ToString());

                        intType = SqlLibrary.CheckUser(strUserName, strPassword);
                        this.strContent = "{\"Status\":" + intType + "}";
                        break;
                    case 4:
                        intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 2));

                        long longTime = SqlLibrary.ReadOnlineTime(intUserID);
                        this.strContent = "{\"OnlineTime\":" + longTime + "}";
                        break;
                }
            }
        }
    }
}
