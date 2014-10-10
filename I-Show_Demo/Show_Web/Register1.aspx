<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register1.aspx.cs" Inherits="Show_Web.Register1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Register</title>
    <link rel="Stylesheet" type="text/css" href="css/reg.css" media="screen"/>
    <link rel="Stylesheet" type="text/css" href="css/dialog.css" media="screen"/>    
    <script src="JS/dialog_reg.js" type="text/javascript"></script>
    <script src="JS/ajax.js" type="text/javascript"></script>
    <script src="JS/common.js" type="text/javascript"></script>
    <script src="JS/game.js" type="text/javascript"></script>
    <script src="JS/reg.js" type="text/javascript"></script>
    <script src="JS/SelectList.js" type="text/javascript"></script>
    <script src="JS/jquery-1.4.2.min.js" type="text/javascript"></script>    
    <script type="text/javascript">
        function ShowDialog_s(title,content){
            dialog_reg(title,"text:" + content,"200px","auto","text");
        }
    </script>
</head>
<body onload="YYYYMMDDstart()" scroll="no">
    <div class="register1">
        <form id="form1" runat="server" accept="btnRegNext" style="position:relative;margin:0 auto;width:514px;height:382px;">           
            <div class="content">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="aa">用户名：</td>
                        <td class="ab"><input type="text" id="tbUserName" class="RegfTxt" onblur="HasUserName()" /></td>
                        <td class="ac" id="divUserName">6-16个英文字母或数字组成,字母区分大小写</td>
                    </tr>
                    <tr>
                        <td class="aa">密码：</td>
                        <td class="ab"><input type="password" id="tbPassword" class="RegfTxt" onblur="CheckPassword()" /></td>
                        <td class="ac" id="divPassword">密码长度6-16位，字母区分大小写</td>
                    </tr>
                    <tr>
                        <td class="aa">确认密码：</td>
                        <td class="ab"><input type="password" id="tbRePassword" class="RegfTxt" onblur="LeaveRePassword()" /></td>
                        <td class="ac" id="divRePassword">请再输入一遍上面的密码</td>
                    </tr>
                    <tr>
                        <td class="aa">昵称：</td>
                        <td class="ab"><input type="text" id="tbNickName" class="RegfTxt" onkeyup="CheckInputLength(this,20,event)" onblur="HasNickName()" /></td>
                        <td class="ac" id="divNickName">用户角色名称，支持中文</td>
                    </tr>
                    <%--<tr>
                        <td class="aa">QQ号：</td>
                        <td class="ab"><input type="text" id="tbQQ" class="RegfTxt" onkeyup="CheckInputLength(this,11,event)" /></td>
                        <td class="ac" id="divQQ">（选填）</td>
                    </tr>--%>
                </table>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="ba">性别：</td>
                        <td class="bb">
                            <input type="radio" id="rdMan" name="rdGender" checked="checked" onclick="document.getElementById('ddlGender').value=1;" />男
                            <input type="radio" id="rdWoman" name="rdGender" onclick="document.getElementById('ddlGender').value=0;"/>女
                            <input type="hidden" id="ddlGender" value="1" />
                        </td>
                        <td class="mb" id="divGender"></td>
                    </tr>
                    <tr>
                        <td class="ba">生日：</td>
                        <td class="bb">
                            <select name="YYYY" id="YYYY" style="width:60px; color:#A9A9A9; background-color:#292B2F; border-bottom-color:#A9A9A9; border-right-color:#A9A9A9;" onchange="YYYYDD(this.value)" onblur="CheckBirthDay()">
                            <option value="">
                                --
                            </option>
                            </select>
                            年
                            <select name="MM" id="MM" onchange="MMDD(this.value)" style="width:46px; color:#A9A9A9; background-color:#292B2F; border-bottom-color:#A9A9A9; border-right-color:#A9A9A9;" onblur="CheckBirthDay()">
                                <option value="">
                                    --
                                </option>
                            </select>
                            月
                            <select name="DD" id="DD" onblur="CheckBirthDay()" style="width:46px; color:#A9A9A9; background-color:#292B2F; border-bottom-color:#A9A9A9; border-right-color:#A9A9A9;">
                                <option value="">
                                    --
                                </option>
                            </select>
                            日
                        </td>
                        <td class="bc" id="divBirthDay">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="ba">地区：</td>
                        <td class="bb">
                            <input type="hidden" id="hddCity" name="hddCity" value="" />
                            <input type="hidden" id="HidProvince" name="HidProvince" value="" />
                            <input type="hidden" id="Hidcity2" name="Hidcity2" />
                            <select onchange="setcity(0,'')" id="prv" name="prv" style="width:97px; color:#A9A9A9; background-color:#292B2F; border-bottom-color:#A9A9A9; border-right-color:#A9A9A9;" onblur="CheckSpace()">
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
                            <select name="city" id="city" style="width:97px; color:#A9A9A9; background-color:#292B2F; border-bottom-color:#A9A9A9; border-right-color:#A9A9A9;" onchange="ChangeCity(this.value)" onblur="CheckSpace()">
                                <option value="" id="opCity">
                                    — —市
                                </option>
                            </select>
                        </td>
                        <td class="bc" id="divSpace">必填项，帮助您更方便找到同城玩家</td>
                    </tr>
                    <tr>
                        <td class="ba">验证码：</td>
                        <td class="bb">
                            <input id="txtCheckVCode" class="bg2" />
                            <img id="CreateVcode" src="CreateVcode.aspx" width="63" height="22" align="absmiddle" alt="验证码" />
                        </td>
                        <td class="bc">
                            <a href="#" onclick="ChangeVcode();">看不清换一个</a>
                            <span style="color:#898989;" id="divVcode"></span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="sub_b1">
                <a href="#">
                    <img src="Images/sub1.png" onMouseOver='this.src="Images/sub1_hover.png"' onclick='this.src="Images/sub1_active.png";RegEnd();' id="btnRegNext" onMouseOut='this.src="Images/sub1.png"'>
                </a>
            </div>
            <div id="cmd"></div>
        </form>
    </div>
</body>
</html>
