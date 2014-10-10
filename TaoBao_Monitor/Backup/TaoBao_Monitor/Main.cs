using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace TaoBao_Monitor
{
    public partial class Main : Form
    {
        TaoBao_Monitor.WebClient HTTPproc = new WebClient();

        public Main()
        {
            InitializeComponent();
            panel_Option.Visible = false;
            LoadLinks();
        }

        private void ShowProductInfo()
        {
            string strRequest = null;
            string strContent = null;
            string resultString = null;

            HTTPproc.Encoding = System.Text.Encoding.Default;

            strRequest = ReadLinks("link1");
            strContent = HTTPproc.OpenRead(strRequest);

            try
            {
                resultString = Regex.Match(strContent, "<title>.*</title>").Value.Replace("<title>","").Replace("-淘宝网</title>","");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("由于网络原因，请求超时！");
                // Syntax error in the regular expression
            }
            //label1.Text = resultString;
            groupBox_Product_1.Text = resultString;

            try
            {
                resultString = Regex.Match(strContent, "<strong id=\"J_StrPrice\" >.*</strong>").Value.Replace("<strong id=\"J_StrPrice\" >","").Replace("</strong>","");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label2.Text = "价格：" + resultString;

            try
            {
                resultString = Regex.Match(strContent, "<li class=\"sold-out clearfix\"><span>.*</li>").Value.Replace("<li class=\"sold-out clearfix\"><span>", "").Replace("</span><em>", "").Replace("</em>", "").Replace("</li>", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label3.Text = resultString;

            try
            {
                resultString = Regex.Match(strContent, "成交记录.*件").Value.Replace("(<em>","：").Replace("</em>","");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label1.Text = resultString;


            strRequest = ReadLinks("link2");
            strContent = HTTPproc.OpenRead(strRequest);

            try
            {
                resultString = Regex.Match(strContent, "<title>.*</title>").Value.Replace("<title>", "").Replace("-淘宝网</title>", "");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("由于网络原因，请求超时！");
                // Syntax error in the regular expression
            }
            //label1.Text = resultString;
            groupBox_Product_2.Text = resultString;

            try
            {
                resultString = Regex.Match(strContent, "<strong id=\"J_StrPrice\" >.*</strong>").Value.Replace("<strong id=\"J_StrPrice\" >", "").Replace("</strong>", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label5.Text = "价格：" + resultString;

            try
            {
                resultString = Regex.Match(strContent, "<li class=\"sold-out clearfix\"><span>.*</li>").Value.Replace("<li class=\"sold-out clearfix\"><span>", "").Replace("</span><em>", "").Replace("</em>", "").Replace("</li>", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label6.Text = resultString;

            try
            {
                resultString = Regex.Match(strContent, "成交记录.*件").Value.Replace("(<em>", "：").Replace("</em>", ""); ;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label4.Text = resultString;

            strRequest = ReadLinks("link3");
            strContent = HTTPproc.OpenRead(strRequest);

            try
            {
                resultString = Regex.Match(strContent, "<title>.*</title>").Value.Replace("<title>", "").Replace("-淘宝网</title>", "");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("由于网络原因，请求超时！");
                // Syntax error in the regular expression
            }
            //label1.Text = resultString;
            groupBox_Product_3.Text = resultString;

            try
            {
                resultString = Regex.Match(strContent, "<strong id=\"J_StrPrice\" >.*</strong>").Value.Replace("<strong id=\"J_StrPrice\" >", "").Replace("</strong>", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label8.Text = "价格：" + resultString;

            try
            {
                resultString = Regex.Match(strContent, "<li class=\"sold-out clearfix\"><span>.*</li>").Value.Replace("<li class=\"sold-out clearfix\"><span>", "").Replace("</span><em>", "").Replace("</em>", "").Replace("</li>", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label9.Text = resultString;

            try
            {
                resultString = Regex.Match(strContent, "成交记录.*件").Value.Replace("(<em>", "：").Replace("</em>", ""); ;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label7.Text = resultString;

            strRequest = ReadLinks("link4");
            strContent = HTTPproc.OpenRead(strRequest);

            try
            {
                resultString = Regex.Match(strContent, "<title>.*</title>").Value.Replace("<title>", "").Replace("-淘宝网</title>", "");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("由于网络原因，请求超时！");
                // Syntax error in the regular expression
            }
            //label1.Text = resultString;
            groupBox_Product_4.Text = resultString;

            try
            {
                resultString = Regex.Match(strContent, "<strong id=\"J_StrPrice\" >.*</strong>").Value.Replace("<strong id=\"J_StrPrice\" >", "").Replace("</strong>", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label11.Text = "价格：" + resultString;

            try
            {
                resultString = Regex.Match(strContent, "<li class=\"sold-out clearfix\"><span>.*</li>").Value.Replace("<li class=\"sold-out clearfix\"><span>", "").Replace("</span><em>", "").Replace("</em>", "").Replace("</li>", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label12.Text = resultString;

            try
            {
                resultString = Regex.Match(strContent, "成交记录.*件").Value.Replace("(<em>", "：").Replace("</em>", ""); ;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label10.Text = resultString;
            ////////////////////////////////////////////////////////////////////////////////////////
            strRequest = ReadLinks("link5");
            strContent = HTTPproc.OpenRead(strRequest);

            try
            {
                resultString = Regex.Match(strContent, "<title>.*</title>").Value.Replace("<title>", "").Replace("-淘宝网</title>", "");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("由于网络原因，请求超时！");
                // Syntax error in the regular expression
            }
            //label1.Text = resultString;
            groupBox_Product_5.Text = resultString;

            try
            {
                resultString = Regex.Match(strContent, "<strong id=\"J_StrPrice\" >.*</strong>").Value.Replace("<strong id=\"J_StrPrice\" >", "").Replace("</strong>", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label13.Text = "价格：" + resultString;

            try
            {
                resultString = Regex.Match(strContent, "<li class=\"sold-out clearfix\"><span>.*</li>").Value.Replace("<li class=\"sold-out clearfix\"><span>", "").Replace("</span><em>", "").Replace("</em>", "").Replace("</li>", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label14.Text = resultString;

            try
            {
                resultString = Regex.Match(strContent, "成交记录.*件").Value.Replace("(<em>", "：").Replace("</em>", ""); ;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label15.Text = resultString;
            ////////////////////////////////////////////////////////////////////////////////////////
            strRequest = ReadLinks("link6");
            strContent = HTTPproc.OpenRead(strRequest);

            try
            {
                resultString = Regex.Match(strContent, "<title>.*</title>").Value.Replace("<title>", "").Replace("-淘宝网</title>", "");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("由于网络原因，请求超时！");
                // Syntax error in the regular expression
            }
            //label1.Text = resultString;
            groupBox_Product_6.Text = resultString;

            try
            {
                resultString = Regex.Match(strContent, "<strong id=\"J_StrPrice\" >.*</strong>").Value.Replace("<strong id=\"J_StrPrice\" >", "").Replace("</strong>", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label16.Text = "价格：" + resultString;

            try
            {
                resultString = Regex.Match(strContent, "<li class=\"sold-out clearfix\"><span>.*</li>").Value.Replace("<li class=\"sold-out clearfix\"><span>", "").Replace("</span><em>", "").Replace("</em>", "").Replace("</li>", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label17.Text = resultString;

            try
            {
                resultString = Regex.Match(strContent, "成交记录.*件").Value.Replace("(<em>", "：").Replace("</em>", ""); ;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            label18.Text = resultString;
        }

        private void button_Refresh_Click(object sender, EventArgs e)
        {
            ShowProductInfo();
        }

        private void button_capimg_Click(object sender, EventArgs e)
        {
            Rectangle r = Screen.PrimaryScreen.Bounds;
            Main main = new Main();
            //main.Size.Width
            Image img = new Bitmap(main.Size.Width, main.Size.Height);
            Graphics g = Graphics.FromImage(img);

            //g.CopyFromScreen(new Point(intX, intY), new Point(0, 0), main.Size);
            g.CopyFromScreen(Left,Top,0,0,main.Size);
            string dt = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
            img.Save(Application.StartupPath + Path.DirectorySeparatorChar + "img" + Path.DirectorySeparatorChar + dt.ToString() + ".bmp");
        }

        private void button_Option_Click(object sender, EventArgs e)
        {
            panel_Option.Visible = true;
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            string filePath = Path.GetFullPath(@"Links.ini");
            INIClass iniClass = new INIClass(filePath);

            iniClass.IniWriteValue("Links", "link1", textBox_Link19.Text.Trim());
            iniClass.IniWriteValue("Links", "link2", textBox_Link20.Text.Trim());
            iniClass.IniWriteValue("Links", "link3", textBox_Link21.Text.Trim());
            iniClass.IniWriteValue("Links", "link4", textBox_Link22.Text.Trim());
            iniClass.IniWriteValue("Links", "link5", textBox_Link23.Text.Trim());
            iniClass.IniWriteValue("Links", "link6", textBox_Link24.Text.Trim());
            panel_Option.Visible = false;
        }

        private string ReadLinks(string strLinks)
        {
            string strLink = null;
            string filePath = Path.GetFullPath(@"Links.ini");
            INIClass iniClass = new INIClass(filePath);

            if (strLinks == "link1")
            {
                strLink = iniClass.IniReadValue("Links", "link1");
            }
            if (strLinks == "link2")
            {
                strLink = iniClass.IniReadValue("Links", "link2");
            }
            if (strLinks == "link3")
            {
                strLink = iniClass.IniReadValue("Links", "link3");
            }
            if (strLinks == "link4")
            {
                strLink = iniClass.IniReadValue("Links", "link4");
            }
            if (strLinks == "link5")
            {
                strLink = iniClass.IniReadValue("Links", "link5");
            }
            if (strLinks == "link6")
            {
                strLink = iniClass.IniReadValue("Links", "link6");
            }
            return strLink;
        }

        private void LoadLinks()
        {
            string filePath = Path.GetFullPath(@"Links.ini");
            INIClass iniClass = new INIClass(filePath);

            textBox_Link19.Text = iniClass.IniReadValue("Links", "link1");
            textBox_Link20.Text = iniClass.IniReadValue("Links", "link2");
            textBox_Link21.Text = iniClass.IniReadValue("Links", "link3");
            textBox_Link22.Text = iniClass.IniReadValue("Links", "link4");
            textBox_Link23.Text = iniClass.IniReadValue("Links", "link5");
            textBox_Link24.Text = iniClass.IniReadValue("Links", "link6");
        }
    }
}