using System;
using System.Windows.Forms;
using System.Net;

namespace Client
{
    public partial class Form_Download : Form
    {
        WebClient webClient = new WebClient();
        public delegate void EventHandler(object sender);
        public event EventHandler DownloadFileCompleted;//在线状态事件

        public Form_Download(string url,string path)
        {
            InitializeComponent();
            DownloadFile(url, path);
        }

        private void DownloadFile(string strUrl, string strPath)
        {
            //if (webClient.IsBusy)//是否存在正在进行中的Web请求
            //{
            //    webClient.CancelAsync();
            //}

            //为webClient添加事件
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
            //开始下载
            webClient.DownloadFileAsync(new Uri(strUrl), strPath);
        }

        //下载完成时触发
        void webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //MessageBox.Show("下载被取消！");
                DownloadFileCompleted("下载被取消");
                this.Close();
                this.Dispose();
            }
            else if (e.Error != null)
            {
                //MessageBox.Show(e.Error.Message);
                DownloadFileCompleted(e.Error.Message);
                this.Close();
                this.Dispose();
            }
            else
            {
                //MessageBox.Show("下载完成！");
                DownloadFileCompleted("下载完成！");
                this.Close();
                this.Dispose();
            }
        }

        //正在下载
        void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            this.Text = string.Format("正在下载文件，完成进度{0}KB/{1}KB"
                                , e.BytesReceived / 1024
                                , e.TotalBytesToReceive / 1024);
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.webClient.CancelAsync();
            this.webClient.Dispose();
            DownloadFileCompleted("取消下载");
            this.Close();
            this.Dispose();
        }
    }
}
