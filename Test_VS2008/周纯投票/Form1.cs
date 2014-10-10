using System;
using System.Drawing;
using System.Windows.Forms;

namespace 周纯投票
{
    public partial class Form1 : Form
    {
        public delegate void SetTextBox_Debug(string str);  //委托
        
        //定义HTTP请求方法
        WebClient HTTPproc = new WebClient();
        string strRequest = "http://vip.hiao.com/2011/04/babyshow/poll39764.html";
        string strParameter = "";
        string strContent = "";
        string strCode = "";

        public Form1()
        {
            InitializeComponent();
            ImageCode();

            textBox_Code.KeyPress += new KeyPressEventHandler(textBox_Code_KeyPress);

        }

        void textBox_Code_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox_Code.Text.Length == 4)
            {
                DateTime dt = DateTime.Now;
                //设置HTTP请求默认编码
                HTTPproc.Encoding = System.Text.Encoding.Default;
                strCode = textBox_Code.Text.Trim().ToString();
                strParameter = "tbCode=" + strCode + "&referurl=http%3A%2F%2Fvip.hiao.com%2F2011%2F04%2Fbabyshow%2Fview39764.html";
                strContent = HTTPproc.OpenRead(strRequest, strParameter);
                if (strContent.IndexOf("您的投票已经提交成功") > 0)
                {
                    DebugMSG(dt.ToString() + " | 投票成功");
                    textBox_Code.Text = "";
                    ImageCode();
                }
                else
                {
                    DebugMSG(dt.ToString() + " | 投票出错");
                }
            }
        }

        private void pictureBox_Code_Click(object sender, EventArgs e)
        {
            ImageCode();
        }

        #region 获取验证码图片 ImageCode()
        /// <summary>
        /// 获取验证码图片
        /// </summary>
        private void ImageCode()
        {            
            //向指定网址请求返回数据流
            pictureBox_Code.Image = Image.FromStream(HTTPproc.GetStream("http://vip.hiao.com/code/GetCode.asp", ""));
            //将得到的Cookie存储到全局变量strImageCookie
            //strImageCookie = HTTPproc.Cookie.ToString();
        }
        #endregion 获取验证码图片 ImageCode()

        /// <summary>
        /// Web调试信息
        /// </summary>
        /// <param name="strContent">显示内容</param>
        private void DebugMSG(string strContent)
        {
            if (this.textBox_Msg.InvokeRequired)
            {
                SetTextBox_Debug setBox_Debug = new SetTextBox_Debug(DebugMSG);
                this.textBox_Msg.Invoke(setBox_Debug, strContent);
            }
            else
            {
                textBox_Msg.Text += strContent + "\r\n";
                textBox_Msg.SelectionStart = textBox_Msg.Text.Length;
                textBox_Msg.ScrollToCaret();
            }
        }
    }
}
