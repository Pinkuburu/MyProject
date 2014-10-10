using System;
namespace LFNet.QQ.Entities
{
    interface INormalIM
    {
        FontStyle FontStyle { get; set; }
        int FragmentSequence { get; set; }
        bool HasFontAttribute { get; set; }
        string Message { get; }
        byte[] MessageBytes { get; set; }
        int MessageId { get; set; }
        void Read(ByteBuffer buf);
        LFNet.QQ.ReplyType ReplyType { get; set; }
        int TotalFragments { get; set; }
    }
}
