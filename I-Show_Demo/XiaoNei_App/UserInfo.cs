using Newtonsoft.Json.Linq;

namespace XiaoNei_App
{
    public class UserInfo
    {
        private int _uid = 0;//用户UID

        public string Uid
        {            
            get { return this._uid.ToString(); }
            set {
                    JObject o = JObject.Parse(value);                    
                    this._uid = (int)o["uid"]; 
                }
        } 
    }
}
