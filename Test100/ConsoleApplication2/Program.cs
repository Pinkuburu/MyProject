using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Crypt
{
    class Crypt
    {

        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine(SHA1Encrypt("677521"));
            Console.ReadLine();
        }

        class RC2
        {
            private byte[] Key;
            private byte[] IV;
            private byte[] orgText;
            private byte[] encryptText;
            private byte[] decryptText;
            public RC2(string rc2Text)
            {
                orgText = Encoding.Default.GetBytes(rc2Text);
                RC2CryptoServiceProvider myRC2 = new RC2CryptoServiceProvider();
                myRC2.GenerateIV();
                myRC2.GenerateKey();
                Key = myRC2.Key;
                IV = myRC2.IV;
            }
            public string RC2Encrypt()
            {
                RC2CryptoServiceProvider myRC2 = new RC2CryptoServiceProvider();
                ICryptoTransform myCryptoTrans = myRC2.CreateEncryptor(Key, IV);
                MemoryStream MStream = new MemoryStream();
                CryptoStream CStream = new CryptoStream(MStream, myCryptoTrans, CryptoStreamMode.Write);
                CStream.Write(orgText, 0, orgText.Length);
                CStream.FlushFinalBlock();
                StringBuilder EnText = new StringBuilder();
                encryptText = MStream.ToArray();
                foreach (byte Byte in encryptText)
                {
                    EnText.AppendFormat("{0:x2}", Byte);
                }
                CStream.Close();
                return EnText.ToString();
            }
            public string RC2Decrypt()
            {
                RC2CryptoServiceProvider myRC2 = new RC2CryptoServiceProvider();
                ICryptoTransform myCryptoTrans = myRC2.CreateDecryptor(Key, IV);
                MemoryStream MStream = new MemoryStream(encryptText);
                CryptoStream CStream = new CryptoStream(MStream, myCryptoTrans, CryptoStreamMode.Read);
                decryptText = new byte[encryptText.Length];
                CStream.Read(decryptText, 0, decryptText.Length);
                StringBuilder EnText = new StringBuilder();
                CStream.Close();
                ASCIIEncoding myText = new ASCIIEncoding();
                return myText.GetString(decryptText);
            }
        }
        public static string SHA1Encrypt(string EncryptText)
        {
            byte[] StrRes = Encoding.Default.GetBytes(EncryptText);
            HashAlgorithm mySHA = new SHA1CryptoServiceProvider();
            StrRes = mySHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte Byte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", Byte);
            }
            return EnText.ToString();
        }
        public static string HMACSHA1Encrypt(string EncryptText, string EncryptKey)
        {
            byte[] StrRes = Encoding.Default.GetBytes(EncryptText);
            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.Default.GetBytes(EncryptKey));
            CryptoStream CStream = new CryptoStream(Stream.Null, myHMACSHA1, CryptoStreamMode.Write);
            CStream.Write(StrRes, 0, StrRes.Length);
            StringBuilder EnText = new StringBuilder();
            foreach (byte Byte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", Byte);
            }
            return EnText.ToString();
        }
        public static string MD5Encrypt(string CryptText)
        {
            MD5 myMD5 = new MD5CryptoServiceProvider();
            byte[] HashCode;
            HashCode = Encoding.Default.GetBytes(CryptText);
            HashCode = myMD5.ComputeHash(HashCode);
            StringBuilder EnText = new StringBuilder();
            foreach (byte Byte in HashCode)
            {
                EnText.AppendFormat("{0:x2}", Byte);
            }
            return EnText.ToString();
        }
        public static string DESEncrypt(string CryptText, string CryptKey, string CryptIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] textOut = Encoding.Default.GetBytes(CryptText);
            byte[] DESKey = ASCIIEncoding.ASCII.GetBytes(CryptKey);
            byte[] DESIV = ASCIIEncoding.ASCII.GetBytes(CryptKey);
            MemoryStream MStream = new MemoryStream();
            CryptoStream CStream = new CryptoStream(MStream, des.CreateEncryptor(DESKey, DESIV), CryptoStreamMode.Write);
            CStream.Write(textOut, 0, textOut.Length);
            CStream.FlushFinalBlock();
            StringBuilder StrRes = new StringBuilder();
            foreach (byte Byte in MStream.ToArray())
            {
                StrRes.AppendFormat("{0:x2}", Byte);
            }
            return StrRes.ToString();
        }

        public static string DESDecrypt(string CryptText, string CryptKey, string CryptIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] textOut = new byte[CryptText.Length / 2];
            for (int Count = 0; Count < CryptText.Length; Count += 2)
            {
                textOut[Count / 2] = (byte)(Convert.ToInt32(CryptText.Substring(Count, 2), 16));
            }
            byte[] DESKey = ASCIIEncoding.ASCII.GetBytes(CryptKey);
            byte[] DESIV = ASCIIEncoding.ASCII.GetBytes(CryptIV);
            MemoryStream MStream = new MemoryStream();
            CryptoStream CStream = new CryptoStream(MStream, des.CreateDecryptor(DESKey, DESIV), CryptoStreamMode.Write);
            CStream.Write(textOut, 0, textOut.Length);
            CStream.FlushFinalBlock();
            return System.Text.Encoding.Default.GetString(MStream.ToArray());
        }
    }
}
