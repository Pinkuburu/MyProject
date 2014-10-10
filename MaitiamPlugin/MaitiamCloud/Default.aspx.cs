using System;
using System.Web;

namespace MaitiamCloud
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        { 
            string strUsername = txtUsername.Text.Trim();
            string strPassword = txtPassword.Text.Trim();

            int intAdminID = LoginManager.Login(strUsername, strPassword);

            if (intAdminID > 0)
            {
                DateTime dt = DateTime.Now; //需要<%@Import Namespace="System"%>，得到当前时间
                HttpCookie mycookie = new HttpCookie("adminid");//申明新的COOKIE变量
                mycookie.Value = intAdminID.ToString();//赋值
                mycookie.Expires = Convert.ToDateTime(dt + TimeSpan.FromDays(1));//设定过期时间为1天
                Response.Cookies.Add(mycookie);//写入COOKIE
                //Session["AdminID"] = intAdminID;
                Response.Redirect("Main.aspx");
            }
        }
    }
}