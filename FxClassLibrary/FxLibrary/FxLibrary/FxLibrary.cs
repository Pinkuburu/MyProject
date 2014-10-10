using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using FxModule;
using System.Text.RegularExpressions;
using System.Data;
/*
            插件列表

    1、 plugin_result %1 %2 %3
    插件功能：执行结果反馈
    参数说明：
    1：机器人手机号码
    2：命令id
    3：命令结果(base64 encoded)

    2、 plugin_notify %1 %2
    插件功能：通用的系统通知，该插件把飞信服务器传递过来的原始数据转发给插件。
    参数说明：
    1：机器人手机号码
    2：命令结果(base64编码的xml数据)

    3、 plugin_buddy_invite %1 %2
    插件功能：对方发起对话时（双击打开窗口时，一般此时可以发送机器人菜单）
    参数说明：
    1：机器人手机号码
    2：对方URI(base64 encode)

    4、plugin_buddy_data %1 %2
    插件功能：buddy资料传递
    参数说明：
    1：机器人手机号码
    2：base64编码的用户资料
    <buddy>
    <type></type>
    <user-id></user-id>
    <in-reverse-list></in-reverse-list>
    <uri></uri>
    <sid></sid>
    <seg></seg>
    <mobile></mobile>
    <status-code></status-code>
    <online-status></online-status>
    <online-desc>{base64 encoded}</online-desc>
    <nickname>{base64 decoded}</nickname>
    <localname>{base64 encoded}</localname>
    <impresa>{base64 encoded}</impresa>
    </buddy> 

    5、plugin_ handle_contact_request %1 %2
    插件功能：有新的好友加入（为了突破人数限制，此时可以在保留有关数据后，删除该用户）
    参数说明：
    1：机器人手机号码
    2：base64编码的好友URI

    6、plugin_system_message %1 %2
    插件功能：系统通知消息
    参数说明：
    1：机器人手机号码
    2：base64编码的信息

    7、plugin_message %1 %2 %3 %4
    插件功能：消息
    参数说明：
    1：机器人手机号码
    2：base64编码的URI
    3：base64编码的信息
    4：base64编码的消息类型(text/html text/plain) 可以根据此参数，确定消息来自pc端还是手机端 （新增）

    8、plugin_timer %1 %2
    插件功能：10秒钟激活一次
    参数说明：
    1：机器人手机号码
    2：程序启动后的运行秒数（非精确）                       */

//=============================================
//作    者:	HZW
//创建时间:	2009-7-21
//功能描述:	FxClassLibrary
//=============================================

namespace FxLibrary
{
    public class Fetion
    {
        #region 读取发送人
        /// <summary>
        /// 读取发送人
        /// </summary>
        /// <param name="code">code</param>
        /// <returns></returns>
        public string readSendUser(string code)
        {
            string decode = "";

            //Base64解码，注意UTF8
            code = Base64_decode(code);
            byte[] bytes = Convert.FromBase64String(code);
            decode = System.Text.Encoding.UTF8.GetString(bytes);
            //怎么去除多余的内容？sip:78516401@fetion.com.cn;p=187
            //菜鸟用的笨办法，先用正则把里面的多余字符去除，然后把最后３个字符清除．谁有更好的办法，请救了
            decode = Convert.ToString(Regex.Match(decode, @"\d{4,10}@fetion\.com\.cn"));
            //decode = decode.Substring(0, decode.Length - 3);
            return decode;
        }
        #endregion

        #region 读取短信内容
        /// <summary>
        /// 读取短信内容
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string readSMSContent(string code)
        {

            //Base64解码，注意UTF8
            string decode = "";
            code = Base64_decode(code);
            if (code != "")
            {
                byte[] bytes = Convert.FromBase64String(code);
                try
                {
                    decode = System.Text.Encoding.UTF8.GetString(bytes);
                    //使用PC客户端，有html，手机没有，其实发送的还没有测试。
                    decode = Regex.Replace(decode, "<[^>]+>", "");
                }
                catch
                {
                    decode = code;
                }
            }
            return decode;
        }
        #endregion

