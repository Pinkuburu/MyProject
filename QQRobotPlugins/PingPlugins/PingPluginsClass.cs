using System;
using System.Collections.Generic;
using System.Text;
using QQRobot.SDK;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Threading;

namespace PingPlugins
{
    /// <summary>
    /// 标准空类库
    /// </summary>
    public class PingPluginsClass : ClientSdk
    {
        /// <summary>
        /// 安装插件
        /// </summary>
        public override bool Install()
        {
            return true;
        }
        /// <summary>
        /// 初始化插件
        /// </summary>
        public override void Init()
        {
            //加载您需要处理的事件
            this.ReceiveKickOut += new RobotEventHandler(OutPut1);
            this.ReceiveNormalIM += new RobotEventHandler(PingPluginsClass_ReceiveNormalIM);
        }        
        /// <summary>
        /// 加载设置窗体
        /// </summary>
        public override void SetForm()
        {
            new set().ShowDialog();
        }
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="e"></param>
        public void OutPut1(MessageClass e)
        {
            //写你的处理方法
            ErrorMessage = "您掉线了";
        }

        public void PingPluginsClass_ReceiveNormalIM(MessageClass e)
        {
            string[] Arg = e.Message.Split(' ');
            string strResult = null;

            switch (Arg[0].ToLower())
            {
                case "@ping"://PING命令
                    if (Arg.Length == 3 || Arg.Length == 2)
                    {
                        int intSendCount = 0;
                        if (Arg.Length == 2)
                        {
                            intSendCount = 4;
                        }
                        else
                        {
                            intSendCount = Convert.ToInt32(Arg[2]);
                        }
                        strResult = Pings(Arg[1].ToString(), intSendCount);
                        SendMessage(e.Sender, strResult);
                    }
                    else
                    {
                        SendMessage(e.Sender, e.Nick + "，" + "参数错误。\rPING命令的使用方法：@ping 地址/IP 发包数（选填项不填默认为4个包）\r例：@ping www.baidu.com");
                    }
                    break;
            }
        }
        /// <summary>
        /// 插件名称
        /// </summary>
        public override String PluginName { get { return "PingPlugins"; } }
        /// <summary>
        /// 插件唯一名称（英文+数字）
        /// </summary>
        public override String PluginKey { get { return "PingPlugins"; } }
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
        public override string Description { get { return "软件使用说明：\r\n调用命令：@ping\r\n操作说明：@ping 地址/IP 发包数（选填项不填默认为4个包）\r\n例：@ping www.baidu.com"; } }
        /// <summary>
        /// 安装协议，不需要请留空
        /// </summary>
        public override string Agreement { get { return "PingPlugins"; } }

        #region 发送Ping命令 Pings(string strAddress, int intSendCount)
        /// <summary>
        /// 发送Ping命令
        /// </summary>
        /// <param name="strAddress"></param>
        /// <param name="intSendCount"></param>
        /// <returns></returns>
        public string Pings(string strAddress, int intSendCount)
        {
            Ping PingInfo = new Ping();
            string strContent = null;
            int intReplyCount = 0;
            int intLostCount = 0;
            int TimeOut = 1000;

            if (intSendCount > 20)
            {
                intSendCount = 20;
            }
            for (int i = 0; i < intSendCount; i++)
            {
                PingReply reply = PingInfo.Send(strAddress, TimeOut);
                if (reply.Status == IPStatus.Success)
                {
                    intReplyCount++;
                }
                else
                {
                    intLostCount++;
                }
                Thread.Sleep(1000);
            }
            strContent = string.Format("数据包:已发送={0}，已接收={1}，丢失={2}  ({3}% 丢失)", intSendCount, intReplyCount, intLostCount, intLostCount * 100 / intSendCount);
            return strContent;
        }
        #endregion 发送Ping命令 Pings(string strAddress, int intSendCount)
    }
}
