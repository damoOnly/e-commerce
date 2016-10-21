using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;

namespace EcShop.UI.Web.pay
{
	public class app_alipay_notify_url : System.Web.UI.Page
	{
        protected OrderInfo Order;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            ErrorLog.Write("进入支付宝App支付回调...");

			System.Collections.Generic.SortedDictionary<string, string> requestPost = this.GetRequestPost();
			if (requestPost.Count <= 0)
			{
                ErrorLog.Write("支付宝App支付回调，无通知参数");
				base.Response.Write("无通知参数");
				return;
			}

            WriteRequestFormToLog(requestPost);   

			Notify notify = new Notify();
            ErrorLog.Write("支付宝App支付回调notify_id:" + base.Request.Form["notify_id"] + ";sign:" + base.Request.Form["sign"] + "");
			bool flag = notify.Verify(requestPost, base.Request.Form["notify_id"], base.Request.Form["sign"]);
            ErrorLog.Write("支付宝App支付回调flag:" + flag);
            ErrorLog.Write("trade_no支付流水号:" + base.Request.Form["trade_no"]);
			if (flag)
			{
                ErrorLog.Write("支付宝App支付回调，校验成功");
                ErrorLog.Write("支付宝App支付回调out_trade_no:" + base.Request.Form["out_trade_no"] + "");
				string orderId = base.Request.Form["out_trade_no"];
                ErrorLog.Write("支付宝App支付回调trade_no:" + base.Request.Form["trade_no"] + "");
				string gatewayOrderId = base.Request.Form["trade_no"];
                ErrorLog.Write("gatewayOrderId支付流水号:" + gatewayOrderId);
				string a = base.Request.Form["trade_status"];

                ErrorLog.Write(string.Format("支付宝App支付回调，校验成功，订单号：{0}，流水号：{1}，交易状态：{2}", orderId, gatewayOrderId, a));

				if (!(a == "TRADE_FINISHED") && a == "TRADE_SUCCESS")
				{
                    this.Order = ShoppingProcessor.GetOrderInfo(orderId);
                    if (this.Order == null)
					{
                        ErrorLog.Write(string.Format("支付宝App支付回调，校验成功，订单号{0}不存在", orderId));

						base.Response.Write("success");
						return;
					}
                    this.Order.GatewayOrderId = gatewayOrderId;

                    string gateway = "Ecdev.plugins.payment.ws_apppay.wswappayrequest";

                    if (this.Order.Gateway.ToLower() != gateway.ToLower())
                    {
                        PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(gateway);
                        if (paymentMode != null)
                        {
                            this.Order.Gateway = gateway;
                            this.Order.PaymentTypeId = paymentMode.ModeId;
                            this.Order.PaymentType = paymentMode.Name;
                        }
                    }
                    ErrorLog.Write(string.Format("支付宝App支付回调，校验成功，订单号{0}更新订单状态", orderId));
                    this.UserPayOrder(this.Order);
				}
				base.Response.Write("success");
				return;
			}
            ErrorLog.Write("支付宝App支付回调，校验失败");
			base.Response.Write("fail");
		}
        private void UserPayOrder(OrderInfo order)
		{
            ErrorLog.Write(string.Format("支付宝App支付回调，校验成功，开始更新订单{0}状态", this.Order.OrderId));

            if (order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                base.Response.Write("success");
                return;
            }

            if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                base.Response.Write("success");
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
                    base.Response.Write("groupbuyalreadyfinished");
                    return;
                }
                num2 = TradeHelper.GetOrderCount(this.Order.GroupBuyId);
                num3 = this.Order.GetGroupBuyOerderNumber();
                num = groupBuy.MaxCount;
                if (num < num2 + num3)
                {
                    base.Response.Write("exceedordermax");
                    return;
                }
            }

            if (Order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                //如果需要拆单
                if (TradeHelper.CheckIsUnpack(this.Order.OrderId))
                {
                    ErrorLog.Write(string.Format("支付宝App支付，拆单，原订单{0}", this.Order.OrderId));
                    if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(this.Order, false, 1))
                    {
                        //OrderHelper.SetOrderPayStatus(this.Order.OrderId, 2);
                        OrderHelper.SetOrderPayStatus(this.Order.OrderId, 2, Order.PaymentTypeId, Order.PaymentType, Order.Gateway, Order.GatewayOrderId);
                        if (this.Order.UserId != 0 && this.Order.UserId != 1100)
                        {
                            IUser user = Users.GetUser(this.Order.UserId);
                            if (user != null && user.UserRole == UserRole.Member)
                            {
                                Messenger.OrderPayment(user, this.Order, this.Order.GetTotal());
                            }
                        }
                        this.Order.OnPayment();
                        base.Response.Write("success");
                    }
                }
                else if (Order.OrderType == (int)OrderType.WillMerge)//合并单据
                {

                    bool b = ShoppingProcessor.mergeOrder(this.Order);
                    int flag = 0;
                    if (b)
                    {
                        flag = 2;
                    }
                    if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(this.Order, false, flag))
                    {
                        //OrderHelper.SetOrderPayStatus(orderId, 2);
                        OrderHelper.SetOrderPayStatus(this.Order.OrderId, 2, Order.PaymentTypeId, Order.PaymentType, Order.Gateway, Order.GatewayOrderId);
                        if (this.Order.UserId != 0 && this.Order.UserId != 1100)
                        {
                            IUser user = Users.GetUser(this.Order.UserId);
                            if (user != null && user.UserRole == UserRole.Member)
                            {
                                Messenger.OrderPayment(user, this.Order, this.Order.GetTotal());
                            }
                        }
                        this.Order.OnPayment();
                        base.Response.Write("success");
                    }
                }
                else
                {
                    ErrorLog.Write(string.Format("支付宝App支付，正常单据，原订单{0}", this.Order.OrderId));
                    if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(this.Order, false))
                    {
                        OrderHelper.SetOrderPayStatus(this.Order.OrderId, 2, Order.PaymentTypeId, Order.PaymentType, Order.Gateway, Order.GatewayOrderId);

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
                        OrderHelper.SetOrderPayStatus(this.Order.OrderId, 2, Order.PaymentTypeId, Order.PaymentType, Order.Gateway, Order.GatewayOrderId);
                        if (this.Order.UserId != 0 && this.Order.UserId != 1100)
                        {
                            IUser user = Users.GetUser(this.Order.UserId);
                            if (user != null && user.UserRole == UserRole.Member)
                            {
                                Messenger.OrderPayment(user, this.Order, this.Order.GetTotal());
                            }
                        }
                        this.Order.OnPayment();
                        base.Response.Write("success");
                    }
                }
            }
            else
            {
                ErrorLog.Write(string.Format("支付宝App支付，当前状态不支持付款，原订单{0}，订单状态{1}", this.Order.OrderId, (int)this.Order.OrderStatus));
                base.Response.Write("fail");
            }

            //if (order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(order, false))
            //{
            //    order.OnPayment();
            //    base.Response.Write("success");
            //}
		}
		private System.Collections.Generic.SortedDictionary<string, string> GetRequestPost()
		{
			System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
			System.Collections.Specialized.NameValueCollection form = base.Request.Form;
			string[] allKeys = form.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				sortedDictionary.Add(allKeys[i], base.Request.Form[allKeys[i]]);
			}
			return sortedDictionary;
		}

        /// <summary>
        /// 记录支付宝回调的参数
        /// </summary>
        /// <param name="requestPost"></param>
        private void WriteRequestFormToLog(SortedDictionary<string, string> requestPost)
        {
            List<string> list = new List<string>();
            foreach (var item in requestPost)
            {
                list.Add(item.Key + "=" + item.Value);
            }
            ErrorLog.Write("支付回调参数：" + string.Join("&", list));
        }
	}
}
