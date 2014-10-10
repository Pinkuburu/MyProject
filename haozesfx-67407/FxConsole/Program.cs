using System;
using Haozes.FxClient;
using Haozes.FxClient.Core;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Linq;

namespace FxConsole
{
    class Program
    {
        static FxClass fx = new FxClass();
        static void Main(string[] args)
        {
            fx.LoginFx();
            Console.ReadKey();
        }
    }

    public class FxClass
    {
        private Client fetion;

        public void LoginFx()
        {
            string mobile = "13406802804";
            string pwd = "677521cupid0426";

            InitFx();
            fetion.Login(mobile, pwd);
        }

        private void InitFx()
        {
            fetion = new Client();
            //fetion.Log = LogUtil.Log;
            fetion.LoginSucceed += new EventHandler(fetion_LoginSucceed);
            fetion.LoginFailed += new EventHandler<LoginEventArgs>(fetion_LoginFailed);
            fetion.VerifyCodeRequired = (reason, picBuffer) =>
            {
                return this.InputVerifyCode(reason, picBuffer);
            };
            fetion.Load += new EventHandler(fetion_Load);
            fetion.Errored += new EventHandler<FxErrArgs>(fetion_Errored);
            fetion.MsgReceived += new EventHandler<ConversationArgs>(fetion_MsgReceived);
            fetion.Deregistered += new EventHandler(fetion_Deregistered);
            fetion.AddBuddyRequest += new EventHandler<ConversationArgs>(fetion_AddBuddyRequest);
            fetion.AddBuddyResult += new EventHandler<ConversationArgs>(fetion_AddBuddyResult);
            fetion.DeleteBuddyResult += new EventHandler<ConversationArgs>(fetion_DeleteBuddyResult);
        }

        void fetion_DeleteBuddyResult(object sender, ConversationArgs e)
        {
            if (e.Text == "200")
            {
                Console.WriteLine("删除好友成功");
            }
        }

        void fetion_AddBuddyResult(object sender, ConversationArgs e)
        {
            string result = e.Text;
            if (!string.IsNullOrEmpty(result))
            {
                Console.WriteLine(string.Format("添加好友:{0}成功!", result));
            }
            else
            {
                Console.WriteLine("添加好友失败!");
            }
        }

        void fetion_AddBuddyRequest(object sender, ConversationArgs e)
        {
            XDocument doc = XDocument.Parse(e.Text);
            string uri = doc.Element("events").Element("event").Element("application").Attribute("uri").Value;
            string desc = doc.Element("events").Element("event").Element("application").Attribute("desc").Value;
            string userid = doc.Element("events").Element("event").Element("application").Attribute("user-id").Value;

            fetion.SendToSelf(uri + "添加你为好友");
            fetion.AgreeAddBuddy(new SipUri(uri), userid);
        }

        void fetion_Deregistered(object sender, EventArgs e)
        {
            LogoutFx();
            Console.WriteLine("用户已从其他客户端登陆");
        }

        void fetion_MsgReceived(object sender, ConversationArgs e)
        {
            if (sender != null)
            {
                Conversation conv = (Conversation)sender;
                if (e.MsgType == IMType.Text || e.MsgType == IMType.SMS)
                {
                    
                }
            }
        }

        void fetion_Errored(object sender, FxErrArgs e)
        {
            if (e.Level == ErrLevel.Fatal && fetion.IsALive)
            {
                ReLogin();
            }
        }

        void fetion_Load(object sender, EventArgs e)
        {
            Console.WriteLine("获取联系人列表成功");
            Console.Title = fetion.CurrentUser.NickName;
            fetion.SetPresence(PresenceStatus.Active);
                        
        }

        void fetion_LoginFailed(object sender, LoginEventArgs e)
        {
            ReLogin();
        }

        void fetion_LoginSucceed(object sender, EventArgs e)
        {
            Console.WriteLine("登录成功");
        }

        /// <summary>
        /// 提示输入验证码对话框
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="picBuffer">验证码的buffer</param>
        /// <returns></returns>
        private string InputVerifyCode(string reason, byte[] picBuffer)
        {
            using (VerifyCodeFrm frm = new VerifyCodeFrm(reason, picBuffer))
            {
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    return frm.VerfyCode;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private void ReLogin()
        {
            LogoutFx();
            Console.WriteLine("系统遇到错误,20秒后将重新登陆");
            Thread.Sleep(20000);
            LoginFx();
        }

        private void LogoutFx()
        {
            fetion.Exit();
            fetion.LoginSucceed -= new EventHandler(fetion_LoginSucceed);
            fetion.LoginFailed -= new EventHandler<LoginEventArgs>(fetion_LoginFailed);
            fetion.Errored -= new EventHandler<FxErrArgs>(fetion_Errored);
            fetion.MsgReceived -= new EventHandler<ConversationArgs>(fetion_MsgReceived);
            fetion.Deregistered -= new EventHandler(fetion_Deregistered);
            fetion.AddBuddyRequest -= new EventHandler<ConversationArgs>(fetion_AddBuddyRequest);
            fetion.AddBuddyResult -= new EventHandler<ConversationArgs>(fetion_AddBuddyResult);
            fetion.DeleteBuddyResult -= new EventHandler<ConversationArgs>(fetion_DeleteBuddyResult);
        }
    }
}
