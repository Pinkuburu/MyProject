        
function Timer1() {
    $("body").everyTime('4s',vGetRand);     //大厅计时器
}

function Timer2() {
    $("body").everyTime('4s',ShowConcern);  //关注计时器
}

function Timer3() {
    $("body").everyTime('4s',ShowFans);     //粉丝计时器
}
    
function vGetRand() {
    var p = $("#hdpage").val();
    $.get("Refresh.aspx",
    {
        rnd:Math.random(),
        page:p,
        UserID:intUserID
    },
    function(data){
        $(".hic_pic").html(data);
    });
}

function ShowConcern() {
    var p = $("#hdpage").val();
    $("body").stopTime();   
     
    $.get("ConcernList.aspx",
    {
        page:p,
        UserID:intUserID,
        rnd:Math.random()
    },
    function(data){
        $(".hic_pic").html(data);
        if(data.indexOf("当前没有满足查询条件的用户") < 0)
        {
            Timer2();            
        }
    });    
}

function ShowFans() {
    var p = $("#hdpage").val();
    $("body").stopTime();
    
    $.get("FansList.aspx",
        {
            page:p,
            UserID:intUserID,
            rnd:Math.random()
        },
        function(data){
            $(".hic_pic").html(data);
            if(data.indexOf("当前没有满足查询条件的用户") < 0)
            {
                Timer3();                
            }
        });    
}
 
function ShowRoom() {        
    $("#hdpage").attr('value', 1);
    vGetRand();
    RoomPage();
    $("body").stopTime();
    $("body").everyTime('4s',vGetRand);
}
 
function SearchUserList() {
    if (isVIP) {    
        $("body").stopTime();
        var strProvince = $("#prv").find("option:selected").text();
        var strCity = $("#city").find("option:selected").text();
        var strGender = $("#gender").val();
        var strCategory = $("#age").val();
        var strNickName = $("#NickName").val();
        
        if(jQuery.trim(strNickName).length > 0 && strNickName != "请输入用户昵称")
        {
            $.get("Search.aspx?type=2&UserID=" + intUserID + "&NickName=" + encodeURI(strNickName) + "&rnd=" + Math.random(),
            function (data){
                $("#hic_pic").html(data);
            });
        }
        else
        {
    //        if($("#prv").find("option:selected").text() == "— —省")
    //        {
    //            alert("没选省就开始查啊");
    //            return;
    //        }
    //        
    //        if($("#city").find("option:selected").text() == "— —市")
    //        {
    //            alert("没选市就开始查啊");
    //            return;
    //        }
            
            $.get("Search.aspx?type=1&page=1&UserID=" + intUserID + "&Province=" + encodeURI(strProvince) + "&City=" + encodeURI(strCity) + "&Gender=" + encodeURI(strGender) + "&Category=" + encodeURI(strCategory) + "&rnd=" + Math.random(),
                function (data){
                    $("#hic_pic").html(data);
                });
            SearchPage();
        }
    }
    else
    {
        ShowDialog("系统提示","只有会员享有该功能。");
    }
}

function SearchUser() {
    $("body").stopTime();
    var strCategory = $("#Category").val();

    $.get("Search.aspx?type=2&NickName=" + encodeURI(strCategory) + "&rnd=" + Math.random(),
        function (data){
            $("#hic_pic").html(data);
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
            ShowDialog("系统提示","不能给自己发消息");
            //alert("不能给自己发消息");
            return;
        }
        if(strContent.length == 0)
        {
            ShowDialog("系统提示","消息内容不能为空");
            //alert("消息内容不能为空");
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
                    ShowDialog("系统提示","发送成功");
                    //alert("发送成功");
                    $("#SendMessage").css("display","none");
                    $("#Content").text("");
                }
                else
                {
                    ShowDialog("系统提示","距离下次发言还需" + data.CDTime + "分钟。会员享有畅所欲言的特权。");
                    //alert("发送失败");
                    $("#SendMessage").css("display","none");
                }		            
            });
    }
} 

