using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using NetRobotApi;
using QQRobot_Module;


namespace C_wQQ
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            this.Text = "C-QQ";
            this.Size = new Size(164, 155);
        }

        #region 变量

        public static User QQUser = new User();
        public static Friendlist thefriendlist = new Friendlist();
        public static QQMessages qqmessages = new QQMessages();
        public string strContent;

        //===========机器人配置============
        const string fenge = "@"; //分隔符
        const string AdminQQ = "小柏拉,";
        //=================================

        private Stream VerifyPic;
        private string ptwebqq = string.Empty;
        private readonly Uri ckuri = new Uri("http://ptlogin2.qq.com");
        private Thread tdcheck;
        private Thread tdlogin;
        private Thread td;
        private Thread tdsend;

        readonly ServerCheck sc = new ServerCheck();
        readonly ServerMonitor sm = new ServerMonitor();
        readonly SearchInfo si = new SearchInfo();

        #endregion 变量

        #region 检查是否需要输入验证码 check()
        /// <summary>
        /// 检查是否需要输入验证码
        /// </summary>
        private void check()
        {
            Invoke((MethodInvoker)delegate
            {
                Button_LoginQQ.Enabled = false;
            });
            if (QQUser.isNeedVerifyCode())
            {
                VerifyPic = QQUser.getVerifyCodePicStream();
                Invoke((MethodInvoker)delegate
                {
                    Size = new Size(164, 239);
                    pictureBox_verifypic.Image = Image.FromStream(VerifyPic);
                    pictureBox_verifypic.Visible = true;
                    textBox_verifycode.Visible = true;
                    label3.Visible = true;
                });
            }
            else
            {
                Invoke((MethodInvoker)delegate
                {
                    pictureBox_verifypic.Visible = false;
                    textBox_verifycode.Visible = false;
                    label3.Visible = false;
                });
            }
            Invoke((MethodInvoker)delegate
            {
                Button_LoginQQ.Enabled = true;
            });
            Thread.CurrentThread.Abort();
        }
        #endregion 检查是否需要输入验证码 check()

        #region 登录流程 login()
        /// <summary>
        /// 登录流程
        /// </summary>
        private void login()
        {
            if (QQUser.Verifycode == "")
            {
                if (QQUser.isNeedVerifyCode())
                {
                    VerifyPic = QQUser.getVerifyCodePicStream();
                    Invoke((MethodInvoker)delegate
                    {
                        Button_LoginQQ.Enabled = true;
                        textBox_QQ.Enabled = true;
                        textBox_PWD.Enabled = true;
                        Button_LoginQQ.Text = "登陆";
                        pictureBox_verifypic.Image = Image.FromStream(VerifyPic);
                        pictureBox_verifypic.Visible = true;
                        textBox_verifycode.Visible = true;
                        textBox_verifycode.Enabled = true;
                        label3.Visible = true;
                    });
                    Thread.CurrentThread.Abort();
                }
            }
            QQUser.QQPassword = textBox_PWD.Text.Trim();
            string loginstate = QQUser.loginWebQQ(checkBox_offline.Checked);
            if (loginstate != "登录成功")
            {
                MessageBox.Show(loginstate);
                QQUser.Verifycode = "";
                Invoke((MethodInvoker)delegate
                {
                    Button_LoginQQ.Enabled = true;
                    textBox_QQ.Enabled = true;
                    textBox_PWD.Enabled = true;
                    Button_LoginQQ.Text = "登陆";
                });
                Thread.CurrentThread.Abort();
                return;
            }
            ptwebqq = QQUser.Cookie.GetCookies(ckuri)["ptwebqq"].Value;
            var getretcode = new Regex("(retcode\":)(?<retcode>.+?)(,)");
            Invoke((MethodInvoker)delegate
            {
                Button_LoginQQ.Text = "登录成功";
            });
            string tempvalue = QQUser.loginGetValue(ptwebqq);
            if (!getretcode.Match(tempvalue).Success || getretcode.Match(tempvalue).Groups["retcode"].Value != "0")
            {
                MessageBox.Show("登录取值失败");
                QQUser.Verifycode = "";
                Invoke((MethodInvoker)delegate
                {
                    Button_LoginQQ.Enabled = true;
                    textBox_QQ.Enabled = true;
                    textBox_PWD.Enabled = true;
                    Button_LoginQQ.Text = "登陆";
                });
                Thread.CurrentThread.Abort();
                return;
            }
            var regexvalue = new Regex("(vfwebqq\":\")(?<vfwebqq>.+?)(\",\"psessionid\":\")(?<psessionid>.+?)(\"}})");
            QQUser.Vfwebqq = regexvalue.Match(tempvalue).Groups["vfwebqq"].Value;
            QQUser.Psessionid = regexvalue.Match(tempvalue).Groups["psessionid"].Value;
            string tempfriendonline = string.Empty;
            Invoke((MethodInvoker)delegate
            {
                Button_LoginQQ.Text = "获取个人信息";
            });
            string tempmyinfo = QQUser.getfriendinfo(QQUser.Uin);
            if (!getretcode.Match(tempmyinfo).Success || getretcode.Match(tempmyinfo).Groups["retcode"].Value != "0")
            {
                MessageBox.Show("获取个人信息失败");
                QQUser.Verifycode = "";
                Invoke((MethodInvoker)delegate
                {
                    Button_LoginQQ.Enabled = true;
                    textBox_QQ.Enabled = true;
                    textBox_PWD.Enabled = true;
                    Button_LoginQQ.Text = "登陆";
                });
                Thread.CurrentThread.Abort();
                return;
            }
            Invoke((MethodInvoker)delegate
            {
                Button_LoginQQ.Text = "获取好友列表";
            });
            string tempfriend = QQUser.getFriend();
            if (!getretcode.Match(tempfriend).Success || getretcode.Match(tempfriend).Groups["retcode"].Value != "0")
            {
                MessageBox.Show("获取好友列表失败");
                QQUser.Verifycode = "";
                Invoke((MethodInvoker)delegate
                {
                    Button_LoginQQ.Enabled = true;
                    textBox_QQ.Enabled = true;
                    textBox_PWD.Enabled = true;
                    Button_LoginQQ.Text = "登陆";
                });
                Thread.CurrentThread.Abort();
                return;
            }
            Invoke((MethodInvoker)delegate
            {
                Button_LoginQQ.Text = "获取群列表";
            });
            string tempqqgroup = QQUser.getgroup();
            if (!getretcode.Match(tempqqgroup).Success || getretcode.Match(tempqqgroup).Groups["retcode"].Value != "0")
            {
                MessageBox.Show("获取群列表失败");
                QQUser.Verifycode = "";
                Invoke((MethodInvoker)delegate
                {
                    Button_LoginQQ.Enabled = true;
                    textBox_QQ.Enabled = true;
                    textBox_PWD.Enabled = true;
                    Button_LoginQQ.Text = "登陆";
                });
                Thread.CurrentThread.Abort();
                return;
            }
            thefriendlist = GetFriendlists.GetFriendsList(tempfriend);
            thefriendlist.OnLineFriends = GetFriendlists.GetOnlineFriend(tempfriendonline);
            thefriendlist.QQGroups = GetFriendlists.GetGroupList(tempqqgroup);
            thefriendlist.setmyinfo(QQUser.Uin, tempmyinfo);
            td = new Thread(GetMessage);
            td.Start();
            Thread.CurrentThread.Abort();            
        }
        #endregion 登录流程 login()

        #region QQ号码输入验证
        /// <summary>
        /// QQ号码输入验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_QQ_Leave(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBox_QQ.Text.Trim(), "[0-9]{5,11}"))
            {
                if (textBox_QQ.Enabled)
                {
                    QQUser.Uin = textBox_QQ.Text;
                    tdcheck = new Thread(check);
                    tdcheck.Start();
                }
            }
            else
            {
                MessageBox.Show("QQ号不正确");
            }
        }
        #endregion QQ号码输入验证

        private void Button_LoginQQ_Click(object sender, EventArgs e)
        {
            if (textBox_verifycode.Visible)
            {
                if (textBox_verifycode.Text != "")
                {
                    QQUser.Verifycode = textBox_verifycode.Text;
                }
                else
                {
                    MessageBox.Show("请输入验证码");
                    QQUser.Verifycode = "";
                    return;
                }
            }
            if (textBox_PWD.Text != "")
            {
                Button_LoginQQ.Text = "登陆中";
                Button_LoginQQ.Enabled = false;
                textBox_QQ.Enabled = false;
                textBox_PWD.Enabled = false;
                textBox_verifycode.Enabled = false;

                tdlogin = new Thread(login);
                tdlogin.Start();
            }
            else
            {
                MessageBox.Show("请输入密码");
                QQUser.Verifycode = "";
                return;
            }
        }

        #region 轮巡方法 GetMessage()
        /// <summary>
        /// 轮巡方法
        /// </summary>
        private void GetMessage()
        {
            var messagecode = new Regex("(\"retcode\":)(?<retcode>[0-9]{1,3}?)(,\")");
            var messagetype = new Regex("(\"poll_type\":\")(?<type>.+?)(\",\"value\":{)(?<value>.+?)(}})");
            while (true)
            {
                string result = QQUser.getmessage();
                if (messagecode.Matches(result).Count == 0)
                {
                    continue;
                }
                Match itemcode = messagecode.Match(result);
                if (itemcode.Groups["retcode"].Value != "0")
                {
                    continue;
                }
                foreach (Match item in messagetype.Matches(result))
                {
                    if (item.Groups["type"].Value == "message")
                    {
                        //接收消息处理
                        strContent = item.Groups["value"].Value;
                        tdsend = new Thread(sendAmessage);
                        tdsend.Start();
                        //GetAMessage(item.Groups["value"].Value);
                        continue;
                    }
                    else if (item.Groups["type"].Value == "buddies_status_change")
                    {
                        //状态改变处理
                        //StatusChange(item.Groups["value"].Value);
                        continue;
                    }
                    else if (item.Groups["type"].Value == "group_message")
                    {
                        //群消息处理
                        //GetGroupMessage(item.Groups["value"].Value);
                        continue;
                    }
                }
            }
        }
        #endregion 轮巡方法 GetMessage()

        #region 发消息调用线程 sendAmessage()
        /// <summary>
        /// 发消息调用线程
        /// </summary>
        private void sendAmessage()
        {
            GetAMessage(strContent);
            Thread.CurrentThread.Abort();
        }
        #endregion 发消息调用线程 sendAmessage()

        #region 发消息方法 GetAMessage(string value)
        /// <summary>
        /// 发消息方法
        /// </summary>
        /// <param name="value"></param>
        private void GetAMessage(string value)
        {
            var messagecontent = new Regex("(}],)(?<content>.+?)(],\"raw_content)");
            var messageiduin = new Regex("(\"msg_id\":)(?<msgid>[0-9]{1,8}?)(,\"from_uin\":)(?<fromuin>[0-9]{5,11}?)(,\")");
            string tempcontent = Converts.ConvertUnicodeStringToChinese(messagecontent.Match(value).Groups["content"].Value);
            string content = Converts.ArrangeMessage(tempcontent);
            string fromuin = messageiduin.Match(value).Groups["fromuin"].Value;

            string[] command = content.Split(' ');
            switch (command[0].ToLower())
            {
                case "@":
                    if (YZadmin(thefriendlist.Friends[fromuin].name) == "T")
                    {
                        content = string.Format("您好，我是" + thefriendlist.myself.name + "\\\\n======您可以使用如下命令======\\\\n{0}weather    天气查询（新）\\\\n=======系统管理员命令=======\\\\n{0}turn     游戏赛季更新检测  例：{0}turn BB\\\\n{0}status   游戏夜间更新检测  例：{0}status BB\\\\n{0}season   查询赛季更新详情  例：{0}season XBA\\\\n{0}check    查询夜间更新是否执行  例：{0}check BB\\\\n{0}userinfo 查询官方区用户信息\\\\n==========特殊功能==========\\\\n{0}mail  简单邮件发送\\\\n{0}setnick  设置QQ机器人签名\\\\n{0}setqqstatus  设置机器人在线状态", fenge);
                    }
                    else
                    {
                        content = string.Format("您好，我是" + thefriendlist.myself.name + "\\\\n======您可以使用如下命令======\\\\n{0}weather    天气查询（新）", fenge);
                    }
                    break;
                case "@weather":
                    if (command.Length == 2)
                    {
                        var api = new RobotApi();
                        content = api.GetWeather(command[1], "3").Replace("\r","\\\\n");
                    }
                    else
                    {
                        content = "参数错误。\\\\n天气查询（新）使用方法：@weather 城市名 或（区号，拼音（支持模糊），邮编）\\\\n例：\\\\n@weather 北京（城市名）\\\\n@weather 0597（区号）\\\\n@weather beijin（拼音）\\\\n@weather 364000（邮编）";
                    }
                    break;
                case "@turn"://赛季更新查询
                    if (command.Length == 2)
                    {
                        content = sc.GetGameTurn(command[1]).Replace("\n", "\\\\n");
                    }
                    else
                    {
                        content = "参数错误。\\\\n赛季更新检测的使用方法：@turn 项目名称(BB,FB)\\\\n例：@turn BB";
                    }
                    break;
                case "@status"://夜间更新查询
                    if (command.Length == 2)
                    {
                        content = sc.GetGameStatus(command[1]).Replace("\n", "\\\\n");
                    }
                    else
                    {
                        content = "参数错误。\\\\n夜间更新检测的使用方法：@status 项目名称(BB,FB)\\\\n例：@status BB";
                    }
                    break;
                case "@mail"://发送邮件
                    if (command.Length == 4)
                    {
                        content = sm.SendMail(command[1], command[2], command[3]);
                    }
                    else
                    {
                        content = "参数错误。\\\\n使用方法：@mail 收件邮件 邮件主题 邮件内容\\\\n例：@mail toMail@xxx.com 开会 三点开会";
                    }
                    break;
                case "@season"://赛季更新时间查询
                    if (command.Length == 2)
                    {
                        content = sc.GetGameSeason(command[1]).Replace("\n", "\\\\n");
                    }
                    else
                    {
                        content = "参数错误。\\\\n赛季更新检测的使用方法：@season 关键字名称(XBA,DW,TOM)\\\\n例：@season XBA";
                    }
                    break;
                case "@check"://查询夜间更新是否执行
                    if (command.Length == 2)
                    {
                        content = sc.GetGameCheck(command[1]).Replace("\n", "\\\\n");
                    }
                    else
                    {
                        content = "参数错误。\\\\n查询夜间更新是否执行的使用方法：@check 项目名称(BB,FB)\\\\n例：@check BB";
                    }
                    break;
                case "@userinfo"://查询用户信息
                    if (command.Length == 3)
                    {
                        content = si.UserInfo(command[1], command[2]).Replace("\r\n", "\\\\n");
                    }
                    else
                    {
                        content = "参数错误。\\\\n查询用户信息的使用方法：\\\\n@userinfo 用户名类型(0为经理名|1为用户名) 经理名\\\\n例：@userinfo 0 风中脱手";
                    }
                    break;
                case "@setnick"://设置机器人签名
                    if (command.Length == 2)
                    {
                        content = QQUser.getlongnick2(command[1]);
                    }
                    else
                    {
                        content = "参数错误。\\\\n设置机器人签名方法：\\\\n@setnick 签名内容\\\\n例：@setnick 今天好开心啊";
                    }
                    break;
                case "@setqqstatus"://设置机器人在线状态
                    if (command.Length == 2)
                    {
                        content = QQUser.getchangestatus(command[1]);
                    }
                    else
                    {
                        content = "参数错误。\\\\n设置机器人在线状态方法：\\\\n@setqqstatus (在线|离开|隐身)\\\\n例：@setqqstatus 在线";
                    }
                    break;
            }
            string result = QQUser.sendmessage(content, fromuin);
            if (result.IndexOf("ok") == -1)
            {
                MessageBox.Show("消息 " + content + " 发送失败");
            }
            //Thread.CurrentThread.Abort();
        }
        #endregion 发消息方法 GetAMessage(string value)

        #region 单击更换验证码
        /// <summary>
        /// 单击更换验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_verifypic_Click(object sender, EventArgs e)
        {
            tdcheck = new Thread(check);
            tdcheck.Start();
        }
        #endregion 单击更换验证码

        #region 退出时处理方法
        /// <summary>
        /// 退出时处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            QQUser.logout();
            Application.Exit();
            Environment.Exit(Environment.ExitCode);
        }
        #endregion 退出时处理方法

        #region 管理员验证方法 YZadmin(string Sender)
        /// <summary>
        /// 管理员验证方法
        /// </summary>
        /// <param name="Sender"></param>
        /// <returns></returns>
        public static string YZadmin(string Sender)
        {
            Match Yadmin = Regex.Match(AdminQQ, Sender + ",", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (Yadmin.Success)
            {
                const string QQback = "T";
                return QQback;
            }
            else
            {
                const string QQback = "F";
                return QQback;
            }
        }
        #endregion 管理员验证方法 YZadmin(string Sender)

        //最小化时隐藏窗体显示托盘图标
        private void Main_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Visible = false;//隐藏窗体
                notifyIcon1.Visible = true;//显示托盘图标
                notifyIcon1.ShowBalloonTip(2000, "提示", "C-QQ隐藏在这里噢~", ToolTipIcon.Info);
            }  
        }

        //双击最大化显示窗体
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Visible = true;
                Left = Left;
                WindowState = FormWindowState.Normal;
                notifyIcon1.Visible = false;
            }
        }
    }
}
