using System;
using System.Text;

namespace Show_Web
{
    //Modify.aspx
    //参数
    //Type
    //1: //修改密码
    //@UserID int,
    //@UserName varchar(50),
    //@Password varchar(50),
    //@NewPassword varchar(50)

    //2: //修改用户信息
    //@UserID int,
    //@UserName varchar(50),
    //@Password varchar(50),
    //@BirthDay DateTime,
    //@Province varchar(20),
    //@City varchar(20),
    //@QQ varchar(15)

    //3: //更新拍摄状态
    //@UserID int,
    //@UserName varchar(50),
    //@Password varchar(50),
    //@ShootStatus bit

    //4: //更新在线拍摄时间
    //@UserID int,
    //@UserName varchar(50),
    //@Password varchar(50)

    //返回JSON
    //{"Status\":1} 操作成功
    //{"Status\":0} 操作失败

    public partial class Modify : System.Web.UI.Page
    {
        public string strContent = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            int intType = 0;
            int intStatus = 0;

            intType = Convert.ToInt32(Classlibrary.GetRequest("Type", 0));

            if (!intType.Equals(intType) || intType > 0)
            {
                switch (intType)
                {
                    case 1: //修改密码
                        //@UserID int,
                        //@UserName varchar(50),
                        //@Password varchar(50),
                        //@NewPassword varchar(50)

                        int intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 0));
                        string strUserName = Classlibrary.GetRequest("UserName", 1).ToString();
                        string strPassword = Classlibrary.GetRequest("Password", 1).ToString();
                        string strNewPassword = Classlibrary.GetRequest("NewPassword", 1).ToString();

                        intStatus = SqlLibrary.ModifyPassword(intUserID, strUserName, strPassword, strNewPassword);
                        if (intStatus == 1)
                        {
                            sb.Append("{\"Status\":1}");    //修改成功
                            this.strContent = sb.ToString();
                        }
                        else
                        {
                            sb.Append("{\"Status\":0}");    //修改失败
                            this.strContent = sb.ToString();
                        }
                        break;
                    case 2: //修改用户信息
                        //@UserID int,
                        //@UserName varchar(50),
                        //@Password varchar(50),
                        //@BirthDay DateTime,
                        //@Province varchar(20),
                        //@City varchar(20),
                        //@QQ varchar(15)

                        intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 0));
                        strUserName = Classlibrary.GetRequest("UserName", 1).ToString();
                        strPassword = Classlibrary.GetRequest("Password", 1).ToString();
                        DateTime dtBirthDay = Convert.ToDateTime(Classlibrary.GetRequest("BirthDay", 1));
                        string strProvince = Classlibrary.GetRequest("Province", 1).ToString();
                        string strCity = Classlibrary.GetRequest("City", 1).ToString();
                        string strQQ = Classlibrary.GetRequest("QQ", 1).ToString();

                        intStatus = SqlLibrary.ModifyUserInfo(intUserID, strUserName, strPassword, dtBirthDay, strProvince, strCity, strQQ);
                        if (intStatus == 1)
                        {
                            sb.Append("{\"Status\":1}");    //修改成功
                            this.strContent = sb.ToString();
                        }
                        else
                        {
                            sb.Append("{\"Status\":0}");    //修改失败
                            this.strContent = sb.ToString();
                        }
                        break;
                    case 3: //更新拍摄状态
                        //@UserID int,
                        //@UserName varchar(50),
                        //@Password varchar(50),
                        //@ShootStatus bit

                        intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 0));
                        strUserName = Classlibrary.GetRequest("UserName", 1).ToString();
                        strPassword = Classlibrary.GetRequest("Password", 1).ToString();
                        bool blShootStatus = Convert.ToBoolean(Classlibrary.GetRequest("AutoShoot", 1).ToString());

                        intStatus = SqlLibrary.ModifyShootStatus(intUserID, strUserName, strPassword, blShootStatus);
                        if (intStatus == 1)
                        {
                            sb.Append("{\"Status\":1}");    //修改成功
                            this.strContent = sb.ToString();
                        }
                        else
                        {
                            sb.Append("{\"Status\":0}");    //修改失败
                            this.strContent = sb.ToString();
                        }
                        break;
                    case 4: //更新拍摄状态
                        //@UserID int,
                        //@UserName varchar(50),
                        //@Password varchar(50)

                        intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 0));
                        strUserName = Classlibrary.GetRequest("UserName", 1).ToString();
                        strPassword = Classlibrary.GetRequest("Password", 1).ToString();

                        intStatus = SqlLibrary.UpdateOnlineTime(intUserID, strUserName, strPassword);
                        if (intStatus == 1)
                        {
                            sb.Append("{\"Status\":1}");    //修改成功
                            this.strContent = sb.ToString();
                        }
                        else
                        {
                            sb.Append("{\"Status\":0}");    //修改失败
                            this.strContent = sb.ToString();
                        }
                        break;
                }
            }
        }
    }
}
