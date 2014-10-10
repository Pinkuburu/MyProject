<!--
var alertError;

// 放到网上的时候可以禁止别的页面调用，但是在本地测试的时候要先注销掉
/*
var arrHost = window.location.host.split ( '.' );
var domain = arrHost[arrHost.length - 2] + '.' + arrHost[arrHost.length - 1];
if ( document.domain != domain )
{
	document.domain = domain;
}
*/

/*
window.onerror = handle_error;
function handle_error ( e, uri, ln )
{
	// alert ( "Error: " + e + "\nURL: " + uri + "\nLine: " + ln );
	return true;
}
*/

function $ ( objId )
{
	return document.getElementById ( objId );
}

// 保存 Cookie
function setCookie ( name, value )
{
	expires = new Date();
	expires.setTime(expires.getTime() + (1000 * 86400 * 365));
	document.cookie = name + "=" + escape(value) + "; expires=" + expires.toGMTString() +  "; path=/";
}

// 获取 Cookie
function getCookie ( name )
{
	cookie_name = name + "=";
	cookie_length = document.cookie.length;
	cookie_begin = 0;
	while (cookie_begin < cookie_length)
	{
		value_begin = cookie_begin + cookie_name.length;
		if (document.cookie.substring(cookie_begin, value_begin) == cookie_name)
		{
			var value_end = document.cookie.indexOf ( ";", value_begin);
			if (value_end == -1)
			{
				value_end = cookie_length;
			}
			return unescape(document.cookie.substring(value_begin, value_end));
		}
		cookie_begin = document.cookie.indexOf ( " ", cookie_begin) + 1;
		if (cookie_begin == 0)
		{
			break;
		}
	}
	return null;
}

// 清除 Cookie
function delCookie ( name )
{
	var expireNow = new Date();
	document.cookie = name + "=" + "; expires=Thu, 01-Jan-70 00:00:01 GMT" +  "; path=/";
}

// 获取 URL 变量
//queryVar 正则表达式变量
function getURLVar ( queryVar, url )
{
	var url = url == null ? window.location.href : url;
	var re = new RegExp ( ".*[\?|&]" + queryVar + "=([^&#]+).*", "g" );
	
	if ( url.match ( re ) )
	{
		var queryVal = url.replace ( re, "$1" );
		return queryVal;
	}
	return '';
}

// 获取当前域名
function getURLHost ( url )
{
	if ( url == null )
	{
		return window.location.host;
	}

	var re = new RegExp ( "([^\/]*)\/\/([^\/]*)\/.*", "g" );
	
	if ( url.match ( re ) )
	{
		var urlHost = url.replace ( re, "$1//$2" );
		return urlHost;
	}
	return '';
}

function setSelectOptions ( the_form, the_select, do_check )
{
	var selectObject = document.forms[the_form].elements[the_select];
	var selectCount  = selectObject.length;

	for (var i = 0; i < selectCount; i++) {
		  selectObject.options[i].selected = do_check;
	}
	return true;
}

function setChecked ( val, obj, name )
{
	len = obj.elements.length;
	var i=0;
	for( i=0 ; i<len ; i ++ )
	{
		if ( obj.elements[i].name == name )
		{
			obj.elements[i].checked = val;
		}
	}
}

// 在指定对象显示信息
function showMessage ( msg, objName )
{
	var obj;
	if ( typeof (objName) == 'string' )
	{
		obj = $ ( objName );
	}
	else
	{
		obj = objName;
	}

	process.finish ();

	if ( obj )
	{
		obj.innerHTML = msg;
		if ( obj.className.indexOf ( 'dialog_box' ) >= 0 )
		{
			obj.style.zIndex = gzIndex;
			if ( msg.indexOf ( 'dialog.close' ) < 0 )
			{
				appendCloseButton ( obj );
			}
		}
		try
		{
			obj.focus ();
			obj.scrollTop = 0;
		} catch (e) {}
	}
	else
	{
		showDialogBox ( msg, objName, true );
	}

	// 解析 Script
	execScript ( msg );
}


// 添加关闭按钮
function appendCloseButton ( obj, closeScript, hideMask )
{
	var closeButton = document.createElement ( 'div' );
	closeButton.className = 'dialog_close';
	if ( hideMask == null ) hideMask = true;
	closeButton.onclick = function () {
		if ( closeScript != null ) eval ( closeScript );
		dialog.close(this, hideMask);//
		//removeDialogBox(obj, hideMask);
	}
	closeButton.onmouseover = function () { this.style.marginTop = '1px'; }
	closeButton.onmouseout = function () { this.style.marginTop = '0px'; }

	obj.appendChild(closeButton);//closeButton
}

// 添加最小化按钮
function appendMiniButton ( obj, miniScript )
{
	var miniButton = document.createElement ( 'div' );
	miniButton.className = 'dialog_mini';
	miniButton.onclick = function () {
		var lastWidth = obj.offsetWidth;
		if ( obj.style.left.indexOf ( '-' ) == 0 ) obj.style.left = '0px';
		if ( obj.style.top.indexOf ( '-' ) == 0 ) obj.style.top = '0px';
		if ( miniScript != null ) eval ( miniScript );
	}
	miniButton.onmouseover = function () { this.style.marginTop = '1px'; }
	miniButton.onmouseout = function () { this.style.marginTop = '0px'; }
	obj.appendChild ( miniButton );
}

