using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using NetRobotApi;

namespace QQRobot_InterFace
{
    public partial class _Default : System.Web.UI.Page
    {
        static string Copyright = "QQrobot";//密匙信息验证，应与机器人配置相同
        static string AdminQQ = "182536608,";//管理员QQ号码，多个管理员用“ ，”隔开最后一个也要加上
        static string QQ = "1349836289";//QQ机器人号码，多个机器人用“ ，”隔开最后一个也要加上
        static string Filtration = "";//需要过滤群消息的QQ号码，多个QQ用“ ，”隔开最后一个也要加上
        static string RobotName = "【Robot】36";
        public static Queue que = Queue.Synchronized(new Queue());
        public static Hashtable hashWenda = Hashtable.Synchronized(new Hashtable());
        public static Hashtable adminhashWenda = Hashtable.Synchronized(new Hashtable());
        public static Hashtable hashQunWenda = Hashtable.Synchronized(new Hashtable());
        static string getcode;


        static public String YZadmin(string Sender)
        {
            Match Yadmin = Regex.Match(AdminQQ, Sender + ",", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (Yadmin.Success)
            {
                string QQback = "T";
                return QQback;
            }
            else
            {
                string QQback = "F";
                return QQback;
            }
        }

        protected void com(string Event, string Qunid, string Qunnid, string Sender, string Nick, string Message, string AdminQQ)
        {
            string[] sArray1 = System.Text.RegularExpressions.Regex.Split(Message, " ");
            string command = sArray1[0];
            int msgsArray1 = 1;
            int adminmsgzu = 2;
            string msg = "";
            string adminmsg = "";
            if (sArray1.Length > 1)
            {
                while (msgsArray1 < sArray1.Length)
                {
                    msg += sArray1[msgsArray1];
                    msg += " ";
                    msgsArray1++;
                }
                if (sArray1.Length > 2)
                {
                    while (adminmsgzu < sArray1.Length)
                    {
                        adminmsg += sArray1[adminmsgzu];
                        adminmsg += " ";
                        adminmsgzu++;
                    }
                }
            }


            switch (command.ToLower())
            {
                //功能参数区-----------------------------
                case "/":
                    if (YZadmin(Sender) == "T")
                    {
                        Response.Write(string.Format("                  你好，我是V客助手！\n===================您可以使用如下命令：=================\n/聊天  与小i机器人进行对话\n/学习  教机器人说话！\n/tq    天气查询\n/ip    查询IP归属地\n/md5   查询MD5加密数值\n/qun   查询本群信息资料\n/qq    查询QQ状态\n/bm    查询 base64 加密\n/jm    查询 base64 解密\n/by    查询异或后 base64 加密\n/jy    查询异或后 base64 解密\n/cfs   查询 Cfs加密 单项不可逆\n/al    查询全球 Alexa 排名\n/ips   查询同服务器下站点数量\n/dns   查询本域名下的DNS服务器\n/sfz   查询身份证信息\n/t18   15位身份证升级为18为身份证\n/m     查询手机归属地\n/who   *域名或IP地址的 WHOIS 记录查询\n直接输入命令将回复使用方法！\n更多功能正在添加中！\n======您是管理员，可以使用下面的功能！======\n1./send     QQ号码 消息内容   发送QQ消息\n2./qunsend  群内部ID 内容     发送群消息\n3./szd      QQ号码            给好友发送震动\n4./reset    重新启动机器人\n5./update   更新机器人"));
                    }
                    else
                    {
                        Response.Write(string.Format("                  你好，我是V客助手！\n===================您可以使用如下命令：=================\n/聊天  与小i机器人进行对话\n/学习  教机器人说话！\n/tq    天气查询\n/ip    查询IP归属地\n/md5   查询MD5加密数值\n/qun   查询本群信息资料\n/qq    查询QQ状态\n/bm    查询 base64 加密\n/jm    查询 base64 解密\n/by    查询异或后 base64 加密\n/jy    查询异或后 base64 解密\n/cfs   查询 Cfs加密 单项不可逆\n/al    查询全球 Alexa 排名\n/ips   查询同服务器下站点数量\n/dns   查询本域名下的DNS服务器\n/sfz   查询身份证信息\n/t18   15位身份证升级为18为身份证\n/m     查询手机归属地\n/who   *WHOIS记录查询\n直接输入命令将回复使用方法！"));
                    }
                    break;
                case "/聊天":
                    if (sArray1.Length != 1)
                    {
                        if (NetRobotApi.RobotApi.m_strSID == null)
                        {
                            NetRobotApi.RobotApi.Xiaoi();
                            getcode = NetRobotApi.RobotApi.chatXiaoi(msg, RobotName);
                            Response.Write(getcode);
                        }
                        else
                        {
                            getcode = NetRobotApi.RobotApi.chatXiaoi(msg, RobotName);
                            Response.Write(getcode);
                        }
                    }
                    else
                    {
                        Response.Write("你想对我说什么？干嘛那么墨迹...");
                    }
                    break;
                case "/ip":
                    if (sArray1.Length == 2)
                    {
                        getcode = NetRobotApi.RobotApi.chaip(sArray1[1]);
                        Response.Write(getcode);
                    }
                    else
                    {
                        Response.Write("参数错误。请返回检查！谢谢合作！");
                    }
                    break;
                case "/tq"://天气查询
                    if (sArray1.Length == 2)
                    {
                        Match m1 = Regex.Match(sArray1[1], @"^((\(\d{3}\))|(\d{3}\-))?13[0-9]\d{8}|15[89]\d{8}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        if (m1.Success)
                        {
                            getcode = NetRobotApi.RobotApi.tq("mobile", sArray1[1]);
                            Response.Write(getcode);
                            return;
                        }
                        Match m2 = Regex.Match(sArray1[1], "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        if (m2.Success)
                        {
                            getcode = NetRobotApi.RobotApi.tq("ip", sArray1[1]);
                            Response.Write(getcode);
                            return;
                        }
                        getcode = NetRobotApi.RobotApi.tq("city", sArray1[1]);
                        Response.Write(getcode);
                    }
                    else
                    {
                        Response.Write("参数错误。\n天气查询使用方法：/tq 城市名 或（手机号，IP地址）\n例：\n/tq 北京（城市名）\n/tq 13459777850（手机号码）\n/tq 61.153.223.5（IP地址）");
                    }
                    break;
                case "/md5"://查询MD5加密数值
                    if (sArray1.Length == 2)
                    {
                        getcode = NetRobotApi.RobotApi.Md5(sArray1[1]);
                        Response.Write(getcode);
                    }
                    else
                    {
                        Response.Write("参数错误。\nMD5加密查询使用方法：/md5 明文\n例：/md5 Vckers");
                    }
                    break;
                case "/qq"://查询QQ状态
                    if (sArray1.Length == 2)
                    {
                        getcode = NetRobotApi.RobotApi.QQonline(sArray1[1]);
                        Response.Write(getcode);
                    }
                    else
                    {
                        Response.Write("参数错误。\n查询QQ状态使用方法：/qq QQ号码\n例：/qq 582257138");
                    }
                    break;
                case "/bm"://查询 base64 加密
                    if (sArray1.Length == 2)
                    {
                        getcode = NetRobotApi.RobotApi.enbase64(sArray1[1]);
                        Response.Write(getcode);
                    }
                    else
                    {
                        Response.Write("参数错误。\n查询 base64 加密使用方法：/bm 明文\n例：/bm Vckers");
                    }
                    break;
                case "/jm"://查询 base64 解密
                    if (sArray1.Length == 2)
                    {
                        getcode = NetRobotApi.RobotApi.debase64(sArray1[1]);
                        Response.Write(getcode);
                    }
                    else
                    {
                        Response.Write("参数错误。\n查询 base64 解密使用方法：/jm 密文\n例：/jm VmNrZXJz");
                    }
                    break;
                case "/by"://查询异或后 base64 加密
                    if (sArray1.Length == 3)
                    {
                        getcode = NetRobotApi.RobotApi.encode(sArray1[1], sArray1[2]);
                        Response.Write(getcode);
                    }
                    else
                    {
                        Response.Write("参数错误。\n查询异或后 base64 加密使用方法：/by 明文 异或参数\n例：/by 123456 Vckers");
                    }
                    break;
                case "/jy"://查询异或后 base64 解密
                    if (sArray1.Length == 3)
                    {
                        getcode = NetRobotApi.RobotApi.decode(sArray1[1], sArray1[2]);
                        Response.Write(getcode);
                    }
                    else
                    {
                        Response.Write("参数错误。\n查询异或后 base64 解密使用方法：/jy 密文 异或参数\n例：/jy UllWRkZg Vckers");
                    }
                    break;
                case "/cfs"://查询 Cfs加密 单项不可逆
                    if (sArray1.Length == 2)
                    {
                        getcode = NetRobotApi.RobotApi.cfs(sArray1[1]);
                        Response.Write(getcode);
                    }
                    else
                    {
                        Response.Write("参数错误。\n查询 Cfs加密使用方法：/cfs 明文 （注意：该操作不可逆）\n例：/cfs Vckers");
                    }
                    break;
                case "/hb"://货币换算查询
                    if (sArray1.Length == 2)
                    {
                        getcode = NetRobotApi.RobotApi.chaip(sArray1[1]);
                        Response.Write(getcode);
                    }
                    else
                    {
                        Response.Write("参数错误。请返回检查！谢谢合作！");
                    }
                    break;
                case "/al"://查询全球 Alexa 排名
                    if (sArray1.Length == 2)
                    {
                        getcode = NetRobotApi.RobotApi.Alexa(sArray1[1]);
                        Response.Write(getcode);
                    }
                    else
                    {
                        Response.Write("参数错误。\n查询全球 Alexa 排名使用方法：/al 域名 （注意：不需要带http头）\n例：/al Vckers.com");
                    }
                    break;
                case "/ips"://查询同服务器下站点数量
                    if (sArray1.Length == 2)
                    {

                        getcode = NetRobotApi.RobotApi.chaip(sArray1[1]);
                        Response.Write(getcode);
                    }
                    else
                    {
                        Response.Write("参数错误。请返回检查！谢谢合作！");
                    }
                    break;
                case "/dns"://查询本域名下的DNS服务器
                    if (sArray1.Length == 2)
                    {
                        getcode = NetRobotApi.RobotApi.dns(sArray1[1]);
                        Response.Write(getcode);
                        return;
                    }
                    Response.Write("参数错误。\n查询本域名下的DNS服务器使用方法：/dns 域名 （注意：不需要带http头）\n例：/dns Vckers.com");
                    break;
                case "/who"://域名或 IP 地址的 WHOIS 记录查询：
                    if (sArray1.Length == 2)
                    {
                        Match m1 = Regex.Match(sArray1[1], @"([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        if (m1.Success)
                        {
                            getcode = NetRobotApi.RobotApi.whois("domain", sArray1[1]);
                            Response.Write(getcode);
                            return;
                        }
                        Match m2 = Regex.Match(sArray1[1], "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        if (m2.Success)
                        {
                            getcode = NetRobotApi.RobotApi.whois("ip", sArray1[1]);
                            Response.Write(getcode);
                            return;
                        }
                        else
                        {
                            Response.Write("该域名不是标准域名或IP格式！");
                        }
                    }
                    Response.Write("*本功能尚未完善*\n参数错误。\n域名或IP地址的 WHOIS 记录查询使用方法：/dns 域名 或 IP （注意：域名不需要带http头）\n例：\n/who Vckers.com（域名）\n/who 202.108.33.32（IP地址）");
                    break;
                case "/sfz"://身份证信息查询
                    if (sArray1.Length == 2)
                    {
                        getcode = NetRobotApi.RobotApi.sfz(sArray1[1]);
                        Response.Write(getcode);
                        return;
                    }
                    break;
                case "/t18"://15位身份证升级为18为身份证
                    if (sArray1.Length == 2)
                    {

                        getcode = NetRobotApi.RobotApi.upto18(sArray1[1]);
                        Response.Write(getcode);
                        return;
                    }
                    Response.Write("*本功能尚未完善*\n参数错误。\n域名或IP地址的 WHOIS 记录查询使用方法：/dns 域名 或 IP （注意：域名不需要带http头）\n例：\n/who Vckers.com（域名）\n/who 202.108.33.32（IP地址）");
                    break;
                case "/m"://查询手机号码归属地
                    if (sArray1.Length == 2)
                    {
                        if (sArray1[1].Length == 11)
                        {
                            getcode = NetRobotApi.RobotApi.shouji(sArray1[1]);
                            Response.Write(getcode);
                        }
                        else
                        {
                            Response.Write("恩，我记得手机号码应该是11位的吧..你输入的好像不是11位哦？");
                        }
                    }
                    else
                    {
                        Response.Write("参数错误。\n手机归属地查询使用方法：/m 手机号码\n例：/m 13459777850");
                    }
                    break;
                case "/send":
                    if (sArray1.Length > 2)
                    {
                        if (YZadmin(Sender) == "T")
                        {
                            que.Enqueue(string.Format("0\n{0}\n{1}", sArray1[1], adminmsg));
                            Response.Write(string.Format("操作成功！成功把参数传给机器人！请等待机器人回应！"));
                        }
                        else
                        {
                            Response.Write("您好，您不是管理员不能使用该功能！");
                        }
                    }
                    break;
                case "/reset":
                    if (YZadmin(Sender) == "T")
                    {
                        que.Enqueue(string.Format("3\n{0}\n1", Qunnid));
                        Response.Write(string.Format("机器人重启命令发送成功！"));
                    }
                    else
                    {
                        Response.Write("您好，您不是管理员不能使用该功能！");
                    }
                    break;
                case "/update":
                    if (YZadmin(Sender) == "T")
                    {
                        que.Enqueue(string.Format("4\n0\n0"));
                        Response.Write(string.Format("机器人更新命令发送成功！"));
                    }
                    else
                    {
                        Response.Write("您好，您不是管理员不能使用该功能！");
                    }
                    break;
                case "/exit":
                    if (YZadmin(Sender) == "T")
                    {
                        que.Enqueue(string.Format("Exit\n0\n0"));
                        Response.Write(string.Format("机器人退出命令发送成功！"));
                    }
                    else
                    {
                        Response.Write("您好，您不是管理员不能使用该功能！");
                    }
                    break;
                case "/login":
                    if (YZadmin(Sender) == "T")
                    {
                        if (sArray1.Length == 3)
                        {
                            que.Enqueue(string.Format("Login\n{0}\n{1}", sArray1[1], sArray1[2]));
                            Response.Write(string.Format("机器人登陆命令发送成功！"));
                        }
                    }
                    else
                    {
                        Response.Write("您好，您不是管理员不能使用该功能！");
                    }
                    break;
                case "/change":
                    if (YZadmin(Sender) == "T")
                    {
                        if (sArray1.Length == 3)
                        {
                            que.Enqueue(string.Format("Change\n{0}\n{1}", sArray1[1], sArray1[2]));
                            Response.Write(string.Format("更换机器人号码成功！更换后的号码为：" + sArray1[1]));
                        }
                    }
                    else
                    {
                        Response.Write("您好，您不是管理员不能使用该功能！");
                    }
                    break;
                case "/qunsend":
                    if (YZadmin(Sender) == "T")
                    {
                        que.Enqueue(string.Format("2\n{0}\n{1}", sArray1[1], adminmsg));
                        Response.Write(string.Format("操作成功！成功把参数传给机器人！请等待机器人回应！"));
                    }
                    else
                    {
                        Response.Write("您好，您不是管理员不能使用该功能！");
                    }
                    break;
                case "/szd":
                    if (YZadmin(Sender) == "T")
                    {
                        que.Enqueue(string.Format("5\n{0}\n1", sArray1[1]));
                        Response.Write(string.Format("操作成功！成功把参数传给机器人！请等待机器人回应！"));
                    }
                    else
                    {
                        Response.Write("您好，您不是管理员不能使用该功能！");
                    }
                    break;
                case "/qun":
                    Response.Write(string.Format("QQ群信息：\n\n群号：{0}\n群内部ID：{1}", Qunid, Qunnid));
                    break;

                //------------------------------------


                //休眠命令-----------------------------
                case "回忆，咱睡觉去":
                    if (YZadmin(Sender) == "T")
                    {
                        que.Enqueue(string.Format("qunoff\n1\n1"));
                        Response.Write("好啊！累死我了，睡觉去咯.....");
                    }
                    else
                    {
                        Response.Write("咳咳，我和你很熟吗？你就这么命令我？");
                    }
                    break;
                case "回忆，洗白白去咯":
                    if (YZadmin(Sender) == "T")
                    {
                        que.Enqueue(string.Format("qunoff\n1\n1"));
                        Response.Write("洗白白..洗白白...拜拜咯~ (*^__^*) ");
                    }
                    else
                    {
                        Response.Write("你个色狼，想看人家洗澡...呜呜...");
                    }
                    break;
                case "回忆，屏蔽所有信息，等我命令":
                    if (YZadmin(Sender) == "T")
                    {
                        que.Enqueue(string.Format("qunoff\n1\n1"));
                        Response.Write("收到命令！执行……，嘿嘿屏蔽成功！就等你说话呢！");
                    }
                    else
                        Response.Write("你谁啊..又不是我的主人..凭什么命令我？");
                    break;
                case "回忆，撤除屏蔽":
                    if (YZadmin(Sender) == "T")
                    {
                        que.Enqueue(string.Format("qunon\n1\n1"));
                        Response.Write("好了，已经撤除屏蔽");
                        return;
                    }
                    else
                    {
                        Response.Write("恩？你教我撤就撤。那我的面子往哪放？");
                    }
                    break;
                case "回忆，睡觉去吧":
                    if (YZadmin(Sender) == "T")
                    {
                        que.Enqueue(string.Format("qunoff\n1\n1"));
                        Response.Write("恩，好的，天使再见哦");
                    }
                    else
                    {
                        Response.Write("不行，我在等天使呢");
                    }
                    break;
                case "回忆，起床了":
                    if (YZadmin(Sender) == "T")
                    {
                        que.Enqueue(string.Format("qunon\n1\n1"));
                        Response.Write("起床啦....我又回来了");
                        return;
                    }
                    break;
                case "回忆，给我回来":
                    if (YZadmin(Sender) == "T")
                    {
                        que.Enqueue(string.Format("qunon\n1\n1"));
                        Response.Write("我又回来了 - -！");
                        return;
                    }
                    Response.Write("去..给爷滚开，我又没出去！");
                    break;

                //------------------------------------


                //自动回复-----------------------------

                case "回忆，求代码":
                    Response.Write("哈哈，你要我的代码？我现在还没开源哦...");
                    break;
                case "回忆，求程序":
                    if (Qunid == "93068095")
                    {
                        Response.Write("恭喜您，您已经加入了QQ机器人俱乐部。\n现在请打开群共享，选择最新版本下载！");
                    }
                    else
                    {
                        Response.Write("恩，程序啊...请加群93068095获得。");
                    }
                    break;
                case "回忆，程序打不开":
                    if (Qunid == "93068095")
                    {
                        Response.Write("请您打开群共享，下载机器人教程观看！如果无法解决请联系管理员！");
                    }
                    else
                    {
                        Response.Write("打不开？出错了？请加群93068095解决！");
                    }
                    break;
                case "q群":
                case "qq群":
                case "回忆，qq群是多少？":
                case "回忆，交流群号码多少？":
                    Response.Write("QQ机器人俱乐部：93068095\n该群提供机器人程序，机器人接口，支持刷屏，测试QQ机器人等");
                    break;
                case "回忆，喊大爷":
                    if (YZadmin(Sender) == "T")
                    {
                        Response.Write("天使大爷好！");
                    }
                    else
                        Response.Write("喊你个XXOO的");
                    break;
                case "回忆，清屏":
                    Response.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n清理完毕！");
                    break;
                case "回忆":
                    if (YZadmin(Sender) == "T")
                    {
                        Response.Write("天使大爷，我在呢，找小的干嘛？");
                    }
                    else
                    {
                        Response.Write("爷在呢，找我干嘛？");
                    }
                    break;
                case "回忆，骂他们":
                    if (YZadmin(Sender) == "T")
                    {
                        Response.Write("我XX你个OO的...你们再欺负我就画个圈圈诅咒你们！！");
                    }
                    else
                    {
                        Response.Write("你叫我骂就骂啊，那我岂不是很没面子 - -！");
                    }
                    break;
                default:
                    if (Event == "ReceiveNormalIM")
                    {
                        if (NetRobotApi.RobotApi.m_strSID == null)
                        {
                            NetRobotApi.RobotApi.Xiaoi();
                            getcode = NetRobotApi.RobotApi.chatXiaoi(Message, RobotName);
                            Response.Write(getcode);
                        }
                        else
                        {
                            getcode = NetRobotApi.RobotApi.chatXiaoi(Message, RobotName);
                            Response.Write(getcode);
                        }
                    }
                    else 
                    {
 
                    };
                    break;

                //------------------------------------

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RequestType.Equals("post", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    if (Request.Form["Copyright"].Trim() == Copyright)
                    {
                        goto Loding;
                    }
                    else
                    {
                        Response.Write("密匙验证错误！您没有权限使用该接口！");
                        return;
                    }
                }
                catch
                {
                    Response.Write("密匙验证错误！您没有权限使用该接口！");
                    return;
                }

                ///***************************************

            Loding:
                string Event = Request.Form["Event"].Trim();
                string Qunid = Request.Form["Qunid"].Trim();
                string Qunnid = Request.Form["Qunnid"].Trim();
                string Sender = Request.Form["Sender"].Trim();
                string Nick = Request.Form["Nick"].Trim();
                string Message = Request.Form["Message"].Trim();
                string SendTime = Request.Form["SendTime"].Trim();
                string Version = Request.Form["Version"].Trim();
                try
                {
                    string RobotQQ = Request.Form["RobotQQ"].Trim();
                    if (NetRobotApi.RobotApi.YZrobot(RobotQQ, QQ) == "T")
                    {
                    }
                    else
                    {
                        //Response.Write("您的QQ不能使用该接口！");
                        //return;
                    }
                }
                catch
                {
                    Response.Write("您目前的版本不支持该接口！");
                    return;
                }

                if (Event == "ReceiveNormalIM")
                {//qq消息

                    if (Message.Length > 500)
                    {
                        Response.Write("我操，搞个飞机啊。。这么长！");
                        return;
                    }

                    Match m = Regex.Match(Message, @"^/学习 {1}(.*?)\r/回答 {1}(.*?)$|^/学习 {1}(.*?) /回答 {1}(.*?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        if (YZadmin(Sender) == "T")
                        {
                            adminhashWenda[m.Groups[1].Value.Trim()] = m.Groups[2].Value.Trim();
                            Response.Write("我已经学会了，您可以问我：" + m.Groups[1].Value.Trim());
                            return;
                        }
                        else
                        {
                            hashWenda[m.Groups[1].Value.Trim()] = m.Groups[2].Value.Trim();
                            Response.Write("我已经学会了，您可以问我：" + m.Groups[1].Value.Trim());
                        }

                    }
                    else if (adminhashWenda.ContainsKey(Message))
                    {
                        Response.Write(adminhashWenda[Message].ToString());
                        return;
                    }
                    else if (hashWenda.ContainsKey(Message))
                    {
                        Response.Write(hashWenda[Message].ToString());
                    }
                    else
                    {
                        com(Event, Qunid, Qunnid, Sender, Nick, Message, YZadmin(Sender));
                    }
                }
                else if (Event == "ReceiveAddFriends")
                {//加好友消息
                    Response.Write(string.Format("接口演示\n\n收到加好友的请求：{0}的消息：{1}", Sender, Message));
                }
                else if (Event == "ReceiveSignatureChanged")
                {//好友签名改变
                    Response.Write(string.Format("接口演示\n\n收到好友签名改变：QQ：{0} 昵称：{1}\n签名信息：{2}", Sender, Nick, Message));
                }
                else if (Event == "ReceiveFriendChangeStatus")
                {//好友状态改变
                    Response.Write(string.Format("接口演示\n\n收到好友状态改变：QQ：{0} 昵称：{1}\n状态：{2}", Sender, Nick, Message));
                }
                else if (Event == "LoginSucceed")
                {//机器人登陆成功
                    Response.Write(string.Format("接口演示\n\n机器人登陆成功 ：{0}", Message));
                }
                else if (Event == "ReceiveVibration")
                {//收到弹窗
                    Response.Write(string.Format("接口演示\n\n收到你的弹窗！", Sender, Message));
                }
                else if (Event == "ReceiveQunIM")
                {//群消息处理  
                    if (NetRobotApi.RobotApi.YZQQ(Sender, Filtration) == "T") { return; }
                    else if (Message.Length > 500)//消息太长
                    {
                        Response.Write("我操，搞个飞机啊。。这么长！");
                        return;
                    }
                    else
                    {
                        Match m = Regex.Match(Message, @"^/学习 {1}(.*?)\r/回答 {1}(.*?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        if (m.Success)
                        {
                            if (YZadmin(Sender) == "T")
                            {
                                adminhashWenda[m.Groups[1].Value.Trim()] = m.Groups[2].Value.Trim();
                                Response.Write("我已经学会了，您可以问我：" + m.Groups[1].Value.Trim());
                                return;
                            }
                            else
                            {
                                hashQunWenda[m.Groups[1].Value.Trim()] = m.Groups[2].Value.Trim();
                                Response.Write("我已经学会了，您可以问我：" + m.Groups[1].Value.Trim());
                            }
                        }

                        else if (adminhashWenda.ContainsKey(Message))
                        {
                            Response.Write(adminhashWenda[Message].ToString());
                            return;
                        }
                        else if (hashQunWenda.ContainsKey(Message))
                        {
                            Response.Write(hashQunWenda[Message].ToString());
                            return;
                        }
                        else
                        {
                            com(Event, Qunid, Qunnid, Sender, Nick, Message, YZadmin(Sender));
                        }


                    }

                }
                else if (Event == "Reset")
                {//紧急重启
                    que.Enqueue(string.Format("3\n1\n1"));
                }
                else if (Event == "Update")
                {//更新机器人
                    que.Enqueue(string.Format("4\n1\n1"));
                }
            }
            else if (Request.RequestType.Equals("get", StringComparison.InvariantCultureIgnoreCase))
            {
                /*string  a = RobotApi.get("http://127.0.0.1:2010/Api?Key=QQrobot&SendType=SendMessage&QQnumber=测试", "utf-8");
                Response.Write(a);*/
                //string RobotQQ1 = Request.QueryString["RobotQQ"].Trim();
                try
                {
                    string Copyright1 = Request.QueryString["Copyright"].Trim();
                    if (Copyright1 == Copyright)
                    {
                        string RobotQQ2 = Request.QueryString["RobotQQ"].Trim();
                        /*if (NetRobotApi.RobotApi.YZrobot(RobotQQ2, QQ) == "T")
                        {*/
                        if (que.Count > 0)
                        {
                            Response.Write(que.Dequeue() as string);
                        }

                        /* }*/
                        return;

                    }
                    else
                    {
                        Response.Write("密匙验证错误！您没有权限使用该接口！");
                        return;
                    }
                }
                catch
                {
                }

                try
                {
                    string config = Request.QueryString["config"].Trim();
                    if (config == "Qr")
                    {
                        Response.Write("Copyright © Vckers.com 2010-2012");
                        return;
                    }
                }
                catch
                {

                }

            }
        }
    }
}
