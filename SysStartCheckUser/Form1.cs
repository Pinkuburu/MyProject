using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Xml;
using Microsoft.Win32;

//源码下载 www.51aspx.com
namespace SysStartCheckUser
{
    /*本报警器通过调用短信网关实现开机自动报警功能
     网站账号信息 www.woxp.cn 需要注册 配置帐号信息
     */
    public partial class Form1 : Form
    {
        private ArrayList al_session = new ArrayList();
        private string g_eid = "";
        private string g_pwd_md5 = "";
        private string g_uid = "";
        private string g_send_no = "";//
        private string g_find_user = "";
        private string g_sms_head = "未知工作站,";
        private string g_run_file = "";
        private int g_IsLog = 1;
        //是否记录网络状态
        private int g_IsNetStatus = 0;
        private ArrayList al_sms = new ArrayList();
        private string XmlConfig = "config.xml";
        public ManualResetEvent ae_group_exit = new ManualResetEvent(false);
        private string logxml = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "login.log";
        private StreamWriter g_sw = null;
        //定时休息多长时间
        private int g_nSleep = 60;
        private string[] g_args = null;

        public Form1(string[] args)
        {
            g_args = args;
            InitializeComponent();
        }


        private void AddLog(string log)
        {
            try
            {
                if (g_IsLog==1 && g_sw != null)
                {
                    g_sw.WriteLine("[" + DateTime.Now.ToString() + "]" + log);
                    g_sw.Flush();
                }
            }
            catch
            {
                //Application.Exit();
            }
        }
        
