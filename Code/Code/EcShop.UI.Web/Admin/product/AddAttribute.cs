using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using EcShop.UI.Web.Admin.product.ascx;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.AddProductType)]
	public class AddAttribute : AdminPage
	{
		protected AttributeView attributeView;
		protected System.Web.UI.WebControls.Button btnNext;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
		}
		private void btnNext_Click(object sender, System.EventArgs e)
		{
			base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/AddSpecification.aspx?typeId=" + this.Page.Request.QueryString["typeId"]), true);
		}
	}
}
