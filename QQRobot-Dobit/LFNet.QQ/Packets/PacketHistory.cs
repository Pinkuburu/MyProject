
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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
namespace LFNet.QQ.Packets
{
    /// <summary>
    /**
    * 包监视器.
    * 这个类使用一个hashtable和一个linked list来管理收到的包，其他人可以调用他的
    * 方法来检查一个包是否收到，比如重发线程通过它来检查是否一个包的应答已经到达，
    * 如果是，则不需要重发。还有接收线程，一旦收到一个包，就把这个包的hash值存入这
    * 个类，还有包处理器，也要通过来检查是否一个包是重复的。当然这种方法还不能完美
    * 的避免任何不必要的重发，但是至少有点帮助，-_-!....
    * 另外这个包监视器还要管理所有的请求，因为这些包的应答包里面没有什么有用的信息，
    * 信息都在请求包里，所以...
    * <br>(edit by notxx)
    * 改变成使用LinkedHashSet和HashMap来管理. 速度应该快一些.
    * 不需要创建一个Integer对象作为key了, 直接使用Packet本身即可.
    * 
    * @see notxx.lumaqq.qq.packet.Packet#hashCode()
    * @see notxx.lumaqq.qq.packet.Packet#equals(Object)
    * 
    * @author luma
    * @author notXX
    */
    /// </summary>
    public class PacketHistory
    {
        /// <summary>
        /// 用于重复包检测的链接哈希表
        /// </summary>
        private List<int> hash;
        /// <summary>
        /// 用于请求的哈希表
        /// </summary>
        private Hashtable sent;
        /// <summary>
        /// 阈值，超过时清理hash中的数据
        /// </summary>
        static int THRESHOLD = 100;
        public PacketHistory()
        {
            hash = new List<int>();
            sent = new Hashtable();
        }
        /// <summary>
        /// 这个方法检查包是否已收到，要注意的是检查是针对这个包的hash值进行的，
        /// 并不是对packet这个对象，hash值的计算是在packet的hashCode中完成的，
        /// 如果两个packet的序号或者命令有不同，则hash值肯定不同。
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="add">if set to <c>true</c> 如果为true，则当这个包不存在时，添加这个包的hash，否则不添加.</param>
        /// <returns>true如果这个包已经收到，否则false</returns>
        public bool Check(Packet packet, bool add)
        {
            return Check(packet.GetHashCode(), add);
        }

        /// <summary>
        /// 检查指定的hash值是否已经存在
        /// </summary>
        /// <param name="hashValue">The hash value.</param>
        /// <param name="add">if set to <c>true</c> 表示如果不存在则添加这个哈希值.</param>
        /// <returns>true表示已经存在</returns>
        public bool Check(int hashValue, bool add)
        {
            // 检查是否已经有了
            if (hash.Contains(hashValue))
                return true;
            else
            {
                // 如果add标志为false，不添加
                if (add)
                {
                    hash.Add(hashValue);
                }
                else
                    return false;
            }
            // 检查是否超过了阈值
            if (hash.Count >= THRESHOLD)
            {
                hash.RemoveRange(0, THRESHOLD / 2);
            }
            return false;
        }

        /// <summary>
        ///  这个方法检查包是否已收到，要注意的是检查是针对这个包的hash值进行的，
        ///  并不是对packet这个对象，hash值的计算是在packet的hashCode中完成的，
        ///  如果两个packet的序号或者命令有不同，则hash值肯定不同。
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="add">if set to <c>true</c> [add].</param>
        /// <returns>true表示这个包已经收到，否则false</returns>
        public bool Check(object packet, bool add)
        {
            return Check((Packet)packet, add);
        }

        /// <summary>
        /// 把请求推入哈希表
        /// </summary>
        /// <param name="packet">The packet.</param>
        public void PutSent(OutPacket packet)
        {
            if (!sent.Contains(packet))
            {

                lock (sent)
                {
                    sent.Add(packet, packet);
                }
            }           
        }

        /// <summary>
        /// 返回这个回复包对应的请求包
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <returns>你的请求包</returns>
        public OutPacket RetrieveSent(InPacket packet)
        {
            if (!sent.Contains(packet))
            {
                return null;
            }
            OutPacket outPacket = (OutPacket)sent[packet];
            sent.Remove(packet);
            return outPacket;
        }
        /// <summary>
        /// 清空包监视缓冲区
        /// </summary>
        public void Clear()
        {
            hash.Clear();
            sent.Clear();
        }
    }
}
