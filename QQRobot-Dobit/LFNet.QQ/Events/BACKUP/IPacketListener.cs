
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Packets;
namespace LFNet.QQ.Events
{
    public interface IPacketListener
    {        
        void PacketArrived(InPacket inPacket);
         bool Accept(InPacket inPacket);
    }
}
