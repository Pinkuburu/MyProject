
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Packets;
namespace LFNet.QQ.Events
{
    /// <summary>
    /// 包处理器
    /// </summary>
    public class ProcessorRouter
    {
        private List<IPacketListener> listeners;
        private QQClient client;
        public ProcessorRouter(QQClient client)
        {
            this.client = client;
            listeners = new List<IPacketListener>();
        }
        /// <summary>装载包处理器
    /// </summary>
        /// <param name="listener">The listener.</param>
        public void InstallProcessor(IPacketListener listener)
        {
            listeners.Add(listener);
        }
        public void PacketArrived(InPacket inPacket)
        {
            try
            {
                foreach (IPacketListener listener in listeners)
                {
                    if (listener.Accept(inPacket))
                    {
                        listener.PacketArrived(inPacket);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                client.LogManager.Log(ToString() + ":" + e.Message + "\n\r" + e.StackTrace);
                ErrorPacket errorPacket = new ErrorPacket(ErrorPacketType.RUNTIME_ERROR, client,e);
                errorPacket.ErrorMessage = client.GenerateCrashReport(e, inPacket);
                errorPacket.Family = ProtocolFamily.Basic;
                errorPacket.ConnectionId = LFNet.QQ.Net.QQPort.Main.Name;
                client.PacketManager.AddIncomingPacket(errorPacket, errorPacket.ConnectionId);
                
            }
        }
    }
}
