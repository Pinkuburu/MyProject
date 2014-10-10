using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Console_ERRORLEVEL_test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(Timestamp());
            //Console.ReadKey();
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://*:8080/"); //添加需要监听的url范围
            listener.Start(); //开始监听端口，接收客户端请求
            Console.WriteLine("Listening...");

            //阻塞主函数至接收到一个客户端请求为止
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string responseString = string.Format("<HTML><BODY> {0}</BODY></HTML>", DateTime.Now);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            //对客户端输出相应信息.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            //关闭输出流，释放相应资源
            output.Close();

            listener.Stop(); //关闭HttpListener
            Console.ReadKey();
        }

        #region 时间戳 Timestamp()
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        private static long Timestamp()
        {
            long longTimestamp = 0;
            longTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            return longTimestamp;
        }
        #endregion 时间戳 Timestamp()
    }
}


//public class HttpWebServer
//{
//    private HttpListener Listener;

//    public void Start()
//    {
//        Listener = new HttpListener();
//        Listener.Prefixes.Add("http://*:5555/");
//        Listener.Start();
//        Listener.BeginGetContext(ProcessRequest, Listener);
//        Console.WriteLine("Connection Started");
//    }

//    public void Stop()
//    {
//        Listener.Stop();
//    }

//    private void ProcessRequest(IAsyncResult result)
//    {
//        HttpListener listener = (HttpListener)result.AsyncState;
//        HttpListenerContext context = listener.EndGetContext(result);

//        string responseString = "<html>Hello World</html>";
//        byte[] buffer = Encoding.UTF8.GetBytes(responseString);

//        context.Response.ContentLength64 = buffer.Length;
//        System.IO.Stream output = context.Response.OutputStream;
//        output.Write(buffer, 0, buffer.Length);
//        output.Close();

//        Listener.BeginGetContext(ProcessRequest, Listener);
//    }
//}

//static void Main(string[] args)
//{
//    using (HttpListener listerner = new HttpListener())
//    {
//        listerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定身份验证 Anonymous匿名访问
//        listerner.Prefixes.Add("http://localhost:8080/web/");

//        // listerner.Prefixes.Add("http://localhost/web/");
//        listerner.Start();
//        Console.WriteLine("WebServer Start Successed.......");
//        while (true)
//        {
//            //等待请求连接
//            //没有请求则GetContext处于阻塞状态
//            HttpListenerContext ctx = listerner.GetContext();
//            ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码
//            string name = ctx.Request.QueryString["name"];

//            if (name != null)
//            {
//                Console.WriteLine(name);
//            }


//            //使用Writer输出http响应代码
//            using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream))
//            {
//                Console.WriteLine("hello");
//                writer.WriteLine("<html><head><title>The WebServer Test</title></head><body>");
//                writer.WriteLine("<div style=\"height:20px;color:blue;text-align:center;\"><p> hello {0}</p></div>", name);
//                writer.WriteLine("<ul>");

//                foreach (string header in ctx.Request.Headers.Keys)
//                {
//                    writer.WriteLine("<li><b>{0}:</b>{1}</li>", header, ctx.Request.Headers[header]);

//                }
//                writer.WriteLine("</ul>");
//                writer.WriteLine("</body></html>");

//                writer.Close();
//                ctx.Response.Close();
//            }

//        }
//        listerner.Stop();
//    }
//}


//static void Main(string[] args)
//{
//     HttpListener listener = new HttpListener();
//     listener.Prefixes.Add("http://localhost/"); //要监听的url范围
//     listener.Start();   //开始监听端口，接收客户端请求
//     Console.WriteLine("Listening");

//    try
//    {
//        while (true)
//        {
//            //获取一个客户端请求为止
//             HttpListenerContext context = listener.GetContext();
//            //将其处理过程放入线程池
//             System.Threading.ThreadPool.QueueUserWorkItem(ProcessHttpClient, context);
//         }
//     }
//    catch (Exception e)
//    {

//         Console.WriteLine(e.Message);
//     }
//    finally
//    {
//         listener.Stop();    //关闭HttpListener
//     }
    
// }
////客户请求处理
//static void ProcessHttpClient(object obj)
//{
//     HttpListenerContext context = obj as HttpListenerContext;
//     HttpListenerRequest request = context.Request;
//     HttpListenerResponse response = context.Response;

//    //do something as you want
//    string responseString = string.Format("<HTML><BODY> {0}</BODY></HTML>", DateTime.Now);
//    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
//     response.ContentLength64 = buffer.Length;
//     System.IO.Stream output = response.OutputStream;
//     output.Write(buffer, 0, buffer.Length);
    
//    //关闭输出流，释放相应资源
//     output.Close();
// }
//}