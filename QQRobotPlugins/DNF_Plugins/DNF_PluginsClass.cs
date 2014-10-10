using System;
using System.Drawing;

using QQRobot.SDK;
using QQRobot.SDK.Events;

namespace DNF_Plugins
{
    /// <summary>
    /// 标准空类库
    /// </summary>
    public class DNF_PluginsClass : ClientSdk
    {
        #region 插件安装
        /// <summary>
        /// 安装插件函数（安装时调用，安装数据库之类的）
        /// </summary>
        public override void Install()
        {
            base.Install();
        }
        #endregion

        #region 初始化插件
        /// <summary>
        /// 初始化插件（开启插件时调用）
        /// </summary>
        public override void Init()
        {
            //加载您需要处理的事件
            //this.ReceiveKickOut += new RobotEventHandler(OutPut1);
            this.ReceiveNormalIM += new EventHandler<QQRobot.SDK.Events.ReceiveNormalIM>(DNF_PluginsClass_ReceiveNormalIM);
            this.ReceiveClusterIM += new EventHandler<QQRobot.SDK.Events.ReceiveClusterIM>(DNF_PluginsClass_ReceiveClusterIM);
        }

        void DNF_PluginsClass_ReceiveNormalIM(object sender, ReceiveNormalIM e)
        {
            Random rnd = new Random();

            switch (e.Message)
            {
                case "@d4":
                    SendMessage(e.Sender, e.SendName + "，您摇出的数字是：" + rnd.Next(1, 5).ToString());
                    break;
                case "@d6":
                    SendMessage(e.Sender, e.SendName + "，您摇出的数字是：" + rnd.Next(1, 7).ToString());
                    break;
                case "@d8":
                    SendMessage(e.Sender, e.SendName + "，您摇出的数字是：" + rnd.Next(1, 9).ToString());
                    break;
                case "@d10":
                    SendMessage(e.Sender, e.SendName + "，您摇出的数字是：" + rnd.Next(1, 11).ToString());
                    break;
                case "@d12":
                    SendMessage(e.Sender, e.SendName + "，您摇出的数字是：" + rnd.Next(1, 13).ToString());
                    break;
                case "@d20":
                    SendMessage(e.Sender, e.SendName + "，您摇出的数字是：" + rnd.Next(1, 21).ToString());
                    break;
            }
        }

        void DNF_PluginsClass_ReceiveClusterIM(object sender, QQRobot.SDK.Events.ReceiveClusterIM e)
        {
            Random rnd = new Random();

            switch (e.Message)
            {
                case "@d4":
                    SendMessage(e.Sender, e.SendName + "，您摇出的数字是：" + rnd.Next(1, 5).ToString());
                    break;
                case "@d6":
                    SendMessage(e.Sender, e.SendName + "，您摇出的数字是：" + rnd.Next(1, 7).ToString());
                    break;
                case "@d8":
                    SendMessage(e.Sender, e.SendName + "，您摇出的数字是：" + rnd.Next(1, 9).ToString());
                    break;
                case "@d10":
                    SendMessage(e.Sender, e.SendName + "，您摇出的数字是：" + rnd.Next(1, 11).ToString());
                    break;
                case "@d12":
                    SendMessage(e.Sender, e.SendName + "，您摇出的数字是：" + rnd.Next(1, 13).ToString());
                    break;
                case "@d20":
                    SendMessage(e.Sender, e.SendName + "，您摇出的数字是：" + rnd.Next(1, 21).ToString());
                    break;
            }
        }
        #endregion

        #region 设置窗体
        /// <summary>
        /// 加载设置窗体（点击设置时调用）
        /// </summary>
        public override void SetForm()
        {
            //加载您的设置窗体
        }
        #endregion

        #region 插件关闭
        /// <summary>
        /// 插件关闭调用函数（插件被关闭时调用，用于结束线程）
        /// </summary>
        public override void OnClosePlug()
        {
            base.OnClosePlug();
        }
        #endregion

        #region 插件卸载
        /// <summary>
        /// 插件卸载（卸载插件时调用，用于清理数据库）
        /// </summary>
        public override void UnInstall()
        {
            base.UnInstall();
        }
        #endregion

        #region 更新插件
        /// <summary>
        /// 更新插件
        /// </summary>
        public override void Update()
        {
            base.Update();
        }
        #endregion

        #region 插件信息（必填）
        /// <summary>
        /// 插件名称
        /// </summary>
        public override String PluginName { get { return "龙与地下城筛子程序"; } }
        /// <summary>
        /// 插件唯一名称（英文+数字）
        /// </summary>
        public override String PluginKey { get { return "DNF_PluginsClass"; } }
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
        public override string Description { get { return "龙与地下城筛子程序\r\n调用命令：\r\n@d4 //1-4里随机出一个数\r\n@d6 //1-6里面随机出一个数\r\n@d8 //1-8里面随机出一个数\r\n@d10 //1-10里面随机出一个数\r\n@d12 //1-12里面随机出一个数\r\n@d20 //1-20里面随机出一个数"; } }
        /// <summary>
        /// 安装协议，不需要请留空
        /// </summary>
        public override string Agreement { get { return "DNF_Plugins"; } }

        /// <summary>
        /// 开发者Token 如果没有可以不写
        /// </summary>
        public override string DevelopmentToken
        {
            get
            {
                return "";
            }
        } 
        #endregion
    }
}
