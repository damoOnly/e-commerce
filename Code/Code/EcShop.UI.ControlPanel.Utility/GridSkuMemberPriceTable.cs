using EcShop.ControlPanel.Commodities;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class GridSkuMemberPriceTable : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			string text = this.Page.Request.QueryString["productIds"];
			if (!string.IsNullOrEmpty(text))
			{
				DataTable skuMemberPrices = ProductHelper.GetSkuMemberPrices(text);
				if (skuMemberPrices != null && skuMemberPrices.Rows.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("<table cellspacing=\"0\" border=\"0\" style=\"width:100%;border-collapse:collapse;\">");
					stringBuilder.AppendLine("<tr class=\"table_title\">");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\">货号</th>");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\">商品</th>");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\">市场价</th>");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\">成本价</th>");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\">一口价</th>");
					for (int i = 7; i < skuMemberPrices.Columns.Count; i++)
					{
						string text2 = skuMemberPrices.Columns[i].ColumnName;
						text2 = text2.Substring(text2.IndexOf("_") + 1) + "价";
						stringBuilder.AppendFormat("<th class=\"td_right td_left\" scope=\"col\" style=\"width:100px;\">{0}</th>", text2).AppendLine();
					}
					stringBuilder.AppendLine("</tr>");
					foreach (DataRow row in skuMemberPrices.Rows)
					{
						this.CreateRow(row, skuMemberPrices, stringBuilder);
					}
					stringBuilder.AppendLine("</table>");
					writer.Write(stringBuilder.ToString());
				}
			}
		}
		private void CreateRow(DataRow row, DataTable dtSkus, StringBuilder sb)
		{
			string text = row["SkuId"].ToString();
			sb.AppendFormat("<tr class=\"SkuPriceRow\" skuId=\"{0}\" >", text).AppendLine();
			sb.AppendFormat("<td>&nbsp;{0}</td>", row["SKU"]).AppendLine();
			sb.AppendFormat("<td>{0} {1}</td>", row["ProductName"], row["SKUContent"]).AppendLine();
			sb.AppendFormat("<td>&nbsp;{0}</td>", (row["MarketPrice"] != DBNull.Value) ? decimal.Parse(row["MarketPrice"].ToString()).ToString("F2") : "").AppendLine();
			sb.AppendFormat("<td><input type=\"text\" id=\"tdCostPrice_{0}\" style=\"width:50px\" value=\"{1}\" />", text, (row["CostPrice"] != DBNull.Value) ? decimal.Parse(row["CostPrice"].ToString()).ToString("F2") : "").AppendLine();
			sb.AppendFormat("<td><input type=\"text\" id=\"tdSalePrice_{0}\" style=\"width:50px\" value=\"{1}\" />", text, decimal.Parse(row["SalePrice"].ToString()).ToString("F2")).AppendLine();
			for (int i = 7; i < dtSkus.Columns.Count; i++)
			{
				string columnName = dtSkus.Columns[i].ColumnName;
				string[] array = row[columnName].ToString().Split(new char[]
				{
					'|'
				});
				string text2 = "";
				if (array[0].ToString() != "")
				{
					text2 = decimal.Parse(array[0].ToString()).ToString("F2");
				}
				string text3 = array[1].ToString();
				sb.AppendFormat("<td><input type=\"text\" id=\"{0}_tdMemberPrice_{1}\" name=\"tdMemberPrice_{1}\" style=\"width:50px\" value=\"{2}\" />{3}</td>", new object[]
				{
					columnName.Substring(0, columnName.IndexOf("_")),
					text,
					text2,
					text3
				}).AppendLine();
			}
			sb.AppendLine("</tr>");
		}
	}
}
