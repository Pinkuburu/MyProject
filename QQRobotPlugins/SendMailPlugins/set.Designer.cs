namespace SendMailPlugins
{
    partial class set
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.basicButton_Setup = new QQRobot.Skin.BasicButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.alTextBox_Mail = new QQRobot.Skin.Controls.AlSkinControl.AlSkinTextBox.AlTextBox();
            this.alTextBox_Pwd = new QQRobot.Skin.Controls.AlSkinControl.AlSkinTextBox.AlTextBox();
            this.SuspendLayout();
            // 
            // basicButton_Setup
            // 
            this.basicButton_Setup.BackColor = System.Drawing.Color.Transparent;
            this.basicButton_Setup.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.basicButton_Setup.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton_Setup.Location = new System.Drawing.Point(90, 143);
            this.basicButton_Setup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.basicButton_Setup.Name = "basicButton_Setup";
            this.basicButton_Setup.Size = new System.Drawing.Size(69, 21);
            this.basicButton_Setup.TabIndex = 24;
            this.basicButton_Setup.Texts = "设置";
            this.basicButton_Setup.Click += new System.EventHandler(this.basicButton_Setup_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 25;
            this.label1.Text = "发件人邮箱：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 26;
            this.label2.Text = "发件人密码：";
            // 
            // alTextBox_Mail
            // 
            this.alTextBox_Mail.BackColor = System.Drawing.Color.Transparent;
            this.alTextBox_Mail.FontColor = System.Drawing.SystemColors.WindowText;
            this.alTextBox_Mail.Ico = null;
            this.alTextBox_Mail.IcoPadding = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.alTextBox_Mail.Isico = false;
            this.alTextBox_Mail.IsPass = false;
            this.alTextBox_Mail.Lines = new string[0];
            this.alTextBox_Mail.Location = new System.Drawing.Point(90, 46);
            this.alTextBox_Mail.MaxLength = 32767;
            this.alTextBox_Mail.Multiline = false;
            this.alTextBox_Mail.Name = "alTextBox_Mail";
            this.alTextBox_Mail.PasswordChar = '\0';
            this.alTextBox_Mail.ReadOnly = false;
            this.alTextBox_Mail.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.alTextBox_Mail.Size = new System.Drawing.Size(114, 22);
            this.alTextBox_Mail.TabIndex = 27;
            // 
            // alTextBox_Pwd
            // 
            this.alTextBox_Pwd.BackColor = System.Drawing.Color.Transparent;
            this.alTextBox_Pwd.FontColor = System.Drawing.SystemColors.WindowText;
            this.alTextBox_Pwd.Ico = null;
            this.alTextBox_Pwd.IcoPadding = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.alTextBox_Pwd.Isico = false;
            this.alTextBox_Pwd.IsPass = true;
            this.alTextBox_Pwd.Lines = new string[0];
            this.alTextBox_Pwd.Location = new System.Drawing.Point(90, 68);
            this.alTextBox_Pwd.MaxLength = 32767;
            this.alTextBox_Pwd.Multiline = false;
            this.alTextBox_Pwd.Name = "alTextBox_Pwd";
            this.alTextBox_Pwd.PasswordChar = '●';
            this.alTextBox_Pwd.ReadOnly = false;
            this.alTextBox_Pwd.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.alTextBox_Pwd.Size = new System.Drawing.Size(114, 22);
            this.alTextBox_Pwd.TabIndex = 28;
            // 
            // set
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 176);
            this.Controls.Add(this.basicButton_Setup);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.alTextBox_Pwd);
            this.Controls.Add(this.alTextBox_Mail);
            this.Name = "set";
            this.Text = "设置";
            this.Controls.SetChildIndex(this.alTextBox_Mail, 0);
            this.Controls.SetChildIndex(this.alTextBox_Pwd, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.basicButton_Setup, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private QQRobot.Skin.BasicButton basicButton_Setup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private QQRobot.Skin.Controls.AlSkinControl.AlSkinTextBox.AlTextBox alTextBox_Mail;
        private QQRobot.Skin.Controls.AlSkinControl.AlSkinTextBox.AlTextBox alTextBox_Pwd;
    }
}