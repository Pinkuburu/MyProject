using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace HttpListener_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://*:8000/");
            listener.Start();
            Console.WriteLine("Listening...");

            try
            {
                while (true)
                {
                    //获取一个客户端请求为止
                    HttpListenerContext context = listener.GetContext();
                    //将其处理过程放入线程池
                    System.Threading.ThreadPool.QueueUserWorkItem(ProcessHttpClient, context);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                listener.Stop();    //关闭HttpListener
            }
        }

        //客户请求处理
        static void ProcessHttpClient(object obj)
        {
            HttpListenerContext context = obj as HttpListenerContext;
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            Console.WriteLine("{0}  {1}  HTTP/1.1", request.HttpMethod, request.RawUrl);
            Console.WriteLine("Accept:{0}", string.Join(",", request.AcceptTypes));
            Console.WriteLine("Accept-Language:{0}", string.Join(",", request.UserLanguages));
            Console.WriteLine("User-Agent:{0}", request.UserAgent);
            Console.WriteLine("Accept-Encoding:{0}", request.Headers["Accept-Encoding"]);
            Console.WriteLine("Connection:{0}", request.KeepAlive ? "Keep-Alive" : "close");
            Console.WriteLine("Host:{0}", request.UserHostName);
            Console.WriteLine("Pragma:{0}", request.Headers["Pragma"]);

            //do something as you want
            string responseString = string.Format("<HTML><BODY> {0}</BODY></HTML>", DateTime.Now);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            //关闭输出流，释放相应资源
            output.Close();
        }
    }
}
