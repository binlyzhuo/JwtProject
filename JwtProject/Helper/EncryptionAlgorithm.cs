using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace JwtProject.Helper
{
    public static class EncryptionAlgorithm
    {
        public static string Hash(string myStr)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(myStr);
            byte[] dataHashed = sha.ComputeHash(dataToHash);
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");
            return hash;
        }

        /// <summary>
        /// 获取Base64编码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetUtf8Base64FromString(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            string str = Convert.ToBase64String(bytes);
            return str;
        }

        /// <summary>
        /// base64解码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetStringFromBase64(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            byte[] outputb = Convert.FromBase64String(source);
            string orgStr = Encoding.UTF8.GetString(outputb);
            return orgStr;
        }
    }
}