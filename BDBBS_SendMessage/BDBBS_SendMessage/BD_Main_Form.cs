using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;
using TXTClass;

namespace BDBBS_SendMessage
{
    public partial class BD_Main_Form : Form
    {
        //BD_Login_Form Login_Form = new BD_Login_Form();
        txtclass txt = new txtclass();
        public int intStatus = 0;
        public ArrayList arrThreadID = new ArrayList();
        public string strCookie = null;
        public string strUserName = null;
        public string strHostUrl = "http://club.bandao.cn/";
        public Random ran = new Random();   //公用随机函数
        public DateTime dt = DateTime.Now;

        //定义HTTP请求方法
        BDBBS_SendMessage.WebClient HTTPproc = new WebClient();

        public BD_Main_Form()
        {
            InitializeComponent();
            LoadBoardList();
        }

        private void MenuItem_MainForm_Login_Click(object sender, EventArgs e)
        {
            //if (Login_Form.Visible)
            //{
            //    Login_Form.Activate();
            //}
            //else
            //{
            //    Login_Form.Show();
            //}
        }

        #region 动态加载论坛列表 LoadBoardList()
        /// <summary>
        /// 动态加载论坛列表
        /// </summary>
        private void LoadBoardList()
        {
            //设置HTTP请求默认编码
            HTTPproc.Encoding = System.Text.Encoding.Default;
            
            try
            {
                //正则数据  --- showboard.asp?boardid=130">福彩论坛
                ShowSysLog("正在载入论坛列表");
                Regex regexObj = new Regex(@"showboard\.asp\?boardid=\d*"">[\u4e00-\u9fa5,\uFF00-\uFFFF, ,0-9,\w]{1,}");
                Match matchResults = regexObj.Match(HTTPproc.OpenRead("http://club.bandao.cn/blist.html"));
                while (matchResults.Success)
                {
                    matchResults = matchResults.NextMatch();

                    string resultString_0 = null;
                    string resultString_1 = null;
                    try
                    {
                        //正则数据  --- showboard.asp?boardid=130
                        resultString_0 = Regex.Match(matchResults.ToString(), @"showboard\.asp\?boardid=\d*").Value;
                        //正则数据  --- ">福彩论坛
                        resultString_1 = Regex.Match(matchResults.ToString(), @""">[\u4e00-\u9fa5,\uFF00-\uFFFF, ,0-9,\w]{1,}").Value;
                        ListViewItem li = new ListViewItem();
                        li.SubItems.Clear();
                        li.SubItems[0].Text = resultString_1.Replace("\">", "");
                        li.SubItems.Add(resultString_0);
                        listView_MainForm_BoardList.Items.Add(li);
                    }
                    catch (ArgumentException ex)
                    {
                        // Syntax error in the regular expression
                    }
                }
                ShowSysLog("论坛列表加载完成");
                listView_MainForm_BoardList.Items[listView_MainForm_BoardList.Items.Count - 1].Remove();
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
        }
        #endregion 动态加载论坛列表 LoadBoardList()

        #region 读取论坛版块 ReadBoradList_ID(string strCookie)
        /// <summary>
        /// 读取论坛版块
        /// </summary>
        /// <param name="strCookie"></param>
        private void ReadBoradList_ID()
        {
            //设置HTTP请求默认编码
            HTTPproc.Encoding = System.Text.Encoding.Default;
            //设置HTTP请求头部Cookie内容
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookie);
            string strURL = strHostUrl + ReadBoardList();
            string strContent = HTTPproc.OpenRead(strURL);
            ShowSysLog("正在载入论坛列表ID");
            ShowSysLog(strURL);
            ShowSysLog("休息4秒钟");
            Thread.Sleep(4000);
            ThreadID_Analyzer(strURL);
        }
        #endregion 读取论坛版块 ReadBoradList_ID(string strCookie)

        #region 分析ThreadID ThreadID_Analyzer(string strContent)
        /// <summary>
        /// 分析ThreadID
        /// </summary>
        /// <param name="strContent"></param>
        private void ThreadID_Analyzer(string strContent)
        {
            int intIndex = 0;
            string[] arrInfo;
            string strBoardID = null;
            string strThreadID = null;
            //正则数据  boardid=244&id=1282613"

            //设置HTTP请求默认编码
            HTTPproc.Encoding = System.Text.Encoding.Default;
            //设置HTTP请求头部Cookie内容
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookie);
            try
            {
                strContent = HTTPproc.OpenRead(strContent);
                if (strContent.IndexOf("别点的太频繁") > 0)
                {
                    ShowSysLog("休息4秒钟");
                    Thread.Sleep(4000);
                    ReadBoradList_ID();
                }
            }
            catch
            {
                ShowSysLog("读取ThreadID超时，正在重新请求");
                ReadBoradList_ID();
            }
            int intRan = ran.Next(4000, 60000);
            ShowSysLog("休息" + intRan/1000 + "秒钟");
            Thread.Sleep(intRan);
            ShowSysLog("正在读取贴子ID");
            Regex regexObj = new Regex(@"boardid=\d*&id=\d*""");
            Match matchResults = regexObj.Match(strContent);

            arrThreadID.Clear();
            while (matchResults.Success)
            {
                matchResults = matchResults.NextMatch();
                try
                {
                    arrThreadID.Add(matchResults);
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }
            }

            if (arrThreadID.Count == 0)
            {
                ReadBoradList_ID();
            }
            else
            {
                intIndex = ran.Next(4, arrThreadID.Count - 2);
                //存储链接  boardid=244&id=1282613"
                strContent = arrThreadID[intIndex].ToString().Replace("\"", "").Trim();
                if (!CheckLog(strContent))
                {
                    ShowSysLog("贴子ID:" + strContent);
                    SaveLog(strContent);
                    //数据  --- boardid=244&id=1282613"
                    arrInfo = arrThreadID[intIndex].ToString().Split('&');
                    //数据  --- boardid=244
                    strBoardID = arrInfo[0].Replace("boardid=", "").ToString().Trim();
                    //数据  --- id=1282613
                    strThreadID = arrInfo[1].Replace("id=", "").Replace("\"", "").ToString().Trim();

                    //strContent = "路过一下的说";//"%5Bem101%5D%0D%0A%B6%F7%D6%A7%B3%D6%D2%BB%CF%C2";
                    strContent = textBox_MainForm_Content.Text.Trim();
                    if (strContent == "")
                    {
                        MessageBox.Show("发贴信息不能为空", "系统提示");
                        intStatus = 1;
                        btn_MainForm_ShowList.Text = "开始发贴";
                    }
                    else
                    {
                        PostBoardHandle(strBoardID, strThreadID, strUserName, strContent);
                    }
                }
                else
                {
                    ShowSysLog("查出重复发贴，已跳过:" + strContent + "重新请求");
                    ReadBoradList_ID();
                }
            }
        }
        #endregion 分析ThreadID ThreadID_Analyzer(string strContent)

        #region 发贴流程处理 PostBoardHandle(string strBoardID, string strThreadID, string strN_Name, string strContent)
        /// <summary>
        /// 发贴流程处理
        /// </summary>
        /// <param name="strBoardID">版块ID</param>
        /// <param name="strThreadID">贴子ID</param>
        /// <param name="strN_Name">发贴用户登录名</param>
        /// <param name="strContent">发贴内容</param>
        /// <param name="strCookie">Cookie</param>
        private void PostBoardHandle(string strBoardID, string strThreadID, string strN_Name, string strContent)
        {
            //boardid=199&
            //threadid=1570614&     15864702776
            //userlogin=1&
            //n_name=Test002&
            //th=0&
            //content=%5Bem101%5D%0D%0A%B6%F7%D6%A7%B3%D6%D2%BB%CF%C2&
            //isrtimg=&
            //ubb=&
            //em=&
            //imgb=&
            //imgs=

            ShowSysLog("进入发贴流程:boardid=" + strBoardID + "&id=" + strThreadID);
            //设置HTTP请求默认编码
            HTTPproc.Encoding = System.Text.Encoding.Default;
            //设置HTTP请求头部Cookie内容
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookie);
            HTTPproc.RequestHeaders.Add("Referer:" + strHostUrl + "showthread.asp?boardid=" + strBoardID + "&id=" + strThreadID);
            //http://club.bandao.cn/showthread.asp?boardid=199&id=1570614

            //http://club.bandao.cn/newreplyins.asp  发贴页 POST

            //发送POST发贴请求
            try
            {
                HTTPproc.OpenRead("http://club.bandao.cn/newreplyins.asp", "boardid=" + strBoardID + "&threadid=" + strThreadID + "&userlogin=1&n_name=" + strN_Name + "&th=0&content=" + strContent + "&isrtimg=&ubb=&em=&imgb=&imgs=");
                ShowSysLog("发贴完成");
                RefreshUserInfo();
                ShowSysLog("刷新用户信息完成");
                Timer_T();
            }
            catch
            {
                MessageBox.Show("服务器请求超时！", "系统提示");
            }
        }
        #endregion

        #region 随机读取论坛列表 ReadBoardList()
        /// <summary>
        /// 随机读取论坛列表
        /// </summary>
        /// <returns>showboard.asp?boardid=244</returns>
        private string ReadBoardList()
        {
            string strBoardLink = null;
            if (listView_MainForm_SendBoardList.Items.Count == 0)
            {
                int RandKey = ran.Next(0, listView_MainForm_BoardList.Items.Count);
                strBoardLink = listView_MainForm_BoardList.Items[RandKey].SubItems[1].Text.Trim();
            }
            else
            {
                int RandKey = ran.Next(0, listView_MainForm_SendBoardList.Items.Count);
                strBoardLink = listView_MainForm_SendBoardList.Items[RandKey].SubItems[1].Text.Trim();
            }
            return strBoardLink;
        }
        #endregion 随机读取论坛列表 ReadBoardList()

        #region ListView相关操作

        #region 添加 listView_MainForm_BoardList 到 listView_MainForm_SendBoardList
        private void btn_MainForm_AddBoard_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView_MainForm_BoardList.SelectedItems)
            {
                if (listView_MainForm_BoardList.SelectedIndices.Count > 0)
                {
                    ListViewItem li5 = new ListViewItem();
                    li5.SubItems.Clear();
                    li5.SubItems[0].Text = li.SubItems[0].Text;
                    li5.SubItems.Add(li.SubItems[1].Text);
                    listView_MainForm_SendBoardList.Items.Add(li5);                    
                    li.Remove();
                }
                else
                {
                    break;
                }
            }
        }
        #endregion 添加 listView_MainForm_BoardList 到 listView_MainForm_SendBoardList

