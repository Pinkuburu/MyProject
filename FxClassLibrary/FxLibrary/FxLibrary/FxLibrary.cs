using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using FxModule;
using System.Text.RegularExpressions;
using System.Data;
/*
            ����б�

    1�� plugin_result %1 %2 %3
    ������ܣ�ִ�н������
    ����˵����
    1���������ֻ�����
    2������id
    3��������(base64 encoded)

    2�� plugin_notify %1 %2
    ������ܣ�ͨ�õ�ϵͳ֪ͨ���ò���ѷ��ŷ��������ݹ�����ԭʼ����ת���������
    ����˵����
    1���������ֻ�����
    2��������(base64�����xml����)

    3�� plugin_buddy_invite %1 %2
    ������ܣ��Է�����Ի�ʱ��˫���򿪴���ʱ��һ���ʱ���Է��ͻ����˲˵���
    ����˵����
    1���������ֻ�����
    2���Է�URI(base64 encode)

    4��plugin_buddy_data %1 %2
    ������ܣ�buddy���ϴ���
    ����˵����
    1���������ֻ�����
    2��base64������û�����
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

    5��plugin_ handle_contact_request %1 %2
    ������ܣ����µĺ��Ѽ��루Ϊ��ͻ���������ƣ���ʱ�����ڱ����й����ݺ�ɾ�����û���
    ����˵����
    1���������ֻ�����
    2��base64����ĺ���URI

    6��plugin_system_message %1 %2
    ������ܣ�ϵͳ֪ͨ��Ϣ
    ����˵����
    1���������ֻ�����
    2��base64�������Ϣ

    7��plugin_message %1 %2 %3 %4
    ������ܣ���Ϣ
    ����˵����
    1���������ֻ�����
    2��base64�����URI
    3��base64�������Ϣ
    4��base64�������Ϣ����(text/html text/plain) ���Ը��ݴ˲�����ȷ����Ϣ����pc�˻����ֻ��� ��������

    8��plugin_timer %1 %2
    ������ܣ�10���Ӽ���һ��
    ����˵����
    1���������ֻ�����
    2������������������������Ǿ�ȷ��                       */

//=============================================
//��    ��:	HZW
//����ʱ��:	2009-7-21
//��������:	FxClassLibrary
//=============================================

namespace FxLibrary
{
    public class Fetion
    {
        #region ��ȡ������
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="code">code</param>
        /// <returns></returns>
        public string readSendUser(string code)
        {
            string decode = "";

            //Base64���룬ע��UTF8
            code = Base64_decode(code);
            byte[] bytes = Convert.FromBase64String(code);
            decode = System.Text.Encoding.UTF8.GetString(bytes);
            //��ôȥ����������ݣ�sip:78516401@fetion.com.cn;p=187
            //�����õı��취���������������Ķ����ַ�ȥ����Ȼ�����󣳸��ַ������˭�и��õİ취�������
            decode = Convert.ToString(Regex.Match(decode, @"\d{4,10}@fetion\.com\.cn"));
            //decode = decode.Substring(0, decode.Length - 3);
            return decode;
        }
        #endregion

