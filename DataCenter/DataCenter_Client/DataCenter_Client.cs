using System;
using System.Windows.Forms;
using System.Data;
using System.Drawing;

namespace DataCenter_Client
{
    public partial class DataCenter_Client : Form
    {
        public static bool IsOnline = true;

        public DataCenter_Client()
        {
            InitializeComponent();
        }

        //SELECT [ID]
        //      ,[ServerName]
        //      ,[IP]
        //      ,[Area]
        //      ,[Content]
        //      ,[Category]
        //      ,[CreateTime]
        //FROM [Fx_Main].[dbo].[Fx_ServerInfo]
        //[ID]  [ServerName]    [IP]            [Area]      [Content]                                                                                                                                                                               [Category]      [CreateTime]
        //1	    麦迪服	        61.152.108.155	51WAN	    C:\----OS----20002----13469----67|D:\----Site----80003----58950----73|E:\----DataBase----80003----52511----65|F:\----DBbak----85007----65250----76|G:\----Tools----13444----12146----90	1	            2010-02-14 07:30:53.343
        private void Form1_Load(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                button1.Visible = true;
                button2.Visible = false;
            }
        }

        #region 数据库连接字符串 Connection_Fx_Main()
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        /// <returns></returns>
        private static string Connection_Fx_Main()
        {
            if (IsOnline)
                return @"Data Source=222.73.57.140,2149\SQL2005;Initial Catalog=Fx_Main;Persist Security Info=True;User ID=Fx_Admin;Password=qweqwe123";
            else
                return @"Data Source=127.0.0.1,2149\SQL2005;Initial Catalog=Fx_Main;Persist Security Info=True;User ID=Fx_Admin;Password=qweqwe123";
        }
        #endregion 数据库连接字符串

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(tabControl1.TabCount.ToString());
            //MessageBox.Show(tabControl1.SelectedIndex.ToString());
            listView_DiskInfo.Items.Clear();
            string strSQL = "SELECT * FROM Fx_ServerInfo";
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable(Connection_Fx_Main(), CommandType.Text, strSQL);

                foreach (DataRow dr in dt.Rows)
                {
                    ListViewItem li = new ListViewItem();
                    li.SubItems.Clear();
                    li.SubItems[0].Text = dr["ServerName"].ToString();
                    li.SubItems.Add(dr["Area"].ToString());
                    li.SubItems.Add(dr["IP"].ToString());
                    string[] arrContentHandle = dr["Content"].ToString().Replace("|", "----").Replace("----", "-").Split('-');
                    try
                    {
                        li.UseItemStyleForSubItems = false;
                        if (ContentHandle(arrContentHandle[4]) > 90) //硬盘容量大小 90
                        {
                            li.SubItems.Add(arrContentHandle[1] + ": " + ContentHandle(arrContentHandle[4]) + "%", Color.Red, Color.Yellow, Font);
                        }
                        else
                        {
                            li.SubItems.Add(arrContentHandle[1] + ": " + ContentHandle(arrContentHandle[4]) + "%");
                        }

                        if (ContentHandle(arrContentHandle[9]) > 90) //硬盘容量大小 90
                        {
                            li.SubItems.Add(arrContentHandle[6] + ": " + ContentHandle(arrContentHandle[9]) + "%", Color.Red, Color.Yellow, Font);
                        }
                        else
                        {
                            li.SubItems.Add(arrContentHandle[6] + ": " + ContentHandle(arrContentHandle[9]) + "%");
                        }

                        if (ContentHandle(arrContentHandle[14]) > 90) //硬盘容量大小 90
                        {
                            li.SubItems.Add(arrContentHandle[11] + ": " + ContentHandle(arrContentHandle[14]) + "%", Color.Red, Color.Yellow, Font);
                        }
                        else
                        {
                            li.SubItems.Add(arrContentHandle[11] + ": " + ContentHandle(arrContentHandle[14]) + "%");
                        }

                        if (ContentHandle(arrContentHandle[19]) > 90) //硬盘容量大小 90
                        {
                            li.SubItems.Add(arrContentHandle[16] + ": " + ContentHandle(arrContentHandle[19]) + "%", Color.Red, Color.Yellow, Font);
                        }
                        else
                        {
                            li.SubItems.Add(arrContentHandle[16] + ": " + ContentHandle(arrContentHandle[19]) + "%");
                        }

                        if (ContentHandle(arrContentHandle[24]) > 95) //硬盘容量大小 95
                        {
                            li.SubItems.Add(arrContentHandle[21] + ": " + ContentHandle(arrContentHandle[24]) + "%", Color.Red, Color.Yellow, Font);
                        }
                        else
                        {
                            li.SubItems.Add(arrContentHandle[21] + ": " + ContentHandle(arrContentHandle[24]) + "%");
                        }

                        //li.SubItems.Add(ContentHandle(arrContentHandle[9]) + "%");
                        //li.SubItems.Add(ContentHandle(arrContentHandle[14]) + "%");
                        //li.SubItems.Add(ContentHandle(arrContentHandle[19]) + "%");
                        //li.SubItems.Add(ContentHandle(arrContentHandle[24]) + "%");
                    }
                    catch
                    {
                        li.SubItems.Add("");
                    }
                    li.SubItems.Add(dr["CreateTime"].ToString());
                    listView_DiskInfo.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region 硬盘容量信息内容处理 ContentHandle(string strContent)
        /// <summary>
        /// 硬盘容量信息内容处理
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        private static int ContentHandle(string strContent)
        {
            int intContent = 0;
            try
            {
                intContent = 100 - Convert.ToInt32(strContent);
            }
            catch
            {
                intContent = 0;
            }
            return intContent;
        }
        #endregion

        private void TabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            //MessageBox.Show("You are in the TabControl.SelectedIndexChanged event.");
            if (tabControl1.SelectedIndex == 0)
            {
                button1.Visible = true;
                button2.Visible = false;
            }

            if (tabControl1.SelectedIndex == 1)
            {
                button1.Visible = false;
                button2.Visible = true;
            }
        }
    }
}
