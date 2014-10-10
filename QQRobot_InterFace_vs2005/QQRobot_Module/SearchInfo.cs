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
                strContent = "用户名：" + (string)dr["UserName"] + "\r\n";
                strContent += "密码：" + (string)dr["Password"] + "\r\n";
                strContent += "金币：" + (long)dr["Coin"] + "\r\n";
                strContent += "邮箱：" + (string)dr["Email"] + "\r\n";
                strContent += "经理名：" + (string)dr["NickName"] + "\r\n";
                strContent += "生日：" + Convert.ToString((DateTime)dr["Birthday"]) + "\r\n";
                strContent += (string)dr["Province"] + "省" + (string)dr["Province"] + "市" + "\r\n";
                strContent += "注册时间：" + Convert.ToString((DateTime)dr["CreateTime"]) + "\r\n";
                strContent += "最后登录：" + Convert.ToString((DateTime)dr["ActiveTime"]) + "\r\n";
                strContent += "QQ：" + (string)dr["QQ"] + "\r\n";
                strContent += "所在大区：" + (string)dr["Depttag"] + "\r\n";
                strContent += "真实姓名：" + (string)dr["RealName"] + "\r\n";
                strContent += "身份证：" + (string)dr["CardID"] + "\r\n";

                strSQL = "SELECT ISNULL(SUM(Count),0) AS Count,ISNULL(SUM(Price),0)AS Price FROM Main_PayOrder WHERE status=1 AND UserID = " + intUserID;
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(40, "XBA"), CommandType.Text, strSQL);
                strContent += "充值总额：" + (int)dr["Count"] + "金币 " + (int)dr["Price"] + "人民币\r\n";

                strSQL = "SELECT ISNULL(SUM(Outcome),0) AS Outcome FROM Main_CoinFinance WHERE Outcome>0 AND Event NOT LIKE '%交易给%金币' AND Event NOT Like '%赠送给%金币' AND UserID = " + intUserID;
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(40, "XBA"), CommandType.Text, strSQL);
                strContent += "消费总额：" + (long)dr["Outcome"];
            }
            else
            {
                strContent = "查无此用户！";
            }

            return strContent;
        }
    }
}
