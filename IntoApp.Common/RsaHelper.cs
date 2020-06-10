using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IntoApp.Common
{
    public class RsaHelper
    {
        //Rsa加密
        public string RSAEncrypt(string normaltxt)
        {
            var bytes = Encoding.Default.GetBytes(normaltxt);
            var encryptBytes = new RSACryptoServiceProvider(new CspParameters()).Encrypt(bytes, false);
            return Convert.ToBase64String(encryptBytes);
        }
        //RSA解密
        public string RSADecrypt(string securityTxt)
        {
            try//必须使用Try catch,不然输入的字符串不是净荷明文程序就Gameover了
            {
                var bytes = Convert.FromBase64String(securityTxt);
                var DecryptBytes = new RSACryptoServiceProvider(new CspParameters()).Decrypt(bytes, false);
                return Encoding.Default.GetString(DecryptBytes);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 生成密钥
        /// <param name="PrivateKey">私钥</param>
        /// <param name="PublicKey">公钥</param>
        /// <param name="KeySize">密钥长度：512,1024,2048，4096，8192</param>
        /// </summary>
        public static void Generator(out string PrivateKey, out string PublicKey, int KeySize)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KeySize);
            PrivateKey = rsa.ToXmlString(true); //将RSA算法的私钥导出到字符串PrivateKey中 参数为true表示导出私钥 true 表示同时包含 RSA 公钥和私钥；false 表示仅包含公钥。
            PublicKey = rsa.ToXmlString(false); //将RSA算法的公钥导出到字符串PublicKey中 参数为false表示不导出私钥 true 表示同时包含 RSA 公钥和私钥；false 表示仅包含公钥。
        }
        /// <summary>
        /// RSA加密 将公钥导入到RSA对象中，准备加密
        /// </summary>
        /// <param name="PublicKey">公钥</param>
        /// <param name="encryptstring">待加密的字符串</param>
        public static string RSAEncrypt(string PublicKey, string encryptstring)
        {
            byte[] PlainTextBArray;
            byte[] CypherTextBArray;
            string Result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PublicKey);
            PlainTextBArray = (new UnicodeEncoding()).GetBytes(encryptstring);
            CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
            Result = Convert.ToBase64String(CypherTextBArray);
            return Result;
        }
        /// <summary>
        /// RSA解密 将私钥导入RSA中，准备解密
        /// </summary>
        /// <param name="PrivateKey">私钥</param>
        /// <param name="decryptstring">待解密的字符串</param>
        public static string RSADecrypt(string PrivateKey, string decryptstring)
        {
            byte[] PlainTextBArray;
            byte[] DypherTextBArray;
            string Result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PrivateKey);
            PlainTextBArray = Convert.FromBase64String(decryptstring);
            DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
            Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
            return Result;
        }
    }
}
