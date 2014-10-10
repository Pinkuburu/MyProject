using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using databaseitem;

//=============================================
//作    者:	Cupid
//创建时间:	2009-12-10
//功能描述:	游戏服务器轮次查询
//=============================================

namespace ServerInfo
{
    public partial class Form1 : Form
    {
        public string strID = "";
        public string strSQL_1 = "SELECT Turn,Days,Status FROM BTP_Game";  //string strSQL = "Exec NewBTP.dbo.GetGameRow ";  篮球轮次存储过程
        public string strSQL_2 = "SELECT TOP 1 * FROM Game WITH(NOLOCK)";  //string strSQL = "Exec maitiam_football.dbo.GetGameRow ";   足球轮次存储过程

        public Form1()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "";
        }

        private string ServerSwitch(string strServer)
        {
            switch (strServer)
            {
                case "XBA":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "XBA").ToString();
                    break;
                case "CGA":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "CGA").ToString();
                    break;
                case "17173":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "17173").ToString();
                    break;
                case "SOHU":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "SOHU").ToString();
                    break;
                case "51WAN":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "51WAN").ToString();
                    break;
                case "DW":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "DW").ToString();
                    break;
                case "NBA":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "NBA").ToString();
                    break;
                case "SINA":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "SINA").ToString();
                    break;
                case "CGC":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "CGC").ToString();
                    break;
                case "AS":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "AS").ToString();
                    break;
                case "KX":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "KX").ToString();
                    break;
                case "TOM":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "TOM").ToString();
                    break;
                case "5S":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "5S").ToString();
                    break;
                case "MSN":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "MSN").ToString();
                    break;
                case "PPS":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "PPS").ToString();
                    break;
                case "XBAF":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "XBAF").ToString();
                    break;
                case "ASF":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "ASF").ToString();
                    break;
                case "DWF":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "DWF").ToString();
                    break;
                case "17173F":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "17173F").ToString();
                    break;
                case "51WANF":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "51WANF").ToString();
                    break;
                case "NBAF":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "NBAF").ToString();
                    break;
                case "SINAF":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "SINAF").ToString();
                    break;
                case "TT":
                    strID = DBConnection.Get30GamePhoneConnString(9999, "TT").ToString();
                    break;
                default:
                    break;
            }
            //string[] arrServerID = strID.Split(new char[] { ',' });

            //string[] arrServerID = strID.Split(new char[] { ',' });

            return strID;
            //DataTable dt_1;
            //if (strServer == "XBAF" || strServer == "ASF" || strServer == "DWF" || strServer == "17173F" || strServer == "51WANF" || strServer == "NBAF" || strServer == "SINAF" || strServer == "TT")
            //{
            //    //======================== 足球游戏登录量统计 ========================
            //    strSQL = "SELECT UserID FROM LoginIP With(Nolock) WHERE CreateTime > '" + strStart + "' AND CreateTime < '" + strEnd + "' GROUP BY UserID";

            //    for (int i = 0; i < arrServerID.Length; i++)
            //    {
            //        //string sdfsdf = DBConnection.Get30GamePhoneConnString(Convert.ToInt32(arrServerID[i].ToString()), strServer);
            //        dt_1 = SqlHelper.ExecuteDataTable(DBConnection.Get30GamePhoneConnString(Convert.ToInt32(arrServerID[i].ToString()), strServer), CommandType.Text, strSQL);

            //        foreach (DataRow dr_1 in dt_1.Rows)
            //        {
            //            DataRow dr_Add = dt.NewRow();
            //            dr_Add["UserID"] = dr_1["UserID"];
            //            dt.Rows.Add(dr_Add);
            //        }
            //    }
            //}
        }

        private void btn_Basketball_Click(object sender, EventArgs e)
        {
            string[] arrServer = { "XBA", "CGA", "17173", "SOHU", "51WAN", "DW", "NBA", "SINA", "CGC", "AS", "KX", "TOM", "5S", "MSN", "PPS" };

            DataTable dt = new DataTable("Table_GameInfo");
            dt.Columns.Add("UserID", System.Type.GetType("System.Int32"));
            //string strSQL = "Exec NewBTP.dbo.GetGameRow ";

            for(i

        }

        private void btn_Football_Click(object sender, EventArgs e)
        {

        }

                //string strcon1 = DBConnection.Get30GamePhoneConnString(1, "XBAF");
                //dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "XBAF"), CommandType.StoredProcedure, "GetGameRow");
                //intTurnFB1 = (byte)dr["Turn"];
                //intStatusFB1 = (byte)dr["Status"];
                //intDaysFB1 = (byte)dr["Days"];
                //intFinishFB1 = (bool)dr["IsFinish"];

                //string strcon1 = DBConnection.Get30GamePhoneConnString(1, "XBA");
                //dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "XBA"), CommandType.Text, strSQL);
                //intTurnN1 = (int)dr["Turn"];
                //intStatusN1 = (byte)dr["Status"];
                //intDaysN1 = (int)dr["Days"];
    }
}