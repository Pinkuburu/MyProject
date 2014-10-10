using System;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;

namespace TestPing
{
    public partial class _Default : System.Web.UI.Page
    {
        public string strContent = null;
        public int intSendCount = 0;
        public int intReplyCount = 0;
        public int intLostCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Ping PingInfo = new Ping();
            intSendCount = 20;
            int TimeOut = 1000;

            for (int i = 0; i < intSendCount; i++)
            {
                PingReply reply = PingInfo.Send("209.97.50.34", TimeOut);
                displayReply(reply);
                Thread.Sleep(1000);
            }
            Response.Write(string.Format("数据包:已发送={0}，已接收={1}，丢失={2}({3}% 丢失)", intSendCount, intReplyCount, intLostCount, intLostCount * 100 / intSendCount));
        }

        private void displayReply(PingReply reply) //显示结果 
        {
            if (reply.Status == IPStatus.Success)
            {
                intReplyCount++;
            }
            else
            {
                intLostCount++;
            }
        }
    }
}
