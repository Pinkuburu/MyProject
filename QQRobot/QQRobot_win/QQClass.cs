using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Windows.Forms;

namespace QQRobot_win
{
    class QQClass
    {
        //����������Ϣ
        private static int i = 12;
        //����HTTP������
        private static WebClient HTTPproc = new WebClient();
        //�������������
        private static RandStr rnd = new RandStr(true, false, false, false);

        #region QQ��������㷨
        /// <summary>
        /// QQ��������㷨
        /// </summary>
        public static class qqPwdEncrypt
        {
            /// <summary>
            /// ������ҳ��QQ��¼ʱ������ܺ�Ľ��
            /// </summary>
            /// <param name="pwd" />QQ����</param>
            /// <param name="verifyCode" />��֤��</param>
            /// <returns></returns>
            public static String Encrypt(string pwd, string verifyCode)
            {
                return (md5(md5_3(pwd).ToUpper() + verifyCode.ToUpper())).ToUpper();
            }
            /// <summary>
            /// �����ַ���������MD5
            /// </summary>
            /// <param name="s" /></param>
            /// <returns></returns>
            private static String md5_3(String s)
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);

                bytes = md5.ComputeHash(bytes);
                bytes = md5.ComputeHash(bytes);
                bytes = md5.ComputeHash(bytes);

                md5.Clear();

                string ret = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
                }

