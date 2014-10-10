using System;
using System.Text;
using System.Data;

namespace Show_Web
{
    public partial class Page : System.Web.UI.Page
    {
        public string strContent = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            int intType = 0;
            int intCount = 0;
            int intUserID = 0;
            string strSQL = null;
            string strProvince = null;
            string strCity = null;
            string strGender = null;
            string strCategory = null;
            
            intType = Convert.ToInt32(Classlibrary.GetRequest("Type", 0));

            if (!intType.Equals(intType) || intType > 0)
            {
                switch (intType)
                {
                    case 1://大厅分页
                        strSQL = "SELECT ISNULL(Count(id),1) AS Count FROM [MT_Media] WITH(NOLOCK)";
                        intCount = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlLibrary.GetServer_Main(), CommandType.Text, strSQL));
                        if (intCount % 8 > 0)
                        {
                            intCount = intCount / 8 + 1;
                        }
                        else
                        {
                            intCount = intCount / 8;
                        }
                        this.strContent = "{\"Page\":" + intCount + "}";
                        break;
                    case 2://关注分页
                        intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 1));
                        //intUserID = Convert.ToInt32(Session["UserID"]);
                        DataTable dt = SqlLibrary.ReadConcernList(intUserID);
                        if (dt != null)
                        {
                            intCount = dt.Rows.Count;
                            if (intCount % 8 > 0)
                            {
                                intCount = intCount / 8 + 1;
                            }
                            else
                            {
                                intCount = intCount / 8;
                            }
                            this.strContent = "{\"Page\":" + intCount + "}";
                        }
                        else
                        {
                            this.strContent = "{\"Page\":0}";
                        }
                        break;
                    case 3://粉丝分页
                        intUserID = Convert.ToInt32(Classlibrary.GetRequest("UserID", 1));
                        intCount = SqlLibrary.CountFans(intUserID);
                        if (intCount > 0)
                        {
                            if (intCount % 8 > 0)
                            {
                                intCount = intCount / 8 + 1;
                            }
                            else
                            {
                                intCount = intCount / 8;
                            }
                            this.strContent = "{\"Page\":" + intCount + "}";
                        }
                        else
                        {
                            this.strContent = "{\"Page\":0}";
                        }
                        break;
                    case 4://搜索分页
                        strProvince = Classlibrary.GetRequest("Province", 1).ToString();
                        strCity = Classlibrary.GetRequest("City", 1).ToString();
                        strGender = Classlibrary.GetRequest("Gender", 1).ToString();
                        strCategory = Classlibrary.GetRequest("Category", 1).ToString();
                        intCount = SqlLibrary.SearchUserListCount(strProvince, strCity, strGender, strCategory);
                        if (intCount > 0)
                        {
                            if (intCount % 8 > 0)
                            {
                                intCount = intCount / 8 + 1;
                            }
                            else
                            {
                                intCount = intCount / 8;
                            }
                            this.strContent = "{\"Page\":" + intCount + "}";
                        }
                        else
                        {
                            this.strContent = "{\"Page\":0}";
                        }
                        break;
                }
            }
        }
    }
}
