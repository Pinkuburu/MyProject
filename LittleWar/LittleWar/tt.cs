using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Threading;

namespace LittleWar
{
    public partial class tt : Form
    {
        string strRequest = null;
        string strContent = null;
        string resultString = null;
        string strSig = null;
        string strKey = null;
        string strKeyName = null;
        string userRun = null;
        string str_inuId = null;
        int intUid = 0;
        int apiCount = 0;

        Thread xiaoxiaofoodThread = null;
        Thread xiaoxiaoforceThread = null;
        private delegate void _delegateRefreshserverInfo(string strServerInfo);

        WebClient HTTPproc = new WebClient();

        public tt()
        {
            InitializeComponent();
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            Login_Xiaonei("lancelot11@126.com", "123456789");
            Login_Game();
            //gainAndProduceFood(userRun, 1);
            //gainAndProduceForce(userRun, 1);
            StartfoodThread();
            StartforceThread();
        }

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
            //string str_inuId = null;

            string[] arrKeyWords;

            ShowSysLog("���ڵ�¼��Ϸ...");

            strRequest = "http://apps.renren.com/littlewar";
            strContent = HTTPproc.OpenRead(strRequest);

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

            strContent = HTTPproc.OpenRead("http://www.connect.renren.com/login_status.do?api_key=" + xn_sig_api_key + "%20&extern=false&channel=http%3A%2F%2Fxn.lw.fminutes.com%2Fxd_receiver.html");
            try
            {
                resultString = Regex.Match(strContent, @"\{""uid"".*\}").Value;

                //{"uid":228618602,"session_key":"3.a04d712afbce904e54ceb4ecd822deea.21600.1287730800-228618602","secret":"589acb9b16c300b66d25c9f748d7933f","expires":1287730800,"base_domain":"fminutes.com","sig":"459070f88da5fe42ce43fd11602a6400"}
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

            //resultString = HTTPproc.OpenRead("http://api.renren.com/restserver.do", "uids=" + xn_sig_user + "&fields=uid%2Cbirthday%2Csex%2Chometown_location&method=users.getInfo&api_key=" + xn_sig_api_key + "+&format=JSON&call_id=1287705906085&v=1.0&session_key=" + xn_sig_session_key + "&xn_ss=1&sig=" + xn_sig);
            //HTTPproc.RequestHeaders.Add("Referer:http://api.renren.com/static/client_restserver.html?r=10000");
            //resultString = HTTPproc.OpenRead("http://api.renren.com/restserver.do", "method=friends.get&api_key=" + xn_sig_api_key + "%20&format=JSON&call_id=1287705906629&v=1.0&session_key=" + xn_sig_session_key + "&xn_ss=1&sig=" + xn_sig);

            //xn_sig_api_key=str_sig;
            //xn_sig_api_key _expires=str_expires
            //xn_sig_api_key _session_key=str_session_key
            //xn_sig_api_key _ss=str_secret
            //xn_sig_api_key _user=str_uid
            //base_domain_xn_sig_api_key=str_base_domain
            //xnsetting_xn_sig_api_key={"connectState" = 1,"oneLineStorySetting" = 1,"shortStorySetting" = 1,"shareAuth" = null}
            HTTPproc = new WebClient();

            //string strTemp = xn_sig_api_key + "=" + str_sig
            HTTPproc.RequestHeaders.Add(xn_sig_api_key + ":" + str_sig);
            HTTPproc.RequestHeaders.Add(xn_sig_api_key + "_expires:" + str_expires);
            HTTPproc.RequestHeaders.Add(xn_sig_api_key + "_session_key:" + str_session_key);
            HTTPproc.RequestHeaders.Add(xn_sig_api_key + "_ss:" + str_secret);
            HTTPproc.RequestHeaders.Add(xn_sig_api_key + "_user:" + str_uid);
            HTTPproc.RequestHeaders.Add("base_domain_" + xn_sig_api_key + ":" + str_base_domain);
            HTTPproc.RequestHeaders.Add("xnsetting_" + xn_sig_api_key + ":{\"connectState\" = 1,\"oneLineStorySetting\" = 1,\"shortStorySetting\" = 1,\"shareAuth\" = null}");

            //strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Friend.run";
            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Scene.init";
            strContent = Unicode2Character(HTTPproc.OpenRead(strRequest));

            try
            {
                resultString = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;
                JObject o = JObject.Parse(Unicode2Character(resultString));
                strKey = (string)o["info"]["getKey"]["key"];
                strKeyName = (string)o["info"]["getKey"]["keyName"];
                intUid = (int)o["info"]["uid"];
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            strSig = Sig(strKey, strKeyName);

            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Scene.run";
            userRun = Unicode2Character(HTTPproc.OpenRead(strRequest, "data={\"fid\":" + intUid + "\"}&keyName=" + strKeyName + "&requestSig=" + strSig));
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
            userRun = Regex.Match(userRun.Replace("\r\n", ""), @"\{.*\}").Value;

            JObject o = JObject.Parse(userRun);//Unicode2Character(userRun)
            JObject ob = (JObject)o["info"]["enter_scene"]["build_info"]["build_list"];

            JSONObject info = JSONConvert.DeserializeObject(userRun);
            JSONObject build_list = ((JSONObject)((JSONObject)((JSONObject)((JSONObject)info["info"])["enter_scene"])["build_info"])["build_list"]);
            
            int intd = build_list.Count;
            ArrayList foodBuild = new ArrayList();

            string[] strKeys = new string[build_list.Keys.Count];
            build_list.Keys.CopyTo(strKeys, 0);
            for (int i = 0; i < strKeys.Length; i++)
            {
                JSONObject objBuild = (JSONObject)build_list[strKeys[i].ToString().Trim()];
                int intBuild_Id = Convert.ToInt32(objBuild["build_id"].ToString().Trim());
                if (intBuild_Id >= 30000 && intBuild_Id < 40000)
                {
                    foodBuild.Add(Convert.ToInt32(objBuild["id"].ToString().Trim()));

                    long st = Convert.ToInt64(objBuild["start_time"].ToString().Trim());
                    long et = Convert.ToInt64(objBuild["end_time"].ToString().Trim());

                    if (st == 0)
                    {
                        string data = "{\"produce_id\":" + intFoodID + ",\"id\":" + Convert.ToInt32(objBuild["id"].ToString().Trim()) + "}";
                        produceFood(data);
                        //RefeshfoodInfo(produceFood(data));
                    }
                }
            }

            //����ʳ���б����ĸ����ջ�� 
            JArray foodBuildIds = new JArray();
            for (int j = 0; j < foodBuild.Count; j++)
            {
                foodBuildIds.Add(foodBuild[j]);
            }
            //�ջ�����ʳ��
            for (int k = 0; k < strKeys.Length; k++)
            {
                JSONObject objBuild = (JSONObject)build_list[strKeys[k].ToString().Trim()];
                int intBuild_Id = Convert.ToInt32(objBuild["build_id"].ToString().Trim());
                if (intBuild_Id >= 30000 && intBuild_Id < 40000)
                {
                    long st = Convert.ToInt64(objBuild["start_time"].ToString().Trim());
                    long et = Convert.ToInt64(objBuild["end_time"].ToString().Trim());
                    if (et != 0 && et <= Timestamp())
                    {
                        //�����ջ���    
                        String data = "{\"ids\":" + foodBuildIds.ToString().Replace("\r\n", "").Replace("  ", "") + ",\"id\":" + Convert.ToInt32(objBuild["id"].ToString().Trim()) + "}";
                        gainFood(data);
                        //RefeshfoodInfo(gainFood(data));
                        //�ջ���������
                        data = "{\"produce_id\":" + intFoodID + ",\"id\":" + Convert.ToInt32(objBuild["id"].ToString().Trim()) + "}";
                        produceFood(data);
                        //RefeshfoodInfo(produceFood(data));
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
        private string produceFood(string data)
        {
            checkApi();
            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Produce.recruiteSoldier";
            ShowSysLog(data);
            strContent = Unicode2Character(HTTPproc.OpenRead(strRequest, "requestSig=" + strSig + "&data=" + data + "&keyName=" + strKeyName));
            strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;

            JObject o = JObject.Parse(Unicode2Character(strContent));
            if ((int)o["result"] == 0 && o["info"] != null && o["info"]["player_info"] != null)
            {
                //�����ɹ���   
                //ShowSysLog("����ʳ����build��" + data + "�ɹ���");
                RefeshServerInfo("����ʳ����build��" + data + "�ɹ���");
            }
            else
            {
                //ShowSysLog("����ʳ����build��" + data + "ʧ�ܣ�");
                RefeshServerInfo("����ʳ����build��" + data + "ʧ�ܣ�");
            }
            return strContent;
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
            checkApi();
            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Produce.gainFood";
            ShowSysLog(data);
            strContent = Unicode2Character(HTTPproc.OpenRead(strRequest, "requestSig=" + strSig + "&data=" + data + "&keyName=" + strKeyName));
            strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;

            JObject o = JObject.Parse(Unicode2Character(strContent));
            if ((int)o["result"] == 0 && o["info"] != null && o["info"]["player_info"] != null)
            {
                //ShowSysLog("�ջ�ʳ����build��" + data + "�ɹ���");
                RefeshServerInfo("�ջ�ʳ����build��" + data + "�ɹ���");
            }
            else
            {
                //ShowSysLog("�ջ�ʳ����build��" + data + "ʧ�ܣ�");
                RefeshServerInfo("�ջ�ʳ����build��" + data + "ʧ�ܣ�");
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
            userRun = Regex.Match(userRun.Replace("\r\n", ""), @"\{.*\}").Value;

            JSONObject info = JSONConvert.DeserializeObject(userRun);
            JSONObject build_list = ((JSONObject)((JSONObject)((JSONObject)((JSONObject)info["info"])["enter_scene"])["build_info"])["build_list"]);

            int intd = build_list.Count;
            ArrayList foodBuild = new ArrayList();

            string[] strKeys = new string[build_list.Keys.Count];
            build_list.Keys.CopyTo(strKeys, 0);
            for (int i = 0; i < strKeys.Length; i++)
            {
                JSONObject objBuild = (JSONObject)build_list[strKeys[i].ToString().Trim()];
                int intBuild_Id = Convert.ToInt32(objBuild["build_id"].ToString().Trim());
                if (intBuild_Id >= 30000 && intBuild_Id < 40000)
                {
                    //�����ʳ��
                }
                else if (intBuild_Id >= 40000 && intBuild_Id < 50000)
                {
                    //������
                    long st = Convert.ToInt64(objBuild["start_time"].ToString().Trim());
                    long et = Convert.ToInt64(objBuild["end_time"].ToString().Trim());

                    if (st == 0)
                    {
                        //�յ� ������ֱ��������
                        string data = "{\"produce_id\":" + intForceID + ",\"id\":" + Convert.ToInt32(objBuild["id"].ToString().Trim()) + "}";
                        produceForce(data);
                        //RefeshforceInfo(produceForce(data));
                    }
                    else if (et <= Timestamp())
                    {
                        //���ձ�����������
                        string data = "{\"id\":" + Convert.ToInt32(objBuild["id"].ToString().Trim()) + "}";
                        gainForce(data);
                        //RefeshforceInfo(gainForce(data));
                        data = "{\"produce_id\":" + intForceID + ",\"id\":" + Convert.ToInt32(objBuild["id"].ToString().Trim()) + "}";
                        produceForce(data);
                        //RefeshforceInfo(produceForce(data));
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
        private string produceForce(string data)
        {
            checkApi();
            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Produce.recruiteSoldier";
            ShowSysLog(data);
            strContent = Unicode2Character(HTTPproc.OpenRead(strRequest, "requestSig=" + strSig + "&data=" + data + "&keyName=" + strKeyName));
            strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;

            JObject o = JObject.Parse(Unicode2Character(strContent));
            if ((int)o["result"] == 0 && o["info"] != null && o["info"]["player_info"] != null)
            {
                //�����ɹ���   
                //ShowSysLog("����������build��" + data + "�ɹ���");
                RefeshServerInfo("����������build��" + data + "�ɹ���");
            }
            else
            {
                //ShowSysLog("����������build��" + data + "ʧ�ܣ�");
                RefeshServerInfo("����������build��" + data + "ʧ�ܣ�");
            }
            return strContent;
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
            checkApi();
            strRequest = "http://xnapi.lw.fminutes.com/api.php?inuId=" + str_inuId + "&method=Produce.gainRecruitment";
            ShowSysLog(data);
            strContent = Unicode2Character(HTTPproc.OpenRead(strRequest, "requestSig=" + strSig + "&data=" + data + "&keyName=" + strKeyName));
            strContent = Regex.Match(strContent.Replace("\r\n", ""), @"\{.*\}").Value;

            JObject o = JObject.Parse(Unicode2Character(strContent));
            if ((int)o["result"] == 0 && o["info"] != null && o["info"]["player_info"] != null)
            {
                //ShowSysLog("�ջ������build��" + data + "�ɹ���");
                RefeshServerInfo("�ջ������build��" + data + "�ɹ���");
            }
            else
            {
                //ShowSysLog("�ջ������build��" + data + "ʧ�ܣ�");
                RefeshServerInfo("�ջ������build��" + data + "ʧ�ܣ�");
            }
            return strContent;
        }
        #endregion �ջ���� gainForce(string data)

        #region ���APIʹ��ʱ�� checkApi()
        /// <summary>
        /// ���APIʹ��ʱ��
        /// </summary>
        private void checkApi()
        {
            apiCount++;
            if (apiCount > 100)
            {
                //��Ҫ���µ���userRun,��������sig   
                ShowSysLog("sig���ڣ����³�ʼ��");
                try
                {
                    Login_Game();
                }
                catch (Exception e)
                {
                    ShowSysLog(e.ToString());
                }
                apiCount = 0;
            }
        }
        #endregion ���APIʹ��ʱ�� checkApi()

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

        #region ˢ�´��������Ϣ
        #region ��ʳ���
        private void StartfoodThread()
        {
            xiaoxiaofoodThread = new Thread(new ThreadStart(foodThread));
            xiaoxiaofoodThread.Start();
        }

        private void foodThread()
        {
            gainAndProduceFood(userRun, 1);
        }
        #endregion


        #region ʿ�����
        private void StartforceThread()
        {
            xiaoxiaoforceThread = new Thread(new ThreadStart(forceThread));
            xiaoxiaoforceThread.Start();
        }

        private void forceThread()
        {
            gainAndProduceForce(userRun, 1);
        }
        #endregion

        private void RefeshServerInfo(string strInfo)
        {
            if (this.textBox_Log.InvokeRequired)
            {
                _delegateRefreshserverInfo rs = new _delegateRefreshserverInfo(RefeshServerInfo);
                this.textBox_Log.Invoke(rs, new object[] { strInfo });
            }
            else
                RefeshServerInfo(strInfo);
        }

        #endregion
    }
}