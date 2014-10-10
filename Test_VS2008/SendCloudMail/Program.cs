using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using CodeScales.Http;
using CodeScales.Http.Entity;
using CodeScales.Http.Entity.Mime;
using CodeScales.Http.Methods;
using System.Threading;

namespace SendCloudMail
{
    class Program
    {
        static void Main(string[] args)
        {
            string strMailContent = null;
            string strMailContentName = null;
            string strMailList = null;

            //----------------------------------------------------------------
            //StringBuilder sb = new StringBuilder();

            //sb.Append("<html>");
            //sb.Append("<body>");
            //sb.Append("<h1>大家好，欢迎来到XBA玩《武侠全明星》</h1>");
            //sb.Append("<p>大家好，欢迎来到XBA玩《武侠全明星》 <a href=\"http://www.wuxia.com\">点击登录</a></p>");
            //sb.Append("<img src=\"http://www.baidu.com/img/shouye_b5486898c692066bd2cbaeda86d74448.gif\" width=\"270\" height=\"129\">");
            //sb.Append("</body>");
            //sb.Append("</html>");

            //string strMailList = "182536608@qq.com";

            //string[] arrMail = strMailList.Split('|');

            //foreach (string strMail in arrMail)
            //{
            //    SendMail(strMail, "邮件测试(这是测试)", sb.ToString());
            //}
            //----------------------------------------------------------------

            strMailContent = CheckMailContent();
            strMailList = CheckMailList();

            if (strMailContent != "false")
            {
                string strLine;
                string strMail;

                LogMsg("开始加载邮件内容:" + strMailContent);
                strMailContentName = strMailContent.Replace(".html", "");

                StreamReader srmc = new StreamReader(strMailContent);
                strLine = srmc.ReadToEnd();
                if (strLine != "")
                {
                    LogMsg("邮件内容加载完成:" + strMailContent);
                }
                srmc.Close();

                if (strMailList != "false")
                {
                    LogMsg("开始加载读取邮件列表");
                    StreamReader srml = new StreamReader(strMailList);
                    while ((strMail = srml.ReadLine()) != null)
                    {
                        Console.WriteLine("发送邮件:" + strMail);
                        SendMail(strMail, strMailContentName, strLine);
                        Thread.Sleep(3000);
                    }
                    srmc.Close();
                }
                else
                {
                    LogMsg("邮件列表加载失败或文件不存在");
                }
            }
            else
            {
                LogMsg("邮件内容加载失败或文件不存在");
            }
            Console.ReadKey();
        }

        #region 邮件发送 SendMail(string to, string subject, string Content)
        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="Content"></param>
        private static void SendMail(string to, string subject, string Content)
        {
            HttpClient client = new HttpClient();
            HttpPost postMethod = new HttpPost(new Uri("http://sendcloud.sohu.com/webapi/mail.send.xml"));
            MultipartEntity multipartEntity = new MultipartEntity();
            postMethod.Entity = multipartEntity;
            // 不同于登录SendCloud站点的帐号，您需要登录后台创建发信域名，获得对应发信域名下的帐号和密码才可以进行邮件的发送。
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_user", "postmaster@tg.sendcloud.org"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_key", "SdCc6gvb"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "from", "kapaiwuxia015@loltm.com"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "fromname", "XBA游戏网"));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "to", to));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "subject", subject));
            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "html", Content));

            //FileInfo fileInfo = new FileInfo(@"c:\SANCH5890V.jpg");

            //UTF8FileBody fileBody = new UTF8FileBody("file1", "SANCH5890V.jpg", fileInfo);
            //multipartEntity.AddBody(fileBody);

            HttpResponse response = client.Execute(postMethod);

            
            if (response.ResponseCode == 200)
            {
                LogMsg(" 发送邮件给：" + to + " 请求状态：" + response.ResponseCode + " 发送状态：" + EntityUtils.ToString(response.Entity).Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r", ""));
            }
            else
            {
                LogMsg(" 发送邮件给：" + to + " 请求状态：" + response.ResponseCode + " 发送状态：" + EntityUtils.ToString(response.Entity).Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r", ""));
            }
            //Console.WriteLine("Response Code: " + response.ResponseCode);
            //Console.WriteLine("Response Content: " + EntityUtils.ToString(response.Entity));
        }
        #endregion 邮件发送 SendMail(string to, string subject, string Content)

        #region 系统日志 LogMsg(string strCount)
        /// <summary>
        /// 消息插入
        /// </summary>
        /// <param name="strCount"></param>
        private static void LogMsg(string strCount)
        {
            DateTime dt = DateTime.Now;
            Log.WriteLog(LogFile.Trace, strCount);
            Console.WriteLine(dt + "  |  " + strCount);
        }
        #endregion 系统日志 LogMsg(string strCount)

        #region 检查邮件列表文件是否存在 CheckMailList()
        /// <summary>
        /// 检查邮件列表文件是否存在
        /// </summary>
        /// <returns></returns>
        private static string CheckMailList()
        {
            string strFile = null;
            try
            {
                string[] files = Directory.GetFiles(@".");//得到文件
                foreach (string file in files)//循环文件
                {
                    string exname = file.Substring(file.LastIndexOf(".") + 1);//得到后缀名
                    if (".txt".IndexOf(file.Substring(file.LastIndexOf(".") + 1)) > -1)//如果后缀名为.txt文件
                    {
                        FileInfo fi = new FileInfo(file);//建立FileInfo对象
                        strFile = fi.Name;
                        return strFile;
                    }
                }
            }
            catch
            {

            }

            if (strFile == null)
            {
                Console.WriteLine("没有找到要发送的邮件列表");
            }
            return "false";
        }
        #endregion 检查邮件列表文件是否存在 CheckMailList()

        #region 查询邮件发送内容 CheckMailContent()
        /// <summary>
        /// 查询邮件发送内容
        /// </summary>
        /// <returns></returns>
        private static string CheckMailContent()
        {
            string strFile = null;
            try
            {
                string[] files = Directory.GetFiles(@".");//得到文件
                foreach (string file in files)//循环文件
                {
                    string exname = file.Substring(file.LastIndexOf(".") + 1);//得到后缀名
                    if (".html".IndexOf(file.Substring(file.LastIndexOf(".") + 1)) > -1)//如果后缀名为.txt文件
                    {
                        FileInfo fi = new FileInfo(file);//建立FileInfo对象
                        strFile = fi.Name;
                        return strFile;
                    }
                }
            }
            catch
            {
                
            }

            if (strFile == null)
            {
                Console.WriteLine("没有找到要发送的邮件内容");
            }
            return "false";
        }
        #endregion 查询邮件发送内容 CheckMailContent()
    }
}
