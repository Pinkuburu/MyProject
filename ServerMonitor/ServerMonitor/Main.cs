using System;
using System.Windows.Forms;
using XPTable.Models;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace ServerMonitor
{
    public partial class Main : Form
    {
        public string strServer = null;
        public string strUserName = null;
        public string strPassword = null;
        public int intHeight = 0;
        public int intServerCount = 0;
        public int intOnlineCount = 0;
        public int intOffLineCount = 0;

        IniFile ini = new IniFile(@"ServerConfig.ini");
        XMPPClass objXmpp;
        Thread th_Main;

        public delegate void ToolStripMenuItem();
        public delegate void ToolStripMenuItem_Enable(object sender);
        public delegate void del_AddRecord(string strOnlineStatus, string strServer, string strServerName);
        public delegate void del_UpdateInfo(string SendUser, string strContent);

        public Main()
        {
            InitializeComponent();
            //System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            debugToolStripMenuItem.Enabled = false;
            客户端ToolStripMenuItem.Enabled = false;
            系统操作ToolStripMenuItem.Enabled = false;

            label_Status.Text = "";

            if (!File.Exists(@"ServerConfig.ini"))
            {
                Config cfg = new Config();
                cfg.ShowDialog(this);
            }
            else
            {
                this.strServer = ini.GetString("ConnectionServer", "ServerAddress", "");
                this.strUserName = ini.GetString("ConnectionServer", "UserName", "");
                this.strPassword = ini.GetString("ConnectionServer", "Password", "");
                this.strPassword = Cryptography.DESDecrypt(this.strPassword, "xmb*8g4f");
                Log.WriteLog(LogFile.Trace, "配置文件加载成功");
            }
            this.Text = string.Format("ServerMonitor    {0}/{1}/{2}", this.intOffLineCount, this.intOnlineCount, this.intServerCount);
        }

        #region table1 初始化
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            table1.BeginUpdate();
            ImageColumn imageColumn1 = new ImageColumn("#", 16);//声明一个新列

            TextColumn textColumn1 = new TextColumn("Server", 150);//声明一个新列
            textColumn1.Editable = false;
            textColumn1.Visible = false;

            TextColumn textColumn2 = new TextColumn("Name", 100);
            textColumn2.Editable = false;

            TextColumn textServerIP = new TextColumn("IP", 100);
            textServerIP.Editable = false;
            //textServerIP.Visible = false;

            TextColumn textVersion = new TextColumn("Version", 30);
            textVersion.Alignment = ColumnAlignment.Center;

            ProgressBarColumn Cpu = new ProgressBarColumn("CPU", 60);
            ProgressBarColumn disk_C = new ProgressBarColumn("C:", 60);
            ProgressBarColumn disk_D = new ProgressBarColumn("D:", 60);
            ProgressBarColumn disk_E = new ProgressBarColumn("E:", 60);
            ProgressBarColumn disk_F = new ProgressBarColumn("F:", 60);
            ProgressBarColumn disk_G = new ProgressBarColumn("G:", 60);

            TextColumn textStatus = new TextColumn("Status", 48);

            table1.ColumnModel = new ColumnModel(new Column[] { imageColumn1, textColumn1, textColumn2, textServerIP, Cpu, disk_C, disk_D, disk_E, disk_F, disk_G, textVersion, textStatus });//把声明的列添加到列中
            //table1.HeaderRenderer = new GradientHeaderRenderer();//设置样式
            table1.FullRowSelect = true;
            table1.EndUpdate();

            //===================================
            //Row row;
            //Cell cell;
            //row = new Row();
            //row.Cells.Add(new Cell("Offline", (Image)Properties.Resources.offline));
            //row.Cells.Add(new Cell("strCell2"));
            //model.Rows.Add(row);
            //table1.TableModel = model;
            //===================================
        }
        #endregion table1 初始化

        #region 事件处理
        
        void objXmpp_FriendList(agsXMPP.protocol.iq.roster.RosterItem item)//好友列表事件【2】
        {
            del_AddRecord AddRecord_del = new del_AddRecord(AddRecord);
            this.Invoke(AddRecord_del, new string[] { "unavailable", item.Jid, "" });
            Thread.Sleep(50);
            //AddRecord("unavailable", item.Jid, "");
        }

        void objXmpp_ServerStatus(agsXMPP.protocol.client.Presence pres)//用户在线状态事件【3】
        {
            del_AddRecord AddRecord_del = new del_AddRecord(AddRecord);
            this.Invoke(AddRecord_del, new object[] { pres.Type.ToString(), pres.From.Bare, pres.Status });
            Thread.Sleep(50);
            //AddRecord(pres.Type.ToString(), pres.From.Bare, pres.Status);
        }

        void objXmpp_OnlineStatus(object sender)//登录状态事件【1】
        {
            //label_Status.Text = sender.ToString();

            ToolStripMenuItem_Enable ToolStripMenuItem_Enable = new ToolStripMenuItem_Enable(TSMI_enable);
            this.Invoke(ToolStripMenuItem_Enable, new object[] { sender });
 
            //#region 菜单激活
            //debugToolStripMenuItem.Enabled = true;
            //客户端ToolStripMenuItem.Enabled = true;
            //系统操作ToolStripMenuItem.Enabled = true;
            //#endregion 菜单激活
        }

        private void TSMI_enable(object sender)
        {
            label_Status.Text = sender.ToString();
            #region 菜单激活
            debugToolStripMenuItem.Enabled = true;
            客户端ToolStripMenuItem.Enabled = true;
            系统操作ToolStripMenuItem.Enabled = true;
            #endregion 菜单激活
        }

        void objXmpp_Messages(agsXMPP.protocol.client.Message msg)//消息事件【4】
        {
            UpdateInfo(msg.From.Bare, msg.Body);
        }

        #endregion 事件处理

        #region 记录添加 AddRecord(string strOnlineStatus, string strServer, string strServerName)
        /// <summary>
        /// 记录添加
        /// </summary>
        /// <param name="strOnlineStatus">在线状态</param>
        /// <param name="strServer">服务器</param>
        /// <param name="strServerName">服务器名称</param>
        private void AddRecord(string strOnlineStatus, string strServer, string strServerName)
        {
            if (!SearchTable(strServer))
            {
                Row row = new Row();
                if (strOnlineStatus == "available")
                {
                    row.Cells.Add(new Cell("Online", (Image)Properties.Resources.online));
                    this.intOnlineCount++;
                    this.intServerCount++;
                    this.Text = string.Format("ServerMonitor    {0}/{1}/{2}", this.intOffLineCount, this.intOnlineCount, this.intServerCount);
                }
                else
                {
                    row.Cells.Add(new Cell("Offline", (Image)Properties.Resources.offline));
                    this.intOffLineCount++;
                    this.intServerCount++;
                    this.Text = string.Format("ServerMonitor    {0}/{1}/{2}", this.intOffLineCount, this.intOnlineCount, this.intServerCount);
                }
                row.Cells.Add(new Cell(strServer));
                row.Cells.Add(new Cell(strServerName));
                row.Cells.Add(new Cell(""));
                row.Cells.Add(new Cell(0));
                row.Cells.Add(new Cell(0));
                row.Cells.Add(new Cell(0));
                row.Cells.Add(new Cell(0));
                row.Cells.Add(new Cell(0));
                row.Cells.Add(new Cell(0));
                row.Cells.Add(new Cell(0));
                row.Cells.Add(new Cell(0));
                tableModel1.Rows.Add(row);
                table1.TableModel = tableModel1;
                table1.TableModel.RowHeight = 20;
            }
            else
            {
                ChangeStatus(strServer, strOnlineStatus, strServerName);
            }
        }
        #endregion 记录添加 AddRecord(string strOnlineStatus, string strServer, string strServerName)

        #region 查询重复记录 SearchTable(string strContent)
        /// <summary>
        /// 查询重复记录
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        private bool SearchTable(string strContent)
        {
            int i = 0;
            int intRows = 0;
            string strRowsContent = null;

            if (table1.TableModel == null)
            {
                return false;
            }
            else
            {
                intRows = table1.TableModel.Rows.Count;
            }

            if (intRows > 0)
            {
                for (i = 0; i < intRows; i++)
                {
                    strRowsContent = table1.TableModel.Rows[i].Cells[1].Text.ToString();
                    if (strRowsContent == strContent)
                    {
                        return true;
                    }
                }
            }            
            return false;
        }
        #endregion 查询重复记录 SearchTable(string strContent)

        #region 状态改变 ChangeStatus(string strServer, string strOnlineStatus, string strServerName)
        /// <summary>
        /// 状态改变
        /// </summary>
        /// <param name="strServer">服务器</param>
        /// <param name="strOnlineStatus">在线状态</param>
        /// <param name="strServerName">服务器名称</param>
        private void ChangeStatus(string strServer, string strOnlineStatus, string strServerName)
        {
            int i = 0;
            int intRows = 0;
            string strRowsContent = null;

            intRows = table1.TableModel.Rows.Count;

            if (strOnlineStatus == "available" && intRows > 0)
            {
                this.intOnlineCount++;
                this.intOffLineCount--;
                this.Text = string.Format("ServerMonitor    {0}/{1}/{2}", this.intOffLineCount, this.intOnlineCount, this.intServerCount);

                for (i = 0; i < intRows; i++)
                {
                    strRowsContent = table1.TableModel.Rows[i].Cells[1].Text.ToString();
                    if (strRowsContent == strServer)
                    {
                        table1.TableModel.Rows[i].Cells[0].Image = (Image)Properties.Resources.online;
                        table1.TableModel.Rows[i].Cells[0].Text = "Online";
                        if (strUserName != "")
                        {
                            table1.TableModel.Rows[i].Cells[2].Text = strServerName;
                        }
                        Log.WriteLog(LogFile.Trace, "Server: " + strServer + "|Online");
                        return;
                    }
                }                
            }
            else
            {
                this.intOnlineCount--;
                this.intOffLineCount++;
                this.Text = string.Format("ServerMonitor    {0}/{1}/{2}", this.intOffLineCount, this.intOnlineCount, this.intServerCount);

                for (i = 0; i < intRows; i++)
                {
                    strRowsContent = table1.TableModel.Rows[i].Cells[1].Text.ToString();
                    if (strRowsContent == strServer)
                    {
                        table1.TableModel.Rows[i].Cells[0].Image = (Image)Properties.Resources.offline;
                        table1.TableModel.Rows[i].Cells[0].Text = "Offline";
                        Log.WriteLog(LogFile.Trace, "Server: " + strServer + "|Offline");
                        return;
                    }
                }
            }
        }
        #endregion 状态改变 ChangeStatus(string strServer, string strOnlineStatus, string strServerName)

        #region 更新系统信息 UpdateInfo(string SendUser, string strContent)
        /// <summary>
        /// 更新系统信息
        /// </summary>
        /// <param name="SendUser">发送消息的服务器</param>
        /// <param name="strContent">消息内容</param>
        private void UpdateInfo(string SendUser, string strContent)
        {
            string strMessageSource = strContent;
            string strDiskInfo = null;
            string strSystemInfo = null;

            int i = 0;
            int intRows = 0;
            string strRowsContent = null;

            intRows = table1.TableModel.Rows.Count;
            //Log.WriteLog(LogFile.Trace, "SendUser: " + SendUser + "|" + strContent);

            if (intRows > 0)
            {
                string[] aryStatus = strMessageSource.Split('|');
                switch (aryStatus[0])
                {
                    case "Status":
                        for (i = 0; i < intRows; i++)
                        {
                            strRowsContent = table1.TableModel.Rows[i].Cells[1].Text.ToString();
                            if (strRowsContent == SendUser)
                            {
                                table1.TableModel.Rows[i].Cells[11].Text = aryStatus[1];
                            }
                        }
                        break;
                    default:
                        try
                        {
                            strDiskInfo = Regex.Match(strMessageSource, @"\{.*\}").Value.Replace("{", "").Replace("}", "");
                            strSystemInfo = strMessageSource.Replace(strDiskInfo, "").Replace("{", "").Replace("}", "");
                            string[] arySystemInfo = strSystemInfo.Split('|');
                            string[] aryDiskInfo = strDiskInfo.Split('|');

                            for (i = 0; i < intRows; i++)
                            {
                                strRowsContent = table1.TableModel.Rows[i].Cells[1].Text.ToString();
                                if (strRowsContent == SendUser)
                                {
                                    if (strUserName != "")
                                    {
                                        table1.TableModel.Rows[i].Cells[3].Text = arySystemInfo[0].ToString();//IP
                                        table1.TableModel.Rows[i].Cells[4].Data = Convert.ToInt32(arySystemInfo[1]);//CPU
                                        try
                                        {
                                            table1.TableModel.Rows[i].Cells[10].Text = arySystemInfo[2];//Version
                                        }
                                        catch
                                        {
                                            table1.TableModel.Rows[i].Cells[10].Text = "0";
                                        }

                                        //磁盘信息部分
                                        try
                                        {
                                            if (aryDiskInfo[0].ToString() == "C:")
                                            {
                                                table1.TableModel.Rows[i].Cells[5].Data = Convert.ToInt32(aryDiskInfo[1]);//C:
                                                if (Convert.ToInt32(aryDiskInfo[1]) > 90)
                                                {
                                                    table1.TableModel.Rows[i].Cells[5].BackColor = Color.Red;
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            table1.TableModel.Rows[i].Cells[5].Data = 0;
                                        }

                                        try
                                        {
                                            if (aryDiskInfo[2].ToString() == "D:")
                                            {
                                                table1.TableModel.Rows[i].Cells[6].Data = Convert.ToInt32(aryDiskInfo[3]);//D:
                                                if (Convert.ToInt32(aryDiskInfo[3]) > 90)
                                                {
                                                    table1.TableModel.Rows[i].Cells[6].BackColor = Color.Red;
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            table1.TableModel.Rows[i].Cells[6].Data = 0;
                                        }

                                        try
                                        {
                                            if (aryDiskInfo[4].ToString() == "E:")
                                            {
                                                table1.TableModel.Rows[i].Cells[7].Data = Convert.ToInt32(aryDiskInfo[5]);//E:
                                                if (Convert.ToInt32(aryDiskInfo[5]) > 90)
                                                {
                                                    table1.TableModel.Rows[i].Cells[7].BackColor = Color.Red;
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            table1.TableModel.Rows[i].Cells[7].Data = 0;
                                        }

                                        try
                                        {
                                            if (aryDiskInfo[6].ToString() == "F:")
                                            {
                                                table1.TableModel.Rows[i].Cells[8].Data = Convert.ToInt32(aryDiskInfo[7]);//F:
                                                if (Convert.ToInt32(aryDiskInfo[7]) > 90)
                                                {
                                                    table1.TableModel.Rows[i].Cells[8].BackColor = Color.Red;
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            table1.TableModel.Rows[i].Cells[8].Data = 0;
                                        }

                                        try
                                        {
                                            if (aryDiskInfo[8].ToString() == "G:")
                                            {
                                                table1.TableModel.Rows[i].Cells[9].Data = Convert.ToInt32(aryDiskInfo[9]);//G:
                                                if (Convert.ToInt32(aryDiskInfo[9]) > 90)
                                                {
                                                    table1.TableModel.Rows[i].Cells[9].BackColor = Color.Red;
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            table1.TableModel.Rows[i].Cells[9].Data = 0;
                                        }
                                    }
                                    return;
                                }
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            Log.WriteLog(LogFile.Error, ex.ToString());
                        }
                        break;
                }
            }
        }
        #endregion 更新系统信息 UpdateInfo(string SendUser, string strContent)

        #region 委托
        /// <summary>
        /// 登录委托调用
        /// </summary>
        private void Login()
        {
            objXmpp = new XMPPClass(this.strUserName, this.strPassword, this.strServer);
            objXmpp.OnlineStatus += new XMPPClass.EventHandler(objXmpp_OnlineStatus);
            objXmpp.UserStatus += new XMPPClass.OnlineHandler(objXmpp_ServerStatus);
            objXmpp.FriendList += new XMPPClass.FriendHandler(objXmpp_FriendList);
            objXmpp.Messages += new XMPPClass.MessageHandler(objXmpp_Messages);
        }
        #endregion 委托

        #region 菜单
        private void 更新ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string strServerName;
            foreach (Row row in table1.SelectedItems)
            {
                strServerName = row.Cells[1].Text;
                objXmpp.SendCommand(strServerName, "Update");
            }
        }

        private void 更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strServerName;

            foreach (Row row in table1.SelectedItems)
            {
                strServerName = row.Cells[1].Text;
                objXmpp.SendCommand(strServerName, "Update");
            }
        }

        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            th_Main = new Thread(new ThreadStart(Login));
            th_Main.Start();
            //toolStripStatusLabel1.Text = "正在登录...";
            登录ToolStripMenuItem.Enabled = false;
            //objXmpp = new XMPPClass(this.strUserName, this.strPassword, this.strServer);
            //objXmpp.OnlineStatus += new XMPPClass.EventHandler(objXmpp_OnlineStatus);
            //objXmpp.UserStatus += new XMPPClass.OnlineHandler(objXmpp_ServerStatus);
            //objXmpp.FriendList += new XMPPClass.FriendHandler(objXmpp_FriendList);
            //objXmpp.Messages += new XMPPClass.MessageHandler(objXmpp_Messages);
        }

        private void 更改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ChangeUserPassword frm_ChangePwd = new Form_ChangeUserPassword();
            frm_ChangePwd.ShowDialog();

            string strServerName;

            if (frm_ChangePwd.DialogResult == DialogResult.OK)
            {                
                if (frm_ChangePwd.strUserName != "" && frm_ChangePwd.strPassword != "")
                {
                    foreach (Row row in table1.SelectedItems)
                    {
                        strServerName = row.Cells[1].Text;
                        objXmpp.SendCommand(strServerName, "ChangeUserPassword|" + strUserName + "|" + strPassword);
                    }
                }
                else
                {
                    MessageBox.Show("请确认选项是否填写完全！");
                }
            }
            frm_ChangePwd.Close();
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_DebugSendCommand frm_DebugSC = new Form_DebugSendCommand();
            frm_DebugSC.ShowDialog();

            string strServerName;

            if (frm_DebugSC.DialogResult == DialogResult.OK)
            {
                if (frm_DebugSC.strCommand != "")
                {
                    foreach (Row row in table1.SelectedItems)
                    {
                        strServerName = row.Cells[1].Text;
                        objXmpp.SendCommand(strServerName, frm_DebugSC.strCommand);
                    }
                }
            }
            frm_DebugSC.Close();
        }

        private void 开启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strServerName;

            foreach (Row row in table1.SelectedItems)
            {
                strServerName = row.Cells[1].Text;
                objXmpp.SendCommand(strServerName, "ChangeINI|ClientConfig|Debug|1");
            }
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strServerName;

            foreach (Row row in table1.SelectedItems)
            {
                strServerName = row.Cells[1].Text;
                objXmpp.SendCommand(strServerName, "ChangeINI|ClientConfig|Debug|0");
            }
        }

        //功能BUG尚未处理
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Row row in table1.SelectedItems)
            {
                objXmpp.RemoveServer(row.Cells[1].Text);
                tableModel1.Rows.RemoveAt(row.Index);
            }
        }

        private void 文件下载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strServerName;

            Form_Download frm_Download = new Form_Download();
            frm_Download.ShowDialog();

            if (frm_Download.DialogResult == DialogResult.OK)
            {
                if (frm_Download.strUrl != "" && frm_Download.strPath != "")
                {
                    foreach (Row row in table1.SelectedItems)
                    {
                        strServerName = row.Cells[1].Text;
                        //DownloadFile|Url|Path+FileName|UnRAR
                        objXmpp.SendCommand(strServerName, "DownloadFile|" + frm_Download.strUrl + "|" + frm_Download.strPath + "|" + frm_Download.strUnRAR);
                    }
                }
            }
            frm_Download.Close();
        }

        private void table1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point point = this.PointToClient(table1.PointToScreen(new Point(e.X, e.Y)));
                this.contextMenuStrip1.Show(this, point);
            }
        }
        #endregion 菜单        
    }
}
