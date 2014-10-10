using System;
using System.Text;
using System.Net.NetworkInformation;

namespace PingTest
{
    public partial class _Default : System.Web.UI.Page
    {
        public string strContent = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            Ping PingInfo = new Ping();
        }
    }
}
