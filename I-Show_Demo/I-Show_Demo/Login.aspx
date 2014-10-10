<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="I_Show_Demo._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Login</title>
    <style type="text/css">
        body {
            font:12px Tahoma;
        }
            
        #Login_Background {
            width: 250px;
            height: 150px;
            background: #ADD8E6;
            margin: 20% 38%; 
            border-style: solid; 
            border-color: #20B2AA;
            border-width: 1px;           
        }
            
        #Login_UserInfo {
            padding-top: 15px;
            padding-left: 40px;
        }
        
        #Login_Button {
            padding-top: 5px;
            padding-left: 80px;
        }
        
    </style>
</head>
<body>
    <form id="Login_Form" runat="server">
        <div id="Login_Background">
            <div id="Login_UserInfo">
                <div id="Login_UserName">用户名：<asp:TextBox id = "tb_UserName" runat = "server" Width="100px"></asp:TextBox></div>
                <div id="Login_Password">密　码：<asp:TextBox id = "tb_Password" runat = "server" Width="100px" TextMode="Password" ></asp:TextBox></div>
            </div>
            <div id="Login_Button">
                <asp:Button id = "btn_Login" runat = "server" Text = "登录" OnClick="btn_Login_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id = "btn_Register" runat = "server" Text = "注册" OnClick="btn_Register_Click" />
            </div>
            <div id="Login_Info" runat="server" align="center" style="padding-top:3px;"><%=strMsg%></div>
        </div>    
    </form>
</body>
</html>
