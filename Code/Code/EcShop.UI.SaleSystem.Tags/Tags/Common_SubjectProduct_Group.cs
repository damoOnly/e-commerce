using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Comments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_SubjectProduct_Group : WebControl
	{
		private int categoryId;
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
			XmlNode xmlNode = TagsHelper.FindProductNode(this.SubjectId, "group");
			if (xmlNode != null)
			{
				int.TryParse(xmlNode.Attributes["CategoryId"].Value, out this.categoryId);
				stringBuilder.AppendFormat("<div class=\"group{0} cssEdite\" type=\"group\" id=\"products_{1}\" >", xmlNode.Attributes["ImageSize"].Value, this.SubjectId).AppendLine();
				this.RenderHeader(xmlNode, stringBuilder);
				stringBuilder.AppendLine("<div class=\"group_bd\">");
				this.RenderLift(xmlNode, stringBuilder);
				this.RenderMiddle(xmlNode, stringBuilder);
				this.RenderRight(xmlNode, stringBuilder);
				stringBuilder.AppendLine("</div>");
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
		private void RenderHeader(XmlNode node, StringBuilder sb)
		{
			sb.AppendLine("<div class=\"group_hd\">");
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
			CategoryInfo category = CategoryBrowser.GetCategory(this.categoryId);
			while (category != null && category.Depth != 1)
			{
				this.categoryId = category.ParentCategoryId.Value;
				category = CategoryBrowser.GetCategory(this.categoryId);
			}
			if (category != null)
			{
				this.categoryId = category.CategoryId;
			}
			DataTable hotKeywords = CommentBrowser.GetHotKeywords(this.categoryId, int.Parse(node.Attributes["HotKeywordNum"].Value));
			if (hotKeywords != null && hotKeywords.Rows.Count > 0)
			{
				sb.AppendLine("<ul>");
				foreach (DataRow dataRow in hotKeywords.Rows)
				{
					sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().SubCategory((int)dataRow["CategoryId"], null) + "?keywords=" + Globals.UrlEncode((string)dataRow["Keywords"]), dataRow["Keywords"]).AppendLine("");
				}
				sb.AppendLine("</ul>");
			}
			if (node.Attributes["IsShowMoreLink"].Value == "true")
			{
				sb.AppendFormat("<em><a href=\"{0}\">更多>></a></em>", Globals.GetSiteUrls().SubCategory(this.categoryId, null)).AppendLine();
			}
			sb.AppendLine("</div>");
		}
		private void RenderLift(XmlNode node, StringBuilder sb)
		{
			sb.AppendLine("<div class=\"bd_left\">");
			IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, int.Parse(node.Attributes["SubCategoryNum"].Value));
			if (maxSubCategories != null && maxSubCategories.Count > 0)
			{
				sb.AppendLine("<ul>");
				foreach (CategoryInfo current in maxSubCategories)
				{
					sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().SubCategory(current.CategoryId, current.RewriteName), current.Name).AppendLine("");
				}
				sb.AppendLine("</ul>");
			}
			if (!string.IsNullOrEmpty(node.Attributes["AdImageLeft"].Value))
			{
				sb.AppendFormat("<div class=\"bd_left_ad\"><img src=\"{0}\"  /></div>", node.Attributes["AdImageLeft"].Value);
			}
			sb.AppendLine("</div>");
		}
		private void RenderMiddle(XmlNode node, StringBuilder sb)
		{
			sb.AppendLine("<div class=\"bd_middle\">");
			if (!string.IsNullOrEmpty(node.Attributes["AdImageRight"].Value))
			{
				sb.AppendFormat("<div class=\"bd_right_ad\"><img src=\"{0}\"  /></div>", node.Attributes["AdImageRight"].Value);
			}
			DataTable productList = this.GetProductList(node);
			if (productList != null && productList.Rows.Count > 0)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				string text = this.ShowDefaultProductImage(node.Attributes["ImageSize"].Value, masterSettings);
				sb.AppendLine("<ul>");
				foreach (DataRow dataRow in productList.Rows)
				{
					string str = text;
					if (dataRow["ThumbnailUrl" + node.Attributes["ImageSize"].Value] != DBNull.Value)
					{
						str = dataRow["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToString();
					}
					sb.AppendLine("<li>");
					sb.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
					{
						dataRow["ProductId"]
					}), Globals.ApplicationPath + str).AppendLine();
					sb.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
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
					sb.AppendFormat("<div class=\"price\"><b><em>￥</em>{0}</b><span><em>￥</em>{1}</span></div>", Globals.FormatMoney(money), arg).AppendLine();
					sb.AppendFormat("<a class=\"productview\" target=\"_blank\" href=\"{0}\">查看详情</a>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
					{
						dataRow["ProductId"]
					}), dataRow["ProductName"]).AppendLine();
					sb.AppendLine("</li>");
				}
				sb.AppendLine("</ul>");
			}
			sb.AppendLine("</div>");
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
		private void RenderRight(XmlNode node, StringBuilder sb)
		{
			sb.AppendLine("<div class=\"bd_right\">");
			this.RenderBrand(node, sb);
			this.RenderSaleTop(node, sb);
			sb.AppendLine("</div>");
		}
		private void RenderBrand(XmlNode node, StringBuilder sb)
		{
			DataTable brandCategories = CategoryBrowser.GetBrandCategories(this.categoryId, int.Parse(node.Attributes["BrandNum"].Value));
			if (brandCategories != null && brandCategories.Rows.Count > 0)
			{
				sb.AppendLine("<div class=\"bd_brand\">");
				sb.AppendLine("<ul>");
				foreach (DataRow dataRow in brandCategories.Rows)
				{
					sb.AppendFormat("<li><a href=\"{0}\"><img src=\"{1}\" /></a></li>", Globals.GetSiteUrls().SubBrandDetails((int)dataRow["BrandId"], dataRow["RewriteName"]), dataRow["Logo"]).AppendLine();
				}
				sb.AppendLine("</ul>");
				sb.AppendLine("</div>");
			}
		}
		private void RenderSaleTop(XmlNode node, StringBuilder sb)
		{
			DataTable saleProductRanking = ProductBrowser.GetSaleProductRanking(new int?(this.categoryId), int.Parse(node.Attributes["SaleTopNum"].Value));
			if (saleProductRanking != null && saleProductRanking.Rows.Count > 0)
			{
				int num = 0;
				int.TryParse(node.Attributes["ImageNum"].Value, out num);
				bool flag = false;
				bool.TryParse(node.Attributes["IsShowPrice"].Value, out flag);
				bool flag2 = false;
				bool.TryParse(node.Attributes["IsShowSaleCounts"].Value, out flag2);
				bool flag3 = false;
				bool.TryParse(node.Attributes["IsImgShowPrice"].Value, out flag3);
				bool flag4 = false;
				bool.TryParse(node.Attributes["IsImgShowSaleCounts"].Value, out flag4);
				sb.AppendLine("<div class=\"bd_saletop\">");
				sb.AppendLine("<ul>");
				int num2 = 0;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				string text = this.ShowDefaultProductImage(node.Attributes["TopImageSize"].Value, masterSettings);
				foreach (DataRow dataRow in saleProductRanking.Rows)
				{
					string str = text;
					if (dataRow["ThumbnailUrl" + node.Attributes["TopImageSize"].Value] != DBNull.Value)
					{
						str = dataRow["ThumbnailUrl" + node.Attributes["TopImageSize"].Value].ToString();
					}
					num2++;
					sb.AppendFormat("<li class=\"sale_top{0}\">", num2).AppendLine();
					if (num2 <= num)
					{
						sb.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
						{
							dataRow["ProductId"]
						}), Globals.ApplicationPath + str).AppendLine();
					}
					sb.AppendLine("<div class=\"info\">");
					sb.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
					{
						dataRow["ProductId"]
					}), dataRow["ProductName"]).AppendLine();
					if ((flag && num2 > num) || (flag3 && num2 <= num))
					{
						string arg = string.Empty;
						if (dataRow["MarketPrice"] != DBNull.Value)
						{
							arg = Globals.FormatMoney((decimal)dataRow["MarketPrice"]);
						}
						sb.AppendFormat("<div class=\"price\"><b>{0}</b><span>{1}</span></div>", Globals.FormatMoney((decimal)dataRow["SalePrice"]), arg).AppendLine();
					}
					if ((flag2 && num2 > num) || (flag4 && num2 <= num))
					{
						sb.AppendFormat("<div class=\"sale\">已售出<b>{0}</b>件 </div>", dataRow["SaleCounts"]).AppendLine();
					}
					sb.Append("</div>");
					sb.AppendLine("</li>");
				}
				sb.AppendLine("</ul>");
				sb.AppendLine("</div>");
			}
		}
		private DataTable GetProductList(XmlNode node)
		{
			return ProductBrowser.GetSubjectList(new SubjectListQuery
			{
				SortBy = "DisplaySequence",
				SortOrder = SortAction.Desc,
				CategoryIds = node.Attributes["CategoryId"].Value,
				MaxNum = int.Parse(node.Attributes["MaxNum"].Value)
			});
		}
	}
}