        #region 全选 listView_MainForm_BoardList
        private void btn_MainForm_AllSelectBoard_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView_MainForm_BoardList.Items)
            {
                if (li.Selected == false)
                {
                    li.Selected = true;
                    li.BackColor = Color.CadetBlue;
                    li.ForeColor = Color.White;
                }
            }
        }
        #endregion 全选 listView_MainForm_BoardList

        #region 全选 listView_MainForm_SendBoardList
        private void btn_MainForm_AllSelectSendBoard_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView_MainForm_SendBoardList.Items)
            {
                if (li.Selected == false)
                {
                    li.Selected = true;
                    li.BackColor = Color.CadetBlue;
                    li.ForeColor = Color.White;
                }
            }
        }
        #endregion 全选 listView_MainForm_SendBoardList

        #region 添加 listView_MainForm_SendBoardList 到 listView_MainForm_BoardList
        private void btn_MainForm_RemoveBoard_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView_MainForm_SendBoardList.SelectedItems)
            {
                if (listView_MainForm_SendBoardList.SelectedIndices.Count > 0)
                {
                    ListViewItem li4 = new ListViewItem();
                    li4.SubItems.Clear();
                    li4.SubItems[0].Text = li.SubItems[0].Text;
                    li4.SubItems.Add(li.SubItems[1].Text);
                    listView_MainForm_BoardList.Items.Add(li4);
                    li.Remove();
                }
                else
                {
                    break;
                }
            }
        }
        #endregion 添加 listView_MainForm_SendBoardList 到 listView_MainForm_BoardList

        #region 双击鼠标添加 listView_MainForm_BoardList 到 listView_MainForm_SendBoardList
        private void listView_MainForm_BoardList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem li in listView_MainForm_BoardList.SelectedItems)
            {
                if (listView_MainForm_BoardList.SelectedIndices.Count > 0)
                {
                    ListViewItem li5 = new ListViewItem();
                    li5.SubItems.Clear();
                    li5.SubItems[0].Text = li.SubItems[0].Text;
                    li5.SubItems.Add(li.SubItems[1].Text);
                    listView_MainForm_SendBoardList.Items.Add(li5);
                    li.Remove();
                }
                else
                {
                    break;
                }
            }
        }
        #endregion 双击鼠标添加 listView_MainForm_BoardList 到 listView_MainForm_SendBoardList

        #region 双击鼠标添加 listView_MainForm_SendBoardList 到 listView_MainForm_BoardList
        private void listView_MainForm_SendBoardList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem li in listView_MainForm_SendBoardList.SelectedItems)
            {
                if (listView_MainForm_SendBoardList.SelectedIndices.Count > 0)
                {
                    ListViewItem li4 = new ListViewItem();
                    li4.SubItems.Clear();
                    li4.SubItems[0].Text = li.SubItems[0].Text;
                    li4.SubItems.Add(li.SubItems[1].Text);
                    listView_MainForm_BoardList.Items.Add(li4);
                    li.Remove();
                }
                else
                {
                    break;
                }
            }
        }
        #endregion 双击鼠标添加 listView_MainForm_SendBoardList 到 listView_MainForm_BoardList

        #endregion ListView相关操作

        #region 显示系统日志 ShowSysLog(string strLog)
        /// <summary>
        /// 显示系统日志
        /// </summary>
        /// <param name="strLog"></param>        
        private void ShowSysLog(string strLog)
        {
            textBox_MainForm_SysLog.Text += strLog + "\r\n";
            textBox_MainForm_SysLog.SelectionStart = textBox_MainForm_SysLog.Text.Length;
            textBox_MainForm_SysLog.ScrollToCaret();
            //始终显示TextBox最新一行，始终滚动到最底部
            textBox_MainForm_SysLog.SelectionStart = textBox_MainForm_SysLog.Text.Length;
            textBox_MainForm_SysLog.ScrollToCaret(); 
        }
        #endregion 显示系统日志 ShowSysLog(string strLog)

        #region 查询日查文件是否有重复行 CheckLog(string strLog)
        /// <summary>
        /// 查询日查文件是否有重复行
        /// </summary>
        /// <param name="strLog"></param>
        /// <returns></returns>
        private bool CheckLog(string strLog)
        {
            string[] arrContent = strLog.Split('&');
            bool i = Convert.ToBoolean(txt.txtRead("Log" + dt.ToString("yyyy-MM-dd") + ".txt", '&', "c0='" + arrContent[0].ToString().Trim() + "' and c1='" + arrContent[1].ToString().Trim() + "'").Rows.Count);
            return i;
        }
        #endregion 查询日查文件是否有重复行 CheckLog(string strLog)

        #region 写入日志文件 SaveLog(string strLog)
        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="strLog"></param>
        private void SaveLog(string strLog)
        {
            txt.txtWrite("Log" + dt.ToString("yyyy-MM-dd") + ".txt", strLog);
        }
        #endregion 写入日志文件 SaveLog(string strLog)

        #region 刷新用户信息 RefreshUserInfo()
        /// <summary>
        /// 刷新用户信息
        /// </summary>
        private void RefreshUserInfo()
        {
            string strContent = null;
            try
            {
                //设置HTTP请求默认编码
                HTTPproc.Encoding = System.Text.Encoding.Default;
                //设置HTTP请求头部Cookie内容
                HTTPproc.RequestHeaders.Add("Cookie:" + strCookie);
                strContent = HTTPproc.OpenRead("http://club.bandao.cn");
            }
            catch
            {
                MessageBox.Show("服务器请求超时！", "系统提示");
            }

            string resultString = null;
            try
            {
                resultString = Regex.Match(strContent, "欢迎 <font color=\"FF9900\">.[a-z,0-9,A-Z]*").Value.Replace(" <font color=\"FF9900\">", ": ") + "\n";
                resultString += Regex.Match(strContent, "积　分:.[0-9]*").Value + "\n";
                resultString += Regex.Match(strContent, "主题帖:.[0-9]*-.[0-9]*").Value + "\n";
                resultString += Regex.Match(strContent, "回　帖:.[0-9]*-.[0-9]*").Value + "\n";
                resultString += Regex.Match(strContent, "本周积分上升:.[0-9]*").Value + "\n";
                resultString += Regex.Match(strContent, "社区总排名: 第.[0-9]*位").Value;
                label_MainForm_UserInfo.Text = resultString;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion 刷新用户信息 RefreshUserInfo()

        #region 发贴按钮
        private void btn_MainForm_ShowList_Click(object sender, EventArgs e)
        {
            if (btn_MainForm_ShowList.Text == "开始发贴")
            {
                intStatus = 0;
                btn_MainForm_ShowList.Text = "停止发贴";
                Timer_T();
                //SaveSelected_BoardList();
            }
            else
            {
                intStatus = 1;
                btn_MainForm_ShowList.Text = "开始发贴";
                MessageBox.Show("发贴停止！", "系统提示");
            }
        }
        #endregion 发贴按钮

        private void SaveSelected_BoardList()
        {
            string[] arrBoardList_Class;

            ArrayList arrClass = new ArrayList();
            string[] arrString = { "城市聊吧,showboard.asp?boardid=124", "半岛文坛,showboard.asp?boardid=132", "半岛诗坛,showboard.asp?boardid=201", "诗词古韵,showboard.asp?boardid=142", "闲闲书话,showboard.asp?boardid=133" };
            //foreach (ListViewItem li in listView_MainForm_SendBoardList.Items)
            //{
            //    if (listView_MainForm_SendBoardList.Items.Count > 0)
            //    {
            //        arrClass.Add(li.SubItems[0].Text + "," + li.SubItems[1].Text);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //MessageBox.Show(arrClass.ToString());
            foreach(string strString in arrString)
            {
                arrBoardList_Class = strString.Split(',');
                ListViewItem li = new ListViewItem();
                li.SubItems.Clear();
                li.SubItems[0].Text = arrBoardList_Class[0];
                li.SubItems.Add(arrBoardList_Class[1]);
                listView_MainForm_SendBoardList.Items.Add(li);
                //listView_MainForm_SendBoardList.Items.Add(li).BackColor = Color.CadetBlue;
                //listView_MainForm_SendBoardList.Items.Add(li).ForeColor = Color.White;
                //li.Selected = true;
                //li.BackColor = Color.CadetBlue;
                //li.ForeColor = Color.White;
            }

            foreach (ListViewItem li in listView_MainForm_SendBoardList.Items)
            {
                if (listView_MainForm_SendBoardList.Items.Count > 0)
                {
                    ListViewItem li1 = new ListViewItem();
                    li1.SubItems.Clear();
                    li1.SubItems[0].Text = li.SubItems[0].Text;
                    li1.SubItems.Add(li.SubItems[1].Text);
                    listView_MainForm_BoardList.Items.Remove(li1);
                }
                else
                {
                    break;
                }
            }
        }

        private void Timer_T()
        {
            System.Timers.Timer t = new System.Timers.Timer();//实例化Timer类，设置间隔时间为10000毫秒；
            t.Interval = 1 * 1000;
            t.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);//到达时间的时候执行事件； 
            t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            if (intStatus == 0)
            {                
                ReadBoradList_ID();
            }
            else
            {
                intStatus = 0;
            }
        }
    }
}