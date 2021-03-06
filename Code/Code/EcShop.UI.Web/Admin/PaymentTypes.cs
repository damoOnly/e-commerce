using ASPNET.WebControls;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PaymentModes)]
	public class PaymentTypes : AdminPage
	{
		protected System.Web.UI.WebControls.GridView grdPaymentMode;
		protected Pager pager1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdPaymentMode.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdPaymentMode_RowDeleting);
			this.grdPaymentMode.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdPaymentMode_RowCommand);
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}
		private void grdPaymentMode_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName != "Sort")
			{
				int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
				int modeId = (int)this.grdPaymentMode.DataKeys[rowIndex].Value;
				int displaySequence = System.Convert.ToInt32((this.grdPaymentMode.Rows[rowIndex].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
				int num = 0;
				int num2 = 0;
				if (e.CommandName == "Fall")
				{
					if (rowIndex < this.grdPaymentMode.Rows.Count - 1)
					{
						num = (int)this.grdPaymentMode.DataKeys[rowIndex + 1].Value;
						num2 = System.Convert.ToInt32((this.grdPaymentMode.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
					}
				}
				else
				{
					if (e.CommandName == "Rise" && rowIndex > 0)
					{
						num = (int)this.grdPaymentMode.DataKeys[rowIndex - 1].Value;
						num2 = System.Convert.ToInt32((this.grdPaymentMode.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
					}
				}
				if (num > 0 && num2 > 0)
				{
					SalesHelper.SwapPaymentModeSequence(modeId, num, displaySequence, num2);
					this.BindData();
				}
			}
		}
		private void grdPaymentMode_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (SalesHelper.DeletePaymentMode((int)this.grdPaymentMode.DataKeys[e.RowIndex].Value))
			{
				this.BindData();
				this.ShowMsg("成功删除了一个支付方式", true);
				return;
			}
			this.ShowMsg("未知错误", false);
		}
		private void BindData()
		{
			this.grdPaymentMode.DataSource = SalesHelper.GetPaymentModes(PayApplicationType.payOnPC);
			this.grdPaymentMode.DataBind();
		}
	}
}
