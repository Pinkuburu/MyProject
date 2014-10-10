using System;
using System.Security.Cryptography;
using System.IO;

namespace DES_Test
{
    /// <summary>
    /// 非对称RSA
    /// </summary>
    public class RSA_
    {
        private RSACryptoServiceProvider rsa;
        public RSA_()
        {
            rsa = new RSACryptoServiceProvider();
        }
        /// <summary>
        /// 得到公钥
        /// </summary>
        /// <returns></returns>
        public string GetPublicKey()
        {
            //return rsa.ToXmlString(false);
            return "<RSAKeyValue><Modulus>wTtT2F5UYOHPeo/mAVr3z3eNNg90XXnFuL4PtKA8l7rTYzbgrmRkEARcXpGYM+EaLvedEM6yrGZeJ5Y7+P6FBQSJp1HbJ126yHSQzp098lbHVUDHItAc/zy46LH1a4lYuCFVhUKmSmcXX1aH5pl8m0ZqunH7tRuCPBM+i/Fzkb0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        }
        /// <summary>
        /// 得到私钥
        /// </summary>
        /// <returns></returns>
        public string GetPrivateKey()
        {
            //return rsa.ToXmlString(true);
            return "<RSAKeyValue><Modulus>wTtT2F5UYOHPeo/mAVr3z3eNNg90XXnFuL4PtKA8l7rTYzbgrmRkEARcXpGYM+EaLvedEM6yrGZeJ5Y7+P6FBQSJp1HbJ126yHSQzp098lbHVUDHItAc/zy46LH1a4lYuCFVhUKmSmcXX1aH5pl8m0ZqunH7tRuCPBM+i/Fzkb0=</Modulus><Exponent>AQAB</Exponent><P>9x3dUfrDV0y9dl/kpOS4SV4FYrrKiS+drS5S43y/Rd7SvnpHnYQE7PNj2qCanX8bb2xKnvMEkeZ8M0YhzhgBew==</P><Q>yC2T0YpX4MPguOMvQGeDIkhkpbateKda6cQZpJbDLHhP3EP4F7AQs0bLvrRUm7XceAJBKSC0Q5oxu8JwXGaIJw==</Q><DP>iVtkRckpA0F4nm+226D2fnFwdOx238JD1ptFH4Wbm+67HX1CiV90jXDMNB3JU3nvegOrhqZ2B9Mhfi6hY7kcOQ==</DP><DQ>JP7HYuJ+ezu4PHNAOFbpFVzrvPSV+sZzNuDXHGQAjiduGvc00qvnajqbTRNmz6A8rrE7+a3hotzMdDbrLSiF4Q==</DQ><InverseQ>5ILbOlITkUICBg9Nx1OSslfAQV5ocXdv7PNvtpIOzalhZTbREdlYvLCbctE7j9B3eUs20/pa/g5iXHPVhqir1w==</InverseQ><D>g8A86gFfTPOKcQnejLwGzXm5WcvtzrJxwwYsVT8QMXBcI4DOb3nZ1CX1CTICchUgsjd463XT08K0ng9Mcvivnsw+n9i2KPAhd1Lrrc0vzUXCmz89AgpFKtErZYt1EmXyWY58ZBsObLfPJna9YnhOQDFKh1Kn75WYB9/ewaELf20=</D></RSAKeyValue>";
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Source">待加密字符串</param>
        /// <param name="PublicKey">公钥</param>
        /// <returns></returns>
        public string Encrypt(string Source, string PublicKey)
        {
            rsa.FromXmlString(PublicKey);
            byte[] done = rsa.Encrypt(Convert.FromBase64String(Source), false);
            return Convert.ToBase64String(done);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Source">待加密字符数组</param>
        /// <param name="PublicKey">公钥</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] Source, string PublicKey)
        {
            rsa.FromXmlString(PublicKey);
            return rsa.Encrypt(Source, false);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="inFileName">待加密文件路径</param>
        /// <param name="outFileName">加密后文件路径</param>
        /// <param name="PublicKey">公钥</param>
        public void Encrypt(string inFileName, string outFileName, string PublicKey)
        {
            rsa.FromXmlString(PublicKey);
            FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            byte[] bin = new byte[1000];
            long rdlen = 0;
            long totlen = fin.Length;
            int len;

            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 1000);
                byte[] bout = rsa.Encrypt(bin, false);
                fout.Write(bout, 0, bout.Length);
                rdlen = rdlen + len;
            }

            fout.Close();
            fin.Close();

        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Source">待解密字符串</param>
        /// <param name="PrivateKey">私钥</param>
        /// <returns></returns>
        public string Decrypt(string Source, string PrivateKey)
        {
            rsa.FromXmlString(PrivateKey);
            byte[] done = rsa.Decrypt(Convert.FromBase64String(Source), false);
            return Convert.ToBase64String(done);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Source">待解密字符数组</param>
        /// <param name="PrivateKey">私钥</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] Source, string PrivateKey)
        {
            rsa.FromXmlString(PrivateKey);
            return rsa.Decrypt(Source, false);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="inFileName">待解密文件路径</param>
        /// <param name="outFileName">解密后文件路径</param>
        /// <param name="PrivateKey">私钥</param>
        public void Decrypt(string inFileName, string outFileName, string PrivateKey)
        {
            rsa.FromXmlString(PrivateKey);
            FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            byte[] bin = new byte[1000];
            long rdlen = 0;
            long totlen = fin.Length;
            int len;

            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 1000);
                byte[] bout = rsa.Decrypt(bin, false);
                fout.Write(bout, 0, bout.Length);
                rdlen = rdlen + len;
            }

            fout.Close();
            fin.Close();

        }
    }
}
