<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="Show_Web.Show" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Demo</title>
    <script type="text/javascript" src="JS/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="JS/jquery.paginate.js"></script>
    <script type="text/javascript" src="JS/jquery.timers.js"></script>
    <script src="JS/SelectList.js" type="text/javascript"></script>
    <link href="skins/tango/skin.css" rel="Stylesheet" type="text/css" />
    <link rel="Stylesheet" type="text/css" href="css/style.css" media="screen"/>
    <style type="text/css">
        body {
	        font-size: 12px;
	        color: #444;
	        font-family: "微软雅黑";
	        line-height:20px;
	        text-align:left;
	        margin: 0;
	        overflow: hidden;
	        }
        a { color:#005eac; text-decoration:none;}
        a:visited { color:#005eac;}
        a:hover { color:#ff9000; text-decoration: underline;}
        a:visited { color:#005eac;}
        div,ul,li {padding:0; margin:0;}
        img {border:0}

        .body {width:800px; height:592px; background:url(Images/bg.jpg); position:relative; margin:0 auto;}
        .title {
	        position:absolute;
	        left:23px;
	        top:63px;
	        height:24px;
	        line-height:24px;
	        font-size:18px;
	        font-weight:bold;
	        color:#fff;
	        }
        .pic1 {
	        position:absolute;
	        left:21px;
	        top:106px;
	        width:188px;
	        height:181px;
	        background:url(Images/picbg.png);
	        }
        .pic2 {
	        position:absolute;
	        left:213px;
	        top:106px;
	        width:188px;
	        height:181px;
	        background:url(Images/picbg.png);
	        }
        .pic3 {
	        position:absolute;
	        left:405px;
	        top:106px;
	        width:188px;
	        height:181px;
	        background:url(Images/picbg.png);
	        }
        .pic4 {
	        position:absolute;
	        left:595px;
	        top:106px;
	        width:188px;
	        height:181px;
	        background:url(Images/picbg.png);
	        }

        .pic5 {
	        position:absolute;
	        left:21px;
	        top:290px;
	        width:188px;
	        height:181px;
	        background:url(Images/picbg.png);
	        }
        .pic6 {
	        position:absolute;
	        left:213px;
	        top:290px;
	        width:188px;
	        height:181px;
	        background:url(Images/picbg.png);
	        }
        .pic7 {
	        position:absolute;
	        left:405px;
	        top:290px;
	        width:188px;
	        height:181px;
	        background:url(Images/picbg.png);
	        }
        .pic8 {
	        position:absolute;
	        left:595px;
	        top:290px;
	        width:188px;
	        height:181px;
	        background:url(Images/picbg.png);
	        }

        .headpic {
	        position:absolute;
	        left:17px;
	        top:16px;
	        width:149px;
	        height:115px;
	        border:1px solid #9d9a78;
	        }
        .headname {
	        position:absolute;
	        left:18px;
	        top:140px;
	        width:80px;
	        height:20px;
	        }
        .headtime {
	        position:absolute;
	        left:128px;
	        top:140px;
	        width:40px;
	        height:20px;
	        text-align:right
	        }
	    .sms{
	        position:absolute;
	        left:100px;
	        top:139px;
	        }
        .concern{
	        position:absolute;
	        left:120px;
	        top:139px;
	        }
        #page {
	        position:absolute;
	        left:23px;
	        top:475px;
	        width:760px;
	        height:20px;
	        }
        .pagebutton {
	        float:left;
	        clear:right;
	        width:15px;
	        height:15px;
	        background:url(Images/b1.png);
	        }
        .pg2 {
	        float:left;
	        clear:right;
	        width:15px;
	        height:15px;
	        background:url(Images/b2.png);
	        }
        .friendlist{
	        position:absolute;
	        left:40px;
	        top:487px;	/*top:487px;*/
	        }
        .headbg {
	        width:61px;
	        height:78px;
	        background:url(Images/headbg.png);
	        float:left;
	        margin-left:4px;
	        line-height:16px;
	        text-align:center;
	        }
        .headbg img{
	        margin:5px 0 0 -1px;
	        }
    </style>
    
    <script type="text/javascript">
     var intUserID = <% =this.strUserID %>
     //$()
     function Timer1() {
         $("body").everyTime('4s',vGetRand);     //文档全部载入时运行
     }
         
     function Timer2() {
         $("body").everyTime('4s',ShowConcern);     //文档全部载入时运行
     }
     
     function vGetRand()
     {
         var p = $("#hdpage").val();
         $.get("Refresh.aspx",
            {
                rnd:Math.random(),
                page:p,
                UserID:intUserID
            },
	        function(data){
		        $("#ShowImg").html(data)
	        });
     }
     
     function vGetRands(page)
     {         
         $.get("Refresh.aspx",
            {
                rnd:Math.random(),
                page:page,
                UserID:intUserID
            },
	        function(data){
		        $("#ShowImg").html(data)
	        });
     }  
     
     function ShowConcern()
     {
        $("body").stopTime();
        Timer2();
        $.get("ConcernList.aspx?page=1&UserID=" + intUserID + "&rnd=" + Math.random(),
	        function(data){
		        $("#ShowImg").html(data)
	        });
	    ConcernPage();
     }
     
     function vShowConcern()
     {
        $("body").stopTime();
        $.get("ConcernList.aspx?page=1&UserID=" + intUserID + "&rnd=" + Math.random(),
	        function(data){
		        $("#ShowImg").html(data)
	        });
     }
     
     function ShowRoom()
     {        
        $("#hdpage").attr('value', 1);
        vGetRand();
        RoomPage();
        $("body").stopTime();
        $("body").everyTime('4s',vGetRand);
     }
     
     function SearchUserList() {
        $("body").stopTime();
        //alert($("#prv").find("option:selected").text());
        //alert($("#city").find("option:selected").text());
        //alert($("#gender").val());
        //alert($("#age").val());
        var strProvince = $("#prv").find("option:selected").text();
        var strCity = $("#city").find("option:selected").text();
        var strGender = $("#gender").val();
        var strCategory = $("#age").val();
        var strNickName = $("#NickName").val();
        
        if(jQuery.trim(strNickName).length > 0)
        {
            $.get("Search.aspx?type=2&UserID=" + intUserID + "&NickName=" + encodeURI(strNickName) + "&rnd=" + Math.random(),
            function (data) {
                $("#ShowImg").html(data) 
            });
        }
        else
        {
            if($("#prv").find("option:selected").text() == "— —省")
            {
                alert("没先省就开始查啊");
                return;
            }
            
            if($("#city").find("option:selected").text() == "— —市")
            {
                alert("没先市就开始查啊");
                return;
            }
            
            $.get("Search.aspx?type=1&page=1&UserID=" + intUserID + "&Province=" + encodeURI(strProvince) + "&City=" + encodeURI(strCity) + "&Gender=" + encodeURI(strGender) + "&Category=" + encodeURI(strCategory) + "&rnd=" + Math.random(),
                function (data) {
                    $("#ShowImg").html(data) 
                });
        }
     }
     
     function SearchUser() {
        $("body").stopTime();
        var strCategory = $("#Category").val();
        
        $.get("Search.aspx?type=2&NickName=" + encodeURI(strCategory) + "&rnd=" + Math.random(),
            function (data) {
                $("#ShowImg").html(data) 
            });
     }
     
     function HiddenSMS() {
        $("#SendMessage").css("display","none");
     }
     
     function ShowMsgBox(UserID,NickName) {
        $("#SendMessage").css("display","block");        
        $("#hid_UserID").attr("value", UserID);
        $("#txt_NickName").attr("value", NickName);
        $("#Content").html("");
     }
     
     function SendMsg() {
        var intUserIDs = $("#hid_UserID").val();
        var intSendID = intUserID;//测试数据
        var strSender = $("#txt_NickName").val();  //      
        var strContent = $("#Content").val();
        strContent = jQuery.trim(strContent);
        if(intUserIDs > 0)
        {
            if(intUserIDs == intSendID)
            {
                alert("不能给自己发消息");
                return;
            }
            if(strContent.length == 0)
            {
                alert("消息内容不能为空");
                return;
            }
            $.getJSON("MessageBox.aspx",
                {
                    Type:2,
                    UserID:intUserIDs,
                    SendID:intSendID,
                    Sender:encodeURI(strSender),
                    Content:encodeURI(strContent)
                },
	            function(data){	                
		            if(data.Status == 1)
		            {
		                alert("发送成功");
		                $("#SendMessage").css("display","none");
		            }
		            else
		            {
		                alert("发送失败");
		                $("#SendMessage").css("display","none");
		            }		            
	            });
        }
     } 
     
     //添加关注
     function AddConcern(intUserID,intConcernID) {
        alert(intUserID + "," + intConcernID);
        $.getJSON("ConcernOp.aspx",
            {
                Type:1,
                UserID:intUserID,
                ConcernID:intConcernID,
                rnd:Math.random()
            },
            function (data) {
                if(data.Status == 1)
                {
                    alert("添加关注成功");
                }
                else
                {
                    alert("不能重复添加关注");
                }
            });
     }  
     
     //删除关注
     function DeleteConcern(intUserID,intConcernID) {
        $.getJSON("ConcernOp.aspx",
            {
                Type:2,
                UserID:intUserID,
                ConcernID:intConcernID,
                rnd:Math.random()
            },
            function (data) {
                if(data.Status == 1)
                {
                    alert("删除关注成功");
                }
                else
                {
                    alert("不能重复删除关注");
                }
            });
     }
     
     function CheckSpace() {
         var strProvince=$("#HidProvince").val();
         var strCity=$("#hddCity").val();
     }
     
     //设置关注显示
    function ConcernPage() {
        var pageSize = 0;
        $.getJSON("Page.aspx",
        {
            Type:2,
            UserID:intUserID,
            rnd:Math.random()
        },
        function (data) {
            pageSize = data.Page;
            if(pageSize > 0)
            {
                $("#page").paginate({
                    count 		: pageSize,
                    start 		: 1,
                    display     : 12,
                    border					: false,
                    text_color  			: '#79B5E3',
                    background_color    	: 'none',	
                    text_hover_color  		: '#2573AF',
                    background_hover_color	: 'none', 
                    images					: false,
                    mouse					: 'press',
                    onChange				: function(pages){
							                    $.get("ConcernList.aspx?page=" + pages + "&UserID=" + intUserID + "&rnd=" + Math.random(),
							                        function(data){
								                        $("#ShowImg").html(data)
							                        });
							                    $("#hdpage").attr('value', pages);
						                      }
                });
            }
            else
            {
                $("#page").html("");
            }
        });
    }
    </script>   

</head>
<body>
<div class="body">
    <form id="Show_Form" runat="server">
        <div id="page"></div>
        <div id="ShowImg" runat="server">
            <%=strContent%>
        </div>
        <input type="hidden" id="hdpage" value="1" />
    </form>
    <div style="position:absolute; left:17px; top:593px;"><a href="http://ishow.xba.com.cn/image/update/PlayCap.rar">点击此处下载客户端</a></div>
    <div style="position:absolute; left:167px; top:593px;"><a href="javascript:;" onclick="ShowRoom()">大厅</a></div>
    <div style="position:absolute; left:217px; top:593px;"><a href="javascript:;" onclick="ShowConcern()">关注</a></div>
    <div style="position:absolute; left:267px; top:593px;"><a href="javascript:;" onclick="setpage()">test</a></div>
    <div style="position:absolute; left:25px; top:64px;" id="Search">
        <input type="hidden" id="hddCity" name="hddCity" value="" />
        <input type="hidden" id="HidProvince" name="HidProvince" value="" />
        <input type="hidden" id="Hidcity2" name="Hidcity2" />
        <select onchange="setcity(0,'')" id="prv" name="prv" style="width:97px;" onblur="CheckSpace()">
            <option id="opProvince" value="">— —省</option>
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
        <select name="city" id="city" style="width:97px;" onchange="ChangeCity(this.value)" onblur="CheckSpace()">
            <option value="" id="opCity">— —市</option>
        </select>
        <select name="gender" id="gender" style="width:60px;" onchange="" onblur="">
            <option value="2">全部</option>
            <option value="1">男</option>
            <option value="0">女</option>
        </select>
        <select name="age" id="age" style="width:97px;" onchange="" onblur="">
            <option value="0">全部</option>
            <option value="1">小于15岁</option>
            <option value="2">16-22岁</option>
            <option value="3">23-30岁</option>
            <option value="4">31-40岁</option>
            <option value="5">大于40岁</option>
        </select>
        <input type="text" id="NickName" />
        <input type="button" value="test" onclick="SearchUserList()" />
    </div>
    <div id="SendMessage" style="position:absolute; left:281px; top:229px; background-color:#99C; width:223px; height:163px; display:none;">
        <input type="hidden" id="hid_UserID" value="0" />
        <input type="text" id="txt_NickName" value="0" disabled="disabled" />
        <textarea rows="5" cols="20" id="Content" ></textarea>
        <input type="button" value="Send" onclick="SendMsg()" />
        <input type="button" value="Cancel" onclick="HiddenSMS()" />
    </div>
    <div id="cmd"><% =this.strCMD %></div>
</div>
<script type="text/javascript" language="javascript">
    var pagecount = <% =this.intPageSize %>;
    $("#page").paginate({
		    count 		: pagecount,
		    start 		: 1,
		    display     : 12,
		    border					: false,
		    text_color  			: '#79B5E3',
		    background_color    	: 'none',	
		    text_hover_color  		: '#2573AF',
		    background_hover_color	: 'none', 
		    images					: false,
		    mouse					: 'press',
		    onChange				: function(pages){
									    $.get("Refresh.aspx?page=" + pages + "&UserID=" + intUserID + "&rnd=" + Math.random(),
									        function(data){
										        $("#ShowImg").html(data)
									        });
									    $("#hdpage").attr('value', pages);
								      }
	    });
	    
	    //设置大厅显示
	    function RoomPage() {
	        var pageSize = 0;
	        $.getJSON("Page.aspx",
            {
                Type:1,
                rnd:Math.random()
            },
            function (data) {
                pageSize = data.Page;
                if(pageSize > 0)
                {
                    $("#page").paginate({
		                count 		: pageSize,
		                start 		: 1,
		                display     : 12,
		                border					: false,
		                text_color  			: '#79B5E3',
		                background_color    	: 'none',	
		                text_hover_color  		: '#2573AF',
		                background_hover_color	: 'none', 
		                images					: false,
		                mouse					: 'press',
		                onChange				: function(pages){
									                $.get("Refresh.aspx?page=" + pages + "&UserID=" + intUserID + "&rnd=" + Math.random(),
									                    function(data){
										                    $("#ShowImg").html(data)
									                    });
									                $("#hdpage").attr('value', pages);
								                  }
	                });
                }
            }); 
	    }	    
	    
</script>
</body>
</html>
