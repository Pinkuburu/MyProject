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
using System.Collections;

namespace QDBBS_SendMessage
{
    public partial class Form2 : Form
    {
        public int intSendCount = 1;   //发贴统计
        public Random ran = new Random();   //公用随机函数
        public int intTempNum = 0;
       

        public Form2()
        {
            InitializeComponent();
            listView1.MultiSelect = false;
            //Application.Exit();
            //toolStripStatusLabel1.Text = "";
            //listView1.Items.Add("asdfasdfasdF");
            //listView1.Items.Add("asdfasdfasdF");
            //listView1.Items.Add("asdfasdfasdF");
            //listView1.Items.Add("asdfasdfasdF");
            //InitializeListView();
            //string filePath = Path.GetFullPath(@"demo.ini");
            //INIClass ic = new INIClass(filePath);
            //ic.IniWriteValue("TEST", "name", "cupid");
            //ic.IniWriteValue("TEST", "name1", "cupid1");
            //ic.IniWriteValue("TEST", "name2", "cupid2");
            //ic.IniWriteValue("TEST", "name3", "cupid3");
            //ic.IniWriteValue("", "name4", "cupid4");
        }
        
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string[] arrClubList = {"club_entry_2_2_0_1_0.htm", "club_entry_1025_2_0_1_0.htm", "club_entry_48_2_0_1_0.htm", "club_entry_67_2_0_1_0.htm", "club_entry_57_2_0_1_0.htm", 
            //        "club_entry_9_2_0_1_0.htm", "club_entry_1038_2_0_1_0.htm", "club_entry_88_2_0_1_0.htm", "club_entry_123_2_0_1_0.htm", "club_entry_1_2_0_1_0.htm", "club_entry_156_2_0_1_0.htm", 
            //        "club_entry_1018_2_0_1_0.htm", "club_entry_1023_2_0_1_0.htm", "club_entry_1024_2_0_1_0.htm", "club_entry_1115_2_0_1_0.htm", "club_entry_1133_2_0_1_0.htm", "club_entry_41_2_0_1_0.htm", 
            //        "club_entry_1030_2_0_1_0.htm", "club_entry_1170_2_0_1_0.htm", "club_entry_1171_2_0_1_0.htm", "club_entry_1181_2_0_1_0.htm", "club_entry_1188_2_0_1_0.htm", "club_entry_1192_2_0_1_0.htm", 
            //        "club_entry_1199_2_0_1_0.htm", "club_entry_128_3_0_1_0.htm", "club_entry_129_3_0_1_0.htm", "club_entry_130_3_0_1_0.htm", "club_entry_131_3_0_1_0.htm", "club_entry_132_3_0_1_0.htm", 
            //        "club_entry_133_3_0_1_0.htm", "club_entry_134_3_0_1_0.htm", "club_entry_173_3_0_1_0.htm", "club_entry_1053_3_0_1_0.htm", "club_entry_1054_3_0_1_0.htm", "club_entry_1059_3_0_1_0.htm", 
            //        "club_entry_1060_3_0_1_0.htm", "club_entry_39_4_0_1_0.htm", "club_entry_1039_4_0_1_0.htm", "club_entry_1040_4_0_1_0.htm", "club_entry_1041_4_0_1_0.htm", "club_entry_1042_4_0_1_0.htm", 
            //        "club_entry_1043_4_0_1_0.htm", "club_entry_1044_4_0_1_0.htm", "club_entry_1045_4_0_1_0.htm", "club_entry_1046_4_0_1_0.htm", "club_entry_1047_4_0_1_0.htm", "club_entry_1048_4_0_1_0.htm", 
            //        "club_entry_1049_4_0_1_0.htm", "club_entry_1052_4_0_1_0.htm", "club_entry_1055_4_0_1_0.htm", "club_entry_1056_4_0_1_0.htm", "club_entry_1057_4_0_1_0.htm", "club_entry_1061_4_0_1_0.htm", 
            //        "club_entry_1062_4_0_1_0.htm", "club_entry_1063_4_0_1_0.htm", "club_entry_1069_4_0_1_0.htm", "club_entry_1070_4_0_1_0.htm", "club_entry_1071_4_0_1_0.htm", "club_entry_154_4_0_1_0.htm", 
            //        "club_entry_1085_4_0_1_0.htm", "club_entry_1086_4_0_1_0.htm", "club_entry_1179_4_0_1_0.htm", "club_entry_1102_4_0_1_0.htm", "club_entry_1108_4_0_1_0.htm", "club_entry_1109_4_0_1_0.htm", 
            //        "club_entry_1111_4_0_1_0.htm", "club_entry_1121_4_0_1_0.htm", "club_entry_1124_4_0_1_0.htm", "club_entry_1130_4_0_1_0.htm", "club_entry_1131_4_0_1_0.htm", "club_entry_1143_4_0_1_0.htm", 
            //        "club_entry_1157_4_0_1_0.htm", "club_entry_1158_4_0_1_0.htm", "club_entry_1159_4_0_1_0.htm", "club_entry_1169_4_0_1_0.htm", "club_entry_1173_4_0_1_0.htm", "club_entry_1184_4_0_1_0.htm", 
            //        "club_entry_1185_4_0_1_0.htm", "club_entry_1190_4_0_1_0.htm", "club_entry_1172_4_0_1_0.htm", "club_entry_20_5_0_1_0.htm", "club_entry_1010_5_0_1_0.htm", "club_entry_49_5_0_1_0.htm", 
            //        "club_entry_1139_5_0_1_0.htm", "club_entry_1115_6_0_1_0.htm", "club_entry_47_6_0_1_0.htm", "club_entry_66_6_0_1_0.htm", "club_entry_12_6_0_1_0.htm", "club_entry_86_6_0_1_0.htm", 
            //        "club_entry_163_6_0_1_0.htm", "club_entry_13_6_0_1_0.htm", "club_entry_94_6_0_1_0.htm", "club_entry_83_6_0_1_0.htm", "club_entry_174_6_0_1_0.htm", "club_entry_175_6_0_1_0.htm", 
            //        "club_entry_176_6_0_1_0.htm", "club_entry_177_6_0_1_0.htm", "club_entry_1006_6_0_1_0.htm", "club_entry_1007_6_0_1_0.htm", "club_entry_1011_6_0_1_0.htm", "club_entry_1031_6_0_1_0.htm", 
            //        "club_entry_58_7_0_1_0.htm", "club_entry_1005_7_0_1_0.htm", "club_entry_160_7_0_1_0.htm", "club_entry_121_7_0_1_0.htm", "club_entry_1099_7_0_1_0.htm", "club_entry_1100_7_0_1_0.htm", 
            //        "club_entry_64_8_0_1_0.htm", "club_entry_1064_8_0_1_0.htm", "club_entry_1094_8_0_1_0.htm", "club_entry_1127_8_0_1_0.htm", "club_entry_1128_8_0_1_0.htm", "club_entry_1142_8_0_1_0.htm", 
            //        "club_entry_1201_8_0_1_0.htm", "club_entry_4_9_0_1_0.htm", "club_entry_5_9_0_1_0.htm", "club_entry_36_9_0_1_0.htm", "club_entry_52_9_0_1_0.htm", "club_entry_74_9_0_1_0.htm", 
            //        "club_entry_76_9_0_1_0.htm", "club_entry_92_9_0_1_0.htm", "club_entry_124_9_0_1_0.htm", "club_entry_125_9_0_1_0.htm", "club_entry_146_9_0_1_0.htm", "club_entry_162_9_0_1_0.htm", 
            //        "club_entry_1015_9_0_1_0.htm", "club_entry_1016_9_0_1_0.htm", "club_entry_1033_9_0_1_0.htm", "club_entry_1068_9_0_1_0.htm", "club_entry_1084_9_0_1_0.htm", "club_entry_1125_9_0_1_0.htm", 
            //        "club_entry_1151_9_0_1_0.htm", "club_entry_33_10_0_1_0.htm", "club_entry_93_10_0_1_0.htm", "club_entry_77_10_0_1_0.htm", "club_entry_1072_10_0_1_0.htm", "club_entry_1073_10_0_1_0.htm", 
            //        "club_entry_1027_10_0_1_0.htm", "club_entry_1066_10_0_1_0.htm", "club_entry_1077_10_0_1_0.htm", "club_entry_1101_10_0_1_0.htm", "club_entry_1117_10_0_1_0.htm", "club_entry_1189_10_0_1_0.htm", 
            //        "club_entry_40_11_0_1_0.htm", "club_entry_1026_11_0_1_0.htm", "club_entry_24_11_0_1_0.htm", "club_entry_43_11_0_1_0.htm", "club_entry_54_11_0_1_0.htm", "club_entry_70_11_0_1_0.htm", 
            //        "club_entry_72_11_0_1_0.htm", "club_entry_81_11_0_1_0.htm", "club_entry_84_11_0_1_0.htm", "club_entry_122_11_0_1_0.htm", "club_entry_108_11_0_1_0.htm", "club_entry_22_11_0_1_0.htm", 
            //        "club_entry_61_11_0_1_0.htm", "club_entry_8_11_0_1_0.htm", "club_entry_42_11_0_1_0.htm", "club_entry_1126_11_0_1_0.htm", "club_entry_1129_11_0_1_0.htm", "club_entry_1156_11_0_1_0.htm", 
            //        "club_entry_145_11_0_1_0.htm", "club_entry_107_11_0_1_0.htm", "club_entry_144_11_0_1_0.htm", "club_entry_1191_11_0_1_0.htm", "club_entry_1144_11_0_1_0.htm", "club_entry_7_12_0_1_0.htm", 
            //        "club_entry_27_12_0_1_0.htm", "club_entry_1193_12_0_1_0.htm", "club_entry_1183_12_0_1_0.htm", "club_entry_113_12_0_1_0.htm", "club_entry_46_12_0_1_0.htm", "club_entry_60_12_0_1_0.htm", 
            //        "club_entry_1187_12_0_1_0.htm", "club_entry_82_12_0_1_0.htm", "club_entry_98_12_0_1_0.htm", "club_entry_1168_12_0_1_0.htm", "club_entry_1180_12_0_1_0.htm", "club_entry_1075_12_0_1_0.htm", 
            //        "club_entry_1078_12_0_1_0.htm", "club_entry_1087_12_0_1_0.htm", "club_entry_38_12_0_1_0.htm", "club_entry_141_12_0_1_0.htm", "club_entry_96_12_0_1_0.htm", "club_entry_97_12_0_1_0.htm", 
            //        "club_entry_109_12_0_1_0.htm", "club_entry_1032_12_0_1_0.htm", "club_entry_147_12_0_1_0.htm", "club_entry_1013_12_0_1_0.htm", "club_entry_1149_12_0_1_0.htm", "club_entry_149_12_0_1_0.htm", 
            //        "club_entry_1165_12_0_1_0.htm", "club_entry_1200_12_0_1_0.htm", "club_entry_26_17_0_1_0.htm", "club_entry_1067_17_0_1_0.htm", "club_entry_120_17_0_1_0.htm", "club_entry_1096_17_0_1_0.htm", 
            //        "club_entry_1020_17_0_1_0.htm", "club_entry_1145_17_0_1_0.htm", "club_entry_1160_17_0_1_0.htm", "club_entry_71_13_0_1_0.htm", "club_entry_166_13_0_1_0.htm", "club_entry_167_13_0_1_0.htm", 
            //        "club_entry_168_13_0_1_0.htm", "club_entry_169_13_0_1_0.htm", "club_entry_170_13_0_1_0.htm", "club_entry_165_13_0_1_0.htm", "club_entry_172_13_0_1_0.htm", "club_entry_1018_13_0_1_0.htm", 
            //        "club_entry_1030_13_0_1_0.htm", "club_entry_1076_13_0_1_0.htm", "club_entry_1082_13_0_1_0.htm", "club_entry_47_14_0_1_0.htm", "club_entry_29_14_0_1_0.htm", "club_entry_37_14_0_1_0.htm", 
            //        "club_entry_1080_14_0_1_0.htm", "club_entry_1123_14_0_1_0.htm", "club_entry_93_14_0_1_0.htm", "club_entry_112_14_0_1_0.htm", "club_entry_118_14_0_1_0.htm", "club_entry_119_14_0_1_0.htm", 
            //        "club_entry_126_14_0_1_0.htm", "club_entry_159_14_0_1_0.htm", "club_entry_164_14_0_1_0.htm", "club_entry_69_14_0_1_0.htm", "club_entry_1021_14_0_1_0.htm", "club_entry_1089_14_0_1_0.htm", 
            //        "club_entry_1091_14_0_1_0.htm", "club_entry_1116_14_0_1_0.htm", "club_entry_1122_14_0_1_0.htm", "club_entry_3_14_0_1_0.htm", "club_entry_111_14_0_1_0.htm", "club_entry_1012_14_0_1_0.htm", 
            //        "club_entry_1029_14_0_1_0.htm", "club_entry_73_14_0_1_0.htm", "club_entry_1092_14_0_1_0.htm", "club_entry_1137_14_0_1_0.htm", "club_entry_1090_14_0_1_0.htm", "club_entry_1019_14_0_1_0.htm", 
            //        "club_entry_1050_14_0_1_0.htm", "club_entry_114_14_0_1_0.htm", "club_entry_1034_14_0_1_0.htm", "club_entry_1110_14_0_1_0.htm", "club_entry_1134_14_0_1_0.htm", "club_entry_143_14_0_1_0.htm", 
            //        "club_entry_1135_14_0_1_0.htm", "club_entry_152_14_0_1_0.htm", "club_entry_1164_14_0_1_0.htm", "club_entry_1174_14_0_1_0.htm", "club_entry_1175_14_0_1_0.htm", "club_entry_1020_14_0_1_0.htm", 
            //        "club_entry_1182_14_0_1_0.htm", "club_entry_1194_14_0_1_0.htm", "club_entry_1197_14_0_1_0.htm", "club_entry_1198_14_0_1_0.htm", "club_entry_44_15_0_1_0.htm", "club_entry_2_15_0_1_0.htm", 
            //        "club_entry_135_15_0_1_0.htm", "club_entry_99_15_0_1_0.htm", "club_entry_91_15_0_1_0.htm", "club_entry_100_15_0_1_0.htm", "club_entry_101_15_0_1_0.htm", "club_entry_102_15_0_1_0.htm", 
            //        "club_entry_103_15_0_1_0.htm", "club_entry_105_15_0_1_0.htm", "club_entry_106_15_0_1_0.htm", "club_entry_110_15_0_1_0.htm", "club_entry_115_15_0_1_0.htm", "club_entry_136_15_0_1_0.htm", 
            //        "club_entry_137_15_0_1_0.htm", "club_entry_138_15_0_1_0.htm", "club_entry_139_15_0_1_0.htm", "club_entry_140_15_0_1_0.htm", "club_entry_30_16_0_1_0.htm", "club_entry_68_16_0_1_0.htm", 
            //        "club_entry_87_16_0_1_0.htm", "club_entry_1093_16_0_1_0.htm"};

            //Random ran = new Random();
            //int RandKey = ran.Next(1, arrClubList.Length);
            //label1.Text = arrClubList[RandKey].ToString();
        }

