using System;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace ReadSystemInfo
{
    class Program
    {
        static long epoch = 0;
        static void Main(string[] args)
        {
            JObject jsonObj = null;
            string strJson = GetJson("zqs.wxqmx.jj.cn");
            if (strJson.Trim() != "")
            {
                jsonObj = JObject.Parse(strJson);
                Console.WriteLine(GetSystemInfo(jsonObj));
                //Console.WriteLine(jsonObj["Text"]);
                Console.ReadKey();
            }
        }

        #region 读取JSON信息 GetJson()
        /// <summary>
        /// 读取JSON信息
        /// </summary>
        /// <returns></returns>
        private static string GetJson(string strURL)
        {
            WebClient HTTPproc = new WebClient();
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            return HTTPproc.OpenRead("http://" + strURL + ":8085/data.json");
        }
        #endregion 读取JSON信息 GetJson()

        #region 读取系统信息 GetSystemInfo(JObject jsonObj)
        /// <summary>
        /// 读取系统信息
        /// </summary>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        private static string GetSystemInfo(JObject jsonObj)
        {
            //ProcessorNumber,Cores,Speed,Temperature,Load
            string _ProcessorNumber = null;
            string strTemp = null;
            int _CPUNumber = 0;
            int _Cores = 0;
            string _Speed = null;
            string _Temperature = null;
            string _Load = null;
            string _IPAddress = null;
            string _FreeMemory = null;
            string _UseMemory = null;
            string _UseDisk = null;
            JArray jArr = null;

            //JArray jArr = JArray.Parse(jsonObj["Children"][0]["Children"].ToString());
            //Console.WriteLine(jArr.Count);

            if (jsonObj != null)
            {
                //读取IP地址
                _IPAddress = jsonObj["ip"].ToString();

                //读取CPU信息
                foreach (JObject joProcessorNumber in jsonObj["Children"][0]["Children"])
                {
                    strTemp = joProcessorNumber["Text"].ToString();
                    if (strTemp.IndexOf("Intel") > -1 || strTemp.IndexOf("AMD") > -1)
                    {
                        _ProcessorNumber = strTemp;
                        _CPUNumber++;
                    }

                    //读取内存信息
                    if (strTemp.IndexOf("Memory") > -1)
                    {                        
                        foreach (JObject joMemory in joProcessorNumber["Children"])
                        {
                            strTemp = joMemory["Text"].ToString();
                            if (strTemp == "数据")
                            {
                                foreach (JObject joMemoryData in joMemory["Children"])
                                {
                                    strTemp = joMemoryData["Text"].ToString();
                                    if (strTemp == "已用内存")
                                    {
                                        _UseMemory = joMemoryData["Value"].ToString();
                                    }

                                    if (strTemp == "可用内存")
                                    {
                                        _FreeMemory = joMemoryData["Value"].ToString();
                                    }
                                }
                            }
                        }                        
                    }

                    //读取硬盘信息
                    if (strTemp.IndexOf("Disk") > -1)
                    {
                        _UseDisk = joProcessorNumber["Children"][0]["Children"][0]["Value"].ToString();
                    }
                }

                //读取温度及CPU内核数目
                foreach (JObject joTemperatureAndCores in jsonObj["Children"][0]["Children"][0]["Children"])
                {
                    strTemp = joTemperatureAndCores["Text"].ToString();
                    if (strTemp == "温度")
                    {
                        jArr = JArray.Parse(joTemperatureAndCores["Children"].ToString());
                        _Cores = jArr.Count;
                        _Temperature = joTemperatureAndCores["Children"][0]["Value"].ToString();
                    }

                    if (strTemp == "负荷")
                    {
                        _Load = joTemperatureAndCores["Children"][0]["Value"].ToString();
                    }
                }

                //Console.WriteLine(_IPAddress);
                //Console.WriteLine(_ProcessorNumber);
                //Console.WriteLine(_CPUNumber);
                _Cores = _Cores * _CPUNumber;
                //Console.WriteLine(_Cores);
                //Console.WriteLine(_Temperature);
                //Console.WriteLine(_Load);
                //Console.WriteLine(_UseMemory);
                //Console.WriteLine(_FreeMemory);
                //Console.WriteLine(_UseDisk);
            }

            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", _IPAddress, _ProcessorNumber, _CPUNumber, _Cores, _Temperature, _Load, _UseMemory, _FreeMemory, _UseDisk);
        }
        #endregion 读取系统信息 GetSystemInfo(JObject jsonObj)
    }
}
