using EcShop.Core;
using EcShop.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
	public class WapAuth_Script : Literal
	{
		private const string RightFormat = "<script type=\"text/javascript\">{0}</script>";
		protected override void Render(HtmlTextWriter writer)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			if (masterSettings.OpenWap == 1 || Globals.IsTestDomain)
			{
				writer.Write("<script type=\"text/javascript\">{0}</script>", "var HasWapRight = true;");
				return;
			}
			writer.Write("<script type=\"text/javascript\">{0}</script>", "var HasWapRight = false;");
		}
	}
}
