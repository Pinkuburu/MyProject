using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Threading;

namespace BuyTicket
{
    public class Publisher
    {
        //声明一个出版的委托
        public delegate void PublishEventHander();
        //在委托的机制下我们建立以个出版事件
        public event PublishEventHander OnPublish;
        //事件必须要在方法里去触发，出版社发布新书方法
        public void issue()
        {
            //如果有人注册了这个事件，也就是这个事件不是空
            if (OnPublish != null)
            {
                Console.WriteLine("最新一期的《火影忍者》今天出版哦！");
                OnPublish();
            }
        }
    }

    class Program
    {
        static bool bl = true;
        static WebClient HTTPproc = new WebClient();
        public static string strCodes = "";
        public static string strCookies = "";

        static void Main(string[] args)
        {
            TrainCode codeThread = new TrainCode("登陆验证码");
            codeThread.Start();
            codeThread.OnCodeEvent += new TrainCode.CodeEventHander(codeThread_OnCodeEvent);

            Console.Write("用户名:");
            string strUserName = Console.ReadLine();
            Console.Write("密码:");
            string strPassword = Console.ReadLine();

            Console.WriteLine(strUserName + "|" + strPassword + "|" + strCodes + "|" + strCookies);

            bool isLogin = Login(strUserName, strPassword, strCodes);
            Console.WriteLine(isLogin);
            Console.WriteLine(CheckLogin());
            //while (bl)
            //{
            //    SearchTicket("2014-01-15", Station.北京, Station.邯郸, "ADULT");
            //    Thread.Sleep(1000);
            //}
            
            Console.ReadKey();
        }

        static void codeThread_OnCodeEvent(string strCode, string strCookie)
        {
            strCodes = strCode;
            strCookies = strCookie;
        }

        private static void SearchTicket(string strTrain_Date, string strFrom_Station, string strTo_Station, string strPurpost_Codes)
        {
            int intDay = 0;
            int intzy_num = 0;
            int intze_num = 0;
            string strContent = TicketCountContent(strTrain_Date, strFrom_Station, strTo_Station, strPurpost_Codes);
            JObject jo = JObject.Parse(strContent);
            JArray ja = (JArray)JsonConvert.DeserializeObject(jo["data"]["datas"].ToString());

            List<TrainTicket> Trainlist = JsonConvert.DeserializeObject<List<TrainTicket>>(ja.ToString());
            foreach (TrainTicket t in Trainlist)
            {
                if (t.zy_num == "--" || t.zy_num == "无")
                {
                    t.zy_num = "0";
                }

                if (t.ze_num == "--" || t.ze_num == "无")
                {
                    t.ze_num = "0";
                }
                intzy_num = Convert.ToInt32(t.zy_num);
                intze_num = Convert.ToInt32(t.ze_num);
                if (intzy_num > 1 || intze_num > 1)
                {
                    Console.WriteLine("车号：{0} 一等座：{1} 二等座：{2}", t.station_train_code, t.zy_num, t.ze_num);
                    bl = false;
                }                
            }
        }

        private static void SearchTicket(string strTrain_Date, string strFrom_Station, string strTo_Station, string strPurpost_Codes, string strStartTime, string strEndTime)
        {
            DateTime dtStartTime = Convert.ToDateTime(strStartTime);
            DateTime dtEndTime = Convert.ToDateTime(strEndTime);
            DateTime dtST = new DateTime();
            DateTime dtET = new DateTime();
            int intDay = 0;
            string strContent = TicketCountContent(strTrain_Date, strFrom_Station, strTo_Station, strPurpost_Codes);
            JObject jo = JObject.Parse(strContent);
            JArray ja = (JArray)JsonConvert.DeserializeObject(jo["data"]["datas"].ToString());

            List<TrainTicket> Trainlist = JsonConvert.DeserializeObject<List<TrainTicket>>(ja.ToString());
            foreach (TrainTicket t in Trainlist)
            {
                dtST = Convert.ToDateTime(strTrain_Date + " " + t.start_time);
                dtET = Convert.ToDateTime(strTrain_Date + " " + t.arrive_time);
                intDay = Convert.ToInt32(t.day_difference);
                dtET = dtET.AddDays(intDay);
                if (dtStartTime < dtST && dtEndTime > dtET)
                {
                    Console.WriteLine("车号：{0} 途开时：{1} 途经：{2} 途到时：{3} 途到：{4}", t.station_train_code, t.start_time, t.from_station_name, t.arrive_time, t.to_station_name);
                }                
            }
        }

        private static string TicketContent(string strTrain_Date,string strFrom_Station,string strTo_Station,string strPurpost_Codes)
        {
            WebClient httpST = new WebClient();
            httpST.Encoding = Encoding.UTF8;
            string strRequest = "http://kyfw.12306.cn/otn/leftTicket/query?leftTicketDTO.train_date=" + strTrain_Date + "&leftTicketDTO.from_station=" + strFrom_Station + "&leftTicketDTO.to_station=" + strTo_Station + "&purpose_codes=" + strPurpost_Codes;
            string strContent = httpST.OpenRead(strRequest);
            return strContent;
        }

        private static string TicketCountContent(string strTrain_Date, string strFrom_Station, string strTo_Station, string strPurpost_Codes)
        {
            WebClient httpST = new WebClient();
            httpST.Encoding = Encoding.UTF8;
            string strRequest = "http://kyfw.12306.cn/otn/lcxxcx/query?purpose_codes=" + strPurpost_Codes + "&queryDate=" + strTrain_Date + "&from_station=" + strFrom_Station + "&to_station=" + strTo_Station;            
            string strContent = httpST.OpenRead(strRequest);
            return strContent;
        }

