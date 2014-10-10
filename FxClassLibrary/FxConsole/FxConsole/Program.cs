using System;
using System.Collections.Generic;
using System.Text;
using FxLibrary;
using FxModule;
using System.Threading;

namespace Management
{
    class Program
    {
        static void Main(string[] args)
        //static void Main()
        {
            string strMobileNo = "";
            string strTimer = "";
            string strURI = "";
            string strMsg = "";
            string strCmdId = "";
            string strResult = "";
            string strSystemNotify = "";
            string strUserInfo = "";
            string strSIDValue = "sid";
            string strServerName = "";
            string strServerStatus = "";

            int intSID = 0;

            Fetion fx = new Fetion();
            DateTime dt = DateTime.Now;

            switch (args[0].ToString())
            {
                case "evResult":          //ִ�н������
                    strMobileNo = args[1].ToString();
                    strCmdId = args[2].ToString();
                    strResult = fx.cmdResult(args[3]);

                    Console.WriteLine("================= [ִ�н������] ��ʼ ================= \r\n");                    
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>> " + strMobileNo + " <<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("����ID:" + strCmdId + "\r\n");
                    Console.WriteLine("������:" + strResult + "\r\n");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("================= [ִ�н������] �˳� ================= \r\n");
                    break;
                case "evNotify":          //ͨ�õ�ϵͳ֪ͨ���ò���ѷ��ŷ��������ݹ�����ԭʼ����ת���������
                    strMobileNo = args[1].ToString();
                    strSystemNotify = fx.sysNotify(args[2]);

                    Console.WriteLine("========= [���ݻ����˲�ʶ���ԭʼ���ݰ�] ��ʼ ========= \r\n");                    
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>> " + strMobileNo + " <<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("����:" + strSystemNotify + "\r\n");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("========= [���ݻ����˲�ʶ���ԭʼ���ݰ�] �˳� ========= \r\n");
                    break;
                case "evBuddyInvite":     //�Է�����Ի�ʱ��˫���򿪴���ʱ��һ���ʱ���Է��ͻ����˲˵���
                    strMobileNo = args[1].ToString();
                    strURI = fx.readSendUser(args[2]);

                    Console.WriteLine("=================== [�����¼�] ��ʼ =================== \r\n");                    
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>> " + strMobileNo + " <<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("�û�SIP:" + strURI + "\r\n");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("=================== [�����¼�] �˳� =================== \r\n");
                    break;
                case "evBuddyData":       //buddy���ϴ���
                    strMobileNo = args[1].ToString();
                    strUserInfo = fx.userInfo(args[2]);
                    intSID = Convert.ToInt32(fx.ReadXmlTextValue(strUserInfo, strSIDValue));

                    Console.WriteLine("=================== [���ϴ���] ��ʼ =================== \r\n");                    
                    if (intSID == 0)
                    {
                        Console.WriteLine("���������û�SID����:" + intSID + "��������\r\n");
                    }
                    else
                    {
                        if (SqlLibrary.Fx_AddNewUser(intSID) == 1)
                        {
                            Console.WriteLine("��������������û�:" + intSID + "�ɹ� ��������\r\n");
                        }
                        else
                        {
                            Console.WriteLine("�����������û�:" + intSID + "����� ��������\r\n");
                        }
                    }

                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>> " + strMobileNo + " <<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("�û�����:" + strUserInfo + "\r\n");
                    Console.WriteLine("�û�SID:" + intSID + "\r\n");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("=================== [���ϴ���] �˳� =================== \r\n");
                    break;
                case "evNewUserRequest":  //���µĺ��Ѽ��루Ϊ��ͻ���������ƣ���ʱ�����ڱ����й����ݺ�ɾ�����û���
                    strMobileNo = args[1].ToString();
                    strURI = fx.readSendUser(args[2]);

                    Console.WriteLine("================ [���µĺ��Ѽ���] ��ʼ ================ \r\n");                    
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>> " + strMobileNo + " <<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("�û�SIP:" + strURI + "\r\n");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<< \r\n");
                    Console.WriteLine("================ [���µĺ��Ѽ���] �˳� ================ \r\n");
                    break;
                case "evMessage":         //��Ϣ
                    strMobileNo = args[1].ToString();       //�ֻ���
                    strURI = fx.readSendUser(args[2]);      //��ȡ�û�SPI��
                    strMsg = fx.readSMSContent(args[3]);    //��ȡ�û�����Ϣ

                    Console.WriteLine("====================== [��Ϣ] ��ʼ ==================== \r\n");                    
                    Console.WriteLine("�ֻ���:" + strMobileNo + "\r\n");
                    Console.WriteLine("�û�SIP:" + strURI + "\r\n");
                    Console.WriteLine("�û���Ϣ:" + strMsg + "\r\n");
                    Fetion.SaveMessage(strURI, strMsg);
                    Console.WriteLine("====================== [��Ϣ] �˳� ==================== \r\n");
                    break;
                case "evTimer":           //10���Ӽ���һ��
                    strMobileNo = args[1].ToString();       //�ֻ���
                    strTimer = args[2].ToString();          //�������������

                    Console.WriteLine("==================== [��ʱ��] ��ʼ ==================== \r\n");                    
                    Console.WriteLine("�ֻ���:" + strMobileNo + " ����ʱ��:" + strTimer + " ��\r\n");                    
                    Console.WriteLine("==================== [��ʱ��] �˳� ==================== \r\n");
                    Fetion.SendMessage();
                    Fetion.RunTask();
                    break;
                case "evSysMessage":      //ϵͳ��Ϣ
                    strMobileNo = args[1].ToString();       //�ֻ���
                    strTimer = fx.sysMessage(args[2]);      //ϵͳ��Ϣ����

                    Console.WriteLine("=================== [ϵͳ��Ϣ] ��ʼ =================== \r\n");                    
                    Console.WriteLine("�ֻ���:" + strMobileNo + "\r\n");
                    Console.WriteLine("ϵͳ��Ϣ:" + strTimer + "\r\n");
                    Console.WriteLine("=================== [ϵͳ��Ϣ] �˳� =================== \r\n");
                    break;
                case "evServerError":     //������Ԥ������
                    strServerName = args[1].ToString();     //����������
                    strServerStatus = args[2].ToString();   //������״̬
                    SqlLibrary.Fx_AddServerRec(strServerName, strServerStatus);

                    //���շ�����Ԥ����Ϣ�ķ��ź���
                    Module.AddServerEW("660271316", strServerName, strServerStatus);//��־ΰ�ƶ�
                    Thread.Sleep(2000);
                    Module.AddServerEW("432525523", strServerName, strServerStatus);//�������ƶ�

                    Console.WriteLine("================== [������Ԥ��] ��ʼ ================== \r\n");
                    Console.WriteLine("����������:" + strServerName + " ������״̬:" + strServerStatus + "\r\n");
                    Console.WriteLine("Ԥ��ʱ��:" + dt + "\r\n");
                    Console.WriteLine("================== [������Ԥ��] �˳� ================== \r\n");
                    break;
                default:
                    Console.WriteLine("NotEventCase");
                    break;
            }
        }
    }
}