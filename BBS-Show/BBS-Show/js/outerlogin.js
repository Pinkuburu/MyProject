function writeLoginPanel(aa) {
    if (!aa || !aa.domainlist || -1 == aa.domainlist.indexOf(".")) {
        return;
    }

    var bbm = 'if(0 == this.uin.value.length){ this.uin.focus(); return false;};if(0 == this.pwd.value.length){ this.pwd.focus(); return false;};this.submit();this.pwd.value=\'\';return false;',
bDK = '<div id="divLoginpanelHor" class="bizmail_loginpanel" style="width:957px;"><div class="bizmail_LoginBox"><h3>登录邮箱</h3><form action="https://exmail.qq.com/cgi-bin/login" target="_blank" method="post" onsubmit="' + bbm + '"><input type="hidden" name="firstlogin" value="false" /><input type="hidden" name="errtemplate" value="dm_loginpage" /><input type="hidden" name="aliastype" value="other" /><input type="hidden" name="dmtype" value="bizmail" /><input type="hidden" name="p" value="" /><label>帐号:</label><input type="text" name="uin" class="text" value="" />@#domainlist#<label>&nbsp;密码:</label><input type="password" name="pwd" class="text" value="" /><input type="submit" class="" name="" value="登录" />&nbsp;<a href="https://exmail.qq.com/cgi-bin/readtemplate?check=false&t=bizmail_orz" target="_blank">忘记密码？</a></form></div></div>',
bDF = '<div id="divLoginpanelVer" class="bizmail_loginpanel"><div class="bizmail_LoginBox"><h3>登录邮箱</h3><form action="https://exmail.qq.com/cgi-bin/login" target="_blank" method="post" onsubmit="' + bbm + '"><input type="hidden" name="firstlogin" value="false" /><input type="hidden" name="errtemplate" value="dm_loginpage" /><input type="hidden" name="aliastype" value="other" /><input type="hidden" name="dmtype" value="bizmail" /><input type="hidden" name="p" value="" /><div class="bizmail_column"><div class="bizmail_inputArea"><input type="text" name="uin" class="text" value="" />@#domainlist#</div></div><div class="bizmail_column"><div class="bizmail_inputArea"><input type="password" name="pwd" class="text" value="" /></div></div><div class="bizmail_SubmitArea"><input type="submit" class="" name="" style="width:66px;" value="登录" /><a href="https://exmail.qq.com/cgi-bin/readtemplate?check=false&t=bizmail_orz" target="_blank">忘记密码？</a></div></form></div></div>';

    var nD = aa.domainlist.split(";");
    if (nD.length == 1) {
        var atl = '<span>#domain#</span><input type="hidden" name="domain" value="#domain#" />';
        var gh = atl.replace(/#domain#/g, nD[0]);
    }
    else {
        var gh = '<select name="domain">';
        for (i = 0; i < nD.length; i++) {
            gh += '<option value="' + nD[i] + '">' + nD[i] + '</option>';
        }
        gh += '</select>';
    }

    if (!aa.mode || aa.mode == "vertical" || aa.mode == "both") {
        document.write(bDF.replace(/#domainlist#/g, gh));
    }
    if (aa.mode == "horizontal" || aa.mode == "both") {
        document.write(bDK.replace(/#domainlist#/g, gh));
    }
}