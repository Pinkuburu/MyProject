using System;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Data;

namespace LittleWar
{
    public partial class LittleWar_Main : Form
    {
        private string strSig = null;
        private string strKey = null;
        private string strKeyName = null;
        private long longTime = 0;
        private string userRun = null;
        private string str_inuId = null;
        private int intUid = 0;
        private long t = 0;
        private int tu = 0;//测试用计数器
        private bool productStatus = true;
        private bool soldierStatus = true;
        private bool boolAuth = false;

        private User u = new User();
        private ArrayList productTask = new ArrayList();
        private ArrayList soldierTask = new ArrayList();

        private ArrayList buildListIds = new ArrayList();

        private WebClient HTTPproc = new WebClient();

        private System.Threading.Timer tm;
        

        public LittleWar_Main()
        {
            InitializeComponent();
            this.AcceptButton = button_Login;
            Control.CheckForIllegalCrossThreadCalls = false;
            //this.timer1.Enabled = false;
            //this.timer1.Interval = 1000;
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(Start));
            thread.IsBackground = true;
            thread.Start();
            //Thread nonParameterThread = new Thread();
            //Start();
            
        }

        #region 进程启动 Start()
        /// <summary>
        /// 进程启动
        /// </summary>
        private void Start()
        {
            this.productStatus = true;
            this.soldierStatus = true;
            this.productTask.Clear();
            this.soldierTask.Clear();

            if (textBox_UserName.Text == "" || textBox_Password.Text == "")
            {
                ShowSysLog("用户名或密码不能为空！");
            }
            else
            {
                Login_Xiaonei(textBox_UserName.Text.ToString().Trim(), textBox_Password.Text.ToString().Trim());
                CheckAuthUser(textBox_UserName.Text.ToString().Trim());
            }
            
            Login_Game();
            gainAndProduceFood(userRun, 1);
            gainAndProduceForce(userRun, 1);
            attackBeast(userRun, intUid.ToString());
            getFriendList();
            //timer1.Enabled = true;
            this.tm = new System.Threading.Timer(new TimerCallback(starttimer), null, 0, 0);
        }
        #endregion 进程启动 Start()


        #region 登录校内 Login_Xiaonei(string strUserName, string strPassword)
        /// <summary>
        /// 登录校内
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        private string Login_Xiaonei(string strUserName, string strPassword)
        {
            string strContent = null;

            ShowSysLog("正在登录校内...");

            if (strUserName.Trim() != "" && strPassword.Trim() != "")
            {
                //设置HTTP请求默认编码
                HTTPproc.Encoding = System.Text.Encoding.UTF8;
                string strRedirectURL = HTTPproc.OpenRead("http://www.renren.com/PLogin.do", "email=" + UrlEncode(strUserName.Trim(), "UTF-8") + "&password=" + UrlEncode(strPassword.Trim(), "UTF-8") + "&origURL=http%3A%2F%2Fwww.renren.com%2FSysHome.do&domain=renren.com");
                try
                {
                    strRedirectURL = strRedirectURL.Replace("The URL has moved <a href=\"", "").Replace("\">here</a>", "");
                    HTTPproc.OpenRead(strRedirectURL);
                    HTTPproc.OpenRead("http://www.renren.com/SysHome.do");

                    strContent = HTTPproc.OpenRead("http://www.renren.com/Home.do");
                    if (strContent.IndexOf("guide.do") > 0)
                    {
                        HTTPproc.OpenRead("http://guide.renren.com/guide.do");
                        strContent = HTTPproc.OpenRead("http://guide.renren.com/guidexf.do");
                    }
                }
                catch
                {
                    ShowSysLog("用户名或密码出错");
                }
            }
            else
            {
                ShowSysLog("信息填写有误请检查！");
            }

            return strContent;
        }
        #endregion 登录校内 Login_Xiaonei(string strUserName, string strPassword)

