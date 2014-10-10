var intIsChild = 0;
        
function YYYYMMDDstart()
{
    MonHead = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    //先给年下拉框赋内容            
    var ys = new Date().getFullYear();//初始年份
    var y = 1984;
    //以今年为准，前30年，后30年
    for (var i = (ys-60); i < (ys); i++) 
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

//判断地址
function CheckSpace()
{
    var strProvince=encodeURI($("#prv").find("option:selected").text());
    var strCity=encodeURI(document.getElementById("hddCity").value);
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
        document.getElementById("divSpace").innerHTML="";
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
        document.getElementById("divRePassword").innerHTML=" <img src=\"Images/Error.gif\" alt=\"\" style=\"border:0px; padding-right:6px;\" align='absmiddle' />两次输入密码不同，请重新输入密码.";
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
    var strUserName=document.getElementById('tbUserName').value;
    var strPassword=document.getElementById('tbPassword').value;
    var strRePassword=document.getElementById('tbRePassword').value;
    //var strEmail=document.getElementById("tbEmail").value;
    var strNickName=encodeURI(document.getElementById("tbNickName").value);
    //alert(strNickName);
    var strGender=document.getElementById("ddlGender").value;
    var strYear=document.getElementById("YYYY").value;
    var strMonth=document.getElementById("MM").value;
    var strDay=document.getElementById("DD").value;
    var strProvince=encodeURI($("#prv").find("option:selected").text());
    var strCity=encodeURI(document.getElementById("hddCity").value);
    //var strIntro="";
    var strQQ="";//document.getElementById("tbQQ").value;
    //var strMSN="";
    //var strcbReg=document.getElementById("cbReg").checked;
    var strVCode=document.getElementById("txtCheckVCode").value;
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
    //dialog.open("/RegOp.aspx?Type=Reg&UserName="+strUserName+"&Password="+strPassword+"&RePassword="+strRePassword+"&Nickname="+strNickName
    //+"&Gender="+strGender+"&Year="+strYear+"&Month="+strMonth+"&Day="+strDay+"&Province="+strProvince+"&City="+strCity+"&VCode="+strVCode);
    loader.post("/RegOp.aspx?Type=Reg&UserName="+strUserName+"&Password="+strPassword+"&RePassword="+strRePassword+"&Nickname="+strNickName
    +"&Gender="+strGender+"&Year="+strYear+"&Month="+strMonth+"&Day="+strDay+"&Province="+strProvince+"&City="+strCity+"&VCode="+strVCode+"&QQ="+strQQ,cmd,null,null,null,null);
    //alert("/RegOp.aspx?Type=Reg&UserName="+strUserName+"&Password="+strPassword+"&RePassword="+strRePassword+"&Nickname="+strNickName
    //+"&Gender="+strGender+"&Year="+strYear+"&Month="+strMonth+"&Day="+strDay+"&Province="+strProvince+"&City="+strCity+"&VCode="+strVCode);
    //alert(strGender);
    //dialog.open("/Reg/RegOp.aspx?Type=Reg&UserName="+strUserName+"&Password="+strPassword+"&RePassword="+strRePassword+"&Email="+strEmail+"&Nickname="+strNickName
    //+"&Gender="+strGender+"&Year="+strYear+"&Month="+strMonth+"&Day="+strDay+"&Province="+strProvince+"&City="+strCity+"&Intro="+strIntro
    //+"&QQ="+strQQ+"&MSN="+strMSN+"&VCode="+strVCode+"&CardId="+strCardIdEnd+"&cbReg="+strcbReg+"&RealName="+strRName);
}