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
    public partial class _Default : System.Web.UI.Page
    {
        public string strMsg = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Login_Info.Visible = false;
        }

        protected void btn_Register_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            string strSQL = null;
            string strUsername = tb_UserName.Text.Trim();
            string strPassword = tb_Password.Text.Trim();

            if (strUsername != "" && strPassword != "")
            {
                strSQL = "SELECT [UserId],[PassWord] FROM [User] WHERE [UserId] = '" + strUsername + "' AND [PassWord] = '" + strPassword + "'";
                DataRow dr = SqlHelper.ExecuteDataRow(SqlLibrary.GetServer_Main(), CommandType.Text, strSQL);
                if (dr != null)
                {
                    strUsername = (string)dr["UserId"].ToString().Trim(); //���������û��trim() ��������
                    //Session["username"] = strUsername;
                    Response.Redirect("Show.aspx");
                }
                else
                {
                    Login_Info.Visible = true;
                    strMsg = "<font color='red'>�û�������������</font>";
                }
            }
            else
            {
                Login_Info.Visible = true;
                strMsg = "<font color='red'>�û��������벻��Ϊ��</font>";
            }
        }
    }
}
