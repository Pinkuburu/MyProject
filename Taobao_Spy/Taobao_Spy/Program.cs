using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Taobao_Spy
{
    class Program
    {
        static int i = 0;
        static void Main(string[] args)
        {
            string strContent = null;
            string strRequest = null;
            int intStarIndex = 0;
            int intEndIndex = 0;
            int intPageSize = 0;
            string strP_Link = null;
            string strCCCCC = null;

            Taobao_Spy.WebClient HTTPproc = new WebClient();
            HTTPproc.Encoding = System.Text.Encoding.Default;

            //截取商品列表内容部分
            HTTPproc.OpenRead("http://midway.tmall.com/?orderType=_hotsell&search=y&pageNum=1");
            strRequest = HTTPproc.ResponseHeaders[3].ToString();
            HTTPproc.OpenRead(strRequest);
            strRequest = HTTPproc.ResponseHeaders[4].ToString();
            HTTPproc.OpenRead(strRequest);
            strRequest = HTTPproc.ResponseHeaders[3].ToString();
            strContent = HTTPproc.OpenRead(strRequest);
            //得到截取段的索引
            intStarIndex = strContent.IndexOf("page-top page-mini");
            intEndIndex = strContent.IndexOf("确定") - strContent.IndexOf("page-top page-mini");

            //截取后内容
            strContent = strContent.Substring(intStarIndex, intEndIndex);

            //得到商品总页数
            try
            {
                intPageSize = Convert.ToInt32(Regex.Match(strContent, @"/\d*<").Value.Replace("/","").Replace("<",""));
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            for (int i = 1; i <= intPageSize; i++)
            {
                if (i == 1)
                {
                    //ProductAnalyze(strContent);
                }
                else
                {
                    //截取商品列表内容部分
                    strContent = HTTPproc.OpenRead("http://midway.tmall.com/?orderType=_hotsell&search=y&pageNum=" + i.ToString());

                    //得到截取段的索引
                    intStarIndex = strContent.IndexOf("page-top page-mini");
                    intEndIndex = strContent.IndexOf("确定") - strContent.IndexOf("page-top page-mini");

                    //截取后内容
                    strContent = strContent.Substring(intStarIndex, intEndIndex);

                    //ProductAnalyze(strContent);
                }

                
                try
                {
                    //Regex regexObj = new Regex(@"http://item\.taobao\.com/item\.htm\?id=\d*"" t");
                    Regex regexObj = new Regex("【.*");
                    Regex regexObj_1 = new Regex("已销售.*");
                    Match matchResults = regexObj.Match(strContent);
                    Match matchResults_1 = regexObj_1.Match(strContent);
                    while (matchResults.Success)
                    {
                        // matched text: matchResults.Value
                        // match start: matchResults.Index
                        // match length: matchResults.Length
                        strP_Link = matchResults_1.Value.Replace("<em>", "").Replace("</em>", "").ToString();
                        strP_Link += "   " + matchResults.Value.ToString();
                        if (i == 0)
                        {
                            strCCCCC = strP_Link + "\r\n";
                            i++;
                        }
                        else
                        {
                            strCCCCC += strP_Link + "\r\n";
                        }
                        Console.WriteLine(strP_Link);
                        matchResults = matchResults.NextMatch();
                        matchResults_1 = matchResults_1.NextMatch();
                    }
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }
            }
            CreateTxt(strCCCCC);
        }

        #region 生成特定文件格式的TXT文档
        /// <summary>
        /// 生成特定文件格式的TXT文档
        /// </summary>
        public static void CreateTxt(string strContent)//生成特定文件格式的TXT文档
        {
            DateTime dt = DateTime.Now;
            string Path = Environment.CurrentDirectory;
            FileStream fs = new FileStream(@Path + "\\" + dt.ToString("yyyy-MM-dd HHmmss") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("GB2312"));//通过指定字符编码方式可以实现对汉字的支持，否则在用记事本打开查看会出现乱码
            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            sw.WriteLine(strContent);
            sw.Flush();
            sw.Close();
        }
        #endregion
    }
}
