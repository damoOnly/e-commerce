using EcShop.Core.ErrorLog;
using EcShop.Membership.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace EcShop.Entities.YJF
{
    public class CertNoValidHelper
    {
        private CertNoValidRequest yjf;
        private CertNoValidResponse yjfResponse;
        public CertNoValidHelper()
        { }

        public string CertNoValid(string realName, string certNo)
        {
            string str = "";
            if (!string.IsNullOrEmpty(realName) && !string.IsNullOrEmpty(certNo))
            {
                yjf = new CertNoValidRequest();
                yjf.realName = realName;
                yjf.certNo = certNo;
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                if (!string.IsNullOrEmpty(masterSettings.yijifuUrl))
                {
                    yjf.yijifuUrl = masterSettings.yijifuUrl;
                }
                if (!string.IsNullOrEmpty(masterSettings.serviceCertNoValidCode))
                {
                    yjf.serviceCode = masterSettings.serviceCertNoValidCode;
                }
                if (!string.IsNullOrEmpty(masterSettings.partnerId))
                {
                    yjf.partnerId = masterSettings.partnerId;
                }
                if (!string.IsNullOrEmpty(masterSettings.signType))
                {
                    yjf.signType = masterSettings.signType;
                }
                if (!string.IsNullOrEmpty(masterSettings.YJFPaySignKey))
                {
                    yjf.YJFPaySignKey = masterSettings.YJFPaySignKey;
                }
                str=CreateString();
            }
            return str;
        }

        private string CreateString()
        {
            string retstr="验证失败";
            yjfResponse = new CertNoValidResponse();
            Dictionary<string, string> Dictionary = new Dictionary<string, string>();
            yjf.orderNo = Guid.NewGuid().ToString().Substring(0, 32);

            Dictionary.Add("orderNo", yjf.orderNo);
            Dictionary.Add("signType", yjf.signType);
            Dictionary.Add("service", yjf.serviceCode);
            Dictionary.Add("partnerId", yjf.partnerId);
            Dictionary.Add("realName", yjf.realName);
            Dictionary.Add("certNo", yjf.certNo);
            string sign = yjfSign(Dictionary, yjf.YJFPaySignKey);
            Dictionary.Add("sign", sign);
            string signature = BuildQuery(Dictionary, false);
            ErrorLog.Write("易极付实名认证发送结果:" + signature);
            string str = SendData(yjf.yijifuUrl, signature);
            ErrorLog.Write("易极付实名认证返回结果:" + str);
            yjfResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CertNoValidResponse>(str);
            if (yjfResponse!=null)
            {
                if (yjfResponse.resultCode.ToUpper() == "EXECUTE_SUCCESS")
                {
                    if (yjfResponse.realNameQueryResult.ToUpper() == "PASS")
                    {
                        retstr = "实名通过";
                    }
                    if (yjfResponse.realNameQueryResult.ToUpper() == "UNPASS")
                    {
                        retstr = "实名未通过";
                    }
                    if (yjfResponse.realNameQueryResult.ToUpper() == "FAIL")
                    {
                        retstr = "实名查询失败";
                    }
                }
                else
                {
                    retstr = "查询异常";
                }
            }

            return retstr;
        }

        /// <summary>
        /// 创建签名
        /// </summary>
        public string yjfSign(IDictionary<string, string> parameters, string key = "")
        {
            string text = BuildQuery(parameters, false);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(text + key));
            string str = byteToHexstr(bytes);
            return byteToHexstr(bytes).ToLower();
        }
        /// <summary>
        /// 二进制转换成十六进制
        /// </summary>
        public string byteToHexstr(byte[] bytes)
        {
            string str = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    str += bytes[i].ToString("X2");
                }
            }
            return str;
        }

        /// <summary>
        /// 发送POST请求
        /// </summary>
        public string SendData(string url, string postData)
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
                    Stream myResponseStream = response.GetResponseStream();

                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                    str = myStreamReader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                str = string.Format("获取信息错误：{0}", exception.Message);
            }
            return str;
        }
        /// <summary>
        /// 拼接数据
        /// </summary>
        public string BuildQuery(IDictionary<string, string> dict, bool encode)
        {
            SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(dict);
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            IEnumerator<KeyValuePair<string, string>> enumerator = sortedDictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, string> current = enumerator.Current;
                string key = current.Key;
                current = enumerator.Current;
                string value = current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    if (flag)
                    {
                        stringBuilder.Append("&");
                    }
                    stringBuilder.Append(key);
                    stringBuilder.Append("=");
                    stringBuilder.Append(value);
                    //if (encode)
                    //{
                    //    stringBuilder.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    //}
                    //else
                    //{
                    //    stringBuilder.Append(value);
                    //}
                    flag = true;
                }
            }
            return stringBuilder.ToString();
        }
    }
}
