namespace XPtable
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
            XPTable.Models.Row row1 = new XPTable.Models.Row();
            XPTable.Models.Cell cell1 = new XPTable.Models.Cell();
            XPTable.Models.Cell cell2 = new XPTable.Models.Cell();
            XPTable.Models.Cell cell3 = new XPTable.Models.Cell();
            XPTable.Models.RowStyle rowStyle1 = new XPTable.Models.RowStyle();
            XPTable.Models.Row row2 = new XPTable.Models.Row();
            XPTable.Models.Cell cell4 = new XPTable.Models.Cell();
            XPTable.Models.Cell cell5 = new XPTable.Models.Cell();
            XPTable.Models.Cell cell6 = new XPTable.Models.Cell();
            XPTable.Models.Row row3 = new XPTable.Models.Row();
            XPTable.Models.Cell cell7 = new XPTable.Models.Cell();
            XPTable.Models.Cell cell8 = new XPTable.Models.Cell();
            XPTable.Models.Cell cell9 = new XPTable.Models.Cell();
            XPTable.Models.Row row4 = new XPTable.Models.Row();
            XPTable.Models.Cell cell10 = new XPTable.Models.Cell();
            XPTable.Models.Cell cell11 = new XPTable.Models.Cell();
            XPTable.Models.Cell cell12 = new XPTable.Models.Cell();
            XPTable.Models.Row row5 = new XPTable.Models.Row();
            XPTable.Models.Cell cell13 = new XPTable.Models.Cell();
            XPTable.Models.Cell cell14 = new XPTable.Models.Cell();
            XPTable.Models.Cell cell15 = new XPTable.Models.Cell();
            XPTable.Models.Cell cell16 = new XPTable.Models.Cell();
            this.table1 = new XPTable.Models.Table();
            this.columnModel1 = new XPTable.Models.ColumnModel();
            this.textColumn1 = new XPTable.Models.TextColumn();
            this.textColumn2 = new XPTable.Models.TextColumn();
            this.textColumn3 = new XPTable.Models.TextColumn();
            this.tableModel1 = new XPTable.Models.TableModel();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            this.SuspendLayout();
            // 
            // table1
            // 
            this.table1.ColumnModel = this.columnModel1;
            this.table1.GridLines = XPTable.Models.GridLines.Both;
            this.table1.Location = new System.Drawing.Point(12, 12);
            this.table1.Name = "table1";
            this.table1.Size = new System.Drawing.Size(260, 208);
            this.table1.TabIndex = 0;
            this.table1.TableModel = this.tableModel1;
            this.table1.Text = "table1";
            // 
            // columnModel1
            // 
            this.columnModel1.Columns.AddRange(new XPTable.Models.Column[] {
            this.textColumn1,
            this.textColumn2,
            this.textColumn3});
            // 
            // textColumn1
            // 
            this.textColumn1.Alignment = XPTable.Models.ColumnAlignment.Center;
            this.textColumn1.Editable = false;
            this.textColumn1.Text = "ID";
            // 
            // textColumn2
            // 
            this.textColumn2.Alignment = XPTable.Models.ColumnAlignment.Center;
            this.textColumn2.Editable = false;
            this.textColumn2.Text = "好友";
            // 
            // textColumn3
            // 
            this.textColumn3.Alignment = XPTable.Models.ColumnAlignment.Center;
            this.textColumn3.Editable = false;
            this.textColumn3.Text = "等级";
            this.textColumn3.Width = 35;
            // 
            // tableModel1
            // 
            row1.BackColor = System.Drawing.Color.Transparent;
            cell1.Text = "1";
            cell2.Text = "16";
            cell3.Text = "789";
            row1.Cells.AddRange(new XPTable.Models.Cell[] {
            cell1,
            cell2,
            cell3});
            row1.ForeColor = System.Drawing.SystemColors.ControlText;
            rowStyle1.BackColor = System.Drawing.Color.Transparent;
            rowStyle1.Font = null;
            rowStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            row1.RowStyle = rowStyle1;
            cell4.Text = "789";
            cell5.Text = "456";
            cell6.Text = "123";
            row2.Cells.AddRange(new XPTable.Models.Cell[] {
            cell4,
            cell5,
            cell6});
            cell7.Text = "13";
            cell8.Text = "7";
            cell9.Text = "1";
            row3.Cells.AddRange(new XPTable.Models.Cell[] {
            cell7,
            cell8,
            cell9});
            cell10.Text = "7";
            cell11.Text = "2";
            cell12.Text = "25";
            row4.Cells.AddRange(new XPTable.Models.Cell[] {
            cell10,
            cell11,
            cell12});
            cell13.Text = "2";
            cell14.Text = "43";
            cell15.Text = "1";
            row5.Cells.AddRange(new XPTable.Models.Cell[] {
            cell13,
            cell14,
            cell15,
            cell16});
            this.tableModel1.Rows.AddRange(new XPTable.Models.Row[] {
            row1,
            row2,
            row3,
            row4,
            row5});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.table1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private XPTable.Models.Table table1;
        private XPTable.Models.TableModel tableModel1;
        private XPTable.Models.TextColumn textColumn1;
        private XPTable.Models.TextColumn textColumn2;
        private XPTable.Models.TextColumn textColumn3;
        private XPTable.Models.ColumnModel columnModel1;
    }
}

