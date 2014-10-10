<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="MaitiamCloud.Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .warp_table {border-collapse:collapse; width:550px; border:1px solid #4d9ab0}
        .warp_table td {border:1px solid #4d9ab0}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        来吧，这就是主页。</br>
        <asp:Button id="btnShowUser" Text="ShowUser" runat="server" onclick="btnShowUser_Click"/>
    </div>
    <div>
        <%= this.strContent %>
    </div>
    </form>
</body>
</html>
