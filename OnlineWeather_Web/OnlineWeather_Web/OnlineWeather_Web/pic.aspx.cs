using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Data.OleDb;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //打开总计数
        string tongji = null;
        string strCmd = "select tongji from tong Where userid='" + Request["u"].Trim() + "'";
        OleDbDataReader sdr = ExceRead(strCmd);
        if (sdr.Read())
        {
            tongji = sdr["tongji"].ToString();
        }
        sdr.Close();
        sdr.Dispose();
        //统计并更新
        DateTime dt = DateTime.Now;
        DoSql("delete from tongji where dsj < #" + DateTime.Now.AddDays(-3).ToString() + "#");
        DoSql("update tong set tongji=tongji+1 where userid='" + Request["u"].Trim() + "'");
        DoSql("insert into tongji (ip,sj,userid,dsj)values('" + Request.UserHostAddress + "','" + dt.ToShortDateString().ToString() + "','" + Request["u"].Trim() + "','" + dt + "')");
        string j = Judge_Repeat("select Count(id) As id from tongji Where userid='" + Request["u"].Trim() + "' and sj='" + dt.ToShortDateString().ToString() + "'");
        string z = Judge_Repeat("select Count(id) As id from tongji Where userid='" + Request["u"].Trim() + "' and sj='" + DateTime.Now.AddDays(-1).ToShortDateString() + "'");
        //下面是显示出计数器
        System.Drawing.Bitmap bmp = new Bitmap(Bitmap.FromFile(Server.MapPath("2.gif")));//载入图片   
        System.Drawing.Graphics g = Graphics.FromImage(bmp);
        g.DrawString("" + j + " 次", new Font("宋体", 9), new SolidBrush(Color.Red), 65, 9);
        g.DrawString("" + z + " 次", new Font("宋体", 9), new SolidBrush(Color.Red), 65, 26);
        g.DrawString("" + tongji + " 次", new Font("宋体", 9), new SolidBrush(Color.Red), 67, 42);
	
        //这里选择文本字体颜色   
        g.Dispose();
        //输出GIF,你要其它格式也可以自己改   
        Response.ContentType = "image/gif";
        bmp.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
    }
    #region 执行添加、删除、更新时使用
    /// <summary>
    /// 执行添加、删除、更新时使用
    /// </summary> 
    public void DoSql(string sql)//执行添加、删除、更新时使用
    {
        string conString = Server.MapPath("App_Data/panpao.accdb");
        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data source=" + conString;
        OleDbConnection oldCon = new OleDbConnection(conString);
        oldCon.Open();
        OleDbCommand cmd = new OleDbCommand(sql, oldCon);

        cmd.ExecuteNonQuery();//
        oldCon.Close();//关闭数据库
        oldCon.Dispose();
    }
    #endregion
    #region 返回一个SqlDataReader类型数据
    public OleDbDataReader ExceRead(string strCom)//该方法返回一个SqlDataReader类型
    {
        string conString = Server.MapPath("App_Data/panpao.accdb");
        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data source=" + conString;
        OleDbConnection oldCon = new OleDbConnection(conString);
        oldCon.Open();
        OleDbCommand sqlCmd = new OleDbCommand(strCom, oldCon);

        OleDbDataReader sdr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        return sdr;
        oldCon.Close();//关闭数据库
        oldCon.Dispose();
    }
    #endregion
    #region 执行数据库返回一个值
    public string Judge_Repeat(string sql_str)
    {                                                                      //判断表中返回的数量
        string conString = Server.MapPath("App_Data/panpao.accdb");
        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data source=" + conString;
        OleDbConnection oldCon = new OleDbConnection(conString);
        oldCon.Open();
        OleDbCommand Command = new OleDbCommand(sql_str, oldCon);
        OleDbDataReader Dr = Command.ExecuteReader(CommandBehavior.CloseConnection);
        try
        {
            Dr.Read();
            return Dr[0].ToString();
        }
        catch (Exception err)
        {
            return "";
        }
        finally
        {
            Dr.Close();
            Dr.Dispose();
            oldCon.Close();//关闭数据库
            oldCon.Dispose();
        }
    }
    #endregion
}
