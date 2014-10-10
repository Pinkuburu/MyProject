using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace C_wQQ
{
    public class Friend
    {
        public string uin = string.Empty;
        public string name = string.Empty;
        public string card = string.Empty;  // 群名片
        public string stat = string.Empty;  // 群状态
        public int face = 0;
        public string groupid = string.Empty;
        public bool isflash = false;
        public int groupindex = -1;
        public int itemindex = -1;
        public int onlineindex = -1;
        //public bool isonline = false;
    }

    public class Group
    {
        public string name = string.Empty;
        public int membernum = 0;
        public int groupindex = -1;
        public int onlineindex = 0;
    }

    public class OnLineFriend
    {
        public string uin = string.Empty;
        public string state = string.Empty;
        public int client_type = 0;
    }

    public class QQGroup
    {
        public string name = string.Empty;
        public string code = string.Empty;
        public string gid = string.Empty;
        public string flag = string.Empty;
        public bool isflash = false;
        private bool isset = false;
        public int itemindex = -1;
        public Dictionary<string, Friend> members = new Dictionary<string, Friend>();

        public void setgourpmembers(string value)
        {
            if (!isset)
            {
                Regex mininfo = new Regex("(\"minfo\":\\[)(?<members>.+?)(\\],)");
                string temp = mininfo.Match(value).Groups["members"].Value;
                Regex minmember = new Regex("({\"uin\":)(?<uin>.+?)(,\"nick\":)(?<nick>.+?)(\"})");
                foreach (Match item in minmember.Matches(temp))
                {
                    Friend f = new Friend();
                    f.uin = item.Groups["uin"].Value;
                    f.name = Converts.ConvertUnicodeStringToChinese(item.Groups["nick"].Value.Substring(1));
                    members.Add(f.uin, f);
                }
                Regex membercard = new Regex("(\"cards\":\\[)(?<members>.+?)(\\]}})");
                temp = membercard.Match(value).Groups["members"].Value;
                Regex member = new Regex("({\"muin\":)(?<muin>.+?)(,\"card\":\")(?<card>.+?)(\"})");
                foreach (Match item in member.Matches(temp))
                {
                    members[item.Groups["muin"].Value].card = Converts.ConvertUnicodeStringToChinese(item.Groups["card"].Value);
                }
                Regex status = new Regex("(\"stats\":\\[)(?<stats>.+?)(\\],)");
                temp = status.Match(value).Groups["stats"].Value;
                Regex memberstatus = new Regex("({\"uin\":)(?<uin>.+?)(,\"stat\":)(?<stat>.+?)(})");
                foreach (Match item in memberstatus.Matches(temp))
                {
                    members[item.Groups["uin"].Value].stat = item.Groups["stat"].Value;
                }
                isset = true;
            }
        }
    }

    public class Friendlist
    {
        public int groupnum = -1;
        public Dictionary<string, Group> Groups = new Dictionary<string, Group>();
        public Dictionary<string, Friend> Friends = new Dictionary<string, Friend>();
        public Dictionary<string, OnLineFriend> OnLineFriends = new Dictionary<string, OnLineFriend>();
        public Dictionary<string, QQGroup> QQGroups = new Dictionary<string, QQGroup>();
        public Friend myself = new Friend();

        public void setmyinfo(string uin, string value)
        {
            Regex nick = new Regex("(nick\":)(?<nick>.+?)(\",\")");
            Regex face = new Regex("(face\":)(?<face>.+?)(,\")");
            myself.uin = uin;
            myself.name = Converts.ConvertUnicodeStringToChinese(nick.Match(value).Groups["nick"].Value.Substring(1));
            myself.face = Convert.ToInt32(face.Match(value).Groups["face"].Value);
        }
    }

    class GetFriendlists
    {
        public static Friendlist GetFriendsList(string result) 
        {
            Friendlist TheFriendlist = new Friendlist();
            Regex group = new Regex("({\"index\":)(?<index>[0-9]{1,2}?)(,\"name\":\")(?<name>.+?)(\"})");
            Group g0 = new Group();
            g0.name = "在线";
            g0.groupindex = 0;
            TheFriendlist.Groups.Add("100", g0);
            Group g1 = new Group();
            g1.name = "我的好友";
            g1.groupindex = 1;
            TheFriendlist.Groups.Add("0", g1);
            int i = 2;
            foreach (Match item in group.Matches(result)) 
            {
                Group g = new Group();
                g.name = Converts.ConvertUnicodeStringToChinese(item.Groups["name"].Value);
                g.groupindex = i;
                TheFriendlist.Groups.Add(item.Groups["index"].Value, g);
                i++;
            }
            TheFriendlist.groupnum = i;
            Regex friend = new Regex("({\"uin\":)(?<uin>[0-9]{5,11}?)(,\"categories\":)(?<id>[0-9]{1,2}?)(})"); 
            foreach (Match item in friend.Matches(result)) 
            {
                Friend f = new Friend(); 
                f.uin = item.Groups["uin"].Value; 
                f.groupid = item.Groups["id"].Value;
                f.groupindex = TheFriendlist.Groups[f.groupid].groupindex;
                TheFriendlist.Friends.Add(f.uin, f);
            } 
            Regex info = new Regex("({\"uin\":)(?<uin>[0-9]{5,11}?)(,\"nick\":)(?<nick>.+?)(\",\"face\":)(?<face>.+?)(,\"flag\":)(?<flag>.+?)(})"); 
            foreach (Match item in info.Matches(result)) 
            {                
                    TheFriendlist.Friends[item.Groups["uin"].Value].name = Converts.ConvertUnicodeStringToChinese(item.Groups["nick"].Value.Substring(1));
                    TheFriendlist.Friends[item.Groups["uin"].Value].face = Convert.ToInt32(item.Groups["face"].Value);                
            }
            return TheFriendlist; 
        }
        public static Dictionary<string, OnLineFriend> GetOnlineFriend(string result)
        {
            Dictionary<string, OnLineFriend> OnLineFriends = new Dictionary<string, OnLineFriend>();
            Regex onlines = new Regex("({\"uin\":)(?<uin>[0-9]{5,11}?)(,\"status\":\")(?<status>.+?)(\",\"client_type\":)(?<client>.+?)(})");
            foreach (Match item in onlines.Matches(result))
            {
                if (!OnLineFriends.ContainsKey(item.Groups["uin"].Value))
                {
                    OnLineFriend f = new OnLineFriend();
                    f.uin = item.Groups["uin"].Value;
                    f.state = item.Groups["status"].Value;
                    f.client_type = Convert.ToInt32(item.Groups["client"].Value);
                    OnLineFriends.Add(f.uin, f);
                }
            }
            return OnLineFriends;
        }
        public static Dictionary<string, QQGroup> GetGroupList(string result)
        {
            Dictionary<string, QQGroup> QQGroups = new Dictionary<string, QQGroup>();
            Regex qgourp = new Regex("(\"gid\":)(?<gid>[0-9]{5,11}?)(,\"code\":)(?<code>[0-9]{5,11}?)(,\"flag\":)(?<flag>[0-9]{1,11}?)(,\"name\":\")(?<name>.+?)(\"})");
            foreach (Match item in qgourp.Matches(result))
            {
                QQGroup qg = new QQGroup();
                qg.gid = item.Groups["gid"].Value;
                qg.code = item.Groups["code"].Value;
                qg.flag = item.Groups["flag"].Value;
                qg.name = Converts.ConvertUnicodeStringToChinese(item.Groups["name"].Value);
                QQGroups.Add(qg.code, qg);
            }
            return QQGroups;
        }
    }
}
