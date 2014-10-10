using System.Windows.Forms;
using System.IO;
using System;
using System.Threading;
using System.Diagnostics;

namespace Client
{
    public partial class Client : Form
    {
        //添加按钮
        MenuItem menuItem1 = new MenuItem("隐藏");
        MenuItem menuItem2 = new MenuItem("退出");
        MenuItem menuItem3 = new MenuItem("检测更新");
        MenuItem menuItem4 = new MenuItem("查看日志");

        XMPPClass objXmpp;
        Library lib = new Library();
        Thread th_Download;
        Thread th_Login;
        //SysInfoClass SIC = new SysInfoClass();

        public string strUrl;
        public string strPath;
        public string strUnRAR;

        public string strMonitorServer = null;
        public string strUserName = null;
        public string strVersion = null;
        public string strServerName = null;

        public Client()
        {
            InitializeComponent();
            toolStripStatusLabel_Status.Text = "";
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            if (!File.Exists(Application.StartupPath + "\\Client.ini"))
            {
                Config cfg = new Config();
                cfg.ShowDialog(this);
            }
            else
            {
                toolStripStatusLabel_Status.Text = "配置文件加载成功";
                lib.LogTrace("配置文件加载成功");
            }
        }

        private void Client_Load(object sender, System.EventArgs e)
        {
            notifyIcon1.Text = "启动成功";
            notifyIcon1.Icon = Properties.Resources.run;
            notifyIcon1.Visible = true;

            menuItem1.Click += new EventHandler(menuItem1_Click);
            menuItem2.Click += new EventHandler(menuItem2_Click);
            menuItem3.Click += new EventHandler(menuItem3_Click);
            menuItem4.Click += new EventHandler(menuItem4_Click);

            //把按钮添加到notifyIcon1.ContextMenu里
            notifyIcon1.ContextMenu = new ContextMenu(new MenuItem[] { menuItem3, menuItem4, menuItem1, menuItem2 });

            menuItem1.Text = "隐藏";
            menuItem2.Text = "退出";
            menuItem3.Text = "检测更新";
            menuItem4.Text = "查看日志";

            string[] CmdArgs = System.Environment.GetCommandLineArgs();

            //启动参数检测
            if (CmdArgs.Length > 1)
            {
                //参数0是它本身的路径
                //String arg0 = CmdArgs[0].ToString();
                string arg1 = CmdArgs[1].ToString();
                if (arg1 == "AutoLogin")
                {
                    Login();
                }
            }
        }


