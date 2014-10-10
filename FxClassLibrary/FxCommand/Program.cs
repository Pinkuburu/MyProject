using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace FxCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dt = DateTime.Now;
            string strContent = args[1].ToString() + "--" + args[2].ToString();
            if (dt.DayOfWeek.ToString() == "Saturday" || dt.DayOfWeek.ToString() == "Sunday")
            {
                SendSMS("13780645905", strContent);
            }
            else
            {
                if (dt.Hour > 9 && dt.Hour < 18)
                {
                    try
                    {
                        SendQQMsg("279043930", strContent);
                        Thread.Sleep(2000);
                        SendQQMsg("182536608", strContent);
                    }
                    catch
                    {
                        SendSMS("13780645905", strContent);
                    }

                }
                else
                {
                    SendSMS("13780645905", strContent);
                }
            }            
        }

        #region ���ŷ��� SendSMS(string strToMobile, string strMsg)
        /// <summary>
        /// ���ŷ���
        /// </summary>
        /// <param name="strToMobile">�ն����ֻ���</param>
        /// <param name="strMsg">��������</param>
        /// <returns></returns>
        public static string SendSMS(string strToMobile, string strMsg)
        {
            //C:\fetion>fetion --mobile=13691102424 --pwd=UTx/t4YJrvqz8mnO --exit-on-verifycode=1 --to=13573866764 --msg-utf8=TEST
            string strResult = null;
            try
            {
                string strExePath = @"C:\fetion\fetion.exe";
                //strExePath = Path.GetFullPath(strExePath);
                ProcessStartInfo procInfo = new ProcessStartInfo(strExePath, string.Format("--mobile=13691102424 --pwd=UTx/t4YJrvqz8mnO --exit-on-verifycode=1 --to={0} --msg-gb={1}", strToMobile, strMsg));
                // ����EXE���еĴ���
                procInfo.WindowStyle = ProcessWindowStyle.Hidden;
                // exe����
                Process procBatch = Process.Start(procInfo);
                // ȡ��EXE���к�ķ���ֵ������ֵֻ��������
                strResult = "���ŷ��ͳɹ�~~";
            }
            catch
            {
                strResult = "���ŷ���ʧ��~~";
            }
            return strResult;
        }
        #endregion ���ŷ��� SendSMS(string strToMobile, string strMsg)

        #region ����QQ��Ϣ SendQQMsg(string strQQ, string strMsg)
        /// <summary>
        /// ����QQ��Ϣ
        /// </summary>
        /// <param name="strQQ"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public static string SendQQMsg(string strQQ, string strMsg)
        {
            string strContent = null;
            WebClient HTTPproc = new WebClient();
            //Http://61.147.125.204:8848/Api?Key=Cupid&utf=1&SendType=SendMessage&ID=182536608&Message=%e6%b5%8b%e8%af%95%e6%b6%88%e6%81%af&Time=5000  //HttpUtility.UrlEncode(Rich_Message.Text)
            strContent = HTTPproc.OpenRead("Http://61.147.125.204:8848/Api?Key=Cupid&utf=1&SendType=SendMessage&ID=" + strQQ + "&Message=" + UrlEncode(strMsg, "UTF-8") + "&Time=1000");
            return strContent;
        }
        #endregion ����QQ��Ϣ SendQQMsg(string strQQ, string strMsg)

        #region URL���� UrlEncode(string str, string encode)
        /// <summary>
        /// URL����
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private static string UrlEncode(string str, string encode)
        {
            int factor = 0;
            if (encode == "UTF-8")
                factor = 3;
            if (encode == "GB2312")
                factor = 2;
            //����Ҫ������ַ�

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.";
            System.Text.Encoder encoder = System.Text.Encoding.GetEncoding(encode).GetEncoder();
            char[] c1 = str.ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //һ���ַ�һ���ַ��ı���

            for (int i = 0; i < c1.Length; i++)
            {
                //����Ҫ����

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
        #endregion
    }
}