//////////////////////////////////////////////////////////////////////////////
//添加关注
function AddConcern(intUserID,intConcernID) {
    //alert(intUserID + "," + intConcernID);
    $.getJSON("ConcernOp.aspx",
        {
            Type:1,
            UserID:intUserID,
            ConcernID:intConcernID,
            rnd:Math.random()
        },
        function (data){
            if(data.Status == 1)
            {
                ShowDialog("系统提示","添加关注成功");
                //alert("添加关注成功");
            }
            else if(data.Status == -1)
            {
                ShowDialog("系统提示","您所关注的名额已满。会员享有关注人数无上限的特权。");
                //alert("不能重复添加关注");
            }
            else
            {
                ShowDialog("系统提示","不能重复添加关注");
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
        function (data){
            if(data.Status == 1)
            {
                ShowDialog("系统提示","删除关注成功");
                //alert("删除关注成功");
            }
            else
            {
                ShowDialog("系统提示","不能重复删除关注");
                //alert("不能重复删除关注");
            }
        });
}

function CheckSpace() {
    var strProvince=$("#HidProvince").val();
    var strCity=$("#hddCity").val();
}

function ShowDialog(title,content){
    dialog(title,"text:" + content,"200px","auto","text");
}

function BoxAutoHidden() {
    $("body").oneTime('5s','D',function(){
    //do something...
    });
}

function CheckVIP() {
    var sReturn;
    $.getJSON("Login.aspx",
    {
        Type:2,
        UserID:intUserID,
        rnd:Math.random()
    },
    function (data) {
        if(data.VIP == 1)
        {
            sReturn = true;
        }
        else
        {
            sReturn = false;
        }
    });
    return sReturn;
}
/////////////////////////////////////////////////////////////////////////////////////////

//设置大厅分页显示
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
            $(".pages").paginate({
                count 		: pageSize,
                start 		: 1,
                display     : 4,
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
						                            $(".hic_pic").html(data)
					                            });
					                        $("#hdpage").attr('value', pages);
				                          }
            });
        }
    }); 
}
	            
//设置关注分页显示
function ConcernPage() {
    var pageSize = 0;
    $.getJSON("Page.aspx",
    {
        Type:2,
        UserID:intUserID,
        rnd:Math.random()
    },
    function (data){
        pageSize = data.Page;
        if(pageSize > 0)
        {
            $(".pages").paginate({
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
				                                    $("#hic_pic").html(data);
			                                    });
			                                $("#hdpage").attr('value', pages);
		                                  }
            });
        }
        else
        {
            $(".pages").html("");
        }
    });
}

//设置搜索分页显示
function SearchPage() {
    $("body").stopTime();
    var pageSize = 0;
    var strProvince = $("#prv").find("option:selected").text();
    var strCity = $("#city").find("option:selected").text();
    var strGender = $("#gender").val();
    var strCategory = $("#age").val();
    
    $.getJSON("Page.aspx",
    {
        Type:4,
        Province:strProvince,
        City:strCity,
        Gender:strGender,
        Category:strCategory,
        rnd:Math.random()
    },
    function (data){
        pageSize = data.Page;
        if(pageSize > 0)
        {
            $(".pages").paginate({
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
		                                    $.get("Search.aspx?type=1&page=" + pages + "&UserID=" + intUserID + "&Province=" + encodeURI(strProvince) + "&City=" + encodeURI(strCity) + "&Gender=" + encodeURI(strGender) + "&Category=" + encodeURI(strCategory) + "&rnd=" + Math.random(),
		                                        function(data){
			                                        $("#hic_pic").html(data);
		                                        });
		                                    $("#hdpage").attr('value', pages);
	                                      }
            });
        }
        else
        {
            $(".pages").html("");
        }
    });
}

//设置粉丝分页显示
function FansPage() {
    var pageSize = 0;
    $.getJSON("Page.aspx",
    {
        Type:3,
        UserID:intUserID,
        rnd:Math.random()
    },
    function (data){
        pageSize = data.Page;
        if(pageSize > 0)
        {
            $(".pages").paginate({
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
			                                $.get("FansList.aspx?page=" + pages + "&UserID=" + intUserID + "&rnd=" + Math.random(),
			                                    function(data){
				                                    $("#hic_pic").html(data);
			                                    });
			                                $("#hdpage").attr('value', pages);
		                                  }
            });
        }
        else
        {
            $(".pages").html("");
        }
    });
}