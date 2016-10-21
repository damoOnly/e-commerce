using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Messages;
using EcShop.SaleSystem.Member;
using Ecdev.Plugins;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
namespace EcShop.UI.Web.API
{
	[WebService(Namespace = "http://tempuri.org/"), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class OrdersHandler : System.Web.IHttpHandler
	{
		private string erromsg = "";
		private int pagesize = 10;
		private string fomat = "json";
		private string ModelId = "";
		private string key = "";
		private string localcode = "";
		private string jsonformat = "";
		private System.Collections.Generic.SortedDictionary<string, string> tmpParas = new System.Collections.Generic.SortedDictionary<string, string>();
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			string text = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><trade_get_response>{0}</trade_get_response>";
			string str = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
			this.fomat = "json";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			this.jsonformat = "{{\"Oid\": \"{0}\",\"SellerUid\": \"{1}\",\"BuyerNick\": \"{2}\",\"BuyerEmail\": \"{3}\",\"ReceiverName\": \"{4}\",\"ReceiverState\": \"{5}\",\"ReceiverCity\": \"{6}\",\"ReceiverDistrict\":\"{7}\",\"ReceiverAddress\":\"{8}\",\"ReceiverZip\": \"{9}\",\"ReceiverMobile\": \"{10}\",\"ReceiverPhone\":\"{11}\",\"BuyerMemo\": \"{12}\",\"OrderMark\":\"{13}\",\"SellerMemo\":\"{14}\",\"Nums\":\"{15}\",\"Price\": \"{16}\",\"Payment\":\"{17}\",\"PaymentType\": \"{18}\",\"PaymentTypeId\":\"{19}\",\"PaymentName\":\"{20}\",\"PostFee\": \"{21}\",\"DiscountFee\": \"{22}\",\"AdjustFee\": \"{23}\",\"PaymentTs\": \"{24}\",\"SentTs\":\"{25}\",\"RefundStatus\":\"{26}\",\"RefundAmount\":\"{27}\",\"RefundRemark\":\"{28}\",\"Status\": \"{29}\",\"ModeName\": \"{30}\",\"CreateTs\":\"{31}\",\"orders\": [{32}]}},";
			string field = "action";
			string orderitemfomat = "{{\"Tid\": \"{0}\",\"Oid\": \"{1}\",\"GoodsIid\": \"{2}\",\"Title\": \"{3}\",\"OuterId\": \"{4}\",\"SkuId\":\"{5}\",\"SKUContent\": \"{6}\",\"Nums\": \"{7}\",\"Price\": \"{8}\",\"Payment\":\"{9}\",\"ThumUrl\":\"{10}\"}},";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			this.ModelId = context.Request.Form["action"].ToString();
			field = "sign";
			this.key = context.Request.Form["sign"];
			this.localcode = masterSettings.CheckCode;
			new System.Collections.Generic.Dictionary<string, string>();
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			try
			{
				if (!string.IsNullOrEmpty(this.ModelId))
				{
					string modelId;
					if ((modelId = this.ModelId) != null)
					{
						if (!(modelId == "tradelist"))
						{
							if (!(modelId == "tradedetails"))
							{
								if (!(modelId == "send"))
								{
									if (modelId == "mark")
									{
										string value = context.Request.Form["order_mark"].Trim();
										string text2 = context.Request.Form["seller_memo"].Trim();
										if (string.IsNullOrEmpty(context.Request.Form["tid"].Trim()) || string.IsNullOrEmpty(value) || string.IsNullOrEmpty(text2))
										{
											this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "tid or order_mark or seller_memo");
											goto IL_9F1;
										}
										if (System.Convert.ToInt32(value) <= 0 || System.Convert.ToInt32(value) >= 7)
										{
											this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "order_mark");
											goto IL_9F1;
										}
										string text3 = context.Request.Form["tid"].Trim();
										this.tmpParas.Add("tid", text3);
										this.tmpParas.Add("order_mark", value);
										this.tmpParas.Add("seller_memo", text2);
										this.tmpParas.Add("format", this.fomat);
										this.tmpParas.Add("action", this.ModelId);
										if (!APIHelper.CheckSign(this.tmpParas, this.localcode, this.key))
										{
											this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
											goto IL_9F1;
										}
										OrderInfo orderInfo = OrderHelper.GetOrderInfo(text3);
										orderInfo.ManagerMark = new OrderMark?((OrderMark)System.Enum.Parse(typeof(OrderMark), value, true));
										orderInfo.ManagerRemark = Globals.HtmlEncode(text2);
										if (!OrderHelper.SaveRemark(orderInfo))
										{
											this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Paramter_Error, "save is failure ");
											goto IL_9F1;
										}
										if (this.fomat == "json")
										{
											text = string.Format("{{\"trade_get_response\":{{\"trade\":{0}}}}}", this.GetOrderDetails(this.jsonformat, orderitemfomat, orderInfo));
											goto IL_9F1;
										}
										goto IL_9F1;
									}
								}
								else
								{
									string text4 = context.Request.Form["tid"].Trim();
									string text5 = context.Request.Form["out_sid"].Trim();
									string text6 = context.Request.Form["company_code"].Trim();
									if (string.IsNullOrEmpty(text4) || string.IsNullOrEmpty(text6) || string.IsNullOrEmpty(text5))
									{
										this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "paramters");
										goto IL_9F1;
									}
									this.tmpParas.Add("tid", text4);
									this.tmpParas.Add("out_sid", text5);
									this.tmpParas.Add("company_code", text6);
									this.tmpParas.Add("format", this.fomat);
									this.tmpParas.Add("action", this.ModelId);
									if (!APIHelper.CheckSign(this.tmpParas, this.localcode, this.key))
									{
										this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
										goto IL_9F1;
									}
									ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNodeByCode(text6);
									if (string.IsNullOrEmpty(expressCompanyInfo.Name))
									{
										this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.NoExists_Error, "company_code");
										goto IL_9F1;
									}
									ShippingModeInfo shippingModeByCompany = SalesHelper.GetShippingModeByCompany(expressCompanyInfo.Name);
									OrderInfo orderInfo2 = OrderHelper.GetOrderInfo(text4);
									if (orderInfo2 == null)
									{
										this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.NoExists_Error, "tid");
										goto IL_9F1;
									}
									ApiErrorCode apiErrorCode = this.SendOrders(orderInfo2, shippingModeByCompany, text5, expressCompanyInfo);
									if (apiErrorCode != ApiErrorCode.Success)
									{
										this.erromsg = MessageInfo.ShowMessageInfo(apiErrorCode, "It");
										goto IL_9F1;
									}
									orderInfo2 = OrderHelper.GetOrderInfo(text4);
									if (this.fomat == "json")
									{
										text = string.Format("{{\"trade_get_response\":{{\"trade\":{0}}}}}", this.GetOrderDetails(this.jsonformat, orderitemfomat, orderInfo2));
										goto IL_9F1;
									}
									goto IL_9F1;
								}
							}
							else
							{
								if (string.IsNullOrEmpty(context.Request.Form["tid"].Trim()))
								{
									this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "tid");
									goto IL_9F1;
								}
								string text3 = context.Request.Form["tid"].Trim();
								this.tmpParas = new System.Collections.Generic.SortedDictionary<string, string>();
								this.tmpParas.Add("tid", context.Request.Form["tid"]);
								this.tmpParas.Add("format", this.fomat);
								this.tmpParas.Add("action", this.ModelId);
								if (!APIHelper.CheckSign(this.tmpParas, this.localcode, this.key))
								{
									this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "signature");
									goto IL_9F1;
								}
								string text7 = context.Request.Form["tid"].Replace("\r\n", "\n");
								if (string.IsNullOrEmpty(text7))
								{
									this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "tid");
									goto IL_9F1;
								}
								text3 = text7;
								OrderInfo orderInfo3 = TradeHelper.GetOrderInfo(text3);
								if (this.fomat == "json")
								{
									text = string.Format("{{\"trade_get_response\":{{\"trade\":{0}}}}}", this.GetOrderDetails(this.jsonformat, orderitemfomat, orderInfo3));
									goto IL_9F1;
								}
								goto IL_9F1;
							}
						}
						else
						{
							OrderQuery orderQuery = new OrderQuery
							{
								PageSize = 100
							};
							int num = 0;
							string text8 = context.Request.Form["status"].Trim();
							string value2 = context.Request.Form["pagesize"].Trim();
							string text9 = context.Request.Form["datatype"].Trim();
							string text10 = context.Request.Form["buyernick"].Trim();
							string value3 = context.Request.Form["pageindex"].Trim();
							string value4 = context.Request.Form["starttime"].Trim();
							string value5 = context.Request.Form["endtime"].Trim();
							if (!string.IsNullOrEmpty(text8) && System.Convert.ToInt32(text8) >= 0 && text8 != "6")
							{
								orderQuery.Status = (OrderStatus)System.Enum.Parse(typeof(OrderStatus), text8, true);
							}
							else
							{
								if (text8 == "6")
								{
									orderQuery.Status = OrderStatus.All;
									orderQuery.RefundState = (RefundStatus)System.Enum.Parse(typeof(RefundStatus), text8, true);
								}
								else
								{
									this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "status");
								}
							}
							if (!string.IsNullOrEmpty(value3) && System.Convert.ToInt32(value3) > 0)
							{
								orderQuery.PageIndex = System.Convert.ToInt32(value3);
							}
							else
							{
								this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "pageindex");
							}
							if (!string.IsNullOrEmpty(value2) && System.Convert.ToInt32(value2) > 0)
							{
								orderQuery.PageSize = (int)System.Convert.ToInt16(value2);
							}
							if (!string.IsNullOrEmpty(text9) && System.Convert.ToInt32(text9) > 0)
							{
								orderQuery.DataType = int.Parse(text9);
							}
							if (!string.IsNullOrEmpty(this.erromsg))
							{
								this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "paramter");
								goto IL_9F1;
							}
							this.tmpParas.Add("status", text8);
							this.tmpParas.Add("datatype", text9);
							this.tmpParas.Add("buyernick", text10);
							this.tmpParas.Add("pageindex", value3);
							this.tmpParas.Add("pagesize", value2);
							this.tmpParas.Add("starttime", value4);
							this.tmpParas.Add("endtime", value5);
							this.tmpParas.Add("format", this.fomat);
							this.tmpParas.Add("action", this.ModelId);
							if (!APIHelper.CheckSign(this.tmpParas, this.localcode, this.key))
							{
								this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
								goto IL_9F1;
							}
							if (!string.IsNullOrEmpty(text10))
							{
								orderQuery.UserName = text10;
							}
							if (!string.IsNullOrEmpty(value4))
							{
								orderQuery.StartDate = new System.DateTime?(System.Convert.ToDateTime(value4));
							}
							if (!string.IsNullOrEmpty(value5))
							{
								orderQuery.EndDate = new System.DateTime?(System.Convert.ToDateTime(value5));
							}
							string arg = stringBuilder.Append(this.GetOrderList(orderQuery, this.jsonformat, orderitemfomat, out num).ToString()).ToString();
							if (this.fomat == "json")
							{
								text = string.Format("{{\"trade_get_response\":{{\"trade\":[{0}],\"total\":{1}}}}}", arg, num);
								goto IL_9F1;
							}
							goto IL_9F1;
						}
					}
					this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Paramter_Error, "paramters");
				}
				else
				{
					this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Paramter_Error, "sign");
				}
				IL_9F1:;
			}
			catch (System.Exception)
			{
				this.erromsg = MessageInfo.ShowMessageInfo(ApiErrorCode.Unknown_Error, field);
			}
			if (!string.IsNullOrEmpty(this.erromsg))
			{
				text = str + this.erromsg;
				if (this.fomat == "json")
				{
					text = text.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>", "");
					xmlDocument.Load(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(text)));
					text = JsonConvert.SerializeXmlNode(xmlDocument);
				}
			}
			context.Response.ContentType = "text/json";
			context.Response.Write(text);
		}
		public System.Text.StringBuilder GetOrderList(OrderQuery query, string format, string orderitemfomat, out int totalrecords)
		{
			int num = 0;
			Globals.EntityCoding(query, true);
			System.Data.DataSet tradeOrders = OrderHelper.GetTradeOrders(query, out num);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (System.Data.DataRow dataRow in tradeOrders.Tables[0].Rows)
			{
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				System.Data.DataRow[] childRows = dataRow.GetChildRows("OrderRelation");
				for (int i = 0; i < childRows.Length; i++)
				{
					System.Data.DataRow dataRow2 = childRows[i];
					string text = Globals.HtmlEncode(dataRow2["SKUContent"].ToString());
					string text2 = dataRow2["ThumbnailsUrl"].ToString();
					stringBuilder2.AppendFormat(orderitemfomat, new object[]
					{
						dataRow2["Tid"].ToString(),
						dataRow2["OrderId"].ToString(),
						dataRow2["ProductId"].ToString(),
						dataRow2["ItemDescription"].ToString(),
						dataRow2["SKU"].ToString(),
						dataRow2["SkuId"].ToString(),
						text,
						dataRow2["Quantity"].ToString(),
						decimal.Parse(dataRow2["ItemListPrice"].ToString()).ToString("F2"),
						decimal.Parse(dataRow2["ItemAdjustedPrice"].ToString()).ToString("F2"),
						text2
					});
				}
				if (this.fomat == "json" && stringBuilder2.ToString().Length > 0)
				{
					stringBuilder2 = stringBuilder2.Remove(stringBuilder2.ToString().LastIndexOf(','), 1);
				}
				System.Collections.Generic.Dictionary<string, string> shippingRegion = MessageInfo.GetShippingRegion(dataRow["ShippingRegion"].ToString());
				stringBuilder.AppendFormat(this.jsonformat, new object[]
				{
					dataRow["OrderId"].ToString(),
					dataRow["SellerUid"].ToString(),
					dataRow["Username"].ToString(),
					dataRow["EmailAddress"].ToString(),
					dataRow["ShipTo"].ToString(),
					shippingRegion["Province"],
					shippingRegion["City"].ToString(),
					shippingRegion["District"],
					dataRow["Address"].ToString(),
					dataRow["ZipCode"].ToString(),
					dataRow["CellPhone"].ToString(),
					dataRow["TelPhone"].ToString(),
					Globals.HtmlEncode(dataRow["Remark"].ToString()),
					dataRow["ManagerMark"].ToString(),
					dataRow["ManagerRemark"].ToString(),
					dataRow["Nums"].ToString(),
					decimal.Parse(dataRow["OrderTotal"].ToString()).ToString("F2"),
					decimal.Parse(dataRow["OrderTotal"].ToString()).ToString("F2"),
					dataRow["Gateway"].ToString(),
					dataRow["PaymentTypeId"].ToString(),
					dataRow["PaymentType"].ToString(),
					decimal.Parse(dataRow["AdjustedFreight"].ToString()).ToString("F2"),
					"0.00",
					decimal.Parse(dataRow["AdjustedDiscount"].ToString()).ToString("F2"),
					dataRow["PayDate"].ToString(),
					dataRow["ShippingDate"].ToString(),
					dataRow["ReFundStatus"].ToString(),
					dataRow["RefundAmount"].ToString(),
					dataRow["RefundRemark"].ToString(),
					dataRow["OrderStatus"].ToString(),
					dataRow["ModeName"].ToString(),
					dataRow["OrderDate"].ToString(),
					stringBuilder2
				});
			}
			if (this.fomat == "json" && stringBuilder.Length > 0)
			{
				stringBuilder = stringBuilder.Remove(stringBuilder.ToString().LastIndexOf(','), 1);
			}
			totalrecords = num;
			return stringBuilder;
		}
		public System.Text.StringBuilder GetOrderDetails(string format, string orderitemfomat, OrderInfo order)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string text = string.Empty;
			if (order != null)
			{
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				long num = 0L;
				System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = order.LineItems;
				foreach (LineItemInfo current in lineItems.Values)
				{
					stringBuilder2.AppendFormat(orderitemfomat, new object[]
					{
						"0",
						order.OrderId,
						current.ProductId.ToString(),
						current.ItemDescription,
						current.SKU,
						current.SkuId,
						Globals.HtmlEncode(current.SKUContent),
						current.Quantity.ToString(),
						current.ItemListPrice.ToString("F2"),
						current.ItemAdjustedPrice.ToString("F2"),
						current.ThumbnailsUrl
					});
					num += (long)current.Quantity;
				}
				if (this.fomat == "json" && stringBuilder2.Length > 0)
				{
					stringBuilder2 = stringBuilder2.Remove(stringBuilder2.ToString().LastIndexOf(','), 1);
				}
				System.Collections.Generic.Dictionary<string, string> shippingRegion = MessageInfo.GetShippingRegion(order.ShippingRegion);
				string text2 = order.ModeName;
				if (!string.IsNullOrEmpty(order.RealModeName))
				{
					text2 = order.RealModeName;
				}
				if (string.IsNullOrEmpty(text))
				{
					text = order.ModeName;
				}
				string text3 = "";
				string text4 = "";
				string text5 = "";
				if (order.PayDate.Year != 1)
				{
					text3 = order.PayDate.ToString("yyyy-MM-dd");
				}
				if (order.ShippingDate.Year != 1)
				{
					text4 = order.ShippingDate.ToString("yyyy-MM-dd");
				}
				if (order.OrderDate.Year != 1)
				{
					text5 = order.OrderDate.ToString("yyyy-MM-dd");
				}
				stringBuilder.AppendFormat(format, new object[]
				{
					order.OrderId,
					"0",
					order.Username,
					order.EmailAddress,
					order.ShipTo,
					shippingRegion["Province"],
					shippingRegion["City"].ToString(),
					shippingRegion["District"],
					order.Address,
					order.ZipCode,
					order.CellPhone,
					order.TelPhone,
					Globals.HtmlEncode(order.Remark),
					order.ManagerMark,
					order.ManagerRemark,
					num.ToString(),
					order.GetTotal().ToString("F2"),
					order.GetTotal().ToString("F2"),
					order.Gateway,
					order.PaymentTypeId.ToString(),
					order.PaymentType,
					order.AdjustedFreight.ToString("F2"),
					order.ReducedPromotionAmount.ToString("F2"),
					order.AdjustedDiscount.ToString("F2"),
					text3,
					text4,
					((int)order.RefundStatus).ToString(),
					order.RefundAmount.ToString("F2"),
					order.RefundRemark,
					((int)order.OrderStatus).ToString(),
					text2,
					text5,
					stringBuilder2
				});
				if (!string.IsNullOrEmpty(order.ShippingRegion))
				{
					text = order.ShippingRegion;
				}
				if (!string.IsNullOrEmpty(order.Address))
				{
					text += order.Address;
				}
				if (!string.IsNullOrEmpty(order.ShipTo))
				{
					text = text + "   " + order.ShipTo;
				}
				if (!string.IsNullOrEmpty(order.ZipCode))
				{
					text = text + "   " + order.ZipCode;
				}
				if (!string.IsNullOrEmpty(order.TelPhone))
				{
					text = text + "   " + order.TelPhone;
				}
				if (!string.IsNullOrEmpty(order.CellPhone))
				{
					text = text + "   " + order.CellPhone;
				}
				string text6 = "<ShipAddress>{0}</ShipAddress><ShipOrderNumber>{1}</ShipOrderNumber><ExpressCompanyName>{2}</ExpressCompanyName>";
				if (this.fomat == "json")
				{
					text6 = "\"ShipAddress\":\"{0}\",\"ShipOrderNumber\":\"{1}\",\"ExpressCompanyName\":\"{2}\"";
				}
				text6 = string.Format(text6, text, order.ShipOrderNumber, order.ExpressCompanyName);
				if (this.fomat == "json")
				{
					stringBuilder = stringBuilder.Replace(",\"ModeName", "," + text6 + ",\"ModeName");
					stringBuilder = stringBuilder.Remove(stringBuilder.ToString().LastIndexOf(','), 1);
				}
				else
				{
					stringBuilder = stringBuilder.Replace("</Status>", "</Status>" + text6);
				}
			}
			return stringBuilder;
		}
		public ApiErrorCode SendOrders(OrderInfo order, ShippingModeInfo shippingmode, string out_id, ExpressCompanyInfo express)
		{
			if (order.GroupBuyId > 0 && order.GroupBuyStatus != GroupBuyStatus.Success)
			{
				return ApiErrorCode.Group_Error;
			}
			if (!order.CheckAction(OrderActions.SELLER_SEND_GOODS))
			{
				return ApiErrorCode.NoPay_Error;
			}
			if (shippingmode.ModeId <= 0)
			{
				return ApiErrorCode.NoShippingMode;
			}
			if (string.IsNullOrEmpty(out_id) || out_id.Length > 20)
			{
				return ApiErrorCode.ShipingOrderNumber_Error;
			}
			order.RealShippingModeId = shippingmode.ModeId;
			order.RealModeName = shippingmode.Name;
			order.ExpressCompanyName = express.Name;
			order.ExpressCompanyAbb = express.Kuaidi100Code;
			order.ShipOrderNumber = out_id;
			if (OrderHelper.SendAPIGoods(order))
			{
				if (!string.IsNullOrEmpty(order.GatewayOrderId))
				{
					PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(order.PaymentTypeId);
					if (paymentMode != null)
					{
						PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), order.OrderId, order.GetTotal(), "订单发货", "订单号-" + order.OrderId, order.EmailAddress, order.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
						{
							paymentMode.Gateway
						})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
						{
							paymentMode.Gateway
						})), "");
						paymentRequest.SendGoods(order.GatewayOrderId, order.RealModeName, order.ShipOrderNumber, "EXPRESS");
					}
				}
				if (!string.IsNullOrEmpty(order.TaobaoOrderId))
				{
					try
					{
						string requestUriString = string.Format("http://vip.ecdev.cn/UpdateShipping.ashx?tid={0}&companycode={1}&outsid={2}", order.TaobaoOrderId, express.TaobaoCode, order.ShipOrderNumber);
						System.Net.WebRequest webRequest = System.Net.WebRequest.Create(requestUriString);
						webRequest.GetResponse();
					}
					catch
					{
					}
				}
				int num = order.UserId;
				if (num == 1100)
				{
					num = 0;
				}
				IUser user = Users.GetUser(num);
				Messenger.OrderShipping(order, user);
				order.OnDeliver();
				return ApiErrorCode.Success;
			}
			return ApiErrorCode.Unknown_Error;
		}
	}
}
