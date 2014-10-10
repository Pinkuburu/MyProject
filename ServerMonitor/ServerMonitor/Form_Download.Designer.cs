namespace ServerMonitor
{
    partial class Form_Download
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
            this.button_Download = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Url = new System.Windows.Forms.TextBox();
            this.textBox_Path = new System.Windows.Forms.TextBox();
            this.checkBox_unrar = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button_Download
            // 
            this.button_Download.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Download.Location = new System.Drawing.Point(99, 83);
            this.button_Download.Name = "button_Download";
            this.button_Download.Size = new System.Drawing.Size(75, 23);
            this.button_Download.TabIndex = 0;
            this.button_Download.Text = "下载";
            this.button_Download.UseVisualStyleBackColor = true;
            this.button_Download.Click += new System.EventHandler(this.button_Download_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "下载地址:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "保存路径:";
            // 
            // textBox_Url
            // 
            this.textBox_Url.Location = new System.Drawing.Point(78, 10);
            this.textBox_Url.Name = "textBox_Url";
            this.textBox_Url.Size = new System.Drawing.Size(194, 21);
            this.textBox_Url.TabIndex = 3;
            // 
            // textBox_Path
            // 
            this.textBox_Path.Location = new System.Drawing.Point(78, 34);
            this.textBox_Path.Name = "textBox_Path";
            this.textBox_Path.Size = new System.Drawing.Size(194, 21);
            this.textBox_Path.TabIndex = 4;
            // 
            // checkBox_unrar
            // 
            this.checkBox_unrar.AutoSize = true;
            this.checkBox_unrar.Location = new System.Drawing.Point(78, 61);
            this.checkBox_unrar.Name = "checkBox_unrar";
            this.checkBox_unrar.Size = new System.Drawing.Size(96, 16);
            this.checkBox_unrar.TabIndex = 5;
            this.checkBox_unrar.Text = "是否自动解压";
            this.checkBox_unrar.UseVisualStyleBackColor = true;
            // 
            // Form_Download
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 114);
            this.Controls.Add(this.checkBox_unrar);
            this.Controls.Add(this.textBox_Path);
            this.Controls.Add(this.textBox_Url);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_Download);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Download";
            this.Text = "文件下载";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Download;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Url;
        private System.Windows.Forms.TextBox textBox_Path;
        private System.Windows.Forms.CheckBox checkBox_unrar;
    }
}