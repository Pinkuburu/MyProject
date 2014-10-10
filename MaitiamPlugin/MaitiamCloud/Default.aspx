<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MaitiamCloud._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Maitiam Cloud</title>
    <style type="text/css">
        html,body {padding:0;margin:0;overflow:hidden;}
        html {height:100%;}
        td, input, button, select, body {
            font-family: "lucida Grande",Verdana;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <form id = "form1" runat="Server">
        <div style="text-align: left; width: 325px; height: 210px; position: relative; margin: 0 auto;">
            <div style="width:325px; height:200px; text-align:center; border:1px solid #B0C4DE; background-color:#F0F0F0; margin-top:150px; vertical-align: middle;">
                <table style="width:325px; text-align:left; height:200px; padding-top:10px;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="padding-left:10px; border-bottom:1px double #DDE4EE; width: 325px;"><b>Maitiam Cloud</b></td>
                    </tr>
                    <tr>
                        <td style="padding-left:40px; width: 325px">账&nbsp;&nbsp;&nbsp;号：<asp:TextBox id="txtUsername" runat="Server" style="width:150px;  height:23px; line-height:23px;  padding:0 0 0 4px; border:1px solid #DDE4EE; text-align:left;" /></td>
                    </tr>
                    <tr>
                        <td style="padding-left:40px; border-bottom:1px double #DDE4EE; width: 325px">密&nbsp;&nbsp;&nbsp;码：<asp:TextBox id="txtPassword" runat="Server" TextMode="Password" style="width:150px; height:23px; line-height:23px;  padding:0 0 0 4px; border:1px solid #DDE4EE; text-align:left;" /></td>
                    </tr>
                    <tr>
                        <td style="padding-left:110px; width: 325px;"><asp:Button id="btnSend" Text="登  录" runat="Server" style="font-weight:bold;height:25px;width:80px;" onclick="btnSend_Click" /></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