        #region 处理数据位数不足并补齐
        /// <summary>
        /// 处理数据位数不足并补齐
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string Base64_decode(string code)
        {
            while (code.Length % 4 > 0)
            {
                code = code + "=";
            }
            return code;
        }
        #endregion

        #region 计时器
        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="strMobileNo"></param>
        /// <param name="strTimer"></param>
        public void evTimer(string strMobileNo, string strTimer)
        {
            string strCity = "qingdao";
            string strWeather = "";
            DateTime Time = DateTime.Now;

            //strWeather = Module.Weather(strCity);
            //Console.WriteLine(strWeather);
            //Console.WriteLine("事件提醒已发出！");
        }
        #endregion

        #region 执行结果反馈
        /// <summary>
        /// 执行结果反馈
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string cmdResult(string code)
        {

            //Base64解码，注意UTF8
            string decode = "";
            code = Base64_decode(code);
            if (code != "")
            {
                byte[] bytes = Convert.FromBase64String(code);
                try
                {
                    decode = System.Text.Encoding.UTF8.GetString(bytes);
                    //使用PC客户端，有html，手机没有，其实发送的还没有测试。
                    //decode = System.Text.RegularExpressions.Regex.Replace(decode, "<[^>]+>", "");
                }
                catch
                {
                    decode = code;
                }
            }
            return decode;
        }
        #endregion

        #region 通用的系统通知
        /// <summary>
        /// 通用的系统通知
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string sysNotify(string code)
        {

            //Base64解码，注意UTF8
            string decode = "";
            code = Base64_decode(code);
            if (code != "")
            {
                byte[] bytes = Convert.FromBase64String(code);
                try
                {
                    decode = System.Text.Encoding.UTF8.GetString(bytes);
                    //使用PC客户端，有html，手机没有，其实发送的还没有测试。
                    //decode = System.Text.RegularExpressions.Regex.Replace(decode, "<[^>]+>", "");
                }
                catch
                {
                    decode = code;
                }
            }
            return decode;
        }
        #endregion

        #region 用户资料
        /// <summary>
        /// 用户资料
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string userInfo(string code)
        {

            //Base64解码，注意UTF8
            string decode = "";
            code = Base64_decode(code);
            if (code != "")
            {
                byte[] bytes = Convert.FromBase64String(code);
                try
                {
                    decode = System.Text.Encoding.UTF8.GetString(bytes);
                    //使用PC客户端，有html，手机没有，其实发送的还没有测试。
                    //decode = System.Text.RegularExpressions.Regex.Replace(decode, "<[^>]+>", "");
                }
                catch
                {
                    decode = code;
                }
            }
            return decode;
        }
        #endregion

        #region 系统消息
        /// <summary>
        /// 系统消息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string sysMessage(string code)
        {

            //Base64解码，注意UTF8
            string decode = "";
            code = Base64_decode(code);
            if (code != "")
            {
                byte[] bytes = Convert.FromBase64String(code);
                try
                {
                    decode = System.Text.Encoding.UTF8.GetString(bytes);
                    //使用PC客户端，有html，手机没有，其实发送的还没有测试。
                    //decode = System.Text.RegularExpressions.Regex.Replace(decode, "<[^>]+>", "");
                }
                catch
                {
                    decode = code;
                }
            }
            return decode;
        }
        #endregion

