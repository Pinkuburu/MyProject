<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Show_Web.Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Register</title>
    <style type="text/css">
        body {
	        margin:0 auto;
        }
        body,td,th {
	        font-size: 12px;
	        color: #484848;
	        font-family: 宋体;
        }
        a:link { text-decoration:none;color:#1a56dd}
        a:visited { text-decoration:none;color:#1a56dd}
        a:hover {color:#f76907; text-decoration:underline;}
        
        .regTable{margin-top:20px;}
        .regTable tr{line-height:35px;}
        .regTd1{width:150px; padding-right:4px;text-align:right;}
        .regTd2{text-align:left;}
        .RegfTxt{width:195px; height:15px;}
        /* 提示框样式 */
        div.mask {
	        position: absolute;
	        z-index: 99999;
	        float: left;
	        top: 0px;
	        left: 0px;
	        bottom: 0px;
	        right: 0px;
	        filter:alpha(opacity=50);
		    filter: progid:DXImageTransform.Microsoft.Alpha(opacity=30);
		    -moz-opacity:0.8; 
	        opacity: 0.05;
	        width: 100%;
	        height: 1000px;
        }
        div.alhpa {
	        filter:alpha(opacity=50);
	        opacity: 0.05;
        }
        #dialog_box, .dialog_box {
	        position: absolute;
	        z-index: 999;
	        /*border-collapse: collapse;
	        border-top: 0px solid #F7FCF1;
	        border-left: 0px solid #F7FCF1;
	        border-bottom: 0px solid #DBE8C8;
	        border-right: 0px solid #DBE8C8;*/
        }

        * html div.mask {
	        width: 100%;
	        height: 1000px;
	        padding: 0 24px 46px 0;
		-moz-opacity:0.8; 
        }
        iframe.mask {
	        position: absolute;
	        filter:alpha(opacity=50);
		filter: progid:DXImageTransform.Microsoft.Alpha(opacity=30);
		-moz-opacity:0.8; 
	        opacity: 0.05;
	        z-index: 99999;
	        float: left;
	        top: 0px;
	        left: 0px;
	        bottom: 0px;
	        right: 0px;
	        width: 100%;
	        height: 1000px;
		background-color:#CCCCCC;
		
        }
        * html iframe.mask {
	        width: 100%;
	        height: 1000px;
	        padding: 0 24px 46px 0;
		
        }

        .dialog_box_help {
	        position: absolute;
	        z-index: 99999;
	        top: 135px;
	        right: 20px;
	        border: 1px solid #688186;
	        background:#005f91 url(/Images/dialogboxtopbg.png) no-repeat;
	        padding: 2px;
        }

        .dialog_box_help .content {
	        border: 1px solid #aab7ba;
        }

        .dialog_box_help .title {
	        font-weight: bold;
	        color:#fff;
	        cursor: move;
	        background:url(/Images/icon-help.png) no-repeat 3px 4px;
	        padding: 4px 45px 3px 25px;
        }

        .dialog_close {
	        position: absolute;
	        top: 1px;
	        right: 2px;
	        float: right;
	        width: 18px;
	        height: 18px;
	        cursor: pointer;
	        background: transparent url(/Images/Normal/icon-close.gif) no-repeat center;
        }
        .dialog_mini {
	        position: absolute;
	        top: 5px;
	        right: 23px;
	        float: right;
	        width: 18px;
	        height: 18px;
	        cursor: pointer;
	        background: transparent url(/Images/icon-mini.gif) no-repeat center;
        }
        .dialog_max {
	        position: absolute;
	        top: 5px;
	        right: 23px;
	        float: right;
	        width: 18px;
	        height: 18px;
	        cursor: pointer;
	        background: transparent url(/Images/icon-max.gif) no-repeat center;
        }
    </style>
    <script src="JS/ajax.js" type="text/javascript"></script>
    <script src="JS/common.js" type="text/javascript"></script>
    <script src="JS/SelectList.js" type="text/javascript"></script>
    <script src="JS/game.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        var intIsChild = 0;
        
        function YYYYMMDDstart()
        {
            MonHead = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

            //先给年下拉框赋内容            
            var ys = new Date().getFullYear();//初始年份
            var y = 1984;
            //以今年为准，前30年，后30年
            for (var i = (ys-80); i < (ys); i++) 
            {
                document.getElementById("YYYY").options.add(new Option(" "+ i +" ", i));
            }

            //赋月份的下拉框
            for (var i = 1; i < 13; i++)
            {
                document.getElementById("MM").options.add(new Option(" " + i + " ", i));
            }

            document.getElementById("YYYY").value = y;
            document.getElementById("MM").value = 1;//初始月份new Date().getMonth() +
         
            var n = MonHead[new Date().getMonth()];
            if (new Date().getMonth() ==1 && IsPinYear(y)) n++;
            writeDay(31); //赋日期下拉框Author:meizz
            document.getElementById("DD").value = 1;//初始日期new Date().getDate()
        }
        
        function YYYYDD(str) //年发生变化时日期发生变化(主要是判断闰平年)
        {
            var MMvalue = document.getElementById("MM").options[document.getElementById("MM").selectedIndex].value;
            if (MMvalue == "")
            { 
                var e = document.getElementById("DD"); 
                optionsClear(e); 
                return;
            }
            var n = MonHead[MMvalue - 1];
            if (MMvalue ==2 && IsPinYear(str)) n++;
            writeDay(n)
        }
        
        function MMDD(str)  //月发生变化时日期联动
        {
            var YYYYvalue = document.getElementById("YYYY").options[document.getElementById("YYYY").selectedIndex].value;
            if (YYYYvalue == "")
            { 
                var e = document.getElementById("DD"); 
                optionsClear(e); 
                return;
            }
            var n = MonHead[str - 1];
            if (str ==2 && IsPinYear(YYYYvalue)) n++;
            writeDay(n)
        }
        
        
        function writeDay(n)  //据条件写日期的下拉框
        {
            var e = document.getElementById("DD"); optionsClear(e);
           for (var i=1; i<(n+1); i++)
                e.options.add(new Option(" "+ i + " ", i));
        }

        function IsPinYear(year)//判断是否闰平年
        {   
            return(0 == year%4 && (year%100 !=0 || year%400 == 0));
        }
        function optionsClear(e)
        {
            for (var i=e.options.length; i>0; i--)
                e.remove(i);
        }
        //
        function CheckInputLength(obj,intLenght,e)
        {
            var strValue='';
            if(obj!=null) strValue=obj.value;
	        var Len = strValue.replace(/([^\x00-\xff])/g,"#$").length;
	        if ( Len > intLenght )
	        {
	            var strCode=obj.value;
	            var strSubEndValue=strValue.replace(/([^\x00-\xff])/g,"#$");
	            if(strSubEndValue.substring(intLenght-1,intLenght)!="#")
	                strSubEndValue=strSubEndValue.substring(0,intLenght);
	            else
	                strSubEndValue=strSubEndValue.substring(0,intLenght+1);
	            var intCount=0;
	            var intIndex=1;
	            var intError=1000;
	            while(intIndex>=0 && intError>0)
	            {
	                intIndex=strSubEndValue.indexOf("#$");
	                if(intIndex>=0)
	                {
	                    strSubEndValue=strSubEndValue.substring(intIndex+2);
	                    intCount++;
	                }
	                intError--;
	            }
		        obj.value=strCode.substring(0,intLenght-intCount);
	        }
        }
    </script>
</head>
<body onload="YYYYMMDDstart()">
    <form id="form1" runat="server" accept="btnRegNext">
        <div>
            <table border="0" cellpadding="0" cellspacing="0" class="regTable">
                <tr>
                    <td style="width:150px; padding-right:4px;text-align:right; color:Red;">
                        必填选项
                    </td>
                    <td class="regTd2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="regTd1">
                        用户名：
                    </td>
                    <td class="regTd2">
                        <input type="text" id="tbUserName" class="RegfTxt" onblur="HasUserName()" />
                        &nbsp;&nbsp;
                        <span style="color:#898989;" id="divUserName">用户名由6-16个英文字母或数字组成（不支持中文），一旦注册成功，不可修改。</span>
                    </td>
                </tr>
                <tr>
                    <td class="regTd1">
                        密码：
                    </td>
                    <td class="regTd2">
                        <input type="password" id="tbPassword" class="RegfTxt" onblur="CheckPassword()" />
                        &nbsp;&nbsp;
                        <span style="color:#898989;" id="divPassword">密码由6-16个英文字母或数字组成（不支持中文）。</span>
                    </td>
                </tr>
                <tr>
                    <td class="regTd1">
                        确认密码：
                    </td>
                    <td class="regTd2">
                        <input type="password" id="tbRePassword" class="RegfTxt" onblur="LeaveRePassword()" />
                        &nbsp;&nbsp;
                        <span style="color:#898989;" id="divRePassword">请再输入一遍上面的密码。</span>
                    </td>
                </tr>
                <tr>
                    <td class="regTd1" style=" padding-top:12px;">
                        昵称：
                    </td>
                    <td class="regTd2" style=" padding-top:12px;">
                        <input type="text" id="tbNickName" class="RegfTxt" onkeyup="CheckInputLength(this,20,event)" onblur="HasNickName()" />&nbsp;&nbsp;
                        <span style="color:#898989;" id="divNickName">用户角色名称，支持中文</span>
                    </td>
                </tr>
                <tr>
                    <td class="regTd1">
                        性别：
                    </td>
                    <td class="regTd2">
                        <input type="radio" id="rdMan" name="rdGender" checked="checked" onclick="document.getElementById('ddlGender').value=1;" />男
                        <input type="radio" id="rdWoman" name="rdGender" onclick="document.getElementById('ddlGender').value=0;"/>女
                        <input type="hidden" id="ddlGender" value="1" />&nbsp;&nbsp;
                        <span style="color:#898989;" id="divGender">
                        </span>
                    </td>
                </tr>
                <tr>
                    <td class="regTd1">
                        生日：
                    </td>
                    <td class="regTd2">
                        <select name="YYYY" id="YYYY" style="width:60px;" onchange="YYYYDD(this.value)" onblur="CheckBirthDay()">
                            <option value="">
                                --
                            </option>
                        </select>
                        年
                        <select name="MM" id="MM" onchange="MMDD(this.value)" style="width:46px;" onblur="CheckBirthDay()">
                            <option value="">
                                --
                            </option>
                        </select>
                        月
                        <select name="DD" id="DD" onblur="CheckBirthDay()" style="width:46px;">
                            <option value="">
                                --
                            </option>
                        </select>
                        日 &nbsp;&nbsp;
                        <span style="color:#898989;" id="divBirthDay"></span>
                    </td>
                </tr>
                <tr>
                    <td class="regTd1">
                        地区：
                    </td>
                    <td class="regTd2">
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
                            <option value="" id="opCity">
                                — —市
                            </option>
                        </select>
                        &nbsp;&nbsp;
                        <span style="color:#898989;">选填项，帮助您更方便找到同城玩家</span>
                    </td>
                </tr>
                <tr>
                    <td style="line-height:20px;" class="regTd1">
                        验证码：
                    </td>
                    <td class="regTd2">
                        <input id="txtCheckVCode" class="RegfTxt" />&nbsp;&nbsp;
                        <img id="CreateVcode" src="CreateVcode.aspx" width="63" height="22" align="absmiddle" alt="验证码" />&nbsp;&nbsp;
                        <a href="#" onclick="ChangeVcode();">看不清换一个</a> &nbsp;&nbsp;
                        <span style="color:#898989;" id="divVcode"></span>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <input type="button" style="width:100px; line-height:24px;" value="完成注册" title="完成注册" onclick='RegEnd()' id="btnRegNext" />&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>                
            </table>
        </div>
        <script type="text/javascript" language="javascript">
            //判断地址
            function CheckSpace()
            {
                var strProvince=document.getElementById("HidProvince").value;
                var strCity=document.getElementById("hddCity").value;
                loader.get ( '/RegOp.aspx?Type=Pro&Province='+strProvince+'&City='+strCity, null, null, null, OnCompleteSpace );
            }
            
            function OnCompleteSpace(value)
            {
                if(value==1)
                {
                    if(document.getElementById("divSpace").style.color=="#588526")
                    {
                        document.getElementById("divSpace").style.color="#a9a9a9";
                    }
                    else
                    {
                        document.getElementById("divSpace").style.color="#588526";
                    }
                    document.getElementById("divSpace").innerHTML=" 请认真填写您的所在地信息.";
                }
                else if(value==-1)
                {
                    document.getElementById("divSpace").style.color="#f20505";
                    document.getElementById("divSpace").innerHTML=" <img src=\"Images/Error.gif\" alt=\"\" style=\"border:0px; padding-right:6px;\" align='absmiddle' />请选择省份.";
                }
                else if(value=-2)
                {
                    document.getElementById("divSpace").style.color="#f20505";
                    document.getElementById("divSpace").innerHTML=" <img src=\"Images/Error.gif\" alt=\"\" style=\"border:0px; padding-right:6px;\" align='absmiddle' />请选择城市.";
                }
            }
            //判断用户名    
            function HasUserName()
            {
                if(document.getElementById("tbUserName").value!="")
                {
                    document.getElementById("divUserName").innerHTML="检测中...";
                    HasUserNameR();
                }
                else
                {
                    document.getElementById("divUserName").style.color="#a9a9a9";
                    document.getElementById("divUserName").innerHTML="6-16个英文字母或数字组成,不可修改.";
                }
            }
            
            function HasUserNameR()
            {
                var strUserName=document.getElementById("tbUserName").value;
                loader.get ( '/RegOp.aspx?Type=UserName&UserName='+strUserName, null, null, null, OnCompleteUN );
            }
            
            function OnCompleteUN(value)
            {
                if(value==1)
                {
                    document.getElementById("divUserName").style.color="#898989";
                    document.getElementById("divUserName").innerHTML="用户名可以正常使用";
                }
                else if(value==-1)
                {
                    document.getElementById("divUserName").style.color="#f20505";
                    document.getElementById("divUserName").innerHTML=" <img src=\"Images/Error.gif\" alt=\"\" style=\"border:0px; padding-right:6px;\" align='absmiddle' />用户名已存在";
                }
                else if(value==-2)
                {
                    document.getElementById("divUserName").style.color="#f20505";
                    document.getElementById("divUserName").innerHTML=" <img src=\"Images/Error.gif\" alt=\"\" style=\"border:0px; padding-right:6px;\" align='absmiddle' />应由6-16个英文字母或数字组成.";
                }
            }
            
            //判断重复密码
            function LeaveRePassword()
            {
                if(document.getElementById("tbRePassword").value=="")
                {
                    document.getElementById("divRePassword").style.color="#a9a9a9";
                    document.getElementById("divRePassword").innerHTML=" 请再输入一遍上面的密码.";
                }
                else
                {
                    var strPassword=document.getElementById("tbPassword").value;
                    var strRePassword=document.getElementById("tbRePassword").value;
                    loader.get ( '/RegOp.aspx?Type=RePassword&Password='+strPassword+'&RePassword='+strRePassword, null, null, null, OnCompleteLRPW );
                }
            }
            
            function OnCompleteLRPW(value)
            {
                if(value==2)
                {
                    document.getElementById("divRePassword").style.color="#898989";
                    document.getElementById("divRePassword").innerHTML=" 重复密码填写正确.";
                }
                else if(value==-1)
                {
                    document.getElementById("divRePassword").style.color="#a9a9a9";
                    document.getElementById("divRePassword").innerHTML=" 请再输入一遍上面的密码.";
                }
                else if(value==-2)
                {
                    document.getElementById("divRePassword").style.color="#f20505";
                    document.getElementById("divRePassword").innerHTML=" <img src=\"Images/Error.gif\" alt=\"\" style=\"border:0px; padding-right:6px;\" align='absmiddle' />两次输入密码不同,请再输入一遍.";
                }
            }
            
            //判断密码
            function CheckPassword()
            {
                if(document.getElementById("tbPassword").value=="")
                {
                    document.getElementById("divPassword").style.color="#a9a9a9";
                    document.getElementById("divPassword").innerHTML=" 密码由6-16个英文字母或数字组成.";
                }
                else
                {
                    var strPassword=document.getElementById("tbPassword").value;
                    var strRePassword=document.getElementById("tbRePassword").value;
                    loader.get ( '/RegOp.aspx?Type=Password&Password='+strPassword+'&RePassword='+strRePassword, null, null, null, OnCompleteLPW );
                }
            }
            
            function OnCompleteLPW(value)
            {
                if(value==1)
                {
                    document.getElementById("divPassword").style.color="#898989";
                    document.getElementById("divPassword").innerHTML=" 密码可以正常使用.";
                }
                else if(value==-1)
                {
                    document.getElementById("divPassword").style.color="#f20505";
                    document.getElementById("divPassword").innerHTML=" <img src=\"Images/Error.gif\" alt=\"\" style=\"border:0px; padding-right:6px;\" align='absmiddle' />您填写有误,密码由6-16个英文字母或数字组成.";
                }
                else if(value==-2)
                {
                    document.getElementById("divRePassword").style.color="#f20505";
                    document.getElementById("divRePassword").innerHTML=" <img src=\"Images/Error.gif\" alt=\"\" style=\"border:0px; padding-right:6px;\" align='absmiddle' />两次输入密码不同，请重新输入上面的密码.";
                    document.getElementById("divPassword").style.color="#898989";
                    document.getElementById("divPassword").innerHTML=" 密码可以正常使用.";
                }
                else if(value==2)
                {
                    document.getElementById("divRePassword").style.color="#898989";
                    document.getElementById("divRePassword").innerHTML=" 重复密码填写正确.";
                    document.getElementById("divPassword").style.color="#898989";
                    document.getElementById("divPassword").innerHTML=" 密码可以正常使用.";
                }
            }
            
            //判断经理名
            function HasNickName()
            {
                if(document.getElementById("tbNickName").value!="")
                {
                    document.getElementById("divNickName").innerHTML="检测中...";
                    HasNickNameR();
                }
                else
                {
                    document.getElementById("divNickName").style.color="#a9a9a9";
                    document.getElementById("divNickName").innerHTML="用户角色名称，支持中文.";
                }
            }
            
            function HasNickNameR()
            {
                 var strNickName=encodeURI(document.getElementById("tbNickName").value);
                loader.get ( '/RegOp.aspx?Type=Nickname&Nickname='+strNickName, null, null, null, OnCompleteNNC );
            }
            
            function OnCompleteNNC(value)
            {
                if(value==1)
                {
                    document.getElementById("divNickName").style.color="#898989";
                    document.getElementById("divNickName").innerHTML="昵称可以正常使用";
                }
                else if(value==-1)
                {
                    document.getElementById("divNickName").style.color="#f20505";
                    document.getElementById("divNickName").innerHTML=" <img src=\"Images/Error.gif\" alt=\"\" style=\"border:0px; padding-right:6px;\" align='absmiddle' />昵称已存在";
                }
                else if(value==-2)
                {
                    document.getElementById("divNickName").style.color="#f20505";
                    document.getElementById("divNickName").innerHTML=" <img src=\"Images/Error.gif\" alt=\"\" style=\"border:0px; padding-right:6px;\" align='absmiddle' />昵称输入错误或含有非法字符.";
                }
            }
            
            //刷新激活码
            function ChangeVcode()
            {
                var Rand=parseInt(Math.random()*10);
                document.getElementById("CreateVcode").src="CreateVcode.aspx?Parameter="+Rand;
            }
            
            //判断生日
            function CheckBirthDay()
            {
                var strYear=document.getElementById("YYYY").value;
                var strMonth=document.getElementById("MM").value;
                var strDay=document.getElementById("DD").value;
                loader.get ( '/RegOp.aspx?Type=Birthday&Year='+strYear+'&Month='+strMonth+'&Day='+strDay, null, null, null, OnCompleteBir );
            }
            
            function OnCompleteBir(value)
            {
                if(value==1)
                {
                    if(document.getElementById("divBirthDay").style.color=="#588526")
                    {
                        document.getElementById("divBirthDay").style.color="#a9a9a9";
                    }
                    else
                    {
                        document.getElementById("divBirthDay").style.color="#a9a9a9";
                    }
                    document.getElementById("divBirthDay").innerHTML="生日信息格式正确.";
                }
                else if(value==-1)
                {
                    document.getElementById("divBirthDay").style.color="#f20505";
                    document.getElementById("divBirthDay").innerHTML=" <img src=\"Images/Error.gif\" alt=\"\" style=\"border:0px; padding-right:6px;\" align='absmiddle' />年份填写不正确.";
                }
                else if(value==-2)
                {
                    document.getElementById("divBirthDay").style.color="#f20505";
                    document.getElementById("divBirthDay").innerHTML=" <img src=\"Images/Error.gif\" alt=\"\" style=\"border:0px; padding-right:6px;\" align='absmiddle' />月份填写不正确.";
                }
                else if(value==-3)
                {
                    document.getElementById("divBirthDay").style.color="#f20505";
                    document.getElementById("divBirthDay").innerHTML=" <img src=\"Images/Error.gif\" alt=\"\" style=\"border:0px; padding-right:6px;\" align='absmiddle' />日期填写不正确.";
                }
            }
            
            function RegEnd()
            {   
                var strUserName=$('tbUserName').value;
                var strPassword=$('tbPassword').value;
                var strRePassword=$('tbRePassword').value;
                //var strEmail=document.getElementById("tbEmail").value;
                var strNickName=encodeURI(document.getElementById("tbNickName").value);
                //alert(strNickName);
                var strGender=document.getElementById("ddlGender").value;
                var strYear=document.getElementById("YYYY").value;
                var strMonth=document.getElementById("MM").value;
                var strDay=document.getElementById("DD").value;
                var strProvince=encodeURI(document.getElementById("HidProvince").value);
                var strCity=encodeURI(document.getElementById("hddCity").value);
                //var strIntro="";
                //var strQQ="";
                //var strMSN="";
                //var strcbReg=document.getElementById("cbReg").checked;
                var strVCode=$('txtCheckVCode').value;//document.getElementById("txtCheckVCode").value;
                //var strCardIdEnd=document.getElementById('txtCardId').value;
                //var strRName=document.getElementById('txtRealName').value;
	            var strProvince2 = "";
                var strCity2 = "";
                if(strProvince == "")
                {
                    strProvince2 = "--";
                }
                else
                {
                    strProvince2 = strProvince;
                }
                 if(strCity == "")
                {
                    strCity2 = "--";
                }
                else
                {
                    strCity2=strCity;
                }
                //alert(strGender);
                dialog.open("/RegOp.aspx?Type=Reg&UserName="+strUserName+"&Password="+strPassword+"&RePassword="+strRePassword+"&Nickname="+strNickName
                +"&Gender="+strGender+"&Year="+strYear+"&Month="+strMonth+"&Day="+strDay+"&Province="+strProvince+"&City="+strCity+"&VCode="+strVCode);
                //alert("/RegOp.aspx?Type=Reg&UserName="+strUserName+"&Password="+strPassword+"&RePassword="+strRePassword+"&Nickname="+strNickName
                //+"&Gender="+strGender+"&Year="+strYear+"&Month="+strMonth+"&Day="+strDay+"&Province="+strProvince+"&City="+strCity+"&VCode="+strVCode);
                //alert(strGender);
                //dialog.open("/Reg/RegOp.aspx?Type=Reg&UserName="+strUserName+"&Password="+strPassword+"&RePassword="+strRePassword+"&Email="+strEmail+"&Nickname="+strNickName
                //+"&Gender="+strGender+"&Year="+strYear+"&Month="+strMonth+"&Day="+strDay+"&Province="+strProvince+"&City="+strCity+"&Intro="+strIntro
                //+"&QQ="+strQQ+"&MSN="+strMSN+"&VCode="+strVCode+"&CardId="+strCardIdEnd+"&cbReg="+strcbReg+"&RealName="+strRName);
            }
        </script>
    </form>
</body>
</html>
