namespace ServerTurnCheck
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
            this.Button_Setup = new QQRobot.Skin.BasicButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.qQtextBox_FootBall = new QQRobot.Skin.QQtextBox();
            this.qQtextBox_BasketBall = new QQRobot.Skin.QQtextBox();
            this.SuspendLayout();
            // 
            // Button_Setup
            // 
            this.Button_Setup.BackColor = System.Drawing.Color.Transparent;
            this.Button_Setup.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Button_Setup.ForeColor = System.Drawing.Color.DarkBlue;
            this.Button_Setup.Location = new System.Drawing.Point(196, 305);
            this.Button_Setup.Name = "Button_Setup";
            this.Button_Setup.Size = new System.Drawing.Size(69, 21);
            this.Button_Setup.TabIndex = 24;
            this.Button_Setup.Texts = "设置";
            this.Button_Setup.Click += new System.EventHandler(this.Button_Setup_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(21, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 25;
            this.label1.Text = "足球项目：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(21, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 26;
            this.label2.Text = "篮球项目：";
            // 
            // qQtextBox_FootBall
            // 
            this.qQtextBox_FootBall.BackColor = System.Drawing.Color.White;
            this.qQtextBox_FootBall.Icon = null;
            this.qQtextBox_FootBall.Location = new System.Drawing.Point(92, 52);
            this.qQtextBox_FootBall.Multiline = true;
            this.qQtextBox_FootBall.Name = "qQtextBox_FootBall";
            this.qQtextBox_FootBall.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.qQtextBox_FootBall.Size = new System.Drawing.Size(347, 101);
            this.qQtextBox_FootBall.TabIndex = 27;
            // 
            // qQtextBox_BasketBall
            // 
            this.qQtextBox_BasketBall.BackColor = System.Drawing.Color.White;
            this.qQtextBox_BasketBall.Icon = null;
            this.qQtextBox_BasketBall.Location = new System.Drawing.Point(92, 163);
            this.qQtextBox_BasketBall.Multiline = true;
            this.qQtextBox_BasketBall.Name = "qQtextBox_BasketBall";
            this.qQtextBox_BasketBall.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.qQtextBox_BasketBall.Size = new System.Drawing.Size(347, 101);
            this.qQtextBox_BasketBall.TabIndex = 28;
            // 
            // set
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 338);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.qQtextBox_FootBall);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.qQtextBox_BasketBall);
            this.Controls.Add(this.Button_Setup);
            this.Name = "set";
            this.Text = "设置";
            this.Controls.SetChildIndex(this.Button_Setup, 0);
            this.Controls.SetChildIndex(this.qQtextBox_BasketBall, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.qQtextBox_FootBall, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private QQRobot.Skin.BasicButton Button_Setup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private QQRobot.Skin.QQtextBox qQtextBox_FootBall;
        private QQRobot.Skin.QQtextBox qQtextBox_BasketBall;
    }
}