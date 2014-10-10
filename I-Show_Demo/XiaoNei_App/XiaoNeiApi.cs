using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using Newtonsoft.Json.Linq;

namespace XiaoNei_App
{
    public class XiaoNeiApi
    {
        //DemoShow@126.com
        //API Key 
        //e987f59f192b44cd81c4b6dd9d37b100 
        //secret 
        //b29827759753443db41d94b56f911376 

        //cupid0426@163.com
        //string Api_Key = "2c3dae2f4a494b7898b8dd361783f8e2";
        //string Secret_Key = "ce7b5e0162254c6abc078b7a44c4ff5f";

        string Api_Key = "e987f59f192b44cd81c4b6dd9d37b100";
        string Secret_Key = "b29827759753443db41d94b56f911376";
        string RESTServer_URI = "http://api.renren.com/restserver.do";
        string strFormat = "JSON";
        public string strSessionKey = null;

        WebClient HTTPproc = new WebClient();
        UserInfo uInfo = new UserInfo();

        public string CheckFirstLogin(string strUid)
        {
            string strContent = null;
            string strSQL = null;

            strSQL = "SELECT Uid FROM [MT_User] WHERE Uid = " + strUid;
            DataRow dr = SqlHelper.ExecuteDataRow(SqlLibrary.GetXn_Main(), CommandType.Text, strSQL);
            if (dr == null)
            {
                strContent = "1"; //是第一次登录
            }
            else
            {
                strContent = "0"; //不是第一次登录
            }
            return strContent;
        }

        #region 读取用户UID users_getLoggedInUser()
        /// <summary>
        /// 读取用户UID
        /// </summary>
        /// <param name="strSessionKey"></param>
        /// <returns>JSON</returns>
        public string users_getLoggedInUser()
        {
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            string strCallID = null;
            string strSig_Key = null;
            string strContent = null;
            string strParameters = null;

            strCallID = Timestamp().ToString();
            strSig_Key = string.Format("api_key={0}call_id={1}format={2}method={3}session_key={4}v=1.0{5}", this.Api_Key, strCallID, this.strFormat, "xiaonei.users.getLoggedInUser", this.strSessionKey, this.Secret_Key);
            strSig_Key = UserMd5(strSig_Key);
            strParameters = string.Format("api_key={0}&call_id={1}&format={2}&method={3}&session_key={4}&v=1.0&sig={5}", this.Api_Key, strCallID, this.strFormat, "xiaonei.users.getLoggedInUser", this.strSessionKey, strSig_Key);
            strContent = CleanBadWords(HTTPproc.OpenRead(this.RESTServer_URI, strParameters));            
            uInfo.Uid = strContent;

            return strContent;
        }
        #endregion 读取用户UID users_getLoggedInUser(string strSessionKey)

        #region 读取用户基本信息 users_getInfo(string strUid, string strPassword)
        /// <summary>
        /// 读取用户基本信息
        /// </summary>
        /// <param name="strUid"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public string users_getInfo(string strUid, string strPassword)
        {
            HTTPproc.Encoding = System.Text.Encoding.UTF8;

            string strCallID = null;
            string strSig_Key = null;
            string strContent = null;
            string strParameters = null;
            string strSQL = null;
            DateTime dt = DateTime.Now;

            int intStatus = 0;
            int intUid = 0;
            int intSex = 0;
            string strName = null;                        
            string strProvince = null;
            string strCity = null;
            string strTinyurl = null;
            string strHeadurl = null;
            DateTime dtBirthday = DateTime.Parse("1900-01-01");
            DateTime dtLastActiveTime = dt;


            strSQL = "SELECT Uid FROM [MT_User] WHERE Uid = " + strUid;
            DataRow dr = SqlHelper.ExecuteDataRow(SqlLibrary.GetXn_Main(), CommandType.Text, strSQL);

            if (dr == null)
            {
                strCallID = Timestamp().ToString();//fields=uid,name,sex,birthday,tinyurl,headurl,hometown_location
                strSig_Key = string.Format("api_key={0}call_id={1}fields=uid,name,sex,birthday,tinyurl,headurl,hometown_locationformat={2}method={3}session_key={4}uids={5}v=1.0{6}", this.Api_Key, strCallID, this.strFormat, "users.getInfo", this.strSessionKey, strUid, this.Secret_Key);
                strSig_Key = UserMd5(strSig_Key);
                strParameters = string.Format("api_key={0}&call_id={1}&fields=uid,name,sex,birthday,tinyurl,headurl,hometown_location&format={2}&method={3}&session_key={4}&uids={5}&v=1.0&sig={6}", this.Api_Key, strCallID, this.strFormat, "users.getInfo", this.strSessionKey, strUid, strSig_Key);
                try
                {
                    strContent = CleanBadWords(HTTPproc.OpenRead(this.RESTServer_URI, strParameters));
                    JObject o = JObject.Parse(strContent);
                    intUid = (int)o["uid"];
                    try
                    {
                        strTinyurl = (string)o["tinyurl"];
                    }
                    catch
                    {
                        strTinyurl = "";
                    }
                    try
                    {
                        dtBirthday = DateTime.Parse((string)o["birthday"]);
                    }
                    catch
                    {
                        dtBirthday = DateTime.Parse("1900-01-01");
                    }
                    try
                    {
                        strProvince = (string)o["hometown_location"]["province"];
                    }
                    catch
                    {
                        strProvince = "";
                    }
                    try
                    {
                        strCity = (string)o["hometown_location"]["city"];
                    }
                    catch
                    {
                        strCity = "";
                    }
                    try
                    {
                        intSex = (int)o["sex"];
                    }
                    catch
                    {
                        intSex = 0;
                    }
                    strName = (string)o["name"];
                    strHeadurl = (string)o["headurl"];

                    intStatus = SqlLibrary.Xn_AddNewUser(intUid, strPassword, strName, intSex, dtBirthday, strProvince, strCity, strTinyurl, strHeadurl, dtLastActiveTime);

                    if (intStatus == 1)
                    {
                        strContent = "1"; //添加用户成功
                    }
                    else
                    {
                        strContent = "0"; //已有此用户
                    }
                }
                catch(Exception ex)
                {
                    strContent = ex.ToString();
                    strContent = "-1";
                }
            }
            return strContent;
        }

