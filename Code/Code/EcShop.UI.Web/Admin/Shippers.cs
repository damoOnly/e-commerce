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
	[PrivilegeCheck(Privilege.Shippers)]
	public class Shippers : AdminPage
	{
		protected Grid grdShippers;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.grdShippers.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdShippers_RowDeleting);
			this.grdShippers.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdShippers_RowCommand);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindShippers();
			}
		}
		private void grdShippers_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int shipperId = (int)this.grdShippers.DataKeys[e.RowIndex].Value;
			ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
			if (shipper.IsDefault)
			{
				this.ShowMsg("不能删除默认的发货信息", false);
				return;
			}
			if (SalesHelper.DeleteShipper(shipperId))
			{
				this.BindShippers();
				this.ShowMsg("已经成功删除选择的发货信息", true);
				return;
			}
			this.ShowMsg("不能删除默认的发货信息", false);
		}
		private void grdShippers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName == "SetYesOrNo")
			{
				System.Web.UI.WebControls.GridViewRow gridViewRow = (System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
				int num = (int)this.grdShippers.DataKeys[gridViewRow.RowIndex].Value;
				ShippersInfo shipper = SalesHelper.GetShipper(num);
				if (!shipper.IsDefault)
				{
					SalesHelper.SetDefalutShipper(num);
					this.BindShippers();
				}
			}
		}
		private void BindShippers()
		{
			this.grdShippers.DataSource = SalesHelper.GetShippers(false);
			this.grdShippers.DataBind();
		}
	}
}
