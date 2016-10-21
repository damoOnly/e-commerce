using System;
using System.IO;
using System.Text;

using Ecdev.Weixin.Pay;
using Ecdev.Weixin.Pay.Notify;
using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;

namespace EcShop.UI.Web.Pay
{
    public class wx_Pay_notify_url : System.Web.UI.Page
    {
        protected OrderInfo Order;
        protected string OrderId;
        protected PayNotify payNotify = null;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            ErrorLog.Write("进入App微信支付回调(wx_Pay_notify_url) ...");

            //// 看原始NotifyData
            //string notifyData = "空";
            //if (base.Request.InputStream != null)
            //{
            //    StreamReader reader = new StreamReader(base.Request.InputStream, Encoding.GetEncoding("UTF-8"));
            //    notifyData = reader.ReadToEnd();
            //}

            //ErrorLog.Write("App微信支付回调原始数据(wx_Pay_notify_url) ：" + notifyData);

            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.wx_apppay.wxwappayrequest");
            string AppId = "";
            string Key = "";
            string AppSecret = "";
            string Mch_id = "";
            if (paymentMode != null)
            {
                if (paymentMode.Settings != "")
                {
                    string xml = HiCryptographer.Decrypt(paymentMode.Settings);

                    try
                    {
                        System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                        xmlDocument.LoadXml(xml);
                        
                        AppId = xmlDocument.GetElementsByTagName("AppId")[0].InnerText;
                        Key = xmlDocument.GetElementsByTagName("Key")[0].InnerText;
                        AppSecret = xmlDocument.GetElementsByTagName("AppSecret")[0].InnerText;
                        Mch_id = xmlDocument.GetElementsByTagName("Mch_id")[0].InnerText;
                    }
                    catch(System.Exception ex)
                    {
                        ErrorLog.Write("微信App支付回调信息未设置：" + ex.Message);
                    }
                }
            }

            ErrorLog.Write("开始获取App微信支付回调数据(wx_Pay_notify_url) ...");

            NotifyClient notifyClient = new NotifyClient(AppId, AppSecret, Mch_id, Key, "");
            payNotify = notifyClient.GetPayNotify(base.Request.InputStream);
            if (payNotify == null)
            {
                ErrorLog.Write("App微信支付回调数据(wx_Pay_notify_url)为空");
                return;
            }

            this.OrderId = payNotify.PayInfo.OutTradeNo;
            ErrorLog.Write("获取App微信支付回调订单数据(wx_Pay_notify_url)：" + this.OrderId);

            this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
            if (this.Order == null)
            {
                ErrorLog.Write("App微信支付回调数据(wx_Pay_notify_url)没找到订单：" + this.OrderId);
                base.Response.Write("success");
                return;
            }

            ErrorLog.Write("App微信支付回调数据(wx_Pay_notify_url)交易号：" + payNotify.PayInfo.TransactionId);
            this.Order.GatewayOrderId = payNotify.PayInfo.TransactionId;
            this.Order.PaymentTypeId = paymentMode.ModeId;
            this.Order.PaymentType = paymentMode.Name;
            this.Order.Gateway = paymentMode.Gateway;

            ErrorLog.Write("App微信支付回调数据(wx_Pay_notify_url)，设置订单状态：" + this.OrderId);
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
                ErrorLog.Write(string.Format("微信App支付拆单，原订单{0},返回信息{1}", this.Order.OrderId, json));
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
                    base.Response.Write("success");
                }
            }
            else if (Order.OrderType == (int)OrderType.WillMerge && Order.OrderStatus == OrderStatus.WaitBuyerPay)//合并单据
            {
                ErrorLog.Write(string.Format("微信App支付合并单据，原订单{0},返回信息{1}", this.Order.OrderId, json));
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
                    base.Response.Write("success");
                }
            }
            else
            {
                ErrorLog.Write(string.Format("微信App支付正常单据，原订单{0},返回信息{1}", this.Order.OrderId, json));
                if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(this.Order, false))
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
                    base.Response.Write("success");
                }
            }
        }


    }
}
