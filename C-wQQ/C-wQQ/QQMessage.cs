using System;
using System.Collections.Generic;
using System.Text;

namespace C_wQQ
{
    public class aMessage
    {
        public long t = 0;
        public string fromuin = string.Empty;
        public string content = string.Empty;
        public int Msgid = 0;

        public aMessage(string fromuin, string content, long t, int Msgid)
        {
            this.fromuin = fromuin;
            this.content = content;
            this.t = t;
            this.Msgid = Msgid;
        }
    }

    public class QQMessages
    {
        private Dictionary<string,List<aMessage>> TheMessages = new Dictionary<string,List<aMessage>>();
        private Dictionary<string, List<aMessage>> TheGroupMessages = new Dictionary<string, List<aMessage>>();

        public bool IsRepeat(string fromuin, int Msgid)
        {
            if (Msgid == 0)
            {
                return false;
            }
            if (!TheMessages.ContainsKey(fromuin))
            {
                return false;
            }
            else
            {
                for (int i = TheMessages[fromuin].Count - 1; i >= Math.Max(TheMessages[fromuin].Count - 5, 0); i--)
                {
                    if (TheMessages[fromuin][i].Msgid == Msgid)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool GroupIsRepeat(string fromuin, int Msgid)
        {
            if (Msgid == 0)
            {
                return false;
            }
            if (!TheGroupMessages.ContainsKey(fromuin))
            {
                return false;
            }
            else
            {
                for (int i = TheGroupMessages[fromuin].Count - 1; i >= Math.Max(TheGroupMessages[fromuin].Count - 10, 0); i--)
                {
                    if (TheGroupMessages[fromuin][i].Msgid == Msgid)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void SaveMessage(string fromuin, string content, long t,int Msgid)
        {
            if (!TheMessages.ContainsKey(fromuin))
            {
                List<aMessage> amessage = new List<aMessage>();
                TheMessages.Add(fromuin, amessage);
                TheMessages[fromuin].Add(new aMessage(fromuin,content,t,Msgid));
                return;
            }           
            TheMessages[fromuin].Add(new aMessage(fromuin, content, t, Msgid));
            
        }
        public void SaveMessage(string touin, string content, long t)
        {
            SaveMessage(touin, content, t, 0);
        }
        public void SaveGroupMessage(string fromuin, string content, long t, int Msgid)
        {
            if (!TheGroupMessages.ContainsKey(fromuin))
            {
                List<aMessage> amessage = new List<aMessage>();
                TheGroupMessages.Add(fromuin, amessage);
                TheGroupMessages[fromuin].Add(new aMessage(fromuin, content, t, Msgid));
                return;
            }
            TheGroupMessages[fromuin].Add(new aMessage(fromuin, content, t, Msgid));
        }
        public void SaveGroupMessage(string touin, string content, long t)
        {
            SaveGroupMessage(touin, content, t, 0);
        }
        public string GetMessages(string uin)
        {
            string result = string.Empty;
            if (TheMessages.ContainsKey(uin))
            {
                for (int i = Math.Max(TheMessages[uin].Count-10, 0); i < TheMessages[uin].Count; i++)
                {
                        result += TheMessages[uin][i].content;
                }
            }
            return result;
        }
        public string GetGroupMessages(string uin)
        {
            string result = string.Empty;
            if (TheGroupMessages.ContainsKey(uin))
            {
                for (int i = Math.Max(TheGroupMessages[uin].Count - 20, 0); i < TheGroupMessages[uin].Count; i++)
                {
                    result += TheGroupMessages[uin][i].content;
                }
            }
            return result;
        }
    }
}