        #region 读取XML的值
        /// <summary>
        /// 读取XML的值
        /// </summary>
        /// <param name="xmldata"></param>
        public string ReadXmlTextValue(string xmldata, string NodeName)
        {
            //status = 400 在线
            //status = 100 离开
            //status = 600 忙碌

            //创建XmlDocument对象
            XmlDocument xmlDoc = new XmlDocument();
            //载入xml文件名
            //xmlDoc.Load(filename);
            //如果是xml字符串，则用以下形式
            xmlDoc.LoadXml(xmldata);

            //读取根节点的所有子节点，放到xn0中 
            XmlNodeList xn0 = xmlDoc.SelectSingleNode("buddy").ChildNodes;

            //查找二级节点的内容或属性 
            //string strPhoneNum = "";
            //string strNickname = "";
            //string strSid = "";
            string strNodeValue = "";

            foreach (XmlNode node in xn0)
            {
                //if (node.Name == "sid")
                //{
                //    //string innertext = node.InnerText.Trim();//匹配二级节点的内容
                //    //string attr = node.Attributes[0].ToString();//属性
                //    strSid = node.InnerText.Trim();//sid
                //}
                //if (node.Name == "mobile")
                //{
                //    strPhoneNum = node.InnerText.Trim();//电话
                //}
                if (node.Name == NodeName)
                {
                    strNodeValue = node.InnerText.Trim();//昵称
                }
            }
            return strNodeValue;
        }
        #endregion

        #region 短信控制台
        /// <summary>
        /// 短信控制台
        /// </summary>
        /// <param name="intSID">发送方的SID</param>
        /// <param name="Command">执行命令</param>
        /// <param name="Parameter">参数</param>
        public static void SysConsole(int intSID, string Command, string Parameter)
        {
            string strCmdResult = "";
            string strSID = Convert.ToString(intSID);
            switch (Command)
            {
                case "tq":
                    Console.WriteLine("进入 SysConsole.tq");////////////////////////////
                    strCmdResult = Module.Weather(Parameter);
                    Console.WriteLine("调用发送天气预报  " + Parameter);//////////////////////////
                    //strCmdResult = Module.Weather();
                    Console.WriteLine(strCmdResult + "\r\n");
                    Module.CreateTxt(strSID, strCmdResult, 3);
                    break;
                case "st":
                    if (Parameter == "bb")
                    {
                        strCmdResult = Module.GetBasketBallTurn();
                        if (strCmdResult == "False")
                        {
                            Module.CreateTxt(strSID, "今晚篮球没有赛季更新!", 1);
                            break;
                        }
                        else
                        {
                            Module.CreateTxt(strSID, "今晚篮球赛季更新的有\n" + strCmdResult, 1);
                            break;
                        }                        
                    }
                    else if (Parameter == "bbst")
                    {
                        strCmdResult = Module.GetBasketBallStatus();
                        if (strCmdResult == "False")
                        {
                            Module.CreateTxt(strSID, "篮球夜间更新正常！", 1);
                            break;
                        }
                        else
                        {
                            Module.CreateTxt(strSID, "篮球夜间更新出错的有\n" + strCmdResult, 1);
                            break;
                        }   
                    }
                    else if (Parameter == "bbup")
                    {
                        strCmdResult = Module.GetBasketBallUpdateNightCheck();
                        if (strCmdResult == "False")
                        {
                            Module.CreateTxt(strSID, "篮球夜间更新检测正常！", 1);
                            break;
                        }
                        else
                        {
                            Module.CreateTxt(strSID, "篮球夜间更新出错的有\n" + strCmdResult, 1);
                            break;
                        }
                    }
                    else if (Parameter == "fb")
                    {
                        strCmdResult = Module.GetFootBallTurn();
                        if (strCmdResult == "False")
                        {
                            Module.CreateTxt(strSID, "今晚足球没有赛季更新!", 1);
                            break;
                        }
                        else
                        {
                            Module.CreateTxt(strSID, "今晚足球赛季更新的有\n" + strCmdResult, 1);
                            break;
                        }   
                    }
                    else if (Parameter == "fbst")
                    {
                        strCmdResult = Module.GetFootBallStatus();
                        if (strCmdResult == "False")
                        {
                            Module.CreateTxt(strSID, "足球夜间更新正常！", 1);
                            break;
                        }
                        else
                        {
                            Module.CreateTxt(strSID, "足球夜间更新出错的有\n" + strCmdResult, 1);
                            break;
                        }   
                    }
                    else if (Parameter == "fbup")
                    {
                        strCmdResult = Module.GetFootBallUpdateNightCheck();
                        if (strCmdResult == "False")
                        {
                            Module.CreateTxt(strSID, "足球夜间更新检测正常！", 1);
                            break;
                        }
                        else
                        {
                            Module.CreateTxt(strSID, "足球夜间更新出错的有\n" + strCmdResult, 1);
                            break;
                        }
                    }
                    break;
                case "ts":  //英译中
                    strCmdResult = Module.GoogleTranslate(Parameter);
                    Module.CreateTxt(strSID, strCmdResult, 1);
                    Console.WriteLine(strCmdResult + "\r\n");
                    break;
                default:
                    Console.WriteLine("Command Error!");
                    break;
            }
        }
        #endregion