        private void ReadXml()
        {
            string xmlpath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + XmlConfig;
            if (File.Exists(xmlpath))
            {
                try
                {
                    XmlDocument xd = new XmlDocument();
                    xd.Load(xmlpath);
                    XmlNode from = xd.SelectSingleNode("/System_Config");
                    //窗体参数
                    if(from["s_eid"]!=null)
                    {
                        g_eid = from["s_eid"].InnerText.ToString().Trim();
                    }
                    if(from["s_uid"]!=null)
                    {
                        g_uid = from["s_uid"].InnerText.ToString().Trim();
                    }
                    if (from["s_pwd_md5"] != null)
                    {
                        g_pwd_md5 = from["s_pwd_md5"].InnerText.ToString().Trim();
                    }
                    if (from["s_cardno"] != null && from["s_cardno"].InnerText.ToString().Trim() != "")
                    {
                        g_send_no = from["s_cardno"].InnerText.ToString().Trim();
                    }
                    if (from["s_sms_head"] != null && from["s_sms_head"].InnerText.ToString().Trim() != "")
                    {
                        g_sms_head = from["s_sms_head"].InnerText.ToString().Trim();
                    }
                    //授权的用户
                    if (from["s_System_User"] != null && from["s_System_User"].InnerText.ToString().Trim() != "")
                    {
                        g_find_user = from["s_System_User"].InnerText.ToString().Trim();
                    }
                    if (from["g_log"] != null)
                    {
                        Int32.TryParse(from["g_log"].InnerText.ToString().Trim(), out g_IsLog);
                    }
                    if (from["g_net_log"] != null)
                    {
                        Int32.TryParse(from["g_net_log"].InnerText.ToString().Trim(), out g_IsNetStatus);
                    }
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// 开机自动运行程序
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="isAutoRun"></param>
        public bool SetAutoRun(string fileName, bool isAutoRun)
        {
            RegistryKey reg = null;
            try
            {
                if (!System.IO.File.Exists(fileName))
                {
                    return false;
                }
                String name = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (reg == null)
                {
                    reg = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                }
                if (isAutoRun)
                {
                    //加上命令行参数
                    reg.SetValue(name, string.Format("{0}", fileName));
                }
                else
                {
                    reg.DeleteValue(name);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }
            return true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {                
                //提取配置参数
                ReadXml();
                //允许日志
                if (g_IsLog == 1)
                {
                    g_sw = new StreamWriter(logxml, true);
                }
                else
                {
                    g_sw = null;
                }
                AddLog("系统用户登陆检测");//5-1-a--s-p-x
                //信号不发
                ae_group_exit.Reset();
                al_session.Clear();
                this.Visible = false;
                this.Hide();
                this.ShowInTaskbar = false;
            }
            catch
            {
                this.Close();                
                Application.Exit();
                return;
            }
            //服务器上运行
            Thread tr_check = null;
            //发送短信
            Thread tr_sms = null;
            //不同参数执行不同命令
            if (g_args != null && g_args.Length >= 1)
            {
                #region 外部调用参数
                if (g_args.Length == 2 && g_args[0].ToString().Trim() == "-AutoRun")
                {
                    string au = g_args[1].ToString().Trim();
                    string g_ApplicationName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.ToString().Trim();
                    SetAutoRun(g_ApplicationName, au == "1" ? true : false);
                    //直接关闭
                    this.Close();
                    Application.Exit();
                    return;
                }
                else if (g_args.Length == 2 && g_args[0].ToString().Trim()=="-SendMsg")
                {
                    g_sms_head = "";
                    string msg = g_args[1].ToString().Trim();
                    if(msg.Length>70)
                    {
                        msg =msg.Substring(0,70);
                    }
                    //外部发送命令
                    if (msg.Length > 0)
                    {
                        SendSMS(msg);
                    }
                }
                else if (g_args.Length == 2 && g_args[0].ToString().Trim() == "-Time")
                {                    
                    //多长时间检测一次
                    Int32.TryParse(g_args[1].ToString().Trim(), out g_nSleep);
                    if (g_nSleep <= 0)
                    {
                        g_nSleep = 60;
                    }
                    //激活线程运行
                    tr_check = new Thread(new ThreadStart(Thread_Monitoring));
                    tr_check.IsBackground = true;
                    tr_check.Start();
                    //短信运行(长时间运行短信)
                    tr_sms = new Thread(new ThreadStart(Thread_SMS_While));
                    tr_sms.IsBackground = true;
                    tr_sms.Start();              
                }
                else if (g_args.Length == 1 && g_args[0].ToString().Trim() == "-ShutDown")
                {
                    string msg = "正在执行关机或重启,时间:" + DateTime.Now.ToString();
                    SendSMS(msg);
                }
                //发送短信直到成功
                
                //需要长时间运行监控
                if (g_args.Length == 2 && g_args[0].ToString().Trim() == "-Time")
                {
                    //长期运行,直到系统关闭
                    tr_check.Join();
                    tr_sms.Join();
                }
                //运行完就自动关闭
                tr_sms = new Thread(new ThreadStart(Thread_SMS));
                tr_sms.IsBackground = true;
                tr_sms.Start();
                //执行关闭
                if (ae_group_exit.WaitOne())
                {
                    this.Close();
                    Application.Exit();
                    return;
                }
                #endregion
            }
            try
            {                               
                //记录会话用户和网络状态
                if (g_IsNetStatus == 1)
                {
                    try
                    {
                        StreamReader sr = GetCmd("netstat.exe", "-aon");
                        if (sr != null)
                        {
                            AddLog(sr.ReadToEnd());
                        }
                        sr = GetCmd("quser.exe", "");
                        if (sr != null)
                        {
                            AddLog(sr.ReadToEnd());
                        }
                    }
                    catch (Exception ex)
                    {
                        AddLog("执行命令失败;原因:" + ex.Message);
                    }
                }
                //检测用户
                MonitoringSysUser();
                //发送短信直到成功
                tr_sms = new Thread(new ThreadStart(Thread_SMS));
                tr_sms.IsBackground = true;
                tr_sms.Start();
                Thread.Sleep(10 * 1000);
                //发送完成就退出系统
                if (ae_group_exit.WaitOne())
                {
                    AddLog("该账号短信剩余条数:" + GetSMSCount() + ".(小于0代表获取余额错误)");
                    AddLog("检测用户完成.");
                    if (g_sw != null)
                    {
                        g_sw.Close();
                    }
                    this.Close();
                    Application.Exit();                    
                }
            }
            catch
            {
            }
        }
        public string GET_URL(string url, Encoding ReponseEnding, string refererUrl)
        {
            string strRet = "";
            try
            {
                if (url == null)
                {
                    return strRet;
                }
                string targeturl = url.Trim().ToString();
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                if (refererUrl != null)
                {
                    hr.Referer = refererUrl.Trim();
                }
                hr.UserAgent = "Mozilla/4.0   (compatible;   MSIE   6.0;   Windows   NT   5.1)";
                hr.Method = "GET";
                //1分钟超时
                hr.Timeout = 100 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, ReponseEnding == null ? Encoding.Default : ReponseEnding);
                strRet = ser.ReadToEnd();
            }
            catch (Exception ex)
            {
                strRet = ex.Message;
            }
            return strRet;
        }
    
        private StreamReader GetCmd(string exefile, string arg)
        {
            StreamReader sr = null;
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = exefile;
                p.StartInfo.Arguments = arg;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WorkingDirectory = Path.GetDirectoryName(exefile);
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.Start();
                //p.WaitForExit();
                sr = p.StandardOutput;
            }
            catch (Exception ex)
            {
                AddLog("执行命令文件:" + exefile + ",异常。原因:" + ex.Message);
                sr = null;
            }
            return sr;
        }
        private string ReplaceBlank(string cmdline)
        {
            StringBuilder sb = new StringBuilder();
            char[] cb = cmdline.Trim().ToCharArray();
            bool bFirst = true;
            foreach (char bbc in cb)
            {
                if (bbc == ' ')
                {
                    if (!bFirst)
                    {
                        continue;//5+1+a+s+p+x
                    }
                    bFirst = false;
                    sb.Append("|");
                }
                else
                {
                    bFirst = true;
                    sb.Append(bbc);
                }
            }
            return sb.ToString();
        }

        //系统登陆用户信息
        private class LoginUser
        {
            //登陆账号
            public string UserId = "";
            //运行状态
            public string RunStatus = "";
            //会话ID
            public int SessionId = -1;
            //登陆时间
            public string LoginTime = "";
            //会话名
            public string SessionName = "";
        }
        /// <summary>
        /// 快速直接注销
        /// </summary>
        private void Fast_Loginoff()
        {    
            //直接返回参数不对
            if(g_find_user==null || g_find_user.Trim()=="")
            {
                return;
            }
            string ulist = "";
            try
            {
                int count = 0;
                IntPtr buffer = IntPtr.Zero;
                StringBuilder userId = new StringBuilder();
                //提取会话信息
                TSControl.WTS_SESSION_INFO[] pSessionInfo = TSControl.SessionEnumeration();
                for (int i = 0; i < pSessionInfo.Length; i++)
                {
                    bool bsuccess = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, TSControl.WTSInfoClass.WTSUserName, out userId, out count);
                    if (bsuccess && userId.ToString().Trim() != "")
                    {
                        //AddLog("va:"+userId.ToString().Trim());
                        //不符号就注销
                        if(userId.ToString().Trim().ToLower()!=g_find_user.Trim().ToLower())
                        {
                            ulist += userId.ToString().Trim() + ",";
                            //直接注销
                            sys_Logoff(pSessionInfo[i].SessionID);
                        }                        
                    }
                    //AddLog("va:" + userId.ToString().Trim() + pSessionInfo.Length.ToString());
                }
            }
            catch (Exception ex)
            {
                AddLog("get_LoginUser异常,原因:" + ex.Message);
            }
            try
            {
                if (ulist.Trim() != "")
                {
                    if (ulist.EndsWith(","))
                    {
                        ulist = ulist.Substring(0, ulist.Length - 1);
                    }
                    string smsMemo = "发现新登陆用户:[" + ulist + "]时间:" + DateTime.Now.ToString();
                    //发送短信
                    SendSMS(smsMemo);
                }
            }
            catch(Exception ex)
            {
                AddLog("get_LoginUser_短信发送异常,原因:" + ex.Message);
            }
        }
        //多个用户之间用|分开必须显示用户和会话ID.可以终端非法用户
        private string  get_LoginUser()
        {
            string ret = "";
            //清空
            al_session.Clear();
            try
            {
                int count = 0;
                IntPtr buffer = IntPtr.Zero;
                StringBuilder userId = new StringBuilder();
                //提取会话信息
                TSControl.WTS_SESSION_INFO[] pSessionInfo = TSControl.SessionEnumeration();
                for (int i = 0; i < pSessionInfo.Length; i++)
                {
                    bool bsuccess = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, TSControl.WTSInfoClass.WTSUserName, out userId, out count);
                    if (bsuccess && userId.ToString().Trim() != "")
                    {
                        LoginUser lu = new LoginUser();
                        lu.RunStatus = pSessionInfo[i].state.ToString();
                        lu.UserId = userId.ToString().Trim();
                        lu.SessionId = pSessionInfo[i].SessionID;
                        lu.SessionName = pSessionInfo[i].pWinStationName;
                        //加入会话记录
                        al_session.Add(lu);
                        //非授权用户,记录所有登陆到本机器的账号名
                        ret += lu.UserId.ToString().Trim() + ",";
                        
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog("get_LoginUser异常,原因:" + ex.Message);
            }
            if (ret.EndsWith(","))
            {
                ret = ret.Substring(0, ret.Length - 1);
            }
            return ret;
        }
        public string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }
            return (sb.ToString());
        }
        //长时间运行不退出
        private void Thread_SMS_While()
        {            
            int nResult = 0;
            while (true)
            {
                try
                {
                    if (al_sms != null && al_sms.Count > 0)
                    {
                        for (int i = 0; i < al_sms.Count; i++)
                        {
                            Int32.TryParse(GET_URL(al_sms[i].ToString().Trim(), null, null), out nResult);
                            if (nResult > 0)
                            {
                                al_sms.RemoveAt(i);
                            }
                            //休息一下再提交
                            Thread.Sleep(1000 * 2);
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    AddLog("信息转发异常:" + ex.Message);
                }
                //休息
                Thread.Sleep(1000 * 30);
            }            
        }
        //发送线程.对于发送失败的.重新操作
        private void Thread_SMS()
        {
            //上限后就退出
            //int exitCount = 0;
            int nResult = 0;
            while (true)
            {
                try
                {
                    if (al_sms != null && al_sms.Count > 0)
                    {
                        for (int i = 0; i < al_sms.Count; i++)
                        {
                            Int32.TryParse(GET_URL(al_sms[i].ToString().Trim(), null, null), out nResult);
                            if (nResult > 0)
                            {
                                al_sms.RemoveAt(i);
                            }
                            //休息一下再提交
                            Thread.Sleep(1000*2);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    AddLog("信息转发异常:" + ex.Message);
                }
                //休息
                Thread.Sleep(1000 * 15);
            }
            ae_group_exit.Set();
        }

        /// <summary>
        /// 注销会话ID-必须注销
        /// </summary>
        /// <param name="nSessionId"></param>
        private void sys_Logoff(int nSessionId)
        {
            try
            {
                try
                {
                    //立即注销
                    TSControl.WTSLogoffSession(0, nSessionId, false);
                }
                catch
                {
                }
                //二次注销
                GetCmd("logoff", nSessionId.ToString());
                //记录注销信息
                AddLog("会话ID:" + nSessionId.ToString() + "被注销.");
                //发送注销短信
                SendSMS("会话ID:" + nSessionId.ToString() + "执行注销.时间:" + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                AddLog("注销会员:" + nSessionId.ToString() + "异常,原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 查询还有多少条短信
        /// </summary>
        /// <returns></returns>
        private string GetSMSCount()
        {
            string ret = "0";
            string url = "http://api.woxp.cn/utf8/web_api/?x_eid={0}&x_uid={1}&x_pwd_md5={2}&x_ac=31&x_gate_id=300";
            try
            {
                string tourl = string.Format(url, g_eid, g_uid, g_pwd_md5);
                ret = GET_URL(tourl, null, null);
            }
            catch
            {
                ret = "-1";
            }
            return ret;
        }

        private void SendSMS(string Msg)
        {            
            string url = "";
            string tourl = "";
            int nRet = 0;
            //加上短信头
            string NewMsg = g_sms_head + Msg;
            try
            {
                //激活短信发送
                if (g_uid.Trim() != "" && g_eid.Trim() != "" && g_pwd_md5.Trim() != "" && g_send_no.Trim() !="")
                {
                    if (NewMsg.Length > 70)
                    {
                        NewMsg = NewMsg.Substring(0, 70);
                    }
                    url = "http://api.woxp.cn/utf8/web_api/?x_eid={0}&x_uid={1}&x_pwd_md5={2}&x_ac=10&x_gate_id=300&x_target_no={3}&x_memo={4}";
                    tourl = string.Format(url, g_eid, g_uid, g_pwd_md5, g_send_no, UrlEncode(NewMsg));
                    string ret = GET_URL(tourl, null, null);
                    Int32.TryParse(ret, out nRet);
                    //失败就再次发送
                    if (nRet <= 0)
                    {
                        //失败原因记录
                        AddLog(tourl + ",返回值:" + ret);
                        al_sms.Add(tourl);
                    }
                }
                //写入发送短信的日志;
                AddLog(NewMsg);
            }
            catch (Exception ex)
            {
                AddLog("执行SUB_SMS:,异常。原因:" + ex.Message);
            }
        }

        //长时间运行
        private void Thread_Monitoring()
        {
            if (g_nSleep <= 0)
            {
                g_nSleep = 60;
            }
            while (true)
            {
                //监视一下，有非法帐户就终结它
                Fast_Loginoff();
                //休息一下
                Thread.Sleep(1000 * g_nSleep);
            }
        }
        //监视新帐号登陆
        private void MonitoringSysUser()
        {
            string strUser = g_find_user;
            //账号名错误
            if (strUser == null || strUser.Trim() == "")
            {
                return;
            }
            //提取当前已登陆用户
            string strAllUser=get_LoginUser();
            //还没有用户登陆,退出系统
            if (strAllUser.Trim() == "")
            {
                return;
            }
            //没有发现这个用户。第一次登陆
            string strLoginUser = "";
            //string temp = "";
            string smsMemo = "";
            try
            {
                //-1代表只发短信提醒，不执行注销操作
                if (g_find_user.Trim() != "-1")
                {
                    #region 循环查找用户，不符合的就注销
                    for (int i = 0; i < al_session.Count; i++)
                    {
                        try
                        {
                            LoginUser lu = (LoginUser)al_session[i];
                            strLoginUser = lu.UserId;
                            //不符合就注销
                            if (strLoginUser.ToLower().Trim() != strUser.ToLower().Trim())
                            {
                                //日志记录
                                AddLog("注销命令:登陆用户名:" + lu.UserId + ",状态:" + lu.RunStatus + ",会话ID:" + lu.SessionId);
                                //执行注销会话命令
                                sys_Logoff(lu.SessionId);
                            }
                        }
                        catch (Exception ex)
                        {
                            AddLog("监视新用户:【" + strLoginUser + "】异常,原因:" + ex.Message);
                        }
                    }
                    #endregion
                }
                //所有的用户名
                if (strAllUser.Trim() != "")
                {
                    smsMemo = "发现登陆用户:[" + strAllUser + "]时间:" + DateTime.Now.ToString();
                    //发送短信。先发短信再注销
                    SendSMS(smsMemo);
                }
            }
            catch (Exception ex)
            {
                AddLog("监视新用户整个过程异常,原因:" + ex.Message);
            }
        }
    }
}