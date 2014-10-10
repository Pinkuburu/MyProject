using System;
using System.Data;
using System.Text;

//MessageBox.aspx
//参数
//    Type	  1、查询是否有新消息
//            2、发送新消息
//            3、显示收件箱内容
//            4、标记读取消息
//            5、标记删除消息
//            6、显示发件人图片和基本信息
//            7、意见反馈
//    Type=1&UserID=用户ID 返回json数据
//    Type=2&UserID=收件人&SendID=发件人&Sender=发件人昵称&Content=消息内容 返回json数据
//    Type=3&UserID=用户ID	返回json格式数据
//    Type=4&UserID=用户ID&ID=方法三中id字段 返回json数据
//    Type=5&UserID=用户ID&ID=方法三中id字段 返回json数据
//    Type=6&UserID=用户ID 返回json数据
//    Type=7&UserID=用户ID&Content=意见内容 返回json数据


namespace Show_Web
{
    public partial class MessageBox : System.Web.UI.Page
    {
        public string strContent;

        protected void Page_Load(object sender, EventArgs e)
        {
            int intNewMsg = 0;
            int intUserID = 0;
            int intSendID = 0;
            int intType = 0;
            int intID = 0;
            string strSender = null;
            string strContent = null;
            StringBuilder sb = new StringBuilder();

            intType = Convert.ToInt32(Request.QueryString["Type"].ToString().Trim());

            if (!intType.Equals(intType) || intType > 0)
            {                
                switch (intType)
                {                        
                    case 1: //查询是否有新消息
                        intUserID = Convert.ToInt32(Request.QueryString["UserID"].ToString().Trim());
                        intNewMsg = SqlLibrary.CheckNewMessage(intUserID);
                        if (intNewMsg > 0)
                        {
                            this.strContent = "{\"NewMsg\":" + intNewMsg + "}";//"<script type='text/javascript'>NewMessage(" + intNewMsg + ");</script>";
                        }
                        else
                        {
                            this.strContent = "{\"NewMsg\":" + intNewMsg + "}";
                        }
                        break;
                    case 2: //发送新消息
                        intUserID = Convert.ToInt32(Request.QueryString["UserID"].ToString().Trim());
                        intSendID = Convert.ToInt32(Request.QueryString["SendID"].ToString().Trim());
                        strSender = Server.UrlDecode(Request.QueryString["Sender"].ToString().Trim());
                        strContent = Server.UrlDecode(Request.QueryString["Content"].ToString().Trim());
                        DataRow dr_1 = SqlLibrary.SendMessage(intUserID, intSendID, strSender, strContent);
                        intType = (int)dr_1["Type"];
                        int intCDTIME = 10 - (int)dr_1["CDTIME"];

                        if (intType == 1)
                        {
                            this.strContent = "{\"Status\":1}";//"<script type='text/javascript'>alert('新消息发送成功');</script>";
                        }
                        else
                        {
                            this.strContent = "{\"Status\":0,\"CDTime\":" + intCDTIME + "}";//"<script type='text/javascript'>alert('新消息发送失败');</script>";
                        }
                        break;
                    case 3: //显示收件箱内容
                        intUserID = Convert.ToInt32(Request.QueryString["UserID"].ToString().Trim());
                        DataTable dt = SqlLibrary.ShowMessageBox(intUserID);
                        if (dt != null)
                        {
                            sb.Append("{\"Count\":" + dt.Rows.Count + ",\"MessageBox\":[");
                            foreach (DataRow dr in dt.Rows)
                            {
                                sb.Append("{\"id\":" + (int)dr["ID"] + ",\"SendID\":" + (int)dr["SendID"] + ",\"Sender\":\"" + (string)dr["Sender"] + "\",\"Content\":\"" + (string)dr["Content"] + "\",\"Status\":\"" + (bool)dr["Status"] + "\",\"SendTime\":\"" + (DateTime)dr["SendTime"] + "\"},");
                            }
                            sb.Append("]}");
                            this.strContent = sb.ToString().Replace(",]}", "]}");
                        }
                        else
                        {
                            this.strContent = "{\"Count\":0}";
                        }
                        break;
                    case 4: //标记读取消息
                        intUserID = Convert.ToInt32(Request.QueryString["UserID"].ToString().Trim());
                        intID = Convert.ToInt32(Request.QueryString["ID"].ToString().Trim());
                        intType = SqlLibrary.ReadMessage(intUserID, intID);
                        if (intType == 1)
                        {
                            this.strContent = "{\"Status\":1}";//;"<script type='text/javascript'></script>";
                        }
                        else
                        {
                            this.strContent = "{\"Status\":0}";//;"<script type='text/javascript'></script>";
                        }
                        break;
                    case 5: //标记删除消息
                        intUserID = Convert.ToInt32(Request.QueryString["UserID"].ToString().Trim());
                        intID = Convert.ToInt32(Request.QueryString["ID"].ToString().Trim());
                        intType = SqlLibrary.DeleteMessage(intUserID, intID);
                        if (intType == 1)
                        {
                            this.strContent = "{\"Status\":1}";//;"<script type='text/javascript'></script>";
                        }
                        else
                        {
                            this.strContent = "{\"Status\":0}";//;"<script type='text/javascript'></script>";
                        }
                        break;
                    case 6: //显示发件人图片和基本信息
                        intUserID = Convert.ToInt32(Request.QueryString["UserID"].ToString().Trim());
                        DataRow dr_a = SqlLibrary.ReadUserInfoAndImg(intUserID);
                        if (dr_a != null)
                        {
                            sb.Append("{\"UserID\":" + (int)dr_a["UserID"] + ",\"NickName\":\"" + (string)dr_a["NickName"] + "\",\"Sex\":\"" + (bool)dr_a["Sex"] + "\",\"LockTime\":" + (int)dr_a["LockTime"] + ",\"Status\":\"" + (bool)dr_a["Status"] + "\",\"Birthday\":\"" + (DateTime)dr_a["Birthday"] + "\",\"Province\":\"" + (string)dr_a["Province"] + "\",\"City\":\"" + (string)dr_a["City"] + "\",\"Tinyurl\":\"" + (string)dr_a["Tinyurl"] + "\",\"Headurl\":\"" + (string)dr_a["Headurl"] + "\",\"LastActiveTime\":\"" + (DateTime)dr_a["LastActiveTime"] + "\",\"BMP\":\"" + (string)dr_a["BMP"] + "\"}");
                            this.strContent = sb.ToString();
                        }
                        break;
                    case 7: //意见反馈
                        intUserID = Convert.ToInt32(Request.QueryString["UserID"].ToString().Trim());
                        strContent = Server.UrlDecode(Request.QueryString["Content"].ToString().Trim());
                        intType = SqlLibrary.SendPropose(intUserID, strContent);

                        if (intType == 1)
                        {
                            try
                            {
                                SqlLibrary.SendQQMsg("178454109", strContent);
                            }
                            catch
                            {
                            	
                            }                            
                            this.strContent = "{\"Status\":1}";//提交成功
                        }
                        else
                        {
                            this.strContent = "{\"Status\":0}";//提交失败
                        }
                        break;
                }
            }
        }
    }
}
