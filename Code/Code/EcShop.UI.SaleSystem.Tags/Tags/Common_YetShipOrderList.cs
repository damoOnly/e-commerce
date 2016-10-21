using EcShop.Membership.Context;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_YetShipOrderList : ThemedTemplatedRepeater
	{
		protected override void OnLoad(EventArgs e)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (siteSettings.OrderShowDays > 0)
			{
				base.DataSource = ShoppingProcessor.GetYetShipOrders(siteSettings.OrderShowDays);
				base.DataBind();
			}
		}
	}
}
