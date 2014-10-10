namespace Happyisland
{
    partial class Login_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login_Form));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_Login = new System.Windows.Forms.Button();
            this.textBox_LoginForm_UserName = new System.Windows.Forms.TextBox();
            this.textBox_Login_Form_Password = new System.Windows.Forms.TextBox();
            this.radioButton_Login_Form_RR = new System.Windows.Forms.RadioButton();
            this.radioButton_Login_Form_TB = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(430, 222);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btn_Login
            // 
            this.btn_Login.Location = new System.Drawing.Point(339, 69);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(66, 48);
            this.btn_Login.TabIndex = 1;
            this.btn_Login.Text = "Login";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // textBox_LoginForm_UserName
            // 
            this.textBox_LoginForm_UserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_LoginForm_UserName.Location = new System.Drawing.Point(226, 69);
            this.textBox_LoginForm_UserName.Name = "textBox_LoginForm_UserName";
            this.textBox_LoginForm_UserName.Size = new System.Drawing.Size(107, 21);
            this.textBox_LoginForm_UserName.TabIndex = 2;
            // 
            // textBox_Login_Form_Password
            // 
            this.textBox_Login_Form_Password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Login_Form_Password.Location = new System.Drawing.Point(226, 96);
            this.textBox_Login_Form_Password.Name = "textBox_Login_Form_Password";
            this.textBox_Login_Form_Password.Size = new System.Drawing.Size(107, 21);
            this.textBox_Login_Form_Password.TabIndex = 3;
            this.textBox_Login_Form_Password.UseSystemPasswordChar = true;
            // 
            // radioButton_Login_Form_RR
            // 
            this.radioButton_Login_Form_RR.AutoSize = true;
            this.radioButton_Login_Form_RR.Checked = true;
            this.radioButton_Login_Form_RR.Location = new System.Drawing.Point(226, 132);
            this.radioButton_Login_Form_RR.Name = "radioButton_Login_Form_RR";
            this.radioButton_Login_Form_RR.Size = new System.Drawing.Size(59, 16);
            this.radioButton_Login_Form_RR.TabIndex = 4;
            this.radioButton_Login_Form_RR.TabStop = true;
            this.radioButton_Login_Form_RR.Text = "人人网";
            this.radioButton_Login_Form_RR.UseVisualStyleBackColor = true;
            // 
            // radioButton_Login_Form_TB
            // 
            this.radioButton_Login_Form_TB.AutoSize = true;
            this.radioButton_Login_Form_TB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButton_Login_Form_TB.Location = new System.Drawing.Point(291, 132);
            this.radioButton_Login_Form_TB.Name = "radioButton_Login_Form_TB";
            this.radioButton_Login_Form_TB.Size = new System.Drawing.Size(59, 16);
            this.radioButton_Login_Form_TB.TabIndex = 5;
            this.radioButton_Login_Form_TB.TabStop = true;
            this.radioButton_Login_Form_TB.Text = "淘宝网";
            this.radioButton_Login_Form_TB.UseVisualStyleBackColor = true;
            // 
            // Login_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 194);
            this.Controls.Add(this.radioButton_Login_Form_TB);
            this.Controls.Add(this.radioButton_Login_Form_RR);
            this.Controls.Add(this.textBox_Login_Form_Password);
            this.Controls.Add(this.textBox_LoginForm_UserName);
            this.Controls.Add(this.btn_Login);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login_Form";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.TextBox textBox_LoginForm_UserName;
        private System.Windows.Forms.TextBox textBox_Login_Form_Password;
        private System.Windows.Forms.RadioButton radioButton_Login_Form_RR;
        private System.Windows.Forms.RadioButton radioButton_Login_Form_TB;
    }
}