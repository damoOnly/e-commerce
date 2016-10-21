using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;

namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_Token : System.Web.UI.UserControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
            writer.Write(System.Web.Helpers.AntiForgery.GetHtml());

			base.Render(writer);
		}
	}
}
