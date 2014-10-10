using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace XiaoNeiLogin_C
{
    class XiaoNeiLogin
    {
        static void Main(string[] args)
        {
            string strUserName = "";
            string strPassword = "";

            //Console.Write("校内帐号：");
            //strUserName = Console.ReadLine().Trim();
            //Console.Write("校内密码：");
            //strPassword = Console.ReadLine().Trim();

            if (strUserName == "" && strPassword == "")
            {
                //strUserName = "cupid0426@163.com";
                //strPassword = "loveemma++";
                strUserName = "13616394486";
                strPassword = "cupid0426";
            }
            XiaoNeiLogin_C.WebClient HTTPproc = new WebClient();
            //设置HTTP请求默认编码
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            string strRedirectURL = HTTPproc.OpenRead("http://www.renren.com/PLogin.do", "email=" + UrlEncode(strUserName, "UTF-8") + "&password=" + UrlEncode(strPassword, "UTF-8") + "&origURL=http%3A%2F%2Fwww.renren.com%2FSysHome.do&domain=renren.com");
            HTTPproc.OpenRead(strRedirectURL.Replace("The URL has moved <a href=\"", "").Replace("\">here</a>", ""));
            Console.WriteLine(HTTPproc.OpenRead("http://www.renren.com/Home.do"));
            //Console.WriteLine(GetCpuInfo());
            //Console.ReadKey();
        }

        public static string UrlEncode(string str, string encode)
        {
            int factor = 0;
            if (encode == "UTF-8")
                factor = 3;
            if (encode == "GB2312")
                factor = 2;
            //不需要编码的字符

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.";
            System.Text.Encoder encoder = System.Text.Encoding.GetEncoding(encode).GetEncoder();
            char[] c1 = str.ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //一个字符一个字符的编码

            for (int i = 0; i < c1.Length; i++)
            {
                //不需要编码

                if (okChar.IndexOf(c1[i]) > -1)
                    sb.Append(c1[i]);
                else
                {
                    byte[] c2 = new byte[factor];
                    int charUsed, byteUsed; bool completed;

                    encoder.Convert(c1, i, 1, c2, 0, factor, true, out charUsed, out byteUsed, out completed);

                    foreach (byte b in c2)
                    {
                        if (b != 0)
                            sb.AppendFormat("%{0:X}", b);
                    }
                }
            }
            return sb.ToString().Trim();
        }

        ///<summary>    
        ///获取cpu序列号        
        ///</summary>    
        ///<returns> string </returns>    
        public static string GetCpuInfo()   
        {   
            string cpuInfo = " ";   
            using (ManagementClass cimobject = new ManagementClass("Win32_Processor"))   
            {   
                ManagementObjectCollection moc = cimobject.GetInstances();   
  
                foreach (ManagementObject mo in moc)   
                {   
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();   
                    mo.Dispose();   
                }   
            }   
            return cpuInfo.ToString();   
        } 
    }
}
