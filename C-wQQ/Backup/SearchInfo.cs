using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using databaseitem;

namespace QQRobot_Module
{
    public class SearchInfo
    {
        public string UserInfo(string strParameter, string strName)
        {
            string strContent = null;
            string strSQL = null;
            string strNickName = null;
            int intUserID = 0;
            DataRow dr;

            if (strParameter == "0")
            {
                strSQL = "SELECT UserID FROM Main_User WHERE NickName = '" + strName + "'";
            }
            else
            {
                strSQL = "SELECT UserID FROM Main_User WHERE NickName = '" + strName + "'";
            }

            dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(40, "XBA"), CommandType.Text, strSQL);
            if (dr != null)
            {
                intUserID = (int)dr["UserID"];
                strSQL = "SELECT UserName,Password,Coin,Email,NickName,Birthday,Province,City,CreateTime,ActiveTime,QQ,Depttag,RealName,CardID FROM Main_User WHERE UserID = " + intUserID;
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(40, "XBA"), CommandType.Text, strSQL);
                strContent = "�û�����" + (string)dr["UserName"] + "\r\n";
                strContent += "���룺" + (string)dr["Password"] + "\r\n";
                strContent += "��ң�" + (long)dr["Coin"] + "\r\n";
                strContent += "���䣺" + (string)dr["Email"] + "\r\n";
                strContent += "��������" + (string)dr["NickName"] + "\r\n";
                strContent += "���գ�" + Convert.ToString((DateTime)dr["Birthday"]) + "\r\n";
                strContent += (string)dr["Province"] + "ʡ" + (string)dr["Province"] + "��" + "\r\n";
                strContent += "ע��ʱ�䣺" + Convert.ToString((DateTime)dr["CreateTime"]) + "\r\n";
                strContent += "����¼��" + Convert.ToString((DateTime)dr["ActiveTime"]) + "\r\n";
                strContent += "QQ��" + (string)dr["QQ"] + "\r\n";
                strContent += "���ڴ�����" + (string)dr["Depttag"] + "\r\n";
                strContent += "��ʵ������" + (string)dr["RealName"] + "\r\n";
                strContent += "���֤��" + (string)dr["CardID"] + "\r\n";

                strSQL = "SELECT ISNULL(SUM(Count),0) AS Count,ISNULL(SUM(Price),0)AS Price FROM Main_PayOrder WHERE status=1 AND UserID = " + intUserID;
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(40, "XBA"), CommandType.Text, strSQL);
                strContent += "��ֵ�ܶ" + (int)dr["Count"] + "��� " + (int)dr["Price"] + "�����\r\n";

                strSQL = "SELECT ISNULL(SUM(Outcome),0) AS Outcome FROM Main_CoinFinance WHERE Outcome>0 AND Event NOT LIKE '%���׸�%���' AND Event NOT Like '%���͸�%���' AND UserID = " + intUserID;
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(40, "XBA"), CommandType.Text, strSQL);
                strContent += "�����ܶ" + (long)dr["Outcome"];
            }
            else
            {
                strContent = "���޴��û���";
            }

            return strContent;
        }
    }
}
