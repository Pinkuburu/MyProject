namespace QDBBS_SendMessage
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.button2 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.text_SendRecord = new System.Windows.Forms.TextBox();
            this.listView3 = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_DelUser = new System.Windows.Forms.Button();
            this.btn_AddUser = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listView4 = new System.Windows.Forms.ListView();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listView5 = new System.Windows.Forms.ListView();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.btn_AddClub = new System.Windows.Forms.Button();
            this.btn_AllSelect = new System.Windows.Forms.Button();
            this.btn_DelClub = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_Send = new System.Windows.Forms.Button();
            this.text_SendUser = new System.Windows.Forms.TextBox();
            this.text_SendTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btn_DelContent = new System.Windows.Forms.Button();
            this.btn_AddContent = new System.Windows.Forms.Button();
            this.listView6 = new System.Windows.Forms.ListView();
            this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 673);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(897, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(664, 488);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader1});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 447);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(336, 174);
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "栏目名称";
            this.columnHeader2.Width = 96;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "栏目链接";
            this.columnHeader1.Width = 230;
            // 
            // listView2
            // 
            this.listView2.BackColor = System.Drawing.SystemColors.Window;
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader3});
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.Location = new System.Drawing.Point(359, 447);
            this.listView2.Name = "listView2";
            this.listView2.ShowItemToolTips = true;
            this.listView2.Size = new System.Drawing.Size(299, 200);
            this.listView2.TabIndex = 8;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "编号";
            this.columnHeader4.Width = 36;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "帖子列表";
            this.columnHeader3.Width = 220;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "发贴间隔（秒）：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(664, 447);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.text_SendRecord);
            this.groupBox1.Location = new System.Drawing.Point(12, 218);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(476, 164);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "发贴记录";
            // 
            // text_SendRecord
            // 
            this.text_SendRecord.Location = new System.Drawing.Point(6, 20);
            this.text_SendRecord.Multiline = true;
            this.text_SendRecord.Name = "text_SendRecord";
            this.text_SendRecord.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.text_SendRecord.Size = new System.Drawing.Size(463, 138);
            this.text_SendRecord.TabIndex = 0;
            // 
            // listView3
            // 
            this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
            this.listView3.FullRowSelect = true;
            this.listView3.GridLines = true;
            this.listView3.Location = new System.Drawing.Point(6, 20);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(144, 148);
            this.listView3.TabIndex = 1;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "账号";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "密码";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_DelUser);
            this.groupBox2.Controls.Add(this.btn_AddUser);
            this.groupBox2.Controls.Add(this.listView3);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(157, 200);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "发贴用户";
            // 
            // btn_DelUser
            // 
            this.btn_DelUser.Location = new System.Drawing.Point(81, 171);
            this.btn_DelUser.Name = "btn_DelUser";
            this.btn_DelUser.Size = new System.Drawing.Size(60, 23);
            this.btn_DelUser.TabIndex = 3;
            this.btn_DelUser.Text = "删除";
            this.btn_DelUser.UseVisualStyleBackColor = true;
            this.btn_DelUser.Click += new System.EventHandler(this.btn_DelUser_Click);
            // 
            // btn_AddUser
            // 
            this.btn_AddUser.Location = new System.Drawing.Point(15, 171);
            this.btn_AddUser.Name = "btn_AddUser";
            this.btn_AddUser.Size = new System.Drawing.Size(60, 23);
            this.btn_AddUser.TabIndex = 2;
            this.btn_AddUser.Text = "添加";
            this.btn_AddUser.UseVisualStyleBackColor = true;
            this.btn_AddUser.Click += new System.EventHandler(this.btn_AddUser_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listView4);
            this.groupBox3.Location = new System.Drawing.Point(175, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(313, 200);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "所有版块";
            // 
            // listView4
            // 
            this.listView4.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader9});
            this.listView4.FullRowSelect = true;
            this.listView4.GridLines = true;
            this.listView4.Location = new System.Drawing.Point(7, 21);
            this.listView4.Name = "listView4";
            this.listView4.Size = new System.Drawing.Size(299, 173);
            this.listView4.TabIndex = 0;
            this.listView4.UseCompatibleStateImageBehavior = false;
            this.listView4.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "栏目名称";
            this.columnHeader7.Width = 70;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "栏目链接";
            this.columnHeader9.Width = 200;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listView5);
            this.groupBox4.Location = new System.Drawing.Point(575, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(311, 200);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "发贴版块";
            // 
            // listView5
            // 
            this.listView5.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader10});
            this.listView5.FullRowSelect = true;
            this.listView5.GridLines = true;
            this.listView5.Location = new System.Drawing.Point(8, 21);
            this.listView5.Name = "listView5";
            this.listView5.Size = new System.Drawing.Size(296, 173);
            this.listView5.TabIndex = 0;
            this.listView5.UseCompatibleStateImageBehavior = false;
            this.listView5.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "栏目名称";
            this.columnHeader8.Width = 70;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "栏目链接";
            this.columnHeader10.Width = 200;
            // 
            // btn_AddClub
            // 
            this.btn_AddClub.Location = new System.Drawing.Point(494, 73);
            this.btn_AddClub.Name = "btn_AddClub";
            this.btn_AddClub.Size = new System.Drawing.Size(75, 23);
            this.btn_AddClub.TabIndex = 15;
            this.btn_AddClub.Text = "添加-->";
            this.btn_AddClub.UseVisualStyleBackColor = true;
            this.btn_AddClub.Click += new System.EventHandler(this.btn_AddClub_Click);
            // 
            // btn_AllSelect
            // 
            this.btn_AllSelect.Location = new System.Drawing.Point(494, 102);
            this.btn_AllSelect.Name = "btn_AllSelect";
            this.btn_AllSelect.Size = new System.Drawing.Size(75, 23);
            this.btn_AllSelect.TabIndex = 16;
            this.btn_AllSelect.Text = "全选";
            this.btn_AllSelect.UseVisualStyleBackColor = true;
            this.btn_AllSelect.Click += new System.EventHandler(this.btn_AllSelect_Click);
            // 
            // btn_DelClub
            // 
            this.btn_DelClub.Location = new System.Drawing.Point(494, 131);
            this.btn_DelClub.Name = "btn_DelClub";
            this.btn_DelClub.Size = new System.Drawing.Size(75, 23);
            this.btn_DelClub.TabIndex = 17;
            this.btn_DelClub.Text = "<--移除";
            this.btn_DelClub.UseVisualStyleBackColor = true;
            this.btn_DelClub.Click += new System.EventHandler(this.btn_DelClub_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btn_Send);
            this.groupBox5.Controls.Add(this.text_SendUser);
            this.groupBox5.Controls.Add(this.text_SendTime);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Location = new System.Drawing.Point(410, 387);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(476, 54);
            this.groupBox5.TabIndex = 18;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "参数设置";
            // 
            // btn_Send
            // 
            this.btn_Send.Location = new System.Drawing.Point(394, 21);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(75, 23);
            this.btn_Send.TabIndex = 13;
            this.btn_Send.Text = "开始顶贴";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // text_SendUser
            // 
            this.text_SendUser.Location = new System.Drawing.Point(286, 23);
            this.text_SendUser.Name = "text_SendUser";
            this.text_SendUser.Size = new System.Drawing.Size(79, 21);
            this.text_SendUser.TabIndex = 12;
            this.text_SendUser.Text = "0";
            // 
            // text_SendTime
            // 
            this.text_SendTime.Location = new System.Drawing.Point(113, 23);
            this.text_SendTime.Name = "text_SendTime";
            this.text_SendTime.Size = new System.Drawing.Size(78, 21);
            this.text_SendTime.TabIndex = 11;
            this.text_SendTime.Text = "10";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(197, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "切换用户/贴：";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btn_DelContent);
            this.groupBox6.Controls.Add(this.btn_AddContent);
            this.groupBox6.Controls.Add(this.listView6);
            this.groupBox6.Location = new System.Drawing.Point(494, 218);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(392, 164);
            this.groupBox6.TabIndex = 19;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "发贴内容";
            // 
            // btn_DelContent
            // 
            this.btn_DelContent.Location = new System.Drawing.Point(310, 135);
            this.btn_DelContent.Name = "btn_DelContent";
            this.btn_DelContent.Size = new System.Drawing.Size(60, 23);
            this.btn_DelContent.TabIndex = 3;
            this.btn_DelContent.Text = "删除";
            this.btn_DelContent.UseVisualStyleBackColor = true;
            this.btn_DelContent.Click += new System.EventHandler(this.btn_DelContent_Click);
            // 
            // btn_AddContent
            // 
            this.btn_AddContent.Location = new System.Drawing.Point(244, 135);
            this.btn_AddContent.Name = "btn_AddContent";
            this.btn_AddContent.Size = new System.Drawing.Size(60, 23);
            this.btn_AddContent.TabIndex = 1;
            this.btn_AddContent.Text = "添加";
            this.btn_AddContent.UseVisualStyleBackColor = true;
            this.btn_AddContent.Click += new System.EventHandler(this.btn_AddContent_Click);
            // 
            // listView6
            // 
            this.listView6.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11});
            this.listView6.FullRowSelect = true;
            this.listView6.GridLines = true;
            this.listView6.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView6.Location = new System.Drawing.Point(7, 21);
            this.listView6.Name = "listView6";
            this.listView6.Size = new System.Drawing.Size(379, 108);
            this.listView6.TabIndex = 0;
            this.listView6.UseCompatibleStateImageBehavior = false;
            this.listView6.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "内容";
            this.columnHeader11.Width = 350;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.textBox4);
            this.groupBox7.Location = new System.Drawing.Point(12, 388);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(392, 53);
            this.groupBox7.TabIndex = 20;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "固定顶贴";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(6, 20);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(380, 21);
            this.textBox4.TabIndex = 0;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 695);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btn_DelClub);
            this.Controls.Add(this.btn_AllSelect);
            this.Controls.Add(this.btn_AddClub);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.Text = "Form2";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form2_FormClosed);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button btn_DelUser;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_AddClub;
        private System.Windows.Forms.Button btn_AllSelect;
        private System.Windows.Forms.Button btn_DelClub;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        public System.Windows.Forms.ListView listView3;
        public System.Windows.Forms.Button btn_AddUser;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox text_SendUser;
        private System.Windows.Forms.TextBox text_SendTime;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox textBox4;
        public System.Windows.Forms.ListView listView6;
        private System.Windows.Forms.Button btn_DelContent;
        private System.Windows.Forms.Button btn_AddContent;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        public System.Windows.Forms.ListView listView4;
        public System.Windows.Forms.ListView listView5;
        public System.Windows.Forms.TextBox text_SendRecord;
    }
}