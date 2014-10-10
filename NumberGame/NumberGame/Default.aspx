<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NumberGame._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Demo</title>
</head>
<script laugnage="javascript">
    var temp=120;
    function oJump() 
    { 
        temp-=1 
        document.getElementById("Timer").innerText=temp; 
        if(temp==0) 
        {
            clearTimeout(obj);
            document.getElementById("btn_addNum_1").disabled ="false";
            document.getElementById("btn_addNum_2").disabled ="false";
            document.getElementById("btn_addNum_3").disabled ="false";
        } 
    } 
    var obj=setInterval("oJump()",1000);
    
    function btn_addNum_1_Click() //单数计算
    {
        move();
        var num =  parseInt(Math.random()*5+1);
        var addNum = parseInt(document.getElementById("lbl_Num").innerHTML);
        if(calculate(addNum))
        {
            combo(1);
            document.getElementById("lbl_Num").innerText = addNum-1;
        }
        else
        {
            if(addNum%2 == 1)
            {
                combo(0);
                addNum+=num;
                document.getElementById("lbl_Num").innerText = addNum;
            }
            else
            {
                combo(1);
                document.getElementById("lbl_Num").innerText = addNum-1;
            }
        }
    }
     
    function btn_addNum_2_Click() //双数计算
    {
        move();
        var num =  parseInt(Math.random()*5+1);
        var addNum = parseInt(document.getElementById("lbl_Num").innerHTML);
        
        if(calculate(addNum))
        {
            combo(1);
            document.getElementById("lbl_Num").innerText = addNum-1;
        }
        else
        {
            if(addNum%2 == 0)
            {
                combo(0);
                addNum+=num;
                document.getElementById("lbl_Num").innerText = addNum;
            }
            else
            {
                combo(1);
                document.getElementById("lbl_Num").innerText = addNum-1;
            }
        }
    } 
    
    function btn_addNum_3_Click() //素数计算
    {
        move();
        var num =  parseInt(Math.random()*5+1);
        var addNum = parseInt(document.getElementById("lbl_Num").innerHTML);
        
        if(calculate(addNum))
        {
            combo(0);
            addNum+=num;
            document.getElementById("lbl_Num").innerText = addNum;
        }
        else
        {
            combo(1);
            document.getElementById("lbl_Num").innerText = addNum-1;
        }
    }
    
    function OnloadNum() //加载里添加随机数
    {
        var num =  parseInt(Math.random()*5+1);
        document.getElementById("lbl_Num").innerText = num;
    }
    
    function calculate(intNum) //素数判定
    {
        var num=parseInt(intNum);
        if (isNaN(num) || num < 0) 
        {
            return false;
        }
        if (num == 1)
        {
            return false;
        }
        if (num == 2)
        {
            return true;
        }
        for (var i=2;i<num;i++) 
        {
            if (num % i == 0) 
            {
                return false;
            }
        }
        return true;
    } 
    
    function move()//随机按钮位置
    {
        var l="5,105,210";
        var str= new Array(); 
        str=l.split(",");
        var s=parseInt(Math.random()*3+0);
        //document.write(str[s]+"<br/>");
        var l1=str[s];
        str.splice(s,1);
        var s1=parseInt(Math.random()*2+0);
        //document.write(str[s1]+"<br/>");
        var l2=str[s1];
        str.splice(s1,1);
        //document.write(str[0]+"<br/>");
        document.getElementById("btn_addNum_1").style.left=l1+"px";
        document.getElementById("btn_addNum_2").style.left=l2+"px";
        document.getElementById("btn_addNum_3").style.left=str[0]+"px";
    }
    
    function combo(intNum_A) //连击判定
    {
        var num_a = 1;
        var addCombo = parseInt(document.getElementById("lbl_Combo").innerHTML);
       
        //alert(stCombo);
        
        if(intNum_A == 0)
        {
            addCombo += num_a;
            document.getElementById("lbl_Combo").innerText = addCombo;
            //alert(addCombo);
            if(addCombo >= 5)
            {
                if(addCombo%5 == 0)
                {
                    var num_1 =  parseInt(Math.random()*5+1);
                    var addNum_1 = parseInt(document.getElementById("lbl_Num").innerHTML);
                    addNum_1+=num_1;
                    alert(addNum_1+" xxxx "+num_1);
                    document.getElementById("lbl_Num").innerText = addNum_1;
                }
                document.getElementById("lbl_Combo").style.display = "block";
            }
        }
        else
        {
            document.getElementById("lbl_Combo").style.display = "none";
            document.getElementById("lbl_Combo").innerText = 0;
        }
    }
    
    var num =  parseInt(Math.random()*5+1);
</script> 

<body onload="OnloadNum()">
    <form id="form1" runat="server">
        <span id="Timer">120</span>秒<br />
        <asp:Label ID="lbl_Text" runat="server">个人资产：</asp:Label>
        <asp:Label ID="lbl_Num" runat="server">0</asp:Label><br/>
        <input style="position:absolute;top:100px;left:5px; width:80px;" type="button" id = "btn_addNum_1" value="单数写报告" onclick="btn_addNum_1_Click()" />
        <input style="position:absolute;top:100px;left:105px; width:80px;" type="button" id = "btn_addNum_2" value="双数拉客户" onclick="btn_addNum_2_Click()" />
        <input style="position:absolute;top:100px;left:210px; width:80px;" type="button" id = "btn_addNum_3" value="素数抓小偷" onclick="btn_addNum_3_Click()" />
        <input type="hidden" id="hdNum" value="0"/>
        <asp:Label ID="lbl_Combo" runat="server" style ="display:none" ForeColor="red">0</asp:Label>
        <input type="button" value="Test" onclick="combo(0)" style ="display:none"/>
        <input type="button" value="Test1" onclick="combo(1)" style ="display:none"/>
    </form>
</body>
</html>
