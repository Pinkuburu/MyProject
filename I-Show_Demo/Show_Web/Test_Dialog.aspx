<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test_Dialog.aspx.cs" Inherits="Show_Web.Test_Dialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link rel="Stylesheet" type="text/css" href="css/dialog.css" media="screen"/>
    <script type="text/javascript" src="JS/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="JS/dialog.js"></script>
    <script type="text/javascript">
        $(function(){
            $("#form1").submit(function(){
              var str=escape($("#str").val());
              dialog("我的标题","url:post?test.asp?str="+str+"","200px","auto","form");
              return false;
            });
            $("#bt1").click(function(){
              dialog("我的标题","url:get?test.html","200px","auto","text");
            });
            $("#bt2").click(function(){
              dialog("我的标题","text:我的内容","200px","auto","text");
            });
            $("#bt3").click(function(){
              dialog("我的标题","id:testID","300px","auto","id");
            });
            $("#bt4").click(function(){
              dialog("blueidea","iframe:http://www.blueidea.com","500px","500px","iframe");
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="bt2">Test</div>
    </form>
</body>
</html>
