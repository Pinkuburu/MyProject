
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    public class ClusterCommandPacket : BasicOutPacket
    {
        public ClusterCommand SubCommand { get; set; }
        public int ClusterId { get; set; }

        /** 字体属性 */
        protected const byte NONE = 0x00;
        /// <summary>
        /// 
        /// </summary>
        protected const byte BOLD = 0x20;
        /// <summary>
        /// 
        /// </summary>
        protected const byte ITALIC = 0x40;
        /// <summary>
        /// 
        /// </summary>
        protected const byte UNDERLINE = (byte)0x80;

        public ClusterCommandPacket(QQClient client) : base(QQCommand.Cluster_Cmd,true,client) { }
        public ClusterCommandPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void ParseBody(ByteBuffer buf)
        {
            SubCommand = (ClusterCommand)buf.Get();
        }
        public override string GetPacketName()
        {
            switch (SubCommand)
            {
                case ClusterCommand.CREATE_CLUSTER:
                    return "Cluster Create Packet";
                case ClusterCommand.MODIFY_MEMBER:
                    return "Cluster Modify Member Packet";
                case ClusterCommand.MODIFY_CLUSTER_INFO:
                    return "Cluster Modify Info Packet";
                case ClusterCommand.GET_CLUSTER_INFO:
                    return "Cluster Get Info Packet";
                case ClusterCommand.ACTIVATE_CLUSTER:
                    return "Cluster Activate Packet";
                case ClusterCommand.SEARCH_CLUSTER:
                    return "Cluster Search Packet";
                case ClusterCommand.JOIN_CLUSTER:
                    return "Cluster Join Packet";
                case ClusterCommand.JOIN_CLUSTER_AUTH:
                    return "Cluster Auth Packet";
                case ClusterCommand.EXIT_CLUSTER:
                    return "Cluster Exit Packet";
                case ClusterCommand.GET_ONLINE_MEMBER:
                    return "Cluster Get Online Member Packet";
                case ClusterCommand.GET_MEMBER_INFO:
                    return "Cluster Get Member Info Packet";

                case ClusterCommand.SEND_IM_EX:
                case ClusterCommand.SEND_IM_EX09:
                    return "Cluster Send IM Ex Packet";
                case ClusterCommand.CREATE_TEMP:
                    return "Cluster Create Temp Cluster Packet";
                case ClusterCommand.MODIFY_TEMP_MEMBER:
                    return "Cluster Modify Temp Cluster Member Packet";
                case ClusterCommand.EXIT_TEMP:
                    return "Cluster Exit Temp Cluster Packet";
                case ClusterCommand.GET_TEMP_INFO:
                    return "Cluster Get Temp Cluster Info Packet";
                case ClusterCommand.ACTIVATE_TEMP:
                    return "Cluster Get Temp Cluster Member Packet";
                case ClusterCommand.MODIFY_CARD:

                case ClusterCommand.GET_CARD_BATCH:

                case ClusterCommand.GET_CARD:

                case ClusterCommand.COMMIT_ORGANIZATION:

                case ClusterCommand.UPDATE_ORGANIZATION:

                case ClusterCommand.COMMIT_MEMBER_ORGANIZATION:

                case ClusterCommand.GET_VERSION_ID:

                case ClusterCommand.SET_ROLE:

                case ClusterCommand.TRANSFER_ROLE:

                case ClusterCommand.DISMISS_CLUSTER:

                case ClusterCommand.MODIFY_TEMP_INFO:

                case ClusterCommand.SEND_TEMP_IM:

                case ClusterCommand.SUB_CLUSTER_OP:

                default:
                    return "Unknown Cluster Command Packet";
            }
        }
        protected override void PutBody(ByteBuffer buf)
        {
            
        }
    }
}
