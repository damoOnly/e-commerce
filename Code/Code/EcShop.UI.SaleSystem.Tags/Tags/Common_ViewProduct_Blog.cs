using EcShop.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_ViewProduct_Blog : HyperLink
	{
		protected override void Render(HtmlTextWriter writer)
		{
			base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("BlogIt", new object[]
			{
				this.Page.Request.QueryString["productId"]
			});
			base.Render(writer);
		}
	}
}
