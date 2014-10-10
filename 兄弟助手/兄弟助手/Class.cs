using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace 兄弟助手
{
    public class SDGAME
    {
        public string strURL = "http://www.xdgame.cn";

        WebClient HTTPproc = new WebClient();

        #region 登录 Login(string strUsername, string strPassword)
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="strUsername"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public string Login(string strUsername, string strPassword)
        {
            string strSubURL = "/iframePage/Loginiframe.aspx";
            string strContent = null;
            string strParameter = "Login1=" + strUsername + "*" + strPassword;

            HTTPproc.Encoding = Encoding.UTF8;

            strContent = HTTPproc.OpenRead(this.strURL + strSubURL, strParameter);

            try
            {
                strContent = Regex.Match(strContent, @"\w{15,16}").Value;
            }
            catch (ArgumentException ex)
            {
                strContent = "没有找到加密串";
            }

            return strContent;
        }
        #endregion 登录 Login(string strUsername, string strPassword)
    }
}
