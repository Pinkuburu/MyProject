//============================================================
// File: MailSender.cs
//C#-MailSender�ʼ��������Դ����(֧��ESMTP,����,���˽���)
// 
// ֧��ESMTP, �฽��.�������ռ��Ĵ��������޸Ķ�����
//============================================================
/*
 ʹ��:
			MailSender ms = new MailSender ();
			ms.From = "wangyw@wj.wonjoint.com.cn";
			ms.To = "wangyw@wj.wonjoint.com.cn";
			ms.Subject = "Subject";
			ms.Body = "body text";
			ms.UserName = "040184";  // ��ô�ܸ�������
			ms.IsUserCode=false;
			ms.Password = "436209"; // ��ô�ܸ�������
			ms.Server = "10.230.2.37";
			ms.Attachments.Add (new MailSender.AttachmentInfo (@"D:\test.txt"));
			ms.SendMail ();
*/
namespace msgAlert
{
	using System;
	using System.Collections;
	using System.Net.Sockets;
	using System.IO;
	using System.Text;

	/// <summary>
	/// Mail ������
	/// </summary>
	public class MailSender
	{
		private TcpClient tcp=null;
		private string server = ""; //�ʼ�������
		private int port = 25;      //�ʼ��˿�
		private string userName = "";//��½�û���
		private string password = "";//��½����
		private bool isAuthLogin = false;//�û����������Ƿ���Ҫ64λ����

		private string from = "";  //����������
		private string fromName = "";//����������

		private string to = "";//���͵�������,֧�ֶ���������֮����","����
		private string toName = "";//���͵�������
		private string subject = "";//����
		private string body = "";//����
		private string htmlBody = "";//html��ʽ����
		private bool isHtml = false;//�Ƿ���html��ʽ
		private string languageEncoding = "GB2312";//����
		private string encoding = "8bit";//����
		private int priority = 3;//���ȼ�(1,�ߣ�3����ͨ��5����)
		//		private ArrayList attachments = new ArrayList (); //����
		private IList attachments = new ArrayList (); //����
		
		#region ����
		/// <summary>
		/// SMTP����������
		/// </summary>
		public string Server 
		{
			get 
			{
				return server;
			}
			set
			{
				if (value != server) 
					server = value; 
			}
		} 
		

		/// <summary>
		/// SMTP�������˿� [Ĭ��Ϊ25]
		/// </summary>
		public int Port 
		{
			get 
			{
				return port; 
			}
			set 
			{
				if (value != port) 
					port = value;
			}
		} 
		

		/// <summary>
		/// �û��� [�����Ҫ�����֤�Ļ�]
		/// </summary>
		public string UserName 
		{
			get { return userName; }
			set { if (value != userName) userName = value; }
		} 

		/// <summary>
		/// ���� [�����Ҫ�����֤�Ļ�]
		/// </summary>
		public string Password 
		{
			get { return password; }
			set { if (value != password) password = value; }
		}

		/// <summary>
		/// �����˵�ַ
		/// </summary>
		public string From 
		{
			get { return from; }
			set { if (value != from) from = value;}
		} 
		 

		/// <summary>
		/// �ռ��˵�ַ
		/// </summary>
		public string To 
		{
			get { return to; }
			set { if (value != to) to = value;}
		} 
		

		/// <summary>
		/// ����������
		/// </summary>
		public string FromName 
		{
			get { return fromName; }
			set { if (value != fromName) fromName = value; }
		}
		

		/// <summary>
		/// �ռ�������
		/// </summary>
		public string ToName 
		{
			get { return toName; }
			set { if (value != toName) toName = value; }
		} 
		

		/// <summary>
		/// �ʼ�������
		/// </summary>
		public string Subject 
		{
			get { return subject; }
			set { if (value != subject) subject = value; }
		} 
		

		/// <summary>
		/// �ʼ�����
		/// </summary>
		public string Body 
		{
			get { return body; }
			set { if (value != body) body = value; }
		} 
		

		/// <summary>
		/// ���ı���ʽ���ʼ�����
		/// </summary>
		public string HtmlBody 
		{
			get { return htmlBody; }
			set { if (value != htmlBody) htmlBody = value; }
		}
		
		/// <summary>
		/// �Ƿ���html��ʽ���ʼ�
		/// </summary>
		public bool IsHtml 
		{
			get { return isHtml; }
			set { if (value != isHtml) isHtml = value; }
		}

