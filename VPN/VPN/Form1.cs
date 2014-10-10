using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using DotRas;
using DotRas.Design;

namespace VPN {
    public partial class Form1 : Form {

        private delegate void DlgGd1FillData(Object ds);
        private DlgGd1FillData Gd1FillDataHandler = null;

        private delegate void DlgBtnPing(bool enabled);
        private DlgBtnPing BtnPingHandler = null;

        /// <summary>
        /// 当前试图连接的序数(DataGridView 中的序数), 排序后,应重设这个数字为 -1; FindNext 会对这个值加1.
        /// </summary>
        private int currTryIdx = -1;
        private VPNInfoEntity currTryVPNInfoEntity;
        private bool findFlag = true;

        private void Gd1FillData(Object ds) {
            gd1.Rows.Clear();
            gd1.DataSource = ds;
        }

        private void BtnPingEnabled(bool enabled) {
            btnPing.Enabled = enabled;
            btnFind.Enabled = enabled;
            gd1.Refresh();
        }

        public Form1() {
            InitializeComponent();
            Gd1FillDataHandler = new DlgGd1FillData(Gd1FillData);
            BtnPingHandler = new DlgBtnPing(BtnPingEnabled);
            gd1.AutoGenerateColumns = false;
        }

        private void Form1_Load(object sender, EventArgs e) {
            GetData();

            DataGridViewTextBoxColumn _area = new DataGridViewTextBoxColumn();
            _area.HeaderText = "国家/地区";
            _area.Name = "Area";
            _area.DataPropertyName = "Area";
            gd2.Columns.Add(_area);

            DataGridViewTextBoxColumn _ip = new DataGridViewTextBoxColumn();
            _ip.HeaderText = "IP";
            _ip.Name = "Ip";
            _ip.DataPropertyName = "Ip";
            gd2.Columns.Add(_ip);

            DataGridViewTextBoxColumn _user = new DataGridViewTextBoxColumn();
            _user.HeaderText = "用户名";
            _user.Name = "User";
            _user.DataPropertyName = "User";
            gd2.Columns.Add(_user);

            DataGridViewTextBoxColumn _pwd = new DataGridViewTextBoxColumn();
            _pwd.HeaderText = "密码";
            _pwd.Name = "Pwd";
            _pwd.DataPropertyName = "Pwd";
            gd2.Columns.Add(_pwd);
            gd2.AutoGenerateColumns = false;
            SetGd2DataSource();
        }

        private void SetGd2DataSource() {
            SortableBindingList<VPNInfoEntity> ls = new SortableBindingList<VPNInfoEntity>(SerializHelper.EnableList.Values.ToList<VPNInfoEntity>());
            gd2.DataSource = ls;            
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {

        }

        private void GetData() {
            Thread t = new Thread(new ThreadStart(_GetData));
            t.Start();
        }

        private void _GetData() {
            List<VPNInfoEntity> list = VpnListHelper.GetFeiYiVPNS();
            SortableBindingList<VPNInfoEntity> slist = new SortableBindingList<VPNInfoEntity>(list);
            gd1.BeginInvoke(Gd1FillDataHandler, slist);
        }

        private void button1_Click(object sender, EventArgs e) {
            Ping();
            btnPing.Enabled = false;
        }

        private void Ping() {
            Thread t = new Thread(new ThreadStart(_Ping));
            t.Start();
        }

        private void _Ping() {
            foreach (DataGridViewRow row in gd1.Rows) {
                if (!row.IsNewRow) {
                    PingHelper ph = new PingHelper();
                    ph.PingComplete += new PingHelper.DlgPingCompleteHandler(ph_PingComplete);
                    ph.PingIP(row.Cells["ip"].Value.ToString(), row.Index);
                    Thread.Sleep(20);
                }
            }
        }

        void ph_PingComplete(object sender, System.Net.NetworkInformation.PingCompletedEventArgs p, params object[] parameters) {
            int idx = 0;
            int.TryParse(parameters[0].ToString(), out idx);
            VPNInfoEntity entity = (VPNInfoEntity)gd1.Rows[idx].DataBoundItem;
            if (p.Reply.Status == System.Net.NetworkInformation.IPStatus.Success) {
                entity.Ping = p.Reply.RoundtripTime;
                entity.Available = VPNInfoEntity.AvailableStatus.Unknow;
            } else {
                entity.Ping = -1L;
                entity.Available = VPNInfoEntity.AvailableStatus.Disable;
            }

            if (idx == gd1.Rows.Count - 1)//不允许添加新行,所以最后一行不是新行.
                btnPing.BeginInvoke(BtnPingHandler, true);
        }

        private void gd1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                gd1.RowHeadersWidth - 4,
                e.RowBounds.Height);


            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                gd1.RowHeadersDefaultCellStyle.Font, rectangle,
                gd1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void btnFind_Click(object sender, EventArgs e) {
            ChangeBtnFind();
            findFlag = true;
            Find();
            btnFind.Enabled = true;
            btnPing.Enabled = false;
        }

        void btnStop_Click(object sender , EventArgs e) {
            ChangeBtnFind();
            findFlag = false;
            VPNConnectHelper.CancelDialAsync();
            btnPing.Enabled = true;
        }

        private void ChangeBtnFind() {
            if( btnFind.Tag == null || btnFind.Tag.ToString() != "stop" ) {
                btnFind.Enabled = false;
                btnFind.Tag = "stop";
                btnFind.Text = "我不找了！";
                btnFind.Click -= btnFind_Click;
                btnFind.Click += new EventHandler(btnStop_Click);
                VPNConnectHelper.DialAsyncComplete -= VPNConnectHelper_DialAsyncComplete2;
                VPNConnectHelper.DialStateChange += new VPNConnectHelper.DialStateChangeHandler(VPNConnectHelper_DialStateChange);
                VPNConnectHelper.DialAsyncComplete += new VPNConnectHelper.DialAsyncCompleteHandler(VPNConnectHelper_DialAsyncComplete);
            } else {
                btnFind.Tag = "";
                btnFind.Text = "给我找一个可用的!";
                btnFind.Click -= btnStop_Click;
                btnFind.Click += new EventHandler(btnFind_Click);
            }
        }

