using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using EcShop.Core.ErrorLog;
using System.Text;
using Ecdev.Weixin.Pay;
using EcShop.SaleSystem.Shopping;
using EcShop.Entities.Orders;
using EcShop.SaleSystem.Member;
using Ecdev.Weixin.Pay.Notify;
using System.Data;
namespace EcShop.UI.Web.Admin
{
    [AdministerCheck(true)]
    public class OrderRepair : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnOK;
        protected SiteSettings masterSettings;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
            }
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
        }
        protected void btnOK_Click(object sender, System.EventArgs e)
        {
            WriteBackOrders();
            IList<string> orders = ShoppingProcessor.GetPayingOrder();
            if (orders.Count == 0)
            {
                return;
            }
            ErrorLog.Write("start go WXOrderCheckJob");
            StringBuilder sb = new StringBuilder();
            foreach (var orderId in orders)
            {
                sb.Append(orderId);
            }
            ErrorLog.Write(sb.ToString());
            masterSettings = SettingsManager.GetMasterSettings(false);
            NotifyClient notifyClient = new NotifyClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);
            foreach (var orderItem in orders)
            {
                ErrorLog.Write("开始准备请求微信服务器");
                string payOrderInfo = LoadWXHadPayOrder(orderItem);
                ErrorLog.Write(payOrderInfo);
                OrderNotify orderNotify = notifyClient.GetOrderStatusNotify(payOrderInfo);
                if (orderNotify != null)
                {
                    if (orderNotify.result_code == "SUCCESS" && !string.IsNullOrEmpty(orderNotify.transaction_id))
                    {
                        //UpdateOrder(orderNotify);
                        OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderItem);
                        orderInfo.GatewayOrderId = orderNotify.transaction_id;
                        DateTime payDate = DateTime.MinValue;

                        DateTime.TryParseExact(orderNotify.time_end, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out payDate);
                        orderInfo.PayDate = payDate;
                        if (orderInfo != null)
                        {
                            PayOrder(orderInfo);
                            ErrorLog.Write(string.Format("用户已支付成功，检测到的未即时回写订单状态的订单{0}使用jobs回写状态！", orderNotify.out_trade_no));
                        }
                    }
                }
            }
        }
        private void PayOrder(OrderInfo order)
        {
            if (order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                return;
            }
            //如果需要拆单
            if (TradeHelper.CheckIsUnpack(order.OrderId) && order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                if (order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(order, false, 1))
                {
                    //if (order.UserId != 0 && order.UserId != 1100)
                    //{
                    //    IUser user = Users.GetUser(order.UserId);
                    //    if (user != null && user.UserRole == UserRole.Member)
                    //    {
                    //        Messenger.OrderPayment(user, order, order.GetTotal());
                    //    }
                    //}
                }
            }
            else
            {
                if (order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(order, false))
                {
                    //if (order.UserId != 0 && order.UserId != 1100)
                    //{
                    //    IUser user = Users.GetUser(order.UserId);
                    //    if (user != null && user.UserRole == UserRole.Member)
                    //    {
                    //        Messenger.OrderPayment(user, order, order.GetTotal());
                    //    }
                    //}
                }
            }
        }
        private string LoadWXHadPayOrder(string orderId)
        {
            PayClient payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);

            string postData = payClient.BuildOrderPackage(orderId);
            return payClient.RequestOrder(postData);
        }
        private void WriteBackOrders()
        {
            DataTable dtOrders = ShoppingProcessor.GetNoWriteBackOrders();
            int count = dtOrders.Rows.Count;
            string orderId = string.Empty;
            string gatewayOrderId = string.Empty;
            DateTime payDate;
            for (int i = 0; i < count; i++)
            {
                orderId = dtOrders.Rows[i]["OrderId"].ToString();
                gatewayOrderId = dtOrders.Rows[i]["GatewayOrderId"].ToString();
                payDate = DateTime.Parse(dtOrders.Rows[i]["PayDate"].ToString());
                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
                if (orderInfo != null)
                    PayOrder(orderInfo);
            }
        }
    }
}
