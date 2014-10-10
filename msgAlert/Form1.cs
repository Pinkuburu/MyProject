using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using DataProviders.Data;
namespace msgAlert
{
	/// <summary>
	/// Form1 的摘要说明。
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel3;
		private System.Windows.Forms.ListBox lbx_event;
		private System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox tbx_to;
		private System.Windows.Forms.TextBox tbx_smtpserver;
		private System.Windows.Forms.TextBox tbx_smtp_user;
		private System.Windows.Forms.TextBox tbx_smtp_pwd;
		private System.Windows.Forms.TextBox tbx_from;
		private System.Windows.Forms.ListBox lbx_iplist;
		private System.Windows.Forms.TextBox tbx_ip;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.RadioButton rbt_ora;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.RadioButton rbt_sql;
		private System.Windows.Forms.RadioButton rbt_acc;
		private System.Windows.Forms.TextBox tbx_dbora;
		private System.Windows.Forms.TextBox tbx_dbsql;
		private System.Windows.Forms.TextBox tbx_dbacc;
		private System.Windows.Forms.CheckBox ckb_esmtp;
		private System.Windows.Forms.CheckBox ckb_mail;
		private System.Windows.Forms.CheckBox ckb_net;
		private System.ComponentModel.IContainer components;

		private string type="";
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox ckb_sound;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.OpenFileDialog openfdl;
		private System.Windows.Forms.TextBox tbx_sound;
		private System.Windows.Forms.ToolBarButton tbb_start;
		private System.Windows.Forms.ToolBarButton tbb_stop;
		private System.Windows.Forms.ToolBarButton tbb_exit;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.TextBox tbx_time;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private string connstr="";
		private System.Windows.Forms.Button btt_testsound;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.Timer timer2;
		private System.Threading.Thread myTh;
		private System.Threading.Thread myThico;
		private System.Windows.Forms.StatusBarPanel statusBarPanel2;
		private bool ico=true;
		private Icon icon3;
		private Icon icon4;
		private Icon icon5;
		private Icon icon6;
		private System.Windows.Forms.TextBox txt_sms;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.CheckBox ckb_sms;
		private int iconum=0;

		public Form1()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//

			icon3=Icon.FromHandle((new Bitmap(imageList1.Images[3])).GetHicon());
			icon4=Icon.FromHandle((new Bitmap(imageList1.Images[4])).GetHicon());
			icon5=Icon.FromHandle((new Bitmap(imageList1.Images[5])).GetHicon());
			icon6=Icon.FromHandle((new Bitmap(imageList1.Images[6])).GetHicon());
			#region 数据库连接类型和连接判断
			connTodb();
			#endregion

