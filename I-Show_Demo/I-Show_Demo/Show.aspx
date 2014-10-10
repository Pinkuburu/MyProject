<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="I_Show_Demo.Show" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Demo</title>
    
    <script type="text/javascript">
     //setTimeout( 'this.location.reload() ',4000);
     var i=0;
     setInterval("vGetRand()", 4000); //每秒钟取一次数据  /*** 初始化一个xmlhttp对象 * /
     function InitAjax()
     {
         var ajax=false; 
         try 
         { 
            ajax = new ActiveXObject("Msxml2.XMLHTTP"); 
         } 
         catch (e) 
         { 
             try 
             { 
                ajax = new ActiveXObject("Microsoft.XMLHTTP"); 
             } 
             catch (E)
             { 
                ajax = false; 
             } 
         }
         if (!ajax && typeof XMLHttpRequest!='undefined') { 
            ajax = new XMLHttpRequest(); 
         } 
         return ajax;
     }

     function vGetRand()
     {
         var ShowImg=document.getElementById("ShowImg");
         var url="Refresh.aspx?rnd=" + Math.random();
         //var url="Refresh.aspx";
         var ajax = InitAjax();
         ajax.open("GET", url, true);
         ajax.onreadystatechange = function() 
         {
            //alert(ajax.readyState + "  " + ajax.status);
             if (ajax.readyState == 4 && ajax.status == 200) 
             { 
                 //ShowImg.innerHTML = SetDateDemo(ajax.responseText); 
                 //alert(ajax.responseText);
                 i++;
                 ShowImg.innerHTML = ajax.responseText; 
             } 
         }
         ajax.send(null);
    }
    /*
    function SetDateDemo(newdate)
    {
        var d, s; // 声明变量。
        d = new Date(); // 创建 date 对象。
        d.setUTCDate(0); // 设置 date 为 newdate。
        s = "Current setting is ";
        s += d.toLocaleString();
        return (s); // 返回新设的日期。
    }
    */    
  </script>
</head>
<body>
    <form id="Show_Form" runat="server">
        <div id="ShowImg" runat="server">
            <table id="tb_ShowImg">
                <%=strContent%>            
            </table>
        </div>
    </form>
</body>
</html>
