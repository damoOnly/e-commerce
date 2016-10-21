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
            ErrorLog.Write("����App΢��֧���ص�(wx_Pay_notify_url) ...");

            //// ��ԭʼNotifyData
            //string notifyData = "��";
            //if (base.Request.InputStream != null)
            //{
            //    StreamReader reader = new StreamReader(base.Request.InputStream, Encoding.GetEncoding("UTF-8"));
            //    notifyData = reader.ReadToEnd();
            //}

            //ErrorLog.Write("App΢��֧���ص�ԭʼ����(wx_Pay_notify_url) ��" + notifyData);

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
                        ErrorLog.Write("΢��App֧���ص���Ϣδ���ã�" + ex.Message);
                    }
                }
            }

            ErrorLog.Write("��ʼ��ȡApp΢��֧���ص�����(wx_Pay_notify_url) ...");

            NotifyClient notifyClient = new NotifyClient(AppId, AppSecret, Mch_id, Key, "");
            payNotify = notifyClient.GetPayNotify(base.Request.InputStream);
            if (payNotify == null)
            {
                ErrorLog.Write("App΢��֧���ص�����(wx_Pay_notify_url)Ϊ��");
                return;
            }

            this.OrderId = payNotify.PayInfo.OutTradeNo;
            ErrorLog.Write("��ȡApp΢��֧���ص���������(wx_Pay_notify_url)��" + this.OrderId);

            this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
            if (this.Order == null)
            {
                ErrorLog.Write("App΢��֧���ص�����(wx_Pay_notify_url)û�ҵ�������" + this.OrderId);
                base.Response.Write("success");
                return;
            }

            ErrorLog.Write("App΢��֧���ص�����(wx_Pay_notify_url)���׺ţ�" + payNotify.PayInfo.TransactionId);
            this.Order.GatewayOrderId = payNotify.PayInfo.TransactionId;
            this.Order.PaymentTypeId = paymentMode.ModeId;
            this.Order.PaymentType = paymentMode.Name;
            this.Order.Gateway = paymentMode.Gateway;

            ErrorLog.Write("App΢��֧���ص�����(wx_Pay_notify_url)�����ö���״̬��" + this.OrderId);
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
            //�����Ҫ��
            if (TradeHelper.CheckIsUnpack(OrderId) && Order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                ErrorLog.Write(string.Format("΢��App֧���𵥣�ԭ����{0},������Ϣ{1}", this.Order.OrderId, json));
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
            else if (Order.OrderType == (int)OrderType.WillMerge && Order.OrderStatus == OrderStatus.WaitBuyerPay)//�ϲ�����
            {
                ErrorLog.Write(string.Format("΢��App֧���ϲ����ݣ�ԭ����{0},������Ϣ{1}", this.Order.OrderId, json));
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
                ErrorLog.Write(string.Format("΢��App֧���������ݣ�ԭ����{0},������Ϣ{1}", this.Order.OrderId, json));
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
