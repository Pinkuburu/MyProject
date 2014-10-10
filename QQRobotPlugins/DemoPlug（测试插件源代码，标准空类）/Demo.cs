using System;
using System.Drawing;
using QQRobot.SDK;
using TestClass;


namespace S_Demo_Plugin
{
    /// <summary>
    /// 标准空类库
    /// </summary>
    public class S_Demo_Plugin : ClientSdk
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
            #region 挂接所有需要的事件
            //挂接您需要的事件
            //例如
            //this.ReceiveNormalIM +=S_Demo_Plugin_ReceiveNormalIM;
            #endregion
        }
        #endregion

        #region 设置窗体
        /// <summary>
        /// 加载设置窗体（点击设置时调用）
        /// </summary>
        public override void SetForm()
        {
            //加载您的设置窗体
            new set().ShowDialog();
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
        public override String PluginName { get { return "示例插件"; } }
        /// <summary>
        /// 插件唯一名称（英文+数字）
        /// </summary>
        public override String PluginKey { get { return "com.iqqbot.demo"; } }
        /// <summary>
        /// 作者
        /// </summary>
        public override string Author { get { return "天使"; } }
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
        public override string Description { get { return "没有说明"; } }
        /// <summary>
        /// 安装协议，不需要请留空
        /// </summary>
        public override string Agreement { get { return ""; } }

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
