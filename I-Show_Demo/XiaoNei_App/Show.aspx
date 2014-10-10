<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="XiaoNei_App.Show" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Demo</title>
    <link href="skins/tango/skin.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        body {
	        font-size: 12px;
	        color: #444;
	        font-family: "微软雅黑";
	        line-height:20px;
	        text-align:left;
	        margin: 0;
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
        .page {
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
    
    <script type="text/javascript" src="JS/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="JS/jquery.jcarousel.min.js"></script>
    
    <script type="text/javascript">
        // 高亮物件
	    function highlight ( obj, highlightClass )
	    {
		    if ( typeof ( obj ) == 'string' )
		    {
			    obj = $ ( obj );
		    }
		    if ( highlightClass == null )
		    {
			    highlightClass = 'pagebutton';
		    }
		    try 
		    {
			    for ( var i in obj.parentNode.childNodes )
			    {
				    if ( obj.parentNode.childNodes[i].className != null )
				    {
					    obj.parentNode.childNodes[i].className = obj.parentNode.childNodes[i].className.replace ( 'pagebutton', 'pg2' );
				    }			
			    }
			    obj.className = obj.className.replace('pg2','pagebutton');
		    }
		    catch ( e ) {}
	    }
    </script>
    
    <script type="text/javascript">
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
         var p=document.getElementById("hdpage").value;
         var ShowImg=document.getElementById("ShowImg");
         var url="Refresh.aspx?rnd=" + Math.random()+"&page="+p;
         var ajax = InitAjax();
         ajax.open("GET", url, true);
         ajax.onreadystatechange = function() 
         {
             if (ajax.readyState == 4 && ajax.status == 200) 
             {
                 i++;
                 ShowImg.innerHTML = ajax.responseText; 
             } 
         }
         ajax.send(null);
     }
     
     function vGetRands(page)
     {
         var ShowImg=document.getElementById("ShowImg");
         var url="Refresh.aspx?rnd=" + Math.random()+"&Page=" + page;
         var ajax = InitAjax();
         ajax.open("GET", url, true);
         ajax.onreadystatechange = function() 
         {
             if (ajax.readyState == 4 && ajax.status == 200) 
             {
                 i++;
                 ShowImg.innerHTML = ajax.responseText; 
                 document.getElementById("hdpage").value=page;
             } 
         }
         ajax.send(null);
     }  
    </script>
    
    <script type="text/javascript">
        jQuery(document).ready(function() {
            jQuery('#mycarousel').jcarousel();
        });
    </script>

</head>
<body>
<div class="body">
    <form id="Show_Form" runat="server">
        <div id="ShowImg" runat="server">
            <%=strContent%>
        </div>
        <div class="page" align="center">
            <%=strPageShow%>
        </div>
        <div class="friendlist">
		    <ul id="mycarousel" class="jcarousel-skin-tango" >
                <%=strFriend_List%>
            </ul>
        </div>
        <input type="hidden" value="1" id="hdpage" />
    </form>
    <div style="position:absolute;left:17px;top:593px;"><a href="http://ishow.xba.com.cn/image/update/PlayCap.rar">点击此处下载客户端</a></div>
</div>
</body>
</html>
