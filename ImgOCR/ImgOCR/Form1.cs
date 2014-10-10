using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ImgOCR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ImageCode();
            //70*26
        }

        private System.Drawing.Bitmap curBitmap; //全局变量
        private System.Drawing.Bitmap[] PicArray;//切割后的图片

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
            //定义HTTP请求方法
            ImgOCR.WebClient HTTPproc = new WebClient();
            //设置HTTP请求默认编码
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            //向指定网址请求返回数据流
            this.pictureBox_Code.Image = Image.FromStream(HTTPproc.GetStream("http://bo.tianxia.taobao.com/GainCode", ""));
            curBitmap = new Bitmap(pictureBox_Code.Image);
            ImgOCR(curBitmap);
        }
        #endregion 获取验证码图片 ImageCode()

        #region 图片识别 ImgOCR(Bitmap sss)
        /// <summary>
        /// 图片识别
        /// </summary>
        /// <param name="sss"></param>
        private void ImgOCR(Bitmap sss)
        {
            //将图片灰度化
            if (curBitmap != null)
            {

                Color curColor;
                int ret;
                for (int i = 0; i < curBitmap.Width; i++)
                {
                    for (int j = 0; j < curBitmap.Height; j++)
                    {   //获取此 Bitmap 中指定像素的颜色
                        curColor = curBitmap.GetPixel(i, j);
                        //计算灰度值
                        ret = (int)(curColor.R * 0.299 + curColor.G * 0.587 + curColor.B * 0.114);
                        //设置此 Bitmap 中指定像素的颜色
                        curBitmap.SetPixel(i, j, Color.FromArgb(ret, ret, ret));
                    }
                }
            }

            //将图片转成黑白
            int dgGrayValue = GetDgGrayValue(curBitmap);
            curBitmap = this.Threshoding(curBitmap, dgGrayValue);

            //将图片切割
            PicArray = new Bitmap[4];
            Rectangle cloneRect = new Rectangle(10, 3, 12, 18);
            System.Drawing.Imaging.PixelFormat format = curBitmap.PixelFormat;
            PicArray[0] = curBitmap.Clone(cloneRect, format);
            Rectangle cloneRect1 = new Rectangle(23, 3, 12, 18);
            System.Drawing.Imaging.PixelFormat format1 = curBitmap.PixelFormat;
            PicArray[1] = curBitmap.Clone(cloneRect1, format1);
            Rectangle cloneRect2 = new Rectangle(36, 3, 12, 18);
            System.Drawing.Imaging.PixelFormat format2 = curBitmap.PixelFormat;
            PicArray[2] = curBitmap.Clone(cloneRect2, format2);
            Rectangle cloneRect3 = new Rectangle(49, 3, 12, 18);
            System.Drawing.Imaging.PixelFormat format3 = curBitmap.PixelFormat;
            PicArray[3] = curBitmap.Clone(cloneRect3, format3);

            String[] tNumArr = new String[4]; //识别后的4个数字
            String[] tMuban = new String[10] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            //加载10个标准数字
            Bitmap[] sNumArr = new Bitmap[10];

            sNumArr[0] = new Bitmap(Properties.Resources._0);
            sNumArr[1] = new Bitmap(Properties.Resources._1);
            sNumArr[2] = new Bitmap(Properties.Resources._2);
            sNumArr[3] = new Bitmap(Properties.Resources._3);
            sNumArr[4] = new Bitmap(Properties.Resources._4);
            sNumArr[5] = new Bitmap(Properties.Resources._5);
            sNumArr[6] = new Bitmap(Properties.Resources._6);
            sNumArr[7] = new Bitmap(Properties.Resources._7);
            sNumArr[8] = new Bitmap(Properties.Resources._8);
            sNumArr[9] = new Bitmap(Properties.Resources._9);

            //分割出来的4个数字分别与等宽的标准数字相匹配
            for (int i = 0; i < 4; i++)
            {
                int maxMatch = 0;
                Bitmap cutNum = PicArray[i];
                for (int s = 0; s < 10; s++)
                {
                    if (cutNum.Width == sNumArr[s].Width) //等宽则匹配
                    {
                        int curMatch = 0;
                        for (int j = 0; j < cutNum.Width; j++)
                        {
                            for (int k = 0; k < cutNum.Width; k++)
                            {
                                if (cutNum.GetPixel(j, k).ToArgb() == sNumArr[s].GetPixel(j, k).ToArgb())
                                    curMatch++;
                            }
                        }
                        if (curMatch > maxMatch)
                        {
                            maxMatch = curMatch;
                            tNumArr[i] = tMuban[s];
                        }
                    }
                }
            }
            //Form1 frm1 = new Form1();
            //frm1.Text = tNumArr[0] + " " + tNumArr[1] + " " + tNumArr[2] + " " + tNumArr[3];
            MessageBox.Show(tNumArr[0] + " " + tNumArr[1] + " " + tNumArr[2] + " " + tNumArr[3]);
        }
        #endregion 图片识别 ImgOCR(Bitmap sss)

        #region 二值化将图像转为黑白 Threshoding(Bitmap map, int threshold)
        /// <summary>
        /// 二值化将图像转为黑白
        /// </summary>
        /// <param name="map"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public Bitmap Threshoding(Bitmap map, int threshold)
        {

            int x = map.Width;
            int y = map.Height;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (map.GetPixel(i, j).R >= threshold)
                    {
                        map.SetPixel(i, j, Color.FromArgb(0xff, 0xff, 0xff));
                    }
                    else
                    {
                        map.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            return map;
        }
        #endregion 二值化将图像转为黑白 Threshoding(Bitmap map, int threshold)

        #region 最大类间方差法 GetDgGrayValue(Bitmap bmpobj)
        /// <summary>
        /// 最大类间方差法
        /// </summary>
        /// <param name="bmpobj"></param>
        /// <returns></returns>
        public int GetDgGrayValue(Bitmap bmpobj)
        {
            int[] pixelNum = new int[256];           //图象直方图，共256个点
            int n, n1, n2;                              //n为图像总点数，n1前景的总点数，n2背景的总点数
            int total;                              //total为总和，累计值
            double m1, m2, sum, csum, fmax, sb;     //sb为类间方差，fmax存储最大方差值
            int k, t, q;
            int threshValue = 1;                      // 阈值

            //生成直方图
            for (int i = 0; i < bmpobj.Width; i++)
            {
                for (int j = 0; j < bmpobj.Height; j++)
                {
                    //返回各个点的颜色，以RGB表示
                    pixelNum[bmpobj.GetPixel(i, j).R]++;            //相应的直方图加1
                }
            }
            //直方图平滑化
            for (k = 0; k <= 255; k++)
            {
                total = 0;
                for (t = -2; t <= 2; t++)              //与附近2个灰度做平滑化，t值应取较小的值
                {
                    q = k + t;
                    if (q < 0)                     //越界处理
                        q = 0;
                    if (q > 255)
                        q = 255;
                    total = total + pixelNum[q];    //total为总和，累计值
                }
                pixelNum[k] = (int)((float)total / 5.0 + 0.5);    //平滑化，左边2个+中间1个+右边2个灰度，共5个，所以总和除以5，后面加0.5是用修正值
            }
            //求阈值
            sum = csum = 0.0;
            n = 0;
            //计算总的图象的点数和质量矩，为后面的计算做准备
            for (k = 0; k <= 255; k++)
            {
                sum += (double)k * (double)pixelNum[k];     //x*f(x)质量矩，也就是每个灰度的值乘以其点数（归一化后为概率），sum为其总和
                n += pixelNum[k];                       //n为图象总的点数，归一化后就是累积概率
            }

            fmax = -1.0;                          //类间方差sb不可能为负，所以fmax初始值为-1不影响计算的进行
            n1 = 0;
            for (k = 0; k < 256; k++)                  //对每个灰度（从0到255）计算一次分割后的类间方差sb
            {
                n1 += pixelNum[k];                //n1为在当前阈值遍前景图象的点数
                if (n1 == 0) { continue; }            //没有分出前景后景
                n2 = n - n1;                        //n2为背景图象的点数
                if (n2 == 0) { break; }               //n2为0表示全部都是后景图象，与n1=0情况类似，之后的遍历不可能使前景点数增加，所以此时可以退出循环
                csum += (double)k * pixelNum[k];    //前景的“灰度的值*其点数”的总和
                m1 = csum / n1;                     //m1为前景的平均灰度
                m2 = (sum - csum) / n2;               //m2为背景的平均灰度
                sb = (double)n1 * (double)n2 * (m1 - m2) * (m1 - m2);   //sb为类间方差
                if (sb > fmax)                  //如果算出的类间方差大于前一次算出的类间方差
                {
                    fmax = sb;                    //fmax始终为最大类间方差（otsu）
                    threshValue = k;              //取最大类间方差时对应的灰度的k就是最佳阈值
                }
            }
            return threshValue;
        }
        #endregion 最大类间方差法 GetDgGrayValue(Bitmap bmpobj)
    }
}