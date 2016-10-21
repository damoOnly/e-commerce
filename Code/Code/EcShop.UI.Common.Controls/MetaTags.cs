using EcShop.Membership.Context;
using System;
using System.Web;
using System.Web.UI;
namespace EcShop.UI.Common.Controls
{
	[ParseChildren(false), PersistChildren(true)]
	public class MetaTags : Control
	{
		private const string metaFormat = "<meta name=\"{0}\" content=\"{1}\" />";
		private const string metaDescriptionKey = "Ecdev.Meta_Description";
        private const string metaKeywordsKey = "Ecdev.Meta_Keywords";
		public static void AddMetaDescription(string value, HttpContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.Items[metaDescriptionKey] = value;
		}
		public static void AddMetaKeywords(string value, HttpContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.Items[metaKeywordsKey] = value;
		}
		protected override void Render(HtmlTextWriter writer)
		{
			this.RenderMetaDescription(writer);
			this.RenderMetaKeywords(writer);
		}
		protected virtual void RenderMetaDescription(HtmlTextWriter writer)
		{
			if (this.Context.Items.Contains(metaDescriptionKey))
			{
				writer.WriteLine("<meta name=\"{0}\" content=\"{1}\" />", "description", this.Context.Items[metaDescriptionKey]);
				return;
			}
			writer.WriteLine("<meta name=\"{0}\" content=\"{1}\" />", "description", HiContext.Current.SiteSettings.SearchMetaDescription);
		}
		protected virtual void RenderMetaKeywords(HtmlTextWriter writer)
		{
			if (this.Context.Items.Contains(metaKeywordsKey))
			{
				writer.WriteLine("<meta name=\"{0}\" content=\"{1}\" />", "keywords", this.Context.Items[metaKeywordsKey]);
				return;
			}
			writer.WriteLine("<meta name=\"{0}\" content=\"{1}\" />", "keywords", HiContext.Current.SiteSettings.SearchMetaKeywords);
		}
	}
}
