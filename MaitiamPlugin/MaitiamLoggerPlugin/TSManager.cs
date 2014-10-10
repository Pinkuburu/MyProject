using System;
using System.Runtime.InteropServices;

namespace MaitiamLoggerPlugin
{
    class TSManager
    {
        /// <summary>
        /// WTSOpenServer Opens the handle to Server
        /// </summary>
        /// <param name="pServerName"></param>
        /// <returns></returns>
        [DllImport("wtsapi32.dll")]
        static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] String pServerName);

        /// <summary>
        /// WTSCloseServer, close the handle to server
        /// </summary>
        /// <param name="hServer"></param>
        [DllImport("wtsapi32.dll")]
        static extern void WTSCloseServer(IntPtr hServer);

        /// <summary>
        /// WTSEnumerateSessions, Enumerate Sessions on a Server
        /// </summary>
        /// <param name="hServer"></param>
        /// <param name="Reserved"></param>
        /// <param name="Version"></param>
        /// <param name="ppSessionInfo"></param>
        /// <param name="pCount"></param>
        /// <returns></returns>
        [DllImport("wtsapi32.dll")]
        static extern Int32 WTSEnumerateSessions(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.U4)] Int32 Reserved,
            [MarshalAs(UnmanagedType.U4)] Int32 Version,
            ref IntPtr ppSessionInfo,
            [MarshalAs(UnmanagedType.U4)] ref Int32 pCount);

        /// <summary>
        /// Structure holding TS Session
        /// </summary>
        public struct TsSession
        {
            public int SessionID;
            public string StationName;
            public string ConnectionState;
            public string UserName;
            public string DomainName;
            public string ClientName;
            public string IpAddress;
        }

        public const int WTS_CURRENT_SESSION = -1;


        /// <summary>
        /// WTSQuerySessionInformation - Query Session Information
        /// </summary>
        /// <param name="hServer"></param>
        /// <param name="sessionId"></param>
        /// <param name="wtsInfoClass"></param>
        /// <param name="ppBuffer"></param>
        /// <param name="pBytesReturned"></param>
        /// <returns></returns>
        [DllImport("Wtsapi32.dll")]
        public static extern bool WTSQuerySessionInformation(
            System.IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out System.IntPtr ppBuffer, out uint pBytesReturned);

        /// <summary>
        /// Free WTS Memory
        /// </summary>
        /// <param name="pMemory"></param>
        [DllImport("wtsapi32.dll")]
        static extern void WTSFreeMemory(IntPtr pMemory);

        //Structure for TS Client IP Address 
        [StructLayout(LayoutKind.Sequential)]
        private struct _WTS_CLIENT_ADDRESS
        {
            public int AddressFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] Address;
        }

        /// <summary>
        /// Session Info
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_SESSION_INFO
        {
            public Int32 SessionID;

            [MarshalAs(UnmanagedType.LPStr)]
            public String pWinStationName;

            public WTS_CONNECTSTATE_CLASS State;
        }

        #region Enumurations

        public enum WTS_CONNECTSTATE_CLASS
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }

        public enum WTS_INFO_CLASS
        {
            WTSInitialProgram,
            WTSApplicationName,
            WTSWorkingDirectory,
            WTSOEMId,
            WTSSessionId,
            WTSUserName,
            WTSWinStationName,
            WTSDomainName,
            WTSConnectState,
            WTSClientBuildNumber,
            WTSClientName,
            WTSClientDirectory,
            WTSClientProductId,
            WTSClientHardwareId,
            WTSClientAddress,
            WTSClientDisplay,
            WTSClientProtocolType
        }

        #endregion

        /// <summary>
        /// Wrapper to Open a Server
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static IntPtr OpenServer(String Name)
        {
            IntPtr server = WTSOpenServer(Name);
            return server;
        }

        /// <summary>
        /// Wrapper to Close Server
        /// </summary>
        /// <param name="ServerHandle"></param>
        public static void CloseServer(IntPtr ServerHandle)
        {
            WTSCloseServer(ServerHandle);
        }

        /// <summary>
        /// GetClientIP
        /// </summary>
        /// <param name="intSession">SessionID</param>
        /// <returns></returns>
        public static string ListSessions(int intSession)
        {
            IntPtr server = IntPtr.Zero;
            string strIpAddress = null;

            try
            {
                IntPtr ppSessionInfo = IntPtr.Zero;

                Int32 count = 0;
                Int32 retval = WTSEnumerateSessions(server, 0, 1, ref ppSessionInfo, ref count);
                Int32 dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));

                Int32 current = (int)ppSessionInfo;

                if (retval != 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)current, typeof(WTS_SESSION_INFO));
                        current += dataSize;

                        #region OTsSession
                        uint returned = 0; ;
                        TsSession oTsSession = new TsSession();
                        //IP address 
                        IntPtr addr = IntPtr.Zero;
                        if (WTSQuerySessionInformation(server, si.SessionID, WTS_INFO_CLASS.WTSClientAddress, out addr, out returned) == true)
                        {
                            _WTS_CLIENT_ADDRESS obj = new _WTS_CLIENT_ADDRESS();
                            obj = (_WTS_CLIENT_ADDRESS)Marshal.PtrToStructure(addr, obj.GetType());
                            oTsSession.IpAddress = obj.Address[2] + "." + obj.Address[3] + "." + obj.Address[4] + "." + obj.Address[5];
                        }
                        #endregion

                        if (si.SessionID == intSession)
                        {
                            strIpAddress = oTsSession.IpAddress;
                        }

                    }
                    WTSFreeMemory(ppSessionInfo);
                }
            }
            finally
            {
                CloseServer(server);
            }
            return strIpAddress;
        }
    }
}
