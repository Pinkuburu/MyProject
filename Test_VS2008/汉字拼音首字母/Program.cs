using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace 汉字拼音首字母
{
    class Program
    {
        static void Main(string[] args)
        {
            //1鲁菜
            //string strName = "烤小鸡|雪梨肘棒|素锅烤鸭|锅塌鸡签|锅烧鸡|烤花揽桂鱼|纸包鸡|泰安三美豆腐|芝麻鱼球|拔丝苹果|熬黄花鱼|红烧鱼唇|龙凤双腿|四喜鸭子|泰山赤鳞鱼|酥炸春花肉|炸豆腐丸子|胡椒海参汤|双烤肉|奶汤银肺|换心乌贼|馏鱼片|炸菠菜脯|麻粉肘子|双味蹄筋|荷叶肉|福山烧小鸡|素火腿|冬菇烧蹄筋|砂锅三味|奶汤鲜核桃仁|什锦蜂窝豆腐|山东菜丸|烧罗汉面筋|锅烧鸭|整鱼两吃|焖大虾|蜜汁金枣|龙眼凤肝|蜜汁梨球|凤尾金鱼|蒜苗火鸡排|烤花揽鳜鱼|杨梅虾球|酒香椒盐肘子|奶汤蒲菜|清汤柳叶燕菜|油爆双脆|玛瑙银杏|炒豆腐脑|番茄松鼠鱼|珊瑚金钩|扒酿海参|落叶琵琶虾|南煎丸子|龙潭钓玉牌|软烧豆腐|炸灌汤丸子|紫桂焖大排|全虾三做";
            //2粤菜
            //string strName = "辣拌血蛤|香煎茄片|炒桂花鱼翅|蒜子焖酣鱼|广式卤鸭翅|西汁乳鸽|客家酿豆腐|甘菊猪肚|烧瓤鲜沙虫|子萝鸭片|金陵片皮鸭|口福鸡|花雕鸡|果汁鹌鹑|打边炉|金龙乳猪|红斑二吃|蜜汁叉烧|清烩海参|古法扣全端|乳酸菌果汁酱|京乳藕片|枸杞猪肝瘦肉汤|古老肉|红烧猪手|三色龙虾|三皮丝|嘉禾雁扣|蒜香鲶鱼|牛蒡香羹|雪花片汤|灵芝鸡汤|干炒牛河|金银菜东菇墨鱼煲猪踭|香蕉百合银耳汤|凉拌海蜇|兰度鸽脯|炸子鸡|茄汁明虾|脆薯凤尾虾|鱼胶圆肉炖水鸭|海参炖瘦肉|秘制国药中华鳖|西柠蜜糖乳鸽|皱纹圆蹄|莲子山药粥|剑花蜜枣猪肺汤|园林香液鸡|酸汤龙骨面|红扒羊肉|蜜椒蝴蝶片|五柳脆皮鱼|白玉翡翠|梅菜蒸鱼尾|马铃薯笋焖鸡|肉酿生麸圆|豆泥红枣|苹果牛肉炖煮|罗汉果焖瓜子鸡|椰盅海皇|昆布海澡瘦肉汤|青蒜鲤鱼汤|粟米香菇排骨汤|三蛇羹|爆人参鸡片|双菇炖南蛇|蚝油生菜|黑鸡拆烩老猫公|芥菜咸蛋鱼头汤|香荽鱼松酿银萝|梨花豆腐汤|萝卜泡菜|苹果炖鱼|贝丝扒菜胆|凤梨烩排骨|可口的葡萄醋|牛蒡沙拉|沙丁鱼烙|淳安辣椒酱|溜肝尖|西柚三文鱼|鲜竹牛肉|凉瓜浸田鸡|舒心驻颜老火汤|八珍鲩鱼|清汤银耳|玉须泥鳅汤|什锦烩饭|花生红米饭|虾胶龙凤卷|柠檬鸡|芙蓉煎滑蛋|车前子油焖虾|瓜薏米淡菜汤|罗汉果西洋菜猪踭汤|菜干鸭肾蜜枣汤|蚝皇凤爪|番茄蛋花汤|猴头菇炖竹丝鸡|银耳杏仁百合汤|葛根清肺汤|生鱼葛菜汤|雪花鱼翅|香酥凤腿|西红柿肉片汤|杂豆小麦粥|芥兰炒香肠|槐花猪肠汤|荷叶鸡|润燥黄豆肉排汤|白汁鲳鱼|咖喱海鲜锅|酥盐鸡块|潮州肉冻|红枣炖蚕蛹|梅干菜烧肉|油豆腐镶肉|鹿角菜蛤蚧水鱼汤|石榴鸡|三菇浸鱼云|西式芦笋汤|秘制鸳鸯鸡|芦笋煎黄菜|浅色海鲜豉油|煎丸子|桑杏炖猪肺|扁豆薏米炖鸡脚|姜丝肉蟹|板栗焖仔鸡|鲜粟子鸡肉汤|豆腐咸鱼头汤|芡实猪肚汤|银杞明目汤|蜜豆鱼片|菊花猪肝汤|韭菜炒肉|奶味芦笋汤|麻仁当归猪踭汤|生炊麒麟鱼|凉瓜肉排汤|润肺菜干汤|燕窝炖雪梨|罗汉果八珍汤|山楂核桃茶|番茄海蜇|沙参玉竹老鸭汤|雪梨猪踭汤|淡菜紫菜瘦肉汤|米茸芋丝虾煲|七彩瓤猪肚|乌骨鸡归黄汤|密汁牛蒡|参果炖瘦肉|玉米羹|五彩牛肉丝|元宝牛蒡";
            //3川菜
            //string strName = "醋溜黄瓜|金钱口蘑汤|一品豆腐汤|酿青椒|烤扁担肉|冬菜肉末|干煸鳝背|酱爆鸭块|干烧鱼翅|生爆盐煎肉|芪蒸鹌鹑|鸡丝米粉|箩粉鱼头豆腐汤|炒鸡什件|川西肉豆腐|水晶南瓜|泡菜炒肉末|软炸虾糕|龙眼甜烧白|吉利大虾|锅巴肉片|苦瓜酿肉|糖醋红柿椒|芹黄烧鱼条|炸班指|网油腰卷|百仁全鸭|荔枝鱿鱼卷|软烧仔鲇|玉竹心子|鱼香碎滑肉|巴国玉米糕|生烧筋尾舌|白果烧鸡|鸭掌包|双色玫瑰鱼|酥皮龙虾|鹰扬虎视|七星鱼丸汤|鱼香肉片|鲜花豆腐|荷花豆腐大虾|复元汤|沸腾羊肉|辣炒鱿鱼丝|麻辣白菜|油淋笋鸡|网油包烧鸡|碎米鸡丁|家常鸡块|豆苗炒鸡片|酱酥桃仁|酱爆肉|荷包鱿鱼|沙参心肺汤|盐水肫花|火爆荔枝腰|羊耳鸡塔|原笼玉簪|银杏蒸鸭|一品海参|椒盐八宝鸡|荷包豆腐|烩鸭四宝|麻辣猪肝|恋爱豆腐|菠饺鱼肚|鲜椒嫩仔鸡|虫草鸭舌|回锅肉|爆炒腰花|参麦团鱼|成都蛋汤|叉烧鱼|炒面线|南排杂烩|炸蒸肉|牛膝蹄筋|川味牛肉|荷叶凤脯|果仁排骨|苕菜狮子头|蒲江蟹羹|白汁鱼肚|瓤甜椒|麻油鸡|鱼香腰花|豆鼓鱼|红油耳片|手撕鸡|黄焖鸡块|盐煎肉|珍珠圆子|红椒爆鲜虾|正宗重庆辣子鸡|海鲜拼盘|鱼香荷包蛋|艄公号子鱼|香辣虾|贵妃鸡翅|毛肚火锅|红烧蹄筋|豆腐鲫鱼|生炒蒜苔肉|干煸四季豆|红烧狮子头|葱辣鱼条|雪花鸡淖|羊肉汤锅|红枣煨肘|炖牛掌|网油鱼包|炒豌豆夹|红烧牛腩|蚕豆炒虾仁|凉粉鲫鱼|红油抄手|干煸鳝鱼丝|糊涂鸡|连理双味鱼|南荠烧鸭丁|蓉城鸳鸯卷|虾须牛肉|辣椒蟹|四川家常酸辣汤|泡菜肉末|陈皮牛肉|火鞭牛肉";
            //4湘菜
            //string strName = "紫龙脱袍|麻辣子鸡|组庵鱼翅|发丝百叶|水煮猪杂|煎连壳蟹|干锅腊味河蚌|金鱼戏莲|潇湘猪手|麻辣田鸡腿|辣椒炒肚片|炸八块|芙蓉鲫鱼|葵花虾饼|酸辣鸡丁|风情羊柳|翠竹粉蒸鱼|东安鸡|腊味合蒸|浪花天香鱼|好丝百叶|玻璃鲜墨|口味虾|酸辣肘子|黔味大虾|老妈子带鱼|清汤柴把鸭|马蹄白果蛋花汤|东安子鸡|酸辣狗肉|辣椒鱼|洞庭金龟|麻仁香酥鸭|开屏柴把桂鱼|干锅手撕包菜|菊花青鱼|火腿炒茄瓜|剁椒鱼头|冰糖湘莲";
            //5闽菜
            //string strName = "银耳川鸭|一品鲳|爆炒羊肚|蛋拌豆腐|龙须燕丸|炸鸡排|生炒海蚌|七星丸|沙锅鳝鱼|什锦豆腐煲|香油石鳞|干贝水晶鸡|肉米鱼唇|蛏溜奇|白焯响螺|油条西舌|闽生果|金钱干贝|银杏芋泥|香露全鸡|一品鲍鱼|醉排骨|苦中作乐|玟瑰大虾|马蹄鸡丁|椒盐肚尖|太极芋头|佛跳墙|厦门薄饼|沙司牛尾|八生火锅|水晶蹄膀香肴肉|炒芙蓉蟹|龙身凤尾虾|鱼腩煲|彩色肉丝|荤罗汉|糟片鸭|西瓜盅|干烧牛肉片|芝麻豆腐|拉糟鱼块";
            //6浙菜
            //string strName = "香橙糕|酥炸牛肉卷|藕香芹味|海米烧菜花|红烧猴头蘑|糖醋鸡块|馄饨香蕉卷|油焖春笋|铁扒仔鸡|麻酱白菜丝|拌鸡块|三虾豆腐|糟熘鱼片|拌莴笋|南肉春笋|菜心炒猪肝|瑞士排骨|糖拌菜心|糟油青鱼划水|菠萝龙眼鱼|麻酱拌豆腐|虾米拌豆腐";
            //7苏菜
            //string strName = "扬州干丝|清炖蟹粉狮子头|拆冻鲫鱼|鱼羊鲜|肴肉|珊瑚桂鱼";
            //8徽菜
            //string strName = "鸭黄豆腐|李鸿章杂烩|茶叶熏鸡|徽式双冬|玉兔海参|花菇田鸡|板栗仔鸡|铁狮子头";
            //9其他
            string strName = "酸菜鱼|可乐鸡翅|酸辣土豆丝|东坡肘子|蒜烧排骨|醋熘土豆丝|彩椒鲜虾仁|香菇豆腐饼|百合炒鱼片|鱼香豆腐|鸡蛋炒西红柿|客家小炒|凉拌茄子|炝三丝|青椒肉片|椒盐排骨|酱焖鲫鱼|酸辣口水鸡|土豆炖豆角|双菜炒素鸡|脆炒南瓜丝|芋头扣肉|糖醋鱼|皮蛋豆腐";

            string[] arrName = strName.Split('|');
            Random rnd = new Random();

            foreach (string s in arrName)
            {
                string strPinYin = pinyin.GetChineseSpell(s);
                int intPrice = rnd.Next(1000,10000);
                AddMenu(s, strPinYin, "", intPrice, 1, "", 9, 1, "茴香、干贝、蚯蚓、青椒、大蒜", false, true);
                Console.WriteLine(s + "," + strPinYin + "," + "a" + "," + intPrice + "," + 1 + "," + "b" + "," + 1 + "," + 1 + ",牛黄、枸杞、钙片,false,true");
            }

            //Console.WriteLine(pinyin.GetChineseSpell(strName));
            Console.ReadKey();
        }

        /// <summary>
        /// 添加菜品
        /// </summary>
        /// <param name="strDishName">菜名</param>
        /// <param name="strAbbreviation">缩写</param>
        /// <param name="strAlias">别名</param>
        /// <param name="intPrice">价格</param>
        /// <param name="intUnitID">菜品单位，例如 盘，例 Main_Unit.UnitID</param>
        /// <param name="strDishPhoto">菜品图片地址</param>
        /// <param name="intDishCategory">菜品类别，例如 凉菜，热菜，汤 Main_DishCategory.DishClassID</param>
        /// <param name="intDepartment">所属部门，例如 厨房 Main_Department.DepartmentID</param>
        /// <param name="strIngredients">材料信息</param>
        /// <param name="blRecommend">是否推荐</param>
        /// <param name="blStatus">是否可被使用</param>
        /// <returns>1添加成功 0添加失败或已有</returns>
        public static int AddMenu(string strDishName, string strAbbreviation, string strAlias, int intPrice, int intUnitID, string strDishPhoto, int intDishCategory, int intDepartment, string strIngredients, bool blRecommend, bool blStatus)
        {
            SqlParameter[] sp = new SqlParameter[12];

            sp[0] = new SqlParameter("DishName", SqlDbType.VarChar, 50);
            sp[1] = new SqlParameter("Abbreviation", SqlDbType.VarChar, 50);
            sp[2] = new SqlParameter("Alias", SqlDbType.VarChar, 50);
            sp[3] = new SqlParameter("Price", SqlDbType.Int);
            sp[5] = new SqlParameter("UnitID", SqlDbType.Int);
            sp[6] = new SqlParameter("DishPhoto", SqlDbType.VarChar, 500);
            sp[7] = new SqlParameter("DishCategory", SqlDbType.TinyInt);
            sp[8] = new SqlParameter("Department", SqlDbType.TinyInt);
            sp[9] = new SqlParameter("Ingredients", SqlDbType.VarChar, 100);
            sp[10] = new SqlParameter("Recommend", SqlDbType.Bit);
            sp[11] = new SqlParameter("Status", SqlDbType.Bit);

            sp[0].Value = strDishName;
            sp[1].Value = strAbbreviation;
            sp[2].Value = strAlias;
            sp[3].Value = intPrice;
            sp[5].Value = intUnitID;
            sp[6].Value = strDishPhoto;
            sp[7].Value = intDishCategory;
            sp[8].Value = intDepartment;
            sp[9].Value = strIngredients;
            sp[10].Value = blRecommend;
            sp[11].Value = blStatus;

            return (int)SqlHelper.ExecuteScalar(GetConnString(), CommandType.StoredProcedure, "Main_Menu_AddMenu", sp);
        }

        public static string GetConnString()
        {
            return @"Data Source=ishow.xba.com.cn,1595;Initial Catalog=F4Catering;Persist Security Info=True;User ID=cupid;Password=qweqwe123";
        }
    }
}
