using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.SaleSystem.Shopping;
using System;
using System.Web;
namespace EcShop.UI.Web.API
{
	public class TaobaoOrderHandler : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			GzipExtention.Gzip(context);
			string text = context.Request.Form["action"];
			string a;
			if ((a = text) != null)
			{
				if (!(a == "OrderAdd"))
				{
					return;
				}
				this.ProcessOrderAdd(context);
			}
		}
		private void ProcessOrderAdd(System.Web.HttpContext context)
		{
			OrderInfo orderInfo = new OrderInfo();
			orderInfo.OrderId = this.GenerateOrderId();
			orderInfo.TaobaoOrderId = context.Request.Form["TaobaoOrderId"];
			orderInfo.Remark = context.Request.Form["BuyerMemo"] + context.Request.Form["BuyerMessage"];
			string text = context.Request.Form["SellerFlag"];
			if (!string.IsNullOrEmpty(text) && text != "0")
			{
				orderInfo.ManagerMark = new OrderMark?((OrderMark)int.Parse(text));
			}
			orderInfo.ManagerRemark = context.Request.Form["SellerMemo"];
			orderInfo.OrderDate = System.DateTime.Parse(context.Request.Form["OrderDate"]);
			orderInfo.PayDate = System.DateTime.Parse(context.Request.Form["PayDate"]);
			orderInfo.UserId = 1100;
			orderInfo.RealName = (orderInfo.Username = context.Request.Form["Username"]);
			orderInfo.EmailAddress = context.Request.Form["EmailAddress"];
			orderInfo.ShipTo = context.Request.Form["ShipTo"];
			orderInfo.ShippingRegion = context.Request.Form["ReceiverState"] + context.Request.Form["ReceiverCity"] + context.Request.Form["ReceiverDistrict"];
			orderInfo.RegionId = RegionHelper.GetRegionId(context.Request.Form["ReceiverDistrict"], context.Request.Form["ReceiverCity"], context.Request.Form["ReceiverState"]);
			orderInfo.Address = context.Request.Form["ReceiverAddress"];
			orderInfo.TelPhone = context.Request.Form["TelPhone"];
			orderInfo.CellPhone = context.Request.Form["CellPhone"];
			orderInfo.ZipCode = context.Request.Form["ZipCode"];
			orderInfo.RealShippingModeId = (orderInfo.ShippingModeId = 0);
			orderInfo.RealModeName = (orderInfo.ModeName = context.Request.Form["ModeName"]);
			orderInfo.PaymentType = "支付宝担宝交易";
			orderInfo.Gateway = "Ecdev.plugins.payment.alipayassure.assurerequest";
			orderInfo.PayCharge = 0m;
			orderInfo.AdjustedDiscount = 0m;
			string text2 = context.Request.Form["Products"];
			if (string.IsNullOrEmpty(text2))
			{
				context.Response.Write("-1");
				return;
			}
			string[] array = text2.Split(new char[]
			{
				'|'
			});
			if (array.Length <= 0)
			{
				context.Response.Write("-2");
				return;
			}
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text3 = array2[i];
				string[] array3 = text3.Split(new char[]
				{
					','
				});
				LineItemInfo lineItemInfo = new LineItemInfo();
				int productId = 0;
				int.TryParse(array3[1], out productId);
				int shipmentQuantity = 1;
				int.TryParse(array3[3], out shipmentQuantity);
				lineItemInfo.SkuId = array3[0];
				lineItemInfo.ProductId = productId;
				lineItemInfo.SKU = array3[2];
				lineItemInfo.Quantity = (lineItemInfo.ShipmentQuantity = shipmentQuantity);
				lineItemInfo.ItemCostPrice = (lineItemInfo.ItemAdjustedPrice = decimal.Parse(array3[4]));
				lineItemInfo.ItemListPrice = decimal.Parse(array3[5]);
				lineItemInfo.ItemDescription = System.Web.HttpUtility.UrlDecode(array3[6]);
				lineItemInfo.ThumbnailsUrl = array3[7];
				lineItemInfo.ItemWeight = 0m;
				lineItemInfo.SKUContent = array3[8];
				lineItemInfo.PromotionId = 0;
				lineItemInfo.PromotionName = "";
				orderInfo.LineItems.Add(lineItemInfo.SkuId, lineItemInfo);
			}
			orderInfo.AdjustedFreight = (orderInfo.Freight = decimal.Parse(context.Request.Form["PostFee"]));
			orderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;
			orderInfo.RefundStatus = RefundStatus.None;
            //orderInfo.OrderSource = OrderSource.Taobao;
            if (ShoppingProcessor.CreateOrder(orderInfo, false, true))
			{
				context.Response.Write("1");
				return;
			}
			context.Response.Write("0");
		}
		private string GenerateOrderId()
		{
			string text = string.Empty;
			System.Random random = new System.Random();
			for (int i = 0; i < 7; i++)
			{
				int num = random.Next();
				text += ((char)(48 + (ushort)(num % 10))).ToString();
			}
			return System.DateTime.Now.ToString("yyyyMMdd") + text;
		}
	}
}
