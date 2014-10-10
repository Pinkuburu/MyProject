using System;
using System.Collections.Generic;
//using System.Linq;
using System.IO;
using System.Threading;

using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.extensions.bytestreams;
using agsXMPP.protocol.x.data;
using agsXMPP.protocol.extensions.si;
using agsXMPP.protocol.extensions.featureneg;
using agsXMPP.protocol.extensions.filetransfer;

namespace CtalkRobot
{
    public class FileTransfer
    {
        // Add here your file transfer proxy, or disover it with service discovery
        // DONT USE THIS PROXY FOR PRODUCTION. THIS PROXY IS FOR RESTING ONLY. THIS PROXY IS ALSO NOT RUNNING ALL THE TIME
        // Install your own server with bytestream proxy or the external proxy65 module
        // const string PROXY = "proxy.ag-software.de";
        //const string PROXY = "proxy.netlab.cz";
        // const string PROXY = "proxy.jabber.org";        
        //const string PROXY = "v-jabberd.gradwell.net";
        const string PROXY = "ishow.xba.com.cn";

        /// <summary>
        /// SID of the filetransfer
        /// </summary>
        private string m_Sid;
        private Jid m_From;
        private Jid m_To;
        private DateTime m_startDateTime;

        private JEP65Socket _proxySocks5Socket;
        private JEP65Socket _p2pSocks5Socket;

        private XmppClientConnection m_XmppCon;

        private long m_lFileLength;
        private long m_bytesTransmitted = 0;
        private FileStream m_FileStream;
        private DateTime m_lastProgressUpdate;

        agsXMPP.protocol.extensions.filetransfer.File file;
        agsXMPP.protocol.extensions.si.SI si;
        IQ siIq;

        public FileTransfer(XmppClientConnection XmppCon, IQ iq)
        {
            Console.WriteLine("Accepting a file from " + iq.From.ToString());

            siIq = iq;
            si = iq.SelectSingleElement(typeof(agsXMPP.protocol.extensions.si.SI)) as agsXMPP.protocol.extensions.si.SI;
            // get SID for file transfer
            m_Sid = si.Id;
            m_From = iq.From;

            file = si.File;
            m_lFileLength = file.Size;

            m_XmppCon = XmppCon;

            XmppCon.OnIq += new IqHandler(XmppCon_OnIq);

            agsXMPP.protocol.extensions.featureneg.FeatureNeg fNeg = si.FeatureNeg;
            if (fNeg != null)
            {
                agsXMPP.protocol.x.data.Data data = fNeg.Data;
                if (data != null)
                {
                    agsXMPP.protocol.x.data.Field[] field = data.GetFields();
                    if (field.Length == 1)
                    {
                        Dictionary<string, string> methods = new Dictionary<string, string>();
                        foreach (agsXMPP.protocol.x.data.Option o in field[0].GetOptions())
                        {
                            string val = o.GetValue();
                            methods.Add(val, val);
                        }
                        if (methods.ContainsKey(agsXMPP.Uri.BYTESTREAMS))
                        {
                            // supports bytestream, so choose this option
                            agsXMPP.protocol.extensions.si.SIIq sIq = new agsXMPP.protocol.extensions.si.SIIq();
                            sIq.Id = siIq.Id;
                            sIq.To = siIq.From;
                            sIq.Type = IqType.result;

                            sIq.SI.Id = si.Id;
                            sIq.SI.FeatureNeg = new agsXMPP.protocol.extensions.featureneg.FeatureNeg();

                            Data xdata = new Data();
                            xdata.Type = XDataFormType.submit;
                            Field f = new Field();
                            //f.Type = FieldType.List_Single;
                            f.Var = "stream-method";
                            f.AddValue(agsXMPP.Uri.BYTESTREAMS);
                            xdata.AddField(f);
                            sIq.SI.FeatureNeg.Data = xdata;

                            m_XmppCon.Send(sIq);
                        }
                    }
                }
            }
        }

        public FileTransfer(XmppClientConnection XmppCon, Jid m_To)
        {
            m_XmppCon = XmppCon;
            SIIq iq = new SIIq();
            iq.To = m_To;
            iq.Type = IqType.set;

            m_lFileLength = new FileInfo(@"c:\\ping.txt").Length;

            agsXMPP.protocol.extensions.filetransfer.File file;
            file = new agsXMPP.protocol.extensions.filetransfer.File(Path.GetFileName(@"c:\\ping.txt"), m_lFileLength);
            //if (m_DescriptionChanged)
            //    file.Description = txtDescription.Text;
            file.Range = new Range();


            FeatureNeg fNeg = new FeatureNeg();

            Data data = new Data(XDataFormType.form);
            Field f = new Field(FieldType.List_Single);
            f.Var = "stream-method";
            f.AddOption().SetValue(agsXMPP.Uri.BYTESTREAMS);
            data.AddField(f);

            fNeg.Data = data;

            iq.SI.File = file;
            iq.SI.FeatureNeg = fNeg;
            iq.SI.Profile = agsXMPP.Uri.SI_FILE_TRANSFER;

            m_Sid = Guid.NewGuid().ToString();
            iq.SI.Id = m_Sid;

            m_XmppCon.IqGrabber.SendIq(iq, new IqCB(SiIqResult), null);
        }

