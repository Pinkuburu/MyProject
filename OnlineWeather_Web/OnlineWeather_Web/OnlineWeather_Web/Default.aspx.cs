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
			//多云
            //雷阵雨
            //阵雨
            //小雨
            //晴
            //小到中雨
            IPScaner objScan = new IPScaner();
            string ip = "61.133.200.41";
            objScan.DataPath = Server.MapPath(@"Data\QQWry.Dat");
            objScan.IP = ip;
            objScan.IPLocation();
            
            string addre = objScan.Country.ToString();
            string err = objScan.ErrMsg;
            ReadWeatherInfo(ReadWeatherID("青岛"));

            //下面是显示出计数器
            System.Drawing.Bitmap bmp = new Bitmap(Bitmap.FromFile(Server.MapPath(@"Image\model-1.gif")));//载入图片   
            System.Drawing.Graphics g = Graphics.FromImage(bmp);
            ////g.DrawString("" + j + " 次", new Font("宋体", 9), new SolidBrush(Color.Red), 65, 9);
            ////g.DrawString("" + z + " 次", new Font("宋体", 9), new SolidBrush(Color.Red), 65, 26);
            ////g.DrawString("" + tongji + " 次", new Font("宋体", 9), new SolidBrush(Color.Red), 67, 42);

            //这里选择文本字体颜色   
            g.Dispose();
            //输出GIF,你要其它格式也可以自己改   
            Response.ContentType = "image/gif";
            bmp.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
        }

        #region 通过城市名称读取天气预报ID ReadWeatherID(string strCity)
        /// <summary>
        /// 通过城市名称读取天气预报ID
        /// </summary>
        /// <param name="strCity">城市名称</param>
        /// <returns>城市ID</returns>
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
        #endregion 通过城市名称读取天气预报ID ReadWeatherID(string strCity)

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

        #region Url编码 UrlEncode(string url)
        /// <summary>
        /// Url编码
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
}
