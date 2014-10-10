using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 基本QQ信息
    /// 
    /// </summary>
    public class QQBasicInfo
    {
        public QQBasicInfo(int uin, QQType type, int groupId)
        {
            this.QQ = uin;
            this.GroupId = groupId;
            this.Type = type;
        }
        /// <summary>
        /// QQ号
        /// </summary>
        public int QQ { get; set; }
        public QQType Type {get;set;}
        public int GroupId { get; set; }
    }

    /// <summary>
    /// QQ类型
    /// </summary>
    public enum QQType:byte
    {
        QQ=0x01,
        Cluster=0x04
        

        
        //private QQType Friend = new QQType("QQ");
        //private QQType Cluster = new QQType("群");
        //private QQType(string type)
        //{
            
        //}
        //public static QQType Friend


    }
}
