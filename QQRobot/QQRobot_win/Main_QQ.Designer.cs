namespace QQRobot_win
{
    partial class Main_QQ
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_QQ));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_QQ = new System.Windows.Forms.TextBox();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.button_Login = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Code = new System.Windows.Forms.TextBox();
            this.pictureBox_ImageCode = new System.Windows.Forms.PictureBox();
            this.groupBox_Msg = new System.Windows.Forms.GroupBox();
            this.textBox_SysLog = new System.Windows.Forms.TextBox();
            this.button_ReadCode = new System.Windows.Forms.Button();
            this.button_ModifySign = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ImageCode)).BeginInit();
            this.groupBox_Msg.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "QQ号:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "密码:";
            // 
            // textBox_QQ
            // 
            this.textBox_QQ.Location = new System.Drawing.Point(53, 5);
            this.textBox_QQ.Name = "textBox_QQ";
            this.textBox_QQ.Size = new System.Drawing.Size(100, 21);
            this.textBox_QQ.TabIndex = 2;
            // 
            // textBox_Password
            // 
            this.textBox_Password.Location = new System.Drawing.Point(53, 32);
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.Size = new System.Drawing.Size(100, 21);
            this.textBox_Password.TabIndex = 3;
            // 
            // button_Login
            // 
            this.button_Login.Location = new System.Drawing.Point(345, 2);
            this.button_Login.Name = "button_Login";
            this.button_Login.Size = new System.Drawing.Size(53, 53);
            this.button_Login.TabIndex = 4;
            this.button_Login.Text = "Login";
            this.button_Login.UseVisualStyleBackColor = true;
            this.button_Login.Click += new System.EventHandler(this.button_Login_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "验证码";
            // 
            // textBox_Code
            // 
            this.textBox_Code.Location = new System.Drawing.Point(159, 32);
            this.textBox_Code.Name = "textBox_Code";
            this.textBox_Code.Size = new System.Drawing.Size(46, 21);
            this.textBox_Code.TabIndex = 6;
            // 
            // pictureBox_ImageCode
            // 
            this.pictureBox_ImageCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_ImageCode.Location = new System.Drawing.Point(209, 2);
            this.pictureBox_ImageCode.Name = "pictureBox_ImageCode";
            this.pictureBox_ImageCode.Size = new System.Drawing.Size(130, 53);
            this.pictureBox_ImageCode.TabIndex = 7;
            this.pictureBox_ImageCode.TabStop = false;
            this.pictureBox_ImageCode.Click += new System.EventHandler(this.pictureBox_ImageCode_Click);
            // 
            // groupBox_Msg
            // 
            this.groupBox_Msg.Controls.Add(this.textBox_SysLog);
            this.groupBox_Msg.Location = new System.Drawing.Point(13, 55);
            this.groupBox_Msg.Name = "groupBox_Msg";
            this.groupBox_Msg.Size = new System.Drawing.Size(385, 237);
            this.groupBox_Msg.TabIndex = 8;
            this.groupBox_Msg.TabStop = false;
            this.groupBox_Msg.Text = "运行日志";
            // 
            // textBox_SysLog
            // 
            this.textBox_SysLog.Location = new System.Drawing.Point(7, 21);
            this.textBox_SysLog.Multiline = true;
            this.textBox_SysLog.Name = "textBox_SysLog";
            this.textBox_SysLog.Size = new System.Drawing.Size(372, 210);
            this.textBox_SysLog.TabIndex = 0;
            // 
            // button_ReadCode
            // 
            this.button_ReadCode.Location = new System.Drawing.Point(20, 301);
            this.button_ReadCode.Name = "button_ReadCode";
            this.button_ReadCode.Size = new System.Drawing.Size(75, 23);
            this.button_ReadCode.TabIndex = 9;
            this.button_ReadCode.Text = "读取验证码";
            this.button_ReadCode.UseVisualStyleBackColor = true;
            this.button_ReadCode.Click += new System.EventHandler(this.button_ReadCode_Click);
            // 
            // button_ModifySign
            // 
            this.button_ModifySign.Location = new System.Drawing.Point(101, 301);
            this.button_ModifySign.Name = "button_ModifySign";
            this.button_ModifySign.Size = new System.Drawing.Size(75, 23);
            this.button_ModifySign.TabIndex = 10;
            this.button_ModifySign.Text = "修改签名";
            this.button_ModifySign.UseVisualStyleBackColor = true;
            this.button_ModifySign.Click += new System.EventHandler(this.button_ModifySign_Click);
            // 
            // Main_QQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 336);
            this.Controls.Add(this.button_ModifySign);
            this.Controls.Add(this.button_ReadCode);
            this.Controls.Add(this.groupBox_Msg);
            this.Controls.Add(this.pictureBox_ImageCode);
            this.Controls.Add(this.textBox_Code);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_Login);
            this.Controls.Add(this.textBox_Password);
            this.Controls.Add(this.textBox_QQ);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main_QQ";
            this.Text = "7";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ImageCode)).EndInit();
            this.groupBox_Msg.ResumeLayout(false);
            this.groupBox_Msg.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_QQ;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.Button button_Login;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Code;
        private System.Windows.Forms.PictureBox pictureBox_ImageCode;
        private System.Windows.Forms.GroupBox groupBox_Msg;
        private System.Windows.Forms.TextBox textBox_SysLog;
        private System.Windows.Forms.Button button_ReadCode;
        private System.Windows.Forms.Button button_ModifySign;
    }
}

