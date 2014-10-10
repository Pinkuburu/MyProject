namespace BDBBS_SendMessage
{
    partial class BD_Main_Form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BD_Main_Form));
            this.groupBox_MainForm = new System.Windows.Forms.GroupBox();
            this.label_MainForm_UserInfo = new System.Windows.Forms.Label();
            this.menuStrip_MainForm = new System.Windows.Forms.MenuStrip();
            this.MenuItem_MainForm_Login = new System.Windows.Forms.ToolStripMenuItem();
            this.测试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox_MainForm_BoardList = new System.Windows.Forms.GroupBox();
            this.listView_MainForm_BoardList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.btn_MainForm_AddBoard = new System.Windows.Forms.Button();
            this.btn_MainForm_AllSelectBoard = new System.Windows.Forms.Button();
            this.btn_MainForm_RemoveBoard = new System.Windows.Forms.Button();
            this.groupBox_MainForm_SendBoardList = new System.Windows.Forms.GroupBox();
            this.listView_MainForm_SendBoardList = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.btn_MainForm_AllSelectSendBoard = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_MainForm = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox_MainForm_SendLog = new System.Windows.Forms.GroupBox();
            this.textBox_MainForm_SysLog = new System.Windows.Forms.TextBox();
            this.groupBox_MainForm_SendList = new System.Windows.Forms.GroupBox();
            this.textBox_MainForm_Content = new System.Windows.Forms.TextBox();
            this.btn_MainForm_ShowList = new System.Windows.Forms.Button();
            this.groupBox_MainForm.SuspendLayout();
            this.menuStrip_MainForm.SuspendLayout();
            this.groupBox_MainForm_BoardList.SuspendLayout();
            this.groupBox_MainForm_SendBoardList.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox_MainForm_SendLog.SuspendLayout();
            this.groupBox_MainForm_SendList.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_MainForm
            // 
            this.groupBox_MainForm.Controls.Add(this.label_MainForm_UserInfo);
            this.groupBox_MainForm.Location = new System.Drawing.Point(12, 28);
            this.groupBox_MainForm.Name = "groupBox_MainForm";
            this.groupBox_MainForm.Size = new System.Drawing.Size(174, 104);
            this.groupBox_MainForm.TabIndex = 0;
            this.groupBox_MainForm.TabStop = false;
            this.groupBox_MainForm.Text = "用户信息";
            // 
            // label_MainForm_UserInfo
            // 
            this.label_MainForm_UserInfo.AutoSize = true;
            this.label_MainForm_UserInfo.Location = new System.Drawing.Point(7, 21);
            this.label_MainForm_UserInfo.Name = "label_MainForm_UserInfo";
            this.label_MainForm_UserInfo.Size = new System.Drawing.Size(53, 12);
            this.label_MainForm_UserInfo.TabIndex = 0;
            this.label_MainForm_UserInfo.Text = "UserInfo";
            // 
            // menuStrip_MainForm
            // 
            this.menuStrip_MainForm.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip_MainForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_MainForm_Login,
            this.测试ToolStripMenuItem});
            this.menuStrip_MainForm.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_MainForm.Name = "menuStrip_MainForm";
            this.menuStrip_MainForm.Size = new System.Drawing.Size(779, 24);
            this.menuStrip_MainForm.TabIndex = 1;
            this.menuStrip_MainForm.Text = "menuStrip_MainForm";
            // 
            // MenuItem_MainForm_Login
            // 
            this.MenuItem_MainForm_Login.Name = "MenuItem_MainForm_Login";
            this.MenuItem_MainForm_Login.Size = new System.Drawing.Size(41, 20);
            this.MenuItem_MainForm_Login.Text = "登陆";
            this.MenuItem_MainForm_Login.Click += new System.EventHandler(this.MenuItem_MainForm_Login_Click);
            // 
            // 测试ToolStripMenuItem
            // 
            this.测试ToolStripMenuItem.Name = "测试ToolStripMenuItem";
            this.测试ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.测试ToolStripMenuItem.Text = "测试";
            // 
            // groupBox_MainForm_BoardList
            // 
            this.groupBox_MainForm_BoardList.Controls.Add(this.listView_MainForm_BoardList);
            this.groupBox_MainForm_BoardList.Location = new System.Drawing.Point(192, 28);
            this.groupBox_MainForm_BoardList.Name = "groupBox_MainForm_BoardList";
            this.groupBox_MainForm_BoardList.Size = new System.Drawing.Size(245, 205);
            this.groupBox_MainForm_BoardList.TabIndex = 2;
            this.groupBox_MainForm_BoardList.TabStop = false;
            this.groupBox_MainForm_BoardList.Text = "论坛版块";
            // 
            // listView_MainForm_BoardList
            // 
            this.listView_MainForm_BoardList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView_MainForm_BoardList.FullRowSelect = true;
            this.listView_MainForm_BoardList.GridLines = true;
            this.listView_MainForm_BoardList.Location = new System.Drawing.Point(6, 20);
            this.listView_MainForm_BoardList.Name = "listView_MainForm_BoardList";
            this.listView_MainForm_BoardList.Size = new System.Drawing.Size(232, 179);
            this.listView_MainForm_BoardList.TabIndex = 0;
            this.listView_MainForm_BoardList.UseCompatibleStateImageBehavior = false;
            this.listView_MainForm_BoardList.View = System.Windows.Forms.View.Details;
            this.listView_MainForm_BoardList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MainForm_BoardList_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "版块名称";
            this.columnHeader1.Width = 70;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "版块链接";
            this.columnHeader2.Width = 140;
            // 
            // btn_MainForm_AddBoard
            // 
            this.btn_MainForm_AddBoard.Location = new System.Drawing.Point(443, 75);
            this.btn_MainForm_AddBoard.Name = "btn_MainForm_AddBoard";
            this.btn_MainForm_AddBoard.Size = new System.Drawing.Size(75, 23);
            this.btn_MainForm_AddBoard.TabIndex = 3;
            this.btn_MainForm_AddBoard.Text = "添加-->";
            this.btn_MainForm_AddBoard.UseVisualStyleBackColor = true;
            this.btn_MainForm_AddBoard.Click += new System.EventHandler(this.btn_MainForm_AddBoard_Click);
            // 
            // btn_MainForm_AllSelectBoard
            // 
            this.btn_MainForm_AllSelectBoard.Location = new System.Drawing.Point(443, 105);
            this.btn_MainForm_AllSelectBoard.Name = "btn_MainForm_AllSelectBoard";
            this.btn_MainForm_AllSelectBoard.Size = new System.Drawing.Size(75, 23);
            this.btn_MainForm_AllSelectBoard.TabIndex = 4;
            this.btn_MainForm_AllSelectBoard.Text = "<--全选";
            this.btn_MainForm_AllSelectBoard.UseVisualStyleBackColor = true;
            this.btn_MainForm_AllSelectBoard.Click += new System.EventHandler(this.btn_MainForm_AllSelectBoard_Click);
            // 
            // btn_MainForm_RemoveBoard
            // 
            this.btn_MainForm_RemoveBoard.Location = new System.Drawing.Point(443, 163);
            this.btn_MainForm_RemoveBoard.Name = "btn_MainForm_RemoveBoard";
            this.btn_MainForm_RemoveBoard.Size = new System.Drawing.Size(75, 23);
            this.btn_MainForm_RemoveBoard.TabIndex = 5;
            this.btn_MainForm_RemoveBoard.Text = "<--移除";
            this.btn_MainForm_RemoveBoard.UseVisualStyleBackColor = true;
            this.btn_MainForm_RemoveBoard.Click += new System.EventHandler(this.btn_MainForm_RemoveBoard_Click);
            // 
            // groupBox_MainForm_SendBoardList
            // 
            this.groupBox_MainForm_SendBoardList.Controls.Add(this.listView_MainForm_SendBoardList);
            this.groupBox_MainForm_SendBoardList.Location = new System.Drawing.Point(524, 27);
            this.groupBox_MainForm_SendBoardList.Name = "groupBox_MainForm_SendBoardList";
            this.groupBox_MainForm_SendBoardList.Size = new System.Drawing.Size(243, 205);
            this.groupBox_MainForm_SendBoardList.TabIndex = 6;
            this.groupBox_MainForm_SendBoardList.TabStop = false;
            this.groupBox_MainForm_SendBoardList.Text = "发贴版块";
            // 
            // listView_MainForm_SendBoardList
            // 
            this.listView_MainForm_SendBoardList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.listView_MainForm_SendBoardList.FullRowSelect = true;
            this.listView_MainForm_SendBoardList.GridLines = true;
            this.listView_MainForm_SendBoardList.Location = new System.Drawing.Point(6, 20);
            this.listView_MainForm_SendBoardList.Name = "listView_MainForm_SendBoardList";
            this.listView_MainForm_SendBoardList.Size = new System.Drawing.Size(231, 179);
            this.listView_MainForm_SendBoardList.TabIndex = 0;
            this.listView_MainForm_SendBoardList.UseCompatibleStateImageBehavior = false;
            this.listView_MainForm_SendBoardList.View = System.Windows.Forms.View.Details;
            this.listView_MainForm_SendBoardList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MainForm_SendBoardList_MouseDoubleClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "版块名称";
            this.columnHeader3.Width = 70;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "版块链接";
            this.columnHeader4.Width = 140;
            // 
            // btn_MainForm_AllSelectSendBoard
            // 
            this.btn_MainForm_AllSelectSendBoard.Location = new System.Drawing.Point(443, 134);
            this.btn_MainForm_AllSelectSendBoard.Name = "btn_MainForm_AllSelectSendBoard";
            this.btn_MainForm_AllSelectSendBoard.Size = new System.Drawing.Size(75, 23);
            this.btn_MainForm_AllSelectSendBoard.TabIndex = 7;
            this.btn_MainForm_AllSelectSendBoard.Text = "全选-->";
            this.btn_MainForm_AllSelectSendBoard.UseVisualStyleBackColor = true;
            this.btn_MainForm_AllSelectSendBoard.Click += new System.EventHandler(this.btn_MainForm_AllSelectSendBoard_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_MainForm});
            this.statusStrip1.Location = new System.Drawing.Point(0, 508);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(779, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_MainForm
            // 
            this.toolStripStatusLabel_MainForm.Name = "toolStripStatusLabel_MainForm";
            this.toolStripStatusLabel_MainForm.Size = new System.Drawing.Size(0, 17);
            // 
            // groupBox_MainForm_SendLog
            // 
            this.groupBox_MainForm_SendLog.Controls.Add(this.textBox_MainForm_SysLog);
            this.groupBox_MainForm_SendLog.Location = new System.Drawing.Point(12, 240);
            this.groupBox_MainForm_SendLog.Name = "groupBox_MainForm_SendLog";
            this.groupBox_MainForm_SendLog.Size = new System.Drawing.Size(342, 265);
            this.groupBox_MainForm_SendLog.TabIndex = 9;
            this.groupBox_MainForm_SendLog.TabStop = false;
            this.groupBox_MainForm_SendLog.Text = "发送记录";
            // 
            // textBox_MainForm_SysLog
            // 
            this.textBox_MainForm_SysLog.Location = new System.Drawing.Point(6, 20);
            this.textBox_MainForm_SysLog.Multiline = true;
            this.textBox_MainForm_SysLog.Name = "textBox_MainForm_SysLog";
            this.textBox_MainForm_SysLog.ReadOnly = true;
            this.textBox_MainForm_SysLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_MainForm_SysLog.Size = new System.Drawing.Size(330, 239);
            this.textBox_MainForm_SysLog.TabIndex = 0;
            // 
            // groupBox_MainForm_SendList
            // 
            this.groupBox_MainForm_SendList.Controls.Add(this.textBox_MainForm_Content);
            this.groupBox_MainForm_SendList.Location = new System.Drawing.Point(361, 240);
            this.groupBox_MainForm_SendList.Name = "groupBox_MainForm_SendList";
            this.groupBox_MainForm_SendList.Size = new System.Drawing.Size(406, 265);
            this.groupBox_MainForm_SendList.TabIndex = 10;
            this.groupBox_MainForm_SendList.TabStop = false;
            this.groupBox_MainForm_SendList.Text = "发送内容";
            // 
            // textBox_MainForm_Content
            // 
            this.textBox_MainForm_Content.AcceptsReturn = true;
            this.textBox_MainForm_Content.Location = new System.Drawing.Point(7, 21);
            this.textBox_MainForm_Content.Multiline = true;
            this.textBox_MainForm_Content.Name = "textBox_MainForm_Content";
            this.textBox_MainForm_Content.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_MainForm_Content.Size = new System.Drawing.Size(393, 238);
            this.textBox_MainForm_Content.TabIndex = 0;
            // 
            // btn_MainForm_ShowList
            // 
            this.btn_MainForm_ShowList.Location = new System.Drawing.Point(21, 138);
            this.btn_MainForm_ShowList.Name = "btn_MainForm_ShowList";
            this.btn_MainForm_ShowList.Size = new System.Drawing.Size(75, 23);
            this.btn_MainForm_ShowList.TabIndex = 11;
            this.btn_MainForm_ShowList.Text = "开始发贴";
            this.btn_MainForm_ShowList.UseVisualStyleBackColor = true;
            this.btn_MainForm_ShowList.Click += new System.EventHandler(this.btn_MainForm_ShowList_Click);
            // 
            // BD_Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 530);
            this.Controls.Add(this.btn_MainForm_ShowList);
            this.Controls.Add(this.groupBox_MainForm_SendList);
            this.Controls.Add(this.groupBox_MainForm_SendLog);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btn_MainForm_AllSelectSendBoard);
            this.Controls.Add(this.groupBox_MainForm_SendBoardList);
            this.Controls.Add(this.btn_MainForm_RemoveBoard);
            this.Controls.Add(this.btn_MainForm_AllSelectBoard);
            this.Controls.Add(this.btn_MainForm_AddBoard);
            this.Controls.Add(this.groupBox_MainForm_BoardList);
            this.Controls.Add(this.groupBox_MainForm);
            this.Controls.Add(this.menuStrip_MainForm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip_MainForm;
            this.Name = "BD_Main_Form";
            this.Text = "半岛社区顶贴机";
            this.groupBox_MainForm.ResumeLayout(false);
            this.groupBox_MainForm.PerformLayout();
            this.menuStrip_MainForm.ResumeLayout(false);
            this.menuStrip_MainForm.PerformLayout();
            this.groupBox_MainForm_BoardList.ResumeLayout(false);
            this.groupBox_MainForm_SendBoardList.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox_MainForm_SendLog.ResumeLayout(false);
            this.groupBox_MainForm_SendLog.PerformLayout();
            this.groupBox_MainForm_SendList.ResumeLayout(false);
            this.groupBox_MainForm_SendList.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_MainForm;
        private System.Windows.Forms.MenuStrip menuStrip_MainForm;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_MainForm_Login;
        private System.Windows.Forms.ToolStripMenuItem 测试ToolStripMenuItem;
        public System.Windows.Forms.Label label_MainForm_UserInfo;
        private System.Windows.Forms.GroupBox groupBox_MainForm_BoardList;
        private System.Windows.Forms.ListView listView_MainForm_BoardList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btn_MainForm_AddBoard;
        private System.Windows.Forms.Button btn_MainForm_AllSelectBoard;
        private System.Windows.Forms.Button btn_MainForm_RemoveBoard;
        private System.Windows.Forms.GroupBox groupBox_MainForm_SendBoardList;
        private System.Windows.Forms.ListView listView_MainForm_SendBoardList;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btn_MainForm_AllSelectSendBoard;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_MainForm;
        private System.Windows.Forms.GroupBox groupBox_MainForm_SendLog;
        private System.Windows.Forms.GroupBox groupBox_MainForm_SendList;
        private System.Windows.Forms.TextBox textBox_MainForm_Content;
        private System.Windows.Forms.Button btn_MainForm_ShowList;
        private System.Windows.Forms.TextBox textBox_MainForm_SysLog;
    }
}