using Ecdev.Weixin.Pay.Domain;
using Ecdev.Weixin.Pay.Notify;
using Ecdev.Weixin.Pay.Util;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
namespace Ecdev.Weixin.Pay
{
	public class NotifyClient
	{
		public static readonly string Update_Feedback_Url = "https://api.weixin.qq.com/payfeedback/update";
		private PayAccount _payAccount;
		public NotifyClient(string appId, string appSecret, string partnerId, string partnerKey, string paySignKey)
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
		public NotifyClient(PayAccount account) : this(account.AppId, account.AppSecret, account.PartnerId, account.PartnerKey, account.PaySignKey)
		{
		}
		private string ReadString(Stream inStream)
		{
			string result;
			if (inStream == null)
			{
				result = null;
			}
			else
			{
				byte[] array = new byte[inStream.Length];
				inStream.Read(array, 0, array.Length);
				result = Encoding.UTF8.GetString(array);
			}
			return result;
		}
		private bool ValidPaySign(PayNotify notify, out string servicesign)
		{
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("appid", notify.appid);
			payDictionary.Add("bank_type", notify.bank_type);
			payDictionary.Add("cash_fee", notify.cash_fee);
            payDictionary.Add("device_info", notify.device_info);
			payDictionary.Add("fee_type", notify.fee_type);
			payDictionary.Add("is_subscribe", notify.is_subscribe);
			payDictionary.Add("mch_id", notify.mch_id);
			payDictionary.Add("nonce_str", notify.nonce_str);
			payDictionary.Add("openid", notify.openid);
			payDictionary.Add("out_trade_no", notify.out_trade_no);
			payDictionary.Add("result_code", notify.result_code);
			payDictionary.Add("return_code", notify.return_code);
			//payDictionary.Add("sub_mch_id", notify.sub_mch_id);
			payDictionary.Add("time_end", notify.time_end);
			payDictionary.Add("total_fee", notify.total_fee);
			payDictionary.Add("trade_type", notify.trade_type);
			payDictionary.Add("transaction_id", notify.transaction_id);
			servicesign = SignHelper.SignPay(payDictionary, this._payAccount.PartnerKey);
			bool result = notify.sign == servicesign;
			servicesign = servicesign + "-" + SignHelper.BuildQuery(payDictionary, false);
			return result;
		}
		private bool ValidAlarmSign(AlarmNotify notify)
		{
			return true;
		}
		private bool ValidFeedbackSign(FeedBackNotify notify)
		{
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("appid", this._payAccount.AppId);
			payDictionary.Add("timestamp", notify.TimeStamp);
			payDictionary.Add("openid", notify.OpenId);
			return notify.AppSignature == SignHelper.SignPay(payDictionary, "");
		}
		public PayNotify GetPayNotify(Stream inStream)
		{
			string xml = this.ReadString(inStream);
			return this.GetPayNotify(xml);
		}
		public DataTable ErrorTable(string tabName = "Notify")
		{
			return new DataTable
			{
				Columns = 
				{
					new DataColumn("OperTime"),
					new DataColumn("Error"),
					new DataColumn("Param"),
					new DataColumn("PayInfo")
				},
				TableName = tabName
			};
		}
		public void WriteLog(DataTable dt)
		{
			dt.WriteXml(HttpContext.Current.Request.MapPath("/NotifyError.xml"));
		}
        public OrderNotify GetOrderStatusNotify(string xml)
        {
            OrderNotify result = null;
            try
            {
                if (string.IsNullOrEmpty(xml))
                {
                    result = null;
                }
                else
                {
                    result = Utils.GetNotifyObject<OrderNotify>(xml);
                }
            }
            catch
            { 
                
            }
            return result;
        }
		public PayNotify GetPayNotify(string xml)
		{
			DataTable dataTable = this.ErrorTable("Notify");
			DataRow dataRow = dataTable.NewRow();
			dataRow["OperTime"] = DateTime.Now;
			PayNotify result;
			try
			{
				if (string.IsNullOrEmpty(xml))
				{
					result = null;
				}
				else
				{
					PayNotify notifyObject = Utils.GetNotifyObject<PayNotify>(xml);
					string text = "";
					if (notifyObject == null || !this.ValidPaySign(notifyObject, out text))
					{
						dataRow["Error"] = ((notifyObject == null) ? "Notify Null" : "Valid pay Sign Error");
						dataRow["Param"] = xml;
						dataRow["PayInfo"] = string.Concat(new string[]
						{
							notifyObject.out_trade_no,
							"-",
							text,
							"-",
							this._payAccount.PartnerKey
						});
						dataTable.Rows.Add(dataRow);
						this.WriteLog(dataTable);
						result = null;
					}
					else
					{
						notifyObject.PayInfo = new PayInfo
						{
							SignType = "MD5",
							Sign = notifyObject.sign,
							TradeMode = 0,
							BankType = notifyObject.bank_type,
							BankBillNo = "",
							TotalFee = notifyObject.total_fee / 100m,
							FeeType = (notifyObject.fee_type == "CNY") ? 1 : 0,
							NotifyId = "",
							TransactionId = notifyObject.transaction_id,
							OutTradeNo = notifyObject.out_trade_no,
							TransportFee = 0m,
							ProductFee = 0m,
							Discount = 1m,
							BuyerAlias = ""
						};
						result = notifyObject;
					}
				}
			}
			catch (Exception ex)
			{
				dataRow["Error"] = ex.Message;
				dataRow["Param"] = xml;
				dataTable.Rows.Add(dataRow);
				this.WriteLog(dataTable);
				result = null;
			}
			return result;
		}
		public AlarmNotify GetAlarmNotify(Stream inStream)
		{
			string xml = this.ReadString(inStream);
			return this.GetAlarmNotify(xml);
		}
		public AlarmNotify GetAlarmNotify(string xml)
		{
			AlarmNotify result;
			if (string.IsNullOrEmpty(xml))
			{
				result = null;
			}
			else
			{
				AlarmNotify notifyObject = Utils.GetNotifyObject<AlarmNotify>(xml);
				if (notifyObject == null || !this.ValidAlarmSign(notifyObject))
				{
					result = null;
				}
				else
				{
					result = notifyObject;
				}
			}
			return result;
		}
		public FeedBackNotify GetFeedBackNotify(Stream inStream)
		{
			string xml = this.ReadString(inStream);
			return this.GetFeedBackNotify(xml);
		}
		public FeedBackNotify GetFeedBackNotify(string xml)
		{
			FeedBackNotify result;
			if (string.IsNullOrEmpty(xml))
			{
				result = null;
			}
			else
			{
				FeedBackNotify notifyObject = Utils.GetNotifyObject<FeedBackNotify>(xml);
				if (notifyObject == null || !this.ValidFeedbackSign(notifyObject))
				{
					result = null;
				}
				else
				{
					result = notifyObject;
				}
			}
			return result;
		}
		public bool UpdateFeedback(string feedbackid, string openid)
		{
			string token = Utils.GetToken(this._payAccount.AppId, this._payAccount.AppSecret);
			return this.UpdateFeedback(feedbackid, openid, token);
		}
		public bool UpdateFeedback(string feedbackid, string openid, string token)
		{
			string url = string.Format("{0}?access_token={1}&openid={2}&feedbackid={3}", new object[]
			{
				NotifyClient.Update_Feedback_Url,
				token,
				openid,
				feedbackid
			});
			string text = new WebUtils().DoGet(url);
			return !string.IsNullOrEmpty(text) && text.Contains("ok");
		}
	}
}
