using Ecdev.Weixin.MP;
using Ecdev.Weixin.Pay.Domain;
using Ecdev.Weixin.Pay.Util;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;

using EcShop.Core.ErrorLog;
using System.Net.Security;

namespace Ecdev.Weixin.Pay
{
    public class PayClient
    {
        public static readonly string Deliver_Notify_Url = "https://api.weixin.qq.com/pay/delivernotify";
        public static readonly string prepay_id_Url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        public static readonly string wx_refund_Url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
        public static readonly string wx_DownloadBill_Url = "https://api.mch.weixin.qq.com/pay/downloadbill";
        public static readonly string wx_OrderQuery_Url = "https://api.mch.weixin.qq.com/pay/orderquery";
        private PayAccount _payAccount;
        public PayClient(string appId, string appSecret, string partnerId, string partnerKey, string paySignKey)
        {
            this._payAccount = new PayAccount
            {
                AppId = appId,
                AppSecret = appSecret,
                PartnerId = partnerId,
                PartnerKey = partnerKey,
                PaySignKey = paySignKey
            };
        }
        public PayClient(PayAccount account)
            : this(account.AppId, account.AppSecret, account.PartnerId, account.PartnerKey, account.PaySignKey)
        {
        }
        internal string BuildPackage(PackageInfo package)
        {
            PayDictionary payDictionary = new PayDictionary();
            payDictionary.Add("appid", this._payAccount.AppId);
            payDictionary.Add("mch_id", this._payAccount.PartnerId);
            payDictionary.Add("device_info", "");
            payDictionary.Add("nonce_str", Utils.CreateNoncestr());
            payDictionary.Add("body", package.Body);
            payDictionary.Add("attach", "");
            payDictionary.Add("out_trade_no", package.OutTradeNo);
            payDictionary.Add("total_fee", (int)package.TotalFee);
            payDictionary.Add("spbill_create_ip", package.SpbillCreateIp);
            payDictionary.Add("time_start", package.TimeExpire);
            payDictionary.Add("time_expire", "");
            payDictionary.Add("goods_tag", package.GoodsTag);
            payDictionary.Add("notify_url", package.NotifyUrl);
            payDictionary.Add("trade_type", "JSAPI");
            payDictionary.Add("openid", package.OpenId);
            payDictionary.Add("product_id", "");
            string sign = SignHelper.SignPackage(payDictionary, this._payAccount.PartnerKey);
            string text = this.GetPrepay_id(payDictionary, sign);
            if (text.Length > 64)
            {
                text = "";
            }
            return string.Format("prepay_id=" + text, new object[0]);
        }
        internal string BuildRefundPackage(PackageInfo package)
        {
            PayDictionary payDictionary = new PayDictionary();
            payDictionary.Add("appid", this._payAccount.AppId);
            payDictionary.Add("mch_id", this._payAccount.PartnerId);
            //payDictionary.Add("device_info", "");
            payDictionary.Add("nonce_str", Utils.CreateNoncestr());
            payDictionary.Add("transaction_id", package.TransactionId);
            payDictionary.Add("out_trade_no", package.OutTradeNo);
            payDictionary.Add("out_refund_no", package.OutRefundNo);
            payDictionary.Add("total_fee", (int)package.TotalFee);
            payDictionary.Add("refund_fee", (int)package.RefundFee);
            payDictionary.Add("op_user_id", this._payAccount.PartnerId);

            string sign = SignHelper.SignPackage(payDictionary, this._payAccount.PartnerKey);

            payDictionary.Add("sign", sign);
            return SignHelper.BuildXml(payDictionary, false);
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
        /// <summary>
        /// 微信申请退款
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public string RequestRefund(PackageInfo package)
        {
            string postData = BuildRefundPackage(package);
            ErrorLog.Write("【" + package.OutTradeNo + "】【退款】参数：" + postData);
            X509Store store = new X509Store("My", StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            string certpath = @"D:\Plugins\apiclient_cert.p12";
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            X509Certificate cer = new X509Certificate(certpath, this._payAccount.PartnerId);

            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(wx_refund_Url);
            webrequest.ClientCertificates.Add(cer);
            webrequest.Method = "post";
            webrequest.KeepAlive = true;
            webrequest.ContentType = "text/xml";
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            Stream requestStream = webrequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            HttpWebResponse webreponse = (HttpWebResponse)webrequest.GetResponse();

            Stream stream = webreponse.GetResponseStream();
            string text = string.Empty;
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
                ErrorLog.Write("【" + package.OutTradeNo + "】【退款结果】" + text);
            }
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(text);
            }
            catch (Exception ex)
            {
                text = string.Format("微信获取信息错误doc.load：{0}", ex.Message) + text;
                ErrorLog.Write(text);
            }
            string result = string.Empty;
            try
            {
                if (xmlDocument == null)
                {
                    result = text;
                    return result;
                }
                XmlNode xmlNode = xmlDocument.SelectSingleNode("xml/return_code");
                if (xmlNode == null)
                {
                    result = text;
                    return result;
                }
                if (!(xmlNode.InnerText == "SUCCESS"))
                {
                    text = string.Format("微信获取信息失败：{0}", text);
                    ErrorLog.Write(text);
                    result = xmlNode.InnerText;
                    return result;
                }
                XmlNode xmlNode2 = xmlDocument.SelectSingleNode("xml/result_code");
                if (!(xmlNode2.InnerText == "SUCCESS"))
                {
                    result = xmlNode2.InnerText;
                    xmlNode2 = xmlDocument.SelectSingleNode("xml/err_code_des");
                    return result + ":" + xmlNode2.InnerText;
                }
            }
            catch (Exception ex)
            {
                text = string.Format("微信获取信息错误node.load：{0}", ex.Message) + text;
                ErrorLog.Write(text);
            }

            return text;
        }
        //对账单请求数据
        public string  BuildBillPackage(string strDateTime,BillType billType)
        {         
            PayDictionary payDictionary = new PayDictionary();
            payDictionary.Add("appid", this._payAccount.AppId);
            payDictionary.Add("mch_id",this._payAccount.PartnerId);
            payDictionary.Add("nonce_str",Utils.CreateNoncestr());
            payDictionary.Add("bill_date",strDateTime);
            payDictionary.Add("bill_type", billType.ToString());
            string sign = SignHelper.SignPackage(payDictionary, this._payAccount.PartnerKey);

            payDictionary.Add("sign", sign);
            return SignHelper.BuildXml(payDictionary, false);
        }
        public string downloadBill(string postData)
        {
            string billInfo = string.Empty;
            if (!string.IsNullOrEmpty(postData))
            {
                billInfo = new WebUtils().DoPostByXml(wx_DownloadBill_Url, postData);
            }
            return billInfo;
        }
        //查询订单状态
        public string BuildOrderPackage(string orderId)
        {
            PayDictionary payDictionary = new PayDictionary();
            payDictionary.Add("appid", this._payAccount.AppId);
            payDictionary.Add("mch_id", this._payAccount.PartnerId);
            payDictionary.Add("nonce_str", Utils.CreateNoncestr());
            payDictionary.Add("out_trade_no", orderId);
            string sign = SignHelper.SignPackage(payDictionary, this._payAccount.PartnerKey);
            payDictionary.Add("sign", sign);
            return SignHelper.BuildXml(payDictionary, false);
        }
        public string RequestOrder(string postDate)
        {
            string orderInfo = string.Empty;
            if (string.IsNullOrEmpty(postDate))
            {
                return orderInfo;
            }
            orderInfo = new WebUtils().DoPostByXml(wx_OrderQuery_Url, postDate);
            return orderInfo;
        }
        public PayRequestInfo BuildPayRequest(PackageInfo package)
        {
            PayRequestInfo payRequestInfo = new PayRequestInfo();
            payRequestInfo.appId = this._payAccount.AppId;
            payRequestInfo.package = this.BuildPackage(package);
            payRequestInfo.timeStamp = Utils.GetCurrentTimeSeconds().ToString();
            payRequestInfo.nonceStr = Utils.CreateNoncestr();
            payRequestInfo.paySign = SignHelper.SignPay(new PayDictionary
			{

				{
					"appId",
					this._payAccount.AppId
				},

				{
					"timeStamp",
					payRequestInfo.timeStamp
				},

				{
					"package",
					payRequestInfo.package
				},

				{
					"nonceStr",
					payRequestInfo.nonceStr
				},

				{
					"signType",
					"MD5"
				}
			}, this._payAccount.PartnerKey);
            return payRequestInfo;
        }
        public bool DeliverNotify(DeliverInfo deliver)
        {
            string token = Utils.GetToken(this._payAccount.AppId, this._payAccount.AppSecret);
            return this.DeliverNotify(deliver, token);
        }
        public bool DeliverNotify(DeliverInfo deliver, string token)
        {
            PayDictionary payDictionary = new PayDictionary();
            payDictionary.Add("appid", this._payAccount.AppId);
            payDictionary.Add("openid", deliver.OpenId);
            payDictionary.Add("transid", deliver.TransId);
            payDictionary.Add("out_trade_no", deliver.OutTradeNo);
            payDictionary.Add("deliver_timestamp", Utils.GetTimeSeconds(deliver.TimeStamp));
            payDictionary.Add("deliver_status", deliver.Status ? 1 : 0);
            payDictionary.Add("deliver_msg", deliver.Message);
            deliver.AppId = this._payAccount.AppId;
            deliver.AppSignature = SignHelper.SignPay(payDictionary, "");
            payDictionary.Add("app_signature", deliver.AppSignature);
            payDictionary.Add("sign_method", deliver.SignMethod);
            string data = JsonConvert.SerializeObject(payDictionary);
            string url = string.Format("{0}?access_token={1}", PayClient.Deliver_Notify_Url, token);
            string text = new WebUtils().DoPost(url, data);
            return !string.IsNullOrEmpty(text) && text.Contains("ok");
        }
        internal string GetPrepay_id(PayDictionary dict, string sign)
        {
            dict.Add("sign", sign);
            string query = SignHelper.BuildQuery(dict, false);
            string param = SignHelper.BuildXml(dict, false);
            string prepayId = PayClient.PostData(PayClient.prepay_id_Url, param);

            ErrorLog.Write(string.Format("获取预支付Id：OperTime:{0},Info:{1},param:{2},query:{3}", DateTime.Now, prepayId, param, query));

            return prepayId;
        }
        public static string PostData(string url, string postData)
        {
            string text = string.Empty;
            string result;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
                Encoding uTF = Encoding.UTF8;
                byte[] bytes = uTF.GetBytes(postData);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "text/xml";
                httpWebRequest.ContentLength = (long)postData.Length;
                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(postData);
                }
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        Encoding uTF2 = Encoding.UTF8;
                        StreamReader streamReader = new StreamReader(responseStream, uTF2);
                        text = streamReader.ReadToEnd();
                        XmlDocument xmlDocument = new XmlDocument();
                        try
                        {
                            xmlDocument.LoadXml(text);
                        }
                        catch (Exception ex)
                        {
                            text = string.Format("微信获取信息错误doc.load：{0}", ex.Message) + text;

                            ErrorLog.Write(text);
                        }
                        try
                        {
                            if (xmlDocument == null)
                            {
                                result = text;
                                return result;
                            }
                            XmlNode xmlNode = xmlDocument.SelectSingleNode("xml/return_code");
                            if (xmlNode == null)
                            {
                                result = text;
                                return result;
                            }
                            if (!(xmlNode.InnerText == "SUCCESS"))
                            {
                                //
                                text = string.Format("微信获取信息失败：{0}", text);
                                ErrorLog.Write(text);
                                //
                                result = xmlNode.InnerText;
                                return result;
                            }
                            XmlNode xmlNode2 = xmlDocument.SelectSingleNode("xml/prepay_id");
                            if (xmlNode2 != null)
                            {
                                result = xmlNode2.InnerText;
                                return result;
                            }
                        }
                        catch (Exception ex)
                        {
                            text = string.Format("微信获取信息错误node.load：{0}", ex.Message) + text;
                            ErrorLog.Write(text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                text = string.Format("微信获取信息错误post error：{0}", ex.Message) + text;
                ErrorLog.Write(text);
            }
            result = text;
            return result;
        }
    }
}