        #region Login(string strUserName, string strPassword, string strCode)
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="strUserName">UserName</param>
        /// <param name="strPassword">Password</param>
        /// <param name="strCode">Code</param>
        /// <returns>true is success else fail</returns>
        private static bool Login(string strUserName, string strPassword, string strCode)
        {
            HTTPproc.Encoding = Encoding.UTF8;
            string strRequest = "http://kyfw.12306.cn/otn/login/loginAysnSuggest";
            string strParameter = "loginUserDTO.user_name=" + strUserName + "&userDTO.password=" + strPassword + "&randCode=" + strCode;
            //HTTPproc.RequestHeaders.Add("Cookie:" + strCookies);
            Console.WriteLine(strCookies);
            HTTPproc.Cookie = strCookies;
            string strContent = HTTPproc.OpenRead(strRequest, strParameter);

            JObject jo = JObject.Parse(strContent);
            if (jo["data"]["loginCheck"] != null)
            {
                string strResult = jo["data"]["loginCheck"].ToString();
                if (strResult == "Y")
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        private static bool CheckLogin()
        {
            HTTPproc.Encoding = Encoding.UTF8;
            string strRequest = "http://kyfw.12306.cn/otn/login/checkUser";
            string strParameter = "_json_att=";
            Console.WriteLine(HTTPproc.Cookie);
            string strContent = HTTPproc.OpenRead(strRequest, strParameter);
            Console.WriteLine(HTTPproc.Cookie);
            JObject jo = JObject.Parse(strContent);
            if (jo["data"]["flag"] != null)
            {
                string strResult = jo["data"]["flag"].ToString();
                if (strResult == "true")
                {
                    return true;
                }
            }
            return false;
        }

        private static void LoginCode(string strCode)
        {

        }

        private static string DJSON_Ticket(string strContent)
        {
            return "";
        }

        #region URLEncode UrlEncode(string str, string encode)
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
            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-";
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

    #region 验证码
    /// <summary>
    /// 验证码
    /// </summary>
    public class TrainCode
    {
        public string strCookie = "";
        public string strCode = "";
        public string strFormText = "";
        private Thread thread;

        public delegate void CodeEventHander(string strCode,string strCookie);
        public event CodeEventHander OnCodeEvent;

        public TrainCode(string FormText)
        {
            this.strFormText = FormText;
            thread = new Thread(new ThreadStart(Run));
        }

        /// <summary>
        /// 开启线程
        /// </summary>
        public void Start()
        {
            if (thread != null)
            {
                thread.Start();
            }
        }

        /// <summary>
        /// 线程执行内容
        /// </summary>
        private void Run()
        {
            LoginCode_Form lcForm = new LoginCode_Form();
            lcForm.Text = strFormText;
            lcForm.ShowDialog();
            lcForm.Activate();
            if (lcForm.DialogResult == DialogResult.OK)
            {
                //Console.WriteLine(lcForm.strCode + "|" + lcForm.strCookie);
                this.strCookie = lcForm.strCookie;
                this.strCode = lcForm.strCode;
                //触发事件
                Change();
            }            
            //关闭窗口
            lcForm.Close();
            //停止线程
            thread.Abort();
        }

        //
        public void Change()
        {
            //如果有人注册了这个事件，也就是这个事件不是空
            if (OnCodeEvent != null)
            {
                OnCodeEvent(strCode, strCookie);
            }
        }
    }
    #endregion

    #region 车站信息
    /// <summary>
    /// 车站信息
    /// </summary>
    public class Station
    {
        public static string 北京北 = "VAP";
        public static string 北京东 = "BOP";
        public static string 北京 = "BJP";
        public static string 北京南 = "VNP";
        public static string 北京西 = "BXP";
        public static string 广州南 = "IZQ";
        public static string 重庆北 = "CUW";
        public static string 重庆 = "CQW";
        public static string 重庆南 = "CRW";
        public static string 广州东 = "GGQ";
        public static string 上海 = "SHH";
        public static string 上海南 = "SNH";
        public static string 上海虹桥 = "AOH";
        public static string 上海西 = "SXH";
        public static string 天津北 = "TBP";
        public static string 天津 = "TJP";
        public static string 天津南 = "TIP";
        public static string 天津西 = "TXP";
        public static string 长春 = "CCT";
        public static string 长春南 = "CET";
        public static string 长春西 = "CRT";
        public static string 成都东 = "ICW";
        public static string 成都南 = "CNW";
        public static string 成都 = "CDW";
        public static string 长沙 = "CSQ";
        public static string 长沙南 = "CWQ";
        public static string 福州 = "FZS";
        public static string 福州南 = "FYS";
        public static string 贵阳 = "GIW";
        public static string 广州 = "GZQ";
        public static string 哈尔滨 = "HBB";
        public static string 哈尔滨东 = "VBB";
        public static string 哈尔滨西 = "VAB";
        public static string 合肥 = "HFH";
        public static string 呼和浩特东 = "NDC";
        public static string 呼和浩特 = "HHC";
        public static string 海口东 = "HMQ";
        public static string 海口 = "VUQ";
        public static string 杭州东 = "HGH";
        public static string 杭州 = "HZH";
        public static string 杭州南 = "XHH";
        public static string 济南 = "JNK";
        public static string 济南东 = "JAK";
        public static string 济南西 = "JGK";
        public static string 昆明 = "KMM";
        public static string 昆明西 = "KXM";
        public static string 拉萨 = "LSO";
        public static string 兰州东 = "LVJ";
        public static string 兰州 = "LZJ";
        public static string 兰州西 = "LAJ";
        public static string 南昌 = "NCG";
        public static string 南京 = "NJH";
        public static string 南京南 = "NKH";
        public static string 南宁 = "NNZ";
        public static string 石家庄北 = "VVP";
        public static string 石家庄 = "SJP";
        public static string 沈阳 = "SYT";
        public static string 沈阳北 = "SBT";
        public static string 沈阳东 = "SDT";
        public static string 太原北 = "TBV";
        public static string 太原东 = "TDV";
        public static string 太原 = "TYV";
        public static string 武汉 = "WHN";
        public static string 王家营西 = "KNM";
        public static string 乌鲁木齐 = "WMR";
        public static string 西安北 = "EAY";
        public static string 西安 = "XAY";
        public static string 西安南 = "CAY";
        public static string 西宁西 = "XXO";
        public static string 银川 = "YIJ";
        public static string 郑州 = "ZZF";
        public static string 阿尔山 = "ART";
        public static string 安康 = "AKY";
        public static string 阿克苏 = "ASR";
        public static string 阿里河 = "AHX";
        public static string 阿拉山口 = "AKR";
        public static string 安平 = "APT";
        public static string 安庆 = "AQH";
        public static string 安顺 = "ASW";
        public static string 鞍山 = "AST";
        public static string 安阳 = "AYF";
        public static string 北安 = "BAB";
        public static string 蚌埠 = "BBH";
        public static string 白城 = "BCT";
        public static string 北海 = "BHZ";
        public static string 白河 = "BEL";
        public static string 白涧 = "BAP";
        public static string 宝鸡 = "BJY";
        public static string 滨江 = "BJB";
        public static string 博克图 = "BKX";
        public static string 百色 = "BIZ";
        public static string 白山市 = "HJL";
        public static string 北台 = "BTT";
        public static string 包头东 = "BDC";
        public static string 包头 = "BTC";
        public static string 北屯市 = "BXR";
        public static string 本溪 = "BXT";
        public static string 白云鄂博 = "BEC";
        public static string 白银西 = "BXJ";
        public static string 亳州 = "BZH";
        public static string 赤壁 = "CBN";
        public static string 常德 = "VGQ";
        public static string 承德 = "CDP";
        public static string 长甸 = "CDT";
        public static string 赤峰 = "CFD";
        public static string 茶陵 = "CDG";
        public static string 苍南 = "CEH";
        public static string 昌平 = "CPP";
        public static string 崇仁 = "CRG";
        public static string 昌图 = "CTT";
        public static string 长汀镇 = "CDB";
        public static string 曹县 = "CXK";
        public static string 楚雄 = "COM";
        public static string 陈相屯 = "CXT";
        public static string 长治北 = "CBF";
        public static string 长征 = "CZJ";
        public static string 池州 = "IYH";
        public static string 常州 = "CZH";
        public static string 郴州 = "CZQ";
        public static string 长治 = "CZF";
        public static string 沧州 = "COP";
        public static string 崇左 = "CZZ";
        public static string 大安北 = "RNT";
        public static string 大成 = "DCT";
        public static string 丹东 = "DUT";
        public static string 东方红 = "DFB";
        public static string 东莞东 = "DMQ";
        public static string 大虎山 = "DHD";
        public static string 敦煌 = "DHJ";
        public static string 敦化 = "DHL";
        public static string 德惠 = "DHT";
        public static string 东京城 = "DJB";
        public static string 大涧 = "DFP";
        public static string 都江堰 = "DDW";
        public static string 大连北 = "DFT";
        public static string 大理 = "DKM";
        public static string 大连 = "DLT";
        public static string 定南 = "DNG";
        public static string 大庆 = "DZX";
        public static string 东胜 = "DOC";
        public static string 大石桥 = "DQT";
        public static string 大同 = "DTV";
        public static string 东营 = "DPK";
        public static string 大杨树 = "DUX";
        public static string 都匀 = "RYW";
        public static string 邓州 = "DOF";
        public static string 达州 = "RXW";
        public static string 德州 = "DZP";
        public static string 额济纳 = "EJC";
        public static string 二连 = "RLC";
        public static string 恩施 = "ESN";
        public static string 福鼎 = "FES";
        public static string 风陵渡 = "FLV";
        public static string 涪陵 = "FLW";
        public static string 富拉尔基 = "FRX";
        public static string 抚顺北 = "FET";
        public static string 佛山 = "FSQ";
        public static string 阜新 = "FXD";
        public static string 阜阳 = "FYH";
        public static string 格尔木 = "GRO";
        public static string 广汉 = "GHW";
        public static string 古交 = "GJV";
        public static string 桂林北 = "GBZ";
        public static string 古莲 = "GRX";
        public static string 桂林 = "GLZ";
        public static string 固始 = "GXN";
        public static string 广水 = "GSN";
        public static string 干塘 = "GNJ";
        public static string 广元 = "GYW";
        public static string 广州北 = "GBQ";
        public static string 赣州 = "GZG";
        public static string 公主岭 = "GLT";
        public static string 公主岭南 = "GBT";
        public static string 淮安 = "AUH";
        public static string 鹤北 = "HMB";
        public static string 淮北 = "HRH";
        public static string 淮滨 = "HVN";
        public static string 河边 = "HBV";
        public static string 潢川 = "KCN";
        public static string 韩城 = "HCY";
        public static string 邯郸 = "HDP";
        public static string 横道河子 = "HDB";
        public static string 鹤岗 = "HGB";
        public static string 皇姑屯 = "HTT";
        public static string 红果 = "HEM";
        public static string 黑河 = "HJB";
        public static string 怀化 = "HHQ";
        public static string 汉口 = "HKN";
        public static string 葫芦岛 = "HLD";
        public static string 海拉尔 = "HRX";
        public static string 霍林郭勒 = "HWD";
        public static string 海伦 = "HLB";
        public static string 侯马 = "HMV";
        public static string 哈密 = "HMR";
        public static string 淮南 = "HAH";
        public static string 桦南 = "HNB";
        public static string 海宁西 = "EUH";
        public static string 鹤庆 = "HQM";
        public static string 怀柔北 = "HBP";
        public static string 怀柔 = "HRP";
        public static string 黄石东 = "OSN";
        public static string 华山 = "HSY";
        public static string 黄石 = "HSN";
        public static string 黄山 = "HKH";
        public static string 衡水 = "HSP";
        public static string 衡阳 = "HYQ";
        public static string 菏泽 = "HIK";
        public static string 贺州 = "HXZ";
        public static string 汉中 = "HOY";
        public static string 惠州 = "HCQ";
        public static string 吉安 = "VAG";
        public static string 集安 = "JAL";
        public static string 江边村 = "JBG";
        public static string 晋城 = "JCF";
        public static string 金城江 = "JJZ";
        public static string 景德镇 = "JCG";
        public static string 嘉峰 = "JFF";
        public static string 加格达奇 = "JGX";
        public static string 井冈山 = "JGG";
        public static string 蛟河 = "JHL";
        public static string 金华南 = "RNH";
        public static string 金华西 = "JBH";
        public static string 九江 = "JJG";
        public static string 吉林 = "JLL";
        public static string 荆门 = "JMN";
        public static string 佳木斯 = "JMB";
        public static string 济宁 = "JIK";
        public static string 集宁南 = "JAC";
        public static string 酒泉 = "JQJ";
        public static string 江山 = "JUH";
        public static string 吉首 = "JIQ";
        public static string 九台 = "JTL";
        public static string 镜铁山 = "JVJ";
        public static string 鸡西 = "JXB";
        public static string 蓟县 = "JKP";
        public static string 绩溪县 = "JRH";
        public static string 嘉峪关 = "JGJ";
        public static string 江油 = "JFW";
        public static string 锦州 = "JZD";
        public static string 金州 = "JZT";
        public static string 库尔勒 = "KLR";
        public static string 开封 = "KFF";
        public static string 岢岚 = "KLV";
        public static string 凯里 = "KLW";
        public static string 喀什 = "KSR";
        public static string 昆山南 = "KNH";
        public static string 奎屯 = "KTR";
        public static string 开原 = "KYT";
        public static string 六安 = "UAH";
        public static string 灵宝 = "LBF";
        public static string 芦潮港 = "UCH";
        public static string 隆昌 = "LCW";
        public static string 陆川 = "LKZ";
        public static string 利川 = "LCN";
        public static string 临川 = "LCG";
        public static string 潞城 = "UTP";
        public static string 鹿道 = "LDL";
        public static string 娄底 = "LDQ";
        public static string 临汾 = "LFV";
        public static string 良各庄 = "LGP";
        public static string 临河 = "LHC";
        public static string 漯河 = "LON";
        public static string 绿化 = "LWJ";
        public static string 隆化 = "UHP";
        public static string 丽江 = "LHM";
        public static string 临江 = "LQL";
        public static string 龙井 = "LJL";
        public static string 吕梁 = "LHV";
        public static string 醴陵 = "LLG";
        public static string 柳林南 = "LKV";
        public static string 滦平 = "UPP";
        public static string 六盘水 = "UMW";
        public static string 灵丘 = "LVV";
        public static string 旅顺 = "LST";
        public static string 陇西 = "LXJ";
        public static string 澧县 = "LEQ";
        public static string 兰溪 = "LWH";
        public static string 临西 = "UEP";
        public static string 耒阳 = "LYQ";
        public static string 洛阳 = "LYF";
        public static string 龙岩 = "LYS";
        public static string 洛阳东 = "LDF";
        public static string 连云港东 = "UKH";
        public static string 临沂 = "LVK";
        public static string 洛阳龙门 = "LLF";
        public static string 柳园 = "DHR";
        public static string 凌源 = "LYD";
        public static string 辽源 = "LYL";
        public static string 立志 = "LZX";
        public static string 柳州 = "LZZ";
        public static string 辽中 = "LZD";
        public static string 麻城 = "MCN";
        public static string 免渡河 = "MDX";
        public static string 牡丹江 = "MDB";
        public static string 莫尔道嘎 = "MRX";
        public static string 满归 = "MHX";
        public static string 明光 = "MGH";
        public static string 漠河 = "MVX";
        public static string 梅江 = "MKQ";
        public static string 茂名东 = "MDQ";
        public static string 茂名 = "MMZ";
        public static string 密山 = "MSB";
        public static string 马三家 = "MJT";
        public static string 麻尾 = "VAW";
        public static string 绵阳 = "MYW";
        public static string 梅州 = "MOQ";
        public static string 满洲里 = "MLX";
        public static string 宁波东 = "NVH";
        public static string 宁波 = "NGH";
        public static string 南岔 = "NCB";
        public static string 南充 = "NCW";
        public static string 南丹 = "NDZ";
        public static string 南大庙 = "NMP";
        public static string 南芬 = "NFT";
        public static string 讷河 = "NHX";
        public static string 嫩江 = "NGX";
        public static string 内江 = "NJW";
        public static string 南平 = "NPS";
        public static string 南通 = "NUH";
        public static string 南阳 = "NFF";
        public static string 碾子山 = "NZX";
        public static string 平顶山 = "PEN";
        public static string 盘锦 = "PVD";
        public static string 平凉 = "PIJ";
        public static string 平凉南 = "POJ";
        public static string 平泉 = "PQP";
        public static string 坪石 = "PSQ";
        public static string 萍乡 = "PXG";
        public static string 凭祥 = "PXZ";
        public static string 郫县西 = "PCW";
        public static string 攀枝花 = "PRW";
        public static string 蕲春 = "QRN";
        public static string 青城山 = "QSW";
        public static string 青岛 = "QDK";
        public static string 清河城 = "QYP";
        public static string 黔江 = "QNW";
        public static string 曲靖 = "QJM";
        public static string 前进镇 = "QEB";
        public static string 齐齐哈尔 = "QHX";
        public static string 七台河 = "QTB";
        public static string 沁县 = "QVV";
        public static string 泉州东 = "QRS";
        public static string 泉州 = "QYS";
        public static string 衢州 = "QEH";
        public static string 融安 = "RAZ";
        public static string 汝箕沟 = "RQJ";
        public static string 瑞金 = "RJG";
        public static string 日照 = "RZK";
        public static string 双城堡 = "SCB";
        public static string 绥芬河 = "SFB";
        public static string 韶关东 = "SGQ";
        public static string 山海关 = "SHD";
        public static string 绥化 = "SHB";
        public static string 三间房 = "SFX";
        public static string 苏家屯 = "SXT";
        public static string 舒兰 = "SLL";
        public static string 三明 = "SMS";
        public static string 神木 = "OMY";
        public static string 三门峡 = "SMF";
        public static string 商南 = "ONY";
        public static string 遂宁 = "NIW";
        public static string 四平 = "SPT";
        public static string 商丘 = "SQF";
        public static string 上饶 = "SRG";
        public static string 韶山 = "SSQ";
        public static string 宿松 = "OAH";
        public static string 汕头 = "OTQ";
        public static string 邵武 = "SWS";
        public static string 涉县 = "OEP";
        public static string 三亚 = "SEQ";
        public static string 邵阳 = "SYQ";
        public static string 十堰 = "SNN";
        public static string 双鸭山 = "SSB";
        public static string 松原 = "VYT";
        public static string 深圳 = "SZQ";
        public static string 苏州 = "SZH";
        public static string 随州 = "SZN";
        public static string 宿州 = "OXH";
        public static string 朔州 = "SUV";
        public static string 深圳西 = "OSQ";
        public static string 塘豹 = "TBQ";
        public static string 塔尔气 = "TVX";
        public static string 潼关 = "TGY";
        public static string 塘沽 = "TGP";
        public static string 塔河 = "TXX";
        public static string 通化 = "THL";
        public static string 泰来 = "TLX";
        public static string 吐鲁番 = "TFR";
        public static string 通辽 = "TLD";
        public static string 铁岭 = "TLT";
        public static string 陶赖昭 = "TPT";
        public static string 图们 = "TML";
        public static string 铜仁 = "RDQ";
        public static string 唐山北 = "FUP";
        public static string 田师府 = "TFT";
        public static string 泰山 = "TAK";
        public static string 唐山 = "TSP";
        public static string 天水 = "TSJ";
        public static string 通远堡 = "TYT";
        public static string 太阳升 = "TQT";
        public static string 泰州 = "UTH";
        public static string 桐梓 = "TZW";
        public static string 通州西 = "TAP";
        public static string 五常 = "WCB";
        public static string 武昌 = "WCN";
        public static string 瓦房店 = "WDT";
        public static string 威海 = "WKK";
        public static string 芜湖 = "WHH";
        public static string 乌海西 = "WXC";
        public static string 吴家屯 = "WJT";
        public static string 武隆 = "WLW";
        public static string 乌兰浩特 = "WWT";
        public static string 渭南 = "WNY";
        public static string 威舍 = "WSM";
        public static string 歪头山 = "WIT";
        public static string 武威 = "WUJ";
        public static string 武威南 = "WWJ";
        public static string 无锡 = "WXH";
        public static string 乌西 = "WXR";
        public static string 乌伊岭 = "WPB";
        public static string 武夷山 = "WAS";
        public static string 万源 = "WYY";
        public static string 万州 = "WYW";
        public static string 梧州 = "WZZ";
        public static string 温州 = "RZH";
        public static string 温州南 = "VRH";
        public static string 西昌 = "ECW";
        public static string 许昌 = "XCF";
        public static string 西昌南 = "ENW";
        public static string 香坊 = "XFB";
        public static string 轩岗 = "XGV";
        public static string 兴国 = "EUG";
        public static string 宣汉 = "XHY";
        public static string 新会 = "EFQ";
        public static string 新晃 = "XLQ";
        public static string 锡林浩特 = "XTC";
        public static string 兴隆县 = "EXP";
        public static string 厦门北 = "XKS";
        public static string 厦门 = "XMS";
        public static string 厦门高崎 = "XBS";
        public static string 秀山 = "ETW";
        public static string 小市 = "XST";
        public static string 向塘 = "XTG";
        public static string 宣威 = "XWM";
        public static string 新乡 = "XXF";
        public static string 信阳 = "XUN";
        public static string 咸阳 = "XYY";
        public static string 襄阳 = "XFN";
        public static string 熊岳城 = "XYT";
        public static string 兴义 = "XRZ";
        public static string 新沂 = "VIH";
        public static string 新余 = "XUG";
        public static string 徐州 = "XCH";
        public static string 延安 = "YWY";
        public static string 宜宾 = "YBW";
        public static string 亚布力南 = "YWB";
        public static string 叶柏寿 = "YBD";
        public static string 宜昌东 = "HAN";
        public static string 永川 = "YCW";
        public static string 宜春 = "YCG";
        public static string 宜昌 = "YCN";
        public static string 盐城 = "AFH";
        public static string 运城 = "YNV";
        public static string 伊春 = "YCB";
        public static string 榆次 = "YCV";
        public static string 杨村 = "YBP";
        public static string 伊尔施 = "YET";
        public static string 燕岗 = "YGW";
        public static string 永济 = "YIV";
        public static string 延吉 = "YJL";
        public static string 营口 = "YKT";
        public static string 牙克石 = "YKX";
        public static string 阎良 = "YNY";
        public static string 玉林 = "YLZ";
        public static string 榆林 = "ALY";
        public static string 一面坡 = "YPB";
        public static string 伊宁 = "YMR";
        public static string 阳平关 = "YAY";
        public static string 玉屏 = "YZW";
        public static string 原平 = "YPV";
        public static string 延庆 = "YNP";
        public static string 阳泉曲 = "YYV";
        public static string 玉泉 = "YQB";
        public static string 阳泉 = "AQP";
        public static string 玉山 = "YNG";
        public static string 营山 = "NUW";
        public static string 燕山 = "AOP";
        public static string 榆树 = "YRT";
        public static string 鹰潭 = "YTG";
        public static string 烟台 = "YAK";
        public static string 伊图里河 = "YEX";
        public static string 玉田县 = "ATP";
        public static string 义乌 = "YWH";
        public static string 阳新 = "YON";
        public static string 义县 = "YXD";
        public static string 益阳 = "AEQ";
        public static string 岳阳 = "YYQ";
        public static string 永州 = "AOQ";
        public static string 扬州 = "YLH";
        public static string 淄博 = "ZBK";
        public static string 镇城底 = "ZDV";
        public static string 自贡 = "ZGW";
        public static string 珠海 = "ZHQ";
        public static string 珠海北 = "ZIQ";
        public static string 湛江 = "ZJZ";
        public static string 镇江 = "ZJH";
        public static string 张家界 = "DIQ";
        public static string 张家口 = "ZKP";
        public static string 张家口南 = "ZMP";
        public static string 周口 = "ZKN";
        public static string 哲里木 = "ZLC";
        public static string 扎兰屯 = "ZTX";
        public static string 驻马店 = "ZDN";
        public static string 肇庆 = "ZVQ";
        public static string 周水子 = "ZIT";
        public static string 昭通 = "ZDW";
        public static string 中卫 = "ZWJ";
        public static string 资阳 = "ZYW";
        public static string 遵义 = "ZIW";
        public static string 枣庄 = "ZEK";
        public static string 资中 = "ZZW";
        public static string 株洲 = "ZZQ";
        public static string 枣庄西 = "ZFK";
        public static string 昂昂溪 = "AAX";
        public static string 阿城 = "ACB";
        public static string 安达 = "ADX";
        public static string 安定 = "ADP";
        public static string 安广 = "AGT";
        public static string 艾河 = "AHP";
        public static string 安化 = "PKQ";
        public static string 艾家村 = "AJJ";
        public static string 鳌江 = "ARH";
        public static string 安家 = "AJB";
        public static string 阿金 = "AJD";
        public static string 阿克陶 = "AER";
        public static string 安口窑 = "AYY";
        public static string 敖力布告 = "ALD";
        public static string 安龙 = "AUZ";
        public static string 阿龙山 = "ASX";
        public static string 安陆 = "ALN";
        public static string 阿木尔 = "JTX";
        public static string 阿南庄 = "AZM";
        public static string 安庆西 = "APH";
        public static string 鞍山西 = "AXT";
        public static string 安塘 = "ATV";
        public static string 安亭北 = "ASH";
        public static string 阿图什 = "ATR";
        public static string 安图 = "ATL";
        public static string 安溪 = "AXS";
        public static string 博鳌 = "BWQ";
        public static string 白壁关 = "BGV";
        public static string 蚌埠南 = "BMH";
        public static string 巴楚 = "BCR";
        public static string 板城 = "BUP";
        public static string 北戴河 = "BEP";
        public static string 保定 = "BDP";
        public static string 宝坻 = "BPP";
        public static string 八达岭 = "ILP";
        public static string 巴东 = "BNN";
        public static string 柏果 = "BGM";
        public static string 布海 = "BUT";
        public static string 白河东 = "BIY";
        public static string 贲红 = "BVC";
        public static string 宝华山 = "BWH";
        public static string 白河县 = "BEY";
        public static string 白芨沟 = "BJJ";
        public static string 碧鸡关 = "BJM";
        public static string 北滘 = "IBQ";
        public static string 碧江 = "BLQ";
        public static string 白鸡坡 = "BBM";
        public static string 笔架山 = "BSB";
        public static string 八角台 = "BTD";
        public static string 保康 = "BKD";
        public static string 白奎堡 = "BKB";
        public static string 白狼 = "BAT";
        public static string 百浪 = "BRZ";
        public static string 博乐 = "BOR";
        public static string 宝拉格 = "BQC";
        public static string 巴林 = "BLX";
        public static string 宝林 = "BNB";
        public static string 北流 = "BOZ";
        public static string 勃利 = "BLB";
        public static string 布列开 = "BLR";
        public static string 宝龙山 = "BND";
        public static string 八面城 = "BMD";
        public static string 班猫箐 = "BNM";
        public static string 八面通 = "BMB";
        public static string 北马圈子 = "BRP";
        public static string 北票南 = "RPD";
        public static string 白旗 = "BQP";
        public static string 宝泉岭 = "BQB";
        public static string 白泉 = "BQL";
        public static string 白沙 = "BSW";
        public static string 巴山 = "BAY";
        public static string 白水江 = "BSY";
        public static string 白沙坡 = "BPM";
        public static string 白石山 = "BAL";
        public static string 白水镇 = "BUM";
        public static string 坂田 = "BTQ";
        public static string 泊头 = "BZP";
        public static string 北屯 = "BYP";
        public static string 本溪湖 = "BHT";
        public static string 博兴 = "BXK";
        public static string 八仙筒 = "VXD";
        public static string 白音察干 = "BYC";
        public static string 背荫河 = "BYB";
        public static string 北营 = "BIV";
        public static string 巴彦高勒 = "BAC";
        public static string 白音他拉 = "BID";
        public static string 鲅鱼圈 = "BYT";
        public static string 白银市 = "BNJ";
        public static string 白音胡硕 = "BCD";
        public static string 巴中 = "IEW";
        public static string 霸州 = "RMP";
        public static string 北宅 = "BVP";
        public static string 赤壁北 = "CIN";
        public static string 查布嘎 = "CBC";
        public static string 长城 = "CEJ";
        public static string 长冲 = "CCM";
        public static string 承德东 = "CCP";
        public static string 赤峰西 = "CID";
        public static string 嵯岗 = "CAX";
        public static string 柴岗 = "CGT";
        public static string 长葛 = "CEF";
        public static string 柴沟堡 = "CGV";
        public static string 城固 = "CGY";
        public static string 陈官营 = "CAJ";
        public static string 成高子 = "CZB";
        public static string 草海 = "WBW";
        public static string 柴河 = "CHB";
        public static string 册亨 = "CHZ";
        public static string 草河口 = "CKT";
        public static string 崔黄口 = "CHP";
        public static string 巢湖 = "CIH";
        public static string 蔡家沟 = "CJT";
        public static string 成吉思汗 = "CJX";
        public static string 岔江 = "CAM";
        public static string 蔡家坡 = "CJY";
        public static string 昌乐 = "CLK";
        public static string 超梁沟 = "CYP";
        public static string 慈利 = "CUQ";
        public static string 昌黎 = "CLP";
        public static string 长岭子 = "CLT";
        public static string 晨明 = "CMB";
        public static string 长农 = "CNJ";
        public static string 昌平北 = "VBP";
        public static string 常平 = "DAQ";
        public static string 长坡岭 = "CPM";
        public static string 辰清 = "CQB";
        public static string 楚山 = "CSB";
        public static string 长寿 = "EFW";
        public static string 磁山 = "CSP";
        public static string 苍石 = "CST";
        public static string 草市 = "CSL";
        public static string 察素齐 = "CSC";
        public static string 长山屯 = "CVT";
        public static string 长汀 = "CES";
        public static string 昌图西 = "CPT";
        public static string 春湾 = "CQQ";
        public static string 磁县 = "CIP";
        public static string 岑溪 = "CNZ";
        public static string 辰溪 = "CXQ";
        public static string 磁西 = "CRP";
        public static string 长兴南 = "CFH";
        public static string 磁窑 = "CYK";
        public static string 朝阳 = "CYD";
        public static string 春阳 = "CAL";
        public static string 城阳 = "CEK";
        public static string 创业村 = "CEX";
        public static string 朝阳川 = "CYL";
        public static string 朝阳地 = "CDD";
        public static string 长垣 = "CYF";
        public static string 朝阳镇 = "CZL";
        public static string 滁州北 = "CUH";
        public static string 常州北 = "ESH";
        public static string 滁州 = "CXH";
        public static string 潮州 = "CKQ";
        public static string 常庄 = "CVK";
        public static string 曹子里 = "CFP";
        public static string 车转湾 = "CWM";
        public static string 郴州西 = "ICQ";
        public static string 沧州西 = "CBP";
        public static string 德安 = "DAG";
        public static string 大安 = "RAT";
        public static string 大坝 = "DBJ";
        public static string 大板 = "DBC";
        public static string 大巴 = "DBD";
        public static string 到保 = "RBT";
        public static string 定边 = "DYJ";
        public static string 东边井 = "DBB";
        public static string 德伯斯 = "RDT";
        public static string 打柴沟 = "DGJ";
        public static string 德昌 = "DVW";
        public static string 滴道 = "DDB";
        public static string 大德 = "DEM";
        public static string 大磴沟 = "DKJ";
        public static string 刀尔登 = "DRD";
        public static string 得耳布尔 = "DRX";
        public static string 东方 = "UFQ";
        public static string 丹凤 = "DGY";
        public static string 东丰 = "DIL";
        public static string 都格 = "DMM";
        public static string 大官屯 = "DTT";
        public static string 大关 = "RGW";
        public static string 东光 = "DGP";
        public static string 东海 = "DHB";
        public static string 大灰厂 = "DHP";
        public static string 大红旗 = "DQD";
        public static string 东海县 = "DQH";
        public static string 德惠西 = "DXT";
        public static string 达家沟 = "DJT";
        public static string 东津 = "DKB";
        public static string 杜家 = "DJL";
        public static string 大旧庄 = "DJM";
        public static string 大口屯 = "DKP";
        public static string 东来 = "RVD";
        public static string 德令哈 = "DHO";
        public static string 大陆号 = "DLC";
        public static string 带岭 = "DLB";
        public static string 大林 = "DLD";
        public static string 达拉特旗 = "DIC";
        public static string 独立屯 = "DTX";
        public static string 豆罗 = "DLV";
        public static string 达拉特西 = "DNC";
        public static string 东明村 = "DMD";
        public static string 洞庙河 = "DEP";
        public static string 东明县 = "DNF";
        public static string 大拟 = "DNZ";
        public static string 大平房 = "DPD";
        public static string 大盘石 = "RPP";
        public static string 大埔 = "DPI";
        public static string 大堡 = "DVT";
        public static string 大其拉哈 = "DQX";
        public static string 道清 = "DML";
        public static string 对青山 = "DQB";
        public static string 德清西 = "MOH";
        public static string 大庆西 = "RHX";
        public static string 东升 = "DRQ";
        public static string 独山 = "RWW";
        public static string 砀山 = "DKH";
        public static string 登沙河 = "DWT";
        public static string 读书铺 = "DPM";
        public static string 大石头 = "DSL";
        public static string 东胜西 = "DYC";
        public static string 大石寨 = "RZT";
        public static string 东台 = "DBH";
        public static string 定陶 = "DQK";
        public static string 灯塔 = "DGT";
        public static string 大田边 = "DBM";
        public static string 东通化 = "DTL";
        public static string 丹徒 = "RUH";
        public static string 大屯 = "DNT";
        public static string 东湾 = "DRJ";
        public static string 大武口 = "DFJ";
        public static string 低窝铺 = "DWJ";
        public static string 大王滩 = "DZZ";
        public static string 大湾子 = "DFM";
        public static string 大兴沟 = "DXL";
        public static string 大兴 = "DXX";
        public static string 定西 = "DSJ";
        public static string 甸心 = "DXM";
        public static string 东乡 = "DXG";
        public static string 代县 = "DKV";
        public static string 定襄 = "DXV";
        public static string 东戌 = "RXP";
        public static string 东辛庄 = "DXD";
        public static string 丹阳 = "DYH";
        public static string 大雁 = "DYX";
        public static string 德阳 = "DYW";
        public static string 当阳 = "DYN";
        public static string 丹阳北 = "EXH";
        public static string 大英东 = "IAW";
        public static string 东淤地 = "DBV";
        public static string 大营 = "DYV";
        public static string 定远 = "EWH";
        public static string 岱岳 = "RYV";
        public static string 大元 = "DYZ";
        public static string 大营镇 = "DJP";
        public static string 大营子 = "DZD";
        public static string 大战场 = "DTJ";
        public static string 德州东 = "DIP";
        public static string 低庄 = "DVQ";
        public static string 东镇 = "DNV";
        public static string 道州 = "DFZ";
        public static string 东至 = "DCH";
        public static string 东庄 = "DZV";
        public static string 兑镇 = "DWV";
        public static string 豆庄 = "ROP";
        public static string 定州 = "DXP";
        public static string 大竹园 = "DZY";
        public static string 大杖子 = "DAP";
        public static string 豆张庄 = "RZP";
        public static string 峨边 = "EBW";
        public static string 二道沟门 = "RDP";
        public static string 二道湾 = "RDX";
        public static string 二龙 = "RLD";
        public static string 二龙山屯 = "ELA";
        public static string 峨眉 = "EMW";
        public static string 二密河 = "RML";
        public static string 二营 = "RYJ";
        public static string 鄂州 = "ECN";
        public static string 福安 = "FAS";
        public static string 丰城 = "FCG";
        public static string 丰城南 = "FNG";
        public static string 肥东 = "FIH";
        public static string 发耳 = "FEM";
        public static string 富海 = "FHX";
        public static string 福海 = "FHR";
        public static string 凤凰城 = "FHT";
        public static string 奉化 = "FHH";
        public static string 富锦 = "FIB";
        public static string 范家屯 = "FTT";
        public static string 福利屯 = "FTB";
        public static string 丰乐镇 = "FZB";
        public static string 阜南 = "FNH";
        public static string 阜宁 = "AKH";
        public static string 抚宁 = "FNP";
        public static string 福清 = "FQS";
        public static string 福泉 = "VMW";
        public static string 丰水村 = "FSJ";
        public static string 丰顺 = "FUQ";
        public static string 繁峙 = "FSV";
        public static string 抚顺 = "FST";
        public static string 福山口 = "FKP";
        public static string 扶绥 = "FSZ";
        public static string 冯屯 = "FTX";
        public static string 浮图峪 = "FYP";
        public static string 富县东 = "FDY";
        public static string 凤县 = "FXY";
        public static string 富县 = "FEY";
        public static string 费县 = "FXK";
        public static string 凤阳 = "FUH";
        public static string 汾阳 = "FAV";
        public static string 扶余北 = "FBT";
        public static string 分宜 = "FYG";
        public static string 富源 = "FYM";
        public static string 扶余 = "FYT";
        public static string 富裕 = "FYX";
        public static string 抚州北 = "FBG";
        public static string 凤州 = "FZY";
        public static string 丰镇 = "FZC";
        public static string 范镇 = "VZK";
        public static string 固安 = "GFP";
        public static string 广安 = "VJW";
        public static string 高碑店 = "GBP";
        public static string 沟帮子 = "GBD";
        public static string 甘草店 = "GDJ";
        public static string 谷城 = "GCN";
        public static string 藁城 = "GEP";
        public static string 高村 = "GCV";
        public static string 古城镇 = "GZB";
        public static string 广德 = "GRH";
        public static string 贵定 = "GTW";
        public static string 贵定南 = "IDW";
        public static string 古东 = "GDV";
        public static string 贵港 = "GGZ";
        public static string 官高 = "GVP";
        public static string 葛根庙 = "GGT";
        public static string 干沟 = "GGL";
        public static string 甘谷 = "GGJ";
        public static string 高各庄 = "GGP";
        public static string 甘河 = "GAX";
        public static string 根河 = "GEX";
        public static string 郭家店 = "GDT";
        public static string 孤家子 = "GKT";
        public static string 古浪 = "GLJ";
        public static string 皋兰 = "GEJ";
        public static string 高楼房 = "GFM";
        public static string 归流河 = "GHT";
        public static string 关林 = "GLF";
        public static string 甘洛 = "VOW";
        public static string 郭磊庄 = "GLP";
        public static string 高密 = "GMK";
        public static string 公庙子 = "GMC";
        public static string 工农湖 = "GRT";
        public static string 广宁寺 = "GNT";
        public static string 广南卫 = "GNM";
        public static string 高平 = "GPF";
        public static string 甘泉北 = "GEY";
        public static string 共青城 = "GAG";
        public static string 甘旗卡 = "GQD";
        public static string 甘泉 = "GQY";
        public static string 高桥镇 = "GZD";
        public static string 赶水 = "GSW";
        public static string 灌水 = "GST";
        public static string 孤山口 = "GSP";
        public static string 果松 = "GSL";
        public static string 高山子 = "GSD";
        public static string 嘎什甸子 = "GXD";
        public static string 高台 = "GTJ";
        public static string 高滩 = "GAY";
        public static string 古田 = "GTS";
        public static string 官厅 = "GTP";
        public static string 广通 = "GOM";
        public static string 官厅西 = "KEP";
        public static string 贵溪 = "GXG";
        public static string 涡阳 = "GYH";
        public static string 巩义 = "GXF";
        public static string 高邑 = "GIP";
        public static string 巩义南 = "GYF";
        public static string 固原 = "GUJ";
        public static string 菇园 = "GYL";
        public static string 公营子 = "GYD";
        public static string 光泽 = "GZS";
        public static string 古镇 = "GNQ";
        public static string 瓜州 = "GZJ";
        public static string 高州 = "GSQ";
        public static string 固镇 = "GEH";
        public static string 盖州 = "GXT";
        public static string 官字井 = "GOT";
        public static string 革镇堡 = "GZT";
        public static string 冠豸山 = "GSS";
        public static string 盖州西 = "GAT";
        public static string 红安 = "HWN";
        public static string 淮安南 = "AMH";
        public static string 红安西 = "VXN";
        public static string 海安县 = "HIH";
        public static string 黄柏 = "HBL";
        public static string 海北 = "HEB";
        public static string 鹤壁 = "HAF";
        public static string 华城 = "VCQ";
        public static string 合川 = "WKW";
        public static string 河唇 = "HCZ";
        public static string 汉川 = "HCN";
        public static string 海城 = "HCT";
        public static string 黑冲滩 = "HCJ";
        public static string 黄村 = "HCP";
        public static string 海城西 = "HXT";
        public static string 化德 = "HGC";
        public static string 洪洞 = "HDV";
        public static string 霍尔果斯 = "HFR";
        public static string 横峰 = "HFG";
        public static string 韩府湾 = "HXJ";
        public static string 汉沽 = "HGP";
        public static string 黄瓜园 = "HYM";
        public static string 红光镇 = "IGW";
        public static string 浑河 = "HHT";
        public static string 红花沟 = "VHD";
        public static string 黄花筒 = "HUD";
        public static string 贺家店 = "HJJ";
        public static string 和静 = "HJR";
        public static string 红江 = "HFM";
        public static string 黑井 = "HIM";
        public static string 获嘉 = "HJF";
        public static string 河津 = "HJV";
        public static string 涵江 = "HJS";
        public static string 华家 = "HJT";
        public static string 河间西 = "HXP";
        public static string 花家庄 = "HJM";
        public static string 河口南 = "HKJ";
        public static string 黄口 = "KOH";
        public static string 湖口 = "HKG";
        public static string 呼兰 = "HUB";
        public static string 葫芦岛北 = "HPD";
        public static string 浩良河 = "HHB";
        public static string 哈拉海 = "HIT";
        public static string 鹤立 = "HOB";
        public static string 桦林 = "HIB";
        public static string 黄陵 = "ULY";
        public static string 海林 = "HRB";
        public static string 虎林 = "VLB";
        public static string 寒岭 = "HAT";
        public static string 和龙 = "HLL";
        public static string 海龙 = "HIL";
        public static string 哈拉苏 = "HAX";
        public static string 呼鲁斯太 = "VTJ";
        public static string 火连寨 = "HLT";
        public static string 黄梅 = "VEH";
        public static string 蛤蟆塘 = "HMT";
        public static string 韩麻营 = "HYP";
        public static string 黄泥河 = "HHL";
        public static string 海宁 = "HNH";
        public static string 惠农 = "HMJ";
        public static string 和平 = "VAQ";
        public static string 花棚子 = "HZM";
        public static string 花桥 = "VQH";
        public static string 宏庆 = "HEY";
        public static string 怀仁 = "HRV";
        public static string 华容 = "HRN";
        public static string 华山北 = "HDY";
        public static string 黄松甸 = "HDL";
        public static string 和什托洛盖 = "VSR";
        public static string 红山 = "VSB";
        public static string 汉寿 = "VSQ";
        public static string 衡山 = "HSQ";
        public static string 黑水 = "HOT";
        public static string 惠山 = "VCH";
        public static string 虎什哈 = "HHP";
        public static string 红寺堡 = "HSJ";
        public static string 虎石台 = "HUT";
        public static string 海石湾 = "HSO";
        public static string 衡山西 = "HEQ";
        public static string 红砂岘 = "VSJ";
        public static string 黑台 = "HQB";
        public static string 桓台 = "VTK";
        public static string 和田 = "VTR";
        public static string 会同 = "VTQ";
        public static string 海坨子 = "HZT";
        public static string 黑旺 = "HWK";
        public static string 海湾 = "RWH";
        public static string 红星 = "VXB";
        public static string 徽县 = "HYY";
        public static string 红兴隆 = "VHB";
        public static string 换新天 = "VTB";
        public static string 红岘台 = "HTJ";
        public static string 红彦 = "VIX";
        public static string 合阳 = "HAY";
        public static string 海阳 = "HYK";
        public static string 衡阳东 = "HVQ";
        public static string 华蓥 = "HUW";
        public static string 汉阴 = "HQY";
        public static string 黄羊滩 = "HGJ";
        public static string 汉源 = "WHW";
        public static string 湟源 = "HNO";
        public static string 河源 = "VIQ";
        public static string 花园 = "HUN";
        public static string 黄羊镇 = "HYJ";
        public static string 湖州 = "VZH";
        public static string 化州 = "HZZ";
        public static string 黄州 = "VON";
        public static string 霍州 = "HZV";
        public static string 惠州西 = "VXQ";
        public static string 巨宝 = "JRT";
        public static string 靖边 = "JIY";
        public static string 金宝屯 = "JBD";
        public static string 晋城北 = "JEF";
        public static string 金昌 = "JCJ";
        public static string 鄄城 = "JCK";
        public static string 交城 = "JNV";
        public static string 建昌 = "JFD";
        public static string 峻德 = "JDB";
        public static string 井店 = "JFP";
        public static string 鸡东 = "JOB";
        public static string 江都 = "UDH";
        public static string 鸡冠山 = "JST";
        public static string 金沟屯 = "VGP";
        public static string 静海 = "JHP";
        public static string 金河 = "JHX";
        public static string 锦河 = "JHB";
        public static string 锦和 = "JHQ";
        public static string 精河 = "JHR";
        public static string 精河南 = "JIR";
        public static string 江华 = "JHZ";
        public static string 建湖 = "AJH";
        public static string 纪家沟 = "VJD";
        public static string 晋江 = "JJS";
        public static string 江津 = "JJW";
        public static string 姜家 = "JJB";
        public static string 金坑 = "JKT";
        public static string 芨岭 = "JLJ";
        public static string 金马村 = "JMM";
        public static string 角美 = "JES";
        public static string 江门 = "JWQ";
        public static string 莒南 = "JOK";
        public static string 井南 = "JNP";
        public static string 建瓯 = "JVS";
        public static string 经棚 = "JPC";
        public static string 江桥 = "JQX";
        public static string 九三 = "SSX";
        public static string 金山北 = "EGH";
        public static string 京山 = "JCN";
        public static string 建始 = "JRN";
        public static string 嘉善 = "JSH";
        public static string 稷山 = "JVV";
        public static string 吉舒 = "JSL";
        public static string 建设 = "JET";
        public static string 甲山 = "JOP";
        public static string 建三江 = "JIB";
        public static string 嘉善南 = "EAH";
        public static string 金山屯 = "JTB";
        public static string 江所田 = "JOM";
        public static string 景泰 = "JTJ";
        public static string 吉文 = "JWX";
        public static string 进贤 = "JUG";
        public static string 莒县 = "JKK";
        public static string 嘉祥 = "JUK";
        public static string 介休 = "JXV";
        public static string 井陉 = "JJP";
        public static string 嘉兴 = "JXH";
        public static string 嘉兴南 = "EPH";
        public static string 夹心子 = "JXT";
        public static string 简阳 = "JYW";
        public static string 揭阳 = "JRQ";
        public static string 建阳 = "JYS";
        public static string 姜堰 = "UEH";
        public static string 巨野 = "JYK";
        public static string 江永 = "JYZ";
        public static string 靖远 = "JYJ";
        public static string 缙云 = "JYH";
        public static string 江源 = "SZL";
        public static string 济源 = "JYF";
        public static string 靖远西 = "JXJ";
        public static string 胶州北 = "JZK";
        public static string 焦作东 = "WEF";
        public static string 靖州 = "JEQ";
        public static string 荆州 = "JBN";
        public static string 金寨 = "JZH";
        public static string 晋州 = "JXP";
        public static string 胶州 = "JXK";
        public static string 锦州南 = "JOD";
        public static string 焦作 = "JOF";
        public static string 旧庄窝 = "JVP";
        public static string 金杖子 = "JYD";
        public static string 开安 = "KAT";
        public static string 库车 = "KCR";
        public static string 康城 = "KCP";
        public static string 库都尔 = "KDX";
        public static string 宽甸 = "KDT";
        public static string 克东 = "KOB";
        public static string 开江 = "KAW";
        public static string 康金井 = "KJB";
        public static string 喀喇其 = "KQX";
        public static string 开鲁 = "KLC";
        public static string 克拉玛依 = "KHR";
        public static string 口前 = "KQL";
        public static string 奎山 = "KAB";
        public static string 昆山 = "KSH";
        public static string 克山 = "KSB";
        public static string 开通 = "KTT";
        public static string 康熙岭 = "KXZ";
        public static string 克一河 = "KHX";
        public static string 开原西 = "KXT";
        public static string 康庄 = "KZP";
        public static string 来宾 = "UBZ";
        public static string 老边 = "LLT";
        public static string 灵宝西 = "LPF";
        public static string 龙川 = "LUQ";
        public static string 乐昌 = "LCQ";
        public static string 黎城 = "UCP";
        public static string 聊城 = "UCK";
        public static string 蓝村 = "LCK";
        public static string 林东 = "LRC";
        public static string 乐都 = "LDO";
        public static string 梁底下 = "LDP";
        public static string 六道河子 = "LVP";
        public static string 鲁番 = "LVM";
        public static string 廊坊 = "LJP";
        public static string 落垡 = "LOP";
        public static string 廊坊北 = "LFP";
        public static string 禄丰 = "LFM";
        public static string 老府 = "UFD";
        public static string 兰岗 = "LNB";
        public static string 龙骨甸 = "LGM";
        public static string 芦沟 = "LOM";
        public static string 龙沟 = "LGJ";
        public static string 拉古 = "LGB";
        public static string 临海 = "UFH";
        public static string 林海 = "LXX";
        public static string 拉哈 = "LHX";
        public static string 凌海 = "JID";
        public static string 柳河 = "LNL";
        public static string 六合 = "KLH";
        public static string 龙华 = "LHP";
        public static string 滦河沿 = "UNP";
        public static string 六合镇 = "LEX";
        public static string 亮甲店 = "LRT";
        public static string 刘家店 = "UDT";
        public static string 刘家河 = "LVT";
        public static string 连江 = "LKS";
        public static string 李家 = "LJB";
        public static string 罗江 = "LJW";
        public static string 廉江 = "LJZ";
        public static string 庐江 = "UJH";
        public static string 两家 = "UJT";
        public static string 龙江 = "LJX";
        public static string 龙嘉 = "UJL";
        public static string 莲江口 = "LHB";
        public static string 蔺家楼 = "ULK";
        public static string 李家坪 = "LIJ";
        public static string 兰考 = "LKF";
        public static string 林口 = "LKB";
        public static string 路口铺 = "LKQ";
        public static string 老莱 = "LAX";
        public static string 拉林 = "LAB";
        public static string 陆良 = "LRM";
        public static string 龙里 = "LLW";
        public static string 零陵 = "UWZ";
        public static string 临澧 = "LWQ";
        public static string 兰棱 = "LLB";
        public static string 卢龙 = "UAP";
        public static string 喇嘛甸 = "LMX";
        public static string 里木店 = "LMB";
        public static string 洛门 = "LMJ";
        public static string 龙南 = "UNG";
        public static string 梁平 = "UQW";
        public static string 罗平 = "LPM";
        public static string 落坡岭 = "LPP";
        public static string 六盘山 = "UPJ";
        public static string 乐平市 = "LPG";
        public static string 临清 = "UQK";
        public static string 龙泉寺 = "UQJ";
        public static string 乐善村 = "LUM";
        public static string 冷水江东 = "UDQ";
        public static string 连山关 = "LGT";
        public static string 流水沟 = "USP";
        public static string 陵水 = "LIQ";
        public static string 乐山 = "UTW";
        public static string 罗山 = "LRN";
        public static string 鲁山 = "LAF";
        public static string 丽水 = "USH";
        public static string 梁山 = "LMK";
        public static string 灵石 = "LSV";
        public static string 露水河 = "LUL";
        public static string 庐山 = "LSG";
        public static string 林盛堡 = "LBT";
        public static string 柳树屯 = "LSD";
        public static string 梨树镇 = "LSB";
        public static string 龙山镇 = "LAS";
        public static string 李石寨 = "LET";
        public static string 黎塘 = "LTZ";
        public static string 轮台 = "LAR";
        public static string 芦台 = "LTP";
        public static string 龙塘坝 = "LBM";
        public static string 濑湍 = "LVZ";
        public static string 骆驼巷 = "LTJ";
        public static string 李旺 = "VLJ";
        public static string 莱芜东 = "LWK";
        public static string 狼尾山 = "LRJ";
        public static string 灵武 = "LNJ";
        public static string 莱芜西 = "UXK";
        public static string 朗乡 = "LXB";
        public static string 陇县 = "LXY";
        public static string 临湘 = "LXQ";
        public static string 芦溪 = "LUG";
        public static string 林西 = "LXC";
        public static string 滦县 = "UXP";
        public static string 略阳 = "LYY";
        public static string 莱阳 = "LYK";
        public static string 辽阳 = "LYT";
        public static string 临沂北 = "UYK";
        public static string 凌源东 = "LDD";
        public static string 连云港 = "UIH";
        public static string 临颍 = "LNF";
        public static string 老营 = "LXL";
        public static string 龙游 = "LMH";
        public static string 罗源 = "LVS";
        public static string 林源 = "LYX";
        public static string 涟源 = "LAQ";
        public static string 涞源 = "LYP";
        public static string 耒阳西 = "LPQ";
        public static string 临泽 = "LEJ";
        public static string 龙爪沟 = "LZT";
        public static string 雷州 = "UAQ";
        public static string 六枝 = "LIW";
        public static string 鹿寨 = "LIZ";
        public static string 来舟 = "LZS";
        public static string 龙镇 = "LZA";
        public static string 拉鲊 = "LEM";
        public static string 马鞍山 = "MAH";
        public static string 毛坝 = "MBY";
        public static string 毛坝关 = "MGY";
        public static string 麻城北 = "MBN";
        public static string 渑池 = "MCF";
        public static string 明城 = "MCL";
        public static string 庙城 = "MAP";
        public static string 渑池南 = "MNF";
        public static string 茅草坪 = "KPM";
        public static string 猛洞河 = "MUQ";
        public static string 磨刀石 = "MOB";
        public static string 弥渡 = "MDF";
        public static string 帽儿山 = "MRB";
        public static string 明港 = "MGN";
        public static string 梅河口 = "MHL";
        public static string 马皇 = "MHZ";
        public static string 孟家岗 = "MGB";
        public static string 美兰 = "MHQ";
        public static string 汨罗东 = "MQQ";
        public static string 马莲河 = "MHB";
        public static string 茅岭 = "MLZ";
        public static string 庙岭 = "MLL";
        public static string 茂林 = "MLD";
        public static string 穆棱 = "MLB";
        public static string 马林 = "MID";
        public static string 马龙 = "MGM";
        public static string 汨罗 = "MLQ";
        public static string 木里图 = "MUD";
        public static string 玛纳斯湖 = "MNR";
        public static string 冕宁 = "UGW";
        public static string 沐滂 = "MPQ";
        public static string 马桥河 = "MQB";
        public static string 闽清 = "MQS";
        public static string 民权 = "MQF";
        public static string 明水河 = "MUT";
        public static string 麻山 = "MAB";
        public static string 眉山 = "MSW";
        public static string 漫水湾 = "MKW";
        public static string 茂舍祖 = "MOM";
        public static string 米沙子 = "MST";
        public static string 庙台子 = "MZB";
        public static string 美溪 = "MEB";
        public static string 勉县 = "MVY";
        public static string 麻阳 = "MVQ";
        public static string 牧羊村 = "MCM";
        public static string 米易 = "MMW";
        public static string 麦园 = "MYS";
        public static string 墨玉 = "MUR";
        public static string 密云 = "MUP";
        public static string 庙庄 = "MZJ";
        public static string 米脂 = "MEY";
        public static string 明珠 = "MFQ";
        public static string 宁安 = "NAB";
        public static string 农安 = "NAT";
        public static string 南博山 = "NBK";
        public static string 南仇 = "NCK";
        public static string 南城司 = "NSP";
        public static string 宁村 = "NCZ";
        public static string 宁德 = "NES";
        public static string 南观村 = "NGP";
        public static string 南宫东 = "NFP";
        public static string 南关岭 = "NLT";
        public static string 宁国 = "NNH";
        public static string 宁海 = "NHH";
        public static string 南河川 = "NHJ";
        public static string 南华 = "NHS";
        public static string 泥河子 = "NHD";
        public static string 宁家 = "NVT";
        public static string 牛家 = "NJB";
        public static string 南靖 = "NJS";
        public static string 能家 = "NJD";
        public static string 南口 = "NKP";
        public static string 南口前 = "NKT";
        public static string 南朗 = "NNQ";
        public static string 乃林 = "NLD";
        public static string 尼勒克 = "NIR";
        public static string 那罗 = "ULZ";
        public static string 宁陵县 = "NLF";
        public static string 奈曼 = "NMD";
        public static string 宁明 = "NMZ";
        public static string 南木 = "NMX";
        public static string 南平南 = "NNS";
        public static string 那铺 = "NPZ";
        public static string 南桥 = "NQD";
        public static string 那曲 = "NQO";
        public static string 暖泉 = "NQJ";
        public static string 南台 = "NTT";
        public static string 南头 = "NOQ";
        public static string 宁武 = "NWV";
        public static string 南湾子 = "NWP";
        public static string 南翔北 = "NEH";
        public static string 宁乡 = "NXQ";
        public static string 内乡 = "NXF";
        public static string 牛心台 = "NXT";
        public static string 南峪 = "NUP";
        public static string 娘子关 = "NIP";
        public static string 南召 = "NAF";
        public static string 南杂木 = "NZT";
        public static string 平安 = "PAL";
        public static string 蓬安 = "PAW";
        public static string 平安驿 = "PNO";
        public static string 磐安镇 = "PAJ";
        public static string 平安镇 = "PZT";
        public static string 蒲城东 = "PEY";
        public static string 蒲城 = "PCY";
        public static string 裴德 = "PDB";
        public static string 偏店 = "PRP";
        public static string 平顶山西 = "BFF";
        public static string 坡底下 = "PXJ";
        public static string 瓢儿屯 = "PRT";
        public static string 平房 = "PFB";
        public static string 平岗 = "PGL";
        public static string 平关 = "PGM";
        public static string 盘关 = "PAM";
        public static string 平果 = "PGZ";
        public static string 徘徊北 = "PHP";
        public static string 平河口 = "PHM";
        public static string 盘锦北 = "PBD";
        public static string 潘家店 = "PDP";
        public static string 皮口 = "PKT";
        public static string 普兰店 = "PLT";
        public static string 偏岭 = "PNT";
        public static string 平罗 = "SZJ";
        public static string 平山 = "PSB";
        public static string 彭山 = "PSW";
        public static string 皮山 = "PSR";
        public static string 彭水 = "PHW";
        public static string 磐石 = "PSL";
        public static string 平社 = "PSV";
        public static string 平台 = "PVT";
        public static string 平田 = "PTM";
        public static string 莆田 = "PTS";
        public static string 葡萄菁 = "PTW";
        public static string 普湾 = "PWT";
        public static string 平旺 = "PWV";
        public static string 平型关 = "PGV";
        public static string 普雄 = "POW";
        public static string 平洋 = "PYX";
        public static string 彭阳 = "PYJ";
        public static string 平遥 = "PYV";
        public static string 平邑 = "PIK";
        public static string 平原堡 = "PPJ";
        public static string 平原 = "PYK";
        public static string 平峪 = "PYP";
        public static string 彭泽 = "PZG";
        public static string 邳州 = "PJH";
        public static string 平庄 = "PZD";
        public static string 泡子 = "POD";
        public static string 平庄南 = "PND";
        public static string 乾安 = "QOT";
        public static string 庆安 = "QAB";
        public static string 迁安 = "QQP";
        public static string 祁东北 = "QRQ";
        public static string 七甸 = "QDM";
        public static string 曲阜东 = "QAK";
        public static string 庆丰 = "QFT";
        public static string 奇峰塔 = "QVP";
        public static string 曲阜 = "QFK";
        public static string 勤丰营 = "QFM";
        public static string 琼海 = "QYQ";
        public static string 秦皇岛 = "QTP";
        public static string 千河 = "QUY";
        public static string 清河 = "QIP";
        public static string 清河门 = "QHD";
        public static string 清华园 = "QHP";
        public static string 渠旧 = "QJZ";
        public static string 綦江 = "QJW";
        public static string 潜江 = "QJN";
        public static string 全椒 = "INH";
        public static string 秦家 = "QJB";
        public static string 祁家堡 = "QBT";
        public static string 清涧县 = "QNY";
        public static string 秦家庄 = "QZV";
        public static string 七里河 = "QLD";
        public static string 渠黎 = "QLZ";
        public static string 秦岭 = "QLY";
        public static string 青龙山 = "QGH";
        public static string 青龙寺 = "QSM";
        public static string 祁门 = "QIH";
        public static string 前磨头 = "QMP";
        public static string 青山 = "QSB";
        public static string 确山 = "QSN";
        public static string 清水 = "QUJ";
        public static string 前山 = "QXQ";
        public static string 戚墅堰 = "QYH";
        public static string 青田 = "QVH";
        public static string 桥头 = "QAT";
        public static string 青铜峡 = "QTJ";
        public static string 前卫 = "QWD";
        public static string 前苇塘 = "QWP";
        public static string 渠县 = "QRW";
        public static string 祁县 = "QXV";
        public static string 青县 = "QXP";
        public static string 桥西 = "QXJ";
        public static string 清徐 = "QUV";
        public static string 旗下营 = "QXC";
        public static string 千阳 = "QOY";
        public static string 沁阳 = "QYF";
        public static string 泉阳 = "QYL";
        public static string 祁阳北 = "QVQ";
        public static string 七营 = "QYJ";
        public static string 庆阳山 = "QSJ";
        public static string 清远 = "QBQ";
        public static string 清原 = "QYT";
        public static string 钦州东 = "QDZ";
        public static string 钦州 = "QRZ";
        public static string 青州市 = "QZK";
        public static string 瑞安 = "RAH";
        public static string 荣昌 = "RCW";
        public static string 瑞昌 = "RCG";
        public static string 如皋 = "RBH";
        public static string 容桂 = "RUQ";
        public static string 任丘 = "RQP";
        public static string 乳山 = "ROK";
        public static string 融水 = "RSZ";
        public static string 热水 = "RSD";
        public static string 容县 = "RXZ";
        public static string 饶阳 = "RVP";
        public static string 汝阳 = "RYF";
        public static string 绕阳河 = "RHD";
        public static string 汝州 = "ROF";
        public static string 石坝 = "OBJ";
        public static string 上板城 = "SBP";
        public static string 施秉 = "AQW";
        public static string 上板城南 = "OBP";
        public static string 世博园 = "ZWT";
        public static string 双城北 = "SBB";
        public static string 商城 = "SWN";
        public static string 莎车 = "SCR";
        public static string 顺昌 = "SCS";
        public static string 舒城 = "OCH";
        public static string 神池 = "SMV";
        public static string 沙城 = "SCP";
        public static string 石城 = "SCT";
        public static string 山城镇 = "SCL";
        public static string 山丹 = "SDJ";
        public static string 顺德 = "ORQ";
        public static string 绥德 = "ODY";
        public static string 邵东 = "SOQ";
        public static string 水洞 = "SIL";
        public static string 商都 = "SXC";
        public static string 十渡 = "SEP";
        public static string 四道湾 = "OUD";
        public static string 顺德学院 = "OJQ";
        public static string 绅坊 = "OLH";
        public static string 双丰 = "OFB";
        public static string 四方台 = "STB";
        public static string 水富 = "OTW";
        public static string 三关口 = "OKJ";
        public static string 桑根达来 = "OGC";
        public static string 韶关 = "SNQ";
        public static string 上高镇 = "SVK";
        public static string 上杭 = "JBS";
        public static string 沙海 = "SED";
        public static string 松河 = "SBM";
        public static string 沙河 = "SHP";
        public static string 沙河口 = "SKT";
        public static string 赛汗塔拉 = "SHC";
        public static string 沙河市 = "VOP";
        public static string 沙后所 = "SSD";
        public static string 山河屯 = "SHL";
        public static string 三河县 = "OXP";
        public static string 四合永 = "OHD";
        public static string 三汇镇 = "OZW";
        public static string 双河镇 = "SEL";
        public static string 石河子 = "SZR";
        public static string 三合庄 = "SVP";
        public static string 三家店 = "ODP";
        public static string 水家湖 = "SQH";
        public static string 沈家河 = "OJJ";
        public static string 松江河 = "SJL";
        public static string 尚家 = "SJB";
        public static string 孙家 = "SUB";
        public static string 沈家 = "OJB";
        public static string 松江 = "SAH";
        public static string 三江口 = "SKD";
        public static string 司家岭 = "OLK";
        public static string 松江南 = "IMH";
        public static string 石景山南 = "SRP";
        public static string 邵家堂 = "SJJ";
        public static string 三江县 = "SOZ";
        public static string 三家寨 = "SMM";
        public static string 十家子 = "SJD";
        public static string 松江镇 = "OZL";
        public static string 施家嘴 = "SHM";
        public static string 深井子 = "SWT";
        public static string 什里店 = "OMP";
        public static string 疏勒 = "SUR";
        public static string 疏勒河 = "SHJ";
        public static string 舍力虎 = "VLD";
        public static string 石磷 = "SPB";
        public static string 绥棱 = "SIB";
        public static string 石岭 = "SOL";
        public static string 石林 = "SLM";
        public static string 石林南 = "LNM";
        public static string 石龙 = "SLQ";
        public static string 萨拉齐 = "SLC";
        public static string 索伦 = "SNT";
        public static string 商洛 = "OLY";
        public static string 沙岭子 = "SLP";
        public static string 石门县北 = "VFQ";
        public static string 三门峡南 = "SCF";
        public static string 三门县 = "OQH";
        public static string 石门县 = "OMQ";
        public static string 三门峡西 = "SXF";
        public static string 肃宁 = "SYP";
        public static string 宋 = "SOB";
        public static string 双牌 = "SBZ";
        public static string 四平东 = "PPT";
        public static string 遂平 = "SON";
        public static string 沙坡头 = "SFJ";
        public static string 商丘南 = "SPF";
        public static string 水泉 = "SID";
        public static string 石泉县 = "SXY";
        public static string 石桥子 = "SQT";
        public static string 石人城 = "SRB";
        public static string 石人 = "SRL";
        public static string 山市 = "SQB";
        public static string 神树 = "SWB";
        public static string 鄯善 = "SSR";
        public static string 三水 = "SJQ";
        public static string 泗水 = "OSK";
        public static string 石山 = "SAD";
        public static string 松树 = "SFT";
        public static string 首山 = "SAT";
        public static string 三十家 = "SRD";
        public static string 三十里堡 = "SST";
        public static string 松树镇 = "SSL";
        public static string 松桃 = "MZQ";
        public static string 索图罕 = "SHX";
        public static string 三堂集 = "SDH";
        public static string 石头 = "OTB";
        public static string 神头 = "SEV";
        public static string 沙沱 = "SFM";
        public static string 上万 = "SWP";
        public static string 孙吴 = "SKB";
        public static string 沙湾县 = "SXR";
        public static string 遂溪 = "SXZ";
        public static string 沙县 = "SAS";
        public static string 绍兴 = "SOH";
        public static string 歙县 = "OVH";
        public static string 石岘 = "SXL";
        public static string 上西铺 = "SXM";
        public static string 石峡子 = "SXJ";
        public static string 绥阳 = "SYB";
        public static string 沭阳 = "FMH";
        public static string 寿阳 = "SYV";
        public static string 水洋 = "OYP";
        public static string 三阳川 = "SYJ";
        public static string 上腰墩 = "SPJ";
        public static string 三营 = "OEJ";
        public static string 顺义 = "SOP";
        public static string 三义井 = "OYD";
        public static string 三源浦 = "SYL";
        public static string 三原 = "SAY";
        public static string 上虞 = "BDH";
        public static string 上园 = "SUD";
        public static string 水源 = "OYJ";
        public static string 桑园子 = "SAJ";
        public static string 绥中北 = "SND";
        public static string 苏州北 = "OHH";
        public static string 宿州东 = "SRH";
        public static string 深圳东 = "BJQ";
        public static string 深州 = "OZP";
        public static string 孙镇 = "OZY";
        public static string 绥中 = "SZD";
        public static string 尚志 = "SZB";
        public static string 师庄 = "SNM";
        public static string 松滋 = "SIN";
        public static string 师宗 = "SEM";
        public static string 苏州园区 = "KAH";
        public static string 苏州新区 = "ITH";
        public static string 泰安 = "TMK";
        public static string 台安 = "TID";
        public static string 通安驿 = "TAJ";
        public static string 桐柏 = "TBF";
        public static string 通北 = "TBB";
        public static string 汤池 = "TCX";
        public static string 桐城 = "TTH";
        public static string 郯城 = "TZK";
        public static string 铁厂 = "TCL";
        public static string 桃村 = "TCK";
        public static string 通道 = "TRQ";
        public static string 田东 = "TDZ";
        public static string 天岗 = "TGL";
        public static string 土贵乌拉 = "TGC";
        public static string 通沟 = "TOL";
        public static string 太谷 = "TGV";
        public static string 塔哈 = "THX";
        public static string 棠海 = "THM";
        public static string 唐河 = "THF";
        public static string 泰和 = "THG";
        public static string 太湖 = "TKH";
        public static string 团结 = "TIX";
        public static string 谭家井 = "TNJ";
        public static string 陶家屯 = "TOT";
        public static string 唐家湾 = "PDQ";
        public static string 统军庄 = "TZP";
        public static string 泰康 = "TKX";
        public static string 吐列毛杜 = "TMD";
        public static string 图里河 = "TEX";
        public static string 亭亮 = "TIZ";
        public static string 田林 = "TFZ";
        public static string 铜陵 = "TJH";
        public static string 铁力 = "TLB";
        public static string 铁岭西 = "PXT";
        public static string 天门 = "TMN";
        public static string 天门南 = "TNN";
        public static string 太姥山 = "TLS";
        public static string 土牧尔台 = "TRC";
        public static string 土门子 = "TCJ";
        public static string 潼南 = "TVW";
        public static string 洮南 = "TVT";
        public static string 太平川 = "TIT";
        public static string 太平镇 = "TEB";
        public static string 图强 = "TQX";
        public static string 台前 = "TTK";
        public static string 天桥岭 = "TQL";
        public static string 土桥子 = "TQJ";
        public static string 汤山城 = "TCT";
        public static string 桃山 = "TAB";
        public static string 塔石嘴 = "TIM";
        public static string 通途 = "TUT";
        public static string 汤旺河 = "THB";
        public static string 同心 = "TXJ";
        public static string 土溪 = "TSW";
        public static string 桐乡 = "TCH";
        public static string 田阳 = "TRZ";
        public static string 桃映 = "TKQ";
        public static string 天义 = "TND";
        public static string 汤阴 = "TYF";
        public static string 驼腰岭 = "TIL";
        public static string 太阳山 = "TYJ";
        public static string 汤原 = "TYB";
        public static string 塔崖驿 = "TYP";
        public static string 滕州东 = "TEK";
        public static string 台州 = "TZH";
        public static string 天祝 = "TZJ";
        public static string 滕州 = "TXK";
        public static string 天镇 = "TZV";
        public static string 桐子林 = "TEW";
        public static string 天柱山 = "QWH";
        public static string 文安 = "WBP";
        public static string 武安 = "WAP";
        public static string 王安镇 = "WVP";
        public static string 旺苍 = "WEW";
        public static string 五叉沟 = "WCT";
        public static string 文昌 = "WEQ";
        public static string 温春 = "WDB";
        public static string 五大连池 = "WRB";
        public static string 文登 = "WBK";
        public static string 五道沟 = "WDL";
        public static string 五道河 = "WHP";
        public static string 文地 = "WNZ";
        public static string 卫东 = "WVT";
        public static string 武当山 = "WRN";
        public static string 望都 = "WDP";
        public static string 乌尔旗汗 = "WHX";
        public static string 潍坊 = "WFK";
        public static string 万发屯 = "WFB";
        public static string 王府 = "WUT";
        public static string 瓦房店西 = "WXT";
        public static string 王岗 = "WGB";
        public static string 武功 = "WGY";
        public static string 湾沟 = "WGL";
        public static string 吴官田 = "WGM";
        public static string 乌海 = "WVC";
        public static string 苇河 = "WHB";
        public static string 卫辉 = "WHF";
        public static string 吴家川 = "WCJ";
        public static string 五家 = "WUB";
        public static string 威箐 = "WAM";
        public static string 午汲 = "WJP";
        public static string 渭津 = "WJL";
        public static string 王家湾 = "WJJ";
        public static string 倭肯 = "WQB";
        public static string 五棵树 = "WKT";
        public static string 五龙背 = "WBT";
        public static string 乌兰哈达 = "WLC";
        public static string 万乐 = "WEB";
        public static string 瓦拉干 = "WVX";
        public static string 温岭 = "VHH";
        public static string 五莲 = "WLK";
        public static string 乌拉特前旗 = "WQC";
        public static string 乌拉山 = "WSC";
        public static string 卧里屯 = "WLX";
        public static string 渭南北 = "WBY";
        public static string 乌奴耳 = "WRX";
        public static string 万宁 = "WNQ";
        public static string 万年 = "WWG";
        public static string 渭南南 = "WVY";
        public static string 渭南镇 = "WNJ";
        public static string 沃皮 = "WPT";
        public static string 吴堡 = "WUY";
        public static string 吴桥 = "WUP";
        public static string 汪清 = "WQL";
        public static string 武清 = "WWP";
        public static string 武山 = "WSJ";
        public static string 文水 = "WEV";
        public static string 魏善庄 = "WSP";
        public static string 王瞳 = "WTP";
        public static string 五台山 = "WSV";
        public static string 王团庄 = "WZJ";
        public static string 五五 = "WVR";
        public static string 无锡东 = "WGH";
        public static string 卫星 = "WVB";
        public static string 闻喜 = "WXV";
        public static string 武乡 = "WVV";
        public static string 无锡新区 = "IFH";
        public static string 武穴 = "WXN";
        public static string 吴圩 = "WYZ";
        public static string 王杨 = "WYB";
        public static string 五营 = "WWB";
        public static string 武义 = "RYH";
        public static string 瓦窑田 = "WIM";
        public static string 五原 = "WYC";
        public static string 苇子沟 = "WZL";
        public static string 韦庄 = "WZY";
        public static string 五寨 = "WZV";
        public static string 王兆屯 = "WZB";
        public static string 微子镇 = "WQP";
        public static string 魏杖子 = "WKD";
        public static string 新安 = "EAM";
        public static string 兴安 = "XAZ";
        public static string 新安县 = "XAF";
        public static string 新保安 = "XAP";
        public static string 下板城 = "EBP";
        public static string 西八里 = "XLP";
        public static string 宣城 = "ECH";
        public static string 兴城 = "XCD";
        public static string 小村 = "XEM";
        public static string 新绰源 = "XRX";
        public static string 下城子 = "XCB";
        public static string 新城子 = "XCT";
        public static string 喜德 = "EDW";
        public static string 小得江 = "EJM";
        public static string 西大庙 = "XMP";
        public static string 小董 = "XEZ";
        public static string 小东 = "XOD";
        public static string 息烽 = "XFW";
        public static string 信丰 = "EFG";
        public static string 襄汾 = "XFV";
        public static string 新干 = "EGG";
        public static string 孝感 = "XGN";
        public static string 西固城 = "XUJ";
        public static string 夏官营 = "XGJ";
        public static string 西岗子 = "NBB";
        public static string 襄河 = "XXB";
        public static string 新和 = "XIR";
        public static string 宣和 = "XWJ";
        public static string 斜河涧 = "EEP";
        public static string 新华屯 = "XAX";
        public static string 新华 = "XHB";
        public static string 新化 = "EHQ";
        public static string 宣化 = "XHP";
        public static string 兴和西 = "XEC";
        public static string 小河沿 = "XYD";
        public static string 下花园 = "XYP";
        public static string 小河镇 = "EKY";
        public static string 徐家 = "XJB";
        public static string 峡江 = "EJG";
        public static string 新绛 = "XJV";
        public static string 辛集 = "ENP";
        public static string 新江 = "XJM";
        public static string 西街口 = "EKM";
        public static string 许家屯 = "XJT";
        public static string 许家台 = "XTJ";
        public static string 谢家镇 = "XMT";
        public static string 兴凯 = "EKB";
        public static string 小榄 = "EAQ";
        public static string 香兰 = "XNB";
        public static string 兴隆店 = "XDD";
        public static string 新乐 = "ELP";
        public static string 新林 = "XPX";
        public static string 小岭 = "XLB";
        public static string 新李 = "XLJ";
        public static string 西林 = "XYB";
        public static string 西柳 = "GCT";
        public static string 仙林 = "XPH";
        public static string 新立屯 = "XLD";
        public static string 小路溪 = "XLM";
        public static string 兴隆镇 = "XZB";
        public static string 新立镇 = "XGT";
        public static string 新民 = "XMD";
        public static string 西麻山 = "XMB";
        public static string 下马塘 = "XAT";
        public static string 孝南 = "XNV";
        public static string 咸宁北 = "XRN";
        public static string 兴宁 = "ENQ";
        public static string 咸宁 = "XNN";
        public static string 西平 = "XPN";
        public static string 兴平 = "XPY";
        public static string 新坪田 = "XPM";
        public static string 霞浦 = "XOS";
        public static string 溆浦 = "EPQ";
        public static string 犀浦 = "XIW";
        public static string 新青 = "XQB";
        public static string 新邱 = "XQD";
        public static string 兴泉堡 = "XQJ";
        public static string 仙人桥 = "XRL";
        public static string 小寺沟 = "ESP";
        public static string 杏树 = "XSB";
        public static string 夏石 = "XIZ";
        public static string 浠水 = "XZN";
        public static string 下社 = "XSV";
        public static string 徐水 = "XSP";
        public static string 小哨 = "XAM";
        public static string 新松浦 = "XOB";
        public static string 杏树屯 = "XDT";
        public static string 许三湾 = "XSJ";
        public static string 湘潭 = "XTQ";
        public static string 邢台 = "XTP";
        public static string 仙桃西 = "XAN";
        public static string 下台子 = "EIP";
        public static string 徐闻 = "XJQ";
        public static string 新窝铺 = "EPD";
        public static string 修武 = "XWF";
        public static string 新县 = "XSN";
        public static string 息县 = "ENN";
        public static string 西乡 = "XQY";
        public static string 湘乡 = "XXQ";
        public static string 西峡 = "XIF";
        public static string 孝西 = "XOV";
        public static string 小新街 = "XXM";
        public static string 新兴县 = "XGQ";
        public static string 西小召 = "XZC";
        public static string 小西庄 = "XXP";
        public static string 向阳 = "XDB";
        public static string 旬阳 = "XUY";
        public static string 旬阳北 = "XBY";
        public static string 襄阳东 = "XWN";
        public static string 兴业 = "SNZ";
        public static string 小雨谷 = "XHM";
        public static string 信宜 = "EEQ";
        public static string 小月旧 = "XFM";
        public static string 小扬气 = "XYX";
        public static string 祥云 = "EXM";
        public static string 襄垣 = "EIF";
        public static string 夏邑县 = "EJH";
        public static string 新友谊 = "EYB";
        public static string 新阳镇 = "XZJ";
        public static string 徐州东 = "UUH";
        public static string 新帐房 = "XZX";
        public static string 悬钟 = "XRP";
        public static string 新肇 = "XZT";
        public static string 忻州 = "XXV";
        public static string 汐子 = "XZD";
        public static string 西哲里木 = "XRD";
        public static string 新杖子 = "ERP";
        public static string 姚安 = "YAC";
        public static string 依安 = "YAX";
        public static string 永安 = "YAS";
        public static string 永安乡 = "YNB";
        public static string 亚布力 = "YBB";
        public static string 元宝山 = "YUD";
        public static string 羊草 = "YAB";
        public static string 秧草地 = "YKM";
        public static string 阳澄湖 = "AIH";
        public static string 迎春 = "YYB";
        public static string 叶城 = "YER";
        public static string 盐池 = "YKJ";
        public static string 砚川 = "YYY";
        public static string 阳春 = "YQQ";
        public static string 宜城 = "YIN";
        public static string 应城 = "YHN";
        public static string 禹城 = "YCK";
        public static string 晏城 = "YEK";
        public static string 羊场 = "YED";
        public static string 阳城 = "YNF";
        public static string 阳岔 = "YAL";
        public static string 郓城 = "YPK";
        public static string 雁翅 = "YAP";
        public static string 云彩岭 = "ACP";
        public static string 虞城县 = "IXH";
        public static string 营城子 = "YCT";
        public static string 永登 = "YDJ";
        public static string 英德 = "YDQ";
        public static string 尹地 = "YDM";
        public static string 永定 = "YGS";
        public static string 雁荡山 = "YGH";
        public static string 于都 = "YDG";
        public static string 园墩 = "YAJ";
        public static string 英德西 = "IIQ";
        public static string 永丰营 = "YYM";
        public static string 杨岗 = "YRB";
        public static string 阳高 = "YOV";
        public static string 阳谷 = "YIK";
        public static string 友好 = "YOB";
        public static string 余杭 = "EVH";
        public static string 沿河城 = "YHP";
        public static string 岩会 = "AEP";
        public static string 羊臼河 = "YHM";
        public static string 永嘉 = "URH";
        public static string 营街 = "YAM";
        public static string 盐津 = "AEW";
        public static string 余江 = "YHG";
        public static string 叶集 = "YCH";
        public static string 燕郊 = "AJP";
        public static string 姚家 = "YAT";
        public static string 岳家井 = "YGJ";
        public static string 一间堡 = "YJT";
        public static string 英吉沙 = "YIR";
        public static string 云居寺 = "AFP";
        public static string 燕家庄 = "AZK";
        public static string 永康 = "RFH";
        public static string 营口东 = "YGT";
        public static string 银浪 = "YJX";
        public static string 永郎 = "YLW";
        public static string 宜良北 = "YSM";
        public static string 永乐店 = "YDY";
        public static string 伊拉哈 = "YLX";
        public static string 伊林 = "YLB";
        public static string 彝良 = "ALW";
        public static string 杨林 = "YLM";
        public static string 余粮堡 = "YLD";
        public static string 杨柳青 = "YQP";
        public static string 月亮田 = "YUM";
        public static string 亚龙湾 = "TWQ";
        public static string 杨陵镇 = "YSY";
        public static string 义马 = "YMF";
        public static string 玉门 = "YXJ";
        public static string 云梦 = "YMN";
        public static string 元谋 = "YMM";
        public static string 阳明堡 = "YVV";
        public static string 一面山 = "YST";
        public static string 沂南 = "YNK";
        public static string 宜耐 = "YVM";
        public static string 伊宁东 = "YNR";
        public static string 一平浪 = "YIM";
        public static string 营盘水 = "YZJ";
        public static string 羊堡 = "ABM";
        public static string 阳泉北 = "YPP";
        public static string 乐清 = "UPH";
        public static string 焉耆 = "YSR";
        public static string 源迁 = "AQK";
        public static string 姚千户屯 = "YQT";
        public static string 阳曲 = "YQV";
        public static string 榆树沟 = "YGP";
        public static string 月山 = "YBF";
        public static string 玉石 = "YSJ";
        public static string 偃师 = "YSF";
        public static string 沂水 = "YUK";
        public static string 榆社 = "YSV";
        public static string 窑上 = "ASP";
        public static string 元氏 = "YSP";
        public static string 杨树岭 = "YAD";
        public static string 野三坡 = "AIP";
        public static string 榆树屯 = "YSX";
        public static string 榆树台 = "YUT";
        public static string 鹰手营子 = "YIP";
        public static string 源潭 = "YTQ";
        public static string 牙屯堡 = "YTZ";
        public static string 烟筒山 = "YSL";
        public static string 烟筒屯 = "YUX";
        public static string 羊尾哨 = "YWM";
        public static string 越西 = "YHW";
        public static string 攸县 = "YOG";
        public static string 玉溪 = "YXM";
        public static string 永修 = "ACG";
        public static string 酉阳 = "AFW";
        public static string 余姚 = "YYH";
        public static string 弋阳东 = "YIG";
        public static string 岳阳东 = "YIQ";
        public static string 阳邑 = "ARP";
        public static string 鸭园 = "YYL";
        public static string 鸳鸯镇 = "YYJ";
        public static string 燕子砭 = "YZY";
        public static string 宜州 = "YSZ";
        public static string 仪征 = "UZH";
        public static string 兖州 = "YZK";
        public static string 迤资 = "YQM";
        public static string 羊者窝 = "AEM";
        public static string 杨杖子 = "YZD";
        public static string 镇安 = "ZEY";
        public static string 治安 = "ZAD";
        public static string 招柏 = "ZBP";
        public static string 张百湾 = "ZUP";
        public static string 枝城 = "ZCN";
        public static string 子长 = "ZHY";
        public static string 诸城 = "ZQK";
        public static string 邹城 = "ZIK";
        public static string 赵城 = "ZCV";
        public static string 章党 = "ZHT";
        public static string 肇东 = "ZDB";
        public static string 照福铺 = "ZFM";
        public static string 章古台 = "ZGD";
        public static string 赵光 = "ZGB";
        public static string 中和 = "ZHX";
        public static string 中华门 = "VNH";
        public static string 枝江北 = "ZIN";
        public static string 钟家村 = "ZJY";
        public static string 朱家沟 = "ZUB";
        public static string 紫荆关 = "ZYP";
        public static string 周家 = "ZOB";
        public static string 诸暨 = "ZDH";
        public static string 镇江南 = "ZEH";
        public static string 周家屯 = "ZOD";
        public static string 郑家屯 = "ZJD";
        public static string 褚家湾 = "CWJ";
        public static string 湛江西 = "ZWQ";
        public static string 朱家窑 = "ZUJ";
        public static string 曾家坪子 = "ZBW";
        public static string 张兰 = "ZLV";
        public static string 镇赉 = "ZLT";
        public static string 枣林 = "ZIV";
        public static string 扎鲁特 = "ZLD";
        public static string 扎赉诺尔西 = "ZXX";
        public static string 樟木头 = "ZOQ";
        public static string 中牟 = "ZGF";
        public static string 中宁东 = "ZDJ";
        public static string 中宁 = "VNJ";
        public static string 中宁南 = "ZNJ";
        public static string 镇平 = "ZPF";
        public static string 漳平 = "ZPS";
        public static string 泽普 = "ZPR";
        public static string 枣强 = "ZVP";
        public static string 张桥 = "ZQY";
        public static string 章丘 = "ZTK";
        public static string 朱日和 = "ZRC";
        public static string 泽润里 = "ZLM";
        public static string 中山北 = "ZGQ";
        public static string 樟树东 = "ZOG";
        public static string 中山 = "ZSQ";
        public static string 柞水 = "ZSY";
        public static string 钟山 = "ZSZ";
        public static string 樟树 = "ZSG";
        public static string 珠窝 = "ZOP";
        public static string 张维屯 = "ZWB";
        public static string 彰武 = "ZWD";
        public static string 棕溪 = "ZOY";
        public static string 钟祥 = "ZTN";
        public static string 资溪 = "ZXS";
        public static string 镇西 = "ZVT";
        public static string 张辛 = "ZIP";
        public static string 正镶白旗 = "ZXC";
        public static string 紫阳 = "ZVY";
        public static string 枣阳 = "ZYN";
        public static string 竹园坝 = "ZAW";
        public static string 张掖 = "ZYJ";
        public static string 镇远 = "ZUW";
        public static string 朱杨溪 = "ZXW";
        public static string 漳州东 = "GOS";
        public static string 漳州 = "ZUS";
        public static string 壮志 = "ZUX";
        public static string 子洲 = "ZZY";
        public static string 中寨 = "ZZM";
        public static string 涿州 = "ZXP";
        public static string 咋子 = "ZAL";
        public static string 卓资山 = "ZZC";
        public static string 株洲西 = "ZAQ";
        public static string 安阳东 = "ADF";
        public static string 保定东 = "BMP";
        public static string 滨海 = "FHP";
        public static string 滨海北 = "FCP";
        public static string 宝鸡南 = "BBY";
        public static string 长寿北 = "COW";
        public static string 潮汕 = "CBQ";
        public static string 长兴 = "CBH";
        public static string 长阳 = "CYN";
        public static string 潮阳 = "CNQ";
        public static string 东安东 = "DCZ";
        public static string 东二道河 = "DRB";
        public static string 东莞 = "RTQ";
        public static string 大苴 = "DIM";
        public static string 大青沟 = "DSD";
        public static string 德清 = "DRH";
        public static string 定州东 = "DOP";
        public static string 防城港北 = "FBZ";
        public static string 富川 = "FDZ";
        public static string 丰都 = "FUW";
        public static string 涪陵北 = "FEW";
        public static string 抚远 = "FYB";
        public static string 抚州 = "FZG";
        public static string 高碑店东 = "GMP";
        public static string 革居 = "GEM";
        public static string 光明城 = "IMQ";
        public static string 高邑西 = "GNP";
        public static string 鹤壁东 = "HFF";
        public static string 寒葱沟 = "HKB";
        public static string 邯郸东 = "HPP";
        public static string 惠东 = "KDQ";
        public static string 合肥北城 = "COH";
        public static string 洪河 = "HPB";
        public static string 鲘门 = "KMQ";
        public static string 虎门 = "IUQ";
        public static string 哈密南 = "HLR";
        public static string 淮南东 = "HOH";
        public static string 霍邱 = "FBH";
        public static string 惠州南 = "KNQ";
        public static string 军粮城北 = "JMP";
        public static string 将乐 = "JLS";
        public static string 建宁县北 = "JCS";
        public static string 江宁 = "JJH";
        public static string 句容西 = "JWH";
        public static string 建水 = "JSM";
        public static string 库伦 = "KLD";
        public static string 葵潭 = "KTQ";
        public static string 灵璧 = "GMH";
        public static string 离堆公园 = "INW";
        public static string 陆丰 = "LLQ";
        public static string 滦河 = "UDP";
        public static string 漯河西 = "LBN";
        public static string 溧水 = "LDH";
        public static string 溧阳 = "LEH";
        public static string 明港东 = "MDN";
        public static string 庙山 = "MSN";
        public static string 蒙自北 = "MBM";
        public static string 南城 = "NDG";
        public static string 南昌西 = "NXG";
        public static string 南丰 = "NFG";
        public static string 南湖东 = "NDN";
        public static string 普安 = "PAN";
        public static string 普宁 = "PEQ";
        public static string 彭州 = "PMW";
        public static string 青岛北 = "QHK";
        public static string 祁东 = "QMQ";
        public static string 前锋 = "QFB";
        public static string 岐山 = "QAY";
        public static string 庆盛 = "QSQ";
        public static string 祁阳 = "QWQ";
        public static string 全州南 = "QNZ";
        public static string 饶平 = "RVQ";
        public static string 泗洪 = "GQH";
        public static string 三明北 = "SHS";
        public static string 汕尾 = "OGQ";
        public static string 绍兴北 = "SLH";
        public static string 泗县 = "GPH";
        public static string 泗阳 = "MPH";
        public static string 上虞北 = "SSH";
        public static string 深圳北 = "IOQ";
        public static string 深圳坪山 = "IFQ";
        public static string 石嘴山 = "QQJ";
        public static string 石柱县 = "OSW";
        public static string 通海 = "TAM";
        public static string 通化县 = "TXL";
        public static string 泰宁 = "TNS";
        public static string 汤逊湖 = "THN";
        public static string 五女山 = "WET";
        public static string 瓦屋山 = "WAH";
        public static string 许昌东 = "XVF";
        public static string 西丰 = "XFT";
        public static string 孝感北 = "XJN";
        public static string 咸宁东 = "XKN";
        public static string 咸宁南 = "UNN";
        public static string 邢台东 = "EDP";
        public static string 新乡东 = "EGF";
        public static string 西阳村 = "XQF";
        public static string 信阳东 = "OYN";
        public static string 咸阳秦都 = "XOY";
        public static string 迎宾路 = "YFW";
        public static string 永福南 = "YBZ";
        public static string 雨格 = "VTM";
        public static string 洋河 = "GTH";
        public static string 杨陵南 = "YEY";
        public static string 永泰 = "YTS";
        public static string 尤溪 = "YXS";
        public static string 云霄 = "YBS";
        public static string 宜兴 = "YUH";
        public static string 余姚北 = "CTH";
        public static string 诏安 = "ZDS";
        public static string 正定机场 = "ZHP";
        public static string 织金 = "IZW";
        public static string 驻马店西 = "ZLN";
        public static string 漳浦 = "ZCS";
        public static string 庄桥 = "ZQH";
        public static string 涿州东 = "ZAP";
        public static string 卓资东 = "ZDC";
        public static string 郑州东 = "ZAF";
    }
    #endregion

    #region 火车信息
    /// <summary>
    /// 火车信息
    /// </summary>
    public class TrainTicket
    {        
        public string train_no { get; set; }
        /// <summary>
        /// 车号
        /// </summary>
        public string station_train_code { get; set; }
        public string start_station_telecode { get; set; }
        /// <summary>
        /// 始发站
        /// </summary>
        public string start_station_name { get; set; }
        public string end_station_telecode { get; set; }
        /// <summary>
        /// 终点站
        /// </summary>
        public string end_station_name { get; set; }
        public string from_station_telecode { get; set; }
        /// <summary>
        /// 途经站
        /// </summary>
        public string from_station_name { get; set; }
        public string to_station_telecode { get; set; }
        /// <summary>
        /// 途终站
        /// </summary>
        public string to_station_name { get; set; }
        /// <summary>
        /// 出发时间
        /// </summary>
        public string start_time { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        public string arrive_time { get; set; }
        public string day_difference { get; set; }
        public string train_class_name { get; set; }
        /// <summary>
        /// 历时
        /// </summary>
        public string lishi { get; set; }
        public string canWebBuy { get; set; }
        /// <summary>
        /// 历时(整数)
        /// </summary>
        public string lishiValue { get; set; }
        public string yp_info { get; set; }
        public string control_train_day { get; set; }
        public string start_train_date { get; set; }
        public string seat_feature { get; set; }
        public string yp_ex { get; set; }
        public string train_seat_feature { get; set; }
        public string seat_types { get; set; }
        public string location_code { get; set; }
        public string from_station_no { get; set; }
        public string to_station_no { get; set; }
        public string control_day { get; set; }
        public string sale_time { get; set; }
        public string is_support_card { get; set; }
        public string gg_num { get; set; }
        /// <summary>
        /// 高级软卧
        /// </summary>
        public string gr_num { get; set; }
        /// <summary>
        /// 其它
        /// </summary>
        public string qt_num { get; set; }
        /// <summary>
        /// 软卧
        /// </summary>
        public string rw_num { get; set; }
        /// <summary>
        /// 软座
        /// </summary>
        public string rz_num { get; set; }
        /// <summary>
        /// 特等座
        /// </summary>
        public string tz_num { get; set; }
        /// <summary>
        /// 无座
        /// </summary>
        public string wz_num { get; set; }
        public string yb_num { get; set; }
        /// <summary>
        /// 硬卧
        /// </summary>
        public string yw_num { get; set; }
        /// <summary>
        /// 硬座
        /// </summary>
        public string yz_num { get; set; }
        /// <summary>
        /// 二等座
        /// </summary>
        public string ze_num { get; set; }
        /// <summary>
        /// 一等座
        /// </summary>
        public string zy_num { get; set; }
        /// <summary>
        /// 商务座
        /// </summary>
        public string swz_num { get; set; }
        

        public TrainTicket()
        {
            train_no = "";
            station_train_code = "";
            start_station_telecode = "";
            start_station_name = "";
            end_station_telecode = "";
            end_station_name = "";
            from_station_telecode = "";
            from_station_name = "";
            to_station_telecode = "";
            to_station_name = "";
            start_time = "";
            arrive_time = "";
            day_difference = "";
            train_class_name = "";
            lishi = "";
            canWebBuy = "";
            lishiValue = "";
            yp_info = "";
            control_train_day = "";
            start_train_date = "";
            seat_feature = "";
            yp_ex = "";
            train_seat_feature = "";
            seat_types = "";
            location_code = "";
            from_station_no = "";
            to_station_no = "";
            control_day = "";
            sale_time = "";
            is_support_card = "";
            gg_num = "";
            gr_num = "";
            qt_num = "";
            rw_num = "";
            rz_num = "";
            tz_num = "";
            wz_num = "";
            yb_num = "";
            yw_num = "";
            yz_num = "";
            ze_num = "";
            zy_num = "";
            swz_num = "";
        }
    }
    #endregion
}
