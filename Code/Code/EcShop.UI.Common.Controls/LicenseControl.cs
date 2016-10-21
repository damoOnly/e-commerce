using EcShop.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using EcShop.Core;
namespace EcShop.UI.Common.Controls
{
	public class LicenseControl : WebControl
	{
        private readonly string renderFormat = "<a href=\"http://www.ecdev.cn\">惠众云商技术支持</a>";
		protected override void Render(HtmlTextWriter writer)
		{
            if (!CopyrightLicenser.CheckCopyright())
            {
                writer.Write(string.Format(this.renderFormat, HiContext.Current.SiteSettings.SiteUrl));
            }
		}
	}
}
