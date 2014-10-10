namespace XiaoNeiLogin
{
    partial class Login
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
            this.btn_Login = new System.Windows.Forms.Button();
            this.pictureBox_Login = new System.Windows.Forms.PictureBox();
            this.label_Login_UserID = new System.Windows.Forms.Label();
            this.label_Login_NickName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Login_UserName = new System.Windows.Forms.TextBox();
            this.textBox_Login_Password = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Login)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Login
            // 
            this.btn_Login.Location = new System.Drawing.Point(108, 81);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(53, 23);
            this.btn_Login.TabIndex = 0;
            this.btn_Login.Text = "Login";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // pictureBox_Login
            // 
            this.pictureBox_Login.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_Login.Location = new System.Drawing.Point(12, 12);
            this.pictureBox_Login.Name = "pictureBox_Login";
            this.pictureBox_Login.Size = new System.Drawing.Size(50, 50);
            this.pictureBox_Login.TabIndex = 1;
            this.pictureBox_Login.TabStop = false;
            // 
            // label_Login_UserID
            // 
            this.label_Login_UserID.AutoSize = true;
            this.label_Login_UserID.Location = new System.Drawing.Point(68, 19);
            this.label_Login_UserID.Name = "label_Login_UserID";
            this.label_Login_UserID.Size = new System.Drawing.Size(0, 12);
            this.label_Login_UserID.TabIndex = 2;
            // 
            // label_Login_NickName
            // 
            this.label_Login_NickName.AutoSize = true;
            this.label_Login_NickName.Location = new System.Drawing.Point(68, 39);
            this.label_Login_NickName.Name = "label_Login_NickName";
            this.label_Login_NickName.Size = new System.Drawing.Size(0, 12);
            this.label_Login_NickName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "帐号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "密码：";
            // 
            // textBox_Login_UserName
            // 
            this.textBox_Login_UserName.Location = new System.Drawing.Point(74, 16);
            this.textBox_Login_UserName.Name = "textBox_Login_UserName";
            this.textBox_Login_UserName.Size = new System.Drawing.Size(100, 21);
            this.textBox_Login_UserName.TabIndex = 6;
            // 
            // textBox_Login_Password
            // 
            this.textBox_Login_Password.Location = new System.Drawing.Point(74, 43);
            this.textBox_Login_Password.Name = "textBox_Login_Password";
            this.textBox_Login_Password.Size = new System.Drawing.Size(100, 21);
            this.textBox_Login_Password.TabIndex = 7;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 126);
            this.Controls.Add(this.textBox_Login_Password);
            this.Controls.Add(this.textBox_Login_UserName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_Login_NickName);
            this.Controls.Add(this.label_Login_UserID);
            this.Controls.Add(this.pictureBox_Login);
            this.Controls.Add(this.btn_Login);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Login";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Login)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.PictureBox pictureBox_Login;
        private System.Windows.Forms.Label label_Login_UserID;
        private System.Windows.Forms.Label label_Login_NickName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Login_UserName;
        private System.Windows.Forms.TextBox textBox_Login_Password;
    }
}

