using EcShop.Membership.Context;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
namespace EcShop.UI.Common.Controls
{
	[ParseChildren(false), PersistChildren(true)]
	public class PageTitle : Control
	{
		private const string titleKey = "Ecdev.Title.Value";
		public static void AddTitle(string title, HttpContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.Items[titleKey] = title;
		}
		public static void AddTitle(string title)
		{
			if (HttpContext.Current == null)
			{
				throw new ArgumentNullException("context");
			}
			HttpContext.Current.Items[titleKey] = title;
		}
		public static void AddSiteNameTitle(string title)
		{
			PageTitle.AddTitle(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", new object[]
			{
				title,
				HiContext.Current.SiteSettings.SiteName
			}), HiContext.Current.Context);
		}
		protected override void Render(HtmlTextWriter writer)
		{
			string text = this.Context.Items[titleKey] as string;
			if (string.IsNullOrEmpty(text))
			{
				text = HiContext.Current.SiteSettings.SiteName;
			}
			writer.WriteLine("<title>{0}</title>", text);
		}
	}
}
