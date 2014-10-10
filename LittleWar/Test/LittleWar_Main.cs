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
        private int tu = 0;//�����ü�����
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

        #region �������� Start()
        /// <summary>
        /// ��������
        /// </summary>
        private void Start()
        {
            this.productStatus = true;
            this.soldierStatus = true;
            this.productTask.Clear();
            this.soldierTask.Clear();

            if (textBox_UserName.Text == "" || textBox_Password.Text == "")
            {
                ShowSysLog("�û��������벻��Ϊ�գ�");
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
        #endregion �������� Start()


        #region ��¼У�� Login_Xiaonei(string strUserName, string strPassword)
        /// <summary>
        /// ��¼У��
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        private string Login_Xiaonei(string strUserName, string strPassword)
        {
            string strContent = null;

            ShowSysLog("���ڵ�¼У��...");

            if (strUserName.Trim() != "" && strPassword.Trim() != "")
            {
                //����HTTP����Ĭ�ϱ���
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
                    ShowSysLog("�û������������");
                }
            }
            else
            {
                ShowSysLog("��Ϣ��д�������飡");
            }

            return strContent;
        }
        #endregion ��¼У�� Login_Xiaonei(string strUserName, string strPassword)

        #region ��¼ССս�� Login_Game()
        /// <summary>
        /// ��¼ССս��
        /// </summary>
        /// <returns>����Sig</returns>
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

            ShowSysLog("���ڵ�¼��Ϸ...");

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

            //׼���ٴ�����ʱʹ��
            strRequest = resultString;
            //ȥ������������
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

                //��ȡ�����Ϣ   
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
                ShowSysLog("��ȡScene.init����");
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
        #endregion ��¼ССս�� Login_Game()

        #region �������ջ�ʳ�� gainAndProduceFood(string userRun, int intFoodID)
        /// <summary>
        /// �������ջ�ʳ��
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

            //����ʳ��
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

            //����ʳ���б����ĸ����ջ�� 
            for (int j = 0; j < foodBuild.Count; j++)
            {
                foodBuildIds.Add(foodBuild[j]);
            }
            buildListIds.Add(foodBuildIds.ToString().Replace("\r\n", "").Replace("  ", ""));

            //�ջ�����ʳ��
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
                        //�����ջ���                        
                        string data = "{\"ids\":" + foodBuildIds.ToString().Replace("\r\n", "").Replace("  ", "") + ",\"id\":" + (int)objBuild.First + "}";//["id"]
                        gainFood(data);
                        //�ջ���������
                        data = "{\"produce_id\":" + intFoodID + ",\"id\":" + (int)objBuild.First + "}";//["id"]
                        produceFood(data);
                    }
                    else if (et > Timestamp())
                    {
                        string userRuns = "{\"id\":" + (int)objBuild.First + ",\"end_time\":" + et + ",\"ids\":\"" + foodBuildIds.ToString().Replace("\r\n", "").Replace("  ", "") + "\"}";//["id"]
                        productTask.Add(userRuns);
                        ShowSysLog("�ѽ��´��ջ�ʱ�䣺" + et + " ����ƻ������б�");
                        if (productTask.Count > 1)
                        {
                            productTask = productSorter(productTask);//����
                        }
                    }
                }
            }
        }
        #endregion �������ջ�ʳ�� gainAndProduceFood(string userRun, int intFoodID)

        #region ����ʳ�� produceFood(string data)
        /// <summary>
        /// ����ʳ��
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
                    productTask = productSorter(productTask);//����
                }

                //�����ɹ���
                ShowSysLog("����ʳ����build��" + data + "�ɹ���");
                //ShowSysLog("�´��ջ�ʱ���ǣ���ʼʱ�� " + (long)productTime[0]["start_time"] + " �ջ�ʱ�䣺" + (long)productTime[0]["end_time"] + " ����ʱ���ǣ�" + Timestamp());                
                ShowSysLog("�ѽ��´��ջ�ʱ�䣺" + endTime + " ����ƻ������б�");
                return true;
            }
            else
            {
                if ((int)o["result"] == 209)
                {
                    //{"result":102,"msg":"Session Error"}
                    string result = o.ToString();
                    ShowSysLog("����ʳ����build��" + data + "ʧ�ܣ�ԭ��" + Unicode2Character(result).Replace("\r\n", ""));
                    return false;
                }
                else
                {
                    this.productStatus = false;
                    string result = o.ToString();
                    ShowSysLog("����ʳ����build��" + data + "ʧ�ܣ�ԭ��" + Unicode2Character(result).Replace("\r\n", ""));
                    return false;
                }
            }
        }
        #endregion ����ʳ�� produceFood(string data)

        #region �ջ�ʳ�� gainFood(string data)
        /// <summary>
        /// �ջ�ʳ��
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
                ShowSysLog("�ջ�ʳ����build��" + data + "�ɹ���");
            }
            else
            {
                ShowSysLog("�ջ�ʳ����build��" + data + "ʧ�ܣ�");
            }
            return strContent;
        }
        #endregion �ջ�ʳ�� gainFood(string data)

        #region �������ջ���� gainAndProduceForce(string userRun, int intForceID)
        /// <summary>
        /// �������ջ����
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
                    //�����ʳ��
                }
                else if (intBuild_Id >= 40000 && intBuild_Id < 50000)
                {
                    //������
                    long st = (long)objBuild.First.Next.Next.Next;//["start_time"];
                    long et = (long)objBuild.First.Next.Next.Next.Next;//["end_time"];

                    if (st == 0)
                    {
                        //�յ� ������ֱ��������
                        string data = "{\"produce_id\":" + intForceID + ",\"id\":" + (int)objBuild.First + "}";//["id"]
                        produceForce(data);
                    }
                    else if (et <= Timestamp())
                    {
                        //���ձ�����������
                        string data = "{\"id\":" + (int)objBuild.First + "}";//["id"]
                        gainForce(data);
                        data = "{\"produce_id\":" + intForceID + ",\"id\":" + (int)objBuild.First + "}";//["id"]
                        produceForce(data);
                    }
                    else if (et > Timestamp())
                    {
                        string userRuns = "{\"produce_id\":" + intForceID + ",\"end_time\":" + et + ",\"id\":" + (int)objBuild.First + "}";//["id"]
                        soldierTask.Add(userRuns);
                        ShowSysLog("�ѽ��´��ջ�ʱ�䣺" + et + " ����ƻ������б�");
                        if (soldierTask.Count > 1)
                        {
                            soldierTask = soldierSorter(soldierTask);
                        }
                    }
                }
            }
        }
        #endregion �������ջ���� gainAndProduceForce(string userRun, int intForceID)

        #region �������� produceForce(string data)
        /// <summary>
        /// ��������
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

                //�����ɹ���   
                ShowSysLog("����������build��" + data + "�ɹ���");
                ShowSysLog("�ѽ��´��ջ�ʱ�䣺" + endTime + " ����ƻ������б�");
                return true;
            }
            else
            {
                if ((int)o["result"] == 209)
                {
                    //"result":223  �˿�������������
                    string result = o.ToString();
                    ShowSysLog("����������build��" + data + "ʧ�ܣ�ԭ��" + Unicode2Character(result).Replace("\r\n", ""));
                    return false;
                }
                else
                {
                    this.soldierStatus = false;
                    string result = o.ToString();
                    ShowSysLog("����������build��" + data + "ʧ�ܣ�ԭ��" + Unicode2Character(result).Replace("\r\n", ""));
                    return false;
                }
            }
        }
        #endregion �������� produceForce(string data)

        #region �ջ���� gainForce(string data)
        /// <summary>
        /// �ջ����
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
                ShowSysLog("�ջ������build��" + data + "�ɹ���");
            }
            else
            {
                ShowSysLog("�ջ������build��" + data + "ʧ�ܣ�");
            }
            return strContent;
        }
        #endregion �ջ���� gainForce(string data)

        #region �������ѻ������Լ��Ĺ��� attackBeast(string data,string toUid)
        /// <summary>
        /// �������ѻ������Լ��Ĺ���
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
                        //û����ͷ���
                        ShowSysLog("û��Ҫ�����Ĺ���~");
                        return "";
                    }

                    if (intForce <= 20)
                    {
                        ShowSysLog("���������㣬�޷�����~");
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
                            //��ֳɹ���
                            ShowSysLog("�������" + objPve["pointId"].ToString().Trim().Replace("\r\n", "").Replace("  ", "") + "�ɹ����õ���" + o["info"]["awardItemList"].ToString().Trim().Replace("\r\n", "").Replace("  ", ""));
                        }
                        else
                        {
                            ShowSysLog("�������" + objPve["pointId"].ToString().Trim().Replace("\r\n", "").Replace("  ", "") + "ʧ�ܣ�ԭ��" + o["info"].ToString().Trim().Replace("\r\n", "").Replace("  ", ""));
                        }
                        //Thread.Sleep(500);
                    }
                }
            }
            catch
            {
                ShowSysLog("��������ʱ�����쳣~");
            }
            return "";
        }
        #endregion �������ѻ������Լ��Ĺ��� attackBeast(string data,string toUid)

        #region ��ȡ�����б����� getFriendList()
        /// <summary>
        /// ��ȡ�����б�����
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
            //ShowSysLog("�����б�" + friendsGameDataJson.ToString().Trim().Replace("\r\n", "").Replace("  ", ""));
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
            ShowSysLog("���к���" + friendIds.Count + "λ");
            //Random r = new Random();
            ArrayList newfriendids = randomArrayList(friendIds);
            foreach (string fid in newfriendids)
            {
                //int stop = r.Next(1000, 3000);
                //ShowSysLog("��Ϣ" + stop + "����");
                //Thread.Sleep(stop);
                visitFriend(fid);
            }
        }
        #endregion ��ȡ�����б����� getFriendList()

        #region ���ʺ��� visitFriend(string fId)
        /// <summary>
        /// ���ʺ���  
        /// ������͵ȡʳ���֣�Ȼ���ǳû��jie  
        /// ����Լ���������˳���һ��
        /// </summary>
        /// <param name="fId">����fId</param>
        private void visitFriend(string fId)
        {
            string strRequest = null;
            string strContent = null;
            string resultString = null;
            //long longLoot_time = 0;
            ShowSysLog("��ʼ���ʺ��ѣ�" + fId);

            //1.��ȡ������Ϣ   
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

                //2.�����ܲ���͵
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

                ShowSysLog("����ʳ�﹤���У�" + foodBuildIds.ToString().Replace("\r\n", "").Replace("  ", ""));

                //���ÿ��ʳ����д��� 
                foreach (string buildIds in foodBuild)
                {
                    try
                    {
                        //�����ǲ��ǿյģ����������������Ѿ�������ɿ����ջ�
                        JObject build = JObject.Parse(buildIds);

                        long st = (long)build.First.Next.Next.Next;//["start_time"];
                        long et = (long)build.First.Next.Next.Next.Next;//["end_time"];

                        if (st == 0)
                        {

                        }
                        else if (et <= Timestamp())
                        {
                            //��ʾ�Ѿ�����ˣ��������Լ�͵��û�У�û�о�͵   
                            //�ȿ���ʣ��Ĺ�����͵��   
                            int stolen = (int)build["stolen"];
                            int remain = (int)build["remain_food"];
                            int takeTax = (int)build["takeTax"];

                            //ֻ��͵10%   
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
                                    //log("�Ѿ�͵���ú���ʳ�����Բ�ȥ͵ȡ��");   
                                    continue;//�Ѿ�͵����                           
                                }
                                //͵����   

                                strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Social.stealingFood";
                                string data = "{\"desc_id\":\"" + fId + "\",\"factory_id\":" + (int)build["id"] + ",\"ids\":" + foodBuildIds.ToString().Replace("\r\n", "").Replace("  ", "") + "}";
                                strContent = HTTPproc.OpenRead(strRequest, "keyName=" + strKeyName + "&requestSig=" + strSig + "&data={\"desc_id\":\"" + fId + "\",\"factory_id\":" + (int)build["id"] + ",\"ids\":" + foodBuildIds + "}");
                                strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;
                                JObject joResult = JObject.Parse(Unicode2Character(strContent));

                                if ((int)joResult["result"] == 0 && joResult["info"] != null && joResult["info"]["player_info"] != null)
                                {
                                    //͵ȡ�ɹ���   
                                    ShowSysLog("͵ȡ����ʳ�" + data + "�ɹ���");
                                }
                                else
                                {
                                    ShowSysLog("͵ȡ����ʳ�" + data + "ʧ�ܣ�");
                                }
                                //Thread.Sleep(500);

                            }
                            else
                            {
                                ShowSysLog("����ʳ�ﲻ�࣬������͵ȡ��");
                            }
                        }
                    }
                    catch
                    {
                        ShowSysLog("2");
                    }
                }

                //3.������û�й����   
                attackBeast(jsonRun, fId);

                //4.���������ǲ��Ǳ�ռ����            
                try
                {
                    //�����ǲ��Ǵ�jie����  
                    Object t1 = o["info"]["enter_scene"]["loot_flag"];
                    if (t1 != null && -1 == Convert.ToInt32(t1.ToString()))
                    {
                        ShowSysLog("�Ѿ��û��ٹ���!");
                        return;
                    }
                    if ((int)o["info"]["enter_scene"]["loot_times"] <= 0)
                    {
                        ShowSysLog("�û���û�л�����!");
                        return;
                    }

                    //��ռ����Ү���ȳû��jie����Ӫ��   
                    strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Defence.loot";
                    string data = "{\"desc_id\":\"" + fId + "\"}";
                    strContent = HTTPproc.OpenRead(strRequest, "keyName=" + strKeyName + "&requestSig=" + strSig + "&data={\"desc_id\":\"" + fId + "\"}");
                    strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;

                    JObject jsonR = JObject.Parse(strContent);
                    if ((int)jsonR["result"] == 0 && jsonR["info"] != null && jsonR["info"]["player_info"] != null)
                    {
                        //��jie�ɹ���   
                        //log(jsonR);   
                        ShowSysLog("�û��ٺ��ѣ�" + data + "�ɹ���");
                    }
                    else
                    {
                        ShowSysLog("�û��٣�" + data + "ʧ�ܣ�");
                    }
                    //�����Լ���ս��������                    
                    if (o["info"]["enter_scene"]["master_info"].Type.ToString() == "Object")
                    {
                        JObject jsonMaster = (JObject)o["info"]["enter_scene"]["master_info"];
                        User master = new User();
                        master.updateNoTime(jsonMaster);
                        if (u.getForce() > master.getForce())
                        {
                            //TODO Ӫ��                  
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
                ShowSysLog("��ȡ���ѣ�" + fId + "ʱ����������");
            }
        }
        #endregion ���ʺ��� visitFriend(string fId)

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

                t = pTime + 5 - Timestamp();//�õ��ܵĺ�����   
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

                t = pTime + 3 - Timestamp();//�õ��ܵĺ�����   
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

        #region �ƻ���������

        //��������
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

        //�������
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

        //͵ȡ����
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

        #endregion �ƻ���������

        #region ��ʾϵͳ��־ ShowSysLog(string strLog)
        /// <summary>
        /// ��ʾϵͳ��־
        /// </summary>
        /// <param name="strLog"></param>        
        private void ShowSysLog(string strLog)
        {
            DateTime dt_1 = DateTime.Now;
            textBox_Log.Text += dt_1 + "  " + strLog + "\r\n";
            textBox_Log.SelectionStart = textBox_Log.Text.Length;
            textBox_Log.ScrollToCaret();
            //ʼ����ʾTextBox����һ�У�ʼ�չ�������ײ�
            textBox_Log.SelectionStart = textBox_Log.Text.Length;
            textBox_Log.ScrollToCaret();
        }
        #endregion

        #region �õ�Sigֵ Sig(string strKey, string strKeyName)
        /// <summary>
        /// �õ�Sigֵ
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

            #region ����KeyName��ֵ
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
            #endregion ����KeyName��ֵ

            strKeyMD5_2 = UserMd5(UserMd5(strKey));
            strKeyMD5_2 = strKeyMD5_2.Substring(1, 6);
            strKeyMD5_2 = UserMd5(strKeyMD5_2);
            strKeyMD5_2 = strKeyMD5_2.Substring(0, 6);
            intkey = Int32.Parse(strKeyMD5_2, System.Globalization.NumberStyles.AllowHexSpecifier);
            intkey = intkey + intInc;
            strSig = UserMd5(Convert.ToString(intkey));

            return strSig;
        }
        #endregion �õ�Sigֵ Sig(string strKey, string strKeyName)

        #region MD5�����㷨 UserMd5(string str)
        /// <summary>
        /// MD5�����㷨
        /// </summary>
        /// <param name="str">�����ַ���</param>
        /// <returns>���32λСдMD5����ֵ</returns>
        static string UserMd5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//ʵ����һ��md5����  
            // ���ܺ���һ���ֽ����͵����飬����Ҫע�����UTF8/Unicode�ȵ�ѡ��  
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // ͨ��ʹ��ѭ�������ֽ����͵�����ת��Ϊ�ַ��������ַ����ǳ����ַ���ʽ������  
            for (int i = 0; i < s.Length; i++)
            {
                // ���õ����ַ���ʹ��ʮ���������͸�ʽ����ʽ����ַ���Сд����ĸ�����ʹ�ô�д��X�����ʽ����ַ��Ǵ�д�ַ�
                pwd = pwd + s[i].ToString("x2");
            }
            return pwd;
        }
        #endregion

        #region URL���� UrlEncode(string str, string encode)
        /// <summary>
        /// URL����
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
            //����Ҫ������ַ�

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.";
            System.Text.Encoder encoder = System.Text.Encoding.GetEncoding(encode).GetEncoder();
            char[] c1 = str.ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //һ���ַ�һ���ַ��ı���

            for (int i = 0; i < c1.Length; i++)
            {
                //����Ҫ����

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

        #region ��Unicodeת��ΪCharacter Unicode2Character(string str)
        /// <summary>
        /// ��Unicodeת��ΪCharacter
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
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

        #region ʱ��� Timestamp()
        /// <summary>
        /// ʱ���
        /// </summary>
        /// <returns></returns>
        private long Timestamp()
        {
            long longTimestamp = 0;
            longTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return longTimestamp;
        }
        #endregion ʱ��� Timestamp()

        #region ���������
        /// <summary>
        /// ���������
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

        #region ʱ���ʽ�� GetAllTime(long time)
        /// <summary>
        /// ʱ���ʽ��
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string GetAllTime(long time)
        {
            string hh, mm, ss;//, fff;

            //long f = time % 100; // ����   
            long s = time;// / 100; // ת��Ϊ��
            long m = (s / 60) % 60;     // ��
            long h = s / 3600;     // ʱ
            s = s % 60;     // �� 

            //�����ʽ00
            //if (f < 10)
            //{
            //    fff = "0" + f.ToString();
            //}
            //else
            //{
            //    fff = f.ToString();
            //}

            //���ʽ00
            if (s < 10)
            {
                ss = "0" + s.ToString();
            }
            else
            {
                ss = s.ToString();
            }

            //�ָ�ʽ00
            if (m < 10)
            {
                mm = "0" + m.ToString();
            }
            else
            {
                mm = m.ToString();
            }

            //ʱ��ʽ00
            if (h < 10)
            {
                hh = "0" + h.ToString();
            }
            else
            {
                hh = h.ToString();
            }

            //���� hh:mm:ss.ff            
            return hh + ":" + mm + ":" + ss;// +"." + fff;
        }
        #endregion ʱ���ʽ�� GetAllTime(long time)

        #region ��ʱ��
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

        #endregion ��ʱ��

        #region �����֤�û� CheckAuthUser(string strUserName)
        /// <summary>
        /// �����֤�û�
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

                ShowSysLog("�����ʺ�" + strUserName + "�Ѿ��ɹ����");
                ShowSysLog("��������δ��Ȩ�û����������ñ����20���ӡ�");
                this.boolAuth = false;
                string strText = this.Text.ToString().Trim();
                strText = strText.Replace("(δ��Ȩ)", "").Replace("(����Ȩ)", "");
                this.Text = strText + "  (δ��Ȩ)";
            }
            else if (dr != null)
            {
                boolStatus = (bool)dr["Status"];

                if (boolStatus == false)
                {
                    ShowSysLog("��������δ��Ȩ�û����������ñ����20���ӡ�");
                    this.boolAuth = false;
                    string strText = this.Text.ToString().Trim();
                    strText = strText.Replace("(δ��Ȩ)", "").Replace("(����Ȩ)", "");
                    this.Text = strText + "  (δ��Ȩ)";
                }
                else
                {
                    ShowSysLog("�𾴵Ļ�Ա" + strUserName + "��ӭ���ĵ�¼��");
                    this.boolAuth = true;
                    string strText = this.Text.ToString().Trim();
                    strText = strText.Replace("  (δ��Ȩ)", "").Replace("(����Ȩ)", "");
                    this.Text = strText + "  (����Ȩ)";
                }
            }
        }
        #endregion �����֤�û� CheckAuthUser(string strUserName)

        #region ����20���� TestUser()
        private void TestUser()
        {
            this.tu++;
            if (this.tu == 1200)
            {
                Application.Exit();
            }
        }
        #endregion ����20���� TestUser()
    }

    #region JSONConvert
    /// <summary>
    /// ��  ����JSONConvert
    /// ��  ����JSON������
    /// ��  д��dnawo
    /// վ  �㣺http://www.mzwu.com/
    /// ��  �ڣ�2010-01-06
    /// ��  ����1.1.0
    /// </summary>
    public static class JSONConvert
    {
        #region ȫ�ֱ���

        private static JSONObject _json = new JSONObject();//�Ĵ���
        private static readonly string _SEMICOLON = "@semicolon";//�ֺ�ת���
        private static readonly string _COMMA = "@comma"; //����ת���

        #endregion

        #region �ַ���ת��
        /// <summary>
        /// �ַ���ת��,��˫�����ڵ�:��,�ֱ�ת��_SEMICOLON��_COMMA
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
        /// �ַ���ת��,��_SEMICOLON��_COMMA�ֱ�ת��:��,
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string StrDecode(string text)
        {
            return text.Replace(_SEMICOLON, ":").Replace(_COMMA, ",");
        }

        #endregion

        #region JSON��С��Ԫ����

        /// <summary>
        /// ��С����תΪJSONObject
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
        /// ��С����תΪJSONArray
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
        /// �����л�
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string Deserialize(string text)
        {
            _json.Clear();//2010-01-20 ��ռĴ���
            text = StrEncode(text);//ת��;��,

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

        #region �����ӿ�

        /// <summary>
        /// �����л�JSONObject����
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static JSONObject DeserializeObject(string text)
        {
            return _json[Deserialize(text)] as JSONObject;
        }

        /// <summary>
        /// �����л�JSONArray����
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static JSONArray DeserializeArray(string text)
        {
            return _json[Deserialize(text)] as JSONArray;
        }

        /// <summary>
        /// ���л�JSONObject����
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
        /// ���л�JSONArray����
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
    /// ��  ����JSONObject
    /// ��  ����JSON������
    /// ��  д��dnawo
    /// վ  �㣺http://www.mzwu.com/
    /// ��  �ڣ�2010-01-06
    /// ��  ����1.1.0
    /// ������ʷ��
    ///     2010-01-06  �̳�Dictionary<TKey, TValue>����this[]
    /// </summary>
    public class JSONObject : Dictionary<string, object>
    { }

    /// <summary>
    /// ��  ����JSONArray
    /// ��  ����JSON������
    /// ��  д��dnawo
    /// վ  �㣺http://www.mzwu.com/
    /// ��  �ڣ�2010-01-06
    /// ��  ����1.1.0
    /// ������ʷ��
    ///     2010-01-06  �̳�List<T>����this[]
    /// </summary>
    public class JSONArray : List<object>
    { }
    #endregion JSONConvert
}