using System;

namespace uuShare_Login
{
    class Program
    {
        static void Main(string[] args)
        {
            uuShare_Login.WebClient HTTPproc = new WebClient();
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            HTTPproc.OpenRead("http://www.uudisc.com");
            HTTPproc.RequestHeaders.Add("Referer:http://www.uudisc.com/");
            HTTPproc.OpenRead("http://www.uudisc.com/account/login", "username=cupid0616&password=677521&user_login=+%E7%99%BB+%E5%BD%95+");
            HTTPproc.OpenRead("http://www.uudisc.com/user/cupid0616");
        }
    }
}