        #region 存储消息
        /// <summary>
        /// 存储消息
        /// </summary>
        /// <param name="strSID"></param>
        /// <param name="strSMSContent"></param>
        public static void SaveMessage(string strSID, string strSMSContent)
        {
            int intSID = 0;
            int intOut = 0;
            intSID = Convert.ToInt32(Regex.Match(strSID, @"\d{2,15}").ToString());
            if (intSID > 0)
            {
                intOut = SqlLibrary.Fx_SaveMessage(intSID, strSMSContent);
                if (intOut == 0)
                {
                    Console.WriteLine("[收到:" + intSID + "消息！]\r\n");
                }
                else
                {
                    Console.WriteLine("[接收:" + intSID + "出错！]\r\n");
                }
            }
            else
            {
                Console.WriteLine("[用户传入SID出错！]");
            }
        }
        #endregion

        #region 发送消息
        /// <summary>
        /// 发送消息
        /// </summary>
        public static void SendMessage()
        {
            int intID = 0;
            int intSID = 0;
            string strSMSContent = "";

            string strSQL = "SELECT TOP 1 ID,SID,SMSContent FROM Fx_OutBox WHERE [Status] = 0";
            Console.WriteLine(strSQL);//111111111111111111111111111111111
            try
            {
                Console.WriteLine("==================== [发送消息] 开始 ================== \r\n");
                DataRow dr = SqlHelper.ExecuteDataRow(SqlLibrary.GetFx_Main(), CommandType.Text, strSQL);
                intID = Convert.ToInt32(dr["ID"]);
                intSID = Convert.ToInt32(dr["SID"]);
                strSMSContent = Convert.ToString(dr["SMSContent"]);
                Console.WriteLine(intID + " " + intSID + " " + strSMSContent);//111111111111111111111111
                string[] arrSMSContent = strSMSContent.Split(new char[] { ' ' });
                Console.WriteLine(arrSMSContent.Length);//1111111111111111111111111
                if (arrSMSContent.Length > 1)
                {
                    if (intID > 0)
                    {
                        Console.WriteLine("用户状态："+UserStatus(intSID));//111111111111111111
                        if (UserStatus(intSID) == 0)
                        {
                            Console.WriteLine("SysConsole: "+intSID+"  "+arrSMSContent[0].ToLower().ToString()+"  "+arrSMSContent[1].ToLower().ToString());
                            SysConsole(intSID, arrSMSContent[0].ToLower().ToString(), arrSMSContent[1].ToLower().ToString());
                            Console.WriteLine("sysconsole runing");//11111111111111111
                            intID = SqlLibrary.Fx_UpdateMessage(intID);
                            Console.WriteLine("[信息已发出]");
                            Console.WriteLine("发送对像:" + intSID + " 命令:" + arrSMSContent[0].ToString() + " 参数:" + arrSMSContent[1].ToString() + "\r\n");
                        }
                        else
                        {
                            SqlLibrary.Fx_UpdateMessage(intID);
                            Console.WriteLine("[信息已被取消]");
                        }
                    }
                    else
                    {
                        Console.WriteLine("[信息标记出错]");
                    }
                }
                else
                {
                    SqlLibrary.Fx_UpdateMessage(intID);
                    Console.WriteLine("[信息已被取消]");
                }                
                Console.WriteLine("==================== [发送消息] 结束 ================== \r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Console.WriteLine("[没有要发送的消息]\r\n");
                Console.WriteLine("==================== [发送消息] 结束 ================== \r\n");
            }
        }
        #endregion

        #region 运行计划任务
        /// <summary>
        /// 运行计划任务
        /// </summary>
        public static void RunTask()
        {
            int intID = 0;
            int intSID = 0;
            int intCategory = 0;
            string strTask = "";
            byte byteStatus = 0;

            string strSQL = "SELECT TOP 1 ID,[SID],Category,Task,RunTime,[Status] FROM Fx_Task WHERE [Status] = 1 AND RunTime < CONVERT(Char(10),GetDate()+1,120) ORDER BY RunTime";
            try
            {
                Console.WriteLine("==================== [计划任务] 开始 ================== \r\n");

                DataRow dr = SqlHelper.ExecuteDataRow(SqlLibrary.GetFx_Main(), CommandType.Text, strSQL);
                intID = Convert.ToInt32(dr["ID"]);
                intSID = Convert.ToInt32(dr["SID"]);
                intCategory = Convert.ToInt32(dr["Category"]);
                strTask = Convert.ToString(dr["Task"]);
                byteStatus = Convert.ToByte(dr["Status"]);
                string[] arrTask = strTask.Split(new char[] { ' ' });
                Console.WriteLine(Convert.ToDateTime(dr["RunTime"]) + "<" + DateTime.Now + "&&" + byteStatus);
                if (Convert.ToDateTime(dr["RunTime"]) < DateTime.Now && byteStatus == 1)
                {
                    Console.WriteLine(">>>>>>>>>进入计划任务判断<<<<<<<<");
                    SysConsole(intSID, arrTask[0].ToLower().ToString(), arrTask[1].ToLower().ToString());
                    SqlLibrary.Fx_UpdateTaskRuntime(intID);
                    Console.WriteLine("发送对像:" + intSID + " 命令:" + arrTask[0].ToString() + " 参数:" + arrTask[1].ToString() + " 任务编号:" + intID + "\r\n");
                    Console.WriteLine("==================== [计划任务] 结束 ================== \r\n");
                }                
            }
            catch
            {
                Console.WriteLine("---[没有计划任务要执行]--- \r\n");
                Console.WriteLine("==================== [计划任务] 结束 ================== \r\n");
            }
        }
        #endregion

        #region 检查用户状态
        /// <summary>
        /// 检查用户状态
        /// </summary>
        /// <param name="intSID">用户SID</param>
        /// <returns>1\禁用 0\正常</returns>
        public static byte UserStatus(int intSID)
        {
            byte byteStatus = 0;

            string strSQL = "SELECT TOP 1 Status FROM Fx_User WHERE [SID] = " + intSID + "";
            try
            {
                DataRow dr = SqlHelper.ExecuteDataRow(SqlLibrary.GetFx_Main(), CommandType.Text, strSQL);
                byteStatus = Convert.ToByte(dr["Status"]);
                if (byteStatus == 1)
                {
                    Console.WriteLine("==================== [系统消息] 开始 ================== \r\n");
                    Console.WriteLine("UserID:" + intSID + " 已被锁定！\r\n");
                    Console.WriteLine("==================== [系统消息] 结束 ================== \r\n");
                }
                else if (byteStatus == 0)
                {
                    byteStatus = 0;
                }
            }
            catch
            {
                Console.WriteLine("==================== [系统消息] 开始 ================== \r\n");
                Console.WriteLine("UserID:" + intSID + " 查无此用户！\r\n");
                Console.WriteLine("==================== [系统消息] 结束 ================== \r\n");
            }
            return byteStatus;
        }
        #endregion
    }
}
