namespace GodEyes
{
    using _36JI_;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Forms;

    public class Form1 : Form, GUserInterface
    {
        private ColumnHeader columnHeader0;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader10;
        private ColumnHeader columnHeader11;
        private ColumnHeader columnHeader12;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private ColumnHeader columnHeader9;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ListView m_ArmyList;
        private bool m_bArmySorted;
        private bool m_bExpress;
        private bool m_bNeedreLogin;
        private bool m_bSetImage;
        private Button m_BtnLogin;
        private Button m_btnSearch;
        private TextBox m_CityInfo;
        private ListView m_CityManorList;
        private TextBox m_edtSearch;
        private TextBox m_edtValidateCode;
        private TreeView m_NationTree;
        private TabPage m_PageArmy;
        private TabPage m_PageInfo;
        private TabPage m_PageManor;
        private TextBox m_PassWord;
        private PictureBox m_PicValidateCode;
        private CityTreeNode m_RightClickCityNode;
        private TextBox m_SubUser;
        private TabControl m_Tab;
        private int m_Tick;
        private Button m_UpdateMap;
        private GUser m_User;
        private TextBox m_UserName;
        private string m_UserServer;
        private Label m_WorkMessage;

        public Form1()
        {
            this.InitializeComponent();
        }

        private void ArmyList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if ((e.Column == 3) && !this.m_bArmySorted)
            {
                if (this.m_ArmyList.Items.Count > 1)
                {
                    this.m_ArmyList.BeginUpdate();
                    ArrayList list = new ArrayList();
                    int count = this.m_ArmyList.Items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        list.Add(this.m_ArmyList.Items[i]);
                    }
                    list.Sort(0, list.Count, new ArmySortComparer());
                    this.m_ArmyList.Items.Clear();
                    for (int j = 0; j < count; j++)
                    {
                        this.m_ArmyList.Items.Add((ArmyListViewItem) list[j]);
                    }
                    this.m_ArmyList.EndUpdate();
                }
                this.m_bArmySorted = true;
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string text = this.m_UserName.Text;
            string passWord = this.m_PassWord.Text;
            if (this.m_bNeedreLogin)
            {
                this.m_PicValidateCode.Visible = false;
                this.m_edtValidateCode.Visible = false;
                this.m_bNeedreLogin = false;
                this.m_BtnLogin.Text = "登录";
                if (!string.IsNullOrEmpty(this.m_SubUser.Text))
                {
                    text = this.m_SubUser.Text;
                }
                this.m_User.InitUser(text, passWord, this.m_UserServer, this.m_edtValidateCode.Text);
                this.m_NationTree.Nodes.Clear();
                this.m_Tab.TabPages.Remove(this.m_PageArmy);
                this.m_Tab.TabPages.Remove(this.m_PageManor);
                return;
            }
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(passWord))
            {
                MessageBox.Show("用户名和密码不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if (!this.m_User.Logined || (DialogResult.Cancel != MessageBox.Show("已经登陆一个用户,重新登陆?", "问题", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)))
            {
                string str3;
                try
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint remoteEP = new IPEndPoint(new IPAddress(new byte[] { 0xde, 0xd3, 0x5d, 0x80 }), 0x3f3);
                    socket.Connect(remoteEP);
                    string s = string.Format("godeyes-login:{0}", text);
                    byte[] bytes = Encoding.UTF8.GetBytes(s);
                    socket.Send(bytes);
                    byte[] buffer = new byte[0x3e8];
                    int count = socket.Receive(buffer);
                    str3 = Encoding.UTF8.GetString(buffer, 0, count);
                    s = "godeyes-close";
                    bytes = Encoding.UTF8.GetBytes(s);
                    socket.Send(bytes);
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (SocketException)
                {
                    MessageBox.Show("网络连接错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                catch (ObjectDisposedException)
                {
                    MessageBox.Show("网络连接错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                str3 = "36jisvr4.webgame.xunlei.com,10015:1";
                string[] strArray = str3.Split(new char[] { ':' });
                this.m_UserServer = strArray[0];
                int num2 = int.Parse(strArray[1]);
                if (string.Compare(this.m_UserServer, "0") != 0)
                {
                    string userServer;
                    switch (num2)
                    {
                        case -1:
                            MessageBox.Show("用户已过期", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;

                        case 0:
                            this.m_bExpress = true;
                            this.Text = "上帝之眼---试用用户";
                            goto Label_025F;
                    }
                    this.m_bExpress = false;
                    if (this.m_UserServer.IndexOf("17173.com") != -1)
                    {
                        userServer = this.m_UserServer.Split(new char[] { ',' })[0];
                    }
                    else if (this.m_UserServer.IndexOf("xunlei.com") != -1)
                    {
                        userServer = this.m_UserServer.Split(new char[] { ',' })[0];
                    }
                    else
                    {
                        userServer = this.m_UserServer;
                    }
                    this.Text = string.Format("上帝之眼--{0}---用户授权还能使用{1}天", userServer, num2);
                    goto Label_025F;
                }
                MessageBox.Show("用户没有注册", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return;
        Label_025F:
            if (!string.IsNullOrEmpty(this.m_SubUser.Text))
            {
                text = this.m_SubUser.Text;
            }
            if (this.IsNeedValidateCode(this.m_UserServer))
            {
                this.m_PicValidateCode.Visible = true;
                this.m_edtValidateCode.Visible = true;
                this.m_BtnLogin.Text = "验证登录";
                if (this.m_bSetImage)
                {
                    this.m_PicValidateCode.Image.Dispose();
                    this.m_PicValidateCode.Image = null;
                }
                this.m_PicValidateCode.Image = this.m_User.GetValidateCodeStream(this.m_UserServer);
                this.m_bSetImage = true;
                this.m_bNeedreLogin = true;
                this.m_edtValidateCode.Focus();
            }
            else
            {
                this.m_PicValidateCode.Visible = false;
                this.m_edtValidateCode.Visible = false;
                this.m_User.InitUser(text, passWord, this.m_UserServer, "");
                this.m_NationTree.Nodes.Clear();
                this.m_Tab.TabPages.Remove(this.m_PageArmy);
                this.m_Tab.TabPages.Remove(this.m_PageManor);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!this.m_User.Logined)
            {
                MessageBox.Show("请先登录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (this.m_NationTree.Nodes.Count == 0)
            {
                MessageBox.Show("城市节点为空,请先更新地图", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string text = this.m_edtSearch.Text;
                if (string.IsNullOrEmpty(text))
                {
                    MessageBox.Show("请输入搜索内容", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    bool bName = true;
                    text = text.Trim();
                    if (char.IsNumber(text[0]))
                    {
                        string[] strArray = text.Split(new char[] { ' ', ',' });
                        int num = int.Parse(strArray[0]) + 1;
                        int num2 = int.Parse(strArray[1]) + 1;
                        text = string.Format("{0:d2}0{1:d2}", num, num2);
                        bName = false;
                    }
                    CityTreeNode node = null;
                    foreach (TreeNode node2 in this.m_NationTree.Nodes)
                    {
                        foreach (TreeNode node3 in node2.Nodes)
                        {
                            CityTreeNode node4 = node3 as CityTreeNode;
                            if (node4.City.IsThisCity(text, bName))
                            {
                                node = node4;
                                break;
                            }
                        }
                        if (node != null)
                        {
                            break;
                        }
                    }
                    if (node == null)
                    {
                        MessageBox.Show("没找到城市,可能关键字有误,或者该城市为中立城市", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        this.m_NationTree.Focus();
                    }
                    else
                    {
                        node.Checked = true;
                        this.m_NationTree.SelectedNode = node;
                        this.m_NationTree.Focus();
                    }
                }
            }
        }

        public void CityStatusUpdated()
        {
            base.BeginInvoke(new dltUpdate(this.UpdateCityStatus));
        }

        public void CityUpdated()
        {
            base.BeginInvoke(new dltUpdate(this.UpdateCityInfo));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void edtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.btnSearch_Click(sender, new EventArgs());
            }
        }

        public void ForceUpdateCity_Click(object sender, EventArgs e)
        {
            if (this.m_RightClickCityNode != null)
            {
                this.m_RightClickCityNode.Checked = true;
                this.m_RightClickCityNode.City.ScanArmy = false;
                if (this.m_NationTree.SelectedNode == this.m_RightClickCityNode)
                {
                    this.m_User.UpdateCity(this.m_RightClickCityNode.City, false);
                }
                else
                {
                    this.m_NationTree.SelectedNode = this.m_RightClickCityNode;
                }
                this.m_RightClickCityNode = null;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.m_NationTree.Height = base.ClientSize.Height - 0x43;
            this.m_Tab.Width = base.ClientSize.Width - 0xc0;
            this.m_Tab.Height = base.ClientSize.Height - 0x43;
            this.m_WorkMessage.Text = "";
            this.m_Tab.TabPages.Remove(this.m_PageArmy);
            this.m_Tab.TabPages.Remove(this.m_PageManor);
            this.m_User = new GUser(this);
            this.m_bExpress = true;
            this.m_Tick = 0;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(Form1));
            this.m_NationTree = new TreeView();
            this.m_Tab = new TabControl();
            this.m_PageInfo = new TabPage();
            this.m_CityInfo = new TextBox();
            this.m_PageArmy = new TabPage();
            this.m_ArmyList = new ListView();
            this.columnHeader0 = new ColumnHeader();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            this.columnHeader4 = new ColumnHeader();
            this.columnHeader5 = new ColumnHeader();
            this.columnHeader6 = new ColumnHeader();
            this.columnHeader7 = new ColumnHeader();
            this.columnHeader8 = new ColumnHeader();
            this.columnHeader9 = new ColumnHeader();
            this.m_PageManor = new TabPage();
            this.m_CityManorList = new ListView();
            this.columnHeader10 = new ColumnHeader();
            this.columnHeader11 = new ColumnHeader();
            this.columnHeader12 = new ColumnHeader();
            this.m_BtnLogin = new Button();
            this.m_UserName = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.m_PassWord = new TextBox();
            this.m_WorkMessage = new Label();
            this.m_UpdateMap = new Button();
            this.label3 = new Label();
            this.m_edtSearch = new TextBox();
            this.m_btnSearch = new Button();
            this.m_SubUser = new TextBox();
            this.label4 = new Label();
            this.m_PicValidateCode = new PictureBox();
            this.m_edtValidateCode = new TextBox();
            this.m_Tab.SuspendLayout();
            this.m_PageInfo.SuspendLayout();
            this.m_PageArmy.SuspendLayout();
            this.m_PageManor.SuspendLayout();
            ((ISupportInitialize) this.m_PicValidateCode).BeginInit();
            base.SuspendLayout();
            this.m_NationTree.HideSelection = false;
            this.m_NationTree.HotTracking = true;
            this.m_NationTree.Location = new Point(2, 70);
            this.m_NationTree.Name = "m_NationTree";
            this.m_NationTree.Size = new Size(0xaf, 0x13c);
            this.m_NationTree.TabIndex = 12;
            this.m_NationTree.AfterSelect += new TreeViewEventHandler(this.NationTree_AfterSelect);
            this.m_NationTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.NationTree_NodeMouseClick);
            this.m_Tab.Controls.Add(this.m_PageInfo);
            this.m_Tab.Controls.Add(this.m_PageArmy);
            this.m_Tab.Controls.Add(this.m_PageManor);
            this.m_Tab.Location = new Point(190, 70);
            this.m_Tab.Name = "m_Tab";
            this.m_Tab.SelectedIndex = 0;
            this.m_Tab.Size = new Size(0x1a3, 320);
            this.m_Tab.TabIndex = 13;
            this.m_PageInfo.Controls.Add(this.m_CityInfo);
            this.m_PageInfo.Location = new Point(4, 0x16);
            this.m_PageInfo.Name = "m_PageInfo";
            this.m_PageInfo.Padding = new Padding(3);
            this.m_PageInfo.Size = new Size(0x19b, 0x126);
            this.m_PageInfo.TabIndex = 0;
            this.m_PageInfo.Text = "信息";
            this.m_PageInfo.UseVisualStyleBackColor = true;
            this.m_CityInfo.Dock = DockStyle.Fill;
            this.m_CityInfo.Location = new Point(3, 3);
            this.m_CityInfo.Multiline = true;
            this.m_CityInfo.Name = "m_CityInfo";
            this.m_CityInfo.ReadOnly = true;
            this.m_CityInfo.ScrollBars = ScrollBars.Vertical;
            this.m_CityInfo.Size = new Size(0x195, 0x120);
            this.m_CityInfo.TabIndex = 0;
            this.m_PageArmy.Controls.Add(this.m_ArmyList);
            this.m_PageArmy.Location = new Point(4, 0x16);
            this.m_PageArmy.Name = "m_PageArmy";
            this.m_PageArmy.Padding = new Padding(3);
            this.m_PageArmy.Size = new Size(0x19b, 0x126);
            this.m_PageArmy.TabIndex = 1;
            this.m_PageArmy.Text = "军队列表";
            this.m_PageArmy.UseVisualStyleBackColor = true;
            this.m_ArmyList.Activation = ItemActivation.OneClick;
            this.m_ArmyList.Columns.AddRange(new ColumnHeader[] { this.columnHeader0, this.columnHeader1, this.columnHeader2, this.columnHeader3, this.columnHeader4, this.columnHeader5, this.columnHeader6, this.columnHeader7, this.columnHeader8, this.columnHeader9 });
            this.m_ArmyList.Dock = DockStyle.Fill;
            this.m_ArmyList.FullRowSelect = true;
            this.m_ArmyList.HideSelection = false;
            this.m_ArmyList.Location = new Point(3, 3);
            this.m_ArmyList.MultiSelect = false;
            this.m_ArmyList.Name = "m_ArmyList";
            this.m_ArmyList.Size = new Size(0x195, 0x120);
            this.m_ArmyList.TabIndex = 0;
            this.m_ArmyList.UseCompatibleStateImageBehavior = false;
            this.m_ArmyList.View = View.Details;
            this.m_ArmyList.ColumnClick += new ColumnClickEventHandler(this.ArmyList_ColumnClick);
            this.columnHeader0.Text = "国家";
            this.columnHeader1.Text = "玩家";
            this.columnHeader1.TextAlign = HorizontalAlignment.Center;
            this.columnHeader1.Width = 100;
            this.columnHeader2.Text = "英雄";
            this.columnHeader2.TextAlign = HorizontalAlignment.Center;
            this.columnHeader2.Width = 120;
            this.columnHeader3.Text = "总兵力";
            this.columnHeader3.TextAlign = HorizontalAlignment.Center;
            this.columnHeader4.Text = "携带粮食";
            this.columnHeader4.TextAlign = HorizontalAlignment.Center;
            this.columnHeader4.Width = 80;
            this.columnHeader5.Text = "前锋";
            this.columnHeader5.TextAlign = HorizontalAlignment.Center;
            this.columnHeader5.Width = 90;
            this.columnHeader6.Text = "前军";
            this.columnHeader6.TextAlign = HorizontalAlignment.Center;
            this.columnHeader6.Width = 90;
            this.columnHeader7.Text = "中军";
            this.columnHeader7.TextAlign = HorizontalAlignment.Center;
            this.columnHeader7.Width = 90;
            this.columnHeader8.Text = "后军";
            this.columnHeader8.TextAlign = HorizontalAlignment.Center;
            this.columnHeader8.Width = 90;
            this.columnHeader9.Text = "后勤";
            this.columnHeader9.TextAlign = HorizontalAlignment.Center;
            this.columnHeader9.Width = 90;
            this.m_PageManor.Controls.Add(this.m_CityManorList);
            this.m_PageManor.Location = new Point(4, 0x16);
            this.m_PageManor.Name = "m_PageManor";
            this.m_PageManor.Padding = new Padding(3);
            this.m_PageManor.Size = new Size(0x19b, 0x126);
            this.m_PageManor.TabIndex = 2;
            this.m_PageManor.Text = "封地列表";
            this.m_PageManor.UseVisualStyleBackColor = true;
            this.m_CityManorList.Columns.AddRange(new ColumnHeader[] { this.columnHeader10, this.columnHeader11, this.columnHeader12 });
            this.m_CityManorList.Dock = DockStyle.Fill;
            this.m_CityManorList.FullRowSelect = true;
            this.m_CityManorList.HideSelection = false;
            this.m_CityManorList.Location = new Point(3, 3);
            this.m_CityManorList.MultiSelect = false;
            this.m_CityManorList.Name = "m_CityManorList";
            this.m_CityManorList.Size = new Size(0x195, 0x120);
            this.m_CityManorList.TabIndex = 0;
            this.m_CityManorList.UseCompatibleStateImageBehavior = false;
            this.m_CityManorList.View = View.Details;
            this.columnHeader10.Text = "玩家名字";
            this.columnHeader10.Width = 80;
            this.columnHeader11.Text = "封地名称";
            this.columnHeader11.TextAlign = HorizontalAlignment.Center;
            this.columnHeader11.Width = 100;
            this.columnHeader12.Text = "建立时间";
            this.columnHeader12.TextAlign = HorizontalAlignment.Center;
            this.columnHeader12.Width = 150;
            this.m_BtnLogin.Location = new Point(0x1d8, 1);
            this.m_BtnLogin.Name = "m_BtnLogin";
            this.m_BtnLogin.Size = new Size(70, 0x1f);
            this.m_BtnLogin.TabIndex = 6;
            this.m_BtnLogin.Text = "登录";
            this.m_BtnLogin.UseVisualStyleBackColor = true;
            this.m_BtnLogin.Click += new EventHandler(this.BtnLogin_Click);
            this.m_UserName.Location = new Point(0x47, 7);
            this.m_UserName.Name = "m_UserName";
            this.m_UserName.Size = new Size(0x4a, 20);
            this.m_UserName.TabIndex = 1;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(2, 10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名称：";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x13e, 10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2b, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "密码：";
            this.m_PassWord.Location = new Point(0x16d, 7);
            this.m_PassWord.Name = "m_PassWord";
            this.m_PassWord.PasswordChar = '#';
            this.m_PassWord.Size = new Size(0x5c, 20);
            this.m_PassWord.TabIndex = 5;
            this.m_PassWord.KeyDown += new KeyEventHandler(this.PassWord_KeyDown);
            this.m_WorkMessage.AutoSize = true;
            this.m_WorkMessage.Location = new Point(0x216, 0x2d);
            this.m_WorkMessage.Name = "m_WorkMessage";
            this.m_WorkMessage.Size = new Size(0x31, 13);
            this.m_WorkMessage.TabIndex = 11;
            this.m_WorkMessage.Text = "message";
            this.m_UpdateMap.Location = new Point(560, 1);
            this.m_UpdateMap.Name = "m_UpdateMap";
            this.m_UpdateMap.Size = new Size(70, 0x1f);
            this.m_UpdateMap.TabIndex = 7;
            this.m_UpdateMap.Text = "更新地图";
            this.m_UpdateMap.UseVisualStyleBackColor = true;
            this.m_UpdateMap.Click += new EventHandler(this.UpdateMap_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(2, 0x2c);
            this.label3.Name = "label3";
            this.label3.Size = new Size(160, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "城市名称或坐标(例如10 11)：";
            this.m_edtSearch.Location = new Point(0xac, 0x29);
            this.m_edtSearch.Name = "m_edtSearch";
            this.m_edtSearch.Size = new Size(140, 20);
            this.m_edtSearch.TabIndex = 9;
            this.m_edtSearch.KeyDown += new KeyEventHandler(this.edtSearch_KeyDown);
            this.m_btnSearch.Location = new Point(320, 0x24);
            this.m_btnSearch.Name = "m_btnSearch";
            this.m_btnSearch.Size = new Size(70, 0x1f);
            this.m_btnSearch.TabIndex = 10;
            this.m_btnSearch.Text = "查找";
            this.m_btnSearch.UseVisualStyleBackColor = true;
            this.m_btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.m_SubUser.Location = new Point(0xe7, 7);
            this.m_SubUser.Name = "m_SubUser";
            this.m_SubUser.Size = new Size(0x51, 20);
            this.m_SubUser.TabIndex = 3;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x97, 10);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x4f, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "子用户名称：";
            this.m_PicValidateCode.Cursor = Cursors.Hand;
            this.m_PicValidateCode.Location = new Point(0x18c, 0x24);
            this.m_PicValidateCode.Name = "m_PicValidateCode";
            this.m_PicValidateCode.Size = new Size(0x42, 0x1f);
            this.m_PicValidateCode.TabIndex = 14;
            this.m_PicValidateCode.TabStop = false;
            this.m_PicValidateCode.Visible = false;
            this.m_PicValidateCode.Click += new EventHandler(this.PicValidateCode_Click);
            this.m_edtValidateCode.Location = new Point(0x1d4, 0x2a);
            this.m_edtValidateCode.Name = "m_edtValidateCode";
            this.m_edtValidateCode.Size = new Size(60, 20);
            this.m_edtValidateCode.TabIndex = 15;
            this.m_edtValidateCode.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x282, 0x18c);
            base.Controls.Add(this.m_edtValidateCode);
            base.Controls.Add(this.m_PicValidateCode);
            base.Controls.Add(this.m_btnSearch);
            base.Controls.Add(this.m_edtSearch);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.m_UpdateMap);
            base.Controls.Add(this.m_WorkMessage);
            base.Controls.Add(this.m_PassWord);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.m_SubUser);
            base.Controls.Add(this.m_UserName);
            base.Controls.Add(this.m_BtnLogin);
            base.Controls.Add(this.m_Tab);
            base.Controls.Add(this.m_NationTree);
            //base.Icon = (Icon) manager.GetObject("$this.Icon");
            this.MinimumSize = new Size(650, 430);
            base.Name = "Form1";
            this.Text = "上帝之眼";
            base.Load += new EventHandler(this.Form1_Load);
            this.m_Tab.ResumeLayout(false);
            this.m_PageInfo.ResumeLayout(false);
            this.m_PageInfo.PerformLayout();
            this.m_PageArmy.ResumeLayout(false);
            this.m_PageManor.ResumeLayout(false);
            ((ISupportInitialize) this.m_PicValidateCode).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private bool IsNeedValidateCode(string strServer)
        {
            bool flag = false;
            if (strServer.IndexOf("xunlei.com") != -1)
            {
                return true;
            }
            if (strServer.IndexOf("baofeng.com") != -1)
            {
                return true;
            }
            if (strServer.IndexOf("game518.com") != -1)
            {
                flag = true;
            }
            return flag;
        }

        private void NationTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.m_bExpress)
            {
                if (this.m_Tick >= 3)
                {
                    MessageBox.Show("试用用户只能查看3次信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                this.m_Tick++;
            }
            if (e.Node is NationTreeNode)
            {
                if (this.m_Tab.TabPages.Count == 3)
                {
                    this.m_Tab.TabPages.Remove(this.m_PageArmy);
                    this.m_Tab.TabPages.Remove(this.m_PageManor);
                }
                GNation nation = (e.Node as NationTreeNode).Nation;
                string str = string.Format("国家名称：{0}\r\n内部公告：{1}\r\n对外公告：{2}\r\n城池数：{3}", new object[] { nation.NationName, nation.NationBulletinInside, nation.NationBulletinOutside, nation.NationCityAmount });
                this.m_CityInfo.Text = str;
            }
            else if (e.Node is CityTreeNode)
            {
                if (this.m_Tab.TabPages.Count == 1)
                {
                    this.m_Tab.TabPages.Add(this.m_PageArmy);
                    this.m_Tab.TabPages.Add(this.m_PageManor);
                }
                this.m_Tab.SelectedTab = this.m_PageInfo;
                this.m_User.UpdateCity((e.Node as CityTreeNode).City, false);
            }
        }

        private void NationTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if ((e.Node is CityTreeNode) && (e.Button == MouseButtons.Right))
            {
                this.m_RightClickCityNode = e.Node as CityTreeNode;
            }
        }

        public void NationUpdated()
        {
            base.BeginInvoke(new dltUpdate(this.UpdateNationTree));
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (this.m_User != null)
            {
                this.m_User.Exit();
            }
            if (this.m_bSetImage)
            {
                this.m_PicValidateCode.Image.Dispose();
                this.m_PicValidateCode.Image = null;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.m_NationTree.Height = base.ClientSize.Height - 0x43;
            this.m_Tab.Width = base.ClientSize.Width - 0xc0;
            this.m_Tab.Height = base.ClientSize.Height - 0x43;
        }

        private void PassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.BtnLogin_Click(sender, new EventArgs());
            }
        }

        private void PicValidateCode_Click(object sender, EventArgs e)
        {
            if (this.m_bSetImage)
            {
                this.m_PicValidateCode.Image.Dispose();
                this.m_PicValidateCode.Image = null;
            }
            this.m_PicValidateCode.Image = this.m_User.GetValidateCodeStream(this.m_UserServer);
            this.m_bSetImage = true;
        }

        public bool ShowUseSpyWarning()
        {
            return (bool) base.Invoke(new dltShowWarning(this.ShowWarning));
        }

        private bool ShowWarning()
        {
            bool flag = false;
            if ((DialogResult.OK == MessageBox.Show("官方已经关闭了查看其他国家城市的功能,是否愿意花费38黄金看这个城市的兵力?", "问题", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)) && (DialogResult.OK == MessageBox.Show("38黄金只能看到城市总兵力和军队数目，不能看到详细军队情况，是否真的继续?", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)))
            {
                flag = true;
            }
            return flag;
        }

        private void UpdateCityInfo()
        {
            GCity workCity = this.m_User.WorkCity;
            string str = string.Format("名称：{0}\r\n内部公告：{1}\r\n面积：{2}\r\n安定：{3}\r\n城防：{4}\r\n交通：{5}\r\n民心：{6}\r\n状态：{7}\r\n\r\n", new object[] { workCity.CityName, workCity.CityBulletin, workCity.CityArea, workCity.CitySafety, workCity.CityDefense, workCity.CityTraffic, workCity.CityMorale, (workCity.CityStatus == 0) ? "正常" : "战争" });
            if (this.m_User.WorkCity.CityStatus == 0)
            {
                str = str + workCity.GetCityArmyInfo();
            }
            else
            {
                str = str + workCity.GetWarArmyInfo();
            }
            this.m_CityInfo.Text = str;
            List<GArmy> armyList = workCity.ArmyList;
            this.m_ArmyList.Items.Clear();
            this.m_bArmySorted = false;
            if (armyList != null)
            {
                foreach (GArmy army in armyList)
                {
                    ArmyListViewItem item = new ArmyListViewItem(army, this.m_User.WorkCity);
                    this.m_ArmyList.Items.Add(item);
                }
            }
            List<GManor> manorList = workCity.ManorList;
            this.m_CityManorList.Items.Clear();
            if (manorList != null)
            {
                foreach (GManor manor in manorList)
                {
                    ListViewItem item2 = new ListViewItem(manor.PlayerName);
                    item2.SubItems.Add(manor.ManorName);
                    item2.SubItems.Add(manor.CreateTime);
                    this.m_CityManorList.Items.Add(item2);
                }
            }
        }

        private void UpdateCityStatus()
        {
            (this.m_NationTree.SelectedNode as CityTreeNode).UpdateCityStatus();
        }

        private void UpdateMap_Click(object sender, EventArgs e)
        {
            if (!this.m_User.Logined)
            {
                MessageBox.Show("请先登录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.m_NationTree.Nodes.Clear();
                this.m_Tab.TabPages.Remove(this.m_PageArmy);
                this.m_Tab.TabPages.Remove(this.m_PageManor);
                this.m_User.UpdateNationAndCity();
            }
        }

        private void UpdateNationTree()
        {
            foreach (GNation nation in GNations.theNations.AllNations)
            {
                int num = this.m_NationTree.Nodes.Add(new NationTreeNode(nation));
                TreeNode node = this.m_NationTree.Nodes[num];
                foreach (GCity city in nation.AllCity)
                {
                    node.Nodes.Add(new CityTreeNode(city, this));
                }
            }
        }

        private void UpdateWorkMessage(string sText)
        {
            this.m_WorkMessage.Text = sText;
        }

        public void WorkMessageUpdated(string sText)
        {
            base.BeginInvoke(new dltUpdateWorkMessage(this.UpdateWorkMessage), new object[] { sText });
        }

        private class ArmyListViewItem : ListViewItem
        {
            // Fields
            private GArmy m_army;

            // Methods
            public ArmyListViewItem(GArmy army, GCity city)
            {
                this.m_army = army;
                base.Text = GNations.theNations.GetNation(this.m_army.NationID).NationShortName;
                base.SubItems.Add(this.m_army.PlayerName);
                string text = string.Format("{0}(等级{1})", this.m_army.HeroName, this.m_army.HeroLevel);
                base.SubItems.Add(text);
                base.SubItems.Add(this.m_army.TotalAmount.ToString());
                base.SubItems.Add(this.m_army.Food.ToString());
                base.SubItems.Add(this.GetArmyString(this.m_army.Pos1Id, this.m_army.Pos1Amount));
                base.SubItems.Add(this.GetArmyString(this.m_army.Pos2Id, this.m_army.Pos2Amount));
                base.SubItems.Add(this.GetArmyString(this.m_army.Pos3Id, this.m_army.Pos3Amount));
                base.SubItems.Add(this.GetArmyString(this.m_army.Pos4Id, this.m_army.Pos4Amount));
                base.SubItems.Add(this.GetArmyString(this.m_army.Pos5Id, this.m_army.Pos5Amount));
                if (city.CityNationId != army.NationID)
                {
                    base.BackColor = Color.Red;
                    base.ForeColor = Color.Yellow;
                }
            }

            private string GetArmyString(int id, int count)
            {
                string str = "空";
                switch (id)
                {
                    case 0x7d1:
                        return string.Format("轻步兵({0})", count);

                    case 0x7d2:
                        return string.Format("弓箭手({0})", count);

                    case 0x7d3:
                        return string.Format("轻骑兵({0})", count);

                    case 0x7d4:
                        return string.Format("重步兵({0})", count);

                    case 0x7d5:
                        return string.Format("强弓手({0})", count);

                    case 0x7d6:
                        return string.Format("重骑兵({0})", count);

                    case 0x7d7:
                        return string.Format("禁卫军({0})", count);

                    case 0x7d8:
                        return string.Format("弩骑兵({0})", count);

                    case 0x7d9:
                        return string.Format("虎豹骑({0})", count);

                    case 0x7da:
                        return string.Format("运输兵({0})", count);

                    case 0x7db:
                        return string.Format("运输车({0})", count);

                    case 0x7dc:
                        return string.Format("羽林军({0})", count);

                    case 0x7dd:
                        return string.Format("战车({0})", count);

                    case 0x7de:
                        return string.Format("帝国铁骑({0})", count);

                    case 0x7df:
                    case 0x7e0:
                    case 0x7e1:
                        return str;

                    case 0x7e2:
                        return string.Format("义军({0})", count);
                }
                return str;
            }

            // Properties
            public GArmy Army
            {
                get
                {
                    return this.m_army;
                }
            }
        }

        private class ArmySortComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                Form1.ArmyListViewItem item = (Form1.ArmyListViewItem) x;
                Form1.ArmyListViewItem item2 = (Form1.ArmyListViewItem) y;
                if (item.Army.TotalAmount > item2.Army.TotalAmount)
                {
                    return -1;
                }
                if (item.Army.TotalAmount < item2.Army.TotalAmount)
                {
                    return 1;
                }
                return 0;
            }
        }

        private class CityTreeNode : TreeNode
        {
            // Fields
            private GCity m_City;

            // Methods
            public CityTreeNode(GCity city, Form1 thisForm)
            {
                this.m_City = city;
                if (this.m_City.CityStatus == 1)
                {
                    base.BackColor = Color.Red;
                    base.ForeColor = Color.Yellow;
                }
                base.Text = city.CityName;
                ContextMenuStrip strip = new ContextMenuStrip();
                ToolStripMenuItem item = new ToolStripMenuItem("更新城市(强制)");
                item.Click += new EventHandler(thisForm.ForceUpdateCity_Click);
                strip.Items.Add(item);
                this.ContextMenuStrip = strip;
            }

            public void UpdateCityStatus()
            {
                if (this.m_City.CityStatus == 1)
                {
                    base.BackColor = Color.Red;
                    base.ForeColor = Color.Yellow;
                }
                else
                {
                    base.BackColor = Color.White;
                    base.ForeColor = Color.Black;
                }
            }

            // Properties
            public GCity City
            {
                get
                {
                    return this.m_City;
                }
            }
        }

        public delegate bool dltShowWarning();

        public delegate void dltUpdate();

        public delegate void dltUpdateWorkMessage(string sText);

        private class NationTreeNode : TreeNode
        {
            private GNation m_Nation;

            public NationTreeNode(GNation Nation)
            {
                this.m_Nation = Nation;
                base.Text = this.m_Nation.NationName;
            }

            public GNation Nation
            {
                get
                {
                    return this.m_Nation;
                }
            }
        }
    }
}

