using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_SubjectProduct_Tab : WebControl
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
			XmlNode xmlNode = TagsHelper.FindProductNode(this.SubjectId, "tab");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"pro_tab{0} cssEdite\" type=\"tab\" id=\"products_{1}\" >", xmlNode.Attributes["ImageSize"].Value, this.SubjectId).AppendLine();
				this.RenderHeader(xmlNode, stringBuilder);
				stringBuilder.AppendFormat("<div class=\"tab_item\" id=\"products_{0}_item_1\">", this.SubjectId);
				this.RenderProdcutItem(xmlNode, stringBuilder, "Where1");
				stringBuilder.AppendLine("</div>");
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle2"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_2\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where2");
					stringBuilder.AppendLine("</div>");
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle3"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_3\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where3");
					stringBuilder.AppendLine("</div>");
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle4"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_4\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where4");
					stringBuilder.AppendLine("</div>");
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle5"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_5\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where5");
					stringBuilder.AppendLine("</div>");
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle6"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_6\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where6");
					stringBuilder.AppendLine("</div>");
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle7"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_7\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where7");
					stringBuilder.AppendLine("</div>");
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle8"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_8\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where8");
					stringBuilder.AppendLine("</div>");
				}
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
		private void RenderHeader(XmlNode node, StringBuilder sb)
		{
			sb.AppendLine("<div class=\"tab_hd\">");
			sb.AppendLine("<ul>");
			sb.AppendFormat("<li class=\"select\" onmouseover=\"changeTab(this, 'products_{0}', '_item_1')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle1"].Value).AppendLine();
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle2"].Value))
			{
				sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_2')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle2"].Value).AppendLine();
			}
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle3"].Value))
			{
				sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_3')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle3"].Value).AppendLine();
			}
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle4"].Value))
			{
				sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_4')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle4"].Value).AppendLine();
			}
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle5"].Value))
			{
				sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_5')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle5"].Value).AppendLine();
			}
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle6"].Value))
			{
				sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_6')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle6"].Value).AppendLine();
			}
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle7"].Value))
			{
				sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_7')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle7"].Value).AppendLine();
			}
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle8"].Value))
			{
				sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_8')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle8"].Value).AppendLine();
			}
			sb.AppendLine("</ul>");
			sb.AppendLine("</div>");
		}
		private void RenderProdcutItem(XmlNode node, StringBuilder sb, string whereName)
		{
			DataTable tabProductList = this.GetTabProductList(node, whereName);
			if (tabProductList != null && tabProductList.Rows.Count > 0)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				string text = this.ShowDefaultProductImage(node.Attributes["ImageSize"].Value, masterSettings);
				sb.AppendLine("<ul>");
				foreach (DataRow dataRow in tabProductList.Rows)
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
		private DataTable GetTabProductList(XmlNode node, string whereName)
		{
			SubjectListQuery subjectListQuery = new SubjectListQuery();
			subjectListQuery.SortBy = "DisplaySequence";
			subjectListQuery.SortOrder = SortAction.Desc;
			string value = node.Attributes[whereName].Value;
			if (!string.IsNullOrEmpty(value))
			{
				string[] array = value.Split(new char[]
				{
					','
				});
				subjectListQuery.CategoryIds = array[0];
				if (!string.IsNullOrEmpty(array[1]))
				{
					subjectListQuery.TagId = int.Parse(array[1]);
				}
				if (!string.IsNullOrEmpty(array[2]))
				{
					subjectListQuery.BrandCategoryId = new int?(int.Parse(array[2]));
				}
			}
			subjectListQuery.MaxNum = int.Parse(node.Attributes["MaxNum"].Value);
			return ProductBrowser.GetSubjectList(subjectListQuery);
		}
	}
}
