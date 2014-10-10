using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using databaseitem;
using System.Collections;

namespace QQRobot_Module
{
    public class ServerCheck
    {
        #region 游戏赛季更新检测 GetGameTurn(string strProject)
        /// <summary>
        /// 游戏赛季更新检测
        /// </summary>
        /// <param name="strProject">项目名称(BB,FB)</param>
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
                arrServer = new string[] { "XBA", "CGA", "17173", "SOHU", "TW", "DW", "SINA", "CGC", "KX", "5S", "PPS", "YL", "HP", "RR", "NXBA", "UUSEE" };//, "NBA"
            }
            else
            {
                arrServer = new string[] { "XBAF", "XBAF2", "XBAF3", "DWF", "17173F", "SINAF", "SINA2F", "TT", "ESL", "MOP", "PPSF", "TWF", "TY", "PPL", "SOHUF", "PPS2F", "7K7K", "YXY", "TOM", "QIDIAN", "37WAN" };//, "NBAF"
            }

            DateTime dt = DateTime.Now;
            StringBuilder strResult = new StringBuilder("");
            ArrayList arrResult = new ArrayList();
            DataRow dr;

            for (j = 0; j < arrServer.Length; j++)
            {
                //得到服务器Category
                strCategory = DBConnection.Get30GamePhoneConnString(9999, arrServer[j].ToString());
                //得到服务器名称
                strServerName = DBConnection.Get30GamePhoneConnString(8888, arrServer[j].ToString());
                //将Category值存入数组
                arrCategory = strCategory.Split(',');
                //将ServerName值存入数组
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
                            arrResult.Add(string.Format("{0}_{1}_BB 连接超时", arrServer[j].ToString(), arrServerName[i].ToString()));
                        }
                        else
                        {
                            arrResult.Add(string.Format("{0}_{1}_FB 连接超时", arrServer[j].ToString(), arrServerName[i].ToString()));
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
                return strResult.Append("篮球今晚没有赛季更新\n" + string.Format("{0:G}", dt)).ToString();
            }
            else
            {
                return strResult.Append("足球今晚没有赛季更新\n" + string.Format("{0:G}", dt)).ToString();
            }
        }
        #endregion 游戏赛季更新检测 GetGameTurn(string strProject)

        #region 游戏夜间更新检测 GetGameStatus(string strProject)
        /// <summary>
        /// 游戏夜间更新检测
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
                arrServer = new string[] { "XBA", "CGA", "17173", "SOHU", "TW", "DW", "SINA", "CGC", "KX", "5S", "PPS", "YL", "HP", "RR", "NXBA", "UUSEE" };//, "NBA"
            }
            else
            {
                arrServer = new string[] { "XBAF", "XBAF2", "XBAF3", "DWF", "17173F", "SINAF", "SINA2F", "TT", "ESL", "MOP", "PPSF", "TWF", "TY", "PPL", "SOHUF", "PPS2F", "7K7K", "YXY", "TOM", "QIDIAN", "37WAN" };//, "NBAF"
            }

            DateTime dt = DateTime.Now;
            StringBuilder strResult = new StringBuilder("");
            ArrayList arrResult = new ArrayList();
            DataRow dr;

            for (j = 0; j < arrServer.Length; j++)
            {
                //得到服务器Category
                strCategory = DBConnection.Get30GamePhoneConnString(9999, arrServer[j].ToString());
                //得到服务器名称
                strServerName = DBConnection.Get30GamePhoneConnString(8888, arrServer[j].ToString());
                //将Category值存入数组
                arrCategory = strCategory.Split(',');
                //将ServerName值存入数组
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
                                arrResult.Add(string.Format("{0}_{1}_BB_夜间更新出错！", arrServer[j].ToString(), arrServerName[i].ToString()));
                            }
                        }
                        else
                        {
                            byteStatus = (byte)dr["Status"];
                            boolFinish = (bool)dr["IsFinish"];
                            if (boolFinish == false)
                            {
                                arrResult.Add(string.Format("{0}_{1}_FB_联赛出错！", arrServer[j].ToString(), arrServerName[i].ToString()));
                            }
                            else if (byteStatus == 1)
                            {
                                arrResult.Add(string.Format("{0}_{1}_FB_夜间更新出错！", arrServer[j].ToString(), arrServerName[i].ToString()));
                            }
                            else if (byteStatus == 2)
                            {
                                arrResult.Add(string.Format("{0}_{1}_FB_赛季更新出错！", arrServer[j].ToString(), arrServerName[i].ToString()));
                            }
                        }
                    }
                    catch
                    {
                        if (strProject == "BB")
                        {
                            arrResult.Add(string.Format("{0}_{1}_BB 连接超时", arrServer[j].ToString(), arrServerName[i].ToString()));
                        }
                        else
                        {
                            arrResult.Add(string.Format("{0}_{1}_FB 连接超时", arrServer[j].ToString(), arrServerName[i].ToString()));
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
                return strResult.Append("篮球夜间更新正常\n" + string.Format("{0:G}", dt)).ToString();
            }
            else
            {
                return strResult.Append("足球夜间更新正常\n" + string.Format("{0:G}", dt)).ToString();
            }
        }
        #endregion 游戏夜间更新检测 GetGameStatus(string strProject)

        #region 查询赛季更新详情 GetGameSeason(string strKeywords)
        /// <summary>
        /// 查询赛季更新详情
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

            arrProject_BB = new string[] { "XBA", "CGA", "17173", "SOHU", "TW", "DW", "SINA", "CGC", "KX", "5S", "PPS", "YL", "HP", "RR", "NXBA", "UUSEE" };//, "NBA"
            arrProject_FB = new string[] { "XBAF", "XBAF2", "XBAF3", "DWF", "17173F", "SINAF", "SINA2F", "TT", "ESL", "MOP", "PPSF", "TWF", "TY", "PPL", "SOHUF", "PPS2F", "7K7K", "YXY", "TOM", "QIDIAN", "37WAN" };//, "NBAF"

            for (i = 0; i < arrProject_BB.Length; i++)
            {
                if (arrProject_BB[i] == strKeywords)
                {
                    strProject = "BB";
                    Console.WriteLine(strProject);
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
                        Console.WriteLine(strProject);
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

            //得到服务器Category
            strCategory = DBConnection.Get30GamePhoneConnString(9999, strKeywords);
            //得到服务器名称
            strServerName = DBConnection.Get30GamePhoneConnString(8888, strKeywords);
            //将Category值存入数组
            arrCategory = strCategory.Split(',');
            //将ServerName值存入数组
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
                            arrResult.Add(string.Format("{0}_{1}_{2}  赛季更新还有{3}天，将在{4}进行", strKeywords, arrServerName[i].ToString(), strProject, 28 - intDays, dt.AddDays(28 - intDays).ToShortDateString()));
                        }
                        else
                        {
                            arrResult.Add(string.Format("{0}_{1}_{2}  今晚赛季更新", strKeywords, arrServerName[i].ToString(), strProject));
                        }
                    }
                    else
                    {
                        byteDays = (byte)dr["Days"];
                        if (24 - byteDays != 0)
                        {
                            arrResult.Add(string.Format("{0}_{1}_{2}  赛季更新还有{3}天，将在{4}进行", strKeywords, arrServerName[i].ToString(), strProject, 24 - byteDays, dt.AddDays(24 - byteDays).ToShortDateString()));
                        }
                        else
                        {
                            arrResult.Add(string.Format("{0}_{1}_{2}  今晚赛季更新", strKeywords, arrServerName[i].ToString(), strProject));
                        }
                    }
                }
                catch
                {
                    arrResult.Add(string.Format("{0}_{1}_{2} 连接超时", strKeywords, arrServerName[i].ToString(), strProject));
                }
            }

            foreach (string s in arrResult)
            {
                strResult.Append(s.ToString() + "\n");
            }

            strResult.Append(string.Format("{0:G}", dt));
            return strResult.ToString();
        }
        #endregion 查询赛季更新详情 GetGameSeason(string strKeywords)

        #region 查询夜间更新是否执行 GetGameCheck(string strProject)
        /// <summary>
        /// 查询夜间更新是否执行
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
                arrServer = new string[] { "XBA", "CGA", "17173", "SOHU", "TW", "DW", "SINA", "CGC", "KX", "5S", "PPS", "YL", "HP", "RR", "NXBA", "UUSEE" };//, "NBA"
            }
            else
            {
                arrServer = new string[] { "XBAF", "XBAF2", "XBAF3", "DWF", "17173F", "SINAF", "SINA2F", "TT", "ESL", "MOP", "PPSF", "TWF", "TY", "PPL", "SOHUF", "PPS2F", "7K7K", "YXY", "TOM", "QIDIAN", "37WAN" };//, "NBAF"
            }

            DateTime dt = DateTime.Now;
            StringBuilder strResult = new StringBuilder("");
            ArrayList arrResult = new ArrayList();
            DataRow dr;

            for (j = 0; j < arrServer.Length; j++)
            {
                //得到服务器Category
                strCategory = DBConnection.Get30GamePhoneConnString(9999, arrServer[j].ToString());
                //得到服务器名称
                strServerName = DBConnection.Get30GamePhoneConnString(8888, arrServer[j].ToString());
                //将Category值存入数组
                arrCategory = strCategory.Split(',');
                //将ServerName值存入数组
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
                                arrResult.Add(string.Format("{0}_{1}_BB_夜间没有开启！", arrServer[j].ToString(), arrServerName[i].ToString()));
                            }
                        }
                        else
                        {
                            dtimeFinishTime = (DateTime)dr["FinishTime"];
                            byteDays = (byte)dr["Days"];

                            if (dtimeFinishTime < dt.Date && byteDays != 1)
                            {
                                arrResult.Add(string.Format("{0}_{1}_FB_夜间没有开启！", arrServer[j].ToString(), arrServerName[i].ToString()));
                            }
                        }
                    }
                    catch
                    {
                        if (strProject == "BB")
                        {
                            arrResult.Add(string.Format("{0}_{1}_BB 连接超时", arrServer[j].ToString(), arrServerName[i].ToString()));
                        }
                        else
                        {
                            arrResult.Add(string.Format("{0}_{1}_FB 连接超时", arrServer[j].ToString(), arrServerName[i].ToString()));
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
                return strResult.Append("篮球夜间全部执行\n" + string.Format("{0:G}", dt)).ToString();
            }
            else
            {
                return strResult.Append("足球夜间全部执行\n" + string.Format("{0:G}", dt)).ToString();
            }
        }
        #endregion 查询夜间更新是否执行 GetGameCheck(string strProject)
    }
}
