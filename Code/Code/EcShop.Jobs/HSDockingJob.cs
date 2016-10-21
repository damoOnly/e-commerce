using EcShop.ControlPanel.Commodities;
using EcShop.Core.ErrorLog;
using EcShop.Core.Jobs;
using EcShop.Entities.YJF;
using EcShop.Membership.Context;
using log4net.Repository.Hierarchy;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace EcShop.Jobs
{
    public class HSDockingJob : IJob
    {
        private YJFRequest yjf;
        private YJFResponse yjfResponse;
        private vosList voList;
        private resultInfos resultInfo;
        /// <summary>
        /// 获取SiteSettings.config数据
        /// </summary>
        /// <param name="node"></param>
        public void Execute(XmlNode node)
        {

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            yjf = new YJFRequest();
            voList = new vosList();


            //yjf.eshopEntCode = "4403660034";
            //yjf.eshopEntName = "深圳市信捷网科技有限公司";
            //voList.customsCode = "5349";//深圳海关
            ////测试环境
            ////yjf.yijifuUrl = "http://openapi.yijifu.net/gateway.html";
            ////正式环境(国内)
            ////yjf.yijifuUrl="https://openapi.yiji.com/gateway.html";
            ////正式环境(国际)
            //yjf.yijifuUrl = "https://openapiglobal.yiji.com/gateway.html";
            //yjf.serviceCode = "yhtMultiPaymentBillUpload";
            ////测试环境pid
            ////yjf.partnerId = "20140926020000058373";
            ////正式环境pid
            //yjf.partnerId = "20151218020009636822";

            //yjf.signType = "MD5";
            //yjf.returnUrl = "http://localhost:8081/api/HSDocking/GetAsyHSDocking";
            ////yjf.notifyUrl = "http://116.204.11.239:8089/api/HSDocking/GetAsyHSDocking";
            //yjf.notifyUrl = "http://183.62.226.59:8089/api/HSDocking/GetAsyHSDocking";
        
            ////测试环境key
            ////yjf.YJFPaySignKey = "2af0376a5dc1695aa1ab889384a8ade9";
            ////正式环境ky
            //yjf.YJFPaySignKey = "1cc79cc2f1b661b2705161e2c72ab9c3";


            if (!string.IsNullOrEmpty(masterSettings.ebcCode))
            {
                yjf.eshopEntCode = masterSettings.ebcCode;
            }
            if (!string.IsNullOrEmpty(masterSettings.ebcName))
            {
                yjf.eshopEntName = masterSettings.ebcName;
            }
            if (!string.IsNullOrEmpty(masterSettings.customsCode))
            {
                voList.customsCode = masterSettings.customsCode;
            }
            if (!string.IsNullOrEmpty(masterSettings.yijifuUrl))
            {
                yjf.yijifuUrl = masterSettings.yijifuUrl;
            }
            if (!string.IsNullOrEmpty(masterSettings.serviceCode))
            {
                yjf.serviceCode = masterSettings.serviceCode;
            }
            if (!string.IsNullOrEmpty(masterSettings.partnerId))
            {
                yjf.partnerId = masterSettings.partnerId;
            }
            if (!string.IsNullOrEmpty(masterSettings.signType))
            {
                yjf.signType = masterSettings.signType;
            }
            if (!string.IsNullOrEmpty(masterSettings.returnUrl))
            {
                yjf.returnUrl = masterSettings.returnUrl;
            }
            if (!string.IsNullOrEmpty(masterSettings.notifyUrl))
            {
                yjf.notifyUrl = masterSettings.notifyUrl;
            }
            if (!string.IsNullOrEmpty(masterSettings.YJFPaySignKey))
            {
                yjf.YJFPaySignKey = masterSettings.YJFPaySignKey;
            }

            Database database = DatabaseFactory.CreateDatabase();
            //System.Data.Common.DbCommand sqlStringCommand =
            //    database.GetSqlStringCommand("SELECT a.OrderId,a.OrderTotal,a.GatewayOrderId,a.RealName,a.PaymentType FROM Ecshop_Orders a LEFT JOIN HS_Docking b ON a.OrderId=b.OrderId " +
            //                                    "WHERE a.OrderStatus=3 AND (b.OrderId IS NULL OR b.PaymentStatus IN(0,4))");
    //        System.Data.Common.DbCommand sqlStringCommand =
    //database.GetSqlStringCommand("SELECT a.OrderId,a.OrderTotal,a.GatewayOrderId,a.RealName,a.PaymentType,a.IdentityCard,a.ShipTo,a.CouponAmount FROM Ecshop_Orders a LEFT JOIN HS_Docking b ON a.OrderId=b.OrderId " +
    //                                "WHERE ISNULL(a.WMSStatus,0)>=63 AND (b.OrderId IS NULL OR b.PaymentStatus IN(0,4))");
            System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("exec cp_Orders_GetHSDataByPay");//b.OrderId IS NULL OR



            DataTable dt = database.ExecuteDataSet(sqlStringCommand).Tables[0];

            CreateString(dt);

        }

        /// <summary>
        /// 创建传输字符串
        /// </summary>
        /// <param name="dt"></param>

        private void CreateString(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                yjfResponse = new YJFResponse();
                Dictionary<string, string> Dictionary = new Dictionary<string, string>();
                yjf.voList = GetVosList(dr);
                yjf.orderNo = Guid.NewGuid().ToString().Substring(0, 32);

                Dictionary.Add("eshopEntCode", yjf.eshopEntCode);
                Dictionary.Add("eshopEntName", yjf.eshopEntName);
                Dictionary.Add("eshopType", "A");
                Dictionary.Add("notifyUrl", yjf.notifyUrl);
                Dictionary.Add("signType", yjf.signType);
                Dictionary.Add("service", yjf.serviceCode);
                //Dictionary.Add("returnUrl", yjf.returnUrl);
                Dictionary.Add("partnerId", yjf.partnerId);

                Dictionary.Add("orderNo", yjf.orderNo);
                Dictionary.Add("vosList", "[" + yjf.voList + "]");
                string sign = yjfSign(Dictionary, yjf.YJFPaySignKey);
                Dictionary.Add("sign", sign);
                string signature = BuildQuery(Dictionary, false);

                UpdateHSStatus(signature);

                string str = SendData(yjf.yijifuUrl, signature);

                ErrorLog.Write("易极付同步记录:" + str);
                try
                {                 
                    //string str = "{\"sign\":\"0db21c652f8302350772d517878672bc\",\"protocol\":\"httpJson\",\"orderNo\":\"0a48d33e-6431-40f8-8f93-da1d06a6\",\"signType\":\"MD5\",\"resultCode\":\"EXECUTE_SUCCESS\",\"service\":\"yhtMultiPaymentBillUpload\",\"partnerId\":\"20140926020000058373\",\"resultMessage\":\"成功\",\"resultInfos\":[{\"message\":\"--该支付单已经上传，请勿重传--\",\"outOrderNo\":\"2015120775845791\",\"paymentBillMoney\":107.40,\"serviceStatus\":\"FAIL\",\"tradeNo\":\"20151221154126347100\"}],\"success\":true,\"version\":\"1.0\"}";
                    yjfResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<YJFResponse>(str);

                    resultInfo = yjfResponse.resultInfos[0];

                    UpdateHSStatus("");
                }
                catch (Exception ex)
                {
                    string message = "";
                    if(voList.tradeNo.ToString()=="")
                    {
                        message = "tradeNo为空";
                    }
                    HSCodeHelper.SaveHSDocking(3, voList.outOrderNo, "", "", 3, message, "", voList.payerId, voList.payerName, "0", voList.tradeNo);
                }


            }
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

        public string GetVosList(DataRow dr)
        {
            voList.outOrderNo = dr["OrderId"].ToString();
            decimal Amount=0;
            decimal AdjustedFreight=0;
            decimal Tax=0;
            if (!string.IsNullOrEmpty(dr["Amount"].ToString()))
            {
                Amount = (decimal)dr["Amount"];
            }
            if (!string.IsNullOrEmpty(dr["AdjustedFreight"].ToString()))
            {
                AdjustedFreight = (decimal)dr["AdjustedFreight"];
            }
            if (!string.IsNullOrEmpty(dr["Tax"].ToString()))
            {
                Tax = (decimal)dr["Tax"];
            }

            decimal paymentBillAmount = Amount + AdjustedFreight + Tax;

            voList.paymentBillAmount = paymentBillAmount.ToString();

            voList.tradeNo = dr["GatewayOrderId"].ToString();
            //voList.payerName = dr["RealName"].ToString();
            //voList.payerName = "孙明峰";
            //voList.payerId = dr["payerId"].ToString();
            //voList.payerId = "220104198004210012";
            voList.payerName = dr["ShipTo"].ToString();
            voList.payerId = dr["IdentityCard"].ToString();
            voList.paymentType = dr["PaymentType"].ToString();

            if (voList.paymentType == "支付宝")
            {
                voList.paymentType = "ALIPAY";
            }
            else
            {
                voList.paymentType = "WECHAT";
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(voList);
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

        /// <summary>
        /// 更新海关状态
        /// </summary>
        private void UpdateHSStatus(string PaymentData)
        {
            if (yjfResponse.orderNo != null)
            {
                if (voList.outOrderNo == resultInfo.outOrderNo)
                {
                    if (yjfResponse.resultCode.ToUpper() == "EXECUTE_SUCCESS" && resultInfo.serviceStatus.ToUpper() == "SUCCESS")
                    {
                        HSCodeHelper.SaveHSDocking(3, resultInfo.outOrderNo, "", "", 1, resultInfo.message, "", voList.payerId, voList.payerName, "0", voList.tradeNo);
                    }
                    //else if (yjfResponse.resultCode.ToUpper() == "EXECUTE_SUCCESS" && resultInfo.serviceStatus.ToUpper() == "FAIL")
                    //{
                    //    HSCodeHelper.SaveHSDocking(3, resultInfo.outOrderNo, "", "", 3, yjfResponse.resultMessage, "");
                    //}
                    else
                    { 
                        HSCodeHelper.SaveHSDocking(3, resultInfo.outOrderNo, "", "", 3, resultInfo.message, "", voList.payerId, voList.payerName, "0", voList.tradeNo);
                    }
                }
                else
                {
                    HSCodeHelper.SaveHSDocking(3, resultInfo.outOrderNo, "", "", 3, yjfResponse.resultMessage, "",voList.payerId,voList.payerName,"0",voList.tradeNo);
                }
            }
            if (!string.IsNullOrEmpty(PaymentData))
            {
                HSCodeHelper.SaveHSDocking(3, voList.outOrderNo, "", "", 1, "已上传", PaymentData,voList.payerId,voList.payerName,"0",voList.tradeNo);
            }
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
    }
}
