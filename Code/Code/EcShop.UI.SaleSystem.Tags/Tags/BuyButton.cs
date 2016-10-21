using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	[ParseChildren(false), ToolboxData("<{0}:BuyButton runat=\"server\"></{0}:BuyButton>")]
	public class BuyButton : WebControl
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
		public BuyButton() : base(HtmlTextWriterTag.Span)
		{
		}
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(base.CssClass))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "buyButton");
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (HiContext.Current.User.UserRole == UserRole.Member)
			{
				writer.Write("<input type=\"hidden\" id=\"hiddenIsLogin\" value=\"logined\" />");
			}
			else
			{
				writer.Write("<input type=\"hidden\" id=\"hiddenIsLogin\" value=\"nologin\" />");
			}
			if ((this.Stock > 0 || !HiContext.Current.SiteSettings.IsOpenSiteSale) && this.Stock > 0 && HiContext.Current.SiteSettings.IsOpenSiteSale)
			{
				base.Render(writer);
			}
		}
	}
}
