Core.reg(null, function(api) {

	var hotkeys = {
		"engineList": {
			"g":"http://www.google.com.hk/search?client=aff-maxthon-dh&amp;channel=cts&amp;q=",
			"t":"http://60.28.220.95/p_start/start.php?sDomin=taobao&amp;sKey=",
			"b":"http://www.baidu.com/s?tn=myie2dg&amp;ch=1&amp;ie=utf-8&amp;wd="}, 
		"hot": [		
		{"n":"XBA游戏网","type":"g s n ","l":"http://www.xba.com.cn"},
		{"n":"一球成名","type":"b s n","l":"http://zq2.xba.com.cn"},
		{"n":"凡人诛仙传","type":"b s n","l":"http://rpg.xba.com.cn"},
		{"n":"XBA篮球经理","type":"g s n","l":"http://lq.xba.com.cn/"},
		{"n":"梦幻足球","type":"b s n","l":"http://zq.xba.com.cn/"},
		{"n":"人才招募","type":"b s n","l":"http://www.xba.com.cn/IndexNews/newcontent15.aspx"}		
		]
	};

	api.notify('start-hotkeys', hotkeys);
});