namespace BDBBS_SendMessage
{
    partial class BD_Login_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BD_Login_Form));
            this.pictureBox_LoginForm_Image = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_LoginForm_UserName = new System.Windows.Forms.TextBox();
            this.textBox_LoginForm_Password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_LoginForm_ImageCode = new System.Windows.Forms.TextBox();
            this.pictureBox_LoginForm_ImageCode = new System.Windows.Forms.PictureBox();
            this.button_LoginForm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LoginForm_Image)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LoginForm_ImageCode)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_LoginForm_Image
            // 
            this.pictureBox_LoginForm_Image.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_LoginForm_Image.Image")));
            this.pictureBox_LoginForm_Image.ImageLocation = "";
            this.pictureBox_LoginForm_Image.Location = new System.Drawing.Point(49, 12);
            this.pictureBox_LoginForm_Image.Name = "pictureBox_LoginForm_Image";
            this.pictureBox_LoginForm_Image.Size = new System.Drawing.Size(125, 53);
            this.pictureBox_LoginForm_Image.TabIndex = 0;
            this.pictureBox_LoginForm_Image.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户名:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "密　码:";
            // 
            // textBox_LoginForm_UserName
            // 
            this.textBox_LoginForm_UserName.Location = new System.Drawing.Point(84, 78);
            this.textBox_LoginForm_UserName.Name = "textBox_LoginForm_UserName";
            this.textBox_LoginForm_UserName.Size = new System.Drawing.Size(112, 21);
            this.textBox_LoginForm_UserName.TabIndex = 3;
            // 
            // textBox_LoginForm_Password
            // 
            this.textBox_LoginForm_Password.Location = new System.Drawing.Point(84, 105);
            this.textBox_LoginForm_Password.Name = "textBox_LoginForm_Password";
            this.textBox_LoginForm_Password.Size = new System.Drawing.Size(112, 21);
            this.textBox_LoginForm_Password.TabIndex = 4;
            this.textBox_LoginForm_Password.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "验证码:";
            // 
            // textBox_LoginForm_ImageCode
            // 
            this.textBox_LoginForm_ImageCode.Location = new System.Drawing.Point(84, 132);
            this.textBox_LoginForm_ImageCode.Name = "textBox_LoginForm_ImageCode";
            this.textBox_LoginForm_ImageCode.Size = new System.Drawing.Size(46, 21);
            this.textBox_LoginForm_ImageCode.TabIndex = 6;
            // 
            // pictureBox_LoginForm_ImageCode
            // 
            this.pictureBox_LoginForm_ImageCode.Location = new System.Drawing.Point(136, 132);
            this.pictureBox_LoginForm_ImageCode.Name = "pictureBox_LoginForm_ImageCode";
            this.pictureBox_LoginForm_ImageCode.Size = new System.Drawing.Size(60, 20);
            this.pictureBox_LoginForm_ImageCode.TabIndex = 7;
            this.pictureBox_LoginForm_ImageCode.TabStop = false;
            this.pictureBox_LoginForm_ImageCode.Click += new System.EventHandler(this.pictureBox_LoginForm_ImageCode_Click);
            // 
            // button_LoginForm
            // 
            this.button_LoginForm.Location = new System.Drawing.Point(131, 158);
            this.button_LoginForm.Name = "button_LoginForm";
            this.button_LoginForm.Size = new System.Drawing.Size(65, 23);
            this.button_LoginForm.TabIndex = 8;
            this.button_LoginForm.Text = "登　录";
            this.button_LoginForm.UseVisualStyleBackColor = true;
            this.button_LoginForm.Click += new System.EventHandler(this.button_LoginForm_Click);
            // 
            // BD_Login_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 197);
            this.Controls.Add(this.button_LoginForm);
            this.Controls.Add(this.pictureBox_LoginForm_ImageCode);
            this.Controls.Add(this.textBox_LoginForm_ImageCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_LoginForm_Password);
            this.Controls.Add(this.textBox_LoginForm_UserName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox_LoginForm_Image);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BD_Login_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "半岛社区顶贴机";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LoginForm_Image)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LoginForm_ImageCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_LoginForm_Image;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_LoginForm_UserName;
        private System.Windows.Forms.TextBox textBox_LoginForm_Password;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_LoginForm_ImageCode;
        private System.Windows.Forms.PictureBox pictureBox_LoginForm_ImageCode;
        private System.Windows.Forms.Button button_LoginForm;
    }
}

