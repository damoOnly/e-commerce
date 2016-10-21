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
using System.Web.UI;
namespace EcShop.UI.Web.pay
{
	public class wap_alipay_return_url : System.Web.UI.Page
	{
		protected PaymentNotify Notify;
		protected OrderInfo Order;
		protected string OrderId;
		protected decimal Amount;
		protected string Gateway;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			base.Response.Write("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no\"/>");
			System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			};
			this.Gateway = "Ecdev.plugins.payment.ws_wappay.wswappayrequest";
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
			PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode("Ecdev.plugins.payment.ws_wappay.wswappayrequest");
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
				base.Response.Write(string.Format("<p style=\"font-size:16px;\">恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}</p>", this.OrderId, this.Amount.ToString("F")));
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
				base.Response.Write(string.Format("<p style=\"font-size:16px;\">恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}</p>", this.OrderId, this.Amount.ToString("F")));
				return;
			}
			base.Response.Write(string.Format("<p style=\"font-size:16px;color:#ff0000;\">订单支付已成功，但是系统在处理过程中遇到问题，请联系管理员</br>支付金额：{0}</p>", this.Amount.ToString("F")));
		}
		private void FinishOrder()
		{
			if (this.Order.OrderStatus == OrderStatus.Finished)
			{
				base.Response.Write(string.Format("<p style=\"font-size:16px;\">恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}</p>", this.OrderId, this.Amount.ToString("F")));
				return;
			}
			if (this.Order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && TradeHelper.ConfirmOrderFinish(this.Order))
			{
				base.Response.Write(string.Format("<p style=\"font-size:16px;\">恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}</p>", this.OrderId, this.Amount.ToString("F")));
				return;
			}
			base.Response.Write(string.Format("<p style=\"font-size:16px;color:#ff0000;\">订单支付已成功，但是系统在处理过程中遇到问题，请联系管理员</br>支付金额：{0}</p>", this.Amount.ToString("F")));
		}
	}
}
