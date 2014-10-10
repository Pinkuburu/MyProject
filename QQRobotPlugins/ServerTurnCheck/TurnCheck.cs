using System;
using System.Text;
using System.Data;
using databaseitem;
using System.Collections;

namespace ServerTurnCheck
{
    //XBA,CGA,17173,TW,SINA,KX,YL,NXBA
    //XBAF,XBAF3,17173F,SINAF,TT,ESL,MOP,TWF,TY,PPL,YXY,92PK,TOM,QIDIAN,37WAN,9YOU,JUU,QIDIAN2,5A,RRF,HL,ZMR,JJ,CGAF,51WANF,37WAN2,YW,MYY,QIDIAN3,QIDIAN4,LZ,56,ESL,360,KX001,CNTV,QIDIAN5,TWF3,PPSF3,9377
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
                arrServer = ServerTurnCheck.sData.bb_Data.Split(',');
            }
            else
            {
                arrServer = ServerTurnCheck.sData.fb_Data.Split(',');
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
                arrServer = ServerTurnCheck.sData.bb_Data.Split(',');
            }
            else
            {
                arrServer = ServerTurnCheck.sData.fb_Data.Split(',');
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

        #region ��ѯ������������ GetGameSeason(string strKeywords)
        /// <summary>
        /// ��ѯ������������
        /// </summary>
        /// <param name="strKeywords"></param>
        /// <returns></returns>
        public string GetGameSeason(string strKeywords)
        {
            int intDays = 0;
            byte byteDays = 0;
            int i = 0;
            string strCategory = null;
            string strServerName = null;
            string strProject = null;
            string[] arrCategory = { };
            string[] arrServerName = { };
            string[] arrProject_BB = { };
            string[] arrProject_FB = { };

            strKeywords = strKeywords.ToUpper();

            arrProject_BB = ServerTurnCheck.sData.bb_Data.Split(',');
            arrProject_FB = ServerTurnCheck.sData.fb_Data.Split(',');

            for (i = 0; i < arrProject_BB.Length; i++)
            {
                if (arrProject_BB[i] == strKeywords)
                {
                    strProject = "BB";
                    //Console.WriteLine(strProject);
                    break;
                }
            }

            for (i = 0; i < arrProject_FB.Length; i++)
            {
                if (strProject == null)
                {
                    if (arrProject_FB[i] == strKeywords)
                    {
                        strProject = "FB";
                        //Console.WriteLine(strProject);
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            DateTime dt = DateTime.Now;
            StringBuilder strResult = new StringBuilder("");
            ArrayList arrResult = new ArrayList();
            DataRow dr;

            //�õ�������Category
            strCategory = DBConnection.Get30GamePhoneConnString(9999, strKeywords);
            //�õ�����������
            strServerName = DBConnection.Get30GamePhoneConnString(8888, strKeywords);
            //��Categoryֵ��������
            arrCategory = strCategory.Split(',');
            //��ServerNameֵ��������
            arrServerName = strServerName.Split(',');

            for (i = 0; i < arrCategory.Length; i++)
            {
                try
                {
                    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(Convert.ToInt32(arrCategory[i].ToString()), strKeywords), CommandType.StoredProcedure, "GetGameRow");
                    if (strProject == "BB")
                    {
                        intDays = (int)dr["Days"];
                        if (28 - intDays != 0)
                        {
                            arrResult.Add(string.Format("{0}_{1}_{2}  �������»���{3}�죬����{4}����", strKeywords, arrServerName[i].ToString(), strProject, 28 - intDays, dt.AddDays(28 - intDays).ToShortDateString()));
                        }
                        else
                        {
                            arrResult.Add(string.Format("{0}_{1}_{2}  ������������", strKeywords, arrServerName[i].ToString(), strProject));
                        }
                    }
                    else
                    {
                        byteDays = (byte)dr["Days"];
                        if (24 - byteDays != 0)
                        {
                            arrResult.Add(string.Format("{0}_{1}_{2}  �������»���{3}�죬����{4}����", strKeywords, arrServerName[i].ToString(), strProject, 24 - byteDays, dt.AddDays(24 - byteDays).ToShortDateString()));
                        }
                        else
                        {
                            arrResult.Add(string.Format("{0}_{1}_{2}  ������������", strKeywords, arrServerName[i].ToString(), strProject));
                        }
                    }
                }
                catch
                {
                    arrResult.Add(string.Format("{0}_{1}_{2} ���ӳ�ʱ", strKeywords, arrServerName[i].ToString(), strProject));
                }
            }

            foreach (string s in arrResult)
            {
                strResult.Append(s.ToString() + "\n");
            }

            strResult.Append(string.Format("{0:G}", dt));
            return strResult.ToString();
        }
        #endregion ��ѯ������������ GetGameSeason(string strKeywords)

        #region ��ѯҹ������Ƿ�ִ�� GetGameCheck(string strProject)
        /// <summary>
        /// ��ѯҹ������Ƿ�ִ��
        /// </summary>
        /// <param name="strProject"></param>
        /// <returns></returns>
        public string GetGameCheck(string strProject)
        {
            DateTime dtimeFinishTime;
            int intDays = 0;
            byte byteDays = 0;
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
                arrServer = ServerTurnCheck.sData.bb_Data.Split(',');
            }
            else
            {
                arrServer = ServerTurnCheck.sData.fb_Data.Split(',');
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
                            dtimeFinishTime = (DateTime)dr["FinishTime"];
                            intDays = (int)dr["Days"];

                            if (dtimeFinishTime < dt.Date && intDays != 1)
                            {
                                arrResult.Add(string.Format("{0}_{1}_BB_ҹ��û�п�����", arrServer[j].ToString(), arrServerName[i].ToString()));
                            }
                        }
                        else
                        {
                            dtimeFinishTime = (DateTime)dr["FinishTime"];
                            byteDays = (byte)dr["Days"];

                            if (dtimeFinishTime < dt.Date && byteDays != 1)
                            {
                                arrResult.Add(string.Format("{0}_{1}_FB_ҹ��û�п�����", arrServer[j].ToString(), arrServerName[i].ToString()));
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
                return strResult.Append("����ҹ��ȫ��ִ��\n" + string.Format("{0:G}", dt)).ToString();
            }
            else
            {
                return strResult.Append("����ҹ��ȫ��ִ��\n" + string.Format("{0:G}", dt)).ToString();
            }
        }
        #endregion ��ѯҹ������Ƿ�ִ�� GetGameCheck(string strProject)
    }
}
