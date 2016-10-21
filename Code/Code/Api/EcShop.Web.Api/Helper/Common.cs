using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace EcShop.Web.Api.Helper
{
    public class Common
    {
        public static string Md5(string password, string salt)
        {
            password = password + salt;
            
            MD5 md5 = System.Security.Cryptography.MD5.Create();

            return BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))).Replace("-", "").ToLower();

        }

        public static string CreateSalt()
        {
            byte[] array = new byte[16];
            
            new RNGCryptoServiceProvider().GetBytes(array);

            return Convert.ToBase64String(array);
        }

        public static string EncodePassword(MembershipPasswordFormat format, string cleanString, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(salt + cleanString);

            string result;
            switch (format)
            {
                case MembershipPasswordFormat.Clear:
                    result = cleanString;
                    break;
                case MembershipPasswordFormat.Hashed:
                    {
                        byte[] value = ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(bytes);
                        result = BitConverter.ToString(value);
                        break;
                    }
                default:
                    {
                        byte[] value = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes);
                        result = BitConverter.ToString(value);
                        break;
                    }
            }

            return result;
        }

        /// <summary>
        /// 将一个大数字符串从M进制转换成N进制
        /// </summary>
        /// <param name="sourceValue">M进制数字字符串</param>
        /// <param name="sourceBaseChars">M进制字符集</param>
        /// <param name="newBaseChars">N进制字符集</param>
        /// <returns>N进制数字字符串</returns>
        public static string BaseConvert(string sourceValue, string sourceBaseChars, string newBaseChars)
        {

            //M进制
            var sBase = sourceBaseChars.Length;
            //N进制
            var tBase = newBaseChars.Length;
            //M进制数字字符串合法性判断（判断M进制数字字符串中是否有不包含在M进制字符集中的字符）
            if (sourceValue.Any(s => !sourceBaseChars.Contains(s))) return null;
            //将M进制数字字符串的每一位字符转为十进制数字依次存入到LIST中
            var intSource = new List<int>();
            intSource.AddRange(sourceValue.Select(c => sourceBaseChars.IndexOf(c)));
            //余数列表
            var res = new List<int>();
            //开始转换（判断十进制LIST是否为空或只剩一位且这个数字小于N进制）
            while (!((intSource.Count == 1 && intSource[0] < tBase) || intSource.Count == 0))
            {
                //每一轮的商值集合
                var ans = new List<int>();
                var y = 0;
                //十进制LIST中的数字逐一除以N进制（注意：需要加上上一位计算后的余数乘以M进制）
                foreach (var t in intSource)
                {
                    //当前位的数值加上上一位计算后的余数乘以M进制
                    y = y * sBase + t;

                    //保存当前位与N进制的商值
                    ans.Add(y / tBase);

                    //计算余值
                    y %= tBase;

                }

                //将此轮的余数添加到余数列表
                res.Add(y);

                //将此轮的商值（去除0开头的数字）存入十进制LIST做为下一轮的被除数
                var flag = false;

                intSource.Clear();

                foreach (var a in ans.Where(a => a != 0 || flag))
                {
                    flag = true;

                    intSource.Add(a);

                }

            }

            //如果十进制LIST还有数字，需将此数字添加到余数列表后
            if (intSource.Count > 0) res.Add(intSource[0]);

            //将余数列表反转，并逐位转换为N进制字符
            var nValue = string.Empty;

            for (var i = res.Count - 1; i >= 0; i--)
            {

                nValue += newBaseChars[res[i]].ToString();

            }

            return nValue;
        }
    }
}