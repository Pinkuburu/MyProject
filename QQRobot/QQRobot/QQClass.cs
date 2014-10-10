using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace QQRobot
{
    class QQClass
    {
        //����������Ϣ
        public int i = 12;
        //����HTTP������
        private static WebClient HTTPproc = new WebClient();
        //�������������
        private static RandStr rnd = new RandStr(true, false, false, false);
        //��������
        public string strKey = null;
        public string resultString = null;
        public string strQQHost = "http://web-proxy.qq.com/conn_s";
        public string strQQ = null;
        public string strAdminQQ = "182536608";
        public bool boolDebug = true;
        
        public void Login(string strQQ,string strPwd)
        {
            //��������
            string strMsg = null;
            string strReMsg = null;
            string strRequest = null;
            string strContent = null;
            string strCookies = null;
            string strSkey = null;
            string strPtwebqq = null;
            string strPost = null;

            string[] arrResult = { };
            HTTPproc.Encoding = System.Text.Encoding.UTF8;

            this.strQQ = strQQ;
            strRequest = "http://ptlogin2.qq.com/check?uin=" + strQQ + "&appid=1002101&r=" + rnd.GetRandStr(13);
            strContent = HTTPproc.GetHtml(strRequest);

            try
            {
                this.resultString = Regex.Match(strContent, @"'\d','.*'").Value.Replace("\'", "");//'0','!MLF'
                arrResult = this.resultString.Split(',');
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("û�л�ȡ����֤������������飡");
                Console.WriteLine(ex);
            }
            Console.WriteLine("��ȡ����֤��:" + arrResult[1]);

            strCookies = HTTPproc.ResponseHeaders.GetValues(3).GetValue(0).ToString();

            try
            {
                this.resultString = Regex.Replace(strCookies, ";.*", "").Replace("ptvfsession=","");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            HTTPproc.CookieContainer.Add(new Uri("http://ptlogin2.qq.com"), new Cookie("ptvfsession", this.resultString));
            strRequest = "http://ptlogin2.qq.com/login?u=" + strQQ + "&p=" + qqPwdEncrypt.Encrypt(strPwd, arrResult[1]) + "&verifycode=" + arrResult[1] + "&remember_uin=1&aid=1002101&u1=http%3A%2F%2Fweb.qq.com%2Fmain.shtml%3Fdirect__2&h=1&ptredirect=1&ptlang=2052&from_ui=1&pttype=1&dumy=&fp=loginerroralert";

            strContent = HTTPproc.GetHtml(strRequest);

            try
            {
                this.resultString = Regex.Match(strContent, @"'\d','.*'").Value.Replace("\'", "");//'3','0','','0','��������������������ԡ�'
                arrResult = this.resultString.Split(',');
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("û�л�ȡ����֤������������飡");
                Console.WriteLine(ex);
            }

            #region ��¼����ֵ�ж�
            if (arrResult[0] == "0")        //0����¼�ɹ�!
            {
                Console.WriteLine(arrResult[4]);
                strSkey = HTTPproc.ResponseHeaders.GetValues(1).GetValue(2).ToString();
                //strPtcz = HTTPproc.ResponseHeaders.GetValues(1).GetValue(9).ToString();

                for (int j = 0; j < HTTPproc.ResponseHeaders.GetValues(1).Length; j++)
                {
                    if (HTTPproc.ResponseHeaders.GetValues(1).GetValue(j).ToString().IndexOf("ptwebqq") > -1)
                    {
                        strPtwebqq = HTTPproc.ResponseHeaders.GetValues(1).GetValue(j).ToString();
                    }
                }
            }
            else if (arrResult[0] == "1")   //1��ϵͳ��æ�����Ժ����ԡ�
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "2")   //2���Ѿ����ڵ�QQ���롣
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "3")   //3����������������������ԡ�
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "4")   //4�����������֤�����������ԡ�
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "5")   //5��У��ʧ�ܡ�
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "6")   //6�����������������޸Ĺ�����, ���Ժ��ٵ�¼.
            {
                Console.WriteLine(arrResult[4]);
            }
            else if (arrResult[0] == "7")   //7��������������, �����ԡ�
            {
                Console.WriteLine(arrResult[4]);
            }
            else                            //8������IP�������Ĵ������࣬���Ժ����ԡ�
            {
                Console.WriteLine("δ֪����");
            }
            Console.WriteLine(arrResult[1]);

            #endregion ��¼����ֵ�ж�

            strSkey = Regex.Replace(strSkey, ";.*", "").Replace("skey=", "");
            strPtwebqq = Regex.Replace(strPtwebqq, ";.*", "").Replace("ptwebqq=", "");

            strPost = this.strQQ + ";22;0;00000000;" + strSkey + ";" + strPtwebqq + ";0;";
            strContent = HTTPproc.Post(this.strQQHost, strPost);
            Console.WriteLine(strContent);

            string[] arrKey = strContent.Split(';');
            this.strKey = arrKey[4].ToString();
            
            //��ȡ�û�����            
            Console.WriteLine(GetUserGroup());

            //��ȡ�����б� 
            strPost = this.strQQ + ";06;2;" + this.strKey + ";" + this.strQQ + ";" + this.strQQ + ";5c;3;" + this.strKey + ";88;" + this.strQQ + ";67;4;" + this.strKey + ";03;1;" + this.strQQ + ";" + this.strQQ + ";58;5;" + this.strKey + ";0;" + this.strQQ + ";26;6;" + this.strKey + ";0;0;" + this.strQQ + ";3e;7;" + this.strKey + ";4;0;" + this.strQQ + ";65;8;" + this.strKey + ";02;" + this.strQQ + ";" + this.strQQ + ";1d;9;" + this.strKey + ";" + this.strQQ + ";00;10;" + this.strKey + ";";
            strContent = HTTPproc.Post(this.strQQHost, strPost);
            Console.WriteLine(strContent);
            Run();

            #region �ϰ���Ϣѭ��
            //string[] arrReMsg;

            //while (i < 100)
            //{
            //    Console.WriteLine("���ڵȴ�������Ϣ:{0}", i);
            //    strContent = HTTPproc.Post(this.strQQHost, this.strQQ + ";00;" + this.i.ToString() + ";" + this.strKey + ";");

            //    arrReMsg = strContent.Split(';');
            //    if (arrReMsg.Length > 10)
            //    {
            //        if (arrReMsg[1].ToString() == "17")
            //        {
            //            //������Ϣ
            //            //this.i++;
            //            strPost = this.strQQ + ";16;" + this.i.ToString() + ";" + this.strKey + ";" + arrReMsg[3].ToString() + ";0b;528;" + UrlEncode(arrReMsg[7].ToString(), "UTF-8") + ";0a00000010%E5%AE%8B%E4%BD%93;";
            //            strReMsg = HTTPproc.Post(this.strQQHost, strPost);
            //            Console.WriteLine("������Ϣ:{0}", strReMsg);
            //            //1307364337;16;119;165fad25;182536608;0b;528;%E4%BD%A0%E5%A5%BD;0a00000010%E5%AE%8B%E4%BD%93;

            //            //this.i++;
            //            strPost = this.strQQ + ";17;" + arrReMsg[2].ToString() + ";" + this.strKey + ";" + arrReMsg[3].ToString() + ";" + this.strQQ + ";" + arrReMsg[4].ToString() + ";1;" + arrReMsg[10].ToString() + ";";
            //            strMsg = HTTPproc.Post(this.strQQHost, strPost);//" + this.strQQ + ";00;" + this.i.ToString() + ";" + this.strKey + ";");
            //            //
            //            Console.WriteLine("����ȷ����Ϣ:{0}", strMsg);
            //        }
            //    }
            //    Console.WriteLine(strContent);
            //    this.i++;
            //}
            #endregion
        }

        #region ��ȡ�û����� GetUserGroup()
        /// <summary>
        /// ��ȡ�û�����
        /// </summary>
        /// <returns></returns>
        public string GetUserGroup()
        {
            string strUserGroup = null;
            string strPost = null;
            //��ȡ����
            strPost = this.strQQ + ";3c;0;" + this.strKey + ";1;" + this.strQQ + ";00;1;" + this.strKey + ";";
            //strPost = this.strQQ + ";3c;0;" + this.strKey + ";1;";
            strUserGroup = HTTPproc.Post(this.strQQHost, strPost);
            return "������Ϣ:" + strUserGroup;
        }

        #endregion ��ȡ�û����� GetUserGroup()

        #region ��ȡ�����б� GetFriendList()
        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <returns></returns>
        public string GetFriendList()
        {
            string strFriendList = null;
            string strPost = null;
            //��ȡ�����б�
            strPost = this.strQQ + ";58;0;" + this.strKey + ";0;";
            strFriendList = HTTPproc.Post(this.strQQHost, strPost);
            return strFriendList;
        }

        #endregion ��ȡ�����б� GetFriendList()

        #region Robot�ػ����� Run()
        /// <summary>
        /// Robot�ػ�����
        /// </summary>
        public void Run()
        {
            bool boolRun = true;
            string strContent = null;
            string[] arrTemp = { };
            string[] arrCommand = { };
            while (boolRun)
            {
                try
                {
                    strContent = HTTPproc.Post(this.strQQHost, this.strQQ + ";00;" + this.i.ToString() + ";" + this.strKey + ";");
                }
                catch
                {
                    ShowCommand("������Ϣ��ѯ", "Debug");
                    strContent = HTTPproc.Post(this.strQQHost, this.strQQ + ";00;" + this.i.ToString() + ";" + this.strKey + ";");
                }
                ShowCommand("��Ϣ��ѯ: " + strContent, "Debug");
                arrCommand = strContent.Split('');

                try
                {
                    foreach (String strMessage in arrCommand)
                    {
                        arrTemp = strMessage.Split(';');
                        switch (arrTemp[1].ToString())
                        {
                            case "16":
                                SendMessage(arrTemp[3].ToString(), arrTemp[7].ToString());
                                break;
                            case "17":
                                ReMessage(strMessage);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch
                {
 
                }
                this.i++;
            }
        }
        #endregion Robot�ػ����� Run()

        #region ������Ϣ SendMessage(string strQQ, string strMsg)
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="strQQ"></param>
        /// <param name="strMsg"></param>
        private void SendMessage(string strQQ, string strMsg)
        {
            string strContent = null;
            string strPost = null;

            this.i++;
            //������Ϣ
            strPost = this.strQQ + ";16;" + this.i.ToString() + ";" + this.strKey + ";" + strQQ + ";0b;528;" + UrlEncode(strMsg, "UTF-8") + ";0a00000010%E5%AE%8B%E4%BD%93;";
            strContent = HTTPproc.Post(this.strQQHost, strPost);
            ShowCommand("������Ϣ:" + strContent, "Debug");
        }
        #endregion ������Ϣ SendMessage(string strQQ, string strMsg)

        #region ������Ϣ ReMessage(string strReMsg)
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="strReMsg"></param>
        private void ReMessage(string strReMsg)
        {
            string[] arrReMsg = { };
            string strContent = null;
            string strPost = null;
            arrReMsg = strReMsg.Split(';');

            //��ʾ������Ϣ
            ShowCommand("�û���Ϣ:" + arrReMsg[3].ToString() + "˵:" + arrReMsg[7].ToString(), "User");

            //��Ϣ����
            Command(arrReMsg[3].ToString(),arrReMsg[7].ToString());

            this.i++;
            //������Ϣ����
            strPost = this.strQQ + ";17;" + arrReMsg[2].ToString() + ";" + this.strKey + ";" + arrReMsg[3].ToString() + ";" + this.strQQ + ";" + arrReMsg[4].ToString() + ";1;" + arrReMsg[10].ToString() + ";";//" + this.strQQ + ";00;" + this.i.ToString() + ";" + this.strKey + ";";
                        //1349836289;17;                         24634;           0878813c;                     182536608;        1349836289;                        556257;1;3565623212;
            strContent = HTTPproc.Post(this.strQQHost, strPost);
            ShowCommand("������Ϣ:" + strContent, "Debug");
        }
        #endregion ������Ϣ ReMessage(string strReMsg)

        private void Command(string strQQ, string strCmd)
        {
            string strContent = null;
            string[] arrCmd = { };
            if (strCmd.IndexOf("@") == 0)
            {
                arrCmd = strCmd.Trim().Split(' ');
                switch (arrCmd[0].ToString())
                {
                    case "@":
                        ShowCommand("��ʾ����������", "Cmd");
                        strContent = "���ã��Ҿ��Ǵ�˵�еؿۿۻ�����%0A" +
                                     "=========������ʹ����������=========%0A" +
                                     "@Sign              �޸Ļ�����ǩ��%0A" +
                                     "@NickName     �޸Ļ������ǳ�%0A" +
                                     "@Status          ����״̬(Online,Offline,Hidden,Exit)";
                        SendMessage(strQQ, strContent);
                        break;
                    case "@Sign":   //�޸�ǩ��
                        ModifySign(arrCmd[1].ToString());
                        ShowCommand("ǩ���޸�Ϊ:" + arrCmd[1].ToString(), "Cmd");
                        SendMessage(strQQ, "ǩ���޸�Ϊ:" + arrCmd[1].ToString());
                        break;
                    case "@NickName":   //�ǳ�ǩ��
                        ModifyUserInfo(arrCmd[1].ToString());
                        ShowCommand("�ǳ��޸�Ϊ:" + arrCmd[1].ToString(), "Cmd");
                        SendMessage(strQQ, "�ǳ��޸�Ϊ:" + arrCmd[1].ToString());
                        break;
                    case "@Status":     //�޸�����״̬
                        OnlineStatus(arrCmd[1].ToString());
                        ShowCommand("�޸ĵ�ǰ״̬Ϊ:" + arrCmd[1].ToString(), "Cmd");
                        SendMessage(strQQ, "�޸ĵ�״̬Ϊ:" + arrCmd[1].ToString());
                        break;
                    case "@Add":        //��Ӻ���
                        AddFriend(arrCmd[1].ToString());
                        ShowCommand("��Ӻ���:" + arrCmd[1].ToString(), "Cmd");
                        SendMessage(strQQ, "��Ӻ���:" + arrCmd[1].ToString());
                        break;
                    default:
                        ShowCommand("�����������������ʽ��������", "Cmd");
                        SendMessage(strQQ, "�����������������ʽ��������");//%0A ����
                        break;
                }
            }
        }

        #region �޸��ǳ� ModifyUserInfo(string strNickName)
        /// <summary>
        /// �޸��ǳ�
        /// </summary>
        /// <param name="strNickName"></param>
        private void ModifyUserInfo(string strNickName)
        {
            string strPost = null;
            this.i++;
            strPost = this.strQQ + ";04;" + this.i + ";" + this.strKey + ";" + strNickName + "------0---------1--528------000-000;";
            //strPost = this.strQQ + ";04;" + this.i + ";" + this.strKey + ";" + strNickName + "�й�ɽ��----0---------1--528---�ൺ--000-000;";
            //1349836289;04;281;d0dd2dac;Test�й�ɽ��----0---------1--528---�ൺ--000-000;
            HTTPproc.Post(this.strQQHost, strPost);
        }
        #endregion �޸��ǳ� ModifyUserInfo(string strNickName)

        #region �޸�ǩ�� ModifySign(string strSign)
        /// <summary>
        /// �޸�ǩ��
        /// </summary>
        /// <param name="strSign">ǩ������</param>
        private void ModifySign(string strSign)
        {
            string strPost = this.strQQ + ";67;" + this.i + ";" + this.strKey + ";01;" + strSign + ";";
            //1349836289;67;512;ad7e15a4;01;Tests;
            //1349836289;67;14;701f4ae8;01;Tests;
            //1349836289;67;13;eac1407d;01;Cupid;
            HTTPproc.Post(this.strQQHost, strPost);
        }
        #endregion �޸�ǩ�� ModifySign(string strSign)

        #region ��ʾ������ ShowCommand(string strContent, string strColor)
        /// <summary>
        /// ��ʾ������
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="strColor"></param>
        private void ShowCommand(string strContent, string strColor)
        {
            if (this.boolDebug)
            {
                switch (strColor)
                {
                    case "Debug":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(strContent);
                        break;
                    case "User":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(strContent);
                        break;
                    case "Cmd":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(strContent);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(strContent);
                        break;
                }                
            }
        }
        #endregion ��ʾ������ ShowCommand(string strContent, string strColor)

        #region �޸�����״̬ OnlineStatus(string strStatus)
        /// <summary>
        /// �޸�����״̬
        /// </summary>
        /// <param name="strStatus"></param>
        private void OnlineStatus(string strStatus)
        {
            string strPost = null;
            switch (strStatus)
            {
                case "Online":
                    //1349836289;0d;25;e76d930c;10; ����
                    strPost = this.strQQ + ";0d;" + this.i + ";" + this.strKey + ";10;";
                    HTTPproc.Post(this.strQQHost, strPost);
                    break;
                case "Offline":
                    //1349836289;0d;16;e76d930c;30; ����
                    strPost = this.strQQ + ";0d;" + this.i + ";" + this.strKey + ";30;";
                    HTTPproc.Post(this.strQQHost, strPost);
                    break;
                case "Hidden":
                    //1349836289;0d;33;e76d930c;40; ����
                    strPost = this.strQQ + ";0d;" + this.i + ";" + this.strKey + ";40;";
                    HTTPproc.Post(this.strQQHost, strPost);
                    break;
                case "Exit":
                    //1349836289;0d;39;e76d930c;20; �뿪
                    strPost = this.strQQ + ";0d;" + this.i + ";" + this.strKey + ";20;";
                    HTTPproc.Post(this.strQQHost, strPost);
                    break;
            }
        }
        #endregion �޸�����״̬ OnlineStatus(string strStatus)

        #region ��Ӻ���(����) AddFriend(string strF_QQ, string strContent)
        /// <summary>
        /// ��Ӻ���(����)
        /// </summary>
        /// <param name="strF_QQ"></param>
        /// <param name="strContent"></param>
        private void AddFriend(string strF_QQ)
        {
            string strPost = null;
            //��Ӻ���(����)
            //1349836289;a8;33;701f4ae8;2;182536608;1;0;Test;
            //1349836289;a8;58;701f4ae8;2;182536608;1;0;Test;
            strPost = this.strQQ + ";a8;" + this.i + ";" + this.strKey + ";2;" + strF_QQ + ";1;0;" + UrlEncode("����QQ������", "UTF-8") + ";";
            HTTPproc.Post(this.strQQHost, strPost);
        }

        #endregion ��Ӻ���(����) AddFriend(string strF_QQ, string strContent)

        //�յ����Է���Ϊ����(����)
        //1349836289;80;51939;43;1307364337;1349836289;0;1288756278;  �����Ӻ��Ѳ��ҶԷ�ͬ����Ӻ���ʱ���ص�ȷ��
        //1349836289;80;23152;43;1307364337;1349836289;0;1288758359;  �����Ӻ��Ѳ��ҶԷ�ͬ����Ӻ���ʱ���ص�ȷ��

        //1349836289;80;28300;41;1307364337;1349836289;Test;1;1288770561;  ��������Ӻ��ѷ��ص���֤��Ϣ
        //1349836289;80;15959;41;1307364337;1349836289;Test;1;1288770749;  ��������Ӻ��ѷ��ص���֤��Ϣ

        //����Ӻ��Ѵ���
        //1349836289;a8;74;f60704dd;3;1307364337;0;	ͬ����˼����Ӻ��ѷ���
        //1349836289;81;32628;1307364337;10;3;	ͬ�ⱻ��Ӻ��Ѻ��ҶԷ���֤ȷ�Ϻ󷵻�

        //�յ�����ɾ��֪ͨ
        //1349836289;81;32530;1307364337;5122;3;
        //1349836289;81;38986;1307364337;5122;3;
        //1349836289;81;52870;1307364337;5122;3;



        //��ѯ������Ϣ
        //1349836289;06;31;c41a0b3f;1307364337;
        //1349836289;06;160;c41a0b3f;1307364337;

        //��ѯ������Ϣ
        //��ѯ 
        //1349836289;0115;158;c41a0b3f;3;1307364337;
        //1349836289;0115;183;c41a0b3f;3;1301111111;
        //����
        //1349836289;0115;158;3;0;1307364337;��������;528;3;�й�-ɽ��-�ൺ;
        //����
        //1349836289;0115;183;3;0;  --û���ҵ�  ���޲��ҽ��!

        //��ѯ����
        //1349836289;3c;0;2a0d205c;1;

        //��ȡ�û��ȼ���Ϣ
        //1349836289;5c;3;2a0d205c;88;
        //����
        //1349836289;5c;3;88;0;0       ;3       ;0;2;
        //                    ;�û��ȼ�;��������; ;ʣ����������;    

        //��ȡ�û������б�(���غ���QQ�ţ��ǳ�)
        //1307364337;26;6;a3015db3;0;0;
        //����
        //1307364337;26;6;0;4111852;12;29;0;С��;0;182536608;297;24;0;С����;1;790812856;432;22;0;�� ��;1;800015686;0;0;255;XBA��Ϸ��;0;907477127;399;19;1;��������KE;0;1093616771;558;24;1;YY;0;

        //��ȡ�û������б�(�û�����״̬����)
        //1349836289;58;5;2a0d205c;0;
        //����
        //1349836289;58;5;0;182536608;0;0;20;3;

        #region QQ��������㷨
        /// <summary>
        /// QQ��������㷨
        /// </summary>
        private static class qqPwdEncrypt
        {
            /// <summary>
            /// ������ҳ��QQ��¼ʱ������ܺ�Ľ��
            /// </summary>
            /// <param name="pwd" />QQ����</param>
            /// <param name="verifyCode" />��֤��</param>
            /// <returns></returns>
            public static String Encrypt(string pwd, string verifyCode)
            {
                return (md5(md5_3(pwd).ToUpper() + verifyCode.ToUpper())).ToUpper();
            }
            /// <summary>
            /// �����ַ���������MD5
            /// </summary>
            /// <param name="s" /></param>
            /// <returns></returns>
            private static String md5_3(String s)
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);

                bytes = md5.ComputeHash(bytes);
                bytes = md5.ComputeHash(bytes);
                bytes = md5.ComputeHash(bytes);

                md5.Clear();

                string ret = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
                }

                return ret.PadLeft(32, '0');
            }
            /// <summary>
            /// �����ַ�����һ��MD5
            /// </summary>
            /// <param name="s" /></param>
            /// <returns></returns>
            private static String md5(String s)
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);

                bytes = md5.ComputeHash(bytes);

                md5.Clear();

                string ret = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
                }

                return ret.PadLeft(32, '0');
            }
        }
        #endregion QQ��������㷨

        #region URL���� UrlEncode(string str, string encode)
        /// <summary>
        /// URL����
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private static string UrlEncode(string str, string encode)
        {
            int factor = 0;
            if (encode == "UTF-8")
                factor = 3;
            if (encode == "GB2312")
                factor = 2;
            //����Ҫ������ַ�

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.%";
            System.Text.Encoder encoder = System.Text.Encoding.GetEncoding(encode).GetEncoder();
            char[] c1 = str.ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //һ���ַ�һ���ַ��ı���

            for (int i = 0; i < c1.Length; i++)
            {
                //����Ҫ����

                if (okChar.IndexOf(c1[i]) > -1)
                    sb.Append(c1[i]);
                else
                {
                    byte[] c2 = new byte[factor];
                    int charUsed, byteUsed; bool completed;

                    encoder.Convert(c1, i, 1, c2, 0, factor, true, out charUsed, out byteUsed, out completed);

                    foreach (byte b in c2)
                    {
                        if (b != 0)
                            sb.AppendFormat("%{0:X}", b);
                    }
                }
            }
            return sb.ToString().Trim();
        }
        #endregion

        #region ������� ���������֡����� RandStr
        /// <summary>
        /// ������� ���������֡�����
        /// </summary>
        public class RandStr
        {
            private string framerStr = null;
            private string numStr = "0123456789";
            private string upperStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            private string lowerStr = "abcdefghijklmnopqrstuvwxyz";
            private string markStr = @"`-=[];'\,./~!@#$%^&*()_+{}:""|<>?";
            private static Random myRandom = new Random();

            /// <summary>
            /// ��δ�ṩ��������,��Ĭ��������+Сд��ĸ����
            /// </summary>
            public RandStr()
            {
                framerStr = numStr + lowerStr;
            }

            /// <summary>
            /// ���캯��,��ָ�����ɵ��ַ�
            /// </summary>
            /// <param name="useNum">�Ƿ�ʹ������</param>
            /// <param name="useUpper">�Ƿ�ʹ�ô�д��ĸ</param>
            /// <param name="useLower">�Ƿ�ʹ��Сд��ĸ</param>
            /// <param name="useMark">�Ƿ�ʹ�÷���</param>
            public RandStr(bool useNum, bool useUpper, bool useLower, bool useMark)
            {
                // �����ͼ���첻�����κ�����ַ�����,���׳��쳣
                if (!useNum && !useUpper && !useLower && !useMark)
                {
                    throw new ArgumentException("��������ʹ��һ�ֹ����ַ�!");
                }
                else
                {
                    if (useNum)
                        framerStr += numStr;
                    if (useUpper)
                        framerStr += upperStr;
                    if (useLower)
                        framerStr += lowerStr;
                    if (useMark)
                        framerStr += markStr;
                }
            }

            /// <summary>
            /// ʹ���Զ��������ַ�����
            /// </summary>
            /// <param name="userStr">�Զ����ַ�</param>
            public RandStr(string userStr)
            {
                // �����ͼ�ÿ��ַ���������,���׳��쳣
                if (userStr.Length == 0)
                {
                    throw new ArgumentException("������ʹ��һ���ַ�!");
                }
                else
                {
                    framerStr = userStr;
                }
            }

            /// <summary>
            /// ȡ��һ������ַ���
            /// </summary>
            /// <param name="length">ȡ������ַ����ĳ���</param>
            /// <returns>���ص�����ַ���</returns>
            public string GetRandStr(int length)
            {
                // ��ȡ�ĳ��Ȳ���Ϊ0�����߸�����
                if (length < 1)
                {
                    throw new ArgumentException("�ַ����Ȳ���Ϊ0���߸���!");
                }
                else
                {
                    // ���ֻ�ǻ�ȡ��������ַ���,
                    // ����û������.
                    // �������Ҫ��ʱ���ȡ��������ַ����Ļ�,
                    // �����������ܲ���.
                    // ���Ը���StringBuilder�����������,
                    // ��Ҫ�Ŀ����Լ���һ�� ^o^
                    string tempStr = null;
                    for (int i = 0; i < length; i++)
                    {
                        int randNum = myRandom.Next(framerStr.Length);
                        tempStr += framerStr[randNum];
                    }
                    return tempStr;
                }
            }
        }
        #endregion ������� ���������֡����� RandStr

        #region ʱ��� Timestamp()
        /// <summary>
        /// ʱ���
        /// </summary>
        /// <returns></returns>
        private long Timestamp()
        {
            long longTimestamp = 0;
            longTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return longTimestamp;
        }
        #endregion ʱ��� Timestamp()
    }
}
