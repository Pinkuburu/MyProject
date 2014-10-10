<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="BBS_Show.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>青岛美天网络科技有限公司</title>
    <style type="text/css">
        <!--
        .STYLE1 {color: #FF0000}
        .STYLE3 {color: #FF0000; font-weight: bold; }
        -->
        .bizmail_loginpanel{font-size:12px;width:300px;height:auto;border:0px solid #cccccc;background:#ffffff;}
        .bizmail_LoginBox{padding:10px 15px;}
        .bizmail_loginpanel h3{padding-bottom:5px;margin:0 0 5px 0;border-bottom:1px solid #cccccc;font-size:14px;}
        .bizmail_loginpanel form{margin:0;padding:0;}
        .bizmail_loginpanel input.text{font-size:12px;width:100px;height:20px;margin:0 2px;border:1px solid #C3C3C3;border-color:#7C7C7C #C3C3C3 #C3C3C3 #9A9A9A;}
        .bizmail_loginpanel .bizmail_column{height:28px;}
        .bizmail_loginpanel .bizmail_column label{display:block;float:left;width:30px;height:24px;line-height:24px;font-size:12px;}
        .bizmail_loginpanel .bizmail_column .bizmail_inputArea{float:left;width:240px;}
        .bizmail_loginpanel .bizmail_column span{font-size:12px;word-wrap:break-word;margin-left: 2px;line-height:200%;}
        .bizmail_loginpanel .bizmail_SubmitArea{margin-left:30px;clear:both;}
        .bizmail_loginpanel .bizmail_SubmitArea a{font-size:12px;margin-left:5px;}
        .bizmail_loginpanel select{width:110px;height:20px;margin:0 2px;}
    </style>
    <meta content="IE=EmulateIE7" http-equiv="X-UA-Compatible">
    <link href="/css/style.css" rel="stylesheet"></link>
    <script src="/Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
//            $("#news1").click(function () {
//                $("#news1").attr("class", "current");
//                $("#news2").attr("class", "");
//                $("#news-1-2").attr("style", "display:block");
//                $("#news-2-2").attr("style", "display:none");
//            });

//            $("#news2").click(function () {
//                $("#news2").attr("class", "current");
//                $("#news1").attr("class", "");
//                $("#news-1-2").attr("style", "display:none");
//                $("#news-2-2").attr("style", "display:block");
//            });

//            $("#news1").mouseover(function () {
//                $("#news1").attr("class", "current");
//                $("#news2").attr("class", "");
//                $("#news-1-2").attr("style", "display:block");
//                $("#news-2-2").attr("style", "display:none");
//            });

            $("#news2").mouseover(function () {
                $("#news2").attr("class", "current");
                $("#news3").attr("class", "");
                $("#news-2-2").attr("style", "display:block");
                $("#news-3-3").attr("style", "display:none");
            });

            $("#news3").mouseover(function () {
                $("#news2").attr("class", "");
                $("#news3").attr("class", "current");
                $("#news-2-2").attr("style", "display:none");
                $("#news-3-3").attr("style", "display:block");
            });

            $.get("Default.aspx",
                {
                    Category:1,
                    rnd: Math.random()
                },
                function (data) {
                    $("#gonggao1").html(data)
                });

            $.get("Default.aspx",
                {
                    Category: 11,
                    rnd: Math.random()
                },
                function (data) {
                    $("#gonggao2").html(data)
                });

            $.get("Default.aspx",
                {
                    Category: 2,
                    rnd: Math.random()
                },
                function (data) {
                    $("#retie1").html(data)
                });

            $.get("Default.aspx",
                {
                    Category: 3,
                    rnd: Math.random()
                },
                function (data) {
                    $("#retie2").html(data)
                });

            $.get("Default.aspx",
                {
                    Category: 4,
                    rnd: Math.random()
                },
                function (data) {
                    $("#huodong1").html(data)
                });

            $.get("Default.aspx",
                {
                    Category: 5,
                    rnd: Math.random()
                },
                function (data) {
                    $("#huodong2").html(data)
                });
        });
    </script>
    <script type="text/javascript" src="js/outerlogin.js"  charset="gb18030"></script>
</head>
<body>
        <div id="wrapper" class="wrapper">
			<div class="module layout-row" data-item="a" id="header">
				<h1 id="title">
					<img style="behavior: url(/css/iepngfix.htc); margin: 10px 0 0 7px;" alt="" src="/images/logo/today.png">
				</h1>        
				<div class="module" id="calendar">
					<div class="inner">
						<span id="solar-day"></span>
						<a target="_blank" href="http://site.baidu.com/list/wannianli.htm" id="link-calendar" data-name="a1">万年历</a><br>
						<span id="lunar-day"></span>
						<a target="_blank" href="http://site.baidu.com/list/wannianli.htm" id="link-festival" data-name="a2" title=""></a>
					</div>
				</div>
				<div class="module" id="header-menu">
					<div class="max-nav clearfix">
						<a href="http://www.xba.com.cn/" target="_blank" class="left">公司首页</a>
						<a href="http://192.168.1.250/" target="_blank" class="middle">公司论坛</a>
						<a href="#" id="btn-set-homepage" class="right">设为首页</a>
					</div>
					<!-- <div class="max-actions"> -->
						<!-- <a href="#" id="btn-set-homepage" data-name="a6" class="left">设为首页</a> -->
						<!-- <a href="#" target="_blank" id="btn-goto-v2" data-name="a7" class="middle">使用旧版</a> -->
						<!-- <a target="_blank" href="http://bbs.maxthon.cn/viewthread.php?tid=180212&amp;extra=page=1" id="btn-feedback" data-name="a8" class="right">反馈</a> -->
					<!-- </div> -->
				</div>
				<div class="module" id="weather">
					<iframe width="360" scrolling="no" height="50" frameborder="0" src="http://m.weather.com.cn/m/maxthon_new/indexs.html" allowtransparency="true" marginwidth="0" marginheight="0">
					</iframe>
				</div>
			</div>
            <div id="firstNews" style="width:721px; height:200px; background-color:#ECF8FD; border:#9fc9df 1px solid; margin-bottom:10px; margin-top:10px;">
                <p class="STYLE1">◆美天网络企业邮箱@xba.com.cn正式启用◆</p>
                <p> &nbsp;&nbsp;为了全面体现公司企业形象，从明日起2011.06.15，公司邮件群发、内部工作确认等相关工作层面的邮件来往，需全部在企业邮箱内完成。<br />
                &nbsp;&nbsp;为方便记忆和使用，邮箱用户名全部采用姓名的全拼，密码统一为111111，即“姓名全拼@xba.com.cn”，登录后请自行修改密码。<br />
                  例如：<br />
                张忆豫    zhangyiyu@xba.com.cn
                <p>◆登录企业邮箱有2种方式：<br />
                  1.输入网址 <a href="http://exmail.qq.com/" target="_blank">http://exmail.qq.com/</a> 点击“老用户在此登录”，输入邮箱地址和密码即可。 <br />
                  2.公司内部首页<a href="http://192.168.1.250:88/">http://192.168.1.250:88/</a>  右侧快捷登录口，可直接登录。
                <p>◆其他问题请查看论坛 “美天网络企业邮箱使用FAQ”<br />
                  <a href="http://192.168.1.250/viewthread.php?tid=1750&amp;page=1&amp;extra=#pid7397" target="_blank">http://192.168.1.250/viewthread.php?tid=1750&amp;page=1&amp;extra=#pid7397</a></p>
            </div>
            <div class="module layout-row" id="search-bar" data-item="b">
                <a target="_blank" href="" data-flag="1" class="search-logo" style="background-image:url(/images/baidu_2011021201.gif);">
                </a>
                <div class="middle">
                    <form target="_blank" action="#" id="search-form">
                    <!--搜索链接-->
                        <div data-flag="1" class="search-tabs clearfix" style="display: block;"></div>
                        <div class="search-control clearfix">
                            <span class="text-wrapper" id="search-key-wrapper">
                                <input autocomplete="off" id="search-key" class="text">
                            </span>
                            <input type="submit" value="百度一下" id="search-go" class="go">
                        </div>
                        <div class="search-engines clearfix" style="display: block;">
                            <span class="search-engine">
                                <input type="radio" name="searchEngineGroup" id="engine_00">
                                <label for="engine_00" class="checked">
                                    百度
                                </label>
                            </span>
                            <span class="search-engine">
                                <input type="radio" name="searchEngineGroup" id="engine_01">
                                <label for="engine_01">
                                    Google
                                </label>
                            </span>
                            <span class="search-engine">
                                <input type="radio" name="searchEngineGroup" id="engine_02">
                                <label for="engine_02">
                                    多重搜索
                                </label>
                            </span>
                        </div>
                    </form>
                </div>
                <!-- 搜索旁关键词 -->
                <div id="search-scroll-keys" data-action="key">
                </div>
            </div>       
            <div class="module layout-row" id="news-bar">
                <div class="module content-module layout-75" id="news" data-item="f" style="height:190px;">
                    <div class="module-head clearfix tab-container">
                        <div class="tabs" id="news-tabs">
                            <a id="news1" href="#" class="current">
                                <span class="first-tab">
                                    <span class="tab-border-left first-tab">
                                    </span>
                                    <span class="tab-content first-tab">
                                        公司公告
                                    </span>
                                    <span class="tab-border-right first-tab">
                                    </span>
                                </span>
                            </a>
                        </div>
                        <div class="more">
                            <span class="left">
                            </span>
                            <span class="right">
                            </span>
                        </div>
                    </div>
                    <div class="module-body" style="height:160px;">
                        <div id="news-1-2" data-name="要闻-0" class="panel" style="display: block;" status="completed">
                            <%--<div class="headline">
                                <a target="_blank" class="img" title="日本核事故禁区最新照片披露" href="http://slide.news.sina.com.cn/w/slide_1_18255_17113.html?c=spr_sw_bd_maxthon_news"
                                v="要闻" statistics="chief">
                                    <img width="200" height="120" src="http://i0.sinaimg.cn/dy/U4708P1T124D2F2633DT20110413072120.jpg">
                                    <span>
                                        日本核事故禁区最新照片披露
                                    </span>
                                </a>
                            </div>--%>
                            <div id="gonggao1" class="list links-block">
                            </div>
                            <div id="gonggao2" class="list links-block">
                            </div>
                            <%--<div class="thumb-list">
                                <a target="_blank" title="南京卷烟厂扩建计划引发质疑 居民曾多次投诉" href="http://news.sina.com.cn/c/2011-04-13/061422282705.shtml?c=spr_sw_bd_maxthon_news"
                                v="要闻" statistics="chief">
                                    <img width="80" height="60" src="http://i3.sinaimg.cn/dy/c/2011-04-13/U4167P1T1D22282705F21DT20110413061454_small.jpg">
                                    <span>
                                        南京卷烟厂扩建计划引发质疑 居民曾多次投诉
                                    </span>
                                </a>
                                <a target="_blank" title="北京6死1伤燃气爆炸事故管道曾因泄漏报修" href="http://news.sina.com.cn/c/2011-04-13/014622280871.shtml?c=spr_sw_bd_maxthon_news"
                                v="要闻" statistics="chief">
                                    <img width="80" height="60" src="http://i0.sinaimg.cn/dy/c/2011-04-13/1302630396_ui57QT_small.jpg">
                                    <span>
                                        北京6死1伤燃气爆炸事故管道曾因泄漏报修
                                    </span>
                                </a>
                            </div>--%>
                        </div>
                    </div>
                </div>
                <div class="module content-module layout-25 layout-right" data-item="e" id="tools" style="height:188px;">
                    <div class="module-head clearfix">
                        <h2 class="title">
                            邮箱快速登陆
                        </h2>
                    </div>
                    <div class="module-body">
                        <script type="text/javascript">
                            writeLoginPanel({ domainlist: "xba.com.cn", mode: "vertical" });
</script>
                    </div>
                </div>
            </div>
            <div class="module layout-row" id="news-bar">
                <div class="module content-module layout-75" id="news" data-item="f">
                    <div class="module-head clearfix tab-container">
                        <div class="tabs" id="news-tabs">
                            <a id="news2" href="#" class="current">
                                <span class="first-tab">
                                    <span class="tab-border-left first-tab">
                                    </span>
                                    <span class="tab-content first-tab">
                                        论坛热贴
                                    </span>
                                    <span class="tab-border-right first-tab">
                                    </span>
                                </span>
                            </a>
                            <a id="news3" href="#" class="">
                                <span>
                                    <span class="tab-border-left">
                                    </span>
                                    <span class="tab-content">
                                        公众服务区
                                    </span>
                                    <span class="tab-border-right">
                                    </span>
                                </span>
                            </a>
                        </div>
                        <div class="more">
                            <span class="left">
                            </span>
                            <span class="right">
                            </span>
                        </div>
                    </div>
                    <div class="module-body">
                        <div id="news-2-2" class="panel" style="display: block;" status="completed">
                            <%--<div class="headline">
                                <a target="_blank" class="img" title="斯威夫特《ELLE》大片 时尚歌手笑容甜美" href="http://slide.ent.sina.com.cn/slide_4_704_20191.html?c=spr_sw_bd_maxthon_ent"
                                v="娱乐" statistics="ent">
                                    <img width="200" height="120" src="http://i0.sinaimg.cn/ent/402/2009/0701/U3593P28T402D1F12476DT20110413095714.jpg">
                                    <span>
                                        斯威夫特《ELLE》大片 时尚歌手笑容甜美
                                    </span>
                                </a>
                            </div>--%>
                            <div id="retie1" class="list links-block">
                            </div>
                            <div id="retie2" class="list links-block">
                            </div>
                            <%--<div class="thumb-list">
                                <a target="_blank" title="《都市囧人》热演 “空姐”李霞妩媚亮相(图)" href="http://ent.sina.com.cn/j/2011-04-13/12413280209.shtml?c=spr_sw_bd_maxthon_ent"
                                v="娱乐" statistics="ent">
                                    <img width="80" height="60" src="http://i1.sinaimg.cn/ent/j/2011-04-13/U2223P28T3D3280209F326DT20110413124147.jpg">
                                    <span>
                                        《都市囧人》热演 “空姐”李霞妩媚亮相(图)
                                    </span>
                                </a>
                                <a target="_blank" title="揭秘孟版《罗密欧与朱丽叶》如何颠覆经典(图)" href="http://ent.sina.com.cn/j/2011-04-13/12393280208.shtml?c=spr_sw_bd_maxthon_ent"
                                v="娱乐" statistics="ent">
                                    <img width="80" height="60" src="http://i2.sinaimg.cn/ent/j/2011-04-13/U2223P28T3D3280208F326DT20110413123919.JPG">
                                    <span>
                                        揭秘孟版《罗密欧与朱丽叶》如何颠覆经典(图)
                                    </span>
                                </a>
                            </div>--%>
                        </div>
                        <div id="news-3-3" class="panel" style="display: none;" status="completed">
                            <div id="Div1" class="list links-block">
                                <a target="_blank" title="【制度】财务制度" href="http://192.168.1.250/viewthread.php?tid=1587&extra=page%3D1">【制度】财务制度</a>
                                <a target="_blank" title="【制度】丛林生存指南" href="http://192.168.1.250/viewthread.php?tid=1593&extra=page%3D1">【制度】丛林生存指南</a>
                                <a target="_blank" title="【制度】公司关于上班期间娱乐活动的管理办法" href="http://192.168.1.250/viewthread.php?tid=1401&extra=page%3D2">【制度】公司关于上班期间娱乐活动的管理办法</a>
                                <a target="_blank" title="【制度】关于擅自设置IP的处罚规定" href="http://192.168.1.250/viewthread.php?tid=1270&extra=page%3D3">【制度】关于擅自设置IP的处罚规定</a>
                                <a target="_blank" title="【制度】美天科技NDSL游戏使用规范" href="http://192.168.1.250/viewthread.php?tid=730&extra=page%3D5">【制度】美天科技NDSL游戏使用规范</a>
                                <a target="_blank" title="【服务】公司员工通讯录" href="http://192.168.1.250/viewthread.php?tid=1454&extra=page%3D1">【服务】公司员工通讯录</a>
                                <a target="_blank" title="【服务】请假条使用说明" href="http://192.168.1.250/viewthread.php?tid=342&extra=page%3D1">【服务】请假条使用说明</a>                                
                            </div>
                            <div id="Div2" class="list links-block">
                                <a target="_blank" title="【服务】办公用品的领取方式" href="http://192.168.1.250/viewthread.php?tid=323">【服务】办公用品的领取方式</a>
                                <a target="_blank" title="【服务】添加打印机的方法" href="http://192.168.1.250/viewthread.php?tid=229&extra=page%3D1">【服务】添加打印机的方法</a>
                                <a target="_blank" title="【服务】员工生日蛋糕的领取方法" href="http://192.168.1.250/viewthread.php?tid=136">【服务】员工生日蛋糕的领取方法</a>
                                <a target="_blank" title="【服务】药品领用明细" href="http://192.168.1.250/viewthread.php?tid=1592">【服务】药品领用明细</a>
                                <a target="_blank" title="【新人帮助】新人导航（不断更新）" href="http://192.168.1.250/viewthread.php?tid=312&extra=page%3D1">【新人帮助】新人导航（不断更新）</a>
                                <a target="_blank" title="【新人报到】新进员工介绍" href="http://192.168.1.250/viewthread.php?tid=898&extra=page%3D1">【新人报到】新进员工介绍</a>
                                <a target="_blank" title="【软件下载】内部软件下载-帐号：guest密码无" href="\\192.168.1.28\">【软件下载】内部软件下载-帐号：guest密码无</a>
                            </div>
                        </div>                        
                    </div>  
                </div>
                <div class="module content-module layout-25 layout-right" data-item="e" id="Div3">
                    <div class="module-head clearfix">
                        <h2 class="title">
                            常用文档下载（点击即下载）
                        </h2>
                    </div>
                    <div class="module-body" style="padding-left:10px; height:210px; padding-top:5px;">
                        <font style="font-size:14px;">
                            <a target="_blank" title="【行政】员工通讯录" href="http://192.168.1.250:88/File/员工通讯录.xls">【行政】员工通讯录</a></br>
                            <a target="_blank" title="【行政】请假条" href="http://192.168.1.250:88/File/请假条.doc">【行政】请假条</a></br>
                            <a target="_blank" title="【行政】行政处罚单" href="http://192.168.1.250:88/File/行政处罚单.xls">【行政】行政处罚单</a></br>                            
                            <a target="_blank" title="【行政】行政处罚免罚单" href="http://192.168.1.250:88/File/行政处罚免罚单.xls">【行政】行政处罚免罚单</a></br> 
                            <a target="_blank" title="【财务】现金借款单" href="http://192.168.1.250:88/File/现金借款单.doc">【财务】现金借款单</a></br>                                   
                            <a target="_blank" title="【财务】支票领用单" href="http://192.168.1.250:88/File/支票领用单.doc">【财务】支票领用单</a></br>
                            <a target="_blank" title="【财务】现金费用报销单" href="http://192.168.1.250:88/File/现金费用报销单.doc">【财务】现金费用报销单</a></br>
                            <a target="_blank" title="【运营】运营部需求工单（模版）" href="http://192.168.1.250:88/File/运营部需求工单（模版）.doc">【运营】运营部需求工单（模版）</a></br>
                            <a target="_blank" title="【运营】版本更新会签单（模版）" href="http://192.168.1.250:88/File/产品版本更新会签单.doc">【运营】版本更新会签单（模版）</a></br>
                            <a target="_blank" title="【项目】版本交付单（模版）" href="http://192.168.1.250:88/File/产品版本交付单.doc">【项目】版本交付单（模版）</a></br>
                        </font>
                    </div>
                </div>
            </div>
            <div class="module layout-row" id="news-bar">
                <div class="module content-module layout-75" id="news" data-item="f">
                    <div class="module-head clearfix tab-container">
                        <div class="tabs" id="news-tabs">
                            <a id="news4" href="#" class="current">
                                <span class="first-tab">
                                    <span class="tab-border-left first-tab">
                                    </span>
                                    <span class="tab-content first-tab">
                                        公司活动区
                                    </span>
                                    <span class="tab-border-right first-tab">
                                    </span>
                                </span>
                            </a>
                        </div>
                        <div class="more">
                            <span class="left">
                            </span>
                            <span class="right">
                            </span>
                        </div>
                    </div>
                    <div class="module-body">
                        <div id="news-4-4" class="panel" style="display:block;" status="completed">
                            <div id="huodong1" class="list links-block">
                            </div>
                            <div id="huodong2" class="list links-block">
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="module layout-row" id="footer">
                <span class="copyright">
                    版权所有 &copy; 2011 青岛美天网络科技有限公司
                    <a target="_blank" href="#">
                        关于我们
                    </a>
                    <a target="_blank" href="#">
                        联系我们
                    </a>
                    <a target="_blank" href="#">
                        用户反馈
                    </a>
                </span>
            </div>
    	</div>
        <script src="/js/main.js" type="text/javascript"></script>
        <script src="/data/start.js" type="text/javascript"></script>
        <script src="/data/daily.js" type="text/javascript"></script>
        <script src="/data/search.js" type="text/javascript"></script>        
</body>
</html>
