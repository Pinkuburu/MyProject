using System;
using System.Text;

namespace QQRobot_InterFace_vs2010
{
    public partial class _Default : System.Web.UI.Page
    {
        string Copyright = "Cupid";              //密匙信息验证，应与机器人配置相同
        string AdminQQ = "182536608,278614660";  //管理员QQ号码，多个管理员用“ ，”隔开最后一个也要加上
        string QQ = "1349836289,1667100016";     //QQ机器人号码，多个机器人用“ ，”隔开最后一个也要加上
        string Filtration = "";                  //需要过滤群消息的QQ号码，多个QQ用“ ，”隔开最后一个也要加上
        string RobotName = "【Robot】40";        //机器人名字
        string RobotIP = "127.0.0.1";            //机器人IP
        string RobotPort = "8848";               //Api端口
        string strResult = null;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}