



using System;
using System.Collections.Generic;
using System.Text;
using Lynfo.Train;


namespace LFNet.Robot
{
    class Train
    {
        public static string Robot(string[] args)
        {
            switch (args.Length)
          {
              //case 0: return "";
              //case 1: return "";
              case 2: return GetTrain(args[1]);
              case 3: return GetTrains(args[1], args[2]);
              case 4: return GetTrains(args[1], args[2], args[3]);
              default:return HelpMessage();
          }

          //if(args.Length>1)
          //{
          //  //string[] ts=  message.Split(' ');
          

          //}else
          //    return 
          
        
        }

        private static string GetTrains(string f, string t, string c)
        {
            StringBuilder sb = new StringBuilder();
            //List<CStationInfo> csl = new List<CStationInfo>();
            List<TrainInfo> tl = TrainFactory.GetTrains(f,c, t);
            if (tl.Count > 0)
            {
                sb.AppendLine("\t查询结果");
                sb.AppendLine("\t车次\t始发站\t终点站\t发 站\t发时\t到站\t到时");
                foreach (TrainInfo ti in tl)
                {
                    sb.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", ti.TId, ti.FirstStation.Station, ti.EndStation.Station, ti.StartStation.Station, TrainFactory.TimeSpanToString(ti.StartStation.StartTime, "{1:00}:{2:00}"), ti.EndStation.Station, TrainFactory.TimeSpanToString(ti.EndStation.StartTime, "{1:00}:{2:00}")));
                }

            }
            else return string.Format("{0} 和 {1} 之间不能直接通过 {2}中转!", f, t, c) ;
            return sb.ToString();
        }

        private static string GetTrains(string f, string t)
        {
            StringBuilder sb = new StringBuilder();
            List<CStationInfo> csl = new List<CStationInfo>();
            List<TrainInfo> tl = TrainFactory.GetTrains(f, t, out csl);
            if (tl.Count > 0)
            {
                sb.AppendLine("\t找到 " + tl.Count + " 趟列车");
                sb.AppendLine(string.Format("{0,-8}\t{1,3}\t{2,3}\t{3,3}\t{4,3}\t{5,4}\t{6,4}\t{7,4}","车次","始发站","终点站","发站","发时","到站","到时","历时"));
                foreach (TrainInfo ti in tl)
                {
                    sb.AppendLine(string.Format("{0,-10}\t{1,6}\t{2,6}\t{3,6}\t{4,6}\t{5,6}\t{6,6}\t{7,6}", ti.TId, ti.FirstStation.Station, ti.EndStation.Station, ti.StartStation.Station, TrainFactory.TimeSpanToString(ti.StartStation.StartTime, "{1:00}:{2:00}"), ti.EndStation.Station, TrainFactory.TimeSpanToString(ti.EndStation.EndTime, "{1:00}:{2:00}"), TrainFactory.TimeSpanToString(ti.LastTime, "{0:00}:{1:00}", "")));
                }

            }
            else if (csl.Count > 0)
            {
                sb.AppendLine("\t无直达列车，找到中转站 " + csl.Count + " 个");
                sb.AppendLine(string.Format("{0,3}\t{1,5}\t{2,5}\t{3,5}\t{4,4}\t{5,4}","中转站","中转前历时","中转后历时","总历时","发车数","到站车数"));
                foreach (CStationInfo csi in csl)
                {
                    sb.AppendLine(string.Format("{0,3}\t{1,10}\t{2,10}\t{3,10}\t{4,6}\t{5,8}", csi.Station, TrainFactory.TimeSpanToString(csi.BeforeTime, "{0:00}:{1:00}"), TrainFactory.TimeSpanToString(csi.AfterTime, "{0:00}:{1:00}"), TrainFactory.TimeSpanToString(csi.TotalTime, "{0:00}:{1:00}"), csi.BeforeStations, csi.AfterStations));

                }

            }
            else
                return "没找到"+f +"到" +t +"之间的列车！";

            return sb.ToString();
        }

        private static string GetTrain(string p)
        {
            StringBuilder sb = new StringBuilder();
            TrainInfo ti = TrainFactory.GetTrain(p);
            if(ti.FirstStation==null)
            { 
                return "没有该趟列车！"; 
            }
            else 
            {
                sb.AppendLine(string.Format("{0,-6}\t{1,4}\t{2,4}\t{3,4}\t{4,8}\t{5,4}\t{6,4}","车站","到时","发时","天数","里程(km)","平均车速","历时"));
                foreach (StationInfo si in ti.StationList)
                {
                    sb.AppendLine(string.Format("{0,6}\t{1,6}\t{2,6}\t{3,3}\t{4,8}\t{5,8}\t{6,6}", si.Station, TimeSpanToString(si.EndTime), TimeSpanToString(si.StartTime), DayToString(si.StartTime.Days == 30 ? si.EndTime.Days : si.StartTime.Days), si.Km, si.Speed.ToString("F00"), TrainFactory.TimeSpanToString(si.LastTime, "{0:00}:{1:00}", "")));
                }
            }

            return sb.ToString();
        }

        private static object DayToString(int day)
        {
            string str;
            if (day == 0) str = "当天";
            else str = "第" + (day + 1) + "天";
            return str;
        }

        private static string TimeSpanToString(TimeSpan t)
        {
            if (t == new TimeSpan(30, 0, 0, 0)) return "-";
            return string.Format("{0:00}:{1:00}", t.Hours, t.Minutes);
        }

        private static string HelpMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"本次列车时刻表更新日期为"+TrainFactory.Version);
            sb.AppendLine(@"支持的指令如下:");
            sb.AppendLine("/? -获取帮助信息;");
            sb.AppendLine("站名 -车站过境的全部车次;");
            sb.AppendLine("发站 到站 -查询两站之间的全部列车,当找不到时返回可中转的车站;");
            sb.AppendLine(@"发站 到站 中转站 -根据中转站查询相关列车信息;");
            sb.AppendLine(@"车次 -该车次列车停站信息;");
            //sb.AppendLine(@"车次 发站 到站 -该车次列车停站信息;");
            sb.AppendLine("支持网站:www.lynfo.com.");

            return sb.ToString();

        }
    }
}
