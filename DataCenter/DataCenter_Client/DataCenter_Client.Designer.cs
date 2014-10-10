namespace DataCenter_Client
{
    partial class DataCenter_Client
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listView_DiskInfo = new System.Windows.Forms.ListView();
            this.columnServerName = new System.Windows.Forms.ColumnHeader();
            this.columnArea = new System.Windows.Forms.ColumnHeader();
            this.columnIP = new System.Windows.Forms.ColumnHeader();
            this.columnDisk_C = new System.Windows.Forms.ColumnHeader();
            this.columnDisk_D = new System.Windows.Forms.ColumnHeader();
            this.columnDisk_E = new System.Windows.Forms.ColumnHeader();
            this.columnDisk_F = new System.Windows.Forms.ColumnHeader();
            this.columnDisk_G = new System.Windows.Forms.ColumnHeader();
            this.columnCreateTime = new System.Windows.Forms.ColumnHeader();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(866, 371);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listView_DiskInfo);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(858, 346);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "磁盘信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // listView_DiskInfo
            // 
            this.listView_DiskInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnServerName,
            this.columnArea,
            this.columnIP,
            this.columnDisk_C,
            this.columnDisk_D,
            this.columnDisk_E,
            this.columnDisk_F,
            this.columnDisk_G,
            this.columnCreateTime});
            this.listView_DiskInfo.FullRowSelect = true;
            this.listView_DiskInfo.GridLines = true;
            this.listView_DiskInfo.Location = new System.Drawing.Point(-4, 0);
            this.listView_DiskInfo.Name = "listView_DiskInfo";
            this.listView_DiskInfo.Size = new System.Drawing.Size(866, 346);
            this.listView_DiskInfo.TabIndex = 0;
            this.listView_DiskInfo.UseCompatibleStateImageBehavior = false;
            this.listView_DiskInfo.View = System.Windows.Forms.View.Details;
            // 
            // columnServerName
            // 
            this.columnServerName.Text = "服务器名称";
            this.columnServerName.Width = 80;
            // 
            // columnArea
            // 
            this.columnArea.Text = "所在大区";
            this.columnArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnArea.Width = 70;
            // 
            // columnIP
            // 
            this.columnIP.Text = "服务器IP";
            this.columnIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnIP.Width = 105;
            // 
            // columnDisk_C
            // 
            this.columnDisk_C.Text = "C盘";
            this.columnDisk_C.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnDisk_C.Width = 90;
            // 
            // columnDisk_D
            // 
            this.columnDisk_D.Text = "D盘";
            this.columnDisk_D.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnDisk_D.Width = 90;
            // 
            // columnDisk_E
            // 
            this.columnDisk_E.Text = "E盘";
            this.columnDisk_E.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnDisk_E.Width = 90;
            // 
            // columnDisk_F
            // 
            this.columnDisk_F.Text = "F盘";
            this.columnDisk_F.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnDisk_F.Width = 90;
            // 
            // columnDisk_G
            // 
            this.columnDisk_G.Text = "G盘";
            this.columnDisk_G.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnDisk_G.Width = 90;
            // 
            // columnCreateTime
            // 
            this.columnCreateTime.Text = "更新时间";
            this.columnCreateTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnCreateTime.Width = 135;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(858, 346);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(754, 389);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "查看1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(673, 389);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "查看2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 424);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView listView_DiskInfo;
        private System.Windows.Forms.ColumnHeader columnServerName;
        private System.Windows.Forms.ColumnHeader columnArea;
        private System.Windows.Forms.ColumnHeader columnIP;
        private System.Windows.Forms.ColumnHeader columnDisk_C;
        private System.Windows.Forms.ColumnHeader columnDisk_D;
        private System.Windows.Forms.ColumnHeader columnDisk_E;
        private System.Windows.Forms.ColumnHeader columnDisk_F;
        private System.Windows.Forms.ColumnHeader columnDisk_G;
        private System.Windows.Forms.ColumnHeader columnCreateTime;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

