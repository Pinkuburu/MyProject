using System;
using Weixin.App_Code.Log;
using Weixin.App_Code.DBManager;

namespace Weixin.App_Code.CustomMessageHandler
{
    public class MsgHandler
    {        
        #region 文字消息处理 TextRequest(string strContent, string strUserName)
        /// <summary>
        /// 文字消息处理
        /// </summary>
        /// <param name="strContent">消息内容</param>
        /// <param name="strUserName">请求用户名</param>
        /// <returns>消息内容</returns>
        public static string TextRequest(string strContent, string strUserName)
        {            
            string[] strArray = strContent.Split(' ');
            string command = strArray[0];
            int intType = 0;

            Random rnd = new Random();

            switch (command.ToLower())
            {
                case "1":
                case "激活":
                    if (strArray.Length == 2)
                    {
                        intType = UAMS_UserManager.CheckToken(strUserName, strArray[1]);

                        if (intType == 3)
                        {
                            strContent = "激活码已过期";
                            Log.Log.WriteLog(LogFile.Trace, "用户：" + strUserName + " 激活码已过期：" + strArray[1]);
                        }
                        else if (intType == 2)
                        {
                            strContent = "激活码无效或已被使用";
                            Log.Log.WriteLog(LogFile.Trace, "用户：" + strUserName + " 激活码无效：" + strArray[1]);
                        }
                        else
                        {
                            strContent = "激活码已使用";
                            Log.Log.WriteLog(LogFile.Trace, "用户：" + strUserName + " 激活码已使用：" + strArray[1]);
                        }
                    }
                    else
                    {
                        strContent = "激活啊激活";
                    }
                    break;
                case "2":
                case "登录":
                    strContent = rnd.Next(100000, 999999).ToString();
                    UAMS_UserManager.AddToken(strUserName, strContent);
                    Log.Log.WriteLog(LogFile.Trace, "用户：" + strUserName + " 获取Token：" + strContent);
                    break;
                case "3":
                    strContent = "http://ishow.xba.com.cn/1.aspx";
                    break;
                default:
                    strContent = string.Format("【美天网络】统一帐号管理系统\r\n请输入命令编号或命令进行操作：\r\n1.【激活】\r\n2.【登录】");
                    break;
            }

            return strContent;
        }
        #endregion 文字消息处理 TextRequest(string strContent, string strUserName)
    }
}