// 解析 Script
function execScript ( msg )
{
	var _re = /<script[^>]*>([^\x00]+)$/i
	var _msgs = msg.split ( "<\/script>" );
	for ( var _i in _msgs )
	{
		var _strScript;
		if ( _strScript = _msgs[_i].match ( _re ) )
		{
			var _strEval = _strScript[1].replace ( /<!--/, '' );
			try
			{
				eval ( _strEval );
			}
			catch (e) {}
		}
	}
}

// 移除对话框
function removeDialogBox ( objName, hideMask )
{
	if ( objName == null )
	{
		objName = 'dialog_box' + mask.maskIndex;
	}
	if ( hideMask == null ) hideMask = true;
	if ( hideMask ) mask.hide ();
	removeItems ( objName );
	gzIndex --;
}

// 移除物件
function removeItems ( objName )
{
	var obj;
	objs = getObjects ( objName );
	for ( var i in objs )
	{
		try
		{
			obj = objs[i];
			obj.parentNode.removeChild ( obj );
		}
		catch (e) {}
	}
}

// 显示提示窗口
var gzIndex = 999;
function showDialogBox ( msg, objId, noClose, title )
{
	var objClass = 'dialog_box';
	if(title==null)title="XBA"
	
	if ( objId == null )
	{
		objId = objClass + mask.maskIndex;
	}

	var msgBox = document.getElementById ( objId );
	if ( msgBox )
	{
		msgBox.parentNode.removeChild ( msgBox );
		mask.hide ();
	}
	if ( msg != '' ) mask.show ();
	msgBox = document.createElement ( 'div' );
	msgBox.id = objId;
	msgBox.className = objClass;
	document.body.appendChild ( msgBox );
	msgBox.style.zIndex = gzIndex ++;

    var strDiv = "";
    strDiv+="<table width=\"400\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" id=\"floatmain\">"
    +"<tr><td>"
    +"<table border=\"0\" width=\"99.5%\" cellspacing=\"0\" cellpadding=\"0\" id=\"floatborder\">"
    //+"<tr><td id=\"floatop\">"+title+"</td></tr>"
    +"<tr><td id=\"floatcontent\">"+msg+"</td></tr>"
    +"</table>"
    +"</td></tr>"
    +"</table>";
	msgBox.innerHTML = strDiv;

	if ( noClose == null ) noClose = false;
	if ( !noClose && msg.indexOf ( 'dialog.close' ) < 0 )
	{
		appendCloseButton( msgBox );
	}
	centerDiv ( msgBox );
}

// 显示帮助窗口
function showHelpBox ( msg, objId, title, posTop )
{
	var objClass = 'dialog_box_help';
	if ( objId == null ) objId = 'helpDialog';
	if ( title == null ) title = '足球经理手册';

	var contentId = objId + 'Content';

	var msgBox = $ ( objId );

	if ( !msgBox )
	{
		msgBox = document.createElement ( 'div' );
		msgBox.id = objId;
		msgBox.className = objClass;
		if ( posTop != null )
		{
			msgBox.style.top = posTop + 'px';
		}
		document.body.appendChild ( msgBox );

		if ( document.all )
		{
			var showHideScript = 'var obj = $(\"' + contentId + '\"); if ( obj.style.display != \"none\" ) { obj.style.display = \"none\"; } else { obj.style.display = \"\"; }';
		}
		else
		{
			var showHideScript = 'var obj = $(\"' + contentId + '\"); if ( obj.style.visibility != \"hidden\" ) { obj.style.width = \"0px\"; obj.style.height = \"0px\"; obj.style.visibility = \"hidden\"; } else { obj.style.width = \"auto\"; obj.style.height = \"auto\"; obj.style.visibility = \"visible\"; }';
		}

		msgBox.innerHTML = "<div class='title' onmousedown='setDragObj (\"" + objId + "\", 1);'>" + title + "</div><div id='" + contentId + "' class='content' style='visibility:'>" + msg + "</div>";

		appendMiniButton ( msgBox, showHideScript );
		appendCloseButton ( msgBox, null, false );

	}
	else
	{
		var objContent = $ ( contentId );
		if ( document.all )
		{
			objContent.style.display = '';
		}
		else
		{
			objContent.style.visibility = 'visible';
			objContent.style.width = 'auto';
			objContent.style.height = 'auto';
		}
	}
	msgBox.style.zIndex = gzIndex ++;
}

// 字数限制函数
function lengthLimit ( obj, Limit, objShow, objAlert )
{
	var Len = obj.value.replace(/([^\x00-\xff])/g,"EE").length;
	if ( Len > Limit )
	{
		//obj.value = obj.value.substring ( 0, Limit );
		Len = Limit;
		showMessage ( String.sprintf ( "字数超出限制, 最多 %d 字!", Limit ), objAlert );
	}
	if ( objShow = document.getElementById ( objShow ) )
	{
		objShow.innerHTML = Len;
	}	
}

// 列表搜索
function clientSearch ( value, obj )
{
	if ( value != '' )
	{
		for ( var i = obj.selectedIndex + 1; i < obj.options.length; i ++ )
		{
			if ( obj.options[i].text.indexOf ( value, 0 ) >= 0 )
			{
				obj.selectedIndex = i;
				return true;
			}		
		}
	}
	obj.selectedIndex = 0;
}

