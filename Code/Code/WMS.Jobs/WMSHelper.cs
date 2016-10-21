using EcShop.SqlDal.WMS;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace WMS.Jobs
{
    public static class WMSHelper
    {
        public static string BuildSign(Dictionary<string, string> dicArray, string key, string sign_type, string _input_charset)
        {
            return Sign(CreateLinkstring(dicArray) + key, sign_type, _input_charset);
        }

        public static string CreateLinkstring(Dictionary<string, string> dicArray)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in dicArray)
            {
                builder.Append(pair.Key + "=" + pair.Value + "&");
            }
            int length = builder.Length;
            builder.Remove(length - 1, 1);
            return builder.ToString();
        }

        public static Dictionary<string, string> Parameterfilter(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in dicArrayPre)
            {
                if ((((pair.Key.ToLower() != "sign") && (pair.Key.ToLower() != "sign_type")) && (pair.Value != "")) && (pair.Value != null))
                {
                    string key = pair.Key.ToLower();
                    dictionary.Add(key, pair.Value);
                }
            }
            return dictionary;
        }

        public static string PostData(string url, string postData)
        {
            string str = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream2 = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        Stream stream3 = stream2;
                        if (response.ContentEncoding.ToLower() == "gzip")
                        {
                            stream3 = new GZipStream(stream2, CompressionMode.Decompress);
                        }
                        else if (response.ContentEncoding.ToLower() == "deflate")
                        {
                            stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
                        }
                        using (StreamReader reader = new StreamReader(stream3, encoding))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                str = string.Format("获取信息错误：{0}", exception.Message);
            }
            return str;
        }

        public static string Sign(string prestr, string sign_type, string _input_charset)
        {
            StringBuilder builder = new StringBuilder(0x20);
            if (sign_type.ToUpper() == "MD5")
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
                for (int i = 0; i < buffer.Length; i++)
                {
                    builder.Append(buffer[i].ToString("x").PadLeft(2, '0'));
                }
            }
            return builder.ToString();
        }


        ///<summary>
        /// MD5加密
        /// </summary>
        /// <param name="toCryString">被加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5(string toCryString)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(toCryString, "MD5");
        }

        public static string MD51(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(str));
            string str2 = "";
            for (int i = 0; i < result.Length; i++)
            {
                str2 += string.Format("{0:x}", result[i]);
            }
            return str2;
        }


        /// <summary>
        /// 将字符串使用base64算法加密
        /// </summary>
        /// <param name="SourceString">待加密的字符串</param>
        /// <param name="Ens">System.Text.Encoding 对象，如创建中文编码集对象：
        /// System.Text.Encoding.GetEncoding("gb2312")</param>
        /// <returns>编码后的文本字符串</returns>
        public static string EncodingString(string SourceString, System.Text.Encoding Ens)
        {
            return Convert.ToBase64String(Ens.GetBytes(SourceString));
        }


        /// <summary>
        /// 使用缺省的代码页将字符串使用base64算法加密
        /// </summary>
        /// <param name="SourceString">待加密的字符串</param>
        /// <returns>加密后的文本字符串</returns>
        public static string EncodingString(string SourceString)
        {
            return EncodingString(SourceString, System.Text.Encoding.Default);
        }

        /// <summary>
        /// 记录WMS日志
        /// </summary>
        /// <param name="method">方法名</param>
        /// <param name="param">参数</param>
        /// <param name="logcontent">日志内容</param>
        /// <param name="logtype">日志类型 info error</param>
        /// <param name="methodtype">方法类型  in out </param>
        /// <returns></returns>
        public static bool SaveLog(string method, string param, string logcontent, string logtype, string methodtype)
        {
            return new WMSLogDao().SaveLog(method, param, logcontent, logtype, methodtype);
        }

    }
}