        #region 登录小小战争 Login_Game()
        /// <summary>
        /// 登录小小战争
        /// </summary>
        /// <returns>返回Sig</returns>
        private void Login_Game()
        {
            string xn_sig_in_iframe = null;    //xn_sig_in_iframe = 1
            string xn_sig_method = null;       //xn_sig_method = get
            string xn_sig_time = null;         //xn_sig_time = 1287705904481
            string xn_sig_user = null;         //xn_sig_user = 228618602
            string xn_sig_expires = null;      //xn_sig_expires = 1287712800
            string xn_sig_session_key = null;  //xn_sig_session_key = 2.a43a4e84584388ce22dd0193c775e8a4.3600.1287712800-228618602
            string xn_sig_added = null;        //xn_sig_added = 1
            string xn_sig_api_key = null;      //xn_sig_api_key = 98a2ebb4516c4a5f9104255821038173
            string xn_sig_app_id = null;       //xn_sig_app_id = 108245
            string xn_sig_domain = null;       //xn_sig_domain = renren.com
            string xn_sig_user_src = null;     //xn_sig_user_src = rr
            string xn_sig = null;              //xn_sig = 83f8de7c31bdaf0dc2e9abfefc6205a7

            string str_uid = null;
            string str_session_key = null;
            string str_secret = null;
            string str_expires = null;
            string str_base_domain = null;
            string str_sig = null;
            string resultString = null;
            string strRequest = null;
            string strContent = null;

            string[] arrKeyWords;

            ShowSysLog("正在登录游戏...");

            strRequest = "http://apps.renren.com/littlewar";
            strContent = HTTPproc.OpenRead(strRequest);
            //ShowSysLog("1");
            try
            {
                resultString = Regex.Match(strContent, "http://xn.lw.fminutes.com.*\" f").Value.Replace("amp;", "").Replace("\" f", "").Replace("?", "/?");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            //准备再次请求时使用
            strRequest = resultString;
            //去除主域名部分
            arrKeyWords = resultString.Replace("http://xn.lw.fminutes.com/?", "").Split('&');

            xn_sig_in_iframe = arrKeyWords[0].ToString().Replace("xn_sig_in_iframe=", "");
            xn_sig_method = arrKeyWords[1].ToString().Replace("xn_sig_method=", "");
            xn_sig_time = arrKeyWords[2].ToString().Replace("xn_sig_time=", "");
            xn_sig_user = arrKeyWords[3].ToString().Replace("xn_sig_user=", "");
            xn_sig_expires = arrKeyWords[4].ToString().Replace("xn_sig_expires=", "");
            xn_sig_session_key = arrKeyWords[5].ToString().Replace("xn_sig_session_key=", "");
            xn_sig_added = arrKeyWords[6].ToString().Replace("xn_sig_added=", "");
            xn_sig_api_key = arrKeyWords[7].ToString().Replace("xn_sig_api_key=", "");
            xn_sig_app_id = arrKeyWords[8].ToString().Replace("xn_sig_app_id=", "");
            xn_sig_domain = arrKeyWords[9].ToString().Replace("xn_sig_domain=", "");
            xn_sig_user_src = arrKeyWords[10].ToString().Replace("xn_sig_user_src=", "");
            xn_sig = arrKeyWords[11].ToString().Replace("xn_sig=", "");

            strContent = HTTPproc.OpenRead(strRequest);
            try
            {
                str_inuId = Regex.Match(strContent, "inuId.*api_key").Value.Replace("inuId=", "").Replace("&amp;api_key", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            //ShowSysLog("2");
            strContent = HTTPproc.OpenRead("http://www.connect.renren.com/login_status.do?api_key=" + xn_sig_api_key + "%20&extern=false&channel=http%3A%2F%2Fxn.lw.fminutes.com%2Fxd_receiver.html");
            try
            {
                resultString = Regex.Match(strContent, @"\{""uid"".*\}").Value;

                JObject o = JObject.Parse(Unicode2Character(resultString));
                str_uid = Convert.ToString((int)o["uid"]);
                str_session_key = (string)o["session_key"];
                str_secret = (string)o["secret"];
                str_expires = Convert.ToString((int)o["expires"]);
                str_base_domain = (string)o["base_domain"];
                str_sig = (string)o["sig"];
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            HTTPproc = new WebClient();

            HTTPproc.RequestHeaders.Add(xn_sig_api_key + ":" + str_sig);
            HTTPproc.RequestHeaders.Add(xn_sig_api_key + "_expires:" + str_expires);
            HTTPproc.RequestHeaders.Add(xn_sig_api_key + "_session_key:" + str_session_key);
            HTTPproc.RequestHeaders.Add(xn_sig_api_key + "_ss:" + str_secret);
            HTTPproc.RequestHeaders.Add(xn_sig_api_key + "_user:" + str_uid);
            HTTPproc.RequestHeaders.Add("base_domain_" + xn_sig_api_key + ":" + str_base_domain);
            HTTPproc.RequestHeaders.Add("xnsetting_" + xn_sig_api_key + ":{\"connectState\" = 1,\"oneLineStorySetting\" = 1,\"shortStorySetting\" = 1,\"shareAuth\" = null}");
            //ShowSysLog("3");
            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Scene.init";
            strContent = Unicode2Character(HTTPproc.OpenRead(strRequest));

            try
            {
                strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;
                resultString = Regex.Replace(strContent, "\"skillList.*getKey", "\"getKey");

                JObject o = JObject.Parse(Unicode2Character(resultString));

                //获取玩家信息   
                JObject jsonu = (JObject)o["info"]["player_info"];
                u.updateNoTime(jsonu);
                u.setSystemTime((long)o["info"]["time"]);

                strKey = (string)o["info"]["getKey"]["key"];
                strKeyName = (string)o["info"]["getKey"]["keyName"];
                intUid = (int)o["info"]["uid"];
                longTime = (long)o["info"]["time"];
            }
            catch
            {
                ShowSysLog("读取Scene.init出错");
            }

            if (strKeyName.Length == 17)
            {
                strKeyName = strKeyName.Substring(10, strKeyName.Length - 10);
                string[] arrKeyName = { "97ba558178f22af9", "8a57faa3ff0c2cd0", "b05395426617a666", "8054b38ece415448", "5a0815d2500be4c3", "cb47e040c444bb13", "4f0b466d4e838204", "9bb033dd03a03a21", "a548d6aefbeb32e0", "c156d1c03531d5f6" };
                foreach (string ss in arrKeyName)
                {
                    if (ss.IndexOf(strKeyName) > 0)
                    {
                        strKeyName = ss;
                        break;
                    }
                }
            }

            strSig = Sig(strKey, strKeyName);
            //ShowSysLog("5");
            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Scene.run";
            //ShowSysLog(strRequest);
            ShowSysLog("data={\"fid\":" + intUid + "\"}&keyName=" + strKeyName + "&requestSig=" + strSig);
            strContent = HTTPproc.OpenRead(strRequest, "data={\"fid\":" + intUid + "\"}&keyName=" + strKeyName + "&requestSig=" + strSig);
            //ShowSysLog(strContent);
            userRun = Unicode2Character(strContent);
            //ShowSysLog("6");
        }
        #endregion 登录小小战争 Login_Game()

        #region 生产和收获食物 gainAndProduceFood(string userRun, int intFoodID)
        /// <summary>
        /// 生产和收获食物
        /// </summary>
        /// <param name="userRun">Scene.run</param>
        /// <param name="intFoodID">1-4</param>
        private void gainAndProduceFood(string userRun, int intFoodID)
        {
            string resultString = null;
            buildListIds.Clear();
            userRun = Regex.Replace(userRun, @"\d\w{0,}\r\n", "");
            userRun = Regex.Match(userRun.Replace("\r\n", ""), @"\{.*\}").Value;
            userRun = Regex.Replace(userRun, @",""position"":\w[0-9a-zA-Z]{0,}\[\w[0-9a-z]{0,6},\w[0-9a-z]{0,6},\w[0-9a-z]{0,6}\]", "");
            userRun = Regex.Replace(userRun, @",""position"":\[\w[0-9a-z]{0,6},\w[0-9a-z]{0,6},\w[0-9a-z]{0,6}\]", "");
            userRun = Regex.Replace(userRun, @":[a-z0-9]{1,}\{", ":{");
            userRun = Regex.Replace(userRun, @"e\d{0,},", ",");
            userRun = Regex.Replace(userRun, @",\w[0-9a-z]{0,}""", ",\"");
            userRun = Regex.Replace(userRun, @":\d[0-9]+\w[a-z]08", ":30008");
            userRun = Regex.Replace(userRun, @"""called"":\w{0,},", "");
            userRun = Regex.Replace(userRun, @"""id""\w+\d", "\"id\"");
            userRun = Regex.Replace(userRun, @"4000\w+\d", "40003");
            userRun = Regex.Replace(userRun, @",""build_level"":\w+\d", "");
            userRun = Regex.Replace(userRun, @":2000+\w+\d", ":20008");
            resultString = Regex.Match(userRun, @"\w{13},").Value;
            resultString = Regex.Replace(resultString, @"([a-z]\d[a-z])|([a-z][a-z]\d)|([a-z][a-z][a-z])|([a-z]\d\d)", "");
            userRun = Regex.Replace(userRun, @"\w{13},", resultString);

            JObject o = JObject.Parse(userRun);
            JObject build_list = (JObject)o["info"]["enter_scene"]["build_info"]["build_list"];
            ArrayList foodBuild = new ArrayList();
            JArray foodBuildIds = new JArray();

            //生产食物
            foreach (object buildIds in build_list)
            {
                try
                {
                    resultString = Regex.Replace(buildIds.ToString(), @"\[\d{1,3}, ", "");
                    resultString = Regex.Replace(resultString, @"\}\]", "}");

                    JObject objBuild = JObject.Parse(resultString);
                    int intBuild_Id = (int)objBuild.First.Next;//["build_id"];
                    if (intBuild_Id >= 30000 && intBuild_Id < 40000)
                    {
                        foodBuild.Add((int)objBuild.First);//["id"]

                        long st = (long)objBuild.First.Next.Next.Next;//["start_time"];
                        long et = (long)objBuild.First.Next.Next.Next.Next;//["end_time"];

                        if (st == 0)
                        {
                            string data = "{\"produce_id\":" + intFoodID + ",\"id\":" + (int)objBuild.First + "}";//["id"]
                            produceFood(data);
                        }
                    }
                }
                catch
                {
                    // Syntax error in the regular expression
                }
            }

            //看看食物列表里哪个能收获的 
            for (int j = 0; j < foodBuild.Count; j++)
            {
                foodBuildIds.Add(foodBuild[j]);
            }
            buildListIds.Add(foodBuildIds.ToString().Replace("\r\n", "").Replace("  ", ""));

            //收获并生产食物
            foreach (object buildIds in build_list)
            {
                resultString = Regex.Replace(buildIds.ToString(), @"\[\d{1,3}, ", "");
                resultString = Regex.Replace(resultString, @"\}\]", "}");

                JObject objBuild = JObject.Parse(resultString);
                int intBuild_Id = (int)objBuild.First.Next;//"build_id"
                if (intBuild_Id >= 30000 && intBuild_Id < 40000)
                {
                    long st = (long)objBuild.First.Next.Next.Next;//["start_time"];
                    long et = (long)objBuild.First.Next.Next.Next.Next;//["end_time"];

                    if (et != 0 && et <= Timestamp())
                    {
                        //可以收获了                        
                        string data = "{\"ids\":" + foodBuildIds.ToString().Replace("\r\n", "").Replace("  ", "") + ",\"id\":" + (int)objBuild.First + "}";//["id"]
                        gainFood(data);
                        //收货后再生产
                        data = "{\"produce_id\":" + intFoodID + ",\"id\":" + (int)objBuild.First + "}";//["id"]
                        produceFood(data);
                    }
                    else if (et > Timestamp())
                    {
                        string userRuns = "{\"id\":" + (int)objBuild.First + ",\"end_time\":" + et + ",\"ids\":\"" + foodBuildIds.ToString().Replace("\r\n", "").Replace("  ", "") + "\"}";//["id"]
                        productTask.Add(userRuns);
                        ShowSysLog("已将下次收获时间：" + et + " 加入计划任务列表");
                        if (productTask.Count > 1)
                        {
                            productTask = productSorter(productTask);//排序
                        }
                    }
                }
            }
        }
        #endregion 生产和收获食物 gainAndProduceFood(string userRun, int intFoodID)

        #region 生产食物 produceFood(string data)
        /// <summary>
        /// 生产食物
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool produceFood(string data)
        {
            string strRequest = null;
            string strContent = null;
            this.productStatus = true;

            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Produce.recruiteSoldier";
            ShowSysLog(data);
            strContent = Unicode2Character(HTTPproc.OpenRead(strRequest, "requestSig=" + strSig + "&data=" + data + "&keyName=" + strKeyName));
            strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;

            JObject o = JObject.Parse(Unicode2Character(strContent));
            if ((int)o["result"] == 0 && o["info"] != null && o["info"]["player_info"] != null)
            {
                JArray productTime = (JArray)o["info"]["build_update"];
                long endTime = (long)productTime[0]["end_time"];
                JObject id = JObject.Parse(data);
                int build_id = (int)id["id"];
                string strTaskInfo = "{\"id\":" + build_id + ",\"end_time\":" + endTime + ",\"ids\":\"" + buildListIds[0].ToString() + "\"}";
                productTask.Add(strTaskInfo);
                if (productTask.Count > 1)
                {
                    productTask = productSorter(productTask);//排序
                }

                //生产成功：
                ShowSysLog("生产食物在build：" + data + "成功！");
                //ShowSysLog("下次收获时间是：开始时间 " + (long)productTime[0]["start_time"] + " 收获时间：" + (long)productTime[0]["end_time"] + " 现在时间是：" + Timestamp());                
                ShowSysLog("已将下次收获时间：" + endTime + " 加入计划任务列表");
                return true;
            }
            else
            {
                if ((int)o["result"] == 209)
                {
                    //{"result":102,"msg":"Session Error"}
                    string result = o.ToString();
                    ShowSysLog("生产食物在build：" + data + "失败！原因：" + Unicode2Character(result).Replace("\r\n", ""));
                    return false;
                }
                else
                {
                    this.productStatus = false;
                    string result = o.ToString();
                    ShowSysLog("生产食物在build：" + data + "失败！原因：" + Unicode2Character(result).Replace("\r\n", ""));
                    return false;
                }
            }
        }
        #endregion 生产食物 produceFood(string data)

        #region 收获食物 gainFood(string data)
        /// <summary>
        /// 收获食物
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string gainFood(string data)
        {
            string strRequest = null;
            string strContent = null;


            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Produce.gainFood";
            ShowSysLog(data);
            strContent = Unicode2Character(HTTPproc.OpenRead(strRequest, "requestSig=" + strSig + "&data=" + data + "&keyName=" + strKeyName));
            strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;

            JObject o = JObject.Parse(Unicode2Character(strContent));
            if ((int)o["result"] == 0 && o["info"] != null && o["info"]["player_info"] != null)
            {
                ShowSysLog("收获食物在build：" + data + "成功！");
            }
            else
            {
                ShowSysLog("收获食物在build：" + data + "失败！");
            }
            return strContent;
        }
        #endregion 收获食物 gainFood(string data)

        #region 生产和收获兵力 gainAndProduceForce(string userRun, int intForceID)
        /// <summary>
        /// 生产和收获兵力
        /// </summary>
        /// <param name="userRun"></param>
        /// <param name="intForceID"></param>
        private void gainAndProduceForce(string userRun, int intForceID)
        {
            string resultString = null;

            userRun = Regex.Match(userRun.Replace("\r\n", ""), @"\{.*\}").Value;
            userRun = Regex.Replace(userRun, @",""position"":\w[0-9a-zA-Z]{0,}\[\w[0-9a-z]{0,6},\w[0-9a-z]{0,6},\w[0-9a-z]{0,6}\]", "");
            userRun = Regex.Replace(userRun, @",""position"":\[\w[0-9a-z]{0,6},\w[0-9a-z]{0,6},\w[0-9a-z]{0,6}\]", "");
            userRun = Regex.Replace(userRun, @":[a-z0-9]{1,}\{", ":{");
            userRun = Regex.Replace(userRun, @"e\d{0,},", ",");
            userRun = Regex.Replace(userRun, @",\w[0-9a-z]{0,}""", ",\"");
            userRun = Regex.Replace(userRun, @":\d[0-9]+\w[a-z]08", ":30008");
            userRun = Regex.Replace(userRun, @"""called"":\w{0,},", "");
            userRun = Regex.Replace(userRun, @"""id""\w+\d", "\"id\"");
            userRun = Regex.Replace(userRun, @"4000\w+\d", "40003");
            userRun = Regex.Replace(userRun, @",""build_level"":\w+\d", "");
            userRun = Regex.Replace(userRun, @":2000+\w+\d", ":20008");
            userRun = Regex.Replace(userRun, @"""\d{0,}"":\{""id"":\d{0,},""build_id"":(1|2|5|6|7|8|9)\d{0,},""build_level"":\d{0,}\},", "");
            userRun = Regex.Replace(userRun, @"\}\w{3}", "}");
            resultString = Regex.Match(userRun, @"\w{13},").Value;
            resultString = Regex.Replace(resultString, @"([a-z]\d[a-z])|([a-z][a-z]\d)|([a-z][a-z][a-z])|([a-z]\d\d)", "");
            userRun = Regex.Replace(userRun, @"\w{13},", resultString);

            JObject info = JObject.Parse(userRun);
            JObject build_list = (JObject)info["info"]["enter_scene"]["build_info"]["build_list"];

            foreach (object objBuild_info in build_list)
            {
                resultString = Regex.Replace(objBuild_info.ToString(), @"\[\d{1,3}, ", "");
                resultString = Regex.Replace(resultString, @"\}\]", "}");

                JObject objBuild = JObject.Parse(resultString);
                int intBuild_Id = (int)objBuild.First.Next;//["build_id"];
                if (intBuild_Id >= 30000 && intBuild_Id < 40000)
                {
                    //这个是食物
                }
                else if (intBuild_Id >= 40000 && intBuild_Id < 50000)
                {
                    //兵工厂
                    long st = (long)objBuild.First.Next.Next.Next;//["start_time"];
                    long et = (long)objBuild.First.Next.Next.Next.Next;//["end_time"];

                    if (st == 0)
                    {
                        //空的 。可以直接生产兵
                        string data = "{\"produce_id\":" + intForceID + ",\"id\":" + (int)objBuild.First + "}";//["id"]
                        produceForce(data);
                    }
                    else if (et <= Timestamp())
                    {
                        //先收兵，再生产兵
                        string data = "{\"id\":" + (int)objBuild.First + "}";//["id"]
                        gainForce(data);
                        data = "{\"produce_id\":" + intForceID + ",\"id\":" + (int)objBuild.First + "}";//["id"]
                        produceForce(data);
                    }
                    else if (et > Timestamp())
                    {
                        string userRuns = "{\"produce_id\":" + intForceID + ",\"end_time\":" + et + ",\"id\":" + (int)objBuild.First + "}";//["id"]
                        soldierTask.Add(userRuns);
                        ShowSysLog("已将下次收获时间：" + et + " 加入计划任务列表");
                        if (soldierTask.Count > 1)
                        {
                            soldierTask = soldierSorter(soldierTask);
                        }
                    }
                }
            }
        }
        #endregion 生产和收获兵力 gainAndProduceForce(string userRun, int intForceID)

        #region 生产兵力 produceForce(string data)
        /// <summary>
        /// 生产兵力
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool produceForce(string data)
        {
            string strRequest = null;
            string strContent = null;
            this.soldierStatus = true;


            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Produce.recruiteSoldier";
            ShowSysLog(data);
            strContent = Unicode2Character(HTTPproc.OpenRead(strRequest, "requestSig=" + strSig + "&data=" + data + "&keyName=" + strKeyName));
            strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;

            JObject o = JObject.Parse(Unicode2Character(strContent));
            if ((int)o["result"] == 0 && o["info"] != null && o["info"]["player_info"] != null)
            {
                JArray productTime = (JArray)o["info"]["build_update"];
                long endTime = (long)productTime[0]["end_time"];
                JObject id = JObject.Parse(data);
                int force_id = (int)id["id"];
                int produce_id = (int)id["produce_id"];
                string strTaskInfo = "{\"produce_id\":" + produce_id + ",\"end_time\":" + endTime + ",\"id\":" + force_id + "}";
                soldierTask.Add(strTaskInfo);
                if (soldierTask.Count > 1)
                {
                    soldierTask = soldierSorter(soldierTask);
                }

                //生产成功：   
                ShowSysLog("生产兵力在build：" + data + "成功！");
                ShowSysLog("已将下次收获时间：" + endTime + " 加入计划任务列表");
                return true;
            }
            else
            {
                if ((int)o["result"] == 209)
                {
                    //"result":223  人口数量到达上限
                    string result = o.ToString();
                    ShowSysLog("生产兵力在build：" + data + "失败！原因：" + Unicode2Character(result).Replace("\r\n", ""));
                    return false;
                }
                else
                {
                    this.soldierStatus = false;
                    string result = o.ToString();
                    ShowSysLog("生产兵力在build：" + data + "失败！原因：" + Unicode2Character(result).Replace("\r\n", ""));
                    return false;
                }
            }
        }
        #endregion 生产兵力 produceForce(string data)

        #region 收获兵力 gainForce(string data)
        /// <summary>
        /// 收获兵力
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string gainForce(string data)
        {
            string strRequest = null;
            string strContent = null;


            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Produce.gainRecruitment";
            ShowSysLog(data);
            strContent = Unicode2Character(HTTPproc.OpenRead(strRequest, "requestSig=" + strSig + "&data=" + data + "&keyName=" + strKeyName));
            strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;

            JObject o = JObject.Parse(Unicode2Character(strContent));
            if ((int)o["result"] == 0 && o["info"] != null && o["info"]["player_info"] != null)
            {
                ShowSysLog("收获兵力在build：" + data + "成功！");
            }
            else
            {
                ShowSysLog("收获兵力在build：" + data + "失败！");
            }
            return strContent;
        }
        #endregion 收获兵力 gainForce(string data)

        #region 攻击好友或者我自己的怪物 attackBeast(string data,string toUid)
        /// <summary>
        /// 攻击好友或者我自己的怪物
        /// </summary>
        /// <param name="data"></param>
        /// <param name="toUid"></param>
        /// <returns></returns>
        private string attackBeast(string data, string toUid)
        {
            data = Regex.Match(data.Replace("\r\n", ""), @"\{.*\}").Value;
            string strPrm = null;
            string strRequest = null;
            string strContent = null;
            string resultString = null;

            resultString = Regex.Match(data, @"\w{13},").Value;
            resultString = Regex.Replace(resultString, @"([a-z]\d[a-z])|([a-z][a-z]\d)|([a-z][a-z][a-z])|([a-z]\d\d)", "");
            data = Regex.Replace(data, @"\w{13},", resultString);

            try
            {
                JObject info = JObject.Parse(Unicode2Character(data));
                if (info["info"]["enter_scene"]["pve"].Type.ToString() == "Object")
                {
                    JObject pve = (JObject)info["info"]["enter_scene"]["pve"];
                    int intForce = (int)info["info"]["enter_scene"]["player_info"]["population"];

                    if (pve.ToString() == "[]" && pve.Count == 0 || pve == null)
                    {
                        //没怪物就返回
                        ShowSysLog("没有要攻击的怪物~");
                        return "";
                    }

                    if (intForce <= 20)
                    {
                        ShowSysLog("攻击力不足，无法攻击~");
                        return "";
                    }

                    foreach (object pveIds in pve)
                    {
                        resultString = Regex.Replace(pveIds.ToString(), @"\[\d{1,3}, ", "");
                        resultString = Regex.Replace(resultString, @"\}\]", "}");



                        JObject objPve = JObject.Parse(resultString);
                        strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Pve.attackBeast";
                        strPrm = "{\"pointId\":" + objPve["pointId"].ToString().Trim() + ",\"fId\":\"" + toUid + "\"}";
                        strContent = Unicode2Character(HTTPproc.OpenRead(strRequest, "requestSig=" + this.strSig + "&data=" + strPrm + "&keyName=" + this.strKeyName));

                        strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;
                        JObject o = JObject.Parse(Unicode2Character(strContent));
                        if ((int)o["result"] == 0 && o["info"] != null && o["info"]["awardItemList"] != null)
                        {
                            //打怪成功：
                            ShowSysLog("攻击怪物：" + objPve["pointId"].ToString().Trim().Replace("\r\n", "").Replace("  ", "") + "成功！得到：" + o["info"]["awardItemList"].ToString().Trim().Replace("\r\n", "").Replace("  ", ""));
                        }
                        else
                        {
                            ShowSysLog("攻击怪物：" + objPve["pointId"].ToString().Trim().Replace("\r\n", "").Replace("  ", "") + "失败！原因：" + o["info"].ToString().Trim().Replace("\r\n", "").Replace("  ", ""));
                        }
                        //Thread.Sleep(500);
                    }
                }
            }
            catch
            {
                ShowSysLog("攻击怪物时出现异常~");
            }
            return "";
        }
        #endregion 攻击好友或者我自己的怪物 attackBeast(string data,string toUid)

        #region 读取好友列表并打乱 getFriendList()
        /// <summary>
        /// 读取好友列表并打乱
        /// </summary>
        private void getFriendList()
        {
            string strRequest = null;
            string strContent = null;

            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Friend.run";
            strContent = Unicode2Character(HTTPproc.OpenRead(strRequest, "keyName=" + strKeyName + "&requestSig=" + strSig + "&data={friendlistVer:\"" + longTime + "\"}"));

            strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;
            JSONObject friendsJson = JSONConvert.DeserializeObject(Unicode2Character(strContent));
            JSONObject friendsGameDataJson = (JSONObject)((JSONObject)((JSONObject)friendsJson["info"])["gameData"])["data"];
            //ShowSysLog("好友列表：" + friendsGameDataJson.ToString().Trim().Replace("\r\n", "").Replace("  ", ""));
            ArrayList friendIds = new ArrayList();

            string[] strFriends = new string[friendsGameDataJson.Keys.Count];
            friendsGameDataJson.Keys.CopyTo(strFriends, 0);

            for (int i = 0; i < strFriends.Length; i++)
            {
                JSONObject objFriend = (JSONObject)friendsGameDataJson[strFriends[i].ToString().Trim()];
                if (Convert.ToInt32(objFriend["uid"]) != 1)
                {
                    friendIds.Add(objFriend["uid"].ToString());
                }
            }
            ShowSysLog("共有好友" + friendIds.Count + "位");
            //Random r = new Random();
            ArrayList newfriendids = randomArrayList(friendIds);
            foreach (string fid in newfriendids)
            {
                //int stop = r.Next(1000, 3000);
                //ShowSysLog("休息" + stop + "毫秒");
                //Thread.Sleep(stop);
                visitFriend(fid);
            }
        }
        #endregion 读取好友列表并打乱 getFriendList()

        #region 访问好友 visitFriend(string fId)
        /// <summary>
        /// 访问好友  
        /// 首先是偷取食物，打怪，然后是趁火打jie  
        /// 如果自己兵力够则顺便救一下
        /// </summary>
        /// <param name="fId">好友fId</param>
        private void visitFriend(string fId)
        {
            string strRequest = null;
            string strContent = null;
            string resultString = null;
            //long longLoot_time = 0;
            ShowSysLog("开始访问好友：" + fId);

            //1.获取好友信息   
            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + this.str_inuId + "&method=Scene.run";
            strContent = Unicode2Character(HTTPproc.OpenRead(strRequest, "keyName=" + this.strKeyName + "&requestSig=" + this.strSig + "&data={\"fid\":\"" + fId + "\"}"));

            strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;
            strContent = Regex.Replace(strContent, @",""position"":\w[0-9a-zA-Z]{0,}\[\w[0-9a-z]{0,6},\w[0-9a-z]{0,6},\w[0-9a-z]{0,6}\]", "");
            strContent = Regex.Replace(strContent, @",""position"":\[\w[0-9a-z]{0,6},\w[0-9a-z]{0,6},\w[0-9a-z]{0,6}\]", "");
            strContent = Regex.Replace(strContent, @":[a-z0-9]{1,}\{", ":{");
            strContent = Regex.Replace(strContent, @"e\d{0,},", ",");
            strContent = Regex.Replace(strContent, @",\w[0-9a-z]{0,}""", ",\"");
            strContent = Regex.Replace(strContent, @":\d[0-9]+\w[a-z]08", ":30008");
            strContent = Regex.Replace(strContent, @"""called"":\w{0,},", "");
            strContent = Regex.Replace(strContent, @"""id""\w+\d", "\"id\"");
            strContent = Regex.Replace(strContent, @"4000\w+\d", "40003");
            strContent = Regex.Replace(strContent, @",""build_level"":\w+\d", "");
            strContent = Regex.Replace(strContent, @":2000+\w+\d", ":20008");
            strContent = Regex.Replace(strContent, @"""\d{0,}"":\{""id"":\d{0,},""build_id"":(1|2|5|6|7|8|9)\d{0,},""build_level"":\d{0,}\},", "");
            strContent = Regex.Replace(strContent, @"\}\w{3}", "}");
            resultString = Regex.Match(strContent, @"\w{13},").Value;
            resultString = Regex.Replace(resultString, @"([a-z]\d[a-z])|([a-z][a-z]\d)|([a-z][a-z][a-z])", "");
            strContent = Regex.Replace(strContent, @"\w{13},", resultString);
            //ShowSysLog(strContent);

            try
            {
                JObject o = JObject.Parse(Unicode2Character(strContent));
                string jsonRun = strContent;

                //longLoot_time = (long)o["info"]["enter_scene"]["loot_times"];

                //2.看看能不能偷
                JObject build_list = (JObject)o["info"]["enter_scene"]["build_info"]["build_list"];
                ArrayList foodBuild = new ArrayList();
                JArray foodBuildIds = new JArray();

                foreach (object buildIds in build_list)
                {
                    try
                    {
                        resultString = Regex.Replace(buildIds.ToString(), @"\r\n", "");
                        resultString = Regex.Match(resultString, @"\{.*\}").Value;
                        //resultString = Regex.Replace(resultString, @"\[\d{1,3}, ", "");
                        //resultString = Regex.Replace(resultString, @"\}\]", "}");
                        JObject joBuildIds = JObject.Parse(resultString);
                        int intBuild_Id = (int)joBuildIds.First.Next;//["build_id"]
                        if (intBuild_Id >= 30000 && intBuild_Id < 40000)
                        {
                            foodBuild.Add(resultString);
                        }
                    }
                    catch
                    {
                        // Syntax error in the regular expression
                        ShowSysLog("1");
                    }
                }

                foreach (string buildIds in foodBuild)
                {
                    JObject joBuildIds = JObject.Parse(buildIds);
                    foodBuildIds.Add((int)joBuildIds.First);//["id"]
                }

                ShowSysLog("好友食物工厂有：" + foodBuildIds.ToString().Replace("\r\n", "").Replace("  ", ""));

                //针对每个食物进行处理 
                foreach (string buildIds in foodBuild)
                {
                    try
                    {
                        //看看是不是空的，可以生产，或者已经生产完成可以收获
                        JObject build = JObject.Parse(buildIds);

                        long st = (long)build.First.Next.Next.Next;//["start_time"];
                        long et = (long)build.First.Next.Next.Next.Next;//["end_time"];

                        if (st == 0)
                        {

                        }
                        else if (et <= Timestamp())
                        {
                            //表示已经完成了，看看我自己偷过没有，没有就偷   
                            //先看看剩余的够不够偷？   
                            int stolen = (int)build["stolen"];
                            int remain = (int)build["remain_food"];
                            int takeTax = (int)build["takeTax"];

                            //只能偷10%   
                            if ((stolen + takeTax) * 100 / (stolen + remain + takeTax) < 10)
                            {
                                JArray stealList = (JArray)build["stealing_list"];
                                bool bsteal = false;
                                for (int i = 0; i < stealList.Count; i++)
                                {
                                    int id = (int)stealList[i];
                                    if (id == intUid)
                                    {
                                        bsteal = true;
                                        break;
                                    }
                                }

                                if (bsteal)
                                {
                                    //log("已经偷过该好友食物所以不去偷取了");   
                                    continue;//已经偷过了                           
                                }
                                //偷啊！   

                                strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Social.stealingFood";
                                string data = "{\"desc_id\":\"" + fId + "\",\"factory_id\":" + (int)build["id"] + ",\"ids\":" + foodBuildIds.ToString().Replace("\r\n", "").Replace("  ", "") + "}";
                                strContent = HTTPproc.OpenRead(strRequest, "keyName=" + strKeyName + "&requestSig=" + strSig + "&data={\"desc_id\":\"" + fId + "\",\"factory_id\":" + (int)build["id"] + ",\"ids\":" + foodBuildIds + "}");
                                strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;
                                JObject joResult = JObject.Parse(Unicode2Character(strContent));

                                if ((int)joResult["result"] == 0 && joResult["info"] != null && joResult["info"]["player_info"] != null)
                                {
                                    //偷取成功：   
                                    ShowSysLog("偷取好友食物：" + data + "成功！");
                                }
                                else
                                {
                                    ShowSysLog("偷取好友食物：" + data + "失败！");
                                }
                                //Thread.Sleep(500);

                            }
                            else
                            {
                                ShowSysLog("好友食物不多，不能在偷取了");
                            }
                        }
                    }
                    catch
                    {
                        ShowSysLog("2");
                    }
                }

                //3.看看有没有怪物打   
                attackBeast(jsonRun, fId);

                //4.看看好友是不是被占领了            
                try
                {
                    //看看是不是打jie过了  
                    Object t1 = o["info"]["enter_scene"]["loot_flag"];
                    if (t1 != null && -1 == Convert.ToInt32(t1.ToString()))
                    {
                        ShowSysLog("已经趁火打劫过了!");
                        return;
                    }
                    if ((int)o["info"]["enter_scene"]["loot_times"] <= 0)
                    {
                        ShowSysLog("趁火打劫没有机会了!");
                        return;
                    }

                    //被占领了耶。先趁火打jie，再营救   
                    strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Defence.loot";
                    string data = "{\"desc_id\":\"" + fId + "\"}";
                    strContent = HTTPproc.OpenRead(strRequest, "keyName=" + strKeyName + "&requestSig=" + strSig + "&data={\"desc_id\":\"" + fId + "\"}");
                    strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;

                    JObject jsonR = JObject.Parse(strContent);
                    if ((int)jsonR["result"] == 0 && jsonR["info"] != null && jsonR["info"]["player_info"] != null)
                    {
                        //打jie成功：   
                        //log(jsonR);   
                        ShowSysLog("趁火打劫好友：" + data + "成功！");
                    }
                    else
                    {
                        ShowSysLog("趁火打劫：" + data + "失败！");
                    }
                    //看看自己的战斗力够不                    
                    if (o["info"]["enter_scene"]["master_info"].Type.ToString() == "Object")
                    {
                        JObject jsonMaster = (JObject)o["info"]["enter_scene"]["master_info"];
                        User master = new User();
                        master.updateNoTime(jsonMaster);
                        if (u.getForce() > master.getForce())
                        {
                            //TODO 营救                  
                        }
                    }
                }
                catch
                {
                    ShowSysLog("3");
                }
            }
            catch
            {
                ShowSysLog("读取好友：" + fId + "时出错，已跳过");
            }
        }
        #endregion 访问好友 visitFriend(string fId)

        private void productTasks()
        {
            long pTime = 0;
            int build_Id = 0;
            int intFoodID = 1;
            string build_Ids = null;
            string strGainFood = null;
            string strProductFood = null;
            bool result = false;
            try
            {
                JObject o = JObject.Parse(productTask[0].ToString());

                build_Id = (int)o["id"];
                pTime = (long)o["end_time"];
                build_Ids = (string)o["ids"];

                t = pTime + 5 - Timestamp();//得到总的毫秒数   
                this.label3.Text = GetAllTime(t);
                if (t < 0)
                {
                    strGainFood = "{\"ids\":" + build_Ids + ",\"id\":" + build_Id + "}";
                    gainFood(strGainFood);

                    //Random r = new Random();
                    //int stop = r.Next(1000, 3000);
                    //Thread.Sleep(stop);

                    strProductFood = "{\"produce_id\":" + intFoodID + ",\"id\":" + build_Id + "}";
                    result = produceFood(strProductFood);
                    if (result == true || this.productStatus == false)
                    {
                        productTask.RemoveAt(0);
                    }
                    //else
                    //{
                    //    Thread.Sleep(3000);
                    //}

                }
            }
            catch
            { }
        }

        private void soldierTasks()
        {
            long pTime = 0;
            int force_Id = 0;
            int produce_id = 0;
            string strGainForce = null;
            string strProductForce = null;
            bool result = false;

            try
            {
                JObject o = JObject.Parse(soldierTask[0].ToString());

                force_Id = (int)o["id"];
                pTime = (long)o["end_time"];
                produce_id = (int)o["produce_id"];

                t = pTime + 3 - Timestamp();//得到总的毫秒数   
                this.label4.Text = GetAllTime(t);
                if (t < 0)
                {
                    strGainForce = "{\"id\":" + force_Id + "}";
                    gainForce(strGainForce);

                    //Random r = new Random();
                    //int stop = r.Next(1000, 3000);
                    //Thread.Sleep(stop);

                    strProductForce = "{\"produce_id\":" + produce_id + ",\"id\":" + force_Id + "}";
                    result = produceForce(strProductForce);
                    if (result == true || this.soldierStatus == false)
                    {
                        soldierTask.RemoveAt(0);
                    }
                    //else
                    //{
                    //    Thread.Sleep(3000);
                    //}
                }
            }
            catch
            { }
        }

        #region 计划任务排序

        //生产排序
        public ArrayList productSorter(ArrayList list)
        {

            int i, j, temp;
            bool done = false;
            j = 1;
            while ((j < list.Count) && (!done))
            {
                done = true;
                for (i = 0; i < list.Count - j; i++)
                {
                    JObject o = JObject.Parse(list[i].ToString());
                    JObject oo = JObject.Parse(list[i + 1].ToString());

                    int id_1 = (int)o["id"];
                    int id_2 = (int)oo["id"];
                    string ids_1 = (string)o["ids"];
                    string ids_2 = (string)oo["ids"];
                    long end_time_1 = (long)o["end_time"];
                    long end_time_2 = (long)oo["end_time"];

                    if ((int)o["end_time"] > (int)oo["end_time"])
                    {
                        done = false;
                        temp = (int)o["end_time"];
                        list[i] = "{\"id\":" + id_2 + ",\"end_time\":" + end_time_2 + ",\"ids\":\"" + ids_2 + "\"}";
                        list[i + 1] = "{\"id\":" + id_1 + ",\"end_time\":" + end_time_1 + ",\"ids\":\"" + ids_1 + "\"}";
                    }
                }
                j++;
            }
            return list;
        }

        //造兵排序
        public ArrayList soldierSorter(ArrayList list)
        {

            int i, j, temp;
            bool done = false;
            j = 1;
            while ((j < list.Count) && (!done))
            {
                done = true;
                for (i = 0; i < list.Count - j; i++)
                {
                    JObject o = JObject.Parse(list[i].ToString());
                    JObject oo = JObject.Parse(list[i + 1].ToString());

                    int id_1 = (int)o["id"];
                    int id_2 = (int)oo["id"];
                    int produce_id_1 = (int)o["produce_id"];
                    int produce_id_2 = (int)oo["produce_id"];
                    long end_time_1 = (long)o["end_time"];
                    long end_time_2 = (long)oo["end_time"];

                    if ((int)o["end_time"] > (int)oo["end_time"])
                    {
                        done = false;
                        temp = (int)o["end_time"];

                        list[i] = "{\"produce_id\":" + produce_id_2 + ",\"end_time\":" + end_time_2 + ",\"id\":" + id_2 + "}";
                        list[i + 1] = "{\"produce_id\":" + produce_id_1 + ",\"end_time\":" + end_time_1 + ",\"id\":" + id_1 + "}";
                    }
                }
                j++;
            }
            return list;
        }

        //偷取排序
        public ArrayList stealSorter(ArrayList list)
        {
            int i, j, temp;
            bool done = false;
            j = 1;
            while ((j < list.Count) && (!done))
            {
                done = true;
                for (i = 0; i < list.Count - j; i++)
                {
                    JObject o = JObject.Parse(list[i].ToString());
                    JObject oo = JObject.Parse(list[i + 1].ToString());

                    int id_1 = (int)o["id"];
                    int id_2 = (int)oo["id"];
                    string ids_1 = (string)o["ids"];
                    string ids_2 = (string)oo["ids"];
                    long end_time_1 = (long)o["end_time"];
                    long end_time_2 = (long)oo["end_time"];

                    if ((int)o["end_time"] > (int)oo["end_time"])
                    {
                        done = false;
                        temp = (int)o["end_time"];
                        list[i] = "{\"id\":" + id_2 + ",\"end_time\":" + end_time_2 + ",\"ids\":\"" + ids_2 + "\"}";
                        list[i + 1] = "{\"id\":" + id_1 + ",\"end_time\":" + end_time_1 + ",\"ids\":\"" + ids_1 + "\"}";
                    }
                }
                j++;
            }
            return list;
        }

        #endregion 计划任务排序

        #region 显示系统日志 ShowSysLog(string strLog)
        /// <summary>
        /// 显示系统日志
        /// </summary>
        /// <param name="strLog"></param>        
        private void ShowSysLog(string strLog)
        {
            DateTime dt_1 = DateTime.Now;
            textBox_Log.Text += dt_1 + "  " + strLog + "\r\n";
            textBox_Log.SelectionStart = textBox_Log.Text.Length;
            textBox_Log.ScrollToCaret();
            //始终显示TextBox最新一行，始终滚动到最底部
            textBox_Log.SelectionStart = textBox_Log.Text.Length;
            textBox_Log.ScrollToCaret();
        }
        #endregion

        #region 得到Sig值 Sig(string strKey, string strKeyName)
        /// <summary>
        /// 得到Sig值
        /// </summary>
        /// <param name="strKey">Key</param>
        /// <param name="strKeyName">KeyName</param>
        /// <returns></returns>
        private static string Sig(string strKey, string strKeyName)
        {
            int intInc = 0;
            int intkey = 0;
            string strKeyMD5_2 = null;
            string strSig = null;

            #region 返回KeyName的值
            switch (strKeyName)
            {
                case "97ba558178f22af9":
                    intInc = 1;
                    break;
                case "8a57faa3ff0c2cd0":
                    intInc = 2;
                    break;
                case "b05395426617a666":
                    intInc = 3;
                    break;
                case "8054b38ece415448":
                    intInc = 4;
                    break;
                case "5a0815d2500be4c3":
                    intInc = 5;
                    break;
                case "cb47e040c444bb13":
                    intInc = 6;
                    break;
                case "4f0b466d4e838204":
                    intInc = 7;
                    break;
                case "9bb033dd03a03a21":
                    intInc = 8;
                    break;
                case "a548d6aefbeb32e0":
                    intInc = 9;
                    break;
                case "c156d1c03531d5f6":
                    intInc = 10;
                    break;
            }
            #endregion 返回KeyName的值

            strKeyMD5_2 = UserMd5(UserMd5(strKey));
            strKeyMD5_2 = strKeyMD5_2.Substring(1, 6);
            strKeyMD5_2 = UserMd5(strKeyMD5_2);
            strKeyMD5_2 = strKeyMD5_2.Substring(0, 6);
            intkey = Int32.Parse(strKeyMD5_2, System.Globalization.NumberStyles.AllowHexSpecifier);
            intkey = intkey + intInc;
            strSig = UserMd5(Convert.ToString(intkey));

            return strSig;
        }
        #endregion 得到Sig值 Sig(string strKey, string strKeyName)

        #region MD5加密算法 UserMd5(string str)
        /// <summary>
        /// MD5加密算法
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <returns>输出32位小写MD5加密值</returns>
        static string UserMd5(string str)
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

        #region URL编码 UrlEncode(string str, string encode)
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

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.";
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

        #region 将Unicode转找为Character Unicode2Character(string str)
        /// <summary>
        /// 将Unicode转找为Character
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns></returns>
        private string Unicode2Character(string str)
        {
            string text = str;
            string strPattern = "(?<code>\\\\u[A-F0-9]{4})";
            do
            {
                Match m = Regex.Match(text, strPattern, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    string strValue = m.Groups["code"].Value;
                    int i = System.Int32.Parse(strValue.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                    char ch = Convert.ToChar(i);
                    text = text.Replace(strValue, ch.ToString());
                }
                else
                {
                    break;
                }
            }
            while (true);

            return text;
        }
        #endregion

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

        #region 将数组打乱
        /// <summary>
        /// 将数组打乱
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        private ArrayList randomArrayList(ArrayList alIn)
        {
            Random rnd = new Random((int)(DateTime.Now.Ticks & 0xffffffffL) | (int)(DateTime.Now.Ticks >> 32));
            int len = alIn.Count;
            int r = 0;
            ArrayList alReturn = new ArrayList();
            for (int i = 0; i < len; i++)
            {
                r = rnd.Next(0, alIn.Count - 1);
                alReturn.Add(alIn[r]);
                alIn.Remove(alIn[r]);
            }
            return alReturn;
        }
        #endregion

        #region 时间格式化 GetAllTime(long time)
        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string GetAllTime(long time)
        {
            string hh, mm, ss;//, fff;

            //long f = time % 100; // 毫秒   
            long s = time;// / 100; // 转化为秒
            long m = (s / 60) % 60;     // 分
            long h = s / 3600;     // 时
            s = s % 60;     // 秒 

            //毫秒格式00
            //if (f < 10)
            //{
            //    fff = "0" + f.ToString();
            //}
            //else
            //{
            //    fff = f.ToString();
            //}

            //秒格式00
            if (s < 10)
            {
                ss = "0" + s.ToString();
            }
            else
            {
                ss = s.ToString();
            }

            //分格式00
            if (m < 10)
            {
                mm = "0" + m.ToString();
            }
            else
            {
                mm = m.ToString();
            }

            //时格式00
            if (h < 10)
            {
                hh = "0" + h.ToString();
            }
            else
            {
                hh = h.ToString();
            }

            //返回 hh:mm:ss.ff            
            return hh + ":" + mm + ":" + ss;// +"." + fff;
        }
        #endregion 时间格式化 GetAllTime(long time)

        #region 计时器
        private void starttimer(object obj)
        {
            long longTime = Timestamp() - u.getSystemTime();
            if (longTime > 3600)
            {
                //this.timer1.Enabled = false;
                if (this.tm != null)
                {
                    this.tm.Dispose();
                    this.tm = null;
                }
                Start();
            }
            else if (this.productTask.Count == 0 && this.soldierTask.Count == 0)
            {
                //this.timer1.Enabled = false;
                if (this.tm != null)
                {
                    this.tm.Dispose();
                    this.tm = null;
                }
                Start();
            }

            this.tm.Change(Timeout.Infinite, Timeout.Infinite);
            if (this.boolAuth == false)
            {
                TestUser();
            }
            productTasks();
            soldierTasks();
            this.tm.Change(1000, 1000); 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.boolAuth == false)
            {
                TestUser();
            }
            long longTime = Timestamp() - u.getSystemTime();
            productTasks();
            soldierTasks();
            if (longTime > 3600)
            {
                //this.timer1.Enabled = false;
                Start();
            }
            else if (this.productTask.Count == 0 && this.soldierTask.Count == 0)
            {
                //this.timer1.Enabled = false;
                Start();
            }
        }

        #endregion 计时器

        #region 查检认证用户 CheckAuthUser(string strUserName)
        /// <summary>
        /// 查检认证用户
        /// </summary>
        private void CheckAuthUser(string strUserName)
        {
            bool boolStatus = false;
            string strSQL = "SELECT TOP 1 [Status] FROM [LW_User] WHERE UserName = '" + strUserName + "'";

            DataRow dr = SqlHelper.ExecuteDataRow(SqlLibrary.GetServer_Main(), CommandType.Text, strSQL);

            if (dr == null)
            {
                strSQL = "INSERT INTO LW_User(UserName) VALUES('" + strUserName + "')";
                SqlHelper.ExecuteNonQuery(SqlLibrary.GetServer_Main(), CommandType.Text, strSQL);

                ShowSysLog("您的帐号" + strUserName + "已经成功激活！");
                ShowSysLog("您现在是未授权用户，可以试用本软件20分钟。");
                this.boolAuth = false;
                string strText = this.Text.ToString().Trim();
                strText = strText.Replace("(未授权)", "").Replace("(已授权)", "");
                this.Text = strText + "  (未授权)";
            }
            else if (dr != null)
            {
                boolStatus = (bool)dr["Status"];

                if (boolStatus == false)
                {
                    ShowSysLog("您现在是未授权用户，可以试用本软件20分钟。");
                    this.boolAuth = false;
                    string strText = this.Text.ToString().Trim();
                    strText = strText.Replace("(未授权)", "").Replace("(已授权)", "");
                    this.Text = strText + "  (未授权)";
                }
                else
                {
                    ShowSysLog("尊敬的会员" + strUserName + "欢迎您的登录。");
                    this.boolAuth = true;
                    string strText = this.Text.ToString().Trim();
                    strText = strText.Replace("  (未授权)", "").Replace("(已授权)", "");
                    this.Text = strText + "  (已授权)";
                }
            }
        }
        #endregion 查检认证用户 CheckAuthUser(string strUserName)

        #region 试用20分钟 TestUser()
        private void TestUser()
        {
            this.tu++;
            if (this.tu == 1200)
            {
                Application.Exit();
            }
        }
        #endregion 试用20分钟 TestUser()
    }

    #region JSONConvert
    /// <summary>
    /// 类  名：JSONConvert
    /// 描  述：JSON解析类
    /// 编  写：dnawo
    /// 站  点：http://www.mzwu.com/
    /// 日  期：2010-01-06
    /// 版  本：1.1.0
    /// </summary>
    public static class JSONConvert
    {
        #region 全局变量

        private static JSONObject _json = new JSONObject();//寄存器
        private static readonly string _SEMICOLON = "@semicolon";//分号转义符
        private static readonly string _COMMA = "@comma"; //逗号转义符

        #endregion

        #region 字符串转义
        /// <summary>
        /// 字符串转义,将双引号内的:和,分别转成_SEMICOLON和_COMMA
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string StrEncode(string text)
        {
            MatchCollection matches = Regex.Matches(text, "\\\"[^\\\"]*\\\"");
            foreach (Match match in matches)
            {
                text = text.Replace(match.Value, match.Value.Replace(":", _SEMICOLON).Replace(",", _COMMA));
            }

            return text;
        }

        /// <summary>
        /// 字符串转义,将_SEMICOLON和_COMMA分别转成:和,
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string StrDecode(string text)
        {
            return text.Replace(_SEMICOLON, ":").Replace(_COMMA, ",");
        }

        #endregion

        #region JSON最小单元解析

        /// <summary>
        /// 最小对象转为JSONObject
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static JSONObject DeserializeSingletonObject(string text)
        {
            JSONObject jsonObject = new JSONObject();

            MatchCollection matches = Regex.Matches(text, "(\\\"(?<key>[^\\\"]+)\\\":\\\"(?<value>[^,\\\"]*)\\\")|(\\\"(?<key>[^\\\"]+)\\\":(?<value>[^,\\\"\\}]*))");
            foreach (Match match in matches)
            {
                string value = match.Groups["value"].Value;
                jsonObject.Add(match.Groups["key"].Value, _json.ContainsKey(value) ? _json[value] : StrDecode(value));
            }

            return jsonObject;
        }

        /// <summary>
        /// 最小数组转为JSONArray
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static JSONArray DeserializeSingletonArray(string text)
        {
            JSONArray jsonArray = new JSONArray();

            MatchCollection matches = Regex.Matches(text, "(\\\"(?<value>[^,\\\"]+)\")|(?<value>[^,\\[\\]]+)");
            foreach (Match match in matches)
            {
                string value = match.Groups["value"].Value;
                jsonArray.Add(_json.ContainsKey(value) ? _json[value] : StrDecode(value));
            }

            return jsonArray;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string Deserialize(string text)
        {
            _json.Clear();//2010-01-20 清空寄存器
            text = StrEncode(text);//转义;和,

            int count = 0;
            string key = string.Empty;
            string pattern = "(\\{[^\\[\\]\\{\\}]*\\})|(\\[[^\\[\\]\\{\\}]*\\])";

            while (Regex.IsMatch(text, pattern))
            {
                MatchCollection matches = Regex.Matches(text, pattern);
                foreach (Match match in matches)
                {
                    key = "___key" + count + "___";

                    if (match.Value.Substring(0, 1) == "{")
                        _json.Add(key, DeserializeSingletonObject(match.Value));
                    else
                        _json.Add(key, DeserializeSingletonArray(match.Value));

                    text = text.Replace(match.Value, key);

                    count++;
                }
            }
            return text;
        }

        #endregion

        #region 公共接口

        /// <summary>
        /// 反序列化JSONObject对象
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static JSONObject DeserializeObject(string text)
        {
            return _json[Deserialize(text)] as JSONObject;
        }

        /// <summary>
        /// 反序列化JSONArray对象
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static JSONArray DeserializeArray(string text)
        {
            return _json[Deserialize(text)] as JSONArray;
        }

        /// <summary>
        /// 序列化JSONObject对象
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        public static string SerializeObject(JSONObject jsonObject)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (KeyValuePair<string, object> kvp in jsonObject)
            {
                if (kvp.Value is JSONObject)
                {
                    sb.Append(string.Format("\"{0}\":{1},", kvp.Key, SerializeObject((JSONObject)kvp.Value)));
                }
                else if (kvp.Value is JSONArray)
                {
                    sb.Append(string.Format("\"{0}\":{1},", kvp.Key, SerializeArray((JSONArray)kvp.Value)));
                }
                else if (kvp.Value is String)
                {
                    sb.Append(string.Format("\"{0}\":\"{1}\",", kvp.Key, kvp.Value));
                }
                else
                {
                    sb.Append(string.Format("\"{0}\":\"{1}\",", kvp.Key, ""));
                }
            }
            if (sb.Length > 1)
                sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 序列化JSONArray对象
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <returns></returns>
        public static string SerializeArray(JSONArray jsonArray)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < jsonArray.Count; i++)
            {
                if (jsonArray[i] is JSONObject)
                {
                    sb.Append(string.Format("{0},", SerializeObject((JSONObject)jsonArray[i])));
                }
                else if (jsonArray[i] is JSONArray)
                {
                    sb.Append(string.Format("{0},", SerializeArray((JSONArray)jsonArray[i])));
                }
                else if (jsonArray[i] is String)
                {
                    sb.Append(string.Format("\"{0}\",", jsonArray[i]));
                }
                else
                {
                    sb.Append(string.Format("\"{0}\",", ""));
                }

            }
            if (sb.Length > 1)
                sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }

    /// <summary>
    /// 类  名：JSONObject
    /// 描  述：JSON对象类
    /// 编  写：dnawo
    /// 站  点：http://www.mzwu.com/
    /// 日  期：2010-01-06
    /// 版  本：1.1.0
    /// 更新历史：
    ///     2010-01-06  继承Dictionary<TKey, TValue>代替this[]
    /// </summary>
    public class JSONObject : Dictionary<string, object>
    { }

    /// <summary>
    /// 类  名：JSONArray
    /// 描  述：JSON数组类
    /// 编  写：dnawo
    /// 站  点：http://www.mzwu.com/
    /// 日  期：2010-01-06
    /// 版  本：1.1.0
    /// 更新历史：
    ///     2010-01-06  继承List<T>代替this[]
    /// </summary>
    public class JSONArray : List<object>
    { }
    #endregion JSONConvert
}