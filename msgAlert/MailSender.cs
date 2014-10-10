//============================================================
// File: MailSender.cs
//C#-MailSender邮件发送组件源代码(支持ESMTP,附件,多人接收)
// 
// 支持ESMTP, 多附件.在网上收集的代码整理修改而来。
//============================================================
/*
 使用:
			MailSender ms = new MailSender ();
			ms.From = "wangyw@wj.wonjoint.com.cn";
			ms.To = "wangyw@wj.wonjoint.com.cn";
			ms.Subject = "Subject";
			ms.Body = "body text";
			ms.UserName = "040184";  // 怎么能告诉你呢
			ms.IsUserCode=false;
			ms.Password = "436209"; // 怎么能告诉你呢
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
	/// Mail 发送器
	/// </summary>
	public class MailSender
	{
		private TcpClient tcp=null;
		private string server = ""; //邮件服务器
		private int port = 25;      //邮件端口
		private string userName = "";//登陆用户名
		private string password = "";//登陆密码
		private bool isAuthLogin = false;//用户名和密码是否需要64位编码

		private string from = "";  //发送者邮箱
		private string fromName = "";//发送者姓名

		private string to = "";//发送到的邮箱,支持多邮箱邮箱之间用","隔开
		private string toName = "";//发送到的姓名
		private string subject = "";//主题
		private string body = "";//正文
		private string htmlBody = "";//html格式正文
		private bool isHtml = false;//是否用html格式
		private string languageEncoding = "GB2312";//语言
		private string encoding = "8bit";//编码
		private int priority = 3;//优先级(1,高；3，普通；5，低)
		//		private ArrayList attachments = new ArrayList (); //附件
		private IList attachments = new ArrayList (); //附件
		
		#region 属性
		/// <summary>
		/// SMTP服务器域名
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
		/// SMTP服务器端口 [默认为25]
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
		/// 用户名 [如果需要身份验证的话]
		/// </summary>
		public string UserName 
		{
			get { return userName; }
			set { if (value != userName) userName = value; }
		} 

		/// <summary>
		/// 密码 [如果需要身份验证的话]
		/// </summary>
		public string Password 
		{
			get { return password; }
			set { if (value != password) password = value; }
		}

		/// <summary>
		/// 发件人地址
		/// </summary>
		public string From 
		{
			get { return from; }
			set { if (value != from) from = value;}
		} 
		 

		/// <summary>
		/// 收件人地址
		/// </summary>
		public string To 
		{
			get { return to; }
			set { if (value != to) to = value;}
		} 
		

		/// <summary>
		/// 发件人姓名
		/// </summary>
		public string FromName 
		{
			get { return fromName; }
			set { if (value != fromName) fromName = value; }
		}
		

		/// <summary>
		/// 收件人姓名
		/// </summary>
		public string ToName 
		{
			get { return toName; }
			set { if (value != toName) toName = value; }
		} 
		

		/// <summary>
		/// 邮件的主题
		/// </summary>
		public string Subject 
		{
			get { return subject; }
			set { if (value != subject) subject = value; }
		} 
		

		/// <summary>
		/// 邮件正文
		/// </summary>
		public string Body 
		{
			get { return body; }
			set { if (value != body) body = value; }
		} 
		

		/// <summary>
		/// 超文本格式的邮件正文
		/// </summary>
		public string HtmlBody 
		{
			get { return htmlBody; }
			set { if (value != htmlBody) htmlBody = value; }
		}
		
		/// <summary>
		/// 是否是html格式的邮件
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
		/// 语言编码 [默认为GB2312]
		/// </summary>
		public string LanguageEncoding 
		{
			get { return languageEncoding; }
			set { if (value != languageEncoding) languageEncoding = value; }
		}
		
		/// <summary>
		/// 邮件编码 [默认为8bit]
		/// </summary>
		public string MailEncoding 
		{
			get { return encoding; }
			set { if (value != encoding) encoding = value; }
		} 
		

		/// <summary>
		/// 邮件优先级 [默认为3]
		/// </summary>
		public int Priority 
		{
			get { return priority; }
			set { if (value != priority) priority = value; }
		} 
		
		/// <summary>
		/// 附件 [AttachmentInfo]
		/// </summary>
		public IList Attachments 
		{
			get { return attachments; }
			set { if (value != attachments) attachments = value; }
		}
		
		#endregion

		/// <summary>
		/// 发送邮件
		/// </summary>
		public void SendMail ()
		{
			// 创建TcpClient对象， 并建立连接
			//			TcpClient tcp = null;
			tcpConnServer();

			ReadString (tcp.GetStream());//获取连接信息

			// 开始进行服务器认证
			// 如果状态码是250则表示操作成功
			if (!Command (tcp.GetStream(), "EHLO localhost", "250"))
				throw new Exception ("登陆阶段失败");
			
			
			if (isAuthLogin)// 需要身份验证	
			{
				AuthLogin();	
			}

			// 准备发送
			
			string[] sto=to.Split(new char[]{','});
			for(int j=0;j<sto.Length;j++)
			{
				if(sto[j].ToString()!=null&&sto[j].ToString()!="")
				{
					WriteString (tcp.GetStream(), "mail From:" + from);
					WriteString (tcp.GetStream(), "RCPT TO:" + sto[j].ToString());
					//								WriteString (tcp.GetStream(), "rcpt to: " + to);

					WriteString (tcp.GetStream(), "DATA");

					// 发送邮件头
					WriteString (tcp.GetStream(), "Date: " + DateTime.Now); // 时间
					WriteString (tcp.GetStream(), "From: " + fromName + "<" + from + ">"); // 发件人
					WriteString (tcp.GetStream(), "Subject: " + subject); // 主题
					WriteString (tcp.GetStream(), "To:" + toName + "<" + to + ">"); // 收件人
					//					WriteString (tcp.GetStream(), "To:" + toName + "<" + sto[j].ToString() + ">"); // 收件人

					//邮件格式
					WriteString (tcp.GetStream(), "Content-Type: multipart/mixed; boundary=\"unique-boundary-1\"");
					WriteString (tcp.GetStream(), "Reply-To:" + from); // 回复地址
					WriteString (tcp.GetStream(), "X-Priority:" + priority); // 优先级
					WriteString (tcp.GetStream(), "MIME-Version:1.0"); // MIME版本

					// 数据ID,随意
					//   WriteString (tcp.GetStream(), "Message-Id: " + DateTime.Now.ToFileTime() + "@security.com");
					WriteString (tcp.GetStream(), "Content-Transfer-Encoding:" + encoding); // 内容编码
					WriteString (tcp.GetStream(), "X-Mailer:JcPersonal.Utility.MailSender"); // 邮件发送者
					WriteString (tcp.GetStream(), "");

					WriteString (tcp.GetStream(), ToBase64 ("This is a multi-part message in MIME format."));
					WriteString (tcp.GetStream(), "");

					// 从此处开始进行分隔输入
					WriteString (tcp.GetStream(), "--unique-boundary-1");

					// 在此处定义第二个分隔符
					WriteString (tcp.GetStream(), "Content-Type: multipart/alternative;Boundary=\"unique-boundary-2\"");
					WriteString (tcp.GetStream(), "");

					if(!isHtml)
					{
						// 文本信息
						WriteString (tcp.GetStream(), "--unique-boundary-2");
						WriteString (tcp.GetStream(), "Content-Type: text/plain;charset=" + languageEncoding);
						WriteString (tcp.GetStream(), "Content-Transfer-Encoding:" + encoding);
						WriteString (tcp.GetStream(), "");
						WriteString (tcp.GetStream(), body);
						WriteString (tcp.GetStream(), "");//一个部分写完之后就写如空信息，分段
						WriteString (tcp.GetStream(), "--unique-boundary-2--");//分隔符的结束符号，尾巴后面多了--
						WriteString (tcp.GetStream(), "");
					}
					else
					{
						//html信息
						WriteString (tcp.GetStream(), "--unique-boundary-2");
						WriteString (tcp.GetStream(), "Content-Type: text/html;charset=" + languageEncoding);
						WriteString (tcp.GetStream(), "Content-Transfer-Encoding:" + encoding);
						WriteString (tcp.GetStream(), "");
						WriteString (tcp.GetStream(), htmlBody);
						WriteString (tcp.GetStream(), "");
						WriteString (tcp.GetStream(), "--unique-boundary-2--");//分隔符的结束符号，尾巴后面多了--
						WriteString (tcp.GetStream(), "");
					}

					// 发送附件
					// 对文件列表做循环
					for (int i = 0; i < attachments.Count; i++)
					{
						WriteString (tcp.GetStream(), "--unique-boundary-1"); // 邮件内容分隔符
						WriteString (tcp.GetStream(), "Content-Type: application/octet-stream;name=\"" + ((AttachmentInfo)attachments[i]).FileName + "\""); // 文件格式
						WriteString (tcp.GetStream(), "Content-Transfer-Encoding: base64"); // 内容的编码
						WriteString (tcp.GetStream(), "Content-Disposition:attachment;filename=\"" + ((AttachmentInfo)attachments[i]).FileName + "\""); // 文件名
						WriteString (tcp.GetStream(), "");
						WriteString (tcp.GetStream(), ((AttachmentInfo)attachments[i]).Bytes); // 写入文件的内容
						WriteString (tcp.GetStream(), "");
					}
					Command (tcp.GetStream(), ".", "250"); // 最后写完了，输入"."
				}
			}
			Command (tcp.GetStream(), "QUIT", "250"); // 最后写完了，输入"."
			// 关闭连接
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
				throw new Exception ("无法连接服务器");
			}
		}

		protected void AuthLogin()  //如果用户名不为空,则进行ESMTP身份验证,此时只能发送往本地邮件,例发送到126.com的邮件只能来自126.com.否则出错.
		{
			if (!Command (tcp.GetStream(),"AUTH LOGIN","334"))
				throw new Exception ("身份验证阶段失败1");
			
			string nameB64 = ToBase64 (userName); // 此处将username转换为Base64码
			if (!Command (tcp.GetStream(), nameB64, "334"))
				throw new Exception ("身份验证阶段失败2");
			string passB64 = ToBase64 (password); // 此处将password转换为Base64码
			if (!Command (tcp.GetStream(), passB64, "235"))
				throw new Exception ("身份验证阶段失败3");
		}

		/// <summary>
		/// 向流中写入字符
		/// </summary>
		/// <param name="netStream">来自TcpClient的流</param>
		/// <param name="str">写入的字符</param>
		protected void WriteString (NetworkStream netStream, string str)
		{
			str = str + "\r\n"; // 加入换行符

			// 将命令行转化为byte[]
			byte[] bWrite = Encoding.GetEncoding(languageEncoding).GetBytes(str.ToCharArray());

			// 由于每次写入的数据大小是有限制的，那么我们将每次写入的数据长度定在７５个字节，一旦命令长度超过了７５，就分步写入。
			int start=0;
			int length=bWrite.Length;
			int page=0;
			int size=75;
			int count=size;
			try
			{
				if (length>75)
				{
					// 数据分页
					if ((length/size)*size<length)
						page=length/size+1;
					else
						page=length/size;
					for (int i=0;i<page;i++)
					{
						start=i*size;
						if (i==page-1)
							count=length-(i*size);
						netStream.Write(bWrite,start,count);// 将数据写入到服务器上
					}
				}
				else
					netStream.Write(bWrite,0,bWrite.Length);
			}
			catch(Exception)
			{
				throw new Exception ("写数据错误:"+str);
			}
		}

		/// <summary>
		/// 从流中读取字符
		/// </summary>
		/// <param name="netStream">来自TcpClient的流</param>
		/// <returns>读取的字符</returns>
		protected string ReadString (NetworkStream netStream)
		{
			string sp = null;
			byte[] by = new byte[1024];
			int size = netStream.Read(by,0,by.Length);// 读取数据流
			if (size > 0)
			{
				sp = Encoding.Default.GetString(by);// 转化为String
			}
			return sp;
		}

		/// <summary>
		/// 发出命令并判断返回信息是否正确
		/// </summary>
		/// <param name="netStream">来自TcpClient的流</param>
		/// <param name="command">命令</param>
		/// <param name="state">正确的状态码</param>
		/// <returns>是否正确</returns>
		protected bool Command (NetworkStream netStream, string command, string state)
		{
			string sp=null;
			bool success=false;
			try
			{
				WriteString (netStream, command);// 写入命令
				sp = ReadString (netStream);// 接受返回信息
				if (sp.IndexOf(state) != -1)// 判断状态码是否正确
					success=true;
			}
			catch(Exception)
			{
				throw new Exception ("错误!发送命令:"+command+"。应该返回状态:"+state+"。可能邮件服务器为Esmtp");
			}
			return success;
		}

		/// <summary>
		/// 字符串编码为Base64
		/// </summary>
		/// <param name="str">字符串</param>
		/// <returns>Base64编码的字符串</returns>
		protected string ToBase64 (string str)
		{
			try
			{
				byte[] by = Encoding.Default.GetBytes (str.ToCharArray());
				str = Convert.ToBase64String (by);
			}
			catch(Exception)
			{
				throw new Exception ("64编码错误");
			}
			return str;
		}

		/// <summary>
		/// 附件信息
		/// </summary>
		public struct AttachmentInfo
		{
			/// <summary>
			/// 附件的文件名 [如果输入路径，则自动转换为文件名]
			/// </summary>
			public string FileName 
			{
				get { return fileName; }
				set { fileName = Path.GetFileName(value); }
			} private string fileName;

			/// <summary>
			/// 附件的内容 [由经Base64编码的字节组成]
			/// </summary>
			public string Bytes 
			{
				get { return bytes; }
				set { if (value != bytes) bytes = value; }
			} private string bytes;

			/// <summary>
			/// 从流中读取附件内容并构造
			/// </summary>
			/// <param name="ifileName">附件的文件名</param>
			/// <param name="stream">流</param>
			public AttachmentInfo (string ifileName, Stream stream)
			{
				fileName = Path.GetFileName (ifileName);
				byte[] by = new byte [stream.Length];
				stream.Read (by,0,(int)stream.Length); // 读取文件内容
				//格式转换
				bytes = Convert.ToBase64String (by); // 转化为base64编码
			}

			/// <summary>
			/// 按照给定的字节构造附件
			/// </summary>
			/// <param name="ifileName">附件的文件名</param>
			/// <param name="ibytes">附件的内容 [字节]</param>
			public AttachmentInfo (string ifileName, byte[] ibytes)
			{
				fileName = Path.GetFileName (ifileName);
				bytes = Convert.ToBase64String (ibytes); // 转化为base64编码
			}

			/// <summary>
			/// 从文件载入并构造
			/// </summary>
			/// <param name="path"></param>
			public AttachmentInfo (string path)
			{
				fileName = Path.GetFileName (path);
				FileStream file = new FileStream (path, FileMode.Open);
				byte[] by = new byte [file.Length];
				file.Read (by,0,(int)file.Length); // 读取文件内容
				//格式转换
				bytes = Convert.ToBase64String (by); // 转化为base64编码
				file.Close ();
			}
		}
	}
}