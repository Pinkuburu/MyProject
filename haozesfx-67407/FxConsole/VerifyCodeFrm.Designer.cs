namespace FxConsole
{
    partial class VerifyCodeFrm
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
            this.btnSure = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkRetrive = new System.Windows.Forms.LinkLabel();
            this.txtPicNum = new System.Windows.Forms.TextBox();
            this.lblTip = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(20, 124);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(87, 23);
            this.btnSure.TabIndex = 0;
            this.btnSure.Text = "确定";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // linkRetrive
            // 
            this.linkRetrive.AutoSize = true;
            this.linkRetrive.Location = new System.Drawing.Point(18, 82);
            this.linkRetrive.Name = "linkRetrive";
            this.linkRetrive.Size = new System.Drawing.Size(89, 12);
            this.linkRetrive.TabIndex = 2;
            this.linkRetrive.TabStop = true;
            this.linkRetrive.Text = "重新获取验证码";
            // 
            // txtPicNum
            // 
            this.txtPicNum.Location = new System.Drawing.Point(12, 97);
            this.txtPicNum.Name = "txtPicNum";
            this.txtPicNum.Size = new System.Drawing.Size(100, 21);
            this.txtPicNum.TabIndex = 3;
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Location = new System.Drawing.Point(10, 9);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(41, 12);
            this.lblTip.TabIndex = 4;
            this.lblTip.Text = "label1";
            // 
            // VerifyCodeFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(126, 152);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.txtPicNum);
            this.Controls.Add(this.linkRetrive);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnSure);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VerifyCodeFrm";
            this.Text = "验证码";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkRetrive;
        private System.Windows.Forms.TextBox txtPicNum;
        private System.Windows.Forms.Label lblTip;
    }
}