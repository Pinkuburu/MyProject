namespace 挂号_Form
{
    partial class Main_Form
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_UserName = new System.Windows.Forms.TextBox();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.textBox_Code = new System.Windows.Forms.TextBox();
            this.button_Login = new System.Windows.Forms.Button();
            this.pictureBox_Code = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Code)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "密　码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "验证码：";
            // 
            // textBox_UserName
            // 
            this.textBox_UserName.Location = new System.Drawing.Point(59, 6);
            this.textBox_UserName.Name = "textBox_UserName";
            this.textBox_UserName.Size = new System.Drawing.Size(100, 21);
            this.textBox_UserName.TabIndex = 3;
            // 
            // textBox_Password
            // 
            this.textBox_Password.Location = new System.Drawing.Point(59, 34);
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.Size = new System.Drawing.Size(100, 21);
            this.textBox_Password.TabIndex = 4;
            // 
            // textBox_Code
            // 
            this.textBox_Code.Location = new System.Drawing.Point(59, 61);
            this.textBox_Code.Name = "textBox_Code";
            this.textBox_Code.Size = new System.Drawing.Size(100, 21);
            this.textBox_Code.TabIndex = 5;
            // 
            // button_Login
            // 
            this.button_Login.Location = new System.Drawing.Point(165, 6);
            this.button_Login.Name = "button_Login";
            this.button_Login.Size = new System.Drawing.Size(75, 49);
            this.button_Login.TabIndex = 6;
            this.button_Login.Text = "登　录";
            this.button_Login.UseVisualStyleBackColor = true;
            this.button_Login.Click += new System.EventHandler(this.button_Login_Click);
            // 
            // pictureBox_Code
            // 
            this.pictureBox_Code.BackColor = System.Drawing.SystemColors.Info;
            this.pictureBox_Code.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_Code.Location = new System.Drawing.Point(167, 58);
            this.pictureBox_Code.Name = "pictureBox_Code";
            this.pictureBox_Code.Size = new System.Drawing.Size(70, 24);
            this.pictureBox_Code.TabIndex = 7;
            this.pictureBox_Code.TabStop = false;
            this.pictureBox_Code.Click += new System.EventHandler(this.pictureBox_Code_Click);
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 437);
            this.Controls.Add(this.pictureBox_Code);
            this.Controls.Add(this.button_Login);
            this.Controls.Add(this.textBox_Code);
            this.Controls.Add(this.textBox_Password);
            this.Controls.Add(this.textBox_UserName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Main_Form";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Code)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_UserName;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.TextBox textBox_Code;
        private System.Windows.Forms.Button button_Login;
        private System.Windows.Forms.PictureBox pictureBox_Code;
    }
}

