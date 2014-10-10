<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wap.aspx.cs" Inherits="QQRobot_InterFace_vs2005.Wap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID = "btn_Turn_BB" runat="server" Text = "Turn BB" OnClick="btn_Turn_BB_Click" />
        <asp:Button ID = "btn_Turn_FB" runat="server" Text = "Turn FB" OnClick="btn_Turn_FB_Click" /><br />
        <asp:Button ID = "btn_Status_BB" runat="server" Text = "Status BB" OnClick="btn_Status_BB_Click" />
        <asp:Button ID = "btn_Status_FB" runat="server" Text = "Status FB" OnClick="btn_Status_FB_Click" /><br />
        <asp:Button ID = "btn_Check_BB" runat="server" Text = "Check BB" OnClick="btn_Check_BB_Click" />
        <asp:Button ID = "btn_Check_FB" runat="server" Text = "Check FB" OnClick="btn_Check_FB_Click" /><br />
        ------------------------<br />
        <asp:TextBox ID = "tb_KeyWords" runat="server" Width="94px"></asp:TextBox><asp:Button ID = "btn_Season" runat="server" Text = "Season" OnClick="btn_Season_Click" /><br />
    </div>
    </form>
</body>
</html>
