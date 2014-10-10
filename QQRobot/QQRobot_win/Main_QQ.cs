using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace QQRobot_win
{
    public partial class Main_QQ : Form
    {
        public string strContent = null;
        public string strQQ = null;
        public string strRequest = null;
        public string vc_type = null;
        public string strKey = null;
        
        //����������Ϣ
        public int i = 12;
        //����HTTP������
        public WebClient HTTPproc = new WebClient();
        //�������������
        public RandStr rnd = new RandStr(true, false, false, false);

        public Main_QQ()
        {
            InitializeComponent();
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            this.strKey = Login(this.strQQ, textBox_Password.Text.Trim(), textBox_Code.Text.Trim());
        }

        private void button_ReadCode_Click(object sender, EventArgs e)
        {
            if (textBox_QQ.Text.Trim() != "" && textBox_QQ.Text.Trim().Length > 4)
            {
                strContent = Login(textBox_QQ.Text.Trim());
                SysLog("��ȡ����֤��:" + strContent);
            }
            else
            {
                MessageBox.Show("QQ����������", "ϵͳ��Ϣ");
            }

            if (strContent != "")
            {
                vc_type = strContent;
                GetImageCode(strQQ, vc_type);
            }
        }

        #region ͨ��QQ�ŵõ���֤����ܴ� Login(string strQQ)
        /// <summary>
        /// ͨ��QQ�ŵõ���֤����ܴ�
        /// </summary>
        /// <param name="strQQ"></param>
        /// <returns></returns>
        private string Login(string strQQ)
        {            
            //��������
            string strRequest = null;
            string strContent = null;
            string resultString = null;
            string vc_type = null;
            string[] arrResult = { };
            this.strQQ = strQQ;

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

            return vc_type;
        }
        #endregion ͨ��QQ�ŵõ���֤����ܴ� Login(string strQQ)

        #region ��¼QQ�����ص�¼��Ϣ Login(string strQQ, string strPwd, string strImageCode)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strQQ"></param>
        /// <param name="strPwd"></param>
        /// <param name="strImageCode"></param>
        /// <returns>Key</returns>
        private string Login(string strQQ, string strPwd, string strImageCode)
        {
            string[] arrResult = { };
            string[] arrKey = { };
            string resultString = null;
            string strSkey = null;
            string strPtwebqq = null;
            string strPost = null;
            
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            strRequest = "http://ptlogin2.qq.com/login?u=" + strQQ + "&p=" + qqPwdEncrypt.Encrypt(strPwd, strImageCode) + "&verifycode=" + strImageCode + "&remember_uin=1&aid=1002101&u1=http%3A%2F%2Fweb.qq.com%2Fmain.shtml%3Fdirect__2&h=1&ptredirect=1&ptlang=2052&from_ui=1&pttype=1&dumy=&fp=loginerroralert";

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
                SysLog(arrResult[4]);
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
                SysLog(arrResult[4]);
            }
            else if (arrResult[0] == "2")   //2���Ѿ����ڵ�QQ���롣
            {
                SysLog(arrResult[4]);
            }
            else if (arrResult[0] == "3")   //3����������������������ԡ�
            {
                SysLog(arrResult[4]);
            }
            else if (arrResult[0] == "4")   //4�����������֤�����������ԡ�
            {
                SysLog(arrResult[4]);
            }
            else if (arrResult[0] == "5")   //5��У��ʧ�ܡ�
            {
                SysLog(arrResult[4]);
            }
            else if (arrResult[0] == "6")   //6�����������������޸Ĺ�����, ���Ժ��ٵ�¼.
            {
                SysLog(arrResult[4]);
            }
            else if (arrResult[0] == "7")   //7��������������, �����ԡ�
            {
                SysLog(arrResult[4]);
            }
            else                            //8������IP�������Ĵ������࣬���Ժ����ԡ�
            {
                SysLog("δ֪����");
            }
            SysLog(arrResult[1]);
            #endregion ��¼����ֵ�ж�

            strSkey = Regex.Replace(strSkey, ";.*", "").Replace("skey=", "");
            strPtwebqq = Regex.Replace(strPtwebqq, ";.*", "").Replace("ptwebqq=", "");

            strPost = strQQ + ";22;0;00000000;" + strSkey + ";" + strPtwebqq + ";0;";
            this.strRequest = "http://web-proxy.qq.com/conn_s";
            strContent = HTTPproc.Post(this.strRequest, strPost);
            arrKey = strContent.Split(';');
            return arrKey[4].ToString();
        }
        #endregion ��¼QQ�����ص�¼��Ϣ Login(string strQQ, string strPwd, string strImageCode)

        #region ��ȡ��֤�뷽�� GetImageCode(string strQQ,string vc_type)
        private void GetImageCode(string strQQ,string vc_type)
        {
            //��ָ����ַ���󷵻�������
            strRequest = "http://captcha.qq.com/getimage?aid=1002101&r=" + rnd.GetRandStr(13) + "&uin=" + strQQ + "&vc_type=" + vc_type;
            this.pictureBox_ImageCode.Image = Image.FromStream(new MemoryStream(HTTPproc.GetData(strRequest)));
        }
        #endregion ��ȡ��֤�뷽�� GetImageCode(string strQQ,string vc_type)

        #region ˢ����֤�뷽��
        private void pictureBox_ImageCode_Click(object sender, EventArgs e)
        {
            try
            {
                GetImageCode(strQQ, vc_type);
            }
            catch
            {

            }
        }
        #endregion ˢ����֤�뷽��

        #region �޸�ǩ�� ModifySign(string strQQ, int i, string strKey, string strContent)
        /// <summary>
        /// �޸�ǩ��
        /// </summary>
        /// <param name="strQQ">QQ��</param>
        /// <param name="i">��Ϣ����</param>
        /// <param name="strKey">����</param>
        /// <param name="strContent">ǩ������</param>
        private void ModifySign(string strQQ, int i, string strKey, string strContent)
        {
            i++;
            string strPost = strQQ + ";67;" + i + ";" + strKey + ";01;" + strContent + ";";
            //1349836289;67;512;ad7e15a4;01;Tests;
            //1349836289;67;14;701f4ae8;01;Tests;
            //1349836289;67;13;eac1407d;01;Cupid;
            //1349836289;67;15;eac1407d;01;Cupids;
            //1349836289;67;34;eac1407d;01;Cupid;
            HTTPproc.Post(this.strRequest, strPost);
        }

        #endregion �޸�ǩ�� ModifySign(string strQQ, string i, string strKey, string strContent)

        //���Ӻ���
        //��ʶ:80
        //1349836289;80;50680;41;182536608;1349836289;Test;1;1288270763;
        //1349836289;80;8772;41;182536608;1349836289;Test;1;1288271081;
        //1349836289;80;29061;41;182536608;1349836289;Test;1;1288271252;
        //1349836289;80;27158;41;182536608;1349836289;Test;1;1288271786;

        //��Ӻ���(����)
        //1349836289;a8;65;a01b4ae8;3;182536608;0;
        //1349836289;a8;90;a01b4ae8;3;182536608;0;
        //1349836289;a8;109;a01b4ae8;3;182536608;0;        

        #region ��Ӻ���(����) AddFriend(string strQQ,string i,string strKey,string strF_QQ,string strContent)
        /// <summary>
        /// ��Ӻ���(����)
        /// </summary>
        /// <param name="strQQ">QQ��</param>
        /// <param name="i">��Ϣ����</param>
        /// <param name="strKey">����</param>
        /// <param name="strF_QQ">����QQ��</param>
        /// <param name="strContent">��֤��Ϣ</param>
        private void AddFriend(string strQQ,string i,string strKey,string strF_QQ,string strContent)
        {
            //��Ӻ���(����)
            //1349836289;a8;33;701f4ae8;2;182536608;1;0;Test;
            //1349836289;a8;58;701f4ae8;2;182536608;1;0;Test;
            this.strRequest = strQQ + ";a8;" + i + ";" + strKey + ";2;" + strF_QQ + ";1;0;" + strContent + ";";
        }

        #endregion ��Ӻ���(����) AddFriend(string strQQ,string i,string strKey,string strF_QQ,string strContent)

        #region ������־ SysLog(string strLog)
        /// <summary>
        /// ������־
        /// </summary>
        /// <param name="strLog"></param>        
        private void SysLog(string strLog)
        {
            DateTime dt = DateTime.Now;
            textBox_SysLog.Text += dt + "  " + strLog + "\r\n";
            textBox_SysLog.SelectionStart = textBox_SysLog.Text.Length;
            textBox_SysLog.ScrollToCaret();
            //ʼ����ʾTextBox����һ�У�ʼ�չ�������ײ�
            textBox_SysLog.SelectionStart = textBox_SysLog.Text.Length;
            textBox_SysLog.ScrollToCaret();
        }
        #endregion

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

        private void button_ModifySign_Click(object sender, EventArgs e)
        {            
            ModifySign(this.strQQ, this.i, this.strKey, "Testssss");
        }
    }
}