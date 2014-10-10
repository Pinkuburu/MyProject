using System;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Show_Web
{
    public partial class RegOp : System.Web.UI.Page
    {
        public string strOut = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string strType = Classlibrary.GetRequest("Type", 1).ToString();
            string strUserName = "", strPassword = "", strRePassword = "", strEmail = "", strNickname = "", strGender = "",
                        strYear = "", strMonth = "", strDay = "", strProvince = "", strCity = "", strIntro = "", strQQ = "", strMSN = "",
                        strVCode = "", strCardId = "", strRealName = "", strLVcode = "", strRecentServer = "", strRecentLoginTime = "", strStatus = "", strZStatus = "";
            int intUserID = 0;
            string ecUserName = "";
            string strEncrypt = "";
            string strResult = "";
            string strSxUserName = "";
            string strSxUserID = "";
            string strSxEncrypt = "";
            string strRPGResult = "";
            bool blncbReg = false;
            switch (strType)
            {
                #region 注册
                case "Reg":
                    strUserName = Classlibrary.GetRequest("UserName", 1).ToString();
                    strPassword = Classlibrary.GetRequest("Password", 1).ToString();
                    strRePassword = Classlibrary.GetRequest("RePassword", 1).ToString();
                    //strEmail = Classlibrary.GetRequest("Email", 1).ToString();
                    //strNickname = Server.UrlDecode(Server.UrlEncode(Classlibrary.GetRequest("Nickname", 1).ToString()));
                    //strNickname = HttpUtility.UrlDecode(Classlibrary.GetRequest("Nickname", 1).ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                    string strURL = Request.Url.ToString().Trim();
                    int intIndex = strURL.IndexOf("Nickname=");
                    int intIndexN = strURL.IndexOf("&Gender=");
                    strNickname = strURL.Substring(intIndex + 9, intIndexN - intIndex - 9);

                    strGender = Classlibrary.GetRequest("Gender", 1).ToString();
                    strYear = Classlibrary.GetRequest("Year", 1).ToString();
                    strMonth = Classlibrary.GetRequest("Month", 1).ToString();
                    strDay = Classlibrary.GetRequest("Day", 1).ToString();
                    strProvince = Classlibrary.GetRequest("Province", 1).ToString();
                    strCity = Classlibrary.GetRequest("City", 1).ToString();
                    //strIntro = Classlibrary.GetRequest("Intro", 1).ToString();
                    //strQQ = Classlibrary.GetRequest("QQ", 1).ToString();
                    //strMSN = Classlibrary.GetRequest("MSN", 1).ToString();
                    strVCode = Classlibrary.GetRequest("VCode", 1).ToString();
                    //strCardId = Classlibrary.GetRequest("CardId", 1).ToString();
                    //strRealName = Classlibrary.GetRequest("RealName", 1).ToString();
                    //blncbReg = Convert.ToBoolean(Classlibrary.GetRequest("cbReg", 1));

                    //this.strOut =this.AlertMe(false,  "经理名=."strNickname);
                    //获取类型


                    //int intReturn = this.DoIt(strUserName, strPassword, strRePassword, strEmail, strNickname, strGender, strYear, strMonth, strDay, strProvince, strCity, strIntro, strQQ, strMSN, blncbReg, strVCode, strCardId, strRealName);
                    int intReturn = this.DoIt(strUserName, strPassword, strRePassword, strNickname, strGender, strYear, strMonth, strDay, strProvince, strCity, strVCode, strQQ);
                    //this.strOut = intReturn.ToString().Trim();
                    if (intReturn == 1)
                    {
                        this.strOut = this.AlertMe(false, "用户名已存在，请重新填写.");
                    }
                    else if (intReturn == 2)
                    {
                        this.strOut = this.AlertMe(false, "昵称已存在，请重新填写.");
                    }
                    else if (intReturn == 3)
                    {
                        this.strOut = this.AlertMe(false, "Email已存在，请重新填写.");
                    }
                    else if (intReturn == -1)
                    {
                        this.strOut = this.AlertMe(false, "两次密码填写不匹配，请认真填写.");
                    }
                    else if (intReturn == -2)
                    {
                        this.strOut = this.AlertMe(false, "请选择正确的出生年.");
                    }
                    else if (intReturn == -3)
                    {
                        this.strOut = this.AlertMe(false, "请选择正确的出生月.");
                    }
                    else if (intReturn == -4)
                    {
                        this.strOut = this.AlertMe(false, "请选择正确的出生月.");
                    }
                    else if (intReturn == -5)
                    {
                        this.strOut = this.AlertMe(false, "请选择正确的省份.");
                    }
                    else if (intReturn == -6)
                    {
                        this.strOut = this.AlertMe(false, "请选择正确的城市.");
                    }
                    else if (intReturn == -7)
                    {
                        this.strOut = this.AlertMe(false, "请确认您已经仔细阅读并同意注册协议.");
                    }
                    else if (intReturn == -8)
                    {
                        this.strOut = this.AlertMe(false, "请确定推荐人是否存在.");
                    }
                    else if (intReturn == -9)
                    {
                        this.strOut = this.AlertMe(false, "QQ填写错误,请重新填写.");
                    }
                    else if (intReturn == -10)
                    {
                        this.strOut = this.AlertMe(false, "MSN填写错误,请重新填写.");
                    }
                    else if (intReturn == -11)
                    {
                        this.strOut = this.AlertMe(false, "出现错误,请重新填写.");
                    }
                    else if (intReturn == -12)
                    {
                        this.strOut = this.AlertMe(false, "用户名不能为空,请重新填写.");
                    }
                    else if (intReturn == -13)
                    {
                        this.strOut = this.AlertMe(false, "密码或确认密码不能为空,请重新填写.");
                    }
                    else if (intReturn == -14)
                    {
                        this.strOut = this.AlertMe(false, "昵称不能为空,请重新填写.");
                    }
                    else if (intReturn == -15)
                    {
                        this.strOut = this.AlertMe(false, "Email不能为空,请重新填写.");
                    }
                    else if (intReturn == -16)
                    {
                        this.strOut = this.AlertMe(false, "用户名填写错误,请重新填写.");
                    }
                    else if (intReturn == -17)
                    {
                        this.strOut = this.AlertMe(false, "昵称填写不符合规则，请重新填写.");
                    }
                    else if (intReturn == -19)
                    {
                        this.strOut = this.AlertMe(false, "昵称填写不符合规则，请重新填写.");
                    }
                    else if (intReturn == -20)
                    {
                        this.strOut = this.AlertMe(false, "密码填写错误,请重新填写.");
                    }
                    else if (intReturn == -22)
                    {
                        this.strOut = this.AlertMe(false, "您的浏览器已禁用 Cookies,请启用.");
                    }
                    else if (intReturn == -23)
                    {
                        this.strOut = this.AlertMe(false, "验证码输入错误,请输入正确的验证码！");
                    }
                    else if (intReturn == -24)
                    {
                        this.strOut = this.AlertMe(false, "验证码不能为空.");
                    }
                    else if (intReturn == -25)
                    {
                        this.strOut = this.AlertMe(false, "省份选择出现错误,请重新选择.");
                    }
                    else if (intReturn == -26)
                    {
                        this.strOut = this.AlertMe(false, "城市选择出现错误,请重新选择.");
                    }
                    else if (intReturn == 0)
                    {
                        this.strOut = this.AlertMe(false, "注册成功");
                    }
                    else if (intReturn == -48)
                    {
                        this.strOut = this.AlertMe(false, "无身份信息将被列入防沉迷,连续游戏3小时将退出.");
                    }
                    else if (intReturn == -44)
                    {
                        this.strOut = this.AlertMe(false, "身份证号码为15位或者18位数字,请正确输入.");
                    }
                    else if (intReturn == -40)
                    {
                        this.strOut = this.AlertMe(false, "身份证号码为15位或者18位数字,请正确输入.");
                    }
                    else if (intReturn == -41)
                    {
                        this.strOut = this.AlertMe(false, "填写的身份证号非法,请重新填写.");
                    }
                    else if (intReturn == -42)
                    {
                        this.strOut = this.AlertMe(false, "填写的身份证号非法,请重新填写.");
                    }
                    else if (intReturn == -43)
                    {
                        this.strOut = this.AlertMe(false, "填写的身份证号非法,请重新填写.");
                    }
                    else if (intReturn == -46)
                    {
                        this.strOut = this.AlertMe(false, "身份证号码与所选生日不符,请检查.");
                    }
                    else if (intReturn == -47)
                    {
                        this.strOut = this.AlertMe(false, "身份证号码与所选性别不符,请检查.");
                    }
                    else if (intReturn == -49)
                    {
                        this.strOut = this.AlertMe(false, "真实姓名不能为空,请认真填写.");
                    }
                    else if (intReturn == -51)
                    {
                        this.strOut = this.AlertMe(false, "身份证号不能为空,请认真填写.");
                    }
                    else if (intReturn == -50)
                    {
                        this.strOut = this.AlertMe(false, "您的真实姓名不符合规则，请重新填写.");
                    }
                    else if (intReturn == -61)
                    {
                        this.strOut = this.AlertMe(false, "输入的15位身份证号码不在规定范围内，请重新填写");
                    }
                    else if (intReturn == -71)
                    {
                        this.strOut = this.AlertMe(false, "输入的身份证号码不符合规则，请重新填写");
                    }
                    else if (intReturn == -62)
                    {
                        this.strOut = this.AlertMe(false, "输入的身份证号码不在规定范围内，请重新填写.");
                    }
                    else if (intReturn == -63)
                    {
                        this.strOut = this.AlertMe(false, "QQ号码只能是数字,请重新填写.");
                    }
                    else
                    {
                        this.strOut = this.AlertMe(false, "填写内容不充分，请确认后提交.");
                    }
                    break;
                #endregion
                #region 检测用户名
                case "UserName":
                    strUserName = Classlibrary.GetRequest("UserName", 1).ToString();
                    this.strOut = this.HasUserName(strUserName).ToString();
                    break;
                #endregion
                #region 检测经理名
                case "Nickname":
                    //strNickname = Classlibrary.GetRequest("Nickname", 1).ToString();
                    string strNURL = Request.Url.ToString().Trim();
                    int intNIndex = strNURL.IndexOf("Nickname=");
                    int intNIndexN = strNURL.IndexOf("&timeStamp=");
                    strNickname = strNURL.Substring(intNIndex + 9, intNIndexN - intNIndex - 9);
                    this.strOut = this.HasNickName(strNickname).ToString();//this.HasNickName(strNickname).ToString()
                    break;
                #endregion
                #region 检测邮箱
                case "Email":
                    //strEmail = Classlibrary.GetRequest("Email", 1).ToString();
                    //this.strOut = this.HasEmail(strEmail).ToString();
                    break;
                #endregion
                #region 检测密码
                case "Password":
                    strPassword = Classlibrary.GetRequest("Password", 1).ToString();
                    strRePassword = Classlibrary.GetRequest("RePassword", 1).ToString();
                    this.strOut = this.LeavePassword(strPassword, strRePassword).ToString();
                    break;
                #endregion
                #region 检测重复密码
                case "RePassword":
                    strPassword = Classlibrary.GetRequest("Password", 1).ToString();
                    strRePassword = Classlibrary.GetRequest("RePassword", 1).ToString();
                    this.strOut = this.LeaveRePassword(strPassword, strRePassword).ToString();
                    break;
                #endregion
                #region 检测生日
                case "Birthday":
                    strYear = Classlibrary.GetRequest("Year", 1).ToString();
                    strMonth = Classlibrary.GetRequest("Month", 1).ToString();
                    strDay = Classlibrary.GetRequest("Day", 1).ToString();
                    this.strOut = this.CheckBirthDay(strYear, strMonth, strDay).ToString();
                    break;
                #endregion
                #region 检测城市
                case "Pro":
                    strProvince = Classlibrary.GetRequest("Province", 1).ToString();
                    strCity = Classlibrary.GetRequest("City", 1).ToString();
                    this.strOut = this.CheckSpace(strProvince, strCity).ToString();
                    break;
                #endregion
                #region 第一步注册检测
                case "RegFirst":
                    strUserName = Classlibrary.GetRequest("u", 1).ToString().Trim();
                    strPassword = Classlibrary.GetRequest("p", 1).ToString().Trim();
                    strRePassword = Classlibrary.GetRequest("rp", 1).ToString().Trim();
                    strVCode = Classlibrary.GetRequest("c", 1).ToString().Trim();
                    int intReturnFirst = this.CheckRegFirst(strUserName, strPassword, strRePassword, strVCode);
                    if (intReturnFirst == 0)
                        this.strOut = "<script>window.location.href=\"RegSec.aspx?u=" + strUserName + "&p=" + strPassword + "&rp=" + strRePassword + "&c=" + strVCode + "\";</script>";
                    else if (intReturnFirst == -1)
                        this.strOut = this.AlertMe(false, "两次密码填写不匹配，请认真填写.");
                    else if (intReturnFirst == -13)
                        this.strOut = this.AlertMe(false, "密码或确认密码不能为空,请重新填写.");
                    else if (intReturnFirst == -16)
                        this.strOut = this.AlertMe(false, "用户名填写错误,请重新填写.");
                    else if (intReturnFirst == -20)
                        this.strOut = this.AlertMe(false, "密码填写错误,请重新填写.");
                    else if (intReturnFirst == -22)
                        this.strOut = this.AlertMe(false, "您的浏览器已禁用 Cookies,请启用.");
                    else if (intReturnFirst == -23)
                        this.strOut = this.AlertMe(false, "验证码输入错误,请输入正确的验证码！");
                    else if (intReturnFirst == -24)
                        this.strOut = this.AlertMe(false, "验证码不能为空.");
                    else
                        this.strOut = this.AlertMe(false, "程序错误,请联系客服.");
                    break;
                #endregion
                #region 更新玩家最近登陆的服务器
                case "getService":
                    //strRecentServer = Classlibrary.GetRequest("Server", 1).ToString();
                    //strRecentLoginTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    //int strUID = Convert.ToInt32(StringItem.MD5Decrypt(SessionItem.GetCookieValue("MainUserInfo", "UserID").ToString(), ParameterData.MD5OnlineKey));
                    //Classlibrary.UpdateMainUser_RecentServer(strUID, strRecentServer, strRecentLoginTime);
                    break;
                #endregion
                #region 接口注册
                case "RegIf":
                    strUserName = Classlibrary.GetRequest("UserName", 1).ToString();
                    strPassword = Classlibrary.GetRequest("Password", 1).ToString();
                    strRePassword = Classlibrary.GetRequest("RePassword", 1).ToString();
                    //strEmail = Classlibrary.GetRequest("Email", 1).ToString();
                    //strNickname = Server.UrlDecode(Server.UrlEncode(Classlibrary.GetRequest("Nickname", 1).ToString()));
                    //strNickname = HttpUtility.UrlDecode(Classlibrary.GetRequest("Nickname", 1).ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                    strURL = Request.Url.ToString().Trim();
                    intIndex = strURL.IndexOf("Nickname=");
                    intIndexN = strURL.IndexOf("&Gender=");
                    strNickname = strURL.Substring(intIndex + 9, intIndexN - intIndex - 9);

                    strGender = Classlibrary.GetRequest("Gender", 1).ToString();
                    strYear = Classlibrary.GetRequest("Year", 1).ToString();
                    strMonth = Classlibrary.GetRequest("Month", 1).ToString();
                    strDay = Classlibrary.GetRequest("Day", 1).ToString();
                    strProvince = Classlibrary.GetRequest("Province", 1).ToString();
                    strCity = Classlibrary.GetRequest("City", 1).ToString();
                    //strIntro = Classlibrary.GetRequest("Intro", 1).ToString();
                    //strQQ = Classlibrary.GetRequest("QQ", 1).ToString();
                    //strMSN = Classlibrary.GetRequest("MSN", 1).ToString();
                    //strVCode = Classlibrary.GetRequest("VCode", 1).ToString();
                    //strCardId = Classlibrary.GetRequest("CardId", 1).ToString();
                    //strRealName = Classlibrary.GetRequest("RealName", 1).ToString();
                    //blncbReg = Convert.ToBoolean(Classlibrary.GetRequest("cbReg", 1));

                    //this.strOut = "经理名=."strNickname);
                    //获取类型

                    intReturn = this.DoIt(strUserName, strPassword, strRePassword, strNickname, strGender, strYear, strMonth, strDay, strProvince, strCity, strQQ);
                    if (intReturn == 1)
                    {
                        this.strOut = "用户名已存在，请重新填写.";
                    }
                    else if (intReturn == 2)
                    {
                        this.strOut = "昵称已存在，请重新填写.";
                    }
                    else if (intReturn == 3)
                    {
                        this.strOut = "Email已存在，请重新填写.";
                    }
                    else if (intReturn == -1)
                    {
                        this.strOut = "两次密码填写不匹配，请认真填写.";
                    }
                    else if (intReturn == -2)
                    {
                        this.strOut = "请选择正确的出生年.";
                    }
                    else if (intReturn == -3)
                    {
                        this.strOut = "请选择正确的出生月.";
                    }
                    else if (intReturn == -4)
                    {
                        this.strOut = "请选择正确的出生月.";
                    }
                    else if (intReturn == -5)
                    {
                        this.strOut = "请选择正确的省份.";
                    }
                    else if (intReturn == -6)
                    {
                        this.strOut = "请选择正确的城市.";
                    }
                    else if (intReturn == -7)
                    {
                        this.strOut = "请确认您已经仔细阅读并同意注册协议.";
                    }
                    else if (intReturn == -8)
                    {
                        this.strOut = "请确定推荐人是否存在.";
                    }
                    else if (intReturn == -9)
                    {
                        this.strOut = "QQ填写错误,请重新填写.";
                    }
                    else if (intReturn == -10)
                    {
                        this.strOut = "MSN填写错误,请重新填写.";
                    }
                    else if (intReturn == -11)
                    {
                        this.strOut = "出现错误,请重新填写.";
                    }
                    else if (intReturn == -12)
                    {
                        this.strOut = "用户名不能为空,请重新填写.";
                    }
                    else if (intReturn == -13)
                    {
                        this.strOut = "密码或确认密码不能为空,请重新填写.";
                    }
                    else if (intReturn == -14)
                    {
                        this.strOut = "昵称不能为空,请重新填写.";
                    }
                    else if (intReturn == -15)
                    {
                        this.strOut = "Email不能为空,请重新填写.";
                    }
                    else if (intReturn == -16)
                    {
                        this.strOut = "用户名填写错误,请重新填写.";
                    }
                    else if (intReturn == -17)
                    {
                        this.strOut = "昵称填写不符合规则，请重新填写.";
                    }
                    else if (intReturn == -19)
                    {
                        this.strOut = "昵称填写不符合规则，请重新填写.";
                    }
                    else if (intReturn == -20)
                    {
                        this.strOut = "密码填写错误,请重新填写.";
                    }
                    else if (intReturn == -22)
                    {
                        this.strOut = "您的浏览器已禁用 Cookies,请启用.";
                    }
                    else if (intReturn == -23)
                    {
                        this.strOut = "验证码输入错误,请输入正确的验证码！";
                    }
                    else if (intReturn == -24)
                    {
                        this.strOut = "验证码不能为空.";
                    }
                    else if (intReturn == -25)
                    {
                        this.strOut = "省份选择出现错误,请重新选择.";
                    }
                    else if (intReturn == -26)
                    {
                        this.strOut = "城市选择出现错误,请重新选择.";
                    }
                    else if (intReturn == 0)
                    {
                        this.strOut = "注册成功";
                    }
                    else if (intReturn == -48)
                    {
                        this.strOut = "无身份信息将被列入防沉迷,连续游戏3小时将退出.";
                    }
                    else if (intReturn == -44)
                    {
                        this.strOut = "身份证号码为15位或者18位数字,请正确输入.";
                    }
                    else if (intReturn == -40)
                    {
                        this.strOut = "身份证号码为15位或者18位数字,请正确输入.";
                    }
                    else if (intReturn == -41)
                    {
                        this.strOut = "填写的身份证号非法,请重新填写.";
                    }
                    else if (intReturn == -42)
                    {
                        this.strOut = "填写的身份证号非法,请重新填写.";
                    }
                    else if (intReturn == -43)
                    {
                        this.strOut = "填写的身份证号非法,请重新填写.";
                    }
                    else if (intReturn == -46)
                    {
                        this.strOut = "身份证号码与所选生日不符,请检查.";
                    }
                    else if (intReturn == -47)
                    {
                        this.strOut = "身份证号码与所选性别不符,请检查.";
                    }
                    else if (intReturn == -49)
                    {
                        this.strOut = "真实姓名不能为空,请认真填写.";
                    }
                    else if (intReturn == -51)
                    {
                        this.strOut = "身份证号不能为空,请认真填写.";
                    }
                    else if (intReturn == -50)
                    {
                        this.strOut = "您的真实姓名不符合规则，请重新填写.";
                    }
                    else if (intReturn == -61)
                    {
                        this.strOut = "输入的15位身份证号码不在规定范围内，请重新填写";
                    }
                    else if (intReturn == -71)
                    {
                        this.strOut = "输入的身份证号码不符合规则，请重新填写";
                    }
                    else if (intReturn == -62)
                    {
                        this.strOut = "输入的身份证号码不在规定范围内，请重新填写.";
                    }
                    else if (intReturn == -63)
                    {
                        this.strOut = "QQ号码只能是数字,请重新填写.";
                    }
                    else
                    {
                        this.strOut = "填写内容不充分，请确认后提交.";
                    }
                    break;
                #endregion 接口注册
            }
        }

        #region CreateMainUser
        public string CreateMainUser()
        {
            return "<script>window.top.location.href=\"Register?type=ok\"</script>";
        }
        #endregion

        #region CheckRegFirst
        /// <summary>
        /// 名称：第一步注册检测
        /// 方法：CheckRegFirst
        /// 制作人：KXT
        /// 创建时间：2009-06-18 17：00
        /// 修改时间：
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <param name="strPassword">密码</param>
        /// <param name="strRePassword">确认密码</param>
        /// <param name="strVCode">验证码</param>
        /// <returns></returns>
        private int CheckRegFirst(string strUserName, string strPassword, string strRePassword, string strVCode)
        {
            int intReturn = 0;
            if (!StringItem.IsValidLoginLength(strUserName, 6, 16))
            {
                intReturn = -16;
            }
            else
            {
                if (strPassword == "" || strRePassword == "")
                {
                    intReturn = -13;
                }
                else if (strPassword != strRePassword)
                {
                    intReturn = -1;
                }
                else if (!StringItem.IsValidLoginLength(strPassword, 6, 16))
                {
                    intReturn = -20;
                }
                else
                {
                    HttpContext hc = HttpContext.Current;
                    //验证码判断
                    if (strVCode != "")
                    {
                        if (hc.Request.Cookies["CheckCode"] == null)
                        {
                            intReturn = -22;
                        }



                        if (String.Compare(hc.Request.Cookies["CheckCode"].Value, strVCode, true) != 0)
                        {
                            intReturn = -23;
                        }
                    }
                    else
                    {
                        intReturn = -24;
                    }
                }
            }
            return intReturn;
        }
        #endregion

        #region DoIt
        public int DoIt(string strUserName, string strPassword, string strRePassword, string strEmail, string strNickName, string strGender,
            string strYear, string strMonth, string strDay, string strProvince, string strCity, string strIntro, string strQQ, string strMSN, bool blncbReg, string strVCode, string strCardId, string strRealName)
        {
            int intType = -11, intUserID;
            strNickName = strNickName.Trim();
            strUserName = strUserName.Trim();
            strPassword = strPassword.Trim();

            if (strUserName == "")
            {
                return -12;
            }
            //else if (Classlibrary.HasUserName(strUserName))
            //{
            //    return  1;
            //}

            if (strPassword == "" || strRePassword == "")
            {
                return -13;
            }
            if (strPassword != strRePassword)
            {
                return -1;
            }

            if (!StringItem.IsValidLoginLength(strPassword, 6, 16))
            {
                return -20;
            }

            if (!StringItem.IsValidLoginLength(strUserName, 6, 16))
            {
                return -16;
            }

            if (strNickName == "")
            {
                return -14;
            }
            if (Classlibrary.HasNickName(strNickName))
            {
                return 2;
            }

            if (!StringItem.IsValidName(strNickName, 1, 20))//!StringItem.IsValidWord(strNickName) || !StringItem.IsVaildLength(strNickName, 1, 20))
            {
                return -17;//不符合规则
            }

            if (strNickName.IndexOf("@") != -1)
                return -17;//不符合规则

            int intNType = this.CheckNickName(strNickName);
            if (intNType == -1)
            {
                return -17;//不符合规则
            }

            if (strEmail == "")
            {
                return -15;
            }
            //else if (Classlibrary.HasEmail(strEmail))
            //{
            //    return 3;
            //}
            else if (!StringItem.IsValidEmail(strEmail))
            {
                return 3;
            }

            if (!StringItem.IsValidNumber(strYear) || Convert.ToInt32(strYear) < 1)
            {
                return -2;
            }

            if (!StringItem.IsValidNumber(strMonth) || Convert.ToInt32(strMonth) < 1)
            {
                return -3;
            }

            if (!StringItem.IsValidNumber(strDay) || Convert.ToInt32(strDay) < 1)
            {
                return -4;
            }

            if (strMonth.Length == 1)
                strMonth = "0" + strMonth;

            if (strDay.Length == 1)
                strDay = "0" + strDay;

            string strBirthDay = strYear + "-" + strMonth + "-" + strDay;
            DateTime datBirthDay;
            try
            {
                datBirthDay = Convert.ToDateTime(strBirthDay);
            }
            catch
            {
                strBirthDay = "1980-01-01";
            }

            if (strProvince == "")
            {
                return -5;
            }
            else
            {
                if (strProvince.IndexOf("Method error") != -1)
                    return -25;
            }

            if (strCity == "")
            {
                return -6;
            }
            else
            {
                if (strCity.IndexOf("Method error") != -1)
                    return -26;
            }

            if (blncbReg == false)
            {
                return -7;
            }

            if (strIntro != "")
            {
                if (!Classlibrary.HasNickName(strIntro))
                {
                    return -8;
                }
            }

            if (strQQ != "")
            {
                if (!StringItem.IsValidNumber(strQQ))
                {
                    return -9;
                }
            }

            if (strMSN != "")
            {
                if (!StringItem.IsValidEmail(strMSN))
                {
                    return -10;
                }
            }

            HttpContext hc = HttpContext.Current;
            //验证码判断
            if (strVCode != "")
            {
                if (hc.Request.Cookies["CheckCode"] == null)
                {
                    return -22;
                }

                if (String.Compare(hc.Request.Cookies["CheckCode"].Value, strVCode, true) != 0)
                {
                    return -23;
                }
            }
            else
            {
                return -24;
            }

            //else
            //{
            //    lblMessage.Text = "验证码输入正确！";
            //    lblMessage.Visible = true;
            //}

            //if (strVCode == "")
            //{
            //    return -22;
            //}
            //else
            //{ 
            //    if(StringItem.IsVaildLength(strVCode,)
            //}

            //检查真实姓名身份证号是否为空
            if (strRealName == "")
            {
                return -49;
            }
            else if (strCardId == "")
            {
                return -51;
            }

            if (strRealName != "")
            {
                if (!Regex.IsMatch(strRealName, @"^[\u4e00-\u9fa5]{2}([\u4e00-\u9fa5])*$"))
                    return -50;
            }
            string ymd = "";
            if (strCardId.Length == 15)
            {
                ymd = strCardId.Substring(6, 2);//
                if (Convert.ToInt32(ymd) < 0 || Convert.ToInt32(ymd) >= 100)
                    return -61;
                if (!Regex.IsMatch(strCardId, @"/^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$/"))
                    return -71;
            }

            if (strCardId.Length == 18)
            {
                ymd = strCardId.Substring(6, 4);//
                if (Convert.ToInt32(ymd) < 1900 || Convert.ToInt32(ymd) > 2010)
                    return -62;
            }

            int intGender = Convert.ToInt32(strGender);

            string strCardIdA = "";
            if (strCardId != "")
            {
                if (strCardId.Length == 15)
                    strCardIdA = this.per15To18(strCardId);
                else
                    strCardIdA = strCardId;
                strCardIdA = strCardIdA.ToLower();

                if (!Regex.IsMatch(strCardIdA, @"^(\d{15}|(\d{17}(\d|x)))$"))
                    return -44;

                int intReturns = this.CheckCidInfo(strCardIdA, strBirthDay, strProvince, strGender);

                if (intReturns != 0)
                    return intReturns;
            }
            else
            {
                strCardId = "000000000000000000";
            }

            //string strFromURL = SessionItem.GetCookieValue("FromURL");
            //if (strFromURL == null)
            //{
            //    strFromURL = "";
            //}

            //string strSearchTag = SessionItem.GetCookieValue("SearchTag");
            //if (strSearchTag == null)
            //    strSearchTag = "";
            //string strSearchURL = SessionItem.GetCookieValue("SearchURL");
            //if (strSearchURL == null)
            //    strSearchURL = "";
            SqlDataReader sdr = Classlibrary.RegisterUser(strUserName, strPassword, strNickName, intGender, strBirthDay, strProvince, strCity, strQQ);
            if (sdr.Read())
            {
                intType = (int)sdr["Type"];
                intUserID = (int)sdr["UserID"];
                //1 用户名重复，2 昵称重复，3 电子邮箱重复，0 注册成功
                if (intType == 0)
                {
                    DataRow drU = Classlibrary.GetUserRowByUserNamePWD(strUserName, strPassword);
                    if (drU != null)
                    {
                        string strContent = "欢迎注册XBA篮球经理游戏通行证，您的通行证资料如下：<br />"
                                    + "用户名：" + strUserName + "  密码：" + strPassword + " <br />"
                                    + "请妥善保管帐号密码。您可以通过以下网址直接登录进入游戏："
                                    + "http://www.xba.com.cn <br />"
                                    + "如果您的密码丢失，可以在首页的密码找回处寻回。<br />更多问题请咨询客服QQ15908920 工作日9：00到18：00为您服务。<br />"
                                    + "<br />"
                                    + "<p style='text-align:right'>XBA篮球经理运营团队</p>";
                    }
                    else
                    {
                        sdr.Close();
                    }
                }
                sdr.Close();
                return intType;
            }
            else
            {
                sdr.Close();
                return intType;
            }

            //return intType = -11;
        }

        public int DoIt(string strUserName, string strPassword, string strRePassword, string strNickName, string strGender, string strYear, string strMonth, string strDay, string strProvince, string strCity, string strVCode, string strQQ)
        {
            int intType = -11, intUserID;
            strNickName = strNickName.Trim();
            strUserName = strUserName.Trim();
            strPassword = strPassword.Trim();

            if (strUserName == "")
            {
                return -12;
            }
            //else if (Classlibrary.HasUserName(strUserName))
            //{
            //    return  1;
            //}

            if (strPassword == "" || strRePassword == "")
            {
                return -13;
            }
            if (strPassword != strRePassword)
            {
                return -1;
            }

            if (!StringItem.IsValidLoginLength(strPassword, 6, 16))
            {
                return -20;
            }

            if (!StringItem.IsValidLoginLength(strUserName, 6, 16))
            {
                return -16;
            }

            if (strNickName == "")
            {
                return -14;
            }
            if (Classlibrary.HasNickName(strNickName))
            {
                return 2;
            }

            if (!StringItem.IsValidName(strNickName, 1, 20))//!StringItem.IsValidWord(strNickName) || !StringItem.IsVaildLength(strNickName, 1, 20))
            {
                return -17;//不符合规则
            }

            if (strNickName.IndexOf("@") != -1)
                return -17;//不符合规则

            int intNType = this.CheckNickName(strNickName);
            if (intNType == -1)
            {
                return -17;//不符合规则
            }

            if (!StringItem.IsValidNumber(strYear) || Convert.ToInt32(strYear) < 1)
            {
                return -2;
            }

            if (!StringItem.IsValidNumber(strMonth) || Convert.ToInt32(strMonth) < 1)
            {
                return -3;
            }

            if (!StringItem.IsValidNumber(strDay) || Convert.ToInt32(strDay) < 1)
            {
                return -4;
            }

            if (strMonth.Length == 1)
                strMonth = "0" + strMonth;

            if (strDay.Length == 1)
                strDay = "0" + strDay;

            string strBirthDay = strYear + "-" + strMonth + "-" + strDay;
            DateTime datBirthDay;
            try
            {
                datBirthDay = Convert.ToDateTime(strBirthDay);
            }
            catch
            {
                strBirthDay = "1980-01-01";
            }

            if (strProvince == "")
            {
                return -5;
            }
            else
            {
                if (strProvince.IndexOf("Method error") != -1)
                    return -25;
            }

            if (strCity == "")
            {
                return -6;
            }
            else
            {
                if (strCity.IndexOf("Method error") != -1)
                    return -26;
            }

            HttpContext hc = HttpContext.Current;
            //验证码判断
            if (strVCode != "")
            {
                if (hc.Request.Cookies["CheckCode"] == null)
                {
                    return -22;
                }

                if (String.Compare(hc.Request.Cookies["CheckCode"].Value, strVCode, true) != 0)
                {
                    return -23;
                }
            }
            else
            {
                return -24;
            }

            if (strQQ.Length > 0)
            {
                if (!StringItem.IsValidNumber(strQQ) || Convert.ToInt32(strQQ) < 1)
                {
                    return -63;
                }
            }

            int intGender = Convert.ToInt32(strGender);

            SqlDataReader sdr = Classlibrary.RegisterUser(strUserName, strPassword, strNickName, intGender, strBirthDay, strProvince, strCity, strQQ);
            if (sdr.Read())
            {
                intType = (int)sdr["Type"];
                intUserID = (int)sdr["UserID"];
                //1 用户名重复，2 昵称重复，3 电子邮箱重复，0 注册成功
                if (intType == 0)
                {
                    DataRow drU = Classlibrary.GetUserRowByUserNamePWD(strUserName, strPassword);
                    if (drU != null)
                    {
                        string strContent = "欢迎注册XBA篮球经理游戏通行证，您的通行证资料如下：<br />"
                                    + "用户名：" + strUserName + "  密码：" + strPassword + " <br />"
                                    + "请妥善保管帐号密码。您可以通过以下网址直接登录进入游戏："
                                    + "http://www.xba.com.cn <br />"
                                    + "如果您的密码丢失，可以在首页的密码找回处寻回。<br />更多问题请咨询客服QQ15908920 工作日9：00到18：00为您服务。<br />"
                                    + "<br />"
                                    + "<p style='text-align:right'>XBA篮球经理运营团队</p>";
                    }
                    else
                    {
                        sdr.Close();
                    }
                }
                sdr.Close();
                return intType;
            }
            else
            {
                sdr.Close();
                return intType;
            }

            //return intType = -11;
        }

        public int DoIt(string strUserName, string strPassword, string strRePassword, string strNickName, string strGender, string strYear, string strMonth, string strDay, string strProvince, string strCity, string strQQ)
        {
            int intType = -11, intUserID;
            strNickName = strNickName.Trim();
            strUserName = strUserName.Trim();
            strPassword = strPassword.Trim();

            if (strUserName == "")
            {
                return -12;
            }
            //else if (Classlibrary.HasUserName(strUserName))
            //{
            //    return  1;
            //}

            if (strPassword == "" || strRePassword == "")
            {
                return -13;
            }
            if (strPassword != strRePassword)
            {
                return -1;
            }

            if (!StringItem.IsValidLoginLength(strPassword, 6, 16))
            {
                return -20;
            }

            if (!StringItem.IsValidLoginLength(strUserName, 6, 16))
            {
                return -16;
            }

            if (strNickName == "")
            {
                return -14;
            }
            if (Classlibrary.HasNickName(strNickName))
            {
                return 2;
            }

            if (!StringItem.IsValidName(strNickName, 1, 20))//!StringItem.IsValidWord(strNickName) || !StringItem.IsVaildLength(strNickName, 1, 20))
            {
                return -17;//不符合规则
            }

            if (strNickName.IndexOf("@") != -1)
                return -17;//不符合规则

            int intNType = this.CheckNickName(strNickName);
            if (intNType == -1)
            {
                return -17;//不符合规则
            }

            if (!StringItem.IsValidNumber(strYear) || Convert.ToInt32(strYear) < 1)
            {
                return -2;
            }

            if (!StringItem.IsValidNumber(strMonth) || Convert.ToInt32(strMonth) < 1)
            {
                return -3;
            }

            if (!StringItem.IsValidNumber(strDay) || Convert.ToInt32(strDay) < 1)
            {
                return -4;
            }

            if (strMonth.Length == 1)
                strMonth = "0" + strMonth;

            if (strDay.Length == 1)
                strDay = "0" + strDay;

            string strBirthDay = strYear + "-" + strMonth + "-" + strDay;
            DateTime datBirthDay;
            try
            {
                datBirthDay = Convert.ToDateTime(strBirthDay);
            }
            catch
            {
                strBirthDay = "1980-01-01";
            }

            if (strProvince == "")
            {
                return -5;
            }
            else
            {
                if (strProvince.IndexOf("Method error") != -1)
                    return -25;
            }

            if (strCity == "")
            {
                return -6;
            }
            else
            {
                if (strCity.IndexOf("Method error") != -1)
                    return -26;
            }

            if (strQQ.Length > 0)
            {
                if (!StringItem.IsValidNumber(strQQ) || Convert.ToInt32(strQQ) < 1)
                {
                    return -63;
                }
            }

            int intGender = Convert.ToInt32(strGender);

            SqlDataReader sdr = Classlibrary.RegisterUser(strUserName, strPassword, strNickName, intGender, strBirthDay, strProvince, strCity, strQQ);
            if (sdr.Read())
            {
                intType = (int)sdr["Type"];
                intUserID = (int)sdr["UserID"];
                //1 用户名重复，2 昵称重复，3 电子邮箱重复，0 注册成功
                if (intType == 0)
                {
                    DataRow drU = Classlibrary.GetUserRowByUserNamePWD(strUserName, strPassword);
                    if (drU != null)
                    {
                        string strContent = "注册成功";
                    }
                    else
                    {
                        sdr.Close();
                    }
                }
                sdr.Close();
                return intType;
            }
            else
            {
                sdr.Close();
                return intType;
            }

            //return intType = -11;
        }

        #endregion

        #region CreateChinaCode
        public string CreateChinaCode()
        {
            return "<script>window.top.location.href=\"http://www.xba.com.cn/chinaCode.aspx\"</script>";
        }
        #endregion

        #region alertme
        public string AlertMe(bool blIsAutoClose, string strAlertText)
        {
            if (blIsAutoClose)
            {
                return "<script type=\"text/javascript\">function auto_close(){setTimeout(\"dialog.close($('div_public_process'))\", 800);}auto_close();</script><div style=\"width:400px; height:100px;border:3px solid #d35200;background:#fff; text-align:center;line-height:100px;\">" + strAlertText + "</div>";
            }
            else
            {
                //return "<div style=\"width:400px; height:90px;border:3px solid #d35200;background:#fff; text-align:center; padding-top:10px\"><div style=\"line-height:50px;\">" + strAlertText + "</div><input type=\"button\" value=\"确定\" onclick=\"dialog.close();\" style=\"width:80px; height:24px;\"/></div>";
                return "<script type=\"text/javascript\">ShowDialog_s(\"系统消息\",\"" + strAlertText + "\");</script>";
                //return "<div id=\"floatBoxBg\"></div><div id=\"floatBox\" class=\"floatBox\"><div class=\"title\"><h4>系统消息</h4><span>关闭</span></div><div class=\"content\">" + strAlertText + "</div></div>";
            }
        }

        public string AlertMe(bool blIsAutoClose, string strAlertText, string strRedirect, string strMenu)
        {
            //blIsAutoClose = false;
            if (blIsAutoClose)
            {
                return "<script type=\"text/javascript\">function auto_close(){setTimeout(\"dialog.close($('div_public_process'));\", 800);setTimeout(\"" + strMenu + "\", 800);}auto_close();</script><div style=\"width:400px; height:100px;border:3px solid #d35200;background:#fff; text-align:center;line-height:100px;\">" + strAlertText + "</div>";
            }
            else
            {
                return "<div style=\"width:400px; height:90px;border:3px solid #d35200;background:#fff; text-align:center; padding-top:10px\"><div style=\"line-height:50px;\">" + strAlertText + "</div><input type=\"button\" value=\"确定\" style=\"width:80px; height:24px;\" onclick=\"dialog.close();" + strMenu + "\" ></div>";
            }
        }
        #endregion

        #region alertOK
        public string alertOK()
        {

            return "<div style=\"position:absolute;margin:0 atuo;text-align:left;border:1px;height:286px;width:471px;background:#fff;background-image:url(/Images/txzbg.png);\"><div style=\"left:2px; top:2px; width:423px; height:57px; line-height:57px; text-align:center; vertical-align:middle; background-image:url(/Images/txztbg.png);\"><b style=\"font-size:25px; color:Green;\">恭喜您注册成功</b></div><div  style=\"width:423px; height:35px; line-height:35px; text-align:center; vertical-align:middle; background-image:url(/Images/txztbg.png);\">建议您将本站网址<a href=\"#\" style=\"color:Red;\">[加入收藏]</a> 请选择推荐服务器</div><div style=\"top:89px; height:45px; line-height:45px; width:423px; text-align:center; border-bottom:1px; border-color:Black;\"><b>篮球经理：</b><input type=\"button\" style=\"background-image:url(/Images/txzabg.png); width:108px; height:24px;\" />&nbsp;&nbsp;推荐</div><div style=\"top:134px; height:45px; line-height:45px; width:423px; text-align:center; border-bottom:1px; border-color:Black;\"><b>梦幻足球：</b><input type=\"button\" style=\"background-image:url(/Images/txzabg.png); width:108px; height:24px;\" />&nbsp;&nbsp;推荐</div><div style=\"top:179px; height:45px; line-height:45px; width:423px; text-align:center; border-bottom:1px; border-color:Black;\"><b>兽血沸腾：</b><input type=\"button\" style=\"background-image:url(/Images/txzabg.png); width:108px; height:24px;\" />&nbsp;&nbsp;推荐</div><div style=\"top:224px; height:35px; line-height:35px; width:423px; text-align:right;; border-bottom:1px; background-image:url(/Images/txztbg.png); vertical-align:bottom;\"><a href=\"membercenter.aspx\">选择其它游戏服务器</a></div><div></div></div>";
        }
        #endregion

        #region CheckCidInfo
        private int CheckCidInfo(string cid, string strBirthDay, string strProvince, string strGender)
        {
            string[] aCity = new string[] { null, null,null, null, null, null, null, null, null, null, null, 
        "北京", "天津", "河北", "山西", "内蒙古", null, null, null, null, null, 
        "辽宁", "吉林","黑龙江", null, null, null, null, null, null, null, 
        "上海", "江苏", "浙江", "安微", "福建", "江西", "山东", null, null, null, 
        "河南","湖北", "湖南", "广东", "广西", "海南", null, null, null, 
        "重庆", "四川", "贵州", "云南", "西藏", null, null, null, null, null,null, 
        "陕西", "甘肃", "青海", "宁夏", "新疆", null, null, null, null, null, 
        "台湾", null, null, null, null, null, null, null, null, null, 
        "香港", "澳门", null, null, null, null, null, null, null, null, "国外" };
            double iSum = 0;
            string info = "";
            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^(\d{17}(\d|x))$");//@"^d{17}(d|x)$");
            System.Text.RegularExpressions.Match mc = rg.Match(cid);
            //double iSum = 0;
            //string info = "";
            //System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^d{17}(d|x)$");
            //System.Text.RegularExpressions.Match mc = rg.Match(cid);
            if (!mc.Success)
            {
                return -40;//"";
            }

            cid = cid.ToLower();
            cid = cid.Replace("x", "a");

            if (aCity[int.Parse(cid.Substring(0, 2))] == null)
            {
                return -41;//"非法地区";
            }
            //else
            //{
            //    if (strProvince != aCity[int.Parse(cid.Substring(0, 2))])
            //        return -45;//城市与所填写的身份证不相符
            //}

            try
            {
                //DateTime.Parse(cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2));
                //string strBirthdayA = cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2);
                //if (strBirthDay != strBirthdayA)
                //    return -46;//生日与所填写的身份证不相符
            }
            catch
            {
                return -42;// "非法生日";
            }

            for (int i = 17; i >= 0; i--)
            {
                iSum += (System.Math.Pow(2, i) % 11) * int.Parse(cid[17 - i].ToString(), System.Globalization.NumberStyles.HexNumber);// System.Globalization.NumberStyles.HexNumber);
            }
            if (iSum % 11 != 1)
                return -43;// "非法证号";

            if (strGender == "0")
                strGender = "男";
            else
                strGender = "女";

            //if (strGender != (int.Parse(cid.Substring(16, 1)) % 2 == 1 ? "男" : "女"))
            //    return -47;

            return 0;// (aCity[int.Parse(cid.Substring(0, 2))] + "," + cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2) + "," + (int.Parse(cid.Substring(16, 1)) % 2 == 1 ? "男" : "女"));
        }
        #endregion

        #region per15To18
        public string per15To18(string perIDSrc)
        {
            int iS = 0;

            //加权因子常数
            int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //校验码常数
            string LastCode = "10X98765432";
            //新身份证号
            string perIDNew;

            perIDNew = perIDSrc.Substring(0, 6);
            //填在第6位及第7位上填上‘1’，‘9’两个数字
            perIDNew += "19";

            perIDNew += perIDSrc.Substring(6, 9);

            //进行加权求和
            for (int i = 0; i < 17; i++)
            {
                iS += int.Parse(perIDNew.Substring(i, 1)) * iW[i];
            }

            //取模运算，得到模值
            int iY = iS % 11;
            //从LastCode中取得以模为索引号的值，加到身份证的最后一位，即为新身份证号。
            perIDNew += LastCode.Substring(iY, 1);
            return perIDNew;
        }
        #endregion

        #region HasUserName
        public int HasUserName(string strUserName)
        {
            int intType = this.CheckUserName(strUserName);
            if (intType == 1)
            {
                if (!Classlibrary.HasUserName(strUserName))
                {
                    return 1;//"用户名可以正常使用。";
                }
                else
                {
                    return -1;//"用户名已存在，请重新填写。";
                }
            }
            else
            {
                return -2;//不符合规则
            }
        }
        #endregion

        #region CheckUserName
        public int CheckUserName(string strUserName)
        {
            if (StringItem.IsValidLoginLength(strUserName, 6, 16))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        #endregion

        #region HasNickName
        public int HasNickName(string strNickName)
        {
            int intType = this.CheckNickName(strNickName);
            if (intType == 1)
            {
                if (!Classlibrary.HasNickName(strNickName))
                {
                    return 1;//"经理名可以正常使用。";
                }
                else
                {
                    return -1;//"经理名已存在，请重新填写。";
                }
            }
            else
            {
                return -2;//不符合规则
            }
        }
        #endregion

        #region CheckNickName
        public int CheckNickName(string strNickName)
        {
            bool blResult = StringItem.HasInvalidWord(strNickName);
            if (blResult)
            {
                return -1;//不符合规则
            }
            else
            {
                if (StringItem.IsValidName(strNickName, 1, 20))//!StringItem.HasInvalidWord(strNickName, dr) && StringItem.IsValidWord(strNickName) && StringItem.IsVaildLength(strNickName, 1, 20)
                {
                    if (strNickName.IndexOf("@") == -1)
                    {
                        return 1;//符合规则
                    }
                    else
                    {
                        return -1;//不符合规则
                    }
                }
                else
                {
                    return -1;//不符合规则
                }
            }            
        }
        #endregion

        #region HasEmail
        //public int HasEmail(string strEmail)
        //{
        //    int intType = this.CheckEmail(strEmail);
        //    if (intType == 1)
        //    {
        //        if (!Classlibrary.HasEmail(strEmail))
        //        {
        //            return 1;//"经理名可以正常使用。";
        //        }
        //        else
        //        {
        //            return -1;//"经理名已存在，请重新填写。";
        //        }
        //    }
        //    else
        //    {
        //        return -2;//不符合规则
        //    }
        //}
        #endregion

        #region CheckEmail
        public int CheckEmail(string strEmail)
        {
            if (StringItem.IsValidEmail(strEmail))
            {
                return 1;//符合规则
            }
            else
            {
                return -1;//不符合规则
            }
        }
        #endregion

        #region CheckPassword
        public int CheckPassword(string strPassword)
        {
            if (StringItem.IsValidLoginLength(strPassword, 6, 16))
            {
                return 1;//符合规则
            }
            else
            {
                return -1;//不符合规则
            }
        }
        #endregion

        #region LeavePassword
        public int LeavePassword(string strPassword, string strRePassword)
        {
            if (StringItem.IsValidLoginLength(strPassword, 6, 16))
            {
                if (strRePassword != "")
                {
                    if (strPassword == strRePassword)
                    {
                        return 2;
                    }
                    else
                    {
                        return -2;
                    }
                }
                return 1;//符合规则
            }
            else
            {
                return -1;//不符合规则
            }
        }
        #endregion

        #region CheckRePassword
        public int CheckRePassword(string strPassword, string strRePassword)
        {
            if (strRePassword != "")
            {
                if (strPassword == strRePassword)
                {
                    return 1;
                }
                else
                {
                    return -2;
                }
            }
            else
            {
                return -1;
            }
        }
        #endregion

        #region LeaveRePassword
        public int LeaveRePassword(string strPassword, string strRePassword)
        {
            if (strRePassword != "")
            {
                if (strPassword == strRePassword)
                {
                    return 2;
                }
                else
                {
                    return -2;
                }
            }
            else
            {
                return -1;
            }
        }
        #endregion

        #region HasIntro
        //public int HasIntro(string strIntro)
        //{
        //    int intType = this.CheckIntro(strIntro);
        //    if (intType == 1)
        //    {
        //        if (Classlibrary.HasNickName(strIntro))
        //        {
        //            return 1;//"经理名可以正常使用。";
        //        }
        //        else
        //        {
        //            return -1;//"经理名已存在，请重新填写。";
        //        }
        //    }
        //    else
        //    {
        //        return -2;//不符合规则
        //    }
        //}
        #endregion

        #region CheckIntro
        //public int CheckIntro(string strIntro)
        //{
        //    DataRow dr = Main_ParameterManager.GetMainParameterRow();
        //    if (!StringItem.HasInvalidWord(strIntro, dr) && StringItem.IsValidWord(strIntro) && StringItem.IsVaildLength(strIntro, 1, 20))//StringItem.HasInvalidWord(strIntro, dr) || StringItem.IsValidName(strIntro,1,20))//StringItem.IsValidWord(strIntro) || StringItem.IsVaildLength(strIntro, 1, 20))
        //    {
        //        return 1;//符合规则
        //    }
        //    else
        //    {
        //        return -1;//不符合规则
        //    }
        //}
        #endregion

        #region CheckQQ
        public int CheckQQ(string strQQ)
        {
            if (StringItem.IsValidNumber(strQQ))
            {
                return 1;//符合规则
            }
            else
            {
                return -1;//不符合规则
            }
        }
        #endregion

        #region CheckMSN
        public int CheckMSN(string strMSN)
        {
            if (StringItem.IsValidEmail(strMSN))
            {
                return 1;//符合规则
            }
            else
            {
                return -1;//不符合规则
            }
        }
        #endregion

        #region CheckBirthDay
        public int CheckBirthDay(string strYear, string strMonth, string strDay)
        {
            if (!StringItem.IsValidNumber(strYear) || Convert.ToInt32(strYear) < 1900 || Convert.ToInt32(strYear) > DateTime.Now.Year)
            {
                return -1;//年份不符合规则
            }
            else if (!StringItem.IsValidNumber(strMonth) || Convert.ToInt32(strMonth) < 1 || Convert.ToInt32(strMonth) > 12)
            {
                return -2;//月份不符合规则
            }
            else if (!StringItem.IsValidNumber(strDay) || Convert.ToInt32(strDay) < 1 || Convert.ToInt32(strDay) > 31)
            {
                return -3;//日期不符合规则
            }
            else
            {
                return 1;//符合规则
            }
        }
        #endregion

        #region CheckSpace
        public int CheckSpace(string strProvince, string strCity)
        {
            if (!StringItem.IsValidWord(strProvince) || strProvince == "")
            {
                return -1;//省份不符合规则
            }
            else if (!StringItem.IsValidWord(strCity) || strCity == "")
            {
                return -2;//城市不符合规则
            }
            else
            {
                return 1;//符合规则
            }
        }
        #endregion
    }
}
