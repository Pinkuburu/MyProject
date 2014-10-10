using System;
using System.Windows.Forms;
using System.Threading;
using XPTable.Models;
using System.Drawing;
using System.Diagnostics;

namespace ServerList
{
    public partial class Main : Form
    {
        int i = 0;
        delegate void delListview(string a, string b, string c);
        delegate void deltable(string a, int b, int c, int d);
        delListview delListview1;
        deltable deltable1;

        ListViewGroup group1 = new ListViewGroup("001");
        ListViewGroup group2 = new ListViewGroup("002");

        public Main()
        {
            InitializeComponent();
            //listView1.Columns.Add("Columns1");
            //listView1.Columns.Add("Columns2");
            //listView1.Columns.Add("Columns3");

            //delListview1 = new delListview(AddListview);
            //deltable1 = new deltable(AddRow);
            MessageBox.Show("asdfa");

            Process[] localByNameApp = Process.GetProcessesByName("ServerList");//获取程序名的所有进程
            if (localByNameApp.Length > 0)
            {
                this.Dispose();//清理所有正在使用资源
                Application.Exit();//通知所有消息泵必须终止，并且在处理了消息以后关闭所有应用程序窗口。`
                Process.GetCurrentProcess().Kill();//杀死原有进程
            }
        }

        #region table1 初始化
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            table1.BeginUpdate();

            TextColumn textServerIP = new TextColumn("IP", 100);
            ProgressBarColumn Cpu = new ProgressBarColumn("CPU", 60);
            ProgressBarColumn disk_C = new ProgressBarColumn("C:", 60);
            ProgressBarColumn disk_D = new ProgressBarColumn("D:", 60);

            table1.ColumnModel = new ColumnModel(new Column[] {textServerIP, Cpu, disk_C, disk_D});//把声明的列添加到列中
            table1.FullRowSelect = true;
            table1.EndUpdate();
        }
        #endregion table1 初始化

        private void AddRow(string a, int b, int c, int d)
        {
            Row row = new Row();
            row.Cells.Add(new Cell(a));
            row.Cells.Add(new Cell(b));
            row.Cells.Add(new Cell(c));
            row.Cells.Add(new Cell(d));
            tableModel1.Rows.Add(row);
            table1.TableModel = tableModel1;
            table1.TableModel.RowHeight = 20;
        }

        private void AddListview(string a, string b, string c)
        {
            int j = 0;
            Random rnd = new Random();
            ListViewItem lv;
            j = rnd.Next(1, 3);

            if (j == 1)
            {
                lv = new ListViewItem(new string[] { a, "b1", c }, group1);
                listView1.Items.AddRange(new ListViewItem[] { lv });
            }
            else
            {
                lv = new ListViewItem(new string[] { a, "b2", c }, group2);
                listView1.Items.AddRange(new ListViewItem[] { lv });
            }
        }

        public void test()
        {
            string j;
            for (i = 0; i < 10; i++)
            {
                if (i < 10)
                {
                    j = "0" + i;
                }
                else
                {
                    j = i.ToString();
                }
                listView1.Invoke(delListview1, new object[] { "a" + j, "b" + j, "c" + j });
                table1.Invoke(deltable1, new object[] { "Server" + j, i, i, i });
                Thread.Sleep(50);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(test));
            t.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //listView1.View = View.Details;
            listView1.Groups.Add(group1);
            listView1.Groups.Add(group2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.View = View.LargeIcon;
        }

        private void table1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point point = this.PointToClient(table1.PointToScreen(new Point(e.X, e.Y)));
                this.contextMenuStrip1.Show(this, point);
                label1.Text = e.X.ToString();
                label2.Text = e.Y.ToString();
            }              
        }

        private void button4_Click(object sender, EventArgs e)
        {            
            MessageBox.Show(table1.SelectedItems.Length.ToString());            
        }
    }
}
