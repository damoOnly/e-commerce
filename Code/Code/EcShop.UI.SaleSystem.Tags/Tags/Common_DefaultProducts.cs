
using EcShop.Entities.Sales;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_DefaultProducts : AscxTemplatedWebControl
	{
        private ProductPromote promote;
        private int productId=897;
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/tags/skin-Common_DefaultProducts.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
            this.promote = (ProductPromote)this.FindControl("ProductPromote");
            if (this.promote != null)
            {
                this.promote.ProductId = this.productId;
            }
		}
	}
}
