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

//Դ������ www.51aspx.com
namespace SysStartCheckUser
{
    /*��������ͨ�����ö�������ʵ�ֿ����Զ���������
     ��վ�˺���Ϣ www.woxp.cn ��Ҫע�� �����ʺ���Ϣ
     */
    public partial class Form1 : Form
    {
        private ArrayList al_session = new ArrayList();
        private string g_eid = "";
        private string g_pwd_md5 = "";
        private string g_uid = "";
        private string g_send_no = "";//
        private string g_find_user = "";
        private string g_sms_head = "δ֪����վ,";
        private string g_run_file = "";
        private int g_IsLog = 1;
        //�Ƿ��¼����״̬
        private int g_IsNetStatus = 0;
        private ArrayList al_sms = new ArrayList();
        private string XmlConfig = "config.xml";
        public ManualResetEvent ae_group_exit = new ManualResetEvent(false);
        private string logxml = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "login.log";
        private StreamWriter g_sw = null;
        //��ʱ��Ϣ�೤ʱ��
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
                    //�������
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
                    //��Ȩ���û�
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
        /// �����Զ����г���
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
                    //���������в���
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
                //��ȡ���ò���
                ReadXml();
                //������־
                if (g_IsLog == 1)
                {
                    g_sw = new StreamWriter(logxml, true);
                }
                else
                {
                    g_sw = null;
                }
                AddLog("ϵͳ�û���½���");//5-1-a--s-p-x
                //�źŲ���
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
            //������������
            Thread tr_check = null;
            //���Ͷ���
            Thread tr_sms = null;
            //��ͬ����ִ�в�ͬ����
            if (g_args != null && g_args.Length >= 1)
            {
                #region �ⲿ���ò���
                if (g_args.Length == 2 && g_args[0].ToString().Trim() == "-AutoRun")
                {
                    string au = g_args[1].ToString().Trim();
                    string g_ApplicationName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.ToString().Trim();
                    SetAutoRun(g_ApplicationName, au == "1" ? true : false);
                    //ֱ�ӹر�
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
                    //�ⲿ��������
                    if (msg.Length > 0)
                    {
                        SendSMS(msg);
                    }
                }
                else if (g_args.Length == 2 && g_args[0].ToString().Trim() == "-Time")
                {                    
                    //�೤ʱ����һ��
                    Int32.TryParse(g_args[1].ToString().Trim(), out g_nSleep);
                    if (g_nSleep <= 0)
                    {
                        g_nSleep = 60;
                    }
                    //�����߳�����
                    tr_check = new Thread(new ThreadStart(Thread_Monitoring));
                    tr_check.IsBackground = true;
                    tr_check.Start();
                    //��������(��ʱ�����ж���)
                    tr_sms = new Thread(new ThreadStart(Thread_SMS_While));
                    tr_sms.IsBackground = true;
                    tr_sms.Start();              
                }
                else if (g_args.Length == 1 && g_args[0].ToString().Trim() == "-ShutDown")
                {
                    string msg = "����ִ�йػ�������,ʱ��:" + DateTime.Now.ToString();
                    SendSMS(msg);
                }
                //���Ͷ���ֱ���ɹ�
                
                //��Ҫ��ʱ�����м��
                if (g_args.Length == 2 && g_args[0].ToString().Trim() == "-Time")
                {
                    //��������,ֱ��ϵͳ�ر�
                    tr_check.Join();
                    tr_sms.Join();
                }
                //��������Զ��ر�
                tr_sms = new Thread(new ThreadStart(Thread_SMS));
                tr_sms.IsBackground = true;
                tr_sms.Start();
                //ִ�йر�
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
                //��¼�Ự�û�������״̬
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
                        AddLog("ִ������ʧ��;ԭ��:" + ex.Message);
                    }
                }
                //����û�
                MonitoringSysUser();
                //���Ͷ���ֱ���ɹ�
                tr_sms = new Thread(new ThreadStart(Thread_SMS));
                tr_sms.IsBackground = true;
                tr_sms.Start();
                Thread.Sleep(10 * 1000);
                //������ɾ��˳�ϵͳ
                if (ae_group_exit.WaitOne())
                {
                    AddLog("���˺Ŷ���ʣ������:" + GetSMSCount() + ".(С��0�����ȡ������)");
                    AddLog("����û����.");
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
                //1���ӳ�ʱ
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
                AddLog("ִ�������ļ�:" + exefile + ",�쳣��ԭ��:" + ex.Message);
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

        //ϵͳ��½�û���Ϣ
        private class LoginUser
        {
            //��½�˺�
            public string UserId = "";
            //����״̬
            public string RunStatus = "";
            //�ỰID
            public int SessionId = -1;
            //��½ʱ��
            public string LoginTime = "";
            //�Ự��
            public string SessionName = "";
        }
        /// <summary>
        /// ����ֱ��ע��
        /// </summary>
        private void Fast_Loginoff()
        {    
            //ֱ�ӷ��ز�������
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
                //��ȡ�Ự��Ϣ
                TSControl.WTS_SESSION_INFO[] pSessionInfo = TSControl.SessionEnumeration();
                for (int i = 0; i < pSessionInfo.Length; i++)
                {
                    bool bsuccess = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, TSControl.WTSInfoClass.WTSUserName, out userId, out count);
                    if (bsuccess && userId.ToString().Trim() != "")
                    {
                        //AddLog("va:"+userId.ToString().Trim());
                        //�����ž�ע��
                        if(userId.ToString().Trim().ToLower()!=g_find_user.Trim().ToLower())
                        {
                            ulist += userId.ToString().Trim() + ",";
                            //ֱ��ע��
                            sys_Logoff(pSessionInfo[i].SessionID);
                        }                        
                    }
                    //AddLog("va:" + userId.ToString().Trim() + pSessionInfo.Length.ToString());
                }
            }
            catch (Exception ex)
            {
                AddLog("get_LoginUser�쳣,ԭ��:" + ex.Message);
            }
            try
            {
                if (ulist.Trim() != "")
                {
                    if (ulist.EndsWith(","))
                    {
                        ulist = ulist.Substring(0, ulist.Length - 1);
                    }
                    string smsMemo = "�����µ�½�û�:[" + ulist + "]ʱ��:" + DateTime.Now.ToString();
                    //���Ͷ���
                    SendSMS(smsMemo);
                }
            }
            catch(Exception ex)
            {
                AddLog("get_LoginUser_���ŷ����쳣,ԭ��:" + ex.Message);
            }
        }
        //����û�֮����|�ֿ�������ʾ�û��ͻỰID.�����ն˷Ƿ��û�
        private string  get_LoginUser()
        {
            string ret = "";
            //���
            al_session.Clear();
            try
            {
                int count = 0;
                IntPtr buffer = IntPtr.Zero;
                StringBuilder userId = new StringBuilder();
                //��ȡ�Ự��Ϣ
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
                        //����Ự��¼
                        al_session.Add(lu);
                        //����Ȩ�û�,��¼���е�½�����������˺���
                        ret += lu.UserId.ToString().Trim() + ",";
                        
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog("get_LoginUser�쳣,ԭ��:" + ex.Message);
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
        //��ʱ�����в��˳�
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
                            //��Ϣһ�����ύ
                            Thread.Sleep(1000 * 2);
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    AddLog("��Ϣת���쳣:" + ex.Message);
                }
                //��Ϣ
                Thread.Sleep(1000 * 30);
            }            
        }
        //�����߳�.���ڷ���ʧ�ܵ�.���²���
        private void Thread_SMS()
        {
            //���޺���˳�
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
                            //��Ϣһ�����ύ
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
                    AddLog("��Ϣת���쳣:" + ex.Message);
                }
                //��Ϣ
                Thread.Sleep(1000 * 15);
            }
            ae_group_exit.Set();
        }

        /// <summary>
        /// ע���ỰID-����ע��
        /// </summary>
        /// <param name="nSessionId"></param>
        private void sys_Logoff(int nSessionId)
        {
            try
            {
                try
                {
                    //����ע��
                    TSControl.WTSLogoffSession(0, nSessionId, false);
                }
                catch
                {
                }
                //����ע��
                GetCmd("logoff", nSessionId.ToString());
                //��¼ע����Ϣ
                AddLog("�ỰID:" + nSessionId.ToString() + "��ע��.");
                //����ע������
                SendSMS("�ỰID:" + nSessionId.ToString() + "ִ��ע��.ʱ��:" + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                AddLog("ע����Ա:" + nSessionId.ToString() + "�쳣,ԭ��:" + ex.Message);
            }
        }

        /// <summary>
        /// ��ѯ���ж���������
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
            //���϶���ͷ
            string NewMsg = g_sms_head + Msg;
            try
            {
                //������ŷ���
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
                    //ʧ�ܾ��ٴη���
                    if (nRet <= 0)
                    {
                        //ʧ��ԭ���¼
                        AddLog(tourl + ",����ֵ:" + ret);
                        al_sms.Add(tourl);
                    }
                }
                //д�뷢�Ͷ��ŵ���־;
                AddLog(NewMsg);
            }
            catch (Exception ex)
            {
                AddLog("ִ��SUB_SMS:,�쳣��ԭ��:" + ex.Message);
            }
        }

        //��ʱ������
        private void Thread_Monitoring()
        {
            if (g_nSleep <= 0)
            {
                g_nSleep = 60;
            }
            while (true)
            {
                //����һ�£��зǷ��ʻ����ս���
                Fast_Loginoff();
                //��Ϣһ��
                Thread.Sleep(1000 * g_nSleep);
            }
        }
        //�������ʺŵ�½
        private void MonitoringSysUser()
        {
            string strUser = g_find_user;
            //�˺�������
            if (strUser == null || strUser.Trim() == "")
            {
                return;
            }
            //��ȡ��ǰ�ѵ�½�û�
            string strAllUser=get_LoginUser();
            //��û���û���½,�˳�ϵͳ
            if (strAllUser.Trim() == "")
            {
                return;
            }
            //û�з�������û�����һ�ε�½
            string strLoginUser = "";
            //string temp = "";
            string smsMemo = "";
            try
            {
                //-1����ֻ���������ѣ���ִ��ע������
                if (g_find_user.Trim() != "-1")
                {
                    #region ѭ�������û��������ϵľ�ע��
                    for (int i = 0; i < al_session.Count; i++)
                    {
                        try
                        {
                            LoginUser lu = (LoginUser)al_session[i];
                            strLoginUser = lu.UserId;
                            //�����Ͼ�ע��
                            if (strLoginUser.ToLower().Trim() != strUser.ToLower().Trim())
                            {
                                //��־��¼
                                AddLog("ע������:��½�û���:" + lu.UserId + ",״̬:" + lu.RunStatus + ",�ỰID:" + lu.SessionId);
                                //ִ��ע���Ự����
                                sys_Logoff(lu.SessionId);
                            }
                        }
                        catch (Exception ex)
                        {
                            AddLog("�������û�:��" + strLoginUser + "���쳣,ԭ��:" + ex.Message);
                        }
                    }
                    #endregion
                }
                //���е��û���
                if (strAllUser.Trim() != "")
                {
                    smsMemo = "���ֵ�½�û�:[" + strAllUser + "]ʱ��:" + DateTime.Now.ToString();
                    //���Ͷ��š��ȷ�������ע��
                    SendSMS(smsMemo);
                }
            }
            catch (Exception ex)
            {
                AddLog("�������û����������쳣,ԭ��:" + ex.Message);
            }
        }
    }
}