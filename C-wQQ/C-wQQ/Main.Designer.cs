namespace C_wQQ
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_QQ = new System.Windows.Forms.TextBox();
            this.textBox_PWD = new System.Windows.Forms.TextBox();
            this.Button_LoginQQ = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_verifycode = new System.Windows.Forms.TextBox();
            this.pictureBox_verifypic = new System.Windows.Forms.PictureBox();
            this.checkBox_offline = new System.Windows.Forms.CheckBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_verifypic)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Q:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "P:";
            // 
            // textBox_QQ
            // 
            this.textBox_QQ.Location = new System.Drawing.Point(35, 12);
            this.textBox_QQ.Name = "textBox_QQ";
            this.textBox_QQ.Size = new System.Drawing.Size(100, 21);
            this.textBox_QQ.TabIndex = 2;
            this.textBox_QQ.Leave += new System.EventHandler(this.textBox_QQ_Leave);
            // 
            // textBox_PWD
            // 
            this.textBox_PWD.Location = new System.Drawing.Point(35, 39);
            this.textBox_PWD.Name = "textBox_PWD";
            this.textBox_PWD.Size = new System.Drawing.Size(100, 21);
            this.textBox_PWD.TabIndex = 3;
            // 
            // Button_LoginQQ
            // 
            this.Button_LoginQQ.Location = new System.Drawing.Point(35, 89);
            this.Button_LoginQQ.Name = "Button_LoginQQ";
            this.Button_LoginQQ.Size = new System.Drawing.Size(75, 23);
            this.Button_LoginQQ.TabIndex = 4;
            this.Button_LoginQQ.Text = "Login";
            this.Button_LoginQQ.UseVisualStyleBackColor = true;
            this.Button_LoginQQ.Click += new System.EventHandler(this.Button_LoginQQ_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "V:";
            this.label3.Visible = false;
            // 
            // textBox_verifycode
            // 
            this.textBox_verifycode.Location = new System.Drawing.Point(35, 124);
            this.textBox_verifycode.Name = "textBox_verifycode";
            this.textBox_verifycode.Size = new System.Drawing.Size(100, 21);
            this.textBox_verifycode.TabIndex = 6;
            this.textBox_verifycode.Visible = false;
            // 
            // pictureBox_verifypic
            // 
            this.pictureBox_verifypic.Location = new System.Drawing.Point(35, 151);
            this.pictureBox_verifypic.Name = "pictureBox_verifypic";
            this.pictureBox_verifypic.Size = new System.Drawing.Size(100, 50);
            this.pictureBox_verifypic.TabIndex = 7;
            this.pictureBox_verifypic.TabStop = false;
            this.pictureBox_verifypic.Visible = false;
            this.pictureBox_verifypic.Click += new System.EventHandler(this.pictureBox_verifypic_Click);
            // 
            // checkBox_offline
            // 
            this.checkBox_offline.AutoSize = true;
            this.checkBox_offline.Location = new System.Drawing.Point(35, 67);
            this.checkBox_offline.Name = "checkBox_offline";
            this.checkBox_offline.Size = new System.Drawing.Size(60, 16);
            this.checkBox_offline.TabIndex = 8;
            this.checkBox_offline.Text = "Hidden";
            this.checkBox_offline.UseVisualStyleBackColor = true;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "C-QQ";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(158, 211);
            this.Controls.Add(this.checkBox_offline);
            this.Controls.Add(this.pictureBox_verifypic);
            this.Controls.Add(this.textBox_verifycode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Button_LoginQQ);
            this.Controls.Add(this.textBox_PWD);
            this.Controls.Add(this.textBox_QQ);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "无限的未知";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(Main_FormClosed);
            this.SizeChanged += new System.EventHandler(this.Main_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_verifypic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_QQ;
        private System.Windows.Forms.TextBox textBox_PWD;
        private System.Windows.Forms.Button Button_LoginQQ;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_verifycode;
        private System.Windows.Forms.PictureBox pictureBox_verifypic;
        private System.Windows.Forms.CheckBox checkBox_offline;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

