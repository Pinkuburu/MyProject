using System;
using System.Collections.Generic;
using System.Text;

namespace WebFetion
{
    class Program
    {
        static void Main(string[] args)
        {
            FetionWeb_3G fx = new FetionWeb_3G();
            fx.FetionLogin("13406802804", "");
            fx.SendSMS("571887897", "测试中文内容,Test");
            Console.ReadKey();
        }
    }

    public class FetionWeb_3G
    {
        WebClient HTTPproc = new WebClient();        
        RandStr rnd = new RandStr(true,false,false,false);

        public string FetionLogin(string strMobile, string strPassword)
        {
            HTTPproc.Encoding = Encoding.UTF8;
            string strResult = "";
            string strUrl = "http://f.10086.cn/im5/login/login.action";
            HTTPproc.OpenRead(strUrl);
            
            strUrl = "http://f.10086.cn/im5/login/loginHtml5.action?t=" + rnd.GetRandStr(17);
            string strParameter = "m=" + strMobile + "&pass=" + strPassword;
            string strContent = HTTPproc.OpenRead(strUrl, strParameter);
            Console.WriteLine(strContent);

            return strResult;
        }

        //touserid=571887897&msg=TEstseta1231231231
        public void SendSMS(string strUid, string strMsg)
        {
            HTTPproc.Encoding = Encoding.UTF8;
            string strUrl = "http://f.10086.cn/im5/chat/sendNewShortMsg.action";
            string strParameter = "touserid=" + strUid + "&msg=" + strMsg;
            string strContent = HTTPproc.OpenRead(strUrl, strParameter);
            Console.WriteLine(strContent);
        }
    }

    #region 随机生成 字数、数字、符号 RandStr
    /// <summary>
    /// 随机生成 字数、数字、符号
    /// </summary>
    public class RandStr
    {
        private string framerStr = null;
        private string numStr = "0123456789";
        private string upperStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string lowerStr = "abcdefghijklmnopqrstuvwxyz";
        private string markStr = @"`-=[];'\,./~!@#$%^&*()_+{}:""|<>?";
        private static Random myRandom = new Random();

        /// <summary>
        /// 如未提供参数构造,则默认由数字+小写字母构成
        /// </summary>
        public RandStr()
        {
            framerStr = numStr + lowerStr;
        }

        /// <summary>
        /// 构造函数,可指定构成的字符
        /// </summary>
        /// <param name="useNum">是否使用数字</param>
        /// <param name="useUpper">是否使用大写字母</param>
        /// <param name="useLower">是否使用小写字母</param>
        /// <param name="useMark">是否使用符号</param>
        public RandStr(bool useNum, bool useUpper, bool useLower, bool useMark)
        {
            // 如果试图构造不包含任何组成字符的类,则抛出异常
            if (!useNum && !useUpper && !useLower && !useMark)
            {
                throw new ArgumentException("必须至少使用一种构成字符!");
            }
            else
            {
                if (useNum)
                    framerStr += numStr;
                if (useUpper)
                    framerStr += upperStr;
                if (useLower)
                    framerStr += lowerStr;
                if (useMark)
                    framerStr += markStr;
            }
        }

        /// <summary>
        /// 使用自定义的组成字符构造
        /// </summary>
        /// <param name="userStr">自定义字符</param>
        public RandStr(string userStr)
        {
            // 如果试图用空字符串构造类,则抛出异常
            if (userStr.Length == 0)
            {
                throw new ArgumentException("请至少使用一个字符!");
            }
            else
            {
                framerStr = userStr;
            }
        }

        /// <summary>
        /// 取得一个随机字符串
        /// </summary>
        /// <param name="length">取得随机字符串的长度</param>
        /// <returns>返回的随机字符串</returns>
        public string GetRandStr(int length)
        {
            // 获取的长度不能为0个或者负数个
            if (length < 1)
            {
                throw new ArgumentException("字符长度不能为0或者负数!");
            }
            else
            {
                // 如果只是获取少量随机字符串,
                // 这样没有问题.
                // 但如果需要短时间获取大量随机字符串的话,
                // 这样可能性能不高.
                // 可以改用StringBuilder类来提高性能,
                // 需要的可以自己改一下 ^o^
                string tempStr = null;
                for (int i = 0; i < length; i++)
                {
                    int randNum = myRandom.Next(framerStr.Length);
                    tempStr += framerStr[randNum];
                }
                return tempStr;
            }
        }
    }
    #endregion 随机生成 字数、数字、符号 RandStr
}
