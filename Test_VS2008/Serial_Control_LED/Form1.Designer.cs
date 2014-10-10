namespace Serial_Control_LED
{
    partial class Form1
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
            this.button_ON = new System.Windows.Forms.Button();
            this.button_OFF = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox_COM = new System.Windows.Forms.ComboBox();
            this.button_Conn = new System.Windows.Forms.Button();
            this.label_Status = new System.Windows.Forms.Label();
            this.button_BLINK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_ON
            // 
            this.button_ON.Location = new System.Drawing.Point(8, 39);
            this.button_ON.Name = "button_ON";
            this.button_ON.Size = new System.Drawing.Size(34, 23);
            this.button_ON.TabIndex = 0;
            this.button_ON.Text = "开";
            this.button_ON.UseVisualStyleBackColor = true;
            this.button_ON.Click += new System.EventHandler(this.button_ON_Click);
            // 
            // button_OFF
            // 
            this.button_OFF.Location = new System.Drawing.Point(98, 39);
            this.button_OFF.Name = "button_OFF";
            this.button_OFF.Size = new System.Drawing.Size(34, 23);
            this.button_OFF.TabIndex = 1;
            this.button_OFF.Text = "关";
            this.button_OFF.UseVisualStyleBackColor = true;
            this.button_OFF.Click += new System.EventHandler(this.button_OFF_Click);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox1.Location = new System.Drawing.Point(0, 154);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(284, 108);
            this.textBox1.TabIndex = 2;
            // 
            // comboBox_COM
            // 
            this.comboBox_COM.FormattingEnabled = true;
            this.comboBox_COM.Location = new System.Drawing.Point(12, 12);
            this.comboBox_COM.Name = "comboBox_COM";
            this.comboBox_COM.Size = new System.Drawing.Size(55, 20);
            this.comboBox_COM.TabIndex = 3;
            this.comboBox_COM.Text = "端口";
            this.comboBox_COM.DropDown += new System.EventHandler(this.comboBox_COM_DropDown);
            // 
            // button_Conn
            // 
            this.button_Conn.Location = new System.Drawing.Point(73, 10);
            this.button_Conn.Name = "button_Conn";
            this.button_Conn.Size = new System.Drawing.Size(48, 23);
            this.button_Conn.TabIndex = 4;
            this.button_Conn.Text = "连接";
            this.button_Conn.UseVisualStyleBackColor = true;
            this.button_Conn.Click += new System.EventHandler(this.button_Conn_Click);
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Location = new System.Drawing.Point(127, 15);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(41, 12);
            this.label_Status.TabIndex = 5;
            this.label_Status.Text = "label1";
            // 
            // button_BLINK
            // 
            this.button_BLINK.Location = new System.Drawing.Point(48, 39);
            this.button_BLINK.Name = "button_BLINK";
            this.button_BLINK.Size = new System.Drawing.Size(44, 23);
            this.button_BLINK.TabIndex = 6;
            this.button_BLINK.Text = "闪灯";
            this.button_BLINK.UseVisualStyleBackColor = true;
            this.button_BLINK.Click += new System.EventHandler(this.button_BLINK_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button_BLINK);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.button_Conn);
            this.Controls.Add(this.comboBox_COM);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button_OFF);
            this.Controls.Add(this.button_ON);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ON;
        private System.Windows.Forms.Button button_OFF;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox_COM;
        private System.Windows.Forms.Button button_Conn;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.Button button_BLINK;

    }
}

