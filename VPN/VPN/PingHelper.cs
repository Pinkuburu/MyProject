using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace VPN {
    class PingHelper {

        public delegate void DlgPingCompleteHandler(object sender , PingCompletedEventArgs p , params object[] parameters);
        public event DlgPingCompleteHandler PingComplete = null;

        private object[] ps;

        public void PingIP(string ip, params object[] ps) {
            Ping p = new Ping();
            //Console.WriteLine(">>> {0}" , ip);
            this.ps = ps;
            p.PingCompleted += new PingCompletedEventHandler(p_PingCompleted);
            p.SendAsync(ip , 5000 , null);
        }

        private void p_PingCompleted(object sender , PingCompletedEventArgs e) {
            //Console.WriteLine("{0}  {1}",e.Reply.Address.ToString(),e.Reply.RoundtripTime);
            PingComplete(this , e , ps);
            Ping ping = (Ping)sender;
            ping.Dispose();
        }

    }
}
