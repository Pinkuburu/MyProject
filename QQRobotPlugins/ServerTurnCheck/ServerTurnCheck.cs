using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

using QQRobot.SDK;
using QQRobot.SDK.Events;

namespace ServerTurnCheck
{
    public class ServerTurnCheck : ClientSdk
    {
        SDK_Db db { get; set; }
        ServerCheck sc = new ServerCheck();
        public static Data sData = new Data();

        #region 插件安装
        /// <summary>
        /// 安装插件函数（安装时调用，安装数据库之类的）
        /// </summary>
        public override void Install()
        {
            //try
            //{
            //    if (File.Exists(Application.StartupPath + "/databaseitem.dll") & File.Exists(Application.StartupPath + "/servermanager.dll"))
            //    {
            //        base.Install();
            //    }
            //    else
            //    {
            //        Z_Db.WriteFile(Application.StartupPath + "/databaseitem.dll", Properties.Resources.databaseitem);
            //        Z_Db.WriteFile(Application.StartupPath + "/servermanager.dll", Properties.Resources.servermanager);
            //        if (File.Exists(Application.StartupPath + "/databaseitem.dll") & File.Exists(Application.StartupPath + "/servermanager.dll"))
            //        {
            //            base.Install();
            //        }
            //        else
            //        {
            //            //ErrorMessage = "文件释放错误，请联系作者";
                        
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //ErrorMessage = ex.ToString();
                
            //}
            base.Install();
        }
        #endregion

        #region 初始化插件
        /// <summary>
        /// 初始化插件
        /// </summary>
        public override void Init()
        {
            //加载您需要处理的事件
            this.ReceiveNormalIM += new EventHandler<QQRobot.SDK.Events.ReceiveNormalIM>(ServerTurnCheck_ReceiveNormalIM);
        }

        void ServerTurnCheck_ReceiveNormalIM(object sender, ReceiveNormalIM e)
        {
            if (db == null)
            {
                db = new SDK_Db(this);
            }
            db.ReadData();
            if (db.GetObject("fb_Data") != null)
            {
                sData.fb_Data = db.GetObject("fb_Data").ToString();
            }
            if (db.GetObject("bb_Data") != null)
            {
                sData.bb_Data = db.GetObject("bb_Data").ToString();
            }
            db.AddObject("fb_Data", sData.fb_Data);
            db.AddObject("bb_Data", sData.bb_Data);
            db.SavaData();

            string[] Arg = e.Message.Split(' ');
            switch (Arg[0].ToLower())
            {
                case "@turn"://赛季更新查询
                    if (Arg.Length == 2)
                    {
                        SendMessage(e.Sender, sc.GetGameTurn(Arg[1]));
                    }
                    else
                    {
                        SendMessage(e.Sender, e.SendName + "，" + "参数错误。\r赛季更新检测的使用方法：@turn 项目名称(BB,FB)\r例：@turn BB");
                    }
                    break;
                case "@status"://夜间更新查询
                    if (Arg.Length == 2)
                    {
                        SendMessage(e.Sender, sc.GetGameStatus(Arg[1]));
                    }
                    else
                    {
                        SendMessage(e.Sender, e.SendName + "，" + "参数错误。\r夜间更新检测的使用方法：@status 项目名称(BB,FB)\r例：@status BB");
                    }
                    break;
                case "@season"://赛季更新时间查询
                    if (Arg.Length == 2)
                    {
                        SendMessage(e.Sender, sc.GetGameSeason(Arg[1]));
                    }
                    else
                    {
                        SendMessage(e.Sender, e.SendName + "，" + "参数错误。\r赛季更新检测的使用方法：@season 关键字名称(XBA,DW,TOM)\r例：@season XBA");
                    }
                    break;
                case "@check"://查询夜间更新是否执行
                    if (Arg.Length == 2)
                    {
                        SendMessage(e.Sender, sc.GetGameCheck(Arg[1]));
                    }
                    else
                    {
                        SendMessage(e.Sender, e.SendName + "，" + "参数错误。\r查询夜间更新是否执行的使用方法：@check 项目名称(BB,FB)\r例：@check BB");
                    }
                    break;
            }
        }


        #endregion

        #region 设置窗体
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
            if (db.GetObject("fb_Data") != null)
            {
                sData.fb_Data = db.GetObject("fb_Data").ToString();
            }
            if (db.GetObject("bb_Data") != null)
            {
                sData.bb_Data = db.GetObject("bb_Data").ToString();
            }
            new set().ShowDialog();
            db.AddObject("fb_Data", sData.fb_Data);
            db.AddObject("bb_Data", sData.bb_Data);
            db.SavaData();
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
        public override String PluginName { get { return "美天网络日常查询插件"; } }
        /// <summary>
        /// 插件唯一名称（英文+数字）
        /// </summary>
        public override String PluginKey { get { return "MaitiamNetworkPlugins"; } }//R
        /// <summary>
        /// 作者
        /// </summary>
        public override string Author { get { return "Cupid"; } }
        /// <summary>
        /// 插件版本
        /// </summary>
        public override Version PluginVersion { get { return new Version(1, 0, 0, 2); } }
        /// <summary>
        /// 插件图片（暂时无效）
        /// </summary>
        public override Image PlugImage { get { return null; } }
        /// <summary>
        /// 插件说明
        /// </summary>
        public override string Description { get { return "美天网络技术保障部-日常查询插件"; } }
        /// <summary>
        /// 安装协议，不需要请留空
        /// </summary>
        public override string Agreement { get { return "美天网络技术保障部"; } }

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

    [Serializable]//标记一定要
    public class Data
    {
        public string fb_Data = "";
        public string bb_Data = "";
    }
}
