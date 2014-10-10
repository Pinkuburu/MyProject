using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace VPN {
    class VpnListHelper {
        private static readonly string feiYiUrl = "http://www.feiyisoft.cn/vpn/server2.txt";
        private static readonly Regex feiYiReg = new Regex(@"(?<area>.*?),(?<ip>\d+\.\d+\.\d+\.+\d+),(?<user>.*?),(?<pwd>.*?),(?<info1>.*?),(?<info2>.*?),(?<info3>.*?)\+");

        public static List<VPNInfoEntity> GetFeiYiVPNS() {
            //WebHeaderCollection headers;
            //CookieCollection cookies;
            //string ctx = HttpRequest.GetCtx(feiYiUrl, "GET", Encoding.GetEncoding("GB2312"), null, null, "", out headers, out cookies);
            string ctx = HttpRequest.GetCtx(feiYiUrl, "GET", Encoding.GetEncoding("GB2312"));
            MatchCollection ms = feiYiReg.Matches(ctx);

            List<VPNInfoEntity> list = new List<VPNInfoEntity>();
            foreach (Match ma in ms) {
                VPNInfoEntity entity = new VPNInfoEntity();
                entity.Area = ma.Groups["area"].Value;
                entity.Ip = ma.Groups["ip"].Value;
                entity.User = ma.Groups["user"].Value;
                entity.Pwd = ma.Groups["pwd"].Value;
                entity.Info1 = ma.Groups["info1"].Value;
                entity.Info2 = ma.Groups["info2"].Value;
                entity.Info3 = ma.Groups["info3"].Value;

                if( SerializHelper.DisableList.ContainsKey(SerializHelper.GetEntityKey(entity))){
                    entity.Available = VPNInfoEntity.AvailableStatus.Disable;
                }

                list.Add(entity);
            }
            
            return list;
        }
    }
}
