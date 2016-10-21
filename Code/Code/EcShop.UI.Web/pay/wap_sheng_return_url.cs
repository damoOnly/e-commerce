using EcShop.Core;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using Ecdev.Plugins;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web.UI;
namespace EcShop.UI.Web.pay
{
	public class wap_sheng_return_url : System.Web.UI.Page
	{
		protected PaymentNotify Notify;
		protected OrderInfo Order;
		protected string OrderId;
		protected decimal Amount;
		protected string Gateway;
		public string notestr = "";
		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			};
			this.Gateway = "Ecdev.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest";
			this.Notify = PaymentNotify.CreateInstance(this.Gateway, parameters);
			this.OrderId = this.Notify.GetOrderId();
			this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
			if (this.Order == null)
			{
				base.Response.Write("<p style=\"font-size:16px;\">找不到对应的订单，你付款的订单可能已经被删除</p>");
				return;
			}
			this.Amount = this.Notify.GetOrderAmount();
			if (this.Amount <= 0m)
			{
				this.Amount = this.Order.GetTotal();
			}
			this.Order.GatewayOrderId = this.Notify.GetGatewayOrderId();
			PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode("Ecdev.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest");
			if (paymentMode == null)
			{
				base.Response.Write("<p style=\"font-size:16px;\">找不到对应的支付方式，该支付方式可能已经被删除</p>");
				return;
			}
			this.Notify.Finished += new System.EventHandler<FinishedEventArgs>(this.Notify_Finished);
			this.Notify.NotifyVerifyFaild += new System.EventHandler(this.Notify_NotifyVerifyFaild);
			this.Notify.Payment += new System.EventHandler(this.Notify_Payment);
			this.Notify.VerifyNotify(30000, HiCryptographer.Decrypt(paymentMode.Settings));
		}
		private void Notify_Payment(object sender, System.EventArgs e)
		{
			this.UserPayOrder();
		}
		private void Notify_NotifyVerifyFaild(object sender, System.EventArgs e)
		{
			base.Response.Write("<p style=\"font-size:16px;\">签名验证失败，可能支付密钥已经被修改</p>");
		}
		private void Notify_Finished(object sender, FinishedEventArgs e)
		{
			if (e.IsMedTrade)
			{
				this.FinishOrder();
				return;
			}
			this.UserPayOrder();
		}
		private void UserPayOrder()
		{
			if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
				base.Response.Write("OK");
				base.Response.End();
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (this.Order.GroupBuyId > 0)
			{
				GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(this.Order.GroupBuyId);
				if (groupBuy == null || groupBuy.Status != GroupBuyStatus.UnderWay)
				{
					base.Response.Write("<p style=\"font-size:16px;\">订单为团购订单，团购活动已结束，支付失败</p>");
					return;
				}
				num2 = TradeHelper.GetOrderCount(this.Order.GroupBuyId);
				num3 = this.Order.GetGroupBuyOerderNumber();
				num = groupBuy.MaxCount;
				if (num < num2 + num3)
				{
					base.Response.Write("<p style=\"font-size:16px;\">订单为团购订单，订购数量超过订购总数，支付失败</p>");
					return;
				}
			}
			if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(this.Order, false))
			{
				TradeHelper.SaveDebitNote(new DebitNoteInfo
				{
					NoteId = Globals.GetGenerateId(),
					OrderId = this.Order.OrderId,
					Operator = this.Order.Username,
					Remark = "客户订单在线支付成功"
				});
				if (this.Order.GroupBuyId > 0 && num == num2 + num3)
				{
					TradeHelper.SetGroupBuyEndUntreated(this.Order.GroupBuyId);
				}
				if (this.Order.UserId != 0 && this.Order.UserId != 1100)
				{
					IUser user = Users.GetUser(this.Order.UserId);
					if (user != null && user.UserRole == UserRole.Member)
					{
						Messenger.OrderPayment(user, this.Order, this.Order.GetTotal());
					}
				}
				this.Order.OnPayment();
				base.Response.Write("OK");
				base.Response.End();
				return;
			}
			base.Response.Write("OK");
			base.Response.End();
		}
		private void FinishOrder()
		{
			if (this.Order.OrderStatus == OrderStatus.Finished)
			{
				base.Response.Write("OK");
				base.Response.End();
				return;
			}
			if (this.Order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && TradeHelper.ConfirmOrderFinish(this.Order))
			{
				base.Response.Write("OK");
				base.Response.End();
				return;
			}
			base.Response.Write("OK");
			base.Response.End();
		}
		public bool ExistFile(string FileUrl)
		{
			bool result;
			try
			{
				if (System.IO.File.Exists(FileUrl))
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public int CreateFile(string FileUrl, bool OverWrite)
		{
			int result;
			try
			{
				if (FileUrl.Trim().Equals("") || FileUrl.Trim().IndexOf(".") == -1 || FileUrl.Trim().IndexOf(".") == 0 || FileUrl.Trim().IndexOf(".") == FileUrl.Trim().Length - 1)
				{
					result = -2;
				}
				else
				{
					if (this.ExistFile(FileUrl) && !OverWrite)
					{
						result = -1;
					}
					else
					{
						System.IO.File.Create(FileUrl);
						result = 1;
					}
				}
			}
			catch
			{
				result = 0;
			}
			return result;
		}
		public int writeFile(string FileUrl, string FileStr, bool OverWrite, bool IsAppend, System.Text.Encoding enc)
		{
			if (this.ExistFile(FileUrl) && !OverWrite)
			{
				return -1;
			}
			System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(FileUrl, IsAppend, enc);
			streamWriter.WriteLine(FileStr);
			streamWriter.Close();
			return 1;
		}
	}
}