                return ret.PadLeft(32, '0');
            }
            /// <summary>
            /// �����ַ�����һ��MD5
            /// </summary>
            /// <param name="s" /></param>
            /// <returns></returns>
            private static String md5(String s)
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);

                bytes = md5.ComputeHash(bytes);

                md5.Clear();

                string ret = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
                }

                return ret.PadLeft(32, '0');
            }
        }
        #endregion QQ��������㷨
        
        public static string Login(string strQQ,string strPwd)
        {
            //��������
            string strMsg = null;
            string strReMsg = null;
            string strRequest = null;
            string strContent = null;
            string strCookies = null;
            string resultString = null;
            string strSkey = null;
            string strPtcz = null;
            string strPtwebqq = null;
            string strPost = null;

            string[] arrResult = { };
            HTTPproc.Encoding = System.Text.Encoding.UTF8;

            strRequest = "http://ptlogin2.qq.com/check?uin=" + strQQ + "&appid=1002101&r=" + rnd.GetRandStr(13);
            //strContent = HTTPproc.OpenRead(strRequest);
            strContent = HTTPproc.GetHtml(strRequest);

            try
            {
                resultString = Regex.Match(strContent, @"'\d','.*'").Value.Replace("\'", "");//'0','!MLF'
                arrResult = resultString.Split(',');
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("û�л�ȡ����֤������������飡");
                Console.WriteLine(ex);
            }
            Console.WriteLine("��ȡ����֤��:" + arrResult[1]);

            strCookies = HTTPproc.ResponseHeaders.GetValues(3).GetValue(0).ToString();

            try
            {
                resultString = Regex.Replace(strCookies, ";.*", "").Replace("ptvfsession=","");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            HTTPproc.CookieContainer.Add(new Uri("http://ptlogin2.qq.com"), new Cookie("ptvfsession", resultString));
            strRequest = "http://ptlogin2.qq.com/login?u=" + strQQ + "&p=" + qqPwdEncrypt.Encrypt(strPwd, arrResult[1]) + "&verifycode=" + arrResult[1] + "&remember_uin=1&aid=1002101&u1=http%3A%2F%2Fweb.qq.com%2Fmain.shtml%3Fdirect__2&h=1&ptredirect=1&ptlang=2052&from_ui=1&pttype=1&dumy=&fp=loginerroralert";

            //strContent = HTTPproc.OpenRead(strRequest);
            strContent = HTTPproc.GetHtml(strRequest);

            try
            {
                resultString = Regex.Match(strContent, @"'\d','.*'").Value.Replace("\'", "");//'3','0','','0','��������������������ԡ�'
                arrResult = resultString.Split(',');
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("û�л�ȡ����֤������������飡");
                Console.WriteLine(ex);
            }

            #region ��¼����ֵ�ж�
            if (arrResult[0] == "0")        //0����¼�ɹ�!
            {
                Console.WriteLine(arrResult[4]);
                strSkey = HTTPproc.ResponseHeaders.GetValues(1).GetValue(2).ToString();
                //strPtcz = HTTPproc.ResponseHeaders.GetValues(1).GetValue(9).ToString();
                try
                {
                    strPtwebqq = HTTPproc.ResponseHeaders.GetValues(1).GetValue(9).ToString();
                }
                catch
                {
                    strPtwebqq = HTTPproc.ResponseHeaders.GetValues(1).GetValue(8).ToString();
                }
            }
            else if (arrResult[0] == "1")   //1��ϵͳ��æ�����Ժ����ԡ�
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "2")   //2���Ѿ����ڵ�QQ���롣
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "3")   //3����������������������ԡ�
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "4")   //4�����������֤�����������ԡ�
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "5")   //5��У��ʧ�ܡ�
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "6")   //6�����������������޸Ĺ�����, ���Ժ��ٵ�¼.
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "7")   //7��������������, �����ԡ�
            {
                Console.WriteLine(arrResult[4]);
            }
            else                            //8������IP�������Ĵ������࣬���Ժ����ԡ�
            {
                Console.WriteLine("δ֪����");
            }
            Console.WriteLine(arrResult[1]);
            #endregion ��¼����ֵ�ж�
                  
            //strSkey = Regex.Replace(strSkey, ";.*", "").Replace("skey=", "");
            //strPtwebqq = Regex.Replace(strPtwebqq, ";.*", "").Replace("ptwebqq=", "");

            strSkey = Regex.Replace(strSkey, ";.*", "").Replace("skey=", "");
            //strPtcz = Regex.Replace(strPtcz, ";.*", "").Replace("ptcz=", "");
            strPtwebqq = Regex.Replace(strPtwebqq, ";.*", "").Replace("ptwebqq=", "");

            //HTTPproc.CookieContainer.Add(new Uri("http://ptlogin2.qq.com"), new Cookie("skey", resultString));
            //HTTPproc.CookieContainer.Add(new Uri("http://ptlogin2.qq.com"), new Cookie("ptcz", resultString));
            //HTTPproc.CookieContainer.Add(new Uri("http://ptlogin2.qq.com"), new Cookie("ptwebqq", resultString));

            //*strPost = strQQ + ";22;0;00000000;" + strSkey + ";" + strPtwebqq + ";0;";
            //*strRequest = "http://web-proxy.qq.com/conn_s";
            
            //string strOldCookie = HTTPproc.Cookie.ToString();
            //try 
            //{
            //    resultString = Regex.Replace(strOldCookie, "; .*pti", "pti");
            //    resultString = Regex.Replace(resultString, "; .*p", "p");
            //} 
            //catch (ArgumentException ex) 
            //{
            //    // Syntax error in the regular expression
            //}
            //HTTPproc.RequestHeaders.Clear();
            //HTTPproc.RequestHeaders.Add("Cookie:" + resultString);
            //HTTPproc.RequestHeaders.Set("Cookie", resultString);
            //HTTPproc.RequestHeaders.Add("Referer:http://web-proxy.qq.com/");
            //strContent = HTTPproc.OpenRead(strRequest, strPost);
            //*strContent = HTTPproc.Post(strRequest, strPost);
            //*Console.WriteLine(strContent);

            //*string[] arrKey = strContent.Split(';');

            //*//��ȡ����
            //*strContent = HTTPproc.Post(strRequest, strQQ + ";3c;0;" + arrKey[4].ToString() + ";1;" + strQQ + ";00;1;" + arrKey[4].ToString() + ";");
            //*Console.WriteLine(strContent);

            //*//��ȡ�����б�
            //*strContent = HTTPproc.Post(strRequest, strQQ + ";06;2;" + arrKey[4].ToString() + ";" + strQQ + ";" + strQQ + ";5c;3;" + arrKey[4].ToString() + ";88;" + strQQ + ";67;4;" + arrKey[4].ToString() + ";03;1;" + strQQ + ";" + strQQ + ";58;5;" + arrKey[4].ToString() + ";0;" + strQQ + ";26;6;" + arrKey[4].ToString() + ";0;0;" + strQQ + ";3e;7;" + arrKey[4].ToString() + ";4;0;" + strQQ + ";65;8;" + arrKey[4].ToString() + ";02;" + strQQ + ";" + strQQ + ";1d;9;" + arrKey[4].ToString() + ";" + strQQ + ";00;10;" + arrKey[4].ToString() + ";");
            //*Console.WriteLine(strContent);

            //*strContent = HTTPproc.Post(strRequest, strQQ + ";00;11;" + arrKey[4].ToString() + ";");
            //*Console.WriteLine(strContent);

            //*string[] arrReMsg;

            //*while (i < 100)
            //*{
            //*    Console.WriteLine("���ڵȴ�������Ϣ:{0}", i);
            //*    strContent = HTTPproc.Post(strRequest, strQQ + ";00;" + i.ToString() + ";" + arrKey[4].ToString() + ";");

            //*    arrReMsg = strContent.Split(';');
            //*    if (arrReMsg.Length > 10)
            //*    {
            //*        if (arrReMsg[1].ToString() == "17")
            //*        {
            //*            i++;
            //*            strMsg = HTTPproc.Post(strRequest, strQQ + ";17;" + arrReMsg[2].ToString() + ";" + arrKey[4].ToString() + ";" + arrReMsg[3].ToString() + ";" + strQQ + ";" + arrReMsg[4].ToString() + ";1;" + arrReMsg[10].ToString() + ";" + strQQ + ";00;" + i.ToString() + ";" + arrKey[4].ToString() + ";");
            //*                                             //strQQ + ";17;" + arrReMsg[2].ToString() + ";" + arrKey[4].ToString() + ";" + arrReMsg[3].ToString() + ";" + strQQ + ";" + arrReMsg[4].ToString() + ";1;" + arrReMsg[10].ToString() + ";" + strQQ + ";00;" + i.ToString() + ";" + arrKey[4].ToString() + ";"
            //*            Console.WriteLine("����ȷ����Ϣ:{0}", strMsg);

            //*            //������Ϣ
            //*            i++;
            //*            strReMsg = HTTPproc.Post(strRequest, strQQ + ";16;" + i.ToString() + ";" + arrKey[4].ToString() + ";" + arrReMsg[3].ToString() + ";0b;528;" + UrlEncode(arrReMsg[7].ToString(),"UTF-8") + ";0a00000010%E5%AE%8B%E4%BD%93");
            //*            Console.WriteLine("������Ϣ:{0}", strReMsg);
            //*            //1307364337;16;119;165fad25;182536608;0b;528;%E4%BD%A0%E5%A5%BD;0a00000010%E5%AE%8B%E4%BD%93;
            //*            //1307364337;16;135;165fad25;182536608;0b;528;%E4%BD%A0%E5%A5%BD;0aff000010%E5%AE%8B%E4%BD%93;
            //*        }
            //*    }
            //*    Console.WriteLine(strContent);
            //*    i++;
            //*}

            return strSkey;
        }

        public static string Login(string strQQ)
        {            
            //��������
            string strRequest = null;
            string strContent = null;
            string resultString = null;
            string vc_type = null;
            string strCookies = null;
            string[] arrResult = { };

            HTTPproc.Encoding = System.Text.Encoding.UTF8;

            strRequest = "http://ptlogin2.qq.com/check?uin=" + strQQ + "&appid=1002101&r=" + rnd.GetRandStr(13);
            strContent = HTTPproc.GetHtml(strRequest);

            try
            {
                resultString = Regex.Match(strContent, @"'\d','.*'").Value.Replace("\'", "");//'0','!MLF'
                arrResult = resultString.Split(',');
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("û�л�ȡ����֤������������飡");
                MessageBox.Show(ex.ToString());
            }
            vc_type = arrResult[1].ToString();

            strCookies = HTTPproc.ResponseHeaders.GetValues(3).GetValue(0).ToString();

            try
            {
                resultString = Regex.Replace(strCookies, ";.*", "").Replace("ptvfsession=", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            HTTPproc.CookieContainer.Add(new Uri("http://ptlogin2.qq.com"), new Cookie("ptvfsession", resultString));

            return vc_type;
        }        

        public static string ReadUserInfo(string strQQ, string strSkey)
        {
            string strUserInfo = null;            
            string strRequest = null;
            string strPost = null;
            
            try
            {
                strSkey = Regex.Replace(strSkey, ";.*", "").Replace("skey=", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            strPost = strQQ + ";5c;3;" + strSkey + ";88;";
            strRequest = "http://web-proxy.qq.com/conn_s";

            //strContent = HTTPproc.OpenRead(strRequest);
            strUserInfo = HTTPproc.Post(strRequest, strPost);
            return strUserInfo;
        }

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

        #region ������� ���������֡����� RandStr
        /// <summary>
        /// ������� ���������֡�����
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
            /// ��δ�ṩ��������,��Ĭ��������+Сд��ĸ����
            /// </summary>
            public RandStr()
            {
                framerStr = numStr + lowerStr;
            }

            /// <summary>
            /// ���캯��,��ָ�����ɵ��ַ�
            /// </summary>
            /// <param name="useNum">�Ƿ�ʹ������</param>
            /// <param name="useUpper">�Ƿ�ʹ�ô�д��ĸ</param>
            /// <param name="useLower">�Ƿ�ʹ��Сд��ĸ</param>
            /// <param name="useMark">�Ƿ�ʹ�÷���</param>
            public RandStr(bool useNum, bool useUpper, bool useLower, bool useMark)
            {
                // �����ͼ���첻�����κ�����ַ�����,���׳��쳣
                if (!useNum && !useUpper && !useLower && !useMark)
                {
                    throw new ArgumentException("��������ʹ��һ�ֹ����ַ�!");
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
            /// ʹ���Զ��������ַ�����
            /// </summary>
            /// <param name="userStr">�Զ����ַ�</param>
            public RandStr(string userStr)
            {
                // �����ͼ�ÿ��ַ���������,���׳��쳣
                if (userStr.Length == 0)
                {
                    throw new ArgumentException("������ʹ��һ���ַ�!");
                }
                else
                {
                    framerStr = userStr;
                }
            }

            /// <summary>
            /// ȡ��һ������ַ���
            /// </summary>
            /// <param name="length">ȡ������ַ����ĳ���</param>
            /// <returns>���ص�����ַ���</returns>
            public string GetRandStr(int length)
            {
                // ��ȡ�ĳ��Ȳ���Ϊ0�����߸�����
                if (length < 1)
                {
                    throw new ArgumentException("�ַ����Ȳ���Ϊ0���߸���!");
                }
                else
                {
                    // ���ֻ�ǻ�ȡ��������ַ���,
                    // ����û������.
                    // �������Ҫ��ʱ���ȡ��������ַ����Ļ�,
                    // �����������ܲ���.
                    // ���Ը���StringBuilder�����������,
                    // ��Ҫ�Ŀ����Լ���һ�� ^o^
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
        #endregion ������� ���������֡����� RandStr
    }
}
