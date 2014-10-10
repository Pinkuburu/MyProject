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

namespace I_Show_Demo
{
    public partial class Register : System.Web.UI.Page
    {
        public string strMsg = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Register_Info.Visible = false;
        }

        protected void btn_Register_Click(object sender, EventArgs e)
        {
            string strUsername = tb_UserName.Text.Trim();
            string strPassword = tb_Password.Text.Trim();

            string strSQL = "SELECT [UserId] FROM [User] WHERE [UserId] = '" + strUsername + "'";
            if (strUsername.Trim() != "" && strPassword.Trim() != "")
            {
                DataRow dr = SqlHelper.ExecuteDataRow(SqlLibrary.GetServer_Main(), CommandType.Text, strSQL);
                if (dr == null)
                {
                    strSQL = "INSERT INTO [User](UserId,PassWord,Mac) VALUES('" + strUsername + "','" + strPassword + "','111')";
                    SqlHelper.ExecuteNonQuery(SqlLibrary.GetServer_Main(), CommandType.Text, strSQL);
                    Register_Info.Visible = true;
                    strMsg = "<font color='red'>注册成功</font>";
                    Session["username"] = strUsername;
                    Response.Redirect("Show.aspx");
                }
                else
                {
                    Register_Info.Visible = true;
                    strMsg = "<font color='red'>用户名已被注册</font>";
                }
            }
            else
            {
                Register_Info.Visible = true;
                strMsg = "<font color='red'>用户名或密码不能为空</font>";
            }
        }
    }
}
