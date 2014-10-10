<!--
// 捕捉键盘事件
function keyEvent (e)
{
	var evt = e ? e : window.event;
	var keyCode = parseInt ( evt.keyCode );

	if ( keyCode == 27 )
	{
		dialogs = document.getElementsByTagName ( 'div' );
		for ( var i = 0; i < dialogs.length; i ++ )
		{
			if ( dialogs[i].className == 'dialog_box' )
			{
				dialog.close ( dialogs[i] );
				return true;
			}
		}
	}
}

// 获取网站域名
function getRootURL ()
{
	var str = window.location.host.split ( '.' );
	var domain = '';
	if ( ( str[1] == 'my' || str[1] == 'game' ) && ( str[0] == 'ctc' || str[0] == 'cnc' ) )
	{
		domain = str[0] + '.';
		return 'http://' + domain + str[2] + '.' + str[3];
	}
	else if ( str.length == 3 )
	{
		return 'http://www.' + str[1] + '.' + str[2];
	}
	else
	{
		return '/';
	}
}

// 载入 loadUrl 页面内容到 targetObj 物件
function load_page ( loadUrl, targetObj, refresh )
{
	if ( refresh == null ) refresh = false;
	if ( refresh )
	{
		loader.refreshCache ();
	}
	loader.get ( loadUrl, targetObj );
}

// 载入页面 ( 带判断, 若 targetObj 不存在则不执行载入 )
function load_page_with_chk ( loadUrl, targetObj, refresh )
{
	if ( typeof ( targetObj ) == 'string' )
	{
		targetObj = $ ( targetObj );
	}
	if ( targetObj )
	{
		load_page ( loadUrl, targetObj, refresh );
	}
}

// 显示对话框
function show_alert ( message, scriptCode, scriptText )
{
	process.finish ();

	if ( scriptCode == null ) scriptCode = '';
	if ( scriptText == null ) scriptText = '确定';

	scriptCode = htmlchars ( scriptCode );

	var html = '';
	html += '<div style="width:400px; text-align:center">';
	html += message;
	html += '<br><input onclick="this.blur();dialog.close(this);" class="button50" type="button" value="确定"/>'
	html += '</div>';
	

	showDialogBox ( html );
	setFocus ( 'alert_button' );
}


// 显示确认框
function show_confirm ( message, scriptCodeConfirm, scriptCodeCancel, scriptTextConfirm, scriptTextCancel )
{

	if ( scriptCodeConfirm == null ) scriptCodeConfirm = '';
	if ( scriptCodeCancel == null ) scriptCodeCancel = '';
	if ( scriptTextConfirm == null ) scriptTextConfirm = '确定';
	if ( scriptTextCancel == null ) scriptTextCancel = '取消';

	scriptCodeConfirm = htmlchars ( scriptCodeConfirm );
	scriptCodeCancel = htmlchars ( scriptCodeCancel );

	var html = '';
	html += '<div style="width:300px; height:30px; line-height:30px; text-align:center;">';
	html += message;
	html += '</div><div style="width:300px; text-align:center">';
	html += "<input class=\"button50\" name=\"confirm_button\" type=\"button\" onclick=\"this.blur(); dialog.close(this);" + scriptCodeConfirm + "\" value=\"" + scriptTextConfirm + "\" />";
	html += '&nbsp;&nbsp;';
	html += '<input class="button50" name="cancel_button" type="button" onclick="this.blur(); dialog.close(this); ' + scriptCodeCancel + '" value="' + scriptTextCancel + '" />';
	html += '</div>';

	showDialogBox ( html );
	setFocus ( 'confirm_button' );
}

// 载入子菜单
function load_menu ( menuName, defaultIndex, refresh )
{
	var objMenuItem = $ ( menuName );
	if ( !objMenuItem ) return false;

	var objMenu = $('tabmenu');
	var tags = objMenu.getElementsByTagName ( 'ul' );
	for ( var i in tags )
	{
		if ( tags[i].style )
			tags[i].style.visibility = 'hidden';
	}
	objMenuItem.style.visibility = 'visible';
	
	if ( defaultIndex != null )
	{
		try
		{
			var objItem = getTag ( menuName, 'a', defaultIndex );
			objItem.onclick ();
		}
		catch ( e ) {}
	}
}