        private void SiIqResult(object sender, IQ iq, object data)
        {
            // Parse the result of the form
            if (iq.Type == IqType.result)
            {
                // <iq xmlns="jabber:client" id="aab4a" to="gnauck@jabber.org/Psi" type="result"><si 
                // xmlns="http://jabber.org/protocol/si" id="s5b_405645b6f2b7c681"><feature 
                // xmlns="http://jabber.org/protocol/feature-neg"><x xmlns="jabber:x:data" type="submit"><field 
                // var="stream-
                // method"><value>http://jabber.org/protocol/bytestreams</value></field></x></feature></si></iq> 
                SI si = iq.SelectSingleElement(typeof(SI)) as SI;
                if (si != null)
                {
                    FeatureNeg fNeg = si.FeatureNeg;
                    if (SelectedByteStream(fNeg))
                    {
                        SendStreamHosts();
                    }
                }
            }
            else if (iq.Type == IqType.error)
            {
                agsXMPP.protocol.client.Error err = iq.Error;
                if (err != null)
                {
                    switch ((int)err.Code)
                    {
                        case 403:
                            Console.WriteLine("The file was rejected by the remote user", "Error");
                            break;
                    }
                }
            }

        }

        private bool SelectedByteStream(FeatureNeg fn)
        {
            if (fn != null)
            {
                Data data = fn.Data;
                if (data != null)
                {
                    foreach (Field field in data.GetFields())
                    {
                        if (field != null && field.Var == "stream-method")
                        {
                            if (field.GetValue() == agsXMPP.Uri.BYTESTREAMS)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        #region << Send Streamhosts >>
        private void SendStreamHosts()
        {
            /*
             Recv:
            <iq xmlns="jabber:client" from="gnauck@jabber.org/Psi" to="gnauck@ag-software.de/SharpIM" 
            type="set" id="aab6a"> <query xmlns="http://jabber.org/protocol/bytestreams" sid="s5b_405645b6f2b7c681" 
            mode="tcp"> <streamhost port="8010" jid="gnauck@jabber.org/Psi" host="192.168.74.142" />
            <streamhost port="7777" jid="proxy.ag-software.de" host="82.165.34.23">
                <proxy xmlns="http://affinix.com/jabber/stream" />
            </streamhost>
            <fast xmlns="http://affinix.com/jabber/stream" /> </query> </iq> 
            */

            ByteStreamIq bsIq = new ByteStreamIq();
            bsIq.To = m_To;
            bsIq.Type = IqType.set;

            bsIq.Query.Sid = m_Sid;

            string hostname = System.Net.Dns.GetHostName();

            System.Net.IPHostEntry iphe = System.Net.Dns.Resolve(hostname);

            for (int i = 0; i < iphe.AddressList.Length; i++)
            {
                Console.WriteLine("IP address: {0}", iphe.AddressList[i].ToString());
                //bsIq.Query.AddStreamHost(m_XmppCon.MyJID, iphe.AddressList[i].ToString(), 1000);
            }

            bsIq.Query.AddStreamHost(new Jid(PROXY), PROXY, 7777);

            _p2pSocks5Socket = new JEP65Socket();
            _p2pSocks5Socket.Initiator = m_XmppCon.MyJID;
            _p2pSocks5Socket.Target = m_To;
            _p2pSocks5Socket.SID = m_Sid;
            _p2pSocks5Socket.OnConnect += new ObjectHandler(_socket_OnConnect);
            _p2pSocks5Socket.OnDisconnect += new ObjectHandler(_socket_OnDisconnect);
            _p2pSocks5Socket.Listen(1000);


            m_XmppCon.IqGrabber.SendIq(bsIq, new IqCB(SendStreamHostsResult), null);
        }

        private void SendStreamHostsResult(object sender, IQ iq, object data)
        {
            //  <iq xmlns="jabber:client" type="result" to="gnauck@jabber.org/Psi" id="aab6a">
            //      <query xmlns="http://jabber.org/protocol/bytestreams">
            //          <streamhost-used jid="gnauck@jabber.org/Psi" />
            //      </query>
            //  </iq>          
            if (iq.Type == IqType.result)
            {
                ByteStream bs = iq.Query as ByteStream;
                if (bs != null)
                {
                    Jid sh = bs.StreamHostUsed.Jid;
                    if (sh != null & sh.Equals(m_XmppCon.MyJID, new agsXMPP.Collections.FullJidComparer()))
                    {
                        // direct connection
                        SendFile(null);
                    }
                    if (sh != null & sh.Equals(new Jid(PROXY), new agsXMPP.Collections.FullJidComparer()))
                    {
                        _p2pSocks5Socket = new JEP65Socket();
                        _p2pSocks5Socket.Address = PROXY;
                        _p2pSocks5Socket.Port = 7777;
                        _p2pSocks5Socket.Target = m_To;
                        _p2pSocks5Socket.Initiator = m_XmppCon.MyJID;
                        _p2pSocks5Socket.SID = m_Sid;
                        _p2pSocks5Socket.ConnectTimeout = 5000;
                        _p2pSocks5Socket.SyncConnect();

                        if (_p2pSocks5Socket.Connected)
                            ActivateBytestream(new Jid(PROXY));
                    }

                }
            }
        }
        #endregion

        #region << Activate ByteStream >>
        /*
            4.9 Activation of Bytestream

            In order for the bytestream to be used, it MUST first be activated by the StreamHost. If the StreamHost is the Initiator, this is straightforward and does not require any in-band protocol. However, if the StreamHost is a Proxy, the Initiator MUST send an in-band request to the StreamHost. This is done by sending an IQ-set to the Proxy, including an <activate/> element whose XML character data specifies the full JID of the Target.

            Example 17. Initiator Requests Activation of Bytestream

            <iq type='set' 
                from='initiator@host1/foo' 
                to='proxy.host3' 
                id='activate'>
              <query xmlns='http://jabber.org/protocol/bytestreams' sid='mySID'>
                <activate>target@host2/bar</activate>
              </query>
            </iq>
                

            Using this information, with the SID and from address on the packet, the Proxy is able to activate the stream by hashing the SID + Initiator JID + Target JID. This provides a reasonable level of trust that the activation request came from the Initiator.

            If the Proxy can fulfill the request, it MUST then respond to the Initiator with an IQ-result.

            Example 18. Proxy Informs Initiator of Activation

            <iq type='result' 
                from='proxy.host3' 
                to='initiator@host1/foo' 
                id='activate'/>   
         
        */

        private void ActivateBytestream(Jid streamHost)
        {
            ByteStreamIq bsIq = new ByteStreamIq();

            bsIq.To = streamHost;
            bsIq.Type = IqType.set;

            bsIq.Query.Sid = m_Sid;
            bsIq.Query.Activate = new Activate(m_To);

            m_XmppCon.IqGrabber.SendIq(bsIq, new IqCB(ActivateBytestreamResult), null);
        }

        private void ActivateBytestreamResult(object sender, IQ iq, object dat)
        {
            if (iq.Type == IqType.result)
            {
                SendFile(null);
            }
        }
        #endregion

        void _socket_OnDisconnect(object sender)
        {

        }

        void _socket_OnConnect(object sender)
        {

        }

        /// <summary>
        /// Sends the file Async
        /// </summary>
        /// <param name="ar"></param>
        private void SendFile(IAsyncResult ar)
        {
            const int BUFFERSIZE = 1024;
            byte[] buffer = new byte[BUFFERSIZE];
            FileStream fs;
            // AsyncResult is null when we call this function the 1st time
            if (ar == null)
            {
                m_startDateTime = DateTime.Now;
                fs = new FileStream(@"c:\ping.txt", FileMode.Open);
            }
            else
            {

                if (_p2pSocks5Socket.Socket.Connected)
                    _p2pSocks5Socket.Socket.EndReceive(ar);

                fs = ar.AsyncState as FileStream;

                // Windows Forms are not Thread Safe, we need to invoke this :(
                // We're not in the UI thread, so we need to call BeginInvoke
                // to udate the progress bar
                //TimeSpan ts = DateTime.Now - m_lastProgressUpdate;
                //if (ts.Seconds >= 1)
                //{
                //    BeginInvoke(new ObjectHandler(UpdateProgress), new object[] { this });
                //}
            }

            int len = fs.Read(buffer, 0, BUFFERSIZE);
            m_bytesTransmitted += len;

            if (len > 0)
            {
                _p2pSocks5Socket.Socket.BeginSend(buffer, 0, len, System.Net.Sockets.SocketFlags.None, SendFile, fs);
            }
            else
            {
                // Update Pogress when finished
                //BeginInvoke(new ObjectHandler(UpdateProgress), new object[] { this });
                fs.Close();
                fs.Dispose();
                if (_p2pSocks5Socket != null && _p2pSocks5Socket.Connected)
                    _p2pSocks5Socket.Disconnect();
            }
        }  

        private void XmppCon_OnIq(object sender, agsXMPP.protocol.client.IQ iq)
        {
            if (iq.Query != null && iq.Query.GetType() == typeof(agsXMPP.protocol.extensions.bytestreams.ByteStream))
            {
                agsXMPP.protocol.extensions.bytestreams.ByteStream bs = iq.Query as agsXMPP.protocol.extensions.bytestreams.ByteStream;
                // check is this is for the correct file
                if (bs.Sid == m_Sid)
                {
                    Thread t = new Thread(
                        delegate() { HandleStreamHost(bs, iq); }
                    );
                    t.Name = "LoopStreamHosts";
                    t.Start();
                }

            }
        }

        private void HandleStreamHost(ByteStream bs, IQ iq)
        //private void HandleStreamHost(object obj)
        {
            //IQ iq = obj as IQ;
            //ByteStream bs = iq.Query as agsXMPP.protocol.extensions.bytestreams.ByteStream;;
            //ByteStream bs = iq.Query as ByteStream;
            if (bs != null)
            {
                _proxySocks5Socket = new JEP65Socket();
                _proxySocks5Socket.OnConnect += new ObjectHandler(m_s5Sock_OnConnect);
                _proxySocks5Socket.OnReceive += new agsXMPP.net.BaseSocket.OnSocketDataHandler(m_s5Sock_OnReceive);
                _proxySocks5Socket.OnDisconnect += new ObjectHandler(m_s5Sock_OnDisconnect);

                StreamHost[] streamhosts = bs.GetStreamHosts();
                //Scroll through the possible sock5 servers and try to connect
                //foreach (StreamHost sh in streamhosts)
                //this is done back to front in order to make sure that the proxy JID is chosen first
                //this is necessary as at this stage the application only knows how to connect to a 
                //socks 5 proxy.

                foreach (StreamHost sHost in streamhosts)
                {
                    if (sHost.Host != null)
                    {
                        _proxySocks5Socket.Address = sHost.Host;
                        _proxySocks5Socket.Port = sHost.Port;
                        _proxySocks5Socket.Target = m_XmppCon.MyJID;
                        _proxySocks5Socket.Initiator = m_From;
                        _proxySocks5Socket.SID = m_Sid;
                        _proxySocks5Socket.ConnectTimeout = 5000;
                        _proxySocks5Socket.SyncConnect();
                        if (_proxySocks5Socket.Connected)
                        {
                            SendStreamHostUsedResponse(sHost, iq);
                            break;
                        }
                    }
                }

            }
        }

        private void m_s5Sock_OnDisconnect(object sender)
        {
            m_FileStream.Close();
            m_FileStream.Dispose();

            if (m_bytesTransmitted == m_lFileLength)
            {
                // completed
                // tslTransmitted.Text = "completed";
                Console.WriteLine("Finished file transfer");
            }
            else
            {
                // not complete, some error occured or somebody canceled the transfer

            }
        }

        void m_s5Sock_OnReceive(object sender, byte[] data, int count)
        {
            m_FileStream.Write(data, 0, count);

            m_bytesTransmitted += count;


            // Windows Forms are not Thread Safe, we need to invoke this :(
            // We're not in the UI thread, so we need to call BeginInvoke
            // to udate the progress bar	
            /*TimeSpan ts = DateTime.Now - m_lastProgressUpdate;
            if (ts.Seconds >= 1)
            {
                BeginInvoke(new ObjectHandler(UpdateProgress), new object[] { sender });
            }*/

            TimeSpan ts = DateTime.Now - m_lastProgressUpdate;
            if (ts.Seconds >= 1)
            {
                m_lastProgressUpdate = DateTime.Now;
                double percent = (double)m_bytesTransmitted / (double)m_lFileLength * 100;
                Console.WriteLine("Percent: " + percent.ToString());
            }
            //Console.WriteLine("Got data");
        }

        void m_s5Sock_OnConnect(object sender)
        {
            m_startDateTime = DateTime.Now;

            // string path = Util.AppPath + @"\Received Files";
            string path = @"C:\";
            System.IO.Directory.CreateDirectory(path);

            m_FileStream = new FileStream(Path.Combine(path, file.Name), FileMode.Create);

            //throw new Exception("The method or operation is not implemented.");
        }

        private void SendStreamHostUsedResponse(StreamHost sh, IQ iq)
        {
            ByteStreamIq bsIQ = new ByteStreamIq(IqType.result, m_From);
            bsIQ.Id = iq.Id;

            bsIQ.Query.StreamHostUsed = new StreamHostUsed(sh.Jid);
            m_XmppCon.Send(bsIQ);

        }
    }
}
