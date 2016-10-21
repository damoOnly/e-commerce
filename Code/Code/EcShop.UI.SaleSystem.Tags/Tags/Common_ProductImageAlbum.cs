using EcShop.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_ProductImageAlbum : HyperLink
	{
		protected override void Render(HtmlTextWriter writer)
		{
			base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("ProductImages", new object[]
			{
				this.Page.Request.QueryString["productId"]
			});
			base.Render(writer);
		}
	}
}
