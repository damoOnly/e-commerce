using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Messages;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	public class SupplierOrderDetails : AdminPage
	{
		private string orderId;
		private OrderInfo order;
		protected System.Web.UI.WebControls.Literal litOrderId;
		protected OrderStatusLabel lblOrderStatus;
		protected System.Web.UI.WebControls.Label lbCloseReason;
		protected System.Web.UI.WebControls.Label lbReason;
		protected System.Web.UI.WebControls.Literal litUserName;
		protected System.Web.UI.WebControls.Literal litRealName;
		protected System.Web.UI.WebControls.Literal litUserTel;
		protected System.Web.UI.WebControls.Literal litUserEmail;
		protected System.Web.UI.WebControls.Literal litPayTime;
		protected System.Web.UI.WebControls.Literal litSendGoodTime;
		protected System.Web.UI.WebControls.Literal litFinishTime;
		protected System.Web.UI.WebControls.HyperLink lkbtnEditPrice;
		protected System.Web.UI.HtmlControls.HtmlAnchor lbtnClocsOrder;
		protected System.Web.UI.WebControls.HyperLink lkbtnSendGoods;
		protected Order_ItemsList itemsList;
		protected System.Web.UI.WebControls.HyperLink hlkOrderGifts;
		protected Order_ChargesList chargesList;
		protected Order_ShippingAddress shippingAddress;
		protected System.Web.UI.WebControls.Literal spanOrderId;
		protected FormatedTimeLabel lblorderDateForRemark;
		protected FormatedMoneyLabel lblorderTotalForRemark;
		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected CloseTranReasonDropDownList ddlCloseReason;
		protected ShippingModeDropDownList ddlshippingMode;
		protected PaymentDropDownList ddlpayment;
		protected System.Web.UI.WebControls.Button btnRemark;
		protected System.Web.UI.WebControls.Button btnCloseOrder;
		protected System.Web.UI.WebControls.Button btnMondifyShip;
		protected System.Web.UI.WebControls.Button btnMondifyPay;
        protected System.Web.UI.WebControls.Literal litSourceOrder;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
            string key = "orderdetails-frame";
            string script = string.Format("<script>if(window.parent.frames.length == 0) window.location.href=\"{0}\";</script>", Globals.ApplicationPath + "/admin/default.html");
            ClientScriptManager clientScript = this.Page.ClientScript;
            if (!clientScript.IsClientScriptBlockRegistered(key))
            {
                clientScript.RegisterClientScriptBlock(base.GetType(), key, script);
            }
			this.orderId = this.Page.Request.QueryString["OrderId"];
			this.btnMondifyPay.Click += new System.EventHandler(this.btnMondifyPay_Click);
			this.btnMondifyShip.Click += new System.EventHandler(this.btnMondifyShip_Click);
			this.btnCloseOrder.Click += new System.EventHandler(this.btnCloseOrder_Click);
			this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
			this.order = OrderHelper.GetOrderInfo(this.orderId);
			if (this.order == null)
			{
				base.Response.Write("<h3 style=\"color:red;\">订单不存在，或者已被删除。</h3>");
				base.Response.End();
			}
			else
			{
				this.LoadUserControl(this.order);
			}
			if (!this.Page.IsPostBack)
			{
				this.lblOrderStatus.OrderStatusCode = this.order.OrderStatus;
                this.lblOrderStatus.SourceOrderIdCode = this.order.SourceOrderId;
                this.lblOrderStatus.PayDate = this.order.PayDate;
                this.litOrderId.Text = this.order.OrderId;
				this.litUserName.Text = this.order.Username;
				this.litRealName.Text = this.order.RealName;
				this.litUserTel.Text = this.order.TelPhone;
				this.litUserEmail.Text = this.order.EmailAddress;
                string sourceOrderId = order.SourceOrderId;
                if (!string.IsNullOrEmpty(sourceOrderId))
                {
                    string str = "来源单号：";
                    //合单
                    if (sourceOrderId.IndexOf(",") > 0)
                    {
                        string[] sourceOrderIdStr = sourceOrderId.Split(',');
                        if (sourceOrderIdStr != null && sourceOrderIdStr.Length > 0)
                        {
                            foreach (string item in sourceOrderIdStr)
                            {
                                str += "<a href='../../admin/default.html?sales/OrderDetails.aspx?OrderId=" + item + "' target='_blank'>" + item + "</a>&nbsp;";
                            }
                        }
                    }
                    //分单
                    else
                    {
                        str += "<a href='../../admin/default.html?sales/OrderDetails.aspx?OrderId=" + sourceOrderId + "' target='_blank'>" + sourceOrderId + "</a>";
                    }

                    this.litSourceOrder.Text = str;
                }
                if ((int)this.lblOrderStatus.OrderStatusCode != 4)
				{
					this.lbCloseReason.Visible = false;
				}
				else
				{
					this.lbReason.Text = this.order.CloseReason;
				}
				if (this.order.OrderStatus != OrderStatus.WaitBuyerPay && this.order.OrderStatus != OrderStatus.Closed && this.order.Gateway != "Ecdev.plugins.payment.podrequest")
				{
					this.litPayTime.Text = "付款时间：" + this.order.PayDate.ToString("yyyy-MM-dd HH:mm:ss");
				}
				if (this.order.OrderStatus == OrderStatus.SellerAlreadySent || this.order.OrderStatus == OrderStatus.Finished || this.order.OrderStatus == OrderStatus.Returned || this.order.OrderStatus == OrderStatus.ApplyForReturns || this.order.OrderStatus == OrderStatus.ApplyForReplacement)
				{
					this.litSendGoodTime.Text = "发货时间：" + this.order.ShippingDate.ToString("yyyy-MM-dd HH:mm:ss");
				}
				if (this.order.OrderStatus == OrderStatus.Finished)
				{
					this.litFinishTime.Text = "完成时间：" + this.order.FinishDate.ToString("yyyy-MM-dd HH:mm:ss");
				}
				if (this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid || (this.order.OrderStatus == OrderStatus.WaitBuyerPay && this.order.Gateway == "Ecdev.plugins.payment.podrequest"))
				{
					this.lkbtnSendGoods.Visible = true;
				}
				else
				{
					this.lkbtnSendGoods.Visible = false;
				}
				if (this.order.OrderStatus == OrderStatus.WaitBuyerPay)
				{
					this.lbtnClocsOrder.Visible = true;
					this.lkbtnEditPrice.Visible = true;
				}
				else
				{
					this.lbtnClocsOrder.Visible = false;
					this.lkbtnEditPrice.Visible = false;
				}
				this.lkbtnEditPrice.NavigateUrl = string.Concat(new string[]
				{
					"javascript:DialogFrame('",
					Globals.ApplicationPath,
					"/Admin/sales/EditOrder.aspx?OrderId=",
					this.orderId,
					"','修改订单价格',null,null)"
				});
				this.BindRemark(this.order);
				this.ddlshippingMode.DataBind();
				this.ddlshippingMode.SelectedValue = new int?(this.order.ShippingModeId);
				this.ddlpayment.DataBind();
				this.ddlpayment.SelectedValue = new int?(this.order.PaymentTypeId);
				if (this.order.OrderStatus == OrderStatus.WaitBuyerPay || this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
				{
					this.hlkOrderGifts.Visible = true;
					if (this.order.Gifts.Count > 0)
					{
						this.hlkOrderGifts.Text = "编辑订单礼品";
					}
					this.hlkOrderGifts.NavigateUrl = string.Concat(new string[]
					{
						"javascript:DialogFrameClose('",
						Globals.ApplicationPath,
						"/Admin/sales/OrderGifts.aspx?OrderId=",
						this.order.OrderId,
						"','编辑订单礼品',null,null);"
					});
					return;
				}
				this.hlkOrderGifts.Visible = false;
			}
		}
		private void LoadUserControl(OrderInfo order)
		{
			if (order != null)
			{
				this.itemsList.Order = order;
				this.chargesList.Order = order;
				this.shippingAddress.Order = order;
			}
		}
		private void btnMondifyPay_Click(object sender, System.EventArgs e)
		{
			PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(this.ddlpayment.SelectedValue.Value);
			this.order.PaymentTypeId = paymentMode.ModeId;
			this.order.PaymentType = paymentMode.Name;
			this.order.Gateway = paymentMode.Gateway;
			if (OrderHelper.UpdateOrderPaymentType(this.order))
			{
				this.chargesList.LoadControls();
				this.ShowMsg("修改支付方式成功", true);
				return;
			}
			this.ShowMsg("修改支付方式失败", false);
		}
		private void btnMondifyShip_Click(object sender, System.EventArgs e)
		{
			ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.ddlshippingMode.SelectedValue.Value, false);
			this.order.ShippingModeId = shippingMode.ModeId;
			this.order.ModeName = shippingMode.Name;
			if (OrderHelper.UpdateOrderShippingMode(this.order))
			{
				this.chargesList.LoadControls();
				this.shippingAddress.LoadControl();
				this.ShowMsg("修改配送方式成功", true);
				return;
			}
			this.ShowMsg("修改配送方式失败", false);
		}
		private void btnCloseOrder_Click(object sender, System.EventArgs e)
		{
			this.order.CloseReason = this.ddlCloseReason.SelectedValue;
			if (OrderHelper.CloseTransaction(this.order))
			{
				IUser user = Users.GetUser(this.order.UserId);
				Messenger.OrderClosed(user, this.order, this.order.CloseReason);
				this.order.OnClosed();
				this.ShowMsg("关闭订单成功", true);
				return;
			}
			this.ShowMsg("关闭订单失败", false);
		}
		private void btnRemark_Click(object sender, System.EventArgs e)
		{
			if (this.txtRemark.Text.Length > 300)
			{
				this.ShowMsg("备忘录长度限制在300个字符以内", false);
				return;
			}
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
			if (!regex.IsMatch(this.txtRemark.Text))
			{
				this.ShowMsg("备忘录只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
				return;
			}
			this.order.OrderId = this.spanOrderId.Text;
			if (this.orderRemarkImageForRemark.SelectedItem != null)
			{
				this.order.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
			}
			this.order.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
			if (OrderHelper.SaveRemark(this.order))
			{
				this.BindRemark(this.order);
				this.ShowMsg("保存备忘录成功", true);
				return;
			}
			this.ShowMsg("保存失败", false);
		}
		private void BindRemark(OrderInfo order)
		{
			this.spanOrderId.Text = order.OrderId;
			this.lblorderDateForRemark.Time = order.OrderDate;
			this.lblorderTotalForRemark.Money = order.GetTotal();
			this.txtRemark.Text = Globals.HtmlDecode(order.ManagerRemark);
			this.orderRemarkImageForRemark.SelectedValue = order.ManagerMark;
		}
	}
}
