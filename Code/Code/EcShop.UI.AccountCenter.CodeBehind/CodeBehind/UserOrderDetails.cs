using EcShop.Core;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UserOrderDetails : MemberTemplatedWebControl
	{
		private string orderId;
		private FormatedTimeLabel litAddOrderDate;
		private FormatedTimeLabel litPayDate;
		private FormatedTimeLabel litShipDate;
		private FormatedTimeLabel litResultDate;
		private FormatedTimeLabel litPayBack;
		private FormatedTimeLabel litBackShip;
		private System.Web.UI.WebControls.Literal litOrderId;
		private FormatedTimeLabel litAddDate;
		private FormatedMoneyLabel lbltotalPrice;
        private OrderStatusLabel lblOrderStatus;
		private System.Web.UI.WebControls.Literal litCloseReason;
		private System.Web.UI.WebControls.Literal litRemark;
		private System.Web.UI.WebControls.Literal litShipTo;
		private System.Web.UI.WebControls.Literal litRegion;
		private System.Web.UI.WebControls.Literal litAddress;
		private System.Web.UI.WebControls.Literal litZipcode;
		private System.Web.UI.WebControls.Literal litEmail;
		private System.Web.UI.WebControls.Literal litPhone;
		private System.Web.UI.WebControls.Literal litTellPhone;
		private System.Web.UI.WebControls.Literal litShipToDate;
		private System.Web.UI.HtmlControls.HtmlInputHidden hdorderId;
		private System.Web.UI.WebControls.Literal litPaymentType;
		private System.Web.UI.WebControls.Literal litModeName;
		private System.Web.UI.WebControls.Panel plOrderSended;
		private System.Web.UI.WebControls.Literal litRealModeName;
		private System.Web.UI.WebControls.Literal litShippNumber;
		private System.Web.UI.WebControls.HyperLink litDiscountName;
		private System.Web.UI.WebControls.HyperLink litFreeName;
		private FormatedMoneyLabel litTax;
		private System.Web.UI.WebControls.Literal litInvoiceTitle;
		private System.Web.UI.WebControls.Panel plExpress;
		private System.Web.UI.HtmlControls.HtmlAnchor power;
		private Common_OrderManage_OrderItems orderItems;
		private System.Web.UI.WebControls.GridView grdOrderGift;
		private System.Web.UI.WebControls.Panel plOrderGift;
		private System.Web.UI.WebControls.Literal lblBundlingPrice;
		private System.Web.UI.WebControls.Literal litPoints;
		private System.Web.UI.WebControls.HyperLink litSentTimesPointPromotion;
		private System.Web.UI.WebControls.Literal litWeight;
		private System.Web.UI.WebControls.Literal litFree;
		private FormatedMoneyLabel lblFreight;
		private FormatedMoneyLabel lblPayCharge;
		private System.Web.UI.WebControls.Literal litCouponValue;
		private FormatedMoneyLabel lblDiscount;
		private FormatedMoneyLabel litTotalPrice;
		private FormatedMoneyLabel lblAdjustedDiscount;
		private FormatedMoneyLabel lblRefundTotal;
		private System.Web.UI.WebControls.TextBox txtReplaceRemark;
		private System.Web.UI.WebControls.TextBox txtReturnRemark;
		private System.Web.UI.WebControls.TextBox txtRemark;
		private System.Web.UI.WebControls.DropDownList dropReturnRefundType;
		private System.Web.UI.WebControls.DropDownList dropRefundType;
        private System.Web.UI.WebControls.DropDownList dropRefundReason;//退款原因
        private System.Web.UI.WebControls.DropDownList dropReturnReason;//退货原因
		private System.Web.UI.WebControls.DropDownList dropPayType;
		private System.Web.UI.WebControls.Button btnPay;
		private System.Web.UI.WebControls.Button btnOk;
		private System.Web.UI.WebControls.Button btnReturn;
		private System.Web.UI.WebControls.Button btnReplace;
		private System.Web.UI.WebControls.Label lbRefundMoney;
		private System.Web.UI.WebControls.Label lbCloseReason;
		private System.Web.UI.WebControls.LinkButton lkbtnConfirmOrder;
		private System.Web.UI.WebControls.LinkButton lkbtnCloseOrder;
		private System.Web.UI.HtmlControls.HtmlAnchor lkbtnApplyForRefund;
		private System.Web.UI.HtmlControls.HtmlAnchor lkbtnApplyForReturn;
		private System.Web.UI.HtmlControls.HtmlAnchor lkbtnApplyForReplace;
		private System.Web.UI.WebControls.Panel plRefund;
		private FormatedMoneyLabel lblTotalBalance;
		private System.Web.UI.WebControls.Literal litRefundOrderRemark;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserOrderDetails.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.orderId = this.Page.Request.QueryString["orderId"];
			this.litOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderId");
			this.lbltotalPrice = (FormatedMoneyLabel)this.FindControl("lbltotalPrice");
			this.litAddDate = (FormatedTimeLabel)this.FindControl("litAddDate");
            this.lblOrderStatus = (OrderStatusLabel)this.FindControl("lblOrderStatus");
			this.litCloseReason = (System.Web.UI.WebControls.Literal)this.FindControl("litCloseReason");
			this.litRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litRemark");
			this.litShipTo = (System.Web.UI.WebControls.Literal)this.FindControl("litShipTo");
			this.litRegion = (System.Web.UI.WebControls.Literal)this.FindControl("litRegion");
			this.litAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litAddress");
			this.litZipcode = (System.Web.UI.WebControls.Literal)this.FindControl("litZipcode");
			this.litEmail = (System.Web.UI.WebControls.Literal)this.FindControl("litEmail");
			this.litPhone = (System.Web.UI.WebControls.Literal)this.FindControl("litPhone");
			this.litTellPhone = (System.Web.UI.WebControls.Literal)this.FindControl("litTellPhone");
			this.litShipToDate = (System.Web.UI.WebControls.Literal)this.FindControl("litShipToDate");
			this.litPaymentType = (System.Web.UI.WebControls.Literal)this.FindControl("litPaymentType");
			this.litModeName = (System.Web.UI.WebControls.Literal)this.FindControl("litModeName");
			this.plOrderSended = (System.Web.UI.WebControls.Panel)this.FindControl("plOrderSended");
			this.litRealModeName = (System.Web.UI.WebControls.Literal)this.FindControl("litRealModeName");
			this.litShippNumber = (System.Web.UI.WebControls.Literal)this.FindControl("litShippNumber");
			this.litDiscountName = (System.Web.UI.WebControls.HyperLink)this.FindControl("litDiscountName");
			this.lblAdjustedDiscount = (FormatedMoneyLabel)this.FindControl("lblAdjustedDiscount");
			this.litFreeName = (System.Web.UI.WebControls.HyperLink)this.FindControl("litFreeName");
			this.plExpress = (System.Web.UI.WebControls.Panel)this.FindControl("plExpress");
			this.power = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("power");
			this.orderItems = (Common_OrderManage_OrderItems)this.FindControl("Common_OrderManage_OrderItems");
			this.grdOrderGift = (System.Web.UI.WebControls.GridView)this.FindControl("grdOrderGift");
			this.plOrderGift = (System.Web.UI.WebControls.Panel)this.FindControl("plOrderGift");
			this.lblBundlingPrice = (System.Web.UI.WebControls.Literal)this.FindControl("lblBundlingPrice");
			this.litPoints = (System.Web.UI.WebControls.Literal)this.FindControl("litPoints");
			this.litSentTimesPointPromotion = (System.Web.UI.WebControls.HyperLink)this.FindControl("litSentTimesPointPromotion");
			this.litWeight = (System.Web.UI.WebControls.Literal)this.FindControl("litWeight");
			this.litFree = (System.Web.UI.WebControls.Literal)this.FindControl("litFree");
			this.lblFreight = (FormatedMoneyLabel)this.FindControl("lblFreight");
			this.lblPayCharge = (FormatedMoneyLabel)this.FindControl("lblPayCharge");
			this.litCouponValue = (System.Web.UI.WebControls.Literal)this.FindControl("litCouponValue");
			this.lblDiscount = (FormatedMoneyLabel)this.FindControl("lblDiscount");
			this.litTotalPrice = (FormatedMoneyLabel)this.FindControl("litTotalPrice");
			this.lblRefundTotal = (FormatedMoneyLabel)this.FindControl("lblRefundTotal");
			this.litAddOrderDate = (FormatedTimeLabel)this.FindControl("litAddOrderDate");
			this.litPayDate = (FormatedTimeLabel)this.FindControl("litPayDate");
			this.litPayBack = (FormatedTimeLabel)this.FindControl("litPayBack");
			this.litBackShip = (FormatedTimeLabel)this.FindControl("litBackShip");
			this.litShipDate = (FormatedTimeLabel)this.FindControl("litShipDate");
			this.litResultDate = (FormatedTimeLabel)this.FindControl("litResultDate");
			this.lkbtnConfirmOrder = (System.Web.UI.WebControls.LinkButton)this.FindControl("lkbtnConfirmOrder");
			this.lkbtnCloseOrder = (System.Web.UI.WebControls.LinkButton)this.FindControl("lkbtnCloseOrder");
			this.lkbtnApplyForRefund = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("lkbtnApplyForRefund");
			this.lkbtnApplyForReturn = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("lkbtnApplyForReturn");
			this.lkbtnApplyForReplace = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("lkbtnApplyForReplace");
			this.btnOk = (System.Web.UI.WebControls.Button)this.FindControl("btnOk");
			this.btnReplace = (System.Web.UI.WebControls.Button)this.FindControl("btnReplace");
			this.btnReturn = (System.Web.UI.WebControls.Button)this.FindControl("btnReturn");
			this.btnPay = (System.Web.UI.WebControls.Button)this.FindControl("btnPay");
			this.lbRefundMoney = (System.Web.UI.WebControls.Label)this.FindControl("lbRefundMoney");
			this.lbRefundMoney = (System.Web.UI.WebControls.Label)this.FindControl("lbRefundMoney");
			this.lbCloseReason = (System.Web.UI.WebControls.Label)this.FindControl("lbCloseReason");
			this.txtRemark = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRemark");
			this.txtReturnRemark = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReturnRemark");
			this.txtReplaceRemark = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReplaceRemark");
			this.dropRefundType = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropRefundType");
            this.dropRefundReason = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropRefundReason");
            this.dropReturnReason = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropReturnReason");
			this.dropReturnRefundType = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropReturnRefundType");
			this.dropPayType = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropPayType");
			this.plRefund = (System.Web.UI.WebControls.Panel)this.FindControl("plRefund");
			this.lblTotalBalance = (FormatedMoneyLabel)this.FindControl("lblTotalBalance");
			this.litRefundOrderRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litRefundOrderRemark");
			this.litTax = (FormatedMoneyLabel)this.FindControl("litTax");
			this.litInvoiceTitle = (System.Web.UI.WebControls.Literal)this.FindControl("litInvoiceTitle");
			this.hdorderId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdorderId");
			PageTitle.AddTitle("订单详细页", HiContext.Current.Context);
			this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
			this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
			this.lkbtnConfirmOrder.Click += new System.EventHandler(this.lkbtnConfirmOrder_Click);
			this.lkbtnCloseOrder.Click += new System.EventHandler(this.lkbtnCloseOrder_Click);
			if (!this.Page.IsPostBack)
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				if (orderInfo == null || orderInfo.UserId != HiContext.Current.User.UserId)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该订单不存在或者不属于当前用户的订单"));
					return;
				}
				this.BindOrderBase(orderInfo);
				this.BindOrderAddress(orderInfo);
				this.BindOrderItems(orderInfo);
                BindRefundReason();
                BindReturnReason();
			}
		}
		private void btnReplace_Click(object sender, System.EventArgs e)
		{
			if (!TradeHelper.CanReplace(this.hdorderId.Value))
			{
				this.ShowMessage("已有待确认的申请！", false);
				return;
			}
			if (TradeHelper.ApplyForReplace(this.hdorderId.Value, this.txtReplaceRemark.Text))
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				this.BindOrderBase(orderInfo);
				this.ShowMessage("成功的申请了换货", true);
				return;
			}
			this.ShowMessage("申请换货失败", false);
		}
		private void btnReturn_Click(object sender, System.EventArgs e)
		{
			if (!TradeHelper.CanReturn(this.hdorderId.Value))
			{
				this.ShowMessage("已有待确认的申请！", false);
				return;
			}
			if (!this.CanReturnBalance())
			{
				this.ShowMessage("请先开通预付款账户", false);
				return;
			}
            string reason = string.Empty;
            if (this.dropReturnReason.SelectedIndex == this.dropReturnReason.Items.Count - 1)
            {
                reason = this.txtReturnRemark.Text;
            }
            else
            {
                reason = this.dropReturnReason.Text + Environment.NewLine + this.txtReturnRemark.Text;
            }
            if (TradeHelper.ApplyForReturn(this.hdorderId.Value, reason, int.Parse(this.dropReturnRefundType.SelectedValue)))
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				this.BindOrderBase(orderInfo);
				this.ShowMessage("成功的申请了退货", true);
				return;
			}
			this.ShowMessage("申请退货失败", false);
		}
		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if (!TradeHelper.CanRefund(this.hdorderId.Value))
			{
				this.ShowMessage("已有待确认的申请！", false);
				return;
			}
			if (!this.CanRefundBalance())
			{
				this.ShowMessage("请先开通预付款账户", false);
				return;
			}
            OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.hdorderId.Value);
            //if (orderInfo.InClearance)
            //{
            //    this.ShowMessage("清关中，不支持退款", false);
            //    this.lkbtnApplyForReturn.Visible = true;
            //    this.lkbtnApplyForRefund.Visible = false;
            //    return;
            //}
            string reason = this.dropRefundReason.SelectedIndex == this.dropRefundReason.Items.Count - 1 ? this.txtRemark.Text : this.dropRefundReason.Text;

            string flagMsg = "";

            if (TradeHelper.ApplyForRefund(this.hdorderId.Value, reason, int.Parse(this.dropRefundType.SelectedValue), out flagMsg))
			{
				orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				this.BindOrderBase(orderInfo);
				this.ShowMessage("成功的申请了退款", true);
				return;
			}
            this.ShowMessage("申请退款失败 " + flagMsg, false);
		}
		private void btnPay_Click(object sender, System.EventArgs e)
		{
			string value = this.hdorderId.Value;
			int modeId = 0;
			int.TryParse(this.dropPayType.SelectedValue, out modeId);
			PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(modeId);
			if (paymentMode != null)
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(value);
				orderInfo.PaymentTypeId = paymentMode.ModeId;
				orderInfo.PaymentType = paymentMode.Name;
				orderInfo.Gateway = paymentMode.Gateway;
				TradeHelper.UpdateOrderPaymentType(orderInfo);
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("sendPayment", new object[]
			{
				value
			}));
		}
		private void lkbtnConfirmOrder_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
			if (TradeHelper.ConfirmOrderFinish(orderInfo))
			{
				this.ShowMessage("成功的完成了该订单", true);
				HiContext.Current.Context.Response.Redirect("OrderDetails.aspx?OrderId=" + this.orderId);
				return;
			}
			this.ShowMessage("完成订单失败", false);
		}
		private void lkbtnCloseOrder_Click(object sender, System.EventArgs e)
		{
            // 2015-08-19
            if (TradeHelper.CloseOrder(this.orderId))
			{
				this.ShowMessage("成功的关闭了该订单", true);
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				this.BindOrderBase(orderInfo);
				return;
			}
			this.ShowMessage("关闭订单失败", false);
		}
		private bool CanRefundBalance()
		{
			if (System.Convert.ToInt32(this.dropRefundType.SelectedValue) != 1)
			{
				return true;
			}
			Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
			return member.IsOpenBalance;
		}
		private void BindOrderBase(OrderInfo order)
		{
			this.litOrderId.Text = order.OrderId;
			this.lbltotalPrice.Money = order.GetAmount();
			this.litAddDate.Time = order.OrderDate;
			this.litAddOrderDate.Time = order.OrderDate;
			this.litPayDate.Time = order.PayDate;
			this.litShipDate.Time = order.ShippingDate;
			this.litResultDate.Time = order.FinishDate;
            this.lblOrderStatus.IsRefund = order.IsRefund;
			this.lblOrderStatus.OrderStatusCode = order.OrderStatus;
            this.lblOrderStatus.PayDate = order.PayDate;
			if (order.OrderStatus == OrderStatus.Closed)
			{
				this.lbCloseReason.Visible = true;
				this.litCloseReason.Text = order.CloseReason;
			}
			if (order.OrderStatus == OrderStatus.Refunded)
			{
				this.lbRefundMoney.Visible = true;
				this.lblRefundTotal.Money = order.GetTotal();
			}
			if (order.OrderStatus == OrderStatus.Returned)
			{
				this.lbRefundMoney.Visible = true;
				decimal num;
				TradeHelper.GetRefundMoney(order, out num);
				this.lblRefundTotal.Money = num;
			}
			this.litRemark.Text = order.Remark;
			this.JudgeOrderStatus(order);
		}
		private void BindOrderAddress(OrderInfo order)
		{
			this.litShipTo.Text = order.ShipTo;
			this.litRegion.Text = order.ShippingRegion;
			this.litAddress.Text = order.Address;
			this.litZipcode.Text = order.ZipCode;
			this.litEmail.Text = order.EmailAddress;
			this.litTellPhone.Text = order.TelPhone;
			this.litPhone.Text = order.CellPhone;
			this.litShipToDate.Text = order.ShipToDate;
			IUser arg_92_0 = HiContext.Current.User;
			this.litPaymentType.Text = order.PaymentType + "(" + Globals.FormatMoney(order.PayCharge) + ")";
			this.litModeName.Text = order.ModeName + "(" + Globals.FormatMoney(order.AdjustedFreight) + ")";
			if (order.OrderStatus == OrderStatus.SellerAlreadySent || order.OrderStatus == OrderStatus.Finished)
			{
				this.plOrderSended.Visible = true;
				this.litShippNumber.Text = order.ShipOrderNumber;
				this.litRealModeName.Text = order.ExpressCompanyName;
			}
			if (order.OrderStatus != OrderStatus.SellerAlreadySent && order.OrderStatus != OrderStatus.Finished && order.OrderStatus != OrderStatus.ApplyForReplacement && order.OrderStatus != OrderStatus.ApplyForReturns && order.OrderStatus != OrderStatus.Returned && string.IsNullOrEmpty(order.ExpressCompanyAbb))
			{
				if (this.plExpress != null)
				{
					this.plExpress.Visible = true;
				}
				if (Express.GetExpressType() == "kuaidi100" && this.power != null)
				{
					this.power.Visible = true;
				}
			}
		}
		private void BindOrderItems(OrderInfo order)
		{
			this.orderItems.DataSource = order.LineItems.Values;
			this.orderItems.DataBind();
			if (order.Gifts.Count > 0)
			{
				this.plOrderGift.Visible = true;
				this.grdOrderGift.DataSource = order.Gifts;
				this.grdOrderGift.DataBind();
			}
			if (order.BundlingID > 0)
			{
				this.lblBundlingPrice.Text = string.Format("<span style=\"color:Red;\">捆绑价格：{0}</span>", Globals.FormatMoney(order.BundlingPrice));
			}
			this.litWeight.Text = order.Weight.ToString("F2");
			this.lblPayCharge.Money = order.PayCharge;
			this.lblFreight.Money = order.AdjustedFreight;
			if (order.IsFreightFree)
			{
				this.litFreeName.Text = order.FreightFreePromotionName;
				this.litFreeName.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					order.FreightFreePromotionId
				});
			}
			this.litTax.Money = order.Tax;
			this.litInvoiceTitle.Text = order.InvoiceTitle;
			this.lblAdjustedDiscount.Money = order.AdjustedDiscount;
			this.litCouponValue.Text = order.CouponName + " -" + Globals.FormatMoney(order.CouponValue);
			this.lblDiscount.Money = order.ReducedPromotionAmount;
			if (order.IsReduced)
			{
				this.litDiscountName.Text = order.ReducedPromotionName;
				this.litDiscountName.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					order.ReducedPromotionId
				});
			}
			this.litPoints.Text = order.Points.ToString(System.Globalization.CultureInfo.InvariantCulture);
			if (order.IsSendTimesPoint)
			{
				this.litSentTimesPointPromotion.Text = order.SentTimesPointPromotionName;
				this.litSentTimesPointPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					order.SentTimesPointPromotionId
				});
			}
			this.litTotalPrice.Money = order.GetTotal();
		}
        private void BindRefundReason()
        {
            var reasons = TradeHelper.GetOrderHandleReason(OrderHandleReasonType.Refund);
            this.dropRefundReason.DataSource = reasons;
            this.dropRefundReason.DataBind();

            this.dropRefundReason.Items.Insert(this.dropRefundReason.Items.Count, "其他原因...");
        }
        private void BindReturnReason()
        {
            var reasons = TradeHelper.GetOrderHandleReason(OrderHandleReasonType.Return);
            this.dropReturnReason.DataSource = reasons;
            this.dropReturnReason.DataBind();

            this.dropReturnReason.Items.Insert(this.dropReturnReason.Items.Count, "其他原因...");
        }
		private bool CanReturnBalance()
		{
			if (System.Convert.ToInt32(this.dropReturnRefundType.SelectedValue) != 1)
			{
				return true;
			}
			Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
			return member.IsOpenBalance;
		}
		private OrderQuery GetOrderQuery()
		{
			OrderQuery orderQuery = new OrderQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
			{
				orderQuery.OrderId = this.Page.Request.QueryString["orderId"];
			}
			return orderQuery;
		}
		private void JudgeOrderStatus(OrderInfo order)
		{
			this.lkbtnConfirmOrder.Visible = (order.OrderStatus == OrderStatus.SellerAlreadySent);
			this.lkbtnCloseOrder.Visible = (order.OrderStatus == OrderStatus.WaitBuyerPay);
			this.lkbtnApplyForRefund.Visible = (order.OrderStatus == OrderStatus.BuyerAlreadyPaid);
			this.lkbtnApplyForReturn.Visible = (order.OrderStatus == OrderStatus.SellerAlreadySent);
			this.lkbtnApplyForReplace.Visible = (order.OrderStatus == OrderStatus.SellerAlreadySent);

            //if (order.InClearance)
            //{
            //    this.lkbtnApplyForReturn.Visible = true;
            //    this.lkbtnApplyForRefund.Visible = false;
            //}
		}
	}
}
