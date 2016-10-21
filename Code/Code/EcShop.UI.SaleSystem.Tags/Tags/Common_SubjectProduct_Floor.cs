using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_SubjectProduct_Floor : WebControl
	{
		public int SubjectId
		{
			get;
			set;
		}
		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write(this.RendHtml());
		}
		public string RendHtml()
		{
			StringBuilder stringBuilder = new StringBuilder();
			XmlNode xmlNode = TagsHelper.FindProductNode(this.SubjectId, "floor");
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"floor{0} cssEdite\" type=\"floor\" id=\"products_{1}\" >", xmlNode.Attributes["ImageSize"].Value, this.SubjectId).AppendLine();
				this.RenderHeader(xmlNode, stringBuilder);
				stringBuilder.AppendLine("<div class=\"floor_bd\">");
				if (!string.IsNullOrEmpty(xmlNode.Attributes["AdImage"].Value))
				{
					stringBuilder.AppendFormat("<div class=\"floor_ad\"><img src=\"{0}\"  /></div>", xmlNode.Attributes["AdImage"].Value).AppendLine();
				}
				else
				{
					stringBuilder.AppendFormat("<div class=\"floor_ad\"><img src=\"{0}\"  /></div>", SettingsManager.GetMasterSettings(true).DefaultProductImage).AppendLine();
				}
				stringBuilder.AppendLine("<div class=\"floor_pro\">");
				DataTable productList = this.GetProductList(xmlNode);
				if (productList != null && productList.Rows.Count > 0)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
					string text = this.ShowDefaultProductImage(xmlNode.Attributes["ImageSize"].Value, masterSettings);
					stringBuilder.AppendLine("<ul>");
					foreach (DataRow dataRow in productList.Rows)
					{
						string str = text;
						if (dataRow["ThumbnailUrl" + xmlNode.Attributes["ImageSize"].Value] != DBNull.Value)
						{
							str = dataRow["ThumbnailUrl" + xmlNode.Attributes["ImageSize"].Value].ToString();
						}
						stringBuilder.AppendLine("<li>");
						stringBuilder.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
						{
							dataRow["ProductId"]
						}), Globals.ApplicationPath + str).AppendLine();
						stringBuilder.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
						{
							dataRow["ProductId"]
						}), dataRow["ProductName"]).AppendLine();
						decimal money = 0m;
						string arg = string.Empty;
						if (dataRow["MarketPrice"] != DBNull.Value)
						{
							arg = Globals.FormatMoney((decimal)dataRow["MarketPrice"]);
						}
						if (dataRow["RankPrice"] != DBNull.Value)
						{
							money = (decimal)dataRow["RankPrice"];
						}
						stringBuilder.AppendFormat("<div class=\"price\"><b><em>￥</em>{0}</b><span><em>￥</em>{1}</span></div>", Globals.FormatMoney(money), arg).AppendLine();
						stringBuilder.AppendFormat("<a class=\"productview\" target=\"_blank\" href=\"{0}\">查看详情</a>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
						{
							dataRow["ProductId"]
						}), dataRow["ProductName"]).AppendLine();
						stringBuilder.AppendLine("</li>");
					}
					stringBuilder.AppendLine("</ul>");
				}
				stringBuilder.AppendLine("</div>");
				stringBuilder.AppendLine("</div>");
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
		private string ShowDefaultProductImage(string thumbnailsize, SiteSettings settings)
		{
			string result = settings.DefaultProductImage;
			switch (thumbnailsize)
			{
			case "40":
				result = settings.DefaultProductThumbnail1;
				break;
			case "60":
				result = settings.DefaultProductThumbnail2;
				break;
			case "100":
				result = settings.DefaultProductThumbnail3;
				break;
			case "160":
				result = settings.DefaultProductThumbnail4;
				break;
			case "180":
				result = settings.DefaultProductThumbnail5;
				break;
			case "220":
				result = settings.DefaultProductThumbnail6;
				break;
			case "310":
				result = settings.DefaultProductThumbnail7;
				break;
			case "410":
				result = settings.DefaultProductThumbnail8;
				break;
			}
			return result;
		}
		private void RenderHeader(XmlNode node, StringBuilder sb)
		{
			sb.AppendLine("<div class=\"floor_hd\">");
			sb.AppendLine("<div>");
			if (!string.IsNullOrEmpty(node.Attributes["ImageTitle"].Value))
			{
				sb.AppendFormat("<span class=\"icon\"><img src=\"{0}\" /></span>", Globals.ApplicationPath + node.Attributes["ImageTitle"].Value);
			}
			if (!string.IsNullOrEmpty(node.Attributes["Title"].Value))
			{
				sb.AppendFormat("<span class=\"title\">{0}</span>", node.Attributes["Title"].Value);
			}
			sb.AppendLine("</div>");
			int num = 0;
			if (int.TryParse(node.Attributes["CategoryId"].Value, out num))
			{
				IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(num, int.Parse(node.Attributes["SubCategoryNum"].Value));
				if (maxSubCategories != null && maxSubCategories.Count > 0)
				{
					sb.AppendLine("<ul>");
					foreach (CategoryInfo current in maxSubCategories)
					{
						sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().SubCategory(current.CategoryId, current.RewriteName), current.Name).AppendLine("");
					}
					sb.AppendLine("</ul>");
				}
				if (node.Attributes["IsShowMoreLink"].Value == "true")
				{
					sb.AppendFormat("<em><a href=\"{0}\">更多>></a></em>", Globals.GetSiteUrls().SubCategory(num, null)).AppendLine();
				}
			}
			sb.AppendLine("</div>");
		}
		private DataTable GetProductList(XmlNode node)
		{
			SubjectListQuery subjectListQuery = new SubjectListQuery();
			subjectListQuery.SortBy = "DisplaySequence";
			subjectListQuery.SortOrder = SortAction.Desc;
			subjectListQuery.CategoryIds = node.Attributes["CategoryId"].Value;
			if (!string.IsNullOrEmpty(node.Attributes["TagId"].Value))
			{
				subjectListQuery.TagId = int.Parse(node.Attributes["TagId"].Value);
			}
			if (!string.IsNullOrEmpty(node.Attributes["BrandId"].Value))
			{
				subjectListQuery.BrandCategoryId = new int?(int.Parse(node.Attributes["BrandId"].Value));
			}
			subjectListQuery.MaxNum = int.Parse(node.Attributes["MaxNum"].Value);
			return ProductBrowser.GetSubjectList(subjectListQuery);
		}
	}
}
