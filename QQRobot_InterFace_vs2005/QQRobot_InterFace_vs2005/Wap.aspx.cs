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
using QQRobot_Module;

namespace QQRobot_InterFace_vs2005
{
    public partial class Wap : System.Web.UI.Page
    {
        ServerCheck sc = new ServerCheck();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Turn_BB_Click(object sender, EventArgs e)
        {
            Response.Write(sc.GetGameTurn("BB").Replace("\n","<br>"));
        }

        protected void btn_Turn_FB_Click(object sender, EventArgs e)
        {
            Response.Write(sc.GetGameTurn("FB").Replace("\n", "<br>"));
        }

        protected void btn_Status_BB_Click(object sender, EventArgs e)
        {
            Response.Write(sc.GetGameStatus("BB").Replace("\n", "<br>"));
        }

        protected void btn_Status_FB_Click(object sender, EventArgs e)
        {
            Response.Write(sc.GetGameStatus("FB").Replace("\n", "<br>"));
        }

        protected void btn_Check_BB_Click(object sender, EventArgs e)
        {
            Response.Write(sc.GetGameCheck("BB").Replace("\n", "<br>"));
        }

        protected void btn_Check_FB_Click(object sender, EventArgs e)
        {
            Response.Write(sc.GetGameCheck("FB").Replace("\n", "<br>"));
        }

        protected void btn_Season_Click(object sender, EventArgs e)
        {
            if (tb_KeyWords.Text.Trim() != "")
            {
                Response.Write(sc.GetGameSeason(tb_KeyWords.Text.ToString().Trim()).Replace("\n", "<br>"));
            }
        }
    }
}
