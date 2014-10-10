using System.Windows.Forms;
using EXControls;
using System.Threading;

namespace ListView_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            exListView1.Columns.Add(new EXEditableColumnHeader("Movie", 20));
            exListView1.Columns.Add(new EXColumnHeader("Progress", 120));
            exListView1.Columns.Add(new EXColumnHeader("Status", 80));

            for (int i = 0; i < 100; i++)
            {
                //movie
                EXListViewItem item = new EXListViewItem(i.ToString());
                EXControlListViewSubItem cs = new EXControlListViewSubItem();
                ProgressBar b = new ProgressBar();
                b.Tag = item;
                b.Minimum = 0;
                b.Maximum = 100;
                b.Value = 50;
                item.SubItems.Add(cs);
                exListView1.AddControlToSubItem(b, cs);
                EXControlListViewSubItem ss = new EXControlListViewSubItem();
                Label aaa = new Label();
                aaa.Text = "asdf";
                item.SubItems.Add(ss);
                exListView1.AddControlToSubItem(aaa, ss);
                exListView1.Items.Add(item);
            }
        }
    }
}
