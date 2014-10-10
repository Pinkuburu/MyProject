using System;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Drawing;

namespace OnlineWeather_Web
{
    public partial class _Default : System.Web.UI.Page
    {
        OnlineWeather_Web.WebClient HTTPproc = new WebClient();

        protected void Page_Load(object sender, EventArgs e)
        {
			//����
            //������
            //����
            //С��
            //��
            //С������
            IPScaner objScan = new IPScaner();
            string ip = "61.133.200.41";
            objScan.DataPath = Server.MapPath(@"Data\QQWry.Dat");
            objScan.IP = ip;
            objScan.IPLocation();
            
            string addre = objScan.Country.ToString();
            string err = objScan.ErrMsg;
            ReadWeatherInfo(ReadWeatherID("�ൺ"));

            //��������ʾ��������
            System.Drawing.Bitmap bmp = new Bitmap(Bitmap.FromFile(Server.MapPath(@"Image\model-1.gif")));//����ͼƬ   
            System.Drawing.Graphics g = Graphics.FromImage(bmp);
            ////g.DrawString("" + j + " ��", new Font("����", 9), new SolidBrush(Color.Red), 65, 9);
            ////g.DrawString("" + z + " ��", new Font("����", 9), new SolidBrush(Color.Red), 65, 26);
            ////g.DrawString("" + tongji + " ��", new Font("����", 9), new SolidBrush(Color.Red), 67, 42);

            //����ѡ���ı�������ɫ   
            g.Dispose();
            //���GIF,��Ҫ������ʽҲ�����Լ���   
            Response.ContentType = "image/gif";
            bmp.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
        }

        #region ͨ���������ƶ�ȡ����Ԥ��ID ReadWeatherID(string strCity)
        /// <summary>
        /// ͨ���������ƶ�ȡ����Ԥ��ID
        /// </summary>
        /// <param name="strCity">��������</param>
        /// <returns>����ID</returns>
        private string ReadWeatherID(string strCity)
        {
            string strResult = null;
            string strWeatherID = null;
            string resultString = null;

            RandStr rndNum = new RandStr(true,false,false,false);            
            string strRequest = "http://toy.weather.com.cn/SearchBox/searchBox?callback=jsonp" + rndNum.GetRandStr(13) + "&_=" + rndNum.GetRandStr(13) + "&language=zh&keyword=" + strCity + "";

            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            strResult = HTTPproc.OpenRead(strRequest);
            try
            {
                resultString = Regex.Replace(strResult, @"jsonp\d{0,13}\(", "").Replace(");","");
                JObject o = JObject.Parse(resultString);
                JArray WeatherID = (JArray)o["i"];
                strWeatherID = (string)WeatherID[0]["i"];
            }
            catch (ArgumentException ex)
            {
                strWeatherID = "101120201";
            }
            return strWeatherID;
        }
        #endregion ͨ���������ƶ�ȡ����Ԥ��ID ReadWeatherID(string strCity)

        private void ReadWeatherInfo(string strWeatherID)
        {
            string strResult = null;
            string strJsonResult = null;
            string resultString = null;
            string strRequest = "http://m.weather.com.cn/data/" + strWeatherID + ".html";

            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            strResult = HTTPproc.OpenRead(strRequest);
            try
            {
                resultString = Regex.Replace(strResult, @"\d{0,3}\r\n\{", "{");
                resultString = Regex.Replace(resultString, @"\}\r\n0", "}");
            }
            catch
            {
 
            }

            try
            {
                JObject o = JObject.Parse(resultString);
                strJsonResult = (string)o["weatherinfo"]["city"] + "|";

                strJsonResult += (string)o["weatherinfo"]["img1"] + "|";
                strJsonResult += (string)o["weatherinfo"]["img2"] + "|";
                strJsonResult += (string)o["weatherinfo"]["temp1"] + "|";

                strJsonResult += (string)o["weatherinfo"]["img3"] + "|";
                strJsonResult += (string)o["weatherinfo"]["img4"] + "|";
                strJsonResult += (string)o["weatherinfo"]["temp2"] + "|";

                strJsonResult += (string)o["weatherinfo"]["img5"] + "|";
                strJsonResult += (string)o["weatherinfo"]["img6"] + "|";
                strJsonResult += (string)o["weatherinfo"]["temp4"];
            }
            catch
            {
 
            }
        }

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