// 载入子页面内容
var last_content_url; // 最后子页面 url
function load_content ( loadUrl, targetObj, queryString, objHighlight, refresh, hideProcess )
{
	if ( refresh == null ) refresh = false;
	if ( hideProcess == null ) hideProcess = false;

	if ( refresh )
		loader.refreshCache ();

	last_content_url = loadUrl;
	if ( !hideProcess )
	    process.start ( '载入中，请稍候...' );
	
	if ( typeof ( targetObj ) == 'string' ) targetObj = $(targetObj);
	if(targetObj ==null )
	    targetObj = $('content');
	//alert(targetObj.id);
	loader.get ( loadUrl, targetObj, queryString, null, callback_load_content );
	
	
	if ( typeof ( objHighlight ) == 'string' ) objHighlight = $(objHighlight);
	if ( objHighlight != null )
		highlight ( objHighlight );
}

function callback_load_content ( ret, obj )
{
	ajaxCallback ( ret, obj );
	
	// 自动打开页面提示
	try
	{
		var objBody = $ ( 'body' );
		objBody.scrollTop = 0;
		var showTipsSwitch = getCookie ( 'showTipsSwitch' );
		if ( showTipsSwitch == null || showTipsSwitch != 'close' )
		{
			var objTip = $ ( 'tips' );
			objTip.onclick ();
		}
	}
	catch ( e ) 	{}
}

// 刷新子页面内容
function refresh_content ()
{
	load_page ( last_content_url, 'content', true );
}

// 对话框操作类
function clsDialog ( dialogName )
{
	this.dialogIndex = 0;
	this.dialogName = dialogName;

	if ( this.dialogName == null ) this.dialogName = 'dialog';

	// 载入至浮动对话框
	this.open = function ( loadUrl, dialogName, procMsg )
	{
		if ( dialogName == null ) dialogName = this.dialogName + ( ++this.dialogIndex );
		var objDialog = $ ( dialogName );

        if ( procMsg == null )
            procMsg = '载入中，请稍候...';
            
		process.start ( procMsg );
		load_page ( loadUrl, dialogName );
	}

	// 载入至当前浮动对话框
	this.load = function ( loadUrl )
	{
		var obj = getSrcElement ();
		var pObj = getParentDialog ( obj );
		if ( pObj )
		{
			dialogName = pObj.id;
			pObj.style.zIndex = gzIndex;
		}
		if ( dialogName == null ) dialogName = this.dialogName + this.dialogIndex;
		process.start ( '载入中，请稍候...' );
		load_page ( loadUrl, dialogName );
	}

	// 提交表单至浮动对话框
	this.post = function ( loadUrl, form, procMsg, callbackFunc)
	{
		var pObj = getParentDialog ( form );
		if ( pObj ) dialogName = pObj.id;
		if ( dialogName == null ) dialogName = this.dialogName + this.dialogIndex;
		if ( typeof ( form ) == 'string' )
		{
			form = $ ( form );
		}
		process.start ( procMsg );
		loader.refreshCache ();
		loader.post ( loadUrl, dialogName, form , null, callbackFunc);
	}

	// 关闭浮动对话框
	this.close = function ( obj, hideMask )
	{
		if ( obj == null ) var obj = getSrcElement ();
		var pObj = getParentDialog ( obj );
		if ( pObj ) dialogName = pObj.id;
		if ( dialogName == null ) dialogName = this.dialogName + this.dialogIndex;
		if ( dialogName.indexOf ( this.dialogName ) == 0 )
		{
			this.dialogIndex --;
		}
		window.onErrorMsg = false;
		if ( hideMask == null ) hideMask = true;
		//alert(dialogName);
		removeDialogBox ( dialogName, hideMask );
//		location.href = 'Default_2.aspx';
	}
}
