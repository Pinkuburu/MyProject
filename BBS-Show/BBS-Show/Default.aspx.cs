using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySQLDriverCS;
using System.Data;
using System.Text;

namespace BBS_Show
{
    public partial class _Default : System.Web.UI.Page
    {
        public string strContent = null;
        StringBuilder sb = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            string strCategory = Request.QueryString["Category"].ToString();

            MySQLConnection conn = new MySQLConnection(new MySQLConnectionString("192.168.1.250", "discuz_bbs", "cupid", "qweqwe123").AsString);
            //Response.Write("Connecting to database");
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                return;
            }

            MySQLCommand cmd = new MySQLCommand("set names gb2312", conn);
            cmd.ExecuteNonQuery();

            if (strCategory == "1")
            {
                cmd = new MySQLCommand("SELECT fid,tid,author,subject,views FROM `cdb_threads` WHERE fid = 8 ORDER BY dateline DESC LIMIT 0,5", conn);
                //cmd = new MySQLCommand("SELECT fid,tid,author,subject,views FROM `pre_forum_thread` WHERE fid = 8 ORDER BY dateline DESC LIMIT 0,7", conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();
                while (reader.Read())
                {
                    //Response.Write(string.Format("{0}---{1}---{2}---{3}<br>", reader["tid"], reader["author"], reader["subject"].ToString().Replace("??", "--"), reader["views"]));
                    sb.Append(string.Format("<a target=\"_blank\" title=\"{1}\" href=\"http://192.168.1.250/viewthread.php?tid={0}&extra=page%3D1\">{2}{1}</a>\r\n", reader["tid"], reader["subject"].ToString().Replace("??", "--"), ShowFidName(reader["fid"])));
                    //sb.Append(string.Format("<a target=\"_blank\" title=\"{1}\" href=\"http://192.168.1.250/forum.php?mod=viewthread&tid={0}&extra=page%3D1\">{2}{1}</a>\r\n", reader["tid"], reader["subject"].ToString().Replace("??", "--"), ShowFidName(reader["fid"])));
                }
                strContent = sb.ToString();
                reader.Close();
                cmd.Dispose();
                conn.Close();
            }

            if (strCategory == "11")
            {
                cmd = new MySQLCommand("SELECT fid,tid,author,subject,views FROM `cdb_threads` WHERE fid = 8 ORDER BY dateline DESC LIMIT 5,5", conn);
                //cmd = new MySQLCommand("SELECT fid,tid,author,subject,views FROM `pre_forum_thread` WHERE fid = 8 ORDER BY dateline DESC LIMIT 7,7", conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();
                while (reader.Read())
                {
                    //Response.Write(string.Format("{0}---{1}---{2}---{3}<br>", reader["tid"], reader["author"], reader["subject"].ToString().Replace("??", "--"), reader["views"]));
                    sb.Append(string.Format("<a target=\"_blank\" title=\"{1}\" href=\"http://192.168.1.250/viewthread.php?tid={0}&extra=page%3D1\">{2}{1}</a>\r\n", reader["tid"], reader["subject"].ToString().Replace("??", "--"), ShowFidName(reader["fid"])));
                }
                strContent = sb.ToString();
                reader.Close();
                cmd.Dispose();
                conn.Close();
            }

            if (strCategory == "2")
            {
                cmd = new MySQLCommand("SELECT fid,tid,author,subject,views FROM `cdb_threads` WHERE fid not in (8,16,18) ORDER BY dateline DESC LIMIT 0,7", conn);
                //cmd = new MySQLCommand("SELECT fid,tid,author,subject,views FROM `pre_forum_thread` WHERE fid not in (8,16,18) ORDER BY dateline DESC LIMIT 0,7", conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();
                while (reader.Read())
                {
                    //Response.Write(string.Format("{0}---{1}---{2}---{3}<br>", reader["tid"], reader["author"], reader["subject"].ToString().Replace("??", "--"), reader["views"]));
                    sb.Append(string.Format("<a target=\"_blank\" title=\"{1}\" href=\"http://192.168.1.250/viewthread.php?tid={0}&extra=page%3D1\">{2}{1}</a>\r\n", reader["tid"], reader["subject"].ToString().Replace("??", "--"), ShowFidName(reader["fid"])));
                }
                strContent = sb.ToString();
                reader.Close();
                cmd.Dispose();
                conn.Close();
            }

