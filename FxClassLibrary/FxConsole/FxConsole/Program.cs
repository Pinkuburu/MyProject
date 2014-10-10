using System;
using System.Collections.Generic;
using System.Text;
using FxLibrary;
using FxModule;
using System.Threading;

namespace Management
{
    class Program
    {
        static void Main(string[] args)
        //static void Main()
        {
            string strMobileNo = "";
            string strTimer = "";
            string strURI = "";
            string strMsg = "";
            string strCmdId = "";
            string strResult = "";
            string strSystemNotify = "";
            string strUserInfo = "";
            string strSIDValue = "sid";
            string strServerName = "";
            string strServerStatus = "";

            int intSID = 0;

            Fetion fx = new Fetion();
            DateTime dt = DateTime.Now;

            switch (args[0].ToString())
            {
                case "evResult":          //执行结果反馈
                    strMobileNo = args[1].ToString();
                    strCmdId = args[2].ToString();
                    strResult = fx.cmdResult(args[3]);

                    Console.WriteLine("================= [执行结果反馈] 开始 ================= \r\n");                    
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>> " + strMobileNo + " <<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("命令ID:" + strCmdId + "\r\n");
                    Console.WriteLine("命令结果:" + strResult + "\r\n");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("================= [执行结果反馈] 退出 ================= \r\n");
                    break;
                case "evNotify":          //通用的系统通知，该插件把飞信服务器传递过来的原始数据转发给插件。
                    strMobileNo = args[1].ToString();
                    strSystemNotify = fx.sysNotify(args[2]);

                    Console.WriteLine("========= [传递机器人不识别的原始数据包] 开始 ========= \r\n");                    
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>> " + strMobileNo + " <<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("内容:" + strSystemNotify + "\r\n");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("========= [传递机器人不识别的原始数据包] 退出 ========= \r\n");
                    break;
                case "evBuddyInvite":     //对方发起对话时（双击打开窗口时，一般此时可以发送机器人菜单）
                    strMobileNo = args[1].ToString();
                    strURI = fx.readSendUser(args[2]);

                    Console.WriteLine("=================== [窗口事件] 开始 =================== \r\n");                    
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>> " + strMobileNo + " <<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("用户SIP:" + strURI + "\r\n");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("=================== [窗口事件] 退出 =================== \r\n");
                    break;
                case "evBuddyData":       //buddy资料传递
                    strMobileNo = args[1].ToString();
                    strUserInfo = fx.userInfo(args[2]);
                    intSID = Convert.ToInt32(fx.ReadXmlTextValue(strUserInfo, strSIDValue));

                    Console.WriteLine("=================== [资料传递] 开始 =================== \r\n");                    
                    if (intSID == 0)
                    {
                        Console.WriteLine("！！！！用户SID出错:" + intSID + "！！！！\r\n");
                    }
                    else
                    {
                        if (SqlLibrary.Fx_AddNewUser(intSID) == 1)
                        {
                            Console.WriteLine("！！！！添加新用户:" + intSID + "成功 ！！！！\r\n");
                        }
                        else
                        {
                            Console.WriteLine("！！！！此用户:" + intSID + "已添加 ！！！！\r\n");
                        }
                    }

                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>> " + strMobileNo + " <<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("用户资料:" + strUserInfo + "\r\n");
                    Console.WriteLine("用户SID:" + intSID + "\r\n");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("=================== [资料传递] 退出 =================== \r\n");
                    break;
                case "evNewUserRequest":  //有新的好友加入（为了突破人数限制，此时可以在保留有关数据后，删除该用户）
                    strMobileNo = args[1].ToString();
                    strURI = fx.readSendUser(args[2]);

                    Console.WriteLine("================ [有新的好友加入] 开始 ================ \r\n");                    
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>> " + strMobileNo + " <<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("用户SIP:" + strURI + "\r\n");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("================ [有新的好友加入] 退出 ================ \r\n");
                    break;
                case "evMessage":         //消息
                    strMobileNo = args[1].ToString();       //手机号
                    strURI = fx.readSendUser(args[2]);      //读取用户SPI号
                    strMsg = fx.readSMSContent(args[3]);    //读取用户短消息

                    Console.WriteLine("====================== [消息] 开始 ==================== \r\n");                    
                    Console.WriteLine("手机号:" + strMobileNo + "\r\n");
                    Console.WriteLine("用户SIP:" + strURI + "\r\n");
                    Console.WriteLine("用户消息:" + strMsg + "\r\n");
                    Fetion.SaveMessage(strURI, strMsg);
                    Console.WriteLine("====================== [消息] 退出 ==================== \r\n");
                    break;
                case "evTimer":           //10秒钟激活一次
                    strMobileNo = args[1].ToString();       //手机号
                    strTimer = args[2].ToString();          //运行至今的秒数

                    Console.WriteLine("==================== [计时器] 开始 ==================== \r\n");                    
                    Console.WriteLine("手机号:" + strMobileNo + " 运行时间:" + strTimer + " 秒\r\n");                    
                    Console.WriteLine("==================== [计时器] 退出 ==================== \r\n");
                    Fetion.SendMessage();
                    Fetion.RunTask();
                    break;
                case "evSysMessage":      //系统消息
                    strMobileNo = args[1].ToString();       //手机号
                    strTimer = fx.sysMessage(args[2]);      //系统消息内容

                    Console.WriteLine("=================== [系统消息] 开始 =================== \r\n");                    
                    Console.WriteLine("手机号:" + strMobileNo + "\r\n");
                    Console.WriteLine("系统消息:" + strTimer + "\r\n");
                    Console.WriteLine("=================== [系统消息] 退出 =================== \r\n");
                    break;
                case "evServerError":     //服务器预警调用
                    strServerName = args[1].ToString();     //服务器名称
                    strServerStatus = args[2].ToString();   //服务器状态
                    SqlLibrary.Fx_AddServerRec(strServerName, strServerStatus);

                    //接收服务器预警信息的飞信号码
                    Module.AddServerEW("660271316", strServerName, strServerStatus);//韩志伟移动
                    Thread.Sleep(2000);
                    Module.AddServerEW("432525523", strServerName, strServerStatus);//宋永凯移动

                    Console.WriteLine("================== [服务器预警] 开始 ================== \r\n");
                    Console.WriteLine("服务器名称:" + strServerName + " 服务器状态:" + strServerStatus + "\r\n");
                    Console.WriteLine("预警时间:" + dt + "\r\n");
                    Console.WriteLine("================== [服务器预警] 退出 ================== \r\n");
                    break;
                default:
                    Console.WriteLine("NotEventCase");
                    break;
            }
        }
    }
}