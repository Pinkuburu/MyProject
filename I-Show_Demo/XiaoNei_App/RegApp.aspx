<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegApp.aspx.cs" Inherits="XiaoNei_App.RegApp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:xn="http://www.renren.com/2009/xnml">
<head>
  <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
  <title>Renren Connect</title>
</head>
<body>
  <script type="text/javascript" src="http://static.connect.renren.com/js/v1.0/FeatureLoader.jsp"></script>
  <script type="text/javascript">
    XN_RequireFeatures(["EXNML"], function()
    {
      XN.Main.init("2c3dae2f4a494b7898b8dd361783f8e2", "http://xbas.xba.dishun.net/xn/xd_receiver.htm");
      var callback = function(){ window.location.reload() }
      var cancel = function(){ alert('Authorize Failed!'); }
      XN.Connect.showAuthorizeAccessDialog(callback,cancel);
    });
  </script>
</body>
</html>