            #region 启动窗体时读取数据库常用信息并显示在窗体上
			try
			{
				using ( IDataProviderBase dp=DataProvider.Instance(type,connstr))
				{
					DataSet ds=dp.ReturnDataSet("select MAILADDR,SMTPSERVER,SMTPUSER,SMTPPWD,MAILFROM,ISAUTHLOGIN,ISMAIL,IPADDR,ISNET,SOUNDPATH,ISSOUND,sms_add,issms from wyw_msg_conf","msg_conf");
					DataTable dt=ds.Tables["msg_conf"];
					DataRow dr=dt.Rows[0];
					this.tbx_to.Text=dr["MAILADDR"].ToString();
					this.tbx_smtpserver.Text=dr["SMTPSERVER"].ToString();
					this.tbx_smtp_user.Text=dr["SMTPUSER"].ToString();
					this.tbx_smtp_pwd.Text=dr["SMTPPWD"].ToString();
					this.tbx_from.Text=dr["MAILFROM"].ToString();
					this.tbx_sound.Text=dr["SOUNDPATH"].ToString();
					this.txt_sms.Text=dr["sms_add"].ToString();
					this.ckb_esmtp.Checked=dr["ISAUTHLOGIN"].ToString()=="1"?true:false;
					this.ckb_mail.Checked=dr["ISMAIL"].ToString()=="1"?true:false;
					this.ckb_net.Checked=dr["ISNET"].ToString()=="1"?true:false;
					this.ckb_sound.Checked=dr["ISSOUND"].ToString()=="1"?true:false;
					this.ckb_sms.Checked=dr["issms"].ToString()=="1"?true:false;

					string[] strip=dr["IPADDR"].ToString().Split(new char[]{','});

					for(int i=0;i<strip.Length;i++)
						if(strip[i]!=""&&strip[i]!=null)
							lbx_iplist.Items.Add(strip[i]);
					string sqlmsg="select id as id,title as 标题,content as 内容,to_char(time,'yyyy-mm-dd hh24:mi:ss') as 时间,decode(issend,0,'未发送',1,'已发送') as 是否发送,nvl(notice,' ') as 备注 from wyw_msg order by id desc";
					DataSet dsmsg=dp.ReturnDataSet(sqlmsg,"dgd_msg");
					dataGrid1.DataSource=dsmsg.Tables["dgd_msg"];
				}
			}
			catch
			{
				MessageBox.Show("数据库无法连接，请配置数据库!");
			}
			#endregion

		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbx_event = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ckb_net = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tbx_ip = new System.Windows.Forms.TextBox();
            this.lbx_iplist = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckb_mail = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ckb_esmtp = new System.Windows.Forms.CheckBox();
            this.tbx_from = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbx_smtp_pwd = new System.Windows.Forms.TextBox();
            this.tbx_smtp_user = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbx_smtpserver = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbx_to = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.txt_sms = new System.Windows.Forms.TextBox();
            this.ckb_sms = new System.Windows.Forms.CheckBox();
            this.button8 = new System.Windows.Forms.Button();
            this.ckb_sound = new System.Windows.Forms.CheckBox();
            this.btt_testsound = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.tbx_sound = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbx_time = new System.Windows.Forms.TextBox();
            this.rbt_acc = new System.Windows.Forms.RadioButton();
            this.rbt_sql = new System.Windows.Forms.RadioButton();
            this.tbx_dbacc = new System.Windows.Forms.TextBox();
            this.tbx_dbsql = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.rbt_ora = new System.Windows.Forms.RadioButton();
            this.tbx_dbora = new System.Windows.Forms.TextBox();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanel3 = new System.Windows.Forms.StatusBarPanel();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.tbb_start = new System.Windows.Forms.ToolBarButton();
            this.tbb_stop = new System.Windows.Forms.ToolBarButton();
            this.tbb_exit = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.openfdl = new System.Windows.Forms.OpenFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel3)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(0, 40);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(616, 352);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lbx_event);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(608, 327);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "事件";
            // 
            // lbx_event
            // 
            this.lbx_event.ItemHeight = 12;
            this.lbx_event.Location = new System.Drawing.Point(0, 8);
            this.lbx_event.Name = "lbx_event";
            this.lbx_event.Size = new System.Drawing.Size(600, 376);
            this.lbx_event.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(608, 327);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "设置";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ckb_net);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.tbx_ip);
            this.groupBox2.Controls.Add(this.lbx_iplist);
            this.groupBox2.Location = new System.Drawing.Point(8, 152);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(296, 176);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "发送网络消息告警";
            // 
            // ckb_net
            // 
            this.ckb_net.Location = new System.Drawing.Point(144, 112);
            this.ckb_net.Name = "ckb_net";
            this.ckb_net.Size = new System.Drawing.Size(104, 24);
            this.ckb_net.TabIndex = 5;
            this.ckb_net.Text = "启用消息通知";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(144, 136);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 4;
            this.button5.Text = "保存配置";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(144, 80);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "测试消息";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(144, 48);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "添加ip";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tbx_ip
            // 
            this.tbx_ip.Location = new System.Drawing.Point(136, 16);
            this.tbx_ip.Name = "tbx_ip";
            this.tbx_ip.Size = new System.Drawing.Size(144, 21);
            this.tbx_ip.TabIndex = 1;
            this.tbx_ip.Text = "10.230.6.";
            // 
            // lbx_iplist
            // 
            this.lbx_iplist.ItemHeight = 12;
            this.lbx_iplist.Location = new System.Drawing.Point(8, 16);
            this.lbx_iplist.Name = "lbx_iplist";
            this.lbx_iplist.Size = new System.Drawing.Size(120, 148);
            this.lbx_iplist.TabIndex = 0;
            this.lbx_iplist.DoubleClick += new System.EventHandler(this.lbx_iplist_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckb_mail);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.ckb_esmtp);
            this.groupBox1.Controls.Add(this.tbx_from);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbx_smtp_pwd);
            this.groupBox1.Controls.Add(this.tbx_smtp_user);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbx_smtpserver);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbx_to);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(592, 136);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "发送邮件设置";
            // 
            // ckb_mail
            // 
            this.ckb_mail.Location = new System.Drawing.Point(472, 80);
            this.ckb_mail.Name = "ckb_mail";
            this.ckb_mail.Size = new System.Drawing.Size(104, 24);
            this.ckb_mail.TabIndex = 13;
            this.ckb_mail.Text = "启用邮件通知";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(480, 104);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "保存配置";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(480, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "测试邮件";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ckb_esmtp
            // 
            this.ckb_esmtp.Location = new System.Drawing.Point(472, 24);
            this.ckb_esmtp.Name = "ckb_esmtp";
            this.ckb_esmtp.Size = new System.Drawing.Size(104, 24);
            this.ckb_esmtp.TabIndex = 10;
            this.ckb_esmtp.Text = "Esmtp认证";
            this.ckb_esmtp.CheckedChanged += new System.EventHandler(this.ckb_esmtp_CheckedChanged);
            // 
            // tbx_from
            // 
            this.tbx_from.Location = new System.Drawing.Point(320, 104);
            this.tbx_from.Name = "tbx_from";
            this.tbx_from.Size = new System.Drawing.Size(144, 21);
            this.tbx_from.TabIndex = 9;
            this.tbx_from.Text = "wenshao@126.com";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(288, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 23);
            this.label5.TabIndex = 8;
            this.label5.Text = "from";
            // 
            // tbx_smtp_pwd
            // 
            this.tbx_smtp_pwd.Enabled = false;
            this.tbx_smtp_pwd.Location = new System.Drawing.Point(320, 72);
            this.tbx_smtp_pwd.Name = "tbx_smtp_pwd";
            this.tbx_smtp_pwd.PasswordChar = '*';
            this.tbx_smtp_pwd.Size = new System.Drawing.Size(144, 21);
            this.tbx_smtp_pwd.TabIndex = 7;
            this.tbx_smtp_pwd.Text = "pwd";
            // 
            // tbx_smtp_user
            // 
            this.tbx_smtp_user.Enabled = false;
            this.tbx_smtp_user.Location = new System.Drawing.Point(320, 48);
            this.tbx_smtp_user.Name = "tbx_smtp_user";
            this.tbx_smtp_user.Size = new System.Drawing.Size(144, 21);
            this.tbx_smtp_user.TabIndex = 6;
            this.tbx_smtp_user.Text = "wenshao";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(288, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "pwd";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(288, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "name";
            // 
            // tbx_smtpserver
            // 
            this.tbx_smtpserver.Location = new System.Drawing.Point(320, 24);
            this.tbx_smtpserver.Name = "tbx_smtpserver";
            this.tbx_smtpserver.Size = new System.Drawing.Size(144, 21);
            this.tbx_smtpserver.TabIndex = 3;
            this.tbx_smtpserver.Text = "10.230.2.37";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(288, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Smtp";
            // 
            // tbx_to
            // 
            this.tbx_to.Location = new System.Drawing.Point(24, 24);
            this.tbx_to.Multiline = true;
            this.tbx_to.Name = "tbx_to";
            this.tbx_to.Size = new System.Drawing.Size(248, 96);
            this.tbx_to.TabIndex = 1;
            this.tbx_to.Text = "wyw308@126.com";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 116);
            this.label1.TabIndex = 0;
            this.label1.Text = "收件人,分割";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button7);
            this.groupBox3.Controls.Add(this.txt_sms);
            this.groupBox3.Controls.Add(this.ckb_sms);
            this.groupBox3.Controls.Add(this.button8);
            this.groupBox3.Controls.Add(this.ckb_sound);
            this.groupBox3.Controls.Add(this.btt_testsound);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.button6);
            this.groupBox3.Controls.Add(this.tbx_sound);
            this.groupBox3.Location = new System.Drawing.Point(328, 152);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(272, 176);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "声音告警设置";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(16, 152);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 8;
            this.button7.Text = "保存手机号";
            this.button7.Click += new System.EventHandler(this.button7_Click_1);
            // 
            // txt_sms
            // 
            this.txt_sms.Location = new System.Drawing.Point(16, 128);
            this.txt_sms.Name = "txt_sms";
            this.txt_sms.Size = new System.Drawing.Size(232, 21);
            this.txt_sms.TabIndex = 7;
            this.txt_sms.Text = "1395600000,138000000";
            // 
            // ckb_sms
            // 
            this.ckb_sms.Location = new System.Drawing.Point(144, 96);
            this.ckb_sms.Name = "ckb_sms";
            this.ckb_sms.Size = new System.Drawing.Size(104, 24);
            this.ckb_sms.TabIndex = 6;
            this.ckb_sms.Text = "启用手机告警";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(192, 64);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 5;
            this.button8.Text = "保存配置";
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // ckb_sound
            // 
            this.ckb_sound.Location = new System.Drawing.Point(16, 96);
            this.ckb_sound.Name = "ckb_sound";
            this.ckb_sound.Size = new System.Drawing.Size(104, 24);
            this.ckb_sound.TabIndex = 4;
            this.ckb_sound.Text = "启用声音告警";
            // 
            // btt_testsound
            // 
            this.btt_testsound.Location = new System.Drawing.Point(104, 64);
            this.btt_testsound.Name = "btt_testsound";
            this.btt_testsound.Size = new System.Drawing.Size(75, 23);
            this.btt_testsound.TabIndex = 3;
            this.btt_testsound.Text = "播放测试";
            this.btt_testsound.Click += new System.EventHandler(this.button7_Click);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 16);
            this.label9.TabIndex = 2;
            this.label9.Text = "路径：";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(8, 64);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 1;
            this.button6.Text = "选择声音文件";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // tbx_sound
            // 
            this.tbx_sound.Location = new System.Drawing.Point(56, 32);
            this.tbx_sound.Name = "tbx_sound";
            this.tbx_sound.Size = new System.Drawing.Size(200, 21);
            this.tbx_sound.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGrid1);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(608, 327);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "消息列表";
            // 
            // dataGrid1
            // 
            this.dataGrid1.CaptionText = "历史告警事件";
            this.dataGrid1.DataMember = "";
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(0, 0);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.Size = new System.Drawing.Size(608, 320);
            this.dataGrid1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox4);
            this.tabPage4.Location = new System.Drawing.Point(4, 21);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(608, 327);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "数据库连接设置";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.tbx_time);
            this.groupBox4.Controls.Add(this.rbt_acc);
            this.groupBox4.Controls.Add(this.rbt_sql);
            this.groupBox4.Controls.Add(this.tbx_dbacc);
            this.groupBox4.Controls.Add(this.tbx_dbsql);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.rbt_ora);
            this.groupBox4.Controls.Add(this.tbx_dbora);
            this.groupBox4.Location = new System.Drawing.Point(8, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(592, 296);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "数据库连接设置";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(216, 208);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 23);
            this.label11.TabIndex = 11;
            this.label11.Text = "(分钟)";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(16, 208);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 23);
            this.label10.TabIndex = 10;
            this.label10.Text = "检测时间";
            // 
            // tbx_time
            // 
            this.tbx_time.Location = new System.Drawing.Point(96, 208);
            this.tbx_time.Name = "tbx_time";
            this.tbx_time.Size = new System.Drawing.Size(100, 21);
            this.tbx_time.TabIndex = 9;
            this.tbx_time.Text = "2";
            // 
            // rbt_acc
            // 
            this.rbt_acc.AccessibleName = "db";
            this.rbt_acc.Location = new System.Drawing.Point(424, 152);
            this.rbt_acc.Name = "rbt_acc";
            this.rbt_acc.Size = new System.Drawing.Size(104, 24);
            this.rbt_acc.TabIndex = 8;
            this.rbt_acc.Text = "使用";
            // 
            // rbt_sql
            // 
            this.rbt_sql.AccessibleName = "db";
            this.rbt_sql.Location = new System.Drawing.Point(424, 96);
            this.rbt_sql.Name = "rbt_sql";
            this.rbt_sql.Size = new System.Drawing.Size(104, 24);
            this.rbt_sql.TabIndex = 7;
            this.rbt_sql.Text = "使用";
            this.rbt_sql.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // tbx_dbacc
            // 
            this.tbx_dbacc.Location = new System.Drawing.Point(104, 152);
            this.tbx_dbacc.Name = "tbx_dbacc";
            this.tbx_dbacc.Size = new System.Drawing.Size(304, 21);
            this.tbx_dbacc.TabIndex = 6;
            this.tbx_dbacc.Text = "\\Data\\msg.mdb";
            // 
            // tbx_dbsql
            // 
            this.tbx_dbsql.Location = new System.Drawing.Point(104, 96);
            this.tbx_dbsql.Name = "tbx_dbsql";
            this.tbx_dbsql.Size = new System.Drawing.Size(304, 21);
            this.tbx_dbsql.TabIndex = 5;
            this.tbx_dbsql.Text = "Data Source=10.138.208.60;UID=sa;PWD=pwd;DATABASE=czgl";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(24, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 23);
            this.label8.TabIndex = 4;
            this.label8.Text = "access";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(32, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 23);
            this.label7.TabIndex = 3;
            this.label7.Text = "SQL";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(32, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 23);
            this.label6.TabIndex = 2;
            this.label6.Text = "Oracle";
            // 
            // rbt_ora
            // 
            this.rbt_ora.AccessibleName = "db";
            this.rbt_ora.Checked = true;
            this.rbt_ora.Location = new System.Drawing.Point(424, 40);
            this.rbt_ora.Name = "rbt_ora";
            this.rbt_ora.Size = new System.Drawing.Size(104, 24);
            this.rbt_ora.TabIndex = 1;
            this.rbt_ora.TabStop = true;
            this.rbt_ora.Text = "使用";
            // 
            // tbx_dbora
            // 
            this.tbx_dbora.Location = new System.Drawing.Point(104, 40);
            this.tbx_dbora.Name = "tbx_dbora";
            this.tbx_dbora.Size = new System.Drawing.Size(304, 21);
            this.tbx_dbora.TabIndex = 0;
            this.tbx_dbora.Text = "User ID=root;Data Source=orcl;Password=pwd";
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 413);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1,
            this.statusBarPanel2,
            this.statusBarPanel3});
            this.statusBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(616, 22);
            this.statusBar1.TabIndex = 1;
            this.statusBar1.Text = "statusBar1";
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.Icon = ((System.Drawing.Icon)(resources.GetObject("statusBarPanel1.Icon")));
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Text = "状态正常";
            this.statusBarPanel1.Width = 388;
            // 
            // statusBarPanel2
            // 
            this.statusBarPanel2.Icon = ((System.Drawing.Icon)(resources.GetObject("statusBarPanel2.Icon")));
            this.statusBarPanel2.Name = "statusBarPanel2";
            this.statusBarPanel2.Text = "statusBarPanel2";
            this.statusBarPanel2.Width = 20;
            // 
            // statusBarPanel3
            // 
            this.statusBarPanel3.Name = "statusBarPanel3";
            this.statusBarPanel3.Text = "通用消息告警程序。开发：信息班";
            this.statusBarPanel3.Width = 208;
            // 
            // toolBar1
            // 
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbb_start,
            this.tbb_stop,
            this.tbb_exit});
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.imageList1;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(616, 28);
            this.toolBar1.TabIndex = 2;
            this.toolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // tbb_start
            // 
            this.tbb_start.ImageIndex = 1;
            this.tbb_start.Name = "tbb_start";
            this.tbb_start.Text = "开始";
            // 
            // tbb_stop
            // 
            this.tbb_stop.ImageIndex = 2;
            this.tbb_stop.Name = "tbb_stop";
            this.tbb_stop.Text = "停止";
            // 
            // tbb_exit
            // 
            this.tbb_exit.ImageIndex = 0;
            this.tbb_exit.Name = "tbb_exit";
            this.tbb_exit.Text = "退出";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem4});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem3});
            this.menuItem1.Text = "文件";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 0;
            this.menuItem3.Text = "关闭";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "设置";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem5,
            this.menuItem7,
            this.menuItem6});
            this.menuItem4.Text = "帮助";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 0;
            this.menuItem5.Text = "帮助";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 1;
            this.menuItem7.Text = "-";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 2;
            this.menuItem6.Text = "关于";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(616, 435);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.toolBar1);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(624, 469);
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(624, 469);
            this.Name = "Form1";
            this.Text = "通用事件告警程序";
            this.MinimumSizeChanged += new System.EventHandler(this.Form1_MinimumSizeChanged);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.Closed += new System.EventHandler(this.Form1_Closed);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
        /// <summary>
        /// 菜单栏按钮的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e) 
		{
			try
			{
				if(e.Button==tbb_start)
				{
					myTh =new  System.Threading.Thread(new System.Threading.ThreadStart(msgSend));
					myTh.Start();	
					timer1.Start();
					timer2.Start();
					tbb_start.Enabled=false;
					tbb_stop.Enabled=true;
					notifyIcon1.Text="运行状态";
					statusBarPanel1.Text="开始运行";
				}
				if(e.Button==tbb_stop)
				{
					notifyIcon1.Text="停止状态";
					timer1.Stop();
					timer2.Stop();
					tbb_start.Enabled=true;
					tbb_stop.Enabled=false;
					notifyIcon1.Icon = icon3;
					statusBarPanel1.Text="停止运行";
				}
				if(e.Button==tbb_exit)
				{
					this.Close();
				}
			}
			catch
			{
			}
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			if(tbx_ip.Text.Trim()=="")
			{
				MessageBox.Show("请输入ip地址","错误");
				statusBarPanel1.Text="填加ip失败";
			}
			else
			{
				//插入ip地址到列表框
				if(lbx_iplist.Items.Contains(tbx_ip.Text))
					MessageBox.Show("请不要添加重复ip");
				else
				{
					lbx_iplist.Items.Add(tbx_ip.Text);
					statusBarPanel1.Text="填加ip成功";
				}
			}
		}

		private void lbx_iplist_DoubleClick(object sender, System.EventArgs e)
		{
			lbx_iplist.Items.Remove(lbx_iplist.SelectedItem);
			statusBarPanel1.Text="删除ip成功";
		}
		private void NetSendMsg(string strHost,string strMsg)
		{
			try
			{
				ProcessStartInfo psInfo=new ProcessStartInfo();
				psInfo.FileName="net.exe";
				psInfo.Arguments="send "+strHost+" "+strMsg;//net send
				psInfo.WindowStyle=ProcessWindowStyle.Hidden;
				Process pro=Process.Start(psInfo);
				
			}
			catch
			{
				statusBarPanel1.Text=DateTime.Now.ToString()+" send to "+strHost+":"+"发送失败！";
			}
		}
		private string RunCmd(string command)//调用外部命令发信息
		{
			
			Process p = new Process();
 
			//Process類有一個StartInfo屬性，這個是ProcessStartInfo類，包括了一些屬性和方法，下面我們用到了他的幾個屬性：

			p.StartInfo.FileName = "cmd.exe";           //設定程序名
			p.StartInfo.Arguments = "/c " + command;    //設定程式執行參數
			p.StartInfo.UseShellExecute = false;        //關閉Shell的使用
			p.StartInfo.RedirectStandardInput = true;   //重定向標準輸入
			p.StartInfo.RedirectStandardOutput = true;  //重定向標準輸出
			p.StartInfo.RedirectStandardError = true;   //重定向錯誤輸出
			p.StartInfo.CreateNoWindow = true;          //設置不顯示窗口
 
			p.Start();   //啟動
             
			//p.StandardInput.WriteLine(command);       //也可以用這種方式輸入要執行的命令
			//p.StandardInput.WriteLine("exit");        //不過要記得加上Exit要不然下一行程式執行的時候會當機
             
			return p.StandardOutput.ReadToEnd();        //從輸出流取得命令執行結果
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(lbx_iplist.Items.Count>0)
				{
					foreach(object ip_str in lbx_iplist.Items)
					{
						NetSendMsg(lbx_iplist.GetItemText(ip_str).Trim(),"测试消息，是否收到？");
					}
					lbx_event.Items.Add(DateTime.Now.ToString()+"：测试用net send发送消息!");
				}
				else
				{
					statusBarPanel1.Text="错误：请添加接收ip地址";
			
				}
			}
			catch
			{
				MessageBox.Show("没有发送成功，请检查网络和messsage信使服务是否开启！");
			}
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			#region 数据库连接类型和连接判断
			connTodb();
			#endregion

			if(lbx_iplist.Items.Count>0)//如果有ip地址
			{
				string ipadd="";
				foreach(object ip_str in lbx_iplist.Items) //将ip地址连起来以,隔开
				{
					ipadd+=ip_str+",";
				}
				int isnet=0;
				if(ckb_net.Checked)
					isnet=1;
				using ( IDataProviderBase dp=DataProvider.Instance(type,connstr))
				{
					string sql="update wyw_msg_conf set IPADDR='"+ipadd+"',ISNET="+isnet;
					if(dp.ExecuteSQL(sql))
						statusBarPanel1.Text="保存消息发送ip成功";
					else
						statusBarPanel1.Text="保存消息发送ip失败";
				}
			}
			else
			{
				statusBarPanel1.Text="错误：请添加接收ip地址";
			}
			
		
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			try
			{
				MailSender ms = new MailSender ();
				ms.From =tbx_from.Text;
				ms.To = tbx_to.Text;
				ms.Subject = "通用告警程序测试邮件";
				ms.Body = "这是一个通用告警程序平台，支持oracle,SQL,access等数据库类型，支持邮件、net send,声音等告警方式。开发：信息班";
				if(ckb_esmtp.Checked)		
					ms.IsAuthLogin=true;
				ms.UserName = tbx_smtp_user.Text;  
				ms.Password = tbx_smtp_pwd.Text;
				ms.Server = tbx_smtpserver.Text.Trim();
				ms.SendMail ();
				lbx_event.Items.Add(DateTime.Now.ToString()+"：测试用邮件发送消息!");
				MessageBox.Show("测试邮件发送成功，请查收！");
			}
			catch
			{
				MessageBox.Show("邮件发送错误，请检查邮件设置和服务器是否需要Esmtp认证");
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			#region 数据库连接类型和连接判断
			connTodb();
			#endregion
			string mailaddr="";
			string smtpserver="";
			string smtpuser="";
			string smtppwd="";
			string mailfrom="";
			int ismail=0;
			int isauthlogin=0;
			#region 构造sql语句
			if(tbx_to.Text==""||tbx_to.Text==null)
				MessageBox.Show("请添加接收邮件地址!");
			else
				mailaddr=tbx_to.Text;
			if(tbx_smtpserver.Text==""||tbx_smtpserver==null)
				MessageBox.Show("请添加发送邮件smtp地址!");
			else
				smtpserver=tbx_smtpserver.Text;
			if(ckb_esmtp.Checked)
			{
				if(tbx_smtp_user.Text!=""&&tbx_smtp_pwd.Text!="")
				{
					isauthlogin=1;
					smtpuser=tbx_smtp_user.Text;
					smtppwd=tbx_smtp_pwd.Text;
				}
			}
			if(tbx_from.Text!="")
				mailfrom=tbx_from.Text;
			else
				MessageBox.Show("请添加发送邮件");
			if(ckb_mail.Checked)
				ismail=1;
			#endregion
			#region 更新邮件信息
			using ( IDataProviderBase dp=DataProvider.Instance(type,connstr))
			{
				string sql="update wyw_msg_conf set MAILADDR='"+mailaddr+"',SMTPSERVER='"+smtpserver+"',SMTPUSER='"+smtpuser+"',SMTPPWD='"+smtppwd+"',MAILFROM='"+mailfrom+"',ISAUTHLOGIN="+isauthlogin+",ismail="+ismail;
				if(dp.ExecuteSQL(sql))
					statusBarPanel1.Text="保存邮件信息成功";
				else
					statusBarPanel1.Text="保存邮件信息失败";
			}
			#endregion
		}

		private void ckb_esmtp_CheckedChanged(object sender, System.EventArgs e)
		{
			tbx_smtp_user.Enabled=ckb_esmtp.Checked;
			tbx_smtp_pwd.Enabled=ckb_esmtp.Checked;
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			openfdl.Filter="wav声音文件(*.wav)|*.wav";
			openfdl.ShowDialog();
			tbx_sound.Text=openfdl.FileName;
		}

		private void button7_Click(object sender, System.EventArgs e)
		{
			try
			{
				play_sound.PlaySound.Play(tbx_sound.Text);
			}
			catch
			{
				MessageBox.Show("未知播放错误，请检查声音文件！");
			}
		}

		private void button8_Click(object sender, System.EventArgs e)
		{
			#region 数据库连接类型和连接判断
			connTodb();
			#endregion
			int issound=0;
			string soundpath="";
			if(tbx_sound.Text!="")
				soundpath=tbx_sound.Text;
			else
				MessageBox.Show("请添加声音文件地址!");
			if(ckb_sound.Checked)
				issound=1;
			using ( IDataProviderBase dp=DataProvider.Instance(type,connstr))
			{
				string sql="update wyw_msg_conf set SOUNDPATH='"+soundpath+"',ISSOUND="+issound;
				if(dp.ExecuteSQL(sql))
					statusBarPanel1.Text="保存声音配置成功";
				else
					statusBarPanel1.Text="保存声音配置失败";
			}
		}
		private void msgSend()
		{
			MailSender ms=new MailSender();
			string body="来自通用告警程序的告警邮件。内容：";
			if(ckb_mail.Checked)
			{
				ms.From =tbx_from.Text;
				ms.To = tbx_to.Text;
				if(ckb_esmtp.Checked)		
					ms.IsAuthLogin=true;
				ms.UserName = tbx_smtp_user.Text;  
				ms.Password = tbx_smtp_pwd.Text;
				ms.Server = tbx_smtpserver.Text.Trim();
			}
			
			using ( IDataProviderBase dp=DataProvider.Instance(type,connstr))
			{
				string sql="select * from wyw_msg where issend=0";
				string sqlup="";
				DataTable dt=dp.ReturnDataSet(sql,"msg").Tables["msg"];
				for(int j=0;j<dt.Rows.Count;j++)
				{
					sqlup="update wyw_msg set issend=1 where id="+dt.Rows[j]["id"];
					if(dp.ExecuteSQL(sqlup))
					{
						if(ckb_mail.Checked)//邮件发送消息
						{						
							ms.Subject="通用告警程序邮件："+dt.Rows[j]["TITLE"].ToString();
							ms.Body=body+dt.Rows[j]["content"].ToString()+" 事件发生时间："+dt.Rows[j]["time"].ToString()+"。请及时处理！";
							ms.SendMail();
							lbx_event.Items.Add(DateTime.Now.ToString()+"：邮件："+dt.Rows[j]["title"].ToString()+":"+dt.Rows[j]["content"].ToString()+":"+dt.Rows[j]["time"].ToString());
						}
						if(ckb_sms.Checked)//手机告警
						{
							string sms_text=dt.Rows[j]["time"].ToString().Trim()+dt.Rows[j]["content"].ToString().Trim();
							//sms_text=sms_text.Replace(" ","");
							string cmd=Application.StartupPath+"\\"+"sms -f 1390000000 -p pwd -t "+txt_sms.Text.Trim()+" -m \""+sms_text+"\" -a UBUNTU";
							lbx_event.Items.Add(RunCmd(cmd));
						}
						if(ckb_net.Checked)//网络发送消息
						{
							if(lbx_iplist.Items.Count>0)
							{
								foreach(object ip_str in lbx_iplist.Items)
								{
									NetSendMsg(lbx_iplist.GetItemText(ip_str).Trim(),dt.Rows[j]["time"].ToString()+" "+dt.Rows[j]["title"].ToString()+" "+dt.Rows[j]["content"].ToString());
								}
								lbx_event.Items.Add(DateTime.Now.ToString()+"：net send："+dt.Rows[j]["title"].ToString()+":"+dt.Rows[j]["content"].ToString()+":"+dt.Rows[j]["time"].ToString());
							}
							
						}
					}
						
				}
				if(dt.Rows.Count>0)//如果有告警事件，声音告警
					if(ckb_sound.Checked)
					{
						play_sound.PlaySound.Play(tbx_sound.Text);
						lbx_event.Items.Add(DateTime.Now.ToString()+"：以声音告警");
					}
			}
			statusBarPanel1.Text="检测时间："+DateTime.Now.ToString();
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			try
			{
				timer1.Interval=int.Parse(tbx_time.Text)*60000;
				myTh =new  System.Threading.Thread(new System.Threading.ThreadStart(msgSend));
				myTh.Start();	
			}
			catch
			{
				statusBarPanel1.Text="状态:定时器1故障";
			}
		}

		private void Form1_MinimumSizeChanged(object sender, System.EventArgs e)
		{
			
		}

		private void Form1_Closed(object sender, System.EventArgs e)
		{
		
		}

		private void notifyIcon1_DoubleClick(object sender, System.EventArgs e)
		{
			this.Visible=true;
			this.WindowState=FormWindowState.Normal;
		}

		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			Form2 form2=new Form2();
			form2.Show();
		}
		private void connTodb()
		{
			if(rbt_ora.Checked)
			{
				type="ORACLE";
				connstr=tbx_dbora.Text;
			}
			if(rbt_sql.Checked)
			{
				type="SQL";
				connstr=tbx_dbsql.Text;
			}
			if(rbt_acc.Checked)
			{
				type="ACCESS";
				connstr=Application.StartupPath+"\\"+tbx_dbacc.Text;
			}
		}

		private void timer2_Tick(object sender, System.EventArgs e)
		{
			try
			{
				timer2.Interval=1000;//2秒变换一次
//				if(!myThico.IsAlive)
//				{
					myThico =new  System.Threading.Thread(new System.Threading.ThreadStart(icoChange));
					myThico.Start();	
//				}
				
			}
			catch
			{
				statusBarPanel1.Text="状态:定时器故障";
			}
		}
		private void icoChange()
		{
			if(ico)
			{
				if(iconum>3)
				{
					notifyIcon1.Icon = icon3;//从imailist里取
					iconum=0;
				}
				else
				{
					iconum++;
				}
				ico=!ico;
				statusBar1.Panels[1].Icon=icon5;
			}
			else
			{
//				notifyIcon1.Icon = new System.Drawing.Icon(fpath+"\\ima\\4.ico");//手动附值
				if(iconum>3)
				{
					notifyIcon1.Icon = icon4;//从imailist里取
					iconum=0;
				}
				else
				{
					iconum++;
				}
				ico=!ico;
				statusBar1.Panels[1].Icon=icon6;
			}


		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void Form1_SizeChanged(object sender, System.EventArgs e)
		{
			if(this.WindowState==FormWindowState.Minimized)
			{
//				this.ShowInTaskbar=false;//设置窗体在任务栏不可见
				this.Hide();
				this.notifyIcon1.Visible=true;
			}			
		}

		private void button7_Click_1(object sender, System.EventArgs e)
		{
			#region 数据库连接类型和连接判断
			connTodb();
			#endregion
			int issms=0;
			string smsadd="";
			if(txt_sms.Text!="")
				smsadd=txt_sms.Text;
			else
				MessageBox.Show("请添加手机号码!");
			if(ckb_sms.Checked)
				issms=1;
			using ( IDataProviderBase dp=DataProvider.Instance(type,connstr))
			{
				string sql="update wyw_msg_conf set sms_add='"+smsadd+"',issms="+issms;
				if(dp.ExecuteSQL(sql))
					statusBarPanel1.Text="保存手机配置成功";
				else
					statusBarPanel1.Text="保存手机配置失败";
			}
		}
	}
}