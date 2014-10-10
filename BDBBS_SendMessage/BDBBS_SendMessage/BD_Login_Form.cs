using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace BDBBS_SendMessage
{
    public partial class BD_Login_Form : Form
    {
        #region 变量定义

        //变量定义
        public bool debug = false; //是否开启调式模式
        public string strImageCookie = "";

        #endregion 变量定义

        #region BD_Login_Form 窗体属性

        //BD_Login_Form 窗体属性
        public BD_Login_Form()
        {
            InitializeComponent();
            ImageCode();
            this.AcceptButton = button_LoginForm;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        #endregion BD_Login_Form 窗体属性

        #region 点击 pictureBox_LoginForm_ImageCode 控件切换验证码

        //点击 pictureBox_LoginForm_ImageCode 切换验证码
        private void pictureBox_LoginForm_ImageCode_Click(object sender, EventArgs e)
        {
            ImageCode();
        }

        #endregion 点击 pictureBox_LoginForm_ImageCode 控件切换验证码

        #region 登录按钮操作

        private void button_LoginForm_Click(object sender, EventArgs e)
        {
            LoginHandle();
        }

        #endregion 登录按钮操作

        #region 获取验证码图片 ImageCode()
        /// <summary>
        /// 获取验证码图片
        /// </summary>
        private void ImageCode()
        {
            //定义HTTP请求方法
            BDBBS_SendMessage.WebClient HTTPproc = new WebClient();
            //设置HTTP请求默认编码
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            //向指定网址请求返回数据流
            this.pictureBox_LoginForm_ImageCode.Image = Image.FromStream(HTTPproc.GetStream("http://club.bandao.cn/icode/imgcode2.asp", ""));
            //将得到的Cookie存储到全局变量strImageCookie
            strImageCookie = HTTPproc.Cookie.ToString();
        }
        #endregion 获取验证码图片 ImageCode()

        #region 登录方法及信息返回 LoginHandle()
        /// <summary>
        /// 登录方法及信息返回
        /// </summary>
        private void LoginHandle()
        {
            string strContent = null;
            //定义HTTP请求方法
            BDBBS_SendMessage.WebClient HTTPproc = new WebClient();
            //设置HTTP请求默认编码
            HTTPproc.Encoding = System.Text.Encoding.Default;
            //设置HTTP请求头部Cookie内容
            HTTPproc.RequestHeaders.Add("Cookie:__utma=113223638.2059042501.1275448269.1275448269.1275448766.2; __utmz=113223638.1275448269.1.1.utmccn=(direct)|utmcsr=(direct)|utmcmd=(none); lzstat_uv=337678959563538884|11295; " + strImageCookie + "; __utmc=113223638; lzstat_ss=2614707838_0_1275477565_11295");
            //发送HTTP登录请求
            try
            {
                strContent = HTTPproc.OpenRead("http://club.bandao.cn/login.asp", "bdurl=&username=" + textBox_LoginForm_UserName.Text.Trim() + "&password=" + textBox_LoginForm_Password.Text.Trim() + "&icode=" + textBox_LoginForm_ImageCode.Text.Trim() + "&loginsub=submit");         
            }
            catch
            {
                MessageBox.Show("服务器请求超时！", "系统提示");
            }

            if (strContent.IndexOf("密码不正确") > 0)
            {
                MessageBox.Show("您输入的密码不正确", "系统提示");
                ImageCode();
            }
            else if (strContent.IndexOf("该用户名不存在") > 0)
            {
                MessageBox.Show("该用户名不存在", "系统提示");
                ImageCode();
            }
            else if (strContent.IndexOf("验证码不正确") > 0)
            {
                MessageBox.Show("您输入的验证码不正确", "系统提示");
                ImageCode();
            }
            else if (strContent.IndexOf("请输入会员代号和密码") > 0)
            {
                MessageBox.Show("请输入用户名和密码", "系统提示");
                ImageCode();
            }
            else
            {
                try
                {
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

                    DialogResult dr = MessageBox.Show(resultString, "用户信息", MessageBoxButtons.OK);
                    if (dr == DialogResult.OK)
                    {
                        this.Hide();    //隐藏登录窗口
                        BD_Main_Form Main_Form = new BD_Main_Form();    //调用主操作窗口
                        Main_Form.label_MainForm_UserInfo.Text = resultString;  //将用户信息传递到主窗口
                        Main_Form.strCookie = HTTPproc.Cookie;   //传递用户Cookies信息到主窗口
                        Main_Form.strUserName = textBox_LoginForm_UserName.Text.ToString().Trim();  //传递用户UserName信息到主窗口
                        if (Main_Form.ShowDialog() == DialogResult.Cancel)
                        {
                            Application.Exit();
                        }
                    }
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        #endregion 登录方法及信息返回 LoginHandle()
    }
}