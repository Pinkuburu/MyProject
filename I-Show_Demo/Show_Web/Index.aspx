<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Show_Web.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>
        瞧！你在干嘛
    </title>
    <link rel="shortcut icon" href="images/favicon.ico">
    <link rel="stylesheet" type="text/css" href="css/index_2.css" />
    <script type="text/javascript" src="JS/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="JS/DD_belatedPNG_0.0.8a.js"></script>
</head>
    
<body>
    <div class="content">
        <div class="header">
            <div class="logo">
                <img src="images/logo.png">
            </div>
            <div class="home">
                <a href="#">设为首页</a>
                &nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
                <a href="#">加入收藏</a>
            </div>
        </div>
        <div class="nav">
            <a href="#"><img class="nav_index" src="images/nav_index.gif" /></a>
            <a href="http://ishow.xba.com.cn/image/update/瞧你在干嘛[1.0].rar"><img class="nav_download" src="images/nav_download.gif" /></a>
            <div class="online">
                累计<font color="#74fac0"><% =intCount %></font>
                会员&nbsp;&nbsp;当前<font color="#f6dd55"><% =intOnlineCount %></font>在线
            </div>
        </div>
        <div class="ban">
            <a href="http://www.xba.com.cn/Register.aspx">
                <img class="ban_r" src="images/ban_register.png" />
            </a>
            <a href="http://ishow.xba.com.cn/image/update/瞧你在干嘛[1.0].rar">
                <img class="ban_d" src="images/ban_download.png" />
            </a>
        </div>
        <div class="middle_content">
            <div class="middle_top">
            </div>
            <div class="middle_text">
                <div class="middle_left">
                </div>
                <div class="middle_right">
                    <img class="news_title" src="images/news_title.png" />
                    <div class="pic_news">
                        <div class="pic_text_title">
                            <a href="http://ishow.xba.com.cn/news1.html">7月28日热力开测</a>
                        </div>
                        <div class="pic_text_con">
                            7月28日10:00，《瞧，你在干嘛》将正式登陆XBA平台。届时，XBA玩家将成为我们……
                            <a href="http://ishow.xba.com.cn/news1.html">更多>></a>
                        </div>
                    </div>
                    <div class="list_news">
                        <ul class="list_ul">
                            <li>
                                <img src="images/core.gif">
                                <a href="http://ishow.xba.com.cn/news.html">找BUG、提建议，会员就是你！</a>
                                <div class="date">[2011-07-26]</div>
                            </li>
                            <li>
                                <img src="images/core.gif">
                                <a href="#">青大校园单身派对，非诚勿扰</a>
                                <font color="#fed9d9"><图></font>
                                <div class="date">[2011-06-15]</div>
                            </li>
                            <li>
                                <img src="images/core.gif">
                                <a href="#">潮流图像交友软件席卷互联时代</a>
                                <font color="#fed9d9"><图></font>
                                <div class="date">[2011-06-15]</div>
                            </li>
                            <li>
                                <img src="images/core.gif">
                                <a href="#">视频达人秀，火热报名中...</a>
                                <font color="#fed9d9"><图></font>
                                <div class="date">[2011-06-15]</div>
                            </li>
                        </ul>
                    </div>
                    <img class="star_title" src="images/star_title.png" />
                    <div class="star_list">
                        <div class="star1">
                            <img class="star_img" src="images/star1.jpg" width="106" />
                            <img class="num" src="images/no1.png" />
                            <div class="star_name">Amay</div>
                            <div class="star_text">
                                粉丝数量:<font color="#74fac0">20000</font><br>
                                年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;龄：<font color="#f6dd55">20</font><br>
                                所&nbsp;&nbsp;在&nbsp;地：<font color="#e29400">山东省青岛市</font>
                            </div>
                        </div>
                        <div class="star2">
                            <img class="star_img" src="images/star1.jpg" width="106" />
                            <img class="num" src="images/no2.png" />
                            <div class="star_name">
                                帅的想自杀
                            </div>
                            <div class="star_text">
                                粉丝数量:<font color="#74fac0">20000</font><br>
                                年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;龄：<font color="#f6dd55">20</font><br>
                                所&nbsp;&nbsp;在&nbsp;地：<font color="#e29400">山东省济南市</font>
                            </div>
                        </div>
                        <div class="star3">
                            <img class="star_img" src="images/star1.jpg" width="106" />
                            <img class="num" src="images/no3.png" />
                            <div class="star_name">ollyoy</div>
                            <div class="star_text">
                                粉丝数量:<font color="#74fac0">20000</font><br>
                                年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;龄：<font color="#f6dd55">20</font><br>
                                所&nbsp;&nbsp;在&nbsp;地：<font color="#e29400">江苏省南京市</font>
                            </div>
                        </div>
                        <div class="star4">
                            <img class="star_img" src="images/star1.jpg" width="106" />
                            <img class="num" src="images/no4.png" />
                            <div class="star_name">鑫哥</div>
                            <div class="star_text">
                                粉丝数量:<font color="#74fac0">20000</font><br>
                                年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;龄：<font color="#f6dd55">20</font><br>
                                所&nbsp;&nbsp;在&nbsp;地：<font color="#e29400">山东省青岛市</font>
                            </div>
                        </div>
                        <div class="star5">
                            <img class="star_img" src="images/star1.jpg" width="106" />
                            <img class="num" src="images/no5.png" />
                            <div class="star_name">Mr.超</div>
                            <div class="star_text">
                                粉丝数量:<font color="#74fac0">20000</font><br>
                                年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;龄：<font color="#f6dd55">20</font><br>
                                所&nbsp;&nbsp;在&nbsp;地：<font color="#e29400">北京市</font>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="middle_bottom">
            </div>
        </div>
        <div class="bottom">
            ©2009美天网络科技 　网页游戏、广告合作 电话：(0532)86667258 邮箱：wxs@xba.com.cn
            <font color="#e29400">
                官方客服QQ：1970285928
            </font>
            <br>
            增值电信业务经营许可证鲁B2-20071037号 纠纷处理方式：联系客服或依《用户协议》约定方式处理
            <br>
            <br>
            鲁ICP备07004287号 网络文化经营许可证:文网文[2008]037号
        </div>
    </div>
    <!--[if IE 6]>    <script src="JS/DD_belatedPNG_0.0.8a.js"></script>    <script>    DD_belatedPNG.fix('.logo img,.home,.middle_left,.star_title,.ban_r,.ban_d,.news_title,.star1,.star2,.star3,.star4,.star5,.num,.news_middle_left_title');    </script>    <![endif]-->
</body>

</html>