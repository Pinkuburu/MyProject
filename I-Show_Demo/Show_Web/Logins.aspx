<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logins.aspx.cs" Inherits="Show_Web.Logins" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Logins</title>
    <script type="text/javascript" src="JS/jquery-1.4.2.min.js"></script>
    <script type="text/javascript">
        function Login() {
            var strUserName = $("#txt_UserName").val();
            var strPassword = $("#txt_Password").val();
            $.getJSON("Login.aspx",
            {
                type:1,
                UserName:encodeURI(strUserName),
                Password:encodeURI(strPassword)
            },
            function (data) {
                if(data.UserID > 0)
                {
                    window.top.location = "Show1.aspx?UserID=" + data.UserID;
                }
                else
                {
                    alert("Login Error");
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        用户名:<input type="text" id="txt_UserName" />
        密码:<input type="text" id="txt_Password" />
        <input type="button" value="Login" onclick="Login()" />
    </div>
    </form>
</body>
</html>
