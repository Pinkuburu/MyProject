using System;
using System.Windows.Forms;
using System.Threading;
using System.Text;

namespace AttendanceHelper
{
    public partial class Main : Form
    {
        public ClassWX wx = new ClassWX();
        public delegate void SendMsg(string strContent);

        public Main()
        {
            InitializeComponent();
        }
        private bool bIsConnected = false;
        public zkemkeeper.CZKEMClass axCZKEM = new zkemkeeper.CZKEMClass();

        private void btnConnect_Click(object sender, EventArgs e)
        {
            int idwErrorCode = 0;

            if (txtIP.Text.Trim() == "")
            {
                MessageBox.Show("IP cannot be null", "Error");
                return;
            }
            Cursor = Cursors.WaitCursor;

            if (btnConnect.Text == "DisConnect")
            {
                this.axCZKEM.Disconnect();
                this.axCZKEM.OnVerify -= new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM_OnVerify);

                bIsConnected = false;
                btnConnect.Text = "Connect";
                AddMsg("Current State:DisConnected");
                Cursor = Cursors.Default;
                return;
            }

            bIsConnected = this.axCZKEM.Connect_Net(txtIP.Text.Trim(), 4370);
            if (bIsConnected == true)
            {
                btnConnect.Text = "DisConnect";
                btnConnect.Refresh();
                AddMsg("Current State:Connected");

                if (axCZKEM.RegEvent(1, 65535))
                {
                    this.axCZKEM.OnVerify += new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM_OnVerify);
                }
            }
            else
            {
                axCZKEM.GetLastError(ref idwErrorCode);
                MessageBox.Show("Unable to connect the device,ErrorCode=" + idwErrorCode.ToString(), "Error");
            }
            Cursor = Cursors.Default;
        }

        void axCZKEM_OnVerify(int UserID)
        {
            AddMsg("正在验证...");
            if (UserID != -1)
            {
                AddMsg("验证成功，工号ID：" + UserID.ToString());
                if (UserID == 71)//韩志伟
                {
                    SendMsg sdMsg = new SendMsg(SendTo);
                    this.Invoke(sdMsg, new string[] {"工号:" + UserID.ToString() + " 已打卡%0A" + DateTime.Now });
                }
            }
            else
            {
                AddMsg("验证失败... ");
            }
        }

        private void SendTo(string strContent)
        {
            SendMessage(strContent);
        }

        #region 消息插入 AddMsg(string strCount)
        /// <summary>
        /// 消息插入
        /// </summary>
        /// <param name="strCount"></param>
        private void AddMsg(string strCount)
        {
            lbRTShow.Items.Add(strCount + "    " + DateTime.Now);
            Log.WriteLog(LogFile.Trace, strCount);
            int intCount = lbRTShow.Items.Count;
            Lable_Count.Text = intCount.ToString();            
            if (intCount > 1000)
            {
                lbRTShow.Items.Clear();
                Lable_Count.Text = "1";
                lbRTShow.Items.Add(strCount + "    " + DateTime.Now);
            }
        }
        #endregion 消息插入 AddMsg(string strCount)

        #region MSG SendMessage(string strContent)
        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <param name="strContent">消息内容</param>
        private void SendMessage(string strMessage)
        {
            WebClient HTTPmsg = new WebClient();
            HTTPmsg.Encoding = Encoding.UTF8;
            string strRequest = "http://qpush.me/pusher/push_site/";
            string strResponse = "name=cupid&code=733922&sig=&cache=false&msg%5Btext%5D=" + strMessage; 
            string strContent = HTTPmsg.OpenRead(strRequest, strResponse);//{"op":"r","result":"success","message":"naK8ZH"}
            if (strContent == "\"d8cc10e903918f74\"")
            {
                AddMsg("消息发送成功");
            }
            else
            {
                AddMsg("消息发送失败");
            }
        }
        #endregion

        #region URLEncode UrlEncode(string str, string encode)
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private static string UrlEncode(string str, string encode)
        {
            int factor = 0;
            if (encode == "UTF-8")
                factor = 3;
            if (encode == "GB2312")
                factor = 2;

            //不需要编码的字符
            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-";
            System.Text.Encoder encoder = System.Text.Encoding.GetEncoding(encode).GetEncoder();
            char[] c1 = str.ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //一个字符一个字符的编码

            for (int i = 0; i < c1.Length; i++)
            {
                //不需要编码

                if (okChar.IndexOf(c1[i]) > -1)
                    sb.Append(c1[i]);
                else
                {
                    byte[] c2 = new byte[factor];
                    int charUsed, byteUsed; bool completed;

                    encoder.Convert(c1, i, 1, c2, 0, factor, true, out charUsed, out byteUsed, out completed);

                    foreach (byte b in c2)
                    {
                        if (b != 0)
                            sb.AppendFormat("%{0:X}", b);
                    }
                }
            }
            return sb.ToString().Trim();
        }
        #endregion
    }
}
