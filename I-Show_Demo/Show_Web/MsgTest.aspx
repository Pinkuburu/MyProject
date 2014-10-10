<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MsgTest.aspx.cs" Inherits="Show_Web.MsgTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>MsgTest</title>
    <style type="text/css">
        #navigation {
	        position:fixed;
	        z-index: 1000;
	        bottom: -35px;
	        left: 0;
	        width: 100%;
	        height:34px;
	        border-top: 1px solid #222222;
	        background:#000000;
	        background: -moz-linear-gradient(top,  #444444,  #000000);
	        background: -webkit-gradient(linear, left top, left bottom, from(#444444), to(#000000));
        }
    </style>
    <script type="text/javascript" src="JS/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="JS/jquery.timers.js"></script>
    <script type="text/javascript">
        var showBox = false;
        var i = 0;
        $(function () {
            //$("body").everyTime("2s","timer2",CheckNewMessage);            
        });
        
        function NewMessage(newMsgCount) {
            $("#navigation").html("您有 " + newMsgCount + " 条新消息");
            if (showBox == false) {
                $("#navigation").css({bottom: -35}).animate({bottom: -35}, 2500, 
                    function() {
                        $(this).animate({bottom: 0}, 700);
                        showBox = true;
                    }); 
            }
            
        };
        
        function CloseMessage() {
            $("#navigation").css({bottom: 0}).animate({bottom: 0}, 2500, 
                function() {
                    $(this).animate({bottom: -35}, 700);
                }); 
        };
        
        function CheckNewMessage(){
            $.getJSON("MessageBox.aspx",
                {
                    type:1,
                    UserID:1
                },
	            function(data){	                
		            NewMessage(data.NewMsg);
	            });
	        $("#sCount").html(i++);
        };
        
        function SendMessage() {
            intUserID = $("#UserID").val();
            intSendID = $("#SendID").val();
            strSender = $("#Sender").val();
            strContent = $("#Content").val();
            
            $.get("MessageBox.aspx?type=2&UserID=" + intUserID + "&SendID=" + intSendID + "&Sender=" + encodeURI(strSender) + "&Content=" + encodeURI(strContent),
	            function(data){
		            $("#cmd").html(data);
	            });
        };
        
        function StopTimer() {
            $("body").stopTime("timer2");
        };
        
//        $.getJSON("MessageBox.aspx",
//            {
//                type:3,
//                userid:1
//            },
//            function (data) {
//                alert(data.Count);
//                $("#json").append("<table border='1'><tr><td>ID</td><td>SendID</td><td>Sender</td><td>Status</td><td>SendTime</td></tr>");
//                $.each(data.MessageBox,function (i,item) {
//                    //alert(item.id + "--" + item.SendID + "--" + item.Sender + "--" + item.Status + "--" + item.SendTime);
//                    $("#json table").append("<tr><td>" + item.id + "</td><td>" + item.SendID + "</td><td>" + item.Sender + "</td><td>" + item.Status + "</td><td>" + item.SendTime + "</td></tr>");
//                });
//        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a href="#" onclick="CheckNewMessage()" >CheckNewMessage</a>
    </div>
    <div>
        <a href="#" onclick="NewMessage(3)" >NewMessage</a>
    </div>
    <div>
        <a href="#" onclick="CloseMessage()" >CloseMessage</a>
    </div>
    <div>
        <a href="#" onclick="StopTimer()" >StopTimer</a>
    </div>
    <div>
        收件人ID<input type="text" id="UserID"/>发件人ID<input type="text" id="SendID"/>发件人昵称<input type="text" id="Sender"/>内容<input type="text" id="Content"/><input type="button" value="send" onclick="SendMessage();" />
    </div>
    <div id="sCount"></div>
    <div id="navigation" style="color:White;"></div>
    <div id="cmd"></div>
    <div id="json"></div>
    <table border="1" cellspacing="0" cellpadding="0" width="100%">
    	<tr>
    		<td>123</td><td>123</td><td>123</td><td>123</td><td>123</td> 
    	</tr>
    </table>
    </form>
</body>
</html>