        #endregion 读取用户基本信息 users_getInfo(string strUid, string strPassword)
        
        #region 读取用户基本信息 users_getInfo(string strUid)
        /// <summary>
        ///读取用户基本信息 
        /// </summary>
        /// <param name="strUid"></param>
        /// <returns>uid,name,tinyurl</returns>
        public string users_getInfo(string strUid)
        {
            HTTPproc.Encoding = System.Text.Encoding.UTF8;

            string strCallID = null;
            string strSig_Key = null;
            string strContent = null;
            string strParameters = null;

            strCallID = Timestamp().ToString();//fields=uid,name,sex,birthday,tinyurl,headurl,hometown_location
            strSig_Key = string.Format("api_key={0}call_id={1}fields=uid,name,tinyurlformat={2}method={3}session_key={4}uids={5}v=1.0{6}", this.Api_Key, strCallID, this.strFormat, "users.getInfo", this.strSessionKey, strUid, this.Secret_Key);
            strSig_Key = UserMd5(strSig_Key);
            strParameters = string.Format("api_key={0}&call_id={1}&fields=uid,name,tinyurl&format={2}&method={3}&session_key={4}&uids={5}&v=1.0&sig={6}", this.Api_Key, strCallID, this.strFormat, "users.getInfo", this.strSessionKey, strUid, strSig_Key);
            strContent = CleanBadWords(HTTPproc.OpenRead(this.RESTServer_URI, strParameters));
            return strContent;
        }
        #endregion 读取用户基本信息 users_getInfo(string strUid)

        #region 读取用户好友信息 friends_getFriends()
        /// <summary>
        /// 读取用户好友信息
        /// </summary>
        /// <returns>JSON</returns>
        public string friends_getFriends()
        {
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            string strCallID = null;
            string strSig_Key = null;
            string strContent = null;
            string strParameters = null;
            string[] arrFriend = null; 

            strCallID = Timestamp().ToString();
            strSig_Key = string.Format("api_key={0}call_id={1}count=500format={2}method={3}session_key={4}v=1.0{5}", this.Api_Key, strCallID, this.strFormat, "friends.getFriends", this.strSessionKey, this.Secret_Key);
            strSig_Key = UserMd5(strSig_Key);
            strParameters = string.Format("api_key={0}&call_id={1}&count=500&format={2}&method={3}&session_key={4}&v=1.0&sig={5}", this.Api_Key, strCallID, this.strFormat, "friends.getFriends", this.strSessionKey, strSig_Key);
            strContent = CleanBadWords(HTTPproc.OpenRead(this.RESTServer_URI, strParameters));
            arrFriend = strContent.Split(',');
            strContent = "共有好友 " + arrFriend.Length.ToString();
            return strContent;
        }
        #endregion 读取用户好友信息 friends_getFriends()

        #region 读取安装应用的好友
        /// <summary>
        /// 读取安装应用的好友
        /// </summary>
        /// <returns></returns>
        public string friends_getAppFriends()
        {
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            string strCallID = null;
            string strSig_Key = null;
            string strContent = null;
            string strParameters = null;

            strCallID = Timestamp().ToString();
            strSig_Key = string.Format("api_key={0}call_id={1}fields=name,tinyurlformat={2}method={3}session_key={4}v=1.0{5}", this.Api_Key, strCallID, this.strFormat, "friends.getAppFriends", this.strSessionKey, this.Secret_Key);
            strSig_Key = UserMd5(strSig_Key);
            strParameters = string.Format("api_key={0}&call_id={1}&fields=name,tinyurl&format={2}&method={3}&session_key={4}&v=1.0&sig={5}", this.Api_Key, strCallID, this.strFormat, "friends.getAppFriends", this.strSessionKey, strSig_Key);
            strContent = CleanBadWords(HTTPproc.OpenRead(this.RESTServer_URI, strParameters)).Replace("},{","}|{");
            //arrFriend = strContent.Split('|');
            return strContent;
        }
        #endregion 读取安装应用的好友

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

        #region MD5加密算法 UserMd5(string str)
        /// <summary>
        /// MD5加密算法
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <returns>输出32位小写MD5加密值</returns>
        private string UserMd5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像  
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　  
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得  
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                pwd = pwd + s[i].ToString("x2");
            }
            return pwd;
        }
        #endregion

        #region 清楚JSON中脏数据 CleanBadWords(string strContent)
        /// <summary>
        /// 清楚JSON中脏数据
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        private string CleanBadWords(string strContent)
        {
            string resultString = null;
            try
            {
                resultString = Regex.Match(strContent, @"\{.*\}").Value;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            return resultString;
        }
        #endregion 清楚JSON中脏数据 CleanBadWords(string strContent)
    }
}
