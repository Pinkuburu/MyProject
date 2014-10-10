using System;
using System.Text;
using System.Threading;
using System.Collections;
using System.Text.RegularExpressions;

namespace DemoHour_Favorite
{
    //共享资源类  
    class SharedResource
    {
        public string[] arrUserInfo = { "zhangqi7988@126.com,861016qiqi", "379006984@qq.com,zjj1983", "bise_kan@sina.com,800718", "haililover@163.com,780119", "yangshunjing@hotmail.com,19860617", "linlaker76@163.com,linlaker99", "gwcgwy_2@hotmail.com,abc4530", "stevessc@hotmail.com,810224", "5215550@qq.com,790415", "hei_gq8766@hotmail.com,qwer101652", "ueqt@21cn.com,800617", "airtwo@sina.com,231105", "iwantfly530@yahoo.com.cn,anmy1127", "yxshan1976@163.com,33355521", "llrrqq781116@163.com,781116", "cjjom@163.com,ningning", "wangkai43801647@163.com,shmily520", "many170@tom.com,2145095" };
    }

    class Program
    {
        private static event EventHandler OnArryListClear;//数据删除完成引发的事件
        private static ArrayList alUserList;
        private static int AmountThread = 5;//最大线程数
        private static Thread[] ths = new Thread[AmountThread];

        private static string strToken = "";///唯一Token
        private static string strBuyLink = "";
        private static string strSubscribers = "";
        private static string strLinkID = "314397";


