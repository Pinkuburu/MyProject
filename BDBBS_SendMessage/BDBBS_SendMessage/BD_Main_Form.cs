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
        public Random ran = new Random();   //�����������
        public DateTime dt = DateTime.Now;

        //����HTTP���󷽷�
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

        #region ��̬������̳�б� LoadBoardList()
        /// <summary>
        /// ��̬������̳�б�
        /// </summary>
        private void LoadBoardList()
        {
            //����HTTP����Ĭ�ϱ���
            HTTPproc.Encoding = System.Text.Encoding.Default;
            
            try
            {
                //��������  --- showboard.asp?boardid=130">������̳
                ShowSysLog("����������̳�б�");
                Regex regexObj = new Regex(@"showboard\.asp\?boardid=\d*"">[\u4e00-\u9fa5,\uFF00-\uFFFF, ,0-9,\w]{1,}");
                Match matchResults = regexObj.Match(HTTPproc.OpenRead("http://club.bandao.cn/blist.html"));
                while (matchResults.Success)
                {
                    matchResults = matchResults.NextMatch();

                    string resultString_0 = null;
                    string resultString_1 = null;
                    try
                    {
                        //��������  --- showboard.asp?boardid=130
                        resultString_0 = Regex.Match(matchResults.ToString(), @"showboard\.asp\?boardid=\d*").Value;
                        //��������  --- ">������̳
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
                ShowSysLog("��̳�б�������");
                listView_MainForm_BoardList.Items[listView_MainForm_BoardList.Items.Count - 1].Remove();
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
        }
        #endregion ��̬������̳�б� LoadBoardList()

        #region ��ȡ��̳��� ReadBoradList_ID(string strCookie)
        /// <summary>
        /// ��ȡ��̳���
        /// </summary>
        /// <param name="strCookie"></param>
        private void ReadBoradList_ID()
        {
            //����HTTP����Ĭ�ϱ���
            HTTPproc.Encoding = System.Text.Encoding.Default;
            //����HTTP����ͷ��Cookie����
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookie);
            string strURL = strHostUrl + ReadBoardList();
            string strContent = HTTPproc.OpenRead(strURL);
            ShowSysLog("����������̳�б�ID");
            ShowSysLog(strURL);
            ShowSysLog("��Ϣ4����");
            Thread.Sleep(4000);
            ThreadID_Analyzer(strURL);
        }
        #endregion ��ȡ��̳��� ReadBoradList_ID(string strCookie)

        #region ����ThreadID ThreadID_Analyzer(string strContent)
        /// <summary>
        /// ����ThreadID
        /// </summary>
        /// <param name="strContent"></param>
        private void ThreadID_Analyzer(string strContent)
        {
            int intIndex = 0;
            string[] arrInfo;
            string strBoardID = null;
            string strThreadID = null;
            //��������  boardid=244&id=1282613"

            //����HTTP����Ĭ�ϱ���
            HTTPproc.Encoding = System.Text.Encoding.Default;
            //����HTTP����ͷ��Cookie����
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookie);
            try
            {
                strContent = HTTPproc.OpenRead(strContent);
                if (strContent.IndexOf("����̫Ƶ��") > 0)
                {
                    ShowSysLog("��Ϣ4����");
                    Thread.Sleep(4000);
                    ReadBoradList_ID();
                }
            }
            catch
            {
                ShowSysLog("��ȡThreadID��ʱ��������������");
                ReadBoradList_ID();
            }
            int intRan = ran.Next(4000, 60000);
            ShowSysLog("��Ϣ" + intRan/1000 + "����");
            Thread.Sleep(intRan);
            ShowSysLog("���ڶ�ȡ����ID");
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
                //�洢����  boardid=244&id=1282613"
                strContent = arrThreadID[intIndex].ToString().Replace("\"", "").Trim();
                if (!CheckLog(strContent))
                {
                    ShowSysLog("����ID:" + strContent);
                    SaveLog(strContent);
                    //����  --- boardid=244&id=1282613"
                    arrInfo = arrThreadID[intIndex].ToString().Split('&');
                    //����  --- boardid=244
                    strBoardID = arrInfo[0].Replace("boardid=", "").ToString().Trim();
                    //����  --- id=1282613
                    strThreadID = arrInfo[1].Replace("id=", "").Replace("\"", "").ToString().Trim();

                    //strContent = "·��һ�µ�˵";//"%5Bem101%5D%0D%0A%B6%F7%D6%A7%B3%D6%D2%BB%CF%C2";
                    strContent = textBox_MainForm_Content.Text.Trim();
                    if (strContent == "")
                    {
                        MessageBox.Show("������Ϣ����Ϊ��", "ϵͳ��ʾ");
                        intStatus = 1;
                        btn_MainForm_ShowList.Text = "��ʼ����";
                    }
                    else
                    {
                        PostBoardHandle(strBoardID, strThreadID, strUserName, strContent);
                    }
                }
                else
                {
                    ShowSysLog("����ظ�������������:" + strContent + "��������");
                    ReadBoradList_ID();
                }
            }
        }
        #endregion ����ThreadID ThreadID_Analyzer(string strContent)

        #region �������̴��� PostBoardHandle(string strBoardID, string strThreadID, string strN_Name, string strContent)
        /// <summary>
        /// �������̴���
        /// </summary>
        /// <param name="strBoardID">���ID</param>
        /// <param name="strThreadID">����ID</param>
        /// <param name="strN_Name">�����û���¼��</param>
        /// <param name="strContent">��������</param>
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

            ShowSysLog("���뷢������:boardid=" + strBoardID + "&id=" + strThreadID);
            //����HTTP����Ĭ�ϱ���
            HTTPproc.Encoding = System.Text.Encoding.Default;
            //����HTTP����ͷ��Cookie����
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookie);
            HTTPproc.RequestHeaders.Add("Referer:" + strHostUrl + "showthread.asp?boardid=" + strBoardID + "&id=" + strThreadID);
            //http://club.bandao.cn/showthread.asp?boardid=199&id=1570614

            //http://club.bandao.cn/newreplyins.asp  ����ҳ POST

            //����POST��������
            try
            {
                HTTPproc.OpenRead("http://club.bandao.cn/newreplyins.asp", "boardid=" + strBoardID + "&threadid=" + strThreadID + "&userlogin=1&n_name=" + strN_Name + "&th=0&content=" + strContent + "&isrtimg=&ubb=&em=&imgb=&imgs=");
                ShowSysLog("�������");
                RefreshUserInfo();
                ShowSysLog("ˢ���û���Ϣ���");
                Timer_T();
            }
            catch
            {
                MessageBox.Show("����������ʱ��", "ϵͳ��ʾ");
            }
        }
        #endregion

        #region �����ȡ��̳�б� ReadBoardList()
        /// <summary>
        /// �����ȡ��̳�б�
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
        #endregion �����ȡ��̳�б� ReadBoardList()

        #region ListView��ز���

        #region ��� listView_MainForm_BoardList �� listView_MainForm_SendBoardList
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
        #endregion ��� listView_MainForm_BoardList �� listView_MainForm_SendBoardList

        #region ȫѡ listView_MainForm_BoardList
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
        #endregion ȫѡ listView_MainForm_BoardList

        #region ȫѡ listView_MainForm_SendBoardList
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
        #endregion ȫѡ listView_MainForm_SendBoardList

        #region ��� listView_MainForm_SendBoardList �� listView_MainForm_BoardList
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
        #endregion ��� listView_MainForm_SendBoardList �� listView_MainForm_BoardList

        #region ˫�������� listView_MainForm_BoardList �� listView_MainForm_SendBoardList
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
        #endregion ˫�������� listView_MainForm_BoardList �� listView_MainForm_SendBoardList

        #region ˫�������� listView_MainForm_SendBoardList �� listView_MainForm_BoardList
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
        #endregion ˫�������� listView_MainForm_SendBoardList �� listView_MainForm_BoardList

        #endregion ListView��ز���

        #region ��ʾϵͳ��־ ShowSysLog(string strLog)
        /// <summary>
        /// ��ʾϵͳ��־
        /// </summary>
        /// <param name="strLog"></param>        
        private void ShowSysLog(string strLog)
        {
            textBox_MainForm_SysLog.Text += strLog + "\r\n";
            textBox_MainForm_SysLog.SelectionStart = textBox_MainForm_SysLog.Text.Length;
            textBox_MainForm_SysLog.ScrollToCaret();
            //ʼ����ʾTextBox����һ�У�ʼ�չ�������ײ�
            textBox_MainForm_SysLog.SelectionStart = textBox_MainForm_SysLog.Text.Length;
            textBox_MainForm_SysLog.ScrollToCaret(); 
        }
        #endregion ��ʾϵͳ��־ ShowSysLog(string strLog)

        #region ��ѯ�ղ��ļ��Ƿ����ظ��� CheckLog(string strLog)
        /// <summary>
        /// ��ѯ�ղ��ļ��Ƿ����ظ���
        /// </summary>
        /// <param name="strLog"></param>
        /// <returns></returns>
        private bool CheckLog(string strLog)
        {
            string[] arrContent = strLog.Split('&');
            bool i = Convert.ToBoolean(txt.txtRead("Log" + dt.ToString("yyyy-MM-dd") + ".txt", '&', "c0='" + arrContent[0].ToString().Trim() + "' and c1='" + arrContent[1].ToString().Trim() + "'").Rows.Count);
            return i;
        }
        #endregion ��ѯ�ղ��ļ��Ƿ����ظ��� CheckLog(string strLog)

        #region д����־�ļ� SaveLog(string strLog)
        /// <summary>
        /// д����־�ļ�
        /// </summary>
        /// <param name="strLog"></param>
        private void SaveLog(string strLog)
        {
            txt.txtWrite("Log" + dt.ToString("yyyy-MM-dd") + ".txt", strLog);
        }
        #endregion д����־�ļ� SaveLog(string strLog)

        #region ˢ���û���Ϣ RefreshUserInfo()
        /// <summary>
        /// ˢ���û���Ϣ
        /// </summary>
        private void RefreshUserInfo()
        {
            string strContent = null;
            try
            {
                //����HTTP����Ĭ�ϱ���
                HTTPproc.Encoding = System.Text.Encoding.Default;
                //����HTTP����ͷ��Cookie����
                HTTPproc.RequestHeaders.Add("Cookie:" + strCookie);
                strContent = HTTPproc.OpenRead("http://club.bandao.cn");
            }
            catch
            {
                MessageBox.Show("����������ʱ��", "ϵͳ��ʾ");
            }

            string resultString = null;
            try
            {
                resultString = Regex.Match(strContent, "��ӭ <font color=\"FF9900\">.[a-z,0-9,A-Z]*").Value.Replace(" <font color=\"FF9900\">", ": ") + "\n";
                resultString += Regex.Match(strContent, "������:.[0-9]*").Value + "\n";
                resultString += Regex.Match(strContent, "������:.[0-9]*-.[0-9]*").Value + "\n";
                resultString += Regex.Match(strContent, "�ء���:.[0-9]*-.[0-9]*").Value + "\n";
                resultString += Regex.Match(strContent, "���ܻ�������:.[0-9]*").Value + "\n";
                resultString += Regex.Match(strContent, "����������: ��.[0-9]*λ").Value;
                label_MainForm_UserInfo.Text = resultString;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion ˢ���û���Ϣ RefreshUserInfo()

        #region ������ť
        private void btn_MainForm_ShowList_Click(object sender, EventArgs e)
        {
            if (btn_MainForm_ShowList.Text == "��ʼ����")
            {
                intStatus = 0;
                btn_MainForm_ShowList.Text = "ֹͣ����";
                Timer_T();
                //SaveSelected_BoardList();
            }
            else
            {
                intStatus = 1;
                btn_MainForm_ShowList.Text = "��ʼ����";
                MessageBox.Show("����ֹͣ��", "ϵͳ��ʾ");
            }
        }
        #endregion ������ť

        private void SaveSelected_BoardList()
        {
            string[] arrBoardList_Class;

            ArrayList arrClass = new ArrayList();
            string[] arrString = { "�����İ�,showboard.asp?boardid=124", "�뵺��̳,showboard.asp?boardid=132", "�뵺ʫ̳,showboard.asp?boardid=201", "ʫ�ʹ���,showboard.asp?boardid=142", "�����黰,showboard.asp?boardid=133" };
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
            System.Timers.Timer t = new System.Timers.Timer();//ʵ����Timer�࣬���ü��ʱ��Ϊ10000���룻
            t.Interval = 1 * 1000;
            t.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);//����ʱ���ʱ��ִ���¼��� 
            t.AutoReset = false;//������ִ��һ�Σ�false������һֱִ��(true)��
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