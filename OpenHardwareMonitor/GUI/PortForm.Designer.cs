namespace OpenHardwareMonitor.GUI {
  partial class PortForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        this.portOKButton = new System.Windows.Forms.Button();
        this.portCancelButton = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.label4 = new System.Windows.Forms.Label();
        this.webServerLinkLabel = new System.Windows.Forms.LinkLabel();
        this.portNumericUpDn = new System.Windows.Forms.NumericUpDown();
        this.label5 = new System.Windows.Forms.Label();
        ((System.ComponentModel.ISupportInitialize)(this.portNumericUpDn)).BeginInit();
        this.SuspendLayout();
        // 
        // portOKButton
        // 
        this.portOKButton.Location = new System.Drawing.Point(219, 124);
        this.portOKButton.Name = "portOKButton";
        this.portOKButton.Size = new System.Drawing.Size(75, 21);
        this.portOKButton.TabIndex = 0;
        this.portOKButton.Text = "确定";
        this.portOKButton.UseVisualStyleBackColor = true;
        this.portOKButton.Click += new System.EventHandler(this.portOKButton_Click);
        // 
        // portCancelButton
        // 
        this.portCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.portCancelButton.Location = new System.Drawing.Point(137, 124);
        this.portCancelButton.Name = "portCancelButton";
        this.portCancelButton.Size = new System.Drawing.Size(75, 21);
        this.portCancelButton.TabIndex = 1;
        this.portCancelButton.Text = "取消";
        this.portCancelButton.UseVisualStyleBackColor = true;
        this.portCancelButton.Click += new System.EventHandler(this.portCancelButton_Click);
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(13, 98);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(293, 12);
        this.label1.TabIndex = 3;
        this.label1.Text = "注意：您将需要打开端口，在防火墙设置的操作系统。";
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(13, 11);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(131, 12);
        this.label2.TabIndex = 4;
        this.label2.Text = "远程 Web 服务器端口：";
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(13, 36);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(383, 12);
        this.label3.TabIndex = 5;
        this.label3.Text = "如果 Web 服务器正在运行，那么将需要重新启动，端口才能更改生效。";
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Location = new System.Drawing.Point(13, 57);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(173, 12);
        this.label4.TabIndex = 6;
        this.label4.Text = "Web 服务器将可以从浏览器访问";
        // 
        // webServerLinkLabel
        // 
        this.webServerLinkLabel.AutoSize = true;
        this.webServerLinkLabel.Location = new System.Drawing.Point(209, 57);
        this.webServerLinkLabel.Name = "webServerLinkLabel";
        this.webServerLinkLabel.Size = new System.Drawing.Size(65, 12);
        this.webServerLinkLabel.TabIndex = 7;
        this.webServerLinkLabel.TabStop = true;
        this.webServerLinkLabel.Text = "linkLabel1";
        this.webServerLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.webServerLinkLabel_LinkClicked);
        // 
        // portNumericUpDn
        // 
        this.portNumericUpDn.Location = new System.Drawing.Point(154, 7);
        this.portNumericUpDn.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
        this.portNumericUpDn.Minimum = new decimal(new int[] {
            8080,
            0,
            0,
            0});
        this.portNumericUpDn.Name = "portNumericUpDn";
        this.portNumericUpDn.Size = new System.Drawing.Size(75, 21);
        this.portNumericUpDn.TabIndex = 8;
        this.portNumericUpDn.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
        this.portNumericUpDn.ValueChanged += new System.EventHandler(this.portNumericUpDn_ValueChanged);
        // 
        // label5
        // 
        this.label5.AutoSize = true;
        this.label5.Location = new System.Drawing.Point(13, 78);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(281, 12);
        this.label5.TabIndex = 9;
        this.label5.Text = "你将不得不通过从菜单中单击“运行”启动服务器。";
        // 
        // PortForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.portCancelButton;
        this.ClientSize = new System.Drawing.Size(466, 157);
        this.Controls.Add(this.label5);
        this.Controls.Add(this.portNumericUpDn);
        this.Controls.Add(this.webServerLinkLabel);
        this.Controls.Add(this.label4);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.portCancelButton);
        this.Controls.Add(this.portOKButton);
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "PortForm";
        this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "设置端口";
        this.Load += new System.EventHandler(this.PortForm_Load);
        ((System.ComponentModel.ISupportInitialize)(this.portNumericUpDn)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button portOKButton;
    private System.Windows.Forms.Button portCancelButton;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.LinkLabel webServerLinkLabel;
    private System.Windows.Forms.NumericUpDown portNumericUpDn;
    private System.Windows.Forms.Label label5;
  }
}