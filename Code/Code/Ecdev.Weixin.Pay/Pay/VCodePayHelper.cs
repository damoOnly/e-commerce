using Ecdev.Weixin.Pay.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ThoughtWorks.QRCode.Codec;

namespace Ecdev.Weixin.Pay.Pay
{
    public class VCodePayHelper
    {
        /// <summary>
        /// 创建微信支付 地址
        /// </summary>
        public static readonly string prepay_id_Url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        /// <summary>
        /// 查询订单地址
        /// </summary>
        public static readonly string wx_OrderQuery_Url = "https://api.mch.weixin.qq.com/pay/orderquery";
        /// <summary>
        /// 创建退款
        /// </summary>
        public static readonly string wx_refundpay = "https://api.mch.weixin.qq.com/secapi/pay/refund";
        public static string key = "5079D043FCF841DB85937886219DCD67";
        /// <summary>
        /// 二维码保存地址
        /// </summary>
        public static string imgPath = "";
        /// <summary>
        /// 创建二维码支付请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static VCodePayResponsEntity PostData(string url, string postData, out string result)
        {
            string text = string.Empty;
            result = "";
            VCodePayResponsEntity wei = new VCodePayResponsEntity();
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
                            text = string.Format("获取信息错误doc.load：{0}", ex.Message) + text;
                        }
                        try
                        {
                            if (xmlDocument == null)
                            {
                                result = text;
                                return null;
                            }
                            XmlNode xmlNode = xmlDocument.SelectSingleNode("xml/return_code");
                            if (xmlNode == null)
                            {
                                result = text;
                                return null;
                            }
                            if (!(xmlNode.InnerText == "SUCCESS"))
                            {
                                result = xmlNode.InnerText;
                                return null;
                            }
                            XmlNode xmlNode2 = xmlDocument.SelectSingleNode("xml/prepay_id");
                            if (xmlNode2 != null)
                            {
                                wei.prepay_id = xmlNode2.InnerText;

                            }
                            xmlNode2 = xmlDocument.SelectSingleNode("xml/appid");
                            if (xmlNode2 != null)
                            {
                                wei.appid = xmlNode2.InnerText;

                            }
                            xmlNode2 = xmlDocument.SelectSingleNode("xml/mch_id");
                            if (xmlNode2 != null)
                            {
                                wei.mch_id = xmlNode2.InnerText;

                            }
                            xmlNode2 = xmlDocument.SelectSingleNode("xml/device_info");
                            if (xmlNode2 != null)
                            {
                                wei.device_info = xmlNode2.InnerText;

                            }
                            xmlNode2 = xmlDocument.SelectSingleNode("xml/nonce_str");
                            if (xmlNode2 != null)
                            {
                                wei.Nonce_str = xmlNode2.InnerText;

                            }
                            xmlNode2 = xmlDocument.SelectSingleNode("xml/sign");
                            if (xmlNode2 != null)
                            {
                                wei.sign = xmlNode2.InnerText;

                            }
                            xmlNode2 = xmlDocument.SelectSingleNode("xml/trade_type");
                            if (xmlNode2 != null)
                            {
                                wei.trade_type = xmlNode2.InnerText;

                            }
                            xmlNode2 = xmlDocument.SelectSingleNode("xml/code_url");
                            if (xmlNode2 != null)
                            {
                                wei.code_url = xmlNode2.InnerText;

                            }
                            return wei;
                        }
                        catch (Exception ex)
                        {
                            text = string.Format("获取信息错误node.load：{0}", ex.Message) + text;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Format("获取信息错误post error：{0}", ex.Message) + text;

            }
            return null;
        }
        public static string GetOrderState(Dictionary<string, string> dic, string key1, out string transaction_id)
        {
            string sign = GetResultSign(dic, (string.IsNullOrWhiteSpace(key1) ? key : key1));
            dic.Add("sign", sign);
            string reqtest = BuildXml(dic, false);
            string msg;
            return GetOrderState(VCodePayHelper.wx_OrderQuery_Url, reqtest, out msg, out transaction_id).ToString();
        }
        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key1"></param>
        /// <param name="transaction_id"></param>
        /// <returns></returns>
        public static string RequestPayreFund(Dictionary<string, string> dic, string key1, out string transaction_id)
        {
            string sign = GetResultSign(dic, (string.IsNullOrWhiteSpace(key1) ? key : key1));
            dic.Add("sign", sign);
            string reqtest = BuildXml_RM(dic, false);
            string msg;
            return GetOrderState(VCodePayHelper.wx_refundpay, reqtest, out msg, out transaction_id).ToString();
        }
        private static Trade_state GetOrderState(string url, string postData, out string result, out string transaction_id)
        {
            string text = string.Empty;
            transaction_id = string.Empty;
            result = "";
            VCodePayResponsEntity wei = new VCodePayResponsEntity();
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
                            text = string.Format("获取信息错误doc.load：{0}", ex.Message) + text;
                        }
                        try
                        {
                            if (xmlDocument == null)
                            {
                                result = text;
                                return Trade_state.Error;
                            }
                            XmlNode xmlNode = xmlDocument.SelectSingleNode("xml/return_code");
                            if (xmlNode == null)
                            {
                                result = text;
                                return Trade_state.Error;
                            }
                            if (!(xmlNode.InnerText == "SUCCESS"))
                            {
                                result = xmlNode.InnerText;
                                return Trade_state.Error;
                            }
                            xmlNode = xmlDocument.SelectSingleNode("xml/trade_state");
                            if (xmlNode != null)
                            {
                                switch (xmlNode.InnerText)
                                {
                                    case "SUCCESS":
                                        xmlNode = xmlDocument.SelectSingleNode("xml/transaction_id");
                                        if (xmlNode != null)
                                            transaction_id = xmlNode.InnerText;
                                        return Trade_state.SUCCESS;
                                    case "CLOSED":
                                        return Trade_state.CLOSED;
                                    case "NOTPAY":
                                        return Trade_state.NOTPAY;
                                    case "PAYERROR":
                                        return Trade_state.PAYERROR;
                                    case "REFUND":
                                        return Trade_state.REFUND;
                                    case "REVOKED":
                                        return Trade_state.REVOKED;
                                    case "USERPAYING":
                                        return Trade_state.USERPAYING;
                                }
                                return Trade_state.Error;
                            }

                        }
                        catch (Exception ex)
                        {
                            text = string.Format("获取信息错误node.load：{0}", ex.Message) + text;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Format("获取信息错误post error：{0}", ex.Message) + text;

            }
            return Trade_state.Error;
        }
        /// <summary>
        /// 请求参数转换
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private static string BuildXml_RM(IDictionary<string, string> dict, bool encode)
        {
            SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(dict);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<xml>");
            IEnumerator<KeyValuePair<string, string>> enumerator = sortedDictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, string> current = enumerator.Current;
                string key = current.Key;
                current = enumerator.Current;
                string value = current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    decimal num = 0m;
                    bool flag = false;
                    if (!decimal.TryParse(value, out num))
                    {
                        flag = true;
                    }
                    if (encode)
                    {
                        stringBuilder.AppendLine(string.Concat(new string[]
						{
							"<",
							key,
							">",
							flag ? "" : "",
                            value,
							//HttpUtility.UrlEncode(value, Encoding.UTF8),
							flag ? "" : "",
							"</",
							key,
							">"
						}));
                    }
                    else
                    {
                        stringBuilder.AppendLine(string.Concat(new string[]
						{
							"<",
							key,
							">",
							flag ? "" : "",
							value,
							flag ? "" : "",
							"</",
							key,
							">"
						}));
                    }
                }
            }
            stringBuilder.AppendLine("</xml>");
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 请求参数转换
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private static string BuildXml(IDictionary<string, string> dict, bool encode)
        {
            SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(dict);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<xml>");
            IEnumerator<KeyValuePair<string, string>> enumerator = sortedDictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, string> current = enumerator.Current;
                string key = current.Key;
                current = enumerator.Current;
                string value = current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    decimal num = 0m;
                    bool flag = false;
                    if (!decimal.TryParse(value, out num))
                    {
                        flag = true;
                    }
                    if (encode)
                    {
                        stringBuilder.AppendLine(string.Concat(new string[]
						{
							"<",
							key,
							">",
							flag ? "<![CDATA[" : "",
                            value,
							//HttpUtility.UrlEncode(value, Encoding.UTF8),
							flag ? "]]>" : "",
							"</",
							key,
							">"
						}));
                    }
                    else
                    {
                        stringBuilder.AppendLine(string.Concat(new string[]
						{
							"<",
							key,
							">",
							flag ? "<![CDATA[" : "",
							value,
							flag ? "]]>" : "",
							"</",
							key,
							">"
						}));
                    }
                }
            }
            stringBuilder.AppendLine("</xml>");
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 为对象sign属性赋值
        /// </summary>
        /// <param name="weixin"></param>
        private static string GetResultSign(Dictionary<string, string> dic, string key1)
        {
            string request = "";
            foreach (var s in dic.OrderBy(a => a.Key))
            {
                if (request != "")
                    request += "&" + s.Key + "=" + s.Value;
                else
                    request += s.Key + "=" + s.Value;
            }
            //string stringSignTemo = MD5DAL.MD5Helper.Get32MD5(request);
            string sign = request + "&key=" + (string.IsNullOrWhiteSpace(key1) ? key : key1);

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(32);
            System.Security.Cryptography.MD5 mD = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] array = mD.ComputeHash(System.Text.Encoding.UTF8.GetBytes(sign));
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString().ToUpper();
        }
        private static Dictionary<string, string> EntityToDic(VCodePayEntity wei)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (System.Reflection.PropertyInfo v in wei.GetType().GetProperties())
            {
                string str = "";
                try
                {
                    if (v.Name.ToLower() == "total_fee")
                    {
                        if (v.GetValue(wei, null).ToString() == "0")
                        {
                            str = "";
                        }
                        else
                        {
                            str = v.GetValue(wei, null).ToString();
                        }
                    }
                    else
                        str = v.GetValue(wei, null).ToString();
                }
                catch
                {
                }
                if (!string.IsNullOrWhiteSpace(str))
                {
                    dic.Add(v.Name.ToLower(), str);
                }
            }
            return dic;
        }
        /// <summary>
        /// 改变订单状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool ChangOrderStatus(int status)
        {

            return false;
        }
        /// <summary>
        /// 创建微信支付
        /// </summary>
        /// <param name="wei"></param>
        /// <returns></returns>
        public static VCodePayResponsEntity CreateWeixinPay(VCodePayEntity wei)
        {
            Dictionary<string, string> dic = EntityToDic(wei);
            string sign = GetResultSign(dic,null);
            dic.Add("sign", sign);
            string reqtest = BuildXml(dic, false);
            string msg;
            VCodePayResponsEntity post = PostData(prepay_id_Url, reqtest, out msg);
            return post;
        }
        /// <summary>
        /// 根据订单号创建微信支付
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="appid">appId</param>
        /// <param name="mch_id">mch_id</param>
        /// <param name="notify_url">回调格式 http://haimylife.com/pay/wx_Pay_notify_url.aspx </param>
        /// <param name="spbill_create_ip">服务器Ip</param>
        /// <param name="key1">key</param>
        /// <param name="out_trade_no">out_trade_no</param>
        /// <param name="total">订单金额</param>
        /// <param name="msg">错误信息</param>
        /// <returns>微信响应实体</returns>
        public static VCodePayResponsEntity UnifiedOrder(string orderId, decimal total, string out_trade_no, string appid, string mch_id, string key1, string notify_url, string spbill_create_ip, out string msg)
        {
            VCodePayEntity payEntity = new Ecdev.Weixin.Pay.Domain.VCodePayEntity()
                      {
                          appid = appid,
                          //appid = "wx45cd46b5f561deee",
                          //mch_id = "D4DE77CE6F6C9E3A",
                          mch_id = mch_id,
                          body = orderId,
                          nonce_str = VCodePayHelper.CreateRandom(20),
                          out_trade_no = out_trade_no,
                          fee_type = "CNY",
                          //1 = 1分
                          total_fee = (int)(total * 100),
                          spbill_create_ip = spbill_create_ip,
                          notify_url = notify_url,
                          product_id = orderId,
                          Trade_type = "APP"
                      };
            Dictionary<string, string> dic = EntityToDic(payEntity);
            string sign = GetResultSign(dic,key1);
            dic.Add("sign", sign);
            string reqtest = BuildXml(dic, false);
            VCodePayResponsEntity post = PostData(prepay_id_Url, reqtest, out msg);
            return post;
        }
        /// <summary>
        /// 创建二维码支付请求 返回二维码图片地址 返回""：创建失败
        /// </summary>
        /// <param name="wei">实体</param>
        /// <returns>返回二维码图片地址 返回""：创建失败</returns>
        public static string RequestWeixin(VCodePayEntity wei)
        {
            Dictionary<string, string> dic = EntityToDic(wei);
            string sign = GetResultSign(dic,null);
            dic.Add("sign", sign);
            string reqtest = BuildXml(dic, false);
            string msg;

            VCodePayResponsEntity post = null;

            int loop = 0;

            do
            {
                post = PostData(prepay_id_Url, reqtest, out msg);

                if (post != null)
                {
                    break;
                }

                loop++;

                System.Threading.Thread.Sleep(500);

            } while (loop < 5);

            if (post == null)
            {
                return msg;
            }

            try
            {
                CreateQrcode(post);
                return post.sign + ".jpg";
            }
            catch (Exception)
            {

            }

            return "";
        }
        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="post">微信支付创建返回实体</param>
        /// <param name="imgPath">二维码存放根目录</param>
        private static void CreateQrcode(VCodePayResponsEntity post)
        {

            System.Drawing.Bitmap bt;
            //string qrurl = string.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["EC_Url"]) ? "" : System.Configuration.ConfigurationSettings.AppSettings["EC_Url"].ToString();
            //请求地址带参数


            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            bt = qrCodeEncoder.Encode(post.code_url, Encoding.UTF8);
            //Response.Write(path);
            bt.Save((string.IsNullOrWhiteSpace(imgPath) ? "" : imgPath + "/") + post.sign + ".jpg");
            //return "";

        }
        /// <summary>
        /// 创建随机字符串	nonce_str
        /// </summary>
        /// <param name="length">随机字符长度</param>
        /// <returns></returns>
        public static string CreateRandom(int length)
        {
            Random ran = new Random();
            string str = string.Empty;
            while (length >= 0)
            {
                if (ran.Next(0, 2) == 0)
                {
                    str += (char)ran.Next(65, 91);
                }
                else
                {
                    str += (char)ran.Next(97, 122);
                }
                length--;
            }
            return str;
            //65-90 97-121

        }

    }
    /// <summary>
    /// 订单状态枚举
    /// </summary>
    public enum Trade_state
    {
        /// <summary>
        /// 支付成功
        /// </summary>
        SUCCESS,
        /// <summary>
        /// 转入退款
        /// </summary>
        REFUND,
        /// <summary>
        /// 未支付
        /// </summary>
        NOTPAY,
        /// <summary>
        /// 已关闭
        /// </summary>
        CLOSED,
        /// <summary>
        /// 已撤销（刷卡支付）
        /// </summary>
        REVOKED,
        /// <summary>
        /// 用户支付中
        /// </summary>
        USERPAYING,
        /// <summary>
        /// 支付失败(其他原因，如银行返回失败)
        /// </summary>
        PAYERROR,
        /// <summary>
        /// 信息获取失败
        /// </summary>
        Error,
    }
}
