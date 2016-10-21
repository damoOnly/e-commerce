using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AliOHMyFavorites : AliOHMemberTemplatedWebControl
	{
		private AliOHTemplatedRepeater rptProducts;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vMyFavorites.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			string url = this.Page.Request.QueryString["returnUrl"];
			if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["returnUrl"]))
			{
				this.Page.Response.Redirect(url);
			}
			this.rptProducts = (AliOHTemplatedRepeater)this.FindControl("rptProducts");
			this.rptProducts.DataSource = ProductBrowser.GetFavorites();
			this.rptProducts.DataBind();
			PageTitle.AddSiteNameTitle("我的收藏");
		}
	}
}
