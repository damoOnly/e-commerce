using EcShop.Core;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using Ecdev.Plugins;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using EcShop.SaleSystem.Shopping;
using EcShop.Core.ErrorLog;
using EcShop.ControlPanel.Sales;
using System.Text;
using System.Collections.Generic;

namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true), System.Web.UI.PersistChildren(false)]
    public abstract class PaymentTemplatedWebControl : HtmlTemplatedWebControl
    {
        private readonly bool isBackRequest;
        protected PaymentNotify Notify;
        protected OrderInfo Order;
        protected string OrderId;
        protected decimal Amount;
        protected string Gateway;
        public PaymentTemplatedWebControl(bool _isBackRequest)
        {
            this.isBackRequest = _isBackRequest;
        }
        protected override void CreateChildControls()
        {
            ErrorLog.Write("进入标准支付回调...");

            this.Controls.Clear();
            if (!this.isBackRequest)
            {
                if (!base.LoadHtmlThemedControl())
                {
                    throw new SkinNotFoundException(this.SkinPath);
                }
                this.AttachChildControls();
            }
            this.DoValidate();
        }


        private System.Collections.Generic.SortedDictionary<string, string> GetRequestPostForm()
        {
            System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
            System.Collections.Specialized.NameValueCollection form = this.Page.Request.Form;
            string[] allKeys = form.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                sortedDictionary.Add(allKeys[i], this.Page.Request.Form[allKeys[i]]);
            }
            return sortedDictionary;
        }

        private System.Collections.Generic.SortedDictionary<string, string> GetRequestPostQueryString()
        {
            System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
            System.Collections.Specialized.NameValueCollection form = this.Page.Request.QueryString;
            string[] allKeys = form.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                sortedDictionary.Add(allKeys[i], this.Page.Request.QueryString[allKeys[i]]);
            }
            return sortedDictionary;
        }

        /// <summary>
        /// 记录支付宝回调的参数
        /// </summary>
        /// <param name="requestPost"></param>
        private void WriteRequestQueryStringToLog(SortedDictionary<string, string> requestPost)
        {
            List<string> list = new List<string>();
            foreach (var item in requestPost)
            {
                list.Add(item.Key + "=" + item.Value);
            }
            ErrorLog.Write("标准支付回调,支付回调参数QueryString：" + string.Join("&", list));
        }

        private void WriteRequestFormToLog(SortedDictionary<string, string> requestPost)
        {
            List<string> list = new List<string>();
            foreach (var item in requestPost)
            {
                list.Add(item.Key + "=" + item.Value);
            }
            ErrorLog.Write("标准支付回调,支付回调参数Form：" + string.Join("&", list));
        }

        private void DoValidate()
        {
            ErrorLog.Write("标准支付回调开始...");

            try
            {
                System.Collections.Generic.SortedDictionary<string, string> requestFormPost = this.GetRequestPostForm();
                WriteRequestFormToLog(requestFormPost);
                System.Collections.Generic.SortedDictionary<string, string> requestQueryStringPost = this.GetRequestPostForm();
                WriteRequestQueryStringToLog(requestQueryStringPost);
            }
            catch
            {
                ErrorLog.Write("标准支付回调开始：错误：");
            }

            System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			};
            this.Gateway = this.Page.Request.QueryString["HIGW"];
            ErrorLog.Write("标准支付回调，支持回调Gateway:" + this.Gateway);
            this.Notify = PaymentNotify.CreateInstance(this.Gateway, parameters);
            if (this.isBackRequest)
            {
                ErrorLog.Write("标准支付回调，支持回调");

                this.Notify.ReturnUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
				{
					this.Gateway
				})) + "?" + this.Page.Request.Url.Query;
            }
            this.OrderId = this.Notify.GetOrderId();
            this.Order = TradeHelper.GetOrderInfo(this.OrderId);
            if (this.Order == null)
            {
                ErrorLog.Write("标准支付回调，订单不存在：" + this.OrderId);

                this.ResponseStatus(true, "ordernotfound");
                return;
            }
            this.Amount = this.Notify.GetOrderAmount();
            if (this.Amount <= 0m)
            {
                this.Amount = this.Order.GetTotal();
            }
            this.Order.GatewayOrderId = this.Notify.GetGatewayOrderId();

            ErrorLog.Write(string.Format("标准支付回调，订单号：{0}，网关：{1}，流水号：{2}", this.OrderId, this.Gateway, this.Order.GatewayOrderId));
            //PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(this.Order.PaymentTypeId);
            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(this.Gateway);
            if (paymentMode == null)
            {
                ErrorLog.Write(string.Format("标准支付回调，不支持的网关，订单号：{0}，网关：{1}，流水号：{2}", this.OrderId, this.Gateway, this.Order.GatewayOrderId));
                this.ResponseStatus(true, "gatewaynotfound");
                return;
            }

            if (this.Order.Gateway.ToLower() != this.Gateway.ToLower())
            {
                ErrorLog.Write(string.Format("标准支付回调，变更网关，订单号：{0}，网关：{1}，流水号：{2}，新网关：{3}", this.OrderId, this.Order.Gateway, this.Order.GatewayOrderId, this.Gateway));

                PaymentModeInfo paymentType = SalesHelper.GetPaymentMode(this.Gateway);
                if (paymentType != null)
                {
                    this.Order.Gateway = this.Gateway;
                    this.Order.PaymentTypeId = paymentType.ModeId;
                    this.Order.PaymentType = paymentType.Name;
                }
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
            this.ResponseStatus(false, "verifyfaild");
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
        protected abstract void DisplayMessage(string status);
        private void ResponseStatus(bool success, string status)
        {
            if (this.isBackRequest)
            {
                this.Notify.WriteBack(HiContext.Current.Context, success);
                return;
            }
            this.DisplayMessage(status);
        }
        private void UserPayOrder()
        {
            ErrorLog.Write("标准支付回调，处理订单状态，订单号：" + this.OrderId);

            if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                this.ResponseStatus(true, "success");
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
                    this.ResponseStatus(false, "groupbuyalreadyfinished");
                    return;
                }
                num2 = TradeHelper.GetOrderCount(this.Order.GroupBuyId);
                num3 = this.Order.GetGroupBuyOerderNumber();
                num = groupBuy.MaxCount;
                if (num < num2 + num3)
                {
                    this.ResponseStatus(false, "exceedordermax");
                    return;
                }
            }

            //如果需要拆单
            if (TradeHelper.CheckIsUnpack(OrderId) && Order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                ErrorLog.Write(string.Format(this.Order.PaymentType + "支付，拆单，原订单{0}", this.Order.OrderId));
                if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(this.Order, false, 1))
                {
                    //OrderHelper.SetOrderPayStatus(OrderId, 2);
                    OrderHelper.SetOrderPayStatus(OrderId, 2, Order.PaymentTypeId, Order.PaymentType, Order.Gateway, Order.GatewayOrderId);
                    if (this.Order.UserId != 0 && this.Order.UserId != 1100)
                    {
                        IUser user = Users.GetUser(this.Order.UserId);
                        if (user != null && user.UserRole == UserRole.Member)
                        {
                            Messenger.OrderPayment(user, this.Order, this.Order.GetTotal());
                        }
                    }
                    this.Order.OnPayment();
                    this.ResponseStatus(true, "success");
                    return;
                }
            }
            else if (Order.OrderType == (int)OrderType.WillMerge && Order.OrderStatus == OrderStatus.WaitBuyerPay)//合并单据
            {

                bool b = ShoppingProcessor.mergeOrder(this.Order);
                int flag = 0;
                if (b)
                {
                    flag = 2;
                }
                if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(this.Order, false, flag))
                {
                    //OrderHelper.SetOrderPayStatus(OrderId, 2);
                    OrderHelper.SetOrderPayStatus(OrderId, 2, Order.PaymentTypeId, Order.PaymentType, Order.Gateway, Order.GatewayOrderId);
                    if (this.Order.UserId != 0 && this.Order.UserId != 1100)
                    {
                        IUser user = Users.GetUser(this.Order.UserId);
                        if (user != null && user.UserRole == UserRole.Member)
                        {
                            Messenger.OrderPayment(user, this.Order, this.Order.GetTotal());
                        }
                    }
                    this.Order.OnPayment();
                    this.ResponseStatus(true, "success");
                    return;
                }
            }
            else
            {

                ErrorLog.Write(string.Format(this.Order.PaymentType + "支付，正常单据，原订单{0}", this.Order.OrderId));

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

                    OrderHelper.SetOrderPayStatus(OrderId, 2, Order.PaymentTypeId, Order.PaymentType, Order.Gateway, Order.GatewayOrderId);
                    if (this.Order.UserId != 0 && this.Order.UserId != 1100)
                    {
                        IUser user = Users.GetUser(this.Order.UserId);
                        if (user != null && user.UserRole == UserRole.Member)
                        {
                            Messenger.OrderPayment(user, this.Order, this.Order.GetTotal());
                        }
                    }
                    this.Order.OnPayment();
                    this.ResponseStatus(true, "success");
                    return;
                }

            }



            /* 2015-08-17 修改 
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
                this.ResponseStatus(true, "success");
                return;
            }*/
            this.ResponseStatus(false, "fail");
        }

        private void FinishOrder()
        {
            if (this.Order.OrderStatus == OrderStatus.Finished)
            {
                this.ResponseStatus(true, "success");
                return;
            }
            if (this.Order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && TradeHelper.ConfirmOrderFinish(this.Order))
            {
                this.ResponseStatus(true, "success");
                return;
            }
            this.ResponseStatus(false, "fail");
        }
    }
}
