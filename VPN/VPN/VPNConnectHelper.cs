using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using DotRas;

namespace VPN {
    class VPNConnectHelper {

        private static RasVpnStrategy rasstart = RasVpnStrategy.L2tpFirst;
        private static RasPhoneBook pb = new RasPhoneBook();

        private static readonly string VPNNAME = "VPN1";

        private static RasDialer dialer = new RasDialer();

        public delegate void DialStateChangeHandler( RasDialer dialer,  StateChangedEventArgs e);
        public static event DialStateChangeHandler DialStateChange;

        public delegate void DialAsyncCompleteHandler( RasDialer dialer , DialCompletedEventArgs e);
        public static event DialAsyncCompleteHandler DialAsyncComplete;

        static VPNConnectHelper() {
            pb.Open();
            dialer.EntryName = VPNNAME;
            //dialer.Timeout = 20000;
            dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers);
            dialer.StateChanged += new EventHandler<StateChangedEventArgs>(dialer_StateChanged);
            dialer.DialCompleted += new EventHandler<DialCompletedEventArgs>(dialer_DialCompleted);
        }

        private static void CreateOrUpdateVPNEntry( string ip, string user, string pwd ) {
            RasEntry entry;
            if (!pb.Entries.Contains(VPNNAME)) {
                entry = RasEntry.CreateVpnEntry(VPNNAME, ip, rasstart, RasDevice.GetDeviceByName("(L2TP)", RasDeviceType.Vpn));
                pb.Entries.Add(entry);
            } else {
                entry = pb.Entries[VPNNAME];
                entry.PhoneNumber = ip;
                IPAddress _ip;
                IPAddress.TryParse(ip, out _ip);
                entry.IPAddress = _ip;
            }
            NetworkCredential nc = new NetworkCredential(user,pwd);
            entry.UpdateCredentials(nc);
            entry.Update();
        }

        public static void DialAsync( string ip, string user, string pwd ) {
            CreateOrUpdateVPNEntry(ip, user, pwd);
            Disconnect();
            dialer.DialAsync();
        }

        public static void Dial( string ip, string user , string pwd ) {
            CreateOrUpdateVPNEntry(ip, user, pwd);
            Disconnect();
            dialer.Dial();
        }

        public static void CancelDialAsync() {
            dialer.DialAsyncCancel();
        }

        public static void Disconnect() {
            dialer.DialAsyncCancel();
            foreach( RasConnection conn in dialer.GetActiveConnections() ) {
                if( conn.EntryName == VPNNAME ) {
                    conn.HangUp();
                    return;
                }
            }
        }

        private static void dialer_DialCompleted(object sender, DialCompletedEventArgs e) {
            DialAsyncComplete((RasDialer)sender, e);
        }

        private static void dialer_StateChanged(object sender, StateChangedEventArgs e) {
            DialStateChange((RasDialer)sender, e);
        }

        #region
        //RasVpnStrategy rasstart = RasVpnStrategy.PptpFirst;
        //RasPhoneBook pb = new RasPhoneBook();
        //pb.Open();
        //if (!pb.Entries.Contains("VPN1")) {
        //    RasEntry entry = RasEntry.CreateVpnEntry("VPN1", "210.116.114.153", rasstart, RasDevice.GetDeviceByName("(L2TP)", RasDeviceType.Vpn));
        //    pb.Entries.Add(entry);
        //    NetworkCredential nc = new NetworkCredential("iusr_g3way_152", "hack58vpn");
        //    entry.UpdateCredentials(nc);
        //    //entry.ExtendedOptions = RasEntryExtendedOptions.UsePreSharedKey;
        //    //entry.Options = RasEntryOptions.PreviewUserPassword;
        //    //entry.Options += RasEntryOptions.ShowDialingProgress;
        //    //entry.Options += RasEntryOptions.PreviewDomain;
        //    entry.Update();
        //}
        //RasDialer dialer = new RasDialer();
        //dialer.EntryName = "VPN1";
        //dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers);
        //dialer.StateChanged += new EventHandler<StateChangedEventArgs>(dialer_StateChanged);
        //dialer.DialCompleted += new EventHandler<DialCompletedEventArgs>(dialer_DialCompleted);
        //dialer.DialAsync();

        //void dialer_DialCompleted(object sender, DialCompletedEventArgs e) {
        //    string msg = "";
        //    RasDialer dialer = (RasDialer)sender;
        //    msg = e.Connected ? "连接成功" : "连接失败" + "\r\n" + dialer.PhoneNumber + "\r\n" + e.Error.Message;
        //    notifyIcon1.ShowBalloonTip(5, "...", msg, ToolTipIcon.Info);
        //}

        //void dialer_StateChanged(object sender, StateChangedEventArgs e) {

        //}

        #endregion


    }
}
