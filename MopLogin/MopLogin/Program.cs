using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace MopLogin
{
    class Program
    {
        static void Main(string[] args)
        {
            string strContent = null;
            string resultString = null;

            WebClient HTTPproc = new WebClient();
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            try
            {
                //cupid0426  http://3g.mop.com/index.jsp?uid=339798198&key=de5e6d4ad67b5e87
                strContent = HTTPproc.OpenRead("http://3g.mop.com/index.jsp?uid=339798198&key=de5e6d4ad67b5e87");                
                try
                {
                    resultString = Regex.Match(strContent, @"\[<a href=""http://3g\.mop\.com/dzh/zone\.do.*log=1").Value;
                    resultString = HTTPproc.OpenRead(resultString.Replace("amp;", "").Replace("[<a href=\"", ""));
                    resultString = Regex.Match(resultString, @"\[<a href=""http://3g\.mop\.com/dzh/zone\.do.*log=1").Value;
                    HTTPproc.OpenRead(resultString.Replace("amp;", "").Replace("[<a href=\"", ""));
                    //HTTPproc.OpenRead(HTTPproc.ResponseHeaders["Location"].ToString());
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                HTTPproc = new WebClient();
                //snoopy6973  http://3g.mop.com/index.jsp?uid=332561839&key=FhkuaWUuFRgAaW4NCA--
                strContent = HTTPproc.OpenRead("http://3g.mop.com/index.jsp?uid=332561839&key=FhkuaWUuFRgAaW4NCA--");
                try
                {
                    resultString = Regex.Match(strContent, @"\[<a href=""http://3g\.mop\.com/dzh/zone\.do.*log=1").Value;
                    resultString = HTTPproc.OpenRead(resultString.Replace("amp;", "").Replace("[<a href=\"", ""));
                    resultString = Regex.Match(resultString, @"\[<a href=""http://3g\.mop\.com/dzh/zone\.do.*log=1").Value;
                    HTTPproc.OpenRead(resultString.Replace("amp;", "").Replace("[<a href=\"", ""));
                    //HTTPproc.OpenRead(HTTPproc.ResponseHeaders["Location"].ToString());
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                HTTPproc = new WebClient();
                //cupid0616  http://3g.mop.com/index.jsp?uid=425832088&key=bBoNNGZzNAcMFRpybw--
                strContent = HTTPproc.OpenRead("http://3g.mop.com/index.jsp?uid=425832088&key=bBoNNGZzNAcMFRpybw--");
                try
                {
                    resultString = Regex.Match(strContent, @"\[<a href=""http://3g\.mop\.com/dzh/zone\.do.*log=1").Value;
                    resultString = HTTPproc.OpenRead(resultString.Replace("amp;", "").Replace("[<a href=\"", ""));
                    resultString = Regex.Match(resultString, @"\[<a href=""http://3g\.mop\.com/dzh/zone\.do.*log=1").Value;
                    HTTPproc.OpenRead(resultString.Replace("amp;", "").Replace("[<a href=\"", ""));
                    //HTTPproc.OpenRead(HTTPproc.ResponseHeaders["Location"].ToString());
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                HTTPproc = new WebClient();
                //cupid0521  http://3g.mop.com/index.jsp?uid=426030574&key=bBoANGZ3NGcMaBV7Fg--
                strContent = HTTPproc.OpenRead("http://3g.mop.com/index.jsp?uid=426030574&key=bBoANGZ3NGcMaBV7Fg--");
                try
                {
                    resultString = Regex.Match(strContent, @"\[<a href=""http://3g\.mop\.com/dzh/zone\.do.*log=1").Value;
                    resultString = HTTPproc.OpenRead(resultString.Replace("amp;", "").Replace("[<a href=\"", ""));
                    resultString = Regex.Match(resultString, @"\[<a href=""http://3g\.mop\.com/dzh/zone\.do.*log=1").Value;
                    HTTPproc.OpenRead(resultString.Replace("amp;", "").Replace("[<a href=\"", ""));
                    //HTTPproc.OpenRead(HTTPproc.ResponseHeaders["Location"].ToString());
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }                               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
