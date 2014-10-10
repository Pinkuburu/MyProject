<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="XiaoNei_App._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:xn="http://www.renren.com/2009/xnml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>瞧你在干嘛</title>
<style type="text/css">
body {
	font-size: 12px;
	color: #444;
	font-family: "微软雅黑";
	line-height:20px;
	text-align:left;
	margin: 0;
	}
a { color:#005eac; text-decoration:none;}
a:visited { color:#005eac;}
a:hover { color:#ff9000; text-decoration: underline;}
a:visited { color:#005eac;}
div,ul,li {padding:0; margin:0;}
img {border:0}

.body {width:800px; height:592px; background:url(Images/bg2.jpg); position:relative; margin:0 auto;}

.lgbg {
	position:absolute;
	left:137px;
	top:194px;
	width:529px;
	height:205px;
	text-align:left;
	font-size:14px;
	line-height:24px;
	}
</style>
</head>
<body>
<div class="body">
    <form id="Form1" runat="server">
        <div class="lgbg">
            <%= this.strContent %>
            <div id="div_FirstLogin" runat="server" visible = "false">
            第一次登录请输入客户端程序登录密码<br />
            请输入密码：<asp:TextBox ID="tb_Password" runat="server"></asp:TextBox>
                <asp:TextBox ID="tb_DebugSessionKey" runat="server"></asp:TextBox><br />
                <asp:Button ID="btn_Send" runat="server" Text="激活" onclick="btn_Send_Click" />
            </div>
            <%= this.strMsg %>
        </div>
    </form>
</div>
</body>
</html>
