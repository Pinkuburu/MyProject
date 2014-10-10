namespace Baidu_Zhidao
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBox_Log = new System.Windows.Forms.TextBox();
            this.button_Run = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1_AddURL = new System.Windows.Forms.TextBox();
            this.listView_URL = new System.Windows.Forms.ListView();
            this.button_AddURL = new System.Windows.Forms.Button();
            this.button_DelURL = new System.Windows.Forms.Button();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_ADSLuser = new System.Windows.Forms.TextBox();
            this.textBox_ADSLpwd = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(464, 238);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.button_Run);
            this.tabPage1.Controls.Add(this.textBox_Log);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(456, 212);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "运行状态";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.button_DelURL);
            this.tabPage2.Controls.Add(this.button_AddURL);
            this.tabPage2.Controls.Add(this.listView_URL);
            this.tabPage2.Controls.Add(this.textBox1_AddURL);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(456, 212);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "参数设置";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.textBox_ADSLpwd);
            this.tabPage3.Controls.Add(this.textBox_ADSLuser);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(456, 212);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "拔号设置";
            // 
            // textBox_Log
            // 
            this.textBox_Log.Location = new System.Drawing.Point(6, 6);
            this.textBox_Log.Multiline = true;
            this.textBox_Log.Name = "textBox_Log";
            this.textBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Log.Size = new System.Drawing.Size(444, 173);
            this.textBox_Log.TabIndex = 0;
            // 
            // button_Run
            // 
            this.button_Run.Location = new System.Drawing.Point(357, 183);
            this.button_Run.Name = "button_Run";
            this.button_Run.Size = new System.Drawing.Size(75, 23);
            this.button_Run.TabIndex = 1;
            this.button_Run.Text = "开始";
            this.button_Run.UseVisualStyleBackColor = true;
            this.button_Run.Click += new System.EventHandler(this.button_Run_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "刷分地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "刷分列表：";
            // 
            // textBox1_AddURL
            // 
            this.textBox1_AddURL.Location = new System.Drawing.Point(91, 19);
            this.textBox1_AddURL.Name = "textBox1_AddURL";
            this.textBox1_AddURL.Size = new System.Drawing.Size(212, 21);
            this.textBox1_AddURL.TabIndex = 2;
            // 
            // listView_URL
            // 
            this.listView_URL.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView_URL.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView_URL.Location = new System.Drawing.Point(91, 52);
            this.listView_URL.Name = "listView_URL";
            this.listView_URL.Size = new System.Drawing.Size(359, 150);
            this.listView_URL.TabIndex = 3;
            this.listView_URL.UseCompatibleStateImageBehavior = false;
            this.listView_URL.View = System.Windows.Forms.View.Details;
            // 
            // button_AddURL
            // 
            this.button_AddURL.Location = new System.Drawing.Point(310, 17);
            this.button_AddURL.Name = "button_AddURL";
            this.button_AddURL.Size = new System.Drawing.Size(65, 23);
            this.button_AddURL.TabIndex = 4;
            this.button_AddURL.Text = "添加";
            this.button_AddURL.UseVisualStyleBackColor = true;
            this.button_AddURL.Click += new System.EventHandler(this.button_AddURL_Click);
            // 
            // button_DelURL
            // 
            this.button_DelURL.Location = new System.Drawing.Point(382, 17);
            this.button_DelURL.Name = "button_DelURL";
            this.button_DelURL.Size = new System.Drawing.Size(65, 23);
            this.button_DelURL.TabIndex = 5;
            this.button_DelURL.Text = "移除";
            this.button_DelURL.UseVisualStyleBackColor = true;
            this.button_DelURL.Click += new System.EventHandler(this.button_DelURL_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 300;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "ADSL帐号：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "ADSL密码：";
            // 
            // textBox_ADSLuser
            // 
            this.textBox_ADSLuser.Location = new System.Drawing.Point(93, 14);
            this.textBox_ADSLuser.Name = "textBox_ADSLuser";
            this.textBox_ADSLuser.Size = new System.Drawing.Size(152, 21);
            this.textBox_ADSLuser.TabIndex = 2;
            // 
            // textBox_ADSLpwd
            // 
            this.textBox_ADSLpwd.Location = new System.Drawing.Point(93, 40);
            this.textBox_ADSLpwd.Name = "textBox_ADSLpwd";
            this.textBox_ADSLpwd.Size = new System.Drawing.Size(152, 21);
            this.textBox_ADSLpwd.TabIndex = 3;
            this.textBox_ADSLpwd.UseSystemPasswordChar = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 262);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "百度知道最佳答案评价工具";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textBox_Log;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button_Run;
        private System.Windows.Forms.Button button_DelURL;
        private System.Windows.Forms.Button button_AddURL;
        private System.Windows.Forms.ListView listView_URL;
        private System.Windows.Forms.TextBox textBox1_AddURL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TextBox textBox_ADSLpwd;
        private System.Windows.Forms.TextBox textBox_ADSLuser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}

