
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>请求传送文件包的数据封装类，传送文件包是发送消息包的变种格式
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// 文件名
    /// </summary>
        /// <value></value>
        public string FileName { get; set; }
        /// <summary>
        /// 文件大小
    /// </summary>
        /// <value></value>
        public int FileSize { get; set; }
        /// <summary>
        /// 给定一个输入流，解析SendFileRequest结构
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            // 跳过空格符和分隔符
            buf.GetChar();
            // 获取后面的所有内容
            byte[] b = buf.GetByteArray(buf.Remaining());
            // 找到分隔符
            int i = Array.IndexOf<byte>(b, 0, (byte)0x1F);
            // 得到文件名
            FileName = Utils.Util.GetString(b, 0, i);
            // 得到文件大小的字符串形式
            String sizeStr = Utils.Util.GetString(b, i + 1, b.Length - 6 - i);
            FileSize = Utils.Util.GetInt(sizeStr, 0);
        }
    }
}
