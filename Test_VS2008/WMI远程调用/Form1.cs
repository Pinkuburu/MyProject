using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;

namespace WMI远程调用
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            long mb = 1048576;
            //1024x1024
            //设定生成的WMI所需的所有设置
            System.Management.ConnectionOptions Conn = new ConnectionOptions();
            //设定用于WMI连接操作的用户名
            Conn.Username = textBox2.Text;
            //设定用户的口令
            Conn.Password = textBox3.Text;
            //设定用于执行WMI操作的范围
            ManagementScope Ms = new ManagementScope("\\\\" + textBox1.Text + "\\root\\cimv2", Conn);
            try
            {
                //连接到实际操作的WMI范围
                Ms.Connect();
                //设定通过WMI要查询的内容
                ObjectQuery Query = new ObjectQuery("select FreeSpace ,Size ,Name from Win32_LogicalDisk where DriveType=3");
                //WQL语句，设定的WMI查询内容和WMI的操作范围，检索WMI对象集合
                ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Ms, Query);
                //异步调用WMI查询
                ManagementObjectCollection ReturnCollection = Searcher.Get();
                double free = 0;
                double use = 0;
                double total = 0;
                listBox1.Items.Clear();
                //通过对产生的WMI的实例集合进行检索，获得硬盘信息
                foreach (ManagementObject Return in ReturnCollection)
                {
                    listBox1.Items.Add("磁盘名称：" +  Return["Name"].ToString());
                    //获得硬盘的可用空间
                    free = Convert.ToInt64(Return["FreeSpace"]) / mb;
                    //获得硬盘的已用空间
                    use = (Convert.ToInt64(Return["Size"]) - Convert.ToInt64(Return["FreeSpace"])) / mb;
                    //获得硬盘的合计空间
                    total = Convert.ToInt64(Return["Size"]) / mb;
                    listBox1.Items.Add(" 总计：" + total.ToString() + "MB");
                    listBox1.Items.Add("已用空间：" + use.ToString() + "MB");
                    listBox1.Items.Add("可用空间：" + free.ToString() + "MB");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("连接" + textBox1.Text + "出错，出错信息为：" + ee.Message, "出现错误！");
            }
        }
    }
}
