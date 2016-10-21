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
using System.Text;
using System.Web.UI;
using EcShop.Core.ErrorLog;
using EcShop.ControlPanel.Sales;
using EcShop.Entities.Sales;
using EcShop.Core;
namespace EcShop.UI.Web.Pay
{
    public class wx_PayQRcode_notify_url : System.Web.UI.Page
    {
        protected OrderInfo Order;
        protected string OrderId;
        protected PayNotify payNotify = null;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            ErrorLog.Write("进入微信扫码支付回调(wx_PayQRcode_notify_url) ...");

            //// 看原始NotifyData
            //string notifyData = "空";
            //if (base.Request.InputStream != null)
            //{
            //    byte[] array = new byte[base.Request.InputStream.Length];
            //    base.Request.InputStream.Read(array, 0, array.Length);
            //    notifyData = Encoding.UTF8.GetString(array);
            //}

            //ErrorLog.Write("微信扫码回调原始数据(wx_PayQRcode_notify_url) ：" + notifyData);

            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.WxpayQrCode.QrCodeRequest");
            string AppId="";
            string Key = "";
            string AppSecret = "";
            string MchId = "";
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
                         MchId = xmlDocument.GetElementsByTagName("MchId")[0].InnerText;
                    }
                    catch
                    {
                         AppId = "";
                         Key = "";
                         AppSecret ="";
                         MchId = "";

                        ErrorLog.Write("微信扫码回调原始数据(wx_PayQRcode_notify_url) ，支付未配置");
                    }
                }
            }
            NotifyClient notifyClient = new NotifyClient(AppId, AppSecret, MchId, Key, "");
            payNotify = notifyClient.GetPayNotify(base.Request.InputStream);
            if (payNotify == null)
            {
                ErrorLog.Write("微信扫码回调原始数据(wx_PayQRcode_notify_url) ，数据为空");
                return;
            }
            this.OrderId = payNotify.PayInfo.OutTradeNo;
            this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
            if (this.Order == null)
            {
                ErrorLog.Write(string.Format("微信扫码回调原始数据(wx_PayQRcode_notify_url) ，订单不存在，订单号：{0}", this.OrderId));
                base.Response.Write("success");
                return;
            }
            this.Order.GatewayOrderId = payNotify.PayInfo.TransactionId;
            this.UserPayOrder();
        }
        private void UserPayOrder()
        {
            ErrorLog.Write("微信扫码回调原始数据(wx_PayQRcode_notify_url)，开始处理订单状态...");

            if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                base.Response.Write("success");
                return;
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(payNotify);

            if (Order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                //如果需要拆单
                if (TradeHelper.CheckIsUnpack(OrderId))
                {
                    ErrorLog.Write(string.Format("微信扫码回调原始数据(wx_PayQRcode_notify_url)拆单，原订单{0},返回信息{1}", this.Order.OrderId, json));
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
                else if (Order.OrderType == (int)OrderType.WillMerge)//合并单据
                {
                    ErrorLog.Write(string.Format("微信扫码回调原始数据(wx_PayQRcode_notify_url)合并单据，原订单{0},返回信息{1}", this.Order.OrderId, json));
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
                    ErrorLog.Write(string.Format("微信扫码回调原始数据(wx_PayQRcode_notify_url)正常单据，原订单{0},返回信息{1}", this.Order.OrderId, json));
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
            else
            {
                ErrorLog.Write(string.Format("微信扫码支付回调，当前状态不支持付款，订单号：{0}，订单状态：{1}", this.Order.OrderId, this.Order.OrderStatus));
            }
        }
    }
}
