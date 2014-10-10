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

namespace NumberGame
{
    public partial class _Default : System.Web.UI.Page
    {
        public Random rnbNum = new Random();
        protected void Page_Load(object sender, EventArgs e)
        {
            //<span id="Timer">10</span>秒<br />
            //<asp:Label ID="lbl_Text" runat="server">个人资产：</asp:Label>
            //<asp:Label ID="lbl_Num" runat="server">1</asp:Label><br/> 
            //<asp:Button id = "btn_addNum_1" text="单数写报告" runat="server" OnClick="btn_addNum_1_Click" />
            //<asp:Button id = "btn_addNum_2" text="双数拉客户" runat="server" OnClick="btn_addNum_2_Click" />
            //<asp:Button id = "btn_addNum_3" text="素数抓小偷" runat="server" OnClick="btn_addNum_3_Click" />
            //<asp:Label ID="ss" runat="server" ForeColor="red"></asp:Label>
            //ss.Visible = false;
            //int intNum = Convert.ToInt32(this.lbl_Num.Text.ToString().Trim());
            //if (isPrime(intNum))
            //{
            //    ss.Visible = true;
            //    ss.Text = "1";
            //}
        }

        protected void btn_addNum_1_Click(object sender, EventArgs e)
        {
            int intNum = Convert.ToInt32(this.lbl_Num.Text.ToString().Trim());

            if (isPrime(intNum))
            {
                intNum = intNum - 1;
                this.lbl_Num.Text = intNum.ToString();
            }
            else
            {
                if (intNum % 2 == 1)
                {
                    intNum = intNum + rnbNum.Next(1, 5);
                    this.lbl_Num.Text = intNum.ToString();
                }
                else
                {
                    intNum = intNum - 1;
                    this.lbl_Num.Text = intNum.ToString();
                }
            }
        }

        protected void btn_addNum_2_Click(object sender, EventArgs e)
        {
            int intNum = Convert.ToInt32(this.lbl_Num.Text.ToString().Trim());

            if (isPrime(intNum))
            {
                intNum = intNum - 1;
                this.lbl_Num.Text = intNum.ToString();
            }
            else
            {
                if (intNum % 2 == 0)
                {
                    intNum = intNum + rnbNum.Next(1, 5);
                    this.lbl_Num.Text = intNum.ToString();
                }
                else
                {
                    intNum = intNum - 1;
                    this.lbl_Num.Text = intNum.ToString();
                }
            }
        }

        protected void btn_addNum_3_Click(object sender, EventArgs e)
        {
            int intNum = Convert.ToInt32(this.lbl_Num.Text.ToString().Trim());
            if (isPrime(intNum))
            {
                intNum = intNum + rnbNum.Next(1, 5);
                this.lbl_Num.Text = intNum.ToString();
            }
            else
            {
                intNum = intNum - 1;
                this.lbl_Num.Text = intNum.ToString();
            }
        }

        #region 素数判定 isPrime(int a)
        /// <summary>
        /// 素数判定
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool isPrime(int a)
        {
            int i;
            for (i = 2; i < a; i++)
            {
                if (Math.IEEERemainder((float)a, (float)i) == 0)   //是否能被i整除   
                    return false;
            }
            return true;
        }
        #endregion 素数判定

    }
}
