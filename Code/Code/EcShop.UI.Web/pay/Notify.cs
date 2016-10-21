using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Entities.Sales;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
namespace EcShop.UI.Web.pay
{
	public class Notify
	{
		private string _partner = "";
		private string _public_key = "";
		private string _input_charset = "";
		private string _sign_type = "";
		private string Https_veryfy_url = "https://mapi.alipay.com/gateway.do?service=notify_verify&";
		public Notify()
		{
			PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.ws_apppay.wswappayrequest");
			string xml = HiCryptographer.Decrypt(paymentMode.Settings);
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.LoadXml(xml);
			this._partner = xmlDocument.GetElementsByTagName("Partner")[0].InnerText;
            this._public_key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";
			this._input_charset = "utf-8";
			this._sign_type = "RSA";
		}
		public bool Verify(System.Collections.Generic.SortedDictionary<string, string> inputPara, string notify_id, string sign)
		{
			bool signVeryfy = this.GetSignVeryfy(inputPara, sign);
			string a = "true";
			if (notify_id != null && notify_id != "")
			{
				a = this.GetResponseTxt(notify_id);
			}
			return a == "true" && signVeryfy;
		}
		private string GetPreSignStr(System.Collections.Generic.SortedDictionary<string, string> inputPara)
		{
			System.Collections.Generic.Dictionary<string, string> dicArray = new System.Collections.Generic.Dictionary<string, string>();
			dicArray = Core.FilterPara(inputPara);
			return Core.CreateLinkString(dicArray);
		}
		private bool GetSignVeryfy(System.Collections.Generic.SortedDictionary<string, string> inputPara, string sign)
		{
			System.Collections.Generic.Dictionary<string, string> dicArray = new System.Collections.Generic.Dictionary<string, string>();
			dicArray = Core.FilterPara(inputPara);
			string content = Core.CreateLinkString(dicArray);
			bool result = false;
			string sign_type;
			if (sign != null && sign != "" && (sign_type = this._sign_type) != null && sign_type == "RSA")
			{
				result = RSAFromPkcs8.verify(content, sign, this._public_key, this._input_charset);
			}
			return result;
		}
		private string GetResponseTxt(string notify_id)
		{
			string strUrl = string.Concat(new string[]
			{
				this.Https_veryfy_url,
				"partner=",
				this._partner,
				"&notify_id=",
				notify_id
			});
			return this.Get_Http(strUrl, 120000);
		}
		private string Get_Http(string strUrl, int timeout)
		{
			string result;
			try
			{
				System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strUrl);
				httpWebRequest.Timeout = timeout;
				System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
				System.IO.Stream responseStream = httpWebResponse.GetResponseStream();
				System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.Default);
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				while (-1 != streamReader.Peek())
				{
					stringBuilder.Append(streamReader.ReadLine());
				}
				result = stringBuilder.ToString();
			}
			catch (System.Exception ex)
			{
				result = "错误：" + ex.Message;
			}
			return result;
		}
	}
}
