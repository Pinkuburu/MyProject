using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

//=====================
//    QQ�����˲��     
//    ����:Cupid       
// ��������:2010-11-19 
//=====================

namespace QQRobot_Module
{
    public class ServerMonitor
    {
        #region �ʼ����� SendMail(string strMailTo, string strSubject, string strMailContent)
        /// <summary>
        /// �ʼ�����
        /// </summary>
        /// <param name="strMailTo">��������</param>
        /// <param name="strSubject">�ʼ�����</param>
        /// <param name="strMailContent">�ʼ�����</param>
        /// <returns></returns>
        public string SendMail(string strMailTo, string strSubject, string strMailContent)
        {
            string strResult = null;
            try
            {
                string strExePath = @"C:\HQmail.exe";
                //strExePath = Path.GetFullPath(strExePath);
                ProcessStartInfo procInfo = new ProcessStartInfo(strExePath, string.Format("cupid0616@163.com 677521++ {0} {1} {2}", strMailTo, strSubject, strMailContent));
                // ����EXE���еĴ���
                procInfo.WindowStyle = ProcessWindowStyle.Hidden;
                // exe����
                Process procBatch = Process.Start(procInfo);
                // ȡ��EXE���к�ķ���ֵ������ֵֻ��������

                strResult = "�ʼ����ͳɹ�~~";
            }
            catch
            {
                strResult = "�ʼ�����ʧ��~~";
            }
            return strResult;
        }
        #endregion �ʼ����� SendMail(string strMailTo, string strSubject, string strMailContent)

        #region ���ŷ��� SendSMS(string strToMobile, string strMsg)
        /// <summary>
        /// ���ŷ���
        /// </summary>
        /// <param name="strToMobile">�ն����ֻ���</param>
        /// <param name="strMsg">��������</param>
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
                // ����EXE���еĴ���
                procInfo.WindowStyle = ProcessWindowStyle.Hidden;
                // exe����
                Process procBatch = Process.Start(procInfo);
                // ȡ��EXE���к�ķ���ֵ������ֵֻ��������
                strResult = "���ŷ��ͳɹ�~~";
            }
            catch
            {
                strResult = "���ŷ���ʧ��~~";
            }
            return strResult;
        }
        #endregion ���ŷ��� SendSMS(string strToMobile, string strMsg)

        #region ����QQ��Ϣ SendQQMsg(string strQQ, string strMsg)
        /// <summary>
        /// ����QQ��Ϣ
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
        #endregion ����QQ��Ϣ SendQQMsg(string strQQ, string strMsg)
    }
}