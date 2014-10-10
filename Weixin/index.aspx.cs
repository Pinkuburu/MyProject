using System;
using System.IO;

using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Sample.Service;
using Senparc.Weixin.MP.Sample.CustomerMessageHandler;

public partial class index : System.Web.UI.Page 
{
    private readonly string Token = "weixin";
    private LocationService _locationService;
    private EventService _eventService;

    public index()
    {
        _locationService = new LocationService();
        _eventService = new EventService();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string signature = Request["signature"];
        string timestamp = Request["timestamp"];
        string nonce = Request["nonce"];
        string echostr = Request["echostr"];

        if (Request.HttpMethod == "POST")
        {
            //post method
            if (!CheckSignature.Check(signature, timestamp, nonce, Token))
            {
                Content("参数错误！");
                return;
            }

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new CustomMessageHandler(Request.InputStream);

            try
            {
                //测试时可开启此记录，帮助跟踪数据
                messageHandler.RequestDocument.Save(Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Request_" + messageHandler.RequestMessage.FromUserName + ".txt"));
                //执行微信处理过程
                messageHandler.Execute();
                //测试时可开启，帮助跟踪数据
                messageHandler.ResponseDocument.Save(Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Response_" + messageHandler.ResponseMessage.ToUserName + ".txt"));
                Content(messageHandler.ResponseDocument.ToString());
                return;
            }
            catch (Exception ex)
            {
                using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/Error_" + DateTime.Now.Ticks + ".txt")))
                {
                    tw.WriteLine(ex.Message);
                    tw.WriteLine(ex.InnerException.Message);
                    if (messageHandler.ResponseDocument != null)
                    {
                        tw.WriteLine(messageHandler.ResponseDocument.ToString());
                    }
                    tw.Flush();
                    tw.Close();
                }
                Content("");
                return;
            }
        }
        else
        {
            //get method
            if (CheckSignature.Check(signature, timestamp, nonce, Token))
            {
                Content(echostr); //返回随机字符串则表示验证通过
                return;
            }
            else
            {
                Content("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token));
                return;
            }
        }
    }

    private void Content(string str)
    {
        Response.Output.Write(str);
    }
}