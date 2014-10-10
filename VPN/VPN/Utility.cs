using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

public static class Utility {
    public static string ParseString(this string s, string key, bool ignoreCase) {
        Dictionary<string, string> kvs = s.ParseString(ignoreCase);
        if (kvs.ContainsKey(key)) {
            return kvs[key];
        }
        return "";
    }

    public static Dictionary<string, string> ParseString(this string s, bool ignoreCase) {

        if (s.IndexOf('?') != -1) {
            s = s.Remove(0, s.IndexOf('?'));
        }

        Dictionary<string, string> kvs = new Dictionary<string, string>();
        Regex reg = new Regex(@"[\?&]?(?<key>[^=]+)=(?<value>[^\&]*)", RegexOptions.Compiled | RegexOptions.Multiline);
        MatchCollection ms = reg.Matches(s);
        string key;
        foreach (Match ma in ms) {
            key = ignoreCase ? ma.Groups["key"].Value.ToLower() : ma.Groups["key"].Value;
            if (kvs.ContainsKey(key)) {
                kvs[key] += "," + ma.Groups["value"].Value;
            } else {
                kvs[key] = ma.Groups["value"].Value;
            }
        }

        return kvs;
    }

    //String.prototype.setKeyValue = function (key, value) {
    //    if (this.queryString(key) != null) {
    //        var reg = new RegExp("([\?\&])(" + key + "=)([^\&]*)(\&?)", "i");
    //        return this.replace(reg, "$1$2" + value + "$4");
    //    } else {
    //        var add = arguments[2];
    //        if (add === true) {
    //            return this + (this.indexOf("?") > -1 ? "&" : "?") + key + "=" + value;
    //        } else return this;
    //    }
    //}

    public static string SetUrlKeyValue( this string url, string key, string value , Encoding encode) {
        if (url.ParseString(key, true).Trim() != "") {
            Regex reg = new Regex(@"([\?\&])(" + key + @"=)([^\&]*)(\&?)");
            return reg.Replace(url, "$1$2" + HttpUtility.UrlEncode(value,encode)  + "$4");
        } else {
            return url + (url.IndexOf('?') > -1 ? "&" : "?") + key + "=" + HttpUtility.UrlEncode(value, encode);
        }
    }
}