        private void Find() {
            Thread t = new Thread(new ThreadStart(FindNext));
            t.Start();
        }

        private void FindNext() {
            if( !findFlag )
                return;

            currTryIdx++;
            if (gd1.Rows.Count > currTryIdx && !gd1.Rows[currTryIdx].IsNewRow) {
                DataGridViewRow row = gd1.Rows[currTryIdx];
                VPNInfoEntity entity = (VPNInfoEntity)gd1.Rows[currTryIdx].DataBoundItem;
                if( entity.Ping > 0 ) {
                    currTryVPNInfoEntity = entity;
                    VPNConnectHelper.DialAsync(entity.Ip , entity.User , entity.Pwd);
                } else {
                    FindNext();
                }
            } else {
                notifyIcon1.ShowBalloonTip(5000, "...", "以搜索到列表尾!", ToolTipIcon.Info);
            }
        }

        private void gd1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if ( e.ColumnIndex > -1 && e.RowIndex > -1 && gd1.Columns[e.ColumnIndex].DataPropertyName == "Available") {
                VPNInfoEntity entity = (VPNInfoEntity)gd1.Rows[e.RowIndex].DataBoundItem;
                e.Graphics.FillRectangle(Brushes.White , e.CellBounds.Location.X , e.CellBounds.Location.Y , e.CellBounds.Width -1 , e.CellBounds.Height -1);
                Rectangle rectangle = new Rectangle(e.CellBounds.Location.X + 4 , e.CellBounds.Location.Y + 4 , 14 , 14);
                if (entity.Available == VPNInfoEntity.AvailableStatus.Disable) {
                    
                }

                switch( entity.Available ) {
                    case VPNInfoEntity.AvailableStatus.Disable:
                        e.Graphics.DrawImage(Res.Disable , rectangle);
                        break;
                    case VPNInfoEntity.AvailableStatus.Enable:
                        e.Graphics.DrawImage(Res.Enable , rectangle);
                        break;
                    default:
                        e.Graphics.DrawImage(Res.Normal , rectangle);
                        break;
                }

                e.Handled = true;
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e) {
            findFlag = false;
            VPNConnectHelper.Disconnect();
        }

        private void gd2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e) {
            VPNInfoEntity entity = (VPNInfoEntity)gd2.Rows[e.RowIndex].DataBoundItem;
            Connect(entity);
        }

        private void Connect(VPNInfoEntity entity) {
            notifyIcon1.ShowBalloonTip(3000, "...", "正在尝试连接：" + entity.Ip, ToolTipIcon.Info);
            VPNConnectHelper.DialAsyncComplete -= VPNConnectHelper_DialAsyncComplete;
            VPNConnectHelper.DialAsyncComplete += new VPNConnectHelper.DialAsyncCompleteHandler(VPNConnectHelper_DialAsyncComplete2);
            VPNConnectHelper.DialAsync(entity.Ip, entity.User, entity.Pwd);
        }

        private void gd2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
    e.RowBounds.Location.Y,
    gd1.RowHeadersWidth - 4,
    e.RowBounds.Height);


            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                gd2.RowHeadersDefaultCellStyle.Font, rectangle,
                gd2.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void cmsiConnect_Click(object sender, EventArgs e) {
            if (gd2.SelectedRows.Count > 0) {
                VPNInfoEntity entity = (VPNInfoEntity)gd2.SelectedRows[0].DataBoundItem;
                Connect(entity);
            }
        }

        private void cmsi2Drop_Click(object sender, EventArgs e) {
            if (gd2.SelectedRows.Count > 0) {
                VPNInfoEntity entity = (VPNInfoEntity)gd2.SelectedRows[0].DataBoundItem;
                SerializHelper.RemoveEnableEntity(entity);
                SetGd2DataSource();
            }
        }


        void VPNConnectHelper_DialAsyncComplete(RasDialer dialer, DialCompletedEventArgs e) {
            if (e.Connected) {
                currTryVPNInfoEntity.Available = VPNInfoEntity.AvailableStatus.Enable;
                notifyIcon1.ShowBalloonTip(5000, "...", "连接成功", ToolTipIcon.Info);
                SerializHelper.AddEnableEntity(currTryVPNInfoEntity);
                ChangeBtnFind();
            } else if (e.Error != null) {
                currTryVPNInfoEntity.Available = VPNInfoEntity.AvailableStatus.Disable;
                SerializHelper.AddDisableEntity(currTryVPNInfoEntity);
                notifyIcon1.ShowBalloonTip(3000, "...", currTryVPNInfoEntity.Ip + "\r\n" + e.Error.Message, ToolTipIcon.Info);
                FindNext();
            }
        }

        void VPNConnectHelper_DialAsyncComplete2( RasDialer dialer, DialCompletedEventArgs e ) {
            if (e.Connected) {
                notifyIcon1.ShowBalloonTip(5000, "...", "连接成功", ToolTipIcon.Info);
            } else if(e.Error != null) {
                notifyIcon1.ShowBalloonTip(3000, "...", "连接失败" + "\r\n" + e.Error.Message, ToolTipIcon.Info);
            }
        }

        void VPNConnectHelper_DialStateChange(RasDialer dialer, StateChangedEventArgs e) {

        }
    }
}
