using System;
using System.Collections.Generic;
using System.Text;

namespace MailLogin_126
{
    class Program
    {
        static void Main(string[] args)
        {
            string strUserName = args[0].ToString();
            string strPassWord = args[1].ToString();
            string strRequest = null;
            string strContent = null;
            string strParameter = null;
            WebClient HTTPproc = new WebClient();

            HTTPproc.Encoding = System.Text.Encoding.UTF8;

            strRequest = "http://m.mail.163.com/login.s";
            strParameter = "username=" + strUserName + "&password=" + strPassWord + "&timestamp=&srandid=&domain=126.com&method=login&back_url=&srand=&wml=true";

            HTTPproc.RequestHeaders.Add("Referer:http://m.mail.163.com/?domain=126.com&wml=true");
            strContent = HTTPproc.OpenRead(strRequest, strParameter);
            foreach (string strLocation in HTTPproc.ResponseHeaders.GetValues("Location"))
            {
                strRequest = strLocation;
            }
            HTTPproc.OpenRead(strRequest);
        }
    }
}
