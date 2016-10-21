using Ecdev.Weixin.Pay;
using Ecdev.Weixin.Pay.Notify;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Core.Jobs;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.ControlPanel.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace EcShop.Jobs
{
    public class WXOrderCheckJob : IJob
    {
        private Database database;
        private SiteSettings masterSettings;

        IDictionary<string, WxPayPartner> partners;

        public WXOrderCheckJob()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }

        public void Execute(XmlNode node)
        {
            ErrorLog.Write("start go WXOrderCheckJob");

            WriteBackOrders();

            // 取支付方式
            partners = GetWxPayPartners();

            IDictionary<string, string> orders = ShoppingProcessor.GetWxPayingOrder();
            if (orders.Count == 0)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var order in orders)
            {
                sb.Append(order.Key);
            }

            ErrorLog.Write(sb.ToString());

            masterSettings = SettingsManager.GetMasterSettings(false);

            NotifyClient notifyClient = null;


            WxPayPartner partner = null;

            foreach (var order in orders)
            {
                ErrorLog.Write("开始准备请求微信服务器");

                string orderId = order.Key;
                string gateway = order.Value.ToLower();

                partner = null;

                if (!partners.TryGetValue(gateway, out partner))
                {
                    ErrorLog.Write("找不到支付网关信息：" + gateway);
                    continue;
                }

                string payOrderInfo = LoadWXHadPayOrder(orderId, partner);

                ErrorLog.Write(payOrderInfo);

                notifyClient = new NotifyClient(partner.AppId, partner.AppSecret, partner.PartnerId, partner.PartnerKey, partner.SignKey);
                OrderNotify orderNotify = notifyClient.GetOrderStatusNotify(payOrderInfo);

                if (orderNotify != null)
                {
                    if (orderNotify.result_code == "SUCCESS" && !string.IsNullOrEmpty(orderNotify.transaction_id))
                    {
                        //UpdateOrder(orderNotify);
                        OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
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

        private IDictionary<string, WxPayPartner> GetWxPayPartners()
        {
            /*Ecdev.plugins.payment.WxpayQrCode.QrCodeRequest
            Ecdev.plugins.payment.wx_apppay.wxwappayrequest
            Ecdev.plugins.payment.weixinrequest*/
            IDictionary<string, WxPayPartner> partners = new Dictionary<string, WxPayPartner>();

            string appId = "";
            string partnerKey = "";
            string appSecret = "";
            string partnerId = "";
            string signKey = "";

            string key = "Ecdev.plugins.payment.weixinrequest".ToLower();
            masterSettings = SettingsManager.GetMasterSettings(false);

            partners.Add(key, new WxPayPartner(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey));

            key = "Ecdev.plugins.payment.wx_apppay.wxwappayrequest".ToLower();

            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(key);

            if (paymentMode != null)
            {
                if (paymentMode.Settings != "")
                {
                    string xml = HiCryptographer.Decrypt(paymentMode.Settings);

                    try
                    {
                        System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                        xmlDocument.LoadXml(xml);

                        appId = xmlDocument.GetElementsByTagName("AppId")[0].InnerText;
                        partnerKey = xmlDocument.GetElementsByTagName("Key")[0].InnerText;
                        appSecret = xmlDocument.GetElementsByTagName("AppSecret")[0].InnerText;
                        partnerId = xmlDocument.GetElementsByTagName("Mch_id")[0].InnerText;

                        partners.Add(key, new WxPayPartner(appId, appSecret, partnerId, partnerKey, signKey));
                    }
                    catch
                    {
                    }
                }
            }


            key = "Ecdev.plugins.payment.WxpayQrCode.QrCodeRequest".ToLower();

            paymentMode = SalesHelper.GetPaymentMode(key);

            if (paymentMode != null)
            {
                if (paymentMode.Settings != "")
                {
                    string xml = HiCryptographer.Decrypt(paymentMode.Settings);

                    try
                    {
                        System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                        xmlDocument.LoadXml(xml);

                        appId = xmlDocument.GetElementsByTagName("AppId")[0].InnerText;
                        partnerKey = xmlDocument.GetElementsByTagName("Key")[0].InnerText;
                        appSecret = xmlDocument.GetElementsByTagName("AppSecret")[0].InnerText;
                        partnerId = xmlDocument.GetElementsByTagName("Mch_id")[0].InnerText;

                        partners.Add(key, new WxPayPartner(appId, appSecret, partnerId, partnerKey, signKey));
                    }
                    catch
                    {
                    }
                }
            }
            return partners;
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
        private string LoadWXHadPayOrder(string orderId, WxPayPartner partner)
        {
            PayClient payClient = new PayClient(partner.AppId, partner.AppSecret, partner.PartnerId, partner.PartnerKey, partner.SignKey);

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
        private bool UpdateOrder(OrderNotify notify)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_UpdateHadPayOrder");
            this.database.AddInParameter(storedProcCommand, "GatewayOrderId", DbType.String, "");
            int ret = this.database.ExecuteNonQuery(storedProcCommand);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }
    }

    public class WxPayPartner
    {
        public WxPayPartner()
        {

        }

        public WxPayPartner(string appId, string appSecret, string partnerId, string partnerKey, string signKey)
        {
            this.AppId = appId;
            this.AppSecret = appSecret;
            this.PartnerId = partnerId;
            this.PartnerKey = partnerKey;
            this.SignKey = signKey;
        }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string PartnerId { get; set; }
        public string PartnerKey { get; set; }
        public string SignKey { get; set; }
    }
}
