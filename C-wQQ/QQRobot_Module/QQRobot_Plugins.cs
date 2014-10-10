using System;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Text;

//=====================
//    QQ�����˲��     
//    ����:Cupid       
// ��������:2010-11-19 
//=====================

namespace QQRobot_Module
{
    public class QQRobot_Plugins
    {
        QQRobot_Module.WebClient HTTPproc = new WebClient();

        #region ������ѯ_API GetWeather(string strCity)
        /// <summary>
        /// ������ѯ_API
        /// </summary>
        /// <param name="strCity"></param>
        public string GetWeather(string strCity, string strNickName)
        {
            string strResult = null;
            string strJsonResult = null;
            string resultString = null;
            string strRequest = null;
            string strWeatherID = null;

            strRequest = "http://toy.weather.com.cn/SearchBox/searchBox?callback=jsonp" + Timestamp() + "&_=" + Timestamp() + "&language=zh&keyword=" + strCity + "";

            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            strResult = HTTPproc.OpenRead(strRequest);
            try
            {
                resultString = Regex.Replace(strResult, @"jsonp\d{0,13}\(", "").Replace(");", "");
                if (resultString == "{}")
                {
                    strJsonResult = strNickName + "���ܱ�Ǹ��û�в��ҵ���" + strCity + "����������Ϣ����ȷ���������ڻ��ǣ�";
                    return strJsonResult;
                }
                JObject o = JObject.Parse(resultString);
                JArray WeatherID = (JArray)o["i"];
                strWeatherID = (string)WeatherID[0]["i"];
            }
            catch (ArgumentException ex)
            {
                strWeatherID = "101120201";
            }

            strRequest = "http://m.weather.com.cn/data/" + strWeatherID + ".html";

            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            strResult = HTTPproc.OpenRead(strRequest);
            try
            {
                resultString = Regex.Replace(strResult, @".*[0-9,a-z]\r\n\{", "{");
                resultString = Regex.Replace(resultString, @"\}\r\n0", "}");
            }
            catch
            {

            }

            try
            {
                JObject o = JObject.Parse(resultString);
                strJsonResult = strNickName + "�������ǳ��С�" + strCity + "����72Сʱ����Ԥ��\n\n";
                strJsonResult += (string)o["weatherinfo"]["date_y"] + " ";
                strJsonResult += (string)o["weatherinfo"]["date"] + " ";
                strJsonResult += (string)o["weatherinfo"]["week"] + " ";
                strJsonResult += (string)o["weatherinfo"]["city"] + "\n";
                strJsonResult += "���죬";
                strJsonResult += (string)o["weatherinfo"]["weather1"] + "��";
                strJsonResult += (string)o["weatherinfo"]["temp1"] + "��";
                strJsonResult += (string)o["weatherinfo"]["wind1"] + "\n";
                strJsonResult += "���죬";
                strJsonResult += (string)o["weatherinfo"]["weather2"] + "��";
                strJsonResult += (string)o["weatherinfo"]["temp2"] + "��";
                strJsonResult += (string)o["weatherinfo"]["wind2"] + "\n";
                strJsonResult += "���죬";
                strJsonResult += (string)o["weatherinfo"]["weather3"] + "��";
                strJsonResult += (string)o["weatherinfo"]["temp3"] + "��";
                strJsonResult += (string)o["weatherinfo"]["wind3"] + "\n";
            }
            catch
            {
                strJsonResult = strNickName + "���ܱ�Ǹ��û�в��ҵ���" + strCity + "����������Ϣ����ȷ���������ڻ��ǣ�";
            }
            return strJsonResult;
        }
        #endregion ������ѯ_API GetWeather(string strCity)

        #region Url���� UrlEncode(string url)
        /// <summary>
        /// Url����
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string UrlEncode(string url)
        {
            byte[] bs = Encoding.GetEncoding("GB2312").GetBytes(url);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bs.Length; i++)
            {
                if (bs[i] < 128)
                {
                    sb.Append((char)bs[i]);
                }
                else
                {
                    sb.Append("%" + bs[i++].ToString("x").PadLeft(2, '0'));
                    sb.Append("%" + bs[i].ToString("x").PadLeft(2, '0'));
                }
            } return sb.ToString();
        }
        #endregion

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