// 列表项目移动
function moveOptions ( objFrom, objTo, errMsg, moveList )
{
	moveList = moveList != null ? moveList : '';
	moveList = ',' + moveList + ',';
	if ( objFrom.selectedIndex == -1 && errMsg != null )
	{
		alert ( errMsg );
	}
	for ( var i = 0; i < objFrom.options.length; i ++ )
	{
		if ( moveList.match ( ',-1,' ) || moveList.match ( ',' + objFrom.options[i].value + ',' ) || objFrom.options[i].selected )
		{
			objTo.options.add ( new Option ( objFrom.options[i].text, objFrom.options[i].value ) );
			objFrom.options[i--] = null;
		}
	}
}

// 设置状态栏
function setStatus ( w, id )
{
	window.status = w;
	return true;
}
function clearStatus ()
{
	window.status = '';
}

// 显示物件
function showItem ( obj )
{
	if ( typeof (obj) == 'string' )
	{
		obj = document.getElementById ( obj );
	}
	try
	{
		obj.style.display = '';
	}
	catch (e) {}
}

// 隐藏物件
function hideItem ( obj )
{
	if ( typeof (obj) == 'string' )
	{
		obj = document.getElementById ( obj );
	}
	try
	{
		obj.style.display = 'none';
	}
	catch (e) {}
}

// 交替显示/隐藏物件
function itemShowHide ( obj )
{
	if ( typeof ( obj ) == 'string' )
	{
		obj = document.getElementById ( obj );
	}
	try
	{
		if ( obj.style.display != '' )
		{
			showItem ( obj );
		}
		else
		{
			hideItem ( obj );			
		}
	}
	catch (e)
	{
	}
}

// 获取错误信息
function getError ( string )
{
	var errorFlag = '<ERROR>';
	if ( string.substring ( 0, errorFlag.length ) == errorFlag )
	{
		return string.substring ( errorFlag.length, string.length );
	}
	else
	{
		return false;
	}
}

// 根据标签获取物件
function getTag ( obj, tagName, index )
{
	if ( typeof ( obj ) == 'string' )
		obj = $ ( obj );
	var tags = obj.getElementsByTagName ( tagName );
	if ( index != null )
		return tags[index];
	else
		return tags;
}

// Ajax 通用回调函数
function ajaxCallback ( ret, obj )
{
	var errorMsg = getError ( ret );
	if ( errorMsg )
	{
		if ( !window.onErrorMsg )
		{
			window.onErrorMsg = true;
			showMessage ( errorMsg );
		}
	}
	else if ( ret != '' )
	{
		showMessage ( ret, obj );
	}
}

function ajaxCallbackNo ( ret, obj )
{
	var errorMsg = getError ( ret );
	if ( errorMsg )
	{
		if ( !window.onErrorMsg )
		{
			window.onErrorMsg = true;
			showMessage ( errorMsg );
		}
	}
	else if ( ret != '' )
	{
		showMessageNo ( ret, obj );
	}
}

// 固定 Div
function stayDiv ( obj, top, left )
{
	if ( typeof ( obj ) == 'string' )
	{
		obj = document.getElementById ( obj );
	}
	top = top != null ? top : 0;
	left = left != null ? left : 0;
	obj.style.top = top + document.documentElement.scrollTop + 'px';
	obj.style.left = left + document.documentElement.scrollLeft + 'px';
	setTimeout ( "stayDiv('" + obj.id + "'," + top + "," + left + ")", 100 );
}


// Div 居中
function centerDiv ( obj, repeat )
{
	if ( typeof ( obj ) == 'string' )
	{
		obj = document.getElementById ( obj );
	}
	if ( obj )
	{
		obj.style.top = '50%';
		obj.style.left = '50%';
		try
		{
			obj.style.marginLeft = ( - obj.scrollWidth / 2 + document.documentElement.scrollLeft ) + 'px';
			obj.style.marginTop = ( - obj.scrollHeight / 2 + document.documentElement.scrollTop ) + 'px';
		}
		catch (e) {}

		if ( repeat == null ) repeat = true;
		if ( repeat )
		{
			setTimeout ( "centerDiv('" + obj.id + "')", 500 );
		}
	}
}

// 设置背景色
function setBg ( obj, color )
{
	if ( typeof ( obj ) == 'string' )
	{
		obj = document.getElementById ( obj );
	}
	try
	{
		obj.style.backgroundColor = color;
	}
	catch (e)
	{
	}
}