        static void Main(string[] args)
        {
            int intSubscribers = 0;
            bool blLoop = true;
            bool blIsBuy = false;

            //=============== 以下开始信息初始化 ===============
            SharedResource obj = new SharedResource();
            alUserList = new ArrayList();
            string[] arrUserInfo = (obj as SharedResource).arrUserInfo;
            foreach (string strInfo in arrUserInfo)
            {
                alUserList.Add(strInfo);
            }
            //=============== 以上结束信息初始化 ===============

            int j = 0;
            while (blLoop)
            {
                blIsBuy = IsOpen(strLinkID);//分析页面，判定项目是否开始重酬。

                if (blIsBuy)
                {
                    blLoop = false;
                }
                else
                {
                    intSubscribers = Convert.ToInt32(strSubscribers);//关注字符串转化成整型
                    if (intSubscribers > 200)
                    {
                        blLoop = false;//发现关注数已经复合设置条件，重置循环状态，使循环停止。
                        //=============== 以下开始批量关注 ===============
                        for (int i = 0; i < AmountThread; i++)
                        {
                            ths[i] = new Thread(Favorite);
                            ths[i].Name = "线程" + i;
                            ths[i].Start(obj);
                            Console.WriteLine(ths[i].Name + " 启动");
                        }
                        //OnArryListClear += new EventHandler(Program_OnArryListClear);
                        //=============== 以上结束批量关注 ===============
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                    j++;
                    if (j > 100)
                    {
                        Console.Clear();
                        j = 0;
                    }
                }
            }

            Console.ReadKey();
        }

        #region 开始关注(多线程处理) Favorite(Object obj)
        /// <summary>
        /// 开始关注(多线程处理)
        /// </summary>
        /// <param name="obj"></param>
        private static void Favorite(Object obj)
        {
            while (true)
            {
                Monitor.Enter(obj);
                WebClient HTTPproc = new WebClient();

                if (alUserList.Count > 0)
                {
                    string strUser = (string)alUserList[0];
                    string[] arrUser = strUser.Split(',');
                    Console.WriteLine(Thread.CurrentThread.Name + " 读取了：" + arrUser[0] + "|" + arrUser[1] + "|" + DateTime.Now);
                    bool blIsLogin = false;
                    blIsLogin = Login(arrUser[0].ToString(), arrUser[1].ToString(), HTTPproc);
                    if (blIsLogin)
                    {
                        string strRequest = "http://www.demohour.com/projects/" + strLinkID + "/like";
                        string strContent = HTTPproc.OpenRead(strRequest);
                        Console.WriteLine(strContent);
                        //SendMessage("cupid0616", arrUser[0].ToString() + " 关注成功 " + DateTime.Now);
                        //Thread.Sleep(rnd.Next(900000, 1800000));
                    }
                    Console.WriteLine(Thread.CurrentThread.Name + " 删除了：" + strUser);
                    alUserList.RemoveAt(0);//删除ArrayList中的元素
                    Console.WriteLine(alUserList.Count);
                }
                else
                {
                    Console.WriteLine(Thread.CurrentThread.Name + " 没有抢到东西");
                    Console.WriteLine(Thread.CurrentThread.Name + " 已经停止");
                    Thread.CurrentThread.Abort();
                }
                Thread.Sleep(5);
                Monitor.Exit(obj);

            }
        }
        #endregion

        #region AnalyzePage IsOpen(string strLinkID)
        /// <summary>
        /// AnalyzePage
        /// </summary>
        /// <param name="strLinkID">LinkID</param>
        /// <returns>true:false</returns>
        private static bool IsOpen(string strLinkID)
        {
            WebClient HTTP_IsOpen = new WebClient();
            HTTP_IsOpen.Encoding = Encoding.UTF8;

            string strPosts = "";//话题
            //string strSubscribers = "";//关注
            string strGZ = "";//浏览人数
            string strStatus = "";//当前状态

            string strRequest = "http://www.demohour.com/projects/" + strLinkID;
            string strContent = HTTP_IsOpen.OpenRead(strRequest);

            try
            {
                strSubscribers = Regex.Match(strContent, "(?<=查看所有支持者\"><i>).*(?=</i>)").Value;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            if (strBuyLink == "")
            {
                strStatus = "预热中";
                Console.WriteLine("关注：{0} {1}", strSubscribers, DateTime.Now.ToString());
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Login Login(string strUserName, string strPassword, WebClient HTTPproc)
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="strUserName">UserName</param>
        /// <param name="strPassword">Password</param>
        /// <param name="HTTPproc">HTTPproc</param>
        /// <returns>true:false</returns>
        private static bool Login(string strUserName, string strPassword, WebClient HTTPproc)
        {
            string strRequest = "http://www.demohour.com/login";
            string strContent = "";
            string strParameter = "";
            Console.WriteLine("Login..." + strUserName);
            strContent = HTTPproc.OpenRead(strRequest);

            try
            {
                strToken = Regex.Match(strContent, "(?<=content=\".*).*(?=\" name=\"csrf-token)").Value;//抓取登录Token
            }
            catch (ArgumentException ex)
            {
                strToken = "";
            }

            strRequest = "http://www.demohour.com/session";
            strParameter = "utf8=%E2%9C%93&authenticity_token=" + UrlEncode(strToken, "UTF-8") + "&email=" + UrlEncode(strUserName, "UTF-8") + "&password=" + UrlEncode(strPassword, "UTF-8") + "&auto_login=0&auto_login=1";
            strContent = HTTPproc.OpenRead(strRequest, strParameter);
            if (HTTPproc.StatusCode == 302)
            {
                Console.WriteLine("Login Success.");
                return true;
            }
            Console.WriteLine("Login Fail.");
            return false;
        }
        #endregion

        #region URLEncode UrlEncode(string str, string encode)
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private static string UrlEncode(string str, string encode)
        {
            int factor = 0;
            if (encode == "UTF-8")
                factor = 3;
            if (encode == "GB2312")
                factor = 2;

            //不需要编码的字符
            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-";
            System.Text.Encoder encoder = System.Text.Encoding.GetEncoding(encode).GetEncoder();
            char[] c1 = str.ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //一个字符一个字符的编码

            for (int i = 0; i < c1.Length; i++)
            {
                //不需要编码

                if (okChar.IndexOf(c1[i]) > -1)
                    sb.Append(c1[i]);
                else
                {
                    byte[] c2 = new byte[factor];
                    int charUsed, byteUsed; bool completed;

                    encoder.Convert(c1, i, 1, c2, 0, factor, true, out charUsed, out byteUsed, out completed);

                    foreach (byte b in c2)
                    {
                        if (b != 0)
                            sb.AppendFormat("%{0:X}", b);
                    }
                }
            }
            return sb.ToString().Trim();
        }
        #endregion

        #region MSG SendMessage(string strContent)
        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <param name="strContent">消息内容</param>
        private static bool SendMessage(string strMessage)
        {
            WebClient HTTPmsg = new WebClient();
            HTTPmsg.Encoding = Encoding.UTF8;
            string strRequest = "http://qpush.me/pusher/push_site/";
            string strResponse = "name=cupid&code=733922&sig=&cache=false&msg%5Btext%5D=" + strMessage;
            string strContent = HTTPmsg.OpenRead(strRequest, strResponse);//{"op":"r","result":"success","message":"naK8ZH"}
            Console.WriteLine(strContent);
            if (strContent == "\"d8cc10e903918f74\"")
            {
                return true;
            }
            else
            {
                Console.WriteLine("消息发送失败");
                return false;
            }
        }
        #endregion
    }
}
