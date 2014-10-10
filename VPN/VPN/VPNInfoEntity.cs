using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace VPN {
    [Serializable]
    class VPNInfoEntity {

        public enum AvailableStatus {
            Disable = -1,
            Unknow = 0,
            Enable = 1
        }

        private string _area = "", _ip = "", _user = "", _pwd = "", _info1 = "", _info2 = "", _info3 = "";
        private long _ping = 0;
        private AvailableStatus _available = AvailableStatus.Unknow;

        public long Ping {
            get { return _ping; }
            set { _ping = value; }
        }

        public AvailableStatus Available {
            get { return _available; }
            set { _available = value; }
        }

        public string Area {
            get { return _area; }
            set { _area = value; }
        }

        public string Ip {
            get { return _ip; }
            set { _ip = value; }
        }

        public string User {
            get { return _user; }
            set { _user = value; }
        }

        public string Pwd {
            get { return _pwd; }
            set { _pwd = value; }
        }

        public string Info1 {
            get { return _info1; }
            set { _info1 = value; }
        }

        public string Info2 {
            get { return _info2; }
            set { _info2 = value; }
        }

        public string Info3 {
            get { return _info3; }
            set { _info3 = value; }
        }
    }
}