// html 特殊字符
function htmlchars ( string )
{
	string = string.replace ( /\"/g, '&quot;' );
	string = string.replace ( /\'/g, '&#039;' );

	return string;
}

// 格式化输出
function sprintf ()
{
	if ( sprintf.arguments.length < 2 )
	{
		return;
	}
	var data = sprintf.arguments[ 0 ];
	for( var k=1; k<sprintf.arguments.length; ++k )
	{
		switch( typeof( sprintf.arguments[ k ] ) )
		{
			case 'string':
			data = data.replace( /%s/, sprintf.arguments[ k ] );
			break;
			case 'number':
			data = data.replace( /%d/, sprintf.arguments[ k ] );
			break;
			case 'boolean':
			data = data.replace( /%b/, sprintf.arguments[ k ] ? 'true' : 'false' );
			break;
			default:
			break;
		}
	}
	return( data );
}

 if ( !String.sprintf ) String.sprintf = sprintf;

// 图层定位
function moveDivHere ( obj, loadingMsg )
{
	try
	{
		if ( obj != null )
		{
			if ( loadingMsg != null ) obj.innerHTML = loadingMsg;

			// 获取当前鼠标坐标
			var posX = clsMouseCoords.x + 5;
			var posY = clsMouseCoords.y + 5;
			obj.style.left = posX + 'px';
			obj.style.top = posY + 'px';
		}
	}
	catch (e)
	{
	}
}

// 复制URL地址
function setCopy (_sTxt)
{
	if ( navigator.userAgent.toLowerCase().indexOf ( 'ie' ) > -1 )
	{
		clipboardData.setData ( 'Text', _sTxt );
		alert ( '网址“' + _sTxt + '”\n已经复制到您的剪贴板中\n您可以使用Ctrl+V快捷键粘贴到需要的地方' );
	}
	else
	{
		prompt ( '请复制网站地址:', _sTxt ); 
	}
}

// 设为首页
function setHomePage ( url )
{
	var obj = getSrcElement ();

	if ( document.all )
	{
		obj.style.behavior='url(#default#homepage)';
		obj.setHomePage ( url );
	}
	else
	{
		if ( window.netscape )
		{
			try
			{  
				netscape.security.PrivilegeManager.enablePrivilege ( "UniversalXPConnect" );
			}
			catch (e)  
			{  
				alert ( "您的浏览器不支持自动设置首页，请自行手动设置。");
			}
		}

		var prefs = Components.classes['@mozilla.org/preferences-service;1'].getService(Components.interfaces.nsIPrefBranch);
		prefs.setCharPref ( 'browser.startup.homepage', url );
	}
}

// 加入收藏
function addBookmark ( site, url )
{
	if ( document.all )
	{
		window.external.addFavorite ( url, site );
	}
	else if ( window.sidebar )
	{
		window.sidebar.addPanel ( site, url, "" )
	}
	else if ( navigator.userAgent.toLowerCase().indexOf ( 'opera' ) > -1 )
	{
		alert ( '请使用 Ctrl+T 将本页加入收藏夹' );
	}
	else
	{
		alert ( '请使用 Ctrl+D 将本页加入收藏夹' );
	}
}

// 语言包支持
function lang ()
{
	var strInput = langPackage[lang.arguments[0]];
	var strParams = '';

	for( var k=1; k < lang.arguments.length; ++k )
	{
		switch( typeof( lang.arguments[ k ] ) )
		{
			case 'string':
				strParams += ", '" + lang.arguments[ k ] + "'";
			break;
			case 'number':
				strParams += ", " + lang.arguments[ k ] + "";
			break;
		}
	}
	if ( strParams != '' )
	{
		strEval = "strOutput = String.sprintf ( strInput" + strParams + " );";
		eval ( strEval );
	}
	else
	{
		strOutput = strInput;
	}	
	return ( strOutput ); 
}

// 获取鼠标坐标
function mouseCoords ()
{
	this.x = 0;
	this.y = 0;

	this.getMouseCoords = function ()
	{
		var ev = searchEvent ();
		try
		{
			if ( ev.pageX || ev.pageY )
			{
				this.x = ev.pageX;
				this.y = ev.pageY;
			}
			else
			{
				this.x = ev.clientX + document.documentElement.scrollLeft - document.documentElement.clientLeft;
				this.y = ev.clientY + document.documentElement.scrollTop  - document.documentElement.clientTop;
			}
		}
		catch (e) {}
	}
}

// 设置拖动物件
var clsMouseCoords = new mouseCoords ();
var dragObj = false;
var dragModeX = 0; // 0: left, 1: right
var dragModeY = 0; // 0: top, 1: bottom
var dragAlpha = 0; // 透明度

var dragObjFilter = '';

function setDragObj ( obj, modeX, modeY, alpha )
{
	if ( typeof ( obj ) == 'string' ) obj = $( obj );
	if ( modeX == null ) modeX = 0;
	if ( modeY == null ) modeY = 0;
	if ( alpha == null ) dragAlpha = 0;

	mask.show ( false, obj.style.zIndex );

	clsMouseCoords.getMouseCoords ();

	dragObj = obj;
	dragModeX = modeX;
	dragModeY = modeY;
	dragAlpha = alpha;

	if ( dragModeX == 0 ) window.dragX = clsMouseCoords.x - dragObj.offsetLeft;
	else window.dragX = clsMouseCoords.x - dragObj.offsetLeft - dragObj.offsetWidth;
	if ( dragModeY == 0 ) window.dragY = clsMouseCoords.y - dragObj.offsetTop;
	else window.dragY = clsMouseCoords.y - dragObj.offsetTop - dragObj.offsetHeight;

	dragObjFilter = dragObj.style.filter;
	if ( dragAlpha > 0 )
	{
		dragObj.style.filter = 'alpha(opacity=' + dragAlpha + ')';
		dragObj.style.opacity = dragAlpha / 100;
		// setAlpha ( dragObj.id, 100, 60 );
	}

	document.onmousemove = doDragObj;
	document.onmouseup = clearDragObj;
}

function clearDragObj ()
{
	if ( typeof ( dragObj ) == 'object' )
	{
		if ( dragModeX == 0 )
		{
			if ( dragObj.style.left.indexOf ( '-' ) == 0 ) dragObj.style.left = '0px';
		}
		else
		{
			if ( dragObj.style.right.indexOf ( '-' ) == 0 ) dragObj.style.right = '0px';
		}

		if ( dragModeY == 0 )
		{
			if ( dragObj.style.top.indexOf ( '-' ) == 0 ) dragObj.style.top = '0px';
		}
		else
		{
			if ( dragObj.style.bottom.indexOf ( '-' ) == 0 ) dragObj.style.bottom = '0px';
		}

		if ( dragAlpha > 0 )
		{
			dragObj.style.filter = dragObjFilter;
			dragObj.style.opacity = '1.0';
			// setAlpha ( dragObj.id, 60, 100 );
		}
		dragObj.focus ();
		dragObj = false;
		mask.hide ();
	}
}

function setAlpha ( objId, opacity, maxOpacity, step )
{
	if ( opacity == maxOpacity )
	{
		return true;
	}
	else
	{
		var obj = $ ( objId );
		if ( step == null ) step = 10;
		opacity += opacity > maxOpacity ? - step : step;
		obj.style.filter = 'alpha(opacity=' + opacity + ')';
		setTimeout ( "setAlpha ( '" + objId + "', " + opacity + ", " + maxOpacity + " )", 100 );
	}
}

function doDragObj ()
{
	if ( typeof ( dragObj ) == 'object' )
	{
		clsMouseCoords.getMouseCoords ();
		if ( dragModeX == 0 )
		{
			dragObj.style.left = ( clsMouseCoords.x - window.dragX ) + 'px';
		}
		else
		{
			var objHTML = document.getElementsByTagName('html').item(0);
			dragObj.style.right = ( objHTML.offsetWidth - clsMouseCoords.x + window.dragX ) + 'px';
		}

		if ( dragModeY == 0 )
		{
			dragObj.style.top = ( clsMouseCoords.y - window.dragY ) + 'px';
		}
		else
		{
			var objHTML = document.getElementsByTagName('html').item(0);
			dragObj.style.bottom = ( objHTML.offsetHeight - clsMouseCoords.y + window.dragY ) + 'px';
		}	
	}
}

// 更改所有名称为 objName 的物件的 innerHTML
function setValue ( objName, value )
{
	try
	{
		objs = getObjects ( objName );
		for ( var i in objs )
		{
			objs[i].innerHTML =value;
		}
	}
	catch (e){}
}

// 获取物件
function getObjects ( objName )
{
	var objs = new Array ();

	if ( idObjs = document.getElementById ( objName ) )
	{
		objs.push ( idObjs );
	}

	if ( document.all ) // IE
	{
		var objTypes = new Array ( 'table', 'tr', 'td', 'div', 'li', 'span', 'a' );
		for ( var tagType in objTypes )
		{
			var typeObjs = document.getElementsByTagName ( objTypes[tagType] );
			for ( var i in typeObjs )
			{
				try
			    {
			    if ( typeObjs[i].name == objName )
				{
					    objs.push ( typeObjs[i] );
				}
				}catch(e){}
			}
		}
	}
	else // 其他浏览器
	{
		nameObjs = document.getElementsByName ( objName );
		for ( var i in nameObjs )
		{
			objs.push ( nameObjs[i] );
		}
	}

	return objs;
}

// 获得焦点
function setFocus ( objName, select )
{
	var obj;

	var objs = document.getElementsByName ( objName );
	if ( objs.length > 0 )
	{
		obj = objs.item(0);
	}
	else
	{
		obj = document.getElementById ( objName );
	}
	if ( obj )
	{
		if ( select == null ) select = false;
		if ( select )
		{
			obj.select ();
		}
		else
		{
			obj.focus ();
		}
	}
}

// 高亮物件
function highlight ( obj, highlightClass )
{
	if ( typeof ( obj ) == 'string' )
	{
		obj = $ ( obj );
	}
	if ( highlightClass == null )
	{
		highlightClass = 'highlight';
	}
	try 
	{
		for ( var i in obj.parentNode.childNodes )
		{
			if ( obj.parentNode.childNodes[i].className != null )
			{
				var re = new RegExp ( "[ ]*" + highlightClass );
				obj.parentNode.childNodes[i].className = obj.parentNode.childNodes[i].className.replace ( re, '' );
			}
		}
		obj.className = obj.className+" "+highlightClass
	}
	catch ( e ) {}
}

// 球衣高亮
function selectclothes ( obj , Number, Type, highlightClass )
{
	if ( typeof ( obj ) == 'string' )
	{
		obj = document.getElementById ( obj );
	}
	if ( highlightClass == null )
	{
		highlightClass = 'picurrent';
	}
	try 
	{
		for ( var i in obj.parentNode.childNodes )
		{
			if ( obj.parentNode.childNodes[i].className != null )
			{
				var re = new RegExp ( "[ ]*" + highlightClass );
				obj.parentNode.childNodes[i].className = obj.parentNode.childNodes[i].className.replace ( re, 'clothes' );
			}
		}
		
		if(Type==1)
		    document.getElementById("hdClubLogo").value=Number;
		else if(Type==2)
		    document.getElementById("hdhostClother").value=Number;
		else
		    document.getElementById("hdvisitClother").value=Number;
		    
		obj.className = highlightClass;
	}
	catch ( e ) {}
}

// 载入物件
function objLoader ()
{
	this.timeStamp = null;
	this.loadedJs = '';
	this.loadedCss = '';

	// 载入指定页面到指定物件
	/*
		loadUrl : 载入页面的 URL
		targetObj : 目标容器物件 ID
		queryString : 附加提交变量
		loadJs : 附加 Js 文件
		loadingMsg : 载入中提示文字
		method: 以某种方式传输 GET和POST 两种，GET会受到65535字节的限制，POST不会受到该限制
	*/
	this.get = function ( loadUrl, targetObj, queryString, loadingMsg, callbackFunc, loadJs )
	{
		this.load ( 'GET', loadUrl, targetObj, queryString, loadingMsg, callbackFunc, loadJs );
	}

	this.post = function ( loadUrl, targetObj, queryString, loadingMsg, callbackFunc, loadJs )
	{
		this.load ( 'POST', loadUrl, targetObj, queryString, loadingMsg, callbackFunc, loadJs );
	}

	this.load = function ( method, loadUrl, targetObj, queryString, loadingMsg, callbackFunc, loadJs )
	{
	    this.refreshCache ();
		if ( !loadUrl ) return;
		var obj;
		if ( typeof ( targetObj ) == 'string' )
		{
			obj = $ ( targetObj );
		}
		else
		{
			obj = targetObj;
		}
		
		if ( obj )
		{
			if ( loadingMsg != null ) obj.innerHTML = loadingMsg;
		}
		
		if ( callbackFunc == null )
		{
			callbackFunc = ajaxCallback;
		}

		this.getTimeStamp ();
		var re = new RegExp ( "timeStamp=([0-9]+)" );
		if ( loadUrl.match ( re ) )
		{
			loadUrl = loadUrl.replace ( re, 'timeStamp=' + this.timeStamp );
		}
		else
		{
			loadUrl += loadUrl.indexOf ( '?' ) == -1 ? '?' : '&';
			loadUrl += 'timeStamp=' + this.timeStamp;
		}
		if ( queryString == null ) queryString = '';

		if ( window.ajaxProxy && loadUrl.indexOf ( getURLHost ( $( 'ajaxProxy' ).src ) ) >= 0 )
		{
			var clsAjax = new ajaxProxy.Ajax ( loadUrl, queryString, callbackFunc, targetObj );
		}
		else
		{
			var clsAjax = new Ajax ( loadUrl, queryString, callbackFunc, targetObj );
		}
		if ( loadJs != null )
		{
			this.loadJs ( loadJs );
		}
		if ( method.toLowerCase () == 'post' )
		{
			clsAjax.post ();
		}
		else
		{
			clsAjax.get ();
		}
		//mask.show();
	}

	// 载入 Js
	this.loadJs = function ( file, reload )
	{
		if ( !document.getElementById )
		{
			return;
		}

		if ( reload == null ) reload = false;

		var fileref = '';
		if ( reload || this.loadedJs.indexOf ( file ) == -1 )
		{
			fileref = document.createElement ( 'script' );
			fileref.setAttribute ( 'type', 'text/javascript' );
			fileref.setAttribute ( 'src', file );
		}
		if ( fileref != '' )
		{
			document.getElementsByTagName('head').item(0).appendChild ( fileref );
			this.loadedJs += file + ' ';
		}
		return fileref;
	}

	// 载入 Css
	this.loadCss = function ( file, reload )
	{
		if ( !document.getElementById )
		{
			return;
		}

		if ( reload == null ) reload = false;

		var fileref = '';
		if ( reload || this.loadedCss.indexOf ( file ) == -1 )
		{
			fileref=document.createElement ( 'link' );
			fileref.setAttribute ( 'rel', 'stylesheet' );
			fileref.setAttribute ( 'type', 'text/css' );
			fileref.setAttribute ( 'href', file );
		}
		if ( fileref != '' )
		{
			document.getElementsByTagName('head').item(0).appendChild ( fileref );
			this.loadedCss += file + ' ';
		}
	}

	// 设置时间戳, 用于控制页面缓存
	this.refreshCache = function ()
	{
		this.timeStamp = this.makeTimeStamp ();
	}

	// 生成时间戳
	this.makeTimeStamp = function ()
	{
		var dateTime = new Date ();
		var timeStamp = dateTime.getTime ();
		if ( typeof ( timeStamp ) == 'undefined' || timeStamp == null )
		{
			timeStamp = Math.floor ( Math.random () * 10000 * 10000 );
		}
		return timeStamp;
	}
	
	// 获取缓存时间戳
	this.getTimeStamp = function ()
	{
		if ( typeof ( this.timeStamp ) == 'undefined' || this.timeStamp == null ) this.timeStamp = this.makeTimeStamp ();
		return this.timeStamp;
	}
}

var loader = new objLoader ();
loader.refreshCache ();

// 遮罩
var noMask = false;
function clsMask ()
{
	this.maskId = 'mask';
	this.maskIndex = 0;
	this.lastHTMLStyle = null;

	this.show = function ( showFrame, zIndex )
	{
		if ( noMask ) return false;
		if ( showFrame == null ) showFrame = true;
		this.maskIndex ++;
		var maskName = this.maskId + this.maskIndex;
		var mask = document.getElementById ( maskName );
		if ( mask )
		{
			mask.parentNode.removeChild ( mask );
		}
		mask = document.createElement ( 'div' );
		mask.id = maskName;
		mask.className = 'mask';

		zIndex = parseInt ( zIndex );
		if ( isNaN ( zIndex ) ) zIndex = ++ gzIndex;
		mask.style.zIndex = zIndex;

		if ( showFrame )
		{
			mask.className += ' alhpa';
			var maskFrame = document.createElement ( 'iframe' );
			maskFrame.id = maskName + '_frame';
			maskFrame.className = this.maskId;
			maskFrame.style.zIndex = zIndex;
			document.body.appendChild ( maskFrame );
		}

		document.body.appendChild ( mask );

		var objHTML = document.getElementsByTagName('html').item(0);
		if ( this.lastHTMLStyle == null ) this.lastHTMLStyle = objHTML.style.overflow;
		objHTML.style.overflow = 'hidden';
	}

	this.hide = function ()
	{
		if ( noMask ) return false;
		var maskFrame = document.getElementById ( this.maskId + this.maskIndex + '_frame' );
		if ( maskFrame )
		{
			try
			{
				maskFrame.parentNode.removeChild ( maskFrame );
			}
			catch ( e )
			{
			}
		}
		var mask = document.getElementById ( this.maskId + this.maskIndex );
		if ( mask )
		{
			var objHTML = document.getElementsByTagName('html').item(0);
			mask.parentNode.removeChild ( mask );
			this.maskIndex --;
			gzIndex --;
			if ( this.maskIndex == 0 )
			{
				objHTML.style.overflow = this.lastHTMLStyle;
			}
		}
	}
}
var mask = new clsMask ();

var objHTML = document.getElementsByTagName('html').item(0);

// 处理过程提示框
function clsProcess ( dialogName, className ) 
{
	this.processing = false;
	this.dialogName = dialogName;
	this.chkTimeOut = null;
	this.timeOut = 15; // 超时秒数
	this.showMask = false;
	if ( className == null ) className = 'process';
	this.className = className;

	if ( this.dialogName == null ) this.dialogName = 'processing_box';

	// 正在处理
	this.start = function ( procMsg )
	{
		if ( !this.processing )
		{
			if ( procMsg == null ) procMsg = '正在处理，请稍候...';
			showDialogBox ( '<table class="box" style="width:300px;height:60px;"><tr><td align="center"><table align="center"><tr><td align="left" class="message">&nbsp;&nbsp;' + procMsg + '</td></tr></table></td></tr><table>', this.dialogName, true );
			setFocus ( this.dialogName );
			this.processing = true;
			if ( this.timeOut > 0 )
			{
				this.chkTimeOut = setTimeout ( this.className + ".timeOut()", this.timeOut * 1000 );
			}
		}
	}
	
//	// 正在处理无关闭
//	this.startno = function ( procMsg )
//	{
//		if ( !this.processing )
//		{
//			if ( procMsg == null ) procMsg = '正在处理，请稍候...';
//			showDialogBoxNo ( '<table class="box" style="width:300px;height:60px;"><tr><td align="center"><table align="center"><tr><td align="left" class="message"><img alt="" align="absmiddle" src="http://static.9way.cn/football/icons/icon-loading.gif" />&nbsp;&nbsp;' + procMsg + '</td></tr></table></td></tr><table>', this.dialogName);
//			setFocus ( this.dialogName );
//			this.processing = true;
//			if ( this.timeOut > 0 )
//			{
//				this.chkTimeOut = setTimeout ( this.className + ".timeOut()", this.timeOut * 1000 );
//			}
//		}
//	}

	// 处理完成
	this.finish = function ()
	{
		if ( this.processing )
		{
			objContentBody = document.getElementById ( 'body' );
			// objContentBody.scrollTop = 0;
			removeDialogBox ( this.dialogName );
			this.processing = false;			
			clearTimeout ( this.chkTimeOut );
		}
	}

	this.timeOut = function ()
	{
		this.finish ();
		show_alert ( '网络连接超时，请重试！' );
	}
}
var process = new clsProcess ();

// 数值操作类
function clsNumber ( clsName, funcCallback, callbackParams )
{
	this.clsName = clsName;
	this.adding = 0;
	this.interval = null;
	this.callback = funcCallback != null ? funcCallback : null; // 回调函数
	this.callbackParams = callbackParams != null ? callbackParams : null; // 回调函数参数

	this.value = 0;
	this.minValue = 0;
	this.maxValue = -1;

	this.check = function ( obj )
	{
		if ( typeof ( obj ) == 'string' )
		{
			obj = document.getElementById ( obj );
		}
		objValue = Math.round ( obj.value );
		if ( isNaN ( objValue ) || objValue < this.minValue )
		{
			obj.value = this.minValue;
		}
		else if ( this.maxValue >= 0 && objValue > this.maxValue )
		{
			obj.value = this.maxValue;
		}
		this.value = obj.value;
		if ( this.callback != null )
		{
			this.callback ( this.callbackParams );
		}
	}

	this.add = function ( objId, quantity, minValue, maxValue )
	{
		if ( minValue == null ) minValue = this.minValue;
		if ( maxValue == null ) maxValue = this.maxValue;

		obj = document.getElementById ( objId );
		obj.value = Math.round ( obj.value ) + quantity;
		if ( this.adding > 5 )
		{
			obj.value = Math.round ( obj.value / 10 ) * 10;
		}
		if ( this.minValue >= 0 && obj.value <= this.minValue && quantity < 0 )
		{
			obj.value = minValue;
		}
		else if ( maxValue >= 0 && obj.value >= maxValue && quantity > 0 )
		{
			obj.value = maxValue;
		}
		this.value = obj.value;
		this.check ( obj );
	}

	this.startAdd = function ( objId, quantity, minValue, maxValue )
	{
		if ( minValue == null ) minValue = this.minValue;
		if ( maxValue == null ) maxValue = this.maxValue;

		if ( this.adding == 0 )
		{
			this.adding = true;
			this.doAdd ( objId, quantity, minValue, maxValue );
		}
	}

	this.doAdd = function ( objId, quantity, minValue, maxValue )
	{
		if ( this.adding > 0 )
		{
			this.adding ++;

			var addQuantity = Math.max ( 1, Math.floor ( this.adding / 5 ) * 10 ) * quantity;

			this.add ( objId, addQuantity, minValue, maxValue );
			this.interval = setTimeout ( this.clsName + ".doAdd('" + objId + "'," + quantity + "," + minValue + "," + maxValue + ")", 100 );
		}
		else
		{
			clearTimeout ( this.interval );
		}
	}

	this.finishAdd = function ()
	{
		this.adding = 0;
	}
}

var number = new clsNumber ( 'number' );

// 载入完成后调用函数
function callAfterLoaded ( callback, script )
{
	if ( document.all ) // IE 支持
	{
		script.onreadystatechange = function ()
		{
			if ( script.readyState == 'loaded' || script.readyState == 'complete' )
			{
				callback();
			}
		}
	}
	else // Firefox 支持
	{
		script.onload = callback;
	}
}

// 按 td 的 c 属性对查看表格
function table_view_by ( objList, c )
{
	if ( typeof ( objList ) == 'string' )
	{
		objList = document.getElementById ( objList );
	}
	if ( !objList )
	{
		return false;
	}
	var objSubs = objList.getElementsByTagName ( 'td' );
	for ( var i in objSubs )
	{
		try
		{
			var itemCate = objSubs[i].getAttribute ( 'c' );
			if ( itemCate == c )
			{
				objSubs[i].style.display = '';
			}
			else if ( itemCate != null )
			{
				objSubs[i].style.display = 'none';
			}
		}
		catch (e) {}
	}
}

// 随机数
function get_rand ( min, max )
{
	var range = max - min;
	var rand = Math.random();
	return ( min + Math.round ( rand * range ) );
}

// 预载入图片
function pre_load_image ()
{
	var doc = document;
	if ( doc.images )
	{
		doc.preLoadImages = new Array ();
		var args = pre_load_image.arguments;
		for ( var i = 0; i < args.length; i ++ )
		{
			doc.preLoadImages[i] = new Image ();
			doc.preLoadImages[i].src = args[i];
		}
	}
}

// 判定数组中是否存在
function in_array ( value, array )
{
	for ( var i in array )
	{
		if ( array[i] == value )
		{
			return true;
		}
	}
	return false;
}

// 显示图片或 Flash
function show_object ( filename, width, height, objId )
{
	var parts = filename.split ( '.' );
	var fileType = parts.pop ();
	var html;
	var strObjId = strObjName = '';

	if ( objId != null )
	{
		strObjId = ' id="' + objId + '"';
		strObjName = ' name="' + objId + '"';
	}

	switch ( fileType )
	{
		case 'swf':
		{
			html = '<object' + strObjId + ' classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="' + width + '" height="' + height + '"><param name="movie" value="' + filename + '" /><param name="allowscriptaccess" value="always" /><param name="quality" value="high" /><param name="menu" value="false" /><param name="wmode" value="transparent" /><embed' + strObjName + ' src="' + filename + '" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" menu="false" wmode="transparent" width="' + width + '" height="' + height + '" allowscriptaccess="always" swLiveConnect="true"></embed></object>';
			break;
		}
		default :
		{
			html = '<img' + strObjId + ' alt="" src="' + filename + '" />';
		}
	}

	return html;
}

// 限制文本框字数
function maxLength ( obj, length, showAlert )
{
	if ( obj.value.replace(/([^\x00-\xff])/g,"EE").length > length )
	{
		//obj.value = obj.value.substring ( 0, length );
		if ( showAlert != null )
		{
			show_alert ( '<center>很抱歉，您最多只能输入 ' + length + ' 个字符</center>' );
		}
		return false;
	}
}

// 查找 Event 对象
function searchEvent ()
{
	if ( window.event ) return window.event;
	var func = searchEvent.caller;
	while ( func != null )
	{
		var firstArg = func.arguments[0];
		if ( firstArg )
		{
			if ( firstArg.constructor == MouseEvent || firstArg.constructor == Event ) return firstArg;
		}
		func = func.caller;
	}
	return null;
}

function getSrcElement ()
{
	try
	{
		var evt = searchEvent ();
		var srcElem = evt.target;
		if ( typeof ( evt.target ) != 'object' )
		{
			var srcElem = evt.srcElement;
		}
		return srcElem;
	}
	catch (e) 
	{
		return null;
	}
}

// 获取上级对话框
function getParentDialog ( obj, className )
{
	if ( typeof ( obj ) == 'string' )
	{
		obj = $ ( obj );
	}
	if ( className == null ) className = 'dialog_box';
	if ( typeof ( obj ) == 'object' && obj.className && obj.className.indexOf ( className ) == 0 ) return obj;
	var pObj = obj.parentNode;
	if ( pObj == null ) return null;
	if ( pObj.className == className ) return pObj;
	else return getParentDialog ( pObj );
}

