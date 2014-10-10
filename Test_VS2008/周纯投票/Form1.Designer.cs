namespace 周纯投票
{
    partial class Form1
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
            this.pictureBox_Code = new System.Windows.Forms.PictureBox();
            this.textBox_Code = new System.Windows.Forms.TextBox();
            this.textBox_Msg = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Code)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Code
            // 
            this.pictureBox_Code.Location = new System.Drawing.Point(57, 12);
            this.pictureBox_Code.Name = "pictureBox_Code";
            this.pictureBox_Code.Size = new System.Drawing.Size(64, 20);
            this.pictureBox_Code.TabIndex = 0;
            this.pictureBox_Code.TabStop = false;
            this.pictureBox_Code.Click += new System.EventHandler(this.pictureBox_Code_Click);
            // 
            // textBox_Code
            // 
            this.textBox_Code.Location = new System.Drawing.Point(39, 38);
            this.textBox_Code.Name = "textBox_Code";
            this.textBox_Code.Size = new System.Drawing.Size(100, 21);
            this.textBox_Code.TabIndex = 1;
            // 
            // textBox_Msg
            // 
            this.textBox_Msg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox_Msg.Location = new System.Drawing.Point(0, 65);
            this.textBox_Msg.Multiline = true;
            this.textBox_Msg.Name = "textBox_Msg";
            this.textBox_Msg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Msg.Size = new System.Drawing.Size(185, 114);
            this.textBox_Msg.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(185, 179);
            this.Controls.Add(this.textBox_Msg);
            this.Controls.Add(this.textBox_Code);
            this.Controls.Add(this.pictureBox_Code);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Code)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Code;
        private System.Windows.Forms.TextBox textBox_Code;
        private System.Windows.Forms.TextBox textBox_Msg;
    }
}