        public void LoadParameter(string strUserName, string strPassword, string strLoginUrl)
        {
            ReadTxt();
            ReadContentTxt();
            LoadClubInfo();
            //string strOut = "";

            //Login(strUserName, strPassword, strLoginUrl, out strOut);

            #region 论坛数组
            string[,] arrBBS_Club = { { "club_entry_2_2_0_1_0.htm", "青岛论坛" }, { "club_entry_1025_2_0_1_0.htm", "社会万象" }, { "club_entry_48_2_0_1_0.htm", "外经外贸" }, 
                                    { "club_entry_67_2_0_1_0.htm", "漂在岛城" }, { "club_entry_57_2_0_1_0.htm", "城市档案" }, { "club_entry_9_2_0_1_0.htm", "移民留学" }, 
                                    { "club_entry_1038_2_0_1_0.htm", "发现青岛" }, { "club_entry_88_2_0_1_0.htm", "闲闲书话" }, { "club_entry_123_2_0_1_0.htm", "五月的风" }, 
                                    { "club_entry_1_2_0_1_0.htm", "谈天说地" }, { "club_entry_156_2_0_1_0.htm", "男婚女嫁" }, { "club_entry_1018_2_0_1_0.htm", "ＤＶ摄像" }, 
                                    { "club_entry_1023_2_0_1_0.htm", "青岛游子" }, { "club_entry_1024_2_0_1_0.htm", "同楼相约" }, { "club_entry_1115_2_0_1_0.htm", "教育论坛" }, 
                                    { "club_entry_1133_2_0_1_0.htm", "奢华品位" }, { "club_entry_41_2_0_1_0.htm", "人文良友" }, { "club_entry_1030_2_0_1_0.htm", "社区之星" }, 
                                    { "club_entry_1170_2_0_1_0.htm", "我爱打折" }, { "club_entry_1171_2_0_1_0.htm", "棋牌游戏" }, { "club_entry_1181_2_0_1_0.htm", "梦幻足球" }, 
                                    { "club_entry_1188_2_0_1_0.htm", "爱 团 购" }, { "club_entry_1192_2_0_1_0.htm", "３Ｇ世界" }, { "club_entry_1199_2_0_1_0.htm", "海 奥 卡" }, 
                                    { "club_entry_128_3_0_1_0.htm", "黄岛论坛" }, { "club_entry_129_3_0_1_0.htm", "城阳论坛" }, { "club_entry_130_3_0_1_0.htm", "即墨论坛" }, 
                                    { "club_entry_131_3_0_1_0.htm", "胶南论坛" }, { "club_entry_132_3_0_1_0.htm", "胶州论坛" }, { "club_entry_133_3_0_1_0.htm", "莱西论坛" }, 
                                    { "club_entry_134_3_0_1_0.htm", "平度论坛" }, { "club_entry_173_3_0_1_0.htm", "崂山论坛" }, { "club_entry_1053_3_0_1_0.htm", "四方论坛" }, 
                                    { "club_entry_1054_3_0_1_0.htm", "李沧论坛" }, { "club_entry_1059_3_0_1_0.htm", "市南论坛" }, { "club_entry_1060_3_0_1_0.htm", "市北论坛" }, 
                                    { "club_entry_39_4_0_1_0.htm", "车迷论坛" }, { "club_entry_1039_4_0_1_0.htm", "挑车购车" }, { "club_entry_1040_4_0_1_0.htm", "交通路况" }, 
                                    { "club_entry_1041_4_0_1_0.htm", "改装养护" }, { "club_entry_1042_4_0_1_0.htm", "二手交易" }, { "club_entry_1043_4_0_1_0.htm", "凯越车会" }, 
                                    { "club_entry_1044_4_0_1_0.htm", "赛欧车会" }, { "club_entry_1045_4_0_1_0.htm", "夏 友 会" }, { "club_entry_1046_4_0_1_0.htm", "尼桑车会" }, 
                                    { "club_entry_1047_4_0_1_0.htm", "大众车会" }, { "club_entry_1048_4_0_1_0.htm", "马自达会" }, { "club_entry_1049_4_0_1_0.htm", "千里马帮" }, 
                                    { "club_entry_1052_4_0_1_0.htm", "越野大队" }, { "club_entry_1055_4_0_1_0.htm", "狮 友 会" }, { "club_entry_1056_4_0_1_0.htm", "哈 宝 族" }, 
                                    { "club_entry_1057_4_0_1_0.htm", "菲友联盟" }, { "club_entry_1061_4_0_1_0.htm", "自驾旅游" }, { "club_entry_1062_4_0_1_0.htm", "雪铁龙族" }, 
                                    { "club_entry_1063_4_0_1_0.htm", "现代车会" }, { "club_entry_1069_4_0_1_0.htm", "青 奇 军" }, { "club_entry_1070_4_0_1_0.htm", "吉 团 军" }, 
                                    { "club_entry_1071_4_0_1_0.htm", "福特联盟" }, { "club_entry_154_4_0_1_0.htm", "顺 风 车" }, { "club_entry_1085_4_0_1_0.htm", "图 击 队" }, 
                                    { "club_entry_1086_4_0_1_0.htm", "同 乐 会" }, { "club_entry_1179_4_0_1_0.htm", "车险理赔" }, { "club_entry_1102_4_0_1_0.htm", "日报车界" }, 
                                    { "club_entry_1108_4_0_1_0.htm", "福克斯会" }, { "club_entry_1109_4_0_1_0.htm", "丰田车会" }, { "club_entry_1111_4_0_1_0.htm", "思 家 车" }, 
                                    { "club_entry_1121_4_0_1_0.htm", "东南车会" }, { "club_entry_1124_4_0_1_0.htm", "燕 友 会" }, { "club_entry_1130_4_0_1_0.htm", "起 亚 会" }, 
                                    { "club_entry_1131_4_0_1_0.htm", "迪 车 族" }, { "club_entry_1143_4_0_1_0.htm", "奥迪车会" }, { "club_entry_1157_4_0_1_0.htm", "80后车会" }, 
                                    { "club_entry_1158_4_0_1_0.htm", "君友联盟" }, { "club_entry_1159_4_0_1_0.htm", "速腾车会" }, { "club_entry_1169_4_0_1_0.htm", "广本车会" }, 
                                    { "club_entry_1173_4_0_1_0.htm", "龙卡汽车卡" }, { "club_entry_1184_4_0_1_0.htm", "驾校学车" }, { "club_entry_1185_4_0_1_0.htm", "奔 奔 族" }, 
                                    { "club_entry_1190_4_0_1_0.htm", "尊 荣 会" }, { "club_entry_1172_4_0_1_0.htm", "都市车圈" }, { "club_entry_20_5_0_1_0.htm", "青岛楼事" }, 
                                    { "club_entry_1010_5_0_1_0.htm", "地产营销" }, { "club_entry_49_5_0_1_0.htm", "家居在线" }, { "club_entry_1139_5_0_1_0.htm", "家具饰品" }, 
                                    { "club_entry_1115_6_0_1_0.htm", "教育论坛" }, { "club_entry_47_6_0_1_0.htm", "教师论坛" }, { "club_entry_66_6_0_1_0.htm", "韩文天地" }, 
                                    { "club_entry_12_6_0_1_0.htm", "英语世界" }, { "club_entry_86_6_0_1_0.htm", "日语学习" }, { "club_entry_163_6_0_1_0.htm", "法 语 角" }, 
                                    { "club_entry_13_6_0_1_0.htm", "中学校园" }, { "club_entry_94_6_0_1_0.htm", "大学时代" }, { "club_entry_83_6_0_1_0.htm", "考试交流" }, 
                                    { "club_entry_174_6_0_1_0.htm", "海洋大学" }, { "club_entry_175_6_0_1_0.htm", "青岛大学" }, { "club_entry_176_6_0_1_0.htm", "青岛科大" }, 
                                    { "club_entry_177_6_0_1_0.htm", "理工大学" }, { "club_entry_1006_6_0_1_0.htm", "山东科大" }, { "club_entry_1007_6_0_1_0.htm", "外贸学院" }, 
                                    { "club_entry_1011_6_0_1_0.htm", "石油大学" }, { "club_entry_1031_6_0_1_0.htm", "青岛农大" }, { "club_entry_58_7_0_1_0.htm", "育儿论坛" }, 
                                    { "club_entry_1005_7_0_1_0.htm", "早教论坛" }, { "club_entry_160_7_0_1_0.htm", "家有学童" }, { "club_entry_121_7_0_1_0.htm", "亲 子 园" }, 
                                    { "club_entry_1099_7_0_1_0.htm", "宝贝学艺" }, { "club_entry_1100_7_0_1_0.htm", "少儿科普" }, { "club_entry_64_8_0_1_0.htm", "投资理财" }, 
                                    { "club_entry_1064_8_0_1_0.htm", "谈股论金" }, { "club_entry_1094_8_0_1_0.htm", "大话基金" }, { "club_entry_1127_8_0_1_0.htm", "我家理财" },
                                    { "club_entry_1128_8_0_1_0.htm", "收藏鉴宝" }, { "club_entry_1142_8_0_1_0.htm", "保险之家" }, { "club_entry_1201_8_0_1_0.htm", "我 爱 卡" }, 
                                    { "club_entry_4_9_0_1_0.htm", "赛场纵横" }, { "club_entry_5_9_0_1_0.htm", "网话海牛" }, { "club_entry_36_9_0_1_0.htm", "64格论坛" }, 
                                    { "club_entry_52_9_0_1_0.htm", "欧陆烽火" }, { "club_entry_74_9_0_1_0.htm", "我爱篮球" }, { "club_entry_76_9_0_1_0.htm", "奥运论坛" }, 
                                    { "club_entry_92_9_0_1_0.htm", "围棋天地" }, { "club_entry_124_9_0_1_0.htm", "羽球世界" }, { "club_entry_125_9_0_1_0.htm", "滑浪风帆" }, 
                                    { "club_entry_146_9_0_1_0.htm", "快乐排球" }, { "club_entry_162_9_0_1_0.htm", "健 身 房" }, { "club_entry_1015_9_0_1_0.htm", "中国象棋" }, 
                                    { "club_entry_1016_9_0_1_0.htm", "以武会友" }, { "club_entry_1033_9_0_1_0.htm", "台球论坛" }, { "club_entry_1068_9_0_1_0.htm", "网球天地" }, 
                                    { "club_entry_1084_9_0_1_0.htm", "瑜伽男女" }, { "club_entry_1125_9_0_1_0.htm", "小球世界" }, { "club_entry_1151_9_0_1_0.htm", "中能足球" }, 
                                    { "club_entry_33_10_0_1_0.htm", "健康有约" }, { "club_entry_93_10_0_1_0.htm", "白衣使者" }, { "club_entry_77_10_0_1_0.htm", "心理咨询" }, 
                                    { "club_entry_1072_10_0_1_0.htm", "医疗器械 " }, { "club_entry_1073_10_0_1_0.htm", "求医问药" }, { "club_entry_1027_10_0_1_0.htm", "美容护肤" }, 
                                    { "club_entry_1066_10_0_1_0.htm", "减肥纤体 " }, { "club_entry_1077_10_0_1_0.htm", "两性话题 " }, { "club_entry_1101_10_0_1_0.htm", "男人女人 " }, 
                                    { "club_entry_1117_10_0_1_0.htm", "整形美容" }, { "club_entry_1189_10_0_1_0.htm", "民生开讲" }, { "club_entry_40_11_0_1_0.htm", "女性天地" }, 
                                    { "club_entry_1026_11_0_1_0.htm", "征婚交友" }, { "club_entry_24_11_0_1_0.htm", "单身男女" }, { "club_entry_43_11_0_1_0.htm", "围城内外" }, 
                                    { "club_entry_54_11_0_1_0.htm", "人到中年" }, { "club_entry_70_11_0_1_0.htm", "人生百味" }, { "club_entry_72_11_0_1_0.htm", "三十而立" }, 
                                    { "club_entry_81_11_0_1_0.htm", "男人频道" }, { "club_entry_84_11_0_1_0.htm", "缘来是你" }, { "club_entry_122_11_0_1_0.htm", "夕照霞光" }, 
                                    { "club_entry_108_11_0_1_0.htm", "蹉跎岁月" }, { "club_entry_22_11_0_1_0.htm", "宜人茶室" }, { "club_entry_61_11_0_1_0.htm", "书香文字" }, 
                                    { "club_entry_8_11_0_1_0.htm", "真情诉白" }, { "club_entry_42_11_0_1_0.htm", "文学原创" }, { "club_entry_1126_11_0_1_0.htm", "婆媳关系" }, 
                                    { "club_entry_1129_11_0_1_0.htm", "婚姻家庭" }, { "club_entry_1156_11_0_1_0.htm", "诗情画意" }, { "club_entry_145_11_0_1_0.htm", "武侠论谈" },
                                    { "club_entry_107_11_0_1_0.htm", "茶 文 化" }, { "club_entry_144_11_0_1_0.htm", "咬文嚼字" }, { "club_entry_1191_11_0_1_0.htm", "散文诗词" }, 
                                    { "club_entry_1144_11_0_1_0.htm", "绝对隐私" }, { "club_entry_7_12_0_1_0.htm", "笑话天地" }, { "club_entry_27_12_0_1_0.htm", "美食广场" }, 
                                    { "club_entry_1193_12_0_1_0.htm", "食全十美" }, { "club_entry_1183_12_0_1_0.htm", "嘻哈Ｋ歌" }, { "club_entry_113_12_0_1_0.htm", "漂亮女人" }, 
                                    { "club_entry_46_12_0_1_0.htm", "时尚主义" }, { "club_entry_60_12_0_1_0.htm", "动漫论坛" }, { "club_entry_1187_12_0_1_0.htm", "我要结婚" }, 
                                    { "club_entry_82_12_0_1_0.htm", "手工情结" }, { "club_entry_98_12_0_1_0.htm", "酒吧文化" }, { "club_entry_1168_12_0_1_0.htm", "啤酒论坛" }, 
                                    { "club_entry_1180_12_0_1_0.htm", "巧克力吧" }, { "club_entry_1075_12_0_1_0.htm", "吃喝玩乐" }, { "club_entry_1078_12_0_1_0.htm", "形象顾问" }, 
                                    { "club_entry_1087_12_0_1_0.htm", "音响世界" }, { "club_entry_38_12_0_1_0.htm", "音乐世界" }, { "club_entry_141_12_0_1_0.htm", "灌水乐园" }, 
                                    { "club_entry_96_12_0_1_0.htm", "手机乐园" }, { "club_entry_97_12_0_1_0.htm", "家有宠物" }, { "club_entry_109_12_0_1_0.htm", "花鸟鱼虫" }, 
                                    { "club_entry_1032_12_0_1_0.htm", "影视天堂" }, { "club_entry_147_12_0_1_0.htm", "占星奇缘" }, { "club_entry_1013_12_0_1_0.htm", "你问我答" }, 
                                    { "club_entry_1149_12_0_1_0.htm", "渔 乐 圈" }, { "club_entry_149_12_0_1_0.htm", "娱 乐 圈" }, { "club_entry_1165_12_0_1_0.htm", "舞蹈时尚" }, 
                                    { "club_entry_1200_12_0_1_0.htm", "天黑闭眼" }, { "club_entry_26_17_0_1_0.htm", "游山玩水" }, { "club_entry_1067_17_0_1_0.htm", "海奥旅游" }, 
                                    { "club_entry_120_17_0_1_0.htm", "游遍世界" }, { "club_entry_1096_17_0_1_0.htm", "走进大山" }, { "club_entry_1020_17_0_1_0.htm", "晚报旅游" }, 
                                    { "club_entry_1145_17_0_1_0.htm", "旅游DIY" }, { "club_entry_1160_17_0_1_0.htm", "玩 崂 山" }, { "club_entry_71_13_0_1_0.htm", "摄影园地" }, 
                                    { "club_entry_166_13_0_1_0.htm", "明星八卦" }, { "club_entry_167_13_0_1_0.htm", "搞笑图库" }, { "club_entry_168_13_0_1_0.htm", "酷图超市" }, 
                                    { "club_entry_169_13_0_1_0.htm", "风景美图" }, { "club_entry_170_13_0_1_0.htm", "体育图片" }, { "club_entry_165_13_0_1_0.htm", "美女贴图" }, 
                                    { "club_entry_172_13_0_1_0.htm", "闪客帝国" }, { "club_entry_1018_13_0_1_0.htm", "ＤＶ摄像" }, { "club_entry_1030_13_0_1_0.htm", "社区之星" }, 
                                    { "club_entry_1076_13_0_1_0.htm", "人体艺术" }, { "club_entry_1082_13_0_1_0.htm", "泳 装 秀" }, { "club_entry_47_14_0_1_0.htm", "教师论坛" }, 
                                    { "club_entry_29_14_0_1_0.htm", "记者之家" }, { "club_entry_37_14_0_1_0.htm", "法律论坛" }, { "club_entry_1080_14_0_1_0.htm", "会展天地" }, 
                                    { "club_entry_1123_14_0_1_0.htm", "律师咨询" }, { "club_entry_93_14_0_1_0.htm", "白衣使者" }, { "club_entry_112_14_0_1_0.htm", "广 告 人" }, 
                                    { "club_entry_118_14_0_1_0.htm", "营销精英" }, { "club_entry_119_14_0_1_0.htm", "程 序 员" }, { "club_entry_126_14_0_1_0.htm", "创业沙龙" }, 
                                    { "club_entry_159_14_0_1_0.htm", "设计空间" }, { "club_entry_164_14_0_1_0.htm", "票务论坛" }, { "club_entry_69_14_0_1_0.htm", "人才求职" }, 
                                    { "club_entry_1021_14_0_1_0.htm", "买卖驿站" }, { "club_entry_1089_14_0_1_0.htm", "特价机票" }, { "club_entry_1091_14_0_1_0.htm", "司法审判" }, 
                                    { "club_entry_1116_14_0_1_0.htm", "机票超市" }, { "club_entry_1122_14_0_1_0.htm", "工商税务" }, { "club_entry_3_14_0_1_0.htm", "电脑技术" }, 
                                    { "club_entry_111_14_0_1_0.htm", "网站建设" }, { "club_entry_1012_14_0_1_0.htm", "环保论坛" }, { "club_entry_1029_14_0_1_0.htm", "数码地带" }, 
                                    { "club_entry_73_14_0_1_0.htm", "个体私营" }, { "club_entry_1092_14_0_1_0.htm", "财务会计" }, { "club_entry_1137_14_0_1_0.htm", "高端品牌" }, 
                                    { "club_entry_1090_14_0_1_0.htm", "家电论坛" }, { "club_entry_1019_14_0_1_0.htm", "巴士公交" }, { "club_entry_1050_14_0_1_0.htm", "我是义工" }, 
                                    { "club_entry_114_14_0_1_0.htm", "广播电视" }, { "club_entry_1034_14_0_1_0.htm", "晚报生活" }, { "club_entry_1110_14_0_1_0.htm", "梦想剧场" }, 
                                    { "club_entry_1134_14_0_1_0.htm", "艺术青岛" }, { "club_entry_143_14_0_1_0.htm", "军事天地" }, { "club_entry_1135_14_0_1_0.htm", "知识宝典" }, 
                                    { "club_entry_152_14_0_1_0.htm", "苍穹探索" }, { "club_entry_1164_14_0_1_0.htm", "ＴＤ手机" }, { "club_entry_1174_14_0_1_0.htm", "珠宝玉器" }, 
                                    { "club_entry_1175_14_0_1_0.htm", "青岛婚庆" }, { "club_entry_1020_14_0_1_0.htm", "晚报旅游" }, { "club_entry_1182_14_0_1_0.htm", "岛城说法" }, 
                                    { "club_entry_1194_14_0_1_0.htm", "真情人间" }, { "club_entry_1197_14_0_1_0.htm", "电子开发" }, { "club_entry_1198_14_0_1_0.htm", "上 蛤 蜊" }, 
                                    { "club_entry_44_15_0_1_0.htm", "齐鲁论坛" }, { "club_entry_2_15_0_1_0.htm", "青岛论坛 " }, { "club_entry_135_15_0_1_0.htm", "济南论坛" }, 
                                    { "club_entry_99_15_0_1_0.htm", "烟台论坛" }, { "club_entry_91_15_0_1_0.htm", "淄博论坛" }, { "club_entry_100_15_0_1_0.htm", "潍坊论坛" }, 
                                    { "club_entry_101_15_0_1_0.htm", "威海论坛" }, { "club_entry_102_15_0_1_0.htm", "日照论坛" }, { "club_entry_103_15_0_1_0.htm", "济宁论坛" }, 
                                    { "club_entry_105_15_0_1_0.htm", "聊城论坛" }, { "club_entry_106_15_0_1_0.htm", "枣庄论坛" }, { "club_entry_110_15_0_1_0.htm", "泰安论坛" }, 
                                    { "club_entry_115_15_0_1_0.htm", "临沂论坛" }, { "club_entry_136_15_0_1_0.htm", "德州论坛" }, { "club_entry_137_15_0_1_0.htm", "东营论坛" }, 
                                    { "club_entry_138_15_0_1_0.htm", "滨州论坛" }, { "club_entry_139_15_0_1_0.htm", "菏泽论坛" }, { "club_entry_140_15_0_1_0.htm", "莱芜论坛" }, 
                                    { "club_entry_30_16_0_1_0.htm", "批评建议" }, { "club_entry_68_16_0_1_0.htm", "聊友建议" }, { "club_entry_87_16_0_1_0.htm", "版主之家" }, 
                                    { "club_entry_1093_16_0_1_0.htm", "博友之家" } };
            #endregion

            //MessageBox.Show(arrBBS_Club[0, 1].ToString());
            
            //li.SubItems.Add(resultString_1);
            //for (int i = 0; i < arrBBS_Club.Length / 2; i++)
            //{
            //    ListViewItem li = new ListViewItem();
            //    li.SubItems.Clear();
            //    li.SubItems[0].Text = arrBBS_Club[i,1].ToString();
            //    listView4.Items.Add(li);
            //}


            //test();
            //FindClubLink("Asdasd");
            //MessageBox.Show(listView3.Items[0].SubItems[0].Text + "," + listView3.Items[0].SubItems[1].Text);

        }

