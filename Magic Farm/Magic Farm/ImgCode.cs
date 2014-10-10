using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Magic_Farm
{
    public partial class ImgCode : Form
    {
        public string strCookies = null;
        public string strRequest = null;
        public string strImgCode = null;
        string strRequest_s = null;
        Magic_Farm.WebClient HTTPproc = new WebClient();

        public ImgCode()
        {
            InitializeComponent();
            this.AcceptButton = button_ImgCode;
        }

        private void button_ImgCode_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            strImgCode = textBox_ImgCode.Text.Trim().ToString();
            this.Close();
        }

        private void ImgCode_Load(object sender, EventArgs e)
        {
            strRequest_s = strRequest;
            HTTPproc.RequestHeaders.Add("Referer:" + strRequest_s + "");
            HTTPproc.RequestHeaders.Add("Accept:image/png,image/*;q=0.8,*/*;q=0.5");
            HTTPproc.RequestHeaders.Add("Accept-Language:zh;q=0.5");
            HTTPproc.RequestHeaders.Add("Accept-Encoding:gzip,deflate");
            HTTPproc.RequestHeaders.Add("Accept-Charset:GB2312,utf-8;q=0.7,*;q=0.7");
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookies + "");
            //向指定网址请求返回数据流
            strRequest = "http://bo.tianxia.taobao.com/checkcode.jpg";
            HTTPproc.DownloadFile(strRequest, @"Image\Code.jpg");
            pictureBox_ImgCode.ImageLocation = @"Image\Code.jpg";
        }

        private void pictureBox_ImgCode_Click(object sender, EventArgs e)
        {
            strRequest_s = strRequest_s;
            HTTPproc.RequestHeaders.Add("Referer:" + strRequest_s + "");
            HTTPproc.RequestHeaders.Add("Accept:image/png,image/*;q=0.8,*/*;q=0.5");
            HTTPproc.RequestHeaders.Add("Accept-Language:zh;q=0.5");
            HTTPproc.RequestHeaders.Add("Accept-Encoding:gzip,deflate");
            HTTPproc.RequestHeaders.Add("Accept-Charset:GB2312,utf-8;q=0.7,*;q=0.7");
            HTTPproc.RequestHeaders.Add("Cookie:" + strCookies + "");
            //向指定网址请求返回数据流
            strRequest = "http://bo.tianxia.taobao.com/checkcode.jpg";
            HTTPproc.DownloadFile(strRequest, @"Image\Code.jpg");
            pictureBox_ImgCode.ImageLocation = @"Image\Code.jpg";
        }
    }
}