		public bool IsAuthLogin 
		{
			get { return isAuthLogin; }
			set { if (value != isAuthLogin) isAuthLogin = value; }
		}
		

		/// <summary>
		/// ���Ա��� [Ĭ��ΪGB2312]
		/// </summary>
		public string LanguageEncoding 
		{
			get { return languageEncoding; }
			set { if (value != languageEncoding) languageEncoding = value; }
		}
		
		/// <summary>
		/// �ʼ����� [Ĭ��Ϊ8bit]
		/// </summary>
		public string MailEncoding 
		{
			get { return encoding; }
			set { if (value != encoding) encoding = value; }
		} 
		

		/// <summary>
		/// �ʼ����ȼ� [Ĭ��Ϊ3]
		/// </summary>
		public int Priority 
		{
			get { return priority; }
			set { if (value != priority) priority = value; }
		} 
		
		/// <summary>
		/// ���� [AttachmentInfo]
		/// </summary>
		public IList Attachments 
		{
			get { return attachments; }
			set { if (value != attachments) attachments = value; }
		}
		
		#endregion

		/// <summary>
		/// �����ʼ�
		/// </summary>
		public void SendMail ()
		{
			// ����TcpClient���� ����������
			//			TcpClient tcp = null;
			tcpConnServer();

			ReadString (tcp.GetStream());//��ȡ������Ϣ

			// ��ʼ���з�������֤
			// ���״̬����250���ʾ�����ɹ�
			if (!Command (tcp.GetStream(), "EHLO localhost", "250"))
				throw new Exception ("��½�׶�ʧ��");
			
			
			if (isAuthLogin)// ��Ҫ�����֤	
			{
				AuthLogin();	
			}

			// ׼������
			
			string[] sto=to.Split(new char[]{','});
			for(int j=0;j<sto.Length;j++)
			{
				if(sto[j].ToString()!=null&&sto[j].ToString()!="")
				{
					WriteString (tcp.GetStream(), "mail From:" + from);
					WriteString (tcp.GetStream(), "RCPT TO:" + sto[j].ToString());
					//								WriteString (tcp.GetStream(), "rcpt to: " + to);

					WriteString (tcp.GetStream(), "DATA");

					// �����ʼ�ͷ
					WriteString (tcp.GetStream(), "Date: " + DateTime.Now); // ʱ��
					WriteString (tcp.GetStream(), "From: " + fromName + "<" + from + ">"); // ������
					WriteString (tcp.GetStream(), "Subject: " + subject); // ����
					WriteString (tcp.GetStream(), "To:" + toName + "<" + to + ">"); // �ռ���
					//					WriteString (tcp.GetStream(), "To:" + toName + "<" + sto[j].ToString() + ">"); // �ռ���

					//�ʼ���ʽ
					WriteString (tcp.GetStream(), "Content-Type: multipart/mixed; boundary=\"unique-boundary-1\"");
					WriteString (tcp.GetStream(), "Reply-To:" + from); // �ظ���ַ
					WriteString (tcp.GetStream(), "X-Priority:" + priority); // ���ȼ�
					WriteString (tcp.GetStream(), "MIME-Version:1.0"); // MIME�汾

					// ����ID,����
					//   WriteString (tcp.GetStream(), "Message-Id: " + DateTime.Now.ToFileTime() + "@security.com");
					WriteString (tcp.GetStream(), "Content-Transfer-Encoding:" + encoding); // ���ݱ���
					WriteString (tcp.GetStream(), "X-Mailer:JcPersonal.Utility.MailSender"); // �ʼ�������
					WriteString (tcp.GetStream(), "");

					WriteString (tcp.GetStream(), ToBase64 ("This is a multi-part message in MIME format."));
					WriteString (tcp.GetStream(), "");

					// �Ӵ˴���ʼ���зָ�����
					WriteString (tcp.GetStream(), "--unique-boundary-1");

					// �ڴ˴�����ڶ����ָ���
					WriteString (tcp.GetStream(), "Content-Type: multipart/alternative;Boundary=\"unique-boundary-2\"");
					WriteString (tcp.GetStream(), "");

					if(!isHtml)
					{
						// �ı���Ϣ
						WriteString (tcp.GetStream(), "--unique-boundary-2");
						WriteString (tcp.GetStream(), "Content-Type: text/plain;charset=" + languageEncoding);
						WriteString (tcp.GetStream(), "Content-Transfer-Encoding:" + encoding);
						WriteString (tcp.GetStream(), "");
						WriteString (tcp.GetStream(), body);
						WriteString (tcp.GetStream(), "");//һ������д��֮���д�����Ϣ���ֶ�
						WriteString (tcp.GetStream(), "--unique-boundary-2--");//�ָ����Ľ������ţ�β�ͺ������--
						WriteString (tcp.GetStream(), "");
					}
					else
					{
						//html��Ϣ
						WriteString (tcp.GetStream(), "--unique-boundary-2");
						WriteString (tcp.GetStream(), "Content-Type: text/html;charset=" + languageEncoding);
						WriteString (tcp.GetStream(), "Content-Transfer-Encoding:" + encoding);
						WriteString (tcp.GetStream(), "");
						WriteString (tcp.GetStream(), htmlBody);
						WriteString (tcp.GetStream(), "");
						WriteString (tcp.GetStream(), "--unique-boundary-2--");//�ָ����Ľ������ţ�β�ͺ������--
						WriteString (tcp.GetStream(), "");
					}

					// ���͸���
					// ���ļ��б���ѭ��
					for (int i = 0; i < attachments.Count; i++)
					{
						WriteString (tcp.GetStream(), "--unique-boundary-1"); // �ʼ����ݷָ���
						WriteString (tcp.GetStream(), "Content-Type: application/octet-stream;name=\"" + ((AttachmentInfo)attachments[i]).FileName + "\""); // �ļ���ʽ
						WriteString (tcp.GetStream(), "Content-Transfer-Encoding: base64"); // ���ݵı���
						WriteString (tcp.GetStream(), "Content-Disposition:attachment;filename=\"" + ((AttachmentInfo)attachments[i]).FileName + "\""); // �ļ���
						WriteString (tcp.GetStream(), "");
						WriteString (tcp.GetStream(), ((AttachmentInfo)attachments[i]).Bytes); // д���ļ�������
						WriteString (tcp.GetStream(), "");
					}
					Command (tcp.GetStream(), ".", "250"); // ���д���ˣ�����"."
				}
			}
			Command (tcp.GetStream(), "QUIT", "250"); // ���д���ˣ�����"."
			// �ر�����
			tcp.Close ();
		}
		
