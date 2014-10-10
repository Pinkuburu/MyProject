namespace Magic_Farm
{
    partial class ImgCode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImgCode));
            this.button_ImgCode = new System.Windows.Forms.Button();
            this.textBox_ImgCode = new System.Windows.Forms.TextBox();
            this.pictureBox_ImgCode = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ImgCode)).BeginInit();
            this.SuspendLayout();
            // 
            // button_ImgCode
            // 
            this.button_ImgCode.Location = new System.Drawing.Point(103, 54);
            this.button_ImgCode.Name = "button_ImgCode";
            this.button_ImgCode.Size = new System.Drawing.Size(54, 23);
            this.button_ImgCode.TabIndex = 2;
            this.button_ImgCode.Text = "提交";
            this.button_ImgCode.UseVisualStyleBackColor = true;
            this.button_ImgCode.Click += new System.EventHandler(this.button_ImgCode_Click);
            // 
            // textBox_ImgCode
            // 
            this.textBox_ImgCode.Location = new System.Drawing.Point(2, 56);
            this.textBox_ImgCode.Name = "textBox_ImgCode";
            this.textBox_ImgCode.Size = new System.Drawing.Size(90, 21);
            this.textBox_ImgCode.TabIndex = 1;
            // 
            // pictureBox_ImgCode
            // 
            this.pictureBox_ImgCode.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_ImgCode.Name = "pictureBox_ImgCode";
            this.pictureBox_ImgCode.Size = new System.Drawing.Size(165, 50);
            this.pictureBox_ImgCode.TabIndex = 0;
            this.pictureBox_ImgCode.TabStop = false;
            this.pictureBox_ImgCode.Click += new System.EventHandler(this.pictureBox_ImgCode_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(39, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "点击图片可刷新";
            // 
            // ImgCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(165, 99);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_ImgCode);
            this.Controls.Add(this.textBox_ImgCode);
            this.Controls.Add(this.pictureBox_ImgCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImgCode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "淘博园-验证码输入";
            this.Load += new System.EventHandler(this.ImgCode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ImgCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ImgCode;
        private System.Windows.Forms.TextBox textBox_ImgCode;
        private System.Windows.Forms.PictureBox pictureBox_ImgCode;
        private System.Windows.Forms.Label label1;
    }
}