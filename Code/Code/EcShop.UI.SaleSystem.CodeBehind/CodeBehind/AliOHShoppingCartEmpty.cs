using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AliOHShoppingCartEmpty : AliOHTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VShoppingCartEmpty.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("购物车");
		}
	}
}
