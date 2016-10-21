using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

using Ecdev.Weixin.Pay.Domain;
using Ecdev.Weixin.Pay.Pay;
using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.SqlDal.Sales;
using EcShop.UI.Common.Controls;
using Entities;
using EcShop.SqlDal.Orders;
using EcShop.SqlDal.Commodities;
using EcShop.Entities.Commodities;

namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class FinishOrder : HtmlTemplatedWebControl
    {
        private string orderId;
        private System.Web.UI.WebControls.Literal litOrderId;
        private FormatedMoneyLabel litOrderPrice;
        private System.Web.UI.WebControls.Literal litPaymentName;
        private System.Web.UI.WebControls.Button btnSubMitOrder;
        private System.Web.UI.WebControls.HyperLink hlinkOrderDetails;
        private System.Web.UI.HtmlControls.HtmlInputHidden hidd_ServerTimel;
        /// <summary>
        /// ΢��ɨ��֧����ά��
        /// </summary>
        private System.Web.UI.HtmlControls.HtmlImage codePayImg;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-FinishOrder.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            if (this.Page.Request.QueryString["orderId"] == null)
            {
                base.GotoResourceNotFound();
            }
            this.orderId = this.Page.Request.QueryString["orderId"];
            this.litOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderId");
            this.litOrderPrice = (FormatedMoneyLabel)this.FindControl("litOrderPrice");
            this.litPaymentName = (System.Web.UI.WebControls.Literal)this.FindControl("litPaymentName");
            this.btnSubMitOrder = (System.Web.UI.WebControls.Button)this.FindControl("btnSubMitOrder");
            this.codePayImg = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("codePayImg");
            this.hlinkOrderDetails = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkOrderDetails");
            this.hidd_ServerTimel = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidd_ServerTimel");
            this.hidd_ServerTimel.Value = DateTime.Now.ToString();
            if (hlinkOrderDetails != null)
            {
                hlinkOrderDetails.Attributes.Add("href","/user/OrderDetails.aspx?OrderId="+orderId);
            }

            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
            PaymentModeDao payment = new PaymentModeDao();
            PaymentModeInfo info = payment.GetPaymentMode(orderInfo.PaymentTypeId);

            //�ж��Ƿ�Ϊ΢��ɨ��֧��
            //if (info.Settings == "1hSUSkKQ/ENo0JDZah8KKQweixin")
            if (info != null && !string.IsNullOrEmpty(info.Gateway) && info.Gateway.ToLower() == "Ecdev.plugins.payment.WxpayQrCode.QrCodeRequest".ToLower())
            {
                string appId = "";
                string appSecret = "";
                string partnerId = "";
                string partnerKey = "";

                if (info.Settings != "")
                {
                    string xml = HiCryptographer.Decrypt(info.Settings);

                    try
                    {
                        System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                        xmlDocument.LoadXml(xml);

                        appId = xmlDocument.GetElementsByTagName("AppId")[0].InnerText;
                        partnerKey = xmlDocument.GetElementsByTagName("Key")[0].InnerText;
                        appSecret = xmlDocument.GetElementsByTagName("AppSecret")[0].InnerText;
                        partnerId = xmlDocument.GetElementsByTagName("MchId")[0].InnerText;
                    }
                    catch
                    {
                        ErrorLog.Write("΢��ɨ��֧����Ϣδ����");
                        this.ShowMessage("��ȡ֧����ά��ʧ�ܣ�֧����Ϣδ���ã�", false);
                    }
                }
                else
                {
                    ErrorLog.Write("΢��ɨ��֧����Ϣδ����");
                    this.ShowMessage("��ȡ֧����ά��ʧ�ܣ�֧����Ϣδ���ã�", false);
                }

                //\Storage\master\QRCode
                if (Directory.Exists(Globals.MapPath("/storage/master/QRCode")) == false)//��������ھʹ���file�ļ���
                {
                    Directory.CreateDirectory(Globals.MapPath("/storage/master/QRCode"));
                }

                VCodePayHelper.imgPath = Globals.MapPath("/storage/master/QRCode");
               
                //SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                ////PayClient payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);
                //VCodePayHelper.key = masterSettings.WeixinPartnerKey;
                ////����΢��֧����ά��
                //string vCodePayPath = VCodePayHelper.RequestWeixin(new Ecdev.Weixin.Pay.Domain.VCodePayEntity()
                //{
                //    appid = masterSettings.WeixinAppId,
                //    //appid = "wx45cd46b5f561deee",
                //    //mch_id = "D4DE77CE6F6C9E3A",
                //    mch_id = masterSettings.WeixinPartnerID,
                //    body = orderInfo.OrderId,
                //    nonce_str = VCodePayHelper.CreateRandom(20),
                //    out_trade_no = orderInfo.OrderId,
                //    fee_type = "CNY",
                //    //1 = 1��
                //    total_fee = (int)(orderInfo.GetTotal() * 100),
                //    spbill_create_ip = "192.168.1.40",
                //    notify_url = "http://" + base.Context.Request.Url.Host + "/pay/wx_Pay_notify_url.aspx",
                //    product_id = orderInfo.OrderId.ToString()
                //});

                ErrorLog.Write("΢��ɨ��֧������ʼͳһ�µ�����ȡ��ά��");
                string vCodePayPath = CreatePayQRCode(orderInfo, appId, partnerId);
                ErrorLog.Write("΢��ɨ��֧����ͳһ�µ������" + vCodePayPath);

                if (!string.IsNullOrWhiteSpace(vCodePayPath) && vCodePayPath.EndsWith(".jpg"))
                {
                    if (codePayImg != null)
                    {
                        OrderHelper.SetOrderPayStatus(orderInfo.OrderId, 1);
                        
                        codePayImg.Src = "/Storage/master/QRCode/" + vCodePayPath;
                    }
                }
                else
                {
                    ErrorLog.Write("��ȡ֧����ά��ʧ�ܣ�ԭ��" + vCodePayPath);
                    this.ShowMessage("��ȡ֧����ά��ʧ�ܣ�", false);
                }
                this.btnSubMitOrder.Visible = false;
            }
            else
            {
                codePayImg.Visible = false;
                this.btnSubMitOrder.Click += new System.EventHandler(this.btnSubMitOrder_Click);
            }
            if (!this.Page.IsPostBack)
            {
                this.LoadOrderInfo();
            }
        }
        /// <summary>
        /// ����֧����ά�루·����
        /// </summary>
        /// <param name="orderInfo">������Ϣ</param>
        /// <param name="appId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        private string CreatePayQRCode(OrderInfo orderInfo, string appId, string partnerId)
        {
            string orderId = orderInfo.OrderId;
            VCodePayEntity vCodePayEntity = new VCodePayEntity();
            vCodePayEntity.appid = appId;
            vCodePayEntity.mch_id = partnerId;
            vCodePayEntity.body = orderId;
            vCodePayEntity.nonce_str = VCodePayHelper.CreateRandom(20);
            vCodePayEntity.out_trade_no = orderId;
            vCodePayEntity.fee_type = "CNY";
            //1 = 1��
            vCodePayEntity.total_fee = (int)(orderInfo.GetTotal() * 100);
            vCodePayEntity.spbill_create_ip = "127.0.0.1";
            vCodePayEntity.notify_url = "http://" + base.Context.Request.Url.Host + "/pay/wx_PayQRcode_notify_url.aspx";
            vCodePayEntity.product_id = orderId;
            string vCodePayPath = VCodePayHelper.RequestWeixin(vCodePayEntity);
            return vCodePayPath;
        }

        private void ConFirmPay(OrderInfo order)
        {
            OrderDao orderDao = new OrderDao();
            orderDao.DebuctFactStock(order.OrderId);
            ProductDao productDao = new ProductDao();
            foreach (LineItemInfo current in order.LineItems.Values)
            {
                ProductInfo productDetails = productDao.GetProductDetails(current.ProductId);
                productDetails.SaleCounts += current.Quantity;
                productDetails.ShowSaleCounts += current.Quantity;
                productDao.UpdateProduct(productDetails, null);
            }
            OrderHelper.UpdateUserAccount(order);
        }
        private void btnSubMitOrder_Click(object sender, System.EventArgs e)
        {
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);

            if (DateTime.Now < DateTime.Parse("2015-08-21"))
            {
                //�жϸ��û��Ƿ��в����׵��Ļ
                int result = ShoppingProcessor.CheckIsFirstOrder(HiContext.Current.User.UserId, (int)OrderSource.PC);
                if (result > 0 && orderInfo.ActivityType == 1)
                {
                    //this.ShowMessage("���û��Ѿ��μӹ��׵��", false);
                    return;
                }
            }

            if (orderInfo != null)
            {
                if (orderInfo.Gateway != "ecdev.plugins.payment.advancerequest")
                {
                    System.Web.HttpContext.Current.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("sendPayment", new object[]
					{
						this.orderId
					}));
                    return;
                }
                System.Web.HttpContext.Current.Response.Redirect(string.Format("/user/pay.aspx?OrderId={0}", this.orderId));
            }
        }
        public void LoadOrderInfo()
        {
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
            if (orderInfo != null)
            {
                this.litOrderPrice.Money = orderInfo.GetTotal();
                this.litOrderId.Text = orderInfo.OrderId;
                this.litPaymentName.Text = orderInfo.PaymentType;
                if (orderInfo.Gateway == "ecdev.plugins.payment.podrequest")
                {
                    this.btnSubMitOrder.Visible = false;
                }
            }
        }

    }
}
