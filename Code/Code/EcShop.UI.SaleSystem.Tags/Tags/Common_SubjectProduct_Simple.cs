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
	public class Common_SubjectProduct_Simple : WebControl
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
			XmlNode xmlNode = TagsHelper.FindProductNode(this.SubjectId, "simple");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"pro_simple{0} cssEdite\" type=\"simple\" id=\"products_{1}\" >", xmlNode.Attributes["ImageSize"].Value, this.SubjectId);
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
				stringBuilder.Append("</div>");
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
			if (!string.IsNullOrEmpty(node.Attributes["TypeId"].Value))
			{
				subjectListQuery.ProductTypeId = new int?(int.Parse(node.Attributes["TypeId"].Value));
			}
			string value = node.Attributes["AttributeString"].Value;
			if (!string.IsNullOrEmpty(value))
			{
				IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
				string[] array = value.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(new char[]
					{
						'_'
					});
					list.Add(new AttributeValueInfo
					{
						AttributeId = Convert.ToInt32(array2[0]),
						ValueId = Convert.ToInt32(array2[1])
					});
				}
				subjectListQuery.AttributeValues = list;
			}
			subjectListQuery.MaxNum = int.Parse(node.Attributes["MaxNum"].Value);
			return ProductBrowser.GetSubjectList(subjectListQuery);
		}
	}
}