		protected void tcpConnServer()
		{
			try
			{
				tcp = new TcpClient (server, port);
			}
			catch (Exception)
			{
				throw new Exception ("�޷����ӷ�����");
			}
		}

		protected void AuthLogin()  //����û�����Ϊ��,�����ESMTP�����֤,��ʱֻ�ܷ����������ʼ�,�����͵�126.com���ʼ�ֻ������126.com.�������.
		{
			if (!Command (tcp.GetStream(),"AUTH LOGIN","334"))
				throw new Exception ("�����֤�׶�ʧ��1");
			
			string nameB64 = ToBase64 (userName); // �˴���usernameת��ΪBase64��
			if (!Command (tcp.GetStream(), nameB64, "334"))
				throw new Exception ("�����֤�׶�ʧ��2");
			string passB64 = ToBase64 (password); // �˴���passwordת��ΪBase64��
			if (!Command (tcp.GetStream(), passB64, "235"))
				throw new Exception ("�����֤�׶�ʧ��3");
		}

		/// <summary>
		/// ������д���ַ�
		/// </summary>
		/// <param name="netStream">����TcpClient����</param>
		/// <param name="str">д����ַ�</param>
		protected void WriteString (NetworkStream netStream, string str)
		{
			str = str + "\r\n"; // ���뻻�з�

			// ��������ת��Ϊbyte[]
			byte[] bWrite = Encoding.GetEncoding(languageEncoding).GetBytes(str.ToCharArray());

			// ����ÿ��д������ݴ�С�������Ƶģ���ô���ǽ�ÿ��д������ݳ��ȶ��ڣ������ֽڣ�һ������ȳ����ˣ������ͷֲ�д�롣
			int start=0;
			int length=bWrite.Length;
			int page=0;
			int size=75;
			int count=size;
			try
			{
				if (length>75)
				{
					// ���ݷ�ҳ
					if ((length/size)*size<length)
						page=length/size+1;
					else
						page=length/size;
					for (int i=0;i<page;i++)
					{
						start=i*size;
						if (i==page-1)
							count=length-(i*size);
						netStream.Write(bWrite,start,count);// ������д�뵽��������
					}
				}
				else
					netStream.Write(bWrite,0,bWrite.Length);
			}
			catch(Exception)
			{
				throw new Exception ("д���ݴ���:"+str);
			}
		}

