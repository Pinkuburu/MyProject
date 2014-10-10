using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using databaseitem;
using System.Collections;
using System.Diagnostics;

namespace QQRobot_Module
{
    public class ServerCheck
    {
        #region ��Ϸ�������¼�� GetGameTurn(string strProject)
        /// <summary>
        /// ��Ϸ�������¼��
        /// </summary>
        /// <param name="strProject">��Ŀ����(BB,FB)</param>
        /// <returns></returns>
        public string GetGameTurn(string strProject)
        {
            int intTurn = 0;
            int i = 0;
            int j = 0;
            string strCategory = null;
            string strServerName = null;
            string[] arrCategory = { };
            string[] arrServerName = { };
            string[] arrServer = { };

            strProject = strProject.ToUpper();

            if (strProject == "BB")
            {
                arrServer = new string[] { "XBA", "CGA", "17173", "SOHU", "51WAN", "TW", "DW", "NBA", "SINA", "CGC", "KX", "5S", "PPS", "YL", "HP", "RR", "NXBA" };
            }
            else
            {
                arrServer = new string[] { "XBAF", "XBAF2", "XBAF3", "DWF", "17173F", "NBAF", "SINAF", "SINA2F", "TT", "ESL", "MOP", "PPSF", "TWF", "TY", "PPL", "SOHUF", "PPS2F", "7K7K", "YXY" };
            }

            DateTime dt = DateTime.Now;
            StringBuilder strResult = new StringBuilder("");
            ArrayList arrResult = new ArrayList();
            DataRow dr;

            for (j = 0; j < arrServer.Length; j++)
            {
                //�õ�������Category
                strCategory = DBConnection.Get30GamePhoneConnString(9999, arrServer[j].ToString());
                //�õ�����������
                strServerName = DBConnection.Get30GamePhoneConnString(8888, arrServer[j].ToString());
                //��Categoryֵ��������
                arrCategory = strCategory.Split(',');
                //��ServerNameֵ��������
                arrServerName = strServerName.Split(',');

                for (i = 0; i < arrCategory.Length; i++)
                {
                    try
                    {
                        dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(Convert.ToInt32(arrCategory[i].ToString()), arrServer[j].ToString()), CommandType.StoredProcedure, "GetGameRow");
                        if (strProject == "BB")
                        {
                            intTurn = (int)dr["Turn"];
                            if (intTurn == 27)
                            {
                                arrResult.Add(string.Format("{0}_{1}_BB", arrServer[j].ToString(), arrServerName[i].ToString()));
                            }
                        }
                        else
                        {
                            intTurn = (byte)dr["Turn"];
                            if (intTurn == 23)
                            {
                                arrResult.Add(string.Format("{0}_{1}_FB", arrServer[j].ToString(), arrServerName[i].ToString()));
                            }
                        }
                    }
                    catch
                    {
                        if (strProject == "BB")
                        {
                            arrResult.Add(string.Format("{0}_{1}_BB ���ӳ�ʱ", arrServer[j].ToString(), arrServerName[i].ToString()));
                        }
                        else
                        {
                            arrResult.Add(string.Format("{0}_{1}_FB ���ӳ�ʱ", arrServer[j].ToString(), arrServerName[i].ToString()));
                        }
                    }
                }
            }

            foreach (string s in arrResult)
            {
                strResult.Append(s.ToString() + "\n");
            }

            if (strResult.Length > 0)
            {
                strResult.Append(string.Format("{0:G}", dt));
                return strResult.ToString();
            }

            if (strProject == "BB")
            {
                return strResult.Append("�������û����������\n" + string.Format("{0:G}", dt)).ToString();
            }
            else
            {
                return strResult.Append("�������û����������\n" + string.Format("{0:G}", dt)).ToString();
            }
        }
        #endregion ��Ϸ�������¼�� GetGameTurn(string strProject)

