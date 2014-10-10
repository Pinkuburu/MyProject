using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json.Linq;

namespace XiaoNei_App
{
    public class XiaoNeiApi
    {
        string Api_Key = "2c3dae2f4a494b7898b8dd361783f8e2";
        string Secret_Key = "ce7b5e0162254c6abc078b7a44c4ff5f";
        string Format = "JSON";

        WebClient HTTPproc = new WebClient();
        
        public string users_getLoggedInUser(string strSessionKey)
        {
            string strUID = null;
            string strParameters = string.Format("api_key={0}&method={1}&session_key={2}call_id=1&sig=1&v=1.0&format={3}", this.Api_Key, "xiaonei.users.getLoggedInUser", this.Session_Key, this.format);


            return strUID;
        }

        private string strSigKey(string strParameter)
        {
 
        }
    }
}
