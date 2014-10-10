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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "";
            //listView1.Items.Add("asdfasdfasdF");
            //listView1.Items.Add("asdfasdfasdF");
            //listView1.Items.Add("asdfasdfasdF");
            //listView1.Items.Add("asdfasdfasdF");
            //InitializeListView();
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.FocusedItem != null)
            {
                if (listView1.SelectedItems != null)
                {
                    int i = Convert.ToInt32(listView1.SelectedItems.ToString());

                    MessageBox.Show(listView1.Items[i].SubItems[0].Text);
                    //listView1.SelectedItems[0].Selected();
                    //MessageBox.Show(listView1.SelectedItems[i].SubItems[1].Text);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
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
            Random ran = new Random();
            int RandKey = ran.Next(1, arrClubList.Length);
            label1.Text = arrClubList[RandKey].ToString();
        }
    }
}