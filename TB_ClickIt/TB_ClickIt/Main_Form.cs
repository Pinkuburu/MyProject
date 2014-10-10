using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TB_ClickIt
{
    public partial class Main_Form : Form
    {
        TB_ClickIt.WebClient HTTPproc = new WebClient();

        public Main_Form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strResponse = null;
            string resultString = null;
            string strRequest = "http://re.taobao.com/search?keyword=%B7%E7%D6%D0%CD%D1%CA%D6&catid=&refpid=420434_1006&propertyid=&refpos=&frcatid=&back=lo1%3D30%26lo2%3D30%26nt%3D1&dpback=lo1%3D30%26lo2%3D30%26nt%3D1&isinner=1&yp4p_page=2";
            
            HTTPproc.Encoding = System.Text.Encoding.Default;

            strResponse = HTTPproc.OpenRead(strRequest);
            
            try
            {
                resultString = Regex.Match(strResponse, @"href=""http://click.simba.taobao.com/cc_im\?p=.*title=""日本原单").Value;
                strRequest = resultString.Replace("href=\"", "").Replace("\" target=\"_blank\" class=\"EventCanSelect\" title=\"日本原单", "").Replace("amp;","");
                HTTPproc.OpenRead(strRequest);
                resultString = HTTPproc.StrResponseHeaders.ToString();

                try
                {
                    resultString = Regex.Match(resultString, "http://.*").Value;
                    resultString = HTTPproc.OpenRead(resultString);
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }
                MessageBox.Show("完成");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
        }
    }
}