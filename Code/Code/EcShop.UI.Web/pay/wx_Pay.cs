using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using Ecdev.Weixin.Pay;
using Ecdev.Weixin.Pay.Notify;
using System;
using System.Web.UI;
using EcShop.Core.ErrorLog;
using EcShop.ControlPanel.Sales;
namespace EcShop.UI.Web.Pay
{
    public class wx_Pay : System.Web.UI.Page
    {
        protected OrderInfo Order;
        protected string OrderId;
        protected PayNotify payNotify = null;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            NotifyClient notifyClient = new NotifyClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);
            payNotify = notifyClient.GetPayNotify(base.Request.InputStream);
            if (payNotify == null)
            {
                return;
            }
            this.OrderId = payNotify.PayInfo.OutTradeNo;
            this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
            if (this.Order == null)
            {
                base.Response.Write("success");
                return;
            }
            this.Order.GatewayOrderId = payNotify.PayInfo.TransactionId;
            this.UserPayOrder();
        }
        private void UserPayOrder()
        {
            if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                base.Response.Write("success");
                return;
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(payNotify);
            //如果需要拆单
            if (TradeHelper.CheckIsUnpack(OrderId) && Order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                ErrorLog.Write(string.Format("拆单，原订单{0},返回信息{1}", this.Order.OrderId, json));
                if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(this.Order, false, 1))
                {
                    OrderHelper.SetOrderPayStatus(OrderId, 2);
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
            else if (Order.OrderType == (int)OrderType.WillMerge && Order.OrderStatus == OrderStatus.WaitBuyerPay)//合并单据
            {
                ErrorLog.Write(string.Format("合并单据，原订单{0},返回信息{1}", this.Order.OrderId, json));
                bool b = ShoppingProcessor.mergeOrder(this.Order);
                int flag = 0;
                if (b)
                {
                    flag = 2;
                }
                if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(this.Order, false, flag))
                {
                    OrderHelper.SetOrderPayStatus(OrderId, 2);
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
                ErrorLog.Write(string.Format("正常单据，原订单{0},返回信息{1}", this.Order.OrderId, json));
                if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(this.Order, false))
                {
                    OrderHelper.SetOrderPayStatus(OrderId, 2);
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
    }
}