        #region ��Ϸҹ����¼�� GetGameStatus(string strProject)
        /// <summary>
        /// ��Ϸҹ����¼��
        /// </summary>
        /// <param name="strProject"></param>
        /// <returns></returns>
        public string GetGameStatus(string strProject)
        {
            byte byteStatus = 0;
            bool boolFinish = false;
            int i = 0;
            int j = 0;
            string strCategory = null;
            string strServerName = null;
            string[] arrCategory = { };
            string[] arrServerName = { };
            string[] arrServer = { };

            strProject = strProject.ToUpper();

            if (strProject == "BB")
            {
                arrServer = new string[] { "XBA", "CGA", "17173", "SOHU", "51WAN", "TW", "DW", "NBA", "SINA", "CGC", "KX", "5S", "PPS", "YL", "HP", "RR", "NXBA" };
            }
            else
            {
                arrServer = new string[] { "XBAF", "XBAF2", "XBAF3", "DWF", "17173F", "NBAF", "SINAF", "SINA2F", "TT", "ESL", "MOP", "PPSF", "TWF", "TY", "PPL", "SOHUF", "PPS2F", "7K7K", "YXY" };
            }

            DateTime dt = DateTime.Now;
            StringBuilder strResult = new StringBuilder("");
            ArrayList arrResult = new ArrayList();
            DataRow dr;

            for (j = 0; j < arrServer.Length; j++)
            {
                //�õ�������Category
                strCategory = DBConnection.Get30GamePhoneConnString(9999, arrServer[j].ToString());
                //�õ�����������
                strServerName = DBConnection.Get30GamePhoneConnString(8888, arrServer[j].ToString());
                //��Categoryֵ��������
                arrCategory = strCategory.Split(',');
                //��ServerNameֵ��������
                arrServerName = strServerName.Split(',');

                for (i = 0; i < arrCategory.Length; i++)
                {
                    try
                    {
                        dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(Convert.ToInt32(arrCategory[i].ToString()), arrServer[j].ToString()), CommandType.StoredProcedure, "GetGameRow");
                        if (strProject == "BB")
                        {
                            byteStatus = (byte)dr["Status"];
                            if (byteStatus == 1)
                            {
                                arrResult.Add(string.Format("{0}_{1}_BB_ҹ����³���", arrServer[j].ToString(), arrServerName[i].ToString()));
                            }
                        }
                        else
                        {
                            byteStatus = (byte)dr["Status"];
                            boolFinish = (bool)dr["IsFinish"];
                            if (boolFinish == false)
                            {
                                arrResult.Add(string.Format("{0}_{1}_FB_��������", arrServer[j].ToString(), arrServerName[i].ToString()));
                            }
                            else if (byteStatus == 1)
                            {
                                arrResult.Add(string.Format("{0}_{1}_FB_ҹ����³���", arrServer[j].ToString(), arrServerName[i].ToString()));
                            }
                            else if (byteStatus == 2)
                            {
                                arrResult.Add(string.Format("{0}_{1}_FB_�������³���", arrServer[j].ToString(), arrServerName[i].ToString()));
                            }
                        }
                    }
                    catch
                    {
                        if (strProject == "BB")
                        {
                            arrResult.Add(string.Format("{0}_{1}_BB ���ӳ�ʱ", arrServer[j].ToString(), arrServerName[i].ToString()));
                        }
                        else
                        {
                            arrResult.Add(string.Format("{0}_{1}_FB ���ӳ�ʱ", arrServer[j].ToString(), arrServerName[i].ToString()));
                        }
                    }
                }
            }

            foreach (string s in arrResult)
            {
                strResult.Append(s.ToString() + "\n");
            }

            if (strResult.Length > 0)
            {
                strResult.Append(string.Format("{0:G}", dt));
                return strResult.ToString();
            }

            if (strProject == "BB")
            {
                return strResult.Append("����ҹ���������\n" + string.Format("{0:G}", dt)).ToString();
            }
            else
            {
                return strResult.Append("����ҹ���������\n" + string.Format("{0:G}", dt)).ToString();
            }
        }
        #endregion ��Ϸҹ����¼�� GetGameStatus(string strProject)

        try
        {
            string strExePath = ����EXE���ֵ�����·����
            ProcessStartInfo procInfo = new ProcessStartInfo(strExePath, ����EXE �Ĳ���);
            // ����EXE���еĴ���
            procInfo.WindowStyle = ProcessWindowStyle.Hidden;
            // exe����
            Process procBatch = Process.Start(procInfo);
            // ȡ��EXE���к�ķ���ֵ������ֵֻ��������

            int iRtn = procBatch.ExitCode;            
        }
        catch(Exception ex)
        {
        }
        public void 

    }
}
