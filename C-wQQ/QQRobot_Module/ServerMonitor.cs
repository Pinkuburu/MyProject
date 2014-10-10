using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

//=====================
//    QQ机器人插件     
//    作者:Cupid       
// 创建日期:2010-11-19 
//=====================

namespace QQRobot_Module
{
    public class ServerMonitor
    {
        #region 邮件发送 SendMail(string strMailTo, string strSubject, string strMailContent)
        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="strMailTo">接收邮箱</param>
        /// <param name="strSubject">邮件主题</param>
        /// <param name="strMailContent">邮件内容</param>
        /// <returns></returns>
        public string SendMail(string strMailTo, string strSubject, string strMailContent)
        {
            string strResult = null;
            try
            {
                string strExePath = @"C:\HQmail.exe";
                //strExePath = Path.GetFullPath(strExePath);
                ProcessStartInfo procInfo = new ProcessStartInfo(strExePath, string.Format("cupid0616@163.com 677521++ {0} {1} {2}", strMailTo, strSubject, strMailContent));
                // 隐藏EXE运行的窗口
                procInfo.WindowStyle = ProcessWindowStyle.Hidden;
                // exe运行
                Process procBatch = Process.Start(procInfo);
                // 取得EXE运行后的返回值，返回值只能是整型

                strResult = "邮件发送成功~~";
            }
            catch
            {
                strResult = "邮件发送失败~~";
            }
            return strResult;
        }
        #endregion 邮件发送 SendMail(string strMailTo, string strSubject, string strMailContent)

        #region 短信发送 SendSMS(string strToMobile, string strMsg)
        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="strToMobile">收短信手机号</param>
        /// <param name="strMsg">短信内容</param>
        /// <returns></returns>
        public string SendSMS(string strToMobile, string strMsg)
        {
            //C:\fetion>fetion --mobile=13691102424 --pwd=UTx/t4YJrvqz8mnO --exit-on-verifycode=1 --to=13573866764 --msg-utf8=TEST
            string strResult = null;
            try
            {
                string strExePath = @"C:\fetion\fetion.exe";
                //strExePath = Path.GetFullPath(strExePath);
                ProcessStartInfo procInfo = new ProcessStartInfo(strExePath, string.Format("--mobile=13691102424 --pwd=UTx/t4YJrvqz8mnO --exit-on-verifycode=1 --to={0} --msg-utf8={1}", strToMobile, strMsg));
                // 隐藏EXE运行的窗口
                procInfo.WindowStyle = ProcessWindowStyle.Hidden;
                // exe运行
                Process procBatch = Process.Start(procInfo);
                // 取得EXE运行后的返回值，返回值只能是整型
                strResult = "短信发送成功~~";
            }
            catch
            {
                strResult = "短信发送失败~~";
            }
            return strResult;
        }
        #endregion 短信发送 SendSMS(string strToMobile, string strMsg)

        #region 发送QQ消息 SendQQMsg(string strQQ, string strMsg)
        /// <summary>
        /// 发送QQ消息
        /// </summary>
        /// <param name="strQQ"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public string SendQQMsg(string strQQ, string strMsg)
        {
            string strContent = null;
            WebClient HTTPproc = new WebClient();
            strContent = HTTPproc.OpenRead("http://127.0.0.1:8848/Api?Key=Cupid&SendType=SendMessage&ID=" + strQQ + "&Message=" + strMsg);
            return strContent;
        }
        #endregion 发送QQ消息 SendQQMsg(string strQQ, string strMsg)
    }
}