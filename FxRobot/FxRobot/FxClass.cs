using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections;

namespace FxRobot
{
    public class FxClass
    {
        //��ʼ��HTTP��
        WebClient HTTPproc = new WebClient();
        ArrayList FriendList = new ArrayList();
        public string ssid = null;
        public int intVersion = 0;        

        #region ���ŵ�¼ Fx_Login(string strMobile, string strPwd, string Code, int Status)
        /// <summary>
        /// ���ŵ�¼
        /// </summary>
        /// <param name="strMobile">�ֻ���</param>
        /// <param name="strPwd">����</param>
        /// <param name="Code">��֤��</param>
        /// <param name="Status">��¼״̬:400����</param>
        /// <returns></returns>
        public string Fx_Login(string strMobile, string strPwd, string Code, int Status)
        {
            int intCode = 0;
            string strRequest = null;
            string strContent = null;
            string strParameter = null;
            WebClient.strMobile = strMobile;
            //Status:400����
            //ccpsession       e7548ddf-66d7-4773-bbdc-ef43c514a84e                 
            strRequest = "https://webim.feixin.10086.cn/WebIM/Login.aspx";
            strParameter = "UserName=" + strMobile + "&Pwd=" + strPwd + "&Ccp=" + Code + "&OnlineStatus=" + Status;

            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            strContent = HTTPproc.Post(strRequest, strParameter);
            JObject o = JObject.Parse(Unicode2Character(strContent));
            intCode = (int)o["rc"];

            switch (intCode)
            {
                case 312:
                    strContent = "��֤�����~";
                    break;
                case 321:
                    strContent = "�������~";
                    break;
                case 404:
                    strContent = "��δ��ͨ���ŷ���~";
                    break;
                case 200:
                    strContent = "��¼�ɹ�~";
                    this.ssid = HTTPproc.ResponseHeaders.GetValues(1).GetValue(0).ToString().Replace("webim_sessionid=", "").Replace("; path=/", "");
                    GetPersonalInfo();
                    GetContactList();
                    GetFriendList();
                    break;
            }

            return strContent;
        }
        #endregion ���ŵ�¼ Fx_Login(string strMobile, string strPwd, string Code, int Status)

        #region ��ȡ�û���Ϣ GetPersonalInfo()
        /// <summary>
        /// ��ȡ�û���Ϣ
        /// </summary>
        /// <returns></returns>
        private string GetPersonalInfo()
        {
            string strRequest = null;
            string strContent = null;
            string strParameter = null;

            strRequest = "http://webim.feixin.10086.cn/WebIM/GetPersonalInfo.aspx?Version=0";
            strParameter = "ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);
            
            //��ȡ�û���Ϣ�Ͳ�д�ˣ�����Ҳû��ʲô��˼

            return strContent;
        }
        #endregion ��ȡ�û���Ϣ GetPersonalInfo()

        #region ��ȡ���Ѽ�������Ϣ GetContactList()
        /// <summary>
        /// ��ȡ���Ѽ�������Ϣ
        /// </summary>
        /// <returns></returns>
        private string GetContactList()
        {            
            string strRequest = null;
            string strContent = null;
            string strParameter = null;

            strRequest = "http://webim.feixin.10086.cn/WebIM/GetContactList.aspx?Version=" + Version();
            strParameter = "ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);                    

            return strContent;
        }
        #endregion ��ȡ���Ѽ�������Ϣ GetContactList()

        #region ��ȡ�����б� GetFriendList()
        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <returns></returns>
        private string GetFriendList()
        {
            string strRequest = null;
            string strContent = null;
            string strParameter = null;

            strRequest = "http://webim.feixin.10086.cn/WebIM/GetConnect.aspx?Version=" + Version();
            strParameter = "ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);

            JObject o = JObject.Parse((Unicode2Character(strContent)));
            JArray friends = (JArray)o["rv"];

            try
            {
                foreach (JObject uid in friends)
                {
                    FriendList.Add(uid["Data"]);
                }
                strContent = "��ȡ�����б�ɹ�!";
            }
            catch
            {
                strContent = "��ȡ�����б�ʧ��!";
            }
            return strContent;
        }
        #endregion ��ȡ�����б� GetFriendList()

        /// <summary>
        /// �ػ�����
        /// </summary>
        public string GetConnect()
        {
            string strRequest = null;
            string strContent = null;
            string strParameter = null;

            strRequest = "http://webim.feixin.10086.cn/WebIM/GetConnect.aspx?Version=" + Version();
            strParameter = "ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);

            JObject o = JObject.Parse((Unicode2Character(strContent)));

            try
            {
                JArray Event = (JArray)o["rv"];
                if ((int)Event[0]["DataType"] == 3)
                {
                    strContent = SendMsg((int)Event[0]["Data"]["fromUid"], (string)Event[0]["Data"]["msg"], 0);
                }
            }
            catch
            {

            }      
      
            return strContent;
        }

        //����������ʾ״̬
        //{"rc":200,"rv":[{"DataType":2,"Data":{"pb":"400","pd":"","dt":"WEB","dc":"463","uid":289724462}}]}
        //{"rc":200,"rv":[{"DataType":2,"Data":{"pb":"600","pd":"æµ","dt":"WEB","dc":"463","uid":289724462}}]}
        //{"rc":200,"rv":[{"DataType":2,"Data":{"sms":"365.0:0:0","pb":"0","pd":"","dt":"","dc":"0","uid":289724462}}]}
        //{"rc":200,"rv":[{"DataType":2,"Data":{"pb":"0","pd":"","dt":"","dc":"0","uid":289724462}}]}

