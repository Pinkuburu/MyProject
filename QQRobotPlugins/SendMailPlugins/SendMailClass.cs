using System;
using System.Collections.Generic;
using System.Text;
using QQRobot.SDK;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace SendMailPlugins
{
    /// <summary>
    /// 标准空类库
    /// </summary>
    public class SendMailClass : ClientSdk
    {
        SDK_Db db { get; set; }
        public static Data sData = new Data();
        /// <summary>
        /// 安装插件
        /// </summary>
        public override bool Install()
        {
            try
            {
                if (File.Exists(Application.StartupPath + "/HQmail.exe"))
                {
                    return true;
                }
                else
                {
                    Z_Db.WriteFile(Application.StartupPath + "/HQmail.exe", Properties.Resources.HQmail);
                    if (File.Exists(Application.StartupPath + "/HQmail.exe"))
                    {
                        return true;
                    }
                    else
                    {
                        ErrorMessage = "文件释放错误，请联系作者";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 初始化插件
        /// </summary>
        public override void Init()
        {
            //加载您需要处理的事件
            this.ReceiveKickOut += new RobotEventHandler(OutPut1);
            this.ReceiveNormalIM += new RobotEventHandler(SendMailClass_ReceiveNormalIM);
        }       
        /// <summary>
        /// 加载设置窗体
        /// </summary>
        public override void SetForm()
        {
            if (db == null)
            {
                db = new SDK_Db(this);
            }
            db.ReadData();
            if (db.GetObject("mail") != null)
            {
                sData.mail = db.GetObject("mail").ToString();
            }
            if (db.GetObject("pwd") != null)
            {
                sData.pwd = db.GetObject("pwd").ToString();
            }
            new set().ShowDialog();
            db.AddObject("mail", sData.mail);
            db.AddObject("pwd", sData.pwd);
            db.SavaData();
        }
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="e"></param>
        public void OutPut1(MessageClass e)
        {
            //写你的处理方法
            ErrorMessage = "您掉线了，呵呵";
        }

        public void SendMailClass_ReceiveNormalIM(MessageClass e)
        {
            string strResult = null;
            string[] Arg = e.Message.Split(' ');

            if (db == null)
            {
                db = new SDK_Db(this);
            }
            db.ReadData();
            if (db.GetObject("mail") != null)
            {
                sData.mail = db.GetObject("mail").ToString();
            }
            if (db.GetObject("pwd") != null)
            {
                sData.pwd = db.GetObject("pwd").ToString();
            }
            db.AddObject("fb_Data", sData.mail);
            db.AddObject("pwd", sData.pwd);
            db.SavaData();
            switch (Arg[0].ToLower())
            {
                case "@mail"://发送邮件
                    if (Arg.Length == 4)
                    {
                        strResult = SendMail(Arg[1], Arg[2], Arg[3]);
                        SendMessage(e.Sender, strResult);
                    }
                    else
                    {
                        SendMessage(e.Sender, e.Nick + "，" + "参数错误。\r邮件发送的使用方法：@mail 收件邮件 邮件主题 邮件内容\r例：@mail toMail@xxx.com 开会 三点开会");
                    }
                    break;
            }
        }
        /// <summary>
        /// 插件名称
        /// </summary>
        public override String PluginName { get { return "SendMailPlugins"; } }
        /// <summary>
        /// 插件唯一名称（英文+数字）
        /// </summary>
        public override String PluginKey { get { return "SendMailPlugins"; } }
        /// <summary>
        /// 作者
        /// </summary>
        public override string Author { get { return "Cupid"; } }
        /// <summary>
        /// 插件版本
        /// </summary>
        public override Version PluginVersion { get { return new Version(1, 0, 0, 0); } }
        /// <summary>
        /// 插件图片（暂时无效）
        /// </summary>
        public override Image PlugImage { get { return null; } }
        /// <summary>
        /// 插件说明
        /// </summary>
        public override string Description { get { return "软件使用说明：\r\n调用命令：@mail\r\n操作说明：@mail 收件邮件 邮件主题 邮件内容\r\n例：@mail toMail@xxx.com 开会 三点开会"; } }
        /// <summary>
        /// 安装协议，不需要请留空
        /// </summary>
        public override string Agreement { get { return "注意：要在设置内配置发件邮箱，要不无法发送邮件。"; } }


        #region 邮件发送 SendMail(string strMailTo, string strSubject, string strMailContent)
        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="strMailTo">接收邮箱</param>
        /// <param name="strSubject">邮件主题</param>
        /// <param name="strMailContent">邮件内容</param>
        /// <returns></returns>
        public string SendMail(string strMailTo, string strSubject, string strMailContent)
        {
            string strResult = null;
            if (sData.mail == "" || sData.pwd == "")
            {
                strResult = "没有配置发件人邮箱和发件人密码，请检查。";
                return strResult;
            }
            
            try
            {
                string strExePath = @"HQmail.exe";
                //strExePath = Path.GetFullPath(strExePath);
                ProcessStartInfo procInfo = new ProcessStartInfo(strExePath, string.Format(sData.mail + " " + sData.pwd + " {0} {1} {2}", strMailTo, strSubject, strMailContent));
                // 隐藏EXE运行的窗口
                procInfo.WindowStyle = ProcessWindowStyle.Hidden;
                // exe运行
                Process procBatch = Process.Start(procInfo);
                // 取得EXE运行后的返回值，返回值只能是整型

                strResult = "邮件发送成功~~";
            }
            catch
            {
                strResult = "邮件发送失败~~";
            }
            return strResult;
        }
        #endregion 邮件发送 SendMail(string strMailTo, string strSubject, string strMailContent)
    }
    [Serializable]//标记一定要
    public class Data
    {
        public string mail = "";
        public string pwd = "";
    }
}
