using EcShop.Core;
using EcShop.Entities.Orders;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class PurchaseOrderItemUpdateHyperLink : HyperLink
	{
		public object PurchaseStatusCode
		{
			get
			{
				if (this.ViewState["purchaseStatusCode"] == null)
				{
					return null;
				}
				return this.ViewState["purchaseStatusCode"];
			}
			set
			{
				if (value != null)
				{
					this.ViewState["purchaseStatusCode"] = value;
				}
			}
		}
		public object PurchaseOrderId
		{
			get
			{
				if (this.ViewState["PurchaseOrderId"] == null)
				{
					return null;
				}
				return this.ViewState["PurchaseOrderId"];
			}
			set
			{
				if (value != null)
				{
					this.ViewState["PurchaseOrderId"] = value;
				}
			}
		}
		public object DistorUserId
		{
			get
			{
				if (this.ViewState["DistorUserId"] == null)
				{
					return null;
				}
				return this.ViewState["DistorUserId"];
			}
			set
			{
				if (value != null)
				{
					this.ViewState["DistorUserId"] = value;
				}
			}
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (this.PurchaseOrderId.ToString().ToUpper().StartsWith("MP"))
			{
				OrderStatus orderStatus = (OrderStatus)this.PurchaseStatusCode;
				if (orderStatus == OrderStatus.WaitBuyerPay)
				{
					base.NavigateUrl = string.Concat(new object[]
					{
						Globals.ApplicationPath,
						"/admin/purchaseOrder/ChangePurchaseOrderItems.aspx?PurchaseOrderId=",
						this.PurchaseOrderId,
						"&DistorUserId=",
						this.DistorUserId
					});
				}
				else
				{
					base.Visible = false;
					base.Text = string.Empty;
				}
			}
			else
			{
				base.Visible = false;
				base.Text = string.Empty;
			}
			base.Render(writer);
		}
	}
}