        public string ShowFriendList()
        {
            StringBuilder sb = new StringBuilder();
            foreach (JObject userInfo in FriendList)
            {
                sb.Append(userInfo.ToString().Replace("\r\n","").Replace("  ","") + "\r\n");
            }
            return sb.ToString();
        }        

        #region ������Ϣ SendMsg(int To, string strMsg, int IsSendSms)
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="To">���ŷ�����</param>
        /// <param name="strMsg">��Ϣ����</param>
        /// <param name="IsSendSms">��Ϣ���ͣ�0������Ϣ��1������Ϣ</param>
        /// <returns></returns>
        private string SendMsg(int To, string strMsg, int IsSendSms)
        {
            string strRequest = null;
            string strContent = null;
            string strParameter = null;
            int intCode = 0;

            strRequest = "http://webim.feixin.10086.cn/WebIM/SendMsg.aspx?Version=" + Version();
            strParameter = "To=" + To + "&IsSendSms=" + IsSendSms + "&msg=" + strMsg + "&ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);

            JObject o = JObject.Parse(Unicode2Character(strContent));
            intCode = (int)o["rc"];

            if (intCode == 200)
            {
                strContent = "������Ϣ��" + strMsg + "�ɹ�!";
            }
            else
            {
                strContent = "������Ϣ��" + strMsg + "ʧ��!";
            }

            return strContent;
        }
        #endregion ������Ϣ SendMsg(string To, string strMsg, int IsSendSms)

        #region ����ǩ�� SetPersonalInfo(string strImpresa)
        /// <summary>
        /// ����ǩ��
        /// </summary>
        /// <param name="strImpresa">ǩ������</param>
        /// <returns></returns>
        private string SetPersonalInfo(string strImpresa)
        {            
            string strRequest = null;
            string strContent = null;
            string strParameter = null;
            int intCode = 0;

            strRequest = "http://webim.feixin.10086.cn/WebIM/SetPersonalInfo.aspx?Version=" + Version();
            strParameter = "Impresa=" + strImpresa + "&ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);

            JObject o = JObject.Parse(Unicode2Character(strContent));
            intCode = (int)o["rc"];

            if (intCode == 200)
            {
                strContent = "ǩ���޸�Ϊ��" + strImpresa + "�ɹ�!";
            }
            else
            {
                strContent = "ǩ���޸�Ϊ��" + strImpresa + "ʧ��!";
            }

            return strContent;
        }
        #endregion ����ǩ�� SetPersonalInfo(string strImpresa)

        #region ��������״̬ SetPresence(int intStatus)
        /// <summary>
        /// ��������״̬
        /// </summary>
        /// <param name="intStatus"></param>
        /// <returns></returns>
        private string SetPresence(int intStatus)
        {            
            string strRequest = null;
            string strContent = null;
            string strParameter = null;
            string strCustom = null;
            int intCode = 0;

            switch (intStatus)
            {
                case 400:
                    strCustom = "����";
                    break;
                case 600:
                    strCustom = "æµ";
                    break;
                case 100:
                    strCustom = "�뿪";
                    break;
                case 0:
                    strCustom = "����";
                    break;
                case 300:
                    strCustom = "���ϻ���";
                    break;
                case 850:
                    strCustom = "������";
                    break;
                case 500:
                    strCustom = "�ӵ绰";
                    break;
                case 150:
                    strCustom = "����Ͳ�";
                    break;
            }

            strRequest = "http://webim.feixin.10086.cn/WebIM/SetPresence.aspx?Version=" + Version();
            strParameter = "Presence=" + intStatus + "&Custom=" + UrlEncode(strCustom, "UTF-8") + "+&ssid=" + this.ssid;
            strContent = HTTPproc.Post(strRequest, strParameter);

            JObject o = JObject.Parse(Unicode2Character(strContent));
            intCode = (int)o["rc"];

            if (intCode == 200)
            {
                strContent = "���ã�" + strCustom + "״̬�ɹ�!";
            }
            else
            {
                strContent = "���ã�" + strCustom + "״̬ʧ��!";
            }
            
            return strContent;
        }
        #endregion ��������״̬ SetPresence(int intStatus)

        #region ���������� Version()
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        private string Version()
        {
            this.intVersion++;
            return this.intVersion.ToString();
        }
        #endregion ���������� Version()

        #region ��ȡ��֤�� Fx_ImageCode()
        /// <summary>
        /// ��ȡ��֤��
        /// </summary>
        public void Fx_ImageCode()
        {
            string strRequest = null;
            strRequest = "http://webim.feixin.10086.cn/WebIM/GetPicCode.aspx?Type=ccpsession&" + Timestamp();            
            HTTPproc.DownloadFile(strRequest, @"Code.jpg");
        }
        #endregion ��ȡ��֤�� Fx_ImageCode()

        #region ʱ��� Timestamp()
        /// <summary>
        /// ʱ���
        /// </summary>
        /// <returns></returns>
        private long Timestamp()
        {
            long longTimestamp = 0;
            longTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return longTimestamp;
        }
        #endregion ʱ��� Timestamp()

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

        #region ��Unicodeת��ΪCharacter Unicode2Character(string str)
        /// <summary>
        /// ��Unicodeת��ΪCharacter
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
        /// <returns></returns>
        private string Unicode2Character(string str)
        {
            string text = str;
            string strPattern = "(?<code>\\\\u[A-F0-9]{4})";
            do
            {
                Match m = Regex.Match(text, strPattern, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    string strValue = m.Groups["code"].Value;
                    int i = System.Int32.Parse(strValue.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                    char ch = Convert.ToChar(i);
                    text = text.Replace(strValue, ch.ToString());
                }
                else
                {
                    break;
                }
            }
            while (true);

            return text;
        }
        #endregion
    }
}
