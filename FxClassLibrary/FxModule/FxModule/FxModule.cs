using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Data;
using System.Collections;
using databaseitem;
using System.Xml;

namespace FxModule
{
    public class Module
    {
        #region �����ض��ļ���ʽ��TXT�ĵ�
        /// <summary>
        /// �����ض��ļ���ʽ��TXT�ĵ�
        /// </summary>
        /// <param name="strSID">�û�SID</param>
        /// <param name="strSMSContent">��Ϣ����</param>
        /// <param name="type">1\SMS 2\PC</param>
        public static void CreateTxt(string strSID, string strSMSContent, int type)//�����ض��ļ���ʽ��TXT�ĵ�
        {
            Random rnbNum = new Random();
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory; 
            string strNum = Convert.ToString(rnbNum.Next(000000, 999999));
            string filename = "13691102424_" + strNum + "";
            FileStream fs = new FileStream(@"D:\fetion2009\commands\" + filename + ".cmd", FileMode.OpenOrCreate, FileAccess.Write);
            //FileStream fs = new FileStream(@"C:\fetion2009\commands\" + filename + ".cmd", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));//ͨ��ָ���ַ����뷽ʽ����ʵ�ֶԺ��ֵ�֧�֣��������ü��±��򿪲鿴���������

            switch (type)
            {
                case 1:              
                    sw.Flush();
                    sw.BaseStream.Seek(0, SeekOrigin.Begin);
                    sw.WriteLine("sms " + strSID + " " + strSMSContent);
                    sw.Flush();
                    sw.Close();
                    break;
                case 2:
                    sw.Flush();
                    sw.BaseStream.Seek(0, SeekOrigin.Begin);
                    sw.WriteLine("chat " + strSID + " " + strSMSContent);
                    sw.Flush();
                    sw.Close();
                    break;
                case 3:
                    sw.Flush();
                    sw.BaseStream.Seek(0, SeekOrigin.Begin);
                    sw.WriteLine("longsms " + strSID + " " + strSMSContent);
                    sw.Flush();
                    sw.Close();
                    break;
            }            
        }
        #endregion

        #region ��ȡĳ����������Ϣ���OLD ͣ��
        /// <summary>
        /// ��ȡĳ����������Ϣ���
        /// </summary>
        /// <param name="strCity">����ȫƴ</param>
        /// <returns></returns>
        //public static string Weather(string strCity)//��ȡĳ����������Ϣ
        //{
        //    string strMobileData = strCity;
        //    string strWeather = "";
        //    string strUrl = "http://www.google.cn/search?as_q=tq+'" + strMobileData + "'&num=1";
        //    HttpWebRequest oRequest = (HttpWebRequest)WebRequest.Create(strUrl);
        //    HttpWebResponse oResponse = (HttpWebResponse)oRequest.GetResponse();
        //    StreamReader sr = new StreamReader(oResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312")); //�������ʾ���������룬��utf-8��
        //    string sResultContents = sr.ReadToEnd();//��ҳ��HTML���ڱ���sResultContents ��
        //    oResponse.Close();

        //    try
        //    {
        //        int start = sResultContents.IndexOf("<table class=\"ts std\">");
        //        int end = sResultContents.IndexOf("�������", start + 1);
        //        sResultContents = sResultContents.Substring(start, end - start);


        //        sResultContents = sResultContents.Replace("<table class=\"ts std\"><tr><td><div style=\"float:left\" class=med><FONT color=#cc0033>", "");
        //        sResultContents = sResultContents.Replace("</FONT></div><div style=\"float:left\">&nbsp;-&nbsp;", "\\n�¶ȣ�");

        //        int start_sub = sResultContents.IndexOf("<a href=\"http://www.google");
        //        int end_sub = sResultContents.IndexOf("iGoogle</a>");
        //        sResultContents = sResultContents.Remove(start_sub, end_sub - start_sub);


        //        sResultContents = sResultContents.Replace("iGoogle</a></div><tr><td><div style=\"padding:5px;float:left\"><div style=\"font-size:140%\"><b>", "");
        //        sResultContents = sResultContents.Replace("</b></div><div>", "\\n");

        //        sResultContents = sResultContents.Replace("<b>", "");
        //        sResultContents = sResultContents.Replace("</b>", "");

        //        start_sub = sResultContents.IndexOf("</div></div><div align=center style=\"padding:5px;float:left\">");
        //        end_sub = sResultContents.IndexOf("����");
        //        sResultContents = sResultContents.Remove(start_sub, end_sub - start_sub);
        //        sResultContents = sResultContents.Insert(sResultContents.IndexOf("����"), "\\n");

        //        Console.WriteLine(sResultContents);
        //        start_sub = sResultContents.IndexOf("<br><img style=\"border:1px solid");
        //        end_sub = sResultContents.IndexOf("\" title=\"");
        //        sResultContents = sResultContents.Remove(start_sub, end_sub - start_sub + 9);


        //        start_sub = sResultContents.IndexOf("\" width=40 height=40");
        //        sResultContents = sResultContents.Remove(start_sub);

        //        sResultContents = sResultContents.Replace("<br>", "��");
        //        sResultContents = sResultContents.Replace("��ʪ�ȣ�", "\\nʪ�ȣ�");
        //    }
        //    catch
        //    {
        //        sResultContents = "���޴˵�������������";
        //    }
        //    strWeather = sResultContents;
        //    return strWeather;
        //}
        #endregion

        #region ��ȡĳ����������Ϣ���NEW ͣ��
        /// <summary>
        /// ��ȡĳ����������Ϣ���
        /// </summary>
        /// <param name="strCity">������������</param>
        /// <returns></returns>
        //public static string Weather(string strCity)//��ȡĳ����������Ϣ
        //{
        //    string sResultContents;
        //    string strNodeValue = "";
            
        //    sResultContents = PostModel("http://webservice.jtjc.cn/service/weather.asmx/GetWeather", "City=" + strCity + "");
        //    if (sResultContents != "false")
        //    {
        //        XmlDocument xmlDoc = new XmlDocument();
        //        xmlDoc.LoadXml(sResultContents);
        //        XmlNodeList xn0 = xmlDoc.SelectSingleNode("Weather").ChildNodes;


        //        foreach (XmlNode node in xn0)
        //        {
        //            if (node.Name == "City")
        //            {
        //                strNodeValue = node.InnerText.Trim();
        //            }
        //            if (node.Name == "Sj0")
        //            {
        //                strNodeValue += node.InnerText.Trim() + "\\n";
        //                //strNodeValue = strNodeValue.Insert(strNodeValue.IndexOf("("), "\\n");
        //            }
        //            if (node.Name == "Date")
        //            {
        //                foreach (XmlNode node2 in node.ChildNodes)
        //                {
        //                    switch (node2.Name)
        //                    {
        //                        case "Tq":
        //                            strNodeValue += "������" + node2.InnerText.Trim() + "\\n";
        //                            break;
        //                        case "Qw":
        //                            strNodeValue += "���£�" + node2.InnerText.Trim() + "\\n";
        //                            break;
        //                        case "Fx":
        //                            strNodeValue += "������" + node2.InnerText.Trim();
        //                            break;
        //                    }
        //                }
        //                break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        strNodeValue = "������ĳ��������������������ĳ������ƣ�";
        //    }
        //    return strNodeValue;
        //}
        #endregion

        #region ��ȡĳ����������Ϣ���  Weather(string strCity)
        /// <summary>
        /// ��ȡĳ����������Ϣ���
        /// </summary>
        /// <param name="strCity"></param>
        /// <returns></returns>
        public static string Weather(string strCity)
        {
            string strContents = "";

            FxModule.WebClient HTTPproc = new FxModule.WebClient();
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            string sResultContents = HTTPproc.OpenRead("http://webservice.webxml.com.cn/WebServices/WeatherWebService.asmx/getWeatherbyCityName", "theCityName=�ൺ");
            Console.WriteLine("�����ǣ�" + strCity);//////////////////////////
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(sResultContents);
            if (xmlDoc.ChildNodes.Count > 0)
            {
                strContents = xmlDoc.ChildNodes[1].ChildNodes[6].InnerText.ToString() + "��";
                strContents += xmlDoc.ChildNodes[1].ChildNodes[7].InnerText.ToString() + "��";
                strContents += xmlDoc.ChildNodes[1].ChildNodes[10].InnerText.ToString();
                //int intIndex = strContents.IndexOf("����ѹ��");
                //strContents = strContents.Substring(0, intIndex);
                return strContents;
            }
            else
            {
                strContents = "�����ֳ�����!";
                return strContents;
            }
        }
        #endregion

        #region ��ȡ��������Ϸ�ִ�  GetBasketBallTurn()
        /// <summary>
        /// ��ȡ��������Ϸ�ִ�
        /// </summary>
        /// <returns></returns>
        public static string GetBasketBallTurn()
        {
            //�ٷ���
            int intTurnNF2, intTurnHJ3, intTurnHR4, intTurnLN7, intTurnKRT8, intTurnLWF11, intTurnFHC12;
            //�Ʒ���
            int intTurnCGA3, intTurnCGA4;
            //17173��
            int intTurn171733, intTurn171736;
            //51WAN��
            int intTurn51WAN4;
            //������
            int intTurnDW1;
            //�´���
            int intTurnNBA1, intTurnNBA2, intTurnNBA5;
            //������
            int intTurnSINA1, intTurnSINA3;
            //������
            int intTurnCGC1;
            //������
            int intTurnKX1;
            //�Ѻ���
            int intTurnSOHU1;
            //5S��
            int intTurn5S1;
            //̨����
            int intTurnTW1, intTurnTW2;
            //PPS��
            int intTurnPPS1, intTurnPPS3;
            //YL��
            int intTurnYL1;
            //SINAB��
            int intTurnSINAB4;
            //RR��
            int intTurnRR1;
            //HP��
            int intTurnHP1;
            //NXBA�¸���������
            int intTurnNXBA1;

            StringBuilder strBuild = new StringBuilder("");

            DateTime dt = DateTime.Now;

            ArrayList arrResult = new ArrayList();
            DataRow dr;



            try
            {
                //�Ϸ���
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnNF2 = (int)dr["Turn"];
                if (intTurnNF2 == 27)
                {
                    arrResult.Add("NF:" + intTurnNF2 + "");
                }
                //arrResult.Add("2");
            }
            catch
            {
                arrResult.Add("�Ϸ������ӳ�ʱ��");
            }

            try
            {
                //�����
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(5, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnHJ3 = (int)dr["Turn"];
                if (intTurnHJ3 == 27)
                {
                    arrResult.Add("HJ:" + intTurnHJ3 + "");
                }
                //arrResult.Add("3");
            }
            catch
            {
                arrResult.Add("��������ӳ�ʱ��");
            }

            try
            {
                //���˷�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(7, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnHR4 = (int)dr["Turn"];
                if (intTurnHR4 == 27)
                {
                    arrResult.Add("HR:" + intTurnHR4 + "");
                }
                //arrResult.Add("4");
            }
            catch
            {
                arrResult.Add("���˷����ӳ�ʱ��");
            }

            try
            {
                //�������˷�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(10, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnKRT8 = (int)dr["Turn"];
                if (intTurnKRT8 == 27)
                {
                    arrResult.Add("KRT:" + intTurnKRT8 + "");
                }
                //arrResult.Add("6");
            }
            catch
            {
                arrResult.Add("�������˷����ӳ�ʱ��");
            }

            try
            {
                //���Ϸ�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(8, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnLN7 = (int)dr["Turn"];
                if (intTurnLN7 == 27)
                {
                    arrResult.Add("LN:" + intTurnLN7 + "");
                }
                //arrResult.Add("7");
            }
            catch
            {
                arrResult.Add("���Ϸ����ӳ�ʱ��");
            }

            try
            {
                //������
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(11, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnLWF11 = (int)dr["Turn"];
                if (intTurnLWF11 == 27)
                {
                    arrResult.Add("LWF:" + intTurnLWF11 + "");
                }
                //arrResult.Add("7");
            }
            catch
            {
                arrResult.Add("���������ӳ�ʱ��");
            }

            try
            {
                //��˳Ƿ�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(12, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnFHC12 = (int)dr["Turn"];
                if (intTurnFHC12 == 27)
                {
                    arrResult.Add("FHC:" + intTurnFHC12 + "");
                }
                //arrResult.Add("7");
            }
            catch
            {
                arrResult.Add("��˳Ƿ����ӳ�ʱ��");
            }

            try
            {
                //�Ʒ�����
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "CGA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnCGA3 = (int)dr["Turn"];
                if (intTurnCGA3 == 27)
                {
                    arrResult.Add("CGA3:" + intTurnCGA3 + "");
                }
                //arrResult.Add("8");
            }
            catch
            {
                arrResult.Add("�Ʒ��������ӳ�ʱ��");
            }

            try
            {
                //�Ʒ��ķ�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "CGA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnCGA4 = (int)dr["Turn"];
                if (intTurnCGA4 == 27)
                {
                    arrResult.Add("CGA4:" + intTurnCGA4 + "");
                }
                //arrResult.Add("9");
            }
            catch
            {
                arrResult.Add("�Ʒ��ķ����ӳ�ʱ��");
            }

            try
            {
                //17173-3��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "17173"), CommandType.StoredProcedure, "GetGameRow");
                intTurn171733 = (int)dr["Turn"];
                if (intTurn171733 == 27)
                {
                    arrResult.Add("17173-3:" + intTurn171733 + "");
                }
                //arrResult.Add("11");
            }
            catch
            {
                arrResult.Add("17173-3�����ӳ�ʱ��");
            }

            try
            {
                //17173-6��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(6, "17173"), CommandType.StoredProcedure, "GetGameRow");
                intTurn171736 = (int)dr["Turn"];
                if (intTurn171736 == 27)
                {
                    arrResult.Add("17173-6:" + intTurn171736 + "");
                }
                //arrResult.Add("13");
            }
            catch
            {
                arrResult.Add("17173-6�����ӳ�ʱ��");
            }

            try
            {
                //51WAN4��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "51WAN"), CommandType.StoredProcedure, "GetGameRow");
                intTurn51WAN4 = (int)dr["Turn"];
                if (intTurn51WAN4 == 27)
                {
                    arrResult.Add("51WAN4:" + intTurn51WAN4 + "");
                }
                //arrResult.Add("15");
            }
            catch
            {
                arrResult.Add("51WAN4�����ӳ�ʱ��");
            }

            try
            {
                //DW1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "DW"), CommandType.StoredProcedure, "GetGameRow");
                intTurnDW1 = (int)dr["Turn"];
                if (intTurnDW1 == 27)
                {
                    arrResult.Add("DW1:" + intTurnDW1 + "");
                }
                //arrResult.Add("16");
            }
            catch
            {
                arrResult.Add("DW1�����ӳ�ʱ��");
            }

            try
            {
                //NBA1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(6, "NBA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnNBA1 = (int)dr["Turn"];
                if (intTurnNBA1 == 27)
                {
                    arrResult.Add("NBA1:" + intTurnNBA1 + "");
                }
                //arrResult.Add("17");
            }
            catch
            {
                arrResult.Add("NBA1�����ӳ�ʱ��");
            }

            try
            {
                //NBA2��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "NBA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnNBA2 = (int)dr["Turn"];
                if (intTurnNBA2 == 27)
                {
                    arrResult.Add("NBA2:" + intTurnNBA2 + "");
                }
                //arrResult.Add("18");
            }
            catch
            {
                arrResult.Add("NBA2�����ӳ�ʱ��");
            }

            try
            {
                //NBA5��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(5, "NBA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnNBA5 = (int)dr["Turn"];
                if (intTurnNBA5 == 27)
                {
                    arrResult.Add("NBA5:" + intTurnNBA5 + "");
                }
                //arrResult.Add("19");
            }
            catch
            {
                arrResult.Add("NBA5�����ӳ�ʱ��");
            }

            try
            {
                //SINA1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnSINA1 = (int)dr["Turn"];
                if (intTurnSINA1 == 27)
                {
                    arrResult.Add("SINA1:" + intTurnSINA1 + "");
                }
                //arrResult.Add("21");
            }
            catch
            {
                arrResult.Add("SINA1�����ӳ�ʱ��");
            }

            try
            {
                //SINA3��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "SINA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnSINA3 = (int)dr["Turn"];
                if (intTurnSINA3 == 27)
                {
                    arrResult.Add("SINA3:" + intTurnSINA3 + "");
                }
                //arrResult.Add("22");
            }
            catch
            {
                arrResult.Add("SINA3�����ӳ�ʱ��");
            }

            try
            {
                //CGC1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "CGC"), CommandType.StoredProcedure, "GetGameRow");
                intTurnCGC1 = (int)dr["Turn"];
                if (intTurnCGC1 == 27)
                {
                    arrResult.Add("CGC1:" + intTurnCGC1 + "");
                }
                //arrResult.Add("25");
            }
            catch
            {
                arrResult.Add("CGC1�����ӳ�ʱ��");
            }

            try
            {
                //KX1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "KX"), CommandType.StoredProcedure, "GetGameRow");
                intTurnKX1 = (int)dr["Turn"];
                if (intTurnKX1 == 27)
                {
                    arrResult.Add("KX1:" + intTurnKX1 + "");
                }
                //arrResult.Add("28");
            }
            catch
            {
                arrResult.Add("KX1�����ӳ�ʱ��");
            }

            try
            {
                //SOHU1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(51, "17173"), CommandType.StoredProcedure, "GetGameRow");
                intTurnSOHU1 = (int)dr["Turn"];
                if (intTurnSOHU1 == 27)
                {
                    arrResult.Add("SOHU1:" + intTurnSOHU1 + "");
                }
                //arrResult.Add("29");
            }
            catch
            {
                arrResult.Add("SOHU1�����ӳ�ʱ��");
            }

            try
            {
                //5S1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "5S"), CommandType.StoredProcedure, "GetGameRow");
                intTurn5S1 = (int)dr["Turn"];
                if (intTurn5S1 == 27)
                {
                    arrResult.Add("5S1:" + intTurn5S1 + "");
                }
                //arrResult.Add("30");
            }
            catch
            {
                arrResult.Add("5S1�����ӳ�ʱ��");
            }

            try
            {
                //TW1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TW"), CommandType.StoredProcedure, "GetGameRow");
                intTurnTW1 = (int)dr["Turn"];
                if (intTurnTW1 == 27)
                {
                    arrResult.Add("TW1:" + intTurnTW1 + "");
                }
                //arrResult.Add("31");
            }
            catch
            {
                arrResult.Add("TW1�����ӳ�ʱ��");
            }

            try
            {
                //TW2��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "TW"), CommandType.StoredProcedure, "GetGameRow");
                intTurnTW2 = (int)dr["Turn"];
                if (intTurnTW2 == 27)
                {
                    arrResult.Add("TW2:" + intTurnTW2 + "");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("TW2�����ӳ�ʱ��");
            }

            try
            {
                //PPS1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPS"), CommandType.StoredProcedure, "GetGameRow");
                intTurnPPS1 = (int)dr["Turn"];
                if (intTurnPPS1 == 27)
                {
                    arrResult.Add("PPS1:" + intTurnPPS1 + "");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("PPS1�����ӳ�ʱ��");
            }

            try
            {
                //PPS3��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "PPS"), CommandType.StoredProcedure, "GetGameRow");
                intTurnPPS3 = (int)dr["Turn"];
                if (intTurnPPS3 == 27)
                {
                    arrResult.Add("PPS3:" + intTurnPPS3 + "");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("PPS3�����ӳ�ʱ��");
            }

            try
            {
                //YL1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "YL"), CommandType.StoredProcedure, "GetGameRow");
                intTurnYL1 = (int)dr["Turn"];
                if (intTurnYL1 == 27)
                {
                    arrResult.Add("YL1:" + intTurnYL1 + "");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("YL1�����ӳ�ʱ��");
            }

            try
            {
                //SINAB1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "SINA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnSINAB4 = (int)dr["Turn"];
                if (intTurnSINAB4 == 27)
                {
                    arrResult.Add("NBstar1:" + intTurnSINAB4 + "");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("NBstar1�����ӳ�ʱ��");
            }

            try
            {
                //RR1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "RR"), CommandType.StoredProcedure, "GetGameRow");
                intTurnRR1 = (int)dr["Turn"];
                if (intTurnRR1 == 27)
                {
                    arrResult.Add("RR1:" + intTurnRR1 + "");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("RR1�����ӳ�ʱ��");
            }

            try
            {
                //HP1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "HP"), CommandType.StoredProcedure, "GetGameRow");
                intTurnHP1 = (int)dr["Turn"];
                if (intTurnHP1 == 27)
                {
                    arrResult.Add("HP1:" + intTurnHP1 + "");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("HP1�����ӳ�ʱ��");
            }

            try
            {
                //NXBA�¸����
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "NXBA"), CommandType.StoredProcedure, "GetGameRow");
                intTurnNXBA1 = (int)dr["Turn"];
                if (intTurnNXBA1 == 27)
                {
                    arrResult.Add("NXBA1:" + intTurnNXBA1 + "");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("NXBA�����ӳ�ʱ��");
            }

            foreach (string i in arrResult)
            {
                strBuild.Append(i.ToString() + "\\n");
                //Console.WriteLine(i);
            }
            
            if (strBuild.Length > 0)
            {
                strBuild.Append(string.Format("{0:G}", dt));
                return strBuild.ToString();
            }
            return strBuild.Append(false).ToString();
        }
        #endregion

        #region ��ȡ��������Ϸ״̬  GetBasketBallStatus()
        /// <summary>
        /// ��ȡ��������Ϸ״̬
        /// </summary>
        /// <returns></returns>
        public static string GetBasketBallStatus()
        {
            //�ٷ���
            byte byteStatusNF2, byteStatusHJ3, byteStatusHR4, byteStatusLN7, byteStatusKRT8, byteStatusLWF11, byteStatusFHC12;
            //�Ʒ���
            byte byteStatusCGA3, byteStatusCGA4;
            //17173��
            byte byteStatus171733, byteStatus171736;
            //51WAN��
            byte byteStatus51WAN4;
            //������
            byte byteStatusDW1;
            //�´���
            byte byteStatusNBA1, byteStatusNBA2, byteStatusNBA5;
            //������
            byte byteStatusSINA1, byteStatusSINA3;
            //������
            byte byteStatusCGC1;
            //������
            byte byteStatusKX1;
            //�Ѻ���
            byte byteStatusSOHU1;
            //5S��
            byte byteStatus5S1;
            //̨����
            byte byteStatusTW1, byteStatusTW2;
            //PPS��
            byte byteStatusPPS1, byteStatusPPS3;
            //YL��
            byte byteStatusYL1;
            //SINAB��
            byte byteStatusSINAB4;
            //RR��
            byte byteStatusRR1;
            //HP��
            byte byteStatusHP1;
            //NXBA�¸�����
            byte byteStatusNXBA1;

            StringBuilder strBuild = new StringBuilder("");

            DateTime dt = DateTime.Now;

            ArrayList arrResult = new ArrayList();
            DataRow dr;

            try
            {
                //�Ϸ���
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusNF2 = (byte)dr["Status"];
                if (byteStatusNF2 == 1)
                {
                    arrResult.Add("NF:" + byteStatusNF2 + "");
                }
                //arrResult.Add("2");
            }
            catch
            {
                arrResult.Add("�Ϸ������ӳ�ʱ��");
            }

            try
            {
                //�����
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(5, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusHJ3 = (byte)dr["Status"];
                if (byteStatusHJ3 == 1)
                {
                    arrResult.Add("HJ:" + byteStatusHJ3 + "");
                }
                //arrResult.Add("3");
            }
            catch
            {
                arrResult.Add("��������ӳ�ʱ��");
            }

            try
            {
                //���˷�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(7, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusHR4 = (byte)dr["Status"];
                if (byteStatusHR4 == 1)
                {
                    arrResult.Add("HR:" + byteStatusHR4 + "");
                }
                //arrResult.Add("4");
            }
            catch
            {
                arrResult.Add("���˷����ӳ�ʱ��");
            }

            try
            {
                //�������˷�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(10, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusKRT8 = (byte)dr["Status"];
                if (byteStatusKRT8 == 1)
                {
                    arrResult.Add("KRT:" + byteStatusKRT8 + "");
                }
                //arrResult.Add("6");
            }
            catch
            {
                arrResult.Add("�������˷����ӳ�ʱ��");
            }

            try
            {
                //���Ϸ�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(8, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusLN7 = (byte)dr["Status"];
                if (byteStatusLN7 == 1)
                {
                    arrResult.Add("LN:" + byteStatusLN7 + "");
                }
                //arrResult.Add("7");
            }
            catch
            {
                arrResult.Add("���Ϸ����ӳ�ʱ��");
            }

            try
            {
                //������
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(11, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusLWF11 = (byte)dr["Status"];
                if (byteStatusLWF11 == 1)
                {
                    arrResult.Add("LWF:" + byteStatusLWF11 + "");
                }
                //arrResult.Add("7");
            }
            catch
            {
                arrResult.Add("���Ϸ����ӳ�ʱ��");
            }

            try
            {
                //��˳Ƿ�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(12, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusFHC12 = (byte)dr["Status"];
                if (byteStatusFHC12 == 1)
                {
                    arrResult.Add("FHC:" + byteStatusFHC12 + "");
                }
                //arrResult.Add("7");
            }
            catch
            {
                arrResult.Add("��˳Ƿ����ӳ�ʱ��");
            }

            try
            {
                //�Ʒ�����
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "CGA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusCGA3 = (byte)dr["Status"];
                if (byteStatusCGA3 == 1)
                {
                    arrResult.Add("CGA3:" + byteStatusCGA3 + "");
                }
                //arrResult.Add("8");
            }
            catch
            {
                arrResult.Add("�Ʒ��������ӳ�ʱ��");
            }

            try
            {
                //�Ʒ��ķ�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "CGA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusCGA4 = (byte)dr["Status"];
                if (byteStatusCGA4 == 1)
                {
                    arrResult.Add("CGA4:" + byteStatusCGA4 + "");
                }
                //arrResult.Add("9");
            }
            catch
            {
                arrResult.Add("�Ʒ��ķ����ӳ�ʱ��");
            }

            try
            {
                //17173-3��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "17173"), CommandType.StoredProcedure, "GetGameRow");
                byteStatus171733 = (byte)dr["Status"];
                if (byteStatus171733 == 1)
                {
                    arrResult.Add("17173-3:" + byteStatus171733 + "");
                }
                //arrResult.Add("11");
            }
            catch
            {
                arrResult.Add("17173-3�����ӳ�ʱ��");
            }

            try
            {
                //17173-6��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(6, "17173"), CommandType.StoredProcedure, "GetGameRow");
                byteStatus171736 = (byte)dr["Status"];
                if (byteStatus171736 == 1)
                {
                    arrResult.Add("17173-6:" + byteStatus171736 + "");
                }
                //arrResult.Add("13");
            }
            catch
            {
                arrResult.Add("17173-6�����ӳ�ʱ��");
            }

            try
            {
                //51WAN4��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "51WAN"), CommandType.StoredProcedure, "GetGameRow");
                byteStatus51WAN4 = (byte)dr["Status"];
                if (byteStatus51WAN4 == 1)
                {
                    arrResult.Add("51WAN4:" + byteStatus51WAN4 + "");
                }
                //arrResult.Add("15");
            }
            catch
            {
                arrResult.Add("51WAN4�����ӳ�ʱ��");
            }

            try
            {
                //DW1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "DW"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusDW1 = (byte)dr["Status"];
                if (byteStatusDW1 == 1)
                {
                    arrResult.Add("DW1:" + byteStatusDW1 + "");
                }
                //arrResult.Add("16");
            }
            catch
            {
                arrResult.Add("DW1�����ӳ�ʱ��");
            }

            try
            {
                //NBA1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(6, "NBA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusNBA1 = (byte)dr["Status"];
                if (byteStatusNBA1 == 1)
                {
                    arrResult.Add("NBA1:" + byteStatusNBA1 + "");
                }
                //arrResult.Add("17");
            }
            catch
            {
                arrResult.Add("NBA1�����ӳ�ʱ��");
            }

            try
            {
                //NBA2��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "NBA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusNBA2 = (byte)dr["Status"];
                if (byteStatusNBA2 == 1)
                {
                    arrResult.Add("NBA2:" + byteStatusNBA2 + "");
                }
                //arrResult.Add("18");
            }
            catch
            {
                arrResult.Add("NBA2�����ӳ�ʱ��");
            }

            try
            {
                //NBA5��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(5, "NBA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusNBA5 = (byte)dr["Status"];
                if (byteStatusNBA5 == 1)
                {
                    arrResult.Add("NBA5:" + byteStatusNBA5 + "");
                }
                //arrResult.Add("19");
            }
            catch
            {
                arrResult.Add("NBA5�����ӳ�ʱ��");
            }

            try
            {
                //SINA1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusSINA1 = (byte)dr["Status"];
                if (byteStatusSINA1 == 1)
                {
                    arrResult.Add("SINA1:" + byteStatusSINA1 + "");
                }
                //arrResult.Add("21");
            }
            catch
            {
                arrResult.Add("SINA1�����ӳ�ʱ��");
            }

            try
            {
                //SINA3��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "SINA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusSINA3 = (byte)dr["Status"];
                if (byteStatusSINA3 == 1)
                {
                    arrResult.Add("SINA3:" + byteStatusSINA3 + "");
                }
                //arrResult.Add("22");
            }
            catch
            {
                arrResult.Add("SINA3�����ӳ�ʱ��");
            }

            try
            {
                //CGC1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "CGC"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusCGC1 = (byte)dr["Status"];
                if (byteStatusCGC1 == 1)
                {
                    arrResult.Add("CGC1:" + byteStatusCGC1 + "");
                }
                //arrResult.Add("25");
            }
            catch
            {
                arrResult.Add("CGC1�����ӳ�ʱ��");
            }

            try
            {
                //KX1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "KX"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusKX1 = (byte)dr["Status"];
                if (byteStatusKX1 == 1)
                {
                    arrResult.Add("KX1:" + byteStatusKX1 + "");
                }
                //arrResult.Add("28");
            }
            catch
            {
                arrResult.Add("KX1�����ӳ�ʱ��");
            }

            try
            {
                //SOHU1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(51, "17173"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusSOHU1 = (byte)dr["Status"];
                if (byteStatusSOHU1 == 1)
                {
                    arrResult.Add("SOHU1:" + byteStatusSOHU1 + "");
                }
                //arrResult.Add("29");
            }
            catch
            {
                arrResult.Add("SOHU1�����ӳ�ʱ��");
            }

            try
            {
                //5S1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "5S"), CommandType.StoredProcedure, "GetGameRow");
                byteStatus5S1 = (byte)dr["Status"];
                if (byteStatus5S1 == 1)
                {
                    arrResult.Add("5S1:" + byteStatus5S1 + "");
                }
                //arrResult.Add("30");
            }
            catch
            {
                arrResult.Add("5S1�����ӳ�ʱ��");
            }

            try
            {
                //TW1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TW"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusTW1 = (byte)dr["Status"];
                if (byteStatusTW1 == 1)
                {
                    arrResult.Add("TW1:" + byteStatusTW1 + "");
                }
                //arrResult.Add("30");
            }
            catch
            {
                arrResult.Add("TW1�����ӳ�ʱ��");
            }

            try
            {
                //5S1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "TW"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusTW2 = (byte)dr["Status"];
                if (byteStatusTW2 == 1)
                {
                    arrResult.Add("TW2:" + byteStatusTW2 + "");
                }
                //arrResult.Add("30");
            }
            catch
            {
                arrResult.Add("TW2�����ӳ�ʱ��");
            }

            try
            {
                //PPS1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPS"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusPPS1 = (byte)dr["Status"];
                if (byteStatusPPS1 == 1)
                {
                    arrResult.Add("PPS1:" + byteStatusPPS1 + "");
                }
                //arrResult.Add("30");
            }
            catch
            {
                arrResult.Add("PPS1�����ӳ�ʱ��");
            }

            try
            {
                //PPS3��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "PPS"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusPPS3 = (byte)dr["Status"];
                if (byteStatusPPS3 == 1)
                {
                    arrResult.Add("PPS3:" + byteStatusPPS3 + "");
                }
                //arrResult.Add("30");
            }
            catch
            {
                arrResult.Add("PPS3�����ӳ�ʱ��");
            }

            try
            {
                //YL1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "YL"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusYL1 = (byte)dr["Status"];
                if (byteStatusYL1 == 1)
                {
                    arrResult.Add("YL1:" + byteStatusYL1 + "");
                }
                //arrResult.Add("30");
            }
            catch
            {
                arrResult.Add("YL1�����ӳ�ʱ��");
            }

            try
            {
                //SINAB4��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "SINA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusSINAB4 = (byte)dr["Status"];
                if (byteStatusSINAB4 == 1)
                {
                    arrResult.Add("NBstar1:" + byteStatusSINAB4 + "");
                }
                //arrResult.Add("30");
            }
            catch
            {
                arrResult.Add("NBstar1�����ӳ�ʱ��");
            }

            try
            {
                //RR1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "RR"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusRR1 = (byte)dr["Status"];
                if (byteStatusRR1 == 1)
                {
                    arrResult.Add("RR1:" + byteStatusRR1 + "");
                }
                //arrResult.Add("30");
            }
            catch
            {
                arrResult.Add("RR1�����ӳ�ʱ��");
            }

            try
            {
                //HP1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "HP"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusHP1 = (byte)dr["Status"];
                if (byteStatusHP1 == 1)
                {
                    arrResult.Add("HP1:" + byteStatusHP1 + "");
                }
                //arrResult.Add("30");
            }
            catch
            {
                arrResult.Add("HP1�����ӳ�ʱ��");
            }

            try
            {
                //NXBA1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "NXBA"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusNXBA1 = (byte)dr["Status"];
                if (byteStatusNXBA1 == 1)
                {
                    arrResult.Add("NXBA1:" + byteStatusNXBA1 + "");
                }
                //arrResult.Add("30");
            }
            catch
            {
                arrResult.Add("NXBA1�����ӳ�ʱ��");
            }

            foreach (string i in arrResult)
            {
                strBuild.Append(i.ToString() + "\\n");
                //Console.WriteLine(i);
            }
            
            if (strBuild.Length > 0)
            {
                strBuild.Append(string.Format("{0:G}", dt));
                return strBuild.ToString();
            }
            return strBuild.Append(false).ToString();
        }
        #endregion

        #region ��ȡ��������Ϸ�ִ� GetFootBallTurn()
        /// <summary>
        /// ��ȡ��������Ϸ�ִ�
        /// </summary>
        /// <returns></returns>
        public static string GetFootBallTurn()
        {
            //�ٷ���
            int intTurnFB4;
            //������
            int intTurnDW1;
            //17173��
            int intTurn171731;
            //�´���
            //int intTurnNBA1;
            //�г���
            int intTurnCSL1;
            //��̳��
            int intTurnTT1, intTurnTT2;
            //ESL��
            int intTurnESL1;
            //PPSF��
            int intTurnPPSF1;
            //MOP��
            int intTurnMOPF1;
            //TY������
            int intTurnTYF1;
            //XBAF2�ٷ�����2
            int intTurnXBA2FB1, intTurnXBA2FB2;
            //PPS2F PPS����2
            int intTurnPPS2F1;
            //SINA2F SINA����2
            int intTurnSINA2F1;
            //PPL PPL����2
            int intTurnPPLF1;
            //SOHUF SOHUF����2
            int intTurnSOHUF1;
            //TWF ̨����������
            int intTurnTWF1;
            //XBAF3�ٷ�����3
            int intTurnXBA3FB1, intTurnXBA3FB3, intTurnXBA3FB4;
            //7K7K�ٷ�����
            int intTurn7K7K3FB1;
            //YXY�ٷ�����
            int intTurnYXY1FB1;

            StringBuilder strBuild = new StringBuilder("");

            DateTime dt = DateTime.Now;

            ArrayList arrResult = new ArrayList();
            DataRow dr;

            try
            {
                //FB4��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "XBAF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnFB4 = (byte)dr["Turn"];
                if (intTurnFB4 == 23)
                {
                    arrResult.Add("FB-4:" + intTurnFB4 + "");
                }
            }
            catch
            {
                arrResult.Add("FB-4�����ӳ�ʱ��");
            }

            try
            {
                //DWF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "DWF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnDW1 = (byte)dr["Turn"];
                if (intTurnDW1 == 23)
                {
                    arrResult.Add("DW-F1:" + intTurnDW1 + "");
                }
            }
            catch
            {
                arrResult.Add("DW-F1�����ӳ�ʱ��");
            }

            try
            {
                //17173F1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "17173F"), CommandType.StoredProcedure, "GetGameRow");
                intTurn171731 = (byte)dr["Turn"];
                if (intTurn171731 == 23)
                {
                    arrResult.Add("17173-F1:" + intTurn171731 + "");
                }
            }
            catch
            {
                arrResult.Add("17173-F1�����ӳ�ʱ��");
            }

            //try
            //{
            //    //NBAF1��
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "NBAF"), CommandType.StoredProcedure, "GetGameRow");
            //    intTurnNBA1 = (byte)dr["Turn"];
            //    if (intTurnNBA1 == 23)
            //    {
            //        arrResult.Add("NBA-F1:" + intTurnNBA1 + "");
            //    }
            //}
            //catch
            //{
            //    arrResult.Add("NBA-F1�����ӳ�ʱ��");
            //}

            try
            {
                //CSLF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINAF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnCSL1 = (byte)dr["Turn"];
                if (intTurnCSL1 == 23)
                {
                    arrResult.Add("CSL-F1:" + intTurnCSL1 + "");
                }
            }
            catch
            {
                arrResult.Add("CSL-F1�����ӳ�ʱ��");
            }

            try
            {
                //TT1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TT"), CommandType.StoredProcedure, "GetGameRow");
                intTurnTT1 = (byte)dr["Turn"];
                if (intTurnTT1 == 23)
                {
                    arrResult.Add("TT-F1:" + intTurnTT1 + "");
                }
            }
            catch
            {
                arrResult.Add("TT-F1�����ӳ�ʱ��");
            }

            try
            {
                //TT2��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "TT"), CommandType.StoredProcedure, "GetGameRow");
                intTurnTT2 = (byte)dr["Turn"];
                if (intTurnTT2 == 23)
                {
                    arrResult.Add("TT-F2:" + intTurnTT2 + "");
                }
            }
            catch
            {
                arrResult.Add("TT-F2�����ӳ�ʱ��");
            }

            try
            {
                //ESL1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "ESL"), CommandType.StoredProcedure, "GetGameRow");
                intTurnESL1 = (byte)dr["Turn"];
                if (intTurnESL1 == 23)
                {
                    arrResult.Add("ESL-F1:" + intTurnESL1 + "");
                }
            }
            catch
            {
                arrResult.Add("ESL-F1�����ӳ�ʱ��");
            }

            try
            {
                //PPSF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPSF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnPPSF1 = (byte)dr["Turn"];
                if (intTurnPPSF1 == 23)
                {
                    arrResult.Add("PPSF-F1:" + intTurnPPSF1 + "");
                }
            }
            catch
            {
                arrResult.Add("PPSF-F1�����ӳ�ʱ��");
            }

            try
            {
                //MOPF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "MOP"), CommandType.StoredProcedure, "GetGameRow");
                intTurnMOPF1 = (byte)dr["Turn"];
                if (intTurnMOPF1 == 23)
                {
                    arrResult.Add("MOPF-F1:" + intTurnMOPF1 + "");
                }
            }
            catch
            {
                arrResult.Add("MOPF-F1�����ӳ�ʱ��");
            }

            try
            {
                //TYF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TY"), CommandType.StoredProcedure, "GetGameRow");
                intTurnTYF1 = (byte)dr["Turn"];
                if (intTurnTYF1 == 23)
                {
                    arrResult.Add("TY-F1:" + intTurnTYF1 + "");
                }
            }
            catch
            {
                arrResult.Add("TY-F1�����ӳ�ʱ��");
            }

            try
            {
                //XBA2FB1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "XBAF2"), CommandType.StoredProcedure, "GetGameRow");
                intTurnXBA2FB1 = (byte)dr["Turn"];
                if (intTurnXBA2FB1 == 23)
                {
                    arrResult.Add("XBA2FB1:" + intTurnXBA2FB1 + "");
                }
            }
            catch
            {
                arrResult.Add("XBA2FB1�����ӳ�ʱ��");
            }

            try
            {
                //PPS2F1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPS2F"), CommandType.StoredProcedure, "GetGameRow");
                intTurnPPS2F1 = (byte)dr["Turn"];
                if (intTurnPPS2F1 == 23)
                {
                    arrResult.Add("PPS2F1:" + intTurnPPS2F1 + "");
                }
            }
            catch
            {
                arrResult.Add("PPS2F1�����ӳ�ʱ��");
            }

            try
            {
                //SINA2F1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINA2F"), CommandType.StoredProcedure, "GetGameRow");
                intTurnSINA2F1 = (byte)dr["Turn"];
                if (intTurnSINA2F1 == 23)
                {
                    arrResult.Add("SINA2F1:" + intTurnSINA2F1 + "");
                }
            }
            catch
            {
                arrResult.Add("SINA2F1�����ӳ�ʱ��");
            }

            try
            {
                //PPLF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPL"), CommandType.StoredProcedure, "GetGameRow");
                intTurnPPLF1 = (byte)dr["Turn"];
                if (intTurnPPLF1 == 23)
                {
                    arrResult.Add("PPLF1:" + intTurnPPLF1 + "");
                }
            }
            catch
            {
                arrResult.Add("PPLF1�����ӳ�ʱ��");
            }

            try
            {
                //SOHUF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SOHUF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnSOHUF1 = (byte)dr["Turn"];
                if (intTurnSOHUF1 == 23)
                {
                    arrResult.Add("SOHUF1:" + intTurnSOHUF1 + "");
                }
            }
            catch
            {
                arrResult.Add("SOHUF1�����ӳ�ʱ��");
            }

            try
            {
                //XBA2FB2��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "XBAF2"), CommandType.StoredProcedure, "GetGameRow");
                intTurnXBA2FB2 = (byte)dr["Turn"];
                if (intTurnXBA2FB2 == 23)
                {
                    arrResult.Add("XBA2FB2:" + intTurnXBA2FB2 + "");
                }
            }
            catch
            {
                arrResult.Add("XBA2FB2�����ӳ�ʱ��");
            }

            try
            {
                //TWF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TWF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnTWF1 = (byte)dr["Turn"];
                if (intTurnTWF1 == 23)
                {
                    arrResult.Add("TWF1:" + intTurnTWF1 + "");
                }
            }
            catch
            {
                arrResult.Add("TWF1�����ӳ�ʱ��");
            }

            try
            {
                //XBA3FB1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "XBAF3"), CommandType.StoredProcedure, "GetGameRow");
                intTurnXBA3FB1 = (byte)dr["Turn"];
                if (intTurnXBA3FB1 == 23)
                {
                    arrResult.Add("XBA3FB1:" + intTurnXBA3FB1 + "");
                }
            }
            catch
            {
                arrResult.Add("XBA3FB1�����ӳ�ʱ��");
            }

            try
            {
                //XBA3FB3��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "XBAF3"), CommandType.StoredProcedure, "GetGameRow");
                intTurnXBA3FB3 = (byte)dr["Turn"];
                if (intTurnXBA3FB3 == 23)
                {
                    arrResult.Add("XBA3FB3:" + intTurnXBA3FB3 + "");
                }
            }
            catch
            {
                arrResult.Add("XBA3FB3�����ӳ�ʱ��");
            }

            try
            {
                //XBA3FB4��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "XBAF3"), CommandType.StoredProcedure, "GetGameRow");
                intTurnXBA3FB4 = (byte)dr["Turn"];
                if (intTurnXBA3FB4 == 23)
                {
                    arrResult.Add("XBA3FB4:" + intTurnXBA3FB4 + "");
                }
            }
            catch
            {
                arrResult.Add("XBA3FB4�����ӳ�ʱ��");
            }

            try
            {
                //7K7K3FB1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "7K7K"), CommandType.StoredProcedure, "GetGameRow");
                intTurn7K7K3FB1 = (byte)dr["Turn"];
                if (intTurn7K7K3FB1 == 23)
                {
                    arrResult.Add("7K7K3FB1:" + intTurn7K7K3FB1 + "");
                }
            }
            catch
            {
                arrResult.Add("7K7K3FB1�����ӳ�ʱ��");
            }

            try
            {
                //YXY1FB1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "YXY"), CommandType.StoredProcedure, "GetGameRow");
                intTurnYXY1FB1 = (byte)dr["Turn"];
                if (intTurnYXY1FB1 == 23)
                {
                    arrResult.Add("YXY1FB1:" + intTurnYXY1FB1 + "");
                }
            }
            catch
            {
                arrResult.Add("YXY1FB1�����ӳ�ʱ��");
            }

            foreach (string i in arrResult)
            {
                strBuild.Append(i.ToString() + "\\n");
                //Console.WriteLine(i);
            }

            //Console.ReadLine();
            if (strBuild.Length > 0)
            {
                strBuild.Append(string.Format("{0:G}", dt));
                return strBuild.ToString();
            }
            return strBuild.Append(false).ToString();
        }
        #endregion

        #region ��ȡ��������Ϸ״̬ GetFootBallStatus()
        /// <summary>
        /// ��ȡ��������Ϸ״̬
        /// </summary>
        /// <returns></returns>
        public static string GetFootBallStatus()
        {
            //�ٷ���
            byte byteStatusFB4;
            bool boolFinishFB4;
            //������
            byte byteStatusDW1;
            bool boolFinishDW1;
            //17173��
            byte byteStatus171731;
            bool boolFinish171731;
            //�´���
            //byte byteStatusNBA1;
            //bool boolFinishNBA1;
            //�г���
            byte byteStatusCSL1;
            bool boolFinishCSL1;
            //��̳��
            byte byteStatusTT1, byteStatusTT2;
            bool boolFinishTT1, boolFinishTT2;
            //ESL��
            byte byteStatusESL1;
            bool boolFinishESL1;
            //PPSF��
            byte byteStatusPPSF1;
            bool boolFinishPPSF1;
            //MOP��
            byte byteStatusMOPF1;
            bool boolFinishMOPF1;
            //TY������
            byte byteStatusTYF1;
            bool boolFinishTYF1;
            //XBAF2�ٷ�����2
            byte byteStatusXBA2FB1, byteStatusXBA2FB2;
            bool boolFinishXBA2FB1, boolFinishXBA2FB2;
            //PPS2F PPS2����2
            byte byteStatusPPS2F1;
            bool boolFinishPPS2F1;
            //SINA2F SINA����2
            byte byteStatusSINA2F1;
            bool boolFinishSINA2F1;
            //PPL PPL����2
            byte byteStatusPPLF1;
            bool boolFinishPPLF1;
            //SOHUF SOHUF����2
            byte byteStatusSOHUF1;
            bool boolFinishSOHUF1;
            //TWF ̨������
            byte byteStatusTWF1;
            bool boolFinishTWF1;
            //XBAF3�ٷ�����3
            byte byteStatusXBA3FB1, byteStatusXBA3FB3, byteStatusXBA3FB4;
            bool boolFinishXBA3FB1, boolFinishXBA3FB3, boolFinishXBA3FB4;
            //7K7K����
            byte byteStatus7K7K3FB1;
            bool boolFinish7K7K3FB1;
            //YXY����
            byte byteStatusYXY1FB1;
            bool boolFinishYXY1FB1;

            StringBuilder strBuild = new StringBuilder("");

            DateTime dt = DateTime.Now;

            ArrayList arrResult = new ArrayList();

            DataRow dr;

            try
            {
                //FB4��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "XBAF"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusFB4 = (byte)dr["Status"];
                boolFinishFB4 = (bool)dr["IsFinish"];
                if (boolFinishFB4 == false)
                {
                    arrResult.Add("FB-F4:��������");
                }
                else if (byteStatusFB4 == 1)
                {
                    arrResult.Add("FB-F4:ҹ����³���");
                }
                else if (byteStatusFB4 == 2)
                {
                    arrResult.Add("FB-F4:�������³���");
                }
            }
            catch
            {
                arrResult.Add("FB-F4�����ӳ�ʱ��");
            }

            try
            {
                //DWF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "DWF"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusDW1 = (byte)dr["Status"];
                boolFinishDW1 = (bool)dr["IsFinish"];
                if (boolFinishDW1 == false)
                {
                    arrResult.Add("DW-F1:��������");
                }
                else if (byteStatusDW1 == 1)
                {
                    arrResult.Add("DW-F1:ҹ����³���");
                }
                else if (byteStatusDW1 == 2)
                {
                    arrResult.Add("DW-F1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("DW-F1�����ӳ�ʱ��");
            }

            try
            {
                //17173F1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "17173F"), CommandType.StoredProcedure, "GetGameRow");
                byteStatus171731 = (byte)dr["Status"];
                boolFinish171731 = (bool)dr["IsFinish"];
                if (boolFinish171731 == false)
                {
                    arrResult.Add("17173-F1:��������");
                }
                else if (byteStatus171731 == 1)
                {
                    arrResult.Add("17173-F1:ҹ����³���");
                }
                else if (byteStatus171731 == 2)
                {
                    arrResult.Add("17173-F1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("17173-F1�����ӳ�ʱ��");
            }

            //try
            //{
            //    //NBAF1��
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "NBAF"), CommandType.StoredProcedure, "GetGameRow");
            //    byteStatusNBA1 = (byte)dr["Status"];
            //    boolFinishNBA1 = (bool)dr["IsFinish"];
            //    if (boolFinishNBA1 == false)
            //    {
            //        arrResult.Add("NBA-F1:��������");
            //    }
            //    else if (byteStatusNBA1 == 1)
            //    {
            //        arrResult.Add("NBA-F1:ҹ����³���");
            //    }
            //    else if (byteStatusNBA1 == 2)
            //    {
            //        arrResult.Add("NBA-F1:�������³���");
            //    }
            //}
            //catch
            //{
            //    arrResult.Add("NBA-F1�����ӳ�ʱ��");
            //}

            try
            {
                //CSLF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINAF"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusCSL1 = (byte)dr["Status"];
                boolFinishCSL1 = (bool)dr["IsFinish"];
                if (boolFinishCSL1 == false)
                {
                    arrResult.Add("CSL-F1:��������");
                }
                else if (byteStatusCSL1 == 1)
                {
                    arrResult.Add("CSL-F1:ҹ����³���");
                }
                else if (byteStatusCSL1 == 2)
                {
                    arrResult.Add("CSL-F1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("CSL-F1�����ӳ�ʱ��");
            }

            try
            {
                //TT1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TT"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusTT1 = (byte)dr["Status"];
                boolFinishTT1 = (bool)dr["IsFinish"];
                if (boolFinishTT1 == false)
                {
                    arrResult.Add("TT-F1:��������");
                }
                else if (byteStatusTT1 == 1)
                {
                    arrResult.Add("TT-F1:ҹ����³���");
                }
                else if (byteStatusTT1 == 2)
                {
                    arrResult.Add("TT-F1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("TT-F1�����ӳ�ʱ��");
            }

            try
            {
                //TT2��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "TT"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusTT2 = (byte)dr["Status"];
                boolFinishTT2 = (bool)dr["IsFinish"];
                if (boolFinishTT2 == false)
                {
                    arrResult.Add("TT-F2:��������");
                }
                else if (byteStatusTT2 == 1)
                {
                    arrResult.Add("TT-F2:ҹ����³���");
                }
                else if (byteStatusTT2 == 2)
                {
                    arrResult.Add("TT-F2:�������³���");
                }
            }
            catch
            {
                arrResult.Add("TT-F2�����ӳ�ʱ��");
            }

            try
            {
                //ESL1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "ESL"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusESL1 = (byte)dr["Status"];
                boolFinishESL1 = (bool)dr["IsFinish"];
                if (boolFinishESL1 == false)
                {
                    arrResult.Add("ESL-F1:��������");
                }
                else if (byteStatusESL1 == 1)
                {
                    arrResult.Add("ESL-F1:ҹ����³���");
                }
                else if (byteStatusESL1 == 2)
                {
                    arrResult.Add("ESL-F1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("ESL-F1�����ӳ�ʱ��");
            }

            try
            {
                //PPSF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPSF"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusPPSF1 = (byte)dr["Status"];
                boolFinishPPSF1 = (bool)dr["IsFinish"];
                if (boolFinishPPSF1 == false)
                {
                    arrResult.Add("PPSF-F1:��������");
                }
                else if (byteStatusPPSF1 == 1)
                {
                    arrResult.Add("PPSF-F1:ҹ����³���");
                }
                else if (byteStatusPPSF1 == 2)
                {
                    arrResult.Add("PPSF-F1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("PPSF-F1�����ӳ�ʱ��");
            }

            try
            {
                //MOPF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "MOP"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusMOPF1 = (byte)dr["Status"];
                boolFinishMOPF1 = (bool)dr["IsFinish"];
                if (boolFinishMOPF1 == false)
                {
                    arrResult.Add("MOPF-F1:��������");
                }
                else if (byteStatusMOPF1 == 1)
                {
                    arrResult.Add("MOPF-F1:ҹ����³���");
                }
                else if (byteStatusMOPF1 == 2)
                {
                    arrResult.Add("MOPF-F1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("MOPF-F1�����ӳ�ʱ��");
            }

            try
            {
                //TYF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TY"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusTYF1 = (byte)dr["Status"];
                boolFinishTYF1 = (bool)dr["IsFinish"];
                if (boolFinishTYF1 == false)
                {
                    arrResult.Add("TY-F1:��������");
                }
                else if (byteStatusTYF1 == 1)
                {
                    arrResult.Add("TY-F1:ҹ����³���");
                }
                else if (byteStatusTYF1 == 2)
                {
                    arrResult.Add("TY-F1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("TY-F1�����ӳ�ʱ��");
            }

            try
            {
                //XBA2FB1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "XBAF2"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusXBA2FB1 = (byte)dr["Status"];
                boolFinishXBA2FB1 = (bool)dr["IsFinish"];
                if (boolFinishXBA2FB1 == false)
                {
                    arrResult.Add("XBA2FB1:��������");
                }
                else if (byteStatusXBA2FB1 == 1)
                {
                    arrResult.Add("XBA2FB1:ҹ����³���");
                }
                else if (byteStatusXBA2FB1 == 2)
                {
                    arrResult.Add("XBA2FB1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("XBA2FB1�����ӳ�ʱ��");
            }

            try
            {
                //PPS2F1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPS2F"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusPPS2F1 = (byte)dr["Status"];
                boolFinishPPS2F1 = (bool)dr["IsFinish"];
                if (boolFinishPPS2F1 == false)
                {
                    arrResult.Add("PPS2F1:��������");
                }
                else if (byteStatusPPS2F1 == 1)
                {
                    arrResult.Add("PPS2F1:ҹ����³���");
                }
                else if (byteStatusPPS2F1 == 2)
                {
                    arrResult.Add("PPS2F1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("PPS2F1�����ӳ�ʱ��");
            }

            try
            {
                //SINA2F1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINA2F"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusSINA2F1 = (byte)dr["Status"];
                boolFinishSINA2F1 = (bool)dr["IsFinish"];
                if (boolFinishSINA2F1 == false)
                {
                    arrResult.Add("SINA2F1:��������");
                }
                else if (byteStatusSINA2F1 == 1)
                {
                    arrResult.Add("SINA2F1:ҹ����³���");
                }
                else if (byteStatusSINA2F1 == 2)
                {
                    arrResult.Add("SINA2F1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("SINA2F1�����ӳ�ʱ��");
            }

            try
            {
                //PPLF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPL"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusPPLF1 = (byte)dr["Status"];
                boolFinishPPLF1 = (bool)dr["IsFinish"];
                if (boolFinishPPLF1 == false)
                {
                    arrResult.Add("PPLF1:��������");
                }
                else if (byteStatusPPLF1 == 1)
                {
                    arrResult.Add("PPLF1:ҹ����³���");
                }
                else if (byteStatusPPLF1 == 2)
                {
                    arrResult.Add("PPLF1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("PPLF1�����ӳ�ʱ��");
            }

            try
            {
                //SOHUF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SOHUF"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusSOHUF1 = (byte)dr["Status"];
                boolFinishSOHUF1 = (bool)dr["IsFinish"];
                if (boolFinishSOHUF1 == false)
                {
                    arrResult.Add("SOHUF1:��������");
                }
                else if (byteStatusSOHUF1 == 1)
                {
                    arrResult.Add("SOHUF1:ҹ����³���");
                }
                else if (byteStatusSOHUF1 == 2)
                {
                    arrResult.Add("SOHUF1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("SOHUF1�����ӳ�ʱ��");
            }

            try
            {
                //XBA2FB2��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "XBAF2"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusXBA2FB2 = (byte)dr["Status"];
                boolFinishXBA2FB2 = (bool)dr["IsFinish"];
                if (boolFinishXBA2FB2 == false)
                {
                    arrResult.Add("XBA2FB2:��������");
                }
                else if (byteStatusXBA2FB2 == 1)
                {
                    arrResult.Add("XBA2FB2:ҹ����³���");
                }
                else if (byteStatusXBA2FB2 == 2)
                {
                    arrResult.Add("XBA2FB2:�������³���");
                }
            }
            catch
            {
                arrResult.Add("XBA2FB2�����ӳ�ʱ��");
            }

            try
            {
                //TWF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TWF"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusTWF1 = (byte)dr["Status"];
                boolFinishTWF1 = (bool)dr["IsFinish"];
                if (boolFinishTWF1 == false)
                {
                    arrResult.Add("TWF1:��������");
                }
                else if (byteStatusTWF1 == 1)
                {
                    arrResult.Add("TWF1:ҹ����³���");
                }
                else if (byteStatusTWF1 == 2)
                {
                    arrResult.Add("TWF1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("TWF1�����ӳ�ʱ��");
            }

            try
            {
                //XBA3FB1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "XBAF3"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusXBA3FB1 = (byte)dr["Status"];
                boolFinishXBA3FB1 = (bool)dr["IsFinish"];
                if (boolFinishXBA3FB1 == false)
                {
                    arrResult.Add("XBA3FB1:��������");
                }
                else if (byteStatusXBA3FB1 == 1)
                {
                    arrResult.Add("XBA3FB1:ҹ����³���");
                }
                else if (byteStatusXBA3FB1 == 2)
                {
                    arrResult.Add("XBA3FB1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("XBAF3FB1�����ӳ�ʱ��");
            }

            try
            {
                //XBA3FB3��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "XBAF3"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusXBA3FB3 = (byte)dr["Status"];
                boolFinishXBA3FB3 = (bool)dr["IsFinish"];
                if (boolFinishXBA3FB3 == false)
                {
                    arrResult.Add("XBA3FB3:��������");
                }
                else if (byteStatusXBA3FB3 == 1)
                {
                    arrResult.Add("XBA3FB3:ҹ����³���");
                }
                else if (byteStatusXBA3FB3 == 2)
                {
                    arrResult.Add("XBA3FB3:�������³���");
                }
            }
            catch
            {
                arrResult.Add("XBAF3FB1�����ӳ�ʱ��");
            }

            try
            {
                //XBA3FB4��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "XBAF3"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusXBA3FB4 = (byte)dr["Status"];
                boolFinishXBA3FB4 = (bool)dr["IsFinish"];
                if (boolFinishXBA3FB4 == false)
                {
                    arrResult.Add("XBA3FB4:��������");
                }
                else if (byteStatusXBA3FB4 == 1)
                {
                    arrResult.Add("XBA3FB4:ҹ����³���");
                }
                else if (byteStatusXBA3FB4 == 2)
                {
                    arrResult.Add("XBA3FB4:�������³���");
                }
            }
            catch
            {
                arrResult.Add("XBAF3FB4�����ӳ�ʱ��");
            }

            try
            {
                //7K7K3FB1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "7K7K"), CommandType.StoredProcedure, "GetGameRow");
                byteStatus7K7K3FB1 = (byte)dr["Status"];
                boolFinish7K7K3FB1 = (bool)dr["IsFinish"];
                if (boolFinish7K7K3FB1 == false)
                {
                    arrResult.Add("7K7K3FB1:��������");
                }
                else if (byteStatus7K7K3FB1 == 1)
                {
                    arrResult.Add("7K7K3FB1:ҹ����³���");
                }
                else if (byteStatus7K7K3FB1 == 2)
                {
                    arrResult.Add("7K7K3FB1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("7K7K3FB1�����ӳ�ʱ��");
            }

            try
            {
                //YXY1FB1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "YXY"), CommandType.StoredProcedure, "GetGameRow");
                byteStatusYXY1FB1 = (byte)dr["Status"];
                boolFinishYXY1FB1 = (bool)dr["IsFinish"];
                if (boolFinishYXY1FB1 == false)
                {
                    arrResult.Add("YXY1FB1:��������");
                }
                else if (byteStatusYXY1FB1 == 1)
                {
                    arrResult.Add("YXY1FB1:ҹ����³���");
                }
                else if (byteStatusYXY1FB1 == 2)
                {
                    arrResult.Add("YXY1FB1:�������³���");
                }
            }
            catch
            {
                arrResult.Add("YXY1FB1�����ӳ�ʱ��");
            }

            foreach (string i in arrResult)
            {
                strBuild.Append(i.ToString() + "\\n");
                //Console.WriteLine(i);
            }
            //Console.ReadLine();
            if (strBuild.Length > 0)
            {
                strBuild.Append(string.Format("{0:G}", dt));
                return strBuild.ToString();
            }
            return strBuild.Append(false).ToString();
        }
        #endregion

        #region ��ȡ����ҹ�����״̬ GetBasketBallUpdateNightCheck()
        /// <summary>
        /// ��ȡ����ҹ�����״̬
        /// </summary>
        /// <returns></returns>
        public static string GetBasketBallUpdateNightCheck()
        {
            //�ٷ���
            DateTime dtimeFinishTimeNF2, dtimeFinishTimeHJ3, dtimeFinishTimeHR4, dtimeFinishTimeLN7, dtimeFinishTimeKRT8, dtimeFinishTimeLWF11, dtimeFinishTimeFHC12;
            //�Ʒ���
            DateTime dtimeFinishTimeCGA3, dtimeFinishTimeCGA4;
            //17173��
            DateTime dtimeFinishTime171733, dtimeFinishTime171736;
            //51WAN��
            DateTime dtimeFinishTime51WAN4;
            //������
            DateTime dtimeFinishTimeDW1;
            //�´���
            DateTime /*dtimeFinishTimeNBA1,*/ dtimeFinishTimeNBA2, dtimeFinishTimeNBA5;
            //������
            DateTime dtimeFinishTimeSINA1, dtimeFinishTimeSINA3;
            //������
            DateTime dtimeFinishTimeCGC1;
            //������
            //DateTime dtimeFinishTimeKX1;
            //�Ѻ���
            DateTime dtimeFinishTimeSOHU1;
            //5S��
            //DateTime dtimeFinishTime5S1;
            //̨����
            //DateTime dtimeFinishTimeTW1, dtimeFinishTimeTW2;
            //PPS��
            DateTime dtimeFinishTimePPS1, dtimeFinishTimePPS3;
            //YL��
            //DateTime dtimeFinishTimeYL1;
            //SINAB��
            DateTime dtimeFinishTimeSINAB4;
            //RR��
            DateTime dtimeFinishTimeRR1;
            //HP��
            DateTime dtimeFinishTimeHP1;
            //NXBA�¸�����
            DateTime dtimeFinishTimeNXBA1;

            StringBuilder strBuild = new StringBuilder("");

            DateTime dt = DateTime.Now;

            ArrayList arrResult = new ArrayList();
            DataRow dr;

            //�������
            int intDays = 0;

            try
            {
                //�Ϸ���
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeNF2 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeNF2 < dt.Date && intDays == 1)
                {
                    arrResult.Add("NF");
                }
                //arrResult.Add("2");
            }
            catch
            {
                arrResult.Add("�Ϸ������ӳ�ʱ��");
            }

            try
            {
                //�����
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(5, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeHJ3 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeHJ3 < dt.Date && intDays == 1)
                {
                    arrResult.Add("HJ");
                }
                //arrResult.Add("3");
            }
            catch
            {
                arrResult.Add("��������ӳ�ʱ��");
            }

            try
            {
                //���˷�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(7, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeHR4 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeHR4 < dt.Date && intDays == 1)
                {
                    arrResult.Add("HR");
                }
                //arrResult.Add("4");
            }
            catch
            {
                arrResult.Add("���˷����ӳ�ʱ��");
            }

            try
            {
                //�������˷�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(10, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeKRT8 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeKRT8 < dt.Date && intDays == 1)
                {
                    arrResult.Add("KRT");
                }
                //arrResult.Add("6");
            }
            catch
            {
                arrResult.Add("�������˷����ӳ�ʱ��");
            }

            try
            {
                //���Ϸ�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(8, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeLN7 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeLN7 < dt.Date && intDays == 1)
                {
                    arrResult.Add("LWF");
                }
                //arrResult.Add("7");
            }
            catch
            {
                arrResult.Add("���Ϸ����ӳ�ʱ��");
            }

            try
            {
                //������
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(11, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeLWF11 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeLWF11 < dt.Date && intDays == 1)
                {
                    arrResult.Add("LW");
                }
                //arrResult.Add("7");
            }
            catch
            {
                arrResult.Add("���������ӳ�ʱ��");
            }

            try
            {
                //��˳Ƿ�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(12, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeFHC12 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeFHC12 < dt.Date && intDays == 1)
                {
                    arrResult.Add("FHC");
                }
                //arrResult.Add("7");
            }
            catch
            {
                arrResult.Add("��˳Ƿ����ӳ�ʱ��");
            }

            try
            {
                //�Ʒ�����
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "CGA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeCGA3 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeCGA3 < dt.Date && intDays == 1)
                {
                    arrResult.Add("CGA3");
                }
                //arrResult.Add("8");
            }
            catch
            {
                arrResult.Add("�Ʒ��������ӳ�ʱ��");
            }

            try
            {
                //�Ʒ��ķ�
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "CGA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeCGA4 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeCGA4 < dt.Date && intDays == 1)
                {
                    arrResult.Add("CGA4");
                }
                //arrResult.Add("9");
            }
            catch
            {
                arrResult.Add("�Ʒ��ķ����ӳ�ʱ��");
            }

            try
            {
                //17173-3��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "17173"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime171733 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTime171733 < dt.Date && intDays == 1)
                {
                    arrResult.Add("17173-3");
                }
                //arrResult.Add("11");
            }
            catch
            {
                arrResult.Add("17173-3�����ӳ�ʱ��");
            }

            try
            {
                //17173-6��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(6, "17173"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime171736 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTime171736 < dt.Date && intDays == 1)
                {
                    arrResult.Add("17173-6");
                }
                //arrResult.Add("13");
            }
            catch
            {
                arrResult.Add("17173-6�����ӳ�ʱ��");
            }

            try
            {
                //51WAN4��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "51WAN"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime51WAN4 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTime51WAN4 < dt.Date && intDays == 1)
                {
                    arrResult.Add("51WAN4");
                }
                //arrResult.Add("15");
            }
            catch
            {
                arrResult.Add("51WAN4�����ӳ�ʱ��");
            }

            try
            {
                //DW1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "DW"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeDW1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeDW1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("DW1");
                }
                //arrResult.Add("16");
            }
            catch
            {
                arrResult.Add("DW1�����ӳ�ʱ��");
            }

            #region ===================== ��09������� ====================== NBA1
            //try
            //{
            //    //NBA1��
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(6, "NBA"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTimeNBA1 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];
            //
            //    if (dtimeFinishTimeNBA1 < dt.Date && intDays == 1)
            //    {
            //        arrResult.Add("NBA1");
            //    }
            //    //arrResult.Add("17");
            //}
            //catch
            //{
            //    arrResult.Add("NBA1�����ӳ�ʱ��");
            //}
            #endregion =========================================================

            try
            {
                //NBA2��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "NBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeNBA2 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeNBA2 < dt.Date && intDays == 1)
                {
                    arrResult.Add("NBA2");
                }
                //arrResult.Add("18");
            }
            catch
            {
                arrResult.Add("NBA2�����ӳ�ʱ��");
            }

            try
            {
                //NBA5��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(5, "NBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeNBA5 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeNBA5 < dt.Date && intDays == 1)
                {
                    arrResult.Add("NBA5");
                }
                //arrResult.Add("19");
            }
            catch
            {
                arrResult.Add("NBA5�����ӳ�ʱ��");
            }

            try
            {
                //SINA1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeSINA1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeSINA1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("SINA1");
                }
                //arrResult.Add("21");
            }
            catch
            {
                arrResult.Add("SINA1�����ӳ�ʱ��");
            }

            try
            {
                //SINA3��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "SINA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeSINA3 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeSINA3 < dt.Date && intDays == 1)
                {
                    arrResult.Add("SINA3");
                }
                //arrResult.Add("22");
            }
            catch
            {
                arrResult.Add("SINA3�����ӳ�ʱ��");
            }

            #region ===================== ��09������� ====================== XiaoI
            //try
            //{
            //    //XiaoI1��
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "MSN"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTimeXiaoI1 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];
            //
            //    if (dtimeFinishTimeXiaoI1 < dt.Date && intDays == 1)
            //    {
            //        arrResult.Add("XiaoI1");
            //    }
            //    //arrResult.Add("23");
            //}
            //catch
            //{
            //    arrResult.Add("XiaoI1�����ӳ�ʱ��");
            //}
            #endregion =========================================================

            try
            {
                //CGC1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "CGC"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeCGC1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeCGC1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("CGC1");
                }
                //arrResult.Add("25");
            }
            catch
            {
                arrResult.Add("CGC1�����ӳ�ʱ��");
            }

            #region ===================== ��09������� ====================== KX1
            //try
            //{
            //    //KX1��
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "KX"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTimeKX1 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];
            //
            //    if (dtimeFinishTimeKX1 < dt.Date)
            //    {
            //        arrResult.Add("KX1");
            //    }
            //    //arrResult.Add("28");
            //}
            //catch
            //{
            //    arrResult.Add("KX1�����ӳ�ʱ��");
            //}
            #endregion =========================================================

            try
            {
                //SOHU1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(51, "17173"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeSOHU1 = (DateTime)dr["FinishTime"];
                if (dtimeFinishTimeSOHU1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("SOHU1");
                }
                //arrResult.Add("29");
            }
            catch
            {
                arrResult.Add("SOHU1�����ӳ�ʱ��");
            }

            #region ===================== ��09������� ====================== 5S1
            //try
            //{
            //    //5S1��
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "5S"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTime5S1 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];
            //
            //    if (dtimeFinishTime5S1 < dt.Date && intDays == 1)
            //    {
            //        arrResult.Add("5S1");
            //    }
            //    //arrResult.Add("30");
            //}
            //catch
            //{
            //    arrResult.Add("5S1�����ӳ�ʱ��");
            //}
            #endregion =========================================================

            #region ===================== ��09������� ====================== TW1 TW2
            //try
            //{
            //    //TW1��
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TW"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTimeTW1 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];
            //
            //    if (dtimeFinishTimeTW1 < dt.Date && intDays == 1)
            //    {
            //        arrResult.Add("TW1");
            //    }
            //    //arrResult.Add("31");
            //}
            //catch
            //{
            //    arrResult.Add("TW1�����ӳ�ʱ��");
            //}

            //try
            //{
            //    //TW2��
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "TW"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTimeTW2 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];
            //
            //    if (dtimeFinishTimeTW2 < dt.Date && intDays == 1)
            //    {
            //        arrResult.Add("TW2");
            //    }
            //    //arrResult.Add("32");
            //}
            //catch
            //{
            //    arrResult.Add("TW2�����ӳ�ʱ��");
            //}
            #endregion =========================================================

            try
            {
                //PPS1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPS"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimePPS1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimePPS1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("PPS1");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("PPS1�����ӳ�ʱ��");
            }

            try
            {
                //PPS3��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "PPS"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimePPS3 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimePPS3 < dt.Date && intDays == 1)
                {
                    arrResult.Add("PPS3");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("PPS3�����ӳ�ʱ��");
            }

            #region ===================== 09����������޸������ݿ��ֶ� ====================== YL1
            //try
            //{
            //    //YL1��
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "YL"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTimeYL1 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];

            //    if (dtimeFinishTimeYL1 < dt.Date && intDays == 1)
            //    {
            //        arrResult.Add("YL1");
            //    }
            //    //arrResult.Add("32");
            //}
            //catch
            //{
            //    arrResult.Add("YL1�����ӳ�ʱ��");
            //}
            #endregion =========================================================

            try
            {
                //SINAB1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "SINA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeSINAB4 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeSINAB4 < dt.Date && intDays == 1)
                {
                    arrResult.Add("NBstra1");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("NBstra1�����ӳ�ʱ��");
            }

            try
            {
                //RR1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "RR"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeRR1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeRR1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("RR1");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("RR1�����ӳ�ʱ��");
            }

            try
            {
                //HP1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "HP"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeHP1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeHP1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("HP1");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("HP1�����ӳ�ʱ��");
            }

            try
            {
                //NXBA1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "NXBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeNXBA1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeNXBA1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("NXBA1");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("NXBA1�����ӳ�ʱ��");
            }

            foreach (string i in arrResult)
            {
                strBuild.Append(i.ToString() + "\\n");
                //Console.WriteLine(i);
            }

            if (strBuild.Length > 0)
            {
                strBuild.Append(string.Format("{0:G}", dt));
                return strBuild.ToString();
            }
            return strBuild.Append(false).ToString();
        }
        #endregion

        #region ��ȡ����ҹ�����״̬ GetFootBallUpdateNightCheck()
        /// <summary>
        /// ��ȡ����ҹ�����״̬
        /// </summary>
        /// <returns></returns>
        public static string GetFootBallUpdateNightCheck()
        {
            //�ٷ���
            DateTime dtimeFinishTimeFB4;
            //������
            DateTime dtimeFinishTimeDW1;
            //17173��
            DateTime dtimeFinishTime171731;
            //�´���
            //DateTime dtimeFinishTimeNBA1;
            //�г���
            DateTime dtimeFinishTimeCSL1;
            //��̳��
            DateTime dtimeFinishTimeTT1, dtimeFinishTimeTT2;
            //ESL��
            DateTime dtimeFinishTimeESL1;
            //PPSF��
            DateTime dtimeFinishTimePPSF1;
            //MOP��
            DateTime dtimeFinishTimeMOPF1;
            //TY������
            DateTime dtimeFinishTimeTYF1;
            //XBAF2�ٷ�����2
            DateTime dtimeFinishTimeXBA2FB1, dtimeFinishTimeXBA2FB2;
            //PPS2F PPS����2
            DateTime dtimeFinishTimePPS2F1;
            //SINA2F SINA����2
            DateTime dtimeFinishTimeSINA2F1;
            //PPL PPL����2
            DateTime dtimeFinishTimePPLF1;
            //SOHUF SOHUF����2
            DateTime dtimeFinishTimeSOHUF1;
            //TWF ̨������
            DateTime dtimeFinishTimeTWF1;
            //XBAF3�ٷ�����3
            DateTime dtimeFinishTimeXBA3FB1, dtimeFinishTimeXBA3FB3, dtimeFinishTimeXBA3FB4;
            //7K7K����
            DateTime dtimeFinishTime7K7K3FB1;
            //YXY����
            DateTime dtimeFinishTimeYXY1FB1;

            StringBuilder strBuild = new StringBuilder("");

            DateTime dt = DateTime.Now;

            ArrayList arrResult = new ArrayList();
            DataRow dr;

            //�������
            byte byteDays = 0;

            try
            {
                //FB4��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "XBAF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeFB4 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeFB4 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("FB-4");
                }
            }
            catch
            {
                arrResult.Add("FB-4�����ӳ�ʱ��");
            }

            try
            {
                //DWF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "DWF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeDW1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeDW1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("DW-F1");
                }
            }
            catch
            {
                arrResult.Add("DW-F1�����ӳ�ʱ��");
            }

            try
            {
                //17173F1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "17173F"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime171731 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTime171731 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("17173-F1");
                }
            }
            catch
            {
                arrResult.Add("17173-F1�����ӳ�ʱ��");
            }

            //try
            //{
            //    //NBAF1��
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "NBAF"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTimeNBA1 = (DateTime)dr["FinishTime"];
            //    byteDays = (byte)dr["Days"];

            //    if (dtimeFinishTimeNBA1 < dt.Date && byteDays == 1)
            //    {
            //        arrResult.Add("NBA-F1");
            //    }
            //}
            //catch
            //{
            //    arrResult.Add("NBA-F1�����ӳ�ʱ��");
            //}

            try
            {
                //CSLF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINAF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeCSL1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeCSL1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("CSL-F1");
                }
            }
            catch
            {
                arrResult.Add("CSL-F1�����ӳ�ʱ��");
            }

            try
            {
                //TT1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TT"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeTT1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeTT1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("TT-F1");
                }
            }
            catch
            {
                arrResult.Add("TT-F1�����ӳ�ʱ��");
            }

            try
            {
                //TT2��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "TT"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeTT2 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeTT2 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("TT-F2");
                }
            }
            catch
            {
                arrResult.Add("TT-F2�����ӳ�ʱ��");
            }

            try
            {
                //ESL1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "ESL"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeESL1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeESL1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("ESL-F1");
                }
            }
            catch
            {
                arrResult.Add("ESL-F1�����ӳ�ʱ��");
            }

            try
            {
                //PPSF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPSF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimePPSF1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimePPSF1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("PPSF-F1");
                }
            }
            catch
            {
                arrResult.Add("PPSF-F1�����ӳ�ʱ��");
            }

            try
            {
                //MOPF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "MOP"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeMOPF1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeMOPF1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("MOP-F1");
                }
            }
            catch
            {
                arrResult.Add("MOP-F1�����ӳ�ʱ��");
            }

            try
            {
                //TYF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TY"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeTYF1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeTYF1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("TY-F1");
                }
            }
            catch
            {
                arrResult.Add("TY-F1�����ӳ�ʱ��");
            }

            try
            {
                //XBA2FB1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "XBAF2"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeXBA2FB1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeXBA2FB1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("XBA2FB1");
                }
            }
            catch
            {
                arrResult.Add("XBA2FB1�����ӳ�ʱ��");
            }

            try
            {
                //PPS2F1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPS2F"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimePPS2F1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimePPS2F1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("PPS2F1");
                }
            }
            catch
            {
                arrResult.Add("PPS2F1�����ӳ�ʱ��");
            }

            try
            {
                //SINA2F1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINA2F"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeSINA2F1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeSINA2F1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("SINA2F1");
                }
            }
            catch
            {
                arrResult.Add("SINA2F1�����ӳ�ʱ��");
            }

            try
            {
                //PPLF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPL"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimePPLF1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimePPLF1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("PPLF1");
                }
            }
            catch
            {
                arrResult.Add("PPLF1�����ӳ�ʱ��");
            }

            try
            {
                //SOHUF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SOHUF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeSOHUF1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeSOHUF1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("SOHUF1");
                }
            }
            catch
            {
                arrResult.Add("SOHUF1�����ӳ�ʱ��");
            }

            try
            {
                //XBA2FB2��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "XBAF2"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeXBA2FB2 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeXBA2FB2 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("XBA2FB2");
                }
            }
            catch
            {
                arrResult.Add("XBA2FB2�����ӳ�ʱ��");
            }

            try
            {
                //TWF1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TWF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeTWF1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeTWF1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("TWF1");
                }
            }
            catch
            {
                arrResult.Add("TWF1�����ӳ�ʱ��");
            }

            try
            {
                //XBAF3FB1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "XBAF3"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeXBA3FB1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeXBA3FB1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("XBA3FB1");
                }
            }
            catch
            {
                arrResult.Add("XBA3FB1�����ӳ�ʱ��");
            }

            try
            {
                //XBAF3FB3��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "XBAF3"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeXBA3FB3 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeXBA3FB3 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("XBA3FB3");
                }
            }
            catch
            {
                arrResult.Add("XBA3FB3�����ӳ�ʱ��");
            }

            try
            {
                //XBAF3FB3��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "XBAF3"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeXBA3FB4 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeXBA3FB4 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("XBA3FB4");
                }
            }
            catch
            {
                arrResult.Add("XBA3FB4�����ӳ�ʱ��");
            }

            try
            {
                //7K7K��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "7K7K"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime7K7K3FB1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTime7K7K3FB1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("7K7K3FB1");
                }
            }
            catch
            {
                arrResult.Add("7K7K3FB1�����ӳ�ʱ��");
            }

            try
            {
                //YXY1FB1��
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "YXY"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeYXY1FB1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeYXY1FB1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("YXY1FB1");
                }
            }
            catch
            {
                arrResult.Add("YXY1FB1�����ӳ�ʱ��");
            }

            foreach (string i in arrResult)
            {
                strBuild.Append(i.ToString() + "\\n");
                //Console.WriteLine(i);
            }

            //Console.ReadLine();
            if (strBuild.Length > 0)
            {
                strBuild.Append(string.Format("{0:G}", dt));
                return strBuild.ToString();
            }
            return strBuild.Append(false).ToString();
        }
        #endregion

        #region ���������Ԥ������  AddServerEW(string strSID,string strServerName,string strServerStatus)
        /// <summary>
        /// ���������Ԥ������
        /// </summary>
        /// <param name="strSID">�����û�SID</param>
        /// <param name="ServerName">����������</param>
        /// <param name="ServerStatus">������״̬</param>
        public static void AddServerEW(string strSID,string strServerName,string strServerStatus)
        {
            DateTime dt = DateTime.Now;
            if (strServerStatus == "down")
            {
                CreateTxt(strSID, strServerName + " ���������⣬���飡" + "(" + dt + ")", 1);
            }
            else
            {
                CreateTxt(strSID, strServerName + " ���ָ�������" + "(" + dt + ")", 1);
            }
        }
        #endregion

        #region Google���߷���  GoogleTranslate(string word)
        /// <summary>
        /// Google���߷���
        /// </summary>
        /// <param name="word">��ѯ����</param>
        /// <returns></returns>
        public static string GoogleTranslate(string word)
        {
            string url = @"http://translate.google.cn/translate_t?langpair=en|zh-CN&text=" + word + "#";
            WebRequest req = WebRequest.Create(url);
            WebResponse res = req.GetResponse();
            Stream s = res.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.Default);
            char[] cs = new char[1024];
            string str = sr.ReadToEnd();
            int i = str.IndexOf("<div id=result_box dir=\"ltr\">");
            int j = str.IndexOf("</div>", i + 29);
            string result = str.Substring(i + 29, j - i - 29);
            return result;
        }
        #endregion

        #region POST����HTTP��ʽ  PostModel(string strUrl, string strParm)
        /// <summary>
        /// POST����HTTP��ʽ
        /// </summary>
        /// <param name="strUrl">��ַ</param>
        /// <param name="strParm">����</param>
        /// <returns></returns>
        public static string PostModel(string strUrl, string strParm)
        {
            Encoding encode = System.Text.Encoding.Default;

            byte[] arrB = encode.GetBytes(strParm);
            string strBaseUrl = null;

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(strUrl);
            myReq.Method = "POST";
            myReq.ContentType = "application/x-www-form-urlencoded";
            myReq.ContentLength = arrB.Length;
            Stream outStream = myReq.GetRequestStream();
            outStream.Write(arrB, 0, arrB.Length);
            outStream.Close();
            WebResponse myResp = null;
            try
            {
                //����HTTP��������Ӧ
                myResp = myReq.GetResponse();
            }
            catch (Exception e)
            {
                int ii = 0;
            }

            string str = null;
            try
            {
                Stream ReceiveStream = myResp.GetResponseStream();
                StreamReader readStream = new StreamReader(ReceiveStream, System.Text.Encoding.GetEncoding("UTF-8"));
                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);

                while (count > 0)
                {
                    str += new String(read, 0, count);
                    count = readStream.Read(read, 0, 256);
                }
                readStream.Close();
                myResp.Close();
                return str;
            }
            catch
            {
                return str = "false";
            }
        }
        #endregion

        #region GET����HTTP��ʽ  GetModel(string strUrl, string strParm)
        /// <summary>
        /// GET����HTTP��ʽ
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="strParm"></param>
        /// <returns></returns>
        public static string GetModel(string strUrl, string strParm)
        {
            string strRet = null;
            try
            {
                Encoding encode = System.Text.Encoding.Default;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl + strParm);
                request.Timeout = 2000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                System.IO.Stream resStream = response.GetResponseStream();

                StreamReader readStream = new StreamReader(resStream, System.Text.Encoding.GetEncoding("UTF-8"));

                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strRet = strRet + str;
                    count = readStream.Read(read, 0, 256);
                }

                resStream.Close();
            }
            catch (Exception e)
            {
                strRet = "";
                Console.WriteLine(e);
            }
            return strRet;
        }
        #endregion
    }        
}