using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Membership.Context;
using System;
using System.Web.UI;
namespace EcShop.UI.Common.Controls
{
	[ParseChildren(false), PersistChildren(true)]
	public class HeadContainer : Control
	{
		protected override void Render(HtmlTextWriter writer)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			HiContext current = HiContext.Current;
			writer.Write("<script language=\"javascript\" type=\"text/javascript\"> \r\n            var applicationPath = \"{0}\";\r\n            var skinPath = \"{1}\";\r\n            var subsiteuserId = \"{2}\";\r\n            var HasWapRight = {3};\r\n        </script>", new object[]
			{
				Globals.ApplicationPath,
				current.GetSkinPath(),
				current.SiteSettings.UserId.HasValue ? current.SiteSettings.UserId.Value.ToString() : "0",
				(masterSettings.OpenWap == 1 || Globals.IsTestDomain) ? "true" : "false"
			});
			writer.WriteLine();
			this.RenderMetaCharset(writer);
			this.RenderMetaLanguage(writer);
			this.RenderFavicon(writer);
			this.RenderMetaAuthor(writer);
			this.RenderMetaGenerator(writer);
		}
		private void RenderMetaGenerator(HtmlTextWriter writer)
		{
			writer.WriteLine("<meta name=\"GENERATOR\" content=\"" + HiContext.Current.Config.Version + "\" />");
		}
		private void RenderFavicon(HtmlTextWriter writer)
		{
			string arg = Globals.FullPath(Globals.GetSiteUrls().Favicon);
			writer.WriteLine("<link rel=\"icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", arg);
			writer.WriteLine("<link rel=\"shortcut icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", arg);
		}
		private void RenderMetaAuthor(HtmlTextWriter writer)
		{
            writer.WriteLine("<meta name=\"author\" content=\"Ecdev development team\" />");
		}
		private void RenderMetaCharset(HtmlTextWriter writer)
		{
			ApplicationType currentApplicationType = HiContext.Current.Config.AppLocation.CurrentApplicationType;
			if (currentApplicationType == ApplicationType.Admin || currentApplicationType == ApplicationType.Installer)
			{
				writer.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\" />");
			}
		}
		private void RenderMetaLanguage(HtmlTextWriter writer)
		{
			writer.WriteLine("<meta http-equiv=\"content-language\" content=\"zh-CN\" />");
		}
	}
}
