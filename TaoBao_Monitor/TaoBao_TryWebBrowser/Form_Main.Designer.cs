namespace TaoBao_TryWebBrowser
{
    partial class Form_Main
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.webBrowser_Main = new System.Windows.Forms.WebBrowser();
            this.button_OpenLink = new System.Windows.Forms.Button();
            this.textBox_Link = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(68, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 46);
            this.button1.TabIndex = 0;
            this.button1.Text = "试用首页";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(187, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(151, 46);
            this.button2.TabIndex = 1;
            this.button2.Text = "获取答案并自动申请";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(912, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(66, 46);
            this.button3.TabIndex = 2;
            this.button3.Text = "返回";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // webBrowser_Main
            // 
            this.webBrowser_Main.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.webBrowser_Main.Location = new System.Drawing.Point(0, 91);
            this.webBrowser_Main.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser_Main.Name = "webBrowser_Main";
            this.webBrowser_Main.Size = new System.Drawing.Size(817, 610);
            this.webBrowser_Main.TabIndex = 3;
            // 
            // button_OpenLink
            // 
            this.button_OpenLink.Location = new System.Drawing.Point(679, 64);
            this.button_OpenLink.Name = "button_OpenLink";
            this.button_OpenLink.Size = new System.Drawing.Size(91, 21);
            this.button_OpenLink.TabIndex = 4;
            this.button_OpenLink.Text = "打开链接";
            this.button_OpenLink.UseVisualStyleBackColor = true;
            this.button_OpenLink.Click += new System.EventHandler(this.button_OpenLink_Click);
            // 
            // textBox_Link
            // 
            this.textBox_Link.Location = new System.Drawing.Point(12, 65);
            this.textBox_Link.Name = "textBox_Link";
            this.textBox_Link.Size = new System.Drawing.Size(626, 21);
            this.textBox_Link.TabIndex = 5;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(357, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(112, 46);
            this.button4.TabIndex = 6;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(835, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(190, 700);
            this.listBox1.TabIndex = 7;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 701);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox_Link);
            this.Controls.Add(this.button_OpenLink);
            this.Controls.Add(this.webBrowser_Main);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form_Main";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.WebBrowser webBrowser_Main;
        private System.Windows.Forms.Button button_OpenLink;
        private System.Windows.Forms.TextBox textBox_Link;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ListBox listBox1;


    }
}