		/// <summary>
		/// �����ж�ȡ�ַ�
		/// </summary>
		/// <param name="netStream">����TcpClient����</param>
		/// <returns>��ȡ���ַ�</returns>
		protected string ReadString (NetworkStream netStream)
		{
			string sp = null;
			byte[] by = new byte[1024];
			int size = netStream.Read(by,0,by.Length);// ��ȡ������
			if (size > 0)
			{
				sp = Encoding.Default.GetString(by);// ת��ΪString
			}
			return sp;
		}

		/// <summary>
		/// ��������жϷ�����Ϣ�Ƿ���ȷ
		/// </summary>
		/// <param name="netStream">����TcpClient����</param>
		/// <param name="command">����</param>
		/// <param name="state">��ȷ��״̬��</param>
		/// <returns>�Ƿ���ȷ</returns>
		protected bool Command (NetworkStream netStream, string command, string state)
		{
			string sp=null;
			bool success=false;
			try
			{
				WriteString (netStream, command);// д������
				sp = ReadString (netStream);// ���ܷ�����Ϣ
				if (sp.IndexOf(state) != -1)// �ж�״̬���Ƿ���ȷ
					success=true;
			}
			catch(Exception)
			{
				throw new Exception ("����!��������:"+command+"��Ӧ�÷���״̬:"+state+"�������ʼ�������ΪEsmtp");
			}
			return success;
		}

		/// <summary>
		/// �ַ�������ΪBase64
		/// </summary>
		/// <param name="str">�ַ���</param>
		/// <returns>Base64������ַ���</returns>
		protected string ToBase64 (string str)
		{
			try
			{
				byte[] by = Encoding.Default.GetBytes (str.ToCharArray());
				str = Convert.ToBase64String (by);
			}
			catch(Exception)
			{
				throw new Exception ("64�������");
			}
			return str;
		}

		/// <summary>
		/// ������Ϣ
		/// </summary>
		public struct AttachmentInfo
		{
			/// <summary>
			/// �������ļ��� [�������·�������Զ�ת��Ϊ�ļ���]
			/// </summary>
			public string FileName 
			{
				get { return fileName; }
				set { fileName = Path.GetFileName(value); }
			} private string fileName;

			/// <summary>
			/// ���������� [�ɾ�Base64������ֽ����]
			/// </summary>
			public string Bytes 
			{
				get { return bytes; }
				set { if (value != bytes) bytes = value; }
			} private string bytes;

			/// <summary>
			/// �����ж�ȡ�������ݲ�����
			/// </summary>
			/// <param name="ifileName">�������ļ���</param>
			/// <param name="stream">��</param>
			public AttachmentInfo (string ifileName, Stream stream)
			{
				fileName = Path.GetFileName (ifileName);
				byte[] by = new byte [stream.Length];
				stream.Read (by,0,(int)stream.Length); // ��ȡ�ļ�����
				//��ʽת��
				bytes = Convert.ToBase64String (by); // ת��Ϊbase64����
			}

			/// <summary>
			/// ���ո������ֽڹ��츽��
			/// </summary>
			/// <param name="ifileName">�������ļ���</param>
			/// <param name="ibytes">���������� [�ֽ�]</param>
			public AttachmentInfo (string ifileName, byte[] ibytes)
			{
				fileName = Path.GetFileName (ifileName);
				bytes = Convert.ToBase64String (ibytes); // ת��Ϊbase64����
			}

			/// <summary>
			/// ���ļ����벢����
			/// </summary>
			/// <param name="path"></param>
			public AttachmentInfo (string path)
			{
				fileName = Path.GetFileName (path);
				FileStream file = new FileStream (path, FileMode.Open);
				byte[] by = new byte [file.Length];
				file.Read (by,0,(int)file.Length); // ��ȡ�ļ�����
				//��ʽת��
				bytes = Convert.ToBase64String (by); // ת��Ϊbase64����
				file.Close ();
			}
		}
	}
}