namespace BuyTicket
{
    partial class LoginCode_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginCode_Form));
            this.pictureBox_Code = new System.Windows.Forms.PictureBox();
            this.button_Login = new System.Windows.Forms.Button();
            this.textBox_Code = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Code)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Code
            // 
            this.pictureBox_Code.Location = new System.Drawing.Point(12, 12);
            this.pictureBox_Code.Name = "pictureBox_Code";
            this.pictureBox_Code.Size = new System.Drawing.Size(142, 50);
            this.pictureBox_Code.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Code.TabIndex = 0;
            this.pictureBox_Code.TabStop = false;
            this.pictureBox_Code.Click += new System.EventHandler(this.pictureBox_Code_Click);
            // 
            // button_Login
            // 
            this.button_Login.Location = new System.Drawing.Point(12, 95);
            this.button_Login.Name = "button_Login";
            this.button_Login.Size = new System.Drawing.Size(142, 41);
            this.button_Login.TabIndex = 1;
            this.button_Login.Text = "登  录";
            this.button_Login.UseVisualStyleBackColor = true;
            this.button_Login.Click += new System.EventHandler(this.button_Login_Click);
            // 
            // textBox_Code
            // 
            this.textBox_Code.Location = new System.Drawing.Point(12, 68);
            this.textBox_Code.Name = "textBox_Code";
            this.textBox_Code.Size = new System.Drawing.Size(142, 21);
            this.textBox_Code.TabIndex = 2;
            this.textBox_Code.TextChanged += new System.EventHandler(this.textBox_Code_TextChanged);
            // 
            // LoginCode_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(166, 144);
            this.Controls.Add(this.textBox_Code);
            this.Controls.Add(this.button_Login);
            this.Controls.Add(this.pictureBox_Code);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginCode_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "输入验证码";
            this.Load += new System.EventHandler(this.LoginCode_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Code)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Code;
        private System.Windows.Forms.Button button_Login;
        private System.Windows.Forms.TextBox textBox_Code;
    }
}