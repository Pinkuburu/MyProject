using System;
using System.Data;

namespace Show_Web
{
    public partial class Index : System.Web.UI.Page
    {
        public int intCount = 0;
        public int intOnlineCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string strSQL = "SELECT COUNT(*) AS Count FROM MT_User WITH(NOLOCK)";
            DataRow dr = SqlHelper.ExecuteDataRow(SqlLibrary.GetServer_Main(), CommandType.Text, strSQL);
            intCount = (int)dr["Count"];
            strSQL = "SELECT COUNT(*) AS Count FROM MT_Media WITH(NOLOCK) WHERE OnLine = 1";
            dr = SqlHelper.ExecuteDataRow(SqlLibrary.GetServer_Main(), CommandType.Text, strSQL);
            intOnlineCount = (int)dr["Count"];
        }
    }
}
