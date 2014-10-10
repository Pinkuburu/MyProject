using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace QDBBS_SendMessage
{
    public partial class Form1 : Form
    {
        #region 窗体
        public Form1()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "";
            listView1.Visible = false;
            Form2 f2 = new Form2();
            f2.Show();
            //ListViewItem li = new ListViewItem();
            //li.SubItems.Clear();

            //f2.listView1.Items.Add(li);

            //f2.listView1.Items[0].SubItems[0].Text = "qwewqeqwe";
            //f2.listView1.Items[0].SubItems[1].Text = "qwewqeqwe";
        }
        #endregion

        #region 登录
        private void button1_Click(object sender, EventArgs e)
        {
            string strOut = "";
            Login(this.textUserName.Text.ToString().Trim(), this.textPassword.Text.ToString().Trim(), "http://club.qingdaonews.com/sublogin_new.php", out strOut);
            //MessageBox.Show(strOut);
        }
        #endregion

        #region 登录验证
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        /// <param name="strLoginUrl"></param>
        /// <param name="Txt"></param>
        public void Login(string strUserName, string strPassword, string strLoginUrl, out string Txt)
        {
            try
            {
                //定义Cookie容器
                CookieContainer CookieArray = new CookieContainer();

                //创建Http请求
                HttpWebRequest LoginHttpWebRequest = (HttpWebRequest)WebRequest.Create(strLoginUrl);

                //登录数据
                string LoginData = "id=" + strUserName + "&passwd=" + strPassword + "&usertype=0";
                //数据被传输类型
                LoginHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //数据长度
                LoginHttpWebRequest.ContentLength = LoginData.Length;
                //数据传输方法 get或post
                LoginHttpWebRequest.Method = "POST";
                //设置HttpWebRequest的CookieContainer为刚才建立的那个CookieArray  
                LoginHttpWebRequest.CookieContainer = CookieArray;
                //获取登录数据流
                Stream myRequestStream = LoginHttpWebRequest.GetRequestStream();
                //StreamWriter
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                //把数据写入HttpWebRequest的Request流  
                myStreamWriter.Write(LoginData);

                //关闭打开对象     
                myStreamWriter.Close();

                myRequestStream.Close();

                //新建一个HttpWebResponse     
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)LoginHttpWebRequest.GetResponse();

                //获取一个包含url的Cookie集合的CookieCollection     
                myHttpWebResponse.Cookies = CookieArray.GetCookies(LoginHttpWebRequest.RequestUri);

                if (myHttpWebResponse.Cookies.Count > 0)
                {
                    CookieArray.Add(myHttpWebResponse.Cookies);
                }

                string strCookieSessionID = myHttpWebResponse.Cookies[0].Value.ToString();
                string strCookieUserName = myHttpWebResponse.Cookies[1].Value.ToString();
                string strCookiePassowrd = myHttpWebResponse.Cookies[2].Value.ToString();

                WebHeaderCollection a = myHttpWebResponse.Headers;

                Stream myResponseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("gb2312"));

                Txt = myStreamReader.ReadToEnd();

                if (Txt == "<meta http-equiv=Refresh content=0;URL=login_club_new1.php>")
                {
                    toolStripStatusLabel1.Text = "Login.OK";
                    //this.Hide();
                    //Form2 f2 = new Form2();
                    //f2.Show();
                    textUserName.Visible = false;
                    textPassword.Visible = false;
                    button1.Visible = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    //this.Width = 500;
                    //this.Height = 300;
                    listView1.Visible = true;
                    //listView1.Width = 402;
                    //listView1.Height = 214;

                    this.Text = "Sender - " + strUserName;
                    string strOut;
                    LoginSec(strCookieSessionID, strCookieUserName, strCookiePassowrd, out strOut);
                }
                else
                {
                    toolStripStatusLabel1.Text = "Login.ERROR";
                }

                //把数据从HttpWebResponse的Response流中读出     
                myStreamReader.Close();

                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 二次验证
        /// <summary>
        /// 二次验证
        /// </summary>
        /// <param name="strCookieSessionID"></param>
        /// <param name="strCookieUserName"></param>
        /// <param name="strCookiePassword"></param>
        /// <param name="strShowBox"></param>
        public void LoginSec(string strCookieSessionID, string strCookieUserName, string strCookiePassword, out string strShowBox)
        {
            try
            {
                //定义Cookie容器
                CookieContainer CookieArray = new CookieContainer();

                //创建Http请求
                HttpWebRequest LoginHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://club.qingdaonews.com/login_club_new1.php");

                //登录数据
                //string LoginData = "id=" + strUserName + "&passwd=" + strPassword + "&usertype=0";
                //数据被传输类型
                //LoginHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //数据长度
                //LoginHttpWebRequest.ContentLength = LoginData.Length;
                //数据传输方法 get或post
                LoginHttpWebRequest.Method = "POST";
                LoginHttpWebRequest.Accept = "*/*";
                LoginHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; MAXTHON 2.0)";
                //设置HttpWebRequest的CookieContainer为刚才建立的那个CookieArray  
                LoginHttpWebRequest.CookieContainer = CookieArray;
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("PHPSESSID", strCookieSessionID));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[username]", strCookieUserName));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[password]", strCookiePassword));

                //获取登录数据流
                Stream myRequestStream = LoginHttpWebRequest.GetRequestStream();
                //StreamWriter
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                //把数据写入HttpWebRequest的Request流  
                //myStreamWriter.Write(LoginData);

                //关闭打开对象     
                //myStreamWriter.Close();

                myRequestStream.Close();

                //新建一个HttpWebResponse     
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)LoginHttpWebRequest.GetResponse();

                //获取一个包含url的Cookie集合的CookieCollection     
                myHttpWebResponse.Cookies = CookieArray.GetCookies(LoginHttpWebRequest.RequestUri);

                if (myHttpWebResponse.Cookies.Count > 0)
                {
                    CookieArray.Add(myHttpWebResponse.Cookies);
                }

                WebHeaderCollection a = myHttpWebResponse.Headers;

                Stream myResponseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("gb2312"));

                strShowBox = myStreamReader.ReadToEnd();

                //把数据从HttpWebResponse的Response流中读出     
                myStreamReader.Close();

                myResponseStream.Close();

                string[] arrClubList = {"club_entry_2_2_0_1_0.htm", "club_entry_1025_2_0_1_0.htm", "club_entry_48_2_0_1_0.htm", "club_entry_67_2_0_1_0.htm", "club_entry_57_2_0_1_0.htm", 
                    "club_entry_9_2_0_1_0.htm", "club_entry_1038_2_0_1_0.htm", "club_entry_88_2_0_1_0.htm", "club_entry_123_2_0_1_0.htm", "club_entry_1_2_0_1_0.htm", "club_entry_156_2_0_1_0.htm", 
                    "club_entry_1018_2_0_1_0.htm", "club_entry_1023_2_0_1_0.htm", "club_entry_1024_2_0_1_0.htm", "club_entry_1115_2_0_1_0.htm", "club_entry_1133_2_0_1_0.htm", "club_entry_41_2_0_1_0.htm", 
                    "club_entry_1030_2_0_1_0.htm", "club_entry_1170_2_0_1_0.htm", "club_entry_1171_2_0_1_0.htm", "club_entry_1181_2_0_1_0.htm", "club_entry_1188_2_0_1_0.htm", "club_entry_1192_2_0_1_0.htm", 
                    "club_entry_1199_2_0_1_0.htm", "club_entry_128_3_0_1_0.htm", "club_entry_129_3_0_1_0.htm", "club_entry_130_3_0_1_0.htm", "club_entry_131_3_0_1_0.htm", "club_entry_132_3_0_1_0.htm", 
                    "club_entry_133_3_0_1_0.htm", "club_entry_134_3_0_1_0.htm", "club_entry_173_3_0_1_0.htm", "club_entry_1053_3_0_1_0.htm", "club_entry_1054_3_0_1_0.htm", "club_entry_1059_3_0_1_0.htm", 
                    "club_entry_1060_3_0_1_0.htm", "club_entry_39_4_0_1_0.htm", "club_entry_1039_4_0_1_0.htm", "club_entry_1040_4_0_1_0.htm", "club_entry_1041_4_0_1_0.htm", "club_entry_1042_4_0_1_0.htm", 
                    "club_entry_1043_4_0_1_0.htm", "club_entry_1044_4_0_1_0.htm", "club_entry_1045_4_0_1_0.htm", "club_entry_1046_4_0_1_0.htm", "club_entry_1047_4_0_1_0.htm", "club_entry_1048_4_0_1_0.htm", 
                    "club_entry_1049_4_0_1_0.htm", "club_entry_1052_4_0_1_0.htm", "club_entry_1055_4_0_1_0.htm", "club_entry_1056_4_0_1_0.htm", "club_entry_1057_4_0_1_0.htm", "club_entry_1061_4_0_1_0.htm", 
                    "club_entry_1062_4_0_1_0.htm", "club_entry_1063_4_0_1_0.htm", "club_entry_1069_4_0_1_0.htm", "club_entry_1070_4_0_1_0.htm", "club_entry_1071_4_0_1_0.htm", "club_entry_154_4_0_1_0.htm", 
                    "club_entry_1085_4_0_1_0.htm", "club_entry_1086_4_0_1_0.htm", "club_entry_1179_4_0_1_0.htm", "club_entry_1102_4_0_1_0.htm", "club_entry_1108_4_0_1_0.htm", "club_entry_1109_4_0_1_0.htm", 
                    "club_entry_1111_4_0_1_0.htm", "club_entry_1121_4_0_1_0.htm", "club_entry_1124_4_0_1_0.htm", "club_entry_1130_4_0_1_0.htm", "club_entry_1131_4_0_1_0.htm", "club_entry_1143_4_0_1_0.htm", 
                    "club_entry_1157_4_0_1_0.htm", "club_entry_1158_4_0_1_0.htm", "club_entry_1159_4_0_1_0.htm", "club_entry_1169_4_0_1_0.htm", "club_entry_1173_4_0_1_0.htm", "club_entry_1184_4_0_1_0.htm", 
                    "club_entry_1185_4_0_1_0.htm", "club_entry_1190_4_0_1_0.htm", "club_entry_1172_4_0_1_0.htm", "club_entry_20_5_0_1_0.htm", "club_entry_1010_5_0_1_0.htm", "club_entry_49_5_0_1_0.htm", 
                    "club_entry_1139_5_0_1_0.htm", "club_entry_1115_6_0_1_0.htm", "club_entry_47_6_0_1_0.htm", "club_entry_66_6_0_1_0.htm", "club_entry_12_6_0_1_0.htm", "club_entry_86_6_0_1_0.htm", 
                    "club_entry_163_6_0_1_0.htm", "club_entry_13_6_0_1_0.htm", "club_entry_94_6_0_1_0.htm", "club_entry_83_6_0_1_0.htm", "club_entry_174_6_0_1_0.htm", "club_entry_175_6_0_1_0.htm", 
                    "club_entry_176_6_0_1_0.htm", "club_entry_177_6_0_1_0.htm", "club_entry_1006_6_0_1_0.htm", "club_entry_1007_6_0_1_0.htm", "club_entry_1011_6_0_1_0.htm", "club_entry_1031_6_0_1_0.htm", 
                    "club_entry_58_7_0_1_0.htm", "club_entry_1005_7_0_1_0.htm", "club_entry_160_7_0_1_0.htm", "club_entry_121_7_0_1_0.htm", "club_entry_1099_7_0_1_0.htm", "club_entry_1100_7_0_1_0.htm", 
                    "club_entry_64_8_0_1_0.htm", "club_entry_1064_8_0_1_0.htm", "club_entry_1094_8_0_1_0.htm", "club_entry_1127_8_0_1_0.htm", "club_entry_1128_8_0_1_0.htm", "club_entry_1142_8_0_1_0.htm", 
                    "club_entry_1201_8_0_1_0.htm", "club_entry_4_9_0_1_0.htm", "club_entry_5_9_0_1_0.htm", "club_entry_36_9_0_1_0.htm", "club_entry_52_9_0_1_0.htm", "club_entry_74_9_0_1_0.htm", 
                    "club_entry_76_9_0_1_0.htm", "club_entry_92_9_0_1_0.htm", "club_entry_124_9_0_1_0.htm", "club_entry_125_9_0_1_0.htm", "club_entry_146_9_0_1_0.htm", "club_entry_162_9_0_1_0.htm", 
                    "club_entry_1015_9_0_1_0.htm", "club_entry_1016_9_0_1_0.htm", "club_entry_1033_9_0_1_0.htm", "club_entry_1068_9_0_1_0.htm", "club_entry_1084_9_0_1_0.htm", "club_entry_1125_9_0_1_0.htm", 
                    "club_entry_1151_9_0_1_0.htm", "club_entry_33_10_0_1_0.htm", "club_entry_93_10_0_1_0.htm", "club_entry_77_10_0_1_0.htm", "club_entry_1072_10_0_1_0.htm", "club_entry_1073_10_0_1_0.htm", 
                    "club_entry_1027_10_0_1_0.htm", "club_entry_1066_10_0_1_0.htm", "club_entry_1077_10_0_1_0.htm", "club_entry_1101_10_0_1_0.htm", "club_entry_1117_10_0_1_0.htm", "club_entry_1189_10_0_1_0.htm", 
                    "club_entry_40_11_0_1_0.htm", "club_entry_1026_11_0_1_0.htm", "club_entry_24_11_0_1_0.htm", "club_entry_43_11_0_1_0.htm", "club_entry_54_11_0_1_0.htm", "club_entry_70_11_0_1_0.htm", 
                    "club_entry_72_11_0_1_0.htm", "club_entry_81_11_0_1_0.htm", "club_entry_84_11_0_1_0.htm", "club_entry_122_11_0_1_0.htm", "club_entry_108_11_0_1_0.htm", "club_entry_22_11_0_1_0.htm", 
                    "club_entry_61_11_0_1_0.htm", "club_entry_8_11_0_1_0.htm", "club_entry_42_11_0_1_0.htm", "club_entry_1126_11_0_1_0.htm", "club_entry_1129_11_0_1_0.htm", "club_entry_1156_11_0_1_0.htm", 
                    "club_entry_145_11_0_1_0.htm", "club_entry_107_11_0_1_0.htm", "club_entry_144_11_0_1_0.htm", "club_entry_1191_11_0_1_0.htm", "club_entry_1144_11_0_1_0.htm", "club_entry_7_12_0_1_0.htm", 
                    "club_entry_27_12_0_1_0.htm", "club_entry_1193_12_0_1_0.htm", "club_entry_1183_12_0_1_0.htm", "club_entry_113_12_0_1_0.htm", "club_entry_46_12_0_1_0.htm", "club_entry_60_12_0_1_0.htm", 
                    "club_entry_1187_12_0_1_0.htm", "club_entry_82_12_0_1_0.htm", "club_entry_98_12_0_1_0.htm", "club_entry_1168_12_0_1_0.htm", "club_entry_1180_12_0_1_0.htm", "club_entry_1075_12_0_1_0.htm", 
                    "club_entry_1078_12_0_1_0.htm", "club_entry_1087_12_0_1_0.htm", "club_entry_38_12_0_1_0.htm", "club_entry_141_12_0_1_0.htm", "club_entry_96_12_0_1_0.htm", "club_entry_97_12_0_1_0.htm", 
                    "club_entry_109_12_0_1_0.htm", "club_entry_1032_12_0_1_0.htm", "club_entry_147_12_0_1_0.htm", "club_entry_1013_12_0_1_0.htm", "club_entry_1149_12_0_1_0.htm", "club_entry_149_12_0_1_0.htm", 
                    "club_entry_1165_12_0_1_0.htm", "club_entry_1200_12_0_1_0.htm", "club_entry_26_17_0_1_0.htm", "club_entry_1067_17_0_1_0.htm", "club_entry_120_17_0_1_0.htm", "club_entry_1096_17_0_1_0.htm", 
                    "club_entry_1020_17_0_1_0.htm", "club_entry_1145_17_0_1_0.htm", "club_entry_1160_17_0_1_0.htm", "club_entry_71_13_0_1_0.htm", "club_entry_166_13_0_1_0.htm", "club_entry_167_13_0_1_0.htm", 
                    "club_entry_168_13_0_1_0.htm", "club_entry_169_13_0_1_0.htm", "club_entry_170_13_0_1_0.htm", "club_entry_165_13_0_1_0.htm", "club_entry_172_13_0_1_0.htm", "club_entry_1018_13_0_1_0.htm", 
                    "club_entry_1030_13_0_1_0.htm", "club_entry_1076_13_0_1_0.htm", "club_entry_1082_13_0_1_0.htm", "club_entry_47_14_0_1_0.htm", "club_entry_29_14_0_1_0.htm", "club_entry_37_14_0_1_0.htm", 
                    "club_entry_1080_14_0_1_0.htm", "club_entry_1123_14_0_1_0.htm", "club_entry_93_14_0_1_0.htm", "club_entry_112_14_0_1_0.htm", "club_entry_118_14_0_1_0.htm", "club_entry_119_14_0_1_0.htm", 
                    "club_entry_126_14_0_1_0.htm", "club_entry_159_14_0_1_0.htm", "club_entry_164_14_0_1_0.htm", "club_entry_69_14_0_1_0.htm", "club_entry_1021_14_0_1_0.htm", "club_entry_1089_14_0_1_0.htm", 
                    "club_entry_1091_14_0_1_0.htm", "club_entry_1116_14_0_1_0.htm", "club_entry_1122_14_0_1_0.htm", "club_entry_3_14_0_1_0.htm", "club_entry_111_14_0_1_0.htm", "club_entry_1012_14_0_1_0.htm", 
                    "club_entry_1029_14_0_1_0.htm", "club_entry_73_14_0_1_0.htm", "club_entry_1092_14_0_1_0.htm", "club_entry_1137_14_0_1_0.htm", "club_entry_1090_14_0_1_0.htm", "club_entry_1019_14_0_1_0.htm", 
                    "club_entry_1050_14_0_1_0.htm", "club_entry_114_14_0_1_0.htm", "club_entry_1034_14_0_1_0.htm", "club_entry_1110_14_0_1_0.htm", "club_entry_1134_14_0_1_0.htm", "club_entry_143_14_0_1_0.htm", 
                    "club_entry_1135_14_0_1_0.htm", "club_entry_152_14_0_1_0.htm", "club_entry_1164_14_0_1_0.htm", "club_entry_1174_14_0_1_0.htm", "club_entry_1175_14_0_1_0.htm", "club_entry_1020_14_0_1_0.htm", 
                    "club_entry_1182_14_0_1_0.htm", "club_entry_1194_14_0_1_0.htm", "club_entry_1197_14_0_1_0.htm", "club_entry_1198_14_0_1_0.htm", "club_entry_44_15_0_1_0.htm", "club_entry_2_15_0_1_0.htm", 
                    "club_entry_135_15_0_1_0.htm", "club_entry_99_15_0_1_0.htm", "club_entry_91_15_0_1_0.htm", "club_entry_100_15_0_1_0.htm", "club_entry_101_15_0_1_0.htm", "club_entry_102_15_0_1_0.htm", 
                    "club_entry_103_15_0_1_0.htm", "club_entry_105_15_0_1_0.htm", "club_entry_106_15_0_1_0.htm", "club_entry_110_15_0_1_0.htm", "club_entry_115_15_0_1_0.htm", "club_entry_136_15_0_1_0.htm", 
                    "club_entry_137_15_0_1_0.htm", "club_entry_138_15_0_1_0.htm", "club_entry_139_15_0_1_0.htm", "club_entry_140_15_0_1_0.htm", "club_entry_30_16_0_1_0.htm", "club_entry_68_16_0_1_0.htm", 
                    "club_entry_87_16_0_1_0.htm", "club_entry_1093_16_0_1_0.htm"};

                string strOut;
                string strURL = "http://club.qingdaonews.com/club_entry_39_2_0_1_0.htm";
                ShowClubList(strURL, strCookieSessionID, strCookieUserName, strCookiePassword, out strOut);
                //MessageBox.Show(strOut);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 检索论坛信息
        /// <summary>
        /// 检索论坛信息
        /// </summary>
        /// <param name="strURL"></param>
        /// <param name="strCookieSessionID"></param>
        /// <param name="strCookieUserName"></param>
        /// <param name="strCookiePassword"></param>
        /// <param name="strShowBox"></param>
        public void ShowClubList(string strURL, string strCookieSessionID, string strCookieUserName, string strCookiePassword, out string strShowBox)
        {
            toolStripStatusLabel1.Text = "正在请求：" + strURL;
            try
            {
                //定义Cookie容器
                CookieContainer CookieArray = new CookieContainer();

                //创建Http请求
                HttpWebRequest LoginHttpWebRequest = (HttpWebRequest)WebRequest.Create(strURL);

                //登录数据
                //string LoginData = "id=" + strUserName + "&passwd=" + strPassword + "&usertype=0";
                //数据被传输类型
                //LoginHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //数据长度
                //LoginHttpWebRequest.ContentLength = LoginData.Length;
                //数据传输方法 get或post
                LoginHttpWebRequest.Method = "POST";
                LoginHttpWebRequest.Accept = "*/*";
                LoginHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; MAXTHON 2.0)";
                //设置HttpWebRequest的CookieContainer为刚才建立的那个CookieArray  
                LoginHttpWebRequest.CookieContainer = CookieArray;
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("PHPSESSID", strCookieSessionID));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[username]", strCookieUserName));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[password]", strCookiePassword));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews", ""));

                //获取登录数据流
                Stream myRequestStream = LoginHttpWebRequest.GetRequestStream();
                //StreamWriter
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                //把数据写入HttpWebRequest的Request流  
                //myStreamWriter.Write(LoginData);

                //关闭打开对象     
                //myStreamWriter.Close();

                myRequestStream.Close();

                //新建一个HttpWebResponse     
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)LoginHttpWebRequest.GetResponse();

                //获取一个包含url的Cookie集合的CookieCollection     
                myHttpWebResponse.Cookies = CookieArray.GetCookies(LoginHttpWebRequest.RequestUri);

                if (myHttpWebResponse.Cookies.Count > 0)
                {
                    CookieArray.Add(myHttpWebResponse.Cookies);
                }
                string strList_display_time = myHttpWebResponse.Cookies[0].Value.ToString();

                WebHeaderCollection a = myHttpWebResponse.Headers;

                Stream myResponseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("gb2312"));

                strShowBox = myStreamReader.ReadToEnd();

                //把数据从HttpWebResponse的Response流中读出     
                myStreamReader.Close();

                myResponseStream.Close();

                try
                {
                    Regex regexObj = new Regex(@"<a href=""\w{12,30}\.htm"">[\u4e00-\u9fa5, ,0-9,\uFF00-\uFFFF,\w]{4,5}</a>");
                    Match matchResults = regexObj.Match(strShowBox);
                    while (matchResults.Success)
                    {
                        for (int i = 1; i < matchResults.Groups.Count; i++)
                        {
                            Group groupObj = matchResults.Groups[i];
                            if (groupObj.Success)
                            {
                                //matched groupObj.Value.ToString();
                                // matched text: groupObj.Value
                                // match start: groupObj.Index
                                // match length: groupObj.Length
                            }
                        }
                        matchResults = matchResults.NextMatch();

                        string resultString_0 = null;
                        string resultString_1 = null;
                        try
                        {
                            resultString_0 = Regex.Match(matchResults.ToString(), @"club_entry_\d{1,9}_\d{1,9}_\d{1,9}_\d{1,9}_\d{1,9}\.htm").Value;
                            resultString_1 = Regex.Match(matchResults.ToString(), @">[\u4e00-\u9fa5,\uFF00-\uFFFF, ,0-9,\w]{1,5}<").Value;
                            ListViewItem li = new ListViewItem();
                            li.SubItems.Clear();
                            li.SubItems[0].Text = resultString_0;
                            li.SubItems.Add(resultString_1);
                            listView1.Items.Add(li);
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }                        
                    }
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                try
                {
                    int num = 1;
                    Regex regexObj_1 = new Regex(@"showAnnounce_\d{1,9}_\d{1,9}_1_\d{1,9}\.htm");
                    Match matchResults = regexObj_1.Match(strShowBox);                    
                    while (matchResults.Success)
                    {
                        for (int i = 1; i < matchResults.Groups.Count; i++)
                        {
                            Group groupObj = matchResults.Groups[i];
                            if (groupObj.Success)
                            {
                                //matched groupObj.Value.ToString();
                                // matched text: groupObj.Value
                                // match start: groupObj.Index
                                // match length: groupObj.Length
                            }
                        }
                        matchResults = matchResults.NextMatch();                        
                        ListViewItem li = new ListViewItem();
                        li.SubItems.Clear();
                        li.SubItems[0].Text = num.ToString();
                        li.SubItems.Add(matchResults.Value.ToString());
                        listView2.Items.Add(li);
                        num++;
                    }
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                int intLV1 = listView1.Items.Count;
                int intLV2 = listView2.Items.Count;

                listView1.Items[intLV1 - 1].Remove();
                listView2.Items[intLV2 - 1].Remove();

                toolStripStatusLabel1.Text = "正在访问：" + strURL;

                Random ran = new Random();
                int RanKey = ran.Next(0, listView2.Items.Count);

                //showAnnounce_2_4576914_1_0.htm
                string strAnnounceID = listView2.Items[RanKey].SubItems[1].Text.ToString();
                string B_1 = "";
                string T_1 = "";
                try
                {
                    B_1 = Regex.Match(strAnnounceID, @"e_\d{1,9}").Value.ToString();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                try
                {
                    T_1 = Regex.Match(strAnnounceID, @"\d{1,9}_1").Value.ToString();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                string strURL_1 = "http://club.qingdaonews.com/SaveReAnnounce_static.php";
                string strBoard_id = B_1.Replace("e_", "");
                string strTopic_id = T_1.Replace("_1", "");
                string strMessages = "路过打酱油的说。";
                string strOut;
                SendMessage(strURL_1, strAnnounceID, strBoard_id, strTopic_id, strMessages, textUserName.Text.ToString().Trim(), strCookieSessionID, strCookieUserName, strCookiePassword, strList_display_time,out strOut);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion       

        #region 发贴流程
        /// <summary>
        /// 发贴流程
        /// </summary>
        /// <param name="strURL"></param>
        /// <param name="strAnnounceID"></param>
        /// <param name="strBoard_id"></param>
        /// <param name="strTopic_id"></param>
        /// <param name="strMessages"></param>
        /// <param name="strUserName"></param>
        /// <param name="strCookieSessionID"></param>
        /// <param name="strCookieUserName"></param>
        /// <param name="strCookiePassword"></param>
        /// <param name="strList_display_time"></param>
        /// <param name="strShowBox"></param>
        public void SendMessage(string strURL, string strAnnounceID, string strBoard_id, string strTopic_id, string strMessages, string strUserName, string strCookieSessionID, string strCookieUserName, string strCookiePassword, string strList_display_time, out string strShowBox)
        {
            toolStripStatusLabel1.Text = "正在请求：http://club.qingdaonews.com/" + strAnnounceID;
            try
            {
                string strParent_id = strTopic_id; 

                //定义Cookie容器
                CookieContainer CookieArray = new CookieContainer();

                //创建Http请求
                HttpWebRequest LoginHttpWebRequest = (HttpWebRequest)WebRequest.Create(strURL);

                //数据格式
                //viewmode=
                //topic_id=3807645
                //parent_id=3807645
                //board_id=138
                //Page=1
                //a_name=snoopy6973
                //subject=%BB%D8%B8%B4%3A
                //ubb=UBB
                //chkSignature=1
                //body=好吧，我也是路过打酱油的。
                //insertimg=


                //登录数据
                //string LoginData = "viewmode=&topic_id=" + strTopic_id + "&parent_id=" + strParent_id + "&board_id=" + strBoard_id + "&Page=1&a_name=" + strUserName + "&subject=%BB%D8%B8%B4%3A&ubb=UBB&chkSignature=1&body=" + strMessages + "&insertimg=";
                string LoginData = "viewmode=&topic_id=" + strTopic_id + "&parent_id=" + strParent_id + "&board_id=" + strBoard_id + "&Page=1&a_name=" + strUserName + "&subject=%BB%D8%B8%B4%3A&ubb=UBB&chkSignature=1&body=%7B201%7D&insertimg=";
                //数据被传输类型
                LoginHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //数据长度
                LoginHttpWebRequest.ContentLength = LoginData.Length;
                //数据传输方法 get或post
                LoginHttpWebRequest.Method = "POST";
                LoginHttpWebRequest.Accept = "*/*";
                LoginHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; MAXTHON 2.0)";                
                LoginHttpWebRequest.Referer = "http://club.qingdaonews.com/" + strAnnounceID;
                //LoginHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
                //设置HttpWebRequest的CookieContainer为刚才建立的那个CookieArray  
                LoginHttpWebRequest.CookieContainer = CookieArray;
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("PHPSESSID", strCookieSessionID));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[username]", strCookieUserName));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[password]", strCookiePassword));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("list_display_time", strList_display_time));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("hiido_tod", "22"));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("hiido_ui", "0.9752054967479169"));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("hiido_lv", "1258877197212"));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("hiido_ti", "1258877197216"));

                //hiido_tod=22
                //hiido_ui=0.9752054967479169
                //hiido_lv=1258877197212
                //hiido_ti=1258877197216

                //获取登录数据流
                Stream myRequestStream = LoginHttpWebRequest.GetRequestStream();
                //StreamWriter
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                //把数据写入HttpWebRequest的Request流  
                myStreamWriter.Write(LoginData);

                //关闭打开对象     
                myStreamWriter.Close();

                myRequestStream.Close();

                //新建一个HttpWebResponse     
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)LoginHttpWebRequest.GetResponse();

                //获取一个包含url的Cookie集合的CookieCollection     
                myHttpWebResponse.Cookies = CookieArray.GetCookies(LoginHttpWebRequest.RequestUri);

                if (myHttpWebResponse.Cookies.Count > 0)
                {
                    CookieArray.Add(myHttpWebResponse.Cookies);
                }

                WebHeaderCollection a = myHttpWebResponse.Headers;

                Stream myResponseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("gb2312"));

                strShowBox = myStreamReader.ReadToEnd();

                //把数据从HttpWebResponse的Response流中读出     
                myStreamReader.Close();

                myResponseStream.Close();
                //========================================================================
                //http://club.qingdaonews.com/showAnnounce.php?board_id=2&topic_id=4577258


                toolStripStatusLabel1.Text = "发贴成功：http://club.qingdaonews.com/" + strAnnounceID;
                MessageBox.Show("发贴成功：http://club.qingdaonews.com/" + strAnnounceID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}