        #region ��ȡ��������
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string readSMSContent(string code)
        {

            //Base64���룬ע��UTF8
            string decode = "";
            code = Base64_decode(code);
            if (code != "")
            {
                byte[] bytes = Convert.FromBase64String(code);
                try
                {
                    decode = System.Text.Encoding.UTF8.GetString(bytes);
                    //ʹ��PC�ͻ��ˣ���html���ֻ�û�У���ʵ���͵Ļ�û�в��ԡ�
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

        #region ��������λ�����㲢����
        /// <summary>
        /// ��������λ�����㲢����
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

        #region ��ʱ��
        /// <summary>
        /// ��ʱ��
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
            //Console.WriteLine("�¼������ѷ�����");
        }
        #endregion

        #region ִ�н������
        /// <summary>
        /// ִ�н������
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string cmdResult(string code)
        {

            //Base64���룬ע��UTF8
            string decode = "";
            code = Base64_decode(code);
            if (code != "")
            {
                byte[] bytes = Convert.FromBase64String(code);
                try
                {
                    decode = System.Text.Encoding.UTF8.GetString(bytes);
                    //ʹ��PC�ͻ��ˣ���html���ֻ�û�У���ʵ���͵Ļ�û�в��ԡ�
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

        #region ͨ�õ�ϵͳ֪ͨ
        /// <summary>
        /// ͨ�õ�ϵͳ֪ͨ
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string sysNotify(string code)
        {

            //Base64���룬ע��UTF8
            string decode = "";
            code = Base64_decode(code);
            if (code != "")
            {
                byte[] bytes = Convert.FromBase64String(code);
                try
                {
                    decode = System.Text.Encoding.UTF8.GetString(bytes);
                    //ʹ��PC�ͻ��ˣ���html���ֻ�û�У���ʵ���͵Ļ�û�в��ԡ�
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

        #region �û�����
        /// <summary>
        /// �û�����
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string userInfo(string code)
        {

            //Base64���룬ע��UTF8
            string decode = "";
            code = Base64_decode(code);
            if (code != "")
            {
                byte[] bytes = Convert.FromBase64String(code);
                try
                {
                    decode = System.Text.Encoding.UTF8.GetString(bytes);
                    //ʹ��PC�ͻ��ˣ���html���ֻ�û�У���ʵ���͵Ļ�û�в��ԡ�
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

        #region ϵͳ��Ϣ
        /// <summary>
        /// ϵͳ��Ϣ
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string sysMessage(string code)
        {

            //Base64���룬ע��UTF8
            string decode = "";
            code = Base64_decode(code);
            if (code != "")
            {
                byte[] bytes = Convert.FromBase64String(code);
                try
                {
                    decode = System.Text.Encoding.UTF8.GetString(bytes);
                    //ʹ��PC�ͻ��ˣ���html���ֻ�û�У���ʵ���͵Ļ�û�в��ԡ�
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

        #region ��ȡXML��ֵ
        /// <summary>
        /// ��ȡXML��ֵ
        /// </summary>
        /// <param name="xmldata"></param>
        public string ReadXmlTextValue(string xmldata, string NodeName)
        {
            //status = 400 ����
            //status = 100 �뿪
            //status = 600 æµ

            //����XmlDocument����
            XmlDocument xmlDoc = new XmlDocument();
            //����xml�ļ���
            //xmlDoc.Load(filename);
            //�����xml�ַ���������������ʽ
            xmlDoc.LoadXml(xmldata);

            //��ȡ���ڵ�������ӽڵ㣬�ŵ�xn0�� 
            XmlNodeList xn0 = xmlDoc.SelectSingleNode("buddy").ChildNodes;

            //���Ҷ����ڵ�����ݻ����� 
            //string strPhoneNum = "";
            //string strNickname = "";
            //string strSid = "";
            string strNodeValue = "";

            foreach (XmlNode node in xn0)
            {
                //if (node.Name == "sid")
                //{
                //    //string innertext = node.InnerText.Trim();//ƥ������ڵ������
                //    //string attr = node.Attributes[0].ToString();//����
                //    strSid = node.InnerText.Trim();//sid
                //}
                //if (node.Name == "mobile")
                //{
                //    strPhoneNum = node.InnerText.Trim();//�绰
                //}
                if (node.Name == NodeName)
                {
                    strNodeValue = node.InnerText.Trim();//�ǳ�
                }
            }
            return strNodeValue;
        }
        #endregion

        #region ���ſ���̨
        /// <summary>
        /// ���ſ���̨
        /// </summary>
        /// <param name="intSID">���ͷ���SID</param>
        /// <param name="Command">ִ������</param>
        /// <param name="Parameter">����</param>
        public static void SysConsole(int intSID, string Command, string Parameter)
        {
            string strCmdResult = "";
            string strSID = Convert.ToString(intSID);
            switch (Command)
            {
                case "tq":
                    Console.WriteLine("���� SysConsole.tq");////////////////////////////
                    strCmdResult = Module.Weather(Parameter);
                    Console.WriteLine("���÷�������Ԥ��  " + Parameter);//////////////////////////
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
                            Module.CreateTxt(strSID, "��������û����������!", 1);
                            break;
                        }
                        else
                        {
                            Module.CreateTxt(strSID, "���������������µ���\n" + strCmdResult, 1);
                            break;
                        }                        
                    }
                    else if (Parameter == "bbst")
                    {
                        strCmdResult = Module.GetBasketBallStatus();
                        if (strCmdResult == "False")
                        {
                            Module.CreateTxt(strSID, "����ҹ�����������", 1);
                            break;
                        }
                        else
                        {
                            Module.CreateTxt(strSID, "����ҹ����³������\n" + strCmdResult, 1);
                            break;
                        }   
                    }
                    else if (Parameter == "bbup")
                    {
                        strCmdResult = Module.GetBasketBallUpdateNightCheck();
                        if (strCmdResult == "False")
                        {
                            Module.CreateTxt(strSID, "����ҹ����¼��������", 1);
                            break;
                        }
                        else
                        {
                            Module.CreateTxt(strSID, "����ҹ����³������\n" + strCmdResult, 1);
                            break;
                        }
                    }
                    else if (Parameter == "fb")
                    {
                        strCmdResult = Module.GetFootBallTurn();
                        if (strCmdResult == "False")
                        {
                            Module.CreateTxt(strSID, "��������û����������!", 1);
                            break;
                        }
                        else
                        {
                            Module.CreateTxt(strSID, "���������������µ���\n" + strCmdResult, 1);
                            break;
                        }   
                    }
                    else if (Parameter == "fbst")
                    {
                        strCmdResult = Module.GetFootBallStatus();
                        if (strCmdResult == "False")
                        {
                            Module.CreateTxt(strSID, "����ҹ�����������", 1);
                            break;
                        }
                        else
                        {
                            Module.CreateTxt(strSID, "����ҹ����³������\n" + strCmdResult, 1);
                            break;
                        }   
                    }
                    else if (Parameter == "fbup")
                    {
                        strCmdResult = Module.GetFootBallUpdateNightCheck();
                        if (strCmdResult == "False")
                        {
                            Module.CreateTxt(strSID, "����ҹ����¼��������", 1);
                            break;
                        }
                        else
                        {
                            Module.CreateTxt(strSID, "����ҹ����³������\n" + strCmdResult, 1);
                            break;
                        }
                    }
                    break;
                case "ts":  //Ӣ����
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

        #region �洢��Ϣ
        /// <summary>
        /// �洢��Ϣ
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
                    Console.WriteLine("[�յ�:" + intSID + "��Ϣ��]\r\n");
                }
                else
                {
                    Console.WriteLine("[����:" + intSID + "����]\r\n");
                }
            }
            else
            {
                Console.WriteLine("[�û�����SID����]");
            }
        }
        #endregion

        #region ������Ϣ
        /// <summary>
        /// ������Ϣ
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
                Console.WriteLine("==================== [������Ϣ] ��ʼ ================== \r\n");
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
                        Console.WriteLine("�û�״̬��"+UserStatus(intSID));//111111111111111111
                        if (UserStatus(intSID) == 0)
                        {
                            Console.WriteLine("SysConsole: "+intSID+"  "+arrSMSContent[0].ToLower().ToString()+"  "+arrSMSContent[1].ToLower().ToString());
                            SysConsole(intSID, arrSMSContent[0].ToLower().ToString(), arrSMSContent[1].ToLower().ToString());
                            Console.WriteLine("sysconsole runing");//11111111111111111
                            intID = SqlLibrary.Fx_UpdateMessage(intID);
                            Console.WriteLine("[��Ϣ�ѷ���]");
                            Console.WriteLine("���Ͷ���:" + intSID + " ����:" + arrSMSContent[0].ToString() + " ����:" + arrSMSContent[1].ToString() + "\r\n");
                        }
                        else
                        {
                            SqlLibrary.Fx_UpdateMessage(intID);
                            Console.WriteLine("[��Ϣ�ѱ�ȡ��]");
                        }
                    }
                    else
                    {
                        Console.WriteLine("[��Ϣ��ǳ���]");
                    }
                }
                else
                {
                    SqlLibrary.Fx_UpdateMessage(intID);
                    Console.WriteLine("[��Ϣ�ѱ�ȡ��]");
                }                
                Console.WriteLine("==================== [������Ϣ] ���� ================== \r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Console.WriteLine("[û��Ҫ���͵���Ϣ]\r\n");
                Console.WriteLine("==================== [������Ϣ] ���� ================== \r\n");
            }
        }
        #endregion

        #region ���мƻ�����
        /// <summary>
        /// ���мƻ�����
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
                Console.WriteLine("==================== [�ƻ�����] ��ʼ ================== \r\n");

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
                    Console.WriteLine(">>>>>>>>>����ƻ������ж�<<<<<<<<");
                    SysConsole(intSID, arrTask[0].ToLower().ToString(), arrTask[1].ToLower().ToString());
                    SqlLibrary.Fx_UpdateTaskRuntime(intID);
                    Console.WriteLine("���Ͷ���:" + intSID + " ����:" + arrTask[0].ToString() + " ����:" + arrTask[1].ToString() + " ������:" + intID + "\r\n");
                    Console.WriteLine("==================== [�ƻ�����] ���� ================== \r\n");
                }                
            }
            catch
            {
                Console.WriteLine("---[û�мƻ�����Ҫִ��]--- \r\n");
                Console.WriteLine("==================== [�ƻ�����] ���� ================== \r\n");
            }
        }
        #endregion

        #region ����û�״̬
        /// <summary>
        /// ����û�״̬
        /// </summary>
        /// <param name="intSID">�û�SID</param>
        /// <returns>1\���� 0\����</returns>
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
                    Console.WriteLine("==================== [ϵͳ��Ϣ] ��ʼ ================== \r\n");
                    Console.WriteLine("UserID:" + intSID + " �ѱ�������\r\n");
                    Console.WriteLine("==================== [ϵͳ��Ϣ] ���� ================== \r\n");
                }
                else if (byteStatus == 0)
                {
                    byteStatus = 0;
                }
            }
            catch
            {
                Console.WriteLine("==================== [ϵͳ��Ϣ] ��ʼ ================== \r\n");
                Console.WriteLine("UserID:" + intSID + " ���޴��û���\r\n");
                Console.WriteLine("==================== [ϵͳ��Ϣ] ���� ================== \r\n");
            }
            return byteStatus;
        }
        #endregion
    }
}
