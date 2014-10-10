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

        try
        {
            string strExePath = 带有EXE名字的完整路径；
            ProcessStartInfo procInfo = new ProcessStartInfo(strExePath, 传给EXE 的参数);
            // 隐藏EXE运行的窗口
            procInfo.WindowStyle = ProcessWindowStyle.Hidden;
            // exe运行
            Process procBatch = Process.Start(procInfo);
            // 取得EXE运行后的返回值，返回值只能是整型

            int iRtn = procBatch.ExitCode;            
        }
        catch(Exception ex)
        {
        }
        public void 

    }
}
