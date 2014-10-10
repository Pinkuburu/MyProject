using System;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Text;

//=====================
//    QQ机器人插件     
//    作者:Cupid       
// 创建日期:2010-11-19 
//=====================

namespace QQRobot_Module
{
    public class QQRobot_Plugins
    {
        QQRobot_Module.WebClient HTTPproc = new WebClient();

        #region 天气查询_API GetWeather(string strCity)
        /// <summary>
        /// 天气查询_API
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
                    strJsonResult = strNickName + "，很抱歉，没有查找到“" + strCity + "”的天气信息，你确定它不是在火星？";
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
                strJsonResult = strNickName + "，以下是城市“" + strCity + "”的72小时天气预报\n\n";
                strJsonResult += (string)o["weatherinfo"]["date_y"] + " ";
                strJsonResult += (string)o["weatherinfo"]["date"] + " ";
                strJsonResult += (string)o["weatherinfo"]["week"] + " ";
                strJsonResult += (string)o["weatherinfo"]["city"] + "\n";
                strJsonResult += "今天，";
                strJsonResult += (string)o["weatherinfo"]["weather1"] + "，";
                strJsonResult += (string)o["weatherinfo"]["temp1"] + "，";
                strJsonResult += (string)o["weatherinfo"]["wind1"] + "\n";
                strJsonResult += "明天，";
                strJsonResult += (string)o["weatherinfo"]["weather2"] + "，";
                strJsonResult += (string)o["weatherinfo"]["temp2"] + "，";
                strJsonResult += (string)o["weatherinfo"]["wind2"] + "\n";
                strJsonResult += "后天，";
                strJsonResult += (string)o["weatherinfo"]["weather3"] + "，";
                strJsonResult += (string)o["weatherinfo"]["temp3"] + "，";
                strJsonResult += (string)o["weatherinfo"]["wind3"] + "\n";
            }
            catch
            {
                strJsonResult = strNickName + "，很抱歉，没有查找到“" + strCity + "”的天气信息，你确定它不是在火星？";
            }
            return strJsonResult;
        }
        #endregion 天气查询_API GetWeather(string strCity)

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

        #region 时间戳 Timestamp()
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        private long Timestamp()
        {
            long longTimestamp = 0;
            longTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return longTimestamp;
        }
        #endregion 时间戳 Timestamp()

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
