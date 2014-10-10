/*
Ajax 类
sUrl : 目标 URL
sQueryString : 提交变量
callbackFunc : 回调函数
callbackParams : 回调函数参数
sRecvType : 返回值格式 ( 0: 文本, 1: XML );
*/

function Ajax ( sUrl, sQueryString, callbackFunc, callbackParams, sRecvType )
{
	this.url = sUrl;
	this.queryString = sQueryString != null ? sQueryString : '';
	this.response; // 返回值
	this.maxRetry = 3; // 最大重试次数
	this.countRetry = 0; // 重试次数

	this.xmlHttp = this.createXMLHttpRequest ();
	if ( this.xmlHttp == null )
	{
		return false;
	}
	//alert(sUrl);
	var objxml = this.xmlHttp;
	objxml.onreadystatechange = function ()
	{
		try
		{
			Ajax.handleStateChange ( objxml, sRecvType, callbackFunc, callbackParams )
		}
		catch ( e ) {}	
	}
}

Ajax.prototype.createXMLHttpRequest = function ()
{
	try
	{
		return new ActiveXObject ( 'Msxml2.XMLHTTP' );
	}catch(e) {}

	try
	{
		return new ActiveXObject ( 'Microsoft.XMLHTTP' );
	} catch(e) {}

	try
	{
		return new XMLHttpRequest ();
	} catch(e) {}

	return null;
}

Ajax.prototype.createQueryString = function ()
{
	var queryString = '';
	if ( this.queryString != null && typeof ( this.queryString ) != 'string' )
	{
		var elements = this.queryString.elements;
		//alert(elements);
		var pairs = new Array();
		for ( var i = 0; i < elements.length; i++ )
		{
			if ( ( name = elements[i].name ) && ( value = elements[i].value ) )
			{
				var eType = elements[i].getAttribute('type');
				if ( ( eType != 'radio' && eType != 'checkbox' ) || elements[i].checked )
				{
					pairs.push ( name + "=" + encodeURIComponent ( value ) );
				}
			}
		}
		queryString = pairs.join ( '&' );
	}
	else
	{
		queryString = this.queryString;
	}
	return queryString;
}

Ajax.prototype.get = function ()
{
	sUrl = this.url;

	var queryString = sUrl;
	if ( extraQueryString = this.createQueryString() )
	{
		queryString += ( queryString.indexOf ('?') > 0 ? '&' : '?' ) + extraQueryString;
	}
	this.xmlHttp.open ( 'GET', queryString, true );
	this.xmlHttp.send ( null );
}

Ajax.prototype.post = function ()
{
	var sUrl = this.url;
	var queryString = this.createQueryString ();
	this.xmlHttp.open ( 'POST', sUrl, true ); // 使用 POST 时, 必须添加下面这一行
	this.xmlHttp.setRequestHeader ( 'Content-Type', 'application/x-www-form-urlencoded' );
	this.xmlHttp.send ( queryString );
}

Ajax.handleStateChange = function ( xmlHttp, sRecvType, callbackFunc, callbackParams )
{
	if ( xmlHttp.readyState == 4 )
	{
		if ( xmlHttp.status == 200  )
		{
		    clearTimeout(alertError);
			Response = sRecvType ? xmlHttp.responseXML : xmlHttp.responseText;
			if ( callbackFunc != null )
			{
				callbackFunc ( Response, callbackParams );
				return true;
			}
			
		}
	}
}