        #region 菜单事件
        //显示隐藏
        void menuItem1_Click(object sender, System.EventArgs e)
        {
            if (this.Visible)
            {
                menuItem1.Text = "显示";
            }
            else
            {
                menuItem1.Text = "隐藏";
            }
            this.Visible = !this.Visible;
        }
        //退出
        void menuItem2_Click(object sender, System.EventArgs e)
        {            
            try
            {
                objXmpp.SetThreadExit();
                this.Dispose();
                this.Close();
            }
            catch
            {
                this.Dispose();
                this.Close();
            }
        }
        //检测更新
        void menuItem3_Click(object sender, EventArgs e)
        {
            if (lib.CheckUpdate())
            {
                try
                {
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\AutoUpdater.exe");
                }
                catch (Exception ex)
                {
                    lib.LogError(ex.Message);
                }
            }
            else
            {
                notifyIcon1.ShowBalloonTip(3000, "提示信息", "当前已是最新版本", ToolTipIcon.Info);
                notifyIcon1.Visible = true;
            }
        }
        //查看日志
        void menuItem4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + @"\log");
        }
        #endregion 菜单事件

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;//隐藏窗口
            if (!this.Visible)
            {
                menuItem1.Text = "显示";
            }
            else
            {
                menuItem1.Text = "隐藏";
            }
        }

        private void notifyIcon1_DoubleClick(object sender, System.EventArgs e)
        {
            this.Visible = true;//显示窗口
            if (!this.Visible)
            {
                menuItem1.Text = "显示";
            }
            else
            {
                menuItem1.Text = "隐藏";
            }
        }

        private void button_Monitor_Click(object sender, System.EventArgs e)
        {
            ////////////////////////////////////////////////////////////////////
            if (lib.CheckUpdate())
            {
                try
                {
                    this.Dispose();//清理所有正在使用资源
                    Application.Exit();//通知所有消息泵必须终止，并且在处理了消息以后关闭所有应用程序窗口。`
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\AutoUpdater.exe");
                    Process.GetCurrentProcess().Kill();//杀死原有进程
                }
                catch (Exception ex)
                {
                    lib.LogError(ex.Message);
                }
            }
            else
            {
                th_Login = new Thread(new ThreadStart(Login));
                th_Login.Start();
                //Login();
            }

            //////////////////////重启
            //this.Dispose();//清理所有正在使用资源
            //Application.Exit();//通知所有消息泵必须终止，并且在处理了消息以后关闭所有应用程序窗口。
            //Application.Restart();//重启进程
            //Process.GetCurrentProcess().Kill();//杀死原有进程
        }

        //文件下载
        private void DownloadFile()
        {
            //调试时使用
            //this.strUrl = "http://222.73.57.140/服务器设置.rar";
            //this.strPath = "服务器设置.rar";
            //================================================
            Form_Download frm_Download = new Form_Download(this.strUrl, this.strPath);
            frm_Download.DownloadFileCompleted += new Form_Download.EventHandler(frm_Download_DownloadFileCompleted);
            frm_Download.ShowDialog();
        }

        //下载完成时触发
        void frm_Download_DownloadFileCompleted(object sender)
        {
            if (this.strUnRAR == "True")
            {
                //在这里加入解压方法
                lib.CreateDirectory(@"D:\Temp");
                lib.RunCMD("C:\\rar x "+ this.strPath +" D:\\Temp -y", 0);
            }
            th_Download.Abort();
            th_Download.Join();
        }

        //登录方法
        private void Login()
        {
            string strServer = null;            
            string strPassword = null;

            strPassword = lib.ReadINI("ConnectionServer", "Password");
            strServer = lib.ReadINI("ConnectionServer", "ServerAddress");
            this.strUserName = lib.ReadINI("ConnectionServer", "UserName");            
            strPassword = lib.DESDecrypt(strPassword);
            this.strServerName = lib.ReadINI("ClientConfig", "ServerName");
            this.strMonitorServer = lib.ReadINI("ClientConfig", "MonitorServer");
            this.strVersion = lib.ReadINI("Update", "Version"); 

            toolStripStatusLabel_Status.Text = "正在登录...";
            button_Monitor.Enabled = false;
            objXmpp = new XMPPClass(strUserName, strPassword, strServer, strServerName, strMonitorServer);
            objXmpp.OnlineStatus += new XMPPClass.EventHandler(objXmpp_OnlineStatus);
            objXmpp.ConnectChange += new XMPPClass.ConnectChangeHandler(objXmpp_ConnectChange);//掉线时触发
            objXmpp.ReciveMsg += new XMPPClass.MsgHandler(objXmpp_ReciveMsg);
        }

        //收到消息
        void objXmpp_ReciveMsg(agsXMPP.protocol.client.Message msg)
        {
            if (msg.From.Bare == this.strMonitorServer)
            {
                MsgHandle(msg.Body);
            }
        }

        //断开连接时触发
        void objXmpp_ConnectChange(string strStatus)
        {
            //lib.LogTrace("我断线了");
            toolStripStatusLabel_Status.Text = "正在登录...";
            //MessageBox.Show("我断线了");
            //if (strStatus == "Disconnected")
            //{
            //    Login();
            //}
        }

        //登录成功时触发
        void objXmpp_OnlineStatus(object sender)
        {
            toolStripStatusLabel_Status.Text = sender.ToString();
            notifyIcon1.Text = "服务器名称:" + this.strServerName + "\r\n";
            notifyIcon1.Text += "当前用户:" + this.strUserName + "\r\n";
            notifyIcon1.Text += "当前版本:" + this.strVersion;
            this.Close();//登录成功后最小化到任务栏
        }

        #region 消息处理器 MsgHandle(string strContent)
        /// <summary>
        /// 消息处理器
        /// </summary>
        /// <param name="strContent"></param>
        private void MsgHandle(string strContent)
        {
            string[] aryContent = strContent.Split('|');
            string strMsg;

            switch (aryContent[0])
            {
                case "Update":
                    if (lib.CheckUpdate())
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(Application.StartupPath + "\\AutoUpdater.exe");
                        }
                        catch (Exception ex)
                        {
                            lib.LogError(ex.Message);
                        }
                    }
                    break;
                case "ChangeUserPassword"://ChangeUserPassword|UserName|Password
                    strMsg = UserAndGroupHelper.UpdateUserPassword(aryContent[1], aryContent[2]);
                    objXmpp.SendMsg(this.strMonitorServer, "Status|" + strMsg);
                    break;
                case "ChangeINI"://ChangeINI|sectionName|keyName|strValue
                    lib.SaveINI(aryContent[1], aryContent[2], aryContent[3]);
                    lib.LogTrace("SaveINI(" + aryContent[1] + "," + aryContent[2] + "," + aryContent[3] + ")");
                    break;
                case "DownloadFile"://DownloadFile|Url|Path+FileName|UnRAR
                    this.strUrl = aryContent[1];
                    this.strPath = aryContent[2];
                    this.strUnRAR = aryContent[3];
                    th_Download = new Thread(new ThreadStart(DownloadFile));
                    th_Download.Start();
                    break;
            }
        }
        #endregion 消息处理器 MsgHandle(string strContent)

        #region 热键
        /// <summary>
        /// 热键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                Config cfg = new Config();
                cfg.ShowDialog(this);
            }
        }
        #endregion 热键
    }
}
