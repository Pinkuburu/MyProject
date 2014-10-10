using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace C_wQQ
{
    class Converts
    {
        public static string ConvertUnicodeStringToChinese(string unicodeString)
        {
            if (string.IsNullOrEmpty(unicodeString))
                return string.Empty;

            string outStr = unicodeString;

            Regex re = new Regex("\\\\u[0123456789abcdef]{4}", RegexOptions.IgnoreCase);
            MatchCollection mc = re.Matches(unicodeString);
            foreach (Match ma in mc)
            {
                outStr = outStr.Replace(ma.Value, ConverUnicodeStringToChar(ma.Value).ToString());
            }
            return outStr;
        }

        private static char ConverUnicodeStringToChar(string str)
        {
            char outStr = Char.MinValue;
            outStr = (char)int.Parse(str.Remove(0, 2), System.Globalization.NumberStyles.HexNumber);
            return outStr;
        }

        public static string StrConvUrlEncoding(string strIn, string encoding)
        {
            return HttpUtility.UrlEncode(strIn, System.Text.Encoding.GetEncoding(encoding));
        }
        
        public static string ArrangeMessage(string content)
        {
            string result = content;
            string syh = "SYH#$%";
            string zkh = "ZKH#$%";
            string ykh = "YKH#$%";
            result = result.Replace("\\\"", syh);
            result = result.Replace(@"\(", zkh);
            result = result.Replace(@"\)", ykh);
            result = result.Replace("\",[", "\"[");
            result = result.Replace("],[", "][");
            result = result.Replace("],\"", "]\"");
            Regex cface = new Regex("(\"cface\")(?<v>.+?)(\\])");
            Regex offpic = new Regex("(\"offpic\")(?<v>.+?)(\\])");
            Regex face = new Regex("(\"face\",)(?<facenum>[0-9]{1,3}?)(\\])");
            foreach (Match item in cface.Matches(result))
            {
                result = result.Replace("[\"cface\"" + item.Groups["v"].Value + "]", "(自定义表情)");
            }
            foreach (Match item in offpic.Matches(result))
            {
                result = result.Replace("[\"offpic\"" + item.Groups["v"].Value + "]", "(图片)");
            }
            foreach (Match item in face.Matches(result))
            {
                result = result.Replace("[\"face\"," + item.Groups["facenum"].Value + "]", "(表情" + item.Groups["facenum"].Value + ")");
            }
            result = result.Replace("\"", "");
            result = result.Replace(syh, "\"");
            result = result.Replace(zkh, "(");
            result = result.Replace(ykh, ")");
            result = result.Replace(@"\r\n", Environment.NewLine);
            result = result.Replace(@"\r", Environment.NewLine);
            return result;
        }

    }
}
