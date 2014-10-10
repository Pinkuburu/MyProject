namespace TaoBao_TryHelper
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
            this.webBrowser_Main = new System.Windows.Forms.WebBrowser();
            this.btn_OpenLink = new System.Windows.Forms.Button();
            this.textBox_Url = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webBrowser_Main
            // 
            this.webBrowser_Main.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.webBrowser_Main.Location = new System.Drawing.Point(0, 91);
            this.webBrowser_Main.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser_Main.Name = "webBrowser_Main";
            this.webBrowser_Main.Size = new System.Drawing.Size(865, 610);
            this.webBrowser_Main.TabIndex = 0;
            // 
            // btn_OpenLink
            // 
            this.btn_OpenLink.Location = new System.Drawing.Point(762, 62);
            this.btn_OpenLink.Name = "btn_OpenLink";
            this.btn_OpenLink.Size = new System.Drawing.Size(75, 23);
            this.btn_OpenLink.TabIndex = 1;
            this.btn_OpenLink.Text = "打开链接";
            this.btn_OpenLink.UseVisualStyleBackColor = true;
            this.btn_OpenLink.Click += new System.EventHandler(this.btn_OpenLink_Click);
            // 
            // textBox_Url
            // 
            this.textBox_Url.Location = new System.Drawing.Point(0, 64);
            this.textBox_Url.Name = "textBox_Url";
            this.textBox_Url.Size = new System.Drawing.Size(743, 21);
            this.textBox_Url.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 701);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_Url);
            this.Controls.Add(this.btn_OpenLink);
            this.Controls.Add(this.webBrowser_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form_Main";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser_Main;
        private System.Windows.Forms.Button btn_OpenLink;
        private System.Windows.Forms.TextBox textBox_Url;
        private System.Windows.Forms.Button button1;
    }
}

