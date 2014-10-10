<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="XiaoNei_Login_Web._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Test</title>
<style type="text/css">
	#Login{
		background-color:#F0F5F8;
		border:1px solid #B8D4E8;
		margin:0 auto;
		width:250px;
		height:150px;
	}
	#Title{
		background-color:#069;
	 	border-bottom:1px solid #B8D4E8;
		height:25px;
		font-size:14px;
		line-height:25px;
		color:#FFF; 
		font-family:黑体;
	}
	#Login_Form div{
		font-size:12px;
	}
	#UserInfo{
		background-color:#F0F5F8;
		border:1px solid #B8D4E8;
		margin:0 auto;
		width:250px;
		height:150px;
	}
	#UserInfo_Title{
		background-color:#069;
	 	border-bottom:1px solid #B8D4E8;
		height:25px;
		font-size:14px;
		line-height:25px;
		color:#FFF; 
		font-family:黑体;
	}
</style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="Login">
        	<div id="Title">
            	<span style="margin-left:3px">人人登录</span>
            </div>
            <div id="Login_Form" style="margin-top:20px; margin-left:30px;">
            	<div>帐号：<asp:TextBox ID="tbUserName" runat="server" Width="130px" /></div>
                <div>密码：<asp:TextBox ID="tbPassword" runat="server" Width="130px" /></div>
                <div style="margin-left:80px; margin-top:5px;"><asp:Button ID="btn_Login" runat="server" Text="登录" OnClick="btn_Login_Click" /></div>
            </div>
        </div>
        <div id="UserInfo">
            <div id="UserInfo_Title">
                <span style="margin-left:3px">登录统计</span>
            </div>
        </div>
    </form>
</body>
</html>