            if (strCategory == "3")
            {
                cmd = new MySQLCommand("SELECT fid,tid,author,subject,views FROM `cdb_threads` WHERE fid not in (8,16,18) ORDER BY dateline DESC LIMIT 7,7", conn);
                //cmd = new MySQLCommand("SELECT fid,tid,author,subject,views FROM `pre_forum_thread` WHERE fid not in (8,16,18) ORDER BY dateline DESC LIMIT 7,7", conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();
                while (reader.Read())
                {
                    //Response.Write(string.Format("{0}---{1}---{2}---{3}<br>", reader["tid"], reader["author"], reader["subject"].ToString().Replace("??", "--"), reader["views"]));
                    sb.Append(string.Format("<a target=\"_blank\" title=\"{1}\" href=\"http://192.168.1.250/viewthread.php?tid={0}&extra=page%3D1\">{2}{1}</a>\r\n", reader["tid"], reader["subject"].ToString().Replace("??", "--"), ShowFidName(reader["fid"])));
                }
                strContent = sb.ToString();
                reader.Close();
                cmd.Dispose();
                conn.Close();
            }

            //huodong1
            if (strCategory == "4")
            {
                cmd = new MySQLCommand("SELECT fid,tid,author,subject,views FROM `cdb_threads` WHERE fid = 18 ORDER BY dateline DESC LIMIT 0,7", conn);
                //cmd = new MySQLCommand("SELECT fid,tid,author,subject,views FROM `pre_forum_thread` WHERE fid = 18 ORDER BY dateline DESC LIMIT 0,7", conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();
                while (reader.Read())
                {
                    //Response.Write(string.Format("{0}---{1}---{2}---{3}<br>", reader["tid"], reader["author"], reader["subject"].ToString().Replace("??", "--"), reader["views"]));
                    sb.Append(string.Format("<a target=\"_blank\" title=\"{1}\" href=\"http://192.168.1.250/viewthread.php?tid={0}&extra=page%3D1\">{2}{1}</a>\r\n", reader["tid"], reader["subject"].ToString().Replace("??", "--"), ShowFidName(reader["fid"])));
                }
                strContent = sb.ToString();
                reader.Close();
                cmd.Dispose();
                conn.Close();
            }

            //huodong2
            if (strCategory == "5")
            {
                cmd = new MySQLCommand("SELECT fid,tid,author,subject,views FROM `cdb_threads` WHERE fid = 18 ORDER BY dateline DESC LIMIT 7,7", conn);
                //cmd = new MySQLCommand("SELECT fid,tid,author,subject,views FROM `pre_forum_thread` WHERE fid = 18 ORDER BY dateline DESC LIMIT 7,7", conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();
                while (reader.Read())
                {
                    //Response.Write(string.Format("{0}---{1}---{2}---{3}<br>", reader["tid"], reader["author"], reader["subject"].ToString().Replace("??", "--"), reader["views"]));
                    sb.Append(string.Format("<a target=\"_blank\" title=\"{1}\" href=\"http://192.168.1.250/viewthread.php?tid={0}&extra=page%3D1\">{2}{1}</a>\r\n", reader["tid"], reader["subject"].ToString().Replace("??", "--"), ShowFidName(reader["fid"])));
                }
                strContent = sb.ToString();
                reader.Close();
                cmd.Dispose();
                conn.Close();
            }

            //MySQLCommand commn = new MySQLCommand("set names gb2312", conn); 
            //commn.ExecuteNonQuery();
            //DataTable dt = new MySQLSelectCommand(conn, new string[] { "tid", "author", "subject", "views" }, new string[] { "cdb_threads" }, null, null, null, true, 0, 10, false).Table;
            //foreach (DataRow row in dt.Rows)
            //{
            //    Response.Write(string.Format("{0}---{1}---{2}---{3}", row["tid"], row["author"], row["subject"], row["views"]));
            //}
            //dt.Dispose();
            //commn.Dispose();
        }

        #region 论坛中文名称 ShowFidName(object objFid)
        /// <summary>
        /// 论坛中文名称
        /// </summary>
        /// <param name="objFid"></param>
        /// <returns></returns>
        private string ShowFidName(object objFid)
        {
            string strTitle = null;
            int intFid = Convert.ToInt32(objFid);
            switch (intFid)
            {
                case 2:
                    strTitle = "【综合讨论区】";
                    break;
                case 5:
                    strTitle = "【游戏设计讨论区】";
                    break;
                case 8:
                    strTitle = "【公告发布区】";
                    break;
                case 9:
                    strTitle = "【游戏评测区】";
                    break;
                case 10:
                    strTitle = "【“越策越开心”自由讨论区】";
                    break;
                case 12:
                    strTitle = "【新足球】";
                    break;
                case 13:
                    strTitle = "【RPG项目】";
                    break;
                case 14:
                    strTitle = "【市场与运营板块】";
                    break;
                case 15:
                    strTitle = "【策划BLOG】";
                    break;
                case 16:
                    strTitle = "【客户服务专版】";
                    break;
                case 18:
                    strTitle = "【活动】";
                    break;
            }

            return strTitle;
        }
        #endregion 论坛中文名称 ShowFidName(object objFid)
    }
}