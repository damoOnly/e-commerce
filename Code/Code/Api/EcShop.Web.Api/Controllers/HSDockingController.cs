
using EcShop.ControlPanel.Commodities;
using EcShop.Entities.YJF;
using EcShop.Membership.Context;
using EcShop.Web.Api.ApiException;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace EcShop.Web.Api.Controllers
{
    public class HSDockingController : ApiController
    {
        private HttpContextBase request;
        private YJFResponse yjfResponse;

       [HttpPost]
        // GET: api/HSDocking/5
        public string GetAsyHSDocking([FromBody]string value)
        //public string GetAsyHSDocking(string value)
        {
           SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
           //测试环境key
           //string key = "2af0376a5dc1695aa1ab889384a8ade9";
           //正式环境ky
           //string key = "1cc79cc2f1b661b2705161e2c72ab9c3";
           string key = "";
           string retStr = "fail";
           if (!string.IsNullOrEmpty(masterSettings.YJFPaySignKey))
           {
               key = masterSettings.YJFPaySignKey;
           }

           request = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context

           yjfResponse = new YJFResponse();
           
           yjfResponse.protocol = request.Request.Form["protocol"];
           yjfResponse.orderNo = request.Request.Form["orderNo"];
           yjfResponse.service = request.Request.Form["service"];
           yjfResponse.success = request.Request.Form["success"];
           yjfResponse.notifyTime = request.Request.Form["notifyTime"];
           yjfResponse.resultCode = request.Request.Form["resultCode"];
           yjfResponse.sign = request.Request.Form["sign"];
           yjfResponse.signType = request.Request.Form["signType"];
           yjfResponse.partnerId = request.Request.Form["partnerId"];
           yjfResponse.resultMessage = request.Request.Form["resultMessage"];
           yjfResponse.version = request.Request.Form["version"];

           try
           {
               yjfResponse.resultInfosAsy = Newtonsoft.Json.JsonConvert.DeserializeObject<List<resultInfosAsy>>(request.Request.Form["resultInfos"]);

               string resultInfosJson = GetresultInfos(yjfResponse.resultInfosAsy[0]);

               Logger.WriterLogger("yjfResponse Params: " + string.Format("protocol={0}&orderNo={1}&service={2}&success={3}&notifyTime={4}&" +
               "resultCode={5}&sign={6}&signType={7}&partnerId={8}&resultInfosJson={9}&resultMessage={10}&version={11}",
               yjfResponse.protocol, yjfResponse.orderNo, yjfResponse.service, yjfResponse.success,
               yjfResponse.notifyTime, yjfResponse.resultCode, yjfResponse.sign, yjfResponse.signType,
               yjfResponse.partnerId, resultInfosJson, yjfResponse.resultMessage, yjfResponse.version), LoggerType.Info);

               if (yjfResponse.success.ToUpper() == "TRUE")
               {
                   if (compareSign(yjfResponse.sign, resultInfosJson, key))
                   {
                       retStr = "success";
                       UpdateHSStatus();
                   }
               }
           }
           catch (Exception ex)
           {
               Logger.WriterLogger(ex.ToString(), LoggerType.Error);
           }

           return retStr;
        }

        [HttpGet]
        // GET: api/HSDocking/5
        public void GetSynHSDocking(string str)
        {
            //return "value";
        }

        // POST: api/HSDocking
        public string Post([FromBody]string value)
        {
            return "222";
        }

        // PUT: api/HSDocking/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/HSDocking/5
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 更新海关状态
        /// </summary>
        private void UpdateHSStatus()
        {
            if (yjfResponse.orderNo != null)
            {
                if (yjfResponse.resultCode == "EXECUTE_SUCCESS")
                {
                    HSCodeHelper.SaveHSDocking(3, yjfResponse.resultInfosAsy[0].outOrderNo, "", "", 2, yjfResponse.resultInfosAsy[0].message, "", "", "", yjfResponse.resultInfosAsy[0].availableBalance.ToString(), yjfResponse.resultInfosAsy[0].tradeNo);
                }
                else
                {
                    HSCodeHelper.SaveHSDocking(3, yjfResponse.resultInfosAsy[0].outOrderNo, "", "", 3, yjfResponse.resultInfosAsy[0].message, "", "", "", yjfResponse.resultInfosAsy[0].availableBalance.ToString(), yjfResponse.resultInfosAsy[0].tradeNo);
                }
            }
        }

        /// <summary>
        /// 创建签名
        /// </summary>
        public string yjfSign(IDictionary<string, string> parameters, string key)
        {
            string text = BuildQuery(parameters, false);
            MD5 md5 = new MD5CryptoServiceProvider();

            Logger.WriterLogger("sign message : " + text + key);

            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(text + key));

            string str = byteToHexstr(bytes);

            Logger.WriterLogger("sign : " + byteToHexstr(bytes).ToLower());
            return byteToHexstr(bytes).ToLower();
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
                    if (encode)
                    {
                        stringBuilder.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    }
                    else
                    {
                        stringBuilder.Append(value);
                    }
                    flag = true;
                }
            }
            return stringBuilder.ToString();
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
        /// 获取resultInfos的json数据
        /// </summary>
        public string GetresultInfos(resultInfosAsy resultInfosAsy)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(resultInfosAsy);
        }
        /// <summary>
        /// 比较回传签名
        /// </summary>
        /// <returns></returns>
        public bool compareSign(string sign, string resultInfosJson ,string key)
        {
            Dictionary<string, string> Dictionary = new Dictionary<string, string>();

            Dictionary.Add("protocol", yjfResponse.protocol);
            Dictionary.Add("orderNo", yjfResponse.orderNo);
            Dictionary.Add("service", yjfResponse.service);
            Dictionary.Add("success", yjfResponse.success);
            Dictionary.Add("notifyTime", yjfResponse.notifyTime);
            Dictionary.Add("resultCode", yjfResponse.resultCode);
            Dictionary.Add("signType", yjfResponse.signType);
            Dictionary.Add("partnerId", yjfResponse.partnerId);
            Dictionary.Add("resultInfos", "["+resultInfosJson+"]");
            Dictionary.Add("resultMessage", yjfResponse.resultMessage);
            Dictionary.Add("version", yjfResponse.version);

            string hmSign = yjfSign(Dictionary, key);

            if (sign.ToLower() == hmSign.ToLower())
            {
                return true;
            }
            return false;            
        }
        
    }
}
