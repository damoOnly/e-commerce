using EcShop.Entities.Sales;
using EcShop.SaleSystem.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_ShippingTypeSelect : WebControl
	{
		public int RegionId
		{
			get;
			set;
		}
		public decimal Weight
		{
			get;
			set;
		}
		public IList<ShoppingCartItemInfo> ShoppingCartItemInfo
		{
			get;
			set;
		}
		protected override void Render(HtmlTextWriter writer)
		{
			IList<ShippingModeInfo> shippingModes = ShoppingProcessor.GetShippingModes();
			if (shippingModes != null && shippingModes.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(" <button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择配送方式<span class=\"caret\"></span></button>");
				stringBuilder.AppendLine("<ul id=\"shippingTypeUl\" class=\"dropdown-menu\" role=\"menu\">");
				foreach (ShippingModeInfo current in shippingModes)
				{
					decimal num = 0m;
					if (this.ShoppingCartItemInfo.Count != this.ShoppingCartItemInfo.Count((ShoppingCartItemInfo a) => a.IsfreeShipping))
					{
						num = ShoppingProcessor.CalcFreight(this.RegionId, this.Weight, current);
					}
					stringBuilder.AppendFormat("<li><a href=\"#\" name=\"{0}\" value=\"{2}\">{1}</a></li>", current.ModeId, current.Name + "： ￥" + num.ToString("F2"), num.ToString("F2")).AppendLine();
				}
				stringBuilder.AppendLine("</ul>");
				writer.Write(stringBuilder.ToString());
			}
		}
	}
}
