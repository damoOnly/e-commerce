using EcShop.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	[ParseChildren(false), ToolboxData("<{0}:AddCartButton runat=\"server\"></{0}:AddCartButton>")]
	public class AddCartButton : WebControl
	{
		public int Stock
		{
			get
			{
				if (this.ViewState["Stock"] == null)
				{
					return 0;
				}
				return (int)this.ViewState["Stock"];
			}
			set
			{
				this.ViewState["Stock"] = value;
			}
		}
		public AddCartButton() : base(HtmlTextWriterTag.Span)
		{
		}
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(base.CssClass))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "addcartButton");
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (this.Stock > 0 && HiContext.Current.SiteSettings.IsOpenSiteSale)
			{
				base.Render(writer);
			}
		}
	}
}
