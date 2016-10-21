using EcShop.Entities.Sales;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
    public class WAPShoppingCart : WAPTemplatedWebControl
	{
		private WapTemplatedRepeater rptCartProducts;
		private System.Web.UI.WebControls.Literal litTotal;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VShoppingCart.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.rptCartProducts = (WapTemplatedRepeater)this.FindControl("rptCartProducts");
			this.litTotal = (System.Web.UI.WebControls.Literal)this.FindControl("litTotal");
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
			if (shoppingCart != null)
			{
				this.rptCartProducts.DataSource = shoppingCart.LineItems;
				this.rptCartProducts.DataBind();
				this.litTotal.Text = shoppingCart.GetAmount().ToString("F2");
			}
			PageTitle.AddSiteNameTitle("购物车");
		}
	}
}
