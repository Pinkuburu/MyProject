using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;

namespace Baidu_Zhidao
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        #region 显示系统日志 ShowSysLog(string strLog)
        /// <summary>
        /// 显示系统日志
        /// </summary>
        /// <param name="strLog"></param>        
        private void ShowSysLog(string strLog)
        {
            DateTime dt_1 = DateTime.Now;
            textBox_Log.Text += dt_1 + "  " + strLog + "\r\n";
            textBox_Log.SelectionStart = textBox_Log.Text.Length;
            textBox_Log.ScrollToCaret();
            //始终显示TextBox最新一行，始终滚动到最底部
            textBox_Log.SelectionStart = textBox_Log.Text.Length;
            textBox_Log.ScrollToCaret();
        }
        #endregion

        #region 时间戳 Timestamp()
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        private long Timestamp()
        {
            long longTimestamp = 0;
            longTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return longTimestamp;
        }
        #endregion 时间戳 Timestamp()

        #region ADSL拔号模式 ADSL_Dial(string strADSLusername, string strADSLpassword)
        /// <summary>
        /// ADSL拔号模式
        /// </summary>
        /// <param name="strADSLusername"></param>
        /// <param name="strADSLpassword"></param>
        private void ADSL_Dial(string strADSLusername, string strADSLpassword)
        {
            Dial AutoDial = new Dial();
            ShowSysLog("断开ADSL连接...");
            AutoDial.Disconnect("ADSL");
            Thread.Sleep(5000);
            ShowSysLog("ADSL连接断开...");

            ShowSysLog("ADSL开始拔号...");
            AutoDial.Connect("ADSL", strADSLusername, strADSLpassword);
            ShowSysLog("ADSL登录完成...");
        }
        #endregion ADSL拔号模式

        private void button_Run_Click(object sender, EventArgs e)
        {
            bool Status;
            if (button_Run.Text == "开始")
            {
                button_Run.Text = "停止";
                Status = true;
            }
            else
            {
                button_Run.Text = "开始";
                Status = false;
            }
            

            if (textBox_ADSLuser.Text.Trim() == "" || textBox_ADSLpwd.Text.Trim() == "")
            {
                Thread thread = new Thread(new ThreadStart(Start));
                thread.IsBackground = true;
                while (Status)
                {
                    ADSL_Dial(textBox_ADSLuser.Text.Trim(), textBox_ADSLpwd.Text.Trim());
                    thread.Start();
                }

            }
            else
            {
                ShowSysLog("ADSL帐号或密码不能为空");
                Status = false;
            }
        }

        private void button_AddURL_Click(object sender, EventArgs e)
        {
            ListViewItem li_URL = new ListViewItem();
            li_URL.Text = textBox1_AddURL.Text.Trim();
            listView_URL.Items.Add(li_URL);
        }

        private void button_DelURL_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView_URL.SelectedItems)
            {
                li.Remove();
            }
        }

        private void Start()
        {
            string qid = null;
            string bid = null;
            string bun = null;
            string strContent = null;

            try
            {
                foreach (ListViewItem li in listView_URL.Items)
                {
                    WebClient HTTPproc = new WebClient();
                    if (listView_URL.Items.Count > 0)
                    {
                        qid = Regex.Match(li.Text, @"\d{5,}").Value;
                        strContent = HTTPproc.OpenRead(li.Text);
                        if (strContent.IndexOf("最佳答案") > -1)
                        {
                            try
                            {
                                bid = Regex.Match(strContent, @"userId"" value=""\d{3,}").Value.Replace("userId\" value=\"", "");
                                bun = Regex.Match(strContent, "userNameEncode\" value=\".*\"").Value.Replace("userNameEncode\" value=\"", "").Replace("\"", "");
                                strContent = HTTPproc.OpenRead("http://zhidao.baidu.com/q?ct=19&tn=ikasyndatajson", "ct=19&tn=ikasyndatajson&cm=100003&at=10002&qid=" + qid + "&bid=" + bid + "&bun=" + bun + "&t=" + Timestamp());
                                if (strContent == "1")
                                {
                                    ShowSysLog(qid + ".html 成功");
                                }
                                else
                                {
                                    ShowSysLog(qid + ".html 失败");
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                // Syntax error in the regular expression
                            }
                        }
                        else
                        {
                            ShowSysLog(qid + ".html");
                        }
                    }
                    else
                    {
                        ShowSysLog("没有要顶票的内容");
                        break;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
        }
    }
}
