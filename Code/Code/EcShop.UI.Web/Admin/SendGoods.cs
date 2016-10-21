using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Messages;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Plugins;
using Ecdev.Weixin.Pay;
using Ecdev.Weixin.Pay.Domain;
using System;
using System.Net;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.OrderSendGoods)]
	public class SendGoods : AdminPage
	{
		private string orderId;
		protected System.Web.UI.WebControls.Label lblOrderId;
		protected FormatedTimeLabel lblOrderTime;
		protected ShippingModeRadioButtonList radioShippingMode;
		protected ExpressRadioButtonList expressRadioButtonList;
		protected System.Web.UI.WebControls.TextBox txtShipOrderNumber;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtShipOrderNumberTip;
		protected System.Web.UI.WebControls.Button btnSendGoods;
		protected Order_ItemsList itemsList;
		protected System.Web.UI.WebControls.Literal litShippingModeName;
		protected System.Web.UI.WebControls.Literal litReceivingInfo;
		protected System.Web.UI.WebControls.Label litShipToDate;
		protected System.Web.UI.WebControls.Label litRemark;
        protected HiddenField hiddenOrderValue;
        protected SupplierDropDownList ddlSupplier;
        protected Button btnUpdateSupplier;
        protected Button btnUpdateSku;
        protected TrimTextBox txtSku;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.orderId = this.Page.Request.QueryString["OrderId"];
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			this.BindOrderItems(orderInfo);
			this.btnSendGoods.Click += new System.EventHandler(this.btnSendGoods_Click);
            this.btnUpdateSupplier.Click += btnUpdateSupplier_Click;
            this.btnUpdateSku.Click += btnUpdateSku_Click;
			this.radioShippingMode.SelectedIndexChanged += new System.EventHandler(this.radioShippingMode_SelectedIndexChanged);
			if (!this.Page.IsPostBack)
			{
				if (orderInfo == null)
				{
					base.GotoResourceNotFound();
					return;
				}
                this.ddlSupplier.DataBind();
				this.radioShippingMode.DataBind();
				this.radioShippingMode.SelectedValue = new int?(orderInfo.ShippingModeId);
				this.BindExpressCompany(orderInfo.ShippingModeId);
				this.expressRadioButtonList.SelectedValue = orderInfo.ExpressCompanyName;
				this.BindShippingAddress(orderInfo);
				this.litShippingModeName.Text = orderInfo.ModeName;
				this.litShipToDate.Text = orderInfo.ShipToDate;
				this.litRemark.Text = orderInfo.Remark;
				this.txtShipOrderNumber.Text = orderInfo.ShipOrderNumber;
			}
		}
        /// <summary>
        /// 修改货号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnUpdateSku_Click(object sender, EventArgs e)
        {
            int productId;
            string orderId;
            string skuId;
            GetOrderValues(out productId, out orderId, out skuId);
            string newSku = this.txtSku.Text;
            if (OrderHelper.UpdateOrderItemSku(orderId, skuId, productId, newSku))
            {
                this.ShowMsg("修改货号成功", true);
                Response.Redirect(Request.Url.ToString());
                return;
            }
            this.ShowMsg("修改货号失败", false);
        }
        /// <summary>
        /// 修改供应商
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnUpdateSupplier_Click(object sender, EventArgs e)
        {
            int productId;
            string orderId;
            string skuId;
            GetOrderValues(out productId, out orderId, out skuId);
            int newSupplierId = this.ddlSupplier.SelectedValue.Value;
            if (OrderHelper.UpdateOrderItemSupplier(orderId, skuId, productId, newSupplierId))
            {
                this.ShowMsg("修改供货商成功", true);
                Response.Redirect(Request.Url.ToString());
                return;
            }
            this.ShowMsg("修改供货商失败", false);
        }


        private void GetOrderValues(out int productId, out string orderId, out string skuId)
        {
            productId = 0;
            orderId = string.Empty;
            skuId = string.Empty;
            if (!string.IsNullOrEmpty(hiddenOrderValue.Value))
            {
                string[] values = hiddenOrderValue.Value.Split('|');//orderId+'|'+skuId+'|'+productId
                if (values.Length < 3) { return; }
                productId = int.Parse(values[2]);
                orderId = values[0];
                skuId = values[1];
            }
        }

		private void BindOrderItems(OrderInfo order)
		{
			this.lblOrderId.Text = order.OrderId;
			this.lblOrderTime.Time = order.OrderDate;
			this.itemsList.Order = order;
		}
		private void BindShippingAddress(OrderInfo order)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(order.ShippingRegion))
			{
				text = order.ShippingRegion;
			}
			if (!string.IsNullOrEmpty(order.Address))
			{
				text += order.Address;
			}
			if (!string.IsNullOrEmpty(order.ShipTo))
			{
				text = text + "  " + order.ShipTo;
			}
			if (!string.IsNullOrEmpty(order.ZipCode))
			{
				text = text + "  " + order.ZipCode;
			}
			if (!string.IsNullOrEmpty(order.TelPhone))
			{
				text = text + "  " + order.TelPhone;
			}
			if (!string.IsNullOrEmpty(order.CellPhone))
			{
				text = text + "  " + order.CellPhone;
			}
			this.litReceivingInfo.Text = text;
		}
		private void BindExpressCompany(int modeId)
		{
			this.expressRadioButtonList.ExpressCompanies = SalesHelper.GetExpressCompanysByMode(modeId);
			this.expressRadioButtonList.DataBind();
		}
		private void radioShippingMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.radioShippingMode.SelectedValue.HasValue)
			{
				this.BindExpressCompany(this.radioShippingMode.SelectedValue.Value);
			}
		}
		private void btnSendGoods_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			if (orderInfo == null)
			{
				return;
			}
			if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus != GroupBuyStatus.Success)
			{
				this.ShowMsg("当前订单为团购订单，团购活动还未成功结束，所以不能发货", false);
				return;
			}
			if (!orderInfo.CheckAction(OrderActions.SELLER_SEND_GOODS))
			{
				this.ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
				return;
			}
			if (!this.radioShippingMode.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择配送方式", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtShipOrderNumber.Text.Trim()) || this.txtShipOrderNumber.Text.Trim().Length > 20)
			{
				this.ShowMsg("运单号码不能为空，在1至20个字符之间", false);
				return;
			}
			if (string.IsNullOrEmpty(this.expressRadioButtonList.SelectedValue))
			{
				this.ShowMsg("请选择物流公司", false);
				return;
			}
			ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);
			orderInfo.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
			orderInfo.RealModeName = shippingMode.Name;

            orderInfo.ShippingModeId = this.radioShippingMode.SelectedValue.Value;
            orderInfo.ModeName = shippingMode.Name;

			ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(this.expressRadioButtonList.SelectedValue);
			if (expressCompanyInfo != null)
			{
				orderInfo.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
				orderInfo.ExpressCompanyName = expressCompanyInfo.Name;
			}
			orderInfo.ShipOrderNumber = this.txtShipOrderNumber.Text;

            orderInfo.ShippingDate = DateTime.Now;
			if (OrderHelper.SendGoods(orderInfo))
			{
				SendNoteInfo sendNoteInfo = new SendNoteInfo();
				sendNoteInfo.NoteId = Globals.GetGenerateId();
				sendNoteInfo.OrderId = this.orderId;
				sendNoteInfo.Operator = HiContext.Current.User.Username;
				sendNoteInfo.Remark = "后台" + sendNoteInfo.Operator + "发货成功";
				OrderHelper.SaveSendNote(sendNoteInfo);
				if (orderInfo.Gateway == "Ecdev.plugins.payment.weixinrequest")
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
					PayClient payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);
					payClient.DeliverNotify(new DeliverInfo
					{
						TransId = orderInfo.GatewayOrderId,
						OutTradeNo = orderInfo.OrderId,
						OpenId = MemberHelper.GetMember(orderInfo.UserId).OpenId
					});
				}
				else
				{
					if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
					{
						PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.Gateway);
						if (paymentMode != null && !string.IsNullOrEmpty(paymentMode.Settings) &&　paymentMode.Settings != "1hSUSkKQ/ENo0JDZah8KKQweixin")
						{
							PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
							{
								paymentMode.Gateway
							})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
							{
								paymentMode.Gateway
							})), "");
							paymentRequest.SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
						}
					}
					if (!string.IsNullOrEmpty(orderInfo.TaobaoOrderId))
					{
						try
						{
							string requestUriString = string.Format("http://vip.ecdev.cn/UpdateShipping.ashx?tid={0}&companycode={1}&outsid={2}&Host={3}", new object[]
							{
								orderInfo.TaobaoOrderId,
								expressCompanyInfo.TaobaoCode,
								orderInfo.ShipOrderNumber,
								HiContext.Current.SiteUrl
							});
							System.Net.WebRequest webRequest = System.Net.WebRequest.Create(requestUriString);
							webRequest.GetResponse();
						}
						catch
						{
						}
					}
				}
				int num = orderInfo.UserId;
				if (num == 1100)
				{
					num = 0;
				}
				IUser user = Users.GetUser(num);
				Messenger.OrderShipping(orderInfo, user);
				orderInfo.OnDeliver();
				//this.ShowMsg("发货成功", true);
                this.CloseWindow();
				return;
			}
			this.ShowMsg("发货失败", false);
		}
	}
}
