using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace QQRobot
{
    class QQClass
    {
        //声明序列信息
        public int i = 12;
        //声明HTTP请求类
        private static WebClient HTTPproc = new WebClient();
        //声明随机数字类
        private static RandStr rnd = new RandStr(true, false, false, false);
        //声明变量
        public string strKey = null;
        public string resultString = null;
        public string strQQHost = "http://web-proxy.qq.com/conn_s";
        public string strQQ = null;
        public string strAdminQQ = "182536608";
        public bool boolDebug = true;
        
        public void Login(string strQQ,string strPwd)
        {
            //变量声明
            string strMsg = null;
            string strReMsg = null;
            string strRequest = null;
            string strContent = null;
            string strCookies = null;
            string strSkey = null;
            string strPtwebqq = null;
            string strPost = null;

            string[] arrResult = { };
            HTTPproc.Encoding = System.Text.Encoding.UTF8;

            this.strQQ = strQQ;
            strRequest = "http://ptlogin2.qq.com/check?uin=" + strQQ + "&appid=1002101&r=" + rnd.GetRandStr(13);
            strContent = HTTPproc.GetHtml(strRequest);

            try
            {
                this.resultString = Regex.Match(strContent, @"'\d','.*'").Value.Replace("\'", "");//'0','!MLF'
                arrResult = this.resultString.Split(',');
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("没有获取到验证码正则出错，请检查！");
                Console.WriteLine(ex);
            }
            Console.WriteLine("获取的验证码:" + arrResult[1]);

            strCookies = HTTPproc.ResponseHeaders.GetValues(3).GetValue(0).ToString();

            try
            {
                this.resultString = Regex.Replace(strCookies, ";.*", "").Replace("ptvfsession=","");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            HTTPproc.CookieContainer.Add(new Uri("http://ptlogin2.qq.com"), new Cookie("ptvfsession", this.resultString));
            strRequest = "http://ptlogin2.qq.com/login?u=" + strQQ + "&p=" + qqPwdEncrypt.Encrypt(strPwd, arrResult[1]) + "&verifycode=" + arrResult[1] + "&remember_uin=1&aid=1002101&u1=http%3A%2F%2Fweb.qq.com%2Fmain.shtml%3Fdirect__2&h=1&ptredirect=1&ptlang=2052&from_ui=1&pttype=1&dumy=&fp=loginerroralert";

            strContent = HTTPproc.GetHtml(strRequest);

            try
            {
                this.resultString = Regex.Match(strContent, @"'\d','.*'").Value.Replace("\'", "");//'3','0','','0','您输入的密码有误，请重试。'
                arrResult = this.resultString.Split(',');
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("没有获取到验证码正则出错，请检查！");
                Console.WriteLine(ex);
            }

            #region 登录返回值判定
            if (arrResult[0] == "0")        //0：登录成功!
            {
                Console.WriteLine(arrResult[4]);
                strSkey = HTTPproc.ResponseHeaders.GetValues(1).GetValue(2).ToString();
                //strPtcz = HTTPproc.ResponseHeaders.GetValues(1).GetValue(9).ToString();

                for (int j = 0; j < HTTPproc.ResponseHeaders.GetValues(1).Length; j++)
                {
                    if (HTTPproc.ResponseHeaders.GetValues(1).GetValue(j).ToString().IndexOf("ptwebqq") > -1)
                    {
                        strPtwebqq = HTTPproc.ResponseHeaders.GetValues(1).GetValue(j).ToString();
                    }
                }
            }
            else if (arrResult[0] == "1")   //1：系统繁忙，请稍后重试。
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "2")   //2：已经过期的QQ号码。
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "3")   //3：您输入的密码有误，请重试。
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "4")   //4：您输入的验证码有误，请重试。
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "5")   //5：校验失败。
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "6")   //6：密码错误。如果您刚修改过密码, 请稍后再登录.
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "7")   //7：您的输入有误, 请重试。
            {
                Console.WriteLine(arrResult[4]);
            }
            else                            //8：您的IP输入错误的次数过多，请稍后再试。
            {
                Console.WriteLine("未知错误！");
            }
            Console.WriteLine(arrResult[1]);

            #endregion 登录返回值判定

            strSkey = Regex.Replace(strSkey, ";.*", "").Replace("skey=", "");
            strPtwebqq = Regex.Replace(strPtwebqq, ";.*", "").Replace("ptwebqq=", "");

            strPost = this.strQQ + ";22;0;00000000;" + strSkey + ";" + strPtwebqq + ";0;";
            strContent = HTTPproc.Post(this.strQQHost, strPost);
            Console.WriteLine(strContent);

            string[] arrKey = strContent.Split(';');
            this.strKey = arrKey[4].ToString();
            
            //读取用户分组            
            Console.WriteLine(GetUserGroup());

            //读取好友列表 
            strPost = this.strQQ + ";06;2;" + this.strKey + ";" + this.strQQ + ";" + this.strQQ + ";5c;3;" + this.strKey + ";88;" + this.strQQ + ";67;4;" + this.strKey + ";03;1;" + this.strQQ + ";" + this.strQQ + ";58;5;" + this.strKey + ";0;" + this.strQQ + ";26;6;" + this.strKey + ";0;0;" + this.strQQ + ";3e;7;" + this.strKey + ";4;0;" + this.strQQ + ";65;8;" + this.strKey + ";02;" + this.strQQ + ";" + this.strQQ + ";1d;9;" + this.strKey + ";" + this.strQQ + ";00;10;" + this.strKey + ";";
            strContent = HTTPproc.Post(this.strQQHost, strPost);
            Console.WriteLine(strContent);
            Run();

            #region 老版消息循环
            //string[] arrReMsg;

            //while (i < 100)
            //{
            //    Console.WriteLine("正在等待接收消息:{0}", i);
            //    strContent = HTTPproc.Post(this.strQQHost, this.strQQ + ";00;" + this.i.ToString() + ";" + this.strKey + ";");

            //    arrReMsg = strContent.Split(';');
            //    if (arrReMsg.Length > 10)
            //    {
            //        if (arrReMsg[1].ToString() == "17")
            //        {
            //            //发送消息
            //            //this.i++;
            //            strPost = this.strQQ + ";16;" + this.i.ToString() + ";" + this.strKey + ";" + arrReMsg[3].ToString() + ";0b;528;" + UrlEncode(arrReMsg[7].ToString(), "UTF-8") + ";0a00000010%E5%AE%8B%E4%BD%93;";
            //            strReMsg = HTTPproc.Post(this.strQQHost, strPost);
            //            Console.WriteLine("发送消息:{0}", strReMsg);
            //            //1307364337;16;119;165fad25;182536608;0b;528;%E4%BD%A0%E5%A5%BD;0a00000010%E5%AE%8B%E4%BD%93;

            //            //this.i++;
            //            strPost = this.strQQ + ";17;" + arrReMsg[2].ToString() + ";" + this.strKey + ";" + arrReMsg[3].ToString() + ";" + this.strQQ + ";" + arrReMsg[4].ToString() + ";1;" + arrReMsg[10].ToString() + ";";
            //            strMsg = HTTPproc.Post(this.strQQHost, strPost);//" + this.strQQ + ";00;" + this.i.ToString() + ";" + this.strKey + ";");
            //            //
            //            Console.WriteLine("发送确认消息:{0}", strMsg);
            //        }
            //    }
            //    Console.WriteLine(strContent);
            //    this.i++;
            //}
            #endregion
        }

        #region 读取用户分组 GetUserGroup()
        /// <summary>
        /// 读取用户分组
        /// </summary>
        /// <returns></returns>
        public string GetUserGroup()
        {
            string strUserGroup = null;
            string strPost = null;
            //读取分组
            strPost = this.strQQ + ";3c;0;" + this.strKey + ";1;" + this.strQQ + ";00;1;" + this.strKey + ";";
            //strPost = this.strQQ + ";3c;0;" + this.strKey + ";1;";
            strUserGroup = HTTPproc.Post(this.strQQHost, strPost);
            return "分组信息:" + strUserGroup;
        }

        #endregion 读取用户分组 GetUserGroup()

        #region 读取好友列表 GetFriendList()
        /// <summary>
        /// 读取好友列表
        /// </summary>
        /// <returns></returns>
        public string GetFriendList()
        {
            string strFriendList = null;
            string strPost = null;
            //读取好友列表
            strPost = this.strQQ + ";58;0;" + this.strKey + ";0;";
            strFriendList = HTTPproc.Post(this.strQQHost, strPost);
            return strFriendList;
        }

        #endregion 读取好友列表 GetFriendList()

        #region Robot守护进程 Run()
        /// <summary>
        /// Robot守护进程
        /// </summary>
        public void Run()
        {
            bool boolRun = true;
            string strContent = null;
            string[] arrTemp = { };
            string[] arrCommand = { };
            while (boolRun)
            {
                try
                {
                    strContent = HTTPproc.Post(this.strQQHost, this.strQQ + ";00;" + this.i.ToString() + ";" + this.strKey + ";");
                }
                catch
                {
                    ShowCommand("重试消息轮询", "Debug");
                    strContent = HTTPproc.Post(this.strQQHost, this.strQQ + ";00;" + this.i.ToString() + ";" + this.strKey + ";");
                }
                ShowCommand("消息轮询: " + strContent, "Debug");
                arrCommand = strContent.Split('');

                try
                {
                    foreach (String strMessage in arrCommand)
                    {
                        arrTemp = strMessage.Split(';');
                        switch (arrTemp[1].ToString())
                        {
                            case "16":
                                SendMessage(arrTemp[3].ToString(), arrTemp[7].ToString());
                                break;
                            case "17":
                                ReMessage(strMessage);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch
                {
 
                }
                this.i++;
            }
        }
        #endregion Robot守护进程 Run()

        #region 发送消息 SendMessage(string strQQ, string strMsg)
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="strQQ"></param>
        /// <param name="strMsg"></param>
        private void SendMessage(string strQQ, string strMsg)
        {
            string strContent = null;
            string strPost = null;

            this.i++;
            //发送消息
            strPost = this.strQQ + ";16;" + this.i.ToString() + ";" + this.strKey + ";" + strQQ + ";0b;528;" + UrlEncode(strMsg, "UTF-8") + ";0a00000010%E5%AE%8B%E4%BD%93;";
            strContent = HTTPproc.Post(this.strQQHost, strPost);
            ShowCommand("发送消息:" + strContent, "Debug");
        }
        #endregion 发送消息 SendMessage(string strQQ, string strMsg)

        #region 接收消息 ReMessage(string strReMsg)
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="strReMsg"></param>
        private void ReMessage(string strReMsg)
        {
            string[] arrReMsg = { };
            string strContent = null;
            string strPost = null;
            arrReMsg = strReMsg.Split(';');

            //显示接收消息
            ShowCommand("用户消息:" + arrReMsg[3].ToString() + "说:" + arrReMsg[7].ToString(), "User");

            //消息处理
            Command(arrReMsg[3].ToString(),arrReMsg[7].ToString());

            this.i++;
            //接受消息处理
            strPost = this.strQQ + ";17;" + arrReMsg[2].ToString() + ";" + this.strKey + ";" + arrReMsg[3].ToString() + ";" + this.strQQ + ";" + arrReMsg[4].ToString() + ";1;" + arrReMsg[10].ToString() + ";";//" + this.strQQ + ";00;" + this.i.ToString() + ";" + this.strKey + ";";
                        //1349836289;17;                         24634;           0878813c;                     182536608;        1349836289;                        556257;1;3565623212;
            strContent = HTTPproc.Post(this.strQQHost, strPost);
            ShowCommand("发送消息:" + strContent, "Debug");
        }
        #endregion 接收消息 ReMessage(string strReMsg)

        private void Command(string strQQ, string strCmd)
        {
            string strContent = null;
            string[] arrCmd = { };
            if (strCmd.IndexOf("@") == 0)
            {
                arrCmd = strCmd.Trim().Split(' ');
                switch (arrCmd[0].ToString())
                {
                    case "@":
                        ShowCommand("显示机器人命令", "Cmd");
                        strContent = "您好，我就是传说中地扣扣机器人%0A" +
                                     "=========您可以使用如下命令=========%0A" +
                                     "@Sign              修改机器人签名%0A" +
                                     "@NickName     修改机器人昵称%0A" +
                                     "@Status          更改状态(Online,Offline,Hidden,Exit)";
                        SendMessage(strQQ, strContent);
                        break;
                    case "@Sign":   //修改签名
                        ModifySign(arrCmd[1].ToString());
                        ShowCommand("签名修改为:" + arrCmd[1].ToString(), "Cmd");
                        SendMessage(strQQ, "签名修改为:" + arrCmd[1].ToString());
                        break;
                    case "@NickName":   //昵称签名
                        ModifyUserInfo(arrCmd[1].ToString());
                        ShowCommand("昵称修改为:" + arrCmd[1].ToString(), "Cmd");
                        SendMessage(strQQ, "昵称修改为:" + arrCmd[1].ToString());
                        break;
                    case "@Status":     //修改在线状态
                        OnlineStatus(arrCmd[1].ToString());
                        ShowCommand("修改当前状态为:" + arrCmd[1].ToString(), "Cmd");
                        SendMessage(strQQ, "修改当状态为:" + arrCmd[1].ToString());
                        break;
                    case "@Add":        //添加好友
                        AddFriend(arrCmd[1].ToString());
                        ShowCommand("添加好友:" + arrCmd[1].ToString(), "Cmd");
                        SendMessage(strQQ, "添加好友:" + arrCmd[1].ToString());
                        break;
                    default:
                        ShowCommand("您输入的命令有误或格式不对请检查", "Cmd");
                        SendMessage(strQQ, "您输入的命令有误或格式不对请检查");//%0A 换行
                        break;
                }
            }
        }

        #region 修改昵称 ModifyUserInfo(string strNickName)
        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="strNickName"></param>
        private void ModifyUserInfo(string strNickName)
        {
            string strPost = null;
            this.i++;
            strPost = this.strQQ + ";04;" + this.i + ";" + this.strKey + ";" + strNickName + "------0---------1--528------000-000;";
            //strPost = this.strQQ + ";04;" + this.i + ";" + this.strKey + ";" + strNickName + "中国山东----0---------1--528---青岛--000-000;";
            //1349836289;04;281;d0dd2dac;Test中国山东----0---------1--528---青岛--000-000;
            HTTPproc.Post(this.strQQHost, strPost);
        }
        #endregion 修改昵称 ModifyUserInfo(string strNickName)

        #region 修改签名 ModifySign(string strSign)
        /// <summary>
        /// 修改签名
        /// </summary>
        /// <param name="strSign">签名内容</param>
        private void ModifySign(string strSign)
        {
            string strPost = this.strQQ + ";67;" + this.i + ";" + this.strKey + ";01;" + strSign + ";";
            //1349836289;67;512;ad7e15a4;01;Tests;
            //1349836289;67;14;701f4ae8;01;Tests;
            //1349836289;67;13;eac1407d;01;Cupid;
            HTTPproc.Post(this.strQQHost, strPost);
        }
        #endregion 修改签名 ModifySign(string strSign)

        #region 显示命令行 ShowCommand(string strContent, string strColor)
        /// <summary>
        /// 显示命令行
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="strColor"></param>
        private void ShowCommand(string strContent, string strColor)
        {
            if (this.boolDebug)
            {
                switch (strColor)
                {
                    case "Debug":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(strContent);
                        break;
                    case "User":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(strContent);
                        break;
                    case "Cmd":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(strContent);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(strContent);
                        break;
                }                
            }
        }
        #endregion 显示命令行 ShowCommand(string strContent, string strColor)

        #region 修改在线状态 OnlineStatus(string strStatus)
        /// <summary>
        /// 修改在线状态
        /// </summary>
        /// <param name="strStatus"></param>
        private void OnlineStatus(string strStatus)
        {
            string strPost = null;
            switch (strStatus)
            {
                case "Online":
                    //1349836289;0d;25;e76d930c;10; 在线
                    strPost = this.strQQ + ";0d;" + this.i + ";" + this.strKey + ";10;";
                    HTTPproc.Post(this.strQQHost, strPost);
                    break;
                case "Offline":
                    //1349836289;0d;16;e76d930c;30; 离线
                    strPost = this.strQQ + ";0d;" + this.i + ";" + this.strKey + ";30;";
                    HTTPproc.Post(this.strQQHost, strPost);
                    break;
                case "Hidden":
                    //1349836289;0d;33;e76d930c;40; 隐身
                    strPost = this.strQQ + ";0d;" + this.i + ";" + this.strKey + ";40;";
                    HTTPproc.Post(this.strQQHost, strPost);
                    break;
                case "Exit":
                    //1349836289;0d;39;e76d930c;20; 离开
                    strPost = this.strQQ + ";0d;" + this.i + ";" + this.strKey + ";20;";
                    HTTPproc.Post(this.strQQHost, strPost);
                    break;
            }
        }
        #endregion 修改在线状态 OnlineStatus(string strStatus)

        #region 添加好友(主动) AddFriend(string strF_QQ, string strContent)
        /// <summary>
        /// 添加好友(主动)
        /// </summary>
        /// <param name="strF_QQ"></param>
        /// <param name="strContent"></param>
        private void AddFriend(string strF_QQ)
        {
            string strPost = null;
            //添加好友(主动)
            //1349836289;a8;33;701f4ae8;2;182536608;1;0;Test;
            //1349836289;a8;58;701f4ae8;2;182536608;1;0;Test;
            strPost = this.strQQ + ";a8;" + this.i + ";" + this.strKey + ";2;" + strF_QQ + ";1;0;" + UrlEncode("我是QQ机器人", "UTF-8") + ";";
            HTTPproc.Post(this.strQQHost, strPost);
        }

        #endregion 添加好友(主动) AddFriend(string strF_QQ, string strContent)

        //收到被对方加为好友(主动)
        //1349836289;80;51939;43;1307364337;1349836289;0;1288756278;  主动加好友并且对方同意添加好友时返回的确认
        //1349836289;80;23152;43;1307364337;1349836289;0;1288758359;  主动加好友并且对方同意添加好友时返回的确认

        //1349836289;80;28300;41;1307364337;1349836289;Test;1;1288770561;  被别人添加好友返回的验证信息
        //1349836289;80;15959;41;1307364337;1349836289;Test;1;1288770749;  被别人添加好友返回的验证信息

        //被添加好友处理
        //1349836289;a8;74;f60704dd;3;1307364337;0;	同意意思被添加好友发送
        //1349836289;81;32628;1307364337;10;3;	同意被添加好友后并且对方验证确认后返回

        //收到好友删除通知
        //1349836289;81;32530;1307364337;5122;3;
        //1349836289;81;38986;1307364337;5122;3;
        //1349836289;81;52870;1307364337;5122;3;



        //查询好友信息
        //1349836289;06;31;c41a0b3f;1307364337;
        //1349836289;06;160;c41a0b3f;1307364337;

        //查询好友信息
        //查询 
        //1349836289;0115;158;c41a0b3f;3;1307364337;
        //1349836289;0115;183;c41a0b3f;3;1301111111;
        //返回
        //1349836289;0115;158;3;0;1307364337;当乐下载;528;3;中国-山东-青岛;
        //返回
        //1349836289;0115;183;3;0;  --没有找到  暂无查找结果!

        //查询分组
        //1349836289;3c;0;2a0d205c;1;

        //读取用户等级信息
        //1349836289;5c;3;2a0d205c;88;
        //返回
        //1349836289;5c;3;88;0;0       ;3       ;0;2;
        //                    ;用户等级;在线天数; ;剩余升级天数;    

        //读取用户好友列表(返回好友QQ号，昵称)
        //1307364337;26;6;a3015db3;0;0;
        //返回
        //1307364337;26;6;0;4111852;12;29;0;小灰;0;182536608;297;24;0;小柏拉;1;790812856;432;22;0;过 客;1;800015686;0;0;255;XBA游戏网;0;907477127;399;19;1;乖猪猪抱抱KE;0;1093616771;558;24;1;YY;0;

        //读取用户好友列表(用户在线状态返回)
        //1349836289;58;5;2a0d205c;0;
        //返回
        //1349836289;58;5;0;182536608;0;0;20;3;

        #region QQ密码加密算法
        /// <summary>
        /// QQ密码加密算法
        /// </summary>
        private static class qqPwdEncrypt
        {
            /// <summary>
            /// 计算网页上QQ登录时密码加密后的结果
            /// </summary>
            /// <param name="pwd" />QQ密码</param>
            /// <param name="verifyCode" />验证码</param>
            /// <returns></returns>
            public static String Encrypt(string pwd, string verifyCode)
            {
                return (md5(md5_3(pwd).ToUpper() + verifyCode.ToUpper())).ToUpper();
            }
            /// <summary>
            /// 计算字符串的三次MD5
            /// </summary>
            /// <param name="s" /></param>
            /// <returns></returns>
            private static String md5_3(String s)
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);

                bytes = md5.ComputeHash(bytes);
                bytes = md5.ComputeHash(bytes);
                bytes = md5.ComputeHash(bytes);

                md5.Clear();

                string ret = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
                }

                return ret.PadLeft(32, '0');
            }
            /// <summary>
            /// 计算字符串的一次MD5
            /// </summary>
            /// <param name="s" /></param>
            /// <returns></returns>
            private static String md5(String s)
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);

                bytes = md5.ComputeHash(bytes);

                md5.Clear();

                string ret = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
                }

                return ret.PadLeft(32, '0');
            }
        }
        #endregion QQ密码加密算法

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

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.%";
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

        #region 随机生成 字数、数字、符号 RandStr
        /// <summary>
        /// 随机生成 字数、数字、符号
        /// </summary>
        public class RandStr
        {
            private string framerStr = null;
            private string numStr = "0123456789";
            private string upperStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            private string lowerStr = "abcdefghijklmnopqrstuvwxyz";
            private string markStr = @"`-=[];'\,./~!@#$%^&*()_+{}:""|<>?";
            private static Random myRandom = new Random();

            /// <summary>
            /// 如未提供参数构造,则默认由数字+小写字母构成
            /// </summary>
            public RandStr()
            {
                framerStr = numStr + lowerStr;
            }

            /// <summary>
            /// 构造函数,可指定构成的字符
            /// </summary>
            /// <param name="useNum">是否使用数字</param>
            /// <param name="useUpper">是否使用大写字母</param>
            /// <param name="useLower">是否使用小写字母</param>
            /// <param name="useMark">是否使用符号</param>
            public RandStr(bool useNum, bool useUpper, bool useLower, bool useMark)
            {
                // 如果试图构造不包含任何组成字符的类,则抛出异常
                if (!useNum && !useUpper && !useLower && !useMark)
                {
                    throw new ArgumentException("必须至少使用一种构成字符!");
                }
                else
                {
                    if (useNum)
                        framerStr += numStr;
                    if (useUpper)
                        framerStr += upperStr;
                    if (useLower)
                        framerStr += lowerStr;
                    if (useMark)
                        framerStr += markStr;
                }
            }

            /// <summary>
            /// 使用自定义的组成字符构造
            /// </summary>
            /// <param name="userStr">自定义字符</param>
            public RandStr(string userStr)
            {
                // 如果试图用空字符串构造类,则抛出异常
                if (userStr.Length == 0)
                {
                    throw new ArgumentException("请至少使用一个字符!");
                }
                else
                {
                    framerStr = userStr;
                }
            }

            /// <summary>
            /// 取得一个随机字符串
            /// </summary>
            /// <param name="length">取得随机字符串的长度</param>
            /// <returns>返回的随机字符串</returns>
            public string GetRandStr(int length)
            {
                // 获取的长度不能为0个或者负数个
                if (length < 1)
                {
                    throw new ArgumentException("字符长度不能为0或者负数!");
                }
                else
                {
                    // 如果只是获取少量随机字符串,
                    // 这样没有问题.
                    // 但如果需要短时间获取大量随机字符串的话,
                    // 这样可能性能不高.
                    // 可以改用StringBuilder类来提高性能,
                    // 需要的可以自己改一下 ^o^
                    string tempStr = null;
                    for (int i = 0; i < length; i++)
                    {
                        int randNum = myRandom.Next(framerStr.Length);
                        tempStr += framerStr[randNum];
                    }
                    return tempStr;
                }
            }
        }
        #endregion 随机生成 字数、数字、符号 RandStr

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
    }
}
