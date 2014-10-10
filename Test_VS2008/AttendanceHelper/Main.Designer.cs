namespace AttendanceHelper
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.lbRTShow = new System.Windows.Forms.ListBox();
            this.Lable_Count = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(275, 7);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(88, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server IP:";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(159, 9);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 21);
            this.txtIP.TabIndex = 2;
            this.txtIP.Text = "192.168.1.200";
            // 
            // lbRTShow
            // 
            this.lbRTShow.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbRTShow.FormattingEnabled = true;
            this.lbRTShow.ItemHeight = 12;
            this.lbRTShow.Location = new System.Drawing.Point(0, 42);
            this.lbRTShow.Name = "lbRTShow";
            this.lbRTShow.Size = new System.Drawing.Size(452, 220);
            this.lbRTShow.TabIndex = 3;
            // 
            // Lable_Count
            // 
            this.Lable_Count.AutoSize = true;
            this.Lable_Count.ForeColor = System.Drawing.Color.Red;
            this.Lable_Count.Location = new System.Drawing.Point(13, 13);
            this.Lable_Count.Name = "Lable_Count";
            this.Lable_Count.Size = new System.Drawing.Size(11, 12);
            this.Lable_Count.TabIndex = 4;
            this.Lable_Count.Text = "0";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 262);
            this.Controls.Add(this.Lable_Count);
            this.Controls.Add(this.lbRTShow);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "AttHelper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.ListBox lbRTShow;
        private System.Windows.Forms.Label Lable_Count;
    }
}

