using System;
using System.Text;

namespace Show_Web
{
    public partial class ConcernOp : System.Web.UI.Page
    {
        public string strContent = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            int intType = 0;
            int intUserID = 0;
            int intConcernID = 0;
            int intStatus = 0;
            int intCount = 0;

            intType = Convert.ToInt32(Classlibrary.GetRequest("Type", 0));

            if (!intType.Equals(intType) || intType > 0)
            {                
                switch (intType)
                {
                    case 1: //添加关注
                        intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 0));
                        intConcernID = Convert.ToInt32(Classlibrary.GetRequest("ConcernID", 0));
                        intStatus = SqlLibrary.AddConcern(intUserID, intConcernID);

                        if (intStatus == 1)
                        {
                            sb.Append("{\"Status\":1}");    //添加成功
                            this.strContent = sb.ToString();
                        }
                        else if (intStatus == -1)
                        {
                            sb.Append("{\"Status\":-1}");    //添加失败，不是VIP
                            this.strContent = sb.ToString();
                        }
                        else
                        {
                            sb.Append("{\"Status\":0}");    //添加失败，关注已被添加
                            this.strContent = sb.ToString();
                        }
                        break;
                    case 2: //删除关注
                        intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 0));
                        intConcernID = Convert.ToInt32(Classlibrary.GetRequest("ConcernID", 0));
                        intStatus = SqlLibrary.DeleteConcern(intUserID, intConcernID);

                        if (intStatus == 1)
                        {
                            sb.Append("{\"Status\":1}");    //删除成功
                            this.strContent = sb.ToString();
                        }
                        else
                        {
                            sb.Append("{\"Status\":0}");    //删除失败，关注已被删除
                            this.strContent = sb.ToString();
                        }
                        break;
                    case 3: //统计关注数
                        intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 0));
                        intCount = SqlLibrary.CountConcern(intUserID);
                        sb.Append("{\"Count\":" + intCount + "}");
                        this.strContent = sb.ToString();
                        break;
                    case 4: //统计粉丝数
                        intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 0));
                        intCount = SqlLibrary.CountFans(intUserID);
                        sb.Append("{\"Count\":" + intCount + "}");
                        this.strContent = sb.ToString();
                        break;
                }
            }
        }
    }
}
