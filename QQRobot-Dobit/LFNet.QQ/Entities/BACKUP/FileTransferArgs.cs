
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 传送文件的ip，端口信息封装类
    /// </summary>
    public class FileTransferArgs
    {
        // 传输类型
        public TransferType TransferType { get; set; }
        // 连接方式
        public FileConnectMode ConnectMode { get; set; }
        // 发送者外部ip
        public byte[] InternetIP { get; set; }
        /// <summary>
        /// 发送者外部端口
    /// </summary>
        /// <value></value>
        public int InternetPort { get; set; }
        /// <summary>
        /// 第一个监听端口
    /// </summary>
        /// <value></value>
        public int MajorPort { get; set; }
        /// <summary>
        /// 发送者真实ip
    /// </summary>
        /// <value></value>
        public byte[] LocalIP { get; set; }
        /// <summary>
        /// 第二个监听端口
    /// </summary>
        /// <value></value>
        public int MinorPort { get; set; }

        /// <summary>
        /// 给定一个输入流，解析FileTransferArgs结构
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            // 跳过19个无用字节
            buf.Position = buf.Position + 19;
            // 读取传输类型
            TransferType = (TransferType)buf.Get();
            // 读取连接方式
            ConnectMode = (FileConnectMode)buf.Get();
            // 读取发送者外部ip
            InternetIP = buf.GetByteArray(4);
            // 读取发送者外部端口
            InternetPort = (int)buf.GetUShort();
            // 读取文件传送端口
            if (ConnectMode != FileConnectMode.DIRECT_TCP)
                MajorPort = (int)buf.GetUShort();
            else
                MajorPort = InternetPort;
            // 读取发送者真实ip
            LocalIP = buf.GetByteArray(4);
            // 读取发送者真实端口
            MinorPort = (int)buf.GetUShort();
        }
    }
}