        #region 添加栏目列表
        public void btn_AddClub_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView4.SelectedItems)
            {
                if (listView4.SelectedIndices.Count > 0)
                {
                    ListViewItem li5 = new ListViewItem();
                    li5.SubItems.Clear();
                    li5.SubItems[0].Text = li.SubItems[0].Text;
                    li5.SubItems.Add(li.SubItems[1].Text);
                    listView5.Items.Add(li5);
                    li.Remove();
                }
                else
                {
                    break;
                }
                //MessageBox.Show(listView5.Items[0].SubItems[1].Text.Trim());
            }
        }
        #endregion

        #region 全选栏目列表
        private void btn_AllSelect_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView4.Items)
            {
                if (li.Selected == false)
                {
                    li.Selected = true;
                    li.BackColor = Color.CadetBlue;
                    li.ForeColor = Color.White;
                }
            }
        }
        #endregion

        #region 删除栏目列表
        private void btn_DelClub_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView5.SelectedItems)
            {
                if (listView5.SelectedIndices.Count > 0)
                {
                    ListViewItem li4 = new ListViewItem();
                    li4.SubItems.Clear();
                    li4.SubItems[0].Text = li.SubItems[0].Text;
                    li4.SubItems.Add(li.SubItems[1].Text);
                    listView4.Items.Add(li4);
                    li.Remove();
                }
                else
                {
                    break;
                }
            }
        }
        #endregion

        private void Timer_TimesUp(object sender, System.EventArgs e)
        {
            button2.PerformClick();
        }

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
                string LoginData = "id=" + UrlEncode(strUserName,"UTF-8") + "&passwd=" + strPassword + "&usertype=0";
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

                if (myHttpWebResponse.Cookies.Count > 0)    //自动添加Cookies
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

                if (Txt == "<meta http-equiv=Refresh content=0;URL=/>")
                {
                    //toolStripStatusLabel1.Text = "Login.OK";
                    //this.Hide();
                    //Form2 f2 = new Form2();
                    //f2.Show();
                    //textUserName.Visible = false;
                    //textPassword.Visible = false;
                    //button1.Visible = false;
                    //label1.Visible = false;
                    //label2.Visible = false;
                    //this.Width = 500;
                    //this.Height = 300;
                    //listView1.Visible = true;
                    //listView1.Width = 402;
                    //listView1.Height = 214;
                    //intSendCount = intSendCount - 1;
                    this.Text = "Sender - " + strUserName + "    共顶" + intSendCount + "贴";
                    string strOut;
                    LoginSec(strUserName, strCookieSessionID, strCookieUserName, strCookiePassowrd, out strOut);
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
        /// <param name="strUserName"></param>
        /// <param name="strCookieSessionID"></param>
        /// <param name="strCookieUserName"></param>
        /// <param name="strCookiePassword"></param>
        /// <param name="strShowBox"></param>
        public void LoginSec(string strUserName, string strCookieSessionID, string strCookieUserName, string strCookiePassword, out string strShowBox)
        {
            string strClubLink = "";
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

                #region
                //string[] arrClubList = {"club_entry_2_2_0_1_0.htm", "club_entry_1025_2_0_1_0.htm", "club_entry_48_2_0_1_0.htm", "club_entry_67_2_0_1_0.htm", "club_entry_57_2_0_1_0.htm", 
                //    "club_entry_9_2_0_1_0.htm", "club_entry_1038_2_0_1_0.htm", "club_entry_88_2_0_1_0.htm", "club_entry_123_2_0_1_0.htm", "club_entry_1_2_0_1_0.htm", "club_entry_156_2_0_1_0.htm", 
                //    "club_entry_1018_2_0_1_0.htm", "club_entry_1023_2_0_1_0.htm", "club_entry_1024_2_0_1_0.htm", "club_entry_1115_2_0_1_0.htm", "club_entry_1133_2_0_1_0.htm", "club_entry_41_2_0_1_0.htm", 
                //    "club_entry_1030_2_0_1_0.htm", "club_entry_1170_2_0_1_0.htm", "club_entry_1171_2_0_1_0.htm", "club_entry_1181_2_0_1_0.htm", "club_entry_1188_2_0_1_0.htm", "club_entry_1192_2_0_1_0.htm", 
                //    "club_entry_1199_2_0_1_0.htm", "club_entry_128_3_0_1_0.htm", "club_entry_129_3_0_1_0.htm", "club_entry_130_3_0_1_0.htm", "club_entry_131_3_0_1_0.htm", "club_entry_132_3_0_1_0.htm", 
                //    "club_entry_133_3_0_1_0.htm", "club_entry_134_3_0_1_0.htm", "club_entry_173_3_0_1_0.htm", "club_entry_1053_3_0_1_0.htm", "club_entry_1054_3_0_1_0.htm", "club_entry_1059_3_0_1_0.htm", 
                //    "club_entry_1060_3_0_1_0.htm", "club_entry_39_4_0_1_0.htm", "club_entry_1039_4_0_1_0.htm", "club_entry_1040_4_0_1_0.htm", "club_entry_1041_4_0_1_0.htm", "club_entry_1042_4_0_1_0.htm", 
                //    "club_entry_1043_4_0_1_0.htm", "club_entry_1044_4_0_1_0.htm", "club_entry_1045_4_0_1_0.htm", "club_entry_1046_4_0_1_0.htm", "club_entry_1047_4_0_1_0.htm", "club_entry_1048_4_0_1_0.htm", 
                //    "club_entry_1049_4_0_1_0.htm", "club_entry_1052_4_0_1_0.htm", "club_entry_1055_4_0_1_0.htm", "club_entry_1056_4_0_1_0.htm", "club_entry_1057_4_0_1_0.htm", "club_entry_1061_4_0_1_0.htm", 
                //    "club_entry_1062_4_0_1_0.htm", "club_entry_1063_4_0_1_0.htm", "club_entry_1069_4_0_1_0.htm", "club_entry_1070_4_0_1_0.htm", "club_entry_1071_4_0_1_0.htm", "club_entry_154_4_0_1_0.htm", 
                //    "club_entry_1085_4_0_1_0.htm", "club_entry_1086_4_0_1_0.htm", "club_entry_1179_4_0_1_0.htm", "club_entry_1102_4_0_1_0.htm", "club_entry_1108_4_0_1_0.htm", "club_entry_1109_4_0_1_0.htm", 
                //    "club_entry_1111_4_0_1_0.htm", "club_entry_1121_4_0_1_0.htm", "club_entry_1124_4_0_1_0.htm", "club_entry_1130_4_0_1_0.htm", "club_entry_1131_4_0_1_0.htm", "club_entry_1143_4_0_1_0.htm", 
                //    "club_entry_1157_4_0_1_0.htm", "club_entry_1158_4_0_1_0.htm", "club_entry_1159_4_0_1_0.htm", "club_entry_1169_4_0_1_0.htm", "club_entry_1173_4_0_1_0.htm", "club_entry_1184_4_0_1_0.htm", 
                //    "club_entry_1185_4_0_1_0.htm", "club_entry_1190_4_0_1_0.htm", "club_entry_1172_4_0_1_0.htm", "club_entry_20_5_0_1_0.htm", "club_entry_1010_5_0_1_0.htm", "club_entry_49_5_0_1_0.htm", 
                //    "club_entry_1139_5_0_1_0.htm", "club_entry_1115_6_0_1_0.htm", "club_entry_47_6_0_1_0.htm", "club_entry_66_6_0_1_0.htm", "club_entry_12_6_0_1_0.htm", "club_entry_86_6_0_1_0.htm", 
                //    "club_entry_163_6_0_1_0.htm", "club_entry_13_6_0_1_0.htm", "club_entry_94_6_0_1_0.htm", "club_entry_83_6_0_1_0.htm", "club_entry_174_6_0_1_0.htm", "club_entry_175_6_0_1_0.htm", 
                //    "club_entry_176_6_0_1_0.htm", "club_entry_177_6_0_1_0.htm", "club_entry_1006_6_0_1_0.htm", "club_entry_1007_6_0_1_0.htm", "club_entry_1011_6_0_1_0.htm", "club_entry_1031_6_0_1_0.htm", 
                //    "club_entry_58_7_0_1_0.htm", "club_entry_1005_7_0_1_0.htm", "club_entry_160_7_0_1_0.htm", "club_entry_121_7_0_1_0.htm", "club_entry_1099_7_0_1_0.htm", "club_entry_1100_7_0_1_0.htm", 
                //    "club_entry_64_8_0_1_0.htm", "club_entry_1064_8_0_1_0.htm", "club_entry_1094_8_0_1_0.htm", "club_entry_1127_8_0_1_0.htm", "club_entry_1128_8_0_1_0.htm", "club_entry_1142_8_0_1_0.htm", 
                //    "club_entry_1201_8_0_1_0.htm", "club_entry_4_9_0_1_0.htm", "club_entry_5_9_0_1_0.htm", "club_entry_36_9_0_1_0.htm", "club_entry_52_9_0_1_0.htm", "club_entry_74_9_0_1_0.htm", 
                //    "club_entry_76_9_0_1_0.htm", "club_entry_92_9_0_1_0.htm", "club_entry_124_9_0_1_0.htm", "club_entry_125_9_0_1_0.htm", "club_entry_146_9_0_1_0.htm", "club_entry_162_9_0_1_0.htm", 
                //    "club_entry_1015_9_0_1_0.htm", "club_entry_1016_9_0_1_0.htm", "club_entry_1033_9_0_1_0.htm", "club_entry_1068_9_0_1_0.htm", "club_entry_1084_9_0_1_0.htm", "club_entry_1125_9_0_1_0.htm", 
                //    "club_entry_1151_9_0_1_0.htm", "club_entry_33_10_0_1_0.htm", "club_entry_93_10_0_1_0.htm", "club_entry_77_10_0_1_0.htm", "club_entry_1072_10_0_1_0.htm", "club_entry_1073_10_0_1_0.htm", 
                //    "club_entry_1027_10_0_1_0.htm", "club_entry_1066_10_0_1_0.htm", "club_entry_1077_10_0_1_0.htm", "club_entry_1101_10_0_1_0.htm", "club_entry_1117_10_0_1_0.htm", "club_entry_1189_10_0_1_0.htm", 
                //    "club_entry_40_11_0_1_0.htm", "club_entry_1026_11_0_1_0.htm", "club_entry_24_11_0_1_0.htm", "club_entry_43_11_0_1_0.htm", "club_entry_54_11_0_1_0.htm", "club_entry_70_11_0_1_0.htm", 
                //    "club_entry_72_11_0_1_0.htm", "club_entry_81_11_0_1_0.htm", "club_entry_84_11_0_1_0.htm", "club_entry_122_11_0_1_0.htm", "club_entry_108_11_0_1_0.htm", "club_entry_22_11_0_1_0.htm", 
                //    "club_entry_61_11_0_1_0.htm", "club_entry_8_11_0_1_0.htm", "club_entry_42_11_0_1_0.htm", "club_entry_1126_11_0_1_0.htm", "club_entry_1129_11_0_1_0.htm", "club_entry_1156_11_0_1_0.htm", 
                //    "club_entry_145_11_0_1_0.htm", "club_entry_107_11_0_1_0.htm", "club_entry_144_11_0_1_0.htm", "club_entry_1191_11_0_1_0.htm", "club_entry_1144_11_0_1_0.htm", "club_entry_7_12_0_1_0.htm", 
                //    "club_entry_27_12_0_1_0.htm", "club_entry_1193_12_0_1_0.htm", "club_entry_1183_12_0_1_0.htm", "club_entry_113_12_0_1_0.htm", "club_entry_46_12_0_1_0.htm", "club_entry_60_12_0_1_0.htm", 
                //    "club_entry_1187_12_0_1_0.htm", "club_entry_82_12_0_1_0.htm", "club_entry_98_12_0_1_0.htm", "club_entry_1168_12_0_1_0.htm", "club_entry_1180_12_0_1_0.htm", "club_entry_1075_12_0_1_0.htm", 
                //    "club_entry_1078_12_0_1_0.htm", "club_entry_1087_12_0_1_0.htm", "club_entry_38_12_0_1_0.htm", "club_entry_141_12_0_1_0.htm", "club_entry_96_12_0_1_0.htm", "club_entry_97_12_0_1_0.htm", 
                //    "club_entry_109_12_0_1_0.htm", "club_entry_1032_12_0_1_0.htm", "club_entry_147_12_0_1_0.htm", "club_entry_1013_12_0_1_0.htm", "club_entry_1149_12_0_1_0.htm", "club_entry_149_12_0_1_0.htm", 
                //    "club_entry_1165_12_0_1_0.htm", "club_entry_1200_12_0_1_0.htm", "club_entry_26_17_0_1_0.htm", "club_entry_1067_17_0_1_0.htm", "club_entry_120_17_0_1_0.htm", "club_entry_1096_17_0_1_0.htm", 
                //    "club_entry_1020_17_0_1_0.htm", "club_entry_1145_17_0_1_0.htm", "club_entry_1160_17_0_1_0.htm", "club_entry_71_13_0_1_0.htm", "club_entry_166_13_0_1_0.htm", "club_entry_167_13_0_1_0.htm", 
                //    "club_entry_168_13_0_1_0.htm", "club_entry_169_13_0_1_0.htm", "club_entry_170_13_0_1_0.htm", "club_entry_165_13_0_1_0.htm", "club_entry_172_13_0_1_0.htm", "club_entry_1018_13_0_1_0.htm", 
                //    "club_entry_1030_13_0_1_0.htm", "club_entry_1076_13_0_1_0.htm", "club_entry_1082_13_0_1_0.htm", "club_entry_47_14_0_1_0.htm", "club_entry_29_14_0_1_0.htm", "club_entry_37_14_0_1_0.htm", 
                //    "club_entry_1080_14_0_1_0.htm", "club_entry_1123_14_0_1_0.htm", "club_entry_93_14_0_1_0.htm", "club_entry_112_14_0_1_0.htm", "club_entry_118_14_0_1_0.htm", "club_entry_119_14_0_1_0.htm", 
                //    "club_entry_126_14_0_1_0.htm", "club_entry_159_14_0_1_0.htm", "club_entry_164_14_0_1_0.htm", "club_entry_69_14_0_1_0.htm", "club_entry_1021_14_0_1_0.htm", "club_entry_1089_14_0_1_0.htm", 
                //    "club_entry_1091_14_0_1_0.htm", "club_entry_1116_14_0_1_0.htm", "club_entry_1122_14_0_1_0.htm", "club_entry_3_14_0_1_0.htm", "club_entry_111_14_0_1_0.htm", "club_entry_1012_14_0_1_0.htm", 
                //    "club_entry_1029_14_0_1_0.htm", "club_entry_73_14_0_1_0.htm", "club_entry_1092_14_0_1_0.htm", "club_entry_1137_14_0_1_0.htm", "club_entry_1090_14_0_1_0.htm", "club_entry_1019_14_0_1_0.htm", 
                //    "club_entry_1050_14_0_1_0.htm", "club_entry_114_14_0_1_0.htm", "club_entry_1034_14_0_1_0.htm", "club_entry_1110_14_0_1_0.htm", "club_entry_1134_14_0_1_0.htm", "club_entry_143_14_0_1_0.htm", 
                //    "club_entry_1135_14_0_1_0.htm", "club_entry_152_14_0_1_0.htm", "club_entry_1164_14_0_1_0.htm", "club_entry_1174_14_0_1_0.htm", "club_entry_1175_14_0_1_0.htm", "club_entry_1020_14_0_1_0.htm", 
                //    "club_entry_1182_14_0_1_0.htm", "club_entry_1194_14_0_1_0.htm", "club_entry_1197_14_0_1_0.htm", "club_entry_1198_14_0_1_0.htm", "club_entry_44_15_0_1_0.htm", "club_entry_2_15_0_1_0.htm", 
                //    "club_entry_135_15_0_1_0.htm", "club_entry_99_15_0_1_0.htm", "club_entry_91_15_0_1_0.htm", "club_entry_100_15_0_1_0.htm", "club_entry_101_15_0_1_0.htm", "club_entry_102_15_0_1_0.htm", 
                //    "club_entry_103_15_0_1_0.htm", "club_entry_105_15_0_1_0.htm", "club_entry_106_15_0_1_0.htm", "club_entry_110_15_0_1_0.htm", "club_entry_115_15_0_1_0.htm", "club_entry_136_15_0_1_0.htm", 
                //    "club_entry_137_15_0_1_0.htm", "club_entry_138_15_0_1_0.htm", "club_entry_139_15_0_1_0.htm", "club_entry_140_15_0_1_0.htm", "club_entry_30_16_0_1_0.htm", "club_entry_68_16_0_1_0.htm", 
                //    "club_entry_87_16_0_1_0.htm", "club_entry_1093_16_0_1_0.htm"};
                #endregion

                if (listView5.Items.Count == 0)
                {
                    int RandKey = ran.Next(0, listView4.Items.Count);
                    strClubLink = listView4.Items[RandKey].SubItems[1].Text.Trim();
                }
                else
                {
                    int RandKey = ran.Next(0, listView5.Items.Count);
                    strClubLink = listView5.Items[RandKey].SubItems[1].Text.Trim();
                }
               

                string strOut;
                string strURL = "http://club.qingdaonews.com/" + strClubLink;
                ShowClubList(strUserName, strURL, strCookieSessionID, strCookieUserName, strCookiePassword, out strOut);
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
        /// <param name="strUserName"></param>
        /// <param name="strURL"></param>
        /// <param name="strCookieSessionID"></param>
        /// <param name="strCookieUserName"></param>
        /// <param name="strCookiePassword"></param>
        /// <param name="strShowBox"></param>
        public void ShowClubList(string strUserName, string strURL, string strCookieSessionID, string strCookieUserName, string strCookiePassword, out string strShowBox)
        {
            ArrayList List = new ArrayList();
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

                #region 读取栏目链接和名称
                //try
                //{
                //    Regex regexObj = new Regex(@"<a href=""\w{12,30}\.htm"">[\u4e00-\u9fa5, ,0-9,\uFF00-\uFFFF,\w]{4,5}</a>");
                //    Match matchResults = regexObj.Match(strShowBox);
                //    while (matchResults.Success)
                //    {
                //        for (int i = 1; i < matchResults.Groups.Count; i++)
                //        {
                //            Group groupObj = matchResults.Groups[i];
                //            if (groupObj.Success)
                //            {
                //                //matched groupObj.Value.ToString();
                //                // matched text: groupObj.Value
                //                // match start: groupObj.Index
                //                // match length: groupObj.Length
                //            }
                //        }
                //        matchResults = matchResults.NextMatch();

                //        string resultString_0 = null;
                //        string resultString_1 = null;
                //        try
                //        {
                //            resultString_0 = Regex.Match(matchResults.ToString(), @"club_entry_\d{1,9}_\d{1,9}_\d{1,9}_\d{1,9}_\d{1,9}\.htm").Value;    //栏目链接 
                //            resultString_1 = Regex.Match(matchResults.ToString(), @">[\u4e00-\u9fa5,\uFF00-\uFFFF, ,0-9,\w]{1,5}<").Value;              //栏目名称
                //            ListViewItem li = new ListViewItem();
                //            li.SubItems.Clear();
                //            li.SubItems[0].Text = resultString_0;
                //            li.SubItems.Add(resultString_1);
                //            listView1.Items.Add(li);
                //        }
                //        catch (ArgumentException ex)
                //        {
                //            // Syntax error in the regular expression
                //        }
                //    }
                //}
                //catch (ArgumentException ex)
                //{
                //    // Syntax error in the regular expression
                //}
                #endregion

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

                    DataTable dt = new DataTable("Table_Announce");
                    dt.Columns.Add("Link", System.Type.GetType("System.String"));
                    if (matchResults.ToString().Trim() != "")
                    {
                        List.Add(matchResults);
                    }                   
                }


                string test = List[0].ToString();
                string count = List.Count.ToString();

                #region 读取帖子列表
                //try
                //{
                //    int num = 1;
                //    Regex regexObj_1 = new Regex(@"showAnnounce_\d{1,9}_\d{1,9}_1_\d{1,9}\.htm");
                //    Match matchResults = regexObj_1.Match(strShowBox);
                //    while (matchResults.Success)
                //    {
                //        for (int i = 1; i < matchResults.Groups.Count; i++)
                //        {
                //            Group groupObj = matchResults.Groups[i];
                //            if (groupObj.Success)
                //            {
                //                //matched groupObj.Value.ToString();
                //                // matched text: groupObj.Value
                //                // match start: groupObj.Index
                //                // match length: groupObj.Length
                //            }
                //        }
                //        matchResults = matchResults.NextMatch();
                //        ListViewItem li = new ListViewItem();
                //        li.SubItems.Clear();
                //        li.SubItems[0].Text = num.ToString();
                //        li.SubItems.Add(matchResults.Value.ToString());     //帖子链接
                //        listView2.Items.Add(li);
                //        num++;  //编号ID
                //    }
                //}
                //catch (ArgumentException ex)
                //{
                //    // Syntax error in the regular expression
                //}
                #endregion

                //int intLV1 = listView1.Items.Count;
                //int intLV2 = listView2.Items.Count;

                //listView1.Items[intLV1 - 1].Remove();
                //listView2.Items[intLV2 - 1].Remove();

                toolStripStatusLabel1.Text = "正在访问：" + strURL;
                Reload:
                int RanKey = ran.Next(0, List.Count);

                //showAnnounce_2_4576914_1_0.htm
                string strAnnounceID = List[RanKey].ToString().Trim();
                if (text_SendRecord.Text.IndexOf(strAnnounceID) == -1)
                {
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
                    int RandKey = ran.Next(0, listView6.Items.Count);
                    string strMessage = listView6.Items[RandKey].SubItems[0].Text.Trim();

                    string strURL_1 = "http://club.qingdaonews.com/SaveReAnnounce_static.php";
                    string strBoard_id = B_1.Replace("e_", "");
                    string strTopic_id = T_1.Replace("_1", "");
                    //string strMessages = "路过打酱油的说。";
                    string strMessages = strMessage;
                    string strOut;

                    SendMessage(strURL_1, strAnnounceID, strBoard_id, strTopic_id, strMessages, strUserName, strCookieSessionID, strCookieUserName, strCookiePassword, strList_display_time, out strOut);
                }
                else
                {
                    goto Reload;
                }                
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
                string LoginData = "viewmode=&topic_id=" + strTopic_id + "&parent_id=" + strParent_id + "&board_id=" + strBoard_id + "&Page=1&a_name=" + strUserName + "&subject=%BB%D8%B8%B4%3A&ubb=UBB&chkSignature=1&body=" + strMessages + "&insertimg=";
                //string LoginData = "viewmode=&topic_id=" + strTopic_id + "&parent_id=" + strParent_id + "&board_id=" + strBoard_id + "&Page=1&a_name=" + strUserName + "&subject=%BB%D8%B8%B4%3A&ubb=UBB&chkSignature=1&body=%7B201%7D&insertimg=";
                //数据被传输类型
                LoginHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                Encoding encode = System.Text.Encoding.Default;
                byte[] arrB = encode.GetBytes(LoginData);
                //数据长度
                //LoginHttpWebRequest.ContentLength = LoginData.Length;
                LoginHttpWebRequest.ContentLength = arrB.Length;
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

                LoginHttpWebRequest.GetRequestStream().Write(arrB, 0, arrB.Length);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)LoginHttpWebRequest.GetResponse();
                StreamReader reader = new StreamReader(myHttpWebResponse.GetResponseStream(), encode);

                ////获取登录数据流
                //Stream myRequestStream = LoginHttpWebRequest.GetRequestStream();
                ////StreamWriter
                //StreamWriter myStreamWriter = new StreamWriter(myRequestStream, encode); //Encoding.GetEncoding("gb2312")
                ////把数据写入HttpWebRequest的Request流  
                //myStreamWriter.Write(arrB);

                ////关闭打开对象     
                //myStreamWriter.Close();

                //myRequestStream.Close();

                ////新建一个HttpWebResponse     
                //HttpWebResponse myHttpWebResponse = (HttpWebResponse)LoginHttpWebRequest.GetResponse();

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
                text_SendRecord.Text += "发贴成功：http://club.qingdaonews.com/" + strAnnounceID + "  时间：" + System.DateTime.Now + "\r\n";
                //MessageBox.Show("发贴成功：http://club.qingdaonews.com/" + strAnnounceID);
                intSendCount += 1;
                Timer_T();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            //timer1.Enabled = false;
        }

        #region 添加登录用户
        private void btn_AddUser_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog(this);
            if (f3.DialogResult == DialogResult.OK)
            {
                if (f3.text_UserName.Text.Trim() != "" && f3.text_Password.Text.Trim() != "")
                {
                    ListViewItem li = new ListViewItem();
                    li.SubItems.Clear();
                    li.SubItems[0].Text = f3.text_UserName.Text.Trim();
                    li.SubItems.Add(f3.text_Password.Text.Trim());
                    listView3.Items.Add(li);
                    UserTable();
                }
                else
                {
                    MessageBox.Show("用户名，密码不能空请检查！", "提示");
                }
            }
            else
            {
                f3.Close();
            }
        }
        #endregion

        #region 删除登录用户
        private void btn_DelUser_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView3.SelectedItems)
            {
                if (listView3.SelectedIndices.Count > 0)
                {
                    li.Remove();
                }
                else
                {
                    break;
                }
            }
            UserTable();
        }
        #endregion

        #region 用户信息插入虚拟表
        private void UserTable()
        {
            DataTable dt = new DataTable("Table_UserInfo");
            dt.Columns.Add("UserName", System.Type.GetType("System.String"));
            dt.Columns.Add("Password", System.Type.GetType("System.String"));
            DataRow dr = dt.NewRow();

            foreach (ListViewItem li in listView3.Items)
            {
                if (listView3.Items.Count > 0)
                {
                    dr["UserName"] = li.SubItems[0].Text;
                    dr["Password"] = li.SubItems[1].Text;
                    dt.Rows.Add(dr.ItemArray);
                }
                else
                {
                    break;
                }
            }
            CreateTxt(dt);
        }
        #endregion

        #region 生成记录文件
        private void CreateTxt(DataTable dt)
        {
            if (File.Exists(@"LoginUser.txt"))
            {
                File.Delete(@"LoginUser.txt");
            }
            FileStream fs = new FileStream(@"LoginUser.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));//通过指定字符编码方式可以实现对汉字的支持，否则在用记事本打开查看会出现乱码
            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            foreach (DataRow dr in dt.Rows)
            {
                sw.WriteLine(dr["UserName"] + "," + dr["Password"]);
            }
            sw.Flush();
            sw.Close();
        }
        #endregion

        #region 读取记录文件
        private void ReadTxt()
        {
            string[] arrInfo;
            if (File.Exists(@"LoginUser.txt"))
            {
                StreamReader sr = File.OpenText(@"LoginUser.txt");
                string strInfo = sr.ReadLine();//读取一行判定是否为空
                while (strInfo != null)
                {
                    arrInfo = strInfo.Split(',');
                    ListViewItem li = new ListViewItem();
                    li.SubItems.Clear();
                    li.SubItems[0].Text = arrInfo[0].ToString().Trim();
                    li.SubItems.Add(arrInfo[1].ToString().Trim());
                    listView3.Items.Add(li);

                    strInfo = sr.ReadLine();//判定下一行判定是否为空
                }
                sr.Close();
            }
        }
        #endregion

        #region 读取论坛列表
        private void ReadClubList()
        {
            string[] arrInfo;
            if (File.Exists(@"ClubList.txt"))
            {
                StreamReader sr = File.OpenText(@"ClubList.txt");
                string strInfo = sr.ReadLine();//读取一行判定是否为空
                while (strInfo != null)
                {
                    arrInfo = strInfo.Split(',');
                    ListViewItem li = new ListViewItem();
                    li.SubItems.Clear();
                    li.SubItems[0].Text = arrInfo[0].ToString().Trim();
                    li.SubItems.Add(arrInfo[1].ToString().Trim());
                    listView4.Items.Add(li);

                    strInfo = sr.ReadLine();//判定下一行判定是否为空
                }
                sr.Close();
            }
        }
        #endregion

        #region 查找链接
        /// <summary>
        /// 查找链接
        /// </summary>
        /// <param name="strClubName">栏目名称</param>
        /// <returns></returns>
        public string FindClubLink(string strClubName)
        {
            #region 论坛列表
            string[,] arrBBS_Club = { { "club_entry_2_2_0_1_0.htm", "青岛论坛" }, { "club_entry_1025_2_0_1_0.htm", "社会万象" }, { "club_entry_48_2_0_1_0.htm", "外经外贸" }, 
                                    { "club_entry_67_2_0_1_0.htm", "漂在岛城" }, { "club_entry_57_2_0_1_0.htm", "城市档案" }, { "club_entry_9_2_0_1_0.htm", "移民留学" }, 
                                    { "club_entry_1038_2_0_1_0.htm", "发现青岛" }, { "club_entry_88_2_0_1_0.htm", "闲闲书话" }, { "club_entry_123_2_0_1_0.htm", "五月的风" }, 
                                    { "club_entry_1_2_0_1_0.htm", "谈天说地" }, { "club_entry_156_2_0_1_0.htm", "男婚女嫁" }, { "club_entry_1018_2_0_1_0.htm", "ＤＶ摄像" }, 
                                    { "club_entry_1023_2_0_1_0.htm", "青岛游子" }, { "club_entry_1024_2_0_1_0.htm", "同楼相约" }, { "club_entry_1115_2_0_1_0.htm", "教育论坛" }, 
                                    { "club_entry_1133_2_0_1_0.htm", "奢华品位" }, { "club_entry_41_2_0_1_0.htm", "人文良友" }, { "club_entry_1030_2_0_1_0.htm", "社区之星" }, 
                                    { "club_entry_1170_2_0_1_0.htm", "我爱打折" }, { "club_entry_1171_2_0_1_0.htm", "棋牌游戏" }, { "club_entry_1181_2_0_1_0.htm", "梦幻足球" }, 
                                    { "club_entry_1188_2_0_1_0.htm", "爱 团 购" }, { "club_entry_1192_2_0_1_0.htm", "３Ｇ世界" }, { "club_entry_1199_2_0_1_0.htm", "海 奥 卡" }, 
                                    { "club_entry_128_3_0_1_0.htm", "黄岛论坛" }, { "club_entry_129_3_0_1_0.htm", "城阳论坛" }, { "club_entry_130_3_0_1_0.htm", "即墨论坛" }, 
                                    { "club_entry_131_3_0_1_0.htm", "胶南论坛" }, { "club_entry_132_3_0_1_0.htm", "胶州论坛" }, { "club_entry_133_3_0_1_0.htm", "莱西论坛" }, 
                                    { "club_entry_134_3_0_1_0.htm", "平度论坛" }, { "club_entry_173_3_0_1_0.htm", "崂山论坛" }, { "club_entry_1053_3_0_1_0.htm", "四方论坛" }, 
                                    { "club_entry_1054_3_0_1_0.htm", "李沧论坛" }, { "club_entry_1059_3_0_1_0.htm", "市南论坛" }, { "club_entry_1060_3_0_1_0.htm", "市北论坛" }, 
                                    { "club_entry_39_4_0_1_0.htm", "车迷论坛" }, { "club_entry_1039_4_0_1_0.htm", "挑车购车" }, { "club_entry_1040_4_0_1_0.htm", "交通路况" }, 
                                    { "club_entry_1041_4_0_1_0.htm", "改装养护" }, { "club_entry_1042_4_0_1_0.htm", "二手交易" }, { "club_entry_1043_4_0_1_0.htm", "凯越车会" }, 
                                    { "club_entry_1044_4_0_1_0.htm", "赛欧车会" }, { "club_entry_1045_4_0_1_0.htm", "夏 友 会" }, { "club_entry_1046_4_0_1_0.htm", "尼桑车会" }, 
                                    { "club_entry_1047_4_0_1_0.htm", "大众车会" }, { "club_entry_1048_4_0_1_0.htm", "马自达会" }, { "club_entry_1049_4_0_1_0.htm", "千里马帮" }, 
                                    { "club_entry_1052_4_0_1_0.htm", "越野大队" }, { "club_entry_1055_4_0_1_0.htm", "狮 友 会" }, { "club_entry_1056_4_0_1_0.htm", "哈 宝 族" }, 
                                    { "club_entry_1057_4_0_1_0.htm", "菲友联盟" }, { "club_entry_1061_4_0_1_0.htm", "自驾旅游" }, { "club_entry_1062_4_0_1_0.htm", "雪铁龙族" }, 
                                    { "club_entry_1063_4_0_1_0.htm", "现代车会" }, { "club_entry_1069_4_0_1_0.htm", "青 奇 军" }, { "club_entry_1070_4_0_1_0.htm", "吉 团 军" }, 
                                    { "club_entry_1071_4_0_1_0.htm", "福特联盟" }, { "club_entry_154_4_0_1_0.htm", "顺 风 车" }, { "club_entry_1085_4_0_1_0.htm", "图 击 队" }, 
                                    { "club_entry_1086_4_0_1_0.htm", "同 乐 会" }, { "club_entry_1179_4_0_1_0.htm", "车险理赔" }, { "club_entry_1102_4_0_1_0.htm", "日报车界" }, 
                                    { "club_entry_1108_4_0_1_0.htm", "福克斯会" }, { "club_entry_1109_4_0_1_0.htm", "丰田车会" }, { "club_entry_1111_4_0_1_0.htm", "思 家 车" }, 
                                    { "club_entry_1121_4_0_1_0.htm", "东南车会" }, { "club_entry_1124_4_0_1_0.htm", "燕 友 会" }, { "club_entry_1130_4_0_1_0.htm", "起 亚 会" }, 
                                    { "club_entry_1131_4_0_1_0.htm", "迪 车 族" }, { "club_entry_1143_4_0_1_0.htm", "奥迪车会" }, { "club_entry_1157_4_0_1_0.htm", "80后车会" }, 
                                    { "club_entry_1158_4_0_1_0.htm", "君友联盟" }, { "club_entry_1159_4_0_1_0.htm", "速腾车会" }, { "club_entry_1169_4_0_1_0.htm", "广本车会" }, 
                                    { "club_entry_1173_4_0_1_0.htm", "龙卡汽车卡" }, { "club_entry_1184_4_0_1_0.htm", "驾校学车" }, { "club_entry_1185_4_0_1_0.htm", "奔 奔 族" }, 
                                    { "club_entry_1190_4_0_1_0.htm", "尊 荣 会" }, { "club_entry_1172_4_0_1_0.htm", "都市车圈" }, { "club_entry_20_5_0_1_0.htm", "青岛楼事" }, 
                                    { "club_entry_1010_5_0_1_0.htm", "地产营销" }, { "club_entry_49_5_0_1_0.htm", "家居在线" }, { "club_entry_1139_5_0_1_0.htm", "家具饰品" }, 
                                    { "club_entry_1115_6_0_1_0.htm", "教育论坛" }, { "club_entry_47_6_0_1_0.htm", "教师论坛" }, { "club_entry_66_6_0_1_0.htm", "韩文天地" }, 
                                    { "club_entry_12_6_0_1_0.htm", "英语世界" }, { "club_entry_86_6_0_1_0.htm", "日语学习" }, { "club_entry_163_6_0_1_0.htm", "法 语 角" }, 
                                    { "club_entry_13_6_0_1_0.htm", "中学校园" }, { "club_entry_94_6_0_1_0.htm", "大学时代" }, { "club_entry_83_6_0_1_0.htm", "考试交流" }, 
                                    { "club_entry_174_6_0_1_0.htm", "海洋大学" }, { "club_entry_175_6_0_1_0.htm", "青岛大学" }, { "club_entry_176_6_0_1_0.htm", "青岛科大" }, 
                                    { "club_entry_177_6_0_1_0.htm", "理工大学" }, { "club_entry_1006_6_0_1_0.htm", "山东科大" }, { "club_entry_1007_6_0_1_0.htm", "外贸学院" }, 
                                    { "club_entry_1011_6_0_1_0.htm", "石油大学" }, { "club_entry_1031_6_0_1_0.htm", "青岛农大" }, { "club_entry_58_7_0_1_0.htm", "育儿论坛" }, 
                                    { "club_entry_1005_7_0_1_0.htm", "早教论坛" }, { "club_entry_160_7_0_1_0.htm", "家有学童" }, { "club_entry_121_7_0_1_0.htm", "亲 子 园" }, 
                                    { "club_entry_1099_7_0_1_0.htm", "宝贝学艺" }, { "club_entry_1100_7_0_1_0.htm", "少儿科普" }, { "club_entry_64_8_0_1_0.htm", "投资理财" }, 
                                    { "club_entry_1064_8_0_1_0.htm", "谈股论金" }, { "club_entry_1094_8_0_1_0.htm", "大话基金" }, { "club_entry_1127_8_0_1_0.htm", "我家理财" },
                                    { "club_entry_1128_8_0_1_0.htm", "收藏鉴宝" }, { "club_entry_1142_8_0_1_0.htm", "保险之家" }, { "club_entry_1201_8_0_1_0.htm", "我 爱 卡" }, 
                                    { "club_entry_4_9_0_1_0.htm", "赛场纵横" }, { "club_entry_5_9_0_1_0.htm", "网话海牛" }, { "club_entry_36_9_0_1_0.htm", "64格论坛" }, 
                                    { "club_entry_52_9_0_1_0.htm", "欧陆烽火" }, { "club_entry_74_9_0_1_0.htm", "我爱篮球" }, { "club_entry_76_9_0_1_0.htm", "奥运论坛" }, 
                                    { "club_entry_92_9_0_1_0.htm", "围棋天地" }, { "club_entry_124_9_0_1_0.htm", "羽球世界" }, { "club_entry_125_9_0_1_0.htm", "滑浪风帆" }, 
                                    { "club_entry_146_9_0_1_0.htm", "快乐排球" }, { "club_entry_162_9_0_1_0.htm", "健 身 房" }, { "club_entry_1015_9_0_1_0.htm", "中国象棋" }, 
                                    { "club_entry_1016_9_0_1_0.htm", "以武会友" }, { "club_entry_1033_9_0_1_0.htm", "台球论坛" }, { "club_entry_1068_9_0_1_0.htm", "网球天地" }, 
                                    { "club_entry_1084_9_0_1_0.htm", "瑜伽男女" }, { "club_entry_1125_9_0_1_0.htm", "小球世界" }, { "club_entry_1151_9_0_1_0.htm", "中能足球" }, 
                                    { "club_entry_33_10_0_1_0.htm", "健康有约" }, { "club_entry_93_10_0_1_0.htm", "白衣使者" }, { "club_entry_77_10_0_1_0.htm", "心理咨询" }, 
                                    { "club_entry_1072_10_0_1_0.htm", "医疗器械 " }, { "club_entry_1073_10_0_1_0.htm", "求医问药" }, { "club_entry_1027_10_0_1_0.htm", "美容护肤" }, 
                                    { "club_entry_1066_10_0_1_0.htm", "减肥纤体 " }, { "club_entry_1077_10_0_1_0.htm", "两性话题 " }, { "club_entry_1101_10_0_1_0.htm", "男人女人 " }, 
                                    { "club_entry_1117_10_0_1_0.htm", "整形美容" }, { "club_entry_1189_10_0_1_0.htm", "民生开讲" }, { "club_entry_40_11_0_1_0.htm", "女性天地" }, 
                                    { "club_entry_1026_11_0_1_0.htm", "征婚交友" }, { "club_entry_24_11_0_1_0.htm", "单身男女" }, { "club_entry_43_11_0_1_0.htm", "围城内外" }, 
                                    { "club_entry_54_11_0_1_0.htm", "人到中年" }, { "club_entry_70_11_0_1_0.htm", "人生百味" }, { "club_entry_72_11_0_1_0.htm", "三十而立" }, 
                                    { "club_entry_81_11_0_1_0.htm", "男人频道" }, { "club_entry_84_11_0_1_0.htm", "缘来是你" }, { "club_entry_122_11_0_1_0.htm", "夕照霞光" }, 
                                    { "club_entry_108_11_0_1_0.htm", "蹉跎岁月" }, { "club_entry_22_11_0_1_0.htm", "宜人茶室" }, { "club_entry_61_11_0_1_0.htm", "书香文字" }, 
                                    { "club_entry_8_11_0_1_0.htm", "真情诉白" }, { "club_entry_42_11_0_1_0.htm", "文学原创" }, { "club_entry_1126_11_0_1_0.htm", "婆媳关系" }, 
                                    { "club_entry_1129_11_0_1_0.htm", "婚姻家庭" }, { "club_entry_1156_11_0_1_0.htm", "诗情画意" }, { "club_entry_145_11_0_1_0.htm", "武侠论谈" },
                                    { "club_entry_107_11_0_1_0.htm", "茶 文 化" }, { "club_entry_144_11_0_1_0.htm", "咬文嚼字" }, { "club_entry_1191_11_0_1_0.htm", "散文诗词" }, 
                                    { "club_entry_1144_11_0_1_0.htm", "绝对隐私" }, { "club_entry_7_12_0_1_0.htm", "笑话天地" }, { "club_entry_27_12_0_1_0.htm", "美食广场" }, 
                                    { "club_entry_1193_12_0_1_0.htm", "食全十美" }, { "club_entry_1183_12_0_1_0.htm", "嘻哈Ｋ歌" }, { "club_entry_113_12_0_1_0.htm", "漂亮女人" }, 
                                    { "club_entry_46_12_0_1_0.htm", "时尚主义" }, { "club_entry_60_12_0_1_0.htm", "动漫论坛" }, { "club_entry_1187_12_0_1_0.htm", "我要结婚" }, 
                                    { "club_entry_82_12_0_1_0.htm", "手工情结" }, { "club_entry_98_12_0_1_0.htm", "酒吧文化" }, { "club_entry_1168_12_0_1_0.htm", "啤酒论坛" }, 
                                    { "club_entry_1180_12_0_1_0.htm", "巧克力吧" }, { "club_entry_1075_12_0_1_0.htm", "吃喝玩乐" }, { "club_entry_1078_12_0_1_0.htm", "形象顾问" }, 
                                    { "club_entry_1087_12_0_1_0.htm", "音响世界" }, { "club_entry_38_12_0_1_0.htm", "音乐世界" }, { "club_entry_141_12_0_1_0.htm", "灌水乐园" }, 
                                    { "club_entry_96_12_0_1_0.htm", "手机乐园" }, { "club_entry_97_12_0_1_0.htm", "家有宠物" }, { "club_entry_109_12_0_1_0.htm", "花鸟鱼虫" }, 
                                    { "club_entry_1032_12_0_1_0.htm", "影视天堂" }, { "club_entry_147_12_0_1_0.htm", "占星奇缘" }, { "club_entry_1013_12_0_1_0.htm", "你问我答" }, 
                                    { "club_entry_1149_12_0_1_0.htm", "渔 乐 圈" }, { "club_entry_149_12_0_1_0.htm", "娱 乐 圈" }, { "club_entry_1165_12_0_1_0.htm", "舞蹈时尚" }, 
                                    { "club_entry_1200_12_0_1_0.htm", "天黑闭眼" }, { "club_entry_26_17_0_1_0.htm", "游山玩水" }, { "club_entry_1067_17_0_1_0.htm", "海奥旅游" }, 
                                    { "club_entry_120_17_0_1_0.htm", "游遍世界" }, { "club_entry_1096_17_0_1_0.htm", "走进大山" }, { "club_entry_1020_17_0_1_0.htm", "晚报旅游" }, 
                                    { "club_entry_1145_17_0_1_0.htm", "旅游DIY" }, { "club_entry_1160_17_0_1_0.htm", "玩 崂 山" }, { "club_entry_71_13_0_1_0.htm", "摄影园地" }, 
                                    { "club_entry_166_13_0_1_0.htm", "明星八卦" }, { "club_entry_167_13_0_1_0.htm", "搞笑图库" }, { "club_entry_168_13_0_1_0.htm", "酷图超市" }, 
                                    { "club_entry_169_13_0_1_0.htm", "风景美图" }, { "club_entry_170_13_0_1_0.htm", "体育图片" }, { "club_entry_165_13_0_1_0.htm", "美女贴图" }, 
                                    { "club_entry_172_13_0_1_0.htm", "闪客帝国" }, { "club_entry_1018_13_0_1_0.htm", "ＤＶ摄像" }, { "club_entry_1030_13_0_1_0.htm", "社区之星" }, 
                                    { "club_entry_1076_13_0_1_0.htm", "人体艺术" }, { "club_entry_1082_13_0_1_0.htm", "泳 装 秀" }, { "club_entry_47_14_0_1_0.htm", "教师论坛" }, 
                                    { "club_entry_29_14_0_1_0.htm", "记者之家" }, { "club_entry_37_14_0_1_0.htm", "法律论坛" }, { "club_entry_1080_14_0_1_0.htm", "会展天地" }, 
                                    { "club_entry_1123_14_0_1_0.htm", "律师咨询" }, { "club_entry_93_14_0_1_0.htm", "白衣使者" }, { "club_entry_112_14_0_1_0.htm", "广 告 人" }, 
                                    { "club_entry_118_14_0_1_0.htm", "营销精英" }, { "club_entry_119_14_0_1_0.htm", "程 序 员" }, { "club_entry_126_14_0_1_0.htm", "创业沙龙" }, 
                                    { "club_entry_159_14_0_1_0.htm", "设计空间" }, { "club_entry_164_14_0_1_0.htm", "票务论坛" }, { "club_entry_69_14_0_1_0.htm", "人才求职" }, 
                                    { "club_entry_1021_14_0_1_0.htm", "买卖驿站" }, { "club_entry_1089_14_0_1_0.htm", "特价机票" }, { "club_entry_1091_14_0_1_0.htm", "司法审判" }, 
                                    { "club_entry_1116_14_0_1_0.htm", "机票超市" }, { "club_entry_1122_14_0_1_0.htm", "工商税务" }, { "club_entry_3_14_0_1_0.htm", "电脑技术" }, 
                                    { "club_entry_111_14_0_1_0.htm", "网站建设" }, { "club_entry_1012_14_0_1_0.htm", "环保论坛" }, { "club_entry_1029_14_0_1_0.htm", "数码地带" }, 
                                    { "club_entry_73_14_0_1_0.htm", "个体私营" }, { "club_entry_1092_14_0_1_0.htm", "财务会计" }, { "club_entry_1137_14_0_1_0.htm", "高端品牌" }, 
                                    { "club_entry_1090_14_0_1_0.htm", "家电论坛" }, { "club_entry_1019_14_0_1_0.htm", "巴士公交" }, { "club_entry_1050_14_0_1_0.htm", "我是义工" }, 
                                    { "club_entry_114_14_0_1_0.htm", "广播电视" }, { "club_entry_1034_14_0_1_0.htm", "晚报生活" }, { "club_entry_1110_14_0_1_0.htm", "梦想剧场" }, 
                                    { "club_entry_1134_14_0_1_0.htm", "艺术青岛" }, { "club_entry_143_14_0_1_0.htm", "军事天地" }, { "club_entry_1135_14_0_1_0.htm", "知识宝典" }, 
                                    { "club_entry_152_14_0_1_0.htm", "苍穹探索" }, { "club_entry_1164_14_0_1_0.htm", "ＴＤ手机" }, { "club_entry_1174_14_0_1_0.htm", "珠宝玉器" }, 
                                    { "club_entry_1175_14_0_1_0.htm", "青岛婚庆" }, { "club_entry_1020_14_0_1_0.htm", "晚报旅游" }, { "club_entry_1182_14_0_1_0.htm", "岛城说法" }, 
                                    { "club_entry_1194_14_0_1_0.htm", "真情人间" }, { "club_entry_1197_14_0_1_0.htm", "电子开发" }, { "club_entry_1198_14_0_1_0.htm", "上 蛤 蜊" }, 
                                    { "club_entry_44_15_0_1_0.htm", "齐鲁论坛" }, { "club_entry_2_15_0_1_0.htm", "青岛论坛 " }, { "club_entry_135_15_0_1_0.htm", "济南论坛" }, 
                                    { "club_entry_99_15_0_1_0.htm", "烟台论坛" }, { "club_entry_91_15_0_1_0.htm", "淄博论坛" }, { "club_entry_100_15_0_1_0.htm", "潍坊论坛" }, 
                                    { "club_entry_101_15_0_1_0.htm", "威海论坛" }, { "club_entry_102_15_0_1_0.htm", "日照论坛" }, { "club_entry_103_15_0_1_0.htm", "济宁论坛" }, 
                                    { "club_entry_105_15_0_1_0.htm", "聊城论坛" }, { "club_entry_106_15_0_1_0.htm", "枣庄论坛" }, { "club_entry_110_15_0_1_0.htm", "泰安论坛" }, 
                                    { "club_entry_115_15_0_1_0.htm", "临沂论坛" }, { "club_entry_136_15_0_1_0.htm", "德州论坛" }, { "club_entry_137_15_0_1_0.htm", "东营论坛" }, 
                                    { "club_entry_138_15_0_1_0.htm", "滨州论坛" }, { "club_entry_139_15_0_1_0.htm", "菏泽论坛" }, { "club_entry_140_15_0_1_0.htm", "莱芜论坛" }, 
                                    { "club_entry_30_16_0_1_0.htm", "批评建议" }, { "club_entry_68_16_0_1_0.htm", "聊友建议" }, { "club_entry_87_16_0_1_0.htm", "版主之家" }, 
                                    { "club_entry_1093_16_0_1_0.htm", "博友之家" } };
            #endregion

            foreach (ListViewItem li in listView4.Items)
            {
                string st = li.SubItems[0].Text.Trim();
                string sst = li.SubItems[1].Text.Trim();
            }

            //for (int i = 0; i < arrBBS_Club.Length / 2; i++)
            //{
            //    if (strClubName == arrBBS_Club[i, 1].ToString())
            //    {
            //        strClubName = arrBBS_Club[i, 0].ToString();
            //    }
            //}
            return strClubName;
        }
        #endregion

        private ArrayList test()
        {
            string[] arrInfo = { };
            ArrayList List = new ArrayList();
            DataTable dt = new DataTable("Table_Announce");
            dt.Columns.Add("Link", System.Type.GetType("System.String"));

            if (File.Exists(@"ClubList.txt"))
            {
                StreamReader sr = File.OpenText(@"ClubList.txt");
                string strInfo = sr.ReadLine();//读取一行判定是否为空
                while (strInfo != null)
                {
                    List.Add(strInfo);
                    strInfo = sr.ReadLine();//判定下一行判定是否为空
                }
                arrInfo = (string[])List.ToArray(typeof(string));
                return List;
            }
            else
            {
                return null;
            }
        }

        #region 加载最新论坛列表
        private void LoadClubInfo()
        {
            try
            {
                //定义Cookie容器
                CookieContainer CookieArray = new CookieContainer();

                //创建Http请求
                HttpWebRequest LoginHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://club.qingdaonews.com/");

                //登录数据
                //string LoginData = "id=" + strUserName + "&passwd=" + strPassword + "&usertype=0";
                //数据被传输类型
                //LoginHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //数据长度
                //LoginHttpWebRequest.ContentLength = LoginData.Length;
                //数据传输方法 get或post
                //LoginHttpWebRequest.Method = "POST";
                //LoginHttpWebRequest.Accept = "*/*";
                //LoginHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; MAXTHON 2.0)";
                //设置HttpWebRequest的CookieContainer为刚才建立的那个CookieArray  
                //LoginHttpWebRequest.CookieContainer = CookieArray;
                //CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("PHPSESSID", strCookieSessionID));
                //CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[username]", strCookieUserName));
                //CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[password]", strCookiePassword));

                //获取登录数据流
                //Stream myRequestStream = LoginHttpWebRequest.GetRequestStream();

                //StreamWriter
                //StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                //把数据写入HttpWebRequest的Request流  
                //myStreamWriter.Write(LoginData);

                //关闭打开对象     
                //myStreamWriter.Close();
                //myRequestStream.Close();

                //新建一个HttpWebResponse     
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)LoginHttpWebRequest.GetResponse();

                //获取一个包含url的Cookie集合的CookieCollection     
                //myHttpWebResponse.Cookies = CookieArray.GetCookies(LoginHttpWebRequest.RequestUri);

                //if (myHttpWebResponse.Cookies.Count > 0)
                //{
                //    CookieArray.Add(myHttpWebResponse.Cookies);
                //}

                //WebHeaderCollection a = myHttpWebResponse.Headers;

                Stream myResponseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("gb2312"));

                string strShowBox = myStreamReader.ReadToEnd();

                //把数据从HttpWebResponse的Response流中读出     
                myStreamReader.Close();

                myResponseStream.Close();

                try
                {
                    Regex regexObj = new Regex(@"club_entry_\d{1,9}_\d{1,9}_\d{1,9}_\d{1,9}_\d{1,9}\.htm"">[\u4e00-\u9fa5,\uFF00-\uFFFF, ,0-9,\w]{1,5}<");
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
                            resultString_1 = Regex.Match(matchResults.ToString(), @">[\u4e00-\u9fa5,\uFF00-\uFFFF, ,0-9,\w]{1,9}<").Value;
                            ListViewItem li = new ListViewItem();
                            li.SubItems.Clear();
                            string s_1 = resultString_1.Replace(">", "");
                            li.SubItems[0].Text = s_1.Replace("<", "");
                            li.SubItems.Add(resultString_0);
                            listView4.Items.Add(li);
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                    }
                    listView4.Items[listView4.Items.Count - 1].Remove();
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 添加发贴内容
        private void btn_AddContent_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.ShowDialog(this);
            if (f4.DialogResult == DialogResult.OK)
            {
                if (f4.textBox1.Text.Trim() != "")
                {
                    ListViewItem li = new ListViewItem();
                    li.SubItems.Clear();   
                    li.SubItems[0].Text = f4.textBox1.Text.Trim();
                    listView6.Items.Add(li);
                    ContentTable();
                }
                else
                {
                    f4.Close();
                }
            }
            else
            {
                f4.Close();
            }
        }
        #endregion

        #region 发贴内容虚拟表
        private void ContentTable()
        {
            DataTable dt = new DataTable("Table_SendContent");
            dt.Columns.Add("Content", System.Type.GetType("System.String"));
            DataRow dr = dt.NewRow();

            foreach (ListViewItem li in listView6.Items)
            {
                if (listView6.Items.Count > 0)
                {
                    dr["Content"] = li.SubItems[0].Text;
                    dt.Rows.Add(dr.ItemArray);
                }
                else
                {
                    break;
                }
            }
            ContentTxt(dt);
        }
        #endregion

        #region 生成发贴内容记录
        private void ContentTxt(DataTable dt)
        {
            if (File.Exists(@"SendContent.txt"))
            {
                File.Delete(@"SendContent.txt");
            }
            FileStream fs = new FileStream(@"SendContent.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));//通过指定字符编码方式可以实现对汉字的支持，否则在用记事本打开查看会出现乱码
            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            foreach (DataRow dr in dt.Rows)
            {
                sw.WriteLine(dr["Content"]);
            }
            sw.Flush();
            sw.Close();
        }
        #endregion

        #region 读取发贴内容记录
        private void ReadContentTxt()
        {
            if (File.Exists(@"SendContent.txt"))
            {
                StreamReader sr = File.OpenText(@"SendContent.txt");
                string strInfo = sr.ReadLine();//读取一行判定是否为空
                while (strInfo != null)
                {
                    ListViewItem li = new ListViewItem();
                    li.SubItems.Clear();
                    li.SubItems[0].Text = strInfo.Trim();
                    listView6.Items.Add(li);

                    strInfo = sr.ReadLine();//判定下一行判定是否为空
                }
                sr.Close();
            }
        }
        #endregion

        #region 删除发贴内容
        private void btn_DelContent_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView6.SelectedItems)
            {
                if (listView6.SelectedIndices.Count > 0)
                {
                    li.Remove();
                }
                else
                {
                    break;
                }
            }
            ContentTable();
        }
        #endregion

        #region 发贴按钮事件
        private void btn_Send_Click(object sender, EventArgs e)
        {
            
            if (btn_Send.Text == "开始顶贴")
            {
                btn_Send.Text = "停止发贴";
                Timer_T();
            }
            else
            {
                btn_Send.Text = "开始顶贴";
                intSendCount = 1;
            }

        }
        #endregion

        #region 激活计时器
        private void Timer_T()
        {
            System.Timers.Timer t = new System.Timers.Timer();//实例化Timer类，设置间隔时间为10000毫秒；
            //SendContent();
            int intSendTime = Convert.ToInt32(text_SendTime.Text.Trim());
            if (intSendTime < 10)
            {
                intSendTime = 10;
            }
            t.Interval = intSendTime * 1000;
            t.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);//到达时间的时候执行事件； 
            t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;
            //Timer t = new Timer();
            //t.Interval = 5000;
            //t.Tick += new EventHandler(timer1_Tick);
            //t.Enabled = true;
        }
        #endregion

        private void SendContent()
        {
            int intSentTime = Convert.ToInt32(text_SendTime.Text.Trim());    //发贴时间间隔
            int intSendUser = Convert.ToInt32(text_SendUser.Text.Trim());    //发贴用户切换
            //this.timer1.Enabled = false;     //关闭计时器

            string strURL = "http://club.qingdaonews.com/sublogin_new.php";
            //string strURL = "http://club.qingdaonews.com/login_club.php";
            string strOut = "";

            if (listView3.Items.Count > 0)
            {
                if (Convert.ToInt32(text_SendUser.Text.Trim()) > 1)     //判定用户是否多于1个用户
                {
                    if (intSendCount % intSendUser == 0)
                    {
                        intTempNum++;
                        MessageBox.Show(intTempNum.ToString());
                        int intUserCount = listView3.Items.Count;
                        if (intTempNum >= intUserCount)
                        {
                            intTempNum = 0;
                        }
                    }
                    //string strUserName = listView3.Items[intTempNum].SubItems[0].Text.Trim();
                    //MessageBox.Show(strUserName + ",++" + intSendCount);
                    //intSendCount += 1;
                    //Timer_T();
                    Login(listView3.Items[intTempNum].SubItems[0].Text.Trim(), listView3.Items[intTempNum].SubItems[1].Text.Trim(), strURL, out strOut);
                }
                else //单用发送
                {
                    //MessageBox.Show(strUserName + "," + intSendCount);
                    //intSendCount += 1;
                    //Timer_T();
                    Login(listView3.Items[intTempNum].SubItems[0].Text.Trim(), listView3.Items[intTempNum].SubItems[1].Text.Trim(), strURL, out strOut);
                }
            }
            else
            {
                MessageBox.Show("没有添加发送用户");
                btn_Send.Text = "开始顶贴";
            }
        }

        #region 计时器方法
        private void timer1_Tick(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            if (btn_Send.Text == "开始顶贴")
            {
                MessageBox.Show("发贴停止");
                intSendCount = 1;
            }
            else
            {
                //if (intSendCount == 10)
                //{
                //    btn_Send.Text = "开始顶贴";
                //}
                //else
                //{
                    //MessageBox.Show("run");
                    SendContent();
                //}
            }
        }
        #endregion

        #region URL编码 UrlEncode(string str, string encode)
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private static string UrlEncode(string str, string encode)
        {
            int factor = 0;
            if (encode == "UTF-8")
                factor = 3;
            if (encode == "GB2312")
                factor = 2;
            //不需要编码的字符

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.";
            System.Text.Encoder encoder = System.Text.Encoding.GetEncoding(encode).GetEncoder();
            char[] c1 = str.ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //一个字符一个字符的编码

            for (int i = 0; i < c1.Length; i++)
            {
                //不需要编码

                if (okChar.IndexOf(c1[i]) > -1)
                    sb.Append(c1[i]);
                else
                {
                    byte[] c2 = new byte[factor];
                    int charUsed, byteUsed; bool completed;

                    encoder.Convert(c1, i, 1, c2, 0, factor, true, out charUsed, out byteUsed, out completed);

                    foreach (byte b in c2)
                    {
                        if (b != 0)
                            sb.AppendFormat("%{0:X}", b);
                    }
                }
            }
            return sb.ToString().Trim();
        }
        #endregion
    }
}