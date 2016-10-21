using EcShop.SaleSystem.Shopping;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
    public class Common_VoucherSelect : WebControl
	{
		public decimal CartTotal
		{
			get;
			set;
		}
		protected override void Render(HtmlTextWriter writer)
		{
			DataTable coupon = ShoppingProcessor.GetVoucher(this.CartTotal);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择一张现金券<span class=\"caret\"></span></button>");
			stringBuilder.AppendLine("<ul class=\"dropdown-menu\" role=\"menu\">");
			foreach (DataRow dataRow in coupon.Rows)
			{
				stringBuilder.AppendFormat("<li><a href=\"#\" name=\"{0}\" value=\"{1}\">{2}(到期时间{3})</a></li>", new object[]
				{
					
					dataRow["ClaimCode"],
					((decimal)dataRow["discountValue"]).ToString("F2"),
					dataRow["Name"],
                    dataRow["deadline"]
				}).AppendLine();
			}
			stringBuilder.AppendLine("</ul>");
			writer.Write(stringBuilder.ToString());
		}
	}
}
