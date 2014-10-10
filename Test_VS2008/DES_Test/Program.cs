using System;
using System.Collections.Generic;
using System.Text;

namespace DES_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //DES_ des = new DES_("Cupid");
            //string aaa = des.Encrypt("abcd12we9fudfjkqewapjiosdfjwe939024jewklfiojqfei2asdfa34");
            //Console.WriteLine(aaa);
            //string bbb = des.Decrypt(aaa);
            //Console.WriteLine(bbb);
            //Console.ReadKey();
            RSA_ rsa = new RSA_();
            string P_key = rsa.GetPublicKey();
            string S_key = rsa.GetPrivateKey();
            string aaa = rsa.Encrypt("abcd1234asdfqwefasdfqwefasdfqewfar34gq2351435rwaerwfasdf", P_key);
            string bbb = rsa.Decrypt(aaa, S_key);
            Console.WriteLine("公钥：" + P_key);
            Console.WriteLine("私钥：" + S_key);
            Console.WriteLine("加密后的数据：" + aaa);
            Console.WriteLine("解密后的数据：" + bbb);
            Console.ReadKey();
        }
    }
}
