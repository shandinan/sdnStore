using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Framework.Utility
{
    public class MD5Helper
    {

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="strText">要加密字符串</param>
        /// <param name="IsLower">是否以小写方式返回</param>
        /// <returns></returns>
        public static string MD5EncryptLower(string strText, bool IsLower)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(strText);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            // return ret.PadLeft(32, '0');
            if(IsLower)
            {
                return ret.PadLeft(32, '0').ToLower();
            }
            else
            {
                return ret.PadLeft(32, '0');
            }
        }
        /// <summary>
        /// 32位MD5加密大写
        /// </summary>
        /// <param name="strText">要加密的字符串</param>
        /// <param name="isUpper">是否大写输出</param>
        /// <returns></returns>
        public static string MD5Encrypt(string strText,bool isUpper)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(strText);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();
            string ret = "";
            for(int i=0;i<bytes.Length;i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }
            if(isUpper)
            {
                return ret.PadLeft(32, '0').ToUpper();
            }
            else
            {
                return ret.PadLeft(32, '0');
            }
        }
    }
}
