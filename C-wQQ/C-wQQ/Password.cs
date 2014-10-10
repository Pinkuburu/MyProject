using System;
using System.Security.Cryptography;
using System.Text;

namespace C_wQQ
{
	public class Password
	{
		private static string binl2hex(byte[] buffer)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < buffer.Length; i++)
			{
				builder.Append(buffer[i].ToString("x2"));
			}
			return builder.ToString();
		}
		private static string md5_3(string input)
		{
			MD5 md = MD5.Create();
			byte[] buffer = md.ComputeHash(Encoding.Default.GetBytes(input));
			buffer = md.ComputeHash(buffer);
			buffer = md.ComputeHash(buffer);
			return binl2hex(buffer);
		}

		private  static string md5(string input)
		{
			byte[] buffer = MD5.Create().ComputeHash(Encoding.Default.GetBytes(input));
			return binl2hex(buffer);
		}

		public static string getPassword(string password, string verifycode)
		{
			return md5(md5_3(password).ToUpper() + verifycode.ToUpper()).ToUpper();
		}
	}
}
