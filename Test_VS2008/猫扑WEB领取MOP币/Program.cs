using System;
using System.Collections.Generic;
using System.Text;

namespace 猫扑WEB领取MOP币
{
    class Program
    {
        static WebClient HTTPproc = new WebClient();

        static void Main(string[] args)
        {
            string strRequest = null;
            string strContent = null;
            string strParameter = null;

            strRequest = "http://login.hi.mop.com/Login.do";
            strParameter = "nickname=cupid0426&password=677521&origURL=http%3A%2F%2Fhi.mop.com%2FSysHome.do&loginregFrom=index&ss=10101";
            HTTPproc.OpenRead(strRequest, strParameter);
            strRequest = "http://home.hi.mop.com/ajaxGetContinusLoginAward.do";
            strContent = HTTPproc.OpenRead(strRequest);
            Console.WriteLine(strContent);
        }
    }
}
