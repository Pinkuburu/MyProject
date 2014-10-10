namespace VPN {
    partial class Form1 {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.gd1 = new System.Windows.Forms.DataGridView();
            this.Available = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Area = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pwd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PingResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Info1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Info2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPing = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.gd2 = new System.Windows.Forms.DataGridView();
            this.cms2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsiConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsi2Drop = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.gd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gd2)).BeginInit();
            this.cms2.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // gd1
            // 
            this.gd1.AllowUserToAddRows = false;
            this.gd1.AllowUserToDeleteRows = false;
            this.gd1.AllowUserToOrderColumns = true;
            this.gd1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gd1.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.gd1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gd1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gd1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Available,
            this.Area,
            this.IP,
            this.User,
            this.Pwd,
            this.PingResult,
            this.Info1,
            this.Info2});
            this.gd1.Location = new System.Drawing.Point(13, 13);
            this.gd1.Name = "gd1";
            this.gd1.RowTemplate.Height = 23;
            this.gd1.Size = new System.Drawing.Size(729, 221);
            this.gd1.TabIndex = 0;
            this.gd1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.gd1_RowPostPaint);
            this.gd1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gd1_CellPainting);
            // 
            // Available
            // 
            this.Available.DataPropertyName = "Available";
            this.Available.Frozen = true;
            this.Available.HeaderText = "";
            this.Available.Name = "Available";
            this.Available.ReadOnly = true;
            this.Available.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Available.Width = 24;
            // 
            // Area
            // 
            this.Area.DataPropertyName = "area";
            this.Area.Frozen = true;
            this.Area.HeaderText = "地区/国家";
            this.Area.Name = "Area";
            this.Area.ReadOnly = true;
            // 
            // IP
            // 
            this.IP.DataPropertyName = "ip";
            this.IP.HeaderText = "IP";
            this.IP.Name = "IP";
            this.IP.ReadOnly = true;
            // 
            // User
            // 
            this.User.DataPropertyName = "user";
            this.User.HeaderText = "用户名";
            this.User.Name = "User";
            this.User.ReadOnly = true;
            // 
            // Pwd
            // 
            this.Pwd.DataPropertyName = "pwd";
            this.Pwd.HeaderText = "密码";
            this.Pwd.Name = "Pwd";
            this.Pwd.ReadOnly = true;
            // 
            // PingResult
            // 
            this.PingResult.DataPropertyName = "Ping";
            this.PingResult.HeaderText = "Ping";
            this.PingResult.Name = "PingResult";
            this.PingResult.ReadOnly = true;
            // 
            // Info1
            // 
            this.Info1.DataPropertyName = "info1";
            this.Info1.HeaderText = "信息1";
            this.Info1.Name = "Info1";
            this.Info1.ReadOnly = true;
            // 
            // Info2
            // 
            this.Info2.DataPropertyName = "info2";
            this.Info2.HeaderText = "信息2";
            this.Info2.Name = "Info2";
            this.Info2.ReadOnly = true;
            // 
            // btnPing
            // 
            this.btnPing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPing.Location = new System.Drawing.Point(14, 240);
            this.btnPing.Name = "btnPing";
            this.btnPing.Size = new System.Drawing.Size(75, 23);
            this.btnPing.TabIndex = 1;
            this.btnPing.Text = "Ping";
            this.btnPing.UseVisualStyleBackColor = true;
            this.btnPing.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFind.Enabled = false;
            this.btnFind.Location = new System.Drawing.Point(96, 240);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(123, 23);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "给我找一个可用的!";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDisconnect.Location = new System.Drawing.Point(225, 240);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(189, 23);
            this.btnDisconnect.TabIndex = 3;
            this.btnDisconnect.Text = "停止查找，并断开现有VPN连接";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // gd2
            // 
            this.gd2.AllowUserToAddRows = false;
            this.gd2.AllowUserToDeleteRows = false;
            this.gd2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gd2.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.gd2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gd2.ContextMenuStrip = this.cms2;
            this.gd2.Location = new System.Drawing.Point(13, 269);
            this.gd2.Name = "gd2";
            this.gd2.ReadOnly = true;
            this.gd2.RowTemplate.Height = 23;
            this.gd2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gd2.Size = new System.Drawing.Size(729, 144);
            this.gd2.TabIndex = 4;
            this.gd2.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.gd2_RowPostPaint);
            this.gd2.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gd2_CellContentDoubleClick);
            // 
            // cms2
            // 
            this.cms2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsiConnect,
            this.cmsi2Drop});
            this.cms2.Name = "cms2";
            this.cms2.Size = new System.Drawing.Size(125, 48);
            // 
            // cmsiConnect
            // 
            this.cmsiConnect.Name = "cmsiConnect";
            this.cmsiConnect.Size = new System.Drawing.Size(152, 22);
            this.cmsiConnect.Text = "连接此VPN";
            this.cmsiConnect.Click += new System.EventHandler(this.cmsiConnect_Click);
            // 
            // cmsi2Drop
            // 
            this.cmsi2Drop.Name = "cmsi2Drop";
            this.cmsi2Drop.Size = new System.Drawing.Size(152, 22);
            this.cmsi2Drop.Text = "删除";
            this.cmsi2Drop.Click += new System.EventHandler(this.cmsi2Drop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 425);
            this.Controls.Add(this.gd2);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.btnPing);
            this.Controls.Add(this.gd1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gd2)).EndInit();
            this.cms2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.DataGridView gd1;
        private System.Windows.Forms.Button btnPing;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.DataGridViewTextBoxColumn Available;
        private System.Windows.Forms.DataGridViewTextBoxColumn Area;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn User;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pwd;
        private System.Windows.Forms.DataGridViewTextBoxColumn PingResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn Info1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Info2;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.DataGridView gd2;
        private System.Windows.Forms.ContextMenuStrip cms2;
        private System.Windows.Forms.ToolStripMenuItem cmsiConnect;
        private System.Windows.Forms.ToolStripMenuItem cmsi2Drop;
    }
}

