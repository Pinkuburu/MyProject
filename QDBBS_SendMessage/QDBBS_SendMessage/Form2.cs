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
        public int intSendCount = 1;   //����ͳ��
        public Random ran = new Random();   //�����������
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

            #region ��̳����
            string[,] arrBBS_Club = { { "club_entry_2_2_0_1_0.htm", "�ൺ��̳" }, { "club_entry_1025_2_0_1_0.htm", "�������" }, { "club_entry_48_2_0_1_0.htm", "�⾭��ó" }, 
                                    { "club_entry_67_2_0_1_0.htm", "Ư�ڵ���" }, { "club_entry_57_2_0_1_0.htm", "���е���" }, { "club_entry_9_2_0_1_0.htm", "������ѧ" }, 
                                    { "club_entry_1038_2_0_1_0.htm", "�����ൺ" }, { "club_entry_88_2_0_1_0.htm", "�����黰" }, { "club_entry_123_2_0_1_0.htm", "���µķ�" }, 
                                    { "club_entry_1_2_0_1_0.htm", "̸��˵��" }, { "club_entry_156_2_0_1_0.htm", "�л�Ů��" }, { "club_entry_1018_2_0_1_0.htm", "�ģ�����" }, 
                                    { "club_entry_1023_2_0_1_0.htm", "�ൺ����" }, { "club_entry_1024_2_0_1_0.htm", "ͬ¥��Լ" }, { "club_entry_1115_2_0_1_0.htm", "������̳" }, 
                                    { "club_entry_1133_2_0_1_0.htm", "�ݻ�Ʒλ" }, { "club_entry_41_2_0_1_0.htm", "��������" }, { "club_entry_1030_2_0_1_0.htm", "����֮��" }, 
                                    { "club_entry_1170_2_0_1_0.htm", "�Ұ�����" }, { "club_entry_1171_2_0_1_0.htm", "������Ϸ" }, { "club_entry_1181_2_0_1_0.htm", "�λ�����" }, 
                                    { "club_entry_1188_2_0_1_0.htm", "�� �� ��" }, { "club_entry_1192_2_0_1_0.htm", "��������" }, { "club_entry_1199_2_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_128_3_0_1_0.htm", "�Ƶ���̳" }, { "club_entry_129_3_0_1_0.htm", "������̳" }, { "club_entry_130_3_0_1_0.htm", "��ī��̳" }, 
                                    { "club_entry_131_3_0_1_0.htm", "������̳" }, { "club_entry_132_3_0_1_0.htm", "������̳" }, { "club_entry_133_3_0_1_0.htm", "������̳" }, 
                                    { "club_entry_134_3_0_1_0.htm", "ƽ����̳" }, { "club_entry_173_3_0_1_0.htm", "��ɽ��̳" }, { "club_entry_1053_3_0_1_0.htm", "�ķ���̳" }, 
                                    { "club_entry_1054_3_0_1_0.htm", "�����̳" }, { "club_entry_1059_3_0_1_0.htm", "������̳" }, { "club_entry_1060_3_0_1_0.htm", "�б���̳" }, 
                                    { "club_entry_39_4_0_1_0.htm", "������̳" }, { "club_entry_1039_4_0_1_0.htm", "��������" }, { "club_entry_1040_4_0_1_0.htm", "��ͨ·��" }, 
                                    { "club_entry_1041_4_0_1_0.htm", "��װ����" }, { "club_entry_1042_4_0_1_0.htm", "���ֽ���" }, { "club_entry_1043_4_0_1_0.htm", "��Խ����" }, 
                                    { "club_entry_1044_4_0_1_0.htm", "��ŷ����" }, { "club_entry_1045_4_0_1_0.htm", "�� �� ��" }, { "club_entry_1046_4_0_1_0.htm", "��ɣ����" }, 
                                    { "club_entry_1047_4_0_1_0.htm", "���ڳ���" }, { "club_entry_1048_4_0_1_0.htm", "���Դ��" }, { "club_entry_1049_4_0_1_0.htm", "ǧ�����" }, 
                                    { "club_entry_1052_4_0_1_0.htm", "ԽҰ���" }, { "club_entry_1055_4_0_1_0.htm", "ʨ �� ��" }, { "club_entry_1056_4_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_1057_4_0_1_0.htm", "��������" }, { "club_entry_1061_4_0_1_0.htm", "�Լ�����" }, { "club_entry_1062_4_0_1_0.htm", "ѩ������" }, 
                                    { "club_entry_1063_4_0_1_0.htm", "�ִ�����" }, { "club_entry_1069_4_0_1_0.htm", "�� �� ��" }, { "club_entry_1070_4_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_1071_4_0_1_0.htm", "��������" }, { "club_entry_154_4_0_1_0.htm", "˳ �� ��" }, { "club_entry_1085_4_0_1_0.htm", "ͼ �� ��" }, 
                                    { "club_entry_1086_4_0_1_0.htm", "ͬ �� ��" }, { "club_entry_1179_4_0_1_0.htm", "��������" }, { "club_entry_1102_4_0_1_0.htm", "�ձ�����" }, 
                                    { "club_entry_1108_4_0_1_0.htm", "����˹��" }, { "club_entry_1109_4_0_1_0.htm", "���ﳵ��" }, { "club_entry_1111_4_0_1_0.htm", "˼ �� ��" }, 
                                    { "club_entry_1121_4_0_1_0.htm", "���ϳ���" }, { "club_entry_1124_4_0_1_0.htm", "�� �� ��" }, { "club_entry_1130_4_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_1131_4_0_1_0.htm", "�� �� ��" }, { "club_entry_1143_4_0_1_0.htm", "�µϳ���" }, { "club_entry_1157_4_0_1_0.htm", "80�󳵻�" }, 
                                    { "club_entry_1158_4_0_1_0.htm", "��������" }, { "club_entry_1159_4_0_1_0.htm", "���ڳ���" }, { "club_entry_1169_4_0_1_0.htm", "�㱾����" }, 
                                    { "club_entry_1173_4_0_1_0.htm", "����������" }, { "club_entry_1184_4_0_1_0.htm", "��Уѧ��" }, { "club_entry_1185_4_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_1190_4_0_1_0.htm", "�� �� ��" }, { "club_entry_1172_4_0_1_0.htm", "���г�Ȧ" }, { "club_entry_20_5_0_1_0.htm", "�ൺ¥��" }, 
                                    { "club_entry_1010_5_0_1_0.htm", "�ز�Ӫ��" }, { "club_entry_49_5_0_1_0.htm", "�Ҿ�����" }, { "club_entry_1139_5_0_1_0.htm", "�Ҿ���Ʒ" }, 
                                    { "club_entry_1115_6_0_1_0.htm", "������̳" }, { "club_entry_47_6_0_1_0.htm", "��ʦ��̳" }, { "club_entry_66_6_0_1_0.htm", "�������" }, 
                                    { "club_entry_12_6_0_1_0.htm", "Ӣ������" }, { "club_entry_86_6_0_1_0.htm", "����ѧϰ" }, { "club_entry_163_6_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_13_6_0_1_0.htm", "��ѧУ԰" }, { "club_entry_94_6_0_1_0.htm", "��ѧʱ��" }, { "club_entry_83_6_0_1_0.htm", "���Խ���" }, 
                                    { "club_entry_174_6_0_1_0.htm", "�����ѧ" }, { "club_entry_175_6_0_1_0.htm", "�ൺ��ѧ" }, { "club_entry_176_6_0_1_0.htm", "�ൺ�ƴ�" }, 
                                    { "club_entry_177_6_0_1_0.htm", "����ѧ" }, { "club_entry_1006_6_0_1_0.htm", "ɽ���ƴ�" }, { "club_entry_1007_6_0_1_0.htm", "��óѧԺ" }, 
                                    { "club_entry_1011_6_0_1_0.htm", "ʯ�ʹ�ѧ" }, { "club_entry_1031_6_0_1_0.htm", "�ൺũ��" }, { "club_entry_58_7_0_1_0.htm", "������̳" }, 
                                    { "club_entry_1005_7_0_1_0.htm", "�����̳" }, { "club_entry_160_7_0_1_0.htm", "����ѧͯ" }, { "club_entry_121_7_0_1_0.htm", "�� �� ԰" }, 
                                    { "club_entry_1099_7_0_1_0.htm", "����ѧ��" }, { "club_entry_1100_7_0_1_0.htm", "�ٶ�����" }, { "club_entry_64_8_0_1_0.htm", "Ͷ�����" }, 
                                    { "club_entry_1064_8_0_1_0.htm", "̸���۽�" }, { "club_entry_1094_8_0_1_0.htm", "�󻰻���" }, { "club_entry_1127_8_0_1_0.htm", "�Ҽ����" },
                                    { "club_entry_1128_8_0_1_0.htm", "�ղؼ���" }, { "club_entry_1142_8_0_1_0.htm", "����֮��" }, { "club_entry_1201_8_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_4_9_0_1_0.htm", "�����ݺ�" }, { "club_entry_5_9_0_1_0.htm", "������ţ" }, { "club_entry_36_9_0_1_0.htm", "64����̳" }, 
                                    { "club_entry_52_9_0_1_0.htm", "ŷ½���" }, { "club_entry_74_9_0_1_0.htm", "�Ұ�����" }, { "club_entry_76_9_0_1_0.htm", "������̳" }, 
                                    { "club_entry_92_9_0_1_0.htm", "Χ�����" }, { "club_entry_124_9_0_1_0.htm", "��������" }, { "club_entry_125_9_0_1_0.htm", "���˷緫" }, 
                                    { "club_entry_146_9_0_1_0.htm", "��������" }, { "club_entry_162_9_0_1_0.htm", "�� �� ��" }, { "club_entry_1015_9_0_1_0.htm", "�й�����" }, 
                                    { "club_entry_1016_9_0_1_0.htm", "�������" }, { "club_entry_1033_9_0_1_0.htm", "̨����̳" }, { "club_entry_1068_9_0_1_0.htm", "�������" }, 
                                    { "club_entry_1084_9_0_1_0.htm", "�٤��Ů" }, { "club_entry_1125_9_0_1_0.htm", "С������" }, { "club_entry_1151_9_0_1_0.htm", "��������" }, 
                                    { "club_entry_33_10_0_1_0.htm", "������Լ" }, { "club_entry_93_10_0_1_0.htm", "����ʹ��" }, { "club_entry_77_10_0_1_0.htm", "������ѯ" }, 
                                    { "club_entry_1072_10_0_1_0.htm", "ҽ����е " }, { "club_entry_1073_10_0_1_0.htm", "��ҽ��ҩ" }, { "club_entry_1027_10_0_1_0.htm", "���ݻ���" }, 
                                    { "club_entry_1066_10_0_1_0.htm", "�������� " }, { "club_entry_1077_10_0_1_0.htm", "���Ի��� " }, { "club_entry_1101_10_0_1_0.htm", "����Ů�� " }, 
                                    { "club_entry_1117_10_0_1_0.htm", "��������" }, { "club_entry_1189_10_0_1_0.htm", "��������" }, { "club_entry_40_11_0_1_0.htm", "Ů�����" }, 
                                    { "club_entry_1026_11_0_1_0.htm", "���齻��" }, { "club_entry_24_11_0_1_0.htm", "������Ů" }, { "club_entry_43_11_0_1_0.htm", "Χ������" }, 
                                    { "club_entry_54_11_0_1_0.htm", "�˵�����" }, { "club_entry_70_11_0_1_0.htm", "������ζ" }, { "club_entry_72_11_0_1_0.htm", "��ʮ����" }, 
                                    { "club_entry_81_11_0_1_0.htm", "����Ƶ��" }, { "club_entry_84_11_0_1_0.htm", "Ե������" }, { "club_entry_122_11_0_1_0.htm", "Ϧ��ϼ��" }, 
                                    { "club_entry_108_11_0_1_0.htm", "��������" }, { "club_entry_22_11_0_1_0.htm", "���˲���" }, { "club_entry_61_11_0_1_0.htm", "��������" }, 
                                    { "club_entry_8_11_0_1_0.htm", "�����߰�" }, { "club_entry_42_11_0_1_0.htm", "��ѧԭ��" }, { "club_entry_1126_11_0_1_0.htm", "��ϱ��ϵ" }, 
                                    { "club_entry_1129_11_0_1_0.htm", "������ͥ" }, { "club_entry_1156_11_0_1_0.htm", "ʫ�黭��" }, { "club_entry_145_11_0_1_0.htm", "������̸" },
                                    { "club_entry_107_11_0_1_0.htm", "�� �� ��" }, { "club_entry_144_11_0_1_0.htm", "ҧ�Ľ���" }, { "club_entry_1191_11_0_1_0.htm", "ɢ��ʫ��" }, 
                                    { "club_entry_1144_11_0_1_0.htm", "������˽" }, { "club_entry_7_12_0_1_0.htm", "Ц�����" }, { "club_entry_27_12_0_1_0.htm", "��ʳ�㳡" }, 
                                    { "club_entry_1193_12_0_1_0.htm", "ʳȫʮ��" }, { "club_entry_1183_12_0_1_0.htm", "�����˸�" }, { "club_entry_113_12_0_1_0.htm", "Ư��Ů��" }, 
                                    { "club_entry_46_12_0_1_0.htm", "ʱ������" }, { "club_entry_60_12_0_1_0.htm", "������̳" }, { "club_entry_1187_12_0_1_0.htm", "��Ҫ���" }, 
                                    { "club_entry_82_12_0_1_0.htm", "�ֹ����" }, { "club_entry_98_12_0_1_0.htm", "�ư��Ļ�" }, { "club_entry_1168_12_0_1_0.htm", "ơ����̳" }, 
                                    { "club_entry_1180_12_0_1_0.htm", "�ɿ�����" }, { "club_entry_1075_12_0_1_0.htm", "�Ժ�����" }, { "club_entry_1078_12_0_1_0.htm", "�������" }, 
                                    { "club_entry_1087_12_0_1_0.htm", "��������" }, { "club_entry_38_12_0_1_0.htm", "��������" }, { "club_entry_141_12_0_1_0.htm", "��ˮ��԰" }, 
                                    { "club_entry_96_12_0_1_0.htm", "�ֻ���԰" }, { "club_entry_97_12_0_1_0.htm", "���г���" }, { "club_entry_109_12_0_1_0.htm", "�������" }, 
                                    { "club_entry_1032_12_0_1_0.htm", "Ӱ������" }, { "club_entry_147_12_0_1_0.htm", "ռ����Ե" }, { "club_entry_1013_12_0_1_0.htm", "�����Ҵ�" }, 
                                    { "club_entry_1149_12_0_1_0.htm", "�� �� Ȧ" }, { "club_entry_149_12_0_1_0.htm", "�� �� Ȧ" }, { "club_entry_1165_12_0_1_0.htm", "�赸ʱ��" }, 
                                    { "club_entry_1200_12_0_1_0.htm", "��ڱ���" }, { "club_entry_26_17_0_1_0.htm", "��ɽ��ˮ" }, { "club_entry_1067_17_0_1_0.htm", "��������" }, 
                                    { "club_entry_120_17_0_1_0.htm", "�α�����" }, { "club_entry_1096_17_0_1_0.htm", "�߽���ɽ" }, { "club_entry_1020_17_0_1_0.htm", "������" }, 
                                    { "club_entry_1145_17_0_1_0.htm", "����DIY" }, { "club_entry_1160_17_0_1_0.htm", "�� �� ɽ" }, { "club_entry_71_13_0_1_0.htm", "��Ӱ԰��" }, 
                                    { "club_entry_166_13_0_1_0.htm", "���ǰ���" }, { "club_entry_167_13_0_1_0.htm", "��Цͼ��" }, { "club_entry_168_13_0_1_0.htm", "��ͼ����" }, 
                                    { "club_entry_169_13_0_1_0.htm", "�羰��ͼ" }, { "club_entry_170_13_0_1_0.htm", "����ͼƬ" }, { "club_entry_165_13_0_1_0.htm", "��Ů��ͼ" }, 
                                    { "club_entry_172_13_0_1_0.htm", "���͵۹�" }, { "club_entry_1018_13_0_1_0.htm", "�ģ�����" }, { "club_entry_1030_13_0_1_0.htm", "����֮��" }, 
                                    { "club_entry_1076_13_0_1_0.htm", "��������" }, { "club_entry_1082_13_0_1_0.htm", "Ӿ װ ��" }, { "club_entry_47_14_0_1_0.htm", "��ʦ��̳" }, 
                                    { "club_entry_29_14_0_1_0.htm", "����֮��" }, { "club_entry_37_14_0_1_0.htm", "������̳" }, { "club_entry_1080_14_0_1_0.htm", "��չ���" }, 
                                    { "club_entry_1123_14_0_1_0.htm", "��ʦ��ѯ" }, { "club_entry_93_14_0_1_0.htm", "����ʹ��" }, { "club_entry_112_14_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_118_14_0_1_0.htm", "Ӫ����Ӣ" }, { "club_entry_119_14_0_1_0.htm", "�� �� Ա" }, { "club_entry_126_14_0_1_0.htm", "��ҵɳ��" }, 
                                    { "club_entry_159_14_0_1_0.htm", "��ƿռ�" }, { "club_entry_164_14_0_1_0.htm", "Ʊ����̳" }, { "club_entry_69_14_0_1_0.htm", "�˲���ְ" }, 
                                    { "club_entry_1021_14_0_1_0.htm", "������վ" }, { "club_entry_1089_14_0_1_0.htm", "�ؼۻ�Ʊ" }, { "club_entry_1091_14_0_1_0.htm", "˾������" }, 
                                    { "club_entry_1116_14_0_1_0.htm", "��Ʊ����" }, { "club_entry_1122_14_0_1_0.htm", "����˰��" }, { "club_entry_3_14_0_1_0.htm", "���Լ���" }, 
                                    { "club_entry_111_14_0_1_0.htm", "��վ����" }, { "club_entry_1012_14_0_1_0.htm", "������̳" }, { "club_entry_1029_14_0_1_0.htm", "����ش�" }, 
                                    { "club_entry_73_14_0_1_0.htm", "����˽Ӫ" }, { "club_entry_1092_14_0_1_0.htm", "������" }, { "club_entry_1137_14_0_1_0.htm", "�߶�Ʒ��" }, 
                                    { "club_entry_1090_14_0_1_0.htm", "�ҵ���̳" }, { "club_entry_1019_14_0_1_0.htm", "��ʿ����" }, { "club_entry_1050_14_0_1_0.htm", "�����幤" }, 
                                    { "club_entry_114_14_0_1_0.htm", "�㲥����" }, { "club_entry_1034_14_0_1_0.htm", "������" }, { "club_entry_1110_14_0_1_0.htm", "����糡" }, 
                                    { "club_entry_1134_14_0_1_0.htm", "�����ൺ" }, { "club_entry_143_14_0_1_0.htm", "�������" }, { "club_entry_1135_14_0_1_0.htm", "֪ʶ����" }, 
                                    { "club_entry_152_14_0_1_0.htm", "���̽��" }, { "club_entry_1164_14_0_1_0.htm", "�ԣ��ֻ�" }, { "club_entry_1174_14_0_1_0.htm", "�鱦����" }, 
                                    { "club_entry_1175_14_0_1_0.htm", "�ൺ����" }, { "club_entry_1020_14_0_1_0.htm", "������" }, { "club_entry_1182_14_0_1_0.htm", "����˵��" }, 
                                    { "club_entry_1194_14_0_1_0.htm", "�����˼�" }, { "club_entry_1197_14_0_1_0.htm", "���ӿ���" }, { "club_entry_1198_14_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_44_15_0_1_0.htm", "��³��̳" }, { "club_entry_2_15_0_1_0.htm", "�ൺ��̳ " }, { "club_entry_135_15_0_1_0.htm", "������̳" }, 
                                    { "club_entry_99_15_0_1_0.htm", "��̨��̳" }, { "club_entry_91_15_0_1_0.htm", "�Ͳ���̳" }, { "club_entry_100_15_0_1_0.htm", "Ϋ����̳" }, 
                                    { "club_entry_101_15_0_1_0.htm", "������̳" }, { "club_entry_102_15_0_1_0.htm", "������̳" }, { "club_entry_103_15_0_1_0.htm", "������̳" }, 
                                    { "club_entry_105_15_0_1_0.htm", "�ĳ���̳" }, { "club_entry_106_15_0_1_0.htm", "��ׯ��̳" }, { "club_entry_110_15_0_1_0.htm", "̩����̳" }, 
                                    { "club_entry_115_15_0_1_0.htm", "������̳" }, { "club_entry_136_15_0_1_0.htm", "������̳" }, { "club_entry_137_15_0_1_0.htm", "��Ӫ��̳" }, 
                                    { "club_entry_138_15_0_1_0.htm", "������̳" }, { "club_entry_139_15_0_1_0.htm", "������̳" }, { "club_entry_140_15_0_1_0.htm", "������̳" }, 
                                    { "club_entry_30_16_0_1_0.htm", "��������" }, { "club_entry_68_16_0_1_0.htm", "���ѽ���" }, { "club_entry_87_16_0_1_0.htm", "����֮��" }, 
                                    { "club_entry_1093_16_0_1_0.htm", "����֮��" } };
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

        #region �����Ŀ�б�
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

        #region ȫѡ��Ŀ�б�
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

        #region ɾ����Ŀ�б�
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

        #region ��¼��֤
        /// <summary>
        /// ��¼��֤
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        /// <param name="strLoginUrl"></param>
        /// <param name="Txt"></param>
        public void Login(string strUserName, string strPassword, string strLoginUrl, out string Txt)
        {
            try
            {
                //����Cookie����
                CookieContainer CookieArray = new CookieContainer();

                //����Http����
                HttpWebRequest LoginHttpWebRequest = (HttpWebRequest)WebRequest.Create(strLoginUrl);

                //��¼����
                string LoginData = "id=" + UrlEncode(strUserName,"UTF-8") + "&passwd=" + strPassword + "&usertype=0";
                //���ݱ���������
                LoginHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //���ݳ���
                LoginHttpWebRequest.ContentLength = LoginData.Length;
                //���ݴ��䷽�� get��post
                LoginHttpWebRequest.Method = "POST";
                //����HttpWebRequest��CookieContainerΪ�ղŽ������Ǹ�CookieArray  
                LoginHttpWebRequest.CookieContainer = CookieArray;
                //��ȡ��¼������
                Stream myRequestStream = LoginHttpWebRequest.GetRequestStream();
                //StreamWriter
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                //������д��HttpWebRequest��Request��  
                myStreamWriter.Write(LoginData);

                //�رմ򿪶���     
                myStreamWriter.Close();

                myRequestStream.Close();

                //�½�һ��HttpWebResponse     
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)LoginHttpWebRequest.GetResponse();

                //��ȡһ������url��Cookie���ϵ�CookieCollection     
                myHttpWebResponse.Cookies = CookieArray.GetCookies(LoginHttpWebRequest.RequestUri);

                if (myHttpWebResponse.Cookies.Count > 0)    //�Զ����Cookies
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
                    this.Text = "Sender - " + strUserName + "    ����" + intSendCount + "��";
                    string strOut;
                    LoginSec(strUserName, strCookieSessionID, strCookieUserName, strCookiePassowrd, out strOut);
                }
                else
                {
                    toolStripStatusLabel1.Text = "Login.ERROR";
                }

                //�����ݴ�HttpWebResponse��Response���ж���     
                myStreamReader.Close();

                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ������֤
        /// <summary>
        /// ������֤
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
                //����Cookie����
                CookieContainer CookieArray = new CookieContainer();

                //����Http����
                HttpWebRequest LoginHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://club.qingdaonews.com/login_club_new1.php");

                //��¼����
                //string LoginData = "id=" + strUserName + "&passwd=" + strPassword + "&usertype=0";
                //���ݱ���������
                //LoginHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //���ݳ���
                //LoginHttpWebRequest.ContentLength = LoginData.Length;
                //���ݴ��䷽�� get��post
                LoginHttpWebRequest.Method = "POST";
                LoginHttpWebRequest.Accept = "*/*";
                LoginHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; MAXTHON 2.0)";
                //����HttpWebRequest��CookieContainerΪ�ղŽ������Ǹ�CookieArray  
                LoginHttpWebRequest.CookieContainer = CookieArray;
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("PHPSESSID", strCookieSessionID));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[username]", strCookieUserName));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[password]", strCookiePassword));

                //��ȡ��¼������
                Stream myRequestStream = LoginHttpWebRequest.GetRequestStream();
                //StreamWriter
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                //������д��HttpWebRequest��Request��  
                //myStreamWriter.Write(LoginData);

                //�رմ򿪶���     
                //myStreamWriter.Close();

                myRequestStream.Close();

                //�½�һ��HttpWebResponse     
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)LoginHttpWebRequest.GetResponse();

                //��ȡһ������url��Cookie���ϵ�CookieCollection     
                myHttpWebResponse.Cookies = CookieArray.GetCookies(LoginHttpWebRequest.RequestUri);

                if (myHttpWebResponse.Cookies.Count > 0)
                {
                    CookieArray.Add(myHttpWebResponse.Cookies);
                }

                WebHeaderCollection a = myHttpWebResponse.Headers;

                Stream myResponseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("gb2312"));

                strShowBox = myStreamReader.ReadToEnd();

                //�����ݴ�HttpWebResponse��Response���ж���     
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

        #region ������̳��Ϣ
        /// <summary>
        /// ������̳��Ϣ
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
            toolStripStatusLabel1.Text = "��������" + strURL;
            try
            {
                //����Cookie����
                CookieContainer CookieArray = new CookieContainer();

                //����Http����
                HttpWebRequest LoginHttpWebRequest = (HttpWebRequest)WebRequest.Create(strURL);

                //��¼����
                //string LoginData = "id=" + strUserName + "&passwd=" + strPassword + "&usertype=0";
                //���ݱ���������
                //LoginHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //���ݳ���
                //LoginHttpWebRequest.ContentLength = LoginData.Length;
                //���ݴ��䷽�� get��post
                LoginHttpWebRequest.Method = "POST";
                LoginHttpWebRequest.Accept = "*/*";
                LoginHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; MAXTHON 2.0)";
                //����HttpWebRequest��CookieContainerΪ�ղŽ������Ǹ�CookieArray  
                LoginHttpWebRequest.CookieContainer = CookieArray;
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("PHPSESSID", strCookieSessionID));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[username]", strCookieUserName));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[password]", strCookiePassword));
                CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews", ""));

                //��ȡ��¼������
                Stream myRequestStream = LoginHttpWebRequest.GetRequestStream();
                //StreamWriter
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                //������д��HttpWebRequest��Request��  
                //myStreamWriter.Write(LoginData);

                //�رմ򿪶���     
                //myStreamWriter.Close();

                myRequestStream.Close();

                //�½�һ��HttpWebResponse     
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)LoginHttpWebRequest.GetResponse();

                //��ȡһ������url��Cookie���ϵ�CookieCollection     
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

                //�����ݴ�HttpWebResponse��Response���ж���     
                myStreamReader.Close();

                myResponseStream.Close();

                #region ��ȡ��Ŀ���Ӻ�����
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
                //            resultString_0 = Regex.Match(matchResults.ToString(), @"club_entry_\d{1,9}_\d{1,9}_\d{1,9}_\d{1,9}_\d{1,9}\.htm").Value;    //��Ŀ���� 
                //            resultString_1 = Regex.Match(matchResults.ToString(), @">[\u4e00-\u9fa5,\uFF00-\uFFFF, ,0-9,\w]{1,5}<").Value;              //��Ŀ����
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

                #region ��ȡ�����б�
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
                //        li.SubItems.Add(matchResults.Value.ToString());     //��������
                //        listView2.Items.Add(li);
                //        num++;  //���ID
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

                toolStripStatusLabel1.Text = "���ڷ��ʣ�" + strURL;
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
                    //string strMessages = "·�����͵�˵��";
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

        #region ��������
        /// <summary>
        /// ��������
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
            toolStripStatusLabel1.Text = "��������http://club.qingdaonews.com/" + strAnnounceID;
            try
            {
                string strParent_id = strTopic_id;

                //����Cookie����
                CookieContainer CookieArray = new CookieContainer();

                //����Http����
                HttpWebRequest LoginHttpWebRequest = (HttpWebRequest)WebRequest.Create(strURL);

                //���ݸ�ʽ
                //viewmode=
                //topic_id=3807645
                //parent_id=3807645
                //board_id=138
                //Page=1
                //a_name=snoopy6973
                //subject=%BB%D8%B8%B4%3A
                //ubb=UBB
                //chkSignature=1
                //body=�ðɣ���Ҳ��·�����͵ġ�
                //insertimg=

                //��¼����
                string LoginData = "viewmode=&topic_id=" + strTopic_id + "&parent_id=" + strParent_id + "&board_id=" + strBoard_id + "&Page=1&a_name=" + strUserName + "&subject=%BB%D8%B8%B4%3A&ubb=UBB&chkSignature=1&body=" + strMessages + "&insertimg=";
                //string LoginData = "viewmode=&topic_id=" + strTopic_id + "&parent_id=" + strParent_id + "&board_id=" + strBoard_id + "&Page=1&a_name=" + strUserName + "&subject=%BB%D8%B8%B4%3A&ubb=UBB&chkSignature=1&body=%7B201%7D&insertimg=";
                //���ݱ���������
                LoginHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                Encoding encode = System.Text.Encoding.Default;
                byte[] arrB = encode.GetBytes(LoginData);
                //���ݳ���
                //LoginHttpWebRequest.ContentLength = LoginData.Length;
                LoginHttpWebRequest.ContentLength = arrB.Length;
                //���ݴ��䷽�� get��post
                LoginHttpWebRequest.Method = "POST";
                LoginHttpWebRequest.Accept = "*/*";
                LoginHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; MAXTHON 2.0)";
                LoginHttpWebRequest.Referer = "http://club.qingdaonews.com/" + strAnnounceID;
                //LoginHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
                //����HttpWebRequest��CookieContainerΪ�ղŽ������Ǹ�CookieArray  
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

                ////��ȡ��¼������
                //Stream myRequestStream = LoginHttpWebRequest.GetRequestStream();
                ////StreamWriter
                //StreamWriter myStreamWriter = new StreamWriter(myRequestStream, encode); //Encoding.GetEncoding("gb2312")
                ////������д��HttpWebRequest��Request��  
                //myStreamWriter.Write(arrB);

                ////�رմ򿪶���     
                //myStreamWriter.Close();

                //myRequestStream.Close();

                ////�½�һ��HttpWebResponse     
                //HttpWebResponse myHttpWebResponse = (HttpWebResponse)LoginHttpWebRequest.GetResponse();

                //��ȡһ������url��Cookie���ϵ�CookieCollection     
                myHttpWebResponse.Cookies = CookieArray.GetCookies(LoginHttpWebRequest.RequestUri);

                if (myHttpWebResponse.Cookies.Count > 0)
                {
                    CookieArray.Add(myHttpWebResponse.Cookies);
                }

                WebHeaderCollection a = myHttpWebResponse.Headers;

                Stream myResponseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("gb2312"));

                strShowBox = myStreamReader.ReadToEnd();

                //�����ݴ�HttpWebResponse��Response���ж���     
                myStreamReader.Close();

                myResponseStream.Close();
                //========================================================================
                //http://club.qingdaonews.com/showAnnounce.php?board_id=2&topic_id=4577258

                
                toolStripStatusLabel1.Text = "�����ɹ���http://club.qingdaonews.com/" + strAnnounceID;
                text_SendRecord.Text += "�����ɹ���http://club.qingdaonews.com/" + strAnnounceID + "  ʱ�䣺" + System.DateTime.Now + "\r\n";
                //MessageBox.Show("�����ɹ���http://club.qingdaonews.com/" + strAnnounceID);
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

        #region ��ӵ�¼�û�
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
                    MessageBox.Show("�û��������벻�ܿ����飡", "��ʾ");
                }
            }
            else
            {
                f3.Close();
            }
        }
        #endregion

        #region ɾ����¼�û�
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

        #region �û���Ϣ���������
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

        #region ���ɼ�¼�ļ�
        private void CreateTxt(DataTable dt)
        {
            if (File.Exists(@"LoginUser.txt"))
            {
                File.Delete(@"LoginUser.txt");
            }
            FileStream fs = new FileStream(@"LoginUser.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));//ͨ��ָ���ַ����뷽ʽ����ʵ�ֶԺ��ֵ�֧�֣��������ü��±��򿪲鿴���������
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

        #region ��ȡ��¼�ļ�
        private void ReadTxt()
        {
            string[] arrInfo;
            if (File.Exists(@"LoginUser.txt"))
            {
                StreamReader sr = File.OpenText(@"LoginUser.txt");
                string strInfo = sr.ReadLine();//��ȡһ���ж��Ƿ�Ϊ��
                while (strInfo != null)
                {
                    arrInfo = strInfo.Split(',');
                    ListViewItem li = new ListViewItem();
                    li.SubItems.Clear();
                    li.SubItems[0].Text = arrInfo[0].ToString().Trim();
                    li.SubItems.Add(arrInfo[1].ToString().Trim());
                    listView3.Items.Add(li);

                    strInfo = sr.ReadLine();//�ж���һ���ж��Ƿ�Ϊ��
                }
                sr.Close();
            }
        }
        #endregion

        #region ��ȡ��̳�б�
        private void ReadClubList()
        {
            string[] arrInfo;
            if (File.Exists(@"ClubList.txt"))
            {
                StreamReader sr = File.OpenText(@"ClubList.txt");
                string strInfo = sr.ReadLine();//��ȡһ���ж��Ƿ�Ϊ��
                while (strInfo != null)
                {
                    arrInfo = strInfo.Split(',');
                    ListViewItem li = new ListViewItem();
                    li.SubItems.Clear();
                    li.SubItems[0].Text = arrInfo[0].ToString().Trim();
                    li.SubItems.Add(arrInfo[1].ToString().Trim());
                    listView4.Items.Add(li);

                    strInfo = sr.ReadLine();//�ж���һ���ж��Ƿ�Ϊ��
                }
                sr.Close();
            }
        }
        #endregion

        #region ��������
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="strClubName">��Ŀ����</param>
        /// <returns></returns>
        public string FindClubLink(string strClubName)
        {
            #region ��̳�б�
            string[,] arrBBS_Club = { { "club_entry_2_2_0_1_0.htm", "�ൺ��̳" }, { "club_entry_1025_2_0_1_0.htm", "�������" }, { "club_entry_48_2_0_1_0.htm", "�⾭��ó" }, 
                                    { "club_entry_67_2_0_1_0.htm", "Ư�ڵ���" }, { "club_entry_57_2_0_1_0.htm", "���е���" }, { "club_entry_9_2_0_1_0.htm", "������ѧ" }, 
                                    { "club_entry_1038_2_0_1_0.htm", "�����ൺ" }, { "club_entry_88_2_0_1_0.htm", "�����黰" }, { "club_entry_123_2_0_1_0.htm", "���µķ�" }, 
                                    { "club_entry_1_2_0_1_0.htm", "̸��˵��" }, { "club_entry_156_2_0_1_0.htm", "�л�Ů��" }, { "club_entry_1018_2_0_1_0.htm", "�ģ�����" }, 
                                    { "club_entry_1023_2_0_1_0.htm", "�ൺ����" }, { "club_entry_1024_2_0_1_0.htm", "ͬ¥��Լ" }, { "club_entry_1115_2_0_1_0.htm", "������̳" }, 
                                    { "club_entry_1133_2_0_1_0.htm", "�ݻ�Ʒλ" }, { "club_entry_41_2_0_1_0.htm", "��������" }, { "club_entry_1030_2_0_1_0.htm", "����֮��" }, 
                                    { "club_entry_1170_2_0_1_0.htm", "�Ұ�����" }, { "club_entry_1171_2_0_1_0.htm", "������Ϸ" }, { "club_entry_1181_2_0_1_0.htm", "�λ�����" }, 
                                    { "club_entry_1188_2_0_1_0.htm", "�� �� ��" }, { "club_entry_1192_2_0_1_0.htm", "��������" }, { "club_entry_1199_2_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_128_3_0_1_0.htm", "�Ƶ���̳" }, { "club_entry_129_3_0_1_0.htm", "������̳" }, { "club_entry_130_3_0_1_0.htm", "��ī��̳" }, 
                                    { "club_entry_131_3_0_1_0.htm", "������̳" }, { "club_entry_132_3_0_1_0.htm", "������̳" }, { "club_entry_133_3_0_1_0.htm", "������̳" }, 
                                    { "club_entry_134_3_0_1_0.htm", "ƽ����̳" }, { "club_entry_173_3_0_1_0.htm", "��ɽ��̳" }, { "club_entry_1053_3_0_1_0.htm", "�ķ���̳" }, 
                                    { "club_entry_1054_3_0_1_0.htm", "�����̳" }, { "club_entry_1059_3_0_1_0.htm", "������̳" }, { "club_entry_1060_3_0_1_0.htm", "�б���̳" }, 
                                    { "club_entry_39_4_0_1_0.htm", "������̳" }, { "club_entry_1039_4_0_1_0.htm", "��������" }, { "club_entry_1040_4_0_1_0.htm", "��ͨ·��" }, 
                                    { "club_entry_1041_4_0_1_0.htm", "��װ����" }, { "club_entry_1042_4_0_1_0.htm", "���ֽ���" }, { "club_entry_1043_4_0_1_0.htm", "��Խ����" }, 
                                    { "club_entry_1044_4_0_1_0.htm", "��ŷ����" }, { "club_entry_1045_4_0_1_0.htm", "�� �� ��" }, { "club_entry_1046_4_0_1_0.htm", "��ɣ����" }, 
                                    { "club_entry_1047_4_0_1_0.htm", "���ڳ���" }, { "club_entry_1048_4_0_1_0.htm", "���Դ��" }, { "club_entry_1049_4_0_1_0.htm", "ǧ�����" }, 
                                    { "club_entry_1052_4_0_1_0.htm", "ԽҰ���" }, { "club_entry_1055_4_0_1_0.htm", "ʨ �� ��" }, { "club_entry_1056_4_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_1057_4_0_1_0.htm", "��������" }, { "club_entry_1061_4_0_1_0.htm", "�Լ�����" }, { "club_entry_1062_4_0_1_0.htm", "ѩ������" }, 
                                    { "club_entry_1063_4_0_1_0.htm", "�ִ�����" }, { "club_entry_1069_4_0_1_0.htm", "�� �� ��" }, { "club_entry_1070_4_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_1071_4_0_1_0.htm", "��������" }, { "club_entry_154_4_0_1_0.htm", "˳ �� ��" }, { "club_entry_1085_4_0_1_0.htm", "ͼ �� ��" }, 
                                    { "club_entry_1086_4_0_1_0.htm", "ͬ �� ��" }, { "club_entry_1179_4_0_1_0.htm", "��������" }, { "club_entry_1102_4_0_1_0.htm", "�ձ�����" }, 
                                    { "club_entry_1108_4_0_1_0.htm", "����˹��" }, { "club_entry_1109_4_0_1_0.htm", "���ﳵ��" }, { "club_entry_1111_4_0_1_0.htm", "˼ �� ��" }, 
                                    { "club_entry_1121_4_0_1_0.htm", "���ϳ���" }, { "club_entry_1124_4_0_1_0.htm", "�� �� ��" }, { "club_entry_1130_4_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_1131_4_0_1_0.htm", "�� �� ��" }, { "club_entry_1143_4_0_1_0.htm", "�µϳ���" }, { "club_entry_1157_4_0_1_0.htm", "80�󳵻�" }, 
                                    { "club_entry_1158_4_0_1_0.htm", "��������" }, { "club_entry_1159_4_0_1_0.htm", "���ڳ���" }, { "club_entry_1169_4_0_1_0.htm", "�㱾����" }, 
                                    { "club_entry_1173_4_0_1_0.htm", "����������" }, { "club_entry_1184_4_0_1_0.htm", "��Уѧ��" }, { "club_entry_1185_4_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_1190_4_0_1_0.htm", "�� �� ��" }, { "club_entry_1172_4_0_1_0.htm", "���г�Ȧ" }, { "club_entry_20_5_0_1_0.htm", "�ൺ¥��" }, 
                                    { "club_entry_1010_5_0_1_0.htm", "�ز�Ӫ��" }, { "club_entry_49_5_0_1_0.htm", "�Ҿ�����" }, { "club_entry_1139_5_0_1_0.htm", "�Ҿ���Ʒ" }, 
                                    { "club_entry_1115_6_0_1_0.htm", "������̳" }, { "club_entry_47_6_0_1_0.htm", "��ʦ��̳" }, { "club_entry_66_6_0_1_0.htm", "�������" }, 
                                    { "club_entry_12_6_0_1_0.htm", "Ӣ������" }, { "club_entry_86_6_0_1_0.htm", "����ѧϰ" }, { "club_entry_163_6_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_13_6_0_1_0.htm", "��ѧУ԰" }, { "club_entry_94_6_0_1_0.htm", "��ѧʱ��" }, { "club_entry_83_6_0_1_0.htm", "���Խ���" }, 
                                    { "club_entry_174_6_0_1_0.htm", "�����ѧ" }, { "club_entry_175_6_0_1_0.htm", "�ൺ��ѧ" }, { "club_entry_176_6_0_1_0.htm", "�ൺ�ƴ�" }, 
                                    { "club_entry_177_6_0_1_0.htm", "����ѧ" }, { "club_entry_1006_6_0_1_0.htm", "ɽ���ƴ�" }, { "club_entry_1007_6_0_1_0.htm", "��óѧԺ" }, 
                                    { "club_entry_1011_6_0_1_0.htm", "ʯ�ʹ�ѧ" }, { "club_entry_1031_6_0_1_0.htm", "�ൺũ��" }, { "club_entry_58_7_0_1_0.htm", "������̳" }, 
                                    { "club_entry_1005_7_0_1_0.htm", "�����̳" }, { "club_entry_160_7_0_1_0.htm", "����ѧͯ" }, { "club_entry_121_7_0_1_0.htm", "�� �� ԰" }, 
                                    { "club_entry_1099_7_0_1_0.htm", "����ѧ��" }, { "club_entry_1100_7_0_1_0.htm", "�ٶ�����" }, { "club_entry_64_8_0_1_0.htm", "Ͷ�����" }, 
                                    { "club_entry_1064_8_0_1_0.htm", "̸���۽�" }, { "club_entry_1094_8_0_1_0.htm", "�󻰻���" }, { "club_entry_1127_8_0_1_0.htm", "�Ҽ����" },
                                    { "club_entry_1128_8_0_1_0.htm", "�ղؼ���" }, { "club_entry_1142_8_0_1_0.htm", "����֮��" }, { "club_entry_1201_8_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_4_9_0_1_0.htm", "�����ݺ�" }, { "club_entry_5_9_0_1_0.htm", "������ţ" }, { "club_entry_36_9_0_1_0.htm", "64����̳" }, 
                                    { "club_entry_52_9_0_1_0.htm", "ŷ½���" }, { "club_entry_74_9_0_1_0.htm", "�Ұ�����" }, { "club_entry_76_9_0_1_0.htm", "������̳" }, 
                                    { "club_entry_92_9_0_1_0.htm", "Χ�����" }, { "club_entry_124_9_0_1_0.htm", "��������" }, { "club_entry_125_9_0_1_0.htm", "���˷緫" }, 
                                    { "club_entry_146_9_0_1_0.htm", "��������" }, { "club_entry_162_9_0_1_0.htm", "�� �� ��" }, { "club_entry_1015_9_0_1_0.htm", "�й�����" }, 
                                    { "club_entry_1016_9_0_1_0.htm", "�������" }, { "club_entry_1033_9_0_1_0.htm", "̨����̳" }, { "club_entry_1068_9_0_1_0.htm", "�������" }, 
                                    { "club_entry_1084_9_0_1_0.htm", "�٤��Ů" }, { "club_entry_1125_9_0_1_0.htm", "С������" }, { "club_entry_1151_9_0_1_0.htm", "��������" }, 
                                    { "club_entry_33_10_0_1_0.htm", "������Լ" }, { "club_entry_93_10_0_1_0.htm", "����ʹ��" }, { "club_entry_77_10_0_1_0.htm", "������ѯ" }, 
                                    { "club_entry_1072_10_0_1_0.htm", "ҽ����е " }, { "club_entry_1073_10_0_1_0.htm", "��ҽ��ҩ" }, { "club_entry_1027_10_0_1_0.htm", "���ݻ���" }, 
                                    { "club_entry_1066_10_0_1_0.htm", "�������� " }, { "club_entry_1077_10_0_1_0.htm", "���Ի��� " }, { "club_entry_1101_10_0_1_0.htm", "����Ů�� " }, 
                                    { "club_entry_1117_10_0_1_0.htm", "��������" }, { "club_entry_1189_10_0_1_0.htm", "��������" }, { "club_entry_40_11_0_1_0.htm", "Ů�����" }, 
                                    { "club_entry_1026_11_0_1_0.htm", "���齻��" }, { "club_entry_24_11_0_1_0.htm", "������Ů" }, { "club_entry_43_11_0_1_0.htm", "Χ������" }, 
                                    { "club_entry_54_11_0_1_0.htm", "�˵�����" }, { "club_entry_70_11_0_1_0.htm", "������ζ" }, { "club_entry_72_11_0_1_0.htm", "��ʮ����" }, 
                                    { "club_entry_81_11_0_1_0.htm", "����Ƶ��" }, { "club_entry_84_11_0_1_0.htm", "Ե������" }, { "club_entry_122_11_0_1_0.htm", "Ϧ��ϼ��" }, 
                                    { "club_entry_108_11_0_1_0.htm", "��������" }, { "club_entry_22_11_0_1_0.htm", "���˲���" }, { "club_entry_61_11_0_1_0.htm", "��������" }, 
                                    { "club_entry_8_11_0_1_0.htm", "�����߰�" }, { "club_entry_42_11_0_1_0.htm", "��ѧԭ��" }, { "club_entry_1126_11_0_1_0.htm", "��ϱ��ϵ" }, 
                                    { "club_entry_1129_11_0_1_0.htm", "������ͥ" }, { "club_entry_1156_11_0_1_0.htm", "ʫ�黭��" }, { "club_entry_145_11_0_1_0.htm", "������̸" },
                                    { "club_entry_107_11_0_1_0.htm", "�� �� ��" }, { "club_entry_144_11_0_1_0.htm", "ҧ�Ľ���" }, { "club_entry_1191_11_0_1_0.htm", "ɢ��ʫ��" }, 
                                    { "club_entry_1144_11_0_1_0.htm", "������˽" }, { "club_entry_7_12_0_1_0.htm", "Ц�����" }, { "club_entry_27_12_0_1_0.htm", "��ʳ�㳡" }, 
                                    { "club_entry_1193_12_0_1_0.htm", "ʳȫʮ��" }, { "club_entry_1183_12_0_1_0.htm", "�����˸�" }, { "club_entry_113_12_0_1_0.htm", "Ư��Ů��" }, 
                                    { "club_entry_46_12_0_1_0.htm", "ʱ������" }, { "club_entry_60_12_0_1_0.htm", "������̳" }, { "club_entry_1187_12_0_1_0.htm", "��Ҫ���" }, 
                                    { "club_entry_82_12_0_1_0.htm", "�ֹ����" }, { "club_entry_98_12_0_1_0.htm", "�ư��Ļ�" }, { "club_entry_1168_12_0_1_0.htm", "ơ����̳" }, 
                                    { "club_entry_1180_12_0_1_0.htm", "�ɿ�����" }, { "club_entry_1075_12_0_1_0.htm", "�Ժ�����" }, { "club_entry_1078_12_0_1_0.htm", "�������" }, 
                                    { "club_entry_1087_12_0_1_0.htm", "��������" }, { "club_entry_38_12_0_1_0.htm", "��������" }, { "club_entry_141_12_0_1_0.htm", "��ˮ��԰" }, 
                                    { "club_entry_96_12_0_1_0.htm", "�ֻ���԰" }, { "club_entry_97_12_0_1_0.htm", "���г���" }, { "club_entry_109_12_0_1_0.htm", "�������" }, 
                                    { "club_entry_1032_12_0_1_0.htm", "Ӱ������" }, { "club_entry_147_12_0_1_0.htm", "ռ����Ե" }, { "club_entry_1013_12_0_1_0.htm", "�����Ҵ�" }, 
                                    { "club_entry_1149_12_0_1_0.htm", "�� �� Ȧ" }, { "club_entry_149_12_0_1_0.htm", "�� �� Ȧ" }, { "club_entry_1165_12_0_1_0.htm", "�赸ʱ��" }, 
                                    { "club_entry_1200_12_0_1_0.htm", "��ڱ���" }, { "club_entry_26_17_0_1_0.htm", "��ɽ��ˮ" }, { "club_entry_1067_17_0_1_0.htm", "��������" }, 
                                    { "club_entry_120_17_0_1_0.htm", "�α�����" }, { "club_entry_1096_17_0_1_0.htm", "�߽���ɽ" }, { "club_entry_1020_17_0_1_0.htm", "������" }, 
                                    { "club_entry_1145_17_0_1_0.htm", "����DIY" }, { "club_entry_1160_17_0_1_0.htm", "�� �� ɽ" }, { "club_entry_71_13_0_1_0.htm", "��Ӱ԰��" }, 
                                    { "club_entry_166_13_0_1_0.htm", "���ǰ���" }, { "club_entry_167_13_0_1_0.htm", "��Цͼ��" }, { "club_entry_168_13_0_1_0.htm", "��ͼ����" }, 
                                    { "club_entry_169_13_0_1_0.htm", "�羰��ͼ" }, { "club_entry_170_13_0_1_0.htm", "����ͼƬ" }, { "club_entry_165_13_0_1_0.htm", "��Ů��ͼ" }, 
                                    { "club_entry_172_13_0_1_0.htm", "���͵۹�" }, { "club_entry_1018_13_0_1_0.htm", "�ģ�����" }, { "club_entry_1030_13_0_1_0.htm", "����֮��" }, 
                                    { "club_entry_1076_13_0_1_0.htm", "��������" }, { "club_entry_1082_13_0_1_0.htm", "Ӿ װ ��" }, { "club_entry_47_14_0_1_0.htm", "��ʦ��̳" }, 
                                    { "club_entry_29_14_0_1_0.htm", "����֮��" }, { "club_entry_37_14_0_1_0.htm", "������̳" }, { "club_entry_1080_14_0_1_0.htm", "��չ���" }, 
                                    { "club_entry_1123_14_0_1_0.htm", "��ʦ��ѯ" }, { "club_entry_93_14_0_1_0.htm", "����ʹ��" }, { "club_entry_112_14_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_118_14_0_1_0.htm", "Ӫ����Ӣ" }, { "club_entry_119_14_0_1_0.htm", "�� �� Ա" }, { "club_entry_126_14_0_1_0.htm", "��ҵɳ��" }, 
                                    { "club_entry_159_14_0_1_0.htm", "��ƿռ�" }, { "club_entry_164_14_0_1_0.htm", "Ʊ����̳" }, { "club_entry_69_14_0_1_0.htm", "�˲���ְ" }, 
                                    { "club_entry_1021_14_0_1_0.htm", "������վ" }, { "club_entry_1089_14_0_1_0.htm", "�ؼۻ�Ʊ" }, { "club_entry_1091_14_0_1_0.htm", "˾������" }, 
                                    { "club_entry_1116_14_0_1_0.htm", "��Ʊ����" }, { "club_entry_1122_14_0_1_0.htm", "����˰��" }, { "club_entry_3_14_0_1_0.htm", "���Լ���" }, 
                                    { "club_entry_111_14_0_1_0.htm", "��վ����" }, { "club_entry_1012_14_0_1_0.htm", "������̳" }, { "club_entry_1029_14_0_1_0.htm", "����ش�" }, 
                                    { "club_entry_73_14_0_1_0.htm", "����˽Ӫ" }, { "club_entry_1092_14_0_1_0.htm", "������" }, { "club_entry_1137_14_0_1_0.htm", "�߶�Ʒ��" }, 
                                    { "club_entry_1090_14_0_1_0.htm", "�ҵ���̳" }, { "club_entry_1019_14_0_1_0.htm", "��ʿ����" }, { "club_entry_1050_14_0_1_0.htm", "�����幤" }, 
                                    { "club_entry_114_14_0_1_0.htm", "�㲥����" }, { "club_entry_1034_14_0_1_0.htm", "������" }, { "club_entry_1110_14_0_1_0.htm", "����糡" }, 
                                    { "club_entry_1134_14_0_1_0.htm", "�����ൺ" }, { "club_entry_143_14_0_1_0.htm", "�������" }, { "club_entry_1135_14_0_1_0.htm", "֪ʶ����" }, 
                                    { "club_entry_152_14_0_1_0.htm", "���̽��" }, { "club_entry_1164_14_0_1_0.htm", "�ԣ��ֻ�" }, { "club_entry_1174_14_0_1_0.htm", "�鱦����" }, 
                                    { "club_entry_1175_14_0_1_0.htm", "�ൺ����" }, { "club_entry_1020_14_0_1_0.htm", "������" }, { "club_entry_1182_14_0_1_0.htm", "����˵��" }, 
                                    { "club_entry_1194_14_0_1_0.htm", "�����˼�" }, { "club_entry_1197_14_0_1_0.htm", "���ӿ���" }, { "club_entry_1198_14_0_1_0.htm", "�� �� ��" }, 
                                    { "club_entry_44_15_0_1_0.htm", "��³��̳" }, { "club_entry_2_15_0_1_0.htm", "�ൺ��̳ " }, { "club_entry_135_15_0_1_0.htm", "������̳" }, 
                                    { "club_entry_99_15_0_1_0.htm", "��̨��̳" }, { "club_entry_91_15_0_1_0.htm", "�Ͳ���̳" }, { "club_entry_100_15_0_1_0.htm", "Ϋ����̳" }, 
                                    { "club_entry_101_15_0_1_0.htm", "������̳" }, { "club_entry_102_15_0_1_0.htm", "������̳" }, { "club_entry_103_15_0_1_0.htm", "������̳" }, 
                                    { "club_entry_105_15_0_1_0.htm", "�ĳ���̳" }, { "club_entry_106_15_0_1_0.htm", "��ׯ��̳" }, { "club_entry_110_15_0_1_0.htm", "̩����̳" }, 
                                    { "club_entry_115_15_0_1_0.htm", "������̳" }, { "club_entry_136_15_0_1_0.htm", "������̳" }, { "club_entry_137_15_0_1_0.htm", "��Ӫ��̳" }, 
                                    { "club_entry_138_15_0_1_0.htm", "������̳" }, { "club_entry_139_15_0_1_0.htm", "������̳" }, { "club_entry_140_15_0_1_0.htm", "������̳" }, 
                                    { "club_entry_30_16_0_1_0.htm", "��������" }, { "club_entry_68_16_0_1_0.htm", "���ѽ���" }, { "club_entry_87_16_0_1_0.htm", "����֮��" }, 
                                    { "club_entry_1093_16_0_1_0.htm", "����֮��" } };
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
                string strInfo = sr.ReadLine();//��ȡһ���ж��Ƿ�Ϊ��
                while (strInfo != null)
                {
                    List.Add(strInfo);
                    strInfo = sr.ReadLine();//�ж���һ���ж��Ƿ�Ϊ��
                }
                arrInfo = (string[])List.ToArray(typeof(string));
                return List;
            }
            else
            {
                return null;
            }
        }

        #region ����������̳�б�
        private void LoadClubInfo()
        {
            try
            {
                //����Cookie����
                CookieContainer CookieArray = new CookieContainer();

                //����Http����
                HttpWebRequest LoginHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://club.qingdaonews.com/");

                //��¼����
                //string LoginData = "id=" + strUserName + "&passwd=" + strPassword + "&usertype=0";
                //���ݱ���������
                //LoginHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //���ݳ���
                //LoginHttpWebRequest.ContentLength = LoginData.Length;
                //���ݴ��䷽�� get��post
                //LoginHttpWebRequest.Method = "POST";
                //LoginHttpWebRequest.Accept = "*/*";
                //LoginHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; MAXTHON 2.0)";
                //����HttpWebRequest��CookieContainerΪ�ղŽ������Ǹ�CookieArray  
                //LoginHttpWebRequest.CookieContainer = CookieArray;
                //CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("PHPSESSID", strCookieSessionID));
                //CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[username]", strCookieUserName));
                //CookieArray.Add(new Uri("http://club.qingdaonews.com"), new Cookie("qingdaonews[password]", strCookiePassword));

                //��ȡ��¼������
                //Stream myRequestStream = LoginHttpWebRequest.GetRequestStream();

                //StreamWriter
                //StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                //������д��HttpWebRequest��Request��  
                //myStreamWriter.Write(LoginData);

                //�رմ򿪶���     
                //myStreamWriter.Close();
                //myRequestStream.Close();

                //�½�һ��HttpWebResponse     
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)LoginHttpWebRequest.GetResponse();

                //��ȡһ������url��Cookie���ϵ�CookieCollection     
                //myHttpWebResponse.Cookies = CookieArray.GetCookies(LoginHttpWebRequest.RequestUri);

                //if (myHttpWebResponse.Cookies.Count > 0)
                //{
                //    CookieArray.Add(myHttpWebResponse.Cookies);
                //}

                //WebHeaderCollection a = myHttpWebResponse.Headers;

                Stream myResponseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("gb2312"));

                string strShowBox = myStreamReader.ReadToEnd();

                //�����ݴ�HttpWebResponse��Response���ж���     
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

        #region ��ӷ�������
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

        #region �������������
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

        #region ���ɷ������ݼ�¼
        private void ContentTxt(DataTable dt)
        {
            if (File.Exists(@"SendContent.txt"))
            {
                File.Delete(@"SendContent.txt");
            }
            FileStream fs = new FileStream(@"SendContent.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));//ͨ��ָ���ַ����뷽ʽ����ʵ�ֶԺ��ֵ�֧�֣��������ü��±��򿪲鿴���������
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

        #region ��ȡ�������ݼ�¼
        private void ReadContentTxt()
        {
            if (File.Exists(@"SendContent.txt"))
            {
                StreamReader sr = File.OpenText(@"SendContent.txt");
                string strInfo = sr.ReadLine();//��ȡһ���ж��Ƿ�Ϊ��
                while (strInfo != null)
                {
                    ListViewItem li = new ListViewItem();
                    li.SubItems.Clear();
                    li.SubItems[0].Text = strInfo.Trim();
                    listView6.Items.Add(li);

                    strInfo = sr.ReadLine();//�ж���һ���ж��Ƿ�Ϊ��
                }
                sr.Close();
            }
        }
        #endregion

        #region ɾ����������
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

        #region ������ť�¼�
        private void btn_Send_Click(object sender, EventArgs e)
        {
            
            if (btn_Send.Text == "��ʼ����")
            {
                btn_Send.Text = "ֹͣ����";
                Timer_T();
            }
            else
            {
                btn_Send.Text = "��ʼ����";
                intSendCount = 1;
            }

        }
        #endregion

        #region �����ʱ��
        private void Timer_T()
        {
            System.Timers.Timer t = new System.Timers.Timer();//ʵ����Timer�࣬���ü��ʱ��Ϊ10000���룻
            //SendContent();
            int intSendTime = Convert.ToInt32(text_SendTime.Text.Trim());
            if (intSendTime < 10)
            {
                intSendTime = 10;
            }
            t.Interval = intSendTime * 1000;
            t.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);//����ʱ���ʱ��ִ���¼��� 
            t.AutoReset = false;//������ִ��һ�Σ�false������һֱִ��(true)��
            t.Enabled = true;
            //Timer t = new Timer();
            //t.Interval = 5000;
            //t.Tick += new EventHandler(timer1_Tick);
            //t.Enabled = true;
        }
        #endregion

        private void SendContent()
        {
            int intSentTime = Convert.ToInt32(text_SendTime.Text.Trim());    //����ʱ����
            int intSendUser = Convert.ToInt32(text_SendUser.Text.Trim());    //�����û��л�
            //this.timer1.Enabled = false;     //�رռ�ʱ��

            string strURL = "http://club.qingdaonews.com/sublogin_new.php";
            //string strURL = "http://club.qingdaonews.com/login_club.php";
            string strOut = "";

            if (listView3.Items.Count > 0)
            {
                if (Convert.ToInt32(text_SendUser.Text.Trim()) > 1)     //�ж��û��Ƿ����1���û�
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
                else //���÷���
                {
                    //MessageBox.Show(strUserName + "," + intSendCount);
                    //intSendCount += 1;
                    //Timer_T();
                    Login(listView3.Items[intTempNum].SubItems[0].Text.Trim(), listView3.Items[intTempNum].SubItems[1].Text.Trim(), strURL, out strOut);
                }
            }
            else
            {
                MessageBox.Show("û����ӷ����û�");
                btn_Send.Text = "��ʼ����";
            }
        }

        #region ��ʱ������
        private void timer1_Tick(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            if (btn_Send.Text == "��ʼ����")
            {
                MessageBox.Show("����ֹͣ");
                intSendCount = 1;
            }
            else
            {
                //if (intSendCount == 10)
                //{
                //    btn_Send.Text = "��ʼ����";
                //}
                //else
                //{
                    //MessageBox.Show("run");
                    SendContent();
                //}
            }
        }
        #endregion

        #region URL���� UrlEncode(string str, string encode)
        /// <summary>
        /// URL����
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
            //����Ҫ������ַ�

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.";
            System.Text.Encoder encoder = System.Text.Encoding.GetEncoding(encode).GetEncoder();
            char[] c1 = str.ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //һ���ַ�һ���ַ��ı���

            for (int i = 0; i < c1.Length; i++)
            {
                //����Ҫ����

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