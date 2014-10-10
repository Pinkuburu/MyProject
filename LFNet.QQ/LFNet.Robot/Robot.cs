#region 版权声明
//=========================================================== 
// 版权声明：LFNet.QQ是基于QQ2009版本的QQ协议开发而成的，协议
// 分析主要参考自小虾的MyQQ(C++)源代码，代码开发主要基于阿布
// 的LumaQQ.NET的C#.NET代码修改而成，故继续遵照使用LumaQQ的开
// 源协议。
//
// 本人没有对LumaQQ.NET的C#.NET代码的框架做过多的改动，主
// 要工作为将MyQQ的C++协议代码部分翻译成符合LumaQQ.Net框架
// 的C#代码，故请尊重LumaQQ作者Luma的著作权和版权声明。
// 
// 代码开源主要用于解决大家在学习和研究协议过程中遇到由于缺乏代码所带来的制约性问题。
// 本代码仅供学习交流使用，大家在使用此开发包前请自行协调好多方面关系，
// 不得用于任何商业用途和非法用途，本人不享受和承担由此产生的任何权利以及任何法律责任。
// 
// 本源代码可通过以下网址获取:
// http://QQCode.lynfo.com, http://www.lynfo.com, http://bbs.lynfo.com, http://hi.baidu.com/dobit.
//
// Copyright @ 2009-2010  Lynfo.com.  All Rights Reserved.   
// Framework: 2.0
// Author: Luma(java版) → Abu(C# QQ2005协议版) → Dobit(C# QQ2009协议版本)
// Email: dobit@msn.cn   
// Created: 2009-3-1~ 2009-11-28
// Last Modified:2009-11-28    
//   
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details. 
//===========================================================   
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace LFNet.Robot
{
    public class Robot
    {
        public static List<IRobot> Robots = new List<IRobot>();
        public Robot()
        {
            //LoadRobots();
        }
        /// <summary>
        /// 添加一个接口
        /// </summary>
        /// <param name="robot"></param>
        /// <returns></returns>
        public bool Add(IRobot robot)
        {
            return true;
        }

        public bool Remove(IRobot robot)
        {
            if (Robots.Contains(robot))
                Robots.Remove(robot);
            //SaveConfig();
            return true;
        }

        public IRobot RobotInstance (string assemlyName)//LFNet.Robot.Robot, LFNet.Robot
        {
            IRobot obj = (IRobot)Activator.CreateInstance(Type.GetType(assemlyName), false, true); //Activator.CreateInstance(Type, false, true));
                return obj;
        }


        public static  Dictionary<Guid,bool> EnglishReplys=new Dictionary<Guid, bool>();
        public static string Parse(string message,Guid guid, object userObject)
        {

            Regex re = new Regex(@"^[/-](\w+)\s*(\w*/*\w*/*\w*/*\w*)\s*(\w*)\s*(\w*).*$", RegexOptions.None);
            MatchCollection mc = re.Matches(message);
            Match m = re.Match(message);
            List<string> args = new List<string>();
            //object m;
            //m.
            //string cmd = string.Empty;
            //foreach (Group g in m.Groups)
            //{
            //    if (!string.IsNullOrEmpty(g.ToString().Trim()))
            //    {
            //        args.Add(g.ToString());
            //    }

            //}
            //if(args.Count>0)
            //    args.RemoveAt(0);
            for (int i = 0; i < m.Groups.Count; i++)
            {
                //if (i == 0) cmd = m.Groups[i].ToString().Trim();
                // else 
                if (i != 0 && (!string.IsNullOrEmpty(m.Groups[i].ToString())))
                {
                    args.Add(m.Groups[i].ToString());
                }

            }
            if (args.Count > 0 && !string.IsNullOrEmpty(args[0]))
            {
                switch (args[0])
                {
                    case "?":
                    case "h":
                    case "help":
                        return HelpString;
                    case "en":

                        break;
                    case "t":
                    case "train":
                        return Train.Robot(args.ToArray());
                    
                    default: return "指令不支持，输入/help查看帮助信息！";
                }
            }
            return "";

            //return HelpString();
        }



        public static string HelpString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(@"欢迎使用Dobit的QQ机器人服务！");
                sb.AppendLine("机器人支持以下命令：");
                sb.AppendLine("/help 获取指令列表帮助;");
                sb.AppendLine("/train或/t 提供列车时刻表查询;");
                sb.AppendLine("/en  开启关闭英语辅助聊天;");
                sb.AppendLine("祝大家使用愉快！官方网站www.lynfo.com");
                return sb.ToString();
            }
        }


    }
}
