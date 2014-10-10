<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Show1.aspx.cs" Inherits="Show_Web.Show1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Demo</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="Stylesheet" type="text/css" href="css/style.css" media="screen"/>
    <link rel="Stylesheet" type="text/css" href="css/index.css" media="screen"/>
    <link rel="Stylesheet" type="text/css" href="css/msgbox.css" media="screen"/>
    <link rel="Stylesheet" type="text/css" href="css/dialog.css" media="screen"/>
    <script type="text/javascript" src="JS/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="JS/jquery.paginate.js"></script>
    <script type="text/javascript" src="JS/jquery.timers.js"></script>
    <script type="text/javascript" src="JS/SelectList.js"></script>
    <script type="text/javascript" src="JS/dialog.js"></script>
    <script type="text/javascript" src="JS/main.js"></script>
    <!--[if IE 6]>
    <script type="text/javascript" src="JS/DD_belatedPNG_0.0.8a.js"></script>
    <script type="text/javascript">
        DD_belatedPNG.fix('.send_mes_div');
    </script>
    <![endif]-->
    <script type="text/javascript">
        var intUserID = <% =this.strUserID %>    
        var isVIP = false;
            
        function reset_btn() {
            $("#Content").text("");
        }
        $(function(){
            $("#NickName").click(function(){
                if($(this).val() == "请输入用户昵称")
                {
                    $(this).val("");
                    $(this).css("color","white");
                }                
            });
            
            $.getJSON("Login.aspx",
                {
                    Type:2,
                    UserID:intUserID,
                    rnd:Math.random()
                },
                function (data) {
                    if(data.VIP == 1)
                    {
                        isVIP = true;
                    }
                    else
                    {
                        isVIP = false;
                    }
                });
        });
    </script>
</head>
<body scroll="no">
    <div style="position:relative; height:428px;width:682px;margin:0 auto;overflow:hidden;"> 
        <form id="form1" runat="server" style="position:relative;width:682px; overflow:hidden; height:428px; margin:0 auto">
            <div class="show_div">
                <div class="hic_filter_bar">
                    <div class="ss1">
                        <input type="hidden" id="hddCity" name="hddCity" value="" />
                        <input type="hidden" id="HidProvince" name="HidProvince" value="" />
                        <input type="hidden" id="Hidcity2" name="Hidcity2" />
                        <select onchange="setcity(0,'')" id="prv" name="prv" style="width:82px;" onblur="CheckSpace()">
                            <option id="opProvince" value="">省(全部)</option>
			                <option value="安徽">安徽</option>
			                <option value="北京">北京</option>
			                <option value="重庆">重庆</option>
			                <option value="福建">福建</option>
			                <option value="甘肃">甘肃</option>
			                <option value="广东">广东</option>
			                <option value="广西">广西</option>
			                <option value="贵州">贵州</option>
			                <option value="海南">海南</option>
			                <option value="河北">河北</option>
			                <option value="黑龙江">黑龙江</option>
			                <option value="河南">河南</option>
			                <option value="香港">香港</option>
			                <option value="湖北">湖北</option>
			                <option value="湖南">湖南</option>
			                <option value="江苏">江苏</option>
			                <option value="江西">江西</option>
			                <option value="吉林">吉林</option>
			                <option value="辽宁">辽宁</option>
			                <option value="澳门">澳门</option>
			                <option value="内蒙古">内蒙古</option>
			                <option value="宁夏">宁夏</option>
			                <option value="青海">青海</option>
			                <option value="山东">山东</option>
			                <option value="上海">上海</option>
			                <option value="山西">山西</option>
			                <option value="陕西">陕西</option>
			                <option value="四川">四川</option>
			                <option value="台湾">台湾</option>
			                <option value="天津">天津</option>
			                <option value="新疆">新疆</option>
			                <option value="西藏">西藏</option>
			                <option value="云南">云南</option>
			                <option value="浙江">浙江</option>
			                <option value="其他">其他</option>
                        </select>
                    </div>
                    <div class="ss2">
                        <select style="width:82px;" name="city" id="city">
                            <option value="" id="opCity">市(全部)</option>
                        </select>
                    </div>
                    <div class="ss3">
                        <select style="width:87px;" name="gender" id="gender" onchange="" onblur="">
                            <option value="2">性别(全部)</option>
                            <option value="1">男</option>
                            <option value="0">女</option>
                        </select>
                    </div>
                    <div class="ss4">
                        <select name="age" id="age" style="width:87px;" onchange="" onblur="">
                            <option value="0">年龄(全部)</option>
                            <option value="1">小于15岁</option>
                            <option value="2">16-22岁</option>
                            <option value="3">23-30岁</option>
                            <option value="4">31-40岁</option>
                            <option value="5">大于40岁</option>
                        </select>
                    </div>
                    <input id="NickName" type="text" name="" value="请输入用户昵称">
                    <img src="Images/search.png" onMouseOver='this.src="Images/search_hover.jpg"' onclick='this.src="Images/search_active.jpg";SearchUserList();' onMouseOut='this.src="Images/search.jpg"'>
                </div>
                <div id="hic_pic" class="hic_pic">
                    <%=strContent%>
                </div>
                <input type="hidden" id="hdpage" value="1" />
                <div class="pages">
                </div>
            </div>
            <div id="SendMessage" class="send_mes_div">
                <div class="div_off"><a href="#"><img src="Images/off_button.png" onMouseOver='this.src="Images/off_button_hover.png"' onclick='this.src="Images/off_button_active.png";HiddenSMS();' onMouseOut='this.src="Images/off_button.png"'></a></div>
                <div class="name_div"><span>收信人：</span>
                    <input type="hidden" id="hid_UserID" value="0" />
                    <input type="text" id="txt_NickName" value="0" disabled="disabled" />
                </div>
                <div class="text_div"><span>内容：</span>
                    <textarea id="Content"></textarea>
                </div>
                <div class="sub_button">
                    <img class="send_b" onclick="SendMsg();" src="Images/send_button.png" onMouseOver='this.src="Images/send_button_hover.png"'  onMouseOut='this.src="Images/send_button.png"'>
                    <img id="reset_b" onclick="reset_btn();" class="reset_b" src="Images/reset_button.png" onMouseOver='this.src="Images/reset_button_hover.png"'  onMouseOut='this.src="Images/reset_button.png"'>
                </div>
            </div>
            <div id="cmd"><% =this.strCMD %></div>
        </form>
        </div>
    </body>
</html>
