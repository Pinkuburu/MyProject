using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using QQRobot.SDK;
using System.Diagnostics;

namespace CmdPlugins
{
    public class CmdPluginsClass : ClientSdk
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
        }
        /// <summary>
        /// 插件名称
        /// </summary>
        public override String PluginName { get { return "CmdPlugins"; } }
        /// <summary>
        /// 插件唯一名称（英文+数字）
        /// </summary>
        public override String PluginKey { get { return "CmdPlugins"; } }
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
        public override string Description { get { return "您的说明"; } }
        /// <summary>
        /// 安装协议，不需要请留空
        /// </summary>
        public override string Agreement { get { return "安装协议"; } }

        /// <summary>
        /// 运行CMD命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns></returns>
        public static string Cmd(string[] cmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.AutoFlush = true;
            for (int i = 0; i < cmd.Length; i++)
            {
                p.StandardInput.WriteLine(cmd[i].ToString());
            }
            p.StandardInput.WriteLine("exit");
            string strRst = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            return strRst;
        }

        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="ProcName">进程名称</param>
        /// <returns></returns>
        public static bool CloseProcess(string ProcName)
        {
            bool result = false;
            System.Collections.ArrayList procList = new System.Collections.ArrayList();
            string tempName = "";
            int begpos;
            int endpos;
            foreach (System.Diagnostics.Process thisProc in System.Diagnostics.Process.GetProcesses())
            {
                tempName = thisProc.ToString();
                begpos = tempName.IndexOf("(") + 1;
                endpos = tempName.IndexOf(")");
                tempName = tempName.Substring(begpos, endpos - begpos);
                procList.Add(tempName);
                if (tempName == ProcName)
                {
                    if (!thisProc.CloseMainWindow())
                        thisProc.Kill(); // 当发送关闭窗口命令无效时强行结束进程
                    result = true;
                }
            }
            return result;
        }
    }
}
