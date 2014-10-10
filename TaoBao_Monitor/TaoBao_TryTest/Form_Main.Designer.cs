namespace TaoBao_TryTest
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
            this.textBox_Debug = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // webBrowser_Main
            // 
            this.webBrowser_Main.Dock = System.Windows.Forms.DockStyle.Top;
            this.webBrowser_Main.Location = new System.Drawing.Point(0, 0);
            this.webBrowser_Main.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser_Main.Name = "webBrowser_Main";
            this.webBrowser_Main.ScrollBarsEnabled = false;
            this.webBrowser_Main.Size = new System.Drawing.Size(795, 315);
            this.webBrowser_Main.TabIndex = 0;
            this.webBrowser_Main.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_Main_DocumentCompleted);
            // 
            // textBox_Debug
            // 
            this.textBox_Debug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Debug.Location = new System.Drawing.Point(0, 315);
            this.textBox_Debug.Multiline = true;
            this.textBox_Debug.Name = "textBox_Debug";
            this.textBox_Debug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Debug.Size = new System.Drawing.Size(795, 103);
            this.textBox_Debug.TabIndex = 1;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 418);
            this.Controls.Add(this.textBox_Debug);
            this.Controls.Add(this.webBrowser_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form_Main";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser_Main;
        private System.Windows.Forms.TextBox textBox_Debug;

    }
}

