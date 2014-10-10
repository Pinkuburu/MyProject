using System;
using System.Diagnostics;
using System.Web;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using Weixin.App_Code.DBManager;
using Weixin.App_Code.Log;

namespace Senparc.Weixin.MP.Sample.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler : MessageHandler
    {
        public override IResponseMessageBase OnEvent_EnterRequest(RequestMessageEvent_Enter requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "您刚才发送了ENTER事件请求。";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            throw new Exception("暂不可用");
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);

            //获取Senparc.Weixin.MP.dll版本信息
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(HttpContext.Current.Server.MapPath("~/bin/Senparc.Weixin.MP.dll"));
            //var version = fileVersionInfo.FileVersion;

            try
            {
                bool blAddUser = false;
                blAddUser = UAMS_UserManager.AddUser(requestMessage.FromUserName);
                if(!blAddUser)
                {
                    Log.WriteLog(LogFile.Trace, "添加用户：" + requestMessage.FromUserName);
                }
                else
                {
                    Log.WriteLog(LogFile.Trace, "用户已存在：" + requestMessage.FromUserName);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLog(LogFile.Error, "发生了一些不必要的错误：" + ex.ToString());
            }
            
            responseMessage.Content = string.Format("【美天网络】统一帐号管理系统\r\n请输入命令编号或命令进行操作：\r\n1.【激活】\r\n2.【登录】");
            return responseMessage;
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage(requestMessage,
                                                                                         ResponseMsgType.Text) as ResponseMessageText;
            responseMessage.Content = "有空再来";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            throw new Exception("Demo中还没有加入CLICK的测试！");
        }
    }
}